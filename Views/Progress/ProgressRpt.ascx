<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ClientDB.Models.ProgressDetails>" %>

<style type="text/css">
    input.ExportWord,
    input.ExportWord:link,
    input.ExportWord:visited {
        width: 43px;
        height: 43px;
        display: block;
        background: url(../images/ExportWord.png) left top no-repeat;
        float: left;
        margin: 0 15px 0 0;
        border: none;
        cursor: pointer;
    }

        input.ExportWord:hover {
            background-position: 0 -45px;
        }

    .Heading {
        font-family: Arial;
        margin-bottom: 5px;
        width: 100%;
        border-bottom: 1px solid;
        height: 40px;
        font-size: 25px;
        padding-top: 10px;
        color: black;
    }

    .styleA {
        float: right;
        background: none repeat scroll 0 0 #0D668E;
        border: medium none;
        border-radius: 3px;
        color: #FFFFFF;
        cursor: pointer;
        font-size: 10px;
        font-weight: normal;
        margin: 5px 15px 0 0!important;
        padding: 3px 6px;
        text-align: center;
        text-decoration: none;
        width: 50px;
        height: 15px;
    }
</style>
<link href="../../Documents/CSS/jquery-ui.css" rel="stylesheet" />
<link href="../../Documents/CSS/jquery-ui-1.10.3.custom.css" rel="stylesheet" />
<link href="../../Documents/CSS/General.css" rel="stylesheet" />
<script src="../../Documents/JS/jquery-1.8.2.js"></script>
<script src="../../Documents/JS/jquery-ui-1.8.24.js"></script>
<script type="text/javascript">
    $(document).ready(function () {
        $(".datepicker").datepicker({
            changeMonth: true,
            changeYear: true,
            showAnim: "fadeIn",
            yearRange: 'c-30:c+30',
            maxDate: new Date,


            /* fix buggy IE focus functionality */
            fixFocusIE: true,

            /* blur needed to correctly handle placeholder text */
            onSelect: function (dateText, inst) {
                this.fixFocusIE = true;
                $(this).blur().change().focus();
                $(this).blur().change().focus();



            },
            onClose: function (dateText, inst) {
                this.fixFocusIE = true;
                this.focus();
            },
            beforeShow: function (input, inst) {
                var result = $.browser.msie ? !this.fixFocusIE : true;
                this.fixFocusIE = false;
                return result;
            }
        }).attr('readonly', 'true');
    });


</script>
<script type="text/javascript">
    //    var clicks = 0;
    function AddBtn_Click(count) {
        // alert(count)
        $('#divprogress' + count).length;
        //  alert('#divprogress' + count)
        var noOfRep = $('#divprogress' + count + " .Ctable").length;

        var clicks = noOfRep + 1;
        if (noOfRep <= 3) {
            var srtScript = " <table class='Ctable' style='width:100%;border:thick'><tr> <td>Progress Report Date:&nbsp;&nbsp;<input id='report_date" + count + clicks + "' type='text' name='GoalData[" + count + "].RptList[" + noOfRep + "].rptdate' class='datepicker' /> </td> <td></td> <td>Progress Report # " + clicks + " of 4 </td> </tr>." +
                  "<tr><td colspan='3'><input name='GoalData[" + count + "].RptList[" + noOfRep + "].rptinfo'   id='txtreport" + count + clicks + "' type='text' style='width:90%;height:60px;border:1px solid #ccc' /></td></tr></table>";

            // alert(cnt)
            $('#divprogress' + count).append(srtScript);
        }

        $(".datepicker").datepicker({
            changeMonth: true,
            changeYear: true,
            showAnim: "fadeIn",
            yearRange: 'c-30:c+30',
            maxDate: new Date,


            /* fix buggy IE focus functionality */
            fixFocusIE: true,

            /* blur needed to correctly handle placeholder text */
            onSelect: function (dateText, inst) {
                this.fixFocusIE = true;
                $(this).blur().change().focus();
                $(this).blur().change().focus();



            },
            onClose: function (dateText, inst) {
                this.fixFocusIE = true;
                this.focus();
            },
            beforeShow: function (input, inst) {
                var result = $.browser.msie ? !this.fixFocusIE : true;
                this.fixFocusIE = false;
                return result;
            }
        }).attr('readonly', 'true');

    }
    function ExportTemplate() {
        $('#content').load('../Progress/ExportAllData/');

    }
    function ProgressReport() {
        $('#dvProgressReport').load('../Progress/ProgressGrid/');
        $('#divProgress').show();

    }
    function showProgressIEP(IEPId, Status) {
        $('#divProgress').hide();
        $.get("../Progress/ProgressRpt/?Id=" + IEPId + '-' + Status, function (data) {
            $('#content').html(data);
        });
    }
    function showProgress(IEPId) {
        $('#divProgress').hide();
        //$.get("../Progress/ProgressRpt/?Id=" + IEPId, function (data) {

        //});
    }
    $(document).ready(function () {
        function ExportTemplate() {

            $.ajax({
                url: "/Progress/ExportAllData",
                type: 'POST',
                dataType: 'json',
                data: null,
                contentType: 'application/json; charset=utf-8',
                success: function (msg) {
                    window.alert("Okay");

                },
                error: handleError


            });
        }

    });
