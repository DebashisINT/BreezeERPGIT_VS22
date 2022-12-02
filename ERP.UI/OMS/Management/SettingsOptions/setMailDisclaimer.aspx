<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master" Inherits="ERP.OMS.Management.SettingsOptions.management_SettingsOptions_setMailDisclaimer" Codebehind="setMailDisclaimer.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

      
   

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <div class="panel-heading">
        <div class="panel-title">
            <h3>Email Desclaimer</h3>
        </div>

    </div>
     <div class="form_main">

   <table  class="TableMain100">
<%--    <tr>
        <td class="EHEADER" colspan="2" style="text-align:center;">
            <strong><span style="color: #000099">Email Desclaimer</span></strong>
        </td>
    </tr>--%>
    <tr>
        
        <td class="gridcellleft">
            <label>Disclaimer:</label>
            <asp:TextBox ID="txtDisclaimer" runat="server" Font-Size="12px" Width="90%" Height="80px" TextMode="MultiLine" ></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtDisclaimer"
                Display="Dynamic" ErrorMessage="Enter Disclaimer."></asp:RequiredFieldValidator>
            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtDisclaimer"
                Display="Dynamic" ErrorMessage='Can not have " character!' ValidationExpression='^\w[^"]*$'></asp:RegularExpressionValidator></td>
    </tr>
    <tr>
        
        <td class="gridcellleft">
            <asp:Button ID="btnCreate" runat="server" Text="Create" CssClass="btnUpdate btn btn-primary" OnClick="btnCreate_Click" />
        </td>
    </tr>
</table>
         </div>
</asp:Content>
