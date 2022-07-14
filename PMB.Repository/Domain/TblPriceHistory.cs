﻿using System;
using System.Collections.Generic;
using System.Text;

namespace PMB.Repository.Domain
{
    public class TblPriceHistory : BaseEntity
    {
        public double Ex4IrBuyPrice { get; set; } = 0;
        public double Ex4IrSellPrice { get; set; } = 0;
        public double Payfa24BuyPrice { get; set; } = 0;
        public double Payfa24SellPrice { get; set; } = 0;
        public double HdPayBuyPrice { get; set; } = 0;
        public double HdPaySellPrice { get; set; } = 0;
        public double IraniCardBuyPrice { get; set; } = 0;
        public double IraniCardSellPrice { get; set; } = 0;
        public double NobitexBuyPrice { get; set; } = 0;
        public double NobitexSellPrice { get; set; } = 0;
        public double AvvalMoneyBuyPrice { get; set; } = 0;
        public double AvvalMoneySellPrice { get; set; } = 0;
    }
}
