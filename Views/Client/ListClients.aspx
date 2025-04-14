<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<IEnumerable<ClientDB.Models.ClientModel>>" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <meta name="viewport" content="width=device-width" />
    <script src="../../Documents/JS/jquery-1.8.0.min.js"></script>
    <script src="../../Documents/JS/slides.min.jquery.js"></script>
    <link href="../../Documents/CSS/style.css" rel="stylesheet" />
    <title><%=Session["PageName"].ToString() %></title>
    <style type="text/css">
        .Main {
            height: auto;
            width: 100%;
        }

        .dataList {
            background-color: #f1f1f1;
            border: 1px solid #cccccc;
            border-radius: 6px;
            float: left;
            height: auto;
            padding: 5px;
            width: 100% !important;
            cursor: pointer;
        }

            .dataList:hover {
                background-color: #cbc9c9;
            }

        .tdImage {
            width: 115px;
        }

        .tdLabel {
            width: 90px;
        }

        .auto-style1 {
            width: 100%;
        }

        .noItem {
            border: 2px double gray;
            display: block;
            float: left;
            height: 110px;
            margin-left: 350px;
            margin-top: 130px;
            padding-left: 100px;
            padding-top: 100px;
            width: 220px;
            display: none;
        }

        .lblSpan {
        font-style: italic;
        font-weight: bold;
    }
    </style>
    <script type="text/javascript">

        $(document).ready(function () {
            var name = '<%= ViewBag.sessname %>';
            // alert(name);
            var searchStatus = '<%= ViewBag.SearchByStatus%>';
            var sortStatus = '<%= ViewBag.OrderByStatus%>';
            var activeClinentstatus = '<%= ViewBag.activeClientstatus%>';
        });
        function sortClients() {

            var x = document.getElementById("txtSearch");
            var y = "";
            var radioValue = $("input[name='sortOrder']:checked").val();
            var radioValue2 = $("input[name='searchBy']:checked").val();
            var radioValue3 = $("input[name='activeClient1']:checked").val();
           
            if (radioValue == 1) {
                radioValue = true;
            }
            else if (radioValue == 2) {
                radioValue = false;
            }
            if (radioValue2 == 1) {
                radioValue2 = true;
            }
            else if (radioValue2 == 2) {
                radioValue2 = false;
            }
            if (radioValue3 == 1) {
                radioValue3 = 1;
            }
            else if (radioValue3 == 2) {
                radioValue3 = 2;
            }
            else if (radioValue3 == 4) {
                radioValue3 = 4;
            }

            var drpSort = document.getElementById('ddlSort');
            var sortArgmnt = drpSort.options[drpSort.selectedIndex].text;

            if (sortArgmnt == "-----Select-----") {
                sortArgmnt = "";
            }

            if ($('#ddlSort option:selected').attr('value') != "0") {
                $("#hdnSort").val($('#ddlSort option:selected').attr('value'));
                var SearchArgs = "";
                SearchArgs += $('#hdnSort').val() + "_";
                if (sortArgmnt == "Date Of Birth") sortArgmnt = "DateOfBirth";
                if (sortArgmnt == "Admission Date") sortArgmnt = "AdmissionDate";
                if (sortArgmnt == "Client ID") sortArgmnt = "ClientID";
                if (sortArgmnt == "First Name") sortArgmnt = "FirstName";
                if (sortArgmnt == "Last Name") sortArgmnt = "LastName";

                if (x != null) {
                    //alert("1. sort!=null search!=null");
                    y = encodeURI(x.value) + "$" + sortArgmnt;
                    if (y == "$") {
                        y = "";
                    }
                    window.location.href = "../Client/ListClients?Name=" + encodeURI(SearchArgs) + "&argument=" + y + "&bSort=true" + "&orderBy=" + radioValue + "&searchBy=" + radioValue2 + "&activeClient=" + radioValue3;
                } else {
                    //alert("2. sort!=null search==null");
                    window.location.href = "../Client/ListClients?Name=" + encodeURI(SearchArgs) + "&argument=" + sortArgmnt + "&bSort=true" + "&orderBy=" + radioValue + "&searchBy=" + radioValue2 + "&activeClient=" + radioValue3;                   
                }

            } else {
                if (x != "") {
                    //alert("3. sort==null search!=null");
                    window.location.href = "../Client/ListClients?Name=" + encodeURI(SearchArgs) + "&argument=" + encodeURI(x.value) + "&bSort=false" + "&orderBy=" + radioValue + "&searchBy=" + radioValue2 + "&activeClient=" + radioValue3;
                } else {
                    //alert("4. sort==null search==null");
                    window.location.href = "../Client/ListClients?Name=" + encodeURI(SearchArgs) + "&argument=&bSort=false" + "&orderBy=" + radioValue + "&searchBy=" + radioValue2 + "&activeClient=" + radioValue3;
                }
            }
        }


        var currentValue = 0;
        function handleClick(myRadio) {
            //alert('Old value: ' + currentValue);
            //alert('New value: ' + myRadio.value);
            currentValue = myRadio.value;
            window.location.href = "../Client/ListClients?Name=" + encodeURI(SearchArgs) + "&argument=" + sortArgmnt + "&bSort=true";
        }


        function searchButtonNew() {
            var x = document.getElementById("txtSearch");
            var y = "";
            var radioValue = $("input[name='sortOrder']:checked").val();
            var radioValue2 = $("input[name='searchBy']:checked").val();
            var radioValue3 = $("input[name='activeClient1']:checked").val();
            if (radioValue == 1) {
                radioValue = true;
            }
            else if (radioValue == 2) {
                radioValue = false;
            }
            if (radioValue2 == 1) {
                radioValue2 = true;
            }
            else if (radioValue2 == 2) {
                radioValue2 = false;
            }
            //if (radioValue3 == 1) {
            //    radioValue3 = 1;
            //}
            //else if (radioValue3 == 2) {
            //    radioValue3 = 2;
            //}
            //else if (radioValue3 == 4) {
            //    radioValue3 = 4;
            //}

            radioValue3 = 0;
            $("input[name='activeClient1'][value='3']").prop("checked", true);

            var drpSort = document.getElementById('ddlSort');
            var sortArgmnt = drpSort.options[drpSort.selectedIndex].text;

            if (sortArgmnt == "-----Select-----") {
                sortArgmnt = "";
            }

            if ($('#ddlSort option:selected').attr('value') != "0") {
                $("#hdnSort").val($('#ddlSort option:selected').attr('value'));
                var SearchArgs = "";
                SearchArgs += $('#hdnSort').val() + "_";
                if (sortArgmnt == "Date Of Birth") sortArgmnt = "DateOfBirth";
                if (sortArgmnt == "Admission Date") sortArgmnt = "AdmissionDate";
                if (sortArgmnt == "Client ID") sortArgmnt = "ClientID";
                if (sortArgmnt == "First Name") sortArgmnt = "FirstName";
                if (sortArgmnt == "Last Name") sortArgmnt = "LastName";

                if (x == "") {
                    window.location.href = "../Client/ListClients?Name=" + encodeURI(SearchArgs) + "&argument=" + sortArgmnt + "&bSort=true" + "&orderBy=" + radioValue + "&searchBy=" + radioValue2 + "&activeClient=" + radioValue3;
                } else {
                    y = encodeURI(x.value) + "$" + sortArgmnt;
                    if (y == "$") {
                        y = "";
                    }
                    window.location.href = "../Client/ListClients?Name=" + encodeURI(SearchArgs) + "&argument=" + y + "&bSort=true" + "&orderBy=" + radioValue + "&searchBy=" + radioValue2 + "&activeClient=" + radioValue3;
                }

            } else {
                if (x != "") {
                    window.location.href = "../Client/ListClients?&argument=" + encodeURI(x.value) + "&bSort=false" + "&orderBy=" + radioValue + "&searchBy=" + radioValue2 + "&activeClient=" + radioValue3;
                } else {
                    window.location.href = "../Client/ListClients?&argument=&bSort=false" + "&orderBy=" + radioValue + "&searchBy=" + radioValue2 + "&activeClient=" + radioValue3;
                }
            }
        }

        function search(evt) {
            //OC
            //var x = document.getElementById("txtSearch");
            //window.location.href = "../Client/ListClients?argument=" + encodeURI(x.value) + "&bSort=false";
            if (evt.keyCode == 13) {
                var x = document.getElementById("txtSearch");
                var y = "";
                var radioValue = $("input[name='sortOrder']:checked").val();
                var radioValue2 = $("input[name='searchBy']:checked").val();
                var radioValue3 = $("input[name='activeClient1']:checked").val();
                if (radioValue == 1) {
                    radioValue = true;
                }
                else if (radioValue == 2) {
                    radioValue = false;
                }
                if (radioValue2 == 1) {
                    radioValue2 = true;
                }
                else if (radioValue2 == 2) {
                    radioValue2 = false;
                }
                if (radioValue3 == 1) {
                    radioValue3 = 1;
                }
                else if (radioValue3 == 2) {
                    radioValue3 = 2;
                }
                else if (radioValue3 == 4) {
                    radioValue3 = 4;
                }

                var drpSort = document.getElementById('ddlSort');
                var sortArgmnt = drpSort.options[drpSort.selectedIndex].text;

                if (sortArgmnt == "-----Select-----") {
                    sortArgmnt = "";
                }

                if ($('#ddlSort option:selected').attr('value') != "0") {
                    $("#hdnSort").val($('#ddlSort option:selected').attr('value'));
                    var SearchArgs = "";
                    SearchArgs += $('#hdnSort').val() + "_";
                    if (sortArgmnt == "Date Of Birth") sortArgmnt = "DateOfBirth";
                    if (sortArgmnt == "Admission Date") sortArgmnt = "AdmissionDate";
                    if (sortArgmnt == "Client ID") sortArgmnt = "ClientID";
                    if (sortArgmnt == "First Name") sortArgmnt = "FirstName";
                    if (sortArgmnt == "Last Name") sortArgmnt = "LastName";

                    if (x == "") {
                        //alert("5. sort!=null search==null");
                        window.location.href = "../Client/ListClients?Name=" + encodeURI(SearchArgs) + "&argument=" + sortArgmnt + "&bSort=true" + "&orderBy=" + radioValue + "&searchBy=" + radioValue2 + "&activeClient=" + radioValue3;
                    } else {
                        //alert("6. sort!=null search!=null");
                        y = encodeURI(x.value) + "$" + sortArgmnt;
                        if (y == "$") {
                            y = "";
                        }
                        window.location.href = "../Client/ListClients?Name=" + encodeURI(SearchArgs) + "&argument=" + y + "&bSort=true" + "&orderBy=" + radioValue + "&searchBy=" + radioValue2 + "&activeClient=" + radioValue3;
                    }

                } else {
                    if (x != "") {
                        //alert("7. sort==null search!=null");
                        window.location.href = "../Client/ListClients?&argument=" + encodeURI(x.value) + "&bSort=false" + "&orderBy=" + radioValue + "&searchBy=" + radioValue2 + "&activeClient=" + radioValue3;
                    } else {
                        //alert("8. sort==null search==null");
                        window.location.href = "../Client/ListClients?&argument=&bSort=false" + "&orderBy=" + radioValue + "&searchBy=" + radioValue2 + "&activeClient=" + radioValue3;
                    }
                }

            }
        }
        

        function loadData(id) {
            var school = parseInt(<%: ViewBag.schoolId %>);
            <% var sesobj = (clsSession)Session["UserSessionClient"]; %>
           <% sesobj.StudentId = 0; %>
            if (id == -1) {
                $('#txtSearch').val("");
                window.location.href = "../Client/ListClients?argument=*&bSort=false";
            }
            else {
                if (school == 1) {
                    window.location.href = "../ClientRegistrationPA/index?Param=" + id;
                }
                if (school == 2) {
                    window.location.href = "../ClientRegistrationPA/index?Param=" + id;
                }
                //$('.imgcontainer').css("display", "block");
                //$('.imgcontainer').load('../Contact/ImageUploadPanel?edit=1');
                //$('.EditProfile').css("display", "block");
            }

        }




    </script>
    <style type="text/css">
        .slides_control {
            height: 30px !important;
            left: -95px !important;
            position: relative;
            width: 104px !important;
        }

        .slides_container {
            width: -1%;
            min-width: 100px;
            display: none;
            font-size: 10px;
            font-family: Arial, Helvetica, sans-serif;
            text-decoration: none;
            float: left;
            padding: 0 0 0 53%;
            margin: 0 0 0 4%;
        }

            .slides_container div.slide {
                width: 101px;
                height: 25px;
                float: left;
                margin: 5px 0 0 -10px;
                display: block;
            }

        .item {
            float: left;
            height: 15px;
            margin: 0 5px;
            width: 15px;
        }

            .item a, .item a:link, .item a:visited {
                background: none repeat scroll 0 0 #008000;
                border-radius: 2px;
                color: #ffffff;
                display: block;
                font-size: 13px;
                font-weight: bold;
                height: 18px;
                padding-top: 3px;
                text-align: center;
                text-decoration: none;
                width: 20px;
            }

                .item a:hover {
                    background: none repeat scroll 0 0 #0053a1;
                    color: #fff;
                }

        .pagination {
            list-style: none;
            margin: 0;
            padding: 0;
            font-size: 1px;
            display: none;
        }

            .pagination .current a {
                color: red;
                font-size: 1px;
            }

        .next {
            width: 7px;
            height: 11px;
            font-size: 1px;
            color: #3375b4;
            float: left;
            top: 15px;
            display: block;
            position: absolute;
            z-index: 999;
            top: 180px;
            background: url("../Documents/Images/arro.png") left top no-repeat;
            padding: 4px 0 0 1px;
            margin: 12px 0 0 0;
        }

            .next:hover {
                background-position: 0 -15px;
                color: #a8a8a8;
            }

        .prev {
            width: 7px;
            height: 11px;
            font-size: 1px;
            color: #3375b4;
            float: right;
            top: 15px;
            display: block;
            position: absolute;
            z-index: 999;
            top: 180px;
            background: url("../Documents/Images/arro.png")-8px top no-repeat;
            padding: 4px 0 0 1px;
            margin: 12px 0 0 123px;
        }

            .prev:hover {
                background-position: 0 -15px;
                background: url("../Documents/Images/arro.png")-8px -15px no-repeat;
                color: #a8a8a8;
            }


        #butprv {
            float: left;
            font-weight: bold;
            cursor: pointer;
            border-width: 1px;
            /*text-decoration: underline;*/
        }

        #butnext {
            float: right;
            font-weight: bold;
            cursor: pointer;
            border-width: 1px;
            /*text-decoration: underline;*/
        }
    </style>
