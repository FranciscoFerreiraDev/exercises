using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adonis.Models
{
    public class CandidateAndRoleAndSkill
    {
        public int IdCRNS { get; set; }
        public int IdCandidate { get; set; }
        public string Name { get; set; }
        public int IdRolesAndSkills { get; set; }
        public string DateStart { get; set; }
        public string DateFinish { get; set; }
        public string Description { get; set; }
        public int MainExperience { get; set; }
    }
}