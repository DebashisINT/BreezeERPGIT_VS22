<%--================================================== Revision History ====================================================================================================
Rev Number         DATE              VERSION          DEVELOPER           CHANGES
1.0                20-02-2023        2.0.36           Pallab              25575 : Report pages design modification
2.0                02-05-2023        2.0.38           Pallab              25999 : Party Ledger - Vendor module zoom popup upper part visible issue fix for small device
3.0                15-09-2023        2.0.38           Debashis            0026804 : Opening Breakup required in Party Ledger
====================================================== Revision History ========================================================================================================--%>

<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master" CodeBehind="PartyLedger_Vendor.aspx.cs" Inherits="Reports.Reports.GridReports.PartyLedger_Vendor" %>

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

        #ListBoxProjects{
            width: 200px;
        }

        .hide {
            display: none;
        }

        .dxtc-activeTab .dxtc-link {
            color: #fff !important;
        }
        /*rev Pallab*/
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
   
     <%-- For multiselection when click on ok button--%>
    <script type="text/javascript">
        function SetSelectedValues(Id, Name, ArrName) {
            if (ArrName == 'PartyLedgerSource') {
                var key = Id;
                if (key != null && key != '') {
                    $('#PartyLedgModel').modal('hide');
                    ctxtPartyLedger.SetText(Name);
                    GetObjectID('hdnSelectedPartyLedger').value = key;
                }
                else {
                    ctxtPartyLedger.SetText('');
                    GetObjectID('hdnSelectedPartyLedger').value = '';
                }
            }

        }

    </script>
  <%-- For multiselection when click on ok button--%>
     <%-- For multiselection--%>
     <script type="text/javascript">
         $(document).ready(function () {
             $('#PartyLedgModel').on('shown.bs.modal', function () {
                 $('#txtPartyLedgerSearch').focus();
             })
         })
         var PartyLedgArr = new Array();
         $(document).ready(function () {
             var PartyLedgObj = new Object();
             PartyLedgObj.Name = "PartyLedgerSource";
             PartyLedgObj.ArraySource = PartyLedgArr;
             arrMultiPopup.push(PartyLedgObj);
         })
         function PartyLedgerButnClick(s, e) {
             $('#PartyLedgModel').modal('show');
         }

         function PartyLedger_KeyDown(s, e) {
             if (e.htmlEvent.key == "Enter") {
                 $('#PartyLedgModel').modal('show');
             }
         }

         function PartyLedgerkeydown(e) {
             var OtherDetails = {}

             if ($.trim($("#txtPartyLedgerSearch").val()) == "" || $.trim($("#txtPartyLedgerSearch").val()) == null) {
                 return false;
             }
             OtherDetails.SearchKey = $("#txtPartyLedgerSearch").val();
             OtherDetails.LedgerID = document.getElementById('hdnSelectedLedger').value;
             OtherDetails.AllSubLedg = 0;

             if (e.code == "Enter" || e.code == "NumpadEnter") {
                 var HeaderCaption = [];
                 HeaderCaption.push("Description");
                 HeaderCaption.push("Type");

                 if ($("#txtPartyLedgerSearch").val() != "") {
                     callonServerM("Services/Master.asmx/GetSubLedger", OtherDetails, "PartyLedgerTable", HeaderCaption, "dPropertyIndex", "SetSelectedValues", "PartyLedgerSource");
                 }
             }
             else if (e.code == "ArrowDown") {
                 if ($("input[dPropertyIndex=0]"))
                     $("input[dPropertyIndex=0]").focus();
             }
         }

         function SetfocusOnseach(indexName) {
             if (indexName == "dPropertyIndex")
                 $('#txtPartyLedgerSearch').focus();
             else
                 $('#txtPartyLedgerSearch').focus();
         }
   </script>
      <%-- For multiselection--%>

    <script type="text/javascript">

        $(function () {
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

            var ProjectSelection = document.getElementById('hdnProjectSelection').value;
            if (ProjectSelection == "0") {
                $('#divProj').addClass('hidden');
            }
            else {
                $('#divProj').removeClass('hidden');
            }

            $("#ddlbranchHO").change(function () {
                var Ids = $(this).val();
                $('#MandatoryActivityType').attr('style', 'display:none');
                $("#hdnSelectedBranches").val('');
                cProductbranchPanel.PerformCallback('BindComponentGrid' + '~' + $("#ddlbranchHO").val());
            })

        })

        function LedgerValuewisePartyledger() {
            var key = gridledgerLookup.GetGridView().GetRowKey(gridledgerLookup.GetGridView().GetFocusedRowIndex());
            cEmployeetComponentPanel.PerformCallback('BindComponentGrid' + '~' + key);
        }

        $(function () {
            cProjectPanel.PerformCallback('BindProjectGrid');
        });

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
        }

        function btn_ShowRecordsClick(e) {
            var BranchSelection = document.getElementById('hdnBranchSelection').value;
            var ProjectSelection = document.getElementById('hdnProjectSelection').value;
            var ProjectSelectionInReport = document.getElementById('hdnProjectSelectionInReport').value;
            var data = "OnDateChanged";
            $("#hfIsPLedgVendFilter").val("Y");
            $("#drdExport").val(0);

            data += '~' + cxdeFromDate.GetDate();
            data += '~' + cxdeToDate.GetDate();

            var FromDate = (cxdeFromDate.GetValue() != null) ? cxdeFromDate.GetValue() : "";
            var ToDate = (cxdeToDate.GetValue() != null) ? cxdeToDate.GetValue() : "";
            var ledgerid = hdnSelectedLedger.value;
            var partyledgerid = hdnSelectedPartyLedger.value;

            FromDate = GetDateFormat(FromDate);
            ToDate = GetDateFormat(ToDate);
            document.getElementById('<%=DateRange.ClientID %>').innerHTML = "For the period: " + FromDate + " To " + ToDate;

            if (ctxtPartyLedger.GetValue() == null & chkallpartyledg.checked == false) {
                jAlert('Please select atleast one Vendor for generate the report.');
            }
            else if (ProjectSelection == "1") {
                if (ProjectSelectionInReport == "Yes" && gridprojectLookup.GetValue() == null) {
                    jAlert('Please select atleast one Project for generate the report.');
                }
                else if (BranchSelection == "Yes" && gridbranchLookup.GetValue() == null) {
                    jAlert('Please select atleast one branch for generate the report.');
                }
                else {
                    cCallbackPanel.PerformCallback(data + '~' + $("#ddlbranchHO").val() + '~' + ledgerid + '~' + partyledgerid);
                }
            }
            else {
                if (BranchSelection == "Yes" && gridbranchLookup.GetValue() == null) {
                    jAlert('Please select atleast one branch for generate the report.');
                }
                else {
                    cCallbackPanel.PerformCallback(data + '~' + $("#ddlbranchHO").val() + '~' + ledgerid + '~' + partyledgerid);
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

        function PartyLedgAll(obj) {
            if (obj == 'allpartyledg') {
                if (chkallpartyledg.checked == true) {
                    ctxtPartyLedger.SetText('');
                    GetObjectID('hdnSelectedPartyLedger').value = '';
                    document.getElementById("txtPartyLedgerSearch").value = ""
                    ctxtPartyLedger.SetEnabled(false);
                }
                else {
                    ctxtPartyLedger.SetEnabled(true);
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
                jAlert('Please select at least one Party Ledger', "Alert", function () {
                });
            }
            else if (gridbranchLookup.GetValue() == null && gridledgerLookup.GetValue() != null && gridemployeeLookup.GetValue() == null) {
                jAlert('Please select at least one Branch', "Alert", function () {
                });
            }
            else {
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
            document.getElementById('<%=CompBranch.ClientID %>').innerHTML = cCallbackPanel.cpBranchNames
            Grid.Refresh();
            $("#drdExport").val(0);
        }

        function OpenPOSDetails(Uniqueid, type, docno) {
            var url = '';
            <%--Rev 3.0 Mantis: 0026804--%>
            if (type != 'OP') {
            <%--End of Rev 3.0 Mantis: 0026804--%>
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
                else if (type == 'IPBETR') {
                    url = '/Import/PurchaseBillOfEntry-Import.aspx?key=' + Uniqueid + '&IsTagged=1&req=V&type=' + type;
                }

                popupbudget.SetContentUrl(url);
                popupbudget.Show();
                <%--Rev 3.0 Mantis: 0026804--%>
            }
            else {
                jAlert('You Can not Zoom in Opening Document.');
            }
            <%--End of Rev 3.0 Mantis: 0026804--%>

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

        function selectAll_Project() {
            gridprojectLookup.gridView.SelectRows();
        }
        function unselectAll_Project() {
            gridprojectLookup.gridView.UnselectRows();
        }
        function CloseGridProjectLookup() {
            gridprojectLookup.ConfirmCurrentSelection();
            gridprojectLookup.HideDropDown();
            gridprojectLookup.Focus();
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

        label , .mylabel1, .clsTo, .dxeBase_PlasticBlue
        {
            color: #141414 !important;
            font-size: 14px !important;
                font-weight: 500 !important;
                margin-bottom: 0 !important;
        }

        #GrpSelLbl .dxeBase_PlasticBlue
        {
                line-height: 20px !important;
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

        #ASPxFromDate , #ASPxToDate , #ASPxASondate , #ASPxAsOnDate
        {
            position: relative;
            z-index: 1;
            background: transparent;
        }

        #ASPxFromDate_B-1 , #ASPxToDate_B-1 , #ASPxASondate_B-1 , #ASPxAsOnDate_B-1
        {
            background: transparent !important;
            border: none;
            width: 30px;
            padding: 10px !important;
        }

        #ASPxFromDate_B-1 #ASPxFromDate_B-1Img , #ASPxToDate_B-1 #ASPxToDate_B-1Img , #ASPxASondate_B-1 #ASPxASondate_B-1Img , #ASPxAsOnDate_B-1 #ASPxAsOnDate_B-1Img
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
        .simple-select:disabled::after
        {
            background: #1111113b;
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
            padding: 8px 4px 8px 10px;
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

        .for-cust-icon {
            position: relative;
            z-index: 1;
        }

        .dxeDisabled_PlasticBlue, .aspNetDisabled
        {
            background: #f3f3f3 !important;
        }

        .dxeButtonDisabled_PlasticBlue
        {
            background: #b5b5b5 !important;
            border-color: #b5b5b5 !important;
        }

        #ddlValTech
        {
            width: 100% !important;
            margin-bottom: 0 !important;
        }

        .dis-flex
        {
            display: flex;
            align-items: baseline;
        }

        input + label
        {
            line-height: 1;
        }

        .dxtlHeader_PlasticBlue
        {
            background: #094e8c !important;
        }


        /*#lookup_project
        {
            max-width: 100% !important;
        }*/
        /*Rev end 1.0*/

        /*Rev 2.0*/

        #ASPXPopupControl2_PW-1 , #popupApproval_PW-1 , #ASPXPopupControl1_PW-1
        {
            position: fixed !important;
            top: 10% !important;
            left: 10% !important;
        }

        @media only screen and (max-width: 1450px) and (min-width: 1300px)
        {
            #ASPXPopupControl2_PW-1 , #popupApproval_PW-1 , #ASPXPopupControl1_PW-1
            {
                /*position:fixed !important;*/
                left: 6px !important;
                top: 8% !important;
            }
        }

        /*Rev end 2.0*/
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
                    <div>
                        <asp:Label ID="CompBranch" runat="Server" Text="" Width="470px" ></asp:Label>
                    </div>
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
            <%--<div class="col-md-1">--%>
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
            <div class="col-md-2">
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
                                </dxe:ASPxGridLookup>
                            </dxe:PanelContent>
                        </PanelCollection>

                    </dxe:ASPxCallbackPanel>

                </div>
            </div>

            <div class="col-md-2">
                <div style="color: #b5285f; font-weight: bold;" class="clsTo">
                    <asp:Label ID="Label4" runat="Server" Text="Vendor : " CssClass="mylabel1"
                        Width="92px"></asp:Label>
                </div>
                <div>
                      <dxe:ASPxButtonEdit ID="txtPartyLedger" runat="server" ReadOnly="true" ClientInstanceName="ctxtPartyLedger" Width="100%" TabIndex="5">
                            <Buttons>
                                <dxe:EditButton>
                                </dxe:EditButton>
                            </Buttons>
                            <ClientSideEvents ButtonClick="function(s,e){PartyLedgerButnClick();}" KeyDown="function(s,e){PartyLedger_KeyDown(s,e);}" />
                      </dxe:ASPxButtonEdit>
                    <asp:HiddenField ID="hdnSelectedPartyLedger" runat="server" />

                </div>
            </div>
            <div class="col-md-1" style="padding:0;padding-top: 24px;">
                <asp:CheckBox runat="server" ID="chkallpartyledg" Checked="false" Text="All Vendor" />
            </div>
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
                <div class="hide">
                    <div id="ckpar" style="padding-top: 7px;">
                        <asp:CheckBox runat="server" ID="chkparty" Checked="false" Text="Search by Party Inv. Date" />
                    </div>
                </div>            
            <div class="clear"></div>

            <div class="col-md-2" style="padding-top: 1px;" id="divProj">
                <div style="color: #b5285f; font-weight: bold;" class="clsTo">
                    <asp:Label ID="lblProj" runat="Server" Text="Project : " CssClass="mylabel1"></asp:Label>
                </div>
                <asp:ListBox ID="ListBoxProjects" Visible="false" runat="server" SelectionMode="Multiple" Font-Size="12px" Height="60px" Width="100%" CssClass="mb0 chsnProduct  hide" data-placeholder="--- ALL ---"></asp:ListBox>
                <dxe:ASPxCallbackPanel runat="server" ID="ProjectPanel" ClientInstanceName="cProjectPanel" OnCallback="Project_Callback">
                    <PanelCollection>
                        <dxe:PanelContent runat="server">
                            <dxe:ASPxGridLookup ID="lookup_project" SelectionMode="Multiple" runat="server" ClientInstanceName="gridprojectLookup"
                                OnDataBinding="lookup_project_DataBinding"
                                KeyFieldName="ID" Width="100%" TextFormatString="{1}" AutoGenerateColumns="False" MultiTextSeparator=", ">
                                <Columns>
                                    <dxe:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Width="60" Caption=" " />
                                    <dxe:GridViewDataColumn FieldName="project_code" Visible="true" VisibleIndex="1" width="200px" Caption="Project code" Settings-AutoFilterCondition="Contains">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dxe:GridViewDataColumn>

                                    <dxe:GridViewDataColumn FieldName="project_name" Visible="true" VisibleIndex="2" width="200px" Caption="Project Name" Settings-AutoFilterCondition="Contains">
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
                                                            <dxe:ASPxButton ID="ASPxButton1" runat="server" AutoPostBack="false" Text="Select All" ClientSideEvents-Click="selectAll_Project" UseSubmitBehavior="False"/>
                                                        <%--</div>--%>
                                                        <dxe:ASPxButton ID="ASPxButton2" runat="server" AutoPostBack="false" Text="Deselect All" ClientSideEvents-Click="unselectAll_Project" UseSubmitBehavior="False"/>                                                        
                                                        <dxe:ASPxButton ID="ASPxButton5" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseGridProjectLookup" UseSubmitBehavior="False" />
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
                    </PanelCollection>
                </dxe:ASPxCallbackPanel>

                <span id="MandatoryActivityType" style="display: none" class="validclass">
                    <img id="3gridHistory_DXPEForm_efnew_DXEFL_DXEditor1112_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></span>
                <asp:HiddenField ID="hdnSelectedProjects" runat="server" />
            </div>
            <%--Rev 3.0 Mantis: 0026804--%>
            <div class="col-md-2">
            <div style="color: #b5285f; font-weight: bold;" class="clsTo">
                <div style="padding-top: 16px">
                    <div style="padding-right: 1px; vertical-align: middle; padding-top: 6px">
                        <asp:CheckBox ID="chkShowOPBreakUp" runat="server" Checked="false" />
                        Show Opening Breakup
                    </div>
                </div>
                </div>
            </div>
            <%--End of Rev 3.0 Mantis: 0026804--%>
            <div class="col-md-2" style="padding: 5; padding-top: 20px;">
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
                        <dxe:ASPxGridView runat="server" ID="ShowGrid" ClientInstanceName="Grid" Width="100%" EnableRowsCache="false" AutoGenerateColumns="False" KeyboardSupport="true" KeyFieldName="SEQ"
                            DataSourceID="GenerateEntityServerModeDataSource" OnSummaryDisplayText="ShowGrid_SummaryDisplayText" Settings-HorizontalScrollBarMode="Visible">
                            <Columns>
                                <dxe:GridViewDataTextColumn FieldName="BRANCH_DESC" Caption="Unit" Width="150px" VisibleIndex="1" />
                                <dxe:GridViewDataTextColumn FieldName="TRAN_DATE" Caption="Date" Width="90px" VisibleIndex="2" PropertiesTextEdit-DisplayFormatString="dd-MM-yyyy" />
                                <dxe:GridViewDataTextColumn FieldName="PARTY" Caption="Vendor" Width="170px" VisibleIndex="3" />
                                <dxe:GridViewDataTextColumn FieldName="PROJ_NAME" Caption="Project Name" Width="180px" VisibleIndex="4" />

                                <dxe:GridViewDataTextColumn VisibleIndex="5" FieldName="DOCUMENT_NO" Caption="Document No" Width="130px">
                                    <CellStyle HorizontalAlign="Left">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <DataItemTemplate>

                                        <a href="javascript:void(0)" onclick="OpenPOSDetails('<%#Eval("DOC_ID") %>','<%#Eval("MODULE_TYPE") %>' ,'<%#Eval("DOCUMENT_NO") %>')" class="pad">
                                            <%#Eval("DOCUMENT_NO")%>
                                        </a>
                                    </DataItemTemplate>

                                    <EditFormSettings Visible="False" />
                                </dxe:GridViewDataTextColumn>
                               
                                <dxe:GridViewDataTextColumn VisibleIndex="6" FieldName="PARTYINVNO" Caption="Party Invoice No" Width="120px">
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="PARTYINVDT" Caption="Party Invoice Date" Width="120px" VisibleIndex="7" PropertiesTextEdit-DisplayFormatString="dd-MM-yyyy" />

                                <dxe:GridViewDataTextColumn FieldName="PARTICULARS" Caption="Particular" VisibleIndex="8" Width="150px" />
                                <dxe:GridViewDataTextColumn FieldName="PAYEE_INFO" Caption="Payee/Party" VisibleIndex="9" Width="140px" />
                                <dxe:GridViewDataTextColumn FieldName="HEADER_NARRATION" Caption="Header Narration" VisibleIndex="10" Width="200px" />

                                <dxe:GridViewDataTextColumn FieldName="OP_DEBIT" Caption="Opening Dr." Width="90px" VisibleIndex="11" PropertiesTextEdit-DisplayFormatString="0.00">
                                     <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="OP_CREDIT" Caption="Opening Cr." Width="90px" VisibleIndex="12" PropertiesTextEdit-DisplayFormatString="0.00" >
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="PR_DEBIT" Caption="Period Dr." Width="90px" VisibleIndex="13" PropertiesTextEdit-DisplayFormatString="0.00" >
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="PR_CREDIT" Caption="Period Cr." Width="90px" VisibleIndex="14" PropertiesTextEdit-DisplayFormatString="0.00" >
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>                              
                                <dxe:GridViewDataTextColumn FieldName="CL_DEBIT" Caption="Closing Dr." Width="90px" VisibleIndex="15" PropertiesTextEdit-DisplayFormatString="0.00" >
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>                              
                                <dxe:GridViewDataTextColumn FieldName="CL_CREDIT" Caption="Closing Cr." Width="90px" VisibleIndex="16" PropertiesTextEdit-DisplayFormatString="0.00" >
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
                                <dxe:ASPxSummaryItem FieldName="Particulars" SummaryType="Custom" />
                                <dxe:ASPxSummaryItem FieldName="OP_DEBIT" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="OP_CREDIT" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="PR_DEBIT" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="PR_CREDIT" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="CL_DEBIT" SummaryType="Custom" />
                                <dxe:ASPxSummaryItem FieldName="CL_CREDIT" SummaryType="Custom" />
                            </TotalSummary>

                        </dxe:ASPxGridView>
                        <dx:LinqServerModeDataSource ID="GenerateEntityServerModeDataSource" runat="server" OnSelecting="GenerateEntityServerModeDataSource_Selecting"
                        ContextTypeName="ReportSourceDataContext" TableName="PARTYLEDGERCUSTVEND_REPORT" ></dx:LinqServerModeDataSource>
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

    <!--Party Modal -->
    <div class="modal fade" id="PartyLedgModel" role="dialog">
        <div class="modal-dialog">
            <!-- Party content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Party Search</h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="PartyLedgerkeydown(event)" id="txtPartyLedgerSearch" autofocus width="100%" placeholder="Search By Party Name" />
                    <div id="PartyLedgerTable">
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
                     <button type="button" class="btnOkformultiselection btn-default"  onclick="DeSelectAll('PartyLedgerSource')">Deselect All</button>
                    <button type="button" class="btnOkformultiselection btn-default" data-dismiss="modal" onclick="OKPopup('PartyLedgerSource')">OK</button>
                </div>
            </div>
        </div>
    </div>
   <!--Party Modal -->

    <dxe:ASPxCallbackPanel runat="server" ID="CallbackPanel" ClientInstanceName="cCallbackPanel" OnCallback="CallbackPanel_Callback">
        <PanelCollection>
            <dxe:PanelContent runat="server">
                <asp:HiddenField ID="hfIsPLedgVendFilter" runat="server" />
            </dxe:PanelContent>
        </PanelCollection>
    <ClientSideEvents EndCallback="CallbackPanelEndCall" />
    </dxe:ASPxCallbackPanel>

    <asp:HiddenField ID="hdnSelectedLedger" runat="server" />
    <asp:HiddenField ID="hdnBranchSelection" runat="server" />
    <asp:HiddenField ID="hdnProjectSelection" runat="server" />
    <asp:HiddenField ID="hdnProjectSelectionInReport" runat="server" />
</asp:Content>
