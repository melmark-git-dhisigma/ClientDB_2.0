<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ClientDB.Models.MedicalModel>" %>

<%using (@Ajax.BeginForm("SavemedicalData", "Medical", FormMethod.Post, new AjaxOptions { UpdateTargetId = "content", OnSuccess = "check" }, new { id = "MedicalForm" }))
  { %>
<div style="width: 100%">
    <div id="partialMainArea">
        <% if (TempData["notice"] != null)
           { %>
        <p style="width: 100%; color: red; font-size: 14px; font-weight: bold"><%= Html.Encode(TempData["notice"]) %></p>
        <% } %>

        <table class="tblStyle" style="width: 100%;">


            <tr>


                <td>
                  <p style="width: 100%; color: red; font-size: 16px; font-weight: bold;text-align:center">Click <a href="#">Here</a> to Login to EMR</p>
                </td>

              
            </tr>

           
        </table>
    </div>

</div>
<%} %>

