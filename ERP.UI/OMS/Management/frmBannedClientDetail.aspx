<%@ Page Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" Inherits="ERP.OMS.Management.management_frmBannedClientDetail" CodeBehind="frmBannedClientDetail.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link type="text/css" href="../CSS/style.css" rel="Stylesheet" />
    <script language="javascript" type="text/javascript">
        function aa() {
            var val = document.getElementById('hdnlink').value;
            window.open(val);
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <table class="TableMain100">
            <tr>
                <td class="EHEADER" style="text-align: center;">
                    <strong><span style="color: #000099">SEBI DETAIL</span></strong>
                </td>
            </tr>
        </table>
        <table class="TableMain100">
            <tr>
                <td style="width: 128px">
                    <asp:Label ID="Label4" runat="server" Text="OrderDate" Width="173px"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lblBannedOrderDate" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="width: 128px">
                    <asp:Label ID="Label6" runat="server" Text="Particulars" Width="173px"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lblParticulars" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="width: 128px">
                    <asp:Label ID="Label9" runat="server" Text="Description" Width="173px"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lblDescription" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="width: 128px">
                    <asp:Label ID="Label5" runat="server" Text="Pan" Width="173px"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lblBannedPan" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="width: 128px">
                    <asp:Label ID="Label7" runat="server" Text="Circular Date" Width="173px"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lblCirculardate" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="width: 128px">
                    <asp:Label ID="Label8" runat="server" Text="Period" Width="172px"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lblBanPeriod" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="width: 128px">
                    <asp:Label ID="lblCircularLink" runat="server" Text="Circular Link" Width="172px"></asp:Label>
                </td>
                <td>
                    <a href="javascript:aa();" id="CID" runat="server"></a>
                    <asp:HiddenField ID="hdnlink" runat="server" />
                </td>
            </tr>
            <tr>
                <asp:Label ID="Label3" runat="server"></asp:Label>
            </tr>
        </table>
    </div>
</asp:Content>
