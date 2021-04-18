using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adonis.Models
{
    public class Hierarchy
    {
        public int IdHierarchy { get; set; }
        public int IdRespondTo { get; set; }//pai
        public int IdRespondsFrom { get; set; }//filho
    }
}