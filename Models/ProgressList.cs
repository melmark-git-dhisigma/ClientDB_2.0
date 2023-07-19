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
    public class ProgressList
    {
        public virtual ParentServiceReference.ProgressDetails ProgressInfo { get; set; }
        public virtual IList<ReportDetails> RepDetails { get; set; }

        public string SclDistrictName { get; set; }
        public string SclDistrictAddress { get; set; }
        public string SclDistrictContact { get; set; }
        public string StudentName { get; set; }
        public DateTime IEPDtFrom { get; set; }
        public DateTime IEPDtTo { get; set; }
        public DateTime DOB { get; set; }
        public int ID { get; set; }
        public int stdtIEPId { get; set; }
        public List<GoalData> GoalData { get; set; }
       
    
        public ProgressList()
        {
            GoalData = new List<GoalData>();
        }
        public string student { get; set; }
        public string schooladdr { get; set; }
    }

    //public class Goal
    //{
    //    public string GoalName { get; set; }
    //    public string LessonplanName { get; set; }
    //    public int GoalID { get; set; }
    //    public virtual IList<GridListPlacement> PlcacementList { get; set; }
    //    public List<ReportInfo> RptList { get; set; }
    //    public Goal()
    //    {
    //        PlcacementList = new List<GridListPlacement>();
    //        RptList = new List<ReportInfo>();
    //    }
    //}

    public class ReportInfo
    {
        public int rptid { get; set; }
        public DateTime rptdate { get; set; }
        public string rptinfo { get; set; }
        public int goalid { get; set; }
        public int stdtIEPId { get; set; }
        public bool VisibleToParent { get; set; }
    }

    public class GridListPlacement
    {
        public virtual string objective1 { get; set; }
        public virtual string objective2 { get; set; }
        public virtual string objective3 { get; set; }


    }
    public class ProgressDetails
    {
        public virtual string Status { get; set; }

        public string SclDistrictName { get; set; }

        public string SclDistrictAddress { get; set; }

        public string SclDistrictContact { get; set; }

        public string StudentName { get; set; }

        public DateTime IEPDtFrom { get; set; }

        public DateTime IEPDtTo { get; set; }

        public DateTime DOB { get; set; }

        public int ID { get; set; }
       
        public int stdtIEPId { get; set; }

        public virtual GoalData Goalid { get; set; }

        public virtual GoalData Lessonplanid { get; set; }

        public virtual GoalData GoalName { get; set; }

        public virtual GoalData LessonplanName { get; set; }

        public virtual List<GoalData> GoalDt { get; set; }

        public string student { get; set; }
       
        public string schooladdr { get; set; }     

        public ProgressDetails()
        {
            GoalDt = new List<GoalData>();
        }

       
    }

    public class GoalData
    {

        public virtual string GoalName { get; set; }

        public virtual string LessonplanName { get; set; }
        public virtual int Goalid { get; set; }
        public virtual int GoalLPRelId { get; set; }
        public virtual int Lessonplanid { get; set; }

        public virtual int GoalNo { get; set; }

        public virtual List<GridListPlacement> PlcacementList { get; set; }

        public virtual List<ReportInfo> RptList { get; set; }

        public virtual List<ReportDetails> ReportDetails { get; set; }

        public GoalData()
        {
            PlcacementList = new List<GridListPlacement>();
            RptList = new List<ReportInfo>();
            ReportDetails = new List<ReportDetails>();
        }
    }


    public class ReportDetails
    {
        public int iepid { get; set; }

        public string rptdate { get; set; }

        public DateTime startdate { get; set; }

        public DateTime enddate { get; set; }

        public virtual string Status { get; set; }
    }








}










