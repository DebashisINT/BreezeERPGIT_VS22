<%@ Page Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" Inherits="ERP.OMS.Management.management_Import_JournalVoucher" CodeBehind="Import_JournalVoucher.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe000001" %>--%>
    <link type="text/css" href="../CSS/style.css" rel="Stylesheet" />

    <script type="text/javascript" src="/assests/js/loaddata1.js"></script>

    <title>Import JournalVoucher</title>

    <script language="javascript" type="text/javascript">

        function SignOff() {
            window.parent.SignOff();
        }
        function height() {
            if (document.body.scrollHeight >= 500)
                window.frameElement.height = document.body.scrollHeight;
            else
                window.frameElement.height = '500px';
            window.frameElement.Width = document.body.scrollWidth;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div align="center">
        <asp:Panel ID="Panelmain" runat="server" Visible="true">
            <table id="tbl_main" class="login" cellspacing="0" cellpadding="0" width="410">
                <tbody>
                    <tr>
                        <td class="lt1" style="width: 437px">
                            <h5>Import Journal-Voucher
                            </h5>
                        </td>
                    </tr>
                    <tr>
                        <td class="lt brdr" style="height: 280px; width: 437px;">
                            <table cellspacing="0" cellpadding="0" align="center">
                                <tbody>
                                    <tr>
                                        <td class="lt">
                                            <table class="width100per" cellspacing="12" cellpadding="0">
                                                <tbody>
                                                    <tr>
                                                        <td class="lt">
                                                            <asp:Label ID="importstatus" runat="server" Font-Size="XX-Small" Font-Names="Arial"
                                                                Font-Bold="True" ForeColor="Red" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="lt">
                                                            <asp:HyperLink ID="HyperLink1" runat="server" ForeColor="Blue" Width="266px" NavigateUrl="~/Documents/JVDDMMYYYY.xls" Target="_blank">View File Format</asp:HyperLink></td>
                                                    </tr>

                                                    <tr>
                                                        <td align="left">Select File(JVDDMMYYYY.csv):<asp:FileUpload ID="BK01File" runat="server" Width="238px" />
                                                        </td>
                                                    </tr>

                                                    <tr>

                                                        <td valign="top" align="left">
                                                            <asp:Button ID="BtnSave" runat="server" Text="Import File" CssClass="btn" OnClick="BtnSave_Click"
                                                                Width="84px" />
                                                        </td>

                                                    </tr>
                                                </tbody>
                                            </table>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </td>
                    </tr>
                </tbody>
            </table>
        </asp:Panel>
        &nbsp;
    </div>
</asp:Content>

