<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProductReportViewer.aspx.cs" Inherits="ERP.OMS.Reports.XtraReports.ProductReportViewer" %>

<%@ Register Assembly="DevExpress.XtraReports.v15.1.Web, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraReports.Web" TagPrefix="dx" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">
    <link rel="stylesheet" href="/assests/css/custom/main.css" />
    <link rel="stylesheet" type="text/css" href="/assests/fonts/font-awesome/css/font-awesome.min.css" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
         <div class="crossBtn" style="top: 41px;color:#ccc"><a href=" ../master/RptProductMasterReport.aspx"><i class="fa fa-times"></i></a></div>
    <div>
        <dx:ASPxDocumentViewer ID="ASPxDocumentViewer1" runat="server"></dx:ASPxDocumentViewer>
    </div>
    </form>
</body>
</html>
