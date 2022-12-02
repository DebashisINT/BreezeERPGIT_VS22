<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="BreezeChat.Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Login to BreezeERP</title>
    <meta name="description" content="" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />

    <script type="text/javascript" src="/assests/js/jquery.min.js"></script>
     
    <link rel="stylesheet" href="/assests/bootstrap/css/bootstrap.min.css" />
    <link rel="stylesheet" href="/assests/bootstrap/css/bootstrap-theme.min.css" />
    <link rel="stylesheet"  href="https://cdnjs.cloudflare.com/ajax/libs/jquery-confirm/3.3.2/jquery-confirm.min.css" />
    <link href="../assests/css/jquery-ui.css" rel="stylesheet" />
    <link rel="stylesheet" href="/assests/css/custom/main.css" />
    <link href="../assests/css/custom/newtheme.css" rel="stylesheet" />
    <link rel="stylesheet" type="text/css" href="/assests/fonts/font-awesome/css/font-awesome.min.css" />
    <script type="text/javascript" src="/assests/js/modernizr-2.8.3-respond-1.4.2.min.js"></script>
    <meta name="theme-color" content="#0C78B1" />
    <!-- Windows Phone -->
    <link href="MasterPage/css/MasterPage.css" rel="stylesheet" />
    <meta name="msapplication-navbutton-color" content="#0C78B1" />
    <!-- iOS Safari -->
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <meta name="apple-mobile-web-app-status-bar-style" content="black-translucent" />
    <script type="text/javascript" src="/assests/js/mobile-detect.js"></script>
    <script type="text/javascript" src="/assests/js/jquery-ui.min.js"></script>
    <script type="text/javascript" src="/assests/js/jquery.confirm.min.js"></script>
    <script src="/assests/js/modalmsg.js"></script>
    <script language="javascript" type="text/javascript">
        window.history.forward();
        function noBack() { window.history.forward(); }
        function ForNextPage() {
            //window.open('management/ProjectMainPage.aspx','windowname1','fullscreen=yes,titlebar=no,toolbar=no,statusbar=no');
        }

        $(function () {
            localStorage.removeItem("hdnCopymenu");
            localStorage.removeItem("TaxDetailsForSale");
            localStorage.removeItem("fv");
            localStorage.removeItem("verisonUpdate");
            localStorage.removeItem("versionData");
        });

        function blinkIt() {
            if (!document.all) return;
            else {
                for (i = 0; i < document.all.tags('blink').length; i++) {
                    s = document.all.tags('blink')[i];
                    s.style.visibility = (s.style.visibility == 'visible') ? 'hidden' : 'visible';
                }
            }
        }

        function removeCookiesKeyFromStorage() {
            var data = localStorage.getItem("GridCookiesId");
            if (data != null) {
                var splitCookiesData = data.split(',');
                for (var i = 1; i < splitCookiesData.length; i++) {
                    eraseCookie(splitCookiesData[i]);
                }
            }

            localStorage.setItem("GridCookiesId", "");
        }

        function eraseCookie(name) {
            createCookie(name, "", -1);
        }

        function createCookie(name, value, days) {
            if (days) {
                var date = new Date();
                date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
                var expires = "; expires=" + date.toGMTString();
            }
            else var expires = "";
            document.cookie = name + "=" + value + expires + "; path=/";
        }
        function removeMultitabCookies() {
            eraseCookie("ERPACTIVEURL");
        }
        removeMultitabCookies();
        removeCookiesKeyFromStorage();


        function ChangeCompany() {

            document.getElementById("divConn").style.display = "none";
            if ($("#hdnCompanyCount").val() == "2") {
                document.getElementById("btnLoginCls").style.display = "none";
                document.getElementById("btnProceed").style.display = "block";
            }
            else {
                document.getElementById("btnProceed").style.display = "none";
                document.getElementById("btnLoginCls").style.display = "block";
            }
        }

        function button_click(objTextBox, objBtnID) {
            if (window.event.keyCode == 13) {
                document.getElementById(objBtnID).focus();
                document.getElementById(objBtnID).click();
            }
        }


    </script>
   
    <script type="text/javascript">
        $(document).ready(function () {
            var md = new MobileDetect(window.navigator.userAgent);
            if (md.mobile()) {
                if ($('#ismobile').val() == '0' || $('#ismobile').val() == '') {
                    $.confirm({
                        title: 'What do you want to visit',
                        content: 'Choose one to continue',
                        buttons: {
                            Report: {
                                text: 'MIS Reports',
                                btnClass: 'btn-purple',
                                keys: ['enter', 'shift'],
                                action: function () {
                                    $('#ismobile').val('1');
                                }
                            },
                            Erp: {
                                text: 'BreezeERP',
                                btnClass: 'btn-dark',
                                keys: ['enter', 'shift'],
                                action: function () {
                                    $('#ismobile').val('0');
                                }
                            }
                        }
                    });
                } else { }

            } else {
                $('#ismobile').val('0');
            }

        });
    </script>
    <link rel="stylesheet" href="/assests/css/login.css?1.0.2" />
    <style>
        .centerd-box {
                top: auto !important;
                    padding: 24px 50px;
        }
    </style>
