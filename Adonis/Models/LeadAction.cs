using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adonis.Models
{
    public class LeadAction
    {
        public int IdAction { get; set; }
        public string Description { get; set; }
        public int Active { get; set; }
    }
}