using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClientDB.Models
{
    public class ProtocolSummary
    {
        public string HomeCommon { get; set; }
        public string HomeBedroom { get; set; }
        public string HomeBathroom { get; set; }
        public string Campus { get; set; }
        public string Community { get; set; }
        public string SchoolCommon { get; set; }
        public string SchoolBathroom { get; set; }
        public string SchoolOutside { get; set; }
        public string Pool { get; set; }
        public string Van { get; set; }

        public List<AssistTech> ATList { get; set; }

        public string MasteredTask { get; set; }
        public string NewTask { get; set; }

        public string Allergies { get; set; }
        public string SeizureInfo { get; set; }
        public string MedTimes { get; set; }
        public string TakeMed { get; set; }
        public string OtherMedical { get; set; }

        public string DoctorVisit { get; set; }
        public string Dental { get; set; }
        public string BloodWork { get; set; }
        public string HairCuts { get; set; }
        public string OtherBehave { get; set; }

        public string EatingGeneral { get; set; }
        public string EatingAble { get; set; }
        public string EatingNeed { get; set; }
        public string EatingIep { get; set; }

        public string ToiletingGeneral { get; set; }
        public string ToiletingAble { get; set; }
        public string ToiletingNeed { get; set; }
        public string ToiletingIep { get; set; }

        public string BrushingGeneral { get; set; }
        public string BrushingAble { get; set; }
        public string BrushingNeed { get; set; }
        public string BrushingIep { get; set; }

        public string HandGeneral { get; set; }
        public string HandAble { get; set; }
        public string HandNeed { get; set; }
        public string HandIep { get; set; }

        public string DressGeneral { get; set; }
        public string DressAble { get; set; }
        public string DressNeed { get; set; }
        public string DressIep { get; set; }

        public string ShowerGeneral { get; set; }
        public string ShowerAble { get; set; }
        public string ShowerNeed { get; set; }
        public string ShowerIep { get; set; }

        public string BedTime { get; set; }

        public string Morning7 { get; set; }
        public string Morning715 { get; set; }
        public string Morning730 { get; set; }
        public string Morning745 { get; set; }
        public string Morning800 { get; set; }
        public string Morning815 { get; set; }
        public string Morning830 { get; set; }
        public string Morning845 { get; set; }
        public string Morning900 { get; set; }

        public string Noon330 { get; set; }
        public string Noon400 { get; set; }
        public string Noon430 { get; set; }
        public string Noon500 { get; set; }
        public string Noon530 { get; set; }
        public string Noon600 { get; set; }
        public string Noon630 { get; set; }
        public string Noon700 { get; set; }
        public string Noon730 { get; set; }
        public string Noon800 { get; set; }
        public string Noon830 { get; set; }
        public string Noon900 { get; set; }
        public string Noon930 { get; set; }
        public string Noon100 { get; set; }
        public string Noon10to11 { get; set; }
        public string Noon11to7 { get; set; }
        public string Leisure { get; set; }
        public string UpdtLOS { get; set; }
        public string UpdtPageTop { get; set; }
        public string UpdtTPH { get; set; }
        public string UpdtMedInfo { get; set; }
        public string UpdtPerCare { get; set; }
        public string UpdtBehInfo { get; set; }
        public string UpdtTypRoutines { get; set; }
        public string Updt { get; set; }
        public string UpdtATList { get; set; }
        public string UpdtCGList { get; set; }
        public string UpdtFIList { get; set; }
        public string UpdtBBIList { get; set; }

        public DateTime? Modified1 { get; set; }

        public List<CommGuide> CGList { get; set; }
        public List<FamilyInfo> FIList { get; set; }
        public List<BasicBehavInfo> BBIList { get; set; }
        public List<SignatureLi> SignList { get; set; }

        public ProtocolSummary()
        {
            ATList = new List<AssistTech>();
            CGList = new List<CommGuide>();
            FIList = new List<FamilyInfo>();
            BBIList = new List<BasicBehavInfo>();
            SignList = new List<SignatureLi>();
        }

    }

    public class AssistTech
    {
        public int AssistiveId { get; set; }
        public string Type { get; set; }
        public string ScheduleForUse { get; set; }
        public string StorageLocation { get; set; }
        public DateTime? Modified2 { get; set; }
        

    }
    public class CommGuide
    {
        public int CommunityId { get; set; }
        public string TypeA { get; set; }
        public string TypeB { get; set; }
        public DateTime? Modified3 { get; set; }
        
    }
    public class FamilyInfo
    {
        public int FamilyId { get; set; }
        public string FamilyOne { get; set; }
        public string FamilyTwo { get; set; }
        public DateTime? Modified4 { get; set; }
        
    }
    public class BasicBehavInfo
    {
        public int BasicId { get; set; }
        public string Acceleration { get; set; }
        public string Strategy { get; set; }
        public DateTime? Modified5 { get; set; }
        
    }
    public class SignatureLi
    {
        public int SignatureId { get; set; }
        public string PrintName { get; set; }
        public string Signature { get; set; }
        public string Date { get; set; }
        public DateTime? Modified6 { get; set; }
        public string UpdtSignList { get; set; }
    }
}