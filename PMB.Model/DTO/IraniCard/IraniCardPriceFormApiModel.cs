using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace PMB.Model.DTO.IraniCard
{
    public class IraniCardPriceFormApiModel
    {
        [JsonProperty("related_currencies")]
        public List<IraniCardRelatedCurrencyApiModel> RelatedCurrencies { get; set; }
    }
}
