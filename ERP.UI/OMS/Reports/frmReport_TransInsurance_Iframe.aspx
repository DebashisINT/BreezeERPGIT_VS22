<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Reports.Reports_frmReport_TransInsurance_Iframe" Codebehind="frmReport_TransInsurance_Iframe.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <link href="../CSS/style.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript" src="/assests/js/init.js"></script>

    <script type="text/javascript" src="/assests/js/ajax-dynamic-list_rootfile.js"></script>

    <style type="text/css">
	
	/* Big box with list of options */
	#ajax_listOfOptions{
		position:absolute;	/* Never change this one */
		width:50px;	/* Width of box */
		height:auto;	/* Height of box */
		overflow:auto;	/* Scrolling features */
		border:1px solid Blue;	/* Blue border */
		background-color:#FFF;	/* White background color */
		text-align:left;
		font-size:0.9em;
		z-index:32767;
	}
	#ajax_listOfOptions div{	/* General rule for both .optionDiv and .optionDivSelected */
		margin:1px;		
		padding:1px;
		cursor:pointer;
		font-size:0.9em;
	}
	#ajax_listOfOptions .optionDiv{	/* Div for each item in list */
		
	}
	#ajax_listOfOptions .optionDivSelected{ /* Selected item in the list */
		background-color:#DDECFE;
		color:Blue;
	}
	#ajax_listOfOptions_iframe{
		background-color:#F00;
		position:absolute;
		z-index:3000;
	}
	
	form{
		display:inline;
	}
	
	</style>

    <script language="javascript" type="text/javascript">
        function PageLoad() {
            FieldName = 'lstSuscriptions';
            document.getElementById("Trfilter").style.display = 'none';
            height();
        }
        FieldName = 'lstSuscriptions';
        function showOptions(obj1, obj2, obj3) {
            //FieldName='lstSuscriptions';
            FieldName = 'A4';
            var cmb = document.getElementById("cmbsearchOption");
            var obj4 = cmb.value;
            if (cmb.value == 'Clients') {
                if (document.getElementById("rdbbSelected").checked == true) {
                    if (document.getElementById("hdBranch").value != '')
                        obj4 += '~' + document.getElementById("hdBranch").value;
                    else
                        document.getElementById("rdbbAll").checked = true;
                }
            }
            else if (cmb.value == 'Products') {
                if (document.getElementById("rdbInsuCompS").checked == true) {
                    if (document.getElementById("hdCompany").value != '')
                        obj4 += '~' + document.getElementById("hdCompany").value;
                    else
                        document.getElementById("rdbInsuCompA").checked = true;
                }
            }
            else if (cmb.value == 'Ins.Comp') {
                if (document.getElementById("cmbReportType").value == 'L')
                    obj4 += '~' + 'Insurance-Life';
                else if (document.getElementById("cmbReportType").value == 'G')
                    obj4 += '~' + 'Insurance-General';
                else if (document.getElementById("cmbReportType").value == 'B')
                    obj4 += '~' + 'Insurance-Both';
            }
            //            alert(obj4);    
            ajax_showOptions(obj1, obj2, obj3, obj4);
        }
        function ShowBankName(obj1, obj2, obj3) {
            ajax_showOptions(obj1, obj2, obj3);
        }
        function height() {
            if (document.body.scrollHeight >= 350) {
                window.frameElement.height = document.body.scrollHeight;
            }
            else {
                window.frameElement.height = '350px';
            }
            //window.frameElement.width = document.body.scrollWidth;
        }
        function btnAddsubscriptionlist_click() {
            var userid = document.getElementById('txtsubscriptionID');
            if (userid.value != '') {
                var ids = document.getElementById('txtsubscriptionID_hidden');
                var listBox = document.getElementById('lstSuscriptions');
                var tLength = listBox.length;
                //alert(tLength);

                var no = new Option();
                no.value = ids.value;
                no.text = userid.value;
                listBox[tLength] = no;
                var recipient = document.getElementById('txtsubscriptionID');
                recipient.value = '';
            }
            else
                alert('Please search name and then Add!');
            FocusFiter();
        }
        function FocusFiter() {
            var s = document.getElementById('txtsubscriptionID');
            s.focus();
            s.select();
        }
        function Focus(obj) {
            var s = document.getElementById(obj);
            s.focus();
            s.select();
        }
        function btnRemovefromsubscriptionlist_click() {
            var listBox = document.getElementById('lstSuscriptions');
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

        function SegAll(obj) {
            document.getElementById('showFilter').style.display = 'none';
        }
        function SegSelected(obj) {
            document.getElementById('showFilter').style.display = 'inline';
            document.getElementById('cmbsearchOption').value = obj;
            FocusFiter();
        }
        function ShowGrid(obj) {
            if (obj > 0) {
                document.getElementById("TrAll").style.display = 'none';
                document.getElementById("Trfilter").style.display = 'inline';
            }
            else
                alert('No data Found!');
            height();

        }
        function filter() {
            document.getElementById("Trfilter").style.display = 'none';
            document.getElementById("TrAll").style.display = 'inline';
            height();
        }


        function clientselectionfinal() {
            var listBoxSubs = document.getElementById('lstSuscriptions');
            var cmb = document.getElementById('cmbsearchOption');
            var listIDs = '';
            var i;
            var DataToHidden = '';
            if (listBoxSubs.length > 0) {
                for (i = 0; i < listBoxSubs.length; i++) {
                    if (listIDs == '') {
                        listIDs = listBoxSubs.options[i].value + ';' + listBoxSubs.options[i].text;
                        DataToHidden = '\'' + listBoxSubs.options[i].value + '\'';
                    }
                    else {
                        listIDs += ',' + listBoxSubs.options[i].value + ';' + listBoxSubs.options[i].text;
                        DataToHidden += ',\'' + listBoxSubs.options[i].value + '\'';
                    }
                }
                if (cmb.value == 'Ins.Comp') {
                    document.getElementById("hdCompany").value = DataToHidden;
                }
                if (cmb.value == 'Branch') {
                    document.getElementById("hdBranch").value = DataToHidden;
                }
                var sendData = cmb.value + '~' + listIDs;
                CallServer(sendData, "");
            }
            var i;
            for (i = listBoxSubs.options.length - 1; i >= 0; i--) {
                listBoxSubs.remove(i);
            }
            document.getElementById('showFilter').style.display = 'none';
            document.getElementById('Button1').disabled = false;
        }
        function FillValues() {
            var btn = document.getElementById('Button1');
            btn.click();
        }
        //         document.body.style.cursor = 'pointer'; 
        var oldColor = '';
        function ChangeRowColor(rowID, length) {
            //alert(rowID);
            var gridview = document.getElementById('grdCashBankBook');
            var rCount = gridview.rows.length;
            var rowIndex = 1;
            var len;
            if (length > 25)
                len = 3;
            else
                len = 2;
            for (rowIndex; rowIndex <= rCount - len; rowIndex++) {
                var rowElement = gridview.rows[rowIndex];
                rowElement.style.backgroundColor = '#FFFFFF'
            }
            var color = document.getElementById(rowID).style.backgroundColor;
            if (color != '#ffe1ac') {
                oldColor = color;
            }
            if (color == '#ffe1ac') {
                document.getElementById(rowID).style.backgroundColor = oldColor;
            }
            else
                document.getElementById(rowID).style.backgroundColor = '#ffe1ac';

        }

    </script>

    <script type="text/ecmascript">

        function ReceiveServerData(rValue) {
            var Data = rValue.split('~');

            if (Data[0] == 'Branch') {
                var combo = document.getElementById('litBranch');
                var NoItems = Data[1].split(',');
                var i;
                var val = '';
                for (i = 0; i < NoItems.length; i++) {
                    var items = NoItems[i].split(';');
                    //var items1=items[1].split('-');
                    if (val == '') {
                        val = '(' + items[1];
                    }
                    else {
                        val += ',' + items[1];
                    }
                }
                val = val + ')';
                combo.innerText = val;
                Focus('rdbClientA');
            }
            if (Data[0] == 'Clients') {
                var combo = document.getElementById('spnClient');
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
                combo.innerText = val;
                Focus('rdbInsuCompA');
            }
            if (Data[0] == 'InsuComp') {
                var combo = document.getElementById('spnInsuComp');
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
                combo.innerText = val;
                Focus('rdbProductA');
            }
            if (Data[0] == 'InsuComp') {
                var combo = document.getElementById('spnInsuComp');
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
                combo.innerText = val;
                Focus('rdbProductA');
            }
            if (Data[0] == 'Products') {
                var combo = document.getElementById('spnProduct');
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
                combo.innerText = val;
                Focus('rdbTelecallerA');
            }
            if (Data[0] == 'TeleCaller') {
                var combo = document.getElementById('spnTeleCaller');
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
                combo.innerText = val;
                Focus('rdbSalesRepresentativeA');
            }
            if (Data[0] == 'SaleRep') {
                var combo = document.getElementById('spnSalesRepresentative');
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
                combo.innerText = val;
                Focus('rdbAssociateA');
            }
            if (Data[0] == 'Associate') {
                var combo = document.getElementById('spnAssociate');
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
                combo.innerText = val;
                Focus('rdbSubBroakerA');
            }
            if (Data[0] == 'SubBroker') {
                var combo = document.getElementById('spnSubBroaker');
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
                combo.innerText = val;
                Focus('drpPolicyStatus');
            }
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <table class="TableMain100">
            <tr>
                <td class="EHEADER" colspan="2" style="text-align: center">
                    <span style="color: Blue"><strong>Reports Daily Transaction</strong></span>
                </td>
            </tr>
            <tr id="TrAll">
                <td style="text-align: left; vertical-align: top">
                    <table>
                        <tr>
                            <td class="gridcellleft">
                                <strong><span style="color: #000099">Report Type:</span></strong>
                            </td>
                            <td class="gridcellleft" colspan="2">
                                <asp:DropDownList ID="cmbReportType" runat="server" Font-Size="12px" TabIndex="1">
                                    <asp:ListItem Selected="True" Value="L">LI</asp:ListItem>
                                    <asp:ListItem Value="G">GI</asp:ListItem>
                                    <asp:ListItem Value="B">Both</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="gridcellleft">
                                <strong><span style="color: #000099">Transaction Date:</span></strong>
                            </td>
                            <td colspan="2" class="gridcellleft">
                                <table cellspacing="0px">
                                    <tr>
                                        <td>
                                            <dxe:ASPxDateEdit ID="dtDate" ClientInstanceName="dtDate" runat="server" EditFormat="Custom"
                                                UseMaskBehavior="True" TabIndex="1" Width="135px">
                                                <DropDownButton Text="From ">
                                                </DropDownButton>
                                            </dxe:ASPxDateEdit>
                                        </td>
                                        <td class="gridcellleft">
                                            <dxe:ASPxDateEdit ID="dtToDate" runat="server" EditFormat="Custom" UseMaskBehavior="True"
                                                TabIndex="2" Width="135px">
                                                <DropDownButton Text="To">
                                                </DropDownButton>
                                            </dxe:ASPxDateEdit>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="gridcellleft">
                                <strong><span style="color: #000099">Branch:</span></strong>
                            </td>
                            <td style="text-align: left;" colspan="2">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:RadioButton ID="rdbbAll" runat="server" Checked="True" GroupName="b" TabIndex="3" />
                                        </td>
                                        <td>
                                            All
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="rdbbSelected" runat="server" GroupName="b" />
                                        </td>
                                        <td>
                                            Selected
                                        </td>
                                        <td>
                                            <span id="litBranch" runat="server" style="color: Maroon"></span>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="gridcellleft">
                                <strong><span style="color: #000099">Client:</span></strong>
                            </td>
                            <td style="text-align: left;" colspan="2">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:RadioButton ID="rdbClientA" runat="server" Checked="True" GroupName="c" TabIndex="4" />
                                        </td>
                                        <td>
                                            All
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="rdbClientS" runat="server" GroupName="c" />
                                        </td>
                                        <td>
                                            Selected
                                        </td>
                                        <td>
                                            <span id="spnClient" runat="server" style="color: Maroon"></span>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="gridcellleft">
                                <strong><span style="color: #000099">Insurance Company:</span></strong>
                            </td>
                            <td style="text-align: left;" colspan="2">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:RadioButton ID="rdbInsuCompA" runat="server" Checked="True" GroupName="i" TabIndex="5" />
                                        </td>
                                        <td>
                                            All
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="rdbInsuCompS" runat="server" GroupName="i" />
                                        </td>
                                        <td>
                                            Selected
                                        </td>
                                        <td>
                                            <span id="spnInsuComp" runat="server" style="color: Maroon"></span>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="gridcellleft">
                                <strong><span style="color: #000099">Product:</span></strong>
                            </td>
                            <td style="text-align: left;" colspan="2">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:RadioButton ID="rdbProductA" runat="server" Checked="True" GroupName="p" TabIndex="6" />
                                        </td>
                                        <td>
                                            All
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="rdbProductS" runat="server" GroupName="p" />
                                        </td>
                                        <td>
                                            Selected
                                        </td>
                                        <td>
                                            <span id="spnProduct" runat="server" style="color: Maroon"></span>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="gridcellleft">
                                <strong><span style="color: #000099">Tele Caller:</span></strong>
                            </td>
                            <td style="text-align: left;" colspan="2">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:RadioButton ID="rdbTelecallerA" runat="server" Checked="True" GroupName="t"
                                                TabIndex="7" />
                                        </td>
                                        <td>
                                            All
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="rdbTelecallerS" runat="server" GroupName="t" />
                                        </td>
                                        <td>
                                            Selected
                                        </td>
                                        <td>
                                            <span id="spnTeleCaller" runat="server" style="color: Maroon"></span>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="gridcellleft">
                                <strong><span style="color: #000099">Sales Rep.:</span></strong>
                            </td>
                            <td style="text-align: left;" colspan="2">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:RadioButton ID="rdbSalesRepresentativeA" runat="server" Checked="True" GroupName="s"
                                                TabIndex="8" />
                                        </td>
                                        <td>
                                            All
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="rdbSalesRepresentativeS" runat="server" GroupName="s" />
                                        </td>
                                        <td>
                                            Selected
                                        </td>
                                        <td>
                                            <span id="spnSalesRepresentative" runat="server" style="color: Maroon"></span>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="gridcellleft">
                                <strong><span style="color: #000099">Associate:</span></strong>
                            </td>
                            <td style="text-align: left;" colspan="2">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:RadioButton ID="rdbAssociateA" runat="server" Checked="True" GroupName="a" TabIndex="9" />
                                        </td>
                                        <td>
                                            All
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="rdbAssociateS" runat="server" GroupName="a" />
                                        </td>
                                        <td>
                                            Selected
                                        </td>
                                        <td>
                                            <span id="spnAssociate" runat="server" style="color: Maroon"></span>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="gridcellleft">
                                <strong><span style="color: #000099">Sub Broaker:</span></strong>
                            </td>
                            <td style="text-align: left;" colspan="2">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:RadioButton ID="rdbSubBroakerA" runat="server" Checked="True" GroupName="sb"
                                                TabIndex="10" />
                                        </td>
                                        <td>
                                            All
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="rdbSubBroakerS" runat="server" GroupName="sb" />
                                        </td>
                                        <td>
                                            Selected
                                        </td>
                                        <td>
                                            <span id="spnSubBroaker" runat="server" style="color: Maroon"></span>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="gridcellleft">
                                <strong><span style="color: #000099">Policy Status:</span></strong>
                            </td>
                            <td class="gridcellleft">
                                <asp:DropDownList ID="drpPolicyStatus" runat="server" Width="179px" Font-Size="12px"
                                    TabIndex="11">
                                    <asp:ListItem Value="A">All</asp:ListItem>
                                    <asp:ListItem Value="0">Business in Hand</asp:ListItem>
                                    <asp:ListItem Value="1">Cancelled</asp:ListItem>
                                    <asp:ListItem Value="2">Canclled from inception</asp:ListItem>
                                    <asp:ListItem Value="3">Cheque Bounced</asp:ListItem>
                                    <asp:ListItem Value="4">Issued</asp:ListItem>
                                    <asp:ListItem Value="5">Lapsed</asp:ListItem>
                                    <asp:ListItem Value="6">Login</asp:ListItem>
                                </asp:DropDownList>&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:Button ID="Button1" runat="server" Text="Show" OnClick="Button1_Click" CssClass="btnUpdate"
                                    Height="19px" Width="101px" TabIndex="12" />
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="text-align: right; vertical-align: top;">
                    <table width="100%" id="showFilter" style="display: none;">
                        <tr>
                            <td style="text-align: left; vertical-align: top">
                                <table cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td style="vertical-align: top;">
                                            <table cellpadding="0px" cellspacing="0px">
                                                <tr>
                                                    <td>
                                                        <asp:TextBox ID="txtsubscriptionID" runat="server" Font-Size="12px" Width="150px"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="cmbsearchOption" runat="server" Font-Size="11px" Width="85px"
                                                            Enabled="false">
                                                            <asp:ListItem>Branch</asp:ListItem>
                                                            <asp:ListItem>Clients</asp:ListItem>
                                                            <asp:ListItem>Ins.Comp</asp:ListItem>
                                                            <asp:ListItem>Products</asp:ListItem>
                                                            <asp:ListItem>TeleCaller</asp:ListItem>
                                                            <asp:ListItem>Sales Rep.</asp:ListItem>
                                                            <asp:ListItem>Associate</asp:ListItem>
                                                            <asp:ListItem>Sub Broker</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <a id="A4" href="javascript:void(0);" onclick="btnAddsubscriptionlist_click()"><span
                                                            style="color: #009900; text-decoration: underline; font-size: 8pt;">Add to List</span></a><span
                                                                style="color: #009900; font-size: 8pt;"> </span>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <br />
                                            <asp:ListBox ID="lstSuscriptions" runat="server" Font-Size="12px" Height="90px" Width="253px">
                                            </asp:ListBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: center">
                                            <table cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td>
                                                        <a id="A2" href="javascript:void(0);" onclick="clientselectionfinal()"><span style="color: #000099;
                                                            text-decoration: underline; font-size: 10pt;">Done</span></a>&nbsp;&nbsp;
                                                    </td>
                                                    <td>
                                                        <a id="A1" href="javascript:void(0);" onclick="btnRemovefromsubscriptionlist_click()">
                                                            <span style="color: #cc3300; text-decoration: underline; font-size: 8pt;">Remove</span></a>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr style="display: none">
                            <td>
                                <asp:TextBox ID="txtsubscriptionID_hidden" runat="server" Font-Size="12px" Width="1px"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td class="gridcellleft" id="Trfilter">
                    <a href="#" style="font-weight: bold; color: Blue" onclick="javascript:filter();"><span
                        style="text-decoration: underline">Filter</span></a> &nbsp;| &nbsp;
                    <asp:LinkButton ID="Export" runat="server" Font-Bold="True" Font-Underline="True"
                        ForeColor="Blue" OnClick="Export_Click">Export to Excel</asp:LinkButton>
                </td>
            </tr>
            <tr>
                <td colspan="2" style="text-align: center">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Panel ID="Panel1" runat="server" Height="100%" Width="990px" ScrollBars="Horizontal">
                                <asp:GridView ID="grdCashBankBook" runat="server" Width="100%" BorderColor="CornflowerBlue"
                                    AllowSorting="true" ShowFooter="True" OnRowDataBound="grdCashBankBook_RowDataBound"
                                    AutoGenerateColumns="false" AllowPaging="True" BorderStyle="Solid" BorderWidth="2px"
                                    CellPadding="4" ForeColor="#0000C0" PageSize="25" OnRowCreated="grdCashBankBook_RowCreated"
                                    OnSorting="grdCashBankBook_Sorting" Font-Size="12px" EmptyDataText="No Data Found!"
                                    OnPageIndexChanging="grdCashBankBook_PageIndexChanging">
                                    <FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></FooterStyle>
                                    <Columns>
                                        <asp:TemplateField HeaderText="Tr. Date" SortExpression="transDate">
                                            <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                            <HeaderStyle Wrap="False" HorizontalAlign="Center" Font-Bold="False"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lbltransDateF" runat="server" Font-Size="12px" Text='<%# Eval("transDateF")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Issue Date" SortExpression="issueDate">
                                            <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                            <HeaderStyle Wrap="False" HorizontalAlign="Center" Font-Bold="False"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblissueDateF" runat="server" Font-Size="12px" Text='<%# Eval("issueDateF")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Customer" SortExpression="Customer">
                                            <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                            <HeaderStyle Wrap="False" HorizontalAlign="Center" Font-Bold="False"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblCustomer" runat="server" Font-Size="12px" Text='<%# Eval("Customer")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="branch" SortExpression="branch">
                                            <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                            <HeaderStyle Wrap="False" HorizontalAlign="Center" Font-Bold="False"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblbranch" runat="server" Font-Size="12px" Text='<%# Eval("branch")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="policy" SortExpression="policy">
                                            <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                            <HeaderStyle Wrap="False" HorizontalAlign="Center" Font-Bold="False"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblpolicy" runat="server" Font-Size="12px" Text='<%# Eval("policy")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Status" SortExpression="policy">
                                            <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                            <HeaderStyle Wrap="False" HorizontalAlign="Center" Font-Bold="False"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblpolicy" runat="server" Font-Size="12px" Text='<%# Eval("status")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="mode" SortExpression="mode">
                                            <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                            <HeaderStyle Wrap="False" HorizontalAlign="Center" Font-Bold="False"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblmode" runat="server" Font-Size="12px" Text='<%# Eval("mode")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="premiumAmt">
                                            <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="right"></ItemStyle>
                                            <HeaderStyle Wrap="False" HorizontalAlign="Center" Font-Bold="False"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblpremiumAmt" runat="server" Font-Size="12px" Text='<%# Eval("premiumAmt")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="WAPI">
                                            <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                            <HeaderStyle Wrap="False" HorizontalAlign="Center" Font-Bold="False"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblWAPI" runat="server" Font-Size="12px" Text='<%# Eval("WAPI")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="TeleCaller">
                                            <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                            <HeaderStyle Wrap="False" HorizontalAlign="Center" Font-Bold="False"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblTeleCaller" runat="server" Font-Size="12px" Text='<%# Eval("TeleCaller")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Sales Rep.">
                                            <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                            <HeaderStyle Wrap="False" HorizontalAlign="Center" Font-Bold="False"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblSalesRep" runat="server" Font-Size="12px" Text='<%# Eval("SalesRep")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="SubBroker">
                                            <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                            <HeaderStyle Wrap="False" HorizontalAlign="Center" Font-Bold="False"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblSubBroker" runat="server" Font-Size="12px" Text='<%# Eval("SubBroker")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Associates">
                                            <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                            <HeaderStyle Wrap="False" HorizontalAlign="Center" Font-Bold="False"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblAssociates" runat="server" Font-Size="12px" Text='<%# Eval("Associates")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <PagerSettings FirstPageImageUrl="~/images/pFirst.png" LastPageImageUrl="~/images/pLast.png"
                                        Mode="NextPreviousFirstLast" NextPageImageUrl="~/images/pNext.png" PreviousPageImageUrl="~/images/pPrev.png" />
                                    <RowStyle BackColor="White" ForeColor="#330099" BorderColor="#BFD3EE" BorderStyle="Double"
                                        BorderWidth="1px" Font-Size="12px"></RowStyle>
                                    <EditRowStyle BackColor="#E59930"></EditRowStyle>
                                    <SelectedRowStyle BackColor="#D1DDF1" ForeColor="#333333" Font-Bold="True"></SelectedRowStyle>
                                    <PagerStyle ForeColor="White" HorizontalAlign="Center"></PagerStyle>
                                    <HeaderStyle ForeColor="Black" BorderWidth="1px" CssClass="EHEADER" BorderColor="AliceBlue"
                                        Font-Bold="False"></HeaderStyle>
                                </asp:GridView>
                                <asp:HiddenField ID="CurrentPage" runat="server"></asp:HiddenField>
                                <asp:HiddenField ID="TotalPages" runat="server"></asp:HiddenField>
                            </asp:Panel>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click"></asp:AsyncPostBackTrigger>
                        </Triggers>
                    </asp:UpdatePanel>
                    <asp:HiddenField ID="hdBranch" runat="server" />
                    <asp:HiddenField ID="hdCompany" runat="server" />
                </td>
            </tr>
        </table>
</asp:Content>
