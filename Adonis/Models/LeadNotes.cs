using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adonis.Models
{
    public class LeadNotes
    {
        public int IdLeadNote { get; set; }
        public int IdLead { get; set; }
        public string Description { get; set; }
        public int IdUser { get; set; }
        public string User { get; set; }
        public string TimeStamp { get; set; }
        public int Active { get; set; }
    }
}