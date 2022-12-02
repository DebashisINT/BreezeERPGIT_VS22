<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WhatsNewAddEdit.aspx.cs" Inherits="ERP.OMS.Management.WhatsNewAddEdit" %>

<%@ Register Assembly="DevExpress.Web.ASPxRichEdit.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxRichEdit" TagPrefix="dx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script>
        function toggleFullScreenModefunction(s, e) {
            crichedit.toggleFullScreenMode();
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <dx:ASPxRichEdit ID="whatsNew" runat="server" WorkDirectory="~\App_Data\WorkDirectory"  ClientInstanceName="crichedit"
            ShowConfirmOnLosingChanges="false" Width="100%" Height="700px">

             <ClientSideEvents Init="toggleFullScreenModefunction" />

        </dx:ASPxRichEdit>
    </div>
    </form>
</body>
</html>
