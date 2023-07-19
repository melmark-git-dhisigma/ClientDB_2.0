using ClientDB.Models;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.Office.Interop.Word;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Packaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Xml;
using System.Web.Mvc;
using ClientDB.DbModel;
using ClientDB.AppFunctions;
using System.Net;
using System.Web.ClientServices;
using System.Text.RegularExpressions;
using ClientDB.ParentServiceReference;

namespace ClientDB.AppFunctions
{

    public class GoalHashData
    {
        public string[] LPHeader = new string[5];
        public Hashtable goalHash = new Hashtable();
        public Hashtable LessonHash = new Hashtable();

    }

    public class ProgressExprtApp : System.Web.UI.Page
    {

        clsSession sess = null;
        static string[] columns;
        static string[] columnsCheck;
        static int checkCount = 0;
        static string[] placeHolders;

        public static string URLTOHTML(string Url)
        {
            string result = "";
            try
            {

                using (StreamReader reader = new StreamReader(Url))
                {
                    string line = "";
                    while ((line = reader.ReadLine()) != null)
                    {
                        result += line;
                    }
                }
            }
            catch (IOException ex)
            {
                throw ex;
            }
            return result;
        }

        public string ExportAll(clsSession sess)
        {

            sess = (clsSession)Session["UserSessionClient"];
            string[] plcT, TextT;
            string Path = Server.MapPath("~\\Administration\\Temp\\IEP1.docx");
            string NewPath = CopyTemplate(Path, "0");
            Guid g = Guid.NewGuid();
            string ids = g.ToString();

            Thread thread = new Thread(new ThreadStart(WorkThreadFunction));
            string hPath = Server.MapPath("~\\Administration") + "\\IEPTemp1\\HTML" + ids + ".html";
            PageConvert(NewPath, hPath, WdSaveFormat.wdFormatHTML);
            System.Threading.Thread.Sleep(5000);
            thread.Abort();
            string Temp = Server.MapPath("~\\Administration") + "\\XMLProgressReport\\";
            string HtmlData = URLTOHTML(hPath);

            string[] filePaths = Directory.GetFiles(Temp);
            CreateQuery1("NE", "~\\Administration\\XMLProgressReport\\ProgressReport.xml", out plcT, out TextT, true, sess);
            HtmlData = replaceWithTexts(HtmlData, plcT, TextT);

            string htmlProgress = getHtmlStringProgress(sess);
            HtmlData = HtmlData.Replace("plcPRI", htmlProgress);


            HtmlData = HtmlData.Replace("’", "'");
            HtmlData = HtmlData.Replace("…", "...");
            HtmlData = HtmlData.Replace("‘", "'");
            HtmlData = HtmlData.Replace("·", "- ");
            HtmlData = HtmlData.Replace("“", "'");
            HtmlData = HtmlData.Replace("”", "'");
            HtmlData = HtmlData.Replace("�", "");
            HtmlData = HtmlData.Replace("<table>", "<table style='width:100%'>");
            HtmlData = HtmlData.Replace("<th>", "<th style='text-align:left;'>");
            HtmlData = HtmlData.Replace("MsoListParagraphCxSpFirst", "-");
            HtmlData = HtmlData.Replace("mso-list", "-");

            ExportToWord(HtmlData, NewPath);

            FileInfo f1 = new FileInfo(NewPath);
            if (f1.Exists)
            {
                f1.Delete();
            }

            f1 = new FileInfo(hPath);
            if (f1.Exists)
            {
                f1.Delete();
            }
            string path = Server.MapPath("~\\Administration") + "\\IEPTemp1";
            System.IO.DirectoryInfo downloadedMessageInfo = new DirectoryInfo(path);
            foreach (FileInfo file in downloadedMessageInfo.GetFiles())
            {
                file.Delete();
            }
            foreach (DirectoryInfo dir in downloadedMessageInfo.GetDirectories())
            {
                dir.Delete(true);
            }

            return HtmlData;
        }

        public string ExportAllNew(clsSession sess, string path)
        {
            string[] plcT, TextT;
            string newPath = CopyTemplate(path, "0");

            string sName = "";
            Progress tempz = new Progress();
            ParentServiceClient parentService = new ParentServiceClient();
            tempz.ProgressInfo = parentService.CLientsReport(sess.SchoolId, sess.StudentId, 0);

            if (tempz.ProgressInfo != null)
            {
                    sName = tempz.ProgressInfo.StudentName;
                    string sDate = tempz.ProgressInfo.IEPDtFrom.ToString("MM/dd/yyyy").Replace('-', '/');
                    string eDate = tempz.ProgressInfo.IEPDtTo.ToString("MM/dd/yyyy").Replace('-', '/');
                    string sID = tempz.ProgressInfo.ID.ToString();
                    string sDOB = tempz.ProgressInfo.DOB.ToString("MM/dd/yyyy").Replace('-', '/');

                    using (WordprocessingDocument theDoc = WordprocessingDocument.Open(newPath, true))
                    {
                        MainDocumentPart mainPart = theDoc.MainDocumentPart;
                        string content = null;

                        using (StreamReader reader = new StreamReader(theDoc.MainDocumentPart.HeaderParts.First().GetStream()))
                        {
                            content = reader.ReadToEnd();
                        }
                        Regex exSName = new Regex("plcStdName");
                        Regex exSDate = new Regex("plcIEPFrmDate");
                        Regex exEDate = new Regex("plcIEPToDate");
                        Regex exSID = new Regex("plcStdID");
                        Regex exSDOB = new Regex("plcStdDOB");

                        content = exSName.Replace(content, sName);
                        content = exSDate.Replace(content, sDate);
                        content = exEDate.Replace(content, eDate);
                        content = exSID.Replace(content, sID);
                        content = exSDOB.Replace(content, sDOB);

                        using (StreamWriter writer = new StreamWriter(theDoc.MainDocumentPart.HeaderParts.First().GetStream(FileMode.Create)))
                        {
                            writer.Write(content);
                        }
                        mainPart.Document.Save();
                    }
            }

            sess = (clsSession)Session["UserSession"];

            using (WordprocessingDocument theDoc = WordprocessingDocument.Open(newPath, true))
            {
                CreateQuery1("NE", "~\\Administration\\XMLProgressReport\\ProgressReport.xml", out plcT, out TextT, true, sess);
                replaceWithTexts1(theDoc.MainDocumentPart, plcT, TextT);
            }

            using (WordprocessingDocument theDoc = WordprocessingDocument.Open(newPath, true))
            {
                string htmlProgress = getHtmlStringProgress1(sess);
                replaceWithTextsSingle(theDoc.MainDocumentPart, "plcPRI", htmlProgress);
            }
            return newPath;
        }

        public void replaceWithTexts1(MainDocumentPart mainPart, string[] plcT, string[] TextT)
        {
            int count = plcT.Count();
            NotesFor.HtmlToOpenXml.HtmlConverter converter = new NotesFor.HtmlToOpenXml.HtmlConverter(mainPart);
            for (int i = 0; i < count; i++)
            {
                string textData = "";
                if (TextT[i] != null)
                {
                    textData = TextT[i];
                }
                else
                {
                    textData = "";
                }

                var paras = mainPart.Document.Descendants<DocumentFormat.OpenXml.Wordprocessing.Paragraph>().Where(element => element.InnerText == plcT[i]);

                string textDataNoSpace = textData.Replace(" ", "");

                foreach (DocumentFormat.OpenXml.Wordprocessing.Paragraph para in paras)
                {
                    var paragraphs = converter.Parse(textData);
                    if (paragraphs.Count == 0)
                    {
                        DocumentFormat.OpenXml.Wordprocessing.Paragraph tempPara = new DocumentFormat.OpenXml.Wordprocessing.Paragraph();
                        para.Parent.Append(tempPara);
                        para.Remove();
                    }
                    else
                    {
                        for (int k = 0; k < paragraphs.Count; k++)
                        {
                            bool isBullet = false;
                            if (textDataNoSpace.Contains("<li>" + paragraphs[k].InnerText.Trim()))
                                isBullet = true;
                            if (isBullet)
                            {
                                ParagraphProperties paraProp = new ParagraphProperties();
                                ParagraphStyleId paraStyleid = new ParagraphStyleId() { Val = "BulletPara" };
                                NumberingProperties numProp = new NumberingProperties();
                                NumberingLevelReference numLvlRef = new NumberingLevelReference() { Val = 0 };
                                NumberingId numID = new NumberingId() { Val = 1 };
                                numProp.Append(numLvlRef);
                                numProp.Append(numID);
                                paraProp.Append(paraStyleid);
                                paraProp.Append(numProp);

                                if (((DocumentFormat.OpenXml.Wordprocessing.Paragraph)paragraphs[k]).ParagraphProperties != null)
                                {
                                    //Assign Bullet point property to paragraph
                                    ((DocumentFormat.OpenXml.Wordprocessing.Paragraph)paragraphs[k]).ParagraphProperties.Append(paraProp);
                                }
                            }
                            para.Parent.Append(paragraphs[k]);

                        }
                        para.Remove();
                    }
                }
            }
        }

