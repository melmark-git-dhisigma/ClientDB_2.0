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
    
    public partial class Client_Dashboard_Result
    {
        public int ReferralId { get; set; }
        public string ReferralName { get; set; }
        public Nullable<System.DateTime> BirthDate { get; set; }
        public string Gender { get; set; }
        public Nullable<System.DateTime> Appdate { get; set; }
        public string ImageUrl { get; set; }
        public Nullable<double> Percentage { get; set; }
        public string LastCompleted { get; set; }
        public string CompletedBy { get; set; }
        public Nullable<int> ActiveProcess { get; set; }
        public string QueueType { get; set; }
    }
}
