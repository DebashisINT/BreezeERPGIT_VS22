<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/Erp.Master"
    Inherits="ERP.OMS.Management.management_Activities_ShowHistory_Phonecall" CodeBehind="ShowHistory_Phonecall.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <table class="TableMain100" width="100%">
            <tr>
                <td class="Ecoheadtxt" align="Center" style="text-align: center">
                    <asp:Label ID="lblName" runat="Server" ForeColor="red"></asp:Label></td>
            </tr>
            <tr>
                <td class="Ecoheadtxt">
                    <div class="Ecoheadtxt" runat="Server" id="showContactPanel">
                    </div>
                </td>
            </tr>
            <tr>
                <td valign="top" id="showCallHistory" runat="Server">
                    <asp:GridView ID="grdShowHistory" runat="Server" CellPadding="4" ForeColor="#333333"
                        GridLines="None" BorderWidth="1px" BorderColor="#507CD1" AutoGenerateColumns="true"
                        PageSize="15" Width="100%" OnRowDataBound="grdShowHistory_RowDataBound">
                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                        <RowStyle BackColor="#EFF3FB" BorderWidth="1px" />
                        <EditRowStyle BackColor="#2461BF" />
                        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                        <HeaderStyle Font-Bold="false" ForeColor="black" CssClass="EHEADER" BorderColor="AliceBlue"
                            BorderWidth="1px" />
                        <AlternatingRowStyle BackColor="White" />
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td class="Ecoheadtxt">
                    <div class="Ecoheadtxt" runat="Server" id="showActivityPanl">
                    </div>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
