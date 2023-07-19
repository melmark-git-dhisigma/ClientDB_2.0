using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Objects.SqlClient;
using ClientDB.Models;
using ClientDB.AppFunctions;
using ClientDB.ParentServiceReference;
using System.IO;
using System.Text;
using System.Configuration;

namespace ClientDB.Controllers
{
    public class ProgressController : Controller
    {
        //
        // GET: /Progress/
        Other_Functions objFuns = new Other_Functions();
        ProgressExprtApp objExprt = new ProgressExprtApp();
        clsSession sess = null;
        // ParentServiceReference parentService=new ParentServiceReference()
        ParentServiceClient parentService = new ParentServiceClient();
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult Index()
        {
            // return View();
            return RedirectToAction("ProgressGrid");
        }
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult ProgressGrid(ClientDB.Models.ProgressGrid mdl)
        {
            Progress model = new Progress();
            sess = (clsSession)Session["UserSessionClient"];

            int SchoolID = sess.SchoolId, StudentID = sess.StudentId, iepID = sess.IEPId;

            mdl.ProgressReports = parentService.GetProgressReport(StudentID, SchoolID);

            return View("ProgressView", mdl);
        }
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult ProgressRpt1(int IEPId, int Schoolid, int Studentid)
        {
            Session["studentId"] = Studentid;
            Session["IEPID"] = IEPId;
            Session["schoolId"] = Schoolid;
            return ProgressRpt("");
        }
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult ProgressRpt(string Id = "")
        {
            if (Id != "")
            {
                sess = (clsSession)Session["UserSessionClient"];
                // if (Id != 0) sess.IEPId = Id;
                int SchoolID = sess.SchoolId;
                int StudentID = sess.StudentId;
                int iepID = Convert.ToInt32(sess.IEPId);
                return ProgressReport(Id, SchoolID, StudentID);
            }
            else
            {
                sess = (clsSession)Session["UserSessionClient"];
                //if (Id != 0) sess.IEPId = Id;
                int SchoolID = sess.SchoolId, StudentID = sess.StudentId, iepID = sess.IEPId;
                return ProgressReport(Id, SchoolID, StudentID);
            }


        }
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult ProgressReport(string Vals, int schoolID, int Studentid)
        {
            int IEPId = 0;
            string Status = "";
            sess = (clsSession)Session["UserSessionClient"];

            if (Vals != null && Vals.Length > 2)
            {
                string[] ar = Vals.Split('-');
                if (ar.Count() > 0)
                {
                    IEPId = Convert.ToInt16(ar[0]);
                    if (IEPId != 0) sess.IEPId = IEPId;
                    Status = Convert.ToString(ar[1]);
                }
            }

            Progress model = new Progress();


            //if (Id != 0) sess.IEPId = Id;

            //int SchoolID = sess.SchoolId, StudentID = sess.StudentId, iepID = sess.IEPId;
            Other_Functions objFuns = new Other_Functions();
            if (sess != null)
            {
                ViewBag.permission = (objFuns.setClientPermission("Progress Report") == 2) ? "true" : "false";
                model.ProgressInfo = parentService.CLientsReport(schoolID, Studentid, IEPId);
            }

            ClientDB.Models.ProgressDetails objProgress = new ClientDB.Models.ProgressDetails();

            sess.IEPId = model.ProgressInfo.stdtIEPId;
            objProgress.SclDistrictName = model.ProgressInfo.SclDistrictName;
            objProgress.DOB = model.ProgressInfo.DOB;
            objProgress.IEPDtFrom = model.ProgressInfo.IEPDtFrom;
            objProgress.IEPDtTo = model.ProgressInfo.IEPDtTo;
            objProgress.StudentName = model.ProgressInfo.StudentName;
            objProgress.SclDistrictAddress = model.ProgressInfo.SclDistrictAddress;
            objProgress.SclDistrictContact = model.ProgressInfo.SclDistrictContact;
            objProgress.SclDistrictName = model.ProgressInfo.SclDistrictName;
            objProgress.ID = model.ProgressInfo.ID;

            objProgress.Status = Status;

            foreach (var GL in model.ProgressInfo.GoalDt)
            {
                ClientDB.Models.GoalData Goal = new ClientDB.Models.GoalData();
                ClientDB.Models.GridListPlacement PlcacementList = new ClientDB.Models.GridListPlacement();
                ClientDB.Models.ReportInfo ReportInfoRptList = new ClientDB.Models.ReportInfo();
                ClientDB.Models.ReportDetails RDet = new ClientDB.Models.ReportDetails();

                Goal.Goalid = GL.Goalid;
                Goal.GoalLPRelId = GL.GoalLPRelId;
                Goal.GoalName = GL.GoalName;
                Goal.GoalNo = GL.GoalNo;
                Goal.Lessonplanid = GL.Lessonplanid;
                Goal.LessonplanName = GL.LessonplanName;

                foreach (var PL in GL.PlcacementList)
                {
                    PlcacementList.objective1 = PL.objective1;
                    PlcacementList.objective2 = PL.objective2;
                    PlcacementList.objective3 = PL.objective3;


                }
                Goal.PlcacementList.Add(PlcacementList);
                foreach (var RP in GL.RptList)
                {
                    ReportInfoRptList = new ClientDB.Models.ReportInfo();
                    ReportInfoRptList.rptid = RP.rptid;
                    ReportInfoRptList.rptdate = (DateTime)RP.rptdate;
                    ReportInfoRptList.rptinfo = RP.rptinfo;
                    ReportInfoRptList.goalid = RP.goalid;
                    ReportInfoRptList.stdtIEPId = RP.stdtIEPId;
                    ReportInfoRptList.VisibleToParent = (bool)RP.VisibleToParent;
                    ViewBag.IsChecked = RP.VisibleToParent;
                    Goal.RptList.Add(ReportInfoRptList);
                }


                foreach (var RP in GL.ProgressInfo)
                {
                    RDet.iepid = RP.iepid;
                    RDet.rptdate = RP.rptdate;
                    RDet.startdate = RP.startdate;
                    RDet.enddate = RP.enddate;
                    RDet.Status = RP.Status;
                    Goal.ReportDetails.Add(RDet);
                }


                objProgress.GoalDt.Add(Goal);

            }



            //if (ViewBag.IsChecked == true)
            //{
            //    ViewBag.IsChecked = "checked";
            //}
            //else
            //{
            //    ViewBag.IsChecked = "";
            //}



          
            return View("ProgressRpt", objProgress);

        }

