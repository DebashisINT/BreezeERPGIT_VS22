<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="OpeningPurchaseOrderList.aspx.cs" Inherits="OpeningEntry.OpeningEntry.OpeningPurchaseOrderList" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="OpeningCSS/OpeningSearchPopup.css" rel="stylesheet" />
    <%--Code Added By Sandip For Approval Detail Section Start--%>
    <script>
        var isFirstTime = true;

        // Purchase Invoice Section Start
        updatePOGridByDate = function () {
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

                localStorage.setItem("POFromDate", cFormDate.GetDate().format('yyyy-MM-dd'));
                localStorage.setItem("POToDate", ctoDate.GetDate().format('yyyy-MM-dd'));
                localStorage.setItem("POBranch", ccmbBranchfilter.GetValue());

                //cdownpaygrid.PerformCallback('FilterGridByDate~' + cFormDate.GetDate().format('yyyy-MM-dd') + '~' + ctoDate.GetDate().format('yyyy-MM-dd') + '~' + ccmbBranchfilter.GetValue());

                $("#hfFromDate").val(cFormDate.GetDate().format('yyyy-MM-dd'));
                $("#hfToDate").val(ctoDate.GetDate().format('yyyy-MM-dd'));
                $("#hfBranchID").val(ccmbBranchfilter.GetValue());
                $("#hfIsFilter").val("Y");

                CgvPurchaseOrder.Refresh();

                $("#drdExport").val(0);
            }
        }

        AllControlInitilize = function (s, e) {
            if (isFirstTime) {
                //if (localStorage.getItem('POFromDate')) {
                //    var fromdatearray = localStorage.getItem('POFromDate').split('-');
                //    var fromdate = new Date(fromdatearray[0], parseFloat(fromdatearray[1]) - 1, fromdatearray[2], 0, 0, 0, 0);
                //    cFormDate.SetDate(fromdate);
                //}

                //if (localStorage.getItem('POToDate')) {
                //    var todatearray = localStorage.getItem('POToDate').split('-');
                //    var todate = new Date(todatearray[0], parseFloat(todatearray[1]) - 1, todatearray[2], 0, 0, 0, 0);
                //    ctoDate.SetDate(todate);
                //}

                //if (localStorage.getItem('POBranch')) {
                //    if (ccmbBranchfilter.FindItemByValue(localStorage.getItem('POBranch'))) {
                //        ccmbBranchfilter.SetValue(localStorage.getItem('POBranch'));
                //    }
                //}
            }
        }


        //function onPrintJv(id) {
        //    window.location.href = "../../reports/XtraReports/Viewer/OrderReportViewer.aspx?id=" + id;
        //}

        //This function is called to show the Status of All Sales Order Created By Login User Start
        function OpenPopUPUserWiseQuotaion() {
            cgridUserWiseQuotation.PerformCallback();
            cPopupUserWiseQuotation.Show();
        }
        // function above  End

        //This function is called to show all Pending Approval of Sales Order whose Userid has been set LevelWise using Approval Configuration Module 
        function OpenPopUPApprovalStatus() {
            cgridPendingApproval.PerformCallback();
            cpopupApproval.Show();
        }
        // function above  End


        // Status 2 is passed If Approved Check box is checked by User Both Below function is called and used to show in POPUP,  the Add Page of Respective Segment(like Page for Adding Quotation ,Sale Order ,Challan)
        function GetApprovedQuoteId(s, e, itemIndex) {
            var rowvalue = cgridPendingApproval.GetRowValues(itemIndex, 'ID', OnGetApprovedRowValues);
            //cgridPendingApproval.PerformCallback('Status~' + rowvalue);
            //cgridPendingApproval.GetRowValues(itemIndex, 'ID', OnGetApprovedRowValues);

        }
        function OnGetApprovedRowValues(obj) {
            uri = "PurchaseOrder.aspx?key=" + obj + "&status=2" + '&type=PO';
            popup.SetContentUrl(uri);
            popup.Show();
        }
        // function above  End For Approved

        // Status 3 is passed If Approved Check box is checked by User Both Below function is called and used to show in POPUP,  the Add Page of Respective Segment(like Page for Adding Quotation ,Sale Order ,Challan)
        function GetRejectedQuoteId(s, e, itemIndex) {
            debugger;
            cgridPendingApproval.GetRowValues(itemIndex, 'ID', OnGetRejectedRowValues);

        }
        function OnGetRejectedRowValues(obj) {
            uri = "PurchaseOrder.aspx?key=" + obj + "&status=3" + '&type=PO';
            popup.SetContentUrl(uri);
            popup.Show();
        }
        // function above  End For Rejected

        // To Reflect the Data in Pending Waiting Grid and Pending Waiting Counting if the user approve or Rejecte the Order and Saved 

        function OnApprovalEndCall(s, e) {
            $.ajax({
                type: "POST",
                url: "PurchaseOrderList.aspx/GetPendingCase",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    $('#<%= lblWaiting.ClientID %>').text(data.d);
                }
            });
            }

            // function above  End 

    </script>
    <script>
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
                localStorage.setItem("PurchaseOrderFromDate", cFormDate.GetDate().format('yyyy-MM-dd'));
                localStorage.setItem("PurchaseOrderToDate", ctoDate.GetDate().format('yyyy-MM-dd'));
                localStorage.setItem("PurchaseOrderBranch", ccmbBranchfilter.GetValue());

                CgvPurchaseOrder.PerformCallback('FilterGridByDate~' + cFormDate.GetDate().format('yyyy-MM-dd') + '~' + ctoDate.GetDate().format('yyyy-MM-dd') + '~' + ccmbBranchfilter.GetValue())
            }
        }
    </script>


    <%-- Code Added By Sandip For Approval Detail Section End--%>
    <script type="text/javascript">
        var POrderId = 0;
        function onPrintJv(id, visibleIndex) {
            //window.location.href = "../../reports/XtraReports/Viewer/PurchaseOrderReportViewer.aspx?id=" + id;
            debugger;
            var PurOrderNumber = CgvPurchaseOrder.GetDataRow(visibleIndex).children[0].innerText;

            POrderId = id;
            cDocumentsPopup.Show();
            cCmbDesignName.SetSelectedIndex(0);
            cSelectPanel.PerformCallback('Bindalldesignes');
            $('#btnOK').focus();
        }

        function PerformCallToGridBind() {
            cSelectPanel.PerformCallback('Bindsingledesign');
            cDocumentsPopup.Hide();
            return false;
        }

        function cSelectPanelEndCall(s, e) {
            debugger;
            if (cSelectPanel.cpSuccess != null) {
                var TotDocument = cSelectPanel.cpSuccess.split(',');
                var reportName = cCmbDesignName.GetValue();
                var module = 'Porder';
                window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + reportName + '&modulename=' + module + '&id=' + POrderId, '_blank')
            }
            cSelectPanel.cpSuccess = null
            if (cSelectPanel.cpSuccess == null) {
                cCmbDesignName.SetSelectedIndex(0);
            }
        }
        document.onkeydown = function (e) {
            if ((event.keyCode == 120 || event.keyCode == 66) && event.altKey == true) { //run code for Ctrl+B -- ie, Add Both Item
                StopDefaultAction(e);
                AddBothItem();
            }

            if ((event.keyCode == 120 || event.keyCode == 73) && event.altKey == true) { //run code for Ctrl+I -- ie, Add Inventory Item
                StopDefaultAction(e);
                AddInventoryItem();
            }

            if ((event.keyCode == 120 || event.keyCode == 78) && event.altKey == true) { //run code for Ctrl+N -- ie, Add Non-Inventory Item
                StopDefaultAction(e);
                AddNonInventoryItem();
            }

            if ((event.keyCode == 120 || event.keyCode == 67) && event.altKey == true) { //run code for Ctrl+C -- ie, Add Capital Item
                StopDefaultAction(e);
                AddCapitalItem();
            }
        }
        function ShowMsgLastCall() {

            if (CgvPurchaseOrder.cpDelete != null) {

                jAlert(CgvPurchaseOrder.cpDelete)
                //CgvPurchaseOrder.PerformCallback();
                CgvPurchaseOrder.Refresh();
                CgvPurchaseOrder.cpDelete = null
            }

            if (CgvPurchaseOrder.cpMessage != null) {
                if (CgvPurchaseOrder.cpMessage == "Generated") {
                    CgvPurchaseOrder.cpMessage = null;
                    jAlert('Barcode generation successfully.');
                }
                else if (CgvPurchaseOrder.cpMessage == "NullStock") {
                    CgvPurchaseOrder.cpMessage = null;
                    jAlert('No stock available.');
                }
                else if (CgvPurchaseOrder.cpMessage == "NullBarcode") {
                    CgvPurchaseOrder.cpMessage = null;
                    jAlert('All Barcode generated.');
                }
                else if (CgvPurchaseOrder.cpMessage == "Error") {
                    CgvPurchaseOrder.cpMessage = null;
                    jAlert('Please try again later');
                }
            }
        }
        function StopDefaultAction(e) {
            if (e.preventDefault) { e.preventDefault() }
            else { e.stop() };

            e.returnValue = false;
            e.stopPropagation();
        }

        function AddBothItem() {
            var keyOpening = document.getElementById('hdfOpening').value;
            if (keyOpening != '') {
                var url = 'PurchaseOrder.aspx?key=' + 'ADD&&op=' + 'yes&&InvType=B';
            }
            else {
                var url = 'PurchaseOrder.aspx?key=' + 'ADD&&InvType=B';
            }

            window.location.href = url;
        }

        function AddInventoryItem() {
            var keyOpening = document.getElementById('hdfOpening').value;
            if (keyOpening != '') {
                var url = 'PurchaseOrder.aspx?key=' + 'ADD&&op=' + 'yes&&InvType=Y';
            }
            else {
                var url = 'PurchaseOrder.aspx?key=' + 'ADD&&InvType=Y';
            }

            window.location.href = url;
        }
        function AddNonInventoryItem() {
            var keyOpening = document.getElementById('hdfOpening').value;
            if (keyOpening != '') {
                var url = 'PurchaseOrder.aspx?key=' + 'ADD&&op=' + 'yes&&InvType=N';
            }
            else {
                var url = 'PurchaseOrder.aspx?key=' + 'ADD&&InvType=N';
            }

            window.location.href = url;
        }
        function AddCapitalItem() {
            var keyOpening = document.getElementById('hdfOpening').value;
            if (keyOpening != '') {
                var url = 'PurchaseOrder.aspx?key=' + 'ADD&&op=' + 'yes&&InvType=C';
            }
            else {
                var url = 'PurchaseOrder.aspx?key=' + 'ADD&&InvType=C';
            }

            window.location.href = url;
        }

        function OnMoreInfoClick(keyValue, visibleIndex) {
            var IsBarcode = $("#hfIsBarcode").val();
            CgvPurchaseOrder.SetFocusedRowIndex(visibleIndex);
            var IsCancelFlag = CgvPurchaseOrder.GetRow(CgvPurchaseOrder.GetFocusedRowIndex()).children[12].innerText;
            if (IsCancelFlag.trim() != 'Cancelled' && IsCancelFlag.trim() != 'Closed') {
                if (IsBarcode == "true") {
                    //jAlert("Cannot edit as barcode has already been generated", "Alert", function () { });
                    jAlert("Cannot edit as barcode has already activated in the system.", "Alert", function () { });
                }
                else {
                    var url = 'PurchaseOrder.aspx?key=' + keyValue + '&type=PO';
                    window.location.href = url;
                }
            }
            else {
                jAlert("Purchase Order is " + IsCancelFlag.trim() + ". Edit is not allowed.");
            }


        }

        function OnCancelClick(keyValue, visibleIndex) {
            debugger;
            $("#<%=hddnKeyValue.ClientID%>").val(keyValue);
            //var IsCancel = cGrdOrder.GetRow(cGrdOrder.GetFocusedRowIndex()).children[6].innerText;

            jConfirm('Do you want to cancel the Order ?', 'Confirm Dialog', function (r) {
                if (r == true) {
                    $('#MandatoryRemarksFeedback').attr('style', 'display:none;position: absolute; right: -20px; top: 8px;');
                    cPopup_Feedback.Show();
                }
                else {
                    return false;
                }
            });

        }

        function OnClosedClick(keyValue, visibleIndex) {
            debugger;
            $("#<%=hddnKeyValue.ClientID%>").val(keyValue);
            CgvPurchaseOrder.SetFocusedRowIndex(visibleIndex);


            jConfirm('Do you want to close the Order ?', 'Confirm Dialog', function (r) {
                if (r == true) {
                    $('#MandatoryRemarksFeedback1').attr('style', 'display:none;position: absolute; right: -20px; top: 8px;');
                    cPopup_Closed.Show();
                }
                else {
                    return false;
                }
            });

        }


        function CancelSalesOrder(keyValue, Reason) {
            debugger;
            $.ajax({
                type: "POST",
                url: "PurchaseOrderList.aspx/CancelPurchaseOrderOnRequest",
                data: JSON.stringify({ keyValue: keyValue, Reason: Reason }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,//Added By:Subhabrata
                success: function (msg) {
                    debugger;
                    var status = msg.d;
                    if (status == "1") {
                        jAlert("Purchase Order is cancelled successfully.");
                        //cGrdOrder.PerformCallback('BindGrid');

                    }
                    else if (status == "-1") {
                        jAlert("Purchase Order is not cancelled. Try again later");
                    }
                    else if (status == "-2") {
                        jAlert("Selected order is tagged in other module. Cannot proceed.");
                    }
                    else if (status == "-3") {
                        jAlert("Purchase Order is  already cancelled.");
                    }
                    else if (status == "-4") {
                        jAlert("Order is already closed. Cannot proceed.");
                    }
                    else if (status == "-5") {
                        jAlert("No balance quantity available for this order. Cannot proceed.");
                    }
                }
            });
        }


        function ClosedSalesOrder(keyValue, Reason) {
            debugger;
            $.ajax({
                type: "POST",
                url: "PurchaseOrderList.aspx/ClosedPurchaseOrderOnRequest",
                data: JSON.stringify({ keyValue: keyValue, Reason: Reason }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,//Added By:Subhabrata
                success: function (msg) {
                    debugger;
                    var status = msg.d;
                    if (status == "1") {
                        jAlert("Order is closed successfully.");
                        //cGrdOrder.PerformCallback('BindGrid');

                    }
                    else if (status == "-1") {
                        jAlert("Order is not closed. Try again later");
                    }
                    else if (status == "-2") {
                        jAlert("Selected order is tagged in other module. Cannot proceed.");
                    }
                    else if (status == "-3") {
                        jAlert("Order is  already closed.");
                    }
                    else if (status == "-4") {
                        jAlert("Order is already cancelled. Cannot proceed.");
                    }
                    else if (status == "-5") {
                        jAlert("No balance quantity available for this order. Cannot proceed.");
                    }
                }
            });
        }

        function CallFeedback_save() {
            debugger;
            var KeyVal = $("#<%=hddnKeyValue.ClientID%>").val();
            var flag = true;
             <%--$("#<%=hddnIsSavedFeedback.ClientID%>").val("1");--%>
            var Remarks = txtFeedback.GetValue();
            if (Remarks == "" || Remarks == null) {
                $('#MandatoryRemarksFeedback').attr('style', 'display:block;position: absolute; right: -20px; top: 8px;');
                flag = false;
            }
            else {
                $('#MandatoryRemarksFeedback').attr('style', 'display:none;position: absolute; right: -20px; top: 8px;');
                cPopup_Feedback.Hide();
                CancelSalesOrder(KeyVal, Remarks);
                CgvPurchaseOrder.Refresh();
                //cGrdOrder.PerformCallback('Feedback~' + KeyVal + '~' + Remarks);
                //Grid.Refresh();


            }
            return flag;

        }


        function CancelFeedback_save() {

            <%-- $("#<%=hddnIsSavedFeedback.ClientID%>").val("0");--%>
            txtFeedback.SetValue();
            cPopup_Feedback.Hide();
            //$('#chkmailfeedback').prop('checked', false);
        }


        function CallClosed_save() {
            debugger;
            var KeyVal = $("#<%=hddnKeyValue.ClientID%>").val();
            var flag = true;
             <%--$("#<%=hddnIsSavedFeedback.ClientID%>").val("1");--%>
            var Remarks = txtClosed.GetValue();
            if (Remarks == "" || Remarks == null) {
                $('#MandatoryRemarksFeedback1').attr('style', 'display:block;position: absolute; right: -20px; top: 8px;');
                flag = false;
            }
            else {
                $('#MandatoryRemarksFeedback1').attr('style', 'display:none;position: absolute; right: -20px; top: 8px;');
                cPopup_Closed.Hide();
                ClosedSalesOrder(KeyVal, Remarks);
                CgvPurchaseOrder.Refresh();
                //cGrdOrder.PerformCallback('Feedback~' + KeyVal + '~' + Remarks);
                //Grid.Refresh();


            }
            return flag;

        }

        function CancelClosed_save() {

            <%-- $("#<%=hddnIsSavedFeedback.ClientID%>").val("0");--%>
            txtClosed.SetValue();
            cPopup_Closed.Hide();
            //$('#chkmailfeedback').prop('checked', false);
        }





        ////##### coded by Samrat Roy - 17/04/2017 - ref IssueLog(P Order - 70) 
        ////Add an another param to define request type 
        function OnViewClick(keyValue) {
            var url = '/OMS/Management/Activities/PurchaseOrder.aspx?key=' + keyValue + '&req=V' + '&type=PO';
            window.location.href = url;
        }
        function OnClickDelete(keyValue, visibleIndex) {
            CgvPurchaseOrder.SetFocusedRowIndex(visibleIndex);
            var IsCancelFlag = CgvPurchaseOrder.GetRow(CgvPurchaseOrder.GetFocusedRowIndex()).children[12].innerText;
            if (IsCancelFlag.trim() != 'Cancelled' && IsCancelFlag.trim() != 'Closed') {
                jConfirm('Confirm delete?', 'Confirmation Dialog', function (r) {
                    if (r == true) {
                        CgvPurchaseOrder.PerformCallback('Delete~' + keyValue);
                    }
                });
            }
            else {
                jAlert("Purchase Order is " + IsCancelFlag.trim() + ".Delete is not allowed.");
            }
        }
    </script>
    <script>
        function GenerateBarcode() {
            var visibleIndex = CgvPurchaseOrder.GetFocusedRowIndex();
            var key = CgvPurchaseOrder.GetRowKey(visibleIndex);
            GetObjectID('hdfDocNumber').value = key;

            if (visibleIndex != -1) {
                cPopupWarehouse.Show();
                cDocumentPanel.PerformCallback('GetDocumentDetails~' + key);
            }
            else {
                jAlert('No data available.');
            }
        }
        function PrintBarcode() {
            //LoadingPanel.Show();
            //LoadingPanel.SetText("Please wait...");

            var visibleIndex = CgvPurchaseOrder.GetFocusedRowIndex();
            var key = CgvPurchaseOrder.GetRowKey(visibleIndex);
            var Branch = ccmbBranchfilter.GetValue();
            var doctype = 'PO';
            var module = 'BRCODE';
            var reportName = 'Barcode~D';
            window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + reportName + "&modulename=BRCODE&id=" + key + "&doctype=" + doctype + "&Branch=" + Branch, '_blank')

        }
        function GenerateProductBarcode() {
            var Qty = ctxtQty.GetValue();

            cDocumentPanel.PerformCallback('GenerateBarcode~' + Qty);
        }
        function OnRowClick(s, e) {
            var visibleIndex = e.visibleIndex;
            var key = cDocumentGrid.GetRowKey(visibleIndex);
            GetObjectID('hdfDocDetailsNumber').value = key;

            if (visibleIndex != -1) {
                cDocumentPanel.PerformCallback('GetDocumentStock');
            }
            else {
                jAlert('No data available.');
            }
        }
        function OnDetailsRowClick(s, e) {
            var visibleIndex = e.visibleIndex;
            var key = cStockGrid.GetRowKey(visibleIndex);
            GetObjectID('hdfMapID').value = key;
        }
        function SaveSerial() {
            var MapID = GetObjectID('hdfMapID').value;
            var Serial = cSerialNo.GetValue();

            cDocumentPanel.PerformCallback('SaveSerial');
        }
        function DocumentPanelEndCall(s, e) {
            if (cDocumentPanel.cpDocDetails) {
                var DocDetails = cDocumentPanel.cpDocDetails;
                cDocumentPanel.cpDocDetails = null;

                var DocNumber = DocDetails.split('~')[0];
                var Vendor = DocDetails.split('~')[1];
                var Branch = DocDetails.split('~')[2];
                var BranchID = DocDetails.split('~')[3];

                ctxtDocNumber.SetValue(DocNumber);
                ctxtVendor.SetValue(Vendor);
                ctxtBatch.SetValue(Branch);
                GetObjectID('hdfBranch').value = BranchID;

                ctxtProduct.SetValue("");
                ctxtQty.SetValue("0");
                ctxtQty.SetMaxValue("0");
                ctxtQty.SetMinValue("0");
            }

            if (cDocumentPanel.cpStockDetails) {
                var DocDetails = cDocumentPanel.cpStockDetails;
                cDocumentPanel.cpStockDetails = null;

                var Products_Name = DocDetails.split('~')[0];
                var Quantity = DocDetails.split('~')[1];

                ctxtProduct.SetValue(Products_Name);
                ctxtQty.SetValue(Quantity);
                ctxtQty.SetMaxValue(Quantity);
                ctxtQty.SetMinValue("0");
            }

        }
    </script>
    <style>
        strong label {
            font-weight: bold !important;
        }

        input[type="radio"] {
            webkit-transform: translateY(3px);
            -moz-transform: translateY(3px);
            transform: translateY(3px);
        }

        .blink {
            animation: blink-animation 1s steps(5, start) infinite;
            -webkit-animation: blink-animation 1s steps(5, start) infinite;
            cursor: pointer;
            color: #128AC9;
        }

        @keyframes blink-animation {
            to {
                visibility: hidden;
            }
        }

        @-webkit-keyframes blink-animation {
            to {
                visibility: hidden;
            }
        }

        .padTab > tbody > tr > td {
            padding-right: 15px;
        }
    </style>
    <style>
        .dynamicPopupTbl > thead > tr > th, .dynamicPopupTbl > tbody > tr > td {
            padding: 5px 8px !important;
        }

        .dynamicPopupTbl > tbody > tr > td {
            cursor: pointer;
        }

        .dynamicPopupTbl.back > thead > tr > th {
            background: #4e64a6;
            color: #fff;
        }

        .dynamicPopupTbl.back > tbody > tr > td {
            background: #fff;
        }

        .dynamicPopupTbl > tbody > tr > td input {
            border: none !important;
            cursor: pointer;
            background: transparent !important;
        }

        .focusrow {
            background-color: #0000ff3d;
        }

        .HeaderStyle {
            background-color: #180771d9;
            color: #f5f5f5;
        }

        .gridStatic {
            margin-top: 25px;
        }

            .gridStatic .dynamicPopupTbl {
                width: 100%;
            }

        .bodBot {
            border-bottom: 1px solid #ccc;
            padding-bottom: 10px;
        }

        table.scroll {
            /* width: 100%; */ /* Optional */
            /* border-collapse: collapse; */
            border-spacing: 0;
            width: 100%;
        }

            table.scroll tbody,
            table.scroll thead {
                display: block;
            }

        thead tr th {
            height: 30px;
            line-height: 30px;
            /* text-align: left; */
        }

        table.scroll tbody {
            height: 250px;
            overflow-y: auto;
            overflow-x: hidden;
        }

        .tblPad > tbody > tr > td {
            padding-bottom: 8px;
        }

        .pdTop {
            padding-top: 10px;
        }

        .pdLeft {
            padding-left: 15px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading clearfix">
        <div class="panel-title pull-left" id="td_contact1" runat="server">
            <h3>
                <asp:Label ID="lblHeadTitle" runat="server" Text="Opening Purchase Order"></asp:Label>
            </h3>
        </div>
        <table class="padTab pull-right">
            <tr>
                <td>
                    <label>From Date</label></td>
                <td>&nbsp;</td>
                <td>
                    <dxe:ASPxDateEdit ID="FormDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cFormDate" Width="100%">
                        <ButtonStyle Width="13px">
                        </ButtonStyle>
                    </dxe:ASPxDateEdit>
                </td>
                <td>&nbsp;</td>
                <td>
                    <label>To Date</label>
                </td>
                <td>&nbsp;</td>
                <td>
                    <dxe:ASPxDateEdit ID="toDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="ctoDate" Width="100%">
                        <ButtonStyle Width="13px">
                        </ButtonStyle>
                    </dxe:ASPxDateEdit>
                </td>
                <td>&nbsp;</td>
                <td>Unit</td>
                <td>
                    <dxe:ASPxComboBox ID="cmbBranchfilter" runat="server" ClientInstanceName="ccmbBranchfilter" Width="100%">
                    </dxe:ASPxComboBox>
                </td>
                <td>&nbsp;</td>
                <td>
                    <input type="button" value="Show" class="btn btn-primary" onclick="updatePOGridByDate()" />
                </td>

            </tr>

        </table>
    </div>
    <div class="form_main clearfix" id="btnAddNew">
        <div style="float: left; padding-right: 5px;">
            <% if (rights.CanAdd)
               { %>
           <%-- <a href="javascript:void(0);" onclick="AddInventoryItem()" class="btn btn-primary"><span>Add <u>I</u>nventory Items</span> </a>
            <a href="javascript:void(0);" onclick="AddNonInventoryItem()" class="btn btn-primary"><span>Add <u>N</u>on Inventory Items</span> </a>
            <a href="javascript:void(0);" onclick="AddCapitalItem()" class="btn btn-primary"><span>Add <u>C</u>apital Goods</span> </a>
            <a href="javascript:void(0);" onclick="AddBothItem()" class="btn btn-primary"><span>Add <u>B</u>oth Items</span> </a>--%>
            <% } %>
            <% if (rights.CanExport)
               { %>
           <%-- <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-sm btn-primary" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true">
                <asp:ListItem Value="0">Export to</asp:ListItem>
                <asp:ListItem Value="1">PDF</asp:ListItem>
                <asp:ListItem Value="2">XLS</asp:ListItem>
                <asp:ListItem Value="3">RTF</asp:ListItem>
                <asp:ListItem Value="4">CSV</asp:ListItem>
            </asp:DropDownList>--%>
            <% } %>
            <%--Sandip Section for Approval Section in Design Start --%>
            <span id="spanStatus" runat="server">
                <a href="javascript:void(0);" onclick="OpenPopUPUserWiseQuotaion()" class="btn btn-primary">
                    <span>My Purchase Order Status</span>
                    <%--<asp:Label ID="Label1" runat="server" Text=""></asp:Label>--%>                   
                </a>
            </span>
            <span id="divPendingWaiting" runat="server">
                <a href="javascript:void(0);" onclick="OpenPopUPApprovalStatus()" class="btn btn-primary">
                    <span>Approval Waiting</span>

                    <asp:Label ID="lblWaiting" runat="server" Text=""></asp:Label>
                </a>
                <i class="fa fa-reply blink" style="font-size: 20px; margin-right: 10px;" aria-hidden="true"></i>

            </span>
            <%--Sandip Section for Approval Section in Design End --%>
            <dxe:ASPxButton ID="btnGenerate" ClientInstanceName="cbtnGenerate" runat="server" AutoPostBack="false" Text="Generate Barcode" CssClass="btn btn-success" UseSubmitBehavior="false">
                <ClientSideEvents Click="function(s, e) {GenerateBarcode();}" />
            </dxe:ASPxButton>
            <dxe:ASPxButton ID="btnPrint" ClientInstanceName="cbtnPrint" runat="server" AutoPostBack="false" Text="Print Barcode" CssClass="btn btn-warning" UseSubmitBehavior="false">
                <ClientSideEvents Click="function(s, e) {PrintBarcode();}" />
            </dxe:ASPxButton>
        </div>
    </div>
    <div class="GridViewArea">
        <%--Settings-HorizontalScrollBarMode="Auto" --%>
    </div>
    <dxe:ASPxGridView ID="Grid_PurchaseOrder" runat="server" AutoGenerateColumns="False" KeyFieldName="PurchaseOrder_Id" SettingsBehavior-AllowFocusedRow="true"
        ClientInstanceName="CgvPurchaseOrder" Width="100%" OnSummaryDisplayText="Grid_PurchaseOrder_SummaryDisplayText" OnCustomCallback="Grid_PurchaseOrder_CustomCallback"
        OnDataBinding="Grid_PurchaseOrder_DataBinding" OnHtmlRowPrepared="AvailableStockgrid_HtmlRowPrepared" DataSourceID="EntityServerModeDataSource" SettingsDataSecurity-AllowEdit="false" SettingsDataSecurity-AllowInsert="false" SettingsDataSecurity-AllowDelete="false" SettingsBehavior-ColumnResizeMode="Control">
        <SettingsSearchPanel Visible="True" />
        <%-- SettingsCookies-Enabled="true" SettingsCookies-StorePaging="true" SettingsCookies-StoreFiltering="true" SettingsCookies-StoreGroupingAndSorting="true" --%>

        <ClientSideEvents />
        <Columns>
            <dxe:GridViewDataCheckColumn VisibleIndex="0" Visible="false">
                <EditFormSettings Visible="True" />
                <EditItemTemplate>
                    <dxe:ASPxCheckBox ID="ASPxCheckBox1" Text="" runat="server"></dxe:ASPxCheckBox>
                </EditItemTemplate>
                <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                <Settings AllowAutoFilterTextInputTimer="False" />
            </dxe:GridViewDataCheckColumn>
            <dxe:GridViewDataTextColumn FieldName="PurchaseOrder_Id" Visible="false" SortOrder="Descending">
                <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                <Settings AllowAutoFilterTextInputTimer="False" />
                <Settings AutoFilterCondition="Contains" />
            </dxe:GridViewDataTextColumn>
            <dxe:GridViewDataTextColumn FieldName="IsCancel" Visible="false" SortOrder="Descending">
                <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                <Settings AllowAutoFilterTextInputTimer="False" />
                <Settings AutoFilterCondition="Contains" />
            </dxe:GridViewDataTextColumn>
            <dxe:GridViewDataTextColumn FieldName="IsClosed" Visible="false" SortOrder="Descending">
                <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                <Settings AllowAutoFilterTextInputTimer="False" />
                <Settings AutoFilterCondition="Contains" />
            </dxe:GridViewDataTextColumn>
            <dxe:GridViewDataTextColumn VisibleIndex="1" FixedStyle="Left" Caption="Document No." FieldName="PurchaseOrder_Number">
                <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                <Settings AllowAutoFilterTextInputTimer="False" />
                <Settings AutoFilterCondition="Contains" />
            </dxe:GridViewDataTextColumn>
            <%--Width="120px"--%>
            <dxe:GridViewDataTextColumn VisibleIndex="2" Caption="Posting Date" FieldName="PurchaseOrderDt" PropertiesTextEdit-DisplayFormatString="dd-MM-yyyy">
                <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                <Settings AllowAutoFilterTextInputTimer="False" />
                <Settings AutoFilterCondition="Contains" />
            </dxe:GridViewDataTextColumn>
            <%-- Width="100px"--%>
            <dxe:GridViewDataTextColumn VisibleIndex="3" Caption="Vendor" FieldName="Customer">
                <CellStyle Wrap="true" CssClass="gridcellleft"></CellStyle>
                <Settings AllowAutoFilterTextInputTimer="False" />
                <Settings AutoFilterCondition="Contains" />
            </dxe:GridViewDataTextColumn>
            <%--Width="150px"--%>
            <dxe:GridViewDataTextColumn VisibleIndex="4" Caption="Amount" FieldName="ValueInBaseCurrency" HeaderStyle-HorizontalAlign="Right">
                <CellStyle Wrap="False" CssClass="gridcellleft" HorizontalAlign="Right"></CellStyle>
                <Settings AllowAutoFilterTextInputTimer="False" />
                <Settings AutoFilterCondition="Contains" />
                <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
            </dxe:GridViewDataTextColumn>
            <%--  Width="86px"--%>
            <dxe:GridViewDataTextColumn VisibleIndex="5" Caption="Indent Number" FieldName="Indent_Numbers">
                <CellStyle Wrap="True" CssClass="gridcellleft" HorizontalAlign="left"></CellStyle>
                <Settings AllowAutoFilterTextInputTimer="False" />
                <Settings AutoFilterCondition="Contains" />
            </dxe:GridViewDataTextColumn>
            <dxe:GridViewDataTextColumn VisibleIndex="6" Caption="Indent Date" FieldName="Indent_RequisitionDate" PropertiesTextEdit-DisplayFormatString="dd-MM-yyyy">
                <CellStyle Wrap="True" CssClass="gridcellleft" HorizontalAlign="left"></CellStyle>
                <Settings AllowAutoFilterTextInputTimer="False" />
                <Settings AutoFilterCondition="Contains" />
            </dxe:GridViewDataTextColumn>
            <%-- Width="100px"--%>
            <%-- Width="130px"--%>
            <%--<dxe:GridViewDataTextColumn VisibleIndex="7" Caption="Status" FieldName="PurchaseOrder_Status">
                    <CellStyle  Wrap="True" CssClass="gridcellleft"></CellStyle>
                </dxe:GridViewDataTextColumn>--%>
            <dxe:GridViewDataTextColumn VisibleIndex="8" Caption="Ship to Party" FieldName="ShiftPartyName">
                <CellStyle Wrap="True" CssClass="gridcellleft"></CellStyle>
                <Settings AllowAutoFilterTextInputTimer="False" />
                <Settings AutoFilterCondition="Contains" />
            </dxe:GridViewDataTextColumn>
            <%--Width="99px"--%>
            <dxe:GridViewDataTextColumn VisibleIndex="9" Caption="Invoice Number" FieldName="Invoice_Number">
                <CellStyle Wrap="True" CssClass="gridcellleft"></CellStyle>
                <Settings AllowAutoFilterTextInputTimer="False" />
                <Settings AutoFilterCondition="Contains" />
            </dxe:GridViewDataTextColumn>
            <%--Width="120px"--%>
            <dxe:GridViewDataTextColumn VisibleIndex="10" Caption="Invoice Date" FieldName="Invoice_Date" PropertiesTextEdit-DisplayFormatString="dd-MM-yyyy">
                <CellStyle Wrap="True" CssClass="gridcellleft"></CellStyle>
                <Settings AllowAutoFilterTextInputTimer="False" />
                <Settings AutoFilterCondition="Contains" />
            </dxe:GridViewDataTextColumn>

            <dxe:GridViewDataTextColumn VisibleIndex="11" Caption="Place of Supply[GST]" FieldName="PosState" >
                <CellStyle Wrap="True" CssClass="gridcellleft"></CellStyle>
                <Settings AllowAutoFilterTextInputTimer="False" />
                <Settings AutoFilterCondition="Contains" />
            </dxe:GridViewDataTextColumn>

            <dxe:GridViewDataTextColumn VisibleIndex="12" FieldName="EnteredBy"
                Caption="Entered By">
                <CellStyle HorizontalAlign="left" CssClass="gridcellleft">
                </CellStyle>
                <Settings AllowAutoFilterTextInputTimer="False" />
            </dxe:GridViewDataTextColumn>
            <dxe:GridViewDataTextColumn VisibleIndex="13" FieldName="LastUpdateOn" Settings-AllowAutoFilter="False"
                Caption="Last Update On">
                <CellStyle HorizontalAlign="Right" CssClass="gridcellleft">
                </CellStyle>
                <Settings AllowAutoFilterTextInputTimer="False" />
            </dxe:GridViewDataTextColumn>
            <dxe:GridViewDataTextColumn VisibleIndex="14" FieldName="UpdatedBy" Settings-AllowAutoFilter="False"
                Caption="Updated By">
                <CellStyle HorizontalAlign="left" CssClass="gridcellleft">
                </CellStyle>
                <Settings AllowAutoFilterTextInputTimer="False" />
            </dxe:GridViewDataTextColumn>
            <dxe:GridViewDataTextColumn VisibleIndex="15" FieldName="Doc_status" Settings-AllowAutoFilter="False"
                Caption="Status">
                <CellStyle HorizontalAlign="left" CssClass="gridcellleft">
                </CellStyle>
                <Settings AllowAutoFilterTextInputTimer="False" />
            </dxe:GridViewDataTextColumn>

           

            <dxe:GridViewDataTextColumn HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="center" VisibleIndex="17" Width="170px">
                <DataItemTemplate>
                    <% if (rights.CanView)
                       { %>
                    <a href="javascript:void(0);" onclick="OnViewClick('<%# Container.KeyValue %>')" class="pad" title="View">
                        <img src="../../../assests/images/viewIcon.png" /></a>
                    <% } %>
                    <% if (rights.CanEdit)
                       { %>
                   <%-- <a href="javascript:void(0);" onclick="OnMoreInfoClick('<%# Container.KeyValue %>','<%# Container.VisibleIndex %>')" class="pad" title="Edit">
                        <img src="../../../assests/images/info.png" /></a>--%>
                    <% } %>
                    <% if (rights.CanDelete)
                       { %>
                    <%--<a href="javascript:void(0);" onclick="OnClickDelete('<%# Container.KeyValue %>','<%# Container.VisibleIndex %>')" class="pad" title="Delete">
                        <img src="../../../assests/images/Delete.png" /></a>--%>
                    <% } %>

                     <% if (rights.CanCancel)
                       { %>
                 <%--   <a href="javascript:void(0);" onclick="OnCancelClick('<%# Container.KeyValue %>')" class="pad" title="Cancel">
                        <img src="../../../assests/images/not-verified.png" /></a>--%>
                    <% } %>

                     <% if (rights.CanClose)
                       { %>
                  <%--  <a href="javascript:void(0);" onclick="OnClosedClick('<%# Container.KeyValue %>')" class="pad" title="Close">
                        <img src="../../../assests/images/Closed.png" /></a>--%>
                    <% } %>


                    <% if (rights.CanPrint)
                       { %>
                   <%-- <a href="javascript:void(0);" onclick="onPrintJv('<%# Container.KeyValue %>','<%# Container.VisibleIndex %>')" class="pad" title="print">
                        <img src="../../../assests/images/Print.png" />
                    </a>--%>
                    <%} %>

                    <%--<a href="javascript:void(0);" onclick="OnClickCopy('<%# Container.KeyValue %>')" class="pad" title="Copy ">
                            <i class="fa fa-copy"></i></a>
                        <a href="javascript:void(0);" onclick="OnClickStatus('<%# Container.KeyValue %>')" class="pad" title="Status">
                            <img src="../../../assests/images/verified.png" /></a>
                        <a href="javascript:void(0);" onclick="OnclickViewAttachment('<%# Container.KeyValue %>')" class="pad" title="View Attachment">
                            <img src="../../../assests/images/attachment.png" />--%>
                        </a>
                </DataItemTemplate>
                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                <CellStyle HorizontalAlign="Center"></CellStyle>
                <HeaderTemplate><span>Actions</span></HeaderTemplate>
                <EditFormSettings Visible="False"></EditFormSettings>
                <Settings AllowAutoFilterTextInputTimer="False" />
            </dxe:GridViewDataTextColumn>

        </Columns>
        <SettingsContextMenu Enabled="true"></SettingsContextMenu>
        <TotalSummary>
            <dxe:ASPxSummaryItem FieldName="ValueInBaseCurrency" SummaryType="Sum" />
        </TotalSummary>
        <Settings ShowGroupPanel="True" ShowStatusBar="Visible" ShowFilterRow="true" ShowFilterRowMenu="true" />
        <ClientSideEvents EndCallback="function(s, e) {
	                                        ShowMsgLastCall();
                                        }" />
        <SettingsBehavior ConfirmDelete="True" />
        <Styles>
            <Header SortingImageSpacing="5px" ImageSpacing="5px"></Header>
            <FocusedRow HorizontalAlign="Left" VerticalAlign="Top" CssClass="gridselectrow"></FocusedRow>
            <LoadingPanel ImageSpacing="10px"></LoadingPanel>
            <FocusedGroupRow CssClass="gridselectrow"></FocusedGroupRow>
            <Footer CssClass="gridfooter"></Footer>
        </Styles>
        <SettingsPager NumericButtonCount="10" PageSize="10" ShowSeparators="True" Mode="ShowPager">
            <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
        </SettingsPager>
        <Settings ShowFooter="true" ShowGroupFooter="VisibleIfExpanded" ShowGroupPanel="True" ShowStatusBar="Hidden"
            ShowHorizontalScrollBar="False" ShowFilterRow="true" ShowFilterRowMenu="true" HorizontalScrollBarMode="Visible" />


    </dxe:ASPxGridView>
    <dx:LinqServerModeDataSource ID="EntityServerModeDataSource" runat="server" OnSelecting="EntityServerModeDataSource_Selecting"
        ContextTypeName="ERPDataClassesDataContext" TableName="v_PurchaseOrderList" />
    <%--<dxe:ASPxGridViewExporter ID="exporter" runat="server">
        </dxe:ASPxGridViewExporter>--%>
    <dxe:ASPxGridViewExporter ID="exporter" GridViewID="GrdQuotation" runat="server" Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
    </dxe:ASPxGridViewExporter>
    <asp:HiddenField ID="hdfOpening" runat="server" />

    <%--DEBASHIS--%>
    <div class="PopUpArea">

         <dxe:ASPxPopupControl ID="Popup_Feedback" runat="server" ClientInstanceName="cPopup_Feedback"
            Width="400px" HeaderText="Reason For Cancel" PopupHorizontalAlign="WindowCenter"
            BackColor="white" Height="100px" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
            Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True">
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">
                    <%--<div style="Width:400px;background-color:#FFFFFF;margin:0px;border:1px solid red;">--%>
                    <div class="Top clearfix">

                        <table style="width:94%">
                           
                            <tr><td>Reason<span style="color: red">*</span></td>
                                <td class="relative">
                                     <dxe:ASPxMemo ID="txtInstFeedback" runat="server" Width="100%" Height="50px" ClientInstanceName="txtFeedback"></dxe:ASPxMemo>
                                                                        <span id="MandatoryRemarksFeedback" style="display: none">
                                        <img id="gridHistory_DXPEForm_efnew_DXEFL_DXEditor1234_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></span>
                                </td></tr>
                             
                            <tr>
                                <td></td>
                                <td colspan="2" style="padding-top: 10px;">
                                    <input id="btnFeedbackSave" class="btn btn-primary" onclick="CallFeedback_save()" type="button" value="Save" />
                                    <input id="btnFeedbackCancel" class="btn btn-danger" onclick="CancelFeedback_save()" type="button" value="Cancel" />
                                </td>

                            </tr>
                        </table>


                    </div>

                </dxe:PopupControlContentControl>
            </ContentCollection>
            <HeaderStyle BackColor="LightGray" ForeColor="Black" />

          
        </dxe:ASPxPopupControl>


        <dxe:ASPxPopupControl ID="Popup_Closed" runat="server" ClientInstanceName="cPopup_Closed"
            Width="400px" HeaderText="Reason For Close" PopupHorizontalAlign="WindowCenter"
            BackColor="white" Height="100px" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
            Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True">
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">
                    <%--<div style="Width:400px;background-color:#FFFFFF;margin:0px;border:1px solid red;">--%>
                    <div class="Top clearfix">

                        <table style="width:94%">
                           
                            <tr><td>Reason<span style="color: red">*</span></td>
                                <td class="relative">
                                     <dxe:ASPxMemo ID="ASPxMemo1" runat="server" Width="100%" Height="50px" ClientInstanceName="txtClosed"></dxe:ASPxMemo>
                                                                        <span id="MandatoryRemarksFeedback1" style="display: none">
                                        <img id="gridHistory_DXPEForm_efnew_DXEFL_DXEditor1234_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></span>
                                </td></tr>
                             
                            <tr>
                                <td></td>
                                <td colspan="2" style="padding-top: 10px;">
                                    <input id="btnClosedSave" class="btn btn-primary" onclick="CallClosed_save()" type="button" value="Save" />
                                    <input id="btnClosedCancel" class="btn btn-danger" onclick="CancelClosed_save()" type="button" value="Cancel" />
                                </td>

                            </tr>
                        </table>


                    </div>

                </dxe:PopupControlContentControl>
            </ContentCollection>
            <HeaderStyle BackColor="LightGray" ForeColor="Black" />

          
        </dxe:ASPxPopupControl>



        <dxe:ASPxPopupControl ID="ASPxDocumentsPopup" runat="server" ClientInstanceName="cDocumentsPopup"
            Width="350px" HeaderText="Select Design(s)" PopupHorizontalAlign="WindowCenter"
            PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
            Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True">
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">
                    <dxe:ASPxCallbackPanel runat="server" ID="SelectPanel" ClientInstanceName="cSelectPanel" OnCallback="SelectPanel_Callback" ClientSideEvents-EndCallback="cSelectPanelEndCall">
                        <PanelCollection>
                            <dxe:PanelContent runat="server">
                                <dxe:ASPxComboBox ID="CmbDesignName" ClientInstanceName="cCmbDesignName" runat="server" ValueType="System.String" Width="100%" EnableSynchronization="True">
                                </dxe:ASPxComboBox>
                                <div class="text-center pTop10">
                                    <dxe:ASPxButton ID="btnOK" ClientInstanceName="cbtnOK" runat="server" AutoPostBack="False" Text="OK" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                                        <ClientSideEvents Click="function(s, e) {return PerformCallToGridBind();}" />
                                    </dxe:ASPxButton>
                                </div>
                            </dxe:PanelContent>
                        </PanelCollection>
                    </dxe:ASPxCallbackPanel>
                </dxe:PopupControlContentControl>
            </ContentCollection>
        </dxe:ASPxPopupControl>
    </div>
    <%-- Sandip Approval Dtl Section Start--%>
    <div class="PopUpArea">
        <dxe:ASPxPopupControl ID="popupApproval" runat="server" ClientInstanceName="cpopupApproval"
            Width="900px" HeaderText="Pending Approvals" PopupHorizontalAlign="WindowCenter" HeaderStyle-CssClass="wht"
            PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
            Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
            ContentStyle-CssClass="pad">
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">
                    <%--<div style="Width:400px;background-color:#FFFFFF;margin:0px;border:1px solid red;">--%>
                    <div class="row">
                        <div class="col-md-12">
                            <dxe:ASPxGridView ID="gridPendingApproval" runat="server" KeyFieldName="ID" AutoGenerateColumns="False"
                                Width="100%" ClientInstanceName="cgridPendingApproval" OnCustomCallback="gridPendingApproval_CustomCallback" OnPageIndexChanged="gridPendingApproval_PageIndexChanged">
                                <Columns>
                                    <dxe:GridViewDataTextColumn Caption="Document No." FieldName="Number"
                                        VisibleIndex="0" FixedStyle="Left">
                                        <CellStyle CssClass="gridcellleft" Wrap="true">
                                        </CellStyle>
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn Caption="Posting Date" FieldName="CreateDate"
                                        VisibleIndex="1" FixedStyle="Left">
                                        <CellStyle CssClass="gridcellleft" Wrap="true">
                                        </CellStyle>
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn Caption="Unit" FieldName="branch_description"
                                        VisibleIndex="2" FixedStyle="Left">
                                        <CellStyle CssClass="gridcellleft" Wrap="true">
                                        </CellStyle>
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn Caption="Entered By" FieldName="craetedby"
                                        VisibleIndex="3" FixedStyle="Left">
                                        <CellStyle CssClass="gridcellleft" Wrap="true">
                                        </CellStyle>
                                    </dxe:GridViewDataTextColumn>

                                    <dxe:GridViewDataCheckColumn UnboundType="Boolean" Caption="Approved">
                                        <DataItemTemplate>
                                            <dxe:ASPxCheckBox ID="chkapprove" runat="server" AllowGrayed="false" OnInit="chkapprove_Init" ValueType="System.Boolean" ValueChecked="true" ValueUnchecked="false">
                                                <%--<ClientSideEvents CheckedChanged="function (s, e) {ch_fnApproved();}" />--%>
                                            </dxe:ASPxCheckBox>
                                        </DataItemTemplate>
                                        <Settings ShowFilterRowMenu="False" AllowFilterBySearchPanel="False" AllowAutoFilter="False" />
                                    </dxe:GridViewDataCheckColumn>

                                    <dxe:GridViewDataCheckColumn UnboundType="Boolean" Caption="Rejected">
                                        <DataItemTemplate>
                                            <dxe:ASPxCheckBox ID="chkreject" runat="server" AllowGrayed="false" OnInit="chkreject_Init" ValueType="System.Boolean" ValueChecked="true" ValueUnchecked="false">
                                                <%--<ClientSideEvents CheckedChanged="function (s, e) {ch_fnApproved();}" />--%>
                                            </dxe:ASPxCheckBox>
                                        </DataItemTemplate>
                                        <Settings ShowFilterRowMenu="False" AllowFilterBySearchPanel="False" AllowAutoFilter="False" />
                                    </dxe:GridViewDataCheckColumn>
                                </Columns>
                                <SettingsBehavior AllowFocusedRow="true" />
                                <SettingsPager NumericButtonCount="20" PageSize="10" ShowSeparators="True">
                                    <FirstPageButton Visible="True">
                                    </FirstPageButton>
                                    <LastPageButton Visible="True">
                                    </LastPageButton>
                                </SettingsPager>
                                <SettingsEditing Mode="Inline">
                                </SettingsEditing>
                                <%-- <SettingsSearchPanel Visible="True" />--%>
                                <Settings ShowGroupPanel="True" ShowStatusBar="Hidden" ShowHorizontalScrollBar="False" ShowFilterRow="true" ShowFilterRowMenu="true" />
                                <SettingsLoadingPanel Text="Please Wait..." />
                                <ClientSideEvents EndCallback="OnApprovalEndCall" />
                            </dxe:ASPxGridView>
                        </div>
                        <div class="clear"></div>


                        <%--<div class="col-md-12" style="padding-top: 10px;">
                            <dxe:ASPxButton ID="ASPxButton1" CausesValidation="true" ClientInstanceName="cbtn_PrpformaStatus" runat="server"
                                AutoPostBack="False" Text="Save" CssClass="btn btn-primary">
                                <ClientSideEvents Click="function (s, e) {SaveApprovalStatus();}" />
                            </dxe:ASPxButton>
                        </div>--%>
                    </div>
                </dxe:PopupControlContentControl>
            </ContentCollection>
        </dxe:ASPxPopupControl>
        <dxe:ASPxPopupControl ID="ASPXPopupControl" runat="server"
            CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="popup" Height="630px"
            Width="1200px" HeaderText="Quotation Approval" Modal="true" AllowResize="true" ResizingMode="Postponed">
            <HeaderTemplate>
                <span>User Approval</span>
            </HeaderTemplate>
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">
                </dxe:PopupControlContentControl>
            </ContentCollection>
        </dxe:ASPxPopupControl>
        <dxe:ASPxPopupControl ID="PopupUserWiseQuotation" runat="server" ClientInstanceName="cPopupUserWiseQuotation"
            Width="900px" HeaderText="User Wise Purchase Order Status" PopupHorizontalAlign="WindowCenter" HeaderStyle-CssClass="wht"
            PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
            Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
            ContentStyle-CssClass="pad">
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">
                    <div class="row">
                        <div class="col-md-12">
                            <dxe:ASPxGridView ID="gridUserWiseQuotation" runat="server" KeyFieldName="ID" AutoGenerateColumns="False"
                                Width="100%" ClientInstanceName="cgridUserWiseQuotation" OnCustomCallback="gridUserWiseQuotation_CustomCallback" OnPageIndexChanged="gridUserWiseQuotation_PageIndexChanged">
                                <Columns>
                                    <dxe:GridViewDataTextColumn Caption="Document No." FieldName="number"
                                        VisibleIndex="0" FixedStyle="Left">
                                        <CellStyle CssClass="gridcellleft" Wrap="true">
                                        </CellStyle>
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn Caption="Approval User" FieldName="approvedby"
                                        VisibleIndex="1" FixedStyle="Left">
                                        <CellStyle CssClass="gridcellleft" Wrap="true">
                                        </CellStyle>
                                    </dxe:GridViewDataTextColumn>

                                    <dxe:GridViewDataTextColumn Caption="User Level" FieldName="UserLevel"
                                        VisibleIndex="2" FixedStyle="Left">
                                        <CellStyle CssClass="gridcellleft" Wrap="true">
                                        </CellStyle>
                                    </dxe:GridViewDataTextColumn>

                                    <dxe:GridViewDataTextColumn Caption="Status" FieldName="status"
                                        VisibleIndex="3" FixedStyle="Left">
                                        <CellStyle CssClass="gridcellleft" Wrap="true">
                                        </CellStyle>
                                    </dxe:GridViewDataTextColumn>

                                </Columns>
                                <SettingsBehavior AllowFocusedRow="true" />
                                <SettingsPager NumericButtonCount="20" PageSize="10" ShowSeparators="True">
                                    <FirstPageButton Visible="True">
                                    </FirstPageButton>
                                    <LastPageButton Visible="True">
                                    </LastPageButton>
                                </SettingsPager>
                                <SettingsEditing Mode="Inline">
                                </SettingsEditing>
                                <%-- <SettingsSearchPanel Visible="True" />--%>
                                <Settings ShowGroupPanel="True" ShowStatusBar="Hidden" ShowHorizontalScrollBar="False" ShowFilterRow="true" ShowFilterRowMenu="true" />
                                <SettingsLoadingPanel Text="Please Wait..." />

                            </dxe:ASPxGridView>
                        </div>
                        <div class="clear"></div>
                    </div>
                </dxe:PopupControlContentControl>
            </ContentCollection>
        </dxe:ASPxPopupControl>
    </div>

    <%-- Sandip Approval Dtl Section End--%>
    <dxe:ASPxGlobalEvents ID="GlobalEvents" runat="server">
        <ClientSideEvents ControlsInitialized="AllControlInitilize" />
    </dxe:ASPxGlobalEvents>

    <div>
        <asp:HiddenField ID="hfIsFilter" runat="server" />
        <asp:HiddenField ID="hfFromDate" runat="server" />
        <asp:HiddenField ID="hfToDate" runat="server" />
        <asp:HiddenField ID="hfBranchID" runat="server" />
        <asp:HiddenField ID="hfIsBarcode" runat="server" />
    </div>

    <div>
        <dxe:ASPxPopupControl ID="PopupWarehouse" runat="server" ClientInstanceName="cPopupWarehouse"
            Width="900px" HeaderText="Stock Details" PopupHorizontalAlign="WindowCenter"
            BackColor="white" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
            Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
            ContentStyle-CssClass="pad">
            <ContentStyle VerticalAlign="Top" CssClass="pad">
            </ContentStyle>
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">
                    <dxe:ASPxCallbackPanel runat="server" ID="DocumentPanel" ClientInstanceName="cDocumentPanel" OnCallback="DocumentPanel_Callback">
                        <PanelCollection>
                            <dxe:PanelContent runat="server">
                                <div>
                                    <div class="col-md-12">
                                        <div class="row">
                                            <div class="col-md-6">
                                                <table width="100%" class="tblPad">
                                                    <tr>
                                                        <td>Document No.</td>
                                                        <td>
                                                            <dxe:ASPxTextBox ID="ASPxTextBox1" runat="server" ClientInstanceName="ctxtDocNumber" ReadOnly="true" Width="100%">
                                                            </dxe:ASPxTextBox>
                                                        </td>
                                                        <td></td>
                                                    </tr>
                                                    <tr>
                                                        <td>Unit</td>
                                                        <td>
                                                            <dxe:ASPxTextBox ID="ASPxTextBox2" runat="server" ClientInstanceName="ctxtBatch" ReadOnly="true" Width="100%">
                                                            </dxe:ASPxTextBox>
                                                        </td>
                                                        <td></td>
                                                    </tr>
                                                    <tr>
                                                        <td>Vendor</td>
                                                        <td>
                                                            <dxe:ASPxTextBox ID="ASPxTextBox3" runat="server" ClientInstanceName="ctxtVendor" ReadOnly="true" Width="100%">
                                                            </dxe:ASPxTextBox>
                                                        </td>
                                                        <td></td>
                                                    </tr>
                                                </table>
                                            </div>
                                            <div class="col-md-6" style="border-left: 1px solid #ccc; height: 98px;">
                                                <table width="100%" class="tblPad">
                                                    <tr>
                                                        <td>Product Name</td>
                                                        <td>
                                                            <dxe:ASPxTextBox ID="ASPxTextBox4" runat="server" ClientInstanceName="ctxtProduct" ReadOnly="true" Width="100%">
                                                            </dxe:ASPxTextBox>
                                                        </td>
                                                        <td></td>
                                                    </tr>
                                                    <tr>
                                                        <td>Generate Qty</td>
                                                        <td>
                                                            <dxe:ASPxSpinEdit ID="ctxtQty" ClientInstanceName="ctxtQty" runat="server" NumberType="Integer"
                                                                Width="100%" AllowMouseWheel="false">
                                                                <SpinButtons ShowIncrementButtons="false"></SpinButtons>
                                                            </dxe:ASPxSpinEdit>
                                                        </td>
                                                        <td class="pdLeft">
                                                            <dxe:ASPxButton ID="ASPxButton1" ClientInstanceName="cbtn_Generate" runat="server" AutoPostBack="False"
                                                                Text="G&#818;enerate" CssClass="btn btn-primary" UseSubmitBehavior="false">
                                                                <ClientSideEvents Click="function(s, e) {GenerateProductBarcode();}" />
                                                            </dxe:ASPxButton>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-6">

                                                <div class="clear"></div>
                                                <dxe:ASPxGridView ID="DocumentGrid" ClientInstanceName="cDocumentGrid" runat="server" AutoGenerateColumns="False"
                                                    KeyFieldName="OrderDetails_Id" Width="100%" Settings-VerticalScrollBarMode="Visible" SettingsBehavior-AllowFocusedRow="true"
                                                    SettingsBehavior-AllowDragDrop="false" SettingsPager-Mode="ShowAllRecords" Settings-VerticalScrollableHeight="300">
                                                    <Columns>
                                                        <dxe:GridViewDataTextColumn FieldName="Products_Name" Caption="Product Name" VisibleIndex="0">
                                                        </dxe:GridViewDataTextColumn>
                                                        <dxe:GridViewDataTextColumn FieldName="Quantity" Caption="Quantity" Width="60" VisibleIndex="1">
                                                        </dxe:GridViewDataTextColumn>
                                                    </Columns>
                                                    <ClientSideEvents RowClick="OnRowClick" />
                                                    <Settings ShowFilterRow="false" ShowFilterRowMenu="false" />
                                                    <SettingsBehavior AllowDragDrop="False"></SettingsBehavior>
                                                    <SettingsPager Visible="false"></SettingsPager>
                                                </dxe:ASPxGridView>
                                            </div>
                                            <div class="col-md-6" style="border-left: 1px solid #ccc;">

                                                <div class="clear"></div>
                                                <dxe:ASPxGridView ID="StockGrid" ClientInstanceName="cStockGrid" runat="server" AutoGenerateColumns="False"
                                                    KeyFieldName="PurchaseOrder_MapId" Width="100%" Settings-VerticalScrollBarMode="Visible" SettingsBehavior-AllowFocusedRow="true"
                                                    SettingsBehavior-AllowDragDrop="false" SettingsPager-Mode="ShowAllRecords" Settings-VerticalScrollableHeight="300">
                                                    <Columns>
                                                        <dxe:GridViewDataTextColumn FieldName="Barcode" Caption="Barcode" VisibleIndex="0">
                                                        </dxe:GridViewDataTextColumn>
                                                        <dxe:GridViewDataTextColumn FieldName="Serial_Number" Caption="Serial No" VisibleIndex="1">
                                                        </dxe:GridViewDataTextColumn>
                                                    </Columns>
                                                    <ClientSideEvents RowClick="OnDetailsRowClick" />
                                                    <Settings ShowFilterRow="false" ShowFilterRowMenu="false" />
                                                    <SettingsBehavior AllowDragDrop="False"></SettingsBehavior>
                                                    <SettingsPager Visible="false"></SettingsPager>
                                                </dxe:ASPxGridView>
                                                <div class="clear"></div>
                                                <table width="100%">
                                                    <tr>
                                                        <td class="pdTop">Put Serial No</td>
                                                        <td class="pdTop">
                                                            <dxe:ASPxTextBox ID="cSerialNo" runat="server" ClientInstanceName="cSerialNo" Width="100%">
                                                            </dxe:ASPxTextBox>
                                                        </td>
                                                        <td class="pdTop pdLeft">
                                                            <dxe:ASPxButton ID="cbtn_Save" ClientInstanceName="cbtn_Save" runat="server" AutoPostBack="False"
                                                                Text="Update" CssClass="btn btn-primary" UseSubmitBehavior="false">
                                                                <ClientSideEvents Click="function(s, e) {SaveSerial();}" />
                                                            </dxe:ASPxButton>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div>
                                    <asp:HiddenField runat="server" ID="hdfDocNumber" />
                                    <asp:HiddenField runat="server" ID="hdfDocDetailsNumber" />
                                    <asp:HiddenField runat="server" ID="hdfBranch" />
                                    <asp:HiddenField runat="server" ID="hdfMapID" />
                                    <asp:HiddenField runat="server" ID="hdfIsBarcodeActive" />
                                    <asp:HiddenField runat="server" ID="hdfIsBarcodeGenerator" />
                                    <asp:HiddenField ID="hddnKeyValue" runat="server" />
                                </div>
                            </dxe:PanelContent>
                        </PanelCollection>
                        <ClientSideEvents EndCallback="DocumentPanelEndCall" />
                    </dxe:ASPxCallbackPanel>
                </dxe:PopupControlContentControl>
            </ContentCollection>
            <HeaderStyle BackColor="LightGray" ForeColor="Black" />
        </dxe:ASPxPopupControl>
    </div>
</asp:Content>

