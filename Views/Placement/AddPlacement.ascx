<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ClientDB.Models.AddPlacementModel>" %>
<script src="../Documents/JS/jquery.validationEngine-en.js"></script>
<script src="../Documents/JS/jquery.validationEngine.js"></script>


<link href="../../Documents/CSS/jquery-ui.css" rel="stylesheet" />
<script src="../../Documents/JS/jquery-ui-1.8.24.js"></script>
<script type="text/javascript">


    var Arr_Record = [];
    var Arr_Ticker = [];
    var Arr_NewEventLogsParent = [];
    var Arr_NewEventLogsChild = [];


    function ValidateDate() {
        checkStudentStatus();
        //var discharged = $("#staticTab").find("span").html();
        //if (discharged == "(Discharge)") {
        //    alert("Activate student");
        //    return false;
        //}
        var fromDate = $('#StartDate').val();
        var toDate = $('#EndDate').val();

        var startDate = new Date(fromDate);
        var endDate = new Date(toDate);
        if (fromDate != '' && toDate != '' && startDate > endDate) {
            alert("Start Date must be greater than or equal to  End Date.");
            return false;
        }

        return true;
    }

    var date = new Date();
    date.setDate(date.getDate());

    $('#ddlPlcreason').change(function(){
        var plcVal = $(this).find('option:selected').text();
        if (plcVal == "Discharge")
        {
            $('#EndDate').addClass("validate[required]");
            $("#lblEnd").after("<span class='span-align' id='idSpan'>*</span>");
        }
        else
        {
            $('#EndDate').removeClass("validate[required]");
            $('#idSpan').hide();
        }
        });

    $(document).ready(function () {

        $("#StartDate").datepicker({
            changeMonth: true,
            changeYear: true,
            showAnim: "fadeIn",
            yearRange: 'c-80:c+80',
            //minDate: date,
            /* fix buggy IE focus functionality */
            fixFocusIE: false,
            onSelect: function (dateText, inst) {

                var newValue = $(this).val();
                var controlId = $(this).attr('id');
                var labelText = $(this).siblings('.lblSpan').text();
                var prevValue = Arr_Record[3];

                if (isInitialValue(controlId, newValue)) {
                   
                    message = '[' + labelText + ']' + ' was changed from "' + prevValue + '" to "' + newValue + '".';
                    
                    Arr_Record[4] = newValue;
                    Arr_Record[5] = message;
                    Arr_Ticker.push(Arr_Record);
                    Arr_Record = [];

                    //newEvent = "Placement||| calendar |||" + labelText + "|||" + prevValue + "|||" + newValue;
                    //document.getElementById('newEventLog').value += newEvent + ">>>";
                    var pageType = "Placement";

                    Arr_NewEventLogsChild = [pageType, "calendar", labelText, prevValue, newValue];
                    var flag = 0;

                    if (Arr_NewEventLogsParent.length == 0) {
                        Arr_NewEventLogsParent.push(Arr_NewEventLogsChild);
                    }
                    else {
                        for (var i = 0; i < Arr_NewEventLogsParent.length; i++) {
                            if (Arr_NewEventLogsParent[i][2] == labelText) {
                                Arr_NewEventLogsParent[i][4] = newValue;
                                flag = 1;
                            }
                        }
                        if (flag == 0) {
                            Arr_NewEventLogsParent.push(Arr_NewEventLogsChild);
                        }
                    }
                    Arr_NewEventLogsChild = [];
                }
                
                writeToTickerBox(Arr_Ticker);
            }


        }).attr('readonly', 'true');
        $('#EndDate').datepicker({
            changeMonth: true,
            changeYear: true,
            showAnim: "fadeIn",
            yearRange: 'c-80:c+80',
            //minDate: date,
            /* fix buggy IE focus functionality */
            fixFocusIE: false,
            onSelect: function (dateText, inst) {

                var newValue = $(this).val();
                var controlId = $(this).attr('id');
                var labelText = $(this).siblings('.lblSpan').text();
                var prevValue = Arr_Record[3];

                if (isInitialValue(controlId, newValue)) {

                    message = '[' + labelText + ']' + ' was changed from "' + prevValue + '" to "' + newValue + '".';

                    Arr_Record[4] = newValue;
                    Arr_Record[5] = message;
                    Arr_Ticker.push(Arr_Record);
                    Arr_Record = [];

                    //newEvent = "Placement||| calendar |||" + labelText + "|||" + prevValue + "|||" + newValue;
                    //document.getElementById('newEventLog').value += newEvent + ">>>";
                    var pageType = "Placement";

                    Arr_NewEventLogsChild = [pageType, "calendar", labelText, prevValue, newValue];
                    var flag = 0;

                    if (Arr_NewEventLogsParent.length == 0) {
                        Arr_NewEventLogsParent.push(Arr_NewEventLogsChild);
                    }
                    else {
                        for (var i = 0; i < Arr_NewEventLogsParent.length; i++) {
                            if (Arr_NewEventLogsParent[i][2] == labelText) {
                                Arr_NewEventLogsParent[i][4] = newValue;
                                flag = 1;
                            }
                        }
                        if (flag == 0) {
                            Arr_NewEventLogsParent.push(Arr_NewEventLogsChild);
                        }
                    }
                    Arr_NewEventLogsChild = [];
                }

                writeToTickerBox(Arr_Ticker);
            }
        }).attr('readonly', 'true');

        $("#ddlPlcreason option").each(function () {
            if ($(this).text().trim() === "Discharge") {
                $(this).prop("disabled", true);
            }
        });
    });
    jQuery("#addPlacement").validationEngine();

    function Redirect(data) {

        try{
            var ServerObj = JSON.parse(data);
            if (ServerObj.hasOwnProperty("logicError")) {
                
                if (ServerObj.logicError == true) {
                    if (ServerObj.id > 0) {
                        // $('#AddNewPlacement').load('../Placement/AddPlacement/' + ServerObj.id);
                        $('#divErrorLimit').find('p').empty();
                        $('#divErrorLimit').find('p').append(ServerObj.ErrorMsg);
                    }
                    else {
                        $('#divErrorLimit').find('p').empty();
                        $('#divErrorLimit').find('p').append(ServerObj.ErrorMsg);
                        //errMsg.text = ServerObj.ErrorMsg;
                        //alert(errMsg.text)
                    }

                } else
                    $('#content').load('../Placement/Placement');
            }
            else {
                $('#content').load('../Placement/Placement');
            }
        }
        catch (e) {
            console.log(JSON.stringify(e.message));
            $('#content').load('../Placement/Placement');
        }

    }

