using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ClientDB.DbModel;
using ClientDB.Models;
using ClientDB.AppFunctions;
using System.Data.Entity.Validation;
using System.Diagnostics;

namespace ClientDB.Models
{
    public class LetterGenerationViewModel
    {
        public virtual IList<LetterList> LetterLists { get; set; }
        public virtual PagingModel pageModel { get; set; }



        public static LetterGenerationViewModel getLetterAll(int page, int pageSize)
        {
            var LetterList = new List<Models.LetterList>();
            clsSession session = (clsSession)HttpContext.Current.Session["UserSessionClient"];
            BiWeeklyRCPNewEntities objData = new BiWeeklyRCPNewEntities();

            LetterGenerationViewModel listModel = new LetterGenerationViewModel();
            listModel.pageModel.CurrentPageIndex = page;
            listModel.pageModel.PageSize = pageSize;

            try
            {
                LetterList = (from x in objData.ref_LetterTrayValues
                              join y in objData.LetterEngines on x.LetterId equals y.LetterEngineId
                              join z in objData.ref_Recipients on x.RecipientId equals z.RecipientId
                              join referral in objData.StudentPersonals on x.StudentPersonalId equals referral.StudentPersonalId
                              where (x.StudentPersonalId == session.StudentId)
                              select new LetterList
                              {
                                  LetterName = y.LetterEngineName,
                                  ReferralName = referral.LastName + "," + referral.FirstName,
                                  ReferralFName = referral.FirstName,
                                  ReferralLName = referral.LastName,
                                  RecipientName = z.RecipientName,
                                  CreatedOn = x.CreatedOn,
                                  //checkListval=x.Status.ToString(),
                                  status = (bool)x.Status,
                                  SentOn = x.ModifiedOn,
                                  LetterTrayId = x.LetterTrayId
                              }).ToList();

                

            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
            }
            listModel.pageModel.TotalRecordCount = LetterList.Count;
            LetterList = LetterList.OrderByDescending(objletter => objletter.LetterTrayId).Skip((page - 1) * pageSize).Take(pageSize).ToList();
            listModel.LetterLists = LetterList;
            if (listModel.pageModel.PageSize > listModel.pageModel.TotalRecordCount) { listModel.pageModel.PageSize = listModel.pageModel.TotalRecordCount; }
            if (listModel.pageModel.TotalRecordCount == 0) { listModel.pageModel.CurrentPageIndex = 0; }

            return listModel;
        }
        public LetterGenerationViewModel()
        {
            LetterLists = new List<LetterList>();
            pageModel = new PagingModel();
        }

    }
    public class LetterList
    {
        public virtual string LetterName { get; set; }
        public virtual string ReferralName { get; set; }
        public virtual string ReferralFName { get; set; }
        public virtual string ReferralLName { get; set; }
        public virtual string RecipientName { get; set; }
        public virtual DateTime? CreatedOn { get; set; }
        public virtual bool status { get; set; }
        public virtual DateTime? SentOn { get; set; }
        public virtual string checkListval { get; set; }
        public virtual int LetterTrayId { get; set; }
        public virtual string CreatedDate { get; set; }
        public virtual string SentDate { get; set; }
    }
}