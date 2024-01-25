<%--==========================================================Revision History ============================================================================================   
   1.0   Priti    V2.0.36     16-02-2023     Afer Listing view upgradation delete data show in list issue solved. 
   2.0   Priti    V2.0.38     03-04-2023     0025257: Views to be converted to Procedures in the Listing Page of Transaction / Sales / Inquiry
   3.0   Pallab   V2.0.38     05-04-2023     0025847: Sales Inquiry module design modification
   4.0   Priti    V2.0.43     23-01-2024     0026947: "Clear Filter" is required in landing page of  Entry screens.

========================================== End Revision History =======================================================================================================--%>

<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="SalesInquiry.aspx.cs" Inherits="ERP.OMS.Management.Activities.SalesInquiry" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script>

       
          
        function timerTick() {
            //   cwatingInvoicegrid.Refresh();


            $.ajax({
                type: "POST",
                url: "SalesQuotationList.aspx/GetTotalWatingQuotationCount",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {

                    var status = msg.d;
                    //console.log(status);
                    //ClblQuoteweatingCount.SetText(status);
                    var fetcheddata = parseFloat(document.getElementById('waitingQuotationCount').value);
                    if (status != fetcheddata) {
                        CwatingQuotegrid.Refresh();
                        document.getElementById('waitingQuotationCount').value = status;
                    }
                }
            });

        }



        function OpenPopUPUserWiseQuotaion() {
            cgridUserWiseQuotation.PerformCallback();
            cPopupUserWiseQuotation.Show();
        }
        document.onkeydown = function (e) {
            if (event.keyCode == 18) isCtrl = true;
            if (event.keyCode == 65 && isCtrl == true) { //run code for Ctrl+S -- ie, Save & New  
                StopDefaultAction(e);
                OnAddButtonClick();
            }
        }
        function StopDefaultAction(e) {
            if (e.preventDefault) { e.preventDefault() }
            else { e.stop() };

            e.returnValue = false;
            e.stopPropagation();
        }

       


        function OnClickDelete(keyValue) {
            var IsCancelFlag = cGrdQuotation.GetRow(cGrdQuotation.GetFocusedRowIndex()).children[7].innerText;
            if (IsCancelFlag.trim() != 'Cancelled' && IsCancelFlag.trim() != 'Closed') {
            $.ajax({
                type: "POST",
                url: "SalesInquiry.aspx/GetSalesInquiryIsExistInSalesQuotationr",
                data: "{'keyValue':'" + keyValue + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (msg) {
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
                                cGrdQuotation.PerformCallback('Delete~' + keyValue);
                            }
                        });
                    }
                }
            });
          }
            else
            {
                jAlert("Sales Inquiry is " + IsCancelFlag.trim() + ". Delete is not allowed.");
            }
        }


        function OnCancelClick(keyValue) {
            debugger;
            var IsCancelFlag = cGrdQuotation.GetRow(cGrdQuotation.GetFocusedRowIndex()).children[7].innerText;
            $("#<%=hddnKeyValue.ClientID%>").val(keyValue);
           <%-- $("#<%=hddnKeyValue.ClientID%>").val(keyValue);
                $("#<%=hddnCancelCloseFlag%>").val('CA');--%>
            //cGrdQuotation.SetFocusedRowIndex(visibleIndex);
            if (IsCancelFlag.trim() != 'Cancelled' && IsCancelFlag.trim() != 'Closed')
            {

                jConfirm('Do you want to cancel the Inquiry ?', 'Confirm Dialog', function (r) {
                    if (r == true) {
                        $('#MandatoryRemarksFeedback').attr('style', 'display:none;position: absolute; right: -20px; top: 8px;');
                        cPopup_Feedback.Show();
                    }
                    else {
                        return false;
                    }
                });
            }
            else
            {
                jAlert("Sales Inquiry is already " + IsCancelFlag.trim() + ".");
            }
         }

        function CancelFeedback_save() {

           
             txtFeedback.SetValue();
             cPopup_Feedback.Hide();
           
         }
        function CallFeedback_save() {
            // debugger;
            var keyValue = $("#<%=hddnKeyValue.ClientID%>").val();
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
                   CancelSalesOrder(keyValue, Remarks);
                   cGrdQuotation.Refresh();
                   //cGrdOrder.PerformCallback('Feedback~' + KeyVal + '~' + Remarks);
                   //Grid.Refresh();


               }
               return flag;

           }



        function CancelSalesOrder(keyValue, Reason) {
            //debugger;
            $.ajax({
                type: "POST",
                url: "SalesInquiry.aspx/CancelSalesInquiryOnRequest",
                data: JSON.stringify({ keyValue: keyValue, Reason: Reason }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,//Added By:Subhabrata
                success: function (msg) {
                    //debugger;
                    var status = msg.d;
                    if (status == "1") {
                        jAlert("Inquiry is cancelled successfully.");
                        //cGrdOrder.PerformCallback('BindGrid');


                    }
                    else if (status == "-1") {
                        jAlert("Inquiry is not cancelled.Try again later");
                    }
                    else if (status == "-2") {
                        jAlert("Selected Inquiry is tagged in other module. Cannot proceed.");
                    }
                    else if (status == "-3") {
                        jAlert("Inquiry is  already cancelled.");
                    }
                    else if (status == "-4") {
                        jAlert("Inquiry is already closed. Cannot proceed.");
                    }
                    else if (status == "-5") {
                        jAlert("No balance quantity available for this Inquiry. Cannot proceed.");
                    }
                }
            });
        }


        function OnClickStatus(keyValue) {
            GetObjectID('hiddenedit').value = keyValue;
            cGrdQuotation.PerformCallback('Edit~' + keyValue);
        }

        function OpenPopUPApprovalStatus() {
            cgridPendingApproval.PerformCallback();
            cpopupApproval.Show();
        }

        var PIQAId = 0;
        function onPrintJv(id) {
            debugger;
            PIQAId = id;
            cSelectPanel.cpSuccess = "";
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
            if (cSelectPanel.cpSuccess != "") {
                var TotDocument = cSelectPanel.cpSuccess.split(',');
                var reportName = cCmbDesignName.GetValue();
                //mantise issue:0025139
                //var module = 'PIQuotation';
                var module = 'SalesInquiry';
                //End of mantise issue:0025139
                window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + reportName + '&modulename=' + module + '&id=' + PIQAId, '_blank')
            }
            //cSelectPanel.cpSuccess = null
            if (cSelectPanel.cpSuccess == "") {
                cCmbDesignName.SetSelectedIndex(0);
            }
        }

        //var globalRowIndex;
        //function GetVisibleIndex(s, e) {
        //    globalRowIndex = e.visibleIndex;
        //}
        //RowClick = "GetVisibleIndex"

        //function Approvalgrid_EndCallBack() {
        //    if (cgridPendingApproval.cpEdit != null) {
        //        cpopupApproval.Show(); 
        //    }
        //}
        function OpenWaitingQuote() {
            Cpopup_QuoteWait.Show();
            CwatingQuotegrid.Focus();
        }

        function QuotationWattingOkClick() {
            debugger;
            var index = CwatingQuotegrid.GetFocusedRowIndex();
            var listKey = CwatingQuotegrid.GetRowKey(index);
            if (listKey) {
                if (CwatingQuotegrid.GetRow(index).children[6].innerText != "Advance") {
                    var url = 'SalesQuotation.aspx?key=' + 'ADD&&BasketId=' + listKey;
                    LoadingPanel.Show();
                    window.location.href = url;
                } else {
                    ShowbasketReceiptPayment(listKey);
                }
            }
        }


        function RemoveQuote(obj) {
            if (obj) {
                jConfirm("Clicking on Delete will not allow to use this Billing request again. Are you sure?", "Alert", function (ret) {
                    if (ret) {
                        CwatingQuotegrid.PerformCallback('Remove~' + obj);
                    }
                });

            }
        }

        function watingQuotegridEndCallback() {
            if (CwatingQuotegrid.cpReturnMsg) {
                if (CwatingQuotegrid.cpReturnMsg != "") {
                    jAlert(CwatingQuotegrid.cpReturnMsg);
                    document.getElementById('waitingQuotationCount').value = parseFloat(document.getElementById('waitingQuotationCount').value) - 1;
                    CwatingQuotegrid.cpReturnMsg = null;
                }
            }
        }


        function ListRowClicked(s, e) {

            var index = e.visibleIndex;
            var listKey = CwatingQuotegrid.GetRowKey(index);
            if (e.htmlEvent.target.id != "CloseRemoveWattingBtn") {
                if (CwatingQuotegrid.GetRow(index).children[6].innerText != "Advance") {
                    var url = 'SalesQuotation.aspx?key=' + 'ADD&BasketId=' + listKey;
                    //LoadingPanel.Show();
                    window.location.href = url;
                } else {
                    // ShowbasketReceiptPayment(listKey);
                }
            }
        }
        function OnWaitingGridKeyPress(e) {

            if (e.code == "Enter") {
                var index = CwatingQuotegrid.GetFocusedRowIndex();
                var listKey = CwatingQuotegrid.GetRowKey(index);
                if (listKey) {
                    if (CwatingQuotegrid.GetRow(index).children[6].innerText != "Advance") {
                        var url = 'SalesQuotation.aspx?key=' + 'ADD&&BasketId=' + listKey;
                        // LoadingPanel.Show();
                        window.location.href = url;
                    } else {
                        //ShowbasketReceiptPayment(listKey);
                    }
                }
            }

        }

        function grid_EndCallBack() {
            if (cGrdQuotation.cpEdit != null) {
                GetObjectID('hiddenedit').value = cGrdQuotation.cpEdit.split('~')[0];
                cProforma.SetText(cGrdQuotation.cpEdit.split('~')[1]);
                cCustomer.SetText(cGrdQuotation.cpEdit.split('~')[4]);
                var pro_status = cGrdQuotation.cpEdit.split('~')[2]
                //cGrdQuotation.cpEdit = null;
                if (pro_status != null) {
                    var radio = $("[id*=rbl_QuoteStatus] label:contains('" + pro_status + "')").closest("td").find("input");
                    radio.attr("checked", "checked");
                    //return false;
                    //$('#rbl_QuoteStatus[type=radio][value=' + pro_status + ']').prop('checked', true); 
                    cQuotationRemarks.SetText(cGrdQuotation.cpEdit.split('~')[3]);

                    cQuotationStatus.Show();
                }
            }
            if (cGrdQuotation.cpUpdate != null) {
                GetObjectID('hiddenedit').value = '';
                cProforma.SetText('');
                cCustomer.SetText('');
                cQuotationRemarks.SetText('');
                var pro_status = 2;
                if (pro_status != null) {
                    var radio = $("[id*=rbl_QuoteStatus] label:contains('" + pro_status + "')").closest("td").find("input");
                    radio.attr("checked", "checked");
                    cQuotationStatus.Hide();
                }
                jAlert(cGrdQuotation.cpUpdate);
                cGrdQuotation.Refresh();
            }
            if (cGrdQuotation.cpDelete != null) {
                jAlert(cGrdQuotation.cpDelete);
                cGrdQuotation.cpDelete = null;
                /* Rev 1.0*/
                cGrdQuotation.Refresh();
                /* Rev 2.0*/
                //updateGridByDate();
                /* Rev 2.0 End*/
                /* Rev 1.0 End*/
            }


        }
        function SavePrpformaStatus() {
            if (document.getElementById('hiddenedit').value == '') {
                cGrdQuotation.PerformCallback('save~');
            }
            else {
                var checked_radio = $("[id*=rbl_QuoteStatus] input:checked");
                var status = checked_radio.val();
                var remarks = cQuotationRemarks.GetText();
                cGrdQuotation.PerformCallback('update~' + GetObjectID('hiddenedit').value + '~' + status + '~' + remarks);
            }

        }
        //Rev for approve reject  Tanmoy
        //function OnApproveClick(keyValue) {
        //    var url = 'SalesQuotation.aspx?key=' + keyValue + '&req=V' + '&type=QO';
        //    window.location.href = url;
        //}
        //Rev for approve reject

        ////##### coded by Samrat Roy - 04/05/2017  
        ////Add an another param to define request type 
        function OnViewClick(keyValue) {
            var url = 'SalesInquiryAdd.aspx?key=' + keyValue + '&req=V' + '&type=SINQ';
            window.location.href = url;
        }


        function OnclickViewAttachment(obj) {
            //var URL = '/OMS/Management/Activities/Quotation_Document.aspx?idbldng=' + obj + '&type=Quotation';
            var URL = '/OMS/Management/Activities/EntriesDocuments.aspx?idbldng=' + obj + '&type=Quotation';
            window.location.href = URL;
        }

        function OnAddButtonClick() {
            var url = 'SalesInquiryAdd.aspx?key=' + 'ADD';
            window.location.href = url;
        }
        

        
        var keyval;
        //function FocusedRowChanged(s, e) {
        //    keyval=s.GetRowKey(s.GetFocusedRowIndex());
        //}

        //var globalRowIndex;

        //function GetVisibleIndex(s, e) {
        //    globalRowIndex = e.visibleIndex;
        //}
        //RowClick = "GetVisibleIndex"

        // User Approval Status Start

        function GetApprovedQuoteId(s, e, itemIndex) {
            var rowvalue = cgridPendingApproval.GetRowValues(itemIndex, 'Quote_Id', OnGetApprovedRowValues);
            //var currentRow = cgridPendingApproval.GetRow(0);
            //var col1 = currentRow.find("td:eq(0)").html();

            cgridPendingApproval.PerformCallback('Status~' + rowvalue);
            cgridPendingApproval.GetRowValues(itemIndex, 'Quote_Id', OnGetApprovedRowValues);

        }
        function OnGetApprovedRowValues(obj) {
            uri = "SalesQuotation.aspx?key=" + obj + "&status=2" + '&type=QO';
            popup.SetContentUrl(uri);
            popup.Show();
            //window.location.href = uri;

        }

        function ch_fnApproved() {
        }


        function GetRejectedQuoteId(s, e, itemIndex) {
            debugger;
            cgridPendingApproval.GetRowValues(itemIndex, 'Quote_Id', OnGetRejectedRowValues);

        }
        function OnGetRejectedRowValues(obj) {
            uri = "SalesQuotation.aspx?key=" + obj + "&status=3" + '&type=QO';
            popup.SetContentUrl(uri);
            popup.Show();
        }

        // User Approval Status End

        function OnApprovalEndCall(s, e) {
            $.ajax({
                type: "POST",
                url: "SalesQuotationList.aspx/GetPendingCase",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    $('#<%= lblWaiting.ClientID %>').text(data.d);
                }
            });
            }
            function gridRowclick(s, e) {
                //alert('hi');
                $('#gridcrmCampaign').find('tr').removeClass('rowActive');
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
            function OnMoreInfoClick(keyValue) {
                var ActiveUser = '<%=Session["userid"]%>'

                var IsCancelFlag = cGrdQuotation.GetRow(cGrdQuotation.GetFocusedRowIndex()).children[7].innerText;

                if (IsCancelFlag.trim() != 'Cancelled' && IsCancelFlag.trim() != 'Closed') {
                    if (ActiveUser != null) {
                        $.ajax({
                            type: "POST",
                            url: "SalesInquiry.aspx/GetEditablePermissions",
                            //data: "{'ActiveUser':'" + ActiveUser + "'}",
                            //Rev Rajdip
                            data: "{'ActiveUser':'" + ActiveUser + "','SalesDocId':'" + keyValue + "'}",
                            //END Rev Rajdip
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (msg) {
                                var status = msg.d;
                                //var url = 'SalesQuotation.aspx?key=' + keyValue + '&Permission=' + status + '&type=QO';
                                var url = 'SalesInquiryAdd.aspx?key=' + keyValue + '&Permission=' + status + '&type=SINQ';
                                window.location.href = url;
                            }
                        });
                    }
                }
                else
                {
                    jAlert("Sales Inquiry is " + IsCancelFlag.trim() + ". Edit is not allowed.");
                }
            }
            //Rev Rajdip
            //Start For Copy
            function fn_CopySalesQuotation(keyValue) {
                var ActiveUser = '<%=Session["userid"]%>'
            if (ActiveUser != null) {
                $.ajax({
                    type: "POST",
                    url: "SalesQuotationList.aspx/GetEditablePermissions",
                    //data: "{'ActiveUser':'" + ActiveUser + "'}",
                    data: "{'ActiveUser':'" + ActiveUser + "','SalesDocId':'" + keyValue + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        var status = msg.d;
                        var url = 'SalesQuotation.aspx?key=' + keyValue + '&Permission=' + status + '&type=QO&Typenew=Copy';
                        window.location.href = url;
                    }
                });
            }

            }


        function OnclickViewAttachment(obj) {
            //var URL = '/OMS/Management/Activities/Quotation_Document.aspx?idbldng=' + obj + '&type=Quotation';
            var URL = '/OMS/Management/Activities/EntriesDocuments.aspx?idbldng=' + obj + '&type=SINQ';
            window.location.href = URL;
        }
        function OnApproveClick(keyValue, visibleIndex) {
            debugger;
            cGrdQuotation.SetFocusedRowIndex(visibleIndex);
            var IsBalMapQtyExists = cGrdQuotation.GetRow(cGrdQuotation.GetFocusedRowIndex()).children[7].innerHTML; //cGrdOrder.GetRow(cGrdOrder.GetFocusedRowIndex()).children[18].innerText;
          


            var IsCancelFlag = cGrdQuotation.GetRow(cGrdQuotation.GetFocusedRowIndex()).children[7].innerText;

            if (IsCancelFlag.trim() != 'Cancelled' && IsCancelFlag.trim() != 'Closed') {
                var ActiveUser = '<%=Session["userid"]%>'
                if (ActiveUser != null) {
                    $.ajax({
                        type: "POST",
                        url: "SalesInquiry.aspx/GetEditablePermission",
                        data: "{'ActiveUser':'" + ActiveUser + "'}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        async: false,//Added By:Subhabrata
                        success: function (msg) {
                            //debugger;
                            var status = msg.d;
                            var url = 'SalesInquiryAdd.aspx?key=' + keyValue + '&Permission=' + status + '&type=SINQ' + '&type1=AppSINQ';
                            window.location.href = url;
                        }
                    });
                }
            }
            else {
                jAlert("Sales Inquiry is " + IsCancelFlag.trim() + ". Approve is not allowed.");
            }
        }

        //End For Copy
        //Start For Close Sales Quotation
        function fn_CloseSalesOrder(keyValue) {
            var IsCancelFlag = cGrdQuotation.GetRow(cGrdQuotation.GetFocusedRowIndex()).children[7].innerText;
            if (IsCancelFlag.trim() != 'Cancelled' && IsCancelFlag.trim() != 'Closed') {
                jConfirm('Do you want to close the Sales Inquiry ?', 'Confirmation Dialog', function (r) {
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
            else
               {
                jAlert("Sales Inquiry is already " + IsCancelFlag.trim() + ".");
              }
        }
        function CloseSalesQuotationQuantity(Quote_Id, Remarks) {
            debugger;
            $.ajax({
                type: "POST",
                url: "SalesInquiryAdd.aspx/CloseInquiryquantity",
                data: JSON.stringify({ Quote_Id: Quote_Id, Remarks: Remarks }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var status = msg.d;
                    if (status == "1") {

                        //jAlert("Sales Quotation is closed successfully.");
                        jAlert("Sales Inquiry is closed successfully.", "Approval", function () {
                            location.reload();
                        });
                        //
                    }
                    //var url = 'SalesQuotation.aspx?key=' + keyValue + '&Permission=' + status + '&type=QO';
                    //window.location.href = url;
                }
            });
        }

        function CancelClosed_save() {
            debugger;
            <%-- $("#<%=hddnIsSavedFeedback.ClientID%>").val("0");--%>
            txtClosed.SetValue();
            cPopup_Closed.Hide();
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
                CloseSalesQuotationQuantity(KeyVal, Remarks);
                cPopup_Closed.Hide();

                cGrdOrder.Refresh();
                //cGrdOrder.PerformCallback('Feedback~' + KeyVal + '~' + Remarks);
                //Grid.Refresh();


            }
            return flag;

        }
        //End For Close Sales Quotation
        //Rev Rajdip
    </script>
    <style>
        #btnClearFilter{
            height:33px;
        }
        strong label {
            font-weight: bold !important;
        }

        input[type="radio"] {
            webkit-transform: translateY(3px);
            -moz-transform: translateY(3px);
            transform: translateY(3px);
        }

    </style>
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
    <script>
        var isFirstTime = true;

        function AllControlInitilize() {
            if (isFirstTime) {
                if (localStorage.getItem('QuotationList_FromDate')) {
                    var fromdatearray = localStorage.getItem('QuotationList_FromDate').split('-');
                    var fromdate = new Date(fromdatearray[0], parseFloat(fromdatearray[1]) - 1, fromdatearray[2], 0, 0, 0, 0);
                    cFormDate.SetDate(fromdate);
                }

                if (localStorage.getItem('QuotationList_ToDate')) {
                    var todatearray = localStorage.getItem('QuotationList_ToDate').split('-');
                    var todate = new Date(todatearray[0], parseFloat(todatearray[1]) - 1, todatearray[2], 0, 0, 0, 0);
                    ctoDate.SetDate(todate);
                }

                if (localStorage.getItem('QuotationList_Branch')) {
                    if (ccmbBranchfilter.FindItemByValue(localStorage.getItem('QuotationList_Branch'))) {
                        ccmbBranchfilter.SetValue(localStorage.getItem('QuotationList_Branch'));
                    }

                }

                //updateGridByDate();
                isFirstTime = false;
            }
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
                localStorage.setItem("QuotationList_FromDate", cFormDate.GetDate().format('yyyy-MM-dd'));
                localStorage.setItem("QuotationList_ToDate", ctoDate.GetDate().format('yyyy-MM-dd'));
                localStorage.setItem("QuotationList_Branch", ccmbBranchfilter.GetValue());

                $("#hfFromDate").val(cFormDate.GetDate().format('yyyy-MM-dd'));
                $("#hfToDate").val(ctoDate.GetDate().format('yyyy-MM-dd'));
                $("#hfBranchID").val(ccmbBranchfilter.GetValue());
                $("#hfIsFilter").val("Y");

                $("#hFilterType").val("All");
                cCallbackPanel.PerformCallback("");
                //cGrdQuotation.Refresh();

                //cGrdQuotation.PerformCallback('FilterGridByDate~' + cFormDate.GetDate().format('yyyy-MM-dd') + '~' + ctoDate.GetDate().format('yyyy-MM-dd') + '~' + ccmbBranchfilter.GetValue())
            }
        }
        function CallbackPanelEndCall(s, e) {
            cGrdQuotation.Refresh();
        }
        $(document).ready(function () {
            //Toggle fullscreen expandEntryGrid
            $("#expandcGrdQuotation").click(function (e) {
                e.preventDefault();
                var $this = $(this);
                if ($this.children('i').hasClass('fa-expand')) {
                    $this.removeClass('hovered half').addClass('full');
                    $this.attr('title', 'Minimize Grid');
                    $this.children('i').removeClass('fa-expand');
                    $this.children('i').addClass('fa-arrows-alt');
                    var gridId = $(this).attr('data-instance');
                    $(this).closest('.makeFullscreen').addClass('panel-fullscreen');
                    var cntWidth = $(this).parent('.makeFullscreen').width();
                    var browserHeight = document.documentElement.clientHeight;
                    var browserWidth = document.documentElement.clientWidth;
                    cGrdQuotation.SetHeight(browserHeight - 150);
                    cGrdQuotation.SetWidth(cntWidth);
                }
                else if ($this.children('i').hasClass('fa-arrows-alt')) {
                    $this.children('i').removeClass('fa-arrows-alt');
                    $this.removeClass('full').addClass('hovered half');
                    $this.attr('title', 'Maximize Grid');
                    $this.children('i').addClass('fa-expand');
                    var gridId = $(this).attr('data-instance');
                    $(this).closest('.makeFullscreen').removeClass('panel-fullscreen');
                    var browserHeight = document.documentElement.clientHeight;
                    var browserWidth = document.documentElement.clientWidth;
                    cGrdQuotation.SetHeight(300);
                    var cntWidth = $this.parent('.makeFullscreen').width();
                    cGrdQuotation.SetWidth(cntWidth);
                }
            });
        });
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            $(document).ready(function () {
                if ($('body').hasClass('mini-navbar')) {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 90;
                    cGrdQuotation.SetWidth(cntWidth);
                } else {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 220;
                    cGrdQuotation.SetWidth(cntWidth);
                }

                $('.navbar-minimalize').click(function () {
                    if ($('body').hasClass('mini-navbar')) {
                        var windowWidth = $(window).width();
                        var cntWidth = windowWidth - 220;
                        cGrdQuotation.SetWidth(cntWidth);
                    } else {
                        var windowWidth = $(window).width();
                        var cntWidth = windowWidth - 90;
                        cGrdQuotation.SetWidth(cntWidth);
                    }

                });
            });

        });
    </script>

    <style>
        /*Rev 3.0*/

        select
        {
            height: 30px !important;
            border-radius: 4px !important;
            /*-webkit-appearance: none;
            position: relative;
            z-index: 1;*/
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
            right: 20px;
            z-index: 0;
            cursor: pointer;
        }

        #FormDate , #toDate , #dtTDate , #InstDate
        {
            position: relative;
            z-index: 1;
            background: transparent;
        }

        .dxeDisabled_PlasticBlue
        {
            z-index: 0 !important;
        }

        #FormDate_B-1 , #toDate_B-1 , #dtTDate_B-1 , #InstDate_B-1
        {
            background: transparent !important;
            border: none;
            width: 30px;
            padding: 10px !important;
        }

        #FormDate_B-1 #FormDate_B-1Img , #toDate_B-1 #toDate_B-1Img , #dtTDate_B-1 #dtTDate_B-1Img , #InstDate_B-1 #InstDate_B-1Img
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

        .TableMain100 #ShowGrid , .TableMain100 #ShowGridList , .TableMain100 #ShowGridRet , .TableMain100 #ShowGridLocationwiseStockStatus 
        
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

        .col-sm-3{
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

        .makeFullscreen >table
        {
            z-index: 0;
        }
        .makeFullscreen .makeFullscreen-icon.half
        {
                z-index: 0;
        }
        /*Rev end 3.0*/
        </style>
     
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--<dxe:ASPxGlobalEvents ID="GlobalEvents" runat="server">
           <ClientSideEvents ControlsInitialized="AllControlInitilize" />
        </dxe:ASPxGlobalEvents>--%>
    <%--Rev 3.0: "outer-div-main" class add --%>
    <div class="outer-div-main clearfix">
        <div class="panel-heading clearfix">
        <div class="panel-title pull-left">
            <h3>Sales Inquiry</h3>
        </div>
        <table class="padTab pull-right" style="margin-top: 7px;">
            <tr>
                <td>
                    <label>From </label>
                </td>
                <%--Rev 3.0: "for-cust-icon" class add --%>
                <td style="width: 150px" class="for-cust-icon">
                    <dxe:ASPxDateEdit ID="FormDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cFormDate" Width="100%" UseMaskBehavior="True">
                        <ButtonStyle Width="13px">
                        </ButtonStyle>
                    </dxe:ASPxDateEdit>
                    <%--Rev 3.0--%>
                    <img src="/assests/images/calendar-icon.png" class="calendar-icon"/>
                    <%--Rev end 3.0--%>
                </td>
                <td>
                    <label>To </label>
                </td>
                <%--Rev 3.0: "for-cust-icon" class add --%>
                <td style="width: 150px" class="for-cust-icon">
                    <dxe:ASPxDateEdit ID="toDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="ctoDate" Width="100%" UseMaskBehavior="True">
                        <ButtonStyle Width="13px">
                        </ButtonStyle>
                    </dxe:ASPxDateEdit>
                    <%--Rev 3.0--%>
                    <img src="/assests/images/calendar-icon.png" class="calendar-icon"/>
                    <%--Rev end 3.0--%>
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
            <a href="javascript:void(0);" onclick="OnAddButtonClick()" class="btn btn-success"><span class="btn-icon"><i class="fa fa-plus" ></i></span><span>New</span> </a><%} %>

            <%--<dxe:ASPxButton ID="btn_Approval" runat="server" class="btn btn-primary" Text="Pending Approval" ClientInstanceName="cbtn_Approval">
                <ClientSideEvents Click="function (s, e) {OpenPopUPApprovalStatus();}" />
            </dxe:ASPxButton>--%>
            <% if (rights.CanExport)
               { %>
            <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-primary btn-radius" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true" OnChange="if(!AvailableExportOption()){return false;}">
                <asp:ListItem Value="0">Export to</asp:ListItem>
                <asp:ListItem Value="1">PDF</asp:ListItem>
                <asp:ListItem Value="2">XLS</asp:ListItem>
                <asp:ListItem Value="3">RTF</asp:ListItem>
                <asp:ListItem Value="4">CSV</asp:ListItem>
            </asp:DropDownList>
            <% } %>

            
            <%--REV 4.0--%>
            <dxe:ASPxButton ID="btnClearFilter" runat="server" Text="Clear Filter"  UseSubmitBehavior="false" CssClass="btn btn-primary " AutoPostBack="False">
            <ClientSideEvents Click="function(s, e) {
            ASPxClientUtils.DeleteCookie('SalesInquiryCookies');
            location.reload(true);
            }" />
            </dxe:ASPxButton>
            <%--REV 4.0 END--%>
           <%-- <dxe:ASPxButton ID="btn_WaitQuote" ClientInstanceName="Cbtn_WaitQuote" runat="server" AutoPostBack="False" Text="Edit" CssClass="btn btn-primary" >
                                            <ClientSideEvents Click="function(s, e) {OpenWaitingQuote();}" />
            </dxe:ASPxButton>--%>
           <%-- <a href="javascript:void(0);" onclick="OpenWaitingQuote()" class="btn btn-warning typeNotificationBtn btn-radius hide"><span><u>Q</u>uotation Waiting </span>
                <span class="typeNotification"><dxe:ASPxLabel runat="server" Text="" ID="lblQuoteweatingCount" ClientInstanceName="ClblQuoteweatingCount"></dxe:ASPxLabel></span>
            </a>--%>
         <%--   <span id="spanStatus" runat="server" class="hide">
                <a href="javascript:void(0);" onclick="OpenPopUPUserWiseQuotaion()" class="btn btn-primary btn-radius">
                    <span>My Quotation Status</span>--%>
                    <%--<asp:Label ID="Label1" runat="server" Text=""></asp:Label>--%>                   
             <%--   </a>
            </span>--%>
            <span id="divPendingWaiting" runat="server">
                <a href="javascript:void(0);" onclick="OpenPopUPApprovalStatus()" class="btn btn-info btn-radius">
                    <span>Approval Waiting</span>
                    <asp:Label ID="lblWaiting" runat="server" Text=""></asp:Label>
                </a>
                <i class="fa fa-reply blink" style="font-size: 20px; margin-right: 10px;" aria-hidden="true"></i>
            </span>
        </div>
    </div>
        <div class="GridViewArea relative">
        <div class="makeFullscreen ">
             <span class="fullScreenTitle">Sales Inquiry</span>
             <span class="makeFullscreen-icon half hovered " data-instance="cGrdQuotation" title="Maximize Grid" id="expandcGrdQuotation">
               <i class="fa fa-expand"></i>
             </span>

        <dxe:ASPxGridView ID="GrdQuotation" runat="server" KeyFieldName="Quote_Id" AutoGenerateColumns="False" SettingsBehavior-AllowFocusedRow="true" Settings-VerticalScrollableHeight="250" Settings-VerticalScrollBarMode="Auto"
            Width="100%" ClientInstanceName="cGrdQuotation" OnCustomCallback="GrdQuotation_CustomCallback" DataSourceID="EntityServerModeDataSource" SettingsDataSecurity-AllowEdit="false" SettingsDataSecurity-AllowInsert="false"
             SettingsDataSecurity-AllowDelete="false" OnHtmlRowPrepared="AvailableStockgrid_HtmlRowPrepared" Settings-HorizontalScrollBarMode="Auto">
           <SettingsSearchPanel Visible="True" Delay="5000" />
             <Columns>
                 <%--Rev Sayantani--%>
                <%--<dxe:GridViewDataTextColumn FieldName="Quote_Id" Visible="false" SortOrder="Descending">
                    <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                </dxe:GridViewDataTextColumn>--%>
                 <dxe:GridViewDataTextColumn FieldName="Quote_Id" Visible="false" ShowInCustomizationForm="false"  VisibleIndex="0">
                    <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                </dxe:GridViewDataTextColumn>
                 <%--End of Rev Sayantani--%> 
                <dxe:GridViewDataTextColumn Caption="Document No." FieldName="QuotationNo"
                    VisibleIndex="1" FixedStyle="Left" Width="150px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Posting Date" FieldName="Quote_Date" 
                    VisibleIndex="2" FixedStyle="Left" Width="100px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <PropertiesTextEdit DisplayFormatString="dd-MM-yyyy"></PropertiesTextEdit>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Customer" FieldName="CustomerName"
                    VisibleIndex="3" FixedStyle="Left">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>

                <dxe:GridViewDataDateColumn Caption="Expiry Date" FieldName="Quote_Expiry"  PropertiesDateEdit-DisplayFormatString="dd-MM-yyyy" PropertiesDateEdit-EditFormatString="dd-MM-yyyy"
                    VisibleIndex="4"  Width="100px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <%--<PropertiesTextEdit DisplayFormatString="dd-MM-yyyy"></PropertiesTextEdit>--%>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataDateColumn>
                <dxe:GridViewDataTextColumn Caption="Ref. Inquiry" FieldName="Inquiry_Reference" 
                    VisibleIndex="5"  Width="150px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>

                  <dxe:GridViewDataTextColumn Caption="Customer Acceptance Status" FieldName="Status" 
                    VisibleIndex="5"  Width="150px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>


                 <dxe:GridViewDataTextColumn Caption="Amount" FieldName="Amount" HeaderStyle-HorizontalAlign="Right"
                    VisibleIndex="6" Width="150px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                    <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                </dxe:GridViewDataTextColumn>
                 <%-- Rev Rajdip --%>
                    <dxe:GridViewDataTextColumn Caption="Status" FieldName="IsClosedStatus" Visible="True"
                    VisibleIndex="7"  Width="150px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                  <dxe:GridViewDataTextColumn Caption="Type" FieldName="Type" Visible="false"
                    VisibleIndex="8"  Width="150px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>

                 <dxe:GridViewDataTextColumn Caption="Bal Quantity" FieldName="BalQty"
                    VisibleIndex="9" Width="0">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                     <Settings AllowAutoFilterTextInputTimer="False" />
                </dxe:GridViewDataTextColumn>

             

                 <dxe:GridViewDataTextColumn Caption="IsClosed" FieldName="IsClosed"
                    VisibleIndex="10" Width="0">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                     <Settings AllowAutoFilterTextInputTimer="False" />
                </dxe:GridViewDataTextColumn>

               <dxe:GridViewDataTextColumn Caption="Project Name" FieldName="Proj_Name"
                    VisibleIndex="12" Width="250px" Settings-ShowFilterRowMenu="True" Settings-AllowAutoFilter="True">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                     <Settings AllowAutoFilterTextInputTimer="True" />
                </dxe:GridViewDataTextColumn>

         <dxe:GridViewDataTextColumn Caption="Revision No." FieldName="RevNo"
                    VisibleIndex="13" Width="150px" Settings-ShowFilterRowMenu="True" Settings-AllowAutoFilter="True">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                     <Settings AllowAutoFilterTextInputTimer="False" />
                </dxe:GridViewDataTextColumn>

                   <dxe:GridViewDataTextColumn Caption="Revision Date" FieldName="RevDate"
                    VisibleIndex="14" Width="150px" Settings-ShowFilterRowMenu="True" Settings-AllowAutoFilter="True">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                     <Settings AllowAutoFilterTextInputTimer="False" />
                </dxe:GridViewDataTextColumn>


                  <dxe:GridViewDataTextColumn Caption="Valid From" FieldName="ProjectValidFrom"
                    VisibleIndex="15" Width="150px" Settings-ShowFilterRowMenu="True">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                     <Settings AllowAutoFilterTextInputTimer="False" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Valid Up To" FieldName="ProjectValidUpto"
                    VisibleIndex="16" Width="150px" Settings-ShowFilterRowMenu="True">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                     <Settings AllowAutoFilterTextInputTimer="False" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Approval Status" FieldName="Project_ApproveStatus"
                    VisibleIndex="17" Width="150px" Settings-ShowFilterRowMenu="True" Settings-AllowAutoFilter="True">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                     <Settings AllowAutoFilterTextInputTimer="False" />
                </dxe:GridViewDataTextColumn>



                <%--  <dxe:GridViewDataTextColumn Caption="Remarks" FieldName="Remarks" Visible="false"
                    VisibleIndex="7" FixedStyle="Left" Width="150px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>--%>
                 <%-- End Rev Rajdip --%>
                <dxe:GridViewDataTextColumn HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="center" VisibleIndex="11" Width="1">
                    <DataItemTemplate>
                        <div class='floatedBtnArea'>
                     
                        <% if (rights.CanView)
                           { %>
                        <a href="javascript:void(0);" onclick="OnViewClick('<%# Container.KeyValue %>')" class="" title="">
                            <span class='ico ColorFour'><i class='fa fa-eye'></i></span><span class='hidden-xs'>View</span></a>
                        <% } %>
                        <% if (rights.CanEdit)
                           { %>
                        <a href="javascript:void(0);" onclick="OnMoreInfoClick('<%# Container.KeyValue %>')" class="" title="" style='<%#Eval("InquiryLastEntry")%>'>
                            
                            <span class='ico editColor'><i class='fa fa-pencil' aria-hidden='true'></i></span><span class='hidden-xs'>Edit</span></a><%} %>

                     <%--mantise issue:0025139--%>
                            <% if (rights.CanPrint)
                                   { %>
                                <a href="javascript:void(0);" onclick="onPrintJv('<%# Container.KeyValue %>')" class="" title="">
                                    <span class='ico ColorSix'><i class='fa fa-print'></i></span><span class='hidden-xs'>Print</span>
                                </a><%} %>
                    <%--End of mantise issue:0025139--%>

                        <% if (rights.CanDelete)
                           { %>
                        <a href="javascript:void(0);" onclick="OnClickDelete('<%# Container.KeyValue %>')" class="" title="" style='<%#Eval("InquiryLastEntry")%>'>
                            <span class='ico deleteColor'><i class='fa fa-trash' aria-hidden='true'></i></span><span class='hidden-xs'>Delete</span></a><%} %>

                                     <% if (rights.CanEdit)
                           { %>
                        <a href="javascript:void(0);" onclick="OnClickStatus('<%# Container.KeyValue %>')" class="" title="">
                            <span class='ico ColorSeven'><i class='fa fa-check'></i></span><span class='hidden-xs'>Status</span></a><%} %>


                            <% if (rights.CanCancel)
                           { %>
                          <a href="javascript:void(0);" onclick="OnCancelClick('<%# Container.KeyValue %>')" class="" title="">
                            
                            <span class='ico deleteColor'><i class='fa fa-times' aria-hidden='true'></i></span><span class='hidden-xs'>Cancel</span>

                        </a><% } %>

                      
                                 <%if (rights.CanClose)
                       { %>
                     <a href="javascript:void(0);" onclick="fn_CloseSalesOrder('<%# Container.KeyValue %>')" title="" class="">
                        <%--  <a href="javascript:void(0);" onclick="fn_Editcity(<%#Eval("sProducts_ID")%>)" title="" class="">--%>
                      <span class='ico deleteColor'><i class='fa fa-times' aria-hidden='true'></i></span><span class='hidden-xs'>Close</span>
                      <%} %>
                     
                           <% if (rights.CanApproved && isApprove)
                           { %>
                          <a href="javascript:void(0);" onclick="OnApproveClick('<%#Eval("Quote_Id")%>',<%# Container.VisibleIndex %>)" class="" title="" style="">
                            
                            <span class='ico editColor'><i class='fa fa-check' aria-hidden='true'></i></span><span class='hidden-xs'>Approve/Reject</span>

                        </a><% } %>

                              <% if (rights.CanView)
                           { %>
                        <a href="javascript:void(0);" onclick="OnclickViewAttachment('<%# Container.KeyValue %>')" class="" title="">
                            <span class='ico ColorFive'><i class='fa fa-share-alt'></i></span><span class='hidden-xs'>Add/View Attachment</span>
                        </a><%} %>
                   
                
                         <%--End Rev Rajdip--%>
                            </div>
                    </DataItemTemplate>
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    <CellStyle HorizontalAlign="Center"></CellStyle>
                    <%-- Rev RAJDIP --%>
                    <%--<HeaderTemplate><span>Actions</span></HeaderTemplate>--%>
                    <EditFormSettings Visible="False"></EditFormSettings>
                    <%--END Rev RAJDIP --%>
                    
                    <Settings AllowAutoFilterTextInputTimer="False" />
                </dxe:GridViewDataTextColumn>
            </Columns>
           <%-- --Rev Sayantani--%>
           <SettingsBehavior ConfirmDelete="true" EnableCustomizationWindow="true" EnableRowHotTrack="true" />
            <SettingsCookies Enabled="true" StorePaging="true" CookiesID="SalesInquiryCookies"  Version="15.8" />
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
        
            <Settings ShowGroupPanel="True" ShowStatusBar="Hidden" ShowHorizontalScrollBar="False" ShowFilterRow="true" ShowFilterRowMenu="true" />
            <SettingsLoadingPanel Text="Please Wait..." />
        </dxe:ASPxGridView>
        <dx:LinqServerModeDataSource ID="EntityServerModeDataSource" runat="server" OnSelecting="EntityServerModeDataSource_Selecting"
            ContextTypeName="ERPDataClassesDataContext" TableName="v_SalesInquiryList" />
        <asp:HiddenField ID="hiddenedit" runat="server" />
    </div></div>
    </div>
        <div style="display: none">
        <dxe:ASPxGridViewExporter ID="exporter" GridViewID="GrdQuotation" runat="server" Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
        </dxe:ASPxGridViewExporter>
    </div>
        <div class="PopUpArea">
        <%--Client Wise Quotation Status Section Start--%>

        <dxe:ASPxPopupControl ID="Popup_QuotationStatus" runat="server" ClientInstanceName="cQuotationStatus"
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
                                <table width="">
                                    <tr>
                                        <td style="padding-right: 8px; vertical-align: top">
                                            <strong>
                                                <label style="margin-bottom: 5px">Sales Inquiry:-</label></strong>
                                        </td>
                                        <td style="padding-top: 0; vertical-align: top; padding-right: 35px">
                                            <%--<dxe:ASPxTextBox ID="txt_Proforma" MaxLength="80" ClientInstanceName="cProforma" TabIndex="1" 
                                                runat="server" Width="100%"> 
                                            </dxe:ASPxTextBox>--%>
                                            <strong>
                                                <dxe:ASPxLabel ID="lbl_Proforma" runat="server" ClientInstanceName="cProforma" Text="ASPxLabel"></dxe:ASPxLabel>
                                            </strong>
                                        </td>
                                        <td style="padding-right: 8px; padding-left: 8px; vertical-align: top">
                                            <strong>
                                                <label style="margin-bottom: 5px">Customer:-</label></strong>
                                        </td>
                                        <td style="padding-top: 0; vertical-align: top">
                                            <%-- <dxe:ASPxTextBox ID="txt_Customer" ClientInstanceName="cCustomer"  runat="server" MaxLength="100" TabIndex="2"
                                            Width="100%"> 
                                        </dxe:ASPxTextBox>--%>
                                            <strong>
                                                <dxe:ASPxLabel ID="lbl_Customer" runat="server" ClientInstanceName="cCustomer" Text="ASPxLabel"></dxe:ASPxLabel>
                                            </strong>
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
                                <td style="width: 70px; padding: 13px 0;"><strong>Status </strong></td>
                                <td>
                                    <asp:RadioButtonList ID="rbl_QuoteStatus" runat="server" Width="172px" CssClass="mTop5" RepeatDirection="Horizontal">
                                        <asp:ListItem Text="Accepted" Value="2" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="Declined" Value="3"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div class="clear"></div>
                    <div class="col-md-12">
                        <div class="" style="margin-bottom: 5px;">
                            <strong>Reason </strong>
                        </div>
                        <div>
                            <dxe:ASPxMemo ID="txt_QuotationRemarks" runat="server" ClientInstanceName="cQuotationRemarks" Height="71px" Width="100%"></dxe:ASPxMemo>
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

        <%--Client Wise Quotation Status Section END--%>

        <%-- Sandip Approval Dtl Section Start--%>


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
                            <dxe:ASPxGridView ID="gridPendingApproval" runat="server" KeyFieldName="Quote_Id" AutoGenerateColumns="False" OnPageIndexChanged="gridPendingApproval_PageIndexChanged"
                                Width="100%" ClientInstanceName="cgridPendingApproval" OnCustomCallback="gridPendingApproval_CustomCallback">
                                <Columns>
                                    <dxe:GridViewDataTextColumn Caption="Proforma/Quotation No." FieldName="Quote_Number"
                                        VisibleIndex="0" FixedStyle="Left">
                                        <CellStyle CssClass="gridcellleft" Wrap="true">
                                        </CellStyle>
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn Caption="Created On" FieldName="Quote_Date"
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
                                                <%-- <ClientSideEvents CheckedChanged="function (s, e) {ch_fnApproved();}" />--%>
                                            </dxe:ASPxCheckBox>
                                        </DataItemTemplate>
                                        <Settings ShowFilterRowMenu="False" AllowFilterBySearchPanel="False" AllowAutoFilter="False" />
                                    </dxe:GridViewDataCheckColumn>

                                    <dxe:GridViewDataCheckColumn UnboundType="Boolean" Caption="Rejected">
                                        <DataItemTemplate>
                                            <dxe:ASPxCheckBox ID="chkreject" runat="server" AllowGrayed="false" OnInit="chkreject_Init" ValueType="System.Boolean" ValueChecked="true" ValueUnchecked="false">
                                                <%-- <ClientSideEvents CheckedChanged="function (s, e) {ch_fnApproved();}" />--%>
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
            Width="900px" HeaderText="User Wise Quotation Status" PopupHorizontalAlign="WindowCenter" HeaderStyle-CssClass="wht"
            PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
            Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
            ContentStyle-CssClass="pad">
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">
                    <div class="row">
                        <div class="col-md-12">
                            <dxe:ASPxGridView ID="gridUserWiseQuotation" runat="server" KeyFieldName="Quote_Id" AutoGenerateColumns="False"
                                Width="100%" ClientInstanceName="cgridUserWiseQuotation" OnCustomCallback="gridUserWiseQuotation_CustomCallback">
                                <Columns>
                                    <dxe:GridViewDataTextColumn Caption="Proforma/Quotation No." FieldName="Quote_Number"
                                        VisibleIndex="0" FixedStyle="Left">
                                        <CellStyle CssClass="gridcellleft" Wrap="true">
                                        </CellStyle>
                                    </dxe:GridViewDataTextColumn>

                                    <dxe:GridViewDataTextColumn Caption="Entered On" FieldName="createddate"
                                        VisibleIndex="1" FixedStyle="Left">
                                        <CellStyle CssClass="gridcellleft" Wrap="true">
                                        </CellStyle>
                                    </dxe:GridViewDataTextColumn>

                                    <dxe:GridViewDataTextColumn Caption="Approval User" FieldName="approvedby"
                                        VisibleIndex="2" FixedStyle="Left">
                                        <CellStyle CssClass="gridcellleft" Wrap="true">
                                        </CellStyle>
                                    </dxe:GridViewDataTextColumn>

                                    <dxe:GridViewDataTextColumn Caption="User Level" FieldName="UserLevel"
                                        VisibleIndex="3" FixedStyle="Left">
                                        <CellStyle CssClass="gridcellleft" Wrap="true">
                                        </CellStyle>
                                    </dxe:GridViewDataTextColumn>

                                    <dxe:GridViewDataTextColumn Caption="Status" FieldName="status"
                                        VisibleIndex="4" FixedStyle="Left">
                                        <CellStyle CssClass="gridcellleft" Wrap="true">
                                        </CellStyle>
                                    </dxe:GridViewDataTextColumn>

                                    <dxe:GridViewDataTextColumn Caption="Approved On" FieldName="ApprovedOn"
                                        VisibleIndex="5" FixedStyle="Left">
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

        <%-- Sandip Approval Dtl Section End--%>
    </div>
        <div class="PopUpArea">

