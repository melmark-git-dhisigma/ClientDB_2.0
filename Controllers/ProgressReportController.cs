using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ClientDB.Models;
using ClientDB.AppFunctions;
using ClientDB.DbModel;
using System.Data;
using System.Collections;
using System.IO;

namespace ClientDB.Controllers
{
    public class ProgressReportController : Controller
    {
        //
        // GET: /ProgressReport/
        Other_Functions objFuns = new Other_Functions();
        public clsSession sess = null;

        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ViewResult Index(int filter = 1)
        {
            sess = (clsSession)Session["UserSessionClient"];
            ProgressReport bindModel = new ProgressReport();
            if (sess != null)
            {
                ViewBag.permission = (objFuns.setClientPermission("Progress Report") == 2) ? "true" : "false";
            }
            if (sess != null)
            {
                //bindModel = FillCRMonthly();
                bindModel = FillProReportList(filter);
            }
            if (bindModel == null)
            {
                bindModel = new ProgressReport();
                return View(bindModel);
            }
            else
            {
                bindModel.reportType = filter;
                return View(bindModel);
            }
        }

        public ActionResult GetProgRpt(int studentid)
        {
            setSession();
            sess = (clsSession)Session["UserSessionClient"];
            sess.StudentId = studentid;
            if (Session["PlacementStat"] == null)
            {
                Session["PlacementStat"] = "A";
            }
            //FillSummary();
            return View();
        }

