using Adonis.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Adonis.Controllers
{
     
    public class AdnController : Controller
    {
        protected string currentPage = String.Empty;
        AppGeneral app = new AppGeneral();
        Exception ex = new Exception("NONE");
        List<User> UserList = new List<User>();

        // GET: Adn
        public ActionResult Index()
        {
            try
            {
                StartApp();
                string session = Convert.ToString(Session["User"].ToString());
                if (session != null)
                {
                    currentPage = "Index";
                    LoggingAction("R&S-US.004", "Read", "", "", ex);
                    return View("Index", this);
                }
            }
            catch
            {
                User loggedInUser = new User()
                {
                    IdUser = -1,
                    Name = "Guest",
                };
                ViewBag.loggedInUser = loggedInUser;
                Session["User"] = "Guest";
                Session["IdUser"] = -1;
                return RedirectToAction("LogIn", this);
            }
            return View();
        }

        public ActionResult ErrorPage()
        {
            StartApp();

            AdnDbController controller = new AdnDbController();
            int log=0;
            try
            {
                log = controller.GetLastLogging(GetLoggedInUser());
            }
            catch (Exception ex)
            {
                LoggingAction("Logging", "Read", "","",new Exception("Failed to get las Logg:-:"+ex.Message.ToString()));
            }

            ViewBag.ErrorCode = log;
            return View();
        }

        ////////////////////////////////////////
        /////////////Roles//////////////////////
        
        //GET:Roles
        [AllowAnonymous]
        public ActionResult Roles()
        {
            StartApp();
            //GetListsForFront();
            ViewBag.Roles = GetListRoles();
            currentPage = "Roles";
            LoggingAction("R&S-US.002", "Read", "", "", ex);
            return View();
        }

        //POST: CreateRoleAction
        [AllowAnonymous]
        public ActionResult CreateRoleAction(string newRole)
        {
            try
            {
                AdnDbController controller = new AdnDbController();

                Role role = new Role()
                {
                    NameRole = newRole,
                    Active = 1,
                };
                LoggingAction("R&S-US.004", "Create", "", string.Concat(newRole, ",", 1), ex); ;
                controller.CreateRole(role, GetLoggedInUser());

                return RedirectToAction("Roles", controller);
            }
            catch(Exception e)
            {
                LoggingAction("R&S-US.004", "Create", "", string.Concat(newRole, ",", 1),new Exception("Create Role Failed:-:"+e.Message));
                AdnDbController controller = new AdnDbController();
                return RedirectToAction("Roles", controller);
            }
           
        }

        //POST:EditRoleAction
        [AllowAnonymous]
        public ActionResult EditRoleAction(string newName, int Activated, string OldId)
        {
            try
            {
                AdnDbController controller = new AdnDbController();
                Role update = new Role()
                {
                    IdRole = int.Parse(OldId),
                    NameRole = newName.Trim(),
                    Active = Activated,
                };
                controller.EditRole(update, GetLoggedInUser());
                LoggingAction("R&S-US.004", "Update", "", string.Concat(update.NameRole, ",", update.Active), new Exception("Update Role Success"));
                return RedirectToAction("Roles", controller);
            }
            catch(Exception e)
            {
                LoggingAction("R&S-US.004", "Update", "", string.Concat(newName, ",", Activated), new Exception( "Update Role Failed:-:" + e.Message));
                return View("Roles");
            }
        }

        //Post: CreateCategoryAction
        public ActionResult CreateCategoryAction(string newCategory)
        {
            AdnDbController controller = new AdnDbController();
            controller.CreateCategory(newCategory);

            return RedirectToAction("CreateCategory", controller);
        }

        //POST: EditCategoryAction
        public ActionResult EditCategoryAction(int OldId, string NewName, int Active)
        {
            AdnDbController controller = new AdnDbController();
            SkillCategory update = new SkillCategory()
            {
                IdCategory = OldId,
                NameCategory = NewName.Trim(),
                Activated = Active,
            };
            controller.EditCategory(update);

            return View();
        }

        //GET:Skills
        public ActionResult Skills()
        {
            AdnDbController controller = new AdnDbController();
            ViewBag.Skills = GetListSkills();
            ViewBag.SkillCategories = GetListCategory();

            currentPage = "Skills";
            LoggingAction("R&S","Read","","",ex);
            return View();
        }

        //POST: CreateSkillAction
        public ActionResult CreateSkillAction(string newSkill, int Category)
        {
            AdnDbController controller = new AdnDbController();
            if (newSkill != null || newSkill != "")
            {
                controller.CreateSkill(newSkill, int.Parse(Category.ToString()));
            }
            return RedirectToAction("Skills", controller);
        }

        //POST: EditSkillAction
        public ActionResult EditSkillAction(int OldId, string oldSkill, int CategoryEdit, int Activated)
        {
            AdnDbController controller = new AdnDbController();
            
            Skills update = new Skills()
            {
                IdSkill = OldId,
                Name = oldSkill,
                IdCategory = CategoryEdit,
                Active = Activated,
            };
            controller.EditSkill(update);

            return RedirectToAction("Skills", controller);
        }

        /////////////////////////////////////////////////////////////////
        //GET: Candidacies
        [HttpGet]
        public ActionResult Candidacies()
        {
            try
            {
                StartApp();
                //ViewBag.Candidacies = GetListCandidacy();
                currentPage = "Candidacies";
                LoggingAction("R&S-US.004", "Read", "", "", new Exception("Candidacies Page Succeeded"));
                return View();
            }
            catch (Exception e)
            {
                LoggingAction("R&S-US.004", "Read", "", "", new Exception("Candidacies Page Failed:-:" + e.Message));
                return RedirectToAction("Index", this);
            }
            
        }
        
        //Gets a JSon List of Candidacies
        [HttpGet]
        public ActionResult JSONCandidaciesList()
        {
            try
            {
                var result = GetListCandidacy();
                LoggingAction("R&S-US.004", "Read", "", "", new Exception("Candidacies JSON Get Succeeded"));
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                LoggingAction("R&S-US.004", "Read", "", "", new Exception("Candidacies JSON Get Failed:-:" + e.Message));
                return Json("", JsonRequestBehavior.AllowGet);
            }
            
        }

        //POST: CreateCandidacy 
        [HttpPost]
        public ActionResult CreateCandidacy(string newcandidacy)
        {
            try
            {
                AdnDbController controller = new AdnDbController();
                
                Candidacy newCandidacy = new Candidacy()
                {
                    Description = newcandidacy,
                    Activated = 1,
                };
                controller.CreateCandidacy(newCandidacy);
                LoggingAction("R&S-US.004", "Create", "", newcandidacy+","+"1", new Exception("Candidacies Create Succeeded"));
                return RedirectToAction("Candidacies", controller);
            }
            catch (Exception e)
            {
                LoggingAction("R&S-US.004", "Create", "", "", new Exception("Candidacies Create Failed:-:"+e.Message));
                return RedirectToAction("Candidacies", this);
            }
            
        }

        //POST: EditCandidacy
        [HttpPost]
        public ActionResult EditCandidacy(string newName, int Activated, int OldId)
        {
            try
            {
                AdnDbController controller = new AdnDbController();
                Candidacy OldCandidacy = new Candidacy();
                List<Candidacy> CandidacyList = GetListCandidacy();

                Candidacy UpdatedCandidacy = new Candidacy()
                {
                    IdCandidacy = OldId,
                    Description = newName,
                    Activated = Activated,
                };

                foreach (var item in CandidacyList)
                {
                    if (item.IdCandidacy == UpdatedCandidacy.IdCandidacy && (item.Description != UpdatedCandidacy.Description || item.Activated != UpdatedCandidacy.Activated))
                    {
                        OldCandidacy.IdCandidacy = item.IdCandidacy;
                        OldCandidacy.Description = item.Description;
                        OldCandidacy.Activated = item.Activated;
                        controller.EditCandidacy(UpdatedCandidacy);
                    }
                }
                LoggingAction("R&S-US.004", "Update", OldCandidacy.IdCandidacy+","+OldCandidacy.Description+","+OldCandidacy.Activated, OldId+","+newName+","+Activated, new Exception("Candidacies Edit Succeeded"));
                return RedirectToAction("Candidacies", controller);
            }
            catch (Exception e)
            {
                LoggingAction("R&S-US.004", "Update", "", OldId + "," + newName + "," + Activated, new Exception("Candidacies Edit Failed:-:"+e.Message));
                return RedirectToAction("Candidacies", this);
            }
        }

        ///////////////////////////////////////////////////////////////////////

        //GET: List of Roles and Skills to populate list boxes !---:---!
        public ActionResult GetRolesAndSkillsLists()
        {
            AdnDbController controller = new AdnDbController();

            IEnumerable<Role> resultListRole = null;
            IEnumerable<Skills> resultListSkill = null;

            List<Role> RolesList = new List<Role>();
            List<Skills> SkillsList = new List<Skills>();
            try
            {
                resultListRole = controller.GetListRoles();

                resultListSkill = controller.GetListSkills();

                foreach (var item in resultListRole)
                {
                    Role role = new Role()
                    {
                        IdRole = item.IdRole,
                        NameRole = item.NameRole,
                    };

                    RolesList.Add(role);
                }

                foreach (var item in resultListSkill)
                {
                    Skills role = new Skills()
                    {
                        IdSkill = item.IdSkill,
                        Name = item.Name,
                    };

                    SkillsList.Add(role);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            List<object> Box = new List<object>();
            Box.Add(resultListRole);
            Box.Add(resultListSkill);

            ViewBag.RandSBox = Box;
            ViewBag.CounterStyle = 0;

            return View();
        }

        //POST: Creates a relation RoleAndSkill !---:---!
        public ActionResult CreateRelationRS(int RnSValue, FormCollection relation)
        {
            AdnDbController controller = new AdnDbController();

            var skill = from item in relation.AllKeys
                        where relation[item] != "false"
                        select item;

            List<string> skilllist = skill.ToList();
            List<RoleAndSkill> listNewRoleAndSkill = new List<RoleAndSkill>();

            //iterator begins at 1 because of variable relation. FormCollection gets the name of select element but not its selected value 
            for (int i = 1; i < skill.Count(); i++)
            {
                RoleAndSkill newRnS = new RoleAndSkill()
                {
                    IdRole = RnSValue,
                    IdSkill = int.Parse(skilllist[i].ToString()),
                };
                listNewRoleAndSkill.Add(newRnS);
            }
            controller.CreateRoleAndSkill(listNewRoleAndSkill);
            return RedirectToAction("GetRolesAndSkillsLists", controller);
        }

        //GET: EditRnS  !---:---!
        public ActionResult EditRnS()
        {
            ViewBag.ListRnS = GetListRnS();
            ViewBag.ListSkills = GetListSkills();
            ViewBag.ListRoles = GetListRoles();
            ViewBag.ListSkillGrade = GetListGrades();

            return View();
        }

        //POST: Lists Candidates and their info in Roles and Skills !---:---!
        public ActionResult GetCandidateFullData()
        {
            try
            {
                return View(GetListCandidatesAndInfo());
            }catch(Exception e)
            {
                throw e;
            }
            
        }

        //GET: CreateCandidateAndRandS !---:---!
        [Authorize]
        public ActionResult CreateCandidateAndRandS()
        {
            AdnDbController controller = new AdnDbController();
            ViewBag.ListCandidates = controller.GetListCandidate();
            ViewBag.ListRolesAndSkills = controller.GetListRolesAndSkills();
            StartApp();

            return View();
        }

        //POST: CreateCandidateAndRandSAction !---:---!
        [Authorize]
        public ActionResult CreateCandidateAndRandSAction(string IdCandidate, int rands, string init, string end, string description)
        {
            //int candidateId, int rands, string start, string finish, string description, int? teste
            AdnDbController controller = new AdnDbController();

            CandidateAndRoleAndSkill crs = new CandidateAndRoleAndSkill()
            {
                IdCandidate = int.Parse(IdCandidate),
                IdRolesAndSkills = rands,
                DateStart = init,
                DateFinish = end,
                Description = description,
            };

            controller.CreateCandRandS(crs);
            return View();
        }

        //GET: CreateGrade !---:---! Pensar em criar uma página grade
        [Authorize]
        public ActionResult CreateGrade()
        {
            return View();
        }

        //POST: CreateGradeAction  !---:---!
        [Authorize]
        public ActionResult CreateGradeAction(string Name)
        {
            AdnDbController controller = new AdnDbController();

            SkillGrade gra = new SkillGrade()
            {
                Grade = Name.ToString(),
            };

            controller.CreateGrade(gra);
            return RedirectToAction("CreateGrade", controller);
        }

        //////////////////////////////////////////////////
        ///////////CandidateStatus////////////////////////
        
        //GET:CandidateStatus
        public ActionResult CandidateStatus()
        {
            try
            {
                StartApp();
                currentPage = "CandidateStatus";
                LoggingAction("R&S-US.004", "Read", "", "", new Exception("CandidateStatus Page Succeeded"));
                return View();
            }
            catch (Exception e)
            {
                LoggingAction("R&S-US.004", "Read", "", "", new Exception("CandidateStatus Page Failed:-:"+e.Message));
                return RedirectToAction("ErrorPage",this);
            }
            
        }

        //GET: JSONCandidateStatusList
        public ActionResult JSONCandidateStatusList()
        {
            try
            {
                var result = GetListCandidateStatus();
                LoggingAction("R&S-US.004", "Read", "", "", new Exception("CandidateStatus JSon List Succeeded"));
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                LoggingAction("R&S-US.004", "Read", "", "", new Exception("CandidateStatus JSon List Failed:-:" + e.Message));
                return Json("", JsonRequestBehavior.AllowGet);
            }
            
        }

        //POST: CreateCandidateStatusAction
        public ActionResult CreateCandidateStatusAction(string description, string R, string G, string B)
        {
            try
            {
                currentPage = "Candidacies";
                AdnDbController controller = new AdnDbController();
                if ((description != null || description != ""))
                {
                    controller.CreateCandidateStatus(description, R, G, B);
                }
                LoggingAction("R&S-US.004", "Create", "", "", new Exception("CandidateStatus Create Succeeded"));
                return RedirectToAction("CandidateStatus", controller);
            }
            catch (Exception e)
            {
                LoggingAction("R&S-US.004", "Create", "", "", new Exception("CandidateStatus Create Failed:-:" + e.Message));
                return RedirectToAction("ErrorPage", this);
            }
            
        }

        //POST: EditCandidateStatusAction
        public ActionResult EditCandidateStatusAction(string eDescription, string eR, string eG, string eB, string IdStatus, int Active)
        {
            AdnDbController controller = new AdnDbController();
            List<CandidateStatus> list = controller.GetListCandidateStatus();
            CandidateStatus old = new CandidateStatus();

            int id = int.Parse(IdStatus);

            foreach (CandidateStatus item in list)
            {
                if (item.IdStatus == id)
                    old = item;
            }
            try
            {
                if ((eDescription != null || eDescription != ""))
                {
                    controller.UpdateCandidateStatus(eDescription, eR, eG, eB, IdStatus, Active);
                }
                LoggingAction("R&S", "Update",old.IdStatus+","+old.Description+","+old.Color+","+old.Activated, id+","+eDescription+","+eR+eG+eB+","+Active, new Exception("CandidateStatus Edit Succeeded"));
                return RedirectToAction("CandidateStatus", controller);
            }
            catch (Exception e)
            {
                LoggingAction("R&S", "Update", old.IdStatus + "," + old.Description + "," + old.Color + "," + old.Activated, id + "," + eDescription + "," + eR + eG + eB + "," + Active, new Exception("CandidateStatus Edit Failed"+e.Message));
                return RedirectToAction("ErrorPage", this);
            }
            
        }

        ////////////////////////////////////////////////////////////
        //GET: Availability
        public ActionResult Availability()
        {
            try
            {
                StartApp();
                currentPage = "Availability";
                LoggingAction("R&S-US.002", "Read", "","",new Exception("Availability Page Succeeded"));
                return View();
            }
            catch (Exception e)
            {
                LoggingAction("R&S-US.002", "Read", "", "", new Exception("Availability Page Failed:-:"+e.Message));
                return RedirectToAction("Index", this);
            }
            
        }

        public ActionResult JSONAvailabilityList()
        {
            try
            {
                var result = GetListAvailability();
                LoggingAction("R&S-US.002", "Read", "", "", new Exception("Availability Json List Succeeded"));
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var result = "";
                LoggingAction("R&S-US.002", "Read", "", "", new Exception("Availability Json List Failed:-:" + e.Message));
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            
        }

        //POST: CreateAvailabilityAction
        public ActionResult CreateAvailabilityAction(string description)
        {
            try
            {
                AdnDbController controller = new AdnDbController();
                controller.CreateAvailability(description);
                LoggingAction("R&S-US.002", "Create", "", description, new Exception("Availability Retrieving Info Succeeded"));
                return RedirectToAction("Availability", controller);
            }
            catch (Exception e)
            {
                LoggingAction("R&S-US.002", "Create", "", description, new Exception("Availability Retrieving Info Failed" + e.Message));
                return RedirectToAction("Availability", this);
            }
            
        }

        //POST: EditAvailabilityAction
        public ActionResult EditAvailabilityAction(string OldId, string newName, int Activated)
        {
            try
            {
                AdnDbController controller = new AdnDbController();

                controller.EditAvailability(OldId, newName, Activated);
                LoggingAction("R&S-US.002", "Update", OldId, OldId+";"+newName+";"+Activated, new Exception("Availability Updating Info Succeeded"));
                return RedirectToAction("Availability", controller);
            }
            catch (Exception e)
            {
                LoggingAction("R&S-US.002", "Update", OldId, OldId + ";" + newName + ";" + Activated, new Exception("Availability Updating Info Failed" + e.Message));
                return RedirectToAction("Availability", this);
            }
        }
        
        /////////////////////////////////////////////////
        //GET:Location

        public ActionResult Location()
        {
            try
            {
                currentPage = "Location";
                StartApp();
                LoggingAction("R&S", "Read", "", "", new Exception("Get Location Succeded")); ;
                return View();
            }
            catch(Exception e)
            {
                LoggingAction("R&S", "Read", "", "", new Exception("Location Failed :-: " + e.Message));
                return RedirectToAction("ErrorPage", this);
            }
        }

        public ActionResult JSONLocationList()
        {
            var result = GetListLocation();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CreateLocationAction(string description)
        {
            AdnDbController controller = new AdnDbController();
            controller.CreateLocation(description);

            return RedirectToAction("Location", controller);
        }

        //POST: EditAvailabilityAction
        public ActionResult EditLocationAction(string OldId, string newName, int Activated)
        {
            AdnDbController controller = new AdnDbController();

            controller.EditLocation(OldId, newName, Activated);

            return RedirectToAction("Location", controller); ;
        }

        ////////////////////////////////////////////////
        //GET:Candidate
        [HttpGet]
        public ActionResult Candidates()
        {
            try
            {
                currentPage = "Candidates";
                StartApp();
                GetListsForFront();
                LoggingAction("R&S-US.002", "Read", "", "", ex = new Exception("Candidate Page Accessed"));

                return View();
            }
            catch(Exception e)
            {
                LoggingAction("R&S-US.002", "Read", "", "", e);
                return RedirectToAction("ErrorPage", this);
            }
        }

        //GET: JSONCandidateList
        [HttpGet]
        public ActionResult JSONCandidateList()
        {
            try
            {
                currentPage = "Candidates";
                var result = GetListCandidatesAndInfo();
                LoggingAction("R&S-US.002", "Read", "", "", ex = new Exception("Asked bd for Candidate List"));
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                LoggingAction("R&S-US.002", "Read", "", "", e);
                return RedirectToAction("ErrorPage",this);
            }
        }

        [HttpGet]
        public ActionResult JsonGetLeadsCandidatesList()
        {
            List<LeadsCandidates> list = new List<LeadsCandidates>();
            try 
            {
                AdnDbController controller = new AdnDbController();
                list = controller.GetListLeadsCandidates();
                return Json(list, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        //POST: CreateCandidateFullCVAction ActionResult
        public int CreateCandidateFullCVAction(string CreateName, int CreateNationality, int Role, string Init, string End, int Skill, string Desc, int Grade, int? Gross,
            string Eval, int avaiability, int? DailyGains, string? RemunerationNotes, int CurrentPlace, int status, string Cand, int? NET, string CandidateCode,
            string? interview, int mainxp, string CreateBirthDate, string textRole, string textSkill, string textPlace, string textStatus, string textAvailaility, string textNat, string textEval, string textGrade,
            int CreateContactNumber, string CreateEmail)
        {
            currentPage = "Candidates";
            try
            {
                Candidate candidate = new Candidate()
                {
                    Name = CreateName,
                    Activated = 1,
                    //GrossRemuneration = Gross,
                    Availability = avaiability,

                    //NET =NET,
                    //DailyGains = DailyGains,
                    RemunerationNotes = RemunerationNotes,
                    CurrentPlace = CurrentPlace,
                    IdStatus = status,
                    Interview = interview,
                    Candidacy = Cand,
                    IdClassification = int.Parse(Eval),
                    CandidateCode = CandidateCode,
                    IdNationality = CreateNationality,
                    BirthDate = CreateBirthDate,
                    Description = Desc,
                    ContactNumber = CreateContactNumber,
                    Email = CreateEmail,
                };

                candidate = CheckIfRemunerationsAreNull(candidate, Gross, DailyGains, NET);
                
                RoleAndSkill rns = new RoleAndSkill()
                {
                    IdRole = Role,
                    IdSkill = Skill,
                    IdGrade = Grade,
                };

                AdnDbController controller = new AdnDbController();
                controller.CreateCandidate(candidate);
                controller.CreateRoleAndSkillSingle(rns);

                int LastIdCandidate = controller.GetLastCandidate();

                CandidateAndRoleAndSkill crs = new CandidateAndRoleAndSkill()
                {
                    IdCandidate = LastIdCandidate,
                    IdRolesAndSkills = controller.GetLastRoleAndSKillId(),
                    DateStart = Init,
                    DateFinish = End,
                    MainExperience = mainxp,
                };
                
                controller.CreateCandRandS(crs);

                CandidateNotes description = new CandidateNotes()
                {
                    IdCandidate = LastIdCandidate,
                    Note = Desc,
                    TimeStamp = DateTime.Now.ToString(),
                };

                //controller.CreateCandidateDescription(description);
                controller.GatherCandidateInfo(candidate, rns, crs, currentPage, Session["User"].ToString(), textRole, textSkill, textPlace, textStatus, textAvailaility, textNat, textEval, textGrade);

                //return RedirectToAction("Candidates", controller);
                return 1;
            }
            catch (Exception e)
            {
                throw e;
            }
            
        }

        //POST: EditCandidateFullInfoAction
        public ActionResult EditCandidateFullInfoAction(string id, string UpdateName, string UpdateRole, string UpdateInit, string UpdateEnd, string UpdateSkill, string UpdateDesc, string UpdateGrade,
            int UpdateActivated, string? UpdateGross, int UpdateAvaiability, string crns, string RnS, int UpdateEvaluation, string? UpdateNET, string? UpdateDailyGains, string? UpdateRemunerationNotes,
            int UpdateCurrentPlace, string? UpdateInterview, int UpdateStatus, int UpdateCand, string UpdateCandidateCode, int UpdateMainxp, int EditNationality, string EditBirthDate)
        {
            try
            {
                AdnDbController controller = new AdnDbController();

                Candidate CandidateEdit = new Candidate()
                {
                    IdCandidate = int.Parse(id),
                    Name = UpdateName,
                    Activated = UpdateActivated,
                    Availability = UpdateAvaiability,
                    RemunerationNotes = UpdateRemunerationNotes,
                    CurrentPlace = UpdateCurrentPlace,
                    status = UpdateStatus,
                    IdStatus = UpdateStatus,
                    Interview = UpdateInterview,
                    Candidacy = UpdateCand.ToString(),
                    IdClassification = UpdateEvaluation,
                    CandidateCode = UpdateCandidateCode,
                    IdNationality = EditNationality,
                    BirthDate = EditBirthDate,
                };

                int resultGross;
                int resultNet;
                int resultDR;

                int.TryParse(UpdateGross, out resultGross);
                int.TryParse(UpdateNET, out resultNet);
                int.TryParse(UpdateDailyGains, out resultDR);

                CandidateEdit = CheckIfRemunerationsAreNull(CandidateEdit, resultGross, resultDR, resultNet);


                CandidateAndRoleAndSkill CandidateRnSEdit = new CandidateAndRoleAndSkill()
                {
                    IdCRNS = int.Parse(crns),
                    DateStart = UpdateInit,
                    DateFinish = UpdateEnd,
                    Description = UpdateDesc,
                    MainExperience = UpdateMainxp,
                };

                RoleAndSkill RnSEdit = new RoleAndSkill()
                {
                    Id = int.Parse(RnS),
                    IdRole = int.Parse(UpdateRole),
                    IdSkill = int.Parse(UpdateSkill),
                };

                CandidateNotes DescriptionEdit = new CandidateNotes()
                {
                    IdCandidate = int.Parse(id),
                    Note = UpdateDesc,
                    TimeStamp = DateTime.Now.ToString(),
                };

                if (UpdateGrade != null)
                    RnSEdit.IdGrade = int.Parse(UpdateGrade);

                controller.EditCandidate(GetLoggedInUser(), GetCurrentPage(), CandidateEdit, CandidateRnSEdit, RnSEdit, DescriptionEdit);
                LoggingAction("R&S-US.002", "Update", "", "", new Exception("EditCandidate Succeeded"));
                return RedirectToAction("Candidates", controller);
            }
            catch (Exception e)
            {
                LoggingAction("R&S-US.002", "Update", "", "", new Exception("EditCandidate Failed :-:" + e.Message));
                return RedirectToAction("ErrorPage",this);
            }
        }
        
        //GET: GetCandidateNotes
        public ActionResult JSONGetCandidateNotes()
        {
            try
            {
                currentPage = "Candidates";
                AdnDbController controller = new AdnDbController();
                List<CandidateNotes> list = controller.GetCandidateNotes();
                LoggingAction("R&S-US.002", "GET", "", "", new Exception("JSONGetCandidateNotes Succeeded"));
                return Json(list, JsonRequestBehavior.AllowGet); ;
            }
            catch (Exception e)
            {
                LoggingAction("R&S-US.002", "GET", "", "", new Exception("JSONGetCandidateNotes Failed :-:" + e.Message));
                return RedirectToAction("ErrorPage", this);
            }
        }

        //POST: AddNoteAction
        public Boolean AddNoteAction(int IdCandidateNote, string AddNote, string CandidateName)
        {
            try
            {
                CandidateNotes note = new CandidateNotes()
                {
                    IdCandidate = IdCandidateNote,
                    CandidateName = CandidateName,
                    Note = AddNote,
                    Active = 1,
                    IdUser = int.Parse(Session["IdUser"].ToString()),
                };

                AdnDbController controller = new AdnDbController();

                controller.AddCandidateNote(note, Session["User"].ToString(), currentPage);
                //controller.EmailSend(currentPage, "Create", CandidateName + "<br/>" + Session["User"].ToString() + "<br/>" + note.Note);

                LoggingAction("R&S-US.002", "POST", "", "", new Exception("AddNoteAction Succeeded"));
                return true;
            }
            catch (Exception e)
            {
                LoggingAction("R&S-US.002", "POST", "", "", new Exception("AddNoteAction Failed :-:" + e.Message));
                return false;
            }
        }

        public bool EditCandidateNoteAction(int IdNoteEdit, string EditNote, string OldNote, string CandidateName)
        {
            try
            {
                CandidateNotes note = new CandidateNotes()
                {
                    CandidateName = CandidateName,
                    IdCandidateDescription = IdNoteEdit,
                    IdUser = int.Parse(Session["IdUser"].ToString()),
                    Note = EditNote,
                };

                AdnDbController controller = new AdnDbController();

                controller.EditCandidateNote(note, OldNote, Session["User"].ToString(), currentPage);

                LoggingAction("R&S-US.002", "POST", "", "", new Exception("AddNoteAction Succeeded"));
                return true;
            }
            catch (Exception e)
            {
                LoggingAction("R&S-US.002", "POST", "", "", new Exception("AddNoteAction Failed :-:" + e.Message));
                RedirectToAction("ErrorPage", this);
                return false;
            }
        }

        //POST: DeleteNoteAction
        public bool DeleteNoteAction(int IdNote, string CandidateName , string OldNote)
        {
            try
            {
                AdnDbController controller = new AdnDbController();
                controller.DeleteCandidateNote(IdNote, CandidateName, OldNote, Session["User"].ToString(), currentPage);

                LoggingAction("R&S-US.002", "POST", "", "", new Exception("DeleteNoteAction Succeeded"));
                return true;
            }
            catch (Exception e)
            {
                LoggingAction("R&S-US.002", "POST", "", "", new Exception("DeleteNoteAction Failed :-: " + e.Message));
                return false;
            }
        }

        //POST: AddExp
        public int AddExp(int IdCandidateAddExp, int IdRoleAddExp, int IdSkillAddExp, string StartAddExp, string FinishAddExp, int IdGradeAddExp, string DescAddExp, int MainEXPAddExp, string CandidateName, string newRole, string newSkill)
        {
            try
            {
                AdnDbController controller = new AdnDbController();

                RoleAndSkill Rns = new RoleAndSkill()
                {
                    IdRole = IdRoleAddExp,
                    IdSkill = IdSkillAddExp,
                    IdGrade = IdGradeAddExp,
                };

                controller.CreateRoleAndSkillSingle(Rns);
                int i = controller.GetLastRoleAndSKillId();

                CandidateAndRoleAndSkill Crns = new CandidateAndRoleAndSkill()
                {
                    IdRolesAndSkills = i,
                    IdCandidate = IdCandidateAddExp,
                    DateStart = StartAddExp,
                    DateFinish = FinishAddExp,
                    Description = DescAddExp,
                    MainExperience = MainEXPAddExp,
                };

                controller.CreateCandRandS(Crns);
                controller.GatherInfoExp(Rns, Crns, currentPage, Session["User"].ToString(), CandidateName, newRole, newSkill);

                LoggingAction("R&S-US.002", "Create", "", IdCandidateAddExp + ",", new Exception("EditExp Succeeded"));
                //return RedirectToAction("Candidates", this);
                return 1;
            }
            catch (Exception e)
            {
                LoggingAction("R&S-US.002", "Create", "", IdCandidateAddExp + ",", new Exception("EditExp Failed :-:"+e.Message));
                //return RedirectToAction("ErrorPage", this);
                return 0;
            }
        }
            
        //POST: EDITCandidate
        public int EDITCandidate(int id, string UpdateName, string UpdateDesc, string? UpdateGross, int UpdateAvaiability, int UpdateEvaluation, string? UpdateNET, 
            string? UpdateDailyGains, string? UpdateRemunerationNotes, int UpdateCurrentPlace, string? UpdateInterview, int UpdateStatus, int UpdateCand, string UpdateCandidateCode, 
            int EditNationality, string EditBirthDate, int UpdateContactNumber, string UpdateEmail)
        {
            try
            {
                Candidate update = new Candidate()
                {
                    IdCandidate = id,
                    IdClassification = UpdateEvaluation,
                    IdNationality = EditNationality,
                    IdAvailability = UpdateAvaiability,
                    IdStatus = UpdateStatus,
                    Candidacy = UpdateCand.ToString(),
                    CurrentPlace = UpdateCurrentPlace,
                    Name = UpdateName,
                    CandidaqteDescription = UpdateDesc,
                    Interview = UpdateInterview,
                    CandidateCode = UpdateCandidateCode,
                    RemunerationNotes = UpdateRemunerationNotes,
                    BirthDate = EditBirthDate,
                    Description = UpdateDesc,
                    ContactNumber = UpdateContactNumber,
                    Email = UpdateEmail,
                };

                int resultGross = 0;
                int resultNet = 0;
                int resultDR = 0;

                int.TryParse(UpdateGross, out resultGross);
                int.TryParse(UpdateNET, out resultNet);
                int.TryParse(UpdateDailyGains, out resultDR);

                update = CheckIfRemunerationsAreNull(update, resultGross, resultDR, resultNet);

                AdnDbController controller = new AdnDbController();
                controller.EditCandidateONLY(update, "Candidates", Session["User"].ToString());

                LoggingAction("R&S-US.002", "Edit", "", ",", new Exception("EditCandidate Succeeded"));
                return 1;
            }
            catch (Exception e)
            {
                LoggingAction("R&S-US.002", "Edit", "", ",", new Exception("EditCandidate Failed :-:" + e.Message));
                //return RedirectToAction("ErrorPage",this);
                return 0;
            }
        }

        //POST: EditExp
        public int EditExp(int IdRnSERNS, int IdECRnS, int IdCandidateECRnS, int IdRoleERNS, int IdSkillERNS,
            string StartERNS, string FinishERNS, int IdGradeERNS, string DescERNS, int MainEXPRNS,
                string textSkill, string textRole, string textGrade, string CandidateName)
        {
            try
            {
                AdnDbController controller = new AdnDbController();
                controller.EditRoleAndSkill(IdRnSERNS, IdRoleERNS, IdSkillERNS, IdGradeERNS);
                controller = new AdnDbController();
                controller.EditCandidateRoleAndSkill(IdECRnS, IdCandidateECRnS, StartERNS, FinishERNS, DescERNS, MainEXPRNS, "Candidates", Session["User"].ToString(), textSkill, textRole, textGrade, CandidateName);
                
                LoggingAction("R&S-US.002", "Update", "", IdRnSERNS + ",", new Exception("EditExp Succeeded"));
                return 1;
            }
            catch (Exception e)
            {
                LoggingAction("R&S-US.002", "Update", "", IdRnSERNS + ",", new Exception("EditExp Failed :-:"+e.Message));
                return 0;
            }
        }

        //POST: SearchCandidateAction *-> FOR WP1 release <-*//
        //public ActionResult SearchCandidateAction(string Role, string Skill, string Name)
        //{
        //    AdnDbController controller = new AdnDbController();
        //    ViewBag.RoleList = controller.GetListRoles();
        //    ViewBag.SkillList = controller.GetListSkills();

        //    ViewBag.ResultList = controller.SearchCandidatesBy(Role, Skill, Name).ToList();
            
        //    StartApp();

        //    if ((Role != "-" || Skill != "-" || Name != ""))
        //        return View(ViewBag.ResultList);
        //    else
        //        return RedirectToAction("SearchCandidate", controller);
        //}

        ////////////////////////////////////////////////////////////////
        ////Users///////////////////////////////////////////////////////

        //GETS: Users
        [HttpGet]
        public ActionResult Users()
        {
            StartApp();
            currentPage = "Users";
            ViewBag.UserRoles = GetUserRolesList();
            LoggingAction("Users-US.001", "Read", "","",ex);
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult JSONGetUsersList()
        {
            var result = GetUsersInfo();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //POST: CreateUserAction
        [HttpPost]
        public bool CreateUserAction(string Name, string Password, int IdCRole, string CreateRoleDescription) //no Futuro, usar os departments tambem
        {
            try
            {
                AdnDbController controller = new AdnDbController();

                User user = new User()
                {
                    Name = Name,
                    Password = Password,
                };

                controller.CreateUser(user);

                UserUserRoles uur = new UserUserRoles()
                {
                    IdUserRole = IdCRole,
                    IdUser = controller.GetLastUser(),
                    Description = CreateRoleDescription,
                };

                controller.CreateUserUserRole(uur, Session["User"].ToString());
                controller.EmailSendNotes("User", "Create", "New User has been created!<br />" + "Author: " + user + "<br /> Name: "+Name + "<br/>Pass: "+Password +"<br />Role: " + CreateRoleDescription);
                LoggingAction("Users-US.001", "Create", "", user.Name+","+user.Password+","+uur.IdUserRole, ex = new Exception("Create User Succeded"));
                return true;
            }
            catch (Exception e)
            {
                LoggingAction("Users-US.001", "Create", "", Name + "," + Password + "," + IdCRole, ex = new Exception("Create User Failed:-: " + e.Message));
                return false;
            }
        }

        [HttpPost]
        public bool EditUserAction(int IdUser, string editName, string editPass, int EditIdCRole, int Activated, string RoleText) //no Futuro, usar os departments tambem
        {
            try
            {
                AdnDbController controller = new AdnDbController();

                User user = new User()
                {
                    IdUser = IdUser,
                    Name = editName,
                    Password = editPass,
                    Activated = Activated,
                };

                UserUserRoles uur = new UserUserRoles()
                {
                    IdUser = IdUser,
                    IdUserRole = EditIdCRole,
                };

                controller.EditUser(user, uur);

                controller.EmailSendNotes("User", "Edit", "New User has been Edited!<br />" + "Author: " + user + "<br /> Name: " + editName + "<br/>Pass: " + editPass + "<br />Role: " + RoleText);
                LoggingAction("Users-US.001", "Edit", "", IdUser+","+editName+","+editPass+","+Activated, ex = new Exception("Edit User Succeded"));
                return true;
            }
            catch (Exception e)
            {
                LoggingAction("Users-US.001", "Edit", "", IdUser + "," + editName + "," + editPass + "," + Activated, ex = new Exception("Edit User Failed:-: " + e.Message));
                return false;
            }
        }

        //GET: UserRoles
        public ActionResult UserRoles()
        {
            try
            {
                StartApp();
                currentPage = "UserRoles";
                LoggingAction("Users", "Read", "", "", ex = new Exception("UserRoles Page Accessed"));
                return View();
            }
            catch(Exception e)
            {
                LoggingAction("Users", "Read", "", "", ex = new Exception("UserRoles Page Failed :-:"+e.Message));
                return RedirectToAction("ErrorPage",this);
            }
        }

        [HttpGet]
        public ActionResult JSONGetUserRolesList()
        {
            try
            {
                var result = GetUserRolesList();

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                LoggingAction("Users", "Read", "", "", ex = new Exception("UserRolesList Get Failed :-:"+e.Message));
                return View();
            }
        }

        [HttpPost]
        public ActionResult CreateUserRoleAction()
        {
            try
            {


                LoggingAction("Users", "Read", "", "", ex = new Exception("CreateUserRole Page Accessed"));
                return View();
            }
            catch(Exception e)
            {
                LoggingAction("Users", "Read", "", "", ex = new Exception("CreateUserRole Page Failed :-:" + e.Message));
                return RedirectToAction("ErrorPage", this);
            }
        }

        [HttpPost]
        public ActionResult EditUserRoleAction()
        {
            try
            {
                LoggingAction("Users", "Read", "", "", ex = new Exception("EditUserRole Page Accessed"));
                return View();
            }
            catch (Exception e)
            {
                LoggingAction("Users", "Read", "", "", ex = new Exception("EditUserRole Page Failed :-:" + e.Message));
                return RedirectToAction("ErrorPage", this);
            }
        }

        /// <summary>
        /// WorkPackage///////////////////////////////////////////////////////////////
        /// </summary>
        [HttpGet]
        public ActionResult WorkPackage()
        {
            StartApp();
            currentPage = "WorkPackage";
            LoggingAction("Users", "Read", "", "", ex = new Exception("WorkPackage Page Accessed"));
            return View();
        }

        [HttpGet]
        public ActionResult JSONGetWorkPackagesList()
        {
            try
            {
                var result = GetListWP();
                LoggingAction("Users", "Read", "", "", new Exception("GetWorkPackagesList Succeeded"));
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                LoggingAction("Users","Read","","",new Exception("GetWorkPackagesList Failed :-:"+e.Message));
                return View("ErrorPage");
            }
        }

        [HttpPost]
        public ActionResult CreateWorkPackage(string newWP, string newDescription)
        {
            AdnDbController controller = new AdnDbController();

            try
            {
                controller.CreateWorkPackage(newWP, newDescription);
                

                LoggingAction("Users", "Read", "", "", new Exception("CreateWorkPackages Succeeded"));
            }
            catch (Exception e)
            {
                LoggingAction("Users", "Read", "", "", new Exception("CreateWorkPackages Failed :-:" + e.Message));
            }

            return RedirectToAction("WorkPackage",this);
        }

        /// <summary>
        /// Customers////////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>

        [HttpGet]
        public ActionResult Customers()
        {
            StartApp();
            currentPage = "Customers";
            LoggingAction("Leads-US.0014", "Read", "", "", new Exception("Get Customers Page Succedded"));
            return View();
        }

        [HttpGet]
        public ActionResult JSONGetCustomersList()
        {
            try
            {
                AdnDbController controller = new AdnDbController();
                return Json(controller.GetListCustomers(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                LoggingAction("Leads-US.0014", "Read", "", "", new Exception("Get Customers List Failed :-:" + e.Message));
                return RedirectToAction("ErrorPage", this);
            }
        }

        [HttpPost]
        public ActionResult CreateCustomer(string ClientName, int NIF, string RepresentativeName, string Adress, string Email, string FinancialDepartmentEmail)
        {
            try
            {
                AdnDbController controller = new AdnDbController();
                controller.createCustomer(ClientName, NIF, RepresentativeName, Adress, Email, FinancialDepartmentEmail);
                LoggingAction("Leads-US.0014", "Create", "", "", new Exception("Get Customers List Failed :-:" + new Exception("CreateCustomer Succeeded")));
                return RedirectToAction("Customers", this);
            }
            catch(Exception e)
            {
                LoggingAction("Leads-US.0014", "Create", "", "", new Exception("Get Customers List Failed :-:" + new Exception("CreateCustomer Failed" + e.Message)));
                return RedirectToAction("ErrorPage", this);
            }
        }

        [HttpPost]
        public ActionResult EditCustomer(int oldId, string oldClientName, int oldNIf, string oldLegalRepresentative, string oldAdress, string oldEmail, string oldFinancialDepartmentEmail, int oldActive)
        {
            try
            {
                AdnDbController controller = new AdnDbController();
                controller.editCustomer(oldId, oldClientName, oldNIf, oldLegalRepresentative, oldAdress, oldEmail, oldFinancialDepartmentEmail, oldActive);
                LoggingAction("Leads-US.0014", "Create", "", oldId+","+ oldClientName + "," + oldNIf + "," + oldLegalRepresentative + "," + oldAdress + "," + oldEmail + "," + oldEmail +","+ oldFinancialDepartmentEmail + ","+ oldActive
                    , new Exception("Get Customers List Failed :-:" + new Exception("CreateCustomer Succeeded")));
                return RedirectToAction("Customers", this);
            }
            catch (Exception e)
            {
                LoggingAction("Leads-US.0014", "Create", "", "", new Exception("Get Customers List Failed :-:" + new Exception("CreateCustomer Failed" + e.Message)));
                return RedirectToAction("ErrorPage", this);
            }
        }

        /// <summary>
        /// Leads////////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>

        //GET: Leads
        [HttpGet]
        public ActionResult Leads()
        {
            try
            {
                StartApp();
                AdnDbController controller = new AdnDbController();
                ViewBag.Customers = controller.GetListCustomers();
                ViewBag.Skills = controller.GetListSkills();
                ViewBag.Nationalities = controller.GetListNationalities();
                ViewBag.LeadStatus = controller.getLeadStatusList();
                ViewBag.Roles = controller.GetListRoles();
                ViewBag.Availability = GetListAvailability();
                currentPage = "Leads";
                LoggingAction("Leads-US.0014", "Read","","",new Exception("Leads Page Succedded"));
                return View();
            }
            catch (Exception e)
            {
                LoggingAction("Leads-US.0014", "Read","","",new Exception("Leads Page Failed :-:" + e.Message));
                return RedirectToAction("ErrorPage", this);
            }
        }

        //GET: JSONGetLeadList
        [HttpGet]
        public ActionResult JSONGetLeadsList() 
        {
            try
            {
                currentPage = "Leads";
                AdnDbController controller = new AdnDbController();
                List<LeadsFullInfo> List = controller.GetLeadsList();
                LoggingAction("Leads-US.0014", "Read", "", "", new Exception("JSONGetLeadsList Succedded"));

                return Json(List, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                LoggingAction("Leads-US.0014", "Read", "", "", new Exception("JSONGetLeadsList Failed :-:" + e.Message));
                return RedirectToAction("ErrorPage", this);
            }
        }

        //POST: CreateLeadAction
        [HttpPost]
        public ActionResult CreateLeadAction(int createLeadCustomer, int createLeadSkill, int? createLeadSkillEXP, int createLeadRole, int? createLeadRoleExp, int? createLeadMinXP, string createLeadCode,
            int? createLeadMaxXP, int? createLeadMinAge, int? createLeadMaxAge, int createLeadNationality, int? createLeadMinSal, int? createLeadMaxSal, int createLeadAvailability, string createDescription)
        {
            try
            {
                AdnDbController controller = new AdnDbController();

                Leads lead = new Leads()
                {
                    IdCustomer = createLeadCustomer,
                    IdLeadStatus = 1,
                    Description = createDescription,
                    IdSkill = createLeadSkill,
                    IdRole = createLeadRole,
                    IdNationality = createLeadNationality,
                    IdAvailability = createLeadAvailability,
                    LeadCode = createLeadCode,
                };

                lead = checkIfRigth(lead, createLeadSkillEXP, createLeadRoleExp, createLeadMaxAge, createLeadMinAge, createLeadMinSal, createLeadMaxSal, createLeadMaxXP, createLeadMinXP);

                controller.CreateFullLead(lead);

                LoggingAction("Leads-US.0014", "Create", "", createLeadCustomer + "," + createLeadSkill + "," + createLeadSkillEXP + "," + createLeadRole + "," + createLeadRoleExp + "," +
                    createLeadMinXP + "," + createLeadMaxXP + "," + createLeadMinAge + "," + createLeadMaxAge + "," + createLeadNationality + "," + createLeadSkill + "," + createLeadSkill, new Exception("CreateLeadAction Succeeded"));
                
                return RedirectToAction("Leads",this);
            }
            catch (Exception e)
            {
                LoggingAction("Leads-US.0014","Create","", createLeadCustomer +","+ createLeadSkill + "," + createLeadSkillEXP + "," + createLeadRole + "," + createLeadRoleExp + "," +
                    createLeadMinXP + "," + createLeadMaxXP + "," + createLeadMinAge + "," + createLeadMaxAge + "," + createLeadNationality + "," + createLeadSkill + "," + createLeadSkill, 
                    new Exception("CreateLeadAction Failed :-:"+ e.Message));

                return RedirectToAction("ErrorPage", this);
            }
        }

        private Leads checkIfRigth(Leads lead, int? createLeadSkillEXP, int? createLeadRoleExp, int? createLeadMaxAge, int? createLeadMinAge, int? createLeadMinSal, int? createLeadMaxSal, int? createLeadMaxXP, 
            int? createLeadMinXP)
        {
            Leads package = lead;

            if (createLeadSkillEXP == null)
                package.SkillExp = 0;
            else
                package.SkillExp = int.Parse(createLeadSkillEXP.ToString());
            if (createLeadRoleExp == null)
                package.RoleExp = 0;
            else
                package.RoleExp = int.Parse(createLeadRoleExp.ToString());
            if (createLeadMaxAge == null)
                package.MaxAge = 0;
            else
                package.MaxAge = int.Parse(createLeadMaxAge.ToString());
            if (createLeadMinAge == null)
                package.MinAge = 0;
            else
                package.MinAge = int.Parse(createLeadMinAge.ToString());
            if (createLeadMinSal == null)
                package.MinRemuneartion = 0;
            else
                package.MinRemuneartion = int.Parse(createLeadMinSal.ToString());
            if (createLeadMaxSal == null)
                package.MaxRemuneartion = 0;
            else
                package.MaxRemuneartion = int.Parse(createLeadMaxSal.ToString());
            if (createLeadMaxXP == null)
                package.MaxProfExp = 0;
            else
                package.MaxProfExp = int.Parse(createLeadMaxXP.ToString());
            if (createLeadMinXP == null)
                package.MinProfExp = 0;
            else
                package.MinProfExp = int.Parse(createLeadMinXP.ToString());

            return package;
        }

        [HttpPost]
        public ActionResult EditLeadAction(int editLeadCustomer, string editLeadCode, int editLeadSkill, int? editLeadSkillEXP, int editLeadRole, int? editLeadRoleExp, int? editLeadMinXP,
            int? editLeadMaxXP, int? editLeadMinAge, int? editLeadMaxAge, int editLeadNationality, int? editLeadMinSal, int? editLeadMaxSal, int editLeadAvailability, int editLeadStatus,
            string editDescription, int IdLead)
        {
            try
            {
                string Name = Convert.ToString(Session["User"]);
                Leads update = new Leads()
                {
                    IdLead = IdLead,
                    IdCustomer = editLeadCustomer,
                    IdLeadStatus = editLeadStatus,
                    Description = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss") + " " + Name + Environment.NewLine + editDescription,
                    IdSkill = editLeadSkill,
                    IdRole = editLeadRole,
                    IdNationality = editLeadNationality,
                    IdAvailability = editLeadAvailability,
                    LeadCode = editLeadCode,
                };

                update = checkIfRigth(update, editLeadSkillEXP, editLeadRoleExp, editLeadMaxAge, editLeadMinAge, editLeadMinSal, editLeadMaxSal, editLeadMaxXP, editLeadMinXP);

                AdnDbController controller = new AdnDbController();
                controller.EditFullLead(update);

                LoggingAction("Leads-US.0014", "Update", "", editLeadCustomer + "," + editLeadSkill + "," + editLeadSkillEXP + "," + editLeadRole + "," + editLeadRoleExp + "," +
                    editLeadMinXP + "," + editLeadMaxXP + "," + editLeadMinAge + "," + editLeadMaxAge + "," + editLeadNationality + "," + editLeadSkill + "," + editLeadSkill, new Exception("CreateLeadAction Succeeded"));

                return RedirectToAction("Leads", this);
            }
            catch (Exception e)
            {
                LoggingAction("Leads-US.0014", "Update", "", editLeadCustomer + "," + editLeadSkill + "," + editLeadSkillEXP + "," + editLeadRole + "," + editLeadRoleExp + "," +
                    editLeadMinXP + "," + editLeadMaxXP + "," + editLeadMinAge + "," + editLeadMaxAge + "," + editLeadNationality + "," + editLeadSkill + "," + editLeadSkill,
                    new Exception("CreateLeadAction Failed :-:" + e.Message));

                return RedirectToAction("ErrorPage", this);
            }
        }

        [HttpGet]
        public ActionResult JSONGetLeadsNotes()
        {
            try
            {
                currentPage = "Leads";
                AdnDbController controller = new AdnDbController();
                LoggingAction("Leads-US.0014", "Read", "", "", new Exception("JSONGetLeadsNotes Succeeded"));
                return Json(controller.GetListLeadsNotes(), JsonRequestBehavior.AllowGet); ;
            }
            catch (Exception e)
            {
                LoggingAction("Leads-US.0014", "Read", "", "", new Exception("JSONGetLeadsNotes Failed :-:" + e.Message));

                return RedirectToAction("ErrorPage", this);
            }
        }

        [HttpPost]
        public bool CreateLeadNoteAction(int IdLead, string AddNoteText)
        {
            try
            {
                currentPage = "Leads";
                AdnDbController controller = new AdnDbController();

                LeadNotes note = new LeadNotes()
                {
                    IdLead = IdLead,
                    Description = AddNoteText,
                    Active = 1,
                    IdUser = int.Parse(Session["IdUser"].ToString()),
                };

                controller.CreateLeadNote(note, Session["User"].ToString(), currentPage);
                LoggingAction("Leads-US.0014", "Create", "", "", new Exception("CreateLeadNoteAction Succeeded"));
                return true;
            }
            catch(Exception e)
            {
                LoggingAction("Leads-US.0014", "Create", "", "", new Exception("CreateLeadNoteAction Failed :-:" + e.Message));
                return false;
            }
        }

        [HttpPost]
        public bool EditLeadNoteAction(int IdNote, string EditNote, string OldNote)
        {
            try
            {
                currentPage = "Leads";
                AdnDbController controller = new AdnDbController();

                LeadNotes note = new LeadNotes()
                {
                    IdLeadNote = IdNote,
                    Description = EditNote,
                    IdUser = int.Parse(Session["IdUser"].ToString()),
                };

                controller.EditLeadNote(note, Session["User"].ToString(), currentPage, OldNote);

                LoggingAction("Leads-US.0014", "Update", "", "", new Exception("EditLeadNoteAction Succeeded"));
                return true;
            }
            catch(Exception e)
            {
                LoggingAction("Leads-US.0014", "Update", "", "", new Exception("EditLeadNoteAction Failed :-: " + e.Message));
                return false;
            }
        }

        [HttpPost]
        public bool DeleteLeadNoteAction(int IdLeadNote, string oldNote)
        {
            try 
            { 
                currentPage = "Leads";
                AdnDbController controller = new AdnDbController();
                controller.DeleteLeadNote(IdLeadNote, Session["User"].ToString(), oldNote, currentPage);
                LoggingAction("Leads-US.0014", "Update", "", "", new Exception("DeleteLeadNoteAction Succeeded"));
                return true;
            }
            catch (Exception e)
            {
                LoggingAction("Leads-US.0014", "Update", "", "", new Exception("DeleteLeadNoteAction Failed :-: " + e.Message));
                return false;
            }
        }

        [HttpPost]
        public void SendCandidatesLead(string leadCode, string[] box, int idLead)
        {
            try
            {
                List<CandidateFullInfo> List = new List<CandidateFullInfo>();

                //I most surtenly understand this part is useless extra code.... Cold just send box and it was alright i think
                foreach (var item in box)
                {
                    CandidateFullInfo Candidate = new CandidateFullInfo();
                    string[] result = item.Split(',');
                    Candidate.CandidateCode = result[0];
                    Candidate.Name = result[1];
                    Candidate.NationalityDescription = result[2];
                    Candidate.Role = result[3];
                    Candidate.Skill = result[4];
                    Candidate.GrossRemuneration = result[5];
                    Candidate.Availability = result[6];
                    Candidate.CandidateDescription = result[7];
                    Candidate.IdCandidate = int.Parse(result[8]);
                    Candidate.IdLeadCandidateStatus = 1;

                    List.Add(Candidate);
                }

                AdnDbController controller = new AdnDbController();
                controller.FormCandidateListMail(leadCode, List, idLead);
                LoggingAction("Leads-US.0014", "Update", "", "", new Exception("SendCandidatesLead Succeeded"));
            }
            catch (Exception e)
            {
                LoggingAction("Leads-US.0014", "Update", "", "", new Exception("SendCandidatesLead Failed :-: " + e.Message));
            }
        }

        public ActionResult JSONGetCandidateLeadStatus()
        {
            List<CandidateLeadStatus> list = null;
            try
            {
                AdnDbController controller = new AdnDbController();
                list = controller.GetCandidateLeadStatusList();
                LoggingAction("Leads-US.0014", "Read", "", "", new Exception("JSONGetCandidateLeadStatus succeded"));
                return Json(list, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                LoggingAction("Leads-US.0014", "Read", "", "", new Exception("JSONGetCandidateLeadStatus Failed :-: " + e.Message));
                return Json(list, JsonRequestBehavior.AllowGet); ;
            }
        }

        public bool ChangeCandidatesCandidateLeadStatus(int idlead, int idcandidate, int idcls)
        {
            try
            {
                LeadsCandidates lc = new LeadsCandidates()
                {
                    IdLead = idlead,
                    IdCandidate = idcandidate,
                    IdCandidateLeadStatus = idcls,
                };

                AdnDbController controller = new AdnDbController();
                controller.ChangeLeadCandidateStatus(lc);
                LoggingAction("Leads-US.0014", "Read", "", "", new Exception("JSONGetCandidateLeadStatus succeded"));
                return true;
            }
            catch (Exception e)
            {
                LoggingAction("Leads-US.0014", "Read", "", "", new Exception("ChangeCandidatesCandidateLeadStatus Failed :-: " + e.Message));
                return false;
            }
        }

        /// <summary>
        /// LeadsStatus//////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>

        [HttpGet]
        public ActionResult LeadStatus()
        {
            try
            {
                StartApp();
                currentPage = "LeadStatus";
                LoggingAction("Leads-US.0014","Read", "","",new Exception("Get LeadStatus page Succeeded"));
                return View();
            }
            catch(Exception e)
            {
                LoggingAction("Leads-US.0014", "Read", "", "", new Exception("Get LeadStatus page Failed :-:" + e.Message));
                return RedirectToAction("ErrorPage", this);
            }
        }   

        [HttpGet]
        public ActionResult JSONLeadStatusList()
        {
            try
            {
                AdnDbController controller = new AdnDbController();
                LoggingAction("Leads-US.0014", "Read", "", "", new Exception("Get LeadStatus List Succeeded"));
                return Json(controller.getLeadStatusList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                LoggingAction("Leads-US.0014", "Read", "", "", new Exception("Get LeadStatus List Failed :-:" + e.Message));
                return RedirectToAction("ErrorPage", this);
            }
        }

        [HttpPost]
        public ActionResult CreateLeadStatusAction(string description, string R, string G, string B)
        {
            try
            {
                AdnDbController controller = new AdnDbController();
                controller.CreateLeadStatus(description, R,G,B);
                LoggingAction("Leads-US.0014", "Create", "", description +", "+R+G+B+" ,"+1.ToString(), new Exception("Create LeadStatus Succeeded"));
                return RedirectToAction("LeadStatus", this);
            }
            catch (Exception e)
            {
                LoggingAction("Leads-US.0014", "Create", "", "", new Exception("Create LeadStatus Failed :-:" + e.Message));
                return RedirectToAction("ErrorPage", this);
            }
        }

        [HttpPost]
        public ActionResult EditLeadStatusAction(int IdStatus,string eDescription, string eR, string eG, string eB, int Active)
        {
            try
            {
                AdnDbController controller = new AdnDbController();
                controller.EditLeadStatus(IdStatus, eDescription, eR, eG, eB, Active);
                LoggingAction("Leads-US.0014", "Create", "", "", new Exception("Create LeadStatus Succeeded"));
                return RedirectToAction("LeadStatus", this);
            }
            catch (Exception e)
            {
                LoggingAction("Leads-US.0014", "Create", "", "", new Exception("Create LeadStatus Failed :-:" + e.Message));
                return RedirectToAction("ErrorPage", this);
            }
        }

        /// <summary>
        /// Nationalities////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        
        //GET: Nationalities
        [HttpGet]
        public ActionResult Nationalities()
        {
            try
            {
                StartApp();
                LoggingAction("Leads-US.0014", "Read", "", "", new Exception("Get Nationalities Succeeded"));
                return View();
            }
            catch (Exception e)
            {
                LoggingAction("Leads-US.0014", "Read", "", "", new Exception("Get Nationalities Failed :-:" + e.Message));
                return RedirectToAction("ErrorPage", this);
            }
        }

        [HttpGet]
        public ActionResult JSONNationalitiesList()
        {
            try
            {
                AdnDbController controller = new AdnDbController();
                
                LoggingAction("Leads-US.0014", "Read", "", "", new Exception("Get Nationalities List Succeeded"));
                return Json(GetListNationalities(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                LoggingAction("Leads-US.0014", "Read", "", "", new Exception("Get Nationalities Failed :-:" + e.Message));
                return RedirectToAction("ErrorPage", this);
            }
        }

        [HttpPost]
        public ActionResult CreateNationalityAction(string newNationality, int NewforCandidates, int NewforLeads)
        {
            try
            {
                AdnDbController controller = new AdnDbController();
                controller.CreateNationality(newNationality, NewforCandidates, NewforLeads);
                LoggingAction("Leads-US.0014", "Create", "", newNationality+","+NewforCandidates+","+NewforLeads, new Exception("Create Nationalities Succeeded"));
                return RedirectToAction("Nationalities", this);
            }
            catch (Exception e)
            {
                LoggingAction("Leads-US.0014", "Create", "", newNationality + "," + NewforCandidates + "," + NewforLeads, new Exception("Create Nationalities Failed :-:"+e.Message));
                return RedirectToAction("ErrorPage", this);
            }
        }

        [HttpPost]
        public ActionResult EditNationalityAction(int oldId, string oldNationality, int oldforCandidates, int oldforLeads, int oldActive)
        {
            try
            {
                AdnDbController controller = new AdnDbController();
                controller.EditNationality(oldId, oldNationality, oldforCandidates, oldforLeads, oldActive);
                LoggingAction("Leads-US.0014", "Create", "", oldId + "," + oldNationality + "," + oldforCandidates + "," + oldforLeads + "," + oldActive, new Exception("Edit Nationalities Succeeded"));
                return RedirectToAction("Nationalities", this);
            }
            catch (Exception e)
            {
                LoggingAction("Leads-US.0014", "Create", "", oldId + "," + oldNationality + "," + oldforCandidates + "," + oldforLeads + "," + oldActive, new Exception("Edit Nationalities Failed :-:" + e.Message));
                return RedirectToAction("ErrorPage", this);
            }
        }
        /// <summary>
        /// LogIn////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>

        //GET: LogIn
        [HttpGet]
        public ActionResult LogIn()
        {
            try
            {
                GetUsersAndWP();

                string session = Convert.ToString(Session["User"]);
                if (session == "Guest" || session == "")
                {
                    StartApp();
                    ViewBag.FailedLogInAttempt = "0";
                    currentPage = "LogIn";
                    LoggingAction("Users-US.001", "Read", "", "", ex = new Exception("LogIn Page Accessed"));
                    return View();
                }
                else
                    return RedirectToAction("Index", this);
            }
            catch(Exception e)
            {
                LoggingAction("Users-US.001", "Read", "", "", ex = new Exception("LogIn Page Failed :-:" + e.Message));
                return RedirectToAction("ErrorPage",this); ;
            }
        }

        [HttpPost]
        public int LogInAction(string user, string password)
        {
            AdnController controller = new AdnController();
            try
            {
                List<User> Users = GetUsersInfo();
                currentPage = "LogIn";
                
                foreach (var item in Users)
                {
                    if (user == item.Name && password == item.Password)
                    {
                        if (!Request.IsAuthenticated)
                        {
                            ViewBag.UserInfo = item;
                            Session["User"] = item.Name;
                            Session["IdUser"] = item.IdUser;
                            ViewBag.loggedInUser = item;
                            LoggingAction("LogIn - US.001", "Read", "", string.Concat(ViewBag.loggedInUser.Name, ",", ViewBag.loggedInUser.IdUser), ex = new Exception("LogIn Allowed Credentials"));
                            //return RedirectToAction("Index", this);
                            return -1;
                        }
                    }
                    else
                    {
                        User guest = new User()
                        {
                            Name = "Guest",
                            IdUser = -1,
                        };

                        ViewBag.loggedInUser = guest;
                    }
                }
                
                ViewBag.FailedLogInAttempt += 1.ToString();
                LoggingAction("LogIn - US.001", "Read", "", string.Concat(ViewBag.loggedInUser.Name, ",", ViewBag.loggedInUser.IdUser), ex = new Exception("LogIn Failed Credentials"));
                return int.Parse(ViewBag.FailedLogInAttempt);
            }
            catch(Exception e)
            {
                if(ViewBag.loggedInUser == null)
                    LoggingAction("LogIn - US.001", "Read", string.Concat("Guest", ",", ViewBag.loggedInUser.IdUser), string.Concat(ViewBag.loggedInUser.Name, ",", ViewBag.loggedInUser.IdUser), ex = new Exception("LogIn Failed:-:"+e.Message));
                else
                    LoggingAction("LogIn - US.001", "Read", string.Concat(ViewBag.loggedInUser.Name, ",", ViewBag.loggedInUser.IdUser), string.Concat(ViewBag.loggedInUser.Name, ",", ViewBag.loggedInUser.IdUser), ex = new Exception("LogIn Failed:-:"+e.Message));
                return int.Parse(ViewBag.FailedLogInAttempt);
            }
        }

        [HttpGet]
        public ActionResult Configurations()
        {
            try
            {
                StartApp();
                LoggingAction("Leads-US.0014", "Read", "", "", new Exception("Get Congifurations Succeeded"));
                return View();
            }
            catch (Exception e)
            {
                LoggingAction("Leads-US.0014", "Read", "", "", new Exception("Get Congifurations Failed :-:" + e.Message));
                return RedirectToAction("ErrorPage", this);
            }
        }

        [HttpGet]
        public ActionResult GetLogPath()
        {
            AdnDbController controller = new AdnDbController();
            LoggingAction("Leads-US.0014", "Read", "", "", new Exception("GetLogPath Succeeded"));

            return Json(controller.GetLogPath(), JsonRequestBehavior.AllowGet); ;
        }

        public bool ReadFileAction(string path)
        {
            try
            {
                AdnDbController controller = new AdnDbController();
                List<string> Upload = controller.CSVFileReader(path,1);
                int state = 0;
                foreach (var item in Upload)
                {
                    string[] candidate = item.Split(',');

                    for(int i = 0; i < candidate.Length; i++)
                    {
                        if (i == 8 || i == 11 || i == 17 || i == 20 || i == 29)
                        {
                            if (candidate[i] == "") { candidate[i] = "0"; }
                        }
                    }
                    state = CreateCandidateFullCVAction(candidate[0], int.Parse(candidate[1]), int.Parse(candidate[2]), candidate[3], candidate[4], int.Parse(candidate[5]), candidate[6], int.Parse(candidate[7]), 
                        int.Parse(candidate[8]), candidate[9], int.Parse(candidate[10]), int.Parse(candidate[11]), candidate[12], int.Parse(candidate[13]), int.Parse(candidate[14]), candidate[15],
                        int.Parse(candidate[16]), candidate[17], candidate[18], int.Parse(candidate[19]), candidate[20], candidate[21], candidate[22], candidate[23], candidate[24], candidate[25], 
                        candidate[26], candidate[27], candidate[28], int.Parse(candidate[29].ToString()), candidate[23]);
                }
                if (state == 1) return true;
                else return false;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        /// <summary>
        /// Departements and company////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>

        [HttpGet]
        public ActionResult Departments()
        {
            return View();
        }

        [HttpGet]
        public ActionResult CompanyRoles()
        {
            return View();
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary> /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// GET LISTS SECTION AND TOOL BOX ////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary> ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        public void StartApp()
        {
            AdnDbController controller = new AdnDbController();
            ViewBag.AppVersion = app.GetVersion();
            ViewBag.Features = app.GetFeatures();
            ViewBag.Warning = app.GetWarning();
        }

        private List<CandidateFullInfo> GetListCandidatesAndInfo()
        {
            ViewBag.AppVersion = app.GetVersion();
            AdnDbController controller = new AdnDbController();
            IEnumerable<CandidateFullInfo> resultList = null;

            List<CandidateFullInfo> CandidateList = new List<CandidateFullInfo>();
            try
            {
                resultList = controller.GetListCandidateFullRS();

                foreach (var item in resultList)
                {
                    CandidateFullInfo candidateFull = new CandidateFullInfo()
                    {
                        IdRolesAndSkills = item.IdRolesAndSkills,
                        IdCRnS = item.IdCRnS,
                        IdCandidate = item.IdCandidate,
                        Name = item.Name,
                        IdRole = item.IdRole,
                        Role = item.Role,
                        DateStart = item.DateStart,
                        DateFinish = item.DateFinish,
                        IdSkill = item.IdSkill,
                        Skill = item.Skill,
                        SkillType = item.SkillType,
                        Description = item.Description,
                        ExpDescription = item.ExpDescription,
                        Grade = item.Grade,
                        Activated = item.Activated,
                        //GrossRemuneration = item.GrossRemuneration,
                        Availability = item.Availability,
                        IdAvailability = item.IdAvailability,
                        Classification = item.Classification,
                        UserEvaluation = item.UserEvaluation,
                        IdCandidacy = item.IdCandidacy,
                        IdStatus = item.IdStatus,
                        BirthDate = item.BirthDate,

                        //NET = item.NET,
                        CurrentPlace = item.CurrentPlace,
                        IdLocation = item.IdLocation,
                        //DailyGains = item.DailyGains,
                        Candidacy = item.Candidacy,
                        RemunerationNotes = item.RemunerationNotes,
                        Status = item.Status,
                        Interview = item.Interview,
                        //IdCandidacy = item.IdCandidacy,
                        MainExperience = item.MainExperience,
                        StatusColor = item.StatusColor,
                        CandidateCode = item.CandidateCode,
                        IdNationality = item.IdNationality,
                        NationalityDescription = item.NationalityDescription,
                        ContactNumber = item.ContactNumber,
                        Email = item.Email,
                    };
                    
                    CandidateFullInfo test = new CandidateFullInfo()
                    {
                        GrossRemuneration = item.GrossRemuneration,
                        NET = item.NET,
                        DailyGains = item.DailyGains,
                    };

                    //Candidate package = CheckIfRemunerationsAreNull(test, test.GrossRemuneration, test.NET, test.DailyGains);
                    CandidateFullInfo package = CheckIfRemunerationsAreZERO(test, test.GrossRemuneration, test.NET, test.DailyGains);
                    candidateFull.NET = package.NET.ToString();
                    candidateFull.GrossRemuneration = package.GrossRemuneration.ToString();
                    candidateFull.DailyGains = package.DailyGains.ToString();

                    candidateFull.Interview = CheckIfInterviewIsNULL(candidateFull.Interview);

                    candidateFull.Candidacy = item.Candidacy;

                    CandidateList.Add(candidateFull);
                }
                return CandidateList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private List<Role> GetListRoles()
        {
            
            IEnumerable<Role> resultList = null;

            List<Role> RoleList = new List<Role>();
            try
            {
                AdnDbController controller = new AdnDbController();
                resultList = controller.GetListRoles();

                foreach (var item in resultList)
                {
                    Role candidate = new Role
                    {
                        IdRole = item.IdRole,
                        NameRole = item.NameRole,
                        Active = item.Active,
                    };
                    RoleList.Add(candidate);
                }

                return RoleList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private List<Skills> GetListSkills()
        {
            AdnDbController controller = new AdnDbController();
            IEnumerable<Skills> resultList = null;
            IEnumerable<SkillCategory> resultCategories = null;

            List<Skills> SkillList = new List<Skills>();
            try
            {
                resultList = controller.GetListSkills();
                resultCategories = controller.GetCategoriesList();

                foreach (var item in resultList)
                {
                    Skills candidate = new Skills
                    {
                        IdSkill = item.IdSkill,
                        Name = item.Name,
                        IdCategory = item.IdCategory,
                        Active = item.Active,
                    };
                    SkillList.Add(candidate);
                }

                foreach (var item in SkillList)
                {
                    foreach(var target in resultCategories)
                    {
                        if (item.IdCategory == target.IdCategory)
                            item.NameCategory = target.NameCategory;
                    }
                }
                return SkillList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private List<SkillGrade> GetListGrades()
        {
            AdnDbController controller = new AdnDbController();
            IEnumerable<SkillGrade> resultList = null;

            List<SkillGrade> GradeList = new List<SkillGrade>();
            try
            {
                resultList = controller.GetListGrades();

                foreach (var item in resultList)
                {
                    SkillGrade candidate = new SkillGrade()
                    {
                        IdGrade = item.IdGrade,
                        Grade = item.Grade,
                    };
                    GradeList.Add(candidate);
                }

                return GradeList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private dynamic GetListCategory()
        {
            AdnDbController controller = new AdnDbController();

            try
            {
                var list = controller.GetCategoriesList();
                return list.ToList(); ;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        private List<RoleAndSkill> GetListRnS()
        {
            AdnDbController controller = new AdnDbController();

            try
            {
                var list = controller.GetListRolesAndSkills();
                return list.ToList(); ;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        private List<CandidateEvaluation> GetListUserEval()
        {
            AdnDbController controller = new AdnDbController();

            try
            {
                var list = controller.GetListCandidateEvaluation();
                return list.ToList(); ;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        private List<Contacts> GetListContacts()
        {
            List<Contacts> list = new List<Contacts>();
            return list;
        }

        private List<Candidacy> GetListCandidacy()
        {
           AdnDbController controller = new AdnDbController();
           return controller.GetListCandidacy();
        }

        private List<CandidateStatus> GetListCandidateStatus()
        {
            AdnDbController controller = new AdnDbController();

            return controller.GetListCandidateStatus();
        }

        private List<Availability> GetListAvailability()
        {
            AdnDbController controller = new AdnDbController();
            return controller.GetListAvailability();
        }

        private List<Location> GetListLocation()
        {
            AdnDbController controller = new AdnDbController();
            return controller.GetListLocations();
        }

        private List<Nationalities> GetListNationalities()
        {
            AdnDbController controller = new AdnDbController();
            return controller.GetListNationalities();
        }

        private void GetListsForFront()
        {
            ViewBag.Roles = GetListRoles();
            ViewBag.Skills = GetListSkills();
            ViewBag.Grades = GetListGrades();
            ViewBag.SkillCategories = GetListCategory();
            ViewBag.Candidacies = GetListCandidacy();
            ViewBag.Evaluations = GetListUserEval();
            ViewBag.CandidateStatus = GetListCandidateStatus();
            ViewBag.Locations = GetListLocation();
            ViewBag.Availability = GetListAvailability();
            ViewBag.Nationalities = GetListNationalities();
        }

        private Candidate CheckIfRemunerationsAreNull(Candidate candidate, int? gross, int? dailyGains, int? nET)
        {
            if (gross == null)
                candidate.GrossRemuneration = 0;
            else
                candidate.GrossRemuneration = int.Parse(gross.ToString());

            if (nET == null)
                candidate.NET = 0;
            else
                candidate.NET = int.Parse(nET.ToString());

            if (dailyGains == null)
                candidate.DailyGains = 0;
            else
                candidate.DailyGains = int.Parse(dailyGains.ToString());

            return candidate;
        }

        private CandidateFullInfo CheckIfRemunerationsAreZERO(CandidateFullInfo candidate, string? gross, string? nET, string? dailyGains)
        {
            if (gross == "0")
                candidate.GrossRemuneration = "N/D";
            else
                candidate.GrossRemuneration = gross.ToString();

            if (nET == "0")
                candidate.NET = "N/D";
            else
                candidate.NET = nET.ToString();

            if (dailyGains == "0")
                candidate.DailyGains = "N/D";
            else
                candidate.DailyGains = dailyGains.ToString();

            return candidate;
        }

        private string CheckIfInterviewIsNULL(string interview)
        {
            if(interview == null || interview == "")
                return "N/D";
            
            return interview;
        }

        private string CheckIfCandidateEvaluationIsND(string value)
        {
            if(value == "0")
                return "N/D";
            return value;
        }

        private int CheckIfCandidateEvaluationIsZERO(string value)
        {
            if (value == "N/D")
                return 0;
            return int.Parse(value);
        }

        private List<User> GetUsersInfo()
        {
            AdnDbController controller = new AdnDbController();
            return controller.GetUsersList();
        }

        private List<UserRoles> GetUserRolesList()
        {
            AdnDbController controller = new AdnDbController();
            return controller.GetUserRolesList();
        }

        private List<WorkPackage> GetListWP()
        {
            AdnDbController controller = new AdnDbController();
            return controller.GetListWorkPackage();
        }

        private void GetListCompanyRoles()
        {
            AdnDbController controller = new AdnDbController();
        }

        private void GetListDepartments()
        {
            AdnDbController controller = new AdnDbController();
        }

        public void LoggingAction(string epic, string typeofCRU, string before, string after, Exception ex)
        {
            AdnDbController controller = new AdnDbController();
            string Name = Convert.ToString(Session["User"]);
            int id = 0;
            if (Name == "")
            {
                Name = "Guest";
                id = -1;
            }
            else
            {
                Name = Convert.ToString(Session["User"]);
                id = int.Parse(Convert.ToString(Session["IdUser"]));
            }

            controller.ActionLOGGING(Name, id, epic, currentPage, typeofCRU, before, after, ex);
        }

        public string GetPathLogging()
        {
            return null;
        }

        public User GetLoggedInUser()
        {
            if (ViewBag.loggedInUser != null )
                return ViewBag.loggedInUser;
            else
            {
                User Defau = new User() { Name = "Guest", IdUser = -1 };
                return Defau;
            }
                
        }

        public string GetCurrentPage()
        {
            return currentPage;
        }

        private void GetUsersAndWP()
        {
            AdnDbController controller = new AdnDbController();
            UserList = controller.GetUsersList();
        }
    }
}