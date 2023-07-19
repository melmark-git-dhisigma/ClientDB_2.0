using ClientDB.AppFunctions;
using ClientDB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ClientDB.DbModel;
using System.Net;
using System.Web.ClientServices;
using System.Xml;
using System.IO;

namespace ClientDB.Controllers
{
    public class CallLogController : Controller
    {
        Other_Functions objFuns = new Other_Functions();
        clsSession sess = null;
        public ActionResult Index()
        {
            sess = (clsSession)Session["UserSessionClient"];
            if (sess != null)
            {
                ViewBag.permission = (objFuns.setClientPermission("Family & Agency Communication Log") == 2) ? "true" : "false";
            }
            return View();
        }

        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult CallLog(int page = 1, int pageSize = 5)
        {
            CallLogModel bindModel = new CallLogModel();
            sess = (clsSession)Session["UserSessionClient"];
            if (sess != null)
            {
                ViewBag.permission = (objFuns.setClientPermission("Family & Agency Communication Log") == 2) ? "true" : "false";
                bindModel = bindModel.getCallLog(page, pageSize);
            }
            return View(bindModel);
        }

        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult CallLogDetails(int CallLogId)
        {
            CallLogModel Model = new CallLogModel();
            BiWeeklyRCPNewEntities objData = new BiWeeklyRCPNewEntities();
            //ref_CallLogs refcall = new ref_CallLogs();
            var data = objData.ref_CallLogs.Where(m => m.CallLogId == CallLogId).ToList();
            foreach (var item in data)
            {
                Model.NameofContact= item.Nameofcontact;
                Model.Conversation = item.Conversation;
            }
           
            return View("CallLogDetails", Model);
        }

        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult CallLog2(CallLogModel model, int CallLogid = 0)
        {
            sess = (clsSession)Session["UserSessionClient"];
            BiWeeklyRCPNewEntities objData = new BiWeeklyRCPNewEntities();
            IList<LookUp> Relation = new List<LookUp>();

            ClientDB.AppFunctions.Other_Functions OF = new ClientDB.AppFunctions.Other_Functions();
            ViewBag.permission = (OF.setClientPermission("Family & Agency Communication Log") == 2) ? "true" : "false";

            SelectListItem onesele = new SelectListItem { Text = "-- Select --", Value = "" };
            IList<SelectListItem> relationSelecteditem = new List<SelectListItem>();
            relationSelecteditem.Add(onesele);
            try
            {
                Relation = objData.LookUps.Where(x => x.LookupType == "Relationship").OrderBy(x => x.LookupName).ToList();
            }
            catch (Exception ex)
            {
            }
            var relationSelecteditemsub = (from Relationship in Relation select new SelectListItem { Text = Relationship.LookupName, Value = Relationship.LookupId.ToString() }).ToList();
            foreach (SelectListItem sele in relationSelecteditemsub)
            {
                relationSelecteditem.Add(sele);
            }
            model.RelationshipList = relationSelecteditem;
            if (CallLogid == 0)
            {
                model.CallDateShow2 = DateTime.Now.Date.ToString("MM/dd/yyyy").Replace("-", "/");
                model.CallTimeShow2 = DateTime.Now.ToString("hh:mmtt");
                model.StaffName2 = sess.UserName;
                if (sess.StudentId > 0)
                {
                    var Refname = objData.StudentPersonals.Where(x => x.StudentPersonalId == sess.StudentId).SingleOrDefault();
                    model.ReferralName2 = Refname.LastName + "," + Refname.FirstName;
                }
            }
            else
            {
                Session["CallLogId"] = CallLogid;
                model.CallLogId2 = CallLogid;
                var returnModel = objData.ref_CallLogs.Where(x => x.CallLogId == CallLogid).SingleOrDefault();
                model.CallDateShow2 = (returnModel.CallTime == null) ? null : Convert.ToDateTime(returnModel.CallTime).ToString("MM/dd/yyyy").Replace("-", "/");
                model.CallTimeShow2 = (returnModel.CallTime == null) ? null : Convert.ToDateTime(returnModel.CallTime).ToString("hh:mmtt");
                model.StaffName2 = returnModel.StaffName;
                var Refname = objData.StudentPersonals.Where(x => x.StudentPersonalId == returnModel.StudentId).SingleOrDefault();
                model.ReferralName2 = Refname.LastName + "," + Refname.FirstName;
                model.Conversation2 = returnModel.Conversation;
                model.ContactlogType = Convert.ToString(returnModel.CallFlag);
                model.NameOfContact2 = returnModel.Nameofcontact;
            }
            return View(model);
        }

