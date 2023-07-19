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
//using Microsoft.Office.Interop.Word;
using System.Text.RegularExpressions;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;


namespace ClientDB.Controllers
{
    public class FormsController : Controller
    {
        //
        // GET: /Forms/

        Other_Functions objFuns = new Other_Functions();
        public string[] columns;
        public string[] placeHolders;

        public string[] columnsCheck;
        public string[] placeHoldersCheck;


        public string[] columnsP4;
        public string[] placeHoldersP4;

        public int checkCount = 0;
        public int P4TotalCount = 1;
        public string strQuery = "";

        clsSession sess = null;
        public ActionResult Index()
        {
            sess = (clsSession)Session["UserSessionClient"];
            if (sess != null)
            {
                ViewBag.permission = (objFuns.setClientPermission("Document Tray") == 2) ? "true" : "false";
            }
            return View();
        }
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult ListDocuments(int page = 1, int pageSize = 5)
        {


            ContactSearch search = new ContactSearch();
            SignatureModel bindModel = new SignatureModel();

            sess = (clsSession)Session["UserSessionClient"];
            if (sess != null)
            {
                ViewBag.permission = (objFuns.setClientPermission("Document Tray") == 2) ? "true" : "false";
                bindModel = SignatureModel.fillDocuments(page, pageSize);
                IList<SelectListItem> x = new List<SelectListItem>();
                x.Add(new SelectListItem { Text = "--Select--", Value = "0" });
                x.Add(new SelectListItem { Text = "Referral", Value = "1" });
                x.Add(new SelectListItem { Text = "Client", Value = "2" });
                x.Add(new SelectListItem { Text = "Parent", Value = "3" });
                x.Add(new SelectListItem { Text = "Bi-Weekly", Value = "4" });
                bindModel.DocumentModuleList = x;
            }
            return View(bindModel);
        }

        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]

        public ActionResult FilterDocuments(int id, int page = 1, int pageSize = 5)
        {
            AddDocumentModel model = new AddDocumentModel();
            ContactSearch search = new ContactSearch();
            SignatureModel bindModel = new SignatureModel();
            //int page = 1, pagesize = 5;
            sess = (clsSession)Session["UserSessionClient"];
            if (sess != null)
            {
                ViewBag.permission = (objFuns.setClientPermission("Document Tray") == 2) ? "true" : "false";
                bindModel = SignatureModel.filterDocuments(page, pageSize, id);
                IList<SelectListItem> x = new List<SelectListItem>();
                x.Add(new SelectListItem { Text = "--Select--", Value = "0" });
                x.Add(new SelectListItem { Text = "Referral", Value = "1" });
                x.Add(new SelectListItem { Text = "Client", Value = "2" });
                x.Add(new SelectListItem { Text = "Parent", Value = "3" });
                x.Add(new SelectListItem { Text = "Bi-Weekly", Value = "4" });
                bindModel.DocumentModuleList = x;
            }
            return View("FilterDocuments", bindModel);
        }

