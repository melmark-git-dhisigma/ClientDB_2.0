<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="ClientReports.aspx.cs" Inherits="ClientDB.Reports.ClientReports" %>


<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register TagPrefix="asp" Namespace="Saplin.Controls" Assembly="DropDownCheckBoxes" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta name="viewport" content="width=device-width" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <%--    <link href="../Documents/CSS/General.css" rel="stylesheet" />--%>
    <script src="../Documents/JS/jquery-1.8.0.min.js"></script>
    <script src="../Documents/JS/jquery.form.js"></script>
    <%-- <script src="../Documents/JS/jquery-ui-1.10.3.custom.js"></script>--%>
    <link href="../Documents/CSS/jquery-ui-1.10.3.custom.css" rel="stylesheet" />
    <script src="../Documents/JS/jquery.validationEngine-en.js"></script>
    <script src="../Documents/JS/jquery.validationEngine.js"></script>
    <script src="../Documents/JS/jquery.unobtrusive-ajax.js"></script>
    <link href="../Documents/CSS/jquery-ui.css" rel="stylesheet" />
    <script src="../Documents/JS/jquery-ui-1.11.2.js"></script>
    <link href="../Documents/CSS/validationEngine.jquery.css" rel="stylesheet" />
    <link href="../Documents/CSS/ReportStyle.css" rel="stylesheet" />
    <script src="../../Documents/JS/jquery.timeentry.js" type="text/javascript"></script>
    <script src="~/Documents/JS/jquery-ui-1.8.24.js"></script>
    <style type="text/css">
        .ui-datepicker select.ui-datepicker-month, .ui-datepicker select.ui-datepicker-year {
            width: 50% !important;
        }

        .ui-datepicker select.ui-datepicker-month, .ui-datepicker select.ui-datepicker-month {
            width: 50% !important;
        }
    </style>
    <script type="text/javascript">

        $(document).ready(function () {


            $("#ddlDeptLocDept").prop('disabled', true);
            $("#ddlDeptLocLoc").prop('disabled', true);
            $("#ddlDeptPlctypeDept").prop('disabled', true);
            $("#ddlDeptPlctypePlcType").prop('disabled', true);
            $("#ddlLocLoc").prop('disabled', true);
            $("#txtActiveStartDate").prop('disabled', true);
            $("#txtActiveEndDate").prop('disabled', true);
            $("#txtNewStartDate").prop('disabled', true);
            $("#txtNewEndDate").prop('disabled', true);
            $("#txtDischrStartDate").prop('disabled', true);
            $("#txtDischrEndDate").prop('disabled', true);

            var date = new Date();
            date.setDate(date.getDate());
            $('.datepicker').datepicker(
             {
                 dateFormat: "mm/dd/yy",
                 changeMonth: true,
                 changeYear: true,
                 showAnim: "fadeIn",
                 yearRange: 'c-100:c+100',
                 //minDate: date,
                 /* fix buggy IE focus functionality */
                 fixFocusIE: false,
                 constrainInput: false
             });





            $.get("../ClientRegistration/GetTitleReport", function (data) {
                document.title = data;

            });



        });

        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;
            return true;
        }

        function GetValidate() {
            if ($('#hdnballet').val() == "Choose Department and Location") {
                if ($('#ddlDeptLocDept').prop('selectedIndex') == 0) {
                    alert("Please select Department");
                    return false;
                }
                if ($('#ddlDeptLocLoc').prop('selectedIndex') == 0) {
                    alert("Please select Location");
                    return false;
                }

            }
            else if ($('#hdnballet').val() == "Choose Department and Placement Type") {
                if ($('#ddlDeptPlctypeDept').prop('selectedIndex') == 0) {
                    alert("Please select Department");
                    return false;
                }
                if ($('#ddlDeptPlctypePlcType').prop('selectedIndex') == 0) {
                    alert("Please select Placement Type");
                    return false;
                }
            }
            else if ($('#hdnballet').val() == "Choose Location") {
                if ($('#ddlLocLoc').prop('selectedIndex') == 0) {
                    alert("Please select Location");
                    return false;
                }
            }
            if ($('#hdnDateRange').val() == "Active Placement") {
                if ($('#txtActiveStartDate').val() == "") {
                    alert("Please select Startdate");
                    return false;
                }
                if ($('#txtActiveEndDate').val() == "") {
                    alert("Please select Enddate");
                    return false;
                }

            }
            else if ($('#hdnDateRange').val() == "Discharged Placement") {
                if ($('#txtDischrStartDate').val() == "") {
                    alert("Please select Startdate");
                    return false;
                }
                if ($('#txtDischrEndDate').val() == "") {
                    alert("Please select Enddate");
                    return false;
                }
            }
            else if ($('#hdnDateRange').val() == "New Placement") {
                if ($('#txtNewStartDate').val() == "") {
                    alert("Please select Startdate");
                    return false;
                }
                if ($('#txtNewEndDate').val() == "") {
                    alert("Please select Enddate");
                    return false;
                }
            }
            return true;
        }


        function ChangeSelectedMenu(MenuClass) {
            if ($(".leftMenu")[0]) {
                $('.leftMenu').css('background-position', 'none');
            }
            $('#' + MenuClass).css('background-position', '0 0');
        }


        $('.leftMenu').click(function () {

            var elmId = $(this).attr('id');
            $('.leftMenu').removeClass('current');
            $(this).addClass('current');
            if (elmId == "btnallClient") {
                //$('#content').load('../ClientRegistration/ClientRegistration|Fill');
                $('.imgcontainer').css("display", "block");
                $('.imgcontainer').load('../Contact/ImageUploadPanel?edit=1');
                $('.EditProfile').css("display", "block");
                $('#calender').css("display", "none");


            }
            if (elmId == "btnClienContact") {
                // $('#content').load('../Medical/Medical/');
                //       $('#content').load('/Medical/FillMedicalData/0');
                $('.imgcontainer').css("display", "block");
                $('.imgcontainer').load('../Contact/ImageUploadPanel?edit=0');

                $('#calender').css("display", "block");

            }
            if (elmId == "btnPgmRoster") {
                //$('#content').load('../Placement/Placement/');
                $('#calender').css("display", "none");
                $('.imgcontainer').css("display", "block");
                $('.imgcontainer').load('../Contact/ImageUploadPanel?edit=0');
            }
            if (elmId == "btnVendor") {
                //$('#content').load('../Contact/ListContactVendor/');
                $('#calender').css("display", "none");
                $('.imgcontainer').css("display", "block");
                $('.imgcontainer').load('../Contact/ImageUploadPanel?edit=0');
                // $('#content').load('/Contact/fillContactDetails/');
            }
            if (elmId == "btnBirthdate") {
                //$('#content').load('../Visitation/Visitation/');
                $('#calender').css("display", "none");
                $('.imgcontainer').css("display", "block");
                $('.imgcontainer').load('../Contact/ImageUploadPanel?edit=0');
            }
            if (elmId == "btnResRoster") {
                //$('#content').load('../Event/EventsList/');
                $('#calender').css("display", "none");
                $('.imgcontainer').css("display", "block");
                $('.imgcontainer').load('../Contact/ImageUploadPanel?edit=0');
            }

            if (elmId == "btnAllPlacement") {
                //$('#content').load('../Event/EventsList/');
                $('#calender').css("display", "none");
                $('.imgcontainer').css("display", "block");
                $('.imgcontainer').load('../Contact/ImageUploadPanel?edit=0');
            }

            if (elmId == "btnStatistical") {
                //$('#content').load('../Event/EventsList/');
                $('#calender').css("display", "none");
                $('.imgcontainer').css("display", "block");
                $('.imgcontainer').load('../Contact/ImageUploadPanel?edit=0');
            }

            if (elmId == "btnAllDischargedate") {
                //$('#content').load('../Event/EventsList/');
                $('#calender').css("display", "none");
                $('.imgcontainer').css("display", "block");
                $('.imgcontainer').load('../Contact/ImageUploadPanel?edit=0');
            }

            if (elmId == "btnAllAdmissionDate") {
                //$('#content').load('../Event/EventsList/');
                $('#calender').css("display", "none");
                $('.imgcontainer').css("display", "block");
                $('.imgcontainer').load('../Contact/ImageUploadPanel?edit=0');
            }

            if (elmId == "btnAllBirthdate") {
                //$('#content').load('../Event/EventsList/');
                $('#calender').css("display", "none");
                $('.imgcontainer').css("display", "block");
                $('.imgcontainer').load('../Contact/ImageUploadPanel?edit=0');
            }

            if (elmId == "btnContactChanges") {
                //$('#content').load('../Event/EventsList/');
                $('#calender').css("display", "none");
                $('.imgcontainer').css("display", "block");
                $('.imgcontainer').load('../Contact/ImageUploadPanel?edit=0');
            }

            if (elmId == "btnGuardianChanges") {
                //$('#content').load('../Event/EventsList/');
                $('#calender').css("display", "none");
                $('.imgcontainer').css("display", "block");
                $('.imgcontainer').load('../Contact/ImageUploadPanel?edit=0');
            }

            if (elmId == "btnPlcChange") {
                //$('#content').load('../Event/EventsList/');
                $('#calender').css("display", "none");
                $('.imgcontainer').css("display", "block");
                $('.imgcontainer').load('../Contact/ImageUploadPanel?edit=0');
            }

            if (elmId == "btnFundChange") {
                //$('#content').load('../Event/EventsList/');
                $('#calender').css("display", "none");
                $('.imgcontainer').css("display", "block");
                $('.imgcontainer').load('../Contact/ImageUploadPanel?edit=0');
            }
        });

        function ValidateChanges() {
            if ($('#txtchangeSdate').val() == "") {
                alert("Please select Startdate");
                return false;
            }
            else if ($('#txtchangeEdate').val() == "") {
                alert("Please select Enddate");
                return false;
            }            
                return true;
        }

        function resetVal() {
            $('#txtchangeSdate').val("") ;
            $('#txtchangeEdate').val("") ;
        }

    </script>

    <style type="text/css">
        .leftMenu:active {
            background-position: 0 0;
        }
    </style>


