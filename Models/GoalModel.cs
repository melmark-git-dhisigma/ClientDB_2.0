using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ClientDB.DbModel;
using System.Data;

namespace ClientDB.Models
{
    public class GoalModel
    {
        public int? GoalId { get; set; }
        public string GoalName { get; set; }
        public string Objective1 { get; set; }
        public string Objective2 { get; set; }
        public string Objective3 { get; set; }

        public List<LessonPlanModel> LessonPlans { get; set; }

        private DateTime StartDate;
        private DateTime EndDate;
        private int StudentId;
        public List<GoalModel> ProcessedData { get; set; }

        public GoalModel(int studId, DateTime SDate, DateTime ETime)
        {
            StudentId = studId;
            StartDate = SDate;
            EndDate = ETime;
            // GetGoal();
            ProcessedData = Get_Goal_Lessons_Behavior(StartDate, EndDate, StudentId);
            TestData();
        }

        public GoalModel()
        {
            LessonPlans = new List<LessonPlanModel>();
        }

        private void GetGoal()
        {
            var result = new List<GoalModel>();
            // do coding...
            //return result;
        }

        private void TestData()
        {
            ProcessedData = new List<GoalModel>();
            GoalModel G1 = new GoalModel();
            LessonPlanModel l1 = new LessonPlanModel();
            MeasureModel lm1 = new MeasureModel();
            lm1.CreateDateTime = DateTime.Now;
            lm1.IsFromBehavior = false;
            lm1.MeasureType = "%Accuracy";
            lm1.Score = 100;
            l1.Measures.Add(lm1);
            l1.LessonName = "Brushing";
            l1.LessonPlanId = 1;
            LessonPlanModel l2 = new LessonPlanModel();
            MeasureModel lm2 = new MeasureModel();
            lm2.CreateDateTime = DateTime.Now;
            lm2.IsFromBehavior = false;
            lm2.MeasureType = "%Accuracy";
            lm2.Score = 0;
            l2.Measures.Add(lm2);
            l2.LessonName = "PushUp";
            l2.LessonPlanId = 2;
            G1.LessonPlans.Add(l1);
            G1.LessonPlans.Add(l2);
            G1.GoalId = 1;
            G1.GoalName = "Body";

            GoalModel G2 = new GoalModel();
            LessonPlanModel gl1 = new LessonPlanModel();
            MeasureModel glm1 = new MeasureModel();
            glm1.CreateDateTime = DateTime.Now;
            glm1.IsFromBehavior = false;
            glm1.MeasureType = "%Accuracy";
            glm1.Score = 100;
            gl1.Measures.Add(glm1);
            gl1.LessonName = "Loving";
            gl1.LessonPlanId = 3;
            LessonPlanModel gl2 = new LessonPlanModel();
            MeasureModel glm2 = new MeasureModel();
            glm2.CreateDateTime = DateTime.Now;
            glm2.IsFromBehavior = false;
            glm2.MeasureType = "%Accuracy";
            glm2.Score = 0;
            gl2.Measures.Add(glm2);
            gl2.LessonName = "Painting";
            gl2.LessonPlanId = 4;
            G2.LessonPlans.Add(gl1);
            G2.LessonPlans.Add(gl2);
            G2.GoalId = 2;
            G2.GoalName = "Action";

            ProcessedData.Add(G1);
            ProcessedData.Add(G2);
        }


        public List<GoalModel> Get_Goal_Lessons_Behavior(DateTime StartDate, DateTime EndDate, int StudentId)
        {
            //try
            //{
            ProcessedData = new List<GoalModel>();
            BiWeeklyRCPNewEntities objData = new BiWeeklyRCPNewEntities();
            var goalLessons = objData.Goal_Lessons_Behavior(StartDate, EndDate, StudentId).ToList();



            int goalid = 0;
            GoalModel GModel = new GoalModel();

            int? curr_goalId = goalLessons.Count > 0 ? goalLessons[0].GoalId : 0;
            GModel.GoalId = goalLessons.Count > 0 ? goalLessons[0].GoalId : 0;
            GModel.GoalName = goalLessons.Count > 0 ? goalLessons[0].GoalName : "";
            GModel.Objective1 = goalLessons.Count > 0 ? goalLessons[0].Objective1 : "";
            GModel.Objective2 = goalLessons.Count > 0 ? goalLessons[0].Objective2 : "";
            GModel.Objective3 = goalLessons.Count > 0 ? goalLessons[0].Objective3 : "";

            foreach (var item in goalLessons)
            {

                if (curr_goalId != item.GoalId)
                {
                    ProcessedData.Add(GModel);
                    GModel = new GoalModel();
                    GModel.GoalId = item.GoalId;
                    GModel.GoalName = item.GoalName;
                    GModel.Objective1 = item.Objective1;
                    GModel.Objective2 = item.Objective2;
                    GModel.Objective3 = item.Objective3;
                }

                LessonPlanModel gl1 = new LessonPlanModel();
                MeasureModel glm1 = new MeasureModel();
                glm1.CreateDateTime = DateTime.Now;
                glm1.IsFromBehavior = Convert.ToBoolean(item.IsBehavior);

                glm1.MeasureType = item.CalcType;
                glm1.Score = item.Score;
                gl1.Measures.Add(glm1);
                if (glm1.IsFromBehavior == true)
                {
                    if (item.LessonPlanName != null)
                    {
                        gl1.LessonName = item.LessonPlanName;
                        gl1.LessonPlanId = item.LessonPlanId;
                    }
                    else
                    {
                        gl1.LessonName = item.Behavior;
                        gl1.LessonPlanId = item.DSTempHdrId;
                    }
                }
                else
                {
                    gl1.LessonName = item.LessonPlanName;
                    gl1.LessonPlanId = item.LessonPlanId;
                    gl1.Objective = item.Objective;
                    gl1.Baseline = item.Baseline;
                }
                GModel.LessonPlans.Add(gl1);

                curr_goalId = item.GoalId;

            }
            if (goalLessons.Count > 0)
                ProcessedData.Add(GModel);
            return ProcessedData;
            //}
            //catch(Exception ex)
            //{

            //}


        }

    }

    public class LessonPlanModel
    {
        public int? LessonPlanId { get; set; }
        public string LessonName { get; set; }
        public string Objective { get; set; }
        public string Baseline { get; set; }
        public List<MeasureModel> Measures { get; set; }
        public LessonPlanModel()
        {
            Measures = new List<MeasureModel>();
        }
    }

    public class MeasureModel
    {
        public string MeasureType { get; set; }
        public double? Score { get; set; }
        public bool IsFromBehavior { get; set; }
        public DateTime CreateDateTime { get; set; }
        public MeasureModel()
        {

        }
    }
}