<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ClientDB.Models.ProgressReport>" %>

<%--<%@ Register TagPrefix="FTB" Namespace="FreeTextBoxControls" Assembly="FreeTextBox" %>--%>
<!DOCTYPE HTML>

<html>
<head id="Head1" runat="server">
    <title>Progress Report
    </title>

    <style type="text/css">
        .tabTable {
            width: 100%;
            border: 1px solid #CCCCCC;
            padding: 3px;
        }

        .ui-datepicker select.ui-datepicker-month, .ui-datepicker select.ui-datepicker-year {
            width: 49% !important;
        }

        .tabHeader {
            font-size: 11px;
        }

        .lblSpan {
            font-style: italic;
            font-weight: bold;
        }

        .tblClass, .tblClass td {
            border: 1px solid black;
        }
    </style>

    <%--<script src="../Documents/JS/jquery.validationEngine-en.js"></script>
    <script src="../Documents/JS/jquery.validationEngine.js"></script>--%>

    <script type="text/javascript">

        $(".datepicker").datepicker({
            changeMonth: true,
            changeYear: true,
            showAnim: "fadeIn",
            yearRange: 'c-30:c+30',

            fixFocusIE: false,

        }).attr('readonly', 'true');
        

        var options = {
            success: showResponse  // post-submit callback 
        };
        function showResponse(responseText, statusText, xhr, $form) {

            ///alert("Saved Successfully..!");
            $('#content').html(responseText);
        }
        $(document).ready(function () {
            $("#tabs").tabs();
            jQuery("#saveTab1").validationEngine();
            jQuery("#saveTab2").validationEngine();
            $('#saveTab1,#saveTab2').ajaxForm(options);
            
        });

        $('#btnBack').on("click", function (event) {
            $('#content').load('../ProgressReport/Index');
        });
        function load(type) {
            if (type == "Month") {
                window.location.href = "../ProgressReport/ExportAllProgress?Id= " + <%=Model.RTFMId%> +"&type=" + type;
            }
            if(type=="Ouarter")
            {
                window.location.href = "../ProgressReport/ExportAllProgress?Id= " + <%=Model.RTFQId%> +"&type=" + type;
            }

        }

        function checkme1() {
            if ($('#profilePicture1').val() != "" && $('#tbdoc1').val() != "") {
                return true
            }
            else {
                alert("Please fill mandatory fields");
                return false;
            }
        }

        function checkme2() {
            if ($('#profilePicture2').val() != "" && $('#tbdoc2').val() != "") {
                return true
            }
            else {
                alert("Please fill mandatory fields");
                return false;
            }
        }

        function loadFunction(editid, type) {
            if (type == "DeleteTab1") {
                if (confirm('Are you sure you want to delete this report?')) {
                    $('#content').load('../ProgressReport/DeleteTab1Doc/' + editid);
                } else {
                    return false;
                }
            }
            if (type == "DeleteTab2") {
                if (confirm('Are you sure you want to delete this report?')) {
                    $('#content').load('../ProgressReport/DeleteTab2Doc/' + editid);
                } else {
                    return false;
                }
            }
            if (type == "Download1") {
                //alert('d1' + editid)
                window.open('../ProgressReport/viewDoc1?id=' + editid + '   ', '_blank');
            }
            if (type == "Download2") {
                //alert('d2' + editid)
                window.open('../ProgressReport/viewDoc2?id=' + editid + '   ', '_blank');
            }
            if (type == "Back") {
                $('#content').load('../ProgressReport/Index/');
            }

        }


        
        jQuery("#saveRTFQuarterly").validationEngine();
        jQuery("#saveRTFMonthly").validationEngine();

    </script>