        [HttpPost]
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult SaveForms(AddDocumentModel model, HttpPostedFileBase profilePicture)
        {
            sess = (clsSession)Session["UserSessionClient"];
            string result = "";

            if (sess != null)
            {
                ViewBag.permission = (objFuns.setClientPermission("Document Tray") == 2) ? "true" : "false";
            }
            result = objFuns.SaveForms(model, profilePicture);

            if (result == "No Client Selected")
            {
                TempData["notice"] = "No Client Selected";
            }
            return RedirectToAction("ListDocuments");
        }
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult DeleteDocuments(int id)
        {
            sess = (clsSession)Session["UserSessionClient"];

            objFuns.deleteDocument(sess.StudentId, id);

            return RedirectToAction("ListDocuments");

        }
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult UploadForms(int id)
        {
            AddDocumentModel model = new AddDocumentModel();
            ViewBag.permission = (objFuns.setClientPermission("Document Tray") == 2) ? "true" : "false";
            model.DocumentTypeList = objFuns.getDocumentType();
            return View(model);
        }


        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult viewDoc(int id)
        {


            sess = (clsSession)Session["UserSessionClient"];
            if (sess == null)
            {
                return RedirectToAction("Client", "ListClients");
            }
            else
            {
                byte[] fileData;
                string fileName;
                BiWeeklyRCPNewEntities dbobj = new BiWeeklyRCPNewEntities();


                var record = from objdoc in dbobj.binaryFiles
                             where objdoc.BinaryId == id
                             select objdoc;
                fileData = (byte[])record.First().Data.ToArray();
                fileName = record.First().ContentType;
                return new FileContentResult(fileData, fileName);


                //result = objFuns.ViewDocument(id);

                //string[] Filename = result.Split('/');
                //filePath = Filename[Filename.Length - 1];
                //if (result.Contains(".jpg") || result.Contains(".jpeg") || result.Contains(".png"))
                //    return File(result, System.Net.Mime.MediaTypeNames.Image.Jpeg);
                //else if (result.Contains(".gif"))
                //{
                //    return File(result, System.Net.Mime.MediaTypeNames.Image.Gif);
                //}
                //else if (result.Contains(".tiff"))
                //{
                //    return File(result, System.Net.Mime.MediaTypeNames.Image.Tiff);
                //}
                //else if (result.Contains(".pdf"))
                //{
                //    return File(result, System.Net.Mime.MediaTypeNames.Application.Pdf);
                //}
                //else if (result.Contains(".doc"))
                //{
                //    return File(result, System.Net.Mime.MediaTypeNames.Application.Rtf, "Document.doc");

                //}

                //return File(result, Server.UrlEncode(result));
            }

        }

        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public string AllInOne()
        {
            sess = (clsSession)Session["UserSessionClient"];

            string Path = "";
            string NewPath = "";
            try
            {

                CreateQuery("NE", "XMLS\\Template.xml");

                objFuns.getBindData(out columns, sess.StudentId, sess.SchoolId);
                if (columns != null)
                {
                    Path = AppDomain.CurrentDomain.BaseDirectory + "Templates\\Release of Information Form Dummy Template.docx";
                    // Path = Server.MapPath("~\\Administration\\IEPTemplates\\IEP1.docx");
                    NewPath = CopyTemplate(Path, sess.StudentId.ToString() + "-Release of Information Form.docx");
                    if (NewPath != "")
                    {
                        SearchAndReplace(NewPath);
                    }
                }
            }
            catch
            {

            }
            return "";
        }

        private void CreateQuery(string StateName, string Path)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(AppDomain.CurrentDomain.BaseDirectory + (Path));

