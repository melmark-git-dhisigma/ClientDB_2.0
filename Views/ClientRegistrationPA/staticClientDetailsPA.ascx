<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ClientDB.Models.ClientRegistrationPAModel>" %>
<style type="text/css">
    .spnLabel {
        font-weight: bold;
        font-style: italic;
    }

    .spnField {
        font-size: 13px;
    }

    .tabHeight {
        height: 30px;
    }

    .datagrid table {
        border-collapse: collapse;
        text-align: left;
        width: 100%;
    }

    .datagrid {
        font: normal 12px/150% Arial, Helvetica, sans-serif;
        background: #fff;
        overflow: hidden;
        border: 3px solid #006699;
        -webkit-border-radius: 3px;
        -moz-border-radius: 3px;
        border-radius: 3px;
    }

        .datagrid table td, .datagrid table th {
            padding: 3px 10px;
        }

        .datagrid table tbody td {
            color: #00557F;
            border-left: 1px solid #00557F;
            font-size: 12px;
            border-bottom: 1px solid #00557F;
            font-weight: normal;
        }

        .datagrid table tbody .alt td {
            background: #E1EEf4;
            color: #00557F;
        }

        .datagrid table tbody td:first-child {
            border-left: none;
        }

        .datagrid table tbody tr:last-child td {
            border-bottom: none;
        }


        .white_content {
  display: none;
  position: absolute;
  top: 25%;
  left: 25%;
  width: 300px;
  height: 110px;
  padding: 16px;
  border: 1px solid black;
  border-radius: 10px;
  -webkit-border-radius: 10px;
        -moz-border-radius: 10px;
  background-color: white;
  z-index: 1002;
  overflow: auto;
}

</style>

<script type="text/javascript">
function DischargeClient() {
    document.getElementById('popup').style.display = 'none';
                $.ajax( 
                  {
                      type: "POST",
                      url: "../ClientRegistrationPA/DischargeStudent",
                      data: "",
                      //data: "{'stdId':'" + stdId + "'}",
                      contentType: "application/json; charset=utf-8",
                      dataType: "json",
                      //async: false,
                      success: function (response) {
                          loadClientStaticDetails();
                      }
                  });
                $('#btnGeneral').trigger("click");
}
function ActivateClient() {
    document.getElementById('popup2').style.display = 'none';
    $.ajax(
      {
          type: "POST",
          url: "../ClientRegistrationPA/ActivateStudent",
          data: "",
          //data: "{'stdId':'" + stdId + "'}",
          contentType: "application/json; charset=utf-8",
          dataType: "json",
          //async: false,
          success: function (data) {
              loadClientStaticDetails();
          }
      });
    $('#btnGeneral').trigger("click");
}
</script>


