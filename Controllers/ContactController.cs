using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ClientDB.Models;
using ClientDB.AppFunctions;
using ClientDB.DbModel;

namespace ClientDB.Controllers
{
    [ValidateInput(false)]
    public class ContactController : Controller
    {
        //
        // GET: /Contact/
        Other_Functions objFuns = new Other_Functions();
        public clsSession sess = null;
        public static string SetLookUpCode = "USA";
        public ActionResult Index()
        {
            sess = (clsSession)Session["UserSessionClient"];
            //sess.StudentId = 1;
            //Session["UserSessionClient"] = sess;
            return RedirectToAction("Contact");


        }
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult Contact(ContactModel model = null)
        {
            // ContactModel ContactMod = new ContactModel();
            sess = (clsSession)Session["UserSessionClient"];
            IList<checkBoxViewModel> checkmode = new List<checkBoxViewModel>
            {
                new checkBoxViewModel(){name="Emergency",check=false},
                new checkBoxViewModel(){name="Incedent",check=false},
                new checkBoxViewModel(){name="Mail",check=false},
               
            };
            Other_Functions objFuns = new Other_Functions();
            string country = objFuns.getCountryName();

            IList<SelectListItem> x = new List<SelectListItem>();
            x.Add(new SelectListItem { Text = "--Select--", Value = "0" });
            x.Add(new SelectListItem { Text = "Mr.", Value = "1" });
            x.Add(new SelectListItem { Text = "Mrs.", Value = "2" });
            x.Add(new SelectListItem { Text = "Ms.", Value = "3" });
            x.Add(new SelectListItem { Text = "Dr.", Value = "4" });
            model.FirstNamePrefixList = x;

            x = new List<SelectListItem>();
            x.Add(new SelectListItem { Text = "--Select--", Value = "0" });
            x.Add(new SelectListItem { Text = "Jr.", Value = "1" });
            x.Add(new SelectListItem { Text = "Sr.", Value = "2" });
            x.Add(new SelectListItem { Text = "I", Value = "3" });
            x.Add(new SelectListItem { Text = "II", Value = "4" });
            x.Add(new SelectListItem { Text = "III", Value = "5" });
            x.Add(new SelectListItem { Text = "IV", Value = "6" });
            x.Add(new SelectListItem { Text = "V", Value = "7" });
            x.Add(new SelectListItem { Text = "VI", Value = "8" });
            if (sess != null)
            {
                ViewBag.permission = (objFuns.setClientPermission("Contact/Vendor") == 2) ? "true" : "false";

                model.LastNameSuffixList = x;
                model.SpouseList = model.GetSpouseContact(sess.StudentId);
                model.RelationList = objFuns.getRelationshipList();
                model.HomeAddressTypeList = objFuns.getAddressTypes();
                model.WorkAddressTypeList = objFuns.getAddressTypes();
                model.OtherAddressTypeList = objFuns.getAddressTypes();

                x = new List<SelectListItem>();
                x.Add(new SelectListItem { Text = ""+country, Value = "1" });
                model.HomeCountryList = x;

                x = new List<SelectListItem>();
                x.Add(new SelectListItem { Text = ""+country, Value = "1" });
                model.WorkCountryList = x;

                x = new List<SelectListItem>();
                x.Add(new SelectListItem { Text = ""+country, Value = "1" });
                model.OtherCountryList = x;

                model.HomeStateList = objFuns.getStateList();
                model.WorkStateList = objFuns.getStateList();
                model.OtherStateList = objFuns.getStateList();

                //model.HomeCountryList = objFuns.getCountryList();
               //model.WorkCountryList = objFuns.getCountryList();
                //model.OtherCountryList = objFuns.getCountryList();
               // model.HomeStateList = objFuns.getStateList(model.HomeCountry);
                //model.WorkStateList = objFuns.getStateList(model.WorkCountry);
                //model.OtherStateList = objFuns.getStateList(model.OtherCountry);
                model.checkbox = checkmode;
            }
            return View(model);
        }
        [ValidateInput(false)]
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult SaveContacts(ContactModel model)
        {
            string result = "";
            result = objFuns.SaveContactData(model);
            if (result == "No Client Selected")
            {
                TempData["notice"] = "No Client Selected";
            }
            return RedirectToAction("ListContactVendor");
        }

        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult QuickView(int id)
        {
            
            
            ContactModel model;
            Other_Functions objFuns = new Other_Functions();
            BiWeeklyRCPNewEntities dbobj = new BiWeeklyRCPNewEntities();
            IList<GridList> retunmodel = new List<GridList>();
            sess = (clsSession)Session["UserSessionClient"];
            var lukName=new ContactModel();
            model = objFuns.bindContactData(sess.StudentId, id);
            lukName = (from objContactPersonal in dbobj.ContactPersonals
                       join objContactRelation in dbobj.StudentContactRelationships on objContactPersonal.ContactPersonalId equals objContactRelation.ContactPersonalId
                       join objLookUp in dbobj.LookUps on objContactRelation.RelationshipId equals objLookUp.LookupId
                       where (objContactPersonal.StudentPersonalId == sess.StudentId && objContactPersonal.Status == 1 && objContactRelation.ContactPersonalId == id)
                       select new ContactModel
                       {
                           RelationName = objLookUp.LookupName
                           
                       }).FirstOrDefault();
            if (lukName != null)
            {
                model.RelationName = lukName.RelationName;
            }
            int spId = Convert.ToInt32( dbobj.ContactPersonals.Where(objContactPersonal => objContactPersonal.StudentPersonalId == sess.StudentId && objContactPersonal.ContactPersonalId == id &&
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

            var contactPersonalList = dbobj.ContactPersonals.Where(objContactPersonal => objContactPersonal.StudentPersonalId == sess.StudentId && objContactPersonal.ContactPersonalId ==id &&
                objContactPersonal.Status == 1).ToList();

            if (contactPersonalList != null)
            {
                foreach (var item in contactPersonalList)
                {
                    model.IsCorrespondence = item.IsCorrespondence.GetBool();
                    model.IsGuardian = item.IsGuardian.GetBool();
                    model.IsIncident = item.IsIncident.GetBool();
                    //model.IsEmergency = item.IsEmergency.GetBool();
                    model.IsEmergencyP = item.IsEmergencyP.GetBool();
                    model.IsEmergencyS = item.IsEmergencyS.GetBool();
                    model.IsEmergencyT = item.IsEmergencyT.GetBool();
                    model.IsEmergencyO = item.IsEmergencyO.GetBool();
                    model.IsBilling = item.IsBilling.GetBool();
                    model.IsCustody = item.IsCustody.GetBool();
                    model.IsSchool = item.IsSchool.GetBool();
                    model.IsNextOfKin = item.IsNextOfKin.GetBool();
                    model.ApprovedVisitor = item.ApprovedVisitor.GetBool();
                    model.Note = item.Note;
                    model.IsOnCampusWithStaff = item.IsOnCampusWithStaff.GetBool();
                    model.IsOnCampusAlone = item.IsOnCampusAlone.GetBool();
                    model.IsOffCampus = item.IsOffCampus.GetBool();
                }
            }
            
            return View(model);
        }


        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult fillContactDetails(int id)
        {
            //int ClientId = 1;
            ContactModel model;
            Other_Functions objFuns = new Other_Functions();
            BiWeeklyRCPNewEntities dbobj = new BiWeeklyRCPNewEntities();

            sess = (clsSession)Session["UserSessionClient"];
            if (sess != null)
            {
                ViewBag.permission = (objFuns.setClientPermission("Contact/Vendor") == 2) ? "true" : "false";
            }
            //sess.ClientId = ClientId;
           
            model = objFuns.bindContactData(sess.StudentId, id);



            IList<checkBoxViewModel> checkmode = new List<checkBoxViewModel>
            {
                new checkBoxViewModel(){name="Emergency",check=false},
                new checkBoxViewModel(){name="Incedent",check=false},
                new checkBoxViewModel(){name="Mail",check=false},
               
            };

            //IList<SelectListItem> x = new List<SelectListItem>();
            //x.Add(new SelectListItem { Text = "--Select--", Value = "0" });
            //x.Add(new SelectListItem { Text = "Mr.", Value = "1" });
            //x.Add(new SelectListItem { Text = "Mrs.", Value = "2" });
            //x.Add(new SelectListItem { Text = "Ms.", Value = "3" });
            //x.Add(new SelectListItem { Text = "Dr.", Value = "4" });
            //model.FirstNamePrefixList = x;
            //x = new List<SelectListItem>();
            //x.Add(new SelectListItem { Text = "--Select--", Value = "0" });
            //x.Add(new SelectListItem { Text = "Jr.", Value = "1" });
            //x.Add(new SelectListItem { Text = "Sr.", Value = "2" });
            //model.LastNameSuffixList = x;
            model.FirstNamePrefixList = Helpers.GetFirstNamePrefix;
            model.LastNameSuffixList = Helpers.GetLastNameSuffix;

            model.SpouseList = model.GetSpouseContact(sess.StudentId);
            model.RelationList = objFuns.getRelationshipList();
            model.HomeAddressTypeList = objFuns.getAddressTypes();
            model.WorkAddressTypeList = objFuns.getAddressTypes();
            model.OtherAddressTypeList = objFuns.getAddressTypes();
            model.HomeCountryList = objFuns.getCountryList();
            model.WorkCountryList = objFuns.getCountryList();
            model.OtherCountryList = objFuns.getCountryList();
            if (model.WorkCountry == 0 || model.OtherCountry == 0 || model.HomeCountry==0)
            {
                int countryId = dbobj.LookUps.Where(objLookup => objLookup.LookupType == "Country" && objLookup.LookupCode == SetLookUpCode).Select(objLookup => objLookup.LookupId).Single();
                model.WorkCountry = countryId;
                model.OtherCountry = countryId;
                model.HomeCountry = countryId;
            }

            model.HomeStateList = objFuns.getStateList(model.HomeCountry);
            model.WorkStateList = objFuns.getStateList(model.WorkCountry);
            model.OtherStateList = objFuns.getStateList(model.OtherCountry);
            model.checkbox = checkmode;

            return View("Contact", model);

        }
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult DeleteContactDetails(int id)
        {
            // int ClientId = 1;
            sess = (clsSession)Session["UserSessionClient"];
            //sess.ClientId = ClientId;
            objFuns.deleteContact(sess.StudentId, id);
            return RedirectToAction("ListContactVendor");
        }


        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult ImageUploadPanel(string edit = null)
        {
            ClsErrorLog error = new ClsErrorLog();
            sess = (clsSession)Session["UserSessionClient"];
            string temp = "";
            ViewBag.imageUrl = "";
            ViewBag.StudentId = "";
            ViewBag.StudentName = "";
            ViewBag.ModelId = sess.StudentId;
            ViewBag.SchoolId = sess.SchoolId;


            Other_Functions of = new Other_Functions();
            ViewBag.permission = (of.setClientPermission("Contact/Vendor") == 2) ? "true" : "false";


            ImageModel imgModel = new ImageModel();
            if (sess != null)
            {
                imgModel = objFuns.bindImage(Convert.ToInt32(Session["TempStudentId"]));
                if ((imgModel.ImageUrl != null) && (imgModel.StudentId != null))
                {
                    ViewBag.imageUrl = imgModel.ImageUrl;
                    ViewBag.StudentId = "Client Id : " + imgModel.StudentId;
                    ViewBag.PhotoDate = imgModel.PhotoDate;

                    temp = imgModel.LastName + " " + imgModel.Suffix + "," + imgModel.FirstName;
                    int length = temp.Length;
                    ViewBag.StudentName = temp;
                    if (temp.Length > 18)
                    {
                        //temp = temp.Remove(18, length - 18);
                        ViewBag.StudentName = temp;
                    }
                }
            }
            else
            {
                error.WriteToLog(" Session Not Found..  ");
            }
            if (edit == "0")
            {
                ViewBag.editButton = "none";
            }
            else
            {
                ViewBag.editButton = "block";
            }
            return View();
        }

        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult ListContactVendor(int page = 1, int pageSize = 15)
        {
            sess = (clsSession)Session["UserSessionClient"];
            ContactSearch search = new ContactSearch();
            ListContactModel bindModel = new ListContactModel();


            ClientDB.AppFunctions.Other_Functions OF = new ClientDB.AppFunctions.Other_Functions();
            ViewBag.permission = (OF.setClientPermission("Contact/Vendor") == 2) ? "true" : "false";

            //search.SearchArgument = argument;
            //search.SortStatus = bSort;
            //search.PagingArgument = Data;
            //ViewBag.curval = "";
            //ViewBag.flage = "";
            //ViewBag.SearchArg = "";
            //ViewBag.itemCount = 0;
            //if ((bSort == false) && (argument != "*"))
            //{
            //    ViewBag.SearchArg = argument;
            //}

            //ViewBag.SortArg = argument;
            Other_Functions objFuns = new Other_Functions();
            if (sess != null)
            {
                ViewBag.permission = (objFuns.setClientPermission("Contact/Vendor") == 2) ? "true" : "false";
                bindModel = ListContactModel.fillContacts(page, pageSize);
            }
            return View(bindModel);
        }
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult UpdateContactDetails(ContactModel model)
        {

            string result = "";
            // result = objFuns.UpdateData(model);
            return Content(result);
        }

    }
}
