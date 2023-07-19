<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ClientDB.Models.HomeModel>" %>

<style type="text/css">
    .maintable {
        width: 100%;
        border: none;
    }

    div.middleContainer table tr td {
        border-bottom: 1px solid #DDDDDD;
        border-right: 1px solid #DDDDDD;
        color: #525252;
        font-family: Arial,Helvetica,sans-serif;
        font-size: 11px;
        font-weight: bold;
        padding: 8px 1px;
        width: 25%;
    }

    .boldLabel {
        font-weight: bold;
        font-size: 18px !important;
    }

    .boldText {
        font-weight: bold;
        font-size: 14px;
    }

    table {
        width: 100%;
        float: none;
        border-top: 1px solid #dddddd;
    }

    .auto-style1 {
        font-size: 11px;
        height: auto;
    }

    .auto-style2 {
        height: 25px;
    }

    .auto-style3 {
        font-weight: bold;
        font-size: 14px;
        height: 25px;
    }
</style>
<div>
    <%if (ViewBag.curstatus != 0)
      {%>
    <table class="maintable" align="center" cellpadding="0" cellspacing="0" border="0">
        <tr>
            <td style="border: none;" class="boldLabel">Client Profile / Protocol Summary</td>
            <td style="border: none;">
                <table cellpadding="0" cellspacing="0" border="0">
                    <tr>
                        <td class="boldText" style="background-color: #F2EEEE; width: 25%;">Date Updated:</td>
                        <%if (Model.parentInfoPA != null)
                          {
                              if (Model.parentInfoPA.AdmissionDate != null)
                              {
                                  DateTime date = (DateTime)Model.parentInfoPA.AdmissionDate;
                                  string result = date.ToString("MM/dd/yyyy").Replace('-', '/');%>
                        <td style="width: 75%;"><%=result%></td>
                        <%}
                              else
                              { %>
                        <td></td>
                        <%}
                          } %>
                    </tr>
                </table>
                <br />
            </td>
        </tr>
        <tr>
            <td style="border: none; width: 50%;">
                <table cellpadding="0" cellspacing="0" border="0">
                    <tr>
                        <td class="boldText" style="border-top: 1px; width: 25%;">Name:
                            <br />
                            <br />
                            <br />
                        </td>
                        <td style="border-top: 1px; width: 75%;"><%if (Model.parentInfoPA.LastName != null)
                                                                   { %>
                            <%=Model.parentInfoPA.LastName %>, <%=Model.parentInfoPA.FirstName %><%} %>
                            <br />
                            <br />
                            <br />
                        </td>
                    </tr>
                </table>
            </td>
            <td style="border: none;">
                <table style="float: left; width: 100%;" cellpadding="0" cellspacing="0" border="0">
                    <tr>
                        <td class="boldText" style="border-top: 1px; width: 25%;">Gender:</td>
                        <%if (Model.parentInfoPA.Gender == "1")
                          { %>
                        <td style="border-top: 1px; width: 75%;">Male</td>
                        <%}
                          else if (Model.parentInfoPA.Gender == "2")
                          {%>
                        <td style="border-top: 1px; width: 75%;">Female</td>
                        <%} %>
                    </tr>
                    <tr>
                        <td class="boldText" style="width: 25%;">Birthdate:</td>
                        <td style="width: 75%;"><%if (Model.parentInfoPA.DOB != null)
                                                  {
                                                      DateTime date = (DateTime)Model.parentInfoPA.DOB;
                                                      string result = date.ToString("MM/dd/yyyy").Replace('-', '/');
                        %><%=result %><%} %></td>
                    </tr>
                </table>

            </td>
        </tr>
        <tr>
            <td style="border: none;" colspan="2">
                <br />
                <table cellpadding="0" cellspacing="0" border="0">
                    <tr>
                        <td colspan="2" class="boldLabel" style="background-color: #F2EEEE;">Contact Information</td>
                    </tr>
                </table>
                <br />
            </td>
        </tr>
        <tr>
            <td style="border: none; width: 50%;">
                <table cellpadding="0" cellspacing="0" border="0">
                    <tr>
                        <td class="boldLabel">Resident / Individual</td>
                        <td></td>
                    </tr>
                    <tr>
                        <td rowspan="2" class="boldText" style="width: 35%;">Current Address:</td>
                        <td style="width: 65%;"><%=Model.parentInfoPA.Address2 %></td>
                    </tr>
                    <tr>
                        <td style="width: 65%;"><%=Model.parentInfoPA.Address1 %></td>
                    </tr>
                    <tr style="display: none">
                        <td style="width: 65%;"><%=Model.parentInfoPA.Address3 %></td>
                    </tr>


                    <tr>
                        <td style="width: 35%;">City:</td>
                        <td style="width: 65%;"><%=Model.parentInfoPA.City %></td>
                    </tr>
                    <tr>
                        <td style="width: 35%;">State:</td>
                        <td style="width: 65%;"><%if (Model.parentInfoPA.State != null && Model.parentInfoPA.State != 0)
                                                  { %><%=ViewBag.State %><%} %></td>
                    </tr>
                    <tr>
                        <td style="width: 35%;">Zip:</td>
                        <td style="width: 65%;"><%=Model.parentInfoPA.Zip %></td>
                    </tr>
                    <tr>
                        <td style="width: 35%;">Country:</td>
                        <td style="width: 65%;"><%if (Model.parentInfoPA.Country != null)
                                                  { %> <%=ViewBag.Country %>
                            <%} %>
                        </td>
                    </tr>
                    <tr>
                        <td class="boldText" style="width: 35%;">Phone Number:</td>
                        <td style="width: 65%;"><%if (Model.parentInfoPA.PrimaryCntct != null)
                                                  { %><%=Model.parentInfoPA.PrimaryCntct.HomePhone %><%} %></td>
                    </tr>
                    <tr>
                        <td class="boldText" style="width: 35%;">Funding Source
                        </td>
                        <td style="width: 65%;"><%=Model.parentInfoPA.FundingSource %></td>
                    </tr>
                </table>
                <br />
            </td>
            <td style="border: none; vertical-align: top; width: 50%">
                <table cellpadding="0" cellspacing="0" border="0">

                    <tr>
                        <td class="boldLabel">Primary Contact</td>
                         <%if (Model.parentInfoPA.PrimaryCntct != null)
                      { %>
                        <td><%if (Model.parentInfoPA.PrimaryCntct.LName != null || Model.parentInfoPA.PrimaryCntct.FName != null)
                              { %><%=Model.parentInfoPA.PrimaryCntct.LName%>,<%=Model.parentInfoPA.PrimaryCntct.FName%><%} %></td>
                    </tr>
                    <%}else{ %>
                    <td></td>
                    </tr>
                    <%} %>
                    <%if (Model.parentInfoPA.PrimaryCntct != null)
                      { %>
                    <tr>
                        <td rowspan="3" class="boldText" style="width: 35%;">
                            <br />
                            Address:
                            <br />
                            <br />
                        </td>
                        <td style="width: 65%;"><%=Model.parentInfoPA.PrimaryCntct.Address2 %></td>

                    </tr>
                    <tr>
                        <td style="width: 65%;"><%=Model.parentInfoPA.PrimaryCntct.Address1 %></td>
                    </tr>
                    <tr>
                        <td style="width: 65%;"><%=Model.parentInfoPA.PrimaryCntct.Address3 %></td>
                    </tr>
                    <%-- <tr>
                        <td style=" width:35%;">State:</td>
                        <td style=" width:65%;"><%if(Model.parentInfoPA.PrimaryCntct.State!=null){ %>
                              <%=ViewBag.PrcontactState%>
                            <%  } %></td>
                    </tr>--%>
                    <tr>
                        <td style="width: 35%;">Primary Contact #:</td>
                        <td style="width: 65%;"><%=Model.parentInfoPA.PrimaryCntct.HomePhone %></td>
                    </tr>
                    <tr>
                        <td style="width: 35%;">Mobile #:</td>
                        <td style="width: 65%;"><%=Model.parentInfoPA.PrimaryCntct.CellPhone %></td>
                    </tr>
                    <tr>
                        <td style="width: 35%;">Other#:</td>
                        <td style="width: 65%;"><%=Model.parentInfoPA.PrimaryCntct.WorkPhone %></td>
                    </tr>

                    <tr>
                        <td style="width: 35%;">E-Mail Address:</td>
                        <td style="width: 65%;"><%=Model.parentInfoPA.PrimaryCntct.Email %></td>
                    </tr>
                    <%} %>
                </table>
                <br />
            </td>
        </tr>
        <tr>
            <td style="border: none;">
                <table cellpadding="0" cellspacing="0" border="0">
                    <tr>
                        <td class="boldLabel" style="width: 35%;">Legal Guardian</td>
                         <%if (Model.parentInfoPA.LegalGrdn1 != null)
                      { %>
                        <td style="width: 65%;"><%if (Model.parentInfoPA.LegalGrdn1.LName != null || Model.parentInfoPA.LegalGrdn1.FName != null)
                                                  { %><%=Model.parentInfoPA.LegalGrdn1.LName%>,<%=Model.parentInfoPA.LegalGrdn1.FName%><%} %></td>
                    </tr>
                    <%}else{ %>
                    <td style="width: 65%;"></td></tr>
                    <%} %>
                    <%if (Model.parentInfoPA.LegalGrdn1 != null)
                      { %>
                    <tr>
                        <td rowspan="3" class="boldText" style="width: 35%;">Address:</td>
                        <td style="width: 65%;"><%=Model.parentInfoPA.LegalGrdn1.Address2 %></td>
                    </tr>
                    <tr>
                        <td style="width: 65%;"><%=Model.parentInfoPA.LegalGrdn1.Address1 %></td>
                    </tr>
                    <tr>
                        <td style="width: 65%;"><%=Model.parentInfoPA.LegalGrdn1.Address3 %></td>
                    </tr>
                    <tr>
                        <td class="boldText" style="width: 35%;">Primary Contact #:</td>
                        <td style="width: 65%;"><%=Model.parentInfoPA.LegalGrdn1.HomePhone %></td>
                    </tr>

                    <tr>
                        <td class="boldText" style="width: 35%;">Mobile #:</td>
                        <td style="width: 65%;"><%=Model.parentInfoPA.LegalGrdn1.CellPhone %></td>
                    </tr>
                    <tr>
                        <td class="boldText" style="width: 35%;">Other #:</td>
                        <td style="width: 65%;"><%=Model.parentInfoPA.LegalGrdn1.WorkPhone %></td>
                    </tr>
                    <tr>
                        <td class="boldText" style="width: 35%;">E-Mail Address:</td>
                        <td style="width: 65%;"><%=Model.parentInfoPA.LegalGrdn1.Email %></td>
                    </tr>
                    <%} %>
                </table>
                <br />
            </td>
            <td style="border: none;">
                <table cellpadding="0" cellspacing="0" border="0">
                    <tr>
                        <td class="boldLabel" style="width: 35%;">Legal Guardian</td>
                         <%if (Model.parentInfoPA.LegalGrdn2 != null)
                      { %>
                        <td style="width: 65%;"><%if (Model.parentInfoPA.LegalGrdn2.LName != null || Model.parentInfoPA.LegalGrdn2.FName != null)
                                                  { %><%=Model.parentInfoPA.LegalGrdn2.LName %>,<%=Model.parentInfoPA.LegalGrdn2.FName%><%} %></td>

                    </tr>
                     <%}else{ %>
                    <td style="width: 65%;"></td></tr>
                    <%} %>
                    <%if (Model.parentInfoPA.LegalGrdn2 != null)
                      { %>
                    <tr>
                        <td rowspan="3" class="boldText" style="width: 35%;">Address:</td>
                        <td style="width: 65%;"><%=Model.parentInfoPA.LegalGrdn2.Address2 %></td>
                    </tr>
                    <tr>
                        <td style="width: 65%;"><%=Model.parentInfoPA.LegalGrdn2.Address1 %></td>
                    </tr>
                    <tr>
                        <td style="width: 65%;"><%=Model.parentInfoPA.LegalGrdn2.Address3 %></td>
                    </tr>
                    <tr>
                        <td class="boldText" style="width: 35%;">Primary Contact#:</td>
                        <td style="width: 65%;"><%=Model.parentInfoPA.LegalGrdn2.HomePhone %></td>
                    </tr>

                    <tr>
                        <td class="boldText" style="width: 35%;">Mobile #:</td>
                        <td style="width: 65%;"><%=Model.parentInfoPA.LegalGrdn2.CellPhone %></td>
                    </tr>
                    <tr>
                        <td class="boldText">Other #:</td>
                        <td style="width: 65%;"><%=Model.parentInfoPA.LegalGrdn2.WorkPhone %></td>
                    </tr>
                    <tr>
                        <td class="boldText" style="width: 35%;">E-Mail Address:</td>
                        <td style="width: 65%;"><%=Model.parentInfoPA.LegalGrdn2.Email %></td>
                    </tr>
                    <%} %>
                </table>
                <br />
            </td>
        </tr>
        <tr>
            <td style="border: none;">
                <table cellpadding="0" cellspacing="0" border="0">
                    <tr>
                        <td class="boldLabel" style="width: 35%;">Support Coordinator</td>
                         <%if (Model.parentInfoPA.LegalGrdn1 != null)
                      { %>
                        <td style="width: 65%;"><%if (Model.parentInfoPA.SupportCord.LName != null || Model.parentInfoPA.SupportCord.FName != null)
                                                  { %><%=Model.parentInfoPA.SupportCord.LName%>,<%=Model.parentInfoPA.SupportCord.FName%><%} %></td>
                    </tr>
                     <%}else{ %>
                    <td style="width: 65%;"></td></tr>
                    <%} %>
                    <%if (Model.parentInfoPA.SupportCord != null)
                      { %>
                    <tr>
                        <td rowspan="3" class="boldText" style="width: 35%;">Address:</td>
                        <td style="width: 65%;"><%=Model.parentInfoPA.SupportCord.Address2 %></td>
                    </tr>
                    <tr>
                        <td style="width: 65%;"><%=Model.parentInfoPA.SupportCord.Address1 %></td>
                    </tr>
                    <tr>
                        <td style="width: 65%;"><%=Model.parentInfoPA.SupportCord.Address3 %></td>
                    </tr>
                    <tr>
                        <td class="boldText" style="width: 35%;">Primary Contact#:</td>
                        <td style="width: 65%;"><%=Model.parentInfoPA.SupportCord.HomePhone %></td>
                    </tr>
                    <tr>
                        <td class="boldText" style="width: 35%;">Mobile #:</td>
                        <td style="width: 65%;"><%=Model.parentInfoPA.SupportCord.CellPhone %></td>
                    </tr>
                    <tr>
                        <td class="boldText" style="width: 35%;">Other #:</td>
                        <td style="width: 65%;"><%=Model.parentInfoPA.SupportCord.WorkPhone %></td>
                    </tr>

                    <tr>
                        <td class="boldText" style="width: 35%;">E-Mail Address:</td>
                        <td style="width: 65%;"><%=Model.parentInfoPA.SupportCord.Email %></td>
                    </tr>
                    <%} %>
                </table>
                <br />
            </td>
            <td style="border: none;">
                <table cellpadding="0" cellspacing="0" border="0">
                    <tr>
                        <td class="boldLabel" style="width: 35%;">Advocate</td>
                         <%if (Model.parentInfoPA.LegalGrdn1 != null)
                      { %>
                        <%if (Model.parentInfoPA.Advocate.LName != null || Model.parentInfoPA.Advocate.FName != null)
                          { %>
                        <td style="width: 65%;"><%=Model.parentInfoPA.Advocate.LName %>,<%=Model.parentInfoPA.Advocate.FName %></td>
                        <%}
                          else
                          { %>
                        <td style="width: 65%;"></td>
                        <%} %>
                    </tr>
                     <%}else{ %>
                    <td style="width: 65%;"></td></tr>
                    <%} %>
                    <%if (Model.parentInfoPA.Advocate != null)
                      { %>
                    <tr>
                        <td rowspan="3" class="boldText" style="width: 35%;">Address:</td>
                        <td style="width: 65%;"><%=Model.parentInfoPA.Advocate.Address2 %></td>
                    </tr>
                    <tr>
                        <td style="width: 65%;"><%=Model.parentInfoPA.Advocate.Address1 %></td>
                    </tr>
                    <tr>
                        <td style="width: 65%;"><%=Model.parentInfoPA.Advocate.Address3 %></td>
                    </tr>
                    <tr>
                        <td class="boldText" style="width: 35%;">Primary Contact#:</td>
                        <td style="width: 65%;"><%=Model.parentInfoPA.Advocate.HomePhone %></td>
                    </tr>
                    <tr>
                        <td class="boldText" style="width: 35%;">Mobile #:</td>
                        <td style="width: 65%;"><%=Model.parentInfoPA.Advocate.CellPhone %></td>
                    </tr>
                    <tr>
                        <td class="boldText" style="width: 35%;">Other #:</td>
                        <td style="width: 65%;"><%=Model.parentInfoPA.Advocate.WorkPhone %></td>
                    </tr>

                    <tr>
                        <td class="boldText" style="width: 35%;">E-Mail Address:</td>
                        <td style="width: 65%;"><%=Model.parentInfoPA.Advocate.Email %></td>
                    </tr>
                    <%} %>
                </table>
                <br />
            </td>
        </tr>
        <tr>
            <td style="border: none;" colspan="2">
                <table cellpadding="0" cellspacing="0" border="0">
                    <tr>
                        <td colspan="4" class="boldLabel" style="background-color: #F2EEEE;">Diagnosis:</td>

                    </tr>
                    <%foreach (var item in Model.parentInfoPA.Diagnoses)
                      { %>
                    <tr>
                        <td colspan="4"><%=item.DiagnosesName %></td>
                    </tr>
                    <%} %>
                </table>
                <br />
            </td>
        </tr>
        <tr>
            <td style="border: none;" colspan="2">
                <table cellpadding="0" cellspacing="0" border="0">
                    <tr>
                        <td class="boldLabel" style="background-color: #F2EEEE;">Level Of Supervision</td>
                    </tr>
                </table>
                <br />
            </td>
        </tr>
        <tr>
            <td style="border: none;" colspan="2">
                <table cellpadding="0" cellspacing="0" border="0">
                    <tr>
                        <td colspan="2" class="boldLabel">General</td>
                    </tr>
                    <tr>
                        <td class="boldText" style="width: 25%;">Bathroom</td>
                        <td style="width: 75%;"><%=Model.parentInfoPA.BathRoom %></td>
                    </tr>
                    <tr>
                        <td class="boldText" style="width: 25%;">On Campus</td>
                        <td style="width: 75%;"><%=Model.parentInfoPA.OnCampus %></td>
                    </tr>
                    <tr>
                        <td class="boldText" style="width: 25%;">When Transporting</td>
                        <td style="width: 75%;"><%=Model.parentInfoPA.WhenTransporting %></td>
                    </tr>
                    <tr>
                        <td class="boldText" style="width: 25%;">Off Campus</td>
                        <td style="width: 75%;"><%=Model.parentInfoPA.OffCampus %></td>
                    </tr>
                    <tr>
                        <td class="boldText" style="width: 25%;">Pool / Swimming</td>
                        <td style="width: 75%;"><%=Model.parentInfoPA.Pool_Swimming %></td>
                    </tr>
                    <tr>
                        <td class="boldText" style="width: 25%;">Van</td>
                        <td style="width: 75%;"><%=Model.parentInfoPA.Van %></td>
                    </tr>
                </table>
                <br />
            </td>
        </tr>
        <tr>
            <td style="border: none;">
                <table cellpadding="0" cellspacing="0" border="0">
                    <tr>
                        <td colspan="2" class="boldLabel">Home / Residential</td>
                    </tr>
                    <tr>
                        <td class="boldText" style="width: 35%;">Common Areas</td>
                        <td style="width: 65%;"><%=Model.parentInfoPA.CommonAreas %></td>
                    </tr>
                    <tr>
                        <td class="boldText" style="width: 35%;">Bedroom Awake</td>
                        <td style="width: 65%;"><%=Model.parentInfoPA.BedroomAwake %></td>
                    </tr>
                    <tr>
                        <td class="boldText" style="width: 35%;">Bedroom Asleep</td>
                        <td style="width: 65%;"><%=Model.parentInfoPA.BedroomAsleep %></td>
                    </tr>

                </table>
                <br />
            </td>
            <td style="border: none;">
                <table cellpadding="0" cellspacing="0" border="0">
                    <tr>
                        <td colspan="2" class="boldLabel">Day Program</td>
                    </tr>
                    <tr>
                        <td class="boldText" style="width: 35%;">Task / Break</td>
                        <td style="width: 65%;"><%=Model.parentInfoPA.Task_Break %></td>
                    </tr>
                    <tr>
                        <td class="boldText" style="width: 35%;">Transitions Inside</td>
                        <td style="width: 65%;"><%=Model.parentInfoPA.TransitionsInside %></td>
                    </tr>
                    <tr>
                        <td class="boldText" style="width: 35%;">Transitions Uneven Ground</td>
                        <td style="width: 65%;"><%=Model.parentInfoPA.TransitionsUneven %></td>
                    </tr>

                </table>
                <br />
            </td>
        </tr>
    </table>
    <br />

    <br />
    <table class="maintable" align="center" cellpadding="0" cellspacing="0" border="0">

        <tr>
            <td colspan="2" style="border: none;">
                <br />
                <table style="width: 100%;" cellpadding="0" cellspacing="0" border="0">
                    <tr>
                        <td class="boldLabel" style="background-color: #F2EEEE;">Evacuation Skills <span style="font-size: 10px;">(How does this person evacuate during fire drills,what assistance do they need)</span></td>
                    </tr>
                    <tr>
                        <td>
                            <ul>
                                <li>Risk of Resistance: <%=Model.parentInfoPA.RiskofResistance %></li>
                                <li>Mobility: <%=Model.parentInfoPA.Mobility %></li>
                                <li>Need for extra help: <%=Model.parentInfoPA.NeedforExtraHelp %></li>
                                <li>Response to Instructions: <%=Model.parentInfoPA.ResponseToInstructions %></li>
                                <li>Consciousness: <%=Model.parentInfoPA.Consciousness %></li>
                                <li>Waking Response: <%=Model.parentInfoPA.WakingResponse %></li>
                            </ul>

                        </td>
                    </tr>

                </table>
                <br />
            </td>
        </tr>
        <tr>
            <td colspan="2" style="border: none;">
                <table style="width: 100%;" cellpadding="0" cellspacing="0" border="0">
                    <tr>
                        <td class="boldLabel" colspan="2" style="background-color: #F2EEEE;">Medical Information</td>
                    </tr>
                    <tr>
                        <td class="boldText" style="width: 25%;">Allergies</td>
                        <td style="width: 75%;"><%=Model.parentInfoPA.Allergies %></td>
                    </tr>
                    <tr>
                        <td class="boldText" style="width: 25%;">Seizures</td>
                        <td style="width: 75%;"><%=Model.parentInfoPA.Seizures %></td>
                    </tr>
                    <tr>
                        <td class="boldText" style="width: 25%;">Diet</td>
                        <td style="width: 75%;"><%=Model.parentInfoPA.Diet %></td>
                    </tr>
                    <tr>
                        <td class="boldText" style="width: 25%;">Other</td>
                        <td style="width: 75%;"><%=Model.parentInfoPA.Other %></td>
                    </tr>
                </table>
                <br />
            </td>
        </tr>
        <tr>
            <td colspan="2" style="border: none;">
                <table style="width: 100%;" cellpadding="0" cellspacing="0" border="0">
                    <tr>
                        <td class="boldLabel" style="background-color: #F2EEEE;">Lifting / Transfers<span style="font-size: 10px;"> (How does this person transfer, how are they lifted-If applicable)</span></td>
                    </tr>
                    <tr>
                        <td>
                            <ul class="auto-style1">
                                <%foreach (var item in Model.parentInfoPA.LiftingOrTransfers)
                                  { %>
                                <li><%=item.BehaviorName %></li>
                                <%} %>
                            </ul>
                        </td>
                    </tr>
                </table>
                <br />
            </td>
        </tr>
        <tr>
            <td colspan="2" style="border: none;">
                <table style="width: 100%;" cellpadding="0" cellspacing="0" border="0">
                    <tr>
                        <td class="boldLabel" style="background-color: #F2EEEE;">Ambulation<span style="font-size: 10px;"> (How does this person get from place to place)</span></td>
                    </tr>
                    <tr>
                        <td>
                            <ul class="auto-style1">
                                <%foreach (var item in Model.parentInfoPA.Ambulation)
                                  { %>
                                <li><%=item.BehaviorName %></li>
                                <%} %>
                            </ul>
                        </td>
                    </tr>
                </table>
                <br />
            </td>
        </tr>
        <tr>
            <td colspan="2" style="border: none;">
                <table style="width: 100%;" cellpadding="0" cellspacing="0" border="0">
                    <tr>
                        <td class="boldLabel" style="background-color: #F2EEEE;">Toileting<span style="font-size: 10px;"> (General information-what are they able to do-what kind of support/assistance is needed)</span></td>
                    </tr>
                    <tr>
                        <td>
                            <ul class="auto-style1">
                                <%foreach (var item in Model.parentInfoPA.Toileting)
                                  { %>
                                <li><%=item.BehaviorName %></li>
                                <%} %>
                            </ul>
                        </td>
                    </tr>
                </table>
                <br />
            </td>
        </tr>
        <tr>
            <td colspan="2" style="border: none;">
                <table style="width: 100%;" cellpadding="0" cellspacing="0" border="0">
                    <tr>
                        <td class="boldLabel" style="background-color: #F2EEEE;">Eating<span style="font-size: 10px;"> (General information-what are they able to do-what kind of support/assistance is needed)</span></td>
                    </tr>
                    <tr>
                        <td>
                            <ul class="auto-style1">
                                <%foreach (var item in Model.parentInfoPA.Eating)
                                  { %>
                                <li><%=item.BehaviorName %></li>
                                <%} %>
                            </ul>
                        </td>
                    </tr>
                </table>
                <br />
            </td>
        </tr>
        <tr>
            <td colspan="2" style="border: none;">
                <table style="width: 100%;" cellpadding="0" cellspacing="0" border="0">
                    <tr>
                        <td class="boldLabel" style="background-color: #F2EEEE;">Showering<span style="font-size: 10px;"> (General information-what are they able to do-what kind of support/assistance is needed)</span></td>
                    </tr>
                    <tr>
                        <td>
                            <ul class="auto-style1">
                                <%foreach (var item in Model.parentInfoPA.Showering)
                                  { %>
                                <li><%=item.BehaviorName %></li>
                                <%} %>
                            </ul>
                        </td>
                    </tr>
                </table>
                <br />
            </td>
        </tr>
        <tr>
            <td colspan="2" style="border: none;">
                <table style="width: 100%;" cellpadding="0" cellspacing="0" border="0">
                    <tr>
                        <td class="boldLabel" style="background-color: #F2EEEE;">Toothbrushing<span style="font-size: 10px;"> (General information-what are they able to do-what kind of support/assistance is needed)</span></td>
                    </tr>
                    <tr>
                        <td>
                            <ul class="auto-style1">
                                <%foreach (var item in Model.parentInfoPA.ToothBrushing)
                                  { %>
                                <li><%=item.BehaviorName %></li>
                                <%} %>
                            </ul>
                        </td>
                    </tr>
                </table>
                <br />
            </td>
        </tr>
        <tr>
            <td colspan="2" style="border: none;">
                <table style="width: 100%;" cellpadding="0" cellspacing="0" border="0">
                    <tr>
                        <td class="boldLabel" style="background-color: #F2EEEE;">Dressing<span style="font-size: 10px;"> (General information-what are they able to do-what kind of support/assistance is needed)</span></td>
                    </tr>
                    <tr>
                        <td>
                            <ul class="auto-style1">
                                <%foreach (var item in Model.parentInfoPA.Dressing)
                                  { %>
                                <li><%=item.BehaviorName %></li>
                                <%} %>
                            </ul>
                        </td>
                    </tr>
                </table>
                <br />
            </td>
        </tr>
        <tr>
            <td colspan="2" style="border: none;">
                <table style="width: 100%;" cellpadding="0" cellspacing="0" border="0">
                    <tr>
                        <td class="boldLabel" style="background-color: #F2EEEE;">Skin care / Skin integrity<span style="font-size: 10px;"> (General information-what are they able to do-what kind of support/assistance is needed)</span></td>
                    </tr>
                    <tr>
                        <td>
                            <ul class="auto-style1">
                                <%foreach (var item in Model.parentInfoPA.SkinCareOrSkinIntegrity)
                                  { %>
                                <li><%=item.BehaviorName %></li>
                                <%} %>
                            </ul>
                        </td>
                    </tr>
                </table>
                <br />
            </td>
        </tr>
        <tr>
            <td colspan="2" style="border: none;">
                <table style="width: 100%;" cellpadding="0" cellspacing="0" border="0">
                    <tr>
                        <td class="boldLabel" style="background-color: #F2EEEE;">Communication<span style="font-size: 10px;"> (How does this person communicate, words used, words to avoid, etc)</span></td>
                    </tr>
                    <tr>
                        <td>
                            <ul class="auto-style1">
                                <%foreach (var item in Model.parentInfoPA.Communication)
                                  { %>
                                <li><%=item.BehaviorName %></li>
                                <%} %>
                            </ul>
                        </td>
                    </tr>
                </table>
                <br />
            </td>
        </tr>
    </table>
    <br />

    <br />
    <table class="maintable" align="center" cellpadding="0" cellspacing="0" border="0">

        <tr>
            <td colspan="2" style="border: none;">
                <br />
                <table style="width: 100%;" cellpadding="0" cellspacing="0" border="0">
                    <tr>
                        <td colspan="4" class="boldLabel" style="background-color: #F2EEEE;">Adaptive Equipment / Health related protections</td>
                    </tr>
                    <tr>
                        <td style="text-align: center;" class="boldText">Item</td>
                        <td style="text-align: center;" class="boldText">Schedule for use</td>
                        <td style="text-align: center;" class="boldText">Storage Location</td>
                        <td style="text-align: center;" class="boldText">Cleaning Instructions</td>
                    </tr>
                    <%foreach (var item in Model.parentInfoPA.AdaptiveEquip)
                      { %>
                    <tr>
                        <td><%=item.Item %></td>
                        <td><%=item.ScheduleForUse %></td>
                        <td><%=item.StorageLocation %></td>
                        <td><%=item.CleaningInstructions %></td>
                    </tr>
                    <%} %>
                </table>
                <br />
            </td>
        </tr>
        <tr>
            <td colspan="2" style="border: none;">
                <table style="width: 100%;" cellpadding="0" cellspacing="0" border="0">
                    <tr>
                        <td colspan="3" class="boldLabel" style="background-color: #F2EEEE;">Basic Behavioral Information</td>
                    </tr>
                    <tr>
                        <td style="text-align: center;" class="boldText">Target Behavior
                                <br />
                            <span style="font-size: 10px;">(For Acceleration & Deceleration)</span></td>
                        <td style="text-align: center;" class="boldText">Definition</td>
                        <td style="text-align: center;" class="boldText">Response / Strategy</td>
                    </tr>
                    <%foreach (var item in Model.parentInfoPA.BasicBehaviouralInfo)
                      { %>
                    <tr>
                        <td rowspan="3"><%=item.TargetBehaviour %></td>
                        <td rowspan="3"><%=item.Definition %></td>
                        <td class="boldText" style="font-size: 12px;">Antecedent: <%=item.Antecedent %></td>
                    </tr>
                    <tr>
                        <td class="boldText" style="font-size: 12px;">FCT: <%=item.FCT %></td>
                    </tr>
                    <tr>
                        <td class="boldText" style="font-size: 12px;">Consequence: <%=item.Consequence %></td>
                    </tr>
                    <%} %>
                </table>
                <br />
            </td>
        </tr>
        <tr>
            <td colspan="2" style="border: none;">
                <table style="width: 100%;" cellpadding="0" cellspacing="0" border="0">
                    <tr>
                        <td colspan="3" class="boldLabel" style="background-color: #F2EEEE;">Preferred Activities</td>
                    </tr>
                    <tr>
                        <td>
                            <ul class="auto-style1">
                                <%foreach (var item in Model.parentInfoPA.PreferredActivities)
                                  { %>
                                <li><%=item.BehaviorName %></li>
                                <%} %>
                            </ul>
                        </td>
                    </tr>
                </table>
                <br />
            </td>
        </tr>
        <tr>
            <td colspan="2" style="border: none;">
                <table style="width: 100%;" cellpadding="0" cellspacing="0" border="0">
                    <tr>
                        <td class="boldLabel" style="background-color: #F2EEEE;">General Information<span style="font-size: 10px;"> (Important to know, what gets this person upset, what makes them happy, things to watch out for,

any important structure needed in their day, routines, preferred items)</span></td>
                    </tr>
                    <tr>
                        <td>
                            <ul class="auto-style1">
                                <%foreach (var item in Model.parentInfoPA.GeneralInformation)
                                  { %>
                                <li><%=item.BehaviorName %></li>
                                <%} %>
                            </ul>
                        </td>
                    </tr>
                </table>
                <br />
            </td>
        </tr>
        <tr>
            <td colspan="2" style="border: none;">
                <table style="width: 100%;" cellpadding="0" cellspacing="0" border="0">
                    <tr>
                        <td class="boldLabel" style="background-color: #F2EEEE;">Suggested Proactive Environmental Procedures<span style="font-size: 10px;"> (What can be done in the environment to support this

person, How should the staff respond if this person is upset, etc)</span></td>
                    </tr>
                    <tr>
                        <td>
                            <ul class="auto-style1">
                                <%foreach (var item in Model.parentInfoPA.SuggestedProactiveEnvironmentalProcedures)
                                  { %>
                                <li><%=item.BehaviorName %></li>
                                <%} %>
                            </ul>
                        </td>
                    </tr>
                </table>
                <br />
            </td>
        </tr>
    </table>
    <br />

    <br />

    <%}
      else
      {%>
    <div id="divresponse" style="overflow-y: auto; overflow-x: hidden; display: block; top: 49%; z-index: 1000; width: 50%; margin-left: 30%; margin-top: 20%;">

        <br />
       
        <div id="Div2">
            <table style="width: 100%;border: 1px ridge;">
                <tr>
                    <td style="text-align: center;border: medium none;">
                        <h3 style="color:red">An Error Has Occured. Please Contact Administrator.</h3>
                    </td>
                </tr>

            </table>


        </div>

    </div>


    <%} %>
</div>