<div class="datagrid">
    <table>
        <tbody>
            <tr>
                <td><span class="spnLabel">Client ID:</span><br />
                    <%--<span class="spnField"><%= Model.Id %></span>--%>
                    <span class="spnField"><%= Model.StudentId %></span>
                </td>
                <td><span class="spnLabel">Admission Date:</span><br />
                    <span class="spnField"><%=Model.AdmissinDate %></span></td>
                <td><span class="spnLabel">Birth Date:</span><br />
                    <span class="spnField"><%=Model.DateOfBirth %></span></td>
                <td><span class="spnLabel">Nick Name:</span><br />
                    <span class="spnField"><%=Model.NickName %></span></td>
            </tr>
            <tr>
                <td><span class="spnLabel">Age:</span> <%
                                                           DateTime Cday = DateTime.Now;
                                                           DateTime Bday = DateTime.ParseExact(Model.DateOfBirth, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                                           int Years, Months, Days;


                                                           if ((Cday.Year - Bday.Year) > 0 ||
                                               (((Cday.Year - Bday.Year) == 0) && ((Bday.Month < Cday.Month) ||
                                                 ((Bday.Month == Cday.Month) && (Bday.Day <= Cday.Day)))))
                                                           {
                                                               int DaysInBdayMonth = DateTime.DaysInMonth(Bday.Year, Bday.Month);
                                                               int DaysRemain = Cday.Day + (DaysInBdayMonth - Bday.Day);

                                                               if (Cday.Month > Bday.Month)
                                                               {
                                                                   Years = Cday.Year - Bday.Year;
                                                                   Months = Cday.Month - (Bday.Month + 1) + Math.Abs(DaysRemain / DaysInBdayMonth);
                                                                   Days = (DaysRemain % DaysInBdayMonth + DaysInBdayMonth) % DaysInBdayMonth;
                                                               }
                                                               else if (Cday.Month == Bday.Month)
                                                               {
                                                                   if (Cday.Day >= Bday.Day)
                                                                   {
                                                                       Years = Cday.Year - Bday.Year;
                                                                       Months = 0;
                                                                       Days = Cday.Day - Bday.Day;
                                                                   }
                                                                   else
                                                                   {
                                                                       Years = (Cday.Year - 1) - Bday.Year;
                                                                       Months = 11;
                                                                       Days = DateTime.DaysInMonth(Bday.Year, Bday.Month) - (Bday.Day - Cday.Day);
                                                                   }
                                                               }
                                                               else
                                                               {
                                                                   Years = (Cday.Year - 1) - Bday.Year;
                                                                   Months = Cday.Month + (11 - Bday.Month) + Math.Abs(DaysRemain / DaysInBdayMonth);
                                                                   Days = (DaysRemain % DaysInBdayMonth + DaysInBdayMonth) % DaysInBdayMonth;
                                                               }
                                                           }
                                                           else
                                                           {
                                                               throw new ArgumentException("Birthday date must be earlier than current date");
                                                           }
                    
                %>
                    <br />
                    <span class="spnField"><%=Years.ToString() %> Years &nbsp; <%= Months.ToString() %> Months</span></td>
                <td><span class="spnLabel">Race:</span><br />
                    <span class="spnField"><%= Model.StrRace%></span></td>
                <td><span class="spnLabel">Gender:</span><br />
                    <span class="spnField"><%= (Model.Gender!=null&&Model.Gender!=""&&Model.Gender!="0")? Model.GenderList.Where(x=>x.Value==Model.Gender).First().Text:"" %></span></td>
                <td><span class="spnLabel">Ambulatory:</span><br />
                    <span class="spnField"><%= (Model.Ambulatory == true)?"Yes":"No" %></span></td>
            </tr>
            <tr>
                <td colspan="4"><span class="spnLabel">Funder:</span><br />
                    <span class="spnField"><%=Model.FunderListString %></span></td>

            </tr>
            <tr>
                <td><span class="spnLabel">Communication:</span><br />
                    <span class="spnField"><%=Model.Communication1 %></span></td>
                <td><span class="spnLabel">Guardian(Self):</span><br />
                    <span class="spnField"><%= (Model.IsGuardian == true)?"Yes":"No" %></span></td>
                <td><span class="spnLabel">Intensive Staffing:</span><br />
                    <span class="spnField"><%= Model.Intensive %></span></td>
                <td><span class="spnLabel"></span>
                    <br />
                    <span class="spnField"></span></td>
            </tr>
            <tr>
                <td colspan="4"><span class="spnLabel">Classification:</span><br />
                    <%
                        string classificationString = "";
                        classificationString += Model.ClassificationName1;
                        classificationString += (Model.ClassificationName2 != "" && Model.ClassificationName2 != null) ? ";" + Model.ClassificationName2 : "";
                        classificationString += (Model.ClassificationName3 != "" && Model.ClassificationName3 != null) ? ";" + Model.ClassificationName3 : "";
                        classificationString += (Model.ClassificationName4 != "" && Model.ClassificationName4 != null) ? ";" + Model.ClassificationName4 : "";
                        classificationString += (Model.ClassificationName5 != "" && Model.ClassificationName5 != null) ? ";" + Model.ClassificationName5 : "";
                        
                    %>
                    <span class="spnField"><%=classificationString %></span></td>
            </tr>
            <tr>
                <td colspan="2"><span class="spnLabel">Photo Release Permission:</span><br />
                    <span class="spnField"><%= (Model.PhotoReleasePermission == true)?"Yes":"No" %></span></td>
                <td><span class="spnLabel">BSU#/SASID#:</span><br />
                    <span class="spnField"><%=Model.SASID %></span></td>
                <%-- <td><span class="spnLabel">PA Medicaid#:</span><br /> --%>
                <td> <span class="spnLabel">Medicaid #:</%><br />   
                    <span class="spnField"><%=Model.Medicaid %></span></td>
            </tr>
            <tr>
                <td colspan="2"><span class="spnLabel">Photo Release Note:</span><br />
                    <span class="spnField"><%=Model.PhotoPermComment %></span></td>
                <td colspan="2"><span class="spnLabel">Client Notes:</span><br />
                    <span class="spnField"><%=Model.ClientInfoComments %></span></td>
            </tr>
            <tr>
                <td colspan="2"><span class="spnLabel">Allergies:</span><br />
                    <span class="spnField"><%=Model.Allergie %></span></td>
                <td colspan="2"><span class="spnLabel">Diagnosis:</span><br />
                    <%if (Model.Diagnosis.Count != 0)
                      { %>
                    <span class="spnField"><%=Model.Diagnosis[0].Name %></span>
                    <%}
                      else
                      { %>
                    <span class="spnField"></span>
                    <%} %>
                </td>
            </tr>
            <% string Color = "";
               string DischBtnStatus = "block";
               string ActivateBtnStatus = "none";
               if (Model.ClientStatus == "Active")
               {
                   Color = "green";
               }
               else if (Model.ClientStatus == "On-Hold")
               {
                   Color = "orange";
               }
               else if (Model.ClientStatus == "Inactive")
               {
                   Color = "red";
               }
               else if (Model.ClientStatus == "Discharge")
               {
                   Color = "red";
                   DischBtnStatus = "none";
                   ActivateBtnStatus = "block";
               }
            %>
            <tr style="display: none">
                <td colspan="4" id="tdstudentName" class="nobdr" style="display: none;"><%=Model.LastName %> <%=Model.LastNameSuffix %>, <%=Model.FirstName %> <span style="color: <%=Color %>"><%= "("+Model.ClientStatus+")" %></span></td>
            </tr>
        </tbody>
    </table>
                         <div id="disch" style="display:none">
    <input id="btnDischarge"  type="submit" value="Discharge Client"  onClick="document.getElementById('popup').style.display = 'block';" style="display:<%=DischBtnStatus %>;float:right;border:none;width:120px !important; height:43px;font-weight:bold;font-size:small;background-repeat:no-repeat;background-position:top left;margin-top:5px; margin-right: 5px; margin-bottom: 5px;background:red !important;"/>
    <input id="btnActivate" type="submit" value="Activate Client" onClick="document.getElementById('popup2').style.display = 'block';" style="display:<%=ActivateBtnStatus %>;float:right;border:none;width:120px !important; height:43px;font-weight:bold;font-size:small;background-repeat:no-repeat;background-position:top left;margin-top:5px; margin-right: 5px; margin-bottom: 5px;"/>
    </div>
</div>
<div id="popup"  class="white_content">
    <table>
                            <tr>
                                <td>
                                <p style="font-size:16px;color:black;"><b>Are you sure you want to discharge client?</b></p>
                    </td>
                </tr>
        
                            <tr>
                                <td>
                                    <input id="Nobutt" type="submit" value="No"  onClick="document.getElementById('popup').style.display = 'none';" style="margin-right:10px" />
                                    
                                    <input id="Yesbutt" type="submit" value="Yes" onClick="DischargeClient();" style="margin-right:90px"  />
                                </td>
                            </tr>
            </table>

</div>

<div id="popup2"  class="white_content">
    <table>
                            <tr>
                                <td>
                                <p style="font-size:16px;color:black;"><b>Are you sure you want to Active client?</b></p>
                    </td>
                </tr>
        
                            <tr>
                                <td>
                                    <input id="Submit1" type="submit" value="No"  onClick="document.getElementById('popup2').style.display = 'none';" style="margin-right:10px" />
                                    
                                    <input id="Submit2" type="submit" value="Yes" onClick="ActivateClient();" style="margin-right:90px"  />
                                </td>
                            </tr>
            </table>

</div>

