<%--=======================================================Revision History=====================================================    
    1.0   Pallab    V2.0.38   09-05-2023      26060: Add Purchase Return Request module design modification & check in small device
    2.0   Pallab    V2.0.38   19-06-2023    0026386: Add Purchase Return Request all bootstrap modal outside click event disable
=========================================================End Revision History===================================================--%>

<%@ Page Title="BranchRequisitionReturn" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="BranchRequisitionReturn.aspx.cs" Inherits="ERP.OMS.Management.Activities.BranchRequisitionReturn" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <link href="CSS/SearchPopup.css" rel="stylesheet" />
    <script src="JS/SearchPopup.js"></script>
    
    <script>
        function GridCallBack() {
            InsgridBatch.PerformCallback('Display');
        }
        //Code for UDF Control 
        function OpenUdf() {
            if (document.getElementById('IsUdfpresent').value == '0') {
                jAlert("UDF not define.");
            }
            else {
                var keyVal = document.getElementById('Keyval_internalId').value;
                var url = '/OMS/management/Master/frm_BranchUdfPopUp.aspx?Type=PRR&&KeyVal_InternalID=' + keyVal;
                popup.SetContentUrl(url);
                popup.Show();
            }
            return true;
        }
        function acbpCrpUdfEndCall(s, e) {
            if (cacbpCrpUdf.cpUDFBI) {
                if (cacbpCrpUdf.cpUDFBI == "true") {
                    InsgridBatch.batchEditApi.EndEdit();
                    InsgridBatch.UpdateEdit();
                    cacbpCrpUdf.cpUDFBI = null;
                }
                else {

                    jAlert('UDF is set as Mandatory. Please enter values.', 'Alert Dialog: [BranchRequisition]', function (r) {
                        if (r == true) {
                            OpenUdf();
                            InsgridBatch.batchEditApi.StartEdit(-1, 1);
                            InsgridBatch.batchEditApi.StartEdit(0, 2);
                        }
                    });

                    cacbpCrpUdf.cpUDFBI = null;
                }
            }
        }
        // End Udf Code
        function gridFocusedRowChanged(s, e) {
            globalRowIndex = e.visibleIndex;
        }
        var preColumn = '';
        var globalRowIndex;
        var chkAccount = 0;
        var currentval = '';
        function PageLoad() {
            FinYearCheckOnPageLoad();
        }
        function FinYearCheckOnPageLoad() {
            var SelectedDate = new Date(ctDate.GetDate());
            var monthnumber = SelectedDate.getMonth();
            var monthday = SelectedDate.getDate();
            var year = SelectedDate.getYear();

            var SelectedDateValue = new Date(year, monthnumber, monthday);

            var FYS = "<%=Session["FinYearStart"]%>";
            var FYE = "<%=Session["FinYearEnd"]%>";
            var LFY = "<%=Session["LastFinYear"]%>";
            var FinYearStartDate = new Date(FYS);
            var FinYearEndDate = new Date(FYE);
            var LastFinYearDate = new Date(LFY);

            monthnumber = FinYearStartDate.getMonth();
            monthday = FinYearStartDate.getDate();
            year = FinYearStartDate.getYear();
            var FinYearStartDateValue = new Date(year, monthnumber, monthday);


            monthnumber = FinYearEndDate.getMonth();
            monthday = FinYearEndDate.getDate();
            year = FinYearEndDate.getYear();
            var FinYearEndDateValue = new Date(year, monthnumber, monthday);

            var SelectedDateNumericValue = SelectedDateValue.getTime();
            var FinYearStartDateNumericValue = FinYearStartDateValue.getTime();
            var FinYearEndDatNumbericValue = FinYearEndDateValue.getTime();
            if (SelectedDateNumericValue >= FinYearStartDateNumericValue && SelectedDateNumericValue <= FinYearEndDatNumbericValue) {

            }
            else {
                if (SelectedDateNumericValue < FinYearStartDateNumericValue) {
                    ctDate.SetDate(new Date(FinYearStartDate));
                }
                if (SelectedDateNumericValue > FinYearEndDatNumbericValue) {
                    ctDate.SetDate(new Date(FinYearEndDate));
                }
            }
        }
        function TDateChange() {
            var SelectedDate = new Date(ctDate.GetDate());
            var monthnumber = SelectedDate.getMonth();
            var monthday = SelectedDate.getDate();
            var year = SelectedDate.getYear();

            var SelectedDateValue = new Date(year, monthnumber, monthday);
            ///Checking of Transaction Date For MaxLockDate
            var MaxLockDate = new Date('<%=Session["LCKBNK"]%>');
            monthnumber = MaxLockDate.getMonth();
            monthday = MaxLockDate.getDate();
            year = MaxLockDate.getYear();
            var MaxLockDateNumeric = new Date(year, monthnumber, monthday).getTime();

            if (SelectedDateValue <= MaxLockDateNumeric) {
                jAlert('This Entry Date has been Locked.');
                MaxLockDate.setDate(MaxLockDate.getDate() + 1);
                ctDate.SetDate(MaxLockDate);
                return;
            }
            ///End Checking of Transaction Date For MaxLockDate

            ///Date Should Between Current Fin Year StartDate and EndDate
            var FYS = "<%=Session["FinYearStart"]%>";
            var FYE = "<%=Session["FinYearEnd"]%>";
            var LFY = "<%=Session["LastFinYear"]%>";
            var FinYearStartDate = new Date(FYS);
            var FinYearEndDate = new Date(FYE);
            var LastFinYearDate = new Date(LFY);

            monthnumber = FinYearStartDate.getMonth();
            monthday = FinYearStartDate.getDate();
            year = FinYearStartDate.getYear();
            var FinYearStartDateValue = new Date(year, monthnumber, monthday);


            monthnumber = FinYearEndDate.getMonth();
            monthday = FinYearEndDate.getDate();
            year = FinYearEndDate.getYear();
            var FinYearEndDateValue = new Date(year, monthnumber, monthday);


            var SelectedDateNumericValue = SelectedDateValue.getTime();
            var FinYearStartDateNumericValue = FinYearStartDateValue.getTime();
            var FinYearEndDatNumbericValue = FinYearEndDateValue.getTime();
            if (SelectedDateNumericValue >= FinYearStartDateNumericValue && SelectedDateNumericValue <= FinYearEndDatNumbericValue) {

            }
            else {
                jAlert('Enter Date Is Outside Of Financial Year !!');
                if (SelectedDateNumericValue < FinYearStartDateNumericValue) {
                    ctDate.SetDate(new Date(FinYearStartDate));
                }
                if (SelectedDateNumericValue > FinYearEndDatNumbericValue) {
                    ctDate.SetDate(new Date(FinYearEndDate));
                }
            }
            ///End OF Date Should Between Current Fin Year StartDate and EndDate
        }
        function InstrumentDateChange() {

            var ExpectedDeliveryDate = new Date(InsgridBatch.GetEditor('ExpectedDeliveryDate').GetValue());
            var requisitionDate = new Date(ctDate.GetValue());


            var datediff = ExpectedDeliveryDate - requisitionDate;
            if (ExpectedDeliveryDate.format('yyyy-MM-dd') != requisitionDate.format('yyyy-MM-dd'))
                if (ExpectedDeliveryDate < requisitionDate) {
                    jAlert('Expected Delivery date must be same or later to Requisition Date. Cannot Proceed.');
                    InsgridBatch.GetEditor('ExpectedDeliveryDate').SetValue(null);
                }


        }
        function ddlBranchFor_SelectedIndexChanged() {
            var BranchFor = $("#ddlBranch").val();
            cddlBranchTo.PerformCallback(BranchFor);

        }
        function GetVisibleIndex(s, e) {
            globalRowIndex = e.visibleIndex;
        }
        //...................Shortcut keys.................
        var isCtrl = false;
        document.onkeydown = function (e) {
            if (event.keyCode == 18) isCtrl = true;



            if (event.keyCode == 78 && event.altKey == true) {
                //run code for Ctrl+S -- ie, save!          

                if (document.getElementById('btnnew').style.display != 'none') {
                    Save_ButtonClick();
                }

            }
            else if (event.keyCode == 88 && event.altKey == true) {

                //run code for Ctrl+X -- ie, Save & Exit! 
                if (document.getElementById('btnSaveExit').style.display != 'none') {
                    document.getElementById('btnSaveExit').click();
                    return false;
                }
            }
            else if ((event.keyCode == 120 || event.keyCode == 65) && event.altKey == true) {
                //run code for Ctrl+A -- ie, Add New
                if (document.getElementById('DivEntry').style.display != 'block') {
                    AddButtonClick();
                }
            }
            else if (event.keyCode == 85 && event.altKey == true) {
                OpenUdf();
            }
        }
        //...................end............................
        function ChangeBranchTo() {

            if (document.getElementById('ddlBranchTo').value == "0") {
                $("#MandatoryBranchTo").show();

            }
            else {
                $("#MandatoryBranchTo").hide();
            }
        }
        function ShowMsgLastCall() {

            if (CgvPurchaseIndent.cpDelete != null) {

                jAlert(CgvPurchaseIndent.cpDelete)
                CgvPurchaseIndent.PerformCallback();
                CgvPurchaseIndent.cpDelete = null;
            }
        }
        function CustomButtonClick(s, e) {
            if (e.buttonID == 'CustomBtnView') {
                $('#<%=hdnEditClick.ClientID %>').val('T'); //Edit

                VisibleIndexE = e.visibleIndex;
                $('#<%= lblHeading.ClientID %>').text("View Branch Requisition");
                document.getElementById('DivEntry').style.display = 'block';

                document.getElementById('DivEdit').style.display = 'none';
                document.getElementById('btnAddNew').style.display = 'none';

                btncross.style.display = "block";
                $('#<%=hdn_Mode.ClientID %>').val('View');
                InsgridBatch.PerformCallback("View~" + VisibleIndexE);
                //LoadingPanel.Show();
                chkAccount = 1;
                document.getElementById('divNumberingScheme').style.display = 'none';
            }
            if (e.buttonID == 'CustomBtnEdit') {
                var userbranchID = '<%=Session["userbranchID"]%>';

                s.GetRowValues(e.visibleIndex, 'Indent_BranchIdFor', function (value) {

                    var BranchIdFor = value;

                    if (userbranchID != BranchIdFor) {
                        jAlert("Requested By Other Branch. Cannot Modify.");
                    }
                    else {
                        $('#<%=hdnEditClick.ClientID %>').val('T'); //Edit
                        $('#<%=hdn_Mode.ClientID %>').val('Edit'); //Edit
                        VisibleIndexE = e.visibleIndex;

                        $('#<%= lblHeading.ClientID %>').text("Modify Branch Requisition");
                        document.getElementById('DivEntry').style.display = 'block';
                        document.getElementById('DivEdit').style.display = 'none';
                        document.getElementById('btnAddNew').style.display = 'none';
                        btncross.style.display = "block";
                        chkAccount = 1;

                        InsgridBatch.PerformCallback("Edit~" + VisibleIndexE);

                        document.getElementById('divNumberingScheme').style.display = 'none';
                        //InsgridBatch.AddNewRow();
                        //InsgridBatch.SetFocusedRowIndex();
                        //s.GetRowValues(e.visibleIndex, 'ValueDate;TransactionDate;MaxLockDate', OnGetRowValuesOnEdit);
                    }
                });
            }
            if (e.buttonID == 'CustomBtnDelete') {
                jConfirm('Confirm delete?', 'Confirmation Dialog', function (r) {
                    if (r == true) {
                        VisibleIndexE = e.visibleIndex;
                        CgvPurchaseIndent.PerformCallback("Delete~" + VisibleIndexE);

                    }
                    else {
                        return false;
                    }
                });


            }
            else if (e.buttonID == 'CustomBtnPrint') {

                var keyValueindex = s.GetRowKey(e.visibleIndex);
                onPrintJv(keyValueindex);

            }
        }
        function SaveExitButtonClick() {
            $('#<%=hdnSaveNew.ClientID %>').val("Save_Exit");
            $('#<%=hdnRefreshType.ClientID %>').val('E');
            $('#<%=hdfIsDelete.ClientID %>').val('I');
            if (document.getElementById('<%= txtVoucherNo.ClientID %>').value == "") {
                $("#MandatoryBillNo").show();

                return false;
            }

            if (cddlBranchTo.GetValue() == null) {
                $("#MandatoryBranchTo").show();
                return false;
            }
            if (ctxtMemoPurpose.GetValue() == null) {
                $("#MandatoryRegion").show();
                return false;

            }
            var IsType = "";
            var frontRow = 0;
            var backRow = -1;

            for (var i = 0; i <= InsgridBatch.GetVisibleRowsOnPage() ; i++) {
                var frontProduct = "";
                var backProduct = "";

                frontProduct = (InsgridBatch.batchEditApi.GetCellValue(backRow, 'ProductName') != null) ? (InsgridBatch.batchEditApi.GetCellValue(backRow, 'ProductName')) : "";
                backProduct = (InsgridBatch.batchEditApi.GetCellValue(frontRow, 'ProductName') != null) ? (InsgridBatch.batchEditApi.GetCellValue(frontRow, 'ProductName')) : "";
                if (frontProduct != "" || backProduct != "") {
                    IsType = "Y";
                    break;
                }
                backRow--;
                frontRow++;
            }

            if (InsgridBatch.GetVisibleRowsOnPage() > 0) {

                if (IsType == "Y") {

                    InsgridBatch.UpdateEdit();
                }
                else {
                    jAlert('Cannot Save. You must enter atleast one Product to save this entry.');

                }
            }
            else {
                jAlert('Cannot Save. You must enter atleast one Product to save this entry.');

            }

        }
        function deleteAllRows() {
            var frontRow = 0;
            var backRow = -1;
            for (var i = 0; i <= InsgridBatch.GetVisibleRowsOnPage() + 100 ; i++) {
                InsgridBatch.DeleteRow(frontRow);
                InsgridBatch.DeleteRow(backRow);
                backRow--;
                frontRow++;
            }

        }

        function AutoCalValueBtRate(s, e) {
            var Quantity = (InsgridBatch.GetEditor('gvColQuantity').GetValue() != null) ? parseFloat(InsgridBatch.GetEditor('gvColQuantity').GetValue()) : "0";
            var Rate = (InsgridBatch.GetEditor('gvColRate').GetValue() != null) ? parseFloat(InsgridBatch.GetEditor('gvColRate').GetValue()) : "0";
            InsgridBatch.GetEditor('gvColValue').SetValue(Quantity * Rate);
        }
        function txtBillNo_TextChanged() {
            var VoucherNo = document.getElementById("txtVoucherNo").value;

            $.ajax({
                type: "POST",
                url: "BranchRequisition.aspx/CheckUniqueName",
                data: JSON.stringify({ VoucherNo: VoucherNo }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var data = msg.d;

                    if (data == true) {
                        $("#MandatoryBillNo").show();

                        document.getElementById("txtVoucherNo").value = '';
                        document.getElementById("txtVoucherNo").focus();
                    }
                    else {
                        $("#MandatoryBillNo").hide();
                    }
                }
            });
        }
        function BtnVisible() {
            document.getElementById('btnSaveExit').style.display = 'none'
            document.getElementById('btnnew').style.display = 'none'
            document.getElementById('tagged').style.display = 'block'

        }
        function AddNewRow() {
            InsgridBatch.AddNewRow();
            var noofvisiblerows = InsgridBatch.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
            var tbQuotation = InsgridBatch.GetEditor("SrlNo");
            tbQuotation.SetValue(noofvisiblerows);
        }
        function OnAddNewClick() {
            InsgridBatch.AddNewRow();
            var noofvisiblerows = InsgridBatch.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
            var i;
            var cnt = 1;
            for (i = -1 ; cnt <= noofvisiblerows ; i--) {
                var tbQuotation = InsgridBatch.GetEditor("SrlNo");
                tbQuotation.SetValue(cnt);
                cnt++;
            }

        }
        function OnEndCallback(s, e) {
            var pageStatus = document.getElementById('hdnPageStatus').value;
            if (InsgridBatch.cpComponent) {
                if (InsgridBatch.cpComponent == 'true') {
                    InsgridBatch.cpComponent = null;
                    OnAddNewClick();
                }
            }
            if (InsgridBatch.cpSaveSuccessOrFail == "nullQuantity") {

                InsgridBatch.AddNewRow();
                var noofvisiblerows = InsgridBatch.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
                var tbQuotation = InsgridBatch.GetEditor("SrlNo");
                tbQuotation.SetValue(noofvisiblerows);
                InsgridBatch.cpSaveSuccessOrFail = null;
                jAlert('Cannot save. Entered quantity must be greater then ZERO(0).');
            }
            else if (InsgridBatch.cpSaveSuccessOrFail == "duplicateProduct") {
                AddNewRow();
                InsgridBatch.cpSaveSuccessOrFail = null;
                jAlert('Can not Add Duplicate Product in the Purchase Return Request.');
                InsgridBatch.cpSaveSuccessOrFail = '';
            }
            else if (InsgridBatch.cpSaveSuccessOrFail == "outrange") {
                InsgridBatch.batchEditApi.StartEdit(0, 2);
                jAlert('Can Not Add More Quotation Number as Quotation Scheme Exausted.<br />Update The Scheme and Try Again');

            }
            else if (InsgridBatch.cpSaveSuccessOrFail == "duplicate") {
                InsgridBatch.batchEditApi.StartEdit(0, 2);
                jAlert('Can Not Save as Duplicate Quotation Numbe No. Found');

            }
            else if (InsgridBatch.cpSaveSuccessOrFail == "errorInsert") {
                InsgridBatch.batchEditApi.StartEdit(0, 2);
                jAlert('Please try after sometime.');

            }
            else {
                if (InsgridBatch.cpVouvherNo != null) {
                    var JV_Number = InsgridBatch.cpVouvherNo;

                    var value = document.getElementById('hdnRefreshType').value;

                    var JV_Msg = "Purchase Return Request No. " + JV_Number + " generated.";
                    var strSchemaType = document.getElementById('hdnSchemaType').value;

                    if (value == "E") {

                        if (JV_Number != "") {
                            // if (strSchemaType == '1') {

                            jAlert(JV_Msg, 'Alert Dialog: [PurchaseReturnRequest]', function (r) {
                                if (r == true) {
                                    InsgridBatch.cpVouvherNo = null;
                                    window.location.assign("BranchRequisitionReturnList.aspx");
                                }
                            });

                            //}
                            //else {
                            //    window.location.assign("BranchRequisitionReturnList.aspx");
                            //}
                        }
                        else {
                            window.location.assign("BranchRequisitionReturnList.aspx");
                        }
                    }
                    else if (value == "S") {

                        if (JV_Number != "") {
                            //if (strSchemaType == '1') {
                            jAlert(JV_Msg, 'Alert Dialog: [PurchaseReturnRequest]', function (r) {
                                if (r == true) {
                                    InsgridBatch.cpVouvherNo = null;
                                    window.location.assign("BranchRequisitionReturn.aspx?key=ADD");
                                }
                            });

                            //}
                        }
                        else {
                            window.location.assign("BranchRequisitionReturn.aspx?key=ADD");
                        }
                    }
                }
                else {
                    if (pageStatus == "first") {
                        if (InsgridBatch.GetVisibleRowsOnPage() == 0) {
                            OnAddNewClick();
                        }
                        InsgridBatch.batchEditApi.EndEdit();
                        $('#<%=hdnPageStatus.ClientID %>').val('');
                    }
                    else if (pageStatus == "update") {

                        InsgridBatch.StartEditRow(0);
                        $('#<%=hdnPageStatus.ClientID %>').val('');


                    }
            }
        }
}

