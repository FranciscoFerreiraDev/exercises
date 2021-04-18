using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adonis.Models
{
    public class LeadsCandidates
    {
        public int IdLeadCandidates { get; set; }
        public int IdLead { get; set; }
        public int IdCandidate { get; set; }
        public int IdCandidateLeadStatus { get; set; }
        public string CandidateLeadStatusDescription { get; set; }
    }
}