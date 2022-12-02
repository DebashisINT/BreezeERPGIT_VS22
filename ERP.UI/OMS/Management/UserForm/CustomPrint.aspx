<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CustomPrint.aspx.cs" Inherits="ERP.OMS.Management.UserForm.CustomPrint" %>

<%@ Register Assembly="DevExpress.XtraReports.v15.1.Web, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraReports.Web" TagPrefix="dx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
      <script type="text/javascript" src="https://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.12.4.min.js"></script> 
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <dx:ASPxDocumentViewer ID="ASPxDocumentViewer1" ClientInstanceName="printRpt" runat="server" Width="100%"></dx:ASPxDocumentViewer>
    </div>
    </form>
</body>
</html>
