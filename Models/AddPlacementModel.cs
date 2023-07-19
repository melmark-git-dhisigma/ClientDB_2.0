using ClientDB.DbModel;
using System;
using System.Collections.Generic;
using System.Data.Objects.SqlClient;
using System.Linq;
using System.Web.Mvc;

namespace ClientDB.Models
{
    public class AddPlacementModel
    {
        public virtual string placemntLogText { get; set; }
        public virtual int Id { get; set; }
        public virtual IEnumerable<SelectListItem> PlacementTypeList { get; set; }
        public virtual int? PlacementType { get; set; }
        public virtual IEnumerable<SelectListItem> DepartmentList { get; set; }
        public virtual int? Department { get; set; }
        public virtual IEnumerable<SelectListItem> PrimaryNurseList { get; set; }
        public virtual int? PrimaryNurse { get; set; }
        public virtual IEnumerable<SelectListItem> BehaviorAnalystList { get; set; }
        public virtual int? BehaviorAnalyst { get; set; }
        public virtual IEnumerable<SelectListItem> UnitClerkList { get; set; }
        public virtual int? UnitClerk { get; set; }
        public virtual string EndDateDate { get; set; }
        public virtual string StartDate { get; set; }

        public virtual string Reason { get; set; }
        public virtual string AssociatedPersonnel { get; set; }
        public virtual int? LocationId { get; set; }
        public virtual string LocationDisplay { get; set; }
        public virtual int? PlacementDepartmentId { get; set; }
        public virtual int? PlacementReason { get; set; }

        public virtual bool IsMonday { get; set; }
        public virtual bool IsTuesday { get; set; }
        public virtual bool IsWednesday { get; set; }
        public virtual bool IsThursday { get; set; }
        public virtual bool IsFriday { get; set; }
        public virtual bool IsSaturday { get; set; }
        public virtual bool IsSunday { get; set; }
        public virtual int? MaxStudents { get; set; }

        public virtual string MondayNote { get; set; }
        public virtual string TuesdayNote { get; set; }
        public virtual string WednesdayNote { get; set; }
        public virtual string ThursdayNote { get; set; }
        public virtual string FridayNote { get; set; }
        public virtual string SaturdayNote { get; set; }
        public virtual string SundayNote { get; set; }

        public virtual string newEventLog { get; set; }
        public List<SelectListItem> LocationList { get; set; }
        public List<SelectListItem> PlacementDepartmentList { get; set; }
        public List<SelectListItem> PlacementReasonList { get; set; }
        public List<SelectListItem> GetLocationList()
        {
            List<SelectListItem> result = new List<SelectListItem>();
            result.Add(new SelectListItem
            {
                Text = "---Select---",
                Value = ""
            });
            BiWeeklyRCPNewEntities dbobj = new BiWeeklyRCPNewEntities();
            var data = (from clas in dbobj.Classes
                        where clas.ActiveInd == "A"
                        select new SelectListItem
                        {
                            Text = clas.ClassName,
                            Value = SqlFunctions.StringConvert((decimal)clas.ClassId).Trim(),
                        }).ToList();
            foreach (var item in data)
            {
                result.Add(item);
            }
            return result;
        }

        public List<SelectListItem> GetPlacementDepartment(int SchoolId)
        {
            List<SelectListItem> result = new List<SelectListItem>();
            result.Add(new SelectListItem
            {
                Text = "---Select---",
                Value = ""
            });
            BiWeeklyRCPNewEntities dbobj = new BiWeeklyRCPNewEntities();
            var data = (from look in dbobj.LookUps
                        where look.LookupType == "PlacementDepartment" && look.SchoolId == SchoolId && look.ActiveInd=="A"
                        select new SelectListItem
                        {
                            Text = look.LookupName,
                            Value = SqlFunctions.StringConvert((decimal)look.LookupId).Trim(),
                        }).ToList();
            foreach (var item in data)
            {
                result.Add(item);
            }
            return result;
        }

        public List<SelectListItem> GetPlacementReason(int SchoolId)
        {
            List<SelectListItem> result = new List<SelectListItem>();
            result.Add(new SelectListItem
            {
                Text = "---Select---",
                Value = ""
            });
            BiWeeklyRCPNewEntities dbobj = new BiWeeklyRCPNewEntities();
            var data = (from look in dbobj.LookUps
                        where look.LookupType == "Placement Reason" && look.SchoolId == SchoolId
                        select new SelectListItem
                        {
                            Text = look.LookupName,
                            Value = SqlFunctions.StringConvert((decimal)look.LookupId).Trim(),
                        }).ToList();
            foreach (var item in data)
            {
                result.Add(item);
            }
            return result;
        }

        

        public AddPlacementModel()
        {
            LocationList = new List<SelectListItem>();
            PlacementDepartmentList = new List<SelectListItem>();
            PlacementReasonList = new List<SelectListItem>();
        }
    }
}