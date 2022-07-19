using Microsoft.Extensions.Configuration;
using PMB.Business;
using PMB.Model.DTO.AvvalMoney;
using PMB.Model.DTO.Ex4;
using PMB.Model.DTO.HdPay;
using PMB.Model.DTO.IraniCard;
using PMB.Model.DTO.Payfa24;
using PMB.Model.General;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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
        IConfigurationBuilder builder;
        IConfigurationRoot config;

        public GetPriceSchedule(IEx4IrBusiness ex4IrBusiness,
            IPriceHistoryBusiness priceHistoryBusiness,
            IHdPayBusiness hdPayBusiness,
            IPayfa24Business payfa24Business,
            IAvvalMoneyBusiness avvalMoneyBusiness,
            IIraniCardBusiness iraniCardBusiness)
        {
            _ex4IrBusiness = ex4IrBusiness;
            _priceHistoryBusiness = priceHistoryBusiness;
            _hdPayBusiness = hdPayBusiness;
            _payfa24Business = payfa24Business;
            _avvalMoneyBusiness = avvalMoneyBusiness;
            _iraniCardBusiness = iraniCardBusiness;
            builder = new ConfigurationBuilder().AddJsonFile($"appsettings.json", true, true);
            config = builder.Build();
        }

        private bool pause = false;
        private ResultApiModel<Ex4ApiModel> resultApiEx4;
        private ResultApiModel<HdPayApiModel> resultApiHdPay;
        private ResultApiModel<Payfa24ResultItemModel> resultApiPayfa;
        private ResultApiModel<AvvalMoneyApiModel> resultApiAvvalMoney;
        private ResultApiModel<IraniCardPriceModel> resultIraniCard;

        private Ex4ApiModel ex4ApiData;
        private HdPayApiModel hdPayData;
        private Payfa24ResultItemModel payfa24Data;
        private AvvalMoneyApiModel avvalMoneyData;
        private IraniCardPriceModel iraniCardData;

        public void Start()
        {
            new Timer(TimerCallback, null, 0, 60000);
        }

        public void TimerCallback(Object o)
        {
            if (pause)
            {
                return;
            }
            CallApis().GetAwaiter().GetResult();
        }

        public async Task CallApis()
        {
            ex4ApiData = await CallEx4Api();
            hdPayData = await CallHdPayApi();
            payfa24Data = await CallPayfa24Api();
            avvalMoneyData = await CallAvvalMoneyApi();
            iraniCardData = await CallIraniCardApi();
        }

        public async Task<Ex4ApiModel> CallEx4Api()
        {
            resultApiEx4 = await _ex4IrBusiness.GetPrice();
            Console.WriteLine(resultApiEx4.Message);
            if (resultApiEx4.Result)
            {
                return resultApiEx4.Data;
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
    }
    public interface IGetPriceSchedule
    {
        void Start();
        void TimerCallback(Object o);
        Task CallApis();
        Task<Ex4ApiModel> CallEx4Api();
        Task<HdPayApiModel> CallHdPayApi();
        Task<Payfa24ResultItemModel> CallPayfa24Api();
        Task<AvvalMoneyApiModel> CallAvvalMoneyApi();
        Task<IraniCardPriceModel> CallIraniCardApi();
    }
}
