using Adonis.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adonis.Models
{
    public class AppGeneral
    {
        private string AppVersion;
        private int safe = 0;
        private List<CompanyRoles> ListCompanyRoles = new List<CompanyRoles>();
        //private AdnDbController controller = new AdnDbController();
        private HtmlString Features;
        private HtmlString Warning;
        private List<string> Images = new List<string>();

        public AppGeneral()
        {
            AppVersion = "Adonis 0.2.9a";
            HtmlString feat = new HtmlString("<ul>" + "<li>Managements of Candidatesis up and running without known fault.</li>" +
                "<li>Management of Leads up and running without any known fault..</li>"+
                "<li>Log in is somewhat running. It logs in the User, but User can iteract with all functionalities without security check.</li>"+
                "<li>Changes done to candidates and Leads are logged and informed via email to the respectives departments email.</li>"+
                "<li>Candidates can be uploaded into the Adonis via .csv files.</li>"+
                "</ul>");
            Features = feat;

            HtmlString war = new HtmlString("<p> In case of any type of problem (interface being ugly, errors appearing, etc...), send an email with the details to <a href= 'mailto:ricolavo+vaeyjnped0m9xi6g5eso@boards.trello.com?Subject=Defect.' target= '_top' >Reloading Tech Trello Board</a>.</p>");

            Warning = war;
        }

        public string GetVersion()
        {
            return AppVersion;
        }

        public HtmlString GetFeatures()
        {
            return Features;
        }

        public HtmlString GetWarning()
        {
            return Warning;
        }
    }
}