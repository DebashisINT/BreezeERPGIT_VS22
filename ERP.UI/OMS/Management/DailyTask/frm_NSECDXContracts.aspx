<%@ Page Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" Inherits="ERP.OMS.Management.DailyTask.Management_DailyTask_frm_NSECDXContracts" CodeBehind="frm_NSECDXContracts.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    

    

    <script language="javascript" type="text/javascript">
        function SignOff() {
            window.parent.SignOff();
        }
        function height() {
            if (document.body.scrollHeight >= 300)
                window.frameElement.height = document.body.scrollHeight;
            else
                window.frameElement.height = '300px';

            window.frameElement.Width = document.body.scrollWidth;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <asp:Panel ID="Panelmain" runat="server" HorizontalAlign="Center" Style="z-index: 100; top: 22px"
            Visible="true">
            <table id="tbl_main" cellpadding="0" cellspacing="0" class="login" width="410" style="display: inline-table;">
                <tbody>
                    <tr>
                        <td class="lt1" style="height: 22px">
                            <h5>Imports NSE-CDX Contracts From "X_CN01_DDMMYYYY.CSV" files
                            </h5>
                        </td>
                    </tr>

                    <tr>
                        <td class="lt" style="height: 115px">
                            <table cellpadding="0" cellspacing="12" class="width100per" style="width: 100%">
                                <tbody>
                                    <tr>
                                        <%--<td class="lt" style="height: 22px">
                                                    </td>--%>
                                        <td align="right" style="width: 278px; height: 22px">
                                            <asp:FileUpload ID="MCXSelectFile" runat="server" Height="20px" Width="272px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right" colspan="1" valign="middle">
                                            <asp:Button ID="BtnSave" runat="server" CssClass="btn" OnClick="BtnSave_Click" Text="Import File"
                                                Width="84px" /></td>
                                        <td align="right" valign="middle">
                                            <table cellpadding="0" cellspacing="0">
                                                <tbody>
                                                    <tr>
                                                        <td align="center" style="height: 19px" valign="top">&nbsp;</td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </td>
                                    </tr>
                                    <%--<tr style="display: none">
                                                    <td>
                                                        <asp:TextBox ID="txtTableName" runat="server" Width="272px">TempTable</asp:TextBox></td>
                                                    <td style="width: 278px">
                                                        <asp:TextBox ID="txtCSVDir" runat="server" Width="272px">Import/Table</asp:TextBox></td>
                                                    <td>
                                                    </td>
                                                </tr>--%>
                                </tbody>
                            </table>
                        </td>
                    </tr>
                </tbody>
            </table>
        </asp:Panel>

    </div>
</asp:Content>
