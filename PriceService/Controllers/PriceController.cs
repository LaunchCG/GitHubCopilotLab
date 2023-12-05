using Microsoft.AspNetCore.Mvc;
using PriceService.ApiModel;
using PriceService.Services;

namespace PriceService.Controllers;

[ApiController]
public class PriceController : ControllerBase
{
    private readonly ILogger<PriceController> _logger;
    private readonly IPriceService _priceService;

    public PriceController(ILogger<PriceController> logger, IPriceService priceService)
    {
        _logger = logger;
        _priceService = priceService;
    }

    [HttpGet]
    [Route("api/price/info/{symbol}")]
    public IActionResult GetInfo(string symbol)
    {
        var result = _priceService.GetCompanyInfo(symbol);
        return new OkObjectResult(result);
    }

    [HttpGet]
    [Route("api/price/closing/{symbol}")]
    public IActionResult GetClosingPrice(
        [FromRoute] string symbol)
    {
        var result = _priceService.GetLastClosingPrice(symbol);
        return new OkObjectResult(result);
    }

    [HttpPost]
    [Route("api/admin/symbolupdate")]
    public IActionResult UpdateSymbols()
    {
        var result = _priceService.UpdateSymbols();
        return new OkObjectResult(result);
    }

    [HttpGet]
    [Route("api/admin/symbolupdate/status/{runId}")]
    public IActionResult GetSymbolUpdateStatus(
        [FromRoute] int runId)
    {
        var result = _priceService.GetSymbolUpdateStatus(runId);
        return new OkObjectResult(result);
    }

    // [HttpGet]
    // [Route("api/price/range/{symbol}")]
    // public IActionResult GetPriceRange(
    //     [FromRoute] string symbol, DateTime startdate, DateTime enddate)
    // {
    //     if (enddate == DateTime.MinValue)
    //         enddate = DateTime.Now;
    //     if (startdate == DateTime.MinValue)
    //         startdate = enddate.AddDays(-3); // Make start date 3 days earlier than current time by default

    //     var result = _priceService.GetPrices(symbol, startdate, enddate);
    //     return new OkObjectResult(result);
    // }

    [HttpGet]
    [Route("api/version")]
    public IActionResult GetVersion()
    {
        return new OkObjectResult(GlobalEnv.API_VERSION);
    }
}
