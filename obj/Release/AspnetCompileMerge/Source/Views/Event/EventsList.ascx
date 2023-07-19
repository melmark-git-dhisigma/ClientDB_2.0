<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ClientDB.Models.EventsModel>" %>

<script type="text/javascript">
    $(document).ready(function () {

        $('#AddNewEvents').load('../Event/AddEvents/0');

    });


    $("#tblContacList tr:odd").css("background-color", "#F3F3F3");
    // $('#imagePanel').load('/Contact/ImageUploadPanel');

    function loadFunction(id, type) {

        if (type == "Edit") {
            //alert(id + " " + type)
            $('#AddNewEvents').load('../Event/AddEvents/' + id);
        }
        if (type == "Delete") {
            //alert(id + " " + type)
            if (confirm('Are you sure you want to delete this event?')) {
                $('#content').load('../Event/DeleteEvents/' + id);
            } else {
                return false;
            }

        }
    }

</script>


<div id="partialMainArea">
    <% if (TempData["notice"] != null)
       { %>
    <p style="width: 100%; color: red; font-size: 14px; font-weight: bold"><%= Html.Encode(TempData["notice"]) %></p>
    <% } %>
    <div id="AddNewEvents">
    </div>
    <div id="Sample" style="float: left; width: 100%">


        <table id="tblContacList" style="width: 100%">
            <tr class="HeaderStyle">
                <th class="tdLabel">Event Name</th>
                <th class="tdLabel">Event Type</th>
                <th class="tdLabel">Event Status</th>
                <th class="tdLabel">Expires On</th>
                <th class="tdLabel">Edit</th>
                <th class="tdLabel">Delete</th>
            </tr>
            <%foreach (var item in Model.listEvents)
              {
            %>
            <%
                
                
            %>


            <tr class="RowStyle">
                <td style="width: 20%"><%= item.EventName %></td>
                <td style="width: 20%"><%= item.EventType %></td>
                <td style="width: 20%"><%= item.EventStatus %></td>
                <td style="width: 20%"><%= (item.ExpiredOn != null) ? item.ExpiredOn.Value.ToString("MM/dd/yyyy") : "n/a" %></td>
                <td style="width: 10%">
                    <img src="../../Images/edit.PNG" onclick="loadFunction(<%= item.EventId %>,'Edit');" style="cursor: pointer;" /></td>
                <td style="width: 10%">
                    <%if (ViewBag.permission == "true")
                      {
                          if (Session["PlacementStat"].ToString() != "I")
                          {  
                    %>
                    <img src="../../Images/delete.PNG" onclick="loadFunction(<%= item.EventId%>,'Delete');" style="cursor: pointer;" />
                    <%    }
                      } %>
                </td>
            </tr>
            <%
              } %>
        </table>
        <div class="pager"><% Html.RenderPartial("Pager", Model.pageModel); %></div>
    </div>


</div>


