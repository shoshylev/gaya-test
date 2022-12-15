using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Gaya.Models
{
    public class History
    {
        public int Id { get; set; }
        [Required]
        public string FirstField { get; set; }
        [Required]
        public string SecondField { get; set; }
        public string Result { get; set; }
        public DateTime? ExecutionDate{ get; set; }

        // Foreign Key
        public int ProcessorId { get; set; }
        // Navigation property
        public Processor Processor { get; set; }

        public History()
        {
        }
    }
}