</head>







<body>
    <form id="FormClientReport" runat="server">
        <div>
            <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
            <asp:HiddenField ID="hdnType" runat="server" />
        </div>
        <div class="mainContainer">

            <div class="topHead">
                <a class="admin" href="#">

                     <% sess = (clsSession)Session["UserSessionClient"]; %>
                    <%=sess.UserName %>



                </a>
                <a class="logout" href="../../../Login.aspx">Logout</a>
                <a class="Report" href="../Reports/ClientReports.aspx">Reports</a>
                <a class="home" href="../Client/Index">Home</a>
            </div>
            <div class="contentPart">
                <div class="imgcorner">
                    <a class="logo" href="#">
                        <img src="../Documents/images/logo.jpg" width="200" height="40" /></a>
                </div>
                <div class="ContentAreaContainer">
                    <div class="leftContainer2" style="width: 23%">

                        <asp:Button ID="btnallClient" CssClass="leftMenu" runat="server" Text="All Clients Info" ToolTip="All Clients Info" OnClick="btnallClient_Click"></asp:Button>

                        <asp:Button ID="btnClienContact" CssClass="leftMenu" runat="server" Text="Emergency/Home Contact" ToolTip="Emergency/Home Contact" OnClick="btnClienContact_Click"></asp:Button>

                        <%--                                    <asp:Button ID="btnClientContactRes" CssClass="leftMenu" runat="server" Text="Emergency/Home Contact – Residence Only" ToolTip="Emergency/Home Contact – Residence Only"   ></asp:Button>--%>

                        <asp:Button ID="btnPgmRoster" CssClass="leftMenu" runat="server" Text="Program Roster" ToolTip="Program Roster" OnClick="btnPgmRoster_Click"></asp:Button>

                        <asp:Button ID="btnVendor" runat="server" CssClass="leftMenu" Text="Client/Contact/Vendor" ToolTip="Client/Contact/Vendor" OnClick="btnVendor_Click"></asp:Button>

                        <%--<asp:Button ID="btnVenderDischarged" runat="server" CssClass="leftMenu" Text="Client/Contact/Vendor – Discharged" ToolTip="Client/Contact/Vendor – Discharged"   ></asp:Button>--%>

                        <asp:Button ID="btnBirthdate" runat="server" CssClass="leftMenu" Text="All Clients by Birthdate Quarter" ToolTip="All Clients by Birthdate Quarter" OnClick="btnBirthdate_Click"></asp:Button>

                        <asp:Button ID="btnResRoster" runat="server" CssClass="leftMenu" Text=" Residential Roster Report" ToolTip=" Residential Roster Reports" OnClick="btnResRoster_Click"></asp:Button>
                        <asp:Button ID="btnAllFunder" runat="server" CssClass="leftMenu" Text="All Clients by Funder" ToolTip="All Clients by Funder" OnClick="btnAllFunder_Click"></asp:Button>
                        <asp:Button ID="btnAllPlacement" runat="server" CssClass="leftMenu" Text="All Clients by Placement" ToolTip="All Clients by placement" OnClick="btnAllPlacement_Click"></asp:Button>
                        <asp:Button ID="btnAllBirthdate" runat="server" CssClass="leftMenu" Text="All Clients by Birthdate" ToolTip="All Clients by Birthdate" OnClick="btnAllBirthdate_Click"></asp:Button>
                        <asp:Button ID="btnAllAdmissionDate" runat="server" CssClass="leftMenu" Text="All Clients by Admission date" ToolTip="All Clients by Admission date" OnClick="btnAllAdmissionDate_Click"></asp:Button>
                        <asp:Button ID="btnAllDischargedate" runat="server" CssClass="leftMenu" Text="All Clients by Discharge date" ToolTip="All Clients by Discharge date" OnClick="btnAllDischargedate_Click"></asp:Button>
                        <asp:Button ID="btnStatistical" runat="server" CssClass="leftMenu" Text="Statistical Report" ToolTip="Statistical Report" OnClick="btnStatistical_Click"></asp:Button>
                        <asp:Button ID="btnFundChange" runat="server" CssClass="leftMenu" Text="Funding Changes" ToolTip="Funding Changes" OnClick="btnFundChange_Click" OnClientClick="resetVal();"></asp:Button>
                        <asp:Button ID="btnPlcChange" runat="server" CssClass="leftMenu" Text="Placement Changes" ToolTip="Placement Changes" OnClick="btnPlcChange_Click" OnClientClick="resetVal();"></asp:Button>
                        <asp:Button ID="btnGuardianChanges" runat="server" CssClass="leftMenu" Text="Guardianship Changes" ToolTip="Guardianship Changes" OnClick="btnGuardianChanges_Click" OnClientClick="resetVal();"></asp:Button>
                        <asp:Button ID="btnContactChanges" runat="server" CssClass="leftMenu" Text="Contact Changes" ToolTip="Contact Changes" OnClick="btnContactChanges_Click" OnClientClick="resetVal();"></asp:Button>
                    </div>


                    <div class="middleContainer" style="width: 75%">

                        <div id="content">
                            <div class="headingDivBar" style="width: 100%" id="HeadingDiv" runat="server" visible="false">
                            </div>
                            <div style="float: left; width: 100%" id="tdMsg" runat="server" visible="false">
                            </div>

                                 <%--Code design for contact report--%>                       

                                <div runat="server" id="divContact" visible="false">
                                        <asp:UpdatePanel ID="UpdatePanelContact" runat="server">
                                        <ContentTemplate>
                                <table>                                
                                    <tr>
                                    
                                         <td>
                                             <asp:Label ID="Labelclient" runat="server" ForeColor="Black" Text="Client:"></asp:Label>
                                         </td>
                                         <td>
                                            <asp:DropDownCheckBoxes ID="DropDownCheckBoxesConStudname" runat="server" Width="180px" UseSelectAllNode="false" AddJQueryReference="False" UseButtons="true" style="color: #00549f; height: 13px;" AutoPostBack="False" OnSelectedIndexChanged="DropDownCheckBoxesConStudname_SelectedIndexChanged">
                                            <Style SelectBoxWidth="195" DropDownBoxBoxWidth="160" DropDownBoxBoxHeight="190" DropDownBoxCssClass="ddchkLesson"/><Texts SelectBoxCaption="All"/>
                                            </asp:DropDownCheckBoxes>                                        

                                         </td>

                                         <td>
                                             <asp:Label ID="Labelrelation" runat="server" ForeColor="Black" Text="Relationship:"></asp:Label>
                                         </td>
                                         <td>    
                                            <asp:DropDownCheckBoxes ID="DropDownCheckBoxesRelation" runat="server" Width="180px" UseSelectAllNode="false" AddJQueryReference="False" UseButtons="true" style="color: #00549f; height: 13px;" AutoPostBack="False" OnSelectedIndexChanged="DropDownCheckBoxesRelation_SelectedIndexChanged">
                                            <Style SelectBoxWidth="195" DropDownBoxBoxWidth="160" DropDownBoxBoxHeight="190" DropDownBoxCssClass="ddchkLesson"/><Texts SelectBoxCaption="All"/>
                                            </asp:DropDownCheckBoxes>                                                  
                                         </td>

                                         <td>
                                            <asp:CheckBoxList ID="CheckBoxListcontact" runat="server" RepeatDirection="Horizontal">
                                            <asp:ListItem Selected="True">Active</asp:ListItem>
                                            <asp:ListItem>Inactive</asp:ListItem>
                                            <asp:ListItem>Discharged</asp:ListItem>
                                            </asp:CheckBoxList>
                                         </td>
                                        
                                        <td><asp:Button ID="Btncontact" runat="server" Text="Show Report" OnClick="btnShowVendor_Click" Width="120px" BackColor="#03507D" ForeColor="#FFFFFF" Font-Bold="True" />
                                        </td>
                                        <td><asp:Button ID="BtncontactReset" runat="server" Text="Reset" OnClick="btnVendor_Click" Width="120px" BackColor="#03507D" ForeColor="#FFFFFF" Font-Bold="True" />
                                        </td>
                                    </tr>

                                    <tr>
                                    <td><asp:HiddenField runat="server" ID="HContactRelation"/></td>
                                    <td><asp:HiddenField runat="server" ID="HContactstatus"/></td>
                                    <td><asp:HiddenField runat="server" ID="HContactStudname"/></td>
                                    </tr>

                                    </table>
                                         </ContentTemplate>
                                         </asp:UpdatePanel>                     
                                 </div>
                            <%--Code design end for contact report--%>

                            <div>

                                <div id="divbirthdate" runat="server" visible="false">
                                    <table style="width: 100%">
                                        <tr>
                                            <td style="width: 15%">
                                                <asp:Label ID="Label1" runat="server" Text="Birthdate Quarter"></asp:Label>
                                            </td>
                                            <td style="width: 25%">
                                                <asp:DropDownList ID="ddlQuarter" runat="server">
                                                    <asp:ListItem Value="0">---------------Select--------------</asp:ListItem>
                                                    <asp:ListItem Value="1">January - March</asp:ListItem>
                                                    <asp:ListItem Value="2">April - June</asp:ListItem>
                                                    <asp:ListItem Value="3">July - September</asp:ListItem>
                                                    <asp:ListItem Value="4">October - December</asp:ListItem>
                                                </asp:DropDownList>


                                            </td>
                                            <td>
                                                <asp:Button ID="btnquarter" runat="server" Text="Show Report" OnClick="btnquarter_Click" Width="120px" BackColor="#03507D" ForeColor="#FFFFFF" Font-Bold="True" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div id="divPlacement" runat="server" visible="false">
                                    <table style="width: 100%">
                                        <tr>
                                            <td>
                                                <fieldset>
                                                    <legend>
                                                        <input type="checkbox" class="chb" value="Choose Department and Location" id="rbtnDeptLoc" />
                                                        Choose Department and Location
       
                                                    </legend>
                                                    <table style="width: 100%">
                                                        <tr>
                                                            <td>Department</td>
                                                            <td>

                                                                <asp:DropDownList ID="ddlDeptLocDept" runat="server" Width="120px">
                                                                </asp:DropDownList>

                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>Location</td>
                                                            <td>

                                                                <asp:DropDownList ID="ddlDeptLocLoc" runat="server" Width="120px">
                                                                </asp:DropDownList>

                                                            </td>
                                                        </tr>
                                                    </table>
                                                </fieldset>
                                            </td>
                                            <td>
                                                <fieldset>
                                                    <legend>
                                                        <input type="checkbox" class="chb" value="Choose Department and Placement Type" id="rbtnDeptPlaceType" />
                                                        Choose Department and Placement Type
     
                                                    </legend>
                                                    <table style="width: 100%">
                                                        <tr>
                                                            <td>Department</td>
                                                            <td>

                                                                <asp:DropDownList ID="ddlDeptPlctypeDept" runat="server" Width="120px">
                                                                </asp:DropDownList>

                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>Placement Type</td>
                                                            <td>

                                                                <asp:DropDownList ID="ddlDeptPlctypePlcType" runat="server" Width="120px">
                                                                </asp:DropDownList>

                                                            </td>
                                                        </tr>
                                                    </table>
                                                </fieldset>
                                            </td>
                                            <td>
                                                <fieldset>
                                                    <legend>
                                                        <input type="checkbox" class="chb" value="Choose Location" id="rbtnLocation" />
                                                        Choose Location
                                                    </legend>
                                                    <table style="width: 100%">
                                                        <tr>
                                                            <td>Location</td>
                                                            <td>

                                                                <asp:DropDownList ID="ddlLocLoc" runat="server" Width="120px">
                                                                </asp:DropDownList>

                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td></td>
                                                            <td></td>
                                                        </tr>
                                                    </table>
                                                </fieldset>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <fieldset>
                                                    <legend>
                                                        <input type="checkbox" class="DateRange" value="Active Placement" id="rbtnActivePlc" />
                                                        Active Placement</legend>
                                                    <table style="width: 100%">
                                                        <tr>
                                                            <td>Start Date</td>
                                                            <td>

                                                                <asp:TextBox ID="txtActiveStartDate" runat="server" Width="120px" CssClass="datepicker" onkeypress="return false"></asp:TextBox>

                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>End Date</td>
                                                            <td>

                                                                <asp:TextBox ID="txtActiveEndDate" runat="server" Width="120px" CssClass="datepicker" onkeypress="return false"></asp:TextBox>

                                                            </td>
                                                        </tr>
                                                    </table>
                                                </fieldset>
                                            </td>
                                            <td>
                                                <fieldset>
                                                    <legend>
                                                        <input type="checkbox" class="DateRange" value="Discharged Placement" id="rbtnDischargedPlc" />
                                                        Discharged Placement
                                                    </legend>
                                                    <table style="width: 100%">
                                                        <tr>
                                                            <td>Start Date</td>
                                                            <td>

                                                                <asp:TextBox ID="txtDischrStartDate" runat="server" Width="120px" CssClass="datepicker" onkeypress="return false"></asp:TextBox>

                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>End Date</td>
                                                            <td>

                                                                <asp:TextBox ID="txtDischrEndDate" runat="server" Width="120px" CssClass="datepicker" onkeypress="return false"></asp:TextBox>

                                                            </td>
                                                        </tr>
                                                    </table>
                                                </fieldset>
                                            </td>
                                            <td>
                                                <fieldset>
                                                    <legend>
                                                        <input type="checkbox" class="DateRange" value="New Placement" id="rbtnNewPlacement" />
                                                        New Placement
                                                    </legend>
                                                    <table style="width: 100%">
                                                        <tr>
                                                            <td>Start Date</td>
                                                            <td>

                                                                <asp:TextBox ID="txtNewStartDate" runat="server" Width="120px" CssClass="datepicker" onkeypress="return false"></asp:TextBox>

                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>End Date</td>
                                                            <td>

                                                                <asp:TextBox ID="txtNewEndDate" runat="server" Width="120px" CssClass="datepicker" onkeypress="return false"></asp:TextBox>

                                                            </td>
                                                        </tr>
                                                    </table>
                                                </fieldset>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td></td>
                                            <td></td>
                                            <td>

                                                <asp:Button ID="btnShowPlacement" runat="server" Text="Show Report" BackColor="#03507D" ForeColor="#FFFFFF" Font-Bold="True" OnClick="btnShowPlacement_Click" OnClientClick="return GetValidate();" />

                                            </td>
                                        </tr>
                                    </table>
                                </div>

                                <div id="divFunder" runat="server" visible="false">
                                    <table style="width: 100%">
                                        <tr>
                                            <td style="width: 15%">Funding Source</td>
                                            <td style="width: 20%">
                                                <asp:DropDownList ID="ddlFundingSource" runat="server" Width="220px">
                                                </asp:DropDownList>
                                            </td>
                                            <td style="width: 65%">
                                                <asp:Button ID="btnShowFunder" runat="server" Text="Show Report" BackColor="#03507D" ForeColor="#FFFFFF" Font-Bold="True" OnClick="btnShowFunder_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div runat="server" visible="false" id="divbyBirthdate">
                                    <table style="width: 100%">
                                        <tr>
                                            <td>Month</td>
                                            <td style="width: 10%">
                                                <asp:DropDownList ID="ddlMonth" runat="server" Width="130px">
                                                    <asp:ListItem Value="0">------Select------</asp:ListItem>
                                                    <asp:ListItem>January</asp:ListItem>
                                                    <asp:ListItem>February</asp:ListItem>
                                                    <asp:ListItem>March</asp:ListItem>
                                                    <asp:ListItem>April</asp:ListItem>
                                                    <asp:ListItem>May</asp:ListItem>
                                                    <asp:ListItem>June</asp:ListItem>
                                                    <asp:ListItem>July</asp:ListItem>
                                                    <asp:ListItem>August</asp:ListItem>
                                                    <asp:ListItem>September</asp:ListItem>
                                                    <asp:ListItem>October</asp:ListItem>
                                                    <asp:ListItem>November</asp:ListItem>
                                                    <asp:ListItem>December</asp:ListItem>
                                                </asp:DropDownList></td>
                                            <td></td>
                                            <td style="width: 8%">Age From</td>
                                            <td style="width: 10%">
                                                <asp:TextBox ID="txtAgeFrom" runat="server" onkeypress="return isNumberKey(event)"></asp:TextBox></td>
                                            <td></td>
                                            <td style="width: 4%">To</td>
                                            <td>
                                                <asp:TextBox ID="txtAgeTo" runat="server" onkeypress="return isNumberKey(event)"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td>Start Date</td>
                                            <td>
                                                <asp:TextBox ID="txtBithdateStart" runat="server" CssClass="datepicker" onkeypress="return false"></asp:TextBox></td>
                                            <td></td>
                                            <td>End Date</td>
                                            <td>
                                                <asp:TextBox ID="txtBirthdateEnd" runat="server" CssClass="datepicker" onkeypress="return false"></asp:TextBox></td>
                                            <td></td>
                                            <td></td>
                                            <td>
                                                <asp:Button ID="btnShowBirthdate" runat="server" Text="Show Report" BackColor="#03507D" ForeColor="#FFFFFF" Font-Bold="True" OnClick="btnShowBirthdate_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>

                                <div runat="server" visible="false" id="divAdmission">
                                    <table style="width: 100%">
                                        <tr>
                                            <td>Admission Date From</td>
                                            <td>
                                                <asp:TextBox ID="txtAdmissionFrom" runat="server" CssClass="datepicker" onkeypress="return false"></asp:TextBox></td>
                                            <td></td>
                                            <td>To</td>
                                            <td>
                                                <asp:TextBox ID="txtAdmissionTo" runat="server" CssClass="datepicker" onkeypress="return false"></asp:TextBox></td>
                                            <td></td>
                                            <td>Number of Admissions</td>
                                            <td>
                                                <asp:TextBox ID="txtNumberOfAdmission" runat="server" onkeypress="return isNumberKey(event)"></asp:TextBox>
                                            </td>
                                            <td></td>
                                            <td>
                                                <asp:Button ID="btnShowAdmissionDate" runat="server" Text="Show Report" BackColor="#03507D" ForeColor="#FFFFFF" Font-Bold="True" OnClick="btnShowAdmissionDate_Click" /></td>
                                        </tr>
                                    </table>
                                </div>
                                <div runat="server" visible="false" id="divDischarge">
                                    <table style="width: 100%">
                                        <tr>
                                            <td style="width: 20%">
                                                <%--<asp:RadioButtonList ID="rbtnDischargeStatus" runat="server" RepeatDirection="Horizontal">
                                                    <asp:ListItem Selected="True" Value="A">Active</asp:ListItem>
                                                    <asp:ListItem Value="I">Inactive</asp:ListItem>
                                                </asp:RadioButtonList></td>--%>
                                            <td>&nbsp;</td>
                                            <td>
                                                <asp:Button ID="btnShowDischarge" runat="server" Text="Show Report" BackColor="#03507D" ForeColor="#FFFFFF" Font-Bold="True" OnClick="btnShowDischarge_Click" /></td>
                                        </tr>
                                    </table>
                                </div>
                                <div runat="server" id="divStatistical" visible="false">
                                    <table style="width: 100%">
                                        <tr>
                                            <td>
                                                <asp:CheckBoxList ID="ChkStatisticalList" runat="server" RepeatDirection="Horizontal">
                                                    <asp:ListItem Selected="True">Total number of client</asp:ListItem>
                                                    <asp:ListItem Selected="True">Gender</asp:ListItem>
                                                    <asp:ListItem Selected="True">Department</asp:ListItem>
                                                    <asp:ListItem Selected="True">Placement Type</asp:ListItem>
                                                    <asp:ListItem Selected="True">Program</asp:ListItem>
                                                    <asp:ListItem Selected="True">Location</asp:ListItem>
                                                    <asp:ListItem Selected="True">Race</asp:ListItem>
                                                    <asp:ListItem Selected="True">Maximum client occupancy</asp:ListItem>
                                                </asp:CheckBoxList>
                                            </td>

                                        </tr>
                                        <tr>
                                            <td style="text-align: right">

                                                <asp:Button ID="btnShowStatistical" runat="server" Text="Show Report" BackColor="#03507D" ForeColor="#FFFFFF" Font-Bold="True" OnClick="btnShowStatistical_Click" />

                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div runat="server" id="divStatisticalNew" visible="false">
                                    <div id="tesss">
                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <span style="text-align:right;font-weight:bolder;font-size:12px;color: #00549f;">Student Name:</span><br />
                                                        <asp:DropDownCheckBoxes ID="DropDownCheckBoxesStudname" runat="server" Width="180px" UseSelectAllNode="false" AddJQueryReference="False" UseButtons="true" style="color: #00549f; height: 13px;" AutoPostBack="False" OnSelectedIndexChanged="DropDownCheckBoxesStudname_SelectedIndexChanged">
                                                        <Style SelectBoxWidth="195" DropDownBoxBoxWidth="160" DropDownBoxBoxHeight="190" DropDownBoxCssClass="ddchkLesson"/>
                                                        </asp:DropDownCheckBoxes>
                                                    </td>
                                                    <td>
                                                        <span style="text-align:right;font-weight:bolder;font-size:12px;color: #00549f;">Location:</span><br /> 
                                                        <asp:DropDownCheckBoxes ID="DropDownCheckBoxesLocation" runat="server" Width="180px" UseSelectAllNode="false" AddJQueryReference="False" UseButtons="true" style="color: #00549f; height: 13px;" AutoPostBack="False" OnSelectedIndexChanged="DropDownCheckBoxesLocation_SelectedIndexChanged">
                                                        <Style SelectBoxWidth="195" DropDownBoxBoxWidth="160" DropDownBoxBoxHeight="190" DropDownBoxCssClass="ddchkLesson"/>
                                                        </asp:DropDownCheckBoxes>
                                                    </td>
                                                    <td>
                                                        <span style="text-align:right;font-weight:bolder;font-size:12px;color: #00549f;">Races:</span><br /> 
                                                        <asp:DropDownCheckBoxes ID="DropDownCheckBoxesRaces" runat="server" Width="180px" UseSelectAllNode="false" AddJQueryReference="False" UseButtons="true" style="color: #00549f; height: 13px;" AutoPostBack="False" OnSelectedIndexChanged="DropDownCheckBoxesRaces_SelectedIndexChanged">
                                                        <Style SelectBoxWidth="275" DropDownBoxBoxWidth="240" DropDownBoxBoxHeight="90" DropDownBoxCssClass="ddchkLesson"/>
                                                        </asp:DropDownCheckBoxes>
                                                    </td>
                                                    <td>
                                                        <span style="text-align:right;font-weight:bolder;font-size:12px;color: #00549f;">Status:</span><br /> 
                                                        <asp:DropDownCheckBoxes ID="DropDownCheckBoxesActive" runat="server" Width="180px" UseSelectAllNode="false" AddJQueryReference="False" UseButtons="true" style="color: #00549f; height: 13px;" AutoPostBack="False" OnSelectedIndexChanged="DropDownCheckBoxesActive_SelectedIndexChanged">
                                                        <Style SelectBoxWidth="195" DropDownBoxBoxWidth="160" DropDownBoxBoxHeight="50" DropDownBoxCssClass="ddchkLesson"/>
                                                        <%--<Items>
                                                        <asp:ListItem Text="Active" Value="A"></asp:ListItem>
                                                        <asp:ListItem Text="Discharged" Value="D"></asp:ListItem> 
                                                        </Items>--%>
                                                        </asp:DropDownCheckBoxes>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td><asp:HiddenField runat="server" ID="hfstudname"/></td>
                                                    <td><asp:HiddenField runat="server" ID="hflocation"/></td>
                                                    <td><asp:HiddenField runat="server" ID="hfrace"/></td>
                                                    <td><asp:HiddenField runat="server" ID="hfstatus"/></td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                        </asp:UpdatePanel> 
                                    </div>
                                    <table style="width: 100%">
                                        <tr>
                                            <td>
                                               <span style="text-align:right;font-weight:bolder;font-size:12px;color: #00549f;">Show Labels:</span><br />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:CheckBoxList ID="ChkStatisticalList2" runat="server" RepeatDirection="Horizontal">
                                                    <asp:ListItem Selected="True">Total number of client</asp:ListItem>
                                                    <asp:ListItem Selected="True">Student Name</asp:ListItem>
                                                    <asp:ListItem Selected="True">Location</asp:ListItem>
                                                    <asp:ListItem Selected="True">City</asp:ListItem>
                                                    <asp:ListItem Selected="True">State</asp:ListItem>
                                                    <asp:ListItem Selected="True">Primary Language</asp:ListItem>
                                                    <asp:ListItem Selected="True">Race</asp:ListItem>
                                                    <asp:ListItem Selected="True">Placement Type</asp:ListItem>
                                                    <asp:ListItem Selected="True">Department</asp:ListItem>
                                                    <asp:ListItem Selected="True">Program</asp:ListItem>
                                                    <asp:ListItem Selected="True">Gender</asp:ListItem>
                                                    <asp:ListItem Selected="True">Status</asp:ListItem>
                                                    <%--<asp:ListItem Selected="True" style="display:none;">Maximum client occupancy</asp:ListItem>--%>
                                                </asp:CheckBoxList>
                                            </td>

                                        </tr>
                                        <tr>
                                            <td style="text-align: right">
                                                <asp:Button ID="btnShowStatistical2" runat="server" Text="Show Report" BackColor="#03507D" ForeColor="#FFFFFF" Font-Bold="True" OnClick="btnShowStatistical2_Click" />
                                                <asp:Button ID="btnReset" runat="server" Text="Reset" BackColor="#03507D" ForeColor="#FFFFFF" Font-Bold="True" OnClick="btnReset_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                    <div runat="server" id="divnodata" visible="false">
                                        <p style="text-align:center;vertical-align:central">Please select report items</p>
                                    </div>
                                </div>
                                <div runat="server" id="divchanges" visible="false">
                                    <table style="width:100%">
                                          <tr>
                                            <td>Start Date</td>
                                            <td>
                                                <asp:TextBox ID="txtchangeSdate" runat="server" CssClass="datepicker" onkeypress="return false"></asp:TextBox></td>
                                            <td></td>
                                            <td>End Date</td>
                                            <td>
                                                <asp:TextBox ID="txtchangeEdate" runat="server" CssClass="datepicker" onkeypress="return false"></asp:TextBox></td>
                                            <td></td>
                                            <td></td>
                                            <td>
                                                <asp:Button ID="btnChangeResult" runat="server" Text="Show Report" BackColor="#03507D" ForeColor="#FFFFFF" Font-Bold="True" OnClick="btnChangeResult_Click" OnClientClick="return ValidateChanges();" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div>
                                    <asp:HiddenField ID="hdnballet" runat="server" />
                                    <asp:HiddenField ID="hdnDateRange" runat="server" />
                                </div>
                            </div>
                            <div style="width: 100%; overflow-x: auto">
                                <rsweb:ReportViewer ID="RVClientReport" runat="server" ProcessingMode="Remote" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" ShowBackButton="false" ShowCredentialPrompts="false" ShowDocumentMapButton="true" ShowExportControls="true" ShowFindControls="false" ShowPageNavigationControls="true" ShowParameterPrompts="true" ShowPrintButton="false" ShowPromptAreaButton="true" ShowRefreshButton="false" ShowToolBar="true" ShowWaitControlCancelLink="true" ShowZoomControl="false" SizeToReportContent="true" Width="100%" Visible="false" AsyncRendering="true" Height="1000px">

                                    <ServerReport ReportServerUrl="<%$ appSettings:ReportUrl %>" />

                                </rsweb:ReportViewer>

                            </div>
                        </div>




                    </div>





                    <div class="clear">
                        <asp:HiddenField ID="hdnMenu" runat="server" />
                    </div>
                </div>

                <div class="clear"></div>
            </div>

            <div class="clear"></div>
            <div class="footer">

                <img src="../../Documents/images/smllogo.JPG" width="100" height="23" />
                <div class="copyright">&copy; Copyright 2015, Melmark, Inc. All rights reserved.</div>
            </div>


            <div class="clear"></div>
        </div>

    </form>

    <script type="text/javascript">
        $(document).ready(function () {

            var MenuType = document.getElementById('hdnMenu').value;
            if (MenuType != "") {
                ChangeSelectedMenu(MenuType);
            }
        });
    </script>
