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
    
    public partial class ProgressRpt_Doc
    {
        public int DocId { get; set; }
        public Nullable<int> SchoolId { get; set; }
        public Nullable<int> StudentId { get; set; }
        public int ProgressId { get; set; }
        public Nullable<int> TabId { get; set; }
        public string DocumentName { get; set; }
        public string ContentType { get; set; }
        public byte[] Data { get; set; }
        public string Type { get; set; }
        public string ModuleName { get; set; }
        public string VersionNo { get; set; }
        public Nullable<bool> Verified { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
    }
}
