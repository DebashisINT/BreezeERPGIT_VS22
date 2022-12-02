<%@ Page Language="C#" MasterPageFile="~/MasterPage/ERP.Master"  AutoEventWireup="true" CodeBehind="StartChat.aspx.cs" Inherits="BreezeChat.StartChat" %>







<%--<%@ Register Src="~/controls/ctlChatBox.ascx" TagName="ctlChatBox" TagPrefix="uc1" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Private Chat App</title>
    <script type="text/javascript">
        var srp = '<%=Page.ResolveUrl("~") %>';
    </script>
    <link href="<%=Page.ResolveUrl("~") %>Styles/bootstrap.css" rel="stylesheet" />
    <link href="<%=Page.ResolveUrl("~") %>Styles/jquery.ui.chatbox.css" rel="stylesheet" />
    <link href="<%=Page.ResolveUrl("~") %>Styles/style.css" rel="stylesheet" />        
    <link href="<%=Page.ResolveUrl("~") %>fonts/font-awesome/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="<%=Page.ResolveUrl("~") %>Scripts/jquery/jquery-ui/jquery-ui.css" rel="stylesheet" />
    <script src="<%=Page.ResolveUrl("~") %>Scripts/jquery.js"></script>    
    <script src="<%=Page.ResolveUrl("~") %>Scripts/jquery/jquery-ui/jquery-ui.js" type="text/javascript"></script>
    <script src="<%=Page.ResolveUrl("~") %>Scripts/bootstrap.min.js"></script>
    <style>
        .flRight {
            display:block;
            float:right !important;
        }
        #chat_box {
            background: #eff5f4;
            padding: 7px;
        }
        #chat_box> a {
            margin-bottom:5px;
            border-radius:4px;
            overflow:hidden;
            background: #fff;
            box-shadow: 0px 2px 5px rgb(43 42 42 / 8%);
            -webkit-transition:all 0.3s ease-in;
            transition:all 0.3s ease-in;
        }
        #chat_box> a:hover {
            -webkit-transform:scale(1.02);
            transform:scale(1.02);
        }
        #chat_widnow {
           width: 220px;
           border-radius: 6px 6px 0 0;
           overflow: hidden;
        }
        #chat_title_bar {
            height: 28px;
        }
        .userImg {
            max-width: 32px;
            margin-right: 7px;
        }
        .uName {
            text-transform:capitalize;
        }
        .onlineFlag {
            display: block;
            width: 8px;
            height: 8px;
            background: #4dd24d;
            border-radius: 50%;
            position: absolute;
            right: 12px;
            top: 22px;
        }
        textarea.ui-widget-content:focus {
            border:1px solid #2890E1
        }
         .userEnd> img {
             float:left
         }
        .self .uiCnt, .userEnd .uiCnt {
            float: left;
            background: #f1f1f1;
            font-size: 12px;
            padding: 2px 15px;
            border-radius: 5px;
                max-width: 75%;
        }
        .self .uiCntHd, .userEnd .uiCntHd {
            font-weight: 600;
            text-transform: capitalize;
            font-size: 10px;
            color: #66a966;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
        </div>
        <uc1:ctlChatBox ID="ctlChatBox1" runat="server" />
        <asp:HiddenField ID="hdnCurrentUserIDS" runat="server" />
    </form>
</body>
</html>--%>
