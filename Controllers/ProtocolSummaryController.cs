using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ClientDB.Models;
using ClientDB.AppFunctions;
using ClientDB.DbModel;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System.IO;
using System.Net;
using DocumentFormat.OpenXml;


namespace ClientDB.Controllers
{
    public class ProtocolSummaryController : Controller
    {
        Other_Functions objFuns = null;
        public clsSession sess = null;

        //
        // GET: /ProtocolSummary/
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult Index()
        {
            ProtocolSummary model = new ProtocolSummary();
            model = FillSummary();


            Other_Functions of = new Other_Functions();
            ViewBag.permission = (of.setClientPermission("Protocol Summary") == 2) ? "true" : "false";


            if (model == null)
            {
                ViewBag.status = "save";
                return View();
            }
            else
            {
                ViewBag.status = "update";
                return View(model);
            }
        }

        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public void btnProto_Click()
        {
            sess = (clsSession)Session["UserSessionClient"];
            Protocol objP = new Protocol();
            StudentPersonal sp = new StudentPersonal();
            ProtocolSummary model = new ProtocolSummary();
            ClientRegistrationPAModel model1 = new ClientRegistrationPAModel();
            BiWeeklyRCPNewEntities dbobj = new BiWeeklyRCPNewEntities();
            StudentPersonalPA studentPA = new StudentPersonalPA();
            var pId1 = (from m in dbobj.Protocols
                        where (m.SchoolId == sess.SchoolId && m.StudentId == sess.StudentId)
                        select m.ProtocolId).ToList();

            var Details = (from m in dbobj.StudentPersonals
                           where (m.SchoolId == sess.SchoolId && m.StudentPersonalId == sess.StudentId)
                           select m).SingleOrDefault();

            model1.DateOfBirth = (Details.BirthDate != null) ? ((DateTime)Details.BirthDate).ToString("MM.dd.yyyy") : "";
            model1.FirstName = Details.FirstName + " " + Details.LastName;


            if (pId1.Count > 0)
            {
                int pId = pId1[0];

                var SummaryItems = (from x in dbobj.Protocols
                                    //where (x.SchoolId==sess.SchoolId && x.StudentId==sess.StudentId)
                                    where (x.ProtocolId == pId)
                                    select new ProtocolSummary
                                    {
                                        HomeCommon = x.HomeCommon,
                                        HomeBedroom = x.HomeBedroom,
                                        HomeBathroom = x.HomeBathroom,
                                        Campus = x.Campus,
                                        Community = x.Community,
                                        SchoolCommon = x.SchoolCommon,
                                        SchoolBathroom = x.SchoolBathroom,
                                        SchoolOutside = x.SchoolOutside,
                                        Pool = x.Pool,
                                        Van = x.Van,

                                        MasteredTask = x.MasteredTask,
                                        NewTask = x.NewTask,

                                        Allergies = x.Allergies,
                                        SeizureInfo = x.SeizureInfo,
                                        MedTimes = x.MedTimes,
                                        TakeMed = x.TakeMed,
                                        OtherMedical = x.OtherMedical,

                                        DoctorVisit = x.DoctorVisit,
                                        Dental = x.Dental,
                                        BloodWork = x.BloodWork,
                                        HairCuts = x.HairCuts,
                                        OtherBehave = x.OtherBehave,

                                        EatingGeneral = x.EatingGeneral,
                                        EatingAble = x.EatingAble,
                                        EatingNeed = x.EatingNeed,
                                        EatingIep = x.EatingIep,

                                        ToiletingGeneral = x.ToiletingGeneral,
                                        ToiletingAble = x.ToiletingAble,
                                        ToiletingNeed = x.ToiletingNeed,
                                        ToiletingIep = x.ToiletingIep,

                                        BrushingGeneral = x.BrushingGeneral,
                                        BrushingAble = x.BrushingAble,
                                        BrushingNeed = x.BrushingNeed,
                                        BrushingIep = x.BrushingIep,

                                        HandGeneral = x.HandGeneral,
                                        HandAble = x.HandAble,
                                        HandNeed = x.HandNeed,
                                        HandIep = x.HandIep,

                                        DressGeneral = x.DressGeneral,
                                        DressAble = x.DressAble,
                                        DressNeed = x.DressNeed,
                                        DressIep = x.DressIep,

                                        ShowerGeneral = x.ShowerGeneral,
                                        ShowerAble = x.ShowerAble,
                                        ShowerNeed = x.ShowerNeed,
                                        ShowerIep = x.ShowerIep,

                                        BedTime = x.BedTime,

                                        Morning7 = x.Morning7,
                                        Morning715 = x.Morning715,
                                        Morning730 = x.Morning730,
                                        Morning745 = x.Morning745,
                                        Morning800 = x.Morning800,
                                        Morning815 = x.Morning815,
                                        Morning830 = x.Morning830,
                                        Morning845 = x.Morning845,
                                        Morning900 = x.Morning900,

                                        Noon330 = x.Noon330,
                                        Noon400 = x.Noon400,
                                        Noon430 = x.Noon430,
                                        Noon500 = x.Noon500,
                                        Noon530 = x.Noon530,
                                        Noon600 = x.Noon600,
                                        Noon630 = x.Noon630,
                                        Noon700 = x.Noon700,
                                        Noon730 = x.Noon730,
                                        Noon800 = x.Noon800,
                                        Noon830 = x.Noon830,
                                        Noon900 = x.Noon900,
                                        Noon930 = x.Noon930,
                                        Noon100 = x.Noon100,
                                        Noon10to11 = x.Noon10to11,
                                        Noon11to7 = x.Noon11to7,
                                        Leisure = x.Leisure,
                                        Modified1 = x.ModifiedOn,

                                        UpdtLOS = x.UpdtLOS,
                                        UpdtPageTop = x.UpdtTop,
                                        UpdtTPH = x.UpdtTPH,
                                        UpdtMedInfo = x.UpdtMedInfo,
                                        UpdtPerCare = x.UpdtPerCare,
                                        UpdtBehInfo = x.UpdtBehInfo,
                                        UpdtTypRoutines = x.UpdtTypRoutines,

                                        UpdtATList = x.UpdtAtList,
                                        UpdtCGList = x.UpdtCGList,
                                        UpdtFIList = x.UpdtFIList,
                                        UpdtBBIList = x.UpdtBBIList
                                    }).SingleOrDefault();

                SummaryItems.ATList = (from a in dbobj.Protocol_1_Assistive
                                       where (a.ProtocolId == pId)
                                       select new AssistTech
                                       {
                                           Type = a.Type,
                                           ScheduleForUse = a.ScheduleForUse,
                                           StorageLocation = a.StorageLocation,
                                           Modified2 = a.ModifiedOn,
                                           AssistiveId = a.AssistiveId
                                           //UpdtATList=a.UpdtATList
                                       }).ToList();

                SummaryItems.CGList = (from b in dbobj.Protocol_2_Community
                                       where (b.ProtocolId == pId)
                                       select new CommGuide
                                       {
                                           TypeA = b.TypeA,
                                           TypeB = b.TypeB,
                                           Modified3 = b.ModifiedOn,
                                           CommunityId = b.CommunityId
                                           //UpdtCGList=b.UpdtCGList
                                       }).ToList();

                SummaryItems.FIList = (from c in dbobj.Protocol_3_Family
                                       where (c.ProtocolId == pId)
                                       select new FamilyInfo
                                       {
                                           FamilyOne = c.FamilyOne,
                                           FamilyTwo = c.FamilyTwo,
                                           Modified4 = c.ModifiedOn,
                                           FamilyId = c.FamilyId
                                           //UpdtFIList=c.UpdtFIList
                                       }).ToList();

                SummaryItems.BBIList = (from d in dbobj.Protocol_4_Basic
                                        where (d.ProtocolId == pId)
                                        select new BasicBehavInfo
                                        {
                                            Acceleration = d.Acceleration,
                                            Strategy = d.Strategy,
                                            Modified5 = d.ModifiedOn,
                                            BasicId = d.BasicId
                                            //UpdtBBIList=d.UpdtBBIList
                                        }).ToList();

                SummaryItems.SignList = (from e in dbobj.Protocol_5_Signature
                                         where (e.ProtocolId == pId)
                                         select new SignatureLi
                                         {
                                             SignatureId = e.SignatureId,
                                             PrintName = e.PrintName,
                                             Signature = e.Signature,
                                             Date = e.Date,
                                             Modified6 = e.ModifiedOn
                                         }).ToList();
                model = SummaryItems;
            }
            int ClientId = Convert.ToInt32(Session["TempStudentId"]);
            studentPA = dbobj.StudentPersonalPAs.Where(objStudentPA => objStudentPA.StudentPersonalId == ClientId && objStudentPA.SchoolId == sess.SchoolId).SingleOrDefault();
            if (studentPA != null)
            {
                model.Allergies = studentPA.Allergies;
                model.SeizureInfo = studentPA.Seizures;
            }

            string path = Server.MapPath("~\\Templates\\Protocol.docx");
            string NewPath = CopyTemplate(path, "0");
            using (WordprocessingDocument theDoc = WordprocessingDocument.Open(NewPath, true))
            {
                try
                {
                    MainDocumentPart mainPart = theDoc.MainDocumentPart;

                    // This should return only one content control element: the one with 
                    // the specified tag value.
                    // If not, "Single()" throws an exception.


                    // This should return only one table.

                    SdtElement headDOB = mainPart.HeaderParts.First().Header.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtHeadDOB").SingleOrDefault();
                    headDOB.InsertAfterSelf(new Paragraph(new Run(new Text("DOB:" + model1.DateOfBirth))));
                    headDOB.Remove();

                    SdtElement headName = mainPart.HeaderParts.First().Header.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtHeadName").SingleOrDefault();
                    headName.InsertAfterSelf(new Paragraph(new Run(new Text("Student:" + model1.FirstName))));
                    headName.Remove();

                    SdtElement headUp = mainPart.HeaderParts.First().Header.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtHeadUp").SingleOrDefault();
                    headUp.InsertAfterSelf(new Paragraph(new Run(new Text("Updated:" + model.Modified1))));
                    headUp.Remove();

                    SdtElement blockUp = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtUpdatedLevel").Single();
                    blockUp.InsertAfterSelf(new Paragraph(new Run(new Text(model.UpdtLOS))));
                    blockUp.Remove();

                    SdtElement block = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "plcCommon").Single();
                    block.InsertAfterSelf(new Paragraph(new Run(new Text(model.HomeCommon))));
                    block.Remove();

                    SdtElement blockUpTop = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtUpTop").Single();
                    blockUpTop.InsertAfterSelf(new Paragraph(new Run(new Text(model.UpdtPageTop))));
                    blockUpTop.Remove();

                    SdtElement block1 = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtBedroom").Single();
                    block1.InsertAfterSelf(new Paragraph(new Run(new Text(model.HomeBedroom))));
                    block1.Remove();

                    SdtElement block2 = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtHomeBathroom").Single();
                    block2.InsertAfterSelf(new Paragraph(new Run(new Text(model.HomeBathroom))));
                    block2.Remove();

                    SdtElement block3 = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtCampus").Single();
                    block3.InsertAfterSelf(new Paragraph(new Run(new Text(model.Campus))));
                    // block3.Remove();

                    SdtElement block4 = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtCommunity").Single();
                    block4.InsertAfterSelf(new Paragraph(new Run(new Text(model.Community))));
                    block4.Remove();

                    SdtElement block5 = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtSchool").Single();
                    block5.InsertAfterSelf(new Paragraph(new Run(new Text(model.SchoolCommon))));
                    block5.Remove();

                    SdtElement block6 = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtScBath").Single();
                    block6.InsertAfterSelf(new Paragraph(new Run(new Text(model.SchoolBathroom))));
                    block6.Remove();

                    SdtElement block7 = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtSchoolTransitive").Single();
                    block7.InsertAfterSelf(new Paragraph(new Run(new Text(model.SchoolOutside))));
                    block7.Remove();

                    SdtElement block8 = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtPool").Single();
                    block8.InsertAfterSelf(new Paragraph(new Run(new Text(model.Pool))));
                    block8.Remove();

                    SdtElement block9 = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtVanTag").Single();
                    block9.InsertAfterSelf(new Paragraph(new Run(new Text(model.Van))));
                    block9.Remove();

                    SdtElement blockAs = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtUpdatedAssis").Single();
                    blockAs.InsertAfterSelf(new Paragraph(new Run(new Text(model.UpdtATList))));
                    blockAs.Remove();

                    SdtElement blockPer = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtUpdatedPer").Single();
                    blockPer.InsertAfterSelf(new Paragraph(new Run(new Text(model.UpdtPerCare))));
                    blockPer.Remove();

                    SdtElement blockFam = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtUpdatedFamily").Single();
                    blockFam.InsertAfterSelf(new Paragraph(new Run(new Text(model.UpdtFIList))));
                    blockFam.Remove();


                    SdtElement blockTyp = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtUpdatedTypical").Single();
                    blockTyp.InsertAfterSelf(new Paragraph(new Run(new Text(model.UpdtTypRoutines))));
                    blockTyp.Remove();

                    SdtElement blockCom = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtUpdatedCom").Single();
                    blockCom.InsertAfterSelf(new Paragraph(new Run(new Text(model.UpdtCGList))));
                    blockCom.Remove();

                    SdtElement blockBeh = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtUpdatedBeh").Single();
                    blockBeh.InsertAfterSelf(new Paragraph(new Run(new Text(model.UpdtBehInfo))));
                    blockBeh.Remove();

                    if (model.ATList != null)
                    {
                        if (model.ATList.Count > 0)
                        {
                            SdtBlock ccWithTable = mainPart.Document.Body.Descendants<SdtBlock>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "tblAssistive").Single();
                            DocumentFormat.OpenXml.Wordprocessing.Table theTable = ccWithTable.Descendants<DocumentFormat.OpenXml.Wordprocessing.Table>().Single();
                            DocumentFormat.OpenXml.Wordprocessing.TableRow theRow = theTable.Elements<DocumentFormat.OpenXml.Wordprocessing.TableRow>().Last();
                            foreach (var item in model.ATList)
                            {
                                DocumentFormat.OpenXml.Wordprocessing.TableRow rowCopy = (DocumentFormat.OpenXml.Wordprocessing.TableRow)theRow.CloneNode(true);
                                rowCopy.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(0).Append(new Paragraph
                                    (new Run(new Text(item.Type))));
                                rowCopy.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(1).Append(new Paragraph
                                    (new Run(new Text(item.ScheduleForUse))));
                                rowCopy.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(2).Append(new Paragraph
                                    (new Run(new Text(item.StorageLocation))));
                                theTable.AppendChild(rowCopy);
                            }
                            theTable.RemoveChild(theRow);
                        }
                    }

                    SdtElement blockBas = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtUpdatedBasic").Single();
                    blockBas.InsertAfterSelf(new Paragraph(new Run(new Text(model.UpdtBBIList))));
                    blockBas.Remove();

                    SdtElement blockUpTyp = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtUpdatedTyp").Single();
                    blockUpTyp.InsertAfterSelf(new Paragraph(new Run(new Text(model.UpdtTPH))));
                    blockUpTyp.Remove();

                    SdtElement block10 = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtMastered").Single();
                    block10.InsertAfterSelf(new Paragraph(new Run(new Text(model.MasteredTask))));
                    block10.Remove();

                    SdtElement block11 = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtNewTag").Single();
                    block11.InsertAfterSelf(new Paragraph(new Run(new Text(model.NewTask))));
                    block11.Remove();

                    SdtElement blockMed = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtUpdatedMed").Single();
                    blockMed.InsertAfterSelf(new Paragraph(new Run(new Text(model.UpdtMedInfo))));
                    blockMed.Remove();

                    SdtElement block12 = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtAllergyes").Single();
                    block12.InsertAfterSelf(new Paragraph(new Run(new Text(model.Allergies))));
                    block12.Remove();

                    SdtElement block13 = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtSeizure").Single();
                    block13.InsertAfterSelf(new Paragraph(new Run(new Text(model.SeizureInfo))));
                    block13.Remove();

                    SdtElement block14 = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtMedTag").Single();
                    block14.InsertAfterSelf(new Paragraph(new Run(new Text(model.MedTimes))));
                    block14.Remove();

                    SdtElement block15 = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtHowTag").Single();
                    block15.InsertAfterSelf(new Paragraph(new Run(new Text(model.TakeMed))));
                    block15.Remove();

                    SdtElement block16 = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtOtherTag").Single();
                    block16.InsertAfterSelf(new Paragraph(new Run(new Text(model.OtherMedical))));
                    block16.Remove();

                    SdtElement block17 = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtDoctor").Single();
                    block17.InsertAfterSelf(new Paragraph(new Run(new Text(model.DoctorVisit))));
                    block17.Remove();

                    SdtElement block18 = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtDental").Single();
                    block18.InsertAfterSelf(new Paragraph(new Run(new Text(model.Dental))));
                    block18.Remove();

                    SdtElement block19 = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtBloodTag").Single();
                    block19.InsertAfterSelf(new Paragraph(new Run(new Text(model.BloodWork))));
                    block19.Remove();

                    SdtElement block20 = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtHaircut").Single();
                    block20.InsertAfterSelf(new Paragraph(new Run(new Text(model.HairCuts))));
                    block20.Remove();

                    SdtElement block21 = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txt2Tag").Single();
                    block21.InsertAfterSelf(new Paragraph(new Run(new Text(model.OtherBehave))));
                    block21.Remove();

                    SdtElement block22 = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtGeneralEat").Single();
                    block22.InsertAfterSelf(new Paragraph(new Run(new Text(model.EatingGeneral))));
                    block22.Remove();

                    SdtElement block23 = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtAbleEat").Single();
                    block23.InsertAfterSelf(new Paragraph(new Run(new Text(model.EatingAble))));
                    block23.Remove();

                    SdtElement block24 = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtNeedEat").Single();
                    block24.InsertAfterSelf(new Paragraph(new Run(new Text(model.EatingNeed))));
                    block24.Remove();

                    SdtElement block25 = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtIepEat").Single();
                    block25.InsertAfterSelf(new Paragraph(new Run(new Text(model.EatingIep))));
                    block25.Remove();

                    SdtElement block26 = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtToGen").Single();
                    block26.InsertAfterSelf(new Paragraph(new Run(new Text(model.ToiletingGeneral))));
                    block26.Remove();

                    SdtElement block27 = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtIleAble").Single();
                    block27.InsertAfterSelf(new Paragraph(new Run(new Text(model.ToiletingAble))));
                    block27.Remove();

                    SdtElement block28 = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtHelpTo").Single();
                    block28.InsertAfterSelf(new Paragraph(new Run(new Text(model.ToiletingNeed))));
                    block28.Remove();

                    SdtElement block29 = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtIspTo").Single();
                    block29.InsertAfterSelf(new Paragraph(new Run(new Text(model.ToiletingIep))));
                    block29.Remove();

                    SdtElement block30 = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtBrushTag").Single();
                    block30.InsertAfterSelf(new Paragraph(new Run(new Text(model.BrushingGeneral))));
                    block30.Remove();

                    SdtElement block31 = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtIngTeeth").Single();
                    block31.InsertAfterSelf(new Paragraph(new Run(new Text(model.BrushingAble))));
                    block31.Remove();

                    SdtElement block32 = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtWithBrush").Single();
                    block32.InsertAfterSelf(new Paragraph(new Run(new Text(model.BrushingNeed))));
                    block32.Remove();

                    SdtElement block33 = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtGoalBrush").Single();
                    block33.InsertAfterSelf(new Paragraph(new Run(new Text(model.BrushingIep))));
                    block33.Remove();

                    SdtElement block34 = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "plcHandTag").Single();
                    block34.InsertAfterSelf(new Paragraph(new Run(new Text(model.HandGeneral))));
                    block34.Remove();

