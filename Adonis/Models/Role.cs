using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adonis.Models
{
    public class Role
    {
        public int IdRole { get; set; }
        public string NameRole { get; set; }
        public int Active { get; set; }
    }
}