        //public ActionResult saveReport(Progress model)
        //{

        //    return View();
        //}
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult saveReport(ProgressList model, string visibleChkBox)
        {
            string result = "";

            //   sess = (clsSession)Session["UserSessionClient"];
            bool IsVisible = false; if (visibleChkBox != "") IsVisible = Convert.ToBoolean(visibleChkBox);
            //     bool IsVisible = Convert.ToBoolean(visibleChkBox);
            result = objFuns.SaveReportData(model, IsVisible);

            if (result == "sucess")
            {
                TempData["ProgressMessage"] = "Data Saved Successfully";
                return ProgressRpt("");
            }
            else
            {
                TempData["ProgressMessage"] = "Failed To Insert Data";
                return View();
            }
        }
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult ExportAllData(Progress model)
        {
            sess = (clsSession)Session["UserSessionClient"];
            string result = "";
            string path = Server.MapPath("~\\Administration\\PRTemplate\\PR1.docx");

            result = objExprt.ExportAllNew(sess, path);

            string dwnld = result;
            FileInfo file = new FileInfo(dwnld);
            if (file.Exists)
            {
                if (dwnld.ToLower().EndsWith(".docx"))
                {
                    string NewFileName = "ProgressReportDoc.doc";
                    Response.Clear();
                    Response.ContentType = "application/msword";
                    Response.AddHeader("Content-Disposition", "Attachment; filename=\"" + NewFileName);
                    Response.AddHeader("Content-Length", file.Length.ToString());
                    Response.TransmitFile(file.FullName);
                    Response.End();
                }
                file.Delete();
            }
            if (result == "")
            {
                return Index();
            }
            else
                return Index();
        }

        //public FileStreamResult CreateFile(string result)
        //{
        //    //todo: add some data from your database into that string:
        //   // var string_with_your_data = "";

        //    var byteArray = Encoding.ASCII.GetBytes(result);
        //    var stream = new MemoryStream(byteArray);

        //    return File(stream, "text/plain", "your_file_name.doc");
        //}
    }


}
