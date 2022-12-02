<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ForgotPassword.aspx.cs" Inherits="ERP.OMS.ForgotPassword" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Forgot Password- CRM</title>
    <meta name="description" content="" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />

    <link rel="stylesheet" href="/assests/bootstrap/css/bootstrap.min.css" />
    <link rel="stylesheet" href="/assests/bootstrap/css/bootstrap-theme.min.css" />
    <link rel="stylesheet" href="/assests/css/custom/main.css" />
    <link rel="stylesheet" type="text/css" href="/assests/fonts/font-awesome/css/font-awesome.min.css" />
    <script type="text/javascript" src="/Scripts/vendor/modernizr-2.8.3-respond-1.4.2.min.js"></script>
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
        function capLock(e) {

            kc = e.keyCode ? e.keyCode : e.which;
            sk = e.shiftKey ? e.shiftKey : ((kc == 16) ? true : false);
            if (((kc >= 65 && kc <= 90) && !sk) || ((kc >= 97 && kc <= 122) && sk))
                document.getElementById('divMayus').style.visibility = 'visible';
            else
                document.getElementById('divMayus').style.visibility = 'hidden';
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
    </script>
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
                    <input id="rurl" name="rurl" type="hidden" value="" />
                    <div class="form-group logoicon">
                        <div>
                            <asp:TextBox ID="txtEmail" CssClass="form-control smalltext fontAwesome" runat="server" placeholder=" Enter Email" TabIndex="1"></asp:TextBox>
                        </div>
                        <div style="text-align: left">
                            <em style="color: red; position: relative; right: -160px; top: -35px;"></em>
                            <span style="color: red;"><span class="field-validation-valid" data-valmsg-for="Email" data-valmsg-replace="true"></span></span>
                        </div>
                    </div>
                    <%--                    <div class="form-group">
                        <asp:TextBox ID="txtPassword" CssClass="form-control smalltext fontAwesome" placeholder=" Password" runat="server" TextMode="Password" TabIndex="2"></asp:TextBox>
                    </div>
                    <div style="text-align: left">
                        <em style="color: red; position: relative; right: 55px; top: 160px;"></em>
                        <span style="color: red"><span class="field-validation-valid" data-valmsg-for="Password" data-valmsg-replace="true"></span></span>
                    </div>--%>
                    <%--<div style="text-align: left">
                            <input data-val="true" data-val-required="The RememberMe field is required." id="RememberMe" name="RememberMe" type="checkbox" value="true"><input name="RememberMe" type="hidden" value="false">
                            Remember Me
                        </div>--%>
                    <div class="form-group" style="text-align: center">
                        <asp:Button ID="btnReLogin" runat="server" CssClass="btn btn-login btn-full" Text="Submit" OnClick="btnReLogin_Click" TabIndex="2" />
                        <asp:LinkButton ID="lbtnBackToLogin" runat="server" CausesValidation="False" TabIndex="4" CssClass="compemail" OnClick="lbtnBackToLogin_Click">Back to Login?</asp:LinkButton>

                    </div>
                </form>
            </div>
        </div>
    </div>
    <footer class="fixedfooter">
        <div class="container">
            <p class="copyright">© Copyright 2017 Indusnet Technologies Version 1.0.4</p>
        </div>
    </footer>
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.11.2/jquery.min.js"></script>
    <script type="text/javascript" src="/assests/bootstrap/js/bootstrap.min.js"></script>
    <script type="text/javascript" src="/assests/js/main.js"></script>
</body>
</html>
