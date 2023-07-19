using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ClientDB.AppFunctions;
using ClientDB.DbModel;
using System.Web.Mvc;

namespace ClientDB.Models
{
    public class RegistrationModel
    {

        public virtual int Id { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual string MiddleName { get; set; }
        public virtual string NickName { get; set; }
        public virtual string ClientStatus { get; set; }
        public virtual IEnumerable<SelectListItem> LastNameSuffixList { get; set; }
        public virtual string LastNameSuffix { get; set; }
        public virtual string DateOfBirth { get; set; }
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
        public virtual string ModifiedDate { get; set; }
        public virtual string UpdatedOn { get; set; }
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
        public virtual string EducationalSurrogate { get; set; }
        public virtual IEnumerable<SelectListItem> AddressList { get; set; }
        public virtual int AddressID { get; set; }
        public virtual string studCounty { get; set; }
        public virtual string AddressLine1 { get; set; }
        public virtual string AddressLine2 { get; set; }
        public virtual string AddressLine3 { get; set; }
        public virtual IEnumerable<SelectListItem> CountryList { get; set; }
        public virtual int? Country { get; set; }
        public virtual string StrCountry { get; set; }
        public virtual IEnumerable<SelectListItem> StateList { get; set; }
        public virtual IEnumerable<SelectListItem> SchoolState1 { get; set; }
        public virtual IEnumerable<SelectListItem> SchoolState2 { get; set; }
        public virtual IEnumerable<SelectListItem> SchoolState3{ get; set; }
        public virtual int? State { get; set; }
        public virtual string StrState { get; set; }
        public virtual string City { get; set; }
        public virtual IEnumerable<SelectListItem> ZipList { get; set; }
        public virtual int Zip { get; set; }
        public virtual string ZipCode { get; set; }
        public virtual string InsuranceType { get; set; }
        public virtual string PolicyNumber { get; set; }
        public virtual string PolicyHolder { get; set; }
        public virtual string DateInitiallyEligibleforSpecialEducation { get; set; }
        public virtual string DateofMostRecentSpecialEducationEvaluations { get; set; }
        public virtual string DateofNextScheduled3YearEvaluation { get; set; }
        public virtual string CurrentIEPStartDate { get; set; }
        public virtual string CurrentIEPExpirationDate { get; set; }
        public virtual string DischargeDate { get; set; }
        public virtual string LocationAfterDischarge { get; set; }
        public virtual string MelmarkNewEnglandsFollowUpResponsibilities { get; set; }
        public virtual string StudentId { get; set; }
        public virtual string Name { get; set; }
        public virtual string ImageUrl { get; set; }
        public virtual bool? PhotoReleasePermission { get; set; }
        public HttpFileCollectionBase profilePicture { get; set; }

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

        public virtual string PrimaryPhysicianName { get; set; }
        public virtual string PrimaryPhysicianAddress { get; set; }
        public virtual string PrimaryPhysicianPhone { get; set; }

        public virtual string DateOfLastPhysicalExam { get; set; }
        public virtual string MedicalConditionOrDiagnosis { get; set; }
        public virtual string Allergies { get; set; }
        public virtual string CurrentMedications { get; set; }
        public virtual string SelfPreservationAbility { get; set; }
        public virtual string SignificantBehaviorCharacteristics { get; set; }
        public virtual string Capabilities { get; set; }
        public virtual string Limitations { get; set; }
        public virtual string Preferences { get; set; }

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
        public virtual string ReferralIEPFullName { get; set; }
        public virtual string ReferralIEPTitle { get; set; }
        public virtual string ReferralIEPPhone { get; set; }
        public virtual string ReferralIEPReferringAgency { get; set; }
        public virtual string ReferralIEPSourceofTuition { get; set; }
        public virtual IEnumerable<Insurance> InsuranceList { get; set; }
        public virtual IEnumerable<EmergencyContactPersonalModel> EmergencyContactList { get; set; }

        //public static MelmarkRCPEntities RPCobj = new MelmarkRCPEntities();


        public static void SaveClients()
        {

        }

        public RegistrationModel()
        {
            LastNameSuffixList = new List<SelectListItem>();
            GenderList = new List<SelectListItem>();
            RaceList = new List<SelectListItem>();
            CountryOfBirthList = new List<SelectListItem>();
            StateOfBirthList = new List<SelectListItem>();
            CitizenshipList = new List<SelectListItem>();
            AddressList = new List<SelectListItem>();
            CountryList = new List<SelectListItem>();
            StateList = new List<SelectListItem>();
            SchoolState1= new List<SelectListItem>();
            SchoolState2 = new List<SelectListItem>();
            SchoolState3=new List<SelectListItem>();
            ZipList = new List<SelectListItem>();
            InsuranceList = new List<Insurance>();
            EmergencyContactList = new List<EmergencyContactPersonalModel>();
        }

    }

    public class ImageUploader
    {

    }
}