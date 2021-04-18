using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adonis.Models
{
    public class Location
    {
        public int IdLocation { get; set; }
        public string Description { get; set; }
        public int Activated { get; set; }
    }
}