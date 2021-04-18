using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adonis.Models
{
    public class RoleAndSkill
    {
        public int Id { get; set; }
        public int IdRole { get; set; }
        public int IdSkill { get; set; }
        public int IdGrade { get; set; }
    }
}