        public void replaceWithTextsSingle(MainDocumentPart mainPart, string plcT, string TextT)
        {
            try
            {
                NotesFor.HtmlToOpenXml.HtmlConverter converter = new NotesFor.HtmlToOpenXml.HtmlConverter(mainPart);

                string textData = "";

                if (TextT != null)
                {
                    textData = TextT;
                }
                else
                {
                    textData = "";
                }

                var paras = mainPart.Document.Descendants<DocumentFormat.OpenXml.Wordprocessing.Paragraph>().Where(element => element.InnerText == plcT);
                string textDataNoSpace = textData.Replace(" ", "");

                foreach (DocumentFormat.OpenXml.Wordprocessing.Paragraph para in paras)
                {
                    var paragraphs = converter.Parse(textData);

                    if (paragraphs.Count == 0)
                    {
                        DocumentFormat.OpenXml.Wordprocessing.Paragraph tempPara = new DocumentFormat.OpenXml.Wordprocessing.Paragraph();
                        para.Parent.Append(tempPara);
                    }
                    else
                    {
                        for (int k = 0; k < paragraphs.Count; k++)
                        {
                            bool isBullet = false;

                            if (textDataNoSpace.Contains("<li>" + paragraphs[k].InnerText.Trim()))
                                isBullet = true;

                            if (isBullet)
                            {
                                ParagraphProperties paraProp = new ParagraphProperties();
                                ParagraphStyleId paraStyleid = new ParagraphStyleId() { Val = "BulletPara" };
                                NumberingProperties numProp = new NumberingProperties();
                                NumberingLevelReference numLvlRef = new NumberingLevelReference() { Val = 0 };
                                NumberingId numID = new NumberingId() { Val = 1 };
                                numProp.Append(numLvlRef);
                                numProp.Append(numID);
                                paraProp.Append(paraStyleid);
                                paraProp.Append(numProp);

                                if (((DocumentFormat.OpenXml.Wordprocessing.Paragraph)paragraphs[k]).ParagraphProperties != null)
                                {
                                    //Assign Bullet point property to paragraph
                                    ((DocumentFormat.OpenXml.Wordprocessing.Paragraph)paragraphs[k]).ParagraphProperties.Append(paraProp);
                                }
                            }

                            para.Parent.Append(paragraphs[k]);
                        }
                    }
                    //para.RemoveAllChildren<Run>();
                }

                paras = mainPart.Document.Descendants<DocumentFormat.OpenXml.Wordprocessing.Paragraph>().Where(element => element.InnerText == plcT);

                foreach (DocumentFormat.OpenXml.Wordprocessing.Paragraph para in paras)
                {
                    para.RemoveAllChildren<Run>();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string getHtmlStringProgress1(clsSession sess)
        {
            try
            {
                sess = (clsSession)Session["UserSessionClient"];
                clsExport objExport = new clsExport();
                Progress tempz = new Progress();
                ParentServiceClient parentService = new ParentServiceClient();
                int SchoolID = sess.SchoolId;
                int StudentID = sess.StudentId;
                tempz.ProgressInfo = parentService.CLientsReport(SchoolID, StudentID, 0);
                string HTML3 = "";
                int count = 0;

                string studName = "";
                string studNbr = "";
                string studDOB = "";
                string DistrictName = "";
                string DistrictAddress = "";
                string DistrictContact = "";
                studName = tempz.ProgressInfo.StudentName;
                studNbr = tempz.ProgressInfo.ID.ToString();
                studDOB = tempz.ProgressInfo.DOB.ToString("MM/dd/yyyy").Replace('-', '/');
                DistrictName = tempz.ProgressInfo.SclDistrictName.ToString();
                DistrictAddress = tempz.ProgressInfo.SclDistrictAddress.ToString();
                DistrictContact = tempz.ProgressInfo.SclDistrictContact.ToString();

                    HTML3 += "<div style='width:100%'>";

                    if (tempz.ProgressInfo.GoalDt != null)
                    {
                            foreach (var item in tempz.ProgressInfo.GoalDt)
                            {
                                int tmp = count;

                                HTML3 += "<table style='width:100%; border:1px solid black; border-collapse:collapse;'>" +
                                "<tr><td colspan='2' style='border:1px solid black; background-color:black; color:white; text-align:center'><font face='Arial' size='11px'><b>INFORMATION FROM CURRENT IEP</b></font></td></tr>" +
                                "<tr><td style='border:1px solid black;'><font face='Arial' size='10px'><b>Goal #: " + item.Goalid + "</b></font></td><td style='border:1px solid black;'><font face='Arial' size='10px'><b>Specific Goal Focus:</b></font><font face='Helvetica' size='8px'> " + item.LessonplanName + "</font></td></tr>" +
                                "</table>";

                                string obj1 = "";
                                string obj2 = "";
                                string obj3 = "";
                                foreach (var itemsub in item.PlcacementList)
                                {
                                    obj1 = itemsub.objective1;
                                    obj2 = itemsub.objective2;
                                    obj3 = itemsub.objective3;
                                }

                                HTML3 += "<font face='Arial' size='8px'><b>Current Performance Level:</b> <i>What can the student currently do?</i></font><hr>" + obj1 + "<br><hr>" +
                                    "<font face='Arial' size='8px'><b>Measurable Annual Goal:</b> What challenging, yet attainable, goal can we expect the student to meet by the end of this IEP period?<br />How will we know that the student has reached this goal?</font><hr>" + obj2 + "<br><hr>" +
                                    "<font face='Arial' size='8px'><b>Benchmark/Objectives:</b> What will the student need to do to complete this goal?</font><hr>" + obj3 + "";

                                int index = 0;
                                int qu = 1;
                                int rptCount = 0;
                                foreach (var itemsub1 in item.RptList)
                                {
                                    rptCount++;
                                }

                                if (rptCount > 0)
                                {
                                        HTML3 += "<table style='width:100%; border:1px solid black; border-collapse:collapse;'><tr>" +
                                        "<td style='border:1px solid black; background-color:black; color:white; text-align:center'><font face='Arial' size='12px'><b>PROGRESS REPORT INFORMATION</b></font></td>" +
                                        "</tr></table>";

                                        foreach (var itemsub1 in item.RptList)
                                        {

                                            DateTime rDate = itemsub1.rptdate ?? DateTime.Now;
                                            string rDateSplit = rDate.ToString("MM/dd/yyyy").Replace('-', '/');

                                            HTML3 += "<table style='width:100%;'>" +
                                            "<tr>" +
                                            "<td><font face='Arial' size='9px'>Progress Report Date: <b>" + rDateSplit + "</b></font></td>" +
                                            "<td></td>" +
                                            "<td><font face='Arial' size='9px'>Progress Report #<b>" + qu + "</b> of <b>4</b></font></td>" +
                                            "</tr></table><br><hr>";

                                            if (qu == 1)
                                            {
                                                HTML3 += "<table style='width:100%;'><tr><td>";
                                                HTML3 += "<font face='Arial' size='3px'>Progress Reports are required to be sent to parents at least as often as parents are informed of their non-disabled children’s progress. Each progress report must describe the student’s progress toward meeting each annual goal.</font>";
                                                HTML3 += "</td></tr></table><br><hr>";
                                            }

                                            HTML3 += "<table style='width:100%;'><tr><td colspan='3'>" + itemsub1.rptinfo + "</td></tr></table><br><hr>";
                                            index++; qu++;
                                        }
                                }
                            }
                    }

                    HTML3 += "</div>";
                return HTML3;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public void WorkThreadFunction()
        {
            try
            {

                Thread.Sleep(5000);
            }
            catch (Exception ex)
            {
                // log errors
            }
        }


        public class ObjectiveClass
        {
            public ArrayList lessonaPlans = new ArrayList();
            public ArrayList Objective = new ArrayList();
            public ArrayList BEnchmark = new ArrayList();
            public string objectives = "";
        }


        public static GoalHashData getLEssonplanData(int progressID, ProgressReport CRMonthlyItems)
        {
            ObjectiveClass objClass = null;
            ProgressReport returnModel = new ProgressReport();
            clsSession sess = (clsSession)HttpContext.Current.Session["UserSessionClient"];
            BiWeeklyRCPNewEntities dbobj = new BiWeeklyRCPNewEntities();

            //  var CRMonthlyItems = new ProgressReport();

            if (progressID > 0)
            {



                //string date = "13.04.2012";
                //string date2 = (DateTime.Parse(date).AddDays(1)).ToShortDateString();

                DateTime dt1 = DateTime.ParseExact(CRMonthlyItems.RTF_M_RptDate, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                DateTime dt2 = dt1.AddDays(-30);

                DateTime dt3 = dt2.AddDays(-1);
                DateTime dt4 = dt3.AddDays(-30);

                DateTime dt5 = dt4.AddDays(-1);
                DateTime dt6 = dt5.AddDays(-30);

                DateTime blStart = DateTime.ParseExact(CRMonthlyItems.RTF_M_BLStart, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                DateTime blEnd = DateTime.ParseExact(CRMonthlyItems.RTF_M_BLEnd, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                CRMonthlyItems.date1 = dt1;
                CRMonthlyItems.date2 = dt2;
                CRMonthlyItems.date3 = dt3;
                CRMonthlyItems.date4 = dt4;
                CRMonthlyItems.date5 = dt5;
                CRMonthlyItems.date6 = dt6;

                GoalModel baseline = new GoalModel();

                GoalModel objGoal1 = new GoalModel();
                GoalModel objGoal2 = new GoalModel();
                GoalModel objGoal3 = new GoalModel();


                CRMonthlyItems.baseline = baseline.Get_Goal_Lessons_Behavior(blStart, blEnd, sess.StudentId);
                CRMonthlyItems.gmodel1 = objGoal1.Get_Goal_Lessons_Behavior(dt6, dt5, sess.StudentId);//sess.StudentId, dt5, dt6
                CRMonthlyItems.gmodel2 = objGoal2.Get_Goal_Lessons_Behavior(dt4, dt3, sess.StudentId);//sess.StudentId, dt3, dt4
                CRMonthlyItems.gmodel3 = objGoal3.Get_Goal_Lessons_Behavior(dt2, dt1, sess.StudentId);//sess.StudentId, dt1, dt2



                Hashtable ht = new Hashtable();

                foreach (var item in objGoal3.LessonPlans)
                {

                    //int gid=goal.GoalId;
                    //string gname = goal.GoalName;
                    //foreach (var lesson in goal.LessonPlans)
                    //{
                    //    string lpname = lesson.LessonName;
                    //    int? lpId = lesson.LessonPlanId;
                    //    foreach (var meas in lesson.Measures)
                    //    {
                    //        string type = meas.MeasureType;
                    //        string messcore = meas.Score.ToString();
                    //        bool isbehav = meas.IsFromBehavior;
                    //    }
                    //}

                }

                Hashtable GoalHash = new Hashtable();

                foreach (var item in objGoal3.ProcessedData)
                {
                    objClass = new ObjectiveClass();
                    if (!GoalHash.ContainsKey(item.GoalName))
                    {
                        //ArrayList LessonList = new ArrayList();


                        objClass.objectives = item.Objective1 + " " + item.Objective2 + " " + item.Objective3;
                        foreach (LessonPlanModel lp in item.LessonPlans)
                        {
                            if (!objClass.lessonaPlans.Contains(lp.LessonName))
                            {
                                objClass.lessonaPlans.Add(lp.LessonName);
                            }
                        }

                        GoalHash.Add(item.GoalName, objClass);
                    }
                    else
                    {
                        //ArrayList LessonList = (ArrayList)GoalHash[item.GoalName];
                        foreach (LessonPlanModel lp in item.LessonPlans)
                        {
                            if (!objClass.lessonaPlans.Contains(lp.LessonName))
                                objClass.lessonaPlans.Add(lp.LessonName);
                        }
                    }

                }
                foreach (var item in objGoal3.ProcessedData)
                {
                    foreach (var goal in item.LessonPlans)
                    {
                        if (goal.LessonName != null && !ht.ContainsKey(goal.LessonName))
                        {
                            string[] marrayString = new string[4];
                            marrayString[3] = GetMeasureScores(goal.Measures);
                            ht.Add(goal.LessonName, marrayString);
                        }
                        else
                        {
                            var CurData = (string[])ht[goal.LessonName];
                            var tempVar = GetMeasureScores(goal.Measures);
                            if(tempVar!="")
                            CurData[3] += " | " + GetMeasureScores(goal.Measures);
                        }
                    }

                }

                foreach (var item in objGoal2.ProcessedData)
                {
                    foreach (var goal in item.LessonPlans)
                    {
                        if (goal.LessonName != null && !ht.ContainsKey(goal.LessonName))
                        {
                            string[] marrayString = (string[])ht[goal.LessonName];
                            if (marrayString == null)
                            {
                                marrayString = new string[4];
                            }
                            var tempVar = GetMeasureScores(goal.Measures);
                            if (tempVar != "")
                            {
                                if (marrayString[2] == null)
                                {
                                    marrayString[2] += GetMeasureScores(goal.Measures);
                                }
                                else
                                marrayString[2] += " | " + GetMeasureScores(goal.Measures);
                            }
                        }
                        else
                        {
                            var CurData = (string[])ht[goal.LessonName];
                            var tempVar = GetMeasureScores(goal.Measures);
                            if (tempVar != "")
                            {
                                if (CurData[2] == null)
                                {
                                    CurData[2] += GetMeasureScores(goal.Measures);
                                }
                                else
                                CurData[2] += " | " + GetMeasureScores(goal.Measures);
                            }
                        }
                    }

                }
                foreach (var item in objGoal1.ProcessedData)
                {
                    foreach (var goal in item.LessonPlans)
                    {
                        if (goal.LessonName != null && !ht.ContainsKey(goal.LessonName))
                        {
                            string[] marrayString = (string[])ht[goal.LessonName];
                            if (marrayString == null)
                            {
                                marrayString = new string[4];
                            }
                            var tempVar = GetMeasureScores(goal.Measures);
                            if (tempVar != "")
                            {
                                if (marrayString[1] == null)
                                {
                                    marrayString[1] += GetMeasureScores(goal.Measures);
                                }
                                else
                                marrayString[1] += " | " + GetMeasureScores(goal.Measures);
                            }

                        }
                        else
                        {
                            var CurData = (string[])ht[goal.LessonName];
                            var tempVar = GetMeasureScores(goal.Measures);
                            if (tempVar != "")
                            {
                                if (CurData[1] == null)
                                {
                                    CurData[1] += GetMeasureScores(goal.Measures);
                                }
                                else
                                CurData[1] += " | " + GetMeasureScores(goal.Measures);
                            }
                        }
                    }

                }
                foreach (var item in baseline.ProcessedData)
                {
                    foreach (var goal in item.LessonPlans)
                    {
                        if (goal.LessonName != null && !ht.ContainsKey(goal.LessonName))
                        {
                            string[] marrayString = (string[])ht[goal.LessonName];
                            if (marrayString == null)
                            {
                                marrayString = new string[4];
                            }
                            var tempVar = GetMeasureScores(goal.Measures);
                            if (tempVar != "")
                            {
                                if (marrayString[0] == null)
                                {
                                    marrayString[0] += GetMeasureScores(goal.Measures);
                                }
                                else
                                marrayString[0] += " | " + GetMeasureScores(goal.Measures);
                            }
                        }
                        else
                        {
                            var CurData = (string[])ht[goal.LessonName];
                            var tempVar = GetMeasureScores(goal.Measures);
                            if (tempVar != "")
                            {
                                if (CurData[0] == null)
                                {
                                    CurData[0] += GetMeasureScores(goal.Measures);
                                }
                                else
                                CurData[0] += " | " + GetMeasureScores(goal.Measures);
                            }
                        }
                    }

                }

                GoalHashData objGoalHah = new GoalHashData();
                objGoalHah.goalHash = GoalHash;
                objGoalHah.LessonHash = ht;





                return objGoalHah;

            }

            return null;

        }

        public static GoalHashData getLEssonplanDataQuarter(int progressID, ProgressReport CRMonthlyItems)
        {
            ObjectiveClass objClass = null;
            ProgressReport returnModel = new ProgressReport();
            clsSession sess = (clsSession)HttpContext.Current.Session["UserSessionClient"];
            BiWeeklyRCPNewEntities dbobj = new BiWeeklyRCPNewEntities();

            // var CRMonthlyItems = new ProgressReport();

            if (progressID > 0)
            {
                //string date = "13.04.2012";
                //string date2 = (DateTime.Parse(date).AddDays(1)).ToShortDateString();

                DateTime dt1 = DateTime.ParseExact(CRMonthlyItems.RTF_Q_RptDate, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                DateTime dt2 = dt1.AddDays(-90);

                DateTime dt3 = dt2.AddDays(-1);
                DateTime dt4 = dt3.AddDays(-90);


                DateTime blStart = DateTime.ParseExact(CRMonthlyItems.RTF_Q_BLStart, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                DateTime blEnd = DateTime.ParseExact(CRMonthlyItems.RTF_Q_BLEnd, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                CRMonthlyItems.dateQ1 = dt1;
                CRMonthlyItems.dateQ2 = dt2;
                CRMonthlyItems.dateQ3 = dt3;
                CRMonthlyItems.dateQ4 = dt4;

                GoalModel baseline = new GoalModel();

                GoalModel objGoal1 = new GoalModel();
                GoalModel objGoal2 = new GoalModel();
                GoalModel objGoal3 = new GoalModel();


                CRMonthlyItems.baseline = baseline.Get_Goal_Lessons_Behavior(blStart, blEnd, sess.StudentId);
                CRMonthlyItems.gmodel2 = objGoal2.Get_Goal_Lessons_Behavior(dt4, dt3, sess.StudentId);//sess.StudentId, dt3, dt4
                CRMonthlyItems.gmodel3 = objGoal3.Get_Goal_Lessons_Behavior(dt2, dt1, sess.StudentId);//sess.StudentId, dt1, dt2

                //start
                Hashtable ht = new Hashtable();
                Hashtable GoalHash = new Hashtable();

                foreach (var item in objGoal3.ProcessedData)
                {
                    objClass = new ObjectiveClass();
                    if (!GoalHash.ContainsKey(item.GoalName))
                    {
                        //ArrayList LessonList = new ArrayList();

                        objClass.objectives = item.Objective1 + " " + item.Objective2 + " " + item.Objective3;
                        foreach (LessonPlanModel lp in item.LessonPlans)
                        {
                            if (!objClass.lessonaPlans.Contains(lp.LessonName))
                            {
                                objClass.lessonaPlans.Add(lp.LessonName);
                                objClass.Objective.Add(lp.Objective);
                                objClass.Objective.Add(lp.Objective);
                                objClass.BEnchmark.Add(lp.Baseline);
                            }

                        }

                        GoalHash.Add(item.GoalName, objClass);
                    }
                    else
                    {
                        //ArrayList LessonList = (ArrayList)GoalHash[item.GoalName];
                        foreach (LessonPlanModel lp in item.LessonPlans)
                        {
                            if (!objClass.lessonaPlans.Contains(lp.LessonName))
                                objClass.lessonaPlans.Add(lp.LessonName);
                        }
                    }

                }
                foreach (var item in objGoal3.ProcessedData)
                {
                    foreach (var goal in item.LessonPlans)
                    {//here
                        //string[] marrayString = new string[4];
                        if (goal.LessonName != null && !ht.ContainsKey(goal.LessonName))
                        {
                            string[] marrayString = new string[4];
                            marrayString[3] = GetMeasureScores(goal.Measures);
                            ht.Add(goal.LessonName, marrayString);
                        }
                        else
                        {
                            var CurData=(string[])ht[goal.LessonName];
                            //bd
                            //for(int i=CurData.Length-1;i>=0;i--)
                            //{
                            //    if (CurData[i] == null)
                            //    {
                            //        CurData[i] = GetMeasureScores(goal.Measures);
                            //        break;
                            //    }
                            //}
                            var tempVar = GetMeasureScores(goal.Measures);
                            if (tempVar != "")
                            CurData[3] += " | " + GetMeasureScores(goal.Measures);


                            //marrayString[2] = GetMeasureScores(goal.Measures);
                        }
                        //ht.Add(goal.LessonName, marrayString);
                    }

                }

                foreach (var item in objGoal2.ProcessedData)
                {
                    foreach (var goal in item.LessonPlans)
                    {
                        if (goal.LessonName != null && !ht.ContainsKey(goal.LessonName))
                        {
                            string[] marrayString = (string[])ht[goal.LessonName];
                            if (marrayString == null)
                            {
                                marrayString = new string[4];
                            }
                            var tempVar = GetMeasureScores(goal.Measures);
                            if (tempVar != "")
                            {
                                if (marrayString[2] == null)
                                {
                                    marrayString[2] += GetMeasureScores(goal.Measures);
                                }
                                else
                                    marrayString[2] += " | " + GetMeasureScores(goal.Measures);
                            }
                        }
                        else
                        {
                            var CurData = (string[])ht[goal.LessonName];
                            var tempVar = GetMeasureScores(goal.Measures);
                            if (tempVar != "")
                            {
                                if (CurData[2] == null)
                                {
                                    CurData[2] += GetMeasureScores(goal.Measures);
                                }
                                else
                                CurData[2] += " | " + GetMeasureScores(goal.Measures);
                            }
                        }
                    }

                }
                foreach (var item in baseline.ProcessedData)
                {
                    foreach (var goal in item.LessonPlans)
                    {
                        if (goal.LessonName != null && !ht.ContainsKey(goal.LessonName))
                        {
                            string[] marrayString = (string[])ht[goal.LessonName];
                            if (marrayString == null)
                            {
                                marrayString = new string[4];
                            }
                            var tempVar = GetMeasureScores(goal.Measures);
                            if (tempVar != "")
                            {
                                if (marrayString[0] == null)
                                {
                                    marrayString[0] += GetMeasureScores(goal.Measures);
                                }
                                else
                                marrayString[0] += " | " + GetMeasureScores(goal.Measures);
                            }
                        }
                        else
                        {
                            var CurData = (string[])ht[goal.LessonName];
                            var tempVar = GetMeasureScores(goal.Measures);
                            if (tempVar != "")
                            {
                                if (CurData[0] == null)
                                {
                                    CurData[0] += GetMeasureScores(goal.Measures);
                                }
                                else
                                CurData[0] += " | " + GetMeasureScores(goal.Measures);
                            }
                        }
                    }

                }

                //end

                //Hashtable ht = new Hashtable();

                //Hashtable GoalHash = new Hashtable();

                //foreach (var item in objGoal3.ProcessedData)
                //{
                //    if (!GoalHash.ContainsKey(item.GoalName))
                //    {
                //        ArrayList LessonList = new ArrayList();
                //        foreach (LessonPlanModel lp in item.LessonPlans)
                //        {
                //            if (!LessonList.Contains(lp.LessonName))
                //                LessonList.Add(lp.LessonName);
                //        }

                //        GoalHash.Add(item.GoalName, LessonList);
                //    }
                //    else
                //    {
                //        ArrayList LessonList = (ArrayList)GoalHash[item.GoalName];
                //        foreach (LessonPlanModel lp in item.LessonPlans)
                //        {
                //            if (!LessonList.Contains(lp.LessonName))
                //                LessonList.Add(lp.LessonName);
                //        }
                //    }

                //}
                //foreach (var item in objGoal3.ProcessedData)
                //{
                //    foreach (var goal in item.LessonPlans)
                //    {
                //        if (goal.LessonName != null && !ht.ContainsKey(goal.LessonName))
                //        {
                //            string[] marrayString = new string[4];
                //            marrayString[3] = GetMeasureScores(goal.Measures);
                //            ht.Add(goal.LessonName, marrayString);
                //        }
                //    }

                //}

                //foreach (var item in objGoal2.ProcessedData)
                //{
                //    foreach (var goal in item.LessonPlans)
                //    {
                //        if (goal.LessonName != null && ht.ContainsKey(goal.LessonName))
                //        {
                //            string[] marrayString = (string[])ht[goal.LessonName];
                //            marrayString[2] = GetMeasureScores(goal.Measures);

                //        }
                //    }

                //}

                //foreach (var item in baseline.ProcessedData)
                //{
                //    foreach (var goal in item.LessonPlans)
                //    {
                //        if (goal.LessonName != null && ht.ContainsKey(goal.LessonName))
                //        {
                //            string[] marrayString = (string[])ht[goal.LessonName];
                //            marrayString[0] = GetMeasureScores(goal.Measures);

                //        }
                //    }

                //}

                GoalHashData objGoalHah = new GoalHashData();
                objGoalHah.goalHash = GoalHash;
                objGoalHah.LessonHash = ht;





                return objGoalHah;

            }

            return null;

        }




        public static string GetMeasureScores(List<MeasureModel> measureList)
        {
            string sScore = "";
            foreach (MeasureModel mm in measureList)
            {
                if (mm.MeasureType == null) continue;

                if (mm.MeasureType.Contains("Duration"))
                {
                    if(mm.Score!=null)
                    sScore += mm.Score + " " + "ADD ";
                }
                else if (mm.MeasureType.Contains("Frequency"))
                {
                    if (mm.Score != null)
                    sScore += mm.Score + " " + "ADF ";
                }
                else if (mm.MeasureType.Contains("Accuracy") || mm.MeasureType.Contains("Prompted") || mm.MeasureType.Contains("Independent"))
                {
                    if (mm.Score != null)
                    sScore += mm.Score + " " + "ADP ";
                }
                else
                {
                    if (mm.Score != null)
                    sScore += mm.Score + " " + "ADS ";
                }
            }
            return sScore;
        }

        private string replaceWithTexts(string HtmlData, string[] plcT, string[] TextT)
        {
            int count = plcT.Count();

            for (int i = 0; i < count; i++)
            {
                if (TextT[i] != null)
                {
                    HtmlData = HtmlData.Replace(plcT[i], TextT[i]);
                }
                else
                {
                    HtmlData = HtmlData.Replace(plcT[i], "");
                }
            }
            return HtmlData;
        }

        private void ExportToWord(string htmlData, string NewPath)
        {
            try
            {
                // StringBuilder strBody = new StringBuilder();
                // strBody.Append(htmlData);
                // Context.Response.AppendHeader("Content-Type", "application/msword");
                // Context.Response.AppendHeader("Content-disposition", "attachment; filename=IEPDocument.doc");
                //Context.Response.Write(strBody);


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void PageConvert(string input, string output, WdSaveFormat format)
        {
            try
            {
                // Create an instance of Word.exe
                Microsoft.Office.Interop.Word._Application oWord = new Microsoft.Office.Interop.Word.Application();

                // Make this instance of word invisible (Can still see it in the taskmgr).
                oWord.Visible = false;

                // Interop requires objects.
                object oMissing = System.Reflection.Missing.Value;
                object isVisible = true;
                object readOnly = false;
                object oInput = input;
                object oOutput = output;
                object oFormat = format;
                object oFileShare = true;

                // Load a document into our instance of word.exe
                Microsoft.Office.Interop.Word._Document oDoc = oWord.Documents.Open(ref oInput, ref oMissing, ref readOnly, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref isVisible, ref oMissing, ref oMissing, ref oMissing, ref oMissing);

                // Make this document the active document.
                oDoc.Activate();

                // Save this document in Word 2003 format.
                oDoc.SaveAs(ref oOutput, ref oFormat, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing);

                // Always close Word.exe.
                oWord.Quit(ref oMissing, ref oMissing, ref oMissing);

                using (var fs = new FileStream(output, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    fs.Close();
                }
            }
            catch (IOException ex)
            {
                throw ex;
            }
        }

        private void CreateQuery1(string StateName, string Path, out string[] plcT, out string[] TextT, bool check, clsSession sess)
        {

            TextT = new string[0];
            plcT = new string[0];
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(Server.MapPath(Path));

            XmlNodeList xmlList = null;
            xmlList = xmlDoc.GetElementsByTagName("State");
            foreach (XmlNode st in xmlList)
            {
                if (st.Attributes["Name"].Value == StateName)
                {
                    XmlNodeList xmlListColumns = null;
                    xmlListColumns = st.ChildNodes.Item(0).ChildNodes;

                    int textCount = 0, chkCount = 0;

                    foreach (XmlNode stMs in xmlListColumns)
                    {
                        if (stMs.Attributes["PlaceHolder"].Value == "abcdefgh")
                        {
                            chkCount++;
                        }
                        else
                        {
                            textCount++;
                        }

                    }
                    TextT = new string[textCount];
                    plcT = new string[textCount];
                    columns = getColumns(textCount, sess);
                    int j = 0, k = 0, l = 0;
                    if (check == true)
                    {
                        foreach (XmlNode stMs in xmlListColumns)
                        {

                            TextT[k] = columns[l];
                            plcT[k] = stMs.Attributes["PlaceHolder"].Value.ToString().Trim();
                            k++;
                            l++;

                        }
                    }
                    columns = null;
                }

            }

        }
        private string CopyTemplate(string oldPath, string PageNo)
        {
            string Time = DateTime.Now.TimeOfDay.ToString();
            string[] ar = Time.Split('.');
            Time = ar[0];
            Time = Time.Replace(":", "-");
            string Datet = DateTime.Today.Day.ToString() + "-" + DateTime.Today.Month + "-" + DateTime.Today.Year.ToString() + "-" + Time;

            string path = Server.MapPath("~\\Administration") + "\\TempPR1";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            Guid g;
            g = Guid.NewGuid();
            sess = (clsSession)Session["UserSessionClient"];
            string newpath = path + "\\";
            string ids = g.ToString();
            ids = ids.Replace("-", "");
            string newFileName = "PRTemporyTemplate" + ids.ToString();
            FileInfo f1 = new FileInfo(oldPath);
            if (f1.Exists)
            {
                if (!Directory.Exists(newpath))
                {
                    Directory.CreateDirectory(newpath);
                }
                f1.CopyTo(string.Format("{0}{1}{2}", newpath, newFileName, f1.Extension));
            }
            return newpath + newFileName + f1.Extension;
        }

        private string CopyTemplateExport(string oldPath, string PageNo)
        {
            try
            {
                string Time = DateTime.Now.TimeOfDay.ToString();
                string[] ar = Time.Split('.');
                Time = ar[0];
                Time = Time.Replace(":", "-");
                string Datet = DateTime.Today.Day.ToString() + "-" + DateTime.Today.Month + "-" + DateTime.Today.Year.ToString() + "-" + Time;

                string path = Server.MapPath("~\\Templates") + "\\Progress";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                Guid g;

                g = Guid.NewGuid();

                sess = (clsSession)Session["UserSessionClient"];
                string newpath = path + "\\";
                string ids = g.ToString();
                ids = ids.Replace("-", "");

                string newFileName = "ProgressReport" + ids.ToString();
                FileInfo f1 = new FileInfo(oldPath);
                if (f1.Exists)
                {
                    if (!Directory.Exists(newpath))
                    {
                        Directory.CreateDirectory(newpath);
                    }

                    f1.CopyTo(string.Format("{0}{1}{2}", newpath, newFileName, f1.Extension));
                }
                return newpath + newFileName + f1.Extension;
            }
            catch (Exception Ex)
            {
                return "";

            }


        }



        private string[] getColumns(int Count, clsSession sess)
        {
            string[] colum = new string[13];

            sess = (clsSession)Session["UserSessionClient"];
            clsExport objExport = new clsExport();
            Progress tempz = new Progress();
            //clsExport tempz = objExport.CLientsReport1(sess);
            ParentServiceClient parentService = new ParentServiceClient();
            int SchoolID = sess.SchoolId;
            int StudentID = sess.StudentId;
            // clsExport tempz = new clsExport();
            tempz.ProgressInfo = parentService.CLientsReport(SchoolID, StudentID, 0);


            ClientDB.Models.ProgressDetails objProgress = new ClientDB.Models.ProgressDetails();

            int IEPId = tempz.ProgressInfo.stdtIEPId;
            objProgress.SclDistrictName = tempz.ProgressInfo.SclDistrictName;
            objProgress.DOB = tempz.ProgressInfo.DOB;
            objProgress.IEPDtFrom = tempz.ProgressInfo.IEPDtFrom;
            objProgress.IEPDtTo = tempz.ProgressInfo.IEPDtTo;
            objProgress.StudentName = tempz.ProgressInfo.StudentName;
            objProgress.SclDistrictAddress = tempz.ProgressInfo.SclDistrictAddress;
            objProgress.SclDistrictContact = tempz.ProgressInfo.SclDistrictContact;
            objProgress.SclDistrictName = tempz.ProgressInfo.SclDistrictName;
            objProgress.ID = tempz.ProgressInfo.ID;

            foreach (var GL in tempz.ProgressInfo.GoalDt)
            {
                ClientDB.Models.GoalData Goal = new ClientDB.Models.GoalData();
                ClientDB.Models.GridListPlacement PlcacementList = new ClientDB.Models.GridListPlacement();
                ClientDB.Models.ReportInfo ReportInfoRptList = new ClientDB.Models.ReportInfo();
                ClientDB.Models.ReportDetails RDet = new ClientDB.Models.ReportDetails();

                Goal.Goalid = GL.Goalid;
                Goal.GoalName = GL.GoalName;
                Goal.LessonplanName = GL.LessonplanName;

                foreach (var PL in GL.PlcacementList)
                {
                    PlcacementList.objective1 = PL.objective1;
                    PlcacementList.objective2 = PL.objective2;
                    PlcacementList.objective3 = PL.objective3;


                }
                Goal.PlcacementList.Add(PlcacementList);
                foreach (var RP in GL.RptList)
                {
                    ReportInfoRptList.rptid = RP.rptid;
                    ReportInfoRptList.rptdate = (DateTime)RP.rptdate;
                    ReportInfoRptList.rptinfo = RP.rptinfo;
                    ReportInfoRptList.goalid = RP.goalid;
                    ReportInfoRptList.stdtIEPId = RP.stdtIEPId;
                    Goal.RptList.Add(ReportInfoRptList);
                }
                foreach (var RP in GL.ProgressInfo)
                {
                    RDet.iepid = RP.iepid;
                    RDet.rptdate = RP.rptdate;
                    RDet.startdate = RP.startdate;
                    RDet.enddate = RP.enddate;
                    RDet.Status = RP.Status;
                    Goal.ReportDetails.Add(RDet);
                }


                objProgress.GoalDt.Add(Goal);

            }







            colum[0] = tempz.ProgressInfo.SclDistrictName;
            colum[1] = tempz.ProgressInfo.SclDistrictAddress;
            colum[2] = tempz.ProgressInfo.SclDistrictContact;
            colum[3] = tempz.ProgressInfo.IEPDtFrom.ToString("MM/dd/yyyy").Replace('-', '/');
            colum[4] = tempz.ProgressInfo.IEPDtTo.ToString("MM/dd/yyyy").Replace('-', '/');
            colum[5] = tempz.ProgressInfo.StudentName.ToString();
            colum[6] = tempz.ProgressInfo.DOB.ToString("MM/dd/yyyy").Replace('-', '/');
            colum[7] = tempz.ProgressInfo.ID.ToString();
            colum[8] = tempz.ProgressInfo.StudentName.ToString();
            colum[9] = tempz.ProgressInfo.DOB.ToString("MM/dd/yyyy").Replace('-', '/');
            colum[10] = tempz.ProgressInfo.ID.ToString();
            colum[11] = tempz.ProgressInfo.IEPDtFrom.ToString("MM/dd/yyyy").Replace('-', '/');
            colum[12] = tempz.ProgressInfo.IEPDtTo.ToString("MM/dd/yyyy").Replace('-', '/');



            return colum;

        }

        public string getHtmlStringProgress(clsSession sess)
        {
            sess = (clsSession)Session["UserSessionClient"];
            clsExport objExport = new clsExport();
            Progress tempz = new Progress();
            ParentServiceClient parentService = new ParentServiceClient();
            int SchoolID = sess.SchoolId;
            int StudentID = sess.StudentId;
            // clsExport tempz = new clsExport();
            tempz.ProgressInfo = parentService.CLientsReport(SchoolID, StudentID, 0);
            string HTML3 = "";
            int count = 0;
            foreach (var item in tempz.ProgressInfo.GoalDt)
            {
                int tmp = count;
                // count = 1;
                HTML3 += "<div id='divgrid' style='float: left; width: 100%'> <hr style='width: 100%; border: 5px solid;'/><br />" +
                    "<table id='tblDatedetails' style='width: 100%'><tr><td><h1 style='color: black'>Progress Report</h1></td>" +
                "<td>on IEP Dated: from " + tempz.ProgressInfo.IEPDtFrom.ToString("MM/dd/yyyy").Replace('-', '/') + "</td><td>to &nbsp;" + tempz.ProgressInfo.IEPDtTo.ToString("MM/dd/yyyy").Replace('-', '/') + "</td></tr></table><hr style='width:100%' border:'solid'/>" +
                 "<table id='tblStudentDetails' style='width: 100%'><tr><td class='tdLabel'>Student Name:" + tempz.ProgressInfo.StudentName + "</td>" +
                "<td class='tdLabel'>DOB:" + tempz.ProgressInfo.DOB.ToString("MM/dd/yyyy").Replace('-', '/') + "</td><td class='tdLabel'>ID #:" + tempz.ProgressInfo.ID + "</table>" +
                    "<div id='divInfo' style='width: 100%; height: 10%; background-color: black; color: white; font-size: 18px; text-align: center'>INFORMATION FROM CURRENT IEP</div>" +
                        "<table id='tblCurrentIEP' style='width: 100%; border: thick'><tr><td>Goal #:" + item.Goalid + "</td><td>Specific Goal Focus:" + item.LessonplanName + "</td></tr></table>";
                foreach (var itemsub in item.PlcacementList)
                {
                    HTML3 += "<br />Current Performance Level:&nbsp; What can the student currently do?<br /><ul><li><b>" + itemsub.objective1 + "</b></li></ul><br /><hr  style='width:100%' border:'solid' />" +
                             "Measurable Annual Goal:What challenging, yet attainable, goal can we expect the student to meet by the end of this IEP period?<br />How will we know that the student has reached this goal?<br />" +
                             "<ul><li><b>" + itemsub.objective2 + "</b></li></ul><br /><hr  style='width:100%' border:'solid' />Benchmark/Objectives: What will the student need to do to complete this goal?<br /><hr  style='width:100%' border:'solid' />" +
                             "<ul><li><b>" + itemsub.objective3 + "</b></li></ul><br /><br />";
                }

                HTML3 += "<div id='PrgRptInfo' style='width: 100%; height: 10%; background-color: black; color: white; font-size: 18px; text-align: center'>PROGRESS REPORT INFORMATION</div></br>";
                int index = 0;
                int qu = 1;
                // if (item.RptList.Count > 0)
                //{


                foreach (var itemsub1 in item.RptList)
                {

                    HTML3 += "<table class='Ctable' id='tblinfo' style='width: 100%; border: thick'><tr><hr style='width:100%' border:'solid' /><td>Progress Report Date:" + itemsub1.rptdate + "</td>" +
                "<td></td><td>Progress Report #" + qu + "of  4</td></tr><tr>" +
                "<td colspan='3'><hr  style='width:100%' border:'solid';font-size:18px />Progress Reports are required to be sent to parents at least as often as parents are informed of their non-disabled children’s progress. Each progress report must describe the student’s progress toward meeting each annual goal.<hr  style='width:100%' border:'solid' /></td>" +
            "</tr><tr><td colspan='3'>" + itemsub1.rptinfo + "</tr></table>";
                    index++; qu++;
                }



                //}
                // else
                //    {
                //        HTML3 +="<table class='Ctable' id='Table1' style='width: 100%; border: thick'><tr><td>Progress Report Date:"+item.+""+
                //    "</td><td></td><td>Progress Report # 1 of  4</td></tr><tr>"+
                //    "<td colspan='3'>Progress Reports are required to be sent to parents at least as often as parents are informed of their non-disabled children’s progress. Each progress report must describe the student’s progress toward meeting each annual goal.</td>"+
                //"</tr><tr><td colspan='3'>";
                //    }
            }

            HTML3 += "</td></tr></div></table>";
            return HTML3;

        }
        #region TableProperties

        static TableProperties GetTableBorderProperties()
        {
            //// Create the table properties
            TableProperties tblProperties = new TableProperties();

            //// Create Table Borders
            TableBorders tblBorders = new TableBorders();

            TopBorder topBorder = new TopBorder();
            topBorder.Val = new EnumValue<BorderValues>(BorderValues.Thick);
            topBorder.Color = "000000";
            tblBorders.AppendChild(topBorder);

            BottomBorder bottomBorder = new BottomBorder();
            bottomBorder.Val = new EnumValue<BorderValues>(BorderValues.Thick);
            bottomBorder.Color = "000000";
            tblBorders.AppendChild(bottomBorder);

            RightBorder rightBorder = new RightBorder();
            rightBorder.Val = new EnumValue<BorderValues>(BorderValues.Thick);
            rightBorder.Color = "000000";
            tblBorders.AppendChild(rightBorder);

            LeftBorder leftBorder = new LeftBorder();
            leftBorder.Val = new EnumValue<BorderValues>(BorderValues.Thick);
            leftBorder.Color = "000000";
            tblBorders.AppendChild(leftBorder);

            InsideHorizontalBorder insideHBorder = new InsideHorizontalBorder();
            insideHBorder.Val = new EnumValue<BorderValues>(BorderValues.Thick);
            insideHBorder.Color = "000000";
            tblBorders.AppendChild(insideHBorder);

            InsideVerticalBorder insideVBorder = new InsideVerticalBorder();
            insideVBorder.Val = new EnumValue<BorderValues>(BorderValues.Thick);
            insideVBorder.Color = "000000";
            tblBorders.AppendChild(insideVBorder);

            //// Add the table borders to the properties
            tblProperties.AppendChild(tblBorders);

            return tblProperties;
        }

        #endregion

        /*Function to export Quarterly RTF
         Hari 11-June-2015*/

        public string ExpAllProgressQuarter(clsSession sess, string path, int id)
        {
            BiWeeklyRCPNewEntities dbobj = new BiWeeklyRCPNewEntities();
            StudentPersonal sp = new StudentPersonal();
            ProgressReport pr = new ProgressReport();
            var CRMonthlyItems = new ProgressReport();
            CRMonthlyItems = (from a in dbobj.ProgressRpt_RTF_Q
                              where (a.RTFQId == id)
                              select new ProgressReport
                              {
                                  ProReportId = a.ProReportId,
                                  RTF_Q_BLStart = a.RTF_Q_BLStart,
                                  RTF_Q_BLEnd = a.RTF_Q_BLEnd,
                                  RTF_Q_RptDate = a.RTF_Q_RptDate,
                                  RTF_Q_TBehavior = a.RTF_Q_TBehavior,
                                  RTF_Q_Outlines = a.RTF_Q_Outlines,
                              }).First();
            GoalHashData objQuarter = getLEssonplanDataQuarter(id, CRMonthlyItems);

            string NewPath = CopyTemplateExport(path, "0");
            var basicDetails = (from m in dbobj.StudentPersonals
                                where (m.StudentPersonalId == sess.StudentId && m.SchoolId == sess.SchoolId)
                                select m).SingleOrDefault();
            pr.StudentName = basicDetails.FirstName + " " + basicDetails.LastName;
            pr.DateOfBirth = (basicDetails.BirthDate != null) ? ((DateTime)basicDetails.BirthDate).ToString("MM.dd.yyyy") : "";

            #region QuarterDate
            DateTime DateQ1 = DateTime.ParseExact(CRMonthlyItems.RTF_Q_RptDate.Replace('/', '.'), "MM.dd.yyyy", System.Globalization.CultureInfo.InvariantCulture);
            DateTime DateQ2 = DateQ1.AddDays(-90);
            DateTime DateQ3 = DateQ2.AddDays(-1);
            DateTime DateQ4 = DateQ3.AddDays(-90);
            #endregion

            using (WordprocessingDocument theDoc = WordprocessingDocument.Open(NewPath, true))
            {

                try
                {
                    MainDocumentPart mainPart = theDoc.MainDocumentPart;

                    SdtElement sdStudQuarter = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtStudQuarter").Single();
                    sdStudQuarter.InsertAfterSelf(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new Run(new Text(pr.StudentName))));
                    sdStudQuarter.Remove();

                    SdtElement sdRepo = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtRepoQuarter").Single();
                    sdRepo.InsertAfterSelf(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new Run(new Text(DateQ1.ToString("MM.dd.yyyy")))));
                    sdRepo.Remove();

                    SdtElement sdComp = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtRepoComp").Single();
                    sdComp.InsertAfterSelf(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new Run(new Text(sess.RoleName))));
                    sdComp.Remove();

                    SdtElement sdName = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "TextFirstPara").Single();

                    string firstPara = @"The following is a description and summary of behaviors targeted for deceleration and acceleration in " + pr.StudentName + @"'s Behavior Support Plan during the current quarter. The Behavior Support Plan is changed based on student progress and current behavior levels; therefore, details regarding the specific intervention procedures can be found in his current Behavior Support Plan document attached to this report";
                    sdName.InsertAfterSelf(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new Run(new Text(firstPara))));
                    sdName.Remove();

                    SdtElement sdContent = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtContentData").Single();
                    string contentData = @"Averages of the data collected from each day during the current academic quarter (" + DateQ2.ToString("MM/dd/yyyy").Replace('-', '.') + @"-" + DateQ1.ToString("MM/dd/yyyy").Replace('-', '.') + @") for each challenging behavior described above is compared to the averages from the previous academic quarter (" + DateQ4.ToString("MM/dd/yyyy").Replace('-', '.') + "-" + DateQ3.ToString("MM/dd/yyyy").Replace('-', '.') + @") in the table below:";
                    sdContent.InsertAfterSelf(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new Run(new Text(contentData))));
                    sdContent.Remove();

                    SdtElement sdDesc = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtDescription").Single();
                    sdDesc.InsertAfterSelf(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new Run(new Text(Convert.ToString(CRMonthlyItems.RTF_Q_TBehavior)))));
                    sdDesc.Remove();

                    Body body = new Body();
                    DocumentFormat.OpenXml.Wordprocessing.Table table = new DocumentFormat.OpenXml.Wordprocessing.Table();
                    DocumentFormat.OpenXml.Wordprocessing.Table table2 = new DocumentFormat.OpenXml.Wordprocessing.Table();
                    foreach (var item in objQuarter.goalHash.Keys)
                    {
                        var lesson = ((ObjectiveClass)objQuarter.goalHash[item]).lessonaPlans;
                        foreach (var item1 in lesson)
                        {

                            var doc = theDoc.MainDocumentPart.Document;

                            TableProperties tblPr = new TableProperties();
                            TableBorders tblBorders = new TableBorders();
                            tblBorders.TopBorder = new TopBorder();
                            tblBorders.TopBorder.Color = "000000";
                            tblBorders.BottomBorder = new BottomBorder();
                            tblBorders.BottomBorder.Color = "000000";
                            tblBorders.LeftBorder = new LeftBorder();
                            tblBorders.LeftBorder.Color = "000000";
                            tblBorders.RightBorder = new RightBorder();
                            tblBorders.RightBorder.Color = "000000";
                            tblBorders.InsideHorizontalBorder = new InsideHorizontalBorder();
                            tblBorders.InsideHorizontalBorder.Color = "000000";
                            tblBorders.InsideVerticalBorder = new InsideVerticalBorder();
                            tblBorders.InsideVerticalBorder.Color = "000000";
                            tblPr.Append(tblBorders);
                            table.Append(tblPr);
                            TableRow tr = new TableRow();
                            TableCell tc = new TableCell();
                            tc = new TableCell(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new Run(
                                               new Text(item1.ToString()))));
                            TableCellProperties tcp = new TableCellProperties();
                            GridSpan gridSpan = new GridSpan();
                            gridSpan.Val = 11;
                            tcp.Append(gridSpan);
                            tc.Append(tcp);
                            tr.Append(tc);
                            table.Append(tr);
                        }
                    }

                    string[] heading1 = new string[4];
                    heading1[0] = "Target Behavior";
                    heading1[1] = "Setting ";
                    heading1[2] = "Daily average from previous quarter (" + DateQ4.ToString("MM/dd/yyyy").Replace('-', '.') + "-" + DateQ3.ToString("MM/dd/yyyy").Replace('-', '.') + ")";
                    heading1[3] = "Daily average from current quarter (" + DateQ2.ToString("MM/dd/yyyy").Replace('-', '.') + "-" + DateQ1.ToString("MM/dd/yyyy").Replace('-', '.') + ")";
                    ArrayList scoreList = new ArrayList();

                    foreach (var item in objQuarter.goalHash.Keys)
                    {
                        foreach (var les in ((ObjectiveClass)objQuarter.goalHash[item]).lessonaPlans)
                        {
                            string[] score = new string[4];
                            score[0] = les.ToString();
                            score[1] = "School";
                            score[2] = ((string[])objQuarter.LessonHash[les])[1];
                            score[3] = ((string[])objQuarter.LessonHash[les])[0];
                            scoreList.Add(score);

                        }
                    }
                    table2 = GetGoalTableQuarter(table2, heading1, scoreList);

                    SdtElement s = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "findMe").Single();
                    s.InsertAfterSelf(table);

                    SdtElement sFind = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "findAgain").Single();
                    sFind.InsertAfterSelf(table2);
                    mainPart.Document.Save();
                }

                catch (Exception ex)
                {
                    throw ex;
                }

            }
            return NewPath;
        }

        

        static DocumentFormat.OpenXml.Wordprocessing.Table GetGoalTable(DocumentFormat.OpenXml.Wordprocessing.Table tlb1, string GoalName, string GoalObjective, string[] heading, ArrayList scorearray)
        {

            TableRow row1 = new TableRow();
            //Add the Goal Name
            TableCell cell1 = new TableCell(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new Run(new Text(GoalName))));
            row1.Append(cell1);
            //Goal Name Row
            tlb1.Append(row1);


            TableRow row12 = new TableRow();
            //Add the Goal Objective
            TableCell cell12 = new TableCell(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new Run(new Text(GoalObjective))));
            row12.Append(cell12);
            //Goal Objective Row
            tlb1.Append(row12);

            TableRow row13 = new TableRow();
            //Create the Cell to Hold the Inner Score Table
            TableCell cell13 = new TableCell();
            row13.Append(cell13);
            tlb1.Append(row13);


            //Build the Score Table
            DocumentFormat.OpenXml.Wordprocessing.Table tlb2 = new DocumentFormat.OpenXml.Wordprocessing.Table();
            //Set the boarders for the Inner Table to hold the scores
            tlb2.AppendChild(GetTableBorderProperties());


            //Heading for the Score table
            TableRow rowHeader = new TableRow();
            for (int i = 0; i < heading.Length; i++)
            {
                TableCell cell2 = new TableCell(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new Run(new Text(heading[i]))));
                rowHeader.Append(cell2);
            }

            tlb2.Append(rowHeader);
            //Heading

            //Build the Score Part for the Table
            foreach (object obj in scorearray)
            {
                TableRow row2 = new TableRow();
                string[] scores = (string[])obj;
                for (int i = 0; i < scores.Length; i++)
                {
                    TableCell cell2 = new TableCell(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new Run(new Text(scores[i]))));
                    row2.Append(cell2);
                }

                tlb2.Append(row2);
            }

            //Add the Score Table to the Outer Tables Row3 Cell1
            cell13.AppendChild(tlb2);
            //Append a Dummy Para 
            cell13.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Paragraph());

