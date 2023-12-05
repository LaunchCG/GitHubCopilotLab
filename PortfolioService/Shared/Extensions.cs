using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Newtonsoft.Json;

namespace ProtfolioService.Shared;

public static class Extensions 
{
    public static async Task<T> BuildModel<T>(this HttpRequest req)
    {
        var result = JsonConvert.DeserializeObject<T>(await req.ReadAsStringAsync());
        return result;
    }

    public static async Task<Validation<T>> Validate<T>(this HttpRequest req)
    {
        var result = await req.BuildModel<T>();
        var resultList = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(result, new ValidationContext(result,null,null), resultList );
        var message = $"Invalid input: {string.Join(", ", resultList.Select(s => s.ErrorMessage).ToArray())}";
        return new Validation<T>(isValid,result,message);

    }

}