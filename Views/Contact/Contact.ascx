<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ClientDB.Models.ContactModel>" %>
<%--<script src="../../Documents/JS/jquery-1.8.2.js"></script>--%>
<script src="../../Documents/JS/jquery-ui-1.8.24.js"></script>
<script src="../../Documents/JS/jquery.validationEngine.js"></script>
<script src="../../Documents/JS/jquery.validationEngine-en.js"></script>
<script src="../../Documents/JS/jquery.mask.js"></script>
<script src="../../Documents/JS/jquery.form.js"></script>
<style type="text/css">
    input[type="button"] {
        background-color: #f0f0f0 !important;
        border-top-left-radius: 3px !important;
        border-top-right-radius: 3px !important;
        border-top: 1px solid #808080 !important;
        border-left: 1px solid #808080 !important;
        border-right: 1px solid #808080 !important;
        color: black;
        margin-left: 2px !important;
    }
</style>



<script type="text/javascript">

    //  $('#imagePanel').load('/Contact/ImageUploadPanel');


    //var styl = document.getElementById('imagePanel').style;
    //styl.display = "block";
    //styl.border = "1px solid #CCCCCC";
    //styl.float = "left";
    //styl.marginleft = "5px";
    //styl.marginright = "20px";
    //styl.margintop = "20px";
    //styl.width = " 18%";

    $(document).ready(function () {

        $('#btnHome').css('color', '#23A7E3');
        $('.zipDefault').keypress(function (event) {
            var keyVal = event.which;
            if ((keyVal > 31) && (keyVal < 48) || keyVal > 57)
                return false;
            return true;

        })


        $('.zipDefault').blur(function () {
            var textCont = $(this).val();
            var preText = "";
            if (textCont.length < 5) {
                for (var i = 0; i < (5 - textCont.length) ; i++) {
                    preText = preText + "0";
                }
            }

            $(this).val(preText + textCont);

        });

        $('.usPhone').mask('(000)000-0000');
        $('.usPhoneExt').mask('00000');
        $('.namefield').keypress(function (event) {
            var keyValue = event.which;
            if ((keyValue >= 65 && keyValue <= 90) || (keyValue >= 97 && keyValue <= 122) || (keyValue == 39) || (keyValue == 32) || (keyValue == 8) || (keyValue == 0) || (keyValue == 45)) {

            }
            else {
                event.preventDefault();
            }

        });

    });

    function CheckboxApprovdVisitor() {
        if ($('#IsOnCampusWithStaff').prop("checked") == true || $('#IsOnCampusAlone').prop("checked") == true || $('#IsOffCampus').prop("checked") == true) {
            $('#ApprovedVisitor').prop("checked", true);
        }
        else {
            $('#ApprovedVisitor').prop("checked", false);
        }
    }


    function populate(element) {
        //btnHome btnWork btnOther
        var style;
        var item = element.id;
        if (item == 'btnHome') {
            style = document.getElementById('btnHome').style;
            style.color = "#23A7E3";
            style.fontSize = 16;
            style = document.getElementById('contactHome').style;
            style.display = "block";
            style = document.getElementById('contactOther').style;
            style.display = "none";
            style = document.getElementById('contactWork').style;
            style.display = "none";
            style = document.getElementById(item).style;
            style.borderBottom = "none";
            style = document.getElementById('btnWork').style;
            style.color = "black";
            style = document.getElementById('btnOther').style;
            style.color = "black";
        }
        if (item == 'btnWork') {
            style = document.getElementById('btnWork').style;
            style.color = "#23A7E3";
            style.fontSize = 16;
            style = document.getElementById('contactWork').style;
            style.display = "block";
            style = document.getElementById('contactHome').style;
            style.display = "none";
            style = document.getElementById('contactOther').style;
            style.display = "none";
            style = document.getElementById(item).style;
            style.borderBottom = "none";
            style = document.getElementById('btnHome').style;
            style.color = "black";
            style.backgroundColor = "white";
            style = document.getElementById('btnOther').style;
            style.color = "black";

        }
        if (item == 'btnOther') {
            style = document.getElementById('btnOther').style;
            style.color = "#23A7E3";
            style.fontSize = 16;
            style = document.getElementById('contactOther').style;
            style.display = "block";
            style = document.getElementById('contactHome').style;
            style.display = "none";
            style = document.getElementById('contactWork').style;
            style.display = "none";
            style = document.getElementById(item).style;
            style.borderBottom = "none";
            style = document.getElementById('btnWork').style;
            style.color = "black";
            style = document.getElementById('btnHome').style;
            style.color = "black";
        }

    }

    $('#ddlHomeCountry').change(function () {

        var countryId = $('#ddlHomeCountry').val();
        $.getJSON('../ClientRegistration/getStates', { countryid: countryId }, function (result) {
            var ddlState = $('#ddlHomeState');
            $('#ddlHomeState').empty();

            $.each(result, function (index, item) {
                ddlState.append($('<option/>', {
                    value: item.Value,
                    text: item.Text
                }));
            });
            //  $('#assistenceCityId').find(":selected").removeAttr('selected');
            $('#ddlHomeState>option:eq(0)').attr('selected', true);
        });


    });
    $('#ddlWorkCountry').change(function () {

        var countryId = $('#ddlWorkCountry').val();
        $.getJSON('../ClientRegistration/getStates', { countryid: countryId }, function (result) {
            var ddlState = $('#ddlWorkState');
            $('#ddlWorkState').empty();

            $.each(result, function (index, item) {
                ddlState.append($('<option/>', {
                    value: item.Value,
                    text: item.Text
                }));
            });
            //  $('#assistenceCityId').find(":selected").removeAttr('selected');
            $('#ddlWorkState>option:eq(0)').attr('selected', true);
        });


    });
    $('#ddlOtherCountry').change(function () {

        var countryId = $('#ddlOtherCountry').val();
        $.getJSON('../ClientRegistration/getStates', { countryid: countryId }, function (result) {
            var ddlState = $('#ddlOtherState');
            $('#ddlOtherState').empty();

            $.each(result, function (index, item) {
                ddlState.append($('<option/>', {
                    value: item.Value,
                    text: item.Text
                }));
            });
            //  $('#assistenceCityId').find(":selected").removeAttr('selected');
            $('#ddlOtherState>option:eq(0)').attr('selected', true);
        });


    });



    //$("#btnList").on("click", function (event) {
    //    //event.preventDefault();

    //    //if (jQuery("#registrationForm").validationEngine('validate')) {
    //    $('#content').load('/Contact/fillContactDetails/');


    //    //}
    //});

    //var style = document.getElementById('FirstName').t;
    if (($("#LastName").val() != '')) {
        $("#btnUpdateContact").show();
        $("#btnSaveContact").hide();
    }
    else {
        $("#btnUpdateContact").hide();
        $("#btnSaveContact").show();
    }

    $("#btnUpdateContact").on("click", function (event) {


        //$('#content').load('/Contact/UpdateContactDetails/');
        WriteToString(Arr_NewEventLogsParent);



    });


    function displayDiv() {
        if (($('#Id').val() == "") && ($('#Id').val() == null)) {
            if (($('#ddlPrntRel :selected').text() == "Parent")) {
                if (($('#hdnUser').val() != "") && ($('#hdnUser').val() != null)) {
                    $('#Div1').show()
                }
                else {
                    $('#shwtxt').show()
                }
            }
            else
                $('#Div1').hide()



        }
        else {
            if (($('#ddlPrntRel :selected').text() == "Parent")) {
                if (($('#hdnUser').val() != "") && ($('#hdnUser').val() != null)) {
                    $('#Div1').show()
                }
                else {
                    $('#shwtxt').show()
                }
            }
            else {
                $('#Div1').hide()
                $('#shwtxt').hide();
            }

        }

    }


    $('#noActive').attr('disabled', 'disabled');


    function FillB() {
        var txtB = document.getElementById("ddlDocumentTypeTxt");
        var txtA = document.getElementById("txtA");


        txtB.value = txtA.value;

    }
    function funPaste(e) {
        e.preventDefault();
    }

    function ChangeSRelship(ele) {
        var data = ele.options[ele.selectedIndex].innerHTML;
        var words = data.match(/[^[\]]+(?=])/g);
        if (words != null && words.length > 0) {
            $('#SpouseRelationship').val(words[0]);
        }
        else {
            $('#SpouseRelationship').val("");
        }
    }


    jQuery("#ContactForm").validationEngine();

    //jQuery("#ContactForm").ajaxForm(function () {
        //alert('done');
        loadClientStaticDetails();
    //});
