using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adonis.Models
{
    public class CandidateFullInfo
    {
        public int IdCRnS { get; set; }
        public int IdCandidate { get; set; }
        public string Name { get; set; }
        public int IdRolesAndSkills { get; set; }
        public int IdRole { get; set; }
        public string Role { get; set; }
        public int IdSkill { get; set; }
        public string Skill { get; set; }
        public string SkillType { get; set; }
        public string DateStart { get; set; }
        public string DateFinish { get; set; }
        public string Description { get; set; }
        public string ExpDescription { get; set; }
        public string Grade { get; set; }
        public string GrossRemuneration { get; set; }
        public int IdAvailability { get; set; }
        public string Availability { get; set; }
        public int Classification { get; set; }
        public int Activated { get; set; }
        public int UserEvaluation { get; set; }
        public int ContactNumber { get; set; }
        public string Email { get; set; }
        /// <summary>
        /// ///////////////////////////////////////
        /// </summary>
        public string NET { get; set; }
        public string DailyGains { get; set; }
        public string RemunerationNotes { get; set; }
        public string CurrentPlace { get; set; }
        public int IdLocation { get; set; }
        public int IdStatus { get; set; }
        public string Status { get; set; }
        public string StatusColor { get; set; }
        public string Interview { get; set; }
        public string Candidacy { get; set; }
        public int IdCandidacy { get; set; }
        public int MainExperience { get; set; }
        public string CandidateCode { get; set; }
        public int IdNationality { get; set; }
        public string NationalityDescription { get; set; }
        public string BirthDate { get; set; }
        public string CandidateDescription { get; set; }
        public int IdLeadCandidateStatus { get; set; }
        public string LeadCandidateStatusDescription { get; set; }
    }
}