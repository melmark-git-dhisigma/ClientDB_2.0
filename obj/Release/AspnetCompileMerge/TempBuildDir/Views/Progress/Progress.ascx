<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ClientDB.Models.Progress>" %>

<style type="text/css">
    .auto-style2 {
        width: 920px;
    }
</style>

   <script type="text/javascript">
       
       function loadFunction(id, type) {


           if (type == "Edit") {
               //alert(id + " " + type)
               $('#content').load('../Progress/ProgressRpt/' + id);

           }
           if (type == "View") {
               //alert(id + " " + type)
               $('#content').load('../Progress/ProgressRpt/' + id);

           }
       }

</script>


   
<div id="partialMainArea">
    
   
    <div id="Sample" style="float: left; width: 100%">


         <table id="tblContacList" style="width: 100%">
            <tr class="HeaderStyle">
                <th class="tdLabel">Report Date</th>
                <th class="tdLabel">Start Date</th>
                <th class="tdLabel">End Date</th>
                <th class="tdLabel">Status</th>
                <th class="tdLabel">Edit</th>
                <th class="tdLabel">View</th>
            </tr>
            <%foreach (var item in Model.GoalData)
              {
                 foreach (var item1 in item.ProgressInfo)
                  {
                    
               
            %>
            <%
                
                
            %>


            <tr class="RowStyle">
                <td><%=item1.rptdate %></td>
                <td><%=item1.startdate  %></td>
                <td><%=item1.enddate  %></td>
                <td><%=item1.Status %> </td>
                <td>
                     <img src="../../Images/edit.PNG" onclick="loadFunction(<%=item1.iepid  %>,'Edit');" /></td>
                <td><img src="../../Images/download.PNG" onclick="loadFunction(<%=item1.iepid  %>,'View');" /></td>
            </tr>
            <%}
              } %>
        </table>
  </div>
   

</div>
