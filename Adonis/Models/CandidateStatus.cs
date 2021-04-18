using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adonis.Models
{
    public class CandidateStatus
    {
        public int IdStatus { get; set; }
        public string Description { get; set; }
        public int Activated { get; set; }
        public string Color { get; set; }
    }
}