<%@ Page Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" Inherits="ERP.OMS.Reports.Reports_SRVTAXSTATEMENTDP" CodeBehind="SRVTAXSTATEMENTDP.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.2, Version=10.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>--%>

    <link href="../CSS/style.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="/assests/js/init.js"></script>
    <script type="text/javascript" src="/assests/js/ajaxList_rootfile.js"></script>
    
    <style type="text/css">
        /* Big box with list of options */
        #ajax_listOfOptions {
            position: absolute; /* Never change this one */
            width: 50px; /* Width of box */
            height: auto; /* Height of box */
            overflow: auto; /* Scrolling features */
            border: 1px solid Blue; /* Blue border */
            background-color: #FFF; /* White background color */
            text-align: left;
            font-size: 0.9em;
            z-index: 32767;
        }


            #ajax_listOfOptions div { /* General rule for both .optionDiv and .optionDivSelected */
                margin: 1px;
                padding: 1px;
                cursor: pointer;
                font-size: 0.9em;
            }

            #ajax_listOfOptions .optionDiv { /* Div for each item in list */
            }

            #ajax_listOfOptions .optionDivSelected { /* Selected item in the list */
                background-color: #DDECFE;
                color: Blue;
            }

        #ajax_listOfOptions_iframe {
            background-color: #F00;
            position: absolute;
            z-index: 3000;
        }

        form {
            display: inline;
        }

        .grid_scroll {
            overflow-y: no;
            overflow-x: scroll;
            width: 90%;
            scrollbar-base-color: #C0C0C0;
        }

        .tableBorderClass {
            /* border: 0px; */
            border: 1px solid #aaa !important;
        }
    </style>
    <script language="javascript" type="text/javascript">
        function Page_Load()///Call Into Page Load
        {
            Hide('showFilter');
            Hide('DATADISPLAY');
            Hide('tr_filter');
            document.getElementById('hiddencount').value = 0;
            height();
        }
        function Hide(obj) {
            document.getElementById(obj).style.display = 'none';
        }
        function Show(obj) {
            document.getElementById(obj).style.display = 'inline';
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

        function Branch(obj) {
            if (obj == "a")
                Hide('showFilter');
            else {
                var cmb = document.getElementById('cmbsearchOption');
                cmb.value = 'Branch';
                Show('showFilter');
            }
            selecttion();
        }
        function Segment(obj) {
            if (obj == "a") {
                Hide('showFilter');
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
            document.getElementById('btn_show').disabled = false;
            Hide('UpdatePanel4');
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

        function selecttion() {
            var combo = document.getElementById('ddlExport');
            combo.value = 'Ex';
        }
        function FunClientScrip(objID, objListFun, objEvent) {
            cmbVal = document.getElementById('cmbsearchOption').value + '~' + '0';
            ajax_showOptions(objID, objListFun, objEvent, cmbVal);
        }
        function NORECORD(obj) {

            Show('tabdisplay');
            Hide('showFilter');
            Hide('DATADISPLAY');
            Hide('tr_filter');
            if (obj == '1') {
                alert('No Record Found');
            }
            height();
        }
        function Display() {
            Hide('tabdisplay');
            Hide('showFilter');
            Show('DATADISPLAY');
            Show('tr_filter');
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
        FieldName = 'lstSlection';
    </script>
    <script type="text/ecmascript">

        function ReceiveServerData(rValue) {
            var Data = rValue.split('~');
            if (Data[0] == 'Segment') {

                var combo = document.getElementById('litSegmentMain');
                var NoItems = Data[1].split(',');
                var i;
                var val = '';
                for (i = 0; i < NoItems.length; i++) {
                    var items = NoItems[i].split(';');
                    if (val == '') {
                        val = items[1];
                    }
                    else {
                        val += ',' + items[1];
                    }
                }
                combo.innerText = '[' + val + ']';
                document.getElementById('HiddenField_Segment').value = Data[2];
            }
            if (Data[0] == 'Branch') {
                document.getElementById('HiddenField_Branch').value = Data[2];
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <table class="TableMain100">
            <tr>
                <td class="EHEADER" colspan="0" style="text-align: center; height: 22px;">
                    <strong><span id="SpanHeader" style="color: #000099">Service Tax Statement</span></strong>
                </td>
                <td class="EHEADER" width="25%" id="tr_filter" style="height: 22px">
                    <a href="javascript:void(0);" onclick="NORECORD(2);"><span style="color: Blue; text-decoration: underline; font-size: 8pt; font-weight: bold">Filter</span></a> &nbsp; &nbsp;||&nbsp;
                        <asp:DropDownList ID="ddlExport" Font-Size="Smaller" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlExport_SelectedIndexChanged">
                            <asp:ListItem Selected="True" Value="Ex">Export</asp:ListItem>
                            <asp:ListItem Value="E">Excel</asp:ListItem>
                            <asp:ListItem Value="P">PDF</asp:ListItem>
                        </asp:DropDownList>
                </td>
            </tr>
        </table>
        <table>
            <tr valign="top">
                <td>
                    <table id="tabdisplay" cellpadding="0" cellspacing="0" class="tableBorderClass">
                        <tr valign="top">
                            <td class="gridcellleft" bgcolor="#B7CEEC" style="height: 25px;">Period :</td>
                            <td>
                                <dxe:ASPxDateEdit ID="dtfrom" runat="server" EditFormat="Custom" UseMaskBehavior="True"
                                    Font-Size="12px" Width="110px" ClientInstanceName="dtfor">
                                    <DropDownButton Text="For"></DropDownButton>
                                </dxe:ASPxDateEdit>
                            </td>
                            <td>
                                <dxe:ASPxDateEdit ID="dtto" runat="server" EditFormat="Custom" UseMaskBehavior="True"
                                    Font-Size="12px" Width="110px" ClientInstanceName="dtFrom">
                                    <DropDownButton Text="From"></DropDownButton>
                                </dxe:ASPxDateEdit>
                            </td>
                        </tr>
                        <tr valign="top">
                            <td class="gridcellleft" bgcolor="#B7CEEC" style="height: 25px;">Report Type :
                            </td>
                            <td colspan="2" style="height: 25px;">
                                <asp:DropDownList ID="cmbreporttype" runat="server" Width="100px" Font-Size="12px" Style="height: 25px;">
                                    <asp:ListItem Value="Date Wise">Date Wise</asp:ListItem>
                                    <asp:ListItem Value="Month Wise">Month Wise</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="gridcellleft" bgcolor="#B7CEEC" style="height: 25px;">Segment :</td>
                            <td>
                                <asp:RadioButton ID="rdbSegmentAll" runat="server" GroupName="b" onclick="Segment('a')" />All
                            </td>
                            <td style="width: 151px" style="height: 25px;">
                                <asp:RadioButton ID="rdbSegmentSelected" runat="server" Checked="True" GroupName="b"
                                    onclick="Segment('b')" />Selected <span id="litSegmentMain" runat="server" style="color: Maroon"></span>
                            </td>
                        </tr>
                        <tr>
                            <td class="gridcellleft" bgcolor="#B7CEEC" style="height: 25px;">Branch :</td>
                            <td>
                                <asp:RadioButton ID="rdbBrtanchAll" runat="server" Checked="True" GroupName="c" onclick="Branch('a')" />
                                All
                            </td>
                            <td style="width: 151px">
                                <asp:RadioButton ID="rdbBrtanchSelected" runat="server" GroupName="c" onclick="Branch('b')" />Selected
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4" class="gridcellleft" style="height: 25px;">
                                <asp:Button ID="btn_show" runat="server" CssClass="btnUpdate" Height="20px" Text="Show"
                                    Width="101px" OnClientClick="selecttion()" OnClick="btn_show_Click" />
                            </td>
                        </tr>
                    </table>
                </td>
                <td>
                    <table id="showFilter" width="100%">
                        <tr>
                            <td style="vertical-align: top; height: 134px; text-align: right">
                                <table cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td id="TdFilter" class="gridcellleft" style="vertical-align: top; height: 21px; text-align: right">
                                            <asp:TextBox ID="txtSelectionID" runat="server" Font-Size="12px" onkeyup="FunClientScrip(this,'ShowClientFORMarginStocks',event)"
                                                Width="150px"></asp:TextBox>
                                            <asp:DropDownList ID="cmbsearchOption" runat="server" Enabled="false" Font-Size="11px"
                                                Width="70px">
                                                <asp:ListItem>Segment</asp:ListItem>
                                                <asp:ListItem>Branch</asp:ListItem>
                                            </asp:DropDownList>
                                            <a id="A4" href="javascript:void(0);" onclick="btnAddsubscriptionlist_click()"><span
                                                style="font-size: 8pt; color: #009900; text-decoration: underline">Add to List</span></a><span
                                                    style="font-size: 8pt; color: #009900"> </span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:ListBox ID="lstSlection" runat="server" Font-Size="12px" Height="90px" Width="290px"></asp:ListBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: center">
                                            <table cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td>
                                                        <a id="A2" href="javascript:void(0);" onclick="clientselectionfinal()"><span style="font-size: 10pt; color: #000099; text-decoration: underline">Done</span></a> &nbsp;
                                                    </td>
                                                    <td>
                                                        <a id="A1" href="javascript:void(0);" onclick="btnRemovefromsubscriptionlist_click()">
                                                            <span style="font-size: 8pt; color: #cc3300; text-decoration: underline">Remove</span></a>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <table id="tab3">
            <tr>
                <td style="display: none;">
                    <asp:TextBox ID="txtSelectionID_hidden" runat="server" Font-Size="12px" Width="150px"></asp:TextBox>
                    <asp:HiddenField ID="hiddencount" runat="server" />
                    <asp:HiddenField ID="HiddenField_Segment" runat="server" />
                    <asp:HiddenField ID="HiddenField_Branch" runat="server" />
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



    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <table id="DATADISPLAY">

                <tr>
                    <td colspan="2">
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
</asp:Content>

