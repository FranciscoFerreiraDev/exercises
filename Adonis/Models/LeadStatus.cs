using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adonis.Models
{
    public class LeadStatus
    {
        public int IdLeadStatus { get; set; }
        public string Description { get; set; }
        public string Color { get; set; }
        public int Active { get; set; }
    }
}