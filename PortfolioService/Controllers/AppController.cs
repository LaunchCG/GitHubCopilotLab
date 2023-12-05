using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace SALearning.Controllers;

public class AppController
{
    [FunctionName("GetVersion")]
    public IActionResult GetVersion(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "application/version")] HttpRequest req,
        ILogger log)
    {
        log.LogInformation("GetVersion called");
        return new OkObjectResult(GlobalEnv.API_VERSION);
    }
}
