using Adonis.Models;
using MimeKit.Cryptography;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net.Mail;
//using Microsoft.Office.Interop.Excel;
//using _Excel = Microsoft.Office.Interop.Excel;
//using System.Web.Mail;

namespace Adonis.Controllers
{
    public class AdnDbController
    {
        private ContextModel context = new ContextModel();
        private List<WorkPackage> WPList = new List<WorkPackage>();

        public AdnDbController()
        {
            WPList = context.GetListWP();
        }

        //It asks for a list of Candidates from the model context
        public List<CandidateFullInfo> GetListCandidateFullRS()
        {
            context = new ContextModel();
            var result = context.TestQueryCandidate();
            return result;
        }

        //It asks for a list of Roles
        public List<Role> GetListRoles()
        {
            context = new ContextModel();
            List<Role> result = context.QueryRoles();
            return result;
        }

        //It creates a new Role
        public void CreateRole(Role target, User logged)
        {
            try
            {
                context = new ContextModel();

                //context.ChangeState(true);
                context.CreateRole(target);
                context.CloseDBConnections();
            }
            catch (Exception ex)
            {
                //throw ex;
                ActionLOGGING(logged.Name,logged.IdUser, "R&S-US.004","Roles","Create","",target.NameRole+","+target.Active, new Exception("Create Role Failed:-:"+ex.Message));
            }
        }

        //It updates a Role
        public void EditRole(Role update, User logged)
        {
            try
            {
                context = new ContextModel();
                var old = context.GetRole(update.IdRole);
                
                context.UpdateRole(update);
                ActionLOGGING(logged.Name, logged.IdUser, "R&S-US.004", "Roles", "Update", old.NameRole+","+old.Active.ToString(),update.NameRole + "," + update.Active.ToString(), new Exception("Edit Role Succeeded"));
            }
            catch (Exception ex)
            {
                ActionLOGGING(logged.Name, logged.IdUser, "R&S-US.004", "Roles", "Update", "", update.NameRole + "," + update.Active, new Exception("Edit Role Failed:-:" + ex.Message));
            }
            context.CloseDBConnections();
        }

        //It asks for a list of Skills
        public List<Skills> GetListSkills()
        {
            context = new ContextModel();
            List<Skills> result = context.QuerySkills();

            return result;
        }

