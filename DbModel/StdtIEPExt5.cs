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
    
    public partial class StdtIEPExt5
    {
        public int TableId { get; set; }
        public string TMName { get; set; }
        public string TMRole { get; set; }
        public string InitialIfInAttn { get; set; }
        public int StdtIEPId { get; set; }
    
        public virtual StdtIEPExt4 StdtIEPExt4 { get; set; }
    }
}