</script>
<style type="text/css">
    table tr td {
        width: 1% !important;
    }

    input[type="text"] {
        background-color: white;
        border: 1px solid #d7cece;
        border-radius: 3px;
        color: #676767;
        font-family: Arial,Helvetica,sans-serif;
        font-size: 13px;
        height: 25px;
        line-height: 26px;
        padding: 0 5px 0 10px;
        width: 150px !important;
        margin-bottom: 4px;
    }

    select {
        width: 167px !important;
        border: 1px solid #CCC;
        height: 29px;
        vertical-align: middle;
        font-family: Arial;
        font-size: 12px;
        color: #333;
        margin-bottom: 5px;
    }

    div.mainContainer div.contentPart div.ContentAreaContainer div.middleContainer table tr td {
        color: #525252;
        font-family: Arial,Helvetica,sans-serif;
        font-size: 11px !important;
        width: auto !important;
    }

    .greyspan-align {
        color: #F0F0F0;
    }

    .lblSpan {
        font-style: italic;
        font-weight: bold;
    }
</style>
<%using (@Ajax.BeginForm("SaveContacts", "Contact", FormMethod.Post, new AjaxOptions { UpdateTargetId = "content" }, new { id = "ContactForm" }))
  { %>

<div id="partialMainArea">
    <table class="tblStyle" style="width: 70% !important;">
        <%=Html.HiddenFor(m=>m.Id,Model.Id) %>
        <%=Html.HiddenFor(m=>m.newEventLog,Model.newEventLog) %>

        <tr>
            <td colspan="2">
                <label class="lblSpan">Prefix</label><span class="nospan-align">*</span><br />
                <%=Html.DropDownListFor(m => m.FirstNamePrefix, Model.FirstNamePrefixList, new {  @class="sd"})%></td>
            <td colspan="2">
                <label class="lblSpan">First Name</label><span class="span-align">*</span><br />
                <%=Html.TextBoxFor(m => m.FirstName, Model.FirstName, new {  @class="validate[required] namefield",onpaste="funPaste(event);",maxlength=50,ID="txtA" })%></td>
        </tr>

        <tr>
            <td colspan="2">
                <label class="lblSpan">Last Name</label><span class="span-align">*</span><br />
                <%=Html.TextBoxFor(m => m.LastName, Model.LastName, new {  @class="validate[required] namefield",onpaste="funPaste(event);",maxlength=50 })%>
                <%--<%=Html.DropDownListFor(m => m.LastNameSuffix, Model.LastNameSuffixList, new {  @class="sd" })%>--%>
            </td>
            <td colspan="2">
                <label class="lblSpan">Middle Name</label><span class="nospan-align">*</span><br />
                <%=Html.TextBoxFor(m => m.MiddleName, Model.MiddleName, new {  @class="sd namefield",onpaste="funPaste(event);",maxlength=50 })%></td>
        </tr>

        <tr>
            <td colspan="2">
                <label class="lblSpan">Suffix</label><span class="nospan-align">*</span><br />
                <%=Html.DropDownListFor(m => m.LastNameSuffix, Model.LastNameSuffixList, new {  @class="sd"})%></td>


        </tr>

        <tr>
            <td colspan="2">
                <label class="lblSpan">Spouse</label><span class="nospan-align">*</span><br />
                <%=Html.DropDownListFor(m => m.SpouseId, Model.SpouseList, new {  @class="sd",onchange="ChangeSRelship(this)"})%></td>

            <td colspan="2">
                <label class="lblSpan">Spouse Relationship</label><span class="nospan-align">*</span><br />
                <input id="SpouseRelationship" readonly disabled type="text" value="<%:Model.GetSelectSpouseRelation() %>" class="sd" />

            </td>

        </tr>


        <tr>
            <td colspan="2">
                <label class="lblSpan">Relationship</label><span class="span-align">*</span><br />
                <%-- if (Model.ContactFlag == "Referral")
                  { %>
                <%=Html.DropDownListFor(m=>m.Relation,Model.RelationList,new { @class="validate[required] inactive",ID="noActive"}) %></td>




            <%}

                  else
                  {
                --%>
                <%=Html.DropDownListFor(m=>m.Relation,Model.RelationList,new { @class="validate[required]",ID="ddlPrntRel",onChange = "displayDiv()"}) %>




                <%--} --%>
            <td colspan="2">
                <label class="lblSpan">Primary Language</label><span class="nospan-align">*</span><br />
                <%=Html.TextBoxFor(m => m.PrimaryLanguage, Model.PrimaryLanguage, new {  @class="sd",maxlength=50 })%></td>
        </tr>
        <tr>
            <td colspan="2">
                <label class="lblSpan">Relationship Description</label><span class="nospan-align">*</span><br />
                <%=Html.TextBoxFor(m => m.Spouse, Model.Spouse, new {  @class="sd",maxlength=50 })%></td>


            <td colspan="2">
                <%if (Model.Id == 0)
                  { %>
                <div id="shwtxt" style="display: none;">
                    <table style="width: 100%">
                        <tr>
                            <td colspan="2">
                                <label class="lblSpan">Username</label><span class="span-align">*</span><br />
                                <%=Html.TextBoxFor(m => m.UserID,new { @class = "validate[required,custom[checkUsername]]", ID = "ddlDocumentTypeTxt",value=Model.UserID })%></td>

                        </tr>
                    </table>
                </div>
                <%}
                  else
                  {%>
                <div id="shwtxt" style="display: none;">
                    <table style="width: 100%">
                        <tr>
                            <td colspan="2">
                                <label class="lblSpan">Username</label><span class="span-align">*</span><br />
                                <%=Html.TextBoxFor(m => m.UserID,new { @class = "validate[required,custom[checkUsername]]", ID = "ddlDocumentTypeTxt",value=Model.UserID })%></td>

                        </tr>
                    </table>
                </div>
                <%if (Model.UserID != "")
                  { %>
                <input id="hdnUser" type="hidden" value="<%=Model.UserID %>" />
                <div id="Div1" style="display: block;">
                    <table style="width: 100%">
                        <tr>
                            <td colspan="2">
                                <label class="lblSpan">Username</label><span class="nospan-align">*</span><br />
                                <%=Html.LabelFor(m=>m.UserID,Model.UserID, new {  @class="sd",maxlength=50 }) %></td>
                        </tr>
                    </table>
                </div>
                <%}
                  } %>
            </td>
        </tr>
        <tr>
            <td colspan="1">
                <label class="lblSpan">Guardian</label><span class="nospan-align">*</span><br />
                <%=Html.CheckBoxFor(x=>x.IsGuardian)%>

            </td>
            <td colspan="1">
                <label class="lblSpan">Custody</label><span class="nospan-align">*</span><br />
                <%=Html.CheckBoxFor(x=>x.IsCustody)%>

            </td>
            <td colspan="1">
                <label class="lblSpan">Next of Kin</label><span class="nospan-align">*</span><br />
                <%=Html.CheckBoxFor(x=>x.IsNextOfKin)%>

            </td>
            
            <td colspan="1">
                <label class="lblSpan">School Contact</label><span class="nospan-align">*</span><br />
                <%=Html.CheckBoxFor(x=>x.IsSchool)%>
            </td>
       
        </tr>
        <tr>
            <td colspan="1">
                <label class="lblSpan">Emergency Contact - Primary</label><span class="nospan-align">*</span><br />
                <%=Html.CheckBoxFor(x=>x.IsEmergencyP,new { @onclick = "CheckP()"})%>

            </td>
            <td colspan="1">
                <label class="lblSpan">Emergency Contact - Secondary</label><span class="nospan-align">*</span><br />
                <%=Html.CheckBoxFor(x=>x.IsEmergencyS,new { @onclick = "CheckS()"})%>

            </td>
            <td colspan="1">
                <label class="lblSpan">Emergency Contact - Tertiary</label><span class="nospan-align">*</span><br />
                <%=Html.CheckBoxFor(x=>x.IsEmergencyT,new { @onclick = "CheckT()"})%>

            </td>
            <td colspan="1">
                <label class="lblSpan">Emergency Contact - Other</label><span class="nospan-align">*</span><br />
                <%=Html.CheckBoxFor(x=>x.IsEmergencyO,new { @onclick = "CheckO()"})%>

            </td>
            </tr>
        <tr>
            <td colspan="1">
                <label class="lblSpan">Incident Contact</label><span class="nospan-align">*</span><br />
                <%=Html.CheckBoxFor(x=>x.IsIncident)%>

            </td>
            <td colspan="1">
                <label class="lblSpan">Correspondence Contact</label><span class="nospan-align">*</span><br />
                <%=Html.CheckBoxFor(x=>x.IsCorrespondence)%>
            </td>
            <td colspan="1">
                <label class="lblSpan">Billing Contact</label><span class="nospan-align">*</span><br />
                <%=Html.CheckBoxFor(x=>x.IsBilling)%>
            </td>
        </tr>
        
        <tr style="display:none">
            
             <td colspan="1">
                <label class="lblSpan">Approved Visitor</label><span class="nospan-align">*</span><br />
                 <%=Html.CheckBoxFor(x=>x.ApprovedVisitor)%>

            </td>
        </tr>
        <tr>
           <td><br /></td> 
        </tr>
        <tr>
            <%--<td>
                 <label class="lblSpan">On Campus, With Staff</label><span class="nospan-align">*</span><br />
                <%=Html.CheckBoxFor(x=>x.IsOnCampusWithStaff)%>
            </td>
           
        
            <td>
                 <label class="lblSpan">On Campus, Alone</label><span class="nospan-align">*</span><br />
                <%=Html.CheckBoxFor(x=>x.IsOnCampusAlone)%>
            </td>
            
       
            <td>
                 <label class="lblSpan">Off Campus</label><span class="nospan-align">*</span><br />
                <%=Html.CheckBoxFor(x=>x.IsOffCampus)%>
            </td>--%>
            <td colspan="4">
            <fieldset>
                    <legend style="font-size:12px;"><b>Approved Visitor</b></legend>
                    
                 
                
                 
                
              
                
                <table>
                    <tr>
                        <td>
                            <label class="lblSpan">On Campus, With Staff</label><span class="nospan-align">*</span>
                            <br /><%=Html.CheckBoxFor(x => x.IsOnCampusWithStaff, new { onclick="CheckboxApprovdVisitor();"})%>
                        </td>
                        <td>
                            <label class="lblSpan" style="padding-left: 100px" >On Campus, Alone</label><span class="nospan-align">*</span>
                            <br /><%=Html.CheckBoxFor(x=>x.IsOnCampusAlone,new { @class="",@style="margin-left: 105px;" ,onclick="CheckboxApprovdVisitor();" })%>
                        </td>
                        <td>
                            <label class="lblSpan" style="padding-left: 90px" >Off Campus</label><span class="nospan-align">*</span><br />
                            <%=Html.CheckBoxFor(x=>x.IsOffCampus,new { @class="",@style="margin-left: 95px;" ,onclick="CheckboxApprovdVisitor();" })%>
                        </td>
                    </tr>
                </table>
                

                </fieldset>
            </td>
        </tr>

        <tr>
            <td colspan="4">
                <label class="lblSpan">Contact Notes</label><span class="nospan-align">*</span><br />
                <%=Html.TextAreaFor(m => m.Note,new {value=@Model.Note,  @class="sd", @rows="3", @cols="160" })%>
            </td>
        </tr>

        <tr>
            <td></td>
            <td></td>
            <td><%--Contact For--%></td>
            <td><%foreach (var item in Model.checkbox)
                  {%>
                <div style="float: left; display: none"><%=@Html.CheckBox("getcheked", item.check, new {value=item.name }) %> <%=item.name %></div>
                <%}%></td>

        </tr>
        <tr>
            <td colspan="4">
            <div id="ContactSection">
                <div id="Cntactmenu" style="width: 15%; margin-left: 95px; margin-bottom: -4px">

                    <table style="margin-bottom: -2px" width="100%" border="0" cellpadding="0" cellspacing="0">
                        <tr>
                            <td class="nomarg">
                                <input id="btnHome" title="Home" type="button" value="Home" onclick="populate(this);" style="border-bottom: none; margin: 10px 0 2px 0; background-color: #F0F0F0" /></td>
                            <td class="nomarg">
                                <input id="btnWork" title="Work" type="button" value="Work" onclick="populate(this);" style="margin: 10px 0 2px 0; background-color: white" /></td>
                            <td class="nomarg">
                                <input id="btnOther" title="Other" type="button" value="Other" onclick="populate(this);" style="margin: 10px 0 2px 0; background-color: white" /></td>

                        </tr>
                    </table>

                </div>
                <div id="contactHome">

                    <table style="border: 1px solid #F0F0F0; background: none repeat scroll 0 0 #F0F0F0;" width="100%" cellpadding="0" cellspacing="0" style="background: #F0F0F0">

                        <tr>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <label class="lblSpan"><b>Address</b></label><span class="greyspan-align">*</span></td>


                            <td colspan="2">

                                <label class="lblSpan"><b>Phone</b></label><span class="greyspan-align">*</span></td>

                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                        </tr>

                        <tr>
                            <td colspan="2">

                                <label class="lblSpan">Address Type</label><span class="greyspan-align">*</span><br />
                                <%=Html.DropDownListFor(m=>m.HomeAddressTypeId,Model.HomeAddressTypeList,new { ID="ddlAddressListHome"}) %></td>
                            <td colspan="2">

                                <table>
                                    <tr>
                                        <td>
                                            <label class="lblSpan">Home</label><span class="greyspan-align">*</span><br />
                                            <%=Html.TextBoxFor(m => m.HomePhone, Model.HomePhone, new {  @class="validate[custom[usPhoneNumber]] usPhone",onpaste="funPaste(event);"})%></td>
                                        <%--<td>
                                                <label class="lblSpan">Ext</label><span class="greyspan-align">*</span><br />
                                            <%=Html.TextBoxFor(m=>m.HomeExtension,Model.HomeExtension,new { @class="",@style="max-width: 25px;"}) %>

                                            </td>--%>
                                    </tr>

                                </table>


                            </td>

                        </tr>

                        <tr>
                            <td colspan="2">
                                <label class="lblSpan">Street</label><span class="greyspan-align">*</span><br />
                                <%=Html.TextBoxFor(m => m.HomeAddressLine2, Model.HomeAddressLine2, new {  @class="sd",maxlength=50 })%></td>
                            <td colspan="2">
                                <label class="lblSpan">Mobile</label><span class="greyspan-align">*</span><br />
                                <%=Html.TextBoxFor(m => m.HomeMobilePhone, Model.HomeMobilePhone, new {  @class="validate[custom[usPhoneNumber]] usPhone",onpaste="funPaste(event);" })%>
                        </tr>

                        <tr>
                            <td colspan="2">
                                <label class="lblSpan">Unit #</label><span class="greyspan-align">*</span><br />
                                <%=Html.TextBoxFor(m => m.HomeAddressLine1, Model.HomeAddressLine1, new { maxlength=50 })%></td>

                            <td colspan="2">
                                <label class="lblSpan">Fax</label><span class="greyspan-align">*</span><br />
                                <%=Html.TextBoxFor(m => m.HomeFax, Model.HomeFax, new {  @class="validate[custom[usFaxNumber]] usPhone",onpaste="funPaste(event);" })%>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <label class="lblSpan">City</label><span class="greyspan-align">*</span><br />
                                <%=Html.TextBoxFor(m => m.HomeCity, Model.HomeCity, new {maxlength=50 })%>
                            </td>
                            <td colspan="2">
                                <table>
                                    <tr>
                                        <td>
                                            <label class="lblSpan">Other</label><span class="greyspan-align">*</span><br />
                                            <%=Html.TextBoxFor(m => m.HomeWorkPhone, Model.HomeWorkPhone, new {  @class="validate[custom[usPhoneNumber]] usPhone",onpaste="funPaste(event);"})%> </td>
                                        <td>
                                            <label class="lblSpan">Ext</label><span class="greyspan-align">*</span><br />
                                            <%=Html.TextBoxFor(m=>m.HomeExtension,Model.HomeExtension,new { @class="usPhoneExt",@style="max-width: 35px;"}) %>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <label class="lblSpan">State</label><span class="greyspan-align">*</span><br />
                                <%=Html.DropDownListFor(m => m.HomeState, Model.HomeStateList, new {  @class="sd", ID="ddlHomeState"})%></td>
                            <td colspan="2">
                                <label class="lblSpan"><b>Email</b></label><span class="greyspan-align">*</span></td>

                        </tr>

                        <tr>
                            <td colspan="2">
                                <label class="lblSpan">Zip Code</label><span class="greyspan-align">*</span><br />
                                <%=Html.TextBoxFor(m => m.HomeZip, Model.HomeZip, new {  @class="sd zipDefault",onpaste="funPaste(event);",maxlength=5 })%></td>
                            <td colspan="2">
                                <label class="lblSpan">Home</label><span class="greyspan-align">*</span><br />
                                <%=Html.TextBoxFor(m => m.HomeEmail, Model.HomeEmail, new {  @class="validate[custom[email]]", })%></td>
                        </tr>

                        <tr>
                            <td colspan="2">
                                <label class="lblSpan">County</label><span class="greyspan-align">*</span><br />
                                <%=Html.TextBoxFor(m=>m.HomeCounty,Model.HomeCounty,new { @class="sd", ID="ddlHomeCounty"}) %>
                            </td>
                            <td colspan="2">
                                <label class="lblSpan">Other</label><span class="greyspan-align">*</span><br />
                                <%=Html.TextBoxFor(m => m.HomeOtherMail, Model.HomeOtherMail, new {  @class="validate[custom[email]]", })%></td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <label class="lblSpan">Country</label><span class="greyspan-align">*</span><br />
                                <%=Html.DropDownListFor(m=>m.HomeCountry,Model.HomeCountryList,new { @class="sd", ID="ddlHomeCountry"}) %></td>


                        </tr>

                        <tr>
                            <td colspan="1">
                                <label class="lblSpan">Mail Opt-In</label><span class="greyspan-align">*</span><br />
                                <%:Html.CheckBoxFor(x=>x.HomeMailOptIn) %>
                            </td>
                            <td colspan="1">
                                <label class="lblSpan">Email Opt-In</label><span class="greyspan-align">*</span><br />
                                <%:Html.CheckBoxFor(x=>x.HomeEmailOptIn) %>
                            </td>
                            <td colspan="1">
                                <label class="lblSpan">Phone Opt-In</label><span class="greyspan-align">*</span><br />
                                <%:Html.CheckBoxFor(x=>x.HomePhoneOptIn) %>
                            </td>
                            <td colspan="1">
                                <label class="lblSpan">Mailing Address</label><span class="greyspan-align">*</span><br />
                                <%:Html.CheckBoxFor(x=>x.HomeIsMailingAddress) %>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="1">
                                <label class="lblSpan">Mail Merge Opt-In</label><span class="greyspan-align">*</span><br />
                                <%:Html.CheckBoxFor(x=>x.HomeMailMergeOptIn) %>
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="contactWork">
                    <table style="border: 1px solid #F0F0F0; background: none repeat scroll 0 0 #F0F0F0;" width="100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <label class="lblSpan">Organization</label><span class="greyspan-align"></span><br />
                                <%--<%=Html.TextBoxFor(m=>m.Employer,Model.Employer,new { @class="sd"}) %>--%>
                                <%=Html.TextAreaFor(m => m.Employer  ,new {value=@Model.Employer ,  @class="sd", @rows="3",@cols="30"})%></td>
                            <td colspan="2">
                                <label class="lblSpan">Occupation</label><span class="greyspan-align"></span><br />
                                <%=Html.TextBoxFor(m=>m.Occupation,Model.Occupation,new { @class="sd"}) %></td>

                        </tr>
                        <tr>
                            <td colspan="2">
                                <label class="lblSpan"><b>Address</b></label><span class="greyspan-align">*</span></td>



                            <td colspan="2">
                                <label class="lblSpan"><b>Phone</b></label><span class="greyspan-align">*</span></td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <label class="lblSpan">Address Type</label><span class="greyspan-align">*</span><br />
                                <%=Html.DropDownListFor(m=>m.WorkAddressTypeId,Model.WorkAddressTypeList,new { @class="sd", ID="ddlAddressListWork"}) %></td>
                            <td colspan="2">
                                <table>
                                    <tr>
                                        <td>
                                            <label class="lblSpan">Primary Contact</label><span class="greyspan-align">*</span><br />
                                            <%=Html.TextBoxFor(m => m.WorkHomePhone, Model.WorkHomePhone, new {  @class="validate[custom[usPhoneNumber]] usPhone",onpaste="funPaste(event);" })%>
                                                -
                                        </td>

                                        <td colspan="2">
                                            <label class="lblSpan">Ext</label><span class="greyspan-align">*</span><br />
                                            <%=Html.TextBoxFor(m=>m.WorkExtension,Model.WorkExtension,new {@class="usPhoneExt",@style="max-width: 35px;"}) %></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>

                        <tr>
                            <td colspan="2">
                                <label class="lblSpan">Street</label><span class="greyspan-align">*</span><br />
                                <%=Html.TextBoxFor(m => m.WorkAddressLine2, Model.WorkAddressLine2, new {  @class="sd",maxlength=50 })%></td>
                            <td colspan="2">
                                <label class="lblSpan">Mobile</label><span class="greyspan-align">*</span><br />
                                <%=Html.TextBoxFor(m => m.WorkMobilePhone, Model.WorkMobilePhone, new {  @class="validate[custom[usPhoneNumber]] usPhone",onpaste="funPaste(event);" })%>
                            </td>
                        </tr>

                        <tr>
                            <td colspan="2">
                                <label class="lblSpan">Unit #</label><span class="greyspan-align">*</span><br />
                                <%=Html.TextBoxFor(m => m.WorkAddressLine1, Model.WorkAddressLine1, new {maxlength=50 })%></td>
                            <td colspan="2">
                                <label class="lblSpan">Fax</label><span class="greyspan-align">*</span><br />
                                <%=Html.TextBoxFor(m => m.WorkFax, Model.WorkFax, new {  @class="validate[custom[usFaxNumber]] usPhone",onpaste="funPaste(event);" })%>
                            </td>
                        </tr>



                        <tr>
                            <td colspan="2">
                                <label class="lblSpan">City</label><span class="greyspan-align">*</span><br />
                                <%=Html.TextBoxFor(m => m.WorkCity, Model.WorkCity, new {  @class="sd",maxlength=50 })%></td>
                            <td colspan="2">
                                <table>
                                    <tr>
                                        <td>
                                            <label class="lblSpan">Other</label><span class="greyspan-align">*</span><br />
                                            <%=Html.TextBoxFor(m => m.WorkPhone, Model.WorkPhone, new {  @class="validate[custom[usPhoneNumber]] usPhone",onpaste="funPaste(event);" })%> - 
                                        </td>
                                        <td colspan="2">
                                            <label class="lblSpan">Ext</label><span class="greyspan-align">*</span><br />
                                            <%=Html.TextBoxFor(m=>m.OtherExtension2,Model.OtherExtension2,new {@class="usPhoneExt",@style="max-width: 35px;"}) %></td>

                                    </tr>
                                </table>
                            </td>
                        </tr>

                        <tr>
                            <td colspan="2">
                                <label class="lblSpan">State</label><span class="greyspan-align">*</span><br />
                                <%=Html.DropDownListFor(m => m.WorkState, Model.WorkStateList, new {  @class="sd",  ID="ddlWorkState"})%></td>
                            <td colspan="2">
                                <label class="lblSpan"><b>Email</b></label><span class="greyspan-align">*</span>
                        </tr>

                        <tr>
                            <td colspan="2">
                                <label class="lblSpan">Zip Code</label><span class="greyspan-align">*</span><br />
                                <%=Html.TextBoxFor(m => m.WorkZip, Model.WorkZip, new {  @class="sd zipDefault",onpaste="funPaste(event);",maxlength=5 })%></td>
                            <td colspan="2">
                                <label class="lblSpan">Work</label><span class="greyspan-align">*</span><br />
                                <%=Html.TextBoxFor(m => m.WorkEmail, Model.WorkEmail, new {  @class="validate[custom[email]]", })%>
                            </td>
                        </tr>

                        <tr>
                            <td colspan="2">
                                <label class="lblSpan">County</label><span class="greyspan-align">*</span><br />
                                <%=Html.TextBoxFor(m=>m.WorkCounty,Model.WorkCounty,new { @class="sd", ID="ddlHomeCounty"}) %>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <label class="lblSpan">Country</label><span class="greyspan-align">*</span><br />
                                <%=Html.DropDownListFor(m=>m.WorkCountry,Model.WorkCountryList,new { @class="sd", ID="ddlWorkCountry"}) %></td>


                        </tr>

                        <tr>
                            <td colspan="1">
                                <label class="lblSpan">Mail Opt-In</label><span class="greyspan-align">*</span><br />
                                <%:Html.CheckBoxFor(x=>x.WorkMailOptIn) %>
                            </td>
                            <td colspan="1">
                                <label class="lblSpan">Email Opt-In</label><span class="greyspan-align">*</span><br />
                                <%:Html.CheckBoxFor(x=>x.WorkEmailOptIn) %>
                            </td>
                            <td colspan="1">
                                <label class="lblSpan">Phone Opt-In</label><span class="greyspan-align">*</span><br />
                                <%:Html.CheckBoxFor(x=>x.WorkPhoneOptIn) %>
                            </td>
                            <td colspan="1">
                                <label class="lblSpan">Mailing Address</label><span class="greyspan-align">*</span><br />
                                <%:Html.CheckBoxFor(x=>x.WorkIsMailingAddress) %>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="1">
                                <label class="lblSpan">Mail Merge Opt-In</label><span class="greyspan-align">*</span><br />
                                <%:Html.CheckBoxFor(x=>x.WorkMailMergeOptIn) %>
                            </td>
                        </tr>

                    </table>
                </div>
                <div id="contactOther">
                    <table style="border: 1px solid #F0F0F0; background: none repeat scroll 0 0 #F0F0F0;" width="100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <label class="lblSpan"><b>Address</b></label><span class="greyspan-align">*</span></td>
                            <td colspan="2">
                                <label class="lblSpan"><b>Phone</b></label><span class="greyspan-align">*</span></td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <label class="lblSpan">Address Type</label><span class="greyspan-align">*</span><br />
                                <%=Html.DropDownListFor(m=>m.OtherAddressTypeId,Model.OtherAddressTypeList,new { @class="sd", ID="ddlAddressOther"}) %>
                            </td>
                            <td colspan="2">

                                <table>
                                    <tr>
                                        <td>
                                            <label class="lblSpan">Primary Contact</label><span class="greyspan-align">*</span><br />
                                            <%=Html.TextBoxFor(m => m.OtherHomePhone, Model.OtherHomePhone, new {  @class="validate[custom[usPhoneNumber]] usPhone",onpaste="funPaste(event);" })%>
                                                -
                                        </td>
                                        <td colspan="2">
                                            <label class="lblSpan">Ext</label><span class="greyspan-align"></span><br />
                                            <%=Html.TextBoxFor(m=>m.OtherExtension,Model.OtherExtension,new { @class="usPhoneExt",@style="max-width: 35px;"}) %></td>
                                    </tr>
                                </table>

                        </tr>

                        <tr>
                            <td colspan="2">
                                <label class="lblSpan">Street</label><span class="greyspan-align">*</span><br />
                                <%=Html.TextBoxFor(m => m.OtherAddressLine2, Model.OtherAddressLine2, new {  @class="sd",maxlength=50 })%>
                            </td>
                            <td colspan="2">
                                <label class="lblSpan">Mobile</label><span class="greyspan-align">*</span><br />
                                <%=Html.TextBoxFor(m => m.OtherMobilePhone, Model.OtherMobilePhone, new {  @class="validate[custom[usPhoneNumber]] usPhone",onpaste="funPaste(event);" })%>
                            </td>
                        </tr>

                        <tr>
                            <td colspan="2">
                                <label class="lblSpan">Unit #</label><span class="greyspan-align">*</span><br />
                                <%=Html.TextBoxFor(m => m.OtherAddressLine1, Model.OtherAddressLine1, new {maxlength=50 })%></td>
                            <td colspan="2">
                                <label class="lblSpan">Fax</label><span class="greyspan-align">*</span><br />
                                <%=Html.TextBoxFor(m => m.OtherFax, Model.OtherFax, new {  @class="validate[custom[usFaxNumber]] usPhone",onpaste="funPaste(event);" })%>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <label class="lblSpan">City</label><span class="greyspan-align">*</span><br />
                                <%=Html.TextBoxFor(m => m.OtherCity, Model.OtherCity, new {  @class="sd",maxlength=50 })%></td>
                            <td colspan="2">
                                <table>
                                    <tr>
                                        <td>
                                            <label class="lblSpan">Other</label><span class="greyspan-align">*</span><br />
                                            <%=Html.TextBoxFor(m => m.OtherWorkPhone, Model.OtherWorkPhone, new {  @class="validate[custom[usPhoneNumber]] usPhone",onpaste="funPaste(event);" })%>
                                        <td colspan="2">
                                            <label class="lblSpan">Ext</label><span class="greyspan-align">*</span><br />
                                            <%=Html.TextBoxFor(m=>m.OtherExtension3,Model.OtherExtension3,new {@class="usPhoneExt",@style="max-width: 35px;"}) %></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>


                        <tr>
                            <td colspan="2">
                                <label class="lblSpan">State</label><span class="greyspan-align">*</span><br />
                                <%=Html.DropDownListFor(m => m.OtherState, Model.OtherStateList, new {  @class="sd",  ID="ddlOtherState"})%></td>
                        </tr>

                        <tr>
                            <td colspan="2">
                                <label class="lblSpan">Zip Code</label><span class="greyspan-align">*</span><br />
                                <%=Html.TextBoxFor(m => m.OtherZip, Model.OtherZip, new {  @class="sd zipDefault",onpaste="funPaste(event);",maxlength=5 })%></td>
                        </tr>

                        <tr>
                            <td colspan="2">
                                <label class="lblSpan">County</label><span class="greyspan-align">*</span><br />
                                <%=Html.TextBoxFor(m=>m.OtherCounty,Model.OtherCounty,new { @class="sd", ID="ddlHomeCounty"}) %>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <label class="lblSpan">Country</label><span class="greyspan-align">*</span><br />
                                <%=Html.DropDownListFor(m=>m.OtherCountry,Model.OtherCountryList,new { @class="sd", ID="ddlOtherCountry"}) %></td>


                        </tr>

                        <tr>
                            <td colspan="1">
                                <label class="lblSpan">Mail Opt-In</label><span class="greyspan-align">*</span><br />
                                <%:Html.CheckBoxFor(x=>x.OtherMailOptIn) %>
                            </td>
                            <td colspan="1">
                                <label class="lblSpan">Email Opt-In</label><span class="greyspan-align">*</span><br />
                                <%:Html.CheckBoxFor(x=>x.OtherEmailOptIn) %>
                            </td>
                            <td colspan="1">
                                <label class="lblSpan">Phone Opt-In</label><span class="greyspan-align">*</span><br />
                                <%:Html.CheckBoxFor(x=>x.OtherPhoneOptIn) %>
                            </td>
                            <td colspan="1">
                                <label class="lblSpan">Mailing Address</label><span class="greyspan-align">*</span><br />
                                <%:Html.CheckBoxFor(x=>x.OtherIsMailingAddress) %>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="1">
                                <label class="lblSpan">Mail Merge Opt-In</label><span class="greyspan-align">*</span><br />
                                <%:Html.CheckBoxFor(x=>x.OtherMailMergeOptIn) %>
                            </td>
                        </tr>

                    </table>
                </div>
                <table>
                    <tr>

                        <td>
                            <% if (ViewBag.permission == "true")
                               {
                                   //if (Session["PlacementStat"].ToString() != "I")
                                   %>
                            <input id="btnSaveContact" type="submit" value="Save" onclick="" /></td>
                        <td>
                            <input id="btnUpdateContact" type="submit" value="Update" onclick="" style="display: none" />
                            <%} %>
                        </td>
                        <td>
                            <%-- <input id="btnList" type="submit" value="List" onclick=""  />--%></td>

                    </tr>
                </table>
            </div>
            </td>
            </tr>

        </table>
    </div>
            <%=Html.TextAreaFor(m => m.eventLogNote,new {Style="width:200px; height:200px;display:none; position:fixed; top:0px; left:0px;", ID="tickerBox"})%>
            <%} %>
            <script type="text/javascript">

                var Arr_Record = [];
                var Arr_Ticker = [];
                var Arr_NewEventLogsParent = [];
                var Arr_NewEventLogsChild = [];

                $(document).ready(function () {

                    staffNameAutocomplete();

                });

                function staffNameAutocomplete() {
                    
                    $("#ddlDocumentTypeTxt").autocomplete({
                        source: function (request, response) {
                            $.ajax({
                                url: "../Forms/searchParentFirstName",
                                type: "POST",
                                dataType: "json",
                                data: { term: request.term },
                                success: function (data) {
                                    if ($(data).length == 0) {
                                        $('#ddlDocumentTypeTxt').css('border-color', 'Green');
                                    }
                                    else {
                                        $('#ddlDocumentTypeTxt').css('border-color', 'Red');
                                    }
                                },
                            })
                        },
                        messages: {
                            noResults: '',
                            results: function (resultsCount) { }
                        }
                    });
                }

                $('input,select,textarea').focus(function () {
                    var controlId = this.id;
                    var controlType = this.type;
                    var prevValue = "";
                    var newValue = "";
                    var labelText = $(this).siblings('.lblSpan').text();
                    var message = "";

                    switch (controlType) {

                        case "text":
                            prevValue = $(this).val();
                            break;
                        case "textarea":
                            prevValue = $(this).val();
                            break;
                        case "select-one":
                            prevValue = $("#" + controlId + " option:selected").text();
                            break;
                        case "checkbox":
                            prevValue = ($(this).prop('checked')) ? "Checked" : "Unchecked";
                            break;
                    }
                    // alert(controlType);
                    Arr_Record = [controlId, controlType, labelText, prevValue, newValue, message];
                    Arr_NewEventLogsChild = [labelText, prevValue, newValue];
                });
                $('input,select,textarea').blur(function () {
                    var controlId = this.id;
                    var controlType = this.type;
                    var prevValue = Arr_Record[3];
                    var newValue = "";
                    var labelText = $(this).siblings('.lblSpan').text();
                    var message = "";
                    var newEvent = "";

                    switch (controlType) {

                        case "text":
                            newValue = $(this).val();
                            break;
                        case "textarea":
                            newValue = $(this).val();
                            break;
                        case "select-one":
                            newValue = $("#" + controlId + " option:selected").text();
                            break;
                        case "checkbox":
                            newValue = ($(this).prop('checked')) ? "Checked" : "Unchecked";
                            break;
                    }

                    if (controlId == Arr_Record[0]) {

                        if (prevValue == newValue) {
                            Arr_Record = [];
                            Arr_NewEventLogsChild = [];
                        }
                        else {
                            if (isInitialValue(controlId, newValue)) {
                                if (controlType == "textarea") {
                                    message = '[' + labelText + ']' + ' was changed.';
                                }
                                else {
                                    message = '[' + labelText + ']' + ' was changed from "' + prevValue + '" to "' + newValue + '".';
                                }
                                Arr_Record[4] = newValue;
                                Arr_Record[5] = message;
                                Arr_Ticker.push(Arr_Record);
                                Arr_Record = [];

                                var newControlType = "";
                                if (controlType == "select-one") {
                                    newControlType = "dropdownlist";
                                }
                                else {
                                    newControlType = controlType;
                                }

                                var pageType = "Contact";
                                if (labelText == "Guardian") {
                                    pageType = "Guardianship";
                                }
                                if (labelText == "Billing Contact") {
                                    pageType = "Funder";
                                }

                                Arr_NewEventLogsChild = [pageType, newControlType, labelText, prevValue, newValue];
                                var flag = 0;

                                if (Arr_NewEventLogsParent.length == 0) {
                                    Arr_NewEventLogsParent.push(Arr_NewEventLogsChild);
                                }
                                else {
                                    for (var i = 0; i < Arr_NewEventLogsParent.length; i++) {
                                        if (Arr_NewEventLogsParent[i][2] == labelText) {
                                            Arr_NewEventLogsParent[i][4] = newValue;
                                            flag = 1;
                                        }
                                    }
                                    if (flag == 0) {
                                        Arr_NewEventLogsParent.push(Arr_NewEventLogsChild);
                                    }
                                }
                                Arr_NewEventLogsChild = [];
                            }
                        }
                    }
                    writeToTickerBox(Arr_Ticker);
                });

                function isInitialValue(controlId, newVal) {
                    for (var i = 0; i < Arr_Ticker.length; i++) {
                        if (Arr_Ticker[i][0] == controlId) {
                            if (Arr_Ticker[i][3] == newVal) {
                                Arr_Ticker.splice(i, 1);
                                Arr_NewEventLogsParent.splice(i, 1);
                                return false;
                            }
                        }

                    }

                    return true;
                }

                function writeToTickerBox(arrToWrite) {

                    var logString = "";
                    for (var i = 0; i < arrToWrite.length; i++) {

                        logString += (i + 1) + ") " + arrToWrite[i][5] + "\n";
                    }
                    //if (logString != "") {
                    $('#tickerBox').val(logString);
                    // }
                }

                function WriteToString(para_Arr_NewEventLogsParent) {
                    document.getElementById('newEventLog').value = "";
                    //for (var i = 0; i < para_Arr_NewEventLogsParent.length; i++) {
                        //alert(para_Arr_NewEventLogsParent[i]);
                    //}
                    for (var i = 0; i < para_Arr_NewEventLogsParent.length; i++) {
                        newEvent = para_Arr_NewEventLogsParent[i][0] + "|||" + para_Arr_NewEventLogsParent[i][1] + "|||" + para_Arr_NewEventLogsParent[i][2] + "|||" + para_Arr_NewEventLogsParent[i][3] + "|||" + para_Arr_NewEventLogsParent[i][4];
                        document.getElementById('newEventLog').value += newEvent + ">>>";
                    }
                }
                function CheckP() {
                    if ($('#IsEmergencyP').prop("checked") == true)
                    {
                        $('#IsEmergencyS').prop("checked", false);
                        $('#IsEmergencyT').prop("checked", false);
                        $('#IsEmergencyO').prop("checked", false);
                    }
                }
                function CheckS() {
                    if ($('#IsEmergencyS').prop("checked") == true)
                    {
                        $('#IsEmergencyP').prop("checked", false);
                        $('#IsEmergencyT').prop("checked", false);
                        $('#IsEmergencyO').prop("checked", false);
                    }
                }
                function CheckT() {
                    if ($('#IsEmergencyT').prop("checked") == true)
                    {
                        $('#IsEmergencyS').prop("checked", false);
                        $('#IsEmergencyP').prop("checked", false);
                        $('#IsEmergencyO').prop("checked", false);
                    }
                }
                function CheckO() {
                    if ($('#IsEmergencyO').prop("checked") == true)
                    {
                        $('#IsEmergencyS').prop("checked", false);
                        $('#IsEmergencyT').prop("checked", false);
                        $('#IsEmergencyP').prop("checked", false);
                    }
                    }

            </script>

            