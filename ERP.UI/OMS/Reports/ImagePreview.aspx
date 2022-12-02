<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Reports.Management_ImagePreview" Codebehind="ImagePreview.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script language="javascript" type="text/javascript" >
    function Close()
    {
        //alert("aa");
         window.opener.location = '/ImagePreview.aspx'; 
         window.close();
        //window.close();
    }
    </script>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <asp:Image ID="imgCashBankDoc" runat="server"  Width="95%" />
    </div>
    <div>
       
    </div>
</asp:Content>
