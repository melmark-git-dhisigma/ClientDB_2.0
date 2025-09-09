<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ClientDB.Models.ClientRegistrationPAModel>" %>

<%--  Comment out by benhur.. because these are loaded in index page
<link href="../../Documents/CSS/jquery-ui.css" rel="stylesheet" />
<script src="../../Documents/JS/jquery-ui-1.8.24.js"></script>
<script src="../../Documents/JS/jquery.form.js"></script> --%>
<%--<script src="../Documents/JS/jquery.mask.js"></script>--%>
<%--<script src="../../Documents/JS/jquery.form.js"></script>--%>

<%--<script src="../../Documents/JS/jquery.validationEngine-en.js"></script>
<script src="../../Documents/JS/jquery.validationEngine.js"></script>--%>




<style type="text/css">
    .auto-style1 {
        font-size: x-small;
    }
    /*#rightSidePanel {
        margin-top: -2548px !important;
    }*/
    .auto-style2 {
    }



    select {
        width: 167px !important;
        border: 1px solid #CCC;
        height: 29px;
        vertical-align: middle;
        font-family: Arial;
        font-size: 12px;
        color: #333;
        margin-bottom: 8px;
    }

    #rightSidePanel {
        border: 1px solid;
        /*margin: 0 15px 0 90% !important;*/
        padding: 0px;
        /*position: absolute;*/
        /*right: 0;*/
        /*top: 27.5%;*/
        border-radius: 2px;
        z-index: 999;
        /*width: 215px !important;*/
    }

    #leftinnerdiv {
        width: 66.5% !important;
    }

    .ui-datepicker select.ui-datepicker-month, .ui-datepicker select.ui-datepicker-year {
        width: 49% !important;
    }

    .lblSpan {
        font-style: italic;
        font-weight: bold;
    }

    #newPreview, #newPreview1 {
        filter: progid:DXImageTransform.Microsoft.AlphaImageLoader(sizingMethod='scale');
        height: 180px;
        width: 180px;
        display: none;
    }

    #cancelPreview {
        text-decoration: underline;
        color: red;
        cursor: pointer;
        text-align: center;
        width: 180px;
    }

    table tr td {
        vertical-align: top;
    }
</style>

