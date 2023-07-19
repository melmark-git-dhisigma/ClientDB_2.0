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
    public class VisitationController : Controller
    {
        //
        // GET: /Visitation/
        Other_Functions objFuns = new Other_Functions();
        public clsSession sess = null;
        public ActionResult Index()
        {
            sess = (clsSession)Session["UserSessionClient"];
            if (sess != null)
            {
                ViewBag.permission = objFuns.setPermission();
            }
            return View();
        }
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult Visitation(int page = 1, int pageSize = 5)
        {
           
            ContactSearch search = new ContactSearch();
            VisitationModel bindModel = new VisitationModel();
              Other_Functions objFuns = new Other_Functions();
             
              sess = (clsSession)Session["UserSessionClient"];
              if (sess != null)
              {
                  ViewBag.permission = objFuns.setPermission();
             
                  bindModel = VisitationModel.fillVisitations(page, pageSize);
              }

            return View(bindModel);
        }
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult AddVisitation(int id)
        {
            AddVisitaionModel model = new AddVisitaionModel();
            Other_Functions objFuns = new Other_Functions();
            sess = (clsSession)Session["UserSessionClient"];
            if (sess != null)
            {
                ViewBag.permission = objFuns.setPermission();
            }
                  if (id > 0)
                  {
                      model = objFuns.bindVisitation(id);
                  }
            
            model.EventTypeList = objFuns.getEventType();
            model.EventStatusList = objFuns.getEventStatus();
            model.FormList = model.GetFormList(sess.StudentId,sess.SchoolId);
            return View(model);
        
        }
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult SaveVisitation(AddVisitaionModel model)
        {
            string result = "";
             
                  result = objFuns.SaveVisitationData(model);
              
            if (result == "No Client Selected")
            {
                TempData["notice"] = "No Client Selected";
            }
            return RedirectToAction("Visitation");
                 
        }
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult DeleteVisitationDetails(int id)
        {
            sess = (clsSession)Session["UserSessionClient"];
             
                  objFuns.deleteVisitation(sess.StudentId, id);
              
            return RedirectToAction("Visitation");
            
        }

    }
}
