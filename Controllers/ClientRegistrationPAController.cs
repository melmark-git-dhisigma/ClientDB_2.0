using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ClientDB.Models;
using ClientDB.DbModel;
using ClientDB.AppFunctions;
using ClientDB.ParentServiceReference;
using System.IO;

namespace ClientDB.Controllers
{
    [ValidateInput(false)]
    public class ClientRegistrationPAController : Controller
    {
        //
        // GET: /ClientRegistrationPA/
        ParentServiceClient parentservice = null;
        Other_Functions objFuns = new Other_Functions();
        public clsSession sess = null;
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult Index(int Param = 0)
        {
            BiWeeklyRCPNewEntities dbobj = new BiWeeklyRCPNewEntities();
            ClsErrorLog error = new ClsErrorLog();
            error.WriteToLog("Param Value  " + Param);
            sess = (clsSession)Session["UserSessionClient"];
            if (Param != 0)
            {
                sess.StudentId = Param;
            }
            if (sess.StudentId == 0)
            {
                sess.StudentId = Param;
                ViewBag.Param = Param;
            }
            else
            {
                ViewBag.Param = sess.StudentId;  
            }

            var StudStatus = dbobj.StudentPersonals.Where(x => x.StudentPersonalId == sess.StudentId && x.SchoolId == sess.SchoolId).SingleOrDefault();
            if (StudStatus != null)
            {
                Session["PlacementStat"] = (StudStatus.PlacementStatus == null) ? "A" : StudStatus.PlacementStatus;
            }
            error.WriteToLog("sess.StudentId   " + sess.StudentId);
            Session["TempStudentId"] = ViewBag.Param;
            ViewBag.Usename = sess.UserName;
            return View();
        }
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult ClientRegistrationPA(ClientRegistrationPAModel model = null, string data = null)
        {
            errorLog errlog = new errorLog();
            string templastname = "", temprace = "", tempcitizenship = "";
            //  errlog.WriteToLog("clientregistation");
            sess = (clsSession)Session["UserSessionClient"];
            // sess.ClientId = Param;
            ViewBag.curstatus = 1;
            ViewBag.Usename = sess.UserName;
            Session["PlacementStat"] = "A";
            if (data == null)
                data = "0|*";

            string[] Param = data.Split('|');
            model = new ClientRegistrationPAModel();
            Other_Functions objFuns = new Other_Functions();
            if (sess != null)
            {
                ViewBag.permission = (objFuns.setClientPermission("General Client") == 2) ? "true" : "false";
                if (Convert.ToInt32(Param[0]) > 0 || sess.StudentId > 0)
                {
                    model = objFuns.bindCliendDataPA(Convert.ToInt32(Session["TempStudentId"]));
                    if (model.PlacementStat != null)
                        Session["PlacementStat"] = model.PlacementStat;
                    else
                        Session["PlacementStat"] = "A";
                }
                else
                {
                    model.Diagnosis.Add(new Diagnosis
                    {
                        Name = null
                    });
                }
            }
            string country = objFuns.getCountryName();
            IList<SelectListItem> x = new List<SelectListItem>();
            x.Add(new SelectListItem { Text = "--Select--", Value = "0" });
            x.Add(new SelectListItem { Text = "Jr.", Value = "1" });
            x.Add(new SelectListItem { Text = "Sr.", Value = "2" });
            x.Add(new SelectListItem { Text = "I", Value = "3" });
            x.Add(new SelectListItem { Text = "II", Value = "4" });
            x.Add(new SelectListItem { Text = "III", Value = "5" });
            x.Add(new SelectListItem { Text = "IV", Value = "6" });
            x.Add(new SelectListItem { Text = "V", Value = "7" });
            x.Add(new SelectListItem { Text = "VI", Value = "8" });
            model.LastNameSuffixList = x;

            x = new List<SelectListItem>();
            x.Add(new SelectListItem { Text = "--Select--", Value = "0" });
            x.Add(new SelectListItem { Text = "Male", Value = "1" });
            x.Add(new SelectListItem { Text = "Female", Value = "2" });
            model.GenderList = x;

            x = new List<SelectListItem>();
            x.Add(new SelectListItem { Text = ""+country, Value = "1" });
            model.CountryList = x;

            x = new List<SelectListItem>();
            x.Add(new SelectListItem { Text = ""+country, Value = "1" });
            model.CountryOfBirthList = x;

            x = new List<SelectListItem>();
            x.Add(new SelectListItem { Text = "--Select--", Value = "0" });
            x.Add(new SelectListItem { Text = "Dual national", Value = "1014" });
            x.Add(new SelectListItem { Text = "Non-resident alien", Value = "1015" });
            x.Add(new SelectListItem { Text = "Resident alien", Value = "1016" });
            x.Add(new SelectListItem { Text = "United States Citizen", Value = "1017" });
            x.Add(new SelectListItem { Text = "Other", Value = "9999" });
            model.CitizenshipList = x;

            //model.CountryList = objFuns.getCountryList();
            // model.CountryOfBirthList = objFuns.getCountryList();

            model.StateOfBirthList = objFuns.getStateList();// model.StateOfBirthList = objFuns.getStateList(model.CountryofBirth);

            model.StateList = objFuns.getStateList();//model.StateList = objFuns.getStateList(model.Country);

            model.EmgContactList = ContactModel.GetEmegContactList(sess.StudentId);
            model.EmergencyContactList = objFuns.GetEmergencyContactList();

            model.SchoolState1 = objFuns.getStateList();
            model.SchoolState2 = objFuns.getStateList();
            model.SchoolState3 = objFuns.getStateList();
            //model.Classification = model.GetClassificationList(sess.SchoolId);

            if (sess.SchoolId == 1)
            {
                model.Classification = model.GetClassificationList(sess.SchoolId, "ClassificationNE"); // 25-jan-2021 - List #6 Task 1
                model.StaffPositionList = model.GetStaffPositionList(sess.SchoolId, "StaffPositionLabelNE");  // 1-Feb-2021 - List #6 Task 2
                model.StaffList = model.GetStaffsList(sess.SchoolId, "A");  // 1-Feb-2021 - List #6 Task 2
                model.PopulatePositionList = model.PopulatePositions(sess.SchoolId, "StaffPositionLabelNE"); // 2-Mar-2021 - List #6 Task 2
                model.PositionLabel1 = model.GetPositionLabel(sess.SchoolId, "StaffPositionLabelNE", 1);
                model.PositionLabel2 = model.GetPositionLabel(sess.SchoolId, "StaffPositionLabelNE", 2);
                model.PositionLabel3 = model.GetPositionLabel(sess.SchoolId, "StaffPositionLabelNE", 3);
                model.PositionLabel4 = model.GetPositionLabel(sess.SchoolId, "StaffPositionLabelNE", 4);
                model.PositionLabel5 = model.GetPositionLabel(sess.SchoolId, "StaffPositionLabelNE", 5);
                model.PositionLabel6 = model.GetPositionLabel(sess.SchoolId, "StaffPositionLabelNE", 6);
                model.PositionLabel7 = model.GetPositionLabel(sess.SchoolId, "StaffPositionLabelNE", 7);
                model.PositionLabel8 = model.GetPositionLabel(sess.SchoolId, "StaffPositionLabelNE", 8);
                model.PositionLabel9 = model.GetPositionLabel(sess.SchoolId, "StaffPositionLabelNE", 9);
                model.PositionLabel10 = model.GetPositionLabel(sess.SchoolId, "StaffPositionLabelNE", 10);

                model.StaffPosition1 = model.GetPositionLabelID(sess.SchoolId, "StaffPositionLabelNE", 1);
                model.StaffPosition2 = model.GetPositionLabelID(sess.SchoolId, "StaffPositionLabelNE", 2);
                model.StaffPosition3 = model.GetPositionLabelID(sess.SchoolId, "StaffPositionLabelNE", 3);
                model.StaffPosition4 = model.GetPositionLabelID(sess.SchoolId, "StaffPositionLabelNE", 4);
                model.StaffPosition5 = model.GetPositionLabelID(sess.SchoolId, "StaffPositionLabelNE", 5);
                model.StaffPosition6 = model.GetPositionLabelID(sess.SchoolId, "StaffPositionLabelNE", 6);
                model.StaffPosition7 = model.GetPositionLabelID(sess.SchoolId, "StaffPositionLabelNE", 7);
                model.StaffPosition8 = model.GetPositionLabelID(sess.SchoolId, "StaffPositionLabelNE", 8);
                model.StaffPosition9 = model.GetPositionLabelID(sess.SchoolId, "StaffPositionLabelNE", 9);
                model.StaffPosition10 = model.GetPositionLabelID(sess.SchoolId, "StaffPositionLabelNE", 10);
            }
            else
            {
                model.Classification = model.GetClassificationList(sess.SchoolId, "Classification");
                model.StaffPositionList = model.GetStaffPositionList(sess.SchoolId, "StaffPositionLabelPA");  // 1-Feb-2021 - List #6 Task 2
                model.StaffList = model.GetStaffsList(sess.SchoolId, "A");  // 1-Feb-2021 - List #6 Task 2
                model.PopulatePositionList = model.PopulatePositions(sess.SchoolId, "StaffPositionLabelPA"); // 2-Mar-2021 - List #6 Task 2
                model.PositionLabel1 = model.GetPositionLabel(sess.SchoolId, "StaffPositionLabelPA", 1);
                model.PositionLabel2 = model.GetPositionLabel(sess.SchoolId, "StaffPositionLabelPA", 2);
                model.PositionLabel3 = model.GetPositionLabel(sess.SchoolId, "StaffPositionLabelPA", 3);
                model.PositionLabel4 = model.GetPositionLabel(sess.SchoolId, "StaffPositionLabelPA", 4);
                model.PositionLabel5 = model.GetPositionLabel(sess.SchoolId, "StaffPositionLabelPA", 5);
                model.PositionLabel6 = model.GetPositionLabel(sess.SchoolId, "StaffPositionLabelPA", 6);
                model.PositionLabel7 = model.GetPositionLabel(sess.SchoolId, "StaffPositionLabelPA", 7);
                model.PositionLabel8 = model.GetPositionLabel(sess.SchoolId, "StaffPositionLabelPA", 8);
                model.PositionLabel9 = model.GetPositionLabel(sess.SchoolId, "StaffPositionLabelPA", 9);
                model.PositionLabel10 = model.GetPositionLabel(sess.SchoolId, "StaffPositionLabelPA", 10);

                model.StaffPosition1 = model.GetPositionLabelID(sess.SchoolId, "StaffPositionLabelPA", 1);
                model.StaffPosition2 = model.GetPositionLabelID(sess.SchoolId, "StaffPositionLabelPA", 2);
                model.StaffPosition3 = model.GetPositionLabelID(sess.SchoolId, "StaffPositionLabelPA", 3);
                model.StaffPosition4 = model.GetPositionLabelID(sess.SchoolId, "StaffPositionLabelPA", 4);
                model.StaffPosition5 = model.GetPositionLabelID(sess.SchoolId, "StaffPositionLabelPA", 5);
                model.StaffPosition6 = model.GetPositionLabelID(sess.SchoolId, "StaffPositionLabelPA", 6);
                model.StaffPosition7 = model.GetPositionLabelID(sess.SchoolId, "StaffPositionLabelPA", 7);
                model.StaffPosition8 = model.GetPositionLabelID(sess.SchoolId, "StaffPositionLabelPA", 8);
                model.StaffPosition9 = model.GetPositionLabelID(sess.SchoolId, "StaffPositionLabelPA", 9);
                model.StaffPosition10 = model.GetPositionLabelID(sess.SchoolId, "StaffPositionLabelPA", 10);
            }
            model.ApprovedVisitor = ContactModel.GetApprovedVisitor(sess.StudentId);

            //model.Classification1List = model.GetClassificationList(sess.SchoolId, 1);
            //model.Classification2List = model.GetClassificationList(sess.SchoolId, 2);
            //model.Classification3List = model.GetClassificationList(sess.SchoolId, 3);
            //model.Classification4List = model.GetClassificationList(sess.SchoolId, 4);
            //model.Classification5List = model.GetClassificationList(sess.SchoolId, 5);


            //x = new List<SelectListItem>();
            //x.Add(new SelectListItem { Text = "--Select--", Value = "0" });
            //x.Add(new SelectListItem { Text = "American Indian or Alaska Native", Value = "998" });
            //x.Add(new SelectListItem { Text = "Asian", Value = "999" });
            //x.Add(new SelectListItem { Text = "Black or African American", Value = "1000" });
            //x.Add(new SelectListItem { Text = "Native Hawaiian or Other Pacific Islander", Value = "1001" });
            //x.Add(new SelectListItem { Text = "White", Value = "1002" });
            model.RaceList = objFuns.getRaceList();


            x = new List<SelectListItem>();
            x.Add(new SelectListItem { Text = "--Select--", Value = "0" });
            x.Add(new SelectListItem { Text = "Dual national", Value = "1014" });
            x.Add(new SelectListItem { Text = "Non-resident alien", Value = "1015" });
            x.Add(new SelectListItem { Text = "Resident alien", Value = "1016" });
            x.Add(new SelectListItem { Text = "United States Citizen", Value = "1017" });
            x.Add(new SelectListItem { Text = "Other", Value = "9999" });
            model.CitizenshipList = x;


            if (Convert.ToInt32(Param[0]) > 0 && Param[1] == "Fill")
            {
                if (model.LastNameSuffix != null)
                {
                    templastname = model.LastNameSuffix.ToString();
                    if (templastname != "0")
                        model.LastNameSuffix = model.LastNameSuffixList.Where(objTempList => objTempList.Value == templastname).Select(objTempList => objTempList.Text).First();
                    else
                        model.LastNameSuffix = "";
                }
                else model.LastNameSuffix = "";
                if (model.Race != null)
                {
                    temprace = model.Race.ToString();
                    if (temprace != "0")
                        model.StrRace = model.RaceList.Where(objTempList => objTempList.Value == temprace).Select(objTempList => objTempList.Text).First();
                    else
                        model.StrRace = "";
                }
                else model.StrRace = "";
                if (model.Citizenship != null)
                {
                    tempcitizenship = model.Citizenship.ToString();
                    if (tempcitizenship != "0")
                        model.CitizenshipBirth = model.CitizenshipList.Where(objTempList => objTempList.Value == tempcitizenship).Select(objTempList => objTempList.Text).First();
                    else
                        model.CitizenshipBirth = "";
                }
                else model.CitizenshipBirth = "";
                HomeModel Hmodel = new HomeModel();

                //return RedirectToAction("GetParent", Hmodel);
                objFuns = new Other_Functions();
                sess = (clsSession)Session["UserSessionClient"];
                parentservice = new ParentServiceClient();
                try
                {
                    //
                    Hmodel.parentInfoPA = parentservice.GetParentDetailsPA(Convert.ToInt32(Session["TempStudentId"]));

                }
                catch (Exception ex)
                {
                    ViewBag.curstatus = 0;
                }
                try
                {

                    int id = (int)Hmodel.parentInfoPA.Country;
                    ViewBag.Country = objFuns.getCountry(id);
                    id = (int)Hmodel.parentInfoPA.State;
                    ViewBag.State = objFuns.getState(id);
                    id = (int)Hmodel.parentInfoPA.PrimaryCntct.State;
                    ViewBag.PrcontactState = objFuns.getState(id);
                }
                catch (Exception ex)
                {

                }

                return View("ClientRegistrationViewPa", Hmodel);
            }
            else
            {
                return View(model);
            }
            // getCountries();

        }

        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult QuickViewEmerg(int ContactId, string Fname1)
        {
            BiWeeklyRCPNewEntities dbobj = new BiWeeklyRCPNewEntities();
            ClientRegistrationPAModel model = new ClientRegistrationPAModel();
            sess = (clsSession)Session["UserSessionClient"];
            model.EmgContactIndividual = ContactModel.GetEmergContactIndividual(ContactId, sess.StudentId);
            var lukName = new ContactModel();
            lukName = (from objContactPersonal in dbobj.ContactPersonals
                       join objContactRelation in dbobj.StudentContactRelationships on objContactPersonal.ContactPersonalId equals objContactRelation.ContactPersonalId
                       join objLookUp in dbobj.LookUps on objContactRelation.RelationshipId equals objLookUp.LookupId
                       where (objContactPersonal.StudentPersonalId == sess.StudentId && objContactPersonal.Status == 1 && objContactRelation.ContactPersonalId == ContactId)
                       select new ContactModel
                       {
                           RelationName = objLookUp.LookupName,
                       }).FirstOrDefault();
            if (lukName != null)
            {
                model.RelationNameEmer = lukName.RelationName;
                
                ViewBag.Fname = Fname1;
            }

            int spId = Convert.ToInt32(dbobj.ContactPersonals.Where(objContactPersonal => objContactPersonal.StudentPersonalId == sess.StudentId && objContactPersonal.ContactPersonalId == ContactId &&
                objContactPersonal.Status == 1).Select(sp => sp.SpouseId).SingleOrDefault());


            if (spId != 0)
            {
                var data = (from cont in dbobj.ContactPersonals
                            join conrel in dbobj.StudentContactRelationships on cont.ContactPersonalId equals conrel.ContactPersonalId into ConrelNul
                            from ConrelFull in ConrelNul.DefaultIfEmpty()
                            join look in dbobj.LookUps on ConrelFull.RelationshipId equals look.LookupId into lookupnull
                            from lookupFull in lookupnull.DefaultIfEmpty()
                            where cont.StudentPersonalId == sess.StudentId && cont.Status == 1 && cont.ContactPersonalId == spId
                            select new
                            {
                                text = cont.LastName + ", " + cont.FirstName + " [" + lookupFull.LookupName + "]"
                            }).ToList();
                foreach (var item in data)
                {
                    model.SpouseName = item.text;
                }
            }


            var contactPersonalList = dbobj.ContactPersonals.Where(objContactPersonal => objContactPersonal.StudentPersonalId == sess.StudentId && objContactPersonal.ContactPersonalId==ContactId &&
                objContactPersonal.IsEmergency == true && objContactPersonal.Status == 1).ToList();
            if (contactPersonalList != null)
            {
                foreach (var item in contactPersonalList)
                {
                    model.IsCorrespondence = item.IsCorrespondence.GetBool();
                    model.IsGuardian = item.IsGuardian.GetBool();
                    model.Isincident = item.IsIncident.GetBool();
                    //model.IsEmergency = item.IsEmergency.GetBool();
                    model.IsEmergencyP = item.IsEmergencyP.GetBool();
                    model.IsEmergencyS = item.IsEmergencyS.GetBool();
                    model.IsEmergencyT = item.IsEmergencyT.GetBool();
                    model.IsEmergencyO = item.IsEmergencyO.GetBool();
                    model.IsBilling = item.IsBilling.GetBool();
                    model.IsCustody = item.IsCustody.GetBool();
                    model.IsNextOfKin = item.IsNextOfKin.GetBool();
                    model.IsApprovedVisitor = item.ApprovedVisitor.GetBool();
                    model.IsSchool = item.IsSchool.GetBool();
                    model.Note = item.Note;
                    model.IsOnCampusWithStaff = item.IsOnCampusWithStaff.GetBool();
                    model.IsOnCampusAlone = item.IsOnCampusAlone.GetBool();
                    model.IsOffCampus = item.IsOffCampus.GetBool();
                }
            }


            return View(model);
        }

        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult QuickViewApprove(int ContactId, string Fname1)
        {
            BiWeeklyRCPNewEntities dbobj = new BiWeeklyRCPNewEntities();
            ClientRegistrationPAModel model = new ClientRegistrationPAModel();
            sess = (clsSession)Session["UserSessionClient"];
            var lukName = new ContactModel();
            model.ApprovedVisitorIndi = ContactModel.GetApprovedVisitorIndividual(ContactId, sess.StudentId);
            lukName = (from objContactPersonal in dbobj.ContactPersonals
                       join objContactRelation in dbobj.StudentContactRelationships on objContactPersonal.ContactPersonalId equals objContactRelation.ContactPersonalId
                       join objLookUp in dbobj.LookUps on objContactRelation.RelationshipId equals objLookUp.LookupId
                       where (objContactPersonal.StudentPersonalId == sess.StudentId && objContactPersonal.Status == 1 && objContactRelation.ContactPersonalId == ContactId)
                       select new ContactModel
                       {
                           RelationName = objLookUp.LookupName
                       }).FirstOrDefault();
            if (lukName != null)
            {
                model.RelationNameVisit = lukName.RelationName;
                ViewBag.Fname = Fname1;
            }

            int spId = Convert.ToInt32(dbobj.ContactPersonals.Where(objContactPersonal => objContactPersonal.StudentPersonalId == sess.StudentId && objContactPersonal.ContactPersonalId == ContactId &&
                objContactPersonal.Status == 1).Select(sp => sp.SpouseId).SingleOrDefault());


            if (spId != 0)
            {
                var data = (from cont in dbobj.ContactPersonals
                            join conrel in dbobj.StudentContactRelationships on cont.ContactPersonalId equals conrel.ContactPersonalId into ConrelNul
                            from ConrelFull in ConrelNul.DefaultIfEmpty()
                            join look in dbobj.LookUps on ConrelFull.RelationshipId equals look.LookupId into lookupnull
                            from lookupFull in lookupnull.DefaultIfEmpty()
                            where cont.StudentPersonalId == sess.StudentId && cont.Status == 1 && cont.ContactPersonalId == spId
                            select new
                            {
                                text = cont.LastName + ", " + cont.FirstName + " [" + lookupFull.LookupName + "]"
                            }).ToList();
                foreach (var item in data)
                {
                    model.SpouseName = item.text;
                }
            }

            var contactPersonalList = dbobj.ContactPersonals.Where(objContactPersonal => objContactPersonal.StudentPersonalId == sess.StudentId && objContactPersonal.ContactPersonalId == ContactId &&
                (objContactPersonal.IsOnCampusWithStaff == true || objContactPersonal.IsOnCampusAlone == true || objContactPersonal.IsOffCampus == true) && objContactPersonal.Status == 1).ToList();
            if (contactPersonalList != null)
            {
                foreach (var item in contactPersonalList)
                {
                    model.IsCorrespondence = item.IsCorrespondence.GetBool();
                    model.IsGuardian = item.IsGuardian.GetBool();
                    model.Isincident = item.IsIncident.GetBool();
                    //model.IsEmergency = item.IsEmergency.GetBool();
                    model.IsEmergencyP = item.IsEmergencyP.GetBool();
                    model.IsEmergencyS = item.IsEmergencyS.GetBool();
                    model.IsEmergencyT = item.IsEmergencyT.GetBool();
                    model.IsEmergencyO = item.IsEmergencyO.GetBool();
                    model.IsBilling = item.IsBilling.GetBool();
                    model.IsCustody = item.IsCustody.GetBool();
                    model.IsNextOfKin = item.IsNextOfKin.GetBool();
                    model.IsApprovedVisitor = item.ApprovedVisitor.GetBool();
                    model.IsSchool = item.IsSchool.GetBool();
                    model.Note = item.Note;
                    model.IsOnCampusWithStaff = item.IsOnCampusWithStaff.GetBool();
                    model.IsOnCampusAlone = item.IsOnCampusAlone.GetBool();
                    model.IsOffCampus = item.IsOffCampus.GetBool();
                }
            }
            return View(model);
        }



        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult staticClientDetailsPA(ClientRegistrationPAModel model = null, string data = null)
        {
            errorLog errlog = new errorLog();
            string templastname = "", temprace = "", tempcitizenship = "";
            //  errlog.WriteToLog("clientregistation");
            sess = (clsSession)Session["UserSessionClient"];
            // sess.ClientId = Param;
            ViewBag.curstatus = 1;
            ViewBag.Usename = sess.UserName;
            if (data == null)
                data = "0|*";

            string[] Param = data.Split('|');
            model = new ClientRegistrationPAModel();
            Other_Functions objFuns = new Other_Functions();
            if (sess != null)
            {
                ViewBag.permission = (objFuns.setClientPermission("General Client") == 2) ? "true" : "false";
                if (Convert.ToInt32(Param[0]) > 0 || sess.StudentId > 0)
                {
                    model = objFuns.bindCliendDataPA(Convert.ToInt32(Session["TempStudentId"]));
                }
            }
            string country = objFuns.getCountryName();
            IList<SelectListItem> x = new List<SelectListItem>();
            x.Add(new SelectListItem { Text = "--Select--", Value = "0" });
            x.Add(new SelectListItem { Text = "Jr.", Value = "1" });
            x.Add(new SelectListItem { Text = "Sr.", Value = "2" });
            x.Add(new SelectListItem { Text = "I", Value = "3" });
            x.Add(new SelectListItem { Text = "II", Value = "4" });
            x.Add(new SelectListItem { Text = "III", Value = "5" });
            x.Add(new SelectListItem { Text = "IV", Value = "6" });
            x.Add(new SelectListItem { Text = "V", Value = "7" });
            x.Add(new SelectListItem { Text = "VI", Value = "8" });
            model.LastNameSuffixList = x;

            x = new List<SelectListItem>();
            x.Add(new SelectListItem { Text = "--Select--", Value = "0" });
            x.Add(new SelectListItem { Text = "Male", Value = "1" });
            x.Add(new SelectListItem { Text = "Female", Value = "2" });
            model.GenderList = x;

            x = new List<SelectListItem>();
            x.Add(new SelectListItem { Text = ""+country, Value = "1" });
            model.CountryList = x;

            x = new List<SelectListItem>();
            x.Add(new SelectListItem { Text = ""+country, Value = "1" });
            model.CountryOfBirthList = x;

            //model.CountryList = objFuns.getCountryList();
            // model.CountryOfBirthList = objFuns.getCountryList();

            model.StateOfBirthList = objFuns.getStateList();// model.StateOfBirthList = objFuns.getStateList(model.CountryofBirth);

            model.StateList = objFuns.getStateList();//model.StateList = objFuns.getStateList(model.Country);




            //x = new List<SelectListItem>();
            //x.Add(new SelectListItem { Text = "--Select--", Value = "0" });
            //x.Add(new SelectListItem { Text = "American Indian or Alaska Native", Value = "998" });
            //x.Add(new SelectListItem { Text = "Asian", Value = "999" });
            //x.Add(new SelectListItem { Text = "Black or African American", Value = "1000" });
            //x.Add(new SelectListItem { Text = "Native Hawaiian or Other Pacific Islander", Value = "1001" });
            //x.Add(new SelectListItem { Text = "White", Value = "1002" });
            model.RaceList = objFuns.getRaceList();


            x = new List<SelectListItem>();
            x.Add(new SelectListItem { Text = "--Select--", Value = "0" });
            x.Add(new SelectListItem { Text = "Dual national", Value = "1014" });
            x.Add(new SelectListItem { Text = "Non-resident alien", Value = "1015" });
            x.Add(new SelectListItem { Text = "Resident alien", Value = "1016" });
            x.Add(new SelectListItem { Text = "United States Citizen", Value = "1017" });
            x.Add(new SelectListItem { Text = "Other", Value = "9999" });
            model.CitizenshipList = x;


            if (Convert.ToInt32(Param[0]) > 0 && Param[1] == "Fill")
            {
                if (model.LastNameSuffix != null)
                {
                    templastname = model.LastNameSuffix.ToString();
                    if (templastname != "0")
                        model.LastNameSuffix = model.LastNameSuffixList.Where(objTempList => objTempList.Value == templastname).Select(objTempList => objTempList.Text).First();
                    else
                        model.LastNameSuffix = "";
                }
                else model.LastNameSuffix = "";
                if (model.Race != null)
                {
                    temprace = model.Race.ToString();
                    if (temprace != "0")
                        model.StrRace = model.RaceList.Where(objTempList => objTempList.Value == temprace).Select(objTempList => objTempList.Text).First();
                    else
                        model.StrRace = "";
                }
                else model.StrRace = "";
                if (model.Citizenship != null)
                {
                    tempcitizenship = model.Citizenship.ToString();
                    if (tempcitizenship != "0")
                        model.CitizenshipBirth = model.CitizenshipList.Where(objTempList => objTempList.Value == tempcitizenship).Select(objTempList => objTempList.Text).First();
                    else
                        model.CitizenshipBirth = "";
                }
                else model.CitizenshipBirth = "";
                HomeModel Hmodel = new HomeModel();

                //return RedirectToAction("GetParent", Hmodel);
                objFuns = new Other_Functions();
                sess = (clsSession)Session["UserSessionClient"];
                parentservice = new ParentServiceClient();
                try
                {
                    //
                    Hmodel.parentInfoPA = parentservice.GetParentDetailsPA(Convert.ToInt32(Session["TempStudentId"]));

                }
                catch (Exception ex)
                {
                    ViewBag.curstatus = 0;
                }
                try
                {

                    //int id = (int)Hmodel.parentInfoPA.Country;
                    //ViewBag.Country = objFuns.getCountry(id);
                    //id = (int)Hmodel.parentInfoPA.State;
                    //ViewBag.State = objFuns.getState(id);
                    //id = (int)Hmodel.parentInfoPA.PrimaryCntct.State;
                    //ViewBag.PrcontactState = objFuns.getState(id);
                }
                catch (Exception ex)
                {

                }
                model.ClientStatus = objFuns.DisplayStudStatus();
                return View(model);
                //return View("staticClientDetailsPA", Hmodel);
            }
            else
            {
                model.ClientStatus = objFuns.DisplayStudStatus();
                return View(model);
            }


        }

        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public string DisplayClientName()
        {
            sess = (clsSession)Session["UserSessionClient"];
            Other_Functions ObjOther = new Other_Functions();
            string Status = ObjOther.DisplayStudStatus();
            BiWeeklyRCPNewEntities dbobj = new BiWeeklyRCPNewEntities();
            var sname = dbobj.StudentPersonals.Where(x => x.StudentPersonalId == sess.StudentId).ToList();
            string Color = "";
            if (Status == "Active")
                Color = "<span style='color:green'> (" + Status + ")</span>";
            else if (Status == "On-Hold")
                Color = "<span style='color:orange'> (" + Status + ")</span>";
            else if (Status == "Inactive")
                Color = "<span style='color:red'> (" + Status + ")</span>";
            else if (Status == "Discharge")
                Color = "<span style='color:red'> (" + Status + ")</span>";
            string ClientName = sname[0].LastName + " , " + sname[0].FirstName + Color;
            return ClientName;
        }

