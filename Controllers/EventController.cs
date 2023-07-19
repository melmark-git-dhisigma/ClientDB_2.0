using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ClientDB.DbModel;
using ClientDB.Models;
using ClientDB.AppFunctions;


namespace ClientDB.Controllers
{
    public class EventController : Controller
    {
        //
        // GET: /Event/
        Other_Functions objFuns = new Other_Functions();
        public clsSession sess = null;
        public ActionResult Index()
        {
            sess = (clsSession)Session["UserSessionClient"];
            if (sess != null)
            {
                ViewBag.permission = (objFuns.setClientPermission("Event Logs") == 2) ? "true" : "false";
            }
            return View();
        }
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult EventsList(int page = 1, int pageSize = 5)
        {
            ContactSearch search = new ContactSearch();
            EventsModel bindModel = new EventsModel();
          
            sess = (clsSession)Session["UserSessionClient"];
            if (sess != null)
            {
                ViewBag.permission = (objFuns.setClientPermission("Event Logs") == 2) ? "true" : "false";
                bindModel = EventsModel.fillEvents(page, pageSize);
            }
            return View(bindModel);
        }
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult AddEvents(int id)
        {
            AddEventModel model = new AddEventModel();
            Other_Functions objFuns = new Other_Functions();
            sess = (clsSession)Session["UserSessionClient"];
            if (sess != null)
            {
                ViewBag.permission = (objFuns.setClientPermission("Event Logs") == 2) ? "true" : "false";
            }

                if (id > 0)
                {
                    model = objFuns.bindEvents(id);
                }
                else
                {
                    // setting default values
                    model.UserName = sess.UserName;
                    model.EventDate = DateTime.Now.ToString("MM/dd/yyyy").Replace("-","/");
                }



                model.EventTypeList = model.GetEventTypeList(sess.SchoolId);
                model.Contactlist = model.GetContactList(sess.StudentId);
            model.EventStatusList = objFuns.getEventStatus();
            return View(model);
        }
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult SaveEvents(AddEventModel model)
        {
            string result = "";
            
                 result = objFuns.SaveEventData(model);
             
            if (result == "No Client Selected")
            {
                TempData["notice"] = "No Client Selected";
            }
            return RedirectToAction("EventsList");
        }
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult DeleteEvents(int id)
        {
            sess = (clsSession)Session["UserSessionClient"];
            objFuns.deleteEvents(sess.StudentId, id);
            return RedirectToAction("EventsList");

        }
    }
}
