using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SALearning.Services;
using SALearning.ApiModel;
using ProtfolioService.Shared;

namespace SALearning.Controllers
{
    public class HoldingController
    {
        private readonly IPortfolioService _portfolioSvc;
        
        public HoldingController(IPortfolioService portfolioService)
        {
            _portfolioSvc = portfolioService;
        }

        [FunctionName("GetHolding")]
        public async Task<IActionResult> GetHolding(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "account/{accountNumber}/holding/{symbol}")] HttpRequest req,
            int accountNumber,
            string symbol,
            ILogger log)
        {
            log.LogInformation($"Get Holding {symbol} called for account {accountNumber}");
            var holding = _portfolioSvc.GetHolding(accountNumber, symbol);

            return new OkObjectResult(await Task.FromResult(holding));
        }

        [FunctionName("UpdateHolding")]
        public async Task<IActionResult> UpdateHolding(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "account/{accountNumber}/holding/{symbol}")] HttpRequest req,
            int accountNumber,
            string symbol,
            ILogger log)
        {
            symbol = symbol.ToUpper();
            log.LogInformation($"UpdateHolding {symbol} called for account {accountNumber}");

            var holdingBody = await req.BuildModel<Holding>();
            if (holdingBody.Symbol != symbol)
            {
                return new BadRequestObjectResult(new { Description = "Symbol in path must match symbol in body" });
            }

            var holding = _portfolioSvc.UpdateHolding(accountNumber, holdingBody);

            return new OkObjectResult(await Task.FromResult(holding));
        }

        [FunctionName("DeleteHolding")]
        public async Task<IActionResult> DeleteHolding(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "account/{accountNumber}/holding/{symbol}")] HttpRequest req,
            int accountNumber,
            string symbol,
            ILogger log)
        {
            log.LogInformation($"DeleteHolding {symbol} called for account {accountNumber}");
            int affectedRecs = _portfolioSvc.DeleteHolding(accountNumber, symbol);

            return new OkObjectResult(await Task.FromResult(affectedRecs));
        }

        [FunctionName("CreateHolding")]
        public async Task<IActionResult> CreateHolding(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "account/{accountNumber}/holding")] HttpRequest req,
            int accountNumber,
            ILogger log)
        {
            log.LogInformation($"CreateHolding called for account {accountNumber}");

            var holdingBody = await req.BuildModel<Holding>();
            var holding = _portfolioSvc.CreateHolding(accountNumber, holdingBody);

            return new OkObjectResult(await Task.FromResult(holding));
        }

        [FunctionName("GetAllHoldings")]
        public async Task<IActionResult> GetAllHoldings(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "account/{accountNumber}/holding")] HttpRequest req,
            int accountNumber, 
            ILogger log)
        {
            log.LogInformation($"GetAllHoldings called for account {accountNumber}");
            var holdingList = _portfolioSvc.GetHoldingList(accountNumber);

            return new OkObjectResult(await Task.FromResult(holdingList));
        }

    }
}
