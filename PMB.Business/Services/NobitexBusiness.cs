using PMB.Model.DTO.Nobitex;
using PMB.Model.General;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PMB.Services.Business
{
    public class NobitexBusiness : INobitexBusiness
    {
        private readonly IErrorBusiness _errorBusiness;
        public NobitexBusiness(IErrorBusiness errorBusiness)
        {
            _errorBusiness = errorBusiness;
        }

        public async Task<ResultApiModel<NobitexPriceModel>> GetPrice()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var json = Newtonsoft.Json.JsonConvert.SerializeObject(new Dictionary<string, string> { { "srcCurrency", "usdt" }, { "dstCurrency", "rls" } });
                    var data = new StringContent(json, Encoding.UTF8, "application/json");

                    ServicePointManager.Expect100Continue = true;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                    var response = await client.PostAsync("https://api.nobitex.ir/market/stats", data);

                    if (!response.IsSuccessStatusCode)
                    {
                        _errorBusiness.AddError(await response.Content.ReadAsStringAsync(), MethodBase.GetCurrentMethod().DeclaringType.FullName);
                        return new ResultApiModel<NobitexPriceModel>
                        {
                            Result = false,
                            Message = "error in call nobitex api error message: " + await response.Content.ReadAsStringAsync()
                        };
                    }

                    var result = Newtonsoft.Json.JsonConvert.DeserializeObject<NobitexResponseModel>(await response.Content.ReadAsStringAsync());
                    if (result.Status != "ok")
                    {
                        _errorBusiness.AddError($"status message: {result.Status}", MethodBase.GetCurrentMethod().DeclaringType.FullName);
                        return new ResultApiModel<NobitexPriceModel>
                        {
                            Result = false,
                            Message = "error in call nobitex api status code: " + result.Status
                        };
                    }

                    if (result.Stats.USDT.IsClosed)
                    {
                        _errorBusiness.AddError("closed price usdt nobitex", MethodBase.GetCurrentMethod().DeclaringType.FullName);
                        return new ResultApiModel<NobitexPriceModel>
                        {
                            Result = false,
                            Message = "closed price usdt nobitex"
                        };
                    }

                    return new ResultApiModel<NobitexPriceModel>
                    {
                        Data = new NobitexPriceModel
                        {
                            Price = Convert.ToDouble(result.Stats.USDT.Latest) / 10 * 1003 / 1021
                        },
                        Result = true,
                        Message = "success call api nobitex"
                    };
                }
            }
            catch (Exception ex)
            {
                _errorBusiness.AddError(ex, MethodBase.GetCurrentMethod().DeclaringType.FullName);
                return new ResultApiModel<NobitexPriceModel>
                {
                    Result = false,
                    Message = "error in call nobitex api exception message: " + ex.Message
                };
            }
        }
    }
    public interface INobitexBusiness
    {
        Task<ResultApiModel<NobitexPriceModel>> GetPrice();
    }
}
