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
    
    public partial class ProgressRpt
    {
        public int ProgressId { get; set; }
        public int SchoolId { get; set; }
        public int StudentId { get; set; }
        public int ProReportId { get; set; }
        public string CRM_Academic { get; set; }
        public string CRM_Clinical { get; set; }
        public string CRM_Outings { get; set; }
        public string CRM_Other { get; set; }
        public string RTF_Q_BLStart { get; set; }
        public string RTF_Q_BLEnd { get; set; }
        public string RTF_Q_RptDate { get; set; }
        public string RTF_Q_TBehavior { get; set; }
        public string RTF_Q_Outlines { get; set; }
        public string RTF_M_BLStart { get; set; }
        public string RTF_M_BLEnd { get; set; }
        public string RTF_M_RptDate { get; set; }
        public string RTF_M_BgInfo { get; set; }
        public string RTF_M_BSPlan { get; set; }
        public string RTF_M_Assessments { get; set; }
        public string RTF_M_CIntegration { get; set; }
        public string RTF_M_CMedication { get; set; }
        public string RTF_M_DPlanning { get; set; }
        public string RTF_M_ADSite { get; set; }
        public string RTF_M_ADStay { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
    }
}
