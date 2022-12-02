<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master" 
    Inherits="ERP.OMS.Reports.Reports_frmBannedClient" Codebehind="frmBannedClient.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <iframe id="fclient" src="frmBannedClientUrl.aspx" frameborder="0" scrolling="auto" style="width:940px;height:600px"></iframe>
        
    </div>
</asp:Content>
