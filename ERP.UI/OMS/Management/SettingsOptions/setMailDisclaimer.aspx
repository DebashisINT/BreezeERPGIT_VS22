<%--================================================== Revision History =============================================
1.0   Pallab    V2.0.38      23-05-2023          0026205: Email Desclaimer module design modification & check in small device
====================================================== Revision History =============================================--%>

<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master" Inherits="ERP.OMS.Management.SettingsOptions.management_SettingsOptions_setMailDisclaimer" Codebehind="setMailDisclaimer.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

      
   <%--Rev 1.0--%>
    <link href="/assests/css/custom/newcustomstyle.css" rel="stylesheet" />
    
    <style>
        select
        {
            z-index: 1;
        }

        #gridAdvanceAdj {
            max-width: 99% !important;
        }
        #FormDate, #toDate, #dtTDate, #dt_PLQuote, #dt_PlQuoteExpiry {
            position: relative;
            z-index: 1;
            background: transparent;
        }

        select
        {
            -webkit-appearance: none;
        }

        .calendar-icon
        {
            right: 20px;
        }

        .panel-title h3
        {
            padding-top: 0px !important;
        }

        .fakeInput
        {
                min-height: 30px;
    border-radius: 4px;
        }
        
    </style>
    <%--Rev end 1.0--%>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--Rev 1.0: "outer-div-main" class add --%>
    <div class="outer-div-main clearfix">
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
            <asp:Button ID="btnCreate" runat="server" Text="Create" CssClass="btnUpdate btn btn-primary mt-10" OnClick="btnCreate_Click" />
        </td>
    </tr>
</table>
         </div>
    </div>
</asp:Content>
