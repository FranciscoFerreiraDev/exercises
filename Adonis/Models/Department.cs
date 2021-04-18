using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adonis.Models
{
    public class Department
    {
        public int IdDepartment { get; set; }
        public string DepName { get; set; }
        public int IdCompany { get; set; }
    }
}