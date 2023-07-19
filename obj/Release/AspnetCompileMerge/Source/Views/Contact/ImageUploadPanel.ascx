<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>

<%--<script src="../../Documents/JS/jquery-1.8.2.js"></script>--%>
<style type="text/css">
    .tblStyle {
        width: 100%;
    }



    input[type="submit"] {
        width: 60px;
        border: 1px solid #CCC;
        height: 24px;
        vertical-align: middle;
    }

    input[type="checkbox"] {
        width: 22px;
        border: 1px solid #CCC;
        height: 24px;
        vertical-align: middle;
    }
</style>

<script type="text/javascript">
    function EditMedicalData(id) {
        var data = null;
        var school = parseInt('<%: ViewBag.SchoolId %>');
        data = id + '|*';
        if (school == 1) {
            $('#content').load('../ClientRegistration/ClientRegistration?data=' + data);
        }
        if (school == 2) {
            $('#content').load('../ClientRegistrationPA/ClientRegistrationPA?data=' + data);
        }
        $('.imgcontainer').css("display", "none");
        $('#calender').css("display", "none");
    }
    function EditMediData(id) {
        var data = null;
        var text = id.split('-');
        id = text[1];
        data = id + '|*';
        $('#content').load('../Medical/FillMedicalData?data=' + data);
        $("#calender").css({ left: '' });
    }
</script>


<table class="tblStyle">
    
    <tr>
        <td id="tdstudentName" style="text-align:center;font-weight:bold;font-family:Arial;display:none;"> <%=ViewBag.StudentName %></td>
    </tr>
    <tr>
        <td style="text-align:center;font-weight:bold;font-family:Arial;display:none;"><%=ViewBag.StudentId %></td>
    </tr>
    <tr>
        <td style="height:121px">
            <%-- <%=Html.DisplayFor(m=>m.ImageUrl) %>--%>
            <img id="stdImage" alt="No Image Uploaded" src="data:image/gif;base64,<%=ViewBag.imageUrl %>" /></td>
    </tr>

    <tr>
        <td>
            <% 
                if(ViewBag.permission == "true"){if (Session["PlacementStat"].ToString() != "I")
           { %>
             <input class="EditProfile" type="button" value="Edit Profile" id="<%=ViewBag.ModelId %>" onclick="EditMedicalData(this.id);" style="float:left; margin-left: 30%;display:<%=ViewBag.editButton %>" /></td>
        <%} } %>
    </tr>
</table>