</script>


  <%using (Ajax.BeginForm("saveReport", "Progress", FormMethod.Post, new AjaxOptions { UpdateTargetId = "" }))//Modified_10-7-2014/Neethu
    { %>

 <%if (Model.ID > 0)
   {
          
           %>
    <table style="width:100%;"><tr>
        <td style="width:45%">

          
        </td>

       

        <td style="text-align:right;width:30%"><%
            
       if (ViewBag.IsChecked == false)
       {
                %>
            <%= Html.CheckBox("visibleChkBox")%> Allow Visible To Parent

            <%}
            else
            {
                %>

            <% =Html.CheckBox("visibleChkBox", new {@checked= "checked"})%> Allow Visible To Parent

            <%} %>
        </td>

        <td style="width:15%">  <a href="../Progress/ExportAllData/" class="styleA" target="_blank" style="border:none;width:100px;height:19px;font-weight:bold;font-size:small">Export</a></td>
        <td style="width:10%">  <input id="Button1"   class="styleA" type="button" value="Progress List" onclick="ProgressReport()" style="border:none;width:100px;height:25px" /></td>
                               </tr></table>

<div id="partialMainArea" style="border:1px solid;padding:5px;">

   
   
    <div>
    <div id="Sample" style="float: left; width: 100%">


        <table id="tblSchooldetails" style="width: 100%">
            <tr>
                <td class="tdLabel">School District Name:</td>
                <td colspan="2"><%=Model.SclDistrictName %></td>
                <td><%--<input id="btnExport" type="button" value="Export" onclick="ExportTemplate()"; style="margin-left: 5%" />--%>
                  <%--  <a href="../Progress/ExportAllData/" class="ExportWord" target="_blank">Export</a>--%>
                
                 
                   
                </td>
            </tr>
            <tr>
                <td class="tdLabel">School District Address:</td>
                <td colspan="2"><%=Model.SclDistrictAddress %></td>
            </tr>
            <tr>
                <td class="tdLabel">School District Contact Person/Phone #:</td>
                <td colspan="2"><%=Model.SclDistrictContact %></td>
            </tr>

        </table>
         <input type="hidden" name="stdtiep" value="<%=Model.stdtIEPId%>" />
        <br />
    </div>
    <%int count = 0;%>
    <%foreach (var item in Model.GoalDt)
      {
          int temp = count;
          string name = "divprogress" + temp;
         
    %>
    <div id="divgrid" style="float: left; width: 100%">
        <input type="hidden" name="GoalData[<%=temp %>].GoalID" value="<%=item.Goalid %>" />
        <input type="hidden" name="GoalData[<%=temp %>].GoalLPRelId" value="<%=item.GoalLPRelId %>" />
        <hr style="width: 100%; border: 5px solid;" />
        <br />
        <table id="tblDatedetails" style="width: 100%">
            <tr>
                <td>
                    Progress Report
                </td>
                <td>on IEP Dated: from &nbsp;<%=Model.IEPDtFrom.ToString("MM/dd/yyyy").Replace('-', '/') %></td>
                <td>to &nbsp; <%=Model.IEPDtTo.ToString("MM/dd/yyyy").Replace('-', '/') %></td>
            </tr>
        </table>
        <hr  style="width:100%" border:"solid" >
        <table id="tblStudentDetails" style="width: 100%">
            <tr>
                <td class="tdLabel">Student Name:&nbsp;&nbsp;<%=Model.StudentName %></td>

                <td class="tdLabel">DOB:&nbsp;&nbsp;<%=Model.DOB.ToString("MM/dd/yyyy").Replace('-', '/') %></td>

                <td class="tdLabel">ID #:&nbsp;&nbsp;<%=Model.ID %></td>
            </tr>

        </table>
        <div id="divInfo" style="width: 100%; height: 10%; background-color: black; color: white; font-size: 18px; text-align: center">INFORMATION FROM CURRENT IEP</div>
        <table id="tblCurrentIEP" style="width: 100%; border: thick">


            <tr>
                <td>Goal #:&nbsp;&nbsp;<%=item.GoalNo%>&nbsp;&nbsp;<%=item.GoalName%></td>
                <td>Specific Goal Focus:&nbsp;<%=item.LessonplanName%></td>
            </tr>
        </table>
        <%foreach (var data in item.PlcacementList)
          {
              
        %> 
        Current Performance Level:&nbsp; What can the student currently do?<br />
        <ul>
            <li><b><%=data.objective1 %></b></li>
        </ul>
        <br />
        <%--<%=Html.TextBoxFor(x=>item.PlcacementList[0].objective1,data.objective1) %>--%>
        <hr  style="width:100%" border:"solid" />
        Measurable Annual Goal:What challenging, yet attainable, goal can we expect the student to meet by the end of this IEP period?<br />
        How will we know that the student has reached this goal?<br />
        <ul>
            <li><b><%=data.objective2 %></b></li>
        </ul>
        <br />
        <hr  style="width:100%" border:"solid" />
        Benchmark/Objectives: What will the student need to do to complete this goal?<br />
        <ul>
            <li><b><%=data.objective3 %></b></li>
        </ul>
        <br />
        <br />
        <%} %>
           
        <div id="PrgRptInfo" style="width: 100%; height: 10%; background-color: black; color: white; font-size: 18px; text-align: center">PROGRESS REPORT INFORMATION</div>
        <div id="<%=name%>">
            <% string rptdate = "report_date" + temp;
               string txtreport = "txtreport" + temp;
               int index = 0;
               int quarter = 1;
               if (item.RptList != null)
               {
                   foreach (var reports in item.RptList)
                   {
                       
               
               %>

          
        <table class="Ctable" id="tblinfo" style="width: 100%; border: thick">
            <tr><td style="visibility:hidden"> <input type="text" id="<%=reports.rptid%>" name="GoalData[<%=temp %>].RptList[<%=index %>].rptid" value="<%=reports.rptid%>" /></td></tr>
            <tr>
                <td>Progress Report Date:&nbsp;&nbsp;
                    <input type="text" id="<%=rptdate%>+GoalData[<%=temp %>].RptList[<%=index %>].rptdate" name="GoalData[<%=temp %>].RptList[<%=index %>].rptdate" class="datepicker" value="<%=reports.rptdate.ToString("MM/dd/yyyy").Replace('-', '/')%>" />
                </td>
                <td></td>
                <td>Progress Report # <%=quarter %> of  4</td>
            </tr>
            <tr>
                <td colspan="3">Progress Reports are required to be sent to parents at least as often as parents are informed of their non-disabled children’s progress. Each progress report must describe the student’s progress toward meeting each annual goal.</td>
            </tr>
            <tr>
                <td colspan="3">
                    <input id="<%=txtreport+temp %>" value="<%=reports.rptinfo %>" name="GoalData[<%=temp %>].RptList[<%=index %>].rptinfo" type="text" style="width: 90%; height: 60px; border: 1px solid #ccc" /></td>
            </tr>


        </table>
               
            <%
                       index++; quarter++;
                   }
               }
               else
               {%>
                   <input type="hidden" name="GoalData[<%=temp %>].RptList[<%=index %>].rptid " value="" />
                 <table class="Ctable" id="Table1" style="width: 100%; border: thick">
            <tr>
                <td>Progress Report Date:&nbsp;&nbsp;
                    <input type="text" id="<%=rptdate%>" name="GoalData[<%=temp %>].RptList[0].rptdate" class="datepicker" " />
                </td>
                <td></td>
                <td>Progress Report # 1 of  4</td>
            </tr>
            <tr>
                <td colspan="3">Progress Reports are required to be sent to parents at least as often as parents are informed of their non-disabled children’s progress. Each progress report must describe the student’s progress toward meeting each annual goal.</td>
            </tr>
            <tr>
                <td colspan="3">
                    <input id="<%=txtreport%>" name="GoalData[<%=temp %>].RptList[0].rptinfo" type="text" style="width: 90%; height: 60px; border: 1px solid #ccc" /></td>
            </tr>


        </table>
            <%} %>
    </div>
       
    <input id="btnAdd" name="<%=temp%>" class="styleA" type="button" value="ADD HERE" onclick="AddBtn_Click(this.name)" />
    <br />
    <br />
    <br />
</div>
<%count++;
      }
      if (ViewBag.permission == "true" && (Model.Status == "" || Model.Status == "Approved"))
      {
       %>
      <input id="btnSubmit" name="btnSubmit" type="submit" value="SUBMIT" /> 
        <%} %>
        </div>
    <%}
   else
   { %>
    <div>

        No Goals and Lesson Plans Found in IEP For Progress Report....
         
    </div><%} %>
</div>

<%} %>



 <div id="divProgress" class="popUpStyle" style="width: 76%; height: 70%; left: 11%; top: 15%;overflow-y:auto;overflow-x:hidden">
        <a id="close_x" onclick="showProgress()" class="close sprited1" href="#" style="">
            <img src="../Documents/CSS/images/button_red_close.png" height="18" width="18" alt="" style="float: right; margin-right: 22px; margin-top: 16px; z-index: 300" /></a>

        
   <span> Progress List</span>
        <hr />

       <div id="dvProgressReport" >


</div>

    </div>