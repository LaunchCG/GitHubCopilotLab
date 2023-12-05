using PriceService.ApiModel;

namespace PriceService.Services
{
    public interface IPriceService {
            CompanyInfo GetCompanyInfo(string symbol);
            Price GetLastClosingPrice(string symbol);
            // Updates information for all symbols cached in DB
            int UpdateSymbols();
            // Gets status of batch operation
            BatchRun GetSymbolUpdateStatus(int runId);
    }
}
