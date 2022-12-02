<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CustomDesginer.aspx.cs" Inherits="ERP.OMS.Management.UserForm.CustomDesginer" %>

<%@ Register Assembly="DevExpress.XtraReports.v15.1.Web, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraReports.Web" TagPrefix="dx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <dx:ASPxReportDesigner ID="ASPxReportDesigner1" runat="server" Width="100%"
                OnSaveReportLayout="ASPxReportDesigner1_SaveReportLayout">

            </dx:ASPxReportDesigner>
        </div>
    </form>
</body>
</html>
