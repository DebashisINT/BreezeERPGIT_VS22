<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master" Inherits="ERP.OMS.Management.management_Contact_CommissionProfile" CodeBehind="Contact_CommissionProfile.aspx.cs" %>


<%--<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe000001" %>--%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../CSS/style.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <table class="TableMain100">
            <tr>
                <td>Company:
                </td>
                <td>
                    <asp:DropDownList ID="cmbOrganization" runat="server" Width="205px" Font-Size="11px"
                        TabIndex="24">
                    </asp:DropDownList>
                </td>

            </tr>
            <tr>
                <td>Profile Type:
                </td>
                <td>
                    <asp:DropDownList ID="drpType" Width="203px" Font-Size="11px" runat="server" TabIndex="7" Enabled="false">
                        <asp:ListItem Text="Group" Value="2"></asp:ListItem>
                        <asp:ListItem Text="Specific" Value="1"></asp:ListItem>
                    </asp:DropDownList>
                </td>

            </tr>
            <tr>
                <td>Group Code:
                </td>
                <td>
                    <asp:DropDownList ID="drpGroupCode" runat="server" Width="205px" Font-Size="11px"
                        TabIndex="24">
                    </asp:DropDownList>
                </td>

            </tr>
            <tr>
                <td>From Date:
                </td>
                <td>
                    <dxe:ASPxDateEdit ID="txtFromDate" runat="server" EditFormat="Custom" UseMaskBehavior="True"
                        TabIndex="9" Width="202px">
                        <ButtonStyle Width="13px">
                        </ButtonStyle>
                    </dxe:ASPxDateEdit>
                </td>

            </tr>
            <tr>
                <td></td>
                <td>
                    <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btnUpdate" OnClick="btnSave_Click"
                        TabIndex="31" ValidationGroup="a" />
                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btnUpdate" OnClientClick="Close()"
                        TabIndex="32" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