                    SdtElement block35 = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtWasAble").Single();
                    block35.InsertAfterSelf(new Paragraph(new Run(new Text(model.HandAble))));
                    block35.Remove();

                    SdtElement block36 = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtAshNeed").Single();
                    block36.InsertAfterSelf(new Paragraph(new Run(new Text(model.HandNeed))));
                    block36.Remove();

                    SdtElement block37 = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtPeiHand").Single();
                    block37.InsertAfterSelf(new Paragraph(new Run(new Text(model.HandIep))));
                    block37.Remove();

                    SdtElement block38 = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtDressTag").Single();
                    block38.InsertAfterSelf(new Paragraph(new Run(new Text(model.DressGeneral))));
                    block38.Remove();

                    SdtElement block39 = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtResAble").Single();
                    block39.InsertAfterSelf(new Paragraph(new Run(new Text(model.DressAble))));
                    block39.Remove();

                    SdtElement block40 = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtEssNeed").Single();
                    block40.InsertAfterSelf(new Paragraph(new Run(new Text(model.DressNeed))));
                    block40.Remove();

                    SdtElement block41 = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtPsiDress").Single();
                    block41.InsertAfterSelf(new Paragraph(new Run(new Text(model.DressIep))));
                    block41.Remove();

                    SdtElement block42 = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "ShowerGen").Single();
                    block42.InsertAfterSelf(new Paragraph(new Run(new Text(model.ShowerGeneral))));
                    block42.Remove();

