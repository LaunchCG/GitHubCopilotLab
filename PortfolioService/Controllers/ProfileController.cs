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
    public class ProfileController
    {
        private readonly IPortfolioService _portfolioSvc;
        
        public ProfileController(IPortfolioService portfolioService)
        {
            _portfolioSvc = portfolioService;
        }

        [FunctionName("Login")]
        public async Task<IActionResult> Login(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "profile/login")] HttpRequest req,
            ILogger log)
        {
            var loginBody = await req.BuildModel<UserProfile>();
            log.LogInformation($"Login called with local object id = {loginBody.AadId}");

            var profile = _portfolioSvc.Login(loginBody);
            return new OkObjectResult(await Task.FromResult(profile));
        }

        [FunctionName("GetProfile")]
        public async Task<IActionResult> GetProfile(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "profile/{profileId}")] HttpRequest req,
            int profileId,
            ILogger log)
        {
            log.LogInformation($"Get Profile called for id {profileId}");
            var profile = _portfolioSvc.GetProfile(profileId);

            return new OkObjectResult(await Task.FromResult(profile));
        }

        [FunctionName("UpdateProfile")]
        public async Task<IActionResult> UpdateProfile(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "profile/{profileId}")] HttpRequest req,
            int profileId,
            ILogger log)
        {
            log.LogInformation($"UpdateProfile called for id {profileId}");

            var profileBody = await req.BuildModel<UserProfile>();
            if (profileBody.ProfileId != profileId)
            {
                return new BadRequestObjectResult(new { Description = "Profile Id in path must match profile Id in body" });
            }

            var profile = _portfolioSvc.UpdateProfile(profileBody);

            return new OkObjectResult(await Task.FromResult(profile));
        }

        [FunctionName("DeleteProfile")]
        public async Task<IActionResult> DeleteProfile(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "profile/{profileId}")] HttpRequest req,
            int profileId,
            ILogger log)
        {
            log.LogInformation($"DeleteProfile called for profile {profileId}");
            int affectedRecs = _portfolioSvc.DeleteProfile(profileId);

            return new OkObjectResult(await Task.FromResult(affectedRecs));
        }

        [FunctionName("GetAllProfiles")]
        public async Task<IActionResult> GetAllProfiles(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "profile")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation($"GetAllProfiles called");
            var profileList = _portfolioSvc.GetProfileList();

            return new OkObjectResult(await Task.FromResult(profileList));
        }
    }
}