        //It creates a new Skill and relates it to a Category
        public void CreateSkill(string newSkill, int selectedCategory)
        {
            try
            {
                Skills skill = new Skills()
                {
                    Name = newSkill,
                    IdCategory = selectedCategory,
                };

                context = new ContextModel();

                context.CreateSkill(skill);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //It edits a Skill
        public void EditSkill(Skills update)
        {
            try
            {
                context = new ContextModel();
                CheckIfCreated();
                var old = context.QuerySkills();

                foreach (var item in old)
                {
                    if (item.IdSkill == update.IdSkill && (item.Name != update.Name || item.Active != update.Active || item.IdCategory != update.IdCategory))
                    {
                        context = new ContextModel();
                        context.ChangeState(true);
                        context.UpdateSkill(update);
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //It asks for a list of RolesAndSkill
        public List<RoleAndSkill> GetListRolesAndSkills()
        {
            context.CreateConnection();
            context.ChangeState(true);
            List<RoleAndSkill> result = new List<RoleAndSkill>(); ;
            try
            {
                if (context.GetConnectStringState() == false)
                    context = new ContextModel();

                result = context.QueryRolesAndSkills();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        //It creates a new relationship between a role and a skill
        public Boolean CreateRoleAndSkillSingle(RoleAndSkill target)
        {
            try
            {
                //if (context.GetConnectStringState() == false)
                    context = new ContextModel();

                context.ChangeState(true);
                context.CreateRollAndSkill(target);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }

        //It creates new relationships between a role and skills
        public Boolean CreateRoleAndSkill(List<RoleAndSkill> target)
        {
            try
            {
                if (context.GetConnectStringState() == false)
                    context = new ContextModel();

                foreach (var item in target)
                {
                    context.ChangeState(true);
                    context.CreateRollAndSkill(item);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }

        //It gets a certain RoleAndSkill
        public RoleAndSkill GetRoleAndSKill(int id)
        {
            RoleAndSkill result = context.GetRoleAndSkill(id);

            return result;
        }

        //It gets the Last RoleAndSkill
        public int GetLastRoleAndSKillId()
        {
            context = new ContextModel();
            int result = context.SelectLastRnSId();

            return result;
        }

        public void EditRoleAndSkill(int IdRnS, int IdRole, int IdSkill, int IdGrade)
        {
            try
            {
                RoleAndSkill Update = new RoleAndSkill()
                {
                    Id = IdRnS,
                    IdRole = IdRole,
                    IdSkill = IdSkill,
                    IdGrade = IdGrade,
                };

                context = new ContextModel();

                context.UpdateRoleAndSkill(Update);
                context.CloseDBConnections();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        //It Edits a CandidateRoleAndSkill
        public void EditCandidateRoleAndSkill(int IdCRNS, int IdCandidate, string start, string finish, string DescERNS, int MainEXPRNS, string wp, string user, string textSkill, string textRole, string textGrade, string candidateName)
        {
            try
            {
                CandidateAndRoleAndSkill Update = new CandidateAndRoleAndSkill()
                {
                    IdCRNS = IdCRNS,
                    IdCandidate = IdCandidate,
                    DateStart = start,
                    DateFinish = finish,
                    Description = DescERNS,
                    MainExperience = MainEXPRNS,
                };
                if(MainEXPRNS == 1)
                {
                    context = new ContextModel();
                    context.RevokeMainExp(Update.IdCandidate);
                    context.CloseDBConnections();
                }
                context = new ContextModel();
                context.UpdateCandidateRoleAndSkill(Update);
                context.CloseDBConnections();
                
                string Message = "An experience has been update to " + candidateName + ".<br />Author: " + user + "<br />  TimeStamp: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "<br />" +
                    "<table border='1'><tr>" + EmailExperienceHeader() + "</tr><tr><td>" + textRole + "</td><td>" + textSkill + "</td><td>" + Update.DateStart + "</td><td>" + Update.DateFinish + "</td><td>" + textGrade
                    + "</td><td>";

                if (Update.MainExperience == 1)
                    Message += "Yes";
                else
                    Message += "No";

                Message += "</td></tr><tr><th colspan='6'>Description</th></tr><tr><td colspan='6'>"+Update.Description+"</td></tr></table>";

                EmailSendNotes(wp, "Update", Message);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        //It asks for a list of Categories
        public List<SkillCategory> GetCategoriesList()
        {
            try
            {
                context = new ContextModel();

                List<SkillCategory> list = context.QueryCategories();
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //It creates a new Category of Skills
        public void CreateCategory(string newCategory)
        {
            try
            {
                if (context.GetConnectStringState() == false)
                    context = new ContextModel();

                context.CreateCategory(newCategory);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Edits a Category of Skills
        public void EditCategory(SkillCategory update)
        {
            try
            {
                CheckIfCreated();
                var old = context.QueryCategories();

                foreach (var item in old)
                {
                    if (item.IdCategory == update.IdCategory && (item.NameCategory != update.NameCategory || item.Activated != update.Activated))
                    {
                        context.ChangeState(true);
                        context.UpdateCategory(update);
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //It creates a new Candidate
        public void CreateCandidate(Candidate newCandidate)
        {
            try
            {
                context = new ContextModel();
                context.CreateCandidate(newCandidate);
                context.CloseDBConnections();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CandidateNotes> GetCandidateNotes()
        {
            try
            {
                context = new ContextModel();
                return context.GetListCandidateNotes();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void AddCandidateNote(CandidateNotes note, string user, string currentPage)
        {
            try
            {
                note.TimeStamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                context = new ContextModel();
                context.CreateCandidateNote(note);
                context.CloseDBConnections();

                EmailSendNotes("Candidates", "Create" , "<b>time:</b> " +note.TimeStamp + " <br /><b>By:</b> " + user + " <br /> <b>Candidate:</b> " + note.CandidateName + "<br /><b>Change:</b><br/>" + note.Note);
            }
            catch(Exception e)
            {
                EmailSendNotes("ERROR", "Delete", "<b>time:</b> " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " <br /><b>By:</b> " + user + " <br /> <b>Candidate:</b> " + note.CandidateName + "<br />" +
                    "<b>Old Note:</b> <br />" + note.Note);
                throw e;
            }
        }

        public void EditCandidateNote(CandidateNotes note, string oldNote, string user, string currentPage)
        {
            try
            {
                note.TimeStamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                context = new ContextModel();
                context.EditCandidateNote(note);
                context.CloseDBConnections();

                EmailSendNotes("Candidates", "Update", "<b>time:</b> " + note.TimeStamp + " <br /><b>By:</b> " + user + " <br /> <b>Candidate:</b> " + note.CandidateName + "<br />"+
                    "<b>Old Note:</b> <br />"+oldNote+"<br /><b>Change:</b><br/>" + note.Note);
            }
            catch (Exception e)
            {
                EmailSendNotes("ERROR", "Delete", "<b>time:</b> " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " <br /><b>By:</b> " + user + " <br /> <b>Candidate:</b> " + note.CandidateName + "<br />" +
                    "<b>Old Note:</b> <br />" + oldNote + "<br /><b>Change:</b> Note Deleted");
                throw e;
            }
        }

        public void DeleteCandidateNote(int target, string CandidateName, string OldNote, string user, string currentPage)
        {
            try
            {
                context = new ContextModel();
                context.DeleteCandidateNote(target);
                context.CloseDBConnections();

                EmailSendNotes("Candidates", "Delete", "<b>time:</b> " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " <br /><b>By:</b> " + user + " <br /> <b>Candidate:</b> " + CandidateName + "<br />" +
                    "<b>Old Note:</b> <br />" + OldNote + "<br /><b>Change:</b> Note Deleted");
            }
            catch (Exception e)
            {
                EmailSendNotes("ERROR", "Delete", "<b>time:</b> " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " <br /><b>By:</b> " + user + " <br /> <b>Candidate:</b> " + CandidateName + "<br />" +
                    "<b>Old Note:</b> <br />" + OldNote + "<br /><b>Change:</b> Note Deleted");
                throw e;
            }
        }

        public void CreateCandidateDescription(CandidateNotes description)
        {
            try
            {
                context = new ContextModel();

                context.CloseDBConnections();
            }
            catch (Exception e)
            {
                throw e;
            }
        }



        //It asks for a list of Candidates
        public List<Candidate> GetListCandidate()
        {
            List<Candidate> listCandidate = new List<Candidate>();

            try
            {
                if (context.GetConnectStringState() == false)
                    context = new ContextModel();

                listCandidate = context.QueryCandidate();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return listCandidate;
        }

        //It Gets a specified Candidate
        public Candidate GetCandidate(int id)
        {
            Candidate result = context.GetCandidate(id);

            return result;
        }

        //It Edits a Candidate, CandidateAndRollAndSkill, RollAndSkill
        public void EditCandidate(User loggedUser, string page, Candidate CandidateEdit, CandidateAndRoleAndSkill CandidteRnSEdit, RoleAndSkill RnSEdit, CandidateNotes descriptionEdit)
        {

            try
            {
                context = new ContextModel();
                Candidate oldCandidate = GetCandidate(CandidateEdit.IdCandidate);
                if (!CheckIfCandidateEqual(oldCandidate, CandidateEdit))
                    context.UpdateCandidate(CandidateEdit);


            context = new ContextModel();
            CandidateAndRoleAndSkill oldCRS = GetCandidateRS(CandidteRnSEdit.IdCRNS);
                        
                context = new ContextModel();
                if (!CheckIfCandidateAndRollAndSkillEqual(oldCRS, CandidteRnSEdit))
                    context.UpdateCRnS(CandidteRnSEdit);
            
            

            context = new ContextModel();
            RoleAndSkill oldRS = GetRoleAndSKill(RnSEdit.Id);
            
                context = new ContextModel();
                if (!CheckIfRollAndSkillEqual(oldRS, RnSEdit))
                    context.UpdateRoleAndSkill(RnSEdit);

                context = new ContextModel();
                context.UpdateDescription(descriptionEdit);

            context.CloseDBConnections();

            ActionLOGGING(loggedUser.Name, loggedUser.IdUser, "R&S-US.002", page, "Update", "", string.Concat(CandidateEdit.Name, ",", CandidateEdit.CandidateCode, ",", CandidateEdit.GrossRemuneration, ",", CandidateEdit.NET
                    , ",", CandidateEdit.DailyGains, ",", CandidateEdit.RemunerationNotes, ",", CandidateEdit.status, ",", CandidateEdit.Interview, ",", CandidateEdit.CurrentPlace, ",", CandidateEdit.Candidacy
                    , ",", CandidateEdit.Availability, ",", CandidateEdit.Activated), new Exception("Editing RollAndSkill Succeeded"));
            return;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        //It Edits ONLY the Candidate
        public void EditCandidateONLY(Candidate update, string currentPage, string user)
        {
            try
            {
                context = new ContextModel();
                Candidate old = context.GetCandidate(update.IdCandidate);
                CandidateNotes desc = new CandidateNotes()
                {
                    IdCandidate = update.IdCandidate,
                    Note = update.Description,
                    TimeStamp = DateTime.Now.ToString("yyyy/mm/dd hh:mm:ss"),
                };
                
                context.UpdateCandidate(update);
                context.UpdateDescription(desc);
                context.CloseDBConnections();

                EmailSendNotes(currentPage, "Update", "A Candidate has been Updated!<br /><table border=1><tr>" + EmailCandidateTableHeader(1) +
                    "</tr><tr>" + "<td>" + old.Name + "</td><td>" + old.CandidateCode + "</td><td>" + old.NationalityDescription + "</td><td>" + old.BirthDate + "</td><td>" + old.CurrentPlaceDescription + "</td><td>" + old.GrossRemuneration +
                    "</td><td>" + old.NET + "</td><td>" + old.DailyGains + "</td><td>" + old.Candidacy + "</td><td>" + old.Availability + "</td><td>" + old.IdClassification + "</td</tr><tr><td colspan='11'>" + old.Description + "</td></tr></table>" +
                    "<br /><b>Updated Info.</b><br />" +
                    "<table border = 1><tr> " + EmailCandidateTableHeader(1) + "</tr><tr>" +
                   "<td>" + update.Name + "</td><td>" + update.CandidateCode + "</td><td>" + update.NationalityDescription + "</td><td>" + update.BirthDate + "</td><td>" + update.CurrentPlaceDescription + "</td><td>" + update.GrossRemuneration +
                    "</td><td>" + update.NET + "</td><td>" + update.DailyGains + "</td><td>" + update.Candidacy + "</td><td>" + update.Availability + "</td><td>" + update.IdClassification + "</td</tr><tr><td colspan='11'>" + update.Description + 
                    "</td></tr></table>");
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        //It creates a new relation between Candidate and CandidatAndRandS
        public void CreateCandRandS(CandidateAndRoleAndSkill CRS)
        {
            try
            {
                if (CRS.MainExperience == 1)
                {
                    context = new ContextModel();
                    context.RevokeMainExp(CRS.IdCandidate);
                    context.CloseDBConnections();
                }

                if (context.GetConnectStringState() == false)
                    context = new ContextModel();

                context.CreateCandidateRollAndSkill(CRS);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void GatherInfoExp(RoleAndSkill rns, CandidateAndRoleAndSkill crns, string currentPage, string user, string candidateName, string newRole, string newSkill)
        {
            try
            {
                string message = "A new <b>experience</b> has been added to <b>" + candidateName + "</b>. <br />Author: " + user + "<br />" +"TimeStamp: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") +
                "<table border='1'><tr>" + EmailExperienceHeader() + "</tr><tr><td>" + newRole + "</td><td>" + newSkill + "</td><td>" + crns.DateStart + "</td><td>" + crns.DateFinish + "</td><td>" +
                    rns.IdGrade + "</td><td>";

                if (crns.MainExperience == 1)
                    message += "Yes";
                else
                    message += "No";

                message += "</td></tr><tr>td colspan='6'></td></tr><tr><td colspan='6'>" + crns.Description + "</td></tr></table>";
                EmailSendNotes("Candidates", "Create", message);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private string EmailExperienceHeader()
        {
            return "<th>Role</th><th>Skill</th><th>Start<br />date</th><th>Finish<br />date</th><th>Skill<br />Eval.</th><th>Main<br />Exp?</th>";
        }

        // It gathers a CREATED candidate full info and sends it to the email method
        public void GatherCandidateInfo(Candidate candidate, RoleAndSkill rns, CandidateAndRoleAndSkill crs, string wp, string user, string textRole, string textSkill, string textPlace, string textStatus, string textAvailaility, string textNat, string textEval, string textGrade)
        {
            try
            {
                string header = EmailCandidateTableHeader(0);
                EmailSendNotes(wp, "Create", "A new Candidate has been added!<br />"+"<table border=1><tr>" + header +
                    "</tr><tr>" +
                    "<td>" + candidate.Name + "</td><td>" + candidate.CandidateCode + "</td><td>" + textNat + "</td><td>" + candidate.BirthDate + "</td><td>" + textRole + "</td><td>" + textSkill + "</td></td>" + crs.DateStart + 
                    "</td><td>" + crs.DateFinish + "</td><td>" + textGrade + "</td><td>" + textPlace + "</td><td>" + textAvailaility +"</td><td>"+ candidate.GrossRemuneration + "</td><td>" + candidate.NET + "</td><td>" + candidate.DailyGains +
                    textStatus + "</td><td>" + textEval  + "</td>" +
                    "</tr></table>");
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private string EmailCandidateTableHeader(int i)
        {
            if (i == 0)
            {
                return "<th>Name</th><th>Candidate<br />Code</th><th>Nat.</th><th>Date of<br />Birth</th><th>Role</th><th>Skill</th><th>Start<br />Date</th><th>Finish<br />Date</th><th>Skill<br />Eval.</th><th>Current<br />Location</th>" +
                    "<th>Avail.</th><th>Gross.<br />Salary</th><th>Net<br />Salary</th><th>Daily<br />Rate</th><th>Candidate<br />Status</th><th>Overall<br />Eval.</th>";
            }
            if (i == 1)
            {
                return "<th>Name</th><th>Candidate<br />Code</th><th>Nat.</th><th>Date of<br />Birth</th><th>Current<br />Location</th>" +
                    "<th>Avail.</th><th>Gross.<br />Salary</th><th>Net<br />Salary</th><th>Daily<br />Rate</th><th>Salary<br />Notes</th><th>Candidate<br />Status</th><th>Overall<br />Eval.</th>";
            }
            else
                return "";
        }

        //It gets a certain CandidateAndRoleAndSkill
        public CandidateAndRoleAndSkill GetCandidateRS(int id)
        {
            CandidateAndRoleAndSkill result = context.GetCandidateRoleAndSkill(id);

            return result;
        }

        //It creates a new Grade
        public void CreateGrade(SkillGrade target)
        {
            try
            {
                if (context.GetConnectStringState() == false)
                    context = new ContextModel();

                context.CreateGrade(target.Grade);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //It lists Grades
        public List<SkillGrade> GetListGrades()
        {
            try
            {
                //if (context.GetConnectStringState() == false || context == null)
                    context = new ContextModel();

                List<SkillGrade> list = context.QueryGrades();
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //It lists UserEvaluations
        public List<CandidateEvaluation> GetListCandidateEvaluation()
        {
            try
            {
                //if (context.GetConnectStringState() == false)
                context = new ContextModel();

                List<CandidateEvaluation> list = context.QueryCandidateEvaluations();
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //It Creates a User
        public void CreateUser(User user)
        {
            context = new ContextModel();
            try
            {
                context.CreateUser(user);
                context.CloseDBConnections();
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        public void EditUser(User update, UserUserRoles uur)
        {
            context = new ContextModel();
            context.EditUser(update);
            context.EditUserUserRole(uur);
            context.CloseDBConnections();
        }

        public int GetLastUser()
        {
            try
            {
                context = new ContextModel();
                return context.GetLastUserId();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        //It gets a list of Users
        public List<User> GetUsersList()
        {
            try
            {
                context = new ContextModel();

                return context.GetUsers();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        //It gets a list of User Roles
        public List<UserRoles> GetUserRolesList()
        {
            try
            {
                context = new ContextModel();

                return context.GetUserRolesList();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        //It Creates a User Role
        public void CreateUserRole(UserRoles role)
        {
            try
            {
                context = new ContextModel();
                context.CreateUserRole(role);
                context.CloseDBConnections();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void CreateUserUserRole(UserUserRoles uur, string user)
        {
            try
            {
                ContextModel context = new ContextModel();
                context.CreateUserUserRole(uur);

                context.CloseDBConnections();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// WP/////////////////////////////
        /// </summary>
        public List<WorkPackage> GetListWorkPackage()
        {
            context = new ContextModel();

            return context.GetListWP();
        }

        public void CreateWorkPackage(string newCode, string newDescription)
        {
            context=new ContextModel();
            WorkPackage newWP = new WorkPackage()
            {
                WPCode = newCode,
                Description = newDescription,
                Active = 1,
            };
            context.CreateWP(newWP);
        }

        /// <summary>
        /// Customers///////////////////////////////
        /// </summary>
        
        //It gets a list of Customers
        public List<Customers> GetListCustomers()
        {
            try
            {
                context = new ContextModel();
                //throw new Exception("teste error");
                return context.GetListCustomers();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        //It creates a Customer
        public void createCustomer(string clientName, int nif, string legalRepresentative, string adress, string email, string FinancialDepartmentEmail)
        {
            try
            {
                Customers newCustomer = new Customers()
                {
                    ClientName = clientName,
                    NIF = nif,
                    LegalRepresentative = legalRepresentative,
                    Adress = adress,
                    Email = email,
                    FinancialOrProcurementDepartmentEmail = FinancialDepartmentEmail,
                    Active = 1,
                };
                context = new ContextModel();

                context.CreateCustomer(newCustomer);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void editCustomer(int oldId, string oldClientName, int oldNIf, string oldLegalRepresentative, string oldAdress, string oldEmail, string oldFinancialDepartmentEmail, int oldActive)
        {
            try
            {
                Customers update = new Customers()
                {
                    IdCustomer = oldId,
                    ClientName = oldClientName,
                    NIF = oldNIf,
                    LegalRepresentative = oldLegalRepresentative,
                    Adress = oldAdress,
                    Email = oldEmail,
                    FinancialOrProcurementDepartmentEmail = oldFinancialDepartmentEmail,
                    Active = oldActive,
                };
                context = new ContextModel();
                context.UpdateCustomer(update);
                context.CloseDBConnections();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Leads///////////////////////////////
        /// </summary>

        //It gets a list of Leads
        public List<LeadsFullInfo> GetLeadsList()
        {
            try
            {
                context = new ContextModel();
                return context.GetLeadsList();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void CreateFullLead(Leads lead)
        {
            try
            {
                context = new ContextModel();
                context.CreateLead(lead);
                context.CloseDBConnections();
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        public void EditFullLead(Leads update)
        {
            try
            {
                context = new ContextModel();

                context.EditLead(update);
                context.CloseDBConnections();
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        public List<LeadNotes> GetListLeadsNotes()
        {
            try
            {
                context = new ContextModel();
                return context.GetListLeadsNotes();
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        public void CreateLeadNote(LeadNotes note, string user, string wp)
        {
            try
            {
                note.TimeStamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                context = new ContextModel();
                context.CreateLeadsNote(note);
                EmailSendNotes(wp, "Create", "<b>User:</b> " + user + "<br /><b>Time:</b> " + note.TimeStamp + "<br /><b>Change:</b> " + note.Description);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void EditLeadNote(LeadNotes update, string user, string wp, string oldNote)
        {
            try
            {
                update.TimeStamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                context = new ContextModel();
                context.EditLeadsNote(update);
                EmailSendNotes(wp, "Update", "<b>User:</b> " + user + "<br /><b>Time:</b> " + update.TimeStamp + "<br /><b>Before:</b> "+ oldNote + "<br /><b>Change:</b> " + update.Description);
            }
            catch (Exception e) 
            {
                throw e;
            }
        }

        public void DeleteLeadNote(int idNote, string user, string oldNote, string wp)
        {
            try
            {
                context = new ContextModel();
                context.DeleteLeadNote(idNote);

                EmailSendNotes(wp, "Delete", "<b>User:</b> " + user + "<br /><b>Time:</b> " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "<br /><b>Before:</b> " + oldNote + "<br /><b>Change:</b> Deleted");
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void FormCandidateListMail(string leadCode, List<CandidateFullInfo> list, int idLead)
        {
            try
            {
                var LeadCandidateList = context.QueryLeadsCandidatesOfACertaiLead(idLead);

                context = new ContextModel();
                context.CloseDBConnections();

                foreach (var  item in list)
                {
                    bool exist = false;

                    foreach (var jitem in LeadCandidateList)
                    {
                        if (item.IdCandidate == jitem.IdCandidate)
                        {
                            exist = true;
                            break;
                        }
                    }

                    if (exist == false) {
                        LeadsCandidates lc = new LeadsCandidates()
                        {
                            IdLead = idLead,
                            IdCandidate = item.IdCandidate,
                            IdCandidateLeadStatus = 1,
                        };
                        context.CreateLeadCandidate(lc);
                        context.CloseDBConnections();
                    }
                }

                List<CandidateLeadStatus> StatusList = context.GetCandidateLeadStatusList();
                list = LeadCandidateStatusGiver(StatusList, list);


                string Row = "These are a list of possible candidates for the lead <b>" + leadCode + "</b><br /><table border='1'>";
                Row += "<tr><th>Status</th><th>Lead Code</th><th>Name</th><th>Nat.</th><th>Role - skill</th><th>Rate</th><th>Service</th><th>Avail.</th><th>Profile</th><th>History</th></tr>";
                foreach (CandidateFullInfo Item in list)
                {
                    Row += "<tr><td> " +Item.LeadCandidateStatusDescription+ " </td><td>" + leadCode + " </td><td>" + Item.CandidateCode + " - " + Item.Name + " </td><td>" + Item.NationalityDescription + " </td><td>" + Item.Role + " - " + Item.Skill +
                        "</td><td>";
                    if (Item.GrossRemuneration != "N/D")
                        Row += RateCandidateMail(Item.GrossRemuneration);
                    else
                        Row += "N/D";
                    Row += "</td><td>Outsourcing</td><td>" + Item.Availability + "</td><td>"+Item.CandidateDescription + "</td><td></td></tr>";
                }
                Row += "</table>";

                LeadCandidateListMAILSEND( leadCode, Row);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private double RateCandidateMail(string gross)
        {
            int k = int.Parse(gross);
            return ((k * 18 + 500)/0.8);
        }

        protected List<CandidateFullInfo> LeadCandidateStatusGiver(List<CandidateLeadStatus> listStatus, List<CandidateFullInfo> listCandidates)
        {
            try
            {
                foreach (var item in listCandidates)
                {
                    foreach (var jitem in listStatus)
                    {
                        if (jitem.IdCandidateLeadStatus == item.IdLeadCandidateStatus)
                            item.LeadCandidateStatusDescription = jitem.Description;
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return listCandidates;
        }

        private void LeadCandidateListMAILSEND(string leadCode, string message)
        {
            try
            {
                MailMessage mail = new MailMessage()
                {
                    From = new MailAddress("adonis@reloading.biz"),
                    Subject = "Lead " + leadCode + " Candidate list",
                    Body = message,
                    Priority = MailPriority.Normal,
                    IsBodyHtml = true,
                };
                mail.To.Add("tech@reloading.biz");

                SmtpClient SmtpServer = new SmtpClient();

                SmtpServer.EnableSsl = false;

                SmtpServer.Send(mail);                
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<CandidateLeadStatus> GetCandidateLeadStatusList()
        {
            try
            {
                context = new ContextModel();
                return context.GetCandidateLeadStatusList();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void ChangeLeadCandidateStatus(LeadsCandidates lc)
        {
            try
            {
                context = new ContextModel();
                var ListLC = context.QueryLeadsCandidates();
                foreach (var item in ListLC)
                {
                    //if (item.)
                }

                context.UpdateLeadCandidate(lc);
                context.CloseDBConnections();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<LeadsCandidates> GetListLeadsCandidates()
        {
            try
            {
                context = new ContextModel();
                List<LeadsCandidates> list = context.QueryLeadsCandidates();
                context.CloseDBConnections();
                return list;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// LeadStatus/////////////////////////
        /// </summary>
        
        //It gets a list of LeadStatus
        public List<LeadStatus> getLeadStatusList()
        {
            try
            {
                context = new ContextModel();
                return context.GetLeadStatusList();
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        public void CreateLeadStatus(string description, string r, string g, string b)
        {
            try
            {
                LeadStatus newLeadStatus = new LeadStatus()
                {
                    Description = description,
                    Color = String.Concat(r, g, b),
                    Active = 1,
                };
                context = new ContextModel();
                context.CreateLeadStatus(newLeadStatus);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void EditLeadStatus(int idstatus, string eDescription, string eR, string eG, string eB, int Active)
        {
            try
            {
                int IdStatus = int.Parse(idstatus.ToString());
                LeadStatus update = new LeadStatus()
                {
                    IdLeadStatus = IdStatus,
                    Description = eDescription,
                    Color = String.Concat(eR, eG, eB),
                    Active = Active,
                };

                context = new ContextModel();
                context.EditLeadStatus(update);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //public List<CandidateFullInfo> SearchCandidatesBy(string Role, string Skill, string Name)
        //{
        //    context = new ContextModel();
        //    context.ChangeState(true);
        //    List<CandidateFullInfo> result = new List<CandidateFullInfo>();
        //    if (Role != "-" && Skill == "-" && Name == "" ) //Role
        //        result = context.QueryCandidatesByRole(Role);
        //    else if (Role == "-" && Skill != "-" && Name == "") //Skill
        //        result = context.QueryCandidatesBySkill(Skill);
        //    else if (Role != "-" && Skill != "-" && Name == "") //Role OR Skill
        //        result = context.QueryCandidatesByRoleOrSkill(Role, Skill);
        //    else if (Role == "-" && Skill == "-" && Name != "") //Name
        //        result = context.QueryCandidatesByName(Name);
        //    else if (Role == "-" && Skill == "-" && Name == "") //reset search
        //        result = context.QueryCandidatesByRoleOrSkill(Role, Skill);

        //    return result;
        //}

        //It asks for a List of Candidacies
        public List<Candidacy> GetListCandidacy()
        {
            try
            {
                context = new ContextModel();

                List<Candidacy> list = context.QueryCandidacy();
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //It Creates a Candidacie
        public void CreateCandidacy(Candidacy newCandidacy)
        {
            try
            {
                context = new ContextModel();

                context.CreateCandidacy(newCandidacy);
                context.CloseDBConnections();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //It Edits a Candidacie
        public void EditCandidacy(Candidacy UpdatedCandidacy)
        {
            try
            {
                context = new ContextModel();
                CheckIfCreated();

                context = new ContextModel();
                context.ChangeState(true);
                context.UpdateCandidacy(UpdatedCandidacy);
                context.CloseDBConnections();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CandidateStatus> GetListCandidateStatus()
        {
            try
            {
                context = new ContextModel();
                return context.QueryCandidateStatus();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateCandidateStatus(string description, string R, string G, string B, string idstatus, int Active)
        {
            try
            {
                int IdStatus = int.Parse(idstatus);
                CandidateStatus update = new CandidateStatus()
                {
                    IdStatus = IdStatus,
                    Description = description,
                    Color = String.Concat(R, G, B),
                    Activated = Active,
                };

                context = new ContextModel();
                context.EditCandidateStatus(update);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void CreateCandidateStatus(string description, string R, string G, string B)
        {
            try
            {
                CandidateStatus candidateStatus = new CandidateStatus()
                {
                    Description = description,
                    Color = String.Concat( R, G, B),
                    Activated = 1,
                };

                context = new ContextModel();
                context.CreateCandidateStatus(candidateStatus);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Availability> GetListAvailability()
        {
            try
            {
                context = new ContextModel();
                return context.QueryAvailability();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void CreateAvailability(string description)
        {
            try
            {
                context = new ContextModel();

                Availability availability = new Availability()
                {
                    Description = description,
                    Activated = 1,
                };

                context.CreateAvailability(availability);
                context.CloseDBConnections();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void EditAvailability(string IdAvailability, string newName, int Activated)
        {
            Availability update = new Availability()
            {
                IdAvailability = int.Parse(IdAvailability),
                Description = newName,
                Activated = Activated,
            };

            context = new ContextModel();

            try
            {
                context.EditAvailability(update);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public List<Location> GetListLocations()
        {
            try
            {
                context = new ContextModel();
                return context.QueryLocation();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        

        public void CreateLocation(string description)
        {
            try
            {
                context = new ContextModel();

                Location location = new Location()
                {
                    Description = description,
                    Activated = 1,
                };

                context.CreateLocation(location);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        
        public void EditLocation(string IdAvailability, string newName, int Activated)
        {
            Location update = new Location()
            {
                IdLocation = int.Parse(IdAvailability),
                Description = newName,
                Activated = Activated,
            };

            context = new ContextModel();

            try
            {
                context.EditLocation(update);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public List<Nationalities> GetListNationalities()
        {
            try
            {
                context = new ContextModel();
                return context.GetNationalitiesList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void CreateNationality(string newNationality, int newforCandidates, int newforLeads)
        {
            try
            {
                Nationalities NewNationality = new Nationalities()
                {
                    NationalityDescription = newNationality,
                    forCandidates = newforCandidates,
                    forLeads = newforLeads,
                    Active = 1,
                };

                context = new ContextModel();
                context.CreateNationality(NewNationality);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void EditNationality(int oldId, string oldNationality, int oldforCandidates, int oldforLeads, int oldActive)
        {
            try
            {
                Nationalities UpdateNationality = new Nationalities()
                {
                    IdNationality = oldId,
                    NationalityDescription = oldNationality,
                    forCandidates = oldforCandidates,
                    forLeads = oldforLeads,
                    Active = oldActive,
                };

                context = new ContextModel();
                context.UpdateNationality(UpdateNationality);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        //Gets Last CandidateId
        public int GetLastCandidate()
        {
            int IdCandidate = 0;

            try
            {
                if (context.GetConnectStringState() == false)
                    context = new ContextModel();

                IdCandidate = context.SelectLastCandidate();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return IdCandidate;
        }

        //ANY ACTION GOES TO LOGGING
        public void ActionLOGGING(string loggedInUser, int idUser, string epic, string page, string cru, string before, string after, Exception custom)
        {
            try
            {
                context = new ContextModel();
                context.Logging(loggedInUser, idUser, epic, page, cru, before, after, custom);
                context.ChangeState(true);

                string dir = context.GetLoggingPath();

                //Revisitar este ponto. Logging tem o prob de que ele precisa de ter uma direção definida.
                //Falar sobre isto. Provavelmente terei que fazer uma página para o configurador.

                //Checks if directory exists
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }

                bool fileexist = File.Exists(dir + @"\" + @"\" + DateTime.Now.ToString("yyyy-MM-dd") + "_ADONIS-LOG.log");
                
                dir += @"\" + DateTime.Now.ToString("yyyy-MM-dd") + "_ADONIS-LOG.log";
                if (!fileexist)
                    File.Create(dir).Close();

                LoggingWriter(dir, loggedInUser, idUser, epic, page, cru, before,after,custom);
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        //It gets the last logging
        public int GetLastLogging(User user)
        {
            try
            {
                context = new ContextModel();

                return context.GetLastLoggingCode();
            }
            catch (Exception ex)
            {
                ActionLOGGING(user.Name, user.IdUser, "Logging", "Error","Read","","",new Exception("Failed to get last Logg:-:"+ex.Message.ToString()));
                return 0;
            }
        }

        //Gets the path for Logging Actions
        public string GetLogPath()
        {
            context = new ContextModel();
            return context.GetLoggingPath();
        }

        //It send an email to X targets
        public void EmailSendNotes(string wp, string type, string changes)
        {
            try
            {
                foreach (WorkPackage item in WPList)
                {
                    if (item.WPCode == wp)
                    {
                        MailMessage mail = new MailMessage()
                        {
                            From = new MailAddress("adonis@reloading.biz"),
                            Subject = "New Changes have been made on " + wp,
                            Body = "Changes of type <b>" + type + "</b> have been made on <b>" + wp + "</b>.<br />" + changes,
                            Priority = MailPriority.Normal,
                            IsBodyHtml = true,
                        };
                        mail.To.Add(item.WPEmail);
                        //mail.To.Add("tech@reloading.biz");

                        SmtpClient SmtpServer = new SmtpClient();
                        //SmtpServer.Host = "mail.reloading.biz";
                        //SmtpServer.Host = ConfigurationManager.ConnectionStrings["host"].ToString();
                        //SmtpServer.Port = 465; //em vez de porto 25
                        //SmtpServer.UseDefaultCredentials = true; //quando se seleciona o porto 25, deverá estar a true
                        //SmtpServer.Credentials = new System.Net.NetworkCredential("fferreira@reloading.biz", "T3chW€ll");
                        SmtpServer.EnableSsl = false;

                        SmtpServer.Send(mail);

                        break;
                        //MailMessage mailMessage = new MailMessage();
                        //mailMessage.From = new MailAddress("fferreira@reloading.biz");
                        //mailMessage.To.Add(new MailAddress("tech@reloading.biz"));
                        //string emailBody = "<font face=arial size=2>Test Asp.net Send Mail</font>";
                        //mailMessage.IsBodyHtml = true;
                        //mailMessage.Subject = "test";
                        //mailMessage.Body = emailBody;
                        //SmtpClient smtpClient = new SmtpClient();
                        //smtpClient.Send(mailMessage);
                        ////lblInfo.Text = "Email has been sent";
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            
        }

        public List<string> CSVFileReader(string path, int sheet)
        {
            try
            {
                //in excel
                //_Application Excel = new _Excel.Application();
                //Workbook Wb;
                //Worksheet Ws;
                //path = @"C:\Users\Francisco Ferreira\Desktop\WORK RELATED\projecto\";
                //Wb = Excel.Workbooks.Open(path + "Livro1");
                //Ws = Wb.Worksheets[sheet];
                //var l = ReadExcelCell(1,1,Ws);

                var Reader = new StreamReader(File.OpenRead(path));
                List<string> data = new List<string>();

                while (!Reader.EndOfStream)
                {
                    string line = Reader.ReadLine();

                    if (!String.IsNullOrWhiteSpace(line))
                    {
                        //string row = line;
                        data.Add(line);
                    }
                }
                return data;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /////////////////////////tool box/////////////////////////////////////////////////////////////////
        private ContextModel CheckIfCreated()
        {
            if (context.GetConnectStringState() == false)
                return new ContextModel();
            return null;
        }

        private bool CheckIfCandidateEqual(Candidate a, Candidate b)
        {
            if (a.IdCandidate == b.IdCandidate && a.Name == b.Name && a.GrossRemuneration == b.GrossRemuneration && a.Activated == b.Activated && a.Availability == b.Availability && a.Candidacy == b.Candidacy)
                return true;

            return false;
        }

        private bool CheckIfCandidateAndRollAndSkillEqual(CandidateAndRoleAndSkill a, CandidateAndRoleAndSkill b)
        {
            if (a.IdCRNS == b.IdCRNS && a.IdRolesAndSkills == b.IdRolesAndSkills && a.Description == b.Description && a.DateStart == b.DateStart && a.DateFinish == b.DateFinish && a.Description == b.Description)
                return true;

            return false;
        }

        private bool CheckIfRollAndSkillEqual(RoleAndSkill a, RoleAndSkill b)
        {
            if (a.Id == b.Id && a.IdGrade == b.IdGrade && a.IdRole == b.IdRole && a.IdSkill == b.IdSkill)
                return true;

            return false;
        }

        private void LoggingWriter(string dir, string loggedUser, int idUser, string epic, string page, string cru, string before, string after, Exception custom)
        {
            
            //StreamReader strReader = new StreamReader(dir);
            //string Logg = strReader.ReadToEnd();
            //strReader.Close();

            StreamWriter streamWriter = new StreamWriter(dir,append: true);
            //if (!(Logg == ""))
            //    Logg += @"\LF"+@"\CR";
            //Logg += DateTime.Now.ToString("yyyy-MM-dd") + " _ " + DateTime.Now.ToString("HH:mm:ss:tt") + " _ " + idUser + " _ " + loggedUser + " _ " + epic + " _ " + cru + " _ " + page + " _ " + before + " _ " + after + " _ " + custom;
            streamWriter.WriteLine(DateTime.Now.ToString("yyyy-MM-dd") + " _ " + DateTime.Now.ToString("HH:mm:ss:tt") + " _ " + idUser + " _ " + loggedUser + " _ " + epic + " _ " + cru + " _ " + page + " _ " + before + " _ " + after + " _ " + custom + 
                Environment.NewLine);
            //streamWriter.Write(Logg);
            //streamWriter.
            streamWriter.Close();

        }
    }
}