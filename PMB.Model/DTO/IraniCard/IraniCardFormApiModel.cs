using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace PMB.Model.DTO.IraniCard
{
    public class IraniCardFormApiModel
    {
        [JsonProperty("price_form")]
        public IraniCardPriceFormApiModel PriceForm { get; set; }
    }
}
