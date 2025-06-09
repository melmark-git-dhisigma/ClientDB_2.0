<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Share/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<ClientDB.Models.RegistrationModel>>" %>




<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
     <%--<% var sess = (clsSession)Session["UserSessionClient"]; %>
       <% if(sess.SchoolId==2){ %> Melmark Pennsylvania
    <%}else if(sess.SchoolId==1){ %>
    Melmark New England
    <%} %>--%>
   EnvisionSmart
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%-- <script src="../../Documents/JS/jquery-ui-1.10.3.custom.js"></script>--%>
    <link href="../../Documents/CSS/style.css" rel="stylesheet" />
    <link href="../../Documents/CSS/General.css" rel="stylesheet" />

    <link href="../../Documents/CSS/jquery-ui.css" rel="stylesheet" />
    <%--<script src="../../Documents/JS/jquery-1.8.2.js"></script>--%>
    <%--<script src="../../Documents/JS/jquery-ui-1.11.2.js"></script>--%>
     <script src="../../Documents/JS/jquery-ui-1.8.24.js"></script>
    <script src="../../Documents/JS/jquery.validationEngine-en.js"></script>
    <script src="../../Documents/JS/jquery.validationEngine.js"></script>
    <script src="../../Documents/JS/jquery.form.js"></script>
    <script src="../../Documents/JS/jquery.mask.js"></script>
   <!--[if IE]>
  <script src="http://html5shiv.googlecode.com/svn/trunk/html5.js"></script>
