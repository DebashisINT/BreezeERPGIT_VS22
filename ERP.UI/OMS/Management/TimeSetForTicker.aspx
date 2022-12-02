<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Management.JSFUNCTION_TimeSetForTicker" CodeBehind="TimeSetForTicker.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function SignOff() {
            window.parent.SignOff()
        }
        function height() {
            window.frameElement.height = document.body.scrollHeight;
            window.frameElement.widht = document.body.scrollWidht;
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table class="TableMain100">
        <tr>
            <td class="EHEADER" colspan="2" style="text-align: center;">
                <strong><span style="color: #000099">Add/Edit Time Period</span></strong></td>
        </tr>
        <tr>
            <td colspan="2">
                <br />
            </td>
        </tr>
        <tr>
            <td class="gridcellright">
                <asp:Label ID="Label1" runat="server" Text="Set Time to refresh Ticker:" CssClass="Ecoheadtxt"
                    Width="130px" Height="16px"></asp:Label>
            </td>
            <td class="gridcellleft">
                <asp:DropDownList ID="cmbTimeForTicker" runat="server" Width="161px" Font-Size="12px"
                    ForeColor="Navy" Height="24px">
                    <%--<asp:ListItem Value="30">30 Seconds</asp:ListItem>
                    <asp:ListItem Value="60">1 Minute</asp:ListItem>
                    <asp:ListItem Value="120">2 Minutes</asp:ListItem>--%>
                    <asp:ListItem Value="300">5 Minutes</asp:ListItem>
                    <asp:ListItem Value="600">10 Minutes</asp:ListItem>
                    <asp:ListItem Value="6150">15 Minutes</asp:ListItem>
                    <asp:ListItem Value="1800">30 Minutes</asp:ListItem>
                    <asp:ListItem Value="3600">1 Hour</asp:ListItem>
                </asp:DropDownList>
                <asp:Label ID="lblMessage" runat="server" Font-Bold="True" ForeColor="Red"></asp:Label></td>
        </tr>
        <tr>
            <td style="height: 22px"></td>
            <td class="gridcellleft" style="height: 22px">
                <asp:Button ID="btnsave" runat="server" Text="Save" CssClass="btnUpdate" Height="20px"
                    OnClick="btnsave_Click" Width="59px" />
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <br />
            </td>
        </tr>
    </table>
</asp:Content>
