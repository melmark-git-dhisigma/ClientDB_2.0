<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ClientDB.Models.ProgressReport>" %>

<!DOCTYPE HTML>
<html>
<head>
    <title>Progress Report
    </title>
    <script type="text/javascript">
        var tabid = 1;
        $("#tblContacList tr:odd").css("background-color", "#F3F3F3");

        function loadFunction(editid, type) {

            if (type == "Edit") {
                
                $('#content').load('../ProgressReport/EditProgressReport?id=' + editid + '&tabId=<%=Model.reportType%>');
            }
            if (type == "New") {
                $('#content').load('../ProgressReport/NewProgressReport?id=' + editid + '&tabId=<%=Model.reportType%>');
            }
            if (type == "DeleteRpt") {
                if (confirm('Are you sure you want to delete this report?')) {
                    $('#content').load('../ProgressReport/DeleteProReportList?id=' + editid + '&tabId=<%=Model.reportType%>');
                } else {
                    return false;
                }
            }
        }
        //$('#btnCreateNewReport').on("click", function (event) {
        //    $('#content').load('../ProgressReport/Page2');
        //});

        function loadData() {
            tabid = $('#FormSelect').val();
            //alert(tabid)
            $('#content').load('../ProgressReport/Index?filter='+tabid);
        }


    </script>

    <style type="text/css">
        .dropstyle {
            margin-left:20px;
        }
    </style>

</head>
<body>
    <div style="width: 100%; float: right; margin-top: 30px; margin-right: 5px">
        <label style="margin-left:15px;" >Progress Report Type </label>
        <%=Html.DropDownListFor(m => m.reportType, new List<SelectListItem> {new SelectListItem{ Text="Residential and Educational Progress Report",Value="1"},
    new SelectListItem {Text="Children’s Residential Quarterly Progress Report",Value="2"},
    new SelectListItem{Text="RTF Quarterly Progress Report",Value="3"},
    new SelectListItem{Text="RTF Monthly Progress Report",Value="4"},
    new SelectListItem{Text="Children's Residential Monthly Progress Report",Value="5"}
    
}, new { ID = "FormSelect" ,@class="dropstyle" , onchange="loadData();"})%>
        <%if(ViewBag.permission=="true")
        {if(Session["PlacementStat"].ToString() != "I"){%>
        <input id="btnCreateNewReport" type="button" value="Create New" onclick="loadFunction(0, 'New');" />
        <%} }%>
        <br />
    </div>
    <div  style="float: left; width: 100%">
        <br />
        <table id="tblProReportList" style="width: 100%">
            <tr class="HeaderStyle">
                <th class="tdLabel">Sl.No.</th>
                <th class="tdLabel">Created On</th>
                <th class="tdLabel">Edit</th>
                <th class="tdLabel">Delete</th>
            </tr>
            <% var RptRowId = 1; if(Model.Rpt_List!=null){ foreach (var item in Model.Rpt_List)
               {
            %>
            
            <tr class="RowStyle">
                <td><%=RptRowId%></td>
                <td><%=item.RptCreatedOn%></td>
                
                <td>
                    <%if(ViewBag.permission=="true"){ %>
                    <img src="../../Images/edit.PNG" style="cursor: pointer" onclick="loadFunction(<%= item.ProReportId%>,'Edit'); " />
                    <%} %>
                </td>
                <td>
                    <%if (ViewBag.permission == "true")
                      {
                          if (Session["PlacementStat"].ToString() != "I")
                          { %>
                    <img src="../../Images/delete.PNG" style="cursor: pointer" onclick="loadFunction(<%= item.ProReportId%>,'DeleteRpt'); " />
                    <%}
                      } %>
                </td>
               
            </tr>
            <%
                  RptRowId++;
                  }
              } %>
        </table>
    </div>
</body>
</html>
