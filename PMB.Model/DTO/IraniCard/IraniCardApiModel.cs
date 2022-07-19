using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace PMB.Model.DTO.IraniCard
{
    public class IraniCardApiModel
    {
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("status_code")]
        public int StatusCode { get; set; }

        [JsonProperty("data")]
        public IraniCardDataApiModel Data { get; set; }
    }
}
