using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adonis.Models
{
    public class CandidateNotes
    {
        public int IdCandidateDescription { get; set; }
        public int IdCandidate { get; set; }
        public string CandidateName { get; set; }
        public string TimeStamp { get; set; }
        public string Note { get; set; }
        public int Active { get; set; }
        public int IdUser { get; set; }
        public string User { get; set; }
    }
}