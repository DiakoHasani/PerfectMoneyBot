using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PMB.Repository.Domain
{
    public class BaseEntity
    {
        [Key]
        public int Id { get; set; }

        public DateTime CreteDate { get; set; } = DateTime.Now;
    }
}
