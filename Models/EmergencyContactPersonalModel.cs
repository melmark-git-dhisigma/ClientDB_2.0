using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClientDB.Models
{
    public class EmergencyContactPersonalModel
    {
        public string Relation { get; set; }
        public string Name { get; set; }
        public string PrimaryLanguage { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string OtherPhone { get; set; }
        public string PrimaryEmail { get; set; }
    }
}