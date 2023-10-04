<%--/*************************************************************************************************************************************************
 *  Rev 1.0     Sanchita    V2.0.40     28-09-2023      Data Freeze Required for Project Sale Invoice & Project Purchase Invoice. Mantis:26854
 *************************************************************************************************************************************************/--%>

<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="projectOrderList.aspx.cs" Inherits="ERP.OMS.Management.Activities.projectOrderList" %>



<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <%--<link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/jquery-confirm/3.1.0/jquery-confirm.min.css">--%>
    <link href="../../../assests/css/custom/jquery.confirm.css" rel="stylesheet" />
    <%--Code Added By Sandip For Approval Detail Section Start--%>
    <script>
        <%-- Code block By Debashis Talukder--%>
        //function onPrintJv(id) {
        //    window.location.href = "../../reports/XtraReports/Viewer/OrderReportViewer.aspx?id=" + id;
        //}
        <%-- Code block By Debashis Talukder--%>
        function OnProductWiseClosedClick(keyValue, visibleIndex, PurchaseOrder) {
            $("#hddnKeyValue").val(keyValue);
            cGrdOrder.SetFocusedRowIndex(visibleIndex);
            cPopupProductwiseClose.Show();
            $("#txtVoucherNo").val(PurchaseOrder);
            cbtnCloseProduct.SetVisible(true);
            cgridProductwiseClose.PerformCallback("ShowData~" + keyValue);
        }

        function CloseProduct() {
            cgridProductwiseClose.PerformCallback("CloseData");
        }
        function OnProductCloseEndCall() {
            if (cgridProductwiseClose.cpProductClose == "YES") {
                jAlert("Close successfully");
                cGrdOrder.Refresh();
                cgridProductwiseClose.cpProductClose = null
                cPopupProductwiseClose.Hide();
            }
            else if (cgridProductwiseClose.cpBtnCloseVIsible == "NO") {

                //cbtnCloseProduct.SetVisible(false);
                cgridProductwiseClose.cpBtnCloseVIsible = "YES";
                cPopupProductwiseClose.Hide();
                jAlert("No balance quantity available for this order. Cannot proceed.");


            }

        }

        //This function is called to show the Status of All Sales Order Created By Login User Start
        function timerTick() {
            //   cwatingInvoicegrid.Refresh();

            $.ajax({
                type: "POST",
                url: "projectOrderList.aspx/GetTotalWatingOrderCount",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    debugger;
                    var status = msg.d;
                    console.log(status);
                    ClblQuoteweatingCount.SetText(status);
                    var fetcheddata = parseFloat(document.getElementById('waitingOrderCount').value);
                    if (status != fetcheddata) {
                        CwatingQuotegrid.Refresh();
                        document.getElementById('waitingOrderCount').value = status;
                    }
                }
            });

        }


        function OrderWattingOkClick() {
            var index = CwatingOrdergrid.GetFocusedRowIndex();
            var listKey = CwatingOrdergrid.GetRowKey(index);
            if (listKey) {
                if (CwatingOrdergrid.GetRow(index).children[6].innerText != "Advance") {
                    var url = 'ProjectOrder.aspx?key=' + 'ADD&&BasketId=' + listKey;
                    //LoadingPanel.Show();
                    window.location.href = url;
                } else {
                    // ShowbasketReceiptPayment(listKey);
                }
            }
        }


        function OnWaitingGridKeyPress(e) {

            if (e.code == "Enter") {
                var index = CwatingOrdergrid.GetFocusedRowIndex();
                var listKey = CwatingOrdergrid.GetRowKey(index);
                if (listKey) {
                    if (CwatingOrdergrid.GetRow(index).children[6].innerText != "Advance") {
                        var url = 'ProjectOrder.aspx?key=' + 'ADD&&BasketId=' + listKey;
                        // LoadingPanel.Show();
                        window.location.href = url;
                    } else {
                        //ShowbasketReceiptPayment(listKey);
                    }
                }
            }

        }

        function OpenWaitingOrder() {
            Cpopup_OrderWait.Show();
            CwatingOrdergrid.Focus();
        }

        function ListRowClicked(s, e) {

            var index = e.visibleIndex;
            var listKey = CwatingOrdergrid.GetRowKey(index);
            if (e.htmlEvent.target.id != "CloseRemoveWattingBtn") {
                if (CwatingOrdergrid.GetRow(index).children[6].innerText != "Advance") {
                    var url = 'ProjectOrder.aspx?key=' + 'ADD&&BasketId=' + listKey;
                    //LoadingPanel.Show();
                    window.location.href = url;
                } else {
                    // ShowbasketReceiptPayment(listKey);
                }
            }
        }


        function watingOrdergridEndCallback() {
            if (CwatingOrdergrid.cpReturnMsg) {
                if (CwatingOrdergrid.cpReturnMsg != "") {
                    jAlert(CwatingOrdergrid.cpReturnMsg);
                    document.getElementById('waitingOrderCount').value = parseFloat(document.getElementById('waitingOrderCount').value) - 1;
                    CwatingOrdergrid.cpReturnMsg = null;
                }
            }
        }

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
            uri = "ProjectOrder.aspx?key=" + obj + "&status=2" + '&type=SO' + '&isformApprove=YES';
            popup.SetContentUrl(uri);
            popup.Show();
        }
        function closeUserApproval() {
            popup.Hide();
        }
        // function above  End For Approved

        // Status 3 is passed If Approved Check box is checked by User Both Below function is called and used to show in POPUP,  the Add Page of Respective Segment(like Page for Adding Quotation ,Sale Order ,Challan)
        function GetRejectedQuoteId(s, e, itemIndex) {
            //debugger;
            cgridPendingApproval.GetRowValues(itemIndex, 'ID', OnGetRejectedRowValues);

        }
        function OnGetRejectedRowValues(obj) {
            uri = "ProjectOrder.aspx?key=" + obj + "&status=3" + '&type=SO';
            popup.SetContentUrl(uri);
            popup.Show();
        }
        // function above  End For Rejected

        // To Reflect the Data in Pending Waiting Grid and Pending Waiting Counting if the user approve or Rejecte the Order and Saved 

        function OnApprovalEndCall(s, e) {
            $.ajax({
                type: "POST",
                url: "ProjectOrder.aspx/GetPendingCase",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    $('#<%= lblWaiting.ClientID %>').text(data.d);
                }
            });
            }

            // function above  End 

    </script>
    <style>
        #ASPXPopupControl_PW-1 iframe .crossBtn {
            display: none;
        }

        .btn.typeNotificationBtn {
            position: relative;
            padding-right: 16px !important;
        }

        .typeNotification {
            position: absolute;
            width: 22px;
            height: 22px;
            background: #ff5140;
            display: block;
            font-size: 12px;
            border-radius: 50%;
            right: -7px;
            top: -9px;
            line-height: 22px;
        }
    </style>


    <%-- Code Added By Sandip For Approval Detail Section End--%>

    <script>
         <%-- Code Added By Debashis Talukder For Document Printing End--%>
        var SOrderId = 0;
        function onPrintJv(id) {
            //window.location.href = "../../reports/XtraReports/Viewer/PurchaseOrderReportViewer.aspx?id=" + id;
            //debugger;
            SOrderId = id;
            cDocumentsPopup.Show();
            cCmbDesignName.SetSelectedIndex(0);
            cSelectPanel.PerformCallback('Bindalldesignes');
            $('#btnOK').focus();
        }



        var InvoiceId = 0;
        var Type = '';
        function onPrintSi(id, type) {
            //debugger;
            Type = type;
            InvoiceId = id;
            cInvoiceSelectPanel.cpSuccess = "";
            cInvoiceDocumentsPopup.Show();
            CselectOriginal.SetCheckState('UnChecked');
            CselectDuplicate.SetCheckState('UnChecked');
            CselectTriplicate.SetCheckState('UnChecked');
            CselectOfficecopy.SetCheckState('UnChecked');
            cInvoiceCmbDesignName.SetSelectedIndex(0);
            cInvoiceSelectPanel.PerformCallback('Bindalldesignes' + '~' + Type);
            $('#btnInvoiceOK').focus();
        }



        function PerformCallToGridBind() {
            cSelectPanel.PerformCallback('Bindsingledesign');
            cDocumentsPopup.Hide();
            return false;
        }


        function PerformInvoiceCallToGridBind() {
            //cgriddocuments.PerformCallback('BindDocumentsGridOnSelection' + '~' + InvoiceId);
            //cDocumentsPopup.Hide();
            //return false;
            cInvoiceSelectPanel.PerformCallback('Bindsingledesign' + '~' + Type);
            cInvoiceDocumentsPopup.Hide();
            return false;
        }
        var isFirstTime = true;
        function AllControlInitilize() {
            //debugger;
            if (isFirstTime) {

                if (localStorage.getItem('FromDateSalesOrder')) {
                    var fromdatearray = localStorage.getItem('FromDateSalesOrder').split('-');
                    var fromdate = new Date(fromdatearray[0], parseFloat(fromdatearray[1]) - 1, fromdatearray[2], 0, 0, 0, 0);
                    cFormDate.SetDate(fromdate);
                }
                if (localStorage.getItem('ToDateSalesOrder')) {
                    var todatearray = localStorage.getItem('ToDateSalesOrder').split('-');
                    var todate = new Date(todatearray[0], parseFloat(todatearray[1]) - 1, todatearray[2], 0, 0, 0, 0);
                    ctoDate.SetDate(todate);
                }
                if (localStorage.getItem('OrderBranch')) {
                    if (ccmbBranchfilter.FindItemByValue(localStorage.getItem('OrderBranch'))) {
                        ccmbBranchfilter.SetValue(localStorage.getItem('OrderBranch'));
                    }

                }
                //updateGridByDate();

                isFirstTime = false;
            }
        }

        //Function for Date wise filteration
        function updateGridByDate() {
            //debugger;
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
                localStorage.setItem("FromDateSalesOrder", cFormDate.GetDate().format('yyyy-MM-dd'));
                localStorage.setItem("ToDateSalesOrder", ctoDate.GetDate().format('yyyy-MM-dd'));
                localStorage.setItem("OrderBranch", ccmbBranchfilter.GetValue());
                //cGrdOrder.PerformCallback('FilterGridByDate~' + cFormDate.GetDate().format('yyyy-MM-dd') + '~' + ctoDate.GetDate().format('yyyy-MM-dd') + '~' + ccmbBranchfilter.GetValue());
                $("#hfFromDate").val(cFormDate.GetDate().format('yyyy-MM-dd'));
                $("#hfToDate").val(ctoDate.GetDate().format('yyyy-MM-dd'));
                $("#hfBranchID").val(ccmbBranchfilter.GetValue());
                $("#hfIsFilter").val("Y");
                cGrdOrder.Refresh();


            }
        }
        //End

        function cSelectPanelEndCall(s, e) {
            //debugger;
            if (cSelectPanel.cpSuccess != null) {
                var TotDocument = cSelectPanel.cpSuccess.split(',');
                var reportName = cCmbDesignName.GetValue();
                var module = 'Sorder';
                window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + reportName + '&modulename=' + module + '&id=' + SOrderId, '_blank')
            }
            cSelectPanel.cpSuccess = null
            if (cSelectPanel.cpSuccess == null) {
                cCmbDesignName.SetSelectedIndex(0);
            }
        }



        function cInvoiceSelectPanelEndCall(s, e) {
            //debugger;
            if (cInvoiceSelectPanel.cpSuccess != "") {
                var TotDocument = cInvoiceSelectPanel.cpSuccess.split(',');
                var reportName = cInvoiceCmbDesignName.GetValue();
                if (cInvoiceSelectPanel.cpType == "SI") {
                    var module = 'Invoice';
                }
                else {
                    var module = 'TSInvoice';
                }

                if (TotDocument.length > 0) {
                    for (var i = 0; i < TotDocument.length; i++) {
                        if (TotDocument[i] != "") {
                            window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + reportName + '&modulename=' + module + '&id=' + InvoiceId + '&PrintOption=' + TotDocument[i], '_blank')
                        }
                    }
                }
            }
            //cSelectPanel.cpSuccess = null
            if (cInvoiceSelectPanel.cpSuccess == "") {
                if (cInvoiceSelectPanel.cpChecked != "") {
                    jAlert('Please check Original For Recipient and proceed.');
                }
                //CselectDuplicate.SetEnabled(false);
                //CselectTriplicate.SetEnabled(false);
                CselectOriginal.SetCheckState('UnChecked');
                CselectDuplicate.SetCheckState('UnChecked');
                CselectTriplicate.SetCheckState('UnChecked');
                CselectOfficecopy.SetCheckState('UnChecked');
                cInvoiceCmbDesignName.SetSelectedIndex(0);
            }
        }
         <%-- Code Added By Debashis Talukder For Document Printing End--%>
        document.onkeydown = function (e) {
            var isCtrl = false;
            if (event.keyCode == 18) isCtrl = true;


            if (event.keyCode == 65 && isCtrl == true) { //run code for alt+a -- ie, Add
                StopDefaultAction(e);
                OnAddButtonClick();
            }
            //Subhabrata cancel close hotkey

            var CancelCloseFlag = $("#<%=hddnCancelCloseFlag%>").val();

            if (event.keyCode == 83 && isCtrl == true && CancelCloseFlag.trim() == 'CA') {
                CallFeedback_save();
            }
            if (event.keyCode == 67 && isCtrl == true && CancelCloseFlag.trim() == 'CA') {
                CancelFeedback_save();
            }
            if (event.keyCode == 83 && isCtrl == true && CancelCloseFlag.trim() == 'CL') {
                CallClosed_save();
            }
            if (event.keyCode == 67 && isCtrl == true && CancelCloseFlag.trim() == 'CL') {
                CancelClosed_save();
            }
            //End

        }

        function StopDefaultAction(e) {
            if (e.preventDefault) { e.preventDefault() }
            else { e.stop() };

            e.returnValue = false;
            e.stopPropagation();
        }
        function OnClickDelete(keyValue, visibleIndex) {
            cGrdOrder.SetFocusedRowIndex(visibleIndex);
            var IsBalMapQtyExists = cGrdOrder.GetRow(cGrdOrder.GetFocusedRowIndex()).children[18].innerText;
            //if (IsBalMapQtyExists.trim() != "0") {
            var IsCancelFlag = cGrdOrder.GetRow(cGrdOrder.GetFocusedRowIndex()).children[14].innerText;
            if (IsCancelFlag.trim() != 'Cancelled' && IsCancelFlag.trim() != 'Closed') {

                $.ajax({
                    type: "POST",
                    url: "SalesOrderList.aspx/GetSalesInvoiceIsExistInSalesInvoice",
                    data: "{'keyValue':'" + keyValue + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: false,//Added By:Subhabrata
                    success: function (msg) {
                        //debugger;
                        var status = msg.d;
                        if (status == "1") {
                            jAlert('Used in other module(s). Cannot Delete.', 'Confirmation Dialog', function (r) {
                                if (r == true) {
                                    return false;
                                }
                            });
                        }
                        else {
                            jConfirm('Confirm delete?', 'Confirmation Dialog', function (r) {
                                if (r == true) {
                                    cGrdOrder.PerformCallback('Delete~' + keyValue);
                                }
                            });
                        }
                    }
                });
            }
            else {
                jAlert("Project Order is " + IsCancelFlag.trim() + ".Delete is not allowed.");
            }
            //}
            //else
            //{
            //    jAlert("Sales Order is tagged with other module.Delete is not allowed.");
            //}
        }

        function OnclickViewAttachment(obj) {
            //var URL = '/OMS/Management/Activities/SalesOrder_Document.aspx?idbldng=' + obj + '&type=SalesOrder';
            var URL = '/OMS/Management/Activities/EntriesDocuments.aspx?idbldng=' + obj + '&type=ProjectSOrder';
            window.location.href = URL;
        }
        function OnClickStatus(keyValue) {
            GetObjectID('hiddenedit').value = keyValue;
            cGrdOrder.PerformCallback('Edit~' + keyValue);
        }
        function grid_EndCallBack() {
            if (cGrdOrder.cpEdit != null) {
                GetObjectID('hiddenedit').value = cGrdOrder.cpEdit.split('~')[0];
                cProforma.SetText(cGrdOrder.cpEdit.split('~')[1]);
                cCustomer.SetText(cGrdOrder.cpEdit.split('~')[4]);
                var pro_status = cGrdOrder.cpEdit.split('~')[2]
                if (pro_status != null) {
                    var radio = $("[id*=rbl_OrderStatus] label:contains('" + pro_status + "')").closest("td").find("input");
                    radio.attr("checked", "checked");
                    //return false;
                    //$('#rbl_QuoteStatus[type=radio][value=' + pro_status + ']').prop('checked', true); 
                    cOrderRemarks.SetText(cGrdOrder.cpEdit.split('~')[3]);
                    cOrderStatus.Show();
                }
            }
            if (cGrdOrder.cpDelete != null) {
                jAlert(cGrdOrder.cpDelete);
                cGrdOrder.cpDelete = null;
                cGrdOrder.Refresh();
            }
        }
        function SavePrpformaStatus() {
            if (document.getElementById('hiddenedit').value == '') {
                cGrdOrder.PerformCallback('save~');
            }
            else {
                cGrdOrder.PerformCallback('update~' + GetObjectID('hiddenedit').value);
            }

        }

        function OnMoreInfoClick(keyValue, visibleIndex) {
            debugger;
            cGrdOrder.SetFocusedRowIndex(visibleIndex);
            var IsBalMapQtyExists = cGrdOrder.GetRow(cGrdOrder.GetFocusedRowIndex()).children[17].innerHTML; //cGrdOrder.GetRow(cGrdOrder.GetFocusedRowIndex()).children[18].innerText;
            //if (IsBalMapQtyExists.trim() != "0") {


            var IsCancelFlag = cGrdOrder.GetRow(cGrdOrder.GetFocusedRowIndex()).children[16].innerText;

            if (IsCancelFlag.trim() != 'Cancelled' && IsCancelFlag.trim() != 'Closed') {
                var ActiveUser = '<%=Session["userid"]%>'
                if (ActiveUser != null) {
                    $.ajax({
                        type: "POST",
                        url: "SalesOrderList.aspx/GetEditablePermission",
                        data: "{'ActiveUser':'" + ActiveUser + "'}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        async: false,//Added By:Subhabrata
                        success: function (msg) {
                            //debugger;
                            var status = msg.d;
                            var url = 'ProjectOrder.aspx?key=' + keyValue + '&Permission=' + status + '&type=SO';
                            window.location.href = url;
                        }
                    });
                }
            }
            else {
                jAlert("Project Sales Order is " + IsCancelFlag.trim() + ".Edit is not allowed.");
            }
            //}
            //else {
            //    jAlert("Sales Order is tagged with other module.Edit is not allowed.");
            //}
        }

        function OnApproveClick(keyValue, visibleIndex) {
            if ($("#hdnIsMultiuserApprovalRequired").val() == "Yes") {
                $.ajax({
                    type: "POST",
                    url: "projectOrderList.aspx/SalesOrderApproval",
                    data: JSON.stringify({ keyValue: keyValue }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: false,
                    success: function (msg) {
                        if (msg != null && msg == true) {
                            //document.getElementById("forfeitTable2").style.display = "block";
                            //document.getElementById("forfeitTable2").style.display = "block";
                            cGrdOrder.SetFocusedRowIndex(visibleIndex);
                            var IsBalMapQtyExists = cGrdOrder.GetRow(cGrdOrder.GetFocusedRowIndex()).children[17].innerHTML; //cGrdOrder.GetRow(cGrdOrder.GetFocusedRowIndex()).children[18].innerText;
                            //if (IsBalMapQtyExists.trim() != "0") {
                            var IsCancelFlag = cGrdOrder.GetRow(cGrdOrder.GetFocusedRowIndex()).children[16].innerText;
                            if (IsCancelFlag.trim() != 'Cancelled' && IsCancelFlag.trim() != 'Closed') {
                                var ActiveUser = '<%=Session["userid"]%>'
                                if (ActiveUser != null) {
                                    $.ajax({
                                        type: "POST",
                                        url: "SalesOrderList.aspx/GetEditablePermission",
                                        data: "{'ActiveUser':'" + ActiveUser + "'}",
                                        contentType: "application/json; charset=utf-8",
                                        dataType: "json",
                                        async: false,//Added By:Subhabrata
                                        success: function (msg) {
                                            //debugger;
                                            var status = msg.d;
                                            var url = 'ProjectOrder.aspx?key=' + keyValue + '&Permission=' + status + '&type=SO' + '&type1=PO';
                                            window.location.href = url;
                                        }
                                    });
                                }
                            }
                            else {
                                jAlert("Project Sales Order is " + IsCancelFlag.trim() + ".Edit is not allowed.");
                            }
                        }
                        else {
                            jAlert('You dont have permission.');
                            return false;
                        }
                    }
                });
            }
            else {
                //document.getElementById("forfeitTable2").style.display = "block";
                //document.getElementById("forfeitTable2").style.display = "block";
                cGrdOrder.SetFocusedRowIndex(visibleIndex);
                var IsBalMapQtyExists = cGrdOrder.GetRow(cGrdOrder.GetFocusedRowIndex()).children[17].innerHTML; //cGrdOrder.GetRow(cGrdOrder.GetFocusedRowIndex()).children[18].innerText;
                //if (IsBalMapQtyExists.trim() != "0") {
                var IsCancelFlag = cGrdOrder.GetRow(cGrdOrder.GetFocusedRowIndex()).children[16].innerText;
                if (IsCancelFlag.trim() != 'Cancelled' && IsCancelFlag.trim() != 'Closed') {
                    var ActiveUser = '<%=Session["userid"]%>'
                    if (ActiveUser != null) {
                        $.ajax({
                            type: "POST",
                            url: "SalesOrderList.aspx/GetEditablePermission",
                            data: "{'ActiveUser':'" + ActiveUser + "'}",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            async: false,//Added By:Subhabrata
                            success: function (msg) {
                                //debugger;
                                var status = msg.d;
                                var url = 'ProjectOrder.aspx?key=' + keyValue + '&Permission=' + status + '&type=SO' + '&type1=PO';
                                window.location.href = url;
                            }
                        });
                    }
                }
                else {
                    jAlert("Project Sales Order is " + IsCancelFlag.trim() + ".Edit is not allowed.");
                }
            }
        }


        ////##### coded by Samrat Roy - 04/05/2017  
        ////Add an another param to define request type 
        function OnViewClick(keyValue) {
            var url = 'ProjectOrder.aspx?key=' + keyValue + '&req=V';
            window.location.href = url;
        }

        function OnCancelClick(keyValue, visibleIndex) {
            //debugger;
            $("#<%=hddnKeyValue.ClientID%>").val(keyValue);
            $("#<%=hddnCancelCloseFlag%>").val('CA');
            cGrdOrder.SetFocusedRowIndex(visibleIndex);
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
            //debugger;
            $("#<%=hddnKeyValue.ClientID%>").val(keyValue);
             $("#<%=hddnCancelCloseFlag%>").val('CL');
             cGrdOrder.SetFocusedRowIndex(visibleIndex);
             var IsCancel = cGrdOrder.GetRow(cGrdOrder.GetFocusedRowIndex()).children[6].innerText;

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

         function CallFeedback_save() {
             // debugger;
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
                cGrdOrder.Refresh();
                //cGrdOrder.PerformCallback('Feedback~' + KeyVal + '~' + Remarks);
                //Grid.Refresh();


            }
            return flag;

        }

        function CallClosed_save() {
            //debugger;
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
                 cGrdOrder.Refresh();
                 //cGrdOrder.PerformCallback('Feedback~' + KeyVal + '~' + Remarks);
                 //Grid.Refresh();


             }
             return flag;

         }

         function CancelSalesOrder(keyValue, Reason) {
             //debugger;
             $.ajax({
                 type: "POST",
                 url: "projectOrderList.aspx/CancelProjectSalesOrderOnRequest",
                 data: JSON.stringify({ keyValue: keyValue, Reason: Reason }),
                 contentType: "application/json; charset=utf-8",
                 dataType: "json",
                 async: false,//Added By:Subhabrata
                 success: function (msg) {
                     //debugger;
                     var status = msg.d;
                     if (status == "1") {
                         jAlert("Order is cancelled successfully.");
                         //cGrdOrder.PerformCallback('BindGrid');


                     }
                     else if (status == "-1") {
                         jAlert("Order is not cancelled.Try again later");
                     }
                     else if (status == "-2") {
                         jAlert("Selected order is tagged in other module. Cannot proceed.");
                     }
                     else if (status == "-3") {
                         jAlert("Order is  already cancelled.");
                     }
                     else if (status == "-4") {
                         jAlert("Order is already closed. Cannot proceed.");
                     }
                     //else if (status == "-5") {
                     //    jAlert("No balance quantity available for this order. Cannot proceed.");
                     //}
                 }
             });
         }


         function ClosedSalesOrder(keyValue, Reason) {
             //debugger;
             $.ajax({
                 type: "POST",
                 url: "SalesOrderEntityList.aspx/ClosedSalesOrderOnRequest",
                 data: JSON.stringify({ keyValue: keyValue, Reason: Reason }),
                 contentType: "application/json; charset=utf-8",
                 dataType: "json",
                 async: false,//Added By:Subhabrata
                 success: function (msg) {
                     //debugger;
                     var status = msg.d;
                     if (status == "1") {
                         jAlert("Order is closed successfully.");
                         //cGrdOrder.PerformCallback('BindGrid');

                     }
                     else if (status == "-1") {
                         jAlert("Order is not closed.Try again later");
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




         function CancelFeedback_save() {

            <%-- $("#<%=hddnIsSavedFeedback.ClientID%>").val("0");--%>
            txtFeedback.SetValue();
            cPopup_Feedback.Hide();
            //$('#chkmailfeedback').prop('checked', false);
        }

        function CancelClosed_save() {

            <%-- $("#<%=hddnIsSavedFeedback.ClientID%>").val("0");--%>
             txtClosed.SetValue();
             cPopup_Closed.Hide();
             //$('#chkmailfeedback').prop('checked', false);
         }




         function OnAddButtonClick() {
             var url = 'ProjectOrder.aspx?key=' + 'ADD';
             window.location.href = url;
         }





         function OnAddEditClick(e, obj) {

             var data = obj.split('~');
             cInvoiceNumberpopup.Show();
             popInvoiceNumberPanel.PerformCallback(obj);

         }


         function OnAddEditDateClick(e, obj) {
             var data = obj.split('~');

             //if (data[2] == 'Multiple') {
             cInvoiceDatepopup.Show();
             popInvoiceDatePanel.PerformCallback(obj);
             // }

         }
         function gridRowclick(s, e) {
             //alert('hi');
             $('#GrdOrder').find('tr').removeClass('rowActive');
             $('.floatedBtnArea').removeClass('insideGrid');
             //$('.floatedBtnArea a .ico').css({ 'opacity': '0' });
             $(s.GetRow(e.visibleIndex)).find('.floatedBtnArea').addClass('insideGrid');
             $(s.GetRow(e.visibleIndex)).addClass('rowActive');
             setTimeout(function () {
                 //alert('delay');
                 var lists = $(s.GetRow(e.visibleIndex)).find('.floatedBtnArea a');
                 //$(s.GetRow(e.visibleIndex)).find('.floatedBtnArea a .ico').css({'opacity': '1'});
                 //$(s.GetRow(e.visibleIndex)).find('.floatedBtnArea a').each(function (e) {
                 //    setTimeout(function () {
                 //        $(this).fadeIn();
                 //    }, 100);
                 //});    
                 $.each(lists, function (index, value) {
                     //console.log(index);
                     //console.log(value);
                     setTimeout(function () {
                         console.log(value);
                         $(value).css({ 'opacity': '1' });
                     }, 100);
                 });
             }, 200);
         }
         $(document).ready(function () {
             if ($('body').hasClass('mini-navbar')) {
                 var windowWidth = $(window).width();
                 var cntWidth = windowWidth - 90;
                 cGrdOrder.SetWidth(cntWidth);
             } else {
                 var windowWidth = $(window).width();
                 var cntWidth = windowWidth - 220;
                 cGrdOrder.SetWidth(cntWidth);
             }

             $('.navbar-minimalize').click(function () {
                 if ($('body').hasClass('mini-navbar')) {
                     var windowWidth = $(window).width();
                     var cntWidth = windowWidth - 220;
                     cGrdOrder.SetWidth(cntWidth);
                 } else {
                     var windowWidth = $(window).width();
                     var cntWidth = windowWidth - 90;
                     cGrdOrder.SetWidth(cntWidth);
                 }

             });
         });

    </script>
    <style>
        .padTab > tbody > tr > td {
            padding-right: 15px;
            vertical-align: middle;
        }

            .padTab > tbody > tr > td > label {
                margin-bottom: 0 !important;
            }

            .padTab > tbody > tr > td > .btn {
                margin-top: 0 !important;
            }
        /* #GrdOrder_DXMainTable>tbody>tr {
                position:relative !important;

            }
            #GrdOrder_DXMainTable>tbody>tr:hover .floatedIcons {
                visibility:visible;
            }
            .floatedIcons {
                    position: absolute;
                    left: 20px;
                    transform: translateY(-7px);
                    background: #fff;
                    padding: 0 15px;
                    visibility: hidden;
            }
            .floatedIcons>a {
                margin-right:12px;
                z-index:5;
                -moz-transition:box-shadow 0.2s ease;
                -webkit-transition:box-shadow 0.2s ease;
                transition:box-shadow  0.2s ease, transform 0.3s ease;
                position:relative;
            }
            .floatedIcons>a:before {
                    content: '';
                    width: 35px;
                    height: 35px;
                    position: absolute;
                    border-radius: 50%;
                    background: rgba(247, 170, 170, 0.5);
                    left: -10px;
                    top: -9px;
                    z-index: -1;
                    opacity:0;
                    transform:scale(0);
                    -webkit-transition:all 0.1s ease;
                    -moz-transition:all 0.1s ease;
                    transition:all 0.1s ease;
            }
             .floatedIcons>a:hover:before {
                 transform:scale(1);
                 opacity:1;
             }
            .floatedIcons>a:hover {
                box-shadow:0px 0px 0px 10px rgba(0,0,0,0.4);
                border-radius:30px;
                z-index:7;
                transform:scale(1.3);
                
            }*/
        .closeApprove {
            float: right;
            margin-right: 7px;
        }
           }
           #GrdOrder_DXMainTable>tbody>tr>td:first-child, #GrdOrder_DXHeaderTable>tbody>tr>td:first-child {
               width:40px !important;
           }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <dxe:ASPxPopupControl ID="Popup_OrderStatus" runat="server" ClientInstanceName="cOrderStatus"
        Width="500px" HeaderText="Approvers Configuration" PopupHorizontalAlign="WindowCenter" HeaderStyle-CssClass="wht"
        PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
        ContentStyle-CssClass="pad">
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
                <%--<div style="Width:400px;background-color:#FFFFFF;margin:0px;border:1px solid red;">--%>
                <div class="row">
                    <div class="col-md-12">
                        <div class="col-md-12">
                            <table width="100%">
                                <tr>
                                    <td style="padding-right: 20px">
                                        <label style="margin-bottom: 5px">Proforma</label>
                                    </td>
                                    <td>
                                        <%--<dxe:ASPxTextBox ID="txt_Proforma" MaxLength="80" ClientInstanceName="cProforma" TabIndex="1" 
                                                runat="server" Width="100%"> 
                                            </dxe:ASPxTextBox>--%>
                                        <dxe:ASPxLabel ID="lbl_Proforma" runat="server" ClientInstanceName="cProforma" Text="ASPxLabel"></dxe:ASPxLabel>
                                    </td>
                                    <td style="padding-right: 20px; padding-left: 8px">
                                        <label style="margin-bottom: 5px">Customer</label>
                                    </td>
                                    <td>
                                        <%-- <dxe:ASPxTextBox ID="txt_Customer" ClientInstanceName="cCustomer"  runat="server" MaxLength="100" TabIndex="2"
                                            Width="100%"> 
                                        </dxe:ASPxTextBox>--%>
                                        <dxe:ASPxLabel ID="lbl_Customer" runat="server" ClientInstanceName="cCustomer" Text="ASPxLabel"></dxe:ASPxLabel>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div class="col-md-6">
                        </div>
                        <div class="col-md-6">
                        </div>
                        <div class="clear"></div>
                    </div>
                </div>
                <div class="col-md-12">
                    <table>
                        <tr>
                            <td style="width: 70px; padding: 13px 0;">Status </td>
                            <td>
                                <asp:RadioButtonList ID="rbl_OrderStatus" runat="server" Width="250px" CssClass="mTop5" RepeatDirection="Horizontal">
                                    <asp:ListItem Text="Pending" Value="1" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="Accepted" Value="2"></asp:ListItem>
                                    <asp:ListItem Text="Pending" Value="3"></asp:ListItem>
                                </asp:RadioButtonList></td>
                        </tr>
                    </table>
                </div>
                <div class="clear"></div>
                <div class="col-md-12">

                    <div class="" style="margin-bottom: 5px;">
                        Reason 
                    </div>

                    <div>
                        <dxe:ASPxMemo ID="txt_OrderRemarks" runat="server" ClientInstanceName="cOrderRemarks" Height="71px" Width="100%"></dxe:ASPxMemo>
                    </div>
                </div>

                <div class="col-md-12" style="padding-top: 10px;">
                    <dxe:ASPxButton ID="btn_PrpformaStatus" CausesValidation="true" ClientInstanceName="cbtn_PrpformaStatus" runat="server"
                        AutoPostBack="False" Text="Save" CssClass="btn btn-primary">
                        <ClientSideEvents Click="function (s, e) {SavePrpformaStatus();}" />
                    </dxe:ASPxButton>
                </div>
            </dxe:PopupControlContentControl>
        </ContentCollection>
    </dxe:ASPxPopupControl>
    <div class="panel-heading clearfix">
        <div class="panel-title pull-left">
            <h3>Project Sales Order</h3>
        </div>
        <table class="padTab pull-right" style="margin-top: 7px">
            <tr>
                <td>
                    <label>From </label>
                </td>
                <td style="width: 150px">
                    <dxe:ASPxDateEdit ID="FormDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cFormDate" Width="100%">
                        <ButtonStyle Width="13px">
                        </ButtonStyle>
                    </dxe:ASPxDateEdit>
                </td>
                <td>
                    <label>To </label>
                </td>
                <td style="width: 150px">
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
            <a href="javascript:void(0);" onclick="OnAddButtonClick()" class="btn btn-success btn-radius"><span class="btn-icon"><i class="fa fa-plus"></i></span><span>New</span> </a>
            <% } %>

            <% if (rights.CanExport)
               { %>
            <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-primary btn-radius" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true" OnChange="if(!AvailableExportOption()){return false;}">
                <asp:ListItem Value="0">Export to</asp:ListItem>
                <asp:ListItem Value="1">PDF</asp:ListItem>
                <asp:ListItem Value="2">XLSX</asp:ListItem>
                <asp:ListItem Value="3">RTF</asp:ListItem>
                <asp:ListItem Value="4">CSV</asp:ListItem>
            </asp:DropDownList>
            <% } %>
            <a href="javascript:void(0);" onclick="OpenWaitingOrder()" class="btn btn-warning  typeNotificationBtn btn-radius" style="display: none"><span><u>O</u>rder Waiting </span>
                <span class="typeNotification">
                    <dxe:ASPxLabel runat="server" Text="" ID="lblQuoteweatingCount" ClientInstanceName="ClblQuoteweatingCount"></dxe:ASPxLabel>
                </span>
            </a>
            <%--Sandip Section for Approval Section in Design Start --%>

            <span id="spanStatus" runat="server">
                <a href="javascript:void(0);" onclick="OpenPopUPUserWiseQuotaion()" class="btn btn-primary btn-radius">
                    <span>My Project Order Status</span>
                    <%--<asp:Label ID="Label1" runat="server" Text=""></asp:Label>--%>                   
                </a>
            </span>
            <span id="divPendingWaiting" runat="server">
                <a href="javascript:void(0);" onclick="OpenPopUPApprovalStatus()" class="btn btn-primary btn-radius">
                    <span>Approval Waiting</span>

                    <asp:Label ID="lblWaiting" runat="server" Text=""></asp:Label>
                </a>
                <i class="fa fa-reply blink" style="font-size: 20px; margin-right: 10px;" aria-hidden="true"></i>

            </span>

            <%--Sandip Section for Approval Section in Design End --%>
        </div>
    </div>
    <%--Rev 1.0--%>
    <div id="spnEditLock" runat="server" style="display: none; color: red; text-align: center"></div>
    <div id="spnDeleteLock" runat="server" style="display: none; color: red; text-align: center"></div>
    <%--End of Rev 1.0--%>
    <div class="GridViewArea relative">
        <dxe:ASPxGridView ID="GrdOrder" runat="server" KeyFieldName="SlNo" AutoGenerateColumns="False" DataSourceID="EntityServerModeDataSource" SettingsDataSecurity-AllowEdit="false"
            SettingsDataSecurity-AllowInsert="false" SettingsDataSecurity-AllowDelete="false" OnSummaryDisplayText="Grid_SalesOrder_SummaryDisplayText"
            Width="100%" ClientInstanceName="cGrdOrder" OnCustomCallback="GrdOrder_CustomCallback"
            SettingsBehavior-AllowFocusedRow="true" OnHtmlRowPrepared="AvailableStockgrid_HtmlRowPrepared"
            HorizontalScrollBarMode="Auto" Settings-VerticalScrollableHeight="250" Settings-VerticalScrollBarMode="Auto" SettingsBehavior-ColumnResizeMode="Control">
            <SettingsSearchPanel Visible="True" Delay="5000" />
            <Columns>

                <dxe:GridViewDataTextColumn HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="center" VisibleIndex="0" Width="40px" FixedStyle="Left">
	                <DataItemTemplate>
	                 <img src="/assests/images/attachment.png" style='<%#Eval("IsAttachmentDoc")%>' />
	                </DataItemTemplate>
	                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
	                <CellStyle HorizontalAlign="Center"></CellStyle>

	                <HeaderTemplate><span></span></HeaderTemplate>
	                <EditFormSettings Visible="False"></EditFormSettings>
	                <Settings AllowAutoFilterTextInputTimer="False"/>
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Document No." FieldName="OrderNo"
                    VisibleIndex="1" FixedStyle="Left" Width="140px" Settings-ShowFilterRowMenu="True">

                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Posting Date" FieldName="Order_Date"
                    VisibleIndex="2" Width="150px" Settings-ShowFilterRowMenu="True">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Customer" FieldName="CustomerName"
                    VisibleIndex="3" Width="300px" Settings-ShowFilterRowMenu="True">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Unit" FieldName="BranchName"
                    VisibleIndex="4" Width="250px" Settings-ShowFilterRowMenu="True">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Project Name" FieldName="Proj_Name"
                    VisibleIndex="5" Width="250px" Settings-ShowFilterRowMenu="True" Settings-AllowAutoFilter="True">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="True" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Revision No." FieldName="RevNo"
                    VisibleIndex="22" Width="150px" Settings-ShowFilterRowMenu="True" Settings-AllowAutoFilter="True">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Revision Date" FieldName="RevDate"
                    VisibleIndex="22" Width="150px" Settings-ShowFilterRowMenu="True" Settings-AllowAutoFilter="True">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                </dxe:GridViewDataTextColumn>

                <dxe:GridViewDataTextColumn Caption="Amount" FieldName="Amount"
                    VisibleIndex="7" Width="100" Settings-ShowFilterRowMenu="True">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                </dxe:GridViewDataTextColumn>

                <dxe:GridViewDataTextColumn Caption="Basic Amount" FieldName="BasicAmount"
                    VisibleIndex="6" Width="100" Settings-ShowFilterRowMenu="True">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                </dxe:GridViewDataTextColumn>

                <%--<dxe:GridViewDataTextColumn Caption="Challan No" FieldName="ChallanNo"
                    VisibleIndex="6"  Width="150">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Challan Date" FieldName="Challan_Date"
                    VisibleIndex="7"  Width="150" Settings-ShowFilterRowMenu="True">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                </dxe:GridViewDataTextColumn>--%>

                <%--<dxe:GridViewDataTextColumn Caption="Vehicle No" FieldName="VehicleNos"
                    VisibleIndex="6" Width="120">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                     <Settings AllowAutoFilterTextInputTimer="False" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Vehicle Out Date" FieldName="VehicleOutDateTime"
                    VisibleIndex="7" Width="120">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                     <Settings AllowAutoFilterTextInputTimer="False" />
                </dxe:GridViewDataTextColumn>--%>

                <dxe:GridViewDataTextColumn FieldName="VehicleNos" Caption="Vehicle No" VisibleIndex="8" Width="150px">
                    <DataItemTemplate>
                        <a href="javascript:void(0);" onclick="OnAddEditClick(this,'Show~'+'<%# Eval("Order_Id") %>'+'~'+'<%# Eval("InvoiceNo") %>')" style='<%#Eval("MultipleStatusV")%>'>
                            <dxe:ASPxLabel ID="ASPxLabel1" runat="server" Text='<%# Eval("VehicleNos")%>'
                                ToolTip="Invoice Number">
                            </dxe:ASPxLabel>
                        </a>

                        <a href="javascript:void(0);" onclick="OnAddEditClick(this,'Show~'+'<%# Eval("Order_Id") %>'+'~'+'<%# Eval("InvoiceNo") %>')" style='<%#Eval("SingleStatusV")%>'>
                            <dxe:ASPxLabel ID="ASPxLabel2" runat="server" Text='<%# Eval("VehicleNos")%>'
                                ToolTip="Invoice Number">
                            </dxe:ASPxLabel>
                        </a>

                    </DataItemTemplate>
                    <EditFormSettings Visible="False" />
                    <CellStyle Wrap="False" CssClass="text-center">
                    </CellStyle>
                    <%-- <HeaderTemplate>
                                                                                Status
                                                                            </HeaderTemplate>--%>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AllowAutoFilter="False" />
                    <HeaderStyle Wrap="False" CssClass="text-center" />
                </dxe:GridViewDataTextColumn>


                <dxe:GridViewDataTextColumn FieldName="VehicleOutDateTime" Caption="Vehicle Date" VisibleIndex="9" Width="120px">
                    <DataItemTemplate>
                        <a href="javascript:void(0);" onclick="OnAddEditDateClick(this,'Show~'+'<%# Eval("Order_Id") %>'+'~'+'<%# Eval("InvoiceDate") %>')" style='<%#Eval("MultipleStatusV")%>'>
                            <dxe:ASPxLabel ID="ASPxLabel3" runat="server" Text='<%# Eval("VehicleOutDateTime")%>'
                                ToolTip="Invoice Date">
                            </dxe:ASPxLabel>
                        </a>

                        <a href="javascript:void(0);" onclick="OnAddEditDateClick(this,'Show~'+'<%# Eval("Order_Id") %>'+'~'+'<%# Eval("InvoiceDate") %>')" style='<%#Eval("SingleStatusV")%>'>
                            <dxe:ASPxLabel ID="ASPxLabel4" runat="server" Text='<%# Eval("VehicleOutDateTime")%>'
                                ToolTip="Invoice Date">
                            </dxe:ASPxLabel>
                        </a>
                    </DataItemTemplate>
                    <EditFormSettings Visible="False" />
                    <CellStyle Wrap="False" CssClass="text-center">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AllowAutoFilter="False" />
                    <HeaderStyle Wrap="False" CssClass="text-center" />
                </dxe:GridViewDataTextColumn>




                <dxe:GridViewDataTextColumn FieldName="InvoiceNo" Caption="Invoice No." VisibleIndex="10" Width="150px">
                    <DataItemTemplate>
                        <a href="javascript:void(0);" onclick="OnAddEditClick(this,'Show~'+'<%# Eval("Order_Id") %>'+'~'+'<%# Eval("InvoiceNo") %>')" style='<%#Eval("MultipleStatus")%>'>
                            <dxe:ASPxLabel ID="ASPxTextBox2" runat="server" Text='<%# Eval("InvoiceNo")%>'
                                ToolTip="Invoice Number">
                            </dxe:ASPxLabel>
                        </a>

                        <a href="javascript:void(0);" onclick="OnAddEditClick(this,'Show~'+'<%# Eval("Order_Id") %>'+'~'+'<%# Eval("InvoiceNo") %>')" style='<%#Eval("SingleStatus")%>'>
                            <dxe:ASPxLabel ID="ASPxTextBox3" runat="server" Text='<%# Eval("InvoiceNo")%>'
                                ToolTip="Invoice Number">
                            </dxe:ASPxLabel>
                        </a>

                        <%--  <asp:Label ID="lblSN" runat="server" Text='<%# Eval("InvoiceNo") %>' style='<%#Eval("SingleStatus")%>'></asp:Label>--%>
                    </DataItemTemplate>
                    <EditFormSettings Visible="False" />
                    <CellStyle Wrap="False" CssClass="text-center">
                    </CellStyle>
                    <%-- <HeaderTemplate>
                                                                                Status
                                                                            </HeaderTemplate>--%>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AllowAutoFilter="False" />
                    <HeaderStyle Wrap="False" CssClass="text-center" />
                </dxe:GridViewDataTextColumn>


                <dxe:GridViewDataTextColumn FieldName="InvoiceDate" Caption="Invoice Date" VisibleIndex="11" Width="80px">
                    <DataItemTemplate>
                        <a href="javascript:void(0);" onclick="OnAddEditDateClick(this,'Show~'+'<%# Eval("Order_Id") %>'+'~'+'<%# Eval("InvoiceDate") %>')" style='<%#Eval("MultipleStatus")%>'>
                            <dxe:ASPxLabel ID="ASPxTextBox2" runat="server" Text='<%# Eval("InvoiceDate")%>'
                                ToolTip="Invoice Date">
                            </dxe:ASPxLabel>
                        </a>

                        <a href="javascript:void(0);" onclick="OnAddEditDateClick(this,'Show~'+'<%# Eval("Order_Id") %>'+'~'+'<%# Eval("InvoiceDate") %>')" style='<%#Eval("SingleStatus")%>'>
                            <dxe:ASPxLabel ID="ASPxTextBox3" runat="server" Text='<%# Eval("InvoiceDate")%>'
                                ToolTip="Invoice Date">
                            </dxe:ASPxLabel>
                        </a>
                    </DataItemTemplate>
                    <EditFormSettings Visible="False" />
                    <CellStyle Wrap="False" CssClass="text-center">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AllowAutoFilter="False" />
                    <HeaderStyle Wrap="False" CssClass="text-center" />
                </dxe:GridViewDataTextColumn>



                <dxe:GridViewDataTextColumn Caption="Invoice Net Amount" FieldName="InvoiceNetAmount"
                    VisibleIndex="12" Width="150">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                </dxe:GridViewDataTextColumn>


                <dxe:GridViewDataTextColumn Caption="Invoice Amount Received" FieldName="InvoiceAmountReceived"
                    VisibleIndex="13" Width="150">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                </dxe:GridViewDataTextColumn>


                <dxe:GridViewDataTextColumn Caption="Invoice Balance Amount" FieldName="InvoiceBalanceAmount"
                    VisibleIndex="14" Width="150">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Place of Supply[GST]" FieldName="PosState"
                    VisibleIndex="15" Width="150">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Entered By" FieldName="EnteredBy"
                    VisibleIndex="16" Width="80">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Last Modified On" FieldName="LastModifiedOn"
                    VisibleIndex="17" Width="70">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Updated By" FieldName="UpdatedBy"
                    VisibleIndex="18" Width="80">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Created From" FieldName="CreatedFrom"
                    VisibleIndex="19" Width="120">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Status" FieldName="Doc_status"
                    VisibleIndex="20" Width="120">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                </dxe:GridViewDataTextColumn>
                <%--<dxe:GridViewDataTextColumn Caption="Transit Sales Invoice" FieldName="Transit_Invoice"
                    VisibleIndex="18" Width="120">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                     <Settings AllowAutoFilterTextInputTimer="False" />
                </dxe:GridViewDataTextColumn>--%>

                <dxe:GridViewDataTextColumn Caption="Bal Quantity" FieldName="BalQty"
                    VisibleIndex="21" Width="0">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                </dxe:GridViewDataTextColumn>


                <dxe:GridViewDataTextColumn Caption="Valid From" FieldName="ProjectValidFrom"
                    VisibleIndex="22" Width="150px" Settings-ShowFilterRowMenu="True">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Valid Up To" FieldName="ProjectValidUpto"
                    VisibleIndex="23" Width="150px" Settings-ShowFilterRowMenu="True">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Approval Status" FieldName="Project_ApproveStatus"
                    VisibleIndex="24" Width="150px" Settings-ShowFilterRowMenu="True" Settings-AllowAutoFilter="True">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                </dxe:GridViewDataTextColumn>

                <%--Rev Sayantani--%>
                <%-- <dxe:GridViewDataTextColumn Caption="Status" FieldName="Status"
                    VisibleIndex="111" Visible="false">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                     <Settings AllowAutoFilterTextInputTimer="False" />
                </dxe:GridViewDataTextColumn>--%>
                <dxe:GridViewDataTextColumn Caption="Status" FieldName="Status"
                    VisibleIndex="25" Visible="false" ShowInCustomizationForm="false">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                </dxe:GridViewDataTextColumn>
                <%--Rev Sayantani--%>
                <%-- Rev Sayantani--%>
                <%--<dxe:GridViewDataTextColumn Caption="IsCancel" FieldName="IsCancel"
                    VisibleIndex="112" Visible="false">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                     <Settings AllowAutoFilterTextInputTimer="False" />
                </dxe:GridViewDataTextColumn>--%>
                <dxe:GridViewDataTextColumn Caption="IsCancel" FieldName="IsCancel"
                    VisibleIndex="26" Visible="false" ShowInCustomizationForm="false">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                </dxe:GridViewDataTextColumn>
                <%-- End of Rev Sayantani--%>
                <%-- Rev Sayantani--%>
                <%-- <dxe:GridViewDataTextColumn Caption="IsClosed" FieldName="IsClosed"
                    VisibleIndex="113" Visible="false">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                     <Settings AllowAutoFilterTextInputTimer="False" />
                </dxe:GridViewDataTextColumn>--%>
                <dxe:GridViewDataTextColumn Caption="IsClosed" FieldName="IsClosed"
                    VisibleIndex="27" Visible="false" ShowInCustomizationForm="false">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                </dxe:GridViewDataTextColumn>


                <dxe:GridViewDataTextColumn Caption="LastEntryValue" FieldName="LastEntryValue"
                    VisibleIndex="28" Width="0px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                </dxe:GridViewDataTextColumn>

                <%--  End of Rev Sayantani --%>
                <dxe:GridViewDataTextColumn HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="center" VisibleIndex="29" Width="0">
                    <DataItemTemplate>
                        <div class="floatedIcons">
                            <div class='floatedBtnArea'>
                                <% if (rights.CanView)
                                   { %>
                                <a href="javascript:void(0);" onclick="OnViewClick('<%#Eval("Order_Id")%>')" class="" title="">
                                    <span class='ico ColorSix'><i class='fa fa-eye'></i></span><span class='hidden-xs'>View</span></a>
                                <% } %>
                                <% if (rights.CanEdit)
                                   { 
                                %>

                                <%--Rev 1.0 [style='<%#Eval("Editlock")%>' and <%#Eval("Deletelock")%> added for Edit and Delete respectively ] --%>
                                <a href="javascript:void(0);" onclick="OnMoreInfoClick('<%#Eval("Order_Id")%>',<%# Container.VisibleIndex %>)" class="" style='<%#Eval("isLastEntry")%>; <%#Eval("Editlock")%>' title="">
                                    <span class='ico editColor'><i class='fa fa-pencil' aria-hidden='true'></i></span><span class='hidden-xs'>Modify</span></a>  <% } %>
                                <% if (rights.CanDelete)
                                   { %>
                                <a href="javascript:void(0);" onclick="OnClickDelete('<%#Eval("Order_Id")%>',<%# Container.VisibleIndex %>)" class="" style='<%#Eval("isLastEntry")%>; <%#Eval("Deletelock")%>' title="">
                                    <span class='ico deleteColor'><i class='fa fa-trash' aria-hidden='true'></i></span><span class='hidden-xs'>Delete</span></a>
                                <% } %>


                                <% if (rights.CanApproved && isApprove)
                                   { %>
                                <a href="javascript:void(0);" onclick="OnApproveClick('<%#Eval("Order_Id")%>',<%# Container.VisibleIndex %>)" class="" title="">

                                    <span class='ico editColor'><i class='fa fa-check' aria-hidden='true'></i></span><span class='hidden-xs'>Approve/Reject</span>

                                </a><% } %>




                                <% if (rights.CanCancel)
                                   { %>
                                <a href="javascript:void(0);" onclick="OnCancelClick('<%#Eval("Order_Id")%>',<%# Container.VisibleIndex %>)" class="" title="" style='<%#Eval("isLastEntry")%>'>

                                    <span class='ico deleteColor'><i class='fa fa-times' aria-hidden='true'></i></span><span class='hidden-xs'>Cancel Order</span>

                                </a><% } %>

                                <% if (rights.CanClose)
                                   { %>
                                <a href="javascript:void(0);" onclick="OnClosedClick('<%#Eval("Order_Id")%>',<%# Container.VisibleIndex %>)" class="" title="" style='<%#Eval("isLastEntry")%>'>

                                    <span class='ico deleteColor'><i class='fa fa-times' aria-hidden='true'></i></span><span class='hidden-xs'>Closed Order</span>

                                </a><% } %>

                                <a href="javascript:void(0);" onclick="OnClickCopy('<%#Eval("Order_Id")%>')" class="" title=" " style="display: none">
                                    <span class='ico ColorFour'><i class='fa fa-files-o'></i></span><span class='hidden-xs'>Copy</span>
                                </a>
                                <a href="javascript:void(0);" onclick="OnClickStatus('<%#Eval("Order_Id")%>')" class="" title="" style="display: none">
                                    <span class='ico ColorFive'><i class='fa fa-check'></i></span><span class='hidden-xs'>Status</span></a>
                                <% if (rights.CanAddUpdateDocuments)
                                   { %>
                                <a href="javascript:void(0);" onclick="OnclickViewAttachment('<%#Eval("Order_Id")%>')" class="" title="">
                                    <span class='ico ColorSix'><i class='fa fa-paperclip'></i></span><span class='hidden-xs'>Add/View Attachment</span>
                                </a>
                                <% } %>
                                <% if (rights.CanPrint)
                                   { %>
                                <a href="javascript:void(0);" onclick="onPrintJv('<%#Eval("Order_Id")%>')" class="" title="">
                                    <span class='ico ColorSeven'><i class='fa fa-print'></i></span><span class='hidden-xs'>Print</span>
                                </a><%} %>

                                <% if (rights.CanClose)
                                       { %>
                                    <% if (ShowProductWiseClose.ToUpper() == "1")
                                       {%>
                                    <a href="javascript:void(0);" onclick="OnProductWiseClosedClick('<%#Eval("Order_Id")%>','<%# Container.VisibleIndex %>','<%#Eval("OrderNo") %>')" class="" title="">
                                        <span class='ico deleteColor'><i class='fa fa-times' aria-hidden='true'></i></span><span class='hidden-xs'>Product Wise Closed</span></a>
                                    <% } %>
                                    <% } %>
                            </div>
                        </div>
                    </DataItemTemplate>
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    <CellStyle HorizontalAlign="Center"></CellStyle>
                    <HeaderTemplate><span>Actions</span></HeaderTemplate>
                    <EditFormSettings Visible="False"></EditFormSettings>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                </dxe:GridViewDataTextColumn>
            </Columns>
            <%-- --Rev Sayantani--%>
            <SettingsBehavior ConfirmDelete="true" EnableCustomizationWindow="true" EnableRowHotTrack="true" />
            <SettingsCookies Enabled="true" StorePaging="true" Version="4.3" />
            <%-- -- End of Rev Sayantani --%>
            <SettingsContextMenu Enabled="true"></SettingsContextMenu>
            <ClientSideEvents EndCallback="function (s, e) {grid_EndCallBack();}" RowClick="gridRowclick" />
            <SettingsPager NumericButtonCount="10" PageSize="10" ShowSeparators="True" Mode="ShowPager">
                <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
                <FirstPageButton Visible="True">
                </FirstPageButton>
                <LastPageButton Visible="True">
                </LastPageButton>
            </SettingsPager>
            <%--<SettingsSearchPanel Visible="True" />--%>
            <Settings ShowGroupPanel="True" ShowFooter="true" ShowGroupFooter="VisibleIfExpanded" ShowStatusBar="Hidden" ShowHorizontalScrollBar="true" ShowFilterRow="true" ShowFilterRowMenu="false" />
            <SettingsLoadingPanel Text="Please Wait..." />

            <TotalSummary>
                <dxe:ASPxSummaryItem FieldName="BasicAmount" SummaryType="Custom" Tag="TotalBasicAmount" />
                <dxe:ASPxSummaryItem FieldName="Amount" SummaryType="Custom" Tag="TotalValueInBaseCurrency" />
            </TotalSummary>
        </dxe:ASPxGridView>
        <asp:HiddenField ID="hiddenedit" runat="server" />
        <dx:LinqServerModeDataSource ID="EntityServerModeDataSource" runat="server" OnSelecting="EntityServerModeDataSource_Selecting"
            ContextTypeName="ERPDataClassesDataContext" TableName="v_GetSalesOrderEntityList" />
    </div>
    <div style="display: none">
        <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
        </dxe:ASPxGridViewExporter>
    </div>

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

                        <table style="width: 94%">

                            <tr>
                                <td>Reason<span style="color: red">*</span></td>
                                <td class="relative">
                                    <dxe:ASPxMemo ID="txtInstFeedback" runat="server" Width="100%" Height="50px" ClientInstanceName="txtFeedback"></dxe:ASPxMemo>
                                    <span id="MandatoryRemarksFeedback" style="display: none">
                                        <img id="gridHistory_DXPEForm_efnew_DXEFL_DXEditor1234_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></span>
                                </td>
                            </tr>

                            <tr>
                                <td></td>
                                <td colspan="2" style="padding-top: 10px;">
                                    <input id="btnFeedbackSave" class="btn btn-primary" onclick="CallFeedback_save()" type="button" value="Save" />&nbsp;&nbsp;
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
                        <table style="width: 94%">
                            <tr>
                                <td>Reason<span style="color: red">*</span></td>
                                <td class="relative">
                                    <dxe:ASPxMemo ID="ASPxMemo1" runat="server" Width="100%" Height="50px" ClientInstanceName="txtClosed"></dxe:ASPxMemo>
                                    <span id="MandatoryRemarksFeedback1" style="display: none">
                                        <img id="gridHistory_DXPEForm_efnew_DXEFL_DXEditor1234_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></span>
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td colspan="2" style="padding-top: 10px;">
                                    <input id="btnClosedSave" class="btn btn-primary" onclick="CallClosed_save()" type="button" value="Save" />&nbsp;&nbsp;
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

        <dxe:ASPxPopupControl ID="popup_OrderWait" runat="server" Width="1200"
            CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="Cpopup_OrderWait"
            HeaderText="Order Waiting" AllowResize="false" ResizingMode="Postponed">
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">

                    <div onkeypress="OnWaitingGridKeyPress(event)">

                        <dxe:ASPxGridView ID="watingOrdergrid" runat="server" KeyFieldName="SBMain_Id" AutoGenerateColumns="False"
                            Width="100%" ClientInstanceName="CwatingOrdergrid" OnCustomCallback="watingOrdergrid_CustomCallback" KeyboardSupport="true"
                            SettingsBehavior-AllowSelectSingleRowOnly="true" OnDataBinding="watingOrdergrid_DataBinding" SettingsBehavior-AllowFocusedRow="true"
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
                                        <a href="javascript:void(0);" onclick="RemoveQuote('<%# Container.KeyValue %>')" class="pad" title="Remove">
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

                            <ClientSideEvents RowClick="ListRowClicked" EndCallback="watingOrdergridEndCallback" />

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


                    <dxe:ASPxButton ID="OrderWattingOk" runat="server" AutoPostBack="false" Text="O&#818;k" CssClass="btn btn-primary okClass"
                        ClientSideEvents-Click="OrderWattingOkClick" UseSubmitBehavior="False" />
                </dxe:PopupControlContentControl>
            </ContentCollection>
        </dxe:ASPxPopupControl>

    </div>
    <dxe:ASPxTimer runat="server" Interval="10000" ClientInstanceName="Timer1">
        <ClientSideEvents Tick="timerTick" />
    </dxe:ASPxTimer>
    <asp:HiddenField ID="waitingOrderCount" runat="server" />
    <div class="PopUpArea">
        <%-- <button class="btn btn-primary" onclick="ApproveAll();">Approve Selection</button>
        <button class="btn btn-primary" onclick="RejectAll();">Reject Selection</button>--%>

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
                            <dxe:ASPxGridView ID="gridPendingApproval" runat="server" KeyFieldName="ID" AutoGenerateColumns="False" OnPageIndexChanged="gridPendingApproval_PageIndexChanged"
                                Width="100%" ClientInstanceName="cgridPendingApproval" OnCustomCallback="gridPendingApproval_CustomCallback"
                                SettingsBehavior-ColumnResizeMode="Control">
                                <Columns>
                                    <%-- <dxe:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Width="60" Caption=" " />--%>
                                    <dxe:GridViewDataTextColumn Caption="Document No." FieldName="Number"
                                        VisibleIndex="0" FixedStyle="Left">
                                        <CellStyle CssClass="gridcellleft" Wrap="true">
                                        </CellStyle>
                                    </dxe:GridViewDataTextColumn>

                                    <dxe:GridViewDataTextColumn Caption="Party Name" FieldName="customer"
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
                                <SettingsSearchPanel Visible="True" />
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
        <dxe:ASPxPopupControl ID="ASPXPopupControl" runat="server" ClientInstanceName="popup"
            CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" Height="630px"
            Width="1200px" HeaderText="Quotation Approval" Modal="true" AllowResize="true" ResizingMode="Postponed">
            <HeaderTemplate>
                <span>User Approval</span>
                <div class="closeApprove" onclick="closeUserApproval();"><i class="fa fa-close"></i></div>
            </HeaderTemplate>
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">
                </dxe:PopupControlContentControl>
            </ContentCollection>
        </dxe:ASPxPopupControl>
        <dxe:ASPxPopupControl ID="PopupUserWiseQuotation" runat="server" ClientInstanceName="cPopupUserWiseQuotation"
            Width="900px" HeaderText="User Wise Project Order Status" PopupHorizontalAlign="WindowCenter" HeaderStyle-CssClass="wht"
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
                                    <dxe:GridViewDataTextColumn Caption="Branch" FieldName="Branch"
                                        VisibleIndex="0" FixedStyle="Left">
                                        <CellStyle CssClass="gridcellleft" Wrap="true">
                                        </CellStyle>
                                        <Settings AllowAutoFilterTextInputTimer="true" />
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn Caption="Sale Order No." FieldName="number"
                                        VisibleIndex="1" FixedStyle="Left">
                                        <CellStyle CssClass="gridcellleft" Wrap="true">
                                        </CellStyle>
                                        <Settings AllowAutoFilterTextInputTimer="true" />
                                    </dxe:GridViewDataTextColumn>

                                    <dxe:GridViewDataTextColumn Caption="Date" FieldName="OrderedDate"
                                        VisibleIndex="2" FixedStyle="Left">
                                        <CellStyle CssClass="gridcellleft" Wrap="true">
                                        </CellStyle>
                                        <Settings AllowAutoFilterTextInputTimer="true" />
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn Caption="Customer" FieldName="Customer"
                                        VisibleIndex="2" FixedStyle="Left">
                                        <CellStyle CssClass="gridcellleft" Wrap="true">
                                        </CellStyle>
                                        <Settings AllowAutoFilterTextInputTimer="true" />
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn Caption="Approval User" FieldName="approvedby"
                                        VisibleIndex="3" FixedStyle="Left">
                                        <CellStyle CssClass="gridcellleft" Wrap="true">
                                        </CellStyle>
                                        <Settings AllowAutoFilterTextInputTimer="false" />
                                    </dxe:GridViewDataTextColumn>

                                    <dxe:GridViewDataTextColumn Caption="User Level" FieldName="UserLevel"
                                        VisibleIndex="4" FixedStyle="Left">
                                        <CellStyle CssClass="gridcellleft" Wrap="true">
                                        </CellStyle>
                                    </dxe:GridViewDataTextColumn>

                                    <dxe:GridViewDataTextColumn Caption="Status" FieldName="status"
                                        VisibleIndex="5" FixedStyle="Left">
                                        <CellStyle CssClass="gridcellleft" Wrap="true">
                                        </CellStyle>
                                        <Settings AllowAutoFilterTextInputTimer="true" />
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
                                <SettingsSearchPanel Visible="True" />
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

    <%--Kaushik--%>

    <div class="PopUpArea">
        <dxe:ASPxPopupControl ID="ASPxInvoiceDocumentsPopup" runat="server" ClientInstanceName="cInvoiceDocumentsPopup"
            Width="350px" HeaderText="Select Design(s)" PopupHorizontalAlign="WindowCenter"
            PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
            Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True">
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">

                    <dxe:ASPxCallbackPanel runat="server" ID="SelectInvoicePanel" ClientInstanceName="cInvoiceSelectPanel" OnCallback="SelectInvoicePanel_Callback" ClientSideEvents-EndCallback="cInvoiceSelectPanelEndCall">
                        <PanelCollection>
                            <dxe:PanelContent runat="server">

                                <dxe:ASPxCheckBox ID="selectOriginal" Text="Original For Recipient" runat="server" ToolTip="Select Original"
                                    ClientInstanceName="CselectOriginal">
                                </dxe:ASPxCheckBox>
                                <dxe:ASPxCheckBox ID="selectDuplicate" Text="Duplicate For Transporter" runat="server" ToolTip="Select Duplicate"
                                    ClientInstanceName="CselectDuplicate">
                                </dxe:ASPxCheckBox>
                                <dxe:ASPxCheckBox ID="selectTriplicate" Text="Triplicate For Supplier" runat="server" ToolTip="Select Triplicate"
                                    ClientInstanceName="CselectTriplicate">
                                </dxe:ASPxCheckBox>
                                <dxe:ASPxCheckBox ID="selectOfficecopy" Text="Extra/Office Copy" runat="server" ToolTip="Select Office Copy"
                                    ClientInstanceName="CselectOfficecopy">
                                </dxe:ASPxCheckBox>

                                <dxe:ASPxComboBox ID="CmbInvoiceDesignName" ClientInstanceName="cInvoiceCmbDesignName" runat="server" ValueType="System.String" Width="100%" EnableSynchronization="True">
                                </dxe:ASPxComboBox>

                                <div class="text-center pTop10">
                                    <dxe:ASPxButton ID="btnInvoiceOK" ClientInstanceName="cInvoicebtnOK" runat="server" AutoPostBack="False" Text="OK" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                                        <ClientSideEvents Click="function(s, e) {return PerformInvoiceCallToGridBind();}" />
                                    </dxe:ASPxButton>

                                </div>




                            </dxe:PanelContent>
                        </PanelCollection>
                    </dxe:ASPxCallbackPanel>



                </dxe:PopupControlContentControl>

            </ContentCollection>
        </dxe:ASPxPopupControl>
    </div>
    <%--Kaushik--%>

    <dxe:ASPxGlobalEvents ID="GlobalEvents" runat="server">
        <ClientSideEvents ControlsInitialized="AllControlInitilize" />
    </dxe:ASPxGlobalEvents>


    <dxe:ASPxPopupControl ID="InvoiceNumberpopup" ClientInstanceName="cInvoiceNumberpopup" runat="server"
        AllowDragging="False" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" HeaderText="Invoice Number Detail"
        EnableHotTrack="False" BackColor="#DDECFE" Width="850px" CloseAction="CloseButton">
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
                <dxe:ASPxCallbackPanel ID="InvoiceNumberpanel" runat="server" Width="650px" ClientInstanceName="popInvoiceNumberPanel"
                    OnCallback="InvoiceNumberpanel_Callback">
                    <PanelCollection>
                        <dxe:PanelContent runat="server">
                            <div>
                                <dxe:ASPxGridView ID="grdInvoiceNumber" runat="server" KeyFieldName="id" AutoGenerateColumns="False"
                                    Width="100%" ClientInstanceName="cpbInvoiceNumber" OnDataBinding="InvoiceNumberpanel_DataBinding">
                                    <Columns>
                                        <dxe:GridViewDataTextColumn Caption="Invoice Number" FieldName="Invoice_Number" HeaderStyle-CssClass="text-left"
                                            VisibleIndex="0" FixedStyle="Left" Width="100px">
                                            <CellStyle CssClass="text-left" Wrap="true">
                                            </CellStyle>
                                            <Settings AutoFilterCondition="Contains" />
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="Invoice Date" FieldName="Invoice_Date" HeaderStyle-CssClass="text-center"
                                            VisibleIndex="1" FixedStyle="Left" Width="100px">
                                            <CellStyle CssClass="text-center" Wrap="true">
                                            </CellStyle>
                                            <Settings AutoFilterCondition="Contains" />
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="Invoice Net Amt" FieldName="NetAmount" HeaderStyle-CssClass="right"
                                            VisibleIndex="2" FixedStyle="Left" Width="100px">
                                            <CellStyle CssClass="text-center" Wrap="true">
                                            </CellStyle>
                                            <Settings AutoFilterCondition="Contains" />
                                        </dxe:GridViewDataTextColumn>

                                        <dxe:GridViewDataTextColumn Caption="Invoice Amt Received" FieldName="AmountReceived" HeaderStyle-CssClass="right"
                                            VisibleIndex="3" FixedStyle="Left" Width="100px">
                                            <CellStyle CssClass="text-center" Wrap="true">
                                            </CellStyle>
                                            <Settings AutoFilterCondition="Contains" />
                                        </dxe:GridViewDataTextColumn>

                                        <dxe:GridViewDataTextColumn Caption="Invoice Balance Amt" FieldName="BalanceAmount" HeaderStyle-CssClass="right"
                                            VisibleIndex="4" FixedStyle="Left" Width="100px">
                                            <CellStyle CssClass="text-center" Wrap="true">
                                            </CellStyle>
                                            <Settings AutoFilterCondition="Contains" />
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="Vehicle No" FieldName="VehicleNos" HeaderStyle-CssClass="right"
                                            VisibleIndex="5" FixedStyle="Left" Width="100px">
                                            <CellStyle CssClass="text-center" Wrap="true">
                                            </CellStyle>
                                            <Settings AutoFilterCondition="Contains" />
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="Vehicle Out Date" FieldName="VehicleOutDateTime" HeaderStyle-CssClass="right"
                                            VisibleIndex="6" FixedStyle="Left" Width="100px">
                                            <CellStyle CssClass="text-center" Wrap="true">
                                            </CellStyle>
                                            <Settings AutoFilterCondition="Contains" />
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="Type" FieldName="Type" HeaderStyle-CssClass="right"
                                            VisibleIndex="7" FixedStyle="Left" Width="100px">
                                            <CellStyle CssClass="text-center" Wrap="true">
                                            </CellStyle>
                                            <Settings AutoFilterCondition="Contains" />
                                        </dxe:GridViewDataTextColumn>


                                        <dxe:GridViewDataTextColumn Caption="Print" HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="center" VisibleIndex="8" Width="240px">
                                            <DataItemTemplate>

                                                <a href="javascript:void(0);" onclick="onPrintSi('<%#Eval("Invoice_Id")%>','<%#Eval("Type")%>')" class="pad" title="print">
                                                    <img src="../../../assests/images/Print.png" />
                                                </a>
                                            </DataItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                            <CellStyle HorizontalAlign="Center"></CellStyle>
                                            <HeaderTemplate><span>Print</span></HeaderTemplate>
                                            <EditFormSettings Visible="False"></EditFormSettings>
                                        </dxe:GridViewDataTextColumn>
                                    </Columns>
                                </dxe:ASPxGridView>
                            </div>
                        </dxe:PanelContent>
                    </PanelCollection>
                </dxe:ASPxCallbackPanel>
            </dxe:PopupControlContentControl>
        </ContentCollection>
        <HeaderStyle HorizontalAlign="Left">
            <Paddings PaddingRight="6px" />
        </HeaderStyle>
        <SizeGripImage Height="16px" Width="16px" />
        <CloseButtonImage Height="12px" Width="13px" />
        <ClientSideEvents CloseButtonClick="function(s, e) {
popup.Hide();
}" />
    </dxe:ASPxPopupControl>

    <dxe:ASPxPopupControl ID="InvoiceDatepopup" ClientInstanceName="cInvoiceDatepopup" runat="server"
        AllowDragging="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" HeaderText="Invoice Date Detail"
        EnableHotTrack="False" BackColor="#DDECFE" Width="850px" CloseAction="CloseButton">
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
                <dxe:ASPxCallbackPanel ID="InvoiceDatepanel" runat="server" Width="650px" ClientInstanceName="popInvoiceDatePanel"
                    OnCallback="InvoiceDatepanel_Callback">
                    <PanelCollection>
                        <dxe:PanelContent runat="server">
                            <div>
                                <dxe:ASPxGridView ID="grdInvoiceDate" runat="server" KeyFieldName="id" AutoGenerateColumns="False"
                                    Width="100%" ClientInstanceName="cpbInvoiceDate" OnDataBinding="InvoiceDatepanel_DataBinding">
                                    <Columns>
                                        <dxe:GridViewDataTextColumn Caption="Invoice Number" FieldName="Invoice_Number" HeaderStyle-CssClass="text-left"
                                            VisibleIndex="0" FixedStyle="Left" Width="100px">
                                            <CellStyle CssClass="text-left" Wrap="true">
                                            </CellStyle>
                                            <Settings AutoFilterCondition="Contains" />
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="Invoice Date" FieldName="Invoice_Date" HeaderStyle-CssClass="text-center"
                                            VisibleIndex="1" FixedStyle="Left" Width="100px">
                                            <CellStyle CssClass="text-center" Wrap="true">
                                            </CellStyle>
                                            <Settings AutoFilterCondition="Contains" />
                                        </dxe:GridViewDataTextColumn>

                                        <dxe:GridViewDataTextColumn Caption="Invoice Net Amt" FieldName="NetAmount" HeaderStyle-CssClass="right"
                                            VisibleIndex="2" FixedStyle="Left" Width="100px">
                                            <CellStyle CssClass="text-center" Wrap="true">
                                            </CellStyle>
                                            <Settings AutoFilterCondition="Contains" />
                                        </dxe:GridViewDataTextColumn>

                                        <dxe:GridViewDataTextColumn Caption="Invoice Amt Received" FieldName="AmountReceived" HeaderStyle-CssClass="right"
                                            VisibleIndex="3" FixedStyle="Left" Width="100px">
                                            <CellStyle CssClass="text-center" Wrap="true">
                                            </CellStyle>
                                            <Settings AutoFilterCondition="Contains" />
                                        </dxe:GridViewDataTextColumn>

                                        <dxe:GridViewDataTextColumn Caption="Invoice Balance Amt" FieldName="BalanceAmount" HeaderStyle-CssClass="right"
                                            VisibleIndex="4" FixedStyle="Left" Width="100px">
                                            <CellStyle CssClass="text-center" Wrap="true">
                                            </CellStyle>
                                            <Settings AutoFilterCondition="Contains" />
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="Vehicle No" FieldName="VehicleNos" HeaderStyle-CssClass="right"
                                            VisibleIndex="5" FixedStyle="Left" Width="100px">
                                            <CellStyle CssClass="text-center" Wrap="true">
                                            </CellStyle>
                                            <Settings AutoFilterCondition="Contains" />
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="Vehicle Out Date" FieldName="VehicleOutDateTime" HeaderStyle-CssClass="right"
                                            VisibleIndex="6" FixedStyle="Left" Width="100px">
                                            <CellStyle CssClass="text-center" Wrap="true">
                                            </CellStyle>
                                            <Settings AutoFilterCondition="Contains" />
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="Type" FieldName="Type" HeaderStyle-CssClass="right"
                                            VisibleIndex="7" FixedStyle="Left" Width="100px">
                                            <CellStyle CssClass="text-center" Wrap="true">
                                            </CellStyle>
                                            <Settings AutoFilterCondition="Contains" />
                                        </dxe:GridViewDataTextColumn>

                                        <dxe:GridViewDataTextColumn Caption="Print" HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="center" VisibleIndex="8" Width="240px">
                                            <DataItemTemplate>

                                                <a href="javascript:void(0);" onclick="onPrintSi('<%#Eval("Invoice_Id")%>','<%#Eval("Type")%>')" class="pad" title="print">
                                                    <img src="../../../assests/images/Print.png" />
                                                </a>
                                            </DataItemTemplate>
                                        </dxe:GridViewDataTextColumn>
                                    </Columns>
                                </dxe:ASPxGridView>
                            </div>
                        </dxe:PanelContent>
                    </PanelCollection>
                </dxe:ASPxCallbackPanel>
            </dxe:PopupControlContentControl>
        </ContentCollection>
        <HeaderStyle HorizontalAlign="Left">
            <Paddings PaddingRight="6px" />
        </HeaderStyle>
        <SizeGripImage Height="16px" Width="16px" />
        <CloseButtonImage Height="12px" Width="13px" />
        <ClientSideEvents CloseButtonClick="function(s, e) {
popup.Hide();
}" />
    </dxe:ASPxPopupControl>

    <div>
        <asp:HiddenField ID="hfIsFilter" runat="server" />
        <asp:HiddenField ID="hfFromDate" runat="server" />
        <asp:HiddenField ID="hfToDate" runat="server" />
        <asp:HiddenField ID="hfBranchID" runat="server" />
        <asp:HiddenField ID="hdnIsUserwiseFilter" runat="server" />
        <asp:HiddenField ID="hddnKeyValue" runat="server" />
        <asp:HiddenField ID="hddnCancelCloseFlag" runat="server" />
         <asp:HiddenField ID="hdnIsMultiuserApprovalRequired" runat="server" />
        <%--Rev Sanchita--%>
        <asp:HiddenField ID="hdnLockFromDateedit" runat="server" />
        <asp:HiddenField ID="hdnLockToDateedit" runat="server" />

        <asp:HiddenField ID="hdnLockFromDatedelete" runat="server" />
        <asp:HiddenField ID="hdnLockToDatedelete" runat="server" />
        <%--End of Rev Sanchita--%>
    </div>
     <dxe:ASPxPopupControl ID="PopupProductwiseClose" runat="server" ClientInstanceName="cPopupProductwiseClose"
        Width="900px" HeaderText="Product wise - Close" PopupHorizontalAlign="WindowCenter" HeaderStyle-CssClass="wht"
        PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
        ContentStyle-CssClass="pad">
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
                <div class="row">
                    <div class="col-md-4">
                        <dxe:ASPxLabel ID="lbl_PIQuoteNo" runat="server" Text="Document No.">
                        </dxe:ASPxLabel>
                        <asp:TextBox ID="txtVoucherNo" runat="server" Width="100%" MaxLength="50" ReadOnly="true">                             
                        </asp:TextBox>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <dxe:ASPxGridView ID="gridProductwiseClose" runat="server" KeyFieldName="OrderDetails_Id" AutoGenerateColumns="False"
                            Width="100%" ClientInstanceName="cgridProductwiseClose" OnCustomCallback="gridProductwiseClose_CustomCallback"
                            OnRowInserting="gridProductwiseClose_RowInserting" OnRowUpdating="gridProductwiseClose_RowUpdating" OnRowDeleting="gridProductwiseClose_RowDeleting" Settings-VerticalScrollableHeight="400" Settings-VerticalScrollBarMode="Visible">
                            <Columns>
                                <dxe:GridViewCommandColumn VisibleIndex="0" ShowSelectCheckbox="True" Width="60" Caption=" " />
                                <dxe:GridViewDataTextColumn Caption="Product" FieldName="ProductName"
                                    VisibleIndex="1">
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="Description" FieldName="Description"
                                    VisibleIndex="2">
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="Quantity" FieldName="Quantity"
                                    VisibleIndex="3">
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="Balance Quantity" FieldName="BalQuantity"
                                    VisibleIndex="4">
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn VisibleIndex="5" FieldName="OrderDetails_OrderId" ReadOnly="true" Caption="OrderDetails_OrderId" Width="0">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn VisibleIndex="6" FieldName="OrderDetails_ProductId" ReadOnly="true" Caption="OrderDetails_ProductId" Width="0">
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

                            <Settings ShowGroupPanel="True" ShowStatusBar="Hidden" ShowHorizontalScrollBar="False" ShowFilterRow="true" ShowFilterRowMenu="true" />
                            <SettingsLoadingPanel Text="Please Wait..." />
                            <ClientSideEvents EndCallback="OnProductCloseEndCall" />
                        </dxe:ASPxGridView>
                    </div>
                    <div class="clear"></div>
                    <div class="text-center pTop10">
                        <dxe:ASPxButton ID="btnCloseProduct" ClientInstanceName="cbtnCloseProduct" runat="server" AutoPostBack="False" Text="Close Product" CssClass="btn btn-primary" UseSubmitBehavior="False">
                            <ClientSideEvents Click="function(s, e) {return CloseProduct();}" />
                        </dxe:ASPxButton>
                    </div>



                </div>
            </dxe:PopupControlContentControl>
        </ContentCollection>
    </dxe:ASPxPopupControl>
</asp:Content>

