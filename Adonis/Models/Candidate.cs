using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adonis.Models
{
    public class Candidate
    {
        public int IdCandidate { get; set; }
        public string Name { get; set; }
        public int Activated { get; set; }
        public int GrossRemuneration { get; set; }
        public int Availability { get; set; }
        public int NET { get; set; }
        public int DailyGains { get; set; }
        public string RemunerationNotes { get; set; }
        public int CurrentPlace { get; set; }
        public int IdAvailability { get; set; }
        public string Interview { get; set; }
        public string Candidacy { get; set; }
        public int IdClassification { get; set; }
        public int IdStatus { get; set; }
        public string CandidateCode { get; set; }
        public int status { get; set; }
        public int IdNationality { get; set; }
        public string NationalityDescription { get; set; }
        public string BirthDate { get; set; }
        public string CandidaqteDescription { get; set; }
        public string Description { get; set; }
        public string CurrentPlaceDescription { get; set; }
        public string AvailabilityDescription { get; set; }
        public string CandidacyDescription { get; set; }
        public string CandidateStatusDescription { get; set; }
        public int ContactNumber { get; set; }
        public string Email { get; set; }
    }
}