        [HttpPost]
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public string SaveClients(ClientRegistrationPAModel model, HttpPostedFileBase profilePicture)
        {
            sess = (clsSession)Session["UserSessionClient"];
            string result = "";
            //result = objFuns.SaveData(model, profilePicture); //Commented pn 23-Jun-2020

            //Image Crop and Upload - Dev 2 [23-jun-2020] - Start
            string PathtoImg = ImageCompresor.getimgpath(profilePicture);
            HttpPostedFileBase objFile = null;
            if (PathtoImg != "")
            {
                byte[] bytes = System.IO.File.ReadAllBytes(PathtoImg);
                var contentTypeFile = "image/png";
                var fileName = "images.png";
                objFile = (HttpPostedFileBase)new MemoryPostedFile(new MemoryStream(bytes), contentTypeFile, fileName);
            }
            result = objFuns.SaveData(model, objFile);
            //Image Crop and Upload - Dev 2 [23-jun-2020] - End


            if (result == "Failed")
            {
                //  TempData["notice"] = "Failed To Insert Data";
                ViewBag.id = sess.StudentId;
                //return RedirectToAction("ClientRegistration");
                return result + "|" + sess.StudentId;
            }
            else
            {
                // TempData["notice"] = "Data Saved Sucessfully";
                ViewBag.id = sess.StudentId;
                return result + "|" + sess.StudentId;
                //string data = sess.ClientId + "|Fill";
                //return RedirectToAction("ClientRegistration",data);
            }


        }

