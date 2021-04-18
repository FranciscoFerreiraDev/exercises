using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adonis.Models
{
    public class Contacts
    {
        public int IdContact { get; set; }
        public int IdIndividual { get; set; }
        public string Description { get; set; }
        public int IdContactType { get; set; }
    }
}