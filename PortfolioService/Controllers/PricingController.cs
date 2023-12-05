using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SALearning.Services;

namespace SALearning.Controllers;

public class PricingController
{
    private readonly IPricingService _pricingSvc;
    
    public PricingController(IPricingService pricingService)
    {
        _pricingSvc = pricingService;
    }

    [FunctionName("GetPricingInfo")]
    public async Task<IActionResult> GetPricingInfo(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "pricing/getinfo/{symbol}")] HttpRequest req, 
        string symbol,
        ILogger log)
    {
        log.LogInformation($"Get Pricing Info called with symbol=${symbol}");
        var retval = await _pricingSvc.GetPricingInfo(symbol);

        return new OkObjectResult(await Task.FromResult(retval));
    }
}