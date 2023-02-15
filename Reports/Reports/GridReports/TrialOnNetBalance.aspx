<%--================================================== Revision History ============================================
Rev Number         DATE              VERSION          DEVELOPER           CHANGES
1.0                15-02-2023        V2.0.36           Pallab              25575 : Report pages design modification
====================================================== Revision History ================================================--%>

<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="TrialOnNetBalance.aspx.cs" Inherits="Reports.Reports.GridReports.TrialOnNetBalance" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
     Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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
        #ListBoxBranches {
            width: 200px;
        }
        .hide {
            display: none;
        }
        .dxtc-activeTab .dxtc-link {
            color: #fff !important;
        }
        .pad32 {
        padding-left:32px;
        }
        .pad52 {
        padding-left:52px;
        }
        .makebold label{
        font-weight:bold !important;
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
            right: 74%;
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
        //for Esc
        document.onkeyup = function (e) {
            if (event.keyCode == 27 && cpopup2ndLevel.GetVisible() == true && popupdocument.GetVisible() == false) { //run code for alt+N -- ie, Save & New  
                popupHide();
            }
            else if (event.keyCode == 27 && cpopup2ndLevelStock.GetVisible() == true && popupdocument.GetVisible() == false && cpopupStkValDet.GetVisible() == false) { //run code for alt+N -- ie, Save & New  
                popup2Hide();
            }
            else if (event.keyCode == 27 && cpopup2ndLevelStock.GetVisible() == true && cpopupStkValDet.GetVisible() == true) { //run code for alt+N -- ie, Save & New  
                popupStkDetHide();
            }
            else if (event.keyCode == 27 && popupdocument.GetVisible() == true) { //run code for alt+N -- ie, Save & New  
                popupHide2();
            }
        }

        function popupHide(s, e) {
            cpopup2ndLevel.Hide();
            Grid.Focus();
            $("#drdExport").val(0);
        }

        function popup2Hide(s, e) {
            cpopup2ndLevelStock.Hide();
            Grid.Focus();
            $("#drdExport").val(0);
        }

        function popupHide2(s, e) {
            popupdocument.Hide();
            cShowGridDetails2Level.Focus();
            $("#drdExport").val(0);
        }

        function popupStkDetHide(s, e) {
            cpopupStkValDet.Hide();
            cShowGridDetails2Level.Focus();
            $("#ddlExport2").val(0);
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
                if (Amount != 0) {
                    loadCurrencyMassage.style.display = "block";
                }
                else {
                    loadCurrencyMassage.style.display = "none";
                }
                $("#drdExport").val(0);
                Grid.Focus();
                Grid.SetFocusedRowIndex(0);
            }
            Grid.cpErrorFinancial = null;
        }
        var AsonWise = false;
        var Showparrentgrp = false;
        var Hierarchical = false;
        var Showgrp = false;
        var Shownetdrcropcl = false;
        var ason = 'Y';
        $(document).ready(function () {
            document.getElementById("lblToDate").innerHTML = 'As On Date :'
            document.getElementById("dvFromdate").style.display = "none";
            document.getElementById("ASPxToDate").style.visibility = "visible";
            AsonWise = true;
            Showparrentgrp = false;
            Hierarchical = false;
            Showgrp = false;
            Shownetdrcropcl = false;
        })

        function btn_ShowRecordsClick(e) {
            var data = "Grid";
            $("#hfIsTrialOnNetBalFilter").val("Y");
            $("#drdExport").val(0);
            var branchid = $('#ddlbranchHO').val();
            GetObjectID('HDShowOrPrintClick').value = "Show";
            if (gridbranchLookup.GetValue() == null) {
                jAlert('Please select atleast one Branch for generate the report.');
            }
            else {
                cCallbackPanel.PerformCallback(data + '~' + AsonWise + '~' + branchid);
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

        function btn_PrintRecordsClick(e) {
            var data = "Print";
            $("#drdExport").val(0);
            var branchid = $('#ddlbranchHO').val();
            GetObjectID('HDShowOrPrintClick').value = "Print";
            if (gridbranchLookup.GetValue() == null) {
                jAlert('Please select atleast one Branch for generate the report.');
            }
            else {
                cCallbackPanel.PerformCallback(data + '~' + AsonWise + '~' + branchid);
            }
        }

        function CallbackPanelEndCall(s, e) {
            document.getElementById('<%=CompBranch.ClientID %>').innerHTML = cCallbackPanel.cpBranchNames
            var HDShwOrPrntClck = document.getElementById('HDShowOrPrintClick').value;
            if (cCallbackPanel.cpPrintUrl != "") {
                var parameter;
                parameter = cCallbackPanel.cpPrintUrl;
                var reportName = parameter.split("\\")[0];
                var FROMDATE = parameter.split("\\")[1];
                var TODATE = parameter.split("\\")[2];
                var BRANCH_ID = parameter.split("\\")[3];
                var asondate = parameter.split("\\")[4];
                var chkzerobal = parameter.split("\\")[5];
                var Hobran = parameter.split("\\")[6];
                var GroupList = parameter.split("\\")[7];
                var chkCLStk = parameter.split("\\")[8];
                var Valtype = parameter.split("\\")[9];
                var Owmastvaltech = parameter.split("\\")[10];
                var PGrp = parameter.split("\\")[11];
                var Hierchy = parameter.split("\\")[12];
                var ShwGrp = parameter.split("\\")[13];
                var ShwNetDrCrOpCl = parameter.split("\\")[14];
                var OpAsOnDt = parameter.split("\\")[15];
                //var RptModuleName = parameter.split("\\")[16];
                //var RptModuleName = parameter.split("\\")[17];
                var ConsLandCost = parameter.split("\\")[16];
                var ConsOverheadCost = parameter.split("\\")[17];
                var RptModuleName = parameter.split("\\")[18];
                //window.location.href = "../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + reportName + "&&StartDate=" + FROMDATE + "&&EndDate=" + TODATE + "&&BranId=" + BRANCH_ID + "&&Ason=" + asondate + "&&Zerobal=" + chkzerobal + "&&Ho=" + Hobran + "&&GrpLst=" + GroupList + "&&ClStk=" + chkCLStk + "&&Valtech=" + Valtype + "&&Owmvt=" + Owmastvaltech + "&&PG=" + PGrp + "&&Hrchy=" + Hierchy + "&&SwGrp=" + ShwGrp + "&&NDrcrOpCl=" + ShwNetDrCrOpCl + "&&OpAsonDt=" + OpAsOnDt + "&&reportname=" + RptModuleName;
                //window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + reportName + "&&StartDate=" + FROMDATE + "&&EndDate=" + TODATE + "&&BranId=" + BRANCH_ID + "&&Ason=" + asondate + "&&Zerobal=" + chkzerobal + "&&Ho=" + Hobran + "&&GrpLst=" + GroupList + "&&ClStk=" + chkCLStk + "&&Valtech=" + Valtype + "&&Owmvt=" + Owmastvaltech + "&&PG=" + PGrp + "&&Hrchy=" + Hierchy + "&&SwGrp=" + ShwGrp + "&&NDrcrOpCl=" + ShwNetDrCrOpCl + "&&OpAsonDt=" + OpAsOnDt + "&&reportname=" + RptModuleName, '_blank')
                //window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + reportName + "&&StartDate=" + FROMDATE + "&&EndDate=" + TODATE + "&&BranId=" + BRANCH_ID + "&&Ason=" + asondate + "&&Zerobal=" + chkzerobal + "&&Ho=" + Hobran + "&&GrpLst=" + GroupList + "&&ClStk=" + chkCLStk + "&&Valtech=" + Valtype + "&&Owmvt=" + Owmastvaltech + "&&PG=" + PGrp + "&&Hrchy=" + Hierchy + "&&SwGrp=" + ShwGrp + "&&NDrcrOpCl=" + ShwNetDrCrOpCl + "&&OpAsonDt=" + OpAsOnDt + "&&ConsLandCost=" + ConsLandCost + "&&reportname=" + RptModuleName, '_blank')
                window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + reportName + "&&StartDate=" + FROMDATE + "&&EndDate=" + TODATE + "&&BranId=" + BRANCH_ID + "&&Ason=" + asondate + "&&Zerobal=" + chkzerobal + "&&Ho=" + Hobran + "&&GrpLst=" + GroupList + "&&ClStk=" + chkCLStk + "&&Valtech=" + Valtype + "&&Owmvt=" + Owmastvaltech + "&&PG=" + PGrp + "&&Hrchy=" + Hierchy + "&&SwGrp=" + ShwGrp + "&&NDrcrOpCl=" + ShwNetDrCrOpCl + "&&OpAsonDt=" + OpAsOnDt + "&&ConsLandCost=" + ConsLandCost + "&&ConsOverheadCost=" + ConsOverheadCost + "&&reportname=" + RptModuleName, '_blank')
            }
            if (HDShwOrPrntClck == "Show") {
                Grid.Refresh();
            }
        }

        function CallbackPanelDetailEndCall(s, e) {
            cShowGridDetails2Level.Focus();
            cShowGridDetails2Level.SetFocusedRowIndex(0);
            ctxtLedger2ndLevel.SetText(cCallbackPanel2.cpLedger);
            if (document.getElementById('radAsDate').checked) {
                $("#lblFromDate2ndLevel")[0].innerHTML = "As on : " + cCallbackPanel2.cpToDate;
                $("#lblToDate2ndLevel")[0].innerHTML = "";
            }
            else if (document.getElementById('radPeriod').checked) {
                $("#lblFromDate2ndLevel")[0].innerHTML = "From " + cCallbackPanel2.cpFromDate;
                $("#lblToDate2ndLevel")[0].innerHTML = " To " + cCallbackPanel2.cpToDate;
            }
            if (cCallbackPanel2.cpLedgerType == "LEDG") {
                $("#Label3")[0].innerHTML = "Ledger :";
            }
            else if (cCallbackPanel2.cpLedgerType == "BRAN") {
                $("#Label3")[0].innerHTML = "Branch :";
            }
            cShowGridDetails2Level.Refresh();
        }

        function CallbackPanelDetail2EndCall(s, e) {
            cShowGridDetails3Level.Focus();
            cShowGridDetails3Level.SetFocusedRowIndex(0);
            ctxtLedger2ndLevelStock.SetText(cCallbackPanel3.cpLedger);
            if (document.getElementById('radAsDate').checked) {
                $("#lblFromDate2ndLevelStock")[0].innerHTML = "As on : " + cCallbackPanel3.cpToDate;
                $("#lblToDate2ndLevelStock")[0].innerHTML = "";
            }
            else if (document.getElementById('radPeriod').checked) {
                $("#lblFromDate2ndLevelStock")[0].innerHTML = "From " + cCallbackPanel3.cpFromDate;
                $("#lblToDate2ndLevelStock")[0].innerHTML = " To " + cCallbackPanel3.cpToDate;
            }
            if (cCallbackPanel3.cpLedgerType == "STKV") {
                $("#Label5")[0].innerHTML = "Ledger :";
            }
            cShowGridDetails3Level.Refresh();
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

        //function OnGetRowValuesCallback(values) {
        //    alert(values);
        //}

        function OpenStockDetails(branch, prodid) {
            if (AsonWise == false) {
                ason = 'N';
            }
            else {
                ason = 'Y';
            }
            $("#hfIsTrialOnNetBalStkValDetFilter").val("Y");
            cCallbackPanelStockDetail.PerformCallback('BndPopupgrid~' + branch + '~' + prodid + '~' + ason);
            cpopupStkValDet.Show();
            return true;
        }

        function popupStkDetHide(s, e) {
            cpopupStkValDet.Hide();
        }

        function CallbackPanelStockDetEndCall(s, e) {
            cgridValuationdetails.Refresh();
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
                document.getElementById("lblToDate").innerHTML = 'As On Date :'
                document.getElementById("dvFromdate").style.display = "none";
                document.getElementById("ASPxToDate").style.visibility = "visible";
                CchkHierarchy.SetEnabled(true);
                CchkGroup.SetEnabled(true);
                CchkOpAsOnDt.SetEnabled(true);
                CchkNetDrCrOpCl.SetEnabled(false);
                CchkNetDrCrOpCl.SetCheckState('UnChecked');
                CchkPGRP.SetEnabled(true);
                AsonWise = true;
            }
            else {
                document.getElementById("lblToDate").style.visibility = "visible";
                document.getElementById("lblToDate").innerHTML = 'To Date :'
                document.getElementById("dvFromdate").style.display = "block";
                document.getElementById("ASPxToDate").style.visibility = "visible";
                CchkHierarchy.SetCheckState('UnChecked');
                CchkHierarchy.SetEnabled(false);
                CchkGroup.SetCheckState('UnChecked');
                CchkGroup.SetEnabled(false);
                CchkOpAsOnDt.SetCheckState('UnChecked');
                CchkOpAsOnDt.SetEnabled(false);
                CchkNetDrCrOpCl.SetEnabled(true);
                CchkPGRP.SetEnabled(true);
                AsonWise = false;
            }
        }

        function CheckConsCLStk(s, e) {
            if (s.GetCheckState() == 'Checked') {
                CchkOWMSTVT.SetEnabled(true);
                CchkConsLandCost.SetEnabled(true);
                CchkConsOverHeadCost.SetEnabled(true)
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

        function CheckConsHierarchy(s, e) {
            if (s.GetCheckState() == 'Checked') {
                //CchkGroup.SetEnabled(false);
                CchkGroup.SetCheckState('UnChecked');
                CchkPGRP.SetEnabled(false);
                CchkPGRP.SetCheckState('UnChecked');
            }
            else {
                CchkGroup.SetEnabled(true);
                CchkPGRP.SetEnabled(true);
            }
        }

        function CheckConsGroup(s, e) {
            if (s.GetCheckState() == 'Checked') {
                //CchkHierarchy.SetEnabled(false);
                CchkHierarchy.SetCheckState('UnChecked');
                CchkPGRP.SetEnabled(false);
                CchkPGRP.SetCheckState('UnChecked');
            }
            else {
                CchkHierarchy.SetEnabled(true);
                CchkPGRP.SetEnabled(true);
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

        //function BudgetAfterHide(s, e) {
        //    popupbudget.Hide();
        //}

        //function CloseGridQuotationLookup() {
        //    gridquotationLookup.ConfirmCurrentSelection();
        //    gridquotationLookup.HideDropDown();
        //    gridquotationLookup.Focus();
        //}
        function CloseGridLookupbranch() {
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
            clookupGroup.gridView.SelectRows();
        }
        function unselectAll() {
            clookupGroup.gridView.UnselectRows();
        }
        function CloseLookup() {
            clookupGroup.ConfirmCurrentSelection();
            clookupGroup.HideDropDown();
            clookupGroup.Focus();
        }
    </script>
    <%--for 2nd level popup--%>
    <script type="text/javascript">
        function OnWaitingGridKeyPress(e) {
            if (e.key == "Enter" || e.key == "NumpadEnter") {
                var index = Grid.GetFocusedRowIndex();
                var LedgerShortName = "";
                if (AsonWise == false) {
                    ason = 'N';
                }
                else {
                    ason = 'Y';
                }
                if (CchkPGRP.GetCheckState() == 'Checked') {
                    Showparrentgrp = true;
                }
                else if (CchkPGRP.GetCheckState() == 'Unchecked') {
                    Showparrentgrp = false;
                }
                if (CchkNetDrCrOpCl.GetCheckState() == 'Checked')
                {
                    Shownetdrcropcl = true;
                }
                else if (CchkNetDrCrOpCl.GetCheckState() == 'Unchecked')
                {
                    Shownetdrcropcl = false;
                }
                if (CchkHierarchy.GetCheckState() == 'Checked') {
                    Hierarchical = true;
                }
                else if (CchkHierarchy.GetCheckState() == 'Unchecked') {
                    Hierarchical = false;
                }
                if (CchkGroup.GetCheckState() == 'Checked') {
                    Showgrp = true;
                }
                else if (CchkGroup.GetCheckState() == 'Unchecked') {
                    Showgrp = false;
                }

                var index = Grid.GetFocusedRowIndex();
                var ledger = Grid.GetRow(index).children[0].innerHTML
                var branchid = $('#ddlbranchHO').val();
                var ledgertype = Grid.GetRow(index).children[1].innerHTML
                if (AsonWise == false && Showparrentgrp == false && Shownetdrcropcl == false) {
                    LedgerShortName = Grid.GetRow(index).children[8].children[1].innerText
                }
                else if (AsonWise == false && Showparrentgrp == true && Shownetdrcropcl == false) {
                    LedgerShortName = Grid.GetRow(index).children[9].children[1].innerText
                }
                else if (AsonWise == false && Showparrentgrp == true && Shownetdrcropcl == true) {
                    LedgerShortName = Grid.GetRow(index).children[8].children[1].innerText
                }
                else if (AsonWise == false && Showparrentgrp == false && Shownetdrcropcl == true) {
                    LedgerShortName = Grid.GetRow(index).children[7].children[1].innerText
                }
                else if (AsonWise == true && Showparrentgrp == false && Hierarchical == false && Showgrp == false) {
                    LedgerShortName = Grid.GetRow(index).children[4].children[1].innerText
                }
                else if (AsonWise == true && Showparrentgrp == true && Hierarchical == false && Showgrp == false) {
                    LedgerShortName = Grid.GetRow(index).children[5].children[1].innerText
                }
                else if (AsonWise == true && Hierarchical == true && Showgrp == false && Showparrentgrp == false) {
                    LedgerShortName = "Hierarchical"
                }
                else if (AsonWise == true && Showgrp == true && Hierarchical == false && Showparrentgrp == false) {
                    LedgerShortName = "Group"
                }

                if (LedgerShortName.trim() == "SYSTM00007" || LedgerShortName.trim() == "SYSTM00008") {
                    $("#hfIsTrialOnNetBalDet2Filter").val("Y");
                }
                else {
                    $("#hfIsTrialOnNetBalDetFilter").val("Y");
                }                
               
                if (LedgerShortName.trim() == "SYSTM00010") {
                    jAlert('Zooming from Suspense is not possible.');
                    cpopup2ndLevel.Hide();
                    cpopup2ndLevelStock.Hide();
                }
                else if (LedgerShortName.trim() == "Hierarchical") {
                    jAlert('Zooming from Hierarchical is not possible.');
                    cpopup2ndLevel.Hide();
                    cpopup2ndLevelStock.Hide();
                }
                else if (LedgerShortName.trim() == "Group") {
                    jAlert('Zooming from Group is not possible.');
                    cpopup2ndLevel.Hide();
                    cpopup2ndLevelStock.Hide();
                }
                else {
                    if (LedgerShortName.trim() == "SYSTM00007" || LedgerShortName.trim() == "SYSTM00008") {
                        cCallbackPanel3.PerformCallback(ledger + "~" + ason + "~" + branchid + "~" + ledgertype);
                        cpopup2ndLevelStock.Show();
                    }
                    else {
                        cCallbackPanel2.PerformCallback(ledger + "~" + ason + "~" + branchid + "~" + ledgertype);
                        cpopup2ndLevel.Show();
                    }
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
            var ledgertype = Grid.GetRow(index).children[1].innerHTML
            var branchid = $('#ddlbranchHO').val();
            if (LedgerShortName.trim() == "SYSTM00007" || LedgerShortName.trim() == "SYSTM00008") {
                $("#hfIsTrialOnNetBalDet2Filter").val("Y");
            }
            else {
                $("#hfIsTrialOnNetBalDetFilter").val("Y");
            }
            if (LedgerShortName.trim() == "SYSTM00007" || LedgerShortName.trim() == "SYSTM00008") {
                cCallbackPanel3.PerformCallback(ledger + "~" + ason + "~" + branchid + "~" + ledgertype);
                cpopup2ndLevelStock.Show();
            }
            else {
                cCallbackPanel2.PerformCallback(ledger + "~" + ason + "~" + branchid + "~" + ledgertype);
                cpopup2ndLevel.Show();
            }
            //cCallbackPanel2.PerformCallback(ledger + "~" + ason + "~" + branchid);
            //cpopup2ndLevel.Show();
        }

        function closePopup(s, e) {
            e.cancel = false;
            Grid.Focus();
            $("#ddlExport2").val(0);
        }

        function closePopup2(s, e) {
            e.cancel = false;
            Grid.Focus();
            $("#ddlExport3").val(0);
        }
    </script>
    <%--end 2nd level popup--%>

    <%--end 3rd level popup--%>
    <script type="text/javascript">
        //function OnWaitingGridKeyPress2(e) {
        //    if (e.code == "Enter" || e.code == "NumpadEnter") {
        //    }
        //}

        //function EndShowGridDetails3Level() {
        //    cShowGridDetails3Level.Focus();
        //    ctxtProductCode3rdLevel.SetText(ctxtProductCode2ndLevel.GetText());
        //    ctxtProductDesc3rdLevel.SetText(ctxtProductDesc2ndLevel.GetText());
        //    $("#lblFromDate3rdLevel")[0].innerHTML = $("#lblFromDate2ndLevel")[0].innerHTML;
        //    $("#lblToDate3rdLevel")[0].innerHTML = $("#lblToDate2ndLevel")[0].innerHTML;
        //}

        function OnGetRowValuesLvl3(Uniqueid, type, docno) {
            var url = '';
            if (type == 'POS') {
                url = '/OMS/Management/Activities/posSalesInvoice.aspx?key=' + Uniqueid + '&IsTagged=1&Viemode=1';
            }
            else if (type == 'SI') {
                url = '/OMS/Management/Activities/view/SalesInvoice.html?id=' + Uniqueid + '';
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
                url = '/OMS/Management/Activities/view/PurchaseInvoice.html?id=' + Uniqueid + '';
            }
            else if (type == 'VP' || type == 'VR') {
                url = '/OMS/Management/Activities/view/VendorReceiptPayment.html?id=' + Uniqueid + '';
            }
            else if (type == 'PR') {
                url = '/OMS/Management/Activities/PReturn.aspx?key=' + Uniqueid + '&IsTagged=1&req=V&type=PR';
            }
            else if (type == 'SC') {
                url = '/OMS/Management/Activities/CustomerReturn.aspx?key=' + Uniqueid + '&IsTagged=1&req=V&type=' + type;
            }
            else if (type == 'CP' || type == 'CR') {
                url = '/OMS/Management/Activities/view/CustomerReceiptPayment.html?id=' + Uniqueid + '';
            }
            else if (type == 'JV') {
                url = '/OMS/Management/dailytask/JournalEntry.aspx?key=' + Uniqueid + '&IsTagged=1&req=' + docno;
            }
            else if (type == 'CBV') {
                url = '/OMS/Management/Activities/view/CashBank.html?id=' + Uniqueid + '';
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
        table.dxeDisabled_PlasticBlue{
            width:100%;
        }

        /*Rev 1.0*/
        .outer-div-main {
            background: #ffffff;
            padding: 10px;
            border-radius: 10px;
            box-shadow: 1px 1px 10px #11111154;
        }

        .form_main {
            overflow: hidden;
        }

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
            right: 22px;
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

        table
        {
            max-width: 99% !important;
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
        <div class="clearfix" >
            <div class="col-md-2" style="padding-top: 23px;color: #141414; font-weight: bold;" >
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
                                                            <dxe:ASPxButton ID="Close" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseGridLookupbranch" UseSubmitBehavior="False" />
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
            

            <div class="col-md-2">
                <label style="color: #b5285f; font-weight: bold;" class="clsTo">
                    <asp:Label ID="Label4" runat="Server" Text="Group : " CssClass="mylabel1"
                        Width="92px" Font-Bold="True"></asp:Label>
                </label>
                <dxe:ASPxCallbackPanel runat="server" ID="GroupPanel" ClientInstanceName="cGroupPanel" OnCallback="GroupPanel_Callback">
                    <PanelCollection>
                        <dxe:PanelContent runat="server">
                            <dxe:ASPxGridLookup ID="lookupGroup" ClientInstanceName="clookupGroup" SelectionMode="Multiple" runat="server"
                                OnDataBinding="lookupGroup_DataBinding" KeyFieldName="ID" Width="100%" TextFormatString="{0}" AutoGenerateColumns="False" MultiTextSeparator=", ">
                                <Columns>
                                    <dxe:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Width="60" Caption=" " />
                                    <dxe:GridViewDataColumn FieldName="Name" Visible="true" VisibleIndex="1" Caption="Name" Settings-AutoFilterCondition="Contains" Width="250px">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dxe:GridViewDataColumn>
                                    <dxe:GridViewDataColumn FieldName="ID" Visible="true" VisibleIndex="2" Caption="ID" Settings-AutoFilterCondition="Contains" Width="0">
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
                                                            <dxe:ASPxButton ID="ASPxButton6" runat="server" AutoPostBack="false" Text="Select All" ClientSideEvents-Click="selectAll" />
                                                        <%--</div>--%>
                                                        <dxe:ASPxButton ID="ASPxButton7" runat="server" AutoPostBack="false" Text="Deselect All" ClientSideEvents-Click="unselectAll" />
                                                        <dxe:ASPxButton ID="ASPxButton8" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseLookup" UseSubmitBehavior="False" />
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
                <span id="MandatorClass" style="display: none" class="validclass" />
            </div>
            <div class="clear"></div>
            <%--Rev 1.0--%>
            <%--<div class="col-md-2" style="padding-top: 8px;color: #b5285f; font-weight: bold;">--%>
            <div class="col-md-2" style="padding-top: 20px;color: #141414; font-weight: bold;">
                <%--Rev end 1.0--%>
                <div style="padding-right: 10px; vertical-align: middle; padding-top: 6px">
                    <asp:CheckBox ID="chkZero" runat="server" Checked="false"/>
                    Show Zero Value Account
                </div>
            </div>
            <%--Rev 1.0--%>
            <%--<div class="col-md-2" style="margin-top:8px;color: #b5285f; font-weight: bold;">--%>
            <div class="col-md-2" style="margin-top:20px;color: #141414; font-weight: bold;">
                <%--Rev end 1.0--%>
             <dxe:ASPxCheckBox runat="server" ID="chkCLStk" Checked="false" Text="Consider Closing Stock" >
                 <ClientSideEvents CheckedChanged="CheckConsCLStk" />
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
            <%--<div class="col-md-3" style="margin-top:17px;color: #b5285f; font-weight: bold;">--%>
            <div class="col-md-3" style="margin-top:24px;color: #b5285f; font-weight: bold;">
                <%--Rev end 1.0--%>
                <dxe:ASPxCheckBox runat="server" ID="chkOWMSTVT" Checked="false" ClientEnabled="false" Text="Override Product Valuation Technique in Master" ClientInstanceName="CchkOWMSTVT">
                </dxe:ASPxCheckBox>
            </div>
            <%--Rev 1.0--%>
            <%--<div class="col-md-3" style="margin-top:17px;color: #b5285f; font-weight: bold;">--%>
            <div class="col-md-3" style="margin-top:24px;color: #b5285f; font-weight: bold;">
                <%--Rev end 1.0--%>
                <dxe:ASPxCheckBox runat="server" ID="chkConsLandCost" Checked="false" ClientEnabled="false" Text="Consider Landed Cost" ClientInstanceName="CchkConsLandCost">
                </dxe:ASPxCheckBox>
            </div>
            <div class="clear"></div>
             <div class="col-md-2" style="margin-top:8px;color: #b5285f; font-weight: bold;">
                <dxe:ASPxCheckBox runat="server" ID="chkConsOverHeadCost" Checked="false" ClientEnabled="false" Text="Consider Overhead Cost" ClientInstanceName="CchkConsOverHeadCost">
                </dxe:ASPxCheckBox>
            </div>
            <div class="col-md-2" style="margin-top:8px;color: #b5285f; font-weight: bold;">
                <dxe:ASPxCheckBox runat="server" ID="chkPGRP" Checked="false" Text="Show Parent Group" ClientInstanceName="CchkPGRP">
                </dxe:ASPxCheckBox>
            </div>
            <%--<div class="clear"></div> --%>
            <div class="col-md-2" style="margin-top:8px;color: #b5285f; font-weight: bold;">
                <dxe:ASPxCheckBox runat="server" ID="chkHierarchy" Checked="false" Text="Hierarchial" ClientInstanceName="CchkHierarchy">
                    <ClientSideEvents CheckedChanged="CheckConsHierarchy" />
                </dxe:ASPxCheckBox>
            </div>

            <div class="col-md-2" style="margin-top:8px;color: #b5285f; font-weight: bold;">
                <dxe:ASPxCheckBox runat="server" ID="chkGroup" Checked="false" Text="Group" ClientInstanceName="CchkGroup">
                    <ClientSideEvents CheckedChanged="CheckConsGroup" />
                </dxe:ASPxCheckBox>
            </div>

             <div class="col-md-3" style="margin-top:8px;color: #b5285f; font-weight: bold;">
                <dxe:ASPxCheckBox runat="server" ID="chkNetDrCrOpCl" Checked="false" ClientEnabled="false" Text="Show Net of Dr/Cr for Opening/Closing" ClientInstanceName="CchkNetDrCrOpCl">
                </dxe:ASPxCheckBox>
            </div>
            <div class="clear"></div>
            <%--Rev 1.0--%>
            <%--<div class="col-md-2" style="margin-top:8px;color: #b5285f; font-weight: bold;">--%>
            <div class="col-md-2" style="margin-top:16px;color: #b5285f; font-weight: bold;">
                <%--Rev end 1.0--%>
                <dxe:ASPxCheckBox runat="server" ID="chkOpAsOnDt" Checked="false" Text="Opening As on Date" ClientInstanceName="CchkOpAsOnDt">
                </dxe:ASPxCheckBox>
            </div>
            <%--<div class="clear"></div> --%>
            <%--Rev 1.0--%>
            <%--<div class="col-md-3" style="padding-top: 8px">--%>
            <div class="col-md-3" style="padding-top: 14px">
                <%--Rev end 1.0--%>
                <button id="btnShow" class="btn btn-success" type="button" onclick="btn_ShowRecordsClick(this);">Show</button>
                <% if (rights.CanPrint)
                       { %>
                        <button id="btnPrint" class="btn btn-info" type="button" onclick="btn_PrintRecordsClick(this);">Print</button>
                <% } %>
                 <%--<% if (rights.CanExport)
                       { %>--%>
                  <div class="hide">
                <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-sm btn-primary"
                    OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true" OnChange="if(!AvailableExportOption()){return false;}">
                    <asp:ListItem Value="0">Export to</asp:ListItem>
                    <asp:ListItem Value="1">XLSX</asp:ListItem>
                    <asp:ListItem Value="2">PDF</asp:ListItem>
                    <asp:ListItem Value="3">CSV</asp:ListItem>
                    <asp:ListItem Value="4">RTF</asp:ListItem>

                </asp:DropDownList>
                      </div>
                 <%--<% } %>--%>
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

        <table class="TableMain100" style="margin-top:10px">
            <tr>
                <td colspan="2">
                    <div onkeypress="OnWaitingGridKeyPress(event)">
                        <dxe:ASPxGridView runat="server" ID="ShowGrid" ClientInstanceName="Grid" Width="100%" EnableRowsCache="false" AutoGenerateColumns="False" KeyboardSupport="true" KeyFieldName="SEQ"
                             DataSourceID="GenerateEntityServerModeDataSource"  OnDataBound="ShowGrid_DataBound" OnDataBinding="ShowGrid_DataBinding" OnSummaryDisplayText="ShowGrid_SummaryDisplayText" 
                            OnHtmlDataCellPrepared="ShowGrid_HtmlDataCellPrepared" OnHtmlRowPrepared="ShowGrid_HtmlRowPrepared"
                            SettingsBehavior-AllowFocusedRow="true" SettingsBehavior-AllowSelectSingleRowOnly="true" Settings-HorizontalScrollBarMode="Visible">
                            <columns>
                                <dxe:GridViewDataTextColumn FieldName="LEDGR_ID" Caption="LEDGR_ID" Width="0%" VisibleIndex="0" HeaderStyle-CssClass="colDisable">
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="LEDGER_TYPE" Caption="LEDGER_TYPE" Width="0%" VisibleIndex="1" HeaderStyle-CssClass="colDisable">
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="LEDGER" Caption="Account" Width="400px" VisibleIndex="2" HeaderStyle-CssClass="colDisable">
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="GRPSUBGRPLEDGER" Caption ="Account/Group Name" Width="400px" VisibleIndex="3" HeaderStyle-CssClass="colDisable">
                                     <DataItemTemplate>                        
                                        <label class='<%#Eval("TREESPACE") %>'><%#Eval("GRPSUBGRPLEDGER") %></label>
                                     </DataItemTemplate> 
                                    <EditFormSettings Visible="False"></EditFormSettings>
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="PARENTGRPNAME" Caption ="Parent Group" Width="200px" VisibleIndex="4" HeaderStyle-CssClass="colDisable">
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="OP_DR" Caption="Opening Bal(Dr)" Width="150px" VisibleIndex="5" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" Settings-AllowAutoFilter="False" HeaderStyle-CssClass="colDisable">
                                    <HeaderStyle HorizontalAlign="Right" />
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="OP_CR" Caption="Opening Bal(Cr)" Width="150px" VisibleIndex="6" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" Settings-AllowAutoFilter="False" HeaderStyle-CssClass="colDisable">
                                <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="OPBAL" Caption="Opening Bal." Width="120px" VisibleIndex="7" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" Settings-AllowAutoFilter="False" HeaderStyle-CssClass="colDisable">
                                    <HeaderStyle HorizontalAlign="Right" />
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="OPDRCRTYPE" Caption="Type" Width="50px" VisibleIndex="8" Settings-AllowAutoFilter="False" HeaderStyle-CssClass="colDisable">
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>
                                
                                <dxe:GridViewDataTextColumn FieldName="PR_DR" Caption="Debit" Width="120px" VisibleIndex="9" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" Settings-AllowAutoFilter="False" HeaderStyle-CssClass="colDisable">
                                    <HeaderStyle HorizontalAlign="Right" />
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="PR_CR" Caption="Credit" Width="120px" VisibleIndex="10" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" Settings-AllowAutoFilter="False" HeaderStyle-CssClass="colDisable">
                                    <HeaderStyle HorizontalAlign="Right" />
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="CLOSE_DR" Caption="Closing Bal(Dr)" Width="120px" VisibleIndex="11" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" Settings-AllowAutoFilter="False" HeaderStyle-CssClass="colDisable">
                                    <HeaderStyle HorizontalAlign="Right" />
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="CLOSE_CR" Caption="Closing Bal(Cr)" Width="120px" VisibleIndex="12" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" Settings-AllowAutoFilter="False" HeaderStyle-CssClass="colDisable">
                                    <DataItemTemplate>
                                        <span> <%#Eval("CLOSE_CR") %></span>
                                        <span style="display:none"> <%#Eval("LEDGER_SHORTNAME") %></span>
                                    </DataItemTemplate> 
                                    <%--<HeaderTemplate><span>Closing Bal(Cr)</span></HeaderTemplate> --%>
                                    <HeaderStyle HorizontalAlign="Right" />
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="CLBAL" Caption="Closing Bal." Width="120px" VisibleIndex="13" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" Settings-AllowAutoFilter="False" HeaderStyle-CssClass="colDisable">
                                    <DataItemTemplate>
                                        <span> <%#Eval("CLBAL") %></span>
                                        <span style="display:none"> <%#Eval("LEDGER_SHORTNAME") %></span>
                                    </DataItemTemplate> 
                                    <HeaderStyle HorizontalAlign="Right" />
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="CLDRCRTYPE" Caption="Type" Width="50px" VisibleIndex="14" Settings-AllowAutoFilter="False" HeaderStyle-CssClass="colDisable">
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="TREESPACE" Caption="TREESPACE" Width="0%" VisibleIndex="15" HeaderStyle-CssClass="colDisable">
                                </dxe:GridViewDataTextColumn>

                                <%--<dxe:GridViewDataTextColumn FieldName="LEDGR_ID" Caption="LEDGR_ID" Width="0%" VisibleIndex="15" HeaderStyle-CssClass="colDisable">
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="LEDGER_TYPE" Caption="LEDGER_TYPE" Width="0%" VisibleIndex="16" HeaderStyle-CssClass="colDisable">
                                </dxe:GridViewDataTextColumn>--%>

                                <%-- <dxe:GridViewDataTextColumn FieldName="Close_Cr" Caption="Cr.(Closing)" HeaderStyle-HorizontalAlign="right"  PropertiesTextEdit-DisplayFormatString="0.00" CellStyle-HorizontalAlign="right" VisibleIndex="8" Width="25%">
                                <DataItemTemplate>
                                    <span> <%#Eval("Close_Cr") %></span>
                                    <span style="display:none"> <%#Eval("Ledger_ShortName") %></span>
                                    </DataItemTemplate> 
                                <HeaderTemplate><span>Cr.(Closing)</span></HeaderTemplate> 
                            </dxe:GridViewDataTextColumn>--%>
                            </columns>
                            <SettingsDataSecurity AllowDelete="false" AllowEdit="false" AllowInsert="false" />
                            <settingsbehavior confirmdelete="true" enablecustomizationwindow="true" enablerowhottrack="true" />
                            <settings showfooter="true" showgrouppanel="false" showgroupfooter="VisibleIfExpanded" showstatusbar="Visible" showfilterrow="true"/>
                            <settingsediting mode="EditForm" />
                            <settingscontextmenu enabled="true" />
                            <settingsbehavior AutoExpandAllGroups="true" AllowSort="False" columnresizemode="Control" />
                            <settingssearchpanel visible="False" />
                            <SettingsPager PageSize="50">
                                <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100" />
                            </SettingsPager>
                            <%--<settingspager mode="ShowAllRecords"></settingspager>--%>
                            <settings showfilterrow="True" showfilterrowmenu="true" showstatusbar="Visible" usefixedtablelayout="true" />

                            <totalsummary>
                                <dxe:ASPxSummaryItem FieldName="LEDGER" SummaryType="Custom" />
                                <dxe:ASPxSummaryItem FieldName="GRPSUBGRPLEDGER" SummaryType="Custom" />
                                <dxe:ASPxSummaryItem FieldName="OP_DR" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="OP_CR" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="OPBAL" SummaryType="Custom" />
                                <dxe:ASPxSummaryItem FieldName="PR_DR" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="PR_CR" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="CLOSE_DR" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="CLOSE_CR" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="CLBAL" SummaryType="Custom" />
                            </totalsummary>
                            <clientsideevents endcallback="Callback2_EndCallback" />
                        </dxe:ASPxGridView>
                         <dx:LinqServerModeDataSource ID="GenerateEntityServerModeDataSource" runat="server" OnSelecting="GenerateEntityServerModeDataSource_Selecting"
                            ContextTypeName="ReportSourceDataContext" TableName="TRIALONNETBALANCE_REPORT"></dx:LinqServerModeDataSource>
                    </div>
                </td>
            </tr>
        </table>

        <div class="text-center" style="display: none;">
            <label style="color: #b5285f; font-weight: bold;" class="clsFrom">
                <dxe:ASPxTextBox ID="txtdiffcalculation" ClientInstanceName="ctxtdiffcalculation" runat="server" ReadOnly="true" Width="50px"></dxe:ASPxTextBox>
                <dxe:ASPxTextBox ID="txtdiffcalculationText" ClientInstanceName="ctxtdiffcalculationText" runat="server" ReadOnly="true" Width="100px"></dxe:ASPxTextBox>

            </label>
        </div>
        <div id="loadCurrencyMassage" style="display: none;">
            <br />
        </div>
    </div>
    </div>

    <dxe:ASPxPopupControl ID="popup" runat="server" ClientInstanceName="cpopup2ndLevel"
        Width="1100px" Height="600px" ScrollBars="Vertical" HeaderText="Trial On Net Balance - Detailed" PopupHorizontalAlign="WindowCenter" HeaderStyle-CssClass="wht"
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
                                            <asp:Label ID="lblFromDate2ndLevel" runat="Server" Font-Bold="true" CssClass="mylabel1"></asp:Label>
                                        </label>
                                        <label style="color: #b5285f; font-weight: bold; margin-top: 5px;" class="clsTo">
                                            <asp:Label ID="lblToDate2ndLevel" runat="Server" Font-Bold="true"  CssClass="mylabel1"></asp:Label>
                                        </label>
                                        <span style="padding-left: 10px;color: #b5285f; display: inline-block"><strong>Press < Esc > Key to Close</strong></span></td>
                                
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
                        <dxe:ASPxGridView runat="server" ID="ShowGridDetails2Level" ClientInstanceName="cShowGridDetails2Level" KeyFieldName="SEQ" Width="100%" EnableRowsCache="false" AutoGenerateColumns="False"
                             KeyboardSupport="true" Settings-HorizontalScrollBarMode="Visible" OnDataBinding="ShowGridDetails2Level_DataBinding"
                             DataSourceID="GenerateEntityServerModeDataSourceDetails" OnSummaryDisplayText="ShowGridDetails2Level_SummaryDisplayText" >
                            <Columns>
                                <dxe:GridViewDataTextColumn FieldName="BRANCH_DESCRIPTION" Caption="Unit" Width="200px" VisibleIndex="1" Settings-AllowAutoFilter="False">
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="TRN_REFID" Caption="Doc. No" Width="150px">
                                    <CellStyle HorizontalAlign="Center"></CellStyle>
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <DataItemTemplate>
                                            <a href="javascript:void(0)" onclick="OnGetRowValuesLvl3('<%#Eval("DOC_ID") %>','<%#Eval("MODULE_TYPE") %>' ,'<%#Eval("TRN_REFID") %>')" class="pad">
                                            <%#Eval("TRN_REFID")%>
                                            </a> 
                                    </DataItemTemplate>
                                    <EditFormSettings Visible="False" />
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Doc. Date" Width="120px" FieldName="ACL_TRNDT" VisibleIndex="3">
                                    <PropertiesTextEdit DisplayFormatString="dd-MM-yyyy"></PropertiesTextEdit>
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="TRAN_TYPE" Caption="Document Type" Width="150px" VisibleIndex="4" Settings-AllowAutoFilter="False">
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="CUSTVENDNAME" Caption="Party" Width="200px" VisibleIndex="5">
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="CVGRP_NAME" Caption="Group Name" Width="150px" VisibleIndex="6">
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="OP_DR" Caption="Dr.(Opening)" Width="120px" VisibleIndex="7" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" Settings-AllowAutoFilter="False">
                                    <HeaderStyle HorizontalAlign="Right" />
                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>
                           
                                <dxe:GridViewDataTextColumn FieldName="OP_CR" Caption="Cr.(Opening)" Width="120px" VisibleIndex="7" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" Settings-AllowAutoFilter="False">
                                    <HeaderStyle HorizontalAlign="Right" />
                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>
                                
                                <dxe:GridViewDataTextColumn FieldName="PR_DR" Caption="Dr.(Period)" Width="120px" VisibleIndex="9" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" Settings-AllowAutoFilter="False">
                                    <HeaderStyle HorizontalAlign="Right" />
                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="PR_CR" Caption="Cr.(Period)" Width="120px" VisibleIndex="10" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" Settings-AllowAutoFilter="False">
                                    <HeaderStyle HorizontalAlign="Right" />
                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="CLOSE_DR" Caption="Dr.(Closing)" Width="120px" VisibleIndex="11" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" Settings-AllowAutoFilter="False">
                                    <HeaderStyle HorizontalAlign="Right" />
                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="CLOSE_CR" Caption="Cr.(Closing)" Width="120px" VisibleIndex="12" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" Settings-AllowAutoFilter="False">
                                    <HeaderStyle HorizontalAlign="Right" />
                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="DOC_ID" Caption="Doc_Id" Width="120px" VisibleIndex="13" Visible="false">
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="MODULE_TYPE" Caption="MODULE_TYPE" Width="120px" VisibleIndex="14" Visible="false">
                                </dxe:GridViewDataTextColumn>
                            </Columns>
                            <SettingsDataSecurity AllowDelete="false" AllowEdit="false" AllowInsert="false" />
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
                                 <dxe:ASPxSummaryItem FieldName="OP_DR" SummaryType="Sum"/>
                                 <dxe:ASPxSummaryItem FieldName="OP_CR" SummaryType="Sum"/>
                                 <dxe:ASPxSummaryItem FieldName="PR_DR" SummaryType="Sum"/>
                                 <dxe:ASPxSummaryItem FieldName="PR_CR" SummaryType="Sum"/>
                                 <dxe:ASPxSummaryItem FieldName="CLOSE_DR" SummaryType="Sum" />
                                 <dxe:ASPxSummaryItem FieldName="CLOSE_CR" SummaryType="Sum" />
                            </TotalSummary>
                        </dxe:ASPxGridView>
                         <dx:LinqServerModeDataSource ID="GenerateEntityServerModeDataSourceDetails" runat="server" OnSelecting="GenerateEntityServerModeDataSourceDetails_Selecting"
                            ContextTypeName="ReportSourceDataContext" TableName="TRIALONNETBALANCEDETAIL_REPORT"></dx:LinqServerModeDataSource>

                    </div>
                </dxe:PopupControlContentControl>
            </contentcollection>

    </dxe:ASPxPopupControl>

    <dxe:ASPxPopupControl ID="stkpopup" runat="server" ClientInstanceName="cpopup2ndLevelStock"
        Width="1100px" Height="600px" ScrollBars="Vertical" HeaderText="Trial On Net Balance - Detailed" PopupHorizontalAlign="WindowCenter" HeaderStyle-CssClass="wht"
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
                                            <asp:Label ID="Label5" runat="Server" CssClass="mylabel1" ClientInstanceName="clblLedger2ndLevelStock"></asp:Label>
                                        </label>
                                    </td>
                                    <td style="padding-top: 10px">
                                        <dxe:ASPxTextBox ID="txtLedger2ndLevelStock" ClientInstanceName="ctxtLedger2ndLevelStock" runat="server" ReadOnly="true" Width="600px"></dxe:ASPxTextBox>
                                    </td>
                                </tr>

                                <tr>
                                    <td></td>
                                    <td>
                                        <label style="color: #b5285f; font-weight: bold; margin-top: 5px;" class="clsTo">
                                            <asp:Label ID="lblFromDate2ndLevelStock" runat="Server" Font-Bold="true" CssClass="mylabel1"></asp:Label>
                                        </label>
                                        <label style="color: #b5285f; font-weight: bold; margin-top: 5px;" class="clsTo">
                                            <asp:Label ID="lblToDate2ndLevelStock" runat="Server" Font-Bold="true"  CssClass="mylabel1"></asp:Label>
                                        </label>
                                        <span style="padding-left: 10px;color: #b5285f; display: inline-block"><strong>Press < Esc > Key to Close</strong></span>
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
                    <div class="clearfix" onkeypress="OnWaitingGridKeyPress3(event)">
                        <dxe:ASPxGridView runat="server" ID="ShowGridDetails3Level" ClientInstanceName="cShowGridDetails3Level" KeyFieldName="SEQ" Width="100%" EnableRowsCache="false" AutoGenerateColumns="False"
                             KeyboardSupport="true" Settings-HorizontalScrollBarMode="Visible" DataSourceID="GenerateEntityServerModeDataSourceDetails2" OnSummaryDisplayText="ShowGridDetails3Level_SummaryDisplayText" >
                            <Columns>
                                <dxe:GridViewDataTextColumn FieldName="BRANCH_DESCRIPTION" Caption="Unit" Width="200px" VisibleIndex="1" Settings-AllowAutoFilter="False">
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="PRODDESC" Caption="Product Name" Width="320px" VisibleIndex="2" Settings-AllowAutoFilter="False">
                                    <CellStyle HorizontalAlign="Center"></CellStyle>
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <Settings AutoFilterCondition="Contains" />
                                    <DataItemTemplate>
                                        <a href="javascript:void(0)" onclick="OpenStockDetails('<%#Eval("BRANCHID") %>','<%#Eval("PRODID") %>')" class="pad">
                                            <%#Eval("PRODDESC")%>
                                        </a>
                                    </DataItemTemplate>
                                    <EditFormSettings Visible="False" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="STOCKUOM" Caption="Stock UOM" Width="150px" VisibleIndex="3" Settings-AllowAutoFilter="False">
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="PRODCLASS" Caption="Class" Width="150px" VisibleIndex="4" Settings-AllowAutoFilter="False">
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="BRAND_NAME" Caption="Brand" Width="150px" VisibleIndex="5" Settings-AllowAutoFilter="False">
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="IN_QTY_OP" Caption="Quantity" Width="120px" VisibleIndex="6" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" Settings-AllowAutoFilter="False">
                                    <HeaderStyle HorizontalAlign="Right" />
                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="IN_PRICE_OP" Caption="Rate" Width="120px" VisibleIndex="7" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" Settings-AllowAutoFilter="False">
                                    <HeaderStyle HorizontalAlign="Right" />
                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="IN_TOTAL_OP" Caption="Total" Width="120px" VisibleIndex="8" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" Settings-AllowAutoFilter="False">
                                    <HeaderStyle HorizontalAlign="Right" />
                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>
                            </Columns>
                            <SettingsDataSecurity AllowDelete="false" AllowEdit="false" AllowInsert="false" />
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
                                 <dxe:ASPxSummaryItem FieldName="IN_QTY_OP" SummaryType="Sum"/>
                                 <dxe:ASPxSummaryItem FieldName="IN_TOTAL_OP" SummaryType="Sum"/>
                            </TotalSummary>
                        </dxe:ASPxGridView>
                         <dx:LinqServerModeDataSource ID="GenerateEntityServerModeDataSourceDetails2" runat="server" OnSelecting="GenerateEntityServerModeDataSourceDetails2_Selecting"
                            ContextTypeName="ReportSourceDataContext" TableName="TRIALONNETBALANCEDETAIL_REPORT"></dx:LinqServerModeDataSource>

                    </div>
                </dxe:PopupControlContentControl>
            </contentcollection>

    </dxe:ASPxPopupControl>

    <dxe:ASPxPopupControl ID="popupStkValDet" runat="server" ClientInstanceName="cpopupStkValDet"
        Width="1200px" Height="600px" ScrollBars="Vertical" HeaderText="Valuation Details" PopupHorizontalAlign="WindowCenter" HeaderStyle-CssClass="wht"
        PopupVerticalAlign="TopSides" CloseAction="CloseButton"
        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
        ContentStyle-CssClass="pad">
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
                <% if (rights.CanExport)
                            { %> 
                <asp:DropDownList ID="ddlstkdetails" runat="server" CssClass="btn btn-sm btn-primary"
                    AutoPostBack="true" OnSelectedIndexChanged="cmbExportStkDet_SelectedIndexChanged">
                    <asp:ListItem Value="0">Export to</asp:ListItem>
                    <asp:ListItem Value="1">XLSX</asp:ListItem>
                    <asp:ListItem Value="2">PDF</asp:ListItem>
                    <asp:ListItem Value="3">CSV</asp:ListItem>
                    <asp:ListItem Value="4">RTF</asp:ListItem>
                </asp:DropDownList>
                <% } %>

                <div class="row">
                    <div>
                    </div>
                    <div class="col-md-12">
                        <dxe:ASPxGridView ID="grivaluation" runat="server" AutoGenerateColumns="False" Width="100%" ClientInstanceName="cgridValuationdetails" KeyFieldName="SLNO"
                            DataSourceID="GenerateEntityServerStockDetailsModeDataSource"  OnSummaryDisplayText="grivaluation_SummaryDisplayText" 
                            Settings-HorizontalScrollBarMode="Auto" Settings-VerticalScrollBarMode="Auto" Settings-VerticalScrollableHeight ="300">
                            <Columns>
                                <dxe:GridViewDataTextColumn Caption="Document Date" FieldName="Document_Date" PropertiesTextEdit-DisplayFormatString="dd-MM-yyyy" VisibleIndex="0" Width="150px">
                                    <CellStyle CssClass="gridcellleft" Wrap="true"></CellStyle>
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Document No" FieldName="Document_No" VisibleIndex="1" Width="120px">
                                    <CellStyle CssClass="gridcellleft" Wrap="true"></CellStyle>
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Document Type" FieldName="Trans_Type" VisibleIndex="2" Width="200px">
                                    <CellStyle CssClass="gridcellleft" Wrap="true"></CellStyle>
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Product" FieldName="sProducts_Description" VisibleIndex="3" Width="150px">
                                    <CellStyle CssClass="gridcellleft" Wrap="true"></CellStyle>
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>
                                
                                <dxe:GridViewDataTextColumn FieldName="ProductClass_Description" Width="100px" Caption="Class" VisibleIndex="4">
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="Brand_Name" Width="100px" Caption="Brand" VisibleIndex="5">
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="STOCKUOM" Width="80px" Caption="Stock Unit" VisibleIndex="6">
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="ALTSTOCKUOM" Width="80px" Caption="Alt. Unit" VisibleIndex="7">
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="IN_QTY_OP" Caption="Available Stock" Width="120px" VisibleIndex="8" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" Settings-AllowAutoFilter="False">
                                    <HeaderStyle HorizontalAlign="Right" />
                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="ALTIN_QTY" Caption="Alt. Stock Qty." Width="120px" VisibleIndex="9" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" Settings-AllowAutoFilter="False">
                                    <HeaderStyle HorizontalAlign="Right" />
                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="IN_PRICE_OP" Caption="Rate" Width="120px" VisibleIndex="10" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" Settings-AllowAutoFilter="False">
                                    <HeaderStyle HorizontalAlign="Right" />
                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="LANDEDCOSTVALUE" Caption="Landed Cost" Width="120px" VisibleIndex="11" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" Settings-AllowAutoFilter="False">
                                    <HeaderStyle HorizontalAlign="Right" />
                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="IN_TOTAL_OP" Caption="Value" Width="120px" VisibleIndex="12" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" Settings-AllowAutoFilter="False">
                                    <HeaderStyle HorizontalAlign="Right" />
                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                 <dxe:GridViewDataTextColumn FieldName="IN_QTY" Caption="Recd. Qty." Width="120px" VisibleIndex="13" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" Settings-AllowAutoFilter="False">
                                    <HeaderStyle HorizontalAlign="Right" />
                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="ALTIN_QTY" Caption="Recd. Alt. Qty." Width="120px" VisibleIndex="14" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" Settings-AllowAutoFilter="False">
                                    <HeaderStyle HorizontalAlign="Right" />
                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="OUT_QTY" Caption="Delv. Qty." Width="120px" VisibleIndex="15" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" Settings-AllowAutoFilter="False">
                                    <HeaderStyle HorizontalAlign="Right" />
                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                 <dxe:GridViewDataTextColumn FieldName="ALTOUT_QTY" Caption="Delv. Alt. Qty." Width="120px" VisibleIndex="16" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" Settings-AllowAutoFilter="False">
                                    <HeaderStyle HorizontalAlign="Right" />
                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="IN_RATE" Caption="Doc. Rate" Width="120px" VisibleIndex="17" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" Settings-AllowAutoFilter="False">
                                    <HeaderStyle HorizontalAlign="Right" />
                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="IN_VALUE" Caption="Recd. Value" Width="120px" VisibleIndex="18" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" Settings-AllowAutoFilter="False">
                                    <HeaderStyle HorizontalAlign="Right" />
                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="OUT_VALUE" Caption="Delv. Value" Width="120px" VisibleIndex="19" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" Settings-AllowAutoFilter="False">
                                    <HeaderStyle HorizontalAlign="Right" />
                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="CLOSE_QTY" Caption="Balance Qty." Width="120px" VisibleIndex="20" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" Settings-AllowAutoFilter="False">
                                    <HeaderStyle HorizontalAlign="Right" />
                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="CLOSE_RATE" Caption="Close Rate" Width="120px" VisibleIndex="21" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" Settings-AllowAutoFilter="False">
                                    <HeaderStyle HorizontalAlign="Right" />
                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="CLOSE_VAL" Caption="Balance Value" Width="120px" VisibleIndex="22" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" Settings-AllowAutoFilter="False">
                                    <HeaderStyle HorizontalAlign="Right" />
                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>
                            </Columns>

                            <SettingsDataSecurity AllowDelete="false" AllowEdit="false" AllowInsert="false" />
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
                                <dxe:ASPxSummaryItem FieldName="IN_QTY_OP" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="LANDEDCOSTVALUE" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="IN_TOTAL_OP" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="IN_QTY" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="OUT_QTY" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="IN_VALUE" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="OUT_VALUE" SummaryType="Sum" />
                            </TotalSummary>

                        </dxe:ASPxGridView>
                          <dx:LinqServerModeDataSource ID="GenerateEntityServerStockDetailsModeDataSource" runat="server" OnSelecting="GenerateEntityServerStockDetailsModeDataSource_Selecting"
                        ContextTypeName="ReportSourceDataContext" TableName="STOCKVALUATION_REPORT" ></dx:LinqServerModeDataSource>
                    </div>
                    <div class="clear"></div>
                </div>
            </dxe:PopupControlContentControl>
        </ContentCollection>
        <ClientSideEvents CloseUp="popupHide" />

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
        <PanelCollection>
            <dxe:PanelContent runat="server">
            <asp:HiddenField ID="hfIsTrialOnNetBalFilter" runat="server" />
            </dxe:PanelContent>
        </PanelCollection>
        <ClientSideEvents EndCallback="CallbackPanelEndCall" />
    </dxe:ASPxCallbackPanel>

    <dxe:ASPxCallbackPanel runat="server" ID="CallbackPanel2" ClientInstanceName="cCallbackPanel2" OnCallback="CallbackPanel2_Callback">
        <PanelCollection>
            <dxe:PanelContent runat="server">
            <asp:HiddenField ID="hfIsTrialOnNetBalDetFilter" runat="server" />
            </dxe:PanelContent>
        </PanelCollection>
        <ClientSideEvents EndCallback="CallbackPanelDetailEndCall" />
    </dxe:ASPxCallbackPanel>

    <dxe:ASPxCallbackPanel runat="server" ID="CallbackPanel3" ClientInstanceName="cCallbackPanel3" OnCallback="CallbackPanel3_Callback">
        <PanelCollection>
            <dxe:PanelContent runat="server">
            <asp:HiddenField ID="hfIsTrialOnNetBalDet2Filter" runat="server" />
            </dxe:PanelContent>
        </PanelCollection>
        <ClientSideEvents EndCallback="CallbackPanelDetail2EndCall" />
    </dxe:ASPxCallbackPanel>

    <dxe:ASPxCallbackPanel runat="server" ID="CallbackPanelStockDetail" ClientInstanceName="cCallbackPanelStockDetail" OnCallback="CallbackPanelStockDetail_Callback">
        <PanelCollection>
            <dxe:PanelContent runat="server">
                <asp:HiddenField ID="hfIsTrialOnNetBalStkValDetFilter" runat="server" />
            </dxe:PanelContent>
        </PanelCollection>
        <ClientSideEvents EndCallback="CallbackPanelStockDetEndCall" />
    </dxe:ASPxCallbackPanel>

    <asp:HiddenField ID="HDShowOrPrintClick" runat="server" />
</asp:Content>