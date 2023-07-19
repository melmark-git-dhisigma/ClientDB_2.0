using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ClientDB.Models;
using System.Data.SqlClient;
using System.Data;
using ClientDB.AppFunctions;
using ClientDB.DbModel;



namespace ClientDB.Controllers
{
    public class ClientController : Controller
    {
        //
        // GET: /Client/
        public clsSession sess = null;
        string session = "";
        //[OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult Index(int Param = 0)
        {
            //// Static Session [Start]
            //Session["Values"] = "1#1#das"; //For NE = "1#1#das" For PA = "2#2#das"
            //setSession();
            //setClientPermissions();
            //string schoolType = System.Web.Configuration.WebConfigurationManager.AppSettings["Server"].ToString();
            //if (schoolType == "NE") Session["PageName"] = "Melmark New England";
            //else
            //    Session["PageName"] = "Melmark Pennsylvania";
            //return RedirectToAction("ListClients");
            //// Static Session [End]

            // Dynamic Session [Start]
            try
            {
                if (Session["Values"] != null)
                {

                    setSession();
                    setClientPermissions();
                    sess = (clsSession)Session["UserSessionClient"];
                    string schoolType = System.Web.Configuration.WebConfigurationManager.AppSettings["Server"].ToString();
                    if (schoolType == "NE") Session["PageName"] = "Melmark New England";
                    else
                        Session["PageName"] = "Melmark Pennsylvania";
                    return RedirectToAction("ListClients");
                }
                else
                {
                    return RedirectToAction("Logout");
                }
            }
            catch
            {
                return RedirectToAction("Logout");
            }
            // Dynamic Session [End]
        }
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult ListClients(string Name, string argument = "*", bool bSort = false, string Data = "0*k", bool orderBy = true, bool searchBy = true, int activeClient=1)
        {
            ClsErrorLog error = new ClsErrorLog();
            //setSession();
            ClientModel model = new ClientModel();
            ClientSearch search = new ClientSearch();
            IList<ClientModel> bindObj = new List<ClientModel>();
            sess = (clsSession)Session["UserSessionClient"];


            if (argument == "$")
            {
                argument = "";
            }
            string searchArgs = null;
            string sortArgs = null;
            if ((argument != "*" && argument != "Date of Birth" && argument != "AdmissionDate") && argument.Contains("$") && searchArgs != "ClientID" && searchArgs != "FirstName" && searchArgs != "LastName")
            {
                string[] datasplit = argument.Split('$');
                searchArgs = datasplit[0];
                sortArgs = datasplit[1];
            }
            else
            {
                searchArgs = argument;
                sortArgs = argument;
            }

            search.SearchArgument = argument;
            ViewBag.Sort = argument;
            search.OrderBy = orderBy; //order by asc or desc
            search.SearchBy = searchBy; //search by name or client id'
            search.activeClient = activeClient;
            ViewBag.SearchByStatus = searchBy;
            ViewBag.OrderByStatus = orderBy;
            ViewBag.activeClientstatus = activeClient;
            search.SortStatus = bSort;
            search.PagingArgument = Data;
            if (ViewBag == null)
                error.WriteToLog("View Bag Is Null");
            if (sess == null)
                error.WriteToLog("sess is Null");
            ViewBag.schoolId = sess.SchoolId;
            if (Name != null)
            {
                string[] SearchArgs = Name.Split('_');
                ViewBag.Sort = SearchArgs[0];
            }
            

            if (Name == null && argument != "*")
            {
                if (argument == "AdmissionDate")
                {
                    ViewBag.Sort = "1";

                }
                else if (argument == "Date of Birth")
                {
                    ViewBag.Sort = "2";
                }
                else if (argument == "ClientID")
                {
                    ViewBag.Sort = "3";
                }
                else if (argument == "FirstName")
                {
                    ViewBag.Sort = "4";
                }
                else if (argument == "LastName")
                {
                    ViewBag.Sort = "5";
                }
            }



            ViewBag.curval = "";
            ViewBag.flage = "";
            ViewBag.SearchArg = "";
            ViewBag.itemCount = 0;
            ViewBag.sessname = session;
            ViewBag.Usename = sess.UserName;
            if ((bSort == false) && (argument != "*"))
            {
                //OC
                //ViewBag.SearchArg = argument;   

                //MC
                ViewBag.SearchArg = searchArgs;

            }

            //OC
            //ViewBag.SortArg = argument;

            //MC
            //if (searchArgs != "*" && searchArgs!="")
            if (searchArgs != "*" && searchArgs != "AdmissionDate" && searchArgs != "Date of Birth" && searchArgs != "ClientID" && searchArgs != "FirstName" && searchArgs != "LastName")
            {
                ViewBag.SearchArg = searchArgs;
            }
            else
            {
                ViewBag.SearchArg = "";
            }

            Other_Functions objFuns = new Other_Functions();
            if (sess != null)
            {
                ViewBag.permission = (objFuns.setClientPermission("General Client") == 2) ? "true" : "false";
                bindObj = ClientModel.fillCLients(search, sess);
            }
            ViewBag.flage = search.flag;
            ViewBag.curval = search.perPage;
            ViewBag.itemCount = search.itemCount;

            return View(bindObj);
        }


        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult GenerateProgressReport(int studentID = 0, int iepID = 0, int schootID = 0)
        {

            BiweeklyIntegrationModel bwModel = new BiweeklyIntegrationModel();
            //SchoolID, StudentID, iepID
            Session["studentId"] = studentID;
            Session["IEPID"] = iepID;
            Session["studentId"] = schootID;

            return RedirectToAction("ProgressRpt", "Progress");
        }

