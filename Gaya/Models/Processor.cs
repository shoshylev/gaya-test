using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Gaya.Models
{
    public class Processor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        public string Action { get; set; }
        public bool FirstParameterAsString { get; set; }
        public bool SecondParameterAsString { get; set; }
    }
}