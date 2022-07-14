using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace PMB.Model.DTO.Ex4
{
    public class Ex4TokenModel
    {
        public IEnumerable<Cookie> Cookies { get; set; }
        public string CsrfToken { get; set; }
    }
}
