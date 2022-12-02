<%@ Page Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" Inherits="ERP.OMS.Management.management_frm_SalesCheckList" CodeBehind="frm_SalesCheckList.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe000001" %>--%>
    <%@ Register Src="Headermain.ascx" TagName="Headermain" TagPrefix="uc1" %>
    <link href="../CSS/style.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <table class="TableMain100" style="border: solid 1px blue">
            <tr>
                <td colspan="2" align="center"><strong>Checklist</strong></td>
            </tr>
            <tr>
                <td style="width: 2%">
                    <asp:Label ID="Label31" runat="server" CssClass="mylabel1"
                        Text="Product Application form" Width="133px"></asp:Label></td>
                <td>
                    <asp:DropDownList ID="drpProductApplicationForm" runat="Server" Width="164px">
                        <asp:ListItem>Pending</asp:ListItem>
                        <asp:ListItem>Recevied</asp:ListItem>
                        <asp:ListItem>Not Applicable</asp:ListItem>
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td style="height: 24px">
                    <asp:Label ID="Label30" runat="server" CssClass="mylabel1" Text="Photo ID Proof"
                        Width="132px"></asp:Label></td>
                <td style="height: 24px">
                    <asp:DropDownList ID="drpPhotoIdProof" runat="Server" Width="165px">
                        <asp:ListItem>Pending</asp:ListItem>
                        <asp:ListItem>Recevied</asp:ListItem>
                        <asp:ListItem>Not Applicable</asp:ListItem>
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label29" runat="server" CssClass="mylabel1" Text="Address Proof"
                        Width="129px"></asp:Label></td>
                <td>
                    <asp:DropDownList ID="drpAddressProof" runat="Server" Width="166px">
                        <asp:ListItem>Pending</asp:ListItem>
                        <asp:ListItem>Recevied</asp:ListItem>
                        <asp:ListItem>Not Applicable</asp:ListItem>
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label28" runat="server" CssClass="mylabel1" Text="Age Proof" Width="65px"></asp:Label></td>
                <td>
                    <asp:DropDownList ID="drpAgeProof" runat="Server" Width="168px">
                        <asp:ListItem>Pending</asp:ListItem>
                        <asp:ListItem>Recevied</asp:ListItem>
                        <asp:ListItem>Not Applicable</asp:ListItem>
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label27" runat="server" CssClass="mylabel1" Text="Signature Proof"
                        Width="132px"></asp:Label></td>
                <td>
                    <asp:DropDownList ID="drpSignatureProof" runat="Server" Width="166px">
                        <asp:ListItem>Pending</asp:ListItem>
                        <asp:ListItem>Recevied</asp:ListItem>
                        <asp:ListItem>Not Applicable</asp:ListItem>
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td style="height: 24px">
                    <asp:Label ID="Label26" runat="server" CssClass="mylabel1" Text="KYC Document"
                        Width="131px"></asp:Label></td>
                <td style="height: 24px">
                    <asp:DropDownList ID="drpKYCDocument" runat="Server" Width="167px">
                        <asp:ListItem>Pending</asp:ListItem>
                        <asp:ListItem>Recevied</asp:ListItem>
                        <asp:ListItem>Not Applicable</asp:ListItem>
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label25" runat="server" CssClass="mylabel1" Text="Tripartite Agreement"
                        Width="128px"></asp:Label></td>
                <td>
                    <asp:DropDownList ID="drpTripartiteAgreement" runat="Server" Width="168px">
                        <asp:ListItem>Pending</asp:ListItem>
                        <asp:ListItem>Recevied</asp:ListItem>
                        <asp:ListItem>Not Applicable</asp:ListItem>
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label24" runat="server" CssClass="mylabel1" Text="POA Agreement"
                        Width="129px"></asp:Label></td>
                <td>
                    <asp:DropDownList ID="drpPOAAgreement" runat="Server" Width="168px">
                        <asp:ListItem>Pending</asp:ListItem>
                        <asp:ListItem>Recevied</asp:ListItem>
                        <asp:ListItem>Not Applicable</asp:ListItem>
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label23" runat="server" CssClass="mylabel1" Text="Medical Reports"
                        Width="128px"></asp:Label></td>
                <td>
                    <asp:DropDownList ID="drpMedicalReports" runat="Server" Width="168px">
                        <asp:ListItem>Pending</asp:ListItem>
                        <asp:ListItem>Recevied</asp:ListItem>
                        <asp:ListItem>Not Applicable</asp:ListItem>
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td class="rt">
                    <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" /></td>
                <td>
                    <asp:Button ID="btnDiacard" runat="server" Text="Discard" /></td>
            </tr>
        </table>
    </div>
</asp:Content>


