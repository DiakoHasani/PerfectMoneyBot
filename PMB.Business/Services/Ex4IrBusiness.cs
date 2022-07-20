using PMB.Model.DTO.Ex4;
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
    public class Ex4IrBusiness : IEx4IrBusiness
    {
        private readonly IErrorBusiness _errorBusiness;
        public Ex4IrBusiness(IErrorBusiness errorBusiness)
        {
            _errorBusiness = errorBusiness;
        }
        public async Task<Ex4TokenModel> GetCookie(string url)
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
                        var html = await (await httpClient.GetAsync(new Uri(url))).Content.ReadAsStringAsync();
                        return new Ex4TokenModel
                        {
                            Cookies = cookieContainer.GetCookies(new Uri(url)).Cast<Cookie>(),
                            CsrfToken = GetCsrfToken(html)
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                _errorBusiness.AddError(ex, MethodBase.GetCurrentMethod().DeclaringType.FullName);
                return null;
            }
        }

        public string GetCsrfToken(string html)
        {
            try
            {
                return html.Split(new[] { "name=\"csrf-token\"" }, StringSplitOptions.None)[1].Split('>')[0].Split('\"')[1];
            }
            catch (Exception ex)
            {
                _errorBusiness.AddError(ex, MethodBase.GetCurrentMethod().DeclaringType.FullName);
                return "";
            }
        }

        public async Task<ResultApiModel<Ex4ApiModel>> GetPrice()
        {
            try
            {
                var ex4token = await GetCookie("https://ex4ir.net");
                using (var handler = new HttpClientHandler { UseCookies = false })
                using (var client = new HttpClient(handler) { BaseAddress = new Uri("https://ex4ir.net") })
                {
                    var message = new HttpRequestMessage(HttpMethod.Post, "/assets/get");
                    message.Headers.Add("x-csrf-token", ex4token.CsrfToken);
                    message.Headers.Add("Cookie", $"XSRF-TOKEN={ex4token.Cookies.Where(a => a.Name == "XSRF-TOKEN").FirstOrDefault().Value};" +
                        " _ga=GA1.2.1026739839.1648366814; _gat_gtag_UA_110545111_1=1; _gid=GA1.2.1636441919.1656325824;" +
                        $"sesid={ex4token.Cookies.Where(a => a.Name == "sesid").FirstOrDefault().Value}");

                    var response = await client.SendAsync(message);
                    if (response.IsSuccessStatusCode)
                    {
                        return new ResultApiModel<Ex4ApiModel>
                        {
                            Result = true,
                            Message = "Success Call Api Ex4",
                            Data = Newtonsoft.Json.JsonConvert
                        .DeserializeObject<List<Ex4ApiModel>>(await response.Content.ReadAsStringAsync()).Where(a => a.Id == 2).FirstOrDefault()
                        };
                    }
                    else
                    {

                        _errorBusiness.AddError(await response.Content.ReadAsStringAsync(), MethodBase.GetCurrentMethod().DeclaringType.FullName);
                        return new ResultApiModel<Ex4ApiModel>
                        {
                            Result = false,
                            Message = "Error in call api Ex4 Message: " + (await response.Content.ReadAsStringAsync())
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                _errorBusiness.AddError(ex, MethodBase.GetCurrentMethod().DeclaringType.FullName);
                return new ResultApiModel<Ex4ApiModel>
                {
                    Result = false,
                    Message = "Error in call api Ex4 Message: " + ex.Message
                };
            }
        }
    }
    public interface IEx4IrBusiness
    {
        string GetCsrfToken(string html);
        Task<Ex4TokenModel> GetCookie(string url);
        Task<ResultApiModel<Ex4ApiModel>> GetPrice();
    }
}
