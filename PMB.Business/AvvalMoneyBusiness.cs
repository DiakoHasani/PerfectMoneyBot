using PMB.Model.DTO.AvvalMoney;
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
    public class AvvalMoneyBusiness : IAvvalMoneyBusiness
    {
        private readonly IErrorBusiness _errorBusiness;
        public AvvalMoneyBusiness(IErrorBusiness errorBusiness)
        {
            _errorBusiness = errorBusiness;
        }

        public async Task<ResultApiModel<AvvalMoneyApiModel>> GetPrice()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    ServicePointManager.Expect100Continue = true;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                    var response = await client.GetAsync("https://panel.avvalmoney.com/api/Price/GetPriceHome");

                    if (response.IsSuccessStatusCode)
                    {
                        return new ResultApiModel<AvvalMoneyApiModel>
                        {
                            Data = Newtonsoft.Json.JsonConvert.DeserializeObject<AvvalMoneyApiModel>(await response.Content.ReadAsStringAsync()),
                            Result = true,
                            Message = "success call avvalmoney api"
                        };
                    }
                    else
                    {
                        _errorBusiness.AddError(await response.Content.ReadAsStringAsync(), MethodBase.GetCurrentMethod().DeclaringType.FullName);
                        return new ResultApiModel<AvvalMoneyApiModel>
                        {
                            Result = false,
                            Message = "error in call avvalmoney api error message: " + await response.Content.ReadAsStringAsync()
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                _errorBusiness.AddError(ex, MethodBase.GetCurrentMethod().DeclaringType.FullName);
                return new ResultApiModel<AvvalMoneyApiModel>
                {
                    Result = false,
                    Message = "error in call avvalmoney api exception message: " + ex.Message
                };
            }
        }
    }
    public interface IAvvalMoneyBusiness
    {
        Task<ResultApiModel<AvvalMoneyApiModel>> GetPrice();
    }
}
