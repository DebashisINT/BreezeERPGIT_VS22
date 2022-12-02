<%@ Page Title="Carry Forward Of Balance" Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Management.DailyTask.management_DailyTask_CarryForwardLedger" CodeBehind="CarryForwardLedger.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   
    
    <script language="javascript" type="text/javascript">
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_initializeRequest(InitializeRequest);
        prm.add_endRequest(EndRequest);
        var postBackElement;
        function InitializeRequest(sender, args) {
            if (prm.get_isInAsyncPostBack())
                args.set_cancel(true);
            postBackElement = args.get_postBackElement();
            $get('UpdateProgress1').style.display = 'block';
        }
        function EndRequest(sender, args) {
            $get('UpdateProgress1').style.display = 'none';
            divscroll(sessionvalue);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <div class="panel-heading">
        <div class="panel-title">
            <h3>Carry Forward Of Balance</h3>
        </div>

    </div>
    <div class="form_main">
       <%-- <table class="TableMain100">
            <tr>
                <td class="EHEADER" style="text-align: center;">
                    <strong><span style="color: #000099">Carry Forward Of Balance</span></strong>
                </td>
            </tr>
        </table>--%>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <table class="TableMain100">
                    <tr>
                        <td align="">
                            <table>
                                <tr>
                                    <td colspan="2" style="text-align: left">
                                        <asp:Button ID="btnGenerate" runat="server" Text="Carry Forward" CssClass="btn btn-primary"
                                             OnClick="btnGenerate_Click" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
        <table>
            <tr>
                <td>
                    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
                        <ProgressTemplate>
                            <div id='Div1' style='position: absolute; font-family: arial; font-size: 30; left: 50%; top: 50%; background-color: white; layer-background-color: white; height: 80; width: 150;'>
                                <table width='100' height='35' border='1' cellpadding='0' cellspacing='0' bordercolor='#C0D6E4'>
                                    <tr>
                                        <td>
                                            <table>
                                                <tr>
                                                    <td height='25' align='center' bgcolor='#FFFFFF'>
                                                        <img src='/assests/images/progress.gif' width='18' height='18'></td>
                                                    <td height='10' width='100%' align='center' bgcolor='#FFFFFF'>
                                                        <font size='2' face='Tahoma'><strong align='center'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Loading..</strong></font></td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
