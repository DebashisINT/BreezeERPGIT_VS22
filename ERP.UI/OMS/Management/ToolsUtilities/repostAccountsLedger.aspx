<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Management.ToolsUtilities.management_toolsutilities_repostAccountsLedger" CodeBehind="repostAccountsLedger.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script language="javascript" type="text/javascript">
        //function SignOff() {
        //    window.parent.SignOff();
        //}
        //function height() {
        //    if (document.body.scrollHeight >= 500)
        //        window.frameElement.height = document.body.scrollHeight;
        //    else
        //        window.frameElement.height = '500px';
        //    window.frameElement.Width = document.body.scrollWidth;
        //}
        function Page_Load() {
            document.getElementById('Div1').style.display = "none";
        }
        function btn_Click() {
            document.getElementById('Div1').style.display = "inline";
            combo.PerformCallback();
        }
        function ShowError(obj) {
            document.getElementById('Div1').style.display = "none";
            if (obj == "b") {
                alert('Accounts Ledger Repost !!');
            }
            else {
                alert('No Data In This Company And Segment !!');
            }

        }
    </script>
    <div class="panel-heading">
        <div class="panel-title">
            <h3>Reposts Accounts Ledger</h3>
        </div>
    </div>
    <div class="form_main">
        <table class="TableMain100">
            <%--<tr>
                    <td class="EHEADER" style="text-align: center;">
                        <strong><span style="color: #000099">Reposts Accounts Ledger</span></strong>
                    </td>
                </tr>--%>
        </table>
        <div id='Div1' style='position: absolute; font-family: arial; font-size: 30; left: 40%; top: 25%; background-color: white; layer-background-color: white; height: 80; width: 150;'>
            <table class="TableMain100">
                <tr>
                    <td>
                        <table>
                            <tr>
                                <td height='25' align='center' bgcolor='#FFFFFF'>
                                    <img src='/assests/images/progress.gif' width='18' height='18'></td>
                                <td height='10' width='100%' align='center' bgcolor='#FFFFFF'><font size='2' face='Tahoma'> 
 	                        <strong align='center'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Please Wait..</strong></font></td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
        <table class="TableMain100">
            <tr>
                <td align="center">
                    <input id="Button1" type="button" value="Repost Accounts Ledger" onclick="btn_Click()" class="btnUpdate btn btn-primary" style="width: 385px" />
                </td>
            </tr>
            <tr style="display: none">
                <td>
                    <dxe:ASPxComboBox ID="ASPxComboBox1" ClientInstanceName="combo" runat="server" OnCallback="ASPxComboBox1_Callback" OnCustomJSProperties="ASPxComboBox1_CustomJSProperties">
                        <ClientSideEvents EndCallback="function(s,e) { ShowError(s.cpInsertError); }" />
                    </dxe:ASPxComboBox>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
