using ClientDB.DbModel;
using System;
using System.Collections.Generic;
using System.Data.Objects.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClientDB.Models
{
    public class ClientRegistrationPAModel
    {
        public virtual string eventLogNote { get; set; }
        public virtual int Id { get; set; }
        public virtual string Prefix { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual string MiddleName { get; set; }
        public virtual string NickName { get; set; }
        public virtual string ClientStatus { get; set; }
        public virtual string newEventLog { get; set; }
        public virtual IEnumerable<SelectListItem> LastNameSuffixList { get; set; }
        public virtual IEnumerable<SelectListItem> HomeCountryList { get; set; }
        public virtual int HomeCountry { get; set; }
        private  List<SelectListItem> _PrefixList = new List<SelectListItem>();
        public int hdnPlacementCount { get; set; }
        public virtual List<SelectListItem> PrefixList
        {
            get
            {
                if(_PrefixList==null || _PrefixList.Count==0)
                {
                    _PrefixList = new List<SelectListItem>();
                    _PrefixList.Add(new SelectListItem { Text = "--Select--", Value = "0" });
                    _PrefixList.Add(new SelectListItem { Text = "Master", Value = "1" });
                    _PrefixList.Add(new SelectListItem { Text = "Ms", Value = "2" });
                    _PrefixList.Add(new SelectListItem { Text = "Miss", Value = "3" });
                    _PrefixList.Add(new SelectListItem { Text = "Mrs", Value = "4" });
                    _PrefixList.Add(new SelectListItem { Text = "Mr", Value = "5" });
                    //_PrefixList.Add(new SelectListItem { Text = "Master", Value = "6" });
                    _PrefixList.Add(new SelectListItem { Text = "Dr", Value = "6" });
                    return _PrefixList;
                }
                else
                {
                    return _PrefixList;
                }
            }
            set
            {
                _PrefixList = value;
            }
        }
        public virtual string LastNameSuffix { get; set; }

        public virtual bool? Ambulatory { get; set; }
        private List<SelectListItem> _YesNo = new List<SelectListItem>();
        public virtual List<SelectListItem> YesNo
        {
            get
            {
                if (_YesNo == null || _YesNo.Count == 0)
                {
                    _YesNo = new List<SelectListItem>();
                    _YesNo.Add(new SelectListItem { Text = "--Select--", Value = "" });
                    _YesNo.Add(new SelectListItem { Text = "Yes", Value = "true" });
                    _YesNo.Add(new SelectListItem { Text = "No", Value = "false" });
                    return _YesNo;
                }
                else
                {
                    return _YesNo;
                }
            }
            set
            {
                _YesNo = value;
            }
        }

        public virtual int? EnglishProficiency { get; set; }
        public string GetEnglishProficiency()
        {
            string Result = "";
            if (EnglishProficiency == null) return Result;

            var filterResult = YesNoId.Where(x => x.Value == EnglishProficiency.ToString()).ToList();
            if(filterResult.Count>0)
            {
                Result = filterResult[0].Text;
            }
            return Result;
        }
        private List<SelectListItem> _YesNoId = new List<SelectListItem>();
        public virtual List<SelectListItem> YesNoId
        {
            get
            {
                if (_YesNoId == null || _YesNoId.Count == 0)
                {
                    _YesNoId = new List<SelectListItem>();
                    _YesNoId.Add(new SelectListItem { Text = "--Select--", Value = "0" });
                    _YesNoId.Add(new SelectListItem { Text = "Yes", Value = "1" });
                    _YesNoId.Add(new SelectListItem { Text = "No", Value = "2" });
                    return _YesNoId;
                }
                else
                {
                    return _YesNoId;
                }
            }
            set
            {
                _YesNoId = value;
            }
        }

        public virtual string SASID { get; set; }
        public virtual string Medicaid { get; set; }
        public virtual string Note { get; set; }

        public virtual string Intensive { get; set; }
        public virtual bool IsGuardian { get; set; }
        public virtual bool IsCorrespondence { get; set; }
        public virtual bool Isincident { get; set; }
        public virtual bool IsEmergency { get; set; }
        public virtual bool IsEmergencyP { get; set; }
        public virtual bool IsEmergencyS { get; set; }
        public virtual bool IsEmergencyT { get; set; }
        public virtual bool IsEmergencyO { get; set; }
        public virtual bool IsOnCampusWithStaff { get; set; }
        public virtual bool IsOnCampusAlone { get; set; }
        public virtual bool IsOffCampus { get; set; }

        public virtual bool IsBilling { get; set; }
        public virtual bool IsSchool { get; set; }
        public virtual bool IsCustody { get; set; }
        public virtual bool IsNextOfKin { get; set; }
        public virtual bool IsApprovedVisitor { get; set; }
        public virtual string SpouseName { get; set; }

        public List<ContactModel> EmgContactList { get; set; }
        public List<ContactModel> ApprovedVisitor { get; set; }
        public List<ContactModel> EmgContactIndividual { get; set; }
        public List<ContactModel> ApprovedVisitorIndi { get; set; }

        public virtual string EmergencyContactFirstName1 { get; set; }
        public virtual string EmergencyContactLastName1 { get; set; }
        public virtual string EmergencyContactTitle1 { get; set; }
        public virtual string EmergencyContactPhone1 { get; set; }
        public virtual string EmergencyContactFirstName2 { get; set; }
        public virtual string EmergencyContactLastName2 { get; set; }
        public virtual string EmergencyContactTitle2 { get; set; }
        public virtual string EmergencyContactPhone2 { get; set; }
        public virtual string EmergencyContactFirstName3 { get; set; }
        public virtual string EmergencyContactLastName3 { get; set; }
        public virtual string EmergencyContactTitle3 { get; set; }
        public virtual string EmergencyContactPhone3 { get; set; }
        public virtual string EmergencyContactFirstName4 { get; set; }
        public virtual string EmergencyContactLastName4 { get; set; }
        public virtual string EmergencyContactTitle4 { get; set; }
        public virtual string EmergencyContactPhone4 { get; set; }
        public virtual string EmergencyContactFirstName5 { get; set; }
        public virtual string EmergencyContactLastName5 { get; set; }
        public virtual string EmergencyContactTitle5 { get; set; }
        public virtual string EmergencyContactPhone5 { get; set; }

        public virtual string SchoolName1 { get; set; }
        public virtual string DateFrom1 { get; set; }
        public virtual string DateTo1 { get; set; }
        public virtual string SchoolAttendedAddress11 { get; set; }
        public virtual string SchoolAttendedAddress21 { get; set; }
        public virtual string SchoolAttendedCity1 { get; set; }
        public virtual string SchoolAttendedState1 { get; set; }
        public virtual int intSchoolAttendedState1 { get; set; }
        public virtual string SchoolName2 { get; set; }
        public virtual string DateFrom2 { get; set; }
        public virtual string DateTo2 { get; set; }
        public virtual string SchoolAttendedAddress12 { get; set; }
        public virtual string SchoolAttendedAddress22 { get; set; }
        public virtual string SchoolAttendedCity2 { get; set; }
        public virtual string SchoolAttendedState2 { get; set; }
        public virtual int intSchoolAttendedState2 { get; set; }
        public virtual string SchoolName3 { get; set; }
        public virtual string DateFrom3 { get; set; }
        public virtual string DateTo3 { get; set; }
        public virtual string SchoolAttendedAddress13 { get; set; }
        public virtual string SchoolAttendedAddress23 { get; set; }
        public virtual string SchoolAttendedCity3 { get; set; }
        public virtual string SchoolAttendedState3 { get; set; }
        public virtual int intSchoolAttendedState3 { get; set; }

        public virtual string InsuranceType { get; set; }
        public virtual string PolicyNumber { get; set; }
        public virtual string PolicyHolder { get; set; }
        public virtual string InsuranceType1 { get; set; }
        public virtual string PolicyNumber1 { get; set; }
        public virtual string PolicyHolder1 { get; set; }

        public virtual string ReferralIEPFullName { get; set; }
        public virtual string ReferralIEPTitle { get; set; }
        public virtual string ReferralIEPPhone { get; set; }
        public virtual string ReferralIEPReferringAgency { get; set; }
        public virtual string ReferralIEPSourceofTuition { get; set; }

        public virtual string DateInitiallyEligibleforSpecialEducation { get; set; }
        public virtual string DateofMostRecentSpecialEducationEvaluations { get; set; }
        public virtual string DateofNextScheduled3YearEvaluation { get; set; }
        public virtual string CurrentIEPStartDate { get; set; }
        public virtual string CurrentIEPExpirationDate { get; set; }

        public virtual string DischargeDate { get; set; }
        public virtual string LocationAfterDischarge { get; set; }
        public virtual string MelmarkNewEnglandsFollowUpResponsibilities { get; set; }

        //public virtual IEnumerable<SelectListItem> AddressList { get; set; }
        //public virtual int AddressID { get; set; }
        public virtual string studCounty { get; set; }
        //public virtual string AddressLine1 { get; set; }
        //public virtual string AddressLine2 { get; set; }
        //public virtual string AddressLine3 { get; set; }
        //public virtual IEnumerable<SelectListItem> CountryList { get; set; }
        //public virtual int? Country { get; set; }
        //public virtual string StrCountry { get; set; }
        //public virtual IEnumerable<SelectListItem> StateList { get; set; }
        public virtual IEnumerable<SelectListItem> SchoolState1 { get; set; }
        public virtual IEnumerable<SelectListItem> SchoolState2 { get; set; }
        public virtual IEnumerable<SelectListItem> SchoolState3{ get; set; }
        //public virtual int? State { get; set; }
        //public virtual string StrState { get; set; }
        //public virtual string City { get; set; }

        public virtual string DayProgarm { get; set; }
        public virtual string ClassroomWorkshop { get; set; }
        public virtual string TeacherInstructor { get; set; }
        public virtual string ProgramSpecialist { get; set; }
        public virtual string EDUBehaviorAnalyst { get; set; }
        public virtual string CurriculumCoordinator { get; set; }
        public virtual string ResidentialProgram { get; set; }
        public virtual string House { get; set; }
        public virtual string ProgramManagerQMRP { get; set; }
        public virtual string HouseSupervisor { get; set; }
        public virtual string ResidentialBehaviorAnalyst { get; set; }
        public virtual string PrimaryNurse { get; set; }
        public virtual string UnitClerk { get; set; }
        public virtual string PhotoPermComment { get; set; }
        public virtual string PhotoRelease { get; set; }
        public virtual string TripRestriction1 { get; set; }
        public virtual string TripRestriction2 { get; set; }
        public virtual string TripComments { get; set; }
        public virtual string ClientInfoComments { get; set; }

        public virtual int? Classification1 { get; set; }
        public virtual int? Classification2 { get; set; }
        public virtual int? Classification3 { get; set; }
        public virtual int? Classification4 { get; set; }
        public virtual int? Classification5 { get; set; }

        public virtual string ClassificationName1 { get; set; }
        public virtual string ClassificationName2 { get; set; }
        public virtual string ClassificationName3 { get; set; }
        public virtual string ClassificationName4 { get; set; }
        public virtual string ClassificationName5 { get; set; }
        public virtual string PlacementStat { get; set; }

        public virtual List<SelectListItem> Classification { get; set; }

        public virtual string PositionLabelAdd { get; set; } //==== List 6 - Task #2
        public virtual int? StaffPosition1 { get; set; } // List 6 - task #2
        public virtual int? StaffPosition2 { get; set; } // List 6 - task #2
        public virtual int? StaffPosition3 { get; set; } // List 6 - task #2
        public virtual int? StaffPosition4 { get; set; } // List 6 - task #2
        public virtual int? StaffPosition5 { get; set; } // List 6 - task #2
        public virtual int? StaffPosition6 { get; set; } // List 6 - task #2
        public virtual int? StaffPosition7 { get; set; } // List 6 - task #2
        public virtual int? StaffPosition8 { get; set; } // List 6 - task #2
        public virtual int? StaffPosition9 { get; set; } // List 6 - task #2
        public virtual int? StaffPosition10 { get; set; } // List 6 - task #2
        public virtual List<SelectListItem> StaffPositionList { get; set; } // List 6 - task #2

        public virtual int? Position1Staff { get; set; } // List 6 - task #2
        public virtual int? Position2Staff { get; set; } // List 6 - task #2
        public virtual int? Position3Staff { get; set; } // List 6 - task #2
        public virtual int? Position4Staff { get; set; } // List 6 - task #2
        public virtual int? Position5Staff { get; set; } // List 6 - task #2
        public virtual int? Position6Staff { get; set; } // List 6 - task #2
        public virtual int? Position7Staff { get; set; } // List 6 - task #2
        public virtual int? Position8Staff { get; set; } // List 6 - task #2
        public virtual int? Position9Staff { get; set; } // List 6 - task #2
        public virtual int? Position10Staff { get; set; } // List 6 - task #2
        public virtual List<SelectListItem> StaffList { get; set; } // List 6 - task #2

        //==========================================================

        public string PositionLabel1 { get; set; } // List 6 - task #2
        public string PositionLabel2 { get; set; } // List 6 - task #2
        public string PositionLabel3 { get; set; } // List 6 - task #2
        public string PositionLabel4 { get; set; } // List 6 - task #2
        public string PositionLabel5 { get; set; } // List 6 - task #2
        public string PositionLabel6 { get; set; } // List 6 - task #2
        public string PositionLabel7 { get; set; } // List 6 - task #2
        public string PositionLabel8 { get; set; } // List 6 - task #2
        public string PositionLabel9 { get; set; } // List 6 - task #2
        public string PositionLabel10 { get; set; } // List 6 - task #2        

        //=================================================================

        public virtual int? GetPopulatePositionID { get; set; } // List 6 - task #2
        public virtual List<SelectListItem> PopulatePositionList { get; set; } // List 6 - task #2

        //public virtual List<SelectListItem> Classification1List { get; set; }
        //public virtual List<SelectListItem> Classification2List { get; set; }
        //public virtual List<SelectListItem> Classification3List { get; set; }
        //public virtual List<SelectListItem> Classification4List { get; set; }
        //public virtual List<SelectListItem> Classification5List { get; set; }

        public virtual string PrimaryDiag { get; set; }

        public virtual IEnumerable<EmergencyContactPersonalModel> EmergencyContactList { get; set; }
        public virtual IEnumerable<Insurance> InsuranceList { get; set; }

        public List<SelectListItem> GetClassificationList(int Schoolid, string lookupclassification) //======List #6 - Task 1
        {
            BiWeeklyRCPNewEntities dbobj = new BiWeeklyRCPNewEntities();
            List<SelectListItem> result = new List<SelectListItem>();
            result.Add(new SelectListItem
            {
                Text = "---Select---",
                Value = 0.ToString()
            });
            //string classfiVal= "Classification" + ClassfiNo;
            List<SelectListItem> data = new List<SelectListItem>();
            data = (from look in dbobj.LookUps
                    where look.SchoolId == Schoolid && look.LookupType == lookupclassification
                    select new SelectListItem
                    {
                        Text = look.LookupName,
                        Value = SqlFunctions.StringConvert((decimal)look.LookupId).Trim(),
                    }).ToList();
            foreach(var item in data)
            {
                result.Add(item);
            }
            return result;
        }

        //=========== List #6 Task 2
        public List<SelectListItem> GetStaffPositionList(int Schoolid, string lookupstaffposition) //======List #6 - Task 2
        {
            BiWeeklyRCPNewEntities dbobj = new BiWeeklyRCPNewEntities();
            List<SelectListItem> result = new List<SelectListItem>();
            result.Add(new SelectListItem
            {
                Text = "---Select---",
                Value = 0.ToString()
            });
            List<SelectListItem> data = new List<SelectListItem>();
            data = (from look in dbobj.LookUps
                    where look.SchoolId == Schoolid && look.LookupType == lookupstaffposition && look.ActiveInd == "A"
                    select new SelectListItem
                    {
                        Text = look.LookupName,
                        Value = SqlFunctions.StringConvert((decimal)look.LookupId).Trim(),
                    }).ToList();
            foreach (var item in data)
            {
                result.Add(item);
            }
            return result;
        }


        public List<SelectListItem> GetStaffsList(int Schoolid, string lookupstaffactiveid)
        {
            BiWeeklyRCPNewEntities dbobj = new BiWeeklyRCPNewEntities();
            List<SelectListItem> result = new List<SelectListItem>();
            result.Add(new SelectListItem
            {
                Text = "---Select---",
                Value = 0.ToString()
            });
            List<SelectListItem> data = new List<SelectListItem>();
            data = (from look in dbobj.Users
                    where look.SchoolId == Schoolid && look.ActiveInd == lookupstaffactiveid
                    select new SelectListItem
                    {
                        Text = look.UserLName + ", " + look.UserFName,
                        Value = SqlFunctions.StringConvert((decimal)look.UserId).Trim(),
                    }).OrderBy(x => x.Text).ToList(); ;
            foreach (var item in data)
            {
                result.Add(item);
            }
            return result;
        }

        public List<SelectListItem> PopulatePositions(int Schoolid, string lookupstaffposition)
        {
            BiWeeklyRCPNewEntities dbobj = new BiWeeklyRCPNewEntities();
            List<SelectListItem> result = new List<SelectListItem>();
            result.Add(new SelectListItem
            {
                Text = "---Select---",
                Value = 0.ToString()
            });
            List<SelectListItem> data = new List<SelectListItem>();
            data = (from look in dbobj.LookUps
                    where look.SchoolId == Schoolid && look.LookupType == lookupstaffposition && look.ActiveInd == "A"
                    select new SelectListItem
                    {
                        Text = look.LookupName,
                        Value = SqlFunctions.StringConvert((decimal)look.LookupId).Trim(),
                    }).ToList();
            foreach (var item in data)
            {
                result.Add(item);
            }
            return result;
        }
        //=========== List #6 Task 2

        //==================================================

        public string GetPositionLabel(int Schoolid, string lookupstaffposition, int getsortorder)
        {
            BiWeeklyRCPNewEntities dbobj = new BiWeeklyRCPNewEntities();
            string result = "SamplePosition";
            var data = (from look in dbobj.LookUps
                        where look.SchoolId == Schoolid && look.LookupType == lookupstaffposition && look.SortOrder == getsortorder && look.ActiveInd == "A"
                        select new SelectListItem
                        {
                            Text = look.LookupName,
                        }).SingleOrDefault();
            result = data.Text;
            return result;
        }

        public int GetPositionLabelID(int Schoolid, string lookupstaffposition, int getsortorder)
        {
            BiWeeklyRCPNewEntities dbobj = new BiWeeklyRCPNewEntities();
            int result = 0;
            var data = (from look in dbobj.LookUps
                        where look.SchoolId == Schoolid && look.LookupType == lookupstaffposition && look.SortOrder == getsortorder && look.ActiveInd == "A"
                        select new
                        {
                            Value = SqlFunctions.StringConvert((decimal)look.LookupId).Trim(),
                        }).SingleOrDefault();
            result = Convert.ToInt32(data.Value);
            return result;
        }

        //==================================================


        public virtual string DateOfBirth { get; set; }
        public virtual string DateUpdated { get; set; }
        public virtual string AdmissinDate { get; set; }
        public virtual string Photodate { get; set; }
        public virtual IEnumerable<SelectListItem> GenderList { get; set; }
        public virtual string Gender { get; set; }
        public virtual IEnumerable<SelectListItem> RaceList { get; set; }
        public virtual int Race { get; set; }
        public virtual string StrRace { get; set; }
        public virtual IEnumerable<SelectListItem> CountryOfBirthList { get; set; }
        public virtual int? CountryofBirth { get; set; }
        public virtual string CountryBirth { get; set; }
        public virtual string PlaceOfBirth { get; set; }
        public virtual IEnumerable<SelectListItem> StateOfBirthList { get; set; }
        public virtual int? StateOfBirth { get; set; }
        public virtual string StateBirth { get; set; }
        public virtual IEnumerable<SelectListItem> CitizenshipList { get; set; }
        public virtual int Citizenship { get; set; }
        public virtual string CitizenshipBirth { get; set; }
        public virtual string Height { get; set; }
        public virtual string Weight { get; set; }
        public virtual string HairColor { get; set; }
        public virtual string EyeColor { get; set; }
        public virtual string PrimaryLanguage { get; set; }
        public virtual string LegalCompetencyStatus { get; set; }
        public virtual string GuardianshipStatus { get; set; }
        public virtual string OtherStateAgenciesInvolvedWithStudent { get; set; }
        public virtual string DistigushingMarks { get; set; }
        public virtual string MaritalStatusofBothParents { get; set; }
        public virtual string CaseManagerResidential { get; set; }
        public virtual string CaseManagerEducational { get; set; }
        public virtual IEnumerable<SelectListItem> AddressList { get; set; }
        public virtual int AddressID { get; set; }
        public virtual string AddressLine1 { get; set; }
        public virtual string AddressLine2 { get; set; }
        public virtual string AddressLine3 { get; set; }
        public virtual IEnumerable<SelectListItem> CountryList { get; set; }
        public virtual int? Country { get; set; }
        public virtual string StrCountry { get; set; }
        public virtual IEnumerable<SelectListItem> StateList { get; set; }
        public virtual int? State { get; set; }
        public virtual string StrState { get; set; }
        public virtual string City { get; set; }
        public virtual string Funding { get; set; }
        public virtual IEnumerable<SelectListItem> ZipList { get; set; }
        public virtual int Zip { get; set; }
        public virtual string ZipCode { get; set; }
        public virtual string PhoneNumber { get; set; }
        public virtual string MostRecentGradeLevel { get; set; }
        public virtual string ClientAddressPhone { get; set; }


        public virtual string Bathroom { get; set; }
        public virtual string OnCampus { get; set; }
        public virtual string WhenTranspoting { get; set; }
        public virtual string OffCampus { get; set; }
        public virtual string PoolOrSwimming { get; set; }
        public virtual string van { get; set; }
        public virtual string CommonAreas { get; set; }
        public virtual string BedroomAwake { get; set; }
        public virtual string BedroomAsleep { get; set; }
        public virtual string TaskORBreak { get; set; }
        public virtual string TransitionInside { get; set; }
        public virtual string TransitionUnevenGround { get; set; }
        public virtual string RiskOfResistance { get; set; }
        public virtual string Mobility { get; set; }
        public virtual string StudentId { get; set; }
        public virtual string Name { get; set; }
        public virtual string ImageUrl { get; set; }
        public virtual bool? PhotoReleasePermission { get; set; }
        public HttpFileCollectionBase profilePicture { get; set; }

        public virtual string NeedForExtraHelp { get; set; }
        public virtual string ResponseToInstruction { get; set; }
        public virtual string Consciousness { get; set; }
        public virtual string WalkingResponse { get; set; }
        public virtual string Allergie { get; set; }
        public virtual string Seizures { get; set; }
        public virtual string Diet { get; set; }
        public virtual string Other { get; set; }
        public virtual string LiftingOrTransfers1 { get; set; }
        public virtual string LiftingOrTransfers2 { get; set; }
        public virtual string Ambulation1 { get; set; }
        public virtual string Ambulation2 { get; set; }
        public virtual string Toileting1 { get; set; }
        public virtual string Toileting2 { get; set; }
        public virtual string Eating1 { get; set; }
        public virtual string Eating2 { get; set; }
        public virtual string Showering1 { get; set; }
        public virtual string Showering2 { get; set; }
        public virtual string ToothBrushing1 { get; set; }
        public virtual string ToothBrushing2 { get; set; }

        public virtual string Dressing1 { get; set; }
        public virtual string Dressing2 { get; set; }
        public virtual string SkinCare1 { get; set; }
        public virtual string SkinCare2 { get; set; }
        public virtual string Communication1 { get; set; }
        public virtual string Communication2 { get; set; }
        public virtual string preferedActivities1 { get; set; }
        public virtual string preferedActivities2 { get; set; }
        public virtual string GeneralInformation1 { get; set; }
        public virtual string GeneralInformation2 { get; set; }
        public virtual string SuggestedProactiveEnvironmentalProcedures1 { get; set; }
        public virtual string SuggestedProactiveEnvironmentalProcedures2 { get; set; }
        public virtual string RelationNameEmer { get; set; }
        public virtual string RelationNameVisit { get; set; }

        public virtual string FunderListString { get; set; }
       

        //public virtual string SchoolAttendedCity2 { get; set; }
        //public virtual string SchoolAttendedState2 { get; set; }
        //public virtual string SchoolName3 { get; set; }
        //public virtual string DateFrom3 { get; set; }
        //public virtual string DateTo3 { get; set; }
        //public virtual string SchoolAttendedAddress13 { get; set; }
        //public virtual string SchoolAttendedAddress23 { get; set; }
        //public virtual string SchoolAttendedCity3 { get; set; }
        //public virtual string SchoolAttendedState3 { get; set; }
        public virtual IList<AdaptiveEquipmentz> Adapt{get;set;}
        public virtual IList<BasicBehavior> BasicBehave { get; set; }
        public virtual IList<Diagnosis> Diagnosis { get; set; }
       
        public ClientRegistrationPAModel()
        {
            Adapt = new List<AdaptiveEquipmentz>();
            BasicBehave = new List<BasicBehavior>();
            Diagnosis = new List<Diagnosis>();
            EmgContactList = new List<ContactModel>();
            Classification = new List<SelectListItem>();
            ApprovedVisitor = new List<ContactModel>();
            EmgContactIndividual = new List<ContactModel>();
            ApprovedVisitorIndi = new List<ContactModel>();
            EmergencyContactList = new List<EmergencyContactPersonalModel>();
            AddressList = new List<SelectListItem>();
            CountryList = new List<SelectListItem>();
            StateList = new List<SelectListItem>();
            SchoolState1 = new List<SelectListItem>();
            SchoolState2 = new List<SelectListItem>();
            SchoolState3 = new List<SelectListItem>();
            InsuranceList = new List<Insurance>();
            //Classification1List = new List<SelectListItem>();
            //Classification2List = new List<SelectListItem>();
            //Classification3List = new List<SelectListItem>();
            //Classification4List = new List<SelectListItem>();
            //Classification5List = new List<SelectListItem>();
        }
       
    }

    

    public class  AdaptiveEquipmentz
    {
        //finance officer university of kerala tvm 15010 and 725
        //- hon director uit headqtrs university of kerasla tvm 1510 SBI/SBT/DCB
        //50012 674.67 15010 1510 725 5000 
        public virtual string item { get; set; }
        public virtual string ScheduledForUss { get; set; }
        public virtual string StorageLocation { get; set; }
        public virtual string CleaningInstruction { get; set; }
        public virtual int AdaptiveEquimentId { get; set; }

    }

    public class BasicBehavior
    {
        public virtual string TargetBehavior { get; set; }
        public virtual string Definition { get; set; }
        public virtual string Antecedent { get; set; }
        public virtual string FCT { get; set; }
        public virtual string Consequances { get; set; }
        public virtual int BasicBehavioralInformationId { get; set; }
    }

    public class Diagnosis
    {
        public virtual string Name { get; set; }
        //public virtual string Value { get; set; }
    }


}