        public ActionResult GetParent(HomeModel Hmodel)
        {
            objFuns = new Other_Functions();
            sess = (clsSession)Session["UserSessionClient"];
            parentservice = new ParentServiceClient();
            Hmodel.parentInfoPA = parentservice.GetParentDetailsPA(sess.StudentId);
            try
            {
                int id = (int)Hmodel.parentInfoPA.Country;
                ViewBag.Country = objFuns.getCountry(id);
                id = (int)Hmodel.parentInfoPA.State;
                ViewBag.State = objFuns.getState(id);
            }
            catch (Exception ex)
            {
            }

            return View("ClientRegistrationViewPa", Hmodel);
        }

        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public void SavePosition(string DocName)
        {
            try
            {
                string resultpos = "";
                if (DocName != "")
                {
                    resultpos = objFuns.SavePositionData(DocName);
                }
                //string tests = Request.UrlReferrer.ToString();
            }
            catch (Exception e)
            {
                e.ToString();
            }
        }

        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public void DeletePosition(int DocName)
        {
            try
            {
                string resultpos = "";
                if (DocName != null)
                {
                    resultpos = objFuns.DeletePositionData(DocName);
                }
                //string tests = Request.UrlReferrer.ToString();
            }
            catch (Exception e)
            {
                e.ToString();
            }
        }
        public String DischargeStudent()
        {

            sess = (clsSession)Session["UserSessionClient"];
            BiWeeklyRCPNewEntities dbobj = new BiWeeklyRCPNewEntities();

            int ClientID = sess.StudentId, SchoolId = sess.SchoolId;

            var currLocationId = dbobj.Placements.Where(x => x.StudentPersonalId == ClientID).ToList();

            int? currLocation = currLocationId[0].Location;

            
            StdtClass stdc = dbobj.StdtClasses.Where(x => x.StdtId == ClientID && x.ClassId == currLocation && x.ActiveInd == "A").SingleOrDefault();
            Class DischargeClass = dbobj.Classes.Where(x => x.ClassCd == "DSCH" && x.SchoolId == SchoolId && x.ActiveInd == "A").SingleOrDefault();
            var placements = dbobj.Placements.Where(x => x.StudentPersonalId == ClientID && x.Status == 1).ToList();
            var studentPersonals = dbobj.StudentPersonals.Where(x => x.StudentPersonalId == ClientID && x.SchoolId == SchoolId).SingleOrDefault();
                try
                {
                    if (placements != null)
                    {
                        foreach (var item in placements)
                        {
                            //item.Location = Convert.ToInt32(DischargeClass.ClassId);
                            item.PrevClassId = item.Location;
                            item.Location = Convert.ToInt32(DischargeClass.ClassId);
                            item.ModifiedBy = sess.LoginId;
                            item.ModifiedOn = DateTime.Now;
                            item.EndDate = DateTime.Now;
                            item.Discharge = 1;
                        }
                    }
                    if (studentPersonals != null)
                    {
                        studentPersonals.PlacementStatus = "D";
                    }
                    dbobj.SaveChanges();
                    return "Success";
                }
                catch
                {
                    return "Failed";
                }
        }
        public string ActivateStudent()
        {

            sess = (clsSession)Session["UserSessionClient"];
            BiWeeklyRCPNewEntities dbobj = new BiWeeklyRCPNewEntities();

            int ClientID = sess.StudentId, SchoolId = sess.SchoolId;

            Class DischargeClass = dbobj.Classes.Where(x => x.ClassCd == "DSCH" && x.SchoolId == SchoolId && x.ActiveInd == "A").SingleOrDefault();
            int DischargeClassId = Convert.ToInt32(DischargeClass.ClassId);
            StdtClass stdc = dbobj.StdtClasses.Where(x => x.StdtId == ClientID && x.ClassId == DischargeClassId && x.ActiveInd == "A").SingleOrDefault();
            var placements = dbobj.Placements.Where(x => x.StudentPersonalId == ClientID && x.Status == 1).ToList();
            var studentPersonals = dbobj.StudentPersonals.Where(x => x.StudentPersonalId == ClientID && x.SchoolId == SchoolId).SingleOrDefault();
            try
            {
                if (placements != null)
                {
                    foreach (var item in placements)
                    {
                        item.Location = item.PrevClassId;
                        item.ModifiedBy = sess.LoginId;
                        item.ModifiedOn = DateTime.Now;
                        item.Discharge = 0;
                    }
                }
                if (studentPersonals != null)
                {
                    if (placements != null)
                    {
                        if (placements.Count > 0)
                        {
                            studentPersonals.PlacementStatus = "A";
                        }
                        else
                            studentPersonals.PlacementStatus = "I";
                    }
                    else
                        studentPersonals.PlacementStatus = "I";
                }
                dbobj.SaveChanges();

                return "Success";
            }
            catch
            {
                return "Failed";
            }
        }
    }
}
