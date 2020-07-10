using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LightBootstrapTemplate.Models
{
    public class Income
    {
        public int Id { get; set; }
        public DateTime DateReceived { get; set; }
        public double Amount { get; set; }
    }
}