<script type="text/javascript">
    $(document).ready(function () {

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
        $('.numbersOnlyT').keypress(function (event) {
            var inputValue = event.which;
            if ((inputValue >= 48 && inputValue <= 57)) {
            }
            else {
                event.preventDefault();
            }
        });

        $('.numbersOnlyTs').keypress(function (event) {
            var inputValue = event.which;
            if ((inputValue >= 48 && inputValue <= 57) || inputValue==46) {
                var number = ($(this).val().split('.'));
                if (number[1] && number[1].length > 0) {
                    event.preventDefault();
                }

            }
            else {
                event.preventDefault();
            }
        });
        $('.namefield').keypress(function (event) {
            var inputValue = event.which;
            if (((inputValue >= 65 && inputValue <= 90) || (inputValue >= 97 && inputValue <= 122) || (inputValue == 32) || (inputValue == 39) || (inputValue == 45) || (inputValue == 8) || (inputValue == 0))) {
            }
            else {
                event.preventDefault();
            }
        });

        $('.imgcontainer').css("display", "none");
        $("#dateOfBirth").datepicker({
            changeMonth: true,
            changeYear: true,
            showAnim: "fadeIn",
            yearRange: 'c-30:c+30',
            maxDate: new Date,


            /* fix buggy IE focus functionality */
            fixFocusIE: false,
            onSelect: function (dateText, inst) {
                var newValue = $(this).val();
                var controlId = $(this).attr('id');
                var labelText = $(this).siblings('.lblSpan').text();
                var prevValue = Arr_Record[3];

                if (isInitialValue(controlId, newValue)) {
                    message = '[' + labelText + ']' + ' was changed from "' + prevValue + '" to "' + newValue + '".';
                    Arr_Record[4] = newValue;
                    Arr_Record[5] = message;
                    Arr_Ticker.push(Arr_Record);
                    Arr_Record = [];
                    var pageType = "";

                    Arr_NewEventLogsChild = [pageType, "calendar", labelText, prevValue, newValue];
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
                writeToTickerBox(Arr_Ticker);
            }

            /* blur needed to correctly handle placeholder text */

            //onSelect: function (dateText, inst) {
            //    this.fixFocusIE = true;
            //    $(this).blur().change().focus();
            //},
            //onClose: function (dateText, inst) {
            //    this.fixFocusIE = true;
            //    this.focus();
            //},
            //beforeShow: function (input, inst) {
            //    var result = $.browser.msie ? !this.fixFocusIE : true;
            //    this.fixFocusIE = false;
            //    return result;
            //}
        }).attr('readonly', 'true');
        $("#dateUpdated,#AdmissinDate").datepicker({
            changeMonth: true,
            changeYear: true,
            showAnim: "fadeIn",
            yearRange: 'c-30:c+30',
			maxDate: "+10y",
            //maxDate: new Date,


            /* fix buggy IE focus functionality */
            fixFocusIE: false,
            onSelect: function (dateText, inst) {

                var newValue = $(this).val();
                var controlId = $(this).attr('id');
                var labelText = $(this).siblings('.lblSpan').text();
                var prevValue = Arr_Record[3];

                if (isInitialValue(controlId, newValue)) {

                    message = '[' + labelText + ']' + ' was changed from "' + prevValue + '" to "' + newValue + '".';

                    Arr_Record[4] = newValue;
                    Arr_Record[5] = message;
                    Arr_Ticker.push(Arr_Record);
                    Arr_Record = [];
                    var pageType = "";

                    Arr_NewEventLogsChild = [pageType, "calendar", labelText, prevValue, newValue];
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
                writeToTickerBox(Arr_Ticker);
            }
        }).attr('readonly', 'true');
    });

    $("#Photodate").datepicker({
        changeMonth: true,
        changeYear: true,
        showAnim: "fadeIn",
        yearRange: 'c-30:c+30',
        fixFocusIE: true
    });

    $('#ddlCountry').change(function () {
        var countryId = $('#ddlCountry').val();
        $.getJSON('../ClientRegistration/getStates', { countryid: countryId }, function (result) {
            var ddlState = $('#ddlState');
            $('#ddlState').empty();

            $.each(result, function (index, item) {
                ddlState.append($('<option/>', {
                    value: item.Value,
                    text: item.Text
                }));
            });
            //  $('#assistenceCityId').find(":selected").removeAttr('selected');
            $('#ddlState>option:eq(0)').attr('selected', true);
        });
    });
    $('#ddlState').change(function () {

    });

    jQuery("#registrationFormPA").validationEngine();

    var options = {
        success: showResponse  // post-submit callback 
    };
    $('#registrationFormPA').ajaxForm(options);

    $("#CurrentIEPStartDate").datepicker({
        changeMonth: true,
        changeYear: true,
        showAnim: "fadeIn",
        yearRange: 'c-30:c+30',
        maxDate: new Date,
        fixFocusIE: true
    });

    $('#CurrentIEPExpirationDate').datepicker({
        changeMonth: true,
        changeYear: true,
        showAnim: "fadeIn",
        yearRange: 'c-30:c+30',
        fixFocusIE: true,
    }).attr('readonly', 'true');

    $("#DateInitiallyEligibleforSpecialEducation").datepicker({
        changeMonth: true,
        changeYear: true,
        showAnim: "fadeIn",
        yearRange: 'c-30:c+30',
        maxDate: new Date,
        fixFocusIE: true
    });

    $("#DateofMostRecentSpecialEducationEvaluations").datepicker({
        changeMonth: true,
        changeYear: true,
        showAnim: "fadeIn",
        yearRange: 'c-30:c+30',
        maxDate: new Date,
        fixFocusIE: true
    });

    $("#DateofNextScheduled3YearEvaluation").datepicker({
        changeMonth: true,
        changeYear: true,
        showAnim: "fadeIn",
        yearRange: 'c-30:c+30',
        fixFocusIE: true
    });

    $("#DateFrom1").datepicker({
        changeMonth: true,
        changeYear: true,
        showAnim: "fadeIn",
        yearRange: 'c-30:c+30',
        fixFocusIE: true,
        onSelect: function (dateText, inst) {
            var dt2 = $('#DateTo1');
            var startDate = $(this).datepicker('getDate');
            startDate.setDate(startDate.getDate());
            var minDate = $(this).datepicker('getDate');
            dt2.datepicker('option', 'minDate', minDate);

        }
    }).attr('readonly', 'true');
    $('#DateTo1').datepicker({
        changeMonth: true,
        changeYear: true,
        showAnim: "fadeIn",
        yearRange: 'c-30:c+30',
        fixFocusIE: true,
        onSelect: function (dateText, inst) {
            var dt1 = $('#DateFrom1');
            var startDate = $(this).datepicker('getDate');
            dt1.datepicker('option', 'maxDate', startDate);
        }
    }).attr('readonly', 'true');

    $("#DischargeDate").datepicker({
        changeMonth: true,
        changeYear: true,
        showAnim: "fadeIn",
        yearRange: 'c-30:c+30',
        maxDate: new Date,


        /* fix buggy IE focus functionality */
        fixFocusIE: true
    });

    $(document).ready(function () {
        $("#DateFrom2").datepicker({
            changeMonth: true,
            changeYear: true,
            showAnim: "fadeIn",
            yearRange: 'c-30:c+30',
            fixFocusIE: true,
            onSelect: function (dateText, inst) {
                var dt2 = $('#DateTo2');
                var startDate = $(this).datepicker('getDate');
                startDate.setDate(startDate.getDate());
                var minDate = $(this).datepicker('getDate');
                dt2.datepicker('option', 'minDate', minDate);

            }
        }).attr('readonly', 'true');
        $('#DateTo2').datepicker({
            changeMonth: true,
            changeYear: true,
            showAnim: "fadeIn",
            yearRange: 'c-30:c+30',
            fixFocusIE: true,

            onSelect: function (dateText, inst) {
                var dt1 = $('#DateFrom2');
                var startDate = $(this).datepicker('getDate');
                dt1.datepicker('option', 'maxDate', startDate);
            }
        }).attr('readonly', 'true');
    });
    $(document).ready(function () {
        $("#DateFrom3").datepicker({
            changeMonth: true,
            changeYear: true,
            showAnim: "fadeIn",
            yearRange: 'c-30:c+30',
            fixFocusIE: true,
            onSelect: function (dateText, inst) {
                var dt2 = $('#DateTo3');
                var startDate = $(this).datepicker('getDate');
                startDate.setDate(startDate.getDate());
                var minDate = $(this).datepicker('getDate');
                dt2.datepicker('option', 'minDate', minDate);

            }
        }).attr('readonly', 'true');
        $('#DateTo3').datepicker({
            changeMonth: true,
            changeYear: true,
            showAnim: "fadeIn",
            yearRange: 'c-30:c+30',
            fixFocusIE: false,

            onSelect: function (dateText, inst) {
                var dt1 = $('#DateFrom3');
                var startDate = $(this).datepicker('getDate');
                dt1.datepicker('option', 'maxDate', startDate);
            }
            
        }).attr('readonly', 'true');
    });

    function showResponse(responseText, statusText, xhr, $form) {
        var item = responseText.toString().split('|');
        if (item[0] == "Sucess") {
            $('#content').load('../ClientRegistrationPA/ClientRegistrationPA?data=' + item[1] + '|Fill');
            $('.imgcontainer').css("display", "block");
            $('.imgcontainer').load('../Contact/ImageUploadPanel');
            $('.EditProfile').css("display", "block");
            window.location.reload(true);
        }
        else if (item[0] == "Failed") {
            if (item.length > 2 && item[2]) {
                alert(item[2]);
            } else {
                alert("Save operation failed.");
            }
        }
    }

    function funPaste(e) {
        e.preventDefault();
    }

    jQuery("#ContactForm").ajaxForm(function () {
        loadClientStaticDetails();
    });

    function Quick(quickId, type, fullName) {
        if (type == 'Emer') {
            $('#LoadDiv').load('../ClientRegistrationPA/QuickViewEmerg?ContactId=' + quickId + '&Fname1=' + fullName);
        } else
            $('#LoadDiv').load('../ClientRegistrationPA/QuickViewApprove?ContactId=' + quickId + '&Fname1=' + fullName);
        $("#divQuickView").hide();
    }

    function showQuickView(quickId, type, fullName) {
        Quick(quickId, type, fullName)
        $("#divQuickView").show();
    }
</script>
<script language="javascript" type="text/javascript">

    function cancelPreview() {
        $("#newPreview").hide();
        $("#stdImage").show();
        $("#cancelPreview").hide();

        $("#profilePicture").replaceWith($("#profilePicture").clone(true));
        $("#File1").replaceWith($("#File1").clone(true));
    }

    $(document).ready(function () {

        $("#profilePicture, #File1").change(function () {
            $("#newPreview").html("");
            var regex = /^([a-zA-Z0-9\s_\-:().\\])+(.jpg|.jpeg|.gif|.png|.bmp)$/i;
            if ($(this).val() != "") {
                if (regex.test($(this).val().toLowerCase())) {
                    //if ($.browser.msie && parseFloat(jQuery.browser.version) <= 9.0) {
                    //    $("#newPreview").show();

                    //    $("#newPreview")[0].filters.item("DXImageTransform.Microsoft.AlphaImageLoader").src = $(this).val();
                    //    $("#stdImage").hide();
                    //    $("#cancelPreview").show();
                    //   alert(
                    //}
                    //else {
                    if (typeof (FileReader) != "undefined") {
                        $("#newPreview").show();
                        $("#newPreview").append("<img />");
                        var reader = new FileReader();
                        reader.onload = function (e) {
                            
                            $('#stdImage').hide();
                            $("#newPreview img").attr("height", "180px");
                            $("#newPreview img").attr("width", "180px");
                            $("#newPreview img").attr("src", e.target.result);

                            $("#cancelPreview").show();
                                                   
                            var newValue = e.target.result;
                            var controlId = $(this).attr('id');
                            var labelText = 'Image';
                            var prevValue = $("#hdnPrevImg1").text();

                            Arr_Record = [];                            
                            if (isInitialValue(controlId, newValue)) {
                                message = 'Photo was changed.';                                
                                Arr_Record = [controlId, "file", labelText, prevValue, newValue, message];
                                Arr_Ticker.push(Arr_Record);
                                Arr_Record = [];
                                var pageType = "";

                                Arr_NewEventLogsChild = [pageType, "ImgUrl", labelText, prevValue, newValue];
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
                            writeToTickerBox(Arr_Ticker);

                        }                     


                        reader.readAsDataURL($(this)[0].files[0]);
                        //alert('2');
                    } else {
                        alert("This browser does not support FileReader.");
                    }
                    // }
                } else {
                    alert("Please upload a valid image file.");
                }
            }
        });

    });

    //===List 6 - Task #2
    $(document).ready(function () {
        $("#sample2").click(function () {
            var Refe = $('#testbox').val();
            if (Refe != "") {
                $.get('../ClientRegistrationPA/SavePosition?DocName=' + encodeURI(Refe));
                //$.get('../ClientRegistrationPA/SavePosition?DocName=' + encodeURI(Refe) + '   ', '_blank');
                window.location.reload();
            } else {
                alert("Please type a position");
            }
        });
    });

    $(document).ready(function () {
        $("#sample3").click(function () {
            var Refe = $('#selectbox').val();
            if (Refe != 0 || Refe > 0) {
                if (confirm("Are you Sure want to Delete this Position?")) {
                    $.get('../ClientRegistrationPA/DeletePosition?DocName=' + encodeURI(Refe));
                    //$.get('../ClientRegistrationPA/DeletePosition?DocName=' + encodeURI(Refe) + '   ', '_blank');
                    window.location.reload();
                }
            } else {
                alert("Please Select a Position to Delete");
            }
        });
    });
    //=====List 6 - Task #2

</script>

<%using (@Html.BeginForm("SaveClients", "ClientRegistrationPA", FormMethod.Post, new { id = "registrationFormPA", enctype = "multipart/form-data" }))
  { %>
<div style="width: 100%;">
    <%=Html.HiddenFor(m=>m.newEventLog,Model.newEventLog) %>
    <% if (TempData["notice"] != null)
       { %>
    <p style="width: 100%; color: black; font-size: 12px; font-weight: bold"><%= Html.Encode(TempData["notice"]) %></p>
    <% } %>
    <div>
        <%-- <input type="file" id="profilePicture1" />
        <div id="newPreview1"></div>--%>
        <table style="width: 70%; border-bottom: none; display: inline-block;">
            <tr>
                <td>
                    <label class="lblSpan">Prefix</label><span class="nospan-align">*</span><br />
                    <%: Html.DropDownListFor(x => x.Prefix, Model.PrefixList, new {@class="newClass", Style="width:90px !important;margin-bottom:0px;" })%></td>
                <td>
                    <label class="lblSpan">First Name</label><span class="span-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.FirstName, Model.FirstName, new {  @class="validate[required] namefield newClass",onpaste="funPaste(event);",maxlength=50,Style="width:150px !important;" })%></td>
                <td>
                    <label class="lblSpan">Middle Name</label><span class="nospan-align">*</span><br />

                    <%=Html.TextBoxFor(m => m.MiddleName, Model.MiddleName, new {  @class="sd namefield newClass",onpaste="funPaste(event);",maxlength=50,Style="width:150px !important;" })%></td>
                <td>
                    <label class="lblSpan">Last Name</label><span class="span-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.LastName, Model.LastName, new {  @class="validate[required] namefield newClass",onpaste="funPaste(event);",maxlength=50,Style="width:150px !important;" })%></td>
                <td>
                    <label class="lblSpan">Suffix</label><span class="nospan-align">*</span><br />
                    <%: Html.DropDownListFor(x => x.LastNameSuffix, Model.LastNameSuffixList, new {@class="newClass", Style="width:90px !important;margin-bottom:0px;" })%></td>
            </tr>
            <tr>
                <td colspan="5">
                    <label class="lblSpan">Nick Name</label><span class="nospan-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.NickName, Model.NickName, new {  @class="sd namefield newClass",onpaste="funPaste(event);",maxlength=50})%>
                </td>
            </tr>
        </table>
        <table>
            <tr>
                <td style="width: 0">
                    <label class="lblSpan">Birth Date</label><span class="span-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.DateOfBirth, Model.DateOfBirth, new { @class = "validate[required] datepicker newClass", ID = "dateOfBirth", Style="width:160px;" })%>

                </td>
                <td style="width: 0">
                    <label class="lblSpan">Gender</label><span class="nospan-align">*</span><br />
                    <%=Html.DropDownListFor(m => m.Gender, Model.GenderList, new {  @class="sd newClass",ID = "ddlGender", Style="width:145px;"  })%>

                </td>
                <td style="width: 0">
                    <label class="lblSpan">Race</label><span class="nospan-align">*</span><br />
                    <%=Html.DropDownListFor(m => m.Race, Model.RaceList, new {@class="newClass", maxlength=50, Style="width:145px;"})%>

                </td>
                <td style="width: 0">
                    <label class="lblSpan">Admission Date</label><span class="span-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.DateUpdated, Model.DateUpdated, new { @class = "validate[required] datepicker newClass", ID = "dateUpdated", Style="width:160px;" })%>

                </td>
            </tr>
            <tr>
                 <td style="width: 0">
                    <label class="lblSpan">Place of Birth</label><span class="nospan-align">*</span><br />
                      <%=Html.TextBoxFor(m => m.PlaceOfBirth, Model.PlaceOfBirth, new {  @class="sd",maxlength=50 })%>                                        
                </td> 

                 <td style="width: 0">
                    <label class="lblSpan">Coutry of Birth</label><span class="nospan-align">*</span><br />
                   <%--  <%=Html.DropDownListFor(m=>m.HomeCountry,Model.HomeCountryList,new { @class="sd", ID="ddlHomeCountry"}) %></td>
                         <%=Html.DropDownListFor(m=>m.HomeCountry,Model.HomeCountryList,new { @class="sd", ID="ddlHomeCountry"}) %></td> 
                    <%=Html.DropDownListFor(m => m.Race, Model.RaceList, new {@class="newClass", maxlength=50, Style="width:145px;"})%>  --%>
                    <%=Html.DropDownListFor(m => m.Country, Model.CountryList, new {  @class="sd newClass",ID = "ddlCountry" })%>
                </td>

                <td style="width: 0">
                    <label class="lblSpan">State of Birth</label><span class="nospan-align">*</span><br />
                      <%=Html.DropDownListFor(m => m.StateOfBirth, Model.StateList, new {  @class="sd newClass",ID = "ddlState" })%>
                </td>

                 <td style="width: 0">
                      <label class="lblSpan">Citizenship</label><span class="nospan-align">*</span><br />
                    <%=Html.DropDownListFor(m => m.Citizenship, Model.CitizenshipList, new {  @class="sd",ID = "ddlCityzenShip" })%>
                   
                </td>

            </tr>
            <tr>
                  <td style="width: 0">
                    <label class="lblSpan">Height(Inches)</label><span class="nospan-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.Height, Model.Height, new {  @class="validate[Custom] numbersOnlyT newClass",onpaste="funPaste(event);",ID="txtHeight" })%></td>
                <td  style="width: 0">
                    <label class="lblSpan">Weight(lbs)</label><span class="nospan-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.Weight, Model.Weight, new {  @class="validate[Custom] numbersOnlyTs newClass",onpaste="funPaste(event);",ID="txtWeight" })%></td>

                 <td  style="width: 0">
                    <label class="lblSpan">Hair Color</label><span class="nospan-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.HairColor, Model.HairColor, new {  @class="validate[custom[englishOnlywithspaces]]",maxlength=50 })%>
                </td>
                <td  style="width: 0">
                    <label class="lblSpan">Eye Color</label><span class="nospan-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.EyeColor, Model.EyeColor, new {  @class="validate[custom[englishOnlywithspaces]]",maxlength=50 })%></td>
         

            </tr>  
            <tr>
                <td style="width: 0">
                    <label class="lblSpan">Primary Language</label><span class="nospan-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.PrimaryLanguage, Model.PrimaryLanguage, new {  @class="sd",maxlength=50 })%></td>
                <td  style="width: 0">
                    <label class="lblSpan">Legal Competency Status</label><span class="nospan-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.LegalCompetencyStatus, Model.LegalCompetencyStatus, new {  @class="sd",maxlength=50 })%></td>

                <td style="width: 0"">
                    <label class="lblSpan">Guardianship Status</label><span class="nospan-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.GuardianshipStatus, Model.GuardianshipStatus, new {  @class="sd",maxlength=50 })%></td>
                <td style="width: 0">
                    <label class="lblSpan">Other State Agencies Involved With Student</label><span class="nospan-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.OtherStateAgenciesInvolvedWithStudent, Model.OtherStateAgenciesInvolvedWithStudent, new {  @class="sd",maxlength=250 })%></td>
                  </tr>
            <tr>               
                
                <td style="width: 0">
                    <label class="lblSpan">Distinguishing Marks</label><span class="nospan-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.DistigushingMarks, Model.DistigushingMarks, new {  @class="sd" })%></td>
                <td style="width: 0">
                    <label class="lblSpan">Marital Status of Both Parents</label><span class="nospan-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.MaritalStatusofBothParents, Model.MaritalStatusofBothParents, new {  @class="sd",maxlength=50 })%></td>
                <td colspan="1">
                    <label class="lblSpan">Case Manager Residential</label><span class="nospan-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.CaseManagerResidential, Model.CaseManagerResidential, new {  @class="sd",maxlength=50 })%></td>
                <td colspan="1">
                    <label class="lblSpan">Case Manager Educational</label><span class="nospan-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.CaseManagerEducational, Model.CaseManagerEducational, new {  @class="sd",maxlength=50 })%></td>
            </tr>
        
            <tr>
                <td colspan="4">
                    <label class="lblSpan">Guardian(Self)</label><span class="nospan-align">*</span><br />
                    <%=Html.CheckBoxFor(x=>x.IsGuardian,new {@class="newClass"})  %>
                </td>
            </tr>
        </table>
        <table>
            <tr>
                <td>
                    <label class="lblSpan">Ambulatory</label><span class="nospan-align">*</span><br />
                    <%=Html.DropDownListFor(m => m.Ambulatory, Model.YesNo, new {@class="newClass", maxlength=50})%>

                </td>
                <td>
                    <label class="lblSpan">Communication</label><span class="nospan-align">*</span><br />
                    <%=Html.TextAreaFor(m => m.Communication1,new {value=@Model.Communication1,  @class="sd newClass", @rows="3",@cols="30", Style="width:222px;"})%>

                </td>
                <td>
                    <label class="lblSpan">Intensive Staffing</label><span class="nospan-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.Intensive, Model.Intensive, new {@class="newClass", maxlength=300})%>

                </td>
            </tr>
            <tr>
                <td>
                    <label class="lblSpan">Limited English Proficiency</label><span class="nospan-align">*</span><br />
                    <%=Html.DropDownListFor(m => m.EnglishProficiency, Model.YesNoId, new {@class="newClass", maxlength=50})%>
                </td>
                <td colspan="2">
                    <label class="lblSpan">First Language</label><span class="nospan-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.PrimaryLanguage, Model.PrimaryLanguage, new {@class="newClass", maxlength=50})%>
                </td>
            </tr>
            <tr>

                <td>
                    <label class="lblSpan">BSU#/SASID#</label><span class="nospan-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.SASID, Model.SASID, new {@class="newClass", maxlength=50})%>

                </td>

                <td>
                    <label class="lblSpan">Medicaid #</label><span class="nospan-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.Medicaid, Model.Medicaid, new {@class="newClass", maxlength=50})%>

                </td>
                <td>
                    <label class="lblSpan">Funding Source</label><span class="nospan-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.Funding, Model.Funding, new {  @class="sd newClass" })%>
                </td>
            </tr>
        </table>
        <table>
            <tr>
                <td>
                    <label class="lblSpan">Classification 1</label><span class="nospan-align">*</span><br />
                    <%=Html.DropDownListFor(m => m.Classification1, Model.Classification, new { maxlength=50,@class="newClass"})%>
                </td>
                <td>
                    <label class="lblSpan">Classification 2</label><span class="nospan-align">*</span><br />
                    <%=Html.DropDownListFor(m => m.Classification2, Model.Classification, new { maxlength=50,@class="newClass"})%>

                </td>
                <td>
                    <label class="lblSpan">Classification 3</label><span class="nospan-align">*</span><br />
                    <%=Html.DropDownListFor(m => m.Classification3, Model.Classification, new { maxlength=50,@class="newClass"})%>

                </td>
                <td>
                    <label class="lblSpan">Classification 4</label><span class="nospan-align">*</span><br />
                    <%=Html.DropDownListFor(m => m.Classification4, Model.Classification, new { maxlength=50,@class="newClass"})%>

                </td>
                <td>
                    <label class="lblSpan">Classification 5</label><span class="nospan-align">*</span><br />
                    <%=Html.DropDownListFor(m => m.Classification5, Model.Classification, new { maxlength=50,@class="newClass"})%>

                </td>
            </tr>
            <tr>
                <td colspan="5">
                    <label class="lblSpan">Client Info Comments </label>
                    <span class="nospan-align"></span>
                    <br />
                    <%=Html.TextAreaFor(m => m.ClientInfoComments  ,new {value=@Model.ClientInfoComments,  @class="sd newClass", @rows="3",@cols="30", Style="width:100%;"})%>
                </td>
            </tr>
        </table>
       <%-- <table style="width: 100%;">
            <tr>
                <td class="auto-style2" colspan="4">
                    <h4>Diagnosis</h4>
                </td>
            </tr>
        </table>
        <table style="width: 100%;">  --%>
            <%-- first --%>
          <%--   <%for (int i = 0; i < Model.Diagnosis.Count && i < 3; i++)
              { %>
            <tr>
                <%if (Model.Diagnosis.Count > i)
                  {
                      if (Model.Diagnosis[i] != null)
                      { %>
                <td class="auto-style2" colspan="2"><%=Html.TextBoxFor(m => m.Diagnosis[i].Name, Model.Diagnosis[i].Name, new {  @class=" newClass",maxlength=50 })%></td>
                <%--   <td class="auto-style3" colspan="2"><%=Html.TextBoxFor(m => m.Diagnosis[i].Value, Model.Diagnosis[i].Value, new {  @class=" ",maxlength=50 })%></td>--%>
               <%-- %> <% }
                  }
                  else
                  {%>
                <td class="auto-style2" colspan="2">
                    <input type="text" value="" name="Diagnosis[<%=i %>].Name" /></td>
                <%-- <td class="auto-style3" colspan="2">
                    <input type="text" value="" name="Diagnosis[<%=i %>].Value" /></td>--%>
             <%-- %>   <%
                  } %>
            </tr>
            <%} %>
        </table>

        <table style="width: 100%;">

            <tr>
                <td class="auto-style2" colspan="7">
                    <h4>Medical Information</h4>
                </td>
            </tr>
        </table>
        <table>
            <tr>
                <td colspan="2">
                    <label class="lblSpan">Allergies</label><span class="nospan-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.Allergie, Model.Allergie, new {  @class="[] newClass",maxlength=500 })%></td>
            </tr>
            <tr>
                <td colspan="2">
                    <label class="lblSpan">Seizures</label><span class="nospan-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.Seizures, Model.Seizures, new {  @class="[] newClass",maxlength=500 })%></td>
            </tr>
            <tr>
                <td colspan="2">
                    <label class="lblSpan">Diet</label><span class="nospan-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.Diet, Model.Diet, new {  @class="[] newClass",maxlength=500 })%></td>
            </tr>
            <tr>
                <td colspan="2">
                    <label class="lblSpan">Other</label><span class="nospan-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.Other, Model.Other, new {  @class="[] newClass",maxlength=500 })%></td>
            </tr>

            <tr>
                <td colspan="2">
                    <label class="lblSpan">Day Program </label><span class="nospan-align"></span><br />
                    <%=Html.TextBoxFor(m => m.DayProgarm , Model.DayProgarm , new { maxlength=250 })%></td>
                <td colspan="2">
                    <label class="lblSpan">Classroom Workshop  </label><span class="nospan-align"></span><br />
                    <%=Html.TextBoxFor(m => m.ClassroomWorkshop  , Model.ClassroomWorkshop  , new { maxlength=250 })%></td>
            </tr>
        </table>--%>

        <table style="width: 100%;">
            <tr>
                <td colspan="9">
                    <h4>Emergency Contact - School</h4>
                </td>
            </tr>
        </table>
        <table style="width: 70%;">
            <tr>
                <td colspan="1">
                    <label class="lblSpan">First Name</label><span class="nospan-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.EmergencyContactFirstName1, Model.EmergencyContactFirstName1, new {  @class="newClass",maxlength=50 })%></td>
                <td colspan="1">
                    <label class="lblSpan">Last Name</label><span class="nospan-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.EmergencyContactLastName1, Model.EmergencyContactLastName1, new {  @class="newClass",maxlength=50 })%></td>
            </tr>

            <tr>
                <td colspan="1">
                    <label class="lblSpan">Title</label><span class="nospan-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.EmergencyContactTitle1, Model.EmergencyContactTitle1, new {  @class="sd newClass",maxlength=50 })%>
                </td>
                <td colspan="1">
                    <label class="lblSpan">Phone</label><span class="nospan-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.EmergencyContactPhone1, Model.EmergencyContactPhone1, new {  @class="validate[custom[usPhoneNumber]] usPhone newClass" })%></td>
            </tr>

            <tr>
                <td colspan="1">
                    <label class="lblSpan">First Name</label><span class="nospan-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.EmergencyContactFirstName2, Model.EmergencyContactFirstName2, new {  @class="newClass",onpaste="funPaste(event);",maxlength=50 })%></td>

                <td colspan="1">

                    <label class="lblSpan">Last Name</label><span class="nospan-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.EmergencyContactLastName2, Model.EmergencyContactLastName2, new {  @class="newClass",onpaste="funPaste(event);",maxlength=50 })%></td>
            </tr>

            <tr>
                <td colspan="1">
                    <label class="lblSpan">Title</label><span class="nospan-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.EmergencyContactTitle2, Model.EmergencyContactTitle2, new {  @class="sd newClass",maxlength=50 })%></td>

                <td colspan="1">
                    <label class="lblSpan">Phone</label><span class="nospan-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.EmergencyContactPhone2, Model.EmergencyContactPhone2, new {  @class="validate[custom[usPhoneNumber]] usPhone newClass" })%></td>
            </tr>

            <tr>
                <td colspan="1">
                    <label class="lblSpan">First Name</label><span class="nospan-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.EmergencyContactFirstName3, Model.EmergencyContactFirstName3, new {  @class="newClass",maxlength=50 })%></td>
                <td colspan="1">
                    <label class="lblSpan">Last Name</label><span class="nospan-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.EmergencyContactLastName3, Model.EmergencyContactLastName3, new {  @class="newClass",maxlength=50 })%></td>
            </tr>

            <tr>
                <td colspan="1">
                    <label class="lblSpan">Title</label><span class="nospan-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.EmergencyContactTitle3, Model.EmergencyContactTitle3, new {  @class="sd newClass",maxlength=50 })%></td>
                <td colspan="1">
                    <label class="lblSpan">Phone</label><span class="nospan-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.EmergencyContactPhone3, Model.EmergencyContactPhone3, new {  @class="validate[custom[usPhoneNumber]] usPhone newClass" })%></td>
            </tr>


            <tr>
                <td colspan="1">
                    <label class="lblSpan">First Name</label><span class="nospan-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.EmergencyContactFirstName4, Model.EmergencyContactFirstName4, new {  @class="newClass",maxlength=50 })%></td>
                <td colspan="1">
                    <label class="lblSpan">Last Name</label><span class="nospan-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.EmergencyContactLastName4, Model.EmergencyContactLastName4, new {  @class="newClass",maxlength=50})%></td>
            </tr>

            <tr>
                <td colspan="1">
                    <label class="lblSpan">Title</label><span class="nospan-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.EmergencyContactTitle4, Model.EmergencyContactTitle4, new {  @class="sd newClass",maxlength=50 })%></td>
                <td colspan="1">
                    <label class="lblSpan">Phone</label><span class="nospan-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.EmergencyContactPhone4, Model.EmergencyContactPhone4, new {  @class="validate[custom[usPhoneNumber]] usPhone newClass" })%></td>
            </tr>

            <tr>
                <td colspan="1">
                    <label class="lblSpan">First Name</label><span class="nospan-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.EmergencyContactFirstName5, Model.EmergencyContactFirstName5, new {  @class="newClass",maxlength=50 })%></td>
                <td colspan="1">
                    <label class="lblSpan">Last Name</label><span class="nospan-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.EmergencyContactLastName5, Model.EmergencyContactLastName5, new {  @class="newClass",maxlength=50 })%></td>
            </tr>

            <tr>
                <td colspan="1">
                    <label class="lblSpan">Title</label><span class="nospan-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.EmergencyContactTitle5, Model.EmergencyContactTitle5, new {  @class="sd newClass",maxlength=50 })%></td>
                <td colspan="1">
                    <label class="lblSpan">Phone</label><span class="nospan-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.EmergencyContactPhone5, Model.EmergencyContactPhone5, new {  @class="validate[custom[usPhoneNumber]] usPhone newClass" })%></td>
            </tr>
        </table>
        <table style="width: 100%;">

            <tr>
                <td class="auto-style2" colspan="7">
                    <h4>Emergency Contact</h4>
                </td>
            </tr>
        </table>

        <% if (Model.EmgContactList != null)
           {
               if (Model.EmgContactList.Count > 0)
               {
        %>

        <table style="width: 100%;" class="tblContacList">
            <thead>
                <tr class="HeaderStyle">
                    <th class="tdLabel" style="text-align: center;">Name</th>
                    <th class="tdLabel" style="text-align: center;">Relationship</th>
                    <th class="tdLabel" style="text-align: center;">Incident</th>
                    <th class="tdLabel" style="text-align: center;">Correspondence</th>
                    <th class="tdLabel" style="text-align: center;">Guardian</th>
                    <th class="tdLabel" style="text-align: center;">Home Phone</th>
                    <th class="tdLabel" style="text-align: center;">Work Phone</th>
                    <th class="tdLabel" style="text-align: center;">Other Phone</th>
                    <th class="tdLabel" style="text-align: center;">Approved<br />
                        Visitor</th>
                    <th class="tdLabel" style="text-align: center;">Quick View</th>
                </tr>
            </thead>
            <tbody>
                <%foreach (var item in Model.EmgContactList)
                  {
                %>


                <tr class="RowStyle">

                    <td><%:item.FUllName %> </td>
                    <td><%:item.RelationName%></td>
                    <%if (item.IsIncident == true)
                      { %>

                    <td style="text-align: left">
                        <input style="width: auto;" type="checkbox" onclick="return false" checked="checked" /></td>
                    <%}
                      else
                      { %>
                    <td style="text-align: left">
                        <input style="width: auto;" type="checkbox" onclick="return false" /></td>
                    <%} %>
                    <%if (item.IsCorrespondence == true)
                      { %>

                    <td style="text-align: left">
                        <input style="width: auto;" type="checkbox" onclick="return false" checked="checked" /></td>
                    <%}
                      else
                      { %>
                    <td style="text-align: left">
                        <input style="width: auto;" type="checkbox" onclick="return false" /></td>
                    <%} %>
                    <%if (item.IsGuardian == true)
                      { %>

                    <td style="text-align: left">
                        <input style="width: auto;" type="checkbox" onclick="return false" checked="checked" /></td>
                    <%}
                      else
                      { %>
                    <td style="text-align: left">
                        <input style="width: auto;" type="checkbox" onclick="return false" /></td>
                    <%} %>
                    <td><%:item.HomePhone%></td>
                    <td><%:item.WorkMobilePhone%></td>
                    <td><%:item.OtherMobilePhone%></td>
                    <td style="text-align: left;">
                        <%if (item.IsOnCampusWithStaff == true)
                          { %>
                    On Campus, With Staff;
                        <%}
                          else
                          { %>
                        <%} %>
                        <%if (item.IsOnCampusAlone == true)
                          { %>
                   On Campus, Alone;
                        <%}
                          else
                          { %>
                        <%} %>
                        <%if (item.IsOffCampus == true)
                          { %>
                    Off Campus<br />
                        <%}
                          else
                          { %>
                        <%} %>
                    </td>
                    <td>
                        <img src="../../Images/view-icon.png" style="cursor: pointer;" onclick="showQuickView(<%=item.Id%>,'Emer', '<%=Url.Encode( item.FUllName) %>');" />
                    </td>
                </tr>

                <%
                  }
               }
           }%>
            </tbody>
        </table>
        <%} %>

        <table style="width: 100%;">
            <tr>

                <td colspan="9">
                    <h4>Schools Attended</h4>
                </td>
            </tr>
        </table>
        <table style="width: 70%;">

            <tr>
                <td colspan="1">
                    <label class="lblSpan">School Name</label><span class="nospan-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.SchoolName1, Model.SchoolName1, new {  @class="sd",maxlength=50 })%></td>
            </tr>
            <tr>
                <td colspan="1">
                    <label class="lblSpan">Address Line 1</label><span class="nospan-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.SchoolAttendedAddress11, Model.SchoolAttendedAddress11, new {  @class="sd",maxlength=50 })%></td>

            </tr>


            <tr>
                <td colspan="1">
                    <label class="lblSpan">Address Line 2</label><span class="nospan-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.SchoolAttendedAddress21, Model.SchoolAttendedAddress21, new {  @class="sd",maxlength=50 })%></td>
            </tr>


            <tr>
                <td colspan="1">
                    <label class="lblSpan">City</label><span class="nospan-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.SchoolAttendedCity1, Model.SchoolAttendedCity1, new {  @class="sd",maxlength=50 })%></td>
            </tr>


            <tr>
                <td colspan="1">
                    <label class="lblSpan">State</label><span class="nospan-align">*</span><br />
                    <%--  <%=Html.TextBoxFor(m => m.SchoolAttendedState1, Model.SchoolAttendedState1, new {  @class="sd",maxlength=50 })%></td>--%>
                    <%=Html.DropDownListFor(m => m.intSchoolAttendedState1, Model.SchoolState1, new {  @class="sd",ID = "ddlState1" })%></td>
            </tr>


            <tr>
                <td colspan="1">
                    <label class="lblSpan">
                        Dates Attended
                    (MM/dd/yyyy)</label><span class="nospan-align">*</span><br />

                </td>

            </tr>

            <tr>
                <td colspan="1">
                    <label class="lblSpan">From</label><span class="nospan-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.DateFrom1, Model.DateFrom1, new {  @class = "datepicker",ID = "DateFrom1" })%></td>
                <td colspan="1">
                    <label class="lblSpan">To</label><span class="nospan-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.DateTo1, Model.DateTo1, new {  @class = "datepicker",ID = "DateTo1" })%></td>
            </tr>


            <tr>
                <td colspan="1">
                    <label class="lblSpan">School Name</label><span class="nospan-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.SchoolName2, Model.SchoolName2, new {  @class="sd",maxlength=50 })%></td>
            </tr>
            <tr>
                <td colspan="1">
                    <label class="lblSpan">Address Line 1</label><span class="nospan-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.SchoolAttendedAddress12, Model.SchoolAttendedAddress12, new {  @class="sd",maxlength=50 })%></td>
            </tr>

            <tr>
                <td colspan="1">
                    <label class="lblSpan">Address Line 2</label><span class="nospan-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.SchoolAttendedAddress22, Model.SchoolAttendedAddress22, new {  @class="sd",maxlength=50 })%></td>
            </tr>


            <tr>
                <td colspan="1">
                    <label class="lblSpan">City</label><span class="nospan-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.SchoolAttendedCity2, Model.SchoolAttendedCity2, new {  @class="sd",maxlength=50 })%></td>
            </tr>

            <tr>
                <td colspan="1">
                    <label class="lblSpan">State</label><span class="nospan-align">*</span><br />
                    <%--      <%=Html.TextBoxFor(m => m.SchoolAttendedState2, Model.SchoolAttendedState2, new {  @class="sd",maxlength=50 })%></td>--%>
                    <%=Html.DropDownListFor(m => m.intSchoolAttendedState2, Model.SchoolState2, new {  @class="sd",ID = "ddlState2" })%></td>
            </tr>

            <tr>
                <td colspan="1">
                    <label class="lblSpan">
                        Dates Attended
                    (MM/dd/yyyy)</label><span class="nospan-align">*</span><br />
                </td>
            </tr>

            <tr>
                <td colspan="1">
                    <label class="lblSpan">From</label><span class="nospan-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.DateFrom2, Model.DateFrom2, new {  @class = "datepicker" ,ID = "DateFrom2" })%></td>
                <td colspan="1">
                    <label class="lblSpan">To</label><span class="nospan-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.DateTo2, Model.DateTo2, new {  @class = "datepicker",ID = "DateTo2" })%></td>
            </tr>

            <tr>
                <td colspan="1">
                    <label class="lblSpan">School Name</label><span class="nospan-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.SchoolName3, Model.SchoolName3, new {  @class="sd",maxlength=50 })%></td>
            </tr>


            <tr>
                <td colspan="1">
                    <label class="lblSpan">Address Line 1</label><span class="nospan-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.SchoolAttendedAddress13, Model.SchoolAttendedAddress13, new {  @class="sd",maxlength=50 })%></td>
            </tr>

            <tr>
                <td colspan="1">
                    <label class="lblSpan">Address Line 2</label><span class="nospan-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.SchoolAttendedAddress23, Model.SchoolAttendedAddress23, new {  @class="sd",maxlength=50 })%></td>
            </tr>

            <tr>
                <td colspan="1">
                    <label class="lblSpan">City</label><span class="nospan-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.SchoolAttendedCity3, Model.SchoolAttendedCity3, new {  @class="sd",maxlength=50 })%></td>
            </tr>

            <tr>
                <td colspan="1">
                    <label class="lblSpan">State</label><span class="nospan-align">*</span><br />
                    <%--     <%=Html.TextBoxFor(m => m.SchoolAttendedState3, Model.SchoolAttendedState3, new {  @class="sd",maxlength=50 })%></td>--%>
                    <%=Html.DropDownListFor(m => m.intSchoolAttendedState3, Model.SchoolState3, new {  @class="sd",ID = "ddlState3" })%></td>
            </tr>


            <tr>
                <td colspan="1">
                    <label class="lblSpan">
                        Dates Attended
                    (MM/dd/yyyy)</label><span class="nospan-align">*</span><br />
                </td>
            </tr>
            <tr>
                <td colspan="1">
                    <label class="lblSpan">From</label><span class="nospan-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.DateFrom3, Model.DateFrom3, new {  @class = "datepicker",ID = "DateFrom3" })%></td>
                <td colspan="1">
                    <label class="lblSpan">To</label><span class="nospan-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.DateTo3, Model.DateTo3, new {  @class = "datepicker",ID = "DateTo3" })%></td>
            </tr>
        </table>
        <table style="width: 100%;">

            <tr>
                <td colspan="9">
                    <h4>Insurance</h4>
                </td>
            </tr>
        </table>
        <table style="width: 70%;">
            <tr>
                <td colspan="1">
                    <label class="lblSpan">Insurance Type1</label><span class="nospan-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.InsuranceType, Model.InsuranceType, new {  @class="sd",maxlength=50 })%></td>
                <td colspan="1">
                    <label class="lblSpan">Policy Number1</label><span class="nospan-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.PolicyNumber, Model.PolicyNumber, new {  @class="sd",maxlength=50 })%></td>
            </tr>

            <tr>
                <td colspan="1">
                    <label class="lblSpan">Policy Holder1</label><span class="nospan-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.PolicyHolder, Model.PolicyHolder, new {  @class="sd",maxlength=50 })%></td>

            </tr>
            <tr>
                <td colspan="1">
                    <label class="lblSpan">Insurance Type2</label><span class="nospan-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.InsuranceType1, Model.InsuranceType1, new {  @class="sd",maxlength=50 })%></td>
                <td colspan="1">
                    <label class="lblSpan">Policy Number2</label><span class="nospan-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.PolicyNumber1, Model.PolicyNumber1, new {  @class="sd",maxlength=50 })%></td>
            </tr>

            <tr>
                <td colspan="1">
                    <label class="lblSpan">Policy Holder2</label><span class="nospan-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.PolicyHolder1, Model.PolicyHolder1, new {  @class="sd",maxlength=50 })%></td>

            </tr>
        </table>
        <table style="width: 100%;">
            <tr>
                <td colspan="9">
                    <h4>IEP Information</h4>
                </td>
            </tr>
        </table>
        <table style="width: 70%;">

            <tr>
                <td colspan="1">
                    <label class="lblSpan">Full Name</label><span class="nospan-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.ReferralIEPFullName, Model.ReferralIEPFullName, new {  @class="namefield",onpaste="funPaste(event);",maxlength=250 })%></td>
                <td colspan="1">
                    <label class="lblSpan">Title</label><span class="nospan-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.ReferralIEPTitle, Model.ReferralIEPTitle, new {  @class="sd",maxlength=250 })%></td>
            </tr>

            <tr>
                <td colspan="1">
                    <label class="lblSpan">Phone</label><span class="nospan-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.ReferralIEPPhone, Model.ReferralIEPPhone, new {  @class="validate[custom[usPhoneNumber]] usPhone",onpaste="funPaste(event);",maxlength=50 })%></td>
                <td colspan="1">
                    <label class="lblSpan">Referring Agency</label><span class="nospan-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.ReferralIEPReferringAgency, Model.ReferralIEPReferringAgency, new {  @class="sd" })%></td>
            </tr>


            <tr>
                <td colspan="1">
                    <label class="lblSpan">Source Of Tuition</label><span class="nospan-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.ReferralIEPSourceofTuition, Model.ReferralIEPSourceofTuition, new {  @class="sd" })%></td>
            </tr>
        </table>
        <table style="width: 100%;">

            <tr>
                <td colspan="9">
                    <h4>Education History</h4>
                </td>
            </tr>
        </table>
        <table style="width: 70%;">
            <tr>
                <td colspan="1">
                    <label class="lblSpan">Date Initially Eligible for Special Education</label><span class="nospan-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.DateInitiallyEligibleforSpecialEducation, Model.DateInitiallyEligibleforSpecialEducation, 
            new {  @class="datepicker", ID = "DateInitiallyEligibleforSpecialEducation"  })%></td>
                <td colspan="1">
                    <label class="lblSpan">Date of Most Recent Special Education Evaluations</label><span class="nospan-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.DateofMostRecentSpecialEducationEvaluations, Model.DateofMostRecentSpecialEducationEvaluations, 
            new {  @class="datepicker", ID = "DateofMostRecentSpecialEducationEvaluations"  })%></td>
            </tr>

            <tr>
                <td colspan="1">
                    <label class="lblSpan">Date of Next Scheduled 3-Year Evaluation</label><span class="nospan-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.DateofNextScheduled3YearEvaluation, Model.DateofNextScheduled3YearEvaluation, 
            new {  @class="datepicker", ID = "DateofNextScheduled3YearEvaluation"  })%></td>
                <td colspan="1">
                    <label class="lblSpan">Current IEP Start Date</label><span class="nospan-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.CurrentIEPStartDate, Model.CurrentIEPStartDate, new {  @class="datepicker", ID = "CurrentIEPStartDate"  })%></td>
            </tr>

            <tr>
                <td colspan="1">
                    <label class="lblSpan">Current IEP Expiration Date</label><span class="nospan-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.CurrentIEPExpirationDate, Model.CurrentIEPExpirationDate, new {  @class="datepicker", ID = "CurrentIEPExpirationDate"  })%></td>
            </tr>
        </table>

        <table style="width: 100%;">

            <tr>
                <td colspan="9">
                    <h4>Discharge Information</h4>
                </td>
            </tr>
        </table>
        <table>
            <tr>
                <td colspan="1">
                    <label class="lblSpan">Discharge Date</label><span class="nospan-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.DischargeDate, Model.DischargeDate, new {  @class="datepicker", ID = "DischargeDate"  })%></td>
                <td colspan="1">
                    <label class="lblSpan">Location After Discharge</label><span class="nospan-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.LocationAfterDischarge, Model.LocationAfterDischarge, new {  @class="sd",maxlength=50 })%></td>
            </tr>

            <tr>
                <td colspan="1">
                    <label class="lblSpan">Melmark New England&#39;s Follow Up Responsibilities</label><span class="nospan-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.MelmarkNewEnglandsFollowUpResponsibilities, Model.MelmarkNewEnglandsFollowUpResponsibilities, new {  @class="sd" })%></td>
            </tr>
        </table>

        <table style="width: 100%;">

            <tr>
                <td class="auto-style2" colspan="7">
                    <h4>Approved Visitors</h4>
                </td>
            </tr>
        </table>
        <%if (Model.ApprovedVisitor != null)
          {
              if (Model.ApprovedVisitor.Count > 0)
              {%>
        <table style="width: 100%;" class="tblContacList">
            <thead>
                <tr class="HeaderStyle">
                    <th class="tdLabel" style="text-align: center;">Name</th>
                    <th class="tdLabel" style="text-align: center;">Relationship</th>
                    <th class="tdLabel" style="text-align: center;">Incident</th>
                    <th class="tdLabel" style="text-align: center;">Emergency</th>
                    <th class="tdLabel" style="text-align: center;">Correspondence</th>
                    <th class="tdLabel" style="text-align: center;">Guardian</th>
                    <th class="tdLabel" style="text-align: center;">Home Phone</th>
                    <th class="tdLabel" style="text-align: center;">Work Phone</th>
                    <th class="tdLabel" style="text-align: center;">Other Phone</th>
                    <th class="tdLabel" style="text-align: center;">Approved<br />
                        Visitor</th>
                    <th class="tdLabel" style="text-align: center;">Quick View</th>
                </tr>
            </thead>
            <tbody>
                <%foreach (var item in Model.ApprovedVisitor)
                  {
                %>
                <tr class="RowStyle">
                    <td><%:item.FUllName %></td>
                    <td><%:item.RelationName %></td>
                    <%if (item.IsIncident == true)
                      { %>

                    <td style="text-align: left">
                        <input style="width: auto;" type="checkbox" onclick="return false" checked="checked" /></td>
                    <%}
                      else
                      { %>
                    <td style="text-align: left">
                        <input style="width: auto;" type="checkbox" onclick="return false" /></td>
                    <%} %>
                    <%if (item.IsEmergency == true)
                      { %>

                    <td style="text-align: left">
                        <input style="width: auto;" type="checkbox" onclick="return false" checked="checked" /></td>
                    <%}
                      else
                      { %>
                    <td style="text-align: left">
                        <input style="width: auto;" type="checkbox" onclick="return false" /></td>
                    <%} %>
                    <%if (item.IsCorrespondence == true)
                      { %>

                    <td style="text-align: left">
                        <input style="width: auto;" type="checkbox" onclick="return false" checked="checked" /></td>
                    <%}
                      else
                      { %>
                    <td style="text-align: left">
                        <input style="width: auto;" type="checkbox" onclick="return false" /></td>
                    <%} %>
                    <%if (item.IsGuardian == true)
                      { %>

                    <td style="text-align: left">
                        <input style="width: auto;" type="checkbox" onclick="return false" checked="checked" /></td>
                    <%}
                      else
                      { %>
                    <td style="text-align: left">
                        <input style="width: auto;" type="checkbox" onclick="return false" /></td>
                    <%} %>
                    <td><%:item.HomePhone %></td>
                    <td><%:item.WorkMobilePhone %></td>
                    <td><%:item.OtherMobilePhone %></td>
                    <td style="text-align: left;">
                        <%if (item.IsOnCampusWithStaff == true)
                          { %>
                    On Campus, With Staff;
                        <%}
                          else
                          { %>
                        <%} %>
                        <%if (item.IsOnCampusAlone == true)
                          { %>
                    On Campus, Alone;
                        <%}
                          else
                          { %>
                        <%} %>
                        <%if (item.IsOffCampus == true)
                          { %>
                    Off Campus<br />
                        <%}
                          else
                          { %>
                        <%} %>
                    </td>
                    <td>
                        <img src="../../Images/view-icon.png" style="cursor: pointer;" onclick="showQuickView(<%=item.Id %>,'Approve', '<%=Url.Encode(item.FUllName) %>');" />
                    </td>
                </tr>

                <%} %>
            </tbody>
        </table>
        <%}
          }%>






        <table style="width: 100%;">

            <tr>
                <td class="auto-style2" colspan="7">
                    <h4>Associated Melmark Staff</h4>
                </td>
            </tr>
        </table>
        <div class="documentDiv3"  style="display:none;visibility:hidden"> <!-- List 6 -Task #2-->
            <div id="PositionDiv" style="border: 1px solid black;width: 97%;padding: 10px;margin: 3px;padding-bottom:25px;margin-bottom:20px;float:left">   
                <div class="addposition" style="margin-top:10px;padding-top: 20px;text-align: center;vertical-align: middle;width: 50%;float:left">
                    <label class="lblSpan" style="margin-right: 73%">Add Positions</label><span class="nospan-align"></span><br /><br />
                    <div id="PosSubDiv1" style="margin-left:10px">
				        <label class="lblSpan" style="float:left;padding:5px">Position: </label>
				        <%=Html.TextBoxFor(m => m.PositionLabelAdd, new { maxlength=250, @id= "testbox", @class="namefield newClass", @Style ="float:left;margin-left:10px" })%>
                        <input class="btn-default" type="button" id="sample2" value="Add" style="float:left;background: none!important;border: none;padding: 0!important;font-family: arial, sans-serif;color: #069;text-decoration: underline;cursor: pointer;outline:none"/>
                    </div>
			    </div>
                <div id="removeposition" style="margin-top:10px;padding-top: 20px;text-align: center;vertical-align: middle;width: 50%;float:right">
                    <label class="lblSpan" style="margin-right: 70%">Remove Positions</label><span class="nospan-align"></span><br /><br />
                    <div id="PosSubDiv2" style="margin-left:5px">
                        <label class="lblSpan" style="float:left;padding:5px">Position: </label>
                        <%=Html.DropDownListFor(m => m.GetPopulatePositionID, Model.PopulatePositionList, new { @id= "selectbox", @style="width:225px !important;float:left;margin-left:10px",@class="namefield newClass" })%>                    
                        <input class="btn-default" type="button" id="sample3" value="Delete" style="float:left;background: none!important;border: none;padding: 0!important;font-family: arial, sans-serif;color: #069;text-decoration: underline;cursor: pointer;outline:none"/>
                    </div>
                </div>
            </div>
            <table style="display:none;visibility:hidden">
                <tr>
                    <%--<td colspan="2"><%=Html.DropDownListFor(m => m.StaffPosition1, Model.StaffPositionList, new { @style="width:225px !important",@class="namefield newClass" })%></td>--%>
                    <%--<td colspan="2"><%=Html.DropDownListFor(m => m.Position1Staff, Model.StaffList, new { @style="width:225px !important",@class="namefield newClass" })%></td>--%>
                </tr>
                <tr>
                    <%--<td colspan="2"><%=Html.DropDownListFor(m => m.StaffPosition2, Model.StaffPositionList, new { @style="width:225px !important",@class="namefield newClass" })%></td>--%>
                    <%--<td colspan="2"><%=Html.DropDownListFor(m => m.Position2Staff, Model.StaffList, new { @style="width:225px !important",@class="namefield newClass" })%></td>--%>
                </tr>
                <tr>
                    <%--<td colspan="2"><%=Html.DropDownListFor(m => m.StaffPosition3, Model.StaffPositionList, new { @style="width:225px !important",@class="namefield newClass" })%></td>--%>
                    <%--<td colspan="2"><%=Html.DropDownListFor(m => m.Position3Staff, Model.StaffList, new { @style="width:225px !important",@class="namefield newClass" })%></td>--%>
                </tr>
                <tr>
                    <%--<td colspan="2"><%=Html.DropDownListFor(m => m.StaffPosition4, Model.StaffPositionList, new { @style="width:225px !important",@class="namefield newClass" })%></td>--%>
                    <%--<td colspan="2"><%=Html.DropDownListFor(m => m.Position4Staff, Model.StaffList, new { @style="width:225px !important",@class="namefield newClass" })%></td>--%>
                </tr>
                <tr>
                    <%--<td colspan="2"><%=Html.DropDownListFor(m => m.StaffPosition5, Model.StaffPositionList, new { @style="width:225px !important",@class="namefield newClass" })%></td>--%>
                    <%--<td colspan="2"><%=Html.DropDownListFor(m => m.Position5Staff, Model.StaffList, new { @style="width:225px !important",@class="namefield newClass" })%></td>--%>
                </tr>
                <tr>
                    <%--<td colspan="2"><%=Html.DropDownListFor(m => m.StaffPosition6, Model.StaffPositionList, new { @style="width:225px !important",@class="namefield newClass" })%></td>--%>
                    <%--<td colspan="2"><%=Html.DropDownListFor(m => m.Position6Staff, Model.StaffList, new { @style="width:225px !important",@class="namefield newClass" })%></td>--%>
                </tr>
                <tr>
                    <%--<td colspan="2"><%=Html.DropDownListFor(m => m.StaffPosition7, Model.StaffPositionList, new { @style="width:225px !important",@class="namefield newClass" })%></td>--%>
                    <%--<td colspan="2"><%=Html.DropDownListFor(m => m.Position7Staff, Model.StaffList, new { @style="width:225px !important",@class="namefield newClass" })%></td>--%>
                </tr>
                <tr>
                    <%--<td colspan="2"><%=Html.DropDownListFor(m => m.StaffPosition8, Model.StaffPositionList, new { @style="width:225px !important",@class="namefield newClass" })%></td>--%>
                    <%--<td colspan="2"><%=Html.DropDownListFor(m => m.Position8Staff, Model.StaffList, new { @style="width:225px !important",@class="namefield newClass" })%></td>--%>
                </tr>
                <tr>
                    <%--<td colspan="2"> <%=Html.DropDownListFor(m => m.StaffPosition9, Model.StaffPositionList, new { @style="width:225px !important",@class="namefield newClass" })%></td>--%>
                    <%--<td colspan="2"><%=Html.DropDownListFor(m => m.Position9Staff, Model.StaffList, new { @style="width:225px !important",@class="namefield newClass" })%></td>--%>
                </tr>
                <tr>
                    <%--<td colspan="2"><%=Html.DropDownListFor(m => m.StaffPosition10, Model.StaffPositionList, new { @style="width:225px !important",@class="namefield newClass" })%></td>--%>
                    <%--<td colspan="2"><%=Html.DropDownListFor(m => m.Position10Staff, Model.StaffList, new { @style="width:225px !important",@class="namefield newClass" })%></td>--%>
                </tr>
            </table>
        </div> <!-- List 6 -Task #2-->
        <table style="display:block;visibility:visible">
            <tr>
                <td colspan="2">
                    <%--<label class="lblSpan">Teacher Instructor</label><span class="nospan-align"></span><br />--%>
                    <%--<%=Html.TextBoxFor(m => m.TeacherInstructor, Model.TeacherInstructor, new { maxlength=250,@class="namefield newClass",onpaste="funPaste(event);" })%>--%>
                    <%=Html.TextBoxFor(m => m.PositionLabel1, Model.PositionLabel1, new { maxlength=250,@class="positionlabel_textbox",onpaste="e.preventDefault();" })%>
                    <%=Html.DropDownListFor(m => m.Position1Staff, Model.StaffList, new { @style="width:225px !important",@class="namefield newClass" })%>
                    <%=Html.HiddenFor(m => m.StaffPosition1,Model.StaffPosition1)%>
                </td>
                <td colspan="2">
                    <%--<label class="lblSpan">Program Specialist </label><span class="nospan-align"></span><br />--%>
                    <%--<%=Html.TextBoxFor(m => m.ProgramSpecialist, Model.ProgramSpecialist, new { maxlength=250 ,@class="namefield newClass",onpaste="funPaste(event);"})%>--%>
                    <%=Html.TextBoxFor(m => m.PositionLabel2, Model.PositionLabel2, new { maxlength=250,@class="positionlabel_textbox",onpaste="e.preventDefault();" })%>
                    <%=Html.DropDownListFor(m => m.Position2Staff, Model.StaffList, new { @style="width:225px !important",@class="namefield newClass" })%>
                    <%=Html.HiddenFor(m => m.StaffPosition2,Model.StaffPosition2)%>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <%--<label class="lblSpan">EDU Behavior Analyst</label><span class="nospan-align"></span><br />--%>
                    <%--<%=Html.TextBoxFor(m => m.EDUBehaviorAnalyst, Model.EDUBehaviorAnalyst, new { maxlength=250 ,@class="namefield newClass",onpaste="funPaste(event);"})%>--%>
                    <%=Html.TextBoxFor(m => m.PositionLabel3, Model.PositionLabel3, new { maxlength=250,@class="positionlabel_textbox",onpaste="e.preventDefault();" })%>
                    <%=Html.DropDownListFor(m => m.Position3Staff, Model.StaffList, new { @style="width:225px !important",@class="namefield newClass" })%>
                    <%=Html.HiddenFor(m => m.StaffPosition3,Model.StaffPosition3)%>
                </td>
                <td colspan="2">
                    <%--<label class="lblSpan">Curriculum Coordinator</label><span class="nospan-align"></span><br />--%>
                    <%--<%=Html.TextBoxFor(m => m.CurriculumCoordinator, Model.CurriculumCoordinator, new { maxlength=250 ,@class="namefield newClass",onpaste="funPaste(event);"})%>--%>
                    <%=Html.TextBoxFor(m => m.PositionLabel4, Model.PositionLabel4, new { maxlength=250,@class="positionlabel_textbox",onpaste="e.preventDefault();" })%>
                    <%=Html.DropDownListFor(m => m.Position4Staff, Model.StaffList, new { @style="width:225px !important",@class="namefield newClass" })%>
                    <%=Html.HiddenFor(m => m.StaffPosition4,Model.StaffPosition4)%>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <%--<label class="lblSpan">Residential Program</label><span class="nospan-align"></span><br />--%>
                    <%--<%=Html.TextBoxFor(m => m.ResidentialProgram, Model.ResidentialProgram, new { maxlength=250 ,@class="namefield newClass",onpaste="funPaste(event);"})%>--%>
                    <%=Html.TextBoxFor(m => m.PositionLabel5, Model.PositionLabel5, new { maxlength=250,@class="positionlabel_textbox",onpaste="e.preventDefault();" })%>
                    <%=Html.DropDownListFor(m => m.Position5Staff, Model.StaffList, new { @style="width:225px !important",@class="namefield newClass" })%>
                    <%=Html.HiddenFor(m => m.StaffPosition5,Model.StaffPosition5)%>
                </td>
                <td colspan="2">
                    <%--<label class="lblSpan">House</label><span class="nospan-align"></span><br />
                    <%=Html.TextBoxFor(m => m.House, Model.House, new { maxlength=250 })%>--%>
                    <%--<label class="lblSpan">Program Manager QMRP</label><span class="nospan-align"></span><br />--%>
                    <%--<%=Html.TextBoxFor(m => m.ProgramManagerQMRP , Model.ProgramManagerQMRP , new { maxlength=250 ,@class="namefield newClass",onpaste="funPaste(event);"})%>--%>
                    <%=Html.TextBoxFor(m => m.PositionLabel6, Model.PositionLabel6, new { maxlength=250,@class="positionlabel_textbox",onpaste="e.preventDefault();" })%>
                    <%=Html.DropDownListFor(m => m.Position6Staff, Model.StaffList, new { @style="width:225px !important",@class="namefield newClass" })%>
                    <%=Html.HiddenFor(m => m.StaffPosition6,Model.StaffPosition6)%>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <%--<label class="lblSpan">House Supervisor</label><span class="nospan-align"></span><br />--%>
                    <%--<%=Html.TextBoxFor(m => m.HouseSupervisor, Model.HouseSupervisor, new { maxlength=250 ,@class="namefield newClass",onpaste="funPaste(event);"})%>--%>
                    <%=Html.TextBoxFor(m => m.PositionLabel7, Model.PositionLabel7, new { maxlength=250,@class="positionlabel_textbox",onpaste="e.preventDefault();" })%>
                    <%=Html.DropDownListFor(m => m.Position7Staff, Model.StaffList, new { @style="width:225px !important",@class="namefield newClass" })%>
                    <%=Html.HiddenFor(m => m.StaffPosition7,Model.StaffPosition7)%>
                </td>
                <td colspan="2">
                    <%--<label class="lblSpan">Residential Behavior Analyst</label><span class="nospan-align"></span><br />--%>
                    <%--<%=Html.TextBoxFor(m => m.ResidentialBehaviorAnalyst , Model.ResidentialBehaviorAnalyst , new { maxlength=250 ,@class="namefield newClass",onpaste="funPaste(event);"})%>--%>
                    <%=Html.TextBoxFor(m => m.PositionLabel8, Model.PositionLabel8, new { maxlength=250,@class="positionlabel_textbox",onpaste="e.preventDefault();" })%>
                    <%=Html.DropDownListFor(m => m.Position8Staff, Model.StaffList, new { @style="width:225px !important",@class="namefield newClass" })%>
                    <%=Html.HiddenFor(m => m.StaffPosition8,Model.StaffPosition8)%>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <%--<label class="lblSpan">Primary Nurse</label><span class="nospan-align"></span><br />--%>
                    <%--<%=Html.TextBoxFor(m => m.PrimaryNurse, Model.PrimaryNurse, new { maxlength=250,@class="namefield newClass",onpaste="funPaste(event);" })%>--%>
                    <%=Html.TextBoxFor(m => m.PositionLabel9, Model.PositionLabel9, new { maxlength=250,@class="positionlabel_textbox",onpaste="e.preventDefault();" })%>
                    <%=Html.DropDownListFor(m => m.Position9Staff, Model.StaffList, new { @style="width:225px !important",@class="namefield newClass" })%>
                    <%=Html.HiddenFor(m => m.StaffPosition9,Model.StaffPosition9)%>
                </td>
                <td colspan="2">
                    <%--<label class="lblSpan">Unit Clerk</label><span class="nospan-align"></span><br />--%>
                    <%--<%=Html.TextBoxFor(m => m.UnitClerk , Model.UnitClerk , new { maxlength=250,@class="namefield newClass",onpaste="funPaste(event);" })%>--%>
                    <%=Html.TextBoxFor(m => m.PositionLabel10, Model.PositionLabel10, new { maxlength=250,@class="positionlabel_textbox",onpaste="e.preventDefault();" })%>
                    <%=Html.DropDownListFor(m => m.Position10Staff, Model.StaffList, new { @style="width:225px !important",@class="namefield newClass" })%>
                    <%=Html.HiddenFor(m => m.StaffPosition10,Model.StaffPosition10)%>
                </td>
            </tr>


        </table>

        <table>

            <%--<tr>
                <td class="lblSpan" colspan="7">
                    <strong>Lifting / Transfers<span class="auto-style1"> (How does this person transfer, How are they lifted - If applicable)</span></strong> </td>
            </tr>
            <tr>
                <td class="auto-style2" colspan="7"><%=Html.TextAreaFor(m => m.LiftingOrTransfers1,new {value=@Model.LiftingOrTransfers1,  @class="sd", @rows="3", @cols="160" })%>
                    <%=Html.TextAreaFor(m => m.LiftingOrTransfers2,new {value=@Model.LiftingOrTransfers2,  @class="sd", @rows="2", @cols="160" })%>
                </td>
            </tr>
            <tr>
                <td class="lblSpan" colspan="7"><strong>Ambulation<span class="auto-style1"> (How does this person get from place to place)</span></strong></td>
            </tr>
            <tr>
                <td class="auto-style2" colspan="7"><%=Html.TextAreaFor(m => m.Ambulation1,new {value=@Model.Ambulation1,  @class="sd", @rows="3", @cols="160" })%>
                    <%=Html.TextAreaFor(m => m.Ambulation2,new {value=@Model.Ambulation2,  @class="sd", @rows="3", @cols="160" })%></td>
            </tr>
            <tr>
                <td class="lblSpan" colspan="7"><strong>Toileting<span class="auto-style1"> (General Information - What are they able to do - What kind of support / Assistance is needed)</span></strong></td>
            </tr>
            <tr>
                <td class="auto-style3" colspan="7"><%=Html.TextAreaFor(m => m.Toileting1,new {value=@Model.Toileting1,  @class="sd", @rows="2", @cols="160" })%>
                    <%=Html.TextAreaFor(m => m.Toileting2,new {value=@Model.Toileting2,  @class="sd", @rows="3", @cols="160" })%></td>
            </tr>
            <tr>
                <td class="lblSpan" colspan="7"><strong>Eating <span class="auto-style1">(General Information - What are they able to do - What kind of support / Assistance is needed)</span></strong></td>
            </tr>
            <tr>
                <td class="auto-style2" colspan="7"><%=Html.TextAreaFor(m => m.Eating1,new {value=@Model.Eating1,  @class="sd", @rows="3", @cols="160" })%>
                    <%=Html.TextAreaFor(m => m.Eating2,new {value=@Model.Eating2,  @class="sd", @rows="3", @cols="160" })%></td>
            </tr>
            <tr>
                <td class="lblSpan" colspan="7"><strong>Showering <span class="auto-style1">(General Information - What are they able to do - What kind of support / Assistance is needed)</span></strong></td>
            </tr>
            <tr>
                <td class="auto-style2" colspan="4"><%=Html.TextAreaFor(m => m.Showering1,new {value=@Model.Showering1,  @class="sd", @rows="3", @cols="160" })%>
                    <%=Html.TextAreaFor(m => m.Showering2,new {value=@Model.Showering2,  @class="sd", @rows="3", @cols="160" })%></td>
            </tr>
            <tr>
                <td class="lblSpan" colspan="7"><strong>Toothbrushing<span class="auto-style1"> (General Information - What are they able to do - What kind of support / Assistance is needed)</span></strong></td>
            </tr>
            <tr>
                <td class="auto-style2" colspan="7"><%=Html.TextAreaFor(m => m.ToothBrushing1,new {value=@Model.ToothBrushing1,  @class="sd", @rows="3", @cols="160" })%>
                    <%=Html.TextAreaFor(m => m.ToothBrushing2,new {value=@Model.ToothBrushing2,  @class="sd", @rows="3", @cols="160" })%></td>
            </tr>
            <tr>
                <td class="lblSpan" colspan="7"><strong>Dressing<span class="auto-style1"> (General Information - What are they able to do - What kind of support / Assistance is needed)</span></strong></td>
            </tr>
            <tr>
                <td class="auto-style2" colspan="7"><%=Html.TextAreaFor(m => m.Dressing1,new {value=@Model.Dressing1,  @class="sd", @rows="3", @cols="160" })%>
                    <%=Html.TextAreaFor(m => m.Dressing2,new {value=@Model.Dressing2,  @class="sd", @rows="3", @cols="160" })%></td>
            </tr>
            <tr>
                <td class="lblSpan" colspan="7"><strong>Skin Care / Skin Integrity <span class="auto-style1">(General Information - Special care needs - What kind of support / Assistance is needed)</span></strong></td>
            </tr>
            <tr>
                <td class="auto-style2" colspan="7"><%=Html.TextAreaFor(m => m.SkinCare1,new {value=@Model.SkinCare1,  @class="sd", @rows="3", @cols="160" })%>
                    <%=Html.TextAreaFor(m => m.SkinCare2,new {value=@Model.SkinCare2,  @class="sd", @rows="3", @cols="160" })%></td>
            </tr>--%>
            <%-- <tr>
                <td class="lblSpan" colspan="7"><strong>Communication<span class="auto-style1"> (How does this person communicate, Words used, Words to avoid, etc)</span></strong> </td>
            </tr>--%>
            <%--<tr>
                <td class="auto-style2" colspan="7">
                   <%-- <%=Html.TextAreaFor(m => m.Communication2,new {value=@Model.Communication2,  @class="sd", @rows="3", @cols="160" })%></td>
            </tr>--%>

            <%--<tr>
                <td class="auto-style2" colspan="7"><strong>Adaptive Equipment/Health related protections</strong></td>
            </tr>
            <tr>
                <td class="auto-style2" style="text-align: center">Item</td>
                <td class="auto-style4" style="text-align: center">Scheduled for Use</td>
                <td class="auto-style3" style="text-align: center">Storage Location</td>
                <td class="nobdr" style="text-align: center">Cleaning Instructions</td>

            </tr>--%>
            <%-- Second------ --%>

            <%--<% int cnt = Model.Adapt.Count;
               for (int i = 0; i < 3; i++)
               { %>
            <tr>
                <%if (cnt > 0)
                  {
                      if (Model.Adapt[i] != null)
                      { %>
                <td class="auto-style2" style="text-align: center"><%=Html.TextBoxFor(m => m.Adapt[i].item, Model.Adapt[i].item, new { @class = "", maxlength = 100 })%></td>
                <td class="auto-style4" style="text-align: center"><%=Html.TextBoxFor(m => m.Adapt[i].ScheduledForUss, Model.Adapt[i].ScheduledForUss, new { @class = " ", maxlength = 100 })%></td>
                <td class="auto-style3" style="text-align: center"><%=Html.TextBoxFor(m => m.Adapt[i].StorageLocation, Model.Adapt[i].StorageLocation, new { @class = " ", maxlength = 100 })%></td>
                <td class="nobdr" style="text-align: center"><%=Html.TextBoxFor(m => m.Adapt[i].CleaningInstruction, Model.Adapt[i].CleaningInstruction, new { @class = "", maxlength = 100 })%>

                    <%=Html.HiddenFor(m => m.Adapt[i].AdaptiveEquimentId )%>


                    <%  cnt--;
                  }
              }
                  else
                  { %>
                <td class="auto-style2" style="text-align: center">
                    <input type="text" value="" name="Adapt[<%=i %>].item" /></td>
                <td class="auto-style4" style="text-align: center">
                    <input type="text" value="" name="Adapt[<%=i %>].ScheduledForUss" /></td>
                <td class="auto-style3" style="text-align: center">
                    <input type="text" value="" name="Adapt[<%=i %>].StorageLocation" /></td>
                <td class="nobdr" style="text-align: center">
                    <input type="text" value="" name="Adapt[<%=i %>].CleaningInstruction" />
                    <input type="hidden" value="" name="Adapt[<%=i %>].AdaptiveEquimentId" />

                    <%}
                  
                    %>
            </tr>
            <%} %>--%>
            <%--<tr>
                <td class="auto-style2" colspan="7"><strong>Basic Behavioral Information</strong></td>
            </tr>
            <tr>
                <td>
                    <label class="align-label">Target Behavior</label><span class="nospan-align">*</span></td>
                <td>
                    <label class="align-label">Definition</label><span class="nospan-align">*</span></td>
                <td class="auto-style3" colspan="2" style="text-align: left">Response / Strategy</td>
            </tr>--%>
            <%-- third------------ --%>
            <%--<% int cnt1 = Model.BasicBehave.Count;
               for (int i = 0; i < 3; i++)
               { %>

            <%if (cnt1 > 0)
              {
                  if (Model.BasicBehave[i] != null)
                  { %>
            <tr>
                <td class="auto-style2" rowspan="3">
                    <%=Html.HiddenFor(m => m.BasicBehave[i].BasicBehavioralInformationId )%>
                    <%=Html.TextAreaFor(m => m.BasicBehave[i].TargetBehavior, 
           new { value = @Model.BasicBehave[i].TargetBehavior, @class = "sd", @rows = "2", @cols = "160",@style="height: 117px; width: 200px;"})%></td>
                <td class="auto-style4" rowspan="3"><%=Html.TextAreaFor(m => m.BasicBehave[i].Definition, 
          new { value = @Model.BasicBehave[i].Definition, @class = "sd", @rows = "2", @cols = "160" ,@style="height: 117px; width: 200px;" })%></td>
                <td colspan="2">
                    <label class="lblSpan">Antecedent</label><span class="nospan-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.BasicBehave[i].Antecedent, Model.BasicBehave[i].Antecedent, new { @class = "", maxlength = 100 })%></td>
            </tr>
            <tr>
                <td colspan="2">
                    <label class="lblSpan">Fct</label><span class="nospan-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.BasicBehave[i].FCT, Model.BasicBehave[i].FCT, new { @class = "", maxlength = 100 })%></td>
            </tr>
            <tr>
                <td colspan="2">
                    <label class="lblSpan">Consequences</label><span class="nospan-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.BasicBehave[i].Consequances, Model.BasicBehave[i].Consequances, new { @class = "", maxlength = 100 })%></td>
            </tr>
            <% cnt1--;
              }
          }
              else
              { %>
            <tr>
                <td class="auto-style2" rowspan="3">
                    <input type="hidden" value="" name="BasicBehave[<%=i %>].BasicBehavioralInformationId" />

                    <textarea style="height: 117px; width: 200px;" name="BasicBehave[<%=i %>].TargetBehavior" />


                </td>
                <td class="auto-style4" rowspan="3">
                    <textarea style="height: 117px; width: 200px;" name="BasicBehave[<%=i %>].Definition" />


                </td>
                <td colspan="2">
                    <label class="lblSpan">Antecedent</label><span class="nospan-align">*</span><br />

                    <input type="text" value="" name="BasicBehave[<%=i %>].Antecedent" />


                </td>
            </tr>
            <tr>

                <td colspan="2">
                    <label class="lblSpan">Fct</label><span class="nospan-align">*</span><br />

                    <input type="text" value="" name="BasicBehave[<%=i %>].FCT" />


                </td>
            </tr>
            <tr>

                <td colspan="2">
                    <label class="lblSpan">Consequences</label><span class="nospan-align">*</span><br />

                    <input type="text" value="" name="BasicBehave[<%=i %>].Consequances" />


                </td>
            </tr>
            <%}
           } %>--%>
            <%--<tr>
                <td class="lblSpan" colspan="7"><strong>Preferred Activities</strong></td>
            </tr>
            <tr>
                <td class="auto-style2" colspan="7"><%=Html.TextAreaFor(m => m.preferedActivities1,new {value=@Model.preferedActivities1,  @class="sd", @rows="3", @cols="160" })%>
                    <%=Html.TextAreaFor(m => m.preferedActivities2,new {value=@Model.preferedActivities2,  @class="sd", @rows="3", @cols="160" })%></td>
            </tr>
            <tr>
                <td class="lblSpan" colspan="7"><strong>General Information<span class="auto-style1"> (Important to know, What gets this person upset, What makes them happy, Things to watch out for, Any important structure needed in their day, Routines, Preferred items)</span></strong></td>
            </tr>
            <tr>
                <td class="auto-style2" colspan="7"><%=Html.TextAreaFor(m => m.GeneralInformation1,new {value=@Model.GeneralInformation1,  @class="sd", @rows="3", @cols="160" })%>
                    <%=Html.TextAreaFor(m => m.GeneralInformation2,new {value=@Model.GeneralInformation2,  @class="sd", @rows="3", @cols="160" })%></td>
            </tr>
            <tr>
                <td class="lblSpan" colspan="7"><strong>Suggested Proactive Environmental Procedures<span class="auto-style1"> (What can be done in the environment to support this person, How should the staff respond if this person is upset, etc)</span></strong></td>
            </tr>
            <tr>
                <td class="auto-style2" colspan="7"><%=Html.TextAreaFor(m => m.SuggestedProactiveEnvironmentalProcedures1,new {value=@Model.SuggestedProactiveEnvironmentalProcedures1,  @class="sd", @rows="3", @cols="160" })%>
                    <%=Html.TextAreaFor(m => m.SuggestedProactiveEnvironmentalProcedures2,new {value=@Model.SuggestedProactiveEnvironmentalProcedures2,  @class="sd", @rows="3", @cols="160" })%></td>
            </tr>--%>
        </table>

        <table style="width: 100%;">

            <tr>
                <td class="auto-style2" colspan="7">
                   <%--  <h4>Client Address (N/A if Melmark Resident)</h4> --%>
                      <h4>Primary Address</h4>
                </td>
            </tr>
        </table>
        <table style="width: 70%;">
            <tr>
                <td colspan="2">
                    <label class="lblSpan">Street Address </label>
                    <span class="nospan-align">*</span><br />

                    <%=Html.TextBoxFor(m => m.AddressLine2, Model.AddressLine2, new {  maxlength=50,@class="newClass" })%></td>
            </tr>
            <tr>
                <td colspan="2">
                    <label class="lblSpan">Unit # </label>
                    <span class="nospan-align">*</span><br />

                    <%=Html.TextBoxFor(m => m.AddressLine1, Model.AddressLine1, new {  maxlength=50,@class="newClass" })%></td>
            </tr>
            <tr>
                <td colspan="2">
                    <label class="lblSpan">City</label><span class="nospan-align">*</span><br />

                    <%=Html.TextBoxFor(m => m.City, Model.City, new {  @class="sd newClass" })%></td>
            </tr>
            <tr>
                <td colspan="2">
                    <label class="lblSpan">State</label><span class="nospan-align">*</span><br />

                    <%=Html.DropDownListFor(m => m.State, Model.StateList, new {  @class="sd newClass",ID = "ddlState" })%></td>

            </tr>
            <tr>
                <td colspan="2">
                    <label class="lblSpan">Zip</label><span class="nospan-align">*</span><br />

                    <%=Html.TextBoxFor(m => m.ZipCode, Model.ZipCode, new {  @class="sd zipDefault newClass",onpaste="funPaste(event);",maxlength=5,ID = "" })%></td>


            </tr>

            <tr>
                <td colspan="2">
                    <label class="lblSpan">Country</label><span class="nospan-align">*</span><br />

                    <%=Html.DropDownListFor(m => m.Country, Model.CountryList, new {  @class="sd newClass",ID = "ddlCountry" })%></td>
                <td colspan="2"></td>
            </tr>

            <tr>
                <td colspan="2">
                    <label class="lblSpan">Phone</label><span class="nospan-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.ClientAddressPhone, Model.ClientAddressPhone, new {  @class="validate[custom[usPhoneNumber]] usPhone newClass",onpaste="funPaste(event);"})%>
                </td>
            </tr>

        </table>


        <table style="width: 100%;">

            <tr>
                <td class="auto-style2" colspan="7">
                    <h4>Photo Release and Trip Restrictions</h4>
                </td>
            </tr>
        </table>
        <table>
            <tr>
                <td colspan="2">
                    <label class="lblSpan">Photo Release Permission</label><span class="nospan-align">*</span><br />
                    <%=Html.DropDownListFor(m => m.PhotoReleasePermission, Model.YesNo, new { maxlength=50,@class="newClass"})%></td>
            </tr>
            <tr>
                <td colspan="2">
                    <label class="lblSpan">Photo Permission Comment</label><span class="nospan-align"></span><br />
                    <%=Html.TextAreaFor(m => m.PhotoPermComment,new {value=@Model.PhotoPermComment,  @class="sd newClass", @rows="3",@cols="30", Style="width:100%;"})%>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <label class="lblSpan">Photo Release History</label><span class="nospan-align"></span><br />
                    <%=Html.TextAreaFor(m => m.PhotoRelease  , new {value=@Model.PhotoRelease,  @class="sd newClass", @rows="3",@cols="30", Style="width:100%;"})%>
            </tr>

            <tr>
                <td colspan="2">
                    <label class="lblSpan">Trip Restriction 1</label><span class="nospan-align"></span><br />
                    <%=Html.TextAreaFor(m => m.TripRestriction1 ,new {value=@Model.TripRestriction1,  @class="sd newClass", @rows="3",@cols="30", Style="width:100%;"})%>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <label class="lblSpan">Trip Restriction 2 </label>
                    <span class="nospan-align"></span>
                    <br />
                    <%=Html.TextAreaFor(m => m.TripRestriction2  , new {value=@Model.TripRestriction2,  @class="sd newClass", @rows="3",@cols="30", Style="width:100%;"})%>
                </td>
            </tr>

            <tr>
                <td colspan="2">
                    <label class="lblSpan">Trip Comments</label><span class="nospan-align"></span><br />
                    <%=Html.TextAreaFor(m => m.TripComments ,new {value=@Model.TripComments,  @class="sd newClass", @rows="3",@cols="30", Style="width:100%;"})%>
                </td>
            </tr>


            <tr>
                <td>
                    <label class="lblSpan">Notes</label><span class="nospan-align"></span><br />
                    <%=Html.TextAreaFor(m => m.Note,new {value=@Model.Note,  @class="sd newClass", @rows="3",@cols="30", Style="width:100%;"})%>
                </td>

            </tr>
        </table>

        <table>
            <tr>

                <td>
                    <% if (ViewBag.permission == "true")
                       {
                           if (Model.Id > 0)
                           {
                               if (Session["PlacementStat"].ToString() != "I")
                               {
                    %>
                    <input id="btnUpdate" type="submit" value="Update" style="margin-left: 5%" />
                    <%}
                           }
                           else
                           {%>
                    <input id="btnSubmit" type="submit" value="Save" style="margin-left: 5%" /><% }
                       }%>
                </td>
                <td></td>
                <td></td>
            </tr>
        </table>

        <div id="divQuickView" class="popUpStyle" style="height: 75%; width: 75%; z-index: 1010; padding: 12px;overflow-y: auto;overflow-x: hidden; border-radius: 18px; left: 11%; top: 5%; position: fixed;">
            <table style="width: 100%">
            </table>
            <div id="close" onclick="javascript:$('#divQuickView').hide();event.preventDefault();" href="#" style="position:fixed;cursor:pointer; width: 73%;margin-top: 6px">
                <img src="../../Images/delete.png" alt="" style=" width:auto;height:auto; z-index: 300" />
            </div>
            <div id="LoadDiv"></div>
        </div>

        <div class="rightContainer_new" style="display: inline-block !important; float: right; margin-right: -27%; padding-top: 3px; width: 27%; min-width: 215px;">
            <div id="rightSidePanel" style="margin-left: 30px ! important; top: 177px ! important; position: absolute ! important; right: 15px; width: 200px">
                <table>
                    <tr>
                        <%if (Model.Id > 0)
                          { 
                        %>

                        <td id="tdstudentName" class="nobdr" style="display: none;"><%=Model.LastName %>&nbsp; <%=Model.LastNameSuffix %>, <%=Model.FirstName %></td>
                        <%}
                          else
                          { } %>
                    </tr>
                    <tr>
                        <%if (Model.Id > 0)
                          {%>
                        <td style="display: none;"><%=Model.StudentId %></td>
                        <% }
                          else
                          { } %>
                    </tr>
                    <tr>

                        <%-- <%=Html.DisplayFor(m=>m.ImageUrl) %>--%>
                        <td>
                            <img id="stdImage" alt="No Image Uploaded" src="data:image/gif;base64,<%=Model.ImageUrl %>" class="newClass"/>
                            <div id="newPreview"></div>
                            <div id="cancelPreview" onclick="cancelPreview()" style="display: none;">Cancel</div>

                            <span id="hdnPrevImg1" style="display:none;">data:image/gif;base64,<%=Model.ImageUrl %></span>
                        </td>

                    </tr>
                    <tr>

                        <%if (Session["PlacementStat"].ToString() != "I")
                          {
                              if (Model.ImageUrl != null)
                              {%>

                        <td>
                            <input type="file" id="profilePicture" name="profilePicture" style="width: 180px;" />
                            <%-- <%=Html.TextBoxFor(m => m.profilePicture, new { type = "file" })%>--%>
                            <br /><br />
                            
                             <label class="lblSpan">Photo Date Taken: </label>
                            <%=Html.TextBoxFor(m => m.Photodate, Model.Photodate, 
                             new {  @class="datepicker", ID = "Photodate", Style="width:70px;font-size:10px;" })%>


                           
                        </td>
                        <% }
                              else
                              {  %>
                        <td>
                            <input type="file" id="File1" name="profilePicture" style="width: 180px; float: left;"
                                class="" /><br /><br />
                            
                             <label class="lblSpan">Photo Date Taken: </label>
                            <%=Html.TextBoxFor(m => m.Photodate, Model.Photodate, 
                             new {  @class="datepicker", ID = "Photodate", Style="width:70px;font-size:10px;" })%>

                        </td>
                        <%-- <%=Html.TextBoxFor(m => m.profilePicture, new { type = "file" })%>--%>
                        <%}
                          } %>
                    </tr>
                    <tr>

                        <%if (Model.PhotoReleasePermission == true)
                          { %>

                        <%}
                          else
                          { %>

                        <%} %>
                        <%-- <input id="chkPhotoPermission" type="checkbox" onchange="" />Photo Release Permitted</td>--%>
                    </tr>

                </table>
            </div>
            <div>
            </div>


        </div>
        <div class="clear"></div>

        <%=Html.TextAreaFor(m => m.eventLogNote,new {Style="width:200px; height:200px;display:none; position:fixed; top:0px; left:0px;", ID="tickerBox"})%>

        <input type="hidden" id="PreviousVal" value="" />


        <%--<table style="width: 100%;">
            <tr>
                <td class="auto-style2" colspan="7">
                    <h4>Level of Supervision</h4>
                </td>
            </tr>
        </table>
        <table style="width: 70%;">
            <tr>
                <td class="auto-style2" style="text-align: center" colspan="2">GENERAL</td>
            </tr>
            <tr>
                <td colspan="2">
                    <label class="lblSpan">Bathroom</label><span class="nospan-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.Bathroom, Model.Bathroom, new {  @class="",maxlength=250 })%></td>
            </tr>
            <tr>
                <td colspan="2">
                    <label class="lblSpan">On Campus</label><span class="nospan-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.OnCampus, Model.OnCampus, new {  @class="",maxlength=250 })%></td>
            </tr>
            <tr>
                <td colspan="2">
                    <label class="lblSpan">When Transporting</label><span class="nospan-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.WhenTranspoting, Model.WhenTranspoting, new {  @class="",maxlength=250 })%></td>
            </tr>
            <tr>
                <td colspan="2">
                    <label class="lblSpan">Off Campus</label><span class="nospan-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.OffCampus, Model.OffCampus, new {  @class="",maxlength=250 })%></td>
            </tr>
            <tr>
                <td colspan="2">
                    <label class="lblSpan">Pool / Swimming</label><span class="nospan-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.PoolOrSwimming, Model.PoolOrSwimming, new {  @class="",maxlength=250 })%></td>
            </tr>
            <tr>

                <td colspan="2">
                    <label class="lblSpan">Van</label><span class="nospan-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.van, Model.van, new {  @class="",maxlength=250 })%></td>

            </tr>
            <tr>
                <td class="auto-style2" colspan="2" style="text-align: center">HOME / RESIDENTIAL</td>
                <td class="auto-style3" colspan="2" style="text-align: center">DAY PROGRAM</td>
            </tr>
            <tr>
                <td colspan="2">
                    <label class="lblSpan">Common Areas</label><span class="nospan-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.CommonAreas, Model.CommonAreas, new {  @class="[] ",maxlength=250 })%></td>
                <td colspan="2">
                    <label class="lblSpan">Task / Break</label><span class="nospan-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.TaskORBreak, Model.TaskORBreak, new {  @class="[] ",maxlength=250 })%></td>

            </tr>
            <tr>
                <td colspan="2">
                    <label class="lblSpan">Bedroom Awake</label><span class="nospan-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.BedroomAwake, Model.BedroomAwake, new {  @class="[] ",maxlength=250 })%></td>
                <td colspan="2">
                    <label class="lblSpan">Transition Inside</label><span class="nospan-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.TransitionInside, Model.TransitionInside, new {  @class="[] ",maxlength=250 })%></td>

            </tr>
            <tr>
                <td colspan="2">
                    <label class="lblSpan">Bedroom Asleep</label><span class="nospan-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.BedroomAsleep, Model.BedroomAsleep, new {  @class="[] ",maxlength=250 })%></td>
                <td colspan="2">
                    <label class="lblSpan">Transition Uneven Ground</label><span class="nospan-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.TransitionUnevenGround, Model.TransitionUnevenGround, new {  @class="[] ",maxlength=250 })%></td>

            </tr>
        </table>
        <table style="width: 100%;">

            <tr>
                <td class="auto-style2" colspan="7">
                    <h4>Education Skills<span class="auto-style1"> (How does this person evacuate during fire drills, What assistance do they need)</span></h4>
                </td>
            </tr>
        </table>
        <table style="width: 100%;">
            <tr>
                <td colspan="2">
                    <label class="lblSpan">Risk Of Resistance</label><span class="nospan-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.RiskOfResistance, Model.RiskOfResistance, new {  @class="[] ",maxlength=250 })%></td>
            </tr>
            <tr>
                <td colspan="2">
                    <label class="lblSpan">Mobility</label><span class="nospan-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.Mobility, Model.Mobility, new {  @class="[] ",maxlength=250 })%></td>
            </tr>
            <tr>
                <td colspan="2">
                    <label class="lblSpan">Need For Extra Help</label><span class="nospan-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.NeedForExtraHelp, Model.NeedForExtraHelp, new {  @class="[] ",maxlength=250 })%></td>
            </tr>
            <tr>
                <td colspan="2">
                    <label class="lblSpan">Response To Instruction</label><span class="nospan-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.ResponseToInstruction, Model.ResponseToInstruction, new {  @class="[] ",maxlength=250 })%></td>
            </tr>
            <tr>
                <td colspan="2">
                    <label class="lblSpan">Consciousness</label><span class="nospan-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.Consciousness, Model.Consciousness, new {  @class="[] ",maxlength=250 })%></td>
            </tr>
            <tr>
                <td colspan="2">
                    <label class="lblSpan">Walking Response</label><span class="nospan-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.WalkingResponse, Model.WalkingResponse, new {  @class="[] ",maxlength=250 })%></td>
            </tr>
            </table>--%>

        <script type="text/javascript">

            $("#btnUpdate").on("click", function (event) {
                WriteToString(Arr_NewEventLogsParent);
            });

            var Arr_Record = [];
            var Arr_Ticker = [];
            var Arr_NewEventLogsParent = [];
            var Arr_NewEventLogsChild = [];

            $('.newClass').focus(function () {
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
                Arr_Record = [controlId, controlType, labelText, prevValue, newValue, message];
                Arr_NewEventLogsChild = [labelText, prevValue, newValue];
                $('#PreviousVal').val(prevValue);
            });
            $('.newClass').blur(function () {
                var controlId = this.id;
                var controlType = this.type;
                var prevValue = Arr_Record[3];
                var newValue = "";
                var labelText = $(this).siblings('.lblSpan').text();
                var message = "";

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
                        if (controlType == "text") {
                            if ($('#' + controlId).hasClass('hasDatepicker')) {
                                return;
                            }
                        }
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


                            //if (labelText == "Guardian(Self)") {
                            //    newEvent = "Guardianship|||" + newControlType + "|||" + labelText + "|||" + prevValue + "|||" + newValue;
                            //    document.getElementById('newEventLog').value += newEvent + ">>>";
                            //}
                            //if (labelText == "Funding Source") {
                            //    newEvent = "Funder|||" + newControlType + "|||" + labelText + "|||" + prevValue + "|||" + newValue;
                            //    document.getElementById('newEventLog').value += newEvent + ">>>";
                            //}

                            var pageType = "";
                            if (labelText == "Guardian(Self)") {
                                pageType = "Guardianship";
                            }
                            if (labelText == "Funding Source") {
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
                for (var i = 0; i < para_Arr_NewEventLogsParent.length; i++) {
                    newEvent = para_Arr_NewEventLogsParent[i][0] + "|||" + para_Arr_NewEventLogsParent[i][1] + "|||" + para_Arr_NewEventLogsParent[i][2] + "|||" + para_Arr_NewEventLogsParent[i][3] + "|||" + para_Arr_NewEventLogsParent[i][4];
                    document.getElementById('newEventLog').value += newEvent + ">>>";
                }
            }

        </script>
        <script>loadClientStaticDetails();</script>
