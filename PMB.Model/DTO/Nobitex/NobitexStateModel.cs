using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace PMB.Model.DTO.Nobitex
{
    public class NobitexStateModel
    {
        [JsonProperty("usdt-rls")]
        public NobitexRlsModel USDT { get; set; }
    }
}
