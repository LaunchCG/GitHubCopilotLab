using System.Collections.Generic;
using System.Threading.Tasks;
using SALearning.ApiModel;

namespace SALearning.Services;
public interface IPricingService
{
    Task<PricingInfo> GetPricingInfo(string symbol);
    Task<List<decimal>> GetPricingList(List<string> symbolList);
}