        public void setSession()
        {
            sess = new clsSession();
            // sess = (clsSession)Session["UserSessionClient"];
            BiWeeklyRCPNewEntities Objdata = new BiWeeklyRCPNewEntities();

            if (Session["Values"] != null)
            {
                string Values = Session["Values"].ToString();
                string[] arValues = Values.Split('#');
                sess.LoginId = Convert.ToInt16(arValues[1]);
                sess.SchoolId = Convert.ToInt16(arValues[0]);
                //sess.UserName = Convert.ToString(arValues[3]);

                if (sess.LoginId != 0)              //set the remaining session used in the Referral DB
                {
                    var Role = (from Objrole in Objdata.Roles
                                join objrgp in Objdata.RoleGroups on Objrole.RoleId equals objrgp.RoleId
                                select new
                                {
                                    RoleId = Objrole.RoleId,
                                    Roledesc = Objrole.RoleDesc,
                                    SchoolId = Objrole.SchoolId
                                }).ToList();
                    var Usr = (from Objrole in Role
                               from Objusr in Objdata.Users
                               where Objusr.UserId == sess.LoginId
                               select new
                               {
                                   Objrole.RoleId,
                                   Objrole.Roledesc,

                                   Objusr.UserId,
                                   Objusr.UserFName,
                                   Objusr.UserLName,
                                   Objusr.Gender
                               }).ToList();

                    if (Usr == null) return;
                    if (Usr.Count() > 0)
                    {

                        sess.IsLogin = true;
                        sess.LoginTime = (DateTime.Now.ToShortTimeString()).ToString();

                        sess.LoginId = Convert.ToInt32(Usr[0].UserId);
                        sess.UserName = Convert.ToString(Usr[0].UserLName + "," + Usr[0].UserFName);
                        sess.RoleId = Convert.ToInt32(Usr[0].RoleId);
                        sess.Gender = Convert.ToString(Usr[0].Gender);
                        sess.RoleName = Convert.ToString(Usr[0].Roledesc);
                        sess.SessionID = Session.SessionID.ToString();
                        //sess.ReferralId = 0;
                    }
                }

                Session["UserSessionClient"] = sess;
            }

        }


        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ProgressReport FillProReportList(int filter)
        {
            ProgressReport model = new ProgressReport();
            sess = (clsSession)Session["UserSessionClient"];
            BiWeeklyRCPNewEntities dbobj = new BiWeeklyRCPNewEntities();

            int proReportId = 0;
            var proReportList = new ProgressReport();

            if (filter == 0)
                filter = 1;

            if (filter == 1)
            {
                proReportList.Rpt_List = (from x in dbobj.ProgressRpt_Doc
                                          where x.SchoolId == sess.SchoolId && x.StudentId == sess.StudentId && x.TabId == 1
                                          select new ProRptList
                                                        {
                                                            ProReportId = x.DocId,
                                                            RptCreatedOn = x.CreatedOn
                                                        }).ToList();

                

            }
            if (filter == 2)
            {
                proReportList.Rpt_List = (from x in dbobj.ProgressRpt_Doc
                                          where x.SchoolId == sess.SchoolId && x.StudentId == sess.StudentId && x.TabId == 2
                                          select new ProRptList
                                          {
                                              ProReportId = x.DocId,
                                              RptCreatedOn = x.CreatedOn
                                          }).ToList();



            }
            if (filter == 3)
            {
                proReportList.Rpt_List = (from x in dbobj.ProgressRpt_RTF_Q
                                          where x.SchoolId == sess.SchoolId && x.StudentId == sess.StudentId
                                          select new ProRptList
                                          {
                                              ProReportId = x.RTFQId,
                                              RptCreatedOn = x.CreatedOn
                                          }).ToList();



            }
            if (filter == 4)
            {
                proReportList.Rpt_List = (from x in dbobj.ProgressRpt_RTF_M
                                          where x.SchoolId == sess.SchoolId && x.StudentId == sess.StudentId
                                          select new ProRptList
                                          {
                                              ProReportId = x.RTFMId,
                                              RptCreatedOn = x.CreatedOn
                                          }).ToList();



            }
            if (filter == 5)
            {
                proReportList.Rpt_List = (from x in dbobj.ProgressRpts
                                          where x.SchoolId == sess.SchoolId && x.StudentId == sess.StudentId
                                          select new ProRptList
                                          {
                                              ProReportId = x.ProgressId,
                                              RptCreatedOn = x.CreatedOn
                                          }).ToList();



            }
            if (proReportList.Rpt_List.Count == 0)
            {
                proReportList.Rpt_List = null;
            }

            return proReportList;
        }

        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult DeleteProReportList(int id, int tabId)
        {
            
            //objFuns.deleteProRptList(id);
            ProgressReport model = new ProgressReport();
            if (id != 0)
            {
                if (tabId == 1)
                {
                    objFuns.deleteTab1Document(id);
                }
                else if (tabId == 2)
                {
                    objFuns.deleteTab2Document(id);
                }
                else if (tabId == 3)
                {
                    objFuns.deleteTab3List(id);
                }
                else if (tabId == 4)
                {
                    objFuns.deleteTab4List(id);
                }
                else if (tabId == 5)
                {
                    objFuns.deleteTab5List(id);
                }

            }
            model.TabId = tabId;
            return RedirectToAction("Index", new { filter = tabId });

        }

        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult DeleteTab1Doc(int id)
        {
            ProgressReport model = new ProgressReport();
            objFuns.deleteTab1Document(id);
            return RedirectToAction("Index", new { filter = 1 });
        }

        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult DeleteTab2Doc(int id)
        {
            ProgressReport model = new ProgressReport();
            objFuns.deleteTab2Document(id);
            return RedirectToAction("Index", new { filter = 2 });
        }

        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult EditProgressReport(int id, int tabId)
        {
            ProgressReport model = new ProgressReport();
            sess = (clsSession)Session["UserSessionClient"];
            if (sess != null)
            {
                ViewBag.permission = (objFuns.setClientPermission("Progress Report") == 2) ? "true" : "false";
            }
            if (id != 0)
            {
                if (tabId == 1)
                {
                    model = FillTab1(id);
                }
                else if (tabId == 2)
                {
                    model = FillTab2(id);
                }
                else if (tabId == 3)
                {
                    model = FillRTFQuarterly(id);
                }
                else if (tabId == 4)
                {
                    model = FillRTFMonthly(id);
                }
                else if (tabId == 5)
                {
                    model = FillCRMonthly(id);
                }

            }
            model.TabId = tabId;
            return View("Page", model);
        }

        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult NewProgressReport(int id, int tabId)
        {
            ProgressReport model = new ProgressReport();
            if (id != 0)
            {
                if (tabId == 1)
                {
                    model = FillTab1(id);
                }
                else if (tabId == 2)
                {
                    model = FillTab2(id);
                }
                else if (tabId == 3)
                {
                    model = FillRTFQuarterly(id);
                }
                else if (tabId == 4)
                {
                    model = FillRTFMonthly(id);
                }
                else if (tabId == 5)
                {
                    model = FillCRMonthly(id);
                }

            }
            else if (tabId == 5)
            {
                model = FillCRMonthly(id);
            }
            model.TabId = tabId;
            return View("Page", model);
        }

