using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ClientDB.ParentServiceReference;

namespace ClientDB.Models
{
    public class ProgressGrid
    {

        public virtual IEnumerable<ParentServiceReference.ReportDetails> ProgressReports { get; set; }

        ParentServiceClient parentService = new ParentServiceClient();
        public ProgressGrid()
        {
            ProgressReports = new List<ParentServiceReference.ReportDetails>();           
        }
        public void GetProgressList(int UserId, int SchoolId)
        {
           var ProgressReports = parentService.GetProgressReport(1, 1);
            //return ProgressGrid;
        }
    }
    
}
