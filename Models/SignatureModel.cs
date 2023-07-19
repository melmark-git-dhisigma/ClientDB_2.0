using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ClientDB.DbModel;
using ClientDB.Models;
using ClientDB.AppFunctions;
using System.Data.Entity.Validation;
using System.Diagnostics;

using System.Web.Mvc;

namespace ClientDB.Models
{
    public class SignatureModel
    {
        GlobalData MetaData = new GlobalData();
        public static clsSession sess = null;
        public virtual string Searchtext { get; set; }
        public virtual int moduleid { get; set; }
        public virtual string modulename { get; set; }
        public virtual PagingModel pageModel { get; set; }
        public virtual IList<GridDocument> listDocuments { get; set; }
        public virtual IEnumerable<SelectListItem> DocumentModuleList { get; set; }
        public virtual int? DocumentModule { get; set; }
        public virtual IList<GridDocument> FilterDocuments { get; set; }
        public static BiWeeklyRCPNewEntities RPCobj = new BiWeeklyRCPNewEntities();

        public static SignatureModel fillDocuments(int page, int pageSize)
        {

            sess = (clsSession)HttpContext.Current.Session["UserSessionClient"];
            SignatureModel listModel = new SignatureModel();
            listModel.pageModel.CurrentPageIndex = page;
            listModel.pageModel.PageSize = pageSize;
            IList<GridDocument> retunmodel = new List<GridDocument>();

            int userID = sess.LoginId;
            try
            {


                retunmodel = (from objDocuments in RPCobj.binaryFiles

                              //join objusr in RPCobj.Users on objDocuments.CreatedBy equals objusr.UserId

                              join objDocs in RPCobj.Documents on objDocuments.DocId equals objDocs.DocumentId
                              join objlookup in RPCobj.LookUps on objDocs.DocumentType equals objlookup.LookupId
                              where ((objDocuments.ModuleName == "IEP" && objDocuments.Varified == true) || (objDocuments.ModuleName != "IEP")) && objDocuments.StudentId == sess.StudentId && objDocuments.Active == true
                              select new GridDocument
                              {
                                  DocumentId = objDocuments.BinaryId,
                                  DocumentName = objDocuments.DocumentName,
                                  CreatedOn = objDocuments.CreatedOn,
                                  DocumentType = objlookup.LookupName,
                                  Other = objDocs.Other,
                                  OtherDocumentType=objDocuments.type,
                                  //UserType = objusr.UserLName + "," + objusr.UserFName,

                                  UserType = objDocs.UserType,
                                  CreatedBy = (int)objDocs.CreatedBy,
                              }).ToList();



                if (retunmodel != null)
                {
                    foreach (var item in retunmodel)
                    {
                        //if (item.CreatedBy != userID)
                        //{


                        //    var data = (from objusr in RPCobj.Parents

                        //                where (item.CreatedBy == objusr.ParentID)
                        //                select new GridDocument
                        //                {

                        //                    UserType = objusr.Lname + "," + objusr.Fname,
                        //                }).First();

                        //    item.UserType = data.UserType;
                        //}

                        //else
                        //{
                        //var data = (from objusr in RPCobj.Users
                        //            where (item.CreatedBy == UserID)
                        //            select new GridDocument
                        //            {

                        //                UserType = objusr.UserLName + "," + objusr.UserFName,
                        //            }).First();

                        //item.UserType = data.UserType;

                        if (item.UserType == "Staff")
                        {
                            var data = (from objusr in RPCobj.Users
                                        where (item.CreatedBy == objusr.UserId)
                                        select new GridDocument
                                        {

                                            UserType = objusr.UserLName + "," + objusr.UserFName,
                                        }).First();

                            item.UserType = data.UserType;
                        }
                        else
                        {
                            var data = (from objusr in RPCobj.Parents

                                        where (item.CreatedBy == objusr.ParentID)
                                        select new GridDocument
                                        {

                                            UserType = objusr.Lname + "," + objusr.Fname,
                                        }).First();

                            item.UserType = data.UserType;
                        }



                        //}

                    }
                }

            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
            }
            listModel.pageModel.TotalRecordCount = retunmodel.Count;
            retunmodel = retunmodel.OrderByDescending(objDocument => objDocument.DocumentId).Skip((page - 1) * pageSize).Take(pageSize).ToList();
            listModel.listDocuments = retunmodel;
            if (listModel.pageModel.PageSize > listModel.pageModel.TotalRecordCount) { listModel.pageModel.PageSize = listModel.pageModel.TotalRecordCount; }
            if (listModel.pageModel.TotalRecordCount == 0) { listModel.pageModel.CurrentPageIndex = 0; }

            return listModel;
        }




