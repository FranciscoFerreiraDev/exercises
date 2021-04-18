using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adonis.Models
{
    public class WorkPackage
    {
        public int IdWP { get; set; }
        public string WPCode { get; set; }
        public string Description { get; set; }
        public int Active { get; set; }
        public string WPEmail { get; set; }
    }
}