        // Call log with ajax   
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        [MultiButton(MatchFormKey = "SaveCallLog", MatchFormValue = "Save")]
        public ActionResult SaveCallLog2_2(CallLogModel model)
        {
            sess = (clsSession)Session["UserSessionClient"];
            BiWeeklyRCPNewEntities objData = new BiWeeklyRCPNewEntities();
            ref_CallLogs Objcalllog = new ref_CallLogs();
            DateTime dtcalldate = new DateTime();
            Other_Functions OF = new Other_Functions();
            if (sess != null)
            {
                int Type = Convert.ToInt32(model.ContactlogType);
                if (model.Relationship2 == "")
                    Objcalllog.RelationshipId = 0;
                else
                    Objcalllog.RelationshipId = Convert.ToInt32(model.Relationship2);
                Objcalllog.StudentId = sess.StudentId;
                Objcalllog.SchoolId = sess.SchoolId;
                Objcalllog.StaffName = model.StaffName2;
                Objcalllog.CallFlag = Type;
                if (model.CallDateShow2 != null)
                    dtcalldate = DateTime.ParseExact(model.CallDateShow2, "MM'/'dd'/'yyyy", System.Globalization.CultureInfo.CurrentCulture);
                if (model.CallTimeShow2 == null && model.CallDateShow2 != null)
                    model.CallTimeShow2 = "00:00AM";
                if (model.CallDateShow2 != null)
                {
                    model.CallTime2 = dtcalldate.Add(TimeSpan.Parse(OF.amPmTo24hourConverter(model.CallTimeShow2)));
                }
                Objcalllog.CallTime = model.CallTime2;
                Objcalllog.Nameofcontact = model.NameOfContact2;
                Objcalllog.AppointmentTime = model.AppntTime2;
                Objcalllog.Conversation = model.Conversation2;
                Objcalllog.CreatedBy = sess.LoginId;
                Objcalllog.CreatedOn = DateTime.Now;
                Objcalllog.Draft = "Y";
                Objcalllog.Type = "NA";
                Objcalllog.AcReviewId = 0;
                Objcalllog.QueueStatusId = 0;
                objData.ref_CallLogs.Add(Objcalllog);
                objData.SaveChanges();
                model.CallLogId2 = 0;


            }
            return Content("Success*" + sess.StudentId.ToString());
        }


        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        [MultiButton(MatchFormKey = "SaveCallLog", MatchFormValue = "Update")]
        public ActionResult SaveCallLog2_2(CallLogModel model, string a)
        {
            sess = (clsSession)Session["UserSessionClient"];
            BiWeeklyRCPNewEntities objData = new BiWeeklyRCPNewEntities();
            ref_CallLogs Objcalllog = new ref_CallLogs();
            DateTime dtcalldate = new DateTime();
            Other_Functions OF = new Other_Functions();
            if (sess != null)
            {
                if (Session["CallLogId"] != null)
                {
                    int callLogid = Convert.ToInt32(Session["CallLogId"]);
                    Objcalllog = objData.ref_CallLogs.Where(x => x.CallLogId == callLogid).SingleOrDefault();
                    int Type = Convert.ToInt32(model.ContactlogType);
                    Objcalllog.StudentId = sess.StudentId;
                    Objcalllog.SchoolId = sess.SchoolId;
                    Objcalllog.StaffName = model.StaffName2;
                    Objcalllog.CallFlag = Type;
                    if (model.CallDateShow2 != null)
                        dtcalldate = DateTime.ParseExact(model.CallDateShow2, "MM'/'dd'/'yyyy", System.Globalization.CultureInfo.CurrentCulture);
                    if (model.CallTimeShow2 == null && model.CallDateShow2 != null)
                        model.CallTimeShow2 = "00:00AM";
                    if (model.CallDateShow2 != null)
                    {
                        model.CallTime2 = dtcalldate.Add(TimeSpan.Parse(OF.amPmTo24hourConverter(model.CallTimeShow2)));
                    }
                    Objcalllog.CallTime = model.CallTime2;
                    Objcalllog.Nameofcontact = model.NameOfContact2;
                    Objcalllog.AppointmentTime = model.AppntTime2;
                    Objcalllog.Conversation = model.Conversation2;
                    objData.SaveChanges();
                }


            }
            return Content("Success*" + sess.StudentId.ToString());
        }

        #region Auto Complete

        public JsonResult AutoCompleteCountry(string term)
        {

            Other_Functions OF = new Other_Functions();
            IList<StudentSearchDetails> val = OF.GetStudentSearch(term);

            if (val != null)
            {

                var result = (from r in val
                              where r.ReferralName.ToLower().Contains(term.ToLower())
                              select new { r.ReferralName, r.ReferralId });
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(null, JsonRequestBehavior.AllowGet);

        }

        public JsonResult AutoCompleteRefsearch(string term)
        {

            Other_Functions OF = new Other_Functions();
            IList<StudentSearchDetails> val = OF.GetStudentSearch_ref(term);

            string[] args = term.Split('$');

            if (args.Length > 1)
            {
                term = args[0].ToString();
            }
            if (val != null)
            {

                var result = (from r in val
                              where r.ReferralName.ToLower().Contains(term.ToLower())
                              select new { r.ReferralName, r.ReferralName_short });
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(null, JsonRequestBehavior.AllowGet);

        }

        public JsonResult AutoCompleteStaffName(string term)
        {

            Other_Functions OF = new Other_Functions();
            IList<StaffSearchDetails> val = OF.GetStaffList(term);

            if (val != null)
            {

                var result = (from r in val
                              where (r.UserName.ToLower().Contains(term.ToLower()))
                              select new { r.UserName }).Distinct();

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(null, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AutoCompleteContactName(string term)
        {

            Other_Functions OF = new Other_Functions();
            IList<ContactNameSearchDetails> val = OF.GetContactNameList(term);

            if (val != null)
            {

                var result = (from r in val
                              where (r.ContactName.ToLower().Contains(term.ToLower()))
                              select new { r.ContactName, r.ContactId }).Distinct();

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(null, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }

   
}
