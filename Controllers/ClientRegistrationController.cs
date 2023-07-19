using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ClientDB.Models;
using ClientDB.DbModel;
using ClientDB.AppFunctions;
using System.IO;
using Facesheet;

namespace ClientDB.Controllers
{
    public class ClientRegistrationController : Controller
    {
        //
        // GET: /ClientRegistration/
        Other_Functions objFuns = new Other_Functions();
        public clsSession sess = null;
        FacesheetClass objFacesheet = null;
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult Index(int Param = 0)
        {
            //clsSessionCon obj = new clsSessionCon();
            //sess = (ClsSession)obj.getSessionObject();
            //if (sess == null)
            //{
            //    Response.Redirect("Error.aspx?Error=Your session has expired. Please log-in again");
            //}
            ClsErrorLog error = new ClsErrorLog();
            error.WriteToLog("Param Value  " + Param);
            sess = (clsSession)Session["UserSessionClient"];
            if (sess.StudentId == 0)
            {
                sess.StudentId = Param;
                ViewBag.Param = Param;
            }
            else
            {
                ViewBag.Param = sess.StudentId;
            }
            error.WriteToLog("sess.StudentId   " + sess.StudentId);
            Session["TempStudentId"] = ViewBag.Param;


            //ViewBag.Param = Param;
            ViewBag.Usename = sess.UserName;

            return View();
        }

        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public string GetTitleReport()
        {
            string title = System.Configuration.ConfigurationManager.AppSettings["Server"].ToString();
            if (title == "PA")
                title = "Melmark Pennsylvania";
            else
                title = "Melmark New England";
            return title;

        }

        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult ClientRegistration(RegistrationModel model = null, string data = null)
        {
            errorLog errlog = new errorLog();
            string templastname = "", temprace = "", tempcitizenship = "";
            //  errlog.WriteToLog("clientregistation");
            sess = (clsSession)Session["UserSessionClient"];
            // sess.ClientId = Param;
            ViewBag.Usename = sess.UserName;
            if (data == null)
                data = "0|*";

            string[] Param = data.Split('|');
            model = new RegistrationModel();
            Other_Functions objFuns = new Other_Functions();
            if (sess != null)
            {
                ViewBag.permission = (objFuns.setClientPermission("General Client") == 2) ? "true" : "false";
                if (Convert.ToInt32(Param[0]) > 0 || sess.StudentId > 0)
                {
                    model = objFuns.bindCliendData(Convert.ToInt32(Session["TempStudentId"]));
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
            model.CountryOfBirthList = x;

            x = new List<SelectListItem>();
            x.Add(new SelectListItem { Text = ""+country, Value = "1" });
            model.CountryList = x;

            //model.CountryList = objFuns.getCountryList();
            //model.CountryOfBirthList = objFuns.getCountryList();

            model.StateOfBirthList = objFuns.getStateList();//objFuns.getStateList(model.CountryofBirth);

            model.StateList = objFuns.getStateList();//objFuns.getStateList(model.Country);
            model.SchoolState1 = objFuns.getStateList();
            model.SchoolState2 = objFuns.getStateList();
            model.SchoolState3 = objFuns.getStateList();




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
            model.EmergencyContactList = objFuns.GetEmergencyContactList();

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
                temprace = model.Race.ToString();
                if (temprace != "0")
                    try
                    {
                        model.StrRace = model.RaceList.Where(objTempList => objTempList.Value == temprace).Select(objTempList => objTempList.Text).First();
                    }
                    catch { }
                else
                    model.StrRace = "";
                tempcitizenship = model.Citizenship.ToString();
                if (tempcitizenship != "0")
                    try
                    {
                        model.CitizenshipBirth = model.CitizenshipList.Where(objTempList => objTempList.Value == tempcitizenship).Select(objTempList => objTempList.Text).First();
                    }
                    catch { }
                else
                    model.CitizenshipBirth = "";
                return View("ClientRegistrationView", model);

            }
            else
            {

                return View(model);
            }
            // getCountries();

        }

        [HttpPost]
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public string SaveClients(RegistrationModel model, HttpPostedFileBase profilePicture)
        {
            sess = (clsSession)Session["UserSessionClient"];
            string result = "";

            //if (profilePicture != null) //already commented

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
                //TempData["notice"] = "Failed To Insert Data";
                ViewBag.id = sess.StudentId;
                //return RedirectToAction("ClientRegistration");
                return result + "|" + sess.StudentId;
            }
            else
            {
                ViewBag.id = sess.StudentId;
                return result + "|" + sess.StudentId;
                //string data = sess.ClientId + "|Fill";
                //return RedirectToAction("ClientRegistration",data);
            }


        }

