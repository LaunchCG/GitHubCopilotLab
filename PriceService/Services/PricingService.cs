namespace PriceService.Services;

using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using PriceService.ApiModel;
using PriceService.Database;
using PriceService.DBModel;

public class PricingService : IPriceService
{
    const string IEXAPI_URL = "https://api.iex.cloud/v1";

    static HttpClient _httpClient = new HttpClient();
    private readonly PriceContext _context;
    private readonly ILogger<PricingService> _logger;
    private readonly IServiceProvider _serviceProvider;


    public PricingService(PriceContext priceContext, ILogger<PricingService> logger, IServiceProvider serviceProvider)
    {
        _context = priceContext;
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    public CompanyInfo GetCompanyInfo(string symbol)
    {
        symbol = symbol.ToUpper();
        var dbCompanyInfo = _context.CompanyInfo.Find(symbol);
        if (dbCompanyInfo != null)
            return new CompanyInfo {
                Description = dbCompanyInfo.Description,
                Name = dbCompanyInfo.Name
            };

        var companyInfo = GetInfoFromIEX(symbol);
        if (companyInfo != null)
        {
            _context.Add(new DBCompanyInfo{
                Symbol = symbol,
                Name = companyInfo.Name,
                Description = companyInfo.Description,
                CreateDate = DateTime.UtcNow});
            _context.SaveChanges();
        }
        return companyInfo;
    }

    private CompanyInfo GetInfoFromIEX(string symbol)
    {
        var getUrl = $"{IEXAPI_URL}/data/CORE/company/{symbol}?token={GlobalEnv.IEX_KEY}";
        var jsonResponse = _httpClient.GetStringAsync(getUrl).Result;

        using var doc = JsonDocument.Parse(jsonResponse);
        var docRoot = doc.RootElement;
        var docInfo = docRoot.EnumerateArray().First();

        var name = docInfo.GetProperty("companyName").GetString();
        var description = docInfo.GetProperty("shortDescription").GetString();

        return new CompanyInfo {
            Name = name,
            Description = description
        };            
    }

    public Price GetLastClosingPrice(string symbol)
    {
        // For now, skip pricing cache
        return GetPriceFromIEX(symbol);
    }

    private Price GetPriceFromIEX(string symbol)
    {
        var getUrl = $"{IEXAPI_URL}/data/core/quote/{symbol}?token={GlobalEnv.IEX_KEY}";
        var jsonResponse = _httpClient.GetStringAsync(getUrl).Result;

        using var doc = JsonDocument.Parse(jsonResponse);
        var docRoot = doc.RootElement;
        var docQuote = docRoot.EnumerateArray().First();

        Price retPrice = new Price();

        // If market open, use previous close, otherwise use current close
        var usMarketOpen = docQuote.GetProperty("isUSMarketOpen").GetBoolean();
        if (usMarketOpen) {
            double previousClose = docQuote.GetProperty("previousClose").GetDouble();
            retPrice.ClosePrice = previousClose;
            retPrice.CloseDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-1));
        }
        else {
            double curClose = docQuote.GetProperty("iexClose").GetDouble();
            retPrice.ClosePrice = curClose;
            retPrice.CloseDate = DateOnly.FromDateTime(DateTime.Now);
        }

        return retPrice;
    }

    public int UpdateSymbols()
    {
        // Get all symbols which don't have a name but are marked as valid
        var symbolList = _context.CompanyInfo
                .Where(comp => String.IsNullOrEmpty(comp.Name) && comp.IsValid)
                .Select(comp => comp.Symbol).ToList();
        
        // Create new task in database
        var dbBatchRun = new DBBatchRun{ StartTime = DateTime.UtcNow, Duration = 0, RunType = "UpdateSymbols" };
        _context.BatchRun.Add(dbBatchRun);
        _context.SaveChanges();

        // Create start message
        _context.BatchDetail.Add(new DBBatchDetail { 
            RunId = dbBatchRun.RunId,
            Message = $"Starting Batch Run - {symbolList.Count} symbols to resolve"
        });
        _context.SaveChanges();

        // Run operation on background thread and return batch job id to caller
        Task.Run(() => UpdatePricesForSymbols(dbBatchRun.RunId, symbolList));
        return dbBatchRun.RunId;
    }

    private int UpdatePricesForSymbols(int batchId, List<string> symbolList)
    {
        using var scope = _serviceProvider.CreateScope();

        var updateContext = scope.ServiceProvider.GetRequiredService<PriceContext>();

        // Perform batch operation, space calls to avoid throttling logic
        const int BATCH_SIZE = 8;
        const int WAIT_SECS = 10;
        int index = 0;
        const int MAX_ERROR_COUNT = 1000;
        int curErrorCount = 0;

        foreach (var symbol in symbolList)
        {
            try {
                // Get information from API
                var info = GetInfoFromIEX(symbol);
                var entity = updateContext.CompanyInfo.Find(symbol);
                entity.Description = info.Description ?? "";
                entity.Name = info.Name ?? "";
                entity.IsValid = (!string.IsNullOrEmpty(info.Name));
            }
            catch (Exception ex) {
                // On exception, see if we should terminate.  Otherwise, log error
                if (curErrorCount++ >= MAX_ERROR_COUNT)
                {
                    updateContext.BatchDetail.Add(new DBBatchDetail {RunId = batchId, Message = "Error: Exceeded maximum errors, terminating run"});
                    throw;
                }
                updateContext.BatchDetail.Add(new DBBatchDetail {RunId = batchId, Message = $"Error: {symbol}: {ex.ToString()}"});
                // Sleep just in case we hit rate limit error
                Thread.Sleep(WAIT_SECS * 1000);                    
            }

            if (++index % BATCH_SIZE == 0)
            {
                // Commit changes as we go...
                updateContext.BatchDetail.Add(new DBBatchDetail {RunId = batchId, Message = $"Status: Updated {index} total records, errorCount = {curErrorCount}"});
                updateContext.SaveChanges();

                // Sleep before we send next batch
                Thread.Sleep(WAIT_SECS * 1000);
            }
        }

        // Update overall run status and send final detail message
        updateContext.SaveChanges();
        updateContext.BatchDetail.Add(new DBBatchDetail {RunId = batchId, Message = $"Status: Batch completed successfully, {index} symbols processed, {curErrorCount} errors."});
        var batchRun = updateContext.BatchRun.Find(batchId);
        batchRun.Duration = (int) (DateTime.UtcNow - batchRun.StartTime).TotalSeconds;
        batchRun.ErrorCount = curErrorCount;
        batchRun.SuccessCount = index - curErrorCount;
        updateContext.SaveChanges();

        return index;
    }

    public BatchRun GetSymbolUpdateStatus(int runId)
    {
        using var scope = _serviceProvider.CreateScope();

        var updateContext = scope.ServiceProvider.GetRequiredService<PriceContext>();
        var dbRun = updateContext.BatchRun.Find(runId);
        var msgList = updateContext.BatchDetail.Where(bd => bd.RunId == runId).Select(bd => bd.Message).ToList();

        return new BatchRun {
            RunId = dbRun.RunId,
            StartTime = dbRun.StartTime,
            RunType = dbRun.RunType,
            SuccessCount = dbRun.SuccessCount,
            ErrorCount = dbRun.ErrorCount,
            Duration = dbRun.Duration,
            Messages = msgList
        };
    }
}


