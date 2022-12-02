<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Reports.Reports_Cashbankbook" CodeBehind="Cashbankbook.aspx.cs" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript" src="/assests/js/ajaxList_rootfile.js"></script>
    <script type="text/javascript" src="/assests/js/GenericJScript.js"></script>
    <link type="text/css" href="../CSS/AjaxStyle.css" rel="Stylesheet" />

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

    <script language="javascript" type="text/javascript">
        function BtnShow_Click() {
            HideShow('Tblfooter', "S");
            HideShow('Tblheader', "H");
            grid.PerformCallback("Show~~~");
        }
        function BtnShowFilter_Click() {
            HideShow('Tblfooter', "H");
            HideShow('Tblheader', "S");
            //height();
            //Page_Load();
        }
        function PopulateGrid(obj) {
            grid.PerformCallback("SearchByNavigation1");
        }
        function ddlExport_OnChange() {
            var ddlExport = document.getElementById("<%=ddlExport.ClientID%>");
            //grid.PerformCallback("ExcelExport~~~");
            ddlExport.options[0].selected = true;
            document.getElementById('BtnForExportEvent').click();
        }
        function OnLeftNav_Click() {
            var i = document.getElementById("A1").innerText;
            document.getElementById("hdn_GridBindOrNotBind").value = "False"; //To Stop Bind On Page Load
            if (parseInt(i) > 1) {
                //            if(crbDOJ_Specific_All.GetValue()=="S")
                //                cGrdEmployee.PerformCallback("SearchByNavigation~"+cDtFrom.GetValue()+'~'+cDtTo.GetValue()+"~"+document.getElementById("A1").innerText+"~LeftNav");
                //            else
                HideShow('Tblfooter', "S");
                HideShow('Tblheader', "H");
                grid.PerformCallback("SearchByNavigation~~~" + document.getElementById("A1").innerText + "~LeftNav");
            }
            else {
                alert('No More Pages.');
            }
        }

        function OnPageNo_Click(obj) {
            var i = document.getElementById(obj).innerText;
            document.getElementById("hdn_GridBindOrNotBind").value = "False"; //To Stop Bind On Page Load
            HideShow('Tblfooter', "S");
            HideShow('Tblheader', "H");
            grid.PerformCallback("SearchByNavigation~~~" + i + "~PageNav");

        }
        function OnRightNav_Click() {
            var TestEnd = document.getElementById("A10").innerText;
            document.getElementById("hdn_GridBindOrNotBind").value = "False"; //To Stop Bind On Page Load
            var TotalPage = document.getElementById("B_TotalPage").innerText;
            if (TestEnd == "" || TestEnd == TotalPage) {
                alert('No More Records.');
                return;
            }
            var i = document.getElementById("A1").innerText;
            if (parseInt(i) < TotalPage) {
                //            if(crbDOJ_Specific_All.GetValue()=="S")
                //                cGrdEmployee.PerformCallback("SearchByNavigation~"+cDtFrom.GetValue()+'~'+cDtTo.GetValue()+"~"+document.getElementById("A1").innerText+"~RightNav");
                //            else
                HideShow('Tblfooter', "S");
                HideShow('Tblheader', "H");
                grid.PerformCallback("SearchByNavigation~~~" + document.getElementById("A1").innerText + "~RightNav");
            }
            else {
                alert('You are at the End');
            }
        }
        function gridasset_EndCallBack() {
            //        if(grid.cpExcelExport!=undefined)
            //        {
            //            document.getElementById('BtnForExportEvent').click();                                    
            //        }
            if (grid.cpRefreshNavPanel != undefined) {
                // alert(grid.cpRefreshNavPanel);
                document.getElementById("B_PageNo").innerText = '';
                document.getElementById("B_TotalPage").innerText = '';
                document.getElementById("B_TotalRows").innerText = '';

                var NavDirection = grid.cpRefreshNavPanel.split('~')[0];
                var PageNum = grid.cpRefreshNavPanel.split('~')[1];
                var TotalPage = grid.cpRefreshNavPanel.split('~')[2];
                var TotalRows = grid.cpRefreshNavPanel.split('~')[3];

                if (NavDirection == "RightNav") {
                    PageNum = parseInt(PageNum) + 10;
                    document.getElementById("B_PageNo").innerText = PageNum;
                    document.getElementById("B_TotalPage").innerText = TotalPage;
                    document.getElementById("B_TotalRows").innerText = TotalRows;
                    var n = parseInt(TotalPage) - parseInt(PageNum) > 10 ? parseInt(11) : parseInt(TotalPage) - parseInt(PageNum) + 2;
                    for (r = 1; r < n; r++) {
                        var obj = "A" + r;
                        document.getElementById(obj).innerText = PageNum++;
                    }
                    for (r = n; r < 11; r++) {
                        var obj = "A" + r;
                        document.getElementById(obj).innerText = "";
                    }
                }
                if (NavDirection == "LeftNav") {
                    if (parseInt(PageNum) > 1) {
                        PageNum = parseInt(PageNum) - 10;
                        document.getElementById("B_PageNo").innerText = PageNum;
                        document.getElementById("B_TotalPage").innerText = TotalPage;
                        document.getElementById("B_TotalRows").innerText = TotalRows;
                        for (l = 1; l < 11; l++) {
                            var obj = "A" + l;
                            document.getElementById(obj).innerText = PageNum++;
                        }
                    }
                    else {
                        alert('No More Pages.');
                    }
                }
                if (NavDirection == "PageNav") {
                    document.getElementById("B_PageNo").innerText = PageNum;
                    document.getElementById("B_TotalPage").innerText = TotalPage;
                    document.getElementById("B_TotalRows").innerText = TotalRows;
                }
                if (NavDirection == "ShowBtnClick") {
                    document.getElementById("B_PageNo").innerText = PageNum;
                    document.getElementById("B_TotalPage").innerText = TotalPage;
                    document.getElementById("B_TotalRows").innerText = TotalRows;
                    var n = parseInt(TotalPage) - parseInt(PageNum) > 10 ? parseInt(11) : parseInt(TotalPage) - parseInt(PageNum) + 2;

                    for (r = 1; r < n; r++) {

                        var obj = "A" + r;
                        document.getElementById(obj).innerText = PageNum++;
                    }

                    for (r = n; r < 11; r++) {
                        var obj = "A" + r;
                        document.getElementById(obj).innerText = "";
                    }

                }
            }
            if (grid.cpCallOtherWhichCallCondition != undefined) {
                if (grid.cpCallOtherWhichCallCondition == "Show") {
                    //                if(crbDOJ_Specific_All.GetValue()=="S")
                    //                    grid.PerformCallback("Show~"+cDtFrom.GetValue()+'~'+cDtTo.GetValue());
                    //                else
                    grid.PerformCallback("Show~~~");
                }
            }
            //Now Reset GridBindOrNotBind to True for Next Page Load
            document.getElementById("hdn_GridBindOrNotBind").value = "True";
            height();
        }
    </script>

    <script language="javascript" type="text/javascript">


        function Page_Load() {
            height();
            HideShow('showFilter', "H");
            HideShow('Tblfooter', "H");

        }
        function SignOff() {
            window.parent.SignOff();
        }
        FieldName = 'Button1';
        function showOptions(obj1, obj2, obj3) {
            var cmb = document.getElementById('cmbsearchOption');
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

        function clientselection() {
            var listBoxSubs = document.getElementById('SelectionList');
            var cmb = document.getElementById('<%=cmbsearch.ClientID%>');
            var listIDs = '';
            var i;
            if (listBoxSubs.length > 0) {
                for (i = 0; i < listBoxSubs.length; i++) {
                    if (listIDs == '')
                        listIDs = listBoxSubs.options[i].value + ';' + listBoxSubs.options[i].text;
                    else
                        listIDs += '!' + listBoxSubs.options[i].value + ';' + listBoxSubs.options[i].text;
                }
                var sendData = cmb.value + '^' + listIDs;

                CallServer(sendData, "");

            }
            var i;
            for (i = listBoxSubs.options.length - 1; i >= 0; i--) {
                listBoxSubs.remove(i);
            }
            HideShow('showFilter', "H");
        }

        function btnAddIDTolist_click() {
            var cmb = document.getElementById('cmbsearch');
            var userid = document.getElementById('txtSelection');
            if (userid.value != '') {
                var ids = document.getElementById('txtSelection_hidden');
                var listBox = document.getElementById('SelectionList');
                var tLength = listBox.length;


                var no = new Option();
                no.value = ids.value;
                no.text = userid.value;
                listBox[tLength] = no;
                var recipient = document.getElementById('txtSelection');
                recipient.value = '';
                var s = document.getElementById('txtSelection');
                s.focus();
                s.select();
            }
            else {
                alert('Please search name and then Add!')
                var s = document.getElementById('txtSelection');
                s.focus();
                s.select();
            }
        }

        function btnRemoveFromlist_click() {

            var listBox = document.getElementById('SelectionList');
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

        function updateCashBankDetail(objDate, objVouNo, objMainID, objSubID, objCompID, objSegID) {
            var URL = 'CashBankEntryEdit.aspx?date=' + objDate + ' &vNo=' + objVouNo + ' &Compid=' + objCompID + ' &SegID=' + objSegID;
            editwin = dhtmlmodal.open("Editbox", "iframe", URL, "Update Cash/Bank Book", "width=940px,height=450px,center=1,resize=1,top=500", "recal");
            editwin.onclose = function () {
                grid.PerformCallback("Callfromanotherpages");
            }


        }
        function Showbranch(obj) {
            if (obj == "A")
                HideShow('showFilter', "H");
            else {
                HideShow('showFilter', "S");
                SetValue('cmbsearch', "Branch");
                CallServer("Ajax-Branch", "");
            }
        }

        function Showsegment(obj) {
            if (obj == "A")
                HideShow('showFilter', "H");
            else {
                HideShow('showFilter', "S");
                SetValue('cmbsearch', "Segment");
                CallServer("Ajax-Segment", "");
            }
        }

        function DateChangeForFrom(DateobjFrm, DateobjTo) {
            var Msg = "To Date Can Not Be Less Than From Date!!!";
            DevE_CompareDateForMin(DateobjFrm, DateobjTo, Msg);
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

        function DevE_CompareDateForMin(DateObjectFrm, DateObjectTo, Msg) {
            var SelectedDate = new Date(DateObjectFrm.GetDate());
            var monthnumber = SelectedDate.getMonth();
            var monthday = SelectedDate.getDate();
            var year = SelectedDate.getYear();
            var SelectedDateValueFrm = new Date(year, monthnumber, monthday);

            var SelectedDate = new Date(DateObjectTo.GetDate());
            monthnumber = SelectedDate.getMonth();
            monthday = SelectedDate.getDate();
            year = SelectedDate.getYear();
            var SelectedDateValueTo = new Date(year, monthnumber, monthday);
            var SelectedDateNumericValueFrm = SelectedDateValueFrm.getTime();
            var SelectedDateNumericValueTo = SelectedDateValueTo.getTime();
            if (SelectedDateNumericValueFrm > SelectedDateNumericValueTo) {
                alert(Msg);
                DateObjectTo.SetDate(new Date(DateObjectFrm.GetDate()));
            }
        }

        function DateChangeForTo(DateobjFrm, DateobjTo) {
            var Msg = "To Date Can Not Be Less Than From Date!!!";
            DevE_CompareDateForMin(DateobjFrm, DateobjTo, Msg);
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

        function ReceiveSvrData(rValue) {
            var Data = rValue.split('@');
            var btnHideGroupType = document.getElementById('btnGroupTypehide');

            if (Data[1] != "undefined") {
                if (Data[0] == 'Segment') {
                    document.getElementById('HiddenField_Segment').value = Data[1];
                }
                else {
                    document.getElementById('HiddenField_Branch').value = Data[1];

                }
            }
            if (Data[0] == 'AjaxQuery') {
                AjaxComQuery = Data[1];
                var AjaxList_TextBox = document.getElementById('<%=txtSelection.ClientID%>');
                AjaxList_TextBox.value = '';
                AjaxList_TextBox.detachEvent('onkeyup', CallGenericAjaxJS);
                AjaxList_TextBox.attachEvent('onkeyup', CallGenericAjaxJS);

            }
        }
        function CallGenericAjaxJS(e) {
            var AjaxList_TextBox = document.getElementById('<%=txtSelection.ClientID%>');
            AjaxList_TextBox.focus();
            AjaxComQuery = AjaxComQuery.replace("\'", "'");
            ajax_showOptions(AjaxList_TextBox, 'GenericAjaxList', e, replaceChars(AjaxComQuery), 'Main');

        }
    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <div>
            <div class="TableMain100">
                <div class="EHEADER" style="text-align: center;">
                    <strong><span style="color: #000099">CashBank Book</span></strong>
                </div>
            </div>
            <table id="Tblheader">
                <tr>
                    <td>
                        <div class="pageContent">
                            <div id="divPageheader">
                                <div class="right" style="width: 472px; margin-right: 10px;">
                                    <div id="showFilter" class="left frmContent" style="display: none;">
                                        <div style="width: 100%">
                                            <div class="frmleftContent">
                                                <asp:TextBox ID="txtSelection" runat="server" Font-Size="12px" Height="20px" Width="250px"
                                                    TabIndex="0"></asp:TextBox>
                                            </div>
                                            <div class="frmleftContent" style="padding-top: 3px">
                                                <asp:DropDownList ID="cmbsearch" runat="server" Font-Size="13px" Width="80px" Enabled="false">
                                                    <asp:ListItem>Segment</asp:ListItem>
                                                    <asp:ListItem>Branch</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div class="frmleftContent">
                                                <a id="A11" href="javascript:void(0);" tabindex="0" onclick="btnAddIDTolist_click()">
                                                    <span style="color: #009900; text-decoration: underline; font-size: 10pt; line-height: 2;">Add to List</span></a>
                                            </div>
                                        </div>
                                        <span class="clear" style="background-color: #B7CEEC;"></span>
                                        <div class="frmleftContent" style="height: 105px; margin-top: 5px">
                                            <asp:ListBox ID="SelectionList" runat="server" Font-Size="12px" Height="100px" Width="450px"
                                                TabIndex="0"></asp:ListBox>
                                        </div>
                                        <span class="clear" style="background-color: #B7CEEC;"></span>
                                        <div class="frmleftContent" style="text-align: center">
                                            <a id="AA2" href="javascript:void(0);" tabindex="0" onclick="clientselection()"><span
                                                style="color: #000099; text-decoration: underline; font-size: 10pt; line-height: 2;">Done</span></a>&nbsp;&nbsp; <a id="AA1" href="javascript:void(0);" tabindex="0" onclick="btnRemoveFromlist_click()">
                                                    <span style="color: #cc3300; text-decoration: underline; font-size: 10pt; line-height: 2;">Remove</span></a>
                                        </div>
                                    </div>
                                </div>
                                <div id="dvMainFilter" class="frmContent" style="width: 500px;">
                                    <div id="Trbankname">
                                        <div class="frmleftCont" style="width: 110px; line-height: 20px">
                                            <asp:Label ID="lblaccount" runat="server" Text="Bank Name : "></asp:Label>
                                        </div>
                                        <div class="frmleftContent">
                                            <asp:TextBox ID="txtbank" runat="server" Width="300px" Font-Size="12px" Height="20px"
                                                TabIndex="0"></asp:TextBox>
                                            <asp:TextBox ID="txtbank_hidden" runat="server" Width="2px" Font-Size="1px" Height="2px"
                                                Style="display: none" TabIndex="0"></asp:TextBox>
                                        </div>
                                    </div>
                                    <span class="clear"></span>
                                    <div id="divsegment">
                                        <div class="frmleftCont" style="width: 110px; line-height: 20px">
                                            <asp:Label ID="lblsegment" runat="server" Text="Segment:"></asp:Label>
                                        </div>
                                        <div class="frmleftContent">
                                            <dxe:ASPxRadioButtonList ID="rdbSgment" runat="server" SelectedIndex="0" ItemSpacing="10px"
                                                RepeatDirection="Horizontal" TextWrap="False" Font-Size="12px" ClientInstanceName="crbsegment"
                                                TabIndex="0">
                                                <Items>
                                                    <dxe:ListEditItem Text="All" Value="A" />
                                                    <dxe:ListEditItem Text="Specific" Value="S" />
                                                </Items>
                                                <ClientSideEvents ValueChanged="function(s, e) {Showsegment(s.GetValue());}" />
                                                <Border BorderWidth="0px" />
                                            </dxe:ASPxRadioButtonList>
                                        </div>
                                    </div>
                                    <span class="clear"></span>
                                    <div id="TrBranch">
                                        <div class="frmleftCont" style="width: 110px; line-height: 20px">
                                            <asp:Label ID="lblsett" runat="server" Text="Branch : "></asp:Label>
                                        </div>
                                        <div class="frmleftContent">
                                            <dxe:ASPxRadioButtonList ID="rdbBranch" runat="server" SelectedIndex="0" ItemSpacing="10px"
                                                RepeatDirection="Horizontal" TextWrap="False" Font-Size="12px" ClientInstanceName="crbSettlement"
                                                TabIndex="0">
                                                <Items>
                                                    <dxe:ListEditItem Text="All" Value="A" />
                                                    <dxe:ListEditItem Text="Specific" Value="S" />
                                                </Items>
                                                <ClientSideEvents ValueChanged="function(s, e) {Showbranch(s.GetValue());}" />
                                                <Border BorderWidth="0px" />
                                            </dxe:ASPxRadioButtonList>
                                        </div>
                                    </div>
                                    <span class="clear"></span>
                                    <div id="TrSettlementType">
                                        <div class="frmleftCont" style="width: 110px; line-height: 20px">
                                            <asp:Label ID="lblsettype" runat="server" Text="Date :"></asp:Label>
                                        </div>
                                        <div class="frmleftContent">
                                            <dxe:ASPxDateEdit ID="dtDate" runat="server" EditFormat="Custom" ClientInstanceName="dtDate"
                                                EditFormatString="dd-MM-yyyy" UseMaskBehavior="True">
                                                <DropDownButton Text="From ">
                                                </DropDownButton>
                                                <ClientSideEvents DateChanged="function(s,e){DateChangeForFrom(dtDate,dtToDate);}" />
                                            </dxe:ASPxDateEdit>
                                        </div>
                                        <div class="frmleftContent">
                                            <dxe:ASPxDateEdit ID="dtToDate" runat="server" EditFormat="Custom" ClientInstanceName="dtToDate"
                                                EditFormatString="dd-MM-yyyy" UseMaskBehavior="True">
                                                <DropDownButton Text="To">
                                                </DropDownButton>
                                                <ClientSideEvents DateChanged="function(s,e){DateChangeForTo(dtDate,dtToDate);}" />
                                            </dxe:ASPxDateEdit>
                                        </div>
                                    </div>
                                    <span class="clear" style="height: 5px;">&nbsp;</span>
                                    <div id="divbutton">
                                        <div style="width: 120px; float: left;">
                                        </div>
                                        <div style="width: 110px; line-height: 20px;">
                                            <dxe:ASPxButton ID="BtnShow" runat="server" AutoPostBack="False" Text="Show" Width="85px">
                                                <ClientSideEvents Click="function (s, e) {BtnShow_Click();}" />
                                            </dxe:ASPxButton>
                                        </div>
                                    </div>
                                    <span class="clear" style="height: 5px;">&nbsp;</span>
                                </div>
                            </div>
                        </div>
                    </td>
                </tr>
            </table>
            <table id="Tblfooter">
                <tr>
                    <td>
                        <table border="1" style="width: 60%">

                            <tr>
                                <td style="vertical-align: top; width: 34px; background-color: #b7ceec; text-align: left"
                                    valign="top">Page</td>
                                <td style="width: 4px" valign="top">
                                    <b id="B_PageNo" runat="server" style="text-align: right"></b>
                                </td>
                                <td style="vertical-align: top; background-color: #b7ceec; text-align: left" valign="top">Of
                                </td>
                                <td valign="top">
                                    <b id="B_TotalPage" runat="server" style="text-align: right"></b>
                                </td>
                                <td style="vertical-align: top; background-color: #b7ceec; text-align: left" valign="top">( <b id="B_TotalRows" runat="server" style="text-align: right"></b>&nbsp;items )
                                </td>
                                <td valign="top">
                                    <table width="100%">
                                        <tr>
                                            <td style="vertical-align: top; background-color: #b7ceec; text-align: left" valign="top">
                                                <a id="A_LeftNav" runat="server" href="javascript:void(0);" onclick="OnLeftNav_Click()">
                                                    <img src="/assests/images/LeftNav.gif" alt="" width="10" />
                                                </a>
                                            </td>
                                            <td style="vertical-align: top; background-color: #b7ceec; text-align: left" valign="top">
                                                <a id="A1" runat="server" href="javascript:void(0);" onclick="OnPageNo_Click('A1')">1</a>
                                            </td>
                                            <td style="vertical-align: top; background-color: #b7ceec; text-align: left" valign="top">
                                                <a id="A2" runat="server" href="javascript:void(0);" onclick="OnPageNo_Click('A2')">2</a>
                                            </td>
                                            <td style="vertical-align: top; background-color: #b7ceec; text-align: left" valign="top">
                                                <a id="A3" runat="server" href="javascript:void(0);" onclick="OnPageNo_Click('A3')">3</a>
                                            </td>
                                            <td style="vertical-align: top; background-color: #b7ceec; text-align: left" valign="top">
                                                <a id="A4" runat="server" href="javascript:void(0);" onclick="OnPageNo_Click('A4')">4</a>
                                            </td>
                                            <td style="vertical-align: top; background-color: #b7ceec; text-align: left" valign="top">
                                                <a id="A5" runat="server" href="javascript:void(0);" onclick="OnPageNo_Click('A5')">5</a>
                                            </td>
                                            <td style="vertical-align: top; background-color: #b7ceec; text-align: left" valign="top">
                                                <a id="A6" runat="server" href="javascript:void(0);" onclick="OnPageNo_Click('A6')">6</a>
                                            </td>
                                            <td style="vertical-align: top; background-color: #b7ceec; text-align: left" valign="top">
                                                <a id="A7" runat="server" href="javascript:void(0);" onclick="OnPageNo_Click('A7')">7</a>
                                            </td>
                                            <td style="vertical-align: top; background-color: #b7ceec; text-align: left" valign="top">
                                                <a id="A8" runat="server" href="javascript:void(0);" onclick="OnPageNo_Click('A8')">8</a>
                                            </td>
                                            <td style="vertical-align: top; background-color: #b7ceec; text-align: left" valign="top">
                                                <a id="A9" runat="server" href="javascript:void(0);" onclick="OnPageNo_Click('A9')">9</a>
                                            </td>
                                            <td style="vertical-align: top; background-color: #b7ceec; text-align: left" valign="top">
                                                <a id="A10" runat="server" href="javascript:void(0);" onclick="OnPageNo_Click('A10')">10</a>
                                            </td>
                                            <td style="vertical-align: top; background-color: #b7ceec; text-align: right" valign="top">
                                                <a id="A_RightNav" runat="server" href="javascript:void(0);" onclick="OnRightNav_Click()">
                                                    <img src="../images/RightNav.gif" width="10" alt="" />
                                                </a>
                                            </td>
                                            <td style="vertical-align: top; background-color: #b7ceec; text-align: right" valign="top">
                                                <asp:DropDownList ID="ddlExport" runat="server" Font-Size="12px" Onchange="ddlExport_OnChange()"
                                                    Width="100px">
                                                    <asp:ListItem Selected="True" Value="Ex">Export</asp:ListItem>
                                                    <asp:ListItem Value="1">Excel</asp:ListItem>
                                                </asp:DropDownList>&nbsp;
                                            </td>
                                            <td id="TdShowFilter" style="vertical-align: top; background-color: #b7ceec; text-align: right"
                                                valign="top">
                                                <dxe:ASPxButton ID="btnShowFilter" runat="server" AutoPostBack="False" ClientInstanceName="cbtnShowFilter"
                                                    Text="Show Filter" Width="85px">
                                                    <ClientSideEvents Click="function(s,e){BtnShowFilter_Click();}" />
                                                </dxe:ASPxButton>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <dxe:ASPxGridView ID="gridasset" runat="server" AutoGenerateColumns="False" KeyFieldName="IDfinal"
                            Width="95%" ClientInstanceName="grid" OnHtmlRowCreated="gridasset_HtmlRowCreated"
                            OnCustomCallback="gridasset_CustomCallback">
                            <SettingsBehavior ConfirmDelete="True" AllowFocusedRow="True" />
                            <Styles>
                                <LoadingPanel ImageSpacing="10px">
                                </LoadingPanel>
                                <Header ImageSpacing="5px" SortingImageSpacing="5px" CssClass="gridheader">
                                </Header>
                                <FocusedGroupRow CssClass="gridselectrow">
                                </FocusedGroupRow>
                                <FocusedRow CssClass="gridselectrow">
                                </FocusedRow>
                            </Styles>
                            <SettingsPager NumericButtonCount="20" PageSize="20" ShowSeparators="True" Mode="ShowAllRecords">
                                <FirstPageButton Visible="True">
                                </FirstPageButton>
                                <LastPageButton Visible="True">
                                </LastPageButton>
                            </SettingsPager>
                            <SettingsEditing Mode="PopupEditForm" PopupEditFormHeight="200px" PopupEditFormHorizontalAlign="Center"
                                PopupEditFormModal="True" PopupEditFormVerticalAlign="WindowCenter" PopupEditFormWidth="600px"
                                EditFormColumnCount="1" />
                            <SettingsText PopupEditFormCaption="Add/Modify " ConfirmDelete="Are you sure to Delete this Record!" />
                            <ClientSideEvents EndCallback="function(s, e) {gridasset_EndCallBack();}" />
                            <Columns>
                                <dxe:GridViewDataTextColumn FieldName="TrDate" Caption="Tr.Date" VisibleIndex="0" Width="8%">
                                    <CellStyle CssClass="gridcellleft" Wrap="False">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Left" />
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="ValueDate" Caption="Value Date" VisibleIndex="1" Width="6%">
                                    <CellStyle CssClass="gridcellleft" Wrap="False">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Left" />
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="TransactionReferenceID" Caption="Voucher No"
                                    VisibleIndex="2" Width="6%">
                                    <CellStyle CssClass="gridcellleft" Wrap="False">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Left" />
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="Narration" Caption="Description" VisibleIndex="3" Width="17%">
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <FooterTemplate>
                                        Total Closing :
                                    </FooterTemplate>
                                    <FooterCellStyle Font-Bold="true" BackColor="#b7ceec" Font-Size="12px">
                                    </FooterCellStyle>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="AccountName" FieldName="AccountName" VisibleIndex="4" Width="16%">
                                    <CellStyle CssClass="gridcellleft" Wrap="default">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Left" />
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="Inst.Number" FieldName="InstrumentNumber"
                                    VisibleIndex="5" Width="6%">
                                    <CellStyle CssClass="gridcellleft" Wrap="False">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Left" />
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="Inst.Date" FieldName="Instrumentdate" VisibleIndex="6" Width="6%">
                                    <CellStyle CssClass="gridcellleft" Wrap="False">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Left" />
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="AmmountDr" Caption="Dr(Receipt)" VisibleIndex="7"
                                    Width="10%">
                                    <CellStyle CssClass="gridcellleft" Wrap="False" HorizontalAlign="Right">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="right" />
                                    <FooterTemplate>
                                        <span>
                                            <%# GetSummaryValueDr(Container)%>
                                        </span>
                                    </FooterTemplate>
                                    <FooterCellStyle HorizontalAlign="Right" BackColor="#b7ceec" Font-Bold="true">
                                    </FooterCellStyle>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="AmmountCr" Caption="Cr(Payment)" VisibleIndex="8"
                                    Width="10%">
                                    <CellStyle CssClass="gridcellleft" Wrap="False" HorizontalAlign="Right">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="right" />
                                    <FooterTemplate>
                                        <span>
                                            <%# GetSummaryValueCr(Container)%>
                                        </span>
                                    </FooterTemplate>
                                    <FooterCellStyle HorizontalAlign="Right" BackColor="#b7ceec" Font-Bold="true">
                                    </FooterCellStyle>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="FinalClosing" Caption="Closing" VisibleIndex="9"
                                    Width="10%">
                                    <CellStyle CssClass="gridcellleft" Wrap="False" HorizontalAlign="Right">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="right" />
                                    <FooterTemplate>
                                        <span>
                                            <%# GetSummaryValueClosing(Container)%>
                                        </span>
                                    </FooterTemplate>
                                    <FooterCellStyle HorizontalAlign="Right" BackColor="#b7ceec" Font-Bold="true">
                                    </FooterCellStyle>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="CompanyID" Visible="False" Caption="Closing"
                                    VisibleIndex="11">
                                    <CellStyle CssClass="gridcellleft" Wrap="False" HorizontalAlign="Right">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Left" />
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="MainID" Visible="False" Caption="Closing"
                                    VisibleIndex="12">
                                    <CellStyle CssClass="gridcellleft" Wrap="False" HorizontalAlign="Right">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Left" />
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="SubID" Visible="False" Caption="Closing"
                                    VisibleIndex="13">
                                    <CellStyle CssClass="gridcellleft" Wrap="False" HorizontalAlign="Right">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Left" />
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="TransactionReferenceID" Visible="False"
                                    Caption="Closing" VisibleIndex="14">
                                    <CellStyle CssClass="gridcellleft" Wrap="False" HorizontalAlign="Right">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Left" />
                                </dxe:GridViewDataTextColumn>
                            </Columns>
                            <Settings ShowFooter="true" />
                            <%--<TotalSummary>
                                <dxe:ASPxSummaryItem FieldName="FinalClosing" ShowInColumn="FinalClosing" DisplayFormat="00,00,00.00"
                                    SummaryType="None" />
                            </TotalSummary>--%>
                            <Settings ShowHorizontalScrollBar="false" ShowGroupButtons="False" ShowGroupPanel="false"
                                ShowStatusBar="Hidden" />
                        </dxe:ASPxGridView>
                    </td>
                </tr>
            </table>
        </div>
        <div style="display: none">
            <%-- <asp:TextBox ID="txtISIN_hidden" runat="server" Width="14px"></asp:TextBox>
                            <asp:HiddenField ID="HiddenField_Client" runat="server" />
                            <asp:HiddenField ID="HiddenField_Productscriptisin" runat="server" />
                            <asp:HiddenField ID="HiddenField_accountst" runat="server" />--%>
            <asp:HiddenField ID="txtSelection_hidden" runat="server" />
            <asp:HiddenField ID="HiddenField_Segment" runat="server" />
            <asp:HiddenField ID="HiddenField_Branch" runat="server" />
            <asp:HiddenField ID="HiddenFieldpagenav" runat="server" />
            <asp:HiddenField ID="hdn_GridBindOrNotBind" runat="server" />
            <asp:Button ID="BtnForExportEvent" runat="server" OnClick="cmbExport_SelectedIndexChanged"
                BackColor="#DDECFE" BorderStyle="None" />
        </div>
    </div>
</asp:Content>