function OnCustomButtonClick(s, e) {

    if (e.buttonID == 'CustomDeleteIDS') {

        if (InsgridBatch.GetVisibleRowsOnPage() > 1) {
            var tbQuotation = InsgridBatch.GetEditor("SrlNo");
            InsgridBatch.batchEditApi.EndEdit();
            InsgridBatch.DeleteRow(e.visibleIndex);
            $('#<%=hdfIsDelete.ClientID %>').val('D');

            InsgridBatch.UpdateEdit();
            InsgridBatch.PerformCallback('Display');

            var noofvisiblerows = InsgridBatch.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc                  
            tbQuotation.SetValue(noofvisiblerows);
        }

    }
    if (e.buttonID == 'CustomAddNewRow') {

        InsgridBatch.batchEditApi.StartEdit(e.visibleIndex, 2);
        var Product = (InsgridBatch.GetEditor('gvColProduct').GetValue() != null) ? InsgridBatch.GetEditor('gvColProduct').GetValue() : "";
        var Quantity = (InsgridBatch.GetEditor('gvColQuantity').GetValue() != null) ? InsgridBatch.GetEditor('gvColQuantity').GetValue() : "0.0";
        var SpliteDetails = Product.split("||@||");
        var IsComponentProduct = SpliteDetails[16];
        var ComponentProduct = SpliteDetails[17];
        if (Product != "" && Quantity != "0.0") {
            if (IsComponentProduct == "Y") {
                var messege = "Selected product is defined with components.<br/> Would you like to proceed with components (" + ComponentProduct + ") ?";
                jConfirm(messege, 'Confirmation Dialog', function (r) {
                    if (r == true) {
                        //InsgridBatch.AddNewRow();
                        //var noofvisiblerows = InsgridBatch.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
                        //var tbQuotation = InsgridBatch.GetEditor("SrlNo");
                        //tbQuotation.SetValue(noofvisiblerows);

                        //setTimeout(function () {
                        //    InsgridBatch.batchEditApi.StartEdit(globalRowIndex, 2);
                        //}, 500);
                        //return false;
                        InsgridBatch.batchEditApi.StartEdit(globalRowIndex);
                        var IsComponentProduct = InsgridBatch.GetEditor("IsComponentProduct");
                        IsComponentProduct.SetValue("Y");
                        $('#<%=hdfIsDelete.ClientID %>').val('C');

                        InsgridBatch.UpdateEdit();
                        InsgridBatch.PerformCallback('Display~fromComponent');
                    }
                    else {
                        OnAddNewClick();
                    }
                });
            }
            else {
                OnAddNewClick();
                setTimeout(function () {
                    InsgridBatch.batchEditApi.StartEdit(globalRowIndex, 2);
                }, 500);
                return false;
            }


        }
    }
    else if (e.buttonID == 'CustomWarehouse') {
        var index = e.visibleIndex;
        InsgridBatch.batchEditApi.StartEdit(index, 2)
        // var inventoryType = (document.getElementById("ddlInventory").value != null) ? document.getElementById("ddlInventory").value : "";

        // if (inventoryType == "C" || inventoryType == "Y") {
        Warehouseindex = index;

        var SrlNo = (InsgridBatch.GetEditor('SrlNo').GetValue() != null) ? InsgridBatch.GetEditor('SrlNo').GetValue() : "";
        var ProductID = (InsgridBatch.GetEditor('gvColProduct').GetText() != null) ? InsgridBatch.GetEditor('gvColProduct').GetText() : "0";
        var QuantityValue = (InsgridBatch.GetEditor('gvColQuantity').GetValue() != null) ? InsgridBatch.GetEditor('gvColQuantity').GetValue() : "0";
        //var StkQuantityValue = (grid.GetEditor('StockQuantity').GetValue() != null) ? grid.GetEditor('StockQuantity').GetValue() : "0";

        $("#spnCmbWarehouse").hide();
        $("#spnCmbBatch").hide();
        $("#spncheckComboBox").hide();
        $("#spntxtQuantity").hide();

        if (ProductID != "" && parseFloat(QuantityValue) != 0) {
            var SpliteDetails = ProductID.split("||@||");
            var strProductID = SpliteDetails[0];
            var strDescription = SpliteDetails[1];
            var strUOM = SpliteDetails[2];
            var strStkUOM = SpliteDetails[4];
            var strMultiplier = SpliteDetails[7];
            var strProductName = strDescription;
            //var strProductName = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "";
            var StkQuantityValue = QuantityValue * strMultiplier;
            var Ptype = SpliteDetails[8];
            $('#<%=hdfProductType.ClientID %>').val(Ptype);

            document.getElementById('<%=lblProductName.ClientID %>').innerHTML = strProductName;
            document.getElementById('<%=txt_SalesAmount.ClientID %>').innerHTML = QuantityValue;
            document.getElementById('<%=txt_SalesUOM.ClientID %>').innerHTML = strUOM;
            document.getElementById('<%=txt_StockAmount.ClientID %>').innerHTML = StkQuantityValue;
            document.getElementById('<%=txt_StockUOM.ClientID %>').innerHTML = strStkUOM;

            $('#<%=hdfProductID.ClientID %>').val(strProductID);
            $('#<%=hdfProductSerialID.ClientID %>').val(SrlNo);
            $('#<%=hdfProductSerialID.ClientID %>').val(SrlNo);
            $('#<%=hdnProductQuantity.ClientID %>').val(QuantityValue);
            //cacpAvailableStock.PerformCallback(strProductID);

            if (Ptype == "W") {
                div_Warehouse.style.display = 'block';
                div_Batch.style.display = 'none';
                div_Serial.style.display = 'none';
                div_Quantity.style.display = 'block';
                cCmbWarehouse.PerformCallback('BindWarehouse');
                cGrdWarehouse.PerformCallback('Display~' + SrlNo);
                $("#ADelete").css("display", "block");//Subhabrata
                SelectedWarehouseID = "0";
                cPopup_Warehouse.Show();
            }
            else if (Ptype == "B") {
                div_Warehouse.style.display = 'none';
                div_Batch.style.display = 'block';
                div_Serial.style.display = 'none';
                div_Quantity.style.display = 'block';
                cCmbBatch.PerformCallback('BindBatch~' + "0");
                cGrdWarehouse.PerformCallback('Display~' + SrlNo);
                $("#ADelete").css("display", "block");//Subhabrata
                SelectedWarehouseID = "0";
                cPopup_Warehouse.Show();
            }
            else if (Ptype == "S") {
                div_Warehouse.style.display = 'none';
                div_Batch.style.display = 'none';
                div_Serial.style.display = 'block';
                div_Quantity.style.display = 'none';
                checkListBox.PerformCallback('BindSerial~' + "0" + '~' + "0");
                cGrdWarehouse.PerformCallback('Display~' + SrlNo);
                $("#ADelete").css("display", "none");//Subhabrata
                SelectedWarehouseID = "0";
                cPopup_Warehouse.Show();
            }
            else if (Ptype == "WB") {
                div_Warehouse.style.display = 'block';
                div_Batch.style.display = 'block';
                div_Serial.style.display = 'none';
                div_Quantity.style.display = 'block';
                cCmbWarehouse.PerformCallback('BindWarehouse');
                cGrdWarehouse.PerformCallback('Display~' + SrlNo);
                $("#ADelete").css("display", "block");//Subhabrata
                SelectedWarehouseID = "0";
                cPopup_Warehouse.Show();
            }
            else if (Ptype == "WS") {
                div_Warehouse.style.display = 'block';
                div_Batch.style.display = 'none';
                div_Serial.style.display = 'block';
                div_Quantity.style.display = 'none';
                cCmbWarehouse.PerformCallback('BindWarehouse');
                cGrdWarehouse.PerformCallback('Display~' + SrlNo);
                $("#ADelete").css("display", "none");//Subhabrata
                SelectedWarehouseID = "0";
                cPopup_Warehouse.Show();
            }
            else if (Ptype == "WBS") {
                div_Warehouse.style.display = 'block';
                div_Batch.style.display = 'block';
                div_Serial.style.display = 'block';
                div_Quantity.style.display = 'none';
                cCmbWarehouse.PerformCallback('BindWarehouse');
                cGrdWarehouse.PerformCallback('Display~' + SrlNo);
                $("#ADelete").css("display", "none");//Subhabrata
                SelectedWarehouseID = "0";
                cPopup_Warehouse.Show();
            }
            else if (Ptype == "BS") {
                div_Warehouse.style.display = 'none';
                div_Batch.style.display = 'block';
                div_Serial.style.display = 'block';
                div_Quantity.style.display = 'none';
                cCmbBatch.PerformCallback('BindBatch~' + "0");
                cGrdWarehouse.PerformCallback('Display~' + SrlNo);
                $("#ADelete").css("display", "none");//Subhabrata
                SelectedWarehouseID = "0";
                cPopup_Warehouse.Show();
            }
            else {

                jAlert("No Warehouse or Batch or Serial is actived !");
            }
        }
        else if (ProductID != "" && parseFloat(QuantityValue) == 0) {


            jAlert("Please enter Quantity !");
        }
        //}
        //else {


        //    jAlert("You have selected Non-Inventory Item, so You cannot updated Stock.");
        //}
    }
}
//....Tab Index Change From Rate to Grid First Column......
$(document).ready(function () {


    $('#txtMemoPurpose_I').blur(function () {
        if (InsgridBatch.GetVisibleRowsOnPage() == 1) {
            InsgridBatch.batchEditApi.StartEdit(-1, 2);
        }
    })


});
//.....end..........
function Save_ButtonClick() {

    $('#<%=hdfIsDelete.ClientID %>').val('I');
    $('#<%=hdnRefreshType.ClientID %>').val('S');
    if (document.getElementById('<%= txtVoucherNo.ClientID %>').value == "") {
        $("#MandatoryBillNo").show();

        return false;
    }
    if (cddlBranchTo.GetValue() == null) {
        $("#MandatoryBranchTo").show();
        return false;
    }
    if (ctxtMemoPurpose.GetValue() == null) {
        $("#MandatoryRegion").show();
        return false;

    }
    var IsType = "";
    var frontRow = 0;
    var backRow = -1;

    for (var i = 0; i <= InsgridBatch.GetVisibleRowsOnPage() ; i++) {
        var frontProduct = "";
        var backProduct = "";

        frontProduct = (InsgridBatch.batchEditApi.GetCellValue(backRow, 'ProductName') != null) ? (InsgridBatch.batchEditApi.GetCellValue(backRow, 'ProductName')) : "";
        backProduct = (InsgridBatch.batchEditApi.GetCellValue(frontRow, 'ProductName') != null) ? (InsgridBatch.batchEditApi.GetCellValue(frontRow, 'ProductName')) : "";
        if (frontProduct != "" || backProduct != "") {
            IsType = "Y";
            break;
        }
        backRow--;
        frontRow++;
    }
    if (InsgridBatch.GetVisibleRowsOnPage() > 0) {


        if (IsType == "Y") {
            InsgridBatch.AddNewRow();
            InsgridBatch.UpdateEdit();
        }
        else {
            jAlert('Cannot Save. You must enter atleast one Product to save this entry.');
        }
    }
    else {
        jAlert('Cannot Save. You must enter atleast one Product to save this entry.');

    }

}

