<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ClientDB.Models.RegistrationModel>" %>
<%--<script src="../../Documents/JS/jquery-1.8.2.js"></script>--%>

<%-- commected by benhur 05-28-2015 , Coe it is already loading from its main page..
<link href="../../Documents/CSS/jquery-ui.css" rel="stylesheet" />
<script src="../../Documents/JS/jquery-ui-1.8.24.js"></script>

<script src="../../Documents/JS/jquery.validationEngine-en.js"></script>
<script src="../../Documents/JS/jquery.validationEngine.js"></script>
<script src="../../Documents/JS/jquery.form.js"></script>
<script src="../../Documents/JS/jquery.mask.js"></script>
--%>
<script src="../../Documents/JS/jquery.validationEngine-en.js"></script>
<script src="../../Documents/JS/jquery.validationEngine.js"></script>
<style type="text/css">
    #rightSidePanel {
        margin-left: 30px !important;
        position: absolute !important;
        right: 15px;
        top: 177px !important;
        width: 200px;
    }

    .lblSpan {
        font-style: italic;
        font-weight: bold;
    }

    p.MColumnHead {
        margin-bottom: .0001pt;
        page-break-after: avoid;
        punctuation-wrap: simple;
        text-autospace: none;
        font-size: 10.0pt;
        font-family: "Arial","sans-serif";
        font-weight: bold;
        margin-left: 0cm;
        margin-right: 0cm;
        margin-top: 0cm;
    }

    p.MTableText {
        margin: 3.0pt 0cm;
        punctuation-wrap: simple;
        text-autospace: none;
        font-size: 9.0pt;
        font-family: "Times New Roman","serif";
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
        margin-bottom: 1px;
    }

    select {
        width: 167px !important;
        border: 1px solid #CCC;
        height: 29px;
        vertical-align: middle;
        font-family: Arial;
        font-size: 12px;
        color: #333;
        margin-bottom: 1px;
    }


    .ui-datepicker select.ui-datepicker-month, .ui-datepicker select.ui-datepicker-year {
        width: 49% !important;
    }

    div.mainContainer div.contentPart div.ContentAreaContainer div.middleContainer table tr td {
        color: #525252;
        font-family: Arial,Helvetica,sans-serif;
        font-size: 11px !important;
        width: 55% !important;
    }

    .label-to {
        margin-left: 70px !important;
        width: 24px !important;
        margin: 6px 25px 0 15px;
        float: left;
    }
    #cancelPreview{
        cursor:pointer;
        color:red;
        text-decoration:underline;
        width:180px;
        text-align:center;
    }
