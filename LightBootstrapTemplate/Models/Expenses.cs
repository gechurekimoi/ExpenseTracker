using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LightBootstrapTemplate.Models
{
    public class Expenses
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime DateSpent { get; set; }
        public int CategoryId { get; set; }
        public double Amount { get; set; }

        public Category Category { get; set; }
    }
}