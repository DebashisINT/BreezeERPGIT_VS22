<%@ Page Language="C#" AutoEventWireup="true" Inherits="pLogin" EnableEventValidation="false" CodeBehind="Login.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Login to BreezeERP</title>
    <meta name="description" content="" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />

    <script type="text/javascript" src="/assests/js/jquery.min.js"></script>

    <link rel="stylesheet" href="/assests/bootstrap/css/bootstrap.min.css" />
    <link rel="stylesheet" href="/assests/bootstrap/css/bootstrap-theme.min.css" />
    <link rel="stylesheet" href="/assests/css/custom/main.css" />
    <link rel="stylesheet" type="text/css" href="/assests/fonts/font-awesome/css/font-awesome.min.css" />
    <script type="text/javascript" src="/assests/js/modernizr-2.8.3-respond-1.4.2.min.js"></script>
    <meta name="theme-color" content="#0C78B1" />
    <!-- Windows Phone -->
    <meta name="msapplication-navbutton-color" content="#0C78B1" />
    <!-- iOS Safari -->
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <meta name="apple-mobile-web-app-status-bar-style" content="black-translucent" />
    <script language="javascript" type="text/javascript">
        window.history.forward();
        function noBack() { window.history.forward(); }
        function ForNextPage() {
            //window.open('management/ProjectMainPage.aspx','windowname1','fullscreen=yes,titlebar=no,toolbar=no,statusbar=no');
        }       

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

        removeCookiesKeyFromStorage();
    </script>
    <style>
        .logo-wrap > img {
            width:auto;
            max-width:100%  ;
            height:auto;
        }
    </style>
</head>
<body onload="noBack();setInterval('blinkIt()',500);" onpageshow="if (event.persisted) noBack();"
    onunload="">

    <div class="bgImage">
        <img src="/assests/images/bglogin.jpg" style="width: 100%; height: 100%;">
    </div>
    <div class="container boxWraper">
        <div class="centerd-box">
            <div class="logo-wrap">
                <img src="/assests/images/logo.png" width="230" height="70" />
            </div>
            <div>
                <asp:Label ID="lblMessage" runat="server" Text="" ForeColor="Red"></asp:Label>

            </div>
            <div class="loginArea">
                <form action="" method="post" runat="server" novalidate="novalidate">
                    <input id="rurl" name="rurl" runat="server" type="hidden" value="" />
                    <div class="form-group logoicon">
                        <div>
                            <asp:TextBox ID="txtUserName" CssClass="form-control smalltext fontAwesome" runat="server" placeholder=" User Name" TabIndex="1"></asp:TextBox>
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
                    <div class="form-group">
                        <asp:TextBox ID="txtPassword" CssClass="form-control smalltext fontAwesome" placeholder=" Password" runat="server" TextMode="Password" TabIndex="2"></asp:TextBox>
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
                    <%--<div style="text-align: left">
                            <input data-val="true" data-val-required="The RememberMe field is required." id="RememberMe" name="RememberMe" type="checkbox" value="true"><input name="RememberMe" type="hidden" value="false">
                            Remember Me
                        </div>--%>
                    <div class="form-group" style="text-align: center">
                        <asp:Button ID="Submit1" ValidationGroup="login" runat="server" CssClass="btn btn-login btn-full" Text="Submit" OnClick="Login_User" TabIndex="3" />
                        <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" TabIndex="4" CssClass="compemail" OnClick="LinkButton1_Click1">Forgot  Password?</asp:LinkButton>
                    </div>

                </form>
            </div>
        </div>
    </div>
    <footer class="fixedfooter">
            <div class="container">
                <p class="copyright">
                    © Copyright 2017 Indusnet Technologies Version <asp:Label ID="lblVersion" runat="server" Text="1.0.4" /> 
                </p>
            </div>
        </footer>
    
    <script type="text/javascript" src="/assests/bootstrap/js/bootstrap.min.js"></script>
    <script type="text/javascript" src="/assests/js/main.js"></script>

</body>
</html>
