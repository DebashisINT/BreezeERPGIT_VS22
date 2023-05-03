<%--================================================== Revision History =============================================
Rev Number         DATE              VERSION          DEVELOPER           CHANGES
1.0                21-02-2023        2.0.36           Pallab              25575 : Report pages design modification
2.0                02-05-2023        2.0.38           Pallab              25997 : Party Ledger - All module zoom popup upper part visible issue fix
====================================================== Revision History =============================================--%>

<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master" CodeBehind="PartyLedger.aspx.cs" Inherits="Reports.Reports.GridReports.PartyLedger" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
     Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
        <link href="CSS/SearchPopup.css" rel="stylesheet" />
        <script src="JS/SearchPopup.js"></script>
        <script src="JS/SearchMultiPopup.js"></script>

    <script src="/assests/pluggins/choosen/choosen.min.js"></script>
    <style>
        .colDisable {
        cursor:default !important;
        }
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
             });

             $("#ddlCriteria").change(function () {
                 var Values = $("#ddlCriteria").val();
                 $("#ckparty").attr('style', 'display:inline-block; padding-left: 15px; padding-top: 3px;color: #b5285f; font-weight: bold;" class="clsTo;');

                 if (Values == 'ALL') {
                     CchkAllParty.SetText('All Parties');
                 }
                 else if (Values == 'EM') {
                     CchkAllParty.SetText('All Employees');
                 }
                 else if (Values == 'CL') {
                     CchkAllParty.SetText('All Customers');
                 }
                 else if (Values == 'DV') {
                     CchkAllParty.SetText('All Vendors');
                 }
                 else if (Values == 'TR') {
                     CchkAllParty.SetText('All Transporter');
                 }
                 else if (Values == 'RA') {
                     CchkAllParty.SetText('All Influencers');
                 }
                 else if (Values == 'SL') {
                     CchkAllParty.SetText('All Sub Ledgers');
                 }
                 $("#ckparty").attr('style', 'padding-left: 1px; padding-top: 15px;color: #b5285f; font-weight: bold;" class="clsTo;');
             });
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

             if (e.code == "Enter" || e.code == "NumpadEnter") {
                 var HeaderCaption = [];
                 HeaderCaption.push("Description");
                 HeaderCaption.push("Type");
                 HeaderCaption.push("Address");

                 if ($("#txtPartyLedgerSearch").val() != "") {
                     //callonServerM("Services/Master.asmx/GetPartyLedger", OtherDetails, "PartyLedgerTable", HeaderCaption, "dPropertyIndex", "SetSelectedValues", "PartyLedgerSource");
                     if (ddlCriteria.value == "ALL")
                     {
                         OtherDetails.Type = "ALL"
                         callonServerM("Services/Master.asmx/GetPartyLedgerAll", OtherDetails, "PartyLedgerTable", HeaderCaption, "dPropertyIndex", "SetSelectedValues", "PartyLedgerSource");
                     }
                     else if (ddlCriteria.value == "EM") {
                         OtherDetails.Type = "EM"
                         callonServerM("Services/Master.asmx/GetPartyLedgerAll", OtherDetails, "PartyLedgerTable", HeaderCaption, "dPropertyIndex", "SetSelectedValues", "PartyLedgerSource");
                     }
                     else if (ddlCriteria.value == "CL") {
                         OtherDetails.Type = "CL"
                         callonServerM("Services/Master.asmx/GetPartyLedgerAll", OtherDetails, "PartyLedgerTable", HeaderCaption, "dPropertyIndex", "SetSelectedValues", "PartyLedgerSource");
                     }
                     else if (ddlCriteria.value == "DV") {
                         OtherDetails.Type = "DV"
                         callonServerM("Services/Master.asmx/GetPartyLedgerAll", OtherDetails, "PartyLedgerTable", HeaderCaption, "dPropertyIndex", "SetSelectedValues", "PartyLedgerSource");
                     }
                     else if (ddlCriteria.value == "TR") {
                         OtherDetails.Type = "TR"
                         callonServerM("Services/Master.asmx/GetPartyLedgerAll", OtherDetails, "PartyLedgerTable", HeaderCaption, "dPropertyIndex", "SetSelectedValues", "PartyLedgerSource");
                     }
                     else if (ddlCriteria.value == "RA") {
                         OtherDetails.Type = "RA"
                         callonServerM("Services/Master.asmx/GetPartyLedgerAll", OtherDetails, "PartyLedgerTable", HeaderCaption, "dPropertyIndex", "SetSelectedValues", "PartyLedgerSource");
                     }
                     else if (ddlCriteria.value == "SL") {
                         OtherDetails.Type = "SL"
                         callonServerM("Services/Master.asmx/GetPartyLedgerAll", OtherDetails, "PartyLedgerTable", HeaderCaption, "dPropertyIndex", "SetSelectedValues", "PartyLedgerSource");
                     }
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

        function OnGetRowValuesCallback(values) {
            alert(values);
        }

        $(function () {
            cProjectPanel.PerformCallback('BindProjectGrid');
        });

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
            $("#hfIsPartyLedgerFilter").val("Y");
            $("#drdExport").val(0);

            data += '~' + cxdeFromDate.GetDate();
            data += '~' + cxdeToDate.GetDate();

            var FromDate = (cxdeFromDate.GetValue() != null) ? cxdeFromDate.GetValue() : "";
            var ToDate = (cxdeToDate.GetValue() != null) ? cxdeToDate.GetValue() : "";
            var partyledgerid = hdnSelectedPartyLedger.value;

            FromDate = GetDateFormat(FromDate);
            ToDate = GetDateFormat(ToDate);
            document.getElementById('<%=DateRange.ClientID %>').innerHTML = "For the period: " + FromDate + " To " + ToDate;

            //if (ctxtPartyLedger.GetValue() == null) {
            if (ctxtPartyLedger.GetValue() == null && CchkAllParty.GetChecked() == false) {
                jAlert('Please select atleast one Party for generate the report.');
            }
            else if (ProjectSelection == "1") {
                if (ProjectSelectionInReport == "Yes" && gridprojectLookup.GetValue() == null) {
                    jAlert('Please select atleast one Project for generate the report.');
                }
                else if (BranchSelection == "Yes" && gridbranchLookup.GetValue() == null) {
                    jAlert('Please select atleast one branch for generate the report.');
                }
                else {
                    cCallbackPanel.PerformCallback(data + '~' + $("#ddlbranchHO").val() + '~' + partyledgerid);
                }
            }
            else {
                if (BranchSelection == "Yes" && gridbranchLookup.GetValue() == null) {
                    jAlert('Please select atleast one branch for generate the report.');
                }
                else {
                    cCallbackPanel.PerformCallback(data + '~' + $("#ddlbranchHO").val() + '~' + partyledgerid);
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

        function OnGetRowValues(types) {
            var BranchSelection = document.getElementById('hdnBranchSelection').value;
            var data = "OnDateChanged";

            if (gridemployeeLookup.GetValue() == null) {
                jAlert('Please select atleast one Party Ledger', "Alert", function () {
                });
            }
            else {
                if (BranchSelection == "Yes" && gridbranchLookup.GetValue() == null) {
                    jAlert('Please select atleast one branch for generate the report.');
                }
                else {
                    Grid.PerformCallback(data + '~' + types[1] + '~' + $("#ddlbranchHO").val());
                }
            }
        }

        function CallbackPanelEndCall(s, e) {
            document.getElementById('<%=CompBranch.ClientID %>').innerHTML = cCallbackPanel.cpBranchNames
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
                url = '/OMS/Management/Activities/VendorDrCrNoteAdd.aspx?key=' + Uniqueid + '&IsTagged=1&req=V&type=CDCN';
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
            max-width: 99% !important;
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
            left: 12% !important;
        }

        @media only screen and (max-width: 1450px) and (min-width: 1300px)
        {
            #ASPXPopupControl2_PW-1 , #popupApproval_PW-1 , #ASPXPopupControl1_PW-1
            {
                /*position:fixed !important;*/
                left: 6px !important;
                top: 10% !important;
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

        function CheckConsAllParty(s, e) {
            if (s.GetCheckState() == 'Checked') {
                ctxtPartyLedger.SetEnabled(false);
                ctxtPartyLedger.SetText('');
                GetObjectID('hdnSelectedPartyLedger').value = '';
               <%-- $(<%=ddlCriteria.ClientID%>).prop("disabled", true)
                $("#ddlCriteria").val("ALL");      --%>          
            }
            else {
                ctxtPartyLedger.SetEnabled(true);
               <%-- $(<%=ddlCriteria.ClientID%>).prop("disabled", false)
                $("#ddlCriteria").val("ALL");--%>
            }
        }
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
                    <div style="color: #b5285f" class="clsTo">
                        <asp:Label ID="Label3" runat="Server" Text="Head Branch : " CssClass="mylabel1"
                            Width="92px"></asp:Label>
                    </div>
                <div>
                    <asp:DropDownList ID="ddlbranchHO" runat="server" Width="100%"></asp:DropDownList>
                </div>
            </div>
            <div class="col-md-2">
                <div style="color: #b5285f" class="clsTo">
                        <asp:Label ID="Label1" runat="Server" Text="Branch : " CssClass="mylabel1"
                            Width="92px"></asp:Label>
                    </div>
                <div>

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
                                            <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,20,50,100" />
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
                <div style="color: #b5285f" class="clsTo">
                    <asp:Label ID="Label4" runat="Server" Text="Party : " CssClass="mylabel1"
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

            <%--Rev 1.0--%>
            <%--<div class="col-md-2">--%>
            <div class="col-md-2 simple-select">
                <%--Rev end 1.0--%>
                <div style="color: #b5285f" class="clsTo">
                    <asp:Label ID="Label2" runat="Server" Text="Criteria : " CssClass="mylabel1"></asp:Label>
                </div>
                <asp:DropDownList ID="ddlCriteria" runat="server" Width="100%">
                    <%--<asp:ListItem Text="All" Value="ALL"></asp:ListItem>--%>
                    <asp:ListItem Text="Employee" Value="EM" Selected="True"></asp:ListItem>
                    <asp:ListItem Text="Customer" Value="CL"></asp:ListItem>
                    <asp:ListItem Text="Vendor" Value="DV"></asp:ListItem>
                    <asp:ListItem Text="Transporter" Value="TR"></asp:ListItem>
                    <asp:ListItem Text="Influencer" Value="RA"></asp:ListItem>
                    <asp:ListItem Text="Sub Ledger" Value="SL"></asp:ListItem>
                </asp:DropDownList>
            </div>

             <div class="col-md-2" style="padding-top: 1px;color: #b5285f">
                <div id="ckparty" style="padding-right: 10px; vertical-align: middle; padding-top: 15px">
                    <dxe:ASPxCheckBox ID="chkAllParty" runat="server" Checked="false" Text="All Employees" ClientInstanceName="CchkAllParty">
                        <ClientSideEvents CheckedChanged="CheckConsAllParty" />
                    </dxe:ASPxCheckBox>                    
                </div>
            </div>

            <div class="col-md-2" style="padding-top: 1px;color: #141414">
                <div style="padding-right: 10px; vertical-align: middle; padding-top: 15px">
                    <dxe:ASPxCheckBox ID="chkPostDet" runat="server" Checked="false"></dxe:ASPxCheckBox>
                    Posting Details
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

            <div class="col-md-2">               
                    <div style="color: #b5285f" class="clsFrom">
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
               
                    <div style="color: #b5285f" class="clsTo">
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
            <div class="col-md-4" style="padding-top: 20px;">
                <div>
                    <button id="btnShow" class="btn btn-success" type="button" onclick="btn_ShowRecordsClick(this);">Show</button>
                     <% if (rights.CanExport)
                       { %>
                    <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-sm btn-primary"
                        OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true" OnChange="if(!AvailableExportOption()){return false;}">
                        <asp:ListItem Value="0">Export to</asp:ListItem>
                        <asp:ListItem Value="1">XLSX</asp:ListItem>
                        <asp:ListItem Value="2">PDF</asp:ListItem>
                        <asp:ListItem Value="3">CSV</asp:ListItem>
                        <asp:ListItem Value="4">RTF</asp:ListItem>

                    </asp:DropDownList>
                     <% } %>
                </div>
            </div>
        </div>
       

        <table class="TableMain100">
            <tr>
                <td colspan="2">
                    <div onkeypress="OnWaitingGridKeyPress(event)">
                        <dxe:ASPxGridView runat="server" ID="ShowGrid" ClientInstanceName="Grid" Width="100%" EnableRowsCache="false" AutoGenerateColumns="False" KeyboardSupport="true" KeyFieldName="SEQ"
                            DataSourceID="GenerateEntityServerModeDataSource" OnSummaryDisplayText="ShowGrid_SummaryDisplayText" Settings-HorizontalScrollBarMode="Visible">
                            <Columns>
                                <dxe:GridViewDataTextColumn FieldName="BRANCH_DESC" Caption="Unit" Width="150px" VisibleIndex="1" HeaderStyle-CssClass="colDisable"/>
                                <%--<dxe:GridViewDataTextColumn FieldName="LEDG_DESC" Caption="Ledger Description" Width="150px" VisibleIndex="2" />--%>
                                <dxe:GridViewDataTextColumn FieldName="SUBLEDGER" Caption="Party" Width="170px" VisibleIndex="2" HeaderStyle-CssClass="colDisable"/>

                                <dxe:GridViewDataTextColumn FieldName="PROJ_NAME" Caption="Project Name" Width="200px" VisibleIndex="3" HeaderStyle-CssClass="colDisable"/>

                                <dxe:GridViewDataTextColumn FieldName="TRAN_DATE" Caption="Date" Width="90px" VisibleIndex="4" PropertiesTextEdit-DisplayFormatString="dd-MM-yyyy" HeaderStyle-CssClass="colDisable"/>

                                <dxe:GridViewDataTextColumn VisibleIndex="5" FieldName="DOCUMENT_NO" Caption="Document No" Width="120px" HeaderStyle-CssClass="colDisable">
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
                               

                                <dxe:GridViewDataTextColumn FieldName="PARTICULARS" Caption="Particular" VisibleIndex="6" Width="120px" HeaderStyle-CssClass="colDisable"/>
                                <dxe:GridViewDataTextColumn FieldName="LEDG_DESC" Caption="Ledger Description" Width="150px" VisibleIndex="7" HeaderStyle-CssClass="colDisable"/>
                                <dxe:GridViewDataTextColumn FieldName="PAYEE_INFO" Caption="Payee/Party" VisibleIndex="8" Width="140px" HeaderStyle-CssClass="colDisable"/>
                                <dxe:GridViewDataTextColumn FieldName="HEADER_NARRATION" Caption="Header Narration" VisibleIndex="9" Width="200px" HeaderStyle-CssClass="colDisable"/>

                                <dxe:GridViewDataTextColumn FieldName="OP_DEBIT" Caption="Opening Dr." Width="90px" VisibleIndex="10" PropertiesTextEdit-DisplayFormatString="0.00" HeaderStyle-CssClass="colDisable">
                                     <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="OP_CREDIT" Caption="Opening Cr." Width="90px" VisibleIndex="11" PropertiesTextEdit-DisplayFormatString="0.00" HeaderStyle-CssClass="colDisable">
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="PR_DEBIT" Caption="Period Dr." Width="90px" VisibleIndex="12" PropertiesTextEdit-DisplayFormatString="0.00" HeaderStyle-CssClass="colDisable">
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="PR_CREDIT" Caption="Period Cr." Width="90px" VisibleIndex="13" PropertiesTextEdit-DisplayFormatString="0.00" HeaderStyle-CssClass="colDisable">
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>                              
                                <dxe:GridViewDataTextColumn FieldName="CL_DEBIT" Caption="Closing Dr." Width="90px" VisibleIndex="14" PropertiesTextEdit-DisplayFormatString="0.00" HeaderStyle-CssClass="colDisable">
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>                              
                                <dxe:GridViewDataTextColumn FieldName="CL_CREDIT" Caption="Closing Cr." Width="90px" VisibleIndex="15" PropertiesTextEdit-DisplayFormatString="0.00" HeaderStyle-CssClass="colDisable">
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>                              
                            </Columns>
                            <SettingsBehavior ConfirmDelete="true" EnableCustomizationWindow="true" EnableRowHotTrack="true" />
                            <Settings ShowFooter="true" ShowGroupPanel="true" ShowGroupFooter="VisibleIfExpanded" />
                            <SettingsEditing Mode="EditForm" />
                            <SettingsContextMenu Enabled="true" />
                            <SettingsBehavior AutoExpandAllGroups="true" AllowSort="False" ColumnResizeMode="Control" />
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
                        ContextTypeName="ReportSourceDataContext" TableName="PARTYLEDGERPOSTING_REPORT" ></dx:LinqServerModeDataSource>
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
                                <th>Address</th>
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
                <asp:HiddenField ID="hfIsPartyLedgerFilter" runat="server" />
            </dxe:PanelContent>
        </PanelCollection>
    <ClientSideEvents EndCallback="CallbackPanelEndCall" />
    </dxe:ASPxCallbackPanel>

    <asp:HiddenField ID="hdnBranchSelection" runat="server" />
    <asp:HiddenField ID="hdnProjectSelection" runat="server" />
    <asp:HiddenField ID="hdnProjectSelectionInReport" runat="server" />
</asp:Content>