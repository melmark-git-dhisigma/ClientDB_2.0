using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ClientDB.DbModel;
using ClientDB.Models;
using ClientDB.AppFunctions;
using System.Web.Configuration;

namespace ClientDB.Controllers
{
    public class PlacementController : Controller
    {
        //
        // GET: /Placement/
        Other_Functions objFuns = new Other_Functions();
        public clsSession sess = null;
        public ActionResult Index()
        {
            sess = (clsSession)Session["UserSessionClient"];
            if (sess != null)
            {
                ViewBag.permission = (objFuns.setClientPermission("Placement") == 2) ? "true" : "false";
            }
            return View();
        }
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult Placement(int page = 1, int pageSize = 5)
        {
            string school = WebConfigurationManager.AppSettings["Server"];
            ContactSearch search = new ContactSearch();
            PlacementModel bindModel = new PlacementModel();
            sess = (clsSession)Session["UserSessionClient"];
            if (sess != null)
            {
                ViewBag.permission = (objFuns.setClientPermission("Placement") == 2) ? "true" : "false";
                ViewBag.school = school;
                if (TempData["noticeLimit"] != "" && TempData["noticeLimit"] != null)
                {
                    ViewBag.LimitError = TempData["noticeLimit"];
                }
                bindModel = PlacementModel.fillPlacement(page, pageSize);
            }
            return View(bindModel);
        }

        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult AddPlacement(int id)
        {
            string school = WebConfigurationManager.AppSettings["Server"];
            Other_Functions objFuns = new Other_Functions();
            sess = (clsSession)Session["UserSessionClient"];
            if (sess != null)
            {
                ViewBag.school = school;
                ViewBag.permission = (objFuns.setClientPermission("Placement") == 2) ? "true" : "false";
            }
            AddPlacementModel model = new AddPlacementModel();


            if (id > 0)
            {
                model = objFuns.bindPlacement(id);
            }

            model.PlacementTypeList = objFuns.getType("Placement Type");
            model.PrimaryNurseList = objFuns.getUserType("PRIMARY NURSE");
            model.UnitClerkList = objFuns.getUserType("UNIT CLERK");
            model.BehaviorAnalystList = objFuns.getUserType("BEHAVIOR ANALYST");
            model.DepartmentList = objFuns.getType("Department");

            model.PlacementDepartmentList = model.GetPlacementDepartment(sess.SchoolId);
            model.PlacementReasonList = model.GetPlacementReason(sess.SchoolId);
            model.LocationList = model.GetLocationList();
            return View(model);

        }

        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult SavePlacement(AddPlacementModel model)
        {
            if (Session["PlacementStat"] != null && Session["PlacementStat"].ToString() == "D")
            {
                return Content(Newtonsoft.Json.JsonConvert.SerializeObject(new
                {
                    logicError = true,
                    ErrorMsg = "Student is discharged. Activate student for placement operations."
                }));
            }
            BiWeeklyRCPNewEntities dbobj = new BiWeeklyRCPNewEntities();
            string result = "";
            Other_Functions objFuns = new Other_Functions();
            sess = (clsSession)Session["UserSessionClient"];
            int ClientID = sess.StudentId;

            int classId = Convert.ToInt32(model.LocationId);
            var stMaxCount = dbobj.Classes.Where(x => x.ClassId == classId && x.ActiveInd == "A").SingleOrDefault();

            if (stMaxCount.MaxStudents == null || stMaxCount.MaxStudents.ToString() == "0")
            {
                result = objFuns.SavePlacementData(model);
                
            }
            else
            {
                
                var nofStudents = dbobj.StdtClasses.Where(x => x.ClassId == classId && x.ActiveInd == "A").Count();
                if (model.Id != 0)
                {

                    Placement placement = new Placement();
                    placement = dbobj.Placements.Where(objPlacement => objPlacement.PlacementId == model.Id && objPlacement.StudentPersonalId == ClientID).SingleOrDefault();
                    if (placement.Location == classId)
                    {
                        result = objFuns.SavePlacementData(model);
                    }
                    else
                    {
                        if (stMaxCount.MaxStudents > nofStudents)
                        {
                            result = objFuns.SavePlacementData(model);
                        }
                        else
                        {
                            // TempData["noticeLimit"] = "Student Limit Exceed for the class";
                            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(new { logicError = true, id = model.Id, ErrorMsg = "This Location is Full" }));
                        }
                    }
                }
                else
                {

                    if (stMaxCount.MaxStudents > nofStudents)
                    {
                        result = objFuns.SavePlacementData(model);
                    }
                    else
                    {
                        // TempData["noticeLimit"] = "Student Limit Exceed for the class";
                        return Content(Newtonsoft.Json.JsonConvert.SerializeObject(new { logicError = true, id = model.Id, ErrorMsg = "This Location is Full" }));
                    }
                }
            }
           
            if (result == "No Client Selected")
            {
                TempData["notice"] = "No Client Selected";
            }
            var res = dbobj.Update_StudentStatus_Automatically(sess.StudentId, sess.LoginId);
            var StudStatus = dbobj.StudentPersonals.Where(x => x.StudentPersonalId == sess.StudentId && x.SchoolId == sess.SchoolId).SingleOrDefault();
            if (StudStatus != null)
            {
                Session["PlacementStat"] = (StudStatus.PlacementStatus == null) ? "A" : StudStatus.PlacementStatus;
            }
            //return RedirectToAction("Placement");
            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(new { logicError = false, id = model.Id }));

        }
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult DeletePlacementDetails(int id)
        {
            if (Session["PlacementStat"] != null && Session["PlacementStat"].ToString() == "D")
            {
                return Content(Newtonsoft.Json.JsonConvert.SerializeObject(new
                {
                    logicError = true,
                    ErrorMsg = "Student is discharged. Activate student for placement operations."
                }));
            }
            BiWeeklyRCPNewEntities dbobj = new BiWeeklyRCPNewEntities();
            sess = (clsSession)Session["UserSessionClient"];
            Other_Functions objFuns = new Other_Functions();
            objFuns.deletePlacement(sess.StudentId, id);
            var res = dbobj.Update_StudentStatus_Automatically(sess.StudentId, sess.LoginId);
            var StudStatus = dbobj.StudentPersonals.Where(x => x.StudentPersonalId == sess.StudentId && x.SchoolId == sess.SchoolId).SingleOrDefault();
            if (StudStatus != null)
            {
                Session["PlacementStat"] = (StudStatus.PlacementStatus == null) ? "A" : StudStatus.PlacementStatus;
            }
            return RedirectToAction("Placement");

        }


    }
}
