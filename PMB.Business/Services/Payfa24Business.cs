using PMB.Model.DTO.Payfa24;
using PMB.Model.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PMB.Services.Business
{
    public class Payfa24Business : IPayfa24Business
    {
        private readonly IErrorBusiness _errorBusiness;
        public Payfa24Business(IErrorBusiness errorBusiness)
        {
            _errorBusiness = errorBusiness;
        }

        public async Task<string> GetCsrfToken(Payfa24CookieModel model, string url)
        {
            try
            {
                using (var handler = new HttpClientHandler { UseCookies = false })
                using (var client = new HttpClient(handler) { BaseAddress = new Uri(url) })
                {
                    var message = new HttpRequestMessage(HttpMethod.Get, "/");
                    message.Headers.Add("Cookie", $"XSRF-TOKEN={model.XSRFTOKEN};payfa24_session={model.Session}");
                    var response = await client.SendAsync(message);
                    if (response.IsSuccessStatusCode)
                    {
                        return (await response.Content.ReadAsStringAsync()).Split(new[] { "name=\"csrf-token\"" }, StringSplitOptions.None)[1].Split('>')[0].Split('\"')[1];
                    }
                    else
                    {
                        _errorBusiness.AddError(await response.Content.ReadAsStringAsync(), MethodBase.GetCurrentMethod().DeclaringType.FullName);
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                _errorBusiness.AddError(ex, MethodBase.GetCurrentMethod().DeclaringType.FullName);
                return null;
            }
        }

        public async Task<ResultApiModel<Payfa24ResultItemModel>> GetPrice(Payfa24CookieModel model)
        {
            try
            {
                var csrf = await GetCsrfToken(model, "https://panel.payfa24.com");
                using (var handler = new HttpClientHandler { UseCookies = false })
                using (var client = new HttpClient(handler) { BaseAddress = new Uri("https://panel.payfa24.com") })
                {
                    var message = new HttpRequestMessage(HttpMethod.Post, "/stock");
                    message.Headers.Add("Cookie", $"XSRF-TOKEN={model.XSRFTOKEN};payfa24_session={model.Session};analytics_token=35782813-91c9-b027-1fd6-8969dd6bd8be");
                    message.Content = new StringContent("{\"_token\":\"" + csrf + "\"}", Encoding.UTF8, "application/json");
                    var response = await client.SendAsync(message);
                    if (response.IsSuccessStatusCode)
                    {
                        var responseValue = await response.Content.ReadAsStringAsync();
                        if (responseValue.Contains("<!doctype html>"))
                        {
                            _errorBusiness.AddError("expired coockie payfa24", MethodBase.GetCurrentMethod().DeclaringType.FullName);
                            return new ResultApiModel<Payfa24ResultItemModel>
                            {
                                Result = false,
                                Message = "expired coockie payfa24"
                            };
                        }
                        return new ResultApiModel<Payfa24ResultItemModel>
                        {
                            Result = true,
                            Message = "success call api payfa24",
                            Data = Newtonsoft.Json.JsonConvert.DeserializeObject<Payfa24ResultModel>(responseValue).PerfectMoney
                        };
                    }
                    else
                    {
                        _errorBusiness.AddError(await response.Content.ReadAsStringAsync(), MethodBase.GetCurrentMethod().DeclaringType.FullName);
                        return new ResultApiModel<Payfa24ResultItemModel>
                        {
                            Message = "error in call get price payfa24 error message: " + response.Content.ReadAsStringAsync(),
                            Result = false
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                _errorBusiness.AddError(ex, MethodBase.GetCurrentMethod().DeclaringType.FullName);
                return new ResultApiModel<Payfa24ResultItemModel>
                {
                    Result = false,
                    Message = "error in call get price payfa24 exceptionMessage:" + ex.Message
                };
            }
        }


    }
    public interface IPayfa24Business
    {
        Task<string> GetCsrfToken(Payfa24CookieModel model, string url);
        Task<ResultApiModel<Payfa24ResultItemModel>> GetPrice(Payfa24CookieModel model);
    }
}
