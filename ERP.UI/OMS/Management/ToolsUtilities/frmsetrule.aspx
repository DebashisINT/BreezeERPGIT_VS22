<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Management.ToolsUtilities.management_utilities_frmsetrule" Codebehind="frmsetrule.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <% showlist();%>
        <asp:Button ID="Button1" runat="server" Text="Close" OnClick="Button1_Click" />
    </div>
</asp:Content>
