using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClientDB.AppFunctions
{
    [Serializable]
    public class ClientPermissions
    {
        public string objectName { set; get; }
        public bool? ReadInd { set; get; }
        public bool? WriteInd { set; get; }
        








    }
}