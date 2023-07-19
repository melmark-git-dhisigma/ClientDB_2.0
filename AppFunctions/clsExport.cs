using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ClientDB.AppFunctions;
using ClientDB.DbModel;


namespace ClientDB.AppFunctions
{
    public class clsExport
    {
        public virtual ParentServiceReference.ProgressDetails ProgressInfo { get; set; }

        public clsExport()
        {
            ProgressInfo = new ParentServiceReference.ProgressDetails();

        }
    }
}
