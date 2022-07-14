using System;
using System.Collections.Generic;
using System.Text;

namespace PMB.Repository.Domain
{
    public class TblError : BaseEntity
    {
        public string Message { get; set; }

        public string InnerException { get; set; }

        public string FileName { get; set; }

        public string MethodName { get; set; }

        public int Line { get; set; }

        public int Col { get; set; }

        public string StackTrace { get; set; }
    }
}