            XmlNodeList xmlList = null;
            xmlList = xmlDoc.GetElementsByTagName("State");
            checkCount = 0;
            foreach (XmlNode st in xmlList)
            {
                if (st.Attributes["Name"].Value == StateName)
                {
                    XmlNodeList xmlListColumns = null;
                    xmlListColumns = st.ChildNodes.Item(0).ChildNodes;

                    columns = new string[xmlListColumns.Count];
                    placeHolders = new string[xmlListColumns.Count];




                    int i = 0, j = 0;
                    foreach (XmlNode stMs in xmlListColumns)
                    {
                        columns[i] = stMs.Attributes["Column"].Value;
                        i++;
                    }
                    foreach (XmlNode stMs in xmlListColumns)
                    {
                        placeHolders[j] = stMs.Attributes["PlaceHolder"].Value;

                        if (stMs.Attributes["PlaceHolder"].Value == "abcdefgh")
                        {
                            checkCount++;
                        }
                        j++;
                    }

                }
            }

        }


        private string CopyTemplate(string oldPath, string PageNo)
        {
            try
            {
                string Time = DateTime.Now.TimeOfDay.ToString();
                string[] ar = Time.Split('.');
                Time = ar[0];
                Time = Time.Replace(":", "-");
                string Datet = DateTime.Today.Day.ToString() + "-" + DateTime.Today.Month + "-" + DateTime.Today.Year.ToString() + "-" + Time;

                string path = AppDomain.CurrentDomain.BaseDirectory + "\\Temp\\";

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                string newpath = path;
                string newFileName = PageNo;
                FileInfo f1 = new FileInfo(oldPath);
                if (f1.Exists)
                {
                    if (!Directory.Exists(newpath))
                    {
                        Directory.CreateDirectory(newpath);
                    }
                    FileInfo f2 = new FileInfo(newpath + PageNo);
                    if (!f2.Exists)
                    {
                        f1.CopyTo(string.Format("{0}{1}", newpath, newFileName));
                    }

                }
                return newpath + newFileName;
            }
            catch (Exception Ex)
            {

                return "";
            }
        }

        public void SearchAndReplace(string document)
        {
            int m = 0;

            try
            {
                WordprocessingDocument wordDoc = WordprocessingDocument.Open(document, true);


                string docText = null;
                using (StreamReader sr = new StreamReader(wordDoc.MainDocumentPart.GetStream()))
                {
                    docText = sr.ReadToEnd();
                }
                string col = "";
                string plc = "";

                columnsCheck = new string[checkCount];


                for (int i = 0; i < columns.Length; i++)
                {
                    plc = placeHolders[i].ToString().Trim();
                    col = columns[i].ToString().Trim();

                    if (plc == "abcdefgh")
                    {
                        columnsCheck[m] = col;
                        m++;
                    }
                    else
                    {
                        Regex regexText = new Regex(plc);
                        docText = regexText.Replace(docText, col);
                    }


                }
                using (StreamWriter sw = new StreamWriter(wordDoc.MainDocumentPart.GetStream(FileMode.Create)))
                {
                    sw.Write(docText);
                }
                wordDoc.Close();
            }
            catch
            {

            }


        }


        public JsonResult otherDocumentTypes(string term)
        {
            BiWeeklyRCPNewEntities RPCobj = new BiWeeklyRCPNewEntities();
            sess = (clsSession)Session["UserSessionClient"];
            SignatureModel listModel = new SignatureModel();
            IList<GridDocument> retunmodel = new List<GridDocument>();


            retunmodel = (from objDocuments in RPCobj.Documents
                          where objDocuments.Other != null
                          select new GridDocument
                          {
                              OtherDocumentType = objDocuments.Other,
                          }).Distinct().ToList();



            var result = (from r in retunmodel
                          where r.OtherDocumentType.ToLower().Contains(term.ToLower())
                          select new { r.OtherDocumentType }).Distinct();



            return Json(result, JsonRequestBehavior.AllowGet);


        }

        public string AllowParent(int docId)
        {
            try
            {
                BiWeeklyRCPNewEntities RPCobj = new BiWeeklyRCPNewEntities();
                sess = (clsSession)Session["UserSessionClient"];
                SignatureModel listModel = new SignatureModel();
                IList<GridDocument> retunmodel = new List<GridDocument>();
                binaryFile objBinary = new binaryFile();

                objBinary = RPCobj.binaryFiles.Where(x => x.BinaryId == docId).FirstOrDefault();


                if (objBinary != null)
                {


                    if (objBinary.AllowParent == true)
                        objBinary.AllowParent = false;
                    else
                        objBinary.AllowParent = true;
                    RPCobj.SaveChanges();
                }
                return "Sucess";
            }
            catch (Exception)
            {

                return "";

            }



        }

        public JsonResult searchParentFirstName(string term)
        {
            BiWeeklyRCPNewEntities RPCobj = new BiWeeklyRCPNewEntities();
            sess = (clsSession)Session["UserSessionClient"];
            IList<contactFirstName> retunmodel = new List<contactFirstName>();


            retunmodel = (from objDocuments in RPCobj.Parents
                          where objDocuments.Username != null
                          select new contactFirstName
                          {
                              username = objDocuments.Username,
                          }).Distinct().ToList();



            var result = (from r in retunmodel
                          where r.username.ToLower() == (term.ToLower())
                          select new { r.username }).Distinct();


            var resultcount=(from obj in result
            select new contactFirstName
                          {
                              Usercount=result.Count(),
                          }).Distinct().ToList();


            return Json(resultcount, JsonRequestBehavior.AllowGet);


        }

    }
}
