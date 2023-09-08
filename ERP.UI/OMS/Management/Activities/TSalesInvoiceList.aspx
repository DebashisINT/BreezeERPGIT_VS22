<%--==========================================================Revision History ============================================================================================   
   1.0   Priti     V2.0.36     10-01-2023      0025324: Views to be converted to Procedures in the Listing Page of Transaction / Transit Sales/Purchase / Sales Invoice
   2.0   Priti     V2.0.36     17-02-2023      After Listing view upgradation delete data show in listing issue solved.
   3.0   Pallab    V2.0.38     16-05-2023      26142: Transit Sales Invoice module design modification & check in small device
   4.0   Priti     V2.0.39     06-09-2023      0026790:Action Button "Update Transporter" required in Transit Sales Invoice module
========================================== End Revision History =======================================================================================================--%>
<%@ Page Title="Transit Sales Invoice" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true"  EnableEventValidation="false"
    CodeBehind="TSalesInvoiceList.aspx.cs" Inherits="ERP.OMS.Management.Activities.TSalesInvoiceList" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>
<%--Rev 4.0--%>
<%@ Register Src="~/OMS/Management/Activities/UserControls/VehicleDetailsControl.ascx" TagPrefix="uc1" TagName="VehicleDetailsControl" %>
<%--Rev 4.0 End--%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--Filteration Section Start By Sam--%>
    <script src="JS/TransitSalesInvoice.js?v1.1"></script>
    <script type="text/javascript" src="../../CentralData/JSScript/GenericJScript.js"></script>
    <%-- Filteration Section Start By Sam--%>
    <%--Code Added By Sandip For Approval Detail Section Start--%>
    <script>
        /*  Rev 4.0*/
        function UpdateTransporter(values) {
            $("#hddnInvoiceID").val(values);
            callTransporterControl(values, "TSI");
            $("#exampleModal").modal('show');
        }
        function InsertTransporterControlDetails(data) {
            var InvoiceID = $("#hddnInvoiceID").val();
            $.ajax({
                type: "POST",
                url: "TSalesInvoiceList.aspx/InsertTransporterControlDetails",
                data: "{'id':'" + InvoiceID + "','hfControlData':'" + data + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (msg) {

                }
            });
        }
        /*  Rev 4.0 End*/
        function OnEWayBillClick(id, VisibleIndex, EWayBillNumber, EWayBillDate, EWayBillValue, TransDate) {

            var ActiveEInvoice = $('#hdnActiveEInvoice').val();
            if (ActiveEInvoice == "1") {
               // if ((new Date(TransDate)) >= (new Date("01-01-2021"))) {
                var dateParts = TransDate.split("-");
                if ((new Date(+dateParts[2], dateParts[1] - 1, +dateParts[0])) >= (new Date("01-01-2021"))) {
                    $.ajax({
                        type: "POST",
                        url: "TSalesInvoiceList.aspx/GetEditablePermissionFromEInvoice",
                        data: "{'TSalesInvoiceID':'" + id + "','Action':'EWayBill'}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (msg) {
                            var status = msg.d;
                            if (status == "Yes") {
                                $("#btnEWayBillSave").removeClass('hide');
                                $("#lblEwayBillStatus").text("");
                            }
                            else {
                                $("#lblEwayBillStatus").text("IRN not generated can not update.");
                                ;
                                //jAlert("IRN not generated can not print.");
                                $("#btnEWayBillSave").addClass('hide');
                            }
                        }
                    });
                }
            }

           // cgrid.SetFocusedRowIndex(VisibleIndex);
            if (EWayBillNumber.trim() != "") {
                ctxtEWayBillNumber.SetText(EWayBillNumber);
            }
            else {
                ctxtEWayBillNumber.SetText("");
            }

            if (EWayBillDate.trim() != "" && EWayBillDate.trim() != "01-01-1970" && EWayBillDate.trim() != "01-01-1900") {
                cdt_EWayBill.SetText(EWayBillDate);
            }
            else {
                cdt_EWayBill.SetText("");
            }
            if (EWayBillValue.trim() != "0.00" && EWayBillValue.trim() != "") {
                ctxtEWayBillValue.SetText(EWayBillValue);
            }
            else {
                ctxtEWayBillValue.SetText("0.0");
            }


            ctxtTransporterGSTIN.SetText('');
            ctxtTransporterName.SetText('');
            $("#ddlTransportationMode").val('0');
            ctxtTransportationDistance.SetText('0');
            ctxtTransporterDocNo.SetText('');
            ctxtVehicleNo.SetText('');
            $("#ddlVehicleType").val('0');

            $.ajax({
                type: "POST",
                url: "TSalesInvoiceList.aspx/EditEWayBill",
                data: JSON.stringify({
                    DocID: id
                }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (msg) {
                    var status = msg.d[0];
                    console.log(msg);
                    if (status != "") {
                        console.log(status);
                        ctxtTransporterGSTIN.SetText(status.TransporterGSTIN);
                        ctxtTransporterName.SetText(status.TransporterName);
                        if (status.Transporter_Mode != '') {
                            $("#ddlTransportationMode").val(status.Transporter_Mode);
                        }
                        if (status.Transporter_Distance != '') {
                            ctxtTransportationDistance.SetText(status.Transporter_Distance);
                        }
                        if (status.Transporter_DocNo != '') {
                            ctxtTransporterDocNo.SetText(status.Transporter_DocNo);
                        }
                        if (status.Vehicle_No != '') {
                            ctxtVehicleNo.SetText(status.Vehicle_No);
                        }
                        if (status.Vehicle_type != '') {
                            $("#ddlVehicleType").val(status.Vehicle_type);
                        }
                    }
                }
            });

            $('#hddnInvoiceID').val(id);
            cPopup_EWayBill.Show();
            ctxtEWayBillNumber.Focus();
        }
        function GetEWayBillDateFormat(today) {
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
                today = yyyy + '-' + mm + '-' + dd;
            }

            return today;
        }
        function CallEWayBill_save() {

            var InvoiceID = $("#<%=hddnInvoiceID.ClientID%>").val();
            //if (ctxtEWayBillNumber.GetValue() == null) {                
            //    jAlert("Please enter E-Way Bill Number.");
            //        return false;               
            //}
            //else
            //{
            var UpdateEWayBill = ctxtEWayBillNumber.GetValue();
            if (UpdateEWayBill == "0") {
                UpdateEWayBill = "";
            }
            if (cdt_EWayBill.GetValue() == "" && cdt_EWayBill.GetValue() == null) {
                var EWayBillDate = "1990-01-01";
            }
            else {
                var EWayBillDate = GetEWayBillDateFormat(new Date(cdt_EWayBill.GetValue()));
            }

            var EWayBillValue = ctxtEWayBillValue.GetValue();

            //Rev Extra column Tanmoy
            var TransporterGSTIN = ctxtTransporterGSTIN.GetText();
            var TransporterName = ctxtTransporterName.GetText();
            var TransportationMode = $("#ddlTransportationMode").val();
            var TransportationDistance = ctxtTransportationDistance.GetText();
            var TransporterDocNo = ctxtTransporterDocNo.GetText();
            var TransporterDocDate = null;
            if (cdt_TransporterDocDate.GetValue() == "" && cdt_TransporterDocDate.GetValue() == null) {
                var TransporterDocDate = "1990-01-01";
            }
            else {
                if (cdt_TransporterDocDate.GetValue() == null) {
                    var TransporterDocDate = null;
                }
                else {
                    var TransporterDocDate = GetEWayBillDateFormat(new Date(cdt_TransporterDocDate.GetValue()));
                }
            }
            var VehicleNo = ctxtVehicleNo.GetText();
            var VehicleType = $("#ddlVehicleType").val();
            //End of Rev

            $.ajax({
                type: "POST",
                url: "TSalesInvoiceList.aspx/UpdateEWayBill",
                data: JSON.stringify({
                    InvoiceID: InvoiceID, UpdateEWayBill: UpdateEWayBill, EWayBillDate: EWayBillDate, EWayBillValue: EWayBillValue, TransporterGSTIN: TransporterGSTIN
                    , TransporterName: TransporterName, TransportationMode: TransportationMode, TransportationDistance: TransportationDistance, TransporterDocNo: TransporterDocNo
                    , TransporterDocDate: TransporterDocDate, VehicleNo: VehicleNo, VehicleType: VehicleType
                }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (msg) {
                    var status = msg.d;
                    if (status == "1") {
                        jAlert("Saved successfully.");
                        //ctxtEWayBillNumber.SetText("");
                        cPopup_EWayBill.Hide();
                        cgrid.Refresh();
                    }
                    else if (status == "-10") {
                        jAlert("Data not saved.");
                        cPopup_EWayBill.Hide();
                    }
                }
            });
            //}           
        }
        function CancelEWayBill_save() {
            cPopup_EWayBill.Hide();
        }


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

        }
        function OnGetApprovedRowValues(obj) {
            uri = "TSalesInvoice.aspx?key=" + obj + "&status=2" + '&type=SI';
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
            uri = "TSalesInvoice.aspx?key=" + obj + "&status=3" + '&type=SI';
            popup.SetContentUrl(uri);
            popup.Show();
        }
        // function above  End For Rejected

        // To Reflect the Data in Pending Waiting Grid and Pending Waiting Counting if the user approve or Rejecte the Order and Saved 

        function OnApprovalEndCall(s, e) {
            $.ajax({
                type: "POST",
                url: "TSalesInvoiceList.aspx/GetPendingCase",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    $('#<%= lblWaiting.ClientID %>').text(data.d);
                }
            });
            }

            // function above  End 

    </script>
    <%-- Code Added By Sandip For Approval Detail Section End--%>
    <script>
        var InvoiceId = 0;
        function onPrintJv(id,TransDate) {
            var ActiveEInvoice = $('#hdnActiveEInvoice').val();
            if (ActiveEInvoice == "1") {
                // Mantis Issue 23810 (14/05/2021)
                // if ((new Date(TransDate)) >= (new Date("01-01-2021"))) {
                var dateParts = TransDate.split("-");
                if ((new Date(+dateParts[2], dateParts[1] - 1, +dateParts[0])) >= (new Date("01-01-2021"))) {
                    // End of Mantis Issue 23810 (14/05/2021)
                    $.ajax({
                        type: "POST",
                        url: "TSalesInvoiceList.aspx/GetEditablePermissionFromEInvoice",
                        data: "{'TSalesInvoiceID':'" + id + "','Action':'Print'}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (msg) {
                            var status = msg.d;
                            if (status == "Yes") {

                                InvoiceId = id;
                                cSelectPanel.cpSuccess = "";
                                cDocumentsPopup.Show();
                                CselectOriginal.SetCheckState('UnChecked');
                                CselectDuplicate.SetCheckState('UnChecked');
                                CselectTriplicate.SetCheckState('UnChecked');
                                CselectOfficecopy.SetCheckState('UnChecked');
                                cCmbDesignName.SetSelectedIndex(0);
                                cSelectPanel.PerformCallback('Bindalldesignes');
                                $('#btnOK').focus();

                            } else {
                                jAlert("IRN not generated can not print.");
                            }
                        }
                    });
                }
                else {
                    InvoiceId = id;
                    cSelectPanel.cpSuccess = "";
                    cDocumentsPopup.Show();
                    CselectOriginal.SetCheckState('UnChecked');
                    CselectDuplicate.SetCheckState('UnChecked');
                    CselectTriplicate.SetCheckState('UnChecked');
                    CselectOfficecopy.SetCheckState('UnChecked');
                    cCmbDesignName.SetSelectedIndex(0);
                    cSelectPanel.PerformCallback('Bindalldesignes');
                    $('#btnOK').focus();
                }
            }
            else
            {
                InvoiceId = id;
                cSelectPanel.cpSuccess = "";
                cDocumentsPopup.Show();
                CselectOriginal.SetCheckState('UnChecked');
                CselectDuplicate.SetCheckState('UnChecked');
                CselectTriplicate.SetCheckState('UnChecked');
                CselectOfficecopy.SetCheckState('UnChecked');
                cCmbDesignName.SetSelectedIndex(0);
                cSelectPanel.PerformCallback('Bindalldesignes');
                $('#btnOK').focus();
            }
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
                var module = 'TSInvoice';
                if (TotDocument.length > 0) {
                    for (var i = 0; i < TotDocument.length; i++) {
                        if (TotDocument[i] != "") {
                            window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + reportName + '&modulename=' + module + '&id=' + InvoiceId + '&PrintOption=' + TotDocument[i], '_blank')
                        }
                    }
                }
            }
            if (cSelectPanel.cpSuccess == "") {
                if (cSelectPanel.cpChecked != "") {
                    jAlert('Please check Original For Recipient and proceed.');
                }
                CselectOriginal.SetCheckState('UnChecked');
                CselectDuplicate.SetCheckState('UnChecked');
                CselectTriplicate.SetCheckState('UnChecked');
                CselectOfficecopy.SetCheckState('UnChecked');
                cCmbDesignName.SetSelectedIndex(0);
            }
        }

        function OnclickViewAttachment(obj) {
            //var URL = '/OMS/Management/Activities/SalesInvoice_Document.aspx?idbldng=' + obj + '&type=SalesInvoice';
            var URL = '/OMS/Management/Activities/EntriesDocuments.aspx?idbldng=' + obj + '&type=TransitSalesInvoice';
            window.location.href = URL;
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
        function OnAddButtonClick() {
            var url = 'TSalesInvoice.aspx?key=' + 'ADD';
            window.location.href = url;
        }
        function OnMoreInfoClick(keyValue,TransDate) {

            var ActiveEInvoice = $('#hdnActiveEInvoice').val();
            if (ActiveEInvoice == "1") {
                // if ((new Date(TransDate)) >= (new Date("01-01-2021"))) {
                var dateParts = TransDate.split("-");
                if ((new Date(+dateParts[2], dateParts[1] - 1, +dateParts[0])) >= (new Date("01-01-2021"))) {
                    $.ajax({
                        type: "POST",
                        url: "TSalesInvoiceList.aspx/GetEditablePermissionFromEInvoice",
                        data: "{'TSalesInvoiceID':'" + keyValue + "','Action':'Edit'}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (msg) {
                            var status = msg.d;
                            if (status == "Yes") {

                                var ActiveUser = '<%=Session["userid"]%>'
                                if (ActiveUser != null) {
                                    $.ajax({
                                        type: "POST",
                                        url: "TSalesInvoiceList.aspx/GetEditablePermission",
                                        data: "{'ActiveUser':'" + ActiveUser + "'}",
                                        contentType: "application/json; charset=utf-8",
                                        dataType: "json",
                                        success: function (msg) {
                                            var status = msg.d;
                                            var url = 'TSalesInvoice.aspx?key=' + keyValue + '&Permission=' + status + '&type=TSI';
                                            window.location.href = url;
                                        }
                                    });
                                }
                            }
                            else {
                                jAlert("IRN generated can not edit.");
                            }
                        }
                    });
                }
                else
                {
                    var ActiveUser = '<%=Session["userid"]%>'
                    if (ActiveUser != null) {
                        $.ajax({
                            type: "POST",
                            url: "TSalesInvoiceList.aspx/GetEditablePermission",
                            data: "{'ActiveUser':'" + ActiveUser + "'}",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (msg) {
                                var status = msg.d;
                                var url = 'TSalesInvoice.aspx?key=' + keyValue + '&Permission=' + status + '&type=TSI';
                                window.location.href = url;
                            }
                        });
                    }
                }

            }
            else
            {
                var ActiveUser = '<%=Session["userid"]%>'
                if (ActiveUser != null) {
                    $.ajax({
                        type: "POST",
                        url: "TSalesInvoiceList.aspx/GetEditablePermission",
                        data: "{'ActiveUser':'" + ActiveUser + "'}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (msg) {
                            var status = msg.d;
                            var url = 'TSalesInvoice.aspx?key=' + keyValue + '&Permission=' + status + '&type=TSI';
                            window.location.href = url;
                        }
                    });
                }
            }
        }

        ////##### coded by Samrat Roy - 04/05/2017  
        ////Add an another param to define request type 
        function OnViewClick(keyValue) {
            var url = 'TSalesInvoice.aspx?key=' + keyValue + '&req=V' + '&type=TSI';
            window.location.href = url;
        }

        function OnClickDelete(keyValue,TransDate) {

            var ActiveEInvoice = $('#hdnActiveEInvoice').val();
            if (ActiveEInvoice == "1") {
                // if ((new Date(TransDate)) >= (new Date("01-01-2021"))) {
                var dateParts = TransDate.split("-");
                if ((new Date(+dateParts[2], dateParts[1] - 1, +dateParts[0])) >= (new Date("01-01-2021"))) {
                    $.ajax({
                        type: "POST",
                        url: "TSalesInvoiceList.aspx/GetEditablePermissionFromEInvoice",
                        data: "{'TSalesInvoiceID':'" + keyValue + "','Action':'Delete'}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (msg) {
                            var status = msg.d;
                            if (status == "Yes") {
                                jConfirm('Confirm delete?', 'Confirmation Dialog', function (r) {
                                    if (r == true) {
                                        cgrid.PerformCallback('Delete~' + keyValue);
                                    }
                                });
                            }
                            else {
                                jAlert("IRN generated can not delete.");
                            }
                        }
                    });
                }
                else
                {
                    jConfirm('Confirm delete?', 'Confirmation Dialog', function (r) {
                        if (r == true) {
                            cgrid.PerformCallback('Delete~' + keyValue);
                        }
                    });
                }
            }
            else {
                jConfirm('Confirm delete?', 'Confirmation Dialog', function (r) {
                    if (r == true) {
                        cgrid.PerformCallback('Delete~' + keyValue);
                    }
                });
            }
        }

        function grid_EndCallBack() {
            if (cgrid.cpDelete != null) {
                jAlert(cgrid.cpDelete);
                cgrid.cpDelete = null;
                updateGridAfterDelete();
                //cgrid.cpDelete == null;
            }
        }
        function gridRowclick(s, e) {
            $('#GrdQuotation').find('tr').removeClass('rowActive');
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
    
    <script type="text/javascript">
        $(document).ready(function () {
            if ($('body').hasClass('mini-navbar')) {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 90;
                cgrid.SetWidth(cntWidth);
            } else {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 220;
                cgrid.SetWidth(cntWidth);
            }
            $('.navbar-minimalize').click(function () {
                if ($('body').hasClass('mini-navbar')) {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 220;
                    cgrid.SetWidth(cntWidth);
                } else {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 90;
                    cgrid.SetWidth(cntWidth);
                }

            });
        });
    </script>
    <link href="CSS/TSalesInvoiceList.css" rel="stylesheet" />
    <%--Rev 3.0--%>
    <link href="/assests/css/custom/newcustomstyle.css" rel="stylesheet" />
    
    <style>
        select
        {
            z-index: 0;
        }

        #GrdQuotation {
            max-width: 99% !important;
        }
        #FormDate, #toDate, #dtTDate, #dt_PLQuote, #dt_PlQuoteExpiry {
            position: relative;
            z-index: 1;
            background: transparent;
        }

        select
        {
            -webkit-appearance: auto;
        }

        .calendar-icon
        {
            right: 20px;
        }

        .panel-title h3
        {
            padding-top: 0px !important;
        }

        .fakeInput
        {
                min-height: 30px;
    border-radius: 4px;
        }
        
    </style>
    <%--Rev end 3.0--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--Rev 3.0: "outer-div-main" class add --%>
    <div class="outer-div-main clearfix">
        <div class="panel-heading">
        <div class="panel-title clearfix">
            <h3 class="pull-left">Transit Sales Invoice</h3>
        </div>
        <table class="padTab pull-right" style="margin-top:5px;">
            <tr>
                <td>From </td>
                <%--Rev 3.0: "for-cust-icon" class add --%>
                <td style="width: 150px" class="for-cust-icon">
                    <dxe:ASPxDateEdit ID="FormDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cFormDate" Width="100%">
                        <ButtonStyle Width="13px">
                        </ButtonStyle>
                    </dxe:ASPxDateEdit>
                    <%--Rev 3.0--%>
                    <img src="/assests/images/calendar-icon.png" class="calendar-icon"/>
                    <%--Rev end 3.0--%>
                </td>
                <td>To 
                </td>
                <%--Rev 3.0: "for-cust-icon" class add --%>
                <td style="width: 150px" class="for-cust-icon">
                    <dxe:ASPxDateEdit ID="toDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="ctoDate" Width="100%">
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
                    <input type="button" value="Show" class="btn btn-primary" onclick="updateTSIGridByDate()" />
                    <%--<input type="button" value="Clear" class="btn btn-primary" onclick="ClearField()" />--%>
                </td>
            </tr>
        </table>

    </div>
        <%--Code Added by Sam For Filteration Section Start--%>

        <%--Code Added by Sam For Filteration Section Start--%>
        <div class="form_main">
        <div class="clearfix">
            <% if (rights.CanAdd)
               { %>
            <a href="javascript:void(0);" onclick="OnAddButtonClick()" class="btn btn-success "><span class="btn-icon"><i class="fa fa-plus" ></i></span><span><u>A</u>dd New</span> </a><%} %>

            <% if (rights.CanExport)
               { %>
            <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-primary " OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true" OnChange="if(!AvailableExportOption()){return false;}">
                <asp:ListItem Value="0">Export to</asp:ListItem>
                <asp:ListItem Value="1">PDF</asp:ListItem>
                <asp:ListItem Value="2">XLS</asp:ListItem>
                <asp:ListItem Value="3">RTF</asp:ListItem>
                <asp:ListItem Value="4">CSV</asp:ListItem>
            </asp:DropDownList>
            <% } %>
            <%--Sandip Section for Approval Section in Design Start --%>

            <span id="spanStatus" runat="server">
                <a href="javascript:void(0);" onclick="OpenPopUPUserWiseQuotaion()" class="btn btn-primary btn-radius">
                    <span>My Sales Invoice Status</span>
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
        <div class="GridViewArea relative">
        <dxe:ASPxGridView ID="GrdQuotation" runat="server" KeyFieldName="Invoice_Id" AutoGenerateColumns="False" SettingsBehavior-AllowFocusedRow="true" OnPageIndexChanged="GrdQuotation_PageIndexChanged"
            Width="100%" ClientInstanceName="cgrid" OnCustomCallback="GrdQuotation_CustomCallback" OnDataBinding="GrdQuotation_DataBinding"
            DataSourceID="EntityServerModeDataSource" SettingsDataSecurity-AllowEdit="false" SettingsDataSecurity-AllowInsert="false" 
            SettingsDataSecurity-AllowDelete="false" Settings-VerticalScrollableHeight="280" Settings-VerticalScrollBarMode="Auto" Settings-HorizontalScrollBarMode="Auto">

            <SettingsSearchPanel Visible="True" Delay="5000" />
            <Columns>
                <dxe:GridViewDataTextColumn Caption="Document No." FieldName="InvoiceNo"
                    VisibleIndex="0" FixedStyle="Left" Width="150px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Posting Date" FieldName="InvoiceDt" PropertiesTextEdit-DisplayFormatString="dd-MM-yyyy"
                    VisibleIndex="1" FixedStyle="Left" Width="100px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Customer" FieldName="CustomerName"
                    VisibleIndex="2" FixedStyle="Left" Width="200px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Project Name" FieldName="Proj_Name"
                    VisibleIndex="2" FixedStyle="Left" Width="160px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                  <dxe:GridViewDataTextColumn Caption="Unit" FieldName="Unit"
                    VisibleIndex="3" FixedStyle="Left" Width="200px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Gross Amount" FieldName="GrossAmount"
                    VisibleIndex="4" FixedStyle="Left" Width="100px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                      <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Tax & Charges" FieldName="ChargesAmount"
                    VisibleIndex="5" FixedStyle="Left" Width="100px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                      <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Net Amount" FieldName="NetAmount"
                    VisibleIndex="6" FixedStyle="Left" Width="130px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                      <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="E-Way Bill No." FieldName="EWayBillNumber" VisibleIndex="7" Width="100px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                 <dxe:GridViewDataTextColumn Caption="Place of Supply[GST]" FieldName="PosState" VisibleIndex="8" Width="100px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="IRN ?" FieldName="IsIRN" VisibleIndex="9" Width="100px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                  <dxe:GridViewDataTextColumn Caption="IRN" FieldName="IRN" VisibleIndex="10" Width="100px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Ack No" FieldName="AckNo" VisibleIndex="11" Width="100px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Ack Date" FieldName="AckDt" VisibleIndex="12" Width="100px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="center" VisibleIndex="13" Width="1px">
                    <DataItemTemplate>
                        <div class='floatedBtnArea'>
                        <% if (rights.CanView)
                           { %>
                        <a href="javascript:void(0);" onclick="OnViewClick('<%# Container.KeyValue %>')" class="" title="">
                            <span class='ico ColorFive'><i class='fa fa-eye'></i></span><span class='hidden-xs'>View</span></a>
                        <% } %>
                        <% if (rights.CanEdit)
                           { %>
                        <a href="javascript:void(0);" onclick="OnMoreInfoClick('<%# Container.KeyValue %>','<%#Eval("InvoiceDt") %>')" class="" title="">
                            <span class='ico editColor'><i class='fa fa-pencil' aria-hidden='true'></i></span><span class='hidden-xs'>Edit</span></a><%} %>
                        <% if (rights.CanDelete)
                           { %>
                        <a href="javascript:void(0);" onclick="OnClickDelete('<%# Container.KeyValue %>','<%#Eval("InvoiceDt") %>')" class="" title="">
                            <span class='ico deleteColor'><i class='fa fa-trash' aria-hidden='true'></i></span><span class='hidden-xs'>Delete</span></a><%} %>
                        <%-- <a href="javascript:void(0);" onclick="OnClickCopy('<%# Container.KeyValue %>')" class="pad" title="Copy ">
                            <i class="fa fa-copy"></i></a>--%>
                        <% if (rights.CanView)
                           { %>
                        <a href="javascript:void(0);" onclick="OnclickViewAttachment('<%# Container.KeyValue %>')" class="" title="">
                            <span class='ico ColorSix'><i class='fa fa-paperclip'></i></span><span class='hidden-xs'>Add/View Attachment</span>
                        </a><%} %>
                        <a href="javascript:void(0);" onclick="OnEWayBillClick('<%# Container.KeyValue %>','<%# Container.VisibleIndex %>','<%#Eval("EWayBillNumber") %>','<%#Eval("EWayBillDate") %>','<%#Eval("EWayBillValue") %>','<%#Eval("InvoiceDt") %>')" class="" title="">
                            <span class='ico ColorFour'><i class='fa fa-file-text-o'></i></span><span class='hidden-xs'>Update E-Way Bill</span>
                            <% if (rights.CanPrint)
                               { %>
                            <a href="javascript:void(0);" onclick="onPrintJv('<%# Container.KeyValue %>','<%#Eval("InvoiceDt") %>')" class="" title="">
                                <span class='ico ColorSeven'><i class='fa fa-print'></i></span><span class='hidden-xs'>Print</span>
                            </a><%} %>
                        <%--REV 4.0--%>
                       <%-- <% if (rights.CanUpdateTransporter)
                            { %>--%>
                        <a href="javascript:void(0);" onclick="UpdateTransporter('<%# Container.KeyValue %>')" class="" title="" >
                            <span class='ico editColor'><i class='fa fa-pencil' aria-hidden='true'></i></span><span class='hidden-xs'>Update Transporter</span></a>
                      <%--  <% } %>--%>
                        <%--REV 4.0 END--%>
                        </div>
                    </DataItemTemplate>
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    <CellStyle HorizontalAlign="Center"></CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <HeaderTemplate><span></span></HeaderTemplate>
                    <EditFormSettings Visible="False"></EditFormSettings>
                </dxe:GridViewDataTextColumn>
            </Columns>
            <SettingsContextMenu Enabled="true"></SettingsContextMenu>
            <ClientSideEvents EndCallback="function (s, e) {grid_EndCallBack();}" RowClick="gridRowclick" />
            <SettingsPager NumericButtonCount="10" PageSize="10" ShowSeparators="True" Mode="ShowPager">
                <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
                <FirstPageButton Visible="True">
                </FirstPageButton>
                <LastPageButton Visible="True">
                </LastPageButton>
            </SettingsPager>
             <SettingsCookies Enabled="true" StorePaging="true" Version="2.0" />
            <GroupSummary>
                <dxe:ASPxSummaryItem FieldName="GrossAmount" SummaryType="Sum" DisplayFormat="Total Gross Amount : {0}" />
                <dxe:ASPxSummaryItem FieldName="ChargesAmount" SummaryType="Sum" DisplayFormat="Total Tax & Charges : {0}" />
                <dxe:ASPxSummaryItem FieldName="NetAmount" SummaryType="Sum" DisplayFormat="Total Net Amount : {0}" />
                <dxe:ASPxSummaryItem FieldName="AmountReceived" SummaryType="Sum" DisplayFormat="Total Amount Received : {0}" />
                <dxe:ASPxSummaryItem FieldName="BalanceAmount" SummaryType="Sum" DisplayFormat="Total Balance Amount : {0}" />
            </GroupSummary>
            <%--<SettingsSearchPanel Visible="True" />--%>
            <Settings ShowGroupPanel="True" ShowStatusBar="Hidden" ShowHorizontalScrollBar="False" ShowFilterRow="true" ShowFilterRowMenu="true" />
            <SettingsLoadingPanel Text="Please Wait..." />
        </dxe:ASPxGridView>
        <asp:HiddenField ID="hiddenedit" runat="server" />
    </div>
    </div>
    <dx:LinqServerModeDataSource ID="EntityServerModeDataSource" runat="server" OnSelecting="EntityServerModeDataSource_Selecting"
        ContextTypeName="ERPDataClassesDataContext" TableName="v_TransitSBList" />
    <div style="display: none">
        <dxe:ASPxGridViewExporter ID="exporter" GridViewID="GrdQuotation" runat="server" Landscape="true" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
        </dxe:ASPxGridViewExporter>
    </div>
     <%--  REV 4.0--%>
    <uc1:VehicleDetailsControl runat="server" ID="VehicleDetailsControl" />
     <%--  REV 4.0 END--%>
  <%--  REV 1.0--%>
     <dxe:ASPxCallbackPanel runat="server" ID="CallbackPanel" ClientInstanceName="cCallbackPanel" OnCallback="CallbackPanel_Callback">
        <PanelCollection>
            <dxe:PanelContent runat="server">           
            </dxe:PanelContent>
        </PanelCollection>
        <ClientSideEvents EndCallback="CallbackPanelEndCall" />
    </dxe:ASPxCallbackPanel>
    <%--END REV 1.0--%>
    <%--DEBASHIS--%>
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
                                Width="100%" ClientInstanceName="cgridPendingApproval" OnCustomCallback="gridPendingApproval_CustomCallback">
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
            Width="900px" HeaderText="User Wise Sales Invoice Status" PopupHorizontalAlign="WindowCenter" HeaderStyle-CssClass="wht"
            PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
            Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
            ContentStyle-CssClass="pad">
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">
                    <div class="row">
                        <div class="col-md-12">
                            <dxe:ASPxGridView ID="gridUserWiseQuotation" runat="server" KeyFieldName="ID" AutoGenerateColumns="False"
                                Width="100%" ClientInstanceName="cgridUserWiseQuotation" OnCustomCallback="gridUserWiseQuotation_CustomCallback">
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


    <dxe:ASPxGlobalEvents ID="GlobalEvents" runat="server">
        <ClientSideEvents ControlsInitialized="AllControlInitilize" />
    </dxe:ASPxGlobalEvents>
    <%-- Sandip Approval Dtl Section End--%>

    <div>
        <asp:HiddenField ID="hfIsFilter" runat="server" />
        <asp:HiddenField ID="hfFromDate" runat="server" />
        <asp:HiddenField ID="hfToDate" runat="server" />
        <asp:HiddenField ID="hfBranchID" runat="server" />
        <asp:HiddenField ID="hddnInvoiceID" runat="server" />
        <asp:HiddenField ID="hfIsUserwise" runat="server" />
        <asp:HiddenField runat="server" ID="hdnActiveEInvoice" />
           <asp:HiddenField ID="hFilterType" runat="server" />
    </div>

    <dxe:ASPxPopupControl ID="Popup_EWayBill" runat="server" ClientInstanceName="cPopup_EWayBill"
        Width="400px" HeaderText="Update E-Way Bill" PopupHorizontalAlign="WindowCenter"
        BackColor="white" Height="150px" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True">
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">

                <div class="Top clearfix">

                    <table style="width: 100%; margin: 0 auto; margin-top: 5px;">

                        <tr>
                           <label> <dxe:ASPxLabel ID="ASPxLabel12" runat="server" Text="E-Way Bill Number">
                            </dxe:ASPxLabel></label>

                            <dxe:ASPxTextBox ID="txtEWayBillNumber" MaxLength="12" ClientInstanceName="ctxtEWayBillNumber"
                                runat="server" Width="100%">
                                <%--Rev work start 19.07.2022 mantise no:0025047: Allow to add Alphabetical values like "NA" in the eway Bill No field og the Transit Sales Invoice Module--%>
                                <%--<MaskSettings Mask="<0..999999999999>" AllowMouseWheel="false" />--%>
                                <%--Rev work close 19.07.2022 mantise no:0025047: Allow to add Alphabetical values like "NA" in the eway Bill No field og the Transit Sales Invoice Module--%>
                                <ValidationSettings RequiredField-IsRequired="false" ErrorDisplayMode="None"></ValidationSettings>
                            </dxe:ASPxTextBox>
                        </tr>

                        <tr>
                            <td>
                                <label style="margin-top:6px"><dxe:ASPxLabel ID="ASPxLabel1" runat="server" Text="E-Way Bill Date">
                                </dxe:ASPxLabel></label>
                                <dxe:ASPxDateEdit ID="dt_EWayBill" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cdt_EWayBill" Width="100%" UseMaskBehavior="True">
                                    <ButtonStyle Width="13px">
                                    </ButtonStyle>
                                </dxe:ASPxDateEdit>
                            </td>
                            

                        </tr>
                        <tr>
                            <td>
                                <label style="margin-top:6px"><dxe:ASPxLabel ID="ASPxLabel2" runat="server" Text="E-Way Bill Value">
                                </dxe:ASPxLabel></label>
                                <dxe:ASPxTextBox ID="txtEWayBillValue" ClientInstanceName="ctxtEWayBillValue"
                                    runat="server" Width="100%">
                                    <MaskSettings Mask="<0..999999999>.<0..99>" AllowMouseWheel="false" />
                                    <ValidationSettings RequiredField-IsRequired="false" ErrorDisplayMode="None"></ValidationSettings>
                                </dxe:ASPxTextBox>
                            </td>
                        </tr>
                         <%-- add extra column --%>
                        <tr>
                            <td>
                                <label style="margin-top: 6px">
                                    <dxe:ASPxLabel ID="ASPxLabel3" runat="server" Text="Transporter GSTIN">
                                    </dxe:ASPxLabel>
                                </label>
                                <dxe:ASPxTextBox ID="txtTransporterGSTIN" ClientInstanceName="ctxtTransporterGSTIN" runat="server" Width="100%">
                                    <%--<MaskSettings Mask="<0..999999999>.<0..99>" AllowMouseWheel="false" />--%>
                                    <ValidationSettings RequiredField-IsRequired="false" ErrorDisplayMode="None"></ValidationSettings>
                                </dxe:ASPxTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label style="margin-top: 6px">
                                    <dxe:ASPxLabel ID="ASPxLabel4" runat="server" Text="Transporter Name">
                                    </dxe:ASPxLabel>
                                </label>
                                <dxe:ASPxTextBox ID="txtTransporterName" ClientInstanceName="ctxtTransporterName"
                                    runat="server" Width="100%">
                                    <%--<MaskSettings Mask="<0..999999999>.<0..99>" AllowMouseWheel="false" />--%>
                                    <ValidationSettings RequiredField-IsRequired="false" ErrorDisplayMode="None"></ValidationSettings>
                                </dxe:ASPxTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label style="margin-top: 6px">
                                    <dxe:ASPxLabel ID="ASPxLabel5" runat="server" Text="Mode of transportation">
                                    </dxe:ASPxLabel>
                                </label>
                                <asp:DropDownList ID="ddlTransportationMode" runat="server" Width="100%">
                                    <asp:ListItem Selected="True" Text="Select" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="Road" Value="1" />
                                    <asp:ListItem Text="Rail" Value="2" />
                                    <asp:ListItem Text="Air" Value="3" />
                                    <asp:ListItem Text="Ship" Value="4" />
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label style="margin-top: 6px">
                                    <dxe:ASPxLabel ID="ASPxLabel6" runat="server" Text="Distance of transportation">
                                    </dxe:ASPxLabel>
                                </label>
                                <dxe:ASPxTextBox ID="txtTransportationDistance" ClientInstanceName="ctxtTransportationDistance" runat="server" Width="100%">
                                    <MaskSettings Mask="<0..9999>" AllowMouseWheel="false" />
                                    <ValidationSettings RequiredField-IsRequired="false" ErrorDisplayMode="None"></ValidationSettings>
                                </dxe:ASPxTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label style="margin-top: 6px">
                                    <dxe:ASPxLabel ID="ASPxLabel7" runat="server" Text="Transporter Doc No">
                                    </dxe:ASPxLabel>
                                </label>
                                <dxe:ASPxTextBox ID="txtTransporterDocNo" ClientInstanceName="ctxtTransporterDocNo" runat="server" Width="100%">
                                    <%--<MaskSettings Mask="<0..999999999>.<0..99>" AllowMouseWheel="false" />--%>
                                    <ValidationSettings RequiredField-IsRequired="false" ErrorDisplayMode="None"></ValidationSettings>
                                </dxe:ASPxTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label style="margin-top: 6px">
                                    <dxe:ASPxLabel ID="ASPxLabel8" runat="server" Text="Transporter Doc Date">
                                    </dxe:ASPxLabel>
                                </label>
                                <dxe:ASPxDateEdit ID="dt_TransporterDocDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cdt_TransporterDocDate" Width="100%" UseMaskBehavior="True">
                                    <ButtonStyle Width="13px">
                                    </ButtonStyle>
                                </dxe:ASPxDateEdit>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label style="margin-top: 6px">
                                    <dxe:ASPxLabel ID="ASPxLabel9" runat="server" Text="Vehicle No">
                                    </dxe:ASPxLabel>
                                </label>
                                <dxe:ASPxTextBox ID="txtVehicleNo" ClientInstanceName="ctxtVehicleNo" runat="server" Width="100%">
                                    <%--<MaskSettings Mask="<0..999999999>.<0..99>" AllowMouseWheel="false" />--%>
                                    <ValidationSettings RequiredField-IsRequired="false" ErrorDisplayMode="None"></ValidationSettings>
                                </dxe:ASPxTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label style="margin-top: 6px">
                                    <dxe:ASPxLabel ID="ASPxLabel10" runat="server" Text="Vehicle Type">
                                    </dxe:ASPxLabel>
                                </label>
                                <asp:DropDownList ID="ddlVehicleType" runat="server" Width="100%">
                                    <asp:ListItem Selected="True" Text="Select" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="ODC" Value="O" />
                                    <asp:ListItem Text="Regular" Value="R" />
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                    <div style="margin-top: 10px;">
                        <input id="btnEWayBillSave" class="btn btn-primary" onclick="CallEWayBill_save()" type="button" value="Save" />
                        <input id="btnEWayBillCancel" class="btn btn-danger" onclick="CancelEWayBill_save()" type="button" value="Cancel" />
                         <dxe:ASPxLabel ID="lblEwayBillStatus" runat="server" Text="" style="color: red; font-size:large"></dxe:ASPxLabel>
                    </div>

                </div>

            </dxe:PopupControlContentControl>
        </ContentCollection>
        <HeaderStyle BackColor="LightGray" ForeColor="Black" />


    </dxe:ASPxPopupControl>
</asp:Content>
