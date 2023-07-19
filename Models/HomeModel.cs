using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClientDB.Models
{
    public class HomeModel
    {
        public virtual ParentServiceReference.ParentDetails parentInfo { get; set; }
        public virtual ParentServiceReference.ParentDetailsPA parentInfoPA { get; set; }
        public HomeModel()
        {
            parentInfo = new ParentServiceReference.ParentDetails();
            parentInfoPA = new ParentServiceReference.ParentDetailsPA();
        }

    }
}