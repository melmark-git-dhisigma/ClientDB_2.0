using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ClientDB.DbModel;
using ClientDB.Models;
using ClientDB.AppFunctions;


namespace ClientDB.Models
{
    public class PlacementModel
    {
        GlobalData MetaData = new GlobalData();
        public static clsSession sess = null;
        public virtual PagingModel pageModel { get; set; }
        public virtual string Searchtext { get; set; }
        public virtual IList<GridListPlacement> listPlacement { get; set; }
        public static BiWeeklyRCPNewEntities RPCobj = new BiWeeklyRCPNewEntities();
        public static PlacementModel fillPlacement(int page, int pageSize)
        {

            sess = (clsSession)HttpContext.Current.Session["UserSessionClient"];
            GridListPlacement grdPlacement = new GridListPlacement();
            PlacementModel listModel = new PlacementModel();
            listModel.pageModel.CurrentPageIndex = page;
            listModel.pageModel.PageSize = pageSize;
            IList<GridListPlacement> retunmodel = new List<GridListPlacement>();
            if (sess != null)
            {
                //var Deptmdl = (from objPlacement in RPCobj.Placements
                //               join objLkUp in RPCobj.LookUps on objPlacement.Department equals objLkUp.LookupId
                //               where (objPlacement.StudentPersonalId == sess.StudentId && objPlacement.Status == 1)
                //               select new
                //               {
                //                   placementId = objPlacement.PlacementId,
                //                   DeptName = objLkUp.LookupName
                //               }).ToList();
                retunmodel = (from objPlacement in RPCobj.Placements
                              join objLookUp in RPCobj.LookUps on objPlacement.PlacementType equals objLookUp.LookupId
                              join objLkUp in RPCobj.LookUps on objPlacement.Department equals objLkUp.LookupId
                              join objdept in RPCobj.LookUps on objPlacement.PlacementDepartment equals objdept.LookupId
                              join objloc in RPCobj.Classes on objPlacement.Location equals objloc.ClassId
                              where (objPlacement.StudentPersonalId == sess.StudentId && objPlacement.Status == 1)
                              select new GridListPlacement
                              {
                                  PlacementId = objPlacement.PlacementId,
                                  PlacementName = objdept.LookupName + "-" + objLookUp.LookupName,
                                  Program = objLkUp.LookupName,
                                  StartDate = objPlacement.StartDate,
                                  EndDate = objPlacement.EndDate,
                                  LocationId=objloc.ClassName,
                                  IsMonday=objPlacement.IsMonday,
                                  IsTuesday=objPlacement.IsTuesday,
                                  IsWednesday=objPlacement.IsWednesday,
                                  IsThursday=objPlacement.IsThursday,
                                  IsFriday=objPlacement.IsFriday,
                                  IsSaturday=objPlacement.IsSaturday,
                                  IsSunday=objPlacement.IsSunday,
                                  MaxStudents=objloc.MaxStudents

                              }).ToList();

                listModel.pageModel.TotalRecordCount = retunmodel.Count;
                retunmodel = retunmodel.OrderByDescending(objEvents => objEvents.PlacementId).Skip((page - 1) * pageSize).Take(pageSize).ToList();
                listModel.listPlacement = retunmodel;
                if (listModel.pageModel.PageSize > listModel.pageModel.TotalRecordCount) { listModel.pageModel.PageSize = listModel.pageModel.TotalRecordCount; }
                if (listModel.pageModel.TotalRecordCount == 0) { listModel.pageModel.CurrentPageIndex = 0; }
            }
            return listModel;
        }
        public PlacementModel()
        {
            listPlacement = new List<GridListPlacement>();
            pageModel = new PagingModel();
        }

        public class GridListPlacement
        {
            public virtual int PlacementId { get; set; }
            public virtual string PlacementName { get; set; }
            public virtual string Program { get; set; }
            public virtual string PlacementnStatus { get; set; }
            public DateTime? EndDate;
            public DateTime? StartDate;
            public virtual string LocationId { get; set; }
            public virtual bool? IsMonday { get; set; }
            public virtual bool? IsTuesday{ get; set; }
            public virtual bool? IsWednesday { get; set; }
            public virtual bool? IsThursday { get; set; }
            public virtual bool? IsFriday { get; set; }
            public virtual bool? IsSaturday { get; set; }
            public virtual bool? IsSunday { get; set; }
            public virtual int? MaxStudents { get; set; }
            public virtual string datetime
            {
                get
                {
                    if (EndDate != null)
                    {
                        return ((DateTime)EndDate).ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        return "";
                    }
                }

            }
            public virtual string startdatetime
            {
                get
                {
                    if (StartDate != null)
                    {
                        return ((DateTime)StartDate).ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        return "";
                    }
                }

            }
        }
    }
}