            TableRow row14 = new TableRow();
            //Add the Goal Objective
            TableCell cell14 = new TableCell(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new Run(new Text(" "))));
            row14.Append(cell14);
            //Goal Objective Row
            tlb1.Append(row14);

            //Return the Outer Table
            return tlb1;

        }

        static DocumentFormat.OpenXml.Wordprocessing.Table GetGoalTableQuarter(DocumentFormat.OpenXml.Wordprocessing.Table tlb1, string[] heading, ArrayList scorearray)
        {

            TableRow row13 = new TableRow();
            //Create the Cell to Hold the Inner Score Table
            TableCell cell13 = new TableCell();
            row13.Append(cell13);
            tlb1.Append(row13);


            //Build the Score Table
            DocumentFormat.OpenXml.Wordprocessing.Table tlb2 = new DocumentFormat.OpenXml.Wordprocessing.Table();
            //Set the boarders for the Inner Table to hold the scores
            tlb2.AppendChild(GetTableBorderProperties());


            //Heading for the Score table
            TableRow rowHeader = new TableRow();
            for (int i = 0; i < heading.Length; i++)
            {
                TableCell cell2 = new TableCell(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new Run(new Text(heading[i]))));
                rowHeader.Append(cell2);
            }

            tlb2.Append(rowHeader);
            //Heading

            //Build the Score Part for the Table
            foreach (object obj in scorearray)
            {
                TableRow row2 = new TableRow();
                string[] scores = (string[])obj;
                for (int i = 0; i < scores.Length; i++)
                {
                    TableCell cell2 = new TableCell(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new Run(new Text(scores[i]))));
                    row2.Append(cell2);
                }

                tlb2.Append(row2);
            }

            //Add the Score Table to the Outer Tables Row3 Cell1
            cell13.AppendChild(tlb2);
            //Append a Dummy Para 
            cell13.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Paragraph());

            TableRow row14 = new TableRow();
            //Add the Goal Objective
            TableCell cell14 = new TableCell(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new Run(new Text(" "))));
            row14.Append(cell14);
            //Goal Objective Row
            tlb1.Append(row14);

            //Return the Outer Table
            return tlb1;

        }
        /*Function to export Monthly RTF
         Hari 11-June-2015*/
        public string ExpAllProgressMonth(clsSession sess, string path, int id)
        {
            try
            {
                BiWeeklyRCPNewEntities dbobj = new BiWeeklyRCPNewEntities();
                StudentPersonal sp = new StudentPersonal();
                ProgressReport pr = new ProgressReport();
                var CRMonthlyItems = new ProgressReport();
                CRMonthlyItems = (from a in dbobj.ProgressRpt_RTF_M
                                  where (a.RTFMId == id)
                                  select new ProgressReport
                                  {
                                      RTF_M_BLStart = a.RTF_M_BLStart,
                                      RTF_M_BLEnd = a.RTF_M_BLEnd,
                                      RTF_M_RptDate = a.RTF_M_RptDate,
                                      RTF_M_BgInfo = a.RTF_M_BgInfo,
                                      RTF_M_BSPlan = a.RTF_M_BSPlan,
                                      RTF_M_Assessments = a.RTF_M_Assessments,
                                      RTF_M_CIntegration = a.RTF_M_CIntegration,
                                      RTF_M_CMedication = a.RTF_M_CMedication,
                                      RTF_M_DPlanning = a.RTF_M_DPlanning,
                                      RTF_M_ADSite = a.RTF_M_ADSite,
                                      RTF_M_ADStay = a.RTF_M_ADStay
                                  }).First();
                GoalHashData objHash = getLEssonplanData(id, CRMonthlyItems);

                string NewPath = CopyTemplateExport(path, "0");
                var basicDetails = (from m in dbobj.StudentPersonals
                                    where (m.StudentPersonalId == sess.StudentId && m.SchoolId == sess.SchoolId)
                                    select m).SingleOrDefault();
                pr.StudentName = basicDetails.FirstName + " " + basicDetails.LastName;
                pr.DateOfBirth = (basicDetails.BirthDate != null) ? ((DateTime)basicDetails.BirthDate).ToString("MM/dd/yyyy").Replace('-', '/') : "";

                //Calculate the Monthly dates
                DateTime rpttDate1 = DateTime.ParseExact(CRMonthlyItems.RTF_M_RptDate, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                DateTime rpttDate2 = rpttDate1.AddDays(-30);
                DateTime rpttDate3 = rpttDate2.AddDays(-1);
                DateTime rpttDate4 = rpttDate3.AddDays(30);
                DateTime rpttDate5 = rpttDate4.AddDays(-1);
                DateTime rpttDate6 = rpttDate5.AddDays(30);

                #region Monthly

                using (WordprocessingDocument theDoc = WordprocessingDocument.Open(NewPath, true))
                {


                    MainDocumentPart mainPart = theDoc.MainDocumentPart;
                    DocumentFormat.OpenXml.Wordprocessing.Table tlb1 = new DocumentFormat.OpenXml.Wordprocessing.Table();
                    Body body = new Body();

                    #region New

                    foreach (var item in objHash.goalHash.Keys)
                    {

                        //Heading Data
                        string[] heading = new string[5];
                        heading[0] = "Target Behavior";
                        heading[1] = "Baseline " + CRMonthlyItems.RTF_M_BLStart + "-" + CRMonthlyItems.RTF_M_BLEnd;
                        heading[2] = CRMonthlyItems.date6.ToString("MM/dd/yyyy").Replace('-', '/') + "-" + CRMonthlyItems.date5.ToString("MM/dd/yyyy").Replace('-', '/');
                        heading[3] = CRMonthlyItems.date4.ToString("MM/dd/yyyy").Replace('-', '/') + "-" + CRMonthlyItems.date3.ToString("MM/dd/yyyy").Replace('-', '/');
                        heading[4] = CRMonthlyItems.date2.ToString("MM/dd/yyyy").Replace('-', '/') + "-" + CRMonthlyItems.date1.ToString("MM/dd/yyyy").Replace('-', '/');
                        ArrayList scoreList = new ArrayList();

                        foreach (var lessonitem in ((ObjectiveClass)objHash.goalHash[item]).lessonaPlans)
                        {
                            string[] scores = new string[5];
                            scores[0] = lessonitem.ToString();
                            scores[1] = ((string[])objHash.LessonHash[lessonitem])[0];
                            scores[2] = ((string[])objHash.LessonHash[lessonitem])[1];
                            scores[3] = ((string[])objHash.LessonHash[lessonitem])[2];
                            scores[4] = ((string[])objHash.LessonHash[lessonitem])[3];

                            scoreList.Add(scores);

                        }
                        tlb1 = GetGoalTable(tlb1, item.ToString(), ((ObjectiveClass)objHash.goalHash[item]).objectives, heading, scoreList);
                    }
                    SdtElement sd1 = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "findMemonth").Single();
                    sd1.InsertAfterSelf(tlb1);

                    #endregion

                    SdtElement sdInfo = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtBackground").Single();
                    sdInfo.InsertAfterSelf(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new Run(new Text(CRMonthlyItems.RTF_M_BgInfo))));
                    sdInfo.Remove();

                    SdtElement sdPlan = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtBehavior").Single();
                    sdPlan.InsertAfterSelf(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new Run(new Text(CRMonthlyItems.RTF_M_BSPlan))));
                    sdPlan.Remove();

                    SdtElement sdAsmt = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtAssesment").Single();
                    sdAsmt.InsertAfterSelf(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new Run(new Text(CRMonthlyItems.RTF_M_Assessments))));
                    sdAsmt.Remove();

                    SdtElement sdInt = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtCommunity").Single();
                    sdInt.InsertAfterSelf(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new Run(new Text(CRMonthlyItems.RTF_M_CIntegration))));
                    sdInt.Remove();

                    SdtElement sdMed = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtCurntMed").Single();
                    sdMed.InsertAfterSelf(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new Run(new Text(CRMonthlyItems.RTF_M_CMedication))));
                    sdMed.Remove();

                    SdtElement sdSite = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtAnticipated").Single();
                    sdSite.InsertAfterSelf(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new Run(new Text(CRMonthlyItems.RTF_M_ADSite))));
                    sdSite.Remove();

                    SdtElement sdStay = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtDuration").Single();
                    sdStay.InsertAfterSelf(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new Run(new Text(CRMonthlyItems.RTF_M_ADStay))));
                    sdStay.Remove();

                    SdtElement sdOpen = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtOpenText").Single();
                    sdOpen.InsertAfterSelf(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new Run(new Text(CRMonthlyItems.RTF_M_DPlanning))));
                    sdOpen.Remove();

                    foreach (HeaderPart hpart in mainPart.HeaderParts)
                    {
                        SdtElement headIndi = hpart.Header.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "hdIndividual").SingleOrDefault();
                        if (headIndi != null)
                        {
                            headIndi.InsertAfterSelf(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new Run(new Text(pr.StudentName))));
                            headIndi.Remove();
                            break;
                        }
                    }

                    foreach (HeaderPart hpart in mainPart.HeaderParts)
                    {
                        SdtElement headIndi = hpart.Header.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "hdDate").SingleOrDefault();
                        if (headIndi != null)
                        {
                            headIndi.InsertAfterSelf(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new Run(new Text(pr.DateOfBirth))));
                            headIndi.Remove();
                            break;
                        }
                    }

                    foreach (HeaderPart hpart in mainPart.HeaderParts)
                    {
                        SdtElement headIndi = hpart.Header.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "hdMonthly").SingleOrDefault();
                        if (headIndi != null)
                        {
                            headIndi.InsertAfterSelf(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new Run(new Text(rpttDate2.ToString("MM/dd/yyyy").Replace('-', '/') + "-" + (rpttDate1.ToString("MM/dd/yyyy").Replace('-', '/'))))));
                            headIndi.Remove();
                            break;
                        }
                    }

                    foreach (HeaderPart hpart in mainPart.HeaderParts)
                    {
                        SdtElement headIndi = hpart.Header.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "hdIndividual2").SingleOrDefault();
                        if (headIndi != null)
                        {
                            headIndi.InsertAfterSelf(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new Run(new Text(pr.StudentName))));
                            headIndi.Remove();
                        }
                    }

                    foreach (HeaderPart hpart in mainPart.HeaderParts)
                    {
                        SdtElement headIndi = hpart.Header.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "hdDate2").SingleOrDefault();
                        if (headIndi != null)
                        {
                            headIndi.InsertAfterSelf(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new Run(new Text(pr.DateOfBirth))));
                            headIndi.Remove();
                        }
                    }

                    foreach (HeaderPart hpart in mainPart.HeaderParts)
                    {
                        SdtElement headIndi = hpart.Header.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "hdMonthly2").SingleOrDefault();
                        if (headIndi != null)
                        {
                            headIndi.InsertAfterSelf(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new Run(new Text(CRMonthlyItems.date1.ToString("MM/dd/yyyy").Replace('-', '/') + "-" + (CRMonthlyItems.date2.ToString("MM/dd/yyyy").Replace('-', '/'))))));
                            headIndi.Remove();
                        }
                    }

                    mainPart.Document.Save();

                }
                #endregion

                return NewPath;
            }
            catch (Exception e)
            {
                throw e;
            }

        }
    }
}

