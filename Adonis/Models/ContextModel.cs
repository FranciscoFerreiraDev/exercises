using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;


namespace Adonis.Models
{
    public class ContextModel
    {
        //public SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["AdonDB"].ConnectionString);
        private SqlCommand command = new SqlCommand();
        private SqlDataReader reader;
        public SqlConnection connection;
        
        
        public ContextModel()
        {
            CreateConnection();
            //ChangeState(true);
            //CreateCommand();
        }

        //It creates the connection
        public void CreateConnection()
        {
            //if (this.GetConnectStringState() == false)
            //{
            //Este problema foi resolvido ao por a informaçao da ligação no projecto que inicia (Adon) 
            // e acedendo ao web config atraves do confiduration manager.
            //O web config acedido é sempre o do projeto iniciante.
            //A connection string é posta logo na variavel connection.
            connection = new SqlConnection(ConfigurationManager.ConnectionStrings["AdonDB"].ConnectionString);
            connection.Open();
            CreateCommand();

            //  }
        }

        //Return a bool dependig on the state of the connection string (false if null, true if !null)
        public bool GetConnectStringState()
        {
            if (connection.ConnectionString.Length == 0)
            {
                return false;
            }
            else
                return true;
        }

        //Changes the state of the connection
        public void ChangeState(Boolean state)
        {
            //this.CreateConnection();
            if (connection.ConnectionString == "")
            {
                CreateConnection();
            }
            try
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                else if (connection.State == ConnectionState.Open && state == false)
                {
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Logging("",(-1),"MODEL","NONE","NONE","","",new Exception("ERROR on connection state :-:"+ex.Message));
                throw ex;
            }
        }

        //Gets the state of connection (closed or open)
        public bool GetState()
        {
            if (connection.State == ConnectionState.Closed)
                return false;
            else
                return true;
        }

        //Creates the command and gives it its connection
        private void CreateCommand()
        {
            command.Connection = connection;
        }

        //Creates a Role
        public void CreateRole(Role target)
        {
            if (connection.State == ConnectionState.Closed)
                CreateConnection();



            using (connection)
            {
                command.CommandText = "INSERT INTO dbo.Roles (NameRole, Active) VALUES(@NameRole, @Active)";

                command.Parameters.Add("@NameRole", SqlDbType.NChar).Value = target.NameRole;
                command.Parameters.Add("@Active", SqlDbType.Int).Value = target.Active;

                reader = command.ExecuteReader();
                connection.Close();
            }
        }

        //Lists Roles And Skills
        public List<RoleAndSkill> QueryRolesAndSkills()
        {
            command.CommandText = "SELECT * FROM dbo.RoleAndSkill";
            var ListRole = new List<RoleAndSkill>();
            reader = command.ExecuteReader();

            using (connection)
            {
                while (reader.Read())
                {
                    RoleAndSkill RNS = new RoleAndSkill();
                    RNS.Id = int.Parse(reader["IdRoleAndSkill"].ToString());
                    RNS.IdRole = int.Parse(reader["IdRole"].ToString());
                    RNS.IdSkill = int.Parse(reader["IdSkill"].ToString());

                    ListRole.Add(RNS);
                }
            }
            connection.Close();

            return ListRole;
        }

        //Gets a certain RoleAndSkill
        public RoleAndSkill GetRoleAndSkill(int id)
        {
            if (connection.State == ConnectionState.Closed)
                CreateConnection();

            command.CommandText = "SELECT RoleAndSkill.IdRole, RoleAndSkill.IdSkill, RoleAndSkill.IdGradeLevel FROM dbo.RoleAndSkill WHERE RoleAndSkill.IdRoleAndSkill = @idRoleAndSkill";
            command.Parameters.Add("@idRoleAndSkill", SqlDbType.Int).Value = id;

            reader = command.ExecuteReader();
            reader.Read();
            using (connection)
            {
                RoleAndSkill result = new RoleAndSkill()
                {
                    IdRole = int.Parse(reader["IdRole"].ToString()),

                };
                connection.Close();
                return result;
            }
        }