</script>
<style type="text/css">
    .ui-datepicker select.ui-datepicker-month, .ui-datepicker select.ui-datepicker-year {
        width: 49% !important;
    }

    .lblSpan {
        font-style: italic;
        font-weight: bold;
    }

    input[type="text"] {
        background-color: white;
        border: 1px solid #d7cece;
        border-radius: 3px;
        color: #676767;
        font-family: Arial,Helvetica,sans-serif;
        font-size: 13px;
        height: 25px;
        line-height: 26px;
        padding: 0 5px 0 10px;
        width: 150px !important;
        margin-bottom: 1px;
    }

    select {
        width: 167px !important;
        border: 1px solid #CCC;
        height: 29px;
        vertical-align: middle;
        font-family: Arial;
        font-size: 12px;
        color: #333;
        margin-bottom: 1px;
    }
</style>

<%using (@Ajax.BeginForm("SavePlacement", "Placement", FormMethod.Post, new AjaxOptions { UpdateTargetId = "", OnSuccess = "Redirect" }, new { id = "addPlacement", enctype = "multipart/form-data" }))
  { %>
    <div id="divErrorLimit">
    <p  style="width: 100%; color: red; font-size: 14px; font-weight: bold"></p>
   </div>

<table style="width: 100%;">
    <%=Html.HiddenFor(m=>m.Id,Model.Id) %>
    <%=Html.HiddenFor(m=>m.newEventLog,Model.newEventLog) %>

    <tr>
         <td colspan="2">
            <label class="lblSpan">Department</label><span class="span-align">*</span><br />
            <%=Html.DropDownListFor(m => m.PlacementDepartmentId, Model.PlacementDepartmentList, new {@class = "validate[required]"})%></td>
        
        <td colspan="2">
            <label class="lblSpan">Start Date</label><span class="span-align">*</span><br />
            <%=Html.TextBoxFor(m => m.StartDate, Model.StartDate, new { @class = "validate[required] datepicker", ID = "StartDate" })%></td>
        <td rowspan="7" colspan="4">
            <table>
                <tr>
                    <th style="min-width:95px; text-align:center !important;" colspan="2">
                        Days
                    </th>
                    <th>
                        Timings/Time Of the Day
                    </th>
                </tr>
                <tr>
                    <td style="min-width:95px;" colspan="2">
                        
                        <%: Html.CheckBoxFor(x=>x.IsMonday) %>
                        <label class="lblSpan">Monday</label><span class="nospan-align"></span>
                        
                    </td>
                    <td colspan="1">
                        <%=Html.TextBoxFor(m => m.MondayNote, Model.MondayNote)%>
                    </td>
                </tr>
                <tr>
                    <td style="min-width:95px;" colspan="2">
                        
                        <%: Html.CheckBoxFor(x=>x.IsTuesday ) %>
                        <label class="lblSpan">Tuesday</label><span class="nospan-align"></span>
                        
                    </td>
                    <td style="min-width:95px;" colspan="1">
                        <%=Html.TextBoxFor(m => m.TuesdayNote, Model.TuesdayNote)%>
                    </td>
                </tr>
                <tr>
                     <td style="min-width:95px;" colspan="2">
                        
                        <%: Html.CheckBoxFor(x=>x.IsWednesday ) %>
                         <label class="lblSpan">Wednesday </label><span class="nospan-align"></span>
                       
                    </td>
                     <td colspan="1">
                          <%=Html.TextBoxFor(m => m.WednesdayNote, Model.WednesdayNote)%>
                         </td>
                </tr>
                <tr>
                    <td style="min-width:95px;" colspan="2">
                        
                        <%: Html.CheckBoxFor(x=>x.IsThursday  ) %>
                        <label class="lblSpan">Thursday </label><span class="nospan-align"></span>
                        
                    </td>
                    <td colspan="1">
                        <%=Html.TextBoxFor(m => m.ThursdayNote, Model.ThursdayNote)%>
                        </td>
                </tr>
                <tr>
                    <td style="min-width:95px;" colspan="2">
                        
                        <%: Html.CheckBoxFor(x=>x.IsFriday ) %>
                        <label class="lblSpan">Friday  </label><span class="nospan-align"></span>
                        
                    </td>
                     <td colspan="1">
                         <%=Html.TextBoxFor(m => m.FridayNote, Model.FridayNote)%>
                         </td>
                </tr>
                <tr>
                    <td style="min-width:95px;" colspan="2">
                        
                        <%: Html.CheckBoxFor(x=>x.IsSaturday   ) %>
                        <label class="lblSpan">Saturday</label><span class="nospan-align"></span>
                       
                    </td>
                    <td colspan="1">
                         <%=Html.TextBoxFor(m => m.SaturdayNote, Model.SaturdayNote)%>
                        </td>
                </tr>
                <tr>
                    <td style="min-width:95px;" colspan="2">
                       
                        <%: Html.CheckBoxFor(x=>x.IsSunday) %>
                         <label class="lblSpan">Sunday </label><span class="nospan-align"></span>
                       
                    </td>
                    <td colspan="1">
                         <%=Html.TextBoxFor(m => m.SundayNote, Model.SundayNote)%>
                        </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <label class="lblSpan">Placement Type</label><span class="span-align">*</span><br />
            <%=Html.DropDownListFor(m => m.PlacementType, Model.PlacementTypeList, new { @class = "validate[required] ", ID = "ddlEventType" })%></td>
        
         <td colspan="2">
            <label class="lblSpan" id="lblEnd">Discharge/End Date</label><span id=""></span><br />
            <%=Html.TextBoxFor(m => m.EndDateDate, Model.EndDateDate, new { @class = "datepicker", ID = "EndDate" })%></td>
        

    </tr>
    <tr>
        <td colspan="2">
            <label class="lblSpan">Program</label><span class="span-align">*</span><br />
            <%=Html.DropDownListFor(m => m.Department, Model.DepartmentList, new { @class = "validate[required] ", ID = "ddlEventPgm" })%></td>
       
       <td colspan="2">
            <label class="lblSpan">Location</label><span class="span-align">*</span><br />
            <%=Html.DropDownListFor(m => m.LocationId, Model.LocationList, new {@class = "validate[required] ", ID = "ddlLoc"})%></td>
    </tr>
   
        <tr>
        <td colspan="2">
            <label class="lblSpan">Placement Description/Reason</label><span class="nospan-align"></span><br />
            <%=Html.TextAreaFor(m => m.Reason,new {value=@Model.Reason,  @class="sd", @rows="3",@cols="50"})%>
        </td>
            <td colspan="2"> <label class="lblSpan">Placement Reason</label><span class="span-align">*</span><br />
             <%=Html.DropDownListFor(m => m.PlacementReason, Model.PlacementReasonList, new { @class = "validate[required] ", ID = "ddlPlcreason"  })%></td>
    </tr>

    <%if (ViewBag.school == "NE")
      { %>


    <%}
      else
      { %>

    <tr style="display:none">
        <td colspan="2">
            <label class="lblSpan">Primary Nurse</label><span class="nospan-align">*</span><br />
            <%=Html.DropDownListFor(m => m.PrimaryNurse, Model.PrimaryNurseList, new { @class = " ", ID = "ddlEventType" })%></td>
        <td colspan="2">
            <label class="lblSpan">Behavior Analyst</label><span class="nospan-align">*</span><br />
            <%=Html.DropDownListFor(m => m.BehaviorAnalyst, Model.BehaviorAnalystList, new { @class = " ", ID = "ddlEventType" })%></td>
    </tr>


    <tr  style="display:none">
        <td colspan="2">
            <label class="lblSpan">Unit Clerk</label><span class="nospan-align">*</span><br />
            <%=Html.DropDownListFor(m => m.UnitClerk, Model.UnitClerkList, new { @class = "", ID = "ddlEventType" })%></td>
    </tr>

    <%} %>
</table>
<div class="btnDoneStyle">
    <%if (ViewBag.permission == "true")
      {
          if (Model.Id > 0)
          { %>
    <input id="btnUpdatePlacement" type="submit" value="Update" onclick="return ValidateDate();" />
    <%}
          else
          { %>
    <input id="btnAddPlacement" type="submit" value="Save" onclick="return ValidateDate();" />
    <%}
      }%>
</div>
<div style="float: left; padding-left: 5%; padding-top: 1%">

    <%--  <input id="btnCancel" type="button" value="Cancel" onclick="" />--%>
    <%=Html.TextAreaFor(m => m.placemntLogText,new {Style="width:200px;display:none; height:200px; position:fixed; top:0px; left:0px;", ID="tickerBox"})%>
</div>

<%} %>
<script>
    
    $("#btnUpdatePlacement").on("click", function (event) {
        WriteToString(Arr_NewEventLogsParent);
    });

    $('input,select,textarea').focus(function () {
        var controlId = this.id;
        var controlType = this.type;
        var prevValue = "";
        var newValue = "";
        var labelText = $(this).siblings('.lblSpan').text();
        var message = "";

        switch (controlType) {

            case "text":
                prevValue = $(this).val();
                break;
            case "textarea":
                prevValue = $(this).val();
                break;
            case "select-one":
                prevValue = $("#" + controlId + " option:selected").text();
                break;
            case "checkbox":
                prevValue = ($(this).prop('checked')) ? "Checked" : "Unchecked";
                break;
        }
        Arr_Record = [];
        Arr_Record = [controlId, controlType, labelText, prevValue, newValue, message];
        Arr_NewEventLogsChild = [labelText, prevValue, newValue];
    });
    $('input,select,textarea').blur(function () {
        
        var controlId = this.id;
        var controlType = this.type;
        var prevValue = Arr_Record[3];
        var newValue = "";
        var labelText = $(this).siblings('.lblSpan').text();
        var message = "";

        var newEvent = "";

        switch (controlType) {

            case "text":
                newValue = $(this).val();
                break;
            case "textarea":
                newValue = $(this).val();
                break;
            case "select-one":
                newValue = $("#" + controlId + " option:selected").text();
                break;
            case "checkbox":
                newValue = ($(this).prop('checked')) ? "Checked" : "Unchecked";
                break;
        }

        if (controlId == Arr_Record[0]) {

            if (prevValue == newValue) {
                if (controlType == "text") {
                    if ($('#' + controlId).hasClass('hasDatepicker')) {


                        return;

                    }

                }
                Arr_Record = [];
                Arr_NewEventLogsChild = [];
            }
            else {
                if (isInitialValue(controlId, newValue)) {

                    if (labelText == "") { labelText = controlId}

                    if (controlType == "textarea") {
                        message = '[' + labelText + ']' + ' was changed.';
                    }
                    else {
                        message = '[' + labelText + ']' + ' was changed from "' + prevValue + '" to "' + newValue + '".';
                    }
                    
                    Arr_Record[4] = newValue;
                    Arr_Record[5] = message;
                    Arr_Ticker.push(Arr_Record);
                    Arr_Record = [];

                    var newControlType = "";
                    if (controlType == "select-one") {
                        newControlType = "dropdownlist";
                    }
                    else {
                        newControlType = controlType;
                    }

                    //newEvent = "Placement|||" + newControlType + "|||" + labelText + "|||" + prevValue + "|||" + newValue;
                    //document.getElementById('newEventLog').value += newEvent + ">>>";

                    var pageType = "Placement";

                    Arr_NewEventLogsChild = [pageType, newControlType, labelText, prevValue, newValue];
                    var flag = 0;

                    if (Arr_NewEventLogsParent.length == 0) {
                        Arr_NewEventLogsParent.push(Arr_NewEventLogsChild);
                    }
                    else {
                        for (var i = 0; i < Arr_NewEventLogsParent.length; i++) {
                            if (Arr_NewEventLogsParent[i][2] == labelText) {
                                Arr_NewEventLogsParent[i][4] = newValue;
                                flag = 1;
                            }
                        }
                        if (flag == 0) {
                            Arr_NewEventLogsParent.push(Arr_NewEventLogsChild);
                        }
                    }
                    Arr_NewEventLogsChild = [];
                }
            }
        }
        writeToTickerBox(Arr_Ticker);
    });

    function isInitialValue(controlId, newVal) {
        for (var i = 0; i < Arr_Ticker.length; i++) {
            if (Arr_Ticker[i][0] == controlId) {
                if (Arr_Ticker[i][3] == newVal) {
                    Arr_Ticker.splice(i, 1);
                    Arr_NewEventLogsParent.splice(i, 1);
                    return false;
                }
            }

        }

        return true;
    }

    function writeToTickerBox(arrToWrite) {

        var logString = "";
        for (var i = 0; i < arrToWrite.length; i++) {

            logString += (i + 1) + ") " + arrToWrite[i][5] + "\n";
        }
        //if (logString != "") {
        $('#tickerBox').val(logString);
        // }
    }

    function WriteToString(para_Arr_NewEventLogsParent) {
        document.getElementById('newEventLog').value = "";
        for (var i = 0; i < para_Arr_NewEventLogsParent.length; i++) {
            newEvent = para_Arr_NewEventLogsParent[i][0] + "|||" + para_Arr_NewEventLogsParent[i][1] + "|||" + para_Arr_NewEventLogsParent[i][2] + "|||" + para_Arr_NewEventLogsParent[i][3] + "|||" + para_Arr_NewEventLogsParent[i][4];
            document.getElementById('newEventLog').value += newEvent + ">>>";
        }
    }

    loadClientStaticDetails();
</script>