<dxe:ASPxPopupControl ID="popup_QuoteWait" runat="server" Width="1200"
        CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="Cpopup_QuoteWait"
        HeaderText="Quotation Waiting" AllowResize="false" ResizingMode="Postponed">
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">

                <div onkeypress="OnWaitingGridKeyPress(event)">

                    <dxe:ASPxGridView ID="watingQuotegrid" runat="server" KeyFieldName="SBMain_Id" AutoGenerateColumns="False"
                        Width="100%" ClientInstanceName="CwatingQuotegrid" OnCustomCallback="watingQuotegrid_CustomCallback" KeyboardSupport="true"
                        SettingsBehavior-AllowSelectSingleRowOnly="true" OnDataBinding="watingQuotegrid_DataBinding" SettingsBehavior-AllowFocusedRow="true"
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

                        <ClientSideEvents RowClick="ListRowClicked" EndCallback="watingQuotegridEndCallback" />

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
                    ClientSideEvents-Click="QuotationWattingOkClick" UseSubmitBehavior="False" />
            </dxe:PopupControlContentControl>
        </ContentCollection>
    </dxe:ASPxPopupControl>

    </div>
     <dxe:ASPxTimer runat="server" Interval="10000" ClientInstanceName="Timer1">
        <ClientSideEvents Tick="timerTick" />
    </dxe:ASPxTimer>
    <asp:HiddenField ID="waitingQuotationCount" runat="server" />
    <div class="PopUpArea">
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

    <dxe:ASPxGlobalEvents ID="GlobalEvents" runat="server">
        <ClientSideEvents ControlsInitialized="AllControlInitilize" />
    </dxe:ASPxGlobalEvents>

    <div>
        <asp:HiddenField ID="hfIsFilter" runat="server" />
        <asp:HiddenField ID="hfFromDate" runat="server" />
        <asp:HiddenField ID="hfToDate" runat="server" />
        <asp:HiddenField ID="hfBranchID" runat="server" />
        <asp:HiddenField ID="hfIsUserwise" runat="server" />
    </div>
    <%-- Rev Rajdip --%>
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
    <asp:HiddenField ID="hddnKeyValue" runat="server" />
    <%--End Rev Rajdip --%>

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

      <dxe:ASPxCallbackPanel runat="server" ID="CallbackPanel" ClientInstanceName="cCallbackPanel" OnCallback="CallbackPanel_Callback">
        <PanelCollection>
            <dxe:PanelContent runat="server">           
            </dxe:PanelContent>
        </PanelCollection>
        <ClientSideEvents EndCallback="CallbackPanelEndCall" />
    </dxe:ASPxCallbackPanel>
    <asp:HiddenField ID="hFilterType" runat="server" />

</asp:Content>
