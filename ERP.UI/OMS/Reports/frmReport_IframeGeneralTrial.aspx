<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    EnableEventValidation="false" Inherits="ERP.OMS.Reports.Reports_frmReport_IframeGeneralTrial" CodeBehind="frmReport_IframeGeneralTrial.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="/assests/js/init.js"></script>
    <script type="text/javascript" src="/assests/js/ajax-dynamic-list_rootfile.js"></script>

    <script language="javascript" type="text/javascript">
        function cgdGeneralTrialEndCallBack() {
        }
        function aspxPeriodGridEndCallBack() {

        }
        //        function winclose()
        //        {
        //        alert("No Data found");
        //        return false;
        //        }

        function filter() {
            document.getElementById("Trfilter").style.display = 'none';
            document.getElementById("TrAll").style.display = 'inline';
            document.getElementById('TrSearch').style.display = 'none';
            height();
        }

        function ShowHidePeriod(obj, obj1) {
            document.getElementById("TdAsOnDate").style.display = 'none';
            document.getElementById("TdPeriod").style.display = 'inline';


            document.getElementById("TrAll").style.display = 'none';
            document.getElementById("Trfilter").style.display = 'inline';
            document.getElementById("showDetail").style.display = 'inline';
            document.getElementById("spanshow1").innerText = obj;
            document.getElementById("spanshow3").innerText = obj1;
            height();

        }

        function ShowHideAsOn(obj, obj1) {

            document.getElementById("TdAsOnDate").style.display = 'inline';
            document.getElementById("TdPeriod").style.display = 'none';

            document.getElementById("TrAll").style.display = 'none';
            document.getElementById("Trfilter").style.display = 'inline';
            document.getElementById("showDetail").style.display = 'inline';
            document.getElementById("spanshow1").innerText = obj;
            document.getElementById("spanshow3").innerText = obj1;
            height();

        }

        function SignOff() {
            window.parent.SignOff();
        }
        FieldName = 'lstSuscriptions';
        function height() {
            if (document.body.scrollHeight >= 350) {
                window.frameElement.height = document.body.scrollHeight;
            }
            else {
                window.frameElement.height = '350px';
            }
            window.frameElement.widht = document.body.scrollWidht;
        }
        //        function ShowHide(obj)
        //        {
        //            document.getElementById("TdExport").style.display='inline';
        //            height();
        //        }
        function Disable(obj) {
            var gridview = document.getElementById('grdGeneralTrial');
            var rCount = gridview.rows.length;
            if (rCount < 10)
                rCount = '0' + rCount;
            if (obj == 'P') {
                document.getElementById("grdGeneralTrial_ctl18_FirstPage").style.display = 'none';
                document.getElementById("grdGeneralTrial_ctl18_PreviousPage").style.display = 'none';
                document.getElementById("grdGeneralTrial_ctl18_NextPage").style.display = 'inline';
                document.getElementById("grdGeneralTrial_ctl18_LastPage").style.display = 'inline';
            }
            else {
                document.getElementById("grdGeneralTrial_ctl" + rCount + "_NextPage").style.display = 'none';
                document.getElementById("grdGeneralTrial_ctl" + rCount + "_LastPage").style.display = 'none';
            }
        }
        function ShowGeneralTrialDetail(objMainAcc, objDate, objMain, objSegment, objTo, objType, ObjAcType, objZero) {
            var URL = 'frmReport_GeneralTrialDetail.aspx?mainacc=' + objMainAcc + ' &date=' + objDate + ' &Segment=' + objSegment + ' &TDate=' + objTo + ' &Type=' + objType + ' &AccName=' + objMain + ' &AccType=' + ObjAcType + ' &ZeroBal=' + objZero;
            console.log(URL);
            // parent.ParentWindowShow(URL,objMain);
            //  OnMoreInfoClick(URL,objMain,'980px','500px',"Y");
            popup.SetContentUrl(URL);
            popup.SetWidth(document.documentElement.clientWidth - 200);
            popup.SetHeight(document.documentElement.clientHeight - 200);
            popup.Show();
        }



        function ShowCashBankDetail(objMainAcc, objDate, objMain, objSegment, objTo, objType, ObjAcType) {
            var URL = 'frmReport_IFrameCashBankBook.aspx?mainacc=' + objMainAcc + ' &date=' + objDate + ' &Segment=' + objSegment + ' &TDate=' + objTo + ' &Type=' + objType + ' &AccName=' + objMain + ' &AccType=' + ObjAcType;
            popup.SetContentUrl(URL);
            popup.SetWidth(document.documentElement.clientWidth - 200);
            popup.SetHeight(document.documentElement.clientHeight - 200);
            popup.Show();
            console.log(URL);
            // OnMoreInfoClick(URL,objMain,'980px','500px',"Y");
        }


        function ShowLedger(objMainID, objSubID, objSegmentID, objMainAcc, objSubAcc, objUcc, objDate) {
            var URL = 'frmReport_IFrameLedgerView.aspx?MainID=' + objMainID + ' &SubID=' + objSubID + ' &SegmentID=' + objSegmentID + ' &date=' + objDate;
            // editwin=dhtmlmodal.open("Editbox", "iframe", URL, "Bill For - "+objMainAcc+"", "width=940px,height=450px,center=1,resize=1,top=500", "recal");                 
            //  OnMoreInfoClick(URL,objMainAcc,'980px','500px',"Y");
            popup.SetContentUrl(URL);
            popup.SetWidth(document.documentElement.clientWidth - 200);
            popup.SetHeight(document.documentElement.clientHeight - 200);
            popup.Show();
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
            var s = document.getElementById('txtsubscriptionID');
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
        function clientselectionfinal() {
            var listBoxSubs = document.getElementById('lstSuscriptions');
            var listIDs = '';
            var i;
            if (listBoxSubs.length > 0) {
                for (i = 0; i < listBoxSubs.length; i++) {
                    if (listIDs == '')
                        listIDs = listBoxSubs.options[i].value + ';' + listBoxSubs.options[i].text;
                    else
                        listIDs += ',' + listBoxSubs.options[i].value + ';' + listBoxSubs.options[i].text;
                }
                var sendData = 'Segment' + '~' + listIDs;
                CallServer(sendData, "");
            }
            var i;
            for (i = listBoxSubs.options.length - 1; i >= 0; i--) {
                listBoxSubs.remove(i);
            }
            document.getElementById('TdFilter').style.display = 'none';
            document.getElementById('TdSelect').style.display = 'none';
        }
        function ShowMainAccountName(obj1, obj2, obj3) {
            ajax_showOptions(obj1, obj2, obj3, 'Segment');
        }
        function MainAll(obj) {

            if (obj == 'all') {
                document.getElementById('TdFilter').style.display = 'none';
                document.getElementById('TdSelect').style.display = 'none';
            }
            else {
                if (document.getElementById('HDNAccInd').value == 'Y') {
                    document.getElementById('TdFilter').style.display = '';
                    document.getElementById('TdSelect').style.display = '';
                }
            }
        }
        var PeriodWise = false;
        function DateAll(obj) {
            if (obj == 'all') {
                //as on date
                $('.assOndate').show();
                $('.datePeriod').hide();
                PeriodWise = false;
            }
            else {
                $('.assOndate').hide();
                $('.datePeriod').show();
                PeriodWise = true;
            }
            //if(obj=='all')
            //{
            //    document.getElementById('asOnDate1').style.display='';
            //    document.getElementById('AsOnTr').style.display='';                
            //    document.getElementById('asOnDate2').style.display='';
            //    document.getElementById('Period1').style.display='none';
            //    document.getElementById('PeriodTr').style.display='none';	            
            //    document.getElementById('Period2').style.display='none';
            //    document.getElementById('Period3').style.display='none';

            //}
            //else    
            //{
            //    document.getElementById('asOnDate1').style.display='none';
            //     document.getElementById('AsOnTr').style.display='none';
            //    document.getElementById('asOnDate2').style.display='none';
            //    document.getElementById('Period1').style.display='';
            //     document.getElementById('PeriodTr').style.display='';
            //    document.getElementById('Period2').style.display='';
            //    document.getElementById('Period3').style.display='';	        
            //}
        }
        function Page_Load() {
            document.getElementById("showDetail").style.display = 'none';
            document.getElementById('Trfilter').style.display = 'none';
            document.getElementById('TdFilter').style.display = 'none';
            document.getElementById('TdSelect').style.display = 'none';
            $('.datePeriod').hide();
            //  document.getElementById('asOnDate1').style.display='';
            // document.getElementById('AsOnTr').style.display='';
            // document.getElementById('asOnDate2').style.display='';
            //  document.getElementById('Period1').style.display='none';
            // document.getElementById('PeriodTr').style.display='none';
            //  document.getElementById('Period2').style.display='none';
            // document.getElementById('Period3').style.display='none';

        }
        function ChangeRowColor(rowID, rowNumber) {
            var gridview = document.getElementById('grdGeneralTrial');
            var rCount = gridview.rows.length;
            var rowIndex = 1;
            var rowCount = 0;
            if (rCount == 18)
                rowCount = 15;
            else
                rowCount = rCount - 3;
            if (rowNumber > 15 && rCount < 18)
                rowCount = rCount - 3;
            for (rowIndex; rowIndex <= rowCount; rowIndex++) {
                var rowElement = gridview.rows[rowIndex];
                rowElement.style.backgroundColor = '#FFFFFF'
            }
            document.getElementById(rowID).style.backgroundColor = '#ffe1ac';

        }
        function ChangeRowColorPeriod(rowID, rowNumber) {
            var gridview = document.getElementById('grdPeriod');
            var rCount = gridview.rows.length;
            var rowIndex = 2;
            var rowCount = 0;
            if (rCount == 18)
                rowCount = 15;
            else
                rowCount = rCount - 3;
            if (rowNumber > 15 && rCount < 18)
                rowCount = rCount - 3;
            for (rowIndex; rowIndex <= rowCount; rowIndex++) {
                var rowElement = gridview.rows[rowIndex];
                rowElement.style.backgroundColor = '#FFFFFF';
            }
            document.getElementById(rowID).style.backgroundColor = '#ffe1ac';
        }
        function DisablePeriod(obj) {
            var gridview = document.getElementById('grdPeriod');
            var rCount = gridview.rows.length;
            if (rCount < 10)
                rCount = '0' + (rCount - 1);
            if (obj == 'P') {
                document.getElementById("grdPeriod_ctl18_FirstPage").style.display = 'none';
                document.getElementById("grdPeriod_ctl18_PreviousPage").style.display = 'none';
                document.getElementById("grdPeriod_ctl18_NextPage").style.display = '';
                document.getElementById("grdPeriod_ctl18_LastPage").style.display = '';
            }
            else {
                document.getElementById("grdPeriod_ctl" + rCount + "_NextPage").style.display = 'none';
                document.getElementById("grdPeriod_ctl" + rCount + "_LastPage").style.display = 'none';
            }
        }
        function DisableFirst() {
            var gridview = document.getElementById('grdGeneralTrial');
            var rCount = gridview.rows.length;
            if (rCount < 10)
                rCount = '0' + (rCount - 1);
            document.getElementById("grdGeneralTrial_ctl" + rCount + "_NextPage").style.display = 'none';
            document.getElementById("grdGeneralTrial_ctl" + rCount + "_LastPage").style.display = 'none';
            document.getElementById("grdGeneralTrial_ctl" + rCount + "_FirstPage").style.display = 'none';
            document.getElementById("grdGeneralTrial_ctl" + rCount + "_PreviousPage").style.display = 'none';
        }
        function selecttion() {
            var combo = document.getElementById('ddlExport');
            combo.value = 'Ex';
            
            var BranchID = "";

            var items = checkListBox.GetSelectedItems();
            var vals = [];
            var texts = [];

            for (var i = 0; i < items.length; i++) {
                if (items[i].index != 0) {
                    if (i == 0) {
                        BranchID = items[i].value;
                    }
                    else {
                        if (BranchID == "") {
                            BranchID = items[i].value;
                        }
                        else {
                            BranchID = BranchID + ',' + items[i].value;
                        }
                    }
                }
            }

            document.getElementById('branchList').value = BranchID;
            document.getElementById('IsPeriod').value = PeriodWise;

            if (PeriodWise) {
                caspxPeriodGrid.PerformCallback();
                caspxPeriodGrid.Refresh();
            }
            else {
                cgdGeneralTrial.PerformCallback('');
                cgdGeneralTrial.Refresh();
            }

            return false;
        }
        function DateChangeForFrom() {

            var sessionValFrom = "<%=Session["FinYearStart"]%>";
            var sessionValTo = "<%=Session["FinYearEnd"]%>";
            var sessionVal = "<%=Session["LastFinYear"]%>";
            var objsession = sessionVal.split('-');
            var MonthDate = dtFrom.GetDate().getMonth() + 1;
            var DayDate = dtFrom.GetDate().getDate();
            var YearDate = dtFrom.GetDate().getYear();
            var Cdate = MonthDate + "/" + DayDate + "/" + YearDate;
            var Sto = new Date(sessionValTo).getMonth() + 1;
            var SFrom = new Date(sessionValFrom).getMonth() + 1;
            var SDto = new Date(sessionValTo).getDate();
            var SDFrom = new Date(sessionValFrom).getDate();

            if (YearDate >= objsession[0]) {
                if (MonthDate < SFrom && YearDate == objsession[0]) {
                    alert('Enter Date Is Outside Of Financial Year !!');
                    var datePost = (SFrom + '-' + SDFrom + '-' + objsession[0]);
                    dtFrom.SetDate(new Date(datePost));
                }
                else if (MonthDate > Sto && YearDate == objsession[1]) {
                    alert('Enter Date Is Outside Of Financial Year !!');
                    var datePost = (SFrom + '-' + SDFrom + '-' + objsession[0]);
                    dtFrom.SetDate(new Date(datePost));
                }
                else if (YearDate != objsession[0] && YearDate != objsession[1]) {
                    alert('Enter Date Is Outside Of Financial Year !!');
                    var datePost = (SFrom + '-' + SDFrom + '-' + objsession[0]);
                    dtFrom.SetDate(new Date(datePost));
                }
            }
            else {
                alert('Enter Date Is Outside Of Financial Year !!');
                var datePost = (SFrom + '-' + SDFrom + '-' + objsession[0]);
                dtFrom.SetDate(new Date(datePost));
            }
        }
        function DateChangeForTo() {
            var sessionValFrom = "<%=Session["FinYearStart"]%>";
            var sessionValTo = "<%=Session["FinYearEnd"]%>";
            var sessionVal = "<%=Session["LastFinYear"]%>";
            var objsession = sessionVal.split('-');
            var MonthDate = dtToDate.GetDate().getMonth() + 1;
            var DayDate = dtToDate.GetDate().getDate();
            var YearDate = dtToDate.GetDate().getYear();
            var Cdate = MonthDate + "/" + DayDate + "/" + YearDate;
            var Sto = new Date(sessionValTo).getMonth() + 1;
            var SFrom = new Date(sessionValFrom).getMonth() + 1;
            var SDto = new Date(sessionValTo).getDate();
            var SDFrom = new Date(sessionValFrom).getDate();

            if (YearDate <= objsession[1]) {
                if (MonthDate < SFrom && YearDate == objsession[0]) {
                    alert('Enter Date Is Outside Of Financial Year !!');
                    var datePost = (Sto + '-' + SDto + '-' + objsession[1]);
                    dtToDate.SetDate(new Date(datePost));
                }
                else if (MonthDate > Sto && YearDate == objsession[1]) {
                    alert('Enter Date Is Outside Of Financial Year !!');
                    var datePost = (Sto + '-' + SDto + '-' + objsession[1]);
                    dtToDate.SetDate(new Date(datePost));
                }
                else if (YearDate != objsession[0] && YearDate != objsession[1]) {
                    alert('Enter Date Is Outside Of Financial Year !!');
                    var datePost = (Sto + '-' + SDto + '-' + objsession[1]);
                    dtToDate.SetDate(new Date(datePost));
                }
            }
            else {
                alert('Enter Date Is Outside Of Financial Year !!');
                var datePost = (Sto + '-' + SDto + '-' + objsession[1]);
                dtToDate.SetDate(new Date(datePost));
            }
        }
    </script>
    <script type="text/ecmascript">

        function ReceiveServerData(rValue) {
            var Data = rValue.split('~');
            if (Data[0] == 'Segment') {

                var combo = document.getElementById('litSegment');
                var NoItems = Data[1].split(',');
                var i;
                var val = '';
                var seg = '';
                for (i = 0; i < NoItems.length; i++) {
                    var items = NoItems[i].split(';');
                    if (val == '') {
                        seg = items[0];
                        val = items[1];

                    }
                    else {
                        seg += ',' + items[0];
                        val += ',' + items[1];

                    }
                }
                document.getElementById('hdnSegment').value = seg;
                combo.innerText = val;
            }
        }

    </script>
    <style>
        .dxeButtonEditSys.dxeButtonEdit_PlasticBlue {
            margin-bottom: 0;
        }

        .mTop5 {
            margin-top: 5px;
        }
    </style>
    <script type="text/javascript">
        var textSeparator = ";";
        var selectedChkValue = "";

        function OnListBoxSelectionChanged(listBox, args) {
            if (args.index == 0)
                args.isSelected ? listBox.SelectAll() : listBox.UnselectAll();
            UpdateSelectAllItemState();
            UpdateText();
        }
        function UpdateSelectAllItemState() {
            IsAllSelected() ? checkListBox.SelectIndices([0]) : checkListBox.UnselectIndices([0]);
        }
        function IsAllSelected() {
            var selectedDataItemCount = checkListBox.GetItemCount() - (checkListBox.GetItem(0).selected ? 0 : 1);
            return checkListBox.GetSelectedItems().length == selectedDataItemCount;
        }
        function UpdateText() {
            var selectedItems = checkListBox.GetSelectedItems();
            //checkComboBox.SetText(GetSelectedItemsText(selectedItems));

            selectedChkValue = GetSelectedItemsText(selectedItems);
            var ItemCount = GetSelectedItemsCount(selectedItems);

            if (ItemCount > 0) {
                checkComboBox.SetText(ItemCount + " Items");
            }
            else {
                checkComboBox.SetText("");
            }
            
            }
        function SynchronizeListBoxValues(dropDown, args) {
            checkListBox.UnselectAll();
            //var texts = dropDown.GetText().split(textSeparator);
            var texts = selectedChkValue.split(textSeparator);

            var values = GetValuesByTexts(texts);
            checkListBox.SelectValues(values);
            UpdateSelectAllItemState();
            UpdateText(); // for remove non-existing texts
        }
        function GetSelectedItemsCount(items) {
            var texts = [];
            for (var i = 0; i < items.length; i++)
                if (items[i].index != 0)
                    texts.push(items[i].text);
            return texts.length;
        }
        function GetSelectedItemsText(items) {
            var texts = [];
            for (var i = 0; i < items.length; i++)
                if (items[i].index != 0)
                    texts.push(items[i].text);
            return texts.join(textSeparator);
        }
        function GetValuesByTexts(texts) {
            var actualValues = [];
            var item;
            for (var i = 0; i < texts.length; i++) {
                item = checkListBox.FindItemByText(texts[i]);
                if (item != null)
                    actualValues.push(item.value);
            }
            return actualValues;
        }
    </script>
    <style>
        .table-pad>tbody>tr>td {
            padding-right:15px;
            vertical-align:middle;
        }
    </style>
</asp:Content>



<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>


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
        <div class="panel-heading">
            <div class="panel-title">
                <h3>General Trial</h3>
            </div>

        </div>

        <div class="form_main inner">
            <table class="TableMain100" border="0">
                <tr id="TrAll">
                    <td style="vertical-align: top">
                        <table class="table-pad">
                            <tr>

                                <td style="width: 160px; vertical-align: middle">
                                    <table width="100%">
                                        <tr>
                                            <td>
                                                <asp:RadioButton ID="radAsDate" runat="server" Checked="True" GroupName="a1" />
                                            </td>
                                            <td>As On Date
                                            </td>
                                            <td>
                                                <asp:RadioButton ID="radPeriod" runat="server" GroupName="a1" />
                                            </td>
                                            <td>Period
                                            </td>
                                            <td>
                                                <span id="Span1" runat="server" style="color: Maroon"></span>
                                            </td>
                                        </tr>
                                    </table>
                                </td>

                                <td>
                                    <table class="datePeriod">
                                        <tr>
                                            <td style="padding: 10px">From</td>
                                            <td id="Period2" style="padding-right: 10px">
                                                <dxe:ASPxDateEdit ID="dtFrom" runat="server" EditFormat="Custom" ClientInstanceName="dtFrom"
                                                    UseMaskBehavior="True">
                                                    <%-- <ClientSideEvents DateChanged="function(s,e){DateChangeForFrom();}" />--%>
                                                </dxe:ASPxDateEdit>
                                            </td>
                                            <td style="padding: 10px">To</td>
                                            <td id="Period3">
                                                <dxe:ASPxDateEdit ID="dtTo" runat="server" EditFormat="Custom" ClientInstanceName="dtToDate"
                                                    UseMaskBehavior="True">
                                                    <%--<ClientSideEvents DateChanged="function(s,e){DateChangeForTo();}" />--%>
                                                </dxe:ASPxDateEdit>
                                            </td>
                                        </tr>
                                    </table>
                                </td>

                                <td style="padding-right: 10px">
                                    <table class="assOndate">
                                        <tr>
                                            <td style="width: 160px" id="asOnDate2">
                                                <dxe:ASPxDateEdit ID="dtDate" runat="server" EditFormat="Custom" UseMaskBehavior="True">
                                                    <%-- <DropDownButton Text="As on Date">
                                                    </DropDownButton>--%>
                                                </dxe:ASPxDateEdit>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td>Branch</td>
                                <td>
                                    <table>
                                        <tr>                                            
                                            <td>
                                                <dxe:ASPxDropDownEdit ClientInstanceName="checkComboBox" ID="cmdBranch" Width="210px" runat="server" AnimationType="None">
                                                    <DropDownWindowStyle BackColor="#EDEDED" />
                                                    <DropDownWindowTemplate>
                                                        <dxe:ASPxListBox Width="100%" Height="250px" ID="listBox" ClientInstanceName="checkListBox" SelectionMode="CheckColumn"
                                                            runat="server" OnInit="listBox_Init">
                                                            <Border BorderStyle="None" />
                                                            <BorderBottom BorderStyle="Solid" BorderWidth="1px" BorderColor="#DCDCDC" />
                                                            <Items>
                                                                <dxe:ListEditItem Text="(Select all)" />
                                                                <dxe:ListEditItem Text="Chrome" Value="1" />
                                                                <dxe:ListEditItem Text="Firefox" Value="2" />
                                                            </Items>
                                                            <ClientSideEvents SelectedIndexChanged="OnListBoxSelectionChanged" />
                                                        </dxe:ASPxListBox>
                                                        <table style="width: 100%">
                                                            <tr>
                                                                <td style="padding: 4px">
                                                                    <dxe:ASPxButton ID="ASPxButton1" AutoPostBack="False" runat="server" Text="Close" Style="float: right">
                                                                        <ClientSideEvents Click="function(s, e){ checkComboBox.HideDropDown(); }" />
                                                                    </dxe:ASPxButton>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </DropDownWindowTemplate>
                                                    <ClientSideEvents TextChanged="SynchronizeListBoxValues" DropDown="SynchronizeListBoxValues" />
                                                </dxe:ASPxDropDownEdit>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td style="padding-right: 10px; vertical-align: middle">
                                    <asp:CheckBox ID="chkZero" runat="server" />
                                    Show Zero Balance Account
                                </td>
                               
                                <td>
                                    <asp:Button ID="btnShow" runat="server" Text="Show" CssClass="btnUpdate mTop5 btn btn-primary"
                                        OnClientClick="return selecttion()" OnClick="btnShow_Click" />
                                    <asp:TextBox ID="txtsubscriptionID_hidden" runat="server" Font-Size="12px" Width="1px" CssClass="hide"></asp:TextBox>
                                    <% if (rights.CanExport)
                                       { %>
                                    <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-sm btn-primary mTop5" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" OnChange="if(!AvailableExportOption()){return false;}" AutoPostBack="true">
                                        <asp:ListItem Value="0">Export to</asp:ListItem>
                                        <asp:ListItem Value="1">PDF</asp:ListItem>
                                        <asp:ListItem Value="2">XLS</asp:ListItem>
                                        <asp:ListItem Value="3">RTF</asp:ListItem>
                                        <asp:ListItem Value="4">CSV</asp:ListItem>
                                    </asp:DropDownList>
                                    <% } %>
                                    <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="true" PaperKind="A3" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
                                    </dxe:ASPxGridViewExporter>

                                    <asp:HiddenField runat="server" ID="branchList" />
                                    <asp:HiddenField runat="server" ID="IsPeriod" />
                                </td>
                            </tr>
                            <tr id="TrAccount" runat="Server" class="hide">
                                <td class="gridcellleft">Segment :
                                </td>
                                <td>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:RadioButton ID="rdbMainAll" runat="server" GroupName="a" />
                                            </td>
                                            <td>All
                                            </td>
                                            <td>
                                                <asp:RadioButton ID="rdbMainSelected" runat="server" Checked="True" GroupName="a" />
                                            </td>
                                            <td>Selected
                                            </td>
                                            <td>[<span id="litSegment" runat="server" style="color: Maroon"></span>]
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>

                            <tr>
                                <td></td>

                            </tr>
                        </table>
                    </td>
                    <td style="vertical-align: top">
                        <table>
                            <tr>
                                <td class="gridcellleft" style="vertical-align: top; text-align: right" id="TdFilter">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtsubscriptionID" runat="server" Font-Size="12px" Width="193px"></asp:TextBox>
                                                <a id="A4" href="javascript:void(0);" onclick="btnAddsubscriptionlist_click()"><span
                                                    style="color: #009900; text-decoration: underline; font-size: 8pt;">Add to List</span></a><span
                                                        style="color: #009900; font-size: 8pt;"> </span>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right; vertical-align: top">
                                    <table cellpadding="0" cellspacing="0" id="TdSelect">
                                        <tr>
                                            <td>
                                                <asp:ListBox ID="lstSuscriptions" runat="server" Font-Size="12px" Height="90px" Width="253px"></asp:ListBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: center">
                                                <table cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td>
                                                            <a id="A2" href="javascript:void(0);" onclick="clientselectionfinal()"><span style="color: #000099; text-decoration: underline; font-size: 10pt;">Done</span></a>&nbsp;&nbsp;
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
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
                            <ProgressTemplate>
                                <div id='Div1' style='position: absolute; font-family: arial; font-size: 30; left: 100; top: 8; background-color: white; layer-background-color: white; height: 80; width: 150;'>
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
                <tr style="text-align: right" id="Trfilter">
                    <td style="text-align: right" colspan="2">
                        <a href="#" style="font-weight: bold; color: Blue" onclick="javascript:filter();">Filter</a>
                        ||
                        <asp:DropDownList ID="ddlExport" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlExport_SelectedIndexChanged">
                            <asp:ListItem Value="Ex" Selected="True">Export</asp:ListItem>
                            <asp:ListItem Value="E">Excel</asp:ListItem>
                            <asp:ListItem Value="P">PDF</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td id="showDetail">
                        <span id="spanshow" style="color: Blue; font-weight: bold"></span>&nbsp;&nbsp;<span
                            id="spanshow1" style="color: Blue; font-weight: bold"></span>&nbsp;&nbsp;<span id="spanshow2"
                                style="color: Blue; font-weight: bold"></span>&nbsp;&nbsp;<span id="spanshow3"></span>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" id="TdAsOnDate">
                        <table style="width: 100%">
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:GridView ID="grdGeneralTrial" runat="server" Width="100%" BorderColor="CornflowerBlue"
                                                ShowFooter="True" AutoGenerateColumns="false" AllowPaging="true" BorderStyle="Solid"
                                                AllowSorting="true" BorderWidth="2px" CellPadding="4" ForeColor="#0000C0" PageSize="20000"
                                                OnRowCreated="grdSubsidiaryTrial_RowCreated" OnRowDataBound="grdGeneralTrial_RowDataBound"
                                                OnSorting="grdGeneralTrial_Sorting">
                                                <FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></FooterStyle>
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Main Account" SortExpression="accountsledger_mainaccountid">
                                                        <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                        <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblMainAccount" runat="server" Text='<%# Eval("accountsledger_mainaccountid")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Main Account Code" SortExpression="MainAccID">
                                                        <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                        <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblSubAccount" runat="server" Text='<%# Eval("MainAccID")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Detail">
                                                        <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                        <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                                        <ItemTemplate>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Debit">
                                                        <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="right"></ItemStyle>
                                                        <HeaderStyle Wrap="False" HorizontalAlign="center" Font-Bold="False"></HeaderStyle>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDebit" runat="server" Text='<%# Eval("AmountDR")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Credit">
                                                        <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="right"></ItemStyle>
                                                        <HeaderStyle Wrap="False" HorizontalAlign="Center" Font-Bold="False"></HeaderStyle>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblCredit" runat="server" Text='<%# Eval("AmountCR")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Credit" Visible="false">
                                                        <ItemStyle BorderWidth="1px" HorizontalAlign="right"></ItemStyle>
                                                        <HeaderStyle Wrap="False" HorizontalAlign="Center" Font-Bold="False"></HeaderStyle>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblMainAcc" runat="server" Text='<%# Eval("ID")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Credit" Visible="false">
                                                        <ItemStyle BorderWidth="1px" HorizontalAlign="right"></ItemStyle>
                                                        <HeaderStyle Wrap="False" HorizontalAlign="Center" Font-Bold="False"></HeaderStyle>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblSubLedgerType" runat="server" Text='<%# Eval("SubLedgerType")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="CashBank" Visible="false">
                                                        <ItemStyle BorderWidth="1px" HorizontalAlign="right"></ItemStyle>
                                                        <HeaderStyle Wrap="False" HorizontalAlign="Center" Font-Bold="False"></HeaderStyle>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblCashBankType" runat="server" Text='<%# Eval("CashBankType")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <PagerTemplate>
                                                    <table width="100%">
                                                        <tr>
                                                            <td colspan="4" style="text-align: right">[Records
                                                                <%if (grdGeneralTrial.PageCount == grdGeneralTrial.PageIndex + 1)
                                                                  {%>
                                                                <%= grdGeneralTrial.PageIndex * grdGeneralTrial.PageSize%>
                                                                -
                                                                <%= grdGeneralTrial.PageIndex * grdGeneralTrial.PageSize + grdGeneralTrial.Rows.Count-1%>
                                                                <%}
                                                                  else
                                                                  {%>
                                                                <%= grdGeneralTrial.PageIndex * grdGeneralTrial.PageSize%>
                                                                -
                                                                <%= grdGeneralTrial.PageIndex * grdGeneralTrial.PageSize + grdGeneralTrial.PageSize - 1 %>
                                                                <%}%>
                                                                ]
                                                                <asp:LinkButton ID="FirstPage" runat="server" Font-Bold="true" CommandName="First"
                                                                    OnCommand="NavigationLink_Click" Text="[First Page]"> </asp:LinkButton>
                                                                <asp:LinkButton ID="PreviousPage" runat="server" Font-Bold="true" CommandName="Prev"
                                                                    OnCommand="NavigationLink_Click" Text="[Previous Page]">  </asp:LinkButton>
                                                                <asp:LinkButton ID="NextPage" runat="server" Font-Bold="true" CommandName="Next"
                                                                    OnCommand="NavigationLink_Click" Text="[Next Page]">
                                                                </asp:LinkButton>
                                                                <asp:LinkButton ID="LastPage" runat="server" Font-Bold="true" CommandName="Last"
                                                                    OnCommand="NavigationLink_Click" Text="[Last Page]">
                                                                </asp:LinkButton>
                                                            </td>
                                                            <td colspan="2" style="text-align: right; font-weight: bold">
                                                                <asp:Literal ID="litDiff" runat="server"></asp:Literal>
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
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="btnShow" EventName="Click" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <div class="GridViewArea assOndate">
                            <dxe:ASPxGridView ID="aspxGdGeneralTrial" runat="server" AutoGenerateColumns="False" ClientInstanceName="cgdGeneralTrial"
                                Width="100%" CssClass="pull-left" OnCustomCallback="aspxGdGeneralTrial_CustomCallback" OnDataBinding="grid_DataBinding"
                                SettingsPager-Mode="ShowAllRecords" Settings-VerticalScrollBarMode="auto" Settings-VerticalScrollableHeight="530" OnHtmlRowPrepared="aspxGdGeneralTrial_HtmlRowPrepared">

                                <SettingsPager Mode="ShowAllRecords"></SettingsPager>

                                <Settings ShowStatusBar="Hidden" ShowFilterRow="false" ShowFilterRowMenu="false" />
                                <Columns>

                                    <dxe:GridViewDataTextColumn Caption="Main Account" FieldName="accountsledger_mainaccountid" ReadOnly="True"
                                        Visible="True" FixedStyle="Left" VisibleIndex="0">
                                        <EditFormSettings Visible="True" />
                                        <Settings AutoFilterCondition="Contains" />
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn Caption="Main Account Code" FieldName="MainAccID" ReadOnly="True"
                                        Visible="True" FixedStyle="Left" VisibleIndex="1">
                                        <EditFormSettings Visible="True" />
                                        <Settings AutoFilterCondition="Contains" />
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn Caption="Detail" ReadOnly="True"
                                        Visible="True" FixedStyle="Left" VisibleIndex="2">
                                        <EditFormSettings Visible="True" />
                                        <Settings AutoFilterCondition="Contains" />
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn Caption="Debit" FieldName="AmountDr" ReadOnly="True"
                                        Visible="True" FixedStyle="Left" VisibleIndex="3">
                                        <Settings AutoFilterCondition="Contains" />
                                        <EditFormSettings Visible="True" />
                                        <CellStyle Wrap="False" HorizontalAlign="Right" CssClass="gridcellleft"></CellStyle>
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn Caption="Credit" FieldName="AmountCr" ReadOnly="True"
                                        Visible="True" FixedStyle="Left" VisibleIndex="4">
                                        <Settings AutoFilterCondition="Contains" />
                                        <EditFormSettings Visible="True" />
                                        <CellStyle Wrap="False" HorizontalAlign="Right" CssClass="gridcellleft"></CellStyle>
                                    </dxe:GridViewDataTextColumn>


                                </Columns>
                                <SettingsBehavior ColumnResizeMode="NextColumn" />
                                <ClientSideEvents EndCallback="cgdGeneralTrialEndCallBack" />
                            </dxe:ASPxGridView>
                        </div>

                    </td>
                </tr>
                <tr>
                    <td colspan="2" id="TdPeriod">
                        <table style="width: 100%">
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:GridView ID="grdPeriod" runat="server" Width="100%" BorderColor="CornflowerBlue"
                                                ShowFooter="True" AutoGenerateColumns="false" AllowPaging="True" BorderStyle="Solid"
                                                AllowSorting="true" BorderWidth="2px" CellPadding="4" ForeColor="#0000C0" PageSize="20000"
                                                OnRowCreated="grdPeriod_RowCreated" OnRowDataBound="grdPeriod_RowDataBound">
                                                <FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></FooterStyle>
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Main Account">
                                                        <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                        <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblMainAccount" runat="server" Text='<%# Eval("accountsledger_mainaccountid")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Code">
                                                        <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                        <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblSubAccount" runat="server" Text='<%# Eval("MainAccID")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Detail">
                                                        <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                        <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                                        <ItemTemplate>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Opening Dr">
                                                        <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="right"></ItemStyle>
                                                        <HeaderStyle Wrap="False" HorizontalAlign="Center" Font-Bold="False"></HeaderStyle>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblCredit" runat="server" Text='<%# Eval("OpeningDr")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Opening Cr">
                                                        <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="right"></ItemStyle>
                                                        <HeaderStyle Wrap="False" HorizontalAlign="center" Font-Bold="False"></HeaderStyle>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDebit" runat="server" Text='<%# Eval("OpeningCr")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Amount Dr">
                                                        <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="right"></ItemStyle>
                                                        <HeaderStyle Wrap="False" HorizontalAlign="center" Font-Bold="False"></HeaderStyle>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDebit1" runat="server" Text='<%# Eval("AmountDR")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Amount Cr">
                                                        <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="right"></ItemStyle>
                                                        <HeaderStyle Wrap="False" HorizontalAlign="Center" Font-Bold="False"></HeaderStyle>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblCredit1" runat="server" Text='<%# Eval("AmountCR")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Closing Dr">
                                                        <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="right"></ItemStyle>
                                                        <HeaderStyle Wrap="False" HorizontalAlign="center" Font-Bold="False"></HeaderStyle>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDebit2" runat="server" Text='<%# Eval("ClosingDr")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Closing Cr">
                                                        <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="right"></ItemStyle>
                                                        <HeaderStyle Wrap="False" HorizontalAlign="Center" Font-Bold="False"></HeaderStyle>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblCredit2" runat="server" Text='<%# Eval("ClosingCr")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Credit" Visible="false">
                                                        <ItemStyle BorderWidth="1px" HorizontalAlign="right"></ItemStyle>
                                                        <HeaderStyle Wrap="False" HorizontalAlign="Center" Font-Bold="False"></HeaderStyle>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblMainAcc" runat="server" Text='<%# Eval("ID")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Credit" Visible="false">
                                                        <ItemStyle BorderWidth="1px" HorizontalAlign="right"></ItemStyle>
                                                        <HeaderStyle Wrap="False" HorizontalAlign="Center" Font-Bold="False"></HeaderStyle>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblSubLedgerType" runat="server" Text='<%# Eval("SubLedgerType")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="CashBank" Visible="false">
                                                        <ItemStyle BorderWidth="1px" HorizontalAlign="right"></ItemStyle>
                                                        <HeaderStyle Wrap="False" HorizontalAlign="Center" Font-Bold="False"></HeaderStyle>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblCashBankType" runat="server" Text='<%# Eval("CashBankType")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <PagerTemplate>
                                                    <table width="100%">
                                                        <tr>
                                                            <td colspan="4" style="text-align: center">
                                                                <asp:LinkButton ID="FirstPage" runat="server" Font-Bold="true" CommandName="First"
                                                                    OnCommand="NavigationLinkPeriod_Click" Text="[First Page]"> </asp:LinkButton>
                                                                <asp:LinkButton ID="PreviousPage" runat="server" Font-Bold="true" CommandName="Prev"
                                                                    OnCommand="NavigationLinkPeriod_Click" Text="[Previous Page]">  </asp:LinkButton>
                                                                <asp:LinkButton ID="NextPage" runat="server" Font-Bold="true" CommandName="Next"
                                                                    OnCommand="NavigationLinkPeriod_Click" Text="[Next Page]">
                                                                </asp:LinkButton>
                                                                <asp:LinkButton ID="LastPage" runat="server" Font-Bold="true" CommandName="Last"
                                                                    OnCommand="NavigationLinkPeriod_Click" Text="[Last Page]">
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
                                                <HeaderStyle ForeColor="Black" BorderWidth="1px" BackColor="#C6D6FD" BorderColor="AliceBlue"
                                                    Font-Bold="False"></HeaderStyle>
                                                <%--<AlternatingRowStyle BackColor="White"></AlternatingRowStyle>--%>
                                            </asp:GridView>
                                            <asp:HiddenField ID="CurrentPagePeriod" runat="server"></asp:HiddenField>
                                            <asp:HiddenField ID="TotalPagesPeriod" runat="server"></asp:HiddenField>
                                            <asp:HiddenField ID="TotalClientPeriod" runat="server" />
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="btnShow" EventName="Click" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <div class="GridViewArea datePeriod">
                            <dxe:ASPxGridView ID="aspxPeriodGrid" runat="server" AutoGenerateColumns="False" ClientInstanceName="caspxPeriodGrid"
                                Width="100%" CssClass="pull-left" OnCustomCallback="aspxPeriodGrid_CustomCallback" OnDataBinding="aspxPeriodGrid_DataBinding"
                                SettingsPager-Mode="ShowAllRecords" Settings-VerticalScrollBarMode="auto" Settings-VerticalScrollableHeight="530" OnHtmlRowPrepared="aspxPeriodGrid_HtmlRowPrepared">

                                <SettingsPager Mode="ShowAllRecords"></SettingsPager>

                                <Settings ShowStatusBar="Hidden" ShowFilterRow="false" ShowFilterRowMenu="false" />
                                <Columns>

                                    <dxe:GridViewDataTextColumn Caption="Main Account" FieldName="accountsledger_mainaccountid" ReadOnly="True"
                                        Visible="True" FixedStyle="Left" VisibleIndex="0">
                                        <EditFormSettings Visible="True" />
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn Caption="Code" FieldName="MainAccID" ReadOnly="True"
                                        Visible="True" FixedStyle="Left" VisibleIndex="1">
                                        <EditFormSettings Visible="True" />
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn Caption="Detail" ReadOnly="True"
                                        Visible="True" FixedStyle="Left" VisibleIndex="2">
                                        <EditFormSettings Visible="True" />
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn Caption="Opening Dr" FieldName="OpeningDr" ReadOnly="True"
                                        Visible="True" FixedStyle="Left" VisibleIndex="3">
                                        <EditFormSettings Visible="True" />
                                        <CellStyle Wrap="False" HorizontalAlign="Right" CssClass="gridcellleft"></CellStyle>
                                    </dxe:GridViewDataTextColumn>

                                    <dxe:GridViewDataTextColumn Caption="Opening Cr" FieldName="OpeningCr" ReadOnly="True"
                                        Visible="True" FixedStyle="Left" VisibleIndex="4">
                                        <EditFormSettings Visible="True" />
                                        <CellStyle Wrap="False" HorizontalAlign="Right" CssClass="gridcellleft"></CellStyle>
                                    </dxe:GridViewDataTextColumn>

                                    <dxe:GridViewDataTextColumn Caption="Amount Dr" FieldName="AmountDR" ReadOnly="True"
                                        Visible="True" FixedStyle="Left" VisibleIndex="4">
                                        <EditFormSettings Visible="True" />
                                        <CellStyle Wrap="False" HorizontalAlign="Right" CssClass="gridcellleft"></CellStyle>
                                    </dxe:GridViewDataTextColumn>

                                    <dxe:GridViewDataTextColumn Caption="Amount Cr" FieldName="AmountCR" ReadOnly="True"
                                        Visible="True" FixedStyle="Left" VisibleIndex="4">
                                        <EditFormSettings Visible="True" />
                                        <CellStyle Wrap="False" HorizontalAlign="Right" CssClass="gridcellleft"></CellStyle>
                                    </dxe:GridViewDataTextColumn>

                                    <dxe:GridViewDataTextColumn Caption="Closing Dr" FieldName="ClosingDr" ReadOnly="True"
                                        Visible="True" FixedStyle="Left" VisibleIndex="4">
                                        <EditFormSettings Visible="True" />
                                        <CellStyle Wrap="False" HorizontalAlign="Right" CssClass="gridcellleft"></CellStyle>
                                    </dxe:GridViewDataTextColumn>

                                    <dxe:GridViewDataTextColumn Caption="Closing Cr" FieldName="ClosingCr" ReadOnly="True"
                                        Visible="True" FixedStyle="Left" VisibleIndex="4">
                                        <EditFormSettings Visible="True" />
                                        <CellStyle Wrap="False" HorizontalAlign="Right" CssClass="gridcellleft"></CellStyle>
                                    </dxe:GridViewDataTextColumn>

                                    <dxe:GridViewDataTextColumn Caption="Credit" FieldName="ID" ReadOnly="True"
                                        Visible="False" FixedStyle="Left" VisibleIndex="4">
                                        <Settings AutoFilterCondition="Contains" />
                                        <EditFormSettings Visible="True" />
                                    </dxe:GridViewDataTextColumn>

                                    <dxe:GridViewDataTextColumn Caption="Credit" FieldName="SubLedgerType" ReadOnly="True"
                                        Visible="False" FixedStyle="Left" VisibleIndex="4">
                                        <Settings AutoFilterCondition="Contains" />
                                        <EditFormSettings Visible="True" />
                                    </dxe:GridViewDataTextColumn>

                                    <dxe:GridViewDataTextColumn Caption="CashBank" FieldName="CashBankType" ReadOnly="True"
                                        Visible="False" FixedStyle="Left" VisibleIndex="4">
                                        <Settings AutoFilterCondition="Contains" />
                                        <EditFormSettings Visible="True" />
                                    </dxe:GridViewDataTextColumn>

                                </Columns>
                                <SettingsBehavior ColumnResizeMode="NextColumn" />
                                <ClientSideEvents EndCallback="aspxPeriodGridEndCallBack" />
                            </dxe:ASPxGridView>
                        </div>
                    </td>

                </tr>

                <tr>
                    <td colspan="2">
                        <asp:HiddenField ID="hdnSegment" runat="server" />
                        <asp:HiddenField ID="HDNAccInd" runat="server" />
                    </td>
                </tr>
            </table>

            <%--View PopUp Debjyoti--%>
            <dxe:ASPxPopupControl ID="ASPXPopupControl" runat="server"
                CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="popup" Height="630px"
                Width="1200" HeaderText="General Trial Details" Modal="true" AllowResize="true" ResizingMode="Postponed">
                <ContentCollection>
                    <dxe:PopupControlContentControl runat="server">
                    </dxe:PopupControlContentControl>
                </ContentCollection>
            </dxe:ASPxPopupControl>

        </div>
</asp:Content>
