<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ClientDB.Models.ProtocolSummary>" %>



<%--<script src="../../Documents/JS/jquery-1.8.2.js"></script>
<script src="../../Documents/JS/jquery-ui-1.8.24.js"></script>--%>
<script type="text/javascript">



    var ATCounter = <%=Model.ATList.Count %>;
    var CGCounter = <%=Model.CGList.Count %>;
    var FICounter = <%=Model.FIList.Count %>;
    var BBICounter = <%=Model.BBIList.Count %>;
    var SignCounter = <%=Model.SignList.Count %>;




    $(document).ready(function () {

        function addRow() {
            var html = '<tr style="text-align:center">' +
            '<td style="width:30%"><textarea name="ATList[' + ATCounter + '].Type" style="width:99%"></textarea><input type="hidden" name="ATList[' + ATCounter + '].AssistiveId" value="0" /></td>' +
            
                '<td style="width:30%"><textarea name="ATList[' + ATCounter + '].ScheduleForUse" style="width:99%"></textarea></td>' +
                '<td style="width:30%"><textarea name="ATList[' + ATCounter + '].StorageLocation" style="width:99%"></textarea></td>' +
                '<td style="width:20px"><input type="button" value="X" class="btnDel" style="float:right;width:20px"" onclick="DeleteRow6(this);" /></td>' +
                '</tr>'
            $(html).appendTo($("#table2 > tbody"));
            ATCounter++;
        };
        $(".btnAdd1").click(addRow);
    });

    function DeleteRow(btnDel,ele) {
        //$('#content').load('../ProtocolSummary/DeleteRowATList/' + btnDelid);
        $.get('../ProtocolSummary/DeleteRowATList?id2=' + btnDel, function(data, status){
            //alert("Status: " + status);
            //alert("#"+btnDel);
            //$("#"+btnDel).remove();
            $(ele).closest("tr").remove();
        });
        //$(btnDel).closest("tr").remove();
    }

    function DeleteRow2(btnDel,ele) {
        $.get('../ProtocolSummary/DeleteRowCGList?id2=' + btnDel, function(data, status){
            //alert("Status: " + status);
            $(ele).closest("tr").remove();
        });
    }

    function DeleteRow3(btnDel,ele) {
        $.get('../ProtocolSummary/DeleteRowFIList?id2=' + btnDel, function(data, status){
            //alert("Status: " + status);
            $(ele).closest("tr").remove();
        });
    }

    function DeleteRow4(btnDel,ele) {
        $.get('../ProtocolSummary/DeleteRowBBIList?id2=' + btnDel, function(data, status){
            //alert("Status: " + status);
            $(ele).closest("tr").remove();
        });
    }

    function DeleteRow5(btnDel,ele) {
        //alert(btnDel,ele);
        $.get('../ProtocolSummary/DeleteRowSignList?id2=' + btnDel, function(data, status){
            //alert("Status: " + status);
            $(ele).closest("tr").remove();
        });
    }

    function DeleteRow6(ele) {
        //alert(btnDel,ele);
        //$.get('../ProtocolSummary/DeleteRowSignList?id2=' + btnDel, function(data, status){
        //alert("Status: " + status);
        $(ele).closest("tr").remove();
        //});
    }

    $(document).ready(function () {
        function addRow() {
            var html = '<tr>' +
                '<td style="width:20%"><textarea name="CGList[' + CGCounter + '].TypeA" style="width:99%"></textarea><input type="hidden" name="CGList[' + CGCounter + '].CommunityId" value="0" /></td>' +
                '<td style="width:auto"><textarea name="CGList[' + CGCounter + '].TypeB" style="width:99%"></textarea></td>' +
                '<td style="width:50px"><input type="button" value="X" class="btnDel" style="float:right;width:20px"" onclick="DeleteRow6(this);" /></td>' +
            '</tr>'
            $(html).appendTo($("#table7 > tbody"));
            CGCounter++;
        };
        $(".btnAdd2").click(addRow);
    });

    $(document).ready(function () {
        function addRow() {
            var html = '<tr>' +
                '<td style="width:20%"><textarea name="FIList[' + FICounter + '].FamilyOne" style="width:99%"></textarea><input type="hidden" name="FIList[' + FICounter + '].FamilyId" value="0" /></td>' +
                '<td style="width:auto"><textarea name="FIList[' + FICounter + '].FamilyTwo" style="width:99%"></textarea></td>' +
                '<td style="width:50px"><input type="button" value="X" class="btnDel" style="float:right;width:20px"" onclick="DeleteRow6(this);" /></td>' +
            '</tr>'
            $(html).appendTo($("#table8 > tbody"));
            FICounter++;
        };
        $(".btnAdd3").click(addRow);
    });

    $(document).ready(function () {
        function addRow() {
            var html = '<tr>' +
                '<td style="width:20%"><textarea name="BBIList[' + BBICounter + '].Acceleration" style="width:99%"></textarea><input type="hidden" name="BBIList[' + BBICounter + '].BasicId" value="0" /></td>' +
                '<td style="width:auto"><textarea name="BBIList[' + BBICounter + '].Strategy" style="width:99%"></textarea></td>' +
                '<td style="width:50px"><input type="button" value="X" class="btnDel" style="float:right;width:20px"" onclick="DeleteRow6(this);" /></td>' +
            '</tr>'
            $(html).appendTo($("#table9 > tbody"));
            BBICounter++;
        };
        $(".btnAdd4").click(addRow);
    });

    function loadData1() {
        window.location.href = "../ProtocolSummary/btnProto_Click";
    }
    function Validate(e)
    {
            var inputValue = e.which;
            if (((inputValue >= 65 && inputValue <= 90) || (inputValue >= 97 && inputValue <= 122) || (inputValue == 32) || (inputValue == 39) || (inputValue == 45) || (inputValue == 8) || (inputValue == 0))) {
            }
            else {
                e.preventDefault();
            }
    }

    $('.namefield').keypress(function (event) {
        var inputValue = event.which;
        if (((inputValue >= 65 && inputValue <= 90) || (inputValue >= 97 && inputValue <= 122) || (inputValue == 32) || (inputValue == 39) || (inputValue == 45) || (inputValue == 8) || (inputValue == 0))) {
        }
        else {
            event.preventDefault();
        }
    });

    $(document).ready(function () {
        function addRow() {
            var html = '<tr style="text-align:center">' +
            '<td style="width:30%"><input type="text" name="SignList[' + SignCounter + '].PrintName" style="width:99%" onkeypress="Validate(event);"/><input type="hidden" name="SignList[' + SignCounter + '].SignatureId" value="0" /></td>' +
                '<td style="width:30%"><input type="text" name="SignList[' + SignCounter + '].Signature" style="width:99%" /></td>' +
                '<td style="width:30%"><input type="text" name="SignList[' + SignCounter + '].Date" style="width:99%" class = "datepicker", ID = "Date[' + SignCounter + ']" /></td>'+
                '<td style="width:20px"><input type="button" value="X" class="btnDel" style="float:right;width:20px"" onclick="DeleteRow6(this);" /></td>' +
                '</tr>'
            $(html).appendTo($("#table12 > tbody"));
            SignCounter++;

            $(".datepicker").datepicker({
                changeMonth: true,
                changeYear: true,
                showAnim: "fadeIn",
                yearRange: 'c-30:c+30',
                minDate: 0,

                fixFocusIE: false,

            }).attr('readonly', 'true');
        };
        $(".btnAdd6").click(addRow);
    });

    //$(document).ready(function () {

        
    //    $(".btnAdd6").click(addRow);

    //    $(".datepicker").datepicker({
    //        changeMonth: true,
    //        changeYear: true,
    //        showAnim: "fadeIn",
    //        yearRange: 'c-30:c+30',
    //        minDate: 0,

    //        fixFocusIE: false,

    //    }).attr('readonly', 'true');


        
    //});

         

