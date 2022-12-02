<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Management.ToolsUtilities.management_utilities_frm_LeaveApplication_popup" Codebehind="frm_LeaveApplication_popup.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../../css/style.css" rel="stylesheet" type="text/css" />
</asp:Content>



<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     
    <table>
            <tr>
                <td class="gridcellleft">
                    <span class="Ecoheadtxt" style="color: Blue"><strong>PL</strong></span>
                </td>
                <td class="gridcellleft">
                    <asp:Label ID="lblPL" runat="server" Text="0.0"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="gridcellleft">
                    <span class="Ecoheadtxt" style="color: Blue"><strong>CL</strong></span>
                </td>
                <td class="gridcellleft">
                    <asp:Label ID="lblCL" runat="server" Text="0.0"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="gridcellleft">
                    <span class="Ecoheadtxt" style="color: Blue"><strong>SL</strong></span>
                </td>
                <td class="gridcellleft">
                    <asp:Label ID="lblSL" runat="server" Text="0.0"></asp:Label>
                </td>
            </tr>
        </table>
</asp:Content>
