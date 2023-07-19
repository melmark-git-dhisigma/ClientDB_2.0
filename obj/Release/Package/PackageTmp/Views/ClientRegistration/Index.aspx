<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Share/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<ClientDB.Models.RegistrationModel>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Melmark New England
   
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <link href="../Documents/CSS/style.css" rel="stylesheet" />
    <link href="../Documents/CSS/General.css" rel="stylesheet" />

    <link href="../Documents/CSS/jquery-ui.css" rel="stylesheet" />
    <%--<script src="../../Documents/JS/jquery-1.8.2.js"></script>--%>
    <script src="../Documents/JS/jquery-ui-1.11.2.js"></script>


    <script type="text/javascript">
        $(function () {
            setTimeout(function () {
                 <% if (ViewBag.Param == 0)
                    {%>
                $('#content').load('../ClientRegistration/ClientRegistration?data=0|Fill');
                <%}%>
            <% else
                    {%>

                $('.imgcontainer').css("display", "block");
                $('.imgcontainer').load('../Contact/ImageUploadPanel?edit=1');
                $('.EditProfile').css("display", "block");

                setTimeout(function () {
                    $('#content').load('../ClientRegistration/ClientRegistration?data=<%= ViewBag.Param%>|Fill');
                }, 500);
            <%}%>
            }, 1000);


            //  $(".datepicker").datepicker();



            //var options = {
            //    success: showResponse  // post-submit callback 
            //};

            //<div id="calender" style="float: left; font-size: 12px; margin: 5px 5px 5px 11px; position: relative; width: 150px;"></div>     

            //       $('#registrationForm').ajaxForm(options);

            // $('.leftMenu').click(function () {
            $(document).delegate('.leftMenu', "click", function () {

                var elmId = $(this).attr('id');
                $('.leftMenu').removeClass('current');
                $(this).addClass('current');
                if (elmId == "btnGeneral") {

                    $('#content').load('../ClientRegistration/ClientRegistration?data=<%= ViewBag.Param%>|Fill');
                    $('.imgcontainer').css("display", "block");
                    $('.imgcontainer').load('../Contact/ImageUploadPanel?edit=1');
                    $('.EditProfile').css("display", "block");
                    $('#calender').css("display", "none");

                    $('#btnGeneral').css("background", "#23a7e3");

                    $('#btnGeneral').hover(function () {
                        $(this).css({ "background-color": "#23a7e3" });
                    }, function () {
                        $(this).css({ "background-color": "#23a7e3" });
                    });

                    $('#btnMedical').hover(function () {
                        $(this).css({ "background-color": "#3dc153" });
                    }, function () {
                        $(this).css({ "background-color": "#007ab1" });
                    });

                    $('#btnPlacement').hover(function () {
                        $(this).css({ "background-color": "#3dc153" });
                    }, function () {
                        $(this).css({ "background-color": "#007ab1" });
                    });

                    $('#btnContact').hover(function () {
                        $(this).css({ "background-color": "#3dc153" });
                    }, function () {
                        $(this).css({ "background-color": "#007ab1" });
                    });

                    $('#btnVisitation').hover(function () {
                        $(this).css({ "background-color": "#3dc153" });
                    }, function () {
                        $(this).css({ "background-color": "#007ab1" });
                    });

                    $('#btnEventLogs').hover(function () {
                        $(this).css({ "background-color": "#3dc153" });
                    }, function () {
                        $(this).css({ "background-color": "#007ab1" });
                    });

                    $('#btnForms').hover(function () {
                        $(this).css({ "background-color": "#3dc153" });
                    }, function () {
                        $(this).css({ "background-color": "#007ab1" });
                    });

                    $('#btnProgress').hover(function () {
                        $(this).css({ "background-color": "#3dc153" });
                    }, function () {
                        $(this).css({ "background-color": "#007ab1" });
                    });

                    $('#btnLetter').hover(function () {
                        $(this).css({ "background-color": "#3dc153" });
                    }, function () {
                        $(this).css({ "background-color": "#007ab1" });
                    });

                    $('#btnCallLogs').hover(function () {
                        $(this).css({ "background-color": "#3dc153" });
                    }, function () {
                        $(this).css({ "background-color": "#007ab1" });
                    });


                    $('#btnMedical').css("background", "#007ab1");
                    $('#btnPlacement').css("background", "#007ab1");
                    $('#btnVisitation').css("background", "#007ab1");
                    $('#btnLetter').css("background", "#007ab1");
                    $('#btnEventLogs').css("background", "#007ab1");
                    $('#btnForms').css("background", "#007ab1");
                    $('#btnProgress').css("background", "#007ab1");
                    $('#btnContact').css("background", "#007ab1");
                    $('#btnCallLogs').css("background", "#007ab1");

                    if ($('#tdstudentName').length != 0) {
                        $('#dvHeader').html("Client Details : " + $('#tdstudentName').html())
                    } else {
                        $('#dvHeader').html("Client Details").html();
                    }
                    //$('#dvHeader').html("Client Details").html();
                }
                if (elmId == "btnMedical") {

                    $('#content').load('../Medical/Medical/');
                    //       $('#content').load('/Medical/FillMedicalData/0');
                    $('.imgcontainer').css("display", "block");
                    $('.imgcontainer').load('../Contact/ImageUploadPanel?edit=0');

                    $('#calender').css("display", "block");

                    if ($('#tdstudentName').length != 0) {
                        $('#dvHeader').html("Medical Details : " + $('#tdstudentName').html())
                    } else {
                        $('#dvHeader').html("Medical Details").html();
                    }

                    $('#btnMedical').css("background", "#23a7e3");

                    $('#btnMedical').hover(function () {
                        $(this).css({ "background-color": "#23a7e3" });
                    }, function () {
                        $(this).css({ "background-color": "#23a7e3" });
                    });

                    $('#btnGeneral').hover(function () {
                        $(this).css({ "background-color": "#3dc153" });
                    }, function () {
                        $(this).css({ "background-color": "#007ab1" });
                    });

                    $('#btnPlacement').hover(function () {
                        $(this).css({ "background-color": "#3dc153" });
                    }, function () {
                        $(this).css({ "background-color": "#007ab1" });
                    });

                    $('#btnContact').hover(function () {
                        $(this).css({ "background-color": "#3dc153" });
                    }, function () {
                        $(this).css({ "background-color": "#007ab1" });
                    });

                    $('#btnVisitation').hover(function () {
                        $(this).css({ "background-color": "#3dc153" });
                    }, function () {
                        $(this).css({ "background-color": "#007ab1" });
                    });

                    $('#btnEventLogs').hover(function () {
                        $(this).css({ "background-color": "#3dc153" });
                    }, function () {
                        $(this).css({ "background-color": "#007ab1" });
                    });

                    $('#btnForms').hover(function () {
                        $(this).css({ "background-color": "#3dc153" });
                    }, function () {
                        $(this).css({ "background-color": "#007ab1" });
                    });

                    $('#btnProgress').hover(function () {
                        $(this).css({ "background-color": "#3dc153" });
                    }, function () {
                        $(this).css({ "background-color": "#007ab1" });
                    });

                    $('#btnLetter').hover(function () {
                        $(this).css({ "background-color": "#3dc153" });
                    }, function () {
                        $(this).css({ "background-color": "#007ab1" });
                    });

                    $('#btnCallLogs').hover(function () {
                        $(this).css({ "background-color": "#3dc153" });
                    }, function () {
                        $(this).css({ "background-color": "#007ab1" });
                    });

                    $('#btnContact').css("background", "#007ab1");
                    $('#btnGeneral').css("background", "#007ab1");
                    $('#btnPlacement').css("background", "#007ab1");
                    $('#btnVisitation').css("background", "#007ab1");
                    $('#btnEventLogs').css("background", "#007ab1");
                    $('#btnForms').css("background", "#007ab1");
                    $('#btnLetter').css("background", "#007ab1");
                    $('#btnProgress').css("background", "#007ab1");
                    $('#btnCallLogs').css("background", "#007ab1");

                }
                if (elmId == "btnPlacement") {                   
                    $('#content').load('../Placement/Placement/');
                    $('#calender').css("display", "none");
                    $('.imgcontainer').css("display", "block");
                    $('.imgcontainer').load('../Contact/ImageUploadPanel?edit=0');

                    if ($('#tdstudentName').length != 0) {
                        $('#dvHeader').html("Placements : " + $('#tdstudentName').html())
                    } else {
                        $('#dvHeader').html("Placements").html();
                    }
                    $('#btnPlacement').css("background", "#23a7e3");

                    $('#btnPlacement').hover(function () {
                        $(this).css({ "background-color": "#23a7e3" });
                    }, function () {
                        $(this).css({ "background-color": "#23a7e3" });
                    });

                    $('#btnGeneral').hover(function () {
                        $(this).css({ "background-color": "#3dc153" });
                    }, function () {
                        $(this).css({ "background-color": "#007ab1" });
                    });

                    $('#btnMedical').hover(function () {
                        $(this).css({ "background-color": "#3dc153" });
                    }, function () {
                        $(this).css({ "background-color": "#007ab1" });
                    });

                    $('#btnContact').hover(function () {
                        $(this).css({ "background-color": "#3dc153" });
                    }, function () {
                        $(this).css({ "background-color": "#007ab1" });
                    });

                    $('#btnVisitation').hover(function () {
                        $(this).css({ "background-color": "#3dc153" });
                    }, function () {
                        $(this).css({ "background-color": "#007ab1" });
                    });

                    $('#btnEventLogs').hover(function () {
                        $(this).css({ "background-color": "#3dc153" });
                    }, function () {
                        $(this).css({ "background-color": "#007ab1" });
                    });

                    $('#btnForms').hover(function () {
                        $(this).css({ "background-color": "#3dc153" });
                    }, function () {
                        $(this).css({ "background-color": "#007ab1" });
                    });

                    $('#btnProgress').hover(function () {
                        $(this).css({ "background-color": "#3dc153" });
                    }, function () {
                        $(this).css({ "background-color": "#007ab1" });
                    });

                    $('#btnLetter').hover(function () {
                        $(this).css({ "background-color": "#3dc153" });
                    }, function () {
                        $(this).css({ "background-color": "#007ab1" });
                    });

                    $('#btnCallLogs').hover(function () {
                        $(this).css({ "background-color": "#3dc153" });
                    }, function () {
                        $(this).css({ "background-color": "#007ab1" });
                    });

                    $('#btnGeneral').css("background", "#007ab1");
                    $('#btnMedical').css("background", "#007ab1");

                    $('#btnVisitation').css("background", "#007ab1");
                    $('#btnEventLogs').css("background", "#007ab1");
                    $('#btnForms').css("background", "#007ab1");
                    $('#btnLetter').css("background", "#007ab1");
                    $('#btnProgress').css("background", "#007ab1");
                    $('#btnContact').css("background", "#007ab1");
                    $('#btnCallLogs').css("background", "#007ab1");

                }
                if (elmId == "btnContact") {
                    $('#content').load('../Contact/ListContactVendor/');
                    $('#calender').css("display", "none");
                    $('.imgcontainer').css("display", "block");
                    $('.imgcontainer').load('../Contact/ImageUploadPanel?edit=0');

                    if ($('#tdstudentName').length != 0) {
                        $('#dvHeader').html("Contact / Vendor : " + $('#tdstudentName').html())
                    } else {
                        $('#dvHeader').html("Contact / Vendor").html();
                    }

                    $('#btnContact').css("background", "#23a7e3");

                    $('#btnContact').hover(function () {
                        $(this).css({ "background-color": "#23a7e3" });
                    }, function () {
                        $(this).css({ "background-color": "#23a7e3" });
                    });

                    $('#btnGeneral').hover(function () {
                        $(this).css({ "background-color": "#3dc153" });
                    }, function () {
                        $(this).css({ "background-color": "#007ab1" });
                    });

                    $('#btnMedical').hover(function () {
                        $(this).css({ "background-color": "#3dc153" });
                    }, function () {
                        $(this).css({ "background-color": "#007ab1" });
                    });
                    $('#btnVisitation').hover(function () {
                        $(this).css({ "background-color": "#3dc153" });
                    }, function () {
                        $(this).css({ "background-color": "#007ab1" });
                    });

                    $('#btnEventLogs').hover(function () {
                        $(this).css({ "background-color": "#3dc153" });
                    }, function () {
                        $(this).css({ "background-color": "#007ab1" });
                    });

                    $('#btnForms').hover(function () {
                        $(this).css({ "background-color": "#3dc153" });
                    }, function () {
                        $(this).css({ "background-color": "#007ab1" });
                    });

                    $('#btnProgress').hover(function () {
                        $(this).css({ "background-color": "#3dc153" });
                    }, function () {
                        $(this).css({ "background-color": "#007ab1" });
                    });

                    $('#btnLetter').hover(function () {
                        $(this).css({ "background-color": "#3dc153" });
                    }, function () {
                        $(this).css({ "background-color": "#007ab1" });
                    });

                    $('#btnCallLogs').hover(function () {
                        $(this).css({ "background-color": "#3dc153" });
                    }, function () {
                        $(this).css({ "background-color": "#007ab1" });
                    });
                    $('#btnPlacement').hover(function () {
                        $(this).css({ "background-color": "#3dc153" });
                    }, function () {
                        $(this).css({ "background-color": "#007ab1" });
                    });
                    $('#btnGeneral').css("background", "#007ab1");
                    $('#btnMedical').css("background", "#007ab1");
                    $('#btnPlacement').css("background", "#007ab1");

                    $('#btnLetter').css("background", "#007ab1");
                    $('#btnVisitation').css("background", "#007ab1");
                    $('#btnEventLogs').css("background", "#007ab1");
                    $('#btnForms').css("background", "#007ab1");
                    $('#btnProgress').css("background", "#007ab1");
                    $('#btnCallLogs').css("background", "#007ab1");



                }
                if (elmId == "btnVisitation") {
                    $('#content').load('../Visitation/Visitation/');
                    $('#calender').css("display", "none");
                    $('.imgcontainer').css("display", "block");
                    $('.imgcontainer').load('../Contact/ImageUploadPanel?edit=0');

                    if ($('#tdstudentName').length != 0) {
                        $('#dvHeader').html("Visitation / Trip : " + $('#tdstudentName').html());
                    } else {
                        $('#dvHeader').html("Visitation / Trip").html();
                    }

                    $('#btnVisitation').css("background", "#23a7e3");

                    $('#btnVisitation').hover(function () {
                        $(this).css({ "background-color": "#23a7e3" });
                    }, function () {
                        $(this).css({ "background-color": "#23a7e3" });
                    });
                    $('#btnGeneral').hover(function () {
                        $(this).css({ "background-color": "#3dc153" });
                    }, function () {
                        $(this).css({ "background-color": "#007ab1" });
                    });

                    $('#btnMedical').hover(function () {
                        $(this).css({ "background-color": "#3dc153" });
                    }, function () {
                        $(this).css({ "background-color": "#007ab1" });
                    });
                    $('#btnEventLogs').hover(function () {
                        $(this).css({ "background-color": "#3dc153" });
                    }, function () {
                        $(this).css({ "background-color": "#007ab1" });
                    });

                    $('#btnForms').hover(function () {
                        $(this).css({ "background-color": "#3dc153" });
                    }, function () {
                        $(this).css({ "background-color": "#007ab1" });
                    });

                    $('#btnProgress').hover(function () {
                        $(this).css({ "background-color": "#3dc153" });
                    }, function () {
                        $(this).css({ "background-color": "#007ab1" });
                    });

                    $('#btnLetter').hover(function () {
                        $(this).css({ "background-color": "#3dc153" });
                    }, function () {
                        $(this).css({ "background-color": "#007ab1" });
                    });

                    $('#btnCallLogs').hover(function () {
                        $(this).css({ "background-color": "#3dc153" });
                    }, function () {
                        $(this).css({ "background-color": "#007ab1" });
                    });
                    $('#btnPlacement').hover(function () {
                        $(this).css({ "background-color": "#3dc153" });
                    }, function () {
                        $(this).css({ "background-color": "#007ab1" });
                    });
                    $('#btnContact').hover(function () {
                        $(this).css({ "background-color": "#3dc153" });
                    }, function () {
                        $(this).css({ "background-color": "#007ab1" });
                    });

                    $('#btnGeneral').css("background", "#007ab1");
                    $('#btnMedical').css("background", "#007ab1");
                    $('#btnPlacement').css("background", "#007ab1");
                    $('#btnContact').css("background", "#007ab1");
                    $('#btnEventLogs').css("background", "#007ab1");
                    $('#btnLetter').css("background", "#007ab1");
                    $('#btnForms').css("background", "#007ab1");
                    $('#btnProgress').css("background", "#007ab1");
                    $('#btnCallLogs').css("background", "#007ab1");


                }
                if (elmId == "btnEventLogs") {
                    $('#content').load('../Event/EventsList/');
                    $('#calender').css("display", "none");
                    $('.imgcontainer').css("display", "block");
                    $('.imgcontainer').load('../Contact/ImageUploadPanel?edit=0');

                    if ($('#tdstudentName').length != 0) {
                        $('#dvHeader').html("Event Logs : " + $('#tdstudentName').html())
                    } else {
                        $('#dvHeader').html("Event Logs :").html();
                    }
                    $('#btnEventLogs').css("background", "#23a7e3");

                    $('#btnEventLogs').hover(function () {
                        $(this).css({ "background-color": "#23a7e3" });
                    }, function () {
                        $(this).css({ "background-color": "#23a7e3" });
                    });

                    $('#btnGeneral').hover(function () {
                        $(this).css({ "background-color": "#3dc153" });
                    }, function () {
                        $(this).css({ "background-color": "#007ab1" });
                    });

                    $('#btnMedical').hover(function () {
                        $(this).css({ "background-color": "#3dc153" });
                    }, function () {
                        $(this).css({ "background-color": "#007ab1" });
                    });

                    $('#btnForms').hover(function () {
                        $(this).css({ "background-color": "#3dc153" });
                    }, function () {
                        $(this).css({ "background-color": "#007ab1" });
                    });

                    $('#btnProgress').hover(function () {
                        $(this).css({ "background-color": "#3dc153" });
                    }, function () {
                        $(this).css({ "background-color": "#007ab1" });
                    });

                    $('#btnLetter').hover(function () {
                        $(this).css({ "background-color": "#3dc153" });
                    }, function () {
                        $(this).css({ "background-color": "#007ab1" });
                    });

                    $('#btnCallLogs').hover(function () {
                        $(this).css({ "background-color": "#3dc153" });
                    }, function () {
                        $(this).css({ "background-color": "#007ab1" });
                    });
                    $('#btnPlacement').hover(function () {
                        $(this).css({ "background-color": "#3dc153" });
                    }, function () {
                        $(this).css({ "background-color": "#007ab1" });
                    });
                    $('#btnContact').hover(function () {
                        $(this).css({ "background-color": "#3dc153" });
                    }, function () {
                        $(this).css({ "background-color": "#007ab1" });
                    });
                    $('#btnVisitation').hover(function () {
                        $(this).css({ "background-color": "#3dc153" });
                    }, function () {
                        $(this).css({ "background-color": "#007ab1" });
                    });
                    $('#btnGeneral').css("background", "#007ab1");
                    $('#btnMedical').css("background", "#007ab1");
                    $('#btnPlacement').css("background", "#007ab1");
                    $('#btnContact').css("background", "#007ab1");
                    $('#btnVisitation').css("background", "#007ab1");

                    $('#btnForms').css("background", "#007ab1");
                    $('#btnLetter').css("background", "#007ab1");
                    $('#btnProgress').css("background", "#007ab1");
                    $('#btnCallLogs').css("background", "#007ab1");


                }
                if (elmId == "btnForms") {
                    $('#content').load('../Forms/ListDocuments/');
                    $('#calender').css("display", "none");
                    $('.imgcontainer').css("display", "block");
                    $('.imgcontainer').load('../Contact/ImageUploadPanel?edit=0');

                    if ($('#tdstudentName').length != 0) {
                        $('#dvHeader').html("Documents : " + $('#tdstudentName').html())
                    } else {
                        $('#dvHeader').html("Documents :").html();
                    }

                    $('#btnForms').css("background", "#23a7e3");

                    $('#btnForms').hover(function () {
                        $(this).css({ "background-color": "#23a7e3" });
                    }, function () {
                        $(this).css({ "background-color": "#23a7e3" });
                    });

                    $('#btnGeneral').hover(function () {
                        $(this).css({ "background-color": "#3dc153" });
                    }, function () {
                        $(this).css({ "background-color": "#007ab1" });
                    });

                    $('#btnMedical').hover(function () {
                        $(this).css({ "background-color": "#3dc153" });
                    }, function () {
                        $(this).css({ "background-color": "#007ab1" });
                    });

                    $('#btnProgress').hover(function () {
                        $(this).css({ "background-color": "#3dc153" });
                    }, function () {
                        $(this).css({ "background-color": "#007ab1" });
                    });

                    $('#btnLetter').hover(function () {
                        $(this).css({ "background-color": "#3dc153" });
                    }, function () {
                        $(this).css({ "background-color": "#007ab1" });
                    });

                    $('#btnCallLogs').hover(function () {
                        $(this).css({ "background-color": "#3dc153" });
                    }, function () {
                        $(this).css({ "background-color": "#007ab1" });
                    });
                    $('#btnPlacement').hover(function () {
                        $(this).css({ "background-color": "#3dc153" });
                    }, function () {
                        $(this).css({ "background-color": "#007ab1" });
                    });
                    $('#btnContact').hover(function () {
                        $(this).css({ "background-color": "#3dc153" });
                    }, function () {
                        $(this).css({ "background-color": "#007ab1" });
                    });
                    $('#btnVisitation').hover(function () {
                        $(this).css({ "background-color": "#3dc153" });
                    }, function () {
                        $(this).css({ "background-color": "#007ab1" });
                    });

                    $('#btnEventLogs').hover(function () {
                        $(this).css({ "background-color": "#3dc153" });
                    }, function () {
                        $(this).css({ "background-color": "#007ab1" });
                    });
                    $('#btnGeneral').css("background", "#007ab1");
                    $('#btnMedical').css("background", "#007ab1");
                    $('#btnPlacement').css("background", "#007ab1");
                    $('#btnContact').css("background", "#007ab1");
                    $('#btnVisitation').css("background", "#007ab1");
                    $('#btnEventLogs').css("background", "#007ab1");

                    $('#btnProgress').css("background", "#007ab1");
                    $('#btnLetter').css("background", "#007ab1");
                    $('#btnCallLogs').css("background", "#007ab1");
                }
                if (elmId == "btnProgress") {
                    $('#content').load('../Progress/ProgressRpt/');
                    $('#calender').css("display", "none");
                    $('.imgcontainer').css("display", "block");
                    $('.imgcontainer').load('../Contact/ImageUploadPanel?edit=0');
                    //$('.rightContainer').hide();

                    if ($('#tdstudentName').length != 0) {
                        $('#dvHeader').html("Progress Reports : " + $('#tdstudentName').html())
                    } else {
                        $('#dvHeader').html("Progress Reports")();
                    }
                    $('#btnProgress').css("background", "#23a7e3");

                    $('#btnProgress').hover(function () {
                        $(this).css({ "background-color": "#23a7e3" });
                    }, function () {
                        $(this).css({ "background-color": "#23a7e3" });
                    });

                    $('#btnGeneral').hover(function () {
                        $(this).css({ "background-color": "#3dc153" });
                    }, function () {
                        $(this).css({ "background-color": "#007ab1" });
                    });

                    $('#btnMedical').hover(function () {
                        $(this).css({ "background-color": "#3dc153" });
                    }, function () {
                        $(this).css({ "background-color": "#007ab1" });
                    });

                    $('#btnLetter').hover(function () {
                        $(this).css({ "background-color": "#3dc153" });
                    }, function () {
                        $(this).css({ "background-color": "#007ab1" });
                    });

                    $('#btnCallLogs').hover(function () {
                        $(this).css({ "background-color": "#3dc153" });
                    }, function () {
                        $(this).css({ "background-color": "#007ab1" });
                    });
                    $('#btnPlacement').hover(function () {
                        $(this).css({ "background-color": "#3dc153" });
                    }, function () {
                        $(this).css({ "background-color": "#007ab1" });
                    });
                    $('#btnContact').hover(function () {
                        $(this).css({ "background-color": "#3dc153" });
                    }, function () {
                        $(this).css({ "background-color": "#007ab1" });
                    });
                    $('#btnVisitation').hover(function () {
                        $(this).css({ "background-color": "#3dc153" });
                    }, function () {
                        $(this).css({ "background-color": "#007ab1" });
                    });

                    $('#btnEventLogs').hover(function () {
                        $(this).css({ "background-color": "#3dc153" });
                    }, function () {
                        $(this).css({ "background-color": "#007ab1" });
                    });

                    $('#btnForms').hover(function () {
                        $(this).css({ "background-color": "#3dc153" });
                    }, function () {
                        $(this).css({ "background-color": "#007ab1" });
                    });
                    $('#btnGeneral').css("background", "#007ab1");
                    $('#btnMedical').css("background", "#007ab1");
                    $('#btnPlacement').css("background", "#007ab1");
                    $('#btnContact').css("background", "#007ab1");
                    $('#btnVisitation').css("background", "#007ab1");
                    $('#btnEventLogs').css("background", "#007ab1");
                    $('#btnForms').css("background", "#007ab1");

                    $('#btnLetter').css("background", "#007ab1");
                    $('#btnCallLogs').css("background", "#007ab1");



                }
                if (elmId == "btnLetter") {
                    $('#content').load('../Letter/AllLetter/');
                    $('#calender').css("display", "none");
                    $('.imgcontainer').css("display", "block");
                    $('.imgcontainer').load('../Contact/ImageUploadPanel?edit=0');
                    //$('.rightContainer').hide();

                    if ($('#tdstudentName').length != 0) {
                        $('#dvHeader').html("Letter Tray : " + $('#tdstudentName').html())
                    } else {
                        $('#dvHeader').html("Letter Tray").html();
                    }

                    $('#btnLetter').css("background", "#23a7e3");

                    $('#btnLetter').hover(function () {
                        $(this).css({ "background-color": "#23a7e3" });
                    }, function () {
                        $(this).css({ "background-color": "#23a7e3" });
                    });

                    $('#btnGeneral').hover(function () {
                        $(this).css({ "background-color": "#3dc153" });
                    }, function () {
                        $(this).css({ "background-color": "#007ab1" });
                    });

                    $('#btnMedical').hover(function () {
                        $(this).css({ "background-color": "#3dc153" });
                    }, function () {
                        $(this).css({ "background-color": "#007ab1" });
                    });

                    $('#btnCallLogs').hover(function () {
                        $(this).css({ "background-color": "#3dc153" });
                    }, function () {
                        $(this).css({ "background-color": "#007ab1" });
                    });
                    $('#btnPlacement').hover(function () {
                        $(this).css({ "background-color": "#3dc153" });
                    }, function () {
                        $(this).css({ "background-color": "#007ab1" });
                    });
                    $('#btnContact').hover(function () {
                        $(this).css({ "background-color": "#3dc153" });
                    }, function () {
                        $(this).css({ "background-color": "#007ab1" });
                    });
                    $('#btnVisitation').hover(function () {
                        $(this).css({ "background-color": "#3dc153" });
                    }, function () {
                        $(this).css({ "background-color": "#007ab1" });
                    });

                    $('#btnEventLogs').hover(function () {
                        $(this).css({ "background-color": "#3dc153" });
                    }, function () {
                        $(this).css({ "background-color": "#007ab1" });
                    });

                    $('#btnForms').hover(function () {
                        $(this).css({ "background-color": "#3dc153" });
                    }, function () {
                        $(this).css({ "background-color": "#007ab1" });
                    });

                    $('#btnProgress').hover(function () {
                        $(this).css({ "background-color": "#3dc153" });
                    }, function () {
                        $(this).css({ "background-color": "#007ab1" });
                    });

                    $('#btnGeneral').css("background", "#007ab1");
                    $('#btnMedical').css("background", "#007ab1");
                    $('#btnPlacement').css("background", "#007ab1");
                    $('#btnContact').css("background", "#007ab1");
                    $('#btnVisitation').css("background", "#007ab1");
                    $('#btnEventLogs').css("background", "#007ab1");
                    $('#btnForms').css("background", "#007ab1");
                    $('#btnProgress').css("background", "#007ab1");

                    $('#btnCallLogs').css("background", "#007ab1");



                }

                if (elmId == "btnCallLogs") {

                    $('#content').load('../CallLog/CallLog/');
                    $('#calender').css("display", "none");
                    $('.imgcontainer').css("display", "block");
                    $('.imgcontainer').load('../Contact/ImageUploadPanel?edit=0');
                    //$('.rightContainer').hide();

                    if ($('#tdstudentName').length != 0) {
                        $('#dvHeader').html("Call Logs : " + $('#tdstudentName').html())
                    } else {
                        $('#dvHeader').html("Call Logs").html();
                    }

                    $('#btnCallLogs').css("background", "#23a7e3");

                    $('#btnCallLogs').hover(function () {
                        $(this).css({ "background-color": "#23a7e3" });
                    }, function () {
                        $(this).css({ "background-color": "#23a7e3" });
                    });

                    $('#btnGeneral').hover(function () {
                        $(this).css({ "background-color": "#3dc153" });
                    }, function () {
                        $(this).css({ "background-color": "#007ab1" });
                    });

                    $('#btnMedical').hover(function () {
                        $(this).css({ "background-color": "#3dc153" });
                    }, function () {
                        $(this).css({ "background-color": "#007ab1" });
                    });

                    $('#btnPlacement').hover(function () {
                        $(this).css({ "background-color": "#3dc153" });
                    }, function () {
                        $(this).css({ "background-color": "#007ab1" });
                    });
                    $('#btnContact').hover(function () {
                        $(this).css({ "background-color": "#3dc153" });
                    }, function () {
                        $(this).css({ "background-color": "#007ab1" });
                    });
                    $('#btnVisitation').hover(function () {
                        $(this).css({ "background-color": "#3dc153" });
                    }, function () {
                        $(this).css({ "background-color": "#007ab1" });
                    });

                    $('#btnEventLogs').hover(function () {
                        $(this).css({ "background-color": "#3dc153" });
                    }, function () {
                        $(this).css({ "background-color": "#007ab1" });
                    });

                    $('#btnForms').hover(function () {
                        $(this).css({ "background-color": "#3dc153" });
                    }, function () {
                        $(this).css({ "background-color": "#007ab1" });
                    });

                    $('#btnProgress').hover(function () {
                        $(this).css({ "background-color": "#3dc153" });
                    }, function () {
                        $(this).css({ "background-color": "#007ab1" });
                    });

                    $('#btnLetter').hover(function () {
                        $(this).css({ "background-color": "#3dc153" });
                    }, function () {
                        $(this).css({ "background-color": "#007ab1" });
                    });

                    $('#btnGeneral').css("background", "#007ab1");
                    $('#btnMedical').css("background", "#007ab1");
                    $('#btnPlacement').css("background", "#007ab1");
                    $('#btnContact').css("background", "#007ab1");
                    $('#btnVisitation').css("background", "#007ab1");
                    $('#btnEventLogs').css("background", "#007ab1");
                    $('#btnForms').css("background", "#007ab1");
                    $('#btnProgress').css("background", "#007ab1");
                    $('#btnLetter').css("background", "#007ab1");

                }

                if (elmId == "btnHome") {

                }

            });

            /*
                        //$("#btnGeneral").on("click", function (event) {
                        //    //event.preventDefault();
            
                        //    //if (jQuery("#registrationForm").validationEngine('validate')) {
                        //    $('#content').load('/ClientRegistration/ClientRegistration/');
                        //    //div.mainContainer div.contentPart div.ContentAreaContainer div.leftContainer input:hover
            
            
            
            
                        //    //}
                        //});
            
                        //$("#btnMedical").on("click", function (event) {
                        //    //event.preventDefault();
            
                        //    //if (jQuery("#registrationForm").validationEngine('validate')) {
                        //    //$('#content').load('/Medical/Medical/');
                        //    $('#content').load('/Medical/FillMedicalData/0');
                        //    //
            
            
                        //    //}
                        //});
            
                        //$("#btnPlacement").on("click", function (event) {
                        //    //event.preventDefault();
            
                        //    //if (jQuery("#registrationForm").validationEngine('validate')) {
                        //    $('#content').load('/Placement/Placement/');
            
            
                        //    //}
                        //});
            
                        //$("#btnContact").on("click", function (event) {
                        //    //event.preventDefault();
            
                        //    //if (jQuery("#registrationForm").validationEngine('validate')) {
                        //    $('#content').load('/Contact/ListContactVendor/');
                        //    // $('#content').load('/Contact/fillContactDetails/');
            
            
                        //    //}
                        //});
                        //$("#btnVisitation").on("click", function (event) {
                        //    //event.preventDefault();
                        //    $('#content').load('/Visitation/Visitation/');
                        //    //if (jQuery("#registrationForm").validationEngine('validate')) {
                        //    //$('#content').load('/ClientRegistration/ClientRegistration/');
            
            
                        //    //}
                        //});
                        //$("#btnReports").on("click", function (event) {
                        //    //event.preventDefault();
            
                        //    //if (jQuery("#registrationForm").validationEngine('validate')) {
                        //    $('#content').load('/ClientRegistration/ClientRegistration/');
            
            
                        //    //}
                        //});
            
                        //$("#btnHome").on("click", function (event) {
                        //    //event.preventDefault();
            
                        //    //if (jQuery("#registrationForm").validationEngine('validate')) {
                        //    $('#content').load('/Client/Index?Param=0');
            
            
                        //    //}
                        //});
                        //$("#btnEventLogs").on("click", function (event) {
                        //    //event.preventDefault();
            
                        //    //if (jQuery("#registrationForm").validationEngine('validate')) {
                        //    $('#content').load('/ClientRegistration/ClientRegistration/');
            
            
                        //    //}
                        //});
                        //$("#btnForms").on("click", function (event) {
                        //    //event.preventDefault();
            
                        //    //if (jQuery("#registrationForm").validationEngine('validate')) {
                        //    $('#content').load('/ClientRegistration/ClientRegistration/');
            
            
                        //    //}
                        //});
                        */


        });

        //function showResponse(responseText, statusText, xhr, $form) {
        //    alert(responseText);
        //    $('#partialMainArea').html(responseText);
        //}






    </script>


    <div class="mainContainer">
        <div class="topHead">
            <a class="admin" title="<%=ViewBag.Usename%>" href="#"><%=ViewBag.Usename %></a>
            <a class="logout" href="../Client/Logout/" title="Logout">Logout</a>
            <a class="Report" title="Reports" href="../Reports/ClientReports.aspx">Reports</a>
            <a class="home" title="Home" href="../Client/Index/">Home</a>
        </div>
        <div class="contentPart">
            <div class="imgcorner">
                <a class="logo" href="#">
                    <img src="../Documents/images/logo.JPG" width="200" height="40" /></a>
            </div>
            <div class="ContentAreaContainer">
                <div class="leftContainer">

                    <h2 id="Caption" class="leftMenuHeader">
                        <a class="gray MenuTooltip" title="Client Portal">Client Portal</a>
                    </h2>


                    <h2 id="btnGeneral" class="leftMenu">
                        <a class="gray MenuTooltip" title="General">General</a>
                    </h2>
                    <% var sesobj = (clsSession)Session["UserSessionClient"]; %>

                    <% if(sesobj.StudentId>0){ %>
                    <h2 id="btnMedical" class="leftMenu">
                        <a class="gray MenuTooltip" title="Medical">Medical</a>
                    </h2>


                    <h2 id="btnPlacement" class="leftMenu">
                        <a class="gray MenuTooltip" title="Placement">Placement</a>
                    </h2>

                    <h2 id="btnContact" class="leftMenu">
                        <a class="gray MenuTooltip" title="Contact / Vendor">Contact / Vendor</a>
                    </h2>

                    <%--<h2 id="btnVisitation" class="leftMenu">
                        <a class="gray MenuTooltip" title="Visitation / Trip">Visitation / Trip</a>
                    </h2>--%>
                    <h2 id="btnEventLogs" class="leftMenu">
                        <a class="gray MenuTooltip" title="Event Logs">Event Logs</a>

                    </h2>
                    <h2 id="btnForms" class="leftMenu">
                        <a class="gray MenuTooltip" title="Forms">Document Tray</a>
                    </h2>
                    <h2 id="btnLetter" class="leftMenu">
                        <a class="gray MenuTooltip" title="Letter Tray">Letter Tray</a>
                    </h2>

                    <h2 id="btnCallLogs" class="leftMenu">
                        <a class="gray MenuTooltip" title="Call Logs">Call Logs</a>
                    </h2>
                    <h2 id="btnProgress" class="leftMenu">
                        <a class="gray MenuTooltip" title="Progress Report">Progress Report</a>
                    </h2>
                    <% } %>
                </div>

                <div id="leftinnerdiv" style="float: left; width: 81%; border-right: 3px solid rgb(229, 229, 229); margin-top: 4px;">
                    <div id="dvHeader" style="background-color: rgb(241, 241, 241); font-size: 20px; height: 31px; color: rgb(0, 0, 0); font-weight: bold; width: 100%; padding: 8px 0px 0px 4px;">
                        Client Details
                    </div>
                    <div class="middleContainer">

                        <div id="content">
                        </div>

                    </div>



                    <div class="rightContainer">
                        <div class="imgcontainer" style="width: 100% !important">
                        </div>
                        <div id="calender" style="float: left; font-size: 12px; margin: 5px 5px 5px 0px; position: relative; width: 160px;"></div>

                    </div>

                </div>



                <div class="clear"></div>
            </div>

            <div class="clear"></div>
        </div>

        <div class="clear"></div>
        <div class="footer">
            <img src="../../Documents/images/smllogo.JPG" width="109" height="30px" />
            <div class="copyright">&copy; Copyright 2015, Melmark, Inc. All rights reserved.</div>
        </div>


        <div class="clear"></div>
    </div>

</asp:Content>
