<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ClientDB.Models.SignatureModel>"%>




<script type="text/javascript">
    $(document).ready(function () {

        $('#UploadDocuments').load('../Forms/UploadForms/0');

    });


    $("#tblContacList tr:odd").css("background-color", "#F3F3F3");
    // $('#imagePanel').load('/Contact/ImageUploadPanel');

    function loadFunction(id, type) {

        if (type == "Delete") {
            if (confirm('Are you sure you want to delete this document?')) {
                $('#content').load('../Forms/DeleteDocuments/' + id);
            } else {
                return false;
            }

        }
        if (type == "Download") {
            //alert(id + " " + type)
            window.open('../Forms/viewDoc?id=' + id + '   ', '_blank');

        }
    }

</script>
<script type="text/javascript">
    function DropdownChangeFun(ddl) {
        // alert(ddl.selectedIndex);

        $('#content').load('../Forms/FilterDocuments/' + ddl.selectedIndex);

    }
</script>
<div id="partialMainArea">
    <% if (TempData["notice"] != null)
       {
           %>
    <p style="width: 100%; color: red; font-size: 14px; font-weight: bold"><%= Html.Encode(TempData["notice"]) %></p>
    <%  
        } %>
    <div id="UploadDocuments">
    </div>
  
    <div id="Sample" style="float: left; width: 100%">
         <table style="width: 100%">
    
    <tr>
          <td>
               <label class="lblSpan">Filter By Module </label><span class="nospan-align">*</span><br />
        <%=Html.DropDownListFor(m => m.DocumentModule, Model.DocumentModuleList, new { @style = "width:25%;margin-Left:3%", @Id = "DdlType", onchange = "DropdownChangeFun(this)" })%></td>
        
    </tr>
    </table>
       
        <table id="tblContacList" style="width: 100%">
            <tr class="HeaderStyle">
                <th class="tdLabel">Document Name</th>
                <th class="tdLabel">Document Type</th>
                <th class="tdLabel">Uploaded On</th>
                <th class="tdLabel">Uploaded By</th>
                <th class="tdLabel">Delete</th>
                <th class="tdLabel">Download</th>
                <%if(Model.moduleid==4) 
                  {%>
                <th class="tdLabel">Allow Parent</th>
                <%} %>
            </tr>
            <%foreach (var item in Model.FilterDocuments)
              {
            %>
            <%
                
                
            %>


            <tr class="RowStyle">
                <td><%= item.DocumentName %></td>
                <%if(item.DocumentType=="Other")
                  {%>
                <td><%= item.Other %></td>
                <%}else{ %>
                <td><%=item.DocumentType %></td>
                <%} %>
                <td><%=Convert.ToDateTime(item.CreatedOn).ToString("MM/dd/yyyy") %></td>
                <td><%= item.UserType %></td>
                <%if (ViewBag.permission == "true")
                  {
                      if (Session["PlacementStat"].ToString() != "I")
                      {%>
                <td><img src="../../Images/delete.PNG" onclick="loadFunction(<%= item.DocumentId%>,'Delete');" style="cursor:pointer;" />
                   </td>
                <td><img src="../../Images/download.PNG" onclick="loadFunction(<%= item.DocumentId%>,'Download');" style="cursor:pointer;" />

                    </td>
                <%if (Model.moduleid == 4)
                  { %>
                <td>
                    
                        <%=Html.CheckBoxFor(x => item.IsAllowParent, new { @onclick = "SetParent(" + item.DocumentId + ")", @checked = item.IsAllowParent })%>
                   
                </td>
                <%}
                      }
                  }
                  else
                  {%>
                      <td></td>
                      <td></td>
                <%} %>
            </tr>

            
            <%
              } %>
        </table>
        <div class="pager"><% Html.RenderPartial("Pager", Model.pageModel); %></div>
    </div>


</div>






<script>

    function SetParent(dcoid) {
        //if ($('#item_IsAllowParent').checked == true)
        //{
        //    alert("hai");
        //}
        
            $.ajax({
                type: "POST",
                url: "../Forms/AllowParent",
                data: "{'docId':'"+dcoid+"'}",
                contentType: "application/json; charset=utf-8",
               // dataType: "json",
                success: function (response) {
                   
                },
                failure: function (response) {
                    
                }
            });
        }

</script>