        public static SignatureModel filterDocuments(int page, int pageSize, int id)//2
        {

            sess = (clsSession)HttpContext.Current.Session["UserSessionClient"];
            SignatureModel listModel = new SignatureModel();
            listModel.pageModel.CurrentPageIndex = page;
            listModel.pageModel.PageSize = pageSize;
            listModel.DocumentModule = id;
            listModel.modulename = "All";
            if (id == 1)
                listModel.modulename = "Referal";
            else if (id == 2)
                listModel.modulename = "Client";
            else if (id == 3)
                listModel.modulename = "Parent";
            else if (id == 4)
                listModel.modulename = "BW";
            listModel.moduleid = id;

            int userID = sess.LoginId;
            IList<GridDocument> retunmodel = new List<GridDocument>();
            try
            {
                var user = RPCobj.Users.Where(objuserid => objuserid.UserId == userID).ToList();

                if (listModel.modulename != "Parent" && listModel.modulename != "BW" && listModel.modulename != "Client" && listModel.modulename != "All")
                {
                    //retunmodel = (from objDocuments in RPCobj.binaryFiles
                    //              join objusr in RPCobj.Users on objDocuments.CreatedBy equals objusr.UserId
                    //              join objDocs in RPCobj.Documents on objDocuments.DocId equals objDocs.DocumentId
                    //              join objlookup in RPCobj.LookUps on objDocs.DocumentType equals objlookup.LookupId
                    //              where (objDocuments.StudentId == sess.StudentId && objDocuments.type == listModel.modulename && objDocuments.Varified == true)
                    //              select new GridDocument
                    //              {
                    //                  DocumentId = objDocuments.BinaryId,
                    //                  DocumentName = objDocuments.DocumentName,
                    //                  CreatedOn = objDocuments.CreatedOn,
                    //                  DocumentType = objlookup.LookupName,
                    //                  Other = objDocs.Other,
                    //                  UserType = objusr.UserLName + "," + objusr.UserFName,
                    //              }).ToList();

                    retunmodel = (from objDocuments in RPCobj.binaryFiles
                                  join objusr in RPCobj.Users on objDocuments.CreatedBy equals objusr.UserId
                                  join objDocs in RPCobj.Documents on objDocuments.DocId equals objDocs.DocumentId
                                  join objlookup in RPCobj.LookUps on objDocs.DocumentType equals objlookup.LookupId
                                  where ((objDocuments.ModuleName == "IEP" && objDocuments.Varified == true) || (objDocuments.ModuleName != "IEP"))
                                           && objDocuments.StudentId == sess.StudentId && objDocuments.type == listModel.modulename && objDocuments.Active == true
                                  select new GridDocument
                                  {
                                      DocumentId = objDocuments.BinaryId,
                                      DocumentName = objDocuments.DocumentName,
                                      CreatedOn = objDocuments.CreatedOn,
                                      DocumentType = objlookup.LookupName,
                                      Other = objDocs.Other,
                                      // UserType = objusr.UserLName + "," + objusr.UserFName,
                                      CreatedBy = (int)objDocs.CreatedBy,
                                  }).ToList();

                   

                }
                else if (listModel.modulename == "BW")
                {
                    //retunmodel = (from objDocuments in RPCobj.binaryFiles
                    //              join objusr in RPCobj.Users on objDocuments.CreatedBy equals objusr.UserId
                    //              join objDocs in RPCobj.Documents on objDocuments.DocId equals objDocs.DocumentId
                    //              join objlookup in RPCobj.LookUps on objDocs.DocumentType equals objlookup.LookupId
                    //              where (objDocuments.StudentId == sess.StudentId && objDocuments.type == listModel.modulename && objDocuments.ModuleName == "IEP" && objDocuments.Varified == true)
                    //              select new GridDocument
                    //              {
                    //                  DocumentId = objDocuments.BinaryId,
                    //                  DocumentName = objDocuments.DocumentName,
                    //                  CreatedOn = objDocuments.CreatedOn,
                    //                  DocumentType = objDocuments.ModuleName,
                    //                  Other = objDocs.Other,
                    //                  _IsAllowParent = objDocuments.AllowParent,

                    //                  UserType = objusr.UserLName + "," + objusr.UserFName,
                    //              }).ToList();
                    retunmodel = (from objDocuments in RPCobj.binaryFiles
                                  //join objusr in RPCobj.Users on objDocuments.CreatedBy equals objusr.UserId
                                  join objDocs in RPCobj.Documents on objDocuments.DocId equals objDocs.DocumentId
                                  join objlookup in RPCobj.LookUps on objDocs.DocumentType equals objlookup.LookupId
                                  where ((objDocuments.ModuleName == "IEP" && objDocuments.Varified == true) || (objDocuments.ModuleName != "IEP"))
                                           && objDocuments.StudentId == sess.StudentId && objDocuments.type == listModel.modulename && objDocuments.Active == true
                                  select new GridDocument
                                  {
                                      DocumentId = objDocuments.BinaryId,
                                      DocumentName = objDocuments.DocumentName,
                                      CreatedOn = objDocuments.CreatedOn,
                                      DocumentType = objlookup.LookupName,
                                      Other = objDocs.Other,
                                      _IsAllowParent = objDocuments.AllowParent,
                                      // UserType = objusr.UserLName + "," + objusr.UserFName,
                                      CreatedBy = (int)objDocs.CreatedBy,
                                  }).ToList();


                }

                else if (listModel.modulename == "Client")
                {
                    //retunmodel = (from objDocuments in RPCobj.binaryFiles
                    //              join objusr in RPCobj.Users on objDocuments.CreatedBy equals objusr.UserId
                    //              join objDocs in RPCobj.Documents on objDocuments.DocId equals objDocs.DocumentId
                    //              join objlookup in RPCobj.LookUps on objDocs.DocumentType equals objlookup.LookupId
                    //              where (objDocuments.StudentId == sess.StudentId && objDocuments.type == listModel.modulename && objDocuments.Varified == true)
                    //              select new GridDocument
                    //              {
                    //                  DocumentId = objDocuments.BinaryId,
                    //                  DocumentName = objDocuments.DocumentName,
                    //                  CreatedOn = objDocuments.CreatedOn,
                    //                  DocumentType = objDocuments.ModuleName,
                    //                  Other = objDocs.Other,
                    //                  _IsAllowParent = objDocuments.AllowParent,

                    //                  UserType = objusr.UserLName + "," + objusr.UserFName,
                    //              }).ToList();

                    retunmodel = (from objDocuments in RPCobj.binaryFiles
                                  //join objusr in RPCobj.Users on objDocuments.CreatedBy equals objusr.UserId
                                  join objDocs in RPCobj.Documents on objDocuments.DocId equals objDocs.DocumentId
                                  join objlookup in RPCobj.LookUps on objDocs.DocumentType equals objlookup.LookupId
                                  where ((objDocuments.ModuleName == "IEP" && objDocuments.Varified == true) || (objDocuments.ModuleName != "IEP"))
                                           && objDocuments.StudentId == sess.StudentId && objDocuments.type == listModel.modulename && objDocuments.Active == true
                                  select new GridDocument
                                  {
                                      DocumentId = objDocuments.BinaryId,
                                      DocumentName = objDocuments.DocumentName,
                                      CreatedOn = objDocuments.CreatedOn,
                                      DocumentType = objlookup.LookupName,
                                      Other = objDocs.Other,
                                      _IsAllowParent = objDocuments.AllowParent,
                                      // UserType = objusr.UserLName + "," + objusr.UserFName,
                                      CreatedBy = (int)objDocs.CreatedBy,
                                  }).ToList();

                }

                else if (listModel.modulename == "Parent")
                {
                    //retunmodel = (from objDocuments in RPCobj.binaryFiles
                    //              join objusr in RPCobj.Parents on objDocuments.CreatedBy equals objusr.ParentID
                    //              join objDocs in RPCobj.Documents on objDocuments.DocId equals objDocs.DocumentId
                    //              join objlookup in RPCobj.LookUps on objDocs.DocumentType equals objlookup.LookupId
                    //              where (objDocuments.StudentId == sess.StudentId && objDocuments.type == listModel.modulename)
                    //              select new GridDocument
                    //              {
                    //                  DocumentId = objDocuments.BinaryId,
                    //                  DocumentName = objDocuments.DocumentName,
                    //                  CreatedOn = objDocuments.CreatedOn,
                    //                  DocumentType = objlookup.LookupName,
                    //                  Other = objDocs.Other,
                    //                  UserType = objusr.Lname + "," + objusr.Fname,
                    //              }).ToList();

                    retunmodel = (from objDocuments in RPCobj.binaryFiles
                                  //join objusr in RPCobj.Users on objDocuments.CreatedBy equals objusr.UserId
                                  join objDocs in RPCobj.Documents on objDocuments.DocId equals objDocs.DocumentId
                                  join objlookup in RPCobj.LookUps on objDocs.DocumentType equals objlookup.LookupId
                                  where ((objDocuments.ModuleName == "IEP" && objDocuments.Varified == true) || (objDocuments.ModuleName != "IEP"))
                                           && objDocuments.StudentId == sess.StudentId && objDocuments.type == listModel.modulename && objDocuments.Active == true
                                  select new GridDocument
                                  {
                                      DocumentId = objDocuments.BinaryId,
                                      DocumentName = objDocuments.DocumentName,
                                      CreatedOn = objDocuments.CreatedOn,
                                      DocumentType = objlookup.LookupName,
                                      Other = objDocs.Other,
                                      // UserType = objusr.UserLName + "," + objusr.UserFName,
                                      CreatedBy = (int)objDocs.CreatedBy,
                                  }).ToList();

                }
                else
                {
                    //retunmodel = (from objDocuments in RPCobj.binaryFiles
                    //              join objusr in RPCobj.Parents on objDocuments.CreatedBy equals objusr.ParentID
                    //              join objDocs in RPCobj.Documents on objDocuments.DocId equals objDocs.DocumentId
                    //              join objlookup in RPCobj.LookUps on objDocs.DocumentType equals objlookup.LookupId
                    //              where (objDocuments.StudentId == sess.StudentId && objDocuments.type == listModel.modulename)
                    //              select new GridDocument
                    //              {
                    //                  DocumentId = objDocuments.BinaryId,
                    //                  DocumentName = objDocuments.DocumentName,
                    //                  CreatedOn = objDocuments.CreatedOn,
                    //                  DocumentType = objlookup.LookupName,
                    //                  Other = objDocs.Other,
                    //                  UserType = objusr.Lname + "," + objusr.Fname,
                    //              }).ToList();

                    retunmodel = (from objDocuments in RPCobj.binaryFiles
                                  //join objusr in RPCobj.Users on objDocuments.CreatedBy equals objusr.UserId
                                  join objDocs in RPCobj.Documents on objDocuments.DocId equals objDocs.DocumentId
                                  join objlookup in RPCobj.LookUps on objDocs.DocumentType equals objlookup.LookupId
                                  where ((objDocuments.ModuleName == "IEP" && objDocuments.Varified == true) || (objDocuments.ModuleName != "IEP"))
                                           && objDocuments.StudentId == sess.StudentId && objDocuments.Active == true
                                  select new GridDocument
                                  {
                                      DocumentId = objDocuments.BinaryId,
                                      DocumentName = objDocuments.DocumentName,
                                      CreatedOn = objDocuments.CreatedOn,
                                      DocumentType = objlookup.LookupName,
                                      Other = objDocs.Other,
                                      // UserType = objusr.UserLName + "," + objusr.UserFName,
                                      CreatedBy = (int)objDocs.CreatedBy,
                                  }).ToList();

                }

                if (retunmodel != null)
                {
                    foreach (var item in retunmodel)
                    {
                        if (item.CreatedBy != userID)
                        {


                            var data = (from objusr in RPCobj.Parents

                                        where (item.CreatedBy == objusr.ParentID)
                                        select new GridDocument
                                        {

                                            UserType = objusr.Lname + "," + objusr.Fname,
                                        }).First();

                            item.UserType = data.UserType;
                        }

                        else
                        {
                            var data = (from objusr in RPCobj.Users
                                        where (item.CreatedBy == userID)
                                        select new GridDocument
                                        {

                                            UserType = objusr.UserLName + "," + objusr.UserFName,
                                        }).First();

                            item.UserType = data.UserType;



                        }

                    }
                }

            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
            }
            listModel.pageModel.TotalRecordCount = retunmodel.Count;
            retunmodel = retunmodel.OrderByDescending(objDocument => objDocument.DocumentId).Skip((page - 1) * pageSize).Take(pageSize).ToList();
            listModel.FilterDocuments = retunmodel;
            if (listModel.pageModel.PageSize > listModel.pageModel.TotalRecordCount) { listModel.pageModel.PageSize = listModel.pageModel.TotalRecordCount; }
            if (listModel.pageModel.TotalRecordCount == 0) { listModel.pageModel.CurrentPageIndex = 0; }

            return listModel;
        }

