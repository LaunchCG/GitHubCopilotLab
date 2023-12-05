using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SALearning.Services;
using SALearning.ApiModel;
using ProtfolioService.Shared;

namespace SALearning.Controllers;

public class AccountController
{
    private readonly IPortfolioService _portfolioSvc;
    
    public AccountController(IPortfolioService portfolioService)
    {
        _portfolioSvc = portfolioService;
    }

    [FunctionName("GetLeaderboard")]
    public async Task<IActionResult> GetLeaderboard(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "leaderboard")] HttpRequest req,
        ILogger log)
    {
        log.LogInformation($"Get Leaderboard called");
        var retval = _portfolioSvc.GetLeaderboard();

        return new OkObjectResult(await retval);
    }

    [FunctionName("GetAccount")]
    public async Task<IActionResult> GetAccount(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "account/{accountId}")] HttpRequest req,
        int accountId,
        ILogger log)
    {
        log.LogInformation($"Get Account called for account {accountId}");
        var account = _portfolioSvc.GetAccount(accountId);

        return new OkObjectResult(await Task.FromResult(account));
    }

    [FunctionName("GetAccountPerformance")]
    public async Task<IActionResult> GetAccountPerformance(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "account/{accountId}/performance")] HttpRequest req,
        int accountId,
        ILogger log)
    {
        log.LogInformation($"Get Account Performance called for account {accountId}");
        var retVal = await _portfolioSvc.GetAccountPerformance(accountId);

        return new OkObjectResult(await Task.FromResult(retVal));
    }

    [FunctionName("UpdateAccount")]
    public async Task<IActionResult> UpdateAccount(
        [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "account/{accountId}")] HttpRequest req,
        int accountId,
        ILogger log)
    {
        log.LogInformation($"UpdateAccount called for account {accountId}");
        
        var accountBody = await req.BuildModel<Account>(); 
        if (accountBody.AccountNumber != accountId)
        {
            return new BadRequestObjectResult(new { Description = "Account Id in path must match account Id in body" });
        }

        var account = _portfolioSvc.UpdateAccount(accountBody);

        return new OkObjectResult(await Task.FromResult(account));
    }

    [FunctionName("DeleteAccount")]
    public async Task<IActionResult> DeleteAccount(
        [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "account/{accountId}")] HttpRequest req,
        int accountId,
        ILogger log)
    {
        log.LogInformation($"DeleteAccount called for account {accountId}");
        int affectedRecs = _portfolioSvc.DeleteAccount(accountId);

        return new OkObjectResult(await Task.FromResult(affectedRecs));
    }

    [FunctionName("CreateAccount")]
    public async Task<IActionResult> CreateAccount(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "account")] HttpRequest req,
        ILogger log)
    {
        log.LogInformation($"CreateAccount called");

        var accountBody = await req.BuildModel<Account>();

        if (accountBody.ProfileId == 0)
            return new BadRequestObjectResult(new { Description = "CreateAccount: Expected profile id as part of account creation request" });

        var account = _portfolioSvc.CreateAccount(accountBody);

        return new OkObjectResult(await Task.FromResult(account));
    }

    [FunctionName("GetProfileAccount")]
    public async Task<IActionResult> GetProfileAccount(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "account/profile/{profileId}")] HttpRequest req,
        int profileId,
        ILogger log)
    {
        log.LogInformation($"Get Profile Account called");

        var account = _portfolioSvc.GetProfileAccount(profileId);

        return new OkObjectResult(await Task.FromResult(account));
    }

    [FunctionName("GetAllAccounts")]
    public async Task<IActionResult> GetAllAccounts(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "account")] HttpRequest req,
        ILogger log)
    {
        log.LogInformation($"GetAllAccounts called");
        var accountList = _portfolioSvc.GetAccountList();

        return new OkObjectResult(await Task.FromResult(accountList));
    }

    [FunctionName("BuyStock")]
    public async Task<IActionResult> BuyStock(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "account/{accountId}/buystock")] HttpRequest req,
        int accountId,
        ILogger log)
    {
        log.LogInformation($"Buy Stock Called");
        var operation = await req.BuildModel<Operation>();

        if (operation.OperationType != OperationType.BuyStock) {
            return new BadRequestObjectResult(new { Description = "BuyStock: Expected operation type to be Buy Stock" });
        }

        var retVal = await _portfolioSvc.TradeStock(accountId, operation);

        return new OkObjectResult(retVal);
    }

    [FunctionName("SellStock")]
    public async Task<IActionResult> SellStock(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "account/{accountId}/sellstock")] HttpRequest req,
        int accountId,
        ILogger log)
    {
        log.LogInformation($"Sell Stock Called");
        var operation = await req.BuildModel<Operation>();

        if (operation.OperationType != OperationType.SellStock) {
            return new BadRequestObjectResult(new { Description = "SellStock: Expected operation type to be Sell Stock" });
        }

        var retVal = await _portfolioSvc.TradeStock(accountId, operation);

        return new OkObjectResult(retVal);
    }

    [FunctionName("TransferCash")]
    public async Task<IActionResult> TransferCash(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "account/{accountId}/transfercash")] HttpRequest req,
        int accountId,
        ILogger log)
    {
        log.LogInformation($"TransferCash called");
        var amount = await req.BuildModel<decimal>();

        var retVal = await _portfolioSvc.TransferCash(accountId, amount);

        return new OkObjectResult(retVal);
    }

        [FunctionName("GenerateRecommendations")]
    public async Task<IActionResult> GenerateRecommendations(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "account/{accountId}/recommendations")] HttpRequest req,
        int accountId,
        ILogger log)
    {
        log.LogInformation($"GenerateRecommendations called");

        var retVal = await _portfolioSvc.GenerateRecommendations(accountId);

        return new OkObjectResult(retVal);
    }

}
