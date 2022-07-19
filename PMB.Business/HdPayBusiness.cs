using PMB.Model.DTO.HdPay;
using PMB.Model.General;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PMB.Business
{
    public class HdPayBusiness : IHdPayBusiness
    {
        private readonly IErrorBusiness _errorBusiness;
        public HdPayBusiness(IErrorBusiness errorBusiness)
        {
            _errorBusiness = errorBusiness;
        }

        public async Task<string> GetBuyPrice()
        {
            using (var client = new HttpClient())
            {
                try
                {
                    ServicePointManager.Expect100Continue = true;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    var response = await client.GetAsync("https://hdpay.ir/ajax/exchange/rate?c1=1&c2=8");
                    if (response.IsSuccessStatusCode)
                    {
                        return (await response.Content.ReadAsStringAsync()).Split("</b>")[1].Split('=')[0].Replace("TOMAN", "").Trim().Replace(",", "");
                    }
                    else
                    {
                        _errorBusiness.AddError(await response.Content.ReadAsStringAsync(), MethodBase.GetCurrentMethod().DeclaringType.FullName);
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    _errorBusiness.AddError(ex, MethodBase.GetCurrentMethod().DeclaringType.FullName);
                    return null;
                }
            }
        }

        public async Task<ResultApiModel<HdPayApiModel>> GetPrice()
        {
            var buyPrice = await GetBuyPrice();
            var sellPrice = await GetSellPrice();

            if (string.IsNullOrWhiteSpace(buyPrice) || string.IsNullOrWhiteSpace(sellPrice))
            {
                return new ResultApiModel<HdPayApiModel>
                {
                    Result = false,
                    Message = "error in call hdpay api"
                };
            }
            return new ResultApiModel<HdPayApiModel>
            {
                Result = true,
                Message = "success call api hdpay",
                Data = new HdPayApiModel
                {
                    BuyPrice = Convert.ToDouble(buyPrice),
                    SellPrice = Convert.ToDouble(sellPrice)
                }
            };
        }

        public async Task<string> GetSellPrice()
        {
            using (var client = new HttpClient())
            {
                try
                {
                    ServicePointManager.Expect100Continue = true;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    var response = await client.GetAsync("https://hdpay.ir/ajax/exchange/rate?c1=8&c2=1");
                    if (response.IsSuccessStatusCode)
                    {
                        return (await response.Content.ReadAsStringAsync()).Split("=")[1].Replace("TOMAN", "").Trim();
                    }
                    else
                    {
                        _errorBusiness.AddError(await response.Content.ReadAsStringAsync(), MethodBase.GetCurrentMethod().DeclaringType.FullName);
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    _errorBusiness.AddError(ex, MethodBase.GetCurrentMethod().DeclaringType.FullName);
                    return null;
                }
            }
        }
    }
    public interface IHdPayBusiness
    {
        Task<ResultApiModel<HdPayApiModel>> GetPrice();
        Task<string> GetBuyPrice();
        Task<string> GetSellPrice();
    }
}
