using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adonis.Models
{
    public class LeadsProcessFlow
    {
        public int ProcessFlow { get; set; }
        public int FromProcess { get; set; }
        public int ToProcess { get; set; }
        public int IdAction { get; set; }
    }
}