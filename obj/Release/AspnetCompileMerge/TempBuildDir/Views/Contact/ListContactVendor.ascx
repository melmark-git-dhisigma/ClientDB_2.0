<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ClientDB.Models.ListContactModel>" %>

<script type="text/javascript">
    $("#tblContacList tr:odd").css("background-color", "#F3F3F3");
    //$('#imagePanel').load('/Contact/ImageUploadPanel');
    
    function loadFunction(id, type) {

        if (type == "Edit") {
            $('#content').load('../Contact/fillContactDetails/' + id);
        }
        if (type == "Delete") {
            if (confirm('Are you sure you want to delete this contact/vendor?')) {
                $('#content').load('../Contact/DeleteContactDetails/' + id);
            } else {
                return false;
            }
        }
        
    }
    function Contacts(id) {
        $('#LoadContactLog').load('../Contact/QuickView/' + id);
        $("#divContactLog").hide();
    }
    function ContactShow(id) {
        Contacts(id)
        $("#divContactLog").show();
    }

    $('#btnAddContact').on("click", function (event) {
        $('#content').load('../Contact/Index');
    });
</script>
<div id="partialMainArea">
    <% if (TempData["notice"] != null)
       { %>
    <p style="width:100%;color:red;font-size:14px;font-weight:bold"><%= Html.Encode(TempData["notice"]) %></p>
    <% } %>
    <div style="width: 30%; float: left; display: none">
        <select id="ddlContact" name="ddlContact" style="width: 150px" onchange="ListContacts()">
            <option>-Select-</option>
            <option>Contact</option>
            <option>Vendor</option>
        </select>
    </div>

    <div style="width: 30%; float: left; display: none">
        <%-- <%=Html.TextBoxFor(m => m.Searchtext, Model.Searchtext, new {  @class="validate[required] " })%>--%>
        <input id="btnSearch" type="button" value="Search" onclick="" />
    </div>

    <div style="float: left; width: 100%">
        <table id="tblContacList" style="width: 100%">
            <tr class="HeaderStyle">
                <th class="tdLabel">Name</th>
                <th class="tdLabel">Relationship</th>
                <th class="tdLabel">Relationship Description</th>
                <th class="tdLabel">Emergency</th>
                <th class="tdLabel">Incident</th>
                <th class="tdLabel">Correspondence&nbsp;</th>
                <th class="tdLabel">Guardian</th>
                <th class="tdLabel">Approved<br />Visitor</th>
                <th class="tdLabel">Quick<br />View</th>
                <th class="tdLabel">Edit</th>
                <th class="tdLabel">Delete</th>
                
            </tr>
            <%foreach (var item in Model.listContacts)
              {
            %>
            <tr class="RowStyle">
                <%--<td><%= item.Name %></td>--%>
                <td><%= item.FullName %></td>
                <td><%= item.Relation %></td>
                <td><%= item.RelationDesc %></td>
                
                    <%if(item.IsEmer==true){ %>

                    <td style="text-align:left"><input style="width:auto;" type="checkbox" onclick="return false" checked="checked" /></td>
                <%}else {%>
                <td style="text-align:"><input style="width:auto;" type="checkbox" onclick="return false"/></td>
                <%} %>
                <%if(item.IsIncident==true){ %>

                    <td style="text-align:left"><input style="width:auto;" type="checkbox" onclick="return false" checked="checked" /></td>
                <%}else{ %>
                <td style="text-align:left"><input style="width:auto;" type="checkbox" onclick="return false"/></td>
                <%} %>
                <%if(item.IsCor==true){ %>

                    <td style="text-align:left"><input style="width:auto;" type="checkbox" onclick="return false" checked="checked" /></td>
                <%}else{ %>
                <td style="text-align:left;"><input style="width:auto;" type="checkbox" onclick="return false"/></td>
                <%} %>
                <%if(item.IsGuardian==true){ %>

                    <td style="text-align:left"><input style="width:auto;" type="checkbox" onclick="return false" checked="checked" /></td>
                <%}else{ %>
                <td style="text-align:left"><input style="width:auto;" type="checkbox" onclick="return false"/></td>
                <%} %>
                <td style="text-align:left; padding-right: 10px;">
                <%if(item.IsOnCampusWithStaff==true){ %>
                    On Campus, With Staff;
                <%}else{ %>
                <%} %>
                    <%if(item.IsOnCampusAlone==true){ %>
                    On Campus, Alone;
                <%}else{ %>
                <%} %>
                    <%if(item.IsOffCampus==true){ %>
                    Off Campus<br />
                <%}else{ %>
                <%} %>
                    </td>
                <td><img src="../../Images/view-icon.PNG" style="cursor:pointer" onclick="ContactShow(<%= item.ContactId %>);" /></td>
                <td>
                    <img src="../../Images/edit.PNG" style="cursor:pointer" onclick="loadFunction(<%= item.ContactId %>,'Edit'); " /></td>
                <td>
                   <%if(ViewBag.permission == "true"){
                         //if (Session["PlacementStat"].ToString() != "I"){ %>
                  
                    <img src="../../Images/delete.PNG" style="cursor:pointer" onclick="loadFunction(<%= item.ContactId %>,'Delete');" /></td>
               <% }%>
                
            </tr>
            <%
              } %>
        </table>
        <div class="pager"><% Html.RenderPartial("Pager", Model.pageModel); %></div>
        <div style="width: 5%; float: right; margin-top: 30px;">
              <%if(ViewBag.permission == "true"){
                         if (Session["PlacementStat"].ToString() != "I"){ %>
            <input id="btnAddContact" type="button" value="Add Contact" onclick="" />
             <%} }%>
        </div>
    </div>

    <div id="divContactLog" class="popUpStyle"  style="width: 84%; height: 75%; left: 9%; top: 15%; overflow-y: auto; padding:12px; overflow-x: hidden; border-radius:18px; z-index: 1010 !important;position:fixed;">
    <div id="close_x1" onclick="javascript: $('#divContactLog').hide();event.preventDefault();" class="close sprited1" href="#" style="position: fixed; cursor: pointer; margin-top: 6px; width: 82%;">
        <img src="../../Images/delete.png" alt="" style=" height:auto;width:auto;z-index: 300" />
    </div>
    <br />
    <div class="messageDiv" style="color: red;"></div>
    <div id="LoadContactLog"></div>

</div>

</div>
<%--<div id="imagePanel" style="border: 1px solid #CCCCCC; float: left; margin-left: 5px; margin-right: 20px; margin-top: 20px; width: 18%;">
</div>--%>


<script>loadClientStaticDetails();</script>