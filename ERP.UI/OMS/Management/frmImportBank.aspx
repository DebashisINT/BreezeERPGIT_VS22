<%@ Page Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" Inherits="ERP.OMS.Management.management_frmImportBank" CodeBehind="frmImportBank.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../CSS/style.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript" src="/assests/js/init.js"></script>
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
        <table class="TableMain100">
            <tr>
                <td class="EHEADER">
                    <span style="color: blue"><strong>BANK IMPORT</strong></span></td>
            </tr>
        </table>
        <asp:Panel ID="Panelmain" runat="server" Visible="true" HorizontalAlign="Center">
            <table id="tbl_main" class="login" cellspacing="0" cellpadding="0" width="510" height="300">
                <tbody>

                    <tr>
                        <td class="lt">
                            <table class="width100per" cellspacing="12" cellpadding="0">
                                <tbody>
                                    <tr>
                                        <td colspan="2">This Routine Imports NEFT enabled bank branches.Download available from http://www.rbi.org.in/scripts/neft.aspx and convert it into CSV file and then upload.
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right" style="width: 278px" colspan="2">
                                            <asp:FileUpload ID="NCDEXSelectFile" runat="server" Width="372px" Height="21px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 200px">
                                            <asp:Label ID="lblMsgAccCode" Width="120px" ForeColor="Red" runat="server"></asp:Label></td>
                                        <td align="left" valign="middle">
                                            <table cellspacing="0" cellpadding="0">
                                                <tbody>
                                                    <tr>
                                                        <td valign="top" align="left" style="height: 19px">
                                                            <asp:Button ID="BtnSave" runat="server" Text="Import File" CssClass="btn"
                                                                Width="84px" OnClick="BtnSave_Click" />
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
    </div>
</asp:Content>

