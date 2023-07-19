<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ClientDB.Models.CallLogModel>" %>

<style>
    .popUpStyle {
        background: url("../Images/smlmelmark.JPG") no-repeat scroll 7px 5px #F8F7FC;
        border: 2px solid #B2CCCA;
        display: none;
        font-family: Arial;
        font-size: 12px;
        height: 350px;
        left: 32%;
        padding: 15px;
        position: absolute;
        top: 35%;
        width: 450px;
        z-index: 200;
    }
</style>

<script type="text/javascript">

    function CallDetailsSelect(val) {
        $('#callLogPopup').load('../CallLog/CallLogDetails?CallLogId=' + val);
        $("#Popup").show();
    }

    function EditLog(val) {
        $('#divCallLog').load('../CallLog/CallLog2?CallLogid=' + val);

    }

        function closePop() {
            $("#Popup").hide();
        }

        $(document).ready(function () {

            $('#divCallLog').load('../CallLog/CallLog2');
        });

</script>
<div id="divCallLog"></div>

<div id="partialMainArea">
    <table>
        <%if (Model.CallLists.Count == 0)
          { %>
        <tr>
            <td class="RowStyle" style="width: 100%;">No Family and Agency Communication logs found for this Student...</td>
        </tr>
    </table>
    <%}
          else
          { %>

    <table id="tblContacList" style="width: 100%">
        <tr class="HeaderStyle">
            <th class="tdLabel">Contact Name</th>
            <th class="tdLabel">Relationship</th>
            <th class="tdLabel">Call Time</th>
            <th class="tdLabel">Call Type</th>
            <th class="tdLabel">Details</th>
            <th class="tdLabel">Edit</th>
        </tr>

        <%foreach (var item in Model.CallLists)
          {
        %>
        <tr class="RowStyle">
            <td style="width: 10%;"><%= item.ContactName%></td>
            <td style="width: 10%;"><%= item.Relationship%></td>
            <td style="width: 10%;"><%=Convert.ToDateTime(item.Calltime).ToString("MMM/dd/yyyy hh:mm:ss tt")%></td>
            <%if (item.CallflagName == "Incoming")
              { %>
            <td style="width: 10%;">Incoming</td>
            <%}
              else if (item.CallflagName == "Outgoing")
              { %>
            <td style="width: 10%;">Outgoing</td>
            <%} 
              else if(item.CallflagName=="Others")
              {%>
                  <td style="width: 10%;">Others</td>
              <% }%>
            
            <td><a href="#" onclick="CallDetailsSelect(<%=item.CallLogId%>)">Details</a></td>
            <td><a href="#" onclick="EditLog(<%=item.CallLogId%>)">Edit</a></td>
        </tr>
        <%
          }
          } %>
    </table>
    <div id="Popup" class="popUpStyle" style="width: 50%; height: 40%; left: 23%; top: 26%; overflow-y: auto; overflow-x: hidden">
        <a id="close_x" class="close sprited1" href="#" style="" onclick="closePop()">
            <img src="../Documents/CSS/images/button_red_close.png" height="18" width="18" alt="" style="float: right; margin-right: 22px; margin-top: 16px; z-index: 300" /></a>
        <div id="callLogPopup">
        </div>
    </div>
    <div class="pager"><% Html.RenderPartial("Pager", Model.pageModel); %></div>
</div>
