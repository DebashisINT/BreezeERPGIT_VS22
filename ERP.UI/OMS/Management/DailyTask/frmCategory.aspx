<%@ Page Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" Inherits="ERP.OMS.Management.DailyTask.management_DailyTask_frmCategory" CodeBehind="frmCategory.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    

    <%--<script type="text/javascript" src="/assests/js/loaddata1.js"></script>--%>

    <script language="javascript" type="text/javascript">
        function SignOff() {
            window.parent.SignOff();
        }
        function height() {
            if (document.body.scrollHeight >= 350)
                window.frameElement.height = document.body.scrollHeight;
            else
                window.frameElement.height = '350px';

            window.frameElement.Width = document.body.scrollWidth;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="text-align: center">
        <asp:Panel ID="Panelmain" runat="server" Visible="true">
            <table id="tbl_main" class="login" cellspacing="0" cellpadding="0" width="410" style="display: inline-table;">
                <tbody>
                    <tr>
                        <td class="lt1" style="height: 22px">
                            <h5>Category &nbsp;Files
                            </h5>
                        </td>
                    </tr>


                    <tr>
                        <td class="lt brdr" style="height: 280px">
                            <table cellspacing="0" cellpadding="0" align="center">
                                <tbody>
                                    <tr>
                                        <td class="lt">
                                            <table class="width100per" cellspacing="12" cellpadding="0">
                                                <tbody>
                                                    <tr>
                                                        <td class="lt" style="height: 22px; text-align: center;" colspan="2">
                                                            <asp:Label ID="importstatus" runat="server" Font-Size="XX-Small" Font-Names="Arial"
                                                                Font-Bold="True" ForeColor="Red" />
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td style="height: 5px; vertical-align: middle; text-align: center;" colspan="2">


                                                            <table style="width: 239px">

                                                                <tr>
                                                                    <td style="width: 6px; height: 18px;">
                                                                        <asp:FileUpload ID="brSelectFile" runat="server" Width="272px" Height="23px" />
                                                                    </td>

                                                                    <%-- <td>
                                                    <table cellspacing="0" cellpadding="0">
                                                            <tbody>
                                                                <tr>
                                                                    <td valign="top" align="left" style="height: 19px">
                                                                        <asp:Button ID="BtnSave" runat="server" Text="Import File" CssClass="btn" OnClick="BtnSave_Click"
                                                                            Width="84px" />
                                                                    </td>
                                                                </tr>
                                                            </tbody>
                                                        </table>
                                                        </td>--%>
                                                                </tr>
                                                                <tr>
                                                                    <td></td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Button ID="btnImport" runat="server" Text="Import File" CssClass="btn" OnClick="btnImport_Click" Width="84px" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="lt" style="height: 22px"></td>
                                                        <td align="right" style="width: 274px; height: 22px;">
                                                            <asp:HiddenField ID="hdnDate" runat="server" />
                                                            &nbsp;</td>
                                                    </tr>

                                                    <tr>
                                                        <td class="lt" colspan="2" style="height: 22px; text-align: center"></td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right" valign="middle" colspan="2"></td>
                                                    </tr>

                                                    <tr style="display: none">
                                                        <td>
                                                            <asp:TextBox ID="txtTableName" runat="server" Width="272px">TempTable</asp:TextBox></td>
                                                        <td style="width: 274px">
                                                            <asp:TextBox ID="txtCSVDir" runat="server" Width="272px">Import/Table</asp:TextBox></td>
                                                        <td></td>
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

