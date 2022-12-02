<%@ Page Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" Inherits="ERP.OMS.Management.management_frmcallforward" CodeBehind="frmcallforward.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link type="text/css" href="../CSS/style.css" rel="Stylesheet" />
    <script type="text/javascript" src="/assests/js/loaddata1.js"></script>
    <script language="javascript" type="text/javascript">
        function SignOff() {
            window.parent.SignOff()
        }
        function height() {

            if (document.body.scrollHeight >= 550) {
                window.frameElement.height = document.body.scrollHeight;
            }
            else {
                window.frameElement.height = '550px';
            }
            window.frameElement.widht = document.body.scrollWidht;

        }
        function lostFocus() {
            var str = window.opener.document.getElementById("ctl00_ContentPlaceHolder3_txtNote");
            str.focus();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <table class="TableMain100">
            <tr>
                <td colspan="3" class="Ecoheadtxt" style="text-align: center">Call Forward/Deligate</td>
            </tr>
            <tr>
                <td style="width: 20%" class="Ecoheadtxt">Call Outcome</td>
                <td style="width: 1%" class="Ecoheadtxt">:</td>
                <td class="Ecoheadtxt" style="text-align: left">
                    <asp:TextBox ID="txtCallOutcome" runat="server" Width="380px"></asp:TextBox></td>
            </tr>
            <tr>
                <td style="width: 20%" class="Ecoheadtxt">Forward To</td>
                <td style="width: 1%" class="Ecoheadtxt">:</td>
                <td class="Ecoheadtxt" style="text-align: left">
                    <asp:DropDownList ID="drpCallForward" runat="Server" Width="383px"></asp:DropDownList></td>
            </tr>
            <tr>
                <td style="width: 20%" class="Ecoheadtxt">Call DateTime</td>
                <td style="width: 1%" class="Ecoheadtxt">:</td>
                <td class="Ecoheadtxt" style="text-align: left">
                    <asp:Panel ID="pnlnextcall" runat="server" Width="380px"></asp:Panel>
                </td>
            </tr>
            <tr style="display: none;" class="Ecoheadtxt">
                <td style="width: 20%" class="Ecoheadtxt">Expected End DateTime</td>
                <td style="width: 1%" class="Ecoheadtxt">:</td>
                <td class="Ecoheadtxt" style="text-align: left">
                    <asp:Panel ID="pnlEndDate" runat="Server" Width="380px"></asp:Panel>
                </td>
            </tr>
            <tr>
                <td style="width: 20%" class="Ecoheadtxt">Insutruction</td>
                <td style="width: 1%" class="Ecoheadtxt">:</td>
                <td class="Ecoheadtxt" style="text-align: left">
                    <asp:TextBox ID="txtInsutruction" runat="Server" TextMode="MultiLine" Width="380px"></asp:TextBox></td>
            </tr>
            <tr>
                <td colspan="3" align="center">
                    <asp:Button ID="btnSave" runat="Server" CssClass="btnUpdate" Text="Forward/Deligate" OnClick="btnSave_Click" />
                    <input type="button" id="btnDiscard" name="btnDiscard" class="btnUpdate" value="Discard" onclick="window.close();" />
                </td>
            </tr>
        </table>

    </div>
</asp:Content>

