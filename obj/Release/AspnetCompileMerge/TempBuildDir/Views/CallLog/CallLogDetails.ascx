<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ClientDB.Models.CallLogModel>" %>

       <table style="width: 100%" class="gridStyle">
            <thead>
                <tr class="HeaderStyle">
                    <td style="width: 20%"><b>Contact Name</b>
                    <td style="width: 20%"><b>Conversation Details</b>
                    </td>
                </tr>
                <tr>
                    <td><%=Model.NameofContact%></td>
                    <td><%=Model.Conversation%></td>
                </tr>


            </thead>


        </table>

