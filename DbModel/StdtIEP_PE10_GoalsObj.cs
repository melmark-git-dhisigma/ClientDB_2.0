//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ClientDB.DbModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class StdtIEP_PE10_GoalsObj
    {
        public int Id { get; set; }
        public Nullable<int> StdtIEP_PEId { get; set; }
        public Nullable<int> GoalID { get; set; }
        public string MeasureAnualGoal { get; set; }
        public string StudentsProgress { get; set; }
        public string DescReportProgress { get; set; }
        public string ReportProgress { get; set; }
        public Nullable<int> StatusId { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
    }
}
