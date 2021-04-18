using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adonis.Models
{
    public class UserUserRoles
    {
        public int IdUUR { get; set; }
        public int IdUser { get; set; }
        public int IdUserRole { get; set; }
        public string Description { get; set; }
        public int Active { get; set; }
    }
}