</script>

<style type="text/css">
    .ui-datepicker select.ui-datepicker-month, .ui-datepicker select.ui-datepicker-year {
        width: 49% !important;
    }
</style>

<!DOCTYPE html>

<html>
<head id="Head1" runat="server">
    <meta name="viewport" content="width=device-width" />
    <title>Index</title>
    <style type="text/css">
        .auto-style2 {
            width: 492px;
        }

        .auto-style4 {
            width: 122px;
        }

        .auto-style5 {
            width: 121px;
        }

        .lblSpan {
            font-style: italic;
            font-weight: bold;
        }
    </style>
</head>

    <%using (@Ajax.BeginForm("SaveSummary", "ProtocolSummary", FormMethod.Post, new AjaxOptions { UpdateTargetId = "content" }, new { id = "protocolsummary", enctype = "multipart/form-data" }))
      { %>

    <body>
        <table style="width:99%; border:1px double gray;" >
            <tr>
                    <td style="text-align:center;"><label class="lblSpan">Updated:</label> <%=Html.TextBoxFor(m => m.UpdtPageTop, Model.UpdtPageTop, new { @style = "width:20%;" })%></td>
            </tr>
            <tr>
                <td>
<% if (ViewBag.permission == "true")
   {
       if (Session["PlacementStat"] == null || Session["PlacementStat"].ToString() != "I")
       {  %>                  
<input type="button" value="Export" style="float:right;" onclick="loadData1()" />
                     <%}
   } %>
                </td>
                    
            </tr>

        </table>
        <table style="width: 99%; border:1px double gray;">
            <tr>
                <td style="text-align:center;"><label class="lblSpan">Level of Supervision<br />Updated:</label> <%=Html.TextBoxFor(m => m.UpdtLOS, Model.UpdtLOS, new { @style = "width:20%;" })%></td>
            </tr>
            
        </table>
        <table id="table1" style="width: 99%; border:1px double gray;">
            <tr>
                <td><label class="lblSpan">Home (common areas)</label><br />
                <%=Html.TextAreaFor(m => m.HomeCommon, new { value = @Model.HomeCommon, @style = "width:99%;" })%></td>
                <td><label class="lblSpan">School (common area)</label><br />
                <%=Html.TextAreaFor(m => m.SchoolCommon, new { value = @Model.SchoolCommon, @style = "width:99%;" })%></td>
            </tr>
            <tr>
                <td><label class="lblSpan">Home (bedroom)</label><br />
                <%=Html.TextAreaFor(m => m.HomeBedroom, new { value = @Model.HomeBedroom, @style = "width:99%;" })%></td>
                <td><label class="lblSpan">School (bathroom)</label><br />
                <%=Html.TextAreaFor(m => m.SchoolBathroom, new { value = @Model.SchoolBathroom, @style = "width:99%;" })%></td>
            </tr>
            <tr>
                <td><label class="lblSpan">Home (bathroom)</label><br />
                <%=Html.TextAreaFor(m => m.HomeBathroom, new { value = @Model.HomeBathroom, @style = "width:99%;" })%></td>
                <td><label class="lblSpan">School (transitions outside CR)</label><br />
                <%=Html.TextAreaFor(m => m.SchoolOutside, new { value = @Model.SchoolOutside, @style = "width:99%;" })%></td>
            </tr>
            <tr>
                <td><label class="lblSpan">Campus</label><br />
                <%=Html.TextAreaFor(m => m.Campus, new { value = @Model.Campus, @style = "width:99%;" })%></td>
                <td><label class="lblSpan">Pool</label><br />
                <%=Html.TextAreaFor(m => m.Pool, new { value = @Model.Pool, @style = "width:99%;" })%></td>
            </tr>
            <tr>
                <td><label class="lblSpan">Community</label><br />
                <%=Html.TextAreaFor(m => m.Community, new { value = @Model.Community, @style = "width:99%;" })%></td>
                <td><label class="lblSpan">Van</label><br />
                <%=Html.TextAreaFor(m => m.Van, new { value = @Model.Van, @style = "width:99%;" })%></td>
            </tr>
        </table>
        <br />

        
        <table style="width: 99%; border:1px double gray;" id="t1">
            <tr>
                <td colspan="3" style="text-align:center"><label class="lblSpan">Assistive Technology<br />Updated:</label> <%=Html.TextBoxFor(m => m.UpdtATList, Model.UpdtATList, new { @style = "width:20%;" })%></td>
            </tr>
            </table>
        <table id="table2" style="width: 99%; border:1px double gray;">
            <tbody>
            <tr style="text-align:center">
                <td style="width:30%"><b>Type</b></td>
                <td style="width:30%"><b>Schedule for use</b></td>
                <td style="width:30%"><b>Storage Location</b></td>
                <td style="width:20px"></td>
            </tr>
                <% var ATRowId = 0; foreach (var item in Model.ATList)
                   {%>
            <tr style="text-align:center">
                <td style="width:30%"><%=Html.TextAreaFor(m => m.ATList[ATRowId].Type, new { value = @Model.ATList[ATRowId].Type, @style = "width:99%;" })%>
                    <%=Html.HiddenFor(x=>x.ATList[ATRowId].AssistiveId,Model.ATList[ATRowId].AssistiveId) %></td>
                <td style="width:30%"><%=Html.TextAreaFor(m => m.ATList[ATRowId].ScheduleForUse, new { value = @Model.ATList[ATRowId].ScheduleForUse, @style = "width:99%;" })%></td>
                <td style="width:30%"><%=Html.TextAreaFor(m => m.ATList[ATRowId].StorageLocation, new { value = @Model.ATList[ATRowId].StorageLocation, @style = "width:99%;" })%></td>
                <td style="width:20px">
                    <% if (ViewBag.permission == "true")
                       {
                           if (Session["PlacementStat"] == null || Session["PlacementStat"].ToString() != "I")
                           {%>
                    <input type="button" value="X" class="btnDel" style="float:right;width:20px" onclick="DeleteRow(<%= item.AssistiveId%>,this)" />
                    <%}
                       }%>
                </td>
            </tr>
                   <% ATRowId++;
                   }%>
                </tbody>
            <tfoot>
            <tr>
                <td colspan="4">
                    <% if (ViewBag.permission == "true")
                       {
                           if (Session["PlacementStat"] == null || Session["PlacementStat"].ToString() != "I")
                           {%>
                    <input type="button" value="Add Row" class="btnAdd1" style="float:right" />
                    <%}
                       } %>
                </td>
            </tr>
                </tfoot>
        </table>
        <br />
        <table style="width: 99%; border:1px double gray;">
            <tr>
                <td colspan="2" style="text-align:center"><label class="lblSpan">Typical Prompting Hierarchy<br />Updated:</label> <%=Html.TextBoxFor(m => m.UpdtTPH, Model.UpdtTPH, new { @style = "width:20%;" })%></td>
            </tr>
            </table>
        <table id="table3" style="width: 99%; border:1px double gray;">
            <tr>
                <td><label class="lblSpan">Mastered Tasks:</label><br /><%=Html.TextBoxFor(m => m.MasteredTask, Model.MasteredTask, new { @style = "width:95%;" })%></td>
                <td><label class="lblSpan">New/Novel Tasks:</label><br /><%=Html.TextBoxFor(m => m.NewTask, Model.NewTask, new { @style = "width:95%;" })%></td>
            </tr>
        </table>
        <br />
        <table style="width: 99%; border:1px double gray;">
            <tr>
                <td colspan="1" style="text-align:center"><label class="lblSpan">Medical Information<br />Updated:</label> <%=Html.TextBoxFor(m => m.UpdtMedInfo, Model.UpdtMedInfo, new { @style = "width:20%;" })%></td>
            </tr>
            </table>
        <table id="table4" style="width: 99%; border:1px double gray;">
            <tbody>
            <tr>
                <td style="width:20%"><label class="lblSpan">Allergies</label><br />
                <%=Html.TextAreaFor(m => m.Allergies, new { value = @Model.Allergies, @style = "width:99%;" })%></td>
            </tr>
                <tr>
                <td style="width:20%"><label class="lblSpan">Seizure Information</label><br />
                <%=Html.TextAreaFor(m => m.SeizureInfo, new { value = @Model.SeizureInfo, @style = "width:99%;" })%></td>
            </tr>
                <tr>
                <td style="width:20%"><label class="lblSpan">Med Times</label><br />
                <%=Html.TextAreaFor(m => m.MedTimes, new { value = @Model.MedTimes, @style = "width:99%;" })%></td>
            </tr>
                <tr>
                <td style="width:20%"><label class="lblSpan">How student takes meds</label><br />
                <%=Html.TextAreaFor(m => m.TakeMed, new { value = @Model.TakeMed, @style = "width:99%;" })%></td>
            </tr>
                <tr>
                <td style="width:20%"><label class="lblSpan">Other:</label><br />
                <%=Html.TextAreaFor(m => m.OtherMedical, new { value = @Model.OtherMedical, @style = "width:99%;" })%></td>
            </tr>
                </tbody>
        </table>
        <br />

        <table style="width: 99%; border:1px double gray;">
            <tr>
                <td colspan="2" style="text-align:center"><label class="lblSpan">Behavior during Appointments Information<br />Updated:</label> <%=Html.TextBoxFor(m => m.UpdtBehInfo, Model.UpdtBehInfo, new { @style = "width:20%;" })%></td>
            </tr>
            </table>
        <table id="table5" style="width: 99%; border:1px double gray;">
            <tbody>
            <tr>
                <td style="width:20%"><label class="lblSpan">Doctor visits</label><br />
                <%=Html.TextAreaFor(m => m.DoctorVisit, new { value = @Model.DoctorVisit, @style = "width:99%;" })%></td>
            </tr>
                <tr>
                <td style="width:20%"><label class="lblSpan">Dental</label><br />
                <%=Html.TextAreaFor(m => m.Dental, new { value = @Model.Dental, @style = "width:99%;" })%></td>
            </tr>
                <tr>
                <td style="width:20%"><label class="lblSpan">Bloodwork</label><br />
                <%=Html.TextAreaFor(m => m.BloodWork, new { value = @Model.BloodWork, @style = "width:99%;" })%></td>
            </tr>
                <tr>
                <td style="width:20%"><label class="lblSpan">Haircuts</label><br />
                <%=Html.TextAreaFor(m => m.HairCuts, new { value = @Model.HairCuts, @style = "width:99%;" })%></td>
            </tr>
                <tr>
                <td style="width:20%"><label class="lblSpan">Other:</label><br />
                <%=Html.TextAreaFor(m => m.OtherBehave, new { value = @Model.OtherBehave, @style = "width:99%;" })%></td>
            </tr>
                </tbody>
        </table>
        <br />

        <table style="width: 99%; border:1px double gray;">
            <tr>
                <td colspan="5" style="text-align:center"><label class="lblSpan">Personal Care<br />Updated:</label> <%=Html.TextBoxFor(m => m.UpdtPerCare, Model.UpdtPerCare, new { @style = "width:20%;" })%></td>
            </tr>
            </table>
        <table id="table6" style="width: 99%; border:1px double gray;">
            <tbody>
            <tr style="text-align:center">
                <td><label class="lblSpan">Activity</label></td>
                <td><label class="lblSpan">General Info</label></td>
                <td><label class="lblSpan">Able to:</label></td>
                <td><label class="lblSpan">Needs help with:</label></td>
                <td><label class="lblSpan">IEP related goals:</label></td>
            </tr>
            <tr>
                <td><label class="lblSpan">Eating</label></td>
                <td><%=Html.TextAreaFor(m => m.EatingGeneral, new { value = @Model.EatingGeneral, @style = "width:99%;" })%></td>
                <td><%=Html.TextAreaFor(m => m.EatingAble, new { value = @Model.EatingAble, @style = "width:99%;" })%></td>
                <td><%=Html.TextAreaFor(m => m.EatingNeed, new { value = @Model.EatingNeed, @style = "width:99%;" })%></td>
                <td><%=Html.TextAreaFor(m => m.EatingIep, new { value = @Model.EatingIep, @style = "width:99%;" })%></td>
            </tr>
                <tr>
                <td><label class="lblSpan">Toileting</label></td>
                <td><%=Html.TextAreaFor(m => m.ToiletingGeneral, new { value = @Model.ToiletingGeneral, @style = "width:99%;" })%></td>
                <td><%=Html.TextAreaFor(m => m.ToiletingAble, new { value = @Model.ToiletingAble, @style = "width:99%;" })%></td>
                <td><%=Html.TextAreaFor(m => m.ToiletingNeed, new { value = @Model.ToiletingNeed, @style = "width:99%;" })%></td>
                <td><%=Html.TextAreaFor(m => m.ToiletingIep, new { value = @Model.ToiletingIep, @style = "width:99%;" })%></td>
            </tr>
                <tr>
                <td><label class="lblSpan">Brushing Teeth</label></td>
                <td><%=Html.TextAreaFor(m => m.BrushingGeneral, new { value = @Model.BrushingGeneral, @style = "width:99%;" })%></td>
                <td><%=Html.TextAreaFor(m => m.BrushingAble, new { value = @Model.BrushingAble, @style = "width:99%;" })%></td>
                <td><%=Html.TextAreaFor(m => m.BrushingNeed, new { value = @Model.BrushingNeed, @style = "width:99%;" })%></td>
                <td><%=Html.TextAreaFor(m => m.BrushingIep, new { value = @Model.BrushingIep, @style = "width:99%;" })%></td>
            </tr>
                <tr>
                <td><label class="lblSpan">Hand washing</label></td>
                <td><%=Html.TextAreaFor(m => m.HandGeneral, new { value = @Model.HandGeneral, @style = "width:99%;" })%></td>
                <td><%=Html.TextAreaFor(m => m.HandAble, new { value = @Model.HandAble, @style = "width:99%;" })%></td>
                <td><%=Html.TextAreaFor(m => m.HandNeed, new { value = @Model.HandNeed, @style = "width:99%;" })%></td>
                <td><%=Html.TextAreaFor(m => m.HandIep, new { value = @Model.HandIep, @style = "width:99%;" })%></td>
            </tr>
                <tr>
                <td><label class="lblSpan">Dressing</label></td>
                <td><%=Html.TextAreaFor(m => m.DressGeneral, new { value = @Model.DressGeneral, @style = "width:99%;" })%></td>
                <td><%=Html.TextAreaFor(m => m.DressAble, new { value = @Model.DressAble, @style = "width:99%;" })%></td>
                <td><%=Html.TextAreaFor(m => m.DressNeed, new { value = @Model.DressNeed, @style = "width:99%;" })%></td>
                <td><%=Html.TextAreaFor(m => m.DressIep, new { value = @Model.DressIep, @style = "width:99%;" })%></td>
            </tr>
                <tr>
                <td><label class="lblSpan">Showering</label></td>
                <td><%=Html.TextAreaFor(m => m.ShowerGeneral, new { value = @Model.ShowerGeneral, @style = "width:99%;" })%></td>
                <td><%=Html.TextAreaFor(m => m.ShowerAble, new { value = @Model.ShowerAble, @style = "width:99%;" })%></td>
                <td><%=Html.TextAreaFor(m => m.ShowerNeed, new { value = @Model.ShowerNeed, @style = "width:99%;" })%></td>
                <td><%=Html.TextAreaFor(m => m.ShowerIep, new { value = @Model.ShowerIep, @style = "width:99%;" })%></td>
            </tr>
                <tr>
                    <td><label class="lblSpan">Bedtime Routine</label></td>
                    <td colspan="4"><%=Html.TextAreaFor(m => m.BedTime, new { value = @Model.BedTime, @style = "width:99%;" })%></td>
            </tr>
                </tbody>
        </table>
        <br />

        <table style="width: 99%; border:1px double gray;">
            <tr>
                <td colspan="2" style="text-align:center"><label class="lblSpan">Community Guidelines<br />Updated:</label> <%=Html.TextBoxFor(m => m.UpdtCGList, Model.UpdtCGList, new { @style = "width:20%;" })%></td>
            </tr>
            </table>
        <table id="table7" style="width: 99%; border:1px double gray;">
            <tbody>
                <% var CGRowId = 0; foreach (var item in Model.CGList)
                   { %>
            <tr>
                <td style="width:20%"><%=Html.TextAreaFor(m => m.CGList[CGRowId].TypeA, new { value = @Model.CGList[CGRowId].TypeA, @style = "width:99%;" })%>
                <%=Html.HiddenFor(x=>x.CGList[CGRowId].CommunityId,Model.CGList[CGRowId].CommunityId) %></td>
                <td style="width:auto"><%=Html.TextAreaFor(m => m.CGList[CGRowId].TypeB, new { value = @Model.CGList[CGRowId].TypeB, @style = "width:99%;" })%></td>
                <td style="width:50px">
                    <% if (ViewBag.permission == "true")
                       {
                           if (Session["PlacementStat"] == null || Session["PlacementStat"].ToString() != "I")
                           {
                           
                           %>
                    <input type="button" value="X" class="btnDel" style="float:right;width:20px"" onclick="DeleteRow2(<%= item.CommunityId%>,this);" />
                    <%}
                       }
                       
                        %>
                </td>
            </tr>
                    
                <% CGRowId++;
                   } %>
                </tbody>
            <tfoot>
            <tr>
                <td colspan="3"">
                    <% if (ViewBag.permission == "true")
                       {
                           if (Session["PlacementStat"] == null || Session["PlacementStat"].ToString() != "I")
                           {%>
                    <input type="button" value="Add Row" class="btnAdd2" style="float:right" />
                    <%}
                       } %>
                </td>
            </tr>
                </tfoot>
        </table>
        <br />

        <table style="width: 99%; border:1px double gray;">
            <tr>
                <td colspan="2" style="text-align:center"><label class="lblSpan">Family Information<br />Updated:</label> <%=Html.TextBoxFor(m => m.UpdtFIList, Model.UpdtFIList, new { @style = "width:20%;" })%></td>
            </tr>
            </table>
        <table id="table8" style="width: 99%; border:1px double gray;">
            <tbody>
                <% var FIRowId = 0; foreach (var item in Model.FIList)
                   {%>
            <tr>
                <td style="width:20%"><%=Html.TextAreaFor(m => m.FIList[FIRowId].FamilyOne, new { value = @Model.FIList[FIRowId].FamilyOne, @style = "width:99%;" })%>
                    <%=Html.HiddenFor(x=>x.FIList[FIRowId].FamilyId,Model.FIList[FIRowId].FamilyId) %>
                </td>
                <td style="width:auto"><%=Html.TextAreaFor(m => m.FIList[FIRowId].FamilyTwo, new { value = @Model.FIList[FIRowId].FamilyTwo, @style = "width:99%;" })%></td>
                <td style="width:50px">
                    <% if (ViewBag.permission == "true")
                       {
                           if (Session["PlacementStat"] == null || Session["PlacementStat"].ToString() != "I")
                           { 
                           %>
                    <input type="button" value="X" class="btnDel" style="float:right;width:20px"" onclick="DeleteRow3(<%= item.FamilyId%>,this);" />
                    <%}
                       }
                        %>
                </td>
            </tr>
                
                    <% FIRowId++;
                   }%>
                </tbody>
            <tfoot>
            <tr>
                <td colspan="3"">
                    <% if (ViewBag.permission == "true")
                       {
                           if (Session["PlacementStat"] == null || Session["PlacementStat"].ToString() != "I")
                           { 
                           %>
                    <input type="button" value="Add Row" class="btnAdd3" style="float:right" />
                    <%}
                       }
                        %>
                </td>
            </tr>
                </tfoot>
        </table>
        <br />

        <table style="width: 99%; border:1px double gray;">
            <tr>
                <td colspan="2" style="text-align:center"><label class="lblSpan">Basic Behavioral Information<br />Updated:</label> <%=Html.TextBoxFor(m => m.UpdtBBIList, Model.UpdtBBIList, new { @style = "width:20%;" })%></td>
                
            </tr>
            </table>
        <table id="table9" style="width: 99%; border:1px double gray;">
            <tbody>
            <tr style="text-align:center">
                <td style="width:20%"><b>Behavior (for acceleration & deceleration)</b></td>
                <td style="width:auto"><b>Strategy/Response</b></td>
                <td style="width:50px"></td>
            </tr>
                <% var BBIRowId = 0; foreach (var item in Model.BBIList)
                   { %>
            <tr>

                <td style="width:20%"><%=Html.TextAreaFor(m => m.BBIList[BBIRowId].Acceleration, new { value = @Model.BBIList[BBIRowId].Acceleration, @style = "width:99%;" })%>
                    <%=Html.HiddenFor(x=>x.BBIList[BBIRowId].BasicId,Model.BBIList[BBIRowId].BasicId) %>
                </td>
                <td style="width:auto"><%=Html.TextAreaFor(m => m.BBIList[BBIRowId].Strategy, new { value = @Model.BBIList[BBIRowId].Strategy, @style = "width:99%;" })%></td>
                <td style="width:50px">
                    <% if (ViewBag.permission == "true")
                       {
                           if (Session["PlacementStat"] == null || Session["PlacementStat"].ToString() != "I")
                           {    %>
                    <input type="button" value="X" class="btnDel" style="float:right;width:20px"" onclick="DeleteRow4(<%= item.BasicId%>,this);" /> 
                    <%}
                       } %>
                </td>
            </tr>
                   <% BBIRowId++;
                   } %>
                </tbody>
            <tfoot>
            <tr>
                <td colspan="3">
                    <% if (ViewBag.permission == "true")
                       {
                           if (Session["PlacementStat"] == null || Session["PlacementStat"].ToString() != "I")
                           {  %>
                    <input type="button" value="Add Row" class="btnAdd4" style="float:right" />
                    <%}
                       }%>
                </td>
            </tr>
                </tfoot>
        </table>
        <br />
        <table style="width: 99%; border:1px double gray;">
            <tr>
                <td colspan="4" style="text-align:center"><label class="lblSpan">Typical Routines<br />Updated:</label> <%=Html.TextBoxFor(m => m.UpdtTypRoutines, Model.UpdtTypRoutines, new { @style = "width:20%;" })%></td>
            </tr>
            </table>
        <table id="table10" style="width: 99%; border:1px double gray;">
            <tr style="text-align:center">
                <td></td>
                <td><label class="lblSpan">Morning Routine</label></td>
                <td></td>
                <td><label class="lblSpan">Afternoon/Evening Routine</label></td>
            </tr>
            <tr>
                <td style="text-align:center" class="auto-style4"><label class="lblSpan">7:00</label></td>
                <td class="auto-style2"><%=Html.TextBoxFor(m => m.Morning7, Model.Morning7, new { @style = "width:90%;" })%></td>
                <td style="text-align:center" class="auto-style5"><label class="lblSpan">3:30</label></td>
                <td><%=Html.TextBoxFor(m => m.Noon330, Model.Noon330, new { @style = "width:90%;" })%></td>
            </tr>
            <tr>
                <td style="text-align:center" class="auto-style4"><label class="lblSpan">7:15</label></td>
                <td class="auto-style2"><%=Html.TextBoxFor(m => m.Morning715, Model.Morning715, new { @style = "width:90%;" })%></td>
                <td style="text-align:center" class="auto-style5"><label class="lblSpan">4:00</label></td>
                <td><%=Html.TextBoxFor(m => m.Noon400, Model.Noon400, new { @style = "width:90%;" })%></td>
            </tr>
            <tr>
                <td style="text-align:center" class="auto-style4"><label class="lblSpan">7:30</label></td>
                <td class="auto-style2"><%=Html.TextBoxFor(m => m.Morning730, Model.Morning730, new { @style = "width:90%;" })%></td>
                <td style="text-align:center" class="auto-style5"><label class="lblSpan">4:30</label></td>
                <td><%=Html.TextBoxFor(m => m.Noon430, Model.Noon430, new { @style = "width:90%;" })%></td>
            </tr>
            <tr>
                <td style="text-align:center" class="auto-style4"><label class="lblSpan">7:45</label></td>
                <td class="auto-style2"><%=Html.TextBoxFor(m => m.Morning745, Model.Morning745, new { @style = "width:90%;" })%></td>
                <td style="text-align:center" class="auto-style5"><label class="lblSpan">5:00</label></td>
                <td><%=Html.TextBoxFor(m => m.Noon500, Model.Noon500, new { @style = "width:90%;" })%></td>
            </tr>
            <tr>
                <td style="text-align:center" class="auto-style4"><label class="lblSpan">8:00</label></td>
                <td class="auto-style2"><%=Html.TextBoxFor(m => m.Morning800, Model.Morning800, new { @style = "width:90%;" })%></td>
                <td style="text-align:center" class="auto-style5"><label class="lblSpan">5:30</label></td>
                <td><%=Html.TextBoxFor(m => m.Noon530, Model.Noon530, new { @style = "width:90%;" })%></td>
            </tr>
            <tr>
                <td style="text-align:center" class="auto-style4"><label class="lblSpan">8:15</label></td>
                <td class="auto-style2"><%=Html.TextBoxFor(m => m.Morning815, Model.Morning815, new { @style = "width:90%;" })%></td>
                <td style="text-align:center" class="auto-style5"><label class="lblSpan">6:00</label></td>
                <td><%=Html.TextBoxFor(m => m.Noon600, Model.Noon600, new { @style = "width:90%;" })%></td>
            </tr>
            <tr>
                <td style="text-align:center" class="auto-style4"><label class="lblSpan">8:30</label></td>
                <td class="auto-style2"><%=Html.TextBoxFor(m => m.Morning830, Model.Morning830, new { @style = "width:90%;" })%></td>
                <td style="text-align:center" class="auto-style5"><label class="lblSpan">6:30</label></td>
                <td><%=Html.TextBoxFor(m => m.Noon630, Model.Noon630, new { @style = "width:90%;" })%></td>
            </tr>
            <tr>
                <td style="text-align:center" class="auto-style4"><label class="lblSpan">8:45</label></td>
                <td class="auto-style2"><%=Html.TextBoxFor(m => m.Morning845, Model.Morning845, new { @style = "width:90%;" })%></td>
                <td style="text-align:center" class="auto-style5"><label class="lblSpan">7:00</label></td>
                <td><%=Html.TextBoxFor(m => m.Noon700, Model.Noon700, new { @style = "width:90%;" })%></td>
            </tr>

            <tr>
                <td style="text-align:center" class="auto-style4"><label class="lblSpan">9:00</label></td>
                <td class="auto-style2"><%=Html.TextBoxFor(m => m.Morning900, Model.Morning900, new { @style = "width:90%;" })%></td>
                <td style="text-align:center" class="auto-style5"><label class="lblSpan">7:30</label></td>
                <td><%=Html.TextBoxFor(m => m.Noon730, Model.Noon730, new { @style = "width:90%;" })%></td>
            </tr>
            <tr>
                <td colspan="2" style="text-align:center"><label class="lblSpan">Preferred Leisure Activities</label></td>
                <td style="text-align:center" class="auto-style5"><label class="lblSpan">8:00</label></td>
                <td><%=Html.TextBoxFor(m => m.Noon800, Model.Noon800, new { @style = "width:90%;" })%></td>
            </tr>
            <tr>
                <td colspan="2" rowspan="6"><%=Html.TextAreaFor(m => m.Leisure, new { value = @Model.Leisure, @style = "width:100%;" })%></td>
                <td style="text-align:center" class="auto-style5"><label class="lblSpan">8:30</label></td>
                <td><%=Html.TextBoxFor(m => m.Noon830, Model.Noon830, new { @style = "width:90%;" })%></td>
            </tr>
            <tr>
                <td style="text-align:center" class="auto-style5"><label class="lblSpan">9:00</label></td>
                <td><%=Html.TextBoxFor(m => m.Noon900, Model.Noon900, new { @style = "width:90%;" })%></td>
            </tr>
            <tr>
                <td style="text-align:center" class="auto-style5"><label class="lblSpan">9:30</label></td>
                <td><%=Html.TextBoxFor(m => m.Noon930, Model.Noon930, new { @style = "width:90%;" })%></td>
            </tr>
            <tr>
                <td style="text-align:center" class="auto-style5"><label class="lblSpan">10:00</label></td>
                <td><%=Html.TextBoxFor(m => m.Noon100, Model.Noon100, new { @style = "width:90%;" })%></td>
            </tr>
            <tr>
                <td style="text-align:center" class="auto-style5"><label class="lblSpan">10:00-11:00</label></td>
                <td><%=Html.TextBoxFor(m => m.Noon10to11, Model.Noon10to11, new { @style = "width:90%;" })%></td>
            </tr>
            <tr>
                <td style="text-align:center" class="auto-style5"><label class="lblSpan">11:00-7:00</label></td>
                <td><%=Html.TextBoxFor(m => m.Noon11to7, Model.Noon11to7, new { @style = "width:90%;" })%></td>
            </tr>

        </table>
        <br />
        <table style="width: 99%;">
            <tr>
                <td style="text-align:center"><label class="lblSpan">PROTOCOL SUMMARY RESPONSIBILITIES</label></td>
            </tr>
            </table>
        <table style="width: 99%; border:1px double gray;">
            <tr>
                <td><p>Prior to working with a student, you must first be trained on the person’s protocol summary. This document is updated throughout the month on an “as needed” basis, and re-issued monthly with a new “updated” date at the top of the document.  The most recent changes are highlighted. When a change has occurred, or the document has been reissued, this will be reflected on the “updated date” section in the upper right corner of the document.</p>
<p>Please REVIEW protocol changes before working with this student.  If you have any questions please find someone in a supervisory position (i.e., a BA, PMs. Teachers, BSS, RSS, or Case Manager).  Once you have reviewed this new protocol, you must sign this sheet below.</p>
Please note:<br/>
<ol type="1">
<li>Each time the student’s protocol summary is updated or reissued, all staff working with a student need to be retrained on the protocol summary. Each time you are trained, you and the trainer will need to sign the training log below.</li>
<li>Contact a supervisor for clarifications on any questions you have while working with a student.</li>
<li>Never transfer LOS to another staff member without assuring the person’s explicit agreement to assume LOS.</li>
</ol></td>
            </tr>
        </table>
        <br />
        <table id="table11" style="width: 99%; border:1px double gray; text-align:center">
            
            <tr>
                <td style="width:30%"><label class="lblSpan">PRINT NAME</label></td>
                <td style="width:30%"><label class="lblSpan">SIGNATURE</label></td>
                <td style="width:30%"><label class="lblSpan">DATE</label></td>
                <td style="width:20px"></td>
            </tr>
            </table>
        <table id="table12" style="width: 99%; border:1px double gray; text-align:center">
            <tbody>

            <% var SignRowId = 0; foreach (var item in Model.SignList)
               { %>
            <tr>
                <td style="width:30%"><%=Html.TextBoxFor(m => m.SignList[SignRowId].PrintName, new {@class="namefield",value = @Model.SignList[SignRowId].PrintName, @style = "width:99%;" })%>
                    <%=Html.HiddenFor(x=>x.SignList[SignRowId].SignatureId, Model.SignList[SignRowId].SignatureId) %></td>
                <td style="width:30%"><%=Html.TextBoxFor(m => m.SignList[SignRowId].Signature, new { value = @Model.SignList[SignRowId].Signature, @style = "width:99%;" })%></td>
                <td style="width:30%"><%=Html.TextBoxFor(m => m.SignList[SignRowId].Date, new { @class = "validate[required] datepicker", @style = "width:99%;" })%></td>
                <td style="width:20px">
                    <% if (ViewBag.permission == "true")
                       {
                           if (Session["PlacementStat"] == null || Session["PlacementStat"].ToString() != "I")
                           {    %>
                    <input type="button" value="X" class="btnDel" style="float:right;width:20px"" onclick="DeleteRow5(<%= item.SignatureId%>,this);" />
                    <%}
                       }%>
                </td>
            </tr>
            <% SignRowId++;
               } %>
                </tbody>
            <tfoot>
                <tr>
                <td colspan="4">
                    <% if (ViewBag.permission == "true")
                       {
                           if (Session["PlacementStat"] == null || Session["PlacementStat"].ToString() != "I")
                           { 
                            %>
                    <input type="button" value="Add Row" class="btnAdd6" style="float:right" />
                    <%}
                       }%>
                </td>
            </tr>
            </tfoot>
        </table>
        <br />
        <%--<table>
            <tr>
                <td style="align-items:center">

                    <input id="btnSubmit" type="submit" value="Save" style="margin-left: 5%" />
                </td>
            </tr>
        </table>--%>
        <div style="width:99%">
            <% if (ViewBag.permission == "true")
               {
                   if (Session["PlacementStat"] == null || Session["PlacementStat"].ToString() != "I")
                   {%>
    <%if (ViewBag.status == "update")
      { %>
    <input id="btnSubmit1" type="submit" value="Update" />
    <%}
      else
      { %>
    <input id="btnSubmit2" type="submit" value="Save" />
    <%}
                   }%>
            <%} %>
</div>
    </body>

    <%} %>

</html>