        [HttpPost]
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult SaveDocTab1(ProgressReport model, HttpPostedFileBase profilePicture1)
        {
            sess = (clsSession)Session["UserSessionClient"];
            string result = "";

            if (sess != null)
            {
                ViewBag.permission = (objFuns.setClientPermission("Progress Report") ==2) ? "true" : "false";
            }
            if (profilePicture1 != null)
            {
                result = objFuns.SaveDocTabOne(model, profilePicture1);
            }

            if (result == "No Client Selected")
            {
                TempData["notice"] = "No Client Selected";
            }
            return RedirectToAction("Index", new { filter = 1 });
        }

        //[HttpPost]
        //[OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        //public ActionResult SaveDocTab1()
        //{
        //    return RedirectToAction("Page");
        //}

        [HttpPost]
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult SaveDocTab2(ProgressReport model, HttpPostedFileBase profilePicture2)
        {
            sess = (clsSession)Session["UserSessionClient"];
            string result = "";

            if (sess != null)
            {
                ViewBag.permission = (objFuns.setClientPermission("Progress Report") == 2) ? "true" : "false";
            }
            if (profilePicture2 != null)
            {
                result = objFuns.SaveDocTabTwo(model, profilePicture2);
            }

            if (result == "No Client Selected")
            {
                TempData["notice"] = "No Client Selected";
            }
            return RedirectToAction("Index", new { filter = 2 });
        }

        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ProgressReport FillCRMonthly(int id)
        {
            sess = (clsSession)Session["UserSessionClient"];
            BiWeeklyRCPNewEntities dbobj = new BiWeeklyRCPNewEntities();
            ProgressExprtApp clsProgress = new ProgressExprtApp();
            var CRMonthlyItems = new ProgressReport();

            if (id > 0)
            {
                CRMonthlyItems = (from a in dbobj.ProgressRpts
                                  where (a.ProgressId == id && a.SchoolId == sess.SchoolId && a.StudentId == sess.StudentId)
                                  select new ProgressReport
                                  {
                                      ProgressId=a.ProgressId,
                                      ProReportId = a.ProReportId,
                                      CRM_Academic = a.CRM_Academic,
                                      CRM_Clinical = a.CRM_Clinical,
                                      CRM_Outings = a.CRM_Outings,
                                      CRM_Other = a.CRM_Other
                                  }).First();

               
            }

            var EventResult = dbobj.GetEvents(sess.StudentId, sess.SchoolId, DateTime.Now.AddDays(-90), DateTime.Now).ToList();

            CRMonthlyItems.CRM_PhaseLines = EventResult[0].Eventdata;
            CRMonthlyItems.CRM_ConditionLines = EventResult[1].Eventdata;
            CRMonthlyItems.CRM_ArrowNotes = EventResult[2].Eventdata;

            var temp0 = (from o in dbobj.StudentPersonals
                         where (o.StudentPersonalId == sess.StudentId && o.SchoolId == sess.SchoolId)
                         select new
                         {
                             o.LastName,
                             o.FirstName
                         }).ToList();

            var temp1 = (from b in dbobj.StdtIEP_PE
                         join c in dbobj.AsmntYears on b.AsmntYearId equals c.AsmntYearId
                         where (b.SchoolId == sess.SchoolId && b.StudentId == sess.StudentId)
                         select new
                         {
                             b.EffStartDate,
                             b.EffEndDate,
                             c.AsmntYearStartDt,
                             c.AsmntYearEndDt
                         }).ToList();

            var temp3 = (from l in dbobj.Placements
                         join m in dbobj.LookUps on l.Department equals m.LookupId
                         join n in dbobj.Classes on l.Location equals n.ClassId
                         where (l.SchoolId == sess.SchoolId && l.StudentPersonalId == sess.StudentId)
                         select new
                         {
                             m.LookupName,
                             n.ClassName
                         }).ToList();

            if (temp0.Count != 0)
            {
                CRMonthlyItems.StdtName = Convert.ToString(temp0[0].LastName + ", " + temp0[0].FirstName);
            }

            if (temp1.Count != 0)
            {int i = temp1.Count;
                CRMonthlyItems.IEPYrStart = Convert.ToDateTime(temp1[i-1].EffStartDate);
                CRMonthlyItems.IEPYrEnd = Convert.ToDateTime(temp1[i-1].EffEndDate);
                CRMonthlyItems.PeriodStart = Convert.ToDateTime(temp1[i-1].AsmntYearStartDt);
                CRMonthlyItems.PeriodEnd = Convert.ToDateTime(temp1[i-1].AsmntYearEndDt);
            }

            if (temp3.Count != 0)
            {
                CRMonthlyItems.Program = Convert.ToString(temp3[0].LookupName);
                CRMonthlyItems.Location = Convert.ToString(temp3[0].ClassName);
            }

            //if (id > 0)
            //{
            //    CRMonthlyItems = (from a in dbobj.ProgressRpts
            //                      where (a.ProgressId == id && a.SchoolId == sess.SchoolId && a.StudentId == sess.StudentId)
            //                      select new ProgressReport
            //                      {
            //                          ProgressId=a.ProgressId,
            //                          ProReportId = a.ProReportId,
            //                          CRM_Academic = a.CRM_Academic,
            //                          CRM_Clinical = a.CRM_Clinical,
            //                          CRM_Outings = a.CRM_Outings,
            //                          CRM_Other = a.CRM_Other
            //                      }).First();

               
            //}
            if (id > 0)
            {
                CRMonthlyItems.btnStatus = true;
            }
            else
            {
                CRMonthlyItems.btnStatus = false;
            }
            return CRMonthlyItems;
        }

