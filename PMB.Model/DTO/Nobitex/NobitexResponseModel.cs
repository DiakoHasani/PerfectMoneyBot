using System;
using System.Collections.Generic;
using System.Text;

namespace PMB.Model.DTO.Nobitex
{
    public class NobitexResponseModel
    {
        public bool ResultApi { get; set; }
        public string Message { get; set; }
        public string Status { get; set; }
        public NobitexStateModel Stats { get; set; }
    }
}