        public ActionResult getCountries()
        {

            objFuns = new Other_Functions();
            var Areas = objFuns.getCountryList();
            return Json(Areas, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getStates(int? countryid)
        {

            objFuns = new Other_Functions();
            var Areas = objFuns.getStateList(countryid);
            return Json(Areas, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ImageUploadPanel(ImageUploader model = null)
        {
            Other_Functions of = new Other_Functions();
            ViewBag.permission = (of.setClientPermission("General Client") == 2) ? "true" : "false";

            return View(model);
        }

        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        [HttpPost]
        public ActionResult saveUploadDocument(HttpPostedFileBase file, ImageUploader model)
        {
            string result = "";
            result = objFuns.SaveImage(file, model);
            return Content(result);
        }

        public ActionResult fillCLientDetails(RegistrationModel model = null)
        {
            // int ClientId = 1;

            sess = (clsSession)Session["UserSessionClient"];
            // sess.ClientId = ClientId;
            StudentPersonal student = new StudentPersonal();
            Other_Functions objFuns = new Other_Functions();
            if (sess != null)
            {
                ViewBag.permission = (objFuns.setClientPermission("General Client") == 2) ? "true" : "false";
                model = objFuns.bindCliendData(sess.StudentId);
            }
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

            model.CountryList = objFuns.getCountryList();
            model.CountryOfBirthList = objFuns.getCountryList();

            model.StateOfBirthList = objFuns.getStateList(model.CountryofBirth);

            model.StateList = objFuns.getStateList(model.Country);


            model.SchoolState1 = objFuns.getStateList();
            model.SchoolState2 = objFuns.getStateList();
            model.SchoolState3 = objFuns.getStateList();


            //x = new List<SelectListItem>();
            //x.Add(new SelectListItem { Text = "--Select--", Value = "0" });
            //x.Add(new SelectListItem { Text = "American Indian or Alaska Native", Value = "0998" });
            //x.Add(new SelectListItem { Text = "Asian", Value = "0999" });
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


            return View("ClientRegistration", model);
        }
        public ActionResult ExportAllData(Progress model)
        {
          
            sess = (clsSession)Session["UserSessionClient"];
            string[] args = new string[10];
            
            String connection = System.Configuration.ConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString;  
            Other_Functions otherFun = new Other_Functions();
            RegistrationModel regModl = new RegistrationModel();
            regModl = otherFun.bindCliendData(sess.StudentId);
            string FileLocation = "";           
            FileLocation = Server.MapPath("~\\Administration\\FacesheetNE");
            string studentName = "";
            //if (regModl.MiddleName != null)
            //    studentName = regModl.LastName + "," + regModl.FirstName + "," + regModl.MiddleName;
            //else
                studentName = regModl.LastName + "," + regModl.FirstName;
            string modifiedDate = regModl.ModifiedDate;
            string UpdatedOn = regModl.UpdatedOn;
            string dateOfBirth = regModl.DateOfBirth;
            args[0] = "NE";
            args[1] = connection;
            args[2] = FileLocation;
            args[3] = sess.StudentId.ToString();
            args[4] = sess.SchoolId.ToString();
            args[5] = sess.LoginId.ToString();
            args[6] = studentName;
            args[7] = modifiedDate;
            args[8] = UpdatedOn;
            args[9] = dateOfBirth;
            objFacesheet = new FacesheetClass();
            string path = FileLocation + "\\Temp";
            //liju Changed function
            //string HtmlData = objFacesheet.Main(args);

            string newPath = objFacesheet.ProcessTemplateFile(args);
            //string newPath = "";
            ////liju
            //HtmlData = HtmlData.Replace("’", "'");
            //HtmlData = HtmlData.Replace("…", "...");
            //HtmlData = HtmlData.Replace("‘", "'");
            //HtmlData = HtmlData.Replace("·", "- ");
            //HtmlData = HtmlData.Replace("“", "'");
            //HtmlData = HtmlData.Replace("”", "'");
            //HtmlData = HtmlData.Replace("�", "'");
            //CreateDocument(HtmlData, path, out newPath);
            ////liju
            //System.Threading.Thread.Sleep(2000);
            ExportToWord("", newPath);
    
            return Index();
        }
        private void ExportToWord(string htmlData, string path)
        {
            try
            {
                FileInfo file = new FileInfo(path);
                if (file.Exists)
                {
                    Response.ContentType = "application/msword";
                    Response.AddHeader("Content-Disposition", "Attachment; filename= Facesheet.doc");
                    Response.AddHeader("Content-Length", file.Length.ToString());
                    Response.TransmitFile(file.FullName);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }



}

