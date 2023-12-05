using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using SALearning.ApiModel;

namespace SALearning.Services
{
    public class PricingService : IPricingService
    {
        static HttpClient _httpClient = new HttpClient();
        
        class CompanyInfo {
            public string Name { get; set; }
            public string Description { get; set; }
        }

        class CompanyPrice {
            public DateTime CloseDate { get; set; }
            public double ClosePrice { get; set; }
        }

        public PricingService()
        {
        } 

        public async Task<PricingInfo> GetPricingInfo(string symbol)
        {
            var getUrl = $"{GlobalEnv.PRICEURL}/api/price/info/{symbol}";
            var companyInfo = await _httpClient.GetFromJsonAsync<CompanyInfo>(getUrl);

            if (companyInfo != null)
            {
                var getPriceUrl = $"{GlobalEnv.PRICEURL}/api/price/closing/{symbol}";
                var price = await _httpClient.GetFromJsonAsync<CompanyPrice>(getPriceUrl);

                return new PricingInfo {
                    Symbol = symbol.ToUpper(),
                    Name = companyInfo.Name,
                    Description = companyInfo.Description,
                    Price = price.ClosePrice
                };
            }

            return null;
        }

        public async Task<List<decimal>> GetPricingList(List<string> symbolList)
        {
            var retList = new List<decimal>();

            foreach (var symbol in symbolList)
            {
                var getPriceUrl = $"{GlobalEnv.PRICEURL}/api/price/closing/{symbol}";
                var price = await _httpClient.GetFromJsonAsync<CompanyPrice>(getPriceUrl);
                retList.Add(Convert.ToDecimal(price.ClosePrice));
            }
            return retList;
        }
    }
}
