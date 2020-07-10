using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LightBootstrapTemplate.Models
{
    public class Goal
    {
        public int Id { get; set; }
        public DateTime DateSet { get; set; }
        public string Description { get; set; }
        public string typeGoal { get; set; }
        public DateTime DateToBeAchieved { get; set; }

    }
}