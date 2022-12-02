<%@ Page Language="C#" AutoEventWireup="true"  MasterPageFile="~/OMS/MasterPage/ERP.Master" Inherits="ERP.OMS.Reports.frmReport_IframeLedgerViewSingleClient" Codebehind="frmReport_IframeLedgerViewSingleClient.aspx.cs" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <link type="text/css" href="../CSS/style.css" rel="Stylesheet" />

    <script type="text/javascript" src="/assests/js/loaddata1.js"></script>

    <script type="text/javascript" src="/assests/js/init.js"></script>

    <script type="text/javascript" src="/assests/js/ajaxList_rootfile.js"></script>

    

     <script type="text/javascript">
         $(document).ready(function () {

             $(".water").each(function () {
                 if ($(this).val() == this.title) {
                     $(this).addClass("opaque");
                 }
             });

             $(".water").focus(function () {
                 if ($(this).val() == this.title) {
                     $(this).val("");
                     $(this).removeClass("opaque");
                 }
             });

             $(".water").blur(function () {
                 if ($.trim($(this).val()) == "") {
                     $(this).val(this.title);
                     $(this).addClass("opaque");
                 }
                 else {
                     $(this).removeClass("opaque");
                 }
             });
         });

    </script>
   

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
        function ForFilterOff() {
            document.getElementById("TrAll").style.display = 'none';
            document.getElementById("TdAll1").style.display = 'none';
            document.getElementById("TrBtn").style.display = 'none';
            document.getElementById('spanBtn').style.display = 'none';
            height();
        }
        function MailsendT() {
            alert("Mail Sent Successfully");
        }
        function MailsendF() {
            alert("Error on sending!Try again..");
        }
        function SignOff() {
            window.parent.SignOff();
        }
        FieldName = 'lstSuscriptions';
        function showOptions(obj1, obj2, obj3) {
            var cmb = document.getElementById('cmbsearchOption');
            //alert(cmb.value);
            ajax_showOptions(obj1, obj2, obj3, cmb.value, 'Sub');
        }
        function showOptions1(obj1, obj2, obj3) {
            ajax_showOptions(obj1, obj2, obj3, 'Ac Name1', 'Main');
        }
        function showOptionsforSunAc(obj1, obj2, obj3) {
            var cmb = document.getElementById('ddlAccountType');
            var litMain = document.getElementById('litAcMain');
            if (cmb.value != '3') {
                ajax_showOptions(obj1, obj2, obj3, '0', 'Sub');
            }
            else {
                var valMainAc1 = document.getElementById('txtMainAccount_hidden').value;
                var valMainAc2 = valMainAc1.split('~');
                var valMainAc = "('" + valMainAc2[0] + "')";
                ajax_showOptions(obj1, obj2, obj3, valMainAc, 'Sub');
            }
        }
        function keyVal(obj) {
            var obj1 = obj.split('~');
            if (obj1[1] == 'None') {
                //document.getElementById('TrSubAccount').style.display='none';
                dateTimeForSubledger = 'a';
            }
            else {
                // document.getElementById('TrSubAccount').style.display='inline';
                dateTimeForSubledger = 'b';
            }
            //            var btn = document.getElementById('ButtonUpdate');
            //            btn.click();
        }
        function height() {
            if (document.body.scrollHeight >= 500) {
                window.frameElement.height = document.body.scrollHeight;
            }
            else {
                window.frameElement.height = '500';
            }
            window.frameElement.widht = document.body.scrollWidht;
        }
        function ShowHide(obj) {
            //document.getElementById('Div1').style.display="none";

            document.getElementById("TrAll").style.display = 'none';
            document.getElementById("TdAll1").style.display = 'none';
            document.getElementById("TrBtn").style.display = 'none';
            document.getElementById('TrForFilter').style.display = 'inline';
            document.getElementById('showDetail').style.display = 'inline';
            document.getElementById('spanshow3').style.display = 'inline';
            document.getElementById('spanshow3').innerText = obj;

            height();
        }
        function ForFilter() {
            document.getElementById("TrAll").style.display = 'inline';
            document.getElementById("TdAll1").style.display = 'inline';
            document.getElementById("TrBtn").style.display = 'inline';
            document.getElementById('TrForFilter').style.display = 'none';

            document.getElementById('spanBtn').style.display = 'inline';
            document.getElementById('TrPrevNext').style.display = 'none';
            height();
        }
        function ChangeAccountType() {

            var AcVal = document.getElementById('ddlAccountType').value;

            if (AcVal == '3') {
                document.getElementById('TdAc').style.display = 'inline';
                document.getElementById('TdAc1').style.display = 'inline';
            }
            else {
                document.getElementById('TdAc').style.display = 'none';
                document.getElementById('TdAc1').style.display = 'none';
            }

            document.getElementById('txtMainAccount_hidden').value = '';
            document.getElementById('txtMainAccount').value = '';


        }
        function Page_Load() {

            document.getElementById('showFilter').style.display = 'none';
            document.getElementById('TdFilter').style.display = 'none';

            document.getElementById("TrAll").style.display = 'none';
            document.getElementById("TrBtn").style.display = 'none';
            document.getElementById('showDetail').style.display = 'inline';
            document.getElementById('spanshow3').style.display = 'inline';
            //            document.getElementById('spanshow3').innerText='ttt';

            dateTimeForSubledger = 'b';

            height();

        }


        function btnAddsubscriptionlist_click() {
            var cmb = document.getElementById('cmbsearchOption');
            if (cmb.value != 'Sub Ac') {
                var userid = document.getElementById('txtsubscriptionID');
                if (userid.value != '') {
                    var ids = document.getElementById('txtsubscriptionID_hidden');
                    var listBox = document.getElementById('lstSuscriptions');
                    var tLength = listBox.length;

                    var no = new Option();
                    no.value = ids.value;
                    no.text = userid.value;
                    listBox[tLength] = no;
                    var recipient = document.getElementById('txtsubscriptionID');
                    recipient.value = '';
                }
                else
                    alert('Please search name and then Add!')
                var s = document.getElementById('txtsubscriptionID');
                s.focus();
                s.select();
                var AcVal = document.getElementById('ddlAccountType').value;
                if (AcVal == '3')
                    clientselectionfinal();
            }
            else {
                var userid = document.getElementById('txtSubsubcriptionID');
                if (userid.value != '') {
                    var ids = document.getElementById('txtSubsubcriptionID_hidden');
                    var listBox = document.getElementById('lstSuscriptions');
                    var tLength = listBox.length;

                    var no = new Option();
                    no.value = ids.value;
                    no.text = userid.value;
                    listBox[tLength] = no;
                    var recipient = document.getElementById('txtSubsubcriptionID');
                    recipient.value = '';
                }
                else
                    alert('Please search name and then Add!')
                var s = document.getElementById('txtSubsubcriptionID');
                s.focus();
                s.select();
            }
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
        function clientselectionfinal() {
            var listBoxSubs = document.getElementById('lstSuscriptions');
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
            document.getElementById('showFilter').style.display = 'none';
            document.getElementById('TdFilter').style.display = 'none';
            document.getElementById('Button1').disabled = false;
        }
        function Disable(obj) {
            var gridview = document.getElementById('grdCashBankBook');
            var rCount = gridview.rows.length;
            if (rCount < 10)
                rCount = '0' + rCount;
            if (obj == 'P') {
                document.getElementById("grdCashBankBook_ctl09_FirstPage").style.display = 'none';
                document.getElementById("grdCashBankBook_ctl09_PreviousPage").style.display = 'none';
                document.getElementById("grdCashBankBook_ctl09_NextPage").style.display = 'inline';
                document.getElementById("grdCashBankBook_ctl09_LastPage").style.display = 'inline';
            }
            else {
                document.getElementById("grdCashBankBook_ctl" + rCount + "_NextPage").style.display = 'none';
                document.getElementById("grdCashBankBook_ctl" + rCount + "_LastPage").style.display = 'none';
            }
        }


        function btnCancel_Click() {
            document.getElementById("TrAll").style.display = 'none';
            document.getElementById("TdAll1").style.display = 'none';
            document.getElementById("TrBtn").style.display = 'none';
            document.getElementById('TrForFilter').style.display = 'inline';
            document.getElementById('showDetail').style.display = 'inline';
            document.getElementById('spanBtn').style.display = 'none';
            height();
        }
        function alertMessage() {
            //document.getElementById('Div1').style.display="none";
            alert('No Matching Clients !');
            document.getElementById('spanBtn').style.display = 'none';
            document.getElementById('TrForFilter').style.display = 'none';
            document.getElementById('showDetail').style.display = 'none';
            document.getElementById('TrPrevNext').style.display = 'none';
            document.getElementById('HdnSelectLedger').value = 'P';
            height();
        }

        function dateChange() {
            //            var SubChk=document.getElementById('rdSubAcAll').checked;
            //            if(SubChk==true)
            //            {
            //                if(dateTimeForSubledger!='a')
            //                {
            //                    var dtFrom1=Date.parse(dtFrom.GetDate());
            //                    var dtTo1=Date.parse(dtTo.GetDate());
            //                    var dateDiff=(dtTo1-dtFrom1)/(24*60*60*1000)
            //                    if(dateDiff>30)
            //                    {
            //                        alert('You can view ledger for all accounts only for a period of 30 days.\n Please Select another date range !!');
            //                        var datePost=(dtFrom.GetDate().getMonth()+2)+'-'+dtFrom.GetDate().getDate()+'-'+dtFrom.GetDate().getYear();
            //                        dtTo.SetDate(new Date(datePost));
            //                    }
            //                }
            //            }
            //document.getElementById('Button1').disabled=true;
            //            var btn = document.getElementById('ButtonUpdate');
            //            btn.click();
        }
        document.body.style.cursor = 'pointer';
        var oldColor = '';
        function ChangeRowColor(rowID, rowNumber) {
            var gridview = document.getElementById('grdCashBankBook');
            var rCount = gridview.rows.length;
            var rowIndex = 1;
            var rowCount = 0;
            if (rCount == 28)
                rowCount = 25;
            else
                rowCount = rCount - 2;
            if (rowNumber > 25 && rCount < 28)
                rowCount = rCount - 3;
            for (rowIndex; rowIndex <= rowCount; rowIndex++) {
                var rowElement = gridview.rows[rowIndex];
                rowElement.style.backgroundColor = '#FFFFFF';
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
        function HideoffOnButton() {

            document.getElementById('Button1').disabled = false;
        }
        function HideOnOffLoading() {
            //document.getElementById('Div1').style.display="inline";
        }
        function DisabledDrp(obj) {
            if (obj == 'a')
                document.getElementById('TrPrevNext').style.display = "none";
            else
                document.getElementById('TrPrevNext').style.display = "inline";
            //            var valMainAc=document.getElementById('txtMainAccount_hidden');
            //            var valMainAc1=document.getElementById('txtMainAccount');
            //            valMainAc.value="";
            //            valMainAc1.value="";
        }
        function selecttion() {
            var combo = document.getElementById('ddlExport');
            combo.value = 'Ex';
        }
        function DateChangeForFrom() {
            //var datePost=(dtFrom.GetDate().getMonth()+2)+'-'+dtFrom.GetDate().getDate()+'-'+dtFrom.GetDate().getYear();
            var sessionVal = "<%#Session["LastFinYear"]%>";
            var objsession = sessionVal.split('-');
            var MonthDate = dtFrom.GetDate().getMonth() + 1;
            var DayDate = dtFrom.GetDate().getDate();
            var YearDate = dtFrom.GetDate().getYear();
            if (YearDate >= objsession[0]) {
                if (MonthDate < 4 && YearDate == objsession[0]) {
                    alert('Enter Date Is Outside Of Financial Year !!');
                    var datePost = (4 + '-' + 1 + '-' + objsession[0]);
                    dtFrom.SetDate(new Date(datePost));
                }
                else if (MonthDate > 3 && YearDate == objsession[1]) {
                    alert('Enter Date Is Outside Of Financial Year !!');
                    var datePost = (4 + '-' + 1 + '-' + objsession[0]);
                    dtFrom.SetDate(new Date(datePost));
                }
                else if (YearDate != objsession[0] && YearDate != objsession[1]) {
                    alert('Enter Date Is Outside Of Financial Year !!');
                    var datePost = (4 + '-' + 1 + '-' + objsession[0]);
                    dtFrom.SetDate(new Date(datePost));
                }
            }
            else {
                alert('Enter Date Is Outside Of Financial Year !!');
                var datePost = (4 + '-' + 1 + '-' + objsession[0]);
                dtFrom.SetDate(new Date(datePost));
            }
        }
        function DateChangeForTo() {
            var sessionVal = "<%#Session["LastFinYear"]%>";
        var objsession = sessionVal.split('-');
        var MonthDate = dtTo.GetDate().getMonth() + 1;
        var DayDate = dtTo.GetDate().getDate();
        var YearDate = dtTo.GetDate().getYear();

        if (YearDate <= objsession[1]) {
            if (MonthDate < 4 && YearDate == objsession[0]) {
                alert('Enter Date Is Outside Of Financial Year !!');
                var datePost = (3 + '-' + 31 + '-' + objsession[1]);
                dtTo.SetDate(new Date(datePost));
            }
            else if (MonthDate > 3 && YearDate == objsession[1]) {
                alert('Enter Date Is Outside Of Financial Year !!');
                var datePost = (3 + '-' + 31 + '-' + objsession[1]);
                dtTo.SetDate(new Date(datePost));
            }
            else if (YearDate != objsession[0] && YearDate != objsession[1]) {
                alert('Enter Date Is Outside Of Financial Year !!');
                var datePost = (3 + '-' + 31 + '-' + objsession[1]);
                dtTo.SetDate(new Date(datePost));
            }
        }
        else {
            alert('Enter Date Is Outside Of Financial Year !!');
            var datePost = (3 + '-' + 31 + '-' + objsession[1]);
            dtTo.SetDate(new Date(datePost));
        }
    }
    function updateCashBankDetail(objDate, objVouNo, objMainID, objSubID, objCompID, objSegID) {
        var combo = document.getElementById('ddlExport');
        combo.value = 'Ex';
        document.getElementById('HdnSelectLedger').value = 'S';
        var URL = 'frmReport_CashBankBookUpdate.aspx?date=' + objDate + ' &vNo=' + objVouNo + ' &Compid=' + objCompID + ' &SegID=' + objSegID;
        OnMoreInfoClick(URL, "Update Cash/Bank Book", '940px', '450px', "Y");

    }
    function updateJournalDetail(objDate, objVouNo, objMainID, objSubID, objCompID, objSegID) {
        var combo = document.getElementById('ddlExport');
        combo.value = 'Ex';
        document.getElementById('HdnSelectLedger').value = 'S';
        var URL = 'journalPopupReport.aspx?date=' + objDate + ' &id=' + objVouNo + ' &Compid=' + objCompID + ' &exch=' + objSegID;
        OnMoreInfoClick(URL, "Update JournalVoucher", '940px', '450px', "Y");
    }
    function ShowObligationBreakUp(objBill, objVouId, objSegment, objTranDate) {
        var combo = document.getElementById('ddlExport');
        combo.value = 'Ex';
        document.getElementById('HdnSelectLedger').value = 'S';
        var URL = 'ShowScrip.aspx?Bill=' + objBill + ' &TranID=' + objVouId + ' &SegMentName=' + objSegment + ' &TranDate=' + objTranDate;
        OnMoreInfoClick(URL, "Show Obligation BreakUp", '940px', '450px', "Y");
    }
    function callback() {
        var btn = document.getElementById('Button1');
        btn.click();
    }

    function dateChange11() {
        //document.getElementById('Button1').disabled=true;
    }
    </script>

    <script type="text/ecmascript">

        function ReceiveServerData(rValue) {
            var Data = rValue.split('~');
            if (Data[0] == 'Segment') {
                document.getElementById('HdnBranch').value = Data[1];
            }
            if (Data[0] == 'Branch') {
                document.getElementById('HdnSegment').value = Data[1];
            }
            if (Data[0] == 'Sub Ac') {
                document.getElementById('HdnSubAc').value = Data[1];
            }
            if (Data[0] == 'Ac Name') {
                document.getElementById('HdnMainAc').value = Data[1];
                document.getElementById('HdnSubLedgerType').value = Data[2];
            }
        }


    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <table class="TableMain100">
            <tr>
                <td class="EHEADER" style="text-align: center;">
                    <strong><span style="color: #000099">Reports Ledger View</span></strong>
                </td>
            </tr>
        </table>
        <div>
            <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="36000">
            </asp:ScriptManager>
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
            <table class="TableMain100">
                <tr id="TdAll1">
                    <td colspan="2">
                        <table width="100%">
                            <tr>
                                <td class="gridcellleft" style="width: 76px;">
                                     <asp:HiddenField ID="txtMainAccount_hidden" runat="server" />
                                </td>
                                <td>
                                   
                                </td>
                                <td class="gridcellleft" style="vertical-align: top; text-align: right" id="TdFilter">
                                    <span id="spanall">
                                        <asp:TextBox ID="txtsubscriptionID" runat="server" Font-Size="12px" Width="150px"></asp:TextBox></span>
                                    <span id="spanSub">
                                        <asp:TextBox ID="txtSubsubcriptionID" runat="server" Font-Size="12px" Width="150px"></asp:TextBox></span>
                                    <asp:DropDownList ID="cmbsearchOption" runat="server" Font-Size="11px" Width="70px"
                                        Enabled="false">
                                        <asp:ListItem>Segment</asp:ListItem>
                                        <asp:ListItem>Branch</asp:ListItem>
                                        <asp:ListItem>Ac Name</asp:ListItem>
                                        <asp:ListItem>Sub Ac</asp:ListItem>
                                    </asp:DropDownList>
                                    <a id="A4" href="javascript:void(0);" onclick="btnAddsubscriptionlist_click()"><span
                                        style="color: #009900; text-decoration: underline; font-size: 8pt;">Add to List</span></a><span
                                            style="color: #009900; font-size: 8pt;"> </span>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr id="TrAll">
                    <td style="text-align: left; vertical-align: top;">
                        <table border="0">
                            
                           
                            
                            
                            <tr>
                                <td style="width: 110px">
                                    <dxe:ASPxDateEdit ID="dtFrom" runat="server" ClientInstanceName="dtFrom" EditFormat="Custom"
                                        UseMaskBehavior="True" Font-Size="12px" Width="108px">
                                        <DropDownButton Text="From">
                                        </DropDownButton>
                                        <ClientSideEvents ValueChanged="function(s,e){dateChange();DateChangeForFrom();}" GotFocus="function(s,e){dateChange11();}" />
                                    </dxe:ASPxDateEdit>
                                </td>
                                <td style="width: 110px">
                                    <dxe:ASPxDateEdit ID="dtTo" runat="server" ClientInstanceName="dtTo" EditFormat="Custom"
                                        UseMaskBehavior="True" Font-Size="12px" Width="98px">
                                        <DropDownButton Text="To">
                                        </DropDownButton>
                                        <ClientSideEvents ValueChanged="function(s,e){dateChange();DateChangeForTo();}" GotFocus="function(s,e){dateChange11();}"/>
                                    </dxe:ASPxDateEdit>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td style="text-align: right; vertical-align: top; width: 16%">
                        <table width="100%" id="showFilter">
                            <tr>
                                <td style="text-align: right; vertical-align: top">
                                    <table cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td>
                                                <asp:ListBox ID="lstSuscriptions" runat="server" Font-Size="12px" Height="90px" Width="290px">
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
                                    <asp:TextBox ID="txtSubsubcriptionID_hidden" runat="server" Font-Size="12px" Width="1px"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>   
                <tr>
                    <td colspan="2">
                        <table>
                            <tr>
                                <td>
                                    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
                                        <ProgressTemplate>
                                            <div id='Div2' style='position: absolute; font-family: arial; font-size: 30; left: 50%;
                                                top: 50%; background-color: white; layer-background-color: white; height: 80;
                                                width: 150;'>
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
                    </td>
                </tr>             
                <tr>
                    <td colspan="2" style="text-align: left; vertical-align: bottom;">
                        <table class="TableMain100">
                            <tr>
                                <td id="TrPrevNext">
                                   
                                </td>
                                <td class="gridcellright">
                                    <table width="100%">
                                        
                                        <tr>
                                            <td id="TrForFilter" colspan="3">
                                                <a href="#" style="font-weight: bold; color: Blue" onclick="javascript:ForFilter();">
                                                    Filter</a> || <asp:LinkButton ID="btnEmail" runat="server"  Font-Bold="True" Font-Underline="True" ForeColor="Blue" OnClick="btnEmail_Click">SendEmail</asp:LinkButton> || 
                                                <asp:DropDownList ID="ddlExport" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlExport_SelectedIndexChanged1" >
                                                    <asp:ListItem Value="Ex" Selected="True">Export</asp:ListItem>
                                                    <asp:ListItem Value="E">Excel</asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:CheckBox ID="chkDouble" runat="server" /><span style="color: Blue">Check here for Both Page Printing , </span> 
                                                <asp:LinkButton ID="lnkExport" runat="server" OnClick="lnkExport_Click" ForeColor="Blue"><span style="font-weight: bold; color: Blue">Print</span> </asp:LinkButton>

                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr id="TrBtn">
                    <td>
                        <asp:Button ID="Button1" runat="server" Text="Show" CssClass="btnUpdate" Height="23px"
                            Width="101px" OnClientClick="javascript:selecttion();" OnClick="Button1_Click" />
                        <span id="spanBtn" style="display: none"><a href="#" style="font-weight: bold; color: Blue"
                            onclick="javascript:btnCancel_Click();">Cancel</a></span>
                    </td>
                    <td style="display: none">
                        <asp:Button ID="ButtonUpdate" runat="server" Text="Button" OnClick="ButtonUpdate_Click" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2" id="showDetail">
                        <span id="spanshow" style="color: Blue; font-weight: bold"></span>&nbsp;&nbsp;&nbsp;&nbsp;
                        <span id="spanshow2" style="color: Blue; font-weight: bold">Period :</span>&nbsp;&nbsp;<span
                            id="spanshow3"></span>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:GridView ID="grdCashBankBook" runat="server" Width="100%" BorderColor="CornflowerBlue"
                                    ShowFooter="True" OnRowDataBound="grdCashBankSummary_RowDataBound" AllowSorting="true"
                                    AutoGenerateColumns="false" BorderStyle="Solid" BorderWidth="2px" CellPadding="4"
                                    ForeColor="#0000C0" OnRowCreated="grdCashBankBook_RowCreated"
                                    OnSorting="grdCashBankBook_Sorting">
                                    <FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></FooterStyle>
                                    <Columns>
                                        <asp:TemplateField HeaderText="Tr. Date" SortExpression="accountsledger_transactiondate">
                                            <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                            <HeaderStyle Wrap="False" HorizontalAlign="Center" Font-Bold="False"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblTrDate" runat="server" Text='<%# Eval("TrDate")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="ValueDate">
                                            <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                            <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblValueDate" runat="server" Text='<%# Eval("ValueDate")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Voucher No." SortExpression="accountsledger_TransactionReferenceID">
                                            <ItemStyle BorderWidth="1px" HorizontalAlign="Left"></ItemStyle>
                                            <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblVoucherNo" runat="server" Text='<%# Eval("accountsledger_TransactionReferenceID")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Description">
                                            <ItemStyle BorderWidth="1px" HorizontalAlign="Left"></ItemStyle>
                                            <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblDescrip" runat="server" Text='<%# Eval("accountsledger_Narration")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="AccountName" Visible="false">
                                            <ItemStyle BorderWidth="1px" HorizontalAlign="Left"></ItemStyle>
                                            <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblAccName" runat="server" Text='<%# Eval("AccountName")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Trade Date">
                                            <ItemStyle BorderWidth="1px" HorizontalAlign="Left"></ItemStyle>
                                            <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblPayoutDate" runat="server" Text='<%# Eval("PayoutDate")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                         <asp:TemplateField HeaderText="Branch Code">
                                            <ItemStyle BorderWidth="1px" HorizontalAlign="Left"></ItemStyle>
                                            <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblBranchCode" runat="server" Text='<%# Eval("BranchCode")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>   
                                        <asp:TemplateField HeaderText="Settlement No.">
                                            <ItemStyle BorderWidth="1px" HorizontalAlign="Left"></ItemStyle>
                                            <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblSettNo" runat="server" Text='<%# Eval("SettlementNumber")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>   
                                        <asp:TemplateField HeaderText="Instrument No." SortExpression="accountsledger_InstrumentNumber">
                                            <ItemStyle BorderWidth="1px" HorizontalAlign="Left"></ItemStyle>
                                            <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblInstNo" runat="server" Text='<%# Eval("accountsledger_InstrumentNumber")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>                                                                             
                                        <asp:TemplateField HeaderText="Debit">
                                            <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                            <HeaderStyle Wrap="False" HorizontalAlign="Right" Font-Bold="False"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblAmtDr" runat="server" Text='<%# Eval("Accountsledger_AmountCr")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Credit">
                                            <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                            <HeaderStyle Wrap="False" HorizontalAlign="Right" Font-Bold="False"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblAmtCr" runat="server" Text='<%# Eval("Accountsledger_AmountDr")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Closing">
                                            <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                            <HeaderStyle Wrap="False" HorizontalAlign="Right" Font-Bold="False"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblClosing" runat="server" Text='<%# Eval("Closing")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Closing" Visible="false">
                                            <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                            <HeaderStyle Wrap="False" HorizontalAlign="Right" Font-Bold="False"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblTradeDate" runat="server" Text='<%# Eval("accountsledger_transactiondate")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                         <asp:TemplateField HeaderText="MainID" Visible="false">
                                            <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                            <HeaderStyle Wrap="False" HorizontalAlign="Right" Font-Bold="False">
                                            </HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblMainID" runat="server" Text='<%# Eval("MainID")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="SubID" Visible="false">
                                            <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                            <HeaderStyle Wrap="False" HorizontalAlign="Right" Font-Bold="False">
                                            </HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblSubID" runat="server" Text='<%# Eval("SubID")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="MainID" Visible="false">
                                            <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                            <HeaderStyle Wrap="False" HorizontalAlign="Right" Font-Bold="False">
                                            </HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblCompID" runat="server" Text='<%# Eval("CompanyID")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="SubID" Visible="false">
                                            <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                            <HeaderStyle Wrap="False" HorizontalAlign="Right" Font-Bold="False">
                                            </HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblSegID" runat="server" Text='<%# Eval("SegID")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="CashType" Visible="false">
                                            <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                            <HeaderStyle Wrap="False" HorizontalAlign="Right" Font-Bold="False">
                                            </HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblCashType" runat="server" Text='<%# Eval("CashType")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <PagerTemplate>
                                        <table>
                                            <tr>
                                                <td colspan="10">
                                                    <asp:LinkButton ID="FirstPage" runat="server" Font-Bold="true" CommandName="First" OnClientClick="selecttion()"
                                                         Text="[First Page]"> </asp:LinkButton>
                                                    <asp:LinkButton ID="PreviousPage" runat="server" Font-Bold="true" CommandName="Prev" OnClientClick="selecttion()"
                                                         Text="[Previous Page]">  </asp:LinkButton>
                                                    <asp:LinkButton ID="NextPage" runat="server" Font-Bold="true" CommandName="Next" OnClientClick="selecttion()"
                                                         Text="[Next Page]">
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="LastPage" runat="server" Font-Bold="true" CommandName="Last" OnClientClick="selecttion()"
                                                        Text="[Last Page]">
                                                    </asp:LinkButton>
                                                </td>
                                            </tr>
                                        </table>
                                    </PagerTemplate>
                                    <RowStyle BackColor="#FFFFFF" ForeColor="#330099" BorderColor="#BFD3EE" BorderStyle="Double"
                                        BorderWidth="1px"></RowStyle>
                                    <EditRowStyle BackColor="#E59930"></EditRowStyle>
                                    <SelectedRowStyle BackColor="#D1DDF1" ForeColor="#333333" Font-Bold="True"></SelectedRowStyle>
                                    <PagerStyle ForeColor="White" HorizontalAlign="Center"></PagerStyle>
                                    <HeaderStyle ForeColor="Black" BorderWidth="1px" CssClass="EHEADER" BorderColor="AliceBlue"
                                        Font-Bold="False"></HeaderStyle>
                                    <%--<AlternatingRowStyle BackColor="White"></AlternatingRowStyle>--%>
                                </asp:GridView>
                                <asp:HiddenField ID="CurrentPage" runat="server"></asp:HiddenField>
                                <asp:HiddenField ID="TotalPages" runat="server"></asp:HiddenField>
                                <asp:HiddenField ID="TotalClient" runat="server" />
                                
                                <asp:HiddenField ID="HdnSelectLedger" runat="server" />
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click"></asp:AsyncPostBackTrigger>
                               
                             
                            </Triggers>
                        </asp:UpdatePanel>
                        <asp:HiddenField ID="HdnBranch" runat="server" />
                        <asp:HiddenField ID="HdnSegment" runat="server" />
                        <asp:HiddenField ID="HdnMainAc" runat="server" />
                        <asp:HiddenField ID="HdnSubAc" runat="server" />
                        <asp:HiddenField ID="HdnSubLedgerType" runat="server" />
                    </td>
                </tr>                
            </table>
        </div>
</asp:Content>