        //Fill Tab1
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ProgressReport FillTab1(int pId)
        {
            sess = (clsSession)Session["UserSessionClient"];
            BiWeeklyRCPNewEntities dbobj = new BiWeeklyRCPNewEntities();
            ProgressExprtApp clsProgress = new ProgressExprtApp();

            var Tab1Items = new ProgressReport();

            if (pId > 0)
            {
                Tab1Items.Tab1Doc_List = (from a in dbobj.ProgressRpt_Doc
                                          where (a.DocId == pId && a.TabId == 1 && a.SchoolId == sess.SchoolId && a.StudentId == sess.StudentId)
                                          select new Tab1DocList
                                          {
                                              ProReportId = a.ProgressId,
                                              DocumentId = a.DocId,
                                              DocumentName = a.DocumentName,
                                              DocumentType = a.Type,
                                              CreatedOn = a.CreatedOn
                                          }).ToList();
                Tab1Items.btnStatus = true;
            }
            else
            {
                Tab1Items = new ProgressReport();
                Tab1Items.btnStatus = false;
            }
            return Tab1Items;
        }

        //Fill Tab2
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ProgressReport FillTab2(int pId)
        {
            sess = (clsSession)Session["UserSessionClient"];
            BiWeeklyRCPNewEntities dbobj = new BiWeeklyRCPNewEntities();
            ProgressExprtApp clsProgress = new ProgressExprtApp();

            var Tab2Items = new ProgressReport();

            if (pId > 0)
            {
                Tab2Items.Tab2Doc_List = (from a in dbobj.ProgressRpt_Doc
                                          where (a.DocId == pId && a.TabId == 2 && a.SchoolId == sess.SchoolId && a.StudentId == sess.StudentId)
                                               select new Tab2DocList
                                               {
                                                   ProReportId = a.ProgressId,
                                                   DocumentId = a.DocId,
                                                   DocumentName = a.DocumentName,
                                                   DocumentType = a.Type,
                                                   CreatedOn = a.CreatedOn
                                               }).ToList();
                Tab2Items.btnStatus = true;
            }
            else
            {
                Tab2Items = new ProgressReport();
                Tab2Items.btnStatus = false;
            }
            return Tab2Items;
        }

