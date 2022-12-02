<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReportDesignerRepx.aspx.cs" Inherits="ERP.OMS.Reports.REPXReports.ReportDesignerRepx" %>

<%--<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>--%>
<%@ Register assembly="DevExpress.XtraReports.v15.1.Web, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.XtraReports.Web" tagprefix="dx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <dx:ASPxReportDesigner ID="ASPxReportDesigner1" runat="server" OnSaveReportLayout="ASPxReportDesigner1_SaveReportLayout"></dx:ASPxReportDesigner>
    </div>
          <asp:HiddenField ID ="RptName" runat="server" />
    </form>
</body>
</html>
