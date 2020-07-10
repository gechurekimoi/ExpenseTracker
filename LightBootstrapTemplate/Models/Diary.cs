using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LightBootstrapTemplate.Models
{
    public class Diary
    {
        public int Id { get; set; }
        public DateTime EntryDate { get; set; }
        public string Title { get; set; }
        public string Thoughts { get; set; }
    }
}