</head>
<body>
    <div class="mainContainer">
        <div class="topHead">
            <a class="admin" title="<%=ViewBag.Usename %>" href="#"><%=ViewBag.Usename %> </a>
            <a class="logout" title="Logout" href="../Client/Logout/">Logout</a>
            <a class="home" href="../../../(S(<%=Session.SessionID %>))/LoginContinue.aspx" title="StartUp Page" ">
                                 Landing Portal</a>

            <a class="Report" title="Reports" href="../Reports/ClientReports.aspx">Reports</a>
            <a class="home" title="Home" href="../Client/Index">Home</a>

        </div>
        <div class="contentPart">
            <div class="imgcorner nobddr">
                <a class="logo" style="margin: 11px 0 0 17px !Important;" href="#">
                    <img src="../../Documents/images/logo.jpg" width="200" height="40" /></a>
            </div>
            <div class="ContentAreaContainer nobg">


                <div class="middleContainer edited" style="border-left: none;">
                    <h2>Client Dashboard</h2>
                    <div id="content">
                        <div class="Main MiddleContainerStyle">
                            <div style="width: 90%; margin: 0 auto">
                                <table>
                                    <tr>
                                        <td class="nobdr" style="padding: 10px; width: 5%">
                                            <div class="topbarCo">
                                                <div class="selContainer">
                                                    <div id="slides">
                                                        <div class="slides_container">

                                                            <div class="slide">
                                                                <div class="item"><a href="../Client/ListClients?argument=A&bSort=false">A</a></div>
                                                                <div class="item"><a href="../Client/ListClients?argument=B&bSort=false">B</a></div>
                                                                <div class="item"><a href="../Client/ListClients?argument=C&bSort=false">C</a></div>
                                                                <div class="item"><a href="../Client/ListClients?argument=D&bSort=false">D</a></div>
                                                            </div>
                                                            <div class="slide">

                                                                <div class="item"><a href="../Client/ListClients?argument=E&bSort=false">E</a></div>
                                                                <div class="item"><a href="../Client/ListClients?argument=F&bSort=false">F</a></div>
                                                                <div class="item"><a href="../Client/ListClients?argument=G&bSort=false">G</a></div>
                                                                <div class="item"><a href="../Client/ListClients?argument=H&bSort=false">H</a></div>
                                                            </div>
                                                            <div class="slide">
                                                                <div class="item"><a href="../Client/ListClients?argument=I&bSort=false">I</a></div>
                                                                <div class="item"><a href="../Client/ListClients?argument=J&bSort=false">J</a></div>
                                                                <div class="item"><a href="../Client/ListClients?argument=K&bSort=false">K</a></div>
                                                                <div class="item"><a href="../Client/ListClients?argument=L&bSort=false">L</a></div>
                                                            </div>
                                                            <div class="slide">
                                                                <div class="item"><a href="../Client/ListClients?argument=M&bSort=false">M</a></div>
                                                                <div class="item"><a href="../Client/ListClients?argument=N&bSort=false">N</a></div>
                                                                <div class="item"><a href="../Client/ListClients?argument=O&bSort=false">O</a></div>
                                                                <div class="item"><a href="../Client/ListClients?argument=P&bSort=false">P</a></div>
                                                            </div>
                                                            <div class="slide">
                                                                <div class="item"><a href="../Client/ListClients?argument=Q&bSort=false">Q</a></div>
                                                                <div class="item"><a href="../Client/ListClients?argument=R&bSort=false">R</a></div>
                                                                <div class="item"><a href="../Client/ListClients?argument=S&bSort=false">S</a></div>
                                                                <div class="item"><a href="../Client/ListClients?argument=T&bSort=false">T</a></div>
                                                            </div>
                                                            <div class="slide">
                                                                <div class="item"><a href="../Client/ListClients?argument=U&bSort=false">U</a></div>
                                                                <div class="item"><a href="../Client/ListClients?argument=V&bSort=false">V</a></div>
                                                                <div class="item"><a href="../Client/ListClients?argument=W&bSort=false">W</a></div>
                                                                <div class="item"><a href="../Client/ListClients?argument=X&bSort=false">X</a></div>
                                                            </div>
                                                            <div class="slide">
                                                                <div class="item"><a href="../Client/ListClients?argument=Y&bSort=false">Y</a></div>
                                                                <div class="item"><a href="../Client/ListClients?argument=Z&bSort=false">Z</a></div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>



                                            </div>
                                        </td>


                                        <td style="width:18%;">
                                            <span class="lblSpan">Search</span><br />
                                        <%--<td class="nobdr" style="padding: 0; text-align: left; width: 10%">--%>
                                            <%if(ViewBag.SearchByStatus==false)
                                              { %>
                                            <input type="radio" name="searchBy" value="1" />Name
                                            <input type="radio" name="searchBy" value="2" checked="checked" />Client ID
                                            <%} else {%>
                                            <input type="radio" name="searchBy" value="1" checked="checked" />Name
                                            <input type="radio" name="searchBy" value="2" />Client ID
                                            <%} %>
                                            <br />
                                            <input type="text" id="txtSearch" onkeyup="search(event)" style="width: 130px" >
                                            <input type="button" id="btnSearchNew" style="float:none;" value="Search" onclick="searchButtonNew()" />
                                        </td>
                                         <td style="width:15%;">
                                            <span class="lblSpan">Search By</span><br />
                                            <%if (ViewBag.activeClientstatus == 1)
                                              { %>
                                            <input type="radio" name="activeClient1" value="1" onchange="sortClients()"  checked="checked" />Active
                                             <%--<input type="radio" name="activeClient1" value="2" onchange="sortClients()"  />Inactive--%>
                                            <input type="radio" name="activeClient1" value="4" onchange="sortClients()"  />Discharge
                                              <input type="radio" name="activeClient1" value="3" onchange="sortClients()"/>All
                                            
                                            <%--<%}
                                              else if (ViewBag.activeClientstatus == 2)
                                              {%>
                                            <input type="radio" name="activeClient1" value="1" onchange="sortClients()"  />Active
                                             <%--<input type="radio" name="activeClient1" value="2" onchange="sortClients()" checked="checked" />Inactive--%>
                                            <%--<input type="radio" name="activeClient1" value="4" onchange="sortClients()"  />Discharge
                                              <input type="radio" name="activeClient1" value="3" onchange="sortClients()"/>All--%>
                                    
                                            
                                             <%}
                                               else if (ViewBag.activeClientstatus == 4)
                                              {%>
                                            <input type="radio" name="activeClient1" value="1" onchange="sortClients()"  />Active
                                             <%--<input type="radio" name="activeClient1" value="2" onchange="sortClients()"  />Inactive--%>
                                            <input type="radio" name="activeClient1" value="4" onchange="sortClients()" checked="checked" />Discharge
                                              <input type="radio" name="activeClient1" value="3" onchange="sortClients()"/>All
                                    
                                            
                                             <%}
                                              else
                                              {%>
                                             <input type="radio" name="activeClient1" value="1" onchange="sortClients()"/>Active
                                             <%--<input type="radio" name="activeClient1" value="2" onchange="sortClients()"  />Inactive--%>
                                            <input type="radio" name="activeClient1" value="4" onchange="sortClients()" />Discharge
                                              <input type="radio" name="activeClient1" value="3" onchange="sortClients()"  checked="checked"/>All
                                                <%} %>       
                                             </td>
                                        <td style="width:10%;">
                                            <span class="lblSpan">Sort By</span><br />
                                            <%if(ViewBag.OrderByStatus==false)
                                              { %>
                                            <input type="radio" name="sortOrder" value="1" onchange="sortClients()" />Ascending
                                            <input type="radio" name="sortOrder" value="2" onchange="sortClients()" checked="checked" />Descending
                                            <%} else {%>
                                            <input type="radio" name="sortOrder" value="1" onchange="sortClients()" checked="checked" />Ascending
                                            <input type="radio" name="sortOrder" value="2" onchange="sortClients()" />Descending
                                            <%} %>
                                            <br />
                                        <%--td class="nobdr" style="text-align: left; padding: 0">--%>
                                            <%--<select id="ddlSort" name="ddlSort" style="width: 165px" onchange="sortClients()">
                                                <option>-----Select-----</option>
                                                <option>Admission Date</option>
                                                <option>Age</option>
                                                <option>Date Of Birth</option>
                                            </select>--%>
                                            <input type="hidden" id="hdnSort" />
                                            <%string ddlSort = ViewBag.Sort;
                                              var SelectList = new List<SelectListItem>
                                              {
                                                  new SelectListItem{Value="0",Text="-----Select-----"},
                                                  new SelectListItem{Value="1",Text="Admission Date"},
                                                  new SelectListItem{Value="2",Text="Date of Birth"},
                                                  new SelectListItem{Value="3",Text="Client ID"},
                                                  new SelectListItem{Value="4",Text="First Name"},
                                                  new SelectListItem{Value="5",Text="Last Name"}
                                              };
                                                %>
                                            <%=Html.DropDownListFor(x => ddlSort, SelectList.SelectValue(ddlSort), new { id = "ddlSort", onchange = "sortClients()" }) %>

                                        <%--</td>
                                        <td style="width:100%">--%>
                                        </td>
                                        

                                        <td class="nobdr" style="width: 1%; padding-left: 45px; padding-bottom: 3px;" title="View all Clients">
                                            <img src="../../Documents/images/ViewAll.png" style="cursor: pointer;" alt="View All" onclick="loadData(-1)" />

                                        </td>
                                        <td class="nobdr" style="width: 1%; padding-left: 45px; padding-bottom: 3px;" title="New Registration">
                                            <% if(ViewBag.permission ==  "true"){ %>
                                            <img src="../../Documents/images/NewReg.png" style="cursor: pointer;" alt="New Registration" onclick="loadData(0)" />
                                            <%} %>

                                            <%-- <input id="btnNewRegistration" name="prv" style="width: 120px;"
                                                type="button" value="New Registration" onclick="loadData(0)" />--%>
                                        </td>
                                    </tr>
                                </table>


                            </div>
                            <div class="clear"></div>
                            <hr style="color: #f1f1f1;" />
                            <div class="centerpartCon">
                                <div id="noMatch" class="noItem">
                                    No Match found..!
                                </div>
                                <%var count = 0;
                            
                                %>

                                <%foreach (var item in Model)
                                  {
                  
                  
                                %>

                                <div style="float: left; margin: 3px; width: 355px">

                                    <table class="dataList boxcontainer" id="<%=item.Id %>" onclick="javascript: loadData(this.id)">

                                        <tr>

                                            <td rowspan="4" class="boxstyleCon">
                                                <%
                                      if (item.ImageUrl == null)
                                      {
                                          if (item.Gender == "Male")
                                          {%>
                                                <img src="../../Images/1376318130_elementary_school.png" style="width: 67px; height: 67px;" />
                                                <% } %>
                                                <% else
                                          { %>
                                                <img src="../../Images/1376318118_student_b.png" style="width: 67px; height: 67px;" />
                                                <% }
                                      }
                                      else
                                      { %>
                                                <img src="data:image/gif;base64,<%=item.ImageUrl %>" style="width: 67px; height: 67px;" />
                                                <% }
                                                %>
                                            </td>
                                            <td class="boxstyleCon">Name</td>
                                            <%
                                      string Name = "";
                                      if (item.Name.Length > 30)
                                      {
                                          Name = item.Name.Substring(0, 30) + "...";
                                      }
                                      else
                                      {
                                          Name = item.Name;
                                      } %>
                                            <td class="boxstyleCon"><%= Name %></td>
                                        </tr>
                                        <tr>
                                            <td class="boxstyleCon">Date Of Birth</td>
                                            <td class="boxstyleCon"><%=item.DateOfBirth %></td>
                                        </tr>
                                        <tr>
                                            <td class="boxstyleCon">Age</td>
                                            <td class="boxstyleCon"><%=item.Age %></td>
                                        </tr>
                                        <tr>
                                            <td class="boxstyleCon">Gender</td>
                                            <td class="boxstyleCon"><%=item.Gender %></td>
                                        </tr>
                                    </table>

                                </div>

                                <%
                                  } %>
                            </div>

                            <div style="width: 91%; float: left; margin-left: 38px;">
                                <table style="width: 100%">
                                    <tr>
                                        <td class="nobdr">
                                            <input id="curval" name="curval" value="<%: ViewBag.curval %>" type="hidden" />
                                            <input id="flage" value="<%: ViewBag.flage %>" type="hidden" />
                                            <input id="butprv" style="display: block; cursor: pointer; background: url('../../Documents/images/Prev.png') no-repeat scroll left top transparent; height: 35px; border-radius: 0px;" name="prv" type="button" />
                                            <input id="butnext" name="next" style="display: block; cursor: pointer; background: url('../../Documents/images/Next.png') no-repeat scroll left top transparent; height: 35px; border-radius: 0px;" type="button" />
                                        </td>
                                    </tr>
                                </table>

                            </div>
                        </div>
                    </div>

                </div>


                <div class="clear"></div>
            </div>

            <div class="clear"></div>
        </div>

        <div class="clear"></div>
        <div class="footer">
            <img src="../../Documents/images/smllogo.JPG" width="109" height="30" />
            <div class="copyright">&copy; Copyright 2015, Melmark, Inc. All rights reserved.</div>
        </div>


        <div class="clear"></div>
    </div>
    <script type="text/javascript">

        $(function () {


            $('#slides').slides({
                preload: true,
                generateNextPrev: true
            });
            var itemCount = 0;
            var value = decodeURI( "<%= ViewBag.SearchArg %>");
            //$('#butnext').show();
            //$('#butprv').show();
            var textbox = document.getElementById('txtSearch');
            textbox.focus();
            textbox.value = value;


            var flaglim = $('#flage').val();

            $('#butprv').hide();
            $('#butnext').hide();
            if (flaglim == ">") {
                $('#butnext').show();
            }
            if (flaglim == "<") {
                $('#butprv').show();
            }
            if (flaglim == "<>") {
                $('#butnext').show();
                $('#butprv').show();
            }

            $("#butnext,#butprv").on('click', function (e) {
                e.preventDefault();



                var drpSort = document.getElementById('ddlSort');
                var sortArgmnt = drpSort.options[drpSort.selectedIndex].text;

                

                if (sortArgmnt == "-----Select-----") {
                    sortArgmnt = null;
                }

                

                var x = document.getElementById("txtSearch");
                

                if (encodeURI(x.value) == "") {
                    x = null;
                }

                var y = "";
                var radioValue = $("input[name='sortOrder']:checked").val();
                var radioValue2 = $("input[name='searchBy']:checked").val();
                var radioValue3 = $("input[name='activeClient1']:checked").val();
                if (radioValue == 1) {
                    radioValue = true;
                }
                else if (radioValue == 2) {
                    radioValue = false;
                }
                if (radioValue2 == 1) {
                    radioValue2 = true;
                }
                else if (radioValue2 == 2) {
                    radioValue2 = false;
                }
                if (radioValue3 == 1) {
                    radioValue3 = 1;
                }
                else if (radioValue3 == 2) {
                    radioValue3 = 2;
                }
                else if (radioValue3 == 4) {
                    radioValue3 = 4;
                }

                var sortArg = "";
                var data;
                var page = $('#curval').val();
                if ($(this).attr("name") == "next") {
                    data = page + "*n";
                }
                else {
                    data = page + "*p";
                }
                sortArg = decodeURI("<%= Url.Encode( ViewBag.SearchArg) %>");

                var SearchArgs = "";
                if ($('#ddlSort option:selected').attr('value') != "0")
                {
                    $("#hdnSort").val($('#ddlSort option:selected').attr('value'));
                    SearchArgs += $('#hdnSort').val() + "_";
                }

                    if (sortArgmnt == "Date Of Birth") sortArgmnt = "DateOfBirth";
                    if (sortArgmnt == "Admission Date") sortArgmnt = "AdmissionDate";
                    if (sortArgmnt == "Client ID") sortArgmnt = "ClientID";
                    if (sortArgmnt == "First Name") sortArgmnt = "FirstName";
                    if (sortArgmnt == "Last Name") sortArgmnt = "LastName";

                    //alert("sortArgmnt=" + sortArgmnt);
                    //alert("x=" +x);
                    //alert("sortarg=" + sortArg);
                    //alert("searchargs=" + encodeURI(SearchArgs));
                    
                    if (x != null && sortArgmnt != null)
                    {
                        //alert("1. x != null && sortArgmnt != null");
                        y = encodeURI(x.value) + "$" + sortArgmnt;
                        if (y == "$") {
                            y = "";
                        }
                        window.location.href = "../Client/ListClients?Name=" + encodeURI(SearchArgs) + "&argument=" + y + "&bSort=true &Data=" + data + "" + "&orderBy=" + radioValue + "&searchBy=" + radioValue2 + "&activeClient=" + radioValue3;
                    }

                    if (x == null && sortArgmnt != null)
                    {
                        //alert("2. x == null && sortArgmnt != null");
                        window.location.href = "../Client/ListClients?Name=" + encodeURI(SearchArgs) + "&argument=$" + sortArgmnt + "&bSort=true &Data=" + data + "" + "&orderBy=" + radioValue + "&searchBy=" + radioValue2 + "&activeClient=" + radioValue3;
                    }

                    if (x != null && sortArgmnt == null)
                    {
                        //alert("3. x != null && sortArgmnt == null");
                        window.location.href = "../Client/ListClients?argument=" + x.value + "&bSort=false &Data=" + data + "" + "&orderBy=" + radioValue + "&searchBy=" + radioValue2 + "&activeClient=" + radioValue3;
                    }

                    if (x == null && sortArgmnt == null)
                    {
                        //alert("4. x == null && sortArgmnt == null");
                        window.location.href = "../Client/ListClients?argument=&bSort=false &Data=" + data + "" + "&orderBy=" + radioValue + "&searchBy=" + radioValue2 + "&activeClient=" + radioValue3;
                    }

                

                

                    //OC
                    //window.location.href = "../Client/ListClients?argument=" + sortArg + "&bSort=true &Data=" + data + "";

                    //$("body").addClass("loading");
                    //window.location = "../admin/userMangment?data=" + data;

            }
                );


            itemCount = parseInt(<%: ViewBag.itemCount %>);

            if (itemCount == 0) {

                var styl = document.getElementById("noMatch").style;
                styl.display = "block";
            }

        });
    </script>

</body>
</html>
