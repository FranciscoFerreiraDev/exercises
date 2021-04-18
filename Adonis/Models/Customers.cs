using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adonis.Models
{
    public class Customers
    {
        public int IdCustomer { get; set; }
        public string ClientName { get; set; }
        public int NIF { get; set; }
        public string LegalRepresentative { get; set; }
        public string Adress { get; set; }
        public string Email { get; set; }
        public string FinancialOrProcurementDepartmentEmail { get; set; }
        public int Active { get; set; }
    }
}