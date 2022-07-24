using Microsoft.Extensions.Configuration;
using PMB.Services.Business;
using PMB.Model.DTO.AvvalMoney;
using PMB.Model.DTO.Ex4;
using PMB.Model.DTO.HdPay;
using PMB.Model.DTO.IraniCard;
using PMB.Model.DTO.Nobitex;
using PMB.Model.DTO.Payfa24;
using PMB.Model.General;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace PMB.Bot.Schedules
{
    public class GetPriceSchedule : IGetPriceSchedule
    {
        private readonly IEx4IrBusiness _ex4IrBusiness;
        private readonly IPriceHistoryBusiness _priceHistoryBusiness;
        private readonly IHdPayBusiness _hdPayBusiness;
        private readonly IPayfa24Business _payfa24Business;
        private readonly IAvvalMoneyBusiness _avvalMoneyBusiness;
        private readonly IIraniCardBusiness _iraniCardBusiness;
        private readonly INobitexBusiness _nobitexBusiness;
        private readonly IErrorBusiness _errorBusiness;
        IConfigurationBuilder builder;
        IConfigurationRoot config;

        public GetPriceSchedule(IEx4IrBusiness ex4IrBusiness,
            IPriceHistoryBusiness priceHistoryBusiness,
            IHdPayBusiness hdPayBusiness,
            IPayfa24Business payfa24Business,
            IAvvalMoneyBusiness avvalMoneyBusiness,
            IIraniCardBusiness iraniCardBusiness,
            INobitexBusiness nobitexBusiness,
            IErrorBusiness errorBusiness)
        {
            _ex4IrBusiness = ex4IrBusiness;
            _priceHistoryBusiness = priceHistoryBusiness;
            _hdPayBusiness = hdPayBusiness;
            _payfa24Business = payfa24Business;
            _avvalMoneyBusiness = avvalMoneyBusiness;
            _iraniCardBusiness = iraniCardBusiness;
            _nobitexBusiness = nobitexBusiness;
            _errorBusiness = errorBusiness;
            builder = new ConfigurationBuilder().AddJsonFile($"appsettings.json", true, true);
            config = builder.Build();
        }

        private bool pause = false;
        private MessageModel resultAddPriceToDatabase;

        private ResultApiModel<Ex4ApiModel> resultApiEx4;
        private ResultApiModel<HdPayApiModel> resultApiHdPay;
        private ResultApiModel<Payfa24ResultItemModel> resultApiPayfa;
        private ResultApiModel<AvvalMoneyApiModel> resultApiAvvalMoney;
        private ResultApiModel<IraniCardPriceModel> resultIraniCard;
        private ResultApiModel<NobitexPriceModel> resultNobitex;

        private Ex4ApiModel ex4ApiData;
        private HdPayApiModel hdPayData;
        private Payfa24ResultItemModel payfa24Data;
        private AvvalMoneyApiModel avvalMoneyData;
        private IraniCardPriceModel iraniCardData;
        private NobitexPriceModel nobitexData;

        public async Task Start()
        {
            while (true)
            {
                if (!pause)
                {
                    try
                    {
                        pause = true;
                        await CallApis();
                        await AddPriceToDataBase();
                        ShowPrices();
                        ClearData();
                        Thread.Sleep(600000);
                    }
                    catch (Exception ex)
                    {
                        _errorBusiness.AddError(ex, MethodBase.GetCurrentMethod().DeclaringType.FullName);
                        Console.WriteLine("error in main start excption message: " + ex.Message);
                    }
                    finally
                    {
                        pause = false;
                        Console.WriteLine();
                        Console.WriteLine("----------------------------------------------------------");
                        Console.WriteLine();
                    }
                }
            }
        }

        public async Task CallApis()
        {
            ex4ApiData = await CallEx4Api();
            hdPayData = await CallHdPayApi();
            payfa24Data = await CallPayfa24Api();
            avvalMoneyData = await CallAvvalMoneyApi();
            iraniCardData = await CallIraniCardApi();
            nobitexData = await CallNobitex();
        }

        public async Task<Ex4ApiModel> CallEx4Api()
        {
            resultApiEx4 = await _ex4IrBusiness.GetPrice();
            if (resultApiEx4.Result)
            {
                Console.WriteLine(resultApiEx4.Message);
                return resultApiEx4.Data;
            }
            else
            {
                Console.WriteLine(resultApiEx4.Message);
            }
            return null;
        }

        public async Task<HdPayApiModel> CallHdPayApi()
        {
            resultApiHdPay = await _hdPayBusiness.GetPrice();
            Console.WriteLine(resultApiHdPay.Message);
            if (resultApiHdPay.Result)
            {
                return resultApiHdPay.Data;
            }
            return null;
        }

        public async Task<Payfa24ResultItemModel> CallPayfa24Api()
        {
            resultApiPayfa = await _payfa24Business.GetPrice(new Payfa24CookieModel
            {
                Session = config["Payfa24:Session"],
                XSRFTOKEN = config["Payfa24:XSRFTOKEN"]
            });

            Console.WriteLine(resultApiPayfa.Message);
            if (resultApiPayfa.Result)
            {
                return resultApiPayfa.Data;
            }
            return null;
        }

        public async Task<AvvalMoneyApiModel> CallAvvalMoneyApi()
        {
            resultApiAvvalMoney = await _avvalMoneyBusiness.GetPrice();
            Console.WriteLine(resultApiAvvalMoney.Message);
            if (resultApiAvvalMoney.Result)
            {
                return resultApiAvvalMoney.Data;
            }
            return null;
        }

        public async Task<IraniCardPriceModel> CallIraniCardApi()
        {
            resultIraniCard = await _iraniCardBusiness.GetPrice(new IraniCardBodyLoginModel
            {
                Mobile = config["IraniCard:UserName"],
                Password = config["IraniCard:Password"]
            });
            Console.WriteLine(resultIraniCard.Message);
            if (resultIraniCard.Result)
            {
                return resultIraniCard.Data;
            }
            return null;
        }

        public async Task<NobitexPriceModel> CallNobitex()
        {
            resultNobitex = await _nobitexBusiness.GetPrice();
            Console.WriteLine(resultNobitex.Message);
            if (resultNobitex.Result)
            {
                return resultNobitex.Data;
            }
            return null;
        }

        public async Task AddPriceToDataBase()
        {
            resultAddPriceToDatabase = await _priceHistoryBusiness.Add(new Model.DTO.PriceHistory.AddPriceHistoryModel
            {
                AvvalMoneyBuyPrice = avvalMoneyData != null ? avvalMoneyData.Data.FirstOrDefault(a => a.Type == "Buy").Rials : 0,
                AvvalMoneySellPrice = avvalMoneyData != null ? avvalMoneyData.Data.FirstOrDefault(a => a.Type == "Sell").Rials : 0,
                Ex4IrBuyPrice = ex4ApiData != null ? Convert.ToDouble(ex4ApiData.Buy_Price, System.Globalization.CultureInfo.InvariantCulture) : 0,
                Ex4IrSellPrice = ex4ApiData != null ? Convert.ToDouble(ex4ApiData.Sell_Price, System.Globalization.CultureInfo.InvariantCulture) : 0,
                HdPayBuyPrice = hdPayData != null ? hdPayData.BuyPrice : 0,
                HdPaySellPrice = hdPayData != null ? hdPayData.SellPrice : 0,
                IraniCardBuyPrice = iraniCardData != null ? iraniCardData.BuyPrice : 0,
                IraniCardSellPrice = iraniCardData != null ? iraniCardData.SellPrice : 0,
                NobitexBuyPrice = nobitexData != null ? nobitexData.Price : 0,
                NobitexSellPrice = nobitexData != null ? nobitexData.Price : 0,
                Payfa24BuyPrice = payfa24Data != null ? Convert.ToDouble(payfa24Data.Fee_Buy, System.Globalization.CultureInfo.InvariantCulture) : 0,
                Payfa24SellPrice = payfa24Data != null ? Convert.ToDouble(payfa24Data.Fee_Sell, System.Globalization.CultureInfo.InvariantCulture) : 0
            });
            Console.WriteLine(resultAddPriceToDatabase.Message);
        }

        public void ShowPrices()
        {
            try
            {
                if (ex4ApiData != null)
                {
                    Console.WriteLine($"Ex4         BuyPrice: {Convert.ToDouble(ex4ApiData.Buy_Price).ToString("N0")} - SellPrice: {Convert.ToDouble(ex4ApiData.Sell_Price).ToString("N0")}");
                }

                if (hdPayData != null)
                {
                    Console.WriteLine($"HdPay       BuyPrice: {hdPayData.BuyPrice.ToString("N0")} - SellPrice: {hdPayData.SellPrice.ToString("N0")}");
                }

                if (payfa24Data != null)
                {
                    Console.WriteLine($"Payfa       BuyPrice: {payfa24Data.Fee_Buy} - SellPrice: {payfa24Data.Fee_Sell}");
                }

                if (avvalMoneyData != null)
                {
                    Console.WriteLine($"AvvalMoney  BuyPrice: {avvalMoneyData.Data.FirstOrDefault(a => a.Type == "Buy").Rials.ToString("N0")} - SellPrice: {avvalMoneyData.Data.FirstOrDefault(a => a.Type == "Sell").Rials.ToString("N0")}");
                }

                if (iraniCardData != null)
                {
                    Console.WriteLine($"IraniCard   BuyPrice: {iraniCardData.BuyPrice.ToString("N0")} - SellPrice: {iraniCardData.SellPrice.ToString("N0")}");
                }

                if (nobitexData != null)
                {
                    Console.WriteLine($"Nobitex     ___Price: {nobitexData.Price.ToString("N0")}");
                }
            }
            catch (Exception ex)
            {
                _errorBusiness.AddError(ex, MethodBase.GetCurrentMethod().DeclaringType.FullName);
                Console.WriteLine("error in ShowPrices exception message: " + ex.Message);
            }
        }

        public void ClearData()
        {
            ex4ApiData = null;
            hdPayData = null;
            payfa24Data = null;
            avvalMoneyData = null;
            iraniCardData = null;
            nobitexData = null;
        }
    }
    public interface IGetPriceSchedule
    {
        Task Start();
        Task CallApis();
        Task AddPriceToDataBase();
        Task<Ex4ApiModel> CallEx4Api();
        Task<HdPayApiModel> CallHdPayApi();
        Task<Payfa24ResultItemModel> CallPayfa24Api();
        Task<AvvalMoneyApiModel> CallAvvalMoneyApi();
        Task<IraniCardPriceModel> CallIraniCardApi();
        Task<NobitexPriceModel> CallNobitex();
        void ShowPrices();
        void ClearData();
    }
}
