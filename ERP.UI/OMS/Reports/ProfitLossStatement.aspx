<%@ Page Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" Inherits="ERP.OMS.Reports.Reports_ProfitLossStatement" CodeBehind="ProfitLossStatement.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../CSS/style.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript" src="/assests/js/init.js"></script>

    <script type="text/javascript" src="/assests/js/ajax-dynamic-list_rootfile.js"></script>

    


    <script type="text/javascript">


        function Page_Load()///Call Into Page Load
        {
            Hide('showFilter');
            Hide('td_filter');
            document.getElementById('hiddencount').value = 0;
            FnddlGeneration('1');
            height();
        }
        function height() {
            if (document.body.scrollHeight >= 350) {
                window.frameElement.height = document.body.scrollHeight;
            }
            else {
                window.frameElement.height = '350px';
            }
            window.frameElement.Width = document.body.scrollWidth;
        }
        function Hide(obj) {
            document.getElementById(obj).style.display = 'none';
        }
        function Show(obj) {
            document.getElementById(obj).style.display = 'block';
        }

        function FunClientScrip(objID, objListFun, objEvent) {
            if (document.getElementById('cmbsearchOption').value == "Company") {
                ajax_showOptions(objID, "Company", objEvent);
            }
            else {
                ajax_showOptions(objID, objListFun, objEvent, document.getElementById('cmbsearchOption').value);
            }

        }


        function fnBranch(obj) {
            if (obj == "a")
                Hide('showFilter');
            else {
                document.getElementById('cmbsearchOption').value = 'Branch';
                Show('showFilter');
            }
            selecttion();
        }
        function fnCompany(obj) {
            if (obj == "a")
                Hide('showFilter');
            else {
                Show('showFilter');
                document.getElementById('cmbsearchOption').value = 'Company';
            }
            selecttion();
        }
        function fnSegment(obj) {
            if (obj == "a")
                Hide('showFilter');
            else if (obj == "c") {
                Hide('showFilter');
                Show('Td_Specific');
            }
            else {
                var cmb = document.getElementById('cmbsearchOption');
                cmb.value = 'Segment';
                Show('showFilter');
            }
            selecttion();
        }
        function btnAddsubscriptionlist_click() {

            var cmb = document.getElementById('cmbsearchOption');
            var userid = document.getElementById('txtSelectionID');
            if (userid.value != '') {
                var ids = document.getElementById('txtSelectionID_hidden');
                var listBox = document.getElementById('lstSlection');
                var tLength = listBox.length;


                var no = new Option();
                no.value = ids.value;
                no.text = userid.value;
                listBox[tLength] = no;
                var recipient = document.getElementById('txtSelectionID');
                recipient.value = '';
            }
            else
                alert('Please search name and then Add!')
            var s = document.getElementById('txtSelectionID');
            s.focus();
            s.select();

        }

        function clientselectionfinal() {
            var listBoxSubs = document.getElementById('lstSlection');

            var cmb = document.getElementById('cmbsearchOption');
            var listIDs = '';
            var i;
            if (listBoxSubs.length > 0) {

                for (i = 0; i < listBoxSubs.length; i++) {
                    if (listIDs == '')
                        listIDs = listBoxSubs.options[i].value + ';' + listBoxSubs.options[i].text;
                    else
                        listIDs += ',' + listBoxSubs.options[i].value + ';' + listBoxSubs.options[i].text;
                }
                var sendData = cmb.value + '~' + listIDs;
                CallServer(sendData, "");

            }
            var i;
            for (i = listBoxSubs.options.length - 1; i >= 0; i--) {
                listBoxSubs.remove(i);
            }

            Hide('showFilter');
            document.getElementById('btnScreen').disabled = false;
        }


        function btnRemovefromsubscriptionlist_click() {

            var listBox = document.getElementById('lstSlection');
            var tLength = listBox.length;

            var arrTbox = new Array();
            var arrLookup = new Array();
            var i;
            var j = 0;
            for (i = 0; i < listBox.options.length; i++) {
                if (listBox.options[i].selected && listBox.options[i].value != "") {

                }
                else {
                    arrLookup[listBox.options[i].text] = listBox.options[i].value;
                    arrTbox[j] = listBox.options[i].text;
                    j++;
                }
            }
            listBox.length = 0;
            for (i = 0; i < j; i++) {
                var no = new Option();
                no.value = arrLookup[arrTbox[i]];
                no.text = arrTbox[i];
                listBox[i] = no;
            }
        }



        function fnRecord(obj) {
            if (obj == "1") {
                Hide('td_filter');
                Show('tab1');
                Hide('displayAll');
                Hide('showFilter');
                alert('No Record Found!!');
            }
            if (obj == "2") {
                Show('td_filter');
                Hide('tab1');
                Show('displayAll');
                Hide('showFilter');

            }
            if (obj == "3") {
                Hide('td_filter');
                Show('tab1');
                Hide('displayAll');
                Hide('showFilter');

            }
            document.getElementById('hiddencount').value = 0;

            height();
            selecttion();
        }

        function FnddlGeneration(obj) {
            if (obj == "1") {
                Show('td_Screen');
                Hide('td_Export');
            }
            if (obj == "2") {
                Hide('td_Screen');
                Show('td_Export');
            }
            Hide('showFilter');
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
        function selecttion() {
            var combo = document.getElementById('cmbExport');
            combo.value = 'Ex';
        }

        FieldName = 'lstSlection';
    </script>



    <script type="text/ecmascript">
        function ReceiveServerData(rValue) {
            var j = rValue.split('~');
            if (j[0] == 'Branch') {
                document.getElementById('HiddenField_Branch').value = j[1];
                document.getElementById('HiddenField_BranchName').value = j[2];
            }
            if (j[0] == 'Segment') {
                document.getElementById('HiddenField_Segment').value = j[1];
                document.getElementById('HiddenField_SegmentName').value = j[2];
            }
            if (j[0] == 'Company') {
                document.getElementById('HiddenField_Company').value = j[1];
                document.getElementById('HiddenField_CompanyName').value = j[2];
            }

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
    <style>
        .bacgrnded {
            padding: 13px 7px;
            background: #c5d3da;
            border-radius: 5px;
            margin-bottom: 7px;
        }
        .padTable td{
            padding:3px 0
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title">
            <h3>Profit & Loss Statement</h3>
        </div>
    </div>
    <div class="form_main">
        <table class="TableMain100">
            <tr>
                <%--<td class="EHEADER" colspan="0" style="text-align: center; height: 20px;">
                    <strong><span id="SpanHeader" style="color: #000099">Profit & Loss Statement</span></strong></td>--%>

                <td class="EHEADER" width="15%" id="td_filter" style="height: 22px">
                    <a href="javascript:void(0);" onclick="fnRecord(3);"><span style="color: Blue; text-decoration: underline; font-size: 8pt; font-weight: bold">Filter</span></a>
                    <asp:DropDownList ID="cmbExport" runat="server" AutoPostBack="True" Font-Size="12px" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged">
                        <asp:ListItem Selected="True" Value="Ex">Export</asp:ListItem>
                        <asp:ListItem Value="E">Excel</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
        </table>

        <table id="tab1" border="0" cellpadding="0" cellspacing="0">
            <tr valign="top">
                <td>
                    <table class="padTable">
                        <tr>
                            <td class="gridcellleft" style="width:120px">For The Period :
                            </td>
                            <td class="gridcellleft">
                                <dxe:ASPxDateEdit ID="DtFrom" runat="server" EditFormat="Custom" UseMaskBehavior="True"
                                    Font-Size="12px" Width="115px" ClientInstanceName="DtFrom">
                                    <DropDownButton Text="From">
                                    </DropDownButton>
                                </dxe:ASPxDateEdit>
                            </td>
                            <td class="gridcellleft">
                                <dxe:ASPxDateEdit ID="DtTo" runat="server" EditFormat="Custom" UseMaskBehavior="True"
                                    Font-Size="12px" Width="115px" ClientInstanceName="DtTo">
                                    <DropDownButton Text="To">
                                    </DropDownButton>
                                </dxe:ASPxDateEdit>
                            </td>
                        </tr>

                        <tr id="Tr_Company">
                            <td class="gridcellleft" >Company :</td>
                            <td colspan="2">
                                <table width="180px">
                                    <tr>
                                        <td>
                                            <asp:RadioButton ID="RdbAllCompany" runat="server" GroupName="a" onclick="fnCompany('a')" />
                                            All
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="RdbCurrentCompany" runat="server" Checked="True" GroupName="a"
                                            onclick="fnCompany('a')" />
                                            Current
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="RdbSelectedCompany" runat="server" GroupName="a" onclick="fnCompany('b')" />Selected
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>

                        <tr>
                            <td class="gridcellleft" >Segment :</td>
                            <td colspan="2">
                                <table style=" width: 180px;">
                                    <tr>
                                        <td>
                                            <asp:RadioButton ID="rdbSegmentAll" runat="server" GroupName="b" onclick="fnSegment('a')" />All
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="rdbSegmentSpecific" runat="server" Checked="True" GroupName="b"
                                                onclick="fnSegment('c')" />Specific
                                        </td>
                                        <td>[ <span id="litSegmentMain" runat="server" style="color: Maroon"></span>]</td>
                                        <td id="Td_SegmentSelected">
                                            <asp:RadioButton ID="rdSegmentSelected" runat="server" GroupName="b" onclick="fnSegment('b')" />Selected
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="gridcellleft" >Branch :</td>
                            <td colspan="2">
                                <table style=" width: 120px">
                                    <tr>
                                        <td>
                                            <asp:RadioButton ID="RdBranchAll" runat="server" Checked="True" GroupName="c" onclick="fnBranch('a')" />
                                            All
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="RdBranchSelected" runat="server" GroupName="c" onclick="fnBranch('b')" />Selected
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="gridcellleft" >Report View :</td>
                            <td colspan="2">
                                <asp:DropDownList ID="DdlrptView" runat="server" Width="250px" Font-Size="12px">
                                    <asp:ListItem Value="1">Company Wise</asp:ListItem>
                                    <asp:ListItem Value="2">Consolidated-Company Wise</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="gridcellleft" >Report Style :</td>
                            <td colspan="2">
                                <asp:DropDownList ID="DdlrptStyle" runat="server" Width="250px" Font-Size="12px">
                                    <asp:ListItem Value="1">Account Head Wise</asp:ListItem>
                                    <asp:ListItem Value="2">Account Group Wise</asp:ListItem>

                                </asp:DropDownList>
                            </td>
                        </tr>


                        <tr>
                            <td colspan="3">
                                <table  style=" width: 100%;">
                                    <tr>
                                        <td class="gridcellleft" >
                                            <asp:CheckBox ID="ChkMonthlyBreakUp" runat="server" />
                                            Show Monthly BreakUp
                                        </td>
                                        <td class="gridcellleft" >
                                            <asp:CheckBox ID="ChkZeroAmntAc" runat="server" />
                                            Show Zero Amount Account
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="gridcellleft" >Generate Type :</td>
                            <td colspan="2">
                                <asp:DropDownList ID="ddlGeneration" runat="server" Width="250px" Font-Size="12px"
                                    onchange="FnddlGeneration(this.value)">
                                    <asp:ListItem Value="1">Screen</asp:ListItem>
                                    <asp:ListItem Value="2">Export</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>

                        <tr>
                            <td></td>
                            <td colspan="2">
                                <span id="td_Screen">
                                    <asp:Button ID="btnScreen" runat="server" CssClass="btnUpdate btn btn-primary"  Text="Screen"
                                         OnClientClick="selecttion()" OnClick="btnScreen_Click" />
                                </span>
                                <span id="td_Export">
                                    <asp:Button ID="btnExcel" runat="server" CssClass="btnUpdate btn btn-primary" Text="Export To Excel"
                                        OnClientClick="selecttion()" OnClick="btnExcel_Click" /></span>
                            </td>
                        </tr>

                    </table>
                </td>
                <td>
                    <div class="bacgrnded" id="showFilter">
                    <table cellpadding="1" cellspacing="1" >
                        <tr>
                            <td class="gridcellleft" style="vertical-align: top; text-align: right; height: 21px;"
                                id="TdFilter">
                                <asp:TextBox ID="txtSelectionID" runat="server" Font-Size="12px" Width="150px" onkeyup="FunClientScrip(this,'ShowClientFORMarginStocks',event)"></asp:TextBox>
                                
                            </td>
                            <td>
                                <asp:DropDownList ID="cmbsearchOption" runat="server" Font-Size="11px" Width="70px"
                                    Enabled="false">

                                    <asp:ListItem>Branch</asp:ListItem>

                                    <asp:ListItem>Segment</asp:ListItem>
                                    <asp:ListItem>Company</asp:ListItem>



                                </asp:DropDownList>
                                <a id="A4" href="javascript:void(0);" onclick="btnAddsubscriptionlist_click()"><span
                                    style="color: #009900; text-decoration: underline; font-size: 8pt;">Add to List</span></a><span
                                        style="color: #009900; font-size: 8pt;"> </span>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:ListBox ID="lstSlection" runat="server" Font-Size="12px" Height="90px" Width="100%"></asp:ListBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <table cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <a id="A2" href="javascript:void(0);" onclick="clientselectionfinal()" class="btn btn-primary btn-small"><span>Done</span></a>&nbsp;&nbsp;
                                        </td>
                                        <td>
                                            <a id="A1" href="javascript:void(0);" onclick="btnRemovefromsubscriptionlist_click()" class="btn btn-danger btn-small">
                                                <span>Remove</span></a>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                  </div>
                </td>
            </tr>
        </table>
        <table>
            <tr>
                <td style="display: none;">
                    <asp:TextBox ID="txtSelectionID_hidden" runat="server" Font-Size="12px" Width="150px"></asp:TextBox>

                    <asp:HiddenField ID="HiddenField_Branch" runat="server" />
                    <asp:HiddenField ID="HiddenField_BranchName" runat="server" />
                    <asp:HiddenField ID="hiddencount" runat="server" />
                    <asp:HiddenField ID="HiddenField_Segment" runat="server" />
                    <asp:HiddenField ID="HiddenField_SegmentName" runat="server" />
                    <asp:HiddenField ID="HiddenField_CompanyName" runat="server" />
                    <asp:HiddenField ID="HiddenField_Company" runat="server" />
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
        <div id="displayAll" style="display: none;" width="100%">
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
                    <asp:AsyncPostBackTrigger ControlID="btnScreen" EventName="Click"></asp:AsyncPostBackTrigger>
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>
