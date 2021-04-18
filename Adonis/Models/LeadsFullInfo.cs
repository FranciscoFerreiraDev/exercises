using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adonis.Models
{
    public class LeadsFullInfo
    {
        public int IdLead { get; set; }
        public string Adress{ get; set; }
        public string LeadCode { get; set; }
        public int IdCustomer { get; set; }
        public int IdLeadStatus { get; set; }
        public string ClientName { get; set; }
        public int IdAction { get; set;  }
        public string ActionDescription { get; set; }
        public string LeadStatusDescription { get; set; }
        public string LegalRepresentative { get; set; }
        public string Financial_ProcurementDepartmentEmail { get; set; }
        public int NIF { get; set; }
        public int IdAvailability { get; set; }
        public string AvailabilityDescription { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
        public int SkillExp { get; set; }
        public int IdSkill { get; set; }
        public int IdRole { get; set; }
        public int RoleExp { get; set; }
        public int IdNationality { get; set; }
        public string NationalityDescription { get; set; }
        public int MinProfessionalExp { get; internal set; }
        public int MaxProfessionalExp { get; internal set; }
        public int MaxRemuneration { get; set; }
        public int MinRemuneration { get; set; }
        public string LeadStatusColor { get; set; }
        public string NameRole { get; set; }
        public string NameSkill { get; set; }
        public int Active { get; set; }
        public int MinAge { get; set; }
        public int MaxAge { get; set; }
        public List<CandidateFullInfo> CandidateList { get; set; }
    }
}