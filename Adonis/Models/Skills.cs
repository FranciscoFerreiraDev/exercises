using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adonis.Models
{
    public class Skills
    {
        public int IdSkill { get; set; }
        public string Name { get; set; }
        public int IdCategory { get; set; }
        public string NameCategory { get; set; }
        public int Active { get; set; }
    }
}