        private void setClientPermissions()
        {
            clsSession session = null;
            BiWeeklyRCPNewEntities Objdata = new BiWeeklyRCPNewEntities();
            session = (clsSession)Session["UserSessionClient"];

            //select o.ObjectName,rgp.ReadInd,rgp.WriteInd from UserRoleGroup as urg 
            //Join RoleGroupPerm as rgp on urg.RoleGroupId = rgp.RoleGroupId
            //Join [Object] as o on rgp.ObjectId = o.ObjectId
            //where urg.UserId = 1004 and o.ParntObjectId = (select ObjectId from [Object] where ObjectName = 'clients')

            var clientObjId = Objdata.Objects.Where(x => x.ObjectName == "Client").SingleOrDefault();

            List<ClientPermissions> permissionList = (from urg in Objdata.UserRoleGroups
                                                      join rgp in Objdata.RoleGroupPerms on urg.RoleGroupId equals rgp.RoleGroupId
                                                      join o in Objdata.Objects on rgp.ObjectId equals o.ObjectId
                                                      where urg.UserId == session.LoginId && o.ParntObjectId == clientObjId.ObjectId
                                                      select new ClientPermissions
                                                      {
                                                          objectName = o.ObjectName,
                                                          ReadInd = rgp.ReadInd,
                                                          WriteInd = rgp.WriteInd

                                                      }).ToList();



            Session["ClientPermissions"] = permissionList;
        }


        private void setSession()
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

        public ActionResult Logout()
        {
            Session["UserSessionClient"] = null;
            Session.RemoveAll();
            Session.Abandon();
            return View();
        }

        //private void setSession()
        //{
        //    string Values = "";

        //    clsSession sess = new clsSession();

        //    Values = Session["Values"].ToString();
        //    string[] arValues = Values.Split('#');
        //    sess.SchoolId = Convert.ToInt16(arValues[0]);
        //    sess.LoginId = Convert.ToInt16(arValues[1]);
        //    sess.UserName = Convert.ToString(arValues[2]);


        //    Session["UserSessionClient"] = sess;
        //    sess = (clsSession)Session["UserSessionClient"];
        //}


        //private void setSession()
        //{
        //    clsSession session = new clsSession();

        //    BiWeeklyRCPNewEntities Objdata = new BiWeeklyRCPNewEntities();

        //    if (Session["Values"] != null)
        //    {
        //        string Values = Session["Values"].ToString();
        //        string[] arValues = Values.Split('#');
        //        session.LoginId = Convert.ToInt16(arValues[1]);

        //        if (session.LoginId != 0)              //set the remaining session used in the Referral DB
        //        {
        //            var Role = (from Objrole in Objdata.Roles
        //                        join objrgp in Objdata.RoleGroups on Objrole.RoleId equals objrgp.RoleId
        //                        select new
        //                        {
        //                            RoleId = Objrole.RoleId,
        //                            Roledesc = Objrole.RoleDesc,
        //                            SchoolId = Objrole.SchoolId
        //                        }).ToList();
        //            var Usr = (from Objrole in Role
        //                       from Objusr in Objdata.Users
        //                       where Objusr.UserId == session.LoginId
        //                       select new
        //                       {
        //                           Objrole.RoleId,
        //                           Objrole.Roledesc,
        //                           Objusr.SchoolId,
        //                           Objusr.UserId,
        //                           Objusr.UserFName,
        //                           Objusr.UserLName,
        //                           Objusr.Gender
        //                       }).ToList();

        //            if (Usr == null) return;
        //            if (Usr.Count() > 0)
        //            {

        //                session.IsLogin = true;
        //                session.LoginTime = (DateTime.Now.ToShortTimeString()).ToString();
        //                session.SchoolId = Convert.ToInt32(Usr[0].SchoolId);
        //                session.LoginId = Convert.ToInt32(Usr[0].UserId);
        //                session.UserName = Convert.ToString(Usr[0].UserLName + "," + Usr[0].UserFName);
        //                session.RoleId = Convert.ToInt32(Usr[0].RoleId);
        //                session.Gender = Convert.ToString(Usr[0].Gender);
        //                session.RoleName = Convert.ToString(Usr[0].Roledesc);
        //                session.SessionID = Session.SessionID.ToString();
        //                //  session.ReferralId = 0;
        //            }
        //        }

        //        Session["UserSessionClient"] = session;
        //        session = (clsSession)Session["UserSessionClient"];
        //    }

        //}



    }
}
