<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ClientDB.Models.RegistrationModel>" %>
<style type="text/css">
    div.middleContainer table tr td {
        border-bottom: 1px solid #DDDDDD;
        border-right: 1px solid #DDDDDD;
        color: #525252;
        font-family: Arial,Helvetica,sans-serif;
        font-size: 11px !important;
        font-weight: bold;
        padding: 8px 1px;
        width: 25%;
    }
    .auto-style1 {
        height: 23px;
    }
</style>
<div style="width: 100%">
    <div>
        <table>

            <tr>
                
                <td style="border-right: none;" colspan="4">&nbsp;</td>
            </tr>
            
            <tr>
                
                <td>Legal Name(Last, First, MI)</td>
                <td>
                    <%=Model.LastName%><%=Model.LastNameSuffix%>,
                    <%=Model.FirstName%>,
                    <%=Model.MiddleName%>
                </td>
                <td>Nick Name</td>
                <td><%=Model.NickName%></td>
            </tr>
            
            <tr>
                <td>Admission Date </td>
                <td>
                    <%=Model.AdmissinDate%>
                <td>Gender</td>
                <%if (Model.Gender == "1")
                  { %>
                <td>Male</td>
                <%}
                  else if (Model.Gender == "2")
                  { %>
                <td>Female</td>
                <%} %>
            </tr>
            <tr>
                <td>Date of Birth </td>
                <td>
                    <%=Model.DateOfBirth%>
                <td>Race</td>
                <td><%=Model.StrRace%></td>
            </tr>
            <tr>
                <td>Place of Birth</td>
                <td>
                    <%=Model.PlaceOfBirth%></td>
                <td>Country of Birth</td>
                <td><%=Model.CountryBirth%></td>
            </tr>
            <tr>
                <td>State of Birth</td>
                <td>
                    <%=Model.StateBirth%></td>
                <td>Citizenship</td>
                <td><%=Model.CitizenshipBirth%></td>
            </tr>
            <tr>
                <td>Height (in)</td>
                <td>
                    <%=Model.Height%></td>
                <td>Weight (lbs)</td>
                <td><%=Model.Weight%></td>
            </tr>
            <tr>
                <td>Hair Color</td>
                <td>
                    <%=Model.HairColor%></td>
                <td>Eye Color</td>
                <td><%=Model.EyeColor%></td>
            </tr>
            <tr>
                <td>Primary Language</td>
                <td>
                    <%=Model.PrimaryLanguage%></td>
                <td>Legal Competency Status</td>
                <td><%=Model.LegalCompetencyStatus%></td>
            </tr>
            <tr>
                <td>Guardianship Status</td>
                <td>
                    <%=Model.GuardianshipStatus%></td>
                <td>Other State Agencies Involved With Student</td>
                <td><%=Model.OtherStateAgenciesInvolvedWithStudent%></td>
            </tr>
            <tr>
                <td>Distinguishing Marks</td>
                <td>
                    <%=Model.DistigushingMarks%></td>
                <td>Marital Status of Both Parents</td>
                <td><%=Model.MaritalStatusofBothParents%></td>
            </tr>
            <tr>
                <td>Case Manager Residential</td>
                <td>
                    <%=Model.CaseManagerResidential%></td>
                <td>Case Manager Educational</td>
                <td><%=Model.CaseManagerEducational%></td>
            </tr>
            <tr>
                <td>Educational Surrogate:(If applicable):</td>
                <td>
                    <%=Model.EducationalSurrogate%></td>
                <td colspan="2"></td>
                
            </tr>                      
                
            <tr>
                <td>Primary Nurse</td>
                <td colspan="3"><%=Model.PrimaryNurseMT%></td>
            </tr>
            <tr>
                <td>Wellness Check Status</td>
                <td colspan="3"><%=Model.WellnessCheckStatusMT%></td>
            </tr>
            
                
            <tr>
                <td style="border-right: none; padding-top: 10px;" colspan="4">
                    <h4>Emergency Contacts – Personal</h4>
                </td>
                
            </tr>
       <%if (Model.EmergencyContactList != null && Model.EmergencyContactList.Any())
              {foreach(var data in Model.EmergencyContactList)
              { %>
           <tr>
               <td colspan="4">
                   <table style="width:100%">
                       <tr>
                           <td>Relation</td>
                           <td>  <%= data.Relation  %></td>
                           <td>Full Name</td>
                           <td><%= data.Name %></td>
                           <td>Primary Language</td>
                           <td><%= data.PrimaryLanguage %></td>
                       </tr>
                       <tr>
                           <td rowspan="3">Address</td>
                           <td colspan="3" rowspan="3"><%= data.Address%></td>
                           <td>Home Phone</td>
                           <td><%= data.Phone %></td>
                       </tr>
                       <tr>
                           <td>Other Phone</td>
                           <td><%=data.OtherPhone%></td>
                       </tr>
                       <tr>
                           <td>Email</td>
                           <td><%=data.PrimaryEmail%></td>
                       </tr>
                   </table>
               </td>
           </tr>
            <%}}
              
              else
         {
             %>
           
            <tr>
               <td colspan="4">
                   <table style="width:100%">
                       <tr>
                           <td>Relation</td>
                           <td></td>
                           <td>Full Name</td>
                           <td></td>
                           <td>Primary Language</td>
                           <td></td>
                       </tr>
                       <tr>
                           <td rowspan="3">Address</td>
                           <td colspan="3" rowspan="3"></td>
                           <td>Home Phone</td>
                           <td></td>
                       </tr>
                       <tr>
                           <td>Other Phone</td>
                           <td></td>
                       </tr>
                       <tr>
                           <td>Email</td>
                           <td></td>
                       </tr>
                   </table>
               </td>
           </tr>

            <%
         } %>
            
            <tr>
                <td style="border-right: none; padding-top: 10px;" colspan="4">
                    <h4>Emergency Contacts - School</h4>
                </td>
            </tr>
            
            <tr>

                <td>First Name</td>
                <td>
                    <%=Model.EmergencyContactFirstName1%></td>
                <td>Last Name</td>
                <td><%=Model.EmergencyContactLastName1%></td>
            </tr>
            <tr>
                <td>Title</td>
                <td>
                    <%=Model.EmergencyContactTitle1%></td>
                <td>Phone</td>
                <td><%=Model.EmergencyContactPhone1%></td>
            </tr>
                       
            <tr>
                <td>First Name</td>
                <td>
                    <%=Model.EmergencyContactFirstName2%></td>
                <td>Last Name</td>
                <td><%=Model.EmergencyContactLastName2%></td>
            </tr>
            <tr>
                <td>Title</td>
                <td>
                    <%=Model.EmergencyContactTitle2%></td>
                <td>Phone</td>
                <td><%=Model.EmergencyContactPhone2%></td>
            </tr>
                        
            <tr>
                <td>First Name</td>
                <td>
                    <%=Model.EmergencyContactFirstName3%></td>
                <td>Last Name</td>
                <td><%=Model.EmergencyContactLastName3%></td>
            </tr>
            <tr>
                <td>Title</td>
                <td>
                    <%=Model.EmergencyContactTitle3%></td>
                <td>Phone</td>
                <td><%=Model.EmergencyContactPhone3%></td>
            </tr>
            
            <tr>
                <td>First Name</td>
                <td>
                    <%=Model.EmergencyContactFirstName4%></td>
                <td>Last Name</td>
                <td><%=Model.EmergencyContactLastName4%></td>
            </tr>
            <tr>
                <td>Title</td>
                <td>
                    <%=Model.EmergencyContactTitle4%></td>
                <td>Phone</td>
                <td><%=Model.EmergencyContactPhone4%></td>
            </tr>
            
            <tr>
                <td>First Name</td>
                <td>
                    <%=Model.EmergencyContactFirstName5%></td>
                <td>Last Name</td>
                <td><%=Model.EmergencyContactLastName5%></td>
            </tr>
            <tr>
                <td>Title</td>
                <td>
                    <%=Model.EmergencyContactTitle5%></td>
                <td>Phone</td>
                <td><%=Model.EmergencyContactPhone5%></td>
            </tr>
            
            <tr>
                <td style="border-right: none; padding-top: 10px;" colspan="4">
                    <h4>Medical and Insurance</h4>
                </td>
            </tr>
                        
            <tr>
                <td colspan="4"><b>Primary Physician</b></td>
            </tr>
            <tr>
                <td>Physician Name</td>
                <td><%=Model.PCPNameMT%></td>
                <td>Phone</td>
                <td><%=Model.PCPPhoneMT%></td>
            </tr>

            <tr>
                <td>Address</td>
                <td colspan="3"><%=Model.PCPAddressMT%></td>
            </tr>          
            
            <tr>
                <td class="nobdr", colspan="4">&nbsp;</td>
            </tr>
            
            <tr>
                <td colspan="4" colspan="4"><b>Insurance</b></td>
            </tr>
            <%
                   foreach (var Ins in Model.InsuranceList)
                   {
            %>
            <tr>
                <td>Insurance Type</td>
                <td><%=Ins.InsuranceType%></td>
                <td>Policy Number</td>
                <td><%=Ins.PolicyNumber%></td>
            </tr>

            <tr>
                <td>Policy Holder</td>
                <td><%=Ins.PolicyHolder%></td>
            </tr>

           <%} %>

            <tr>
                <td class="nobdr" colspan="2">&nbsp;</td>
            </tr>
            
            <tr>
                <td>Date Of Last Physical Exam</td>
                <td><%=Model.DateOfLastPhysicalExam%></td>
            </tr>
            
            <tr>
                <td class="auto-style1">Medical Conditions/Diagnosis</td>
                <td class="auto-style1"><%=Model.MedicalConditionOrDiagnosis%></td>
            </tr>
            
            <tr>
                <td>Allergies</td>
                <td><%=Model.Allergies%></td>
            </tr>
            
            <tr>
                <td>Current Medications</td>
                <td><%=Model.CurrentMedications%></td>
            </tr>
            
            <tr>
                <td>Self Preservation Ability</td>
                <td><%=Model.SelfPreservationAbilityGT%></td>
            </tr>
            
            <tr>
                <td>Significant Behavior Characteristics</td>
                <td><%=Model.SignificantBehavioralCharacteristicsGT%></td>
            </tr>
            
            <tr>
                <td rowspan="3">Relevent Capabilities,Limitations,and Preferences</td>
                <td><b>Capabilities</b><br>
                    <%=Model.CapabilitiesGT%></td>
            </tr>
            <tr>
                <td><b>Limitations</b><br>
                    <%=Model.LimitationsGT%></td>
            </tr>
            <tr>
                <td><b>Preferences</b><br>
                    <%=Model.PreferencesGT%></td>
            </tr>
            
            
            <tr>
                <td style="border-right: none; padding-top: 10px;" colspan="4">
                    <h4>Referral/IEP Information</h4>
                </td>
            </tr>

            <tr>
                <td>Full Name</td>
                <td><%=Model.ReferralIEPFullName%></td>
                <td>Title</td>
                <td><%=Model.ReferralIEPTitle%></td>
            </tr>
            <tr>
                <td>Phone</td>
                <td><%=Model.ReferralIEPPhone%></td>
                <td>Referring Agency</td>
                <td><%=Model.ReferralIEPReferringAgency%></td>
            </tr>
            <tr>
                <td>Source Of Tuition</td>
                <td colspan="3"><%=Model.ReferralIEPSourceofTuition%></td>
            </tr>

            <tr>
                <td  style="border-right: none; padding-top: 10px;" colspan="4">
                    <h4>Education History</h4>
                </td>
            </tr>
            
            <tr>
                <td class="auto-style5">Date Initially Eligible for Special Education</td>
                <td class="auto-style6">
                    <%=Model.DateInitiallyEligibleforSpecialEducation%></td>
                <td class="auto-style7">Date of Most Recent Special Education Evaluations</td>
                <td class="nobdr">
                    <%=Model.DateofMostRecentSpecialEducationEvaluations%></td>
            </tr>
            <tr>
                <td>Date of Next Scheduled 3-Year Evaluation</td>
                <td>
                    <%=Model.DateofNextScheduled3YearEvaluation%></td>
                <td>Current IEP Start Date</td>
                <td class="nobdr">
                    <%=Model.CurrentIEPStartDate%></td>
            </tr>
            <tr>
                <td>Current IEP Expiration Date</td>
                <td>
                    <%=Model.CurrentIEPExpirationDate%></td>
            </tr>

            <tr>
                <td  style="border-right: none; padding-top: 10px;" style="border-right: none; padding-top: 10px;" colspan="4">
                    <h4>Schools Attended</h4>
                </td>
            </tr>
            
            <tr>
                <td class="auto-style1">School Name</td>
                <td class="auto-style1">
                    <%=Model.SchoolName1%></td>                
            </tr>
            <tr>
                <td>Dates Attended<br />
                </td>
                <td>From: &nbsp<%=Model.DateFrom1%>&nbsp To:&nbsp<%=Model.DateTo1%> </td>
            </tr>
            <tr>
                <td>Address Line 1</td>
                <td><%=Model.SchoolAttendedAddress11%></td>
            </tr>
            <tr>
                <td>Address Line 2</td>
                <td>
                    <%=Model.SchoolAttendedAddress21%>
                    </td>
            </tr>
            
            <tr>
                <td>City</td>
                <td>
                    <%=Model.SchoolAttendedCity1%></td>
             </tr>
            <tr>
                <td>State</td>
                <td><%=Model.SchoolAttendedState1%></td>
            </tr>
            
            <tr>
                <td class="nobdr" colspan="2"></td>
            </tr>
            
            <tr>
                <td>School Name</td>
                <td>
                    <%=Model.SchoolName2%></td>                
            </tr>
            <tr>
                <td>Dates Attended<br />
                </td>
                <td>From:&nbsp<%=Model.DateFrom2%>&nbsp To:&nbsp<%=Model.DateTo2%></td>
            </tr>
            <tr>
                <td>Address Line 1</td>
                <td><%=Model.SchoolAttendedAddress12%></td>
            </tr>
            <tr>
                <td>Address Line 2</td>
                <td>
                    <%=Model.SchoolAttendedAddress22%>
                    </td>
             </tr>
            
            <tr>
                <td>City</td>
                <td>
                    <%=Model.SchoolAttendedCity2%></td>
             </tr>
            <tr>
                <td>State</td>
                <td><%=Model.SchoolAttendedState2%></td>
            </tr>
            
            <tr>
                <td class="nobdr" colspan="2">&nbsp;</td>
            </tr>
            
            <tr>
                <td>School Name</td>
                <td>
                    <%=Model.SchoolName3%></td>
                
            </tr>
            
            <tr>
                <td>Dates Attended</td>
                <td>From:&nbsp
                    <%=Model.DateFrom3%>&nbsp
                    To:&nbsp
                    <%=Model.DateTo3%></td>
            </tr>
            <tr>
                <td>Address Line 1</td>
                <td><%=Model.SchoolAttendedAddress13%></td>
            </tr>
            <tr>
                <td>Address Line 2</td>
                <td>
                    <%=Model.SchoolAttendedAddress23%>
                    </td>
             </tr>
             
            <tr>
                <td>City</td>
                <td>
                    <%=Model.SchoolAttendedCity3%></td>
             </tr>
             <tr>
                <td>State</td>
                <td><%=Model.SchoolAttendedState3%></td>
            </tr>

            <tr>
                <td class="auto-style9" style="border-right: none; padding-top: 10px;" colspan="4">
                    <h4>Discharge Information</h4>
                </td>
            </tr>

            <tr>
                <td>Discharge Date</td>
                <td>
                    <%=Model.DischargeDate%></td>
                <td>Location After Discharge</td>
                <td>
                    <%=Model.LocationAfterDischarge%></td>
            </tr>
            <tr>
                <td>Melmark New England&#39;s Follow Up Responsibilities</td>
                <td  colspan="3">
                    <%=Model.MelmarkNewEnglandsFollowUpResponsibilities%></td>
            </tr>

            </table>

    </div>
</div>

<script>loadClientStaticDetails();</script>