</head>
<body onload="noBack();setInterval('blinkIt()',500);" onpageshow="if (event.persisted) noBack();"
    onunload="">
    <%--for mobile redirection--%>
   <%-- <img src="../assests/images/flag.png" class="event" />--%>
   <div class="backColor"></div>
    <%--<img src="/assests/images/diwali.png" class="festiveImage" alt="Happy Diwali" />--%>
    <div class="bgImage">
       <%-- <img src="/assests/images/bglogin.jpg" style="width: 100%; height: 100%;" alt="" />--%>
         <img src="/assests/images/webP/bglogin.webp" style="width: 100%; height: 100%;" alt="" />
    </div>
    <div class="container boxWraper loginFlex">
        <div class="centerd-box">
            <div class="logo-wrap">
                <img src="/assests/images/webP/logo.webp" width="230" height="70" alt="" />
            </div>
            <div>
                <asp:Label ID="lblMessage" runat="server" Text="" ForeColor="Red" CssClass="hide"></asp:Label>

            </div>
            <div class="loginArea">
                <form action="" method="post" runat="server" novalidate="novalidate">
                    <input id="rurl" name="rurl" runat="server" type="hidden" value="" />
                    <div class="form-group inp mT40">
                        <label for="txtUserName">User Name </label>
                        <div>
                            <asp:TextBox ID="txtUserName" onblur="return ChangeCompany();" CssClass="form-control smalltext fontAwesome" runat="server" placeholder="" TabIndex="1"></asp:TextBox>
                        </div>
                        <div style="text-align: left">
                            <em style="color: red; position: relative; right: -160px; top: -35px;"></em>
                            <span style="color: red;">
                                <asp:RequiredFieldValidator ID="rqvUserName" runat="server"
                                    ControlToValidate="txtUserName"
                                    ErrorMessage="Please enter User Name."
                                    ForeColor="Red" ValidationGroup="login" Display="Dynamic">
                                </asp:RequiredFieldValidator></span>
                        </div>
                    </div>
                    <div class="form-group inp">
                        <label for="txtPassword">Password </label>
                        <asp:TextBox ID="txtPassword" CssClass="form-control smalltext fontAwesome" placeholder="" runat="server" TextMode="Password" TabIndex="2"></asp:TextBox>
                    </div>






                    <div style="text-align: left">
                        <em style="color: red; position: relative; right: 55px; top: 160px;"></em>
                        <span style="color: red">
                            <asp:RequiredFieldValidator ID="rqvPassword" runat="server"
                                ControlToValidate="txtPassword"
                                ErrorMessage="Please enter Password."
                                ForeColor="Red" ValidationGroup="login" Display="Dynamic">
                            </asp:RequiredFieldValidator></span>
                    </div>

                    <div runat="server" id="divConn">
                        <div class="form-group">
                            <asp:DropDownList ID="ddlConn" CssClass="form-control smalltext fontAwesome" placeholder=" Company" runat="server" TabIndex="3"></asp:DropDownList>
                        </div>
                        <div style="text-align: left">
                            <em style="color: red; position: relative; right: 55px; top: 160px;"></em>
                            <span style="color: red">
                                <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                                    ControlToValidate="ddlConn"
                                    ErrorMessage="Please select company."
                                    ForeColor="Red" ValidationGroup="login" Display="Dynamic">
                                </asp:RequiredFieldValidator>--%></span>
                        </div>
                    </div>
                    <%--<div style="text-align: left">
                            <input data-val="true" data-val-required="The RememberMe field is required." id="RememberMe" name="RememberMe" type="checkbox" value="true"><input name="RememberMe" type="hidden" value="false">
                            Remember Me
                        </div>--%>

                    <div id="btnProceed" runat="server" class="form-group" style="text-align: center">
                        <asp:Button ID="ShowDB" runat="server" CssClass="btn btn-login btn-full" Text="Proceed" OnClick="ShowDB_Click" TabIndex="3" UseSubmitBehavior="false" />

                        <%--   <asp:Label ID="lblmac" runat="server"></asp:Label>--%>
                    </div>


                    <div id="btnLoginCls" runat="server" class="form-group" style="text-align: center">
                        <asp:Button ID="Submit1" ValidationGroup="login" runat="server" CssClass="btn btn-login btn-full" Text="Submit" OnClick="Login_User" TabIndex="3" UseSubmitBehavior="false" />
                        <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" TabIndex="4" CssClass="compemail hide" OnClick="LinkButton1_Click1">Forgot  Password?</asp:LinkButton>


                        <%--   <asp:Label ID="lblmac" runat="server"></asp:Label>--%>
                    </div>
                    <asp:HiddenField runat="server" ID="hdnCompanyCount" />
                    <asp:HiddenField runat="server" ID="ismobile" />
                </form>
            </div>
        </div>
    </div>
    <footer class="fixedfooter">
            <div class="container">
                <p class="copyright">
                    © Copyright <%= DateTime.Now.Year.ToString() %> Indus Net Technologies. [BreezeERP Version
                     <asp:Label ID="lblVersion" runat="server" Text="1.0.4" /> <!--<a href="Management/Master/view-version-features.aspx" target="_blank">( What's New )</a>--><br />
                    
                </p>
                
            </div>
        </footer>

    <script type="text/javascript" src="/assests/bootstrap/js/bootstrap.min.js"></script>
    <script type="text/javascript" src="/assests/js/main.js"></script>
    
</body>
</html>



