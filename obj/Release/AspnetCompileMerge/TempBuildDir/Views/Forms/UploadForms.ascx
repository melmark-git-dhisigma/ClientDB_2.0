<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ClientDB.Models.AddDocumentModel>" %>

<script src="../Documents/JS/jquery.validationEngine-en.js"></script>
<script src="../Documents/JS/jquery.validationEngine.js"></script>
<script src="../Documents/JS/jquery.form.js"></script>

<%--<script src="../../Documents/JS/jquery.form.js"></script>--%>
<%--<script src="../../Documents/JS/jquery-ui-1.11.2.js"></script>--%>
<script type="text/javascript">

    //jQuery("#UploadForm").validationEngine();

    var options = {
        success: showResponse  // post-submit callback 
    };
    $(document).ready(function () {
        jQuery("#UploadForm").validationEngine();
        $('#UploadForm').ajaxForm(options);
        // $('#content').load('../Forms/ListDocuments/');
    });



    function showResponse(responseText, statusText, xhr, $form) {

        $('#content').load('../Forms/ListDocuments/');

    }

    function generateTemplate(id) {
        $('#content').load('/Forms/AllInOne/');
    }
    function displayDiv() {
        if (($('#ddlDocumentType :selected').text() == "Other"))
            $('#shwtxt').show();
        else
            $('#shwtxt').hide();

    }

    

</script>
<style type="text/css">
   

    div.mainContainer div.contentPart div.ContentAreaContainer div.middleContainer table tr td {
        color: #525252;
        font-family: Arial,Helvetica,sans-serif;
        font-size: 11px !important;
        width: 0% !important;
    }

      .lblSpan {
        font-style: italic;
        font-weight: bold;
    }
</style>

<%using (@Html.BeginForm("SaveForms", "Forms", FormMethod.Post, new { id = "UploadForm", enctype = "multipart/form-data" }))
  { %>
<table style="width: 70%">
    <%=Html.HiddenFor(m=>m.Id,Model.Id) %>
    <tr>
        <td colspan="3">
            <label class="lblSpan">Document Name</label><span class="span-align">*</span><br />
            <%=Html.TextBoxFor(m => m.DocumentName, Model.DocumentName, new { @class = "validate[required]",maxlength=50 })%></td>
        <td colspan="3">
            <label class="lblSpan">Document Type </label>
            <span class="span-align">*</span><br />
            <%=Html.DropDownListFor(m => m.DocumentType, Model.DocumentTypeList, new { @class = "validate[required]", ID = "ddlDocumentType", onChange = "displayDiv()" })%></td>
    </tr>
    <tr>
        <td>
            <%--<div id="shwtxt" style="display:none; float:right;">
                Other Type(Specify)<span style="color: red;">*</span>
               <span style="float:right;"> <%=Html.TextBoxFor(m => m.Other,new { @class = "validate[required] ", ID = "ddlDocumentType",value=Model.Other })%></span>
            </div>--%>
        </td>
    </tr>


    <tr>
        <td colspan="3">
            <label class="lblSpan">Select File</label><span class="span-align">*</span><br />
            <input type="file" id="profilePicture" name="profilePicture" class="validate[required] " /></td>
        <td colspan="2">
            <div id="shwtxt" style="display: none;">
                <label class="lblSpan">Other Type(Specify)</label><span class="span-align">*</span><br />
                <%=Html.TextBoxFor(m => m.Other,new { @class = "validate[required] ", ID = "ddlDocumentTypeTxt",value=Model.Other })%>
            </div>
        </td>

        <td>

            <%--<input id="btnGenerate" type="button" value="Generate Form" onclick="generateTemplate(this.id);" />--%>
        </td>
    </tr>

    <%--<tr>
        <td colspan="4"><br /><br /></td>
    </tr>--%>

    <%--<tr>
        <td colspan="4"></td>
    </tr>--%>
    <tr>
        <td>&nbsp;</td>
    </tr>
    <tr>

        <td></td>
        <td style="padding-right: 0%;">
            <% if (ViewBag.permission == "true")
               {
                   if (Session["PlacementStat"].ToString() != "I")
                   {%>
            <input id="btnEvents" type="submit" value="Save" onclick="" />
            <%}
               } %></td>
        <%-- <td>--%>

        <%--<input id="btnGenerate" type="button" value="Generate Form" onclick="generateTemplate(this.id);" />--%>
        <%--</td>--%>
        <%-- <td style="text-align:left"></td>--%>
    </tr>
    <%--<tr>--%>
    <%--<td colspan="4"></td>--%>
    <%--</tr>  <tr>--%>
    <%--<td colspan="4"></td>--%>
    <%--</tr>--%>
</table>
<div class="btnDoneStyle">
</div>

<%} %>




<script type="text/javascript">

                             $(document).ready(function () {

                                 staffNameAutocomplete();

                             });

                             function staffNameAutocomplete() {


                                 $("#ddlDocumentTypeTxt").autocomplete({
                                     source: function (request, response) {
                                         $.ajax({
                                             url: "../Forms/otherDocumentTypes",
                                             type: "POST",
                                             dataType: "json",
                                             data: { term: request.term },
                                             success: function (data) {
                                                 //response(data.ReferralName);
                                                 response($.map(data, function (item) {
                                                     return { label: item.OtherDocumentType, value: item.OtherDocumentType };
                                                 }))
                                             },
                                         })
                                     },
                                     messages: {
                                         noResults: '',
                                         results: function (resultsCount) { }
                                     }
                                 });
                             }
</script>
