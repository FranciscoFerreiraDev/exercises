using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adonis.Models
{
    public class Nationalities
    {
        public int IdNationality { get; set; }
        public string NationalityDescription { get; set; }
        public int forCandidates { get; set; }
        public int forLeads { get; set; }
        public int Active { get; set; }
    }
}