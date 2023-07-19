using ClientDB.DbModel;
using System;
using System.Collections.Generic;
using System.Data.Objects.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;


namespace ClientDB.Models
{
    public class ContactModel
    {
        public virtual string FUllName
        {
            get
            {
                return Helpers.GetPrefixText(FirstNamePrefix).EmptyIfNull() + " " + LastName.EmptyIfNull() + " " + FirstName.EmptyIfNull() + " " + Helpers.GetSuffixText(LastNameSuffix).EmptyIfNull();
            }
        }
        public virtual string eventLogNote { get; set; }
        public virtual string newEventLog { get; set; }

        public virtual int Id { get; set; }
        public virtual IEnumerable<SelectListItem> FirstNamePrefixList { get; set; }
        public virtual string FirstNamePrefix { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual string MiddleName { get; set; }
        public virtual string ContactFor { get; set; }
        public virtual IEnumerable<SelectListItem> LastNameSuffixList { get; set; }
        public virtual string LastNameSuffix { get; set; }
        public virtual IEnumerable<SelectListItem> RelationList { get; set; }
        public virtual int Relation { get; set; }
        public virtual string Spouse { get; set; }
        public virtual string UserID { get; set; }
        public virtual string PrimaryLanguage { get; set; }
        public virtual IEnumerable<SelectListItem> HomeAddressTypeList { get; set; }
        public virtual int HomeAddressTypeId { get; set; }
        public virtual IEnumerable<SelectListItem> WorkAddressTypeList { get; set; }
        public virtual int WorkAddressTypeId { get; set; }
        public virtual IEnumerable<SelectListItem> OtherAddressTypeList { get; set; }
        public virtual int OtherAddressTypeId { get; set; }
        public virtual string HomeAddressLine1 { get; set; }
        public virtual string HomeAddressLine2 { get; set; }
        public virtual string HomeAddressLine3 { get; set; }
        public virtual string ContactFlag { get; set; }
        public virtual IEnumerable<SelectListItem> HomeCountryList { get; set; }
        public virtual int HomeCountry { get; set; }
        public virtual IEnumerable<SelectListItem> HomeStateList { get; set; }
        public virtual int? HomeState { get; set; }
        public virtual string HmeState { get; set; }
        public virtual string HomeCity { get; set; }
        public virtual string HomeStreet { get; set; }
        public virtual int? HomeNumber { get; set; }
        // public virtual IEnumerable<SelectListItem> ZipList { get; set; }
        public virtual string HomeZip { get; set; }
        public virtual string HomePhone { get; set; }
        public virtual string HomeMobilePhone { get; set; }
        public virtual string HomeWorkPhone { get; set; }
        public virtual string HomeFax { get; set; }
        public virtual string HomeCounty { get; set; }
        public virtual string HomeEmail { get; set; }
        public virtual string HomeOtherMail { get; set; }
        public virtual string HomeWorkEmail { get; set; }

        public virtual string WorkAddressLine1 { get; set; }
        public virtual string WorkAddressLine2 { get; set; }
        public virtual string WorkAddressLine3 { get; set; }
        public virtual IEnumerable<SelectListItem> WorkCountryList { get; set; }
        public virtual int WorkCountry { get; set; }
        public virtual IEnumerable<SelectListItem> WorkStateList { get; set; }
        public virtual int? WorkState { get; set; }
        public virtual string WrkState { get; set; }
        public virtual string WorkCity { get; set; }
        // public virtual IEnumerable<SelectListItem> ZipList { get; set; }
        public virtual string WorkZip { get; set; }
        public virtual string WorkHomePhone { get; set; }
        public virtual string WorkMobilePhone { get; set; }
        public virtual string WorkPhone { get; set; }
        public virtual string WorkFax { get; set; }
        public virtual string WorkCounty { get; set; }
        public virtual string WorkHomeEmail { get; set; }
        public virtual string WorkEmail { get; set; }

        public virtual string OtherAddressLine1 { get; set; }
        public virtual string OtherAddressLine2 { get; set; }
        public virtual string OtherAddressLine3 { get; set; }
        public virtual IEnumerable<SelectListItem> OtherCountryList { get; set; }
        public virtual int OtherCountry { get; set; }
        public virtual IEnumerable<SelectListItem> OtherStateList { get; set; }
        public virtual int? OtherState { get; set; }
        public virtual string OthState { get; set; }
        public virtual string OtherCity { get; set; }
        // public virtual IEnumerable<SelectListItem> ZipList { get; set; }
        public virtual string OtherZip { get; set; }
        public virtual string OtherHomePhone { get; set; }
        public virtual string OtherMobilePhone { get; set; }
        public virtual string OtherWorkPhone { get; set; }
        public virtual string OtherFax { get; set; }
        public virtual string OtherCounty { get; set; }
        public virtual string OtherHomeEmail { get; set; }
        public virtual string OtherWorkEmail { get; set; }

        public virtual bool WorkIsMailingAddress { get; set; }
        public virtual bool HomeIsMailingAddress { get; set; }
        public virtual bool OtherIsMailingAddress { get; set; }

        public virtual string WorkExtension { get; set; }
        public virtual string HomeExtension { get; set; }
        public virtual string OtherExtension { get; set; }
        public virtual string OtherExtension2 { get; set; }
        public virtual string OtherExtension3 { get; set; }

        public virtual bool WorkMailOptIn { get; set; }
        public virtual bool HomeMailOptIn { get; set; }
        public virtual bool OtherMailOptIn { get; set; }

        public virtual bool WorkEmailOptIn { get; set; }
        public virtual bool HomeEmailOptIn { get; set; }
        public virtual bool OtherEmailOptIn { get; set; }

        public virtual bool WorkPhoneOptIn { get; set; }
        public virtual bool HomePhoneOptIn { get; set; }
        public virtual bool OtherPhoneOptIn { get; set; }

        public virtual bool WorkMailMergeOptIn { get; set; }
        public virtual bool HomeMailMergeOptIn { get; set; }
        public virtual bool OtherMailMergeOptIn { get; set; }

        public virtual string ImageUrl { get; set; }


        public IEnumerable<checkBoxViewModel> checkbox { get; set; }

        public IEnumerable<string> getcheked { get; set; }

        public virtual bool IsEmergency { get; set; }
        public virtual bool IsEmergencyP { get; set; }
        public virtual bool IsEmergencyS { get; set; }
        public virtual bool IsEmergencyT { get; set; }
        public virtual bool IsEmergencyO { get; set; }
        public virtual bool IsIncident { get; set; }
        public virtual bool IsBilling { get; set; }
        public virtual bool IsSchool { get; set; }
        public virtual int? SpouseId { get; set; }
        public virtual string Note { get; set; }
        public virtual bool IsOnCampusWithStaff { get; set; }
        public virtual bool IsOnCampusAlone { get; set; }
        public virtual bool IsOffCampus { get; set; }

        public virtual bool IsCorrespondence { get; set; }
        public virtual bool IsCustody { get; set; }
        public virtual bool IsGuardian { get; set; }
        public virtual bool IsNextOfKin { get; set; }
        public virtual bool ApprovedVisitor { get; set; }

        public virtual string Employer { get; set; }
        public virtual string Occupation { get; set; }
        public virtual string RelationName { get; set; }
        public virtual string SpouseName { get; set; }
        public List<SelectListItem> SpouseList { get; set; }

        public string GetSelectSpouseRelation()
        {
            string pattern = @"\[([^]]*)\]";
            string result = "";
            if (SpouseId != null && SpouseList != null && SpouseList.Count > 0)
            {
                var Data = SpouseList.Where(x => x.Value == SpouseId.ToString()).ToList();
                if (Data.Count > 0)
                {
                    var matchresult = Regex.Matches(Data[0].Text, pattern)
                                        .Cast<Match>()
                                        .Select(x => x.Groups[1].Value)
                                        .ToList();
                    if (matchresult.Count > 0)
                    {
                        result = matchresult[0];
                    }
                }

            }
            return result;
        }

        public List<SelectListItem> GetSpouseContact(int StudentId)
        {
            BiWeeklyRCPNewEntities dbobj = new BiWeeklyRCPNewEntities();
            List<SelectListItem> result = new List<SelectListItem>();
            result.Add(new SelectListItem
            {
                Text = "---Select---",
                Value = "0"
            });
            List<SelectListItem> Data = (from cont in dbobj.ContactPersonals
                                         join conrel in dbobj.StudentContactRelationships on cont.ContactPersonalId equals conrel.ContactPersonalId into ConrelNul
                                         from ConrelFull in ConrelNul.DefaultIfEmpty()
                                         join look in dbobj.LookUps on ConrelFull.RelationshipId equals look.LookupId into lookupnull
                                         from lookupFull in lookupnull.DefaultIfEmpty()
                                         where cont.StudentPersonalId == StudentId && cont.Status == 1

                                         select new SelectListItem
                                         {
                                             Text = cont.LastName + ", " + cont.FirstName + " [" + lookupFull.LookupName + "]",
                                             Value = SqlFunctions.StringConvert((decimal)cont.ContactPersonalId).Trim(),
                                         }).ToList();
            foreach (var item in Data)
            {
                result.Add(item);
            }
            return result;
        }

        public ContactModel()
        {
            checkbox = new List<checkBoxViewModel>();
            FirstNamePrefixList = new List<SelectListItem>();
            LastNameSuffixList = new List<SelectListItem>();
            RelationList = new List<SelectListItem>();
            HomeCountryList = new List<SelectListItem>();
            HomeStateList = new List<SelectListItem>();
            WorkCountryList = new List<SelectListItem>();
            WorkStateList = new List<SelectListItem>();
            OtherCountryList = new List<SelectListItem>();
            OtherStateList = new List<SelectListItem>();
            SpouseList = new List<SelectListItem>();

            getcheked = new List<string>();
        }

        public static List<ContactModel> GetEmegContactList(int Studentid)
        {
            //sess = (ClsSession)HttpContext.Current.Session["UserSessionClient"];
            var dbobj = new BiWeeklyRCPNewEntities();

            var Result = new List<ContactModel>();

            //ContactPersonal contactPersonal = new ContactPersonal();
            StudentContactRelationship contactRelation = new StudentContactRelationship();
            StudentAddresRel addressRelation = new StudentAddresRel();
            AddressList adrList = new AddressList();
            var contactPersonalList = dbobj.ContactPersonals.Where(objContactPersonal => objContactPersonal.StudentPersonalId == Studentid &&
                objContactPersonal.IsEmergency == true && objContactPersonal.Status == 1).ToList();



            foreach (var contactPersonal in contactPersonalList)
            {
                ContactModel contactModel = new ContactModel();
                contactModel.Id = contactPersonal.ContactPersonalId;
                contactModel.FirstName = contactPersonal.FirstName;
                contactModel.LastName = contactPersonal.LastName;
                contactModel.FirstNamePrefix = contactPersonal.Prefix;
                contactModel.LastNameSuffix = contactPersonal.Suffix;
                contactModel.Spouse = contactPersonal.Spouse;
                contactModel.MiddleName = contactPersonal.MiddleName;
                contactModel.ContactFlag = contactPersonal.ContactFlag;
                contactModel.PrimaryLanguage = contactPersonal.PrimaryLanguage;

                contactModel.IsBilling = contactPersonal.IsBilling.GetBool();
                contactModel.IsEmergency = contactPersonal.IsEmergency.GetBool();
                contactModel.IsIncident = contactPersonal.IsIncident.GetBool();
                contactModel.IsSchool = contactPersonal.IsSchool.GetBool();
                contactModel.Note = contactPersonal.Note;
                contactModel.SpouseId = contactPersonal.SpouseId;
                contactModel.IsOnCampusWithStaff = contactPersonal.IsOnCampusWithStaff.GetBool();
                contactModel.IsOnCampusAlone = contactPersonal.IsOnCampusAlone.GetBool();
                contactModel.IsOffCampus = contactPersonal.IsOffCampus.GetBool();

                contactModel.IsCorrespondence = contactPersonal.IsCorrespondence.GetBool();
                contactModel.IsCustody = contactPersonal.IsCustody.GetBool();
                contactModel.IsGuardian = contactPersonal.IsGuardian.GetBool();
                contactModel.IsNextOfKin = contactPersonal.IsNextOfKin.GetBool();
                contactModel.ApprovedVisitor = contactPersonal.ApprovedVisitor.GetBool();

                contactRelation = dbobj.StudentContactRelationships.Where(objContactrelation => objContactrelation.
                    ContactPersonalId == contactPersonal.ContactPersonalId).SingleOrDefault();
                contactModel.Relation = contactRelation.RelationshipId;

                var contactRelationName = new ContactModel();
                contactRelationName = (from objContactPersonal in dbobj.ContactPersonals
                                       join objContactRelation in dbobj.StudentContactRelationships on objContactPersonal.ContactPersonalId equals objContactRelation.ContactPersonalId
                                       join objLookUp in dbobj.LookUps on objContactRelation.RelationshipId equals objLookUp.LookupId
                                       where (objContactPersonal.StudentPersonalId == Studentid && objContactPersonal.Status == 1 && objContactRelation.ContactPersonalId == contactPersonal.ContactPersonalId)
                                       select new ContactModel
                                       {
                                           RelationName = objLookUp.LookupName,

                                       }).FirstOrDefault();


                if (contactRelationName != null)
                {
                    contactModel.RelationName = contactRelationName.RelationName;
                }

                LookUp lk = dbobj.LookUps.Where(objlk => objlk.LookupId == contactModel.Relation).SingleOrDefault();
                if (lk != null && lk.LookupName == "Parent")
                {
                    Parent prnt = dbobj.Parents.Where(objparent => objparent.ContactPersonalId == contactPersonal.ContactPersonalId).SingleOrDefault();
                    contactModel.UserID = prnt.Username;
                }
                else
                    contactModel.UserID = "";

                var addresses = dbobj.StudentAddresRels.Join(dbobj.ContactPersonals, objAddresRel => objAddresRel.ContactPersonalId,
                    objContactPersonal => objContactPersonal.ContactPersonalId,
                (objAddresRel, objContactPersonal) => new
                {
                    ClientId = objAddresRel.StudentPersonalId,
                    ContactSequance = objAddresRel.ContactSequence,
                    AddressId = objAddresRel.AddressId,
                    ContactID = objAddresRel.ContactPersonalId
                }).
                Where(x => x.ClientId == Studentid && x.ContactSequance > 0 && x.ContactID == contactPersonal.ContactPersonalId).ToList().OrderBy(x => x.ContactSequance);

                // var contactList = dbobj.AddressLists.Where(x => x.AddressId == contactPersonal.ContactPersonalId).ToList();
                foreach (var item in addresses)
                {
                    if (item.ContactSequance == 1)
                    {
                        adrList = dbobj.AddressLists.Where(x => x.AddressId == item.AddressId).SingleOrDefault();
                        contactModel.HomeAddressTypeId = adrList.AddressType;
                        contactModel.HomeAddressLine1 = adrList.ApartmentType;
                        contactModel.HomeAddressLine2 = adrList.StreetName;
                        contactModel.HomeCity = adrList.City;
                        contactModel.HomeState = Convert.ToInt32(adrList.StateProvince);

                        contactModel.HomeCountry = Convert.ToInt32(adrList.CountryId);
                        contactModel.HomeCounty = adrList.County;
                        contactModel.HomePhone = adrList.Phone;
                        contactModel.HomeMobilePhone = adrList.Mobile;
                        contactModel.HomeWorkPhone = adrList.OtherPhone;
                        contactModel.HomeFax = adrList.Fax;
                        contactModel.HomeEmail = adrList.PrimaryEmail;
                        contactModel.HomeOtherMail = adrList.EmailOther;
                        contactModel.HomeWorkEmail = adrList.SecondryEmail;
                        contactModel.HomeZip = adrList.PostalCode;

                        contactModel.HomeIsMailingAddress = adrList.IsMailingAddress.GetBool();
                        contactModel.HomeExtension = adrList.Extension;
                        contactModel.HomeMailOptIn = adrList.MailOptIn.GetBool();
                        contactModel.HomeEmailOptIn = adrList.EmailOptIn.GetBool();
                        contactModel.HomePhoneOptIn = adrList.PhoneOptIn.GetBool();
                        contactModel.HomeMailMergeOptIn = adrList.MailMergeOptIn.GetBool();

                    }
                    if (item.ContactSequance == 2)
                    {
                        adrList = dbobj.AddressLists.Where(x => x.AddressId == item.AddressId).SingleOrDefault();
                        contactModel.WorkAddressTypeId = adrList.AddressType;
                        contactModel.WorkAddressLine1 = adrList.ApartmentType;
                        contactModel.WorkAddressLine2 = adrList.StreetName;
                        contactModel.WorkCity = adrList.City;
                        contactModel.WorkState = Convert.ToInt32(adrList.StateProvince);
                        contactModel.WorkCountry = Convert.ToInt32(adrList.CountryId);
                        contactModel.WorkCounty = adrList.County;
                        contactModel.WorkHomePhone = adrList.Phone;
                        contactModel.WorkMobilePhone = adrList.Mobile;
                        contactModel.WorkPhone = adrList.OtherPhone;
                        contactModel.WorkFax = adrList.Fax;
                        //contactModel.WorkHomeEmail = adrList.PrimaryEmail;
                        //contactModel.WorkEmail = adrList.SecondryEmail;
                        contactModel.WorkEmail = adrList.PrimaryEmail;
                        contactModel.WorkZip = adrList.PostalCode;

                        contactModel.WorkIsMailingAddress = adrList.IsMailingAddress.GetBool();
                        contactModel.WorkExtension = adrList.Extension;
                        contactModel.WorkMailOptIn = adrList.MailOptIn.GetBool();
                        contactModel.WorkEmailOptIn = adrList.EmailOptIn.GetBool();
                        contactModel.WorkPhoneOptIn = adrList.PhoneOptIn.GetBool();
                        contactModel.WorkMailMergeOptIn = adrList.MailMergeOptIn.GetBool();


                    }
                    if (item.ContactSequance == 3)
                    {
                        adrList = dbobj.AddressLists.Where(x => x.AddressId == item.AddressId).SingleOrDefault();
                        contactModel.OtherAddressTypeId = adrList.AddressType;
                        contactModel.OtherAddressLine1 = adrList.ApartmentType;
                        contactModel.OtherAddressLine2 = adrList.StreetName;
                        contactModel.OtherCity = adrList.City;
                        contactModel.OtherState = Convert.ToInt32(adrList.StateProvince);
                        contactModel.OtherCountry = Convert.ToInt32(adrList.CountryId);
                        contactModel.OtherCounty = adrList.County;
                        contactModel.OtherHomePhone = adrList.Phone;
                        contactModel.OtherMobilePhone = adrList.Mobile;
                        contactModel.OtherWorkPhone = adrList.OtherPhone;
                        contactModel.OtherFax = adrList.Fax;
                        contactModel.OtherHomeEmail = adrList.PrimaryEmail;
                        contactModel.OtherWorkEmail = adrList.SecondryEmail;
                        contactModel.OtherZip = adrList.PostalCode;
                        contactModel.HomeStreet = adrList.StreetName;
                        contactModel.HomeNumber = adrList.ApartmentNumber;

                        contactModel.OtherIsMailingAddress = adrList.IsMailingAddress.GetBool();
                        contactModel.OtherExtension = adrList.Extension;
                        contactModel.OtherMailOptIn = adrList.MailOptIn.GetBool();
                        contactModel.OtherEmailOptIn = adrList.EmailOptIn.GetBool();
                        contactModel.OtherPhoneOptIn = adrList.PhoneOptIn.GetBool();
                        contactModel.OtherMailMergeOptIn = adrList.MailMergeOptIn.GetBool();

                    }
                }
                Result.Add(contactModel);
            }
            return Result;
        }

        public static List<ContactModel> GetEmergContactIndividual(int ContactPerId, int StudentId)
        {
            //sess = (ClsSession)HttpContext.Current.Session["UserSessionClient"];
            var dbobj = new BiWeeklyRCPNewEntities();

            var Result = new List<ContactModel>();

            //ContactPersonal contactPersonal = new ContactPersonal();
            StudentContactRelationship contactRelation = new StudentContactRelationship();
            StudentAddresRel addressRelation = new StudentAddresRel();
            AddressList adrList = new AddressList();
            var contactPersonalList = dbobj.ContactPersonals.Where(objContactPersonal => objContactPersonal.StudentPersonalId == StudentId &&
                objContactPersonal.IsEmergency == true && objContactPersonal.Status == 1 && objContactPersonal.ContactPersonalId == ContactPerId).ToList();



            foreach (var contactPersonal in contactPersonalList)
            {
                ContactModel contactModel = new ContactModel();
                contactModel.Id = contactPersonal.ContactPersonalId;
                contactModel.FirstName = contactPersonal.FirstName;
                contactModel.LastName = contactPersonal.LastName;
                contactModel.FirstNamePrefix = contactPersonal.Prefix;
                contactModel.LastNameSuffix = contactPersonal.Suffix;
                contactModel.Spouse = contactPersonal.Spouse;
                contactModel.MiddleName = contactPersonal.MiddleName;
                contactModel.ContactFlag = contactPersonal.ContactFlag;
                contactModel.PrimaryLanguage = contactPersonal.PrimaryLanguage;

                contactModel.IsBilling = contactPersonal.IsBilling.GetBool();
                contactModel.IsEmergency = contactPersonal.IsEmergency.GetBool();
                contactModel.IsIncident = contactPersonal.IsIncident.GetBool();
                contactModel.IsSchool = contactPersonal.IsSchool.GetBool();
                contactModel.Note = contactPersonal.Note;
                contactModel.SpouseId = contactPersonal.SpouseId;
                contactModel.IsOnCampusWithStaff = contactPersonal.IsOnCampusWithStaff.GetBool();
                contactModel.IsOnCampusAlone = contactPersonal.IsOnCampusAlone.GetBool();
                contactModel.IsOffCampus = contactPersonal.IsOffCampus.GetBool();

                contactModel.IsCorrespondence = contactPersonal.IsCorrespondence.GetBool();
                contactModel.IsCustody = contactPersonal.IsCustody.GetBool();
                contactModel.IsGuardian = contactPersonal.IsGuardian.GetBool();
                contactModel.IsNextOfKin = contactPersonal.IsNextOfKin.GetBool();
                contactModel.ApprovedVisitor = contactPersonal.ApprovedVisitor.GetBool();

                contactModel.Occupation = contactPersonal.Occupation;
                contactModel.Employer = contactPersonal.Employer;

                contactRelation = dbobj.StudentContactRelationships.Where(objContactrelation => objContactrelation.
                    ContactPersonalId == contactPersonal.ContactPersonalId).SingleOrDefault();
                contactModel.Relation = contactRelation.RelationshipId;
                LookUp lk = dbobj.LookUps.Where(objlk => objlk.LookupId == contactModel.Relation).SingleOrDefault();
                if (lk != null && lk.LookupName == "Parent")
                {
                    Parent prnt = dbobj.Parents.Where(objparent => objparent.ContactPersonalId == contactPersonal.ContactPersonalId).SingleOrDefault();
                    contactModel.UserID = prnt.Username;
                }
                else
                    contactModel.UserID = "";

                var addresses = dbobj.StudentAddresRels.Join(dbobj.ContactPersonals, objAddresRel => objAddresRel.ContactPersonalId,
                    objContactPersonal => objContactPersonal.ContactPersonalId,
                (objAddresRel, objContactPersonal) => new
                {
                    ClientId = objAddresRel.StudentPersonalId,
                    ContactSequance = objAddresRel.ContactSequence,
                    AddressId = objAddresRel.AddressId,
                    ContactID = objAddresRel.ContactPersonalId
                }).
                Where(x => x.ClientId == StudentId && x.ContactSequance > 0 && x.ContactID == contactPersonal.ContactPersonalId).ToList().OrderBy(x => x.ContactSequance);



                // var contactList = dbobj.AddressLists.Where(x => x.AddressId == contactPersonal.ContactPersonalId).ToList();
                foreach (var item in addresses)
                {
                    if (item.ContactSequance == 1)
                    {
                        
                        adrList = dbobj.AddressLists.Where(x => x.AddressId == item.AddressId).SingleOrDefault();
                        contactModel.HomeAddressTypeId = adrList.AddressType;
                        contactModel.HomeAddressLine1 = adrList.ApartmentType;
                        contactModel.HomeAddressLine2 = adrList.StreetName;
                        contactModel.HomeCity = adrList.City;
                        contactModel.HomeState = Convert.ToInt32(adrList.StateProvince);
                        if (contactModel.HomeState != 0 )
                        {
                            contactModel.HmeState = dbobj.LookUps.Where(objLookup => objLookup.LookupType == "State" && objLookup.LookupId == contactModel.HomeState).Select(objLookup => objLookup.LookupName).Single();
                        }
                        contactModel.HomeCountry = Convert.ToInt32(adrList.CountryId);
                        contactModel.HomeCounty = adrList.County;
                        contactModel.HomePhone = adrList.Phone;
                        contactModel.HomeMobilePhone = adrList.Mobile;
                        contactModel.HomeWorkPhone = adrList.OtherPhone;
                        contactModel.HomeExtension = adrList.Extension;
                        contactModel.HomeFax = adrList.Fax;
                        contactModel.HomeEmail = adrList.PrimaryEmail;
                        contactModel.HomeOtherMail = adrList.EmailOther;
                        contactModel.HomeWorkEmail = adrList.SecondryEmail;
                        contactModel.HomeZip = adrList.PostalCode;

                        contactModel.HomeIsMailingAddress = adrList.IsMailingAddress.GetBool();
                        contactModel.HomeMailOptIn = adrList.MailOptIn.GetBool();
                        contactModel.HomeEmailOptIn = adrList.EmailOptIn.GetBool();
                        contactModel.HomePhoneOptIn = adrList.PhoneOptIn.GetBool();
                        contactModel.HomeMailMergeOptIn = adrList.MailMergeOptIn.GetBool();

                    }
                    if (item.ContactSequance == 2)
                    {
                        adrList = dbobj.AddressLists.Where(x => x.AddressId == item.AddressId).SingleOrDefault();
                        contactModel.WorkAddressTypeId = adrList.AddressType;
                        contactModel.WorkAddressLine1 = adrList.ApartmentType;
                        contactModel.WorkAddressLine2 = adrList.StreetName;
                        contactModel.WorkCity = adrList.City;
                        contactModel.WorkState = Convert.ToInt32(adrList.StateProvince);
                        if (contactModel.WorkState != 0)
                        {
                            contactModel.WrkState = dbobj.LookUps.Where(objLookup => objLookup.LookupType == "State" && objLookup.LookupId == contactModel.WorkState).Select(objLookup => objLookup.LookupName).Single();
                        }
                        contactModel.WorkCountry = Convert.ToInt32(adrList.CountryId);
                        contactModel.WorkCounty = adrList.County;
                        contactModel.WorkHomePhone = adrList.Phone;
                        contactModel.WorkMobilePhone = adrList.Mobile;
                        contactModel.WorkPhone = adrList.OtherPhone;
                        contactModel.WorkFax = adrList.Fax;
                        //contactModel.WorkHomeEmail = adrList.PrimaryEmail;
                        //contactModel.WorkEmail = adrList.SecondryEmail;
                        contactModel.WorkEmail = adrList.PrimaryEmail;
                        contactModel.WorkZip = adrList.PostalCode;

                        contactModel.WorkIsMailingAddress = adrList.IsMailingAddress.GetBool();
                        contactModel.WorkExtension = adrList.Extension;
                        contactModel.OtherExtension2 = adrList.Extension2;
                        contactModel.WorkMailOptIn = adrList.MailOptIn.GetBool();
                        contactModel.WorkEmailOptIn = adrList.EmailOptIn.GetBool();
                        contactModel.WorkPhoneOptIn = adrList.PhoneOptIn.GetBool();
                        contactModel.WorkMailMergeOptIn = adrList.MailMergeOptIn.GetBool();


                    }
                    if (item.ContactSequance == 3)
                    {
                        adrList = dbobj.AddressLists.Where(x => x.AddressId == item.AddressId).SingleOrDefault();
                        contactModel.OtherAddressTypeId = adrList.AddressType;
                        contactModel.OtherAddressLine1 = adrList.ApartmentType;
                        contactModel.OtherAddressLine2 = adrList.StreetName;
                        contactModel.OtherCity = adrList.City;
                        contactModel.OtherState = Convert.ToInt32(adrList.StateProvince);
                        if (contactModel.OtherState != 0)
                        {
                            contactModel.OthState = dbobj.LookUps.Where(objLookup => objLookup.LookupType == "State" && objLookup.LookupId == contactModel.OtherState).Select(objLookup => objLookup.LookupName).Single();
                        }
                        contactModel.OtherCountry = Convert.ToInt32(adrList.CountryId);
                        contactModel.OtherCounty = adrList.County;
                        contactModel.OtherHomePhone = adrList.Phone;
                        contactModel.OtherMobilePhone = adrList.Mobile;
                        contactModel.OtherWorkPhone = adrList.OtherPhone;
                        contactModel.OtherFax = adrList.Fax;
                        contactModel.OtherHomeEmail = adrList.PrimaryEmail;
                        contactModel.OtherWorkEmail = adrList.SecondryEmail;
                        contactModel.OtherZip = adrList.PostalCode;
                        contactModel.HomeStreet = adrList.StreetName;
                        contactModel.HomeNumber = adrList.ApartmentNumber;

                        contactModel.OtherIsMailingAddress = adrList.IsMailingAddress.GetBool();
                        contactModel.OtherExtension = adrList.Extension;
                        contactModel.OtherExtension3 = adrList.Extension2;
                        contactModel.OtherMailOptIn = adrList.MailOptIn.GetBool();
                        contactModel.OtherEmailOptIn = adrList.EmailOptIn.GetBool();
                        contactModel.OtherPhoneOptIn = adrList.PhoneOptIn.GetBool();
                        contactModel.OtherMailMergeOptIn = adrList.MailMergeOptIn.GetBool();

                    }
                }
                Result.Add(contactModel);
            }
            return Result;
        }

        public static List<ContactModel> GetApprovedVisitor(int Studentid)
        {
            //sess = (ClsSession)HttpContext.Current.Session["UserSessionClient"];
            var dbobj = new BiWeeklyRCPNewEntities();

            var Result = new List<ContactModel>();

            //ContactPersonal contactPersonal = new ContactPersonal();
            StudentContactRelationship contactRelation = new StudentContactRelationship();
            StudentAddresRel addressRelation = new StudentAddresRel();
            AddressList adrList = new AddressList();
            var contactApproved = dbobj.ContactPersonals.Where(objContactPersonal => objContactPersonal.StudentPersonalId == Studentid && (objContactPersonal.IsOnCampusWithStaff == true || objContactPersonal.IsOnCampusAlone == true || objContactPersonal.IsOffCampus == true) &&
                  objContactPersonal.Status == 1).ToList();



            foreach (var contactAppove in contactApproved)
            {
                ContactModel contactModel = new ContactModel();
                contactModel.Id = contactAppove.ContactPersonalId;
                contactModel.FirstName = contactAppove.FirstName;
                contactModel.LastName = contactAppove.LastName;
                contactModel.FirstNamePrefix = contactAppove.Prefix;
                contactModel.LastNameSuffix = contactAppove.Suffix;
                contactModel.Spouse = contactAppove.Spouse;
                contactModel.MiddleName = contactAppove.MiddleName;
                contactModel.ContactFlag = contactAppove.ContactFlag;
                contactModel.ApprovedVisitor = contactAppove.ApprovedVisitor.GetBool();
                contactModel.Occupation = contactAppove.Occupation;
                contactModel.Employer = contactAppove.Employer;
                contactModel.IsGuardian = contactAppove.IsGuardian.GetBool();
                contactModel.IsIncident = contactAppove.IsIncident.GetBool();
                contactModel.IsCorrespondence = contactAppove.IsCorrespondence.GetBool();
                contactModel.IsEmergency = contactAppove.IsEmergency.GetBool();
                contactModel.IsOnCampusWithStaff = contactAppove.IsOnCampusWithStaff.GetBool();
                contactModel.IsOnCampusAlone = contactAppove.IsOnCampusAlone.GetBool();
                contactModel.IsOffCampus = contactAppove.IsOffCampus.GetBool();
                var contactRelationName = new ContactModel();
                contactRelationName = (from objContactPersonal in dbobj.ContactPersonals
                                       join objContactRelation in dbobj.StudentContactRelationships on objContactPersonal.ContactPersonalId equals objContactRelation.ContactPersonalId
                                       join objLookUp in dbobj.LookUps on objContactRelation.RelationshipId equals objLookUp.LookupId
                                       where (objContactPersonal.StudentPersonalId == Studentid && objContactPersonal.Status == 1 && objContactRelation.ContactPersonalId == contactAppove.ContactPersonalId)
                                       select new ContactModel
                                       {
                                           RelationName = objLookUp.LookupName
                                       }).FirstOrDefault();

                if (contactRelationName != null)
                {
                    contactModel.RelationName = contactRelationName.RelationName;
                }
                var addresses = dbobj.StudentAddresRels.Join(dbobj.ContactPersonals, objAddresRel => objAddresRel.ContactPersonalId,
                    objContactPersonal => objContactPersonal.ContactPersonalId,
                (objAddresRel, objContactPersonal) => new
                {
                    ClientId = objAddresRel.StudentPersonalId,
                    ContactSequance = objAddresRel.ContactSequence,
                    AddressId = objAddresRel.AddressId,
                    ContactID = objAddresRel.ContactPersonalId
                }).
                Where(x => x.ClientId == Studentid && x.ContactSequance > 0 && x.ContactID == contactAppove.ContactPersonalId).ToList().OrderBy(x => x.ContactSequance);



                // var contactList = dbobj.AddressLists.Where(x => x.AddressId == contactPersonal.ContactPersonalId).ToList();
                foreach (var item in addresses)
                {
                    if (item.ContactSequance == 1)
                    {
                        adrList = dbobj.AddressLists.Where(x => x.AddressId == item.AddressId).SingleOrDefault();
                        contactModel.HomeAddressTypeId = adrList.AddressType;
                        contactModel.HomeAddressLine1 = adrList.ApartmentType;
                        contactModel.HomeAddressLine2 = adrList.StreetName;
                        contactModel.HomeCity = adrList.City;
                        contactModel.HomeState = Convert.ToInt32(adrList.StateProvince);
                        contactModel.HomeCountry = Convert.ToInt32(adrList.CountryId);
                        contactModel.HomeCounty = adrList.County;
                        contactModel.HomePhone = adrList.Phone;
                        contactModel.HomeMobilePhone = adrList.Mobile;
                        contactModel.HomeWorkPhone = adrList.OtherPhone;
                        contactModel.HomeFax = adrList.Fax;
                        contactModel.HomeEmail = adrList.PrimaryEmail;
                        contactModel.HomeOtherMail = adrList.EmailOther;
                        contactModel.HomeWorkEmail = adrList.SecondryEmail;
                        contactModel.HomeZip = adrList.PostalCode;

                        contactModel.HomeIsMailingAddress = adrList.IsMailingAddress.GetBool();
                        contactModel.HomeExtension = adrList.Extension;
                        contactModel.HomeMailOptIn = adrList.MailOptIn.GetBool();
                        contactModel.HomeEmailOptIn = adrList.EmailOptIn.GetBool();
                        contactModel.HomePhoneOptIn = adrList.PhoneOptIn.GetBool();
                        contactModel.HomeMailMergeOptIn = adrList.MailMergeOptIn.GetBool();

                    }
                    if (item.ContactSequance == 2)
                    {
                        adrList = dbobj.AddressLists.Where(x => x.AddressId == item.AddressId).SingleOrDefault();
                        contactModel.WorkAddressTypeId = adrList.AddressType;
                        contactModel.WorkAddressLine1 = adrList.ApartmentType;
                        contactModel.WorkAddressLine2 = adrList.StreetName;
                        contactModel.WorkCity = adrList.City;
                        contactModel.WorkState = Convert.ToInt32(adrList.StateProvince);
                        contactModel.WorkCountry = Convert.ToInt32(adrList.CountryId);
                        contactModel.WorkCounty = adrList.County;
                        contactModel.WorkHomePhone = adrList.Phone;
                        contactModel.WorkMobilePhone = adrList.Mobile;
                        contactModel.WorkPhone = adrList.OtherPhone;
                        contactModel.WorkFax = adrList.Fax;
                        //contactModel.WorkHomeEmail = adrList.PrimaryEmail;
                        //contactModel.WorkEmail = adrList.SecondryEmail;
                        contactModel.WorkEmail = adrList.PrimaryEmail;
                        contactModel.WorkZip = adrList.PostalCode;

                        contactModel.WorkIsMailingAddress = adrList.IsMailingAddress.GetBool();
                        contactModel.WorkExtension = adrList.Extension;
                        contactModel.WorkMailOptIn = adrList.MailOptIn.GetBool();
                        contactModel.WorkEmailOptIn = adrList.EmailOptIn.GetBool();
                        contactModel.WorkPhoneOptIn = adrList.PhoneOptIn.GetBool();
                        contactModel.WorkMailMergeOptIn = adrList.MailMergeOptIn.GetBool();


                    }
                    if (item.ContactSequance == 3)
                    {
                        adrList = dbobj.AddressLists.Where(x => x.AddressId == item.AddressId).SingleOrDefault();
                        contactModel.OtherAddressTypeId = adrList.AddressType;
                        contactModel.OtherAddressLine1 = adrList.ApartmentType;
                        contactModel.OtherAddressLine2 = adrList.StreetName;
                        contactModel.OtherCity = adrList.City;
                        contactModel.OtherState = Convert.ToInt32(adrList.StateProvince);
                        contactModel.OtherCountry = Convert.ToInt32(adrList.CountryId);
                        contactModel.OtherCounty = adrList.County;
                        contactModel.OtherHomePhone = adrList.Phone;
                        contactModel.OtherMobilePhone = adrList.Mobile;
                        contactModel.OtherWorkPhone = adrList.OtherPhone;
                        contactModel.OtherFax = adrList.Fax;
                        contactModel.OtherHomeEmail = adrList.PrimaryEmail;
                        contactModel.OtherWorkEmail = adrList.SecondryEmail;
                        contactModel.OtherZip = adrList.PostalCode;
                        contactModel.HomeStreet = adrList.StreetName;
                        contactModel.HomeNumber = adrList.ApartmentNumber;

                        contactModel.OtherIsMailingAddress = adrList.IsMailingAddress.GetBool();
                        contactModel.OtherExtension = adrList.Extension;
                        contactModel.OtherMailOptIn = adrList.MailOptIn.GetBool();
                        contactModel.OtherEmailOptIn = adrList.EmailOptIn.GetBool();
                        contactModel.OtherPhoneOptIn = adrList.PhoneOptIn.GetBool();
                        contactModel.OtherMailMergeOptIn = adrList.MailMergeOptIn.GetBool();

                    }
                }
                Result.Add(contactModel);
            }
            return Result;
        }

        public static List<ContactModel> GetApprovedVisitorIndividual(int ContactPerId, int Studentid)
        {
            //sess = (ClsSession)HttpContext.Current.Session["UserSessionClient"];
            var dbobj = new BiWeeklyRCPNewEntities();

            var Result = new List<ContactModel>();

            //ContactPersonal contactPersonal = new ContactPersonal();
            StudentContactRelationship contactRelation = new StudentContactRelationship();
            StudentAddresRel addressRelation = new StudentAddresRel();
            AddressList adrList = new AddressList();
            var contactApproved = dbobj.ContactPersonals.Where(objContactPersonal => objContactPersonal.StudentPersonalId == Studentid &&
                (objContactPersonal.IsOnCampusWithStaff == true || objContactPersonal.IsOnCampusAlone == true || objContactPersonal.IsOffCampus == true) && objContactPersonal.Status == 1 && objContactPersonal.ContactPersonalId == ContactPerId).ToList();



            foreach (var contactAppove in contactApproved)
            {
                ContactModel contactModel = new ContactModel();
                contactModel.Id = contactAppove.ContactPersonalId;
                contactModel.FirstName = contactAppove.FirstName;
                contactModel.LastName = contactAppove.LastName;
                contactModel.FirstNamePrefix = contactAppove.Prefix;
                contactModel.LastNameSuffix = contactAppove.Suffix;
                contactModel.Spouse = contactAppove.Spouse;
                contactModel.MiddleName = contactAppove.MiddleName;
                contactModel.ContactFlag = contactAppove.ContactFlag;
                contactModel.ApprovedVisitor = contactAppove.ApprovedVisitor.GetBool();
                contactModel.Occupation = contactAppove.Occupation;
                contactModel.Employer = contactAppove.Employer;
               

                var addresses = dbobj.StudentAddresRels.Join(dbobj.ContactPersonals, objAddresRel => objAddresRel.ContactPersonalId,
                    objContactPersonal => objContactPersonal.ContactPersonalId,
                (objAddresRel, objContactPersonal) => new
                {
                    ClientId = objAddresRel.StudentPersonalId,
                    ContactSequance = objAddresRel.ContactSequence,
                    AddressId = objAddresRel.AddressId,
                    ContactID = objAddresRel.ContactPersonalId
                }).
                Where(x => x.ClientId == Studentid && x.ContactSequance > 0 && x.ContactID == contactAppove.ContactPersonalId).ToList().OrderBy(x => x.ContactSequance);



                // var contactList = dbobj.AddressLists.Where(x => x.AddressId == contactPersonal.ContactPersonalId).ToList();
                foreach (var item in addresses)
                {
                    if (item.ContactSequance == 1)
                    {
                        adrList = dbobj.AddressLists.Where(x => x.AddressId == item.AddressId).SingleOrDefault();
                        contactModel.HomeAddressTypeId = adrList.AddressType;
                        contactModel.HomeAddressLine1 = adrList.ApartmentType;
                        contactModel.HomeAddressLine2 = adrList.StreetName;
                        contactModel.HomeCity = adrList.City;
                        contactModel.HomeState = Convert.ToInt32(adrList.StateProvince);
                        if (contactModel.HomeState != 0)
                        {
                            contactModel.HmeState = dbobj.LookUps.Where(objLookup => objLookup.LookupType == "State" && objLookup.LookupId == contactModel.HomeState).Select(objLookup => objLookup.LookupName).Single();
                        }
                        contactModel.HomeCountry = Convert.ToInt32(adrList.CountryId);
                        contactModel.HomeCounty = adrList.County;
                        contactModel.HomePhone = adrList.Phone;
                        contactModel.HomeMobilePhone = adrList.Mobile;
                        contactModel.HomeWorkPhone = adrList.OtherPhone;
                        contactModel.HomeFax = adrList.Fax;
                        contactModel.HomeEmail = adrList.PrimaryEmail;
                        contactModel.HomeOtherMail = adrList.EmailOther;
                        contactModel.HomeWorkEmail = adrList.SecondryEmail;
                        contactModel.HomeZip = adrList.PostalCode;

                        contactModel.HomeIsMailingAddress = adrList.IsMailingAddress.GetBool();
                        contactModel.HomeExtension = adrList.Extension;
                        contactModel.HomeMailOptIn = adrList.MailOptIn.GetBool();
                        contactModel.HomeEmailOptIn = adrList.EmailOptIn.GetBool();
                        contactModel.HomePhoneOptIn = adrList.PhoneOptIn.GetBool();
                        contactModel.HomeMailMergeOptIn = adrList.MailMergeOptIn.GetBool();

                    }
                    if (item.ContactSequance == 2)
                    {
                        adrList = dbobj.AddressLists.Where(x => x.AddressId == item.AddressId).SingleOrDefault();
                        contactModel.WorkAddressTypeId = adrList.AddressType;
                        contactModel.WorkAddressLine1 = adrList.ApartmentType;
                        contactModel.WorkAddressLine2 = adrList.StreetName;
                        contactModel.WorkCity = adrList.City;
                        contactModel.WorkState = Convert.ToInt32(adrList.StateProvince);
                        if (contactModel.WorkState != 0)
                        {
                            contactModel.WrkState = dbobj.LookUps.Where(objLookup => objLookup.LookupType == "State" && objLookup.LookupId == contactModel.WorkState).Select(objLookup => objLookup.LookupName).Single();
                        }
                        contactModel.WorkCountry = Convert.ToInt32(adrList.CountryId);
                        contactModel.WorkCounty = adrList.County;
                        contactModel.WorkHomePhone = adrList.Phone;
                        contactModel.WorkMobilePhone = adrList.Mobile;
                        contactModel.WorkPhone = adrList.OtherPhone;
                        contactModel.WorkFax = adrList.Fax;
                        //contactModel.WorkHomeEmail = adrList.PrimaryEmail;
                        //contactModel.WorkEmail = adrList.SecondryEmail;
                        contactModel.WorkEmail = adrList.PrimaryEmail;
                        contactModel.WorkZip = adrList.PostalCode;

                        contactModel.WorkIsMailingAddress = adrList.IsMailingAddress.GetBool();
                        contactModel.WorkExtension = adrList.Extension;
                        contactModel.OtherExtension2 = adrList.Extension2;
                        contactModel.WorkMailOptIn = adrList.MailOptIn.GetBool();
                        contactModel.WorkEmailOptIn = adrList.EmailOptIn.GetBool();
                        contactModel.WorkPhoneOptIn = adrList.PhoneOptIn.GetBool();
                        contactModel.WorkMailMergeOptIn = adrList.MailMergeOptIn.GetBool();


                    }
                    if (item.ContactSequance == 3)
                    {
                        adrList = dbobj.AddressLists.Where(x => x.AddressId == item.AddressId).SingleOrDefault();
                        contactModel.OtherAddressTypeId = adrList.AddressType;
                        contactModel.OtherAddressLine1 = adrList.ApartmentType;
                        contactModel.OtherAddressLine2 = adrList.StreetName;
                        contactModel.OtherCity = adrList.City;
                        contactModel.OtherState = Convert.ToInt32(adrList.StateProvince);
                        if (contactModel.OtherState != 0)
                        {
                            contactModel.OthState = dbobj.LookUps.Where(objLookup => objLookup.LookupType == "State" && objLookup.LookupId == contactModel.OtherState).Select(objLookup => objLookup.LookupName).Single();
                        }
                        contactModel.OtherCountry = Convert.ToInt32(adrList.CountryId);
                        contactModel.OtherCounty = adrList.County;
                        contactModel.OtherHomePhone = adrList.Phone;
                        contactModel.OtherMobilePhone = adrList.Mobile;
                        contactModel.OtherWorkPhone = adrList.OtherPhone;
                        contactModel.OtherFax = adrList.Fax;
                        contactModel.OtherHomeEmail = adrList.PrimaryEmail;
                        contactModel.OtherWorkEmail = adrList.SecondryEmail;
                        contactModel.OtherZip = adrList.PostalCode;
                        contactModel.HomeStreet = adrList.StreetName;
                        contactModel.HomeNumber = adrList.ApartmentNumber;

                        contactModel.OtherIsMailingAddress = adrList.IsMailingAddress.GetBool();
                        contactModel.OtherExtension = adrList.Extension;
                        contactModel.OtherExtension3 = adrList.Extension2;
                        contactModel.OtherMailOptIn = adrList.MailOptIn.GetBool();
                        contactModel.OtherEmailOptIn = adrList.EmailOptIn.GetBool();
                        contactModel.OtherPhoneOptIn = adrList.PhoneOptIn.GetBool();
                        contactModel.OtherMailMergeOptIn = adrList.MailMergeOptIn.GetBool();

                    }
                }
                Result.Add(contactModel);
            }
            return Result;
        }


    }
    public class checkBoxViewModel
    {
        public string name { get; set; }
        public bool check { get; set; }
    }
    public class contactFirstName
    {

        public string username { get; set; }
        public int Usercount { get; set; }
    }
}