using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ClientDB.DbModel;
using ClientDB.Models;
using ClientDB.AppFunctions;
using System.Data.Entity.Validation;
using System.Diagnostics;

namespace ClientDB.Models
{
    public class EventsModel
    {
        GlobalData MetaData = new GlobalData();
        public static clsSession sess = null;
        public virtual string Searchtext { get; set; }
        public virtual PagingModel pageModel { get; set; }
        public virtual IList<GridListEvents> listEvents { get; set; }
        public static BiWeeklyRCPNewEntities RPCobj = new BiWeeklyRCPNewEntities();
        
        public static EventsModel fillEvents(int page, int pageSize)
        {
            
            sess = (clsSession)HttpContext.Current.Session["UserSessionClient"];
            EventsModel listModel = new EventsModel();
            listModel.pageModel.CurrentPageIndex = page;
            listModel.pageModel.PageSize = pageSize;
            DateTime now = DateTime.Now;
            IList<GridListEvents> retunmodel = new List<GridListEvents>();
            try
            {
                retunmodel = (from objEvents in RPCobj.Events
                              join objLookUps in RPCobj.LookUps on objEvents.EventStatus equals objLookUps.LookupId
                              join LookEType in RPCobj.LookUps on objEvents.EventType equals LookEType.LookupId
                              where (objEvents.StudentPersonalId == sess.StudentId && objEvents.Status == 1)
                              select new GridListEvents
                              {
                                  EventId = objEvents.EventId,
                                  EventName = objEvents.EventsName,
                                  EventStatus = (objEvents.IsSystemEvent == true) ? "N/A" : objLookUps.LookupName,
                                  EventDate = objEvents.EventDate,
                                  EventType = LookEType.LookupName,
                                  ExpiredOn = objEvents.ExpiredOn,


                              }).ToList();

                ////if (retunmodel != null)
                ////{
                ////    foreach (var item in retunmodel)
                ////    {
                ////        if (item.ExpiredOn <= now)
                ////        {
                ////            var data = (from objEvents in RPCobj.Events
                ////                        join objLookUps in RPCobj.LookUps on objEvents.EventStatus equals objLookUps.LookupId
                ////                        where (objEvents.StudentPersonalId == sess.StudentId && objEvents.Status == 1 && objEvents.EventId == item.EventId)
                ////                        select new GridListEvents
                ////                        {
                ////                            EventStatus = "Expired",
                ////                        }).SingleOrDefault();
                ////            item.EventStatus = data.EventStatus;

                ////        }
                        
                ////    }
                ////}

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
            retunmodel = retunmodel.OrderByDescending(objEvents => objEvents.EventId).Skip((page - 1) * pageSize).Take(pageSize).ToList();
            listModel.listEvents = retunmodel;
            if (listModel.pageModel.PageSize > listModel.pageModel.TotalRecordCount) { listModel.pageModel.PageSize = listModel.pageModel.TotalRecordCount; }
            if (listModel.pageModel.TotalRecordCount == 0) { listModel.pageModel.CurrentPageIndex = 0; }

            return listModel;
        }

      

        public EventsModel()
        {
            listEvents = new List<GridListEvents>();
            pageModel = new PagingModel();
        }
    }
    public class GridListEvents
    {
        public virtual int EventId { get; set; }
        public virtual string EventName { get; set; }
        public virtual DateTime EventDate { get; set; }
        public virtual DateTime? ExpiredOn { get; set; }
        public virtual string EventStatus { get; set; }
        public virtual string EventType { get; set; }
        public virtual string Eventdate { get; set; }
        public virtual string expiredOn { get; set; }
       



    }
}