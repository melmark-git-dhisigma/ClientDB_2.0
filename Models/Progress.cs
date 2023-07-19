using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using ClientDB.AppFunctions;
using ClientDB.DbModel;

using System.Data.Objects.SqlClient;
using System.Collections;


namespace ClientDB.Models
{
    public class Progress
    {
        public virtual ParentServiceReference.ProgressDetails ProgressInfo { get; set; }


        public Progress()
        {
            ProgressInfo = new ParentServiceReference.ProgressDetails();
         
        }
    }


}

















