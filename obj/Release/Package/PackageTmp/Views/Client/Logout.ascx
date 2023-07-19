﻿<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>

<head>
    <meta name="viewport" content="width=device-width" />
    <style type="text/css">
        body {
            margin: 0;
            padding: 0;
            width: 100%;
            height: 100%;
        }

        div.logoutContainer {
            width: 95.5%;
            height: 580px;
            display: block;
            margin: 0 auto 30px auto;
            background: #fff;
            padding: 0 2% 0 2%;
        }

            div.logoutContainer div.lgoContainer {
                background: url("../Documents/Images/loginpage_03.png") no-repeat scroll right top transparent;
                float: left;
                height: 580px;
                margin-left: 19%;
                width: 30.5%;
            }

                div.logoutContainer div.lgoContainer img {
                    float: right;
                    margin: 250px 15px 0 0;
                    display: block;
                }

            div.logoutContainer div.CCContainer {
                width: 49%;
                height: 580px;
                float: right;
            }

                div.logoutContainer div.CCContainer div.topbr {
                    border-bottom: 1px solid #E9E9E9;
                    border-top: 1px solid #E9E9E9;
                    float: left;
                    height: 37px;
                    margin: 270px 0 0 7%;
                    width: 90%;
                }

                    div.logoutContainer div.CCContainer div.topbr img {
                        display: block;
                        margin: 1.8% 2.6% 0 5%;
                        float: left;
                    }

                    div.logoutContainer div.CCContainer div.topbr h4 {
                        color: #555555;
                        font-family: Arial,Helvetica,sans-serif;
                        font-size: 106%;
                        font-weight: bold;
                        letter-spacing: 3px;
                        margin: 1.8% 0 0;
                        padding: 0;
                    }

                        div.logoutContainer div.CCContainer div.topbr h4 span {
                            margin: 0;
                            padding: 0;
                            color: #006754;
                        }

                div.logoutContainer div.CCContainer p {
                    color: #555555;
                    display: block;
                    float: left;
                    font-family: Arial,Helvetica,sans-serif;
                    font-size: 75%;
                    letter-spacing: 2px;
                    margin: 85px 0 0 7%;
                    text-decoration: none;
                    width: 90%;
                }

                    div.logoutContainer div.CCContainer p a,
                    div.logoutContainer div.CCContainer p a:link,
                    div.logoutContainer div.CCContainer p a:visited {
                        color: #006754;
                        margin: 0;
                        padding: 0;
                        text-decoration: none;
                    }

                        div.logoutContainer div.CCContainer p a:hover {
                            text-decoration: none;
                            color: #F63;
                        }
    </style>
</head>
<body>
    <div>
      <div class="logoutContainer">
            <div class="lgoContainer">
                <img src="../../Documents/Images/logo01.jpg" alt="logo" />
            </div>
            <div class="CCContainer">
                <div class="topbr">
                    <img src="../../Documents/Images/ick_03.png" width="29" height="22" />
                    <h4>You are successfully <span>Logged out</span></h4>
                </div>
                <p>You can return to the home page or  <a href="../../../../Login.aspx">Sign in</a> again.</p>
            </div>


        </div>  
    </div>
</body>
<%--  --%>

<%--  --%><%--  --%>