</style>
<script type="text/javascript">

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
            var regex = /^([a-zA-Z0-9\s_\\.\-:])+(.jpg|.jpeg|.gif|.png|.bmp)$/;
            if ($(this).val() != "") {
                if (regex.test($(this).val().toLowerCase())) {
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
                        }
                        reader.readAsDataURL($(this)[0].files[0]);
                    } else {
                        alert("This browser does not support FileReader.");
                    }
                } else {
                    alert("Please upload a valid image file.");
                }
            }
        });
    });



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
        $('.namefield').keypress(function (event) {
            var inputValue = event.which;
            if (((inputValue >= 65 && inputValue <= 90) || (inputValue >= 97 && inputValue <= 122) || (inputValue == 32) || (inputValue == 39) || (inputValue == 45) || (inputValue == 8) || (inputValue == 0))) {
            }
            else {
                event.preventDefault();
            }
        });

        $('.imgcontainer').css("display", "none");
        // $(".datepicker").datepicker();admissionDate 
        //$(".datepicker").datepicker({
        //    changeMonth: true,
        //    changeYear: true,
        //    showAnim: "fadeIn",
        //    yearRange: 'c-30:c+30',


        //    /* fix buggy IE focus functionality */
        //    fixFocusIE: false,

        //    /* blur needed to correctly handle placeholder text */
        //    onSelect: function (dateText, inst) {
        //        this.fixFocusIE = true;
        //        $(this).blur().change().focus();
        //    },
        //    onClose: function (dateText, inst) {
        //        this.fixFocusIE = true;
        //        this.focus();
        //    },
        //    beforeShow: function (input, inst) {
        //        var result = $.browser.msie ? !this.fixFocusIE : true;
        //        this.fixFocusIE = false;
        //        return result;
        //    }
        //}).attr('readonly', 'true');

        $("#CurrentIEPStartDate").datepicker({
            changeMonth: true,
            changeYear: true,
            showAnim: "fadeIn",
            yearRange: 'c-30:c+30',
            maxDate: new Date,


            /* fix buggy IE focus functionality */
            fixFocusIE: true
        });

        //$("#CurrentIEPExpirationDate").datepicker({
        //    changeMonth: true,
        //    changeYear: true,
        //    showAnim: "fadeIn",
        //    yearRange: 'c-30:c+30',
        //    maxDate: new Date,


        //    /* fix buggy IE focus functionality */
        //    fixFocusIE: true
        //});


        $('#CurrentIEPExpirationDate').datepicker({
            changeMonth: true,
            changeYear: true,
            showAnim: "fadeIn",
            yearRange: 'c-30:c+30',


            /* fix buggy IE focus functionality */
            fixFocusIE: true,

            /* blur needed to correctly handle placeholder text */
            //onSelect: function (dateText, inst) {
            //    // this.fixFocusIE = true;
            //    // $(this).blur().change().focus();
            //    var dt1 = $('#CurrentIEPExpirationDate');
            //    var startDate = $(this).datepicker('getDate');
            //    dt1.datepicker('option', 'maxDate', startDate);
            //}
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


        $("#DischargeDate").datepicker({
            changeMonth: true,
            changeYear: true,
            showAnim: "fadeIn",
            yearRange: 'c-30:c+30',
            maxDate: new Date,


            /* fix buggy IE focus functionality */
            fixFocusIE: true
        });

        $("#DateInitiallyEligibleforSpecialEducation").datepicker({
            changeMonth: true,
            changeYear: true,
            showAnim: "fadeIn",
            yearRange: 'c-30:c+30',
            maxDate: new Date,


            /* fix buggy IE focus functionality */
            fixFocusIE: true
        });

        $("#DateofMostRecentSpecialEducationEvaluations").datepicker({
            changeMonth: true,
            changeYear: true,
            showAnim: "fadeIn",
            yearRange: 'c-30:c+30',
            maxDate: new Date,


            /* fix buggy IE focus functionality */
            fixFocusIE: true
        });

        $("#DateofNextScheduled3YearEvaluation").datepicker({
            changeMonth: true,
            changeYear: true,
            showAnim: "fadeIn",
            yearRange: 'c-30:c+30',
            maxDate: new Date,


            /* fix buggy IE focus functionality */
            fixFocusIE: true
        });

        $("#admissionDate").datepicker({
            changeMonth: true,
            changeYear: true,
            showAnim: "fadeIn",
            yearRange: 'c-30:c+30',
            maxDate: new Date,


            /* fix buggy IE focus functionality */
            fixFocusIE: true

            /* blur needed to correctly handle placeholder text */
            //onSelect: function (dateText, inst) {
            //    this.fixFocusIE = true;
            //    $(this).blur().change().focus();
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

        $("#dateOfBirth").datepicker({
            changeMonth: true,
            changeYear: true,
            showAnim: "fadeIn",
            yearRange: 'c-30:c+30',
            maxDate: new Date,


            /* fix buggy IE focus functionality */
            fixFocusIE: true,

            /* blur needed to correctly handle placeholder text */
            onSelect: function (dateText, inst) {
                //this.fixFocusIE = true;
                // $(this).blur().change().focus();
                var dt1 = $('#admissionDate');
                var startDate = $(this).datepicker('getDate');
                dt1.datepicker('option', 'minDate', startDate);
            }

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


        $("#DateFrom1").datepicker({
            changeMonth: true,
            changeYear: true,
            showAnim: "fadeIn",
            yearRange: 'c-30:c+30',
            /* fix buggy IE focus functionality */
            fixFocusIE: true,
            /* blur needed to correctly handle placeholder text */
            //onClose: function (dateText, inst) {
            //    this.fixFocusIE = true;
            //    this.focus();
            //},
            //beforeShow: function (input, inst) {
            //    var result = $.browser.msie ? !this.fixFocusIE : true;
            //    this.fixFocusIE = false;
            //    return result;
            //},
            onSelect: function (dateText, inst) {
                //this.fixFocusIE = true;
                // $(this).blur().change().focus();
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


            /* fix buggy IE focus functionality */
            fixFocusIE: true,

            /* blur needed to correctly handle placeholder text */
            onSelect: function (dateText, inst) {
                // this.fixFocusIE = true;
                // $(this).blur().change().focus();
                var dt1 = $('#DateFrom1');
                var startDate = $(this).datepicker('getDate');
                dt1.datepicker('option', 'maxDate', startDate);
            }
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

        $(document).ready(function () {
            $("#DateFrom2").datepicker({
                changeMonth: true,
                changeYear: true,
                showAnim: "fadeIn",
                yearRange: 'c-30:c+30',
                /* fix buggy IE focus functionality */
                fixFocusIE: true,
                /* blur needed to correctly handle placeholder text */
                //onClose: function (dateText, inst) {
                //    this.fixFocusIE = true;
                //    this.focus();
                //},
                //beforeShow: function (input, inst) {
                //    var result = $.browser.msie ? !this.fixFocusIE : true;
                //    this.fixFocusIE = false;
                //    return result;
                //},
                onSelect: function (dateText, inst) {
                    // this.fixFocusIE = true;
                    // $(this).blur().change().focus();
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


                /* fix buggy IE focus functionality */
                fixFocusIE: true,

                /* blur needed to correctly handle placeholder text */
                onSelect: function (dateText, inst) {
                    // this.fixFocusIE = true;
                    // $(this).blur().change().focus();
                    var dt1 = $('#DateFrom2');
                    var startDate = $(this).datepicker('getDate');
                    dt1.datepicker('option', 'maxDate', startDate);
                }
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
        });
        $(document).ready(function () {
            $("#DateFrom3").datepicker({
                changeMonth: true,
                changeYear: true,
                showAnim: "fadeIn",
                yearRange: 'c-30:c+30',
                /* fix buggy IE focus functionality */
                fixFocusIE: true,
                /* blur needed to correctly handle placeholder text */
                //onClose: function (dateText, inst) {
                //    this.fixFocusIE = true;
                //    this.focus();
                //},
                //beforeShow: function (input, inst) {
                //    var result = $.browser.msie ? !this.fixFocusIE : true;
                //    this.fixFocusIE = false;
                //    return result;
                //},
                onSelect: function (dateText, inst) {
                    // this.fixFocusIE = true;
                    //  $(this).blur().change().focus();
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


                /* fix buggy IE focus functionality */
                fixFocusIE: false,

                /* blur needed to correctly handle placeholder text */
                onSelect: function (dateText, inst) {
                    // this.fixFocusIE = true;
                    //   $(this).blur().change().focus();
                    var dt1 = $('#DateFrom3');
                    var startDate = $(this).datepicker('getDate');
                    dt1.datepicker('option', 'maxDate', startDate);
                }
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
        });
    });
    // var regex = /^[a-zA-Z ]*$/;
    //HairColor
    $(function () {
        $('#EyeColor').keyup(function (e) {
            var textValue = $(this).val().trim();
            var englishValue = /[a-zA-Z\ ]/;
            for (var i = 0; i < textValue.length; i++) {

                if (!englishValue.test(textValue[i])) {
                    return false;
                }
            }
            return true;
        });
    });


    jQuery('.charectorsOnly').keypress(function (event) {

        var textValue = (event.which) ? event.which : event.keyCode;
        var number = /[a-zA-Z ]*/;
        var value = $(this).val();
        var valueSplit = value.split('.');

        if (textValue == 8 || textValue == 9 || textValue == 45 || textValue == 46 || number.test(String.fromCharCode(textValue))) {
            if (valueSplit.length > 1) {
                if (textValue == 46) {
                    return false;
                }

                if (value.length > 50) {
                    return false;
                }

            }
            else {
                if (textValue != 46 && textValue != 8) {
                    if (value.length > 2) {
                        return false;
                    }
                }
            }
            return true;
        }
        return false;
    });

    jQuery('.numbersOnly').keypress(function (event) {

        var textValue = (event.which) ? event.which : event.keyCode;
        var number = /[0-9]/;
        var value = $(this).val();
        var valueSplit = value.split('.');

        if (textValue == 8 || textValue == 9 || textValue == 45 || textValue == 46 || number.test(String.fromCharCode(textValue))) {
            if (valueSplit.length > 1) {
                if (textValue == 46) {
                    return false;
                }

                if (value.length > 5) {
                    return false;
                }

            }
            else {
                if (textValue != 46 && textValue != 8) {
                    if (value.length > 2) {
                        return false;
                    }
                }
            }
            return true;
        }
        return false;
    });

    jQuery('.phoneNumbers').keypress(function (event) {

        var textValue = (event.which) ? event.which : event.keyCode;
        var number = "^(\([0-9]{3}\) |[0-9]{3}-)[0-9]{3}-[0-9]{4}";
        var value = $(this).val();
        var valueSplit = value.split('.');

        if (textValue == 8 || textValue == 9 || textValue == 45 || textValue == 46 || number.test(String.fromCharCode(textValue))) {


            if (valueSplit.length > 1) {
                if (textValue == 46) {
                    return false;
                }

                if (value.length > 5) {
                    return false;
                }

            }
            else {
                if (textValue != 46) {
                    if (value.length > 2) {
                        return false;
                    }

                }


            }



            return true;


        }


        return false;
    });
    //^(\([0-9]{3}\) |[0-9]{3}-)[0-9]{3}-[0-9]{4}$


    $('#ddlCountryOfBirth').change(function () {

        var countryId = $('#ddlCountryOfBirth').val();
        $.getJSON('../ClientRegistration/getStates', { countryid: countryId }, function (result) {
            var ddlState = $('#ddlStateOfBirth');
            $('#ddlStateOfBirth').empty();

            $.each(result, function (index, item) {
                ddlState.append($('<option/>', {
                    value: item.Value,
                    text: item.Text
                }));
            });
            //  $('#assistenceCityId').find(":selected").removeAttr('selected');
            $('#ddlStateOfBirth>option:eq(0)').attr('selected', true);
        });


    });





    $('#ddlStateOfBirth').change(function () {

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
    $('#ddlZip').change(function () {

    });
    $('#ddlGender').change(function () {

    });


    function Redirect() {
        alert("Data Saved Sucessfully");
        window.location.href = "../Client/ListClients?argument=*&bSort=false";

    }

    jQuery("#registrationForm").validationEngine();

    //$("#btnList").on("click", function (event) {
    //    //event.preventDefault();

    //    //if (jQuery("#registrationForm").validationEngine('validate')) {
    //    $('#content').load('/ClientRegistration/fillCLientDetails/');


    //    //}
    //});
    var options = {
        success: showResponse  // post-submit callback 
    };
    $('#registrationForm').ajaxForm(options);

    function showResponse(responseText, statusText, xhr, $form) {
        var item = responseText.split('|');
        if (item[0] == "Sucess") {
            $('#content').load('../ClientRegistration/ClientRegistration?data=' + item[1] + '|Fill');
            $('.imgcontainer').css("display", "block");
            $('.imgcontainer').load('../Contact/ImageUploadPanel');
            $('.EditProfile').css("display", "block");
            //window.location.reload(true);
        }
        else {

        }
    }
    function funPaste(e) {
        e.preventDefault();
    }

</script>

<%--<%using (@Ajax.BeginForm("SaveClients", "ClientRegistration", FormMethod.Post, new AjaxOptions { UpdateTargetId = "content", OnSuccess = "Redirect" }, new { id = "registrationForm", enctype = "multipart/form-data" }))
  { %>--%>

<%using (@Html.BeginForm("SaveClients", "ClientRegistration", FormMethod.Post, new { id = "registrationForm", enctype = "multipart/form-data" }))
  { %>

<div style="width: 100%">
    <div>
        <table style="width: 70%; display: inline-block;">


            <tr>
                <td colspan="1">
                    <label class="lblSpan">First Name</label><span class="span-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.FirstName, Model.FirstName, new {  @class="validate[required] namefield ",onpaste="funPaste(event);",maxlength=50 })%>
                </td>
                <td colspan="1">
                    <label class="lblSpan">Nick Name</label><span class="nospan-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.NickName, Model.NickName, new {  @class="sd namefield",onpaste="funPaste(event);",maxlength=50})%>
                </td>
            </tr>



            <tr>
                <td colspan="1">
                    <label class="lblSpan">Middle Name</label><span class="nospan-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.MiddleName, Model.MiddleName, new {  @class="namefield",onpaste="funPaste(event);",maxlength=50 })%>
                </td>
                <td colspan="1">
                    <label class="lblSpan">Last Name</label><span class="span-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.LastName, Model.LastName, new {  @class="validate[required] namefield",onpaste="funPaste(event);",maxlength=50 })%>
                </td>
            </tr>



            <tr>
                <td colspan="1">
                    <label class="lblSpan">Date of Birth </label>
                    <span class="span-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.DateOfBirth, Model.DateOfBirth, new { @class = "validate[required] datepicker", ID = "dateOfBirth" })%>
                </td>

                <td colspan="1">
                    <label class="lblSpan">Gender</label><span class="nospan-align">*</span><br />
                    <%=Html.DropDownListFor(m => m.Gender, Model.GenderList, new { @class = "sd", ID = "ddlGender" })%>
                </td>
            </tr>


            <tr>
                <td colspan="1">
                    <label class="lblSpan">Admission Date </label>
                    <span class="span-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.AdmissinDate, Model.AdmissinDate, new {  @class=" validate[required] datepicker",ID = "admissionDate" })%>
                </td>

                <td colspan="1">
                    <label class="lblSpan">Race</label><span class="nospan-align">*</span><br />
                    <%=Html.DropDownListFor(m => m.Race, Model.RaceList, new {  @class="sd",ID = "ddlRace" })%></td>
            </tr>


            <tr>
                <td colspan="1">
                    <label class="lblSpan">Place of Birth</label><span class="nospan-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.PlaceOfBirth, Model.PlaceOfBirth, new {  @class="sd",maxlength=50 })%></td>
                <td colspan="1">
                    <label class="lblSpan">Country of Birth</label><span class="nospan-align">*</span><br />
                    <%=Html.DropDownListFor(m => m.CountryofBirth, Model.CountryOfBirthList, new {  @class="sd",ID = "ddlCountryOfBirth" })%></td>
                <td>
            </tr>


            <tr>
                <td colspan="1">
                    <label class="lblSpan">State of Birth</label><span class="nospan-align">*</span><br />
                    <%=Html.DropDownListFor(m => m.StateOfBirth, Model.StateOfBirthList, new {  @class="sd",ID = "ddlStateOfBirth" })%>
                </td>

                <td colspan="1">
                    <label class="lblSpan">Citizenship</label><span class="nospan-align">*</span><br />
                    <%=Html.DropDownListFor(m => m.Citizenship, Model.CitizenshipList, new {  @class="sd",ID = "ddlCityzenShip" })%>
                </td>
            </tr>


            <tr>
                <td colspan="1">
                    <label class="lblSpan">Height</label><span class="nospan-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.Height, Model.Height, new {  @class="numbersOnly",onpaste="funPaste(event);",ID="txtHeight" })%><span>ft</span></td>
                <td colspan="1">
                    <label class="lblSpan">Weight</label><span class="nospan-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.Weight, Model.Weight, new {  @class="numbersOnly",onpaste="funPaste(event);", ID="txtWeight" })%><span>lbs</span></td>
            </tr>


            <tr>
                <td colspan="1">
                    <label class="lblSpan">Hair Color</label><span class="nospan-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.HairColor, Model.HairColor, new {  @class="validate[custom[englishOnlywithspaces]]",maxlength=50 })%>
                </td>
                <td colspan="1">
                    <label class="lblSpan">Eye Color</label><span class="nospan-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.EyeColor, Model.EyeColor, new {  @class="validate[custom[englishOnlywithspaces]]",maxlength=50 })%></td>
            </tr>


            <tr>
                <td colspan="1">
                    <label class="lblSpan">Primary Language</label><span class="nospan-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.PrimaryLanguage, Model.PrimaryLanguage, new {  @class="sd",maxlength=50 })%></td>
                <td colspan="1">
                    <label class="lblSpan">Legal Competency Status</label><span class="nospan-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.LegalCompetencyStatus, Model.LegalCompetencyStatus, new {  @class="sd",maxlength=50 })%></td>

            </tr>


            <tr>
                <td colspan="1">
                    <label class="lblSpan">Guardianship Status</label><span class="nospan-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.GuardianshipStatus, Model.GuardianshipStatus, new {  @class="sd",maxlength=50 })%></td>
                <td colspan="1">
                    <label class="lblSpan">Other State Agencies Involved With Student</label><span class="nospan-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.OtherStateAgenciesInvolvedWithStudent, Model.OtherStateAgenciesInvolvedWithStudent, new {  @class="sd",maxlength=250 })%></td>
            </tr>


            <tr>
                <td colspan="1">
                    <label class="lblSpan">Distinguishing Marks</label><span class="nospan-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.DistigushingMarks, Model.DistigushingMarks, new {  @class="sd" })%></td>
                <td colspan="1">
                    <label class="lblSpan">Marital Status of Both Parents</label><span class="nospan-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.MaritalStatusofBothParents, Model.MaritalStatusofBothParents, new {  @class="sd",maxlength=50 })%></td>
            </tr>


            <tr>
                <td colspan="1">
                    <label class="lblSpan">Case Manager Residential</label><span class="nospan-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.CaseManagerResidential, Model.CaseManagerResidential, new {  @class="sd",maxlength=50 })%></td>
                <td colspan="1">
                    <label class="lblSpan">Case Manager Educational</label><span class="nospan-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.CaseManagerEducational, Model.CaseManagerEducational, new {  @class="sd",maxlength=50 })%></td>
            </tr>
        </table>

        <div class="rightContainer_new" style="display: inline-block !important; float: right; margin-right: -27%; padding-top: 3px; width: 27%; min-width: 215px;">
            <div id="rightSidePanel">
                <table>
                    <tr>
                        <%if (Model.Id > 0)
                          { 
                        %>
                         <% string Color="";
                           if (Model.ClientStatus == "Active")
                          {
                              Color = "green";
                          }
                          else if (Model.ClientStatus == "On-Hold")
                          {
                              Color = "orange";
                          }
                           else if (Model.ClientStatus == "Inactive")
                           {
                               Color = "red";
                           }
                        %>
                        <td id="tdstudentName" class="nobdr" style="display: none;"><%=Model.LastName %> , <%=Model.FirstName %> <span style="color:<%=Color %>"><%= "("+Model.ClientStatus+")" %></span></td>
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
                        <td>
                            <%-- <%=Html.DisplayFor(m=>m.ImageUrl) %>--%>
                            <img id="stdImage" alt="No Image Uploaded" src="data:image/gif;base64,<%=Model.ImageUrl %>" />
                            <div id="newPreview"></div>
                            <div id="cancelPreview" onclick="cancelPreview()" style="display: none;">Cancel</div>
                        </td>
                    </tr>
                    <tr>
                        <%if (Model.ImageUrl != null)
                          {%>
                        <td>
                            <input type="file" id="profilePicture" name="profilePicture" style="width: 185px;" />
                            <%-- <%=Html.TextBoxFor(m => m.profilePicture, new { type = "file" })%>--%>
                        </td>
                        <% }
                          else
                          {  %>

                        <td>
                            <input type="file" id="File1" name="profilePicture" style="width: 150px; float: left;"
                                class=" " />
                        </td>
                        <%-- <%=Html.TextBoxFor(m => m.profilePicture, new { type = "file" })%>--%>
                        <%} %>
                    </tr>
                    <tr>
                        <td>
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
        </div>
        <div class="clear"></div>

        <table style="width: 100%">
            <tr>
                <td colspan="9">
                    <h4>Address Information</h4>
                </td>
            </tr>
        </table>
        <table>
            <tr>
                <td colspan="1">
                    <label class="lblSpan">Street Address</label><span class="span-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.AddressLine2, Model.AddressLine2, new {  @class="validate[required] ",maxlength=50 })%></td>
            </tr>


            <tr>
                <td colspan="1">
                    <label class="lblSpan">Unit #</label><span class="span-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.AddressLine1, Model.AddressLine1, new {  @class="validate[required] ",maxlength=50 })%></td>
            </tr>



            <tr>
                <td colspan="1">
                    <label class="lblSpan">City</label><span class="nospan-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.City, Model.City, new {  @class="sd" })%></td>
            </tr>


            <tr>
                <td colspan="1">
                    <label class="lblSpan">State</label><span class="nospan-align">*</span><br />
                    <%=Html.DropDownListFor(m => m.State, Model.StateList, new {  @class="sd",ID = "ddlState" })%></td>
                <td>
            </tr>

            <tr>
                <td colspan="1">
                    <label class="lblSpan">Zip</label><span class="nospan-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.ZipCode, Model.ZipCode, new {  @class="sd zipDefault",onpaste="funPaste(event);",maxlength=5,ID = ""})%></td>
                <td>
            </tr>
            <tr>
                <td colspan="1">
                    <label class="lblSpan">County</label><span class="nospan-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.studCounty, Model.studCounty, new {  @class="sd" })%></td>
            </tr>


            <tr>
                <td colspan="1">
                    <label class="lblSpan">Country</label><span class="nospan-align">*</span><br />
                    <%=Html.DropDownListFor(m => m.Country, Model.CountryList, new {  @class="sd",ID = "ddlCountry" })%></td>
            </tr>

            <%-- <tr>
                <td class="">Address Line 3 <span style="color: red">*</span></td>
                <td class="">
                    <%=Html.TextBoxFor(m => m.AddressLine3, Model.AddressLine3, new {  @class="validate[required] ",maxlength=50 })%></td>
                
            </tr>--%>
        </table>         
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
                    <%=Html.TextBoxFor(m => m.EmergencyContactFirstName1, Model.EmergencyContactFirstName1, new {  @class="namefield",onpaste="funPaste(event);",maxlength=50 })%></td>
                <td colspan="1">
                    <label class="lblSpan">Last Name</label><span class="nospan-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.EmergencyContactLastName1, Model.EmergencyContactLastName1, new {  @class="namefield",onpaste="funPaste(event);",maxlength=50 })%></td>
            </tr>

            <tr>
                <td colspan="1">
                    <label class="lblSpan">Title</label><span class="nospan-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.EmergencyContactTitle1, Model.EmergencyContactTitle1, new {  @class="sd",maxlength=50 })%>
                </td>
                <td colspan="1">
                    <label class="lblSpan">Phone</label><span class="nospan-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.EmergencyContactPhone1, Model.EmergencyContactPhone1, new {  @class="validate[custom[usPhoneNumber]] usPhone",onpaste="funPaste(event);" })%></td>
            </tr>

            <tr>
                <td colspan="1">
                    <label class="lblSpan">First Name</label><span class="nospan-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.EmergencyContactFirstName2, Model.EmergencyContactFirstName2, new {  @class="namefield",onpaste="funPaste(event);",maxlength=50 })%></td>

                <td colspan="1">

                    <label class="lblSpan">Last Name</label><span class="nospan-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.EmergencyContactLastName2, Model.EmergencyContactLastName2, new {  @class="namefield",onpaste="funPaste(event);",maxlength=50 })%></td>
            </tr>

            <tr>
                <td colspan="1">
                    <label class="lblSpan">Title</label><span class="nospan-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.EmergencyContactTitle2, Model.EmergencyContactTitle2, new {  @class="sd",maxlength=50 })%></td>

                <td colspan="1">
                    <label class="lblSpan">Phone</label><span class="nospan-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.EmergencyContactPhone2, Model.EmergencyContactPhone2, new {  @class="validate[custom[usPhoneNumber]] usPhone",onpaste="funPaste(event);" })%></td>
            </tr>

            <tr>
                <td colspan="1">
                    <label class="lblSpan">First Name</label><span class="nospan-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.EmergencyContactFirstName3, Model.EmergencyContactFirstName3, new {  @class="namefield",onpaste="funPaste(event);",maxlength=50 })%></td>
                <td colspan="1">
                    <label class="lblSpan">Last Name</label><span class="nospan-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.EmergencyContactLastName3, Model.EmergencyContactLastName3, new {  @class="namefield",onpaste="funPaste(event);",maxlength=50 })%></td>
            </tr>

            <tr>
                <td colspan="1">
                    <label class="lblSpan">Title</label><span class="nospan-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.EmergencyContactTitle3, Model.EmergencyContactTitle3, new {  @class="sd",maxlength=50 })%></td>
                <td colspan="1">
                    <label class="lblSpan">Phone</label><span class="nospan-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.EmergencyContactPhone3, Model.EmergencyContactPhone3, new {  @class="validate[custom[usPhoneNumber]] usPhone",onpaste="funPaste(event);" })%></td>
            </tr>


            <tr>
                <td colspan="1">
                    <label class="lblSpan">First Name</label><span class="nospan-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.EmergencyContactFirstName4, Model.EmergencyContactFirstName4, new {  @class="namefield",onpaste="funPaste(event);",maxlength=50 })%></td>
                <td colspan="1">
                    <label class="lblSpan">Last Name</label><span class="nospan-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.EmergencyContactLastName4, Model.EmergencyContactLastName4, new {  @class="namefield",onpaste="funPaste(event);" ,maxlength=50})%></td>
            </tr>

            <tr>
                <td colspan="1">
                    <label class="lblSpan">Title</label><span class="nospan-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.EmergencyContactTitle4, Model.EmergencyContactTitle4, new {  @class="sd",maxlength=50 })%></td>
                <td colspan="1">
                    <label class="lblSpan">Phone</label><span class="nospan-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.EmergencyContactPhone4, Model.EmergencyContactPhone4, new {  @class="validate[custom[usPhoneNumber]] usPhone",onpaste="funPaste(event);" })%></td>
            </tr>

            <tr>
                <td colspan="1">
                    <label class="lblSpan">First Name</label><span class="nospan-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.EmergencyContactFirstName5, Model.EmergencyContactFirstName5, new {  @class="namefield",onpaste="funPaste(event);",maxlength=50 })%></td>
                <td colspan="1">
                    <label class="lblSpan">Last Name</label><span class="nospan-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.EmergencyContactLastName5, Model.EmergencyContactLastName5, new {  @class="namefield",onpaste="funPaste(event);",maxlength=50 })%></td>
            </tr>

            <tr>
                <td colspan="1">
                    <label class="lblSpan">Title</label><span class="nospan-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.EmergencyContactTitle5, Model.EmergencyContactTitle5, new {  @class="sd",maxlength=50 })%></td>
                <td colspan="1">
                    <label class="lblSpan">Phone</label><span class="nospan-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.EmergencyContactPhone5, Model.EmergencyContactPhone5, new {  @class="validate[custom[usPhoneNumber]] usPhone",onpaste="funPaste(event);" })%></td>
            </tr>
        </table>
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
                    <label class="lblSpan">Insurance Type</label><span class="nospan-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.InsuranceType, Model.InsuranceType, new {  @class="sd",maxlength=50 })%></td>
                <td colspan="1">
                    <label class="lblSpan">Policy Number</label><span class="nospan-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.PolicyNumber, Model.PolicyNumber, new {  @class="sd",maxlength=50 })%></td>
            </tr>

            <tr>
                <td colspan="1">
                    <label class="lblSpan">Policy Holder</label><span class="nospan-align">*</span><br />
                    <%=Html.TextBoxFor(m => m.PolicyHolder, Model.PolicyHolder, new {  @class="sd",maxlength=50 })%></td>

            </tr>
        </table>
        <table style="width: 100%;">
            <tr>
                <td colspan="9">
                    <h4>IEP / Referral Information</h4>
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

            <tr>
                <td colspan="1">
                    <% if (ViewBag.permission == "true")
                       {
                           if (Model.Id > 0)
                           { %>

                    <input id="btnUpdate" type="submit" value="Update" style="margin-left: 5%" />
                    <%}
                           else
                           {%>
                    <input id="btnSubmit" type="submit" value="Save" style="margin-left: 5%" /><% }
                       }%>
                </td>
            </tr>
        </table>
    </div>
</div>



<%} %>


<script>
    loadClientStaticDetails();
</script>