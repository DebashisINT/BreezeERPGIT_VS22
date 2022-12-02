<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Maintenance.aspx.cs" Inherits="ERP.OMS.ErrorPages.Maintenance" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Under Maintanance</title>
    <style>
        .flexCenter {
            display: flex;
            flex-direction: column;
            align-items: center;
            width: 100%;
            height: 100vh;
            top: 0;
            right: 0;
            bottom: 0;
            left: 0;
            width: 100%;
            min-height: 600px;
            -webkit-justify-content: center;
            -moz-box-pack: center;
            -ms-flex-pack: center;
            justify-content: center;
        }
        .flexCenter img {
            margin-bottom:25px;
        }
        #LinkButton1 {
            margin-top:25px;
        }
        .mimage {
            width:220px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div class="flexCenter">
        <img src="/assests/images/maintenance.jpg" class="mimage" />
        <h3>This Website is under Maintanance</h3>
        Please visit Sometime later
        <br />

        <%--<a href="/oms/Login.aspx">Back to login</a>--%>
        
    </div>
    </form>
</body>
</html>
