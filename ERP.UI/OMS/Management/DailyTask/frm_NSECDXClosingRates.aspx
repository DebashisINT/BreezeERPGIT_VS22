<%@ Page Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" Inherits="ERP.OMS.Management.DailyTask.management_DailyTask_frm_NSECDXClosingRates" CodeBehind="frm_NSECDXClosingRates.aspx.cs" %>

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
    <script type="text/javascript" src="/assests/js/init.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div align="center">
        <asp:Panel ID="Panelmain" runat="server" HorizontalAlign="Center" Style="z-index: 100; left: 0px; top: 0px"
            Visible="true">
            <table id="tbl_main" cellpadding="0" cellspacing="0" class="login" style="height: 153px; display: inline-table;"
                width="410">
                <tbody>
                    <tr>
                        <td class="lt1" style="height: 22px">
                            <h5>Imports NSE-CDX ClosingRates
                            </h5>
                        </td>
                    </tr>

                    <tr>
                        <td class="lt" style="height: 115px">
                            <table cellpadding="0" cellspacing="12" class="width100per" style="width: 100%">
                                <tbody>
                                    <tr>
                                        <td class="gridcellleft">
                                            <span style="color: #000099; text-align: right">Market Stats</span></td>
                                        <td align="right" style="width: 278px; height: 22px">
                                            <asp:FileUpload ID="MarketStatsSelectFile" runat="server" Height="21px" Width="272px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right" colspan="2" style="height: 20px" valign="middle">
                                            <table cellpadding="0" cellspacing="0">
                                                <tbody>
                                                    <tr>
                                                        <td align="left" style="height: 19px" valign="top">
                                                            <asp:Button ID="BtnSave" runat="server" CssClass="btn" OnClick="BtnSave_Click" Text="Import File"
                                                                Width="84px" />
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr id="trYesNo" runat="server">
                                        <td align="left">
                                            <asp:Label ID="lblMessage" runat="server" ForeColor="Red" Width="258px"></asp:Label>
                                        </td>
                                        <td align="left" style="text-align: right">
                                            <asp:Button ID="btnYes" runat="server" CssClass="btn" OnClick="btnYes_Click" Text="Yes" />
                                            <asp:Button ID="btnNo" runat="server" CssClass="btn" OnClick="btnNo_Click" Text="No" />
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </td>
                    </tr>
                </tbody>
            </table>
            <%--</td>
                                </tr>
                                
                                   
                            </tbody>--%>
            <%--</table>--%>
            <asp:HiddenField ID="hdfname" runat="server" />
        </asp:Panel>
    </div>
</asp:Content>
