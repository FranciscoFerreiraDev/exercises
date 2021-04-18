using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adonis.Models
{
    public class Leads
    {
        public int IdLead { get; set; }
        public int IdCustomer { get; set; }
        public int IdLeadStatus { get; set; }
        public string LeadCode { get; set; }
        public string Description { get; set; }
        public int IdSkill { get; internal set; }
        public int SkillExp { get; internal set; }
        public int IdRole { get; set; }
        public int RoleExp { get; set; }
        public int MinProfExp { get; internal set; }
        public int MaxProfExp { get; internal set; }
        public int MinRemuneartion { get; internal set; }
        public int MaxRemuneartion { get; internal set; }
        public int MinAge { get; internal set; }
        public int MaxAge { get; internal set; }
        public int MaxAvailability { get; internal set; }

        public string ClientName { get; set; }
        public int NIF { get; set; }
        public string LegalRepresentative { get; set; }
        public string Adress { get; set; }
        public string Email { get; set; }
        public string Financial_ProcurementDepartmentEmail { get; set; }

        public int IdAvailability { get; set; }
        public string AvailabilityDescription { get; set; }

        public int IdNationality { get; internal set; }
        public string NationalityDescription { get; set; }

        public string LeadStatusDescription { get; set; }
        public string LeadStatusColor { get; set; }

        public string NameSkill { get; set; }
        public string NameRole { get; set; }
    }
}