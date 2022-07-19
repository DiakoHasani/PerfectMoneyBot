using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace PMB.Model.DTO.IraniCard
{
    public class IraniCardProductApiModel
    {
        [JsonProperty("form")]
        public IraniCardFormApiModel Form { get; set; }
    }
}
