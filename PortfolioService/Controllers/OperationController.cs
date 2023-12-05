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
    public class OperationController
    {
        private readonly IPortfolioService _portfolioSvc;
        
        public OperationController(IPortfolioService portfolioService)
        {
            _portfolioSvc = portfolioService;
        }

        [FunctionName("GetOperation")]
        public async Task<IActionResult> GetOperation(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "account/{accountNumber}/operation/{opId}")] HttpRequest req,
            int accountNumber,
            int opId,
            ILogger log)
        {
            log.LogInformation($"Get Operation {opId} called for account {accountNumber}");
            
            var operation = _portfolioSvc.GetOperation(accountNumber,opId);
            if(operation is null)
            {
                return new NotFoundResult();
            }

            return new OkObjectResult(await Task.FromResult(operation));
        }

        [FunctionName("UpdateOperation")]
        public async Task<IActionResult> UpdateOperation(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "account/{accountNumber}/operation/{opId}")] HttpRequest req,
            int accountNumber,
            int opId,
            ILogger log)
        {
            log.LogInformation($"UpdateOperation {opId} called for account {accountNumber}");
            
            var validation = await req.Validate<Operation>();
            if(!validation.Isvalid)
            {
                return new BadRequestObjectResult(validation.Message);
            }

            if (validation.Value.OperationId != opId)
            {
                return new BadRequestObjectResult(new { Description = "OperationId in path must match OperationId in body" });
            }

            var operation = _portfolioSvc.GetOperation(accountNumber,opId);
            if(operation is null)
            {
                return new NotFoundResult();
            }

            operation = _portfolioSvc.UpdateOperation(accountNumber, validation.Value);

            return new OkObjectResult(await Task.FromResult(operation));
        }

        [FunctionName("DeleteOperation")]
        public async Task<IActionResult> DeleteOperation(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "account/{accountNumber}/operation/{opId}")] HttpRequest req,
            int accountNumber,
            int opId,
            ILogger log)
        {
            log.LogInformation($"DeleteOperation {opId} called for account {accountNumber}");

            var operation = _portfolioSvc.GetOperation(accountNumber,opId);
            if(operation is null)
            {
                return new NotFoundResult();
            }

            int affectedRecs = _portfolioSvc.DeleteOperation(accountNumber, opId);

            return new OkObjectResult(await Task.FromResult(affectedRecs));
        }

        [FunctionName("CreateOperation")]
        public async Task<IActionResult> CreateOperation(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "account/{accountNumber}/operation")] HttpRequest req,
            int accountNumber,
            ILogger log)
        {
            log.LogInformation($"CreateOperation called for account {accountNumber}");

            // verify account exists
            var account = _portfolioSvc.GetAccount(accountNumber);
            if(account is null)
            {
                return new BadRequestObjectResult("Account number does not exist");
            }

            var validation = await req.Validate<Operation>();
            if(!validation.Isvalid)
            {
                return new BadRequestObjectResult(validation.Message);
            }
            
            var operation = _portfolioSvc.CreateOperation(accountNumber, validation.Value);

            return new OkObjectResult(await Task.FromResult(operation));
        }

        [FunctionName("GetAllOperations")]
        public async Task<IActionResult> GetAllOperations(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "account/{accountNumber}/operation")] HttpRequest req,
            int accountNumber, 
            ILogger log)
        {
            log.LogInformation($"GetAllOperations called for account {accountNumber}");
            
            var account = _portfolioSvc.GetAccount(accountNumber);
            if(account is null)
            {
                return new BadRequestObjectResult("Account number does not exist");
            }
            
            var operationList = _portfolioSvc.GetOperationList(accountNumber);

            return new OkObjectResult(await Task.FromResult(operationList));
        }
    }
}
