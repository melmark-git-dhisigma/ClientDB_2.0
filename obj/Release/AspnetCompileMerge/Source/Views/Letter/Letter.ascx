<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ClientDB.Models.LetterGenerationViewModel>" %>

<script type="text/javascript">
    function Show(data) {

        //var mywindow = window.open('', 'Letter', 'height=600,width=600,scrollbars=yes,resizable=yes,fullscreen=no');
        //mywindow.document.write('<html><head><title>Letter</title>');
        ///*optional stylesheet*/ //mywindow.document.write('<link rel="stylesheet" href="main.css" type="text/css" />');
        //mywindow.document.write('</head><body >');
        //mywindow.document.write(data);
        //mywindow.document.write('</body></html>');
        //mywindow.document.close();
        //mywindow.print();


        var mywindow = window.open('', 'Letter', 'height=600,width=600,scrollbars=yes,resizable=yes,fullscreen=no');
        mywindow.document.write('<html><head><title>Letter</title><style type="text/css" media="print">.no-print {display:none;}</style>');
        mywindow.document.write('<script type="text/javascript"> var letter="letter.doc"; var main="tray";var apptype="application/msword"; function downloadDoc(filename, elId, mimeType) { var elHtml = document.getElementById(elId).innerHTML; var blob = new Blob([elHtml],{ type: "application/csv;charset=utf-8;"}); navigator.msSaveBlob(blob, filename); }<\/script>');
        mywindow.document.write('</head><body >');
        mywindow.document.write(data);
        mywindow.document.write('<input type="button" id="btnprint" class="no-print" value="Download" onclick="downloadDoc(letter, main,apptype);" />');
        mywindow.document.write('</body></html>');
        mywindow.document.close();
        mywindow.print();
        return true;


        
    }
</script>
<div id="partialMainArea">





    <table id="tblContacList" style="width: 100%">
        <tr class="HeaderStyle">
            <th class="tdLabel">Letter Name</th>
            <th class="tdLabel">Recipient Name</th>
            <th class="tdLabel">Created On</th>
            <th class="tdLabel">Status</th>
            <th class="tdLabel">Sent On</th>
            <th class="tdLabel">Show Letter</th>
        </tr>
        <%foreach (var item in Model.LetterLists)
          {
        %>


        <tr class="RowStyle">
            <td style="width:15%;"><%= item.LetterName %></td>
            <td style="width:15%;"><%= item.RecipientName %></td>
            <td style="width:10%;"><%=Convert.ToDateTime(item.CreatedOn).ToString("MM/dd/yyyy") %></td>
            <td style="width:10%;"><%= item.status %></td>
            <td style="width:10%;"><%= item.SentOn %></td>
            <td style="width:10%;">
                <%= Ajax.ActionLink("Show/Print", "LetterTrayView", new { LetterTrayId = item.LetterTrayId }, new AjaxOptions { OnSuccess="Show" })  %>
                     
              
            </td>
        </tr>
        <%
          } %>
    </table>


        <div class="pager"><% Html.RenderPartial("Pager", Model.pageModel); %></div>

</div>
