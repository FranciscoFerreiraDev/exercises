using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adonis.Models
{
    public class User
    {
        public int IdUser { get; set; }
        public int IdUserRole { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public int Activated { get; set; }
        public string WPCode { get; set; }
        public string TypeOfAccess { get; set; }
        public string UserRoleDescription { get; set; }
        public string WPAppDescription { get; set; }
    }
}