        //Edits a certain RoleAndSkill
        public void UpdateRoleAndSkill(RoleAndSkill update)
        {
            if (connection.State == ConnectionState.Closed)
                CreateConnection();
            try
            {
                command.CommandText = "UPDATE dbo.RoleAndSkill SET IdRole = @idrole, IdSkill = @idskill, IdGradeLevel = @idgrade WHERE RoleAndSkill.IdRoleAndSkill = @idrns";
                command.Parameters.Add("@idrns", SqlDbType.Int).Value = update.Id;
                command.Parameters.Add("@idrole", SqlDbType.Int).Value = update.IdRole;
                command.Parameters.Add("@idskill", SqlDbType.Int).Value = update.IdSkill;
                command.Parameters.Add("@idgrade", SqlDbType.Int).Value = update.IdGrade;

                using (connection)
                {
                    //connection.Open();
                    reader = command.ExecuteReader();
                    reader.Read();
                }
                connection.Close();
                SqlConnection.ClearAllPools();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void UpdateCandidateRoleAndSkill(CandidateAndRoleAndSkill update)
        {
            if (connection.State == ConnectionState.Closed)
                CreateConnection();
            try
            {
                command.CommandText = "UPDATE dbo.CandidateRAndS SET RoleStart = @RoleStart, RoleFinish = @RoleFinish, MainExperience = @MainExperience, Description = @Description WHERE IdCandidateRaS = @IdCandidateRaS";
                command.Parameters.Add("@IdCandidateRaS", SqlDbType.Int).Value = update.IdCRNS;
                command.Parameters.Add("@RoleStart", SqlDbType.VarChar).Value = update.DateStart;
                command.Parameters.Add("@RoleFinish", SqlDbType.VarChar).Value = update.DateFinish;
                command.Parameters.Add("@MainExperience", SqlDbType.Int).Value = update.MainExperience;
                command.Parameters.Add("@Description", SqlDbType.VarChar).Value = update.Description;

                using (connection)
                {
                    reader = command.ExecuteReader();
                    reader.Read();
                }
                CloseDBConnections();
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        public void UpdateDescription(CandidateNotes update)
        {
            if (connection.State == ConnectionState.Closed)
                CreateConnection();
            try
            {
                command.Parameters.Clear();

                command.CommandText = "UPDATE dbo.CandidateRAndS SET Description = @Description WHERE IdCandidate = @IdCandidate";
                command.Parameters.Add("@Description", SqlDbType.NVarChar).Value = update.Note;
                command.Parameters.Add("@IdCandidate", SqlDbType.Int).Value = update.IdCandidate;

                using (connection)
                {
                    reader = command.ExecuteReader();
                    reader.Read();
                }
                CloseDBConnections();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void RevokeMainExp(int IdCandidate)
        {
            try
            {
                using (connection)
                {
                    command.CommandText = "UPDATE [CandidateRAndS] SET [MainExperience] = 0 WHERE [IdCandidate] =  @IdCandidate";
                    command.Parameters.Add("@IdCandidate", SqlDbType.Int).Value = IdCandidate;

                    reader = command.ExecuteReader();
                    reader.Read();
                }

                connection.Close();
                SqlConnection.ClearAllPools();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<CandidateAndRoleAndSkill> GetCandidateAndRoleAndSkillsList(int IdCandidate)
        {
            List<CandidateAndRoleAndSkill> result = new List<CandidateAndRoleAndSkill>();
            try
            {
                using (connection)
                {
                    command.CommandText = "SELECT * FROM CandidateRAndS WHERE IdCandidate = @IdCandidate";
                    command.Parameters.Add("@IdCandidate", SqlDbType.Int).Value =IdCandidate;

                    reader = command.ExecuteReader();

                    while(reader.Read())
                    {
                        int MainExp = int.Parse(reader["MainExperience"].ToString());
                        if (MainExp == 1)
                        {
                            CandidateAndRoleAndSkill old = new CandidateAndRoleAndSkill()
                            {
                                IdCRNS = int.Parse(reader["IdCandidateRaS"].ToString()),
                                MainExperience = 0,
                            };
                        }
                    }
                }
                CloseDBConnections();
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        //Get last Role and Skill Id
        public int SelectLastRnSId()
        {
            if (connection.State == ConnectionState.Closed)
                CreateConnection();

            int IdRnD = 0;
            List<SkillCategory> categories = new List<SkillCategory>();

            command.CommandText = "SELECT TOP 1 IdRoleAndSkill FROM dbo.RoleAndSkill ORDER BY IdRoleAndSkill DESC";

            reader = command.ExecuteReader();
            reader.Read();
            using (connection)
            {
                IdRnD = int.Parse(reader["IdRoleAndSkill"].ToString());
            }
            connection.Close();
            return IdRnD;
        }

        //Creates a Category
        internal void CreateCategory(string newCategory)
        {
            if (connection.State == ConnectionState.Closed)
                CreateConnection();

            command.CommandText = "INSERT INTO dbo.SkillCategory (NameCategory) VALUES(@NameCategory)";

            command.Parameters.Add("@NameCategory", SqlDbType.NChar).Value = newCategory;

            using (connection)
            {
                reader = command.ExecuteReader();
            }
            connection.Close();
        }

        //Lists Categories
        internal List<SkillCategory> QueryCategories()
        {
            if (connection.State == ConnectionState.Closed)
                CreateConnection();

            List<SkillCategory> categories = new List<SkillCategory>();

            command.CommandText = "SELECT * FROM dbo.SkillCategory;";

            reader = command.ExecuteReader();
            using (connection)
            {

                while (reader.Read())
                {
                    SkillCategory category = new SkillCategory()
                    {
                        IdCategory = int.Parse(reader["IdCategory"].ToString()),
                        NameCategory = reader["NameCategory"].ToString(),
                        Activated = int.Parse(reader["Activated"].ToString()),
                    };
                    categories.Add(category);
                }
            }
            connection.Dispose();
            SqlConnection.ClearAllPools();
            return categories;
        }

        //Edits a Category
        public void UpdateCategory(SkillCategory category)
        {
            if (connection.State == ConnectionState.Closed)
                CreateConnection();

            command.CommandText = "UPDATE dbo.SkillCategory SET NameCategory = @Name, Activated = @Active WHERE IdCategory = @IdCategory";

            command.Parameters.Add("@Name", SqlDbType.NChar).Value = category.NameCategory.ToString();
            command.Parameters.Add("@Active", SqlDbType.Int).Value = category.Activated;
            command.Parameters.Add("@IdCategory", SqlDbType.Int).Value = category.IdCategory;

            //command.Parameters.Add("@Name", SqlDbType.NChar).Value = role.ToString();

            using (connection)
            {
                reader = command.ExecuteReader();
            }
            connection.Close();
        }

        //Lists the candidates, their roles and skills
        public List<CandidateFullInfo> TestQueryCandidate()
        {
            try
            {
                command.CommandText = "SELECT Candidate.IdClassification, CandidateStatus.StatusDescription, CandidateStatus.Color, CandidateRAndS.MainExperience, CandidateRAndS.IdRolesAndSkills, RoleAndSkill.IdRole, RoleAndSkill.IdSkill, Candidate.Name, " +
                "Candidate.IdCandidate ,Roles.NameRole ,Skills.NameSkill ,CandidateRAndS.IdCandidateRaS, CandidateRAndS.RoleStart ,CandidateRAndS.RoleFinish, Candidate.NET ,Candidate.IdLocation, " +
                "Availability.AvailabilityDescription, Availability.IdAvailability, Candidate.DailyGains ,Candidate.RemunerationNotes, Candidate.Interview ,SkillCategory.NameCategory , CandidateStatus.Color, Candidate.CandidateCode, " +
                "Location.LocationDescription, Location.IdLocation, SkillGrade.Grade ,CandidateRAndS.Description ,Candidate.GrossRemuneration, Candidate.Availability ,Candidate.IdCandidacy ,Candidate.Activated, Candidacy.Candidacy, " +
                "Nationality.NationalityDescription, Candidate.IdNationality, CandidateStatus.IdStatus, Candidate.BirthDate/*, CandidateDescription.Description AS CandidateDescription*/, Candidate.ContactNumber, Candidate.Email" +
                " FROM dbo.Candidate " +
                "INNER JOIN dbo.CandidateRAndS ON CandidateRAndS.IdCandidate = Candidate.IdCandidate INNER JOIN dbo.CandidateCassification ON Candidate.IdClassification = CandidateCassification.IdClassification " +
                "INNER JOIN dbo.CandidateStatus ON CandidateStatus.IdStatus = Candidate.IdStatus INNER JOIN dbo.RoleAndSkill ON CandidateRAndS.IdRolesAndSkills = RoleAndSkill.IdRoleAndSkill " +
                "INNER JOIN dbo.Roles ON RoleAndSkill.IdRole = Roles.IdRole INNER JOIN dbo.SkillGrade ON RoleAndSkill.IdGradeLevel = SkillGrade.IdGradeLevel " +
                "INNER JOIN dbo.Skills ON RoleAndSkill.IdSkill = Skills.IdSkill INNER JOIN dbo.SkillCategory ON Skills.IdCategory = SkillCategory.IdCategory " +
                "INNER JOIN dbo.Availability ON Candidate.Availability = Availability.IdAvailability INNER JOIN dbo.Candidacy ON Candidacy.IdCandidacy = Candidate.IdCandidacy " +
                "INNER JOIN dbo.Location ON Candidate.IdLocation = Location.IdLocation INNER JOIN dbo.Nationality ON Nationality.IdNationality = Candidate.IdNationality " +
                "Order by Candidate.Name, CandidateRAndS.MainExperience desc, CandidateRAndS.RoleStart desc";

                var ListCandidateFull = new List<CandidateFullInfo>();
                reader = command.ExecuteReader();

                using (connection)
                {
                    while (reader.Read())
                    {
                        CandidateFullInfo candidate = new CandidateFullInfo();
                        candidate.IdRolesAndSkills = int.Parse(reader["IdRolesAndSkills"].ToString());
                        candidate.IdCRnS = int.Parse(reader["IdCandidateRaS"].ToString());
                        candidate.IdCandidate = int.Parse(reader["IdCandidate"].ToString());
                        candidate.Name = reader["Name"].ToString();
                        candidate.IdRole = int.Parse(reader["IdRole"].ToString());
                        candidate.Role = reader["NameRole"].ToString();
                        candidate.IdSkill = int.Parse(reader["IdSkill"].ToString());
                        candidate.Skill = reader["NameSKill"].ToString();
                        candidate.SkillType = reader["NameCategory"].ToString();
                        candidate.DateStart = reader["RoleStart"].ToString();
                        candidate.DateFinish = reader["RoleFinish"].ToString();
                        candidate.Description = reader["Description"].ToString();
                        candidate.ExpDescription = reader["Description"].ToString();
                        candidate.Status = reader["StatusDescription"].ToString();
                        candidate.Grade = reader["Grade"].ToString();
                        candidate.GrossRemuneration = reader["GrossRemuneration"].ToString();
                        candidate.Availability = reader["AvailabilityDescription"].ToString();
                        candidate.IdAvailability = int.Parse(reader["IdAvailability"].ToString());
                        candidate.Classification = int.Parse(reader["IdClassification"].ToString());
                        candidate.Activated = int.Parse(reader["Activated"].ToString());
                        candidate.NET = reader["NET"].ToString();
                        candidate.CurrentPlace = reader["LocationDescription"].ToString();
                        candidate.IdLocation = int.Parse(reader["IdLocation"].ToString());
                        candidate.DailyGains = reader["DailyGains"].ToString();
                        candidate.Candidacy = reader["Candidacy"].ToString();
                        candidate.RemunerationNotes = reader["RemunerationNotes"].ToString();
                        candidate.Status = reader["StatusDescription"].ToString();
                        candidate.IdStatus = int.Parse(reader["IdStatus"].ToString());
                        candidate.Interview = reader["Interview"].ToString();
                        candidate.IdCandidacy = int.Parse(reader["IdCandidacy"].ToString());
                        candidate.MainExperience = int.Parse(reader["MainExperience"].ToString());
                        candidate.StatusColor = reader["Color"].ToString();
                        candidate.CandidateCode = reader["CandidateCode"].ToString();
                        candidate.IdNationality = int.Parse(reader["IdNationality"].ToString());
                        candidate.NationalityDescription = reader["NationalityDescription"].ToString();
                        candidate.BirthDate = reader["BirthDate"].ToString();
                        //candidate.CandidateDescription = reader["CandidateDescription"].ToString();
                        candidate.ContactNumber = int.Parse(reader["ContactNumber"].ToString());
                        candidate.Email = reader["Email"].ToString();

                        ListCandidateFull.Add(candidate);
                    }
                }
                connection.Close();

                return ListCandidateFull;
            }
            catch (Exception e)
            {
                throw e;
            }
            
        }

        //Gets a certain Candidate
        public Candidate GetCandidate(int id)
        {
            try
            {


                if (connection.State == ConnectionState.Closed)
                    CreateConnection();

                command.CommandText = "SELECT Candidate.*, Location.LocationDescription, Nationality.NationalityDescription, Availability.AvailabilityDescription, CandidateStatus.StatusDescription, Candidacy.Candidacy, CandidateRAndS.Description" +
                    " From Candidate Inner Join"+" Nationality On Candidate.IdNationality = Nationality.IdNationality Inner Join"+
    " Location On Candidate.IdLocation = Location.IdLocation Inner Join"+
    " CandidateStatus On Candidate.IdStatus = CandidateStatus.IdStatus Inner Join"+
    " Availability On Candidate.Availability = Availability.IdAvailability Inner Join"+
    " Candidacy On Candidate.IdCandidacy = Candidacy.IdCandidacy Inner Join"+
    " CandidateRAndS On CandidateRAndS.IdCandidate = Candidate.IdCandidate WHERE Candidate.IdCandidate = @id";

                command.Parameters.Add("@id", SqlDbType.NChar).Value = id;

                Candidate result;

                using (connection)
                {
                    reader = command.ExecuteReader();
                    reader.Read();

                    result = new Candidate()
                    {
                        IdCandidate = int.Parse(reader["IdCandidate"].ToString()),
                        Name = reader["Name"].ToString(),
                        CandidateCode = reader["CandidateCode"].ToString(),
                        Activated = int.Parse(reader["Activated"].ToString()),
                        GrossRemuneration = int.Parse(reader["GrossRemuneration"].ToString()),
                        Availability = int.Parse(reader["Availability"].ToString()),
                        CurrentPlace = int.Parse(reader["IdLocation"].ToString()),
                        IdClassification = int.Parse(reader["IdClassification"].ToString()),

                        NationalityDescription = reader["NationalityDescription"].ToString(),
                        CurrentPlaceDescription = reader["LocationDescription"].ToString(),
                        AvailabilityDescription = reader["AvailabilityDescription"].ToString(),
                        CandidacyDescription = reader["Candidacy"].ToString(),
                        CandidateStatusDescription = reader["StatusDescription"].ToString(),

                        BirthDate = reader["BirthDate"].ToString(),
                        NET = int.Parse(reader["NET"].ToString()),
                        DailyGains = int.Parse(reader["DailyGains"].ToString()),
                        RemunerationNotes = reader["RemunerationNotes"].ToString(),
                        Description = reader["RemunerationNotes"].ToString(),
                    };
                    connection.Close();
                }
                SqlConnection.ClearPool(connection);
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        //Updates a Candidate
        public void UpdateCandidate(Candidate update)
        {
            if (connection.State == ConnectionState.Closed)
                CreateConnection();
            try
            {
                using (connection)
                {
                    command.CommandText = "UPDATE dbo.Candidate SET Name = @name, Activated = @act, GrossRemuneration = @gr, Availability = @ava, NET = @NET, DailyGains = @DailyGains, RemunerationNotes = @RemunerationNotes, " +
                "IdLocation = @IdLocation, IdStatus = @IdStatus, Interview = @Interview, IdCandidacy = @candidacy, IdClassification = @IdClassification, CandidateCode = @CandidateCode, IdNationality = @IdNationality, " +
                "BirthDate = @BirthDate, ContactNumber = @ContactNumber, Email = @Email WHERE Candidate.IdCandidate = @idcandidate; ";

                    command.Parameters.Add("@idcandidate", SqlDbType.Int).Value = update.IdCandidate;
                    command.Parameters.Add("@name", SqlDbType.VarChar).Value = update.Name;
                    command.Parameters.Add("@act", SqlDbType.VarChar).Value = update.Activated;
                    command.Parameters.Add("@gr", SqlDbType.VarChar).Value = update.GrossRemuneration;
                    command.Parameters.Add("@ava", SqlDbType.VarChar).Value = update.IdAvailability;
                    command.Parameters.Add("@NET", SqlDbType.Int).Value = update.NET;
                    command.Parameters.Add("@DailyGains", SqlDbType.Int).Value = update.DailyGains;
                    command.Parameters.Add("@RemunerationNotes", SqlDbType.NVarChar).Value = update.RemunerationNotes;
                    command.Parameters.Add("@IdLocation", SqlDbType.Int).Value = update.CurrentPlace;
                    command.Parameters.Add("@IdStatus", SqlDbType.Int).Value = update.IdStatus;
                    command.Parameters.Add("@Interview", SqlDbType.VarChar).Value = update.Interview;
                    command.Parameters.Add("@candidacy", SqlDbType.Int).Value = update.Candidacy;
                    command.Parameters.Add("@IdClassification", SqlDbType.Int).Value = update.IdClassification;
                    command.Parameters.Add("@IdNationality", SqlDbType.Int).Value = update.IdNationality;
                    command.Parameters.Add("@CandidateCode", SqlDbType.VarChar).Value = update.CandidateCode;
                    command.Parameters.Add("@BirthDate", SqlDbType.VarChar).Value = update.BirthDate;
                    command.Parameters.Add("@ContactNumber", SqlDbType.Int).Value = update.ContactNumber;
                    command.Parameters.Add("@Email", SqlDbType.VarChar).Value = update.Email;

                    reader = command.ExecuteReader();
                    reader.Read();
                    connection.Close();
                    this.CloseDBConnections();
                }
            }
            catch (Exception e)
            {
                throw e;
            }            
        }

        //Selects the last candidate added to the db
        public int SelectLastCandidate()
        {
            if (connection.State == ConnectionState.Closed)
                CreateConnection();

            int Id = 0;

            command.CommandText = "SELECT TOP 1 IdCandidate FROM dbo.Candidate ORDER BY IdCandidate DESC";

            reader = command.ExecuteReader();
            reader.Read();
            using (connection)
            {
                var frase = reader["IdCandidate"].ToString();
                Id = int.Parse(frase);
            }
            connection.Close();
            return Id;
        }

        //Lists the Roles
        public List<Role> QueryRoles()
        {            
            var ListRole = new List<Role>();

            using (connection)
            {
                command.CommandText = "SELECT Roles.NameRole, Roles.IdRole, Roles.Active FROM dbo.Roles";
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Role Role = new Role();
                    Role.IdRole = int.Parse(reader["IdRole"].ToString());
                    Role.NameRole = reader["NameRole"].ToString();
                    Role.Active = int.Parse(reader["Active"].ToString());

                    ListRole.Add(Role);
                }
            }
            connection.Close();
            connection.Dispose();
            SqlConnection.ClearAllPools();
            return ListRole;
        }

        //Lists the Skills
        public List<Skills> QuerySkills()
        {
            var ListSkills = new List<Skills>();

            command.CommandText = "SELECT * FROM dbo.Skills";
            using (connection)
            {
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Skills Skill = new Skills()
                    {
                        IdSkill = int.Parse(reader["IdSkill"].ToString()),
                        Name = reader["NameSkill"].ToString(),
                        IdCategory = int.Parse(reader["IdCategory"].ToString()),
                        Active = int.Parse(reader["Active"].ToString()),
                    };

                    ListSkills.Add(Skill);
                }
            }
            connection.Dispose();
            return ListSkills;
        }

        //Creates a new Skill
        public void CreateSkill(Skills target)
        {
            if (connection.State == ConnectionState.Closed)
                CreateConnection();
            try
            {
                command.CommandText = "INSERT INTO dbo.Skills (NameSkill, IdCategory, Active) VALUES(@NameSkill, @IdCategory, @Active)";

                command.Parameters.Add("@NameSkill", SqlDbType.NChar).Value = target.Name;
                command.Parameters.Add("@IdCategory", SqlDbType.Int).Value = target.IdCategory;
                command.Parameters.Add("@Active", SqlDbType.Int).Value = 1;

                using (connection)
                {
                    reader = command.ExecuteReader();
                }
                connection.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Updates a Skill
        public void UpdateSkill(Skills skill)
        {
            if (connection.State == ConnectionState.Closed)
                CreateConnection();

            command.CommandText = "UPDATE dbo.Skills SET NameSkill = @Name, IdCategory = @IdCategory, Active = @Active WHERE IdSkill = @IdSkill";

            command.Parameters.Add("@Name", SqlDbType.NChar).Value = skill.Name.ToString();
            command.Parameters.Add("@IdCategory", SqlDbType.NChar).Value = int.Parse(skill.IdCategory.ToString());
            command.Parameters.Add("@Active", SqlDbType.Int).Value = skill.Active;
            command.Parameters.Add("@IdSkill", SqlDbType.Int).Value = int.Parse(skill.IdSkill.ToString());

            using (connection)
            {
                reader = command.ExecuteReader();
                connection.Close();
            }
        }

        //Creates a Candidate
        public void CreateCandidate(Candidate target)
        {
            command.CommandText = "INSERT INTO dbo.Candidate (Name, IdNationality, Activated, GrossRemuneration, Availability, NET, IdLocation, DailyGains, RemunerationNotes, IdCandidacy, IdStatus, Interview, IdClassification, " +
                "CandidateCode, BirthDate, ContactNumber, Email) " + "VALUES(@Name, @IdNationality, @Activated, @Gross, @Avaiability, @NET, @IdLocation, @DailyGains, @RemunerationNotes, @IdCandidacy, @IdStatus, @Interview, @IdClassification, @CodeCandidate," +
                " @BirthDate, @ContactNumber, @Email)";

            try
            {
                using (connection)
                {
                    command.Parameters.Add("@Name", SqlDbType.NVarChar).Value = target.Name;
                    command.Parameters.Add("@IdNationality", SqlDbType.Int).Value = target.IdNationality;
                    command.Parameters.Add("@Activated", SqlDbType.Int).Value = target.Activated;
                    command.Parameters.Add("@Gross", SqlDbType.Int).Value = target.GrossRemuneration;
                    command.Parameters.Add("@Avaiability", SqlDbType.Int).Value = target.Availability;
                    command.Parameters.Add("@BirthDate", SqlDbType.VarChar).Value = target.BirthDate;
                    command.Parameters.Add("@NET", SqlDbType.Int).Value = target.NET;
                    command.Parameters.Add("@IdLocation", SqlDbType.VarChar).Value = target.CurrentPlace;
                    command.Parameters.Add("@DailyGains", SqlDbType.Int).Value = target.DailyGains;
                    command.Parameters.Add("@RemunerationNotes", SqlDbType.VarChar).Value = target.RemunerationNotes.ToString();
                    command.Parameters.Add("@IdCandidacy", SqlDbType.Int).Value = int.Parse(target.Candidacy);
                    command.Parameters.Add("@CvRegistered", SqlDbType.Int).Value = target.status;
                    command.Parameters.Add("@Interview", SqlDbType.VarChar).Value = target.Interview;
                    command.Parameters.Add("@IdClassification", SqlDbType.Int).Value = target.IdClassification;
                    command.Parameters.Add("@IdStatus", SqlDbType.Int).Value = target.IdStatus;
                    command.Parameters.Add("@CodeCandidate", SqlDbType.VarChar).Value = target.CandidateCode;
                    command.Parameters.Add("@ContactNumber", SqlDbType.Int).Value = target.ContactNumber;
                    command.Parameters.Add("@Email", SqlDbType.VarChar).Value = target.Email;

                    reader = command.ExecuteReader();
                }
                connection.Close();
                SqlConnection.ClearAllPools();
            }
            catch (Exception ex)
            {
                connection.Close();
                throw ex;
            }
        }

        //Creates a CandidateDescription
        public void CreateCandidateNote(CandidateNotes target)
        {
            if (connection.State == ConnectionState.Closed)
                CreateConnection();

            try
            {
                command.CommandText = "INSERT INTO dbo.CandidateNotes (IdCandidate, Note, TimeStamp, Active, IdUser) VALUES(@IdCandidate, @Note, @TimeStamp, @Active, @IdUser)";

                command.Parameters.Add("@IdCandidate", SqlDbType.Int).Value = target.IdCandidate;
                command.Parameters.Add("@IdUser", SqlDbType.Int).Value = target.IdUser;
                command.Parameters.Add("@Active", SqlDbType.Int).Value = target.Active;
                command.Parameters.Add("@Note", SqlDbType.NVarChar).Value = target.Note;
                command.Parameters.Add("@TimeStamp", SqlDbType.NVarChar).Value = target.TimeStamp;

                using (connection)
                {
                    reader = command.ExecuteReader();
                }
                connection.Close();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        //Edits a Candidate Note
        public void EditCandidateNote(CandidateNotes target)
        {
            if (connection.State == ConnectionState.Closed)
                CreateConnection();

            try
            {
                command.CommandText = "UPDATE dbo.CandidateNotes SET IdUser = @IdUser, Note = @Note, TimeStamp = @TimeStamp WHERE IdCandidateDescription = @IdCandidateDescription";

                command.Parameters.Add("@IdCandidateDescription", SqlDbType.Int).Value = target.IdCandidateDescription;
                command.Parameters.Add("@IdUser", SqlDbType.Int).Value = target.IdUser;
                command.Parameters.Add("@Note", SqlDbType.NVarChar).Value = target.Note;
                command.Parameters.Add("@TimeStamp", SqlDbType.NVarChar).Value = target.TimeStamp;

                using (connection)
                {
                    reader = command.ExecuteReader();
                }
                connection.Close();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        //Changes the state of a CandidateNote from 1 to 0
        public void DeleteCandidateNote(int target)
        {
            if (connection.State == ConnectionState.Closed)
                CreateConnection();

            try
            {
                command.CommandText = "UPDATE dbo.CandidateNotes SET Active = @Active WHERE IdCandidateDescription = @IdCandidateDescription";

                command.Parameters.Add("@IdCandidateDescription", SqlDbType.Int).Value = target;
                command.Parameters.Add("@Active", SqlDbType.Int).Value = 0;

                using (connection)
                {
                    reader = command.ExecuteReader();
                }
                connection.Close();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        //Lists the Candidates
        public List<Candidate> QueryCandidate()
        {
            var ListCandidates = new List<Candidate>();

            command.CommandText = "SELECT * FROM dbo.Candidate";
            this.ChangeState(true);
            reader = command.ExecuteReader();

            using (connection)
            {
                while (reader.Read())
                {
                    Candidate Candidate = new Candidate();
                    Candidate.Name = reader["Name"].ToString();
                    Candidate.IdCandidate = int.Parse(reader["IdCandidate"].ToString());

                    ListCandidates.Add(Candidate);
                }
            }
            connection.Close();
            return ListCandidates;
        }

        //Creates RoleAndSKills that exist within targets
        public void CreateRollAndSkill(RoleAndSkill target)
        {
            if (connection.State == ConnectionState.Closed)
                CreateConnection();

            command.CommandText = "INSERT INTO dbo.RoleAndSkill (IdRole, IdSkill, IdGradeLevel) VALUES(@IdRole, @IdSkill, @IdGradeLevel)";

            command.Parameters.Add("IdRole", SqlDbType.Int).Value = target.IdRole;
            command.Parameters.Add("IdSkill", SqlDbType.Int).Value = target.IdSkill;
            command.Parameters.Add("IdGradeLevel", SqlDbType.Int).Value = int.Parse(target.IdGrade.ToString());

            using (connection)
            {
                reader = command.ExecuteReader();
            }
            command.Parameters.Clear();
            connection.Close();
        }

        //Query the CandidateDescriptions
        public List<CandidateNotes> GetListCandidateNotes()
        {
            try
            {
                if (connection.State == ConnectionState.Closed)
                    CreateConnection();

                List<CandidateNotes> ListDescriptions = new List<CandidateNotes>();

                command.CommandText = "Select CandidateNotes.*, [User].[Name], Candidate.Name as CandidateName From CandidateNotes Inner Join[User] On CandidateNotes.IdUser = [User].IdUser " +
                    "inner join Candidate On CandidateNotes.IdCandidate = Candidate.IdCandidate Order By IdCandidate DESC, TimeStamp DESC";

                using (connection)
                {
                    using (connection)      
                    {
                        reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            CandidateNotes Description = new CandidateNotes()
                            {
                                IdCandidateDescription = int.Parse(reader["IdCandidateDescription"].ToString()),
                                IdCandidate = int.Parse(reader["IdCandidate"].ToString()),
                                CandidateName = reader["CandidateName"].ToString(),
                                Note = reader["Note"].ToString(),
                                TimeStamp = reader["TimeStamp"].ToString(),
                                Active = int.Parse(reader["Active"].ToString()),
                                IdUser = int.Parse(reader["IdUser"].ToString()),
                                User = reader["Name"].ToString(),
                            };
                            ListDescriptions.Add(Description);
                        }
                    }
                }
                command.Parameters.Clear();
                connection.Close();

                return ListDescriptions;
            }
            catch (Exception e)
            {
                throw e;
            }
            
        }

        //Creates CandidateRoleAndSKills
        public void CreateCandidateRollAndSkill(CandidateAndRoleAndSkill target)
        {
            if (connection.State == ConnectionState.Closed)
                CreateConnection();

            command.CommandText = "INSERT INTO dbo.CandidateRAndS (IdCandidate, IdRolesAndSkills, RoleStart, RoleFinish, MainExperience) VALUES(@IdCandidate, @IdRolesAndSkills, @RoleStart, @RoleFinish, @MainExperience)";

            command.Parameters.Add("@IdCandidate", SqlDbType.Int).Value = target.IdCandidate;
            command.Parameters.Add("@IdRolesAndSkills", SqlDbType.Int).Value = target.IdRolesAndSkills;
            command.Parameters.Add("@RoleStart", SqlDbType.NChar).Value = target.DateStart;
            command.Parameters.Add("@RoleFinish", SqlDbType.NChar).Value = target.DateFinish;
            command.Parameters.Add("@MainExperience", SqlDbType.Int).Value = target.MainExperience;

            using (connection)
            {
                reader = command.ExecuteReader();
            }
            connection.Close();
        }

        //Gets a CandidateAndRoleAndSkill
        public CandidateAndRoleAndSkill GetCandidateRoleAndSkill(int target)
        {
            if (connection.State == ConnectionState.Closed)
                CreateConnection();

            command.CommandText = "SELECT * FROM dbo.CandidateRAndS WHERE CandidateRAndS.IdCandidateRaS = @idCRNS";
            command.Parameters.Add("@idCRNS", SqlDbType.Int).Value = target;

            //this.Open();
            //connection.Open();
            //this.CreateConnection();
            reader = command.ExecuteReader();
            reader.Read();
            using (connection)
            {
                CandidateAndRoleAndSkill result = new CandidateAndRoleAndSkill()
                {
                    IdCRNS = int.Parse(reader["IdCandidateRaS"].ToString()),
                    Description = reader["Description"].ToString(),
                    DateStart = reader["RoleStart"].ToString(),
                    DateFinish = reader["RoleFinish"].ToString(),
                    IdCandidate = int.Parse(reader["IdCandidate"].ToString()),
                    IdRolesAndSkills = int.Parse(reader["IdRolesAndSkills"].ToString()),
                };
                connection.Close();
                return result;
            }
        }

        //Updates a CandidateAndRoleAndSkill
        public void UpdateCRnS(CandidateAndRoleAndSkill edit)
        {
            if (connection.State == ConnectionState.Closed)
                CreateConnection();

            using (connection)
            {
                command.CommandText = "UPDATE dbo.CandidateRAndS SET RoleStart = @start, RoleFinish = @finish, Description = @desc, MainExperience = @MainExperience WHERE IdCandidateRaS = @IdRnS";

                command.Parameters.Add("@IdRnS", SqlDbType.Int).Value = int.Parse(edit.IdCRNS.ToString());
                command.Parameters.Add("@MainExperience", SqlDbType.Int).Value = edit.MainExperience;
                command.Parameters.Add("@start", SqlDbType.VarChar).Value = edit.DateStart.ToString();
                command.Parameters.Add("@finish", SqlDbType.VarChar).Value = edit.DateFinish.ToString();
                command.Parameters.Add("@desc", SqlDbType.VarChar).Value = edit.Description.ToString();                         

                //connection.Open();
                reader = command.ExecuteReader();
                reader.Read();
            }
            connection.Close();
            SqlConnection.ClearAllPools();
        }

        //Lists Grades
        public List<SkillGrade> QueryGrades()
        {
            var ListGrades = new List<SkillGrade>();

            using (connection)
            {
                command.CommandText = "SELECT SkillGrade.IdGradeLevel, SkillGrade.Grade FROM dbo.SkillGrade";
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    SkillGrade Grade = new SkillGrade();
                    Grade.IdGrade = int.Parse(reader["IdGradeLevel"].ToString());
                    Grade.Grade = reader["Grade"].ToString();

                    ListGrades.Add(Grade);
                }
            }
            connection.Close();
            SqlConnection.ClearAllPools();
            return ListGrades;
        }

        //Creates a new Grade
        public void CreateGrade(string target)
        {
            if (connection.State == ConnectionState.Closed)
                CreateConnection();

            command.CommandText = "INSERT INTO dbo.SkillGrade (Grade) VALUES(@Name)";

            command.Parameters.Add("@Name", SqlDbType.NChar).Value = target.ToString();

            using (connection)
            {
                reader = command.ExecuteReader();
            }
            connection.Close();
        }

        //Lists UserEvaluation
        public List<CandidateEvaluation> QueryCandidateEvaluations()
        {
            var ListEval = new List<CandidateEvaluation>();

            using (connection)
            {
                command.CommandText = "SELECT * FROM dbo.CandidateCassification";
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    CandidateEvaluation item = new CandidateEvaluation();
                    item.IdEval = int.Parse(reader["IdClassification"].ToString());
                    item.Eval = reader["Name"].ToString();

                    ListEval.Add(item);
                }

                connection.Close();
            }
            SqlConnection.ClearAllPools();
            return ListEval;
        }

        //Edits a Role
        public void UpdateRole(Role role)
        {
            if (connection.State == ConnectionState.Closed)
                CreateConnection();

            //command.Parameters.Add("@Name", SqlDbType.NChar).Value = role.ToString();

            using (connection)
            {
                command.CommandText = "UPDATE dbo.Roles SET NameRole = @Name, Active = @Active WHERE IdRole = @IdRole";

                command.Parameters.Add("@Name", SqlDbType.NChar).Value = role.NameRole.ToString();
                command.Parameters.Add("@Active", SqlDbType.Int).Value = role.Active;
                reader = command.ExecuteReader();
                connection.Close();
            }
        }

        //Gets a certain Role
        public Role GetRole(int idRole)
        {
            if (connection.State == ConnectionState.Closed)
                CreateConnection();

            //command.Parameters.Add("@Name", SqlDbType.NChar).Value = role.ToString();
            Role target = new Role(); ;
            using (connection)
            {
                command.CommandText = "SELECT * FROM dbo.Roles WHERE IdRole = @IdRole";

                command.Parameters.Add("@IdRole", SqlDbType.Int).Value = idRole;
                reader = command.ExecuteReader();
                
                while (reader.Read())
                {
                    target = new Role()
                    {
                        NameRole = reader["NameRole"].ToString(),
                        Active = int.Parse(reader["Active"].ToString()),
                    };
                }
                connection.Close();
            }
            return target;
        }

        //It Gets a list of Candidacies
        public List<Candidacy> QueryCandidacy()
        {
            List<Candidacy> ListCandidacy = new List<Candidacy>();

            using (connection)
            {
                command.CommandText = "SELECT * FROM dbo.Candidacy";
                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Candidacy Candidacy = new Candidacy()
                    {
                        IdCandidacy = int.Parse(reader["IdCandidacy"].ToString()),
                        Description = reader["Candidacy"].ToString(),
                        Activated = int.Parse(reader["Activated"].ToString()),
                    };
                    ListCandidacy.Add(Candidacy);
                }
                connection.Close();
                SqlConnection.ClearAllPools();
                return ListCandidacy;
            }
        }

        //It Creates a Candidacy
        public void CreateCandidacy(Candidacy newCandidacy)
        {
            if (connection.State == ConnectionState.Closed)
                CreateConnection();

            command.CommandText = "INSERT INTO dbo.Candidacy (Candidacy, Activated) VALUES(@Name, @Activated)";
            command.Parameters.Add("@Name", SqlDbType.VarChar).Value = newCandidacy.Description;
            command.Parameters.Add("@Activated", SqlDbType.Int).Value = newCandidacy.Activated;

            using (connection)
            {
                reader = command.ExecuteReader();
            }
            connection.Close();
        }

        //It Updates a Candidacy
        public void UpdateCandidacy(Candidacy update)
        {
            if (connection.State == ConnectionState.Closed)
                CreateConnection();

            command.CommandText = "UPDATE dbo.Candidacy SET candidacy = @Name, Activated = @Active WHERE IdCandidacy = @IdCandidacy";

            command.Parameters.Add("@Name", SqlDbType.NChar).Value = update.Description.ToString();
            command.Parameters.Add("@Active", SqlDbType.Int).Value = update.Activated;
            command.Parameters.Add("@IdCandidacy", SqlDbType.Int).Value = update.IdCandidacy;


            using (connection)
            {
                reader = command.ExecuteReader();
            }
            connection.Close();
        }

        //It gets a List of CandidateStatus
        public List<CandidateStatus> QueryCandidateStatus()
        {
            List<CandidateStatus> ListCandidateStatus = new List<CandidateStatus>();

            //this.CreateConnection();
            using (connection)
            {
                command.CommandText = "SELECT * FROM dbo.CandidateStatus";
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    CandidateStatus item = new CandidateStatus()
                    {
                        IdStatus = int.Parse(reader["IdStatus"].ToString()),
                        Description = reader["StatusDescription"].ToString(),
                        Activated = int.Parse(reader["Activated"].ToString()),
                        Color = reader["Color"].ToString(),
                    };
                    ListCandidateStatus.Add(item);
                }
                connection.Close();
                SqlConnection.ClearAllPools();
            }
            return ListCandidateStatus;
        }

        //It Creates a Candidate Status
        public void CreateCandidateStatus(CandidateStatus newStatus)
        {
            if (connection.State == ConnectionState.Closed)
                CreateConnection();

            command.CommandText = "INSERT INTO dbo.CandidateStatus (StatusDescription, Activated, Color) VALUES(@Description, @Activated, @Color)";
            command.Parameters.Add("@Description", SqlDbType.VarChar).Value = newStatus.Description;
            command.Parameters.Add("@Activated", SqlDbType.Int).Value = newStatus.Activated;
            command.Parameters.Add("@Color", SqlDbType.VarChar).Value = newStatus.Color;

            using (connection)
            {
                reader = command.ExecuteReader();
            }
            connection.Close();
        }

        //It Edits a certain Candidate Status
        public void EditCandidateStatus(CandidateStatus update)
        {
            if (connection.State == ConnectionState.Closed)
                CreateConnection();

            command.CommandText = "UPDATE dbo.CandidateStatus SET StatusDescription = @Description, Activated = @Active, Color = @Color WHERE IdStatus = @IdStatus";

            command.Parameters.Add("@IdStatus", SqlDbType.Int).Value = update.IdStatus;
            command.Parameters.Add("@Description", SqlDbType.VarChar).Value = update.Description;
            command.Parameters.Add("@Active", SqlDbType.Int).Value = update.Activated;
            command.Parameters.Add("@Color", SqlDbType.VarChar).Value = update.Color;

            using (connection)
            {
                reader = command.ExecuteReader();
            }
            connection.Close();
        }

        //It gets a list of Availability
        public List<Availability> QueryAvailability()
        {
            //connection = new SqlConnection();
            List<Availability> ListAvailability = new List<Availability>();

            try
            {
                using (connection)
                {
                    command.CommandText = "SELECT * FROM dbo.Availability";
                    reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Availability availability = new Availability()
                        {
                            IdAvailability = int.Parse(reader["IdAvailability"].ToString()),
                            Description = reader["AvailabilityDescription"].ToString(),
                            Activated = int.Parse(reader["Activated"].ToString()),
                        };
                        ListAvailability.Add(availability);
                    }
                    connection.Close();
                }
                SqlConnection.ClearAllPools();
                return ListAvailability;
            }
            catch (Exception ex) { throw ex; }

        }

        //It Creates an Availability
        public void CreateAvailability(Availability newAvailability)
        {
            connection = new SqlConnection();
            command.CommandText = "INSERT INTO dbo.Availability (AvailabilityDescription, Activated) VALUES(@Description, @Activated)";
            command.Parameters.Add("@Description", SqlDbType.VarChar).Value = newAvailability.Description;
            command.Parameters.Add("@Activated", SqlDbType.Int).Value = newAvailability.Activated;

            using (connection)
            {
                reader = command.ExecuteReader();
            }
            connection.Close();
        }

        //It Edits a certain Availability
        public void EditAvailability(Availability update)
        {
            if (connection.State == ConnectionState.Closed)
                CreateConnection();

            command.CommandText = "UPDATE dbo.Availability SET AvailabilityDescription = @Description, Activated = @Active WHERE IdAvailability = @IdAvailability";

            command.Parameters.Add("@IdAvailability", SqlDbType.Int).Value = update.IdAvailability;
            command.Parameters.Add("@Description", SqlDbType.VarChar).Value = update.Description;
            command.Parameters.Add("@Active", SqlDbType.Int).Value = update.Activated;

            using (connection)
            {
                reader = command.ExecuteReader();
            }
            connection.Close();
        }

        //It gets a List of Locations
        public List<Location> QueryLocation()
        {
            List<Location> ListLocations = new List<Location>();

            using (connection)
            {
                command.CommandText = "SELECT * FROM dbo.Location";
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Location item = new Location()
                    {
                        IdLocation = int.Parse(reader["IdLocation"].ToString()),
                        Description = reader["LocationDescription"].ToString(),
                        Activated = int.Parse(reader["Active"].ToString()),
                    };
                    ListLocations.Add(item);
                }
                connection.Close();
                SqlConnection.ClearAllPools();
                return ListLocations;
            }
        }

        //It Creates a Location
        public void CreateLocation(Location newLocation)
        {
            connection = new SqlConnection();
            command.CommandText = "INSERT INTO dbo.Location (LocationDescription, Active) VALUES(@Description, @Activated)";
            command.Parameters.Add("@Description", SqlDbType.VarChar).Value = newLocation.Description;
            command.Parameters.Add("@Activated", SqlDbType.Int).Value = newLocation.Activated;

            using (connection)
            {
                reader = command.ExecuteReader();
            }
            connection.Close();
        }

        //It Edits a certain Availability
        public void EditLocation(Location update)
        {
            if (connection.State == ConnectionState.Closed)
                CreateConnection();

            command.CommandText = "UPDATE dbo.Location SET LocationDescription = @Description, Active = @Active WHERE IdLocation = @IdLocation";

            command.Parameters.Add("@IdLocation", SqlDbType.Int).Value = update.IdLocation;
            command.Parameters.Add("@Description", SqlDbType.VarChar).Value = update.Description;
            command.Parameters.Add("@Active", SqlDbType.Int).Value = update.Activated;

            using (connection)
            {
                reader = command.ExecuteReader();
            }
            connection.Close();
        }


        /// <summary>
        /// //////////////////////////
        /// USERS AND ROLES
        /// ////////////////////////
        /// </summary>
        

        //It Lists Users
        public List<User> GetUsers()
        {
            if (connection.State == ConnectionState.Closed)
                CreateConnection();

            List<User> listUsers = new List<User>();

            using (connection)
            {
                //command.CommandText = "Select [User].IdUser,[User].Name,[User].Password,UserUserRole.Description,WPApp.WPCode,UserRoleWP.TypeOfAccess,UserRoleWP.Description As Description1, WPApp.Description As Description3," +
                //    " UserRole.Description As Description2,[User].Active From [User] Inner Join UserUserRole On UserUserRole.IdUser = [User].IdUser Inner Join UserRole On UserUserRole.IdUserRole = UserRole.IdUserRole " +
                //    "Inner Join UserRoleWP On UserRoleWP.IdUserRole = UserRole.IdUserRole Inner Join WPApp On UserRoleWP.IdWP = WPApp.IdWP";
                //command.CommandText = "Select [User].* from [User]";

                //command.CommandText = "Select [User].* from [User]";

                command.CommandText = "Select [User].*, UserUserRole.IdUUR, UserUserRole.Description, UserRole.Description As RoleDescription, UserRole.IdUserRole " +
                    "From [User] Inner Join UserUserRole On UserUserRole.IdUser = [User].IdUser Inner Join UserRole On UserUserRole.IdUserRole = UserRole.IdUserRole";

                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    User item = new User()
                    {
                        IdUser = int.Parse(reader["IdUser"].ToString()),
                        Name = reader["Name"].ToString(),
                        Password = reader["Password"].ToString(),
                        Activated = int.Parse(reader["Active"].ToString()),
                        UserRoleDescription = reader["RoleDescription"].ToString(),
                        IdUserRole = int.Parse(reader["IdUserRole"].ToString()),
                        //UserRoleDescription = reader["RoleDescription"].ToString(),
                        //WPCode = reader["WPCode"].ToString(),
                        //TypeOfAccess = reader["TypeOfAccess"].ToString(),
                    };
                    listUsers.Add(item);
                }
            }
            connection.Close();
            return listUsers;
        }

        //Creates a new User
        public void CreateUser(User newUser)
        {
            try
            {
                if (connection.State == ConnectionState.Closed)
                    CreateConnection();

                command.CommandText = "INSERT INTO [User] (Name, Password, Active) VALUES(@Name, @Password, @Active)";

                command.Parameters.Add("@Name", SqlDbType.VarChar).Value = newUser.Name.ToString();
                command.Parameters.Add("@Password", SqlDbType.VarChar).Value = newUser.Password.ToString();
                command.Parameters.Add("@Active", SqlDbType.Int).Value = 1;

                using (connection)
                {
                    reader = command.ExecuteReader();
                }
                connection.Close();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void EditUser(User update)
        {
            try
            {
                if (connection.State == ConnectionState.Closed)
                    CreateConnection();

                command.CommandText = "UPDATE [User] SET Name = @Name, Password = @Password, Active = @Active WHERE IdUser = @IdUser";

                command.Parameters.Add("@Name", SqlDbType.VarChar).Value = update.Name;
                command.Parameters.Add("@Password", SqlDbType.VarChar).Value = update.Password;
                command.Parameters.Add("@Active", SqlDbType.Int).Value = update.Activated;
                command.Parameters.Add("@IdUser", SqlDbType.Int).Value = update.IdUser;

                using (connection)
                {
                    reader = command.ExecuteReader();
                }
                connection.Close();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void EditUserUserRole(UserUserRoles uur)
        {
            try
            {
                if (connection.State == ConnectionState.Closed)
                    CreateConnection();

                command.Parameters.Clear();
                command.CommandText = "UPDATE [UserUserRole] SET IdUserRole = @IdUserRole WHERE IdUser = @IdUser";

                command.Parameters.Add("@IdUserRole", SqlDbType.Int).Value = uur.IdUserRole;
                command.Parameters.Add("@IdUser", SqlDbType.Int).Value = uur.IdUser;

                using (connection)
                {
                    reader = command.ExecuteReader();
                }
                connection.Close();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public void CreateUserUserRole(UserUserRoles uur)
        {
            try
            {
                if (connection.State == ConnectionState.Closed)
                    CreateConnection();

                command.CommandText = "INSERT INTO [UserUserRole] (IdUser, IdUserRole, Description, Active) VALUES(@IdUser, @IdUserRole, @Description, @Active)";

                command.Parameters.Add("@IdUser", SqlDbType.Int).Value = uur.IdUser;
                command.Parameters.Add("@IdUserRole", SqlDbType.Int).Value = uur.IdUserRole;
                command.Parameters.Add("@Description", SqlDbType.VarChar).Value = uur.Description;
                command.Parameters.Add("@Active", SqlDbType.Int).Value = 1;

                using (connection)
                {
                    reader = command.ExecuteReader();
                }
                connection.Close();
            }
            catch(Exception e){
                throw e;
            }
        }

        public int GetLastUserId()
        {
            try
            {
                if (connection.State == ConnectionState.Closed)
                    CreateConnection();
                int idUser = 0;
                command.CommandText = "Select [User].IdUser From [User] ORDER BY idUser DESC";

                reader = command.ExecuteReader();
                reader.Read();
                using (connection)
                {
                    idUser = int.Parse(reader["IdUser"].ToString());
                }
                connection.Close();
                return idUser;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// UserRoles////////
        /// </summary>
        public List<UserRoles> GetUserRolesList()
        {
            try
            {
                List<UserRoles> listUserRoles = new List<UserRoles>();
                using (connection)
                {
                    //command.CommandText = "Select UserRole.IdUserRole, [User].Name, [User].Password, [User].Active, UserRole.Description, UserRole.Active As Active1, UserRoleWP.Description As Description1, UserRoleWP.TypeOfAccess, WPApp.WPCode, WPApp.Description As Description2 " +
                    //    "From [User] Inner Join UserUserRole On UserUserRole.IdUser = [User].IdUser Inner Join UserRole On UserUserRole.IdUserRole = UserRole.IdUserRole Inner Join UserRoleWP On UserRoleWP.IdUserRole = UserRole.IdUserRole "+
                    //    "Inner Join WPApp On UserRoleWP.IdWP = WPApp.IdWP";

                    command.CommandText = "Select UserRole.* From UserRole";

                    reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        UserRoles item = new UserRoles()
                        {
                            IdUserRole = int.Parse(reader["IdUserRole"].ToString()),
                            Description = reader["Description"].ToString(),
                            Active = int.Parse(reader["Active"].ToString()),
                            //WPCode = reader["WPCode"].ToString(),
                            //TypeOfAccess = reader["TypeOfAccess"].ToString(),
                        };
                        listUserRoles.Add(item);
                    }
                }
                connection.Close();
                return listUserRoles;
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        public void CreateUserRole(UserRoles role)
        {
            try
            {
                using (connection)
                {
                    if (connection.State == ConnectionState.Closed)
                        CreateConnection();

                    command.CommandText = "INSERT INTO dbo.UserRole (Description, Active) VALUES(@Description, @Activated)";
                    command.Parameters.Add("@Description", SqlDbType.VarChar).Value = role.Description;
                    command.Parameters.Add("@Active", SqlDbType.Int).Value = 1;

                    reader = command.ExecuteReader();
                }
                connection.Close();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public int GetLastUserRole() //Gets the last UserRole created for use in creating and assigning user roles and their level of access
        {
            try
            {
                using (connection)
                {
                    command.CommandText = "SELECT TOP 1 IdUserRole FROM dbo.UserRole ORDER BY IdUserRole DESC";
                    reader = command.ExecuteReader();
                    int i = 0;
                    using (connection)
                    {

                        while (reader.Read())
                        {
                            i = int.Parse(reader["IdLog"].ToString());
                        }
                    }
                    connection.Close();
                    return i;
                }
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        public void CreateUserRoleWP()//rever
        {
            try
            {
                using (connection)
                {
                    if (connection.State == ConnectionState.Closed)
                        CreateConnection();

                    command.CommandText = "INSERT INTO dbo.WPApp (WPCode, Description, Active) VALUES(@WPCode, @Description, @Activated)";
                    //command.Parameters.Add("@Description", SqlDbType.VarChar).Value = role.Description;
                    command.Parameters.Add("@Active", SqlDbType.Int).Value = 1;

                    reader = command.ExecuteReader();
                }
                connection.Close();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        //Gets a List of CompanyRoles
        public List<CompanyRoles> GetListCompanyRoles()
        {
            try
            {
                if (connection.State == ConnectionState.Closed)
                    CreateConnection();

                List<CompanyRoles> listCRoles = new List<CompanyRoles>();

                using (connection)
                {
                    command.CommandText = "SELECT * FROM dbo.CompanyRoles";


                    reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        CompanyRoles item = new CompanyRoles()
                        {
                            IdCRoles = int.Parse(reader["IdCRoles"].ToString()),
                            CRoleName = reader["CRoleName"].ToString(),
                            IdDepartment = int.Parse(reader["IdDepartment"].ToString()),
                        };
                        listCRoles.Add(item);
                    }
                }
                connection.Close();

                return listCRoles;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// WP////////
        /// </summary>
        public List<WorkPackage> GetListWP()
        {
            try
            {
                List<WorkPackage> listWP = new List<WorkPackage>();
                using (connection)
                {
                    command.CommandText = "Select * From WPApp";

                    reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        WorkPackage item = new WorkPackage()
                        {
                            IdWP = int.Parse(reader["IdWP"].ToString()),
                            WPCode = reader["WPCode"].ToString(),
                            Description = reader["Description"].ToString(),
                            Active = int.Parse(reader["Active"].ToString()),
                            WPEmail = reader["WPEmail"].ToString(),
                        };
                        listWP.Add(item);
                    }
                }
                connection.Close();
                return listWP;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void CreateWP(WorkPackage newWP)
        {
            try
            {
                using (connection)
                {
                     if (connection.State == ConnectionState.Closed)
                        CreateConnection();

                    command.CommandText = "INSERT INTO dbo.WPApp (WPCode, Description, Active, WPEmail) VALUES(@WPCode, @Description, @Activated, @WPEmail)";
                    command.Parameters.Add("@Description", SqlDbType.VarChar).Value = newWP.Description;
                    command.Parameters.Add("@WPCode", SqlDbType.VarChar).Value = newWP.WPCode;
                    command.Parameters.Add("@WPEmail", SqlDbType.VarChar).Value = newWP.WPEmail;
                    command.Parameters.Add("@Activated", SqlDbType.Int).Value = newWP.Active;

                    reader = command.ExecuteReader();
                }
                connection.Close();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary> 
        /// Customers////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>

        public List<Customers> GetListCustomers()
        {
            try
            {
                List<Customers> listCustomers = new List<Customers>();
                using (connection)
                {
                    //if (connection.State == ConnectionState.Closed)
                    CreateConnection();

                    command.CommandText = "SELECT * FROM Customers";
                    reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Customers item = new Customers()
                        {
                            IdCustomer = int.Parse(reader["IdCustomer"].ToString()),
                            ClientName = reader["ClientName"].ToString(),
                            NIF = int.Parse(reader["NIF"].ToString()),
                            LegalRepresentative = reader["LegalRepresentative"].ToString(),
                            Adress = reader["Adress"].ToString(),
                            Email = reader["Email"].ToString(),
                            FinancialOrProcurementDepartmentEmail = reader["Financial_ProcurementDepartmentEmail"].ToString(),
                            Active = int.Parse(reader["Active"].ToString()),
                        };
                        listCustomers.Add(item);
                    }
                }
                connection.Close();
                SqlConnection.ClearAllPools();
                return listCustomers;
            }
            catch(Exception e)
            {
                SqlConnection.ClearAllPools();
                throw e;
            }
        }

        public void CreateCustomer(Customers newCustomer)
        {
            try
            {
                CreateConnection();

                using(connection)
                {
                    command.CommandText = "INSERT INTO dbo.Customers (ClientName, NIF, LegalRepresentative, Adress, Email, Financial_ProcurementDepartmentEmail, Active) " +
                        " VALUES(@ClientName, @NIF, @LegalRepresentative, @Adress, @Email, @Financial_ProcurementDepartmentEmail, @Active)";

                    command.Parameters.Add("@ClientName", SqlDbType.VarChar).Value = newCustomer.ClientName.ToString();
                    command.Parameters.Add("@NIF", SqlDbType.Int).Value = newCustomer.NIF;
                    command.Parameters.Add("@LegalRepresentative", SqlDbType.VarChar).Value = newCustomer.LegalRepresentative.ToString();
                    command.Parameters.Add("@Adress", SqlDbType.VarChar).Value = newCustomer.Adress.ToString();
                    command.Parameters.Add("@Email", SqlDbType.VarChar).Value = newCustomer.Email.ToString();
                    command.Parameters.Add("@Financial_ProcurementDepartmentEmail", SqlDbType.VarChar).Value = newCustomer.FinancialOrProcurementDepartmentEmail.ToString();
                    command.Parameters.Add("@Active", SqlDbType.Int).Value = newCustomer.Active;

                    reader = command.ExecuteReader();
                }
                connection.Close();
                SqlConnection.ClearAllPools();
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        public void UpdateCustomer(Customers updateCustomer)
        {
            try
            {
                CreateConnection();

                using (connection)
                {
                    command.CommandText = "UPDATE dbo.Customers SET ClientName = @ClientName, NIF = @NIF, LegalRepresentative = @LegalRepresentative, Adress = @Adress, Email = @Email, Financial_ProcurementDepartmentEmail = @Financial_ProcurementDepartmentEmail,"+
                        "Active = @Active WHERE IdCustomer = @IdCustomer";

                    command.Parameters.Add("@ClientName", SqlDbType.VarChar).Value = updateCustomer.ClientName.ToString();
                    command.Parameters.Add("@NIF", SqlDbType.Int).Value = updateCustomer.NIF;
                    command.Parameters.Add("@LegalRepresentative", SqlDbType.VarChar).Value = updateCustomer.LegalRepresentative.ToString();
                    command.Parameters.Add("@Adress", SqlDbType.VarChar).Value = updateCustomer.Adress.ToString();
                    command.Parameters.Add("@Email", SqlDbType.VarChar).Value = updateCustomer.Email.ToString();
                    command.Parameters.Add("@Financial_ProcurementDepartmentEmail", SqlDbType.VarChar).Value = updateCustomer.FinancialOrProcurementDepartmentEmail.ToString();
                    command.Parameters.Add("@Active", SqlDbType.Int).Value = updateCustomer.Active;
                    command.Parameters.Add("@IdCustomer", SqlDbType.Int).Value = updateCustomer.IdCustomer;

                    reader = command.ExecuteReader();
                }
                connection.Close();
                SqlConnection.ClearAllPools();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary> 
        /// Leads////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>

        public List<LeadsFullInfo> GetLeadsList()
        {
            try
            {
                List<LeadsFullInfo> listLeads = new List<LeadsFullInfo>();
                using (connection)
                {
                    if (connection.State == ConnectionState.Closed)
                        CreateConnection();

                    command.CommandText = "Select Leads.*, Customers.ClientName, Customers.NIF, Customers.LegalRepresentative, Customers.Adress, Customers.Email, Customers.Financial_ProcurementDepartmentEmail, " +
                        "Availability.AvailabilityDescription, Nationality.NationalityDescription, LeadStatus.Description As LeadStatusDescription, LeadStatus.Color, Roles.NameRole, Skills.NameSkill From Leads " +
                        "Inner Join Customers On Leads.IdCustomer = Customers.IdCustomer Inner Join Availability On Leads.IdAvailability = Availability.IdAvailability Inner Join " +
                        "LeadStatus On Leads.IdLeadStatus = LeadStatus.IdLeadStatus Inner Join Nationality On Leads.IdNationality = Nationality.IdNationality Inner Join " +
                        "Roles On Leads.IdRole = Roles.IdRole Inner Join Skills On Leads.IdSkill = Skills.IdSkill";

                    reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        LeadsFullInfo item = new LeadsFullInfo()
                        {
                            IdLead = int.Parse(reader["IdLeads"].ToString()),
                            IdCustomer = int.Parse(reader["IdCustomer"].ToString()),
                            LeadCode = reader["LeadCode"].ToString(),
                            IdLeadStatus = int.Parse(reader["IdLeadStatus"].ToString()),
                            ClientName = reader["ClientName"].ToString(),
                            Adress = reader["Adress"].ToString(),
                            NIF = int.Parse(reader["IdLeadStatus"].ToString()),
                            LegalRepresentative = reader["LegalRepresentative"].ToString(),
                            Email = reader["Email"].ToString(),
                            Financial_ProcurementDepartmentEmail = reader["Financial_ProcurementDepartmentEmail"].ToString(),
                            AvailabilityDescription = reader["AvailabilityDescription"].ToString(),
                            NationalityDescription = reader["NationalityDescription"].ToString(),
                            LeadStatusDescription = reader["LeadStatusDescription"].ToString(),
                            LeadStatusColor = reader["Color"].ToString(),
                            IdRole = int.Parse(reader["IdRole"].ToString()),
                            NameRole = reader["NameRole"].ToString(),
                            IdSkill = int.Parse(reader["IdSkill"].ToString()),
                            NameSkill = reader["NameSkill"].ToString(),
                            RoleExp = int.Parse(reader["RoleExp"].ToString()),
                            SkillExp = int.Parse(reader["SkillExp"].ToString()),
                            IdNationality = int.Parse(reader["IdNationality"].ToString()),
                            MinProfessionalExp = int.Parse(reader["MinProfessionalExp"].ToString()),
                            MaxProfessionalExp = int.Parse(reader["MaxProfessionalExp"].ToString()),
                            Description = reader["Description"].ToString(),
                            MaxRemuneration = int.Parse(reader["MaxRemuneration"].ToString()),
                            MinRemuneration = int.Parse(reader["MinRemuneration"].ToString()),
                            IdAvailability = int.Parse(reader["IdAvailability"].ToString()),
                            MaxAge = int.Parse(reader["MaxAge"].ToString()),
                            MinAge = int.Parse(reader["MinAge"].ToString()),
                            //IdAction = int.Parse(reader["IdAction"].ToString()),
                        };
                        listLeads.Add(item);
                    }
                }
                connection.Close();
                SqlConnection.ClearAllPools();
                return listLeads;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void CreateLead(Leads newLead)
        {
            try
            {
                using (connection)
                {
                    if(connection.State == ConnectionState.Closed)
                        CreateConnection();

                    List<LeadsFullInfo> ListLeads = new List<LeadsFullInfo>();

                    using (connection)
                    {
                        command.CommandText = "INSERT INTO dbo.Leads (IdCustomer, IdLeadStatus, Description, LeadCode, IdSkill, SkillExp, IdRole, RoleExp, MinProfessionalExp, MaxProfessionalExp, IdNationality, MinRemuneration," +
                            " MaxRemuneration, MinAge, MaxAge, IdAvailability) VALUES(@IdCustomer, @IdLeadStatus, @Description, @LeadCode, @IdSkill, @SkillExp, @IdRole, @RoleExp, @MinProfessionalExp, @MaxProfessionalExp," +
                            " @IdNationality, @MinRemuneration, @MaxRemunetarion, @MinAge, @MaxAge, @IdAvailability)";

                        command.Parameters.Add("@IdCustomer", SqlDbType.Int).Value = newLead.IdCustomer;
                        command.Parameters.Add("@IdLeadStatus", SqlDbType.Int).Value = newLead.IdLeadStatus;
                        command.Parameters.Add("@Description", SqlDbType.VarChar).Value = newLead.Description;
                        command.Parameters.Add("@IdSkill", SqlDbType.VarChar).Value = newLead.IdSkill;
                        command.Parameters.Add("@SkillExp", SqlDbType.VarChar).Value = newLead.SkillExp;
                        command.Parameters.Add("@IdRole", SqlDbType.Int).Value = newLead.IdRole;
                        command.Parameters.Add("@RoleExp", SqlDbType.Int).Value = newLead.RoleExp;
                        command.Parameters.Add("@IdNationality", SqlDbType.VarChar).Value = newLead.IdNationality;
                        command.Parameters.Add("@MaxAge", SqlDbType.VarChar).Value = newLead.MaxAge;
                        command.Parameters.Add("@MinAge", SqlDbType.VarChar).Value = newLead.MinAge;
                        command.Parameters.Add("@MinRemuneration", SqlDbType.Int).Value = newLead.MinRemuneartion;
                        command.Parameters.Add("@MaxRemunetarion", SqlDbType.Int).Value = newLead.MaxRemuneartion;
                        command.Parameters.Add("@MinProfessionalExp", SqlDbType.VarChar).Value = newLead.MaxProfExp;
                        command.Parameters.Add("@MaxProfessionalExp", SqlDbType.VarChar).Value = newLead.MinProfExp;
                        command.Parameters.Add("@IdAvailability", SqlDbType.VarChar).Value = newLead.IdAvailability;
                        command.Parameters.Add("@LeadCode", SqlDbType.VarChar).Value = newLead.LeadCode;

                        reader = command.ExecuteReader();
                    }
                    connection.Close();
                    SqlConnection.ClearAllPools();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        //APAGAR?
        public void CreateLeadRequisite(Leads requisite)
        {
            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    this.CreateConnection();
                    command.Parameters.Clear();
                }
                    
                using (connection)
                {
                    command.CommandText = "INSERT INTO dbo.LeadRequisite (IdLead, Description, IdSkill, SkillExp, IdNationality, MinProfessionalExp, MaxProfessionalExp, MinRemuneration, MaxRemuneration, MinAge, MaxAge, MaxAvailability) " +
                                                " VALUES(@IdLead, @Description, @IdSkill, @SkillExp, @IdNationality, @MinProfExp, @MaxProfExp, @MinRemuneration, @MaxRemuneration, @MinAge, @MaxAge, @MaxAvailability)";

                    command.Parameters.Add("@IdLead", SqlDbType.Int).Value = requisite.IdLead;
                    command.Parameters.Add("@IdSkill", SqlDbType.Int).Value = requisite.IdSkill;
                    command.Parameters.Add("@SkillExp", SqlDbType.Int).Value = requisite.SkillExp;
                    command.Parameters.Add("@IdNationality", SqlDbType.Int).Value = requisite.IdNationality;
                    command.Parameters.Add("@MinProfExp", SqlDbType.Int).Value = requisite.MinProfExp;
                    command.Parameters.Add("@MaxProfExp", SqlDbType.Int).Value = requisite.MaxProfExp;
                    command.Parameters.Add("@MinRemuneration", SqlDbType.Int).Value = requisite.MinRemuneartion;
                    command.Parameters.Add("@MaxRemuneration", SqlDbType.Int).Value = requisite.MaxRemuneartion;
                    command.Parameters.Add("@MinAge", SqlDbType.Int).Value = requisite.MinAge;
                    command.Parameters.Add("@MaxAge", SqlDbType.Int).Value = requisite.MaxAge;
                    command.Parameters.Add("@MaxAvailability", SqlDbType.Int).Value = requisite.MaxAvailability;
                    command.Parameters.Add("@Description", SqlDbType.VarChar).Value = requisite.Description;

                    reader = command.ExecuteReader();
                }
                connection.Close();
                SqlConnection.ClearAllPools();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void EditLead(Leads update)
        {
            try
            {
                if(connection.State != ConnectionState.Open)
                    CreateConnection();

                using (connection)
                {
                    command.CommandText = "UPDATE dbo.Leads SET IdCustomer = @IdCustomer, Description = @Description, IdLeadStatus = @IdLeadStatus, IdSkill = @IdSkill, SkillExp = @SkillExp, " +
                        "IdRole = @IdRole, RoleExp = @RoleExp, IdNationality = @IdNationality, MaxAge = @MaxAge, MinAge = @MinAge, MinRemuneration = @MinRemuneration, " +
                        "MaxRemuneration = @MaxRemunetarion, MaxProfessionalExp = @MaxProfessionalExp, MinProfessionalExp = @MinProfessionalExp, IdAvailability = @IdAvailability, LeadCode = @LeadCode WHERE IdLeads = @IdLead";

                    command.Parameters.Add("@IdCustomer", SqlDbType.Int).Value = update.IdCustomer;
                    command.Parameters.Add("@IdLeadStatus", SqlDbType.Int).Value = update.IdLeadStatus;
                    command.Parameters.Add("@Description", SqlDbType.VarChar).Value = update.Description;
                    command.Parameters.Add("@IdSkill", SqlDbType.VarChar).Value = update.IdSkill;
                    command.Parameters.Add("@SkillExp", SqlDbType.VarChar).Value = update.SkillExp;
                    command.Parameters.Add("@IdRole", SqlDbType.Int).Value = update.IdRole;
                    command.Parameters.Add("@RoleExp", SqlDbType.Int).Value = update.RoleExp;
                    command.Parameters.Add("@IdNationality", SqlDbType.VarChar).Value = update.IdNationality;
                    command.Parameters.Add("@MaxAge", SqlDbType.VarChar).Value = update.MaxAge;
                    command.Parameters.Add("@MinAge", SqlDbType.VarChar).Value = update.MinAge;
                    command.Parameters.Add("@MinRemuneration", SqlDbType.Int).Value = update.MinRemuneartion;
                    command.Parameters.Add("@MaxRemunetarion", SqlDbType.Int).Value = update.MaxRemuneartion;
                    command.Parameters.Add("@MinProfessionalExp", SqlDbType.VarChar).Value = update.MinProfExp;
                    command.Parameters.Add("@MaxProfessionalExp", SqlDbType.VarChar).Value = update.MaxProfExp; 
                    command.Parameters.Add("@IdAvailability", SqlDbType.VarChar).Value = update.IdAvailability;
                    command.Parameters.Add("@LeadCode", SqlDbType.VarChar).Value = update.LeadCode;
                    command.Parameters.Add("@IdLead", SqlDbType.VarChar).Value = update.IdLead;

                    reader = command.ExecuteReader();
                }
                connection.Close();
                SqlConnection.ClearAllPools();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public int GetLastLeadId()
        {
            try
            {
                this.CreateConnection();

                command.CommandText = "SELECT TOP 1 IdLeads FROM dbo.Leads ORDER BY IdLeads DESC";
                reader = command.ExecuteReader();
                int i = 0;
                using (connection)
                {

                    while (reader.Read())
                    {
                        i = int.Parse(reader["IdLeads"].ToString());
                    }
                }
                connection.Close();
                return i;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<LeadNotes> GetListLeadsNotes()
        {
            try
            {
                if(connection.State == ConnectionState.Closed)
                    CreateConnection();

                List<LeadNotes> ListNotes = new List<LeadNotes>();

                command.CommandText = "Select LeadNotes.*, [User].Name FROM LeadNotes  Inner Join [User] On LeadNotes.IdUser = [User].IdUser Order By IdLeadNote DESC, TimeStamp DESC";

                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    LeadNotes item = new LeadNotes()
                    {
                        IdLeadNote = int.Parse(reader["IdLeadNote"].ToString()),
                        IdLead = int.Parse(reader["IdLeads"].ToString()),
                        Active = int.Parse(reader["Active"].ToString()),
                        Description = reader["Description"].ToString(),
                        TimeStamp = reader["TimeStamp"].ToString(),
                        User = reader["Name"].ToString(),
                    };
                    ListNotes.Add(item);
                }
                connection.Close();
                SqlConnection.ClearAllPools();
                return ListNotes;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void CreateLeadsNote(LeadNotes note)
        {
            try
            {
                if (connection.State == ConnectionState.Closed)
                    CreateConnection();

                List<LeadNotes> ListNotes = new List<LeadNotes>();

                command.CommandText = "INSERT INTO dbo.LeadNotes (IdLeads, Description, TimeStamp, Active, IdUser) VALUES(@IdLeads, @Description, @TimeStamp, @Active, @IdUser)";

                command.Parameters.Add("@IdLeads", SqlDbType.Int).Value = note.IdLead;
                command.Parameters.Add("@Description", SqlDbType.VarChar).Value = note.Description;
                command.Parameters.Add("@TimeStamp", SqlDbType.VarChar).Value = note.TimeStamp;
                command.Parameters.Add("@Active", SqlDbType.Int).Value = note.Active;
                command.Parameters.Add("@IdUser", SqlDbType.Int).Value = note.IdUser;

                reader = command.ExecuteReader();
                
                connection.Close();
                SqlConnection.ClearAllPools();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void EditLeadsNote(LeadNotes update)
        {
            try
            {
                if (connection.State == ConnectionState.Closed)
                    CreateConnection();

                List<LeadNotes> ListNotes = new List<LeadNotes>();

                command.CommandText = "UPDATE dbo.LeadNotes SET Description = @Description, TimeStamp = @TimeStamp, IdUser = @IdUser WHERE IdLeadNote = @IdLeadNote";

                command.Parameters.Add("@Description", SqlDbType.VarChar).Value = update.Description;
                command.Parameters.Add("@TimeStamp", SqlDbType.VarChar).Value = update.TimeStamp;
                command.Parameters.Add("@IdUser", SqlDbType.Int).Value = update.IdUser;
                command.Parameters.Add("@IdLeadNote", SqlDbType.Int).Value = update.IdLeadNote;

                reader = command.ExecuteReader();

                connection.Close();
                SqlConnection.ClearAllPools();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void DeleteLeadNote(int idNote)
        {
            try
            {
                if (connection.State == ConnectionState.Closed)
                    CreateConnection();

                List<LeadNotes> ListNotes = new List<LeadNotes>();

                command.CommandText = "UPDATE dbo.LeadNotes SET Active = @Active WHERE IdLeadNote = @IdLeadNote";

                command.Parameters.Add("@Active", SqlDbType.Int).Value = 0;
                command.Parameters.Add("@IdLeadNote", SqlDbType.Int).Value = idNote;

                reader = command.ExecuteReader();

                connection.Close();
                SqlConnection.ClearAllPools();
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
                List<CandidateLeadStatus> listCLS = new List<CandidateLeadStatus>();
                using (connection)
                {
                    if (connection.State == ConnectionState.Closed)
                        CreateConnection();

                    command.CommandText = "Select * From CandidateLeadStatus";

                    reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        CandidateLeadStatus item = new CandidateLeadStatus()
                        {
                            IdCandidateLeadStatus = int.Parse(reader["IdCandidateLeadStatus"].ToString()),
                            Description = reader["Description"].ToString(),
                            Active = int.Parse(reader["Active"].ToString()),
                        };
                        listCLS.Add(item);
                    }
                }
                connection.Close();
                SqlConnection.ClearAllPools();
                return listCLS;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<LeadsCandidates> QueryLeadsCandidates()
        {
            try
            {
                List<LeadsCandidates> list = new List<LeadsCandidates>();
                if (connection.State == ConnectionState.Closed)
                    CreateConnection();
                using (connection)
                {
                    //command.CommandText = "SELECT LeadsCandidates.* from [LeadsCandidates]";

                    command.CommandText = "Select LeadsCandidates.*, CandidateLeadStatus.Description " +
                        "From LeadsCandidates Inner Join CandidateLeadStatus On LeadsCandidates.IdCandidateLeadStatus = CandidateLeadStatus.IdCandidateLeadStatus";

                    reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        LeadsCandidates lc = new LeadsCandidates()
                        {
                            IdLeadCandidates = int.Parse(reader["IdLeadCandidates"].ToString()),
                            IdLead = int.Parse(reader["IdLead"].ToString()),
                            IdCandidate = int.Parse(reader["IdCandidate"].ToString()),
                            IdCandidateLeadStatus = int.Parse(reader["IdCandidateLeadStatus"].ToString()),
                            CandidateLeadStatusDescription = reader["Description"].ToString()
                        };

                        list.Add(lc);
                    }
                }
                connection.Close();
                return list;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<LeadsCandidates> QueryLeadsCandidatesOfACertaiLead(int idLead)
        {
            try
            {
                List<LeadsCandidates> list = new List<LeadsCandidates>();

                if (connection.State == ConnectionState.Closed)
                    CreateConnection();
                using (connection)
                {
                    command.CommandText = "SELECT LeadsCandidates.* from [LeadsCandidates] where LeadsCandidates.IdLead = @idLead";
                    command.Parameters.Add("@IdLead", SqlDbType.Int).Value = idLead;
                    reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        LeadsCandidates lc = new LeadsCandidates()
                        {
                            IdLeadCandidates = int.Parse(reader["IdLeadCandidates"].ToString()),
                            IdLead = int.Parse(reader["IdLead"].ToString()),
                            IdCandidate = int.Parse(reader["IdCandidate"].ToString()),
                            IdCandidateLeadStatus = int.Parse(reader["IdCandidateLeadStatus"].ToString()),
                        };

                        list.Add(lc);
                    }
                }
                connection.Close();
                command.Parameters.Clear();
                return list;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void CreateLeadCandidate(LeadsCandidates lc)
        {
            try
            {
                if (connection.State == ConnectionState.Closed)
                    CreateConnection();

                command.CommandText = "INSERT INTO dbo.LeadsCandidates (IdLead, IdCandidate, IdCandidateLeadStatus) " +
                        " VALUES(@IdLead, @IdCandidate, @IdCandidateLeadStatus)";

                command.Parameters.Add("@IdLead", SqlDbType.Int).Value = lc.IdLead;
                command.Parameters.Add("@IdCandidate", SqlDbType.Int).Value = lc.IdCandidate;
                command.Parameters.Add("@IdCandidateLeadStatus", SqlDbType.Int).Value = lc.IdCandidateLeadStatus;

                reader = command.ExecuteReader();
                command.Parameters.Clear();
                CloseDBConnections();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void UpdateLeadCandidate(LeadsCandidates lc)
        {
            try
            {
                if (connection.State == ConnectionState.Closed)
                    CreateConnection();
                using (connection)
                {
                    command.CommandText = "UPDATE dbo.LeadsCandidates SET IdCandidateLeadStatus = @IdCandidateLeadStatus WHERE IdLead = @IdLead AND IdCandidate = @IdCandidate";

                    command.Parameters.Add("@IdCandidateLeadStatus", SqlDbType.Int).Value = lc.IdCandidateLeadStatus;
                    command.Parameters.Add("@IdLead", SqlDbType.Int).Value = lc.IdLead;
                    command.Parameters.Add("@IdCandidate", SqlDbType.Int).Value = lc.IdCandidate;

                    reader = command.ExecuteReader();
                }
                connection.Close();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        /// <summary> 
        /// LeadStatus////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        public List<LeadStatus> GetLeadStatusList()
        {
            try
            {
                List<LeadStatus> listLeadStatus = new List<LeadStatus>();
                using (connection)
                {
                    if (connection.State == ConnectionState.Closed)
                        CreateConnection();

                    command.CommandText = "Select * From LeadStatus";

                    reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        LeadStatus item = new LeadStatus()
                        {
                            IdLeadStatus = int.Parse(reader["IdLeadStatus"].ToString()),
                            Description = reader["Description"].ToString(),
                            Color = reader["Color"].ToString(),
                            Active = int.Parse(reader["Activated"].ToString()),
                        };
                        listLeadStatus.Add(item);
                    }
                }
                connection.Close();
                SqlConnection.ClearAllPools();
                return listLeadStatus;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void CreateLeadStatus(LeadStatus newLeadStatus)
        {
            try
            {
                CreateConnection();

                using (connection)
                {
                    command.CommandText = "INSERT INTO dbo.LeadStatus (Description, Color, Activated) " +
                        " VALUES(@Description, @Color, @Active)";

                    command.Parameters.Add("@Description", SqlDbType.VarChar).Value = newLeadStatus.Description;
                    command.Parameters.Add("@Color", SqlDbType.VarChar).Value = newLeadStatus.Color;
                    command.Parameters.Add("@Active", SqlDbType.Int).Value = newLeadStatus.Active;
                    
                    reader = command.ExecuteReader();
                }
                connection.Close();
                SqlConnection.ClearAllPools();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void EditLeadStatus(LeadStatus update)
        {
            try
            {
                if (connection.State == ConnectionState.Closed)
                    CreateConnection();
                using (connection)
                {
                    command.CommandText = "UPDATE dbo.LeadStatus SET Description = @Description, Activated = @Active, Color = @Color WHERE IdLeadStatus = @IdLeadStatus";

                    command.Parameters.Add("@IdLeadStatus", SqlDbType.Int).Value = update.IdLeadStatus;
                    command.Parameters.Add("@Description", SqlDbType.VarChar).Value = update.Description;
                    command.Parameters.Add("@Active", SqlDbType.Int).Value = update.Active;
                    command.Parameters.Add("@Color", SqlDbType.VarChar).Value = update.Color;

                    reader = command.ExecuteReader();
                }
                connection.Close();
            }
            catch (Exception e)
            {
                throw e;
            }
            SqlConnection.ClearAllPools();
        }

        /// <summary>
        /// Nationalities///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        public List<Nationalities> GetNationalitiesList()
        {
            try
            {
                List<Nationalities> listNationalities = new List<Nationalities>();
                using (connection)
                {
                    if (connection.State == ConnectionState.Closed)
                        CreateConnection();

                    command.CommandText = "Select * From Nationality";

                    reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Nationalities item = new Nationalities()
                        {
                            IdNationality = int.Parse(reader["IdNationality"].ToString()),
                            NationalityDescription = reader["NationalityDescription"].ToString(),
                            forCandidates = int.Parse(reader["forCandidates"].ToString()),
                            forLeads = int.Parse(reader["forLeads"].ToString()),
                            Active = int.Parse(reader["Active"].ToString()),
                        };
                        listNationalities.Add(item);
                    }
                }
                connection.Close();
                SqlConnection.ClearAllPools();
                return listNationalities;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void CreateNationality(Nationalities newNationality)
        {
            try
            {
                using (connection)
                {
                    command.CommandText = "INSERT INTO dbo.Nationality (NationalityDescription, forCandidates, forLeads, Active) " +
                                            " VALUES(@Description, @forCandidates, @forLeads, @Active)";

                    command.Parameters.Add("@Description", SqlDbType.VarChar).Value = newNationality.NationalityDescription;
                    command.Parameters.Add("@forCandidates", SqlDbType.Int).Value = newNationality.forCandidates;
                    command.Parameters.Add("@forLeads", SqlDbType.Int).Value = newNationality.forLeads;
                    command.Parameters.Add("@Active", SqlDbType.Int).Value = newNationality.Active;

                    reader = command.ExecuteReader();
                }
                connection.Close();
                SqlConnection.ClearAllPools();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void UpdateNationality(Nationalities updateNationality)
        {
            try
            {
                using (connection)
                {
                    command.CommandText = "UPDATE dbo.Nationality SET NationalityDescription = @Description, forCandidates = @forCandidates, forLeads = @forLeads, Active = @Active WHERE IdNationality = @IdNationality";

                    command.Parameters.Add("@IdNationality", SqlDbType.Int).Value = updateNationality.IdNationality;
                    command.Parameters.Add("@Description", SqlDbType.VarChar).Value = updateNationality.NationalityDescription;
                    command.Parameters.Add("@forCandidates", SqlDbType.Int).Value = updateNationality.forCandidates;
                    command.Parameters.Add("@forLeads", SqlDbType.Int).Value = updateNationality.forLeads;
                    command.Parameters.Add("@Active", SqlDbType.Int).Value = updateNationality.Active;

                    reader = command.ExecuteReader();
                }
                connection.Close();
                SqlConnection.ClearAllPools();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// LOGGING/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        public void Logging(string user, int idUser, string Epic,  string Page, string TypeOfCRU, string Before, string After, Exception ex)
        {
            try
            {
                using (connection)
                {
                    this.CreateConnection();
                    command.CommandText = "INSERT INTO dbo.Logging (Date, Hour, Minute, Second, IdUser, Name, Epic, TypeOfCRU, Page, Before, After, Exception) "+
                    " VALUES( @Date, @Hour, @Minute, @Second, @IdUser, @Name, @Epic, @TypeOfCRU, @Page, @Before, @After, @Exception)";

                    command.Parameters.Add("@Date", SqlDbType.VarChar).Value = DateTime.Now.ToString("yyyy/MM/dd");
                    command.Parameters.Add("@Hour", SqlDbType.VarChar).Value = DateTime.Now.ToString("HH");
                    command.Parameters.Add("@Minute", SqlDbType.VarChar).Value = DateTime.Now.ToString("mm");
                    command.Parameters.Add("@Second", SqlDbType.VarChar).Value = DateTime.Now.ToString("ss");
                if(user == null || user == "")
                {
                    command.Parameters.Add("@Name", SqlDbType.VarChar).Value = "Guest";
                    command.Parameters.Add("@IdUser", SqlDbType.Int).Value = 1;
                }
                else { 
                    command.Parameters.Add("@Name", SqlDbType.VarChar).Value = user;
                    command.Parameters.Add("@IdUser", SqlDbType.Int).Value = idUser;
                }
                command.Parameters.Add("@Epic", SqlDbType.VarChar).Value = Epic;
                command.Parameters.Add("@TypeOfCRU", SqlDbType.VarChar).Value = TypeOfCRU;
                command.Parameters.Add("@Page", SqlDbType.VarChar).Value = Page;
                command.Parameters.Add("@Before", SqlDbType.VarChar).Value = Before;
                command.Parameters.Add("@After", SqlDbType.VarChar).Value = After;
                if(ex != null)
                    command.Parameters.Add("@Exception", SqlDbType.VarChar).Value = ex.Message;
                else
                {
                    ex = new Exception("NULL");
                    command.Parameters.Add("@Exception", SqlDbType.VarChar).Value = ex.Message;
                }
                    reader = command.ExecuteReader();
                }
                reader.Close();
                this.command.Dispose();
                this.connection.Dispose();
                SqlConnection.ClearAllPools();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public int GetLastLoggingCode()
        {
            try
            {
                this.CreateConnection();

                    command.CommandText = "SELECT TOP 1 IdLog FROM dbo.Logging ORDER BY IdLog DESC";
                    reader = command.ExecuteReader();
                int i = 0;
                using (connection)
                {
                    
                    while (reader.Read())
                    {
                        i = int.Parse(reader["IdLog"].ToString());
                    }
                }
                connection.Close();
                return i;
            }
            catch (Exception e)
            {
                Logging("Adonis",0, "Logging", "Error", "Read","","", new Exception("Getting Last Logg Failed:-:"+e.Message.ToString()));
                return 0;
            }
        }

        public string GetLoggingPath()
        {
            try
            {
                if (connection.State == ConnectionState.Closed)
                    CreateConnection();

                string path = String.Empty;
                using (connection)
                {
                    command.CommandText = "SELECT LoggingFilesPath FROM dbo.AdonisConfig";
                    reader = command.ExecuteReader();

                    while(reader.Read())
                        path = reader["LoggingFilesPath"].ToString();
                }
                    
                
                connection.Close();
                return path;
            }
            catch (Exception e)
            {
                Logging("Adonis", 0, "Logging", "Error", "Read", "", "", new Exception("Getting Last Logg Failed:-:" + e.Message.ToString()));
                return "";
            }
        }

        //Calculates the Experience of a Candidate TO BE REVIEWED
        private bool ExpCalculator(int Exp, string start, string finish)
        {
            string[] StartParts = start.Split('-');
            DateTime StartDate = new DateTime(int.Parse(StartParts[0]), int.Parse(StartParts[1]), int.Parse(StartParts[2]));

            string[] FinishParts = finish.Split('-');
            DateTime FinishDate = new DateTime(int.Parse(FinishParts[0]), int.Parse(FinishParts[1]), int.Parse(FinishParts[2]));

            DateTime zero = new DateTime(1, 1, 1);
            TimeSpan TimeSpanSubtract = (FinishDate - StartDate);
            double result = (zero + TimeSpanSubtract).Year - 1;

            if (result >= Exp)
                return true;

            return false;
        }

        //CLOSE ALL CONNECTIONS
        public void CloseDBConnections()
        {
            SqlConnection.ClearAllPools();
        }
    }
}