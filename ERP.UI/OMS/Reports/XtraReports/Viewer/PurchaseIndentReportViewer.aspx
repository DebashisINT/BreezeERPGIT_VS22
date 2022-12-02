<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PurchaseIndentReportViewer.aspx.cs" Inherits="ERP.OMS.Reports.XtraReports.Viewer.PurchaseIndentReportViewer" %>

<%@ Register assembly="DevExpress.XtraReports.v15.1.Web, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.XtraReports.Web" tagprefix="dx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
   <link rel="stylesheet" href="/assests/css/custom/main.css" />
    <link rel="stylesheet" type="text/css" href="/assests/fonts/font-awesome/css/font-awesome.min.css" />
     <script type="text/javascript" src="/assests/js/jquery.min.js"></script>
    <title>Purchase Order</title>
    <script type="text/javascript">
        $(document).ready(function () {
            cASPxDocumentViewer1.Print();

        });

    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
     <div class="crossBtn" style="top: 41px;color:#ccc"><a href=" ../../../management/Activities/PurchaseIndent.aspx"><i class="fa fa-times"></i></a></div>
    <div>
        <dx:ASPxDocumentViewer ID="ASPxDocumentViewer1" runat="server" ClientInstanceName="cASPxDocumentViewer1" >
            <SettingsReportViewer EnableReportMargins="True" />
        </dx:ASPxDocumentViewer>
    </div>
    </div>
    </form>
</body>
</html>
