using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adonis.Models
{
    public class UserRoles
    {
        public int IdUserRole { get; set; }
        public string Description { get; set; }
        public int Active { get; set; }
        public string TypeOfAccess { get; set; }
        public string WPCode { get; set; }
    }
}