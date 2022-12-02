<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master" Inherits="ERP.OMS.Reports.Reports_ApprovedIlliquidSecurities" CodeBehind="ApprovedIlliquidSecurities.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script language="javascript" type="text/javascript">


        function Page_Load()///Call Into Page Load
        {
            Hide('Td_Filter');
            FnddlGeneration('Screen');
        }
        function SignOff() {
            window.parent.SignOff();
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
        function FnReportView(obj) {
            if (obj == 'Illiquid') {
                Show('Tr_Illiquid');
                Hide('Tr_Approved');
            }
            if (obj == 'Approved') {
                Hide('Tr_Illiquid');
                Show('Tr_Approved');
            }
        }
        function FnddlGeneration(obj) {
            if (obj == 'Screen') {
                Show('td_Screen');
                Hide('td_Export');
            }
            if (obj == 'Export') {
                Hide('td_Screen');
                Show('td_Export');
            }
            FnReportView(document.getElementById('DdlRptView').value);
            height();
        }
        function fnNoRecord(obj) {
            if (obj == '1' || obj == '2') {
                Show('TabSelection');
                Hide('DivDisplay');
                Hide('Td_Filter');
                FnddlGeneration(document.getElementById('ddlGeneration').value);
                if (obj == '1')
                    alert('No Record Found !!');
            }
            if (obj == '3') {
                Hide('TabSelection');
                Show('DivDisplay');
                Show('Td_Filter');
            }
            document.getElementById('hiddencount').value = 0;
            height();
        }
        function heightlight(obj) {

            var colorcode = obj.split('&');

            if ((document.getElementById('hiddencount').value) == 0) {
                prevobj = '';
                prevcolor = '';
                document.getElementById('hiddencount').value = 1;
            }
            document.getElementById(obj).style.backgroundColor = '#ffe1ac';

            if (prevobj != '') {
                document.getElementById(prevobj).style.backgroundColor = prevcolor;
            }
            prevobj = obj;
            prevcolor = colorcode[1];

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
                <td class="EHEADER" colspan="0" style="text-align: center; height: 20px;">
                    <strong><span id="SpanHeader" style="color: #000099">Approved / Illiquid Securities</span></strong></td>

                <td class="EHEADER" width="15%" id="Td_Filter" style="height: 22px">
                    <a href="javascript:void(0);" onclick="fnNoRecord('2');"><span style="color: Blue; text-decoration: underline; font-size: 8pt; font-weight: bold">Filter</span></a>
                    <asp:DropDownList ID="cmbExport" runat="server" AutoPostBack="True" Font-Size="12px" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged">
                        <asp:ListItem Selected="True" Value="Ex">Export</asp:ListItem>
                        <asp:ListItem Value="E">Excel</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
        </table>

        <table border="0" cellpadding="0" cellspacing="0" id="TabSelection">
            <tr>
                <td class="gridcellleft">
                    <table>
                        <tr>
                            <td>
                                <table class="tableClass" cellpadding="1" cellspacing="1">
                                    <tr valign="top">
                                        <td class="gridcellleft" bgcolor="#B7CEEC">Report View :</td>
                                        <td>
                                            <asp:DropDownList ID="DdlRptView" runat="server" Width="150px" Font-Size="12px" onchange="FnReportView(this.value)">
                                                <asp:ListItem Value="Approved">Approved Securities</asp:ListItem>
                                                <asp:ListItem Value="Illiquid">Illiquid Securities</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr id="Tr_Approved">
                <td class="gridcellleft">
                    <table>
                        <tr>
                            <td>
                                <table class="tableClass" cellpadding="1" cellspacing="1">
                                    <tr valign="top">
                                        <td class="gridcellleft" bgcolor="#B7CEEC">As On Date
                                        </td>
                                        <td>
                                            <dxe:ASPxDateEdit ID="DtDate" runat="server" EditFormat="Custom" UseMaskBehavior="True"
                                                Font-Size="12px" Width="108px" ClientInstanceName="DtDate">
                                                <DropDownButton Text="Date">
                                                </DropDownButton>
                                            </dxe:ASPxDateEdit>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr id="Tr_Illiquid">
                <td class="gridcellleft">
                    <table>
                        <tr>
                            <td>
                                <table class="tableClass" cellpadding="1" cellspacing="1">
                                    <tr valign="top">
                                        <td class="gridcellleft" bgcolor="#B7CEEC">Year :
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="DdlYear" runat="server" Width="150px" Font-Size="12px">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td valign="top">
                                <table class="tableClass" cellpadding="1" cellspacing="1">
                                    <tr valign="top">
                                        <td class="gridcellleft" bgcolor="#B7CEEC">Month :</td>
                                        <td>
                                            <asp:DropDownList ID="ddlMonth" runat="server" Width="150px" Font-Size="12px">
                                                <asp:ListItem Value="1">January</asp:ListItem>
                                                <asp:ListItem Value="2">February</asp:ListItem>
                                                <asp:ListItem Value="3">March</asp:ListItem>
                                                <asp:ListItem Value="4">April</asp:ListItem>
                                                <asp:ListItem Value="5">May</asp:ListItem>
                                                <asp:ListItem Value="6">June</asp:ListItem>
                                                <asp:ListItem Value="7">July</asp:ListItem>
                                                <asp:ListItem Value="8">Auguest</asp:ListItem>
                                                <asp:ListItem Value="9">September</asp:ListItem>
                                                <asp:ListItem Value="10">October</asp:ListItem>
                                                <asp:ListItem Value="11">November</asp:ListItem>
                                                <asp:ListItem Value="12">December</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td class="gridcellleft">
                    <table>
                        <tr>
                            <td>
                                <table class="tableClass" cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">Generate Type :</td>
                                        <td>
                                            <asp:DropDownList ID="ddlGeneration" runat="server" Width="100px" Font-Size="12px"
                                                onchange="FnddlGeneration(this.value)">
                                                <asp:ListItem Value="Screen">Screen</asp:ListItem>
                                                <asp:ListItem Value="Export">Export</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>

            <tr>
                <td class="gridcellleft">
                    <table>
                        <tr>
                            <td id="td_Screen">
                                <asp:Button ID="BtnScreen" runat="server" CssClass="btnUpdate" Height="20px" Text="Screen"
                                    Width="101px" OnClientClick="selecttion()" OnClick="BtnScreen_Click" />
                            </td>
                            <td id="td_Export">
                                <asp:Button ID="BtnExcel" runat="server" CssClass="btnUpdate" Height="20px" Text="Export To Excel"
                                    Width="101px" OnClientClick="selecttion()" OnClick="BtnExcel_Click" /></td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <table>
            <tr>
                <td style="display: none;">
                    <asp:HiddenField ID="hiddencount" runat="server" />
                </td>
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
                                                        <font size='1' face='Tahoma'><strong align='center'>Please Wait..</strong></font></td>
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
        <div id="DivDisplay" style="display: none;" width="100%">
            <asp:UpdatePanel runat="server" ID="UpdatePanel1">
                <ContentTemplate>
                    <table width="100%" border="1">

                        <tr>
                            <td>
                                <div id="DivHeader" runat="server">
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div id="Divdisplay" runat="server">
                                </div>
                            </td>
                        </tr>

                    </table>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="BtnScreen" EventName="Click"></asp:AsyncPostBackTrigger>
                </Triggers>
            </asp:UpdatePanel>
        </div>

    </div>
</asp:Content>