</body>
<script type="text/javascript">
    $(document).ready(function () {

        $("#ddlDeptLocDept").prop('disabled', true);
        $("#ddlDeptLocLoc").prop('disabled', true);
        $("#ddlDeptPlctypeDept").prop('disabled', true);
        $("#ddlDeptPlctypePlcType").prop('disabled', true);
        $("#ddlLocLoc").prop('disabled', true);
        $("#txtActiveStartDate").prop('disabled', true);
        $("#txtActiveEndDate").prop('disabled', true);
        $("#txtNewStartDate").prop('disabled', true);
        $("#txtNewEndDate").prop('disabled', true);
        $("#txtDischrStartDate").prop('disabled', true);
        $("#txtDischrEndDate").prop('disabled', true);

        if ($('#hdnballet').val() != "") {
            if ($('#hdnballet').val() == "Choose Department and Location") {
                $('#rbtnDeptLoc').attr('checked', true);
                $("#ddlDeptLocDept").prop('disabled', false);
                $("#ddlDeptLocLoc").prop('disabled', false);
            }
            else if ($('#hdnballet').val() == "Choose Department and Placement Type") {
                $('#rbtnDeptPlaceType').attr('checked', true);
                $("#ddlDeptPlctypeDept").prop('disabled', false);
                $("#ddlDeptPlctypePlcType").prop('disabled', false);
            }
            else if ($('#hdnballet').val() == "Choose Location") {
                $('#rbtnLocation').attr('checked', true);
                $("#ddlLocLoc").prop('disabled', false);
            }

        }
        if ($('#hdnDateRange').val() != "") {
            if ($('#hdnDateRange').val() == "Active Placement") {
                $('#rbtnActivePlc').attr('checked', true);
                $("#txtActiveStartDate").prop('disabled', false);
                $("#txtActiveEndDate").prop('disabled', false);
            }
            else if ($('#hdnDateRange').val() == "Discharged Placement") {
                $('#rbtnDischargedPlc').attr('checked', true);
                $("#txtDischrStartDate").prop('disabled', false);
                $("#txtDischrEndDate").prop('disabled', false);
            }
            else if ($('#hdnDateRange').val() == "New Placement") {
                $('#rbtnNewPlacement').attr('checked', true);
                $("#txtNewStartDate").prop('disabled', false);
                $("#txtNewEndDate").prop('disabled', false);
            }
        }

        var date = new Date();
        date.setDate(date.getDate());
        $('.datepicker').datepicker(
         {
             dateFormat: "mm/dd/yy",
             changeMonth: true,
             changeYear: true,
             showAnim: "fadeIn",
             yearRange: 'c-100:c+100',
             //minDate: date,
             /* fix buggy IE focus functionality */
             fixFocusIE: false,
             constrainInput: false
         });


        $(".chb").click(function () {

            if ($(this).prop('checked') != false) {
                $(".chb").prop('checked', false);
                $(this).prop('checked', true);
            }
            else {
                $('#hdnballet').val("");
            }
            if ($('#rbtnDeptLoc').prop('checked') == true) {
                $('#hdnballet').val($('#rbtnDeptLoc').val());
                $("#ddlDeptLocDept").prop('disabled', false);
                $("#ddlDeptLocLoc").prop('disabled', false);
            }
            else {
                $('#ddlDeptLocDept').prop('selectedIndex', 0);
                $('#ddlDeptLocLoc').prop('selectedIndex', 0);
                $("#ddlDeptLocDept").prop('disabled', true);
                $("#ddlDeptLocLoc").prop('disabled', true);
            }

            if ($('#rbtnDeptPlaceType').prop('checked') == true) {

                $('#hdnballet').val($('#rbtnDeptPlaceType').val());
                $("#ddlDeptPlctypeDept").prop('disabled', false);
                $("#ddlDeptPlctypePlcType").prop('disabled', false);
            }
            else {
                $('#ddlDeptPlctypeDept').prop('selectedIndex', 0);
                $('#ddlDeptPlctypePlcType').prop('selectedIndex', 0);
                $("#ddlDeptPlctypeDept").prop('disabled', true);
                $("#ddlDeptPlctypePlcType").prop('disabled', true);
            }

            if ($('#rbtnLocation').prop('checked') == true) {
                $('#hdnballet').val($('#rbtnLocation').val());
                $("#ddlLocLoc").prop('disabled', false);
            }
            else {
                $('#ddlLocLoc').prop('selectedIndex', 0);
                $("#ddlLocLoc").prop('disabled', true);
            }

        });



        $(".DateRange").click(function () {

            if ($(this).prop('checked') != false) {
                $(".DateRange").prop('checked', false);
                $(this).prop('checked', true);
            }
            else {
                $('#hdnDateRange').val("");
            }

            if ($('#rbtnActivePlc').prop('checked') == true) {
                $('#hdnDateRange').val($('#rbtnActivePlc').val());
                $("#txtActiveStartDate").prop('disabled', false);
                $("#txtActiveEndDate").prop('disabled', false);
            }
            else {
                $("#txtActiveStartDate").prop('disabled', true);
                $("#txtActiveEndDate").prop('disabled', true);
                $("#txtActiveStartDate").val('');
                $("#txtActiveEndDate").val('');
            }

            if ($('#rbtnDischargedPlc').prop('checked') == true) {
                $('#hdnDateRange').val($('#rbtnDischargedPlc').val());
                $("#txtDischrStartDate").prop('disabled', false);
                $("#txtDischrEndDate").prop('disabled', false);
            }
            else {
                $("#txtDischrStartDate").prop('disabled', true);
                $("#txtDischrEndDate").prop('disabled', true);
                $("#txtDischrStartDate").val('');
                $("#txtDischrEndDate").val('');
            }

            if ($('#rbtnNewPlacement').prop('checked') == true) {
                $('#hdnDateRange').val($('#rbtnNewPlacement').val());
                $("#txtNewStartDate").prop('disabled', false);
                $("#txtNewEndDate").prop('disabled', false);
            }
            else {
                $("#txtNewStartDate").prop('disabled', true);
                $("#txtNewEndDate").prop('disabled', true);
                $("#txtNewStartDate").val('');
                $("#txtNewEndDate").val('');
            }


        });


        $.get("../ClientRegistration/GetTitleReport", function (data) {
            document.title = data;

        });



    });
</script>
























</html>

