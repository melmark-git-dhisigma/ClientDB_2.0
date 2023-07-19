using ClientDB.AppFunctions;
using ClientDB.DbModel;
using System;
using System.Collections.Generic;
using System.Data.Objects.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClientDB.Models
{
    public class AddEventModel
    {
        public virtual int Id { get; set; }
        //public virtual IEnumerable<SelectListItem> EventTypeList { get; set; }
        //public virtual int? EventType { get; set; }
        public virtual IEnumerable<SelectListItem> EventStatusList { get; set; }
        public virtual int? EventStatus { get; set; }
        public virtual string EventName { get; set; }
        public virtual string ExpiredOnDate { get; set; }
        public virtual string EventDate { get; set; }
        public virtual int? EventTypes { get; set; }

        public virtual int? Contact { get; set; }
        public virtual string Note { get; set; }
        public virtual string UserName { get; set; }
        public virtual bool IsSystemEvent { get; set; }

        public List<SelectListItem> Contactlist { get; set; }

        public List<SelectListItem> GetContactList(int Studentid)
        {
            BiWeeklyRCPNewEntities dbobj = new BiWeeklyRCPNewEntities();
            List<SelectListItem> result = new List<SelectListItem>();
            result.Add(new SelectListItem
            {
                Text = "---Select---",
                Value = ""
            });

            var data = (from cont in dbobj.ContactPersonals
                        where cont.StudentPersonalId == Studentid && cont.Status == 1
                        select new SelectListItem
                        {
                            Text = cont.LastName + ", " + cont.FirstName,
                            Value = SqlFunctions.StringConvert((decimal)cont.ContactPersonalId).Trim(),
                        }).ToList();
            foreach(var item in data)
            {
                result.Add(item);
            }
            return result;
        }

        public List<SelectListItem> EventTypeList { get; set; }

        public List<SelectListItem> GetEventTypeList(int schoolid)
        {
            BiWeeklyRCPNewEntities dbobj = new BiWeeklyRCPNewEntities();
            List<SelectListItem> result = new List<SelectListItem>();
            result.Add(new SelectListItem
            {
                Text = "---Select---",
                Value = ""
            });

            List<SelectListItem> data = new List<SelectListItem>();
            if (!IsSystemEvent)
            {
                data = (from look in dbobj.LookUps
                        where look.LookupType == "EventType" && look.SchoolId==schoolid
                        select new SelectListItem
                        {
                            Text = look.LookupName,
                            Value = SqlFunctions.StringConvert((decimal)look.LookupId).Trim(),
                        }).ToList();
            }
            else
            {
                data = (from look in dbobj.LookUps
                        where look.LookupType == "SysEventType"
                        select new SelectListItem
                        {
                            Text = look.LookupName,
                            Value = SqlFunctions.StringConvert((decimal)look.LookupId).Trim(),
                        }).ToList();
            }
            foreach (var item in data)
            {
                result.Add(item);
            }
            return result;
        }

        public static bool CreateSystemEvent(string EvtName, string EvetTypes,string Note)
        {
             BiWeeklyRCPNewEntities dbobj = new BiWeeklyRCPNewEntities();
            var sess = (clsSession)HttpContext.Current.Session["UserSessionClient"];
            AddEventModel model = new AddEventModel();
            model.EventDate = DateTime.Now.ToString("MM/dd/yyyy").Replace("-", "/");
            model.ExpiredOnDate = DateTime.Now.ToString("MM/dd/yyyy").Replace("-", "/");
            model.Note = Note;
            model.IsSystemEvent = true;
            model.EventName = EvtName;
            model.UserName = sess.UserName;
            //finding LookupVals
            var EventType = dbobj.LookUps.Where(x => x.LookupCode == EvetTypes && x.LookupType == "SysEventType").ToList();
            var Status = dbobj.LookUps.Where(x => x.LookupCode == "Expired" && x.LookupType == "Visitation Status").ToList();
            if(EventType.Count>0)
            {
                model.EventTypes = EventType[0].LookupId;
            }
            if(Status.Count>0)
            {
                model.EventStatus = Status[0].LookupId;
            }
            Other_Functions objFuns = new Other_Functions();
            var result = objFuns.SaveEventData(model,true);
            if(result=="Sucess")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public AddEventModel()
        {
            Contactlist = new List<SelectListItem>();
            EventTypeList = new List<SelectListItem>();
        }

        
    }
}