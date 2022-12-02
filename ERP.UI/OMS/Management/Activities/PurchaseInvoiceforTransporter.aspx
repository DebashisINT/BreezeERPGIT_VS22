<%@ Page Title="Transporter Bill Entry" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" EnableEventValidation="false"
    AutoEventWireup="true" CodeBehind="PurchaseInvoiceforTransporter.aspx.cs" Inherits="ERP.OMS.Management.Activities.PurchaseInvoiceforTransporter" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>
<%@ Register Src="~/OMS/Management/Activities/UserControls/BillingShippingControl.ascx" TagPrefix="ucBS" TagName="BillingShippingControl" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="https://cdn.datatables.net/1.10.19/css/jquery.dataTables.min.css" rel="stylesheet" />
    <script src="https://cdn.datatables.net/1.10.19/js/jquery.dataTables.min.js"></script>
    <script src="https://cdn.datatables.net/fixedcolumns/3.3.0/js/dataTables.fixedColumns.min.js"></script>
    <link href="CSS/PurchaseInvoiceForTransporter.css" rel="stylesheet" />
    <script src="../../../assests/pluggins/choosen/choosen.min.js"></script>
    <script src="../../Tax%20Details/Js/TaxDetailsItemlevel.js" type="text/javascript"></script>
    <link href="CSS/SearchPopup.css" rel="stylesheet" />
   <%-- <script src="JS/SearchPopup.js"></script>--%>
    <script src="JS/SearchPopupDatatable.js"></script>
    <script src="JS/PurchaseInvoiceForTransporter.js?v=1.5"></script>
    <script>
        function DateCheck() {
            var invtype = $('#ddlInventory').val();
            var type = ($("[id$='rdl_PurchaseInvoice']").find(":checked").val() != null) ? $("[id$='rdl_PurchaseInvoice']").find(":checked").val() : "";
            var endDate = cPLQuoteDate.GetValue();
            var str = $.datepicker.formatDate('yy-mm-dd', endDate);
            var checkval = cchk_reversemechenism.GetChecked();
            //var key = gridLookup.GetValue()
            var key = GetObjectID('hdnCustomerId').value;
            // Waiting for Dirction Start

            // Waiting for Dirction  End 
            if (gridquotationLookup.GetValue() != null) {

                jConfirm('Documents tagged are to be automatically De-selected. Confirm ?', 'Confirmation Dialog', function (r) {
                    if (r == true) {
                        page.SetActiveTabIndex(0);
                        $('.dxeErrorCellSys').addClass('abc');
                        var startDate = new Date();
                        startDate = cPLQuoteDate.GetDate().format('yyyy/MM/dd');
                        cQuotationComponentPanel.PerformCallback('DateCheckOnChanged' + '~' + key + '~' + startDate + '~' + 'DateCheck' + '~' + type + '~' + invtype);
                        var type = ($("[id$='rdl_PurchaseInvoice']").find(":checked").val() != null) ? $("[id$='rdl_PurchaseInvoice']").find(":checked").val() : "";
                        //var key = gridLookup.GetValue()
                        var key = GetObjectID('hdnCustomerId').value;

                        if (key != null && key != '') {
                            var type = ($("[id$='rdl_PurchaseInvoice']").find(":checked").val() != null) ? $("[id$='rdl_PurchaseInvoice']").find(":checked").val() : "";
                            cQuotationComponentPanel.PerformCallback('BindComponentGrid' + '~' + key + '~' + startDate + '~' + 'DateCheck' + '~' + type + '~' + invtype);
                        }
                        grid.PerformCallback('GridBlank');
                        if ("<%=Convert.ToString(Session["TransporterVisibilty"])%>" == "Yes") {
                            clearTransporter(); disc
                        }
                        ccmbGstCstVat.PerformCallback();
                        ccmbGstCstVatcharge.PerformCallback();
                        ctaxUpdatePanel.PerformCallback('DeleteAllTax');
                    }
                });
            }
            else {
                var startDate = new Date();
                startDate = cPLQuoteDate.GetValueString();
                //var key = gridLookup.GetValue()
                var key = GetObjectID('hdnCustomerId').value;
                var type = ($("[id$='rdl_PurchaseInvoice']").find(":checked").val() != null) ? $("[id$='rdl_PurchaseInvoice']").find(":checked").val() : "";
                var componentType = gridquotationLookup.GetGridView().GetRowKey(gridquotationLookup.GetGridView().GetFocusedRowIndex());
                cQuotationComponentPanel.PerformCallback('DateCheckOnChanged' + '~' + key + '~' + startDate + '~' + 'DateCheck' + '~' + type + '~' + invtype);
                if (key != null && key != '' && type != "") {
                    cQuotationComponentPanel.PerformCallback('BindComponentGrid' + '~' + key + '~' + startDate + '~' + 'DateCheck' + '~' + type + '~' + invtype);
                }
                if (componentType != null && componentType != '') {
                    grid.PerformCallback('GridBlank');
                }
            }
        }


        function SetCustomer(Id, Name) {
            var VendorID = Id;
            if (Id != "") {
                var invtype = $('#ddlInventory').val();
                var startDate = new Date();
                startDate = cPLQuoteDate.GetDate().format('yyyy/MM/dd');
                var branchid = $('#ddl_Branch').val();
                var key = Id;
                GetObjectID('hdnCustomerId').value = key;

                // For Checking Shipping AddressOfVendor End  
                if (key != $('#<%=hdnTaggedVender.ClientID %>').val()) {
                    if (gridquotationLookup.GetValue() != null) {
                        jConfirm('Documents tagged are to be automatically De-selected. Confirm ?', 'Confirmation Dialog', function (r) {
                            if (r == true) {
                                var key = GetObjectID('hdnCustomerId').value;
                                if (key != null && key != '') {
                                    if ($('#<%=hdnTaggedVender.ClientID %>').val() != null && $('#<%=hdnTaggedVender.ClientID %>').val() != '') {

                                $('#<%=hdnTaggedVender.ClientID %>').val(key);
                                $('#<%=hdnTaggedVendorName.ClientID %>').val(ctxtVendorName.GetText());
                                //gridquotationLookup.SetText('');
                            }
                            $('.dxeErrorCellSys').addClass('abc');
                            var startDate = new Date();
                            startDate = cPLQuoteDate.GetDate().format('yyyy/MM/dd');
                            var key = GetObjectID('hdnCustomerId').value;

                            if (key != null && key != '') {
                                var type = ($("[id$='rdl_PurchaseInvoice']").find(":checked").val() != null) ? $("[id$='rdl_PurchaseInvoice']").find(":checked").val() : "";
                            }

                            if ("<%=Convert.ToString(Session["TransporterVisibilty"])%>" == "Yes") {
                                clearTransporter();
                            }


                            //###### Added By : Samrat Roy ########## 
                            if ($('#<%=hdnADDEditMode.ClientID %>').val() != 'Edit') {
                                var schemabranchid = $('#ddl_numberingScheme').val();
                                if (schemabranchid != '0') {
                                    var schemabranch = $('#ddl_numberingScheme').val().split('~')[1];
                                    LoadCustomerAddress(key, schemabranch, 'PB');
                                    //page.tabs[0].SetEnabled(true);
                                    page.tabs[1].SetEnabled(true);
                                    //selectValue();
                                }
                            }
                            else if ($('#<%=hdnADDEditMode.ClientID %>').val() == 'Edit') {
                                var schemabranchid = $('#ddl_Branch').val();
                                if (schemabranchid != '0') {
                                    var schemabranch = schemabranchid;
                                    // Geet on 15102017 Start
                                    LoadCustomerAddress(key, schemabranch, 'PB');
                                    // Geet on 15102017 End
                                    //page.tabs[0].SetEnabled(true);
                                    page.tabs[1].SetEnabled(true);
                                }
                            }
                            else {
                                jAlert('Select a numbering schema first');
                                return;
                            }
                        }
                        else {
                            jAlert('Transporter can not be blank.')
                        }
                    }
                    else {
                        var vendorid = $('#<%=hdnTaggedVender.ClientID %>').val();
                        GetObjectID('hdnCustomerId').value = vendorid;
                        ctxtVendorName.SetText($('#<%=hdnTaggedVendorName.ClientID %>').val());

                    }
                });
            }
            else {

                var key = GetObjectID('hdnCustomerId').value;

                if (key != null && key != '') {
                    $('#<%=hdnTaggedVendorName.ClientID %>').val(ctxtVendorName.GetText());

                    if ($('#<%=hdnADDEditMode.ClientID %>').val() != 'Edit') {
                        var schemabranchid = $('#ddl_numberingScheme').val();
                        if (schemabranchid != '0') {
                            var schemabranch = $('#ddl_numberingScheme').val().split('~')[1];

                            LoadCustomerAddress(key, schemabranch, 'PB');

                            page.tabs[1].SetEnabled(true);

                        }
                    }
                    else if ($('#<%=hdnADDEditMode.ClientID %>').val() == 'Edit') {
                        var schemabranchid = $('#ddl_Branch').val();
                        if (schemabranchid != '0') {
                            var schemabranch = schemabranchid;

                            LoadCustomerAddress(key, schemabranch, 'PB');

                            page.tabs[1].SetEnabled(true);
                        }
                    }
                    else {
                        jAlert('Select a numbering schema first');
                        return;
                    }
                }
                else {

                    jAlert('Transporter can not be blank.')
                    $('#ddl_numberingScheme').prop('disabled', false);

                    ctxtVendorName.Focus();
                }
            }
        }
                // Newly added code for vendor search by Sam on 03012018 section End



        $('#CustModel').modal('hide');
        ctxtVendorName.SetText(Name);
                //LoadCustomerAddress(VendorID, $('#ddl_Branch').val(), 'PC');
        page.tabs[0].SetEnabled(true);
        page.tabs[1].SetEnabled(true);
        GetObjectID('hdnCustomerId').value = VendorID;
        if ($('#hfBSAlertFlag').val() == "1") {
            jConfirm('Wish to View/Select Billing and Shipping details?', 'Confirmation Dialog', function (r) {
                if (r == true) {
                    page.SetActiveTabIndex(1);
                }
            });
        }
        ctxt_partyInvNo.Focus();
                //cContactPerson.Focus();
    }
}

