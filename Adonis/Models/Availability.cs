using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adonis.Models
{
    public class Availability
    {
        public int IdAvailability { get; set; }
        public string Description { get; set; }
        public int Activated { get; set; }
    }
}