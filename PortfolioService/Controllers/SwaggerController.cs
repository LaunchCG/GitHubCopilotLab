using System.Net.Http;
using System.Threading.Tasks;
using AzureFunctions.Extensions.Swashbuckle;
using AzureFunctions.Extensions.Swashbuckle.Attribute;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
 
namespace SALearning.Controllers
{
  public static class Swagger
  {
    [SwaggerIgnore]
    [FunctionName("SwaggerApiJson")]
    public static Task<HttpResponseMessage> RunJson(
        [HttpTrigger(
            AuthorizationLevel.Anonymous, 
            "get", Route = "swagger/json")]
        HttpRequestMessage req,
        [SwashBuckleClient] ISwashBuckleClient swashbuckleClient)
    {
      return Task.FromResult(
            swashbuckleClient.CreateSwaggerJsonDocumentResponse(req));
    }
 
    [SwaggerIgnore]
    [FunctionName("Swagger")]
    public static Task<HttpResponseMessage> RunUi(
        [HttpTrigger(
            AuthorizationLevel.Anonymous, 
            "get", 
            Route = "swagger/ui")]
        HttpRequestMessage req,
        [SwashBuckleClient] ISwashBuckleClient swashbuckleClient)
    {
      // CreateOpenApiUIResponse generates the HTML page from the JSON results
      return Task.FromResult(
            swashbuckleClient.CreateSwaggerUIResponse(req, "swagger/json"));
    }
  }
}