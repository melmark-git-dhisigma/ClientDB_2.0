<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ClientDB.Models.AddEventModel>" %>

<script src="../Documents/JS/jquery.validationEngine-en.js"></script>
<script src="../Documents/JS/jquery.validationEngine.js"></script>


<link href="../../Documents/CSS/jquery-ui.css" rel="stylesheet" />
<script src="../../Documents/JS/jquery-ui-1.8.24.js"></script>

<script type="text/javascript">
    $(document).ready(function () {

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
    jQuery("#addEvents").validationEngine();

    loadClientStaticDetails();
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
<%using (@Ajax.BeginForm("SaveEvents", "Event", FormMethod.Post, new AjaxOptions { UpdateTargetId = "content" }, new { id = "addEvents", enctype = "multipart/form-data" }))
  { %>
<table style="width: 100%">
    <%=Html.HiddenFor(m=>m.Id,Model.Id) %>

    <tr>
        <td colspan="2">
            <label class="lblSpan">Event Name</label><span class="span-align">*</span><br />
            <%=Html.TextBoxFor(m => m.EventName, Model.EventName, new { @class = "validate[required]",maxlength=50 })%></td>
        <td colspan="2">
            <label class="lblSpan">Event Date</label><span class="span-align">*</span><br />
            <%=Html.TextBoxFor(m => m.EventDate, Model.EventDate, new { @class = "validate[required] datepicker", ID = "EventDate" })%></td>
        <td rowspan="4">
             <label class="lblSpan">Notes</label><span class="nospan-align"></span><br />
            <%=Html.TextAreaFor(m => m.Note,new {value=@Model.Note,  @class="sd", @rows="3",@cols="30", style="height:165px;width:340px;"})%>
            
        </td>
    </tr>
      
    <tr>
        <td colspan="2">
            <label class="lblSpan">Expires On</label><span class="nospan-align">*</span><br />
            <%=Html.TextBoxFor(m => m.ExpiredOnDate, Model.ExpiredOnDate, new { @class = "datepicker", ID = "expiredOnDate" })%></td>
        <td colspan="2">
            <label class="lblSpan">User</label><span class="span-align"></span><br />
            <%=Html.TextBoxFor(m => m.UserName, Model.UserName, new { })%></td>
    </tr>

    <tr>
        <td colspan="2">
            <label class="lblSpan">Event Type</label><span class="span-align">*</span><br />
            <%=Html.DropDownListFor(m => m.EventTypes, Model.EventTypeList, new { @class = "validate[required]" })%>
        <td colspan="2">
            <%--<%=Html.DropDownListFor(m => m.EventType, Model.EventTypeList, new { @class = "validate[required] ", ID = "ddlEventType" })%>--%>
            <label class="lblSpan">Event Status</label><span class="span-align">*</span><br />
            
             <% if (Model.Id > 0)
                {
                    if (!Model.IsSystemEvent)
                    { %>
                    
                 <%=Html.DropDownListFor(m => m.EventStatus, Model.EventStatusList, new { @class = "validate[required] ", ID = "ddlEventStatus" })%>
                
                  <%}
                    else {%>
                    
                 <select>
                     <option>
                         N/A
                     </option>
                 </select>
                
               <%}

                }
                else { %>
                    
                 <%=Html.DropDownListFor(m => m.EventStatus, Model.EventStatusList, new { @class = "validate[required] ", ID = "ddlEventStatus" })%>
                
               <% }
                %>

        </td>
    </tr>
    <tr>
         <td colspan="2">
            <label class="lblSpan">Contact</label><span class="nospan-align">*</span><br />
            <%=Html.DropDownListFor(m => m.Contact, Model.Contactlist, new { })%>
         
             <td colspan="2">
           
        </td>
    </tr>

</table>
<div class="btnDoneStyle">


    <% if (ViewBag.permission == "true")
       {
           if (Session["PlacementStat"].ToString() != "I")
           {
           if (Model.Id > 0)
           {
               if (!Model.IsSystemEvent)
               {%>
            <input id="btnUpdateEvents" type="submit" value="Update" onclick="" />
            <%}
           }
           else
           { %>
    <input id="btnEvents" type="submit" value="Save" onclick="" />
    <%}
      }} %>
</div>

<%} %>