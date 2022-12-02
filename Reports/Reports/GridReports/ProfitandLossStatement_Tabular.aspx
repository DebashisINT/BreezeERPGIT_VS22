<%@ Page Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="ProfitandLossStatement_Tabular.aspx.cs" Inherits="Reports.Reports.GridReports.ProfitandLossStatement_Tabular" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
     Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">


    <script src="/assests/pluggins/choosen/choosen.min.js"></script>
    <style>
        #pageControl, .dxtc-content {
            overflow: visible !important;
        }

        #MandatoryAssign {
            position: absolute;
            right: -17px;
            top: 6px;
        }

        #MandatorySupervisorAssign {
            position: absolute;
            right: 1px;
            top: 27px;
        }

        .chosen-container.chosen-container-multi,
        .chosen-container.chosen-container-single {
            width: 100% !important;
        }

        .chosen-choices {
            width: 100% !important;
        }

        .disp {
            display: none;
        }

        #ListBoxBranches {
            width: 200px;
        }

        .hide {
            display: none;
        }

        .dxtc-activeTab .dxtc-link {
            color: #fff !important;
        }
    </style>


    <script type="text/javascript">
        $(function () {
            cbranchPanel.PerformCallback('BindComponentGrid' + '~' + $("#ddlbranchHO").val());
            $("#ddlbranchHO").change(function () {
                var Ids = $(this).val();
                cbranchPanel.PerformCallback('BindComponentGrid' + '~' + $("#ddlbranchHO").val());
            })
        });

        document.onkeyup = function (e) {
            if (event.keyCode == 27 && cpopup2ndLevel.GetVisible() == true && popupdocument.GetVisible() == false) {
                popupHide();
            }
            else if (event.keyCode == 27 && popupdocument.GetVisible() == true) {
                popupHide2();
            }
        }
        function popupHide(s, e) {
            cpopup2ndLevel.Hide();
            Grid.Focus();
            $("#drdExport").val(0);
            $("#drdExport2").val(0);
        }
        function popupHide2(s, e) {
            popupdocument.Hide();
            cShowGridDetails2Level.Focus();
            $("#drdExport").val(0);
            $("#drdExport2").val(0);
        }
        function Openpopup2ndLevel(ledgerid, typex) {
            $("#drdExport").val(0);
            $("#drdExport2").val(0);
            var asondatewise = 'N';
            cCallbackPanel.PerformCallback(asondatewise + '~' + ledgerid + '~' + typex);
            cpopup2ndLevel.Show();
            return true;
        }
        function ProffitnLossPanelEndCallBack() {
             <%--Rev Subhra 20-12-2018   0017670--%>
            document.getElementById('<%=CompBranch.ClientID %>').innerHTML = cProffitnLossPanel.cpBranchNames
            //End of Subhra
            Grid.Refresh();
            GridIncome.Refresh();
            if (Grid.cpErrorFinancial == 'ErrorFinancial') {
                jAlert('Date Range should be within Financial Year');
            }
            else {
                $("#drdExport").val(0);
                $("#drdExport2").val(0);
                Grid.Focus();
                Grid.SetFocusedRowIndex(0);
                GridIncome.Focus();
                GridIncome.SetFocusedRowIndex(0);
            }
            Grid.cpErrorFinancial = null;
            GridIncome.cpErrorFinancial = null;
        }
        //function CallbackPanelDetEndCall() {
        //    cShowGridDetails2Level.Refresh();
        //}
        //function EndShowGridDetails2Level() {
        //    cShowGridDetails2Level.Focus();
        //    cShowGridDetails2Level.SetFocusedRowIndex(0);
        //    ctxtLedger2ndLevel.SetText(cShowGridDetails2Level.cpLedger);

        //    $("#lblFromDate2ndLevel")[0].innerHTML = "From " + cShowGridDetails2Level.cpFromDate;
        //    $("#lblToDate2ndLevel")[0].innerHTML = " To " + cShowGridDetails2Level.cpToDate;

        //    if (cShowGridDetails2Level.cpLedgerType != "FOR BRANCH") {
        //        $("#Label3")[0].innerHTML = "Ledger :";
        //    }
        //    else {
        //        $("#Label3")[0].innerHTML = "Branch :";
        //    }
        //}

        //function EndShowGridDetails2Level() {
        //    cShowGridDetails2Level.Focus();
        //    cShowGridDetails2Level.SetFocusedRowIndex(0);
        //    ctxtLedger2ndLevel.SetText(cShowGridDetails2Level.cpLedger);
        //    $("#lblFromDate2ndLevel")[0].innerHTML = "From " + cShowGridDetails2Level.cpFromDate;
        //    $("#lblToDate2ndLevel")[0].innerHTML = " To " + cShowGridDetails2Level.cpToDate;
        //    if (cShowGridDetails2Level.cpLedgerType != "FOR BRANCH") {
        //        $("#Label3")[0].innerHTML = "Ledger :";
        //    }
        //    else {
        //        $("#Label3")[0].innerHTML = "Branch :";
        //    }
        //}

        function CallbackPanelDetEndCall() {
            cShowGridDetails2Level.Focus();
            cShowGridDetails2Level.SetFocusedRowIndex(0);
            ctxtLedger2ndLevel.SetText(cCallbackPanel.cpLedger);

            $("#lblFromDate2ndLevel")[0].innerHTML = "From " + cCallbackPanel.cpFromDate;
            $("#lblToDate2ndLevel")[0].innerHTML = " To " + cCallbackPanel.cpToDate;

            if (cCallbackPanel.cpLedgerType != "FOR BRANCH") {
                $("#Label3")[0].innerHTML = "Ledger :";
            }
            else {
                $("#Label3")[0].innerHTML = "Branch :";
            }
            if (cCallbackPanel.cpBlankLedger == "0") {
                ctxtLedger2ndLevel.SetText('');
            }

            cShowGridDetails2Level.Refresh();
        }

        //function cShowGridDetails2Level() {
        //    cShowGridDetails2Level.Focus();
        //    cShowGridDetails2Level.SetFocusedRowIndex(0);
        //    ctxtLedger2ndLevel.SetText(cShowGridDetails2Level.cpLedger);

        //    $("#lblFromDate2ndLevel")[0].innerHTML = "From " + cShowGridDetails2Level.cpFromDate;
        //    $("#lblToDate2ndLevel")[0].innerHTML = " To " + cShowGridDetails2Level.cpToDate;

        //    if (cShowGridDetails2Level.cpLedgerType != "FOR BRANCH") {
        //        $("#Label3")[0].innerHTML = "Ledger :";
        //    }
        //    else {
        //        $("#Label3")[0].innerHTML = "Branch :";
        //    }
        //}

        function btn_ShowRecordsClick(e) {
            var data = "OnDateChanged";
            var branchid = $('#ddlbranchHO').val();
            var layoutid = $('#ddlLayout').val();
            var AsonWise = false;
            $("#hfIsProfnLossFilter").val("Y");
            //Grid.PerformCallback(data + '~' + AsonWise + '~' + branchid + '~' + layoutid);
            if (gridbranchLookup.GetValue() == null) {
                jAlert('Please select atleast one Branch for generate the report.');
            }
            else {
                //Grid.PerformCallback(data + '~' + AsonWise + '~' + branchid + '~' + layoutid);
                cProffitnLossPanel.PerformCallback(data + '~' + AsonWise + '~' + branchid + '~' + layoutid);
            }

            var FromDate = (cxdeFromDate.GetValue() != null) ? cxdeFromDate.GetValue() : "";
            var ToDate = (cxdeToDate.GetValue() != null) ? cxdeToDate.GetValue() : "";

            FromDate = GetDateFormat(FromDate);
            ToDate = GetDateFormat(ToDate);
            if (AsonWise == false) {
                document.getElementById('<%=DateRange.ClientID %>').innerHTML = "For the period: " + FromDate + " To " + ToDate;
            }
            else {
                document.getElementById('<%=DateRange.ClientID %>').innerHTML = "As on: " + FromDate;
            }
        }

        function GetDateFormat(today) {
            if (today != "") {
                var dd = today.getDate();
                var mm = today.getMonth() + 1; //January is 0!

                var yyyy = today.getFullYear();
                if (dd < 10) {
                    dd = '0' + dd;
                }
                if (mm < 10) {
                    mm = '0' + mm;
                }
                today = dd + '-' + mm + '-' + yyyy;
            }

            return today;
        }

        $(document).ready(function () {
            $("#ListBoxBranches").chosen().change(function () {
                var Ids = $(this).val();
                $('#<%=hdnSelectedBranches.ClientID %>').val(Ids);
                $('#MandatoryActivityType').attr('style', 'display:none');
            })
        })

        function CheckConsPL(s, e) {
            if (s.GetCheckState() == 'Checked') {
                CchkOWMSTVT.SetEnabled(true);
                CchkConsLandCost.SetEnabled(true);
                CchkConsOverHeadCost.SetEnabled(true);
                $(<%=ddlValTech.ClientID%>).prop("disabled", false)
            }
            else {
                CchkOWMSTVT.SetCheckState('UnChecked');
                CchkOWMSTVT.SetEnabled(false);
                CchkConsLandCost.SetCheckState('UnChecked');
                CchkConsLandCost.SetEnabled(false);
                CchkConsOverHeadCost.SetCheckState('UnChecked');
                CchkConsOverHeadCost.SetEnabled(false);
                $(<%=ddlValTech.ClientID%>).prop("disabled", true)
                $("#ddlValTech").val("A");
            }
        }
    </script>

    <script type="text/javascript">

        function cxdeToDate_OnChaged(s, e) {

            var data = "OnDateChanged";
            data += '~' + cxdeFromDate.GetDate();
            data += '~' + cxdeToDate.GetDate();
        }


        function OnContextMenuItemClick(sender, args) {
            if (args.item.name == "ExportToPDF" || args.item.name == "ExportToXLS") {
                args.processOnServer = true;
                args.usePostBack = true;
            } else if (args.item.name == "SumSelected")
                args.processOnServer = true;
        }

        function abc() {
            $("#drdExport").val(0);

        }

        function BudgetAfterHide(s, e) {
            popupbudget.Hide();
        }

        function CloseGridBranchLookupbranch() {
            gridbranchLookup.ConfirmCurrentSelection();
            gridbranchLookup.HideDropDown();
            gridbranchLookup.Focus();
        }

        function selectAll_Branch() {
            gridbranchLookup.gridView.SelectRows();
        }

        function unselectAll_Branch() {
            gridbranchLookup.gridView.UnselectRows();
        }

    </script>
    <%--for 2nd level popup--%>
    <script type="text/javascript">
        function OnWaitingGridKeyPress(e) {
            //if (e.code == "Enter" || e.code == "NumpadEnter") {
            if (e.key == "Enter" || e.key == "NumpadEnter") {
                $("#drdExport").val(0);
                var asondatewise = 'N';
                $("#hfIsProfnlossDetails").val("Y");
                $("#hfIsProfnLossFilter").val("Y");
                var index = Grid.GetFocusedRowIndex();
                var ledger = Grid.GetRow(index).children[3].innerHTML;
                var ledgertype = Grid.GetRow(index).children[2].innerHTML;
                var branchid = $('#ddlbranchHO').val();

                //cCallbackPanel.PerformCallback(asondatewise + '~' + ledger + '~' + ledgertype + '~' + branchid);
                //cpopup2ndLevel.Show();
                if (ledger.trim() == "0") {
                    jAlert('Zooming from Total is not possible.');
                    cpopup2ndLevel.Hide();
                }
                else {
                    cCallbackPanel.PerformCallback(asondatewise + '~' + ledger + '~' + ledgertype + '~' + branchid);
                    cpopup2ndLevel.Show();
                }
                return true;
            }
        }

        function OnWaitingGridKeyPress2(e) {
            //if (e.code == "Enter" || e.code == "NumpadEnter") {
            if (e.key == "Enter" || e.key == "NumpadEnter") {
                $("#drdExport").val(0);
                var asondatewise = 'N';
                $("#hfIsProfnlossDetails").val("Y");
                $("#hfIsProfnLossFilter").val("Y");
                var index = GridIncome.GetFocusedRowIndex();
                var ledger = GridIncome.GetRow(index).children[3].innerHTML;
                var ledgertype = GridIncome.GetRow(index).children[2].innerHTML;
                var branchid = $('#ddlbranchHO').val();

                //cCallbackPanel.PerformCallback(asondatewise + '~' + ledger + '~' + ledgertype + '~' + branchid);
                //cpopup2ndLevel.Show();
                if (ledger.trim() == "0") {
                    jAlert('Zooming from Total is not possible.');
                    cpopup2ndLevel.Hide();
                }
                else {
                    cCallbackPanel.PerformCallback(asondatewise + '~' + ledger + '~' + ledgertype + '~' + branchid);
                    cpopup2ndLevel.Show();
                }
                return true;
            }
        }

        function OnRowClick(e) {
            var index = Grid.GetFocusedRowIndex();
            //if (AsonWise == false) {
            //    ason = 'N';
            //}
            //else {
            //    ason = 'Y';
            //}
            ason = 'N';
            var index = Grid.GetFocusedRowIndex();
            var ledger = Grid.GetRow(index).children[0].innerHTML;
            var branchid = $('#ddlbranchHO').val();
            cCallbackPanel.PerformCallback(ledger + "~" + ason + "~" + branchid);
            //cShowGridDetails2Level.PerformCallback(ledger + "~" + branchid);
            cpopup2ndLevel.Show();

        }

        function closePopup(s, e) {
            e.cancel = false;
            Grid.Focus();
            $("#drdExport").val(0);
        }

       

    </script>
    <%--end 2nd level popup--%>

    <%--end 3rd level popup--%>
    <script type="text/javascript">
        function OnWaitingGridKeyPress3(e) {
            if (e.code == "Enter" || e.code == "NumpadEnter") {
            }
        }

        function EndShowGridDetails3Level() {

            cShowGridDetails3Level.Focus();
            ctxtProductCode3rdLevel.SetText(ctxtProductCode2ndLevel.GetText());
            ctxtProductDesc3rdLevel.SetText(ctxtProductDesc2ndLevel.GetText());
            $("#lblFromDate3rdLevel")[0].innerHTML = $("#lblFromDate2ndLevel")[0].innerHTML;
            $("#lblToDate3rdLevel")[0].innerHTML = $("#lblToDate2ndLevel")[0].innerHTML;
        }

        function OnGetRowValuesLvl3(Uniqueid, type, docno) {
            //alert(Uniqueid + ' ' + type);
            var url = '';
            if (type == 'POS') {
                url = '/OMS/Management/Activities/posSalesInvoice.aspx?key=' + Uniqueid + '&IsTagged=1&Viemode=1';
            }
            else if (type == 'SI') {
                url = '/OMS/Management/Activities/SalesInvoice.aspx?key=' + Uniqueid + '&IsTagged=1&req=V&type=' + type;
            }

            else if (type == 'PC') {
                url = '/OMS/Management/Activities/PurchaseChallan.aspx?key=' + Uniqueid + '&req=V&IsTagged=1&type=' + type;
            }

            else if (type == 'SR') {
                url = '/OMS/Management/Activities/SalesReturn.aspx?key=' + Uniqueid + '&IsTagged=1&req=V&type=' + type;
            }

            else if (type == 'SRM') {
                url = '/OMS/Management/Activities/ReturnManual.aspx?key=' + Uniqueid + '&IsTagged=1&req=V&type=' + type;
            }

            else if (type == 'SRN') {
                url = '/OMS/Management/Activities/ReturnNormal.aspx?key=' + Uniqueid + '&IsTagged=1&req=V&type=' + type;
            }

            else if (type == 'PI') {
                url = '/OMS/Management/Activities/PurchaseInvoice.aspx?key=' + Uniqueid + '&IsTagged=1&req=V&type=PB';
            }


            else if (type == 'VP' || type == 'VR') {
                url = '/OMS/Management/Activities/VendorPaymentReceipt.aspx?key=' + Uniqueid + '&IsTagged=1&req=V&type=VPR';
            }

            else if (type == 'PR') {
                url = '/OMS/Management/Activities/PReturn.aspx?key=' + Uniqueid + '&IsTagged=1&req=V&type=PR';
            }

            else if (type == 'SC') {
                url = '/OMS/Management/Activities/CustomerReturn.aspx?key=' + Uniqueid + '&IsTagged=1&req=V&type=' + type;
            }

            else if (type == 'CP' || type == 'CR') {
                url = '/OMS/Management/Activities/CustomerReceiptPayment.aspx?key=' + Uniqueid + '&IsTagged=1&req=V&type=CRP';
            }

            else if (type == 'JV') {
                url = '/OMS/Management/dailytask/JournalEntry.aspx?key=' + Uniqueid + '&IsTagged=1&req=' + docno;
            }
            else if (type == 'CBV') {
                url = '/OMS/Management/dailytask/CashBankEntry.aspx?key=' + Uniqueid + '&IsTagged=1&req=V';
            }

            else if (type == 'CNC' || type == 'DNC') {
                url = '/OMS/Management/Activities/CustomerNote.aspx?key=' + Uniqueid + '&IsTagged=1&req=V&type=CDCN';
            }

            else if (type == 'CNV' || type == 'DNV') {
                url = '/OMS/Management/Activities/VendorDebitCreditNote.aspx?key=' + Uniqueid + '&IsTagged=1&req=V&type=CDCN';
            }

            else if (type == 'TPB') {

                url = '/OMS/Management/Activities/TPurchaseInvoice.aspx?key=' + Uniqueid + '&IsTagged=1&req=V&type=' + type;
            }

            else if (type == 'TSI') {

                url = '/OMS/Management/Activities/TSalesInvoice.aspx?key=' + Uniqueid + '&IsTagged=1&req=V&type=' + type;
            }
            else if (type == 'IPB') {
                url = '/Import/PurchaseInvoice-Import.aspx?key=' + Uniqueid + '&IsTagged=1&req=V&type=' + type;
            }
            popupdocument.SetContentUrl(url);
            popupdocument.Show();
        }

        function DateChangeForFrom() {

            var sessionValFrom = "<%=Session["FinYearStart"]%>";
            var sessionValTo = "<%=Session["FinYearEnd"]%>";
            var sessionVal = "<%=Session["LastFinYear"]%>";
            var objsession = sessionVal.split('-');
            var MonthDate = cxdeFromDate.GetDate().getMonth() + 1;
            var DayDate = cxdeFromDate.GetDate().getDate();
            var YearDate = cxdeFromDate.GetDate().getYear();
            var Cdate = MonthDate + "/" + DayDate + "/" + YearDate;
            var Sto = new Date(sessionValTo).getMonth() + 1;
            var SFrom = new Date(sessionValFrom).getMonth() + 1;
            var SDto = new Date(sessionValTo).getDate();
            var SDFrom = new Date(sessionValFrom).getDate();

            if (YearDate >= objsession[0]) {
                if (MonthDate < SFrom && YearDate == objsession[0]) {
                    jAlert('Enter Date Is Outside Of Financial Year !!');
                    var datePost = (SFrom + '-' + SDFrom + '-' + objsession[0]);
                    cxdeFromDate.SetDate(new Date(datePost));
                }
                else if (MonthDate > Sto && YearDate == objsession[1]) {
                    jAlert('Enter Date Is Outside Of Financial Year !!');
                    var datePost = (SFrom + '-' + SDFrom + '-' + objsession[0]);
                    cxdeFromDate.SetDate(new Date(datePost));
                }
                else if (YearDate != objsession[0] && YearDate != objsession[1]) {
                    jAlert('Enter Date Is Outside Of Financial Year !!');
                    var datePost = (SFrom + '-' + SDFrom + '-' + objsession[0]);
                    cxdeFromDate.SetDate(new Date(datePost));
                }
            }
            else {
                jAlert('Enter Date Is Outside Of Financial Year !!');
                var datePost = (SFrom + '-' + SDFrom + '-' + objsession[0]);
                cxdeFromDate.SetDate(new Date(datePost));
            }
        }
        function DateChangeForTo() {
            var sessionValFrom = "<%=Session["FinYearStart"]%>";
            var sessionValTo = "<%=Session["FinYearEnd"]%>";
            var sessionVal = "<%=Session["LastFinYear"]%>";
            var objsession = sessionVal.split('-');
            var MonthDate = cxdeToDate.GetDate().getMonth() + 1;
            var DayDate = cxdeToDate.GetDate().getDate();
            var YearDate = cxdeToDate.GetDate().getYear();
            var Cdate = MonthDate + "/" + DayDate + "/" + YearDate;
            var Sto = new Date(sessionValTo).getMonth() + 1;
            var SFrom = new Date(sessionValFrom).getMonth() + 1;
            var SDto = new Date(sessionValTo).getDate();
            var SDFrom = new Date(sessionValFrom).getDate();

            if (YearDate <= objsession[1]) {
                if (MonthDate < SFrom && YearDate == objsession[0]) {
                    jAlert('Enter Date Is Outside Of Financial Year !!');
                    var datePost = (Sto + '-' + SDto + '-' + objsession[1]);
                    cxdeToDate.SetDate(new Date(datePost));
                }
                else if (MonthDate > Sto && YearDate == objsession[1]) {
                    jAlert('Enter Date Is Outside Of Financial Year !!');
                    var datePost = (Sto + '-' + SDto + '-' + objsession[1]);
                    cxdeToDate.SetDate(new Date(datePost));
                }
                else if (YearDate != objsession[0] && YearDate != objsession[1]) {
                    jAlert('Enter Date Is Outside Of Financial Year !!');
                    var datePost = (Sto + '-' + SDto + '-' + objsession[1]);
                    cxdeToDate.SetDate(new Date(datePost));
                }
            }
            else {
                jAlert('Enter Date Is Outside Of Financial Year !!');
                var datePost = (Sto + '-' + SDto + '-' + objsession[1]);
                cxdeToDate.SetDate(new Date(datePost));
            }
        }
    </script>
    <%--end 3rd level popup--%>
    <style>
        .padtbl > tbody > tr > td {
            padding-right: 10px;
        }

        input[type="checkbox"] {
            -webkit-transform: translateY(2px);
            -moz-transform: translateY(2px);
            transform: translateY(2px);
        }
        .plhead a {
            font-size:16px;
            padding-left:10px;
            position:relative;
            width:100%;
            display:block;
            padding:9px 10px 5px 10px;
        }
        .plhead a>i {
            position:absolute;
            top:11px;
            right:15px;
        }
        #accordion {
            margin-bottom:10px;
        }
        .companyName {
            font-size:16px;
            font-weight:bold;
            margin-bottom:15px;
        }
       
        .plhead a.collapsed .fa-minus-circle{
            display:none;
        }
    </style>

    <script>
        function SetExport() {
            document.getElementById('ddlExport2').value = "0";
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


    <div class="panel-heading">
        <%--<div class="panel-title">
            <h3>Profit & Loss Statement (Tabular) </h3>
        </div>--%>
        <div class="panel-group" id="accordion" role="tablist" aria-multiselectable="true">
          <div class="panel panel-info">
            <div class="panel-heading" role="tab" id="headingOne">
              <h4 class="panel-title plhead">
                <a role="button" data-toggle="collapse" data-parent="#accordion" href="#collapseOne" aria-expanded="true" aria-controls="collapseOne" class="collapsed">
                    <asp:Label ID="RptHeading" runat="Server" Text="" Width="470px" style="font-weight:bold;"></asp:Label>
                    <i class="fa fa-plus-circle" ></i>
                    <i class="fa fa-minus-circle"></i>
                </a>
              </h4>
            </div>
            <div id="collapseOne" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingOne">
              <div class="panel-body">
                    <div class="companyName">
                        <asp:Label ID="CompName" runat="Server" Text="" Width="470px" style="font-size: 12px;"></asp:Label>
                    </div>
                   <%--Rev Subhra 20-12-2018   0017670--%>
                    <div>
                        <asp:Label ID="CompBranch" runat="Server" Text="" Width="470px" ></asp:Label>
                    </div>
                    <%--End of Rev--%>
                    <div>
                        <asp:Label ID="CompAdd" runat="Server" Text="" Width="470px" style="font-size: 12px;"></asp:Label>
                    </div>
                    <div>
                        <asp:Label ID="CompOth" runat="Server" Text="" Width="470px" style="font-size: 12px;"></asp:Label>
                    </div>
                    <div>
                        <asp:Label ID="CompPh" runat="Server" Text="" Width="470px" style="font-size: 12px;"></asp:Label>
                    </div>       
                    <div>
                        <asp:Label ID="CompAccPrd" runat="Server" Text="" Width="470px" style="font-size: 12px;"></asp:Label>
                    </div>
                    <div>
                        <asp:Label ID="DateRange" runat="Server" Text="" Width="470px" style="font-size: 12px;"></asp:Label>
                    </div>
              </div>
            </div>
          </div>
        </div>

        <div>
            
        </div>
        
    </div>
    <%--<div>
        <asp:Label ID="ACPrd" runat="Server" Text="" Width="1192px" Style="font-size: 14px; font-weight: bold;"></asp:Label>
    </div>--%>
    <div class="form_main">
        <asp:HiddenField runat="server" ID="hdndaily" />
        <asp:HiddenField runat="server" ID="hdtid" />
        <div class="row">
            <div class="col-md-2">
                <label style="color: #b5285f; font-weight: bold;" class="clsFrom">
                    <asp:Label ID="lblFromDate" runat="Server" Text="From Date : " CssClass="mylabel1"
                        Width="92px" Font-Bold="True"></asp:Label>
                </label>
                <dxe:ASPxDateEdit ID="ASPxFromDate" runat="server" EditFormat="custom" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy"
                    UseMaskBehavior="True" Width="100%" ClientInstanceName="cxdeFromDate">
                    <buttonstyle width="13px">
                        </buttonstyle>
                </dxe:ASPxDateEdit>
            </div>
            <div class="col-md-2" id="dvtodate">
                <label style="color: #b5285f; font-weight: bold;" class="clsTo">
                    <asp:Label ID="lblToDate" runat="Server" Text="To Date : " CssClass="mylabel1"
                        Width="92px" Font-Bold="True"></asp:Label>
                </label>
                <dxe:ASPxDateEdit ID="ASPxToDate" runat="server" EditFormat="custom" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy"
                    UseMaskBehavior="True" Width="100%" ClientInstanceName="cxdeToDate">
                    <buttonstyle width="13px">
                    </buttonstyle>
                </dxe:ASPxDateEdit>
            </div>

            <div class="col-md-2">
                <div style="color: #b5285f; font-weight: bold;" class="clsTo">
                    <label style="color: #b5285f; font-weight: bold;" class="clsTo">
                        <asp:Label ID="Label2" runat="Server" Text="Head Branch : " CssClass="mylabel1"
                            Width="92px" Font-Bold="True"></asp:Label>
                    </label>
                </div>
                <asp:DropDownList ID="ddlbranchHO" runat="server" Width="100%"></asp:DropDownList>
            </div>

            <div class="col-md-2">
                <label style="color: #b5285f; font-weight: bold;" class="clsTo">
                    <asp:Label ID="Label1" runat="Server" Text="Branch : " CssClass="mylabel1"
                        Width="92px" Font-Bold="True"></asp:Label>
                </label>
                <asp:HiddenField ID="hdnActivityType" runat="server" />
                <asp:HiddenField ID="hdnActivityTypeText" runat="server" />
                <span id="MandatoryActivityType" style="display: none" class="validclass">
                    <img id="3gridHistory_DXPEForm_efnew_DXEFL_DXEditor1112_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></span>
                <asp:HiddenField ID="hdnSelectedBranches" runat="server" />

                <dxe:ASPxCallbackPanel runat="server" ID="ASPxCallbackPanel1" ClientInstanceName="cbranchPanel" OnCallback="Componentbranch_Callback">
                    <panelcollection>
                            <dxe:PanelContent runat="server">
                                <dxe:ASPxGridLookup ID="lookup_branch" SelectionMode="Multiple" runat="server" ClientInstanceName="gridbranchLookup"
                                    OnDataBinding="lookup_branch_DataBinding"
                                    KeyFieldName="ID" Width="100%" TextFormatString="{1}" AutoGenerateColumns="False" MultiTextSeparator=", ">
                                    <Columns>
                                        <dxe:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Width="60" Caption=" " />
                                        <dxe:GridViewDataColumn FieldName="branch_code" Visible="true" VisibleIndex="1" width="200px" Caption="Branch code" Settings-AutoFilterCondition="Contains">
                                            <Settings AutoFilterCondition="Contains" />
                                        </dxe:GridViewDataColumn>

                                        <dxe:GridViewDataColumn FieldName="branch_description" Visible="true" VisibleIndex="2" width="200px" Caption="Branch Name" Settings-AutoFilterCondition="Contains">
                                            <Settings AutoFilterCondition="Contains" />
                                        </dxe:GridViewDataColumn>
                                    </Columns>
                                    <GridViewProperties Settings-VerticalScrollBarMode="Auto" SettingsPager-Mode="ShowAllRecords">
                                        <Templates>
                                            <StatusBar>
                                                <table class="OptionsTable" style="float: right">
                                                    <tr>
                                                        <td>
                                                            <dxe:ASPxButton ID="ASPxButton3" runat="server" AutoPostBack="false" Text="Select All" ClientSideEvents-Click="selectAll_Branch" />
                                                            <dxe:ASPxButton ID="ASPxButton4" runat="server" AutoPostBack="false" Text="Deselect All" ClientSideEvents-Click="unselectAll_Branch" />                                                            
                                                            <dxe:ASPxButton ID="Close" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseGridBranchLookupbranch" UseSubmitBehavior="False" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </StatusBar>
                                        </Templates>
                                        <SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True" />
                                        <SettingsPager Mode="ShowPager">
                                        </SettingsPager>

                                        <SettingsPager PageSize="20">
                                            <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,20,50,100,150,200" />
                                        </SettingsPager>

                                        <Settings ShowFilterRow="True" ShowFilterRowMenu="true" ShowStatusBar="Visible" UseFixedTableLayout="true" />
                                    </GridViewProperties>
                                </dxe:ASPxGridLookup>
                            </dxe:PanelContent>
                        </panelcollection>
                </dxe:ASPxCallbackPanel>
            </div>
            <div class="hide">
                <div class="col-md-2">
                    <div style="color: #b5285f; font-weight: bold;" class="clsTo">
                        <label style="color: #b5285f; font-weight: bold;" class="clsTo">
                            <asp:Label ID="Label4" runat="Server" Text="Layout : " CssClass="mylabel1"
                                Width="92px" Font-Bold="True"></asp:Label>
                        </label>
                    </div>
                    <asp:DropDownList ID="ddlLayout" runat="server" Width="100%"></asp:DropDownList>
                </div>
            </div>
            
            <div class="col-md-3" style="padding-top: 15px;color: #b5285f; font-weight: bold;">
                <div style="padding-right: 10px; vertical-align: middle; padding-top: 6px">
                    <asp:CheckBox ID="chkZero" runat="server" Checked="true" Text="Show Zero Balance Account" />
                </div>
            </div>
            <div class="clear"></div>
            <%--<div class="col-md-2" style="padding-top: 16px">
                <div style="padding-right: 10px; vertical-align: middle; padding-top: 6px">
                    <asp:CheckBox ID="chkPL" runat="server" Text="Consider Closing Stock" />
                </div>
            </div>--%>

            <div class="col-md-2" style="margin-top:15px;color: #b5285f; font-weight: bold;">
             <dxe:ASPxCheckBox runat="server" ID="chkPL" Checked="false" Text="Consider Closing Stock" >
                 <ClientSideEvents CheckedChanged="CheckConsPL" />
             </dxe:ASPxCheckBox>
            </div>

             <div class="col-md-2" style="padding-top: 1px;color: #b5285f; font-weight: bold;">
                <div style="padding-right: 10px; vertical-align: middle; padding-top: 1px">
                    <asp:Label ID="Label7" runat="Server" Text="Valuation Technique : " CssClass="mylabel1"></asp:Label>
                </div>
                <div>
                    <asp:DropDownList ID="ddlValTech" runat="server" Width="100%" Enabled="false">
                        <asp:ListItem Text="Average" Value="A"></asp:ListItem>
                        <asp:ListItem Text="FIFO" Value="F"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>

            <div class="col-md-3" style="margin-top:15px;color: #b5285f; font-weight: bold;">
                <dxe:ASPxCheckBox runat="server" ID="chkOWMSTVT" Checked="false" ClientEnabled="false" Text="Override Product Valuation Technique in Master" ClientInstanceName="CchkOWMSTVT">
                </dxe:ASPxCheckBox>
            </div> 
            
            <div class="col-md-2" style="margin-top:15px;color: #b5285f; font-weight: bold;">
                <dxe:ASPxCheckBox runat="server" ID="chkConsLandCost" Checked="false" ClientEnabled="false" Text="Consider Landed Cost" ClientInstanceName="CchkConsLandCost">
                </dxe:ASPxCheckBox>
             </div>
            <div class="col-md-2" style="margin-top:15px;color: #b5285f; font-weight: bold;">
                <dxe:ASPxCheckBox runat="server" ID="chkConsOverHeadCost" Checked="false" ClientEnabled="false" Text="Consider Overhead Cost" ClientInstanceName="CchkConsOverHeadCost">
                </dxe:ASPxCheckBox>
            </div>
            <div class="clear"></div>

            <div class="col-md-2" style="padding-top: 5px">
                <button id="btnShow" class="btn btn-success" type="button" onclick="btn_ShowRecordsClick(this);">Show</button>
            </div>
            <div class="col-md-2" style="padding-top: 20px;display: none">
                <%--<button id="btnShow" class="btn btn-primary" type="button" onclick="btn_ShowRecordsClick(this);">Show</button>--%>
                <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-sm btn-primary"
                    OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true" OnChange="SetExport">
                    <%--OnChange="if(!AvailableExportOption()){return false;}">--%>
                    <asp:ListItem Value="0">Export to</asp:ListItem>
                    <asp:ListItem Value="1">XLSX</asp:ListItem>
                    <asp:ListItem Value="2">PDF</asp:ListItem>
                    <asp:ListItem Value="3">CSV</asp:ListItem>
                    <asp:ListItem Value="4">RTF</asp:ListItem>

                </asp:DropDownList>

            </div>


            <div class="col-md-2" style="padding-top: 6px;display: none">
                <asp:DropDownList ID="drdExport2" runat="server" CssClass="btn btn-sm btn-primary"
                    OnSelectedIndexChanged="drdExport2_SelectedIndexChanged" AutoPostBack="true">
                    <asp:ListItem Value="0">Export to</asp:ListItem>
                    <asp:ListItem Value="1">XLSX</asp:ListItem>
                    <asp:ListItem Value="2">PDF</asp:ListItem>
                    <asp:ListItem Value="3">CSV</asp:ListItem>
                    <asp:ListItem Value="4">RTF</asp:ListItem>

                </asp:DropDownList>
            </div>

            <div class="clear"></div>
            <div class="col-md-2"></div>
        </div>
        <%--<table class="pull-left">
            <tr>
                <td></td>
                <td style="width: 254px"></td>
                <asp:HiddenField ID="HiddenField1" runat="server" />
                <span id="MandatoryCustomerType" style="display: none" class="validclass">
                    <img id="3gridHistory_DXPEForm_efnew_DXEFL_DXEditor1112_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></span>
                </td>
            </tr>
        </table>--%>
        <div class="clear"></div>
        <dxe:ASPxCallbackPanel runat="server" ID="ProffitnLossPanel" ClientInstanceName="cProffitnLossPanel" OnCallback="ComponentPnL_Callback">
            <panelcollection>
        <dxe:PanelContent runat="server">
   
        <div class="MasterDIV row">
         <div id="leftPanel" class="col-md-6 " style="padding-right:0px">
                    <div onkeypress="OnWaitingGridKeyPress(event)">
                        <dxe:ASPxGridView runat="server" ID="ShowGrid" ClientInstanceName="Grid" Width="100%" EnableRowsCache="false" AutoGenerateColumns="False" KeyboardSupport="true" KeyFieldName="SLNO"
                             OnDataBound="ShowGrid_DataBound"  OnHtmlDataCellPrepared="ShowGrid_HtmlDataCellPrepared"  
                            SettingsBehavior-AllowFocusedRow="true" SettingsBehavior-AllowSelectSingleRowOnly="true" Settings-HorizontalScrollBarMode="Visible" 
                            DataSourceID="GenerateEntityServerModeDataSource">
                             <%--OnDataBinding="ShowGrid_DataBinding"--%>
                            <columns>                               
                                 <dxe:GridViewDataTextColumn FieldName="MAINACCOUNT_NAME" Caption="Particulars" Width="70%" VisibleIndex="1" Settings-AllowAutoFilter="true">                                    
                                    <CellStyle HorizontalAlign="Left"></CellStyle>
                                    <HeaderStyle HorizontalAlign="Left" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="PERIOD" Caption="Amount" Width="30%" VisibleIndex="2" Settings-AllowAutoFilter="False">                                    
                                    <PropertiesTextEdit Style-HorizontalAlign="Right" DisplayFormatString="0.00"></PropertiesTextEdit>
                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="TYPEX" Caption="HIDETYPEX" Width="0" VisibleIndex="3" >
                                  <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="MAINACCOUNT_REFERENCEID" Caption="MAINACCOUNT_REFERENCEID" Width="0%" VisibleIndex="4" />

                            </columns>
                            <settingsbehavior confirmdelete="true" enablecustomizationwindow="true" enablerowhottrack="true" />
                            <settings showfooter="true" showgrouppanel="false" showgroupfooter="VisibleIfExpanded" />
                            <settingsediting mode="EditForm" />
                            <settingscontextmenu enabled="true" />
                            <settingsbehavior autoexpandallgroups="true" columnresizemode="Control" />
                            <settings showgrouppanel="false" showstatusbar="Visible" showfilterrow="true" />
                            <settingssearchpanel visible="False" />
                            <settingspager mode="ShowAllRecords"></settingspager>
                            <settings showfilterrow="True" showfilterrowmenu="true" showstatusbar="Visible" usefixedtablelayout="true" />
                        </dxe:ASPxGridView>
                         <dx:LinqServerModeDataSource ID="GenerateEntityServerModeDataSource" runat="server" OnSelecting="GenerateEntityServerModeDataSource_Selecting"
                        ContextTypeName="ReportSourceDataContext" TableName="PROFITANDLOSSTABULAR_REPORT" />
                    </div>
            </div>

       <div id="rightPanel" class="col-md-6" style="padding-left:0px"  >
                    <div onkeypress="OnWaitingGridKeyPress2(event)">
                        <dxe:ASPxGridView runat="server" ID="ShowGrid_Income" ClientInstanceName="GridIncome" Width="100%" EnableRowsCache="false" AutoGenerateColumns="False" KeyboardSupport="true" KeyFieldName="SLNO"
                              OnDataBound="ShowGrid_Income_DataBound"  OnHtmlDataCellPrepared="ShowGrid_Income_HtmlDataCellPrepared"  
                            SettingsBehavior-AllowFocusedRow="true" SettingsBehavior-AllowSelectSingleRowOnly="true" Settings-HorizontalScrollBarMode="Visible" DataSourceID="GenerateEntityServerModeIncomeDataSource">
                           <%-- OnDataBinding="ShowGrid_Income_DataBinding"--%>
                            <columns>                          
                                <dxe:GridViewDataTextColumn FieldName="MAINACCOUNT_NAME" Caption="Particulars" Width="70%" VisibleIndex="1" Settings-AllowAutoFilter="true">                                    
                                    <CellStyle HorizontalAlign="Left"></CellStyle>
                                    <HeaderStyle HorizontalAlign="Left" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="PERIOD" Caption="Amount" Width="30%" VisibleIndex="2" Settings-AllowAutoFilter="False">                                    
                                    <PropertiesTextEdit Style-HorizontalAlign="Right" DisplayFormatString="0.00"></PropertiesTextEdit>
                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="TYPEX" Caption="HIDETYPEX" Width="0" VisibleIndex="3" >
                                <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="MAINACCOUNT_REFERENCEID" Caption="MAINACCOUNT_REFERENCEID" Width="0%" VisibleIndex="4" />
                            </columns>
                            <settingsbehavior confirmdelete="true" enablecustomizationwindow="true" enablerowhottrack="true" />
                            <settings showfooter="true" showgrouppanel="false" showgroupfooter="VisibleIfExpanded" />
                            <settingsediting mode="EditForm" />
                            <settingscontextmenu enabled="true" />
                            <settingsbehavior autoexpandallgroups="true" columnresizemode="Control" />
                            <settings showgrouppanel="false" showstatusbar="Visible" showfilterrow="true" />
                            <settingssearchpanel visible="False" />
                            <settingspager mode="ShowAllRecords"></settingspager>
                            <settings showfilterrow="True" showfilterrowmenu="true" showstatusbar="Visible" usefixedtablelayout="true" />
                        </dxe:ASPxGridView>
                         <dx:LinqServerModeDataSource ID="GenerateEntityServerModeIncomeDataSource" runat="server" OnSelecting="GenerateEntityServerModeDataSourceIncome_Selecting"
                        ContextTypeName="ReportSourceDataContext" TableName="PROFITANDLOSSTABULAR_REPORT" />
                    </div>
            </div>

        </div>
        </dxe:PanelContent>
    </panelcollection>
            <clientsideevents endcallback="ProffitnLossPanelEndCallBack" />
        </dxe:ASPxCallbackPanel>

    </div>

    <dxe:ASPxPopupControl ID="popup" runat="server" ClientInstanceName="cpopup2ndLevel"
        Width="1100px" Height="600px" ScrollBars="Vertical" HeaderText="Profit And Loss Statement - Detailed" PopupHorizontalAlign="WindowCenter" HeaderStyle-CssClass="wht"
        PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
        ContentStyle-CssClass="pad">
        <clientsideevents closing="function(s, e) {
	        closePopup(s, e);}" />
        <contentstyle verticalalign="Top" cssclass="pad">
            </contentstyle>
        <contentcollection>
                <dxe:PopupControlContentControl runat="server">
                    <input id="hfProductID2" type="hidden" />
                    <input id="hfBranchID3" type="hidden" />
                    <div class="col-md-12">
                        <div class="row clearfix">
                            <table class="pdbot" style="margin: 4px 0 16px 10px; float: left;">
                            <tr>
                                <td style="padding-top: 10px">
                                    <label style="color: #b5285f; font-weight: bold; margin-top: 5px;" class="clsTo">
                                        <asp:Label ID="Label3" runat="Server" CssClass="mylabel1" ClientInstanceName="clblLedger2ndLevel"></asp:Label>
                                    </label>
                                </td>
                                <td style="padding-top: 10px">
                                    <dxe:ASPxTextBox ID="txtLedger2ndLevel" ClientInstanceName="ctxtLedger2ndLevel" runat="server" ReadOnly="true" Width="600px"></dxe:ASPxTextBox>
                                </td>

                            </tr>

                            <tr>
                                <td></td>
                                <td>
                                    <label style="color: #b5285f; font-weight: bold; margin-top: 5px;" class="clsTo">
                                        <asp:Label ID="lblFromDate2ndLevel" runat="Server" Font-Bold="true" CssClass="mylabel1"></asp:Label>
                                    </label>
                                    <label style="color: #b5285f; font-weight: bold; margin-top: 5px;" class="clsTo">
                                        <asp:Label ID="lblToDate2ndLevel" runat="Server" Font-Bold="true"  CssClass="mylabel1"></asp:Label>
                                    </label>
                                    <span style="padding-left: 10px;color: #b5285f; display: inline-block"><strong>Press < Esc > Key to Close</strong></span></td>
                                
                            </tr>
                        </table>
                            <div class="pull-right" style="padding-top: 26px;">
                                
                                <asp:DropDownList ID="ddlExport3" runat="server" CssClass="btn btn-sm btn-primary" AutoPostBack="true" OnSelectedIndexChanged="ddlExport3_SelectedIndexChanged">
                                    <asp:ListItem Value="0">Export to</asp:ListItem>
                                    <asp:ListItem Value="1">PDF</asp:ListItem>
                                    <asp:ListItem Value="2">XLSX</asp:ListItem>
                                    <asp:ListItem Value="3">RTF</asp:ListItem>
                                    <asp:ListItem Value="4">CSV</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div class="clearfix" onkeypress="OnWaitingGridKeyPress3(event)">
                        <dxe:ASPxGridView runat="server" ID="ShowGridDetails2Level" ClientInstanceName="cShowGridDetails2Level" KeyFieldName="SEQ" Width="100%" EnableRowsCache="false" AutoGenerateColumns="False"
                            OnDataBinding="ShowGridDetails2Level_DataBinding" KeyboardSupport="true" Settings-HorizontalScrollBarMode="Visible"
                            OnSummaryDisplayText="ShowGridDetails2Level_SummaryDisplayText"  DataSourceID="GenerateEntityServerModeDetailDataSource">

                            <Columns>
                                <%--OnCustomCallback="ShowGridDetails2Level_CustomCallback"--%>

                                <dxe:GridViewDataTextColumn FieldName="BRANCH_DESC" Caption="Unit" Width="30%" VisibleIndex="1" Settings-AllowAutoFilter="False"/>
                                <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="DOC_NO" Caption="Document No." Width="30%" >
                                <CellStyle HorizontalAlign="Left">
                                </CellStyle>
                                <HeaderStyle HorizontalAlign="Center" />
                                <DataItemTemplate>
                                      <a href="javascript:void(0)" onclick="OnGetRowValuesLvl3('<%#Eval("DOC_ID") %>','<%#Eval("MODULE_TYPE") %>' ,'<%#Eval("DOC_NO") %>')" class="pad">
                                        <%#Eval("DOC_NO")%>
                                      </a> 
                                </DataItemTemplate>
                                <EditFormSettings Visible="False" />
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="DOC_TYPE" Caption="Document Type" Width="30%" VisibleIndex="3" Settings-AllowAutoFilter="False"/>

                                <dxe:GridViewDataTextColumn FieldName="PARTY_NAME" Caption="Party" Width="20%" VisibleIndex="3" />

                                <dxe:GridViewDataTextColumn FieldName="OP_DR_AMT" Caption="Opening (Dr.)" Width="15%" VisibleIndex="4" PropertiesTextEdit-DisplayFormatString="0.00" Settings-AllowAutoFilter="False">
                                </dxe:GridViewDataTextColumn>
                           
                                <dxe:GridViewDataTextColumn FieldName="OP_CR_AMT" Caption="Opening (Cr.)" Width="15%" VisibleIndex="5" PropertiesTextEdit-DisplayFormatString="0.00" Settings-AllowAutoFilter="False">
                                </dxe:GridViewDataTextColumn>
                                
                                <dxe:GridViewDataTextColumn FieldName="PR_DR_AMT" Caption="Period (Dr.)" Width="15%" VisibleIndex="6" PropertiesTextEdit-DisplayFormatString="0.00" Settings-AllowAutoFilter="False">
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="PR_CR_AMT" Caption="Period (Cr.)" Width="15%" VisibleIndex="7" PropertiesTextEdit-DisplayFormatString="0.00" Settings-AllowAutoFilter="False">
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="CL_DR_AMT" Caption="Closing (Dr.)" Width="15%" VisibleIndex="8" PropertiesTextEdit-DisplayFormatString="0.00" Settings-AllowAutoFilter="False">
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="CL_CR_AMT" Caption="Closing (Cr.)" Width="15%" VisibleIndex="9" PropertiesTextEdit-DisplayFormatString="0.00" Settings-AllowAutoFilter="False">
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="Doc_Id" Caption="Doc_Id" Width="15%" VisibleIndex="10" Visible="false">
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="MODULE_TYPE" Caption="MODULE_TYPE" Width="15%" VisibleIndex="11" Visible="false">
                                </dxe:GridViewDataTextColumn>
                            </Columns>
                            <SettingsBehavior ConfirmDelete="true" EnableCustomizationWindow="true" ColumnResizeMode="Control" />
                            <Settings ShowFooter="true" ShowGroupPanel="true" ShowGroupFooter="VisibleIfExpanded" />
                            <SettingsEditing Mode="EditForm" />
                            <SettingsContextMenu Enabled="true" />
                            <SettingsBehavior AutoExpandAllGroups="true" />
                            <Settings ShowGroupPanel="True" ShowStatusBar="Visible" ShowFilterRow="true" ShowFilterRowMenu="true" />
                            <SettingsSearchPanel Visible="false" />
                            <SettingsPager PageSize="10">
                                <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
                            </SettingsPager>
                            <TotalSummary>
                                 <dxe:ASPxSummaryItem FieldName="OP_DR_AMT" SummaryType="Sum"/>
                                 <dxe:ASPxSummaryItem FieldName="OP_CR_AMT" SummaryType="Sum"/>
                                 <dxe:ASPxSummaryItem FieldName="PR_DR_AMT" SummaryType="Sum"/>
                                 <dxe:ASPxSummaryItem FieldName="PR_CR_AMT" SummaryType="Sum"/>
                                 <dxe:ASPxSummaryItem FieldName="CL_DR_AMT" SummaryType="Sum"/>
                                 <dxe:ASPxSummaryItem FieldName="CL_CR_AMT" SummaryType="Sum"/>
                            </TotalSummary>
                           <%-- <ClientSideEvents EndCallback="EndShowGridDetails2Level" />--%>

                        </dxe:ASPxGridView>
                          <dx:LinqServerModeDataSource ID="GenerateEntityServerModeDetailDataSource" runat="server" OnSelecting="GenerateEntityServerModeDataSourceDetails_Selecting"
                        ContextTypeName="ReportSourceDataContext" TableName="PROFITANDLOSSDETAILSTATEMENT_REPORT" />
                    </div>
                </dxe:PopupControlContentControl>
            </contentcollection>

    </dxe:ASPxPopupControl>


    <dxe:ASPxPopupControl ID="ASPXPopupControl2" runat="server"
        CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="popupdocument" Height="500px"
        Width="1200px" HeaderText="Details" Modal="true" AllowResize="true">
        <contentcollection>
                <dxe:PopupControlContentControl runat="server">
                </dxe:PopupControlContentControl>
        </contentcollection>
    </dxe:ASPxPopupControl>

    <dxe:ASPxCallbackPanel runat="server" ID="CallbackPanel" ClientInstanceName="cCallbackPanel" OnCallback="CallbackPanel_Callback">
        <PanelCollection>
            <dxe:PanelContent runat="server">
                <asp:HiddenField ID="hfIsProfnlossDetails" runat="server" />
            </dxe:PanelContent>
        </PanelCollection>
        <ClientSideEvents EndCallback="CallbackPanelDetEndCall" />
    </dxe:ASPxCallbackPanel>

    <asp:HiddenField ID="hfIsProfnLossFilter" runat="server" />
    <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
    </dxe:ASPxGridViewExporter>
    <dxe:ASPxGridViewExporter ID="exporter2" runat="server" Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
    </dxe:ASPxGridViewExporter>

    <dxe:ASPxGridViewExporter ID="exporterDetails" runat="server" Landscape="true" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
    </dxe:ASPxGridViewExporter>
</asp:Content>
