<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MultiTabError.aspx.cs" Inherits="ERP.OMS.MultiTabError" %>

 
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../assests/bootstrap/css/bootstrap.min.css" rel="stylesheet" />
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
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div class="flexCenter">
        <img src="../assests/images/Warning.png" />
        You are not allowed to open the Application in Multiple Tab.
        <br /> 
    </div>
    </form>
</body>
</html>