</head>
<body>

    <div id="tabs" class="tabHeader">
        <ul class="topTabs">
            <li><a id="Utabs-1" href="#tabs-1">Residential and Educational Progress Report</a></li>
            <li><a id="Utabs-2" href="#tabs-2">Children’s Residential Quarterly Progress Report</a></li>
            <li><a id="Utabs-3" href="#tabs-3">RTF Quarterly Progress Report</a></li>
            <li><a id="Utabs-4" href="#tabs-4">RTF Monthly Progress Report</a></li>
            <li><a id="Utabs-5" href="#tabs-5">Children's Residential Monthly Progress Report</a></li>
        </ul>

        <div id="tabs-1">
            <%using (@Html.BeginForm("SaveDocTab1", "ProgressReport", FormMethod.Post, new { id = "saveTab1", enctype = "multipart/form-data" }))
              { %>
            <%--<%using (@Ajax.BeginForm("SaveDocTab1", "ProgressReport", FormMethod.Post, new AjaxOptions { UpdateTargetId = "content" }, new { id = "saveTab1", enctype = "multipart/form-data" }))
              { %>--%>
            <div class="tabContainer">
                <table class="tabTable">

                    <%if (Model.Tab1Doc_List.Count == 0)
                      { %>
                    <tr>
                        <td colspan="3">
                            <%=Html.HiddenFor(m=>m.ProReportId,Model.ProReportId) %>
                            <%=Html.HiddenFor(x=>x.reportType,1) %>
                            <label class="lblSpan">Document Name</label><span class="span-align">*</span><br />
                            <%=Html.TextBoxFor(m => m.DocumentName, Model.DocumentName, new { @class = "validate[required]", maxlength=50 })%></td>
                        <td colspan="3">

                            <%--<%=Html.DropDownListFor(m => m.DocumentType, Model.DocumentTypeList, new { @class = "validate[required] ", ID = "ddlDocumentType", onChange = "displayDiv()" })%></td>--%>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <label class="lblSpan">Select File</label><span class="span-align">*</span><br />
                            <input type="file" id="profilePicture1" name="profilePicture1" class="validate[required]" /></td>
                        <td colspan="2"></td>
                    </tr>
                    <%}
                      else
                      { %>
                    <tr>
                        <td>&nbsp;</td>
                    </tr>
                    <tr class="HeaderStyle">
                        <th class="tdLabel">Document Name</th>
                        <th class="tdLabel">Uploaded On</th>
                        <th class="tdLabel">Delete</th>
                        <th class="tdLabel">Download</th>
                    </tr>
                    <%foreach (var item in Model.Tab1Doc_List)
                      {
                    %>
                    <%
                    %>
                    <tr class="RowStyle">
                        <td><%= item.DocumentName %></td>
                        <td><%=Convert.ToDateTime(item.CreatedOn).ToString("MM/dd/yyyy") %></td>
                        <%if (Session["PlacementStat"].ToString() != "I"){%>
                        <td>
                            <img src="../../Images/delete.PNG" onclick="loadFunction(<%= item.DocumentId %>,'DeleteTab1');" style="cursor: pointer;" />
                        </td>
                        <%--<td><a href="../ProgressReport/viewDoc1?id=<%:item.DocumentId %>" target="_blank"><img src="../../Images/download.PNG" style="cursor:pointer;" /></a>--%>
                        <td><img src="../../Images/download.PNG" onclick="loadFunction(<%= item.DocumentId %>,'Download1');" style="cursor:pointer;" />
                            <%=Html.HiddenFor(m=>m.DocId,Model.DocId) %>
                           <% }%>
                    </td>
                    </tr>
                    <%}
                      } %>
                </table>

                <div class="btnDoneStyle">
                    <%if(Model.btnStatus.Equals(false))
                      { if (Session["PlacementStat"].ToString() != "I"){
                          %>
                    <input id="btnSaveTab1" type="submit" value="Save"/>
                    <%}
                      }
                      else
                      { %>
                    <%--<input id="btnUpdateTab1" type="submit" value="Update" />--%>
                    <%} %>
                    <input type="button" value="Back" onclick="loadFunction(0,'Back');" />
                </div>
            </div>
            <%} %>
        </div>
        <div id="tabs-2">
            <%using (@Html.BeginForm("SaveDocTab2", "ProgressReport", FormMethod.Post, new { id = "saveTab2", enctype = "multipart/form-data" }))
              { %>
        <%--<%using (@Ajax.BeginForm("SaveDocTab2", "ProgressReport", FormMethod.Post, new AjaxOptions { UpdateTargetId = "content" }, new { id = "saveTab2", enctype = "multipart/form-data" }))
            { %>--%>
            <div class="tabContainer">
                <table class="tabTable">

                    <%=Html.HiddenFor(m=>m.ProReportId,Model.ProReportId) %>
                    <%=Html.HiddenFor(x=>x.reportType,2) %>
                    
                    <%if (Model.Tab2Doc_List.Count==0)
                      { %>
                    <tr>
                        <td colspan="3">
                            <label class="lblSpan">Document Name</label><span class="span-align">*</span><br />
                            <%=Html.TextBoxFor(m => m.DocumentName, Model.DocumentName, new { @class = "validate[required]", maxlength=50 })%></td>
                        <td colspan="3">

                            <%--<%=Html.DropDownListFor(m => m.DocumentType, Model.DocumentTypeList, new { @class = "validate[required] ", ID = "ddlDocumentType", onChange = "displayDiv()" })%></td>--%>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <label class="lblSpan">Select File</label><span class="span-align">*</span><br />
                            <input type="file" id="profilePicture2" name="profilePicture2" class="validate[required]" /></td>
                        <td colspan="2"></td>
                    </tr>
                    <%}
                      else
                      { %>
                    <tr>
                        <td>&nbsp;</td>
                    </tr>

                    <tr class="HeaderStyle">
                        <th class="tdLabel">Document Name</th>
                        <th class="tdLabel">Uploaded On</th>
                        <th class="tdLabel">Delete</th>
                        <th class="tdLabel">Download</th>
                    </tr>
                    <%foreach (var item in Model.Tab2Doc_List)
                      {
                    %>
                    <%
                    %>
                    <tr class="RowStyle">
                        <td><%= item.DocumentName %></td>
                        <td><%=Convert.ToDateTime(item.CreatedOn).ToString("MM/dd/yyyy") %></td>
                        <%if (Session["PlacementStat"].ToString() != "I"){%>
                        <td>
                            <img src="../../Images/delete.PNG" onclick="loadFunction(<%= item.DocumentId %>,'DeleteTab2');" style="cursor: pointer;" />
                        </td>
                        <td><img src="../../Images/download.PNG" onclick="loadFunction(<%= item.DocumentId %>,'Download2');" style="cursor:pointer;" />
                            <%=Html.HiddenFor(m=>m.DocId,Model.DocId) %>
                            <%} %>
                    </td>
                    </tr>
                    <%}
                      } %>
                </table>

                <div class="btnDoneStyle">
                    <%if(Model.btnStatus.Equals(false))
                      { %>
                    <input id="btnSaveTab2" type="submit" value="Save" />
                    <%}
                      else
                      { %>
                    <%--<input id="btnUpdateTab2" type="submit" value="Update" />--%>
                    <%} %>
                    <input type="button" value="Back" onclick="loadFunction(0, 'Back');" />
                </div>
            </div>
            <%} %>
        </div>
        <div id="tabs-3">
            <%using (@Ajax.BeginForm("SaveRTFQuarterly", "ProgressReport", FormMethod.Post, new AjaxOptions { UpdateTargetId = "content" }, new { id = "saveRTFQuarterly", enctype = "multipart/form-data" }))
              { %>
            <div class="tabContainer">
                <table class="tabTable">
                    <tr>
                        <td>
                            <label class="lblSpan">Baseline Start Date</label><span class="span-align">*</span></td>
                        <td>
                            <label class="lblSpan">Baseline End Date</label><span class="span-align">*</span></td>
                        <td>
                            <label class="lblSpan">Report Date</label><span class="span-align">*</span></td>
                    </tr>
                    <tr>
                        <td style="width:30%"><%=Html.TextBoxFor(m => m.RTF_Q_BLStart, new { @class = "validate[required] datepicker", @style = "width:90%;" })%></td>
                        <td style="width:30%"><%=Html.TextBoxFor(m => m.RTF_Q_BLEnd, new { @class = "validate[required] datepicker", @style = "width:90%;" })%></td>
                        <td style="width:30%"><%=Html.TextBoxFor(m => m.RTF_Q_RptDate, new { @class = "validate[required] datepicker", @style = "width:90%;" })%></td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <label class="lblSpan">Description of Target Behaviors</label><br />
                            <%=Html.TextAreaFor(m => m.RTF_Q_TBehavior, new { value = @Model.RTF_Q_TBehavior, @style = "width:100%;" })%>
                            
                            <%=Html.HiddenFor(m=>m.ProReportId, Model.ProReportId) %>
                            <%=Html.HiddenFor(m=>m.RTFQId, Model.RTFQId) %>
                            <%=Html.HiddenFor(x=>x.reportType,3) %>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <br />
                            <label class="lblSpan"><u>Behaviors Targeted</u></label><br />
                            <%--start--%>
                            <table>
                                        <%foreach (var goalitem in Model.hashQuarter.goalHash.Keys)
                      {%>
                                <tr>
                                    <td>
                                        <label class="lblSpan"><%: goalitem %></label>
                                        <br />
                                        <%: ((ClientDB.AppFunctions.ProgressExprtApp.ObjectiveClass)Model.hashQuarter.goalHash[goalitem]).objectives %>
                                        <br />
                                    </td>
                                </tr>
                                        <%} %>
                            </table>
                            <%--end--%>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <br />
                            <label class="lblSpan">Outlines of Interventions the Behavior Support Plan</label><br />
                            <%=Html.TextAreaFor(m => m.RTF_Q_Outlines, new { value = @Model.RTF_Q_Outlines, @style = "width:100%;" })%>
                        </td>
                    </tr>

                    <%--start--%>
                    <%int goalIdQ = 1;  %>
                    <tr>
                        <td colspan="3">
                            <br />
                           

                            <%--<label class="lblSpan"><u><%=goalIdQ+" - "+ goalitem.ToString()%></u></label>--%>

                            <%--goal description here--%>
                            <% 
                            var q1Date=Model.dateQ1.ToString("MM/dd/yyyy");
                            var q2Date=Model.dateQ2.ToString("MM/dd/yyyy");
                            var q3Date=Model.dateQ3.ToString("MM/dd/yyyy");
                            var q4Date=Model.dateQ4.ToString("MM/dd/yyyy");
                            if (q1Date == "01-01-0001" || q1Date == "01/01/0001")
                            {
                                q1Date = "";
                            }
                            if (q2Date == "01-01-0001" || q2Date == "01/01/0001")
                            {
                                q2Date = "";
                            }
                            if (q3Date == "01-01-0001" || q3Date == "01/01/0001")
                            {
                                q3Date = "";
                            }
                            if (q4Date == "01-01-0001" || q4Date == "01/01/0001")
                            {
                                q4Date = "";
                            }
                            if (q1Date != "" && q2Date != "")
                            {
                                q2Date = q2Date + " - ";
                            }
                            if (q3Date != "" && q4Date != "")
                            {
                                q4Date = q4Date + " - ";
                            }
                            %>
                            

                            <table style="border: 1px solid black; border-collapse: collapse">
                                <tr>
                                    <td class="tblClass">Target Behavior</td>
                                    <td class="tblClass">Setting</td>
                                    <%--<td class="tblClass"><%: Model.dateQ4.ToString("MM/dd/yyyy") +" - "+Model.dateQ3.ToString("MM/dd/yyyy") %></td>
                                    <td class="tblClass"><%: Model.dateQ2.ToString("MM/dd/yyyy") +" - "+Model.dateQ1.ToString("MM/dd/yyyy") %></td>--%>
                                    <td class="tblClass"><%: q4Date + q3Date %></td>
                                    <td class="tblClass"><%: q2Date + q1Date %></td>
                                </tr>

                                 <%foreach (var goalitem in Model.hashQuarter.goalHash.Keys)
                      {%>

                                <%foreach (var lessonitem in ((ClientDB.AppFunctions.ProgressExprtApp.ObjectiveClass)Model.hashQuarter.goalHash[goalitem]).lessonaPlans)
                              { %>
                                <tr>
                                    <%if(lessonitem!=null) 
                                      {%>
                                    <td class="tblClass"><%: lessonitem%></td>
                                    <td class="tblClass">School</td> 
                                    <%if (((string[])Model.hashQuarter.LessonHash[lessonitem])[2]!=null) 
                                      {%>
                                    <td class="tblClass"><%: ((string[])Model.hashQuarter.LessonHash[lessonitem])[2]%></td>
                                    <%} 
                                      else
                                      {%>
                                    <td class="tblClass"></td>
                                    <%}
                                      if (((string[])Model.hashQuarter.LessonHash[lessonitem])[3]!=null) 
                                      {%>
                                    <td class="tblClass"><%: ((string[])Model.hashQuarter.LessonHash[lessonitem])[3]%></td>
                                    <%}
                                      else
                                      {%>
                                    <td class="tblClass"></td>
                                    <%}
                                      }%>
                                </tr>
                                <%} %>
                            
                            <% 
                          goalIdQ++; 
                            }%>
                                </table>
                        </td>
                    </tr>
                    <%--end--%>
                    
                </table>
                <div class="btnDoneStyle">
                    <%if (Model.RTFQId > 0)
                      {
                          if (ViewBag.permission == "true")
                          {
                              if (Session["PlacementStat"].ToString() != "I")
                              {%>
                    <input type="button" value="Export" style="width:91px !important;" onclick="load('Ouarter')" />
                    <%}
                          }
                      }%>
                    <%if(Model.btnStatus.Equals(false))
                      { %>
                    <input id="btnSaveRTFQuarterly" type="submit" value="Save" />
                    <%}
                      else
                      { if(ViewBag.permission=="true")
        {if(Session["PlacementStat"].ToString() != "I"){%>
                    <input id="btnUpdateRTFQuarterly" type="submit" value="Update" />
                     <%} }} %>
                    <input type="button" value="Back" onclick="loadFunction(0, 'Back');" />
                </div>
            </div>
            <%} %>
        </div>
        <div id="tabs-4">
            <%using (@Ajax.BeginForm("SaveRTFMonthly", "ProgressReport", FormMethod.Post, new AjaxOptions { UpdateTargetId = "content" }, new { id = "saveRTFMonthly", enctype = "multipart/form-data" }))
            { %>
            <div class="tabContainer">
                <table class="tabTable">
                    <tr>
                        <td>
                            <label class="lblSpan">Baseline Start Date</label><span class="span-align">*</span></td>
                        <td>
                            <label class="lblSpan">Baseline End Date</label><span class="span-align">*</span></td>
                        <td>
                            <label class="lblSpan">Report Date</label><span class="span-align">*</span></td>
                    </tr>
                    <tr>
                        <td style="width:30%"><%=Html.TextBoxFor(m => m.RTF_M_BLStart, new { @class = "validate[required] datepicker", @style = "width:90%;" })%></td>
                        <td style="width:30%"><%=Html.TextBoxFor(m => m.RTF_M_BLEnd, new { @class = "validate[required] datepicker", @style = "width:90%;" })%></td>
                        <td style="width:30%"><%=Html.TextBoxFor(m => m.RTF_M_RptDate, new { @class = "validate[required] datepicker", @style = "width:90%;" })%></td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <br />
                            <label class="lblSpan">Background Information</label>
                            <br />
                            <%=Html.TextAreaFor(m => m.RTF_M_BgInfo, new { value = @Model.RTF_M_BgInfo, @style = "width:100%;" })%>
                            
                            <%=Html.HiddenFor(m=>m.ProReportId, Model.ProReportId) %>
                            <%=Html.HiddenFor(m=>m.RTFMId, Model.RTFMId) %>
                            <%=Html.HiddenFor(x=>x.reportType,4) %>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <br />
                            <label class="lblSpan">Behavior Support Plan</label>
                            <br />
                            <%=Html.TextAreaFor(m => m.RTF_M_BSPlan, new { value = @Model.RTF_M_BSPlan, @style = "width:100%;" })%>
                    
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <br />
                            <label class="lblSpan">Assessments</label>
                            <br />
                            <%=Html.TextAreaFor(m => m.RTF_M_Assessments, new { value = @Model.RTF_M_Assessments, @style = "width:100%;" })%>
                    
                        </td>
                    </tr>
                    <%int goalId = 1;  %>
                    <% 
                var m1Date = Model.date1.ToString("MM/dd/yyyy");
                var m2Date = Model.date2.ToString("MM/dd/yyyy");
                var m3Date = Model.date3.ToString("MM/dd/yyyy");
                var m4Date = Model.date4.ToString("MM/dd/yyyy");
                var m5Date = Model.date5.ToString("MM/dd/yyyy");
                var m6Date = Model.date6.ToString("MM/dd/yyyy");
                if (m1Date == "01-01-0001" || m1Date == "01/01/0001")
                {
                    m1Date = "";
                }
                if (m2Date == "01-01-0001" || m2Date == "01/01/0001")
                {
                    m2Date = "";
                }
                if (m3Date == "01-01-0001" || m3Date == "01/01/0001")
                {
                    m3Date = "";
                }
                if (m4Date == "01-01-0001" || m4Date == "01/01/0001")
                {
                    m4Date = "";
                }
                if (m5Date == "01-01-0001" || m5Date == "01/01/0001")
                {
                    m5Date = "";
                }
                if (m6Date == "01-01-0001" || m6Date == "01/01/0001")
                {
                    m6Date = "";
                }
                            
                            if (m1Date != "" && m2Date != "")
                            {
                                m2Date = m2Date + " - ";
                            }
                            if (m3Date != "" && m4Date != "")
                            {
                                m4Date = m4Date + " - ";
                            }
                            if (m5Date != "" && m6Date != "")
                            {
                                m6Date = m6Date + " - ";
                            }
                            %>
                    <tr>
                        <td colspan="3">
                            <br />
                            <%foreach (var goalitem in Model.hash.goalHash.Keys)
                      {%>

                            <label class="lblSpan"><u><%=goalId+" - "+ goalitem.ToString()%></u></label>
                            
                            <%--goal description here--%>

                            <table style="border: 1px solid black; border-collapse: collapse">
                                <tr>
                                    <td class="tblClass">Target Behavior</td>
                                    <td class="tblClass">Baseline(<%=Model.RTF_M_BLStart +" - "+Model.RTF_M_BLEnd %>)</td>
                                    <td class="tblClass"><%: m6Date + m5Date %></td>
                                    <td class="tblClass"><%: m4Date + m3Date %></td>
                                    <td class="tblClass"><%: m2Date + m1Date %></td>
                                </tr>
                                <%foreach (var lessonitem in((ClientDB.AppFunctions.ProgressExprtApp.ObjectiveClass) Model.hash.goalHash[goalitem]).lessonaPlans)
                              { %>
                                <tr>
                                    <%if(lessonitem!=null) 
                                      {%>
                                    <td class="tblClass"><%: lessonitem%></td>
                                    <%if (((string[])Model.hash.LessonHash[lessonitem])[0]!=null) 
                                      {%>
                                    <td class="tblClass"><%: ((string[])Model.hash.LessonHash[lessonitem])[0]%></td>
                                    <%}
                                      else
                                      {%>
                                    <td class="tblClass"></td>
                                    <%} 
                                    if (((string[])Model.hash.LessonHash[lessonitem])[1]!=null) 
                                      {%>
                                    <td class="tblClass"><%: ((string[])Model.hash.LessonHash[lessonitem])[1]%></td>
                                    <%} 
                                      else
                                      {%>
                                    <td class="tblClass"></td>
                                    <%}
                                      if (((string[])Model.hash.LessonHash[lessonitem])[2]!=null) 
                                      {%>
                                    <td class="tblClass"><%: ((string[])Model.hash.LessonHash[lessonitem])[2]%></td>
                                    <%}
                                      else
                                      {%>
                                    <td class="tblClass"></td>
                                    <%}
                                      if (((string[])Model.hash.LessonHash[lessonitem])[3]!=null) 
                                      {%>
                                    <td class="tblClass"><%: ((string[])Model.hash.LessonHash[lessonitem])[3]%></td>
                                    <%}
                                    else
                                      {%>
                                    <td class="tblClass"></td>
                                    <%}
                                      }%>
                                </tr>
                                <%} %>
                            </table>
                            <br />
                            <% 
                          goalId++; 
                            }%>
                        </td>
                    </tr>



                    <tr>
                        <td colspan="3">
                            <br />
                            <label class="lblSpan">Community Integration</label>
                            <br />
                            <%=Html.TextAreaFor(m => m.RTF_M_CIntegration, new { value = @Model.RTF_M_CIntegration, @style = "width:100%;" })%>
                    
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <br />
                            <label class="lblSpan">Current Medication</label>
                            <br />
                            <%=Html.TextAreaFor(m => m.RTF_M_CMedication, new { value = @Model.RTF_M_CMedication, @style = "width:100%;" })%>
                    
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <br />
                            <label class="lblSpan"><u>Discharge Planning</u></label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label class="lblSpan">Anticipated discharge site:</label>
                        </td>
                        <td colspan="2">
                            <%=Html.TextBoxFor(m => m.RTF_M_ADSite, Model.RTF_M_ADSite, new { @style = "width:95%;" })%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label class="lblSpan">Anticipated duration of stay:</label>
                        </td>
                        <td colspan="2">
                            <%=Html.TextBoxFor(m => m.RTF_M_ADStay, Model.RTF_M_ADStay, new { @style = "width:95%;" })%>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">

                            <%=Html.TextAreaFor(m => m.RTF_M_DPlanning, new { value = @Model.RTF_M_DPlanning, @style = "width:100%;" })%>
                            
                        </td>
                    </tr>
                </table>
                <div class="btnDoneStyle">
                    <%if (Model.RTFMId > 0)
                      {
                          if (ViewBag.permission == "true")
                          {
                              if (Session["PlacementStat"].ToString() != "I")
                              {%>
                    <input type="button" value="Export" style="width:91px !important;" onclick="load('Month')" />
                    <%}
                          }
                      } %>
                    <%if (Model.btnStatus.Equals(false))
                      { %>
                    <input id="btnSaveRTFMonthly" type="submit" value="Save" />
                    <%}
                      else
                      {
                          if (ViewBag.permission == "true")
                          {
                              if (Session["PlacementStat"].ToString() != "I")
                              {%>
                    <input id="btnUpdateRTFMonthly" type="submit" value="Update" />
                    <%}
                          }
                      }%>
                    <input type="button" value="Back" onclick="loadFunction(0, 'Back');" />
                </div>
            </div>
            <%} %>
        </div>
        <div id="tabs-5">
            <%using (@Ajax.BeginForm("SaveCRMonthly", "ProgressReport", FormMethod.Post, new AjaxOptions { UpdateTargetId = "content" }, new { id = "saveCRMonthly", enctype = "multipart/form-data" }))
              { %>
            <div class="tabContainer">
                <table class="tabTable">
                    <tr>
                        <td style="align-items:center">
                            <table style="width: 100%">
                                <tr>
                                    <td><label class="lblSpan">Student Name</label></td>
                                    <td><label class="lblSpan">Location</label></td>
                                    <td><label class="lblSpan">Program</label></td>
                                    <td><label class="lblSpan">IEP/ISP Year</label></td>
                                    <td><label class="lblSpan">Period of Assessment</label></td>
                                </tr>
                                <% 
                  var d1 = Model.IEPYrStart.ToString("MM/dd/yyyy");
                  var d2 = Model.IEPYrEnd.ToString("MM/dd/yyyy");
                  var d3 = Model.PeriodStart.ToString("MM/dd/yyyy");
                  var d4 = Model.PeriodEnd.ToString("MM/dd/yyyy");
                  if (d1 == "01-01-0001" || d1 == "01/01/0001")
                  {
                      d1 = "";
                  }
                  if (d2 == "01-01-0001" || d2 == "01/01/0001")
                  {
                      d2 = "";
                  }
                  if (d3 == "01-01-0001" || d3 == "01/01/0001")
                  {
                      d3 = "";
                  }
                  if (d4 == "01-01-0001" || d4 == "01/01/0001")
                  {
                      d4 = "";
                  }
                            
                            if (d1 != "" && d2 != "")
                            {
                                d1 = d1 + " - ";
                            }
                            if (d3 != "" && d4 != "")
                            {
                                d3 = d3 + " - ";
                            }
                            %>
                                <tr>
                                    <td><%:Model.StdtName %></td>
                                    <td><%:Model.Location %></td>
                                    <td><%:Model.Program %></td>
                                    <%--<td><%: Model.IEPYrStart.ToString("MM/dd/yyyy") +" - " + Model.IEPYrEnd.ToString("MM/dd/yyyy") %></td>
                                    <td><%: Model.PeriodStart.ToString("MM/dd/yyyy") +" - " + Model.PeriodEnd.ToString("MM/dd/yyyy") %></td>--%>
                                    <td><%: d1 + d2 %></td>
                                    <td><%: d3 + d4 %></td>
                                </tr>
                            </table>
                            <br />

                            <label class="lblSpan"><u>Setting Events and Program Changes</u></label><br />
                            Briefly identify variables or factors in the following areas that may help explain behavior change(Include dates and list most recent first).
        <table style="width: 100%">
            <tr>
                <td style="width: 20%; text-align: center"><label class="lblSpan">Type</label></td>
                <td style="width: 80%; text-align: center"><label class="lblSpan">Description</label></td>
            </tr>
            <tr>
                <td><label class="lblSpan">Phaselines</label></td>
                <td><%=Model.CRM_PhaseLines %><br /></td>
            </tr>
            <tr>
                <td><label class="lblSpan">Condition lines</label></td>
                <td><%=Model.CRM_ConditionLines %><br /></td>
            </tr>
            <tr>
                <td><label class="lblSpan">Arrow notes</label></td>
                <td><%=Model.CRM_ArrowNotes %><br /></td>
            </tr>
            <tr>
                <td><label class="lblSpan">Academic Status</label></td>
                <td><%=Html.TextAreaFor(m => m.CRM_Academic, new { value = @Model.CRM_Academic, @style = "width:100%;" })%>
                    <%=Html.HiddenFor(m=>m.ProReportId, Model.ProReportId) %>
                    <%=Html.HiddenFor(m=>m.ProgressId, Model.ProgressId) %>
                    <%=Html.HiddenFor(x=>x.reportType,5) %>
                </td>
            </tr>
            <tr>
                <td><label class="lblSpan">Clinical Status</label></td>
                <td><%=Html.TextAreaFor(m => m.CRM_Clinical, new { value = @Model.CRM_Clinical, @style = "width:100%;" })%></td>
            </tr>
            <tr>
                <td><label class="lblSpan">Community Outings</label></td>
                <td><%=Html.TextAreaFor(m => m.CRM_Outings, new { value = @Model.CRM_Outings, @style = "width:100%;" })%></td>
            </tr>
            <tr>
                <td><label class="lblSpan">Other</label></td>
                <td><%=Html.TextAreaFor(m => m.CRM_Other, new { value = @Model.CRM_Other, @style = "width:100%;" })%></td>
            </tr>
        </table>
                        </td>
                    </tr>

                </table>

                <div class="btnDoneStyle">
                    <%if (Model.btnStatus.Equals(false))
                      { %>
                    <input id="btnSaveCRMonthly" type="submit" value="Save" />
                    <%}
                      else
                      {
                          if (ViewBag.permission == "true")
                          {
                              if (Session["PlacementStat"].ToString() != "I")
                              {%>
                    <input id="btnUpdateCRMonthly" type="submit" value="Update" />
                    <%}
                          }
                      } %>
                    <input type="button" value="Back" onclick="loadFunction(0, 'Back');" />
                </div>
            </div>
            <%} %>
        </div>
    </div>
</body>
    <script type="text/javascript">
        var tabId = <%=Model.TabId %>
        $('.topTabs li').hide();
        $('#Utabs-' + tabId).parent().show();
        $('#Utabs-' + tabId).trigger('click');
    </script>
</html>
