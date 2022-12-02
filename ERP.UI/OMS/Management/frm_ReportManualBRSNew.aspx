<%@ Page Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true"
    Inherits="ERP.OMS.Management.Reports_frm_ReportManualBRSNew" CodeBehind="frm_ReportManualBRSNew.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--<%@ Register Assembly="DevExpress.Web.v15.1" Namespace="DevExpress.Web"
    TagPrefix="dxe000001" %>--%>
    <%@ Register Assembly="CrystalDecisions.Web, Version=10.2.3600.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
        Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

    <script type="text/javascript">


        function Page_Load()///Call Into Page Load
        {
            Hide('td_filter');
            Hide('showFilter');
            FnddlGeneration('1');
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
        function RecordDisplay() {
            Hide('showFilter');
            Show('td_filter');
            Hide('tab1');
            Show('displayAll');
            document.getElementById('hiddencount').value = 0;
            selecttion();
            height();

        }
        function fnNoRecord(obj) {
            Hide('showFilter');
            Hide('td_filter');
            Show('tab1');
            Hide('displayAll');
            document.getElementById('hiddencount').value = 0;
            if (obj == '1')
                alert('No Record Found!!');
            if (obj == '3')
                alert('Please Select Bank Name!!');
            height();
            selecttion();

        }

        function FnddlGeneration(obj) {
            if (obj == "1") {
                Show('td_Screen');
                Hide('td_Export');
                Hide('td_pdf');

            }
            if (obj == "2") {
                Hide('td_Screen');
                Show('td_Export');
                Hide('td_pdf');
            }
            if (obj == "3") {
                Hide('td_Screen');
                Hide('td_Export');
                Show('td_pdf');
            }
            Hide('showFilter');
        }
        function selecttion() {
            var combo = document.getElementById('cmbExport');
            combo.value = 'Ex';
        }
        function btnAddsubscriptionlist_click() {

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
            var listIDs = '';
            var i;
            if (listBoxSubs.length > 0) {

                for (i = 0; i < listBoxSubs.length; i++) {
                    if (listIDs == '')
                        listIDs = listBoxSubs.options[i].value + ';' + listBoxSubs.options[i].text;
                    else
                        listIDs += ',' + listBoxSubs.options[i].value + ';' + listBoxSubs.options[i].text;
                }

                var sendData = listIDs;
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
        function fnBank(obj) {
            if (obj == "a")
                Hide('showFilter');
            else {
                Show('showFilter');
                document.getElementById('cmbsearchOption').value = 'Bank';
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
        function CallBankAccount(obj1, obj2, obj3) {
            var CurrentSegment = '<%=Session["usersegid"]%>'
            var strPutSegment = " and (MainAccount_ExchangeSegment=" + CurrentSegment + " or MainAccount_ExchangeSegment=0)";
            var strQuery_Table = "(Select MainAccount_AccountCode+\'-\'+MainAccount_Name+\' [ \'+MainAccount_BankAcNumber+\' ]\' as IntegrateMainAccount,MainAccount_AccountCode as MainAccount_AccountCode,MainAccount_Name,MainAccount_BankAcNumber from Master_MainAccount where (MainAccount_BankCashType=\'Bank\' or MainAccount_BankCashType=\'Cash\') and (MainAccount_BankCompany=\'" + '<%=Session["LastCompany"] %>' + "\' Or IsNull(MainAccount_BankCompany,'')='')" + strPutSegment + ") as t1";
            var strQuery_FieldName = " Top 10 * ";
            var strQuery_WhereClause = "MainAccount_AccountCode like (\'%RequestLetter%\') or MainAccount_Name like (\'%RequestLetter%\') or MainAccount_BankAcNumber like (\'%RequestLetter%\')";
            var strQuery_OrderBy = '';
            var strQuery_GroupBy = '';
            var CombinedQuery = new String(strQuery_Table + "$" + strQuery_FieldName + "$" + strQuery_WhereClause + "$" + strQuery_OrderBy + "$" + strQuery_GroupBy);
            ajax_showOptions(obj1, obj2, obj3, replaceChars(CombinedQuery), 'Main');
        }
        function replaceChars(entry) {
            out = "+"; // replace this
            add = "--"; // with this
            temp = "" + entry; // temporary holder

            while (temp.indexOf(out) > -1) {
                pos = temp.indexOf(out);
                temp = "" + (temp.substring(0, pos) + add +
                temp.substring((pos + out.length), temp.length));
            }
            return temp;

        }
        function FnCallajax(objID, objEvent) {
            var cmbVal = document.getElementById('cmbsearchOption').value;
            if (cmbVal == "Company") {
                ajax_showOptions(objID, "Company", objEvent);
            }
            if (cmbVal == "Bank") {
                if (document.getElementById('RdbAllCompany').checked) {
                    ajax_showOptions(objID, "SearchBankNameFromMainAccountForBrs", objEvent, "ALL");
                }
                else if (document.getElementById('RdbCurrentCompany').checked) {
                    ajax_showOptions(objID, "SearchBankNameFromMainAccountForBrs", objEvent, "Current");
                }
                else {
                    ajax_showOptions(objID, "SearchBankNameFromMainAccountForBrs", objEvent, document.getElementById('HiddenField_Company').value);
                }

            }
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
        FieldName = 'lstSlection';
    </script>

    <script type="text/ecmascript">
        function ReceiveServerData(rValue) {

            if (document.getElementById('cmbsearchOption').value == 'Bank') {
                document.getElementById('HiddenField_BRSAC').value = rValue;
            }
            if (document.getElementById('cmbsearchOption').value == 'Company') {
                document.getElementById('HiddenField_Company').value = rValue;
            }
        }
    </script>

    <script type="text/javascript">
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
    <div class="panel-heading">
        <div class="panel-title">
            <h3>Bank Reconciliation Statement</h3>
        </div>
    </div>
    <div class="form_main">
        <table class="TableMain100">
            <tr>
                <td class="EHEADER" id="td_filter">
                    <a href="javascript:void(0);" class="btn btn-primary" onclick="fnNoRecord(2);"><span>Filter</span></a>
                    <asp:DropDownList ID="cmbExport" runat="server" AutoPostBack="True" CssClass="form-control"
                        OnSelectedIndexChanged="cmbExport_SelectedIndexChanged">
                        <asp:ListItem Selected="True" Value="Ex">Export</asp:ListItem>
                        <asp:ListItem Value="E">Excel</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
        <table class="TableMain100">
            <tr>
                <td>
                    <table id="tab1" cellspacing="1" cellpadding="2">
                        <tr>
                            <td class="gridcellleft">Company :</td>
                            <td>
                                <asp:RadioButton ID="RdbAllCompany" runat="server" GroupName="dd" onclick="fnCompany('a')" />
                                All
                                     <asp:RadioButton ID="RdbCurrentCompany" runat="server" Checked="True" GroupName="dd" onclick="fnCompany('a')" />
                                Current
                                    <asp:RadioButton ID="RdbSelectedCompany" runat="server" GroupName="dd" onclick="fnCompany('b')" />Selected
                            </td>
                        </tr>
                        <tr>
                            <td class="gridcellleft">Report View :</td>
                            <td>
                                <asp:DropDownList ID="DdlReportView" runat="server" Width="100px" Font-Size="12px">
                                    <asp:ListItem Value="1">Detail</asp:ListItem>
                                    <asp:ListItem Value="2">Summary</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>

                        <tr id="Tr_BankSummary">
                            <td class="gridcellleft">Bank Name :</td>
                            <td>
                                <asp:RadioButton ID="rdbBankAll" runat="server" Checked="True" GroupName="d" onclick="fnBank('a')" />
                                All
                                    <asp:RadioButton ID="rdBankSelected" runat="server" GroupName="d" onclick="fnBank('b')" />Selected
                            </td>
                        </tr>
                        <tr>
                            <td class="gridcellleft">As On Date
                            </td>
                            <td>
                                <dxe:ASPxDateEdit ID="DtAsOn" runat="server" EditFormat="Custom" UseMaskBehavior="True"
                                    Font-Size="12px" Width="120px" ClientInstanceName="DtAsOn">
                                    <DropDownButton Text="Date">
                                    </DropDownButton>
                                </dxe:ASPxDateEdit>
                            </td>
                        </tr>
                        <tr>
                            <td class="gridcellleft">Consider :</td>
                            <td>
                                <asp:DropDownList ID="DdlConsider" runat="server" CssClass="form-control">
                                    <asp:ListItem Value="1">Value Date</asp:ListItem>
                                    <asp:ListItem Value="2">Statement Date</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="gridcellleft" bgcolor="#B7CEEC">Generate Type :</td>
                            <td>
                                <asp:DropDownList ID="ddlGeneration" runat="server" CssClass="form-control"
                                    onchange="FnddlGeneration(this.value)">
                                    <asp:ListItem Value="1">Screen</asp:ListItem>
                                    <asp:ListItem Value="2">Export</asp:ListItem>
                                    <asp:ListItem Value="3">PDF</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td id="td_Screen">
                                <asp:Button ID="btnScreen" runat="server" CssClass="btn btn-primary" Text="Show"
                                    OnClientClick="selecttion()" OnClick="btnScreen_Click" />
                            </td>
                            <td id="td_Export">
                                <asp:Button ID="btnExcel" runat="server" CssClass="btn btn-warning" Text="Export To Excel"
                                    OnClientClick="selecttion()" OnClick="btnExcel_Click" /></td>
                            <td id="td_pdf">
                                <asp:Button ID="btnpdf" runat="server" CssClass="btn btn-warning" Text="Export to PDF"
                                    nClientClick="selecttion()" OnClick="btnpdf_Click" /></td>
                        </tr>
                        <tr>
                            <td style="display: none;" colspan="2">
                                <asp:TextBox ID="txtSelectionID_hidden" runat="server" Font-Size="12px" Width="150px"></asp:TextBox>
                                <asp:HiddenField ID="HiddenField_BRSAC" runat="server" />
                                <asp:HiddenField ID="HiddenField_Company" runat="server" />
                            </td>
                        </tr>
                    </table>
                </td>
                <td>
                    <table cellpadding="1" cellspacing="1" id="showFilter">
                        <tr>
                            <td>
                                <asp:TextBox ID="txtSelectionID" runat="server" CssClass="form-control" onkeyup="FnCallajax(this,event)"></asp:TextBox></td>
                            <td>
                                <asp:DropDownList ID="cmbsearchOption" runat="server" CssClass="form-control"
                                    Enabled="false">
                                    <asp:ListItem Value="Company">Company</asp:ListItem>
                                    <asp:ListItem Value="Bank">Bank</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <a id="A4" class="btn btn-success" href="javascript:void(0);" onclick="btnAddsubscriptionlist_click()"><span>Add to List</span></a><span> </span>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <asp:ListBox ID="lstSlection" runat="server" Font-Size="12px" Height="120px" Width="400px"></asp:ListBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: center">
                                <table cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <a id="A2" class="btn btn-primary" href="javascript:void(0);" onclick="clientselectionfinal()"><span>Done</span></a>&nbsp;&nbsp;
                                        </td>
                                        <td>
                                            <a id="A1" class="btn btn-danger" href="javascript:void(0);" onclick="btnRemovefromsubscriptionlist_click()">
                                                <span>Remove</span></a>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
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
                        <tr style="display: none;">
                            <td>
                                <asp:HiddenField ID="hiddencount" runat="server" />
                                <asp:DropDownList ID="cmbrecord" runat="server" Font-Size="12px" Width="300px" AutoPostBack="True">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div id="DivHeader" runat="server">
                                </div>
                            </td>
                        </tr>
                        <tr bordercolor="Blue" id="tr_prvnxt">
                            <td align="center">
                                <table id="tblpage" cellspacing="0" cellpadding="0" runat="server" width="100%">
                                    <tr>
                                        <td width="20" style="padding: 5px">
                                            <asp:LinkButton ID="ASPxFirst" runat="server" Font-Bold="True" ForeColor="maroon"
                                                OnClientClick="javascript:selecttion();;" OnClick="ASPxFirst_Click">First</asp:LinkButton></td>
                                        <td width="25">
                                            <asp:LinkButton ID="ASPxPrevious" runat="server" Font-Bold="True" ForeColor="Blue"
                                                OnClientClick="javascript:selecttion();" OnClick="ASPxPrevious_Click">Previous</asp:LinkButton></td>
                                        <td width="20" style="padding: 5px">
                                            <asp:LinkButton ID="ASPxNext" runat="server" Font-Bold="True" ForeColor="maroon"
                                                OnClientClick="javascript:selecttion();" OnClick="ASPxNext_Click">Next</asp:LinkButton></td>
                                        <td width="20">
                                            <asp:LinkButton ID="ASPxLast" runat="server" Font-Bold="True" ForeColor="Blue" OnClientClick="javascript:selecttion();"
                                                OnClick="ASPxLast_Click">Last</asp:LinkButton></td>
                                        <td align="right">
                                            <asp:Label ID="listRecord" runat="server" Font-Bold="True"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
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

