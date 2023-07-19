using ClientDB.DbModel;
using System;
using System.Collections.Generic;
using System.Data.Objects.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClientDB.Models
{
    public class AddVisitaionModel
    {
        public virtual int Id { get; set; }
        public virtual IEnumerable<SelectListItem> EventTypeList { get; set; }
        public virtual int? EventType { get; set; }
        public virtual IEnumerable<SelectListItem> EventStatusList { get; set; }
        public virtual int? EventStatus { get; set; }
        public virtual string EventName { get; set; }
        public virtual string ExpiredOnDate { get; set; }
        public virtual string EventDate { get; set; }

        public virtual DateTime? DateRequested { get; set; }

        public virtual string _DateRequested
        {
            get
            {
                return DateRequested != null ? ((DateTime)DateRequested).ToString("MM/dd/yyyy") : "";
            }
            set
            {
                if(value!=null && value!="")
                {
                    DateRequested = DateTime.ParseExact(value, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                }
                else
                {
                    DateRequested = null;
                }
            }
        }

        public virtual int? Form { get; set; }
        public virtual string Note { get; set; }
        public virtual DateTime? EffectiveDate { get; set; }

        public virtual string _EffectiveDate
        {
            get
            {
                return EffectiveDate != null ? ((DateTime)EffectiveDate).ToString("MM/dd/yyyy") : "";
            }
            set
            {
                if (value != null && value != "")
                {
                    EffectiveDate = DateTime.ParseExact(value, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                }
                else
                {
                    EffectiveDate = null;
                }
            }
        }

        public List<SelectListItem> FormList { get; set; }

        public List<SelectListItem> GetFormList(int studentid,int schoolId)
        {
            BiWeeklyRCPNewEntities dbobj = new BiWeeklyRCPNewEntities();
            List<SelectListItem> result = new List<SelectListItem>();
            result.Add(new SelectListItem
            {
                Text = "---Select---",
                Value = ""
            });

            //var data = (from doc in dbobj.Documents
            //            join look in dbobj.LookUps on doc.DocumentType equals look.LookupId
            //            where look.LookupCode == "Visitation" && doc.StudentPersonalId == studentid && doc.SchoolId == schoolId
            //            select new SelectListItem
            //            {
            //                Text = doc.DocumentName,
            //                Value = SqlFunctions.StringConvert((decimal)doc.DocumentId).Trim(),
            //            }).ToList();

            var data = (from objDocuments in dbobj.binaryFiles
                        join objDocs in dbobj.Documents on objDocuments.DocId equals objDocs.DocumentId
                        join objlookup in dbobj.LookUps on objDocs.DocumentType equals objlookup.LookupId
                        where objDocuments.StudentId == studentid && objlookup.LookupCode == "Visitation" && objDocs.Status == true
                        select new SelectListItem
                        {
                            Value = SqlFunctions.StringConvert((decimal)objDocuments.BinaryId).Trim(),
                            Text = objDocuments.DocumentName,

                        }).ToList();

            foreach(var item in data)
            {
                result.Add(item);
            }
            return result;
        }

        public AddVisitaionModel()
        {
            FormList = new List<SelectListItem>();
        }
    }

}