                    SdtElement block43 = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtWerAble").Single();
                    block43.InsertAfterSelf(new Paragraph(new Run(new Text(model.ShowerAble))));
                    block43.Remove();

                    SdtElement block44 = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtNhwShower").Single();
                    block44.InsertAfterSelf(new Paragraph(new Run(new Text(model.ShowerNeed))));
                    block44.Remove();

                    SdtElement block45 = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtSlaShower").Single();
                    block45.InsertAfterSelf(new Paragraph(new Run(new Text(model.ShowerIep))));
                    block45.Remove();

                    SdtElement block46 = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtBedtime").Single();
                    block46.InsertAfterSelf(new Paragraph(new Run(new Text(model.BedTime))));
                    block46.Remove();

                    if (model.CGList != null)
                    {
                        if (model.CGList.Count > 0)
                        {
                            SdtBlock ccWithTableCom = mainPart.Document.Body.Descendants<SdtBlock>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "tblCommunity").Single();
                            DocumentFormat.OpenXml.Wordprocessing.Table theTableCom = ccWithTableCom.Descendants<DocumentFormat.OpenXml.Wordprocessing.Table>().Single();
                            DocumentFormat.OpenXml.Wordprocessing.TableRow theRowCom = theTableCom.Elements<DocumentFormat.OpenXml.Wordprocessing.TableRow>().Last();

                            foreach (var item in model.CGList)
                            {

                                DocumentFormat.OpenXml.Wordprocessing.TableRow rowCopy1 = (DocumentFormat.OpenXml.Wordprocessing.TableRow)theRowCom.CloneNode(true);
                                rowCopy1.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(0).Append(new Paragraph
                                    (new Run(new Text(item.TypeA))));
                                rowCopy1.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(1).Append(new Paragraph
                                    (new Run(new Text(item.TypeB))));
                                theTableCom.AppendChild(rowCopy1);
                            }
                            theTableCom.RemoveChild(theRowCom);
                        }
                    }
                    if (model.FIList != null)
                    {
                        if (model.FIList.Count > 0)
                        {

                            SdtBlock ccWithTableFam = mainPart.Document.Body.Descendants<SdtBlock>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "tblFamilyInfo").Single();
                            DocumentFormat.OpenXml.Wordprocessing.Table theTableFam = ccWithTableFam.Descendants<DocumentFormat.OpenXml.Wordprocessing.Table>().Single();
                            DocumentFormat.OpenXml.Wordprocessing.TableRow theRowFam = theTableFam.Elements<DocumentFormat.OpenXml.Wordprocessing.TableRow>().Last();
                            foreach (var item in model.FIList)
                            {
                                DocumentFormat.OpenXml.Wordprocessing.TableRow rowCopy2 = (DocumentFormat.OpenXml.Wordprocessing.TableRow)theRowFam.CloneNode(true);
                                rowCopy2.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(0).Append(new Paragraph
                                    (new Run(new Text(item.FamilyOne))));
                                rowCopy2.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(1).Append(new Paragraph
                                    (new Run(new Text(item.FamilyTwo))));
                                theTableFam.AppendChild(rowCopy2);
                            }
                            theTableFam.RemoveChild(theRowFam);
                        }
                    }
                    if (model.BBIList != null)
                    {
                        if (model.BBIList.Count > 0)
                        {

                            SdtBlock ccWithTableBasic = mainPart.Document.Body.Descendants<SdtBlock>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "tblBasicBehave").Single();
                            DocumentFormat.OpenXml.Wordprocessing.Table theTableBasic = ccWithTableBasic.Descendants<DocumentFormat.OpenXml.Wordprocessing.Table>().Single();
                            DocumentFormat.OpenXml.Wordprocessing.TableRow theRowBasic = theTableBasic.Elements<DocumentFormat.OpenXml.Wordprocessing.TableRow>().Last();
                            foreach (var item in model.BBIList)
                            {
                                DocumentFormat.OpenXml.Wordprocessing.TableRow rowCopy3 = (DocumentFormat.OpenXml.Wordprocessing.TableRow)theRowBasic.CloneNode(true);
                                rowCopy3.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(0).Append(new Paragraph
                                    (new Run(new Text(item.Acceleration))));
                                rowCopy3.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(1).Append(new Paragraph
                                    (new Run(new Text(item.Strategy))));
                                theTableBasic.AppendChild(rowCopy3);
                            }
                            theTableBasic.RemoveChild(theRowBasic);
                        }
                    }

                    SdtElement block47 = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtc72tag").Single();
                    block47.InsertAfterSelf(new Paragraph(new Run(new Text(model.Morning7))));
                    block47.Remove();

                    SdtElement block48 = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txt1572Tag").Single();
                    block48.InsertAfterSelf(new Paragraph(new Run(new Text(model.Morning715))));
                    block48.Remove();

                    SdtElement block49 = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txt0372tag").Single();
                    block49.InsertAfterSelf(new Paragraph(new Run(new Text(model.Morning730))));
                    block49.Remove();

                    SdtElement block50 = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txt5472").Single();
                    block50.InsertAfterSelf(new Paragraph(new Run(new Text(model.Morning745))));
                    block50.Remove();

                    SdtElement block51 = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txt8002").Single();
                    block51.InsertAfterSelf(new Paragraph(new Run(new Text(model.Morning800))));
                    block51.Remove();

                    SdtElement block52 = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txt5182").Single();
                    block52.InsertAfterSelf(new Paragraph(new Run(new Text(model.Morning815))));
                    block52.Remove();

                    SdtElement block53 = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txt3082").Single();
                    block53.InsertAfterSelf(new Paragraph(new Run(new Text(model.Morning830))));
                    block53.Remove();

                    SdtElement block54 = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txt4582").Single();
                    block54.InsertAfterSelf(new Paragraph(new Run(new Text(model.Morning845))));
                    block54.Remove();

                    SdtElement block55 = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txt92Tag").Single();
                    block55.InsertAfterSelf(new Paragraph(new Run(new Text(model.Morning900))));
                    block55.Remove();

                    SdtElement block56 = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtAfter2").Single();
                    block56.InsertAfterSelf(new Paragraph(new Run(new Text(model.Noon330))));
                    block56.Remove();

                    SdtElement block57 = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtEve2").Single();
                    block57.InsertAfterSelf(new Paragraph(new Run(new Text(model.Noon400))));
                    block57.Remove();

                    SdtElement block58 = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtRoutine2").Single();
                    block58.InsertAfterSelf(new Paragraph(new Run(new Text(model.Noon430))));
                    block58.Remove();

                    SdtElement block59 = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtNoon2").Single();
                    block59.InsertAfterSelf(new Paragraph(new Run(new Text(model.Noon500))));
                    block59.Remove();

                    SdtElement block60 = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtTypical2").Single();
                    block60.InsertAfterSelf(new Paragraph(new Run(new Text(model.Noon530))));
                    block60.Remove();

                    SdtElement block61 = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtCal62").Single();
                    block61.InsertAfterSelf(new Paragraph(new Run(new Text(model.Noon600))));
                    block61.Remove();

                    SdtElement block62 = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtNes32").Single();
                    block62.InsertAfterSelf(new Paragraph(new Run(new Text(model.Noon630))));
                    block62.Remove();

                    SdtElement block63 = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "ptxtOon72").Single();
                    block63.InsertAfterSelf(new Paragraph(new Run(new Text(model.Noon700))));
                    block63.Remove();

                    SdtElement block64 = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtMorning2").Single();
                    block64.InsertAfterSelf(new Paragraph(new Run(new Text(model.Noon730))));
                    block64.Remove();

                    SdtElement block65 = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtMis82").Single();
                    block65.InsertAfterSelf(new Paragraph(new Run(new Text(model.Noon800))));
                    block65.Remove();

                    SdtElement block66 = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtTable2").Single();
                    block66.InsertAfterSelf(new Paragraph(new Run(new Text(model.Noon830))));
                    block66.Remove();

                    SdtElement block67 = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtFeed2").Single();
                    block67.InsertAfterSelf(new Paragraph(new Run(new Text(model.Noon900))));
                    block67.Remove();

                    SdtElement block68 = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtMay2").Single();
                    block68.InsertAfterSelf(new Paragraph(new Run(new Text(model.Noon930))));
                    block68.Remove();

                    SdtElement block69 = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtJune2").Single();
                    block69.InsertAfterSelf(new Paragraph(new Run(new Text(model.Noon100))));
                    block69.Remove();

                    SdtElement block10to11 = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtData1").Single();
                    block10to11.InsertAfterSelf(new Paragraph(new Run(new Text(model.Noon10to11))));
                    block10to11.Remove();

                    SdtElement block11to12 = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtData2").Single();
                    block11to12.InsertAfterSelf(new Paragraph(new Run(new Text(model.Noon11to7))));
                    block11to12.Remove();

                    SdtElement blockLeis = mainPart.Document.Body.Descendants<SdtElement>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtLeisure2").Single();
                    blockLeis.InsertAfterSelf(new Paragraph(new Run(new Text(model.Leisure))));
                    blockLeis.Remove();


                    if (model.SignList != null)
                    {
                        if (model.SignList.Count > 0)
                        {
                            SdtBlock ccWithTableSign = mainPart.Document.Body.Descendants<SdtBlock>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val == "txtSignature").Single();
                            DocumentFormat.OpenXml.Wordprocessing.Table theTableSign = ccWithTableSign.Descendants<DocumentFormat.OpenXml.Wordprocessing.Table>().Single();
                            DocumentFormat.OpenXml.Wordprocessing.TableRow theRowSign = theTableSign.Elements<DocumentFormat.OpenXml.Wordprocessing.TableRow>().Last();
                            foreach (var item in model.SignList)
                            {
                                DocumentFormat.OpenXml.Wordprocessing.TableRow rowCopy4 = (DocumentFormat.OpenXml.Wordprocessing.TableRow)theRowSign.CloneNode(true);
                                rowCopy4.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(0).Append(new Paragraph
                                    (new Run(new Text(item.PrintName))));
                                rowCopy4.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(1).Append(new Paragraph
                                    (new Run(new Text(item.Signature))));
                                rowCopy4.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ElementAt(2).Append(new Paragraph
                                    (new Run(new Text(item.Date))));
                                theTableSign.AppendChild(rowCopy4);
                            }
                            theTableSign.RemoveChild(theRowSign);
                        }
                    }

                    // Save the changes to the table back into the document.
                    mainPart.Document.Save();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            string dwnld = NewPath;

            FileInfo file = new FileInfo(dwnld);
            if (file.Exists)
            {
                if (dwnld.ToLower().EndsWith(".docx"))
                {
                    string NewFileName = file.FullName.Replace(".docx", ".doc");
                    Response.Clear();
                    Response.ContentType = "application/msword";
                    Response.AddHeader("Content-Disposition", "Attachment; filename=\"" + NewFileName);
                    Response.AddHeader("Content-Length", file.Length.ToString());
                    Response.TransmitFile(file.FullName);
                    Response.End();
                }
                file.Delete();
            }


        }

        private string CopyTemplate(string oldPath, string PageNo)
        {
            try
            {
                string Time = DateTime.Now.TimeOfDay.ToString();
                string[] ar = Time.Split('.');
                Time = ar[0];
                Time = Time.Replace(":", "-");
                string Datet = DateTime.Today.Day.ToString() + "-" + DateTime.Today.Month + "-" + DateTime.Today.Year.ToString() + "-" + Time;

                string path = Server.MapPath("~\\Templates") + "\\Protocol";
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

                string newFileName = "ProtocolTemp" + ids.ToString();
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


        //[HttpPost]
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult SaveSummary(ProtocolSummary model)
        {
            sess = (clsSession)Session["UserSessionClient"];
            ProtocolSummary model1 = new ProtocolSummary();
            objFuns = new Other_Functions();
            string result = "";
            //if (sess != null)
            //{
            ViewBag.permission = (objFuns.setClientPermission("Protocol Summary") == 2) ? "true" : "false";

            result = objFuns.SaveProtocolSummary(model);
            string path = Server.MapPath("~\\Templates\\Protocol.docx");
            model1 = objFuns.ProtocolExpo(sess.StudentId, sess.SchoolId, path); //Save the Exported Protocol Summary to BinaryFiles And Document

            if (result == "sucess")
            {
                TempData["ProgressMessage"] = "Data Saved Successfully";

            }
            else
            {
                TempData["ProgressMessage"] = "Failed To Insert Data";

            }
            //}
            return RedirectToAction("Index");
        }
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult DeleteRowATList(int id2)
        {
            sess = (clsSession)Session["UserSessionClient"];
            objFuns = new Other_Functions();
            BiWeeklyRCPNewEntities dbobj = new BiWeeklyRCPNewEntities();
            var id = (from m in dbobj.Protocol_1_Assistive
                      join n in dbobj.Protocols
                      on m.ProtocolId equals n.ProtocolId
                      where n.SchoolId == sess.SchoolId && n.StudentId == sess.StudentId
                      select m.AssistiveId).ToList();
            if (id.Count > 0)
            {
                objFuns.deleteATList(id2);
            }

            return RedirectToAction("Index");
        }

        //
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult DeleteRowCGList(int id2)
        {
            sess = (clsSession)Session["UserSessionClient"];
            objFuns = new Other_Functions();
            BiWeeklyRCPNewEntities dbobj = new BiWeeklyRCPNewEntities();

            objFuns.deleteCGList(id2);

            return RedirectToAction("Index");
        }

        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult DeleteRowFIList(int id2)
        {
            sess = (clsSession)Session["UserSessionClient"];
            objFuns = new Other_Functions();
            BiWeeklyRCPNewEntities dbobj = new BiWeeklyRCPNewEntities();

            objFuns.deleteFIList(id2);

            return RedirectToAction("Index");
        }

        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult DeleteRowBBIList(int id2)
        {
            sess = (clsSession)Session["UserSessionClient"];
            objFuns = new Other_Functions();
            BiWeeklyRCPNewEntities dbobj = new BiWeeklyRCPNewEntities();

            objFuns.deleteBBIList(id2);

            return RedirectToAction("Index");
        }

        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult DeleteRowSignList(int id2)
        {
            sess = (clsSession)Session["UserSessionClient"];
            objFuns = new Other_Functions();
            BiWeeklyRCPNewEntities dbobj = new BiWeeklyRCPNewEntities();

            objFuns.deleteSignList(id2);

            return RedirectToAction("Index");
        }


        public ActionResult GetProtocolSummary(int StudentId)
        {
            setSession();
            sess = (clsSession)Session["UserSessionClient"];
            sess.StudentId = StudentId;
            if (Session["PlacementStat"] == null)
            {
                Session["PlacementStat"] = "A";
            }
            //FillSummary();
            return View();
        }

        public void setSession()
        {
            sess = new clsSession();
            // sess = (clsSession)Session["UserSessionClient"];
            BiWeeklyRCPNewEntities Objdata = new BiWeeklyRCPNewEntities();

            if (Session["Values"] != null)
            {
                string Values = Session["Values"].ToString();
                string[] arValues = Values.Split('#');
                sess.LoginId = Convert.ToInt16(arValues[1]);
                sess.SchoolId = Convert.ToInt16(arValues[0]);
                //sess.UserName = Convert.ToString(arValues[3]);

                if (sess.LoginId != 0)              //set the remaining session used in the Referral DB
                {
                    var Role = (from Objrole in Objdata.Roles
                                join objrgp in Objdata.RoleGroups on Objrole.RoleId equals objrgp.RoleId
                                select new
                                {
                                    RoleId = Objrole.RoleId,
                                    Roledesc = Objrole.RoleDesc,
                                    SchoolId = Objrole.SchoolId
                                }).ToList();
                    var Usr = (from Objrole in Role
                               from Objusr in Objdata.Users
                               where Objusr.UserId == sess.LoginId
                               select new
                               {
                                   Objrole.RoleId,
                                   Objrole.Roledesc,

                                   Objusr.UserId,
                                   Objusr.UserFName,
                                   Objusr.UserLName,
                                   Objusr.Gender
                               }).ToList();

                    if (Usr == null) return;
                    if (Usr.Count() > 0)
                    {

                        sess.IsLogin = true;
                        sess.LoginTime = (DateTime.Now.ToShortTimeString()).ToString();

                        sess.LoginId = Convert.ToInt32(Usr[0].UserId);
                        sess.UserName = Convert.ToString(Usr[0].UserLName + "," + Usr[0].UserFName);
                        sess.RoleId = Convert.ToInt32(Usr[0].RoleId);
                        sess.Gender = Convert.ToString(Usr[0].Gender);
                        sess.RoleName = Convert.ToString(Usr[0].Roledesc);
                        sess.SessionID = Session.SessionID.ToString();
                        //sess.ReferralId = 0;
                    }
                }

                Session["UserSessionClient"] = sess;
            }

        }


        [HttpPost]
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ProtocolSummary FillSummary()
        {
            sess = (clsSession)Session["UserSessionClient"];
            objFuns = new Other_Functions();
            Protocol objP = new Protocol();


            ProtocolSummary model = new ProtocolSummary();
            BiWeeklyRCPNewEntities dbobj = new BiWeeklyRCPNewEntities();
            StudentPersonalPA studentPA = new StudentPersonalPA();

            var pId1 = (from m in dbobj.Protocols
                        where (m.SchoolId == sess.SchoolId && m.StudentId == sess.StudentId)
                        select m.ProtocolId).ToList();
            if (pId1.Count > 0)
            {
                int pId = pId1[0];

                //var pId = 12;


                var SummaryItems = (from x in dbobj.Protocols
                                    //where (x.SchoolId==sess.SchoolId && x.StudentId==sess.StudentId)
                                    where (x.ProtocolId == pId)
                                    select new ProtocolSummary
                                    {
                                        HomeCommon = x.HomeCommon,
                                        HomeBedroom = x.HomeBedroom,
                                        HomeBathroom = x.HomeBathroom,
                                        Campus = x.Campus,
                                        Community = x.Community,
                                        SchoolCommon = x.SchoolCommon,
                                        SchoolBathroom = x.SchoolBathroom,
                                        SchoolOutside = x.SchoolOutside,
                                        Pool = x.Pool,
                                        Van = x.Van,

                                        MasteredTask = x.MasteredTask,
                                        NewTask = x.NewTask,

                                        //Allergies = x.Allergies,
                                        //SeizureInfo = x.SeizureInfo,
                                        MedTimes = x.MedTimes,
                                        TakeMed = x.TakeMed,
                                        OtherMedical = x.OtherMedical,

                                        DoctorVisit = x.DoctorVisit,
                                        Dental = x.Dental,
                                        BloodWork = x.BloodWork,
                                        HairCuts = x.HairCuts,
                                        OtherBehave = x.OtherBehave,

                                        EatingGeneral = x.EatingGeneral,
                                        EatingAble = x.EatingAble,
                                        EatingNeed = x.EatingNeed,
                                        EatingIep = x.EatingIep,

                                        ToiletingGeneral = x.ToiletingGeneral,
                                        ToiletingAble = x.ToiletingAble,
                                        ToiletingNeed = x.ToiletingNeed,
                                        ToiletingIep = x.ToiletingIep,

                                        BrushingGeneral = x.BrushingGeneral,
                                        BrushingAble = x.BrushingAble,
                                        BrushingNeed = x.BrushingNeed,
                                        BrushingIep = x.BrushingIep,

                                        HandGeneral = x.HandGeneral,
                                        HandAble = x.HandAble,
                                        HandNeed = x.HandNeed,
                                        HandIep = x.HandIep,

                                        DressGeneral = x.DressGeneral,
                                        DressAble = x.DressAble,
                                        DressNeed = x.DressNeed,
                                        DressIep = x.DressIep,

                                        ShowerGeneral = x.ShowerGeneral,
                                        ShowerAble = x.ShowerAble,
                                        ShowerNeed = x.ShowerNeed,
                                        ShowerIep = x.ShowerIep,

                                        BedTime = x.BedTime,

                                        Morning7 = x.Morning7,
                                        Morning715 = x.Morning715,
                                        Morning730 = x.Morning730,
                                        Morning745 = x.Morning745,
                                        Morning800 = x.Morning800,
                                        Morning815 = x.Morning815,
                                        Morning830 = x.Morning830,
                                        Morning845 = x.Morning845,
                                        Morning900 = x.Morning900,

                                        Noon330 = x.Noon330,
                                        Noon400 = x.Noon400,
                                        Noon430 = x.Noon430,
                                        Noon500 = x.Noon500,
                                        Noon530 = x.Noon530,
                                        Noon600 = x.Noon600,
                                        Noon630 = x.Noon630,
                                        Noon700 = x.Noon700,
                                        Noon730 = x.Noon730,
                                        Noon800 = x.Noon800,
                                        Noon830 = x.Noon830,
                                        Noon900 = x.Noon900,
                                        Noon930 = x.Noon930,
                                        Noon100 = x.Noon100,
                                        Noon10to11 = x.Noon10to11,
                                        Noon11to7 = x.Noon11to7,
                                        Leisure = x.Leisure,
                                        Modified1 = x.ModifiedOn,

                                        UpdtLOS = x.UpdtLOS,
                                        UpdtPageTop = x.UpdtTop,
                                        UpdtTPH = x.UpdtTPH,
                                        UpdtMedInfo = x.UpdtMedInfo,
                                        UpdtPerCare = x.UpdtPerCare,
                                        UpdtBehInfo = x.UpdtBehInfo,
                                        UpdtTypRoutines = x.UpdtTypRoutines,

                                        UpdtATList = x.UpdtAtList,
                                        UpdtCGList = x.UpdtCGList,
                                        UpdtFIList = x.UpdtFIList,
                                        UpdtBBIList = x.UpdtBBIList






                                    }).SingleOrDefault();

                SummaryItems.ATList = (from a in dbobj.Protocol_1_Assistive
                                       where (a.ProtocolId == pId)
                                       select new AssistTech
                                       {
                                           Type = a.Type,
                                           ScheduleForUse = a.ScheduleForUse,
                                           StorageLocation = a.StorageLocation,
                                           Modified2 = a.ModifiedOn,
                                           AssistiveId = a.AssistiveId
                                           //UpdtATList=a.UpdtATList
                                       }).ToList();

                SummaryItems.CGList = (from b in dbobj.Protocol_2_Community
                                       where (b.ProtocolId == pId)
                                       select new CommGuide
                                       {
                                           TypeA = b.TypeA,
                                           TypeB = b.TypeB,
                                           Modified3 = b.ModifiedOn,
                                           CommunityId = b.CommunityId
                                           //UpdtCGList=b.UpdtCGList
                                       }).ToList();

                SummaryItems.FIList = (from c in dbobj.Protocol_3_Family
                                       where (c.ProtocolId == pId)
                                       select new FamilyInfo
                                       {
                                           FamilyOne = c.FamilyOne,
                                           FamilyTwo = c.FamilyTwo,
                                           Modified4 = c.ModifiedOn,
                                           FamilyId = c.FamilyId
                                           //UpdtFIList=c.UpdtFIList
                                       }).ToList();

                SummaryItems.BBIList = (from d in dbobj.Protocol_4_Basic
                                        where (d.ProtocolId == pId)
                                        select new BasicBehavInfo
                                        {
                                            Acceleration = d.Acceleration,
                                            Strategy = d.Strategy,
                                            Modified5 = d.ModifiedOn,
                                            BasicId = d.BasicId
                                            //UpdtBBIList=d.UpdtBBIList
                                        }).ToList();

                SummaryItems.SignList = (from e in dbobj.Protocol_5_Signature
                                         where (e.ProtocolId == pId)
                                         select new SignatureLi
                                         {
                                             SignatureId = e.SignatureId,
                                             PrintName = e.PrintName,
                                             Signature = e.Signature,
                                             Date = e.Date,
                                             Modified6 = e.ModifiedOn
                                         }).ToList();
                model = SummaryItems;
            }
            else
            {

            }
            int ClientId = Convert.ToInt32(Session["TempStudentId"]);
            studentPA = dbobj.StudentPersonalPAs.Where(objStudentPA => objStudentPA.StudentPersonalId == ClientId && objStudentPA.SchoolId == sess.SchoolId).SingleOrDefault();
            if (studentPA != null)
            {
                model.Allergies = studentPA.Allergies;
                model.SeizureInfo = studentPA.Seizures;
            }


            return model;


        }



    }
}
