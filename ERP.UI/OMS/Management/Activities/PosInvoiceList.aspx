<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="PosInvoiceList.aspx.cs" Inherits="ERP.OMS.Management.Activities.PosInvoiceList" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .okClass {
            margin-top: 5px;
        }
    </style>
    <script>
        var SelectedInvoiceId = 0;

        var GlobalRowIndex = 0;
        var BranchMassListByKeyValue = [];
        var isFirstTime = true;
        function AllControlInitilize() {
            if (isFirstTime) {
                // PopulateCurrentBankBalance(ccmbBranchfilter.GetValue());

                if (localStorage.getItem('PosListFromDate')) {
                    var fromdatearray = localStorage.getItem('PosListFromDate').split('-');
                    var fromdate = new Date(fromdatearray[0], parseFloat(fromdatearray[1]) - 1, fromdatearray[2], 0, 0, 0, 0);
                    cFormDate.SetDate(fromdate);
                }

                if (localStorage.getItem('PosListToDate')) {
                    var todatearray = localStorage.getItem('PosListToDate').split('-');
                    var todate = new Date(todatearray[0], parseFloat(todatearray[1]) - 1, todatearray[2], 0, 0, 0, 0);
                    ctoDate.SetDate(todate);
                }
                if (localStorage.getItem('PosListBranch')) {
                    if (ccmbBranchfilter.FindItemByValue(localStorage.getItem('PosListBranch'))) {
                        ccmbBranchfilter.SetValue(localStorage.getItem('PosListBranch'));
                    }

                }
                if ($("#LoadGridData").val() == "ok")
                    updateGridByDate();

                isFirstTime = false;
            }
        }



        function updateMassBranchAssign() {

        }
        function PerformCallToRacpayGridBind() {
            CustomerRecpayPanel.PerformCallback('Bindsingledesign');
            cCustDocumentsPopup.Hide();
            return false;
        }

        function CustRacPayPanelEndCall(s, e) {
            debugger;
            if (CustomerRecpayPanel.cpSuccess != null) {
                var TotDocument = CustomerRecpayPanel.cpSuccess.split(',');
                var reportName = cCustCmbDesignName.GetValue();
                var module = 'CUSTRECPAY';
                window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + reportName + '&modulename=' + module + '&id=' + RecPayId + '&PrintOption=' + 1, '_blank')
            }
            CustomerRecpayPanel.cpSuccess = null
            if (CustomerRecpayPanel.cpSuccess == null) {
                CustomerRecpayPanel.SetSelectedIndex(0);
            }
        }

        function onCustomerReceiptPrint(id) {
            RecPayId = id;
            cCustDocumentsPopup.Show();
            cCustCmbDesignName.SetSelectedIndex(0);
            CustomerRecpayPanel.PerformCallback('Bindalldesignes');
            $('#btnOK').focus();
        }

        function OnCustReceiptViewClick(id) {
            uri = "CustomerReceiptPayment.aspx?key=" + id + "&req=V&IsTagged=Y&type=CRP";
            capcReciptPopup.SetContentUrl(uri);
            capcReciptPopup.SetHeaderText("View Money Receipt");
            capcReciptPopup.Show();
        }

        

        function ListingISTGridEndCallback(s, e) {
            IconChangeIST();
            if (cIstGrid.cpCancelAssignMent) {
                if (cIstGrid.cpCancelAssignMent == "yes") {
                    jAlert("Branch Assignment Cancel Successfully.");
                    cIstGrid.cpCancelAssignMent = null;
                    cIstGrid.Refresh();
                }
            }


            if (cIstGrid.cpDelete) {
                jAlert(cIstGrid.cpDelete);
                cIstGrid.cpDelete = null;
                cIstGrid.Refresh();


            }
        }


        function CancelBranchToThisInvoice() {
            cAssignedBranch.SetValue('0');
            AssignedBranchSelectedIndexChanged(cAssignedBranch);
        }

       
        function onISTCancelBranchAssignment(invId) {
            cIstGrid.PerformCallback('CancelAssignment~' + invId);
        }

        

        

       

       

        function BranchChangeOnMassChange(s, e) {

            var AssignedBranch = {
                InvoiceId: '',
                BranchId: ''
            }
            AssignedBranch.InvoiceId = cmassBranch.GetRowKey(GlobalRowIndex);
            AssignedBranch.BranchId = s.GetValue();
            for (var ind = 0; ind < BranchMassListByKeyValue.length; ind++) {
                if (BranchMassListByKeyValue[ind].InvoiceId == AssignedBranch.InvoiceId) {
                    BranchMassListByKeyValue.pop(ind);
                }
            }


            BranchMassListByKeyValue.push(AssignedBranch);
        }


        function MassBranchAssign() {
          
            var url = '/OMS/Management/Activities/PosMassBranch.aspx';
            cmassBranchPopup.SetContentUrl(url);
            cmassBranchPopup.Show();

            //}
            return true;
        }

        

        
        function AssignBranchToThisInvoice() {
            //$('#MandatoryBranchAssign').attr('style', 'display:none');
            //$('#mandetoryAssignedWareHouse').attr('style', 'display:none');


            //if (cAssignedBranch.GetValue() == null || cAssignedBranch.GetValue() == '0') {
            //    $('#MandatoryBranchAssign').attr('style', 'display:block');
            //}
            //else if (cAssignedWareHouse.GetValue() == null) {
            //    $('#mandetoryAssignedWareHouse').attr('style', 'display:block');
            //} else {
            //    cAssignmentGrid.PerformCallback('AssignBranch~' + SelectedInvoiceId + '~' + cAssignedBranch.GetValue() + '~' + cAssignedWareHouse.GetValue());
            //}

            cAssignmentGrid.PerformCallback('AssignBranch~' + SelectedInvoiceId + '~' + cAssignedBranch.GetValue() + '~' + cAssignedWareHouse.GetValue());
        }

        function watingInvoicegridEndCallback() {
            if (cwatingInvoicegrid.cpReturnMsg) {
                if (cwatingInvoicegrid.cpReturnMsg != "") {
                    jAlert(cwatingInvoicegrid.cpReturnMsg);
                    document.getElementById('waitingInvoiceCount').value = parseFloat(document.getElementById('waitingInvoiceCount').value) - 1;
                    cwatingInvoicegrid.cpReturnMsg = null;
                }
            }
        }


        function onViewBranchAssignment(obj) {
            SelectedInvoiceId = obj;
            cAssignmentGrid.PerformCallback('0~0');
            cBranchRequUpdatePanel.PerformCallback(SelectedInvoiceId);
            $('#BranchAssignmentHeader').show();
            cAssignmentPopUp.SetHeaderText('Branch Assignment');
            cAssignmentPopUp.Show();

        }




        
        

        

        function IconChangeIST() {
            $(function () {

                var $tr = $("#IstGrid_DXMainTable > tbody > tr:gt(1)");

                $tr.each(function (index, value) {
                    //var $DelvStatus = $(this).find("td").eq(8).text();
                    //var $payType = $(this).find("td").eq(5).text();
                    //var $DelvType = $(this).find("td").eq(6).text();
                    //var $AssignedBranch = $(this).find("td").eq(15).text();

                    //var $a_Assignment = $(this).find("td").eq(17).find("#ist_Assignment");
                    //var $Cancel_Assignment = $(this).find("td").eq(17).find("#istCancel_Assignment");
                    //var $a_editInvoice = $(this).find("td").eq(17).find("#ist_editInvoice");
                    //var $a_delete = $(this).find("td").eq(17).find("#ist_delete");
                    
                    var $DelvStatus = $(this).find("td").eq(7).text();
                    var $payType = $(this).find("td").eq(4).text();
                    var $DelvType = $(this).find("td").eq(5).text();
                    var $AssignedBranch = $(this).find("td").eq(14).text();

                    var $a_Assignment = $(this).find("td").eq(16).find("#ist_Assignment");
                    var $Cancel_Assignment = $(this).find("td").eq(16).find("#istCancel_Assignment");
                    var $a_editInvoice = $(this).find("td").eq(16).find("#ist_editInvoice");
                    var $a_delete = $(this).find("td").eq(16).find("#ist_delete");


                    if ($DelvStatus == 'Pending') {
                        $a_Assignment.show();
                        $a_editInvoice.show();
                    }
                    else {
                        if ($payType != 'Finance') {
                            $a_Assignment.hide();
                            $a_editInvoice.hide();
                        }
                    }

                    if ($DelvStatus == 'Transfered') {
                        $Cancel_Assignment.show();
                    } else {
                        $Cancel_Assignment.hide();
                    }

                    if ($AssignedBranch.trim() != "") {
                        $a_delete.hide();
                        $a_editInvoice.hide();
                    }
                    if ($DelvType.trim() == "Already Delivered") {
                        $a_delete.hide();
                        $a_editInvoice.hide();
                    }

                });
            });
        }


        $(document).ready(function () {
          
            IconChangeIST();

        });
        

        function OnBeginAfterCallbackIST(s, e) {
            IconChangeIST();
        }

        function updateGridByDate() {
            if (cFormDate.GetDate() == null) {
                jAlert('Please select from date.', 'Alert', function () { cFormDate.Focus(); });
            }
            else if (ctoDate.GetDate() == null) {
                jAlert('Please select to date.', 'Alert', function () { ctoDate.Focus(); });
            }
            else if (ccmbBranchfilter.GetValue() == null) {
                jAlert('Please select Branch.', 'Alert', function () { ccmbBranchfilter.Focus(); });
            }
            else {

                localStorage.setItem("PosListFromDate", cFormDate.GetDate().format('yyyy-MM-dd'));
                localStorage.setItem("PosListToDate", ctoDate.GetDate().format('yyyy-MM-dd'));
                localStorage.setItem("PosListBranch", ccmbBranchfilter.GetValue());

                $('#branchName').text(ccmbBranchfilter.GetText());
                PopulateCurrentBankBalance(ccmbBranchfilter.GetValue());
                if (page.activeTabIndex == 1) {
                    $("#hfFromDateReceipt").val(cFormDate.GetDate().format('yyyy-MM-dd'));
                    $("#hfToDateReceipt").val(ctoDate.GetDate().format('yyyy-MM-dd'));
                    $("#hfBranchIDReceipt").val(ccmbBranchfilter.GetValue());
                    $("#hfIsFilterReceipt").val("Y");
                    cCustomerReceiptGrid.Refresh();
                    //cCustomerReceiptGrid.PerformCallback('FilterGridByDate~' + cFormDate.GetDate().format('yyyy-MM-dd') + '~' + ctoDate.GetDate().format('yyyy-MM-dd') + '~' + ccmbBranchfilter.GetValue())
                }
                if (page.activeTabIndex == 2) {
                    $("#hfFromDateIst").val(cFormDate.GetDate().format('yyyy-MM-dd'));
                    $("#hfToDateIst").val(ctoDate.GetDate().format('yyyy-MM-dd'));
                    $("#hfBranchIDIst").val(ccmbBranchfilter.GetValue());
                    $("#hfIsFilterIst").val("Y");
                    cIstGrid.Refresh();
                    //cIstGrid.PerformCallback('FilterGridByDate~' + cFormDate.GetDate().format('yyyy-MM-dd') + '~' + ctoDate.GetDate().format('yyyy-MM-dd') + '~' + ccmbBranchfilter.GetValue())
                }
            }
        }
        function InvoiceWattingOkClick() {
            var index = cwatingInvoicegrid.GetFocusedRowIndex();
            var listKey = cwatingInvoicegrid.GetRowKey(index);
            if (listKey) {
                if (cwatingInvoicegrid.GetRow(index).children[6].innerText != "Advance") {
                    var url = 'PosSalesInvoice.aspx?key=' + 'ADD&&BasketId=' + listKey;
                    LoadingPanel.Show();
                    window.location.href = url;
                } else {
                    ShowbasketReceiptPayment(listKey);
                }
            }
        }
        function cSelectPanelEndCall(s, e) {
            debugger;
            if (cSelectPanel.cpSuccess != null) {
                var TotDocument = cSelectPanel.cpSuccess.split(',');
                var reportName = cCmbDesignName.GetValue();
                var module = 'Invoice_POS';
                if (TotDocument.length > 0) {
                    for (var i = 0; i < TotDocument.length; i++) {
                        if (TotDocument[i] != "") {
                            //if (cCmbDesignName.GetValue() == 1) {
                            //    window.open("../../reports/XtraReports/Viewer/InvoiceReportViewer.aspx?id=" + InvoiceId + '&PrintOption=' + TotDocument[i], '_blank')
                            //}
                            //else if (cCmbDesignName.GetValue() == 2) {
                            //    window.open("../../reports/XtraReports/Viewer/TaxInvoiceReportViewer.aspx?id=" + InvoiceId + '&PrintOption=' + TotDocument[i], '_blank')
                            //}
                            window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + reportName + '&modulename=' + module + '&id=' + InvoiceId + '&PrintOption=' + TotDocument[i], '_blank')
                        }
                    }
                }
            }
            //cSelectPanel.cpSuccess = null
            if (cSelectPanel.cpSuccess == "") {
                if (cSelectPanel.cpChecked != "") {
                    jAlert('Please check Original For Recipient and proceed.');
                }
                //CselectDuplicate.SetEnabled(false);
                //CselectTriplicate.SetEnabled(false);
                CselectOriginal.SetCheckState('UnChecked');
                CselectDuplicate.SetCheckState('UnChecked');
                CselectFDuplicate.SetCheckState('UnChecked');
                CselectTriplicate.SetCheckState('UnChecked');
                cCmbDesignName.SetSelectedIndex(0);
            }
        }
        function PerformCallToGridBind() {
            //cSelectPanel.PerformCallback();
            cSelectPanel.PerformCallback('Bindsingledesign');
            cDocumentsPopup.Hide();
            return false;
        }
       
        var InvoiceId = 0;
        

        function onPrintJvIST(id, RowIndex) {
            InvoiceId = id;
            cSelectPanel.cpSuccess = "";
            cDocumentsPopup.Show();
            $('#HdInvoiceType').val('Stock Transfer');
            //CselectDuplicate.SetEnabled(false);
            //CselectTriplicate.SetEnabled(false);
            CselectOriginal.SetCheckState('UnChecked');
            CselectDuplicate.SetCheckState('UnChecked');
            CselectFDuplicate.SetCheckState('UnChecked');
            CselectTriplicate.SetCheckState('UnChecked');
            cCmbDesignName.SetSelectedIndex(0);
            cSelectPanel.PerformCallback('Bindalldesignes');
            $('#btnOK').focus();
        }

        function OnWaitingGridKeyPress(e) {

            if (e.code == "Enter") {
                var index = cwatingInvoicegrid.GetFocusedRowIndex();
                var listKey = cwatingInvoicegrid.GetRowKey(index);
                if (listKey) {
                    if (cwatingInvoicegrid.GetRow(index).children[6].innerText != "Advance") {
                        var url = 'PosSalesInvoice.aspx?key=' + 'ADD&&BasketId=' + listKey;
                        LoadingPanel.Show();
                        window.location.href = url;
                    } else {
                        ShowbasketReceiptPayment(listKey);
                    }
                }
            }

        }
        function RemoveInvoice(obj) {
            if (obj) {
                jConfirm("Clicking on Delete will not allow to use this Billing request again. Are you sure?", "Alert", function (ret) {
                    if (ret) {
                        cwatingInvoicegrid.PerformCallback('Remove~' + obj);
                    }
                });

            }
        }


        function ShowReceiptPayment() {
            uri = "CustomerReceiptPayment.aspx?key=ADD&IsTagged=Y";
            capcReciptPopup.SetContentUrl(uri);
            capcReciptPopup.SetHeaderText("Add Money Receipt");
            capcReciptPopup.Show();
        }

        function ShowbasketReceiptPayment(id) {
            uri = "CustomerReceiptPayment.aspx?key=ADD&IsTagged=Y&&basketId=" + id;
            capcReciptPopup.SetContentUrl(uri);
            capcReciptPopup.SetHeaderText("Add Money Receipt");
            capcReciptPopup.Show();
        }

        function timerTick() {
            //   cwatingInvoicegrid.Refresh();


            //$.ajax({
            //       type: "POST",
            //       url: "PosSalesInvoiceList.aspx/GetTotalWatingInvoiceCount",
            //       contentType: "application/json; charset=utf-8",
            //       dataType: "json",
            //       success: function (msg) {
            //           var status = msg.d;
            //           console.log(status);
            //           clblweatingCount.SetText('(' + status + ')');
            //           var fetcheddata = parseFloat(document.getElementById('waitingInvoiceCount').value);
            //           if (status != fetcheddata) {
            //               cwatingInvoicegrid.Refresh();
            //               document.getElementById('waitingInvoiceCount').value = status;
            //           }
            //       }
            //   });

        }
        function InvoiceWatingClick() {

            waitingPopUp.Show();
            cwatingInvoicegrid.Focus();
        }
        function ListRowClicked(s, e) {

            var index = e.visibleIndex;
            var listKey = cwatingInvoicegrid.GetRowKey(index);
            if (e.htmlEvent.target.id != "CloseRemoveWattingBtn") {
                if (cwatingInvoicegrid.GetRow(index).children[6].innerText != "Advance") {
                    var url = 'PosSalesInvoice.aspx?key=' + 'ADD&&BasketId=' + listKey;
                    LoadingPanel.Show();
                    window.location.href = url;
                } else {
                    ShowbasketReceiptPayment(listKey);
                }
            }
        }
        document.onkeydown = function (e) {
            if (event.keyCode == 73 && event.altKey == true) {
                StopDefaultAction(e);
                InvoiceWatingClick();
            }
            else if (event.keyCode == 67 && event.altKey == true) {
                StopDefaultAction(e);
                OnAddInvoiceButtonClick('Cash');
            }
            else if (event.keyCode == 68 && event.altKey == true) {
                StopDefaultAction(e);
                OnAddInvoiceButtonClick('Crd');
            }
            else if (event.keyCode == 70 && event.altKey == true) {
                StopDefaultAction(e);
                OnAddInvoiceButtonClick('Fin');
            }
            else if (event.keyCode == 82 && event.altKey == true) {
                StopDefaultAction(e);
                ShowReceiptPayment();
            }
            else if (event.keyCode == 77 && event.altKey == true) {
                StopDefaultAction(e);
                MassBranchAssign();
            }
            else if (event.keyCode == 83 && event.altKey == true) {
                StopDefaultAction(e);
                if (cmassBranchPopup.IsVisible()) {
                    //MassBranchAssignSaveClick();
                }
                else {
                    OnAddInvoiceButtonClick('IST');
                }
            }

            else if (event.keyCode == 79 && event.altKey == true) {
                if (waitingPopUp.IsVisible()) {
                    StopDefaultAction(e);
                    var index = cwatingInvoicegrid.GetFocusedRowIndex();
                    var listKey = cwatingInvoicegrid.GetRowKey(index);
                    if (listKey) {
                        if (cwatingInvoicegrid.GetRow(index).children[6].innerText != "Advance") {
                            var url = 'PosSalesInvoice.aspx?key=' + 'ADD&&BasketId=' + listKey;
                            LoadingPanel.Show();
                            window.location.href = url;
                        } else {
                            ShowReceiptPayment();
                        }
                    }
                }
            }
        }
        function StopDefaultAction(e) {
            if (e.preventDefault) { e.preventDefault() }
            else { e.stop() };

            e.returnValue = false;
            e.stopPropagation();
        }
        function OnAddButtonClick() {
            var url = 'blankpage.aspx?key=' + 'ADD';

            window.location.href = url;
        }
        function OnAddInvoiceButtonClick(obj) {
            eraseCookie('MenuCloseOpen');

            if (obj != "IST") {
                LoadingPanel.Show();
                var url = 'PosSalesInvoice.aspx?key=ADD&&type=' + obj;
            } else {
                if ($('#hdIsStockLedger').val() == "no") {
                    jAlert("Ledger for Interstate Stk-Out not mapped in Account Heads. Cannot Proceed.");
                    return false;
                } else {
                    LoadingPanel.Show();
                    var url = 'PosSalesInvoice.aspx?key=ADD&&type=' + obj;
                }
            }

            window.location.href = url;
        }
        function openAdvanceReceipt() {
            var url = 'CustomerReceiptPayment.aspx?key=ADD'
            window.location.href = url;
        }
        function OnMoreInfoClick(keyValue) {
            var ActiveUser = '<%=Session["userid"]%>'
            var url = 'posSalesInvoice.aspx?key=' + keyValue + '&Permission=3';
            window.location.href = url;

            //if (ActiveUser != null) {
            //    $.ajax({
            //        type: "POST",
            //        url: "SalesInvoiceList.aspx/GetEditablePermission",
            //        data: "{'ActiveUser':'" + ActiveUser + "'}",
            //        contentType: "application/json; charset=utf-8",
            //        dataType: "json",
            //        success: function (msg) {
            //            var status = msg.d;
            //            var url = 'posSalesInvoice.aspx?key=' + keyValue + '&Permission=' + status;
            //            window.location.href = url;
            //        }
            //    });
            //}
        }
        function viewDocument(keyValue) {
            var url = 'posSalesInvoice.aspx?key=' + keyValue + '&Viemode=1';
            window.location.href = url;
        }
        

        function OnClickDeleteIST(keyValue) {
            jConfirm('Confirm delete?', 'Confirmation Dialog', function (r) {
                if (r == true) {
                    cIstGrid.PerformCallback('Delete~' + keyValue);
                }
            });
        }


        function PopulateCurrentBankBalance(BranchId) {
            var frDate = cFormDate.GetDate().format('yyyy-MM-dd');
            var toDate = ctoDate.GetDate().format('yyyy-MM-dd');

            $.ajax({
                type: "POST",
                url: 'PosSalesInvoicelist.aspx/GetCurrentBankBalance',
                data: JSON.stringify({ BranchId: BranchId, fromDate: frDate, todate: toDate }),

                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var list = msg.d;

                    if (msg.d.length > 0) {
                        document.getElementById("pageheaderContent").style.display = 'block';
                        if (msg.d.split('~')[0] != '') {

                            document.getElementById('<%=B_BankBalance.ClientID %>').innerHTML = msg.d.split('~')[0];
                            document.getElementById('<%=B_BankBalance.ClientID %>').style.color = "Black";
                        }
                        else {
                            document.getElementById('<%=B_BankBalance.ClientID %>').innerHTML = '0.0';
                            document.getElementById('<%=B_BankBalance.ClientID %>').style.color = "Black";

                        }
                    }

                },

            });

        }
        function disp_prompt(name) {

            if (name == "tab0") {
                document.location.href = "PosSalesInvoiceList.aspx";
            }
            if (name == "tab1") {
                //document.location.href = "PosInvoiceList.aspx";
            }
            if (name == "tab2") {
                //document.location.href = "PosInvoiceList.aspx";
            }

        }
        function OnPosBranchAssignopen() {
           
        }
        function AssignedBranchSelectedIndexChanged() {
            cAssignedWareHouse.PerformCallback(cAssignedBranch.GetValue());

            cBranchAssignmentBranch.SetValue(cAssignedBranch.GetValue());
            //updateAssignmentGrid();
        }
        function BranchAssignmentBranchSelectedIndexChanged() {
            cAssignedBranch.SetValue(cBranchAssignmentBranch.GetValue());
            //    AssignedBranchSelectedIndexChanged(cBranchAssignmentBranch);
            cAssignedWareHouse.PerformCallback(cAssignedBranch.GetValue());
        }
        function AssignmentGridEndCallback() {
            if (cAssignmentGrid.cpMsg) {
                if (cAssignmentGrid.cpMsg != '') {
                    jAlert(cAssignmentGrid.cpMsg, 'Alert', function () {
                        cAssignmentPopUp.Hide();
                        if (page.activeTabIndex == 0) {
                            //cGrdQuotation.PerformCallback('RefreshGrid');
                            //cGrdQuotation.Refresh();
                        }
                        else {
                            cIstGrid.PerformCallback('RefreshGrid');
                        }
                    });
                    cAssignmentGrid.cpMsg = null;
                }
            }
        }
    </script>
    <style>
        .smllpad > tbody > tr > td {
            padding-right: 25px;
        }

        .errorField {
            position: absolute;
            right: 5px;
            top: 9px;
        }

        .padTab {
            margin-bottom: 4px;
        }

            .padTab > tbody > tr > td {
                padding-right: 15px;
                vertical-align: middle;
            }

                .padTab > tbody > tr > td > label, .padTab > tbody > tr > td > input[type="button"] {
                    margin-bottom: 0 !important;
                }

        .backBranch {
            font-weight: 600;
            background: #75c1f5;
            padding: 5px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="panel-heading">
        <div class="panel-title clearfix">
            <h3 class="pull-left">Sales Invoice (POS)  
                 <%--<div  style="font-size: 14px;"> : <span class="backBranch"><asp:Label runat="server" ID="branchName" Text=""></asp:Label></span></div>--%></h3>
            <div id="pageheaderContent" class="scrollHorizontal pull-right wrapHolder content horizontal-images">
                <div class="Top clearfix">
                    <ul>
                        <li>
                            <div class="lblHolder" id="idCashbalanace">
                                <table>
                                    <tbody>
                                        <tr>
                                            <td>Cash Balance </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div style="width: 100%;">
                                                    <b style="text-align: center" id="B_BankBalance" runat="server">0.00</b>
                                                </div>

                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </li>
                        <li>
                            <div class="lblHolder">
                                <table>
                                    <tbody>
                                        <tr>
                                            <td>For Unit </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div style="width: 100%;">
                                                    <asp:Label runat="server" ID="branchName" Text=""></asp:Label>
                                                </div>

                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </li>
                    </ul>
                </div>
            </div>
        </div>



        <table class="padTab">
            <tr>
                <td>
                    <label>From Date</label></td>
                <td>
                    <dxe:ASPxDateEdit ID="FormDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cFormDate" Width="100%">
                        <ButtonStyle Width="13px">
                        </ButtonStyle>
                    </dxe:ASPxDateEdit>
                </td>
                <td>
                    <label>To Date</label>
                </td>
                <td>
                    <dxe:ASPxDateEdit ID="toDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="ctoDate" Width="100%">
                        <ButtonStyle Width="13px">
                        </ButtonStyle>
                    </dxe:ASPxDateEdit>

                </td>
                <td>Unit</td>
                <td>
                    <dxe:ASPxComboBox ID="cmbBranchfilter" runat="server" ClientInstanceName="ccmbBranchfilter" Width="100%">
                    </dxe:ASPxComboBox>
                </td>
                <td>
                    <input type="button" value="Show" class="btn btn-primary" onclick="updateGridByDate()" />
                </td>

            </tr>

        </table>





    </div>
    <div class="form_main">
        <div class="clearfix">
            <% if (rights.CanAdd)
               { %>
            <a href="javascript:void(0);" onclick="OnAddInvoiceButtonClick('Cash')" class="btn btn-success btn-radius "><span class="btn-icon"><i class="fa fa-plus" ></i></span><span>Add <u>C</u>ash Invoice</span> </a>
            <a href="javascript:void(0);" onclick="OnAddInvoiceButtonClick('Crd')" class="btn btn-success btn-radius"><span class="btn-icon"><i class="fa fa-plus" ></i></span><span>Add Cre<u>d</u>it Invoice</span> </a>
            <a href="javascript:void(0);" onclick="OnAddInvoiceButtonClick('Fin')" class="btn btn-success btn-radius"><span class="btn-icon"><i class="fa fa-plus" ></i></span><span>Add <u>F</u>inance Invoice</span> </a>
            <a href="javascript:void(0);" onclick="OnAddInvoiceButtonClick('IST')" class="btn btn-success btn-radius"><span class="btn-icon"><i class="fa fa-plus" ></i></span><span>Interstate <u>S</u>tock Transfer</span> </a>

            <a href="javascript:void(0);" onclick="ShowReceiptPayment()" class="btn btn-success btn-radius"><span class="btn-icon"><i class="fa fa-plus" ></i></span><span>Add <u>R</u>eceipt/Payment</span> </a>


            <%} %>

            <% if (rights.CanAssignbranch)
               { %>
            <a id="amassbranch" href="javascript:void(0);" onclick="MassBranchAssign()" class="btn btn-primary btn-radius"><span><u>M</u>ass Unit Assign</span> </a>
            <%} %>
            <%--<a href="javascript:void(0);" class="btn btn-danger"><span>Delete</span> </a>--%>


            <a href="javascript:void(0);" onclick="InvoiceWatingClick()" class="btn btn-warning btn-radius hide"><span><u>I</u>nvoice Waiting </span>
                <dxe:ASPxLabel runat="server" Text="" ID="lblweatingCount" ClientInstanceName="clblweatingCount"></dxe:ASPxLabel>
            </a>



            <% if (rights.CanExport)
               { %>
            <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-primary pull-right btn-radius" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true" OnChange="if(!AvailableExportOption()){return false;}">
                <asp:ListItem Value="0">Export to</asp:ListItem>
                <asp:ListItem Value="1">PDF</asp:ListItem>
                <asp:ListItem Value="2">XLS</asp:ListItem>
                <asp:ListItem Value="3">RTF</asp:ListItem>
                <asp:ListItem Value="4">CSV</asp:ListItem>
            </asp:DropDownList>
            <% } %>

            <i class="fa fa-reply blink hide" style="font-size: 20px; margin-right: 10px;" aria-hidden="true"></i>
        </div>
    </div>
    <div class="PopUpArea">
        <dxe:ASPxPopupControl ID="ASPxDocumentsPopup" runat="server" ClientInstanceName="cDocumentsPopup"
            Width="350px" HeaderText="Select Design(s)" PopupHorizontalAlign="WindowCenter"
            PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
            Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True">
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">
                    <%--   <dxe:ASPxGridView runat="server" KeyFieldName="ID" ClientInstanceName="cgriddocuments" ID="grid_Documents"
                        Width="100%" SettingsBehavior-AllowSort="false" SettingsBehavior-AllowDragDrop="false" SettingsPager-Mode="ShowAllRecords" OnCustomCallback="cgridDocuments_CustomCallback" ClientSideEvents-EndCallback="cgridDocumentsEndCall"
                        Settings-ShowFooter="false" AutoGenerateColumns="False"
                        Settings-VerticalScrollableHeight="100" Settings-VerticalScrollBarMode="Hidden">
                                                      
                        <SettingsBehavior AllowDragDrop="False" AllowSort="False"></SettingsBehavior>
                        <SettingsPager Visible="false"></SettingsPager>
                        <Columns>
                            <dxe:GridViewCommandColumn ShowSelectCheckbox="True" Width="10" Caption=" "  />




                            <dxe:GridViewDataTextColumn VisibleIndex="1" FieldName="ID" Width="0" ReadOnly="true" Caption="No." CellStyle-CssClass="hide" HeaderStyle-CssClass="hide">
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="NAME" Width="" ReadOnly="true" Caption="Design(s)">
                            </dxe:GridViewDataTextColumn>
                        </Columns>
                      
                    </dxe:ASPxGridView>--%>
                    <dxe:ASPxCallbackPanel runat="server" ID="SelectPanel" ClientInstanceName="cSelectPanel" OnCallback="SelectPanel_Callback" ClientSideEvents-EndCallback="cSelectPanelEndCall">
                        <PanelCollection>
                            <dxe:PanelContent runat="server">



                                <%--   <dxe:ASPxCheckBox ID="selectOriginal" Text="Original" runat="server" ToolTip="Select Original"
                                    ClientInstanceName="CselectOriginal"  ClientSideEvents-CheckedChanged="function(s, e) { 
                                      grid.PerformCallback(s.GetChecked()+'^'+'stock'); }">
                                   </dxe:ASPxCheckBox>--%>

                                <%--<dxe:ASPxCheckBox ID="selectOriginal" Text="Original For Recipient" runat="server" ToolTip="Select Original" ClientSideEvents-CheckedChanged="OrginalCheckChange"
                                    ClientInstanceName="CselectOriginal">
                                </dxe:ASPxCheckBox>

                                <dxe:ASPxCheckBox ID="selectDuplicate" Text="Duplicate For Transporter" runat="server" ToolTip="Select Duplicate" ClientSideEvents-CheckedChanged="DuplicateCheckChange"
                                    ClientInstanceName="CselectDuplicate">
                                </dxe:ASPxCheckBox>--%>
                                <dxe:ASPxCheckBox ID="selectOriginal" Text="Original For Recipient" runat="server" ToolTip="Select Original" 
                                    ClientInstanceName="CselectOriginal">
                                </dxe:ASPxCheckBox>
                                <dxe:ASPxCheckBox ID="selectDuplicate" Text="Duplicate For Transporter" runat="server" ToolTip="Select Duplicate" 
                                    ClientInstanceName="CselectDuplicate">
                                </dxe:ASPxCheckBox>
                                <dxe:ASPxCheckBox ID="selectFDuplicate" Text="Duplicate For Financer" runat="server" ToolTip="Select Duplicate For Financer" 
                                    ClientInstanceName="CselectFDuplicate">
                                </dxe:ASPxCheckBox>
                                <dxe:ASPxCheckBox ID="selectTriplicate" Text="Triplicate For Supplier" runat="server" ToolTip="Select Triplicate"
                                    ClientInstanceName="CselectTriplicate">
                                </dxe:ASPxCheckBox>

                                <dxe:ASPxComboBox ID="CmbDesignName" ClientInstanceName="cCmbDesignName" runat="server" ValueType="System.String" Width="100%" EnableSynchronization="True">
                                    <%--<Items>
                                        <dxe:ListEditItem Selected="True" Text="Default" Value="1"></dxe:ListEditItem>
                                        <dxe:ListEditItem Text="Tax_Invoice" Value="2"></dxe:ListEditItem>
                                    </Items>--%>
                                    <%-- <ClientSideEvents ValueChanged="function(s,e){OnCmbCountryName_ValueChange()}"></ClientSideEvents>--%>
                                </dxe:ASPxComboBox>

                                <div class="text-center pTop10">
                                    <dxe:ASPxButton ID="btnOK" ClientInstanceName="cbtnOK" runat="server" AutoPostBack="False" Text="OK" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                                        <ClientSideEvents Click="function(s, e) {return PerformCallToGridBind();}" />
                                    </dxe:ASPxButton>

                                </div>


                                <asp:HiddenField ID="HdInvoiceType" runat="server" />


                            </dxe:PanelContent>
                        </PanelCollection>
                    </dxe:ASPxCallbackPanel>



                </dxe:PopupControlContentControl>

            </ContentCollection>
        </dxe:ASPxPopupControl>
    </div>

    <dxe:ASPxPageControl ID="ASPxPageControl1" runat="server"  ClientInstanceName="page"
        Font-Size="12px" Width="100%">
        <TabPages>
            <dxe:TabPage Name="POS" Text="POS">
                <ContentCollection>
                    <dxe:ContentControl runat="server">
                        <div class="GridViewArea">
                            <asp:HiddenField ID="hiddenedit" runat="server" />
                        </div>
                        <div style="display: none">
                            <dxe:ASPxGridViewExporter ID="exporter" GridViewID="GrdQuotation" runat="server" Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
                            </dxe:ASPxGridViewExporter>
                        </div>

                    </dxe:ContentControl>
                </ContentCollection>
            </dxe:TabPage>
            <dxe:TabPage Name="AdvanceRec" Text="Money Receipt">
                <ContentCollection>
                    <dxe:ContentControl runat="server">


                        <div class="GridViewArea">
                            <%--<dxe:ASPxGridView ID="CustomerReceiptGrid" runat="server" KeyFieldName="ReceiptPayment_ID" AutoGenerateColumns="False"
                                Width="100%" ClientInstanceName="cCustomerReceiptGrid" SettingsBehavior-AllowFocusedRow="true" OnCustomCallback="CustomerReceiptGrid_CustomCallback"
                                SettingsBehavior-AllowSelectSingleRowOnly="false" SettingsBehavior-AllowSelectByRowClick="true"
                                SettingsBehavior-ColumnResizeMode="Control" OnDataBinding="CustomerReceiptGrid_DataBinding" OnSummaryDisplayText="ShowGrid_SummaryDisplayText">--%>
                            <dxe:ASPxGridView ID="CustomerReceiptGrid" runat="server" KeyFieldName="ReceiptPayment_ID" AutoGenerateColumns="False"
                                Width="100%" ClientInstanceName="cCustomerReceiptGrid" SettingsBehavior-AllowFocusedRow="true" 
                                OnCustomCallback="CustomerReceiptGrid_CustomCallback"
                                SettingsBehavior-AllowSelectSingleRowOnly="false" SettingsBehavior-AllowSelectByRowClick="true"
                                SettingsBehavior-ColumnResizeMode="Control" DataSourceID="LinqServerModeDataSource1" OnSummaryDisplayText="ShowGrid_SummaryDisplayText">

                                <Columns>

                                    <dxe:GridViewDataTextColumn Caption="Sl No." FieldName="SlNo" Width="50"
                                        VisibleIndex="0" FixedStyle="Left" Visible="false">
                                        <CellStyle CssClass="gridcellleft" Wrap="true">
                                        </CellStyle>
                                        <Settings AutoFilterCondition="Contains" />
                                    </dxe:GridViewDataTextColumn>

                                      <dxe:GridViewDataTextColumn Caption="Voucher Type" FieldName="ReceiptPayment_TransactionType" Width="85" 
                                        VisibleIndex="1" FixedStyle="Left">
                                        <CellStyle CssClass="gridcellleft" Wrap="true">
                                        </CellStyle>
                                        <Settings AutoFilterCondition="Contains" />
                                    </dxe:GridViewDataTextColumn>


                                    <dxe:GridViewDataTextColumn Caption="Document No." FieldName="ReceiptPayment_VoucherNumber" Width="200"
                                        VisibleIndex="2" FixedStyle="Left">
                                        <CellStyle CssClass="gridcellleft" Wrap="true">
                                        </CellStyle>
                                        <Settings AutoFilterCondition="Contains" />
                                    </dxe:GridViewDataTextColumn>

                                    <dxe:GridViewDataTextColumn Caption="Posting Date" FieldName="ReceiptPayment_TransactionDt" 
                                        VisibleIndex="3">
                                        <CellStyle CssClass="gridcellleft" Wrap="true">
                                        </CellStyle>
                                        <Settings AutoFilterCondition="Contains" />
                                    </dxe:GridViewDataTextColumn>

                                    <dxe:GridViewDataTextColumn Caption="Customer" FieldName="Customer" Width="200"
                                        VisibleIndex="4">
                                        <CellStyle CssClass="gridcellleft" Wrap="true">
                                        </CellStyle>
                                        <Settings AutoFilterCondition="Contains" />
                                    </dxe:GridViewDataTextColumn>

                                    <dxe:GridViewDataTextColumn Caption="Currency" FieldName="ReceiptPayment_Currency" Width="70"
                                        VisibleIndex="5" Settings-AllowAutoFilter="False">
                                        <CellStyle CssClass="gridcellleft" Wrap="true">
                                        </CellStyle>
                                        <Settings AutoFilterCondition="Contains" />
                                    </dxe:GridViewDataTextColumn>

                                    <dxe:GridViewDataTextColumn Caption="Amount" FieldName="Amount" 
                                        VisibleIndex="6">
                                        <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                                        <CellStyle CssClass="gridcellleft" Wrap="true">
                                        </CellStyle>
                                        <Settings AutoFilterCondition="Contains" />
                                    </dxe:GridViewDataTextColumn>

                                    <dxe:GridViewDataTextColumn Caption="Created By" FieldName="ReceiptPayment_CreateUser" Width="200"
                                        VisibleIndex="7" Settings-AllowAutoFilter="False">
                                        <CellStyle CssClass="gridcellleft" Wrap="true">
                                        </CellStyle>
                                        <Settings AutoFilterCondition="Contains" />
                                    </dxe:GridViewDataTextColumn>

                                    <dxe:GridViewDataTextColumn Caption="Modify By" FieldName="ReceiptPayment_ModifyUser" Width="200"
                                        VisibleIndex="8" Settings-AllowAutoFilter="False">
                                        <CellStyle CssClass="gridcellleft" Wrap="true">
                                        </CellStyle>
                                        <Settings AutoFilterCondition="Contains" />
                                    </dxe:GridViewDataTextColumn>



                                    <dxe:GridViewDataTextColumn HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="center" VisibleIndex="9" Width="15%">
                                        <DataItemTemplate>
                                            <a href="javascript:void(0);" onclick="OnCustReceiptViewClick('<%# Container.KeyValue %>')" class="pad" title="View">
                                                <img src="../../../assests/images/viewIcon.png" /></a>

                                            <a href="javascript:void(0);" onclick="onCustomerReceiptPrint('<%# Container.KeyValue %>')" class="pad" title="print">
                                                <img src="../../../assests/images/Print.png" />
                                            </a>


                                        </DataItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                        <CellStyle HorizontalAlign="Center"></CellStyle>
                                        <HeaderTemplate><span>Actions</span></HeaderTemplate>
                                        <EditFormSettings Visible="False"></EditFormSettings>
                                    </dxe:GridViewDataTextColumn>

                                </Columns>
                                <SettingsPager PageSize="10">
                                    <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,20,50,100,150,200" />
                                </SettingsPager>

                               <%-- <SettingsSearchPanel Visible="True" />--%>
                                <Settings ShowGroupPanel="True" ShowFooter="true"  ShowGroupFooter="VisibleIfExpanded" ShowStatusBar="Hidden" ShowHorizontalScrollBar="true" ShowFilterRow="true" ShowFilterRowMenu="true" />
                                <SettingsLoadingPanel Text="Please Wait..." />

                                <TotalSummary>
                                        <dxe:ASPxSummaryItem FieldName="Amount" SummaryType="Sum" /> 
                                  </TotalSummary>


                            </dxe:ASPxGridView>
                            <dx:LinqServerModeDataSource ID="LinqServerModeDataSource1" runat="server" OnSelecting="EntityServerModeDataSource1_Selecting"
                                ContextTypeName="ERPDataClassesDataContext" TableName="v_CustomerReceiptPaymentList" />
                            <asp:HiddenField ID="hfIsFilterReceipt" runat="server" />
                            <asp:HiddenField ID="hfFromDateReceipt" runat="server" />
                            <asp:HiddenField ID="hfToDateReceipt" runat="server" />
                            <asp:HiddenField ID="hfBranchIDReceipt" runat="server" />
                        </div>
                    </dxe:ContentControl>
                </ContentCollection>
            </dxe:TabPage>
            <dxe:TabPage Name="IST" Text="Interstate Stock Transfer">
                <ContentCollection>
                    <dxe:ContentControl runat="server">
                        <div class="GridViewArea">
                            <%--<dxe:ASPxGridView ID="IstGrid" runat="server" KeyFieldName="Invoice_Id" AutoGenerateColumns="False"
                                Width="100%" ClientInstanceName="cIstGrid" OnCustomCallback="IstGrid_CustomCallback" SettingsBehavior-AllowFocusedRow="true"
                                SettingsBehavior-AllowSelectSingleRowOnly="false" SettingsBehavior-AllowSelectByRowClick="true" OnDataBinding="IstGrid_DataBinding" 
                                Settings-HorizontalScrollBarMode="Auto"
                                SettingsCookies-Enabled="true" SettingsCookies-StorePaging="true" SettingsCookies-StoreFiltering="true" 
                                SettingsCookies-StoreGroupingAndSorting="true" SettingsBehavior-ColumnResizeMode="Control"
                                OnSummaryDisplayText="ShowGrid_SummaryDisplayText">--%>
                                <dxe:ASPxGridView ID="IstGrid" runat="server" KeyFieldName="Invoice_Id" AutoGenerateColumns="False"
                                Width="100%" ClientInstanceName="cIstGrid" OnCustomCallback="IstGrid_CustomCallback" SettingsBehavior-AllowFocusedRow="true"
                                SettingsBehavior-AllowSelectSingleRowOnly="false" SettingsBehavior-AllowSelectByRowClick="true" 
                                Settings-HorizontalScrollBarMode="Auto"  SettingsBehavior-ColumnResizeMode="Control"
                                OnSummaryDisplayText="ShowGrid_SummaryDisplayText" DataSourceID="LinqServerModeDataSource2" >
                                <ClientSideEvents BeginCallback="OnBeginAfterCallbackIST" />
                                <Columns>

                                    <dxe:GridViewDataTextColumn Caption="Sl No." FieldName="SlNo" Width="50" Visible="false"
                                        VisibleIndex="0" FixedStyle="Left">
                                        <CellStyle CssClass="gridcellleft" Wrap="true">
                                        </CellStyle>
                                        <Settings AutoFilterCondition="Contains" />
                                    </dxe:GridViewDataTextColumn>

                                    <dxe:GridViewDataTextColumn Caption="Document No." FieldName="InvoiceNo" Width="200"
                                        VisibleIndex="0" FixedStyle="Left">
                                        <CellStyle CssClass="gridcellleft" Wrap="true">
                                        </CellStyle>
                                        <Settings AutoFilterCondition="Contains" />
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn Caption="Posting Date" FieldName="Invoice_Date"
                                        VisibleIndex="1">
                                        <CellStyle CssClass="gridcellleft" Wrap="true">
                                        </CellStyle>
                                        <Settings AutoFilterCondition="Contains" />
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn Caption="Customer" FieldName="CustomerName"
                                        VisibleIndex="2" Width="300">
                                        <CellStyle CssClass="gridcellleft" Wrap="true">
                                        </CellStyle>
                                        <Settings AutoFilterCondition="Contains" />
                                    </dxe:GridViewDataTextColumn>

                                    <dxe:GridViewDataTextColumn Caption="Net Amount" FieldName="NetAmount"
                                        VisibleIndex="3">
                                        <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                                        <CellStyle CssClass="gridcellleft" Wrap="true">
                                        </CellStyle>
                                        <Settings AutoFilterCondition="Contains" />
                                    </dxe:GridViewDataTextColumn>

                                    <dxe:GridViewDataTextColumn Caption="Type" FieldName="Pos_EntryType" Width="100"
                                        VisibleIndex="4">
                                        <CellStyle CssClass="gridcellleft" Wrap="true">
                                        </CellStyle>
                                        <Settings AutoFilterCondition="Contains" />
                                    </dxe:GridViewDataTextColumn>

                                    <dxe:GridViewDataTextColumn Caption="Delv. Type" FieldName="Pos_DeliveryType"
                                        VisibleIndex="4" Width="80">
                                        <CellStyle CssClass="gridcellleft" Wrap="true">
                                        </CellStyle>
                                        <Settings AutoFilterCondition="Contains" />
                                    </dxe:GridViewDataTextColumn>

                                    <dxe:GridViewDataTextColumn Caption="Delv. Date" FieldName="Pos_DeliveryDate"
                                        VisibleIndex="4">
                                        <CellStyle CssClass="gridcellleft" Wrap="true">
                                        </CellStyle>
                                        <Settings AutoFilterCondition="Contains" />
                                    </dxe:GridViewDataTextColumn>

                                    <dxe:GridViewDataTextColumn Caption="Delv. Status" FieldName="DelvStatus"
                                        VisibleIndex="4">
                                        <CellStyle CssClass="gridcellleft" Wrap="true">
                                        </CellStyle>
                                        <Settings AutoFilterCondition="Contains" />
                                    </dxe:GridViewDataTextColumn>

                                    <dxe:GridViewDataTextColumn Caption="Challan No" FieldName="ChallanNo"
                                        VisibleIndex="4" Width="180">
                                        <CellStyle CssClass="gridcellleft" Wrap="true">
                                        </CellStyle>
                                        <Settings AutoFilterCondition="Contains" />
                                    </dxe:GridViewDataTextColumn>

                                    <dxe:GridViewDataTextColumn Caption="Challan Date" FieldName="ChallanDate"
                                        VisibleIndex="4">
                                        <CellStyle CssClass="gridcellleft" Wrap="true">
                                        </CellStyle>
                                        <Settings AutoFilterCondition="Contains" />
                                    </dxe:GridViewDataTextColumn>

                                    <dxe:GridViewDataTextColumn Caption="SRN?" FieldName="isSRN"
                                        VisibleIndex="4" Width="40">
                                        <CellStyle CssClass="gridcellleft" Wrap="true">
                                        </CellStyle>
                                        <Settings AutoFilterCondition="Contains" />
                                    </dxe:GridViewDataTextColumn>

                                    <dxe:GridViewDataTextColumn Caption="SRN No." FieldName="SRNno"
                                        VisibleIndex="4">
                                        <CellStyle CssClass="gridcellleft" Wrap="true">
                                        </CellStyle>
                                        <Settings AutoFilterCondition="Contains" />
                                    </dxe:GridViewDataTextColumn>

                                    <dxe:GridViewDataTextColumn Caption="SRN Date" FieldName="SrnDate"
                                        VisibleIndex="4">
                                        <CellStyle CssClass="gridcellleft" Wrap="true">
                                        </CellStyle>
                                        <Settings AutoFilterCondition="Contains" />
                                    </dxe:GridViewDataTextColumn>

                                    <dxe:GridViewDataTextColumn Caption="CD Date" FieldName="cdDate"
                                        VisibleIndex="4">
                                        <CellStyle CssClass="gridcellleft" Wrap="true">
                                        </CellStyle>
                                        <Settings AutoFilterCondition="Contains" />
                                    </dxe:GridViewDataTextColumn>

                                    <dxe:GridViewDataTextColumn Caption="Assigned Unit" FieldName="pos_assignBranch"
                                        VisibleIndex="4">
                                        <CellStyle CssClass="gridcellleft" Wrap="true">
                                        </CellStyle>
                                        <Settings AutoFilterCondition="Contains" />
                                    </dxe:GridViewDataTextColumn>


                                    <dxe:GridViewDataTextColumn Caption="Entered By" FieldName="EnteredBy"
                                        VisibleIndex="4">
                                        <CellStyle CssClass="gridcellleft" Wrap="true">
                                        </CellStyle>
                                        <Settings AutoFilterCondition="Contains" />
                                    </dxe:GridViewDataTextColumn>
                                  
                                    
                                      <%--ist--%>


                                    <dxe:GridViewDataTextColumn HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="center" VisibleIndex="17" Width="140">
                                        <DataItemTemplate>
                                            <% if (rights.CanEdit)
                                               { %>
                                            <a href="javascript:void(0);" onclick="OnMoreInfoClick('<%# Container.KeyValue %>')" id="ist_editInvoice" class="pad" title="Edit">
                                                <img src="../../../assests/images/info.png" /></a><%} %>
                                            <% if (rights.CanDelete)
                                               { %>
                                               <a href="javascript:void(0);" onclick="OnClickDeleteIST('<%# Container.KeyValue %>')" class="pad" title="Delete"  id="ist_delete">
                                                                            <img src="../../../assests/images/Delete.png" /></a>
                                            <%} %>
                                            
                                            <% if (rights.CanView)
                                               { %>
                                            <a href="javascript:void(0);" onclick="OnclickViewAttachment('<%# Container.KeyValue %>')" class="pad" title="View Attachment">
                                                <img src="../../../assests/images/attachment.png" />
                                            </a><%} %>

                                            <% if (rights.CanView)
                                               { %>
                                            <a href="javascript:void(0);" onclick="viewDocument('<%# Container.KeyValue %>')" class="pad" title="View Document">
                                                <img src="../../../assests/images/viewIcon.png" />
                                            </a><%} %>
                                            <% if (rights.CanPrint)
                                               { %>
                                            <a href="javascript:void(0);" onclick="onPrintJvIST('<%# Container.KeyValue %>','<%# Container.VisibleIndex %>')" class="pad" title="print">
                                                <img src="../../../assests/images/Print.png" />
                                            </a><%} %>
                                            <% if (rights.CanAssignbranch)
                                               { %>
                                            <a href="javascript:void(0);" class="pad" title="Branch Assignment" id="ist_Assignment" onclick="onViewBranchAssignment('<%# Container.KeyValue %>')" style="display: none;">
                                                <span class="fa fa-truck out"></span>
                                            </a>
                                            <%} %>

                                            <% if (rights.Cancancelassignmnt)
                                               { %>
                                            <a href="javascript:void(0);" class="pad" title="Cancel Branch Assignment" id="istCancel_Assignment" onclick="onISTCancelBranchAssignment('<%# Container.KeyValue %>')" style="display: none;">
                                                <img src="../../../assests/images/crossT.png" />
                                            </a>
                                            <%} %>
                                        </DataItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                        <CellStyle HorizontalAlign="Center"></CellStyle>
                                        <HeaderTemplate><span>Actions</span></HeaderTemplate>
                                        <EditFormSettings Visible="False"></EditFormSettings>

                                    </dxe:GridViewDataTextColumn>
                                </Columns>
                               

                                <SettingsPager PageSize="10">
                                    <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,20,50,100,150,200" />
                                </SettingsPager>

                             <%--   <SettingsSearchPanel Visible="True" />--%>
                                <Settings ShowGroupPanel="True" ShowFooter="true"  ShowGroupFooter="VisibleIfExpanded"  ShowStatusBar="Hidden" ShowHorizontalScrollBar="true" ShowFilterRow="true" ShowFilterRowMenu="true" />
                                <SettingsLoadingPanel Text="Please Wait..." />

                                <ClientSideEvents EndCallback="ListingISTGridEndCallback" />

                                  <TotalSummary>
                                        <dxe:ASPxSummaryItem FieldName="NetAmount" SummaryType="Sum" /> 
                                  </TotalSummary>


                            </dxe:ASPxGridView>
                            <dx:LinqServerModeDataSource ID="LinqServerModeDataSource2" runat="server" OnSelecting="EntityServerModeDataSource2_Selecting"
                                ContextTypeName="ERPDataClassesDataContext" TableName="v_posListIST" />
                            <asp:HiddenField ID="hfIsFilterIst" runat="server" />
                            <asp:HiddenField ID="hfFromDateIst" runat="server" />
                            <asp:HiddenField ID="hfToDateIst" runat="server" />
                            <asp:HiddenField ID="hfBranchIDIst" runat="server" />


                            <asp:HiddenField ID="HiddenField1" runat="server" />
                        </div>


                         </dxe:ContentControl>
                </ContentCollection>
            </dxe:TabPage>
        </TabPages>
        <ClientSideEvents ActiveTabChanged="function(s, e) {
	                                            var activeTab   = page.GetActiveTab();
	                                            var Tab0 = page.GetTab(0);
	                                            var Tab1 = page.GetTab(1);
	                                            var Tab2 = page.GetTab(2);
	                                            if(activeTab == Tab0)
	                                            {
	                                                disp_prompt('tab0');
	                                            }
	                                            if(activeTab == Tab1)
	                                            {
	                                                disp_prompt('tab1');
	                                            }
                                                if(activeTab == Tab2)
	                                            {
	                                                disp_prompt('tab2');
	                                            }
	                                            }"></ClientSideEvents>
    </dxe:ASPxPageControl>


    <dxe:ASPxPopupControl ID="ASPXPopupControl" runat="server" Width="1200"
        CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="waitingPopUp"
        HeaderText="Invoice Waiting" AllowResize="false" ResizingMode="Postponed">
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">

                <div onkeypress="OnWaitingGridKeyPress(event)">

                    <dxe:ASPxGridView ID="watingInvoicegrid" runat="server" KeyFieldName="SBMain_Id" AutoGenerateColumns="False"
                        Width="100%" ClientInstanceName="cwatingInvoicegrid" OnCustomCallback="watingInvoicegrid_CustomCallback" KeyboardSupport="true"
                        SettingsBehavior-AllowSelectSingleRowOnly="true" OnDataBinding="watingInvoicegrid_DataBinding" SettingsBehavior-AllowFocusedRow="true"
                        Settings-VerticalScrollBarMode="auto" Settings-VerticalScrollableHeight="320">
                        <Columns>
                            <%--<dxe:GridViewCommandColumn ShowSelectCheckbox="true" VisibleIndex="0" Width="60" Caption="Select" />--%>

                            <dxe:GridViewDataTextColumn Caption="Salesman" FieldName="Salesman"
                                VisibleIndex="0" FixedStyle="Left">
                                <CellStyle CssClass="gridcellleft" Wrap="true">
                                </CellStyle>
                                <Settings AutoFilterCondition="Contains" />
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn Caption="Unit" FieldName="Branch"
                                VisibleIndex="0" FixedStyle="Left">
                                <CellStyle CssClass="gridcellleft" Wrap="true">
                                </CellStyle>
                                <Settings AutoFilterCondition="Contains" />
                            </dxe:GridViewDataTextColumn>


                            <dxe:GridViewDataTextColumn Caption="Customer" FieldName="Name"
                                VisibleIndex="1" FixedStyle="Left">
                                <CellStyle CssClass="gridcellleft" Wrap="true">
                                </CellStyle>
                                <Settings AutoFilterCondition="Contains" />
                            </dxe:GridViewDataTextColumn>


                            <dxe:GridViewDataTextColumn Caption="Product List" FieldName="ProductList"
                                VisibleIndex="1" FixedStyle="Left" Width="180px">
                                <CellStyle CssClass="gridcellleft" Wrap="true">
                                </CellStyle>
                                <Settings AutoFilterCondition="Contains" />
                            </dxe:GridViewDataTextColumn>




                            <dxe:GridViewDataTextColumn Caption="Billing Amount" FieldName="finalAmount"
                                VisibleIndex="2" FixedStyle="Left">
                                <CellStyle CssClass="gridcellleft" Wrap="true">
                                </CellStyle>
                                <Settings AutoFilterCondition="Contains" />
                            </dxe:GridViewDataTextColumn>


                            <dxe:GridViewDataTextColumn Caption="Billing Date" FieldName="Billingdate"
                                VisibleIndex="2" FixedStyle="Left">
                                <CellStyle CssClass="gridcellleft" Wrap="true">
                                </CellStyle>
                                <Settings AutoFilterCondition="Contains" />
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn Caption="Payment Type" FieldName="Paymenttype"
                                VisibleIndex="2" FixedStyle="Left">
                                <CellStyle CssClass="gridcellleft" Wrap="true">
                                </CellStyle>
                                <Settings AutoFilterCondition="Contains" />
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="center" VisibleIndex="17" Width="60">
                                <DataItemTemplate>
                                    <% if (rights.CanDelete)
                                       { %>
                                    <a href="javascript:void(0);" onclick="RemoveInvoice('<%# Container.KeyValue %>')" class="pad" title="Remove">
                                        <%--   <img src="../../../assests/images/Delete.png" />--%>
                                        <i class="fa fa-close" aria-hidden="true" id="CloseRemoveWattingBtn" style="font-size: 19px; color: #f35248;"></i>
                                    </a>
                                    <%} %>
                                </DataItemTemplate>
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                <CellStyle HorizontalAlign="Center"></CellStyle>
                                <HeaderTemplate><span>Actions</span></HeaderTemplate>
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                        </Columns>

                        <ClientSideEvents RowClick="ListRowClicked" EndCallback="watingInvoicegridEndCallback" />

                        <SettingsPager NumericButtonCount="20" PageSize="10" ShowSeparators="True" Mode="ShowPager">
                            <FirstPageButton Visible="True">
                            </FirstPageButton>
                            <LastPageButton Visible="True">
                            </LastPageButton>
                        </SettingsPager>
                        <SettingsSearchPanel Visible="True" />
                        <Settings ShowGroupPanel="False" ShowStatusBar="Hidden" ShowHorizontalScrollBar="False" ShowFilterRow="true" ShowFilterRowMenu="true" />
                        <SettingsLoadingPanel Text="Please Wait..." />
                    </dxe:ASPxGridView>
                </div>


                <dxe:ASPxButton ID="InvoiceWattingOk" runat="server" AutoPostBack="false" Text="O&#818;k" CssClass="btn btn-primary okClass"
                    ClientSideEvents-Click="InvoiceWattingOkClick" UseSubmitBehavior="False" />
            </dxe:PopupControlContentControl>
        </ContentCollection>
    </dxe:ASPxPopupControl>


    <%--mass Branch --%>
    <dxe:ASPxPopupControl ID="massBranchPopup" runat="server" Width="1000"
        CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="cmassBranchPopup" Height="650"
        HeaderText="Branch Assignment" AllowResize="false" ResizingMode="Postponed">
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
            </dxe:PopupControlContentControl>
        </ContentCollection>
    </dxe:ASPxPopupControl>

    <%--end of mass branch--%>

    <dxe:ASPxPopupControl ID="AssignmentPopUp" runat="server" Width="900"
        CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="cAssignmentPopUp" Height="500"
        HeaderText="Branch Assignment" AllowResize="false" ResizingMode="Postponed">
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">

                <div id="BranchAssignmentHeader">
                    <dxe:ASPxCallbackPanel runat="server" ID="BranchRequUpdatePanel" ClientInstanceName="cBranchRequUpdatePanel" OnCallback="BranchRequUpdatePanel_Callback">
                        <PanelCollection>
                            <dxe:PanelContent runat="server">

                                <table class="smllpad">
                                    <tr>
                                        <td style="width: 110px"></td>
                                        <td>
                                            <label>Unit</label></td>
                                        <td>
                                            <label>Warehouse</label></td>
                                        <td></td>

                                    </tr>
                                    <tr>
                                        <td style="padding-right: 10px">
                                            <label>Assigned To</label></td>
                                        <td class="relative">
                                            <dxe:ASPxComboBox ID="AssignedBranch" runat="server" ClientInstanceName="cAssignedBranch" Width="100%">
                                                <ClientSideEvents SelectedIndexChanged="AssignedBranchSelectedIndexChanged"></ClientSideEvents>
                                            </dxe:ASPxComboBox>
                                            <span id="MandatoryBranchAssign" style="display: none" class="errorField">
                                                <img id="MandatoryBranchAssignid" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory">
                                            </span>
                                        </td>
                                        <td class="relative">
                                            <dxe:ASPxComboBox ID="AssignedWareHouse" runat="server" OnCallback="AssignedWareHouse_Callback" ClientInstanceName="cAssignedWareHouse" SelectedIndex="1" Width="100%">
                                            </dxe:ASPxComboBox>
                                            <span id="mandetoryAssignedWareHouse" style="display: none" class="errorField">
                                                <img id="idmandetoryAssignedWareHouse" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory">
                                            </span>
                                        </td>
                                        <td>
                                            <input type="button" value="Assign" class="btn btn-primary" onclick="AssignBranchToThisInvoice()" />
                                            <input type="button" value="Cancel" class="btn btn-danger" onclick="CancelBranchToThisInvoice()" />
                                        </td>

                                    </tr>
                                </table>
                            </dxe:PanelContent>
                        </PanelCollection>
                    </dxe:ASPxCallbackPanel>
                </div>
                <table class="smllpad" style="margin-top: 15px;">
                    <tr>

                        <td style="width: 110px">Select Unit To View Stock </td>
                        <td>
                            <dxe:ASPxComboBox ID="BranchAssignmentBranch" runat="server" ClientInstanceName="cBranchAssignmentBranch" Width="100%">
                                <ClientSideEvents SelectedIndexChanged="BranchAssignmentBranchSelectedIndexChanged"></ClientSideEvents>
                            </dxe:ASPxComboBox>
                        </td>
                        <td>
                            <a href="#" onclick="updateAssignmentGrid()">
                                <button type="button" class="btn btn-primary "><i class="fa fa-search" style="" aria-hidden="true"></i>View Stock</button></a>
                            <%--   <input type="button" value="Show Stock" class="btn btn-primary" onclick="updateAssignmentGrid()" />--%>
                        </td>

                    </tr>

                </table>


                <dxe:ASPxGridView ID="AssignmentGrid" runat="server" KeyFieldName="InvoiceDetails_Id" AutoGenerateColumns="False"
                    Width="100%" ClientInstanceName="cAssignmentGrid" OnCustomCallback="AssignmentGrid_CustomCallback" KeyboardSupport="true"
                    SettingsBehavior-AllowSelectSingleRowOnly="true" OnDataBinding="AssignmentGrid_DataBinding" SettingsBehavior-AllowFocusedRow="true">
                    <Columns>


                        <dxe:GridViewDataTextColumn Caption="Code" FieldName="sProducts_Code"
                            VisibleIndex="0" FixedStyle="Left">
                            <CellStyle CssClass="gridcellleft" Wrap="true">
                            </CellStyle>
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>

                        <dxe:GridViewDataTextColumn Caption="Name" FieldName="InvoiceDetails_ProductDescription"
                            VisibleIndex="0" FixedStyle="Left">
                            <CellStyle CssClass="gridcellleft" Wrap="true">
                            </CellStyle>
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>

                        <dxe:GridViewDataTextColumn Caption="Available Stock" FieldName="availableQty"
                            VisibleIndex="0" FixedStyle="Left">
                            <CellStyle CssClass="gridcellleft" Wrap="true">
                            </CellStyle>
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>

                        <dxe:GridViewDataTextColumn Caption="Curently Invoiced" FieldName="InvoicedBalance"
                            VisibleIndex="0" FixedStyle="Left">
                            <CellStyle CssClass="gridcellleft" Wrap="true">
                            </CellStyle>
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>

                        <dxe:GridViewDataTextColumn Caption="Actual Balance" FieldName="Actual_Balance"
                            VisibleIndex="0" FixedStyle="Left">
                            <CellStyle CssClass="gridcellleft" Wrap="true">
                            </CellStyle>
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>

                    </Columns>

                    <ClientSideEvents EndCallback="AssignmentGridEndCallback" />


                    <SettingsSearchPanel Visible="True" />
                    <Settings ShowGroupPanel="False" ShowStatusBar="Hidden" ShowHorizontalScrollBar="False" ShowFilterRow="true" ShowFilterRowMenu="true" />
                    <SettingsLoadingPanel Text="Please Wait..." />
                </dxe:ASPxGridView>




            </dxe:PopupControlContentControl>
        </ContentCollection>
    </dxe:ASPxPopupControl>








    <%--<dxe:ASPxTimer runat="server" Interval="10000" ClientInstanceName="Timer1">
        <ClientSideEvents Tick="timerTick" />
    </dxe:ASPxTimer>--%>
    <asp:HiddenField ID="waitingInvoiceCount" runat="server" />
    <dxe:ASPxLoadingPanel ID="LoadingPanel" runat="server" ClientInstanceName="LoadingPanel"
        Modal="True">
    </dxe:ASPxLoadingPanel>
    <dxe:ASPxPopupControl ID="apcReciptPopup" runat="server"
        CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="capcReciptPopup" Height="630px"
        Width="1200px" HeaderText="Customer Receipt/Payment" Modal="true" AllowResize="true" ResizingMode="Postponed">
        <HeaderTemplate>
            <span>Money Receipt</span>
        </HeaderTemplate>
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
            </dxe:PopupControlContentControl>
        </ContentCollection>
    </dxe:ASPxPopupControl>

    <asp:HiddenField ID="hdIsStockLedger" runat="server" />
    <asp:HiddenField ID="LoadGridData" runat="server" />

    <dxe:ASPxGlobalEvents ID="GlobalEvents" runat="server">
        <ClientSideEvents ControlsInitialized="AllControlInitilize" />
    </dxe:ASPxGlobalEvents>



     <div class="PopUpArea">
        <dxe:ASPxPopupControl ID="ASPxPopupControl1" runat="server" ClientInstanceName="cCustDocumentsPopup"
            Width="350px" HeaderText="Select Design(s)" PopupHorizontalAlign="WindowCenter"
            PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
            Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True">
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">                    
                    <dxe:ASPxCallbackPanel runat="server" ID="CustomerRecpayPanel" ClientInstanceName="CustomerRecpayPanel" OnCallback="CustRacPayPanel_Callback" ClientSideEvents-EndCallback="CustRacPayPanelEndCall">
                        <PanelCollection>
                            <dxe:PanelContent runat="server">
                                <dxe:ASPxComboBox ID="CustCmbDesignName" ClientInstanceName="cCustCmbDesignName" runat="server" ValueType="System.String" Width="100%" EnableSynchronization="True">
                                </dxe:ASPxComboBox>
                                <div class="text-center pTop10">
                                    <dxe:ASPxButton ID="ASPxButton1" ClientInstanceName="cbtnOK" runat="server" AutoPostBack="False" Text="OK" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                                        <ClientSideEvents Click="function(s, e) {return PerformCallToRacpayGridBind();}" />
                                    </dxe:ASPxButton>
                                </div>
                            </dxe:PanelContent>
                        </PanelCollection>
                    </dxe:ASPxCallbackPanel>
                </dxe:PopupControlContentControl>
            </ContentCollection>
        </dxe:ASPxPopupControl>
    </div>


</asp:Content>