        //Fill RTF Monthly
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ProgressReport FillRTFMonthly(int rtfMId)
        {
            sess = (clsSession)Session["UserSessionClient"];
            BiWeeklyRCPNewEntities dbobj = new BiWeeklyRCPNewEntities();
            ProgressExprtApp clsProgress = new ProgressExprtApp();

            var RTFMonthlyItems = new ProgressReport();

            if (rtfMId > 0)
            {
                RTFMonthlyItems = (from a in dbobj.ProgressRpt_RTF_M
                                   where (a.RTFMId == rtfMId && a.SchoolId == sess.SchoolId && a.StudentId == sess.StudentId)
                                  select new ProgressReport
                                  {
                                      RTFMId=a.RTFMId,
                                      RTF_M_BLStart = a.RTF_M_BLStart,
                                      RTF_M_BLEnd = a.RTF_M_BLEnd,
                                      RTF_M_RptDate = a.RTF_M_RptDate,

                                      RTF_M_BgInfo = a.RTF_M_BgInfo,
                                      RTF_M_BSPlan = a.RTF_M_BSPlan,
                                      RTF_M_Assessments = a.RTF_M_Assessments,
                                      RTF_M_CIntegration = a.RTF_M_CIntegration,
                                      RTF_M_CMedication = a.RTF_M_CMedication,
                                      RTF_M_DPlanning = a.RTF_M_DPlanning,
                                      RTF_M_ADSite = a.RTF_M_ADSite,
                                      RTF_M_ADStay = a.RTF_M_ADStay
                                  }).First();

                ClientDB.AppFunctions.GoalHashData goalHash = ClientDB.AppFunctions.ProgressExprtApp.getLEssonplanData(rtfMId, RTFMonthlyItems);
                RTFMonthlyItems.hash = goalHash;
                RTFMonthlyItems.btnStatus = true;
            }
            else
            {
                RTFMonthlyItems = new ProgressReport();
                RTFMonthlyItems.btnStatus = false;
            }
            return RTFMonthlyItems;
        }