function AddBatchNew() {
    InsgridBatch.batchEditApi.EndEdit();

    var noofvisiblerows = InsgridBatch.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc

    var i;
    var cnt = 1;
    if (noofvisiblerows == "0") {
        InsgridBatch.AddNewRow();
    }
    InsgridBatch.SetFocusedRowIndex();

    for (i = -1 ; cnt <= noofvisiblerows ; i--) {
        cnt++;
    }

    var tbQuotation = InsgridBatch.GetEditor("SrlNo");
    //console.log(tbQuotation);
    tbQuotation.SetValue(cnt);

}
function ProductsGotFocus(s, e) {
    document.getElementById("pageheaderContent").style.display = 'block';
    document.getElementById("liToBranch").style.display = 'block';
    var tbDescription = InsgridBatch.GetEditor("gvColDiscription");
    var tbUOM = InsgridBatch.GetEditor("gvColUOM");


    var ProductID = (InsgridBatch.GetEditor('gvColProduct').GetValue() != null) ? InsgridBatch.GetEditor('gvColProduct').GetValue() : "0";
    var SpliteDetails = ProductID.split("||@||");
    var strProductID = SpliteDetails[0];
    var strDescription = SpliteDetails[1];
    var strUOM = SpliteDetails[2];

    chkAccount = 1;
    tbDescription.SetValue(strDescription);
    tbUOM.SetValue(strUOM);

    $('#<%= lblStkQty.ClientID %>').text("0.00");
    $('#<%= lblStkUOM.ClientID %>').text(strUOM);
    $('#<%= lblStkUOMTo.ClientID %>').text(strUOM);
    if (ProductID != "0") {
        cacpAvailableStock.PerformCallback(strProductID);
    }
}
function ProductsGotFocusFromID(s, e) {
    pageheaderContent.style.display = "block";
    document.getElementById("liToBranch").style.display = 'block';

    var ProductID = (InsgridBatch.GetEditor('gvColProduct').GetValue() != null) ? InsgridBatch.GetEditor('gvColProduct').GetValue() : "0";
    var strProductName = (InsgridBatch.GetEditor('gvColProduct').GetText() != null) ? InsgridBatch.GetEditor('gvColProduct').GetText() : "0";


    var QuantityValue = (InsgridBatch.GetEditor('gvColQuantity').GetValue() != null) ? InsgridBatch.GetEditor('gvColQuantity').GetValue() : "0";
    var tbDescription = InsgridBatch.GetEditor("gvColDiscription");
    var tbUOM = InsgridBatch.GetEditor("gvColUOM");


    var SpliteDetails = ProductID.split("||@||");
    var strProductID = SpliteDetails[0];
    var strDescription = SpliteDetails[1];
    var strUOM = SpliteDetails[2];
    var strUOMstk = SpliteDetails[4];


    chkAccount = 1;
    tbDescription.SetValue(strDescription);
    tbUOM.SetValue(strUOM);

    $('#<%= lblStkQty.ClientID %>').text("0.00");
            $('#<%= lblStkUOM.ClientID %>').text(strUOM);
            $('#<%= lblStkUOMTo.ClientID %>').text(strUOM);

            if (ProductID != "0") {
                cacpAvailableStock.PerformCallback(strProductID);
            }

        }
        function acpAvailableStockEndCall(s, e) {
            if (cacpAvailableStock.cpstock != null) {
                var AvailableStock = cacpAvailableStock.cpstock + " " + document.getElementById('<%=lblStkUOM.ClientID %>').innerHTML;
            $('#<%=B_AvailableStock.ClientID %>').text(AvailableStock);
            cacpAvailableStock.cpstock = null;
        }
        if (cacpAvailableStock.cpstockBranchTo != null) {
            document.getElementById("liToBranch").style.display = 'block';
            var AvailableStock = cacpAvailableStock.cpstockBranchTo + " " + document.getElementById('<%=lblStkUOMTo.ClientID %>').innerHTML;
                    $('#<%=B_AvailableStockToBranch.ClientID %>').text(AvailableStock);
                    cacpAvailableStock.cpstockBranchTo = null;
                }
                if (preColumn == "Product") {
                    InsgridBatch.batchEditApi.StartEdit(globalRowIndex, 5);
                    preColumn = '';
                    return;
                }


            }
            function ProductsComboGotFocusChange(s, e) {

                var tbDescription = InsgridBatch.GetEditor("gvColDiscription");
                var tbUOM = InsgridBatch.GetEditor("gvColUOM");
                var tdRate = InsgridBatch.GetEditor("gvColRate");
                var AvailableStock = InsgridBatch.GetEditor("gvColAvailableStock");
                var ProductID = (InsgridBatch.GetEditor('gvColProduct').GetValue() != null) ? InsgridBatch.GetEditor('gvColProduct').GetValue() : "0";


                var SpliteDetails = ProductID.split("||@||");
                var strProductID = SpliteDetails[0];
                var strDescription = SpliteDetails[1];
                var strUOM = SpliteDetails[2];
                var strRate = SpliteDetails[6];
                chkAccount = 1;
                tbDescription.SetValue(strDescription);
                tbUOM.SetValue(strUOM);
                tdRate.SetValue(strRate);
                var Campany_ID = '<%=Session["LastCompany"]%>';
                    var LastFinYear = '<%=Session["LastFinYear"]%>';
                    var BranchFor = $("#ddlBranch").val();
                    if (ProductID != "0" && ProductID != "") {

                        $.ajax({
                            type: "POST",
                            url: 'BranchRequisition.aspx/getAvilableStock',
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            data: JSON.stringify({ Campany_ID: Campany_ID, ProductID: strProductID, LastFinYear: LastFinYear, BranchFor: BranchFor }),
                            success: function (msg) {
                                var data = msg.d;

                                document.getElementById("pageheaderContent").style.display = 'block';


                                var AvailableStock = data + " " + strUOM;
                                $('#<%=B_AvailableStock.ClientID %>').text(AvailableStock);

                            }
                        });
                        var BranchTo = $("#ddlBranchTo").val();

                        if (BranchTo != "0" && ProductID != "") {

                            $.ajax({
                                type: "POST",
                                url: 'BranchRequisition.aspx/getAvilableStock',
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",
                                data: JSON.stringify({ Campany_ID: Campany_ID, ProductID: strProductID, LastFinYear: LastFinYear, BranchFor: BranchTo }),
                                success: function (msg) {
                                    var data = msg.d;

                                    document.getElementById("liToBranch").style.display = 'block';


                                    var AvailableStock = data + " " + strUOM;
                                    $('#<%=B_AvailableStockToBranch.ClientID %>').text(AvailableStock);

                                }
                            });
                        }

                    }

                }
                function ProductsCombo_SelectedIndexChanged(s, e) {
                    var tbDescription = InsgridBatch.GetEditor("gvColDiscription");
                    var tbUOM = InsgridBatch.GetEditor("gvColUOM");
                    var tdRate = InsgridBatch.GetEditor("gvColRate");
                    var AvailableStock = InsgridBatch.GetEditor("gvColAvailableStock");
                    var ProductID = s.GetValue();
                    var SpliteDetails = ProductID.split("||@||");
                    var strProductID = SpliteDetails[0];
                    var strDescription = SpliteDetails[1];
                    var strUOM = SpliteDetails[2];
                    var strRate = SpliteDetails[6];
                    chkAccount = 1;
                    tbDescription.SetValue(strDescription);
                    tbUOM.SetValue(strUOM);
                    tdRate.SetValue(strRate);
                    var Campany_ID = '<%=Session["LastCompany"]%>';
                    var LastFinYear = '<%=Session["LastFinYear"]%>';
                    var BranchFor = $("#ddlBranch").val();
                    $.ajax({
                        type: "POST",
                        url: 'BranchRequisition.aspx/getAvilableStock',
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        data: JSON.stringify({ Campany_ID: Campany_ID, ProductID: strProductID, LastFinYear: LastFinYear, BranchFor: BranchFor }),
                        success: function (msg) {
                            var data = msg.d;

                            document.getElementById("pageheaderContent").style.display = 'block';

                            var AvailableStock = data + " " + strUOM;
                            $('#<%=B_AvailableStock.ClientID %>').text(AvailableStock);

                        }
                    });
                    var BranchTo = $("#ddlBranchTo").val();

                    if (BranchTo != "0") {
                        $.ajax({
                            type: "POST",
                            url: 'BranchRequisition.aspx/getAvilableStock',
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            data: JSON.stringify({ Campany_ID: Campany_ID, ProductID: strProductID, LastFinYear: LastFinYear, BranchFor: BranchTo }),
                            success: function (msg) {
                                var data = msg.d;

                                document.getElementById("pageheaderAvToBranch").style.display = 'block';

                                var AvailableStock = data + " " + strUOM;
                                $('#<%=B_AvailableStockToBranch.ClientID %>').text(AvailableStock);

                            }
                        });
                    }

                }
                function AddButtonClick() {
                    $('#<%=hdn_Mode.ClientID %>').val('Entry'); //Entry
                    <%--    $('#<%=Keyval_internalId.ClientID %>').val('Add');--%>
                    cCmbScheme.SetValue("0");

                    document.getElementById('DivEntry').style.display = 'block';
                    document.getElementById('DivEdit').style.display = 'none';
                    document.getElementById('btnAddNew').style.display = 'none';
                    btncross.style.display = "block";

                    document.getElementById('<%= txtVoucherNo.ClientID %>').disabled = true;

                    document.getElementById('<%= ddlBranch.ClientID %>').disabled = true;

                    deleteAllRows();
                    InsgridBatch.AddNewRow();
                    var noofvisiblerows = InsgridBatch.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
                    var tbQuotation = InsgridBatch.GetEditor("SrlNo");
                    tbQuotation.SetValue(noofvisiblerows);
                    cCmbScheme.Focus();
                    ddlBranchFor_SelectedIndexChanged();

                }
        function CmbScheme_ValueChange() {
            if (cCmbScheme.GetValue() != null) {
                var schemetypeValue = cCmbScheme.GetValue();

                var schemeID = schemetypeValue.toString().split('~')[0];
                var schemetype = schemetypeValue.toString().split('~')[1];
                var schemelength = schemetypeValue.toString().split('~')[2];
                var branchID = schemetypeValue.toString().split('~')[3];
                var Type = schemetypeValue.toString().split('~')[4];

                var fromdate = schemetypeValue.toString().split('~')[4];
                var todate = schemetypeValue.toString().split('~')[5];

                var dt = new Date();

                ctDate.SetDate(dt);

                if (dt < new Date(fromdate)) {
                    ctDate.SetDate(new Date(fromdate));
                }

                if (dt > new Date(todate)) {
                    ctDate.SetDate(new Date(todate));
                }




                ctDate.SetMinDate(new Date(fromdate));
                ctDate.SetMaxDate(new Date(todate));

                $('#txtVoucherNo').attr('maxLength', schemelength);

                if (schemetypeValue != "") {
                    document.getElementById('ddlBranch').value = branchID;
                    document.getElementById('<%= ddlBranch.ClientID %>').disabled = true;
                    cddlBranchTo.PerformCallback(branchID);

                }
                if (schemetype == '0') {
                    $('#<%=hdnSchemaType.ClientID %>').val('0');
                    document.getElementById('<%= txtVoucherNo.ClientID %>').disabled = false;
                    document.getElementById('<%= txtVoucherNo.ClientID %>').value = "";
                    $('#<%=txtVoucherNo.ClientID %>').focus();
                }
                else if (schemetype == '1') {
                    $('#<%=hdnSchemaType.ClientID %>').val('1');
                    document.getElementById('<%= txtVoucherNo.ClientID %>').disabled = true;
                    document.getElementById('<%= txtVoucherNo.ClientID %>').value = "Auto";
                    $("#MandatoryBillNo").hide();
                    ctDate.Focus();
                }
                else if (schemetype == '2') {
                    $('#<%=hdnSchemaType.ClientID %>').val('2');
                    document.getElementById('<%= txtVoucherNo.ClientID %>').disabled = true;
                    document.getElementById('<%= txtVoucherNo.ClientID %>').value = "Datewise";
                }
                else if (schemetype == 'n') {
                    document.getElementById('<%= txtVoucherNo.ClientID %>').disabled = true;
                    document.getElementById('<%= txtVoucherNo.ClientID %>').value = "";
                }
                <%--$.ajax({
                        type: "POST",
                        url: 'BranchRequisition.aspx/getSchemeType',
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        data: "{sel_scheme_id:\"" + val + "\"}",
                        success: function (type) {
                            var schemetypeValue = type.d;
                            var schemetype = schemetypeValue.toString().split('~')[0];
                            var schemelength = schemetypeValue.toString().split('~')[1];
                            $('#txtVoucherNo').attr('maxLength', schemelength);
                            var branchID = schemetypeValue.toString().split('~')[2];
                            if (schemetypeValue != "") {
                                document.getElementById('ddlBranch').value = branchID;
                                document.getElementById('<%= ddlBranch.ClientID %>').disabled = true;
                                cddlBranchTo.PerformCallback(branchID);

                            }
                            if (schemetype == '0') {
                                $('#<%=hdnSchemaType.ClientID %>').val('0');
                                document.getElementById('<%= txtVoucherNo.ClientID %>').disabled = false;
                                document.getElementById('<%= txtVoucherNo.ClientID %>').value = "";
                                $('#<%=txtVoucherNo.ClientID %>').focus();
                            }
                            else if (schemetype == '1') {
                                $('#<%=hdnSchemaType.ClientID %>').val('1');
                                document.getElementById('<%= txtVoucherNo.ClientID %>').disabled = true;
                                document.getElementById('<%= txtVoucherNo.ClientID %>').value = "Auto";
                                $("#MandatoryBillNo").hide();
                                ctDate.Focus();
                            }
                            else if (schemetype == '2') {
                                $('#<%=hdnSchemaType.ClientID %>').val('2');
                                document.getElementById('<%= txtVoucherNo.ClientID %>').disabled = true;
                                document.getElementById('<%= txtVoucherNo.ClientID %>').value = "Datewise";
                            }
                            else if (schemetype == 'n') {
                                document.getElementById('<%= txtVoucherNo.ClientID %>').disabled = true;
                                document.getElementById('<%= txtVoucherNo.ClientID %>').value = "";
                            }
                        }
                    });--%>
            }

        }


    </script>
    <%--Party  Popup Start--%>
    <script type="text/javascript">
    
        function VendorButnClick(s, e) {
            var txt = "<table border='1' width=\"100%\" class=\"dynamicPopupTbl\"><tr class=\"HeaderStyle\"><th>Party Name</th><th>Unique Id</th><th>Type</th></tr><table>";
            document.getElementById("CustomerTable").innerHTML = txt;
            setTimeout(function () { $("#txtPartySearch").focus(); }, 500);
            $('#txtPartySearch').val('');
            $('#PartyModel').modal('show');
        }
        function VendorKeyDown(s, e) {
            if (e.htmlEvent.key == "Enter" || e.code == "NumpadEnter") {
                s.OnButtonClick(0);
            }
        }
        function VendorModekkeydown(e) {
            var OtherDetails = {}
            OtherDetails.SearchKey = $("#txtPartySearch").val();           
            if (e.code == "Enter" || e.code == "NumpadEnter") {
                var HeaderCaption = [];
                HeaderCaption.push("Party Name");
                HeaderCaption.push("Unique Id");
                HeaderCaption.push("Type");
                if ($("#txtPartySearch").val() != "") {
                    callonServer("Services/Master.asmx/GetParty", OtherDetails, "CustomerTable", HeaderCaption, "customerIndex", "SetCustomer");
                }

            }
            else if (e.code == "ArrowDown") {
                if ($("input[customerindex=0]"))
                    $("input[customerindex=0]").focus();
            }
        }
        function SetCustomer(Id, Name) {
            if (Id) {
                $('#PartyModel').modal('hide');
                ctxtVendorName.SetText(Name);

                GetObjectID('hdnPartyId').value = Id;
                $('#MandatorysVendor').attr('style', 'display:none');
                var VendorId = Id;             
                $('#PartyModel').modal('hide');
                $("#txtPartyInvoiceNo").focus()
                
            }
        }

        function ValueSelected(e, indexName) {
            if (e.code == "Enter" || e.code == "NumpadEnter") {
                var Id = e.target.parentElement.parentElement.cells[0].innerText;
                var name = e.target.parentElement.parentElement.cells[1].children[0].value;
                if (Id) {                    
                        SetCustomer(Id, name);
                }

            }

            else if (e.code == "ArrowDown") {
                thisindex = parseFloat(e.target.getAttribute(indexName));
                thisindex++;
                if (thisindex < 10)
                    $("input[" + indexName + "=" + thisindex + "]").focus();
            }
            else if (e.code == "ArrowUp") {
                thisindex = parseFloat(e.target.getAttribute(indexName));
                thisindex--;
                if (thisindex > -1)
                    $("input[" + indexName + "=" + thisindex + "]").focus();
                else {                    
                        $('#txtPartySearch').focus();
                }
            }

        }
    </script>
    <%--Party  Popup End--%>
    <style>
        
    </style>
    <%--Warehouse Popup Start--%>

    <script>
        var SelectWarehouse = "0";
        var SelectBatch = "0";
        var SelectSerial = "0";
        var SelectedWarehouseID = "0";
        var IsPostBack = "";
        var PBWarehouseID = "";
        var PBBatchID = "";
        var textSeparator = ";";
        var selectedChkValue = "";
        function AutoCalculateMandateOnChange(element) {
            $("#spnCmbWarehouse").hide();
            $("#spnCmbBatch").hide();
            $("#spncheckComboBox").hide();
            $("#spntxtQuantity").hide();

            if (document.getElementById("myCheck").checked == true) {
                divSingleCombo.style.display = "block";
                divMultipleCombo.style.display = "none";

                checkComboBox.Focus();
            }
            else {
                divSingleCombo.style.display = "none";
                divMultipleCombo.style.display = "block";

                ctxtserial.Focus();
            }
        }
        function fn_Deletecity(keyValue) {
            var WarehouseID = (cCmbWarehouse.GetValue() != null) ? cCmbWarehouse.GetValue() : "0";
            var BatchID = (cCmbBatch.GetValue() != null) ? cCmbBatch.GetValue() : "0";
            cGrdWarehouse.PerformCallback('Delete~' + keyValue);
            checkListBox.PerformCallback('BindSerial~' + WarehouseID + '~' + BatchID);
        }
        function fn_Edit(keyValue) {
            //cGrdWarehouse.PerformCallback('EditWarehouse~' + keyValue);
            SelectedWarehouseID = keyValue;
            cCallbackPanel.PerformCallback('EditWarehouse~' + keyValue);
        }
        function GetSelectedItemsText(items) {
            var texts = [];
            for (var i = 0; i < items.length; i++)
                if (items[i].index != 0)
                    texts.push(items[i].text);
            return texts.join(textSeparator);
        }
        function GetValuesByTexts(texts) {
            var actualValues = [];
            var item;
            for (var i = 0; i < texts.length; i++) {
                item = checkListBox.FindItemByText(texts[i]);
                if (item != null)
                    actualValues.push(item.value);
            }
            return actualValues;
        }
        function UpdateSelectAllItemState() {
            IsAllSelected() ? checkListBox.SelectIndices([0]) : checkListBox.UnselectIndices([0]);
        }
        function IsAllSelected() {
            var selectedDataItemCount = checkListBox.GetItemCount() - (checkListBox.GetItem(0).selected ? 0 : 1);
            return checkListBox.GetSelectedItems().length == selectedDataItemCount;
        }
        function UpdateText() {
            var selectedItems = checkListBox.GetSelectedItems();
            selectedChkValue = GetSelectedItemsText(selectedItems);
            //checkComboBox.SetText(GetSelectedItemsText(selectedItems));
            checkComboBox.SetText(selectedItems.length + " Items");

            var val = GetSelectedItemsText(selectedItems);
            $("#abpl").attr('data-content', val);
        }
        function CmbWarehouse_ValueChange() {
            var WarehouseID = cCmbWarehouse.GetValue();
            var type = document.getElementById('hdfProductType').value;

            if (type == "WBS" || type == "WB") {
                cCmbBatch.PerformCallback('BindBatch~' + WarehouseID);
            }
            else if (type == "WS") {
                checkListBox.PerformCallback('BindSerial~' + WarehouseID + '~' + "0");
            }
        }
        function CmbBatch_ValueChange() {
            var WarehouseID = cCmbWarehouse.GetValue();
            var BatchID = cCmbBatch.GetValue();
            var type = document.getElementById('hdfProductType').value;

            if (type == "WBS") {
                checkListBox.PerformCallback('BindSerial~' + WarehouseID + '~' + BatchID);
            }
            else if (type == "BS") {
                checkListBox.PerformCallback('BindSerial~' + "0" + '~' + BatchID);
            }
        }
        function SaveWarehouse() {
            var WarehouseID = (cCmbWarehouse.GetValue() != null) ? cCmbWarehouse.GetValue() : "0";
            var WarehouseName = cCmbWarehouse.GetText();
            var BatchID = (cCmbBatch.GetValue() != null) ? cCmbBatch.GetValue() : "0";
            var BatchName = cCmbBatch.GetText();
            var SerialID = "";
            var SerialName = "";
            var Qty = ctxtQuantity.GetValue();

            var items = checkListBox.GetSelectedItems();
            var vals = [];
            var texts = [];

            for (var i = 0; i < items.length; i++) {
                if (items[i].index != 0) {
                    if (i == 0) {
                        SerialID = items[i].value;
                        SerialName = items[i].text;
                    }
                    else {
                        if (SerialID == "" && SerialID == "") {
                            SerialID = items[i].value;
                            SerialName = items[i].text;
                        }
                        else {
                            SerialID = SerialID + '||@||' + items[i].value;
                            SerialName = SerialName + '||@||' + items[i].text;
                        }
                    }

                }
            }


            $("#spnCmbWarehouse").hide();
            $("#spnCmbBatch").hide();
            $("#spncheckComboBox").hide();
            $("#spntxtQuantity").hide();

            var Ptype = document.getElementById('hdfProductType').value;
            if ((Ptype == "W" && WarehouseID == "0") || (Ptype == "WB" && WarehouseID == "0") || (Ptype == "WS" && WarehouseID == "0") || (Ptype == "WBS" && WarehouseID == "0")) {
                $("#spnCmbWarehouse").show();
            }
            else if ((Ptype == "B" && BatchID == "0") || (Ptype == "WB" && BatchID == "0") || (Ptype == "WBS" && BatchID == "0") || (Ptype == "BS" && BatchID == "0")) {
                $("#spnCmbBatch").show();
            }
            else if ((Ptype == "W" && Qty == "0.0") || (Ptype == "B" && Qty == "0.0") || (Ptype == "WB" && Qty == "0.0")) {
                $("#spntxtQuantity").show();
            }
            else if ((Ptype == "S" && SerialID == "") || (Ptype == "WS" && SerialID == "") || (Ptype == "WBS" && SerialID == "") || (Ptype == "BS" && SerialID == "")) {
                $("#spncheckComboBox").show();
            }
            else {
                if (document.getElementById("myCheck").checked == true && SelectedWarehouseID == "0") {
                    if (Ptype == "W" || Ptype == "WB" || Ptype == "B") {
                        cCmbWarehouse.PerformCallback('BindWarehouse');
                        cCmbBatch.PerformCallback('BindBatch~' + "");
                        checkListBox.PerformCallback('BindSerial~' + "" + '~' + "");
                        ctxtQuantity.SetValue("0");
                    }
                    else {
                        IsPostBack = "N";
                        PBWarehouseID = WarehouseID;
                        PBBatchID = BatchID;
                    }
                }
                else {
                    cCmbWarehouse.PerformCallback('BindWarehouse');
                    cCmbBatch.PerformCallback('BindBatch~' + "");
                    checkListBox.PerformCallback('BindSerial~' + "" + '~' + "");
                    ctxtQuantity.SetValue("0");
                }
                UpdateText();
                cGrdWarehouse.PerformCallback('SaveDisplay~' + WarehouseID + '~' + WarehouseName + '~' + BatchID + '~' + BatchName + '~' + SerialID + '~' + SerialName + '~' + Qty + '~' + SelectedWarehouseID);
                SelectedWarehouseID = "0";
            }
        }


        function FinalWarehouse() {
            cGrdWarehouse.PerformCallback('WarehouseFinal');
        }

        function closeWarehouse(s, e) {
            e.cancel = false;
            cGrdWarehouse.PerformCallback('WarehouseDelete');
            $('#abpl').popover('hide');//Subhabrata
        }
        function OnWarehouseEndCallback(s, e) {
            var Ptype = document.getElementById('hdfProductType').value;

            if (cGrdWarehouse.cpIsSave == "Y") {
                cPopup_Warehouse.Hide();
                InsgridBatch.batchEditApi.StartEdit(Warehouseindex, 5);
            }
            else if (cGrdWarehouse.cpIsSave == "N") {
                jAlert('Sales Quantity must be equal to Warehouse Quantity.');
            }
            else {
                if (document.getElementById("myCheck").checked == true) {
                    if (IsPostBack == "N") {
                        checkListBox.PerformCallback('BindSerial~' + PBWarehouseID + '~' + PBBatchID);

                        IsPostBack = "";
                        PBWarehouseID = "";
                        PBBatchID = "";
                    }

                    if (Ptype == "W" || Ptype == "WB") {
                        cCmbWarehouse.Focus();
                    }
                    else if (Ptype == "B") {
                        cCmbBatch.Focus();
                    }
                    else {
                        ctxtserial.Focus();
                    }
                }
                else {
                    if (Ptype == "W" || Ptype == "WB" || Ptype == "WS" || Ptype == "WBS") {
                        cCmbWarehouse.Focus();
                    }
                    else if (Ptype == "B" || Ptype == "BS") {
                        cCmbBatch.Focus();
                    }
                    else if (Ptype == "S") {
                        checkComboBox.Focus();
                    }
                }
            }
        }
        function CallbackPanelEndCall(s, e) {
            if (cCallbackPanel.cpEdit != null) {
                var strWarehouse = cCallbackPanel.cpEdit.split('~')[0];
                var strBatchID = cCallbackPanel.cpEdit.split('~')[1];
                var strSrlID = cCallbackPanel.cpEdit.split('~')[2];
                var strQuantity = cCallbackPanel.cpEdit.split('~')[3];

                SelectWarehouse = strWarehouse;
                SelectBatch = strBatchID;
                SelectSerial = strSrlID;

                cCmbWarehouse.PerformCallback('BindWarehouse');
                cCmbBatch.PerformCallback('BindBatch~' + strWarehouse);
                checkListBox.PerformCallback('EditSerial~' + strWarehouse + '~' + strBatchID + '~' + strSrlID);

                cCmbWarehouse.SetValue(strWarehouse);
                ctxtQuantity.SetValue(strQuantity);
            }
        }
        function txtserialTextChanged() {
            checkListBox.UnselectAll();
            var SerialNo = (ctxtserial.GetValue() != null) ? (ctxtserial.GetValue()) : "0";

            if (SerialNo != "0") {
                ctxtserial.SetValue("");
                var texts = [SerialNo];
                var values = GetValuesByTexts(texts);

                if (values.length > 0) {
                    checkListBox.SelectValues(values);
                    // UpdateSelectAllItemState();
                    UpdateText(); // for remove non-existing texts
                    SaveWarehouse();
                }
                else {
                    jAlert("This Serial Number does not exists.");
                }
            }
        }
        function SynchronizeListBoxValues(dropDown, args) {
            checkListBox.UnselectAll();
            // var texts = dropDown.GetText().split(textSeparator);
            var texts = selectedChkValue.split(textSeparator);

            var values = GetValuesByTexts(texts);
            checkListBox.SelectValues(values);
            UpdateSelectAllItemState();
            UpdateText(); // for remove non-existing texts
        }
        function OnListBoxSelectionChanged(listBox, args) {
            if (args.index == 0)
                args.isSelected ? listBox.SelectAll() : listBox.UnselectAll();
            UpdateSelectAllItemState();
            UpdateText();

            //Added Subhabrata
            var selectedItems = checkListBox.GetSelectedItems();
            var val = GetSelectedItemsText(selectedItems);
            var strWarehouse = cCmbWarehouse.GetValue();
            var strBatchID = cCmbBatch.GetValue();
            var ProducttId = $("#hdfProductID").val();


        }
        function CmbWarehouseEndCallback(s, e) {
            if (SelectWarehouse != "0") {
                cCmbWarehouse.SetValue(SelectWarehouse);
                SelectWarehouse = "0";
            }
            else {
                cCmbWarehouse.SetEnabled(true);
            }
        }

        function CmbBatchEndCall(s, e) {
            if (SelectBatch != "0") {
                cCmbBatch.SetValue(SelectBatch);
                SelectBatch = "0";
            }
            else {
                cCmbBatch.SetEnabled(true);
            }
        }

        function listBoxEndCall(s, e) {
            if (SelectSerial != "0") {
                var values = [SelectSerial];
                checkListBox.SelectValues(values);
                UpdateSelectAllItemState();
                UpdateText();
                //checkListBox.SetValue(SelectWarehouse);
                SelectSerial = "0";
                cCmbBatch.SetEnabled(false);
                cCmbWarehouse.SetEnabled(false);
            }
        }

    </script>
    <%--Warehouse Popup End--%>
    <%--Batch Product Popup Start--%>

    <script>
        function GetVendorExistsOrNot() {
            if (!cVendorComboBox.FindItemByValue(cVendorComboBox.GetValue())) {
                jAlert("Party Name not Exists.", "Alert", function () { cVendorComboBox.SetValue(); cVendorComboBox.Focus(); });
                return;
            }
        }
        function ProductKeyDown(s, e) {
            //console.log(e.htmlEvent.key);
            if (e.htmlEvent.key == "Enter") {

                s.OnButtonClick(0);
            }
            if (e.htmlEvent.key == "Tab") {

                s.OnButtonClick(0);
            }
        }

        function ProductButnClick(s, e) {
            if (e.buttonIndex == 0) {

                if (cproductLookUp.Clear()) {
                    cProductpopUp.Show();
                    cproductLookUp.Focus();
                    cproductLookUp.ShowDropDown();
                }
            }
        }
        function ProductlookUpKeyDown(s, e) {
            if (e.htmlEvent.key == "Escape") {
                cProductpopUp.Hide();
                InsgridBatch.batchEditApi.StartEdit(globalRowIndex, 5);
            }
        }
        function ProductSelected(s, e) {
            if (cproductLookUp.GetGridView().GetFocusedRowIndex() == -1) {
                cProductpopUp.Hide();
                InsgridBatch.batchEditApi.StartEdit(globalRowIndex, 5);
                return;
            }
            var LookUpData = cproductLookUp.GetGridView().GetRowKey(cproductLookUp.GetGridView().GetFocusedRowIndex());
            var ProductCode = cproductLookUp.GetValue();
            if (!ProductCode) {
                LookUpData = null;
            }
            else {
                chkAccount = 1;
            }
            cProductpopUp.Hide();
            InsgridBatch.batchEditApi.StartEdit(globalRowIndex);
            InsgridBatch.batchEditApi.StartEdit(globalRowIndex, 5);
            InsgridBatch.GetEditor("gvColProduct").SetText(LookUpData);
            InsgridBatch.GetEditor("ProductName").SetText(ProductCode);
            pageheaderContent.style.display = "block";
            var tbDescription = InsgridBatch.GetEditor("gvColDiscription");
            var tbUOM = InsgridBatch.GetEditor("gvColUOM");
            var ProductID = (InsgridBatch.GetEditor('gvColProduct').GetText() != null) ? InsgridBatch.GetEditor('gvColProduct').GetText() : "0";
            var SpliteDetails = ProductID.split("||@||");
            var strProductID = SpliteDetails[0];
            var strDescription = SpliteDetails[1];
            var strUOM = SpliteDetails[2];
            var strStkUOM = SpliteDetails[4];
            var strSalePrice = SpliteDetails[6];
            var QuantityValue = (InsgridBatch.GetEditor('gvColQuantity').GetValue() != null) ? InsgridBatch.GetEditor('gvColQuantity').GetValue() : "0";
            tbDescription.SetValue(strDescription);
            tbUOM.SetValue(strUOM);
            InsgridBatch.GetEditor("gvColQuantity").SetValue("0.00");
            $('#<%= lblStkQty.ClientID %>').text("0.00");
            $('#<%= lblStkUOM.ClientID %>').text(strUOM);
            $('#<%= lblStkUOMTo.ClientID %>').text(strUOM);
            preColumn = "Product";
            cacpAvailableStock.PerformCallback(strProductID);
            InsgridBatch.batchEditApi.StartEdit(globalRowIndex, 5);

        }
    </script>
    <link href="CSS/BranchRequisitionReturn.css" rel="stylesheet" />

    <%--Rev 1.0--%>
    <link href="/assests/css/custom/newcustomstyle.css" rel="stylesheet" />
    
    <style>
        select
        {
            z-index: 1;
        }

        /*#grid {
            max-width: 98% !important;
        }*/
        #FormDate , #toDate , #dtTDate , #dt_PLQuote , #dt_PLSales , #tDate
        {
            position: relative;
            z-index: 1;
            background: transparent;
        }

        #FormDate_B-1 , #toDate_B-1 , #dtTDate_B-1 , #dt_PLQuote_B-1 , #dt_PLSales_B-1 , #tDate_B-1
        {
            background: transparent !important;
            border: none;
            width: 30px;
            padding: 10px !important;
        }

        #FormDate_B-1 #FormDate_B-1Img , #toDate_B-1 #toDate_B-1Img , #dtTDate_B-1 #dtTDate_B-1Img , #dt_PLQuote_B-1 #dt_PLQuote_B-1Img ,
        #dt_PLSales_B-1 #dt_PLSales_B-1Img , #tDate_B-1 #tDate_B-1Img
        {
            display: none;
        }

        /*select
        {
            -webkit-appearance: auto;
        }*/

        .calendar-icon
        {
            right: 20px;
            bottom: -23px;
        }
        .padTabtype2 > tbody > tr > td
        {
            vertical-align: bottom;
        }
        #rdl_Salesquotation
        {
            margin-top: 0px;
        }

        .lblmTop8>span, .lblmTop8>label
        {
            margin-top: 0 !important;
        }

        .col-md-2, .col-md-4 {
            margin-bottom: 10px;
        }

        .simple-select::after
        {
            top: 6px;
            right: -2px;
        }

        .dxeErrorFrameWithoutError_PlasticBlue.dxeControlsCell_PlasticBlue
        {
            padding: 0;
        }

        .aspNetDisabled
        {
            background: #f3f3f3 !important;
        }

        .backSelect {
            background: #42b39e !important;
        }

        #ddlInventory
        {
                -webkit-appearance: auto;
        }

        /*.wid-90
        {
            width: 100%;
        }
        .dxtcLite_PlasticBlue.dxtc-top > .dxtc-content
        {
            width: 97%;
        }*/
        .newLbl
        {
                margin: 3px 0 !important;
        }

        .lblBot > span, .lblBot > label
        {
                margin-bottom: 3px !important;
        }

        .col-md-3>label, .col-md-3>span
        {
                margin-top: 0px;
        }

        #txtVendorName
        {
                width: 100%;
        }

    </style>
    <%--Rev end 1.0--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--Rev 1.0: "outer-div-main" class add --%>
    <div class="outer-div-main clearfix">
        <div class="panel-heading">
        <div class="panel-title clearfix">
            <h3 class="pull-left"><span class="">
                <asp:Label ID="lblHeading" runat="server" Text="Add Purchase Return Request"></asp:Label></span>

            </h3>
            <div id="pageheaderContent" class="pull-right wrapHolder reverse content horizontal-images" style="display: none;">
                <div class="Top clearfix">
                    <ul>
                        <li id="liToBranch" style="display: none;">
                            <div class="lblHolder" style="max-width: 350px">
                                <table>
                                    <tbody>
                                        <tr>
                                            <td>Available Balance of Request To Unit </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div style="width: 100%;">

                                                    <asp:Label ID="B_AvailableStockToBranch" runat="server" Text="0.0"></asp:Label>
                                                </div>

                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </li>
                        <li>
                            <div class="lblHolder" style="max-width: 350px">
                                <table>
                                    <tbody>
                                        <tr>
                                            <td>Available Balance of Request From Unit </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div style="width: 100%;">

                                                    <asp:Label ID="B_AvailableStock" runat="server" Text="0.0"></asp:Label>
                                                </div>

                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </li>
                        <li>
                            <div class="lblHolder" style="display: none;">
                                <table>
                                    <tr>
                                        <td>Stock Quantity</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblStkQty" runat="server" Text="0.00"></asp:Label>
                                            <asp:Label ID="lblStkUOM" runat="server" Text=" "></asp:Label>
                                            <asp:Label ID="lblStkUOMTo" runat="server" Text=" "></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </li>
                    </ul>
                </div>
            </div>


            <div id="btncross" runat="server" class="crossBtn" style="margin-left: 50px;"><a href="BranchRequisitionReturnList.aspx"><i class="fa fa-times"></i></a></div>


        </div>

    </div>
        <div id="DivEntry">
        <div style=" padding: 8px 0; margin-bottom: 15px; border-radius: 4px;" class="clearfix">
            <div class="col-md-2 lblmTop8" id="divNumberingScheme" runat="server">
                <label style="">Numbering Scheme</label>
                <div>
                    <dxe:ASPxComboBox ID="CmbScheme" EnableIncrementalFiltering="True" ClientInstanceName="cCmbScheme"
                        TextField="SchemaName" ValueField="ID" IncrementalFilteringMode="Contains"
                        runat="server" ValueType="System.String" Width="100%" EnableSynchronization="True">
                        <ClientSideEvents ValueChanged="function(s,e){CmbScheme_ValueChange()}"></ClientSideEvents>
                    </dxe:ASPxComboBox>
                    <span id="MandatoryNumberingScheme" class="iconNumberScheme pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>

                </div>
            </div>
            <div class="col-md-2 lblmTop8">
                <label>Document No.<span style="color: red;">*</span></label>
                <div>
                    <asp:TextBox ID="txtVoucherNo" runat="server" Width="100%" MaxLength="50" onchange="txtBillNo_TextChanged()">                             
                    </asp:TextBox>


                    <span id="MandatoryBillNo" class="voucherno  pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>

                </div>
            </div>
            <div class="col-md-2 lblmTop8">
                <label>Posting Date<span style="color: red;">*</span></label>
                <div>
                    <dxe:ASPxDateEdit ID="tDate" runat="server" EditFormat="Custom" ClientInstanceName="ctDate" DisplayFormatString="dd-MM-yyyy"
                        UseMaskBehavior="True" Width="100%" EditFormatString="dd-MM-yyyy">

                        <ClientSideEvents DateChanged="function(s,e){TDateChange();}" GotFocus="function(s,e){ctDate.ShowDropDown();}"></ClientSideEvents>
                        <ValidationSettings RequiredField-IsRequired="true" ErrorFrameStyle-CssClass="absolute"></ValidationSettings>
                    </dxe:ASPxDateEdit>
                    <%--Rev 1.0--%>
                    <img src="/assests/images/calendar-icon.png" class="calendar-icon"/>
                    <%--Rev end 1.0--%>
                </div>
            </div>
            <div class="col-md-3">

                <label>From Unit</label>
                <%--Rev 1.0: "simple-select" class add --%>
                <div class="simple-select">
                    <asp:DropDownList ID="ddlBranch" runat="server" DataSourceID="dsBranch" onchange="ddlBranchFor_SelectedIndexChanged()"
                        DataTextField="BANKBRANCH_NAME" DataValueField="BANKBRANCH_ID" Width="100%">
                    </asp:DropDownList>

                </div>
            </div>
            <div class="col-md-3">

                <label>Request To Unit<span style="color: red;">*</span></label>
                <div>

                    <dxe:ASPxComboBox ID="ddlBranchTo" runat="server" ClientIDMode="Static" ClientInstanceName="cddlBranchTo" Width="100%"
                        OnCallback="ddlBranchTo_Callback">
                        <ClientSideEvents SelectedIndexChanged="ChangeBranchTo" />
                    </dxe:ASPxComboBox>
                    <span id="MandatoryBranchTo" class="BranchTo  pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                </div>
            </div>
            <div style="clear: both"></div>
            <div class="col-md-2 lblmTop8">
                <label>Party Name</label>
                
                    <dxe:ASPxButtonEdit ID="txtVendorName" runat="server" ReadOnly="true" ClientInstanceName="ctxtVendorName">
                        <Buttons>
                            <dxe:EditButton>
                            </dxe:EditButton>
                        </Buttons>
                        <ClientSideEvents ButtonClick="function(s,e){VendorButnClick();}" KeyDown="function(s,e){VendorKeyDown(s,e);}" />
                    </dxe:ASPxButtonEdit>
                    <%-- <dxe:ASPxComboBox ID="VendorComboBox" runat="server" EnableCallbackMode="true" CallbackPageSize="10" Width="98%"                        
                        ValueType="System.String" ValueField="cnt_internalid" ClientInstanceName="cVendorComboBox"
                        OnItemsRequestedByFilterCondition="ASPxComboBox_OnItemsRequestedByFilterCondition_SQL" FilterMinLength="4"
                        OnItemRequestedByValue="ASPxComboBox_OnItemRequestedByValue_SQL" TextFormatString="{1} [{0}]"
                        DropDownStyle="DropDown" DropDownRows="7">
                        <Columns>
                            <dxe:ListBoxColumn FieldName="shortname" Caption="Short Name" Width="320px" />
                            <dxe:ListBoxColumn FieldName="Name" Caption="Name" Width="320px" />
                            <dxe:ListBoxColumn FieldName="Type" Caption="Type" Width="100px" />
                        </Columns>
                        <ClientSideEvents ValueChanged="function(s,e){$('#DeleteCustomer').val('yes'); GetVendorExistsOrNot(e)}" GotFocus="function(s,e){cVendorComboBox.ShowDropDown();}" />
                    </dxe:ASPxComboBox>--%>
               
            </div>
            <div class="col-md-2 lblmTop8">
                <label>Party Invoice No</label>
                <div>
                    <asp:TextBox ID="txtPartyInvoiceNo" runat="server" Width="100%" MaxLength="16">                             
                    </asp:TextBox>
                </div>
            </div>
            <div class="col-md-2 lblmTop8">
                <label>GRN No</label>
                <div>
                    <asp:TextBox ID="txtGRNNo" runat="server" Width="100%" MaxLength="16">                             
                    </asp:TextBox>
                </div>
            </div>
            <div style="clear: both"></div>
            <div class="col-md-8 lblmTop8">
                <label style="margin-bottom: 5px; display: inline-block">Reason<span style="color: red;">*</span></label>
                <div>
                    <dxe:ASPxMemo ID="txtMemoPurpose" ClientInstanceName="ctxtMemoPurpose" runat="server" Height="50px" Width="100%" Font-Names="Arial"></dxe:ASPxMemo>
                    <span id="MandatoryRegion" class="Region  pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>

                </div>
            </div>
            <div style="clear: both"></div>
            <div>
                <br />
            </div>

        </div>
        <div>
            <div>
                <br />
            </div>
            <dxe:ASPxGridView runat="server" ClientInstanceName="InsgridBatch" ID="gridBatch" KeyFieldName="BRDetails_Id" OnBatchUpdate="gridBatch_BatchUpdate"
                OnCellEditorInitialize="gridBatch_CellEditorInitialize" OnDataBinding="gridBatch_DataBinding"
                Width="100%" Settings-ShowFooter="false" SettingsBehavior-AllowSort="false" SettingsBehavior-AllowDragDrop="false"
                OnRowInserting="Grid_RowInserting" Settings-VerticalScrollBarMode="Auto" Settings-VerticalScrollableHeight="200" SettingsPager-Mode="ShowAllRecords"
                OnRowUpdating="Grid_RowUpdating" OnCustomCallback="gridBatch_CustomCallback"
                OnRowDeleting="Grid_RowDeleting">
                <SettingsPager Visible="false"></SettingsPager>
                <Columns>
                    <dxe:GridViewCommandColumn ShowDeleteButton="false" ShowNewButtonInHeader="false" Width="50" VisibleIndex="0" Caption="Action" HeaderStyle-HorizontalAlign="Center">
                        <CustomButtons>
                            <dxe:GridViewCommandColumnCustomButton Text=" " ID="CustomDeleteIDS" Image-Url="/assests/images/crs.png">
                            </dxe:GridViewCommandColumnCustomButton>
                        </CustomButtons>
                    </dxe:GridViewCommandColumn>
                    <dxe:GridViewDataTextColumn FieldName="SrlNo" Caption="Sl" VisibleIndex="1" Width="30">
                        <PropertiesTextEdit>
                        </PropertiesTextEdit>
                        <CellStyle Wrap="False" HorizontalAlign="Right" CssClass="gridcellleft"></CellStyle>
                    </dxe:GridViewDataTextColumn>


                    <dxe:GridViewDataButtonEditColumn FieldName="ProductName" Caption="Product" VisibleIndex="2">
                        <PropertiesButtonEdit>
                            <ClientSideEvents ButtonClick="ProductButnClick" KeyDown="ProductKeyDown" LostFocus="ProductsGotFocus" GotFocus="ProductsGotFocusFromID" />
                            <Buttons>
                                <dxe:EditButton Text="..." Width="20px">
                                </dxe:EditButton>
                            </Buttons>
                        </PropertiesButtonEdit>
                    </dxe:GridViewDataButtonEditColumn>
                    <dxe:GridViewDataTextColumn FieldName="gvColProduct" Caption="hidden Field Id" VisibleIndex="11" ReadOnly="True" Width="0"
                        EditCellStyle-CssClass="hide" PropertiesTextEdit-FocusedStyle-CssClass="hide" PropertiesTextEdit-Style-CssClass="hide">
                        <CellStyle CssClass="hide"></CellStyle>
                    </dxe:GridViewDataTextColumn>


                    <dxe:GridViewDataTextColumn VisibleIndex="3" Caption="Description" FieldName="gvColDiscription">
                        <PropertiesTextEdit>
                        </PropertiesTextEdit>
                        <CellStyle Wrap="true" HorizontalAlign="Left" CssClass="gridcellleft"></CellStyle>
                    </dxe:GridViewDataTextColumn>

                    <dxe:GridViewDataTextColumn VisibleIndex="4" Caption="Quantity" FieldName="gvColQuantity" Width="110" HeaderStyle-HorizontalAlign="Right">
                        <PropertiesTextEdit>
                            <MaskSettings Mask="<0..999999999>.<0..9999>" />

                        </PropertiesTextEdit>

                        <CellStyle Wrap="False" HorizontalAlign="Right" CssClass="gridcellleft"></CellStyle>
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn VisibleIndex="5" Caption="UOM(Stock)" FieldName="gvColUOM" Width="110">
                        <PropertiesTextEdit>
                        </PropertiesTextEdit>
                        <CellStyle Wrap="False" HorizontalAlign="Right" CssClass="gridcellleft"></CellStyle>
                    </dxe:GridViewDataTextColumn>


                    <dxe:GridViewCommandColumn Width="7%" VisibleIndex="7" Caption="Stk Details">
                        <CustomButtons>
                            <dxe:GridViewCommandColumnCustomButton Text=" " ID="CustomWarehouse" Image-Url="/assests/images/warehouse.png">
                            </dxe:GridViewCommandColumnCustomButton>
                        </CustomButtons>
                    </dxe:GridViewCommandColumn>
                    <dxe:GridViewCommandColumn ShowDeleteButton="false" ShowNewButtonInHeader="false" Width="80" VisibleIndex="8" Caption="Action" HeaderStyle-HorizontalAlign="Center">
                        <CustomButtons>
                            <dxe:GridViewCommandColumnCustomButton Text=" " ID="CustomAddNewRow" Image-Url="/assests/images/add.png">
                            </dxe:GridViewCommandColumnCustomButton>

                        </CustomButtons>
                    </dxe:GridViewCommandColumn>
                    <dxe:GridViewDataTextColumn FieldName="IsComponentProduct" Caption="IsComponentProduct" Width="0">
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn FieldName="IsLinkedProduct" Caption="IsLinkedProduct" Width="0">
                    </dxe:GridViewDataTextColumn>



                </Columns>

                <ClientSideEvents EndCallback="OnEndCallback" RowClick="GetVisibleIndex"
                    CustomButtonClick="OnCustomButtonClick" BatchEditStartEditing="gridFocusedRowChanged" />
                <SettingsDataSecurity AllowEdit="true" />
                <SettingsEditing Mode="Batch" NewItemRowPosition="Bottom">
                    <BatchEditSettings ShowConfirmOnLosingChanges="false" EditMode="Row" />
                </SettingsEditing>

                <Styles>
                    <StatusBar CssClass="statusBar">
                    </StatusBar>
                </Styles>
            </dxe:ASPxGridView>

            <div>
                <br />
            </div>

            <table style="float: left;">
                <tr>

                    <td>
                        <asp:Label ID="lbl_quotestatusmsg" runat="server" Text="" Font-Bold="true" ForeColor="Red" Font-Size="Medium"></asp:Label>
                        <dxe:ASPxButton ID="btnnew" ClientInstanceName="cbtn_SaveRecords" runat="server" AutoPostBack="False" Text="Save & N&#818;ew"
                            CssClass="btn btn-success"
                            meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                            <ClientSideEvents Click="function(s, e) {Save_ButtonClick();}" />
                        </dxe:ASPxButton>

                    </td>
                    <td>
                        <dxe:ASPxButton ID="btnSaveExit" ClientInstanceName="cbtn_SaveRecordsExit" runat="server" AutoPostBack="False" Text="Save & Ex&#818;it" CssClass="btn btn-success"
                            meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                            <ClientSideEvents Click="function(s, e) {SaveExitButtonClick();}" />
                        </dxe:ASPxButton>

                    </td>
                    <td>
                        <dxe:ASPxButton ID="btnSaveUdf" ClientInstanceName="cbtn_SaveUdf" runat="server" AutoPostBack="False" Text="U&#818;DF" UseSubmitBehavior="False"
                            CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1">
                            <ClientSideEvents Click="function(s, e) {OpenUdf();}" />
                        </dxe:ASPxButton>
                    </td>


                </tr>
                <tr><b><span id="tagged" style="display: none; color: red">Tagged in Unit Transfer Out. Cannot Modify</span></b></tr>
                <tr><b><span id="taggModify" style="display: none; color: red">Requested By Other Unit. Cannot Modify</span></b></tr>
            </table>
            <%--Batch Product Popup Start--%>

            <dxe:ASPxPopupControl ID="ProductpopUp" runat="server" ClientInstanceName="cProductpopUp"
                CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" Height="400"
                Width="700" HeaderText="Select Product" AllowResize="true" ResizingMode="Postponed" Modal="true">
                <ContentCollection>
                    <dxe:PopupControlContentControl runat="server">
                        <label><strong>Search By Product Name</strong></label>
                        <dxe:ASPxGridLookup ID="productLookUp" runat="server" DataSourceID="ProductDataSource" ClientInstanceName="cproductLookUp"
                            KeyFieldName="Products_ID" Width="800" TextFormatString="{0}" MultiTextSeparator=", " ClientSideEvents-TextChanged="ProductSelected"
                            ClientSideEvents-KeyDown="ProductlookUpKeyDown">
                            <Columns>
                                <dxe:GridViewDataColumn FieldName="ProductsName" Caption="Name" Width="220">
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
                                                    <%--  <dxe:ASPxButton ID="ASPxButton1" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseGridLookup" />--%>
                                                </td>
                                            </tr>
                                        </table>
                                    </StatusBar>
                                </Templates>
                                <Settings ShowFilterRow="True" ShowFilterRowMenu="true" ShowStatusBar="Visible" UseFixedTableLayout="true" />
                            </GridViewProperties>
                        </dxe:ASPxGridLookup>

                    </dxe:PopupControlContentControl>
                </ContentCollection>
                <HeaderStyle BackColor="Blue" Font-Bold="True" ForeColor="White" />
            </dxe:ASPxPopupControl>

            <asp:SqlDataSource runat="server" ID="ProductDataSource" 
                SelectCommand="prc_PurchaseReturnRequestDetailsList" SelectCommandType="StoredProcedure">
                <SelectParameters>
                    <asp:Parameter Type="String" Name="Action" DefaultValue="ProductDetails" />
                </SelectParameters>
            </asp:SqlDataSource>

            <%--Batch Product Popup End--%>
            <dxe:ASPxPopupControl ID="Popup_Warehouse" runat="server" ClientInstanceName="cPopup_Warehouse"
                Width="900px" HeaderText="Warehouse Details" PopupHorizontalAlign="WindowCenter"
                BackColor="white" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
                Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
                ContentStyle-CssClass="pad">
                <ClientSideEvents Closing="function(s, e) {
	                closeWarehouse(s, e);}" />
                <ContentStyle VerticalAlign="Top" CssClass="pad">
                </ContentStyle>
                <ContentCollection>
                    <dxe:PopupControlContentControl runat="server">
                        <div class="Top clearfix">
                            <div id="content-5" class="pull-right wrapHolder content horizontal-images" style="width: 100%; margin-right: 0px;">
                                <ul>
                                    <li>
                                        <div class="lblHolder">
                                            <table>
                                                <tbody>
                                                    <tr>
                                                        <td>Selected Product</td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblProductName" runat="server"></asp:Label></td>
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
                                                        <td>Entered Quantity </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="txt_SalesAmount" runat="server"></asp:Label>
                                                            <asp:Label ID="txt_SalesUOM" runat="server"></asp:Label>

                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </div>
                                    </li>
                                    <li>
                                        <div class="lblHolder" id="divpopupAvailableStock" style="display: none;">
                                            <table>
                                                <tbody>
                                                    <tr>
                                                        <td>Available Stock</td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblAvailableStock" runat="server"></asp:Label>
                                                            <asp:Label ID="lblAvailableStockUOM" runat="server"></asp:Label>

                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </div>
                                    </li>
                                    <li style="display: none;">
                                        <div class="lblHolder">
                                            <table>
                                                <tbody>
                                                    <tr>
                                                        <td>Stock Quantity </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="txt_StockAmount" runat="server"></asp:Label>
                                                            <asp:Label ID="txt_StockUOM" runat="server"></asp:Label></td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </div>
                                    </li>

                                </ul>
                            </div>

                            <div class="clear">
                                <br />
                            </div>
                            <div class="clearfix col-md-12" style="background: #f5f4f3; padding: 8px 0; margin-bottom: 15px; border-radius: 4px; border: 1px solid #ccc;">
                                <div>
                                    <div class="col-md-3" id="div_Warehouse">
                                        <div style="margin-bottom: 5px;">
                                            Warehouse
                                        </div>
                                        <div class="Left_Content" style="">
                                            <dxe:ASPxComboBox ID="CmbWarehouse" EnableIncrementalFiltering="True" ClientInstanceName="cCmbWarehouse" SelectedIndex="0"
                                                TextField="WarehouseName" ValueField="WarehouseID" runat="server" Width="100%" OnCallback="CmbWarehouse_Callback">
                                                <ClientSideEvents ValueChanged="function(s,e){CmbWarehouse_ValueChange()}" EndCallback="CmbWarehouseEndCallback"></ClientSideEvents>
                                            </dxe:ASPxComboBox>
                                            <span id="spnCmbWarehouse" class="pullleftClass fa fa-exclamation-circle iconRed" style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                        </div>
                                    </div>
                                    <div class="col-md-3" id="div_Batch">
                                        <div style="margin-bottom: 5px;">
                                            Batch/Lot
                                        </div>
                                        <div class="Left_Content" style="">
                                            <dxe:ASPxComboBox ID="CmbBatch" EnableIncrementalFiltering="True" ClientInstanceName="cCmbBatch"
                                                TextField="BatchName" ValueField="BatchID" runat="server" Width="100%" OnCallback="CmbBatch_Callback">
                                                <ClientSideEvents ValueChanged="function(s,e){CmbBatch_ValueChange()}" EndCallback="CmbBatchEndCall"></ClientSideEvents>
                                            </dxe:ASPxComboBox>
                                            <span id="spnCmbBatch" class="pullleftClass fa fa-exclamation-circle iconRed" style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                        </div>
                                    </div>
                                    <div class="col-md-4" id="div_Serial">
                                        <div style="margin-bottom: 5px;">
                                            Serial No &nbsp;&nbsp; (
                                                <input type="checkbox" id="myCheck" name="BarCode" onchange="AutoCalculateMandateOnChange(this)">Barcode )
                                        </div>
                                        <div class="" id="divMultipleCombo">
                                            <%--<dxe:ASPxComboBox ID="CmbSerial" EnableIncrementalFiltering="True" ClientInstanceName="cCmbSerial"
                                                    TextField="SerialName" ValueField="SerialID" runat="server" Width="100%" OnCallback="CmbSerial_Callback">
                                                </dxe:ASPxComboBox>--%>
                                            <dxe:ASPxDropDownEdit ClientInstanceName="checkComboBox" ID="ASPxDropDownEdit1" Width="85%" CssClass="pull-left" runat="server" AnimationType="None">
                                                <DropDownWindowStyle BackColor="#EDEDED" />
                                                <DropDownWindowTemplate>
                                                    <dxe:ASPxListBox Width="100%" ID="listBox" ClientInstanceName="checkListBox" SelectionMode="CheckColumn" OnCallback="CmbSerial_Callback"
                                                        runat="server">
                                                        <Border BorderStyle="None" />
                                                        <BorderBottom BorderStyle="Solid" BorderWidth="1px" BorderColor="#DCDCDC" />

                                                        <ClientSideEvents SelectedIndexChanged="OnListBoxSelectionChanged" EndCallback="listBoxEndCall" />
                                                    </dxe:ASPxListBox>
                                                    <table style="width: 100%">
                                                        <tr>
                                                            <td style="padding: 4px">
                                                                <dxe:ASPxButton ID="ASPxButton4" AutoPostBack="False" runat="server" Text="Close" Style="float: right">
                                                                    <ClientSideEvents Click="function(s, e){ checkComboBox.HideDropDown(); }" />
                                                                </dxe:ASPxButton>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </DropDownWindowTemplate>
                                                <ClientSideEvents TextChanged="SynchronizeListBoxValues" DropDown="SynchronizeListBoxValues" GotFocus="function(s, e){ s.ShowDropDown(); }" />
                                            </dxe:ASPxDropDownEdit>
                                            <span id="spncheckComboBox" class="pullleftClass fa fa-exclamation-circle iconRed" style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                            <div class="pull-left">
                                                <i class="fa fa-commenting" id="abpl" aria-hidden="true" style="font-size: 16px; cursor: pointer; margin: 3px 0 0 5px;" title="Serial No " data-container="body" data-toggle="popover" data-placement="right" data-content=""></i>
                                            </div>
                                        </div>
                                        <div class="" id="divSingleCombo" style="display: none;">
                                            <dxe:ASPxTextBox ID="txtserial" runat="server" Width="85%" ClientInstanceName="ctxtserial" HorizontalAlign="Left" Font-Size="12px" MaxLength="49">
                                                <ClientSideEvents LostFocus="txtserialTextChanged" />
                                            </dxe:ASPxTextBox>
                                        </div>
                                    </div>
                                    <div class="col-md-3" id="div_Quantity">
                                        <div style="margin-bottom: 2px;">
                                            Quantity
                                        </div>
                                        <div class="Left_Content" style="">
                                            <dxe:ASPxTextBox ID="txtQuantity" runat="server" ClientInstanceName="ctxtQuantity" HorizontalAlign="Right" Font-Size="12px" Width="100%" Height="15px">
                                                <MaskSettings Mask="<0..999999999999>.<0..99>" IncludeLiterals="DecimalSymbol" />
                                                <ClientSideEvents TextChanged="function(s, e) {SaveWarehouse();}" />
                                            </dxe:ASPxTextBox>
                                            <span id="spntxtQuantity" class="pullleftClass fa fa-exclamation-circle iconRed" style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <div>
                                        </div>
                                        <div class="Left_Content" style="padding-top: 14px">
                                            <dxe:ASPxButton ID="btnWarehouse" ClientInstanceName="cbtnWarehouse" Width="50px" runat="server" AutoPostBack="False" Text="Add" CssClass="btn btn-primary">
                                                <ClientSideEvents Click="function(s, e) {if(!document.getElementById('myCheck').checked) SaveWarehouse();}" />
                                            </dxe:ASPxButton>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="clearfix">
                                <dxe:ASPxGridView ID="GrdWarehouse" runat="server" KeyFieldName="SrlNo" AutoGenerateColumns="False"
                                    Width="100%" ClientInstanceName="cGrdWarehouse" OnCustomCallback="GrdWarehouse_CustomCallback" OnDataBinding="GrdWarehouse_DataBinding"
                                    SettingsPager-Mode="ShowAllRecords" Settings-VerticalScrollBarMode="auto" Settings-VerticalScrollableHeight="200" SettingsBehavior-AllowSort="false">
                                    <Columns>
                                        <dxe:GridViewDataTextColumn Caption="Warehouse Name" FieldName="WarehouseName"
                                            VisibleIndex="0">
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="Available Quantity" FieldName="AvailableQty" Visible="false"
                                            VisibleIndex="1">
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="Quantity" FieldName="SalesQuantity"
                                            VisibleIndex="2">
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="Conversion Foctor" FieldName="ConversionMultiplier" Visible="false"
                                            VisibleIndex="3">
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="Stock Quantity" FieldName="StkQuantity" Visible="false"
                                            VisibleIndex="4">
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="Balance Stock" FieldName="BalancrStk" Visible="false"
                                            VisibleIndex="5">
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="Batch/Lot Number" FieldName="BatchNo"
                                            VisibleIndex="6">
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="Mfg Date" FieldName="MfgDate"
                                            VisibleIndex="7">
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="Expiry Date" FieldName="ExpiryDate"
                                            VisibleIndex="8">
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="Serial Number" FieldName="SerialNo"
                                            VisibleIndex="9">
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn VisibleIndex="10" Width="80px">
                                            <DataItemTemplate>
                                                <a href="javascript:void(0);" onclick="fn_Edit('<%# Container.KeyValue %>')" title="Delete">
                                                    <img src="../../../assests/images/Edit.png" /></a>
                                                &nbsp;
                                                        <a href="javascript:void(0);" id="ADelete" onclick="fn_Deletecity('<%# Container.KeyValue %>')" title="Delete">
                                                            <img src="/assests/images/crs.png" /></a>
                                            </DataItemTemplate>
                                        </dxe:GridViewDataTextColumn>
                                    </Columns>
                                    <ClientSideEvents EndCallback="OnWarehouseEndCallback" />
                                    <SettingsPager Visible="false"></SettingsPager>
                                    <SettingsLoadingPanel Text="Please Wait..." />
                                </dxe:ASPxGridView>
                            </div>
                            <div class="clearfix">
                                <br />
                                <div style="align-content: center">
                                    <dxe:ASPxButton ID="btnWarehouseSave" ClientInstanceName="cbtnWarehouseSave" Width="50px" runat="server" AutoPostBack="False" Text="OK" CssClass="btn btn-primary">
                                        <ClientSideEvents Click="function(s, e) {FinalWarehouse();}" />
                                    </dxe:ASPxButton>
                                </div>
                            </div>
                        </div>
                    </dxe:PopupControlContentControl>
                </ContentCollection>
                <HeaderStyle BackColor="LightGray" ForeColor="Black" />
            </dxe:ASPxPopupControl>
        </div>

    </div>
    </div>
    <div id="HiddenField">
        <asp:HiddenField ID="hdnRefreshType" runat="server" />
        <asp:HiddenField ID="hdnEditIndentID" runat="server" />
        <asp:HiddenField ID="hdfIsDelete" runat="server" />
        <asp:HiddenField ID="hdnSchemaType" runat="server" />
        <asp:HiddenField ID="hdnCurrenctId" runat="server" />
        <asp:HiddenField ID="hdnSaveNew" runat="server" />
        <asp:HiddenField ID="hdn_Mode" runat="server" />
        <asp:HiddenField ID="hdnPageStatus" runat="server" />
        <asp:HiddenField ID="hdnEditClick" runat="server" />
        <asp:HiddenField ID="hdfProductID" runat="server" />
        <asp:HiddenField ID="hdfProductType" runat="server" />
        <asp:HiddenField ID="hdfProductSerialID" runat="server" />
        <asp:HiddenField ID="hdnProductQuantity" runat="server" />
        <asp:HiddenField ID="hdnPartyId" runat="server" />
    </div>
    <asp:SqlDataSource ID="dsBranchTo" runat="server"
        ConflictDetection="CompareAllValues"
        SelectCommand=""></asp:SqlDataSource>
    <asp:SqlDataSource ID="dsBranch" runat="server" 
        ConflictDetection="CompareAllValues"
        SelectCommand=""></asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlSchematype" runat="server"
        SelectCommand="Select * From ((Select '0' as ID,'Select' as SchemaName) Union (Select ID,SchemaName From tbl_master_Idschema  Where TYPE_ID='16')) as X Order By ID ASC"></asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlCurrency" runat="server" 
        SelectCommand="select Currency_ID,Currency_AlphaCode from Master_Currency Order By Currency_ID"></asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlCurrencyBind" runat="server" ></asp:SqlDataSource>
    <dxe:ASPxCallbackPanel runat="server" ID="acpAvailableStock" ClientInstanceName="cacpAvailableStock" OnCallback="acpAvailableStock_Callback">
        <PanelCollection>
            <dxe:PanelContent runat="server">
            </dxe:PanelContent>
        </PanelCollection>
        <ClientSideEvents EndCallback="acpAvailableStockEndCall" />
    </dxe:ASPxCallbackPanel>
    <dxe:ASPxCallbackPanel runat="server" ID="CallbackPanel" ClientInstanceName="cCallbackPanel" OnCallback="CallbackPanel_Callback">
        <PanelCollection>
            <dxe:PanelContent runat="server">
            </dxe:PanelContent>
        </PanelCollection>
        <ClientSideEvents EndCallback="CallbackPanelEndCall" />
    </dxe:ASPxCallbackPanel>
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
    <asp:SqlDataSource ID="VendorDataSource" runat="server"  />
    <!--Customer Modal -->
    <%--Rev 2.0--%>
    <%--<div class="modal fade" id="PartyModel" role="dialog">--%>
        <div class="modal fade" id="PartyModel" role="dialog" data-backdrop="static" data-keyboard="false">
        <%--Rev end 2.0--%>
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Party Search</h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="VendorModekkeydown(event)" id="txtPartySearch" autofocus width="100%" placeholder="Search By Party Name or Unique Id" />

                    <div id="CustomerTable">
                        <table border='1' width="100%" class="dynamicPopupTbl">
                            <tr class="HeaderStyle">
                                <th class="hide">id</th>
                                <th>Party Name</th>
                                <th>Unique Id</th>
                                <th>TYpe</th>
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
    <%--InlineTax--%>
</asp:Content>
