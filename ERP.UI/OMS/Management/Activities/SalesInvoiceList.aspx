<%--==========================================================Revision History ============================================================================================   
   1.0   Priti    V2.0.36     16-02-2023     Afer Listing view upgradation delete data show in list issue solved. 
   2.0	 PRITI    V2.0.38	  03/04/2023	 0025291: Views to be converted to Procedures in the Listing Page of Transaction / Sales / Invoice
   3.0	 Pallab   V2.0.38	  27/04/2023	 0025920: Sales Invoice module design modification 
   4.0   Sanchita V2.0.39     25-07-2023     Attachment icon will be shown against the document number if there is any attachment - Sales Challan
                                             Mantis : 26609   
   5.0   Sanchita V2.0.41     10/11/2023     26996: Sales Invoice Print when IRN not generated will take place based in System Settings - "Is Print Invoice Without IRN?"
========================================== End Revision History =======================================================================================================--%>

<%@ Page Title="Sales Invoice" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="SalesInvoiceList.aspx.cs" Inherits="ERP.OMS.Management.Activities.SalesInvoiceList" 
    EnableEventValidation="false" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>
<%@ Register Src="~/OMS/Management/Activities/UserControls/VehicleDetailsControl.ascx" TagPrefix="uc1" TagName="VehicleDetailsControl" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--Code Added By Sandip For Approval Detail Section Start--%>
    <script>

        $(document).ready(function () {
            $("#btntransporter").hide();
        });

        function WaitingSalesInvoice() {
            cWaitingInvoice.PerformCallback('Bind');
            cpopupWI.Show();
        }

        function WaitingRowClick(Draft_InvoiceId) {
            //var Draft_InvoiceId = s.GetRowKey(s.GetFocusedRowIndex());
            window.location.href = '/OMS/Management/Activities/SalesInvoice.aspx?key=ADD&InvType=S&Draft_Invoice=' + Draft_InvoiceId;

        }


        function OnEWayBillClick(id, VisibleIndex, EWayBillNumber, EWayBillDate, EWayBillValue, TransDate) {

            var ActiveEInvoice = $('#hdnActiveEInvoice').val();
            if (ActiveEInvoice == "1") {
                if ((new Date(TransDate)) >= (new Date("01-01-2021"))) {

                    $.ajax({
                        type: "POST",
                        url: "SalesInvoiceList.aspx/GetEditablePermissionFromEInvoice",
                        data: "{'SalesInvoiceID':'" + id + "','Action':'EWayBill'}",
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

                                //jAlert("IRN not generated can not print.");
                                $("#btnEWayBillSave").addClass('hide');
                            }
                        }
                    });
                }
            }

            cGrdQuotation.SetFocusedRowIndex(VisibleIndex);

            if (EWayBillNumber.trim() != "") {
                ctxtEWayBillNumber.SetText(EWayBillNumber);
            }
            else {
                ctxtEWayBillNumber.SetText("");
            }

            if (EWayBillDate.trim() != "" && EWayBillDate.trim() != "01-01-1970" && EWayBillDate.trim() != "01-01-1900" && EWayBillDate.trim() != "01-01-0100") {
                var d = new Date(EWayBillDate.split('-')[2].trim(), EWayBillDate.split('-')[1].trim() - 1, EWayBillDate.split('-')[0].trim(), 0, 0, 0, 0);
                cdt_EWayBill.SetDate(d);
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
                url: "SalesInvoiceList.aspx/EditEWayBill",
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


        $(document).ready(function () {
            //called when key is pressed in textbox
            $("#txt_ShipBillNum").keypress(function (e) {
                //if the letter is not digit then display error and don't type anything
                if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
                    //display error message
                    //$("#errmsg").html("Digits Only").show().fadeOut("slow");
                    // jAlert("Enter Numeric Values Only");
                    return false;
                }
            });
        });



        function OnShipBillClick(ShippingBill_InvoiceId, VisibleIndex, ShippingBill_Number, ShippingBill_Date, ShippingBill_PortId) {

            cGrdQuotation.SetFocusedRowIndex(VisibleIndex);



            if ((ShippingBill_Number.trim() == "0") || (ShippingBill_Number.trim() == "")) {
                ctxt_ShipBillNum.SetText("");
            }
                //else if
                //if ((ShippingBill_Number.trim() != "") || (ShippingBill_Number.trim() == "0")) {
                //    ctxt_ShipBillNum.SetText(ShippingBill_Number);
                //}
            else {
                ctxt_ShipBillNum.SetText(ShippingBill_Number);
            }

            if (ShippingBill_Date.trim() != "" && ShippingBill_Date.trim() != "01-01-1970" && ShippingBill_Date.trim() != "01-01-1900" && ShippingBill_Date.trim() != "01-01-0100") {
                var d = new Date(ShippingBill_Date.split('-')[2].trim(), ShippingBill_Date.split('-')[1].trim() - 1, ShippingBill_Date.split('-')[0].trim(), 0, 0, 0, 0);
                cShipBillDate.SetDate(d);
            }
            else {
                cShipBillDate.SetText("");
            }


            if (ShippingBill_PortId.trim() != "0" && ShippingBill_PortId.trim() != "") {
                cdrd_PortCode.SetValue(ShippingBill_PortId);
            }
            else {
                cdrd_PortCode.SetValue("");
            }
            $('#hddnInvoiceID').val(ShippingBill_InvoiceId);
            //  cPopup_UpdateShipDetails.Show();
            ctxt_ShipBillNum.Focus();
            cPopup_UpdateShipDetails.Show();
            ctxt_ShipBillNum.SetFocus();
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

        var ShippiongDetails = []


        function CallShipBill_save() {

            var ShipBillDetails = {};
            var ShippingBill_InvoiceId = $("#<%=hddnInvoiceID.ClientID%>").val();
            //$('#ShipDetailsActiontype').val(ShippingBill_InvoiceId);
            var ShippingBill_Number = ctxt_ShipBillNum.GetValue();
            if (ShippingBill_Number == "" || ShippingBill_Number == null) {
                ShippingBill_Number = "";
            }
            if (cShipBillDate.GetValue() == "" && cShipBillDate.GetValue() == null) {
                var ShippingBill_Date = "1990-01-01";
            }
            else {

                if (cShipBillDate.GetValue() == null) {
                    var ShippingBill_Date = "";
                }
                else {
                    var ShippingBill_Date = GetEWayBillDateFormat(new Date(cShipBillDate.GetValue()));
                }

            }


            if (cdrd_PortCode.GetValue() == 0) {
                ShippingBill_PortId = null;
            }
            else {
                var ShippingBill_PortId = cdrd_PortCode.GetValue();
            }
            ShipBillDetails.ShippingBill_InvoiceId = ShippingBill_InvoiceId;
            ShipBillDetails.ShippingBill_Number = ShippingBill_Number;
            ShipBillDetails.ShippingBill_Date = ShippingBill_Date;
            ShipBillDetails.ShippingBill_PortId = ShippingBill_PortId;
            //ShipBillDetails.ShippingBill_Id = ShippingBill_Id;


            ShipBillDetails.actionname = "InsertShipDetailsBill";

            //else
            //{
            //    ShipBillDetails.actionname ="UpdateShipDetailsBill";
            //}
            $.ajax({
                type: "POST",
                url: "SalesInvoiceList.aspx/UpdateShipBillDet",
                data: JSON.stringify(ShipBillDetails),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (msg) {
                    var status = msg.d;

                    if (status == "1") {
                        jAlert("Saved successfully.");
                        //ctxtEWayBillNumber.SetText("");
                        cPopup_UpdateShipDetails.Hide();
                        cGrdQuotation.Refresh();
                    }
                    else {
                        jAlert("Data not saved.");
                        cPopup_UpdateShipDetails.Hide();
                    }
                }
            });



        }


        function CancelShipBill_save() {
            cPopup_UpdateShipDetails.Hide();
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
                //var EWayBillDate = GetEWayBillDateFormat(new Date(cdt_EWayBill.GetValue()));
                //Rev Subhra  0019106  11/12/2018
                if (cdt_EWayBill.GetValue() == null) {
                    var EWayBillDate = null;
                }
                else {
                    var EWayBillDate = GetEWayBillDateFormat(new Date(cdt_EWayBill.GetValue()));
                }
                //End of Rev 
            }


            //Rev Subhra  0019106  11/12/2018
            //var EWayBillValue = ctxtEWayBillValue.GetValue();
            if (ctxtEWayBillValue.GetValue() == 0) {
                EWayBillValue = null;
            }
            else {
                var EWayBillValue = ctxtEWayBillValue.GetValue();
            }
            //End of Rev 

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
                url: "SalesInvoiceList.aspx/UpdateEWayBill",
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
                        cGrdQuotation.Refresh();
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
            //cgridPendingApproval.PerformCallback('Status~' + rowvalue);
            //cgridPendingApproval.GetRowValues(itemIndex, 'ID', OnGetApprovedRowValues);

        }
        function OnGetApprovedRowValues(obj) {
            uri = "SalesInvoice.aspx?key=" + obj + "&status=2" + '&type=SI';
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
            uri = "SalesInvoice.aspx?key=" + obj + "&status=3" + '&type=SI';
            popup.SetContentUrl(uri);
            popup.Show();
        }
        // function above  End For Rejected

        // To Reflect the Data in Pending Waiting Grid and Pending Waiting Counting if the user approve or Rejecte the Order and Saved 

        function OnApprovalEndCall(s, e) {
            $.ajax({
                type: "POST",
                url: "SalesInvoiceList.aspx/GetPendingCase",
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
        function onPrintJv(id, TransDate) {
            var ActiveEInvoice = $('#hdnActiveEInvoice').val();
            // if (ActiveEInvoice == "1") {
            // Rev 5.0
            //if (ActiveEInvoice == "2") {//0023893:ONLY SALES INVOICE MODULE WILL ALLOW PRINT WITHOUT IRN//For not enter in this loop
            if (ActiveEInvoice == "1") {//0023893:ONLY SALES INVOICE MODULE WILL ALLOW PRINT WITHOUT IRN//For not enter in this loop
            // End of Rev 5.0
                // if ((new Date(TransDate)) >= (new Date("01-01-2021"))) {
                var dateParts = TransDate.split("-");

                // Rev 5.0
                //if ((new Date(+dateParts[2], dateParts[1] - 1, +dateParts[0])) >= (new Date("01-01-2021"))) {
                if ((new Date(TransDate)) >= (new Date("01-01-2021"))) {
                    // End of Rev 5.0
                    $.ajax({
                        type: "POST",
                        url: "SalesInvoiceList.aspx/GetEditablePermissionFromEInvoice",
                        data: "{'SalesInvoiceID':'" + id + "','Action':'Print'}",
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
                            }
                            else {
                                jAlert("IRN generation is still pending. Cannot take print.");
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

        //function OrginalCheckChange(s, e) {
        //    debugger;
        //    if (s.GetCheckState() == 'Checked') {
        //        CselectDuplicate.SetEnabled(true);
        //    }
        //    else {
        //        CselectDuplicate.SetCheckState('UnChecked');
        //        CselectDuplicate.SetEnabled(false);
        //        CselectTriplicate.SetCheckState('UnChecked');
        //        CselectTriplicate.SetEnabled(false);
        //    }

        //}
        //function DuplicateCheckChange(s, e) {
        //    if (s.GetCheckState() == 'Checked') {
        //        CselectTriplicate.SetEnabled(true);
        //    }
        //    else {
        //        CselectTriplicate.SetCheckState('UnChecked');
        //        CselectTriplicate.SetEnabled(false);
        //    }

        //}

        function PerformCallToGridBind() {
            //cgriddocuments.PerformCallback('BindDocumentsGridOnSelection' + '~' + InvoiceId);
            //cDocumentsPopup.Hide();
            //return false;
            cSelectPanel.PerformCallback('Bindsingledesign');
            cDocumentsPopup.Hide();
            return false;
        }

        //function cgridDocumentsEndCall(s,e)
        //{
        //    debugger;
        //    if (cgriddocuments.cpSuccess !=null)
        //    {
        //        var TotDocument = cgriddocuments.cpSuccess.split(',');
        //        if (TotDocument.length > 0)
        //        {
        //            for (var i = 0; i <TotDocument.length; i++) {
        //                if (TotDocument[i] != "") {
        //                    if (cCmbDesignName.GetValue() == 1)
        //                        {
        //                        window.open("../../reports/XtraReports/Viewer/InvoiceReportViewer.aspx?id=" + InvoiceId + '&PrintOption=' + TotDocument[i], '_blank')
        //                    }
        //                    else if (cCmbDesignName.GetValue() == 2)
        //                    {
        //                        window.open("../../reports/XtraReports/Viewer/TaxInvoiceReportViewer.aspx?id=" + InvoiceId + '&PrintOption=' + TotDocument[i], '_blank')
        //                    }
        //                }                       
        //            }                    
        //        }                
        //    }
        //}


        function cSelectPanelEndCall(s, e) {
            debugger;
            if (cSelectPanel.cpSuccess != "") {
                var TotDocument = cSelectPanel.cpSuccess.split(',');
                var reportName = cCmbDesignName.GetValue();
                var module = 'Invoice';
                if (TotDocument.length > 0) {
                    for (var i = 0; i < TotDocument.length; i++) {
                        if (TotDocument[i] != "") {
                            //if (cCmbDesignName.GetValue() == 1) {
                            //    window.open("../../reports/XtraReports/Viewer/InvoiceReportViewer.aspx?id=" + InvoiceId + '&PrintOption=' + TotDocument[i], '_blank')
                            //}
                            //else if (cCmbDesignName.GetValue() == 2) {
                            //    window.open("../../reports/XtraReports/Viewer/TaxInvoiceReportViewer.aspx?id=" + InvoiceId + '&PrintOption=' + TotDocument[i], '_blank')
                            //}
                            //else {
                            //    window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + reportName + '&modulename=' + module + '&id=' + InvoiceId + '&PrintOption=' + TotDocument[i], '_blank')
                            //    //window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + reportName + '&modulename=' + module + '&id=' + InvoiceId + '&PrintOption=' + TotDocument[i], 'PrintingFrame')
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
                CselectTriplicate.SetCheckState('UnChecked');
                CselectOfficecopy.SetCheckState('UnChecked');
                cCmbDesignName.SetSelectedIndex(0);
            }
        }

        function OnclickViewAttachment(obj) {
            //var URL = '/OMS/Management/Activities/SalesInvoice_Document.aspx?idbldng=' + obj + '&type=SalesInvoice';
            var URL = '/OMS/Management/Activities/EntriesDocuments.aspx?idbldng=' + obj + '&type=SalesInvoice';
            window.location.href = URL;
        }


        document.onkeydown = function (e) {
            //if (event.keyCode == 18) isCtrl = true;
            //if (event.keyCode == 65 && isCtrl == true) { //run code for Ctrl+S -- ie, Save & New  
            //    StopDefaultAction(e);
            //   // OnAddButtonClick();
            //}

            if (event.keyCode == 73 && event.altKey == true) {
                StopDefaultAction(e);
                OnAddInventoryButtonClick();
            }

            if (event.keyCode == 78 && event.altKey == true) {
                StopDefaultAction(e);
                OnAddNonInventoryButtonClick();
            }

            if (event.keyCode == 83 && event.altKey == true) {
                StopDefaultAction(e);
                OnAddSeriviceButtonClick();
            }

            if (event.keyCode == 67 && event.altKey == true) {
                StopDefaultAction(e);
                OnAddCapitalButtonClick();
            }
            if (event.keyCode == 66 && event.altKey == true) {
                StopDefaultAction(e);
                OnAddBothButtonClick();
            }
        }
        function StopDefaultAction(e) {
            if (e.preventDefault) { e.preventDefault() }
            else { e.stop() };

            e.returnValue = false;
            e.stopPropagation();
        }
        function OnAddButtonClick() {
            var url = 'SalesInvoice.aspx?key=' + 'ADD';
            window.location.href = url;
        }

        function OnAddInventoryButtonClick() {
            var url = 'SalesInvoice.aspx?key=' + 'ADD&&InvType=I';
            window.location.href = url;
        }

        function OnAddNonInventoryButtonClick() {
            var url = 'SalesInvoice.aspx?key=' + 'ADD&&InvType=N';
            window.location.href = url;
        }

        function OnAddSeriviceButtonClick() {
            var url = 'SalesInvoice.aspx?key=' + 'ADD&&InvType=S';
            window.location.href = url;
        }

        function OnAddCapitalButtonClick() {
            var url = 'SalesInvoice.aspx?key=' + 'ADD&&InvType=C';
            window.location.href = url;
        }
        function OnAddBothButtonClick() {
            var url = 'SalesInvoice.aspx?key=' + 'ADD&&InvType=B';
            window.location.href = url;
        }


        function OnMoreInfoClick(keyValue, TransDate) {


            var ActiveEInvoice = $('#hdnActiveEInvoice').val();
            if (ActiveEInvoice == "1") {
                if ((new Date(TransDate)) >= (new Date("01-01-2021"))) {


                    $.ajax({
                        type: "POST",
                        url: "SalesInvoiceList.aspx/GetEditablePermissionFromEInvoice",
                        data: "{'SalesInvoiceID':'" + keyValue + "','Action':'Edit'}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (msg) {
                            var status = msg.d;
                            if (status == "Yes") {
                                var ActiveUser = '<%=Session["userid"]%>'
                                if (ActiveUser != null) {
                                    $.ajax({
                                        type: "POST",
                                        url: "SalesInvoiceList.aspx/GetEditablePermission",
                                        data: "{'ActiveUser':'" + ActiveUser + "'}",
                                        contentType: "application/json; charset=utf-8",
                                        dataType: "json",
                                        success: function (msg) {
                                            var status = msg.d;
                                            var url = 'SalesInvoice.aspx?key=' + keyValue + '&Permission=' + status + '&type=SI';
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
                else {
                    var ActiveUser = '<%=Session["userid"]%>'
                    if (ActiveUser != null) {
                        $.ajax({
                            type: "POST",
                            url: "SalesInvoiceList.aspx/GetEditablePermission",
                            data: "{'ActiveUser':'" + ActiveUser + "'}",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (msg) {
                                var status = msg.d;
                                var url = 'SalesInvoice.aspx?key=' + keyValue + '&Permission=' + status + '&type=SI';
                                window.location.href = url;
                            }
                        });
                    }
                }

            }
            else {
                var ActiveUser = '<%=Session["userid"]%>'
                if (ActiveUser != null) {
                    $.ajax({
                        type: "POST",
                        url: "SalesInvoiceList.aspx/GetEditablePermission",
                        data: "{'ActiveUser':'" + ActiveUser + "'}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (msg) {
                            var status = msg.d;
                            var url = 'SalesInvoice.aspx?key=' + keyValue + '&Permission=' + status + '&type=SI';
                            window.location.href = url;
                        }
                    });
                }
            }

        }

        ////##### coded by Samrat Roy - 04/05/2017  
        ////Add an another param to define request type 
        function OnViewClick(keyValue) {
            //var url = 'SalesInvoice.aspx?key=' + keyValue + '&req=V' + '&type=SI';
            //window.location.href = url;
            var url = '/OMS/management/Activities/View/Invoice.html?id=' + keyValue;
            cPosView.SetContentUrl(url);
            cPosView.RefreshContentUrl();

            cPosView.Show();


        }

        function OnClickDelete(keyValue, TransDate) {

            var ActiveEInvoice = $('#hdnActiveEInvoice').val();
            if (ActiveEInvoice == "1") {

                if ((new Date(TransDate)) >= (new Date("01-01-2021"))) {

                    $.ajax({
                        type: "POST",
                        url: "SalesInvoiceList.aspx/GetEditablePermissionFromEInvoice",
                        data: "{'SalesInvoiceID':'" + keyValue + "','Action':'Delete'}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (msg) {
                            var status = msg.d;
                            if (status == "Yes") {

                                jConfirm('Confirm delete?', 'Confirmation Dialog', function (r) {
                                    if (r == true) {
                                        cGrdQuotation.PerformCallback('Delete~' + keyValue);
                                    }
                                });
                            }
                            else {
                                jAlert("IRN generated can not delete.");
                            }
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
            else {
                jConfirm('Confirm delete?', 'Confirmation Dialog', function (r) {
                    if (r == true) {
                        cGrdQuotation.PerformCallback('Delete~' + keyValue);
                    }
                });
            }
        }
    </script>

    <script>
        function OpenPopUPApproveSO() {
            cgridApprovedSO.PerformCallback();
            cPOPUPApprovedSO.Show();
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

        .dxeErrorFrameWithoutError_PlasticBlue .dxeControlsCell_PlasticBlue, .dxeErrorFrameWithoutError_PlasticBlue.dxeControlsCell_PlasticBlue {
            padding: 0px !important;
        }
    </style>
    <script>
        var isFirstTime = true;

        function AllControlInitilize() {
            if (isFirstTime) {
                if (localStorage.getItem('InvoiceList_FromDate')) {
                    var fromdatearray = localStorage.getItem('InvoiceList_FromDate').split('-');
                    var fromdate = new Date(fromdatearray[0], parseFloat(fromdatearray[1]) - 1, fromdatearray[2], 0, 0, 0, 0);
                    cFormDate.SetDate(fromdate);
                }

                if (localStorage.getItem('InvoiceList_ToDate')) {
                    var todatearray = localStorage.getItem('InvoiceList_ToDate').split('-');
                    var todate = new Date(todatearray[0], parseFloat(todatearray[1]) - 1, todatearray[2], 0, 0, 0, 0);
                    ctoDate.SetDate(todate);
                }

                if (localStorage.getItem('InvoiceList_Branch')) {
                    if (ccmbBranchfilter.FindItemByValue(localStorage.getItem('InvoiceList_Branch'))) {
                        ccmbBranchfilter.SetValue(localStorage.getItem('InvoiceList_Branch'));
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
                localStorage.setItem("InvoiceList_FromDate", cFormDate.GetDate().format('yyyy-MM-dd'));
                localStorage.setItem("InvoiceList_ToDate", ctoDate.GetDate().format('yyyy-MM-dd'));
                localStorage.setItem("InvoiceList_Branch", ccmbBranchfilter.GetValue());

                $("#hfFromDate").val(cFormDate.GetDate().format('yyyy-MM-dd'));
                $("#hfToDate").val(ctoDate.GetDate().format('yyyy-MM-dd'));
                $("#hfBranchID").val(ccmbBranchfilter.GetValue());
                $("#hfIsFilter").val("Y");
                //REV 2.0
                $("#hFilterType").val("All");
                cCallbackPanel.PerformCallback("");
                //cGrdQuotation.Refresh();
                //REV 2.0 END
                //cGrdQuotation.PerformCallback('FilterGridByDate~' + cFormDate.GetDate().format('yyyy-MM-dd') + '~' + ctoDate.GetDate().format('yyyy-MM-dd') + '~' + ccmbBranchfilter.GetValue())
            }
        }
        //REV 2.0 
        function CallbackPanelEndCall(s, e) {
            cGrdQuotation.Refresh();
        }
        //REV 2.0 END
        function grid_EndCallBack(s, e) {
            if (cGrdQuotation.cpDelete) {
                var message = cGrdQuotation.cpDelete;
                cGrdQuotation.cpDelete = null;

                jAlert(message);
                cGrdQuotation.Refresh();
                /*Rev 1.0*/
                //updateGridByDate();
                /*Rev 1.0 End*/
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
    </script>
    <style>
        .searchBoxSmall > table > tbody > tr > td > table {
            max-width: 250px !important;
        }
    </style>
    <script>
        function UpdateTransporter(values) {
            $("#hddnInvoiceID").val(values);
            callTransporterControl(values, "SI");
            $("#exampleModal").modal('show');
        }

        function InsertTransporterControlDetails(data) {
            var InvoiceID = $("#hddnInvoiceID").val();
            $.ajax({
                type: "POST",
                url: "SalesInvoiceList.aspx/InsertTransporterControlDetails",
                data: "{'id':'" + InvoiceID + "','hfControlData':'" + data + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,//Added By:Subhabrata
                success: function (msg) {

                }
            });
        }
    </script>

    <%--Rev 3.0--%>
    <link href="/assests/css/custom/newcustomstyle.css" rel="stylesheet" />

    <style>
        .TableMain100 #ShowGrid, .TableMain100 #ShowGridList, .TableMain100 #ShowGridRet, .TableMain100 #ShowGridLocationwiseStockStatus,
        #GrdQuotation{
    max-width: 98% !important;
}

        select#ddlInventory
        {
            -webkit-appearance: auto !important;
        }

        input + label
        {
            margin-top: 13px;
        }

        #Popup_InlineRemarks_PW-1
            {
                position:fixed !important;
                left: 18% !important;
                top: 20% !important;
            }

        @media only screen and (max-width: 1380px) and (min-width: 1300px)
        {
            #Popup_MultiUOM_PW-1 , #Popup_Warehouse_PW-1 , #Popup_Taxes_PW-1 , #aspxTaxpopUp_PW-1 , #Popup_InlineRemarks_PW-1
            {
                position:fixed !important;
                left: 15% !important;
                top: 60px !important;
            }
        }
    </style>
    <%--Rev end 3.0--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--Rev 3.0: "outer-div-main" class add --%>
    <div class="outer-div-main clearfix">
        <div class="panel-heading">
        <div class="panel-title clearfix">
            <h3 class="pull-left">Sales Invoice</h3>
            <div id="pageheaderContent" class="scrollHorizontal pull-right wrapHolder content horizontal-images" style="display: none;">
                <%--class="pull-right wrapHolder reverse content horizontal-images">--%>
                <div class="Top clearfix">
                    <ul>
                        <li>
                            <div class="lblHolder">
                                <table>
                                    <tr>
                                        <td>Customer Balance</td>
                                    </tr>
                                    <tr>
                                        <td class="lower">
                                            <asp:Label ID="lblAvailableDues" runat="server" Text="0.0"></asp:Label>
                                        </td>
                                    </tr>
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
                <%--Rev 3.0: "for-cust-icon" class add --%>
                <td class="for-cust-icon">
                    <dxe:ASPxDateEdit ID="FormDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cFormDate" Width="100%" UseMaskBehavior="True">
                        <ButtonStyle Width="13px">
                        </ButtonStyle>
                    </dxe:ASPxDateEdit>
                    <%--Rev 3.0--%>
                    <img src="/assests/images/calendar-icon.png" class="calendar-icon"/>
                    <%--Rev end 3.0--%>
                </td>
                <td>
                    <label>To Date</label>
                </td>
                 <%--Rev 3.0: "for-cust-icon" class add --%>
                <td class="for-cust-icon">
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
                    <input type="button" value="Show" class="btn btn-primary " onclick="updateGridByDate()" />
                </td>

            </tr>

        </table>
    </div>
        <div class="form_main">
        <div class="clearfix" >
            <% if (rights.CanAdd)
               { %>
            <%--<a href="javascript:void(0);" onclick="OnAddButtonClick()" class="btn btn-primary"><span><u>A</u>dd New</span> </a><%} %>--%>
            <a href="javascript:void(0);" onclick="OnAddInventoryButtonClick()" class="btn btn-success "><span class="btn-icon"><i class="fa fa-plus"></i></span><span><u>I</u>nventory</span> </a>
            <a href="javascript:void(0);" onclick="OnAddNonInventoryButtonClick()" class="btn btn-success "><span class="btn-icon"><i class="fa fa-plus"></i></span><span><u>N</u>on Inventory</span> </a>
            <a href="javascript:void(0);" onclick="OnAddSeriviceButtonClick()" class="btn btn-success "><span class="btn-icon"><i class="fa fa-plus"></i></span><span><u>S</u>ervice</span> </a>
            <a href="javascript:void(0);" onclick="OnAddCapitalButtonClick()" class="btn btn-success "><span class="btn-icon"><i class="fa fa-plus"></i></span><span><u>C</u>apital Goods</span> </a>
            <a href="javascript:void(0);" onclick="OnAddBothButtonClick()" class="btn btn-success "><span class="btn-icon"><i class="fa fa-plus"></i></span><span><u>A</u>ll Items</span> </a>

            <%} %>

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
                <a href="javascript:void(0);" onclick="OpenPopUPUserWiseQuotaion()" class="btn btn-info ">
                    <span>My Sales Invoice Status</span>
                    <%--<asp:Label ID="Label1" runat="server" Text=""></asp:Label>--%>                   
                </a>
            </span>
            <span id="divPendingWaiting" runat="server">
                <a href="javascript:void(0);" onclick="OpenPopUPApprovalStatus()" class="btn btn-warning ">
                    <span>Approval Waiting</span>

                    <asp:Label ID="lblWaiting" runat="server" Text=""></asp:Label>
                </a>
                <i class="fa fa-reply blink" style="font-size: 20px; margin-right: 10px;" aria-hidden="true"></i>
            </span>
            <span id="span1" runat="server">
                <a href="javascript:void(0);" onclick="OpenPopUPApproveSO()" class="btn btn-primary ">
                    <span>Approved Sales Order</span>
                </a>
            </span>

            <span id="span2" runat="server">
                <a href="javascript:void(0);" onclick="WaitingSalesInvoice()" class="btn btn-primary ">
                    <span>Sales Invoice Waiting</span>
                </a>
            </span>

            <%--Sandip Section for Approval Section in Design End --%>
        </div>
    </div>
    
        <div id="spnEditLock" runat="server" style="display: none; color: red; text-align: center"></div>
        <div id="spnDeleteLock" runat="server" style="display: none; color: red; text-align: center"></div>
    <%--<dxe:ASPxCallbackPanel runat="server" id="ComponentPanel" ClientInstanceName="cComponentPanel" OnCallback="Component_Callback">

         <PanelCollection>

             <dxe:PanelContent runat="server">

             </dxe:PanelContent>
         </PanelCollection>
     </dxe:ASPxCallbackPanel>--%>
        <div class="GridViewArea relative">
        <dxe:ASPxGridView ID="GrdQuotation" runat="server" KeyFieldName="Invoice_Id" AutoGenerateColumns="False" SettingsBehavior-AllowFocusedRow="true"
            Width="100%" ClientInstanceName="cGrdQuotation" OnCustomCallback="GrdQuotation_CustomCallback"
            Settings-HorizontalScrollBarMode="Auto" DataSourceID="EntityServerModeDataSource" SettingsDataSecurity-AllowEdit="false"
            SettingsDataSecurity-AllowInsert="false" SettingsDataSecurity-AllowDelete="false" SettingsBehavior-ColumnResizeMode="Control"
            Settings-VerticalScrollableHeight="250" Settings-VerticalScrollBarMode="Auto" Styles-SearchPanel-CssClass="searchBoxSmall">
            <SettingsSearchPanel Visible="True" Delay="5000" />
            <Columns>
                <%--Rev 4.0--%>
                <dxe:GridViewDataTextColumn HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="center" VisibleIndex="0" Width="40px" FixedStyle="Left">
                    <DataItemTemplate>
                        <img src="../../../assests/images/attachment.png" style='<%#Eval("IsAttachmentDoc")%>' />
                        
                    </DataItemTemplate>
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    <CellStyle HorizontalAlign="Center"></CellStyle>

                    <HeaderTemplate><span></span></HeaderTemplate>
                    <EditFormSettings Visible="False"></EditFormSettings>
                    <Settings AllowAutoFilterTextInputTimer="False"/>
                </dxe:GridViewDataTextColumn>
                <%--End of Rev 4.0--%>
                <dxe:GridViewDataTextColumn FieldName="Invoice_Id" Visible="false" SortOrder="Descending" Width="0">
                    <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Document No." FieldName="InvoiceNo"
                    VisibleIndex="1" FixedStyle="Left" Width="150px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Posting Date" FieldName="TransDate"
                    VisibleIndex="2" PropertiesTextEdit-DisplayFormatString="dd-MM-yyyy">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <PropertiesTextEdit DisplayFormatString="dd-MM-yyyy"></PropertiesTextEdit>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Invoice Type" FieldName="InvoiceType"
                    VisibleIndex="3">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Sales Order No." FieldName="SalesOrder_Number"
                    VisibleIndex="4" Width="150px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="SO Date" FieldName="SalesOrder_Date" Width="150px"
                    VisibleIndex="5">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Sales Challan No." FieldName="SalesChallan_Number" Width="150px"
                    VisibleIndex="6">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Challan Date" FieldName="SalesChallan_Date" Width="150px"
                    VisibleIndex="7">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Ageing Today" FieldName="AgeingDate"
                    VisibleIndex="8" Width="100px" Visible="false">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Customer" FieldName="CustomerName" Width="150px"
                    VisibleIndex="9">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Project Name" FieldName="Proj_Name" Width="150px"
                    VisibleIndex="10" Settings-AllowAutoFilter="True">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="true" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Gross Amount" FieldName="GrossAmount"
                    VisibleIndex="11" Width="130px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Tax & Charges" FieldName="ChargesAmount"
                    VisibleIndex="12" Width="130px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Net Amount" FieldName="NetAmount"
                    VisibleIndex="13" Width="130px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Amount Received" FieldName="AmountReceived"
                    VisibleIndex="14" Width="130px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Balance Amount" FieldName="BalanceAmount"
                    VisibleIndex="15" Width="130px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>

                <dxe:GridViewDataTextColumn Caption="Place of Supply[GST]" FieldName="PosState" VisibleIndex="16" Width="130px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="E-Way Bill No" FieldName="EWayBillNumber" VisibleIndex="17" Width="100px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="IRN ?" FieldName="IsIRN" VisibleIndex="18" Width="100px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="IRN" FieldName="IRN" VisibleIndex="19" Width="200px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Ack No" FieldName="AckNo" VisibleIndex="20" Width="200px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Ack Date" FieldName="AckDt" VisibleIndex="21" Width="100px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Remarks" FieldName="Remarks" VisibleIndex="22" Width="100px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Created By" FieldName="EnteredByName"
                    VisibleIndex="23" Width="100px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Updated By" FieldName="ModifiedByName"
                    VisibleIndex="24" Width="100px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Updated On" FieldName="ModifiedDate"
                    VisibleIndex="25" Width="150px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="center" VisibleIndex="26" Width="0">
                    <DataItemTemplate>
                        <div class='floatedBtnArea'>
                            <% if (rights.CanView)
                               { %>
                            <a href="javascript:void(0);" onclick="OnViewClick('<%# Container.KeyValue %>')" class="" title="">
                                <span class='ico ColorFive'><i class='fa fa-eye'></i></span><span class='hidden-xs'>View</span></a>
                            <% } %>
                            <% if (rights.CanEdit)
                               { %>
                            <a href="javascript:void(0);" onclick="OnMoreInfoClick('<%# Container.KeyValue %>','<%#Eval("TransDate") %>')" class="" title="" style='<%#Eval("Editlock")%>'>
                                <span class='ico editColor'><i class='fa fa-pencil' aria-hidden='true'></i></span><span class='hidden-xs'>Edit</span></a><%} %>
                            <% if (rights.CanDelete)
                               { %>
                            <a href="javascript:void(0);" onclick="OnClickDelete('<%# Container.KeyValue %>','<%#Eval("TransDate") %>')" class="" title="" style='<%#Eval("Deletelock")%>'>
                                <span class='ico deleteColor'><i class='fa fa-trash' aria-hidden='true'></i></span><span class='hidden-xs'>Delete</span></a><%} %>
                            <%-- <a href="javascript:void(0);" onclick="OnClickCopy('<%# Container.KeyValue %>')" class="pad" title="Copy ">
                            <i class="fa fa-copy"></i></a>--%>
                            <% if (rights.CanView)
                               { %>
                            <a href="javascript:void(0);" onclick="OnclickViewAttachment('<%# Container.KeyValue %>')" class="" title="">
                                <span class='ico ColorThree'><i class='fa fa-paperclip'></i></span><span class='hidden-xs'>Add/View Attachment</span>
                            </a><%} %>
                            <a href="javascript:void(0);" onclick="OnEWayBillClick('<%# Container.KeyValue %>','<%# Container.VisibleIndex %>','<%#Eval("EWayBillNumber") %>','<%#Eval("EWayBillDate") %>','<%#Eval("EWayBillValue") %>','<%#Eval("TransDate") %>')" class="" title="">
                                <span class='ico ColorFour'><i class='fa fa-file-text-o'></i></span><span class='hidden-xs'>Update E-Way Bill</span></a>
                            <a href="javascript:void(0);" onclick="OnShipBillClick('<%# Container.KeyValue %>','<%# Container.VisibleIndex %>','<%#Eval("ShippingBill_Number") %>','<%#Eval("ShippingBill_Date") %>','<%#Eval("ShippingBill_PortId") %>')" class="pad" title="" style='<%#Eval("InvoiceAdd_countryId")%>'>
                                <span class='ico ColorSix'><i class='fa fa-map-marker'></i></span><span class='hidden-xs'>Update Shipping Details</span></a>
                            <% if (rights.CanPrint)
                               { %>
                            <a href="javascript:void(0);" onclick="onPrintJv('<%# Container.KeyValue %>','<%#Eval("TransDate") %>')" class="" title="">
                                <span class='ico ColorSeven'><i class='fa fa-print'></i></span><span class='hidden-xs'>Print</span>
                            </a><%} %>
                            <% if (rights.CanUpdateTransporter)
                                { %>
                            <a href="javascript:void(0);" onclick="UpdateTransporter('<%# Container.KeyValue %>')" class="" title="" >
                                <span class='ico editColor'><i class='fa fa-pencil' aria-hidden='true'></i></span><span class='hidden-xs'>Update Transporter</span></a>
                            <% } %>
                        </div>
                    </DataItemTemplate>
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    <CellStyle HorizontalAlign="Center"></CellStyle>
                    <HeaderTemplate><span></span></HeaderTemplate>
                    <EditFormSettings Visible="False"></EditFormSettings>
                    <Settings AllowAutoFilterTextInputTimer="False" />

                </dxe:GridViewDataTextColumn>
            </Columns>
            <SettingsContextMenu Enabled="true"></SettingsContextMenu>
            <ClientSideEvents EndCallback="grid_EndCallBack" RowClick="gridRowclick" />
            <SettingsPager NumericButtonCount="10" PageSize="10" ShowSeparators="True" Mode="ShowPager">
                <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
                <FirstPageButton Visible="True">
                </FirstPageButton>
                <LastPageButton Visible="True">
                </LastPageButton>
            </SettingsPager>
            <GroupSummary>
                <dxe:ASPxSummaryItem FieldName="GrossAmount" SummaryType="Sum" DisplayFormat="Total Gross Amount : {0}" />
                <dxe:ASPxSummaryItem FieldName="ChargesAmount" SummaryType="Sum" DisplayFormat="Total Tax & Charges : {0}" />
                <dxe:ASPxSummaryItem FieldName="NetAmount" SummaryType="Sum" DisplayFormat="Total Net Amount : {0}" />
                <dxe:ASPxSummaryItem FieldName="AmountReceived" SummaryType="Sum" DisplayFormat="Total Amount Received : {0}" />
                <dxe:ASPxSummaryItem FieldName="BalanceAmount" SummaryType="Sum" DisplayFormat="Total Balance Amount : {0}" />
            </GroupSummary>

            <Settings ShowGroupPanel="True" ShowStatusBar="Hidden" ShowHorizontalScrollBar="False" ShowFilterRow="true" ShowFilterRowMenu="true" />
            <SettingsLoadingPanel Text="Please Wait..." />
        </dxe:ASPxGridView>

        <dx:LinqServerModeDataSource ID="EntityServerModeDataSource" runat="server" OnSelecting="EntityServerModeDataSource_Selecting"
            ContextTypeName="ERPDataClassesDataContext" TableName="v_SalesInvoiceList" />
        <asp:SqlDataSource runat="server" ID="dsPortCode" SelectCommand="SELECT [Port_id] Port_Id, [Port_Code] Port_Code FROM [tbl_master_PortCode]"></asp:SqlDataSource>
        <asp:HiddenField ID="hiddenedit" runat="server" />

        <uc1:VehicleDetailsControl runat="server" ID="VehicleDetailsControl" />
    </div>
    </div>
    <div style="display: none">
        <dxe:ASPxGridViewExporter ID="exporter" GridViewID="GrdQuotation" runat="server" Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
        </dxe:ASPxGridViewExporter>
    </div>

    <%--DEBASHIS--%>
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
                                </dxe:ASPxCheckBox>--%>

                                <%--<dxe:ASPxCheckBox ID="selectDuplicate" Text="Duplicate For Transporter" runat="server" ToolTip="Select Duplicate" ClientSideEvents-CheckedChanged="DuplicateCheckChange"
                                    ClientInstanceName="CselectDuplicate">
                                </dxe:ASPxCheckBox>--%>
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
                                    <%-- <Items>
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
                                    <dxe:GridViewDataTextColumn Caption="Sale Invoice No." FieldName="Number"
                                        VisibleIndex="0" FixedStyle="Left">
                                        <CellStyle CssClass="gridcellleft" Wrap="true">
                                        </CellStyle>
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn Caption="Date" FieldName="CreateDate"
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


        <dxe:ASPxPopupControl ID="popupWI" runat="server" ClientInstanceName="cpopupWI"
            Width="900px" HeaderText="Pending Approvals" PopupHorizontalAlign="WindowCenter" HeaderStyle-CssClass="wht"
            PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
            Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
            ContentStyle-CssClass="pad">
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">
                    <%--<div style="Width:400px;background-color:#FFFFFF;margin:0px;border:1px solid red;">--%>
                    <div class="row">
                        <div class="col-md-12">
                            <dxe:ASPxGridView ID="WaitingInvoice" runat="server" KeyFieldName="ID" AutoGenerateColumns="False"
                                Width="100%" ClientInstanceName="cWaitingInvoice" OnCustomCallback="WaitingInvoice_CustomCallback" OnDataBinding="WaitingInvoice_DataBinding">
                                <Columns>
                                    <dxe:GridViewDataTextColumn PropertiesTextEdit-DisplayFormatString="dd-MM-yyyy" Caption="Date" FieldName="Invoice_date"
                                        VisibleIndex="1" FixedStyle="Left">
                                        <CellStyle CssClass="gridcellleft" Wrap="true">
                                        </CellStyle>
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn Caption="Customer" FieldName="Customer"
                                        VisibleIndex="2" FixedStyle="Left">
                                        <CellStyle CssClass="gridcellleft" Wrap="true">
                                        </CellStyle>
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn Caption="Sales Point" FieldName="Sales_Point"
                                        VisibleIndex="3" FixedStyle="Left">
                                        <CellStyle CssClass="gridcellleft" Wrap="true">
                                        </CellStyle>
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn Caption="Product" FieldName="Product"
                                        VisibleIndex="4" FixedStyle="Left">
                                        <CellStyle CssClass="gridcellleft" Wrap="true">
                                        </CellStyle>
                                    </dxe:GridViewDataTextColumn>
                                    
                                    <dxe:GridViewDataTextColumn PropertiesTextEdit-DisplayFormatString="0.00" Caption="Amount" FieldName="Amount"
                                        VisibleIndex="4" FixedStyle="Left">
                                        <CellStyle CssClass="gridcellleft" Wrap="true">
                                        </CellStyle>
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="center" VisibleIndex="5" Width="100">
                                    <DataItemTemplate>
                                        <div class=''>
                                            <a href="javascript:void(0);" onclick="WaitingRowClick('<%# Container.KeyValue %>')" class="" title="">
                                                <span class='ico ColorFive'><i class='fa fa-eye'></i></span><span class='hidden-xs'>View</span></a>
                                            </div>
                                        </DataItemTemplate>
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
                                <%--<ClientSideEvents RowClick="WaitingRowClick" />--%>
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
                                    <dxe:GridViewDataTextColumn Caption="Sale Invoice No." FieldName="number"
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

    <%-- Sandip Approval Dtl Section End--%>

    <%-- Approved Sales Order --%>

    <dxe:ASPxPopupControl ID="POPUPApprovedSO" runat="server" ClientInstanceName="cPOPUPApprovedSO"
        Width="900px" HeaderText="Approved Sales Order" PopupHorizontalAlign="WindowCenter" HeaderStyle-CssClass="wht"
        PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
        ContentStyle-CssClass="pad">
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
                <div class="row">
                    <div class="col-md-12">
                        <dxe:ASPxGridView ID="gridApprovedSO" runat="server" KeyFieldName="Order_Id" AutoGenerateColumns="False" OnDataBinding="gridApprovedSO_DataBinding"
                            Width="100%" ClientInstanceName="cgridApprovedSO" OnCustomCallback="gridApprovedSO_CustomCallback">
                            <Columns>
                                <dxe:GridViewDataTextColumn Caption="SO No." FieldName="Order_Number"
                                    VisibleIndex="0" FixedStyle="Left">
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="SO Date" FieldName="Order_Date"
                                    VisibleIndex="1" FixedStyle="Left">
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="Approved By" FieldName="ApproverName"
                                    VisibleIndex="2" FixedStyle="Left">
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="Approved On" FieldName="ApprovedOn"
                                    VisibleIndex="3" FixedStyle="Left">
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="Customer Name" FieldName="CustomerName"
                                    VisibleIndex="4" FixedStyle="Left">
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="GSTIN" FieldName="GSTIN"
                                    VisibleIndex="5" FixedStyle="Left">
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="Entered By" FieldName="EnteredBy"
                                    VisibleIndex="6" FixedStyle="Left">
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="Unit" FieldName="Branch"
                                    VisibleIndex="7" FixedStyle="Left">
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

    <%-- Approved Sales Order --%>

    <dxe:ASPxGlobalEvents ID="GlobalEvents" runat="server">
        <ClientSideEvents ControlsInitialized="AllControlInitilize" />
    </dxe:ASPxGlobalEvents>

    <div>
        <asp:HiddenField ID="hfIsFilter" runat="server" />
        <asp:HiddenField ID="hfFromDate" runat="server" />
        <asp:HiddenField ID="hfToDate" runat="server" />
        <asp:HiddenField ID="hfBranchID" runat="server" />
        <asp:HiddenField ID="hddnInvoiceID" runat="server" />
        <asp:HiddenField ID="ShipDetailsActiontype" runat="server" />
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
                            <label>
                                <dxe:ASPxLabel ID="ASPxLabel12" runat="server" Text="E-Way Bill Number">
                                </dxe:ASPxLabel>
                            </label>

                            <dxe:ASPxTextBox ID="txtEWayBillNumber" MaxLength="12" ClientInstanceName="ctxtEWayBillNumber"
                                runat="server" Width="100%">
                                <%-- <MaskSettings Mask="<0..999999999999>" AllowMouseWheel="false" />--%>
                                <ValidationSettings RequiredField-IsRequired="false" ErrorDisplayMode="None"></ValidationSettings>
                            </dxe:ASPxTextBox>
                        </tr>

                        <tr>
                            <td>
                                <label style="margin-top: 6px">
                                    <dxe:ASPxLabel ID="ASPxLabel1" runat="server" Text="E-Way Bill Date">
                                    </dxe:ASPxLabel>
                                </label>
                                <dxe:ASPxDateEdit ID="dt_EWayBill" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cdt_EWayBill" Width="100%" UseMaskBehavior="True">
                                    <ButtonStyle Width="13px">
                                    </ButtonStyle>
                                </dxe:ASPxDateEdit>
                            </td>


                        </tr>
                        <tr>
                            <td>
                                <label style="margin-top: 6px">
                                    <dxe:ASPxLabel ID="ASPxLabel2" runat="server" Text="E-Way Bill Value">
                                    </dxe:ASPxLabel>
                                </label>
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
                        <dxe:ASPxLabel ID="lblEwayBillStatus" runat="server" Text="" Style="color: red; font-size: large"></dxe:ASPxLabel>

                    </div>

                </div>

            </dxe:PopupControlContentControl>
        </ContentCollection>
        <HeaderStyle BackColor="LightGray" ForeColor="Black" />


    </dxe:ASPxPopupControl>
    <dxe:ASPxPopupControl ID="Popup_UpdateShipDetails" runat="server" ClientInstanceName="cPopup_UpdateShipDetails"
        Width="400px" HeaderText="Update Shipping Details" PopupHorizontalAlign="WindowCenter"
        BackColor="white" Height="150px" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True">
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">

                <div class="Top clearfix">

                    <table style="width: 100%; margin: 0 auto; margin-top: 5px;">

                        <tr>
                            <td>
                                <label>
                                    <dxe:ASPxLabel ID="lbl_ShipBillNum" runat="server" Text="Shipping Bill Number">
                                    </dxe:ASPxLabel>
                                </label>

                                <dxe:ASPxTextBox ID="txt_ShipBillNum" MaxLength="10" ClientInstanceName="ctxt_ShipBillNum"
                                    runat="server" Width="100%">
                                    <%--  <MaskSettings Mask="<0..9999999999>" AllowMouseWheel="false" />--%>
                                    <%--<ClientSideEvents keyup="return  NumericSettings(event)"  />--%>
                                    <%--   <MaskSettings Mask="&lt;-0..9999999999&gt;" AllowMouseWheel="false" />--%>
                                    <ValidationSettings RequiredField-IsRequired="false" ErrorDisplayMode="None"></ValidationSettings>
                                </dxe:ASPxTextBox>
                                <div>
                                    <label id="ShipMessage" style="color: blue; text-align: right; display: block; padding: 0; padding-top: 5px; margin: 0;"><b>Upto 10 Digits</b></label>
                                </div>

                            </td>
                        </tr>

                        <tr>
                            <td>
                                <label>
                                    <dxe:ASPxLabel ID="lbl_ShipBillDate" runat="server" Text="Shipping Bill Date">
                                    </dxe:ASPxLabel>
                                </label>
                                <dxe:ASPxDateEdit ID="ShipBillDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cShipBillDate" Width="100%" MinDate="01/01/1900" UseMaskBehavior="True">
                                    <ButtonStyle Width="13px">
                                    </ButtonStyle>
                                </dxe:ASPxDateEdit>
                            </td>


                        </tr>
                        <tr>
                            <td>
                                <label style="margin-top: 6px">
                                    <dxe:ASPxLabel ID="lbl_PortCode" runat="server" Text="Port Code">
                                    </dxe:ASPxLabel>
                                </label>
                                <dxe:ASPxComboBox ID="drd_PortCode" ClientInstanceName="cdrd_PortCode" Enabled="true" runat="server" Width="100%"
                                    MaxLength="30" DataSourceID="dsPortCode" ValueField="Port_id" TextField="Port_Code">
                                </dxe:ASPxComboBox>
                            </td>
                        </tr>
                    </table>
                    <div style="margin-top: 10px;">
                        <input id="btnShipBillSave" class="btn btn-primary" onclick="CallShipBill_save()" type="button" value="Save" />
                        <input id="btnShipBillCancel" class="btn btn-danger" onclick="CancelShipBill_save()" type="button" value="Cancel" />
                    </div>

                </div>

            </dxe:PopupControlContentControl>
        </ContentCollection>
        <HeaderStyle BackColor="LightGray" ForeColor="Black" />


    </dxe:ASPxPopupControl>

    <dxe:ASPxPopupControl ID="PosView" runat="server"
        CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="cPosView" Height="650px"
        Width="1020px" HeaderText="POs Sales Invoice" Modal="true">

        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
            </dxe:PopupControlContentControl>
        </ContentCollection>
    </dxe:ASPxPopupControl>
  <%--  REV 2.0--%>
     <dxe:ASPxCallbackPanel runat="server" ID="CallbackPanel" ClientInstanceName="cCallbackPanel" OnCallback="CallbackPanel_Callback">
        <PanelCollection>
            <dxe:PanelContent runat="server">           
            </dxe:PanelContent>
        </PanelCollection>
        <ClientSideEvents EndCallback="CallbackPanelEndCall" />
    </dxe:ASPxCallbackPanel>
     <asp:HiddenField ID="hFilterType" runat="server" />
     <%-- REV 2.0 END--%>
    <asp:HiddenField ID="hdnLockFromDateedit" runat="server" />
    <asp:HiddenField ID="hdnLockToDateedit" runat="server" />
    <asp:HiddenField ID="hdnLockFromDatedelete" runat="server" />
    <asp:HiddenField ID="hdnLockToDatedelete" runat="server" />
    <asp:HiddenField runat="server" ID="hdnActiveEInvoice" />
   
</asp:Content>
