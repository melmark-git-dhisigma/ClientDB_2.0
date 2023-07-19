using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ClientDB.Models;
using ClientDB.AppFunctions;

namespace ClientDB.Controllers
{
    public class MedicalController : Controller
    {
        //
        // GET: /Medical/
        Other_Functions dbFunction = null;
        public clsSession sess = null;
        public ActionResult Index()
        {
            return View();
        }
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult Medical(MedicalModel model)
        {
            sess = (clsSession)Session["UserSessionClient"];
            dbFunction = new Other_Functions();
            if (sess != null)
            {
                ViewBag.permission = (dbFunction.setClientPermission("Medical") == 2) ? "true" : "false";
            }
            string country = dbFunction.getCountryName();
            model = dbFunction.getAllergieData(sess.StudentId);
            IList<SelectListItem> x = new List<SelectListItem>();
            x.Add(new SelectListItem{Text=""+country, Value="1"});
            model.CountryList=x;

            model.StateList = dbFunction.getStateList();
            //model.CountryList = dbFunction.getCountryList();
            //model.StateList = dbFunction.getStateList(model.CountryId);
            model.CalenderDatas = dbFunction.CalanderDatas(sess.StudentId);
            model.PhysicianList = dbFunction.getPhysicianList();

            MedicalModel Medmodel = new MedicalModel();
            //Medmodel = dbFunction.getAllergieData(sess.StudentId);
            //if (Medmodel != null)
            //{
            //    model.Allergie = Medmodel.Allergie;
            //    model.Seizures = Medmodel.Seizures;
            //    model.Diet = Medmodel.Diet;
            //    model.Other = Medmodel.Other;
           // }
            Medmodel = dbFunction.getDiagnosisData(sess.StudentId);
            if (Medmodel != null)
            {
                model.Diagnosis = Medmodel.Diagnosis;
            }
            model.ID = sess.StudentId;
            return View("MedicalForm", model);
        }
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult SavemedicalData(MedicalModel model)
        {
            string result = "";
            sess = (clsSession)Session["UserSessionClient"];
            dbFunction = new Other_Functions();
            model.ID = sess.StudentId;
            if (sess != null)
            {
                ViewBag.permission = (dbFunction.setClientPermission("Medical") == 2) ? "true" : "false";
                result = dbFunction.SaveMedicalDatas(model);
                if (result == "Sucess" && model.ID > 0)
                {
                    result = "Data Updated Sucessfully";
                }
                else if (result == "Sucess")
                {
                    result = "Data Saved Sucessfully";
                }
            }
            if (result == "No Client Selected")
            {
                TempData["notice"] = "No Client Selected";
                return RedirectToAction("Medical", model);
            }
            else if (result == "Please go for some other Date")
            {
                TempData["notice"] = result;
                model.DateOfLastPhysicalExam = "";
                return RedirectToAction("Medical", model);
                
            }
            else
            {
                TempData["notice"] = result;
                model.CalenderDatas = dbFunction.CalanderDatas(sess.StudentId);
                return RedirectToAction("Medical", model = null);
            }
        }
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult FillMedicalData(string data)
        {
            MedicalModel model = new MedicalModel();
            dbFunction = new Other_Functions();
            sess = (clsSession)Session["UserSessionClient"];
            string[] Param = data.Split('|');
            Other_Functions objFuns = new Other_Functions();
            if (sess != null)
            {
                ViewBag.permission = (objFuns.setClientPermission("Medical") == 2) ? "true" : "false";
                model = dbFunction.FillMedicalDatas(Convert.ToInt32(Param[0]));
            }
            model.CountryList = dbFunction.getCountryList();
            model.StateList = dbFunction.getStateList(model.CountryId);
            model.CalenderDatas = dbFunction.CalanderDatas(sess.StudentId);
            model.PhysicianList = dbFunction.getPhysicianList();
            if (Param[1] == "Fill")
            {
                return View("FillMedicalData", model);
            }
            else
            {
                return View("MedicalForm", model);
            }
        }
    }
}
