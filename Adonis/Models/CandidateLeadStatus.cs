using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adonis.Models
{
    public class CandidateLeadStatus
    {
        public int IdCandidateLeadStatus { get; set; }
        public string Description { get; set; }
        public int Active { get; set; }
    }
}