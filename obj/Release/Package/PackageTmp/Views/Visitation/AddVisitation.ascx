<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ClientDB.Models.AddVisitaionModel>" %>
<script src="../Documents/JS/jquery.validationEngine-en.js"></script>
<script src="../Documents/JS/jquery.validationEngine.js"></script>


<link href="../../Documents/CSS/jquery-ui.css" rel="stylesheet" />
<script src="../../Documents/JS/jquery-ui-1.8.24.js"></script>
<script type="text/javascript">
    $(document).ready(function () {
        var date =
        // $(".datepicker").datepicker();
        $(".datepicker").datepicker({
            changeMonth: true,
            changeYear: true,
            showAnim: "fadeIn",
            yearRange: 'c-30:c+30',
            minDate: 0,

            /* fix buggy IE focus functionality */
            fixFocusIE: false,


        }).attr('readonly', 'true');



    });

    function DownloadForm()
    {
        var DownloadId = $("#FormSelect").val();
        if (DownloadId != null && DownloadId != "") {
            window.open('../Forms/viewDoc?id=' + DownloadId + '   ', '_blank');
        }
        else {
            alert("Please Select a Form..")
        }
    }

    jQuery("#addVisitation").validationEngine();
</script>
<style type="text/css">
    .ui-datepicker select.ui-datepicker-month, .ui-datepicker select.ui-datepicker-year {
         width: 49% !important;
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

    .lblSpan {
        font-style: italic;
        font-weight: bold;
    }
    
</style>
<%using (@Ajax.BeginForm("SaveVisitation", "Visitation", FormMethod.Post, new AjaxOptions { UpdateTargetId = "content" }, new { id = "addVisitation", enctype = "multipart/form-data" }))
  { %>

<table style="width: 70%">
    <%=Html.HiddenFor(m=>m.Id,Model.Id) %>
    <tr>
        <td colspan="2">
        <label class="lblSpan">Visitation Name</label><span class="span-align">*</span><br />
        <%=Html.TextBoxFor(m => m.EventName, Model.EventName, new { @class = "validate[required]",maxlength=50 })%></td>
        <td colspan="2">
        <label class="lblSpan">Expires On </label><span class="span-align">*</span><br />
        <%=Html.TextBoxFor(m => m.ExpiredOnDate, Model.ExpiredOnDate, new { @class = "validate[required] datepicker", ID = "expiredOnDate" })%></td>
    </tr>

    <tr>
        <td colspan="2">
        <label class="lblSpan">Visitation Type </label><span class="span-align">*</span><br />
        <%=Html.DropDownListFor(m => m.EventType, Model.EventTypeList, new { @class = "validate[required] ", ID = "ddlEventType" })%></td>
        <td colspan="2">
        <label class="lblSpan">Visitation Status </label><span class="span-align">*</span><br />
        <%=Html.DropDownListFor(m => m.EventStatus, Model.EventStatusList, new { @class = "validate[required] ", ID = "ddlEventStatus" })%></td>
    </tr>
    <tr>
        <td colspan="2">
        <label class="lblSpan">Date requested </label><span class="span-align"></span><br />
        <%=Html.TextBoxFor(m => m._DateRequested, Model._DateRequested, new { @class = "datepicker" })%></td>
        <td colspan="2">
        <label class="lblSpan">Effective Date</label><span class="span-align"></span><br />
        <%=Html.TextBoxFor(m => m._EffectiveDate, Model._EffectiveDate, new { @class = "datepicker" })%></td>
    </tr>
    <tr>
        <td colspan="2">
        <label class="lblSpan">Form</label><span class="nospan-align">*</span><br />
        <%=Html.DropDownListFor(m => m.Form, Model.FormList, new { ID="FormSelect" })%>
            <a href="#" onclick="DownloadForm()" >View</a>
        </td>
        <td colspan="2">
            <label class="lblSpan">Notes</label><span class="nospan-align"></span><br />
            <%=Html.TextAreaFor(m => m.Note,new {value=@Model.Note,  @class="sd", @rows="3",@cols="30"})%>
        </td>
    </tr>

</table>
<div class="btnDoneStyle">
    <% 
      if (ViewBag.permission == "true")
      {
          if (Model.Id > 0)
          { %>
    <input id="btnUpdateVisitation" type="submit" value="Update" onclick="" />
    <%}
         else
         { %>
    <input id="btnAddVisitation" type="submit" value="Save" onclick="" />
    <%}
        } %>
</div>
<div style="float: left; padding-left: 5%; padding-top: 1%">

    <%--  <input id="btnCancel" type="button" value="Cancel" onclick="" />--%>
</div>

<%} %>
<script>loadClientStaticDetails();</script>