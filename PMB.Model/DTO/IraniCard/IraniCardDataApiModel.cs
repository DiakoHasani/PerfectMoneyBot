using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace PMB.Model.DTO.IraniCard
{
    public class IraniCardDataApiModel
    {
        [JsonProperty("product")]
        public IraniCardProductApiModel Product { get; set; }
    }
}
