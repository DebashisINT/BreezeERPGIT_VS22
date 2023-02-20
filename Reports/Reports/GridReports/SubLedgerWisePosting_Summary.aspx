<%--====================================================== Revision History ============================================
Rev Number         DATE              VERSION          DEVELOPER           CHANGES
1.0                13-02-2023        2.0.36           Pallab              25575 : Report pages modification
====================================================== Revision History ================================================--%>

<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master" CodeBehind="SubLedgerWisePosting_Summary.aspx.cs" Inherits="Reports.Reports.GridReports.SubLedgerWisePosting_Summary" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
     Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
        <link href="CSS/SearchPopup.css" rel="stylesheet" />
        <script src="JS/SearchPopup.js"></script>
        <script src="JS/SearchMultiPopup.js"></script>

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
       .btnOkformultiselection {
        border-width: 1px;
        padding: 4px 10px;
        font-size: 13px !important;
        margin-right: 6px;
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

   <%--For Single Selection--%>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#LedgModel').on('shown.bs.modal', function () {
                $('#txtLedgerSearch').focus();
            })
        })
        function LedgerButnClick(s, e) {
            $('#LedgModel').modal('show');
        }

        function Ledger_KeyDown(s, e) {
            if (e.htmlEvent.key == "Enter") {
                $('#LedgModel').modal('show');
            }
        }

        function Ledgerkeydown(e) {
            var OtherDetails = {}

            if ($.trim($("#txtLedgerSearch").val()) == "" || $.trim($("#txtLedgerSearch").val()) == null) {
                return false;
            }
            OtherDetails.SearchKey = $("#txtLedgerSearch").val();
            OtherDetails.BranchID = $('#ddl_Branch').val();

            if (e.code == "Enter" || e.code == "NumpadEnter") {
                var HeaderCaption = [];
                HeaderCaption.push("Description");

                if ($("#txtLedgerSearch").val() != "") {
                    callonServer("Services/Master.asmx/GetLedger", OtherDetails, "LedgerTable", HeaderCaption, "ledgerIndex", "SetLedger");
                }
            }
            else if (e.code == "ArrowDown") {
                if ($("input[ledgerIndex=0]"))
                    $("input[ledgerIndex=0]").focus();
            }
        }

        function SetLedger(Id, Name) {
            var key = Id;
            if (key != null && key != '') {
                $('#LedgModel').modal('hide');
                ctxtLedger.SetText(Name);
                GetObjectID('hdnSelectedLedger').value = key;
            }
            else {
                ctxtLedger.SetText('');
                GetObjectID('hdnSelectedLedger').value = '';
            }
        }

        function ValueSelected(e, indexName) {
            if (e.code == "Enter" || e.code == "NumpadEnter") {
                var Id = e.target.parentElement.parentElement.cells[0].innerText;
                var name = e.target.parentElement.parentElement.cells[1].children[0].value;
                if (Id) {
                    SetLedger(Id, name);
                }
            }
            else if (e.code == "ArrowDown") {
                thisindex = parseFloat(e.target.getAttribute(indexName));
                thisindex++;
                if (thisindex < 10)
                    $("input[" + indexName + "=" + thisindex + "]").focus();
            }
            else if (e.code == "ArrowUp") {
                thisindex = parseFloat(e.target.getAttribute(indexName));
                thisindex--;
                if (thisindex > -1)
                    $("input[" + indexName + "=" + thisindex + "]").focus();
                else {
                    //if (indexName == "ProdIndex")
                    //    $('#txtProdSearch').focus();
                    //else if (indexName == "salesmanIndex")
                    //    ctxtCreditDays.Focus();
                    //else
                    $('#txtLedgerSearch').focus();
                }
            }
        }


    </Script>
    <%--For Single Selection--%>

     <%-- For multiselection when click on ok button--%>
    <script type="text/javascript">
        function SetSelectedValues(Id, Name, ArrName) {
            if (ArrName == 'SubLedgerSource') {
                var key = Id;
                if (key != null && key != '') {
                    $('#SubLedgModel').modal('hide');
                    ctxtSubLedger.SetText(Name);
                    GetObjectID('hdnSelectedSubLedger').value = key;
                }
                else {
                    ctxtSubLedger.SetText('');
                    GetObjectID('hdnSelectedSubLedger').value = '';
                }
            }

        }

    </script>
  <%-- For multiselection when click on ok button--%>
     <%-- For multiselection--%>
     <script type="text/javascript">
         $(document).ready(function () {
             $('#SubLedgModel').on('shown.bs.modal', function () {
                 $('#txtSubLedgerSearch').focus();
             })
         })
         var SubLedgArr = new Array();
         $(document).ready(function () {
             var SubLedgObj = new Object();
             SubLedgObj.Name = "SubLedgerSource";
             SubLedgObj.ArraySource = SubLedgArr;
             arrMultiPopup.push(SubLedgObj);
         })
         function SubLedgerButnClick(s, e) {
             $('#SubLedgModel').modal('show');
         }

         function SubLedger_KeyDown(s, e) {
             if (e.htmlEvent.key == "Enter") {
                 $('#SubLedgModel').modal('show');
             }
         }

         function SubLedgerkeydown(e) {
             var OtherDetails = {}

             if ($.trim($("#txtSubLedgerSearch").val()) == "" || $.trim($("#txtSubLedgerSearch").val()) == null) {
                 return false;
             }
             OtherDetails.SearchKey = $("#txtSubLedgerSearch").val();
             OtherDetails.LedgerID = hdnSelectedLedger.value;
             OtherDetails.AllSubLedg = 0;

             if (e.code == "Enter" || e.code == "NumpadEnter") {

                 var HeaderCaption = [];
                 //HeaderCaption.push("Customer Name");
                 //HeaderCaption.push("Contact No.");
                 //HeaderCaption.push("Alternate No.");
                 //HeaderCaption.push("Address");
                 HeaderCaption.push("Description");
                 HeaderCaption.push("Type");


                 if ($("#txtSubLedgerSearch").val() != "") {
                     callonServerM("Services/Master.asmx/GetSubLedger", OtherDetails, "SubLedgerTable", HeaderCaption, "dPropertyIndex", "SetSelectedValues", "SubLedgerSource");
                 }
             }
             else if (e.code == "ArrowDown") {
                 if ($("input[dPropertyIndex=0]"))
                     $("input[dPropertyIndex=0]").focus();
             }
         }

         function SetfocusOnseach(indexName) {
             if (indexName == "dPropertyIndex")
                 $('#txtSubLedgerSearch').focus();
             else
                 $('#txtSubLedgerSearch').focus();
         }
   </script>
      <%-- For multiselection--%>

    <script type="text/javascript">

        $(function () {
            //cProductbranchPanel.PerformCallback('BindComponentGrid' + '~' + 0);
            cProductbranchPanel.PerformCallback('BindComponentGrid' + '~' + $("#ddlbranchHO").val());
            function OnWaitingGridKeyPress(e) {
                if (e.code == "Enter") {

                }

            }

        });

        $(document).ready(function () {
            $("#ListBoxBranches").chosen().change(function () {
                var Ids = $(this).val();
                $('#<%=hdnSelectedBranches.ClientID %>').val(Ids);
                $('#MandatoryActivityType').attr('style', 'display:none');
            })

            $("#ddlbranchHO").change(function () {
                var Ids = $(this).val();
                $('#MandatoryActivityType').attr('style', 'display:none');
                $("#hdnSelectedBranches").val('');
                cProductbranchPanel.PerformCallback('BindComponentGrid' + '~' + $("#ddlbranchHO").val());
            })

        })


        function BranchValuewiseledger() {
            //cProductComponentPanel_ledger.PerformCallback('BindComponentGrid' + '~' + 0);
            //gridledgerLookup.SetValue(null);
        }

        function LedgerValuewiseSubledger() {
            //if (gridbranchLookup.GetValue() == null) {
            //    jAlert('Please select atleast one branch');
            //    gridledgerLookup.SetValue(null);
            //}
            //else {
            var key = gridledgerLookup.GetGridView().GetRowKey(gridledgerLookup.GetGridView().GetFocusedRowIndex());
            cEmployeetComponentPanel.PerformCallback('BindComponentGrid' + '~' + key);
            //}
        }

        function OnGetRowValuesCallback(values) {
            alert(values);
        }

        $(document).ready(function () {

            $("#ListBoxBranches").chosen().change(function () {
                var Ids = $(this).val();
                cProductComponentPanel_ledger.PerformCallback('BindComponentGrid' + '~' + Ids);
                $('#<%=hdnSelectedBranches.ClientID %>').val(Ids);
                $('#MandatoryActivityType').attr('style', 'display:none');

            })

        })


    </script>

    <script type="text/javascript">

        function cxdeToDate_OnChaged(s, e) {
            var data = "OnDateChanged";
            data += '~' + cxdeFromDate.GetDate();
            data += '~' + cxdeToDate.GetDate();
            //CallServer(data, "");
            // Grid.PerformCallback('');
        }

        function btn_ShowRecordsClick(e) {
            var BranchSelection = document.getElementById('hdnBranchSelection').value;
            var data = "OnDateChanged";
            $("#hfIsSubLedgSumFilter").val("Y");
            $("#drdExport").val(0);

            data += '~' + cxdeFromDate.GetDate();
            data += '~' + cxdeToDate.GetDate();

            var FromDate = (cxdeFromDate.GetValue() != null) ? cxdeFromDate.GetValue() : "";
            var ToDate = (cxdeToDate.GetValue() != null) ? cxdeToDate.GetValue() : "";
            var ledgerid = hdnSelectedLedger.value;
            var subledgerid = hdnSelectedSubLedger.value;

            FromDate = GetDateFormat(FromDate);
            ToDate = GetDateFormat(ToDate);
            document.getElementById('<%=DateRange.ClientID %>').innerHTML = "For the period: " + FromDate + " To " + ToDate;


            //if (ctxtSubLedger.GetValue() == null) {
            if (ctxtSubLedger.GetValue() == null & chkallsubledg.checked == false) {
                jAlert('Please select Sub Ledger for generate the report.');
            }
            else if (ctxtLedger.GetValue() == null & ctxtSubLedger.GetValue() == null & chkallsubledg.checked == true) {
                jAlert('Please select Ledger for All Sub Ledger to generate the report.');
            }
            else {
                //cCallbackPanel.PerformCallback(data + '~' + $("#ddlbranchHO").val() + '~' + ledgerid + '~' + subledgerid);
                if (BranchSelection == "Yes" && gridbranchLookup.GetValue() == null) {
                    jAlert('Please select atleast one branch for generate the report.');
                }
                else {
                    cCallbackPanel.PerformCallback(data + '~' + $("#ddlbranchHO").val() + '~' + ledgerid + '~' + subledgerid);
                }
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

        function SubLedgAll(obj) {
            if (obj == 'allsubledg') {
                if (chkallsubledg.checked == true) {
                    ctxtSubLedger.SetText('');
                    GetObjectID('hdnSelectedSubLedger').value = '';
                    document.getElementById("txtSubLedgerSearch").value = ""
                    ctxtSubLedger.SetEnabled(false);
                }
                else {
                    ctxtSubLedger.SetEnabled(true);
                }
            }
        }

        function OnGetRowValues(types) {
            var BranchSelection = document.getElementById('hdnBranchSelection').value;
            var data = "OnDateChanged";

            if (types[0] == "Sub Ledger") {
                if (gridledgerLookup.GetValue() == null) {
                    jAlert('Please select Ledger first', "Alert", function () {
                    });
                }
                else {
                    Grid.PerformCallback(data + '~' + types[0] + '~' + types[1] + '~' + $("#ddlbranchHO").val());
                }
            }
            if (gridledgerLookup.GetValue() == null && gridemployeeLookup.GetValue() == null) {
                jAlert('Please select at least one Ledger or Sub Ledger', "Alert", function () {
                });
            }
            else if (gridbranchLookup.GetValue() == null && gridledgerLookup.GetValue() != null && gridemployeeLookup.GetValue() == null) {
                jAlert('Please select at least one Branch', "Alert", function () {
                });
            }
            else {
                //Grid.PerformCallback(data + '~' + types[0] + '~' + types[1] + '~' + $("#ddlbranchHO").val());
                if (BranchSelection == "Yes" && gridbranchLookup.GetValue() == null) {
                    jAlert('Please select atleast one branch for generate the report.');
                }
                else {
                    Grid.PerformCallback(data + '~' + types[0] + '~' + types[1] + '~' + $("#ddlbranchHO").val());
                }
            }
        }


        function OnContextMenuItemClick(sender, args) {
            if (args.item.name == "ExportToPDF" || args.item.name == "ExportToXLS") {
                args.processOnServer = true;
                args.usePostBack = true;
            } else if (args.item.name == "SumSelected")
                args.processOnServer = true;
        }

        function CallbackPanelEndCall(s, e) {
             <%--Rev Subhra 24-12-2018   0017670--%>
            document.getElementById('<%=CompBranch.ClientID %>').innerHTML = cCallbackPanel.cpBranchNames
            //End of Subhra
            Grid.Refresh();
            $("#drdExport").val(0);
        }

        function OpenPOSDetails(Uniqueid, type, docno) {
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

            popupbudget.SetContentUrl(url);
            popupbudget.Show();

        }
        function BudgetAfterHide(s, e) {
            popupbudget.Hide();
        }

        function CloseGridemployeeeLookup() {
            gridemployeeLookup.ConfirmCurrentSelection();
            gridemployeeLookup.HideDropDown();
            gridemployeeLookup.Focus();
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

        function selectAll() {
            gridledgerLookup.gridView.SelectRows();
        }

        function unselectAll() {
            gridledgerLookup.gridView.UnselectRows();
        }

    </script>

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
                max-width: 98%  !important;
        }
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
        <%--<div class="panel-title">
            <h3>Ledger Posting</h3>
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
                    <%--Rev Subhra 24-12-2018   0017670--%>
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
            <%--Rev 1.0--%>
            <%--<div class="col-md-2">--%>
            <div class="col-md-2 simple-select">
                <%--Rev end 1.0--%>
                    <div style="color: #b5285f; font-weight: bold;" class="clsTo">
                        <asp:Label ID="Label3" runat="Server" Text="Head Branch : " CssClass="mylabel1"
                            Width="92px"></asp:Label>
                    </div>
                <div>
                    <asp:DropDownList ID="ddlbranchHO" runat="server" Width="100%"></asp:DropDownList>
                </div>
            </div>
            <div class="col-md-2 branch-selection-box">
                <div style="color: #b5285f; font-weight: bold;" class="clsTo">
                        <asp:Label ID="Label1" runat="Server" Text="Branch : " CssClass="mylabel1"
                            Width="92px"></asp:Label>
                    </div>
                <div>

                    <asp:HiddenField ID="hdnActivityType" runat="server" />
                    <asp:HiddenField ID="hdnActivityTypeText" runat="server" />
                    <asp:HiddenField ID="hdnSelectedBranches" runat="server" />
                    <dxe:ASPxCallbackPanel runat="server" ID="ASPxCallbackPanel1" ClientInstanceName="cProductbranchPanel" OnCallback="Componentbranch_Callback">
                        <PanelCollection>
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
                                                            <%--<div class="hide">--%>
                                                                <dxe:ASPxButton ID="ASPxButton3" runat="server" AutoPostBack="false" Text="Select All" ClientSideEvents-Click="selectAll_Branch" UseSubmitBehavior="False"/>
                                                            <%--</div>--%>
                                                            <dxe:ASPxButton ID="ASPxButton4" runat="server" AutoPostBack="false" Text="Deselect All" ClientSideEvents-Click="unselectAll_Branch" UseSubmitBehavior="False"/>                                                            
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
                                    <ClientSideEvents ValueChanged="BranchValuewiseledger" />
                                </dxe:ASPxGridLookup>
                            </dxe:PanelContent>
                        </PanelCollection>

                    </dxe:ASPxCallbackPanel>

                </div>
            </div>
            <div class="col-md-2">
                    <div style="color: #b5285f; font-weight: bold;" class="clsTo">
                        <asp:Label ID="Label2" runat="Server" Text="Ledger : " CssClass="mylabel1"
                            Width="92px"></asp:Label>
                    </div>
                <div>
                                     
                      <dxe:ASPxButtonEdit ID="txtLedger" runat="server" ReadOnly="true" ClientInstanceName="ctxtLedger" Width="100%" TabIndex="5">
                            <Buttons>
                                <dxe:EditButton>
                                </dxe:EditButton>
                            </Buttons>
                            <ClientSideEvents ButtonClick="function(s,e){LedgerButnClick();}" KeyDown="function(s,e){Ledger_KeyDown(s,e);}" />
                      </dxe:ASPxButtonEdit>
                    <asp:HiddenField ID="hdnSelectedLedger" runat="server" />

                    <span id="MandatoryLedgerType" style="display: none" class="validclass">
                        <img id="3gridHistory_DXPEForm_efnew_DXEFL_DXEditor1112_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></span>

                </div>
            </div>


            <div class="col-md-2">
                <div style="color: #b5285f; font-weight: bold;" class="clsTo">
                    <asp:Label ID="Label4" runat="Server" Text="Sub Ledger : " CssClass="mylabel1"
                        Width="92px"></asp:Label>
                </div>
                <div>
                      <dxe:ASPxButtonEdit ID="txtSubLedger" runat="server" ReadOnly="true" ClientInstanceName="ctxtSubLedger" Width="100%" TabIndex="5">
                            <Buttons>
                                <dxe:EditButton>
                                </dxe:EditButton>
                            </Buttons>
                            <ClientSideEvents ButtonClick="function(s,e){SubLedgerButnClick();}" KeyDown="function(s,e){SubLedger_KeyDown(s,e);}" />
                      </dxe:ASPxButtonEdit>
                    <asp:HiddenField ID="hdnSelectedSubLedger" runat="server" />

                </div>
            </div>
            <div class="col-md-1" style="padding:0;padding-top: 20px;">
                <asp:CheckBox runat="server" ID="chkallsubledg" Checked="false" Text="All Sub Ledger" />
            </div>
            <div class="clear"></div>
            <div class="col-md-2">
               
                    <div style="color: #b5285f; font-weight: bold;" class="clsFrom">
                        <asp:Label ID="lblFromDate" runat="Server" Text="From Date : " CssClass="mylabel1"
                            Width="92px"></asp:Label>
                    </div>
                
                <div>
                    <dxe:ASPxDateEdit ID="ASPxFromDate" runat="server" EditFormat="custom" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy"
                        UseMaskBehavior="True" Width="100%" ClientInstanceName="cxdeFromDate">
                        <ButtonStyle Width="13px">
                        </ButtonStyle>
                    </dxe:ASPxDateEdit>
                </div>
                <%--Rev 1.0--%>
                <img src="/assests/images/calendar-icon.png" class="calendar-icon"/>
                <%--Rev end 1.0--%>
            </div>
            <div class="col-md-2">
               
                    <div style="color: #b5285f; font-weight: bold;" class="clsTo">
                        <asp:Label ID="lblToDate" runat="Server" Text="To Date : " CssClass="mylabel1"
                            Width="92px"></asp:Label>
                    </div>              
                    <div>
                        <dxe:ASPxDateEdit ID="ASPxToDate" runat="server" EditFormat="custom" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy"
                            UseMaskBehavior="True" Width="100%" ClientInstanceName="cxdeToDate">
                            <ButtonStyle Width="13px">
                            </ButtonStyle>

                        </dxe:ASPxDateEdit>
                    </div>
                <%--Rev 1.0--%>
                <img src="/assests/images/calendar-icon.png" class="calendar-icon"/>
                <%--Rev end 1.0--%>
            </div>
            <%--<div class="clear"></div>--%>
            <%--<div class="col-md-2">--%>
                <div class="hide">
                    <div id="ckpar" style="padding-top: 7px;">
                        <asp:CheckBox runat="server" ID="chkparty" Checked="false" Text="Search by Party Inv. Date" />
                    </div>
                </div>
            <%--</div>--%>
            <div class="col-md-2" style="padding: 0; padding-top: 20px;">
                <div>
                    <button id="btnShow" class="btn btn-success" type="button" onclick="btn_ShowRecordsClick(this);">Show</button>
                    <%-- <% if (rights.CanExport)
                       { %>--%>
                    <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-sm btn-primary"
                        OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true" OnChange="if(!AvailableExportOption()){return false;}">
                        <asp:ListItem Value="0">Export to</asp:ListItem>
                        <asp:ListItem Value="1">PDF</asp:ListItem>
                        <asp:ListItem Value="2">XLSX</asp:ListItem>
                        <asp:ListItem Value="3">RTF</asp:ListItem>
                        <asp:ListItem Value="4">CSV</asp:ListItem>

                    </asp:DropDownList>
                    <%-- <% } %>--%>
                </div>
            </div>
        </div>
        <table class="pull-left">
           <tr>        
              
            </tr>
            <tr>            
            </tr>
        </table>

        <table class="TableMain100">
            <tr>
                <td colspan="2">
                    <div onkeypress="OnWaitingGridKeyPress(event)">
                        <%-- <div  id="divRowview">--%>
                        <dxe:ASPxGridView runat="server" ID="ShowGrid" ClientInstanceName="Grid" Width="100%" EnableRowsCache="false" AutoGenerateColumns="False" KeyboardSupport="true" KeyFieldName="SEQ"
                            DataSourceID="GenerateEntityServerModeDataSource" OnSummaryDisplayText="ShowGrid_SummaryDisplayText" Settings-HorizontalScrollBarMode="Visible">
                            <Columns>
                                <dxe:GridViewDataTextColumn FieldName="BRANCH_DESC" Caption="Unit" Width="250px" VisibleIndex="1" />
                                <dxe:GridViewDataTextColumn FieldName="LEDG_DESC" Caption="Ledger Description" VisibleIndex="2" Width="250px"/>
                                <dxe:GridViewDataTextColumn FieldName="SUBLEDGER" Caption="Sub Ledger Description" Width="350px" VisibleIndex="3" />
                                <dxe:GridViewDataTextColumn FieldName="OP_DEBIT" Caption="Opening Dr." Width="120px" VisibleIndex="4" PropertiesTextEdit-DisplayFormatString="0.00">
                                     <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="OP_CREDIT" Caption="Opening Cr." Width="120px" VisibleIndex="5" PropertiesTextEdit-DisplayFormatString="0.00" >
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="PR_DEBIT" Caption="Period Dr." Width="120px" VisibleIndex="6" PropertiesTextEdit-DisplayFormatString="0.00" >
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="PR_CREDIT" Caption="Period Cr." Width="120px" VisibleIndex="7" PropertiesTextEdit-DisplayFormatString="0.00" >
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>                              
                                <dxe:GridViewDataTextColumn FieldName="CL_DEBIT" Caption="Closing Dr." Width="120px" VisibleIndex="8" PropertiesTextEdit-DisplayFormatString="0.00" >
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>                              
                                <dxe:GridViewDataTextColumn FieldName="CL_CREDIT" Caption="Closing Cr." Width="120px" VisibleIndex="9" PropertiesTextEdit-DisplayFormatString="0.00" >
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>                              
                            </Columns>
                            <SettingsBehavior ConfirmDelete="true" EnableCustomizationWindow="true" EnableRowHotTrack="true" />
                            <Settings ShowFooter="true" ShowGroupPanel="true" ShowGroupFooter="VisibleIfExpanded" />
                            <SettingsEditing Mode="EditForm" />
                            <SettingsContextMenu Enabled="true" />
                            <SettingsBehavior AutoExpandAllGroups="true" ColumnResizeMode="Control" />
                            <Settings ShowGroupPanel="True" ShowStatusBar="Visible" ShowFilterRow="true" />
                            <SettingsSearchPanel Visible="False" />
                            <SettingsPager PageSize="10">
                                <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100" />
                            </SettingsPager>
                            <Settings ShowFilterRow="True" ShowFilterRowMenu="true" ShowStatusBar="Visible" UseFixedTableLayout="true" />

                            <TotalSummary>
                                <dxe:ASPxSummaryItem FieldName="SUBLEDGER" SummaryType="Custom" />
                                <dxe:ASPxSummaryItem FieldName="OP_DEBIT" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="OP_CREDIT" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="PR_DEBIT" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="PR_CREDIT" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="CL_DEBIT" SummaryType="Custom" />
                                <dxe:ASPxSummaryItem FieldName="CL_CREDIT" SummaryType="Custom" />
                            </TotalSummary>

                        </dxe:ASPxGridView>
                        <dx:LinqServerModeDataSource ID="GenerateEntityServerModeDataSource" runat="server" OnSelecting="GenerateEntityServerModeDataSource_Selecting"
                        ContextTypeName="ReportSourceDataContext" TableName="SUBLEDGERPOSTINGSUM_REPORT" ></dx:LinqServerModeDataSource>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    </div>
    
    <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
    </dxe:ASPxGridViewExporter>
    <dxe:ASPxPopupControl ID="ASPXPopupControl2" runat="server"
        CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="popupbudget" Height="500px"
        Width="1310px" HeaderText="Details" Modal="true" AllowResize="true" ResizingMode="Postponed">
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
            </dxe:PopupControlContentControl>
        </ContentCollection>

        <ClientSideEvents CloseUp="BudgetAfterHide" />
    </dxe:ASPxPopupControl>

       <!--Ledger Modal -->
    <div class="modal fade" id="LedgModel" role="dialog">
        <div class="modal-dialog">
            <!-- Ledger content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Ledger Search</h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="Ledgerkeydown(event)" id="txtLedgerSearch" autofocus width="100%" placeholder="Search By Ledger" />
                    <div id="LedgerTable">
                        <table border='1' width="100%" class="dynamicPopupTbl">
                            <tr class="HeaderStyle">
                                <th class="hide">id</th>
                                <th>Description</th>
                            </tr>
                        </table>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>
    <!--ledger Modal -->

    <!--Sub Ledger Modal -->
    <div class="modal fade" id="SubLedgModel" role="dialog">
        <div class="modal-dialog">
            <!-- Sub Ledger content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Sub Ledger Search</h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="SubLedgerkeydown(event)" id="txtSubLedgerSearch" autofocus width="100%" placeholder="Search By Ledger" />
                    <div id="SubLedgerTable">
                        <table border='1' width="100%" class="dynamicPopupTbl">
                            <tr class="HeaderStyle">
                                <th>Select</th>
                                <th class="hide">cnt_internalId</th>
                                <th>Description</th>
                                <th>Type</th>
                            </tr>
                        </table>
                    </div>
                </div>
                <div class="modal-footer">
                    <%--<button type="button" class="btn btn-default" data-dismiss="modal">Close</button>--%>
                     <button type="button" class="btnOkformultiselection btn-default"  onclick="DeSelectAll('SubLedgerSource')">Deselect All</button>
                    <button type="button" class="btnOkformultiselection btn-default" data-dismiss="modal" onclick="OKPopup('SubLedgerSource')">OK</button>
                </div>
            </div>
        </div>
    </div>
   <!--Sub ledger Modal -->

    <dxe:ASPxCallbackPanel runat="server" ID="CallbackPanel" ClientInstanceName="cCallbackPanel" OnCallback="CallbackPanel_Callback">
        <PanelCollection>
            <dxe:PanelContent runat="server">
                <asp:HiddenField ID="hfIsSubLedgSumFilter" runat="server" />
            </dxe:PanelContent>
        </PanelCollection>
    <ClientSideEvents EndCallback="CallbackPanelEndCall" />
    </dxe:ASPxCallbackPanel>

    <asp:HiddenField ID="hdnBranchSelection" runat="server" />
</asp:Content>