function CmbScheme_ValueChange() {
    ctxtVendorName.SetText('');
    GetObjectID('hdnCustomerId').value = '';
    ctaggingList.SetValue('');
    var val = $("#ddl_numberingScheme").val();
    var schemabranch = $('#ddl_numberingScheme').val().split('~')[1];
    if (val != '0') {
        var type = ($("[id$='rdl_PurchaseInvoice']").find(":checked").val() != null) ? $("[id$='rdl_PurchaseInvoice']").find(":checked").val() : "";
        if (type == 'PO' || type == 'PC') {
            selectValue();
        }
        else {
            $.ajax({
                type: "POST",
                url: "purchaseinvoice.aspx/BindBranchByParentID",
                data: JSON.stringify({ schemabranch: schemabranch }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (msg) {
                    var ddl_Branch = $("[id*=ddl_Branch]");
                    var list = msg.d;

                    if (list.length > 0) {
                        $(".lst-clear").empty();
                        var option = document.createElement('option');
                        for (var i = 0; i < list.length; i++) {
                            var id = '';
                            var name = '';
                            id = list[i].split('~')[0];
                            name = list[i].split('~')[1];
                            ddl_Branch.append($("<option></option>").val(id).html(name));
                        }
                    }
                }
            });
        }
        var schemabranchid = val.toString().split('~')[1];
        if ($('#<%=hdnADDEditMode.ClientID %>').val() != 'Edit') {
                    var schemabranchid = $('#ddl_numberingScheme').val();
                    if (schemabranchid != '0') {
                        var schemabranch = $('#ddl_numberingScheme').val().split('~')[1];
                        document.getElementById('ddl_Branch').value = schemabranch;
                    }
                }
                else if ($('#<%=hdnADDEditMode.ClientID %>').val() == 'Edit') {
                    var schemabranchid = $('#ddl_Branch').val();
                    if (schemabranchid != '0') {
                        var schemabranch = schemabranchid;
                        document.getElementById('ddl_Branch').value = schemabranch;
                    }
                }

                $.ajax({
                    type: "POST",
                    url: 'PurchaseInvoice.aspx/getSchemeType',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: "{sel_scheme_id:\"" + val + "\"}",
                    success: function (type) {

                        var schemetypeValue = type.d;
                        var schemetype = schemetypeValue.toString().split('~')[0];
                        var schemelength = schemetypeValue.toString().split('~')[1];
                        var fromdate = schemetypeValue.toString().split('~')[2];
                        var todate = schemetypeValue.toString().split('~')[3];
                        var dt = new Date();
                        cPLQuoteDate.SetDate(dt);
                        if (dt < new Date(fromdate)) {
                            cPLQuoteDate.SetDate(new Date(fromdate));
                        }
                        if (dt > new Date(todate)) {
                            cPLQuoteDate.SetDate(new Date(todate));
                        }
                        cPLQuoteDate.SetMinDate(new Date(fromdate));
                        cPLQuoteDate.SetMaxDate(new Date(todate));
                        $('#txtVoucherNo').attr('maxLength', schemelength);
                        if (schemetype == '0') {
                            document.getElementById('<%= txtVoucherNo.ClientID %>').disabled = false;
                    document.getElementById('<%= txtVoucherNo.ClientID %>').value = "";
                    if ($('#<%=hdnManual.ClientID %>').val() == 'N') {
                        cPLQuoteDate.SetEnabled(false);
                    }
                    else if ($('#<%=hdnManual.ClientID %>').val() == 'Y') {
                        cPLQuoteDate.SetEnabled(true);
                    }
                    $("#txtVoucherNo").focus();
                }
                else if (schemetype == '1') {
                    document.getElementById('<%= txtVoucherNo.ClientID %>').disabled = true;
                    document.getElementById('<%= txtVoucherNo.ClientID %>').value = "Auto";
                    if (cPLQuoteDate.clientEnabled == false) {
                        ctxt_Refference.Focus();
                    }
                    else {
                        cPLQuoteDate.Focus();
                    }
                    $("#MandatoryBillNo").hide();
                    if ($('#<%=hdnAuto.ClientID %>').val() == 'N') {
                        cPLQuoteDate.SetEnabled(false);
                    }
                    else if ($('#<%=hdnAuto.ClientID %>').val() == 'Y') {
                        cPLQuoteDate.SetEnabled(true);
                    }
                    //Mantis Issue 24164
                    if ($("#HdnBackDatedEntryTransporter").val() == "1") {
                        cPLQuoteDate.SetEnabled(true);
                    }
                    else {
                        cPLQuoteDate.SetEnabled(false);
                    }
                    //End of Mantis Issue 24164
                }
                else if (schemetype == '2') {
                    document.getElementById('<%= txtVoucherNo.ClientID %>').disabled = true;
                    document.getElementById('<%= txtVoucherNo.ClientID %>').value = "Datewise";
                }
                else if (schemetype == 'n') {
                    document.getElementById('<%= txtVoucherNo.ClientID %>').disabled = true;
                    document.getElementById('<%= txtVoucherNo.ClientID %>').value = "";
                }
            }
            });
}
else {
    document.getElementById('<%= txtVoucherNo.ClientID %>').disabled = true;
                document.getElementById('<%= txtVoucherNo.ClientID %>').value = "";
                document.getElementById('ddl_Branch').value = '<%=Session["userbranchID"]%>';
            }
            //clookup_Project.gridView.Refresh();
        }

        function DuplicatePartyNo() {

            var invtype = $('#ddlInventory').val();
            var partyno = '';
            if (invtype != 'N') {
                var PBid = ''
                var partyno = ctxt_partyInvNo.GetText();

                if (partyno != null && partyno != '') {
                    $("#MandatorysPartyinvno").hide();
                }
                else {
                    $("#MandatorysPartyinvno").show();
                    return;
                }
                //var vendorid = gridLookup.GetValue()
                var vendorid = GetObjectID('hdnCustomerId').value;
                if (vendorid != null && vendorid != '') {

                    if ($('#<%=hdnADDEditMode.ClientID %>').val() != 'Edit') {
                        mode = 'A'
                    }
                    else {
                        mode = 'E'
                        PBid = "<%=Convert.ToString(Session["PurchaseInvoice_Id"])%>"
            }
            $.ajax({
                type: "POST",
                url: "purchaseinvoice.aspx/CheckUniquePartyNo",
                data: JSON.stringify({ vendorid: vendorid, partyno: partyno, mode: mode, PBid: PBid }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (msg) {
                    var data = msg.d;

                    if (data == true) {
                        $("#DuplicatePartyinvno").show();
                        ctxt_partyInvNo.SetText('');
                        ctxt_partyInvNo.Focus();
                    }
                    else {
                        $("#DuplicatePartyinvno").hide();
                    }
                }
            });

        }
    }
    else {
        $("#MandatorysPartyinvno").hide();
    }
}
function PerformCallToGridBind() {

    if ($('#<%=hdnADDEditMode.ClientID %>').val() != 'Edit') {
                var loadingmade = $('#<%=hdnADDEditMode.ClientID %>').val();
                grid.PerformCallback('BindGridOnQuotation' + '~' + '@');
                $('#hdnPageStatus').val('Quoteupdate');
                cProductsPopup.Hide();
                //#### added by Samrat Roy for Transporter Control #############
                var quote_Id = gridquotationLookup.gridView.GetSelectedKeysOnPage();
                if ("<%=Convert.ToString(Session["TransporterVisibilty"])%>" == "Yes") {
            callTransporterControl(quote_Id[0], $("#rdl_PurchaseInvoice").find(":checked").val());
        }
        if (quote_Id.length > 0) {
            BSDocTagging(quote_Id[0], $("#rdl_PurchaseInvoice").find(":checked").val());
        }
        if ($("#btn_TermsCondition").is(":visible")) {
            callTCControl(quote_Id[0], $("#rdl_PurchaseInvoice").find(":checked").val());
        }
    }
    else {
        cProductsPopup.Hide();
    }
    return false;
}
function OnEndCallback(s, e) {
    var pageStatus = document.getElementById('hdnPageStatus').value;
    var value = document.getElementById('hdnRefreshType').value;
    var pageStatus = document.getElementById('hdnPageStatus').value;
    LoadingPanel.Hide();
    if (grid.cpComponent) {
        if (grid.cpComponent == 'true') {
            grid.cpComponent = null;
            $('#<%=hdfIsComp.ClientID %>').val('');
                    OnAddNewClick();
                }
            }
            if (grid.cpSaveSuccessOrFail == "outrange") {
                cbtn_SaveRecords.SetEnabled(true);
                grid.batchEditApi.StartEdit(0, 2);
                jAlert('Can Not Add More PI for Transpoter Number as PI for Transpoter Scheme Exausted.<br />Update The Scheme and Try Again');
            }
            else if (grid.cpSaveSuccessOrFail == "AddressProblem") {
                cbtn_SaveRecords.SetEnabled(true);
                page.tabs[1].SetEnabled(true);
                jAlert("Billing and Shipping Address can not be blank.Save unsuccessful.", "Alert", function () { $("#exampleModal").modal('show'); });
                grid.cpSaveSuccessOrFail = null;
                OnAddNewClick();
            }
            else if (grid.cpSaveSuccessOrFail == "duplicateProduct") {
                OnAddNewClick();
                cbtn_SaveRecords.SetEnabled(true);
                jAlert("Duplicate Product not allowed.Save unsuccessful.", "Alert", function () { $("#exampleModal").modal('show'); });
                grid.cpSaveSuccessOrFail = null;
            }
            else if (grid.cpSaveSuccessOrFail == "transporteMandatory") {

                cbtn_SaveRecords.SetEnabled(true);
                jAlert("Transporter is set as Mandatory. Please enter values.", "Alert", function () { $("#exampleModal").modal('show'); });
                grid.cpSaveSuccessOrFail = null;

            }
            else if (grid.cpSaveSuccessOrFail == "Dormant_Customer") {

                cbtn_SaveRecords.SetEnabled(true);
                jAlert('You have selected a "Dormant" Transporter. Please change the Status of this Transporter to "Active" to proceed further. ');
                ctxtVendorName.SetEnabled(true);
                grid.cpSaveSuccessOrFail = null;

            }
            else if (grid.cpSaveSuccessOrFail == "TCMandatory") {
                cbtn_SaveRecords.SetEnabled(true);
                jAlert("Terms and Condition is set as Mandatory. Please enter values.", "Alert", function () { $("#TermsConditionseModal").modal('show'); });
                grid.cpSaveSuccessOrFail = null;
                grid.batchEditApi.StartEdit(0, 2);
            }
            else if (grid.cpSaveSuccessOrFail == "duplicate") {
                cbtn_SaveRecords.SetEnabled(true);
                grid.batchEditApi.StartEdit(0, 2);
                jAlert('Can Not Save as Duplicate Quotation Numbe No. Found');
            }
            else if (grid.cpSaveSuccessOrFail == "errorInsert") {
                cbtn_SaveRecords.SetEnabled(true);
                grid.batchEditApi.StartEdit(0, 2);
                jAlert('Please try after sometime.');
            }
            else if (grid.cpSaveSuccessOrFail == "EmptyProject") {
                cbtn_SaveRecords.SetEnabled(true);
                grid.batchEditApi.StartEdit(0, 2);
                jAlert('Please select Project.');
            }

            else if (grid.cpSaveSuccessOrFail == "Ponotexist") {
                cbtn_SaveRecords.SetEnabled(true);
                grid.batchEditApi.StartEdit(0, 2);
                jAlert('Cannot Save. Selected Purchase Indent(s) in this document do not exist.');
            }
                //Registered Vendor Address Checking 
            else if (grid.cpSaveSuccessOrFail == "VendorAddressProblem") {
                cbtn_SaveRecords.SetEnabled(true);
                grid.batchEditApi.StartEdit(0, 2);
                jAlert('You must enter Transporter Billing and Shipping in Transporter Master and set as default to proceed further.');
            }
            else if (grid.cpRVMechMainAc == '-20') {
                cbtn_SaveRecords.SetEnabled(true);
                grid.batchEditApi.StartEdit(0, 2);
                jAlert('Reverse Charge is applicable here. No ledger is found mapped for posting within Masters->Accounts->Tax Component Scheme->"Reverse Charge Posting Ledger". Cannot Proceed.');
                OnAddNewClick();
            }
            else if (grid.cpReturnLedgerAmt == '-3') {
                var dramt = 0;
                var cramt = 0;
                if (grid.cpDRAmt != null) {
                    dramt = grid.cpDRAmt
                }
                if (grid.cpCRAmt != null) {
                    cramt = grid.cpCRAmt
                }
                cbtn_SaveRecords.SetEnabled(true);
                grid.batchEditApi.StartEdit(0, 2);
                //jAlert('Db toatl= ' + dramt + '.......Cr total= ' + cramt + ' Mismatch Detected.<br/>Cannot Save.');
                jAlert('Mismatch detected in total of Debit & Credit Values.<br/>Cannot Save.');
                grid.cpReturnLedgerAmt = null;
                grid.cpDRAmt = null;
                grid.cpCRAmt = null;
                OnAddNewClick();
            }
            else {
                var PurchaseOrder_Number = grid.cpPurchaseOrderNo;
                var Order_Msg = "Purchase Invoice No. " + PurchaseOrder_Number + " saved.";
                var urlKeys = getUrlVars();
                if (value == "E") {
                    cbtn_SaveRecords.SetEnabled(true);
                    if (grid.cpApproverStatus == "approve") {
                        window.parent.popup.Hide();
                        window.parent.cgridPendingApproval.PerformCallback();
                    }
                    else if (grid.cpApproverStatus == "rejected") {
                        window.parent.popup.Hide();
                        window.parent.cgridPendingApproval.PerformCallback();
                    }

                    if (PurchaseOrder_Number != "") {
                        if ($('#<%=hdnPBAutoPrint.ClientID %>').val() == "1") {
                    if (urlKeys.key == 'ADD') {
                        var newInvoiceId = grid.cpGeneratedInvoice;
                        window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=PurchaseInvoiceForTransporterBranch-HalfPage~D&modulename=PINV_TRANSPORTR&id=" + newInvoiceId, '_blank');
                    }
                }
                jAlert(Order_Msg, 'Alert Dialog: [PurchaseInvoice]', function (r) {
                    if (r == true) {
                        grid.cpPurchaseOrderNo = null;
                        grid.cpGeneratedInvoice = null;
                        window.location.assign("PurchaseInvoiceListForTransporter.aspx");
                    }
                });
            }
            else {
                window.location.assign("PurchaseInvoiceListForTransporter.aspx");
            }



        }
        else if (value == "N") {
            cbtn_SaveRecords.SetEnabled(true);
            if (grid.cpApproverStatus == "approve") {
                window.parent.popup.Hide();
                window.parent.cgridPendingApproval.PerformCallback();
            }
            else if (grid.cpApproverStatus == "rejected") {
                window.parent.popup.Hide();
                window.parent.cgridPendingApproval.PerformCallback();
            }
            if (PurchaseOrder_Number != "") {
                if ($('#<%=hdnPBAutoPrint.ClientID %>').val() == "1") {
                    if (urlKeys.key == 'ADD') {
                        var newInvoiceId = grid.cpGeneratedInvoice;
                        window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=PurchaseInvoiceForTransporterBranch-HalfPage~D&modulename=PINV_TRANSPORTR&id=" + newInvoiceId, '_blank');
                    }
                }
                jAlert(Order_Msg, 'Alert Dialog: [PurchaseInvoice]', function (r) {
                    if (r == true) {
                        grid.cpPurchaseOrderNo = null;
                        grid.cpGeneratedInvoice = null;
                        window.location.assign("PurchaseInvoiceforTransporter.aspx?key=ADD");
                    }
                });
            }
            else {
                window.location.assign("PurchaseInvoiceforTransporter.aspx?key=ADD");
            }
        }
        else {
            if (pageStatus == "first") {
                if (grid.GetVisibleRowsOnPage() == 0) {
                    // OnAddNewClick();
                    grid.batchEditApi.EndEdit();
                    $('#<%=hdnPageStatus.ClientID %>').val('');
                    var val = '<%= Session["schemavaluePB"] %>';
                    if (val != '') {
                        $.ajax({
                            type: "POST",
                            url: 'PurchaseInvoiceforTransporter.aspx/getSchemeType',
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            data: "{sel_scheme_id:\"" + val + "\"}",
                            success: function (type) {
                                var schemetypeValue = type.d;
                                var schemetype = schemetypeValue.toString().split('~')[0];
                                var schemelength = schemetypeValue.toString().split('~')[1];
                                $('#txtVoucherNo').attr('maxLength', schemelength);
                                if (schemetype == '0') {
                                    document.getElementById('<%= txtVoucherNo.ClientID %>').disabled = false;
                                    document.getElementById('<%= txtVoucherNo.ClientID %>').value = "";
                                    $("#txtVoucherNo").focus();
                                    if ($('#<%=hdnManual.ClientID %>').val() == 'N') {
                                        cPLQuoteDate.SetEnabled(false);
                                    }
                                    else if ($('#<%=hdnManual.ClientID %>').val() == 'Y') {
                                        cPLQuoteDate.SetEnabled(true);
                                    }
                                }
                                else if (schemetype == '1') {
                                    document.getElementById('<%= txtVoucherNo.ClientID %>').disabled = true;
                                    document.getElementById('<%= txtVoucherNo.ClientID %>').value = "Auto";
                                    cPLQuoteDate.Focus();
                                    $("#MandatoryBillNo").hide();
                                    if ($('#<%=hdnAuto.ClientID %>').val() == 'N') {
                                        cPLQuoteDate.SetEnabled(false);
                                    }
                                    else if ($('#<%=hdnAuto.ClientID %>').val() == 'Y') {
                                        cPLQuoteDate.SetEnabled(true);
                                    }
                                    //Mantis Issue 24164
                                    if ($("#HdnBackDatedEntryTransporter").val() == "1") {
                                        cPLQuoteDate.SetEnabled(true);
                                    }
                                    else {
                                        cPLQuoteDate.SetEnabled(false);
                                    }
                                    //End of Mantis Issue 24164
                                }
                                else if (schemetype == '2') {
                                    document.getElementById('<%= txtVoucherNo.ClientID %>').disabled = true;
                                    document.getElementById('<%= txtVoucherNo.ClientID %>').value = "Datewise";
                                }
                                else if (schemetype == 'n') {
                                    document.getElementById('<%= txtVoucherNo.ClientID %>').disabled = true;
                                    document.getElementById('<%= txtVoucherNo.ClientID %>').value = "";
                                }
                            }
                        });
        }
    }
}
else if (pageStatus == "update") {
    OnAddNewClick();
    $('#<%=hdnPageStatus.ClientID %>').val('');
}
else if (pageStatus == "Quoteupdate") {
    cProductsPopup.Hide();
    grid.StartEditRow(0);
    $('#<%=hdnPageStatus.ClientID %>').val('');
            }
            else if (pageStatus == "delete") {
                var inventoryItem = $('#ddlInventory').val();
                if (inventoryItem == 'N') {
                    var schemeid = cddl_TdsScheme.GetValue()
                }
                OnAddNewClick();
                $('#<%=hdnPageStatus.ClientID %>').val('');
            }

}
}
    if (grid.cpdelete != null && grid.cpdelete != '' && grid.cpdelete != undefined) {
        if (grid.cpdelete == 'Y') {
            $('#<%=hdnDeleteSrlNo.ClientID %>').val('');
        }
    }
    var inventoryItem = $('#ddlInventory').val();
    if (inventoryItem != 'N') {
        if (gridquotationLookup.GetValue() != null) {
            grid.GetEditor('ProductName').SetEnabled(false);
            grid.GetEditor('Description').SetEnabled(false);
            grid.StartEditRow(0);
            $('#<%=hdnPageStatus.ClientID %>').val('');
        }
    }
    if (cchk_reversemechenism.GetValue()) {
        grid.GetEditor('TaxAmount').SetEnabled(false);
    }
    if (grid.cpPurchaseorderbindnewrow == "yes") {
        grid.AddNewRow();
        var noofvisiblerows = grid.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
        var tbQuotation = grid.GetEditor("SrlNo");
        tbQuotation.SetValue(noofvisiblerows);
        grid.cpPurchaseorderbindnewrow = null;
    }
    if (grid.cpOrderRunningBalance) {
        var RunningBalance = grid.cpOrderRunningBalance;
        var RunningSpliteDetails = RunningBalance.split("~");
        grid.cpOrderRunningBalance = null;
        var SUM_ChargesAmount = RunningSpliteDetails[0];
        var SUM_Amount = RunningSpliteDetails[1];
        var SUM_TaxAmount = RunningSpliteDetails[3];
        var SUM_TotalAmount = RunningSpliteDetails[4];
        var SUM_ProductQuantity = parseFloat(RunningSpliteDetails[6]).toFixed(2);
        cTaxableAmtval.SetValue(SUM_Amount);
        cTaxAmtval.SetValue(SUM_TaxAmount);
        ctxt_Charges.SetValue(SUM_ChargesAmount);
        cOtherTaxAmtval.SetValue(SUM_ChargesAmount);
        cInvValue.SetValue(SUM_TotalAmount);
        cTotalAmt.SetValue(SUM_TotalAmount);
        cTotalQty.SetValue(SUM_ProductQuantity);
    }
}
function ddl_Currency_Rate_Change() {
    var Campany_ID = '<%=Session["LastCompany"]%>';
            var LocalCurrency = '<%=Session["LocalCurrency"]%>';
            var basedCurrency = LocalCurrency.split("~");
            var Currency_ID = $("#ddl_Currency").val();
            if (Currency_ID == basedCurrency[0]) {
                ctxtRate.SetValue("0.00");
                ctxtRate.SetEnabled(false);
            }
            else {
                $.ajax({
                    type: "POST",
                    url: "PurchaseInvoiceforTransporter.aspx/GetRate",
                    data: JSON.stringify({ Currency_ID: Currency_ID, Campany_ID: Campany_ID, basedCurrency: basedCurrency[0] }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        var data = msg.d;
                        ctxtRate.SetValue(data);
                    }
                });
                ctxtRate.SetEnabled(true);
            }
        }
    </script>



    <%--Added By : Samrat Roy -- New Billing/Shipping Section End--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title clearfix">
            <h3 class="pull-left">
                <span class="">
                    <asp:Label ID="lblHeading" runat="server" Text="Add Transporter Bill Entry"></asp:Label>
                </span>
            </h3>
            <div id="pageheaderContent" class="scrollHorizontal pull-right reverse wrapHolder content horizontal-images newW" style="display: none; width: 600px;" runat="server">
                <div class="Top clearfix">
                    <ul>

                        <li>
                            <div class="lblHolder" id="divContactPhone" style="display: none;" runat="server">
                                <table>
                                    <tr>
                                        <td>Contact Person's Phone</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblContactPhone" runat="server" Text="N/A" CssClass="classout"></asp:Label></td>
                                    </tr>
                                </table>
                            </div>

                        </li>
                        <li>
                            <div class="lblHolder" id="divOutstanding" style="display: none;" runat="server">
                                <table>
                                    <tr>
                                        <td>Receivable</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblOutstanding" runat="server" Text="0.0" CssClass="classout"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </li>
                        <li>
                            <div class="lblHolder" id="divAvailableStk" style="display: none;">
                                <table>
                                    <tr>
                                        <td>Available Stock</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblAvailableStkPro" runat="server" Text="0.0"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </li>
                        <li>
                            <div class="lblHolder" id="divPacking" style="display: none;">
                                <table>
                                    <tr>
                                        <td>Packing Quantity</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblPackingStk" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </li>
                        <li>
                            <div class="lblHolder" id="divGSTN" style="display: none;" runat="server">
                                <table>
                                    <tr>
                                        <td>GST Registed?</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblGSTIN" runat="server" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </li>
                    </ul>
                    <ul style="display: none;">
                        <li>
                            <div class="lblHolder">
                                <table>
                                    <tr>
                                        <td>Selected Unit</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label1" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </li>
                        <li>
                            <div class="lblHolder">
                                <table>
                                    <tr>
                                        <td>Selected Product</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblProduct" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </li>
                        <li>
                            <div class="lblHolder">
                                <table>
                                    <tr>
                                        <td>Stock Quantity</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblStkQty" runat="server" Text="0.00"></asp:Label>
                                            <asp:Label ID="lblStkUOM" runat="server" Text=" "></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </li>
                    </ul>
                </div>
            </div>
            <div id="ApprovalCross" runat="server" class="crossBtn">
                <a href=""><i class="fa fa-times"></i></a>
            </div>
            <%--<div id="divcross1" runat="server" class="crossBtn" margin-left: 50px;">--%>
            <div id="crossdiv" runat="server" class="crossBtn">
                <a href="PurchaseInvoiceListForTransporter.aspx"><i class="fa fa-times"></i></a>
            </div>
            <%--</div>--%>
        </div>
    </div>
    <div class="form_main">
        <div class="row">
            <dxe:ASPxPageControl ID="ASPxPageControl1" runat="server" ClientInstanceName="page" Width="100%">
                <TabPages>
                    <dxe:TabPage Name="General" Text="General">
                        <ContentCollection>
                            <dxe:ContentControl runat="server">
                                <div class="row">
                                    <div class="col-md-2">
                                        <div class="cityDiv " style="height: auto;">
                                            <asp:Label ID="Label12" runat="server" Text="Type" CssClass="newLbl"></asp:Label>
                                        </div>
                                        <div class="Left_Content">
                                            <asp:DropDownList ID="ddlInventory" runat="server" Width="100%" CssClass="backSelect" onchange="ddlInventory_OnChange()">
                                                <%-- <asp:ListItem Value="B">Both</asp:ListItem>
                                                <asp:ListItem Value="Y">Inventory Items</asp:ListItem>--%>
                                                <asp:ListItem Value="N">Non-Inventory Items</asp:ListItem>
                                                <%-- <asp:ListItem Value="C">Capital Goods</asp:ListItem>--%>
                                                <asp:ListItem Value="S">Service</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="col-md-2" runat="server" id="divNumberingScheme">
                                        <dxe:ASPxLabel ID="lbl_NumberingScheme" Width="120px" runat="server" Text="Numbering Scheme">
                                        </dxe:ASPxLabel>
                                        <asp:DropDownList ID="ddl_numberingScheme" runat="server" Width="100%" DataSourceID="SqlSchematype" DataTextField="SchemaName" DataValueField="ID"
                                            onchange="CmbScheme_ValueChange()">
                                        </asp:DropDownList>
                                        <%-- --%>
                                    </div>
                                    <div class="col-md-2">
                                        <dxe:ASPxLabel ID="lbl_PIQuoteNo" runat="server" Text="Document No.">
                                        </dxe:ASPxLabel>
                                        <span style="color: red;">*</span>
                                        <asp:TextBox ID="txtVoucherNo" runat="server" Width="100%" MaxLength="50" onchange="txtBillNo_TextChanged()">                             
                                        </asp:TextBox>
                                        <span id="MandatoryBillNo" class="voucherno  pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                        <span id="DuplicateBillNo" class="voucherno  pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Duplicate Bill Number not allowed"></span>

                                    </div>
                                    <div class="col-md-2">
                                        <dxe:ASPxLabel ID="ASPxLabel2" runat="server" Text="Posting Date">
                                        </dxe:ASPxLabel>
                                        <span style="color: red;">*</span>
                                        <dxe:ASPxDateEdit ID="dt_PLQuote" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cPLQuoteDate" Width="100%">
                                            <ButtonStyle Width="13px">
                                            </ButtonStyle>
                                            <ClientSideEvents DateChanged="function(s, e) { DateCheck()}" GotFocus="function(s,e){cPLQuoteDate.ShowDropDown();}" />
                                        </dxe:ASPxDateEdit>
                                        <span id="MandatoryDate" class="PODate  pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>

                                    </div>
                                    <div class="col-md-2">
                                        <dxe:ASPxLabel ID="lbl_Branch" runat="server" Text="for Unit">
                                        </dxe:ASPxLabel>
                                        <span style="color: red;">*</span>
                                        <asp:DropDownList ID="ddl_Branch" runat="server" Width="100%" onchange="CmbBranch_ValueChange()" CssClass="lst-clear">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-md-2">
                                        <dxe:ASPxLabel ID="lbl_Refference" runat="server" Text="Reference">
                                        </dxe:ASPxLabel>
                                        <dxe:ASPxTextBox ID="txt_Refference" runat="server" Width="100%" ClientInstanceName="ctxt_Refference">
                                        </dxe:ASPxTextBox>
                                    </div>
                                    <div style="clear: both"></div>
                                    <div class="col-md-2 lblBot">
                                        <dxe:ASPxLabel ID="lbl_Customer" runat="server" Text="Transporter">
                                        </dxe:ASPxLabel>
                                        <span style="color: red;">*</span>
                                        <dxe:ASPxButtonEdit ID="txtVendorName" runat="server" ReadOnly="true" ClientInstanceName="ctxtVendorName" Width="100%">
                                            <Buttons>
                                                <dxe:EditButton>
                                                </dxe:EditButton>
                                            </Buttons>
                                            <ClientSideEvents ButtonClick="function(s,e){VendorButnClick();}" KeyDown="function(s,e){VendorKeyDown(s,e);}" />
                                        </dxe:ASPxButtonEdit>
                                        <span id="MandatorysVendor" class="customerno pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>

                                        <dxe:ASPxCallbackPanel runat="server" ID="vendorPanel" ClientInstanceName="cvendorPanel" OnCallback="vendorPanel_Callback">
                                            <PanelCollection>

                                                <dxe:PanelContent runat="server">
                                                </dxe:PanelContent>
                                            </PanelCollection>
                                        </dxe:ASPxCallbackPanel>
                                    </div>
                                    <%-- Code Added by Sam on 25052017--%>
                                    <dxe:ASPxCallbackPanel runat="server" ID="partyInvoicepanel" ClientInstanceName="cpartyInvoicepanel" OnCallback="partyInvoicepanel_Callback">
                                        <PanelCollection>
                                            <dxe:PanelContent runat="server">
                                                <div class="col-md-2 lblBot">
                                                    <dxe:ASPxLabel ID="lbl_partyInvNo" runat="server" Text="Party Invoice No">
                                                    </dxe:ASPxLabel>
                                                    <dxe:ASPxTextBox ID="txt_partyInvNo" runat="server" Width="100%" ClientInstanceName="ctxt_partyInvNo" MaxLength="16">
                                                        <ClientSideEvents LostFocus="DuplicatePartyNo" />
                                                        <%--<ClientSideEvents  GotFocus="function(s,e){ctxt_partyInvNo.ShowDropDown();}" />--%>
                                                    </dxe:ASPxTextBox>
                                                    <span id="MandatorysPartyinvno" class="POVendor  pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                                    <span id="DuplicatePartyinvno" class="POVendor  pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Party Invoice No. already exist for the selected vendor."></span>

                                                </div>

                                                <div class="col-md-2 lblBot">
                                                    <dxe:ASPxLabel ID="lbl_partyInvDt" runat="server" Text="Party Invoice Date">
                                                    </dxe:ASPxLabel>

                                                    <dxe:ASPxDateEdit ID="dt_partyInvDt" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cdt_partyInvDt"
                                                        Width="100%">
                                                        <ButtonStyle Width="13px">
                                                        </ButtonStyle>
                                                        <ClientSideEvents LostFocus="partyInvDtMandatorycheck" GotFocus="function(s,e){cdt_partyInvDt.ShowDropDown();}" />
                                                    </dxe:ASPxDateEdit>
                                                    <span id="MandatoryPartyDate" class="PODate  pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                                    <span id="MandatoryEgSDate" class="PODate  pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Party Invoice Date can not be greater than Invoice Date"></span>
                                                    <%--                                            <span id="MandatoryDate" class="PODate  pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>--%>
                                                </div>

                                            </dxe:PanelContent>
                                        </PanelCollection>
                                    </dxe:ASPxCallbackPanel>
                                    <%--Code added by Sam on 25052017  --%>
                                    <div class="col-md-2 lblBot">
                                        <dxe:ASPxLabel ID="lbl_ContactPerson" runat="server" Text="Contact Person">
                                        </dxe:ASPxLabel>
                                        <dxe:ASPxComboBox ID="cmbContactPerson" runat="server" OnCallback="cmbContactPerson_Callback" Width="100%" ClientInstanceName="cContactPerson" Font-Size="12px"
                                            ClientSideEvents-EndCallback="cmbContactPersonEndCall">
                                            <ClientSideEvents TextChanged="function(s, e) { GetContactPersonPhone(e)}" GotFocus="function(s,e){cContactPerson.ShowDropDown();}" />
                                        </dxe:ASPxComboBox>
                                    </div>
                                    <div class="col-md-2 relative" id="rdlbutton" runat="server">
                                        <asp:RadioButtonList ID="rdl_PurchaseInvoice" runat="server" RepeatDirection="Horizontal" onchange="return selectValue();" Width="150px">
                                            <asp:ListItem Text="Order" Value="PO"></asp:ListItem>
                                            <asp:ListItem Text="GRN" Value="PC"></asp:ListItem>
                                        </asp:RadioButtonList>
                                        <dxe:ASPxCallbackPanel runat="server" ID="ComponentQuotationPanel" ClientInstanceName="cQuotationComponentPanel"
                                            OnCallback="ComponentQuotation_Callback">
                                            <PanelCollection>
                                                <dxe:PanelContent runat="server">
                                                    <dxe:ASPxGridLookup ID="lookup_quotation" SelectionMode="Multiple" AllowUserInput="false" runat="server" ClientInstanceName="gridquotationLookup"
                                                        OnDataBinding="lookup_quotation_DataBinding"
                                                        KeyFieldName="ComponentID" Width="100%" TextFormatString="{0}" AutoGenerateColumns="False" MultiTextSeparator=", ">

                                                        <Columns>
                                                            <dxe:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Width="60" Caption=" " />
                                                            <dxe:GridViewDataColumn FieldName="ComponentNumber" Visible="true" VisibleIndex="1" Caption="Document Number" Settings-AutoFilterCondition="Contains" Width="120" />
                                                            <dxe:GridViewDataColumn FieldName="ComponentDate" Visible="true" VisibleIndex="2" Caption="Posting Date" Settings-AutoFilterCondition="Contains" Width="120" />
                                                            <dxe:GridViewDataColumn FieldName="CustomerName" Visible="true" VisibleIndex="3" Caption="Vendor Name" Settings-AutoFilterCondition="Contains" Width="120" />
                                                            <dxe:GridViewDataColumn FieldName="branch" Visible="true" VisibleIndex="4" Caption="Unit" Settings-AutoFilterCondition="Contains" Width="120" />
                                                            <dxe:GridViewDataColumn FieldName="reference" Visible="true" VisibleIndex="5" Caption="Reference" Settings-AutoFilterCondition="Contains" Width="120" />
                                                            <dxe:GridViewDataColumn FieldName="Invtype" Visible="true" VisibleIndex="5" Caption="Type" Settings-AutoFilterCondition="Contains" Width="120" />
                                                            <dxe:GridViewDataColumn FieldName="PartyInvoiceNo" Visible="true" VisibleIndex="5" Caption="Party Invoice No" Settings-AutoFilterCondition="Contains" Width="120" />
                                                            <dxe:GridViewDataColumn FieldName="PartyInvoiceDate" Visible="true" VisibleIndex="5" Caption="Party Invoice Date" Settings-AutoFilterCondition="Contains" Width="120" />
                                                        </Columns>
                                                        <GridViewProperties Settings-VerticalScrollBarMode="Auto" SettingsPager-Mode="ShowAllRecords">
                                                            <Templates>
                                                                <StatusBar>
                                                                    <table class="OptionsTable" style="float: right">
                                                                        <tr>
                                                                            <td>
                                                                                <dxe:ASPxButton ID="Close" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseGridQuotationLookup" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </StatusBar>
                                                            </Templates>
                                                            <SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True" />
                                                            <SettingsPager Mode="ShowAllRecords">
                                                            </SettingsPager>
                                                            <Settings ShowFilterRow="True" ShowFilterRowMenu="true" ShowStatusBar="Visible" UseFixedTableLayout="true" />
                                                        </GridViewProperties>
                                                        <ClientSideEvents ValueChanged="function(s, e) { QuotationNumberChanged();}" GotFocus="function(s,e){gridquotationLookup.ShowDropDown();}" />
                                                        <%-- GotFocus="DisableDeleteOption"--%>
                                                    </dxe:ASPxGridLookup>
                                                    <%--GotFocus="function(s,e){gridquotationLookup.ShowDropDown();}"--%>
                                                </dxe:PanelContent>
                                            </PanelCollection>
                                            <ClientSideEvents EndCallback="componentEndCallBack" />
                                        </dxe:ASPxCallbackPanel>
                                        <dxe:ASPxPopupControl ID="ASPxProductsPopup" runat="server" ClientInstanceName="cProductsPopup"
                                            Width="900px" HeaderText="Select Tax" PopupHorizontalAlign="WindowCenter"
                                            PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
                                            Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True">
                                            <HeaderTemplate>
                                                <strong><span style="color: #fff">Select Products</span></strong>
                                                <dxe:ASPxImage ID="ASPxImage3" runat="server" ImageUrl="/assests/images/closePop.png" Cursor="pointer" CssClass="popUpHeader pull-right">
                                                    <ClientSideEvents Click="function(s, e){ 
                                                                                        cProductsPopup.Hide();
                                                                                    }" />
                                                </dxe:ASPxImage>
                                            </HeaderTemplate>
                                            <ContentCollection>
                                                <dxe:PopupControlContentControl runat="server">
                                                    <div style="padding: 7px 0;" id="divselectunselect" runat="server">
                                                        <input type="button" value="Select All Products" onclick="ChangeState('SelectAll')" class="btn btn-primary"></input>
                                                        <input type="button" value="De-select All Products" onclick="ChangeState('UnSelectAll')" class="btn btn-primary"></input>
                                                        <input type="button" value="Revert" onclick="ChangeState('Revart')" class="btn btn-primary"></input>
                                                    </div>
                                                    <dxe:ASPxGridView runat="server" KeyFieldName="ComponentDetailsID" ClientInstanceName="cgridproducts" ID="grid_Products"
                                                        Width="100%" SettingsBehavior-AllowSort="false" SettingsBehavior-AllowDragDrop="false" SettingsPager-Mode="ShowAllRecords" OnCustomCallback="cgridProducts_CustomCallback"
                                                        Settings-ShowFooter="false" AutoGenerateColumns="False" Settings-VerticalScrollableHeight="300" Settings-VerticalScrollBarMode="Visible" OnCommandButtonInitialize="grid_Products_CommandButtonInitialize">
                                                        <SettingsBehavior AllowDragDrop="False" AllowSort="False"></SettingsBehavior>
                                                        <SettingsPager Visible="false"></SettingsPager>
                                                        <Columns>
                                                            <dxe:GridViewCommandColumn ShowSelectCheckbox="True" Width="40" Caption=" " VisibleIndex="0" />
                                                            <dxe:GridViewDataTextColumn VisibleIndex="1" FieldName="SrlNo" Width="40" ReadOnly="true" Caption="Sl. No.">
                                                            </dxe:GridViewDataTextColumn>
                                                            <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="ComponentNumber" Width="120" ReadOnly="true" Caption="Number">
                                                            </dxe:GridViewDataTextColumn>
                                                            <dxe:GridViewDataTextColumn VisibleIndex="3" FieldName="ProductID" ReadOnly="true" Caption="Product" Width="0">
                                                            </dxe:GridViewDataTextColumn>
                                                            <dxe:GridViewDataTextColumn VisibleIndex="4" FieldName="ProductsName" ReadOnly="true" Width="100" Caption="Product Name">
                                                            </dxe:GridViewDataTextColumn>
                                                            <dxe:GridViewDataTextColumn VisibleIndex="5" FieldName="ProductDescription" Width="200" ReadOnly="true" Caption="Product Description">
                                                            </dxe:GridViewDataTextColumn>
                                                            <dxe:GridViewDataTextColumn Caption="Balance Quantity" FieldName="Quantity" Width="70" VisibleIndex="6">
                                                                <PropertiesTextEdit>
                                                                    <MaskSettings Mask="<0..999999999999>.<0..99>" AllowMouseWheel="false" />
                                                                </PropertiesTextEdit>
                                                            </dxe:GridViewDataTextColumn>
                                                            <dxe:GridViewDataTextColumn VisibleIndex="7" FieldName="ComponentID" ReadOnly="true" Caption="ComponentID" Width="0">
                                                            </dxe:GridViewDataTextColumn>
                                                        </Columns>
                                                        <SettingsDataSecurity AllowEdit="true" />
                                                        <ClientSideEvents EndCallback="HideSelectAllSection" />
                                                    </dxe:ASPxGridView>

                                                    <div class="text-center">


                                                        <dxe:ASPxButton ID="Button2" ClientInstanceName="cButton2" runat="server" AutoPostBack="False" Text="OK" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                                                            <ClientSideEvents Click="function(s, e) {return PerformCallToGridBind();}" />
                                                        </dxe:ASPxButton>
                                                    </div>
                                                </dxe:PopupControlContentControl>
                                            </ContentCollection>
                                            <ContentStyle VerticalAlign="Top" CssClass="pad"></ContentStyle>
                                            <HeaderStyle BackColor="LightGray" ForeColor="Black" />
                                        </dxe:ASPxPopupControl>
                                    </div>
                                    <div class="col-md-2 lblBot" id="rdldate" runat="server">
                                        <dxe:ASPxLabel ID="lbl_InvoiceNO" runat="server" Text="Order/GRN Date" ClientInstanceName="clbl_InvoiceNO">
                                        </dxe:ASPxLabel>
                                        <div style="width: 100%; height: 23px">
                                            <dxe:ASPxTextBox ID="dt_Quotation" runat="server" Width="100%" DisplayFormatString="dd-MM-yyyy" ClientEnabled="false" ClientInstanceName="cPLQADate">
                                            </dxe:ASPxTextBox>
                                        </div>
                                    </div>
                                    <div style="clear: both"></div>
                                    <div class="col-md-2 lblmTop8 hide">
                                        <dxe:ASPxLabel ID="ASPxLabel1" runat="server" Text="Cash">
                                        </dxe:ASPxLabel>
                                        <dxe:ASPxComboBox ID="ddl_cashBank" runat="server" ClientIDMode="Static" ClientInstanceName="cddl_cashBank" Width="100%">
                                            <ClientSideEvents GotFocus="function(s,e){cddl_cashBank.ShowDropDown();}" />
                                        </dxe:ASPxComboBox>
                                    </div>
                                    <div class="col-md-2 ">
                                        <dxe:ASPxLabel ID="lbl_Currency" runat="server" Text="Currency">
                                        </dxe:ASPxLabel>
                                        <asp:DropDownList ID="ddl_Currency" runat="server" Width="100%" onchange="ddl_Currency_Rate_Change()">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-md-2 ">
                                        <dxe:ASPxLabel ID="lbl_Rate" runat="server" Text="Rate">
                                        </dxe:ASPxLabel>
                                        <dxe:ASPxTextBox ID="txt_Rate" runat="server" Width="100%" ClientInstanceName="ctxtRate" CssClass="number" DisplayFormatString="0.00">
                                            <ClientSideEvents LostFocus="ReBindGrid_Currency" GotFocus="GetPreviousCurrency" />
                                        </dxe:ASPxTextBox>
                                    </div>
                                    <div id="divreverse" class="col-md-2 " style="padding-top: 18px;">
                                        <dxe:ASPxCheckBox ID="chk_reversemechenism" ClientInstanceName="cchk_reversemechenism" runat="server">
                                            <ClientSideEvents CheckedChanged="RCMCheckChanged" />
                                        </dxe:ASPxCheckBox>
                                        <dxe:ASPxLabel ID="ASPxLabel3" runat="server" Text="Reverse Mechanism">
                                        </dxe:ASPxLabel>
                                    </div>
                                    <%--SamModification after --%>
                                    <div id="divTdsScheme" class="col-md-2 " runat="server">
                                        <dxe:ASPxLabel ID="ASPxLabel4" runat="server" Text="Select TDS Section">
                                        </dxe:ASPxLabel>
                                        <span style="color: red;">*</span>
                                        <dxe:ASPxComboBox ID="ddl_TdsScheme" runat="server" OnCallback="ddl_TdsScheme_Callback" Width="100%" ClientInstanceName="cddl_TdsScheme" Font-Size="12px">
                                            <ClientSideEvents TextChanged="function(s, e) { GridProductBind(e)}" />
                                        </dxe:ASPxComboBox>
                                        <span id="MandatoryTDS" class="TDsSection  pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                    </div>
                                    <%--SamModification after --%>
                                    <div class="col-md-2 ">
                                        <dxe:ASPxLabel ID="lbl_AmountAre" runat="server" Text="Amounts are">
                                        </dxe:ASPxLabel>
                                        <dxe:ASPxComboBox ID="ddl_AmountAre" runat="server" ClientIDMode="Static" ClientInstanceName="cddl_AmountAre" Width="100%">
                                            <ClientSideEvents SelectedIndexChanged="function(s, e) { PopulateGSTCSTVAT(e)}" />
                                            <ClientSideEvents GotFocus="function(s,e){cddl_AmountAre.ShowDropDown();}" />
                                        </dxe:ASPxComboBox>
                                    </div>
                                    <div class="col-md-2 lblmTop8 hide" style="margin-bottom: 15px">
                                        <dxe:ASPxLabel ID="lblVatGstCst" runat="server" Text="Select GST">
                                        </dxe:ASPxLabel>
                                        <dxe:ASPxComboBox ID="ddl_VatGstCst" runat="server" ClientInstanceName="cddlVatGstCst" Width="100%" OnCallback="ddl_VatGstCst_Callback">
                                            <ClientSideEvents EndCallback="Onddl_VatGstCstEndCallback" GotFocus="function(s,e){cddlVatGstCst.ShowDropDown();}" />
                                        </dxe:ASPxComboBox>
                                    </div>
                                    <div class="col-md-2">
                                        <dxe:ASPxLabel ID="ASPxLabel10" runat="server" Text="Transporter Type">
                                        </dxe:ASPxLabel>
                                        <dxe:ASPxComboBox ID="ddl_vendortype" runat="server" ClientInstanceName="cddl_vendortype" Width="100%" ClientEnabled="false">
                                            <Items>
                                                <dxe:ListEditItem Text="None" Value="R" Selected="true" />
                                                <dxe:ListEditItem Text="Composite" Value="C" />
                                            </Items>
                                        </dxe:ASPxComboBox>
                                    </div>
                                    <div style="clear: both;"></div>
                                    <div class="col-md-2 lblmTop8">
                                        <dxe:ASPxLabel ID="lbl_PBBill" runat="server" Text="Select Bill">
                                        </dxe:ASPxLabel>
                                        <dxe:ASPxButtonEdit ID="taggingList" ClientInstanceName="ctaggingList" runat="server" ReadOnly="true" Width="100%">
                                            <Buttons>
                                                <dxe:EditButton>
                                                </dxe:EditButton>
                                            </Buttons>
                                            <ClientSideEvents ButtonClick="taggingListButnClick" KeyDown="taggingListKeyDown" />
                                        </dxe:ASPxButtonEdit>

                                    </div>
                                    <div class="col-md-2 lblmTop8" style="padding-top: 7px;">
                                        <dxe:ASPxLabel ID="ASPxLabel11" runat="server" Text="Close">
                                        </dxe:ASPxLabel>
                                        <dxe:ASPxCheckBox ID="chk_Isclosed" ClientInstanceName="cchk_Isclosed" runat="server" Checked="true">
                                        </dxe:ASPxCheckBox>
                                    </div>
                                    <%--New Modification For Entry Date on 15022018 by Sam Section Start--%>
                                    <div class="col-md-2 lblmTop8" style="margin-bottom: 15px">
                                        <dxe:ASPxLabel ID="ASPxLabel12" runat="server" Text="Entry Date">
                                        </dxe:ASPxLabel>


                                        <dxe:ASPxDateEdit ID="dt_EntryDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cdt_EntryDate" Width="100%" ClientEnabled="false">
                                            <ButtonStyle Width="13px">
                                            </ButtonStyle>

                                        </dxe:ASPxDateEdit>
                                    </div>
                                    <%-- New Modification For Entry Date on 15022018 by Sam Section End--%>
                                    <div class="col-md-2 lblmTop8">
                                        <dxe:ASPxLabel ID="lblProject" runat="server" Text="Project"></dxe:ASPxLabel>
                                        <dxe:ASPxGridLookup ID="lookup_Project" runat="server" ClientInstanceName="clookup_Project" DataSourceID="EntityServerModeDataSalesOrder"
                                            KeyFieldName="Proj_Id" Width="100%" TextFormatString="{0}" AutoGenerateColumns="False">
                                            <Columns>
                                                <dxe:GridViewDataColumn FieldName="Proj_Code" Visible="true" VisibleIndex="1" Caption="Project Code" Settings-AutoFilterCondition="Contains" Width="200px">
                                                </dxe:GridViewDataColumn>
                                                <dxe:GridViewDataColumn FieldName="Proj_Name" Visible="true" VisibleIndex="2" Caption="Project Name" Settings-AutoFilterCondition="Contains" Width="200px">
                                                </dxe:GridViewDataColumn>
                                                <dxe:GridViewDataColumn FieldName="Customer" Visible="true" VisibleIndex="3" Caption="Customer" Settings-AutoFilterCondition="Contains" Width="200px">
                                                </dxe:GridViewDataColumn>
                                                <dxe:GridViewDataColumn FieldName="HIERARCHY_NAME" Visible="true" VisibleIndex="4" Caption="Hierarchy" Settings-AutoFilterCondition="Contains" Width="200px">
                                                </dxe:GridViewDataColumn>
                                            </Columns>
                                            <GridViewProperties Settings-VerticalScrollBarMode="Auto">
                                                <Templates>
                                                    <StatusBar>
                                                        <table class="OptionsTable" style="float: right">
                                                            <tr>
                                                                <td></td>
                                                            </tr>
                                                        </table>
                                                    </StatusBar>
                                                </Templates>
                                                <SettingsBehavior AllowFocusedRow="True" AllowSelectSingleRowOnly="True"></SettingsBehavior>
                                                <Settings ShowFilterRow="True" ShowFilterRowMenu="true" ShowStatusBar="Visible" UseFixedTableLayout="true" />
                                            </GridViewProperties>
                                            <ClientSideEvents GotFocus="Project_gotFocus" LostFocus="clookup_Project_LostFocus" ValueChanged="ProjectValueChange" />
                                            <ClearButton DisplayMode="Always">
                                            </ClearButton>
                                        </dxe:ASPxGridLookup>
                                        <dx:LinqServerModeDataSource ID="EntityServerModeDataSalesOrder" runat="server" OnSelecting="EntityServerModeDataSalesOrder_Selecting"
                                            ContextTypeName="ERPDataClassesDataContext" TableName="ProjectCodeBind" />
                                    </div>
                                    <div class="col-md-4 lblmTop8">
                                        <dxe:ASPxLabel ID="lblHierarchy" runat="server" Text="Hierarchy">
                                        </dxe:ASPxLabel>
                                        <asp:DropDownList ID="ddlHierarchy" runat="server" Width="100%" Enabled="false">
                                        </asp:DropDownList>
                                    </div>

                                    <div style="clear: both;"></div>
                                    <div class="col-md-10 ">
                                        <dxe:ASPxLabel ID="ASPxLabel9" runat="server" Text="Remarks / Narration">
                                        </dxe:ASPxLabel>
                                        <asp:TextBox ID="txtRemarks" runat="server" MaxLength="500"></asp:TextBox>
                                    </div>
                                    <div style="clear: both;"></div>
                                    <div class="col-md-12 lblmTop8">
                                        <div class="makeFullscreen ">
                                            <span class="fullScreenTitle">Add Transporter Bill Entry</span>
                                            <span class="makeFullscreen-icon half hovered " data-instance="grid" title="Maximize Grid" id="expandgrid">
                                                <i class="fa fa-expand"></i>
                                            </span>
                                            <dxe:ASPxGridView runat="server" OnBatchUpdate="grid_BatchUpdate" KeyFieldName="PurchaseInvoiceDetailID"
                                                ClientInstanceName="grid" ID="grid"
                                                Width="100%" SettingsBehavior-AllowSort="false" SettingsBehavior-AllowDragDrop="false"
                                                OnCellEditorInitialize="grid_CellEditorInitialize" OnHtmlRowPrepared="grid_HtmlRowPrepared"
                                                Settings-ShowFooter="false" OnCustomCallback="grid_CustomCallback" OnDataBinding="grid_DataBinding"
                                                OnRowInserting="Grid_RowInserting" OnRowUpdating="Grid_RowUpdating" OnRowDeleting="Grid_RowDeleting"
                                                Settings-VerticalScrollBarMode="auto" Settings-VerticalScrollableHeight="150" RowHeight="2" OnDataBound="grid_DataBound"
                                                OnCustomColumnDisplayText="grid_CustomColumnDisplayText">

                                                <SettingsPager Mode="ShowAllRecords"></SettingsPager>

                                                <Columns>
                                                    <dxe:GridViewCommandColumn ShowDeleteButton="false" ShowNewButtonInHeader="false" Width="2.5%" VisibleIndex="0"
                                                        Caption="">
                                                        <CustomButtons>
                                                            <dxe:GridViewCommandColumnCustomButton Text=" " ID="CustomDelete" Image-Url="/assests/images/crs.png">
                                                            </dxe:GridViewCommandColumnCustomButton>
                                                        </CustomButtons>
                                                    </dxe:GridViewCommandColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="SrlNo" Caption="Sl" ReadOnly="true" VisibleIndex="1" Width="2%">
                                                        <PropertiesTextEdit>
                                                        </PropertiesTextEdit>
                                                    </dxe:GridViewDataTextColumn>
                                                    <%--Batch Product Popup Start--%>
                                                    <%--<dxe:GridViewDataTextColumn Caption="Indent" FieldName="Indent_Num" ReadOnly="True" Width="80" VisibleIndex="2">
                                                        <PropertiesTextEdit>
                                                        </PropertiesTextEdit>
                                                    </dxe:GridViewDataTextColumn>--%>
                                                    <dxe:GridViewDataTextColumn FieldName="ComponentNumber" Caption="Document No." VisibleIndex="2" ReadOnly="True" Width="8%">
                                                        <CellStyle Wrap="True"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataButtonEditColumn FieldName="ProductName" Caption="Product" VisibleIndex="3" Width="11%" ReadOnly="True">
                                                        <PropertiesButtonEdit>
                                                            <ClientSideEvents ButtonClick="ProductButnClick" KeyDown="ProductKeyDown" GotFocus="ProductsGotFocusFromID" LostFocus="ProductsGotFocus" />
                                                            <Buttons>
                                                                <dxe:EditButton Text="..." Width=".5%">
                                                                </dxe:EditButton>
                                                            </Buttons>
                                                        </PropertiesButtonEdit>
                                                    </dxe:GridViewDataButtonEditColumn>
                                                    <%--<dxe:GridViewDataTextColumn FieldName="ProductID" Caption="hidden Field Id" VisibleIndex="16" ReadOnly="True" Width="0" EditCellStyle-CssClass="hide" PropertiesTextEdit-FocusedStyle-CssClass="hide" PropertiesTextEdit-Style-CssClass="hide" PropertiesTextEdit-Height="15px">
                                                        <CellStyle Wrap="True" CssClass="hide"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>--%>

                                                    <%--Batch Product Popup End--%>
                                                    <%-- <dxe:GridViewDataComboBoxColumn Caption="Product" FieldName="ProductID" VisibleIndex="2" Width="15%">
                                                        <PropertiesComboBox ValueField="ProductID" ClientInstanceName="ProductID" TextField="ProductName"  EnableCallbackMode="true" CallbackPageSize="100">
                                                             <ClientSideEvents SelectedIndexChanged="ProductsCombo_SelectedIndexChanged" GotFocus="ProductsGotFocus" />
                                                        </PropertiesComboBox>
                                                          <CellStyle Wrap="True"></CellStyle>
                                                    </dxe:GridViewDataComboBoxColumn>--%>
                                                    <%--<dxe:GridViewDataTextColumn FieldName="Description" Caption="Description" VisibleIndex="4"  Width="200"  >
                                                        <PropertiesTextEdit>
                                                        </PropertiesTextEdit>
                                                         <CellStyle Wrap="True"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>--%>
                                                    <dxe:GridViewDataTextColumn FieldName="Description" Caption="Description" VisibleIndex="4" ReadOnly="True" Width="14%">
                                                        <CellStyle Wrap="True"></CellStyle>

                                                    </dxe:GridViewDataTextColumn>

                                                    <dxe:GridViewCommandColumn VisibleIndex="5" Caption="Addl. Desc." Width="6%">
                                                        <CustomButtons>
                                                            <dxe:GridViewCommandColumnCustomButton Text=" " ID="addlDesc" Image-Url="/assests/images/more.png" Image-ToolTip="Warehouse">
                                                                <Image ToolTip="Warehouse" Url="/assests/images/more.png">
                                                                </Image>
                                                            </dxe:GridViewCommandColumnCustomButton>
                                                        </CustomButtons>
                                                    </dxe:GridViewCommandColumn>


                                                    <dxe:GridViewDataTextColumn FieldName="Quantity" Caption="Quantity" VisibleIndex="6" Width="7%"
                                                        HeaderStyle-HorizontalAlign="Right">
                                                        <PropertiesTextEdit DisplayFormatString="0.0000" Style-HorizontalAlign="Right"></PropertiesTextEdit>
                                                        <PropertiesTextEdit>
                                                            <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..9999&gt;" AllowMouseWheel="false" />
                                                            <ClientSideEvents LostFocus="QuantityTextChange" GotFocus="QuantityGotFocus" />
                                                            <%-- LostFocus="QuantityTextChange"--%>
                                                        </PropertiesTextEdit>
                                                        <CellStyle HorizontalAlign="Right"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="UOM" Caption="UOM(Purc.)" VisibleIndex="7" Width="6%" ReadOnly="true">
                                                        <PropertiesTextEdit>
                                                            <%-- <ClientSideEvents LostFocus="QuantityTextChange" />--%>
                                                        </PropertiesTextEdit>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewCommandColumn Width="5.5%" VisibleIndex="8" Caption="Stk Details">
                                                        <CustomButtons>
                                                            <dxe:GridViewCommandColumnCustomButton Text=" " ID="CustomWarehouse" Image-Url="/assests/images/warehouse.png">
                                                            </dxe:GridViewCommandColumnCustomButton>
                                                        </CustomButtons>
                                                    </dxe:GridViewCommandColumn>


                                                    <dxe:GridViewDataTextColumn FieldName="PurchasePrice" Caption="Purc. Price" VisibleIndex="9" Width="7%" HeaderStyle-HorizontalAlign="Right">
                                                        <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right"></PropertiesTextEdit>
                                                        <PropertiesTextEdit>
                                                            <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                                            <%-- <MaskSettings Mask="&lt;0..999999999g&gt;.&lt;00..99&gt;" AllowMouseWheel="false"/>--%>
                                                            <ClientSideEvents LostFocus="PurchasePriceTextChange" GotFocus="PurPriceGotFocus" />
                                                            <%--LostFocus="QuantityTextChange"--%>
                                                        </PropertiesTextEdit>
                                                        <CellStyle HorizontalAlign="Right"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="Discount" Caption="Disc(%)" VisibleIndex="10" Width="4%" HeaderStyle-HorizontalAlign="Right">
                                                        <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right"></PropertiesTextEdit>
                                                        <PropertiesTextEdit>
                                                            <MaskSettings Mask="&lt;0..99999999999&gt;.&lt;00..999&gt;" AllowMouseWheel="false" />
                                                            <ClientSideEvents LostFocus="DiscountTextChange" GotFocus="DiscountGotChange" />
                                                            <%--LostFocus="DiscountTextChange" GotFocus="DiscountGotChange"--%>
                                                        </PropertiesTextEdit>
                                                        <CellStyle HorizontalAlign="Right"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>

                                                    <%-- Discount in Amt Section Start By Sam on 17052017--%>

                                                    <dxe:GridViewDataTextColumn FieldName="Discountamt" Caption="Disc Amt" VisibleIndex="11" Width="6%" HeaderStyle-HorizontalAlign="Right">
                                                        <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right"></PropertiesTextEdit>
                                                        <PropertiesTextEdit>
                                                            <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..999&gt;" AllowMouseWheel="false" />
                                                            <ClientSideEvents LostFocus="DiscountAmtTextChange" GotFocus="DiscountAmtGotChange" />
                                                            <%--LostFocus="DiscountAmtTextChange"--%>
                                                        </PropertiesTextEdit>
                                                        <CellStyle HorizontalAlign="Right"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>
                                                    <%-- Discount in Amt Section Start By Sam on 17052017--%>

                                                    <dxe:GridViewDataTextColumn FieldName="Amount" Caption="Amount" VisibleIndex="12" Width="8%" HeaderStyle-HorizontalAlign="Right">
                                                        <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right"></PropertiesTextEdit>
                                                        <PropertiesTextEdit>
                                                            <MaskSettings Mask="&lt;0..9999999999999999999999&gt;.&lt;00..999&gt;" AllowMouseWheel="false" />
                                                            <%-- <MaskSettings Mask="&lt;0..9999999999999999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />--%>
                                                            <ClientSideEvents LostFocus="AmtTextChange" GotFocus="AmtGotFocus" />
                                                            <%-- LostFocus="AmtTextChange"--%>
                                                        </PropertiesTextEdit>
                                                        <CellStyle HorizontalAlign="Right"></CellStyle>

                                                    </dxe:GridViewDataTextColumn>
                                                    <%-- <dxe:GridViewDataTextColumn FieldName="TaxAmount" Caption="Tax Amount" VisibleIndex="12" Width="6%">
                                                        <PropertiesTextEdit>
                                                              <MaskSettings Mask="<0..999999999>.<0..9999>" AllowMouseWheel="false" />
                                                        </PropertiesTextEdit>
                                                    </dxe:GridViewDataTextColumn>--%>

                                                    <dxe:GridViewDataButtonEditColumn FieldName="TaxAmount" Caption="Charges" VisibleIndex="13" Width="6%" HeaderStyle-HorizontalAlign="Right" ReadOnly="True">

                                                        <PropertiesButtonEdit>
                                                            <ClientSideEvents ButtonClick="taxAmtButnClick" GotFocus="taxAmtButnClick1" KeyDown="TaxAmountKeyDown" />
                                                            <Buttons>
                                                                <dxe:EditButton Text="..." Width="20px">
                                                                </dxe:EditButton>
                                                            </Buttons>
                                                            <%--<MaskSettings Mask="<0..999999999999g>.<0..99g>" />--%>
                                                        </PropertiesButtonEdit>
                                                        <CellStyle HorizontalAlign="Right"></CellStyle>
                                                    </dxe:GridViewDataButtonEditColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="TotalAmount" Caption="Net Amount" VisibleIndex="14" Width="6.5%" HeaderStyle-HorizontalAlign="Right">
                                                        <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right"></PropertiesTextEdit>
                                                        <PropertiesTextEdit>
                                                            <MaskSettings Mask="&lt;0..9999999999999999999999&gt;.&lt;00..999&gt;" AllowMouseWheel="false" />
                                                            <%--<MaskSettings Mask="&lt;0..9999999999999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />--%>
                                                        </PropertiesTextEdit>
                                                        <CellStyle HorizontalAlign="Right"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>


                                                    <dxe:GridViewDataTextColumn FieldName="Remarks" Caption="Remarks" VisibleIndex="15" Width="6%" ReadOnly="false">
                                                        <PropertiesTextEdit Style-HorizontalAlign="Left">

                                                            <Style HorizontalAlign="Left">
                                                            </Style>
                                                        </PropertiesTextEdit>
                                                    </dxe:GridViewDataTextColumn>


                                                    <dxe:GridViewCommandColumn ShowDeleteButton="false" ShowNewButtonInHeader="false" Width="5%" VisibleIndex="16" Caption="Add New">
                                                        <CustomButtons>
                                                            <dxe:GridViewCommandColumnCustomButton Text=" " ID="CustomAddNewRow" Image-Url="/assests/images/add.png">
                                                            </dxe:GridViewCommandColumnCustomButton>

                                                        </CustomButtons>

                                                    </dxe:GridViewCommandColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="ComponentID" Caption="Component ID" VisibleIndex="17" ReadOnly="True" Width="0">
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="ComponentDetailID" Caption="ComponentDetailID" VisibleIndex="21" ReadOnly="True" Width="0">
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="TotalQty" Caption="Total Qty" VisibleIndex="18" ReadOnly="True" Width="0">
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="BalanceQty" Caption="Balance Qty" VisibleIndex="19" ReadOnly="True" Width="0">
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="IsComponentProduct" Caption="IsComponentProduct" VisibleIndex="20" ReadOnly="True" Width="0">
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="ProductID" PropertiesTextEdit-ValidationSettings-ErrorImage-IconID="ghg" Caption="hidden Field Id" VisibleIndex="22" ReadOnly="True" Width="0" PropertiesTextEdit-Height="15px" PropertiesTextEdit-Style-CssClass="abcd">
                                                        <CellStyle Wrap="True" CssClass="abcd"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>

                                                </Columns>
                                                <%--      Init="OnInit"BatchEditStartEditing="OnBatchEditStartEditing" BatchEditEndEditing="OnBatchEditEndEditing"
                                                    CustomButtonClick="OnCustomButtonClick" EndCallback="OnEndCallback" --%>
                                                <ClientSideEvents EndCallback="OnEndCallback" CustomButtonClick="OnCustomButtonClick" RowClick="GetVisibleIndex"
                                                    BatchEditStartEditing="gridFocusedRowChanged" />
                                                <SettingsDataSecurity AllowEdit="true" />
                                                <SettingsEditing Mode="Batch" NewItemRowPosition="Bottom">
                                                    <BatchEditSettings ShowConfirmOnLosingChanges="false" EditMode="row" />
                                                </SettingsEditing>
                                            </dxe:ASPxGridView>
                                        </div>
                                    </div>
                                    <div style="clear: both;">
                                        <br />
                                        <div style="display: none;">
                                            <dxe:ASPxLabel ID="txt_Charges" runat="server" Text="0.00" ClientInstanceName="ctxt_Charges" />
                                            <dxe:ASPxLabel ID="txt_cInvValue" runat="server" Text="0.00" ClientInstanceName="cInvValue" />
                                        </div>
                                    </div>
                                    <%-- Sam New Section Added--%>
                                    <div class="content reverse horizontal-images clearfix" style="width: 100%; margin-right: 0; padding: 8px; height: auto; border-top: 1px solid #ccc; border-bottom: 1px solid #ccc; border-radius: 0;">
                                        <ul>
                                            <li class="clsbnrLblTaxableAmt">
                                                <div class="horizontallblHolder">
                                                    <table>
                                                        <tbody>
                                                            <tr>
                                                                <td>
                                                                    <dxe:ASPxLabel ID="bnrLblTotalQty" runat="server" Text="Total Quantity" ClientInstanceName="cbnrLblTotalQty" />
                                                                </td>
                                                                <td>
                                                                    <dxe:ASPxLabel ID="txt_TotalQty" runat="server" Text="0.00" ClientInstanceName="cTotalQty" />
                                                                </td>
                                                            </tr>
                                                        </tbody>
                                                    </table>
                                                </div>
                                            </li>
                                            <li class="clsbnrLblTaxableAmt">
                                                <div class="horizontallblHolder">
                                                    <table>
                                                        <tbody>
                                                            <tr>
                                                                <td>
                                                                    <dxe:ASPxLabel ID="bnrLblTaxableAmt" runat="server" Text="Taxable Amount" ClientInstanceName="cbnrLblTaxableAmt" />
                                                                </td>
                                                                <td>
                                                                    <dxe:ASPxLabel ID="txt_TaxableAmtval" runat="server" Text="0.00" ClientInstanceName="cTaxableAmtval" />
                                                                </td>
                                                            </tr>
                                                        </tbody>
                                                    </table>
                                                </div>
                                            </li>
                                            <li class="clsbnrLblTaxAmt">
                                                <div class="horizontallblHolder">
                                                    <table>
                                                        <tbody>
                                                            <tr>
                                                                <td>
                                                                    <dxe:ASPxLabel ID="bnrLblTaxAmt" runat="server" Text="Tax & Charges" ClientInstanceName="cbnrLblTaxAmt" />
                                                                </td>
                                                                <td>
                                                                    <dxe:ASPxLabel ID="txt_TaxAmtval" runat="server" Text="0.00" ClientInstanceName="cTaxAmtval" />
                                                                </td>
                                                            </tr>
                                                        </tbody>
                                                    </table>
                                                </div>
                                            </li>
                                            <li class="clsbnrLblTaxAmt">
                                                <div class="horizontallblHolder">
                                                    <table>
                                                        <tbody>
                                                            <tr>
                                                                <td>
                                                                    <dxe:ASPxLabel ID="bnrLblOtherTaxAmt" runat="server" Text="Other Charges" ClientInstanceName="cbnrLblOtherTaxAmt" />
                                                                </td>
                                                                <td>
                                                                    <dxe:ASPxLabel ID="txt_OtherTaxAmtval" runat="server" Text="0.00" ClientInstanceName="cOtherTaxAmtval" />
                                                                </td>
                                                            </tr>
                                                        </tbody>
                                                    </table>
                                                </div>
                                            </li>
                                            <li class="clsbnrLblInvVal">
                                                <div class="horizontallblHolder">
                                                    <table>
                                                        <tbody>
                                                            <tr>
                                                                <td>
                                                                    <dxe:ASPxLabel ID="bnrLblInvVal" runat="server" Text="Total Amount" ClientInstanceName="cbnrLblInvVal" />
                                                                </td>
                                                                <td>
                                                                    <dxe:ASPxLabel ID="txt_TotalAmt" runat="server" Text="0.00" ClientInstanceName="cTotalAmt" />
                                                                </td>
                                                            </tr>
                                                        </tbody>
                                                    </table>
                                                </div>
                                            </li>
                                        </ul>
                                    </div>

                                    <%-- Sam New Section Above Added--%>
                                    <%-- id="divSubmitButton" runat="server"--%>
                                    <div class="col-md-12 padTop10">
                                        <asp:Label ID="lbl_quotestatusmsg" runat="server" Text="" Font-Bold="true" ForeColor="Red" Font-Size="Medium"></asp:Label>
                                        <dxe:ASPxButton ID="btn_SaveRecords" ClientInstanceName="cbtn_SaveNewRecords" AutoPostBack="False" runat="server" Text="S&#818;ave & New" CssClass="btn btn-primary" UseSubmitBehavior="False">
                                            <ClientSideEvents Click="function(s, e) {Save_ButtonClick();}" />
                                        </dxe:ASPxButton>
                                        <dxe:ASPxButton ID="btn_SaveExitRecords" ClientInstanceName="cbtn_SaveRecords" AutoPostBack="False" runat="server" Text="Save & E&#818;xit" CssClass="btn btn-primary" UseSubmitBehavior="False">
                                            <ClientSideEvents Click="function(s, e) {SaveExit_ButtonClick();}" />
                                        </dxe:ASPxButton>
                                        <dxe:ASPxButton ID="btnSaveUdf" ClientInstanceName="cbtn_SaveUdf" runat="server" AutoPostBack="False" Text="U&#818;DF" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                                            <ClientSideEvents Click="function(s, e) {OpenUdf();}" />
                                        </dxe:ASPxButton>
                                        <dxe:ASPxButton ID="ASPxButton3" ClientInstanceName="cbtn_SaveTaxesRecords" runat="server" AccessKey="X" AutoPostBack="False" Text="T&#818;axes" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                                            <ClientSideEvents Click="function(s, e) {Save_TaxesClick();}" />
                                        </dxe:ASPxButton>

                                        <dxe:ASPxButton ID="ASPxButton2" ClientInstanceName="cbtn_specialedit" runat="server" AutoPostBack="False" Text="Special Edit" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False" Visible="false">
                                            <ClientSideEvents Click="function(s, e) {specialedit_ButtonClick();}" />
                                        </dxe:ASPxButton>
                                        <asp:HiddenField ID="hfControlData" runat="server" />
                                        <asp:HiddenField ID="hdnPBTaggedYorN" runat="server" />
                                        <asp:HiddenField ID="hdnTaxDeleteByShippingStateMismatch" runat="server"></asp:HiddenField>
                                        <asp:HiddenField ID="hdnRCMChecked" runat="server" />
                                        <span id="spVehTC"></span>
                                        <span id="spVendorImport"></span>

                                        <asp:HiddenField runat="server" ID="hfTermsConditionData" />
                                        <asp:HiddenField runat="server" ID="hfTermsConditionDocType" Value="PB" />
                                        <asp:HiddenField runat="server" ID="hdnVendorImport" Value="PB" />
                                        <asp:HiddenField runat="server" ID="hdnPBAutoPrint" Value="" />
                                        <asp:HiddenField ID="hdnTaggedVender" runat="server" Value="" />
                                        <asp:HiddenField ID="hdnTaggedVendorName" runat="server" Value="" />
                                        <asp:HiddenField ID="hdnTDSShoworNot" runat="server" Value="N" />
                                        <asp:CheckBox ID="chkmail" runat="server" Text="Send Mail" />
                                    </div>
                                </div>
                            </dxe:ContentControl>
                        </ContentCollection>
                    </dxe:TabPage>
                    <dxe:TabPage Name="Billing/Shipping" Text="Our Billing/Shipping">
                        <ContentCollection>
                            <dxe:ContentControl runat="server">
                                <ucBS:BillingShippingControl runat="server" ID="BillingShippingControl" />
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
        <%--InlineTax--%>
        <dxe:ASPxPopupControl ID="aspxTaxpopUp" runat="server" ClientInstanceName="caspxTaxpopUp"
            Width="850px" HeaderText="Select Tax" PopupHorizontalAlign="WindowCenter"
            PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
            Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True">
            <HeaderTemplate>
                <span style="color: #fff"><strong>Select Tax</strong></span>
                <dxe:ASPxImage ID="ASPxImage3" runat="server" ImageUrl="/assests/images/closePop.png" Cursor="pointer" CssClass="popUpHeader pull-right">
                    <ClientSideEvents Click="function(s, e){ 
                                                            cgridTax.CancelEdit();
                                                            caspxTaxpopUp.Hide();
                                                        }" />
                </dxe:ASPxImage>
            </HeaderTemplate>
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">
                    <asp:HiddenField runat="server" ID="setCurrentProdCode" />
                    <asp:HiddenField runat="server" ID="HdSerialNo" />
                    <asp:HiddenField runat="server" ID="hdnInvWiseSlno" />
                    <asp:HiddenField runat="server" ID="HdProdGrossAmt" />
                    <asp:HiddenField runat="server" ID="HdProdNetAmt" />
                    <asp:HiddenField ID="hdnDeleteSrlNo" runat="server" />
                    <asp:HiddenField ID="hdnADDEditMode" runat="server" />
                    <div id="content-6">
                        <div class="col-sm-3">
                            <div class="lblHolder">
                                <table>
                                    <tbody>
                                        <tr>
                                            <td>Gross Amount
                                                    <dxe:ASPxLabel ID="ASPxLabel5" runat="server" Text="(Taxable)" ClientInstanceName="clblTaxableGross"></dxe:ASPxLabel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <dxe:ASPxLabel ID="lblTaxProdGrossAmt" runat="server" Text="" ClientInstanceName="clblTaxProdGrossAmt"></dxe:ASPxLabel>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </div>

                        <div class="col-sm-3 gstGrossAmount hide">
                            <div class="lblHolder">
                                <table>
                                    <tbody>
                                        <tr>
                                            <td>GST</td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <dxe:ASPxLabel ID="lblGstForGross" runat="server" Text="" ClientInstanceName="clblGstForGross"></dxe:ASPxLabel>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </div>

                        <div class="col-sm-3">
                            <div class="lblHolder">
                                <table>
                                    <tbody>
                                        <tr>
                                            <td>Discount</td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <dxe:ASPxLabel ID="lblTaxDiscount" runat="server" Text="" ClientInstanceName="clblTaxDiscount"></dxe:ASPxLabel>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </div>


                        <div class="col-sm-3">
                            <div class="lblHolder">
                                <table>
                                    <tbody>
                                        <tr>
                                            <td>Amount
                                                    <dxe:ASPxLabel ID="ASPxLabel6" runat="server" Text="(Taxable)" ClientInstanceName="clblTaxableNet"></dxe:ASPxLabel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <dxe:ASPxLabel ID="lblProdNetAmt" runat="server" Text="" ClientInstanceName="clblProdNetAmt"></dxe:ASPxLabel>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </div>

                        <div class="col-sm-2 gstNetAmount hide">
                            <div class="lblHolder">
                                <table>
                                    <tbody>
                                        <tr>
                                            <td>GST</td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <dxe:ASPxLabel ID="lblGstForNet" runat="server" Text="" ClientInstanceName="clblGstForNet"></dxe:ASPxLabel>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </div>

                    </div>

                    <%--Error Message--%>
                    <div id="ContentErrorMsg">
                        <div class="col-sm-8">
                            <div class="lblHolder">
                                <table>
                                    <tbody>
                                        <tr>
                                            <td>Status
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Tax Code/Charges Not defined.
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </div>
                    </div>
                    <table style="width: 100%;">
                        <tr>
                            <td colspan="2"></td>
                        </tr>
                        <tr>
                            <td colspan="2"></td>
                        </tr>
                        <tr style="display: none">
                            <td><span><strong>Product Basic Amount</strong></span></td>
                            <td>
                                <dxe:ASPxTextBox ID="txtprodBasicAmt" MaxLength="80" ClientInstanceName="ctxtprodBasicAmt" ReadOnly="true"
                                    runat="server" Width="50%">
                                    <MaskSettings Mask="<0..999999999>.<0..99>" AllowMouseWheel="false" />
                                </dxe:ASPxTextBox>
                            </td>
                        </tr>
                        <tr class="cgridTaxClass">
                            <td colspan="3">
                                <dxe:ASPxGridView runat="server" OnBatchUpdate="taxgrid_BatchUpdate" KeyFieldName="Taxes_ID" ClientInstanceName="cgridTax" ID="aspxGridTax"
                                    Width="100%" SettingsBehavior-AllowSort="false" SettingsBehavior-AllowDragDrop="false" SettingsPager-Mode="ShowAllRecords" OnCustomCallback="cgridTax_CustomCallback"
                                    Settings-ShowFooter="false" AutoGenerateColumns="False" OnCellEditorInitialize="aspxGridTax_CellEditorInitialize" OnHtmlRowCreated="aspxGridTax_HtmlRowCreated"
                                    OnRowInserting="taxgrid_RowInserting" OnRowUpdating="taxgrid_RowUpdating" OnRowDeleting="taxgrid_RowDeleting">
                                    <Settings VerticalScrollableHeight="150" VerticalScrollBarMode="Auto"></Settings>
                                    <SettingsBehavior AllowDragDrop="False" AllowSort="False"></SettingsBehavior>
                                    <SettingsPager Visible="false"></SettingsPager>
                                    <Columns>
                                        <dxe:GridViewDataTextColumn VisibleIndex="1" FieldName="Taxes_Name" ReadOnly="true" Caption="Tax Component ID">
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="taxCodeName" ReadOnly="true" Caption="Tax Component Name">
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn VisibleIndex="3" FieldName="calCulatedOn" ReadOnly="true" Caption="Calculated On">
                                            <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right">
                                                <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                            </PropertiesTextEdit>
                                            <CellStyle Wrap="False" HorizontalAlign="Right"></CellStyle>
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="Percentage" FieldName="TaxField" VisibleIndex="4">
                                            <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right">
                                                <ClientSideEvents LostFocus="txtPercentageLostFocus" GotFocus="CmbtaxClick" />
                                                <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                            </PropertiesTextEdit>
                                            <CellStyle Wrap="False" HorizontalAlign="Right"></CellStyle>
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn VisibleIndex="5" FieldName="Amount" Caption="Amount" ReadOnly="true">
                                            <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right">
                                                <ClientSideEvents LostFocus="taxAmountLostFocus" GotFocus="taxAmountGotFocus" />
                                                <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                            </PropertiesTextEdit>
                                            <CellStyle Wrap="False" HorizontalAlign="Right"></CellStyle>
                                        </dxe:GridViewDataTextColumn>
                                    </Columns>
                                    <SettingsDataSecurity AllowEdit="true" />
                                    <SettingsEditing Mode="Batch">
                                        <BatchEditSettings EditMode="row" />
                                    </SettingsEditing>
                                    <ClientSideEvents EndCallback=" cgridTax_EndCallBack " RowClick="GetTaxVisibleIndex" />
                                </dxe:ASPxGridView>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <table class="InlineTaxClass hide">
                                    <tr class="GstCstvatClass" style="">
                                        <td style="padding-top: 10px; padding-bottom: 15px; padding-right: 25px"><span><strong>GST</strong></span></td>
                                        <td style="padding-top: 10px; padding-bottom: 15px;">
                                            <dxe:ASPxComboBox ID="cmbGstCstVat" ClientInstanceName="ccmbGstCstVat" runat="server" SelectedIndex="-1"
                                                ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True" TextFormatString="{0}"
                                                ClearButton-DisplayMode="Always" OnCallback="cmbGstCstVat_Callback">
                                                <Columns>
                                                    <dxe:ListBoxColumn FieldName="Taxes_Name" Caption="Tax Component ID" Width="250" />
                                                    <dxe:ListBoxColumn FieldName="TaxCodeName" Caption="Tax Component Name" Width="250" />
                                                </Columns>
                                            </dxe:ASPxComboBox>
                                        </td>
                                        <td style="padding-left: 15px; padding-top: 10px; padding-bottom: 15px; padding-right: 25px">
                                            <dxe:ASPxTextBox ID="txtGstCstVat" MaxLength="80" ClientInstanceName="ctxtGstCstVat" ReadOnly="true" Text="0.00"
                                                runat="server" Width="100%">
                                                <MaskSettings Mask="<-999999999..999999999>.<0..99>" AllowMouseWheel="false" />
                                            </dxe:ASPxTextBox>
                                        </td>
                                        <td>
                                            <input type="button" onclick="recalculateTax()" class="btn btn-info btn-small RecalculateInline" value="Recalculate GST" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <div class="pull-left">

                                    <dxe:ASPxButton ID="Button1" ClientInstanceName="cButton1" runat="server" AutoPostBack="False" Text="OK" CssClass="btn btn-primary mTop" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                                        <ClientSideEvents Click="function(s, e) {return BatchUpdate();}" />
                                    </dxe:ASPxButton>
                                    <dxe:ASPxButton ID="Button3" ClientInstanceName="cButton3" runat="server" AutoPostBack="False" Text="Cancel" CssClass="btn btn-danger mTop" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                                        <ClientSideEvents Click="function(s, e) {cgridTax.CancelEdit(); caspxTaxpopUp.Hide(); return false;}" />
                                    </dxe:ASPxButton>
                                </div>
                                <table class="pull-right">
                                    <tr>
                                        <td style="padding-top: 10px; padding-right: 5px"><strong>Total Charges</strong></td>
                                        <td>
                                            <dxe:ASPxTextBox ID="txtTaxTotAmt" MaxLength="80" ClientInstanceName="ctxtTaxTotAmt" Text="0.00" ReadOnly="true"
                                                runat="server" Width="100%" CssClass="pull-left mTop">
                                                <MaskSettings Mask="&lt;-999999999..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                            </dxe:ASPxTextBox>
                                        </td>
                                    </tr>
                                </table>
                                <div class="clear"></div>
                            </td>
                        </tr>

                    </table>
                </dxe:PopupControlContentControl>
            </ContentCollection>
            <ContentStyle VerticalAlign="Top" CssClass="pad"></ContentStyle>
            <HeaderStyle BackColor="LightGray" ForeColor="Black" />
        </dxe:ASPxPopupControl>
        <dxe:ASPxCallbackPanel runat="server" ID="taxUpdatePanel" ClientInstanceName="ctaxUpdatePanel" OnCallback="taxUpdatePanel_Callback">
            <PanelCollection>
                <dxe:PanelContent runat="server">
                </dxe:PanelContent>
            </PanelCollection>
            <ClientSideEvents EndCallback="ctaxUpdatePanelEndCall" />
        </dxe:ASPxCallbackPanel>
        <asp:HiddenField ID="HdChargeProdAmt" runat="server" />
        <asp:HiddenField ID="HdChargeProdNetAmt" runat="server" />
        <%--ChargesTax--%>
        <dxe:ASPxPopupControl ID="Popup_Taxes" runat="server" ClientInstanceName="cPopup_Taxes"
            Width="900px" Height="300px" HeaderText="Purchase Invoice Taxes" PopupHorizontalAlign="WindowCenter"
            BackColor="white" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
            Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
            ContentStyle-CssClass="pad">
            <ContentStyle VerticalAlign="Top" CssClass="pad">
            </ContentStyle>
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">
                    <div class="Top clearfix">
                        <div id="content-5" class="col-md-12  wrapHolder content horizontal-images" style="width: 100%; margin-right: 0;">
                            <ul>
                                <li>
                                    <div class="lblHolder">
                                        <table>
                                            <tbody>
                                                <tr>
                                                    <td>Gross Amount Total
                                                                <dxe:ASPxLabel ID="ASPxLabel7" runat="server" Text="ASPxLabel" ClientInstanceName="clblChargesTaxableGross"></dxe:ASPxLabel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <dxe:ASPxLabel ID="txtProductAmount" runat="server" Text="ASPxLabel" ClientInstanceName="ctxtProductAmount">
                                                        </dxe:ASPxLabel>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </div>
                                </li>
                                <li class="lblChargesGSTforGross">
                                    <div class="lblHolder">
                                        <table>
                                            <tbody>
                                                <tr>
                                                    <td>GST</td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <dxe:ASPxLabel ID="lblChargesGSTforGross" runat="server" Text="ASPxLabel" ClientInstanceName="clblChargesGSTforGross">
                                                        </dxe:ASPxLabel>
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
                                                    <td>Total Discount</td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <dxe:ASPxLabel ID="txtProductDiscount" runat="server" Text="ASPxLabel" ClientInstanceName="ctxtProductDiscount">
                                                        </dxe:ASPxLabel>
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
                                                    <td>Total Charges</td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <dxe:ASPxLabel ID="txtProductTaxAmount" runat="server" Text="ASPxLabel" ClientInstanceName="ctxtProductTaxAmount"></dxe:ASPxLabel>
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
                                                    <td>Net Amount Total
                                                                <dxe:ASPxLabel ID="ASPxLabel8" runat="server" Text="ASPxLabel" ClientInstanceName="clblChargesTaxableNet"></dxe:ASPxLabel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <dxe:ASPxLabel ID="txtProductNetAmount" runat="server" Text="ASPxLabel" ClientInstanceName="ctxtProductNetAmount"></dxe:ASPxLabel>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </div>
                                </li>
                                <li class="lblChargesGSTforNet">
                                    <div class="lblHolder">
                                        <table>
                                            <tbody>
                                                <tr>
                                                    <td>GST</td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <dxe:ASPxLabel ID="lblChargesGSTforNet" runat="server" Text="ASPxLabel" ClientInstanceName="clblChargesGSTforNet">
                                                        </dxe:ASPxLabel>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </div>
                                </li>
                            </ul>
                        </div>
                        <div class="clear">
                        </div>
                        <%--Error Msg--%>

                        <div class="col-md-8" id="ErrorMsgCharges">
                            <div class="lblHolder">
                                <table>
                                    <tbody>
                                        <tr>
                                            <td>Status
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Tax Code/Charges Not Defined.
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>

                        </div>

                        <div class="clear">
                        </div>
                        <div class="col-md-12 gridTaxClass" style="">
                            <dxe:ASPxGridView runat="server" KeyFieldName="TaxID" ClientInstanceName="gridTax" ID="gridTax"
                                Width="100%" SettingsBehavior-AllowSort="false" SettingsBehavior-AllowDragDrop="false"
                                Settings-ShowFooter="false" OnCustomCallback="gridTax_CustomCallback" OnBatchUpdate="gridTax_BatchUpdate"
                                OnRowInserting="gridTax_RowInserting" OnRowUpdating="gridTax_RowUpdating" OnRowDeleting="gridTax_RowDeleting"
                                OnDataBinding="gridTax_DataBinding">
                                <Settings VerticalScrollableHeight="150" VerticalScrollBarMode="Auto"></Settings>
                                <SettingsPager Visible="false"></SettingsPager>
                                <Columns>
                                    <dxe:GridViewDataTextColumn FieldName="TaxName" Caption="Tax" VisibleIndex="0" Width="40%" ReadOnly="true">
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn FieldName="calCulatedOn" Caption="Calculated On" VisibleIndex="0" Width="20%" ReadOnly="true">
                                        <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right">
                                            <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                        </PropertiesTextEdit>
                                        <CellStyle Wrap="False" HorizontalAlign="Right"></CellStyle>
                                    </dxe:GridViewDataTextColumn>

                                    <dxe:GridViewDataTextColumn FieldName="Percentage" Caption="Percentage" VisibleIndex="1" Width="20%">
                                        <PropertiesTextEdit Style-HorizontalAlign="Right" DisplayFormatString="0.00">
                                            <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                            <ClientSideEvents LostFocus="PercentageTextChange" />
                                            <ClientSideEvents />
                                        </PropertiesTextEdit>
                                        <CellStyle Wrap="False" HorizontalAlign="Right"></CellStyle>
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn FieldName="Amount" Caption="Amount" VisibleIndex="2" Width="20%">
                                        <PropertiesTextEdit Style-HorizontalAlign="Right" DisplayFormatString="0.00">
                                            <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                            <ClientSideEvents LostFocus="QuotationTaxAmountTextChange" GotFocus="QuotationTaxAmountGotFocus" />
                                        </PropertiesTextEdit>
                                        <CellStyle Wrap="False" HorizontalAlign="Right"></CellStyle>
                                    </dxe:GridViewDataTextColumn>
                                </Columns>
                                <ClientSideEvents EndCallback="OnTaxEndCallback" />
                                <SettingsDataSecurity AllowEdit="true" />
                                <SettingsEditing Mode="Batch" NewItemRowPosition="Bottom">
                                    <BatchEditSettings ShowConfirmOnLosingChanges="false" EditMode="row" />
                                </SettingsEditing>
                                <Styles>
                                    <StatusBar CssClass="statusBar">
                                    </StatusBar>
                                </Styles>
                            </dxe:ASPxGridView>
                        </div>
                        <div class="col-md-12">
                            <table style="" class="chargesDDownTaxClass">
                                <tr class="chargeGstCstvatClass">
                                    <td style="padding-top: 10px; padding-right: 25px"><span><strong>GST</strong></span></td>
                                    <td style="padding-top: 10px; width: 200px;">
                                        <dxe:ASPxComboBox ID="cmbGstCstVatcharge" ClientInstanceName="ccmbGstCstVatcharge" runat="server" SelectedIndex="0"
                                            ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True" TextFormatString="{0}"
                                            OnCallback="cmbGstCstVatcharge_Callback">
                                            <Columns>
                                                <dxe:ListBoxColumn FieldName="Taxes_Name" Caption="Tax Component ID" Width="250" />
                                                <dxe:ListBoxColumn FieldName="TaxCodeName" Caption="Tax Component Name" Width="250" />
                                            </Columns>
                                        </dxe:ASPxComboBox>
                                    </td>
                                    <td style="padding-left: 15px; padding-top: 10px; width: 200px;">
                                        <dxe:ASPxTextBox ID="txtGstCstVatCharge" MaxLength="80" ClientInstanceName="ctxtGstCstVatCharge" ReadOnly="true" Text="0.00"
                                            runat="server" Width="100%">
                                            <MaskSettings Mask="&lt;-999999999..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                        </dxe:ASPxTextBox>
                                    </td>
                                    <td style="padding-left: 15px; padding-top: 10px">
                                        <input type="button" onclick="recalculateTaxCharge()" class="btn btn-info btn-small RecalculateCharge" value="Recalculate GST" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div class="clear">
                            <br />
                        </div>
                        <div class="col-sm-3">
                            <div>
                                <dxe:ASPxButton ID="btn_SaveTax" ClientInstanceName="cbtn_SaveTax" runat="server" AccessKey="X" AutoPostBack="False" Text="Save" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                                    <ClientSideEvents Click="function(s, e) {Save_TaxClick();}" />
                                </dxe:ASPxButton>
                                <dxe:ASPxButton ID="ASPxButton4" ClientInstanceName="cbtn_tax_cancel" runat="server" AutoPostBack="False" Text="Cancel" CssClass="btn btn-danger" UseSubmitBehavior="False">
                                    <ClientSideEvents Click="function(s, e) {cPopup_Taxes.Hide();}" />
                                </dxe:ASPxButton>
                            </div>
                        </div>
                        <div class="col-sm-9">
                            <table class="pull-right">
                                <tr>
                                    <td style="padding-right: 30px"><strong>Total Charges</strong></td>
                                    <td>
                                        <div>
                                            <dxe:ASPxTextBox ID="txtQuoteTaxTotalAmt" runat="server" Width="100%" ClientInstanceName="ctxtQuoteTaxTotalAmt" Text="0.00" HorizontalAlign="Right" Font-Size="12px" ReadOnly="true">
                                                <MaskSettings Mask="&lt;-999999999..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                            </dxe:ASPxTextBox>
                                        </div>
                                    </td>
                                    <td style="padding-right: 30px; padding-left: 5px"><strong>Total Amount</strong></td>
                                    <td>
                                        <div>
                                            <dxe:ASPxTextBox ID="txtTotalAmount" runat="server" Width="100%" ClientInstanceName="ctxtTotalAmount" Text="0.00" HorizontalAlign="Right" Font-Size="12px" ReadOnly="true">
                                                <MaskSettings Mask="&lt;-999999999..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                            </dxe:ASPxTextBox>
                                        </div>
                                    </td>

                                </tr>
                            </table>
                        </div>
                        <div class="col-sm-2" style="padding-top: 8px;">
                            <span></span>
                        </div>
                        <div class="col-sm-4">
                        </div>
                        <div class="col-sm-2" style="padding-top: 8px;">
                            <span></span>
                        </div>
                        <div class="col-sm-4">
                        </div>
                    </div>
                </dxe:PopupControlContentControl>
            </ContentCollection>
            <HeaderStyle BackColor="LightGray" ForeColor="Black" />
        </dxe:ASPxPopupControl>
        <%--   Inline Tax End    --%>

        <%--   Warehouse     --%>
        <dxe:ASPxPopupControl ID="ASPxPopupControl3" runat="server" ClientInstanceName="cPopup_WarehousePC"
            Width="900px" HeaderText="Stock Details" PopupHorizontalAlign="WindowCenter"
            BackColor="white" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
            Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
            ContentStyle-CssClass="pad">
            <ContentStyle VerticalAlign="Top" CssClass="pad">
            </ContentStyle>
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">
                    <div class="Top clearfix">
                        <div class="row">
                            <div class="col-md-3">
                                <div class="lblHolder">
                                    <table>
                                        <tr>
                                            <td>Selected Branch</td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblbranchName" runat="server"></asp:Label></td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                            <div class="col-md-3">
                                <div class="lblHolder">
                                    <table>
                                        <tr>
                                            <td>Selected Product</td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblpro" runat="server"></asp:Label></td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                            <div class="col-md-3">
                                <div class="lblHolder">
                                    <table>
                                        <tr>
                                            <td>Available Stock</td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblAvailableStk" runat="server" Text="0.0"></asp:Label></td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                            <div class="col-md-3">
                                <div class="lblHolder">
                                    <table>
                                        <tr>
                                            <td>Entered Stock</td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblopeningstock" runat="server" Text="0.0000"></asp:Label></td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                        </div>


                        <div class="clear">
                            <br />
                        </div>
                        <div class="clearfix">
                            <div class="row manAb">
                                <div class="blockone">
                                    <div class="col-md-3">
                                        <div>
                                            <span id="RequiredFieldValidatorCmbWarehousetxt">Warehouse</span>
                                        </div>
                                        <div class="Left_Content relative" style="">
                                            <%-- <dxe:ASPxTextBox ID="txtwarehousname" runat="server" Width="80%" ClientInstanceName="ctxtwarehousname" HorizontalAlign="Left" Font-Size="12px">
                                    </dxe:ASPxTextBox>--%>
                                            <dxe:ASPxComboBox ID="CmbWarehouse" EnableIncrementalFiltering="True" ClientInstanceName="cCmbWarehouse" SelectedIndex="0"
                                                TextField="WarehouseName" ValueField="WarehouseID" runat="server" Width="100%" OnCallback="CmbWarehouse_Callback">
                                                <ClientSideEvents ValueChanged="function(s,e){CmbWarehouse_ValueChange(s)}" EndCallback="function(s,e){endcallcmware(s)}"></ClientSideEvents>

                                            </dxe:ASPxComboBox>
                                            <span id="RequiredFieldValidatorCmbWarehouse" title="Mandatory" class="tp2 fa fa-exclamation-circle iconRed" style="display: none;"></span>
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <div>
                                            <span id="RequiredFieldValidatorCmbWarehouseQuantity">Quantity</span>
                                        </div>
                                        <div class="Left_Content" style="">
                                            <dxe:ASPxTextBox ID="txtqnty" runat="server" Width="100%" ClientInstanceName="ctxtqnty" HorizontalAlign="Left" Font-Size="12px">
                                                <MaskSettings Mask="<0..999999999>.<0..9999>" IncludeLiterals="DecimalSymbol" />
                                                <ClientSideEvents TextChanged="function(s,e){changedqnty(s)}" LostFocus="function(s,e){Setenterfocuse(s)}" KeyPress="function(s, e) {Keypressevt();}" />
                                            </dxe:ASPxTextBox>
                                            <span id="RequiredFieldValidatortxtwareqntity" title="Mandatory" class="tp2 fa fa-exclamation-circle iconRed" style="display: none;"></span>
                                        </div>
                                    </div>
                                </div>
                                <div class="clear"></div>
                                <div class="blocktwo">
                                    <div class="col-md-3">
                                        <div>
                                            <span id="RequiredFieldValidatortxtbatchtxt">Batch</span>
                                        </div>
                                        <div class="Left_Content relative" style="">
                                            <dxe:ASPxTextBox ID="txtbatch" runat="server" Width="100%" ClientInstanceName="ctxtbatch" HorizontalAlign="Left" Font-Size="12px" MaxLength="49">
                                                <ClientSideEvents TextChanged="function(s,e){chnagedbtach(s)}" KeyPress="function(s, e) {Keypressevt();}" />
                                            </dxe:ASPxTextBox>
                                            <span id="RequiredFieldValidatortxtbatch" title="Mandatory" class="tp2 fa fa-exclamation-circle iconRed" style="display: none;"></span>
                                        </div>
                                    </div>
                                    <div class="col-md-3 blocktwoqntity">
                                        <div>
                                            <span id="RequiredFieldValidatorbatchQuantity">Quantity</span>
                                        </div>
                                        <div class="Left_Content" style="">
                                            <dxe:ASPxTextBox ID="batchqnty" runat="server" Width="100%" ClientInstanceName="ctxtbatchqnty" HorizontalAlign="Left" Font-Size="12px">
                                                <MaskSettings Mask="<0..999999999>.<0..9999>" IncludeLiterals="DecimalSymbol" />
                                                <ClientSideEvents TextChanged="function(s,e){changedqntybatch(s)}" />
                                            </dxe:ASPxTextBox>
                                            <span id="RequiredFieldValidatortxtbatchqntity" title="Mandatory" class="tp2 fa fa-exclamation-circle iconRed" style="display: none;"></span>
                                        </div>
                                    </div>

                                    <div class="col-md-3">
                                        <div>
                                            <span id="RequiredFieldValidatortxtbatchtxtmkgdate">Manufacture Date</span>
                                        </div>
                                        <div class="Left_Content" style="">
                                            <%--<dxe:ASPxTextBox ID="txtmkgdate" runat="server" Width="80%" ClientInstanceName="ctxtmkgdate" HorizontalAlign="Left" Font-Size="12px">
                                    </dxe:ASPxTextBox>--%>
                                            <dxe:ASPxDateEdit ID="txtmkgdate" runat="server" Width="100%" EditFormat="custom" UseMaskBehavior="True" ClientInstanceName="ctxtmkgdate" AllowNull="true" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy">
                                                <ButtonStyle Width="13px">
                                                </ButtonStyle>

                                            </dxe:ASPxDateEdit>
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <div>
                                            <span id="RequiredFieldValidatortxtbatchtxtexpdate">Expiry Date</span>
                                        </div>
                                        <div class="Left_Content" style="">
                                            <%-- <dxe:ASPxTextBox ID="txtexpirdate" runat="server" Width="80%" ClientInstanceName="ctxtexpirdate" HorizontalAlign="Left" Font-Size="12px">
                                    </dxe:ASPxTextBox>--%>
                                            <dxe:ASPxDateEdit ID="txtexpirdate" runat="server" Width="100%" EditFormat="custom" UseMaskBehavior="True" ClientInstanceName="ctxtexpirdate" AllowNull="true" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy">
                                                <ButtonStyle Width="13px">
                                                </ButtonStyle>

                                            </dxe:ASPxDateEdit>
                                        </div>
                                    </div>
                                </div>
                                <div class="clear"></div>
                                <div class="blockthree">
                                    <div class="col-md-3">
                                        <div>
                                            <span id="RequiredFieldValidatortxtserialtxt">Serial No</span>
                                        </div>
                                        <div class="Left_Content relative" style="">
                                            <dxe:ASPxTextBox ID="ASPxTextBox1" runat="server" Width="100%" ClientInstanceName="ctxtserial" HorizontalAlign="Left" Font-Size="12px" MaxLength="49">
                                                <ClientSideEvents KeyPress="function(s, e) {Keypressevt();}" />
                                            </dxe:ASPxTextBox>
                                            <span id="RequiredFieldValidatortxtserial" title="Mandatory" class="tp2 fa fa-exclamation-circle iconRed" style="display: none;"></span>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div>
                                    </div>
                                    <div class=" clearfix" style="padding-top: 11px;">
                                        <dxe:ASPxButton ID="ASPxButton5" ClientInstanceName="cbtnaddWarehouse" Width="50px" runat="server" AutoPostBack="False" Text="Add" CssClass="btn btn-primary pull-left" UseSubmitBehavior="False">
                                            <ClientSideEvents Click="function(s, e) {SaveWarehouse();}" />
                                        </dxe:ASPxButton>

                                        <dxe:ASPxButton ID="ASPxButton6" ClientInstanceName="cbtnrefreshWarehouse" Width="50px" runat="server" AutoPostBack="False" Text="Clear Entries" CssClass="btn btn-primary pull-left" UseSubmitBehavior="False">
                                            <ClientSideEvents Click="function(s, e) {Clraear();}" />
                                        </dxe:ASPxButton>

                                    </div>
                                </div>

                            </div>
                            <br />


                            <div class="clearfix">
                                <dxe:ASPxGridView ID="GrdWarehousePC" runat="server" KeyFieldName="SrlNo" AutoGenerateColumns="False" SettingsBehavior-AllowSort="false"
                                    Width="100%" ClientInstanceName="cGrdWarehousePC" OnCustomCallback="GrdWarehousePC_CustomCallback" OnDataBinding="GrdWarehousePC_DataBinding">
                                    <Columns>
                                        <dxe:GridViewDataTextColumn Caption="Warehouse Name" FieldName="viewWarehouseName"
                                            VisibleIndex="0">
                                        </dxe:GridViewDataTextColumn>

                                        <dxe:GridViewDataTextColumn Caption="Batch Number" FieldName="viewBatchNo"
                                            VisibleIndex="2">
                                        </dxe:GridViewDataTextColumn>

                                        <dxe:GridViewDataDateColumn Caption="Manufacture Date" FieldName="viewMFGDate"
                                            VisibleIndex="2">
                                            <Settings AllowHeaderFilter="False" />
                                            <PropertiesDateEdit DisplayFormatString="dd-MM-yyyy"></PropertiesDateEdit>
                                        </dxe:GridViewDataDateColumn>

                                        <dxe:GridViewDataDateColumn Caption="Expiry Date" FieldName="viewExpiryDate"
                                            VisibleIndex="2">
                                            <Settings AllowHeaderFilter="False" />
                                            <PropertiesDateEdit DisplayFormatString="dd-MM-yyyy"></PropertiesDateEdit>
                                        </dxe:GridViewDataDateColumn>
                                        <dxe:GridViewDataTextColumn Caption="Quantity" FieldName="viewQuantity"
                                            VisibleIndex="3">
                                            <Settings ShowInFilterControl="False" />
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="Quantity" FieldName="Quantity"
                                            VisibleIndex="5">
                                            <Settings ShowInFilterControl="False" />
                                        </dxe:GridViewDataTextColumn>

                                        <dxe:GridViewDataTextColumn Caption="Serial Number" FieldName="viewSerialNo"
                                            VisibleIndex="4">
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="Action" FieldName="SrlNo" CellStyle-VerticalAlign="Middle" VisibleIndex="6" CellStyle-HorizontalAlign="Center" Settings-ShowFilterRowMenu="False" Settings-AllowHeaderFilter="False" Settings-AllowAutoFilter="False" Width="100px">
                                            <EditFormSettings Visible="False" />
                                            <DataItemTemplate>
                                                <a href="javascript:void(0);" onclick="UpdateWarehousebatchserial(<%#Eval("SrlNo")%>,'<%#Eval("WarehouseID")%>','<%#Eval("BatchNo")%>','<%#Eval("SerialNo")%>','<%#Eval("isnew")%>','<%#Eval("viewQuantity")%>','<%#Eval("Quantity")%>')" title="update Details" class="pad">
                                                    <img src="../../../assests/images/Edit.png" />
                                                </a>
                                                <a href="javascript:void(0);" onclick="DeleteWarehousebatchserial(<%#Eval("SrlNo")%>,'<%#Eval("BatchWarehouseID")%>','<%#Eval("viewQuantity")%>',<%#Eval("Quantity")%>,'<%#Eval("WarehouseID")%>','<%#Eval("BatchNo")%>')" title="delete Details" class="pad">
                                                    <img src="../../../assests/images/crs.png" />
                                                </a>
                                            </DataItemTemplate>

                                        </dxe:GridViewDataTextColumn>
                                    </Columns>
                                    <ClientSideEvents EndCallback="function(s,e) { cGrdWarehousePCShowError(s.cpInsertError);}" />
                                    <%-- <SettingsPager NumericButtonCount="20" PageSize="10" ShowSeparators="True">
                                    <FirstPageButton Visible="True">
                                    </FirstPageButton>
                                    <LastPageButton Visible="True">
                                    </LastPageButton>
                                </SettingsPager>--ShowFilterRow="true" ShowFilterRowMenu="true" --%>
                                    <SettingsPager Mode="ShowAllRecords" />
                                    <Settings ShowGroupPanel="false" ShowStatusBar="Hidden" ShowHorizontalScrollBar="False" VerticalScrollBarMode="Visible" VerticalScrollableHeight="190" />
                                    <SettingsLoadingPanel Text="Please Wait..." />
                                </dxe:ASPxGridView>
                            </div>
                            <br />
                            <div class="Center_Content" style="">
                                <dxe:ASPxButton ID="ASPxButton7" ClientInstanceName="cbtnWarehouse" Width="50px" runat="server" AutoPostBack="False" Text="S&#818;ave & Exit" AccessKey="S" CssClass="btn btn-primary" UseSubmitBehavior="False">
                                    <ClientSideEvents Click="function(s, e) {SaveWarehouseAll();}" />
                                </dxe:ASPxButton>


                            </div>
                        </div>
                        <%-- <div class="text-center">
                        <table class="pull-right">
                            <tr>
                                <td style="padding-right: 15px"><strong>Total</strong></td>
                                <td>
                                    <dxe:ASPxTextBox ID="ASPxTextBox3" runat="server" Width="100%" ClientInstanceName="ctxtTotalAmount" HorizontalAlign="Right" Font-Size="12px" ReadOnly="true">
                                    </dxe:ASPxTextBox>
                                </td>
                            </tr>
                        </table>
                    </div>--%>
                    </div>
                </dxe:PopupControlContentControl>
            </ContentCollection>
            <HeaderStyle BackColor="LightGray" ForeColor="Black" />
        </dxe:ASPxPopupControl>
        <div id="hdnFieldWareHouse">
            <asp:HiddenField ID="hdfProductIDPC" runat="server" />
            <asp:HiddenField ID="hdfstockidPC" runat="server" />
            <asp:HiddenField ID="hdfopeningstockPC" runat="server" />
            <asp:HiddenField ID="hdbranchIDPC" runat="server" />
            <asp:HiddenField ID="hdnselectedbranch" runat="server" Value="0" />
            <asp:HiddenField ID="hdnProductQuantity" runat="server" />
            <asp:HiddenField ID="hdniswarehouse" runat="server" />
            <asp:HiddenField ID="hdnisbatch" runat="server" />
            <asp:HiddenField ID="hdnisserial" runat="server" />
            <asp:HiddenField ID="hdndefaultID" runat="server" />
            <asp:HiddenField ID="hdnoldrowcount" runat="server" Value="0" />
            <asp:HiddenField ID="hdntotalqntyPC" runat="server" Value="0" />
            <%-- Sam New Modification For Qty Checking--%>
            <asp:HiddenField ID="wbsqtychecking" runat="server" Value="1" />
            <%--<asp:HiddenField ID="producttype" runat="server" Value="" />--%>
            <%--Sam New Modification For Qty Checking--%>
            <asp:HiddenField ID="hdnoldwarehousname" runat="server" />
            <asp:HiddenField ID="hdnoldbatchno" runat="server" />
            <asp:HiddenField ID="hidencountforserial" runat="server" />
            <asp:HiddenField ID="hdnbatchchanged" runat="server" Value="0" />
            <asp:HiddenField ID="hdnrate" runat="server" Value="0" />
            <asp:HiddenField ID="hdnvalue" runat="server" Value="0" />
            <%--<asp:HiddenField ID="oldhdnoldwarehousname" runat="server" Value="0" />--%>
            <%--<asp:HiddenField ID="oldhidencountforserial" runat="server" Value="0" />--%>
            <%--<asp:HiddenField ID="oldhdnbatchchanged" runat="server" Value="0" />--%>
            <asp:HiddenField ID="hdnstrUOM" runat="server" />
            <asp:HiddenField ID="hdnenterdopenqnty" runat="server" />
            <asp:HiddenField ID="hdnnewenterqntity" runat="server" />
            <asp:HiddenField ID="hdnisoldupdate" runat="server" />
            <asp:HiddenField ID="hdncurrentslno" runat="server" />
            <asp:HiddenField ID="oldopeningqntity" runat="server" Value="0" />
            <asp:HiddenField ID="hdnisedited" runat="server" />
            <asp:HiddenField ID="hdnisnewupdate" runat="server" />
            <asp:HiddenField ID="hdnisviewqntityhas" runat="server" />
            <asp:HiddenField ID="hdndeleteqnity" runat="server" Value="0" />
            <asp:HiddenField ID="hdnisolddeleted" runat="server" Value="false" />
            <%--<asp:HiddenField ID="hdnisreduing" runat="server" Value="false" />--%>
            <%-- <asp:HiddenField ID="hdnoutstock" runat="server" Value="0" />--%>
            <asp:HiddenField ID="hdnpcslno" runat="server" Value="0" />
        </div>

        <%--   Warehouse End    --%>

        <%-- HiddenField --%>
        <div>
            <asp:HiddenField runat="server" ID="hdnProjectSelectInEntryModule" />
            <asp:HiddenField ID="hdfLookupCustomer" runat="server" />
            <asp:HiddenField ID="hdfIsDelete" runat="server" />
            <asp:HiddenField ID="hdfIsComp" runat="server" />
            <asp:HiddenField ID="hdnPageStatus" runat="server" />
            <asp:HiddenField ID="hdfProductID" runat="server" />
            <asp:HiddenField ID="hdfProductType" runat="server" />
            <asp:HiddenField ID="hdfProductSerialID" runat="server" />
            <asp:HiddenField ID="hdnRefreshType" runat="server" />
            <asp:HiddenField ID="hdnCustomerId" runat="server" />
            <%--added by sam to delete the noninventory item and its session detail from grid--%>
            <asp:HiddenField ID="hdinvetorttype" runat="server" />
            <%-- added by sam to delete the noninventory item and its session detail from grid--%>
        </div>
        <%-- HiddenField End--%>
        <%--UDF--%>
        <dxe:ASPxPopupControl ID="ASPXPopupControl" runat="server"
            CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="popup" Height="630px"
            Width="600px" HeaderText="Add/Modify UDF" Modal="true" AllowResize="true" ResizingMode="Postponed">
            <%-- <HeaderTemplate>
                <span>UDF</span>
                <dxe:ASPxImage ID="ASPxImage3" runat="server" ImageUrl="/assests/images/closePop.png"  Cursor="pointer" cssClass="popUpHeader" >
                    <ClientSideEvents Click="function(s, e){ 
                        popup.Hide();
                    }" />
            </dxe:ASPxImage>
            </HeaderTemplate>--%>
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">
                </dxe:PopupControlContentControl>
            </ContentCollection>
        </dxe:ASPxPopupControl>
        <asp:HiddenField runat="server" ID="IsUdfpresent" />
        <asp:HiddenField runat="server" ID="Keyval_internalId" />
        <%--End UDF--%>
        <%--Batch Product Popup Start--%>

        <dxe:ASPxPopupControl ID="ProductpopUp" runat="server" ClientInstanceName="cProductpopUp"
            CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" Height="400"
            Width="1000" HeaderText="Select Product " AllowResize="true" ResizingMode="Postponed" Modal="true">
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">
                    <label><strong>Search By Product Name (4 Char)</strong></label>
                    <%--  DataSourceID="ProductDataSource"--%>
                    <dxe:ASPxCallbackPanel runat="server" ID="productPanel" ClientInstanceName="cproductPanel" OnCallback="productPanel_Callback">
                        <PanelCollection>

                            <dxe:PanelContent runat="server">

                                <dxe:ASPxComboBox ID="productLookUp" runat="server" EnableCallbackMode="true" CallbackPageSize="5"
                                    ValueType="System.String" ValueField="Products_ID" ClientInstanceName="cproductLookUp" Width="92%"
                                    OnItemsRequestedByFilterCondition="productLookUp_ItemsRequestedByFilterCondition"
                                    OnItemRequestedByValue="productLookUp_ItemRequestedByValue" TextFormatString="{1} [{0}]"
                                    DropDownStyle="DropDown" DropDownRows="5" ItemStyle-Wrap="True">
                                    <Columns>
                                        <dxe:ListBoxColumn FieldName="Products_Name" Caption="Name" Width="400px" />
                                        <dxe:ListBoxColumn FieldName="IsInventory" Caption="Inventory" Width="90px" />
                                        <dxe:ListBoxColumn FieldName="HSNSAC" Caption="HSN/SAC" Width="100px" />
                                        <dxe:ListBoxColumn FieldName="ClassCode" Caption="Class" Width="100px" />
                                        <dxe:ListBoxColumn FieldName="BrandName" Caption="Brand" Width="100px" />
                                        <dxe:ListBoxColumn FieldName="sProducts_isInstall" Caption="Installation Reqd." Width="100px" />
                                    </Columns>
                                    <ClientSideEvents ValueChanged="ProductSelected" KeyDown="ProductlookUpKeyDown" GotFocus="function(s,e){cproductLookUp.ShowDropDown();}" />

                                </dxe:ASPxComboBox>

                                <%--<dxe:ASPxGridLookup ID="productLookUp" runat="server" ClientInstanceName="cproductLookUp" OnDataBinding="productLookUp_DataBinding"
                        KeyFieldName="Products_ID" Width="800" TextFormatString="{0}" MultiTextSeparator=", "
                         ClientSideEvents-TextChanged="ProductSelected" 
                        ClientSideEvents-KeyDown="ProductlookUpKeyDown">
                        <Columns>
                            <dxe:GridViewDataColumn FieldName="Products_Name" Caption="Name" Width="220">
                                <Settings AutoFilterCondition="Contains" />
                            </dxe:GridViewDataColumn>
                            <dxe:GridViewDataColumn FieldName="IsInventory" Caption="Inventory" Width="60">
                                <Settings AutoFilterCondition="Contains" />
                            </dxe:GridViewDataColumn>
                            <dxe:GridViewDataColumn FieldName="HSNSAC" Caption="HSN/SAC" Width="80">
                                <Settings AutoFilterCondition="Contains" />
                            </dxe:GridViewDataColumn>
                            <dxe:GridViewDataColumn FieldName="ClassCode" Caption="Class" Width="200">
                                <Settings AutoFilterCondition="Contains" />
                            </dxe:GridViewDataColumn>
                            <dxe:GridViewDataColumn FieldName="BrandName" Caption="Brand" Width="100">
                                <Settings AutoFilterCondition="Contains" />
                            </dxe:GridViewDataColumn>
                            <dxe:GridViewDataColumn FieldName="sProducts_isInstall" Caption="Installation Reqd." Width="120">
                                <Settings AutoFilterCondition="Contains" />
                            </dxe:GridViewDataColumn>
                        </Columns>
                        <GridViewProperties Settings-VerticalScrollBarMode="Auto">

                            <Templates>
                                <StatusBar>
                                    <table class="OptionsTable" style="float: right">
                                        <tr>
                                            <td>
                                                
                                            </td>
                                        </tr>
                                    </table>
                                </StatusBar>
                            </Templates>
                            <Settings ShowFilterRow="True" ShowFilterRowMenu="true" ShowStatusBar="Visible" UseFixedTableLayout="true" />
                        </GridViewProperties>
                    </dxe:ASPxGridLookup>--%>
                            </dxe:PanelContent>
                        </PanelCollection>
                    </dxe:ASPxCallbackPanel>

                </dxe:PopupControlContentControl>
            </ContentCollection>
            <HeaderStyle BackColor="Blue" Font-Bold="True" ForeColor="White" />
        </dxe:ASPxPopupControl>

        <%--<asp:SqlDataSource runat="server" ID="ProductDataSource"
            SelectCommand="prc_PurchaseInvoiceDetail" SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:Parameter Type="String" Name="Action" DefaultValue="ProductDetails" />
                <asp:SessionParameter Name="campany_Id" SessionField="LastCompany1" Type="String" />
                <asp:SessionParameter Type="String" Name="FinYear" SessionField="LastFinYear1" />
                <asp:ControlParameter Type="String" Name="IsInventory" ControlID="ctl00$ContentPlaceHolder1$ASPxPageControl1$ddlInventory" PropertyName="SelectedValue" />
                <asp:ControlParameter Type="String" Name="SchemeID" ControlID="ctl00$ContentPlaceHolder1$ASPxPageControl1$ddl_TdsScheme" PropertyName="Value" />

            </SelectParameters>
        </asp:SqlDataSource>--%>

        <%-- <asp:SqlDataSource runat="server" ID="CustomerDataSource" 
            SelectCommand="prc_PurchaseInvoiceDetail" SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:Parameter Type="String" Name="Action" DefaultValue="PopulateVendorsDetailByInventoryItem" />
                <asp:ControlParameter Type="String" Name="InventoryType" ControlID="ctl00$ContentPlaceHolder1$ASPxPageControl1$ddlInventory" PropertyName="SelectedValue" />
                <asp:ControlParameter Type="String" Name="branchId" ControlID="ctl00$ContentPlaceHolder1$ASPxPageControl1$ddl_Branch" PropertyName="SelectedValue" />
            </SelectParameters>
        </asp:SqlDataSource>--%>

        <%--Batch Product Popup End--%>
        <dxe:ASPxCallbackPanel runat="server" ID="acpAvailableStock" ClientInstanceName="cacpAvailableStock" OnCallback="acpAvailableStock_Callback">
            <PanelCollection>
                <dxe:PanelContent runat="server">
                </dxe:PanelContent>
            </PanelCollection>
            <ClientSideEvents EndCallback="acpAvailableStockEndCall" />
        </dxe:ASPxCallbackPanel>

        <%--Div Detail for Vendor Section Start--%>

        <dxe:ASPxCallbackPanel runat="server" ID="acpContactPersonPhone" ClientInstanceName="cacpContactPersonPhone" OnCallback="acpContactPersonPhone_Callback">
            <PanelCollection>
                <dxe:PanelContent runat="server">
                </dxe:PanelContent>
            </PanelCollection>
            <ClientSideEvents EndCallback="acpContactPersonPhoneEndCall" />
        </dxe:ASPxCallbackPanel>
        <%--Div Detail for Vendor Section Start--%>

        <%--Sandip Hidden Field Declaration Start--%>
        <asp:HiddenField runat="server" ID="hdngridvselectedrowno" />
        <%--Sandip Hidden Field Declaration End--%>

        <%--<asp:SqlDataSource ID="SqlSchematype" runat="server" 
            SelectCommand="Select * From ((Select '0' as ID,'Select' as SchemaName) Union (Select ID,SchemaName From tbl_master_Idschema  Where TYPE_ID='17')) as X Order By ID ASC"></asp:SqlDataSource>--%>
        <asp:SqlDataSource ID="SqlSchematype" runat="server"
            SelectCommand="Select * From ((Select '0' as ID,' Select' as SchemaName) Union (Select  convert(nvarchar(10),ID)+'~'+convert(nvarchar(10),b.branch_id) as ID,SchemaName+'('+b.branch_description +')'as SchemaName  From tbl_master_Idschema  join tbl_master_branch b on tbl_master_Idschema.Branch=b.branch_id  Where TYPE_ID='19' 
                    and financial_year_id IN(select FinYear_ID from Master_FinYear where FinYear_Code=@year) 
                    and IsActive=1
                    and Isnull(Branch,'') in (select s FROM dbo.GetSplit(',',@userbranchHierarchy)) and comapanyInt=@company)) as X Order By SchemaName ASC">

            <%--and Branch=@userbranch and comapanyInt=@company)) as X Order By ID ASC">--%>
            <SelectParameters>
                <%-- <asp:sessionparameter name="userbranch" sessionfield="userbranch" type="string" />--%>
                <asp:SessionParameter Name="userbranchHierarchy" SessionField="userbranchHierarchy" Type="string" />
                <asp:SessionParameter Name="company" SessionField="LastCompany" Type="string" />
                <asp:SessionParameter Name="year" SessionField="LastFinYear" Type="string" />
                <%-- <asp:SessionParameter Name="year" SessionField="LastFinYear1" Type="string" />--%>
                <%-- <asp:SessionParameter Name="company" SessionField="LastCompany1" Type="string" />--%>
            </SelectParameters>
        </asp:SqlDataSource>
        <%--  <asp:SqlDataSource ID="SqlIndentRequisitionNo" runat="server" 
            SelectCommand="(Select '0' as Indent_Id,'Select' as Indent_RequisitionNumber) Union
            (select Indent_Id,Indent_RequisitionNumber from tbl_trans_Indent)"></asp:SqlDataSource>--%>


        <%-- <asp:SqlDataSource ID="Sqlvendor" runat="server" 
            SelectCommand="select '0' as cnt_internalId,'Select' as Name 
            union select cnt_internalId,isnull(cnt_firstName,'')+isnull(cnt_middleName,'')+isnull(cnt_lastName,'') Name 
            from tbl_master_contact  where cnt_contacttype='DV'"></asp:SqlDataSource>--%>


        <%--<asp:SqlDataSource ID="SqlCurrency" runat="server" 
            SelectCommand="select Currency_ID,Currency_AlphaCode from Master_Currency Order By Currency_ID"></asp:SqlDataSource>--%>
        <%--<asp:SqlDataSource ID="DS_Branch" runat="server"
            SelectCommand="Select * From (select '0' as branch_id,'Select' as branch_description 
            union select branch_id,branch_description from tbl_master_branch)tbl "></asp:SqlDataSource>--%>
        <%--<asp:SqlDataSource ID="DS_SalesAgent" runat="server" 
            SelectCommand="select '0' as cnt_id,'Select' as Name
            union select cnt_id,isnull(cnt_firstName,'')+isnull(cnt_middleName,'')+isnull(cnt_lastName,'') Name from tbl_master_contact  where Substring(cnt_internalId,1,2)='AG'"></asp:SqlDataSource>--%>
        <%--<asp:SqlDataSource ID="DS_AmountAre" runat="server" 
            SelectCommand="select '0'as taxGrp_Id,'Select'as taxGrp_Description
            union select taxGrp_Id,taxGrp_Description from tbl_master_taxgrouptype order by taxGrp_Id"></asp:SqlDataSource>--%>


        <%--  <asp:SqlDataSource ID="CountrySelect" runat="server" 
            SelectCommand="SELECT cou_id, cou_country as Country FROM tbl_master_country order by cou_country"></asp:SqlDataSource>
        <asp:SqlDataSource ID="StateSelect" runat="server" 
            SelectCommand="SELECT s.id as ID,s.state+' (State Code:' +s.StateCode+')' as State from tbl_master_state s where (s.countryId = @State) ORDER BY s.state">
            <SelectParameters>
                <asp:Parameter Name="State" Type="string" />
            </SelectParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="SelectCity" runat="server" 
            SelectCommand="SELECT c.city_id AS CityId, c.city_name AS City FROM tbl_master_city c where c.state_id=@City order by c.city_name">
            <SelectParameters>
                <asp:Parameter Name="City" Type="string" />
            </SelectParameters>
        </asp:SqlDataSource>

        <asp:SqlDataSource ID="SelectArea" runat="server" 
            SelectCommand="SELECT area_id, area_name from tbl_master_area where (city_id = @Area) ORDER BY area_name">
            <SelectParameters>
                <asp:Parameter Name="Area" Type="string" />
            </SelectParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="SelectPin" runat="server" 
            SelectCommand="select pin_id,pin_code from tbl_master_pinzip where city_id=@City order by pin_code">
            <SelectParameters>
                <asp:Parameter Name="City" Type="string" />
            </SelectParameters>
        </asp:SqlDataSource>--%>


        <dxe:ASPxCallbackPanel runat="server" ID="acbpCrpUdf" ClientInstanceName="cacbpCrpUdf" OnCallback="acbpCrpUdf_Callback">
            <PanelCollection>
                <dxe:PanelContent runat="server">
                </dxe:PanelContent>
            </PanelCollection>
            <ClientSideEvents EndCallback="acbpCrpUdfEndCall" />
        </dxe:ASPxCallbackPanel>

        <%--Inventory Section By Sam Start on 15052017--%>

        <dxe:ASPxPopupControl ID="inventorypopup" runat="server" ClientInstanceName="cinventorypopup"
            Width="1080px" HeaderText="Select TDS" PopupHorizontalAlign="WindowCenter" ShowCloseButton="false"
            PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
            Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True">
            <%-- <HeaderTemplate>--%>
            <%-- <span style="color: #fff"><strong>Select Tax</strong></span>--%>
            <%--<dxe:ASPxImage ID="ASPxImage3" runat="server" ImageUrl="/assests/images/closePop.png" Cursor="pointer" CssClass="popUpHeader pull-right">
                        <ClientSideEvents Click="function(s, e){ 
                                                            
                                                            cinventorypopup.Hide();
                                                        }" />
                    </dxe:ASPxImage>--%>
            <%--</HeaderTemplate>--%>
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">
                    <%--<asp:HiddenField runat="server" ID="HiddenField1" />
                        <asp:HiddenField runat="server" ID="HiddenField2" />
                        <asp:HiddenField runat="server" ID="HiddenField3" />
                        <asp:HiddenField runat="server" ID="HiddenField4" />
                        <asp:HiddenField ID="HiddenField5" runat="server" />--%>
                    <%-- Added by Sam to show default cursor after save--%>

                    <%-- Added by Sam to show default cursor after save--%>


                    <%--Error Message--%>





                    <div class="row">
                        <div class="col-md-3">
                            <label><span><strong>Select Branch</strong></span></label>
                            <%--<dxe:ASPxComboBox ID="ddl_noninventoryBranch" ClientInstanceName="cddl_noninventoryBranch" runat="server" SelectedIndex="-1"
                                ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True" TextFormatString="{0}"
                                ClearButton-DisplayMode="Always" OnCallback="ddl_noninventoryBranch_Callback">
                            </dxe:ASPxComboBox>--%>
                            <div>
                                <asp:Label ID="lbltdsBranch" runat="server"></asp:Label>
                            </div>

                        </div>


                        <div class="col-md-3">
                            <label><span><strong>Edit TDS</strong></span></label>

                            <div>
                                <dxe:ASPxCheckBox ID="chk_TDSEditable" ClientInstanceName="cchk_TDSEditable" runat="server">
                                    <ClientSideEvents CheckedChanged="TDSEditableCheckChanged" />
                                </dxe:ASPxCheckBox>
                            </div>

                        </div>


                        <div class="col-md-3">
                            <label><span><strong>Select Month for TDS</strong></span></label>
                            <dxe:ASPxComboBox ID="ddl_month" ClientInstanceName="cddl_month" runat="server" SelectedIndex="-1"
                                ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True" TextFormatString="{0}"
                                ClearButton-DisplayMode="Always">
                                <Items>
                                    <dxe:ListEditItem Text="April" Value="April" />
                                    <dxe:ListEditItem Text="May" Value="May" />
                                    <dxe:ListEditItem Text="June" Value="June" />
                                    <dxe:ListEditItem Text="July" Value="July" />
                                    <dxe:ListEditItem Text="August" Value="August" />
                                    <dxe:ListEditItem Text="September" Value="September" />
                                    <dxe:ListEditItem Text="October" Value="October" />
                                    <dxe:ListEditItem Text="November" Value="November" />
                                    <dxe:ListEditItem Text="December" Value="December" />
                                    <dxe:ListEditItem Text="January" Value="January" />
                                    <dxe:ListEditItem Text="February" Value="February" />
                                    <dxe:ListEditItem Text="March" Value="March" />
                                </Items>
                            </dxe:ASPxComboBox>
                        </div>

                        <div class="col-md-3 ">
                            <label><span><strong>Product Basic Amount</strong></span></label>
                            <div style="padding-bottom: 5px">
                                <dxe:ASPxTextBox ID="txt_proamt" MaxLength="80" ClientInstanceName="ctxt_proamt" ReadOnly="true" DisplayFormatString="0.00"
                                    runat="server" Width="50%">
                                    <%--<MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />--%>
                                    <%--<MaskSettings Mask="<0..999999999>.<0..99>" AllowMouseWheel="false" />--%>
                                </dxe:ASPxTextBox>
                            </div>
                        </div>
                    </div>
                    <table style="width: 100%;">



                        <tr>
                        </tr>

                        <%--<tr class="cgridTaxClass"> Enabled="false"--%>
                        <tr>

                            <td colspan="4">

                                <dxe:ASPxGridView runat="server" KeyFieldName="TDSID" ClientInstanceName="cgridinventory" ID="gridinventory"
                                    Width="100%" SettingsBehavior-AllowSort="false" OnBatchUpdate="gridinventory_BatchUpdate" SettingsBehavior-AllowDragDrop="false" SettingsPager-Mode="ShowAllRecords"
                                    OnCustomCallback="gridinventory_CustomCallback"
                                    Settings-ShowFooter="false" AutoGenerateColumns="False" OnRowUpdating="gridinventory_RowUpdating" OnRowInserting="gridinventory_RowInserting">
                                    <Settings VerticalScrollableHeight="150" VerticalScrollBarMode="Auto"></Settings>
                                    <SettingsBehavior AllowDragDrop="False" AllowSort="False"></SettingsBehavior>
                                    <SettingsPager Visible="false"></SettingsPager>
                                    <Columns>
                                        <%--<dxe:GridViewDataTextColumn VisibleIndex="1" FieldName="TDSTCSRates_Code" ReadOnly="true" Caption="TDS CODE">
                                            </dxe:GridViewDataTextColumn>--%>

                                        <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="TDSRate" Caption="TDS Rate(%)" Width="8%">
                                            <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right">
                                                <%--<ClientSideEvents LostFocus="txtPercentageLostFocus" GotFocus="CmbtaxClick" />--%>
                                                <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                            </PropertiesTextEdit>
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="TDS amount" FieldName="TDSAmount" VisibleIndex="3" Width="8%">
                                            <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right">
                                                <%--<ClientSideEvents LostFocus="txtPercentageLostFocus" GotFocus="CmbtaxClick" />--%>
                                                <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                                <ClientSideEvents LostFocus="TDSAmtLostFocus" />
                                            </PropertiesTextEdit>
                                            <CellStyle Wrap="False" HorizontalAlign="Right"></CellStyle>

                                        </dxe:GridViewDataTextColumn>


                                        <dxe:GridViewDataTextColumn VisibleIndex="4" FieldName="SurchargeRate" Caption="Surcharge Rate(%)" Width="11%">
                                            <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right">
                                                <%--<ClientSideEvents LostFocus="txtPercentageLostFocus" GotFocus="CmbtaxClick" />--%>
                                                <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                            </PropertiesTextEdit>
                                            <CellStyle Wrap="False" HorizontalAlign="Right"></CellStyle>
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="Surcharge amount" FieldName="SurchargeAmount" VisibleIndex="5" Width="11%">
                                            <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right">
                                                <%--<ClientSideEvents LostFocus="txtPercentageLostFocus" GotFocus="CmbtaxClick" />--%>
                                                <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                                <ClientSideEvents LostFocus="SurchargeAmountLostFocus" />
                                            </PropertiesTextEdit>
                                            <CellStyle Wrap="False" HorizontalAlign="Right"></CellStyle>
                                        </dxe:GridViewDataTextColumn>


                                        <dxe:GridViewDataTextColumn VisibleIndex="6" FieldName="EducationCessRate" Caption="Education Cess Rate(%)" Width="14%">
                                            <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right">
                                                <%--<ClientSideEvents LostFocus="txtPercentageLostFocus" GotFocus="CmbtaxClick" />--%>
                                                <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />

                                            </PropertiesTextEdit>
                                            <CellStyle Wrap="False" HorizontalAlign="Right"></CellStyle>
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="Education Cess Amount" FieldName="EducationCessAmt" VisibleIndex="7" Width="14%">
                                            <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right">
                                                <%--<ClientSideEvents LostFocus="txtPercentageLostFocus" GotFocus="CmbtaxClick" />--%>
                                                <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                                <ClientSideEvents LostFocus="EducationCessAmtLostFocus" />
                                            </PropertiesTextEdit>
                                            <CellStyle Wrap="False" HorizontalAlign="Right"></CellStyle>
                                        </dxe:GridViewDataTextColumn>


                                        <dxe:GridViewDataTextColumn VisibleIndex="8" FieldName="HgrEducationCessRate" Caption="Higher Education Cess Rate(%)" Width="17%">
                                            <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right">
                                                <%--<ClientSideEvents LostFocus="txtPercentageLostFocus" GotFocus="CmbtaxClick" />--%>
                                                <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                            </PropertiesTextEdit>
                                            <CellStyle Wrap="False" HorizontalAlign="Right"></CellStyle>
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="Higher Education Cess Amount" FieldName="HgrEducationCessAmt" VisibleIndex="9" Width="17%">
                                            <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right">
                                                <%--<ClientSideEvents LostFocus="txtPercentageLostFocus" GotFocus="CmbtaxClick" />--%>
                                                <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                                <ClientSideEvents LostFocus="HgrEducationCessAmtLostFocus" />
                                            </PropertiesTextEdit>
                                            <CellStyle Wrap="False" HorizontalAlign="Right"></CellStyle>
                                        </dxe:GridViewDataTextColumn>




                                    </Columns>
                                    <Styles>
                                        <StatusBar CssClass="statusBar"></StatusBar>
                                    </Styles>
                                    <ClientSideEvents EndCallback="OnInventoryEndCallback" />
                                    <%--  <SettingsPager Mode="ShowAllRecords"></SettingsPager>--%>
                                    <%-- <SettingsDataSecurity AllowEdit="true" />--%>
                                    <%--<SettingsEditing Mode="Batch">
                                            <BatchEditSettings EditMode="row" />
                                        </SettingsEditing>--%>
                                    <SettingsDataSecurity AllowEdit="true" />
                                    <SettingsEditing Mode="Batch" NewItemRowPosition="Bottom">
                                        <BatchEditSettings ShowConfirmOnLosingChanges="false" EditMode="row" />
                                    </SettingsEditing>
                                </dxe:ASPxGridView>

                            </td>
                        </tr>

                        <tr>
                            <td colspan="4">
                                <div class="pull-left">

                                    <dxe:ASPxButton ID="btn_noninventoryOk" ClientInstanceName="cbtn_noninventoryOk" runat="server" AutoPostBack="False" Text="OK" CssClass="btn btn-primary mTop" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                                        <ClientSideEvents Click="function(s, e) {return NonInventoryBatchUpdate();}" />
                                    </dxe:ASPxButton>

                                </div>
                                <table class="pull-right">
                                    <tr>
                                        <td style="padding-top: 10px; padding-right: 5px"><strong>Total TDS</strong></td>
                                        <td>
                                            <dxe:ASPxTextBox ID="txt_totalnoninventoryproductamt" MaxLength="80" ClientInstanceName="ctxt_totalnoninventoryproductamt" DisplayFormatString="0.00"
                                                ReadOnly="true"
                                                runat="server" Width="100%" CssClass="pull-left mTop">
                                                <%-- <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..00&gt;" AllowMouseWheel="false" />--%>
                                                <%--<MaskSettings Mask="<-999999999..999999999>.<0..00>" AllowMouseWheel="false" />--%>
                                                <%--<MaskSettings Mask="<0..999999999>.<0..99>" AllowMouseWheel="false" /> --%>
                                            </dxe:ASPxTextBox>

                                        </td>
                                    </tr>
                                </table>


                                <div class="clear"></div>
                            </td>
                        </tr>




                    </table>
                </dxe:PopupControlContentControl>
            </ContentCollection>
            <ContentStyle VerticalAlign="Top" CssClass="pad"></ContentStyle>
            <HeaderStyle BackColor="LightGray" ForeColor="Black" />
        </dxe:ASPxPopupControl>
        <asp:HiddenField ID="hdntdschecking" runat="server" />
        <%--Inventory Section By Sam End on 15052017 --%>

        <dxe:ASPxCallbackPanel runat="server" ID="ApplicableAmtPopup" ClientInstanceName="CApplicableAmtPopup" OnCallback="ApplicableAmtPopup_Callback">
            <PanelCollection>
                <dxe:PanelContent runat="server">
                </dxe:PanelContent>
            </PanelCollection>
            <%-- <clientsideevents endcallback="panelEndCallBack" />--%>
        </dxe:ASPxCallbackPanel>
    </div>






    <%--New Modification for Multi Selection Of Bill type by Sam on 20022018 Section Start--%>
    <div>
        <dxe:ASPxPopupControl ID="popup_taggingGrid" runat="server" ClientInstanceName="cpopup_taggingGrid"
            HeaderText="Select Bill" PopupHorizontalAlign="WindowCenter"
            BackColor="white" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton" Height="400px" Width="850px"
            Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
            ContentStyle-CssClass="pad">
            <ContentStyle VerticalAlign="Top" CssClass="pad">
            </ContentStyle>
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">
                    <%-- <div style="padding: 7px 0;">
                        <input type="button" value="Select All Products" onclick="Tag_ChangeState('SelectAll')" class="btn btn-primary"></input>
                        <input type="button" value="De-select All Products" onclick="Tag_ChangeState('UnSelectAll')" class="btn btn-primary"></input>
                        <input type="button" value="Revert" onclick="Tag_ChangeState('Revart')" class="btn btn-primary"></input>
                    </div>--%>
                    <%--OnCustomCallback="taggingGrid_CustomCallback" OnDataBinding="taggingGrid_DataBinding"--%>
                    <div>
                        <dxe:ASPxGridView ID="taggingGrid" ClientInstanceName="ctaggingGrid" runat="server" KeyFieldName="invoice_id"
                            Width="100%" SettingsBehavior-AllowDragDrop="false" SettingsPager-Mode="ShowAllRecords"
                            Settings-ShowFooter="false" AutoGenerateColumns="False" Settings-VerticalScrollableHeight="300" Settings-VerticalScrollBarMode="Visible"
                            OnCustomCallback="taggingGrid_CustomCallback" OnDataBinding="taggingGrid_DataBinding">

                            <SettingsBehavior AllowDragDrop="False"></SettingsBehavior>
                            <SettingsPager Visible="false"></SettingsPager>
                            <Columns>
                                <%--<dxe:GridViewCommandColumn ShowSelectCheckbox="True" Width="40" Caption=" " VisibleIndex="0" />
                                <dxe:GridViewDataTextColumn FieldName="PurchaseOrder_Number" Caption="PurchaseOrder_Number" Width="150" VisibleIndex="1">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="ComponentDate" Caption="Purchase Date" Width="100" VisibleIndex="2">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="CustomerName" Caption="Vendor Name" Width="150" VisibleIndex="3">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="ReferenceName" Caption="Reference" Width="150" VisibleIndex="4">
                                </dxe:GridViewDataTextColumn>--%>

                                <dxe:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Width="60" Caption=" " />
                                <dxe:GridViewDataColumn FieldName="invoice_number" Visible="true" VisibleIndex="1" Caption="Invoice Number" Settings-AutoFilterCondition="Contains" Width="180" />
                                <dxe:GridViewDataColumn FieldName="Invoice_Date" Visible="true" VisibleIndex="2" Caption="Invoice Date" Settings-AutoFilterCondition="Contains" Width="180" />

                                <dxe:GridViewDataColumn FieldName="PartyInvoiceNo" Visible="true" VisibleIndex="3" Caption="Party Invoice Number" Settings-AutoFilterCondition="Contains" Width="180" />
                                <dxe:GridViewDataColumn FieldName="PartyInvoiceDate" Visible="true" VisibleIndex="4" Caption="Party Invoice Date" Settings-AutoFilterCondition="Contains" Width="180" />
                                <dxe:GridViewDataColumn FieldName="Type" Visible="true" VisibleIndex="5" Caption="Bill Type" Settings-AutoFilterCondition="Contains" Width="180" />
                            </Columns>
                            <SettingsDataSecurity AllowEdit="true" />
                            <Settings ShowFilterRow="true" ShowFilterRowMenu="true" />
                            <ClientSideEvents EndCallback="TaggedDoc_EndCallback" />
                        </dxe:ASPxGridView>
                    </div>
                    <div class="text-center">
                        <dxe:ASPxButton ID="btnTaggingSave" ClientInstanceName="cbtnTaggingSave" runat="server" AutoPostBack="False" Text="OK" CssClass="btn btn-primary" UseSubmitBehavior="false">
                            <ClientSideEvents Click="function(s, e) {SelectTaggDoc();}" />
                        </dxe:ASPxButton>
                    </div>
                </dxe:PopupControlContentControl>
            </ContentCollection>
            <HeaderStyle BackColor="LightGray" ForeColor="Black" />
        </dxe:ASPxPopupControl>
    </div>
    <%--New Modification for Multi Selection Of Bill type by Sam on 20022018 Section End--%>




    <%-- new Modified Hidden Tax Field--%>
    <asp:HiddenField runat="server" ID="HDItemLevelTaxDetails" />
    <asp:HiddenField runat="server" ID="HDHSNCodewisetaxSchemid" />
    <asp:HiddenField runat="server" ID="HDBranchWiseStateTax" />
    <asp:HiddenField runat="server" ID="HDStateCodeWiseStateIDTax" />
    <%--  new Modified Hidden Tax Field--%>

    <asp:HiddenField ID="hdnManual" runat="server" Value="" />
    <asp:HiddenField ID="hdnAuto" runat="server" Value="" />

    <dxe:ASPxGlobalEvents ID="GlobalEvents" runat="server">
        <ClientSideEvents ControlsInitialized="AllControlInitilize" />
    </dxe:ASPxGlobalEvents>

    <dxe:ASPxLoadingPanel ID="LoadingPanel" runat="server" ClientInstanceName="LoadingPanel" ContainerElementID="divSubmitButton"
        Modal="True">
    </dxe:ASPxLoadingPanel>
    <asp:SqlDataSource ID="VendorDataSource" runat="server" />
    <!--Vendor Modal -->
    <div class="modal fade" id="CustModel" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Transporter Search</h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="Customerkeydown(event)" id="txtCustSearch" autofocus width="100%" placeholder="Search By Transporter Name or Unique Id" />
                    <div id="CustomerTable">
                        <table border='1' width="100%">
                            <tr class="HeaderStyle">
                                <th class="hide">id</th>
                                <th>Transporter Name</th>
                                <th>Unique Id</th>
                                <th>Type</th>
                            </tr>
                        </table>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>
    <!--Vendor Modal -->

    <!--Product Modal -->
    <div class="modal fade" id="ProductModel" role="dialog">
        <div class="modal-dialog">

            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Product Search</h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="Purchaseprodkeydown(event)" id="txtProdSearch" autofocus width="100%" placeholder="Search By Product Name" />

                    <div id="ProductTable">
                        <table border='1' width="100%" class="dynamicPopupTbl">
                            <tr class="HeaderStyle">
                                <th class="hide">id</th>
                                <th>Product Code</th>
                                <th>Product Description</th>
                                <th>Inventory</th>
                                <th>HSN/SAC</th>
                                <th>Class</th>
                                <th>Brand</th>

                            </tr>
                        </table>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                </div>
            </div>

        </div>
    </div>
    <asp:HiddenField ID="hdnProjectMandatory" runat="server" />
    <%--Mantis Issue 24164--%>
    <asp:HiddenField ID="HdnBackDatedEntryTransporter" runat="server" />
    <%--End of Mantis Issue 24164--%>

    <dxe:ASPxCallbackPanel runat="server" ID="callback_InlineRemarks" ClientInstanceName="ccallback_InlineRemarks" OnCallback="callback_InlineRemarks_Callback">
        <PanelCollection>
            <dxe:PanelContent runat="server">
                <dxe:ASPxPopupControl ID="Popup_InlineRemarks" runat="server" ClientInstanceName="cPopup_InlineRemarks"
                    Width="900px" HeaderText="Remarks" PopupHorizontalAlign="WindowCenter"
                    BackColor="white" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
                    Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
                    ContentStyle-CssClass="pad">
                    <%--<ClientSideEvents Closing="function(s, e) {
	                   closeRemarks(s, e);}" />--%>
                    <ContentStyle VerticalAlign="Top" CssClass="pad">
                    </ContentStyle>
                    <ContentCollection>


                        <dxe:PopupControlContentControl runat="server">
                            <div>
                                <asp:Label ID="lblInlineRemarks" runat="server" Text="Remarks"></asp:Label>

                                <asp:TextBox ID="txtInlineRemarks" runat="server" TabIndex="16" Width="100%" TextMode="MultiLine" Rows="5" Columns="10" Height="50px" MaxLength="5000"></asp:TextBox>
                            </div>


                            <div class="clearfix">
                                <br />
                                <div style="align-content: center">
                                    <dxe:ASPxButton ID="btnSaveInlineRemarks" ClientInstanceName="cbtnSaveInlineRemarks" Width="50px" runat="server" AutoPostBack="false" Text="OK" CssClass="btn btn-primary" UseSubmitBehavior="false">

                                        <ClientSideEvents Click="function(s, e) {FinalRemarks();}" />
                                    </dxe:ASPxButton>
                                </div>
                            </div>
                        </dxe:PopupControlContentControl>



                    </ContentCollection>
                    <HeaderStyle BackColor="LightGray" ForeColor="Black" />
                </dxe:ASPxPopupControl>
            </dxe:PanelContent>
        </PanelCollection>
        <ClientSideEvents EndCallback="callback_InlineRemarks_EndCall" />
    </dxe:ASPxCallbackPanel>

</asp:Content>
