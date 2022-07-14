using System;
using System.Collections.Generic;
using System.Text;

namespace PMB.Model.General
{
    public class ResultApiModel<T>
    {
        public bool Result { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
    }
}
