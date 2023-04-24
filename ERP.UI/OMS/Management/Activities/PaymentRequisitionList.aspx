<%--=======================================================Revision History=======================================    
    1.0   Pallab    V2.0.38   20-04-2023      25883: Cash / Fund Requisition module design modification
=========================================================End Revision History=====================================--%>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PaymentRequisitionList.aspx.cs" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Management.Activities.PaymentRequisitionList" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/jquery-confirm/3.1.0/jquery-confirm.min.css"/>
    <link href="../../../assests/css/custom/jquery.confirm.css" rel="stylesheet" />
       <script>

           function timerTick() {

               $.ajax({
                   type: "POST",
                   url: "SalesOrderEntityList.aspx/GetTotalWatingOrderCount",
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
                       var url = 'SalesOrderAdd.aspx?key=' + 'ADD&&BasketId=' + listKey;
                       window.location.href = url;
                   } else {
                   }
               }
           }


           function OnWaitingGridKeyPress(e) {

               if (e.code == "Enter") {
                   var index = CwatingOrdergrid.GetFocusedRowIndex();
                   var listKey = CwatingOrdergrid.GetRowKey(index);
                   if (listKey) {
                       if (CwatingOrdergrid.GetRow(index).children[6].innerText != "Advance") {
                           var url = 'SalesOrderAdd.aspx?key=' + 'ADD&&BasketId=' + listKey;
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
                       var url = 'SalesOrderAdd.aspx?key=' + 'ADD&&BasketId=' + listKey;
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
               uri = "SalesOrderAdd.aspx?key=" + obj + "&status=2" + '&type=SO' + '&isformApprove=YES';
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
               uri = "SalesOrderAdd.aspx?key=" + obj + "&status=3" + '&type=SO';
               popup.SetContentUrl(uri);
               popup.Show();
           }
           // function above  End For Rejected

           // To Reflect the Data in Pending Waiting Grid and Pending Waiting Counting if the user approve or Rejecte the Order and Saved 

           function OnApprovalEndCall(s, e) {
               $.ajax({
                   type: "POST",
                   url: "SalesOrderList.aspx/GetPendingCase",
                   contentType: "application/json; charset=utf-8",
                   success: function (data) {
                      <%-- $('#<%= lblWaiting.ClientID %>').text(data.d);--%>
                   }
               });
           }

           // function above  End 
           //---------------------Tab section

           function disp_prompt(name) {

               if (name == "tab0") {
                   // ctxtCustName.Focus();
                   page.GetTabByName('Billing/Shipping').SetEnabled(true);
                   //page.GetTabByName('[B]illing/Shipping').Readonly = false;
                   $("#divcross").show();
                   //alert(name);
                   //document.location.href = "SalesQuotation.aspx?";
               }
               if (name == "tab1") {
                   $("#divcross").hide();
                   var custID = GetObjectID('hdnCustomerId').value;
                   page.GetTabByName('General').SetEnabled(false);
                   // page.GetTabByName('General').Readonly=true;
                   if (custID == null && custID == '') {
                       jAlert('Please select a customer');
                       page.SetActiveTabIndex(0);
                       return;
                   }
                   else {
                       page.SetActiveTabIndex(1);
                       //fn_PopOpen();
                   }
               }
           }
           //----------End Tab Section-------------------
    </script>

    <style>
        #ASPXPopupControl_PW-1 iframe .crossBtn {
            display:none;
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
                cGrdPayReq.Refresh();
                cGrdPayReqGrdPayReqdetails.Refresh();


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
        //function OnClickDelete(keyValue, visibleIndex) {
        //    cGrdOrder.SetFocusedRowIndex(visibleIndex);
        //    var IsBalMapQtyExists = cGrdOrder.GetRow(cGrdOrder.GetFocusedRowIndex()).children[18].innerText;
        //    //if (IsBalMapQtyExists.trim() != "0") {
        //    var IsCancelFlag = cGrdOrder.GetRow(cGrdOrder.GetFocusedRowIndex()).children[14].innerText;
        //    if (IsCancelFlag.trim() != 'Cancelled' && IsCancelFlag.trim() != 'Closed') {

        //        $.ajax({
        //            type: "POST",
        //            url: "SalesOrderList.aspx/GetSalesInvoiceIsExistInSalesInvoice",
        //            data: "{'keyValue':'" + keyValue + "'}",
        //            contentType: "application/json; charset=utf-8",
        //            dataType: "json",
        //            async: false,//Added By:Subhabrata
        //            success: function (msg) {
        //                //debugger;
        //                var status = msg.d;
        //                if (status == "1") {
        //                    jAlert('Used in other module(s). Cannot Delete.', 'Confirmation Dialog', function (r) {
        //                        if (r == true) {
        //                            return false;
        //                        }
        //                    });
        //                }
        //                else {
        //                    jConfirm('Confirm delete?', 'Confirmation Dialog', function (r) {
        //                        if (r == true) {
        //                            cGrdOrder.PerformCallback('Delete~' + keyValue);
        //                        }
        //                    });
        //                }
        //            }
        //        });
        //    }
        //    else {
        //        jAlert("Sales Order is " + IsCancelFlag.trim() + ".Delete is not allowed.");
        //    }
        //    //}
        //    //else
        //    //{
        //    //    jAlert("Sales Order is tagged with other module.Delete is not allowed.");
        //    //}
        //}

        function OnclickViewAttachment(obj) {
            //var URL = '/OMS/Management/Activities/SalesOrder_Document.aspx?idbldng=' + obj + '&type=SalesOrder';
            var URL = '/OMS/Management/Activities/EntriesDocuments.aspx?idbldng=' + obj + '&type=SalesOrder';
            window.location.href = URL;
        }
        function OnClickStatus(keyValue) {
            GetObjectID('hiddenedit').value = keyValue;
            cGrdOrder.PerformCallback('Edit~' + keyValue);
        }
        function fn_rejectreq(keyValue) {
            jConfirm('Do you want to Reject Cash / Fund Requisition ?', 'Confirmation Dialog', function (r) {
                debugger;
                if (r == true) {
                    // $("hddnKeyValue").val(keyValue);
                    hddnKeyValue.value = keyValue;
                    $('#MandatoryRemarksFeedback1').attr('style', 'display:none;position: absolute; right: -20px; top: 8px;');
                    cPopup_Closed.Show();
                    // CloseSalesQuotationQuantity(keyValue);
                }
            });
        }
        function fn_Approvereq(keyValue) {
            jConfirm('Do you want to Approve Cash / Fund Requisition ?', 'Confirmation Dialog', function (r) {
                debugger;
                if (r == true) {
                    // $("hddnKeyValue").val(keyValue);
                    hddnKeyValue.value = keyValue;
                    $('#MandatoryRemarksFeedbackforapprove').attr('style', 'display:none;position: absolute; right: -20px; top: 8px;');
                    cPopup_Approve.Show();
                    // CloseSalesQuotationQuantity(keyValue);
                }
            });
        }
        function griddetails_EndCallBack(s, e) { }
        function grid_EndCallBack(s, e) {
            //if (cGrdOrder.cpEdit != null) {
            //    GetObjectID('hiddenedit').value = cGrdOrder.cpEdit.split('~')[0];
            //    cProforma.SetText(cGrdOrder.cpEdit.split('~')[1]);
            //    cCustomer.SetText(cGrdOrder.cpEdit.split('~')[4]);
            //    var pro_status = cGrdOrder.cpEdit.split('~')[2]
            //    if (pro_status != null) {
            //        var radio = $("[id*=rbl_OrderStatus] label:contains('" + pro_status + "')").closest("td").find("input");
            //        radio.attr("checked", "checked");

            //        cOrderRemarks.SetText(cGrdOrder.cpEdit.split('~')[3]);
            //        cOrderStatus.Show();
            //    }
            //}
            //if (cGrdOrder.cpDelete != null) {
            //    jAlert(cGrdOrder.cpDelete);
            //    cGrdOrder.cpDelete = null;
            //    cGrdOrder.Refresh();
            //}cpDocNo


            if (cGrdPayReq.cpReturnValue != null && cGrdPayReq.cpReturnValue == "1") {
                var cashbankreq_Number = cGrdPayReq.cpDocNo;
                var CashFundRequisition_Msg = "Cash / Fund Requisition. " + cashbankreq_Number + " Deleted Successfully.";

                jAlert(CashFundRequisition_Msg, 'Alert Dialog: [Cash / Fund Requitision]', function (r) {
                    if (r == true)
                    {
                       cGrdPayReq.Refresh();
                       cGrdPayReqGrdPayReqdetails.Refresh();
                       //jAlert(CashFundRequisition_Msg);
                       cGrdPayReq.cpReturnValue = null;
                    }
                });
       
            }
            if (cGrdPayReq.cpReturnValue != null && cGrdPayReq.cpReturnValue == "9") {
                var cashbankreq_Number = cGrdPayReq.cpDocNo;
                var CashFundRequisition_Msg = "Cash / Fund Requisition Approve.Can't Delete!";
                jAlert(CashFundRequisition_Msg, 'Alert Dialog: [Cash / Fund Requitision]', function (r) {
                    if (r == true) {
                       // location.reload();
                    }
                });
            }
            if (cGrdPayReq.cpReturnValue != null && cGrdPayReq.cpReturnValue == "10") {
                var cashbankreq_Number = cGrdPayReq.cpDocNo;
                var CashFundRequisition_Msg = "Cash / Fund Req. already tagged in Cash/Bank Voucher,Can't Delete!";
                jAlert(CashFundRequisition_Msg, 'Alert Dialog: [Cash / Fund Requitision]', function (r) {
                    if (r == true) {
                        //location.reload();
                    }
                });
            }
            else {
                //jAlert(GrdPayReq.cpReturnValue);
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
            //cGrdOrder.SetFocusedRowIndex(visibleIndex);
            //  var IsBalMapQtyExists = cGrdOrder.GetRow(cGrdOrder.GetFocusedRowIndex()).children[17].innerHTML; //cGrdOrder.GetRow(cGrdOrder.GetFocusedRowIndex()).children[18].innerText;
            //if (IsBalMapQtyExists.trim() != "0") {


            // var IsCancelFlag = cGrdOrder.GetRow(cGrdOrder.GetFocusedRowIndex()).children[16].innerText;

           <%-- if (IsCancelFlag.trim() != 'Cancelled' && IsCancelFlag.trim() != 'Closed') {
                var ActiveUser = '<%=Session["userid"]%>'
                if (ActiveUser != null) {
                    $.ajax({
                        type: "POST",
                        url: "SalesOrderList.aspx/GetEditablePermission",
                        data: "{'ActiveUser':'" + ActiveUser + "'}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        async: false,
                        success: function (msg) {
                            var status = msg.d;
                            var url = 'SalesOrderAdd.aspx?key=' + keyValue + '&Permission=' + status + '&type=SO';
                            window.location.href = url;
                        }
                    });
                }
            }
            else {
                jAlert("Sales Order is " + IsCancelFlag.trim() + ".Edit is not allowed.");
            }--%>
            var url = 'PaymentRequisition.aspx?key=' + keyValue + '&Permission=' + status + '&type=SO';
            window.location.href = url;
        }
        function OnClickDelete(keyValue) {
            debugger;
            jConfirm('Confirm delete?', 'Confirmation Dialog', function (r) {
                if (r == true) {
                    cGrdPayReq.PerformCallback('Delete~' + keyValue);
                }
            });
        }

        function OnViewClick(keyValue) {
            var url = 'PaymentRequisition.aspx?key=' + keyValue + '&Permission=' + status + '&req=V';
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

             jConfirm('Do you want to Reject the Cash / Fund Requisition ?', 'Confirm Dialog', function (r) {
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
                 ClosedSalesOrder(KeyVal, Remarks, 'rejectorder');
                 //cGrdOrder.Refresh();
                 //cGrdOrder.PerformCallback('Feedback~' + KeyVal + '~' + Remarks);
                 //Grid.Refresh();


             }
             return flag;

         }
         function CallClosed_saveforapprove() {
             //debugger;
             var KeyVal = $("#<%=hddnKeyValue.ClientID%>").val();
            var flag = true;
             <%--$("#<%=hddnIsSavedFeedback.ClientID%>").val("1");--%>
            var Remarks = txtClosedforsave.GetValue();
            if (Remarks == "" || Remarks == null) {
                $('#MandatoryRemarksFeedbackforapprove').attr('style', 'display:block;position: absolute; right: -20px; top: 8px;');
                flag = false;
            }
            else {
                $('#MandatoryRemarksFeedbackforapprove').attr('style', 'display:none;position: absolute; right: -20px; top: 8px;');
                cPopup_Approve.Hide();
                ClosedSalesOrder(KeyVal, Remarks, 'Approve');
                //cGrdOrder.Refresh();
                //cGrdOrder.PerformCallback('Feedback~' + KeyVal + '~' + Remarks);
                //Grid.Refresh();


            }
            return flag;

        }

        function CancelSalesOrder(keyValue, Reason) {
            //debugger;
            $.ajax({
                type: "POST",
                url: "SalesOrderEntityList.aspx/CancelSalesOrderOnRequest",
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
                    else if (status == "-5") {
                        jAlert("No balance quantity available for this order. Cannot proceed.");
                    }
                }
            });
        }


        function ClosedSalesOrder(keyValue, Reason, status) {
            //debugger;
            $.ajax({
                type: "POST",
                url: "PaymentRequisitionList.aspx/Rejectrequitision",
                data: JSON.stringify({ keyValue: keyValue, Reason: Reason, status: status }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,//Added By:Subhabrata
                success: function (msg) {
                    //debugger;
                    var status = msg.d;
                    if (status == "1") {
                        jAlert("Cash / Fund Requisition Rejected.", "Approval", function () {
                            txtClosed.SetText('');
                            txtClosedforsave.SetText('');
                            //location.reload();
                            cGrdPayReq.Refresh();
                            cGrdPayReqGrdPayReqdetails.Refresh();
                            status = "";
                        });

                    }
                    else if (status == "2") {
                        jAlert("Cash / Fund Requisition Approved.", "Approval", function () {
                            txtClosed.SetText('');
                            txtClosedforsave.SetText('');
                            //location.reload();
                            cGrdPayReq.Refresh();
                            cGrdPayReqGrdPayReqdetails.Refresh();
                            status = "";
                        });

                    }
                    else if (status == "3") {
                        jAlert("Cash / Fund Already Rejected,Can't be Approve!.", "Approval", function () {
                            txtClosed.SetText('');
                            txtClosedforsave.SetText('');
                            //location.reload();
                        });

                    }
                    else if (status == "4") {
                        jAlert("Cash / Fund Already Approve,Can't be Reject!.", "Approval", function () {
                            txtClosed.SetText('');
                            txtClosedforsave.SetText('');
                            //location.reload();
                        });

                    }
                    else if (status == "-10") {
                        jAlert("Try again later");
                    }
                    //else if (status == "-2") {
                    //    jAlert("Selected order is tagged in other module. Cannot proceed.");
                    //}
                    //else if (status == "-3") {
                    //    jAlert("Order is  already closed.");
                    //}
                    //else if (status == "-4") {
                    //    jAlert("Order is already cancelled. Cannot proceed.");
                    //}
                    //else if (status == "-5") {
                    //    jAlert("No balance quantity available for this order. Cannot proceed.");
                    //}
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
             var url = 'PaymentRequisition.aspx?key=' + 'ADD';
             window.location.href = url;
         }
         //});
         $(document).ready(function () {

         });




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
           .closeApprove{

            float: right;
            margin-right: 7px;
           }
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            $('.navbar-minimalize').click(function () {
                if ($('body').hasClass('mini-navbar')) {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 220;
                    cGrdPayReq.SetWidth(cntWidth);
                } else {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 90;
                    cGrdPayReq.SetWidth(cntWidth);
                }
            });
        });
    </script>

    <style>
        /*Rev 1.0*/

        select
        {
            height: 30px !important;
            border-radius: 4px;
            /*-webkit-appearance: none;*/
            position: relative;
            z-index: 1;
            background-color: transparent;
            padding-left: 10px !important;
            padding-right: 22px !important;
        }

        .dxeButtonEditSys.dxeButtonEdit_PlasticBlue , .dxeTextBox_PlasticBlue
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
            right: 18px;
            z-index: 0;
            cursor: pointer;
        }

        .right-20{
            right: 18px !important;
        }

        #ASPxFromDate , #ASPxToDate , #ASPxASondate , #ASPxAsOnDate , #FormDate , #toDate , #dtTDate , #InstDate, #tDate
        {
            position: relative;
            z-index: 1;
            background: transparent;
        }

        .dxeDisabled_PlasticBlue
        {
            z-index: 0 !important;
        }

        #ASPxFromDate_B-1 , #ASPxToDate_B-1 , #ASPxASondate_B-1 , #ASPxAsOnDate_B-1 , #FormDate_B-1 , #toDate_B-1 , #dtTDate_B-1 , #InstDate_B-1, #tDate_B-1
        {
            background: transparent !important;
            border: none;
            width: 30px;
            padding: 10px !important;
        }

        #ASPxFromDate_B-1 #ASPxFromDate_B-1Img , #ASPxToDate_B-1 #ASPxToDate_B-1Img , #ASPxASondate_B-1 #ASPxASondate_B-1Img , #ASPxAsOnDate_B-1 #ASPxAsOnDate_B-1Img ,
        #FormDate_B-1 #FormDate_B-1Img , #toDate_B-1 #toDate_B-1Img , #dtTDate_B-1 #dtTDate_B-1Img , #InstDate_B-1 #InstDate_B-1Img, #tDate_B-1 #tDate_B-1Img
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
            top: 6px;
            right: -2px;
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
            line-height: 18px;
            z-index: 0;
        }
        .simple-select {
            position: relative;
                z-index: 0;
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

        .TableMain100 #ShowGrid , .TableMain100 #ShowGridList , .TableMain100 #ShowGridRet , .TableMain100 #ShowGridLocationwiseStockStatus ,
        #GvCBSearch
        {
            max-width: 98% !important;
        }

        /*div.dxtcSys > .dxtc-content > div, div.dxtcSys > .dxtc-content > div > div
        {
            width: 95% !important;
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
                margin-top: 3px;
        }

        .dxtlHeader_PlasticBlue
        {
            background: #094e8c !important;
        }

        .dxeBase_PlasticBlue .dxichCellSys
        {
            padding-top: 2px !important;
        }

        .pBackDiv
        {
            border-radius: 10px;
            box-shadow: 1px 1px 10px #1111112e;
        }
        .HeaderStyle th
        {
            padding: 5px;
        }

        .for-cust-icon {
            position: relative;
            z-index: 1;
        }

        .dxtcLite_PlasticBlue.dxtc-top > .dxtc-stripContainer
        {
            padding-top: 15px;
        }

        .pt-2
        {
            padding-top: 5px;
        }
        .pt-10
        {
            padding-top: 10px;
        }

        .pt-15
        {
            padding-top: 15px;
        }

        .pb-10
        {
            padding-bottom: 10px;
        }

        .pTop10 {
    padding-top: 20px;
}
        .custom-padd
        {
            padding-top: 4px;
    padding-bottom: 10px;
        }

        input + label
        {
                margin-right: 10px;
        }

        .btn
        {
            margin-bottom: 0;
        }

        .pl-10
        {
            padding-left: 10px;
        }

        /*.col-md-3>label, .col-md-3>span
        {
            margin-top: 0 !important;
        }*/

        .devCheck
        {
            margin-top: 5px;
        }

        .mtc-5
        {
            margin-top: 5px;
        }

        .mtc-10
        {
            margin-top: 10px;
        }

        select.btn
        {
           position: relative;
           z-index: 0;
        }

        select
        {
            margin-bottom: 0;
        }

        .form-control
        {
            background-color: transparent;
        }

        #massrecdt
        {
            width: 100%;
        }

        .mb-10{
            margin-bottom: 10px;
        }

        .crossBtn
        {
            top: 25px;
                right: 25px;
        }

        input[type="text"], input[type="password"], textarea
        {
                margin-bottom: 0;
        }

        #CallbackPanel_LPV
        {
                top: 410px !important;
        }

        .col-md-2
        {
            padding-right: 12px;
            padding-left: 12px;
        }
        /*Rev end 1.0*/
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <%--Rev 1.0: "outer-div-main" class add --%>
        <div class="outer-div-main clearfix">
            <div class="panel-heading clearfix">
        <div class="panel-title pull-left">
            <h3>Cash / Fund Requisition</h3>
        </div>
        <table class="padTab pull-right" style="margin-top: 7px">
            <tr>
                <td>
                    <label>From </label>
                </td>
                <%--Rev 1.0: "for-cust-icon" class add --%>
                <td style="width: 150px" class="for-cust-icon">
                    <dxe:ASPxDateEdit ID="FormDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cFormDate" Width="100%">
                        <ButtonStyle Width="13px">
                        </ButtonStyle>
                    </dxe:ASPxDateEdit>
                    <%--Rev 1.0--%>
                    <img src="/assests/images/calendar-icon.png" class="calendar-icon"/>
                    <%--Rev end 1.0--%>
                </td>
                <td>
                    <label>To </label>
                </td>
                <%--Rev 1.0: "for-cust-icon" class add --%>
                <td style="width: 150px" class="for-cust-icon">
                    <dxe:ASPxDateEdit ID="toDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="ctoDate" Width="100%">
                        <ButtonStyle Width="13px">
                        </ButtonStyle>
                    </dxe:ASPxDateEdit>
                    <%--Rev 1.0--%>
                    <img src="/assests/images/calendar-icon.png" class="calendar-icon"/>
                    <%--Rev end 1.0--%>
                </td>
                <td>Unit</td>
                <td>
                    <dxe:ASPxComboBox ID="cmbBranchfilter" runat="server" ClientInstanceName="ccmbBranchfilter" Width="100%">
                    </dxe:ASPxComboBox>
                </td>
                <td>
                    <input type="button" value="Show" class="btn btn-primary"  onclick="updateGridByDate()"/> <%--onclick="updateGridByDate()"--%>
                </td>

            </tr>

        </table>
    </div>
            <div class="form_main">
        <div class="clearfix">
            <% if (rights.CanAdd)
               { %>
            <a href="javascript:void(0);" onclick="OnAddButtonClick()" class="btn btn-success"><span class="btn-icon"><i class="fa fa-plus" ></i></span><span>Requisition</span> </a>
           <%} %>
             <% if (rights.CanExport)
               { %>
            <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-primary" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true" OnChange="if(!AvailableExportOption()){return false;}">
                <asp:ListItem Value="0">Export to</asp:ListItem>
                <asp:ListItem Value="1">PDF</asp:ListItem>
                <asp:ListItem Value="2">XLS</asp:ListItem>
                <asp:ListItem Value="3">RTF</asp:ListItem>
                <asp:ListItem Value="4">CSV</asp:ListItem>
            </asp:DropDownList>
            <% } %>
          <%-- 
            <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-primary btn-radius" 
                OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true" OnChange="if(!AvailableExportOption()){return false;}">
                <asp:ListItem Value="0">Export to</asp:ListItem>
                <asp:ListItem Value="1">PDF</asp:ListItem>
                <asp:ListItem Value="2">XLS</asp:ListItem>
                <asp:ListItem Value="3">RTF</asp:ListItem>
                <asp:ListItem Value="4">CSV</asp:ListItem>
            </asp:DropDownList>--%>
            
     
            <%--<a href="javascript:void(0);" onclick="OpenWaitingOrder()" class="btn btn-warning  typeNotificationBtn btn-radius"><span><u>O</u>rder Waiting </span>
           <span class="typeNotification"><dxe:ASPxLabel runat="server" Text="" ID="lblQuoteweatingCount" ClientInstanceName="ClblQuoteweatingCount"></dxe:ASPxLabel></span>
            </a>--%>

<%--            <span id="spanStatus" runat="server">
                <a href="javascript:void(0);" onclick="OpenPopUPUserWiseQuotaion()" class="btn btn-primary btn-radius">
                    <span>My Sales Order Status</span>               
                </a>
            </span>--%>
         <%--   <span id="divPendingWaiting" runat="server">
                <a href="javascript:void(0);" onclick="OpenPopUPApprovalStatus()" class="btn btn-primary btn-radius">
                    <span>Approval Waiting</span>

                    <asp:Label ID="lblWaiting" runat="server" Text=""></asp:Label>
                </a>
                <i class="fa fa-reply blink" style="font-size: 20px; margin-right: 10px;" aria-hidden="true"></i>

            </span>--%>

        </div>
    </div>
        
            <div>
         <asp:HiddenField ID="hfIsFilter" runat="server" />
        <asp:HiddenField ID="hfFromDate" runat="server" />
        <asp:HiddenField ID="hfToDate" runat="server" />
        <asp:HiddenField ID="hfBranchID" runat="server" />
        <asp:HiddenField ID="hdnIsUserwiseFilter" runat="server" />
        <asp:HiddenField ID="hddnKeyValue" runat="server" />
        <asp:HiddenField ID="hddnCancelCloseFlag" runat="server" />
    </div>


    <%-- ----------------------------------------------Tab Start--------------------------------------------------- --%>

            <asp:Panel ID="pnl_quotation" runat="server">
            <div class="">
                <dxe:ASPxPageControl ID="ASPxPageControl1" runat="server" ClientInstanceName="page" Width="100%">
                    <TabPages>
                        <dxe:TabPage Name="General" Text="General">
                            <ContentCollection>
                                <dxe:ContentControl runat="server">



    <div class="GridViewArea relative">
        <dxe:ASPxGridView ID="GrdPayReq" runat="server" KeyFieldName="Paymentreqhead_Id" AutoGenerateColumns="False" DataSourceID="gridpaymentreqDataSource" SettingsDataSecurity-AllowEdit="false" 
            SettingsDataSecurity-AllowInsert="false" SettingsDataSecurity-AllowDelete="false" 
            Width="100%" ClientInstanceName="cGrdPayReq" OnCustomCallback="GrdPayReq_CustomCallback" 
            SettingsBehavior-AllowFocusedRow="true" OnHtmlRowPrepared="gridPayReq_HtmlRowPrepared" SettingsBehavior-ColumnResizeMode="Control"
            HorizontalScrollBarMode="Auto" Settings-VerticalScrollableHeight="250" Settings-VerticalScrollBarMode="Auto"  >
             <SettingsSearchPanel Visible="True" Delay="5000" />
            <Columns>
                <dxe:GridViewDataTextColumn Caption="Document No." FieldName="DocNo" 
                    VisibleIndex="1" FixedStyle="Left" Width="140px" Settings-ShowFilterRowMenu="True">
                
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                     <Settings AllowAutoFilterTextInputTimer="False" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Posting Date" FieldName="Date"
                    VisibleIndex="2" Width="150px" Settings-ShowFilterRowMenu="True">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                     <PropertiesTextEdit DisplayFormatString="dd-MM-yyyy"></PropertiesTextEdit>
                     <Settings AllowAutoFilterTextInputTimer="False" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Name" FieldName="Name"
                    VisibleIndex="3" Width="300px" Settings-ShowFilterRowMenu="True">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                     <Settings AllowAutoFilterTextInputTimer="False" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Branch" FieldName="branch_description"
                    VisibleIndex="4" Width="300px" Settings-ShowFilterRowMenu="True">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                     <Settings AllowAutoFilterTextInputTimer="False" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Mode" FieldName="PaymentMode"
                    VisibleIndex="5" Width="250px" Settings-ShowFilterRowMenu="True">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                     <Settings AllowAutoFilterTextInputTimer="False" />
                </dxe:GridViewDataTextColumn>
                  <dxe:GridViewDataTextColumn Caption="Job No." FieldName="projectcode"
                    VisibleIndex="6" Width="250px" Settings-ShowFilterRowMenu="True" Settings-AllowAutoFilter="True">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                     <Settings AllowAutoFilterTextInputTimer="True" />
                </dxe:GridViewDataTextColumn>
                 <dxe:GridViewDataTextColumn Caption="Service Name." FieldName="Service_Name"
                    VisibleIndex="7" Width="250px" Settings-ShowFilterRowMenu="True" Settings-AllowAutoFilter="True">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                     <Settings AllowAutoFilterTextInputTimer="True" />
                </dxe:GridViewDataTextColumn>
                     <dxe:GridViewDataTextColumn Caption="Status" FieldName="IsApprove"
                    VisibleIndex="8" Width="250px" Settings-ShowFilterRowMenu="True" Settings-AllowAutoFilter="True">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                     <Settings AllowAutoFilterTextInputTimer="True" />
                </dxe:GridViewDataTextColumn>
                 <dxe:GridViewDataTextColumn Caption="Created By" FieldName="CreatedByName"
                    VisibleIndex="9" Width="100px" Settings-ShowFilterRowMenu="True" Settings-AllowAutoFilter="True">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                 </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Created On" FieldName="CreatedOn" SortOrder="Descending"
                    VisibleIndex="10" Width="150px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                     <PropertiesTextEdit DisplayFormatString="dd-MM-yyyy"></PropertiesTextEdit>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                 <dxe:GridViewDataTextColumn Caption="Updated By" FieldName="ModifiedByName"
                    VisibleIndex="11" Width="100px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Updated On" FieldName="ModifiedOn"
                    VisibleIndex="12" Width="150px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>

                <dxe:GridViewDataTextColumn HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="center" VisibleIndex="24" Width="0">
                    <DataItemTemplate>
                        <div class="floatedIcons">
                            <div class='floatedBtnArea'>
                     <%--   <% if (rights.CanView)
                           { %>--%>
                                <% if (rights.CanView)
                                       { %>
                        <a href="javascript:void(0);" onclick="OnViewClick('<%#Eval("Paymentreqhead_Id")%>')" class="" title="">
                            <span class='ico ColorSix'><i class='fa fa-eye'></i></span><span class='hidden-xs'>View</span></a>
                                 <%} %>
                                  <% if (rights.CanEdit)
                                       { %>
                                  <a href="javascript:void(0);" onclick="OnMoreInfoClick('<%#Eval("Paymentreqhead_Id")%>',<%# Container.VisibleIndex %>)" class="" title="">

                            <span class='ico editColor'><i class='fa fa-pencil' aria-hidden='true'></i></span><span class='hidden-xs'>Modify</span></a>
                                <%} %>
                                  <% if (rights.CanDelete)
                                       { %>
                            <a href="javascript:void(0);" onclick="OnClickDelete('<%#Eval("Paymentreqhead_Id") %>')" class="" title="">
                            <span class='ico deleteColor'><i class='fa fa-trash' aria-hidden='true'></i></span><span class='hidden-xs'>Delete</span></a>
                      <%} %>
                                  <% if (rights.CanApproved)
                                       { %>
                                   <a href="javascript:void(0);" onclick="fn_Approvereq('<%#Eval("Paymentreqhead_Id") %>')" title="" class="">
                        <%--  <a href="javascript:void(0);" onclick="fn_Editcity(<%#Eval("sProducts_ID")%>)" title="" class="">--%>
                      <span class='ico editColor'><i class='fa fa-check' aria-hidden='true'></i></span><span class='hidden-xs'>Approve</span>
                                       <%} %>
                                       <% if (rights.CanApproved)
                                       { %>
                                                <a href="javascript:void(0);" onclick="fn_rejectreq('<%#Eval("Paymentreqhead_Id") %>')" title="" class="">
                        <%--  <a href="javascript:void(0);" onclick="fn_Editcity(<%#Eval("sProducts_ID")%>)" title="" class="">--%>
                      <span class='ico deleteColor'><i class='fa fa-times' aria-hidden='true'></i></span><span class='hidden-xs'>Reject</span>
                                                     <%} %>
                     
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
            <SettingsCookies Enabled="true" StorePaging="true" StoreColumnsVisiblePosition="true" Version="4.4" />
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
                <dxe:ASPxSummaryItem FieldName="Amount" SummaryType="Sum" />
            </TotalSummary>
        </dxe:ASPxGridView>
        <asp:HiddenField ID="hiddenedit" runat="server" />
        <dx:LinqServerModeDataSource ID="gridpaymentreqDataSource" runat="server" OnSelecting="gridpaymentreq_Selecting"
            ContextTypeName="ERPDataClassesDataContext" TableName="v_GetpaymentRequisitionList" />
    </div>
<div>
</div>
     <div style="display: none">
        <dxe:ASPxGridViewExporter ID="exporter" GridViewID="GrdPayReq" runat="server" Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
        </dxe:ASPxGridViewExporter>
    </div>
 </dxe:ContentControl>
                            </ContentCollection>
                            <%--test generel--%>
                        </dxe:TabPage>


                             <dxe:TabPage Name="Billing/Shipping" Text="Details">
                            <ContentCollection>
                                <dxe:ContentControl runat="server">


<div class="GridViewArea relative">
        <dxe:ASPxGridView ID="GrdPayReqdetails" runat="server" KeyFieldName="Paymentreqhead_Id" AutoGenerateColumns="False" DataSourceID="GrdPayReqdetailsDataSource" SettingsDataSecurity-AllowEdit="false" 
            SettingsDataSecurity-AllowInsert="false" SettingsDataSecurity-AllowDelete="false" 
            Width="100%" ClientInstanceName="cGrdPayReqGrdPayReqdetails" OnCustomCallback="GrdPayReqdetails_CustomCallback" 
            SettingsBehavior-AllowFocusedRow="true" OnHtmlRowPrepared="GrdPayReqdetails_HtmlRowPrepared" SettingsBehavior-ColumnResizeMode="Control"
            HorizontalScrollBarMode="Auto" Settings-VerticalScrollableHeight="250" Settings-VerticalScrollBarMode="Auto"  >
             <SettingsSearchPanel Visible="True" Delay="5000" />
            <Columns>
                <dxe:GridViewDataTextColumn Caption="Document No." FieldName="DocNo" 
                    VisibleIndex="1" FixedStyle="Left" Width="140px" Settings-ShowFilterRowMenu="True">
                
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                     <Settings AllowAutoFilterTextInputTimer="False" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Posting Date" FieldName="Date"
                    VisibleIndex="2" Width="150px" Settings-ShowFilterRowMenu="True">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <PropertiesTextEdit DisplayFormatString="dd-MM-yyyy"></PropertiesTextEdit>
                     <Settings AllowAutoFilterTextInputTimer="False" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Name" FieldName="Name"
                    VisibleIndex="3" Width="300px" Settings-ShowFilterRowMenu="True">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                     <Settings AllowAutoFilterTextInputTimer="False" />
                </dxe:GridViewDataTextColumn>
                 <dxe:GridViewDataTextColumn Caption="Amount" FieldName="Amount"
                    VisibleIndex="4" Width="250px" Settings-ShowFilterRowMenu="True" Settings-AllowAutoFilter="True">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                     <Settings AllowAutoFilterTextInputTimer="True" />
                </dxe:GridViewDataTextColumn>
                 <dxe:GridViewDataTextColumn Caption="Particulars" FieldName="Particulars"
                    VisibleIndex="5" Width="250px" Settings-ShowFilterRowMenu="True" Settings-AllowAutoFilter="True">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                     <Settings AllowAutoFilterTextInputTimer="True" />
                </dxe:GridViewDataTextColumn>
                   <dxe:GridViewDataTextColumn Caption="Remarks" FieldName="Remarks"
                    VisibleIndex="6" Width="250px" Settings-ShowFilterRowMenu="True" Settings-AllowAutoFilter="True">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                     <Settings AllowAutoFilterTextInputTimer="True" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Branch" FieldName="branch_description"
                    VisibleIndex="7" Width="300px" Settings-ShowFilterRowMenu="True">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                     <Settings AllowAutoFilterTextInputTimer="False" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Mode" FieldName="PaymentMode"
                    VisibleIndex="8" Width="250px" Settings-ShowFilterRowMenu="True">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                     <Settings AllowAutoFilterTextInputTimer="False" />
                </dxe:GridViewDataTextColumn>
                  <dxe:GridViewDataTextColumn Caption="Job No." FieldName="projectcode"
                    VisibleIndex="9" Width="250px" Settings-ShowFilterRowMenu="True" Settings-AllowAutoFilter="True">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                     <Settings AllowAutoFilterTextInputTimer="True" />
                </dxe:GridViewDataTextColumn>

                 <dxe:GridViewDataTextColumn Caption="Service Name" FieldName="Service_Name"
                    VisibleIndex="10" Width="250px" Settings-ShowFilterRowMenu="True" Settings-AllowAutoFilter="True">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                     <Settings AllowAutoFilterTextInputTimer="True" />
                </dxe:GridViewDataTextColumn>
                 <dxe:GridViewDataTextColumn Caption="Status" FieldName="IsApprove"
                    VisibleIndex="11" Width="250px" Settings-ShowFilterRowMenu="True" Settings-AllowAutoFilter="True">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                     <Settings AllowAutoFilterTextInputTimer="True" />
                </dxe:GridViewDataTextColumn>
               
     

                <dxe:GridViewDataTextColumn HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="center" VisibleIndex="24" Width="0">
                    <DataItemTemplate>
                        <div class="floatedIcons">
                            <div class='floatedBtnArea'>
                     <%--   <% if (rights.CanView)
                           { %>--%>
                       <%-- <a href="javascript:void(0);" onclick="OnViewClick('<%#Eval("Paymentreqhead_Id")%>')" class="" title="">
                            <span class='ico ColorSix'><i class='fa fa-eye'></i></span><span class='hidden-xs'>View</span></a>

                                  <a href="javascript:void(0);" onclick="OnMoreInfoClick('<%#Eval("Paymentreqhead_Id")%>',<%# Container.VisibleIndex %>)" class="" title="">

                            <span class='ico editColor'><i class='fa fa-pencil' aria-hidden='true'></i></span><span class='hidden-xs'>Modify</span></a>
                               
                            <a href="javascript:void(0);" onclick="OnClickDelete('<%#Eval("Paymentreqhead_Id") %>')" class="" title="">
                            <span class='ico deleteColor'><i class='fa fa-trash' aria-hidden='true'></i></span><span class='hidden-xs'>Delete</span></a>--%>
                      
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
            <SettingsCookies Enabled="true" StorePaging="true" StoreColumnsVisiblePosition="true" />
           <%-- -- End of Rev Sayantani --%>
              <SettingsContextMenu Enabled="true"></SettingsContextMenu>
          <%--  <ClientSideEvents EndCallback="function (s, e) {GrdPayReqdetails_EndCallBack();}" RowClick="GrdPayReqdetailsclick" />--%>
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
           <%-- <TotalSummary>
                <dxe:ASPxSummaryItem FieldName="Amount" SummaryType="Sum" />
            </TotalSummary>--%>
        </dxe:ASPxGridView>
        <asp:HiddenField ID="HiddenField1" runat="server" />
        <dx:LinqServerModeDataSource ID="GrdPayReqdetailsDataSource" runat="server" OnSelecting="GrdPayReqdetails_Selecting"
            ContextTypeName="ERPDataClassesDataContext" TableName="v_GetpaymentRequisitionList" />
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
	                                           else if(activeTab == Tab2)
	                                            {
	                                                disp_prompt('tab2');
	                                            }


	                                            }"></ClientSideEvents>

                </dxe:ASPxPageControl>
                </div>
                </asp:Panel>
        </div>
        <dxe:ASPxPopupControl ID="Popup_Closed" runat="server" ClientInstanceName="cPopup_Closed"
            Width="400px" HeaderText="Reason For Reject" PopupHorizontalAlign="WindowCenter"
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
       <dxe:ASPxPopupControl ID="Popup_Approve" runat="server" ClientInstanceName="cPopup_Approve"
            Width="400px" HeaderText="Reason For Approve" PopupHorizontalAlign="WindowCenter"
            BackColor="white" Height="100px" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
            Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True">
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">
                    <%--<div style="Width:400px;background-color:#FFFFFF;margin:0px;border:1px solid red;">--%>
                    <div class="Top clearfix">
                        <table style="width:94%">                           
                            <tr><td>Reason<span style="color: red">*</span></td>
                                <td class="relative">
                                     <dxe:ASPxMemo ID="ASPxMemo2" runat="server" Width="100%" Height="50px" ClientInstanceName="txtClosedforsave"></dxe:ASPxMemo>
                                                                        <span id="MandatoryRemarksFeedbackforapprove" style="display: none">
                                        <img id="gridHistory_DXPEForm_efnew_DXEFL_DXEditor1234_EIq" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"/></span>
                                </td>
                            </tr>                           
                            <tr>
                                <td></td>
                                <td colspan="2" style="padding-top: 10px;">
                                    <input id="btnClosedSaveforapprove" class="btn btn-primary" onclick="CallClosed_saveforapprove()" type="button" value="Save" />&nbsp;&nbsp;
                                    <input id="btnClosedCancelforapprove" class="btn btn-danger" onclick="CancelClosed_saveforapprove()" type="button" value="Cancel" />
                                </td>
                            </tr>
                        </table>
                    </div>

                </dxe:PopupControlContentControl>
            </ContentCollection>
            <HeaderStyle BackColor="LightGray" ForeColor="Black" />

          
        </dxe:ASPxPopupControl>
   </asp:Content>

  

    