<![endif]-->
    <script type="text/javascript">
        var a;
        function loadClientStaticDetails() {
            $('.staticClientDetials').load('../ClientRegistrationPA/staticClientDetailsPA?data=<%= ViewBag.Param%>|Fill', function () {
                //$('#tdstudentName').load('../ClientRegistrationPA/DisplayClientName');                
                $('#staticTab').html($('#tdstudentName').html());
                if (a == "btnPlacement") {
                    $('#disch').show();

                }
            });
        }


        $(function () {

            setTimeout(function () {
                <% if (ViewBag.Param == 0)
                   {%>
                $('#content').load('../ClientRegistrationPA/ClientRegistrationPA');
                $('.imgcontainer').css("display", "none");
                <%}%>
                <% else
                   {%>

               

                setTimeout(function () {
                    <%clsSession sess = (clsSession)Session["UserSessionClient"];
                      sess.StudentId = ViewBag.Param; %>
                    
                    $('.imgcontainer').css("display", "block");
                    $('.imgcontainer').load('../Contact/ImageUploadPanel?edit=1', function () {
                        $('.EditProfile').css("display", "none");

                        $('#content').load('../ClientRegistrationPA/ClientRegistrationPA?data=<%= ViewBag.Param%>|*');
                        $('.staticClientDetials').load('../ClientRegistrationPA/staticClientDetailsPA?data=<%= ViewBag.Param%>|Fill', function () {
                            //$('#tdstudentName').load('../ClientRegistrationPA/DisplayClientName');
                            $('#staticTab').html($('#tdstudentName').html());
                        });

                    });
                   

                }, 1000);
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
                a = elmId;
                if (elmId == "btnGeneral") {
                    $('#disch').hide();
                    $('#content').load('../ClientRegistrationPA/ClientRegistrationPA?data=<%= ViewBag.Param%>|*');
                    $('.imgcontainer').css("display", "block");
                    $('.imgcontainer').load('../Contact/ImageUploadPanel?edit=1');
                    $('.EditProfile').css("display", "block");
                    $('#calender').css("display", "none");
                   
                        $('#dvHeader').html("Client Details").html();
                  

                    $('#btnGeneral').css("background", "none repeat scroll 0 0 #23a7e3");

                    $('#btnGeneral').hover(function () {
                        $(this).css({ "background-color": "#23a7e3" });
                    }, function () {
                        $(this).css({ "background-color": "#23a7e3" });
                    });

                    $('#btnProtocol').hover(function () {
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

                    $('#btnCallLogs').hover(function () {
                        $(this).css({ "background-color": "#3dc153" });
                    }, function () {
                        $(this).css({ "background-color": "#007ab1" });
                    });

                    $('#btnProtocol').css("background", "none repeat scroll 0 0 #007ab1");
                    $('#btnMedical').css("background", "none repeat scroll 0 0 #007ab1");
                    $('#btnPlacement').css("background", "none repeat scroll 0 0 #007ab1");
                    $('#btnContact').css("background", "none repeat scroll 0 0 #007ab1");
                    $('#btnVisitation').css("background", "none repeat scroll 0 0 #007ab1");
                    $('#btnEventLogs').css("background", "none repeat scroll 0 0 #007ab1");
                    $('#btnForms').css("background", "none repeat scroll 0 0 #007ab1");
                    $('#btnProgress').css("background", "none repeat scroll 0 0 #007ab1");
                    $('#btnLetter').css("background", "none repeat scroll 0 0 #007ab1");
                    //$('#btnLetter').css("background", "#007ab1");
                    $('#btnCallLogs').css("background", "none repeat scroll 0 0 #007ab1");

                    $('#btnFaceSheet').hover(function () {
                        $(this).css({ "background-color": "#3dc153" });
                    }, function () {
                        $(this).css({ "background-color": "#007ab1" });
                    });
                    $('#btnFaceSheet').css("background", "#007ab1");
                }

                //start
                if (elmId == "btnProtocol") {
                    $('#disch').hide();
                    $('#content').load('../ProtocolSummary/Index/');
                    //       $('#content').load('/Medical/FillMedicalData/0');
                    $('.imgcontainer').css("display", "block");
                    $('.imgcontainer').load('../Contact/ImageUploadPanel?edit=0');
                    $('#calender').css("display", "block");
                    //???
                   
                        $('#dvHeader').html("Protocol Summary <font color='red'>(Under construction - Do not use right now)</font>")
                   

                    $('#btnProtocol').css("background", "none repeat scroll 0 0 #23a7e3");
                    $('#btnProtocol').hover(function () {
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

                    $('#btnCallLogs').hover(function () {
                        $(this).css({ "background-color": "#3dc153" });
                    }, function () {
                        $(this).css({ "background-color": "#007ab1" });
                    });



                    $('#btnGeneral').css("background", "none repeat scroll 0 0 #007ab1");
                    $('#btnMedical').css("background", "none repeat scroll 0 0 #007ab1");
                    $('#btnPlacement').css("background", "none repeat scroll 0 0 #007ab1");
                    $('#btnContact').css("background", "none repeat scroll 0 0 #007ab1");
                    $('#btnVisitation').css("background", "none repeat scroll 0 0 #007ab1");
                    $('#btnEventLogs').css("background", "none repeat scroll 0 0 #007ab1");
                    $('#btnForms').css("background", "none repeat scroll 0 0 #007ab1");
                    $('#btnLetter').css("background", "none repeat scroll 0 0 #007ab1");
                    //$('#btnLetter').css("background", "#007ab1");
                    $('#btnProgress').css("background", "none repeat scroll 0 0 #007ab1");
                    $('#btnCallLogs').css("background", "none repeat scroll 0 0 #007ab1");

                    $('#btnFaceSheet').hover(function () {
                        $(this).css({ "background-color": "#3dc153" });
                    }, function () {
                        $(this).css({ "background-color": "#007ab1" });
                    });
                    $('#btnFaceSheet').css("background", "#007ab1");
                }
                //end

                if (elmId == "btnMedical") {
                    $('#disch').hide();
                    $('#content').load('../Medical/Medical/');
                    //       $('#content').load('/Medical/FillMedicalData/0');
                    $('.imgcontainer').css("display", "block");
                    $('.imgcontainer').load('../Contact/ImageUploadPanel?edit=0');
                    $('#calender').css("display", "block");

                   
                        $('#dvHeader').html("Medical Details")
                   

                    $('#btnMedical').css("background", "none repeat scroll 0 0 #23a7e3");
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

                    $('#btnProtocol').hover(function () {
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



                    $('#btnGeneral').css("background", "none repeat scroll 0 0 #007ab1");
                    $('#btnProtocol').css("background", "none repeat scroll 0 0 #007ab1");
                    $('#btnPlacement').css("background", "none repeat scroll 0 0 #007ab1");
                    $('#btnContact').css("background", "none repeat scroll 0 0 #007ab1");
                    $('#btnVisitation').css("background", "none repeat scroll 0 0 #007ab1");
                    $('#btnEventLogs').css("background", "none repeat scroll 0 0 #007ab1");
                    $('#btnForms').css("background", "none repeat scroll 0 0 #007ab1");
                    $('#btnLetter').css("background", "none repeat scroll 0 0 #007ab1");
                    //$('#btnLetter').css("background", "#007ab1");
                    $('#btnProgress').css("background", "none repeat scroll 0 0 #007ab1");
                    $('#btnCallLogs').css("background", "none repeat scroll 0 0 #007ab1");

                    $('#btnFaceSheet').hover(function () {
                        $(this).css({ "background-color": "#3dc153" });
                    }, function () {
                        $(this).css({ "background-color": "#007ab1" });
                    });
                    $('#btnFaceSheet').css("background", "#007ab1");
                }
                if (elmId == "btnPlacement") {
                    $('#disch').show();
                    $('#content').load('../Placement/Placement/');
                    $('#calender').css("display", "none");
                    $('.imgcontainer').css("display", "block");
                    $('.imgcontainer').load('../Contact/ImageUploadPanel?edit=0');

                   
                        $('#dvHeader').html("Placements")
                   

                    $('#btnPlacement').css("background", "none repeat scroll 0 0 #23a7e3");


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

                    $('#btnProtocol').hover(function () {
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





                    $('#btnGeneral').css("background", "none repeat scroll 0 0 #007ab1");
                    $('#btnProtocol').css("background", "none repeat scroll 0 0 #007ab1");
                    $('#btnMedical').css("background", "none repeat scroll 0 0 #007ab1");

                    $('#btnContact').css("background", "none repeat scroll 0 0 #007ab1");
                    $('#btnVisitation').css("background", "none repeat scroll 0 0 #007ab1");
                    $('#btnEventLogs').css("background", "none repeat scroll 0 0 #007ab1");
                    $('#btnForms').css("background", "none repeat scroll 0 0 #007ab1");
                    $('#btnLetter').css("background", "none repeat scroll 0 0 #007ab1");
                    //$('#btnLetter').css("background", "#007ab1");
                    $('#btnProgress').css("background", "none repeat scroll 0 0 #007ab1");
                    $('#btnCallLogs').css("background", "none repeat scroll 0 0 #007ab1");

                    $('#btnFaceSheet').hover(function () {
                        $(this).css({ "background-color": "#3dc153" });
                    }, function () {
                        $(this).css({ "background-color": "#007ab1" });
                    });
                    $('#btnFaceSheet').css("background", "#007ab1");
                }
                if (elmId == "btnContact") {
                    $('#disch').hide();
                    $('#content').load('../Contact/ListContactVendor/');
                    $('#calender').css("display", "none");
                    $('.imgcontainer').css("display", "block");
                    $('.imgcontainer').load('../Contact/ImageUploadPanel?edit=0');

                   
                        $('#dvHeader').html("Contact / Vendor")
                    
                    // $('#content').load('/Contact/fillContactDetails/');

                    $('#btnContact').css("background", "none repeat scroll 0 0 #23a7e3");

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

                    $('#btnProtocol').hover(function () {
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

                    $('#btnGeneral').css("background", "none repeat scroll 0 0 #007ab1");
                    $('#btnProtocol').css("background", "none repeat scroll 0 0 #007ab1");
                    $('#btnMedical').css("background", "none repeat scroll 0 0 #007ab1");
                    $('#btnPlacement').css("background", "none repeat scroll 0 0 #007ab1");

                    $('#btnVisitation').css("background", "none repeat scroll 0 0 #007ab1");
                    $('#btnEventLogs').css("background", "none repeat scroll 0 0 #007ab1");
                    $('#btnForms').css("background", "none repeat scroll 0 0 #007ab1");
                    //$('#btnLetter').css("background", "none repeat scroll 0 0 #007ab1");
                    $('#btnLetter').css("background", "#007ab1");
                    $('#btnProgress').css("background", "none repeat scroll 0 0 #007ab1");
                    $('#btnCallLogs').css("background", "none repeat scroll 0 0 #007ab1");

                    $('#btnFaceSheet').hover(function () {
                        $(this).css({ "background-color": "#3dc153" });
                    }, function () {
                        $(this).css({ "background-color": "#007ab1" });
                    });
                    $('#btnFaceSheet').css("background", "#007ab1");

                }
                if (elmId == "btnVisitation") {
                    $('#disch').hide();
                    $('#content').load('../Visitation/Visitation/');
                    $('#calender').css("display", "none");
                    $('.imgcontainer').css("display", "block");
                    $('.imgcontainer').load('../Contact/ImageUploadPanel?edit=0');

                    
                        $('#dvHeader').html("Visitation / Trip")
                    

                    $('#btnVisitation').css("background", "none repeat scroll 0 0 #23a7e3");

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

                    $('#btnProtocol').hover(function () {
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



                    $('#btnGeneral').css("background", "none repeat scroll 0 0 #007ab1");
                    $('#btnProtocol').css("background", "none repeat scroll 0 0 #007ab1");
                    $('#btnMedical').css("background", "none repeat scroll 0 0 #007ab1");
                    $('#btnPlacement').css("background", "none repeat scroll 0 0 #007ab1");
                    $('#btnContact').css("background", "none repeat scroll 0 0 #007ab1");

                    $('#btnEventLogs').css("background", "none repeat scroll 0 0 #007ab1");
                    $('#btnForms').css("background", "none repeat scroll 0 0 #007ab1");
                    $('#btnLetter').css("background", "none repeat scroll 0 0 #007ab1");
                    //$('#btnLetter').css("background", "#007ab1");
                    $('#btnProgress').css("background", "none repeat scroll 0 0 #007ab1");
                    $('#btnCallLogs').css("background", "none repeat scroll 0 0 #007ab1");

                    $('#btnFaceSheet').hover(function () {
                        $(this).css({ "background-color": "#3dc153" });
                    }, function () {
                        $(this).css({ "background-color": "#007ab1" });
                    });
                    $('#btnFaceSheet').css("background", "#007ab1");
                }
                if (elmId == "btnEventLogs") {
                    $('#disch').hide();
                    $('#content').load('../Event/EventsList/');
                    $('#calender').css("display", "none");
                    $('.imgcontainer').css("display", "block");
                    $('.imgcontainer').load('../Contact/ImageUploadPanel?edit=0');

                    
                        $('#dvHeader').html("Event Logs");
                    

                    $('#btnEventLogs').css("background", "none repeat scroll 0 0 #23a7e3");

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

                    $('#btnProtocol').hover(function () {
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


                    $('#btnGeneral').css("background", "none repeat scroll 0 0 #007ab1");
                    $('#btnProtocol').css("background", "none repeat scroll 0 0 #007ab1");
                    $('#btnMedical').css("background", "none repeat scroll 0 0 #007ab1");
                    $('#btnPlacement').css("background", "none repeat scroll 0 0 #007ab1");
                    $('#btnContact').css("background", "none repeat scroll 0 0 #007ab1");
                    $('#btnVisitation').css("background", "none repeat scroll 0 0 #007ab1");

                    $('#btnForms').css("background", "none repeat scroll 0 0 #007ab1");
                    //$('#btnLetter').css("background", "none repeat scroll 0 0 #007ab1");
                    $('#btnLetter').css("background", "#007ab1");
                    $('#btnProgress').css("background", "none repeat scroll 0 0 #007ab1");
                    $('#btnCallLogs').css("background", "none repeat scroll 0 0 #007ab1");

                    $('#btnFaceSheet').hover(function () {
                        $(this).css({ "background-color": "#3dc153" });
                    }, function () {
                        $(this).css({ "background-color": "#007ab1" });
                    });
                    $('#btnFaceSheet').css("background", "#007ab1");
                }
                if (elmId == "btnForms") {
                    $('#disch').hide();
                    $('#content').load('../Forms/ListDocuments/');
                    $('#calender').css("display", "none");
                    $('.imgcontainer').css("display", "block");
                    $('.imgcontainer').load('../Contact/ImageUploadPanel?edit=0');

                    
                        $('#dvHeader').html("Documents");
                    

                    $('#btnForms').css("background", "none repeat scroll 0 0 #23a7e3");

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

                    $('#btnProtocol').hover(function () {
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

                    $('#btnFaceSheet').hover(function () {
                        $(this).css({ "background-color": "#3dc153" });
                    }, function () {
                        $(this).css({ "background-color": "#007ab1" });
                    });
                    $('#btnFaceSheet').css("background", "#007ab1");

                    $('#btnGeneral').css("background", "none repeat scroll 0 0 #007ab1");
                    $('#btnProtocol').css("background", "none repeat scroll 0 0 #007ab1");
                    $('#btnMedical').css("background", "none repeat scroll 0 0 #007ab1");
                    $('#btnPlacement').css("background", "none repeat scroll 0 0 #007ab1");
                    $('#btnContact').css("background", "none repeat scroll 0 0 #007ab1");
                    $('#btnVisitation').css("background", "none repeat scroll 0 0 #007ab1");
                    $('#btnEventLogs').css("background", "none repeat scroll 0 0 #007ab1");

                    //$('#btnLetter').css("background", "none repeat scroll 0 0 #007ab1");
                    $('#btnLetter').css("background", "#007ab1");
                    $('#btnProgress').css("background", "none repeat scroll 0 0 #007ab1");
                    $('#btnCallLogs').css("background", "none repeat scroll 0 0 #007ab1");

                }

                if (elmId == "btnProgress") {
                    $('#disch').hide();
                    <%clsSession ses = (clsSession)Session["UserSessionClient"];
                      if(ses.SchoolId==1 ){%>
                    
                    $('#content').load('../Progress/ProgressRpt/');
                    <% } else{%>
                    
                    $('#content').load('../ProgressReport/Index/');
                <%}%>
                    $('#calender').css("display", "none");
                    $('.imgcontainer').css("display", "block");
                    $('.imgcontainer').load('../Contact/ImageUploadPanel?edit=0');

                    
                        $('#dvHeader').html("Progress Reports");
                    

                    $('#btnProgress').css("background", "none repeat scroll 0 0 #23a7e3");

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

                    $('#btnProtocol').hover(function () {
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

                    $('#btnFaceSheet').hover(function () {
                        $(this).css({ "background-color": "#3dc153" });
                    }, function () {
                        $(this).css({ "background-color": "#007ab1" });
                    });
                    $('#btnFaceSheet').css("background", "#007ab1");


                    $('#btnGeneral').css("background", "none repeat scroll 0 0 #007ab1");
                    $('#btnProtocol').css("background", "none repeat scroll 0 0 #007ab1");
                    $('#btnMedical').css("background", "none repeat scroll 0 0 #007ab1");
                    $('#btnPlacement').css("background", "none repeat scroll 0 0 #007ab1");
                    $('#btnContact').css("background", "none repeat scroll 0 0 #007ab1");
                    $('#btnVisitation').css("background", "none repeat scroll 0 0 #007ab1");
                    $('#btnEventLogs').css("background", "none repeat scroll 0 0 #007ab1");
                    $('#btnForms').css("background", "none repeat scroll 0 0 #007ab1");
                    //$('#btnLetter').css("background", "none repeat scroll 0 0 #007ab1");
                    $('#btnLetter').css("background", "#007ab1");

                    $('#btnCallLogs').css("background", "none repeat scroll 0 0 #007ab1");


                }
                if (elmId == "btnLetter") {
                    $('#disch').hide();
                    $('#content').load('../Letter/AllLetter/');
                    $('#calender').css("display", "none");
                    $('.imgcontainer').css("display", "block");
                    $('.imgcontainer').load('../Contact/ImageUploadPanel?edit=0');
                    //$('.rightContainer').hide();

                    
                        $('#dvHeader').html("Letter Tray")
                    

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

                    $('#btnProtocol').hover(function () {
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

                    $('#btnFaceSheet').hover(function () {
                        $(this).css({ "background-color": "#3dc153" });
                    }, function () {
                        $(this).css({ "background-color": "#007ab1" });
                    });
                    $('#btnFaceSheet').css("background", "#007ab1");

                    $('#btnGeneral').css("background", "#007ab1");
                    $('#btnProtocol').css("background", "#007ab1");
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
                    $('#disch').hide();
                    $('#content').load('../CallLog/CallLog/');
                    $('#calender').css("display", "none");
                    $('.imgcontainer').css("display", "block");
                    $('.imgcontainer').load('../Contact/ImageUploadPanel?edit=0');
                    //$('.rightContainer').hide();

                   
                    $('#dvHeader').html("Family & Agency Communication Log")
                    

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

                    $('#btnProtocol').hover(function () {
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
                    $('#btnProtocol').css("background", "#007ab1");
                    $('#btnMedical').css("background", "#007ab1");
                    $('#btnPlacement').css("background", "#007ab1");
                    $('#btnContact').css("background", "#007ab1");
                    $('#btnVisitation').css("background", "#007ab1");
                    $('#btnEventLogs').css("background", "#007ab1");
                    $('#btnForms').css("background", "#007ab1");
                    $('#btnProgress').css("background", "#007ab1");
                    $('#btnLetter').css("background", "#007ab1");

                    $('#btnFaceSheet').hover(function () {
                        $(this).css({ "background-color": "#3dc153" });
                    }, function () {
                        $(this).css({ "background-color": "#007ab1" });
                    });
                    $('#btnFaceSheet').css("background", "#007ab1");
                    
                }

                if (elmId == "btnFaceSheet") {
                    $('#disch').hide();
                    $('#content').load('../ClientRegistration/ClientRegistration?data=<%= ViewBag.Param%>|Fill');
                    $('.imgcontainer').css("display", "block");
                    $('.imgcontainer').load('../Contact/ImageUploadPanel?edit=1');
                    $('.EditProfile').css("display", "block");
                    $('.EditProfile').val("Edit FaceSheet");
                    $('#calender').css("display", "none");
                    if ($('#tdstudentName').length != 0) {
                        //$('#dvHeader').html("Client Details : " + $('#tdstudentName').html())
                    } else {
                        //$('#dvHeader').html("Client Details").html();
                    }
                    $('#dvHeader').html("FaceSheet");
                    $('#export').css("visibility", "visible");
                    //$('.rightContainer').hide();

                    //if ($('#tdstudentName').length != 0) {
                    //    $('#dvHeader').html("Contact Logs : " + $('#tdstudentName').html())
                    //} else {
                    //    $('#dvHeader').html("Contact Logs : ")
                    //}

                    $('#btnCallLogs').css("background", "#007ab1");

                    $('#btnCallLogs').hover(function () {
                        $(this).css({ "background-color": "#3dc153" });
                    }, function () {
                        $(this).css({ "background-color": "#007ab1" });
                    });

                    $('#btnGeneral').hover(function () {
                        $(this).css({ "background-color": "#3dc153" });
                    }, function () {
                        $(this).css({ "background-color": "#007ab1" });
                    });

                    $('#btnProtocol').hover(function () {
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
                    $('#btnProtocol').css("background", "#007ab1");
                    $('#btnMedical').css("background", "#007ab1");
                    $('#btnPlacement').css("background", "#007ab1");
                    $('#btnContact').css("background", "#007ab1");
                    $('#btnVisitation').css("background", "#007ab1");
                    $('#btnEventLogs').css("background", "#007ab1");
                    $('#btnForms').css("background", "#007ab1");
                    $('#btnProgress').css("background", "#007ab1");
                    $('#btnLetter').css("background", "#007ab1");

                    $('#btnFaceSheet').hover(function () {
                        $(this).css({ "background-color": "#23a7e3" });
                    }, function () {
                        $(this).css({ "background-color": "#23a7e3" });
                    });
                    $('#btnFaceSheet').css("background", "#23a7e3");

                } else {
                    $('#export').css("visibility", "hidden");
                }

                if (elmId == "btnHome") {
                    $('#disch').hide();
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
            <a class="admin" title="<%=ViewBag.Usename %>" href="#"><%=ViewBag.Usename %></a>
            <a class="logout" title="Logout" href="../Client/Logout/">Logout</a>
            <a class="Report" title="Report" href="../Reports/ClientReports.aspx">Reports</a>
            <a class="home" title="Home" href="../Client/ListClients?argument=*&bSort=false">Home</a>
        </div>
        <div class="contentPart">
            <div class="imgcorner">
                <a class="logo" href="#">
                    <img src="../../Documents/images/logo.jpg" width="200" height="40" /></a>
            </div>
            <div class="ContentAreaContainer" style="min-width:1000px">
                <div class="leftContainer">

                    <h2 id="Caption" class="leftMenuHeader">
                        <a class="gray MenuTooltip" title="Client Portal">Client Portal</a>
                    </h2>

                    <%
                        ClientDB.AppFunctions.Other_Functions of = new ClientDB.AppFunctions.Other_Functions();
                         %>

                    <%if (of.setClientPermission("General Client") > 0)
                      { %>
                    <h2 id="btnGeneral" class="leftMenu">
                        <a class="gray MenuTooltip" title="General">General</a>
                    </h2>
                    <%} %>

                    <% var sesobj = (clsSession)Session["UserSessionClient"];%>
                      <% if(sesobj.StudentId>0){ %>

                   
                       <%if (of.setClientPermission("Medical") > 0)
                      { %>
                    <h2 id="btnMedical" class="leftMenu">
                        <a class="gray MenuTooltip" title="Medical">Medical</a>
                    </h2>
                    <%} %>

                     <%if (of.setClientPermission("Placement") > 0)
                      { %>
                    <h2 id="btnPlacement" class="leftMenu">
                        <a class="gray MenuTooltip" title="Placement">Placement</a>
                    </h2>
                    <%} %>

                     <%if (of.setClientPermission("Contact/Vendor") > 0)
                      { %>
                    <h2 id="btnContact" class="leftMenu">
                        <a class="gray MenuTooltip" title="Contact / Vendor">Contact / Vendor</a>
                    </h2>
                    <%} %>

                    <%--<h2 id="btnVisitation" class="leftMenu">
                        <a class="gray MenuTooltip" title="Visitation / Trip">Visitation / Trip</a>
                    </h2>--%>

                     <%if (of.setClientPermission("Event Logs") > 0)
                      { %>
                    <h2 id="btnEventLogs" class="leftMenu">
                        <a class="gray MenuTooltip" title="Event Logs">Event Logs</a>

                    </h2>
                    <%} %>

                     <%if (of.setClientPermission("Document Tray") > 0)
                      { %>
                    <h2 id="btnForms" class="leftMenu">
                        <a class="gray MenuTooltip" title="Forms">Document Tray</a>
                    </h2>
                    <%} %>

                     <%if (of.setClientPermission("Letter Tray") > 0)
                      { %>
                    <h2 id="btnLetter" class="leftMenu">
                        <a class="gray MenuTooltip" title="Letter Tray">Letter Tray</a>
                    </h2>
                    <%} %>

                     <%if (of.setClientPermission("Family & Agency Communication Log") > 0)
                      { %>
                    <h2 id="btnCallLogs" class="leftMenu">
                        <a class="gray MenuTooltip" title="Family & Agency Communication Log">Family & Agency Comm Log</a>
                    </h2>
                    <%} %>

                  <%--    <%if (of.setClientPermission("Progress Report") > 0)
                      { %>
                    <h2 id="btnProgress" class="leftMenu">
                        <a class="gray MenuTooltip" title="Progress Report">Progress Report</a>
                    </h2>
                    <%} %> --%>

                    
                        <%if(sesobj.SchoolId==2){ 
                              if (of.setClientPermission("Protocol Summary") > 0)
                      { %>
                         <%--<h2 id="btnProtocol" class="leftMenu">
                            <a class="gray MenuTooltip" title="Protocol Summary">Protocol Summary</a>
                        </h2>--%>
                      
                        <%}}
                          else if (sesobj.SchoolId == 1)
                          {
                              if (of.setClientPermission("Facesheet") > 0)
                              { 
                              %>
                            <h2 id="btnFaceSheet" class="leftMenu">
                                <a class="gray MenuTooltip" title="Facesheet">FaceSheet</a>
                            </h2>
                        <%}
                          } %>
                    <%} %>
                   
                </div>


                <div style="float: left; width: 81%; border-right: 3px solid rgb(229, 229, 229); margin-top: 4px;">
                     <div id="staticTab" style="background-color: rgb(241, 241, 241); font-size: 20px;cursor:pointer; height: 31px; color: rgb(0, 0, 0); font-weight: bold; width: 100%; padding: 8px 0px 0px 4px;" onclick="javascript: $('.staticClientDetials_wrapper').slideToggle();"></div>
                    <div class="middleContainer">
                        <div class="staticClientDetials_wrapper" style="font-size: 12px; background-color: #F1F1F1; position: inherit; width: 95%; z-index:1000;">
                            <%--<div class="refreshButton" style="float:right;margin-right:-35px;cursor:pointer;" onclick="loadClientStaticDetails();">
                                <img src="../../Documents/images/Refresh.png" />
                            </div>--%>
                         <div class="staticClientDetials"></div>
                            </div>
                        <a href="../ClientRegistration/ExportAllData/" class="styleA" id="export" target="_blank" style="float:right;border:none;width:43px; height:43px;font-weight:bold;font-size:small;visibility:hidden;background-image:url('../images/ExportWord.png');background-repeat:no-repeat;background-position:top left;">
                            <%--<img src="../images/ExportWord.png" width="43px" height="43px" style="display: block;float: left;margin: 0 15px 0 0;border: none;cursor: pointer; background-repeat:no-repeat;background-position:top left"/>--%>
                        </a>
                          <div id="dvHeader" style="background-color:#F9F9F9; border-bottom :1px solid #3399ff; font-size: 20px; height: 31px; color: rgb(0, 0, 0); font-weight: bold; width: 100%; padding: 8px 0px 0px 4px;">
                        Client Details
                    </div>
                        <div id="content">
                        </div>

                    </div>



                    <div class="rightContainer">
                        <div class="imgcontainer">
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
            <img src="../../Documents/images/smllogo.JPG" width="109" height="18" />
            <div class="copyright">&copy; Copyright 2015, Melmark, Inc. All rights reserved.</div>
        </div>


        <div class="clear"></div>
    </div>

</asp:Content>
