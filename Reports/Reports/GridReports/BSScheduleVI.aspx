<%--================================================== Revision History ============================================
Rev Number         DATE              VERSION          DEVELOPER           CHANGES
1.0                16-02-2023        V2.0.36           Pallab              25575 : Report pages design modification
====================================================== Revision History ================================================--%>

<%@ Page Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="BSScheduleVI.aspx.cs" Inherits="Reports.Reports.GridReports.BSScheduleVI" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <link rel="stylesheet" href="https://use.fontawesome.com/releases/v5.3.1/css/all.css" integrity="sha384-mzrmE5qonljUremFsqc01SB46JvROS7bZs3IO2EmfFsd15uHvIt+Y8vEf7N7fWAU" crossorigin="anonymous">
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

        /*.unit .dxeTextBox_PlasticBlue {
            border: none !important;
        }*/

        .pad32 {
        padding-left:32px;
        }
        .pad52 {
        padding-left:52px;
        }
        .pad72 {
        padding-left:82px;
        }
        .pad102 {
        padding-left:102px;
        }
        .pad132 {
        padding-left:132px;
        }
        .makebold label{
        font-weight:bold !important;
        }
        #ShowGrid, #ShowGrid>tbody>tr>td>div.dxgvCSD {
            width:100% !important;
        }
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
            if (event.keyCode == 27 && cpopup2ndLevel.GetVisible() == true && cpopup3rdLevel.GetVisible() == false && popupdocument.GetVisible() == false) {
                popupHide();
            }
            else if (event.keyCode == 27 && cpopup3rdLevel.GetVisible() == true && popupdocument.GetVisible() == false) {
                popupHide2();
            }
            else if (event.keyCode == 27 && popupdocument.GetVisible() == true) {
                popupHide3();
            }
        }
        function popupHide(s, e) {
            cpopup2ndLevel.Hide();
            Grid.Focus();
            Grid.SetFocusedRowIndex(0);
            $("#drdExport").val(0);
        }
        function popupHide2(s, e) {
            cpopup3rdLevel.Hide();
            cShowGridDetails2Level.Focus();
            cShowGridDetails2Level.SetFocusedRowIndex(0);
            $("#ddlExport2").val(0);
        }
        function popupHide3(s, e) {
            popupdocument.Hide();
            cShowGridDetails3Level.Focus();
            cShowGridDetails3Level.SetFocusedRowIndex(0);
            $("#ddlExport3").val(0);
        }

        function Callback1_EndCallback() {
            if (Grid.cpUnit == 'Ones') {
                //ctxtUnitText.SetText('(Rs. in Ones)')
                //ctxtUnitText.innerHTML = "(<i class='fa  rupee-sign'></i>in Ones)"
                //clblUnit.innerHTML = "(<i class='fa  rupee-sign'></i>in Ones)"
                lblUnit.innerHTML = "(<i class='fa  fa-rupee-sign'></i>)"
            }
            else if (Grid.cpUnit == 'Tens') {
                lblUnit.innerHTML = "(<i class='fa  fa-rupee-sign'></i> in Tens)"
            }
            else if (Grid.cpUnit == 'Hundreds') {
                lblUnit.innerHTML = "(<i class='fa  fa-rupee-sign'></i> in Hundreds)"
            }
            else if (Grid.cpUnit == 'Thousands') {
                lblUnit.innerHTML = "(<i class='fa  fa-rupee-sign'></i> in Thousands)"
            }
            else if (Grid.cpUnit == 'Ten Thousands') {
                lblUnit.innerHTML = "(<i class='fa  fa-rupee-sign'></i> in Ten Thousands)"
            }
            else if (Grid.cpUnit == 'Lakhs') {
                lblUnit.innerHTML = "(<i class='fa  fa-rupee-sign'></i> in Lakhs)"
            }
            else if (Grid.cpUnit == 'Ten Lakhs') {
                lblUnit.innerHTML = "(<i class='fa  fa-rupee-sign'></i> in Ten Lakhs)"
            }
            else if (Grid.cpUnit == 'Crores') {
                lblUnit.innerHTML = "(<i class='fa  fa-rupee-sign'></i> in Crores)"
            }
            else if (Grid.cpUnit == 'Ten Crores') {
                lblUnit.innerHTML = "(<i class='fa  fa-rupee-sign'></i> in Ten Crores)"
            }
            Grid.cpUnit = null;
            $("#drdExport").val(0);
            Grid.Focus();
            Grid.SetFocusedRowIndex(0);
        }

        function Callback2_EndCallback() {
            if (cShowGridDetails2Level.cpUnit2 == 'Ones') {
                lblUnit2ndLevel.innerHTML = "(<i class='fa  fa-rupee-sign'></i>)"
            }
            else if (cShowGridDetails2Level.cpUnit2 == 'Tens') {
                lblUnit2ndLevel.innerHTML = "(<i class='fa  fa-rupee-sign'></i> in Tens)"
            }
            else if (cShowGridDetails2Level.cpUnit2 == 'Hundreds') {
                lblUnit2ndLevel.innerHTML = "(<i class='fa  fa-rupee-sign'></i> in Hundreds)"
            }
            else if (cShowGridDetails2Level.cpUnit2 == 'Thousands') {
                lblUnit2ndLevel.innerHTML = "(<i class='fa  fa-rupee-sign'></i> in Thousands)"
            }
            else if (cShowGridDetails2Level.cpUnit2 == 'Ten Thousands') {
                lblUnit2ndLevel.innerHTML = "(<i class='fa  fa-rupee-sign'></i> in Ten Thousands)"
            }
            else if (cShowGridDetails2Level.cpUnit2 == 'Lakhs') {
                lblUnit2ndLevel.innerHTML = "(<i class='fa  fa-rupee-sign'></i> in Lakhs)"
            }
            else if (cShowGridDetails2Level.cpUnit2 == 'Ten Lakhs') {
                lblUnit2ndLevel.innerHTML = "(<i class='fa  fa-rupee-sign'></i> in Ten Lakhs)"
            }
            else if (cShowGridDetails2Level.cpUnit2 == 'Crores') {
                lblUnit2ndLevel.innerHTML = "(<i class='fa  fa-rupee-sign'></i> in Crores)"
            }
            else if (cShowGridDetails2Level.cpUnit2 == 'Ten Crores') {
                lblUnit2ndLevel.innerHTML = "(<i class='fa  fa-rupee-sign'></i> in Ten Crores)"
            }
            cShowGridDetails2Level.cpUnit2 = null;
            $("#drdExport2").val(0);
            cShowGridDetails2Level.Focus();
            cShowGridDetails2Level.SetFocusedRowIndex(0);
        }

        function Callback3_EndCallback() {
            if (cShowGridDetails3Level.cpUnit3 == 'Ones') {
                lblUnit3rdLevel.innerHTML = "(<i class='fa  fa-rupee-sign'></i>)"
            }
            else if (cShowGridDetails3Level.cpUnit3 == 'Tens') {
                lblUnit3rdLevel.innerHTML = "(<i class='fa  fa-rupee-sign'></i> in Tens)"
            }
            else if (cShowGridDetails3Level.cpUnit3 == 'Hundreds') {
                lblUnit3rdLevel.innerHTML = "(<i class='fa  fa-rupee-sign'></i> in Hundreds)"
            }
            else if (cShowGridDetails3Level.cpUnit3 == 'Thousands') {
                lblUnit3rdLevel.innerHTML = "(<i class='fa  fa-rupee-sign'></i> in Thousands)"
            }
            else if (cShowGridDetails3Level.cpUnit3 == 'Ten Thousands') {
                lblUnit3rdLevel.innerHTML = "(<i class='fa  fa-rupee-sign'></i> in Ten Thousands)"
            }
            else if (cShowGridDetails3Level.cpUnit3 == 'Lakhs') {
                lblUnit3rdLevel.innerHTML = "(<i class='fa  fa-rupee-sign'></i> in Lakhs)"
            }
            else if (cShowGridDetails3Level.cpUnit3 == 'Ten Lakhs') {
                lblUnit3rdLevel.innerHTML = "(<i class='fa  fa-rupee-sign'></i> in Ten Lakhs)"
            }
            else if (cShowGridDetails3Level.cpUnit3 == 'Crores') {
                lblUnit3rdLevel.innerHTML = "(<i class='fa  fa-rupee-sign'></i> in Crores)"
            }
            else if (cShowGridDetails3Level.cpUnit3 == 'Ten Crores') {
                lblUnit3rdLevel.innerHTML = "(<i class='fa  fa-rupee-sign'></i> in Ten Crores)"
            }
            cShowGridDetails3Level.cpUnit3 = null;
            $("#drdExport3").val(0);
            cShowGridDetails3Level.Focus();
            cShowGridDetails3Level.SetFocusedRowIndex(0);
        }

        function btn_ShowRecordsClick(e) {
            $("#hfIsBSScheduleVIFilter").val("Y");
            var branchid = $('#ddlbranchHO').val();
            if (gridbranchLookup.GetValue() == null) {
                jAlert('Please select atleast one Branch for generate the report.');
            }
            else {
                cCallbackPanel.PerformCallback(branchid);
            }

            var AsOnDate = (cxdeAsOnDate.GetValue() != null) ? cxdeAsOnDate.GetValue() : "";
            var LAsOnDate = (cxdeAsOnDate.GetValue() != null) ? cxdeAsOnDate.GetValue() : "";

            AsOnDate = GetDateFormat(AsOnDate);
            LAsOnDate = GetMonthFormat(LAsOnDate);

            document.getElementById('<%=DateRange.ClientID %>').innerHTML = "As on: " + AsOnDate;
            <%--document.getElementById('<%=LAsOnDate.ClientID %>').innerHTML = "As at " + LAsOnDate;--%>
            
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

        function GetMonthFormat(asonday) {
            if (asonday != "") {
                var n;
                var dd = asonday.getDate();
                var mm = asonday.getMonth() + 1; //January is 0!
                var month = new Array();
                var yyyy = asonday.getFullYear();
                if (dd < 10) {
                    dd = '0' + dd;
                }
                if (mm < 10) {
                    mm = '0' + mm;
                }
                if (mm == '01') {
                    n = "January";
                }
                else if (mm == '02') {
                    n = "February";
                }
                else if (mm == '03') {
                    n = "March";
                }
                else if (mm == '04') {
                    n = "April";
                }
                else if (mm == '05') {
                    n = "May";
                }
                else if (mm == '06') {
                    n = "June";
                }
                else if (mm == '07') {
                    n = "July";
                }
                else if (mm == '08') {
                    n = "August";
                }
                else if (mm == '09') {
                    n = "September";
                }
                else if (mm == '10') {
                    n = "October";
                }
                else if (mm == '11') {
                    n = "November";
                }
                else if (mm == '12') {
                    n = "December";
                }
                asonday = dd + ' ' + n + ', ' + yyyy;
            }

            return asonday;
        }        

        $(document).ready(function () {
            $("#ListBoxBranches").chosen().change(function () {
                var Ids = $(this).val();
                $('#<%=hdnSelectedBranches.ClientID %>').val(Ids);
                $('#MandatoryActivityType').attr('style', 'display:none');
            })
        })        
    </script>

    <script type="text/javascript">
        function CallbackPanelEndCall() {
            <%--Rev Subhra 24-12-2018   0017670--%>
            document.getElementById('<%=CompBranch.ClientID %>').innerHTML = cCallbackPanel.cpBranchNames
            //End of Subhra
            Grid.Refresh();
        }

        function OnContextMenuItemClick(sender, args) {
            if (args.item.name == "ExportToPDF" || args.item.name == "ExportToXLS") {
                args.processOnServer = true;
                args.usePostBack = true;
            } else if (args.item.name == "SumSelected")
                args.processOnServer = true;
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
            if (e.key == "Enter" || e.key == "NumpadEnter") {
                var index = Grid.GetFocusedRowIndex();
                $("#ddlExport2").val(0);
                var group = Grid.GetRow(index).children[5].innerHTML;
                var branchid = $('#ddlbranchHO').val();
                $("#hfIsBSScheduleVIL1Filter").val("Y");

                cCallbackPanelDetailL1.PerformCallback(group + "~" + branchid);
                cpopup2ndLevel.Show();

            }
        }

        function closePopup(s, e) {
            e.cancel = false;
            Grid.Focus();
            Grid.SetFocusedRowIndex(0);
            $("#drdExport").val(0);
        }

        function CallbackPanelDetailL1EndCall() {
            cShowGridDetails2Level.Refresh();
            cShowGridDetails2Level.Focus();
            cShowGridDetails2Level.SetFocusedRowIndex(0);
            ctxtLedger2ndLevel.SetText(cCallbackPanelDetailL1.cpGroupDesc);

            $("#lblAsOnDate2ndLevel")[0].innerHTML = "As on : " + cCallbackPanelDetailL1.cpdtAsOnDate;

            $("#Label3")[0].innerHTML = "Group :";

            if (cCallbackPanelDetailL1.cpBlankLedger == "0") {
                ctxtLedger2ndLevel.SetText('');
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
    <%--end 2nd level popup--%>

    <%--For 3rd level popup--%>
    <script type="text/javascript">
        function OnWaitingGridKeyPress2(e) {
            if (e.key == "Enter" || e.key == "NumpadEnter") {
                var index = cShowGridDetails2Level.GetFocusedRowIndex();
                $("#ddlExport3").val(0);
                var branchid = cShowGridDetails2Level.GetRow(index).children[8].innerHTML;
                var ledger = cShowGridDetails2Level.GetRow(index).children[9].innerHTML;
                //var branchid = $('#ddlbranchHO').val();
                $("#hfIsBSScheduleVIL2Filter").val("Y");

                cCallbackPanelDetailL2.PerformCallback(ledger + "~" + branchid);
                cpopup3rdLevel.Show();
            }
        }

        function CallbackPanelDetailL2EndCall() {
            cShowGridDetails3Level.Refresh();
            cShowGridDetails3Level.Focus();
            cShowGridDetails3Level.SetFocusedRowIndex(0);
            ctxtBranch3rdLevel.SetText(cCallbackPanelDetailL2.cpBranch)
            ctxtLedger3rdLevel.SetText(cCallbackPanelDetailL2.cpLedger);

            $("#Label6")[0].innerHTML = "Branch :";
            $("#Label5")[0].innerHTML = "Ledger :";
            $("#lblAsOnDate3rdLevel")[0].innerHTML = "As on : " + cCallbackPanelDetailL2.cpAsOnDate;            

            if (cCallbackPanelDetailL2.cpBlankLedger == "0") {
                ctxtLedger3rdLevel.SetText('');
            }
        }

        function closePopup2(s, e) {
            e.cancel = false;
            cShowGridDetails2Level.Focus();
            cShowGridDetails2Level.SetFocusedRowIndex(0);
            $("#ddlExport2").val(0);
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
            popupdocument.SetContentUrl(url);
            popupdocument.Show();
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
            font-size: 16px;
            padding-left: 10px;
            position: relative;
            width: 100%;
            display: block;
            padding: 9px 10px 5px 10px;
        }

            .plhead a > i {
                position: absolute;
                top: 11px;
                right: 15px;
            }

        #accordion {
            margin-bottom: 10px;
        }

        .companyName {
            font-size: 16px;
            font-weight: bold;
            margin-bottom: 15px;
        }

        
        .plhead a.collapsed .fa-minus-circle {
            display: none;
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
        /*.simple-select::after select:disabled
        {
            background: #1111113b;
        }*/
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

        /*table
        {
            max-width: 99% !important;
        }*/

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
            background: #f7f7f7;
        }

        #ddlValTech
        {
            margin-bottom: 0;
        }

        /*#lookup_project
        {
            max-width: 100% !important;
        }*/
        /*Rev end 1.0*/
    </style>

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
                            <asp:Label ID="RptHeading" runat="Server" Text="" Width="470px" Style="font-weight: bold;"></asp:Label>
                            <i class="fa fa-plus-circle"></i>
                            <i class="fa fa-minus-circle"></i>
                        </a>
                    </h4>
                </div>
                <div id="collapseOne" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingOne">
                    <div class="panel-body">
                        <div class="companyName">
                            <asp:Label ID="CompName" runat="Server" Text="" Width="470px"></asp:Label>
                        </div>
                       <%--Rev Subhra 24-12-2018   0017670--%>
                        <div>
                            <asp:Label ID="CompBranch" runat="Server" Text="" Width="470px" ></asp:Label>
                        </div>
                        <%--End of Rev--%>
                        <div>
                            <asp:Label ID="CompAdd" runat="Server" Text="" Width="470px"></asp:Label>
                        </div>
                        <div>
                            <asp:Label ID="CompOth" runat="Server" Text="" Width="470px"></asp:Label>
                        </div>
                        <div>
                            <asp:Label ID="CompPh" runat="Server" Text="" Width="470px"></asp:Label>
                        </div>
                        <div>
                            <asp:Label ID="CompAccPrd" runat="Server" Text="" Width="470px"></asp:Label>
                        </div>
                        <div>
                            <asp:Label ID="DateRange" runat="Server" Text="" Width="470px"></asp:Label>
                        </div>
                        <%--<div>
                            <asp:Label ID="LAsOnDate" runat="Server" Text="" Width="470px"></asp:Label>
                        </div>--%>
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
           <%-- <div class="col-md-2">
                <asp:Label ID="LAsOnDate" runat="Server" Text="" Width="470px"></asp:Label>
            </div>
            <div class="clear"></div>--%>
            <div class="col-md-2" id="dvtodate">
                <label style="color: #b5285f; font-weight: bold;" class="clsTo">
                    <asp:Label ID="lblToDate" runat="Server" Text="As On Date " CssClass="mylabel1"
                        Width="92px" Font-Bold="True"></asp:Label>
                </label>
                <dxe:ASPxDateEdit ID="ASPxAsOnDate" runat="server" EditFormat="custom" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy"
                    UseMaskBehavior="True" Width="100%" ClientInstanceName="cxdeAsOnDate">
                    <buttonstyle width="13px">
                    </buttonstyle>
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
                                </dxe:ASPxGridLookup>
                            </dxe:PanelContent>
                        </panelcollection>

                </dxe:ASPxCallbackPanel>
            </div>
            <%--<div class="clear"></div>--%>
            <%--Rev 1.0--%>
            <%--<div class="col-md-2">--%>
            <div class="col-md-2 simple-select">
                <%--Rev end 1.0--%>
                <div style="color: #b5285f; font-weight: bold;" class="clsTo">
                    <label style="color: #b5285f; font-weight: bold;" class="clsTo">
                        <asp:Label ID="Label4" runat="Server" Text="Units : " CssClass="mylabel1"
                            Width="92px" Font-Bold="True"></asp:Label>
                    </label>
                </div>
                <asp:DropDownList ID="ddlunits" runat="server" Width="100%">
                    <asp:ListItem Text="Default" Value="1"></asp:ListItem>
                    <asp:ListItem Text="Tens" Value="10"></asp:ListItem>
                    <asp:ListItem Text="Hundreds" Value="100"></asp:ListItem>
                    <asp:ListItem Text="Thousands" Value="1000"></asp:ListItem>
                    <asp:ListItem Text="Ten Thousands" Value="10000"></asp:ListItem>
                    <asp:ListItem Text="Lakhs" Value="100000"></asp:ListItem>
                    <asp:ListItem Text="Ten Lakhs" Value="1000000"></asp:ListItem>
                    <asp:ListItem Text="Crores" Value="10000000"></asp:ListItem>
                    <asp:ListItem Text="Ten Crores" Value="100000000"></asp:ListItem>
                </asp:DropDownList>
            </div>

            <%--<div class="col-md-2" style="padding-top: 16px">
                <div style="padding-right: 10px; vertical-align: middle; padding-top: 6px">
                    <asp:CheckBox ID="chkCLSTK" runat="server" Text="Consider Closing Stock" />
                </div>
            </div>--%>

            <div class="col-md-2" style="margin-top:20px;color: #b5285f; font-weight: bold;">
             <dxe:ASPxCheckBox runat="server" ID="chkCLSTK" Checked="false" Text="Consider Closing Stock" >
                 <ClientSideEvents CheckedChanged="CheckConsPL" />
             </dxe:ASPxCheckBox>
            </div>
            
             <div class="col-md-2 simple-select" style="color: #b5285f; font-weight: bold;">
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
            <div class="clear"></div>
            <div class="col-md-3" style="margin-top:12px;color: #b5285f; font-weight: bold;">
                <dxe:ASPxCheckBox runat="server" ID="chkOWMSTVT" Checked="false" ClientEnabled="false" Text="Override Product Valuation Technique in Master" ClientInstanceName="CchkOWMSTVT">
                </dxe:ASPxCheckBox>
            </div> 

            <div class="col-md-2" style="margin-top:12px;color: #b5285f; font-weight: bold;">
               <dxe:ASPxCheckBox runat="server" ID="chkConsLandCost" Checked="false" ClientEnabled="false" Text="Consider Landed Cost" ClientInstanceName="CchkConsLandCost">
               </dxe:ASPxCheckBox>
            </div>

            <div class="col-md-2" style="margin-top:12px;color: #b5285f; font-weight: bold;">
                <dxe:ASPxCheckBox runat="server" ID="chkConsOverHeadCost" Checked="false" ClientEnabled="false" Text="Consider Overhead Cost" ClientInstanceName="CchkConsOverHeadCost">
                </dxe:ASPxCheckBox>
            </div>

            <div class="col-md-2" style="padding-top: 12px;color: #b5285f; font-weight: bold;">
                <div style="padding-right: 10px; vertical-align: middle">
                    <%--<asp:CheckBox ID="chkRE" runat="server" Text="Consider Retained Earnings" />--%>
                    <dxe:ASPxCheckBox runat="server" ID="chkRE" Checked="false" Text="Consider Retained Earnings">
                    </dxe:ASPxCheckBox>
                </div>
            </div>
            <%--<div class="clear"></div>--%>
            <div class="col-md-2" style="padding-top: 8px">
                <button id="btnShow" class="btn btn-success" type="button" onclick="btn_ShowRecordsClick(this);">Show</button>
                <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-sm btn-primary"
                    OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true" OnChange="if(!AvailableExportOption()){return false;}">
                    <asp:ListItem Value="0">Export to</asp:ListItem>
                   <asp:ListItem Value="1">XLSX</asp:ListItem>
                    <asp:ListItem Value="2">PDF</asp:ListItem>
                    <asp:ListItem Value="3">CSV</asp:ListItem>
                    <asp:ListItem Value="4">RTF</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="clear"></div>

            <%--<div class="pull-right unit" style="padding-top: 1px; margin-right: 15px">--%>
                <%--<label style="color: #b5285f; font-weight: normal;" class="clsFrom">--%>
                    <%--<dxe:ASPxTextBox ID="txtUnitText" ClientInstanceName="ctxtUnitText" runat="server" ReadOnly="true" Width="150px" HorizontalAlign="Right"></dxe:ASPxTextBox>--%>
                    <%--<asp:Label ID="lblUnit" runat="Server" ClientInstanceName="clblUnit" Text="Units : " CssClass="mylabel1"
                            Width="92px" Font-Bold="True"></asp:Label>--%>
                <%--</label>
            </div>--%>

            <div class="pull-right" style="color: #0c45ea; font-weight: bold;padding-top: 1px; margin-right: 20px">
                <label class="clsFrom">
                    <asp:Label ID="lblUnit" runat="Server" ClientInstanceName="clblUnit" Text="" CssClass=" text-right mylabel1" Width="150px" Font-Bold="True"></asp:Label>
                </label>
            </div>

        </div>
       <%-- <div class="clear"></div>
        <div class="col-md-2"></div>--%>
    </div>
    <%--</div>--%>
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
                    <dxe:ASPxGridView runat="server" ID="ShowGrid" ClientInstanceName="Grid" Width="100%" EnableRowsCache="false" AutoGenerateColumns="False" KeyboardSupport="true" KeyFieldName="SL"
                        OnHtmlDataCellPrepared="ShowGrid_HtmlDataCellPrepared" OnDataBound="Showgrid_DataBound" OnHtmlRowPrepared="ShowGrid_HtmlRowPrepared"
                        SettingsBehavior-AllowFocusedRow="true" SettingsBehavior-AllowSelectSingleRowOnly="true" Settings-HorizontalScrollBarMode="Visible" DataSourceID="GenerateEntityServerModeDataSource">
                        <columns>   
                                <dxe:GridViewDataTextColumn FieldName="SLSPACE" Caption="SLSPACE" Width="0%" VisibleIndex="1" HeaderStyle-CssClass="colDisable">
                                </dxe:GridViewDataTextColumn>

                                <%--<dxe:GridViewDataTextColumn FieldName="NAME" Caption="Particulars" Width="55%" VisibleIndex="2">
                                </dxe:GridViewDataTextColumn>--%>

                                <dxe:GridViewDataTextColumn VisibleIndex="2" Width="85%" FieldName="NAME" HeaderStyle-CssClass="colDisable">
                                    <DataItemTemplate>                        
                                        <%--<label style='<%#Eval("SLSPACE") %>' ><%#Eval("NAME") %></label>--%>
                                        <label class='<%#Eval("SLSPACE") %>'><%#Eval("NAME") %></label>
                                    </DataItemTemplate> 
                                    <%--<Settings AllowAutoFilterTextInputTimer="False" />--%>
                                    <%--<HeaderTemplate><span>Particulars</span></HeaderTemplate>--%>
                                    <EditFormSettings Visible="False"></EditFormSettings>
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="SCHEDULE_NUMBER" Caption="Schedule" Width="7%" VisibleIndex="3" Settings-AllowAutoFilter="False" HeaderStyle-CssClass="colDisable">
                                    <PropertiesTextEdit Style-HorizontalAlign="Center"></PropertiesTextEdit>
                                    <CellStyle HorizontalAlign="Center"></CellStyle>
                                    <HeaderStyle HorizontalAlign="Center" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="OPENING1" Caption="Opening1" Width="15%" VisibleIndex="4" Settings-AllowAutoFilter="False" HeaderStyle-CssClass="colDisable">                                    
                                    <PropertiesTextEdit Style-HorizontalAlign="Right" DisplayFormatString="0.00"></PropertiesTextEdit>
                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="OPENING2" Caption="Opening2" Width="15%" VisibleIndex="5" Settings-AllowAutoFilter="False" HeaderStyle-CssClass="colDisable">                                    
                                    <PropertiesTextEdit Style-HorizontalAlign="Right" DisplayFormatString="0.00"></PropertiesTextEdit>
                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>

                                 <dxe:GridViewDataTextColumn FieldName="SCHEDULE_ID" Caption="Schedule Id" Width="0%" VisibleIndex="6" HeaderStyle-CssClass="colDisable"/>
                            </columns>
                        <settingsbehavior confirmdelete="true" enablecustomizationwindow="true" enablerowhottrack="true" />
                        <settings showfooter="true" showgrouppanel="false" showgroupfooter="VisibleIfExpanded" />
                        <settingsediting mode="EditForm" />
                        <settingscontextmenu enabled="true" />
                        <settingsbehavior AutoExpandAllGroups="true" AllowSort="False" columnresizemode="Control" />
                        <settings showgrouppanel="false" showstatusbar="Visible" showfilterrow="true" />
                        <settingssearchpanel visible="False" />
                        <settingspager mode="ShowAllRecords"></settingspager>
                        <settings showfilterrow="True" showfilterrowmenu="true" showstatusbar="Visible" usefixedtablelayout="true" />
                        <clientsideevents endcallback="Callback1_EndCallback" />
                    </dxe:ASPxGridView>
                    <dx:LinqServerModeDataSource ID="GenerateEntityServerModeDataSource" runat="server" OnSelecting="GenerateEntityServerModeDataSource_Selecting"
                        ContextTypeName="ReportSourceDataContext" TableName="BALANCESHEETSCHEDULEVI_REPORT" />
                </div>
            </td>
        </tr>
    </table>
    </div>

    <dxe:ASPxPopupControl ID="popup" runat="server" ClientInstanceName="cpopup2ndLevel"
        Width="1100px" Height="600px" ScrollBars="Vertical" HeaderText="Consolidated Balance Sheet - Ledger Detailed" PopupHorizontalAlign="WindowCenter" HeaderStyle-CssClass="wht"
        PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
        ContentStyle-CssClass="pad">
        <clientsideevents closing="function(s, e) {
	        closePopup(s, e);}" />
        <contentstyle verticalalign="Top" cssclass="pad">
            </contentstyle>
        <contentcollection>
                <dxe:PopupControlContentControl runat="server">
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
                                        <asp:Label ID="lblAsOnDate2ndLevel" runat="Server" Font-Bold="true"  CssClass="mylabel1"></asp:Label>
                                    </label>
                                    <label style="padding-left: 20px;color: #b5285f; font-weight: bold; margin-top: 5px;" class="clsTo">
                                        <asp:Label ID="lblUnit2ndLevel" runat="Server" Font-Bold="true"  CssClass="mylabel1"></asp:Label>
                                    </label>
                                    <span style="padding-left: 20px;color: #b5285f; display: inline-block"><strong>Press < Esc > Key to Close</strong></span>
                                </td>                                
                            </tr>
                        </table>
                            <div class="pull-right" style="padding-top: 26px;">
                                
                                <asp:DropDownList ID="ddlExport2" runat="server" CssClass="btn btn-sm btn-primary" AutoPostBack="true" OnSelectedIndexChanged="ddlExport2_SelectedIndexChanged">
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
                        <dxe:ASPxGridView runat="server" ID="ShowGridDetails2Level" ClientInstanceName="cShowGridDetails2Level" KeyFieldName="SL" Width="100%" EnableRowsCache="false" AutoGenerateColumns="False"
                              KeyboardSupport="true" Settings-HorizontalScrollBarMode="Visible" OnDataBound="ShowGridDetails2Level_DataBound" 
                            OnSummaryDisplayText="ShowGridDetails2Level_SummaryDisplayText" DataSourceID="GenerateEntityServerModeDetailsL1DataSource" >

                            <Columns>
                                <dxe:GridViewDataTextColumn FieldName="BRANCHDESC" Caption="Unit" Width="30%" VisibleIndex="1" Settings-AllowAutoFilter="False"/>

                                <dxe:GridViewDataTextColumn FieldName="LEDGERNAME" Caption="Ledger" Width="30%" VisibleIndex="2" Settings-AllowAutoFilter="False"/>

                                <dxe:GridViewDataTextColumn FieldName="OPDB" Caption="Opening (Dr.)" Width="0%" VisibleIndex="3" PropertiesTextEdit-DisplayFormatString="0.00" Settings-AllowAutoFilter="False">
                                <CellStyle HorizontalAlign="Right"></CellStyle>
                                <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>
                           
                                <dxe:GridViewDataTextColumn FieldName="OPCR" Caption="Opening (Cr.)" Width="0%" VisibleIndex="4" PropertiesTextEdit-DisplayFormatString="0.00" Settings-AllowAutoFilter="False">
                                <CellStyle HorizontalAlign="Right"></CellStyle>
                                <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>
                                
                                <dxe:GridViewDataTextColumn FieldName="PRDB" Caption="Period (Dr.)" Width="15%" VisibleIndex="5" PropertiesTextEdit-DisplayFormatString="0.00" Settings-AllowAutoFilter="False">
                                <CellStyle HorizontalAlign="Right"></CellStyle>
                                <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="PRCR" Caption="Period (Cr.)" Width="15%" VisibleIndex="6" PropertiesTextEdit-DisplayFormatString="0.00" Settings-AllowAutoFilter="False">
                                <CellStyle HorizontalAlign="Right"></CellStyle>
                                <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="CLDB" Caption="Closing (Dr.)" Width="15%" VisibleIndex="7" PropertiesTextEdit-DisplayFormatString="0.00" Settings-AllowAutoFilter="False">
                                <CellStyle HorizontalAlign="Right"></CellStyle>
                                <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="CLCR" Caption="Closing (Cr.)" Width="15%" VisibleIndex="8" PropertiesTextEdit-DisplayFormatString="0.00" Settings-AllowAutoFilter="False">
                                <CellStyle HorizontalAlign="Right"></CellStyle>
                                <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="BRANCHID" Caption="Branch Id" Width="0%" VisibleIndex="9">
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="MAINACCOUNT_REFERENCEID" Caption="Ledger Id" Width="0%" VisibleIndex="10">
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
                                <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100" />
                            </SettingsPager>
                            <TotalSummary>
                                 <dxe:ASPxSummaryItem FieldName="OPDB" SummaryType="Sum"/>
                                 <dxe:ASPxSummaryItem FieldName="OPCR" SummaryType="Sum"/>
                                 <dxe:ASPxSummaryItem FieldName="PRDB" SummaryType="Sum"/>
                                 <dxe:ASPxSummaryItem FieldName="PRCR" SummaryType="Sum"/>
                                 <dxe:ASPxSummaryItem FieldName="CLDB" SummaryType="Sum" />
                                 <dxe:ASPxSummaryItem FieldName="CLCR" SummaryType="Sum" />
                            </TotalSummary>
                            <clientsideevents endcallback="Callback2_EndCallback" />
                        </dxe:ASPxGridView>
                         <dx:LinqServerModeDataSource ID="GenerateEntityServerModeDetailsL1DataSource" runat="server" OnSelecting="GenerateEntityServerModeDetailsL1DataSource_Selecting"
                        ContextTypeName="ReportSourceDataContext" TableName="BALANCESHEETSCHEDULEVIL1_REPORT" />
                    </div>
                </dxe:PopupControlContentControl>
            </contentcollection>
    </dxe:ASPxPopupControl>

    <dxe:ASPxPopupControl ID="popup2" runat="server" ClientInstanceName="cpopup3rdLevel"
        Width="1400px" Height="600px" ScrollBars="Vertical" HeaderText="Consolidated Balance Sheet - Document Detailed" PopupHorizontalAlign="WindowCenter" HeaderStyle-CssClass="wht"
        PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
        ContentStyle-CssClass="pad">
        <clientsideevents closing="function(s, e) {
	        closePopup2(s, e);}" />
        <contentstyle verticalalign="Top" cssclass="pad">
            </contentstyle>
        <contentcollection>
                <dxe:PopupControlContentControl runat="server">
                    <div class="col-md-12">
                        <div class="row clearfix">
                            <table class="pdbot" style="margin: 4px 0 16px 10px; float: left;">
                                <tr>
                                    <td style="padding-top: 10px">
                                        <label style="color: #b5285f; font-weight: bold; margin-top: 5px;" class="clsTo">
                                            <asp:Label ID="Label6" runat="Server" CssClass="mylabel1" ClientInstanceName="clblBranch3rdLevel"></asp:Label>
                                        </label>
                                    </td>
                                    <td style="padding-top: 10px">
                                        <dxe:ASPxTextBox ID="txtBranch3rdLevel" ClientInstanceName="ctxtBranch3rdLevel" runat="server" ReadOnly="true" Width="600px"></dxe:ASPxTextBox>
                                    </td>
                                </tr>
                                
                                <tr>
                                    <td style="padding-top: 10px">
                                        <label style="color: #b5285f; font-weight: bold; margin-top: 5px;" class="clsTo">
                                            <asp:Label ID="Label5" runat="Server" CssClass="mylabel1" ClientInstanceName="clblLedger3rdLevel"></asp:Label>
                                        </label>
                                    </td>
                                    <td style="padding-top: 10px">
                                        <dxe:ASPxTextBox ID="txtLedger3rdLevel" ClientInstanceName="ctxtLedger3rdLevel" runat="server" ReadOnly="true" Width="600px"></dxe:ASPxTextBox>
                                    </td>
                                </tr>

                                <tr>
                                    <td></td>
                                    <td>
                                        <label style="color: #b5285f; font-weight: bold; margin-top: 5px;" class="clsTo">
                                            <asp:Label ID="lblAsOnDate3rdLevel" runat="Server" Font-Bold="true"  CssClass="mylabel1"></asp:Label>
                                        </label>
                                        <label style="padding-left: 20px;color: #b5285f; font-weight: bold; margin-top: 5px;" class="clsTo">
                                            <asp:Label ID="lblUnit3rdLevel" runat="Server" Font-Bold="true"  CssClass="mylabel1"></asp:Label>
                                        </label>
                                        <span style="padding-left: 20px;color: #b5285f; display: inline-block"><strong>Press < Esc > Key to Close</strong></span>
                                    </td>
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
                    <div class="clearfix">
                        <dxe:ASPxGridView runat="server" ID="ShowGridDetails3Level" ClientInstanceName="cShowGridDetails3Level" KeyFieldName="SEQ" Width="100%" EnableRowsCache="false" AutoGenerateColumns="False"
                              KeyboardSupport="true" Settings-HorizontalScrollBarMode="Visible" OnDataBound="ShowGridDetails3Level_DataBound" 
                            OnSummaryDisplayText="ShowGridDetails3Level_SummaryDisplayText" DataSourceID="GenerateEntityServerModeDetailsL2DataSource" >
                            <Columns>
                                <%--<dxe:GridViewDataTextColumn FieldName="BRANCH_DESC" Caption="Unit" Width="30%" VisibleIndex="1" Settings-AllowAutoFilter="False"/>--%>

                                <dxe:GridViewDataTextColumn VisibleIndex="1" FieldName="DOC_NO" Caption="Document No." Width="20%">
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

                                <dxe:GridViewDataTextColumn FieldName="DOC_DATE" Caption="Document Date" Width="13%" VisibleIndex="2" Settings-AllowAutoFilter="False"/>

                                <dxe:GridViewDataTextColumn FieldName="DOC_TYPE" Caption="Document Type" Width="20%" VisibleIndex="3" Settings-AllowAutoFilter="False"/>

                                <dxe:GridViewDataTextColumn FieldName="PARTY_NAME" Caption="Party" Width="40%" VisibleIndex="4" />

                                <dxe:GridViewDataTextColumn FieldName="OP_DR_AMT" Caption="Opening (Dr.)" Width="15%" VisibleIndex="5" PropertiesTextEdit-DisplayFormatString="0.00" Settings-AllowAutoFilter="False">
                                <CellStyle HorizontalAlign="Right"></CellStyle>
                                <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>
                           
                                <dxe:GridViewDataTextColumn FieldName="OP_CR_AMT" Caption="Opening (Cr.)" Width="15%" VisibleIndex="6" PropertiesTextEdit-DisplayFormatString="0.00" Settings-AllowAutoFilter="False">
                                <CellStyle HorizontalAlign="Right"></CellStyle>
                                <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>
                                
                                <dxe:GridViewDataTextColumn FieldName="PR_DR_AMT" Caption="Period (Dr.)" Width="15%" VisibleIndex="7" PropertiesTextEdit-DisplayFormatString="0.00" Settings-AllowAutoFilter="False">
                                <CellStyle HorizontalAlign="Right"></CellStyle>
                                <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="PR_CR_AMT" Caption="Period (Cr.)" Width="15%" VisibleIndex="8" PropertiesTextEdit-DisplayFormatString="0.00" Settings-AllowAutoFilter="False">
                                <CellStyle HorizontalAlign="Right"></CellStyle>
                                <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="CL_DR_AMT" Caption="Closing (Dr.)" Width="15%" VisibleIndex="9" PropertiesTextEdit-DisplayFormatString="0.00" Settings-AllowAutoFilter="False">
                                <CellStyle HorizontalAlign="Right"></CellStyle>
                                <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="CL_CR_AMT" Caption="Closing (Cr.)" Width="15%" VisibleIndex="10" PropertiesTextEdit-DisplayFormatString="0.00" Settings-AllowAutoFilter="False">
                                <CellStyle HorizontalAlign="Right"></CellStyle>
                                <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="DOC_ID" Caption="DOC_ID" Width="0%" VisibleIndex="11">
                                <CellStyle HorizontalAlign="Right"></CellStyle>
                                <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="MODULE_TYPE" Caption="MODULE_TYPE" Width="0%" VisibleIndex="12">
                                </dxe:GridViewDataTextColumn>

                                 <dxe:GridViewDataTextColumn FieldName="LEDGER_ID" Caption="LEDGER_ID" Width="0%" VisibleIndex="13">
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
                                <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100" />
                            </SettingsPager>
                            <TotalSummary>
                                 <dxe:ASPxSummaryItem FieldName="OP_DR_AMT" SummaryType="Sum"/>
                                 <dxe:ASPxSummaryItem FieldName="OP_CR_AMT" SummaryType="Sum"/>
                                 <dxe:ASPxSummaryItem FieldName="PR_DR_AMT" SummaryType="Sum"/>
                                 <dxe:ASPxSummaryItem FieldName="PR_CR_AMT" SummaryType="Sum"/>
                                 <dxe:ASPxSummaryItem FieldName="CL_DR_AMT" SummaryType="Sum" />
                                 <dxe:ASPxSummaryItem FieldName="CL_CR_AMT" SummaryType="Sum" />
                            </TotalSummary>
                            <clientsideevents endcallback="Callback3_EndCallback" />
                        </dxe:ASPxGridView>
                         <dx:LinqServerModeDataSource ID="GenerateEntityServerModeDetailsL2DataSource" runat="server" OnSelecting="GenerateEntityServerModeDetailsL2DataSource_Selecting"
                        ContextTypeName="ReportSourceDataContext" TableName="BALANCESHEETSCHEDULEVIL2_REPORT" />
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
   
    <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
    </dxe:ASPxGridViewExporter>

    <dxe:ASPxGridViewExporter ID="exporterDetails" runat="server" Landscape="true" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
    </dxe:ASPxGridViewExporter>

    <dxe:ASPxCallbackPanel runat="server" ID="CallbackPanel" ClientInstanceName="cCallbackPanel" OnCallback="CallbackPanel_Callback">
        <panelcollection>
            <dxe:PanelContent runat="server">
                <asp:HiddenField ID="hfIsBSScheduleVIFilter" runat="server" />
            </dxe:PanelContent>
        </panelcollection>
        <clientsideevents endcallback="CallbackPanelEndCall" />
    </dxe:ASPxCallbackPanel>

    <dxe:ASPxCallbackPanel runat="server" ID="CallbackPanelDetailL1" ClientInstanceName="cCallbackPanelDetailL1" OnCallback="CallbackPanelDetailL1_Callback">
        <panelcollection>
            <dxe:PanelContent runat="server">
                <asp:HiddenField ID="hfIsBSScheduleVIL1Filter" runat="server" />
            </dxe:PanelContent>
        </panelcollection>
        <clientsideevents endcallback="CallbackPanelDetailL1EndCall" />
    </dxe:ASPxCallbackPanel>

    <dxe:ASPxCallbackPanel runat="server" ID="CallbackPanelDetailL2" ClientInstanceName="cCallbackPanelDetailL2" OnCallback="CallbackPanelDetailL2_Callback">
        <panelcollection>
            <dxe:PanelContent runat="server">
                <asp:HiddenField ID="hfIsBSScheduleVIL2Filter" runat="server" />
            </dxe:PanelContent>
        </panelcollection>
        <clientsideevents endcallback="CallbackPanelDetailL2EndCall" />
    </dxe:ASPxCallbackPanel>
</asp:Content>
