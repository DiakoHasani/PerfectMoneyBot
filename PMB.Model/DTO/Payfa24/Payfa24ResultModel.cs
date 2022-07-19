using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace PMB.Model.DTO.Payfa24
{
    public class Payfa24ResultModel
    {
        [JsonProperty("1")]
        public Payfa24ResultItemModel PerfectMoney { get; set; }
    }
}
