using PMB.Model.DTO.IraniCard;
using PMB.Model.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PMB.Business
{
    public class IraniCardBusiness : IIraniCardBusiness
    {
        private readonly IErrorBusiness _errorBusiness;
        public IraniCardBusiness(IErrorBusiness errorBusiness)
        {
            _errorBusiness = errorBusiness;
        }

        public async Task<ResultApiModel<IraniCardApiModel>> GetBuyPrice(string coockie)
        {
            try
            {
                using (var handler = new HttpClientHandler { UseCookies = false })
                using (var client = new HttpClient(handler) { BaseAddress = new Uri("https://api.iranicard.ir") })
                {
                    var message = new HttpRequestMessage(HttpMethod.Get, "/api/modules/Money/v1/client/showProduct/BuyPerfectmoney");
                    message.Headers.Add("Cookie", $"customer_access_token={coockie}");

                    var response = await client.SendAsync(message);

                    if (response.IsSuccessStatusCode)
                    {
                        var result = Newtonsoft.Json.JsonConvert.DeserializeObject<IraniCardApiModel>(await response.Content.ReadAsStringAsync());
                        if (result.StatusCode == 200)
                        {
                            return new ResultApiModel<IraniCardApiModel>
                            {
                                Result = true,
                                Data = result,
                                Message = "success get buy price irani card"
                            };
                        }
                        else
                        {
                            return new ResultApiModel<IraniCardApiModel>
                            {
                                Result = false,
                                Message = "error in get buy price irani card error message: " + result.Status
                            };
                        }
                    }
                    else
                    {
                        _errorBusiness.AddError(await response.Content.ReadAsStringAsync(), MethodBase.GetCurrentMethod().DeclaringType.FullName);
                        return new ResultApiModel<IraniCardApiModel>
                        {
                            Result = false,
                            Message = "error in get buy price error message: " + await response.Content.ReadAsStringAsync()
                        };
                    }

                    throw new Exception();
                }
            }
            catch (Exception ex)
            {
                _errorBusiness.AddError(ex, MethodBase.GetCurrentMethod().DeclaringType.FullName);
                return new ResultApiModel<IraniCardApiModel>
                {
                    Result = false,
                    Message = "error in get buy price irani card exception message: " + ex.Message
                };
            }
        }

        public async Task<ResultApiModel<IraniCardPriceModel>> GetPrice(IraniCardBodyLoginModel model)
        {
            try
            {
                var login = await Login(model);
                if (!login.Result)
                {
                    return new ResultApiModel<IraniCardPriceModel>
                    {
                        Result = false,
                        Message = login.Message
                    };
                }
                var resultBuyPrice = await GetBuyPrice(login.Data.Coockie);
                var resultSellPrice = await GetSellPrice(login.Data.Coockie);

                if (!resultBuyPrice.Result)
                {
                    return new ResultApiModel<IraniCardPriceModel>
                    {
                        Result = false,
                        Message = resultBuyPrice.Message
                    };
                }

                if (!resultSellPrice.Result)
                {
                    return new ResultApiModel<IraniCardPriceModel>
                    {
                        Result = false,
                        Message = resultSellPrice.Message
                    };
                }

                return new ResultApiModel<IraniCardPriceModel>
                {
                    Result = true,
                    Message = "success get price irani card",
                    Data = new IraniCardPriceModel
                    {
                        BuyPrice = resultBuyPrice.Data.Data.Product.Form.PriceForm.RelatedCurrencies.FirstOrDefault().Price / 10,
                        SellPrice = resultSellPrice.Data.Data.Product.Form.PriceForm.RelatedCurrencies.FirstOrDefault().Price / 10
                    }
                };
            }
            catch (Exception ex)
            {
                _errorBusiness.AddError(ex, MethodBase.GetCurrentMethod().DeclaringType.FullName);
                return new ResultApiModel<IraniCardPriceModel>
                {
                    Result = false,
                    Message = "error in call get price irani card error message: " + ex.Message
                };
            }
        }

        public async Task<ResultApiModel<IraniCardApiModel>> GetSellPrice(string coockie)
        {
            try
            {
                using (var handler = new HttpClientHandler { UseCookies = false })
                using (var client = new HttpClient(handler) { BaseAddress = new Uri("https://api.iranicard.ir") })
                {
                    var message = new HttpRequestMessage(HttpMethod.Get, "/api/modules/Money/v1/client/showProduct/salePerfectmoney");
                    message.Headers.Add("Cookie", $"customer_access_token={coockie}");

                    var response = await client.SendAsync(message);

                    if (response.IsSuccessStatusCode)
                    {
                        var result = Newtonsoft.Json.JsonConvert.DeserializeObject<IraniCardApiModel>(await response.Content.ReadAsStringAsync());
                        if (result.StatusCode == 200)
                        {
                            return new ResultApiModel<IraniCardApiModel>
                            {
                                Result = true,
                                Data = result,
                                Message = "success get sell price irani card"
                            };
                        }
                        else
                        {
                            return new ResultApiModel<IraniCardApiModel>
                            {
                                Result = false,
                                Message = "error in get sell price irani card error message: " + result.Status
                            };
                        }
                    }
                    else
                    {
                        _errorBusiness.AddError(await response.Content.ReadAsStringAsync(), MethodBase.GetCurrentMethod().DeclaringType.FullName);
                        return new ResultApiModel<IraniCardApiModel>
                        {
                            Result = false,
                            Message = "error in get sell price error message: " + await response.Content.ReadAsStringAsync()
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                _errorBusiness.AddError(ex, MethodBase.GetCurrentMethod().DeclaringType.FullName);
                return new ResultApiModel<IraniCardApiModel>
                {
                    Result = false,
                    Message = "error in get sell price irani card exception message: " + ex.Message
                };
            }
        }

        public async Task<ResultApiModel<IraniCardResultLoginModel>> Login(IraniCardBodyLoginModel model)
        {
            try
            {
                var cookieContainer = new CookieContainer();
                using (var httpClientHandler = new HttpClientHandler
                {
                    CookieContainer = cookieContainer
                })
                {
                    using (var httpClient = new HttpClient(httpClientHandler))
                    {
                        var json = "{\"mobile\":\"" + model.Mobile + "\",\"password\":\"" + model.Password + "\"}";
                        var data = new StringContent(json, Encoding.UTF8, "application/json");

                        ServicePointManager.Expect100Continue = true;
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                        var response = await httpClient.PostAsync("https://api.iranicard.ir/api/v1/login", data);

                        if (response.IsSuccessStatusCode)
                        {
                            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<IraniCardResultLoginModel>(await response.Content.ReadAsStringAsync());
                            result.Coockie = cookieContainer.GetCookies(new Uri("https://api.iranicard.ir")).Cast<Cookie>()
                                .Where(a => a.Name.Equals("customer_access_token")).FirstOrDefault().Value;

                            if (result.StatusCode == 200)
                            {
                                return new ResultApiModel<IraniCardResultLoginModel>
                                {
                                    Data = result,
                                    Message = "success call api login irani card",
                                    Result = true
                                };
                            }
                            else
                            {
                                _errorBusiness.AddError(result.Status, MethodBase.GetCurrentMethod().DeclaringType.FullName);
                                return new ResultApiModel<IraniCardResultLoginModel>
                                {
                                    Result = false,
                                    Message = "error in call login api irany card error message: " + result.Status
                                };
                            }
                        }
                        else
                        {
                            _errorBusiness.AddError(await response.Content.ReadAsStringAsync(), MethodBase.GetCurrentMethod().DeclaringType.FullName);
                            return new ResultApiModel<IraniCardResultLoginModel>
                            {
                                Result = false,
                                Message = "error in call login api irany card error Message: " + await response.Content.ReadAsStringAsync()
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _errorBusiness.AddError(ex, MethodBase.GetCurrentMethod().DeclaringType.FullName);
                return new ResultApiModel<IraniCardResultLoginModel>
                {
                    Result = false,
                    Message = "error in call login api irany card exception Message: " + ex.Message
                };
            }
        }
    }
    public interface IIraniCardBusiness
    {
        Task<ResultApiModel<IraniCardResultLoginModel>> Login(IraniCardBodyLoginModel model);
        Task<ResultApiModel<IraniCardPriceModel>> GetPrice(IraniCardBodyLoginModel model);
        Task<ResultApiModel<IraniCardApiModel>> GetBuyPrice(string coockie);
        Task<ResultApiModel<IraniCardApiModel>> GetSellPrice(string coockie);
    }
}