        public static SignatureModel filterDocuments_vist(int page, int pageSize, int id)//2
        {

            sess = (clsSession)HttpContext.Current.Session["UserSessionClient"];
            SignatureModel listModel = new SignatureModel();
            listModel.pageModel.CurrentPageIndex = page;
            listModel.pageModel.PageSize = pageSize;
            listModel.DocumentModule = id;
            listModel.modulename = "Parent";
            if (id == 1)
                listModel.modulename = "Referal";
            else if (id == 2)
                listModel.modulename = "Client";
            else if (id == 3)
                listModel.modulename = "Parent";
            else if (id == 4)
                listModel.modulename = "BW";
            listModel.moduleid = id;

            int userID = sess.LoginId;
            IList<GridDocument> retunmodel = new List<GridDocument>();
            try
            {
                var user = RPCobj.Users.Where(objuserid => objuserid.UserId == userID).ToList();

                if (listModel.modulename != "Parent" && listModel.modulename != "BW" && listModel.modulename != "Client")
                {
                    retunmodel = (from objDocuments in RPCobj.binaryFiles
                                  join objusr in RPCobj.Users on objDocuments.CreatedBy equals objusr.UserId
                                  join objDocs in RPCobj.Documents on objDocuments.DocId equals objDocs.DocumentId
                                  join objlookup in RPCobj.LookUps on objDocs.DocumentType equals objlookup.LookupId
                                  where (objDocuments.StudentId == sess.StudentId && objDocuments.type == listModel.modulename && objDocuments.Varified == true)
                                  select new GridDocument
                                  {
                                      DocumentId = objDocuments.BinaryId,
                                      DocumentName = objDocuments.DocumentName,
                                      CreatedOn = objDocuments.CreatedOn,
                                      DocumentType = objlookup.LookupName,
                                      Other = objDocs.Other,
                                      UserType = objusr.UserLName + "," + objusr.UserFName,
                                  }).ToList();

                }
                else if (listModel.modulename == "BW")
                {
                    retunmodel = (from objDocuments in RPCobj.binaryFiles
                                  join objusr in RPCobj.Users on objDocuments.CreatedBy equals objusr.UserId
                                  join objDocs in RPCobj.Documents on objDocuments.DocId equals objDocs.DocumentId
                                  join objlookup in RPCobj.LookUps on objDocs.DocumentType equals objlookup.LookupId
                                  where (objDocuments.StudentId == sess.StudentId && objDocuments.type == listModel.modulename && objDocuments.ModuleName == "IEP" && objDocuments.Varified == true)
                                  select new GridDocument
                                  {
                                      DocumentId = objDocuments.BinaryId,
                                      DocumentName = objDocuments.DocumentName,
                                      CreatedOn = objDocuments.CreatedOn,
                                      DocumentType = objDocuments.ModuleName,
                                      Other = objDocs.Other,
                                      _IsAllowParent = objDocuments.AllowParent,

                                      UserType = objusr.UserLName + "," + objusr.UserFName,
                                  }).ToList();

                }

                else if (listModel.modulename == "Client")
                {
                    retunmodel = (from objDocuments in RPCobj.binaryFiles
                                  join objusr in RPCobj.Users on objDocuments.CreatedBy equals objusr.UserId
                                  join objDocs in RPCobj.Documents on objDocuments.DocId equals objDocs.DocumentId
                                  join objlookup in RPCobj.LookUps on objDocs.DocumentType equals objlookup.LookupId
                                  where (objDocuments.StudentId == sess.StudentId && objDocuments.type == listModel.modulename && objDocuments.Varified == true)
                                  select new GridDocument
                                  {
                                      DocumentId = objDocuments.BinaryId,
                                      DocumentName = objDocuments.DocumentName,
                                      CreatedOn = objDocuments.CreatedOn,
                                      DocumentType = objDocuments.ModuleName,
                                      Other = objDocs.Other,
                                      _IsAllowParent = objDocuments.AllowParent,

                                      UserType = objusr.UserLName + "," + objusr.UserFName,
                                  }).ToList();

                }

                else
                {
                    retunmodel = (from objDocuments in RPCobj.binaryFiles
                                  join objusr in RPCobj.Parents on objDocuments.CreatedBy equals objusr.ParentID
                                  join objDocs in RPCobj.Documents on objDocuments.DocId equals objDocs.DocumentId
                                  join objlookup in RPCobj.LookUps on objDocs.DocumentType equals objlookup.LookupId
                                  where (objDocuments.StudentId == sess.StudentId && objDocuments.type == listModel.modulename)
                                  select new GridDocument
                                  {
                                      DocumentId = objDocuments.BinaryId,
                                      DocumentName = objDocuments.DocumentName,
                                      CreatedOn = objDocuments.CreatedOn,
                                      DocumentType = objlookup.LookupName,
                                      Other = objDocs.Other,
                                      UserType = objusr.Lname + "," + objusr.Fname,
                                  }).ToList();

                }



            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
            }
            listModel.pageModel.TotalRecordCount = retunmodel.Count;
            retunmodel = retunmodel.OrderByDescending(objDocument => objDocument.DocumentId).Skip((page - 1) * pageSize).Take(pageSize).ToList();
            listModel.FilterDocuments = retunmodel;
            if (listModel.pageModel.PageSize > listModel.pageModel.TotalRecordCount) { listModel.pageModel.PageSize = listModel.pageModel.TotalRecordCount; }
            if (listModel.pageModel.TotalRecordCount == 0) { listModel.pageModel.CurrentPageIndex = 0; }

            return listModel;
        }


        public SignatureModel()
        {
            listDocuments = new List<GridDocument>();
            FilterDocuments = new List<GridDocument>();

            pageModel = new PagingModel();
        }
    }
    public class GridDocument
    {
        public virtual int DocumentId { get; set; }
        public virtual string DocumentName { get; set; }
        public virtual DateTime? CreatedOn { get; set; }
        public virtual string DocumentType { get; set; }
        public virtual int CreatedBy { get; set; }
        public virtual string Createdon { get; set; }
        public virtual string UserType { get; set; }
        public virtual string Other { get; set; }
        public virtual string OtherDocumentType { get; set; }
        public bool? _IsAllowParent;
        public bool IsAllowParent
        {
            get
            {
                if (_IsAllowParent == null)
                {
                    return false;
                }
                else
                {
                    return (bool)_IsAllowParent;
                }
            }
            set
            {
                _IsAllowParent = value;
            }
        }

    }
}