        //Fill RTF Quarterly
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ProgressReport FillRTFQuarterly(int rtfQId)
        {
            sess = (clsSession)Session["UserSessionClient"];
            BiWeeklyRCPNewEntities dbobj = new BiWeeklyRCPNewEntities();
            ProgressExprtApp clsProgress = new ProgressExprtApp();

            var RTFQuarterlyItems = new ProgressReport();

            if (rtfQId > 0)
            {
                RTFQuarterlyItems = (from a in dbobj.ProgressRpt_RTF_Q
                                     where (a.RTFQId == rtfQId && a.SchoolId == sess.SchoolId && a.StudentId == sess.StudentId)
                                  select new ProgressReport
                                  {
                                      RTFQId=a.RTFQId,
                                      RTF_Q_BLStart = a.RTF_Q_BLStart,
                                      RTF_Q_BLEnd = a.RTF_Q_BLEnd,
                                      RTF_Q_RptDate = a.RTF_Q_RptDate,

                                      RTF_Q_TBehavior = a.RTF_Q_TBehavior,
                                      RTF_Q_Outlines = a.RTF_Q_Outlines

                                  }).First();

                ClientDB.AppFunctions.GoalHashData goalHashQuarter = ClientDB.AppFunctions.ProgressExprtApp.getLEssonplanDataQuarter(rtfQId, RTFQuarterlyItems);
                RTFQuarterlyItems.hashQuarter = goalHashQuarter;
                RTFQuarterlyItems.btnStatus = true;
            }
            else
            {
                RTFQuarterlyItems = new ProgressReport();
                RTFQuarterlyItems.btnStatus = false;
            }
            return RTFQuarterlyItems;
        }






        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult saveCRMonthly(ProgressReport model, int ProgressId)
        {
            int CId = ProgressId;
            string result = "";
            result = objFuns.saveCRMonthlyData(model, CId);
            return RedirectToAction("Index", new { filter = 5 });
        }

        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult SaveRTFMonthly(ProgressReport model, int RTFMId)
        {
            int MId = RTFMId;
            string result = "";
            result = objFuns.SaveRTFMonthlyData(model, MId);
            return RedirectToAction("Index", new { filter = 4 });
        }

        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult ExportAllProgress(int id,string type)
        {
            ProgressExprtApp objPro = new ProgressExprtApp();
            sess = (clsSession)Session["UserSessionClient"];
            string result = "";
            if (type == "Ouarter")
            {
                string path = Server.MapPath("~\\Templates\\Quarter.docx");
                result = objPro.ExpAllProgressQuarter(sess, path, id);
            }
            if (type == "Month")
            {
                string path = Server.MapPath("~\\Templates\\Goal.docx");
                result = objPro.ExpAllProgressMonth(sess, path, id);
            }
            string dwnld = result;
            FileInfo file = new FileInfo(dwnld);
            if (file.Exists)
            {
                if (dwnld.ToLower().EndsWith(".docx"))
                {
                    string NewFileName = file.FullName.Replace(".docx", ".doc");
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


        public ActionResult SaveRTFQuarterly(ProgressReport model, int RTFQId)
        {
            int QId = RTFQId;
            string result = "";
            result = objFuns.SaveRTFQuarterlyData(model, QId);
            return RedirectToAction("Index", new { filter = 3 });
        }

        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult viewDoc1(int id)
        {
            sess = (clsSession)Session["UserSessionClient"];
            if (sess == null)
            {
                return RedirectToAction("Index", new { filter = 1 });
            }
            else
            {
                byte[] fileData;
                string fileName;
                BiWeeklyRCPNewEntities dbobj = new BiWeeklyRCPNewEntities();


                var record = from objdoc in dbobj.ProgressRpt_Doc
                             where objdoc.DocId == id
                             select objdoc;
                fileData = (byte[])record.First().Data.ToArray();
                fileName = record.First().DocumentName;
                return new FileContentResult(fileData, fileName);
            }

        }

        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult viewDoc2(int id)
        {
            sess = (clsSession)Session["UserSessionClient"];
            if (sess == null)
            {
                return RedirectToAction("Index", new { filter = 2 });
            }
            else
            {
                byte[] fileData;
                string fileName;
                BiWeeklyRCPNewEntities dbobj = new BiWeeklyRCPNewEntities();


                var record = from objdoc in dbobj.ProgressRpt_Doc
                             where objdoc.DocId == id
                             select objdoc;
                fileData = (byte[])record.First().Data.ToArray();
                fileName = record.First().DocumentName;
                return new FileContentResult(fileData, fileName);
            }

        }

    }
}
