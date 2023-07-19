using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ClientDB.Models;
using ClientDB.DbModel;
using System.Web.Mvc;
using System.Collections;

namespace ClientDB.Models
{
    public class ProgressReport
    {
        public static clsSession sess = null;
        public int ProgressId { get; set; }
        public string StudentName { get; set; }
        public string DateOfBirth { get; set; }
        public int ProReportId { get; set; }

        public bool btnStatus { get; set; }

        public string CRM_PhaseLines { get; set; }
        public string CRM_ConditionLines { get; set; }
        public string CRM_ArrowNotes { get; set; }

        public string CRM_Academic { get; set; }
        public string CRM_Clinical { get; set; }
        public string CRM_Outings { get; set; }
        public string CRM_Other { get; set; }

        public int RTFQId { get; set; }
        public string RTF_Q_BLStart { get; set; }
        public string RTF_Q_BLEnd { get; set; }
        public string RTF_Q_RptDate { get; set; }
        public string RTF_Q_TBehavior { get; set; }
        public string RTF_Q_Outlines { get; set; }

        public int RTFMId { get; set; }
        public string RTF_M_BLStart { get; set; }
        public string RTF_M_BLEnd { get; set; }
        public string RTF_M_RptDate { get; set; }
        public string RTF_M_BgInfo { get; set; }
        public string RTF_M_BSPlan { get; set; }
        public string RTF_M_Assessments { get; set; }
        public string RTF_M_CIntegration { get; set; }
        public string RTF_M_CMedication { get; set; }
        public string RTF_M_DPlanning { get; set; }
        public string RTF_M_ADSite { get; set; }
        public string RTF_M_ADStay { get; set; }

        public string StdtName { get; set; }
        public string Location { get; set; }
        public string Program { get; set; }
        public DateTime IEPYrStart { get; set; }
        public DateTime IEPYrEnd { get; set; }
        public DateTime PeriodStart { get; set; }
        public DateTime PeriodEnd { get; set; }

        public DateTime date1 { get; set; }
        public DateTime date2 { get; set; }
        public DateTime date3 { get; set; }
        public DateTime date4 { get; set; }
        public DateTime date5 { get; set; }
        public DateTime date6 { get; set; }

        public DateTime dateQ1 { get; set; }
        public DateTime dateQ2 { get; set; }
        public DateTime dateQ3 { get; set; }
        public DateTime dateQ4 { get; set; }

        public virtual int DocId { get; set; }
        public int TabId { get; set; }

        public virtual IEnumerable<SelectListItem> DocumentTypeList { get; set; }
        public virtual int? DocumentType { get; set; }
        public virtual IEnumerable<SelectListItem> DocumentModuleList { get; set; }
        public virtual int? DocumentModule { get; set; }
        public virtual string DocumentName { get; set; }
        public HttpFileCollectionBase profilePicture { get; set; }
        public virtual string Other { get; set; }

        public virtual int reportType { get; set; }

        public List<ProRptList> Rpt_List { get; set; }

        public List<SelectListItem> FormList { get; set; }


        public List<Tab1DocList> Tab1Doc_List { get; set; }
        public List<Tab2DocList> Tab2Doc_List { get; set; }

        public List<LessonP> LP_List { get; set; }
        public List<MeaS> Meas_List { get; set; }

        public List<GoalModel> baseline { get; set; }
        public List<GoalModel> gmodel1 { get; set; }
        public List<GoalModel> gmodel2 { get; set; }
        public List<GoalModel> gmodel3 { get; set; }

        public virtual PagingModel pageModel { get; set; }
        public static BiWeeklyRCPNewEntities dbobj = new BiWeeklyRCPNewEntities();

        public virtual ClientDB.AppFunctions.GoalHashData hash { get; set; }
        public virtual ClientDB.AppFunctions.GoalHashData hashQuarter { get; set; }

        public ProgressReport()
        {
            
            pageModel = new PagingModel();
            Rpt_List = new List<ProRptList>();
            Tab1Doc_List = new List<Tab1DocList>();
            Tab2Doc_List = new List<Tab2DocList>();
            baseline = new List<GoalModel>();
            gmodel1 = new List<GoalModel>();
            gmodel2 = new List<GoalModel>();
            gmodel3 = new List<GoalModel>();
            LP_List = new List<LessonP>();
            Meas_List = new List<MeaS>();
            hash = new AppFunctions.GoalHashData();
            hashQuarter = new AppFunctions.GoalHashData();
        }
    }

    //public class GoalHashData
    //{
    //    public string[] LPHeader = new string[5];
    //    public Hashtable goalHash = new Hashtable();
    //    public Hashtable LessonHash = new Hashtable();
    //}

    public class LessonP
    {
       public int LessonPlanId { get; set; }
        public string LessonName { get; set; }
        public List<MeaS> Measures { get; set; }
        public LessonP()
        {
            Measures = new List<MeaS>();
        }
    }

    public class MeaS
    {
       public string MeasureType { get; set; }
        public decimal? Score { get; set; }
        public bool IsFromBehavior { get; set; }
        public DateTime CreateDateTime { get; set; }
        public MeaS()
        {

        }
    }

    public class ProRptList
    {
        public int ProReportId { get; set; }
        public DateTime? RptCreatedOn { get; set; }
        public List<ProRptList> Rpt_List { get; set; }
    }

    public class Tab1DocList
    {
        public virtual int DocumentId { get; set; }
        public int ProReportId { get; set; }
        public virtual string DocumentName { get; set; }
        public virtual string DocumentType { get; set; }
        public DateTime? CreatedOn { get; set; }
        public List<Tab1DocList> Tab1Doc_List { get; set; }
    }

    public class Tab2DocList
    {
        public virtual int DocumentId { get; set; }
        public int ProReportId { get; set; }
        public virtual string DocumentName { get; set; }
        public virtual string DocumentType { get; set; }
        public DateTime? CreatedOn { get; set; }
        public List<Tab2DocList> Tab2Doc_List { get; set; }
    }
}