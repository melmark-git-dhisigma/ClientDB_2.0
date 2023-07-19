<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ClientDB.Models.VisitationModel>" %>

<script type="text/javascript">
    $(document).ready(function () {

        $('#AddNewVisitation').load('../Visitation/AddVisitation/0');

    });
    $("#tblContacList tr:odd").css("background-color", "#F3F3F3");
    // $('#imagePanel').load('/Contact/ImageUploadPanel');

    function loadFunction(id, type) {

        if (type == "Edit") {
            //alert(id + " " + type)
            document.getElementById('AddNewVisitation').style.display = "block";
            $('#AddNewVisitation').load('../Visitation/AddVisitation/' + id);
        }
        if (type == "Delete") {
            //alert(id + " " + type)
            if (confirm('Are you sure you want to delete this visitation?')) {
                $('#content').load('../Visitation/DeleteVisitationDetails/' + id);
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
    <div id="AddNewVisitation">
    </div>
    <div style="float: left; width: 100%">
        <table id="tblContacList" style="width: 100%">
            <tr class="HeaderStyle">
                <th class="tdLabel">Date</th>
                <th class="tdLabel">Name</th>
                <th class="tdLabel">Type</th>
                <th class="tdLabel">Status</th>
                <th class="tdLabel">Expires On</th>
                <th class="tdLabel">Edit</th>
                <th class="tdLabel">Delete</th>
            </tr>
            <%foreach (var item in Model.listVisitation)
              {
            %>
            <%
                
                
            %>


            <tr class="RowStyle">
                <td><%= item.VisitationDate.ToString("MM/dd/yyyy") %></td>
                <td><%= item.VisitationName %></td>
                <td><%= item.VisitationType %></td>
                <td><%= item.VisitaionStatus %></td>
                <td><%= item.ExpiredOn.ToString("MM/dd/yyyy") %></td>
                <td>
                    <img src="../../Images/edit.PNG" onclick="loadFunction(<%= item.VisitationId %>,'Edit');" style="cursor:pointer;" /></td>
                <td>
                    <%if (ViewBag.permission == "true")
                      { %>
                    <img src="../../Images/delete.PNG" onclick="loadFunction(<%= item.VisitationId %>,'Delete');"style="cursor:pointer;" />
                     <%} %>
                </td>
                   
            </tr>
            <%
              } %>
        </table>
        
        <div class="pager"><% Html.RenderPartial("Pager", Model.pageModel); %></div>
    </div>


</div>


