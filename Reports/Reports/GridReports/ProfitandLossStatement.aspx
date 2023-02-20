<%--================================================== Revision History ============================================
Rev Number         DATE              VERSION          DEVELOPER           CHANGES
1.0                15-02-2023        V2.0.36           Pallab              25575 : Report pages design modification
====================================================== Revision History ================================================--%>

<%@ Page Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="ProfitandLossStatement.aspx.cs" Inherits="Reports.Reports.GridReports.ProfitandLossStatement" %>

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
        /*rev Pallab*/
        .branch-selection-box .dxlpLoadingPanel_PlasticBlue tbody, .branch-selection-box .dxlpLoadingPanelWithContent_PlasticBlue tbody, #divProj .dxlpLoadingPanel_PlasticBlue tbody, #divProj .dxlpLoadingPanelWithContent_PlasticBlue tbody
        {
            bottom: 0 !important;
        }
        .dxlpLoadingPanel_PlasticBlue tbody, .dxlpLoadingPanelWithContent_PlasticBlue tbody
        {
            position: absolute;
            bottom: 80px;
            background: #fff;
            box-shadow: 1px 1px 10px #0000001c;
            right: 50%;
        }
        /*rev end Pallab*/
    </style>


    <script type="text/javascript">
        $(function () {
            cProductbranchPanel.PerformCallback('BindComponentGrid' + '~' + $("#ddlbranchHO").val());
            $("#ddlbranchHO").change(function () {
                var Ids = $(this).val();
                cProductbranchPanel.PerformCallback('BindComponentGrid' + '~' + $("#ddlbranchHO").val());
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
        }
        function popupHide2(s, e) {
            popupdocument.Hide();
            cShowGridDetails2Level.Focus();
            $("#ddlExport3").val(0);
        }

        function Callback2_EndCallback() {
            if (Grid.cpErrorFinancial == 'ErrorFinancial') {
                jAlert('Date Range should be within Financial Year');
            }
            else {
                var Amount = parseFloat(Grid.cpSummary);
                ctxtdiffcalculation.SetText(Amount);
                ctxtdiffcalculationText.SetText('Mismatch Defeated');
                Grid.cpSummary = null;
                $("#drdExport").val(0);
                Grid.Focus();
                Grid.SetFocusedRowIndex(2);
            }
            Grid.cpErrorFinancial = null;
        }
        var AsonWise = false;
        var ason = 'Y';
        $(document).ready(function () {
            //document.getElementById("lblFromDate").innerHTML = 'As On Date'
            //document.getElementById("dvtodate").style.display = "none";
            //document.getElementById("ASPxFromDate").style.visibility = "visible";

            document.getElementById("lblToDate").innerHTML = 'As On Date :'
            document.getElementById("dvFromdate").style.display = "none";
            document.getElementById("ASPxToDate").style.visibility = "visible";
            AsonWise = true;
        })

        //function EndShowGridDetails2Level() {
        //    cShowGridDetails2Level.Focus();
        //    cShowGridDetails2Level.SetFocusedRowIndex(0);
        //    ctxtLedger2ndLevel.SetText(cShowGridDetails2Level.cpLedger);
        //    if (document.getElementById('radAsDate').checked) {
        //        $("#lblFromDate2ndLevel")[0].innerHTML = "As on : " + cShowGridDetails2Level.cpFromDate;
        //        $("#lblToDate2ndLevel")[0].innerHTML = "";
        //    }
        //    else if (document.getElementById('radPeriod').checked) {
        //        $("#lblFromDate2ndLevel")[0].innerHTML = "From " + cShowGridDetails2Level.cpFromDate;
        //        $("#lblToDate2ndLevel")[0].innerHTML = " To " + cShowGridDetails2Level.cpToDate;
        //    }
        //    if (cShowGridDetails2Level.cpLedgerType != "FOR BRANCH") {
        //        $("#Label3")[0].innerHTML = "Ledger :";
        //    }
        //    else {
        //        $("#Label3")[0].innerHTML = "Branch :";
        //    }
        //}

        function btn_ShowRecordsClick(e) {
            $("#hfIsProfnloss").val("Y");
            var data = "OnDateChanged";
            var branchid = $('#ddlbranchHO').val();
            var layoutid = $('#ddlLayout').val();
            //Grid.PerformCallback(data + '~' + AsonWise + '~' + branchid + '~' + layoutid);
            if (gridbranchLookup.GetValue() == null) {
                jAlert('Please select atleast one Branch for generate the report.');
            }
            else {
                cCallbackPanel.PerformCallback(data + '~' + AsonWise + '~' + branchid + '~' + layoutid);
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

        //function BindActivityType(noteTilte) {
        //    var lBox = $('select[id$=ListBoxBranches]');
        //    var listItems = [];
        //    var selectedNoteId = '';
        //    if (noteTilte) {

        //        selectedNoteId = noteTilte;
        //    }
        //    lBox.empty();

        //    $.ajax({
        //        type: "POST",
        //        url: 'GeneralTrialReport.aspx/GetBranchesList',
        //        contentType: "application/json; charset=utf-8",
        //        data: JSON.stringify({ NoteId: selectedNoteId }),
        //        dataType: "json",
        //        success: function (msg) {
        //            var list = msg.d;
        //            if (list.length > 0) {

        //                for (var i = 0; i < list.length; i++) {

        //                    var id = '';
        //                    var name = '';
        //                    id = list[i].split('|')[1];
        //                    name = list[i].split('|')[0];

        //                    listItems.push('<option value="' +
        //                    id + '">' + name
        //                    + '</option>');
        //                }
        //                $(lBox).append(listItems.join(''));
        //                ListActivityType();

        //                $('#ListBoxBranches').trigger("chosen:updated");
        //                $('#ListBoxBranches').prop('disabled', false).trigger("chosen:updated");
        //            }
        //            else {
        //                lBox.empty();
        //                $('#ListBoxBranches').trigger("chosen:updated");
        //                $('#ListBoxBranches').prop('disabled', true).trigger("chosen:updated");

        //            }
        //        },
        //        error: function (XMLHttpRequest, textStatus, errorThrown) {
        //        }
        //    });
        //}

        //function ListActivityType() {

        //    $('#ListBoxBranches').chosen();
        //    $('#ListBoxBranches').fadeIn();

        //    var config = {
        //        '.chsnProduct': {},
        //        '.chsnProduct-deselect': { allow_single_deselect: true },
        //        '.chsnProduct-no-single': { disable_search_threshold: 10 },
        //        '.chsnProduct-no-results': { no_results_text: 'Oops, nothing found!' },
        //        '.chsnProduct-width': { width: "100%" }
        //    }
        //    for (var selector in config) {
        //        $(selector).chosen(config[selector]);
        //    }
        //}

        //function BranchValuewiseledger() {

        //}

        function OnGetRowValuesCallback(values) {
            alert(values);
        }

        $(document).ready(function () {
            $("#ListBoxBranches").chosen().change(function () {
                var Ids = $(this).val();
                $('#<%=hdnSelectedBranches.ClientID %>').val(Ids);
                $('#MandatoryActivityType').attr('style', 'display:none');
            })
        })

        function DateAll(obj) {

            if (obj == 'all') {
                //document.getElementById("lblFromDate").innerHTML = 'As On Date'
                //document.getElementById("dvtodate").style.display = "none";
                //document.getElementById("ASPxFromDate").style.visibility = "visible";

                document.getElementById("lblToDate").innerHTML = 'As On Date :'
                document.getElementById("dvFromdate").style.display = "none";
                document.getElementById("ASPxToDate").style.visibility = "visible";
                AsonWise = true;
            }
            else {
                //document.getElementById("lblFromDate").style.visibility = "visible";
                //document.getElementById("lblFromDate").innerHTML = 'From Date'
                //document.getElementById("dvtodate").style.display = "block";
                //document.getElementById("ASPxFromDate").style.visibility = "visible";

                document.getElementById("lblToDate").style.visibility = "visible";
                document.getElementById("lblToDate").innerHTML = 'To Date :'
                document.getElementById("dvFromdate").style.display = "block";
                document.getElementById("ASPxToDate").style.visibility = "visible";
                AsonWise = false;
            }
        }

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
        function CallbackPanelEndCall() {
          <%--Rev Subhra 20-12-2018   0017670--%>
            document.getElementById('<%=CompBranch.ClientID %>').innerHTML = cCallbackPanel.cpBranchNames
            //End of Subhra
            Grid.Refresh();
        }

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

        function CloseGridQuotationLookup() {
            gridquotationLookup.ConfirmCurrentSelection();
            gridquotationLookup.HideDropDown();
            gridquotationLookup.Focus();
        }

        function CloseGridQuotationLookupbranch() {
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
                var index = Grid.GetFocusedRowIndex();
                if (AsonWise == false) {
                    ason = 'N';
                }
                else {
                    ason = 'Y';
                }
                $("#ddlExport3").val(0);
                var index = Grid.GetFocusedRowIndex();
                //var ledger = Grid.GetRowKey(index);
                var ledger = Grid.GetRow(index).children[8].innerHTML;
                var branchid = $('#ddlbranchHO').val();
                $("#hfIsProfnlossDetails").val("Y");
                var ledgertype = Grid.GetRow(index).children[7].innerHTML;

                //cCallbackPanelDetail.PerformCallback(ledger + "~" + ason + "~" + branchid + "~" + ledgertype);
                //cpopup2ndLevel.Show();
                if (ledger.trim() == "0") {
                    jAlert('Zooming from Total is not possible.');
                    cpopup2ndLevel.Hide();
                }
                else {
                    cCallbackPanelDetail.PerformCallback(ledger + "~" + ason + "~" + branchid + "~" + ledgertype);
                    cpopup2ndLevel.Show();
                }
            }
        }

        function OnRowClick(e) {
            var index = Grid.GetFocusedRowIndex();
            if (AsonWise == false) {
                ason = 'N';
            }
            else {
                ason = 'Y';
            }
            var index = Grid.GetFocusedRowIndex();
            var ledger = Grid.GetRow(index).children[0].innerHTML;
            var branchid = $('#ddlbranchHO').val();
            cCallbackPanelDetail.PerformCallback(ledger + "~" + ason + "~" + branchid);
            cpopup2ndLevel.Show();

        }

        function closePopup(s, e) {
            e.cancel = false;
            Grid.Focus();
            $("#drdExport").val(0);
        }

        //function EndShowGridDetails2Level() {
        //    cShowGridDetails2Level.Focus();
        //    cShowGridDetails2Level.SetFocusedRowIndex(0);
        //    ctxtLedger2ndLevel.SetText(cShowGridDetails2Level.cpLedger);

        //    if (document.getElementById('radAsDate').checked) {
        //        $("#lblFromDate2ndLevel")[0].innerHTML = "As on : " + cShowGridDetails2Level.cpFromDate;
        //        $("#lblToDate2ndLevel")[0].innerHTML = "";
        //    }
        //    else if (document.getElementById('radPeriod').checked) {
        //        $("#lblFromDate2ndLevel")[0].innerHTML = "From " + cShowGridDetails2Level.cpFromDate;
        //        $("#lblToDate2ndLevel")[0].innerHTML = " To " + cShowGridDetails2Level.cpToDate;
        //    }

        //    if (cShowGridDetails2Level.cpLedgerType != "FOR BRANCH") {
        //        $("#Label3")[0].innerHTML = "Ledger :";
        //    }
        //    else {
        //        $("#Label3")[0].innerHTML = "Branch :";
        //    }
        //}



        function CallbackPanelDetailEndCall() {
            cShowGridDetails2Level.Refresh();
            cShowGridDetails2Level.Focus();
            cShowGridDetails2Level.SetFocusedRowIndex(0);
            ctxtLedger2ndLevel.SetText(cCallbackPanelDetail.cpLedger);

            if (document.getElementById('radAsDate').checked) {
                $("#lblFromDate2ndLevel")[0].innerHTML = "As on : " + cCallbackPanelDetail.cpFromDate;
                $("#lblToDate2ndLevel")[0].innerHTML = "";
            }
            else if (document.getElementById('radPeriod').checked) {
                $("#lblFromDate2ndLevel")[0].innerHTML = "From " + cCallbackPanelDetail.cpFromDate;
                $("#lblToDate2ndLevel")[0].innerHTML = " To " + cCallbackPanelDetail.cpToDate;
            }

            if (cShowGridDetails2Level.cCallbackPanelDetail != "FOR BRANCH") {
                $("#Label3")[0].innerHTML = "Ledger :";
            }
            else {
                $("#Label3")[0].innerHTML = "Branch :";
            }
            if (cCallbackPanelDetail.cpBlankLedger == "0") {
                ctxtLedger2ndLevel.SetText('');
            }
        }
        
    </script>
    <%--end 2nd level popup--%>

    <%--end 3rd level popup--%>
    <script type="text/javascript">
        function OnWaitingGridKeyPress2(e) {
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
    </style>

    <style>
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

        /*Rev 1.0*/
        .outer-div-main {
            background: #ffffff;
            padding: 10px;
            border-radius: 10px;
            box-shadow: 1px 1px 10px #11111154;
        }

        /*.form_main {
            overflow: hidden;
        }*/

        label , .mylabel1, .clsTo
        {
            color: #141414 !important;
            font-size: 14px;
                font-weight: 500 !important;
                margin-bottom: 0 !important;
        }

        select
        {
            height: 30px !important;
            border-radius: 4px;
            -webkit-appearance: none;
            position: relative;
            z-index: 1;
            background-color: transparent;
            padding-left: 10px !important;
        }

        .dxeButtonEditSys.dxeButtonEdit_PlasticBlue
        {
            height: 30px;
            border-radius: 4px;
        }

        .dxeButtonEditButton_PlasticBlue
        {
            background: #094e8c !important;
            border-radius: 4px !important;
            padding: 0 4px !important;
        }

        .calendar-icon {
            position: absolute;
            bottom: 6px;
            right: 20px;
            z-index: 0;
            cursor: pointer;
        }

        #ASPxFromDate , #ASPxToDate , #ASPxASondate
        {
            position: relative;
            z-index: 1;
            background: transparent;
        }

        #ASPxFromDate_B-1 , #ASPxToDate_B-1 , #ASPxASondate_B-1
        {
            background: transparent !important;
            border: none;
            width: 30px;
            padding: 10px !important;
        }

        #ASPxFromDate_B-1 #ASPxFromDate_B-1Img , #ASPxToDate_B-1 #ASPxToDate_B-1Img , #ASPxASondate_B-1 #ASPxASondate_B-1Img
        {
            display: none;
        }

        .dxtcLite_PlasticBlue > .dxtc-stripContainer .dxtc-activeTab, .dxgvFooter_PlasticBlue
        {
            background: #1b5ea4 !important;
        }

        .simple-select::after {
            /*content: '<';*/
            content: url(../../../assests/images/left-arw.png);
            position: absolute;
            top: 26px;
            right: 13px;
            font-size: 16px;
            transform: rotate(269deg);
            font-weight: 500;
            background: #094e8c;
            color: #fff;
            height: 18px;
            display: block;
            width: 26px;
            /* padding: 10px 0; */
            border-radius: 4px;
            text-align: center;
            line-height: 19px;
            z-index: 0;
        }
        .simple-select {
            position: relative;
        }
        select.btn
        {
            padding-right: 10px !important;
        }

        .panel-group .panel
        {
            box-shadow: 1px 1px 8px #1111113b;
            border-radius: 8px;
        }

        .dxpLite_PlasticBlue .dxp-current
        {
            background-color: #1b5ea4;
            padding: 3px 5px;
            border-radius: 2px;
        }

        #accordion {
            margin-bottom: 20px;
            margin-top: 10px;
        }

        .dxgvHeader_PlasticBlue {
    background: #1b5ea4 !important;
    color: #fff !important;
}
        #ShowGrid
        {
            margin-top: 10px;
        }

        .pt-25{
                padding-top: 25px !important;
        }

        .styled-checkbox {
        position: absolute;
        opacity: 0;
        z-index: 1;
    }

        .styled-checkbox + label {
            position: relative;
            /*cursor: pointer;*/
            padding: 0;
            margin-bottom: 0 !important;
        }

            .styled-checkbox + label:before {
                content: "";
                margin-right: 6px;
                display: inline-block;
                vertical-align: text-top;
                width: 16px;
                height: 16px;
                /*background: #d7d7d7;*/
                margin-top: 2px;
                border-radius: 2px;
                border: 1px solid #c5c5c5;
            }

        .styled-checkbox:hover + label:before {
            background: #094e8c;
        }


        .styled-checkbox:checked + label:before {
            background: #094e8c;
        }

        .styled-checkbox:disabled + label {
            color: #b8b8b8;
            cursor: auto;
        }

            .styled-checkbox:disabled + label:before {
                box-shadow: none;
                background: #ddd;
            }

        .styled-checkbox:checked + label:after {
            content: "";
            position: absolute;
            left: 3px;
            top: 9px;
            background: white;
            width: 2px;
            height: 2px;
            box-shadow: 2px 0 0 white, 4px 0 0 white, 4px -2px 0 white, 4px -4px 0 white, 4px -6px 0 white, 4px -8px 0 white;
            transform: rotate(45deg);
        }

        .dxgvEditFormDisplayRow_PlasticBlue td.dxgv, .dxgvDataRow_PlasticBlue td.dxgv, .dxgvDataRowAlt_PlasticBlue td.dxgv, .dxgvSelectedRow_PlasticBlue td.dxgv, .dxgvFocusedRow_PlasticBlue td.dxgv
        {
            padding: 6px 6px 6px !important;
        }

        #lookupCardBank_DDD_PW-1
        {
                left: -182px !important;
        }
        .plhead a>i
        {
                top: 9px;
        }

        .clsTo
        {
            display: flex;
    align-items: flex-start;
        }

        input[type="radio"], input[type="checkbox"]
        {
            margin-right: 5px;
        }
        .dxeCalendarDay_PlasticBlue
        {
                padding: 6px 6px;
        }

        .modal-dialog
        {
            width: 50%;
        }

        .modal-header
        {
            padding: 4px 4px 4px 10px;
            background: #094e8c !important;
        }

        .TableMain100 #ShowGrid
        {
            max-width: 98% !important;
        }

        .btn-info
        {
                background-color: #1da8d1 !important;
                background-image: none;
        }

        /*#lookup_project
        {
            max-width: 100% !important;
        }*/
        /*Rev end 1.0*/
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            if ($('body').hasClass('mini-navbar')) {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 90;
                Grid.SetWidth(cntWidth);
            } else {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 220;
                Grid.SetWidth(cntWidth);
            }

            $('.navbar-minimalize').click(function () {
                if ($('body').hasClass('mini-navbar')) {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 220;
                    Grid.SetWidth(cntWidth);
                } else {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 90;
                    Grid.SetWidth(cntWidth);
                }

            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


    <div class="panel-heading">
       <%-- <div class="panel-title">
            <h3>Profit & Loss Statement (Group wise) </h3>
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
                        <asp:Label ID="CompName" runat="Server" Text="" Width="470px" ></asp:Label>
                    </div>
                    <%--Rev Subhra 20-12-2018   0017670--%>
                    <div>
                        <asp:Label ID="CompBranch" runat="Server" Text="" Width="470px" ></asp:Label>
                    </div>
                    <%--End of Rev--%>
                    <div>
                        <asp:Label ID="CompAdd" runat="Server" Text="" Width="470px" ></asp:Label>
                    </div>
                    <div>
                        <asp:Label ID="CompOth" runat="Server" Text="" Width="470px" ></asp:Label>
                    </div>
                    <div>
                        <asp:Label ID="CompPh" runat="Server" Text="" Width="470px" ></asp:Label>
                    </div>       
                    <div>
                        <asp:Label ID="CompAccPrd" runat="Server" Text="" Width="470px" ></asp:Label>
                    </div>
                    <div>
                        <asp:Label ID="DateRange" runat="Server" Text="" Width="470px" ></asp:Label>
                    </div>
              </div>
            </div>
          </div>
        </div>
    </div>   
    <%--Rev 1.0: "outer-div-main" class add: --%>
        <div class="outer-div-main">
    <div class="form_main">
        <asp:HiddenField runat="server" ID="hdndaily" />
        <asp:HiddenField runat="server" ID="hdtid" />
        <div class="row">
            <div class="col-md-2" style="padding-top: 23px;color: #141414; font-weight: bold;">
                <table class="padtbl">
                    <tr >
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
                    </tr>
                </table>
            </div>
            <div class="col-md-2" id="dvFromdate">
                <label style="color: #b5285f; font-weight: bold;" class="clsFrom">
                    <asp:Label ID="lblFromDate" runat="Server" Text="From Date : " CssClass="mylabel1"
                        Width="92px" Font-Bold="True"></asp:Label>
                </label>
                <dxe:ASPxDateEdit ID="ASPxFromDate" runat="server" EditFormat="custom" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy"
                    UseMaskBehavior="True" Width="100%" ClientInstanceName="cxdeFromDate">
                    <buttonstyle width="13px">
                        </buttonstyle>
                    <%--  <ClientSideEvents DateChanged="function(s,e){DateChangeForFrom();}" />--%>
                </dxe:ASPxDateEdit>
                <%--Rev 1.0--%>
                <img src="/assests/images/calendar-icon.png" class="calendar-icon"/>
                <%--Rev end 1.0--%>
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
                    <%--   <ClientSideEvents DateChanged="function(s,e){DateChangeForTo();}" />--%>
                </dxe:ASPxDateEdit>
                <%--Rev 1.0--%>
                <img src="/assests/images/calendar-icon.png" class="calendar-icon"/>
                <%--Rev end 1.0--%>
            </div>

            <%--Rev 1.0--%>
            <%--<div class="col-md-2">--%>
            <div class="col-md-2 simple-select">
                <%--Rev end 1.0--%>
                <div style="color: #b5285f; font-weight: bold;" class="clsTo">
                    <label style="color: #b5285f; font-weight: bold;" class="clsTo">
                        <asp:Label ID="Label2" runat="Server" Text="Head Branch : " CssClass="mylabel1"
                            Width="92px" Font-Bold="True"></asp:Label>
                    </label>
                </div>
                <asp:DropDownList ID="ddlbranchHO" runat="server" Width="100%"></asp:DropDownList>
            </div>


            <div class="col-md-2 branch-selection-box">
                <label style="color: #b5285f; font-weight: bold;" class="clsTo">
                    <asp:Label ID="Label1" runat="Server" Text="Branch : " CssClass="mylabel1"
                        Width="92px" Font-Bold="True"></asp:Label>
                </label>
                <%--<asp:ListBox ID="ListBoxBranches" Visible="false" runat="server" SelectionMode="Multiple" Font-Size="12px" Height="90px" Width="100%" CssClass="mb0 chsnProduct  hide" data-placeholder="--- ALL ---"></asp:ListBox>--%>
                <asp:HiddenField ID="hdnActivityType" runat="server" />
                <asp:HiddenField ID="hdnActivityTypeText" runat="server" />
                <span id="MandatoryActivityType" style="display: none" class="validclass">
                    <img id="3gridHistory_DXPEForm_efnew_DXEFL_DXEditor1112_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></span>
                <asp:HiddenField ID="hdnSelectedBranches" runat="server" />

                <dxe:ASPxCallbackPanel runat="server" ID="ASPxCallbackPanel1" ClientInstanceName="cProductbranchPanel" OnCallback="Componentbranch_Callback">
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
                                                           <%-- <div class="hide">--%>
                                                                <dxe:ASPxButton ID="ASPxButton3" runat="server" AutoPostBack="false" Text="Select All" ClientSideEvents-Click="selectAll_Branch" />
                                                           <%-- </div>--%>
                                                            <dxe:ASPxButton ID="ASPxButton4" runat="server" AutoPostBack="false" Text="Deselect All" ClientSideEvents-Click="unselectAll_Branch" />                                                            
                                                            <dxe:ASPxButton ID="Close" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseGridQuotationLookupbranch" UseSubmitBehavior="False" />
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
                                    <%--<ClientSideEvents ValueChanged="BranchValuewiseledger" />--%>
                                </dxe:ASPxGridLookup>
                            </dxe:PanelContent>
                        </panelcollection>

                </dxe:ASPxCallbackPanel>
            </div>
            <div class="col-md-2">
                <div style="color: #b5285f; font-weight: bold;" class="clsTo">
                    <label style="color: #b5285f; font-weight: bold;" class="clsTo">
                        <asp:Label ID="Label4" runat="Server" Text="Layout : " CssClass="mylabel1"
                            Width="92px" Font-Bold="True"></asp:Label>
                    </label>
                </div>
                <asp:DropDownList ID="ddlLayout" runat="server" Width="100%"></asp:DropDownList>
            </div>
            <div class="clear"></div>
            <%--Rev 1.0--%>
            <%--<div class="col-md-2" style="padding-top: 15px;color: #b5285f; font-weight: bold;">--%>
            <div class="col-md-2" style="padding-top: 15px;color: #141414; font-weight: bold;">
                <%--Rev end 1.0--%>
                <div style="padding-right: 10px; vertical-align: middle; padding-top: 6px">
                    <%--<asp:CheckBox ID="chkZero" runat="server" Checked="false" Text="Show Zero Balance Account"/>--%>
                     <dxe:ASPxCheckBox runat="server" ID="chkZero" Checked="false" Text="Show Zero Balance Account" >
                     </dxe:ASPxCheckBox>
                </div>
            </div>

            <%--<div class="col-md-2" style="padding-top: 16px">
                <div style="padding-right: 10px; vertical-align: middle; padding-top: 6px">
                    <asp:CheckBox ID="chkPL" runat="server" Text="Consider Closing Stock"/>
                </div>
            </div>--%>
            <%--Rev 1.0--%>
            <%--<div class="col-md-2" style="margin-top:15px;color: #b5285f; font-weight: bold;">--%>
            <div class="col-md-2" style="margin-top:20px;color: #b5285f; font-weight: bold;">
                <%--Rev end 1.0--%>
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
            <%--Rev 1.0--%>
            <%--<div class="col-md-3" style="margin-top:15px;color: #b5285f; font-weight: bold;">--%>
            <div class="col-md-3" style="margin-top:24px;color: #b5285f; font-weight: bold;">
                <%--Rev end 1.0--%>
                <dxe:ASPxCheckBox runat="server" ID="chkOWMSTVT" Checked="false" ClientEnabled="false" Text="Override Product Valuation Technique in Master" ClientInstanceName="CchkOWMSTVT">
                </dxe:ASPxCheckBox>
            </div> 
            <%--Rev 1.0--%>
            <%--<div class="col-md-3" style="margin-top:15px;color: #b5285f; font-weight: bold;">--%>
            <div class="col-md-3" style="margin-top:24px;color: #b5285f; font-weight: bold;">
                <%--Rev end 1.0--%>
                <dxe:ASPxCheckBox runat="server" ID="chkConsLandCost" Checked="false" ClientEnabled="false" Text="Consider Landed Cost" ClientInstanceName="CchkConsLandCost">
                </dxe:ASPxCheckBox>
             </div>
            <div class="clear"></div>
            <%--Rev 1.0--%>
            <%--<div class="col-md-2" style="margin-top:8px;color: #b5285f; font-weight: bold;">--%>
            <div class="col-md-2" style="margin-top:15px;color: #b5285f; font-weight: bold;">
                <%--Rev end 1.0--%>
                <dxe:ASPxCheckBox runat="server" ID="chkConsOverHeadCost" Checked="false" ClientEnabled="false" Text="Consider Overhead Cost" ClientInstanceName="CchkConsOverHeadCost">
                </dxe:ASPxCheckBox>
            </div>
            <%--Rev 1.0--%>
            <%--<div class="col-md-2" style="padding-top: 8px">--%>
            <div class="col-md-2" style="padding-top: 14px">
                <%--Rev end 1.0--%>
                <button id="btnShow" class="btn btn-success" type="button" onclick="btn_ShowRecordsClick(this);">Show</button>
                <%-- <% if (rights.CanExport)
                       { %>--%>

                <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-sm btn-primary"
                    OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true" OnChange="if(!AvailableExportOption()){return false;}">
                    <asp:ListItem Value="0">Export to</asp:ListItem>
                    <asp:ListItem Value="1">XLSX</asp:ListItem>
                    <asp:ListItem Value="2">PDF</asp:ListItem>
                    <asp:ListItem Value="3">CSV</asp:ListItem>
                    <asp:ListItem Value="4">RTF</asp:ListItem>

                </asp:DropDownList>
                <%-- <% } %>--%>
            </div>
            <div class="clear"></div>
            <div class="col-md-2"></div>
        </div>
        <table class="pull-left">
            <tr>
                <td></td>
                <td style="width: 254px"></td>
                <asp:HiddenField ID="HiddenField1" runat="server" />
                <span id="MandatoryCustomerType" style="display: none" class="validclass">
                    <img id="3gridHistory_DXPEForm_efnew_DXEFL_DXEditor1112_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></span>
                </td>
            </tr>            
        </table>

        <table class="TableMain100">
            <tr>
                <td colspan="2">
                    <div onkeypress="OnWaitingGridKeyPress(event)">
                        <dxe:ASPxGridView runat="server" ID="ShowGrid" ClientInstanceName="Grid" Width="100%" EnableRowsCache="false" AutoGenerateColumns="False" KeyboardSupport="true" KeyFieldName="SLNO"
                             OnDataBound="ShowGrid_DataBound"
                            OnSummaryDisplayText="ShowGrid_SummaryDisplayText" OnCustomSummaryCalculate="ShowGrid_CustomSummaryCalculate" OnHtmlDataCellPrepared="ShowGrid_HtmlDataCellPrepared" 
                            SettingsBehavior-AllowFocusedRow="true" SettingsBehavior-AllowSelectSingleRowOnly="true" Settings-HorizontalScrollBarMode="Visible"  DataSourceID="GenerateEntityServerModeDataSource">
                            <columns>   
                               <%--  OnCustomCallback="Grid_CustomCallback"    OnDataBinding="ShowGrid_DataBinding"    --%>                    
                                <dxe:GridViewDataTextColumn FieldName="ORDID" Caption ="Sl" Width="0" VisibleIndex="0" GroupIndex="0">
                                </dxe:GridViewDataTextColumn>
                             
                                <dxe:GridViewDataTextColumn FieldName="TYPEX" Caption ="Group Head" Width="150" VisibleIndex="1" GroupIndex="0">
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="LAYOUTDETAILS_NAME" Caption="Group Name" Width="55%" VisibleIndex="2" GroupIndex="1">
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="MAINACCOUNT_NAME" Caption="Ledger Name" Width="55%" VisibleIndex="3" >
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="OPENING" Caption="Opening" Width="15%" VisibleIndex="4" Settings-AllowAutoFilter="False">                                    
                                    <PropertiesTextEdit Style-HorizontalAlign="Right" DisplayFormatString="0.00"></PropertiesTextEdit>
                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="PERIOD" Caption="Period Balance" Width="15%" VisibleIndex="5" Settings-AllowAutoFilter="False">                                    
                                    <PropertiesTextEdit Style-HorizontalAlign="Right" DisplayFormatString="0.00"></PropertiesTextEdit>
                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="YTD" Caption="YTD Amount" Width="15%" VisibleIndex="6" Settings-AllowAutoFilter="False">                                    
                                    <PropertiesTextEdit Style-HorizontalAlign="Right" DisplayFormatString="0.00"></PropertiesTextEdit>
                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="TYPEX" Caption="HIDETYPEX" Width="0" VisibleIndex="7" >
                                <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>

                                 <dxe:GridViewDataTextColumn FieldName="MAINACCOUNT_REFERENCEID" Caption="MAINACCOUNT_REFERENCEID" Width="0%" VisibleIndex="8" />
                            </columns>
                            <settingsbehavior confirmdelete="true" enablecustomizationwindow="true" enablerowhottrack="true" />
                            <settings showfooter="true" showgrouppanel="false" showgroupfooter="VisibleIfExpanded" />
                            <settingsediting mode="EditForm" />
                            <settingscontextmenu enabled="true" />
                            <settingsbehavior autoexpandallgroups="true" columnresizemode="Control" />
                            <settings showgrouppanel="false" showstatusbar="Visible" showfilterrow="true" />
                            <settingssearchpanel visible="False" />
                            <%-- <SettingsPager PageSize="10">
                                <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
                            </SettingsPager>--%>
                            <settingspager mode="ShowAllRecords"></settingspager>
                            <settings showfilterrow="True" showfilterrowmenu="true" showstatusbar="Visible" usefixedtablelayout="true" />

                            <%--<totalsummary>
                                <dxe:ASPxSummaryItem FieldName="OPENING" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="PERIOD" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="YTD" SummaryType="Sum" />
                            </totalsummary>--%>
                            <clientsideevents endcallback="Callback2_EndCallback" />
                        </dxe:ASPxGridView>
                        <dx:LinqServerModeDataSource ID="GenerateEntityServerModeDataSource" runat="server" OnSelecting="GenerateEntityServerModeDataSource_Selecting"
                        ContextTypeName="ReportSourceDataContext" TableName="PROFITANDLOSSGROUPWISE_REPORT" />
                    </div>
                </td>
            </tr>
        </table>

        <div class="text-center" style="display: none;">
            <label style="color: #b5285f; font-weight: bold;" class="clsFrom">
                <%--  <asp:Label ID="lbldiffcalculationText" runat="Server" Text="" CssClass="mylabel1"
                        Width="92px" Font-Bold="True" ForeColor="#cc0000"></asp:Label>--%>
                <%--<asp:Label ID="lbldiffcalculation" runat="Server" Text="" CssClass="mylabel1"
                        Width="92px" Font-Bold="True" ForeColor="#cc0000"></asp:Label>--%>
                <dxe:ASPxTextBox ID="txtdiffcalculation" ClientInstanceName="ctxtdiffcalculation" runat="server" ReadOnly="true" Width="50px"></dxe:ASPxTextBox>
                <dxe:ASPxTextBox ID="txtdiffcalculationText" ClientInstanceName="ctxtdiffcalculationText" runat="server" ReadOnly="true" Width="100px"></dxe:ASPxTextBox>

            </label>
        </div>
    </div>
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
                                    <asp:ListItem Value="1">XLSX</asp:ListItem>
                                    <asp:ListItem Value="2">PDF</asp:ListItem>
                                    <asp:ListItem Value="3">CSV</asp:ListItem>
                                    <asp:ListItem Value="4">RTF</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div class="clearfix" onkeypress="OnWaitingGridKeyPress2(event)">
                        <%--Rev Subhra  17-12-2018  0019227--%> 
                            <%--  <dxe:ASPxGridView runat="server" ID="ShowGridDetails2Level" ClientInstanceName="cShowGridDetails2Level" KeyFieldName="DOC_ID" Width="100%" EnableRowsCache="false" AutoGenerateColumns="False"
                              KeyboardSupport="true" Settings-HorizontalScrollBarMode="Visible"
                            OnSummaryDisplayText="ShowGridDetails2Level_SummaryDisplayText" DataSourceID="GenerateEntityServerModeDetailsDataSource" >--%>
                          <dxe:ASPxGridView runat="server" ID="ShowGridDetails2Level" ClientInstanceName="cShowGridDetails2Level" KeyFieldName="SEQ" Width="100%" EnableRowsCache="false" AutoGenerateColumns="False"
                              KeyboardSupport="true" Settings-HorizontalScrollBarMode="Visible"
                            OnSummaryDisplayText="ShowGridDetails2Level_SummaryDisplayText" DataSourceID="GenerateEntityServerModeDetailsDataSource" >
                        <%--End of Rev--%>


                            <Columns>
                              <%-- OnCustomCallback="ShowGridDetails2Level_CustomCallback" OnDataBinding="ShowGridDetails2Level_DataBinding"--%>

                                <dxe:GridViewDataTextColumn FieldName="BRANCH_DESC" Caption="Unit" Width="30%" VisibleIndex="1" Settings-AllowAutoFilter="False"/>
                                <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="DOC_NO" Caption="Document No." Width="30%">
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
                                 <dxe:ASPxSummaryItem FieldName="CL_DR_AMT" SummaryType="Sum" />
                                 <dxe:ASPxSummaryItem FieldName="CL_CR_AMT" SummaryType="Sum" />
                            </TotalSummary>
                        <%--    <ClientSideEvents EndCallback="EndShowGridDetails2Level" />--%>
                        </dxe:ASPxGridView>
                         <dx:LinqServerModeDataSource ID="GenerateEntityServerModeDetailsDataSource" runat="server" OnSelecting="GenerateEntityServerModeDetailsDataSource_Selecting"
                        ContextTypeName="ReportSourceDataContext" TableName="PROFITANDLOSSDETAILSTATEMENT_REPORT" />
                    </div>
                </dxe:PopupControlContentControl>
            </contentcollection>

    </dxe:ASPxPopupControl>
    
    </dxe:ASPxPopupControl>
        <dxe:ASPxPopupControl ID="ASPXPopupControl2" runat="server"
        CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="popupdocument" Height="500px"
        Width="1200px" HeaderText="Details" Modal="true" AllowResize="true">
        <contentcollection>
                <dxe:PopupControlContentControl runat="server">
                </dxe:PopupControlContentControl>
            </contentcollection>
    </dxe:ASPxPopupControl>

    <%--<asp:SqlDataSource ID="SalesDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:crmConnectionString %>"
        SelectCommand="sp_DailySales_Report" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
    <asp:SqlDataSource ID="EntityDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:crmConnectionString %>"
        SelectCommand="sp_DailySales_Report" SelectCommandType="StoredProcedure"></asp:SqlDataSource>--%>
    <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
    </dxe:ASPxGridViewExporter>

    <dxe:ASPxGridViewExporter ID="exporterDetails" runat="server" Landscape="true" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
    </dxe:ASPxGridViewExporter>

    <dxe:ASPxCallbackPanel runat="server" ID="CallbackPanel" ClientInstanceName="cCallbackPanel" OnCallback="CallbackPanel_Callback">
        <PanelCollection>
            <dxe:PanelContent runat="server">
                <asp:HiddenField ID="hfIsProfnloss" runat="server" />
            </dxe:PanelContent>
        </PanelCollection>
        <ClientSideEvents EndCallback="CallbackPanelEndCall" />
    </dxe:ASPxCallbackPanel>

     <dxe:ASPxCallbackPanel runat="server" ID="CallbackPanelDetail" ClientInstanceName="cCallbackPanelDetail" OnCallback="CallbackPanelDetail_Callback">
        <PanelCollection>
            <dxe:PanelContent runat="server">
                <asp:HiddenField ID="hfIsProfnlossDetails" runat="server" />
            </dxe:PanelContent>
        </PanelCollection>
        <ClientSideEvents EndCallback="CallbackPanelDetailEndCall" />
    </dxe:ASPxCallbackPanel>



</asp:Content>