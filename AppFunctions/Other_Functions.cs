/*
 * Developer  : Arun M
 * Created On : 08/12/2013
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ClientDB.DbModel;
using ClientDB.Models;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IO;
using System.Text;
using System.Reflection;
using System.Transactions;
using System.Data.Entity.Validation;
using System.Diagnostics;
using ClientDB.AppFunctions;
using System.Web.Configuration;
using System.Data.Objects.SqlClient;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Net;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ClientDB.Models;
using ClientDB.AppFunctions;
using ClientDB.DbModel;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System.IO;
using System.Net;
using DocumentFormat.OpenXml;


namespace ClientDB.AppFunctions
{
    public class Other_Functions
    {

        BiWeeklyRCPNewEntities dbobj = null;
        public clsSession sess = null;
        GlobalData MetaData = null;
        public clsSession session = null;
        public static string SetLookUpCode = "USA";

        /// <summary>
        /// Function to Get Current Page Name
        /// </summary>
        /// <returns></returns>
        public static string getPageName()
        {
            string PageUrl = System.Web.HttpContext.Current.Request.Url.AbsoluteUri;
            string[] words = PageUrl.Split('/');
            int len = words.Length;
            return words[len - 1].ToString();
        }

        /// <summary>
        /// Function To list States respective to the Country
        /// </summary>
        /// <param name="id"></param>
        /// <param name="flagOverid"></param>
        /// <returns></returns>
        public IEnumerable<SelectListItem> getStateList(int? id = 0, int flagOverid = 0)
        {
            dbobj = new BiWeeklyRCPNewEntities();
            IList<LookUp> Statedata = new List<LookUp>();
            SelectListItem onesele = new SelectListItem { Text = "-- Select --", Value = "" };
            IList<SelectListItem> stateSelecteditem = new List<SelectListItem>();
            stateSelecteditem.Add(onesele);
            try
            {
                Statedata = dbobj.LookUps.Where(x => x.LookupType == "State" && x.ParentLookupId == id).ToList();
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
            }
            var stateSelecteditemsub = (from State in Statedata select new SelectListItem { Text = State.LookupName, Value = State.LookupId.ToString() }).ToList();
            foreach (SelectListItem sele in stateSelecteditemsub)
            {
                stateSelecteditem.Add(sele);
            }
            return stateSelecteditem;
        }
        public IEnumerable<SelectListItem> getCountries()
        {
            BiWeeklyRCPNewEntities RPCobj = new BiWeeklyRCPNewEntities();
            IList<SelectListItem> countriesitems = new List<SelectListItem>();
            SelectListItem onesele = new SelectListItem { Text = "-- Select --", Value = "" };
            countriesitems.Add(onesele);
            var countries = RPCobj.LookUps.Where(x => x.LookupType == "Country").ToList();
            countriesitems = (from country in countries select new SelectListItem { Text = country.LookupName, Value = country.LookupId.ToString() }).ToList();
            return countriesitems;
        }
        private IEnumerable<SelectListItem> getStates(int id)
        {
            IList<SelectListItem> stateItems = new List<SelectListItem>();
            BiWeeklyRCPNewEntities RPCobj = new BiWeeklyRCPNewEntities();
            var states = RPCobj.LookUps.Where(x => x.LookupType == "State" && x.ParentLookupId == id).ToList();
            stateItems = (from state in states select new SelectListItem { Text = state.LookupName, Value = state.LookupId.ToString() }).ToList();
            return stateItems;
        }
        public IEnumerable<SelectListItem> getRaceList()
        {
            dbobj = new BiWeeklyRCPNewEntities();
            IList<LookUp> Racedata = new List<LookUp>();
            SelectListItem onesele = new SelectListItem { Text = "-- Select --", Value = "" };
            IList<SelectListItem> raceSelecteditem = new List<SelectListItem>();
            raceSelecteditem.Add(onesele);
            Racedata = dbobj.LookUps.Where(x => x.LookupType == "Race").ToList();
            var raceSelecteditemsub = (from race in Racedata select new SelectListItem { Text = race.LookupName, Value = race.LookupId.ToString() }).ToList();
            foreach (SelectListItem sele in raceSelecteditemsub)
            {
                raceSelecteditem.Add(sele);
            }
            return raceSelecteditem;
        }

        public List<EmergencyContactPersonalModel> GetEmergencyContactList()
        {
            sess = (clsSession)HttpContext.Current.Session["UserSessionClient"];
            dbobj = new BiWeeklyRCPNewEntities();
            //return dbobj.StudentMoreDetailsNE(sess.SchoolId, sess.StudentId, "ED").ToList();
            var emergencycontactlist = (from objal in dbobj.AddressLists
                                        join objsar in dbobj.StudentAddresRels on objal.AddressId equals objsar.AddressId
                                        join objsp in dbobj.StudentPersonals on objsar.StudentPersonalId equals objsp.StudentPersonalId
                                        join objcp in dbobj.ContactPersonals on objsar.ContactPersonalId equals objcp.ContactPersonalId
                                        join objscr in dbobj.StudentContactRelationships on objcp.ContactPersonalId equals objscr.ContactPersonalId
                                        join objlp in dbobj.LookUps on objscr.RelationshipId equals objlp.LookupId
                                        where objsp.SchoolId == sess.SchoolId && objsp.StudentPersonalId == sess.StudentId && objsar.ContactSequence == 1
                                        //&& objcp.IsEmergency == true && objcp.Status == 1
                                        && (objcp.IsEmergency == true || objcp.IsEmergencyP == true || objcp.IsEmergencyS == true || objcp.IsEmergencyT == true || objcp.IsEmergencyO == true) && objcp.Status == 1
                                        orderby objcp.IsEmergencyP descending, objcp.IsEmergencyS descending, objcp.IsEmergencyT descending, objcp.IsEmergencyO descending

                                        select new EmergencyContactPersonalModel
                                        {
                                            Relation = objlp.LookupName,
                                            Name = objcp.LastName + "," + objcp.FirstName,
                                            Address = ((objal.StreetName == null) ? "" : objal.StreetName + ",") + ((objal.ApartmentType == null) ? "" : objal.ApartmentType + ",") + ((objal.City == null) ? "" : objal.City),
                                            PrimaryLanguage = objcp.PrimaryLanguage,
                                            Phone = objal.Phone,
                                            OtherPhone = objal.OtherPhone,
                                            PrimaryEmail = objal.PrimaryEmail
                                        }).ToList();

            return emergencycontactlist;
        }
        public IEnumerable<SelectListItem> getStateList()
        {
            dbobj = new BiWeeklyRCPNewEntities();
            IList<LookUp> Statedata = new List<LookUp>();
            SelectListItem onesele = new SelectListItem { Text = "-- Select --", Value = "" };
            IList<SelectListItem> stateSelecteditem = new List<SelectListItem>();
            stateSelecteditem.Add(onesele);
            Statedata = dbobj.LookUps.Where(x => x.LookupType == "State").ToList();
            var stateSelecteditemsub = (from state in Statedata select new SelectListItem { Text = state.LookupName, Value = state.LookupId.ToString() }).ToList();
            foreach (SelectListItem sele in stateSelecteditemsub)
            {
                stateSelecteditem.Add(sele);
            }
            return stateSelecteditem;
        }
        public IEnumerable<SelectListItem> getCountryList()
        {
            dbobj = new BiWeeklyRCPNewEntities();
            IList<LookUp> Countrydata = new List<LookUp>();
            SelectListItem onesele = new SelectListItem { Text = "-- Select --", Value = "" };
            IList<SelectListItem> countrySelecteditem = new List<SelectListItem>();
            countrySelecteditem.Add(onesele);
            Countrydata = dbobj.LookUps.Where(x => x.LookupType == "Country").ToList();
            var countrySelecteditemsub = (from country in Countrydata select new SelectListItem { Text = country.LookupName, Value = country.LookupId.ToString() }).ToList();
            foreach (SelectListItem sele in countrySelecteditemsub)
            {
                countrySelecteditem.Add(sele);
            }
            return countrySelecteditem;
        }
        public IEnumerable<SelectListItem> getRelationshipList()
        {
            dbobj = new BiWeeklyRCPNewEntities();
            IList<LookUp> RelationData = new List<LookUp>();
            SelectListItem onesele = new SelectListItem { Text = "-- Select --", Value = "" };
            IList<SelectListItem> relationSelecteditem = new List<SelectListItem>();
            relationSelecteditem.Add(onesele);
            RelationData = dbobj.LookUps.Where(x => x.LookupType == "Relationship").OrderBy(x => x.LookupName).ToList();
            var relationSelecteditemsub = (from relation in RelationData select new SelectListItem { Text = relation.LookupName, Value = relation.LookupId.ToString() }).ToList();
            foreach (SelectListItem sele in relationSelecteditemsub)
            {
                relationSelecteditem.Add(sele);
            }
            return relationSelecteditem;
        }

        public IEnumerable<SelectListItem> getPhysicianList()
        {
            dbobj = new BiWeeklyRCPNewEntities();
            IList<LookUp> PhysicianData = new List<LookUp>();
            SelectListItem onesele = new SelectListItem { Text = "-- Select --", Value = "" };
            IList<SelectListItem> physicianSelecteditem = new List<SelectListItem>();
            physicianSelecteditem.Add(onesele);
            PhysicianData = dbobj.LookUps.Where(x => x.LookupType == "Physician").OrderBy(x => x.LookupName).ToList();
            var physicianSelecteditemsub = (from physician in PhysicianData select new SelectListItem { Text = physician.LookupName, Value = physician.LookupId.ToString() }).ToList();
            foreach (SelectListItem sele in physicianSelecteditemsub)
            {
                physicianSelecteditem.Add(sele);
            }
            return physicianSelecteditem;
        }
        public IEnumerable<SelectListItem> getAddressTypes()
        {
            dbobj = new BiWeeklyRCPNewEntities();
            IList<LookUp> AddressData = new List<LookUp>();
            SelectListItem onesele = new SelectListItem { Text = "-- Select --", Value = "" };
            IList<SelectListItem> addressSelecteditem = new List<SelectListItem>();
            addressSelecteditem.Add(onesele);
            AddressData = dbobj.LookUps.Where(x => x.LookupType == "Address Type").ToList();
            var addressSelecteditemsub = (from address in AddressData select new SelectListItem { Text = address.LookupName, Value = address.LookupId.ToString() }).ToList();
            foreach (SelectListItem sele in addressSelecteditemsub)
            {
                addressSelecteditem.Add(sele);
            }
            return addressSelecteditem;
        }
        public IEnumerable<SelectListItem> getEventStatus()
        {
            dbobj = new BiWeeklyRCPNewEntities();
            IList<LookUp> EventStatusData = new List<LookUp>();
            SelectListItem onesele = new SelectListItem { Text = "-- Select --", Value = "" };
            IList<SelectListItem> eventStatusSelecteditem = new List<SelectListItem>();
            eventStatusSelecteditem.Add(onesele);
            EventStatusData = dbobj.LookUps.Where(objLookUp => objLookUp.LookupType == "Visitation Status").ToList();
            var eventStatusSelecteditemsub = (from eventType in EventStatusData select new SelectListItem { Text = eventType.LookupName, Value = eventType.LookupId.ToString() }).ToList();
            foreach (SelectListItem sele in eventStatusSelecteditemsub)
            {
                eventStatusSelecteditem.Add(sele);
            }
            return eventStatusSelecteditem;
        }
        public IEnumerable<SelectListItem> getEventType()
        {
            dbobj = new BiWeeklyRCPNewEntities();
            IList<LookUp> EventTypeData = new List<LookUp>();
            SelectListItem onesele = new SelectListItem { Text = "-- Select --", Value = "" };
            IList<SelectListItem> eventTypeSelecteditem = new List<SelectListItem>();
            eventTypeSelecteditem.Add(onesele);
            EventTypeData = dbobj.LookUps.Where(objLookUp => objLookUp.LookupType == "Visitation Type").ToList();
            var eventSelecteditemsub = (from eventType in EventTypeData select new SelectListItem { Text = eventType.LookupName, Value = eventType.LookupId.ToString() }).ToList();
            foreach (SelectListItem sele in eventSelecteditemsub)
            {
                eventTypeSelecteditem.Add(sele);
            }
            return eventTypeSelecteditem;
        }

        public IEnumerable<SelectListItem> getDocumentType()
        {
            dbobj = new BiWeeklyRCPNewEntities();
            IList<LookUp> DocumentTypeData = new List<LookUp>();
            SelectListItem onesele = new SelectListItem { Text = "-- Select --", Value = "" };
            IList<SelectListItem> documentTypeSelecteditem = new List<SelectListItem>();
            documentTypeSelecteditem.Add(onesele);
            DocumentTypeData = dbobj.LookUps.Where(objLookUp => objLookUp.LookupType == "Document Type" && objLookUp.LookupName != "ProtocolSummary").ToList();
            var documentSelecteditemsub = (from documentType in DocumentTypeData select new SelectListItem { Text = documentType.LookupName, Value = documentType.LookupId.ToString() }).ToList();
            foreach (SelectListItem sele in documentSelecteditemsub)
            {
                documentTypeSelecteditem.Add(sele);
            }
            return documentTypeSelecteditem;
        }

        public IEnumerable<SelectListItem> getType(string type)
        {
            dbobj = new BiWeeklyRCPNewEntities();
            IList<LookUp> PlacementTypeData = new List<LookUp>();
            SelectListItem onesele = new SelectListItem { Text = "-- Select --", Value = "" };
            IList<SelectListItem> placementTypeSelecteditem = new List<SelectListItem>();
            placementTypeSelecteditem.Add(onesele);
            PlacementTypeData = dbobj.LookUps.Where(objLookUp => objLookUp.LookupType == type && objLookUp.ActiveInd=="A").ToList();
            var placementSelecteditemsub = (from placementType in PlacementTypeData select new SelectListItem { Text = placementType.LookupName, Value = placementType.LookupId.ToString() }).ToList();
            foreach (SelectListItem sele in placementSelecteditemsub)
            {
                placementTypeSelecteditem.Add(sele);
            }
            return placementTypeSelecteditem;
        }

        public IEnumerable<SelectListItem> getUserType(string type)
        {
            dbobj = new BiWeeklyRCPNewEntities();
            IList<User> UserTypeData = new List<User>();
            SelectListItem onesele = new SelectListItem { Text = "-- Select --", Value = "" };
            IList<SelectListItem> placementTypeSelecteditem = new List<SelectListItem>();
            placementTypeSelecteditem.Add(onesele);
            UserTypeData = (from objUser in dbobj.Users
                            join objUserRoleGroup in dbobj.UserRoleGroups on objUser.UserId equals objUserRoleGroup.UserId
                            join objRoleGroup in dbobj.RoleGroups on objUserRoleGroup.RoleGroupId equals objRoleGroup.RoleGroupId
                            join objGroup in dbobj.Groups on objRoleGroup.GroupId equals objGroup.GroupId
                            where objGroup.GroupCode == type && objUser.ActiveInd == "A" && objUserRoleGroup.ActiveInd == "A"
                            select objUser).ToList();

            var placementSelecteditemsub = (from userType in UserTypeData select new SelectListItem { Text = userType.UserLName + " " + userType.UserLName, Value = userType.UserId.ToString() }).ToList();
            foreach (SelectListItem sele in placementSelecteditemsub)
            {
                placementTypeSelecteditem.Add(sele);
            }
            return placementTypeSelecteditem;
        }

        public IEnumerable<SelectListItem> getPhysician()
        {
            dbobj = new BiWeeklyRCPNewEntities();
            IList<LookUp> PhysicianData = new List<LookUp>();
            SelectListItem onesele = new SelectListItem {};
            IList<SelectListItem> physicianSelecteditem = new List<SelectListItem>();
            physicianSelecteditem.Add(onesele);
            PhysicianData = dbobj.LookUps.Where(x => x.LookupType == "Physician").OrderBy(x => x.LookupName).ToList();
            var physicianSelecteditemsub = (from physician in PhysicianData select new SelectListItem { Text = physician.LookupName, Value = physician.LookupId.ToString() }).ToList();
            foreach (SelectListItem sele in physicianSelecteditemsub)
            {
                physicianSelecteditem.Add(sele);
            }
            return physicianSelecteditem;
        }

        public string getCountry(int countreyID)
        {
            dbobj = new BiWeeklyRCPNewEntities();
            string country = dbobj.LookUps.Where(objLookup => objLookup.LookupType == "Country" && objLookup.LookupId == countreyID).Select(objLookup => objLookup.LookupName).Single();
            return country;
        }
        public string getCountryName()
        {
            dbobj = new BiWeeklyRCPNewEntities();
            string country = dbobj.LookUps.Where(objLookup => objLookup.LookupType == "Country" && objLookup.LookupCode == SetLookUpCode).Select(objLookup => objLookup.LookupName).Single();
            return country;
        }
        public string getState(int stateID)
        {
            string state = "";
            try
            {
                dbobj = new BiWeeklyRCPNewEntities();
                state = dbobj.LookUps.Where(objLookup => objLookup.LookupType == "State" && objLookup.LookupId == stateID).Select(objLookup => objLookup.LookupName).Single();
            }
            catch (Exception eX)
            {

            }
            return state;
        }

        /// <summary>
        /// Function To Get Dates to bind medical details calender
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        public string CalanderDatas(int id)
        {
            dbobj = new BiWeeklyRCPNewEntities();
            MedicalAndInsurance tblObjmedical = new MedicalAndInsurance();
            MedicalModel Medicalmodel = new MedicalModel();
            string retunData = "";
            try
            {
                var Medicals = dbobj.MedicalAndInsurances.Where(objMedical => objMedical.StudentPersonalId == id).ToList();
                foreach (var item in Medicals)
                {
                    Medicalmodel.CalenderDatas = item.MedicalInsuranceId + "|" + item.DateOfLastPhysicalExam + "^";
                    if (Medicalmodel.CalenderDatas != null)
                    {
                        retunData += Medicalmodel.CalenderDatas.ToString();
                    }
                }
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
            }


            return retunData;

        }
        public MedicalModel getDiagnosisData(int ClientId)
        {
            sess = (clsSession)HttpContext.Current.Session["UserSessionClient"];
            dbobj = new BiWeeklyRCPNewEntities();
            MedicalModel ReturnMedicalmodel = new MedicalModel();
            // ReturnMedicalmodel.Diagnosis=null;
            try
            {

                var diagonis = dbobj.DiaganosesPAs.Where(objDiagno => objDiagno.StudentPersonalId == ClientId).ToList();
                if (diagonis.Count > 0)
                {
                    foreach (var item in diagonis)
                    {
                        ReturnMedicalmodel.Diagnosis.Add(new Diagnosis
                        {
                          Name = item.Diaganoses
                        });
                       
                    }
                }

            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
            }

            return ReturnMedicalmodel;
        }
        

        public MedicalModel getAllergieData(int ClientId)
        {
            sess = (clsSession)HttpContext.Current.Session["UserSessionClient"];
            dbobj = new BiWeeklyRCPNewEntities();

            MedicalModel ReturnMedicalmodel = new MedicalModel();
            StudentPersonalPA studentPA = new StudentPersonalPA();
            MedicalAndInsurance Medical = new MedicalAndInsurance();
            try
            {

                studentPA = dbobj.StudentPersonalPAs.Where(objStudentPA => objStudentPA.StudentPersonalId == ClientId && objStudentPA.SchoolId == sess.SchoolId).SingleOrDefault();
                if (studentPA != null)
                {
                    ReturnMedicalmodel.Allergie = studentPA.Allergies;
                    ReturnMedicalmodel.Diet = studentPA.Diet;
                    ReturnMedicalmodel.Other = studentPA.Other;
                    ReturnMedicalmodel.Seizures = studentPA.Seizures;
                }
                Medical = dbobj.MedicalAndInsurances.Where(objMedical => objMedical.StudentPersonalId == ClientId && objMedical.SchoolId == sess.SchoolId).FirstOrDefault();
                if(Medical!= null)
                {
                    ReturnMedicalmodel.CurrentMedications = Medical.CurrentMedications;
                    ReturnMedicalmodel.DateOfLastPhysicalExam = ConvertDate(Medical.DateOfLastPhysicalExam);
                    
                }


            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
            }


            return ReturnMedicalmodel;

        }




        /// <summary>
        /// Function to Save / Update Client Information.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="sourceFile"></param>
        /// <returns></returns>

        public string SaveData(RegistrationModel model, HttpPostedFileBase sourceFile)
        {
            sess = (clsSession)HttpContext.Current.Session["UserSessionClient"];

            string Result = "";
            dbobj = new BiWeeklyRCPNewEntities();
            StudentPersonal sp = new StudentPersonal();
            AddressList addr = new AddressList();
            StudentAddresRel adrRel = new StudentAddresRel();
            Insurance objInsurance = new Insurance();
            EmergencyContactSchool objEmergency = new EmergencyContactSchool();
            SchoolsAttended objSchool = new SchoolsAttended();
            MetaData = new GlobalData();
            //  string dirpath = (AppDomain.CurrentDomain.BaseDirectory + "Images/StudentImages/".ToString()).Replace('\\', '/');
            //string dirpath = (System.Web.Configuration.WebConfigurationManager.AppSettings["ImagessLocation"].ToString());
            string dirpath = (AppDomain.CurrentDomain.BaseDirectory + "\\Images\\Client.gif".ToString()).Replace('\\', '/');
            //ImagessLocation
            //  int ClientID = 1;

            try
            {
                using (TransactionScope trans = new TransactionScope(TransactionScopeOption.Required))
                {
                    if (sess.StudentId == 0 && sess.AddressId == 0)
                    {
                        //var LocalId = dbobj.StudentPersonals.Max(objsp => objsp.ClientId).Value;
                        //if (LocalId == 0)
                        //    sp.ClientId = 10000;
                        //else
                        //    sp.ClientId = LocalId + 1;


                        sp.SchoolId = sess.SchoolId;
                        sp.FirstName = model.FirstName;
                        sp.LastName = model.LastName;
                        sp.MiddleName = model.MiddleName;
                        if (model.LastNameSuffix != "0")
                        {
                            sp.Suffix = model.LastNameSuffix;
                        }
                        sp.NickName = model.NickName;
                        sp.BirthDate = DateTime.ParseExact(model.DateOfBirth, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        try
                        {
                            sp.Photodate = DateTime.ParseExact(model.Photodate, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        }
                        catch
                        {
                            sp.Photodate = null;
                        } 
                        if (model.PlaceOfBirth != null)
                        {
                            sp.PlaceOfBirth = model.PlaceOfBirth;
                        }
                        sp.CountryOfBirth = dbobj.LookUps.Where(objlukup => objlukup.LookupType == "Country" && objlukup.LookupCode == SetLookUpCode).Select(objlukup => objlukup.LookupId).Single(); //model.CountryofBirth;
                        if (model.StateOfBirth != null)
                        {
                            sp.StateOfBirth = model.StateOfBirth;
                        }
                        if (model.Citizenship != 0)
                        {
                            sp.CitizenshipStatus = model.Citizenship;
                        }
                        sp.RaceId = model.Race;
                        sp.Gender = model.Gender;
                        if (model.HairColor != null)
                        {
                            sp.HairColor = model.HairColor;
                        }
                        if (model.EyeColor != null)
                        {
                            sp.EyeColor = model.EyeColor;
                        }
                        sp.NewApplication = true;
                        sp.PlacementStatus = "A";
                        try
                        {
                            sp.AdmissionDate = DateTime.ParseExact(model.AdmissinDate, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        }
                        catch
                        {
                            sp.AdmissionDate = null;
                        }
                        if (model.Height != null)
                        {
                            sp.Height = Convert.ToDecimal(model.Height);
                        }
                        if (model.Weight != null)
                        {
                            sp.Weight = Convert.ToDecimal(model.Weight);
                        }
                        if (model.PrimaryLanguage != null)
                        {
                            sp.PrimaryLanguage = model.PrimaryLanguage;
                        }

                        sp.GuardianShip = model.GuardianshipStatus;
                        if (model.DistigushingMarks != null)
                        {
                            sp.DistingushingMarks = model.DistigushingMarks;
                        }
                        sp.LegalCompetencyStatus = model.LegalCompetencyStatus;
                        sp.OtherStateAgenciesInvolvedWithStudent = model.OtherStateAgenciesInvolvedWithStudent;
                        if (model.MaritalStatusofBothParents != null)
                        {
                            sp.MaritalStatusofBothParents = model.MaritalStatusofBothParents;
                        }
                        sp.CaseManagerEducational = model.CaseManagerEducational;
                        sp.CaseManagerResidential = model.CaseManagerResidential;
                        if (sourceFile != null)
                        {
                            sp.ImageUrl = sourceFile.FileName;
                        }
                        else
                        {

                            //string dirpath = WebConfigurationManager.AppSettings["ImagessLocation"].ToString();
                            //dirpath = (AppDomain.CurrentDomain.BaseDirectory + "\\RefferalPhoto\\referal.gif".ToString()).Replace('\\', '/');
                            FileInfo fileInfo = new FileInfo(dirpath);
                            byte[] data = new byte[fileInfo.Length];


                            using (FileStream fs = fileInfo.OpenRead())
                            {
                                fs.Read(data, 0, data.Length);
                                //  sp.ImageUrl = fs.ToString();
                            }
                            sp.ImageUrl = Convert.ToBase64String(data);
                            //int id = objRef.StudentUpldPhoto(StudentPersnlId, data);


                        }
                        sp.ImagePermission = model.PhotoReleasePermission;
                        try
                        {
                            sp.DateInitiallyEligibleforSpecialEducation = DateTime.ParseExact(model.DateInitiallyEligibleforSpecialEducation, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        }
                        catch
                        {
                            sp.DateInitiallyEligibleforSpecialEducation = null;
                        }
                        try
                        {
                            sp.DateofMostRecentSpecialEducationEvaluations = DateTime.ParseExact(model.DateofMostRecentSpecialEducationEvaluations, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        }
                        catch
                        {
                            sp.DateofMostRecentSpecialEducationEvaluations = null;
                        }
                        try
                        {
                            sp.DateofNextScheduled3YearEvaluation = DateTime.ParseExact(model.DateofNextScheduled3YearEvaluation, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        }
                        catch
                        {
                            sp.DateofNextScheduled3YearEvaluation = null;
                        }
                        try
                        {
                            sp.CurrentIEPStartDate = DateTime.ParseExact(model.CurrentIEPStartDate, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        }
                        catch
                        {
                            sp.CurrentIEPStartDate = null;
                        }
                        try
                        {
                            sp.CurrentIEPExpirationDate = DateTime.ParseExact(model.CurrentIEPExpirationDate, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        }
                        catch
                        {
                            sp.CurrentIEPExpirationDate = null;
                        }
                        try
                        {
                            sp.DischargeDate = DateTime.ParseExact(model.DischargeDate, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        }
                        catch
                        {
                            sp.DischargeDate = null;
                        }
                        sp.LocationAfterDischarge = model.LocationAfterDischarge;
                        sp.StudentType = "Client";
                        sp.MelmarkNewEnglandsFollowUpResponsibilities = model.MelmarkNewEnglandsFollowUpResponsibilities;
                        sp.CreatedBy = sess.LoginId;
                        sp.CreatedOn = DateTime.Now;
                        int maxID = 0;
                        try
                        {
                            maxID = dbobj.StudentPersonals.Max(p => p.StudentPersonalId);
                            if (maxID == null || maxID < 0)
                                maxID = 0;
                            maxID++;
                        }
                        catch
                        {
                            maxID = 1;
                        }
                        sp.LocalId = "STD" + maxID;
                        sp.IEPReferralFullName = model.ReferralIEPFullName;
                        sp.IEPReferralPhone = model.ReferralIEPPhone;
                        sp.IEPReferralReferrinAgency = model.ReferralIEPReferringAgency;
                        sp.IEPReferralSourceofTuition = model.ReferralIEPSourceofTuition;
                        sp.IEPReferralTitle = model.ReferralIEPTitle;
                        dbobj.StudentPersonals.Add(sp);
                        dbobj.SaveChanges();
                        sess.StudentId = sp.StudentPersonalId;
                        //
                        sp = dbobj.StudentPersonals.Where(x => x.StudentPersonalId == sess.StudentId && x.SchoolId == sess.SchoolId).SingleOrDefault();
                        var LocalId = dbobj.StudentPersonals.Max(objsp => objsp.ClientId).Value;
                        if (LocalId == 0)
                            sp.ClientId = 50000;
                        else
                            sp.ClientId = LocalId + 1;
                        dbobj.SaveChanges();
                        //ashin
                        if (sourceFile != null)
                        {

                            byte[] fileBytes = new byte[sourceFile.ContentLength];
                            int byteCount = sourceFile.InputStream.Read(fileBytes, 0, (int)sourceFile.ContentLength);

                            sp.ImageUrl = Convert.ToBase64String(fileBytes);
                            // model.ImageUrl = dirpath + "/" + sp.StudentPersonalId + "-" + sourceFile.FileName;



                        }
                        else
                        {

                            FileInfo fileInfo = new FileInfo(dirpath);
                            byte[] data = new byte[fileInfo.Length];
                            //      sp.ImageUrl = Convert.ToBase64String(data);
                            using (FileStream fs = fileInfo.OpenRead())
                            {
                                fs.Read(data, 0, data.Length);
                                //sp.ImageUrl = fs.ToString();
                            }
                            sp.ImageUrl = Convert.ToBase64String(data);


                        }
                        addr.ApartmentType = model.AddressLine1;
                        addr.StreetName = model.AddressLine2;
                        addr.AddressType = MetaData._StudentAddressType;
                        addr.AddressLine3 = model.AddressLine3;
                        addr.City = model.City;
                        addr.County = model.studCounty;
                        addr.CountryId = dbobj.LookUps.Where(objlukup => objlukup.LookupType == "Country" && objlukup.LookupCode == SetLookUpCode).Select(objlukup => objlukup.LookupId).Single(); //model.Country;
                        addr.StateProvince = model.State;
                        addr.PostalCode = model.ZipCode;
                        addr.CreatedBy = sess.LoginId;
                        addr.CreatedOn = DateTime.Now;
                        dbobj.AddressLists.Add(addr);
                        dbobj.SaveChanges();

                        adrRel.AddressId = addr.AddressId;
                        adrRel.StudentPersonalId = sp.StudentPersonalId;
                        adrRel.ContactSequence = 0;
                        adrRel.CreatedBy = sess.LoginId;
                        adrRel.CreatedOn = DateTime.Now;
                        dbobj.StudentAddresRels.Add(adrRel);
                        dbobj.SaveChanges();

                        objEmergency.StudentPersonalId = sp.StudentPersonalId;
                        objEmergency.SchoolId = sess.SchoolId;
                        objEmergency.FirstName = model.EmergencyContactFirstName1;
                        objEmergency.LastName = model.EmergencyContactLastName1;
                        objEmergency.Title = model.EmergencyContactTitle1;
                        objEmergency.Phone = model.EmergencyContactPhone1;
                        objEmergency.CreatedBy = sess.LoginId;
                        objEmergency.CreatedOn = DateTime.Now;
                        objEmergency.SequenceId = 1;
                        dbobj.EmergencyContactSchools.Add(objEmergency);
                        //dbobj.SaveChanges();

                        objEmergency = new EmergencyContactSchool();
                        objEmergency.StudentPersonalId = sp.StudentPersonalId;
                        objEmergency.SchoolId = sess.SchoolId;
                        objEmergency.FirstName = model.EmergencyContactFirstName2;
                        objEmergency.LastName = model.EmergencyContactLastName2;
                        objEmergency.Title = model.EmergencyContactTitle2;
                        objEmergency.Phone = model.EmergencyContactPhone2;
                        objEmergency.CreatedBy = sess.LoginId;
                        objEmergency.CreatedOn = DateTime.Now;
                        objEmergency.SequenceId = 2;
                        dbobj.EmergencyContactSchools.Add(objEmergency);
                        // dbobj.SaveChanges();

                        objEmergency = new EmergencyContactSchool();
                        objEmergency.StudentPersonalId = sp.StudentPersonalId;
                        objEmergency.SchoolId = sess.SchoolId;
                        objEmergency.FirstName = model.EmergencyContactFirstName3;
                        objEmergency.LastName = model.EmergencyContactLastName3;
                        objEmergency.Title = model.EmergencyContactTitle3;
                        objEmergency.Phone = model.EmergencyContactPhone3;
                        objEmergency.CreatedBy = sess.LoginId;
                        objEmergency.CreatedOn = DateTime.Now;
                        objEmergency.SequenceId = 3;
                        dbobj.EmergencyContactSchools.Add(objEmergency);
                        //dbobj.SaveChanges();

                        objEmergency = new EmergencyContactSchool();
                        objEmergency.StudentPersonalId = sp.StudentPersonalId;
                        objEmergency.SchoolId = sess.SchoolId;
                        objEmergency.FirstName = model.EmergencyContactFirstName4;
                        objEmergency.LastName = model.EmergencyContactLastName4;
                        objEmergency.Title = model.EmergencyContactTitle4;
                        objEmergency.Phone = model.EmergencyContactPhone4;
                        objEmergency.CreatedBy = sess.LoginId;
                        objEmergency.CreatedOn = DateTime.Now;
                        objEmergency.SequenceId = 4;
                        dbobj.EmergencyContactSchools.Add(objEmergency);
                        //dbobj.SaveChanges();

                        objEmergency = new EmergencyContactSchool();
                        objEmergency.StudentPersonalId = sp.StudentPersonalId;
                        objEmergency.SchoolId = sess.SchoolId;
                        objEmergency.FirstName = model.EmergencyContactFirstName5;
                        objEmergency.LastName = model.EmergencyContactLastName5;
                        objEmergency.Title = model.EmergencyContactTitle5;
                        objEmergency.Phone = model.EmergencyContactPhone5;
                        objEmergency.CreatedBy = sess.LoginId;
                        objEmergency.CreatedOn = DateTime.Now;
                        objEmergency.SequenceId = 5;
                        dbobj.EmergencyContactSchools.Add(objEmergency);
                        dbobj.SaveChanges();

                        objSchool.StudentPersonalId = sp.StudentPersonalId;
                        objSchool.SchoolId = sess.SchoolId;
                        objSchool.SchoolName = model.SchoolName1;
                        try
                        {
                            objSchool.DateFrom = DateTime.ParseExact(model.DateFrom1, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        }
                        catch
                        {
                            objSchool.DateFrom = null;
                        }
                        try
                        {
                            objSchool.DateTo = DateTime.ParseExact(model.DateTo1, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        }
                        catch
                        {
                            objSchool.DateTo = null;
                        }
                        objSchool.Address1 = model.SchoolAttendedAddress11;
                        objSchool.Address2 = model.SchoolAttendedAddress21;
                        objSchool.City = model.SchoolAttendedCity1;
                        // objSchool.State = model.SchoolAttendedState1;
                        objSchool.State = model.intSchoolAttendedState1.ToString();
                        objSchool.CreatedBy = sess.LoginId;
                        objSchool.CreatedOn = DateTime.Now;
                        objSchool.SequenceId = 1;
                        dbobj.SchoolsAttendeds.Add(objSchool);
                        dbobj.SaveChanges();

                        objSchool = new SchoolsAttended();
                        objSchool.StudentPersonalId = sp.StudentPersonalId;
                        objSchool.SchoolId = sess.SchoolId;
                        objSchool.SchoolName = model.SchoolName2;
                        try
                        {
                            objSchool.DateFrom = DateTime.ParseExact(model.DateFrom2, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        }
                        catch
                        {
                            objSchool.DateFrom = null;
                        }
                        try
                        {
                            objSchool.DateTo = DateTime.ParseExact(model.DateTo2, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        }
                        catch
                        {
                            objSchool.DateTo = null;
                        }
                        objSchool.Address1 = model.SchoolAttendedAddress12;
                        objSchool.Address2 = model.SchoolAttendedAddress22;
                        objSchool.City = model.SchoolAttendedCity2;
                        // objSchool.State = model.SchoolAttendedState2;
                        objSchool.State = model.intSchoolAttendedState2.ToString();
                        objSchool.CreatedBy = sess.LoginId;
                        objSchool.CreatedOn = DateTime.Now;
                        objSchool.SequenceId = 2;
                        dbobj.SchoolsAttendeds.Add(objSchool);
                        dbobj.SaveChanges();

                        objSchool = new SchoolsAttended();
                        objSchool.StudentPersonalId = sp.StudentPersonalId;
                        objSchool.SchoolId = sess.SchoolId;
                        objSchool.SchoolName = model.SchoolName3;
                        try
                        {
                            objSchool.DateFrom = DateTime.ParseExact(model.DateFrom3, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        }
                        catch
                        {
                            objSchool.DateFrom = null;
                        }
                        try
                        {
                            objSchool.DateTo = DateTime.ParseExact(model.DateTo3, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        }
                        catch
                        {
                            objSchool.DateTo = null;
                        }
                        objSchool.Address1 = model.SchoolAttendedAddress13;
                        objSchool.Address2 = model.SchoolAttendedAddress23;
                        objSchool.City = model.SchoolAttendedCity3;
                        // objSchool.State = model.SchoolAttendedState3;
                        objSchool.State = model.intSchoolAttendedState3.ToString();
                        objSchool.CreatedBy = sess.LoginId;
                        objSchool.CreatedOn = DateTime.Now;
                        objSchool.SequenceId = 3;
                        dbobj.SchoolsAttendeds.Add(objSchool);
                        dbobj.SaveChanges();

                        objInsurance.StudentPersonalId = sp.StudentPersonalId;
                        objInsurance.InsuranceType = model.InsuranceType;
                        objInsurance.SchoolId = sess.SchoolId;
                        objInsurance.PolicyHolder = model.PolicyHolder;
                        objInsurance.PolicyNumber = model.PolicyNumber;
                        objInsurance.PreferType = "Primary";
                        objInsurance.CreatedBy = sess.LoginId;
                        objInsurance.CreatedOn = DateTime.Now;
                        dbobj.Insurances.Add(objInsurance);
                        dbobj.SaveChanges();
                        Result = "Sucess";
                    }
                    else
                    {
                        sp = dbobj.StudentPersonals.Where(x => x.StudentPersonalId == sess.StudentId).SingleOrDefault();
                        adrRel = dbobj.StudentAddresRels.Where(objAddressRel => objAddressRel.StudentPersonalId == sess.StudentId && objAddressRel.ContactSequence == 0).SingleOrDefault();
                        sp.LocalId = "STD1002";
                        sp.FirstName = model.FirstName;
                        sp.LastName = model.LastName;
                        sp.MiddleName = model.MiddleName;
                        if (model.LastNameSuffix != null)
                        {
                            sp.Suffix = model.LastNameSuffix;
                        }
                        sp.NickName = model.NickName;
                        sp.BirthDate = DateTime.ParseExact(model.DateOfBirth, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        try
                        {
                            sp.Photodate = DateTime.ParseExact(model.Photodate, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        }
                        catch
                        {
                            sp.Photodate = null;
                        } 
                        if (model.PlaceOfBirth != null)
                        {
                            sp.PlaceOfBirth = model.PlaceOfBirth;
                        }
                        if (model.CountryofBirth != null)
                        {
                            sp.CountryOfBirth = model.CountryofBirth;
                        }
                        if (model.StateOfBirth != null)
                        {
                            sp.StateOfBirth = model.StateOfBirth;
                        }
                        if (model.Citizenship != null)
                        {
                            sp.CitizenshipStatus = model.Citizenship;
                        }
                        sp.RaceId = model.Race;
                        sp.Gender = model.Gender;
                        if (model.HairColor != null)
                        {
                            sp.HairColor = model.HairColor;
                        }
                        if (model.EyeColor != null)
                        {
                            sp.EyeColor = model.EyeColor;
                        }
                        try
                        {
                            sp.AdmissionDate = DateTime.ParseExact(model.AdmissinDate, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        }
                        catch
                        {
                            sp.AdmissionDate = null;
                        }
                        if (model.Height != null)
                        {
                            sp.Height = Convert.ToDecimal(model.Height);
                        }
                        if (model.Weight != null)
                        {
                            sp.Weight = Convert.ToDecimal(model.Weight);
                        }
                        if (model.PrimaryLanguage != null)
                        {
                            sp.PrimaryLanguage = model.PrimaryLanguage;
                        }
                        if (model.DistigushingMarks != null)
                        {
                            sp.DistingushingMarks = model.DistigushingMarks;
                        }
                        if (model.LegalCompetencyStatus != null)
                        {
                            sp.LegalCompetencyStatus = model.LegalCompetencyStatus;
                        }
                        if (model.OtherStateAgenciesInvolvedWithStudent != null)
                        {
                            sp.OtherStateAgenciesInvolvedWithStudent = model.OtherStateAgenciesInvolvedWithStudent;
                        }
                        if (model.MaritalStatusofBothParents != null)
                        {
                            sp.MaritalStatusofBothParents = model.MaritalStatusofBothParents;
                        }
                        if (model.CaseManagerEducational != null)
                        {
                            sp.CaseManagerEducational = model.CaseManagerEducational;
                        }
                        if (model.CaseManagerResidential != null)
                        {
                            sp.CaseManagerResidential = model.CaseManagerResidential;
                        }
                        if (sourceFile != null)
                        {
                            byte[] fileBytes = new byte[sourceFile.ContentLength];
                            int byteCount = sourceFile.InputStream.Read(fileBytes, 0, (int)sourceFile.ContentLength);

                            sp.ImageUrl = Convert.ToBase64String(fileBytes);
                            //sp.ImageUrl = sourceFile.FileName;
                        }

                        sp.ImagePermission = model.PhotoReleasePermission;
                        if (model.GuardianshipStatus != null)
                        {
                            sp.GuardianShip = model.GuardianshipStatus;
                        }
                        //sp.DateInitiallyEligibleforSpecialEducation = DateTime.ParseExact(model.DateInitiallyEligibleforSpecialEducation, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        //sp.DateofMostRecentSpecialEducationEvaluations = DateTime.ParseExact(model.DateofMostRecentSpecialEducationEvaluations, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        //sp.DateofNextScheduled3YearEvaluation = DateTime.ParseExact(model.DateofNextScheduled3YearEvaluation, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        //sp.CurrentIEPStartDate = DateTime.ParseExact(model.CurrentIEPStartDate, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        //sp.CurrentIEPExpirationDate = DateTime.ParseExact(model.CurrentIEPExpirationDate, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        //sp.DischargeDate = DateTime.ParseExact(model.DischargeDate, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        //sp.LocationAfterDischarge = model.LocationAfterDischarge;
                        try
                        {
                            sp.DateInitiallyEligibleforSpecialEducation = DateTime.ParseExact(model.DateInitiallyEligibleforSpecialEducation, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        }
                        catch
                        {
                            sp.DateInitiallyEligibleforSpecialEducation = null;
                        }
                        try
                        {
                            sp.DateofMostRecentSpecialEducationEvaluations = DateTime.ParseExact(model.DateofMostRecentSpecialEducationEvaluations, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        }
                        catch
                        {
                            sp.DateofMostRecentSpecialEducationEvaluations = null;
                        }
                        try
                        {
                            sp.DateofNextScheduled3YearEvaluation = DateTime.ParseExact(model.DateofNextScheduled3YearEvaluation, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        }
                        catch
                        {
                            sp.DateofNextScheduled3YearEvaluation = null;
                        }
                        try
                        {
                            sp.CurrentIEPStartDate = DateTime.ParseExact(model.CurrentIEPStartDate, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        }
                        catch
                        {
                            sp.CurrentIEPStartDate = null;
                        }
                        try
                        {
                            sp.CurrentIEPExpirationDate = DateTime.ParseExact(model.CurrentIEPExpirationDate, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        }
                        catch
                        {
                            sp.CurrentIEPExpirationDate = null;
                        }
                        try
                        {
                            sp.DischargeDate = DateTime.ParseExact(model.DischargeDate, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        }
                        catch
                        {
                            sp.DischargeDate = null;
                        }
                        sp.LocationAfterDischarge = model.LocationAfterDischarge;
                        sp.MelmarkNewEnglandsFollowUpResponsibilities = model.MelmarkNewEnglandsFollowUpResponsibilities;
                        sp.IEPReferralFullName = model.ReferralIEPFullName;
                        sp.IEPReferralPhone = model.ReferralIEPPhone;
                        sp.IEPReferralReferrinAgency = model.ReferralIEPReferringAgency;
                        sp.IEPReferralSourceofTuition = model.ReferralIEPSourceofTuition;
                        sp.IEPReferralTitle = model.ReferralIEPTitle;
                        sp.ModifiedBy = sess.LoginId;
                        sp.ModifiedOn = DateTime.Now;
                        dbobj.SaveChanges();
                        if (sourceFile != null)
                        {
                            model.ImageUrl = sp.ImageUrl;

                        }

                        addr = dbobj.AddressLists.Where(x => x.AddressId == adrRel.AddressId).SingleOrDefault();
                        addr.ApartmentType = model.AddressLine1;
                        addr.StreetName = model.AddressLine2;
                        addr.AddressLine3 = model.AddressLine3;
                        addr.AddressType = MetaData._StudentAddressType;
                        addr.City = model.City;
                        addr.County = model.studCounty;
                        addr.CountryId = model.Country;
                        addr.StateProvince = model.State;
                        addr.PostalCode = model.ZipCode;
                        addr.ModifiedBy = sess.LoginId;
                        addr.ModifiedOn = DateTime.Now;
                        dbobj.SaveChanges();
                        var EmergencyContacts = dbobj.EmergencyContactSchools.Where(objEmergencyContact => objEmergencyContact.StudentPersonalId == sess.StudentId).ToList();
                        if (EmergencyContacts.Count > 0)
                        {
                            foreach (var item in EmergencyContacts)
                            {
                                if (item.SequenceId == 1)
                                {
                                    objEmergency = dbobj.EmergencyContactSchools.Where(objEmergencys => objEmergencys.StudentPersonalId == sess.StudentId &&
                                        objEmergencys.SequenceId == 1).SingleOrDefault();
                                    objEmergency.FirstName = model.EmergencyContactFirstName1;
                                    objEmergency.LastName = model.EmergencyContactLastName1;
                                    objEmergency.Title = model.EmergencyContactTitle1;
                                    objEmergency.Phone = model.EmergencyContactPhone1;
                                    objEmergency.ModifiedBy = sess.LoginId;
                                    objEmergency.ModifiedOn = DateTime.Now;
                                    dbobj.SaveChanges();
                                }
                                if (item.SequenceId == 2)
                                {

                                    objEmergency = dbobj.EmergencyContactSchools.Where(objEmergencys => objEmergencys.StudentPersonalId == sess.StudentId &&
                                        objEmergencys.SequenceId == 2).SingleOrDefault();
                                    objEmergency.FirstName = model.EmergencyContactFirstName2;
                                    objEmergency.LastName = model.EmergencyContactLastName2;
                                    objEmergency.Title = model.EmergencyContactTitle2;
                                    objEmergency.Phone = model.EmergencyContactPhone2;
                                    objEmergency.ModifiedBy = sess.LoginId;
                                    objEmergency.ModifiedOn = DateTime.Now;
                                    dbobj.SaveChanges();
                                }
                                if (item.SequenceId == 3)
                                {

                                    objEmergency = dbobj.EmergencyContactSchools.Where(objEmergencys => objEmergencys.StudentPersonalId == sess.StudentId &&
                                        objEmergencys.SequenceId == 3).SingleOrDefault();
                                    objEmergency.FirstName = model.EmergencyContactFirstName3;
                                    objEmergency.LastName = model.EmergencyContactLastName3;
                                    objEmergency.Title = model.EmergencyContactTitle3;
                                    objEmergency.Phone = model.EmergencyContactPhone3;
                                    objEmergency.ModifiedBy = sess.LoginId;
                                    objEmergency.ModifiedOn = DateTime.Now;
                                    dbobj.SaveChanges();
                                }
                                if (item.SequenceId == 4)
                                {

                                    objEmergency = dbobj.EmergencyContactSchools.Where(objEmergencys => objEmergencys.StudentPersonalId == sess.StudentId &&
                                        objEmergencys.SequenceId == 4).SingleOrDefault();
                                    objEmergency.FirstName = model.EmergencyContactFirstName4;
                                    objEmergency.LastName = model.EmergencyContactLastName4;
                                    objEmergency.Title = model.EmergencyContactTitle4;
                                    objEmergency.Phone = model.EmergencyContactPhone4;
                                    objEmergency.ModifiedBy = sess.LoginId;
                                    objEmergency.ModifiedOn = DateTime.Now;
                                    dbobj.SaveChanges();
                                }
                                if (item.SequenceId == 5)
                                {

                                    objEmergency = dbobj.EmergencyContactSchools.Where(objEmergencys => objEmergencys.StudentPersonalId == sess.StudentId &&
                                        objEmergencys.SequenceId == 5).SingleOrDefault();
                                    objEmergency.FirstName = model.EmergencyContactFirstName5;
                                    objEmergency.LastName = model.EmergencyContactLastName5;
                                    objEmergency.Title = model.EmergencyContactTitle5;
                                    objEmergency.Phone = model.EmergencyContactPhone5;
                                    objEmergency.ModifiedBy = sess.LoginId;
                                    objEmergency.ModifiedOn = DateTime.Now;
                                    dbobj.SaveChanges();
                                }
                            }
                        }
                        else
                        {
                            objEmergency.StudentPersonalId = sess.StudentId;
                            objEmergency.SchoolId = sess.SchoolId;
                            objEmergency.FirstName = model.EmergencyContactFirstName1;
                            objEmergency.LastName = model.EmergencyContactLastName1;
                            objEmergency.Title = model.EmergencyContactTitle1;
                            objEmergency.Phone = model.EmergencyContactPhone1;
                            objEmergency.CreatedBy = sess.LoginId;
                            objEmergency.CreatedOn = DateTime.Now;
                            objEmergency.SequenceId = 1;
                            dbobj.EmergencyContactSchools.Add(objEmergency);
                            // dbobj.SaveChanges();

                            objEmergency = new EmergencyContactSchool();
                            objEmergency.StudentPersonalId = sess.StudentId;
                            objEmergency.SchoolId = sess.SchoolId;
                            objEmergency.FirstName = model.EmergencyContactFirstName2;
                            objEmergency.LastName = model.EmergencyContactLastName2;
                            objEmergency.Title = model.EmergencyContactTitle2;
                            objEmergency.Phone = model.EmergencyContactPhone2;
                            objEmergency.CreatedBy = sess.LoginId;
                            objEmergency.CreatedOn = DateTime.Now;
                            objEmergency.SequenceId = 2;
                            dbobj.EmergencyContactSchools.Add(objEmergency);
                            // dbobj.SaveChanges();

                            objEmergency = new EmergencyContactSchool();
                            objEmergency.StudentPersonalId = sess.StudentId;
                            objEmergency.SchoolId = sess.SchoolId;
                            objEmergency.FirstName = model.EmergencyContactFirstName3;
                            objEmergency.LastName = model.EmergencyContactLastName3;
                            objEmergency.Title = model.EmergencyContactTitle3;
                            objEmergency.Phone = model.EmergencyContactPhone3;
                            objEmergency.CreatedBy = sess.LoginId;
                            objEmergency.CreatedOn = DateTime.Now;
                            objEmergency.SequenceId = 3;
                            dbobj.EmergencyContactSchools.Add(objEmergency);
                            //   dbobj.SaveChanges();

                            objEmergency = new EmergencyContactSchool();
                            objEmergency.StudentPersonalId = sess.StudentId;
                            objEmergency.SchoolId = sess.SchoolId;
                            objEmergency.FirstName = model.EmergencyContactFirstName4;
                            objEmergency.LastName = model.EmergencyContactLastName4;
                            objEmergency.Title = model.EmergencyContactTitle4;
                            objEmergency.Phone = model.EmergencyContactPhone4;
                            objEmergency.CreatedBy = sess.LoginId;
                            objEmergency.CreatedOn = DateTime.Now;
                            objEmergency.SequenceId = 4;
                            dbobj.EmergencyContactSchools.Add(objEmergency);
                            //      dbobj.SaveChanges();

                            objEmergency = new EmergencyContactSchool();
                            objEmergency.StudentPersonalId = sess.StudentId;
                            objEmergency.SchoolId = sess.SchoolId;
                            objEmergency.FirstName = model.EmergencyContactFirstName5;
                            objEmergency.LastName = model.EmergencyContactLastName5;
                            objEmergency.Title = model.EmergencyContactTitle5;
                            objEmergency.Phone = model.EmergencyContactPhone5;
                            objEmergency.CreatedBy = sess.LoginId;
                            objEmergency.CreatedOn = DateTime.Now;
                            objEmergency.SequenceId = 5;
                            dbobj.EmergencyContactSchools.Add(objEmergency);
                            dbobj.SaveChanges();
                        }

                        var SchoolsAttended = dbobj.SchoolsAttendeds.Where(objschools => objschools.StudentPersonalId == sess.StudentId).ToList();
                        if (SchoolsAttended.Count > 0)
                        {
                            foreach (var item in SchoolsAttended)
                            {
                                if (item.SequenceId == 1)
                                {
                                    objSchool = dbobj.SchoolsAttendeds.Where(objSchul => objSchul.StudentPersonalId == sess.StudentId && objSchul.SequenceId == 1).SingleOrDefault();
                                    objSchool.SchoolName = model.SchoolName1;
                                    try
                                    {
                                        objSchool.DateFrom = DateTime.ParseExact(model.DateFrom1, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                    }
                                    catch
                                    {
                                        objSchool.DateFrom = null;
                                    }
                                    try
                                    {
                                        objSchool.DateTo = DateTime.ParseExact(model.DateTo1, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                    }
                                    catch
                                    {
                                        objSchool.DateTo = null;
                                    }
                                    objSchool.Address1 = model.SchoolAttendedAddress11;
                                    objSchool.Address2 = model.SchoolAttendedAddress21;
                                    objSchool.City = model.SchoolAttendedCity1;
                                    // objSchool.State = model.SchoolAttendedState1;
                                    objSchool.State = model.intSchoolAttendedState1.ToString();
                                    objSchool.ModifiedBy = sess.LoginId;
                                    objSchool.ModifiedOn = DateTime.Now;
                                    dbobj.SaveChanges();
                                }
                                if (item.SequenceId == 2)
                                {
                                    objSchool = dbobj.SchoolsAttendeds.Where(objSchul => objSchul.StudentPersonalId == sess.StudentId && objSchul.SequenceId == 2).SingleOrDefault();
                                    objSchool.SchoolName = model.SchoolName2;
                                    try
                                    {
                                        objSchool.DateFrom = DateTime.ParseExact(model.DateFrom2, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                    }
                                    catch
                                    {
                                        objSchool.DateFrom = null;
                                    }
                                    try
                                    {
                                        objSchool.DateTo = DateTime.ParseExact(model.DateTo2, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                    }
                                    catch
                                    {
                                        objSchool.DateTo = null;
                                    }
                                    objSchool.Address1 = model.SchoolAttendedAddress12;
                                    objSchool.Address2 = model.SchoolAttendedAddress22;
                                    objSchool.City = model.SchoolAttendedCity2;
                                    // objSchool.State = model.SchoolAttendedState2;
                                    objSchool.State = model.intSchoolAttendedState2.ToString();
                                    objSchool.ModifiedBy = sess.LoginId;
                                    objSchool.ModifiedOn = DateTime.Now;
                                    dbobj.SaveChanges();
                                }
                                if (item.SequenceId == 3)
                                {
                                    objSchool = dbobj.SchoolsAttendeds.Where(objSchul => objSchul.StudentPersonalId == sess.StudentId && objSchul.SequenceId == 3).SingleOrDefault();
                                    objSchool.SchoolName = model.SchoolName3;
                                    try
                                    {
                                        objSchool.DateFrom = DateTime.ParseExact(model.DateFrom3, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                    }
                                    catch
                                    {
                                        objSchool.DateFrom = null;
                                    }
                                    try
                                    {
                                        objSchool.DateTo = DateTime.ParseExact(model.DateTo3, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                    }
                                    catch
                                    {
                                        objSchool.DateTo = null;
                                    }
                                    objSchool.Address1 = model.SchoolAttendedAddress13;
                                    objSchool.Address2 = model.SchoolAttendedAddress23;
                                    objSchool.City = model.SchoolAttendedCity3;
                                    //objSchool.State = model.SchoolAttendedState3;
                                    objSchool.State = model.intSchoolAttendedState3.ToString();
                                    objSchool.ModifiedBy = sess.LoginId;
                                    objSchool.ModifiedOn = DateTime.Now;
                                    dbobj.SaveChanges();
                                }
                            }
                        }
                        else
                        {
                            objSchool.StudentPersonalId = sess.StudentId;
                            objSchool.SchoolId = sess.SchoolId;
                            objSchool.SchoolName = model.SchoolName1;
                            try
                            {
                                objSchool.DateFrom = DateTime.ParseExact(model.DateFrom1, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                            }
                            catch
                            {
                                objSchool.DateFrom = null;
                            }
                            try
                            {
                                objSchool.DateTo = DateTime.ParseExact(model.DateTo1, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                            }
                            catch
                            {
                                objSchool.DateTo = null;
                            }
                            objSchool.Address1 = model.SchoolAttendedAddress11;
                            objSchool.Address2 = model.SchoolAttendedAddress21;
                            objSchool.City = model.SchoolAttendedCity1;
                            //objSchool.State = model.SchoolAttendedState1;
                            objSchool.State = model.intSchoolAttendedState1.ToString();
                            objSchool.CreatedBy = sess.LoginId;
                            objSchool.CreatedOn = DateTime.Now;
                            objSchool.SequenceId = 1;
                            dbobj.SchoolsAttendeds.Add(objSchool);
                            dbobj.SaveChanges();

                            objSchool = new SchoolsAttended();
                            objSchool.StudentPersonalId = sess.StudentId;
                            objSchool.SchoolId = sess.SchoolId;
                            objSchool.SchoolName = model.SchoolName2;
                            try
                            {
                                objSchool.DateFrom = DateTime.ParseExact(model.DateFrom2, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                            }
                            catch
                            {
                                objSchool.DateFrom = null;
                            }
                            try
                            {
                                objSchool.DateTo = DateTime.ParseExact(model.DateTo2, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                            }
                            catch
                            {
                                objSchool.DateTo = null;
                            }
                            objSchool.Address1 = model.SchoolAttendedAddress12;
                            objSchool.Address2 = model.SchoolAttendedAddress22;
                            objSchool.City = model.SchoolAttendedCity2;
                            //objSchool.State = model.SchoolAttendedState2;
                            objSchool.State = model.intSchoolAttendedState2.ToString();
                            objSchool.CreatedBy = sess.LoginId;
                            objSchool.CreatedOn = DateTime.Now;
                            objSchool.SequenceId = 2;
                            dbobj.SchoolsAttendeds.Add(objSchool);
                            dbobj.SaveChanges();

                            objSchool = new SchoolsAttended();
                            objSchool.StudentPersonalId = sess.StudentId;
                            objSchool.SchoolId = sess.SchoolId;
                            objSchool.SchoolName = model.SchoolName3;
                            try
                            {
                                objSchool.DateFrom = DateTime.ParseExact(model.DateFrom3, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                            }
                            catch
                            {
                                objSchool.DateFrom = null;
                            }
                            try
                            {
                                objSchool.DateTo = DateTime.ParseExact(model.DateTo3, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                            }
                            catch
                            {
                                objSchool.DateTo = null;
                            }
                            objSchool.Address1 = model.SchoolAttendedAddress13;
                            objSchool.Address2 = model.SchoolAttendedAddress23;
                            objSchool.City = model.SchoolAttendedCity3;
                            //objSchool.State = model.SchoolAttendedState3;
                            objSchool.State = model.intSchoolAttendedState3.ToString();
                            objSchool.CreatedBy = sess.LoginId;
                            objSchool.CreatedOn = DateTime.Now;
                            objSchool.SequenceId = 3;
                            dbobj.SchoolsAttendeds.Add(objSchool);
                            dbobj.SaveChanges();

                        }
                        try
                        {
                            objInsurance = dbobj.Insurances.Where(objInsu => objInsu.StudentPersonalId == sess.StudentId && objInsu.PreferType == "Primary").SingleOrDefault();
                            if (objInsurance != null)
                            {
                                objInsurance.InsuranceType = model.InsuranceType;
                                objInsurance.PolicyHolder = model.PolicyHolder;
                                objInsurance.PolicyNumber = model.PolicyNumber;
                                objInsurance.PreferType = "Primary";
                                objInsurance.ModifiedBy = sess.LoginId;
                                objInsurance.ModifiedOn = DateTime.Now;
                                dbobj.SaveChanges();
                            }
                            else
                            {
                                objInsurance = new Insurance();
                                objInsurance.StudentPersonalId = sess.StudentId;
                                objInsurance.SchoolId = sess.SchoolId;
                                objInsurance.InsuranceType = model.InsuranceType;
                                objInsurance.PolicyHolder = model.PolicyHolder;
                                objInsurance.PolicyNumber = model.PolicyNumber;
                                objInsurance.PreferType = "Primary";
                                objInsurance.CreatedBy = sess.LoginId;
                                objInsurance.CreatedOn = DateTime.Now;
                                dbobj.Insurances.Add(objInsurance);
                                dbobj.SaveChanges();
                            }
                        }
                        catch { }
                        //dbobj.SaveChanges();

                        Result = "Sucess";
                    }
                    Result = "Sucess";
                    trans.Complete();
                }
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
                Result = "Failed";
            }
            return Result;
        }


        /// <summary>
        /// Function to Save / Update PA Client Information.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="sourceFile"></param>
        /// <returns></returns>

        public string SaveData(ClientRegistrationPAModel model, HttpPostedFileBase sourceFile)
        {
            sess = (clsSession)HttpContext.Current.Session["UserSessionClient"];

            string Result = "";
            dbobj = new BiWeeklyRCPNewEntities();
            StudentPersonal sp = new StudentPersonal();
            AddressList addr = new AddressList();
            StudentAddresRel adrRel = new StudentAddresRel();
            StudentPersonalPA studentPA = new StudentPersonalPA();
            DiaganosesPA diagnose = new DiaganosesPA();
            BasicBehavioralInformation basicBehavior = new BasicBehavioralInformation();
            AdaptiveEquipment adaptive = new DbModel.AdaptiveEquipment();
            BehavioursPA behavior = new BehavioursPA();
            Insurance objInsurance = new Insurance();
            EmergencyContactSchool objEmergency = new EmergencyContactSchool();
            SchoolsAttended objSchool = new SchoolsAttended();
            MetaData = new GlobalData();
            // string dirpath = (AppDomain.CurrentDomain.BaseDirectory + "Images/StudentImages/".ToString()).Replace('\\', '/');
            string dirpath = (AppDomain.CurrentDomain.BaseDirectory + "\\Images\\Client.gif".ToString()).Replace('\\', '/');

            //  int StudentId = 1;

            try
            {
                using (TransactionScope trans = new TransactionScope(TransactionScopeOption.Required))
                {
                    if (sess.StudentId == 0 && sess.AddressId == 0)
                    {
                        //var LocalId = dbobj.StudentPersonals.Max(objsp => objsp.ClientId).Value;
                        //if (LocalId == 0)
                        //    sp.ClientId = 50000;
                        //else
                        //    sp.ClientId = LocalId + 1;

                        sp.LocalId = "STD1002";
                        sp.SchoolId = sess.SchoolId;
                        sp.Prefix = model.Prefix;
                        sp.FirstName = model.FirstName;
                        sp.LastName = model.LastName;
                        sp.MiddleName = model.MiddleName;
                        if (model.LastNameSuffix != "0")
                        {
                            sp.Suffix = model.LastNameSuffix;
                        }
                        sp.NickName = model.NickName;
                        sp.BirthDate = DateTime.ParseExact(model.DateOfBirth, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        try
                        {
                            sp.Photodate = DateTime.ParseExact(model.Photodate, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        }
                        catch
                        {
                            sp.Photodate = null;
                        }
                        if (model.PlaceOfBirth != null)
                        {
                            sp.PlaceOfBirth = model.PlaceOfBirth;
                        }
                        if (model.CountryofBirth != null)
                        {
                            sp.CountryOfBirth = model.CountryofBirth;
                        }
                        if (model.StateOfBirth != null)
                        {
                            sp.StateOfBirth = model.StateOfBirth;
                        }
                        if (model.Citizenship != null)
                        {
                            sp.CitizenshipStatus = model.Citizenship;
                        }
                        sp.RaceId = model.Race;
                        sp.Gender = model.Gender;
                        if (model.HairColor != null)
                        {
                            sp.HairColor = model.HairColor;
                        }
                        if (model.EyeColor != null)
                        {
                            sp.EyeColor = model.EyeColor;
                        }
                        if (model.MostRecentGradeLevel != null)
                        {
                            sp.MostRecentGradeLevel = model.MostRecentGradeLevel;
                        }
                        sp.NewApplication = true;
                        sp.Ambulatory = model.Ambulatory;
                        sp.EnglishProficiency = model.EnglishProficiency;
                        sp.SASID = model.SASID;
                        sp.Medicaid = model.Medicaid;
                        sp.Notes = model.Note;
                        sp.Intensive = model.Intensive;
                        sp.IsGuardian = model.IsGuardian;

                        if (model.DayProgarm != null)
                        {
                            sp.DayProgarm = model.DayProgarm;
                        }
                        if (model.ClassroomWorkshop != null)
                        {
                            sp.ClassroomWorkshop = model.ClassroomWorkshop;
                        }
                        sp.TeacherInstructor = model.TeacherInstructor;
                        sp.ProgramSpecialist = model.ProgramSpecialist;
                        sp.EDUBehaviorAnalyst = model.EDUBehaviorAnalyst;
                        sp.CurriculumCoordinator = model.CurriculumCoordinator;
                        sp.ResidentialProgram = model.ResidentialProgram;
                        if (model.House != null)
                        {
                            sp.House = model.House;
                        }
                        sp.ProgramManagerQMRP = model.ProgramManagerQMRP;
                        sp.HouseSupervisor = model.HouseSupervisor;
                        sp.ResidentialBehaviorAnalyst = model.ResidentialBehaviorAnalyst;
                        sp.PrimaryNurse = model.PrimaryNurse;
                        sp.UnitClerk = model.UnitClerk;
                        //============================== List 6 - Task #2
                        ////sp.TeacherInstructor = "P" + model.StaffPosition1.ToString() + ',' + "S" + model.Position1Staff.ToString(); // List 6 - task #2 - 1-Feb-2021
                        ////sp.ProgramSpecialist = "P" + model.StaffPosition2.ToString() + ',' + "S" + model.Position2Staff.ToString(); // List 6 - task #2 - 1-Feb-2021
                        ////sp.EDUBehaviorAnalyst = "P" + model.StaffPosition3.ToString() + ',' + "S" + model.Position3Staff.ToString(); // List 6 - task #2 - 1-Feb-2021
                        ////sp.CurriculumCoordinator = "P" + model.StaffPosition4.ToString() + ',' + "S" + model.Position4Staff.ToString(); // List 6 - task #2 - 1-Feb-2021
                        ////sp.ResidentialProgram = "P" + model.StaffPosition5.ToString() + ',' + "S" + model.Position5Staff.ToString();  // List 6 - task #2 - 1-Feb-2021                      
                        ////sp.ProgramManagerQMRP = "P" + model.StaffPosition6.ToString() + ',' + "S" + model.Position6Staff.ToString(); // List 6 - task #2 - 1-Feb-2021
                        ////sp.HouseSupervisor = "P" + model.StaffPosition7.ToString() + ',' + "S" + model.Position7Staff.ToString(); // List 6 - task #2 - 1-Feb-2021
                        ////sp.ResidentialBehaviorAnalyst = "P" + model.StaffPosition8.ToString() + ',' + "S" + model.Position8Staff.ToString(); // List 6 - task #2 - 1-Feb-2021
                        ////sp.PrimaryNurse = "P" + model.StaffPosition9.ToString() + ',' + "S" + model.Position9Staff.ToString(); // List 6 - task #2 - 1-Feb-2021
                        ////sp.UnitClerk = "P" + model.StaffPosition10.ToString() + ',' + "S" + model.Position10Staff.ToString();  // List 6 - task #2 - 1-Feb-2021
                        sp.PositionStaff1 = "P" + model.StaffPosition1.ToString() + ',' + "S" + model.Position1Staff.ToString(); // List 6 - task #2 - 1-Feb-2021
                        sp.PositionStaff2 = "P" + model.StaffPosition2.ToString() + ',' + "S" + model.Position2Staff.ToString(); // List 6 - task #2 - 1-Feb-2021
                        sp.PositionStaff3 = "P" + model.StaffPosition3.ToString() + ',' + "S" + model.Position3Staff.ToString(); // List 6 - task #2 - 1-Feb-2021
                        sp.PositionStaff4 = "P" + model.StaffPosition4.ToString() + ',' + "S" + model.Position4Staff.ToString(); // List 6 - task #2 - 1-Feb-2021
                        sp.PositionStaff5 = "P" + model.StaffPosition5.ToString() + ',' + "S" + model.Position5Staff.ToString();  // List 6 - task #2 - 1-Feb-2021 
                        sp.PositionStaff6 = "P" + model.StaffPosition6.ToString() + ',' + "S" + model.Position6Staff.ToString(); // List 6 - task #2 - 1-Feb-2021
                        sp.PositionStaff7 = "P" + model.StaffPosition7.ToString() + ',' + "S" + model.Position7Staff.ToString(); // List 6 - task #2 - 1-Feb-2021
                        sp.PositionStaff8 = "P" + model.StaffPosition8.ToString() + ',' + "S" + model.Position8Staff.ToString(); // List 6 - task #2 - 1-Feb-2021
                        sp.PositionStaff9 = "P" + model.StaffPosition9.ToString() + ',' + "S" + model.Position9Staff.ToString(); // List 6 - task #2 - 1-Feb-2021
                        sp.PositionStaff10 = "P" + model.StaffPosition10.ToString() + ',' + "S" + model.Position10Staff.ToString();  // List 6 - task #2 - 1-Feb-2021
                        //==============================List 6 - Task #2
                        sp.PhotoPermComment = model.PhotoPermComment;
                        sp.PhotoRelease = model.PhotoRelease;
                        sp.TripRestriction1 = model.TripRestriction1;
                        sp.TripRestriction2 = model.TripRestriction2;
                        sp.TripComments = model.TripComments;
                        sp.ClientInfoComments = model.ClientInfoComments;

                        if (model.Classification1 == null)
                        {
                            model.Classification1 = 0;
                        }
                        if (model.Classification2 == null)
                        {
                            model.Classification2 = 0;
                        }
                        if (model.Classification3 == null)
                        {
                            model.Classification3 = 0;
                        }
                        if (model.Classification4 == null)
                        {
                            model.Classification4 = 0;
                        }
                        if (model.Classification5 == null)
                        {
                            model.Classification5 = 0;
                        }
                        sp.Classification1 = model.Classification1;
                        sp.Classification2 = model.Classification2;
                        sp.Classification3 = model.Classification3;
                        sp.Classification4 = model.Classification4;
                        sp.Classification5 = model.Classification5;

                        try
                        {
                            sp.AdmissionDate = DateTime.ParseExact(model.DateUpdated, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        }
                        catch
                        {
                            sp.AdmissionDate = null;
                        }
                        if (model.Height != null)
                        {
                            sp.Height = Convert.ToDecimal(model.Height);
                        }
                        if (model.Weight != null)
                        {
                            sp.Weight = Convert.ToDecimal(model.Weight);
                        }
                        if (model.PrimaryLanguage != null)
                        {
                            sp.PrimaryLanguage = model.PrimaryLanguage;
                        }
                        if (model.GuardianshipStatus != null)
                        {
                            sp.GuardianShip = model.GuardianshipStatus;
                        }
                        if (model.DistigushingMarks != null)
                        {
                            sp.DistingushingMarks = model.DistigushingMarks;
                        }
                        if (model.LegalCompetencyStatus != null)
                        {
                            sp.LegalCompetencyStatus = model.LegalCompetencyStatus;
                        }
                        if (model.OtherStateAgenciesInvolvedWithStudent != null)
                        {
                            sp.OtherStateAgenciesInvolvedWithStudent = model.OtherStateAgenciesInvolvedWithStudent;
                        }
                        if (model.MaritalStatusofBothParents != null)
                        {
                            sp.MaritalStatusofBothParents = model.MaritalStatusofBothParents;
                        }
                        if (model.CaseManagerEducational != null)
                        {
                            sp.CaseManagerEducational = model.CaseManagerEducational;
                        }
                        if (model.CaseManagerResidential != null)
                        {
                            sp.CaseManagerResidential = model.CaseManagerResidential;
                        }
                        //if (sourceFile != null)
                        //{
                        //    sp.ImageUrl = sourceFile.FileName;
                        //}

                        if (sourceFile != null)
                        {

                            byte[] fileBytes = new byte[sourceFile.ContentLength];
                            int byteCount = sourceFile.InputStream.Read(fileBytes, 0, (int)sourceFile.ContentLength);

                            sp.ImageUrl = Convert.ToBase64String(fileBytes);
                            // model.ImageUrl = dirpath + "/" + sp.StudentPersonalId + "-" + sourceFile.FileName;


                        }
                        else
                        {
                            //dirpath = System.Web.Configuration.WebConfigurationManager.AppSettings["ImagessLocation"].ToString();
                            //dirpath = (AppDomain.CurrentDomain.BaseDirectory + dirpath);
                            FileInfo fileInfo = new FileInfo(dirpath);
                            byte[] data = new byte[fileInfo.Length];


                            using (FileStream fs = fileInfo.OpenRead())
                            {
                                fs.Read(data, 0, data.Length);
                                //  sp.ImageUrl = fs.ToString();
                            }
                            sp.ImageUrl = Convert.ToBase64String(data);
                            //int id = objRef.StudentUpldPhoto(StudentPersnlId, data);

                        }

                        sp.ImagePermission = model.PhotoReleasePermission;

                        sp.IEPReferralFullName = model.ReferralIEPFullName;
                        sp.IEPReferralPhone = model.ReferralIEPPhone;
                        sp.IEPReferralReferrinAgency = model.ReferralIEPReferringAgency;
                        sp.IEPReferralSourceofTuition = model.ReferralIEPSourceofTuition;
                        sp.IEPReferralTitle = model.ReferralIEPTitle;

                        try
                        {
                            sp.DateInitiallyEligibleforSpecialEducation = DateTime.ParseExact(model.DateInitiallyEligibleforSpecialEducation, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        }
                        catch
                        {
                            sp.DateInitiallyEligibleforSpecialEducation = null;
                        }
                        try
                        {
                            sp.DateofMostRecentSpecialEducationEvaluations = DateTime.ParseExact(model.DateofMostRecentSpecialEducationEvaluations, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        }
                        catch
                        {
                            sp.DateofMostRecentSpecialEducationEvaluations = null;
                        }
                        try
                        {
                            sp.DateofNextScheduled3YearEvaluation = DateTime.ParseExact(model.DateofNextScheduled3YearEvaluation, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        }
                        catch
                        {
                            sp.DateofNextScheduled3YearEvaluation = null;
                        }
                        try
                        {
                            sp.CurrentIEPStartDate = DateTime.ParseExact(model.CurrentIEPStartDate, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        }
                        catch
                        {
                            sp.CurrentIEPStartDate = null;
                        }
                        try
                        {
                            sp.CurrentIEPExpirationDate = DateTime.ParseExact(model.CurrentIEPExpirationDate, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        }
                        catch
                        {
                            sp.CurrentIEPExpirationDate = null;
                        }
                        try
                        {
                            sp.DischargeDate = DateTime.ParseExact(model.DischargeDate, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        }
                        catch
                        {
                            sp.DischargeDate = null;
                        }
                        sp.LocationAfterDischarge = model.LocationAfterDischarge;
                        sp.StudentType = "Client";
                        sp.MelmarkNewEnglandsFollowUpResponsibilities = model.MelmarkNewEnglandsFollowUpResponsibilities;

                        sp.StudentType = "Client";
                        sp.PlacementStatus = "A";
                        sp.CreatedBy = 1;
                        sp.CreatedOn = DateTime.Now;
                        dbobj.StudentPersonals.Add(sp);
                        dbobj.SaveChanges();
                        sess.StudentId = sp.StudentPersonalId;

                        objEmergency.StudentPersonalId = sp.StudentPersonalId;
                        objEmergency.SchoolId = sess.SchoolId;
                        objEmergency.FirstName = model.EmergencyContactFirstName1;
                        objEmergency.LastName = model.EmergencyContactLastName1;
                        objEmergency.Title = model.EmergencyContactTitle1;
                        objEmergency.Phone = model.EmergencyContactPhone1;
                        objEmergency.CreatedBy = sess.LoginId;
                        objEmergency.CreatedOn = DateTime.Now;
                        objEmergency.SequenceId = 1;
                        dbobj.EmergencyContactSchools.Add(objEmergency);
                        //dbobj.SaveChanges();

                        objEmergency = new EmergencyContactSchool();
                        objEmergency.StudentPersonalId = sp.StudentPersonalId;
                        objEmergency.SchoolId = sess.SchoolId;
                        objEmergency.FirstName = model.EmergencyContactFirstName2;
                        objEmergency.LastName = model.EmergencyContactLastName2;
                        objEmergency.Title = model.EmergencyContactTitle2;
                        objEmergency.Phone = model.EmergencyContactPhone2;
                        objEmergency.CreatedBy = sess.LoginId;
                        objEmergency.CreatedOn = DateTime.Now;
                        objEmergency.SequenceId = 2;
                        dbobj.EmergencyContactSchools.Add(objEmergency);
                        // dbobj.SaveChanges();

                        objEmergency = new EmergencyContactSchool();
                        objEmergency.StudentPersonalId = sp.StudentPersonalId;
                        objEmergency.SchoolId = sess.SchoolId;
                        objEmergency.FirstName = model.EmergencyContactFirstName3;
                        objEmergency.LastName = model.EmergencyContactLastName3;
                        objEmergency.Title = model.EmergencyContactTitle3;
                        objEmergency.Phone = model.EmergencyContactPhone3;
                        objEmergency.CreatedBy = sess.LoginId;
                        objEmergency.CreatedOn = DateTime.Now;
                        objEmergency.SequenceId = 3;
                        dbobj.EmergencyContactSchools.Add(objEmergency);
                        //dbobj.SaveChanges();

                        objEmergency = new EmergencyContactSchool();
                        objEmergency.StudentPersonalId = sp.StudentPersonalId;
                        objEmergency.SchoolId = sess.SchoolId;
                        objEmergency.FirstName = model.EmergencyContactFirstName4;
                        objEmergency.LastName = model.EmergencyContactLastName4;
                        objEmergency.Title = model.EmergencyContactTitle4;
                        objEmergency.Phone = model.EmergencyContactPhone4;
                        objEmergency.CreatedBy = sess.LoginId;
                        objEmergency.CreatedOn = DateTime.Now;
                        objEmergency.SequenceId = 4;
                        dbobj.EmergencyContactSchools.Add(objEmergency);
                        //dbobj.SaveChanges();

                        objEmergency = new EmergencyContactSchool();
                        objEmergency.StudentPersonalId = sp.StudentPersonalId;
                        objEmergency.SchoolId = sess.SchoolId;
                        objEmergency.FirstName = model.EmergencyContactFirstName5;
                        objEmergency.LastName = model.EmergencyContactLastName5;
                        objEmergency.Title = model.EmergencyContactTitle5;
                        objEmergency.Phone = model.EmergencyContactPhone5;
                        objEmergency.CreatedBy = sess.LoginId;
                        objEmergency.CreatedOn = DateTime.Now;
                        objEmergency.SequenceId = 5;
                        dbobj.EmergencyContactSchools.Add(objEmergency);
                        dbobj.SaveChanges();

                        objSchool.StudentPersonalId = sess.StudentId;
                        objSchool.SchoolId = sess.SchoolId;
                        objSchool.SchoolName = model.SchoolName1;
                        try
                        {
                            objSchool.DateFrom = DateTime.ParseExact(model.DateFrom1, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        }
                        catch
                        {
                            objSchool.DateFrom = null;
                        }
                        try
                        {
                            objSchool.DateTo = DateTime.ParseExact(model.DateTo1, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        }
                        catch
                        {
                            objSchool.DateTo = null;
                        }
                        objSchool.Address1 = model.SchoolAttendedAddress11;
                        objSchool.Address2 = model.SchoolAttendedAddress21;
                        objSchool.City = model.SchoolAttendedCity1;
                        //objSchool.State = model.SchoolAttendedState1;
                        objSchool.State = model.intSchoolAttendedState1.ToString();
                        objSchool.CreatedBy = sess.LoginId;
                        objSchool.CreatedOn = DateTime.Now;
                        objSchool.SequenceId = 1;
                        dbobj.SchoolsAttendeds.Add(objSchool);
                        dbobj.SaveChanges();

                        objSchool = new SchoolsAttended();
                        objSchool.StudentPersonalId = sess.StudentId;
                        objSchool.SchoolId = sess.SchoolId;
                        objSchool.SchoolName = model.SchoolName2;
                        try
                        {
                            objSchool.DateFrom = DateTime.ParseExact(model.DateFrom2, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        }
                        catch
                        {
                            objSchool.DateFrom = null;
                        }
                        try
                        {
                            objSchool.DateTo = DateTime.ParseExact(model.DateTo2, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        }
                        catch
                        {
                            objSchool.DateTo = null;
                        }
                        objSchool.Address1 = model.SchoolAttendedAddress12;
                        objSchool.Address2 = model.SchoolAttendedAddress22;
                        objSchool.City = model.SchoolAttendedCity2;
                        //objSchool.State = model.SchoolAttendedState2;
                        objSchool.State = model.intSchoolAttendedState2.ToString();
                        objSchool.CreatedBy = sess.LoginId;
                        objSchool.CreatedOn = DateTime.Now;
                        objSchool.SequenceId = 2;
                        dbobj.SchoolsAttendeds.Add(objSchool);
                        dbobj.SaveChanges();

                        objSchool = new SchoolsAttended();
                        objSchool.StudentPersonalId = sess.StudentId;
                        objSchool.SchoolId = sess.SchoolId;
                        objSchool.SchoolName = model.SchoolName3;
                        try
                        {
                            objSchool.DateFrom = DateTime.ParseExact(model.DateFrom3, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        }
                        catch
                        {
                            objSchool.DateFrom = null;
                        }
                        try
                        {
                            objSchool.DateTo = DateTime.ParseExact(model.DateTo3, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        }
                        catch
                        {
                            objSchool.DateTo = null;
                        }
                        objSchool.Address1 = model.SchoolAttendedAddress13;
                        objSchool.Address2 = model.SchoolAttendedAddress23;
                        objSchool.City = model.SchoolAttendedCity3;
                        //objSchool.State = model.SchoolAttendedState3;
                        objSchool.State = model.intSchoolAttendedState3.ToString();
                        objSchool.CreatedBy = sess.LoginId;
                        objSchool.CreatedOn = DateTime.Now;
                        objSchool.SequenceId = 3;
                        dbobj.SchoolsAttendeds.Add(objSchool);
                        dbobj.SaveChanges();

                        objInsurance = new Insurance();
                        objInsurance.StudentPersonalId = sess.StudentId;
                        objInsurance.SchoolId = sess.SchoolId;
                        objInsurance.InsuranceType = model.InsuranceType;
                        objInsurance.PolicyHolder = model.PolicyHolder;
                        objInsurance.PolicyNumber = model.PolicyNumber;
                        objInsurance.PreferType = "Primary";
                        objInsurance.CreatedBy = sess.LoginId;
                        objInsurance.CreatedOn = DateTime.Now;
                        dbobj.Insurances.Add(objInsurance);
                        dbobj.SaveChanges();

                        objInsurance = new Insurance();
                        objInsurance.StudentPersonalId = sess.StudentId;
                        objInsurance.SchoolId = sess.SchoolId;
                        objInsurance.InsuranceType = model.InsuranceType1;
                        objInsurance.PolicyHolder = model.PolicyHolder1;
                        objInsurance.PolicyNumber = model.PolicyNumber1;
                        objInsurance.PreferType = "Secondary";
                        objInsurance.CreatedBy = sess.LoginId;
                        objInsurance.CreatedOn = DateTime.Now;
                        dbobj.Insurances.Add(objInsurance);
                        dbobj.SaveChanges();

                        sess.StudentId = sp.StudentPersonalId;
                        //
                        sp = dbobj.StudentPersonals.Where(x => x.StudentPersonalId == sess.StudentId && x.SchoolId == sess.SchoolId).SingleOrDefault();
                        var LocalId = dbobj.StudentPersonals.Max(objsp => objsp.ClientId).Value;
                        if (LocalId == 0)
                            sp.ClientId = 50000;
                        else
                            sp.ClientId = LocalId + 1;
                        dbobj.SaveChanges();
                        //ashin
                        addr.ApartmentType = model.AddressLine1;
                        addr.StreetName = model.AddressLine2;
                        addr.AddressType = MetaData._StudentAddressType;
                        addr.AddressLine3 = model.AddressLine3;
                        addr.Phone = model.PhoneNumber;
                        addr.City = model.City;
                        addr.CountryId = model.Country;
                        addr.StateProvince = model.State;
                        addr.PostalCode = model.ZipCode;
                        addr.ClientAddressPhone = model.ClientAddressPhone;
                        addr.CreatedBy = sess.LoginId;
                        addr.CreatedOn = DateTime.Now;
                        dbobj.AddressLists.Add(addr);
                        dbobj.SaveChanges();

                        adrRel.AddressId = addr.AddressId;
                        adrRel.StudentPersonalId = sp.StudentPersonalId;
                        adrRel.ContactSequence = 0;
                        adrRel.CreatedBy = sess.LoginId;
                        adrRel.CreatedOn = DateTime.Now;
                        dbobj.StudentAddresRels.Add(adrRel);
                        dbobj.SaveChanges();

                        studentPA.StudentPersonalId = sp.StudentPersonalId;
                        studentPA.SchoolId = sess.SchoolId;
                       // studentPA.Allergies = model.Allergie;
                        studentPA.Bathroom = model.Bathroom;
                       // studentPA.Diet = model.Diet;
                        studentPA.dy_TaskOrBreak = model.TaskORBreak;
                        studentPA.dy_TransitionInside = model.TransitionInside;
                        studentPA.dy_TransitionUnevenGround = model.TransitionUnevenGround;
                        studentPA.Consciousness = model.Consciousness;
                        studentPA.CommonAreas = model.CommonAreas;
                        studentPA.RiskOfResistance = model.RiskOfResistance;
                        studentPA.Mobility = model.Mobility;
                        studentPA.NeedForExtraHelp = model.NeedForExtraHelp;
                        studentPA.ResponseToInstruction = model.ResponseToInstruction;
                        studentPA.WalkingResponses = model.WalkingResponse;
                        studentPA.ho_BedroomAsleep = model.BedroomAsleep;
                        studentPA.ho_BedroomAwake = model.BedroomAwake;
                        studentPA.ho_CommonAres = model.CommonAreas;
                        studentPA.OnCampus = model.OnCampus;
                        studentPA.WhenTranspoting = model.WhenTranspoting;
                        studentPA.OffCampus = model.OffCampus;
                        studentPA.PoolOrSwimming = model.PoolOrSwimming;
                        studentPA.Van = model.van;
                       // studentPA.Seizures = model.Seizures;
                       // studentPA.Other = model.Other;
                        studentPA.FundingSource = model.Funding;
                        studentPA.CreatedBy = sess.LoginId;
                        studentPA.CreatedOn = DateTime.Now;
                        dbobj.StudentPersonalPAs.Add(studentPA);
                        dbobj.SaveChanges();
                        //for (int i = 0; i < model.Diagnosis.Count; i++)
                        //{
                        //    diagnose.StudentPersonalId = sp.StudentPersonalId;
                        //    diagnose.SchoolId = sess.SchoolId;
                        //    diagnose.Diaganoses = model.Diagnosis[i].Name;
                        //    diagnose.CreatedBy = sess.LoginId;
                        //    diagnose.CreatedOn = DateTime.Now;
                        //    dbobj.DiaganosesPAs.Add(diagnose);
                        //    dbobj.SaveChanges();
                        //}
                        for (int i = 0; i < model.Adapt.Count; i++)
                        {
                            adaptive.StudentPersonalId = sp.StudentPersonalId;
                            adaptive.SchoolId = sess.SchoolId;
                            adaptive.Item = model.Adapt[i].item;
                            adaptive.ScheduleForUse = model.Adapt[i].ScheduledForUss;
                            adaptive.StorageLocation = model.Adapt[i].StorageLocation;
                            adaptive.CleaningInstruction = model.Adapt[i].CleaningInstruction;
                            adaptive.CreatedBy = sess.LoginId;
                            adaptive.CreatedOn = DateTime.Now;
                            dbobj.AdaptiveEquipments.Add(adaptive);
                            dbobj.SaveChanges();
                        }
                        for (int i = 0; i < model.BasicBehave.Count; i++)
                        {
                            basicBehavior.StudentPersonalId = sp.StudentPersonalId;
                            basicBehavior.SchoolId = sess.SchoolId;
                            basicBehavior.TargetBehavior = model.BasicBehave[i].TargetBehavior;
                            basicBehavior.Definition = model.BasicBehave[i].Definition;
                            basicBehavior.Antecedent = model.BasicBehave[i].Antecedent;
                            basicBehavior.FCT = model.BasicBehave[i].FCT;
                            basicBehavior.Consequence = model.BasicBehave[i].Consequances;
                            basicBehavior.CreatedBy = sess.LoginId;
                            basicBehavior.CreatedOn = DateTime.Now;
                            dbobj.BasicBehavioralInformations.Add(basicBehavior);
                            dbobj.SaveChanges();
                        }



                        saveBehaviors(behavior, sess, model);

                        #region behave save
                        // model.Dressing1;

                        // diagnose.

                        //int parentId = behavId("LIFTING / TRANSFERS");
                        //behavior.StudentPersonalId = sp.StudentPersonalId;
                        //behavior.SchoolId = sess.SchoolId;
                        //behavior.ParentId = parentId;
                        //behavior.BehaviourName = model.LiftingOrTransfers1;
                        //behavior.CreatedBy = sess.LoginId;
                        //behavior.CreatedOn = DateTime.Now;
                        //dbobj.BehavioursPAs.Add(behavior);
                        //dbobj.SaveChanges();
                        //behavior.StudentPersonalId = sp.StudentPersonalId;
                        //behavior.ParentId = parentId;
                        //behavior.SchoolId = sess.SchoolId;
                        //behavior.BehaviourName = model.LiftingOrTransfers2;
                        //behavior.CreatedBy = sess.LoginId;
                        //behavior.CreatedOn = DateTime.Now;
                        //dbobj.BehavioursPAs.Add(behavior);
                        //dbobj.SaveChanges();

                        //parentId = behavId("AMBULATION");
                        //behavior.StudentPersonalId = sp.StudentPersonalId;
                        //behavior.ParentId = parentId;
                        //behavior.SchoolId = sess.SchoolId;
                        //behavior.BehaviourName = model.Ambulation1;
                        //behavior.CreatedBy = sess.LoginId;
                        //behavior.CreatedOn = DateTime.Now;
                        //dbobj.BehavioursPAs.Add(behavior);
                        //dbobj.SaveChanges();
                        //behavior.StudentPersonalId = sp.StudentPersonalId;
                        //behavior.ParentId = parentId;
                        //behavior.SchoolId = sess.SchoolId;
                        //behavior.BehaviourName = model.Ambulation2;
                        //behavior.CreatedBy = sess.LoginId;
                        //behavior.CreatedOn = DateTime.Now;
                        //dbobj.BehavioursPAs.Add(behavior);
                        //dbobj.SaveChanges();

                        //parentId = behavId("TOILETING");
                        //behavior.StudentPersonalId = sp.StudentPersonalId;
                        //behavior.ParentId = parentId;
                        //behavior.SchoolId = sess.SchoolId;
                        //behavior.BehaviourName = model.Toileting1;
                        //behavior.CreatedBy = sess.LoginId;
                        //behavior.CreatedOn = DateTime.Now;
                        //dbobj.BehavioursPAs.Add(behavior);
                        //dbobj.SaveChanges();
                        //behavior.StudentPersonalId = sp.StudentPersonalId;
                        //behavior.ParentId = parentId;
                        //behavior.SchoolId = sess.SchoolId;
                        //behavior.BehaviourName = model.Toileting2;
                        //behavior.CreatedBy = sess.LoginId;
                        //behavior.CreatedOn = DateTime.Now;
                        //dbobj.BehavioursPAs.Add(behavior);
                        //dbobj.SaveChanges();

                        //parentId = behavId("EATING");
                        //behavior.StudentPersonalId = sp.StudentPersonalId;
                        //behavior.ParentId = parentId;
                        //behavior.SchoolId = sess.SchoolId;
                        //behavior.BehaviourName = model.Eating1;
                        //behavior.CreatedBy = sess.LoginId;
                        //behavior.CreatedOn = DateTime.Now;
                        //dbobj.BehavioursPAs.Add(behavior);
                        //dbobj.SaveChanges();
                        //behavior.StudentPersonalId = sp.StudentPersonalId;
                        //behavior.ParentId = parentId;
                        //behavior.SchoolId = sess.SchoolId;
                        //behavior.BehaviourName = model.Eating2;
                        //behavior.CreatedBy = sess.LoginId;
                        //behavior.CreatedOn = DateTime.Now;
                        //dbobj.BehavioursPAs.Add(behavior);
                        //dbobj.SaveChanges();

                        //parentId = behavId("SHOWERING");
                        //behavior.StudentPersonalId = sp.StudentPersonalId;
                        //behavior.ParentId = parentId;
                        //behavior.SchoolId = sess.SchoolId;
                        //behavior.BehaviourName = model.Showering1;
                        //behavior.CreatedBy = sess.LoginId;
                        //behavior.CreatedOn = DateTime.Now;
                        //dbobj.BehavioursPAs.Add(behavior);
                        //dbobj.SaveChanges();
                        //behavior.StudentPersonalId = sp.StudentPersonalId;
                        //behavior.ParentId = parentId;
                        //behavior.SchoolId = sess.SchoolId;
                        //behavior.BehaviourName = model.Showering2;
                        //behavior.CreatedBy = sess.LoginId;
                        //behavior.CreatedOn = DateTime.Now;
                        //dbobj.BehavioursPAs.Add(behavior);
                        //dbobj.SaveChanges();

                        //parentId = behavId("TOOTHBRUSHING");
                        //behavior.StudentPersonalId = sp.StudentPersonalId;
                        //behavior.ParentId = parentId;
                        //behavior.SchoolId = sess.SchoolId;
                        //behavior.BehaviourName = model.ToothBrushing1;
                        //behavior.CreatedBy = sess.LoginId;
                        //behavior.CreatedOn = DateTime.Now;
                        //dbobj.BehavioursPAs.Add(behavior);
                        //dbobj.SaveChanges();
                        //behavior.StudentPersonalId = sp.StudentPersonalId;
                        //behavior.ParentId = parentId;
                        //behavior.SchoolId = sess.SchoolId;
                        //behavior.BehaviourName = model.ToothBrushing2;
                        //behavior.CreatedBy = sess.LoginId;
                        //behavior.CreatedOn = DateTime.Now;
                        //dbobj.BehavioursPAs.Add(behavior);
                        //dbobj.SaveChanges();

                        //parentId = behavId("DRESSING");
                        //behavior.StudentPersonalId = sp.StudentPersonalId;
                        //behavior.ParentId = parentId;
                        //behavior.SchoolId = sess.SchoolId;
                        //behavior.BehaviourName = model.Dressing1;
                        //behavior.CreatedBy = sess.LoginId;
                        //behavior.CreatedOn = DateTime.Now;
                        //dbobj.BehavioursPAs.Add(behavior);
                        //dbobj.SaveChanges();
                        //behavior.StudentPersonalId = sp.StudentPersonalId;
                        //behavior.ParentId = parentId;
                        //behavior.SchoolId = sess.SchoolId;
                        //behavior.BehaviourName = model.Dressing2;
                        //behavior.CreatedBy = sess.LoginId;
                        //behavior.CreatedOn = DateTime.Now;
                        //dbobj.BehavioursPAs.Add(behavior);
                        //dbobj.SaveChanges();

                        //parentId = behavId("SKIN CARE/SKIN INTEGRITY");
                        //behavior.StudentPersonalId = sp.StudentPersonalId;
                        //behavior.ParentId = parentId;
                        //behavior.SchoolId = sess.SchoolId;
                        //behavior.BehaviourName = model.SkinCare1;
                        //behavior.CreatedBy = sess.LoginId;
                        //behavior.CreatedOn = DateTime.Now;
                        //dbobj.BehavioursPAs.Add(behavior);
                        //dbobj.SaveChanges();
                        //behavior.StudentPersonalId = sp.StudentPersonalId;
                        //behavior.ParentId = parentId;
                        //behavior.SchoolId = sess.SchoolId;
                        //behavior.BehaviourName = model.SkinCare2;
                        //behavior.CreatedBy = sess.LoginId;
                        //behavior.CreatedOn = DateTime.Now;
                        //dbobj.BehavioursPAs.Add(behavior);
                        //dbobj.SaveChanges();

                        //parentId = behavId("COMMUNICATION");
                        //behavior.StudentPersonalId = sp.StudentPersonalId;
                        //behavior.ParentId = parentId;
                        //behavior.SchoolId = sess.SchoolId;
                        //behavior.BehaviourName = model.Communication1;
                        //behavior.CreatedBy = sess.LoginId;
                        //behavior.CreatedOn = DateTime.Now;
                        //dbobj.BehavioursPAs.Add(behavior);
                        //dbobj.SaveChanges();
                        //behavior.StudentPersonalId = sp.StudentPersonalId;
                        //behavior.ParentId = parentId;
                        //behavior.SchoolId = sess.SchoolId;
                        //behavior.BehaviourName = model.Communication2;
                        //behavior.CreatedBy = sess.LoginId;
                        //behavior.CreatedOn = DateTime.Now;
                        //dbobj.BehavioursPAs.Add(behavior);
                        //dbobj.SaveChanges();


                        //parentId = behavId("PREFERRED ACTIVITIES");
                        //behavior.StudentPersonalId = sp.StudentPersonalId;
                        //behavior.ParentId = parentId;
                        //behavior.SchoolId = sess.SchoolId;
                        //behavior.BehaviourName = model.preferedActivities1;
                        //behavior.CreatedBy = sess.LoginId;
                        //behavior.CreatedOn = DateTime.Now;
                        //dbobj.BehavioursPAs.Add(behavior);
                        //dbobj.SaveChanges();
                        //behavior.StudentPersonalId = sp.StudentPersonalId;
                        //behavior.ParentId = parentId;
                        //behavior.SchoolId = sess.SchoolId;
                        //behavior.BehaviourName = model.preferedActivities2;
                        //behavior.CreatedBy = sess.LoginId;
                        //behavior.CreatedOn = DateTime.Now;
                        //dbobj.BehavioursPAs.Add(behavior);
                        //dbobj.SaveChanges();

                        //parentId = behavId("GENERAL INFORMATION");
                        //behavior.StudentPersonalId = sp.StudentPersonalId;
                        //behavior.ParentId = parentId;
                        //behavior.SchoolId = sess.SchoolId;
                        //behavior.BehaviourName = model.GeneralInformation1;
                        //behavior.CreatedBy = sess.LoginId;
                        //behavior.CreatedOn = DateTime.Now;
                        //dbobj.BehavioursPAs.Add(behavior);
                        //dbobj.SaveChanges();
                        //behavior.StudentPersonalId = sp.StudentPersonalId;
                        //behavior.ParentId = parentId;
                        //behavior.SchoolId = sess.SchoolId;
                        //behavior.BehaviourName = model.GeneralInformation2;
                        //behavior.CreatedBy = sess.LoginId;
                        //behavior.CreatedOn = DateTime.Now;
                        //dbobj.BehavioursPAs.Add(behavior);
                        //dbobj.SaveChanges();

                        //parentId = behavId("SUGGESTED PROACTIVE ENVIRONMENTAL PROCEDURES");
                        //behavior.StudentPersonalId = sp.StudentPersonalId;
                        //behavior.ParentId = parentId;
                        //behavior.SchoolId = sess.SchoolId;
                        //behavior.BehaviourName = model.SuggestedProactiveEnvironmentalProcedures1;
                        //behavior.CreatedBy = sess.LoginId;
                        //behavior.CreatedOn = DateTime.Now;
                        //dbobj.BehavioursPAs.Add(behavior);
                        //dbobj.SaveChanges();
                        //behavior.StudentPersonalId = sp.StudentPersonalId;
                        //behavior.ParentId = parentId;
                        //behavior.SchoolId = sess.SchoolId;
                        //behavior.BehaviourName = model.SuggestedProactiveEnvironmentalProcedures2;
                        //behavior.CreatedBy = sess.LoginId;
                        //behavior.CreatedOn = DateTime.Now;
                        //dbobj.BehavioursPAs.Add(behavior);
                        //dbobj.SaveChanges();
                        #endregion




                        Result = "Sucess";
                        AddEventModel.CreateSystemEvent("New Client Added[" + model.LastName + ", " + model.FirstName + "]", "Client Added", "1) New Client Named " + model.LastName + ", " + model.FirstName + "  Was Added");
                    }
                    else
                    {
                        sp = dbobj.StudentPersonals.Where(x => x.StudentPersonalId == sess.StudentId && x.SchoolId == sess.SchoolId).SingleOrDefault();
                        adrRel = dbobj.StudentAddresRels.Where(objAddressRel => objAddressRel.StudentPersonalId == sess.StudentId && objAddressRel.ContactSequence == 0).SingleOrDefault();
                        // sp.LocalId = Convert.ToString(sp.ClientId);
                        sp.Prefix = model.Prefix;
                        sp.FirstName = model.FirstName;
                        sp.LastName = model.LastName;
                        sp.MiddleName = model.MiddleName;
                        if (model.LastNameSuffix != "0")
                        {
                            sp.Suffix = model.LastNameSuffix;
                        }
                        sp.BirthDate = DateTime.ParseExact(model.DateOfBirth, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        try
                        {
                            sp.Photodate = DateTime.ParseExact(model.Photodate, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        }
                        catch
                        {
                            sp.Photodate = null;
                        } 
                        if (model.PlaceOfBirth != null)
                        {
                            sp.PlaceOfBirth = model.PlaceOfBirth;
                        }
                        if (model.CountryofBirth != null)
                        {
                            sp.CountryOfBirth = model.CountryofBirth;
                        }
                        if (model.StateOfBirth != null)
                        {
                            sp.StateOfBirth = model.StateOfBirth;
                        }
                        sp.NickName = model.NickName;
                        if (model.Citizenship != 0)
                        {
                            sp.CitizenshipStatus = model.Citizenship;
                        }
                        sp.RaceId = model.Race;
                        sp.Gender = model.Gender;
                        if (model.HairColor != null)
                        {
                            sp.HairColor = model.HairColor;
                        }
                        if (model.EyeColor != null)
                        {
                            sp.EyeColor = model.EyeColor;
                        }
                        if (model.MostRecentGradeLevel != null)
                        {
                            sp.MostRecentGradeLevel = model.MostRecentGradeLevel;
                        }
                        sp.Ambulatory = model.Ambulatory;
                        sp.EnglishProficiency = model.EnglishProficiency;
                        sp.SASID = model.SASID;
                        sp.Medicaid = model.Medicaid;
                        sp.Notes = model.Note;

                        sp.Intensive = model.Intensive;
                        sp.IsGuardian = model.IsGuardian;
                        if (model.DayProgarm != null)
                        {
                            sp.DayProgarm = model.DayProgarm;
                        }
                        if (model.ClassroomWorkshop != null)
                        {
                            sp.ClassroomWorkshop = model.ClassroomWorkshop;
                        }
                        sp.TeacherInstructor = model.TeacherInstructor;
                        sp.ProgramSpecialist = model.ProgramSpecialist;
                        sp.EDUBehaviorAnalyst = model.EDUBehaviorAnalyst;
                        sp.CurriculumCoordinator = model.CurriculumCoordinator;
                        sp.ResidentialProgram = model.ResidentialProgram;
                        if (model.House != null)
                        {
                            sp.House = model.House;
                        }
                        sp.ProgramManagerQMRP = model.ProgramManagerQMRP;
                        sp.HouseSupervisor = model.HouseSupervisor;
                        sp.ResidentialBehaviorAnalyst = model.ResidentialBehaviorAnalyst;
                        sp.PrimaryNurse = model.PrimaryNurse;
                        sp.UnitClerk = model.UnitClerk;
                        //============================== List 6 - Task #2   
                        ////sp.TeacherInstructor = "P" + model.StaffPosition1.ToString() + ',' + "S" + model.Position1Staff.ToString(); // List 6 - task #2 - 1-Feb-2021
                        ////sp.ProgramSpecialist = "P" + model.StaffPosition2.ToString() + ',' + "S" + model.Position2Staff.ToString(); // List 6 - task #2 - 1-Feb-2021
                        ////sp.EDUBehaviorAnalyst = "P" + model.StaffPosition3.ToString() + ',' + "S" + model.Position3Staff.ToString(); // List 6 - task #2 - 1-Feb-2021
                        ////sp.CurriculumCoordinator = "P" + model.StaffPosition4.ToString() + ',' + "S" + model.Position4Staff.ToString(); // List 6 - task #2 - 1-Feb-2021
                        ////sp.ResidentialProgram = "P" + model.StaffPosition5.ToString() + ',' + "S" + model.Position5Staff.ToString();  // List 6 - task #2 - 1-Feb-2021                      
                        ////sp.ProgramManagerQMRP = "P" + model.StaffPosition6.ToString() + ',' + "S" + model.Position6Staff.ToString(); // List 6 - task #2 - 1-Feb-2021
                        ////sp.HouseSupervisor = "P" + model.StaffPosition7.ToString() + ',' + "S" + model.Position7Staff.ToString(); // List 6 - task #2 - 1-Feb-2021
                        ////sp.ResidentialBehaviorAnalyst = "P" + model.StaffPosition8.ToString() + ',' + "S" + model.Position8Staff.ToString(); // List 6 - task #2 - 1-Feb-2021
                        ////sp.PrimaryNurse = "P" + model.StaffPosition9.ToString() + ',' + "S" + model.Position9Staff.ToString(); // List 6 - task #2 - 1-Feb-2021
                        ////sp.UnitClerk = "P" + model.StaffPosition10.ToString() + ',' + "S" + model.Position10Staff.ToString();  // List 6 - task #2 - 1-Feb-2021
                        sp.PositionStaff1 = "P" + model.StaffPosition1.ToString() + ',' + "S" + model.Position1Staff.ToString(); // List 6 - task #2 - 1-Feb-2021
                        sp.PositionStaff2 = "P" + model.StaffPosition2.ToString() + ',' + "S" + model.Position2Staff.ToString(); // List 6 - task #2 - 1-Feb-2021
                        sp.PositionStaff3 = "P" + model.StaffPosition3.ToString() + ',' + "S" + model.Position3Staff.ToString(); // List 6 - task #2 - 1-Feb-2021
                        sp.PositionStaff4 = "P" + model.StaffPosition4.ToString() + ',' + "S" + model.Position4Staff.ToString(); // List 6 - task #2 - 1-Feb-2021
                        sp.PositionStaff5 = "P" + model.StaffPosition5.ToString() + ',' + "S" + model.Position5Staff.ToString();  // List 6 - task #2 - 1-Feb-2021 
                        sp.PositionStaff6 = "P" + model.StaffPosition6.ToString() + ',' + "S" + model.Position6Staff.ToString(); // List 6 - task #2 - 1-Feb-2021
                        sp.PositionStaff7 = "P" + model.StaffPosition7.ToString() + ',' + "S" + model.Position7Staff.ToString(); // List 6 - task #2 - 1-Feb-2021
                        sp.PositionStaff8 = "P" + model.StaffPosition8.ToString() + ',' + "S" + model.Position8Staff.ToString(); // List 6 - task #2 - 1-Feb-2021
                        sp.PositionStaff9 = "P" + model.StaffPosition9.ToString() + ',' + "S" + model.Position9Staff.ToString(); // List 6 - task #2 - 1-Feb-2021
                        sp.PositionStaff10 = "P" + model.StaffPosition10.ToString() + ',' + "S" + model.Position10Staff.ToString();  // List 6 - task #2 - 1-Feb-2021
                        //============================== List 6 - Task #2    
                        sp.PhotoPermComment = model.PhotoPermComment;
                        sp.PhotoRelease = model.PhotoRelease;
                        sp.TripRestriction1 = model.TripRestriction1;
                        sp.TripRestriction2 = model.TripRestriction2;
                        sp.TripComments = model.TripComments;
                        sp.ClientInfoComments = model.ClientInfoComments;

                        if (model.Classification1 == null)
                        {
                            model.Classification1 = 0;
                        }
                        if (model.Classification2 == null)
                        {
                            model.Classification2 = 0;
                        }
                        if (model.Classification3 == null)
                        {
                            model.Classification3 = 0;
                        }
                        if (model.Classification4 == null)
                        {
                            model.Classification4 = 0;
                        }
                        if (model.Classification5 == null)
                        {
                            model.Classification5 = 0;
                        }

                        sp.Classification1 = model.Classification1;
                        sp.Classification2 = model.Classification2;
                        sp.Classification3 = model.Classification3;
                        sp.Classification4 = model.Classification4;
                        sp.Classification5 = model.Classification5;

                        try
                        {
                            sp.AdmissionDate = DateTime.ParseExact(model.DateUpdated, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        }
                        catch
                        {
                            sp.AdmissionDate = null;
                        }
                        if (model.Height != null)
                        {
                            sp.Height = Convert.ToDecimal(model.Height);
                        }
                        if (model.Weight != null)
                        {
                            sp.Weight = Convert.ToDecimal(model.Weight);
                        }
                        if (model.PrimaryLanguage != null)
                        {
                            sp.PrimaryLanguage = model.PrimaryLanguage;
                        }
                        if (model.DistigushingMarks != null)
                        {
                            sp.DistingushingMarks = model.DistigushingMarks;
                        }
                        if (model.LegalCompetencyStatus != null)
                        {
                            sp.LegalCompetencyStatus = model.LegalCompetencyStatus;
                        }
                        if (model.OtherStateAgenciesInvolvedWithStudent != null)
                        {
                            sp.OtherStateAgenciesInvolvedWithStudent = model.OtherStateAgenciesInvolvedWithStudent;
                        }
                        if (model.MaritalStatusofBothParents != null)
                        {
                            sp.MaritalStatusofBothParents = model.MaritalStatusofBothParents;
                        }
                        if (model.CaseManagerEducational != null)
                        {
                            sp.CaseManagerEducational = model.CaseManagerEducational;
                        }
                        if (model.CaseManagerResidential != null)
                        {
                            sp.CaseManagerResidential = model.CaseManagerResidential;
                        }
                        if (sourceFile != null)
                        {

                            byte[] fileBytes = new byte[sourceFile.ContentLength];
                            int byteCount = sourceFile.InputStream.Read(fileBytes, 0, (int)sourceFile.ContentLength);

                            sp.ImageUrl = Convert.ToBase64String(fileBytes);
                            model.ImageUrl = dirpath + "/" + sp.StudentPersonalId + "-" + sourceFile.FileName;


                        }
                        else
                        {
                            if (sp.ImageUrl == null)
                            {
                                FileInfo fileInfo = new FileInfo(dirpath);
                                byte[] data = new byte[fileInfo.Length];

                                using (FileStream fs = fileInfo.OpenRead())
                                {
                                    fs.Read(data, 0, data.Length);
                                }
                                sp.ImageUrl = Convert.ToBase64String(data);
                            }
                        }

                        sp.ImagePermission = model.PhotoReleasePermission;
                        if (model.GuardianshipStatus != null)
                        {
                            sp.GuardianShip = model.GuardianshipStatus;
                        }

                        sp.IEPReferralFullName = model.ReferralIEPFullName;
                        sp.IEPReferralPhone = model.ReferralIEPPhone;
                        sp.IEPReferralReferrinAgency = model.ReferralIEPReferringAgency;
                        sp.IEPReferralSourceofTuition = model.ReferralIEPSourceofTuition;
                        sp.IEPReferralTitle = model.ReferralIEPTitle;

                        try
                        {
                            sp.DateInitiallyEligibleforSpecialEducation = DateTime.ParseExact(model.DateInitiallyEligibleforSpecialEducation, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        }
                        catch
                        {
                            sp.DateInitiallyEligibleforSpecialEducation = null;
                        }
                        try
                        {
                            sp.DateofMostRecentSpecialEducationEvaluations = DateTime.ParseExact(model.DateofMostRecentSpecialEducationEvaluations, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        }
                        catch
                        {
                            sp.DateofMostRecentSpecialEducationEvaluations = null;
                        }
                        try
                        {
                            sp.DateofNextScheduled3YearEvaluation = DateTime.ParseExact(model.DateofNextScheduled3YearEvaluation, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        }
                        catch
                        {
                            sp.DateofNextScheduled3YearEvaluation = null;
                        }
                        try
                        {
                            sp.CurrentIEPStartDate = DateTime.ParseExact(model.CurrentIEPStartDate, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        }
                        catch
                        {
                            sp.CurrentIEPStartDate = null;
                        }
                        try
                        {
                            sp.CurrentIEPExpirationDate = DateTime.ParseExact(model.CurrentIEPExpirationDate, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        }
                        catch
                        {
                            sp.CurrentIEPExpirationDate = null;
                        }
                        try
                        {
                            sp.DischargeDate = DateTime.ParseExact(model.DischargeDate, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        }
                        catch
                        {
                            sp.DischargeDate = null;
                        }
                        sp.LocationAfterDischarge = model.LocationAfterDischarge;
                        sp.StudentType = "Client";
                        sp.MelmarkNewEnglandsFollowUpResponsibilities = model.MelmarkNewEnglandsFollowUpResponsibilities;

                        sp.ModifiedBy = sess.LoginId;
                        sp.ModifiedOn = DateTime.Now;
                        dbobj.SaveChanges();

                        addr = dbobj.AddressLists.Where(x => x.AddressId == adrRel.AddressId).SingleOrDefault();
                        addr.ApartmentType = model.AddressLine1;
                        addr.StreetName = model.AddressLine2;
                        addr.AddressLine3 = model.AddressLine3;
                        addr.AddressType = MetaData._StudentAddressType;
                        addr.City = model.City;
                        addr.CountryId = model.Country;
                        addr.StateProvince = model.State;
                        addr.PostalCode = model.ZipCode;
                        addr.ClientAddressPhone = model.ClientAddressPhone;
                        addr.ModifiedBy = sess.LoginId;
                        addr.ModifiedOn = DateTime.Now;
                        dbobj.SaveChanges();

                        studentPA = dbobj.StudentPersonalPAs.Where(objStudentPersonalPA => objStudentPersonalPA.StudentPersonalId == sess.StudentId
                            && objStudentPersonalPA.SchoolId == sess.SchoolId).SingleOrDefault();
                        if (studentPA == null)
                        {
                            studentPA = new StudentPersonalPA();
                            dbobj.StudentPersonalPAs.Add(studentPA);
                        }

                        studentPA.StudentPersonalId = sess.StudentId;
                        studentPA.SchoolId = sess.SchoolId;
                      //  studentPA.Allergies = model.Allergie;
                        studentPA.Bathroom = model.Bathroom;
                      //  studentPA.Diet = model.Diet;
                        studentPA.dy_TaskOrBreak = model.TaskORBreak;
                        studentPA.dy_TransitionInside = model.TransitionInside;
                        studentPA.dy_TransitionUnevenGround = model.TransitionUnevenGround;
                        studentPA.Consciousness = model.Consciousness;
                        studentPA.CommonAreas = model.CommonAreas;
                        studentPA.RiskOfResistance = model.RiskOfResistance;
                        studentPA.Mobility = model.Mobility;
                        studentPA.NeedForExtraHelp = model.NeedForExtraHelp;
                        studentPA.ResponseToInstruction = model.ResponseToInstruction;
                        studentPA.WalkingResponses = model.WalkingResponse;
                        studentPA.ho_BedroomAsleep = model.BedroomAsleep;
                        studentPA.ho_BedroomAwake = model.BedroomAwake;
                        studentPA.ho_CommonAres = model.CommonAreas;
                        studentPA.OnCampus = model.OnCampus;
                        studentPA.WhenTranspoting = model.WhenTranspoting;
                        studentPA.OffCampus = model.OffCampus;
                        studentPA.PoolOrSwimming = model.PoolOrSwimming;
                        studentPA.Van = model.van;
                       // studentPA.Seizures = model.Seizures;
                       // studentPA.Other = model.Other;
                        studentPA.FundingSource = model.Funding;
                        studentPA.ModifiedBy = sess.LoginId;
                        studentPA.ModifiedOn = DateTime.Now;

                        var EmergencyContacts = dbobj.EmergencyContactSchools.Where(objEmergencyContact => objEmergencyContact.StudentPersonalId == sess.StudentId).ToList();
                        if (EmergencyContacts.Count > 0)
                        {
                            foreach (var item in EmergencyContacts)
                            {
                                if (item.SequenceId == 1)
                                {
                                    objEmergency = dbobj.EmergencyContactSchools.Where(objEmergencys => objEmergencys.StudentPersonalId == sess.StudentId &&
                                        objEmergencys.SequenceId == 1).SingleOrDefault();
                                    objEmergency.FirstName = model.EmergencyContactFirstName1;
                                    objEmergency.LastName = model.EmergencyContactLastName1;
                                    objEmergency.Title = model.EmergencyContactTitle1;
                                    objEmergency.Phone = model.EmergencyContactPhone1;
                                    objEmergency.ModifiedBy = sess.LoginId;
                                    objEmergency.ModifiedOn = DateTime.Now;
                                    dbobj.SaveChanges();
                                }
                                if (item.SequenceId == 2)
                                {

                                    objEmergency = dbobj.EmergencyContactSchools.Where(objEmergencys => objEmergencys.StudentPersonalId == sess.StudentId &&
                                        objEmergencys.SequenceId == 2).SingleOrDefault();
                                    objEmergency.FirstName = model.EmergencyContactFirstName2;
                                    objEmergency.LastName = model.EmergencyContactLastName2;
                                    objEmergency.Title = model.EmergencyContactTitle2;
                                    objEmergency.Phone = model.EmergencyContactPhone2;
                                    objEmergency.ModifiedBy = sess.LoginId;
                                    objEmergency.ModifiedOn = DateTime.Now;
                                    dbobj.SaveChanges();
                                }
                                if (item.SequenceId == 3)
                                {

                                    objEmergency = dbobj.EmergencyContactSchools.Where(objEmergencys => objEmergencys.StudentPersonalId == sess.StudentId &&
                                        objEmergencys.SequenceId == 3).SingleOrDefault();
                                    objEmergency.FirstName = model.EmergencyContactFirstName3;
                                    objEmergency.LastName = model.EmergencyContactLastName3;
                                    objEmergency.Title = model.EmergencyContactTitle3;
                                    objEmergency.Phone = model.EmergencyContactPhone3;
                                    objEmergency.ModifiedBy = sess.LoginId;
                                    objEmergency.ModifiedOn = DateTime.Now;
                                    dbobj.SaveChanges();
                                }
                                if (item.SequenceId == 4)
                                {

                                    objEmergency = dbobj.EmergencyContactSchools.Where(objEmergencys => objEmergencys.StudentPersonalId == sess.StudentId &&
                                        objEmergencys.SequenceId == 4).SingleOrDefault();
                                    objEmergency.FirstName = model.EmergencyContactFirstName4;
                                    objEmergency.LastName = model.EmergencyContactLastName4;
                                    objEmergency.Title = model.EmergencyContactTitle4;
                                    objEmergency.Phone = model.EmergencyContactPhone4;
                                    objEmergency.ModifiedBy = sess.LoginId;
                                    objEmergency.ModifiedOn = DateTime.Now;
                                    dbobj.SaveChanges();
                                }
                                if (item.SequenceId == 5)
                                {

                                    objEmergency = dbobj.EmergencyContactSchools.Where(objEmergencys => objEmergencys.StudentPersonalId == sess.StudentId &&
                                        objEmergencys.SequenceId == 5).SingleOrDefault();
                                    objEmergency.FirstName = model.EmergencyContactFirstName5;
                                    objEmergency.LastName = model.EmergencyContactLastName5;
                                    objEmergency.Title = model.EmergencyContactTitle5;
                                    objEmergency.Phone = model.EmergencyContactPhone5;
                                    objEmergency.ModifiedBy = sess.LoginId;
                                    objEmergency.ModifiedOn = DateTime.Now;
                                    dbobj.SaveChanges();
                                }
                            }
                        }
                        else
                        {
                            objEmergency.StudentPersonalId = sess.StudentId;
                            objEmergency.SchoolId = sess.SchoolId;
                            objEmergency.FirstName = model.EmergencyContactFirstName1;
                            objEmergency.LastName = model.EmergencyContactLastName1;
                            objEmergency.Title = model.EmergencyContactTitle1;
                            objEmergency.Phone = model.EmergencyContactPhone1;
                            objEmergency.CreatedBy = sess.LoginId;
                            objEmergency.CreatedOn = DateTime.Now;
                            objEmergency.SequenceId = 1;
                            dbobj.EmergencyContactSchools.Add(objEmergency);
                            // dbobj.SaveChanges();

                            objEmergency = new EmergencyContactSchool();
                            objEmergency.StudentPersonalId = sess.StudentId;
                            objEmergency.SchoolId = sess.SchoolId;
                            objEmergency.FirstName = model.EmergencyContactFirstName2;
                            objEmergency.LastName = model.EmergencyContactLastName2;
                            objEmergency.Title = model.EmergencyContactTitle2;
                            objEmergency.Phone = model.EmergencyContactPhone2;
                            objEmergency.CreatedBy = sess.LoginId;
                            objEmergency.CreatedOn = DateTime.Now;
                            objEmergency.SequenceId = 2;
                            dbobj.EmergencyContactSchools.Add(objEmergency);
                            // dbobj.SaveChanges();

                            objEmergency = new EmergencyContactSchool();
                            objEmergency.StudentPersonalId = sess.StudentId;
                            objEmergency.SchoolId = sess.SchoolId;
                            objEmergency.FirstName = model.EmergencyContactFirstName3;
                            objEmergency.LastName = model.EmergencyContactLastName3;
                            objEmergency.Title = model.EmergencyContactTitle3;
                            objEmergency.Phone = model.EmergencyContactPhone3;
                            objEmergency.CreatedBy = sess.LoginId;
                            objEmergency.CreatedOn = DateTime.Now;
                            objEmergency.SequenceId = 3;
                            dbobj.EmergencyContactSchools.Add(objEmergency);
                            //   dbobj.SaveChanges();

                            objEmergency = new EmergencyContactSchool();
                            objEmergency.StudentPersonalId = sess.StudentId;
                            objEmergency.SchoolId = sess.SchoolId;
                            objEmergency.FirstName = model.EmergencyContactFirstName4;
                            objEmergency.LastName = model.EmergencyContactLastName4;
                            objEmergency.Title = model.EmergencyContactTitle4;
                            objEmergency.Phone = model.EmergencyContactPhone4;
                            objEmergency.CreatedBy = sess.LoginId;
                            objEmergency.CreatedOn = DateTime.Now;
                            objEmergency.SequenceId = 4;
                            dbobj.EmergencyContactSchools.Add(objEmergency);
                            //      dbobj.SaveChanges();

                            objEmergency = new EmergencyContactSchool();
                            objEmergency.StudentPersonalId = sess.StudentId;
                            objEmergency.SchoolId = sess.SchoolId;
                            objEmergency.FirstName = model.EmergencyContactFirstName5;
                            objEmergency.LastName = model.EmergencyContactLastName5;
                            objEmergency.Title = model.EmergencyContactTitle5;
                            objEmergency.Phone = model.EmergencyContactPhone5;
                            objEmergency.CreatedBy = sess.LoginId;
                            objEmergency.CreatedOn = DateTime.Now;
                            objEmergency.SequenceId = 5;
                            dbobj.EmergencyContactSchools.Add(objEmergency);
                            dbobj.SaveChanges();
                        }

                        var SchoolsAttended = dbobj.SchoolsAttendeds.Where(objschools => objschools.StudentPersonalId == sess.StudentId).ToList();
                        if (SchoolsAttended.Count > 0)
                        {
                            foreach (var item in SchoolsAttended)
                            {
                                if (item.SequenceId == 1)
                                {
                                    objSchool = dbobj.SchoolsAttendeds.Where(objSchul => objSchul.StudentPersonalId == sess.StudentId && objSchul.SequenceId == 1).SingleOrDefault();
                                    objSchool.SchoolName = model.SchoolName1;
                                    try
                                    {
                                        objSchool.DateFrom = DateTime.ParseExact(model.DateFrom1, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                    }
                                    catch
                                    {
                                        objSchool.DateFrom = null;
                                    }
                                    try
                                    {
                                        objSchool.DateTo = DateTime.ParseExact(model.DateTo1, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                    }
                                    catch
                                    {
                                        objSchool.DateTo = null;
                                    }
                                    objSchool.Address1 = model.SchoolAttendedAddress11;
                                    objSchool.Address2 = model.SchoolAttendedAddress21;
                                    objSchool.City = model.SchoolAttendedCity1;
                                    // objSchool.State = model.SchoolAttendedState1;
                                    objSchool.State = model.intSchoolAttendedState1.ToString();
                                    objSchool.ModifiedBy = sess.LoginId;
                                    objSchool.ModifiedOn = DateTime.Now;
                                    dbobj.SaveChanges();
                                }
                                if (item.SequenceId == 2)
                                {
                                    objSchool = dbobj.SchoolsAttendeds.Where(objSchul => objSchul.StudentPersonalId == sess.StudentId && objSchul.SequenceId == 2).SingleOrDefault();
                                    objSchool.SchoolName = model.SchoolName2;
                                    try
                                    {
                                        objSchool.DateFrom = DateTime.ParseExact(model.DateFrom2, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                    }
                                    catch
                                    {
                                        objSchool.DateFrom = null;
                                    }
                                    try
                                    {
                                        objSchool.DateTo = DateTime.ParseExact(model.DateTo2, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                    }
                                    catch
                                    {
                                        objSchool.DateTo = null;
                                    }
                                    objSchool.Address1 = model.SchoolAttendedAddress12;
                                    objSchool.Address2 = model.SchoolAttendedAddress22;
                                    objSchool.City = model.SchoolAttendedCity2;
                                    // objSchool.State = model.SchoolAttendedState2;
                                    objSchool.State = model.intSchoolAttendedState2.ToString();
                                    objSchool.ModifiedBy = sess.LoginId;
                                    objSchool.ModifiedOn = DateTime.Now;
                                    dbobj.SaveChanges();
                                }
                                if (item.SequenceId == 3)
                                {
                                    objSchool = dbobj.SchoolsAttendeds.Where(objSchul => objSchul.StudentPersonalId == sess.StudentId && objSchul.SequenceId == 3).SingleOrDefault();
                                    objSchool.SchoolName = model.SchoolName3;
                                    try
                                    {
                                        objSchool.DateFrom = DateTime.ParseExact(model.DateFrom3, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                    }
                                    catch
                                    {
                                        objSchool.DateFrom = null;
                                    }
                                    try
                                    {
                                        objSchool.DateTo = DateTime.ParseExact(model.DateTo3, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                    }
                                    catch
                                    {
                                        objSchool.DateTo = null;
                                    }
                                    objSchool.Address1 = model.SchoolAttendedAddress13;
                                    objSchool.Address2 = model.SchoolAttendedAddress23;
                                    objSchool.City = model.SchoolAttendedCity3;
                                    //objSchool.State = model.SchoolAttendedState3;
                                    objSchool.State = model.intSchoolAttendedState3.ToString();
                                    objSchool.ModifiedBy = sess.LoginId;
                                    objSchool.ModifiedOn = DateTime.Now;
                                    dbobj.SaveChanges();
                                }
                            }
                        }
                        else
                        {
                            objSchool.StudentPersonalId = sess.StudentId;
                            objSchool.SchoolId = sess.SchoolId;
                            objSchool.SchoolName = model.SchoolName1;
                            try
                            {
                                objSchool.DateFrom = DateTime.ParseExact(model.DateFrom1, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                            }
                            catch
                            {
                                objSchool.DateFrom = null;
                            }
                            try
                            {
                                objSchool.DateTo = DateTime.ParseExact(model.DateTo1, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                            }
                            catch
                            {
                                objSchool.DateTo = null;
                            }
                            objSchool.Address1 = model.SchoolAttendedAddress11;
                            objSchool.Address2 = model.SchoolAttendedAddress21;
                            objSchool.City = model.SchoolAttendedCity1;
                            //objSchool.State = model.SchoolAttendedState1;
                            objSchool.State = model.intSchoolAttendedState1.ToString();
                            objSchool.CreatedBy = sess.LoginId;
                            objSchool.CreatedOn = DateTime.Now;
                            objSchool.SequenceId = 1;
                            dbobj.SchoolsAttendeds.Add(objSchool);
                            dbobj.SaveChanges();

                            objSchool = new SchoolsAttended();
                            objSchool.StudentPersonalId = sess.StudentId;
                            objSchool.SchoolId = sess.SchoolId;
                            objSchool.SchoolName = model.SchoolName2;
                            try
                            {
                                objSchool.DateFrom = DateTime.ParseExact(model.DateFrom2, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                            }
                            catch
                            {
                                objSchool.DateFrom = null;
                            }
                            try
                            {
                                objSchool.DateTo = DateTime.ParseExact(model.DateTo2, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                            }
                            catch
                            {
                                objSchool.DateTo = null;
                            }
                            objSchool.Address1 = model.SchoolAttendedAddress12;
                            objSchool.Address2 = model.SchoolAttendedAddress22;
                            objSchool.City = model.SchoolAttendedCity2;
                            //objSchool.State = model.SchoolAttendedState2;
                            objSchool.State = model.intSchoolAttendedState2.ToString();
                            objSchool.CreatedBy = sess.LoginId;
                            objSchool.CreatedOn = DateTime.Now;
                            objSchool.SequenceId = 2;
                            dbobj.SchoolsAttendeds.Add(objSchool);
                            dbobj.SaveChanges();

                            objSchool = new SchoolsAttended();
                            objSchool.StudentPersonalId = sess.StudentId;
                            objSchool.SchoolId = sess.SchoolId;
                            objSchool.SchoolName = model.SchoolName3;
                            try
                            {
                                objSchool.DateFrom = DateTime.ParseExact(model.DateFrom3, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                            }
                            catch
                            {
                                objSchool.DateFrom = null;
                            }
                            try
                            {
                                objSchool.DateTo = DateTime.ParseExact(model.DateTo3, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                            }
                            catch
                            {
                                objSchool.DateTo = null;
                            }
                            objSchool.Address1 = model.SchoolAttendedAddress13;
                            objSchool.Address2 = model.SchoolAttendedAddress23;
                            objSchool.City = model.SchoolAttendedCity3;
                            //objSchool.State = model.SchoolAttendedState3;
                            objSchool.State = model.intSchoolAttendedState3.ToString();
                            objSchool.CreatedBy = sess.LoginId;
                            objSchool.CreatedOn = DateTime.Now;
                            objSchool.SequenceId = 3;
                            dbobj.SchoolsAttendeds.Add(objSchool);
                            dbobj.SaveChanges();

                        }

                        try
                        {
                            objInsurance = dbobj.Insurances.Where(objInsu => objInsu.StudentPersonalId == sess.StudentId && objInsu.PreferType == "Primary").SingleOrDefault();
                            if (objInsurance != null)
                            {
                                objInsurance.InsuranceType = model.InsuranceType;
                                objInsurance.PolicyHolder = model.PolicyHolder;
                                objInsurance.PolicyNumber = model.PolicyNumber;
                                objInsurance.PreferType = "Primary";
                                objInsurance.ModifiedBy = sess.LoginId;
                                objInsurance.ModifiedOn = DateTime.Now;
                                dbobj.SaveChanges();
                            }
                            else
                            {
                                objInsurance = new Insurance();
                                objInsurance.StudentPersonalId = sess.StudentId;
                                objInsurance.SchoolId = sess.SchoolId;
                                objInsurance.InsuranceType = model.InsuranceType;
                                objInsurance.PolicyHolder = model.PolicyHolder;
                                objInsurance.PolicyNumber = model.PolicyNumber;
                                objInsurance.PreferType = "Primary";
                                objInsurance.CreatedBy = sess.LoginId;
                                objInsurance.CreatedOn = DateTime.Now;
                                dbobj.Insurances.Add(objInsurance);
                                dbobj.SaveChanges();
                            }

                            objInsurance = dbobj.Insurances.Where(objInsu => objInsu.StudentPersonalId == sess.StudentId && objInsu.PreferType == "Secondary").SingleOrDefault();
                            if (objInsurance != null)
                            {
                                objInsurance.InsuranceType = model.InsuranceType1;
                                objInsurance.PolicyHolder = model.PolicyHolder1;
                                objInsurance.PolicyNumber = model.PolicyNumber1;
                                objInsurance.PreferType = "Secondary";
                                objInsurance.ModifiedBy = sess.LoginId;
                                objInsurance.ModifiedOn = DateTime.Now;
                                dbobj.SaveChanges();
                            }
                            else
                            {
                                objInsurance = new Insurance();
                                objInsurance.StudentPersonalId = sess.StudentId;
                                objInsurance.SchoolId = sess.SchoolId;
                                objInsurance.InsuranceType = model.InsuranceType1;
                                objInsurance.PolicyHolder = model.PolicyHolder1;
                                objInsurance.PolicyNumber = model.PolicyNumber1;
                                objInsurance.PreferType = "Secondary";
                                objInsurance.CreatedBy = sess.LoginId;
                                objInsurance.CreatedOn = DateTime.Now;
                                dbobj.Insurances.Add(objInsurance);
                                dbobj.SaveChanges();
                            }
                        }
                        catch { }


                        //var diagnoses = dbobj.DiaganosesPAs.Where(objDiagnoses => objDiagnoses.StudentPersonalId == sess.StudentId && objDiagnoses.SchoolId == sess.SchoolId).ToList();
                        //var diagnoses = dbobj.DiaganosesPAs.Where(objDiagnoses => objDiagnoses.StudentPersonalId == sess.StudentId).ToList();
                        //int i = 0;
                        //if (diagnoses.Count > 0)
                        //{
                        //    foreach (var item in diagnoses)
                        //    {

                        //        //diagnose = dbobj.DiaganosesPAs.Where(objDiagno => objDiagno.DiaganosePAId == item.DiaganosePAId && objDiagno.SchoolId == sess.SchoolId).SingleOrDefault();
                        //        diagnose = dbobj.DiaganosesPAs.Where(objDiagno => objDiagno.DiaganosePAId == item.DiaganosePAId).SingleOrDefault();
                        //        diagnose.Diaganoses = model.Diagnosis[i].Name;
                        //        diagnose.ModifiedBy = sess.LoginId;
                        //        diagnose.ModifiedOn = DateTime.Now;
                        //        dbobj.SaveChanges();
                        //        i++;
                        //    }
                        //}
                        //else
                        //{
                        //    foreach (var item in model.Diagnosis)
                        //    {
                        //        diagnose.StudentPersonalId = sess.StudentId;
                        //        diagnose.SchoolId = sess.SchoolId;
                        //        diagnose.Diaganoses = model.Diagnosis[i].Name;
                        //        diagnose.CreatedBy = sess.LoginId;
                        //        diagnose.CreatedOn = DateTime.Now;
                        //        dbobj.DiaganosesPAs.Add(diagnose);
                        //        dbobj.SaveChanges();
                        //        i++;
                        //    }

                        //}
                        if (model.Adapt.Count > 0)
                        {
                            Int16 Id = 0;

                            for (int l = 0; l < model.Adapt.Count; l++)
                            {
                                if (model.Adapt[l].AdaptiveEquimentId != 0)
                                {
                                    Id = Convert.ToInt16(model.Adapt[l].AdaptiveEquimentId);
                                    var ADP = dbobj.AdaptiveEquipments.Where(objAdapt => objAdapt.AdaptiveEquipmentId == Id && objAdapt.SchoolId == sess.SchoolId).SingleOrDefault();
                                    ADP.Item = model.Adapt[l].item;
                                    ADP.ScheduleForUse = model.Adapt[l].ScheduledForUss;
                                    ADP.StorageLocation = model.Adapt[l].StorageLocation;
                                    ADP.CleaningInstruction = model.Adapt[l].CleaningInstruction;
                                    ADP.ModifiedBy = sess.LoginId;
                                    ADP.ModifiedOn = DateTime.Now;
                                    dbobj.SaveChanges();
                                }

                                else
                                {
                                    adaptive = new AdaptiveEquipment();
                                    adaptive.StudentPersonalId = sp.StudentPersonalId;
                                    adaptive.SchoolId = sess.SchoolId;
                                    adaptive.Item = model.Adapt[l].item;
                                    adaptive.ScheduleForUse = model.Adapt[l].ScheduledForUss;
                                    adaptive.StorageLocation = model.Adapt[l].StorageLocation;
                                    adaptive.CleaningInstruction = model.Adapt[l].CleaningInstruction;
                                    adaptive.CreatedBy = sess.LoginId;
                                    adaptive.CreatedOn = DateTime.Now;
                                    dbobj.AdaptiveEquipments.Add(adaptive);
                                    dbobj.SaveChanges();
                                }
                            }
                        }
                        //var Adaptive = dbobj.AdaptiveEquipments.Where(objAdaptive => objAdaptive.StudentPersonalId == sess.StudentId && objAdaptive.SchoolId == sess.SchoolId).ToList();
                        //i = 0;
                        //if (Adaptive.Count > 0)
                        //{
                        //    foreach (var item in Adaptive)
                        //    {

                        //        adaptive = dbobj.AdaptiveEquipments.Where(objAdapt => objAdapt.AdaptiveEquipmentId == item.AdaptiveEquipmentId && objAdapt.SchoolId == sess.SchoolId).SingleOrDefault();
                        //        adaptive.Item = model.Adapt[i].item;
                        //        adaptive.ScheduleForUse = model.Adapt[i].ScheduledForUss;
                        //        adaptive.StorageLocation = model.Adapt[i].StorageLocation;
                        //        adaptive.CleaningInstruction = model.Adapt[i].CleaningInstruction;
                        //        adaptive.ModifiedBy = sess.LoginId;
                        //        adaptive.ModifiedOn = DateTime.Now;
                        //        dbobj.SaveChanges();
                        //        i++;
                        //    }
                        //}
                        //else
                        //{

                        //    adaptive.StudentPersonalId = sp.StudentPersonalId;
                        //    adaptive.SchoolId = sess.SchoolId;
                        //    adaptive.Item = model.Adapt[i].item;
                        //    adaptive.ScheduleForUse = model.Adapt[i].ScheduledForUss;
                        //    adaptive.StorageLocation = model.Adapt[i].StorageLocation;
                        //    adaptive.CleaningInstruction = model.Adapt[i].CleaningInstruction;
                        //    adaptive.CreatedBy = sess.LoginId;
                        //    adaptive.CreatedOn = DateTime.Now;
                        //    dbobj.AdaptiveEquipments.Add(adaptive);
                        //    dbobj.SaveChanges();
                        //}

                        if (model.BasicBehave.Count > 0)
                        {
                            Int16 Id = 0;

                            for (int l = 0; l < model.BasicBehave.Count; l++)
                            {
                                if (model.BasicBehave[l].BasicBehavioralInformationId != 0)
                                {
                                    Id = Convert.ToInt16(model.BasicBehave[l].BasicBehavioralInformationId);
                                    var BEH = dbobj.BasicBehavioralInformations.Where(objBasicBehav => objBasicBehav.BasicBehavioralInformationId == Id && objBasicBehav.SchoolId == sess.SchoolId).SingleOrDefault();

                                    BEH.TargetBehavior = model.BasicBehave[l].TargetBehavior;
                                    BEH.Definition = model.BasicBehave[l].Definition;
                                    BEH.Antecedent = model.BasicBehave[l].Antecedent;
                                    BEH.FCT = model.BasicBehave[l].FCT;
                                    BEH.Consequence = model.BasicBehave[l].Consequances;
                                    BEH.ModifiedBy = sess.LoginId;
                                    BEH.ModifiedOn = DateTime.Now;
                                    dbobj.SaveChanges();
                                }

                                else
                                {
                                    basicBehavior = new BasicBehavioralInformation();
                                    basicBehavior.StudentPersonalId = sp.StudentPersonalId;
                                    basicBehavior.SchoolId = sess.SchoolId;

                                    basicBehavior.TargetBehavior = model.BasicBehave[l].TargetBehavior;
                                    basicBehavior.Definition = model.BasicBehave[l].Definition;
                                    basicBehavior.Antecedent = model.BasicBehave[l].Antecedent;
                                    basicBehavior.FCT = model.BasicBehave[l].FCT;
                                    basicBehavior.Consequence = model.BasicBehave[l].Consequances;
                                    basicBehavior.ModifiedBy = sess.LoginId;
                                    basicBehavior.ModifiedOn = DateTime.Now;
                                    dbobj.BasicBehavioralInformations.Add(basicBehavior);
                                    dbobj.SaveChanges();
                                }
                            }
                        }

                        //var BasicBehave = dbobj.BasicBehavioralInformations.Where(objBasicBehave => objBasicBehave.StudentPersonalId == sess.StudentId
                        //    && objBasicBehave.SchoolId == sess.SchoolId).ToList();
                        //i = 0;

                        //if (BasicBehave.Count > 0)
                        //{
                        //    foreach (var item in BasicBehave)
                        //    {

                        //        basicBehavior = dbobj.BasicBehavioralInformations.Where(objBasicBehav => objBasicBehav.BasicBehavioralInformationId == item.BasicBehavioralInformationId
                        //            && objBasicBehav.SchoolId == sess.SchoolId).SingleOrDefault();
                        //        basicBehavior.TargetBehavior = model.BasicBehave[i].TargetBehavior;
                        //        basicBehavior.Definition = model.BasicBehave[i].Definition;
                        //        basicBehavior.Antecedent = model.BasicBehave[i].Antecedent;
                        //        basicBehavior.FCT = model.BasicBehave[i].FCT;
                        //        basicBehavior.Consequence = model.BasicBehave[i].Consequances;
                        //        basicBehavior.ModifiedBy = sess.LoginId;
                        //        basicBehavior.ModifiedOn = DateTime.Now;
                        //        dbobj.SaveChanges();
                        //        i++;
                        //    }
                        //}
                        //else
                        //{
                        //    basicBehavior.StudentPersonalId = sp.StudentPersonalId;
                        //    basicBehavior.SchoolId = sess.SchoolId;
                        //    basicBehavior.TargetBehavior = model.BasicBehave[i].TargetBehavior;
                        //    basicBehavior.Definition = model.BasicBehave[i].Definition;
                        //    basicBehavior.Antecedent = model.BasicBehave[i].Antecedent;
                        //    basicBehavior.FCT = model.BasicBehave[i].FCT;
                        //    basicBehavior.Consequence = model.BasicBehave[i].Consequances;
                        //    basicBehavior.CreatedBy = sess.LoginId;
                        //    basicBehavior.CreatedOn = DateTime.Now;
                        //    dbobj.BasicBehavioralInformations.Add(basicBehavior);
                        //    dbobj.SaveChanges();

                        //}

                        var behave = dbobj.BehavioursPAs.Where(objBehav => objBehav.StudentPersonalId == sess.StudentId && objBehav.SchoolId == sess.SchoolId && objBehav.ParentId > 0).ToList();
                        if (behave.Count == 0)
                        {
                            saveBehaviors(behavior, sess, model);
                        }
                        else
                        {
                            int parentId = behavId("LIFTING / TRANSFERS");
                            bool flag = false;
                            foreach (var item in behave)
                            {
                                if (item.ParentId == parentId)
                                {
                                    if (!flag)
                                    {
                                        behavior = dbobj.BehavioursPAs.Where(objBehavs => objBehavs.BehavioursPAId == item.BehavioursPAId).SingleOrDefault();
                                        behavior.BehaviourName = model.LiftingOrTransfers1;
                                        behavior.ModifiedBy = sess.LoginId;
                                        behavior.ModifiedOn = DateTime.Now;
                                        dbobj.SaveChanges();
                                        flag = true;
                                    }
                                    else
                                    {
                                        behavior = dbobj.BehavioursPAs.Where(objBehavs => objBehavs.BehavioursPAId == item.BehavioursPAId).SingleOrDefault();
                                        behavior.BehaviourName = model.LiftingOrTransfers2;
                                        behavior.ModifiedBy = sess.LoginId;
                                        behavior.ModifiedOn = DateTime.Now;
                                        dbobj.SaveChanges();
                                    }
                                }
                            }

                            parentId = behavId("AMBULATION");
                            flag = false;
                            foreach (var item in behave)
                            {
                                if (item.ParentId == parentId)
                                {
                                    if (!flag)
                                    {
                                        behavior = dbobj.BehavioursPAs.Where(objBehavs => objBehavs.BehavioursPAId == item.BehavioursPAId).SingleOrDefault();
                                        behavior.BehaviourName = model.Ambulation1;
                                        behavior.ModifiedBy = sess.LoginId;
                                        behavior.ModifiedOn = DateTime.Now;
                                        dbobj.SaveChanges();
                                        flag = true;
                                    }
                                    else
                                    {
                                        behavior = dbobj.BehavioursPAs.Where(objBehavs => objBehavs.BehavioursPAId == item.BehavioursPAId).SingleOrDefault();
                                        behavior.BehaviourName = model.Ambulation2;
                                        behavior.ModifiedBy = sess.LoginId;
                                        behavior.ModifiedOn = DateTime.Now;
                                        dbobj.SaveChanges();
                                    }
                                }
                            }

                            parentId = behavId("TOILETING");
                            flag = false;
                            foreach (var item in behave)
                            {
                                if (item.ParentId == parentId)
                                {
                                    if (!flag)
                                    {
                                        behavior = dbobj.BehavioursPAs.Where(objBehavs => objBehavs.BehavioursPAId == item.BehavioursPAId).SingleOrDefault();
                                        behavior.BehaviourName = model.Toileting1;
                                        behavior.ModifiedBy = sess.LoginId;
                                        behavior.ModifiedOn = DateTime.Now;
                                        dbobj.SaveChanges();
                                        flag = true;
                                    }
                                    else
                                    {
                                        behavior = dbobj.BehavioursPAs.Where(objBehavs => objBehavs.BehavioursPAId == item.BehavioursPAId).SingleOrDefault();
                                        behavior.BehaviourName = model.Toileting2;
                                        behavior.ModifiedBy = sess.LoginId;
                                        behavior.ModifiedOn = DateTime.Now;
                                        dbobj.SaveChanges();
                                    }
                                }
                            }

                            parentId = behavId("EATING");
                            flag = false;
                            foreach (var item in behave)
                            {
                                if (item.ParentId == parentId)
                                {
                                    if (!flag)
                                    {
                                        behavior = dbobj.BehavioursPAs.Where(objBehavs => objBehavs.BehavioursPAId == item.BehavioursPAId).SingleOrDefault();
                                        behavior.BehaviourName = model.Eating1;
                                        behavior.ModifiedBy = sess.LoginId;
                                        behavior.ModifiedOn = DateTime.Now;
                                        dbobj.SaveChanges();
                                        flag = true;
                                    }
                                    else
                                    {
                                        behavior = dbobj.BehavioursPAs.Where(objBehavs => objBehavs.BehavioursPAId == item.BehavioursPAId).SingleOrDefault();
                                        behavior.BehaviourName = model.Eating2;
                                        behavior.ModifiedBy = sess.LoginId;
                                        behavior.ModifiedOn = DateTime.Now;
                                        dbobj.SaveChanges();
                                    }
                                }
                            }

                            parentId = behavId("SHOWERING");
                            flag = false;
                            foreach (var item in behave)
                            {
                                if (item.ParentId == parentId)
                                {
                                    if (!flag)
                                    {
                                        behavior = dbobj.BehavioursPAs.Where(objBehavs => objBehavs.BehavioursPAId == item.BehavioursPAId).SingleOrDefault();
                                        behavior.BehaviourName = model.Showering1;
                                        behavior.ModifiedBy = sess.LoginId;
                                        behavior.ModifiedOn = DateTime.Now;
                                        dbobj.SaveChanges();
                                        flag = true;
                                    }
                                    else
                                    {
                                        behavior = dbobj.BehavioursPAs.Where(objBehavs => objBehavs.BehavioursPAId == item.BehavioursPAId).SingleOrDefault();
                                        behavior.BehaviourName = model.Showering2;
                                        behavior.ModifiedBy = sess.LoginId;
                                        behavior.ModifiedOn = DateTime.Now;
                                        dbobj.SaveChanges();
                                    }
                                }
                            }

                            parentId = behavId("TOOTHBRUSHING");
                            flag = false;
                            foreach (var item in behave)
                            {
                                if (item.ParentId == parentId)
                                {
                                    if (!flag)
                                    {
                                        behavior = dbobj.BehavioursPAs.Where(objBehavs => objBehavs.BehavioursPAId == item.BehavioursPAId).SingleOrDefault();
                                        behavior.BehaviourName = model.ToothBrushing1;
                                        behavior.ModifiedBy = sess.LoginId;
                                        behavior.ModifiedOn = DateTime.Now;
                                        dbobj.SaveChanges();
                                        flag = true;
                                    }
                                    else
                                    {
                                        behavior = dbobj.BehavioursPAs.Where(objBehavs => objBehavs.BehavioursPAId == item.BehavioursPAId).SingleOrDefault();
                                        behavior.BehaviourName = model.ToothBrushing2;
                                        behavior.ModifiedBy = sess.LoginId;
                                        behavior.ModifiedOn = DateTime.Now;
                                        dbobj.SaveChanges();
                                    }
                                }
                            }

                            parentId = behavId("DRESSING");
                            flag = false;
                            foreach (var item in behave)
                            {
                                if (item.ParentId == parentId)
                                {
                                    if (!flag)
                                    {
                                        behavior = dbobj.BehavioursPAs.Where(objBehavs => objBehavs.BehavioursPAId == item.BehavioursPAId).SingleOrDefault();
                                        behavior.BehaviourName = model.Dressing1;
                                        behavior.ModifiedBy = sess.LoginId;
                                        behavior.ModifiedOn = DateTime.Now;
                                        dbobj.SaveChanges();
                                        flag = true;
                                    }
                                    else
                                    {
                                        behavior = dbobj.BehavioursPAs.Where(objBehavs => objBehavs.BehavioursPAId == item.BehavioursPAId).SingleOrDefault();
                                        behavior.BehaviourName = model.Dressing2;
                                        behavior.ModifiedBy = sess.LoginId;
                                        behavior.ModifiedOn = DateTime.Now;
                                        dbobj.SaveChanges();
                                    }
                                }
                            }

                            parentId = behavId("SKIN CARE/SKIN INTEGRITY");
                            flag = false;
                            foreach (var item in behave)
                            {
                                if (item.ParentId == parentId)
                                {
                                    if (!flag)
                                    {
                                        behavior = dbobj.BehavioursPAs.Where(objBehavs => objBehavs.BehavioursPAId == item.BehavioursPAId).SingleOrDefault();
                                        behavior.BehaviourName = model.SkinCare1;
                                        behavior.ModifiedBy = sess.LoginId;
                                        behavior.ModifiedOn = DateTime.Now;
                                        dbobj.SaveChanges();
                                        flag = true;
                                    }
                                    else
                                    {
                                        behavior = dbobj.BehavioursPAs.Where(objBehavs => objBehavs.BehavioursPAId == item.BehavioursPAId).SingleOrDefault();
                                        behavior.BehaviourName = model.SkinCare2;
                                        behavior.ModifiedBy = sess.LoginId;
                                        behavior.ModifiedOn = DateTime.Now;
                                        dbobj.SaveChanges();
                                    }
                                }
                            }

                            parentId = behavId("COMMUNICATION");
                            flag = false;

                            int isCom = 0;

                            foreach (var item in behave)
                            {
                                if (item.ParentId == parentId)
                                {
                                    isCom = 1;
                                    if (!flag)
                                    {
                                        behavior = dbobj.BehavioursPAs.Where(objBehavs => objBehavs.BehavioursPAId == item.BehavioursPAId).SingleOrDefault();
                                        behavior.BehaviourName = model.Communication1;
                                        behavior.ModifiedBy = sess.LoginId;
                                        behavior.ModifiedOn = DateTime.Now;
                                        dbobj.SaveChanges();
                                        flag = true;
                                    }
                                    else
                                    {
                                        behavior = dbobj.BehavioursPAs.Where(objBehavs => objBehavs.BehavioursPAId == item.BehavioursPAId).SingleOrDefault();
                                        behavior.BehaviourName = model.Communication2;
                                        behavior.ModifiedBy = sess.LoginId;
                                        behavior.ModifiedOn = DateTime.Now;
                                        dbobj.SaveChanges();
                                    }
                                }

                            }

                            if (isCom == 0)
                            {
                                BehavioursPA bpa = new BehavioursPA();

                                bpa.StudentPersonalId = sess.StudentId;
                                bpa.SchoolId = sess.SchoolId;
                                bpa.BehaviourName = model.Communication1;
                                bpa.ParentId = parentId;
                                bpa.CreatedBy = sess.LoginId;
                                bpa.CreatedOn = DateTime.Now;

                                dbobj.BehavioursPAs.Add(bpa);
                                dbobj.SaveChanges();
                            }

                            parentId = behavId("PREFERRED ACTIVITIES");
                            flag = false;
                            foreach (var item in behave)
                            {
                                if (item.ParentId == parentId)
                                {
                                    if (!flag)
                                    {
                                        behavior = dbobj.BehavioursPAs.Where(objBehavs => objBehavs.BehavioursPAId == item.BehavioursPAId).SingleOrDefault();
                                        behavior.BehaviourName = model.preferedActivities1;
                                        behavior.ModifiedBy = sess.LoginId;
                                        behavior.ModifiedOn = DateTime.Now;
                                        dbobj.SaveChanges();
                                        flag = true;
                                    }
                                    else
                                    {
                                        behavior = dbobj.BehavioursPAs.Where(objBehavs => objBehavs.BehavioursPAId == item.BehavioursPAId).SingleOrDefault();
                                        behavior.BehaviourName = model.preferedActivities2;
                                        behavior.ModifiedBy = sess.LoginId;
                                        behavior.ModifiedOn = DateTime.Now;
                                        dbobj.SaveChanges();
                                    }
                                }
                            }

                            parentId = behavId("GENERAL INFORMATION");
                            flag = false;
                            foreach (var item in behave)
                            {
                                if (item.ParentId == parentId)
                                {
                                    if (!flag)
                                    {
                                        behavior = dbobj.BehavioursPAs.Where(objBehavs => objBehavs.BehavioursPAId == item.BehavioursPAId).SingleOrDefault();
                                        behavior.BehaviourName = model.GeneralInformation1;
                                        behavior.ModifiedBy = sess.LoginId;
                                        behavior.ModifiedOn = DateTime.Now;
                                        dbobj.SaveChanges();
                                        flag = true;
                                    }
                                    else
                                    {
                                        behavior = dbobj.BehavioursPAs.Where(objBehavs => objBehavs.BehavioursPAId == item.BehavioursPAId).SingleOrDefault();
                                        behavior.BehaviourName = model.GeneralInformation2;
                                        behavior.ModifiedBy = sess.LoginId;
                                        behavior.ModifiedOn = DateTime.Now;
                                        dbobj.SaveChanges();
                                    }
                                }
                            }

                            parentId = behavId("SUGGESTED PROACTIVE ENVIRONMENTAL PROCEDURES");
                            flag = false;
                            foreach (var item in behave)
                            {
                                if (item.ParentId == parentId)
                                {
                                    if (!flag)
                                    {
                                        behavior = dbobj.BehavioursPAs.Where(objBehavs => objBehavs.BehavioursPAId == item.BehavioursPAId).SingleOrDefault();
                                        behavior.BehaviourName = model.SuggestedProactiveEnvironmentalProcedures1;
                                        behavior.ModifiedBy = sess.LoginId;
                                        behavior.ModifiedOn = DateTime.Now;
                                        dbobj.SaveChanges();
                                        flag = true;
                                    }
                                    else
                                    {
                                        behavior = dbobj.BehavioursPAs.Where(objBehavs => objBehavs.BehavioursPAId == item.BehavioursPAId).SingleOrDefault();
                                        behavior.BehaviourName = model.SuggestedProactiveEnvironmentalProcedures2;
                                        behavior.ModifiedBy = sess.LoginId;
                                        behavior.ModifiedOn = DateTime.Now;
                                        dbobj.SaveChanges();
                                    }
                                }
                            }
                        }
                        Result = "Sucess";
                        //SaveNewEventData(model.newEventLog, model.Id); //Commented for avoiding exception on null check
                        if (model.newEventLog != null)
                        {
                            SaveNewEventData(model.newEventLog, model.Id); //model.newEventlog null check
                        }
                        AddEventModel.CreateSystemEvent("Client Updated [" + model.LastName + ", " + model.FirstName + "]", "Client Updated", model.eventLogNote);
                    }
                    Result = "Sucess";

                    trans.Complete();
                }
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
                Result = "Failed";
            }
            return Result;
        }

        private void saveBehaviors(BehavioursPA behavior, clsSession sess, ClientRegistrationPAModel model)
        {

            int parentId = behavId("LIFTING / TRANSFERS");
            behavior.StudentPersonalId = sess.StudentId;
            behavior.SchoolId = sess.SchoolId;
            behavior.ParentId = parentId;
            behavior.BehaviourName = model.LiftingOrTransfers1;
            behavior.CreatedBy = sess.LoginId;
            behavior.CreatedOn = DateTime.Now;
            dbobj.BehavioursPAs.Add(behavior);
            dbobj.SaveChanges();
            behavior.StudentPersonalId = sess.StudentId;
            behavior.ParentId = parentId;
            behavior.SchoolId = sess.SchoolId;
            behavior.BehaviourName = model.LiftingOrTransfers2;
            behavior.CreatedBy = sess.LoginId;
            behavior.CreatedOn = DateTime.Now;
            dbobj.BehavioursPAs.Add(behavior);
            dbobj.SaveChanges();

            parentId = behavId("AMBULATION");
            behavior.StudentPersonalId = sess.StudentId;
            behavior.ParentId = parentId;
            behavior.SchoolId = sess.SchoolId;
            behavior.BehaviourName = model.Ambulation1;
            behavior.CreatedBy = sess.LoginId;
            behavior.CreatedOn = DateTime.Now;
            dbobj.BehavioursPAs.Add(behavior);
            dbobj.SaveChanges();
            behavior.StudentPersonalId = sess.StudentId;
            behavior.ParentId = parentId;
            behavior.SchoolId = sess.SchoolId;
            behavior.BehaviourName = model.Ambulation2;
            behavior.CreatedBy = sess.LoginId;
            behavior.CreatedOn = DateTime.Now;
            dbobj.BehavioursPAs.Add(behavior);
            dbobj.SaveChanges();

            parentId = behavId("TOILETING");
            behavior.StudentPersonalId = sess.StudentId;
            behavior.ParentId = parentId;
            behavior.SchoolId = sess.SchoolId;
            behavior.BehaviourName = model.Toileting1;
            behavior.CreatedBy = sess.LoginId;
            behavior.CreatedOn = DateTime.Now;
            dbobj.BehavioursPAs.Add(behavior);
            dbobj.SaveChanges();
            behavior.StudentPersonalId = sess.StudentId;
            behavior.ParentId = parentId;
            behavior.SchoolId = sess.SchoolId;
            behavior.BehaviourName = model.Toileting2;
            behavior.CreatedBy = sess.LoginId;
            behavior.CreatedOn = DateTime.Now;
            dbobj.BehavioursPAs.Add(behavior);
            dbobj.SaveChanges();

            parentId = behavId("EATING");
            behavior.StudentPersonalId = sess.StudentId;
            behavior.ParentId = parentId;
            behavior.SchoolId = sess.SchoolId;
            behavior.BehaviourName = model.Eating1;
            behavior.CreatedBy = sess.LoginId;
            behavior.CreatedOn = DateTime.Now;
            dbobj.BehavioursPAs.Add(behavior);
            dbobj.SaveChanges();
            behavior.StudentPersonalId = sess.StudentId;
            behavior.ParentId = parentId;
            behavior.SchoolId = sess.SchoolId;
            behavior.BehaviourName = model.Eating2;
            behavior.CreatedBy = sess.LoginId;
            behavior.CreatedOn = DateTime.Now;
            dbobj.BehavioursPAs.Add(behavior);
            dbobj.SaveChanges();

            parentId = behavId("SHOWERING");
            behavior.StudentPersonalId = sess.StudentId;
            behavior.ParentId = parentId;
            behavior.SchoolId = sess.SchoolId;
            behavior.BehaviourName = model.Showering1;
            behavior.CreatedBy = sess.LoginId;
            behavior.CreatedOn = DateTime.Now;
            dbobj.BehavioursPAs.Add(behavior);
            dbobj.SaveChanges();
            behavior.StudentPersonalId = sess.StudentId;
            behavior.ParentId = parentId;
            behavior.SchoolId = sess.SchoolId;
            behavior.BehaviourName = model.Showering2;
            behavior.CreatedBy = sess.LoginId;
            behavior.CreatedOn = DateTime.Now;
            dbobj.BehavioursPAs.Add(behavior);
            dbobj.SaveChanges();

            parentId = behavId("TOOTHBRUSHING");
            behavior.StudentPersonalId = sess.StudentId;
            behavior.ParentId = parentId;
            behavior.SchoolId = sess.SchoolId;
            behavior.BehaviourName = model.ToothBrushing1;
            behavior.CreatedBy = sess.LoginId;
            behavior.CreatedOn = DateTime.Now;
            dbobj.BehavioursPAs.Add(behavior);
            dbobj.SaveChanges();
            behavior.StudentPersonalId = sess.StudentId;
            behavior.ParentId = parentId;
            behavior.SchoolId = sess.SchoolId;
            behavior.BehaviourName = model.ToothBrushing2;
            behavior.CreatedBy = sess.LoginId;
            behavior.CreatedOn = DateTime.Now;
            dbobj.BehavioursPAs.Add(behavior);
            dbobj.SaveChanges();

            parentId = behavId("DRESSING");
            behavior.StudentPersonalId = sess.StudentId;
            behavior.ParentId = parentId;
            behavior.SchoolId = sess.SchoolId;
            behavior.BehaviourName = model.Dressing1;
            behavior.CreatedBy = sess.LoginId;
            behavior.CreatedOn = DateTime.Now;
            dbobj.BehavioursPAs.Add(behavior);
            dbobj.SaveChanges();
            behavior.StudentPersonalId = sess.StudentId;
            behavior.ParentId = parentId;
            behavior.SchoolId = sess.SchoolId;
            behavior.BehaviourName = model.Dressing2;
            behavior.CreatedBy = sess.LoginId;
            behavior.CreatedOn = DateTime.Now;
            dbobj.BehavioursPAs.Add(behavior);
            dbobj.SaveChanges();

            parentId = behavId("SKIN CARE/SKIN INTEGRITY");
            behavior.StudentPersonalId = sess.StudentId;
            behavior.ParentId = parentId;
            behavior.SchoolId = sess.SchoolId;
            behavior.BehaviourName = model.SkinCare1;
            behavior.CreatedBy = sess.LoginId;
            behavior.CreatedOn = DateTime.Now;
            dbobj.BehavioursPAs.Add(behavior);
            dbobj.SaveChanges();
            behavior.StudentPersonalId = sess.StudentId;
            behavior.ParentId = parentId;
            behavior.SchoolId = sess.SchoolId;
            behavior.BehaviourName = model.SkinCare2;
            behavior.CreatedBy = sess.LoginId;
            behavior.CreatedOn = DateTime.Now;
            dbobj.BehavioursPAs.Add(behavior);
            dbobj.SaveChanges();

            parentId = behavId("COMMUNICATION");
            behavior.StudentPersonalId = sess.StudentId;
            behavior.ParentId = parentId;
            behavior.SchoolId = sess.SchoolId;
            behavior.BehaviourName = model.Communication1;
            behavior.CreatedBy = sess.LoginId;
            behavior.CreatedOn = DateTime.Now;
            dbobj.BehavioursPAs.Add(behavior);
            dbobj.SaveChanges();
            behavior.StudentPersonalId = sess.StudentId;
            behavior.ParentId = parentId;
            behavior.SchoolId = sess.SchoolId;
            behavior.BehaviourName = model.Communication2;
            behavior.CreatedBy = sess.LoginId;
            behavior.CreatedOn = DateTime.Now;
            dbobj.BehavioursPAs.Add(behavior);
            dbobj.SaveChanges();


            parentId = behavId("PREFERRED ACTIVITIES");
            behavior.StudentPersonalId = sess.StudentId;
            behavior.ParentId = parentId;
            behavior.SchoolId = sess.SchoolId;
            behavior.BehaviourName = model.preferedActivities1;
            behavior.CreatedBy = sess.LoginId;
            behavior.CreatedOn = DateTime.Now;
            dbobj.BehavioursPAs.Add(behavior);
            dbobj.SaveChanges();
            behavior.StudentPersonalId = sess.StudentId;
            behavior.ParentId = parentId;
            behavior.SchoolId = sess.SchoolId;
            behavior.BehaviourName = model.preferedActivities2;
            behavior.CreatedBy = sess.LoginId;
            behavior.CreatedOn = DateTime.Now;
            dbobj.BehavioursPAs.Add(behavior);
            dbobj.SaveChanges();

            parentId = behavId("GENERAL INFORMATION");
            behavior.StudentPersonalId = sess.StudentId;
            behavior.ParentId = parentId;
            behavior.SchoolId = sess.SchoolId;
            behavior.BehaviourName = model.GeneralInformation1;
            behavior.CreatedBy = sess.LoginId;
            behavior.CreatedOn = DateTime.Now;
            dbobj.BehavioursPAs.Add(behavior);
            dbobj.SaveChanges();
            behavior.StudentPersonalId = sess.StudentId;
            behavior.ParentId = parentId;
            behavior.SchoolId = sess.SchoolId;
            behavior.BehaviourName = model.GeneralInformation2;
            behavior.CreatedBy = sess.LoginId;
            behavior.CreatedOn = DateTime.Now;
            dbobj.BehavioursPAs.Add(behavior);
            dbobj.SaveChanges();

            parentId = behavId("SUGGESTED PROACTIVE ENVIRONMENTAL PROCEDURES");
            behavior.StudentPersonalId = sess.StudentId;
            behavior.ParentId = parentId;
            behavior.SchoolId = sess.SchoolId;
            behavior.BehaviourName = model.SuggestedProactiveEnvironmentalProcedures1;
            behavior.CreatedBy = sess.LoginId;
            behavior.CreatedOn = DateTime.Now;
            dbobj.BehavioursPAs.Add(behavior);
            dbobj.SaveChanges();
            behavior.StudentPersonalId = sess.StudentId;
            behavior.ParentId = parentId;
            behavior.SchoolId = sess.SchoolId;
            behavior.BehaviourName = model.SuggestedProactiveEnvironmentalProcedures2;
            behavior.CreatedBy = sess.LoginId;
            behavior.CreatedOn = DateTime.Now;
            dbobj.BehavioursPAs.Add(behavior);
            dbobj.SaveChanges();

        }


        public int behavId(string parentname)
        {
            int parentId = 0;
            dbobj = new BiWeeklyRCPNewEntities();
            BehaveLookup behav = new BehaveLookup();
            behav = dbobj.BehaveLookups.Where(objBehave => objBehave.BehaviouralName == parentname).SingleOrDefault();
            if (behav != null) parentId = behav.BehaviouralId;
            else parentId = 0;
            return parentId;

        }





        /// <summary>
        /// Function for upload and Save Client Image.
        /// </summary>
        /// <param name="file"></param>
        /// <param name="model"></param>
        /// <returns></returns>

        public string SaveImage(HttpPostedFileBase file, ImageUploader model)
        {
            string result = "";

            string dirpath = AppDomain.CurrentDomain.BaseDirectory + "Images/StudentImages/";
            try
            {

                if (file != null)
                {
                    if (file.ContentLength < 512000)
                    {
                        string[] fileIesplit = file.FileName.Split('\\');
                        if (Directory.Exists(dirpath))
                        {


                            string[] temp = fileIesplit[fileIesplit.Length - 1].Split('.');

                            //    file.SaveAs(dirpath + "\\" + userSessionObj.id.ToString() + "-" + formData["filetype"] + "-1." + temp[temp.Length - 1]);


                        }
                        else
                        {
                            Directory.CreateDirectory(dirpath);

                            string[] temp = fileIesplit[fileIesplit.Length - 1].Split('.');


                            //    file.SaveAs(dirpath + "\\" + userSessionObj.id.ToString() + "-" + formData["filetype"] + "-1." + temp[temp.Length - 1]);

                        }

                    }
                    else
                    {
                        result = "fileError";
                    }


                }
                dbobj.SaveChanges();
                result = "Sucess";
            }
            catch
            {
                result = "failed";
            }

            return result;
        }



        /// <summary>
        /// Function To load the Client Data.
        /// </summary>
        /// <param name="StudentId"></param>
        /// <returns></returns>

        public RegistrationModel bindCliendData(int StudentId)
        {
            // sess = (ClsSession)HttpContext.Current.Session["UserSessionClient"];

            RegistrationModel regModel = new RegistrationModel();
            dbobj = new BiWeeklyRCPNewEntities();
            StudentPersonal student = new StudentPersonal();
            AddressList address = new AddressList();
            StudentAddresRel stdAddrRel = new StudentAddresRel();
            EmergencyContactSchool EmergencyContact = new EmergencyContactSchool();
            SchoolsAttended SchoolAttended = new SchoolsAttended();
            LookUp objLookUp = new LookUp();
            Insurance objInsurance = new Insurance();
            ContactPersonal contctPersonal = new ContactPersonal();

            try
            {
                student = dbobj.StudentPersonals.Where(x => x.StudentPersonalId == StudentId).SingleOrDefault();
                stdAddrRel = dbobj.StudentAddresRels.Where(x => x.StudentPersonalId == StudentId && x.ContactSequence == 0).SingleOrDefault();
                address = dbobj.AddressLists.Where(x => x.AddressId == stdAddrRel.AddressId && x.AddressType == 0).SingleOrDefault();

                var EmergencyContacts = dbobj.EmergencyContactSchools.Where(objEmergencyContact => objEmergencyContact.StudentPersonalId == StudentId).ToList();

                var SchoolsAttended = dbobj.SchoolsAttendeds.Where(objSchoolAttended => objSchoolAttended.StudentPersonalId == StudentId).ToList();

                var MedicalInsurance = dbobj.MedicalAndInsurances.Where(objMedicalAndInsurances => objMedicalAndInsurances.StudentPersonalId == StudentId).ToList();


                Int32 lookupId=0;
                var contactPersonalId=0;
                try
                {
                    objLookUp = dbobj.LookUps.Where(objlukUp => objlukUp.LookupName == "Educational Surrogate").SingleOrDefault();
                    lookupId = objLookUp.LookupId;
                }
                catch
                {
                    
                }
                


                try
                {
                    regModel.InsuranceList = dbobj.Insurances.Where(objInsur => objInsur.StudentPersonalId == StudentId && objInsur.PreferType == "Primary").ToList();


                    //regModel.InsuranceList = (from objI in objIns                                             
                    //                          select new Insurance
                    //                          {
                    //                              InsuranceType = objInsurance.InsuranceType,
                    //                              PolicyNumber = objInsurance.PolicyNumber,
                    //                              PolicyHolder = objInsurance.PolicyHolder
                    //                          }).ToList();


                    if (regModel.InsuranceList != null)
                    {
                        foreach (var item in regModel.InsuranceList)
                        {
                            regModel.InsuranceType = item.InsuranceType;
                            regModel.PolicyHolder = item.PolicyHolder;
                            regModel.PolicyNumber = item.PolicyNumber;
                        }

                    }
                }
                catch
                {

                }
                regModel.Id = student.StudentPersonalId;
                regModel.FirstName = student.FirstName;
                regModel.LastName = student.LastName;
                regModel.MiddleName = student.MiddleName;
                regModel.NickName = student.NickName;
                regModel.LastNameSuffix = student.Suffix;
                regModel.DateOfBirth = ConvertDate(student.BirthDate);
                regModel.PlaceOfBirth = student.PlaceOfBirth;
                regModel.Photodate = ConvertDate(student.Photodate);
                regModel.CountryofBirth = dbobj.LookUps.Where(objlukup => objlukup.LookupType == "Country" && objlukup.LookupCode == SetLookUpCode).Select(objlukup => objlukup.LookupId).Single(); //student.CountryOfBirth;
                regModel.StateOfBirth = student.StateOfBirth;
                string modifiedDate = "";
                DateTime modifieddate;
                try
                {
                    modifieddate = (DateTime)student.ModifiedOn;
                    modifiedDate = modifieddate.ToString("MM/yyyy").Replace('-', '/');
                }
                catch
                {
                    modifiedDate = null;
                }
                regModel.ModifiedDate = modifiedDate;
                regModel.UpdatedOn = ConvertDate(student.ModifiedOn);
                try
                {
                    objLookUp = dbobj.LookUps.Where(objlukUp => objlukUp.LookupCode == SetLookUpCode && objlukUp.LookupType == "Country").SingleOrDefault();
                    regModel.CountryBirth = objLookUp.LookupName;

                }
                catch
                {
                    regModel.CountryBirth = "";
                }
                try
                {
                    objLookUp = dbobj.LookUps.Where(objlukUp => objlukUp.LookupId == student.StateOfBirth && objlukUp.LookupType == "State").SingleOrDefault();
                    regModel.StateBirth = objLookUp.LookupName;
                }
                catch
                {
                    regModel.StateBirth = "";
                }
                regModel.Citizenship = Convert.ToInt32(student.CitizenshipStatus);
                regModel.Race = Convert.ToInt32(student.RaceId);
                regModel.Gender = student.Gender;
                regModel.HairColor = student.HairColor;
                regModel.EyeColor = student.EyeColor;
                regModel.AdmissinDate = ConvertDate(student.AdmissionDate);

                if (student.Height == null)
                {
                    regModel.Height = "";
                }
                else
                {
	                try
	                {
	                    //regModel.Height = Convert.ToInt32(student.Height).ToString();
	                    Decimal HeightD = Convert.ToDecimal(student.Height);
	                    regModel.Height = Convert.ToString(Math.Floor(HeightD));
	                }
	                catch
	                {
	                    regModel.Height = "";
	                } 
                }
				if (student.Weight == null)
                {
                    regModel.Weight = "";
                }
                else
                {
	                try
	                {
	                    //regModel.Weight = student.Weight.ToString();
	                    regModel.Weight = student.Weight.ToString().Split('.')[0]+'.'+student.Weight.ToString().Split('.')[1].Substring(0,1);
	                }
	                catch
	                {
	                    regModel.Weight = "";
	                }
                }

                regModel.PrimaryLanguage = student.PrimaryLanguage;
                regModel.GuardianshipStatus = student.GuardianShip;
                regModel.DistigushingMarks = student.DistingushingMarks;
                regModel.LegalCompetencyStatus = student.LegalCompetencyStatus;
                regModel.OtherStateAgenciesInvolvedWithStudent = student.OtherStateAgenciesInvolvedWithStudent;
                regModel.MaritalStatusofBothParents = student.MaritalStatusofBothParents;
                regModel.CaseManagerEducational = student.CaseManagerEducational;
                if (lookupId != null)
                {
                    try
                    {
                        var ContactPersonalId = dbobj.StudentContactRelationships.Where(x => x.RelationshipId == lookupId).SingleOrDefault();
                        var EducationalSurrogate = dbobj.ContactPersonals.Where(x => x.ContactPersonalId == ContactPersonalId.ContactPersonalId).SingleOrDefault();

                        if (EducationalSurrogate.MiddleName != null)
                            regModel.EducationalSurrogate = EducationalSurrogate.FirstName + ',' + EducationalSurrogate.MiddleName;
                        else
                            regModel.EducationalSurrogate = EducationalSurrogate.FirstName;
                    }
                    catch
                    {

                    }
                }
                regModel.CaseManagerResidential = student.CaseManagerResidential;
                regModel.ImageUrl = student.ImageUrl;
                regModel.PhotoReleasePermission = student.ImagePermission;
                regModel.DateInitiallyEligibleforSpecialEducation = ConvertDate(student.DateInitiallyEligibleforSpecialEducation);
                regModel.DateofMostRecentSpecialEducationEvaluations = ConvertDate(student.DateofMostRecentSpecialEducationEvaluations);
                regModel.DateofNextScheduled3YearEvaluation = ConvertDate(student.DateofNextScheduled3YearEvaluation);
                regModel.CurrentIEPStartDate = ConvertDate(student.CurrentIEPStartDate);
                regModel.CurrentIEPExpirationDate = ConvertDate(student.CurrentIEPExpirationDate);
                regModel.DischargeDate = ConvertDate(student.DischargeDate);
                regModel.LocationAfterDischarge = student.LocationAfterDischarge;
                regModel.MelmarkNewEnglandsFollowUpResponsibilities = student.MelmarkNewEnglandsFollowUpResponsibilities;
                regModel.ReferralIEPFullName = student.IEPReferralFullName;
                regModel.ReferralIEPPhone = student.IEPReferralPhone;
                regModel.ReferralIEPReferringAgency = student.IEPReferralReferrinAgency;
                regModel.ReferralIEPSourceofTuition = student.IEPReferralSourceofTuition;
                regModel.ReferralIEPTitle = student.IEPReferralTitle;
                if (address != null)
                {
                    regModel.AddressLine1 = address.ApartmentType;
                    regModel.AddressLine2 = address.StreetName;
                    regModel.AddressLine3 = address.AddressLine3;
                    regModel.ZipCode = address.PostalCode;
                    regModel.City = address.City;
                    regModel.studCounty = address.County;
                    regModel.Country = address.CountryId;
                    regModel.State = address.StateProvince;
                    try
                    {
                        objLookUp = dbobj.LookUps.Where(objlukUp => objlukUp.LookupId == address.CountryId && objlukUp.LookupType == "Country").SingleOrDefault();
                        regModel.StrCountry = objLookUp.LookupName;
                    }
                    catch
                    {
                        regModel.StrCountry = "";
                    }
                    try
                    {
                        objLookUp = dbobj.LookUps.Where(objlukUp => objlukUp.LookupId == address.StateProvince && objlukUp.LookupType == "State").SingleOrDefault();
                        regModel.StrState = objLookUp.LookupName;
                    }
                    catch
                    {
                        regModel.StrState = "";
                    }
                }

                if (EmergencyContacts != null)
                {
                    foreach (var item in EmergencyContacts)
                    {
                        if (item.SequenceId == 1)
                        {
                            regModel.EmergencyContactFirstName1 = item.FirstName;
                            regModel.EmergencyContactLastName1 = item.LastName;
                            regModel.EmergencyContactTitle1 = item.Title;
                            regModel.EmergencyContactPhone1 = item.Phone;
                        }
                        if (item.SequenceId == 2)
                        {
                            regModel.EmergencyContactFirstName2 = item.FirstName;
                            regModel.EmergencyContactLastName2 = item.LastName;
                            regModel.EmergencyContactTitle2 = item.Title;
                            regModel.EmergencyContactPhone2 = item.Phone;
                        }
                        if (item.SequenceId == 3)
                        {
                            regModel.EmergencyContactFirstName3 = item.FirstName;
                            regModel.EmergencyContactLastName3 = item.LastName;
                            regModel.EmergencyContactTitle3 = item.Title;
                            regModel.EmergencyContactPhone3 = item.Phone;
                        }
                        if (item.SequenceId == 4)
                        {
                            regModel.EmergencyContactFirstName4 = item.FirstName;
                            regModel.EmergencyContactLastName4 = item.LastName;
                            regModel.EmergencyContactTitle4 = item.Title;
                            regModel.EmergencyContactPhone4 = item.Phone;
                        }
                        if (item.SequenceId == 5)
                        {
                            regModel.EmergencyContactFirstName5 = item.FirstName;
                            regModel.EmergencyContactLastName5 = item.LastName;
                            regModel.EmergencyContactTitle5 = item.Title;
                            regModel.EmergencyContactPhone5 = item.Phone;
                        }
                    }
                }
                if (SchoolsAttended != null)
                {
                    foreach (var item in SchoolsAttended)
                    {
                        if (item.SequenceId == 1)
                        {
                            regModel.SchoolName1 = item.SchoolName;
                            regModel.DateFrom1 = ConvertDate(item.DateFrom);
                            regModel.DateTo1 = ConvertDate(item.DateTo);
                            regModel.SchoolAttendedAddress11 = item.Address1;
                            regModel.SchoolAttendedAddress21 = item.Address2;
                            regModel.SchoolAttendedCity1 = item.City;
                            try
                            {
                                int state1 = Convert.ToInt32(item.State);
                                objLookUp = dbobj.LookUps.Where(objlukUp => objlukUp.LookupId == state1 && objlukUp.LookupType == "State").SingleOrDefault();
                                regModel.SchoolAttendedState1 = objLookUp.LookupName;
                                regModel.intSchoolAttendedState1 = Convert.ToInt32(item.State);
                            }
                            catch
                            {
                                regModel.SchoolAttendedState1 = "";
                            }
                        }
                        if (item.SequenceId == 2)
                        {
                            regModel.SchoolName2 = item.SchoolName;
                            regModel.DateFrom2 = ConvertDate(item.DateFrom);
                            regModel.DateTo2 = ConvertDate(item.DateTo);
                            regModel.SchoolAttendedAddress12 = item.Address1;
                            regModel.SchoolAttendedAddress22 = item.Address2;
                            regModel.SchoolAttendedCity2 = item.City;
                            try
                            {
                                int state2 = Convert.ToInt32(item.State);
                                objLookUp = dbobj.LookUps.Where(objlukUp => objlukUp.LookupId == state2 && objlukUp.LookupType == "State").SingleOrDefault();
                                regModel.SchoolAttendedState2 = objLookUp.LookupName;
                                regModel.intSchoolAttendedState2 = Convert.ToInt32(item.State);
                            }
                            catch
                            {
                                regModel.SchoolAttendedState2 = "";
                            }
                        }
                        if (item.SequenceId == 3)
                        {
                            regModel.SchoolName3 = item.SchoolName;
                            regModel.DateFrom3 = ConvertDate(item.DateFrom);
                            regModel.DateTo3 = ConvertDate(item.DateTo);
                            regModel.SchoolAttendedAddress13 = item.Address1;
                            regModel.SchoolAttendedAddress23 = item.Address2;
                            regModel.SchoolAttendedCity3 = item.City;
                            try
                            {
                                int state3 = Convert.ToInt32(item.State);
                                objLookUp = dbobj.LookUps.Where(objlukUp => objlukUp.LookupId == state3 && objlukUp.LookupType == "State").SingleOrDefault();
                                regModel.SchoolAttendedState3 = objLookUp.LookupName;
                                regModel.intSchoolAttendedState3 = Convert.ToInt32(item.State);
                            }
                            catch
                            {
                                regModel.SchoolAttendedState3 = "";
                            }
                        }
                    }
                }

                if (MedicalInsurance != null)
                {
                    foreach (var item in MedicalInsurance)
                    {
                        if (item.LastName == null)
                        {
                            regModel.PrimaryPhysicianName = item.FirstName;
                        }
                        else
                        {
                            regModel.PrimaryPhysicianName = item.LastName+','+item.FirstName;
                        }
                        regModel.PrimaryPhysicianPhone = item.OfficePhone;
                        var AddressList = dbobj.AddressLists.Where(objAddressLists => objAddressLists.AddressId == item.AddressId).SingleOrDefault();
                        if (AddressList != null)
                        {
                            if (AddressList.StreetName != null)
                                regModel.PrimaryPhysicianAddress = AddressList.StreetName + ',';
                            if (AddressList.ApartmentType != null)
                                regModel.PrimaryPhysicianAddress = regModel.PrimaryPhysicianAddress+AddressList.ApartmentType + ',';
                            if (AddressList.City != null)
                                regModel.PrimaryPhysicianAddress = regModel.PrimaryPhysicianAddress+AddressList.City + ',';
                            if (AddressList.PostalCode != null)
                                regModel.PrimaryPhysicianAddress = regModel.PrimaryPhysicianAddress+AddressList.PostalCode + ',';
                        }

                        var StudentPersonalPA = dbobj.StudentPersonalPAs.Where(objStudentPersonalPAs => objStudentPersonalPAs.StudentPersonalId == StudentId).SingleOrDefault();
                        var DiaganosesPA = dbobj.DiaganosesPAs.Where(objDiaganosesPAs => objDiaganosesPAs.StudentPersonalId == StudentId).SingleOrDefault();

                        regModel.DateOfLastPhysicalExam = ConvertDate(item.DateOfLastPhysicalExam);
                        regModel.MedicalConditionOrDiagnosis = DiaganosesPA.Diaganoses;
                        regModel.Allergies = StudentPersonalPA.Allergies;
                        regModel.CurrentMedications = item.CurrentMedications;
                        regModel.SelfPreservationAbility = item.SelfPreservationAbility;
                        regModel.SignificantBehaviorCharacteristics = item.SignificantBehaviorCharacteristics;
                        regModel.Capabilities = item.Capabilities;
                        regModel.Limitations = item.Limitations;
                        regModel.Preferences = item.Preferances;
                    }
                }
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
            }

            return regModel;
        }



        /// <summary>
        /// Function To load the PA Client Data.
        /// </summary>
        /// <param name="StudentId"></param>
        /// <returns></returns>
        public ClientRegistrationPAModel bindCliendDataPA(int ClientId)
        {
            // sess = (ClsSession)HttpContext.Current.Session["UserSessionClient"];

            ClientRegistrationPAModel regModel = new ClientRegistrationPAModel();
            dbobj = new BiWeeklyRCPNewEntities();
            StudentPersonal student = new StudentPersonal();
            StudentPersonalPA studentPA = new StudentPersonalPA();
            AddressList address = new AddressList();
            StudentAddresRel stdAddrRel = new StudentAddresRel();
            EmergencyContactSchool EmergencyContact = new EmergencyContactSchool();
            SchoolsAttended SchoolAttended = new SchoolsAttended();
            LookUp objLookUp = new LookUp();
            sess = (clsSession)HttpContext.Current.Session["UserSessionClient"];
            Insurance objInsurance = new Insurance();
            try
            {
                student = dbobj.StudentPersonals.Where(x => x.StudentPersonalId == ClientId && x.SchoolId == sess.SchoolId).SingleOrDefault();
                studentPA = dbobj.StudentPersonalPAs.Where(objStudentPA => objStudentPA.StudentPersonalId == ClientId && objStudentPA.SchoolId == sess.SchoolId).SingleOrDefault();
                stdAddrRel = dbobj.StudentAddresRels.Where(x => x.StudentPersonalId == ClientId && x.ContactSequence == 0).SingleOrDefault();
                //AddressType is 0 for MelmarkPA (check for NE)
                address = dbobj.AddressLists.Where(x => x.AddressId == stdAddrRel.AddressId).SingleOrDefault();
                var EmergencyContacts = dbobj.EmergencyContactSchools.Where(objEmergencyContact => objEmergencyContact.StudentPersonalId == ClientId).ToList();
                var SchoolsAttended = dbobj.SchoolsAttendeds.Where(objSchoolAttended => objSchoolAttended.StudentPersonalId == ClientId).ToList();

                try
                {
                    regModel.InsuranceList = dbobj.Insurances.Where(objInsur => objInsur.StudentPersonalId == ClientId && objInsur.PreferType == "Primary").ToList();


                    //regModel.InsuranceList = (from objI in objIns                                             
                    //                          select new Insurance
                    //                          {
                    //                              InsuranceType = objInsurance.InsuranceType,
                    //                              PolicyNumber = objInsurance.PolicyNumber,
                    //                              PolicyHolder = objInsurance.PolicyHolder
                    //                          }).ToList();


                    if (regModel.InsuranceList != null)
                    {
                        foreach (var item in regModel.InsuranceList)
                        {
                            regModel.InsuranceType = item.InsuranceType;
                            regModel.PolicyHolder = item.PolicyHolder;
                            regModel.PolicyNumber = item.PolicyNumber;
                        }

                    }

                    regModel.InsuranceList = dbobj.Insurances.Where(objInsur => objInsur.StudentPersonalId == ClientId && objInsur.PreferType == "Secondary").ToList();

                    if (regModel.InsuranceList != null)
                    {
                        foreach (var item in regModel.InsuranceList)
                        {
                            regModel.InsuranceType1 = item.InsuranceType;
                            regModel.PolicyHolder1 = item.PolicyHolder;
                            regModel.PolicyNumber1 = item.PolicyNumber;
                        }

                    }
                }
                catch
                {

                }

                var clntFunder = (from objContactPersonal in dbobj.ContactPersonals
                                  join objStudentAddresRel in dbobj.StudentAddresRels on objContactPersonal.ContactPersonalId equals objStudentAddresRel.ContactPersonalId
                                  where objStudentAddresRel.ContactSequence == 2 && objContactPersonal.IsBilling == true && objStudentAddresRel.StudentPersonalId == ClientId
                                  && objContactPersonal.Status == 1
                                  select new
                                  {
                                      Employer = objContactPersonal.Employer

                                  }).Distinct().ToList();

                var fundingSource = dbobj.StudentPersonalPAs.Where(x => x.StudentPersonalId == ClientId).SingleOrDefault();

                regModel.FunderListString = "";
                if (fundingSource.FundingSource != null)
                {
                    if (fundingSource.FundingSource != "")
                    {
                        regModel.FunderListString = fundingSource.FundingSource + ";";
                    }
                }
                if (clntFunder.Count > 0)
                {
                    foreach (var item in clntFunder)
                    {
                        if (fundingSource.FundingSource != null)
                        {
                            if (item.Employer != fundingSource.FundingSource)
                            {

                                regModel.FunderListString += (item.Employer != null && item.Employer != "") ? item.Employer + ";" : "";

                            }

                        }
                        else
                        {
                            regModel.FunderListString += (item.Employer != null && item.Employer != "") ? item.Employer + ";" : "";
                        }
                    }



                }

                if (regModel.FunderListString != "")
                {
                    regModel.FunderListString = regModel.FunderListString.Substring(0, regModel.FunderListString.Length - 1);
                }

                regModel.FunderListString.TrimEnd(';');


                // var behave = dbobj.BehavioursPAs.Where(objBehave => objBehave.StudentPersonalId == sess.ClientId && objBehave.SchoolId == sess.SchoolId).ToList();
                //var diagonis = dbobj.DiaganosesPAs.Where(objDiagno => objDiagno.StudentPersonalId == ClientId && objDiagno.SchoolId == sess.SchoolId).ToList();
                var diagonis = dbobj.DiaganosesPAs.Where(objDiagno => objDiagno.StudentPersonalId == ClientId).ToList();
                var adaptive = dbobj.AdaptiveEquipments.Where(objAdaptive => objAdaptive.StudentPersonalId == ClientId && objAdaptive.SchoolId == sess.SchoolId).ToList();
                var basicbehav = dbobj.BasicBehavioralInformations.Where(objBasic => objBasic.StudentPersonalId == ClientId && objBasic.SchoolId == sess.SchoolId).ToList();
                var classifications = (from sp in dbobj.StudentPersonals
                                       where
                                         sp.StudentPersonalId == sess.StudentId
                                       select new
                                       {
                                           sp.StudentPersonalId,
                                           clas1 =
                                             ((from lp in dbobj.LookUps
                                               where
                                                 lp.LookupId == sp.Classification1
                                               select new
                                               {
                                                   lp.LookupName
                                               }).FirstOrDefault().LookupName),
                                           clas2 =
                                             ((from lp in dbobj.LookUps
                                               where
                                                 lp.LookupId == sp.Classification2
                                               select new
                                               {
                                                   lp.LookupName
                                               }).FirstOrDefault().LookupName),
                                           clas3 =
                                             ((from lp in dbobj.LookUps
                                               where
                                                 lp.LookupId == sp.Classification3
                                               select new
                                               {
                                                   lp.LookupName
                                               }).FirstOrDefault().LookupName),
                                           clas4 =
                                             ((from lp in dbobj.LookUps
                                               where
                                                 lp.LookupId == sp.Classification4
                                               select new
                                               {
                                                   lp.LookupName
                                               }).FirstOrDefault().LookupName),
                                           clas5 =
                                             ((from lp in dbobj.LookUps
                                               where
                                                 lp.LookupId == sp.Classification5
                                               select new
                                               {
                                                   lp.LookupName
                                               }).FirstOrDefault().LookupName)
                                       }).ToList();

                if (classifications.Count > 0)
                {
                    regModel.ClassificationName1 = classifications[0].clas1;
                    regModel.ClassificationName2 = classifications[0].clas2;
                    regModel.ClassificationName3 = classifications[0].clas3;
                    regModel.ClassificationName4 = classifications[0].clas4;
                    regModel.ClassificationName5 = classifications[0].clas5;

                }

                //===== List 6 - Task #2                
                var staffpositions = (from sp in dbobj.StudentPersonals
                                      where sp.StudentPersonalId == sess.StudentId
                                      select new
                                      {
                                          sp.PositionStaff1,
                                          sp.PositionStaff2,
                                          sp.PositionStaff3,
                                          sp.PositionStaff4,
                                          sp.PositionStaff5,
                                          sp.PositionStaff6,
                                          sp.PositionStaff7,
                                          sp.PositionStaff8,
                                          sp.PositionStaff9,
                                          sp.PositionStaff10
                                      }).ToList();

                if (staffpositions.Count > 0)
                {
                    // Position 1 --
                    #region --Position 1
                    if (staffpositions[0].PositionStaff1 != null)
                    {
                        string[] strTeacherInstructor = staffpositions[0].PositionStaff1.Split(',');
                        int TeacherInstructorID = 0;
                        int TeacherInstructorStaffID = 0;

                        if (strTeacherInstructor.Length > 0 && staffpositions[0].PositionStaff1.Contains(","))
                        {
                            string TchIns = strTeacherInstructor[0].Replace("P", "");
                            string TchInsStf = strTeacherInstructor[1].Replace("S", "");
                            bool isNumericalTeacherInstructorID = int.TryParse(TchIns, out TeacherInstructorID);
                            bool isNumericalTeacherInstructorStaffID = int.TryParse(TchInsStf, out TeacherInstructorStaffID);
                            if (isNumericalTeacherInstructorID == true && isNumericalTeacherInstructorStaffID == true)
                            {
                                TeacherInstructorID = TchIns != null ? Convert.ToInt32(TchIns) : 0;
                                TeacherInstructorStaffID = TchInsStf != null ? Convert.ToInt32(TchInsStf) : 0;
                                if (TeacherInstructorID > 0 && TeacherInstructorStaffID > 0)
                                {
                                    //regModel.StaffPosition1 = TeacherInstructorID;
                                    regModel.Position1Staff = TeacherInstructorStaffID;
                                }
                                else
                                {
                                    //regModel.StaffPosition1 = 0;
                                    regModel.Position1Staff = 0;
                                }
                            }
                            else
                            {
                                //regModel.StaffPosition1 = 0;
                                regModel.Position1Staff = 0;
                            }
                        }
                        else
                        {
                            //regModel.StaffPosition1 = 0;
                            regModel.Position1Staff = 0;
                        }
                    }
                    else
                    {
                        //regModel.StaffPosition1 = 0;
                        regModel.Position1Staff = 0;
                    }
                    #endregion

                    //Position 2 -- 
                    #region --Position 2
                    if (staffpositions[0].PositionStaff2 != null)
                    {
                        string[] strProgramSpecialist = staffpositions[0].PositionStaff2.Split(',');
                        int ProgramSpecialistID = 0;
                        int ProgramSpecialistStaffID = 0;

                        if (strProgramSpecialist.Length > 0 && staffpositions[0].PositionStaff2.Contains(","))
                        {
                            string PgmSpc = strProgramSpecialist[0].Replace("P", "");
                            string PgmSpcStf = strProgramSpecialist[1].Replace("S", "");
                            bool isNumericalProgramSpecialistID = int.TryParse(PgmSpc, out ProgramSpecialistID);
                            bool isNumericalProgramSpecialistStaffID = int.TryParse(PgmSpcStf, out ProgramSpecialistStaffID);
                            if (isNumericalProgramSpecialistID == true && isNumericalProgramSpecialistStaffID == true)
                            {
                                ProgramSpecialistID = PgmSpc != null ? Convert.ToInt32(PgmSpc) : 0;
                                ProgramSpecialistStaffID = PgmSpcStf != null ? Convert.ToInt32(PgmSpcStf) : 0;
                                if (ProgramSpecialistID > 0 && ProgramSpecialistStaffID > 0)
                                {
                                    //regModel.StaffPosition2 = ProgramSpecialistID;
                                    regModel.Position2Staff = ProgramSpecialistStaffID;
                                }
                                else
                                {
                                    //regModel.StaffPosition2 = 0;
                                    regModel.Position2Staff = 0;
                                }
                            }
                            else
                            {
                                //regModel.StaffPosition2 = 0;
                                regModel.Position2Staff = 0;
                            }
                        }
                        else
                        {
                            //regModel.StaffPosition2 = 0;
                            regModel.Position2Staff = 0;
                        }
                    }
                    else
                    {
                        //regModel.StaffPosition2 = 0;
                        regModel.Position2Staff = 0;
                    }
                    #endregion

                    //Position 3 -- 
                    #region --Position 3
                    if (staffpositions[0].PositionStaff3 != null)
                    {
                        string[] strEDUBehaviorAnalyst = staffpositions[0].PositionStaff3.Split(',');
                        int EDUBehaviorAnalystID = 0;
                        int EDUBehaviorAnalystStaffID = 0;

                        if (strEDUBehaviorAnalyst.Length > 0 && staffpositions[0].PositionStaff3.Contains(","))
                        {
                            string EDUBehAny = strEDUBehaviorAnalyst[0].Replace("P", "");
                            string EDUBehAnyStf = strEDUBehaviorAnalyst[1].Replace("S", "");
                            bool isNumericalEDUBehaviorAnalystID = int.TryParse(EDUBehAny, out EDUBehaviorAnalystID);
                            bool isNumericalEDUBehaviorAnalystStaffID = int.TryParse(EDUBehAnyStf, out EDUBehaviorAnalystStaffID);
                            if (isNumericalEDUBehaviorAnalystID == true && isNumericalEDUBehaviorAnalystStaffID == true)
                            {
                                EDUBehaviorAnalystID = EDUBehAny != null ? Convert.ToInt32(EDUBehAny) : 0;
                                EDUBehaviorAnalystStaffID = EDUBehAnyStf != null ? Convert.ToInt32(EDUBehAnyStf) : 0;
                                if (EDUBehaviorAnalystID > 0 && EDUBehaviorAnalystStaffID > 0)
                                {
                                    //regModel.StaffPosition3 = EDUBehaviorAnalystID;
                                    regModel.Position3Staff = EDUBehaviorAnalystStaffID;
                                }
                                else
                                {
                                    //regModel.StaffPosition3 = 0;
                                    regModel.Position3Staff = 0;
                                }
                            }
                            else
                            {
                                //regModel.StaffPosition3 = 0;
                                regModel.Position3Staff = 0;
                            }
                        }
                        else
                        {
                            //regModel.StaffPosition3 = 0;
                            regModel.Position3Staff = 0;
                        }
                    }
                    else
                    {
                        //regModel.StaffPosition3 = 0;
                        regModel.Position3Staff = 0;
                    }
                    #endregion


                    //Position 4 -- 
                    #region --Position 4
                    if (staffpositions[0].PositionStaff4 != null)
                    {
                        string[] strCurriculumCoordinator = staffpositions[0].PositionStaff4.Split(',');
                        int CurriculumCoordinatorID = 0;
                        int CurriculumCoordinatorStaffID = 0;

                        if (strCurriculumCoordinator.Length > 0 && staffpositions[0].PositionStaff4.Contains(","))
                        {
                            string CurCor = strCurriculumCoordinator[0].Replace("P", "");
                            string CurCorStf = strCurriculumCoordinator[1].Replace("S", "");
                            bool isNumericalCurriculumCoordinatorID = int.TryParse(CurCor, out CurriculumCoordinatorID);
                            bool isNumericalCurriculumCoordinatorStaffID = int.TryParse(CurCorStf, out CurriculumCoordinatorStaffID);
                            if (isNumericalCurriculumCoordinatorID == true && isNumericalCurriculumCoordinatorStaffID == true)
                            {
                                CurriculumCoordinatorID = CurCor != null ? Convert.ToInt32(CurCor) : 0;
                                CurriculumCoordinatorStaffID = CurCorStf != null ? Convert.ToInt32(CurCorStf) : 0;
                                if (CurriculumCoordinatorID > 0 && CurriculumCoordinatorStaffID > 0)
                                {
                                    //regModel.StaffPosition4 = CurriculumCoordinatorID;
                                    regModel.Position4Staff = CurriculumCoordinatorStaffID;
                                }
                                else
                                {
                                    //regModel.StaffPosition4 = 0;
                                    regModel.Position4Staff = 0;
                                }
                            }
                            else
                            {
                                //regModel.StaffPosition4 = 0;
                                regModel.Position4Staff = 0;
                            }
                        }
                        else
                        {
                            //regModel.StaffPosition4 = 0;
                            regModel.Position4Staff = 0;
                        }
                    }
                    else
                    {
                        //regModel.StaffPosition4 = 0;
                        regModel.Position4Staff = 0;
                    }
                    #endregion


                    //Position 5 -- 
                    #region --Position 5
                    if (staffpositions[0].PositionStaff5 != null)
                    {
                        string[] strResidentialProgram = staffpositions[0].PositionStaff5.Split(',');
                        int ResidentialProgramID = 0;
                        int ResidentialProgramStaffID = 0;

                        if (strResidentialProgram.Length > 0 && staffpositions[0].PositionStaff5.Contains(","))
                        {
                            string ResPrg = strResidentialProgram[0].Replace("P", "");
                            string ResPrgStf = strResidentialProgram[1].Replace("S", "");
                            bool isNumericalResidentialProgramID = int.TryParse(ResPrg, out ResidentialProgramID);
                            bool isNumericalResidentialProgramStaffID = int.TryParse(ResPrgStf, out ResidentialProgramStaffID);
                            if (isNumericalResidentialProgramID == true && isNumericalResidentialProgramStaffID == true)
                            {
                                ResidentialProgramID = ResPrg != null ? Convert.ToInt32(ResPrg) : 0;
                                ResidentialProgramStaffID = ResPrgStf != null ? Convert.ToInt32(ResPrgStf) : 0;
                                if (ResidentialProgramID > 0 && ResidentialProgramStaffID > 0)
                                {
                                    //regModel.StaffPosition5 = ResidentialProgramID;
                                    regModel.Position5Staff = ResidentialProgramStaffID;
                                }
                                else
                                {
                                    //regModel.StaffPosition5 = 0;
                                    regModel.Position5Staff = 0;
                                }
                            }
                            else
                            {
                                //regModel.StaffPosition5 = 0;
                                regModel.Position5Staff = 0;
                            }
                        }
                        else
                        {
                            //regModel.StaffPosition5 = 0;
                            regModel.Position5Staff = 0;
                        }
                    }
                    else
                    {
                        //regModel.StaffPosition5 = 0;
                        regModel.Position5Staff = 0;
                    }
                    #endregion


                    //Position 6 -- 
                    #region --Position 6
                    if (staffpositions[0].PositionStaff6 != null)
                    {
                        string[] strProgramManagerQMRP = staffpositions[0].PositionStaff6.Split(',');
                        int ProgramManagerQMRPID = 0;
                        int ProgramManagerQMRPStaffID = 0;

                        if (strProgramManagerQMRP.Length > 0 && staffpositions[0].PositionStaff6.Contains(","))
                        {
                            string PmQmrp = strProgramManagerQMRP[0].Replace("P", "");
                            string PmQmrpStf = strProgramManagerQMRP[1].Replace("S", "");
                            bool isNumericalProgramManagerQMRPID = int.TryParse(PmQmrp, out ProgramManagerQMRPID);
                            bool isNumericalProgramManagerQMRPStaffID = int.TryParse(PmQmrpStf, out ProgramManagerQMRPStaffID);
                            if (isNumericalProgramManagerQMRPID == true && isNumericalProgramManagerQMRPStaffID == true)
                            {
                                ProgramManagerQMRPID = PmQmrp != null ? Convert.ToInt32(PmQmrp) : 0;
                                ProgramManagerQMRPStaffID = PmQmrpStf != null ? Convert.ToInt32(PmQmrpStf) : 0;
                                if (ProgramManagerQMRPID > 0 && ProgramManagerQMRPStaffID > 0)
                                {
                                    //regModel.StaffPosition6 = ProgramManagerQMRPID;
                                    regModel.Position6Staff = ProgramManagerQMRPStaffID;
                                }
                                else
                                {
                                    //regModel.StaffPosition6 = 0;
                                    regModel.Position6Staff = 0;
                                }
                            }
                            else
                            {
                                //regModel.StaffPosition6 = 0;
                                regModel.Position6Staff = 0;
                            }
                        }
                        else
                        {
                            //regModel.StaffPosition6 = 0;
                            regModel.Position6Staff = 0;
                        }
                    }
                    else
                    {
                        //regModel.StaffPosition6 = 0;
                        regModel.Position6Staff = 0;
                    }
                    #endregion


                    //Position 7 -- 
                    #region --Position 7
                    if (staffpositions[0].PositionStaff7 != null)
                    {
                        string[] strHouseSupervisor = staffpositions[0].PositionStaff7.Split(',');
                        int HouseSupervisorID = 0;
                        int HouseSupervisorStaffID = 0;

                        if (strHouseSupervisor.Length > 0 && staffpositions[0].PositionStaff7.Contains(","))
                        {
                            string HouSpvsr = strHouseSupervisor[0].Replace("P", "");
                            string HouSpvsrStf = strHouseSupervisor[1].Replace("S", "");
                            bool isNumericalHouseSupervisorID = int.TryParse(HouSpvsr, out HouseSupervisorID);
                            bool isNumericalHouseSupervisorStaffID = int.TryParse(HouSpvsrStf, out HouseSupervisorStaffID);
                            if (isNumericalHouseSupervisorID == true && isNumericalHouseSupervisorStaffID == true)
                            {
                                HouseSupervisorID = HouSpvsr != null ? Convert.ToInt32(HouSpvsr) : 0;
                                HouseSupervisorStaffID = HouSpvsrStf != null ? Convert.ToInt32(HouSpvsrStf) : 0;
                                if (HouseSupervisorID > 0 && HouseSupervisorStaffID > 0)
                                {
                                    //regModel.StaffPosition7 = HouseSupervisorID;
                                    regModel.Position7Staff = HouseSupervisorStaffID;
                                }
                                else
                                {
                                    //regModel.StaffPosition7 = 0;
                                    regModel.Position7Staff = 0;
                                }
                            }
                            else
                            {
                                //regModel.StaffPosition7 = 0;
                                regModel.Position7Staff = 0;
                            }
                        }
                        else
                        {
                            //regModel.StaffPosition7 = 0;
                            regModel.Position7Staff = 0;
                        }
                    }
                    else
                    {
                        //regModel.StaffPosition7 = 0;
                        regModel.Position7Staff = 0;
                    }
                    #endregion


                    //Position 8 -- 
                    #region --Position 8
                    if (staffpositions[0].PositionStaff8 != null)
                    {
                        string[] strResidentialBehaviorAnalyst = staffpositions[0].PositionStaff8.Split(',');
                        int ResidentialBehaviorAnalystID = 0;
                        int ResidentialBehaviorAnalystStaffID = 0;

                        if (strResidentialBehaviorAnalyst.Length > 0 && staffpositions[0].PositionStaff8.Contains(","))
                        {
                            string ResBany = strResidentialBehaviorAnalyst[0].Replace("P", "");
                            string ResBanyStf = strResidentialBehaviorAnalyst[1].Replace("S", "");
                            bool isNumericalResidentialBehaviorAnalystID = int.TryParse(ResBany, out ResidentialBehaviorAnalystID);
                            bool isNumericalResidentialBehaviorAnalystStaffID = int.TryParse(ResBanyStf, out ResidentialBehaviorAnalystStaffID);
                            if (isNumericalResidentialBehaviorAnalystID == true && isNumericalResidentialBehaviorAnalystStaffID == true)
                            {
                                ResidentialBehaviorAnalystID = ResBany != null ? Convert.ToInt32(ResBany) : 0;
                                ResidentialBehaviorAnalystStaffID = ResBanyStf != null ? Convert.ToInt32(ResBanyStf) : 0;
                                if (ResidentialBehaviorAnalystID > 0 && ResidentialBehaviorAnalystStaffID > 0)
                                {
                                    //regModel.StaffPosition8 = ResidentialBehaviorAnalystID;
                                    regModel.Position8Staff = ResidentialBehaviorAnalystStaffID;
                                }
                                else
                                {
                                    //regModel.StaffPosition8 = 0;
                                    regModel.Position8Staff = 0;
                                }
                            }
                            else
                            {
                                //regModel.StaffPosition8 = 0;
                                regModel.Position8Staff = 0;
                            }
                        }
                        else
                        {
                            //regModel.StaffPosition8 = 0;
                            regModel.Position8Staff = 0;
                        }
                    }
                    else
                    {
                        //regModel.StaffPosition8 = 0;
                        regModel.Position8Staff = 0;
                    }
                    #endregion


                    //Position 9 -- 
                    #region --Position 9
                    if (staffpositions[0].PositionStaff9 != null)
                    {
                        string[] strPrimaryNurse = staffpositions[0].PositionStaff9.Split(',');
                        int PrimaryNurseID = 0;
                        int PrimaryNurseStaffID = 0;

                        if (strPrimaryNurse.Length > 0 && staffpositions[0].PositionStaff9.Contains(","))
                        {
                            string PriNur = strPrimaryNurse[0].Replace("P", "");
                            string PriNurStf = strPrimaryNurse[1].Replace("S", "");
                            bool isNumericalPrimaryNurseID = int.TryParse(PriNur, out PrimaryNurseID);
                            bool isNumericalPrimaryNurseStaffID = int.TryParse(PriNurStf, out PrimaryNurseStaffID);
                            if (isNumericalPrimaryNurseID == true && isNumericalPrimaryNurseStaffID == true)
                            {
                                PrimaryNurseID = PriNur != null ? Convert.ToInt32(PriNur) : 0;
                                PrimaryNurseStaffID = PriNurStf != null ? Convert.ToInt32(PriNurStf) : 0;
                                if (PrimaryNurseID > 0 && PrimaryNurseStaffID > 0)
                                {
                                    //regModel.StaffPosition9 = PrimaryNurseID;
                                    regModel.Position9Staff = PrimaryNurseStaffID;
                                }
                                else
                                {
                                    //regModel.StaffPosition9 = 0;
                                    regModel.Position9Staff = 0;
                                }
                            }
                            else
                            {
                                //regModel.StaffPosition9 = 0;
                                regModel.Position9Staff = 0;
                            }
                        }
                        else
                        {
                            //regModel.StaffPosition9 = 0;
                            regModel.Position9Staff = 0;
                        }
                    }
                    else
                    {
                        //regModel.StaffPosition9 = 0;
                        regModel.Position9Staff = 0;
                    }
                    #endregion


                    //Position 10 -- 
                    #region --Position 10
                    if (staffpositions[0].PositionStaff10 != null)
                    {
                        string[] strUnitClerk = staffpositions[0].PositionStaff10.Split(',');
                        int UnitClerkID = 0;
                        int UnitClerkStaffID = 0;

                        if (strUnitClerk.Length > 0 && staffpositions[0].PositionStaff10.Contains(","))
                        {
                            string UniClk = strUnitClerk[0].Replace("P", "");
                            string UniClkStf = strUnitClerk[1].Replace("S", "");
                            bool isNumericalUnitClerkID = int.TryParse(UniClk, out UnitClerkID);
                            bool isNumericalUnitClerkStaffID = int.TryParse(UniClkStf, out UnitClerkStaffID);
                            if (isNumericalUnitClerkID == true && isNumericalUnitClerkStaffID == true)
                            {
                                UnitClerkID = UniClk != null ? Convert.ToInt32(UniClk) : 0;
                                UnitClerkStaffID = UniClkStf != null ? Convert.ToInt32(UniClkStf) : 0;
                                if (UnitClerkID > 0 && UnitClerkStaffID > 0)
                                {
                                    //regModel.StaffPosition10 = UnitClerkID;
                                    regModel.Position10Staff = UnitClerkStaffID;
                                }
                                else
                                {
                                    //regModel.StaffPosition10 = 0;
                                    regModel.Position10Staff = 0;
                                }
                            }
                            else
                            {
                                //regModel.StaffPosition10 = 0;
                                regModel.Position10Staff = 0;
                            }
                        }
                        else
                        {
                            //regModel.StaffPosition10 = 0;
                            regModel.Position10Staff = 0;
                        }
                    }
                    else
                    {
                        //regModel.StaffPosition10 = 0;
                        regModel.Position10Staff = 0;
                    }
                    #endregion

                }

                //===== List 6 - Task #2  

                regModel.StudentId = student.ClientId.ToString();
                regModel.Id = student.StudentPersonalId;
                regModel.FirstName = student.FirstName;
                regModel.LastName = student.LastName;
                regModel.MiddleName = student.MiddleName;
                regModel.NickName = student.NickName;
                regModel.LastNameSuffix = student.Suffix;
                regModel.DateOfBirth = ConvertDate(student.BirthDate);
                regModel.PlaceOfBirth = student.PlaceOfBirth;
                regModel.Photodate = ConvertDate(student.Photodate);
                regModel.CountryofBirth = student.CountryOfBirth;
                regModel.StateOfBirth = student.StateOfBirth;
                regModel.MostRecentGradeLevel = student.MostRecentGradeLevel;


                regModel.SASID = student.SSINo;
                regModel.Medicaid = student.Medicaid;
                regModel.PhotoReleasePermission = student.ImagePermission;
                regModel.ClientInfoComments = student.ClientInfoComments;
                regModel.PhotoPermComment = student.PhotoPermComment;

                regModel.PrimaryDiag = student.PrimaryDiag;

                try
                {
                    objLookUp = dbobj.LookUps.Where(objlukUp => objlukUp.LookupId == student.CountryOfBirth && objlukUp.LookupType == "Country").SingleOrDefault();
                    regModel.CountryBirth = objLookUp.LookupName;

                }
                catch
                {
                    regModel.CountryBirth = "";
                }
                try
                {
                    objLookUp = dbobj.LookUps.Where(objlukUp => objlukUp.LookupId == student.StateOfBirth && objlukUp.LookupType == "State").SingleOrDefault();
                    regModel.StateBirth = objLookUp.LookupName;
                }
                catch
                {
                    regModel.StateBirth = "";
                }
                regModel.Citizenship = Convert.ToInt32(student.CitizenshipStatus);
                regModel.Race = Convert.ToInt32(student.RaceId);
                regModel.Gender = student.Gender;
                regModel.HairColor = student.HairColor;
                regModel.EyeColor = student.EyeColor;
                regModel.DateUpdated = ConvertDate(student.AdmissionDate);
                //regModel.Height = student.Height.ToString();
                regModel.Note = student.Notes;
                regModel.Prefix = student.Prefix;
                regModel.PrimaryLanguage = student.PrimaryLanguage;
                regModel.EnglishProficiency = student.EnglishProficiency;
                regModel.SASID = student.SASID;
                regModel.Ambulatory = student.Ambulatory;
                regModel.Medicaid = student.Medicaid;
                regModel.Intensive = student.Intensive;
                regModel.AdmissinDate = student.AdmissionDate != null ? ((DateTime)student.AdmissionDate).ToString("MM/dd/yyyy") : "";
                regModel.IsGuardian = student.IsGuardian.GetBool();

                regModel.DayProgarm = student.DayProgarm;
                regModel.ClassroomWorkshop = student.ClassroomWorkshop;
                regModel.TeacherInstructor = student.TeacherInstructor;
                regModel.ProgramSpecialist = student.ProgramSpecialist;
                regModel.EDUBehaviorAnalyst = student.EDUBehaviorAnalyst;
                regModel.CurriculumCoordinator = student.CurriculumCoordinator;
                regModel.ResidentialProgram = student.ResidentialProgram;
                regModel.House = student.House;
                regModel.ProgramManagerQMRP = student.ProgramManagerQMRP;
                regModel.HouseSupervisor = student.HouseSupervisor;
                regModel.ResidentialBehaviorAnalyst = student.ResidentialBehaviorAnalyst;
                regModel.PrimaryNurse = student.PrimaryNurse;
                regModel.UnitClerk = student.UnitClerk;
                regModel.PhotoPermComment = student.PhotoPermComment;
                regModel.PhotoRelease = student.PhotoRelease;
                regModel.TripRestriction1 = student.TripRestriction1;
                regModel.TripRestriction2 = student.TripRestriction2;
                regModel.TripComments = student.TripComments;
                regModel.ClientInfoComments = student.ClientInfoComments;

                regModel.ReferralIEPFullName = student.IEPReferralFullName;
                regModel.ReferralIEPPhone = student.IEPReferralPhone;
                regModel.ReferralIEPReferringAgency = student.IEPReferralReferrinAgency;
                regModel.ReferralIEPSourceofTuition = student.IEPReferralSourceofTuition;
                regModel.ReferralIEPTitle = student.IEPReferralTitle;

                regModel.DateInitiallyEligibleforSpecialEducation = ConvertDate(student.DateInitiallyEligibleforSpecialEducation);
                regModel.DateofMostRecentSpecialEducationEvaluations = ConvertDate(student.DateofMostRecentSpecialEducationEvaluations);
                regModel.DateofNextScheduled3YearEvaluation = ConvertDate(student.DateofNextScheduled3YearEvaluation);
                regModel.CurrentIEPStartDate = ConvertDate(student.CurrentIEPStartDate);
                regModel.CurrentIEPExpirationDate = ConvertDate(student.CurrentIEPExpirationDate);

                regModel.DischargeDate = ConvertDate(student.DischargeDate);
                regModel.LocationAfterDischarge = student.LocationAfterDischarge;
                regModel.MelmarkNewEnglandsFollowUpResponsibilities = student.MelmarkNewEnglandsFollowUpResponsibilities;

                if (student.Classification1 == null)
                {
                    student.Classification1 = 0;
                }
                if (student.Classification2 == null)
                {
                    student.Classification2 = 0;
                }
                if (student.Classification3 == null)
                {
                    student.Classification3 = 0;
                }
                if (student.Classification4 == null)
                {
                    student.Classification4 = 0;
                }
                if (student.Classification5 == null)
                {
                    student.Classification5 = 0;
                }
                regModel.PlacementStat = student.PlacementStatus;
                regModel.Classification1 = student.Classification1;
                regModel.Classification2 = student.Classification2;
                regModel.Classification3 = student.Classification3;
                regModel.Classification4 = student.Classification4;
                regModel.Classification5 = student.Classification5;

                if (student.Height == null)
                {
                    regModel.Height = "";
                }
                else
                {
                    regModel.Height = Convert.ToString(Math.Floor(Convert.ToDecimal(student.Height)));
                }
                if (student.Weight == null)
                {
                    regModel.Weight = "";
                }
                else
                {
                    regModel.Weight = student.Weight.ToString().Split('.')[0] + '.' + student.Weight.ToString().Split('.')[1].Substring(0, 1);
                } 

                regModel.PrimaryLanguage = student.PrimaryLanguage;
                regModel.GuardianshipStatus = student.GuardianShip;
                regModel.DistigushingMarks = student.DistingushingMarks;
                regModel.LegalCompetencyStatus = student.LegalCompetencyStatus;
                regModel.OtherStateAgenciesInvolvedWithStudent = student.OtherStateAgenciesInvolvedWithStudent;
                regModel.MaritalStatusofBothParents = student.MaritalStatusofBothParents;
                regModel.CaseManagerEducational = student.CaseManagerEducational;
                regModel.CaseManagerResidential = student.CaseManagerResidential;
                regModel.ImageUrl = student.ImageUrl;
                regModel.PhotoReleasePermission = student.ImagePermission;
                if (address != null)
                {
                    regModel.AddressLine1 = address.ApartmentType;
                    regModel.AddressLine2 = address.StreetName;
                    regModel.AddressLine3 = address.AddressLine3;
                    regModel.ZipCode = address.PostalCode;
                    regModel.City = address.City;
                    regModel.Country = address.CountryId;
                    regModel.State = address.StateProvince;
                    regModel.ClientAddressPhone = address.ClientAddressPhone;
                }
                if (studentPA != null)
                {
                    regModel.Allergie = studentPA.Allergies;
                    regModel.Bathroom = studentPA.Bathroom;
                    regModel.BedroomAsleep = studentPA.ho_BedroomAsleep;
                    regModel.BedroomAwake = studentPA.ho_BedroomAwake;
                    regModel.Consciousness = studentPA.Consciousness;
                    regModel.CommonAreas = studentPA.CommonAreas;
                    regModel.Diet = studentPA.Diet;
                    regModel.Mobility = studentPA.Mobility;
                    regModel.NeedForExtraHelp = studentPA.NeedForExtraHelp;
                    regModel.OffCampus = studentPA.OffCampus;
                    regModel.OnCampus = studentPA.OnCampus;
                    regModel.Other = studentPA.Other;
                    regModel.PoolOrSwimming = studentPA.PoolOrSwimming;
                    regModel.ResponseToInstruction = studentPA.ResponseToInstruction;
                    regModel.RiskOfResistance = studentPA.RiskOfResistance;
                    regModel.Seizures = studentPA.Seizures;
                    regModel.TaskORBreak = studentPA.dy_TaskOrBreak;
                    regModel.TransitionInside = studentPA.dy_TransitionInside;
                    regModel.TransitionUnevenGround = studentPA.dy_TransitionUnevenGround;
                    regModel.van = studentPA.Van;
                    regModel.WalkingResponse = studentPA.WalkingResponses;
                    regModel.WhenTranspoting = studentPA.WhenTranspoting;
                    regModel.Funding = studentPA.FundingSource;
                }
                if (basicbehav.Count > 0)
                {
                    foreach (var item in basicbehav)
                    {
                        regModel.BasicBehave.Add(new BasicBehavior
                        {
                            Antecedent = item.Antecedent,
                            Consequances = item.Consequence,
                            Definition = item.Definition,
                            FCT = item.FCT,
                            TargetBehavior = item.TargetBehavior,
                            BasicBehavioralInformationId = item.BasicBehavioralInformationId
                        });

                    }
                }
                if (diagonis.Count > 0)
                {
                    foreach (var item in diagonis)
                    {
                        regModel.Diagnosis.Add(new Diagnosis
                        {
                            Name = item.Diaganoses
                        });

                    }
                }
                else
                {
                    regModel.Diagnosis.Add(new Diagnosis
                    {
                        Name = ""
                    });
                }
                if (adaptive.Count > 0)
                {
                    foreach (var item in adaptive)
                    {
                        regModel.Adapt.Add(new AdaptiveEquipmentz
                        {
                            item = item.Item,
                            ScheduledForUss = item.ScheduleForUse,
                            StorageLocation = item.StorageLocation,
                            CleaningInstruction = item.CleaningInstruction,
                            AdaptiveEquimentId = item.AdaptiveEquipmentId
                        });

                    }
                }
                //foreach (var item in adaptive)
                //{
                //    regModel.Adapt.Add(new AdaptiveEquipmentz
                //    {
                //        item = item.Item,
                //        ScheduledForUss = item.ScheduleForUse,
                //        StorageLocation = item.StorageLocation,
                //        CleaningInstruction = item.CleaningInstruction
                //    });

                //}
                try
                {

                    objLookUp = dbobj.LookUps.Where(objlukUp => objlukUp.LookupId == address.CountryId && objlukUp.LookupType == "Country").SingleOrDefault();
                    regModel.StrCountry = objLookUp.LookupName;
                }
                catch
                {
                    regModel.StrCountry = "";
                }
                try
                {
                    objLookUp = dbobj.LookUps.Where(objlukUp => objlukUp.LookupId == address.CountryId && objlukUp.LookupType == "State").SingleOrDefault();
                    regModel.StrState = objLookUp.LookupName;
                }
                catch
                {
                    regModel.StrState = "";
                }


                var behave = dbobj.BehavioursPAs.Where(objBehav => objBehav.StudentPersonalId == ClientId && objBehav.SchoolId == sess.SchoolId && objBehav.ParentId > 0).ToList();
                if (behave.Count > 0)
                {
                    int parentId = behavId("LIFTING / TRANSFERS");
                    bool flag = false;
                    foreach (var item in behave)
                    {
                        if (item.ParentId == parentId)
                        {

                            if (!flag)
                            {
                                regModel.LiftingOrTransfers1 = item.BehaviourName;
                                flag = true;
                            }
                            else
                            {
                                regModel.LiftingOrTransfers2 = item.BehaviourName;
                            }

                        }
                    }

                    parentId = behavId("AMBULATION");
                    flag = false;
                    foreach (var item in behave)
                    {
                        if (item.ParentId == parentId)
                        {

                            if (!flag)
                            {
                                regModel.Ambulation1 = item.BehaviourName;
                                flag = true;
                            }
                            else
                            {
                                regModel.Ambulation2 = item.BehaviourName;
                            }

                        }
                    }

                    parentId = behavId("TOILETING");
                    flag = false;
                    foreach (var item in behave)
                    {
                        if (item.ParentId == parentId)
                        {

                            if (!flag)
                            {
                                regModel.Toileting1 = item.BehaviourName;
                                flag = true;
                            }
                            else
                            {
                                regModel.Toileting2 = item.BehaviourName;
                            }

                        }
                    }

                    parentId = behavId("EATING");
                    flag = false;
                    foreach (var item in behave)
                    {
                        if (item.ParentId == parentId)
                        {

                            if (!flag)
                            {
                                regModel.Eating1 = item.BehaviourName;
                                flag = true;
                            }
                            else
                            {
                                regModel.Eating2 = item.BehaviourName;
                            }

                        }
                    }

                    parentId = behavId("SHOWERING");
                    flag = false;
                    foreach (var item in behave)
                    {
                        if (item.ParentId == parentId)
                        {

                            if (!flag)
                            {
                                regModel.Showering1 = item.BehaviourName;
                                flag = true;
                            }
                            else
                            {
                                regModel.Showering2 = item.BehaviourName;
                            }

                        }
                    }

                    parentId = behavId("TOOTHBRUSHING");
                    flag = false;
                    foreach (var item in behave)
                    {
                        if (item.ParentId == parentId)
                        {

                            if (!flag)
                            {
                                regModel.ToothBrushing1 = item.BehaviourName;
                                flag = true;
                            }
                            else
                            {
                                regModel.ToothBrushing2 = item.BehaviourName;
                            }

                        }
                    }

                    parentId = behavId("DRESSING");
                    flag = false;
                    foreach (var item in behave)
                    {
                        if (item.ParentId == parentId)
                        {

                            if (!flag)
                            {
                                regModel.Dressing1 = item.BehaviourName;
                                flag = true;
                            }
                            else
                            {
                                regModel.Dressing2 = item.BehaviourName;
                            }

                        }
                    }

                    parentId = behavId("SKIN CARE/SKIN INTEGRITY");
                    flag = false;
                    foreach (var item in behave)
                    {
                        if (item.ParentId == parentId)
                        {

                            if (!flag)
                            {
                                regModel.SkinCare1 = item.BehaviourName;
                                flag = true;
                            }
                            else
                            {
                                regModel.SkinCare2 = item.BehaviourName;
                            }
                            ;
                        }
                    }

                    parentId = behavId("COMMUNICATION");
                    flag = false;
                    foreach (var item in behave)
                    {
                        if (item.ParentId == parentId)
                        {

                            if (!flag)
                            {
                                regModel.Communication1 = item.BehaviourName;
                                flag = true;
                            }
                            else
                            {
                                regModel.Communication2 = item.BehaviourName;
                            }

                        }
                    }


                    parentId = behavId("PREFERRED ACTIVITIES");
                    flag = false;
                    foreach (var item in behave)
                    {
                        if (item.ParentId == parentId)
                        {

                            if (!flag)
                            {
                                regModel.preferedActivities1 = item.BehaviourName;
                                flag = true;
                            }
                            else
                            {
                                regModel.preferedActivities2 = item.BehaviourName;
                            }

                        }
                    }

                    parentId = behavId("GENERAL INFORMATION");
                    flag = false;
                    foreach (var item in behave)
                    {
                        if (item.ParentId == parentId)
                        {

                            if (!flag)
                            {
                                regModel.GeneralInformation1 = item.BehaviourName;
                                flag = true;
                            }
                            else
                            {
                                regModel.GeneralInformation2 = item.BehaviourName;
                            }

                        }
                    }

                    parentId = behavId("SUGGESTED PROACTIVE ENVIRONMENTAL PROCEDURES");
                    flag = false;
                    foreach (var item in behave)
                    {
                        if (item.ParentId == parentId)
                        {

                            if (!flag)
                            {
                                regModel.SuggestedProactiveEnvironmentalProcedures1 = item.BehaviourName;
                                flag = true;
                            }
                            else
                            {
                                regModel.SuggestedProactiveEnvironmentalProcedures2 = item.BehaviourName;
                            }

                        }
                    }
                }

                if (EmergencyContacts != null)
                {
                    foreach (var item in EmergencyContacts)
                    {
                        if (item.SequenceId == 1)
                        {
                            regModel.EmergencyContactFirstName1 = item.FirstName;
                            regModel.EmergencyContactLastName1 = item.LastName;
                            regModel.EmergencyContactTitle1 = item.Title;
                            regModel.EmergencyContactPhone1 = item.Phone;
                        }
                        if (item.SequenceId == 2)
                        {
                            regModel.EmergencyContactFirstName2 = item.FirstName;
                            regModel.EmergencyContactLastName2 = item.LastName;
                            regModel.EmergencyContactTitle2 = item.Title;
                            regModel.EmergencyContactPhone2 = item.Phone;
                        }
                        if (item.SequenceId == 3)
                        {
                            regModel.EmergencyContactFirstName3 = item.FirstName;
                            regModel.EmergencyContactLastName3 = item.LastName;
                            regModel.EmergencyContactTitle3 = item.Title;
                            regModel.EmergencyContactPhone3 = item.Phone;
                        }
                        if (item.SequenceId == 4)
                        {
                            regModel.EmergencyContactFirstName4 = item.FirstName;
                            regModel.EmergencyContactLastName4 = item.LastName;
                            regModel.EmergencyContactTitle4 = item.Title;
                            regModel.EmergencyContactPhone4 = item.Phone;
                        }
                        if (item.SequenceId == 5)
                        {
                            regModel.EmergencyContactFirstName5 = item.FirstName;
                            regModel.EmergencyContactLastName5 = item.LastName;
                            regModel.EmergencyContactTitle5 = item.Title;
                            regModel.EmergencyContactPhone5 = item.Phone;
                        }
                    }
                }

                if (SchoolsAttended != null)
                {
                    foreach (var item in SchoolsAttended)
                    {
                        if (item.SequenceId == 1)
                        {
                            regModel.SchoolName1 = item.SchoolName;
                            regModel.DateFrom1 = ConvertDate(item.DateFrom);
                            regModel.DateTo1 = ConvertDate(item.DateTo);
                            regModel.SchoolAttendedAddress11 = item.Address1;
                            regModel.SchoolAttendedAddress21 = item.Address2;
                            regModel.SchoolAttendedCity1 = item.City;
                            try
                            {
                                int state1 = Convert.ToInt32(item.State);
                                objLookUp = dbobj.LookUps.Where(objlukUp => objlukUp.LookupId == state1 && objlukUp.LookupType == "State").SingleOrDefault();
                                regModel.SchoolAttendedState1 = objLookUp.LookupName;
                                regModel.intSchoolAttendedState1 = Convert.ToInt32(item.State);
                            }
                            catch
                            {
                                regModel.SchoolAttendedState1 = "";
                            }
                        }
                        if (item.SequenceId == 2)
                        {
                            regModel.SchoolName2 = item.SchoolName;
                            regModel.DateFrom2 = ConvertDate(item.DateFrom);
                            regModel.DateTo2 = ConvertDate(item.DateTo);
                            regModel.SchoolAttendedAddress12 = item.Address1;
                            regModel.SchoolAttendedAddress22 = item.Address2;
                            regModel.SchoolAttendedCity2 = item.City;
                            try
                            {
                                int state2 = Convert.ToInt32(item.State);
                                objLookUp = dbobj.LookUps.Where(objlukUp => objlukUp.LookupId == state2 && objlukUp.LookupType == "State").SingleOrDefault();
                                regModel.SchoolAttendedState2 = objLookUp.LookupName;
                                regModel.intSchoolAttendedState2 = Convert.ToInt32(item.State);
                            }
                            catch
                            {
                                regModel.SchoolAttendedState2 = "";
                            }
                        }
                        if (item.SequenceId == 3)
                        {
                            regModel.SchoolName3 = item.SchoolName;
                            regModel.DateFrom3 = ConvertDate(item.DateFrom);
                            regModel.DateTo3 = ConvertDate(item.DateTo);
                            regModel.SchoolAttendedAddress13 = item.Address1;
                            regModel.SchoolAttendedAddress23 = item.Address2;
                            regModel.SchoolAttendedCity3 = item.City;
                            try
                            {
                                int state3 = Convert.ToInt32(item.State);
                                objLookUp = dbobj.LookUps.Where(objlukUp => objlukUp.LookupId == state3 && objlukUp.LookupType == "State").SingleOrDefault();
                                regModel.SchoolAttendedState3 = objLookUp.LookupName;
                                regModel.intSchoolAttendedState3 = Convert.ToInt32(item.State);
                            }
                            catch
                            {
                                regModel.SchoolAttendedState3 = "";
                            }
                        }
                    }
                }

            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
            }
            regModel.ClientStatus = DisplayStudStatus();
            return regModel;
        }


        public ClientRegistrationPAModel staticClientDataPA(int ClientId)
        {
            // sess = (ClsSession)HttpContext.Current.Session["UserSessionClient"];

            ClientRegistrationPAModel regModel = new ClientRegistrationPAModel();
            dbobj = new BiWeeklyRCPNewEntities();
            StudentPersonal student = new StudentPersonal();
            StudentPersonalPA studentPA = new StudentPersonalPA();
            AddressList address = new AddressList();
            StudentAddresRel stdAddrRel = new StudentAddresRel();
            EmergencyContactSchool EmergencyContact = new EmergencyContactSchool();
            SchoolsAttended SchoolAttended = new SchoolsAttended();
            LookUp objLookUp = new LookUp();

            //ClientFunderModel clntFunder = new ClientFunderModel();

            sess = (clsSession)HttpContext.Current.Session["UserSessionClient"];
            Insurance objInsurance = new Insurance();
            try
            {
                student = dbobj.StudentPersonals.Where(x => x.StudentPersonalId == ClientId && x.SchoolId == sess.SchoolId).SingleOrDefault();
                studentPA = dbobj.StudentPersonalPAs.Where(objStudentPA => objStudentPA.StudentPersonalId == ClientId && objStudentPA.SchoolId == sess.SchoolId).SingleOrDefault();
                stdAddrRel = dbobj.StudentAddresRels.Where(x => x.StudentPersonalId == ClientId && x.ContactSequence == 0).SingleOrDefault();
                //AddressType is 0 for MelmarkPA (check for NE)
                address = dbobj.AddressLists.Where(x => x.AddressId == stdAddrRel.AddressId).SingleOrDefault();

                // var behave = dbobj.BehavioursPAs.Where(objBehave => objBehave.StudentPersonalId == sess.ClientId && objBehave.SchoolId == sess.SchoolId).ToList();
                // var diagonis = dbobj.DiaganosesPAs.Where(objDiagno => objDiagno.StudentPersonalId == ClientId && objDiagno.SchoolId == sess.SchoolId).ToList();
                var diagonis = dbobj.DiaganosesPAs.Where(objDiagno => objDiagno.StudentPersonalId == ClientId).ToList();
                var adaptive = dbobj.AdaptiveEquipments.Where(objAdaptive => objAdaptive.StudentPersonalId == ClientId && objAdaptive.SchoolId == sess.SchoolId).ToList();
                var basicbehav = dbobj.BasicBehavioralInformations.Where(objBasic => objBasic.StudentPersonalId == ClientId && objBasic.SchoolId == sess.SchoolId).ToList();

                regModel.Id = student.StudentPersonalId;
                regModel.FirstName = student.FirstName;
                regModel.LastName = student.LastName;
                regModel.MiddleName = student.MiddleName;
                regModel.NickName = student.NickName;
                regModel.LastNameSuffix = student.Suffix;
                regModel.DateOfBirth = ConvertDate(student.BirthDate);
                regModel.PlaceOfBirth = student.PlaceOfBirth;
                regModel.Photodate = ConvertDate(student.Photodate);
                regModel.CountryofBirth = student.CountryOfBirth;
                regModel.StateOfBirth = student.StateOfBirth;
                regModel.MostRecentGradeLevel = student.MostRecentGradeLevel;
                try
                {
                    objLookUp = dbobj.LookUps.Where(objlukUp => objlukUp.LookupId == student.CountryOfBirth && objlukUp.LookupType == "Country").SingleOrDefault();
                    regModel.CountryBirth = objLookUp.LookupName;

                }
                catch
                {
                    regModel.CountryBirth = "";
                }
                try
                {
                    objLookUp = dbobj.LookUps.Where(objlukUp => objlukUp.LookupId == student.StateOfBirth && objlukUp.LookupType == "State").SingleOrDefault();
                    regModel.StateBirth = objLookUp.LookupName;
                }
                catch
                {
                    regModel.StateBirth = "";
                }
                regModel.Citizenship = Convert.ToInt32(student.CitizenshipStatus);
                regModel.Race = Convert.ToInt32(student.RaceId);
                regModel.Gender = student.Gender;
                regModel.HairColor = student.HairColor;
                regModel.EyeColor = student.EyeColor;
                regModel.DateUpdated = ConvertDate(student.AdmissionDate);
                regModel.Height = student.Height.ToString();
                regModel.Note = student.Notes;
                regModel.Prefix = student.Prefix;
                regModel.PrimaryLanguage = student.PrimaryLanguage;
                regModel.EnglishProficiency = student.EnglishProficiency;
                regModel.SASID = student.SASID;
                regModel.Ambulatory = student.Ambulatory;
                regModel.Medicaid = student.Medicaid;
                regModel.Intensive = student.Intensive;
                regModel.AdmissinDate = student.AdmissionDate != null ? ((DateTime)student.AdmissionDate).ToString("MM/dd/yyyy") : "";
                regModel.IsGuardian = student.IsGuardian.GetBool();

                if (student.Height == null)
                {
                    regModel.Height = "";
                }
                else
                {
                    regModel.Height = Convert.ToString(Math.Floor(Convert.ToDecimal(student.Height)));
                }
                if (student.Weight == null)
                {
                    regModel.Weight = "";
                }
                else
                {
                    regModel.Weight = student.Weight.ToString().Split('.')[0] + '.' + student.Weight.ToString().Split('.')[1].Substring(0, 1);
                }         

                regModel.PrimaryLanguage = student.PrimaryLanguage;
                regModel.GuardianshipStatus = student.GuardianShip;
                regModel.DistigushingMarks = student.DistingushingMarks;
                regModel.LegalCompetencyStatus = student.LegalCompetencyStatus;
                regModel.OtherStateAgenciesInvolvedWithStudent = student.OtherStateAgenciesInvolvedWithStudent;
                regModel.MaritalStatusofBothParents = student.MaritalStatusofBothParents;
                regModel.CaseManagerEducational = student.CaseManagerEducational;
                regModel.CaseManagerResidential = student.CaseManagerResidential;
                regModel.ImageUrl = student.ImageUrl;
                regModel.PhotoReleasePermission = student.ImagePermission;
                if (address != null)
                {
                    regModel.AddressLine1 = address.ApartmentType;
                    regModel.AddressLine2 = address.StreetName;
                    regModel.AddressLine3 = address.AddressLine3;
                    regModel.ZipCode = address.PostalCode;
                    regModel.City = address.City;
                    regModel.Country = address.CountryId;
                    regModel.State = address.StateProvince;
                }
                if (studentPA != null)
                {
                    regModel.Allergie = studentPA.Allergies;
                    regModel.Bathroom = studentPA.Bathroom;
                    regModel.BedroomAsleep = studentPA.ho_BedroomAsleep;
                    regModel.BedroomAwake = studentPA.ho_BedroomAwake;
                    regModel.Consciousness = studentPA.Consciousness;
                    regModel.CommonAreas = studentPA.CommonAreas;
                    regModel.Diet = studentPA.Diet;
                    regModel.Mobility = studentPA.Mobility;
                    regModel.NeedForExtraHelp = studentPA.NeedForExtraHelp;
                    regModel.OffCampus = studentPA.OffCampus;
                    regModel.OnCampus = studentPA.OnCampus;
                    regModel.Other = studentPA.Other;
                    regModel.PoolOrSwimming = studentPA.PoolOrSwimming;
                    regModel.ResponseToInstruction = studentPA.ResponseToInstruction;
                    regModel.RiskOfResistance = studentPA.RiskOfResistance;
                    regModel.Seizures = studentPA.Seizures;
                    regModel.TaskORBreak = studentPA.dy_TaskOrBreak;
                    regModel.TransitionInside = studentPA.dy_TransitionInside;
                    regModel.TransitionUnevenGround = studentPA.dy_TransitionUnevenGround;
                    regModel.van = studentPA.Van;
                    regModel.WalkingResponse = studentPA.WalkingResponses;
                    regModel.WhenTranspoting = studentPA.WhenTranspoting;
                    regModel.Funding = studentPA.FundingSource;
                }
                if (basicbehav.Count > 0)
                {
                    foreach (var item in basicbehav)
                    {
                        regModel.BasicBehave.Add(new BasicBehavior
                        {
                            Antecedent = item.Antecedent,
                            Consequances = item.Consequence,
                            Definition = item.Definition,
                            FCT = item.FCT,
                            TargetBehavior = item.TargetBehavior,
                            BasicBehavioralInformationId = item.BasicBehavioralInformationId
                        });

                    }
                }
                if (diagonis.Count > 0)
                {
                    foreach (var item in diagonis)
                    {
                        regModel.Diagnosis.Add(new Diagnosis
                        {
                            Name = item.Diaganoses
                        });

                    }
                }
                if (adaptive.Count > 0)
                {
                    foreach (var item in adaptive)
                    {
                        regModel.Adapt.Add(new AdaptiveEquipmentz
                        {
                            item = item.Item,
                            ScheduledForUss = item.ScheduleForUse,
                            StorageLocation = item.StorageLocation,
                            CleaningInstruction = item.CleaningInstruction,
                            AdaptiveEquimentId = item.AdaptiveEquipmentId
                        });

                    }
                }
                //foreach (var item in adaptive)
                //{
                //    regModel.Adapt.Add(new AdaptiveEquipmentz
                //    {
                //        item = item.Item,
                //        ScheduledForUss = item.ScheduleForUse,
                //        StorageLocation = item.StorageLocation,
                //        CleaningInstruction = item.CleaningInstruction
                //    });

                //}
                try
                {

                    objLookUp = dbobj.LookUps.Where(objlukUp => objlukUp.LookupId == address.CountryId && objlukUp.LookupType == "Country").SingleOrDefault();
                    regModel.StrCountry = objLookUp.LookupName;
                }
                catch
                {
                    regModel.StrCountry = "";
                }
                try
                {
                    objLookUp = dbobj.LookUps.Where(objlukUp => objlukUp.LookupId == address.CountryId && objlukUp.LookupType == "State").SingleOrDefault();
                    regModel.StrState = objLookUp.LookupName;
                }
                catch
                {
                    regModel.StrState = "";
                }


                var behave = dbobj.BehavioursPAs.Where(objBehav => objBehav.StudentPersonalId == ClientId && objBehav.SchoolId == sess.SchoolId && objBehav.ParentId > 0).ToList();
                if (behave.Count > 0)
                {
                    int parentId = behavId("LIFTING / TRANSFERS");
                    bool flag = false;
                    foreach (var item in behave)
                    {
                        if (item.ParentId == parentId)
                        {

                            if (!flag)
                            {
                                regModel.LiftingOrTransfers1 = item.BehaviourName;
                                flag = true;
                            }
                            else
                            {
                                regModel.LiftingOrTransfers2 = item.BehaviourName;
                            }

                        }
                    }

                    parentId = behavId("AMBULATION");
                    flag = false;
                    foreach (var item in behave)
                    {
                        if (item.ParentId == parentId)
                        {

                            if (!flag)
                            {
                                regModel.Ambulation1 = item.BehaviourName;
                                flag = true;
                            }
                            else
                            {
                                regModel.Ambulation2 = item.BehaviourName;
                            }

                        }
                    }

                    parentId = behavId("TOILETING");
                    flag = false;
                    foreach (var item in behave)
                    {
                        if (item.ParentId == parentId)
                        {

                            if (!flag)
                            {
                                regModel.Toileting1 = item.BehaviourName;
                                flag = true;
                            }
                            else
                            {
                                regModel.Toileting2 = item.BehaviourName;
                            }

                        }
                    }

                    parentId = behavId("EATING");
                    flag = false;
                    foreach (var item in behave)
                    {
                        if (item.ParentId == parentId)
                        {

                            if (!flag)
                            {
                                regModel.Eating1 = item.BehaviourName;
                                flag = true;
                            }
                            else
                            {
                                regModel.Eating2 = item.BehaviourName;
                            }

                        }
                    }

                    parentId = behavId("SHOWERING");
                    flag = false;
                    foreach (var item in behave)
                    {
                        if (item.ParentId == parentId)
                        {

                            if (!flag)
                            {
                                regModel.Showering1 = item.BehaviourName;
                                flag = true;
                            }
                            else
                            {
                                regModel.Showering2 = item.BehaviourName;
                            }

                        }
                    }

                    parentId = behavId("TOOTHBRUSHING");
                    flag = false;
                    foreach (var item in behave)
                    {
                        if (item.ParentId == parentId)
                        {

                            if (!flag)
                            {
                                regModel.ToothBrushing1 = item.BehaviourName;
                                flag = true;
                            }
                            else
                            {
                                regModel.ToothBrushing2 = item.BehaviourName;
                            }

                        }
                    }

                    parentId = behavId("DRESSING");
                    flag = false;
                    foreach (var item in behave)
                    {
                        if (item.ParentId == parentId)
                        {

                            if (!flag)
                            {
                                regModel.Dressing1 = item.BehaviourName;
                                flag = true;
                            }
                            else
                            {
                                regModel.Dressing2 = item.BehaviourName;
                            }

                        }
                    }

                    parentId = behavId("SKIN CARE/SKIN INTEGRITY");
                    flag = false;
                    foreach (var item in behave)
                    {
                        if (item.ParentId == parentId)
                        {

                            if (!flag)
                            {
                                regModel.SkinCare1 = item.BehaviourName;
                                flag = true;
                            }
                            else
                            {
                                regModel.SkinCare2 = item.BehaviourName;
                            }
                            ;
                        }
                    }

                    parentId = behavId("COMMUNICATION");
                    flag = false;
                    foreach (var item in behave)
                    {
                        if (item.ParentId == parentId)
                        {

                            if (!flag)
                            {
                                regModel.Communication1 = item.BehaviourName;
                                flag = true;
                            }
                            else
                            {
                                regModel.Communication2 = item.BehaviourName;
                            }

                        }
                    }


                    parentId = behavId("PREFERRED ACTIVITIES");
                    flag = false;
                    foreach (var item in behave)
                    {
                        if (item.ParentId == parentId)
                        {

                            if (!flag)
                            {
                                regModel.preferedActivities1 = item.BehaviourName;
                                flag = true;
                            }
                            else
                            {
                                regModel.preferedActivities2 = item.BehaviourName;
                            }

                        }
                    }

                    parentId = behavId("GENERAL INFORMATION");
                    flag = false;
                    foreach (var item in behave)
                    {
                        if (item.ParentId == parentId)
                        {

                            if (!flag)
                            {
                                regModel.GeneralInformation1 = item.BehaviourName;
                                flag = true;
                            }
                            else
                            {
                                regModel.GeneralInformation2 = item.BehaviourName;
                            }

                        }
                    }

                    parentId = behavId("SUGGESTED PROACTIVE ENVIRONMENTAL PROCEDURES");
                    flag = false;
                    foreach (var item in behave)
                    {
                        if (item.ParentId == parentId)
                        {

                            if (!flag)
                            {
                                regModel.SuggestedProactiveEnvironmentalProcedures1 = item.BehaviourName;
                                flag = true;
                            }
                            else
                            {
                                regModel.SuggestedProactiveEnvironmentalProcedures2 = item.BehaviourName;
                            }

                        }
                    }
                }



            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
            }
            regModel.ClientStatus = DisplayStudStatus();
            return regModel;
        }

        /// <summary>
        /// Function is used to Fill the selected Contact Data..
        /// </summary>
        /// <param name="ClientId"></param>
        /// <param name="itemId"></param>
        /// <returns></returns>

        public ContactModel bindContactData(int ClientId, int itemId)
        {
            //sess = (ClsSession)HttpContext.Current.Session["UserSessionClient"];
            dbobj = new BiWeeklyRCPNewEntities();
            ContactModel contactModel = new ContactModel();


            ContactPersonal contactPersonal = new ContactPersonal();
            StudentContactRelationship contactRelation = new StudentContactRelationship();
            StudentAddresRel addressRelation = new StudentAddresRel();
            AddressList adrList = new AddressList();
            contactPersonal = dbobj.ContactPersonals.Where(objContactPersonal => objContactPersonal.StudentPersonalId == ClientId &&
                objContactPersonal.ContactPersonalId == itemId).SingleOrDefault();

            if (contactPersonal != null)
            {
                contactModel.Id = contactPersonal.ContactPersonalId;
                contactModel.FirstName = contactPersonal.FirstName;
                contactModel.LastName = contactPersonal.LastName;
                contactModel.FirstNamePrefix = contactPersonal.Prefix;
                contactModel.LastNameSuffix = contactPersonal.Suffix;
                contactModel.Spouse = contactPersonal.Spouse;
                contactModel.MiddleName = contactPersonal.MiddleName;
                contactModel.ContactFlag = contactPersonal.ContactFlag;
                contactModel.PrimaryLanguage = contactPersonal.PrimaryLanguage;

                contactModel.IsBilling = contactPersonal.IsBilling.GetBool();
                //contactModel.IsEmergency = contactPersonal.IsEmergency.GetBool();
                contactModel.IsEmergencyP = contactPersonal.IsEmergencyP.GetBool();
                contactModel.IsEmergencyS = contactPersonal.IsEmergencyS.GetBool();
                contactModel.IsEmergencyT = contactPersonal.IsEmergencyT.GetBool();
                contactModel.IsEmergencyO = contactPersonal.IsEmergencyO.GetBool();
                contactModel.IsIncident = contactPersonal.IsIncident.GetBool();
                contactModel.IsSchool = contactPersonal.IsSchool.GetBool();
                contactModel.Note = contactPersonal.Note;
                contactModel.SpouseId = contactPersonal.SpouseId;
                contactModel.IsOnCampusAlone = contactPersonal.IsOnCampusAlone.GetBool();
                contactModel.IsOnCampusWithStaff = contactPersonal.IsOnCampusWithStaff.GetBool();
                contactModel.IsOffCampus = contactPersonal.IsOffCampus.GetBool();


                contactModel.IsCorrespondence = contactPersonal.IsCorrespondence.GetBool();
                contactModel.IsCustody = contactPersonal.IsCustody.GetBool();
                contactModel.IsGuardian = contactPersonal.IsGuardian.GetBool();
                contactModel.IsNextOfKin = contactPersonal.IsNextOfKin.GetBool();
                contactModel.ApprovedVisitor = contactPersonal.ApprovedVisitor.GetBool();

                contactModel.Occupation = contactPersonal.Occupation;
                contactModel.Employer = contactPersonal.Employer;

                contactRelation = dbobj.StudentContactRelationships.Where(objContactrelation => objContactrelation.
                    ContactPersonalId == contactPersonal.ContactPersonalId).SingleOrDefault();
                contactModel.Relation = contactRelation.RelationshipId;
                LookUp lk = dbobj.LookUps.Where(objlk => objlk.LookupId == contactModel.Relation).SingleOrDefault();
                if (lk.LookupName == "Parent")
                {
                    Parent prnt = dbobj.Parents.Where(objparent => objparent.ContactPersonalId == contactPersonal.ContactPersonalId).SingleOrDefault();
                    contactModel.UserID = prnt.Username;
                }
                else
                    contactModel.UserID = "";

                var addresses = dbobj.StudentAddresRels.Join(dbobj.ContactPersonals, objAddresRel => objAddresRel.ContactPersonalId,
                    objContactPersonal => objContactPersonal.ContactPersonalId,
                (objAddresRel, objContactPersonal) => new
                {
                    ClientId = objAddresRel.StudentPersonalId,
                    ContactSequance = objAddresRel.ContactSequence,
                    AddressId = objAddresRel.AddressId,
                    ContactID = objAddresRel.ContactPersonalId
                }).
                Where(x => x.ClientId == ClientId && x.ContactSequance > 0 && x.ContactID == itemId).ToList().OrderBy(x => x.ContactSequance);



                // var contactList = dbobj.AddressLists.Where(x => x.AddressId == contactPersonal.ContactPersonalId).ToList();
                foreach (var item in addresses)
                {
                    if (item.ContactSequance == 1)
                    {
                        adrList = dbobj.AddressLists.Where(x => x.AddressId == item.AddressId).SingleOrDefault();
                        contactModel.HomeAddressTypeId = adrList.AddressType;
                        contactModel.HomeAddressLine1 = adrList.ApartmentType;
                        contactModel.HomeAddressLine2 = adrList.StreetName;
                        contactModel.HomeCity = adrList.City;
                        contactModel.HomeState = Convert.ToInt32(adrList.StateProvince);
                        if (contactModel.HomeState != 0)
                        {
                            contactModel.HmeState = dbobj.LookUps.Where(objLookup => objLookup.LookupType == "State" && objLookup.LookupId == contactModel.HomeState).Select(objLookup => objLookup.LookupName).Single();
                        }
                        contactModel.HomeCountry = Convert.ToInt32(adrList.CountryId);
                        contactModel.HomeCounty = adrList.County;
                        contactModel.HomePhone = adrList.Phone;
                        contactModel.HomeMobilePhone = adrList.Mobile;
                        contactModel.HomeWorkPhone = adrList.OtherPhone;
                        contactModel.HomeFax = adrList.Fax;
                        contactModel.HomeEmail = adrList.PrimaryEmail;
                        contactModel.HomeOtherMail = adrList.EmailOther;
                        contactModel.HomeWorkEmail = adrList.SecondryEmail;
                        contactModel.HomeZip = adrList.PostalCode;

                        contactModel.HomeIsMailingAddress = adrList.IsMailingAddress.GetBool();
                        contactModel.HomeExtension = adrList.Extension;
                        contactModel.HomeMailOptIn = adrList.MailOptIn.GetBool();
                        contactModel.HomeEmailOptIn = adrList.EmailOptIn.GetBool();
                        contactModel.HomePhoneOptIn = adrList.PhoneOptIn.GetBool();
                        contactModel.HomeMailMergeOptIn = adrList.MailMergeOptIn.GetBool();

                    }
                    if (item.ContactSequance == 2)
                    {
                        adrList = dbobj.AddressLists.Where(x => x.AddressId == item.AddressId).SingleOrDefault();
                        contactModel.WorkAddressTypeId = adrList.AddressType;
                        contactModel.WorkAddressLine1 = adrList.ApartmentType;
                        contactModel.WorkAddressLine2 = adrList.StreetName;
                        contactModel.WorkCity = adrList.City;
                        contactModel.WorkState = Convert.ToInt32(adrList.StateProvince);
                        if (contactModel.WorkState != 0)
                        {
                            contactModel.WrkState = dbobj.LookUps.Where(objLookup => objLookup.LookupType == "State" && objLookup.LookupId == contactModel.WorkState).Select(objLookup => objLookup.LookupName).Single();
                        }
                        contactModel.WorkCountry = Convert.ToInt32(adrList.CountryId);
                        contactModel.WorkCounty = adrList.County;
                        contactModel.WorkHomePhone = adrList.Phone;
                        contactModel.WorkMobilePhone = adrList.Mobile;
                        contactModel.WorkPhone = adrList.OtherPhone;
                        contactModel.WorkFax = adrList.Fax;
                        //contactModel.WorkHomeEmail = adrList.PrimaryEmail;
                        //contactModel.WorkEmail = adrList.SecondryEmail;
                        contactModel.WorkEmail = adrList.PrimaryEmail;
                        contactModel.WorkZip = adrList.PostalCode;

                        contactModel.WorkIsMailingAddress = adrList.IsMailingAddress.GetBool();
                        contactModel.WorkExtension = adrList.Extension;
                        contactModel.OtherExtension2 = adrList.Extension2;
                        contactModel.WorkMailOptIn = adrList.MailOptIn.GetBool();
                        contactModel.WorkEmailOptIn = adrList.EmailOptIn.GetBool();
                        contactModel.WorkPhoneOptIn = adrList.PhoneOptIn.GetBool();
                        contactModel.WorkMailMergeOptIn = adrList.MailMergeOptIn.GetBool();


                    }
                    if (item.ContactSequance == 3)
                    {
                        adrList = dbobj.AddressLists.Where(x => x.AddressId == item.AddressId).SingleOrDefault();
                        contactModel.OtherAddressTypeId = adrList.AddressType;
                        contactModel.OtherAddressLine1 = adrList.ApartmentType;
                        contactModel.OtherAddressLine2 = adrList.StreetName;
                        contactModel.OtherCity = adrList.City;
                        contactModel.OtherState = Convert.ToInt32(adrList.StateProvince);
                        if (contactModel.OtherState != 0)
                        {
                            contactModel.OthState = dbobj.LookUps.Where(objLookup => objLookup.LookupType == "State" && objLookup.LookupId == contactModel.OtherState).Select(objLookup => objLookup.LookupName).Single();
                        }
                        contactModel.OtherCountry = Convert.ToInt32(adrList.CountryId);
                        contactModel.OtherCounty = adrList.County;
                        contactModel.OtherHomePhone = adrList.Phone;
                        contactModel.OtherMobilePhone = adrList.Mobile;
                        contactModel.OtherWorkPhone = adrList.OtherPhone;
                        contactModel.OtherFax = adrList.Fax;
                        contactModel.OtherHomeEmail = adrList.PrimaryEmail;
                        contactModel.OtherWorkEmail = adrList.SecondryEmail;
                        contactModel.OtherZip = adrList.PostalCode;
                        contactModel.HomeStreet = adrList.StreetName;
                        contactModel.HomeNumber = adrList.ApartmentNumber;

                        contactModel.OtherIsMailingAddress = adrList.IsMailingAddress.GetBool();
                        contactModel.OtherExtension = adrList.Extension;
                        contactModel.OtherExtension3 = adrList.Extension2;
                        contactModel.OtherMailOptIn = adrList.MailOptIn.GetBool();
                        contactModel.OtherEmailOptIn = adrList.EmailOptIn.GetBool();
                        contactModel.OtherPhoneOptIn = adrList.PhoneOptIn.GetBool();
                        contactModel.OtherMailMergeOptIn = adrList.MailMergeOptIn.GetBool();

                    }
                }

            }
            return contactModel;
        }


        /// <summary>
        /// Function To Delete Contact Data.
        /// </summary>
        /// <param name="ClientId"></param>
        /// <param name="itemId"></param>
        /// 

        public void deleteATList(int id)
        {
            dbobj = new BiWeeklyRCPNewEntities();

            var objPS1 = dbobj.Protocol_1_Assistive.Where(x => x.AssistiveId == id).ToList();
            if (objPS1.Count > 0)
            {
                dbobj.Protocol_1_Assistive.Remove(objPS1[0]);
                dbobj.SaveChanges();
            }
        }

        public void deleteCGList(int id)
        {
            dbobj = new BiWeeklyRCPNewEntities();

            var objPS1 = dbobj.Protocol_2_Community.Where(x => x.CommunityId == id).ToList();
            if (objPS1.Count > 0)
            {
                dbobj.Protocol_2_Community.Remove(objPS1[0]);
                dbobj.SaveChanges();
            }
        }

        public void deleteFIList(int id)
        {
            dbobj = new BiWeeklyRCPNewEntities();

            var objPS1 = dbobj.Protocol_3_Family.Where(x => x.FamilyId == id).ToList();
            if (objPS1.Count > 0)
            {
                dbobj.Protocol_3_Family.Remove(objPS1[0]);
                dbobj.SaveChanges();
            }
        }

        public void deleteBBIList(int id)
        {
            dbobj = new BiWeeklyRCPNewEntities();

            var objPS1 = dbobj.Protocol_4_Basic.Where(x => x.BasicId == id).ToList();
            if (objPS1.Count > 0)
            {
                dbobj.Protocol_4_Basic.Remove(objPS1[0]);
                dbobj.SaveChanges();
            }
        }

        public void deleteSignList(int id)
        {
            dbobj = new BiWeeklyRCPNewEntities();

            var objPS1 = dbobj.Protocol_5_Signature.Where(x => x.SignatureId == id).ToList();
            if (objPS1.Count > 0)
            {
                dbobj.Protocol_5_Signature.Remove(objPS1[0]);
                dbobj.SaveChanges();
            }
        }

        public void deleteContact(int ClientId, int itemId)
        {
            MetaData = new GlobalData();
            dbobj = new BiWeeklyRCPNewEntities();
            ContactModel contactModel = new ContactModel();
            ContactPersonal contactPersonal = new ContactPersonal();
            contactPersonal = dbobj.ContactPersonals.Where(objContactPersonal => objContactPersonal.StudentPersonalId == ClientId &&
                objContactPersonal.ContactPersonalId == itemId).SingleOrDefault();
            contactPersonal.Status = MetaData._StatusFalse;
            dbobj.SaveChanges();

            var ParentData = dbobj.Parents.Where(x => x.ContactPersonalId == itemId).ToList();
            if (ParentData.Count > 0)
            {
                dbobj.Parents.Remove(ParentData[0]);
                dbobj.SaveChanges();
            }
            AddEventModel.CreateSystemEvent("Contact Deleted [" + contactPersonal.LastName + ", " + contactPersonal.FirstName + "]", "Contact Deleted", "This contact was deleted.");
        }


        /// <summary>
        /// The below two functions are used to convert date time to sting.
        /// </summary>
        /// <param name="nullable"></param>
        /// <returns></returns>

        private string ConvertDate(DateTime? nullable)
        {
            string result = "";
            DateTime temp;
            try
            {
                temp = (DateTime)nullable;
                result = temp.ToString("MM/dd/yyyy").Replace('-', '/');
            }
            catch
            {
                result = null;
            }

            return result;
        }

        public string ConvertDate(DateTime dateString)
        {
            string result = "";
            DateTime temp = (DateTime)dateString;
            result = temp.ToString("MM/dd/yyyy").Replace('-', '/');
            return result;
        }



        /// <summary>
        /// Function to Bind Student Image to the right side Panel.
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>

        public ImageModel bindImage(int clientId)
        {
            ClsErrorLog error = new ClsErrorLog();
            ImageModel imgmodel = new ImageModel();
            dbobj = new BiWeeklyRCPNewEntities();
            StudentPersonal student = new StudentPersonal();


            string dirpath = System.Web.Configuration.WebConfigurationManager.AppSettings["ImagessLocation"].ToString();
            try
            {
                student = dbobj.StudentPersonals.Where(x => x.StudentPersonalId == clientId).SingleOrDefault();
                if (student != null)
                {
                    imgmodel.FirstName = student.FirstName;
                    imgmodel.LastName = student.LastName;
                    imgmodel.Suffix = student.Suffix;

                    if (student.ImageUrl == null)
                    {

                    }
                    //error.WriteToLog("Image URL Found ");
                    //error.WriteToLog("Student ID  " + student.ClientId);
                    //error.WriteToLog("Student Name  " + student.LastName + " " + student.FirstName);
                    imgmodel.ImageUrl = student.ImageUrl;
                    //imgmodel.ImageUrl = "../../../"+ imgmodel.ImageUrl.Replace('\\', '/');
                    imgmodel.StudentId = student.ClientId.ToString();
                    imgmodel.PhotoDate = ConvertDate(student.Photodate);
                }
                else
                {
                    error.WriteToLog("NO DATA Student ID  " + clientId);
                }
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
            }

            return imgmodel;
        }

        /// <summary>
        /// Funtion To load Saved Events Details For Edit / View
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        /// 
        public AddEventModel bindEvents(int itemId)
        {
            sess = (clsSession)HttpContext.Current.Session["UserSessionClient"];
            dbobj = new BiWeeklyRCPNewEntities();
            AddEventModel returnModel = new AddEventModel();
            DateTime now = DateTime.Now;
            Event events = new Event();
            if (itemId > 0)
            {
                try
                {
                    events = dbobj.Events.Where(objEvents => objEvents.StudentPersonalId == sess.StudentId && objEvents.EventId == itemId).SingleOrDefault();
                    returnModel.Id = events.EventId;
                    returnModel.EventName = events.EventsName;
                    returnModel.EventStatus = events.EventStatus;
                    returnModel.EventTypes = events.EventType;
                    returnModel.ExpiredOnDate = ConvertDate(events.ExpiredOn);
                    returnModel.EventDate = ConvertDate(events.EventDate);
                    returnModel.Note = events.Note;
                    returnModel.Contact = events.Contact;
                    returnModel.UserName = events.Username;
                    returnModel.IsSystemEvent = events.IsSystemEvent.GetBool();

                }
                catch (DbEntityValidationException dbEx)
                {
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                        }
                    }
                }
            }
            return returnModel;
        }

        public void SaveNewEventData(string strNewEvent, int placementId)
        {
            try
            {
                dbobj = new BiWeeklyRCPNewEntities();
                ClientDB.DbModel.EventLog eventObj = new ClientDB.DbModel.EventLog();
                if (strNewEvent != null && strNewEvent != "")
                {
                    string[] arrEvents = strNewEvent.Split(new string[] { ">>>" }, StringSplitOptions.None);
                    foreach (string x in arrEvents)
                    {
                        if (x != "")
                        {
                            string[] subEvent = x.Split(new string[] { "|||" }, StringSplitOptions.None);
                            string objT = subEvent[0];
                            string objFT = subEvent[1];
                            string objF = subEvent[2];
                            string preV = subEvent[3];
                            string newV = subEvent[4];

                            eventObj.StudentPersonalId = sess.StudentId;
                            eventObj.ObjectTypeId = placementId;
                            eventObj.ObjectType = objT;
                            eventObj.ObjectFieldType = objFT;
                            eventObj.ObjectField = objF;
                            eventObj.PreviousValue = preV;
                            eventObj.NewValue = newV;
                            //eventObj.EventDate = DateTime.Now.ToString("MM/dd/yyyy").Replace("-", "/");
                            eventObj.EventDate = DateTime.Now;
                            eventObj.CreatedBy = 1;
                            eventObj.CreatedOn = DateTime.Now;

                            dbobj.EventLogs.Add(eventObj);
                            dbobj.SaveChanges();
                        }
                    }
                }
            }
            catch (Exception e) {
                ClsErrorLog error = new ClsErrorLog();
                error.WriteToLog("Error:" + e);
            }
        }


        /// <summary>
        /// Function to Save / Update Events Data
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>

        public string SaveEventData(AddEventModel model, bool IsSysEvent = false)
        {
            sess = (clsSession)HttpContext.Current.Session["UserSessionClient"];
            dbobj = new BiWeeklyRCPNewEntities();
            int ClientID = sess.StudentId, SchoolId = sess.SchoolId;
            Event events = new Event();
            if (model.Id > 0)
            {
                try
                {
                    events = dbobj.Events.Where(objEvents => objEvents.EventId == model.Id && objEvents.StudentPersonalId == ClientID).SingleOrDefault();
                    events.EventsName = model.EventName;
                    events.EventType = model.EventTypes;
                    events.EventStatus = model.EventStatus;
                    events.Status = 1;
                    //events.EventDate = DateTime.Now;
                    events.ExpiredOn = DateTime.ParseExact(model.ExpiredOnDate, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    events.ModifiedBy = 1;
                    events.ModifiedOn = DateTime.Now;

                    events.Contact = model.Contact;
                    events.EventDate = (model.EventDate != null && model.EventDate != "") ?
                        DateTime.ParseExact(model.EventDate, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture) : DateTime.Now;
                    events.Username = model.UserName;
                    events.Note = model.Note;
                    events.IsSystemEvent = IsSysEvent;

                    dbobj.SaveChanges();
                    return "Sucess";
                }
                catch
                {
                    return "Failed";
                }
            }
            else
            {
                if (ClientID == 0)
                {
                    return "No Client Selected";
                }
                else
                {
                    try
                    {
                        events.SchoolId = SchoolId;
                        events.EventsName = model.EventName;
                        events.EventType = model.EventTypes;
                        events.EventStatus = model.EventStatus;
                        events.Status = 1;
                        events.StudentPersonalId = ClientID;
                        //events.EventDate = DateTime.Now;
                        if (model.ExpiredOnDate != null)
                        {
                            events.ExpiredOn = DateTime.ParseExact(model.ExpiredOnDate, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        }
                        else
                        {
                            events.ExpiredOn = null;
                        }
                        events.CreatedBy = 1;
                        events.CreatedOn = DateTime.Now;

                        events.Contact = model.Contact;
                        events.EventDate = (model.EventDate != null && model.EventDate != "") ?
                            DateTime.ParseExact(model.EventDate, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture) : DateTime.Now;
                        events.Username = model.UserName;
                        events.Note = model.Note;
                        events.IsSystemEvent = IsSysEvent;

                        dbobj.Events.Add(events);
                        dbobj.SaveChanges();
                        return "Sucess";
                    }
                    catch
                    {
                        return "Failed";
                    }
                }
            }

        }



        /// <summary>
        /// Function to Save / Update Documents Data
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>

        public string SaveForms(AddDocumentModel model, HttpPostedFileBase profilePicture)
        {
            sess = (clsSession)HttpContext.Current.Session["UserSessionClient"];
            //dbobj = new BiWeeklyRCPNewEntities();
            //string dirpath = (AppDomain.CurrentDomain.BaseDirectory + "Forms/".ToString()).Replace('\\', '/');
            //dirpath = System.Web.Configuration.WebConfigurationManager.AppSettings["filesLocation"].ToString();
            // string realPath =  Request.Url.GetLeftPart(UriPartial.Authority) + Request.ApplicationPath;
            int ClientID = sess.StudentId, SchoolId = sess.SchoolId;
            //Document Documents = new Document();

            if (ClientID == 0)
            {
                return "No Client Selected";
            }
            else
            {
                try
                {
                    if (model.DocumentName != "" && model.DocumentName != null)
                    {
                        string contentType = "";
                        int rtrnval = -1;
                        if (profilePicture.ContentType != null) contentType = profilePicture.ContentType;



                        byte[] bytes = null;
                        using (Stream fs = profilePicture.InputStream)
                        {
                            using (BinaryReader br = new BinaryReader(fs))
                            {
                                bytes = br.ReadBytes((Int32)fs.Length);
                            }
                        }


                        dbobj = new BiWeeklyRCPNewEntities();
                        LookUp doctype = new LookUp();
                        doctype = dbobj.LookUps.Where(objlk => objlk.LookupId == model.DocumentType).SingleOrDefault();

                        ClientDB.DbModel.Document tblDoc = new ClientDB.DbModel.Document();
                        binaryFile binfile = new binaryFile();
                        StdtIEP std = new StdtIEP();

                        if (doctype.LookupName != "Other")
                        {
                            tblDoc.Other = doctype.LookupName;
                            tblDoc.DocumentName = model.DocumentName;
                            tblDoc.DocumentType = model.DocumentType;
                            tblDoc.SchoolId = SchoolId;
                            tblDoc.StudentPersonalId = sess.StudentId;
                            tblDoc.Status = true;
                            tblDoc.UserType = "Staff";
                            tblDoc.CreatedBy = sess.LoginId;
                            tblDoc.CreatedOn = System.DateTime.Now;
                            dbobj.Documents.Add(tblDoc);
                            dbobj.SaveChanges();
                            rtrnval = tblDoc.DocumentId;




                            binfile.SchoolId = SchoolId;
                            binfile.StudentId = ClientID;
                            binfile.DocumentName = model.DocumentName;
                            binfile.DocId = rtrnval;
                            binfile.ContentType = contentType;
                            binfile.Data = bytes;
                            binfile.CreatedBy = sess.LoginId;
                            binfile.CreatedOn = DateTime.Now;
                            binfile.type = "Client";
                            binfile.Varified = true;
                            binfile.Active = true;
                            binfile.ModuleName = doctype.LookupName;
                            dbobj.binaryFiles.Add(binfile);
                            dbobj.SaveChanges();
                            rtrnval = binfile.BinaryId;
                        }
                        else
                        {

                            tblDoc.Other = model.Other;
                            tblDoc.DocumentName = model.DocumentName;
                            tblDoc.DocumentType = model.DocumentType;
                            tblDoc.SchoolId = SchoolId;
                            tblDoc.StudentPersonalId = sess.StudentId;
                            tblDoc.Status = true;
                            tblDoc.UserType = "Staff";
                            tblDoc.CreatedBy = sess.LoginId;
                            tblDoc.CreatedOn = System.DateTime.Now;
                            dbobj.Documents.Add(tblDoc);
                            dbobj.SaveChanges();
                            rtrnval = tblDoc.DocumentId;


                            //binaryFile binfile = new binaryFile();

                            binfile.SchoolId = SchoolId;
                            binfile.StudentId = ClientID;
                            binfile.DocumentName = model.DocumentName;
                            binfile.DocId = rtrnval;
                            binfile.ContentType = contentType;
                            binfile.Data = bytes;
                            binfile.CreatedBy = sess.LoginId;
                            binfile.CreatedOn = DateTime.Now;
                            binfile.type = "Client";
                            binfile.Varified = true;
                            binfile.Active = true;
                            binfile.ModuleName = doctype.LookupName;
                            dbobj.binaryFiles.Add(binfile);
                            dbobj.SaveChanges();
                            rtrnval = binfile.BinaryId;

                        }



                        //Documents.SchoolId = SchoolId;
                        //Documents.DocumentName = model.DocumentName;
                        //Documents.DocumentType = model.DocumentType;
                        //Documents.Status = true;
                        //Documents.StudentPersonalId = ClientID;
                        //Documents.UserType = "Melmark User";
                        //Documents.CreatedBy = sess.LoginId;
                        //if (profilePicture != null)
                        //{
                        //    Documents.DocumentPath = profilePicture.FileName;
                        //}
                        //Documents.CreatedOn = DateTime.Now;
                        //dbobj.Documents.Add(Documents);
                        //dbobj.SaveChanges();
                        //if (profilePicture != null)
                        //{
                        //    profilePicture.SaveAs(dirpath + Documents.DocumentId + "-" + profilePicture.FileName);
                        //}
                    }
                    return "Success";
                }
                catch (Exception EX)
                {
                    throw EX;
                    //return "Failed";
                }
            }
        }

        public string SaveDocTabOne(ProgressReport model, HttpPostedFileBase profilePicture1)
        {
            sess = (clsSession)HttpContext.Current.Session["UserSessionClient"];
            int ClientID = sess.StudentId, SchoolId = sess.SchoolId;

            if (ClientID == 0)
            {
                return "No Client Selected";
            }
            else
            {
                try
                {
                    if (model.DocumentName != "" && model.DocumentName != null)
                    {
                        string contentType = "";
                        int rtrnval = -1;
                        if (profilePicture1.ContentType != null) contentType = profilePicture1.ContentType;

                        byte[] bytes = null;
                        using (Stream fs = profilePicture1.InputStream)
                        {
                            using (BinaryReader br = new BinaryReader(fs))
                            {
                                bytes = br.ReadBytes((Int32)fs.Length);
                            }
                        }



                        dbobj = new BiWeeklyRCPNewEntities();

                        ProgressRpt_Doc tblDoc = new ProgressRpt_Doc();

                        tblDoc.SchoolId = SchoolId;
                        tblDoc.StudentId = sess.StudentId;

                        tblDoc.ProgressId = model.ProReportId;
                        tblDoc.TabId = 1;

                        tblDoc.DocumentName = model.DocumentName;
                        tblDoc.ContentType = contentType;
                        tblDoc.Data = bytes;

                        tblDoc.CreatedBy = sess.LoginId;
                        tblDoc.CreatedOn = DateTime.Now;

                        dbobj.ProgressRpt_Doc.Add(tblDoc);
                        dbobj.SaveChanges();
                        rtrnval = tblDoc.DocId;
                    }
                    return "Success";
                }
                catch (Exception EX)
                {
                    throw EX;
                }
            }
        }

        public string SaveDocTabTwo(ProgressReport model, HttpPostedFileBase profilePicture2)
        {
            sess = (clsSession)HttpContext.Current.Session["UserSessionClient"];
            int ClientID = sess.StudentId, SchoolId = sess.SchoolId;

            if (ClientID == 0)
            {
                return "No Client Selected";
            }
            else
            {
                try
                {
                    if (model.DocumentName != "" && model.DocumentName != null)
                    {
                        string contentType = "";
                        int rtrnval = -1;
                        if (profilePicture2.ContentType != null) contentType = profilePicture2.ContentType;

                        byte[] bytes = null;
                        using (Stream fs = profilePicture2.InputStream)
                        {
                            using (BinaryReader br = new BinaryReader(fs))
                            {
                                bytes = br.ReadBytes((Int32)fs.Length);
                            }
                        }

                        dbobj = new BiWeeklyRCPNewEntities();

                        ProgressRpt_Doc tblDoc = new ProgressRpt_Doc();

                        tblDoc.SchoolId = SchoolId;
                        tblDoc.StudentId = sess.StudentId;

                        tblDoc.ProgressId = model.ProReportId;
                        tblDoc.TabId = 2;

                        tblDoc.DocumentName = model.DocumentName;
                        tblDoc.ContentType = contentType;
                        tblDoc.Data = bytes;

                        tblDoc.CreatedBy = sess.LoginId;
                        tblDoc.CreatedOn = DateTime.Now;

                        dbobj.ProgressRpt_Doc.Add(tblDoc);
                        dbobj.SaveChanges();
                        rtrnval = tblDoc.DocId;
                    }
                    return "Success";
                }
                catch (Exception EX)
                {
                    throw EX;
                }
            }
        }




        /// <summary>
        /// Function To delete Document.
        /// </summary>
        /// <param name="ClientId"></param>
        /// <param name="itemId"></param>

        public void deleteDocument(int ClientId, int itemId)
        {
            dbobj = new BiWeeklyRCPNewEntities();
            binaryFile Documentz = new binaryFile();
            Documentz = dbobj.binaryFiles.Where(objDocuments => objDocuments.StudentId == ClientId && objDocuments.BinaryId == itemId).SingleOrDefault();
            if (Documentz != null)
            { Documentz.Active = false; }

            dbobj.SaveChanges();
        }

        public void deleteTab1Document(int itemId)
        {
            dbobj = new BiWeeklyRCPNewEntities();
            ProgressRpt_Doc prDoc = new ProgressRpt_Doc();
            prDoc = dbobj.ProgressRpt_Doc.Where(objDoc => objDoc.TabId == 1 && objDoc.DocId == itemId).SingleOrDefault();
            dbobj.ProgressRpt_Doc.Remove(prDoc);
            dbobj.SaveChanges();
        }

        public void deleteTab2Document(int itemId)
        {
            dbobj = new BiWeeklyRCPNewEntities();
            ProgressRpt_Doc prDoc = new ProgressRpt_Doc();
            prDoc = dbobj.ProgressRpt_Doc.Where(objDoc => objDoc.TabId == 2 && objDoc.DocId == itemId).SingleOrDefault();
            dbobj.ProgressRpt_Doc.Remove(prDoc);
            dbobj.SaveChanges();
        }

        public void deleteProRptList(int itemId)
        {
            dbobj = new BiWeeklyRCPNewEntities();
            ProgressRpt_List prList = new ProgressRpt_List();
            prList = dbobj.ProgressRpt_List.Where(objDoc => objDoc.ProReportId == itemId).SingleOrDefault();
            dbobj.ProgressRpt_List.Remove(prList);
            dbobj.SaveChanges();
        }

        public void deleteTab3List(int itemId)
        {
            dbobj = new BiWeeklyRCPNewEntities();
            ProgressRpt_RTF_Q prList = new ProgressRpt_RTF_Q();
            prList = dbobj.ProgressRpt_RTF_Q.Where(objDoc => objDoc.RTFQId == itemId).SingleOrDefault();
            dbobj.ProgressRpt_RTF_Q.Remove(prList);
            dbobj.SaveChanges();
        }

        public void deleteTab4List(int itemId)
        {
            dbobj = new BiWeeklyRCPNewEntities();
            ProgressRpt_RTF_M prList = new ProgressRpt_RTF_M();
            prList = dbobj.ProgressRpt_RTF_M.Where(objDoc => objDoc.RTFMId == itemId).SingleOrDefault();
            dbobj.ProgressRpt_RTF_M.Remove(prList);
            dbobj.SaveChanges();
        }

        public void deleteTab5List(int itemId)
        {
            dbobj = new BiWeeklyRCPNewEntities();
            ProgressRpt prList = new ProgressRpt();
            prList = dbobj.ProgressRpts.Where(objDoc => objDoc.ProgressId == itemId).SingleOrDefault();
            dbobj.ProgressRpts.Remove(prList);
            dbobj.SaveChanges();
        }


        //public FileContentResult ViewDocument(int documentId)
        //{
        //dbobj = new BiWeeklyRCPNewEntities();
        //binaryFile ObjDoc = new binaryFile();
        //////string dirpath = (AppDomain.CurrentDomain.BaseDirectory + "Forms".ToString()).Replace('\\', '/');
        //string dirpath = System.Web.Configuration.WebConfigurationManager.AppSettings["filesLocation"].ToString();
        //sess = (clsSession)HttpContext.Current.Session["UserSessionClient"];
        ////var attachmenttable = dbobj.Appattachments.Single(x => x.Id == id && x.RefObjectid == userSessionObj.id);
        ////var userdoc = dbobj.AppAttachedFiles.Single(x => x.AttachmentId == attachmenttable.Id);
        //ObjDoc = dbobj.binaryFiles.Where(objDocument => objDocument.BinaryId == documentId && objDocument.StudentId == sess.StudentId).SingleOrDefault();

        //if (ObjDoc != null)
        //{
        //    var documentPath = dirpath + "\\" + ObjDoc.BinaryId + "-" + ObjDoc.DocumentPath;
        //    return documentPath.Replace('\\', '/');

        //}
        //return "Failed";


        //}



        /// <summary>
        /// Function To delete Events Data.
        /// </summary>
        /// <param name="StudentId"></param>
        /// <param name="itemId"></param>

        public void deleteEvents(int ClientId, int itemId)
        {
            dbobj = new BiWeeklyRCPNewEntities();
            Event events = new Event();
            events = dbobj.Events.Where(objEvents => objEvents.StudentPersonalId == ClientId && objEvents.EventId == itemId).SingleOrDefault();
            events.Status = 0;
            dbobj.SaveChanges();
        }



        /// <summary>
        /// Function to Save / Update Contact Detais. Model Is Contact
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>

        public string SaveContactData(ContactModel model)
        {
            string Result = "";
            dbobj = new BiWeeklyRCPNewEntities();
            ContactPersonal stdtContactPersonal = new ContactPersonal();
            AddressList stdtContact = new AddressList();
            StudentAddresRel adrRel = new StudentAddresRel();
            StudentContactRelationship contactRelation = new StudentContactRelationship();
            // HttpContext.Current.Session["val"] = 1;
            sess = (clsSession)HttpContext.Current.Session["UserSessionClient"];
            StudentContactRelationship stdtContactRel = new StudentContactRelationship();
            Parent parent = new Parent();
            StudentParentRel studentParentRel = new StudentParentRel();
            MetaData = new GlobalData();
            var RelationData = dbobj.LookUps.Where(x => x.LookupType == "Relationship" && x.LookupName == "Parent").SingleOrDefault();
            int ClientID = sess.StudentId;
            if (ClientID == 0)
            {
                Result = "No Client Selected";
            }
            else
            {
                try
                {
                    using (TransactionScope trans = new TransactionScope(TransactionScopeOption.Required))
                    {
                        if (model.Id > 0)
                        {
                            stdtContactPersonal = dbobj.ContactPersonals.Where(objContactPersonal => objContactPersonal.StudentPersonalId == ClientID &&
                                objContactPersonal.ContactPersonalId == model.Id).SingleOrDefault();
                            if (stdtContactPersonal != null)
                            {
                                //stdtContactPersonal.StudentPersonalId = ClientID;
                                stdtContactPersonal.Prefix = model.FirstNamePrefix;
                                stdtContactPersonal.FirstName = model.FirstName;
                                stdtContactPersonal.LastName = model.LastName;
                                stdtContactPersonal.Suffix = model.LastNameSuffix;
                                stdtContactPersonal.MiddleName = model.MiddleName;
                                stdtContactPersonal.Spouse = model.Spouse;
                                stdtContactPersonal.PrimaryLanguage = model.PrimaryLanguage;
                                stdtContactPersonal.ModifiedBy = sess.LoginId;
                                stdtContactPersonal.ModifiedOn = System.DateTime.Now;

                                stdtContactPersonal.IsEmergency = model.IsEmergencyP || model.IsEmergencyS
                                                                || model.IsEmergencyT || model.IsEmergencyO;
                                stdtContactPersonal.IsEmergencyP = model.IsEmergencyP;
                                stdtContactPersonal.IsEmergencyS = model.IsEmergencyS;
                                stdtContactPersonal.IsEmergencyT = model.IsEmergencyT;
                                stdtContactPersonal.IsEmergencyO = model.IsEmergencyO;
                                stdtContactPersonal.IsBilling = model.IsBilling;
                                stdtContactPersonal.IsIncident = model.IsIncident;
                                stdtContactPersonal.IsSchool = model.IsSchool;
                                stdtContactPersonal.Note = model.Note;
                                stdtContactPersonal.SpouseId = model.SpouseId;
                                stdtContactPersonal.IsOnCampusAlone = model.IsOnCampusAlone;
                                stdtContactPersonal.IsOnCampusWithStaff = model.IsOnCampusWithStaff;
                                stdtContactPersonal.IsOffCampus = model.IsOffCampus;

                                stdtContactPersonal.IsCorrespondence = model.IsCorrespondence;
                                stdtContactPersonal.IsCustody = model.IsCustody;
                                stdtContactPersonal.IsGuardian = model.IsGuardian;
                                stdtContactPersonal.IsNextOfKin = model.IsNextOfKin;
                                stdtContactPersonal.ApprovedVisitor = model.ApprovedVisitor;
                                stdtContactPersonal.Occupation = model.Occupation;
                                stdtContactPersonal.Employer = model.Employer;

                                // dbobj.ContactPersonals.Add(stdtContactPersonal);
                                dbobj.SaveChanges();
                                //if (stdtContactPersonal.ContactFlag == "Referral")
                                //{

                                //}
                                //else
                                //{
                                contactRelation = dbobj.StudentContactRelationships.Where(objStudentContactRel => objStudentContactRel.ContactPersonalId == model.Id).SingleOrDefault();
                                contactRelation.RelationshipId = Convert.ToInt32(model.Relation);
                                contactRelation.ModifiedBy = sess.LoginId;
                                contactRelation.ModifiedOn = System.DateTime.Now;
                                dbobj.SaveChanges();
                                //}

                                var addresses = dbobj.StudentAddresRels.Where(x => x.ContactPersonalId == model.Id && x.ContactSequence == 1).ToList().OrderBy(x => x.ContactSequence).SingleOrDefault();

                                if (addresses != null)
                                {
                                    stdtContact = dbobj.AddressLists.Where(objAddressList => objAddressList.AddressId == addresses.AddressId).SingleOrDefault();
                                    stdtContact.AddressType = model.HomeAddressTypeId;
                                    stdtContact.ApartmentType = model.HomeAddressLine1;
                                    stdtContact.StreetName = model.HomeAddressLine2;
                                    stdtContact.AddressLine3 = model.HomeAddressLine3;
                                    stdtContact.City = model.HomeCity;
                                    stdtContact.StateProvince = model.HomeState;
                                    stdtContact.CountryId = model.HomeCountry;
                                    stdtContact.Phone = model.HomePhone;
                                    stdtContact.Mobile = model.HomeMobilePhone;
                                    stdtContact.OtherPhone = model.HomeWorkPhone;
                                    stdtContact.Fax = model.HomeFax;
                                    stdtContact.County = model.HomeCounty;
                                    stdtContact.PrimaryEmail = model.HomeEmail;
                                    stdtContact.EmailOther = model.HomeOtherMail;
                                    stdtContact.SecondryEmail = model.HomeWorkEmail;
                                    stdtContact.PostalCode = model.HomeZip;
                                    stdtContact.ModifiedBy = sess.LoginId;
                                    stdtContact.ModifiedOn = System.DateTime.Now;

                                    stdtContact.IsMailingAddress = model.HomeIsMailingAddress;
                                    stdtContact.Extension = model.HomeExtension;
                                    stdtContact.MailOptIn = model.HomeMailOptIn;
                                    stdtContact.EmailOptIn = model.HomeEmailOptIn;
                                    stdtContact.PhoneOptIn = model.HomePhoneOptIn;
                                    stdtContact.MailMergeOptIn = model.HomeMailMergeOptIn;

                                    dbobj.SaveChanges();


                                    adrRel = dbobj.StudentAddresRels.Where(objRel => objRel.AddressId == stdtContact.AddressId).SingleOrDefault();
                                    if (adrRel != null)
                                    {
                                        adrRel.AddressId = stdtContact.AddressId;
                                        adrRel.StudentPersonalId = ClientID;
                                        adrRel.ContactPersonalId = stdtContactPersonal.ContactPersonalId;
                                        adrRel.ContactSequence = 1;
                                        adrRel.CreatedBy = sess.LoginId;
                                        adrRel.CreatedOn = DateTime.Now;
                                        dbobj.SaveChanges();
                                    }
                                    else
                                    {
                                        adrRel = new StudentAddresRel();
                                        adrRel.AddressId = stdtContact.AddressId;
                                        adrRel.StudentPersonalId = ClientID;
                                        adrRel.ContactPersonalId = stdtContactPersonal.ContactPersonalId;
                                        adrRel.ContactSequence = 1;
                                        adrRel.CreatedBy = sess.LoginId;
                                        adrRel.CreatedOn = DateTime.Now;
                                        dbobj.StudentAddresRels.Add(adrRel);
                                        dbobj.SaveChanges();

                                    }
                                }
                                else
                                {

                                    stdtContact.AddressType = model.HomeAddressTypeId;
                                    stdtContact.ApartmentType = model.HomeAddressLine1;
                                    stdtContact.StreetName = model.HomeAddressLine2;
                                    stdtContact.AddressLine3 = model.HomeAddressLine3;
                                    stdtContact.City = model.HomeCity;
                                    stdtContact.StateProvince = model.HomeState;
                                    stdtContact.CountryId = model.HomeCountry;
                                    stdtContact.Phone = model.HomePhone;
                                    stdtContact.Mobile = model.HomeMobilePhone;
                                    stdtContact.OtherPhone = model.HomeWorkPhone;
                                    stdtContact.Fax = model.HomeFax;
                                    stdtContact.County = model.HomeCounty;
                                    stdtContact.PrimaryEmail = model.HomeEmail;
                                    stdtContact.EmailOther = model.HomeOtherMail;
                                    stdtContact.SecondryEmail = model.HomeWorkEmail;
                                    stdtContact.PostalCode = model.HomeZip;
                                    stdtContact.ModifiedBy = sess.LoginId;
                                    stdtContact.ModifiedOn = System.DateTime.Now;

                                    stdtContact.IsMailingAddress = model.HomeIsMailingAddress;
                                    stdtContact.Extension = model.HomeExtension;
                                    stdtContact.MailOptIn = model.HomeMailOptIn;
                                    stdtContact.EmailOptIn = model.HomeEmailOptIn;
                                    stdtContact.PhoneOptIn = model.HomePhoneOptIn;
                                    stdtContact.MailMergeOptIn = model.HomeMailMergeOptIn;

                                    dbobj.AddressLists.Add(stdtContact);
                                    dbobj.SaveChanges();


                                    adrRel = dbobj.StudentAddresRels.Where(objRel => objRel.AddressId == stdtContact.AddressId).SingleOrDefault();
                                    if (adrRel != null)
                                    {
                                        adrRel.AddressId = stdtContact.AddressId;
                                        adrRel.StudentPersonalId = ClientID;
                                        adrRel.ContactPersonalId = stdtContactPersonal.ContactPersonalId;
                                        adrRel.ContactSequence = 1;
                                        adrRel.CreatedBy = sess.LoginId;
                                        adrRel.CreatedOn = DateTime.Now;
                                        dbobj.SaveChanges();
                                    }
                                    else
                                    {
                                        adrRel = new StudentAddresRel();
                                        adrRel.AddressId = stdtContact.AddressId;
                                        adrRel.StudentPersonalId = ClientID;
                                        adrRel.ContactPersonalId = stdtContactPersonal.ContactPersonalId;
                                        adrRel.ContactSequence = 1;
                                        adrRel.CreatedBy = sess.LoginId;
                                        adrRel.CreatedOn = DateTime.Now;
                                        dbobj.StudentAddresRels.Add(adrRel);
                                        dbobj.SaveChanges();

                                    }
                                }

                                addresses = dbobj.StudentAddresRels.Where(x => x.ContactPersonalId == model.Id && x.ContactSequence == 2).ToList().OrderBy(x => x.ContactSequence).SingleOrDefault();

                                if (addresses != null)
                                {
                                    stdtContact = dbobj.AddressLists.Where(objAddressList => objAddressList.AddressId == addresses.AddressId).SingleOrDefault();
                                    stdtContact.AddressType = model.WorkAddressTypeId;
                                    stdtContact.ApartmentType = model.WorkAddressLine1;
                                    stdtContact.StreetName = model.WorkAddressLine2;
                                    stdtContact.AddressLine3 = model.WorkAddressLine3;
                                    // stdtContact.StreetName = model.HomeStreet;
                                    stdtContact.ApartmentNumber = model.HomeNumber;
                                    stdtContact.City = model.WorkCity;
                                    stdtContact.StateProvince = model.WorkState;
                                    stdtContact.CountryId = model.WorkCountry;
                                    stdtContact.Phone = model.WorkHomePhone;
                                    stdtContact.Mobile = model.WorkMobilePhone;
                                    stdtContact.OtherPhone = model.WorkPhone;
                                    stdtContact.Fax = model.WorkFax;
                                    stdtContact.County = model.WorkCounty;
                                    //stdtContact.PrimaryEmail = model.WorkHomeEmail;
                                    //stdtContact.SecondryEmail = model.WorkEmail;
                                    stdtContact.PrimaryEmail = model.WorkEmail;
                                    stdtContact.PostalCode = model.WorkZip;
                                    stdtContact.ModifiedBy = sess.LoginId;
                                    stdtContact.ModifiedOn = System.DateTime.Now;

                                    stdtContact.IsMailingAddress = model.WorkIsMailingAddress;
                                    stdtContact.Extension = model.WorkExtension;
                                    stdtContact.Extension2 = model.OtherExtension2;
                                    stdtContact.MailOptIn = model.WorkMailOptIn;
                                    stdtContact.EmailOptIn = model.WorkMailOptIn;
                                    stdtContact.PhoneOptIn = model.WorkPhoneOptIn;
                                    stdtContact.MailMergeOptIn = model.WorkMailMergeOptIn;

                                    dbobj.SaveChanges();


                                    adrRel = dbobj.StudentAddresRels.Where(objRel => objRel.AddressId == stdtContact.AddressId).SingleOrDefault();
                                    if (adrRel != null)
                                    {
                                        adrRel.AddressId = stdtContact.AddressId;
                                        adrRel.StudentPersonalId = ClientID;
                                        adrRel.ContactPersonalId = stdtContactPersonal.ContactPersonalId;
                                        adrRel.ContactSequence = 2;
                                        adrRel.CreatedBy = sess.LoginId;
                                        adrRel.CreatedOn = DateTime.Now;
                                        dbobj.SaveChanges();
                                    }
                                    else
                                    {
                                        adrRel = new StudentAddresRel();
                                        adrRel.AddressId = stdtContact.AddressId;
                                        adrRel.StudentPersonalId = ClientID;
                                        adrRel.ContactPersonalId = stdtContactPersonal.ContactPersonalId;
                                        adrRel.ContactSequence = 2;
                                        adrRel.CreatedBy = sess.LoginId;
                                        adrRel.CreatedOn = DateTime.Now;
                                        dbobj.StudentAddresRels.Add(adrRel);
                                        dbobj.SaveChanges();

                                    }
                                }
                                else
                                {
                                    stdtContact.AddressType = model.WorkAddressTypeId;
                                    stdtContact.ApartmentType = model.WorkAddressLine1;
                                    stdtContact.StreetName = model.WorkAddressLine2;
                                    stdtContact.AddressLine3 = model.WorkAddressLine3;
                                    // stdtContact.StreetName = model.HomeStreet;
                                    stdtContact.ApartmentNumber = model.HomeNumber;
                                    stdtContact.City = model.WorkCity;
                                    stdtContact.StateProvince = model.WorkState;
                                    stdtContact.CountryId = model.WorkCountry;
                                    stdtContact.Phone = model.WorkHomePhone;
                                    stdtContact.Mobile = model.WorkMobilePhone;
                                    stdtContact.OtherPhone = model.WorkPhone;
                                    stdtContact.Fax = model.WorkFax;
                                    stdtContact.County = model.WorkCounty;
                                    //stdtContact.PrimaryEmail = model.WorkHomeEmail;
                                    //stdtContact.SecondryEmail = model.WorkEmail;
                                    stdtContact.PrimaryEmail = model.WorkEmail;
                                    stdtContact.PostalCode = model.WorkZip;
                                    stdtContact.ModifiedBy = sess.LoginId;
                                    stdtContact.ModifiedOn = System.DateTime.Now;

                                    stdtContact.IsMailingAddress = model.WorkIsMailingAddress;
                                    stdtContact.Extension = model.WorkExtension;
                                    stdtContact.Extension2 = model.OtherExtension2;
                                    stdtContact.MailOptIn = model.WorkMailOptIn;
                                    stdtContact.EmailOptIn = model.WorkMailOptIn;
                                    stdtContact.PhoneOptIn = model.WorkPhoneOptIn;
                                    stdtContact.MailMergeOptIn = model.WorkMailMergeOptIn;

                                    dbobj.AddressLists.Add(stdtContact);
                                    dbobj.SaveChanges();


                                    adrRel = dbobj.StudentAddresRels.Where(objRel => objRel.AddressId == stdtContact.AddressId).SingleOrDefault();
                                    if (adrRel != null)
                                    {
                                        adrRel.AddressId = stdtContact.AddressId;
                                        adrRel.StudentPersonalId = ClientID;
                                        adrRel.ContactPersonalId = stdtContactPersonal.ContactPersonalId;
                                        adrRel.ContactSequence = 2;
                                        adrRel.CreatedBy = sess.LoginId;
                                        adrRel.CreatedOn = DateTime.Now;
                                        dbobj.SaveChanges();
                                    }
                                    else
                                    {
                                        adrRel = new StudentAddresRel();
                                        adrRel.AddressId = stdtContact.AddressId;
                                        adrRel.StudentPersonalId = ClientID;
                                        adrRel.ContactPersonalId = stdtContactPersonal.ContactPersonalId;
                                        adrRel.ContactSequence = 2;
                                        adrRel.CreatedBy = sess.LoginId;
                                        adrRel.CreatedOn = DateTime.Now;
                                        dbobj.StudentAddresRels.Add(adrRel);
                                        dbobj.SaveChanges();

                                    }
                                }
                                addresses = dbobj.StudentAddresRels.Where(x => x.ContactPersonalId == model.Id && x.ContactSequence == 3).ToList().OrderBy(x => x.ContactSequence).SingleOrDefault();

                                if (addresses != null)
                                {
                                    stdtContact = dbobj.AddressLists.Where(objAddressList => objAddressList.AddressId == addresses.AddressId).SingleOrDefault();
                                    stdtContact.AddressType = model.OtherAddressTypeId;
                                    stdtContact.ApartmentType = model.OtherAddressLine1;
                                    stdtContact.StreetName = model.OtherAddressLine2;
                                    stdtContact.AddressLine3 = model.OtherAddressLine3;
                                    //stdtContact.StreetName = model.HomeStreet;
                                    stdtContact.ApartmentNumber = model.HomeNumber;
                                    stdtContact.City = model.OtherCity;
                                    stdtContact.StateProvince = model.OtherState;
                                    stdtContact.CountryId = model.OtherCountry;
                                    stdtContact.Phone = model.OtherHomePhone;
                                    stdtContact.Mobile = model.OtherMobilePhone;
                                    stdtContact.OtherPhone = model.OtherWorkPhone;
                                    stdtContact.Fax = model.OtherFax;
                                    stdtContact.County = model.OtherCounty;
                                    stdtContact.PrimaryEmail = model.OtherHomeEmail;
                                    stdtContact.SecondryEmail = model.OtherWorkEmail;
                                    stdtContact.PostalCode = model.OtherZip;
                                    stdtContact.ModifiedBy = sess.LoginId;
                                    stdtContact.ModifiedOn = System.DateTime.Now;

                                    stdtContact.IsMailingAddress = model.OtherIsMailingAddress;
                                    stdtContact.Extension = model.OtherExtension;
                                    stdtContact.Extension2 = model.OtherExtension3;
                                    stdtContact.MailOptIn = model.OtherMailOptIn;
                                    stdtContact.EmailOptIn = model.OtherMailOptIn;
                                    stdtContact.PhoneOptIn = model.OtherPhoneOptIn;
                                    stdtContact.MailMergeOptIn = model.OtherMailMergeOptIn;

                                    dbobj.SaveChanges();


                                    adrRel = dbobj.StudentAddresRels.Where(objRel => objRel.AddressId == stdtContact.AddressId).SingleOrDefault();
                                    if (adrRel != null)
                                    {
                                        adrRel.AddressId = stdtContact.AddressId;
                                        adrRel.StudentPersonalId = ClientID;
                                        adrRel.ContactPersonalId = stdtContactPersonal.ContactPersonalId;
                                        adrRel.ContactSequence = 3;
                                        adrRel.CreatedBy = sess.LoginId;
                                        adrRel.CreatedOn = DateTime.Now;
                                        dbobj.SaveChanges();
                                    }
                                    else
                                    {
                                        adrRel = new StudentAddresRel();
                                        adrRel.AddressId = stdtContact.AddressId;
                                        adrRel.StudentPersonalId = ClientID;
                                        adrRel.ContactPersonalId = stdtContactPersonal.ContactPersonalId;
                                        adrRel.ContactSequence = 3;
                                        adrRel.CreatedBy = sess.LoginId;
                                        adrRel.CreatedOn = DateTime.Now;
                                        dbobj.StudentAddresRels.Add(adrRel);
                                        dbobj.SaveChanges();

                                    }
                                }
                                else
                                {
                                    stdtContact.AddressType = model.OtherAddressTypeId;
                                    stdtContact.ApartmentType = model.OtherAddressLine1;
                                    stdtContact.StreetName = model.OtherAddressLine2;
                                    stdtContact.AddressLine3 = model.OtherAddressLine3;
                                    //stdtContact.StreetName = model.HomeStreet;
                                    stdtContact.ApartmentNumber = model.HomeNumber;
                                    stdtContact.City = model.OtherCity;
                                    stdtContact.StateProvince = model.OtherState;
                                    stdtContact.CountryId = model.OtherCountry;
                                    stdtContact.Phone = model.OtherHomePhone;
                                    stdtContact.Mobile = model.OtherMobilePhone;
                                    stdtContact.OtherPhone = model.OtherWorkPhone;
                                    stdtContact.Fax = model.OtherFax;
                                    stdtContact.County = model.OtherCounty;
                                    stdtContact.PrimaryEmail = model.OtherHomeEmail;
                                    stdtContact.SecondryEmail = model.OtherWorkEmail;
                                    stdtContact.PostalCode = model.OtherZip;
                                    stdtContact.ModifiedBy = sess.LoginId;
                                    stdtContact.ModifiedOn = System.DateTime.Now;

                                    stdtContact.IsMailingAddress = model.OtherIsMailingAddress;
                                    stdtContact.Extension = model.OtherExtension;
                                    stdtContact.Extension2 = model.OtherExtension3;
                                    stdtContact.MailOptIn = model.OtherMailOptIn;
                                    stdtContact.EmailOptIn = model.OtherMailOptIn;
                                    stdtContact.PhoneOptIn = model.OtherPhoneOptIn;
                                    stdtContact.MailMergeOptIn = model.OtherMailMergeOptIn;

                                    dbobj.AddressLists.Add(stdtContact);
                                    dbobj.SaveChanges();


                                    adrRel = dbobj.StudentAddresRels.Where(objRel => objRel.AddressId == stdtContact.AddressId).SingleOrDefault();
                                    if (adrRel != null)
                                    {
                                        adrRel.AddressId = stdtContact.AddressId;
                                        adrRel.StudentPersonalId = ClientID;
                                        adrRel.ContactPersonalId = stdtContactPersonal.ContactPersonalId;
                                        adrRel.ContactSequence = 3;
                                        adrRel.CreatedBy = sess.LoginId;
                                        adrRel.CreatedOn = DateTime.Now;
                                        dbobj.SaveChanges();
                                    }
                                    else
                                    {
                                        adrRel = new StudentAddresRel();
                                        adrRel.AddressId = stdtContact.AddressId;
                                        adrRel.StudentPersonalId = ClientID;
                                        adrRel.ContactPersonalId = stdtContactPersonal.ContactPersonalId;
                                        adrRel.ContactSequence = 3;
                                        adrRel.CreatedBy = sess.LoginId;
                                        adrRel.CreatedOn = DateTime.Now;
                                        dbobj.StudentAddresRels.Add(adrRel);
                                        dbobj.SaveChanges();

                                    }
                                }

                                if (Convert.ToInt32(model.Relation) == RelationData.LookupId)
                                {
                                    //var ParentStudentRelData = dbobj.StudentParentRels.Where(x => x.StudentPersonalId == sess.StudentId).ToList();
                                    //if (ParentStudentRelData.Count > 0)
                                    //{
                                    var ParentData = dbobj.Parents.Where(x => x.ContactPersonalId == stdtContactPersonal.ContactPersonalId).ToList();
                                    if (ParentData.Count > 0)
                                    {
                                        ParentData[0].Fname = model.FirstName;
                                        ParentData[0].Lname = model.LastName;
                                        //ParentData[0].Username = model.UserID;
                                        //ParentData[0].Password = model.LastName;
                                        ParentData[0].ContactPersonalId = stdtContactPersonal.ContactPersonalId;
                                        ParentData[0].ModifiedBy = sess.LoginId;
                                        ParentData[0].ModifiedOn = System.DateTime.Now;
                                        dbobj.SaveChanges();
                                    }
                                    //}
                                    else
                                    {
                                        parent.Fname = model.FirstName;
                                        parent.Lname = model.LastName;
                                        parent.Username = model.UserID;
                                        parent.Password = model.LastName;
                                        parent.ContactPersonalId = stdtContactPersonal.ContactPersonalId;
                                        parent.CreatedBy = sess.LoginId;
                                        parent.CreatedOn = System.DateTime.Now;
                                        dbobj.Parents.Add(parent);
                                        dbobj.SaveChanges();
                                        int parentid = parent.ParentID;

                                        studentParentRel.StudentPersonalId = sess.StudentId;
                                        studentParentRel.ParentID = parentid;
                                        studentParentRel.CreatedBy = sess.LoginId;
                                        studentParentRel.CreatedOn = System.DateTime.Now;
                                        dbobj.StudentParentRels.Add(studentParentRel);
                                        dbobj.SaveChanges();
                                    }
                                }
                                else
                                {
                                    var ParentData = dbobj.Parents.Where(x => x.ContactPersonalId == stdtContactPersonal.ContactPersonalId).ToList();
                                    if (ParentData.Count > 0)
                                    {
                                        dbobj.Parents.Remove(ParentData[0]);
                                        dbobj.SaveChanges();
                                    }
                                }
                            }
                            SaveNewEventData(model.newEventLog, model.Id);
                            AddEventModel.CreateSystemEvent("Contact Updated [" + model.LastName + ", " + model.FirstName + "]", "Contact Updated", model.eventLogNote);
                        }
                        else
                        {
                            stdtContactPersonal = new ContactPersonal();
                            stdtContactPersonal.StudentPersonalId = ClientID;
                            stdtContactPersonal.Prefix = model.FirstNamePrefix;
                            stdtContactPersonal.FirstName = model.FirstName;
                            stdtContactPersonal.LastName = model.LastName;
                            stdtContactPersonal.Suffix = model.LastNameSuffix;
                            stdtContactPersonal.MiddleName = model.MiddleName;
                            stdtContactPersonal.Spouse = model.Spouse;
                            stdtContactPersonal.ContactFlag = "Client";
                            stdtContactPersonal.PrimaryLanguage = model.PrimaryLanguage;
                            stdtContactPersonal.Status = MetaData._StatusTrue;
                            stdtContactPersonal.CreatedBy = sess.LoginId;
                            stdtContactPersonal.CreatedOn = System.DateTime.Now;

                            stdtContactPersonal.IsEmergency = model.IsEmergencyP || model.IsEmergencyS || model.IsEmergencyT || model.IsEmergencyO;
                            stdtContactPersonal.IsEmergencyP = model.IsEmergencyP;
                            stdtContactPersonal.IsEmergencyS = model.IsEmergencyS;
                            stdtContactPersonal.IsEmergencyT = model.IsEmergencyT;
                            stdtContactPersonal.IsEmergencyO = model.IsEmergencyO;
                            stdtContactPersonal.IsBilling = model.IsBilling;
                            stdtContactPersonal.IsIncident = model.IsIncident;
                            stdtContactPersonal.IsSchool = model.IsSchool;
                            stdtContactPersonal.Note = model.Note;
                            stdtContactPersonal.SpouseId = model.SpouseId;
                            stdtContactPersonal.IsOnCampusAlone = model.IsOnCampusAlone;
                            stdtContactPersonal.IsOnCampusWithStaff = model.IsOnCampusWithStaff;
                            stdtContactPersonal.IsOffCampus = model.IsOffCampus;

                            stdtContactPersonal.IsCorrespondence = model.IsCorrespondence;
                            stdtContactPersonal.IsCustody = model.IsCustody;
                            stdtContactPersonal.IsGuardian = model.IsGuardian;
                            stdtContactPersonal.IsNextOfKin = model.IsNextOfKin;
                            stdtContactPersonal.ApprovedVisitor = model.ApprovedVisitor;

                            stdtContactPersonal.Occupation = model.Occupation;
                            stdtContactPersonal.Employer = model.Employer;

                            dbobj.ContactPersonals.Add(stdtContactPersonal);
                            dbobj.SaveChanges();

                            stdtContact.AddressType = model.HomeAddressTypeId;
                            stdtContact.ApartmentType = model.HomeAddressLine1;
                            stdtContact.StreetName = model.HomeAddressLine2;
                            stdtContact.AddressLine3 = model.HomeAddressLine3;
                            //  stdtContact.StreetName = model.HomeStreet;
                            stdtContact.ApartmentNumber = model.HomeNumber;
                            stdtContact.City = model.HomeCity;
                            stdtContact.StateProvince = model.HomeState;
                            stdtContact.CountryId = model.HomeCountry;
                            stdtContact.Phone = model.HomePhone;
                            stdtContact.Mobile = model.HomeMobilePhone;
                            stdtContact.OtherPhone = model.HomeWorkPhone;
                            stdtContact.Fax = model.HomeFax;
                            stdtContact.County = model.HomeCounty;
                            stdtContact.PrimaryEmail = model.HomeEmail;
                            stdtContact.EmailOther = model.HomeOtherMail;
                            stdtContact.SecondryEmail = model.HomeWorkEmail;
                            stdtContact.PostalCode = model.HomeZip;
                            stdtContact.CreatedBy = sess.LoginId;
                            stdtContact.CreatedOn = System.DateTime.Now;

                            stdtContact.IsMailingAddress = model.HomeIsMailingAddress;
                            stdtContact.Extension = model.HomeExtension;
                            stdtContact.MailOptIn = model.HomeMailOptIn;
                            stdtContact.EmailOptIn = model.HomeEmailOptIn;
                            stdtContact.PhoneOptIn = model.HomePhoneOptIn;
                            stdtContact.MailMergeOptIn = model.HomeMailMergeOptIn;

                            dbobj.AddressLists.Add(stdtContact);
                            dbobj.SaveChanges();

                            adrRel.AddressId = stdtContact.AddressId;
                            adrRel.StudentPersonalId = ClientID;
                            adrRel.ContactPersonalId = stdtContactPersonal.ContactPersonalId;
                            adrRel.ContactSequence = 1;
                            adrRel.CreatedBy = sess.LoginId;
                            adrRel.CreatedOn = DateTime.Now;
                            dbobj.StudentAddresRels.Add(adrRel);
                            dbobj.SaveChanges();

                            if (model.WorkAddressTypeId == null)
                            {
                                stdtContact.AddressType = 2;
                            }
                            stdtContact.AddressType = model.WorkAddressTypeId;
                            stdtContact.ApartmentType = model.WorkAddressLine1;
                            stdtContact.StreetName = model.WorkAddressLine2;
                            stdtContact.AddressLine3 = model.WorkAddressLine3;
                            // stdtContact.StreetName = model.HomeStreet;
                            stdtContact.ApartmentNumber = model.HomeNumber;
                            stdtContact.City = model.WorkCity;
                            stdtContact.StateProvince = model.WorkState;
                            stdtContact.CountryId = model.WorkCountry;
                            stdtContact.Phone = model.WorkHomePhone;
                            stdtContact.Mobile = model.WorkMobilePhone;
                            stdtContact.OtherPhone = model.WorkPhone;
                            stdtContact.Fax = model.WorkFax;
                            stdtContact.County = model.WorkCounty;
                            //stdtContact.PrimaryEmail = model.WorkHomeEmail;
                            //stdtContact.SecondryEmail = model.WorkEmail;
                            stdtContact.PrimaryEmail = model.WorkEmail;
                            stdtContact.PostalCode = model.WorkZip;
                            stdtContact.CreatedBy = sess.LoginId;
                            stdtContact.CreatedOn = System.DateTime.Now;

                            stdtContact.IsMailingAddress = model.WorkIsMailingAddress;
                            stdtContact.Extension = model.WorkExtension;
                            stdtContact.Extension2 = model.OtherExtension2;
                            stdtContact.MailOptIn = model.WorkMailOptIn;
                            stdtContact.EmailOptIn = model.WorkEmailOptIn;
                            stdtContact.PhoneOptIn = model.WorkPhoneOptIn;
                            stdtContact.MailMergeOptIn = model.WorkMailMergeOptIn;

                            dbobj.AddressLists.Add(stdtContact);
                            dbobj.SaveChanges();

                            adrRel.AddressId = stdtContact.AddressId;
                            adrRel.StudentPersonalId = ClientID;
                            adrRel.ContactPersonalId = stdtContactPersonal.ContactPersonalId;
                            adrRel.ContactSequence = 2;
                            adrRel.CreatedBy = sess.LoginId;
                            adrRel.CreatedOn = DateTime.Now;
                            dbobj.StudentAddresRels.Add(adrRel);
                            dbobj.SaveChanges();

                            if (model.OtherAddressTypeId == null)
                            {
                                stdtContact.AddressType = 2;
                            }
                            stdtContact.AddressType = model.OtherAddressTypeId;
                            stdtContact.ApartmentType = model.OtherAddressLine1;
                            stdtContact.StreetName = model.OtherAddressLine2;
                            stdtContact.AddressLine3 = model.OtherAddressLine3;
                            // stdtContact.StreetName = model.HomeStreet;
                            stdtContact.ApartmentNumber = model.HomeNumber;
                            stdtContact.City = model.OtherCity;
                            stdtContact.StateProvince = model.OtherState;
                            stdtContact.CountryId = model.OtherCountry;
                            stdtContact.Phone = model.OtherHomePhone;
                            stdtContact.Mobile = model.OtherMobilePhone;
                            stdtContact.OtherPhone = model.OtherWorkPhone;
                            stdtContact.Fax = model.OtherFax;
                            stdtContact.County = model.OtherCounty;
                            stdtContact.PrimaryEmail = model.OtherHomeEmail;
                            stdtContact.PostalCode = model.OtherZip;
                            stdtContact.SecondryEmail = model.OtherWorkEmail;
                            stdtContact.CreatedBy = sess.LoginId;
                            stdtContact.CreatedOn = System.DateTime.Now;

                            stdtContact.IsMailingAddress = model.OtherIsMailingAddress;
                            stdtContact.Extension = model.OtherExtension;
                            stdtContact.Extension2 = model.OtherExtension3;
                            stdtContact.MailOptIn = model.OtherMailOptIn;
                            stdtContact.EmailOptIn = model.OtherEmailOptIn;
                            stdtContact.PhoneOptIn = model.OtherPhoneOptIn;
                            stdtContact.MailMergeOptIn = model.OtherMailMergeOptIn;

                            dbobj.AddressLists.Add(stdtContact);
                            dbobj.SaveChanges();

                            adrRel.AddressId = stdtContact.AddressId;
                            adrRel.StudentPersonalId = ClientID;
                            adrRel.ContactPersonalId = stdtContactPersonal.ContactPersonalId;
                            adrRel.ContactSequence = 3;
                            adrRel.CreatedBy = sess.LoginId;
                            adrRel.CreatedOn = DateTime.Now;
                            dbobj.StudentAddresRels.Add(adrRel);
                            dbobj.SaveChanges();

                            stdtContactRel.ContactPersonalId = stdtContactPersonal.ContactPersonalId;
                            stdtContactRel.RelationshipId = Convert.ToInt32(model.Relation);
                            stdtContactRel.CreatedBy = sess.LoginId;
                            stdtContactRel.CreatedOn = System.DateTime.Now;
                            dbobj.StudentContactRelationships.Add(stdtContactRel);
                            dbobj.SaveChanges();
                            if (Convert.ToInt32(model.Relation) == RelationData.LookupId)
                            {
                                //var ParentStudentRelData = dbobj.StudentParentRels.Where(x => x.StudentPersonalId == sess.StudentId).ToList();
                                //if (ParentStudentRelData.Count > 0)
                                //{
                                var ParentData = dbobj.Parents.Where(x => x.ContactPersonalId == stdtContactPersonal.ContactPersonalId).ToList();
                                if (ParentData.Count > 0)
                                {
                                    ParentData[0].Fname = model.FirstName;
                                    ParentData[0].Lname = model.LastName;
                                    //ParentData[0].Username = model.FirstName;
                                    //ParentData[0].Password = model.LastName;
                                    ParentData[0].ContactPersonalId = stdtContactPersonal.ContactPersonalId;
                                    ParentData[0].ModifiedBy = sess.LoginId;
                                    ParentData[0].ModifiedOn = System.DateTime.Now;
                                    dbobj.SaveChanges();
                                }
                                //}
                                else
                                {
                                    parent.Fname = model.FirstName;
                                    parent.Lname = model.LastName;
                                    parent.Username = model.UserID;
                                    parent.Password = model.LastName;
                                    parent.ContactPersonalId = stdtContactPersonal.ContactPersonalId;
                                    parent.CreatedBy = sess.LoginId;
                                    parent.CreatedOn = System.DateTime.Now;
                                    dbobj.Parents.Add(parent);
                                    dbobj.SaveChanges();
                                    int parentid = parent.ParentID;

                                    studentParentRel.StudentPersonalId = sess.StudentId;
                                    studentParentRel.ParentID = parentid;
                                    studentParentRel.CreatedBy = sess.LoginId;
                                    studentParentRel.CreatedOn = System.DateTime.Now;
                                    dbobj.StudentParentRels.Add(studentParentRel);
                                    dbobj.SaveChanges();
                                }
                            }
                            else
                            {
                                var ParentData = dbobj.Parents.Where(x => x.ContactPersonalId == stdtContactPersonal.ContactPersonalId).ToList();
                                if (ParentData.Count > 0)
                                {
                                    dbobj.Parents.Remove(ParentData[0]);
                                    dbobj.SaveChanges();
                                }
                            }
                            //SaveNewEventData(model.newEventLog, model.Id);
                            AddEventModel.CreateSystemEvent("New Contact Added[" + model.LastName + ", " + model.FirstName + "]", "Contact Added", "1) New Contact Named " + model.LastName + ", " + model.FirstName + "  Was Added");
                        }

                        ////set parent details in parent table
                        //var contactPersonal = dbobj.ContactPersonals.Where(x => x.StudentPersonalId == sess.StudentId).ToList();
                        //if (contactPersonal.Count > 0)
                        //{
                        //    foreach (var item in contactPersonal)
                        //    {

                        //        var contactRelations = dbobj.StudentContactRelationships.Where(x => x.ContactPersonalId == item.ContactPersonalId && x.RelationshipId == RelationData.LookupId).ToList();
                        //        if (contactRelations.Count > 0)
                        //        {
                        //            foreach (var parentDetails in contactRelations)
                        //            {
                        //                model.FirstName = item.FirstName;
                        //                model.LastName = item.LastName;
                        //                var ParentStudentRelData1 = dbobj.StudentParentRels.Where(x => x.StudentPersonalId == sess.StudentId).ToList();
                        //                if (ParentStudentRelData1.Count > 0)
                        //                {
                        //                    int parentId = ParentStudentRelData1[0].ParentID;
                        //                    var ParentData = dbobj.Parents.Where(x => x.ParentID == parentId).ToList();
                        //                    if (ParentData.Count > 0)
                        //                    {
                        //                        ParentData[0].Fname = model.FirstName;
                        //                        ParentData[0].Lname = model.LastName;
                        //                        ParentData[0].Username = model.FirstName;
                        //                        ParentData[0].Password = model.LastName;
                        //                        ParentData[0].ModifiedBy = sess.LoginId;
                        //                        ParentData[0].ModifiedOn = System.DateTime.Now;
                        //                        dbobj.SaveChanges();
                        //                    }
                        //                }
                        //                else
                        //                {
                        //                    parent.Fname = model.FirstName;
                        //                    parent.Lname = model.LastName;
                        //                    parent.Username = model.FirstName;
                        //                    parent.Password = model.LastName;
                        //                    parent.CreatedBy = sess.LoginId;
                        //                    parent.CreatedOn = System.DateTime.Now;
                        //                    dbobj.Parents.Add(parent);
                        //                    dbobj.SaveChanges();
                        //                    int parentid = parent.ParentID;

                        //                    studentParentRel.StudentPersonalId = sess.StudentId;
                        //                    studentParentRel.ParentID = parentid;
                        //                    studentParentRel.CreatedBy = sess.LoginId;
                        //                    studentParentRel.CreatedOn = System.DateTime.Now;
                        //                    dbobj.StudentParentRels.Add(studentParentRel);
                        //                    dbobj.SaveChanges();
                        //                }
                        //            }
                        //        }
                        //    }
                        //}
                        trans.Complete();
                        Result = "Sucess";
                    }
                }
                //catch
                //{
                //    Result = "Failed";
                //}
                catch (DbEntityValidationException dbEx)
                {
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                        }
                    }
                    Result = "Failed";
                }
                catch (Exception e)
                {
                    ClsErrorLog error = new ClsErrorLog();
                    error.WriteToLog("Error:" + e);
                }
            }




            return Result;
        }



        /// <summary>
        /// Funtion To load Saved Visitation Details For Edit / View
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        /// 
        public AddVisitaionModel bindVisitation(int itemId)
        {
            sess = (clsSession)HttpContext.Current.Session["UserSessionClient"];
            dbobj = new BiWeeklyRCPNewEntities();
            AddVisitaionModel returnModel = new AddVisitaionModel();
            Visitation visit = new Visitation();
            DateTime now = DateTime.Now;
            if (itemId > 0)
            {
                try
                {
                    visit = dbobj.Visitations.Where(objvisitation => objvisitation.StudentPersonalId == sess.StudentId && objvisitation.VisitationId == itemId).SingleOrDefault();
                    returnModel.Id = visit.VisitationId;
                    returnModel.EventName = visit.VisitationName;
                    returnModel.EventStatus = visit.VisitationStatus;
                    returnModel.EventType = visit.VisittaionType;
                    returnModel.ExpiredOnDate = ConvertDate(visit.ExpiredOn);
                    returnModel.EventDate = ConvertDate(visit.VisitationDate);
                    returnModel.DateRequested = visit.DateRequested;
                    returnModel.EffectiveDate = visit.EffectiveDate;
                    returnModel.Note = visit.Note;
                    returnModel.Form = visit.Form;
                }
                catch (DbEntityValidationException dbEx)
                {
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                        }
                    }
                }
            }
            return returnModel;
        }

        /// <summary>
        /// Function to Save / Update Visitation Data
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>

        public string SaveVisitationData(AddVisitaionModel model)
        {
            sess = (clsSession)HttpContext.Current.Session["UserSessionClient"];
            dbobj = new BiWeeklyRCPNewEntities();
            int ClientID = sess.StudentId, SchoolId = sess.SchoolId;

            Visitation visit = new Visitation();
            if (model.Id > 0)
            {
                try
                {
                    visit = dbobj.Visitations.Where(objVisitation => objVisitation.VisitationId == model.Id && objVisitation.StudentPersonalId == ClientID).SingleOrDefault();
                    visit.VisitationName = model.EventName;
                    visit.VisittaionType = model.EventType;
                    visit.VisitationStatus = model.EventStatus;
                    visit.Status = 1;
                    visit.VisitationDate = DateTime.Now;
                    visit.ExpiredOn = DateTime.ParseExact(model.ExpiredOnDate, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    visit.ModifiedBy = 1;
                    visit.ModifiedOn = DateTime.Now;

                    visit.DateRequested = model.DateRequested;
                    visit.EffectiveDate = model.DateRequested;
                    visit.Note = model.Note;
                    visit.Form = model.Form;

                    dbobj.SaveChanges();
                    return "Sucess";
                }
                catch
                {
                    return "Failed";
                }
            }
            else
            {
                if (ClientID == 0)
                {
                    return "No Client Selected";
                }
                else
                {
                    try
                    {
                        visit.SchoolId = SchoolId;
                        visit.VisitationName = model.EventName;
                        visit.VisittaionType = model.EventType;
                        visit.VisitationStatus = model.EventStatus;
                        visit.Status = 1;
                        visit.StudentPersonalId = ClientID;
                        visit.VisitationDate = DateTime.Now;
                        visit.ExpiredOn = DateTime.ParseExact(model.ExpiredOnDate, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        visit.CreatedBy = 1;
                        visit.CreatedOn = DateTime.Now;

                        visit.DateRequested = model.DateRequested;
                        visit.EffectiveDate = model.DateRequested;
                        visit.Note = model.Note;
                        visit.Form = model.Form;

                        dbobj.Visitations.Add(visit);
                        dbobj.SaveChanges();
                        return "Sucess";
                    }
                    catch
                    {
                        return "Failed";
                    }
                }
            }

        }


        /// <summary>
        /// Function To delete Visitation Data.
        /// </summary>
        /// <param name="ClientId"></param>
        /// <param name="itemId"></param>

        public void deleteVisitation(int ClientId, int itemId)
        {

            dbobj = new BiWeeklyRCPNewEntities();
            VisitationModel contactModel = new VisitationModel();
            Visitation visitataion = new Visitation();
            visitataion = dbobj.Visitations.Where(objVisitation => objVisitation.StudentPersonalId == ClientId && objVisitation.VisitationId == itemId).SingleOrDefault();
            visitataion.Status = 0;
            dbobj.SaveChanges();
        }

        /// <summary>
        /// Function To Bind visitation Details.
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>

        public AddPlacementModel bindPlacement(int itemId)
        {
            sess = (clsSession)HttpContext.Current.Session["UserSessionClient"];
            dbobj = new BiWeeklyRCPNewEntities();
            AddPlacementModel returnModel = new AddPlacementModel();
            Placement placement = new Placement();
            if (itemId > 0)
            {
                try
                {
                    placement = dbobj.Placements.Where(objPlacement => objPlacement.StudentPersonalId == sess.StudentId && objPlacement.PlacementId == itemId).SingleOrDefault();
                    returnModel.Id = placement.PlacementId;
                    returnModel.PlacementType = placement.PlacementType;
                    returnModel.BehaviorAnalyst = placement.BehaviorAnalyst;
                    returnModel.PrimaryNurse = placement.PrimaryNurse;
                    returnModel.Department = placement.Department;
                    returnModel.UnitClerk = placement.UnitClerk;
                    returnModel.StartDate = ConvertDate(placement.StartDate);
                    returnModel.EndDateDate = ConvertDate(placement.EndDate);

                    returnModel.IsMonday = placement.IsMonday.GetBool();
                    returnModel.IsTuesday = placement.IsTuesday.GetBool();
                    returnModel.IsWednesday = placement.IsWednesday.GetBool();
                    returnModel.IsThursday = placement.IsThursday.GetBool();
                    returnModel.IsFriday = placement.IsFriday.GetBool();
                    returnModel.IsSaturday = placement.IsSaturday.GetBool();
                    returnModel.IsSunday = placement.IsSunday.GetBool();

                    returnModel.MondayNote = placement.MondayNote;
                    returnModel.TuesdayNote = placement.TuesdayNote;
                    returnModel.WednesdayNote = placement.WednesdayNote;
                    returnModel.ThursdayNote = placement.ThursdayNote;
                    returnModel.FridayNote = placement.FridayNote;
                    returnModel.SaturdayNote = placement.SaturdayNote;
                    returnModel.SundayNote = placement.SundayNote;

                    returnModel.Reason = placement.Reason;
                    returnModel.AssociatedPersonnel = placement.AssociatedPersonnel;
                    returnModel.LocationId = placement.Location;
                    returnModel.PlacementDepartmentId = placement.PlacementDepartment;
                    returnModel.PlacementReason = placement.PlacementReason;

                }
                catch (DbEntityValidationException dbEx)
                {
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                        }
                    }
                }
            }
            return returnModel;
        }


        /// <summary>
        /// Function to Save / Update Placement Data
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>

        public string SavePlacementData(AddPlacementModel model)
        {
            sess = (clsSession)HttpContext.Current.Session["UserSessionClient"];
            dbobj = new BiWeeklyRCPNewEntities();
            int ClientID = sess.StudentId, SchoolId = sess.SchoolId;

            Placement placement = new Placement();
            if (model.Id > 0)
            {
                try
                {
                    placement = dbobj.Placements.Where(objPlacement => objPlacement.PlacementId == model.Id && objPlacement.StudentPersonalId == ClientID).SingleOrDefault();
                    placement.PlacementType = model.PlacementType;
                    placement.BehaviorAnalyst = model.BehaviorAnalyst;
                    placement.UnitClerk = model.UnitClerk;
                    placement.PrimaryNurse = model.PrimaryNurse;
                    placement.Department = model.Department;
                    placement.Status = 1;
                    placement.StartDate = DateTime.ParseExact(model.StartDate, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    if (model.EndDateDate != null)
                        placement.EndDate = DateTime.ParseExact(model.EndDateDate, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    else
                        placement.EndDate = null;
                    placement.ModifiedBy = 1;
                    placement.ModifiedOn = DateTime.Now;

                    placement.Reason = model.Reason;
                    placement.AssociatedPersonnel = model.AssociatedPersonnel;
                    placement.Location = model.LocationId;
                    placement.PlacementDepartment = model.PlacementDepartmentId;
                    placement.PlacementReason = model.PlacementReason;

                    placement.IsMonday = model.IsMonday;
                    placement.IsTuesday = model.IsTuesday;
                    placement.IsWednesday = model.IsWednesday;
                    placement.IsThursday = model.IsThursday;
                    placement.IsFriday = model.IsFriday;
                    placement.IsSaturday = model.IsSaturday;
                    placement.IsSunday = model.IsSunday;

                    placement.MondayNote = model.MondayNote;
                    placement.TuesdayNote = model.TuesdayNote;
                    placement.WednesdayNote = model.WednesdayNote;
                    placement.ThursdayNote = model.ThursdayNote;
                    placement.FridayNote = model.FridayNote;
                    placement.SaturdayNote = model.SaturdayNote;
                    placement.SundayNote = model.SundayNote;



                    Other_Functions of = new Other_Functions();
                    of.RemoveFromClass(ClientID, Convert.ToInt32(model.LocationId), model.Id);

                    dbobj.SaveChanges();

                    string PlacementName = "";
                    if (placement.Department != null && placement.Department > 0)
                    {
                        var Plcname = dbobj.LookUps.Where(x => x.LookupId == placement.Department).ToList();
                        if (Plcname.Count > 0) PlacementName = Plcname[0].LookupName;
                    }
                    SaveNewEventData(model.newEventLog, model.Id);
                    if (model.EndDateDate != null && model.EndDateDate != "")
                    {
                        if (model.placemntLogText != "")
                        {
                            // AddEventModel.CreateSystemEvent("Placement " + PlacementName + " Discharged on :" + model.EndDateDate, "Discharged", model.placemntLogText);
                            AddEventModel.CreateSystemEvent("Placement [" + PlacementName + "] was Updated", "Placement Updated", model.placemntLogText);
                        }
                    }
                    else
                    {
                        if (model.placemntLogText != "")
                        {
                            AddEventModel.CreateSystemEvent("Placement [" + PlacementName + "] was Updated", "Placement Updated", model.placemntLogText);
                        }
                        //AddEventModel.CreateSystemEvent("Placement  " + PlacementName + " Changed", "Moved", model.placemntLogText);
                    }
                    //DisplayStatus();
                    return "Sucess";
                }
                catch
                {
                    return "Failed";
                }
            }
            else
            {
                if (ClientID == 0)
                {
                    return "No Client Selected";
                }
                else
                {
                    try
                    {
                        placement.SchoolId = SchoolId;
                        placement.PlacementType = model.PlacementType;
                        placement.BehaviorAnalyst = model.BehaviorAnalyst;
                        placement.UnitClerk = model.UnitClerk;
                        placement.PrimaryNurse = model.PrimaryNurse;
                        placement.Department = model.Department;
                        placement.Status = 1;
                        placement.StudentPersonalId = ClientID;
                        placement.StartDate = DateTime.ParseExact(model.StartDate, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        if (model.EndDateDate != null)
                            placement.EndDate = DateTime.ParseExact(model.EndDateDate, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        placement.CreatedBy = 1;
                        placement.CreatedOn = DateTime.Now;

                        placement.Reason = model.Reason;
                        placement.AssociatedPersonnel = model.AssociatedPersonnel;
                        placement.Location = model.LocationId;
                        placement.PlacementDepartment = model.PlacementDepartmentId;
                        placement.PlacementReason = model.PlacementReason;

                        placement.IsMonday = model.IsMonday;
                        placement.IsTuesday = model.IsTuesday;
                        placement.IsWednesday = model.IsWednesday;
                        placement.IsThursday = model.IsThursday;
                        placement.IsFriday = model.IsFriday;
                        placement.IsSaturday = model.IsSaturday;
                        placement.IsSunday = model.IsSunday;

                        placement.MondayNote = model.MondayNote;
                        placement.TuesdayNote = model.TuesdayNote;
                        placement.WednesdayNote = model.WednesdayNote;
                        placement.ThursdayNote = model.ThursdayNote;
                        placement.FridayNote = model.FridayNote;
                        placement.SaturdayNote = model.SaturdayNote;
                        placement.SundayNote = model.SundayNote;

                        dbobj.Placements.Add(placement);
                        dbobj.SaveChanges();


                        Other_Functions of = new Other_Functions();
                        of.AssignToClass(ClientID, Convert.ToInt32(model.LocationId));


                        string PlacementName = "";
                        if (placement.Department != null && placement.Department > 0)
                        {
                            var Plcname = dbobj.LookUps.Where(x => x.LookupId == placement.Department).ToList();
                            if (Plcname.Count > 0) PlacementName = Plcname[0].LookupName;
                        }
                        //DisplayStatus();
                        AddEventModel.CreateSystemEvent("New Placement " + PlacementName + " Added", "Placement Added", "Placement " + PlacementName + " Added");
                        return "Sucess";
                    }
                    catch
                    {
                        return "Failed";
                    }
                }
            }



        }

        public ProtocolSummary ProtocolExpo(int studentId, int schoolId, string path)
        {
            ClientRegistrationPAModel model1 = new ClientRegistrationPAModel();
            ProtocolSummary model = new ProtocolSummary();
            StudentPersonalPA studentPA = new StudentPersonalPA();
            BiWeeklyRCPNewEntities dbobj = new BiWeeklyRCPNewEntities();
            sess = (clsSession)HttpContext.Current.Session["UserSessionClient"];

            var pId1 = (from m in dbobj.Protocols
                        where (m.SchoolId == schoolId && m.StudentId == studentId)
                        select m.ProtocolId).ToList();

            var Details = (from m in dbobj.StudentPersonals
                           where (m.SchoolId == schoolId && m.StudentPersonalId == studentId)
                           select m).SingleOrDefault();

            model1.DateOfBirth = (Details.BirthDate != null) ? ((DateTime)Details.BirthDate).ToString("MM.dd.yyyy") : "";
            model1.FirstName = Details.FirstName + " " + Details.LastName;

            if (pId1.Count > 0)
            {
                int pId = pId1[0];

                var SummaryItems = (from x in dbobj.Protocols
                                    //where (x.SchoolId==sess.SchoolId && x.StudentId==sess.StudentId)
                                    where (x.ProtocolId == pId)
                                    select new ProtocolSummary
                                    {
                                        HomeCommon = x.HomeCommon,
                                        HomeBedroom = x.HomeBedroom,
                                        HomeBathroom = x.HomeBathroom,
                                        Campus = x.Campus,
                                        Community = x.Community,
                                        SchoolCommon = x.SchoolCommon,
                                        SchoolBathroom = x.SchoolBathroom,
                                        SchoolOutside = x.SchoolOutside,
                                        Pool = x.Pool,
                                        Van = x.Van,

                                        MasteredTask = x.MasteredTask,
                                        NewTask = x.NewTask,

                                        //Allergies = x.Allergies,
                                        // SeizureInfo = x.SeizureInfo,
                                        MedTimes = x.MedTimes,
                                        TakeMed = x.TakeMed,
                                        OtherMedical = x.OtherMedical,

                                        DoctorVisit = x.DoctorVisit,
                                        Dental = x.Dental,
                                        BloodWork = x.BloodWork,
                                        HairCuts = x.HairCuts,
                                        OtherBehave = x.OtherBehave,

                                        EatingGeneral = x.EatingGeneral,
                                        EatingAble = x.EatingAble,
                                        EatingNeed = x.EatingNeed,
                                        EatingIep = x.EatingIep,

                                        ToiletingGeneral = x.ToiletingGeneral,
                                        ToiletingAble = x.ToiletingAble,
                                        ToiletingNeed = x.ToiletingNeed,
                                        ToiletingIep = x.ToiletingIep,

                                        BrushingGeneral = x.BrushingGeneral,
                                        BrushingAble = x.BrushingAble,
                                        BrushingNeed = x.BrushingNeed,
                                        BrushingIep = x.BrushingIep,

                                        HandGeneral = x.HandGeneral,
                                        HandAble = x.HandAble,
                                        HandNeed = x.HandNeed,
                                        HandIep = x.HandIep,

                                        DressGeneral = x.DressGeneral,
                                        DressAble = x.DressAble,
                                        DressNeed = x.DressNeed,
                                        DressIep = x.DressIep,

                                        ShowerGeneral = x.ShowerGeneral,
                                        ShowerAble = x.ShowerAble,
                                        ShowerNeed = x.ShowerNeed,
                                        ShowerIep = x.ShowerIep,

                                        BedTime = x.BedTime,

                                        Morning7 = x.Morning7,
                                        Morning715 = x.Morning715,
                                        Morning730 = x.Morning730,
                                        Morning745 = x.Morning745,
                                        Morning800 = x.Morning800,
                                        Morning815 = x.Morning815,
                                        Morning830 = x.Morning830,
                                        Morning845 = x.Morning845,
                                        Morning900 = x.Morning900,

                                        Noon330 = x.Noon330,
                                        Noon400 = x.Noon400,
                                        Noon430 = x.Noon430,
                                        Noon500 = x.Noon500,
                                        Noon530 = x.Noon530,
                                        Noon600 = x.Noon600,
                                        Noon630 = x.Noon630,
                                        Noon700 = x.Noon700,
                                        Noon730 = x.Noon730,
                                        Noon800 = x.Noon800,
                                        Noon830 = x.Noon830,
                                        Noon900 = x.Noon900,
                                        Noon930 = x.Noon930,
                                        Noon100 = x.Noon100,
                                        Noon10to11 = x.Noon10to11,
                                        Noon11to7 = x.Noon11to7,
                                        Leisure = x.Leisure,
                                        Modified1 = x.ModifiedOn,

                                        UpdtLOS = x.UpdtLOS,
                                        UpdtPageTop = x.UpdtTop,
                                        UpdtTPH = x.UpdtTPH,
                                        UpdtMedInfo = x.UpdtMedInfo,
                                        UpdtPerCare = x.UpdtPerCare,
                                        UpdtBehInfo = x.UpdtBehInfo,
                                        UpdtTypRoutines = x.UpdtTypRoutines,

                                        UpdtATList = x.UpdtAtList,
                                        UpdtCGList = x.UpdtCGList,
                                        UpdtFIList = x.UpdtFIList,
                                        UpdtBBIList = x.UpdtBBIList
                                    }).SingleOrDefault();

                SummaryItems.ATList = (from a in dbobj.Protocol_1_Assistive
                                       where (a.ProtocolId == pId)
                                       select new AssistTech
                                       {
                                           Type = a.Type,
                                           ScheduleForUse = a.ScheduleForUse,
                                           StorageLocation = a.StorageLocation,
                                           Modified2 = a.ModifiedOn,
                                           AssistiveId = a.AssistiveId
                                           //UpdtATList=a.UpdtATList
                                       }).ToList();

                SummaryItems.CGList = (from b in dbobj.Protocol_2_Community
                                       where (b.ProtocolId == pId)
                                       select new CommGuide
                                       {
                                           TypeA = b.TypeA,
                                           TypeB = b.TypeB,
                                           Modified3 = b.ModifiedOn,
                                           CommunityId = b.CommunityId
                                           //UpdtCGList=b.UpdtCGList
                                       }).ToList();

                SummaryItems.FIList = (from c in dbobj.Protocol_3_Family
                                       where (c.ProtocolId == pId)
                                       select new FamilyInfo
                                       {
                                           FamilyOne = c.FamilyOne,
                                           FamilyTwo = c.FamilyTwo,
                                           Modified4 = c.ModifiedOn,
                                           FamilyId = c.FamilyId
                                           //UpdtFIList=c.UpdtFIList
                                       }).ToList();

                SummaryItems.BBIList = (from d in dbobj.Protocol_4_Basic
                                        where (d.ProtocolId == pId)
                                        select new BasicBehavInfo
                                        {
                                            Acceleration = d.Acceleration,
                                            Strategy = d.Strategy,
                                            Modified5 = d.ModifiedOn,
                                            BasicId = d.BasicId
                                            //UpdtBBIList=d.UpdtBBIList
                                        }).ToList();

                SummaryItems.SignList = (from e in dbobj.Protocol_5_Signature
                                         where (e.ProtocolId == pId)
                                         select new SignatureLi
                                         {
                                             SignatureId = e.SignatureId,
                                             PrintName = e.PrintName,
                                             Signature = e.Signature,
                                             Date = e.Date,
                                             Modified6 = e.ModifiedOn
                                         }).ToList();
                model = SummaryItems;
            }
            int ClientId = Convert.ToInt32(HttpContext.Current.Session["TempStudentId"]);
            studentPA = dbobj.StudentPersonalPAs.Where(objStudentPA => objStudentPA.StudentPersonalId == ClientId && objStudentPA.SchoolId == schoolId).SingleOrDefault();
            if (studentPA != null)
            {
                model.Allergies = studentPA.Allergies;
                model.SeizureInfo = studentPA.Seizures;
            }

            //string path = HttpContext.Current.Server.MapPath("~\\Templates\\Protocol.docx");
            string NewPath = CopyTemplate(path, "0");
            using (WordprocessingDocument theDoc = WordprocessingDocument.Open(NewPath, true))
            {
                try
                {
                    MainDocumentPart mainPart = theDoc.MainDocumentPart;

                    // This should return only one content control element: the one with 
                    // the specified tag value.
                    // If not, "Single()" throws an exception.


                    // This should return only one table.

                    SdtElement headDOB = mainPart.HeaderParts.First().Header.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtHeadDOB").SingleOrDefault();
                    headDOB.InsertAfterSelf(new Paragraph(new Run(new Text("DOB:" + model1.DateOfBirth))));
                    headDOB.Remove();

                    SdtElement headName = mainPart.HeaderParts.First().Header.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtHeadName").SingleOrDefault();
                    headName.InsertAfterSelf(new Paragraph(new Run(new Text("Student:" + model1.FirstName))));
                    headName.Remove();

                    SdtElement headUp = mainPart.HeaderParts.First().Header.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtHeadUp").SingleOrDefault();
                    headUp.InsertAfterSelf(new Paragraph(new Run(new Text("Updated:" + model.Modified1))));
                    headUp.Remove();

                    SdtElement blockUp = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtUpdatedLevel").Single();
                    blockUp.InsertAfterSelf(new Paragraph(new Run(new Text(model.UpdtLOS))));
                    blockUp.Remove();

                    SdtElement block = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "plcCommon").Single();
                    block.InsertAfterSelf(new Paragraph(new Run(new Text(model.HomeCommon))));
                    block.Remove();

                    SdtElement blockUpTop = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtUpTop").Single();
                    blockUpTop.InsertAfterSelf(new Paragraph(new Run(new Text(model.UpdtPageTop))));
                    blockUpTop.Remove();

                    SdtElement block1 = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtBedroom").Single();
                    block1.InsertAfterSelf(new Paragraph(new Run(new Text(model.HomeBedroom))));
                    block1.Remove();

                    SdtElement block2 = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtHomeBathroom").Single();
                    block2.InsertAfterSelf(new Paragraph(new Run(new Text(model.HomeBathroom))));
                    block2.Remove();

                    SdtElement block3 = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtCampus").Single();
                    block3.InsertAfterSelf(new Paragraph(new Run(new Text(model.Campus))));
                    // block3.Remove();

                    SdtElement block4 = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtCommunity").Single();
                    block4.InsertAfterSelf(new Paragraph(new Run(new Text(model.Community))));
                    block4.Remove();

                    SdtElement block5 = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtSchool").Single();
                    block5.InsertAfterSelf(new Paragraph(new Run(new Text(model.SchoolCommon))));
                    block5.Remove();

                    SdtElement block6 = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtScBath").Single();
                    block6.InsertAfterSelf(new Paragraph(new Run(new Text(model.SchoolBathroom))));
                    block6.Remove();

                    SdtElement block7 = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtSchoolTransitive").Single();
                    block7.InsertAfterSelf(new Paragraph(new Run(new Text(model.SchoolOutside))));
                    block7.Remove();

                    SdtElement block8 = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtPool").Single();
                    block8.InsertAfterSelf(new Paragraph(new Run(new Text(model.Pool))));
                    block8.Remove();

                    SdtElement block9 = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtVanTag").Single();
                    block9.InsertAfterSelf(new Paragraph(new Run(new Text(model.Van))));
                    block9.Remove();

                    SdtElement blockAs = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtUpdatedAssis").Single();
                    blockAs.InsertAfterSelf(new Paragraph(new Run(new Text(model.UpdtATList))));
                    blockAs.Remove();

                    SdtElement blockPer = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtUpdatedPer").Single();
                    blockPer.InsertAfterSelf(new Paragraph(new Run(new Text(model.UpdtPerCare))));
                    blockPer.Remove();

                    SdtElement blockFam = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtUpdatedFamily").Single();
                    blockFam.InsertAfterSelf(new Paragraph(new Run(new Text(model.UpdtFIList))));
                    blockFam.Remove();


                    SdtElement blockTyp = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtUpdatedTypical").Single();
                    blockTyp.InsertAfterSelf(new Paragraph(new Run(new Text(model.UpdtTypRoutines))));
                    blockTyp.Remove();

                    SdtElement blockCom = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtUpdatedCom").Single();
                    blockCom.InsertAfterSelf(new Paragraph(new Run(new Text(model.UpdtCGList))));
                    blockCom.Remove();

                    SdtElement blockBeh = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtUpdatedBeh").Single();
                    blockBeh.InsertAfterSelf(new Paragraph(new Run(new Text(model.UpdtBehInfo))));
                    blockBeh.Remove();

                    if (model.ATList != null)
                    {
                        if (model.ATList.Count > 0)
                        {
                            SdtBlock ccWithTable = mainPart.Document.Body.Descendants<SdtBlock>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "tblAssistive").Single();
                            DocumentFormat.OpenXml.Wordprocessing.Table theTable = ccWithTable.Descendants<DocumentFormat.OpenXml.Wordprocessing.Table>().Single();
                            DocumentFormat.OpenXml.Wordprocessing.TableRow theRow = theTable.Elements<DocumentFormat.OpenXml.Wordprocessing.TableRow>().Last();
                            foreach (var item in model.ATList)
                            {
                                DocumentFormat.OpenXml.Wordprocessing.TableRow rowCopy = (DocumentFormat.OpenXml.Wordprocessing.TableRow)theRow.CloneNode(true);
                                rowCopy.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(0).Append(new Paragraph
                                    (new Run(new Text(item.Type))));
                                rowCopy.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(1).Append(new Paragraph
                                    (new Run(new Text(item.ScheduleForUse))));
                                rowCopy.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(2).Append(new Paragraph
                                    (new Run(new Text(item.StorageLocation))));
                                theTable.AppendChild(rowCopy);
                            }
                            theTable.RemoveChild(theRow);
                        }
                    }

                    SdtElement blockBas = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtUpdatedBasic").Single();
                    blockBas.InsertAfterSelf(new Paragraph(new Run(new Text(model.UpdtBBIList))));
                    blockBas.Remove();

                    SdtElement blockUpTyp = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtUpdatedTyp").Single();
                    blockUpTyp.InsertAfterSelf(new Paragraph(new Run(new Text(model.UpdtTPH))));
                    blockUpTyp.Remove();

                    SdtElement block10 = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtMastered").Single();
                    block10.InsertAfterSelf(new Paragraph(new Run(new Text(model.MasteredTask))));
                    block10.Remove();

                    SdtElement block11 = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtNewTag").Single();
                    block11.InsertAfterSelf(new Paragraph(new Run(new Text(model.NewTask))));
                    block11.Remove();

                    SdtElement blockMed = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtUpdatedMed").Single();
                    blockMed.InsertAfterSelf(new Paragraph(new Run(new Text(model.UpdtMedInfo))));
                    blockMed.Remove();

                    SdtElement block12 = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtAllergyes").Single();
                    block12.InsertAfterSelf(new Paragraph(new Run(new Text(model.Allergies))));
                    block12.Remove();

                    SdtElement block13 = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtSeizure").Single();
                    block13.InsertAfterSelf(new Paragraph(new Run(new Text(model.SeizureInfo))));
                    block13.Remove();

                    SdtElement block14 = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtMedTag").Single();
                    block14.InsertAfterSelf(new Paragraph(new Run(new Text(model.MedTimes))));
                    block14.Remove();

                    SdtElement block15 = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtHowTag").Single();
                    block15.InsertAfterSelf(new Paragraph(new Run(new Text(model.TakeMed))));
                    block15.Remove();

                    SdtElement block16 = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtOtherTag").Single();
                    block16.InsertAfterSelf(new Paragraph(new Run(new Text(model.OtherMedical))));
                    block16.Remove();

                    SdtElement block17 = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtDoctor").Single();
                    block17.InsertAfterSelf(new Paragraph(new Run(new Text(model.DoctorVisit))));
                    block17.Remove();

                    SdtElement block18 = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtDental").Single();
                    block18.InsertAfterSelf(new Paragraph(new Run(new Text(model.Dental))));
                    block18.Remove();

                    SdtElement block19 = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtBloodTag").Single();
                    block19.InsertAfterSelf(new Paragraph(new Run(new Text(model.BloodWork))));
                    block19.Remove();

                    SdtElement block20 = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtHaircut").Single();
                    block20.InsertAfterSelf(new Paragraph(new Run(new Text(model.HairCuts))));
                    block20.Remove();

                    SdtElement block21 = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txt2Tag").Single();
                    block21.InsertAfterSelf(new Paragraph(new Run(new Text(model.OtherBehave))));
                    block21.Remove();

                    SdtElement block22 = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtGeneralEat").Single();
                    block22.InsertAfterSelf(new Paragraph(new Run(new Text(model.EatingGeneral))));
                    block22.Remove();

                    SdtElement block23 = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtAbleEat").Single();
                    block23.InsertAfterSelf(new Paragraph(new Run(new Text(model.EatingAble))));
                    block23.Remove();

                    SdtElement block24 = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtNeedEat").Single();
                    block24.InsertAfterSelf(new Paragraph(new Run(new Text(model.EatingNeed))));
                    block24.Remove();

                    SdtElement block25 = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtIepEat").Single();
                    block25.InsertAfterSelf(new Paragraph(new Run(new Text(model.EatingIep))));
                    block25.Remove();

                    SdtElement block26 = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtToGen").Single();
                    block26.InsertAfterSelf(new Paragraph(new Run(new Text(model.ToiletingGeneral))));
                    block26.Remove();

                    SdtElement block27 = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtIleAble").Single();
                    block27.InsertAfterSelf(new Paragraph(new Run(new Text(model.ToiletingAble))));
                    block27.Remove();

                    SdtElement block28 = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtHelpTo").Single();
                    block28.InsertAfterSelf(new Paragraph(new Run(new Text(model.ToiletingNeed))));
                    block28.Remove();

                    SdtElement block29 = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtIspTo").Single();
                    block29.InsertAfterSelf(new Paragraph(new Run(new Text(model.ToiletingIep))));
                    block29.Remove();

                    SdtElement block30 = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtBrushTag").Single();
                    block30.InsertAfterSelf(new Paragraph(new Run(new Text(model.BrushingGeneral))));
                    block30.Remove();

                    SdtElement block31 = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtIngTeeth").Single();
                    block31.InsertAfterSelf(new Paragraph(new Run(new Text(model.BrushingAble))));
                    block31.Remove();

                    SdtElement block32 = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtWithBrush").Single();
                    block32.InsertAfterSelf(new Paragraph(new Run(new Text(model.BrushingNeed))));
                    block32.Remove();

                    SdtElement block33 = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtGoalBrush").Single();
                    block33.InsertAfterSelf(new Paragraph(new Run(new Text(model.BrushingIep))));
                    block33.Remove();

                    SdtElement block34 = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "plcHandTag").Single();
                    block34.InsertAfterSelf(new Paragraph(new Run(new Text(model.HandGeneral))));
                    block34.Remove();

                    SdtElement block35 = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtWasAble").Single();
                    block35.InsertAfterSelf(new Paragraph(new Run(new Text(model.HandAble))));
                    block35.Remove();

                    SdtElement block36 = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtAshNeed").Single();
                    block36.InsertAfterSelf(new Paragraph(new Run(new Text(model.HandNeed))));
                    block36.Remove();

                    SdtElement block37 = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtPeiHand").Single();
                    block37.InsertAfterSelf(new Paragraph(new Run(new Text(model.HandIep))));
                    block37.Remove();

                    SdtElement block38 = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtDressTag").Single();
                    block38.InsertAfterSelf(new Paragraph(new Run(new Text(model.DressGeneral))));
                    block38.Remove();

                    SdtElement block39 = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtResAble").Single();
                    block39.InsertAfterSelf(new Paragraph(new Run(new Text(model.DressAble))));
                    block39.Remove();

                    SdtElement block40 = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtEssNeed").Single();
                    block40.InsertAfterSelf(new Paragraph(new Run(new Text(model.DressNeed))));
                    block40.Remove();

                    SdtElement block41 = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtPsiDress").Single();
                    block41.InsertAfterSelf(new Paragraph(new Run(new Text(model.DressIep))));
                    block41.Remove();

                    SdtElement block42 = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "ShowerGen").Single();
                    block42.InsertAfterSelf(new Paragraph(new Run(new Text(model.ShowerGeneral))));
                    block42.Remove();

                    SdtElement block43 = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtWerAble").Single();
                    block43.InsertAfterSelf(new Paragraph(new Run(new Text(model.ShowerAble))));
                    block43.Remove();

                    SdtElement block44 = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtNhwShower").Single();
                    block44.InsertAfterSelf(new Paragraph(new Run(new Text(model.ShowerNeed))));
                    block44.Remove();

                    SdtElement block45 = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtSlaShower").Single();
                    block45.InsertAfterSelf(new Paragraph(new Run(new Text(model.ShowerIep))));
                    block45.Remove();

                    SdtElement block46 = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtBedtime").Single();
                    block46.InsertAfterSelf(new Paragraph(new Run(new Text(model.BedTime))));
                    block46.Remove();

                    if (model.CGList != null)
                    {
                        if (model.CGList.Count > 0)
                        {
                            SdtBlock ccWithTableCom = mainPart.Document.Body.Descendants<SdtBlock>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "tblCommunity").Single();
                            DocumentFormat.OpenXml.Wordprocessing.Table theTableCom = ccWithTableCom.Descendants<DocumentFormat.OpenXml.Wordprocessing.Table>().Single();
                            DocumentFormat.OpenXml.Wordprocessing.TableRow theRowCom = theTableCom.Elements<DocumentFormat.OpenXml.Wordprocessing.TableRow>().Last();

                            foreach (var item in model.CGList)
                            {

                                DocumentFormat.OpenXml.Wordprocessing.TableRow rowCopy1 = (DocumentFormat.OpenXml.Wordprocessing.TableRow)theRowCom.CloneNode(true);
                                rowCopy1.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(0).Append(new Paragraph
                                    (new Run(new Text(item.TypeA))));
                                rowCopy1.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(1).Append(new Paragraph
                                    (new Run(new Text(item.TypeB))));
                                theTableCom.AppendChild(rowCopy1);
                            }
                            theTableCom.RemoveChild(theRowCom);
                        }
                    }
                    if (model.FIList != null)
                    {
                        if (model.FIList.Count > 0)
                        {

                            SdtBlock ccWithTableFam = mainPart.Document.Body.Descendants<SdtBlock>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "tblFamilyInfo").Single();
                            DocumentFormat.OpenXml.Wordprocessing.Table theTableFam = ccWithTableFam.Descendants<DocumentFormat.OpenXml.Wordprocessing.Table>().Single();
                            DocumentFormat.OpenXml.Wordprocessing.TableRow theRowFam = theTableFam.Elements<DocumentFormat.OpenXml.Wordprocessing.TableRow>().Last();
                            foreach (var item in model.FIList)
                            {
                                DocumentFormat.OpenXml.Wordprocessing.TableRow rowCopy2 = (DocumentFormat.OpenXml.Wordprocessing.TableRow)theRowFam.CloneNode(true);
                                rowCopy2.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(0).Append(new Paragraph
                                    (new Run(new Text(item.FamilyOne))));
                                rowCopy2.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(1).Append(new Paragraph
                                    (new Run(new Text(item.FamilyTwo))));
                                theTableFam.AppendChild(rowCopy2);
                            }
                            theTableFam.RemoveChild(theRowFam);
                        }
                    }
                    if (model.BBIList != null)
                    {
                        if (model.BBIList.Count > 0)
                        {

                            SdtBlock ccWithTableBasic = mainPart.Document.Body.Descendants<SdtBlock>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "tblBasicBehave").Single();
                            DocumentFormat.OpenXml.Wordprocessing.Table theTableBasic = ccWithTableBasic.Descendants<DocumentFormat.OpenXml.Wordprocessing.Table>().Single();
                            DocumentFormat.OpenXml.Wordprocessing.TableRow theRowBasic = theTableBasic.Elements<DocumentFormat.OpenXml.Wordprocessing.TableRow>().Last();
                            foreach (var item in model.BBIList)
                            {
                                DocumentFormat.OpenXml.Wordprocessing.TableRow rowCopy3 = (DocumentFormat.OpenXml.Wordprocessing.TableRow)theRowBasic.CloneNode(true);
                                rowCopy3.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(0).Append(new Paragraph
                                    (new Run(new Text(item.Acceleration))));
                                rowCopy3.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(1).Append(new Paragraph
                                    (new Run(new Text(item.Strategy))));
                                theTableBasic.AppendChild(rowCopy3);
                            }
                            theTableBasic.RemoveChild(theRowBasic);
                        }
                    }

                    SdtElement block47 = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtc72tag").Single();
                    block47.InsertAfterSelf(new Paragraph(new Run(new Text(model.Morning7))));
                    block47.Remove();

                    SdtElement block48 = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txt1572Tag").Single();
                    block48.InsertAfterSelf(new Paragraph(new Run(new Text(model.Morning715))));
                    block48.Remove();

                    SdtElement block49 = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txt0372tag").Single();
                    block49.InsertAfterSelf(new Paragraph(new Run(new Text(model.Morning730))));
                    block49.Remove();

                    SdtElement block50 = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txt5472").Single();
                    block50.InsertAfterSelf(new Paragraph(new Run(new Text(model.Morning745))));
                    block50.Remove();

                    SdtElement block51 = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txt8002").Single();
                    block51.InsertAfterSelf(new Paragraph(new Run(new Text(model.Morning800))));
                    block51.Remove();

                    SdtElement block52 = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txt5182").Single();
                    block52.InsertAfterSelf(new Paragraph(new Run(new Text(model.Morning815))));
                    block52.Remove();

                    SdtElement block53 = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txt3082").Single();
                    block53.InsertAfterSelf(new Paragraph(new Run(new Text(model.Morning830))));
                    block53.Remove();

                    SdtElement block54 = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txt4582").Single();
                    block54.InsertAfterSelf(new Paragraph(new Run(new Text(model.Morning845))));
                    block54.Remove();

                    SdtElement block55 = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txt92Tag").Single();
                    block55.InsertAfterSelf(new Paragraph(new Run(new Text(model.Morning900))));
                    block55.Remove();

                    SdtElement block56 = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtAfter2").Single();
                    block56.InsertAfterSelf(new Paragraph(new Run(new Text(model.Noon330))));
                    block56.Remove();

                    SdtElement block57 = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtEve2").Single();
                    block57.InsertAfterSelf(new Paragraph(new Run(new Text(model.Noon400))));
                    block57.Remove();

                    SdtElement block58 = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtRoutine2").Single();
                    block58.InsertAfterSelf(new Paragraph(new Run(new Text(model.Noon430))));
                    block58.Remove();

                    SdtElement block59 = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtNoon2").Single();
                    block59.InsertAfterSelf(new Paragraph(new Run(new Text(model.Noon500))));
                    block59.Remove();

                    SdtElement block60 = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtTypical2").Single();
                    block60.InsertAfterSelf(new Paragraph(new Run(new Text(model.Noon530))));
                    block60.Remove();

                    SdtElement block61 = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtCal62").Single();
                    block61.InsertAfterSelf(new Paragraph(new Run(new Text(model.Noon600))));
                    block61.Remove();

                    SdtElement block62 = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtNes32").Single();
                    block62.InsertAfterSelf(new Paragraph(new Run(new Text(model.Noon630))));
                    block62.Remove();

                    SdtElement block63 = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "ptxtOon72").Single();
                    block63.InsertAfterSelf(new Paragraph(new Run(new Text(model.Noon700))));
                    block63.Remove();

                    SdtElement block64 = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtMorning2").Single();
                    block64.InsertAfterSelf(new Paragraph(new Run(new Text(model.Noon730))));
                    block64.Remove();

                    SdtElement block65 = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtMis82").Single();
                    block65.InsertAfterSelf(new Paragraph(new Run(new Text(model.Noon800))));
                    block65.Remove();

                    SdtElement block66 = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtTable2").Single();
                    block66.InsertAfterSelf(new Paragraph(new Run(new Text(model.Noon830))));
                    block66.Remove();

                    SdtElement block67 = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtFeed2").Single();
                    block67.InsertAfterSelf(new Paragraph(new Run(new Text(model.Noon900))));
                    block67.Remove();

                    SdtElement block68 = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtMay2").Single();
                    block68.InsertAfterSelf(new Paragraph(new Run(new Text(model.Noon930))));
                    block68.Remove();

                    SdtElement block69 = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtJune2").Single();
                    block69.InsertAfterSelf(new Paragraph(new Run(new Text(model.Noon100))));
                    block69.Remove();

                    SdtElement block10to11 = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtData1").Single();
                    block10to11.InsertAfterSelf(new Paragraph(new Run(new Text(model.Noon10to11))));
                    block10to11.Remove();

                    SdtElement block11to12 = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtData2").Single();
                    block11to12.InsertAfterSelf(new Paragraph(new Run(new Text(model.Noon11to7))));
                    block11to12.Remove();

                    SdtElement blockLeis = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtLeisure2").Single();
                    blockLeis.InsertAfterSelf(new Paragraph(new Run(new Text(model.Leisure))));
                    blockLeis.Remove();

                    if (model.SignList != null)
                    {
                        if (model.SignList.Count > 0)
                        {
                            SdtBlock ccWithTableSign = mainPart.Document.Body.Descendants<SdtBlock>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtSignature").Single();
                            DocumentFormat.OpenXml.Wordprocessing.Table theTableSign = ccWithTableSign.Descendants<DocumentFormat.OpenXml.Wordprocessing.Table>().Single();
                            DocumentFormat.OpenXml.Wordprocessing.TableRow theRowSign = theTableSign.Elements<DocumentFormat.OpenXml.Wordprocessing.TableRow>().Last();
                            foreach (var item in model.SignList)
                            {
                                DocumentFormat.OpenXml.Wordprocessing.TableRow rowCopy4 = (DocumentFormat.OpenXml.Wordprocessing.TableRow)theRowSign.CloneNode(true);
                                rowCopy4.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(0).Append(new Paragraph
                                    (new Run(new Text(item.PrintName))));
                                rowCopy4.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(1).Append(new Paragraph
                                    (new Run(new Text(item.Signature))));
                                rowCopy4.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(2).Append(new Paragraph
                                    (new Run(new Text(item.Date))));
                                theTableSign.AppendChild(rowCopy4);
                            }
                            theTableSign.RemoveChild(theRowSign);
                        }
                    }

                    // Save the changes to the table back into the document.
                    mainPart.Document.Save();
                    theDoc.Close();
                    LookUp luk = new LookUp();
                    luk = dbobj.LookUps.Where(objluk => objluk.LookupType == "Document Type" && objluk.LookupName == "ProtocolSummary").SingleOrDefault();
                    ClientDB.DbModel.Document tblDoc = new ClientDB.DbModel.Document();
                    if (luk != null)
                    {

                        tblDoc.DocumentName = luk.LookupName + DateTime.Now;
                        tblDoc.DocumentType = luk.LookupId;
                    }

                    tblDoc.SchoolId = sess.SchoolId;
                    tblDoc.StudentPersonalId = sess.StudentId;
                    tblDoc.Status = true;
                    tblDoc.UserType = "Staff";
                    tblDoc.CreatedBy = sess.LoginId;
                    tblDoc.CreatedOn = DateTime.Now;
                    tblDoc.Varified = true;
                    dbobj.Documents.Add(tblDoc);
                    dbobj.SaveChanges();
                    int rtrnval = tblDoc.DocumentId;

                    string dwnld = NewPath;
                    string contentType = "application/msword";
                    string type = "Client";
                    byte[] fileContent = null;

                    System.IO.FileStream fs = new System.IO.FileStream(dwnld, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                    System.IO.BinaryReader binaryReader = new System.IO.BinaryReader(fs);
                    long byteLength = new System.IO.FileInfo(dwnld).Length;
                    fileContent = binaryReader.ReadBytes((Int32)byteLength);
                    fs.Close();
                    fs.Dispose();
                    binaryReader.Close();

                    binaryFile bin = new binaryFile();
                    bin.SchoolId = sess.SchoolId;
                    bin.DocumentName = "ProtocolSummary" + DateTime.Now;
                    bin.Active = true;
                    bin.DocId = rtrnval;
                    bin.Varified = true;
                    bin.StudentId = sess.StudentId;
                    bin.ContentType = contentType;
                    bin.Data = fileContent;
                    bin.type = type;
                    bin.ModuleName = "Protocol";
                    bin.CreatedBy = sess.LoginId;
                    bin.CreatedOn = DateTime.Now;
                    dbobj.binaryFiles.Add(bin);
                    dbobj.SaveChanges();

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return model;
        }

        private string CopyTemplate(string oldPath, string PageNo)
        {
            try
            {
                string Time = DateTime.Now.TimeOfDay.ToString();
                string[] ar = Time.Split('.');
                Time = ar[0];
                Time = Time.Replace(":", "-");
                string Datet = DateTime.Today.Day.ToString() + "-" + DateTime.Today.Month + "-" + DateTime.Today.Year.ToString() + "-" + Time;

                string path = HttpContext.Current.Server.MapPath("~\\Templates") + "\\Protocol";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                Guid g;

                g = Guid.NewGuid();

                sess = (clsSession)HttpContext.Current.Session["UserSessionClient"];
                string newpath = path + "\\";
                string ids = g.ToString();
                ids = ids.Replace("-", "");

                string newFileName = "ProtocolTemp" + ids.ToString();
                FileInfo f1 = new FileInfo(oldPath);
                if (f1.Exists)
                {
                    if (!Directory.Exists(newpath))
                    {
                        Directory.CreateDirectory(newpath);
                    }

                    f1.CopyTo(string.Format("{0}{1}{2}", newpath, newFileName, f1.Extension));
                }
                return newpath + newFileName + f1.Extension;
            }
            catch (Exception Ex)
            {
                return "";

            }


        }

        public string DisplayStudStatus()
        {
            sess = (clsSession)HttpContext.Current.Session["UserSessionClient"];
            dbobj = new BiWeeklyRCPNewEntities();
            string Status = "Active";
            int ClientID = sess.StudentId;
            int SchoolId = sess.SchoolId;
            var placementStatus = dbobj.StudentPersonals.Where(x => x.StudentPersonalId == ClientID && x.SchoolId == SchoolId).SingleOrDefault();
            if (placementStatus != null)
            {
                if (placementStatus.PlacementStatus == null || placementStatus.PlacementStatus == "A")
                {
                    return Status;
                }
                else if (placementStatus.PlacementStatus == "I")
                {
                    Status = "Inactive";
                }
                else if (placementStatus.PlacementStatus == "H")
                {
                    Status = "On-Hold";
                }
                else if (placementStatus.PlacementStatus == "D")
                {
                    Status = "Discharge";
                }
            }
            return Status;
        }
        public string DisplayStatus()
        {
            sess = (clsSession)HttpContext.Current.Session["UserSessionClient"];
            dbobj = new BiWeeklyRCPNewEntities();
            string Status = "Active";
            int ClientID = sess.StudentId, SchoolId = sess.SchoolId;
            var StatusCnt = dbobj.Placements.Where(x => x.StudentPersonalId == ClientID && x.SchoolId == SchoolId && x.Status == 1).Count();
            var PlacementList = dbobj.Placements.Where(x => x.StudentPersonalId == ClientID && x.SchoolId == SchoolId && x.Status == 1).ToList();
            //var placementStatus = dbobj.StudentPersonals.Where(x => x.StudentPersonalId == ClientID && x.SchoolId == SchoolId).SingleOrDefault();
            //if (placementStatus.PlacementStatus == null || placementStatus.PlacementStatus=="A")
            //{
            //    return Status;
            //}
            //else
            //{
            if (StatusCnt > 0)
            {
                var Listactivedata = dbobj.LookUps.Where(x => x.LookupType == "Placement Reason" && x.LookupDesc == "Active").ToList();
                foreach (var data in PlacementList)
                {
                    foreach (var stat in Listactivedata)
                    {
                        if (data.PlacementReason == stat.LookupId)
                        {
                            Status = "Active";
                            //UpdatePlacementDBStatus(Status, ClientID, SchoolId);
                            return Status;
                        }
                    }
                }

                var Listonholddata = dbobj.LookUps.Where(x => x.LookupType == "Placement Reason" && x.LookupDesc == "On-Hold").ToList();
                foreach (var data in PlacementList)
                {
                    if (data.PlacementReason == Listonholddata[0].LookupId)
                    {
                        Status = "On-Hold";
                        //UpdatePlacementDBStatus(Status, ClientID, SchoolId);
                        return Status;
                    }
                }

                var ListInactivedata = dbobj.LookUps.Where(x => x.LookupType == "Placement Reason" && x.LookupDesc == "Inactive").ToList();
                foreach (var data in PlacementList)
                {

                    if (data.PlacementReason == ListInactivedata[0].LookupId)
                    {
                        Status = "Inactive";
                        //UpdatePlacementDBStatus(Status, ClientID, SchoolId);
                        return Status;
                    }

                }
            }
            //}
            //UpdatePlacementDBStatus(Status, ClientID, SchoolId);
            return Status;
        }

        //public void UpdatePlacementDBStatus(string Status, int ClientID, int SchoolId)
        //{
        //    dbobj = new BiWeeklyRCPNewEntities();
        //    string UpdateStatus = "A";
        //    if (Status == "Active")
        //    {
        //        UpdateStatus = "A";
        //        HttpContext.Current.Session["PlacementStat"] = "A";
        //    }
        //    else if (Status == "On-Hold")
        //    {
        //        UpdateStatus = "H";
        //        HttpContext.Current.Session["PlacementStat"] = "A";
        //    }
        //    else if (Status == "Inactive")
        //    {
        //        UpdateStatus = "I";
        //        HttpContext.Current.Session["PlacementStat"] = "I";
        //    }
        //    var StudStatus = dbobj.StudentPersonals.Where(x => x.StudentPersonalId == ClientID && x.SchoolId == SchoolId).SingleOrDefault();
        //    if (StudStatus != null)
        //    {
        //        StudStatus.PlacementStatus = UpdateStatus;
        //    }
        //    dbobj.SaveChanges();
        //    //var StdStatus = dbobj.StudentPersonals.Where(x => x.StudentPersonalId == ClientID && x.SchoolId == SchoolId).SingleOrDefault();
        //    //HttpContext.Current.Session["PlacementStat"] = StdStatus.PlacementStatus;
        //}


        /// <summary>
        /// Function is used to delete Placement.
        /// </summary>
        /// <param name="ClientId"></param>
        /// <param name="itemId"></param>

        public void deletePlacement(int ClientId, int itemId)
        {
            dbobj = new BiWeeklyRCPNewEntities();
            PlacementModel PlacementModel = new PlacementModel();
            Placement placement = new Placement();
            placement = dbobj.Placements.Where(objPlacement => objPlacement.StudentPersonalId == ClientId && objPlacement.PlacementId == itemId).SingleOrDefault();
            placement.Status = 0;

            if (placement.Location != null)
            {
                DeleteFromClass(ClientId, (int)placement.Location, itemId);
            }
            string PlacementName = "";
            if (placement.Department != null && placement.Department > 0)
            {
                var Plcname = dbobj.LookUps.Where(x => x.LookupId == placement.Department).ToList();
                if (Plcname.Count > 0) PlacementName = Plcname[0].LookupName;
            }
            AddEventModel.CreateSystemEvent("Placement [" + PlacementName + "] was Deleted", "Placement Deleted", "Placement  " + PlacementName + "was Deleted");
            dbobj.SaveChanges();
        }

        public string SaveMedicalDatas(MedicalModel model)
        {
            sess = (clsSession)HttpContext.Current.Session["UserSessionClient"];
            dbobj = new BiWeeklyRCPNewEntities();
            int ClientID = sess.StudentId, SchoolId = sess.SchoolId;
            //dbobj = new BiWeeklyRCPNewEntities();
            MedicalAndInsurance Medical = new MedicalAndInsurance();          
            Insurance insurance = new Insurance();
            AddressList AddrList = new AddressList();
            StudentPersonalPA studentPA = new StudentPersonalPA();
            DiaganosesPA diagnose = new DiaganosesPA();
            // DateTime lastexam =DateTime.ParseExact(model.DateOfLastPhysicalExam, "yyyy/MM/dd",null);

            //string lastexam = model.DateOfLastPhysicalExam;
            //string dateformat = "yyyy-MM-dd";

            //DateTime dt = DateTime.Parse(lastexam.ToString(dateformat));
            //test = dbobj.MedicalAndInsurances.Where(objMedical => objMedical.StudentPersonalId == ClientID && objMedical.DateOfLastPhysicalExam == dt).SingleOrDefault();

            //var exmdate = dbobj.MedicalAndInsurances.Where(objMedical => objMedical.StudentPersonalId == ClientID && objMedical.DateOfLastPhysicalExam.Value.ToShortDateString() == model.DateOfLastPhysicalExam).SingleOrDefault();
            //if (exmdate != null)
            //{
            //    return "This date already available";
            //}

            if (model.ID > 0)
            {
                try
                {

                    string PhDate = model.DateOfLastPhysicalExam;
                    //Medical = dbobj.MedicalAndInsurances.Where(objMedical => objMedical.StudentPersonalId == ClientID && objMedical.DateOfLastPhysicalExam == PhDate).FirstOrDefault();
                    //if (Medical != null)
                    //{
                    //    return "Please go for some other Date";
                    //}
                  
                     Medical = dbobj.MedicalAndInsurances.Where(objMedical => objMedical.StudentPersonalId == ClientID &&   objMedical.SchoolId == sess.SchoolId).FirstOrDefault();
                    
                    if (Medical != null)
                    {
                        //Medical.Allergies = model.Allergies;
                        //Medical.Capabilities = model.Capabilities;
                        //Medical.PhysicianId = model.PhysicianId;
                        //// Medical.City = model.City;
                        // Medical.CountryId = model.CountryId;
                        Medical.CurrentMedications = model.CurrentMedications;
                       
                        try
                        {                           
                            Medical.DateOfLastPhysicalExam = DateTime.ParseExact(model.DateOfLastPhysicalExam,"MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        }
                        catch
                        {
                            Medical.DateOfLastPhysicalExam = null;
                        }
                   
                       // Medical.DateOfLastPhysicalExam = model.DateOfLastPhysicalExam;
                        //Medical.FirstName = model.FirstName;
                        //Medical.LastName = model.LastNames;
                        //Medical.Limitations = model.Limitations;
                        //Medical.MedicalConditionsDiagnosis = model.MedicalConditionsDiagnosis;
                        Medical.ModifiedBy = sess.LoginId;;// 1;
                        Medical.ModifiedOn = System.DateTime.Now;
                        //// Medical.OfficePhone = model.OfficePhone;
                        //Medical.Preferances = model.Preferances;
                        //Medical.SelfPreservationAbility = model.SelfPreservationAbility;
                        //Medical.SignificantBehaviorCharacteristics = model.SignificantBehaviorCharacteristics;
                        //// Medical.StateId = model.StateId;

                        //AddrList = dbobj.AddressLists.Where(objAdress => objAdress.AddressId == Medical.AddressId).SingleOrDefault();
                        //AddrList.CountryId = model.CountryId;
                        //AddrList.StateProvince = model.StateId;
                        //AddrList.City = model.City;
                        //AddrList.Phone = model.OfficePhone;
                        //// dbobj.AddressLists.Add(AddrList);

                        dbobj.SaveChanges();
                    }
                    else
                    {

                        Medical = new MedicalAndInsurance();
                        AddrList.CountryId = model.CountryId;
                        AddrList.StateProvince = model.StateId;
                        AddrList.City = model.City;
                        AddrList.Phone = model.OfficePhone;
                        dbobj.AddressLists.Add(AddrList);
                        dbobj.SaveChanges();
                        Medical.Allergies = model.Allergies;
                        Medical.Capabilities = model.Capabilities;
                        Medical.PhysicianId = model.PhysicianId;
                        //  Medical.City = model.City;
                        //  Medical.CountryId = model.CountryId;
                        Medical.CreatedBy = sess.LoginId;;//1;
                        Medical.CreatedOn = System.DateTime.Now;
                        Medical.CurrentMedications = model.CurrentMedications;
                        try
                        {
                            Medical.DateOfLastPhysicalExam = DateTime.ParseExact(model.DateOfLastPhysicalExam, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        }
                        catch
                        {
                            Medical.DateOfLastPhysicalExam = null;
                        }
                        Medical.FirstName = model.FirstName;
                        Medical.LastName = model.LastNames;
                        Medical.Limitations = model.Limitations;
                        Medical.MedicalConditionsDiagnosis = model.MedicalConditionsDiagnosis;
                        // Medical.OfficePhone = model.OfficePhone;
                        Medical.Preferances = model.Preferances;
                        Medical.SchoolId = SchoolId;
                        Medical.SelfPreservationAbility = model.SelfPreservationAbility;
                        Medical.SignificantBehaviorCharacteristics = model.SignificantBehaviorCharacteristics;
                        //  Medical.StateId = model.StateId;
                        Medical.StudentPersonalId = ClientID;
                        Medical.AddressId = AddrList.AddressId;
                        dbobj.MedicalAndInsurances.Add(Medical);
                        dbobj.SaveChanges();

                    }
                    studentPA = dbobj.StudentPersonalPAs.Where(objStudentPersonalPA => objStudentPersonalPA.StudentPersonalId == sess.StudentId
                          && objStudentPersonalPA.SchoolId == sess.SchoolId).SingleOrDefault();
                    if (studentPA == null)
                    {
                        studentPA = new StudentPersonalPA();
                        dbobj.StudentPersonalPAs.Add(studentPA);
                    }

                    studentPA.Allergies = model.Allergie;
                    studentPA.Diet = model.Diet;
                    studentPA.Seizures = model.Seizures;
                    studentPA.Other = model.Other;
                    //dbobj.StudentPersonalPAs.Add(studentPA);
                    dbobj.SaveChanges();

                    var diagnoses = dbobj.DiaganosesPAs.Where(objDiagnoses => objDiagnoses.StudentPersonalId == sess.StudentId).ToList();
                    int i = 0;
                    if (diagnoses.Count > 0)
                    {
                        foreach (var item in diagnoses)
                        {

                            //diagnose = dbobj.DiaganosesPAs.Where(objDiagno => objDiagno.DiaganosePAId == item.DiaganosePAId && objDiagno.SchoolId == sess.SchoolId).SingleOrDefault();
                            diagnose = dbobj.DiaganosesPAs.Where(objDiagno => objDiagno.DiaganosePAId == item.DiaganosePAId).SingleOrDefault();
                            diagnose.Diaganoses = model.Diagnosis[i].Name;
                            diagnose.ModifiedBy = sess.LoginId;
                            diagnose.ModifiedOn = DateTime.Now;
                            dbobj.SaveChanges();
                            i++;
                        }
                    }
                    else
                    {
                        foreach (var item in model.Diagnosis)
                        {
                            diagnose.StudentPersonalId = sess.StudentId;
                            diagnose.SchoolId = sess.SchoolId;
                            diagnose.Diaganoses = model.Diagnosis[i].Name;
                            diagnose.CreatedBy = sess.LoginId;
                            diagnose.CreatedOn = DateTime.Now;
                            dbobj.DiaganosesPAs.Add(diagnose);
                            dbobj.SaveChanges();
                            i++;
                        }

                    }
                       
                    return "Sucess";

                }
                catch (DbEntityValidationException dbEx)
                {
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                        }
                    }
                    return "Failed";
                }
            }
            else
            {
                if (ClientID == 0)
                {
                    return "No Client Selected";
                }
                else
                {
                    try
                    {
                        string PhDate = model.DateOfLastPhysicalExam;
                        Medical = dbobj.MedicalAndInsurances.Where(objMedical => objMedical.StudentPersonalId == ClientID && objMedical.SchoolId == sess.SchoolId).FirstOrDefault();
                         if (Medical != null)
                        {
                            return "Please go for some other Date";
                        }

                        Medical = new MedicalAndInsurance();
                        AddrList.CountryId = model.CountryId;
                        AddrList.StateProvince = model.StateId;
                        AddrList.City = model.City;
                        AddrList.Phone = model.OfficePhone;
                        dbobj.AddressLists.Add(AddrList);
                        dbobj.SaveChanges();
                        Medical.Allergies = model.Allergies;
                        Medical.Capabilities = model.Capabilities;
                        Medical.PhysicianId = model.PhysicianId;
                        //  Medical.City = model.City;
                        //  Medical.CountryId = model.CountryId;
                        Medical.CreatedBy = 1;
                        Medical.CreatedOn = System.DateTime.Now;
                        Medical.CurrentMedications = model.CurrentMedications;

                       // Medical.DateOfLastPhysicalExam = model.DateOfLastPhysicalExam;
                        Medical.FirstName = model.FirstName;
                        Medical.LastName = model.LastNames;
                        Medical.Limitations = model.Limitations;
                        Medical.MedicalConditionsDiagnosis = model.MedicalConditionsDiagnosis;
                        // Medical.OfficePhone = model.OfficePhone;
                        Medical.Preferances = model.Preferances;
                        Medical.SchoolId = SchoolId;
                        Medical.SelfPreservationAbility = model.SelfPreservationAbility;
                        Medical.SignificantBehaviorCharacteristics = model.SignificantBehaviorCharacteristics;
                        //  Medical.StateId = model.StateId;
                        Medical.StudentPersonalId = ClientID;
                        Medical.AddressId = AddrList.AddressId;
                        dbobj.MedicalAndInsurances.Add(Medical);



                        dbobj.SaveChanges();

                        studentPA.Allergies = model.Allergie;
                        studentPA.Diet = model.Diet;
                        studentPA.Seizures = model.Seizures;
                        studentPA.Other = model.Other;
                        dbobj.StudentPersonalPAs.Add(studentPA);
                        dbobj.SaveChanges();
                        for (int i = 0; i < model.Diagnosis.Count; i++)
                        {
                            diagnose.StudentPersonalId = ClientID;// sp.StudentPersonalId;
                            diagnose.SchoolId = sess.SchoolId;
                            diagnose.Diaganoses = model.Diagnosis[i].Name;
                            diagnose.CreatedBy = sess.LoginId;
                            diagnose.CreatedOn = DateTime.Now;
                            dbobj.DiaganosesPAs.Add(diagnose);
                            dbobj.SaveChanges();
                        }
                   
                  

                        return "Sucess";
                    }
                    catch (DbEntityValidationException dbEx)
                    {
                        foreach (var validationErrors in dbEx.EntityValidationErrors)
                        {
                            foreach (var validationError in validationErrors.ValidationErrors)
                            {
                                Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                            }
                        }
                        return "Failed";
                    }
                }

            }


        }

        //public ProtocolSummary FillProtocolSummary()
        //{
        //    sess = (clsSession)HttpContext.Current.Session["UserSessionClient"];
        //    ProtocolSummary returnModel = new ProtocolSummary();
        //    dbobj = new BiWeeklyRCPNewEntities();

        //    ProtocolSummary objPS = new ProtocolSummary();

        //    Protocol objP = new Protocol();
        //    Protocol_1_Assistive objP1 = new Protocol_1_Assistive();
        //    Protocol_2_Community objP2 = new Protocol_2_Community();
        //    Protocol_3_Family objP3 = new Protocol_3_Family();
        //    Protocol_4_Basic objP4 = new Protocol_4_Basic();

        //    objP = dbobj.Protocols.Where(m => m.ProtocolId == 5).SingleOrDefault();
        //    if (objP != null)
        //    {
        //        returnModel.HomeCommon = objP.HomeCommon;
        //    }

        //    return returnModel;
        //}

        public MedicalModel FillMedicalDatas(int id)
        {
            sess = (clsSession)HttpContext.Current.Session["UserSessionClient"];
            MedicalModel returnModel = new MedicalModel();
            MedicalAndInsurance tblObjMedicalInsurance = new MedicalAndInsurance();
            LookUp objLukup = new LookUp();
            AddressList AddrList = new AddressList();
            dbobj = new BiWeeklyRCPNewEntities();
            try
            {
                tblObjMedicalInsurance = dbobj.MedicalAndInsurances.Where(objMedicalInsurance => objMedicalInsurance.MedicalInsuranceId == id &&
                     objMedicalInsurance.StudentPersonalId == sess.StudentId).SingleOrDefault();
                if (tblObjMedicalInsurance != null)
                {
                    returnModel.Allergies = tblObjMedicalInsurance.Allergies;
                    returnModel.Capabilities = tblObjMedicalInsurance.Capabilities;
                    int addressid = (int)tblObjMedicalInsurance.AddressId;
                    AddrList = dbobj.AddressLists.Where(objAdress => objAdress.AddressId == addressid).SingleOrDefault();
                    returnModel.City = AddrList.City;
                    returnModel.CountryId = AddrList.CountryId;
                    objLukup = dbobj.LookUps.Where(objLookup => objLookup.LookupId == AddrList.CountryId && objLookup.LookupType == "Country").SingleOrDefault();
                    if (objLukup != null)
                    {
                        returnModel.Country = objLukup.LookupName;
                    }
                    returnModel.CurrentMedications = tblObjMedicalInsurance.CurrentMedications;
                   // returnModel.DateOfLastPhysicalExam = tblObjMedicalInsurance.DateOfLastPhysicalExam;
                    returnModel.FirstName = tblObjMedicalInsurance.FirstName;
                    returnModel.ID = tblObjMedicalInsurance.MedicalInsuranceId;
                    returnModel.LastNames = tblObjMedicalInsurance.LastName;
                    returnModel.Limitations = tblObjMedicalInsurance.Limitations;
                    returnModel.MedicalConditionsDiagnosis = tblObjMedicalInsurance.MedicalConditionsDiagnosis;
                    returnModel.OfficePhone = AddrList.Phone;
                    returnModel.Preferances = tblObjMedicalInsurance.Preferances;
                    returnModel.PhysicianId = tblObjMedicalInsurance.PhysicianId;
                    objLukup = dbobj.LookUps.Where(objlookup => objlookup.LookupId == tblObjMedicalInsurance.PhysicianId && objlookup.LookupType == "Physician").SingleOrDefault();
                    if (objLukup != null)
                    {
                        returnModel.Physician = objLukup.LookupName;
                    }

                    returnModel.SelfPreservationAbility = tblObjMedicalInsurance.SelfPreservationAbility;
                    returnModel.SignificantBehaviorCharacteristics = tblObjMedicalInsurance.SignificantBehaviorCharacteristics;
                    returnModel.StateId = AddrList.StateProvince;
                    objLukup = dbobj.LookUps.Where(objLookup => objLookup.LookupId == AddrList.StateProvince && objLookup.LookupType == "State").SingleOrDefault();
                    if (objLukup != null)
                    {
                        returnModel.State = objLukup.LookupName;
                    }
                }



            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
            }
            return returnModel;
        }

        public void getBindData(out string[] C1, int StudentId, int SchoolId)
        {
            dbobj = new BiWeeklyRCPNewEntities();
            StudentPersonal Student = new StudentPersonal();
            AddressList Address = new AddressList();
            ContactPersonal Contact = new ContactPersonal();
            StudentAddresRel AddressRel = new StudentAddresRel();
            StudentContactRelationship ContactRel = new StudentContactRelationship();
            string[] IEPC = new string[6];
            int Count = 6;
            try
            {


                Student = dbobj.StudentPersonals.Where(objStudentPersonal => objStudentPersonal.StudentPersonalId == StudentId).SingleOrDefault();

                IEPC[0] = Student.LastName + " " + Student.FirstName;
                IEPC[1] = Student.BirthDate.ToString();
                IEPC[2] = "Dummy";
                IEPC[3] = "Dummy";
                IEPC[4] = "Dummy";
                IEPC[5] = "Dummy";

            }
            catch (Exception Ex)
            {

            }

            C1 = new string[Count];
            if (IEPC != null) Array.Copy(IEPC, C1, Count);

        }


        //To set permission based on role of user
        public string setPermission()
        {
            clsSession session = null;
            BiWeeklyRCPNewEntities Objdata = new BiWeeklyRCPNewEntities();
            session = (clsSession)HttpContext.Current.Session["UserSessionClient"];
            string permission = "false";
            var Role = (from Objrole in Objdata.Roles
                        join objrgp in Objdata.RoleGroups on Objrole.RoleId equals objrgp.RoleId
                        select new
                        {
                            RoleId = Objrole.RoleId,
                            Roledesc = Objrole.RoleDesc,
                            schoolid = Objrole.SchoolId,
                            RoleCode = Objrole.RoleCode
                        }).ToList();
            var Usr = (from Objrole in Role
                       from Objusr in Objdata.Users
                       where Objusr.UserId == session.LoginId
                       select new
                       {
                           Objrole.RoleId,
                           Objrole.Roledesc,
                           Objusr.SchoolId,
                           Objusr.UserId,
                           Objusr.UserFName,
                           Objusr.UserLName,
                           Objusr.Gender,
                           Objrole.RoleCode

                       }).ToList();

            var rolePerm = (from objRoleGroupPermission in Objdata.RoleGroupPerms
                            join objObject in Objdata.Objects on objRoleGroupPermission.ObjectId equals objObject.ObjectId
                            join objRoleGroup in Objdata.RoleGroups on objRoleGroupPermission.RoleGroupId equals objRoleGroup.RoleGroupId
                            join objRole in Objdata.Roles on objRoleGroup.RoleId equals objRole.RoleId
                            join objUserRoleGroup in Objdata.UserRoleGroups on objRoleGroup.RoleGroupId equals objUserRoleGroup.RoleGroupId
                            where objObject.ObjectName == "General Client" && objUserRoleGroup.UserId == session.LoginId && objUserRoleGroup.ActiveInd == "A"
                            select new
                            {
                                ApproveInd = objRoleGroupPermission.WriteInd,
                                RoleGroupId = objRoleGroupPermission.RoleGroupId,
                                ObjectId = objRoleGroupPermission.ObjectId

                            }).ToList();

            if (Usr.Count() > 0)
            {
                if (rolePerm.Count > 0)
                {
                    permission = "false";
                    for (int i = 0; i < rolePerm.Count; i++)
                    {
                        if (rolePerm[i].ApproveInd == true)
                            permission = "true";
                    }


                }
                else
                    permission = "false";
            }

            return permission;
        }

        public int setClientPermission(string type)
        {

            int permisssion = -1; // 0-NONE; 1-READ ONLY; 2-WRITE;

            List<ClientPermissions> permissionList = new List<ClientPermissions>();
            permissionList = (List<ClientPermissions>)HttpContext.Current.Session["ClientPermissions"];


            try
            {
                foreach (var list in permissionList)
                {
                    if (list.objectName.Trim() == type.Trim())
                    {
                        if (list.WriteInd == true || list.WriteInd == null)
                        {
                           
                            permisssion = 2;
                        }
                        else if (list.ReadInd == true)
                        {
                            if (permisssion < 2)
                            {
                                permisssion = 1;
                            }
                        }
                        else
                        {
                            if (permisssion < 1)
                            {
                                permisssion = 0;
                            }
                        }

                        
                    }
                  
                }
                if(permisssion==-1)
                {
                    permisssion = 2;
                }
                return permisssion;
            }
            catch (Exception Ex)
            {
                if (permisssion == -1)
                {
                    permisssion = 2;
                }
                return permisssion;

            }


            return permisssion;

            //if (permissionList.Count() > 0)
            //{
            //    var list = permissionList.Where(x => x.objectName == type).SingleOrDefault();



            //    if (list.WriteInd == true || list.WriteInd == null)
            //    {
            //        permisssion = 2;
            //    }
            //    else if (list.ReadInd == true)
            //    {
            //        permisssion = 1;
            //    }
            //    else
            //    {
            //        permisssion = 0;
            //    }
            //}
        }

        public string SaveProtocolSummary(ProtocolSummary model)
        {
            string result = "";
            sess = (clsSession)HttpContext.Current.Session["UserSessionClient"];
            int ClientID = sess.StudentId, SchoolId = sess.SchoolId;
            dbobj = new BiWeeklyRCPNewEntities();
            ProtocolSummary objPS = new ProtocolSummary();
            StudentPersonalPA studentPA = new StudentPersonalPA();
            int pID;


            Protocol objP;

            var pID1 = (from m in dbobj.Protocols
                        where m.SchoolId == sess.SchoolId && m.StudentId == sess.StudentId
                        select m.ProtocolId).ToList();

            if (pID1.Count() > 0)
            {
                pID = pID1[0];
                objP = dbobj.Protocols.Where(n => n.ProtocolId.Equals(pID)).First();
            }

            else
            {
                pID = 0;
                objP = new Protocol();
            }


            try
            {

                objP.SchoolId = sess.SchoolId;
                objP.StudentId = sess.StudentId;

                objP.HomeCommon = model.HomeCommon;
                objP.HomeBedroom = model.HomeBedroom;
                objP.HomeBathroom = model.HomeBathroom;
                objP.Campus = model.Campus;
                objP.Community = model.Community;
                objP.SchoolCommon = model.SchoolCommon;
                objP.SchoolBathroom = model.SchoolBathroom;
                objP.SchoolOutside = model.SchoolOutside;
                objP.Pool = model.Pool;
                objP.Van = model.Van;



                objP.MasteredTask = model.MasteredTask;
                objP.NewTask = model.NewTask;

                //objP.Allergies = model.Allergies;
                //objP.SeizureInfo = model.SeizureInfo;
                objP.MedTimes = model.MedTimes;
                objP.TakeMed = model.TakeMed;
                objP.OtherMedical = model.OtherMedical;

                objP.DoctorVisit = model.DoctorVisit;
                objP.Dental = model.Dental;
                objP.BloodWork = model.BloodWork;
                objP.HairCuts = model.HairCuts;
                objP.OtherBehave = model.OtherBehave;

                objP.EatingGeneral = model.EatingGeneral;
                objP.EatingAble = model.EatingAble;
                objP.EatingNeed = model.EatingNeed;
                objP.EatingIep = model.EatingIep;

                objP.ToiletingGeneral = model.ToiletingGeneral;
                objP.ToiletingAble = model.ToiletingAble;
                objP.ToiletingNeed = model.ToiletingNeed;
                objP.ToiletingIep = model.ToiletingIep;

                objP.BrushingGeneral = model.BrushingGeneral;
                objP.BrushingAble = model.BrushingAble;
                objP.BrushingNeed = model.BrushingNeed;
                objP.BrushingIep = model.BrushingIep;

                objP.HandGeneral = model.HandGeneral;
                objP.HandAble = model.HandAble;
                objP.HandNeed = model.HandNeed;
                objP.HandIep = model.HandIep;

                objP.DressGeneral = model.DressGeneral;
                objP.DressAble = model.DressAble;
                objP.DressNeed = model.DressNeed;
                objP.DressIep = model.DressIep;

                objP.ShowerGeneral = model.ShowerGeneral;
                objP.ShowerAble = model.ShowerAble;
                objP.ShowerNeed = model.ShowerNeed;
                objP.ShowerIep = model.ShowerIep;

                objP.BedTime = model.BedTime;



                objP.Morning7 = model.Morning7;
                objP.Morning715 = model.Morning715;
                objP.Morning730 = model.Morning730;
                objP.Morning745 = model.Morning745;
                objP.Morning800 = model.Morning800;
                objP.Morning815 = model.Morning815;
                objP.Morning830 = model.Morning830;
                objP.Morning845 = model.Morning845;
                objP.Morning900 = model.Morning900;

                objP.Noon330 = model.Noon330;
                objP.Noon400 = model.Noon400;
                objP.Noon430 = model.Noon430;
                objP.Noon500 = model.Noon500;
                objP.Noon530 = model.Noon530;
                objP.Noon600 = model.Noon600;
                objP.Noon630 = model.Noon630;
                objP.Noon700 = model.Noon700;
                objP.Noon730 = model.Noon730;
                objP.Noon800 = model.Noon800;
                objP.Noon830 = model.Noon830;
                objP.Noon900 = model.Noon900;
                objP.Noon930 = model.Noon930;
                objP.Noon100 = model.Noon100;
                objP.Noon10to11 = model.Noon10to11;
                objP.Noon11to7 = model.Noon11to7;
                objP.Leisure = model.Leisure;
                objP.UpdtLOS = model.UpdtLOS;
                objP.UpdtTop = model.UpdtPageTop;
                objP.UpdtTPH = model.UpdtTPH;
                objP.UpdtMedInfo = model.UpdtMedInfo;
                objP.UpdtPerCare = model.UpdtPerCare;
                objP.UpdtBehInfo = model.UpdtBehInfo;
                objP.UpdtTypRoutines = model.UpdtTypRoutines;
                objP.UpdtAtList = model.UpdtATList;
                objP.UpdtCGList = model.UpdtCGList;
                objP.UpdtFIList = model.UpdtFIList;
                objP.UpdtBBIList = model.UpdtBBIList;



                //objP1.Type=model.ATList[].Type;

                objP.CreatedBy = sess.LoginId;
                objP.CreatedOn = DateTime.Now;
                objP.ModifiedBy = sess.LoginId;
                objP.ModifiedOn = DateTime.Now;

                if (pID == null || pID == 0)
                {
                    dbobj.Protocols.Add(objP);
                    dbobj.SaveChanges();
                }
                else
                {

                    dbobj.SaveChanges();
                }

                var objP1 = new Protocol_1_Assistive();
                foreach (var item in model.ATList)
                {
                    if (item.AssistiveId == 0)
                    {
                        if (item.Type != null || item.ScheduleForUse != null || item.StorageLocation != null)
                        {
                            objP1 = new Protocol_1_Assistive();
                            objP1.ProtocolId = objP.ProtocolId;
                            objP1.Type = item.Type;
                            objP1.ScheduleForUse = item.ScheduleForUse;
                            objP1.StorageLocation = item.StorageLocation;
                            objP1.CreatedBy = sess.LoginId;
                            objP1.CreatedOn = DateTime.Now;
                            dbobj.Protocol_1_Assistive.Add(objP1);
                            dbobj.SaveChanges();
                        }

                    }
                    else
                    {
                        if (item.Type != null || item.ScheduleForUse != null || item.StorageLocation != null)
                        {
                            objP1 = dbobj.Protocol_1_Assistive.Where(n => n.AssistiveId.Equals(item.AssistiveId)).First();
                            //objP1 = new Protocol_1_Assistive();
                            objP1.ProtocolId = objP.ProtocolId;
                            objP1.Type = item.Type;
                            objP1.ScheduleForUse = item.ScheduleForUse;
                            objP1.StorageLocation = item.StorageLocation;
                            objP1.ModifiedBy = sess.LoginId;
                            objP1.ModifiedOn = DateTime.Now;

                            dbobj.SaveChanges();
                        }
                    }
                }

                //var pID2 = (from m in dbobj.Protocol_1_Assistive
                //            join n in dbobj.Protocols
                //            on m.ProtocolId equals n.ProtocolId
                //            where n.SchoolId == sess.SchoolId && n.StudentId == sess.StudentId
                //            select m.AssistiveId).ToList();
                //if (pID2.Count() > 0)
                //{
                //    pID = pID2[0];
                //    objP1 = dbobj.Protocol_1_Assistive.Where(n => n.AssistiveId.Equals(pID)).First();
                //}
                //else
                //{
                //    pID = 0;
                //    objP1 = new Protocol_1_Assistive();
                //}

                //foreach (var item in model.ATList)
                //{
                //    //var objP1 = new Protocol_1_Assistive();
                //    //objP1 = dbobj.Protocol_1_Assistive.Where(n => n.AssistiveId.Equals(pID)).First();

                //    if (pID2.Count() > 0)
                //    {
                //        //pID = pID2[0];
                //        pID = item.AssistiveId;
                //        if (pID != 0)
                //        {
                //            objP1 = dbobj.Protocol_1_Assistive.Where(n => n.AssistiveId.Equals(pID)).First();
                //        }
                //    }
                //    else
                //    {
                //        pID = 0;
                //        objP1 = new Protocol_1_Assistive();
                //    }

                //    if (item.AssistiveId == 0)
                //    {
                //        pID = 0;
                //        objP1 = new Protocol_1_Assistive();
                //    }

                //    objP1.ProtocolId = objP.ProtocolId;
                //    objP1.Type = item.Type;
                //    objP1.ScheduleForUse = item.ScheduleForUse;
                //    objP1.StorageLocation = item.StorageLocation;
                //    //objP1.UpdtATList = item.UpdtATList;

                //    objP1.CreatedBy = sess.LoginId;
                //    objP1.CreatedOn = DateTime.Now;
                //    objP1.ModifiedBy = sess.LoginId;
                //    objP1.ModifiedOn = DateTime.Now;

                //    if (pID == null || pID == 0)
                //    {
                //        dbobj.Protocol_1_Assistive.Add(objP1);
                //        dbobj.SaveChanges();
                //    }
                //    else
                //    {
                //        dbobj.SaveChanges();
                //    }

                //    //pID++;

                //}

                var objP2 = new Protocol_2_Community();
                foreach (var item in model.CGList)
                {
                    if (item.CommunityId == 0)
                    {
                        if (item.TypeA != null || item.TypeB != null)
                        {
                            objP2 = new Protocol_2_Community();
                            objP2.ProtocolId = objP.ProtocolId;
                            objP2.TypeA = item.TypeA;
                            objP2.TypeB = item.TypeB;
                            objP2.CreatedBy = sess.LoginId;
                            objP2.CreatedOn = DateTime.Now;
                            dbobj.Protocol_2_Community.Add(objP2);
                            dbobj.SaveChanges();
                        }
                    }
                    else
                    {
                        if (item.TypeA != null || item.TypeB != null)
                        {
                            objP2 = dbobj.Protocol_2_Community.Where(n => n.CommunityId.Equals(item.CommunityId)).First();
                            objP2.ProtocolId = objP.ProtocolId;
                            objP2.TypeA = item.TypeA;
                            objP2.TypeB = item.TypeB;
                            objP2.ModifiedBy = sess.LoginId;
                            objP2.ModifiedOn = DateTime.Now;
                            dbobj.SaveChanges();
                        }
                    }
                }

                var objP3 = new Protocol_3_Family();
                foreach (var item in model.FIList)
                {
                    if (item.FamilyId == 0)
                    {
                        if (item.FamilyOne != null || item.FamilyTwo != null)
                        {
                            objP3 = new Protocol_3_Family();
                            objP3.ProtocolId = objP.ProtocolId;
                            objP3.FamilyOne = item.FamilyOne;
                            objP3.FamilyTwo = item.FamilyTwo;
                            objP3.CreatedBy = sess.LoginId;
                            objP3.CreatedOn = DateTime.Now;
                            dbobj.Protocol_3_Family.Add(objP3);
                            dbobj.SaveChanges();
                        }
                    }
                    else
                    {
                        if (item.FamilyOne != null || item.FamilyTwo != null)
                        {
                            objP3 = dbobj.Protocol_3_Family.Where(n => n.FamilyId.Equals(item.FamilyId)).First();
                            objP3.ProtocolId = objP.ProtocolId;
                            objP3.FamilyOne = item.FamilyOne;
                            objP3.FamilyTwo = item.FamilyTwo;
                            objP3.ModifiedBy = sess.LoginId;
                            objP3.ModifiedOn = DateTime.Now;
                            dbobj.SaveChanges();
                        }
                    }
                }

                var objP4 = new Protocol_4_Basic();
                foreach (var item in model.BBIList)
                {
                    if (item.BasicId == 0)
                    {
                        if (item.Acceleration != null || item.Strategy != null)
                        {
                            objP4 = new Protocol_4_Basic();
                            objP4.ProtocolId = objP.ProtocolId;
                            objP4.Acceleration = item.Acceleration;
                            objP4.Strategy = item.Strategy;
                            objP4.CreatedBy = sess.LoginId;
                            objP4.CreatedOn = DateTime.Now;
                            dbobj.Protocol_4_Basic.Add(objP4);
                            dbobj.SaveChanges();
                        }
                    }
                    else
                    {
                        if (item.Acceleration != null || item.Strategy != null)
                        {
                            objP4 = dbobj.Protocol_4_Basic.Where(n => n.BasicId.Equals(item.BasicId)).First();
                            objP4.ProtocolId = objP.ProtocolId;
                            objP4.Acceleration = item.Acceleration;
                            objP4.Strategy = item.Strategy;
                            objP4.ModifiedBy = sess.LoginId;
                            objP4.ModifiedOn = DateTime.Now;
                            dbobj.SaveChanges();
                        }
                    }
                }

                var objP5 = new Protocol_5_Signature();
                foreach (var item in model.SignList)
                {
                    if (item.SignatureId == 0)
                    {
                        if (item.PrintName != null || item.Signature != null || item.Date != null)
                        {
                            objP5 = new Protocol_5_Signature();
                            objP5.ProtocolId = objP.ProtocolId;
                            objP5.PrintName = item.PrintName;
                            objP5.Signature = item.Signature;
                            objP5.Date = item.Date;
                            objP5.CreatedBy = sess.LoginId;
                            objP5.CreatedOn = DateTime.Now;
                            dbobj.Protocol_5_Signature.Add(objP5);
                            dbobj.SaveChanges();
                        }
                    }
                    else
                    {
                        if (item.PrintName != null || item.Signature != null || item.Date != null)
                        {
                            objP5 = dbobj.Protocol_5_Signature.Where(n => n.SignatureId.Equals(item.SignatureId)).First();
                            objP5.ProtocolId = objP.ProtocolId;
                            objP5.PrintName = item.PrintName;
                            objP5.Signature = item.Signature;
                            objP5.Date = item.Date;
                            objP5.ModifiedBy = sess.LoginId;
                            objP5.ModifiedOn = DateTime.Now;
                            dbobj.SaveChanges();
                        }
                    }
                }

                studentPA = dbobj.StudentPersonalPAs.Where(objStudentPersonalPA => objStudentPersonalPA.StudentPersonalId == sess.StudentId
                            && objStudentPersonalPA.SchoolId == sess.SchoolId).SingleOrDefault();
                if (studentPA == null)
                {
                    studentPA = new StudentPersonalPA();
                    dbobj.StudentPersonalPAs.Add(studentPA);
                }

                studentPA.Allergies = model.Allergies;
                studentPA.Seizures = model.SeizureInfo;
                dbobj.SaveChanges();

                result = "sucess";
            }
            catch
            {
                result = "failed";
            }
            return result;
        }


        public string SaveReportData(ProgressList model, bool visibleCheckBox)
        {
            string result = "";
            sess = (clsSession)HttpContext.Current.Session["UserSessionClient"];
            int ClientID = sess.StudentId, SchoolId = sess.SchoolId;
            dbobj = new BiWeeklyRCPNewEntities();

            Progress_Report rpt = new Progress_Report();
            // ParentServiceReference.c
            ParentServiceReference.ParentServiceClient obj = new ParentServiceReference.ParentServiceClient();

            //  List<Progress> goaldata=new List<Progress>();



            try
            {
                foreach (var item in model.GoalData)
                {
                    foreach (var items in item.RptList)
                    {

                        if (items.rptid == 0)
                        {
                            // DateTime date = items.rptdate;
                            rpt.Report_Date = items.rptdate;// DateTime.ParseExact(items.rptdate, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                            rpt.Report_Info = items.rptinfo;
                            rpt.AllowVisible = visibleCheckBox;
                            //var GoalIds = from objStdtLessonPlan in dbobj.StdtLessonPlans
                            //              join ObjGoalLPRel in dbobj.GoalLPRels
                            //                  //on objStdtLessonPlan.GoalId equals  ObjGoalLPRel.GoalId && objStdtLessonPlan.LessonPlanId  equals  ObjGoalLPRel.LessonPlanId  
                            //              on new { a = (int)objStdtLessonPlan.GoalId, b = objStdtLessonPlan.LessonPlanId }
                            //              equals new { a = ObjGoalLPRel.GoalId, b = ObjGoalLPRel.LessonPlanId }
                            //              where objStdtLessonPlan.StdtIEPId == sess.IEPId
                            //              select new
                            //              {
                            //                  ObjGoalLPRel.GoalLPRelId,

                            //              };

                            rpt.GoalId = item.GoalLPRelId;
                            rpt.StdtIEPId = sess.IEPId;
                            rpt.CreatedBy = sess.LoginId;
                            rpt.CreatedOn = DateTime.Now;
                            rpt.ModifiedBy = sess.LoginId;
                            rpt.ModifiedOn = DateTime.Now;
                            dbobj.Progress_Report.Add(rpt);
                            dbobj.SaveChanges();
                            result = "sucess";
                        }
                        else
                        {
                            rpt = dbobj.Progress_Report.Where(objProgreeRPT => objProgreeRPT.Report_Id == items.rptid).SingleOrDefault();
                            rpt.Report_Date = items.rptdate;// DateTime.ParseExact(items.rptdate, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                            rpt.Report_Info = items.rptinfo;
                            rpt.AllowVisible = visibleCheckBox;
                            rpt.GoalId = item.GoalLPRelId;
                            rpt.StdtIEPId = sess.IEPId;
                            rpt.CreatedBy = sess.LoginId;
                            rpt.CreatedOn = DateTime.Now;
                            rpt.ModifiedBy = sess.LoginId;
                            rpt.ModifiedOn = DateTime.Now;
                            dbobj.SaveChanges();
                            result = "sucess";
                        }

                    }
                }
            }
            catch
            {
                result = "failed";
            }



            return result;
        }






        public IList<StudentSearchDetails> GetStudentSearch(string SearchName, int page = 1, int pageSize = 10)
        {
            BiWeeklyRCPNewEntities objData = new BiWeeklyRCPNewEntities();
            IList<StudentSearchDetails> val = null;
            StudentSearchDetails obj = new StudentSearchDetails();
            //PagingModel Pgmodel = new PagingModel();
            //obj.pageModel.CurrentPageIndex = page;
            //obj.pageModel.PageSize = pageSize;
            string studentype = "Referral";
            session = (clsSession)HttpContext.Current.Session["UserSessionClient"];
            //clsReferral objClsRef = new clsReferral();
            int n;
            bool isNumeric = int.TryParse(SearchName, out n);
            if (session != null)
            {
                int ReferralId = 0;

                if (SearchName != "undefined" && SearchName != "" && SearchName != null)
                {
                    string[] args = SearchName.Split('$');
                    if (args.Length > 1)
                    {
                        if (args[1].ToString() == "Referral")
                            studentype = "Referral";
                        else
                            studentype = "Client";
                    }
                    else
                        studentype = "Referral";
                    if (args[0] != "undefined" && args[0] != "" && args[0] != null)
                    {
                        string name = args[0].ToString();
                        val = (from objref in objData.StudentPersonals
                               where (objref.StudentType == studentype && (objref.LastName.ToLower().StartsWith(name.ToLower()) || objref.FirstName.ToLower().StartsWith(name.ToLower()) || (objref.LastName.ToLower() + "," + objref.FirstName.ToLower()).Contains(name.ToLower()) || objref.StudentPersonalId.Equals(n)))
                               select new StudentSearchDetails
                               {
                                   ReferralId = objref.StudentPersonalId,
                                   Gender = objref.Gender,
                                   BirthDate = SqlFunctions.StringConvert((double)objref.BirthDate.Value.Month).TrimStart() + "/" + SqlFunctions.DateName("day", objref.BirthDate).Trim() + "/" + SqlFunctions.DateName("year", objref.BirthDate),
                                   AdmissionDate = SqlFunctions.StringConvert((double)objref.AdmissionDate.Value.Month).TrimStart() + "/" + SqlFunctions.DateName("day", objref.AdmissionDate).Trim() + "/" + SqlFunctions.DateName("year", objref.AdmissionDate),
                                   ReferralName = (objref.LastName ?? "") + "," + (objref.FirstName ?? ""),
                                   LastQueue = "",
                                   FundingVerification = objref.FundingVerification,
                                   InactiveList = objref.InactiveList,
                                   WaitingList = objref.WaitingList,
                               }).ToList();
                    }
                    else
                    {
                        val = (from objref in objData.StudentPersonals
                               where (objref.StudentType == studentype)
                               select new StudentSearchDetails
                               {
                                   ReferralId = objref.StudentPersonalId,
                                   Gender = objref.Gender,
                                   BirthDate = SqlFunctions.StringConvert((double)objref.BirthDate.Value.Month).TrimStart() + "/" + SqlFunctions.DateName("day", objref.BirthDate).Trim() + "/" + SqlFunctions.DateName("year", objref.BirthDate),
                                   AdmissionDate = SqlFunctions.StringConvert((double)objref.AdmissionDate.Value.Month).TrimStart() + "/" + SqlFunctions.DateName("day", objref.AdmissionDate).Trim() + "/" + SqlFunctions.DateName("year", objref.AdmissionDate),
                                   ReferralName = (objref.LastName ?? "") + "," + (objref.FirstName ?? ""),
                                   LastQueue = "",
                                   FundingVerification = objref.FundingVerification,
                                   InactiveList = objref.InactiveList,
                                   WaitingList = objref.WaitingList,
                               }).ToList();
                    }

                }
                else
                {
                    val = (from objref in objData.StudentPersonals
                           where (objref.StudentType == studentype)
                           select new StudentSearchDetails
                           {
                               ReferralId = objref.StudentPersonalId,
                               Gender = objref.Gender,
                               BirthDate = SqlFunctions.StringConvert((double)objref.BirthDate.Value.Month).TrimStart() + "/" + SqlFunctions.DateName("day", objref.BirthDate).Trim() + "/" + SqlFunctions.DateName("year", objref.BirthDate),
                               AdmissionDate = SqlFunctions.StringConvert((double)objref.AdmissionDate.Value.Month).TrimStart() + "/" + SqlFunctions.DateName("day", objref.AdmissionDate).Trim() + "/" + SqlFunctions.DateName("year", objref.AdmissionDate),
                               ReferralName = (objref.LastName ?? "") + "," + (objref.FirstName ?? ""),
                               LastQueue = "",
                               FundingVerification = objref.FundingVerification,
                               InactiveList = objref.InactiveList,
                               WaitingList = objref.WaitingList,
                           }).ToList();

                }


                if (val != null)
                {
                    if (val.Count > 0)
                    {
                        foreach (var item in val)
                        {
                            ReferralId = item.ReferralId;
                            var QueueLevel = objData.ref_QueueStatus.Where(x => x.StudentPersonalId == ReferralId && x.CurrentStatus == true).OrderByDescending(x => x.QueueStatusId).FirstOrDefault();
                            if (QueueLevel != null)
                            {

                                item.QueueId = ReferralId.ToString() + "_" + QueueLevel.QueueId.ToString();
                                var QueueName = objData.ref_Queue.Where(x => x.QueueId == QueueLevel.QueueId).ToList();
                                if (QueueName.Count > 0)
                                {
                                    item.LastQueue = QueueName[0].QueueName;
                                }

                            }
                            //var QueueLevel = objData.ref_QueueStatus.Where(x => x.StudentPersonalId == ReferralId && x.CurrentStatus == true).OrderByDescending(x => x.QueueStatusId).FirstOrDefault();

                        }
                    }
                }


            }
            // obj.pageModel.TotalRecordCount = val.Count;
            // val = val.OrderByDescending(objcall => objcall.ReferralId).Skip((page - 1) * pageSize).Take(pageSize).ToList();
            //// obj.= val;
            // if (obj.pageModel.PageSize > obj.pageModel.TotalRecordCount) { obj.pageModel.PageSize = obj.pageModel.TotalRecordCount; }
            // if (obj.pageModel.TotalRecordCount == 0) { obj.pageModel.CurrentPageIndex = 0; }

            // return obj;
            return val;
        }

        public IList<StudentSearchDetails> GetStudentSearch_ref(string SearchName, int page = 1, int pageSize = 10)
        {
            BiWeeklyRCPNewEntities objData = new BiWeeklyRCPNewEntities();
            IList<StudentSearchDetails> val = null;
            StudentSearchDetails obj = new StudentSearchDetails();
            //PagingModel Pgmodel = new PagingModel();
            //obj.pageModel.CurrentPageIndex = page;
            //obj.pageModel.PageSize = pageSize;

            session = (clsSession)HttpContext.Current.Session["UserSessionClient"];
            //clsReferral objClsRef = new clsReferral();
            string studentype = "Referral";
            if (session != null)
            {
                int ReferralId = 0;

                if (SearchName != "undefined" && SearchName != "" && SearchName != null)
                {
                    string[] args = SearchName.Split('$');
                    if (args.Length > 1)
                    {
                        if (args[1].ToString() == "referral")
                            studentype = "Referral";
                        else
                            studentype = "Client";
                    }
                    else
                        studentype = "Referral";
                    bool isNumber = System.Text.RegularExpressions.Regex.IsMatch(args[0].Trim(), @"^\d+$");

                    if (!isNumber)
                    {
                        string name = args[0].ToString();
                        val = (from objref in objData.StudentPersonals
                               where (objref.StudentType == studentype && (objref.LastName.ToLower().StartsWith(name.ToLower()) || objref.FirstName.ToLower().StartsWith(name.ToLower()) || (objref.LastName.ToLower() + "," + objref.FirstName.ToLower()).Contains(name.ToLower())))
                               select new StudentSearchDetails
                               {
                                   ReferralId = objref.StudentPersonalId,
                                   Gender = objref.Gender,
                                   BirthDate = SqlFunctions.StringConvert((double)objref.BirthDate.Value.Month).TrimStart() + "/" + SqlFunctions.DateName("day", objref.BirthDate).Trim() + "/" + SqlFunctions.DateName("year", objref.BirthDate),
                                   AdmissionDate = SqlFunctions.StringConvert((double)objref.AdmissionDate.Value.Month).TrimStart() + "/" + SqlFunctions.DateName("day", objref.AdmissionDate).Trim() + "/" + SqlFunctions.DateName("year", objref.AdmissionDate),
                                   ReferralName = SqlFunctions.StringConvert((double)objref.StudentPersonalId).Trim() + "|" + (objref.LastName ?? "") + "," + (objref.FirstName ?? ""),
                                   LastQueue = "",
                                   FundingVerification = objref.FundingVerification,
                                   InactiveList = objref.InactiveList,
                                   WaitingList = objref.WaitingList,
                                   ReferralName_short = (objref.LastName ?? "") + "," + (objref.FirstName ?? ""),
                               }).ToList();
                    }
                    else
                    {

                        int searchNum = Convert.ToInt32(args[0].Trim());
                        string name = args[0].ToString();
                        val = (from objref in objData.StudentPersonals
                               where (objref.StudentType == studentype && (objref.LastName.ToLower().StartsWith(name.ToLower()) || objref.FirstName.ToLower().StartsWith(name.ToLower()) || (objref.LastName.ToLower() + "," + objref.FirstName.ToLower()).Contains(name.ToLower()) || objref.StudentPersonalId == searchNum))
                               select new StudentSearchDetails
                               {
                                   ReferralId = objref.StudentPersonalId,
                                   Gender = objref.Gender,
                                   BirthDate = SqlFunctions.StringConvert((double)objref.BirthDate.Value.Month).TrimStart() + "/" + SqlFunctions.DateName("day", objref.BirthDate).Trim() + "/" + SqlFunctions.DateName("year", objref.BirthDate),
                                   AdmissionDate = SqlFunctions.StringConvert((double)objref.AdmissionDate.Value.Month).TrimStart() + "/" + SqlFunctions.DateName("day", objref.AdmissionDate).Trim() + "/" + SqlFunctions.DateName("year", objref.AdmissionDate),
                                   ReferralName = SqlFunctions.StringConvert((double)objref.StudentPersonalId).Trim() + "|" + (objref.LastName ?? "") + "," + (objref.FirstName ?? ""),
                                   LastQueue = "",
                                   FundingVerification = objref.FundingVerification,
                                   InactiveList = objref.InactiveList,
                                   WaitingList = objref.WaitingList,
                                   ReferralName_short = (objref.LastName ?? "") + "," + (objref.FirstName ?? ""),
                               }).ToList();
                    }

                }
                else
                {
                    val = (from objref in objData.StudentPersonals
                           where (objref.StudentType == studentype)
                           select new StudentSearchDetails
                           {
                               ReferralId = objref.StudentPersonalId,
                               Gender = objref.Gender,
                               BirthDate = SqlFunctions.StringConvert((double)objref.BirthDate.Value.Month).TrimStart() + "/" + SqlFunctions.DateName("day", objref.BirthDate).Trim() + "/" + SqlFunctions.DateName("year", objref.BirthDate),
                               AdmissionDate = SqlFunctions.StringConvert((double)objref.AdmissionDate.Value.Month).TrimStart() + "/" + SqlFunctions.DateName("day", objref.AdmissionDate).Trim() + "/" + SqlFunctions.DateName("year", objref.AdmissionDate),
                               ReferralName = (objref.LastName ?? "") + "," + (objref.FirstName ?? ""),
                               LastQueue = "",
                               FundingVerification = objref.FundingVerification,
                               InactiveList = objref.InactiveList,
                               WaitingList = objref.WaitingList,
                           }).ToList();

                }


                if (val != null)
                {
                    if (val.Count > 0)
                    {
                        foreach (var item in val)
                        {
                            ReferralId = item.ReferralId;
                            var QueueLevel = objData.ref_QueueStatus.Where(x => x.StudentPersonalId == ReferralId && x.CurrentStatus == true).OrderByDescending(x => x.QueueStatusId).FirstOrDefault();
                            if (QueueLevel != null)
                            {
                                item.QueueId = ReferralId.ToString() + "_" + QueueLevel.QueueId.ToString();
                                var QueueName = objData.ref_Queue.Where(x => x.QueueId == QueueLevel.QueueId).FirstOrDefault();
                                item.LastQueue = QueueName.QueueName;
                            }
                            //var QueueLevel = objData.ref_QueueStatus.Where(x => x.StudentPersonalId == ReferralId && x.CurrentStatus == true).OrderByDescending(x => x.QueueStatusId).FirstOrDefault();

                        }
                    }
                }


            }
            // obj.pageModel.TotalRecordCount = val.Count;
            // val = val.OrderByDescending(objcall => objcall.ReferralId).Skip((page - 1) * pageSize).Take(pageSize).ToList();
            //// obj.= val;
            // if (obj.pageModel.PageSize > obj.pageModel.TotalRecordCount) { obj.pageModel.PageSize = obj.pageModel.TotalRecordCount; }
            // if (obj.pageModel.TotalRecordCount == 0) { obj.pageModel.CurrentPageIndex = 0; }

            // return obj;
            return val;
        }


        public IList<StaffSearchDetails> GetStaffList(string SearchName)
        {
            BiWeeklyRCPNewEntities objData = new BiWeeklyRCPNewEntities();
            IList<StaffSearchDetails> val = null;
            session = (clsSession)HttpContext.Current.Session["UserSessionClient"];
            //clsReferral objClsRef = new clsReferral();

            if (session != null)
            {

                if (SearchName != "undefined" && SearchName != "" && SearchName != null)
                {
                    val = (from objref in objData.Users
                           where (objref.ActiveInd == "A" && (objref.UserLName.ToLower().StartsWith(SearchName.ToLower()) || objref.UserFName.ToLower().StartsWith(SearchName.ToLower()) || (objref.UserLName.ToLower() + "," + objref.UserFName.ToLower()).Contains(SearchName.ToLower())))
                           select new StaffSearchDetails
                           {
                               UserId = objref.UserId,
                               UserName = (objref.UserLName ?? "") + "," + (objref.UserFName ?? ""),
                           }).ToList();


                }
                else
                {
                    val = (from objref in objData.Users
                           select new StaffSearchDetails
                           {
                               UserId = objref.UserId,
                               UserName = (objref.UserLName ?? "") + "," + (objref.UserFName ?? ""),
                           }).ToList();

                }




            }
            return val;
        }

        public IList<ContactNameSearchDetails> GetContactNameList(string SearchName)
        {
            BiWeeklyRCPNewEntities objData = new BiWeeklyRCPNewEntities();
            IList<ContactNameSearchDetails> val = null;
            session = (clsSession)HttpContext.Current.Session["UserSessionClient"];
            //session1 = (clsSession1)HttpContext.Current.Session["UserSession1"];

            //clsReferral objClsRef = new clsReferral();

            if (session != null)
            {

                if (SearchName != "undefined" && SearchName != "" && SearchName != null)
                {
                    val = (from objref in objData.ContactPersonals
                           where (objref.StudentPersonalId == session.StudentId)
                           select new ContactNameSearchDetails
                           {
                               ContactId = objref.ContactPersonalId,
                               ContactName = (objref.LastName ?? "") + "," + (objref.FirstName ?? "") + "," + (objref.MiddleName ?? ""),

                           }).ToList();


                }
                else
                {
                    val = (from objref in objData.ContactPersonals
                           where (objref.StudentPersonalId == session.StudentId)
                           select new ContactNameSearchDetails
                           {
                               ContactId = objref.ContactPersonalId,
                               ContactName = (objref.LastName ?? "") + "," + (objref.FirstName ?? "") + "," + (objref.MiddleName ?? ""),
                           }).ToList();

                }


                val.Add(new ContactNameSearchDetails { ContactId = 0, ContactName = "Others" });


            }
            return val;
        }

        public string amPmTo24hourConverter(string time)
        {
            string[] Alltime = time.Split(':');
            int h = Convert.ToInt16(Alltime[0]);
            string startAMPM = Alltime[1].Substring(2, 2);
            string m = Alltime[1].Substring(0, 2);
            if (startAMPM == "PM")
            {
                if (h != 12)
                {
                    h += 12;
                }

            }
            else if (startAMPM == "AM")
            {
                if (h == 12)
                {
                    h = 00;
                }
            }
            return (h.ToString() + ":" + m.ToString());
        }

        public int RemoveFromClass(int StudentId, int ClassId, int placementId)
        {
            BiWeeklyRCPNewEntities objData = new BiWeeklyRCPNewEntities();
            IList<ContactNameSearchDetails> val = null;
            IList<GridListPlacement> retunmodel = new List<GridListPlacement>();
            session = (clsSession)HttpContext.Current.Session["UserSessionClient"];

            var currLocationId = objData.Placements.Where(x => x.PlacementId == placementId).ToList();

            int? currLocation = currLocationId[0].Location;
            if (currLocation != ClassId)
            {
                try
                {
                    retunmodel = (from objPlacement in objData.Placements
                                  join objLookUp in objData.LookUps on objPlacement.PlacementType equals objLookUp.LookupId
                                  join objLkUp in objData.LookUps on objPlacement.Department equals objLkUp.LookupId
                                  where (objPlacement.StudentPersonalId == StudentId && objPlacement.Status == 1 && objPlacement.Location == currLocation && objPlacement.PlacementId != placementId)
                                  select new GridListPlacement
                                  {
                                      PlacementId = objPlacement.PlacementId,
                                      PlacementName = objLookUp.LookupName,
                                      Program = objLkUp.LookupName,
                                      StartDate = objPlacement.StartDate,
                                      EndDate = objPlacement.EndDate,


                                  }).ToList();
                }
                catch
                {

                }

                if (retunmodel.Count > 0)
                {
                    return 0;
                }
                else
                {
                    StdtClass stdtc = new StdtClass();
                    var result = objData.StdtClasses.Where(x => x.StdtId == StudentId && x.ClassId == currLocation && x.ActiveInd == "A").ToList();

                    if (result.Count > 0)
                    {
                        result[0].ActiveInd = "D";
                        objData.SaveChanges();
                    }
                    AssignToClass(StudentId, ClassId);

                    return 1;
                }
            }
            return 0;
        }
        public int DeleteFromClass(int StudentId, int ClassId, int placementId)
        {
            BiWeeklyRCPNewEntities objData = new BiWeeklyRCPNewEntities();
            IList<ContactNameSearchDetails> val = null;
            IList<GridListPlacement> retunmodel = new List<GridListPlacement>();
            session = (clsSession)HttpContext.Current.Session["UserSessionClient"];

            var currLocationId = objData.Placements.Where(x => x.PlacementId == placementId).ToList();

            int? currLocation = currLocationId[0].Location;
            // if (currLocation != ClassId)
            //{
            try
            {
                retunmodel = (from objPlacement in objData.Placements
                              join objLookUp in objData.LookUps on objPlacement.PlacementType equals objLookUp.LookupId
                              join objLkUp in objData.LookUps on objPlacement.Department equals objLkUp.LookupId
                              where (objPlacement.StudentPersonalId == StudentId && objPlacement.Status == 1 && objPlacement.Location == ClassId && objPlacement.PlacementId != placementId)
                              select new GridListPlacement
                              {
                                  PlacementId = objPlacement.PlacementId,
                                  PlacementName = objLookUp.LookupName,
                                  Program = objLkUp.LookupName,
                                  StartDate = objPlacement.StartDate,
                                  EndDate = objPlacement.EndDate,


                              }).ToList();
            }
            catch
            {

            }

            if (retunmodel.Count > 0)
            {
                return 0;
            }
            else
            {
                StdtClass stdtc = new StdtClass();
                var result = objData.StdtClasses.Where(x => x.StdtId == StudentId && x.ClassId == currLocation && x.ActiveInd == "A").ToList();

                if (result.Count > 0)
                {
                    result[0].ActiveInd = "D";
                    objData.SaveChanges();
                }
                //AssignToClass(StudentId, ClassId);

                return 1;
            }
            //}
            return 0;
        }

        public int AssignToClass(int StudentId, int ClassId)
        {
            BiWeeklyRCPNewEntities objData = new BiWeeklyRCPNewEntities();
            IList<ContactNameSearchDetails> val = null;
            IList<GridListPlacement> retunmodel = new List<GridListPlacement>();
            session = (clsSession)HttpContext.Current.Session["UserSessionClient"];

            var result = objData.StdtClasses.Where(x => x.StdtId == StudentId && x.ClassId == ClassId && x.ActiveInd == "A").ToList();

            if (result.Count() > 0)
            {
                return 0;
            }
            else
            {
                StdtClass stdc = new StdtClass();

                try
                {
                    stdc.StdtId = StudentId;
                    stdc.ClassId = ClassId;
                    stdc.ActiveInd = "A";
                    stdc.PrimaryInd = "A";
                    stdc.SchoolId = session.SchoolId;
                    stdc.CreatedBy = session.LoginId.ToString();
                    stdc.CreatedOn = DateTime.Now;

                    objData.StdtClasses.Add(stdc);
                    objData.SaveChanges();
                }
                catch
                {

                }

                return 1;
            }
        }


        public string saveCRMonthlyData(ProgressReport model, int CId)
        {
            sess = (clsSession)HttpContext.Current.Session["UserSessionClient"];
            dbobj = new BiWeeklyRCPNewEntities();
            int ClientID = sess.StudentId, SchoolId = sess.SchoolId;

            ProgressRpt progress;

            try
            {
                if (CId == 0)
                {
                    progress = new ProgressRpt();

                    progress.SchoolId = sess.SchoolId;
                    progress.StudentId = sess.StudentId;

                    progress.CRM_Academic = model.CRM_Academic;
                    progress.CRM_Clinical = model.CRM_Clinical;
                    progress.CRM_Outings = model.CRM_Outings;
                    progress.CRM_Other = model.CRM_Other;

                    progress.CreatedBy = sess.LoginId;
                    progress.CreatedOn = DateTime.Now;
                    dbobj.ProgressRpts.Add(progress);
                    dbobj.SaveChanges();
                }
                else
                {
                    progress = dbobj.ProgressRpts.Where(m => m.ProgressId == CId).SingleOrDefault();

                    progress.CRM_Academic = model.CRM_Academic;
                    progress.CRM_Clinical = model.CRM_Clinical;
                    progress.CRM_Outings = model.CRM_Outings;
                    progress.CRM_Other = model.CRM_Other;

                    progress.ModifiedBy = sess.LoginId;
                    progress.ModifiedOn = DateTime.Now;
                    dbobj.SaveChanges();

                }
                return "Success";
            }
            catch
            {
                return "Failed";
            }
        }

        public string SaveRTFMonthlyData(ProgressReport model, int MId)
        {
            sess = (clsSession)HttpContext.Current.Session["UserSessionClient"];
            dbobj = new BiWeeklyRCPNewEntities();
            int ClientID = sess.StudentId, SchoolId = sess.SchoolId;

            //ProgressRpt progress;
            //ProgressRpt_List progressList;

            ProgressRpt_RTF_M objRTFM;

            //int proId = 0;
            //var proIdList = (from x in dbobj.ProgressRpt_List
            //                 where x.SchoolId == sess.SchoolId && x.StudentId == sess.StudentId
            //                 select x.ProReportId).ToList();
            //if (proIdList.Count > 0)
            //{
            //    proId = proIdList[0];
            //}

            try
            {
                if (MId == 0)
                {
                    objRTFM = new ProgressRpt_RTF_M();

                    objRTFM.SchoolId = sess.SchoolId;
                    objRTFM.StudentId = sess.StudentId;

                    objRTFM.RTF_M_BLStart = model.RTF_M_BLStart;
                    objRTFM.RTF_M_BLEnd = model.RTF_M_BLEnd;
                    objRTFM.RTF_M_RptDate = model.RTF_M_RptDate;
                    objRTFM.RTF_M_BgInfo = model.RTF_M_BgInfo;
                    objRTFM.RTF_M_BSPlan = model.RTF_M_BSPlan;
                    objRTFM.RTF_M_Assessments = model.RTF_M_Assessments;
                    objRTFM.RTF_M_CIntegration = model.RTF_M_CIntegration;
                    objRTFM.RTF_M_CMedication = model.RTF_M_CMedication;
                    objRTFM.RTF_M_DPlanning = model.RTF_M_DPlanning;
                    objRTFM.RTF_M_ADSite = model.RTF_M_ADSite;
                    objRTFM.RTF_M_ADStay = model.RTF_M_ADStay;

                    objRTFM.CreatedBy = sess.LoginId;
                    objRTFM.CreatedOn = DateTime.Now;
                    dbobj.ProgressRpt_RTF_M.Add(objRTFM);
                    dbobj.SaveChanges();
                }
                else
                {
                    objRTFM = dbobj.ProgressRpt_RTF_M.Where(m => m.RTFMId == MId).SingleOrDefault();

                    objRTFM.RTF_M_BLStart = model.RTF_M_BLStart;
                    objRTFM.RTF_M_BLEnd = model.RTF_M_BLEnd;
                    objRTFM.RTF_M_RptDate = model.RTF_M_RptDate;
                    objRTFM.RTF_M_BgInfo = model.RTF_M_BgInfo;
                    objRTFM.RTF_M_BSPlan = model.RTF_M_BSPlan;
                    objRTFM.RTF_M_Assessments = model.RTF_M_Assessments;
                    objRTFM.RTF_M_CIntegration = model.RTF_M_CIntegration;
                    objRTFM.RTF_M_CMedication = model.RTF_M_CMedication;
                    objRTFM.RTF_M_DPlanning = model.RTF_M_DPlanning;
                    objRTFM.RTF_M_ADSite = model.RTF_M_ADSite;
                    objRTFM.RTF_M_ADStay = model.RTF_M_ADStay;

                    objRTFM.ModifiedBy = sess.LoginId;
                    objRTFM.ModifiedOn = DateTime.Now;
                    dbobj.SaveChanges();
                }
                return "Success";
            }
            catch
            {
                return "Failed";
            }
        }

        public string SaveRTFQuarterlyData(ProgressReport model, int QId)
        {
            sess = (clsSession)HttpContext.Current.Session["UserSessionClient"];
            dbobj = new BiWeeklyRCPNewEntities();
            int ClientID = sess.StudentId, SchoolId = sess.SchoolId;



            //ProgressReport model;
            ProgressRpt_RTF_Q objRTFQ;
            //ProgressRpt progress;
            //ProgressRpt_List progressList;

            //int proId = 0;
            //var proIdList = (from x in dbobj.ProgressRpt_List
            //                 where x.SchoolId == sess.SchoolId && x.StudentId == sess.StudentId
            //                 select x.ProReportId).ToList();
            //if (proIdList.Count > 0)
            //{
            //    proId = proIdList[0];
            //}



            try
            {
                if (QId == 0)
                {
                    objRTFQ = new ProgressRpt_RTF_Q();

                    objRTFQ.SchoolId = sess.SchoolId;
                    objRTFQ.StudentId = sess.StudentId;

                    objRTFQ.RTF_Q_BLStart = model.RTF_Q_BLStart;
                    objRTFQ.RTF_Q_BLEnd = model.RTF_Q_BLEnd;
                    objRTFQ.RTF_Q_RptDate = model.RTF_Q_RptDate;
                    objRTFQ.RTF_Q_TBehavior = model.RTF_Q_TBehavior;
                    objRTFQ.RTF_Q_Outlines = model.RTF_Q_Outlines;

                    objRTFQ.CreatedBy = sess.LoginId;
                    objRTFQ.CreatedOn = DateTime.Now;
                    dbobj.ProgressRpt_RTF_Q.Add(objRTFQ);
                    dbobj.SaveChanges();
                }
                else
                {
                    objRTFQ = dbobj.ProgressRpt_RTF_Q.Where(q => q.RTFQId == QId).SingleOrDefault();

                    objRTFQ.SchoolId = sess.SchoolId;
                    objRTFQ.StudentId = sess.StudentId;

                    objRTFQ.RTF_Q_BLStart = model.RTF_Q_BLStart;
                    objRTFQ.RTF_Q_BLEnd = model.RTF_Q_BLEnd;
                    objRTFQ.RTF_Q_RptDate = model.RTF_Q_RptDate;
                    objRTFQ.RTF_Q_TBehavior = model.RTF_Q_TBehavior;
                    objRTFQ.RTF_Q_Outlines = model.RTF_Q_Outlines;

                    objRTFQ.ModifiedBy = sess.LoginId;
                    objRTFQ.ModifiedOn = DateTime.Now;
                    dbobj.SaveChanges();
                }
                return "Success";
            }
            catch
            {
                return "Failed";
            }
        }

        //=== List 6 - Task #2
        public string SavePositionData(string posname)
        {
            sess = (clsSession)HttpContext.Current.Session["UserSessionClient"];
            string Result = "";
            dbobj = new BiWeeklyRCPNewEntities();
            LookUp lup = new LookUp();

            try
            {
                lup.SchoolId = sess.SchoolId;
                lup.LookupType = "StaffPositionLabel";
                lup.LookupName = posname;
                lup.LookupCode = posname;
                lup.LookupDesc = posname;
                lup.ActiveInd = "A";
                lup.CreatedBy = sess.LoginId.ToString();
                lup.CreateOn = DateTime.Now;
                lup.isDynamic = false;
                dbobj.LookUps.Add(lup);
                dbobj.SaveChanges();
                Result = "Success";
            }
            catch (Exception e)
            {
                Result = "Failed";
                e.ToString();
            }

            return Result;
        }

        public string DeletePositionData(int posid)
        {
            sess = (clsSession)HttpContext.Current.Session["UserSessionClient"];
            string Result = "";
            dbobj = new BiWeeklyRCPNewEntities();
            LookUp lup = new LookUp();
            try
            {
                var removepos = (from rpos in dbobj.LookUps
                                 where rpos.LookupType == "StaffPositionLabel" && rpos.LookupId == posid
                                 select rpos);
                if (removepos != null)
                {
                    foreach (var remobj in removepos)
                    {
                        remobj.ActiveInd = "D";
                        remobj.ModifiedBy = sess.LoginId.ToString();
                        remobj.ModifiedOn = DateTime.Now;
                    }
                    dbobj.SaveChanges();
                    Result = "Success";
                }
            }
            catch (Exception e)
            {
                Result = "Failed";
                e.ToString();
            }

            return Result;
        }
        //=== List 6 - Task #2
    }

    public class ActiveReferalNdUser
    {
        public string ReferralName;
        public string QueueName;
        public string UserName;
        public string ImageUrl;
        public string Gender;
        public string AssignDate;
        public string CheckListName;
        public string QueueId;
        public string ReferralId;
    }
    public class StudentSearchDetails
    {
        public virtual PagingModel pageModel { get; set; }
        public int ReferralId { get; set; }
        public string QueueId { get; set; }
        public string ReferralName { get; set; }
        public string Gender { get; set; }
        public string AdmissionDate { get; set; }
        public string BirthDate { get; set; }
        public string LastQueue { get; set; }
        public bool? FundingVerification { get; set; }
        public bool WaitingList { get; set; }
        public bool InactiveList { get; set; }
        public string ReferralName_short { get; set; }
    }

    public class StaffSearchDetails
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
    }

    public class ContactNameSearchDetails
    {

        public int ContactId { get; set; }
        public string ContactName { get; set; }
    }

    public class GridListPlacement
    {
        public virtual int PlacementId { get; set; }
        public virtual string PlacementName { get; set; }
        public virtual string Program { get; set; }
        public virtual string PlacementnStatus { get; set; }
        public DateTime? EndDate;
        public DateTime? StartDate;
        public virtual string datetime
        {
            get
            {
                if (EndDate != null)
                {
                    return ((DateTime)EndDate).ToString("MM/dd/yyyy");
                }
                else
                {
                    return "";
                }
            }

        }
        public virtual string startdatetime
        {
            get
            {
                if (StartDate != null)
                {
                    return ((DateTime)StartDate).ToString("MM/dd/yyyy");
                }
                else
                {
                    return "";
                }
            }

        }
    }


}


