<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ClientDB.Models.PlacementModel>" %>
<style>
    .placmntCheck {
    width:auto;
    }
</style>

<script type="text/javascript">
    $(document).ready(function () {

        $('#AddNewPlacement').load('../Placement/AddPlacement/0');

    });
    $("#tblContacList tr:odd").css("background-color", "#F3F3F3");

    function checkStudentStatus() {
        var discharged = $("#staticTab").find("span").html();
        if (discharged.includes("Discharge")) {
            //var status = document.getElementById("hfStudentStatus").value;
            //if (status === "D") {
            alert("Student is discharged. Please activate student for placement operations.");
            return false;
        }
        return true;
    }

    function loadFunction(id, type) {
        if (!checkStudentStatus()) {
            // If checkStudentStatus returns false, stop further processing.
            return false;
        } else {
            if (type == "Edit") {
                //alert(id + " " + type)
                document.getElementById('AddNewPlacement').style.display = "block";
                $('#AddNewPlacement').load('../Placement/AddPlacement/' + id);



            }
            if (type == "Delete") {
                //alert(id + " " + type)
                if (confirm('Are you sure you want to delete this placement?')) {
                    $('#content').load('../Placement/DeletePlacementDetails/' + id);
                } else {
                    return false;
                }

            }
        }
    }


</script>
<div id="partialMainArea">
    <% if (TempData["notice"] != null)
       { %>
    <p style="width: 100%; color: red; font-size: 14px; font-weight: bold"><%= Html.Encode(TempData["notice"]) %></p>
    <% } %>
    <div id="AddNewPlacement">
    </div>

    <div style="float: left; width: 100%">
        <table id="tblContacList" style="width: 100%">
            <tr class="HeaderStyle">
                <th class="tdLabel" style="text-align:left">Placement</th>
                 <th class="tdLabel" style="text-align:left">Program</th>
                <th class="tdLabel" style="text-align:left">Location</th>
                <th class="tdLabel" style="text-align:left">Mon</th>
                <th class="tdLabel" style="text-align:left">Tue</th>
                <th class="tdLabel" style="text-align:left">Wed</th>
                <th class="tdLabel" style="text-align:left">Thu</th>
                <th class="tdLabel" style="text-align:left">Fri</th>
                <th class="tdLabel" style="text-align:left">Sat</th>
                <th class="tdLabel" style="text-align:left">Sun</th>
                <th class="tdLabel" style="text-align:left">Start Date</th>
                <th class="tdLabel" style="text-align:left">End Date</th>
                <th class="tdLabel" style="text-align:left">Edit</th>
                <th class="tdLabel" style="text-align:left">Delete</th>
            </tr>
            <%foreach (var item in Model.listPlacement)
              {
            %>
            <%
                
                
            %>


            <tr class="RowStyle">
                <td style="text-align:left"><%= item.PlacementName %></td>
                 <td style="text-align:left"><%= item.Program %></td>
                 <td style="text-align:left"><%=item.LocationId %></td>
                <%if(item.IsMonday==true)
                      {%>  
                  <td style="text-align:left"><input style="width:auto;" type="checkbox" onclick="return false" checked="checked" /> </td>
                   <%} %>
                <%else{ %>
                <td style="text-align:left"><input style="width:auto;" type="checkbox" onclick="return false" /> </td>
                <%} %>

                 <%if(item.IsTuesday==true)
                      {%>  
                  <td style="text-align:left"><input style="width:auto;" type="checkbox" onclick="return false" checked="checked" /> </td>
                   <%} %>
                <%else{ %>
                <td style="text-align:left"><input style="width:auto;" type="checkbox" onclick="return false" /> </td>
                <%} %>

                 <%if(item.IsWednesday==true)
                      {%>  
                  <td style="text-align:left"><input style="width:auto;" type="checkbox" onclick="return false" checked="checked" /> </td>
                   <%} %>
                <%else{ %>
                <td style="text-align:left"><input style="width:auto;" type="checkbox" onclick="return false" /> </td>
                <%} %>

                 <%if(item.IsThursday==true)
                      {%>  
                  <td style="text-align:left"><input style="width:auto;" type="checkbox" onclick="return false" checked="checked" /> </td>
                   <%} %>
                <%else{ %>
                <td style="text-align:left"><input style="width:auto;" type="checkbox" onclick="return false" /> </td>
                <%} %>

                 <%if(item.IsFriday==true)
                      {%>  
                  <td style="text-align:left"><input style="width:auto;" type="checkbox" onclick="return false" checked="checked" /> </td>
                   <%} %>
                <%else{ %>
                <td style="text-align:left"><input style="width:auto;" type="checkbox" onclick="return false" /> </td>
                <%} %>

                 <%if(item.IsSaturday==true)
                      {%>  
                  <td style="text-align:left"><input style="width:auto;" type="checkbox" onclick="return false" checked="checked" /> </td>
                   <%} %>
                <%else{ %>
                <td style="text-align:left"><input style="width:auto;" type="checkbox" onclick="return false" /> </td>
                <%} %>

                 <%if(item.IsSunday==true)
                      {%>  
                  <td style="text-align:left"><input style="width:auto;" type="checkbox" onclick="return false" checked="checked" /> </td>
                   <%} %>
                <%else{ %>
                <td style="text-align:left"><input style="width:auto;" type="checkbox" onclick="return false" /> </td>
                <%} %>
                
                               
                <td style="text-align:left"><%if(item.StartDate!=null)
                      {%>    
                    <%=item.startdatetime%><%} %>
                    
                   </td>
                <td style="text-align:left"><%if(item.EndDate!=null)
                      {%>    
                    <%=item.datetime%><%} %></td>
                <td style="text-align:left">
                    <img src="../../Images/edit.PNG" onclick="return loadFunction(<%= item.PlacementId %>,'Edit');" style="cursor:pointer;" /></td>
                <td style="text-align:left">
                    <%if (ViewBag.permission == "true")
                      { %>
                    <img src="../../Images/delete.PNG" onclick="return loadFunction(<%= item.PlacementId %>,'Delete');" style="cursor:pointer;" />
                     <%} %>
                </td>
               
            </tr>
            <%
              } %>
        </table>
        <div class="pager"><% Html.RenderPartial("Pager", Model.pageModel); %></div>
    </div>


</div>

