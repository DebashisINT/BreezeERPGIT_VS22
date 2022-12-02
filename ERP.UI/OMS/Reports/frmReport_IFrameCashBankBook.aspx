<%@ Page Language="C#" AutoEventWireup="true" EnableEventValidation="false" MasterPageFile="~/OMS/MasterPage/PopUp.Master"
    Inherits="ERP.OMS.Reports.Reports_frmReport_IFrameCashBankBook" Codebehind="frmReport_IFrameCashBankBook.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../CSS/style.css" rel="stylesheet" type="text/css" />
    <%--<script type="text/javascript" src="/assests/js/loaddata.js"></script>--%>

    <script type="text/javascript" src="/assests/js/init.js"></script>

    <script type="text/javascript" src="/assests/js/ajax-dynamic-list_rootfile.js"></script>

    

    <link rel="stylesheet" href="../windowfiles/dhtmlwindow.css" type="text/css" />

    <script type="text/javascript" src="../windowfiles/dhtmlwindow.js"></script>

    <link rel="stylesheet" href="../modalfiles/modal.css" type="text/css" />

    <script type="text/javascript" src="../modalfiles/modal.js"></script>

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
    <style>
        .radio {
            margin-top:0;
            margin-bottom:0;
        }
    </style>


    <script language="javascript" type="text/javascript">

        function ShowDateRange() {

          //  document.getElementById("ShowDt").style.display = 'none';
          //  document.getElementById("ShowHd").style.display = 'inline';


        }
        function ShowDateSelect() {
          //  document.getElementById("ShowDt").style.display = 'inline';
          //  document.getElementById("ShowHd").style.display = 'none';

        }

        function SignOff() {
            window.parent.SignOff();
        }
        FieldName = 'Button1';
        function showOptions(obj1, obj2, obj3) {
            var cmb = document.getElementById('cmbsearchOption');
            //alert(cmb.value);
            ajax_showOptions(obj1, obj2, obj3, cmb.value);
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
            window.frameElement.widht = document.body.scrollWidht;
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
                alert('Please search name and then Add!')
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
        function Page_Load() {

            document.getElementById('showFilter').style.display = 'none';
            document.getElementById('TdFilter').style.display = 'none';
            document.getElementById('Trfilter').style.display = 'none';
            document.getElementById('TrSearch').style.display = 'none';
            document.getElementById("showDetail").style.display = 'none';
            height();
        }
        function SegAll(obj) {
            document.getElementById('showFilter').style.display = 'none';
            document.getElementById('TdFilter').style.display = 'none';
            if (obj == 'seg') {

            }
            else {
                document.getElementById('litBranch').innerText = '';
                document.getElementById('Button1').disabled = false;
            }
        }
        function SegSelected(obj) {
            document.getElementById('showFilter').style.display = 'inline';
            document.getElementById('TdFilter').style.display = 'inline';
            if (obj == 'seg') {
                document.getElementById('cmbsearchOption').value = 'Segment';
            }
            else {
                document.getElementById('cmbsearchOption').value = 'Branch';
                document.getElementById('Button1').disabled = true;
            }
        }
        function CheckDataExists() {
            alert('No Data Found');
        }
        function Disable(obj) {
            var gridview = document.getElementById('grdCashBankBook');
            var rCount = gridview.rows.length;
            if (rCount < 10)
                rCount = '0' + rCount;
            if (obj == 'P') {
              //  document.getElementById("grdCashBankBook_ctl09_FirstPage").style.display = 'none';
                document.getElementById("grdCashBankBook_ctl09_PreviousPage").style.display = 'none';
                document.getElementById("grdCashBankBook_ctl09_NextPage").style.display = 'inline';
                document.getElementById("grdCashBankBook_ctl09_LastPage").style.display = 'inline';
            }
            else {
                document.getElementById("grdCashBankBook_ctl" + rCount + "_NextPage").style.display = 'none';
                document.getElementById("grdCashBankBook_ctl" + rCount + "_LastPage").style.display = 'none';
            }
        }
        function ShowHide(obj, obj1) {

            if (document.getElementById("HDNMAIN").value != '') {
                document.getElementById("trDateRange").style.display = 'inline';
               // document.getElementById("ShowDt").style.display = 'none';
              //  document.getElementById("ShowHd").style.display = 'inline';
                // document.getElementById("showDetail").style.display='none';

            }
            document.getElementById("TrAll").style.display = 'none';
            document.getElementById("Trfilter").style.display = 'inline';
            document.getElementById("showDetail").style.display = 'inline';
            document.getElementById("spanshow1").innerText = obj;
            document.getElementById("spanshow3").innerText = obj1;
            height();

        }
        function filter() {
            document.getElementById("Trfilter").style.display = 'none';
            document.getElementById("TrAll").style.display = 'inline';
            document.getElementById('TrSearch').style.display = 'none';
            height();
        }
        function search1() {
            document.getElementById('TrSearch').style.display = 'inline';
            height();
        }
        function ShowHideSearch() {
            document.getElementById('TrSearch').style.display = 'none';
            document.getElementById('txtVouno').value = 'Voucher Number';
            document.getElementById('txtInstNo').value = 'Instrument Number';
            //document.getElementById('dtSearchDate_I').GetValue() ='01-01-0100';
            height();
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
        function ColourChange(obj) {
            //obj1.parentElement.parentElement.style.backgroundColor='#EFF3FB';
            //obj.style.backgroundColor='#EFF3FB';
            if (obj.click)
                obj.style.backgroundColor = '#FFE1AC';
            //	        else
            //	            obj.style.backgroundColor='#EFF3FB';
        }
        function updateCashBankDetail(objDate, objVouNo, objMainID, objSubID, objCompID, objSegID) {
            var URL = 'CashBankEntryEdit.aspx?date=' + objDate + ' &vNo=' + objVouNo + ' &Compid=' + objCompID + ' &SegID=' + objSegID;
            editwin = dhtmlmodal.open("Editbox", "iframe", URL, "Update Cash/Bank Book", "width=940px,height=450px,center=1,resize=1,top=500", "recal");
            // parent.ParentWindowShow(URL);
            //editwin=dhtmlmodal.open("Editbox", "iframe", URL, "Modify Employee Details", "width=940px,height=450px,center=1,resize=1,scrolling=2,top=500", "recal")

        }
        function updateCashBankDetailDHTML(objDate, objVouNo, objMainID, objSubID, objCompID, objSegID) {

            var URL = 'CashBankEntryEdit.aspx?date=' + objDate + ' &vNo=' + objVouNo + ' &Compid=' + objCompID + ' &SegID=' + objSegID;
            editwin = dhtmlmodal.open("Editbox", "iframe", URL, "Update Cash/Bank Book", "width=940px,height=450px,center=1,resize=1,top=500", "recal");
        }
        function callback() {
            var btn = document.getElementById('Button1');
            btn.click();
        }
        function frmOpenNewWindow1(location, v_height, v_weight) {
            var y = (screen.availHeight - v_height) / 2;
            var x = (screen.availWidth - v_weight) / 2;
            window.open(location, "Search_Conformation_Box", "height=" + v_height + ",width=" + v_weight + ",top=" + y + ",left=" + x + ",location=no,directories=no,menubar=no,toolbar=no,status=yes,scrollbars=no,resizable=no,dependent=no");
        }
        function FillValues() {

            var btn = document.getElementById('Button1');
            btn.click();
        }
        function CallHeight() {
            height();
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
        function selecttion() {
            var combo = document.getElementById('ddlExport');
            combo.value = 'Ex'; 
            caspxGrdCashBankBook.PerformCallback('');
            caspxGrdCashBankBook.PerformCallback('');
            return false;
        }
        //    function callback()
        //     {
        //     alert("das")
        //     grdCashBankBook.PerformCallback();
        //     }

        function DateChangeForFrom() {
            //var datePost=(dtFrom.GetDate().getMonth()+2)+'-'+dtFrom.GetDate().getDate()+'-'+dtFrom.GetDate().getYear();
            var sessionValFrom = "<%=Session["FinYearStart"]%>";
        var sessionValTo = "<%=Session["FinYearEnd"]%>";
        var sessionVal = "<%=Session["LastFinYear"]%>";
        var objsession = sessionVal.split('-');
        var MonthDate = dtDate.GetDate().getMonth() + 1;
        var DayDate = dtDate.GetDate().getDate();
        var YearDate = dtDate.GetDate().getYear();
        var Cdate = MonthDate + "/" + DayDate + "/" + YearDate;
        var Sto = new Date(sessionValTo).getMonth() + 1;
        var SFrom = new Date(sessionValFrom).getMonth() + 1;
        var SDto = new Date(sessionValTo).getDate();
        var SDFrom = new Date(sessionValFrom).getDate();

        if (YearDate >= objsession[0]) {
            if (MonthDate < SFrom && YearDate == objsession[0]) {
                alert('Enter Date Is Outside Of Financial Year !!');
                var datePost = (SFrom + '-' + SDFrom + '-' + objsession[0]);
                dtDate.SetDate(new Date(datePost));
            }
            else if (MonthDate > Sto && YearDate == objsession[1]) {
                alert('Enter Date Is Outside Of Financial Year !!');
                var datePost = (SFrom + '-' + SDFrom + '-' + objsession[0]);
                dtDate.SetDate(new Date(datePost));
            }
            else if (YearDate != objsession[0] && YearDate != objsession[1]) {
                alert('Enter Date Is Outside Of Financial Year !!');
                var datePost = (SFrom + '-' + SDFrom + '-' + objsession[0]);
                dtDate.SetDate(new Date(datePost));
            }
        }
        else {
            alert('Enter Date Is Outside Of Financial Year !!');
            var datePost = (SFrom + '-' + SDFrom + '-' + objsession[0]);
            dtDate.SetDate(new Date(datePost));
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
                        val = "'" + items[1] + "'";

                    }
                    else {
                        seg += ',' + items[0];
                        val += ",'" + items[1] + "'";

                    }
                }

                combo.innerText = val;
                document.getElementById('HDNSeg').value = seg;

                //                var combo = document.getElementById('litSegment');
                //                var NoItems=Data[1].split(',');
                //                var i;
                //                var val='';
                //                for(i=0;i<NoItems.length;i++)
                //                {
                //                    var items = NoItems[i].split(';');
                //                    if(val=='')
                //                    {
                //                        val=items[1];
                //                    }
                //                    else
                //                    {
                //                        val+=','+items[1];
                //                    }
                //                    alert(val);
                //                }
                //                combo.innerText=val;
            }
            if (Data[0] == 'Branch') {
                var combo = document.getElementById('litBranch');
                var NoItems = Data[1].split(',');
                var i;
                var val = '';
                for (i = 0; i < NoItems.length; i++) {
                    var items = NoItems[i].split(';');
                    var items1 = items[1].split('-');
                    if (val == '') {
                        val = '(' + items1[1];
                    }
                    else {
                        val += ',' + items1[1];
                    }
                }
                val = val + ')';
                combo.innerText = val;
            }
        }
        function CallBankAccount(obj1, obj2, obj3) {
            var CurrentSegment = '<%=Session["usersegid"]%>'
        if (CurrentSegment.length == 8)
            CurrentSegment = document.getElementById("hdn_NsdlCdslExchangeSegment").value;
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

    </script>
  <%--   <script language="javascript" type="text/javascript">
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
            </script>--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <div class="panel-heading">
        <div class="panel-title">
            <h3>Cash/Bank Book</h3>
        </div>

    </div> 
         <div class="form_main">
          
           

            <table class="TableMain100">
                <tr id="TrAll" class="hide">
                    <td colspan="2">
                        <table width="100%">
                            <tr>
                                <td>
                                    <table cellspacing="1" cellpadding="2" width="500px" >
                                        <tr>
                                            <td class="gridcellleft" style="width: 75px">
                                                Bank Name
                                            </td>
                                            <td style="text-align: left; width: 196px;">
                                                <asp:TextBox ID="txtBankName" runat="server" Font-Size="12px" Width="296px" 
                                                onkeyup ="CallBankAccount(this,'GenericAjaxList',event)"></asp:TextBox>
                                                <asp:HiddenField ID="txtBankName_hidden" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="gridcellleft" style="width: 58px">
                                                Segment
                                            </td>
                                            <td style="text-align: left;" colspan="2">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <div class="radio">
                                                              <label>
                                                                <asp:RadioButton ID="rdbSegAll" runat="server"  Checked="True"  GroupName="a" />
                                                                All
                                                              </label>
                                                            </div>
                                                            
                                                        </td>
                                                        <td style="padding-left:15px;">
                                                           <div class="radio">
                                                              <label>
                                                                <asp:RadioButton ID="rdbSegSelected" runat="server"    GroupName="a" />
                                                                Selected
                                                              </label>
                                                            </div> 
                                                        </td>
                                                        <td>
                                                            
                                                        </td>
                                                        <td>
                                                            
                                                        </td>
                                                        <td>
                                                            <span id="litSegment" runat="server" style="color: Maroon"></span>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="gridcellleft" style="width: 58px">
                                                Branch
                                            </td>
                                            <td style="text-align: left;" colspan="2">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <div class="radio">
                                                              <label>
                                                                <asp:RadioButton ID="rdbbAll" runat="server" Checked="True" GroupName="b" />
                                                                All
                                                              </label>
                                                            </div>
                                                            
                                                        </td>
                                                        <td style="padding-left:15px">
                                                            <div class="radio">
                                                              <label>
                                                                <asp:RadioButton ID="rdbbSelected" runat="server" GroupName="b" />
                                                                Selected
                                                              </label>
                                                            </div>
                                                        </td>
                                                        
                                                        <td>
                                                            <span id="litBranch" runat="server" style="color: Maroon"></span>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="gridcellleft" style="width: 58px">
                                                Date
                                            </td>
                                            <td>
                                                <table>
                                                    <tr>
                                                        <td style="width: 196px">
                                                            <dxe:ASPxDateEdit ID="dtDate" runat="server" EditFormat="Custom" ClientInstanceName="dtDate"
                                                                UseMaskBehavior="True">
                                                                <dropdownbutton text="From ">
                                        </dropdownbutton>
                                                                <clientsideevents datechanged="function(s,e){DateChangeForFrom();}" />
                                                            </dxe:ASPxDateEdit>
                                                        </td>
                                                        <td>
                                                            <dxe:ASPxDateEdit ID="dtToDate" runat="server" EditFormat="Custom" ClientInstanceName="dtToDate"
                                                                UseMaskBehavior="True">
                                                                <dropdownbutton text="To">
                                        </dropdownbutton>
                                                                <clientsideevents datechanged="function(s,e){DateChangeForTo();}" />
                                                            </dxe:ASPxDateEdit>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr id="TrBtn">
                                            <td></td>
                                            <td >
                                                <asp:Button ID="Button1" runat="server" Text="Show" OnClick="Button1_Click" CssClass="btnUpdate btn btn-primary"
                                                 OnClientClick="return selecttion()"  />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td>
                                      <table width="100%" id="showFilter">
                                        <tr>
                                            <td class="gridcellleft" style="vertical-align: top;" id="TdFilter">
                                                <asp:TextBox ID="txtsubscriptionID" runat="server" Font-Size="12px" Width="220px"></asp:TextBox>
                                                <asp:DropDownList ID="cmbsearchOption" runat="server" Font-Size="11px" Width="70px"
                                                    Enabled="false">
                                                    <asp:ListItem>Segment</asp:ListItem>
                                                    <asp:ListItem>Branch</asp:ListItem>
                                                </asp:DropDownList>
                                                <a id="A4" href="javascript:void(0);" onclick="btnAddsubscriptionlist_click()"><span
                                                    style="color: #009900; text-decoration: underline; font-size: 8pt;">Add to List</span></a><span
                                                        style="color: #009900; font-size: 8pt;"> </span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: left; vertical-align: top">
                                                <table cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td class="gridcellleft" >
                                                            <asp:ListBox ID="lstSuscriptions" runat="server" Font-Size="12px" Height="90px" Width="300px">
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
                                                <asp:HiddenField ID="HDNMAIN" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr class="hide">
                    <td>
                        <table id="TrSearch">
                            <tr>
                                <td>
                                    <dxe:ASPxDateEdit ID="dtSearchDate" runat="server" CssClass="water" NullText="Tr Date"
                                        ToolTip="Tr Date" EditFormat="Custom" Font-Size="12px" UseMaskBehavior="True">
                                    </dxe:ASPxDateEdit>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtVouno" runat="server" CssClass="water" Text="Voucher Number"
                                        ToolTip="Voucher Number" Font-Size="12px" Width="150px"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtInstNo" runat="server" CssClass="water" Text="Instrument Number"
                                        ToolTip="Instrument Number" Font-Size="12px" Width="150px"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btnUpdate" Height="17px"
                                        OnClick="btnSearch_Click" />
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td style="text-align: right" id="Trfilter">
                        <a href="#" style="font-weight: bold; color: Blue" onclick="javascript:filter();">Filter</a>
                        || <a href="#" style="font-weight: bold; color: Blue" onclick="javascript:search1();">
                            Search</a>
                        <asp:DropDownList ID="ddlExport" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlExport_SelectedIndexChanged">
                            <asp:ListItem Value="Ex" Selected="True">Export</asp:ListItem>
                            <asp:ListItem Value="E">Excel</asp:ListItem>
                            <asp:ListItem Value="P">PDF</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr class="hide">
                    <td colspan="2" id="showDetail">
                        <span id="spanshow" style="color: Blue; font-weight: bold">Bank Name :</span>&nbsp;&nbsp;<span
                            id="spanshow1"></span>&nbsp;&nbsp;<span id="spanshow2" style="color: Blue; font-weight: bold">
                                Period :</span>&nbsp;&nbsp;<span id="spanshow3"></span>
                    </td>
                </tr>
                <tr class="hide">
                    <td colspan="2">
                        <table>
                            <tr>
                                <td>
                                    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
                                        <ProgressTemplate>
                                            <div id='Div2' style='position: absolute; font-family: arial; font-size: 30; left: 50%;
                                                top: 5%; background-color: white; layer-background-color: white; height: 80;
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
                                <td colspan="3">
                                    <% if (rights.CanExport)
                                               { %>
                                    <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-sm btn-primary" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged"   AutoPostBack="true" >
                                        <asp:ListItem Value="0">Export to</asp:ListItem>
                                        <asp:ListItem Value="1">PDF</asp:ListItem>
                                        <asp:ListItem Value="2">XLS</asp:ListItem>
                                        <asp:ListItem Value="3">RTF</asp:ListItem>
                                        <asp:ListItem Value="4">CSV</asp:ListItem>
                                    </asp:DropDownList>
                                     <% } %>

                                    <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="true" PaperKind="A3" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
                                    </dxe:ASPxGridViewExporter>
                                            </td>

                            </tr>



                <tr id="trDateRange" >
                    <td colspan="2">
                        <table>
                            <tr id="ShowHd" class="hide">
                                <td colspan="2">
                                    <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <span id="SpanShowHeader" runat="server" style="font-weight: bold; color: Maroon"></span>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="Button2" EventName="Click" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </td>
                                <td  >
                                    <a href="javascript:void(0);" onclick="ShowDateSelect()" style="font-weight: bold;
                                        color: Blue">Show Another Date Range</a>
                                </td>
                            </tr>
                            <tr id="ShowDt">
                                <td style="width: 110px">
                                    <table>
                                        <tr>
                                            <td style="padding-right: 10px;padding-left: 10px;">
                                                <span>Form</span>
                                            </td>
                                            <td>
                                                 <dxe:ASPxDateEdit ID="dtFromG" runat="server" ClientInstanceName="dtFrom" EditFormat="Custom"
                                                        UseMaskBehavior="True" Font-Size="12px" Width="108px">
                                                    </dxe:ASPxDateEdit>
                                            </td>
                                        </tr>
                                    </table> 
                                </td>
                                <td style="width: 110px">
                                     <table>
                                         <tr>
                                             <td style="padding-right: 10px;padding-left: 10px;"><span>To</span></td>
                                             <td>
                                                    <dxe:ASPxDateEdit ID="dtToG" runat="server" ClientInstanceName="dtTo" EditFormat="Custom"
                                            UseMaskBehavior="True" Font-Size="12px" Width="98px">
                                        </dxe:ASPxDateEdit>

                                             </td>
                                         </tr>
                                     </table>
                                   
                                     
                                     
                                </td>
                                <td style="padding-right: 10px;padding-left: 10px;">
                                    <asp:Button ID="Button2" runat="server" Text="Show" CssClass="btnUpdate btn btn-primary" 
                                        Width="101px" OnClientClick="return selecttion();" OnClick="Button2_Click" />
                                </td>
                            </tr>
                            

                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="2"  style="display:none">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:GridView ID="grdCashBankBook" runat="server" Width="100%" BorderColor="CornflowerBlue"
                                    AllowSorting="true" ShowFooter="True" OnRowDataBound="grdCashBankSummary_RowDataBound"
                                    AutoGenerateColumns="false" AllowPaging="false" BorderStyle="Solid" BorderWidth="2px"
                                    CellPadding="4" ForeColor="#0000C0" PageSize="2500000" OnRowCreated="grdCashBankBook_RowCreated"
                                    OnSorting="grdCashBankBook_Sorting">
                                    <FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></FooterStyle>
                                    <Columns>
                                        <asp:TemplateField HeaderText="Tr. Date" SortExpression="TrDate1">
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
                                            <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Left"></ItemStyle>
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
                                        <asp:TemplateField HeaderText="AccountName" SortExpression="AccountName">
                                            <ItemStyle BorderWidth="1px" HorizontalAlign="Left"></ItemStyle>
                                            <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblAccName" runat="server" Text='<%# Eval("AccountName")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Instrument No." SortExpression="accountsledger_InstrumentNumber">
                                            <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                            <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblInstNo" runat="server" Text='<%# Eval("accountsledger_InstrumentNumber")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Instrument Date">
                                            <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                            <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblInstDate" runat="server" Text='<%# Eval("InstDate")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Receipts(DR.)">
                                            <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                            <HeaderStyle Wrap="False" HorizontalAlign="Right" Font-Bold="False"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblAmtDr" runat="server" Text='<%# Eval("Accountsledger_AmountDr")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Payments(CR.)">
                                            <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                            <HeaderStyle Wrap="False" HorizontalAlign="Right" Font-Bold="False"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblAmtCr" runat="server" Text='<%# Eval("Accountsledger_AmountCr")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Closing">
                                            <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                            <HeaderStyle Wrap="False" HorizontalAlign="Right" Font-Bold="False"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblClosing" runat="server" Text='<%# Eval("Closing")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="TrDate" Visible="false">
                                            <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                            <HeaderStyle Wrap="False" HorizontalAlign="Right" Font-Bold="False"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblTradeDate" runat="server" Text='<%# Eval("TrDate1")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="MainID" Visible="false">
                                            <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                            <HeaderStyle Wrap="False" HorizontalAlign="Right" Font-Bold="False"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblMainID" runat="server" Text='<%# Eval("MainID")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="SubID" Visible="false">
                                            <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                            <HeaderStyle Wrap="False" HorizontalAlign="Right" Font-Bold="False"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblSubID" runat="server" Text='<%# Eval("SubID")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="MainID" Visible="false">
                                            <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                            <HeaderStyle Wrap="False" HorizontalAlign="Right" Font-Bold="False"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblCompID" runat="server" Text='<%# Eval("CompanyID")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="SubID" Visible="false">
                                            <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                            <HeaderStyle Wrap="False" HorizontalAlign="Right" Font-Bold="False"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblSegID" runat="server" Text='<%# Eval("SegID")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <PagerTemplate>
                                        <table>
                                            <tr>
                                                <td colspan="10">
                                                    <asp:LinkButton ID="FirstPage" runat="server" Font-Bold="true" CommandName="First"
                                                        OnClientClick="selecttion()" OnCommand="NavigationLink_Click" Text="[First Page]"> </asp:LinkButton>
                                                    <asp:LinkButton ID="PreviousPage" runat="server" Font-Bold="true" CommandName="Prev"
                                                        OnClientClick="selecttion()" OnCommand="NavigationLink_Click" Text="[Previous Page]">  </asp:LinkButton>
                                                    <asp:LinkButton ID="NextPage" runat="server" Font-Bold="true" CommandName="Next"
                                                        OnClientClick="selecttion()" OnCommand="NavigationLink_Click" Text="[Next Page]">
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="LastPage" runat="server" Font-Bold="true" CommandName="Last"
                                                        OnClientClick="selecttion()" OnCommand="NavigationLink_Click" Text="[Last Page]">
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
                            </ContentTemplate>
                            <Triggers>
                              <%--  <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click"></asp:AsyncPostBackTrigger>
                                <asp:AsyncPostBackTrigger ControlID="Button2" EventName="Click"></asp:AsyncPostBackTrigger>
                                <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />--%>
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                </tr>

                <%--Debjyoti New devExpress Grid--%>
               
                <tr>
                    <td colspan="2">
                             <div class="GridViewArea ">
                    <dxe:ASPxGridView ID="aspxGrdCashBankBook" runat="server" AutoGenerateColumns="False" ClientInstanceName="caspxGrdCashBankBook"
                         Width="100%"  CssClass="pull-left" OnCustomCallback="aspxGrdCashBankBook_CustomCallback" >
                        <Columns>

                            <dxe:GridViewDataTextColumn Caption="Tr. Date" FieldName="TrDate" ReadOnly="True"
                                Visible="True" FixedStyle="Left" VisibleIndex="0">
                                <EditFormSettings Visible="True" /> 
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn Caption="ValueDate" FieldName="ValueDate" ReadOnly="True"
                                Visible="True" FixedStyle="Left" VisibleIndex="1">
                                <EditFormSettings Visible="True" /> 
                            </dxe:GridViewDataTextColumn>
                            
                            <dxe:GridViewDataTextColumn Caption="Voucher No." FieldName="accountsledger_TransactionReferenceID" ReadOnly="True"
                                Visible="True" FixedStyle="Left" VisibleIndex="2"> 
                                <EditFormSettings Visible="True" />
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn Caption="Description" FieldName="accountsledger_Narration" ReadOnly="True"
                                Visible="True" FixedStyle="Left" VisibleIndex="3"> 
                                <EditFormSettings Visible="True" />
                            </dxe:GridViewDataTextColumn> 
                            
                             <dxe:GridViewDataTextColumn Caption="AccountName" FieldName="AccountName" ReadOnly="True"
                                Visible="True" FixedStyle="Left" VisibleIndex="4"> 
                                <EditFormSettings Visible="True" />
                            </dxe:GridViewDataTextColumn> 
                            
                             <dxe:GridViewDataTextColumn Caption="Instrument No." FieldName="accountsledger_InstrumentNumber" ReadOnly="True"
                                Visible="True" FixedStyle="Left" VisibleIndex="5"> 
                                <EditFormSettings Visible="True" />
                            </dxe:GridViewDataTextColumn> 
                            
                             <dxe:GridViewDataTextColumn Caption="Instrument Date" FieldName="InstDate" ReadOnly="True"
                                Visible="True" FixedStyle="Left" VisibleIndex="6"> 
                                <EditFormSettings Visible="True" />
                            </dxe:GridViewDataTextColumn> 
                            
                             <dxe:GridViewDataTextColumn Caption="Receipts(DR.)" FieldName="Accountsledger_AmountDr" ReadOnly="True"
                                Visible="True" FixedStyle="Left" VisibleIndex="7"> 
                                <EditFormSettings Visible="True" />
                            </dxe:GridViewDataTextColumn> 
                            
                             <dxe:GridViewDataTextColumn Caption="Payments(CR.)" FieldName="Accountsledger_AmountCr" ReadOnly="True"
                                Visible="True" FixedStyle="Left" VisibleIndex="8"> 
                                <EditFormSettings Visible="True" />
                            </dxe:GridViewDataTextColumn> 

                            <dxe:GridViewDataTextColumn Caption="Closing" FieldName="Closing" ReadOnly="True"
                                Visible="True" FixedStyle="Left" VisibleIndex="9"> 
                                <EditFormSettings Visible="True" />
                                 <CellStyle Wrap="False" HorizontalAlign="Right" ></CellStyle> 
                            </dxe:GridViewDataTextColumn> 
                            
                             <dxe:GridViewDataTextColumn Caption="TrDate" FieldName="TrDate1" ReadOnly="True"
                                Visible="False" FixedStyle="Left" VisibleIndex="4">
                                <EditFormSettings Visible="True" />
                            </dxe:GridViewDataTextColumn> 
                            
                             <dxe:GridViewDataTextColumn Caption="MainID" FieldName="MainID" ReadOnly="True"
                                Visible="False" FixedStyle="Left" VisibleIndex="4">
                                  <Settings AutoFilterCondition="Contains" />
                                <EditFormSettings Visible="True" />
                            </dxe:GridViewDataTextColumn>  
                            
                            <dxe:GridViewDataTextColumn Caption="SubID" FieldName="SubID" ReadOnly="True"
                                Visible="False" FixedStyle="Left" VisibleIndex="4">
                                  <Settings AutoFilterCondition="Contains" />
                                <EditFormSettings Visible="True" />
                            </dxe:GridViewDataTextColumn> 

                             <dxe:GridViewDataTextColumn Caption="MainID" FieldName="CompanyID" ReadOnly="True"
                                Visible="False" FixedStyle="Left" VisibleIndex="4">
                                  <Settings AutoFilterCondition="Contains" />
                                <EditFormSettings Visible="True" />
                            </dxe:GridViewDataTextColumn> 
                    
                            <dxe:GridViewDataTextColumn Caption="SubID" FieldName="SegID" ReadOnly="True"
                                Visible="False" FixedStyle="Left" VisibleIndex="4">
                                  <Settings AutoFilterCondition="Contains" />
                                <EditFormSettings Visible="True" />
                            </dxe:GridViewDataTextColumn> 

                        </Columns> 
                        <SettingsBehavior ColumnResizeMode="NextColumn" />  
            </dxe:ASPxGridView>
        </div>

                    </td>
                </tr>

            </table>
            <table style="background-color: #DDECFE;" width="100%" height="300px">
                <tr>
                    <td>
                    <asp:HiddenField ID="HDNSeg" runat="server" />
                    <asp:HiddenField ID="hdn_NsdlCdslExchangeSegment" runat="server" />
                    </td>
                </tr>
            </table>
        </div>
</asp:Content>
