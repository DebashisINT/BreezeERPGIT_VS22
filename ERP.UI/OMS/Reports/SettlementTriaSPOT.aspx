<%@ Page Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" Inherits="ERP.OMS.Reports.Reports_SettlementTriaSPOT" CodeBehind="SettlementTriaSPOT.aspx.cs" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.2, Version=10.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>--%>
    <link type="text/css" href="../CSS/style.css" rel="Stylesheet" />
    <script type="text/javascript" src="/assests/js/loaddata1.js"></script>

    <script type="text/javascript" src="/assests/js/init.js"></script>
    <script type="text/javascript" src="/assests/js/ajax-dynamic-list_rootfile.js"></script>

    <script language="javascript" type="text/javascript">
        function Page_Load()///Call Into Page Load
        {
            Hide('displayAll');
            document.getElementById('hiddencount').value = 0;
            height();
        }
        function height() {
            if (document.body.scrollHeight >= 350) {
                window.frameElement.height = document.body.scrollHeight;
            }
            else {
                window.frameElement.height = '350px';
            }
            window.frameElement.width = document.body.scrollwidth;
        }
        function Hide(obj) {
            document.getElementById(obj).style.display = 'none';
        }
        function Show(obj) {
            document.getElementById(obj).style.display = 'inline';
        }
        function display() {
            Show('displayAll');
            Hide('TrAll1');
            Hide('TrAll');
            height();
        }
        function norecord(obj) {
            Hide('displayAll');
            Show('TrAll1');
            Show('TrAll');
            if (obj == '1')
                alert('Rates For This Date Does Not Exists!');
            if (obj == '2')
                alert('No Record Found');
            height();
        }
    </script>
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

        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <table class="TableMain100">
            <tr>
                <td class="EHEADER" style="text-align: center;">
                    <strong><span style="color: #000099">Settlement Trial </span></strong>
                </td>
                <td class="EHEADER" width="15%" id="tr_filter" style="height: 22px">
                    <a href="javascript:void(0);" onclick="norecord(3);"><span style="color: Blue; text-decoration: underline; font-size: 8pt; font-weight: bold">Filter</span></a>
                    <asp:DropDownList ID="cmbExport" runat="server" AutoPostBack="True" Font-Size="12px" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged">
                        <asp:ListItem Selected="True" Value="Ex">Export</asp:ListItem>
                        <asp:ListItem Value="E">Excel</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
        <table class="TableMain100" id="TABLE1">
            <tr id="TrAll1">
                <td>
                    <table border="0">
                        <tr valign="top">
                            <td class="gridcellleft">For :
                            </td>
                            <td class="gridcellleft">
                                <dxe:ASPxDateEdit ID="dtdate" runat="server" EditFormat="Custom" UseMaskBehavior="True"
                                    Font-Size="12px" Width="108px" ClientInstanceName="dtdate">
                                    <DropDownButton Text="For">
                                    </DropDownButton>
                                </dxe:ASPxDateEdit>
                            </td>
                            <td class="gridcellleft">
                                <asp:DropDownList ID="ddlType" runat="server" Width="154px" Font-Size="12px">
                                    <asp:ListItem Value="0">Client Wise </asp:ListItem>
                                    <%-- <asp:ListItem Value="1">Branch Wise</asp:ListItem>--%>
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr id="TrAll">
                <td style="text-align: left; vertical-align: top; width: 953px;">
                    <asp:Button ID="btn_show" runat="server" Text="Show" CssClass="btnUpdate" Height="23px"
                        Width="101px" OnClick="btn_show_Click" />
                </td>
            </tr>
            <tr style="display: none;">
                <td>
                    <asp:HiddenField ID="hiddencount" runat="server" />
                    <asp:DropDownList ID="cmb" runat="server" Font-Size="12px" Width="300px" AutoPostBack="True">
                    </asp:DropDownList></td>
            </tr>
        </table>


    </div>
    <div id="displayAll">
        <asp:UpdatePanel runat="server" ID="u1">
            <ContentTemplate>
                <table width="100%" border="0">

                    <tr>
                        <td>
                            <div id="display" runat="server">
                            </div>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btn_show" EventName="Click"></asp:AsyncPostBackTrigger>
            </Triggers>
        </asp:UpdatePanel>
    </div>
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="u1">
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
</asp:Content>


