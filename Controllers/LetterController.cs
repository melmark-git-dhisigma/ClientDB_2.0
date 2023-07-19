using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ClientDB.DbModel;
using ClientDB.Models;
using ClientDB.AppFunctions;
using System.Net;
using System.Web.ClientServices;
using System.Xml;
using System.IO;

namespace ClientDB.Controllers
{
    public class LetterController : Controller
    {
        Other_Functions objFuns = new Other_Functions();
        clsSession sess = null;
        public ActionResult Index()
        {
            sess = (clsSession)Session["UserSessionClient"];
            if (sess != null)
            {
                ViewBag.permission = (objFuns.setClientPermission("Letter Tray") == 2) ? "true" : "false";
            }
            return View();
        }
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult AllLetter(int page = 1, int pageSize = 5)
        {
            LetterGenerationViewModel LetterModel = new LetterGenerationViewModel();
            LetterModel = LetterGenerationViewModel.getLetterAll(page, pageSize);
            sess = (clsSession)Session["UserSessionClient"];
            if (sess != null)
            {
                LetterModel = LetterGenerationViewModel.getLetterAll(page, pageSize);
            }
            return View("Letter", LetterModel);
        }
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult LetterTrayView(int LetterTrayId)
        {
            BiWeeklyRCPNewEntities objData = new BiWeeklyRCPNewEntities();
            LetterTrayViewModel LetterTray = new LetterTrayViewModel();

            var LetterItems = (from x in objData.ref_LetterTrayValues
                               join z in objData.LetterEngines on x.LetterId equals z.LetterEngineId
                               where (x.LetterTrayId == LetterTrayId)
                               select new LetterTrayViewModel
                               {
                                   LetterItem = x.TrayValue,
                                   LetterName = z.LetterEngineName

                               }).ToList();

            LetterTray.LetterList = LetterItems;
            return View("LetterTrayView", LetterTray);

        }


    }
}
