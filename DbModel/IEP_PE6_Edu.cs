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
    
    public partial class IEP_PE6_Edu
    {
        public int Id { get; set; }
        public Nullable<int> StdtIEP_PEId { get; set; }
        public string Service { get; set; }
        public string Location { get; set; }
        public string Frequency { get; set; }
        public Nullable<System.DateTime> PrjBeginning { get; set; }
        public string AnticipatedDur { get; set; }
        public string Person { get; set; }
    }
}