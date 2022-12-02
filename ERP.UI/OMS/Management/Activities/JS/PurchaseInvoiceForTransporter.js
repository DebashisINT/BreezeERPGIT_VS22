
        function clookup_Project_LostFocus() {        
           
            var projID = clookup_Project.GetValue();
            if (projID == null || projID == "") {
                $("#ddlHierarchy").val(0);
            }            
        }
//Hierarchy Start 
function ProjectValueChange(s, e) {
    var projID = clookup_Project.GetValue();
    $.ajax({
        type: "POST",
        url: 'PurchaseInvoiceforTransporter.aspx/getHierarchyID',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ ProjID: projID }),
        success: function (msg) {
            var data = msg.d;
            $("#ddlHierarchy").val(data);
        }
    });
}
//Hierarchy End 
function Project_gotFocus() {
    if ($("#hdnProjectSelectInEntryModule").val() == "1")
        clookup_Project.gridView.Refresh();
    clookup_Project.ShowDropDown();
}





//<%--Batch Product Popup Start--%>

    var _ComponentDetails;

function TaggedDoc_EndCallback(s, e) {
    if (ctaggingGrid.cpComponentDetails) {
        _ComponentDetails = ctaggingGrid.cpComponentDetails;
        var ComponentDetails = _ComponentDetails.split("~");
        ctaggingGrid.cpComponentDetails = null;
        var ComponentNumber = ComponentDetails[0];
        var ComponentDate = ComponentDetails[1];
        ctaggingList.SetValue(ComponentNumber);
    }
}
function taggingListKeyDown(s, e) {
    if (e.htmlEvent.key == "Enter") {
        s.OnButtonClick(0);
    }
}

function taggingListButnClick(s, e) {
    var branchid = $('#ddl_Branch').val();
    var key = GetObjectID('hdnCustomerId').value;
    if (branchid == '' || branchid == null) {
        //cddl_Bill.SetSelectedIndex(0);
        jAlert('Select for branch first');
        return;

    }
    else if (key == '' || key == null) {


        jAlert('Select Transporter first');
        return;


    }
    else {
        //var BillType = cddl_Bill.GetValue();
        ctaggingGrid.PerformCallback('BindPBforTagging~' + key + '~' + branchid);
        cpopup_taggingGrid.Show();
    }
}
function SelectTaggDoc() {
    var OrderData = ctaggingGrid.GetSelectedKeysOnPage();

    if (OrderData == 0) {
        cpopup_taggingGrid.Hide();
        ctaggingList.SetText('');
        if ($('#hdnADDEditMode').val() != 'Edit') {
            ctxtVendorName.clientEnabled = true;
        }
    }
    else {
        ctxtVendorName.clientEnabled = false;
        ctaggingGrid.PerformCallback('SetTaggingDtl');
        cpopup_taggingGrid.Hide();
        //cProductsPopup.Show();
    }
}
//function Tag_ChangeState(value) {
//    ctaggingGrid.PerformCallback('SelectAndDeSelectProducts' + '~' + value);
//}

//function PopulateTaggedBillByBillType(e) {
//    var branchid = $('#ddl_Branch').val();
//    var key = GetObjectID('hdnCustomerId').value;
//    if (branchid == '' || branchid == null)
//    {
//        cddl_Bill.SetSelectedIndex(0);
//        jAlert('Select for branch first'); 

//    }
//    else if (key == '' || key == null) {

//        if (cddl_Bill.GetSelectedIndex() != 0) {
//            jAlert('Select Vendor first');

//        }
//        cddl_Bill.SetSelectedIndex(0);
//    }
//    else {
//        var BillType = cddl_Bill.GetValue();
//        cPanelTaggedPB.PerformCallback('BindPBforTagging~' + key + '~' + branchid + '~' + BillType);
//    }
//}

function TDSEditableCheckChanged() {
    var checkval = cchk_TDSEditable.GetChecked();
    if (checkval) {
        cgridinventory.SetEnabled(true);
    }
    else {
        cgridinventory.SetEnabled(false);
    }
}

// TDS Section Modification Section Start on 05022017 by Sam
function TDSAmtLostFocus() {

    var TDSAmt = (cgridinventory.GetEditor('TDSAmount').GetValue() != null) ? cgridinventory.GetEditor('TDSAmount').GetValue() : "0";
    var SurchargeAmount = (cgridinventory.GetEditor('SurchargeAmount').GetValue() != null) ? cgridinventory.GetEditor('SurchargeAmount').GetValue() : "0";
    var EducationCessAmt = (cgridinventory.GetEditor('EducationCessAmt').GetValue() != null) ? cgridinventory.GetEditor('EducationCessAmt').GetValue() : "0";
    var HgrEducationCessAmt = (cgridinventory.GetEditor('HgrEducationCessAmt').GetValue() != null) ? cgridinventory.GetEditor('HgrEducationCessAmt').GetValue() : "0";
    var TotaNonInvSecAmt = parseFloat(TDSAmt) + parseFloat(SurchargeAmount) + parseFloat(EducationCessAmt) + parseFloat(HgrEducationCessAmt)
    ctxt_totalnoninventoryproductamt.SetText(Math.round(TotaNonInvSecAmt));
}

function SurchargeAmountLostFocus() {
    var TDSAmt = (cgridinventory.GetEditor('TDSAmount').GetValue() != null) ? cgridinventory.GetEditor('TDSAmount').GetValue() : "0";
    var SurchargeAmount = (cgridinventory.GetEditor('SurchargeAmount').GetValue() != null) ? cgridinventory.GetEditor('SurchargeAmount').GetValue() : "0";
    var EducationCessAmt = (cgridinventory.GetEditor('EducationCessAmt').GetValue() != null) ? cgridinventory.GetEditor('EducationCessAmt').GetValue() : "0";
    var HgrEducationCessAmt = (cgridinventory.GetEditor('HgrEducationCessAmt').GetValue() != null) ? cgridinventory.GetEditor('HgrEducationCessAmt').GetValue() : "0";
    var TotaNonInvSecAmt = parseFloat(TDSAmt) + parseFloat(SurchargeAmount) + parseFloat(EducationCessAmt) + parseFloat(HgrEducationCessAmt)
    ctxt_totalnoninventoryproductamt.SetText(Math.round(TotaNonInvSecAmt));
}

function EducationCessAmtLostFocus() {
    var TDSAmt = (cgridinventory.GetEditor('TDSAmount').GetValue() != null) ? cgridinventory.GetEditor('TDSAmount').GetValue() : "0";
    var SurchargeAmount = (cgridinventory.GetEditor('SurchargeAmount').GetValue() != null) ? cgridinventory.GetEditor('SurchargeAmount').GetValue() : "0";
    var EducationCessAmt = (cgridinventory.GetEditor('EducationCessAmt').GetValue() != null) ? cgridinventory.GetEditor('EducationCessAmt').GetValue() : "0";
    var HgrEducationCessAmt = (cgridinventory.GetEditor('HgrEducationCessAmt').GetValue() != null) ? cgridinventory.GetEditor('HgrEducationCessAmt').GetValue() : "0";
    var TotaNonInvSecAmt = parseFloat(TDSAmt) + parseFloat(SurchargeAmount) + parseFloat(EducationCessAmt) + parseFloat(HgrEducationCessAmt)
    ctxt_totalnoninventoryproductamt.SetText(Math.round(TotaNonInvSecAmt));
}

function HgrEducationCessAmtLostFocus() {
    var TDSAmt = (cgridinventory.GetEditor('TDSAmount').GetValue() != null) ? cgridinventory.GetEditor('TDSAmount').GetValue() : "0";
    var SurchargeAmount = (cgridinventory.GetEditor('SurchargeAmount').GetValue() != null) ? cgridinventory.GetEditor('SurchargeAmount').GetValue() : "0";
    var EducationCessAmt = (cgridinventory.GetEditor('EducationCessAmt').GetValue() != null) ? cgridinventory.GetEditor('EducationCessAmt').GetValue() : "0";
    var HgrEducationCessAmt = (cgridinventory.GetEditor('HgrEducationCessAmt').GetValue() != null) ? cgridinventory.GetEditor('HgrEducationCessAmt').GetValue() : "0";
    var TotaNonInvSecAmt = parseFloat(TDSAmt) + parseFloat(SurchargeAmount) + parseFloat(EducationCessAmt) + parseFloat(HgrEducationCessAmt)
    ctxt_totalnoninventoryproductamt.SetText(Math.round(TotaNonInvSecAmt));
}

// TDS Section Modification Section End on 05022017 by Sam

function RCMCheckChanged() {
    var checkval = cchk_reversemechenism.GetChecked();
    if (checkval) {
        cddl_AmountAre.SetValue(3);
        PopulateGSTCSTVAT();
        cddl_AmountAre.SetEnabled(false);
    }
    else {
        cddl_AmountAre.SetValue(1);
        PopulateGSTCSTVAT();
        cddl_AmountAre.SetEnabled(true);
    }
}
//function ProductSelected(s, e) {
function SetProduct(Id, Name) {
    $('#ProductModel').modal('hide');
    var LookUpData = Id;
    var ProductCode = Name;
    if (!ProductCode) {
        LookUpData = null;
    }
    cProductpopUp.Hide();
    grid.batchEditApi.StartEdit(globalRowIndex);
    grid.GetEditor("ProductID").SetText(LookUpData);
    grid.GetEditor("ProductName").SetText(ProductCode);
    pageheaderContent.style.display = "block";
    cddl_AmountAre.SetEnabled(false);
    cddl_TdsScheme.SetEnabled(false);
    ctxtVendorName.SetEnabled(false);   
    $('#ddlInventory').prop('disabled', true);
    var tbDescription = grid.GetEditor("Description");
    var tbUOM = grid.GetEditor("UOM");
    var tbSalePrice = grid.GetEditor("PurchasePrice");
    var ProductID = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
    var SpliteDetails = ProductID.split("||@||");
    var strProductID = SpliteDetails[0];
    var strDescription = SpliteDetails[1];
    var strUOM = SpliteDetails[2];
    var strStkUOM = SpliteDetails[4];
    var strSalePrice = SpliteDetails[6];
    var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
    var IsPackingActive = SpliteDetails[10];
    var Packing_Factor = SpliteDetails[11];
    var Packing_UOM = SpliteDetails[12];
    var strRate = (ctxtRate.GetValue() != null && ctxtRate.GetValue() != "0") ? ctxtRate.GetValue() : "1";
    if (strRate == 0) {
        strSalePrice = strSalePrice;
    }
    else {
        strSalePrice = strSalePrice / strRate;
    }
    tbDescription.SetValue(strDescription);
    tbUOM.SetValue(strUOM);
    tbSalePrice.SetValue(strSalePrice);
    grid.GetEditor("Quantity").SetValue("0.00");
    grid.GetEditor("Discount").SetValue("0.00");
    grid.GetEditor("Amount").SetValue("0.00");
    grid.GetEditor("TaxAmount").SetValue("0.00");
    grid.GetEditor("TotalAmount").SetValue("0.00");
    var ddlbranch = $("[id*=ddl_Branch]");
    var strBranch = ddlbranch.find("option:selected").text();
    $('#lblStkQty').text("0.00");
    $('#lblStkUOM').text(strStkUOM);
    $('#lblProduct').text(strDescription);
    $('#lblbranchName').text(strBranch);
    if (IsPackingActive == "Y" && (parseFloat(Packing_Factor * QuantityValue) > 0)) {
        $('#lblPackingStk').text(PackingValue);
    } else {
        divPacking.style.display = "none";
    }
    // Running total Calculation Start
    Cur_Quantity = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
    Cur_Amt = (grid.GetEditor('Amount').GetValue() != null) ? grid.GetEditor('Amount').GetValue() : "0";
    Cur_TotalAmt = (grid.GetEditor('TotalAmount').GetValue() != null) ? grid.GetEditor('TotalAmount').GetValue() : "0";
    CalculateAmount();
    // Running total Calculation End

    cchk_reversemechenism.SetEnabled(false);
    cddl_AmountAre.SetEnabled(false);

    //Debjyoti
    //ctaxUpdatePanel.PerformCallback('DelProdbySl~' + grid.GetEditor("SrlNo").GetValue() + '~' + strProductID);
    deleteTax('DelProdbySl', grid.GetEditor("SrlNo").GetValue(), strProductID);
    grid.batchEditApi.StartEdit(globalRowIndex, 6);
}
function deleteTax(Action, srl, productid) {
    var OtherDetail = {};
    OtherDetail.Action = Action;
    OtherDetail.srl = srl;
    OtherDetail.prodid = productid;


    $.ajax({
        type: "POST",
        url: "PurchaseInvoiceforTransporter.aspx/taxUpdatePanel_Callback",
        data: JSON.stringify(OtherDetail),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var Code = msg.d;
            if (Code != null) {
            }
            if (productid != "") {
                setTimeout(function () {
                    grid.batchEditApi.StartEdit(globalRowIndex, 5);
                }, 600)
            }

        }
    });
}


function ProductKeyDown(s, e) {
    if (e.htmlEvent.key == "Enter") {

        s.OnButtonClick(0);
    }

}

//function ProductButnClick(s, e) {
//    if (e.buttonIndex == 0) {

//        if (!GetObjectID('hdnCustomerId').value) {
//            jAlert("Please Select Customer first.", "Alert", function () { $('#txtCustSearch').focus(); })
//            return;
//        }

//        $('#txtProdSearch').val('');
//        $('#ProductModel').modal('show');
//    }
//}
function Purchaseprodkeydown(e) {
    var OtherDetails = {};
    OtherDetails.SearchKey = $("#txtProdSearch").val();
    var invtype = $('#ddlInventory').val();
    var TDSid = cddl_TdsScheme.GetValue();
    OtherDetails.InventoryType = invtype;
    OtherDetails.TDSCode = TDSid;

    if (e.code == "Enter" || e.code == "NumpadEnter") {
        var HeaderCaption = [];
        HeaderCaption.push("Product Code");
        HeaderCaption.push("Product Description");
        HeaderCaption.push("Inventory");
        HeaderCaption.push("HSN/SAC");
        HeaderCaption.push("Class");
        HeaderCaption.push("Brand");
        //HeaderCaption.push("Installation Reqd.");

        if ($("#txtProdSearch").val() != '') {
            callonServer("Services/Master.asmx/GetPurchaseInvoiceProduct", OtherDetails, "ProductTable", HeaderCaption, "ProdIndex", "SetProduct");
        }
    }
    else if (e.code == "ArrowDown") {
        if ($("input[ProdIndex=0]"))
            $("input[ProdIndex=0]").focus();
    }

}

function ProductButnClick(s, e) {
    if (e.buttonIndex == 0) {
        var customerval = GetObjectID('hdnCustomerId').value;
        if ($('#txtVoucherNo').val() == '' || $('#txtVoucherNo').val() == null) {
            jAlert('Select a numbering schema first.');
            $('#ddl_numberingScheme').focus();
            return false;
        }
        else if (customerval == '' || customerval == null || customerval == "") {
            jAlert('Select a Transporter first');
            ctxtVendorName.Focus();
            return false;
        }
        else if (cproductLookUp.Clear()) {
            // Running total Calculation Start 
            Pre_Quantity = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
            Pre_Amt = (grid.GetEditor('Amount').GetValue() != null) ? grid.GetEditor('Amount').GetValue() : "0";
            Pre_TotalAmt = (grid.GetEditor('TotalAmount').GetValue() != null) ? grid.GetEditor('TotalAmount').GetValue() : "0";
            // Running total Calculation End

            //New Modification Section on 05012018
            $('#txtProdSearch').val('');
            $('#ProductModel').modal('show');
            setTimeout(function () { $("#txtProdSearch").focus(); }, 500);
        }
    }
}



function SetHsnSac(newHsnSac) {
    newHsnSac = newHsnSac.trim();
    if (newHsnSac != "") {
        var existsHsnSac = $('#hdHsnList').val();
        if (existsHsnSac.indexOf(',' + newHsnSac + ',') == -1) {
            existsHsnSac = existsHsnSac + newHsnSac + ',';
            $('#hdHsnList').val(existsHsnSac);
        }
    }
}

function RemoveHSnSacFromList(newHsnSac) {
    newHsnSac = newHsnSac.trim();
    if (newHsnSac != "") {
        var existsHsnSac = $('#hdHsnList').val();

        existsHsnSac = existsHsnSac.replace(newHsnSac + ',', '');
        $('#hdHsnList').val(existsHsnSac);

    }
}

function ProductSelected(s, e) {
    if (cproductLookUp.GetGridView().GetFocusedRowIndex() == -1) {
        cProductpopUp.Hide();
        grid.batchEditApi.StartEdit(globalRowIndex, 5);
        return;
    }

    var LookUpData = cproductLookUp.GetGridView().GetRowKey(cproductLookUp.GetGridView().GetFocusedRowIndex());
    var ProductCode = cproductLookUp.GetValue();
    if (!ProductCode) {
        LookUpData = null;
    }
    cProductpopUp.Hide();
    grid.batchEditApi.StartEdit(globalRowIndex);
    //Delete hsn
    if (grid.GetEditor("ProductID").GetText() != "") {
        var previousProductId = grid.GetEditor("ProductID").GetText();
        RemoveHSnSacFromList(previousProductId.split("||@||")[19]);
    }


    grid.GetEditor("ProductID").SetText(LookUpData);
    grid.GetEditor("ProductName").SetText(ProductCode);

    pageheaderContent.style.display = "block";
    cddl_AmountAre.SetEnabled(false);

    var tbDescription = grid.GetEditor("Description");
    var tbUOM = grid.GetEditor("UOM");
    var tbSalePrice = grid.GetEditor("SalePrice");

    var ProductID = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
    var SpliteDetails = ProductID.split("||@||");
    var strProductID = SpliteDetails[0];
    var strDescription = SpliteDetails[1];
    var strUOM = SpliteDetails[2];
    var strStkUOM = SpliteDetails[4];
    var strSalePrice = SpliteDetails[6];
    $('#HDSelectedProduct').val(strProductID);


    var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
    var IsPackingActive = SpliteDetails[10];
    var Packing_Factor = SpliteDetails[11];
    var Packing_UOM = SpliteDetails[12];
    SetHsnSac(SpliteDetails[19]);
    var strRate = (ctxt_Rate.GetValue() != null && ctxt_Rate.GetValue() != "0") ? ctxt_Rate.GetValue() : "1";
    if (strRate == 0) {
        strSalePrice = strSalePrice;
    }
    else {
        strSalePrice = strSalePrice / strRate;
    }

    tbDescription.SetValue(strDescription);
    tbUOM.SetValue(strUOM);
    tbSalePrice.SetValue(strSalePrice);



    var totalNetAmount = grid.GetEditor("TotalAmount").GetValue();

    var newTotalNetAmount = parseFloat(cbnrlblAmountWithTaxValue.GetValue()) - parseFloat(totalNetAmount);
    cbnrlblAmountWithTaxValue.SetValue(parseFloat(Math.round(Math.abs(newTotalNetAmount) * 100) / 100).toFixed(2));
    SetInvoiceLebelValue();


    grid.GetEditor("Quantity").SetValue("0.00");
    grid.GetEditor("Discount").SetValue("0.00");
    grid.GetEditor("Amount").SetValue("0.00");
    grid.GetEditor("TaxAmount").SetValue("0.00");
    grid.GetEditor("TotalAmount").SetValue("0.00");

    var ddlbranch = $("[id*=ddl_Branch]");
    var strBranch = ddlbranch.find("option:selected").text();
    cchk_reversemechenism.SetEnabled(false);
    cddl_AmountAre.SetEnabled(false);


    $('#lblStkQty').text("0.00");
    $('#lblStkUOM').text(strStkUOM);
    $('#lblProduct').text(strDescription);
    $('#lblbranchName').text(strBranch);

    ctaxUpdatePanel.PerformCallback('DelProdbySl~' + grid.GetEditor("SrlNo").GetValue() + '~' + strProductID);
    cbnrOtherChargesvalue.SetText('0.00');
    SetRunningBalance();
    grid.batchEditApi.StartEdit(globalRowIndex, 5);
}



function ProductlookUpKeyDown(s, e) {
    if (e.htmlEvent.key == "Escape") {
        cProductpopUp.Hide();
        grid.batchEditApi.StartEdit(globalRowIndex, 5);
    }
}

//<%--Batch Product Popup End--%>

    // Vendor Search Section Start on 03012018

    function VendorButnClick(s, e) {

        document.getElementById("txtCustSearch").value = "";
        var txt = "<table border='1' width=\"100%\" class='dynamicPopupTbl'><tr class=\"HeaderStyle\"><th>Transporter Name</th><th>Unique Id</th><th>Type</th></tr><table>";
        document.getElementById("CustomerTable").innerHTML = txt;
        setTimeout(function () { $("#txtCustSearch").focus(); }, 500);
        $('#CustModel').modal('show');
        $('#txtCustSearch').focus();
    }

function VendorKeyDown(s, e) {
    if (e.htmlEvent.key == "Enter" || e.code == "NumpadEnter") {
        document.getElementById("txtCustSearch").value = "";
        var txt = "<table border='1' width=\"100%\" class='dynamicPopupTbl'><tr class=\"HeaderStyle\"><th>Transporter Name</th><th>Unique Id</th><th>Type</th></tr><table>";
        document.getElementById("CustomerTable").innerHTML = txt;
        setTimeout(function () { $("#txtCustSearch").focus(); }, 500);
        $('#CustModel').modal('show');
        $('#txtCustSearch').focus();
    }
}

function Customerkeydown(e) {
    var OtherDetails = {};
    OtherDetails.SearchKey = $("#txtCustSearch").val();
    OtherDetails.BranchID = $('#ddl_Branch').val();

    if (e.code == "Enter" || e.code == "NumpadEnter") {
        var HeaderCaption = [];
        HeaderCaption.push("Transporter Name");
        HeaderCaption.push("Unique Id");
        HeaderCaption.push("Type");
        if (OtherDetails.SearchKey != '') {
            callonServer("Services/Master.asmx/GetTransporterWithBranch", OtherDetails, "CustomerTable", HeaderCaption, "customerIndex", "SetCustomer");
        }
    }
    else if (e.code == "ArrowDown") {
        if ($("input[customerindex=0]"))
            $("input[customerindex=0]").focus();
    }
}



function ValueSelected(e, indexName) {
    if (e.code == "Enter") {
        var Id = e.target.parentElement.parentElement.cells[0].innerText;
        var name = e.target.parentElement.parentElement.cells[1].children[0].value;
        if (Id) {
            if (indexName == "ProdIndex")
                SetProduct(Id, name);
            else
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
            if (indexName == "ProdIndex")
                $('#txtProdSearch').focus();
            else
                $('#txtCustSearch').focus();
        }
    }
}

// Vendor Search Section End on 03012018



    // Final Checking by Sam on 15102017 Start


    function ddlInventory_OnChange() {
        var invtype = $('#ddlInventory').val();
        if (invtype == 'N' || invtype == 'S') {
            $('#divTdsScheme').removeClass('hide');
            $('#rdlbutton').addClass('hide');
            $('#rdldate').addClass('hide');
            // Updated by Sam on 13122017 to Hide Terms&Conditions and Vehicle Control Section Start
            $('#spVehTC').addClass('hide');


            // Updated by Sam on 13122017 to Hide Terms&Conditions and Vehicle Control Section End
        }
        else {
            $('#divTdsScheme').addClass('hide');
            $('#rdlbutton').removeClass('hide');
            $('#rdldate').removeClass('hide');
            // Updated by Sam on 13122017 to Show Terms&Conditions and Vehicle Control Section Start
            $('#spVehTC').removeClass('hide');
            // Updated by Sam on 13122017 to Show Terms&Conditions and Vehicle Control Section End
            //var key = gridLookup.GetValue()
            var key = GetObjectID('hdnCustomerId').value;
            if (key != null && key != '') {
                selectValue();
            }
        }
    }



//............Check Unique   Purchase Order................
function txtBillNo_TextChanged() {    // function 3
    var mode = ''
    var VoucherNo = document.getElementById("txtVoucherNo").value;
    $.ajax({
        type: "POST",
        url: "purchaseinvoice.aspx/CheckUniqueName",
        data: JSON.stringify({ VoucherNo: VoucherNo }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (msg) {
            var data = msg.d;

            if (data == true) {
                $("#DuplicateBillNo").show();

                document.getElementById("txtVoucherNo").value = '';
                document.getElementById("txtVoucherNo").focus();
            }
            else {
                $("#DuplicateBillNo").hide();
            }
        }
    });
}
function specialedit_ButtonClick() {
    flag = true;
    var invtype = $('#ddlInventory').val();
    if (invtype != 'N') {
        if (ctxt_partyInvNo.GetText() == '' || ctxt_partyInvNo.GetText() == null) {
            flag = false;
            $("#MandatorysPartyinvno").show();
            LoadingPanel.Hide();
            return false;
        }
    }
    // Invoice Date validation Start
    var sdate = cdt_partyInvDt.GetValue();
    var edate = cPLQuoteDate.GetValue();
    var startDate = new Date(sdate);
    var endDate = new Date(edate);
    if (invtype == 'N') {
        if (sdate == null || sdate == "") {
        }
        else {
            $('#MandatoryPartyDate').attr('style', 'display:none');

            if (startDate > endDate) {
                LoadingPanel.Hide();
                flag = false;
                $('#MandatoryEgSDate').attr('style', 'display:block');
            }
            else {
                $('#MandatoryEgSDate').attr('style', 'display:none');
                flag = true;
            }
        }
    }
    else if (invtype != 'N') {
        if (sdate == null || sdate == "") {
            flag = false;
            $('#MandatoryPartyDate').attr('style', 'display:block');
            LoadingPanel.Hide();
            return false;
        }
        else {
            $('#MandatoryPartyDate').attr('style', 'display:none');

            if (startDate > endDate) {
                LoadingPanel.Hide();
                return false;
                flag = false;
                $('#MandatoryEgSDate').attr('style', 'display:block');
            }
            else {
                $('#MandatoryEgSDate').attr('style', 'display:none');
                flag = true;
            }
        }
    }
    if (flag == true) {
        cpartyInvoicepanel.PerformCallback('SpecialEdit');
    }
}

//<%--// Running Balance Calculation--%>

    function GlobalBillingShippingEndCallBack() {
        if (cbsComponentPanel.cpGlobalBillingShippingEndCallBack_Edit == "0") {
            cbsComponentPanel.cpGlobalBillingShippingEndCallBack_Edit = "0";
            var invtype = $('#ddlInventory').val();
            var startDate = new Date();
            startDate = cPLQuoteDate.GetDate().format('yyyy/MM/dd');
            var branchid = $('#ddl_Branch').val();           
            var key = GetObjectID('hdnCustomerId').value;
            var type = ($("[id$='rdl_PurchaseInvoice']").find(":checked").val() != null) ? $("[id$='rdl_PurchaseInvoice']").find(":checked").val() : "";
            if (type != null && type != '') {
               
               // cContactPerson.PerformCallback('BindContactPerson~' + key + '~' + branchid);
                var OtherDetails = {}
                OtherDetails.VendorId = key;
                OtherDetails.vendorbranchid = branchid;
                $.ajax({
                    type: "POST",
                    url: "PurchaseInvoice.aspx/GetContactPerson",
                    data: JSON.stringify(OtherDetails),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        var returnObject = msg.d;
                        if (returnObject) {
                            SetDataSourceOnComboBox(cContactPerson, returnObject);
                        }
                    }
                });              
                var startDate = new Date();
                startDate = cPLQuoteDate.GetDate().format('yyyy/MM/dd');
               
                if (key != null && key != '') {
                    cQuotationComponentPanel.PerformCallback('DateCheckOnChanged' + '~' + key + '~' + startDate + '~' + 'DateCheck' + '~' + type + '~' + invtype);
                }
               // grid.PerformCallback('GridBlank');
                //ctaxUpdatePanel.PerformCallback('DeleteAllTax');

                deleteTax('DeleteAllTax', "", "");
                gridquotationLookup.SetText('');
            }
            else {              
                var key = GetObjectID('hdnCustomerId').value;
                if (key != null && key != '') {
                    // cContactPerson.PerformCallback('BindContactPerson~' + key + '~' + branchid); 
                    var OtherDetails = {}
                    OtherDetails.VendorId = key;
                    OtherDetails.vendorbranchid = branchid;
                    $.ajax({
                        type: "POST",
                        url: "PurchaseInvoice.aspx/GetContactPerson",
                        data: JSON.stringify(OtherDetails),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (msg) {
                            var returnObject = msg.d;
                            if (returnObject) {
                                SetDataSourceOnComboBox(cContactPerson, returnObject);
                            }
                        }
                    });
                }
            }           
        }
    }
    function SetDataSourceOnComboBox(ControlObject, Source) {
        ControlObject.ClearItems();
        for (var count = 0; count < Source.length; count++) {
            ControlObject.AddItem(Source[count].Name, Source[count].Id);
        }
        ControlObject.SetSelectedIndex(0);
    }
function partyInvDtMandatorycheck() {
    var invtype = $('#ddlInventory').val();
    if (invtype != 'N') {
        var Podt = cdt_partyInvDt.GetValue();
        if (Podt != null) {
            $('#MandatoryPartyDate').attr('style', 'display:none');
            var sdate = cdt_partyInvDt.GetValue();
            var edate = cPLQuoteDate.GetValue();

            var startDate = new Date(sdate);
            var endDate = new Date(edate);
            if (startDate > endDate) {
                $('#MandatoryEgSDate').attr('style', 'display:block');
            }
            else { $('#MandatoryEgSDate').attr('style', 'display:none'); }
        }
        else {
            $('#MandatoryPartyDate').attr('style', 'display:none');
        }
    }
    else {

    }

}




    var Pre_Quantity = "0";
var Pre_Amt = "0";
var Pre_TotalAmt = "0";
var Cur_Quantity = "0";
var Cur_Amt = "0";
var Cur_TotalAmt = "0";

function CalculateAmount() {
    var Quantity = (parseFloat((cTotalQty.GetValue()).toString())).toFixed(2);
    var Amount = (parseFloat((cTaxableAmtval.GetValue()).toString())).toFixed(2);
    var TotalAmount = (parseFloat((cInvValue.GetValue()).toString())).toFixed(2);
    var ChargesAmount = (ctxt_Charges.GetValue() != null) ? (parseFloat(ctxt_Charges.GetValue())).toFixed(2) : "0";

    var Calculate_Quantity = (parseFloat(Quantity) + parseFloat(Cur_Quantity) - parseFloat(Pre_Quantity)).toFixed(2);
    var Calculate_Amount = (parseFloat(Amount) + parseFloat(Cur_Amt) - parseFloat(Pre_Amt)).toFixed(2);
    var Calculate_TotalAmount = (parseFloat(TotalAmount) + parseFloat(Cur_TotalAmt) - parseFloat(Pre_TotalAmt)).toFixed(2);
    var Calculate_TaxAmount = (parseFloat(Calculate_TotalAmount) - parseFloat(Calculate_Amount)).toFixed(2);
    var Calculate_SumAmount = (parseFloat(Calculate_TotalAmount) + parseFloat(ChargesAmount)).toFixed(2);

    cTotalQty.SetValue(Calculate_Quantity);
    cTaxableAmtval.SetValue(Calculate_Amount);
    cTaxAmtval.SetValue(Calculate_TaxAmount);
    cOtherTaxAmtval.SetValue(ChargesAmount);
    cInvValue.SetValue(Calculate_TotalAmount);
    cTotalAmt.SetValue(Calculate_SumAmount);
}




    $(document).ready(function () {
        $('.number').keypress(function (event) {
            if (event.which < 46 || event.which > 59) {
                event.preventDefault();
            } // prevent if not number/dot

            if (event.which == 46 && $(this).val().indexOf('.') != -1) {
                event.preventDefault();
            } // prevent if already dot
        });
    });



    var PreviousCurrency = "1";
function GetPreviousCurrency() {
    PreviousCurrency = ctxtRate.GetValue();
}
$(document).ready(function () {
    OnAddNewClick_Default();
});
function OnAddNewClick_Default() {
    grid.AddNewRow();

    var noofvisiblerows = grid.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
    var tbQuotation = grid.GetEditor("SrlNo");
    tbQuotation.SetValue(noofvisiblerows);
}

function showhide(obj) {
    if (obj == 'Y') {
        $('#divselectunselect').addClass('hide');
    }
    else {
        $('#divselectunselect').removeClass('hide');
    }
}
function GridProductBind(e) {
    var invtype = $('#ddlInventory').val();
    var noofvisiblerows = grid.GetVisibleRowsOnPage();
    if (noofvisiblerows == '0') {
        grid.AddNewRow();
        var tbQuotation = grid.GetEditor("SrlNo");
        tbQuotation.SetValue('1');
    }
    cproductPanel.PerformCallback(invtype);
}


    $(document).ready(function () {
        var mode = $('#hdnADDEditMode').val();
        if (mode == 'Edit') {

            $("#rdl_PurchaseInvoice").find('input').prop('disabled', true);
            if ($('#hdnTDSShoworNot').val() == 'S') {
                $('#divTdsScheme').removeClass('hide');
            }
            else if ($('#hdnTDSShoworNot').val() == 'H') {
                $('#divTdsScheme').addClass('hide');
            }

            if ($('#ddlInventory').val() == 'N') {
                $('#spVehTC').addClass('hide');
            }
            else {
                //$('#spVehTC').addClass('hide');
            }
        }
        else {
            $("#rdl_PurchaseInvoice").find('input').prop('disabled', false);
        }
    })

//<%-- UDF and Transport Section Start--%>

    var canCallBack = true;
function AllControlInitilize() {
    if (canCallBack) {
        if ($('#hdnADDEditMode').val() == 'Edit') {
            //  cchk_reversemechenism.SetEnabled(false);
            if (cchk_reversemechenism.GetValue()) {
                $('#divreverse').removeClass('hide');
                grid.GetEditor('TaxAmount').SetEnabled(false);
            }
        }


        RCMCheckChanged();

        canCallBack = false;
    }
}






function acbpCrpUdfEndCall(s, e) {
    var result = 0;
    if (cacbpCrpUdf.cpUDF == "true") {
        result = 1;
    }
    else {
        jAlert("UDF is set as Mandatory. Please enter values.", "Alert", function () { OpenUdf(); });
        cacbpCrpUdf.cpUDF = null;
        cacbpCrpUdf.cpTransport = null;
        cacbpCrpUdf.cpTC = null;
        LoadingPanel.Hide();
        $('#hdnRefreshType').val('');
        result = 0;
        return;
    }
    if (cacbpCrpUdf.cpTransport == "true") {
        result = 1;
    }
    
var invtype = $('#ddlInventory').val();
if (invtype != 'N') {
    if (cacbpCrpUdf.cpTC == "true") {
        result = 1;
    }
    else {
        jAlert("Terms and Condition is set as Mandatory. Please enter values.", "Alert", function () { $("#TermsConditionseModal").modal('show'); });
        cacbpCrpUdf.cpUDF = null;
        cacbpCrpUdf.cpTransport = null;
        cacbpCrpUdf.cpTC = null;
        LoadingPanel.Hide();
        $('#hdnRefreshType').val('');
        result = 0;
        return;
    }
}
else {
    result = 1;
}
if (cacbpCrpUdf.cpPartyno == "N") {
    result = 1;
}
else {
    jAlert("Party Invoice No. already exist for the selected Transporter.", "Alert", function () { });
    cacbpCrpUdf.cpUDF = null;
    cacbpCrpUdf.cpTransport = null;
    cacbpCrpUdf.cpTC = null;
    LoadingPanel.Hide();
    $('#hdnRefreshType').val('');
    result = 0;
    return;
}

if (cacbpCrpUdf.cpStateId == "Y") {
    result = 1;
    FinalSaveUpdate();
}
else {

    LoadingPanel.Hide();
    FinalSaveUpdate();
    }
}

function FinalSaveUpdate() {
    OnAddNewClick_Default();
    grid.AddNewRow();
    grid.UpdateEdit();
    cacbpCrpUdf.cpUDF = null;
    cacbpCrpUdf.cpTransport = null;
    cacbpCrpUdf.cpTC = null;
}
function gridFocusedRowChanged(s, e) {
    globalRowIndex = e.visibleIndex;
}

//<%--UDF and Transport Section End--%>

    function HideSelectAllSection() {
        if (cgridproducts.cpSelectHide != null) {
            if (cgridproducts.cpSelectHide == 'Y') {
                $('#divselectunselect').addClass('hide')
            }
            else if (cgridproducts.cpSelectHide == 'N') {
                $('#divselectunselect').removeClass('hide')
            }
            else {
                $('#divselectunselect').removeClass('hide')
            }
            cgridproducts.cpSelectHide == null;
        }

        if (cgridproducts.cppartydetail != null) {
            var invtype = $('#ddlInventory').val();
            var partyno = cgridproducts.cppartydetail.toString().split('~')[0];
            var partydate = cgridproducts.cppartydetail.toString().split('~')[1];
            var docdate = cgridproducts.cppartydetail.toString().split('~')[2];
            var modetype = $('#hdnADDEditMode').val();
            if (modetype != 'Edit') {
                ctxt_partyInvNo.SetText(partyno);
                if (invtype != 'N') {
                    if (partyno != null && partyno != '') {
                        $("#MandatorysPartyinvno").hide();
                    }
                    else {
                        $("#MandatorysPartyinvno").show();
                    }
                }
                else {
                    $("#MandatorysPartyinvno").hide();
                }
                if (invtype != 'N') {

                    if (partydate != null && partydate != '') {
                        cdt_partyInvDt.SetText(partydate);
                        $('#MandatoryPartyDate').attr('style', 'display:none');
                    } else {
                        $('#MandatoryPartyDate').attr('style', 'display:block');
                    }
                }
                else {
                    $('#MandatoryPartyDate').attr('style', 'display:none');
                }
            }
            var reff = cgridproducts.cppartydetail.toString().split('~')[3];
            var curr = cgridproducts.cppartydetail.toString().split('~')[4];
            var rate = cgridproducts.cppartydetail.toString().split('~')[5];
            var person = cgridproducts.cppartydetail.toString().split('~')[6];
            var amtare = cgridproducts.cppartydetail.toString().split('~')[7];
            var taxcode = cgridproducts.cppartydetail.toString().split('~')[8];
            cPLQADate.SetText(docdate);
            cddl_AmountAre.SetEnabled(false);
            if (reff != '') {
                ctxt_Refference.SetText(reff);
            }
            if (person != '') {

                cContactPerson.SetValue(person);
            }
            if (curr != '') {
                $("#ddl_Currency").val(curr);
            }
            if (rate != '') {
                ctxtRate.SetText(rate);
            }
            if (amtare != '') {
                cddl_AmountAre.SetValue(amtare);
            }
            if (taxcode != '') {
                cddlVatGstCst.PerformCallback('SetTaxCode' + '~' + taxcode)
                var items = $('#cddlVatGstCst option').length;
                cddlVatGstCst.SetValue(taxcode);
            }
            cgridproducts.cppartydetail = null;
        }
    }
    //<%--Div Detail for Vendor Section Start--%>
    function acpContactPersonPhoneEndCall(s, e) {
        if (cacpContactPersonPhone.cpPhone != null && cacpContactPersonPhone.cpPhone != undefined) {
            pageheaderContent.style.display = "block";
            $("#divContactPhone").attr('style', 'display:block');
            document.getElementById('lblContactPhone').innerHTML = cacpContactPersonPhone.cpPhone;
            cacpContactPersonPhone.cpPhone = null;

    }
    else {
            $("#divContactPhone").attr('style', 'display:none');
            document.getElementById('lblContactPhone').innerHTML = '';
            cacpContactPersonPhone.cpPhone = null;
    }
    }
    function GetContactPersonPhone(e) {
        var key = cContactPerson.GetValue();
        cacpContactPersonPhone.PerformCallback('ContactPersonPhone~' + key);
    }
        function cmbContactPersonEndCall(s, e) {
            if (cContactPerson.cpContactdtl != null && cContactPerson.cpContactdtl != undefined) {
                if (cContactPerson.cpContactdtl == 'Y') {
                    $("#pageheaderContent").attr('style', 'display:block');
                    $("#divContactPhone").attr('style', 'display:block');
                   
                    }
                else {
                    $("#divContactPhone").attr('style', 'display:none');
                    document.getElementById('lblContactPhone').innerHTML = '';
                }
                cContactPerson.cpContactdtl = null;
            }
            else {
                $("#divContactPhone").attr('style', 'display:none');
                document.getElementById('lblContactPhone').innerHTML = '';
            }
            var edate = cPLQuoteDate.GetValue();
            var str = $.datepicker.formatDate('yy-mm-dd', edate);
            if ((cContactPerson.cpGSTN != null && cContactPerson.cpGSTN != undefined) &&
                (cContactPerson.cpvendortype != null && cContactPerson.cpvendortype != undefined)) {


                $("#pageheaderContent").attr('style', 'display:block');
                $("#divGSTN").attr('style', 'display:block');
                document.getElementById('lblGSTIN').innerHTML = cContactPerson.cpGSTN + ' (' + cContactPerson.cpvendortype + ')';
                if (cContactPerson.cpGSTN == 'Yes' && cContactPerson.cpvendortype == 'Regular') {
                    cddl_vendortype.SetValue('R');
                    // Sandip for Terms & Condition Checking where it is mandatory or not is pending  Section Start
                    $('#pnl_TCspecefiFields_PO').css('display', 'none')
                    $('#pnl_TCspecefiFields_Not_PO').css('display', 'block')
                    // Sandip for Terms & Condition Checking where it is mandatory or not Pending Section End
                    var invtype = $('#ddlInventory').val();
                    if (invtype == 'Y' || invtype == 'B' || invtype == 'C') {

                        $('#rdlbutton').removeClass('hide');
                        $('#rdldate').removeClass('hide');
                    }
                    else if (invtype == 'N') {


                        $('#rdlbutton').addClass('hide');
                        $('#rdldate').addClass('hide');
                    }
                    // Code Commented by Sam on 01022018 Section Start
                    //cchk_reversemechenism.SetValue(false); //RCM ticked by default in Transporter bill entry	
                    //riju $('#divreverse').addClass('hide');
                    // Code Commented by Sam on 01022018 Section End


                    // cddl_AmountAre.SetValue(1);
                    PopulateGSTCSTVAT();
                    // cddl_AmountAre.SetEnabled(true);
                }
                else if (cContactPerson.cpGSTN == 'Yes' && cContactPerson.cpvendortype == 'Composite') {
                    // Sandip for Terms & Condition Checking where it is mandatory or not is pending  Section Start
                    $('#pnl_TCspecefiFields_PO').css('display', 'none')
                    $('#pnl_TCspecefiFields_Not_PO').css('display', 'block')
                    // Sandip for Terms & Condition Checking where it is mandatory or not Pending Section Start
                    cddl_vendortype.SetValue('C');
                    var invtype = $('#ddlInventory').val();
                    if (invtype == 'Y' || invtype == 'B' || invtype == 'C') {
                        $('#rdlbutton').removeClass('hide');
                        $('#rdldate').removeClass('hide');
                    }
                    else if (invtype == 'N') {
                        $('#rdlbutton').addClass('hide');
                        $('#rdldate').addClass('hide');
                    }
                    // Code Commented by Sam on 01022018 Section Start
                    //cchk_reversemechenism.SetValue(false); //RCM ticked by default in Transporter bill entry	
                    //riju $('#divreverse').addClass('hide');
                    // Code Commented by Sam on 01022018 Section End

                    cddl_AmountAre.SetValue(3);
                    PopulateGSTCSTVAT();
                    cddl_AmountAre.SetEnabled(false);
                }
                else {
                    if (cContactPerson.cpcountry != null && cContactPerson.cpcountry != '') {
                        if (cContactPerson.cpcountry != '1') {

                            // Code Commented by Sam on 01022018 Section Start
                            //cchk_reversemechenism.SetValue(false);  //Commented  //RCM ticked by default in Transporter bill entry	
                            //riju cchk_reversemechenism.SetEnabled(false) // Commented
                            // Code Commented by Sam on 01022018 Section End

                            $('#rdlbutton').removeClass('hide');
                            $('#rdldate').removeClass('hide');
                            cddl_AmountAre.SetValue(4);
                            $('#hfTCspecefiFieldsVisibilityCheck').val('1');
                            cContactPerson.cpcountry == null
                            $('#pnl_TCspecefiFields_PO').css('display', 'block')
                            $('#pnl_TCspecefiFields_Not_PO').css('display', 'none')
                            //pnl_TCspecefiFields_PO.Style.Add(HtmlTextWriterStyle.Display, "block");
                            //pnl_TCspecefiFields_Not_PO.Style.Add(HtmlTextWriterStyle.Display, "none");
                        }
                        else {
                            $('#hfTCspecefiFieldsVisibilityCheck').val('');
                            var RB = document.getElementById("rdl_PurchaseInvoice");
                            if (RB.rows.length > 0) {
                                for (i = 0; i < RB.rows.length; i++) {
                                    var cell = RB.rows[i].cells;
                                    for (j = 0; j < cell.length; j++) {
                                        if (cell[j].childNodes[0].type == "radio") {
                                            document.getElementById(cell[j].childNodes[0].id).checked = false;
                                        }
                                    }
                                }
                            }
                            $('#rdlbutton').addClass('hide');
                            $('#rdldate').addClass('hide');
                    // Waiting for Dirction  End
                    cddl_AmountAre.SetValue(3);
                    cContactPerson.cpcountry == null
                    $('#pnl_TCspecefiFields_PO').css('display', 'none')
                    $('#pnl_TCspecefiFields_Not_PO').css('display', 'block')
                }

            }
            else {
                $('#hfTCspecefiFieldsVisibilityCheck').val('');
                var RB = document.getElementById("rdl_PurchaseInvoice");
                if (RB.rows.length > 0) {
                    for (i = 0; i < RB.rows.length; i++) {
                        var cell = RB.rows[i].cells;
                        for (j = 0; j < cell.length; j++) {
                            if (cell[j].childNodes[0].type == "radio") {
                                document.getElementById(cell[j].childNodes[0].id).checked = false;
                            }
                        }
                    }
                }
                $('#rdlbutton').addClass('hide');
                $('#rdldate').addClass('hide');
// Waiting for Dirction  End
 cddl_AmountAre.SetValue(3);
cContactPerson.cpcountry == null
$('#pnl_TCspecefiFields_PO').css('display', 'none')
$('#pnl_TCspecefiFields_Not_PO').css('display', 'block')
}
PopulateGSTCSTVAT();
cddl_AmountAre.SetEnabled(false);
}
cContactPerson.cpGSTN = null;
}
else {
    $("#divGSTN").attr('style', 'display:none');
    document.getElementById('lblGSTIN').innerHTML = '';
    cContactPerson.cpGSTN = null;
}
if (cContactPerson.cpOutstanding != null && cContactPerson.cpOutstanding != undefined) {
    $("#pageheaderContent").attr('style', 'display:block');
    $("#divOutstanding").attr('style', 'display:block');
    document.getElementById('lblOutstanding').innerHTML = cContactPerson.cpOutstanding;
    cContactPerson.cpOutstanding = null;
}
else {
    $("#divOutstanding").attr('style', 'display:none');
    document.getElementById('lblOutstanding').innerHTML = '';
}
ctxt_partyInvNo.Focus();
}


        var globalRowIndex;
var rowEditCtrl;

var ProductGetQuantity = "0";
var ProductGetTotalAmount = "0";
var ProductPurchaseprice = "0";
var ProductDiscount = "0";
var ProductDiscountAmt = "0";
var ProductGrsAmt = "0";
// Running Calculation As New Modification
var globalNetAmount = 0;
function DiscountGotChange() {
    globalNetAmount = parseFloat(grid.GetEditor("TotalAmount").GetValue());
    ProductGetTotalAmount = globalNetAmount;
    ProductDiscount = (grid.GetEditor('Discount').GetValue() != null) ? grid.GetEditor('Discount').GetValue() : "0";
    ProductGetQuantity = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
    ProductPurchaseprice = (grid.GetEditor('PurchasePrice').GetValue() != null) ? grid.GetEditor('PurchasePrice').GetValue() : "0";
    // Running total Calculation Start 
    Pre_Quantity = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
    Pre_Amt = (grid.GetEditor('Amount').GetValue() != null) ? grid.GetEditor('Amount').GetValue() : "0";
    Pre_TotalAmt = (grid.GetEditor('TotalAmount').GetValue() != null) ? grid.GetEditor('TotalAmount').GetValue() : "0";
    // Running total Calculation End
}
function DiscountAmtGotChange() {
    globalNetAmount = parseFloat(grid.GetEditor("TotalAmount").GetValue());
    ProductGetTotalAmount = globalNetAmount;
    ProductGetQuantity = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
    ProductPurchaseprice = (grid.GetEditor('PurchasePrice').GetValue() != null) ? grid.GetEditor('PurchasePrice').GetValue() : "0";
    ProductDiscount = (grid.GetEditor('Discount').GetValue() != null) ? grid.GetEditor('Discount').GetValue() : "0";
    ProductDiscountAmt = (grid.GetEditor('Discountamt').GetValue() != null) ? grid.GetEditor('Discountamt').GetValue() : "0";
    // Running total Calculation Start 
    Pre_Quantity = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
    Pre_Amt = (grid.GetEditor('Amount').GetValue() != null) ? grid.GetEditor('Amount').GetValue() : "0";
    Pre_TotalAmt = (grid.GetEditor('TotalAmount').GetValue() != null) ? grid.GetEditor('TotalAmount').GetValue() : "0";
    // Running total Calculation End
}

function AmtGotFocus() {
    globalNetAmount = parseFloat(grid.GetEditor("TotalAmount").GetValue());
    ProductGetTotalAmount = globalNetAmount;
    ProductGetQuantity = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
    ProductPurchaseprice = (grid.GetEditor('PurchasePrice').GetValue() != null) ? grid.GetEditor('PurchasePrice').GetValue() : "0";
    ProductDiscount = (grid.GetEditor('Discount').GetValue() != null) ? grid.GetEditor('Discount').GetValue() : "0";
    ProductDiscountAmt = (grid.GetEditor('Discountamt').GetValue() != null) ? grid.GetEditor('Discountamt').GetValue() : "0";
    ProductGrsAmt = (grid.GetEditor('Amount').GetValue() != null) ? grid.GetEditor('Amount').GetValue() : "0";

    // Running total Calculation Start

    Pre_Quantity = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
    Pre_Amt = (grid.GetEditor('Amount').GetValue() != null) ? grid.GetEditor('Amount').GetValue() : "0";
    Pre_TotalAmt = (grid.GetEditor('TotalAmount').GetValue() != null) ? grid.GetEditor('TotalAmount').GetValue() : "0";

    // Running total Calculation End
}

// Running Calculation As New Modification


// To set Quantity column false if tagging is available by Sam on 10062017 Start   
function QuantityGotFocus(s, e) {
    var taxqty = grid.GetEditor("Quantity").GetValue();
    
    var type = ($("[id$='rdl_PurchaseInvoice']").find(":checked").val() != null) ? $("[id$='rdl_PurchaseInvoice']").find(":checked").val() : "";
    if (type == 'PC') {
        grid.GetEditor("Quantity").SetEnabled(false);
    }
    else {
        grid.GetEditor("Quantity").SetEnabled(true);
    }
    var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
    ProductGetQuantity = QuantityValue;
    globalNetAmount = parseFloat(grid.GetEditor("TotalAmount").GetValue());
    // Running total Calculation Start 
    Pre_Quantity = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
    Pre_Amt = (grid.GetEditor('Amount').GetValue() != null) ? grid.GetEditor('Amount').GetValue() : "0";
    Pre_TotalAmt = (grid.GetEditor('TotalAmount').GetValue() != null) ? grid.GetEditor('TotalAmount').GetValue() : "0";
    // Running total Calculation End
}
// To set Quantity column false if tagging is available by Sam on 10062017 End

function PurPriceGotFocus() {
    ProductPurchaseprice = grid.GetEditor("PurchasePrice").GetValue();
    globalNetAmount = parseFloat(grid.GetEditor("TotalAmount").GetValue());
    // Running total Calculation Start 
    Pre_Quantity = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
    Pre_Amt = (grid.GetEditor('Amount').GetValue() != null) ? grid.GetEditor('Amount').GetValue() : "0";
    Pre_TotalAmt = (grid.GetEditor('TotalAmount').GetValue() != null) ? grid.GetEditor('TotalAmount').GetValue() : "0";
    // Running total Calculation End 
}
//............................Product Pazination..............
function ChangeState(value) {
    cgridproducts.PerformCallback('SelectAndDeSelectProducts' + '~' + value);
}
function ctaxUpdatePanelEndCall(s, e) {
    if (ctaxUpdatePanel.cpstock != null) {
        divAvailableStk.style.display = "block";
        var AvlStk = ctaxUpdatePanel.cpstock + " " + document.getElementById('lblStkUOM').innerHTML;
        document.getElementById('lblAvailableStkPro').innerHTML = AvlStk;
        document.getElementById('lblAvailableStk').innerHTML = ctaxUpdatePanel.cpstock;
        ctaxUpdatePanel.cpstock = null;
        grid.batchEditApi.StartEdit(globalRowIndex, 5);
    }
    return false;
}
function ProductsGotFocusFromID(s, e) {
    //var customerval = gridLookup.GetValue() 
    var customerval = GetObjectID('hdnCustomerId').value;
    if ($('#txtVoucherNo').val() == '' || $('#txtVoucherNo').val() == null) {
        jAlert('Select a numbering schema first.')
        $('#ddl_numberingScheme').focus();
        return false;
    }
    else if (customerval == '' || customerval == null || customerval == "") {
        jAlert('Select a Transporter first')
        //gridLookup.Focus();
        ctxtVendorName.Focus();
        return false;
    }
    else {
        pageheaderContent.style.display = "block";
        var ProductID = (grid.GetEditor('ProductID').GetValue() != null) ? grid.GetEditor('ProductID').GetValue() : "0";
        var strProductName = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
        var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
        var ddlbranch = $("[id*=ddl_Branch]");
        var strBranch = ddlbranch.find("option:selected").text();
        var SpliteDetails = ProductID.split("||@||");
        var strProductID = SpliteDetails[0];
        var strDescription = SpliteDetails[1];
        var strUOM = SpliteDetails[2];
        var strStkUOM = SpliteDetails[4];
        var strSalePrice = SpliteDetails[6];
        var IsPackingActive = SpliteDetails[13];
        var Packing_Factor = SpliteDetails[14];
        var Packing_UOM = SpliteDetails[15];
        var PackingValue = (Packing_Factor * QuantityValue) + " " + Packing_UOM;
        if (IsPackingActive == "Y" && (parseFloat(Packing_Factor * QuantityValue) > 0)) {
            $('#lblPackingStk').text(PackingValue);
        } else {
            divPacking.style.display = "none";
        }
        $('#lblStkQty').text(QuantityValue);
        $('#lblStkUOM').text(strStkUOM);
        $('#lblProduct').text(strProductName);
        $('#lblbranchName').text(strBranch);
        if (ProductID != "0") {
            cacpAvailableStock.PerformCallback(strProductID);
        }
    }
}
function ProductlookUpKeyDown(s, e) {
    if (e.htmlEvent.key == "Escape") {
        cProductpopUp.Hide();
        grid.batchEditApi.StartEdit(globalRowIndex, 6);
    }
}
function ProductSelected(s, e) {
    if (!cproductLookUp.FindItemByValue(cproductLookUp.GetValue())) {
        cProductpopUp.Hide();
        grid.batchEditApi.StartEdit(globalRowIndex, 4);
        jAlert("Product not Exists.", "Alert", function () { cproductLookUp.SetValue(); cproductLookUp.Focus(); });
        return;
    }
    var LookUpData = cproductLookUp.GetValue();
    var ProductCode = cproductLookUp.GetText();
    cProductpopUp.Hide();
    grid.batchEditApi.StartEdit(globalRowIndex);
    grid.GetEditor("ProductID").SetText(LookUpData);
    grid.GetEditor("ProductName").SetText(ProductCode);
    pageheaderContent.style.display = "block";
    cddl_AmountAre.SetEnabled(false);
    cddl_TdsScheme.SetEnabled(false);
    $('#ddlInventory').prop('disabled', true);
    var tbDescription = grid.GetEditor("Description");
    var tbUOM = grid.GetEditor("UOM");
    var tbSalePrice = grid.GetEditor("PurchasePrice");
    var ProductID = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
    var SpliteDetails = ProductID.split("||@||");
    var strProductID = SpliteDetails[0];
    var strDescription = SpliteDetails[1];
    var strUOM = SpliteDetails[2];
    var strStkUOM = SpliteDetails[4];
    var strSalePrice = SpliteDetails[6];
    var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
    var IsPackingActive = SpliteDetails[10];
    var Packing_Factor = SpliteDetails[11];
    var Packing_UOM = SpliteDetails[12];
    var strRate = (ctxtRate.GetValue() != null && ctxtRate.GetValue() != "0") ? ctxtRate.GetValue() : "1";
    if (strRate == 0) {
        strSalePrice = strSalePrice;
    }
    else {
        strSalePrice = strSalePrice / strRate;
    }
    tbDescription.SetValue(strDescription);
    tbUOM.SetValue(strUOM);
    tbSalePrice.SetValue(strSalePrice);
    grid.GetEditor("Quantity").SetValue("0.00");
    grid.GetEditor("Discount").SetValue("0.00");
    grid.GetEditor("Amount").SetValue("0.00");
    grid.GetEditor("TaxAmount").SetValue("0.00");
    grid.GetEditor("TotalAmount").SetValue("0.00");
    var ddlbranch = $("[id*=ddl_Branch]");
    var strBranch = ddlbranch.find("option:selected").text();
    $('#lblStkQty').text("0.00");
    $('#lblStkUOM').text(strStkUOM);
    $('#lblProduct').text(strDescription);
    $('#lblbranchName').text(strBranch);
    if (IsPackingActive == "Y" && (parseFloat(Packing_Factor * QuantityValue) > 0)) {
        $('#lblPackingStk').text(PackingValue);
    } else {
        divPacking.style.display = "none";
    }
    // Running total Calculation Start
    Cur_Quantity = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
    Cur_Amt = (grid.GetEditor('Amount').GetValue() != null) ? grid.GetEditor('Amount').GetValue() : "0";
    Cur_TotalAmt = (grid.GetEditor('TotalAmount').GetValue() != null) ? grid.GetEditor('TotalAmount').GetValue() : "0";
    CalculateAmount();
    // Running total Calculation End

    //Debjyoti
    ctaxUpdatePanel.PerformCallback('DelProdbySl~' + grid.GetEditor("SrlNo").GetValue() + '~' + strProductID);
    grid.batchEditApi.StartEdit(globalRowIndex, 6);
}
function ProductKeyDown(s, e) {
    if (e.htmlEvent.key == "Enter") {
        s.OnButtonClick(0);
    }
    if (e.htmlEvent.key == "Tab") {
        s.OnButtonClick(0);
    }
}
//function ProductButnClick(s, e) {
//    if (e.buttonIndex == 0) {
//        //var customerval = gridLookup.GetValue();
//        var customerval = GetObjectID('hdnCustomerId').value;
//        if ($('#txtVoucherNo').val() == '' || $('#txtVoucherNo').val() == null) {
//            jAlert('Select a numbering schema first.')
//            $('#ddl_numberingScheme').focus();
//            return false;
//        }
//        else if (customerval == '' || customerval == null || customerval == "") { 
//            jAlert('Select a Vendor first')
//            //gridLookup.Focus();
//            ctxtVendorName.Focus();
//            return false;
//        }
//        else if (cproductLookUp.Clear()) { 
//            // Running total Calculation Start 
//            Pre_Quantity = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
//            Pre_Amt = (grid.GetEditor('Amount').GetValue() != null) ? grid.GetEditor('Amount').GetValue() : "0";
//            Pre_TotalAmt = (grid.GetEditor('TotalAmount').GetValue() != null) ? grid.GetEditor('TotalAmount').GetValue() : "0"; 
//            // Running total Calculation End
//            cProductpopUp.Show();
//            cproductLookUp.Focus();
//            cproductLookUp.ShowDropDown();
//        }
//    }
//}
//..............End Product........................
//.............Available Stock Div Show............................
function ProductsGotFocus(s, e) {
    pageheaderContent.style.display = "block";
    var ProductID = (grid.GetEditor('ProductID').GetValue() != null) ? grid.GetEditor('ProductID').GetValue() : "0";
    var strProductName = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
    var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
    var ddlbranch = $("[id*=ddl_Branch]");
    var strBranch = ddlbranch.find("option:selected").text();
    var SpliteDetails = ProductID.split("||@||");
    var strProductID = SpliteDetails[0];
    var strDescription = SpliteDetails[1];
    var strUOM = SpliteDetails[2];
    var strStkUOM = SpliteDetails[4];
    var strSalePrice = SpliteDetails[6];
    var IsPackingActive = SpliteDetails[13];
    var Packing_Factor = SpliteDetails[14];
    var Packing_UOM = SpliteDetails[15];
    var PackingValue = (Packing_Factor * QuantityValue) + " " + Packing_UOM;
    if (IsPackingActive == "Y" && (parseFloat(Packing_Factor * QuantityValue) > 0)) {
        $('#lblPackingStk').text(PackingValue);
        //  divPacking.style.display = "block";
    } else {
        divPacking.style.display = "none";
    }
    $('#lblStkQty').text(QuantityValue);
    $('#lblStkUOM').text(strStkUOM);
    $('#lblProduct').text(strProductName);
    $('#lblbranchName').text(strBranch);
    if (ProductID != "0") {
        cacpAvailableStock.PerformCallback(strProductID);
    }
}
function acpAvailableStockEndCall(s, e) {
    if (cacpAvailableStock.cpstock != null) {
        divAvailableStk.style.display = "block";
        var AvlStk = cacpAvailableStock.cpstock + " " + document.getElementById('lblStkUOM').innerHTML;
        document.getElementById('lblAvailableStkPro').innerHTML = AvlStk;
        document.getElementById('lblAvailableStk').innerHTML = cacpAvailableStock.cpstock;
        cCmbWarehouse.cpstock = null;
    }
}
//................Available Stock Div Show....................
//Code for UDF Control 
function OpenUdf(s, e) {
    if (document.getElementById('IsUdfpresent').value == '0') {
        jAlert("UDF not define.");
    }
    else {
        var keyVal = document.getElementById('Keyval_internalId').value;
        var url = '/OMS/management/Master/frm_BranchUdfPopUp.aspx?Type=PB&&KeyVal_InternalID=' + keyVal;
        popup.SetContentUrl(url);
        popup.Show();
    }
    return true;
}
// End Udf Code 
//..................Rate........................
function ReBindGrid_Currency() {
    var frontRow = 0;
    var backRow = -1;
    var IsProduct = "";
    for (var i = 0; i <= grid.GetVisibleRowsOnPage() ; i++) {
        var frontProduct = (grid.batchEditApi.GetCellValue(backRow, 'ProductID') != null) ? (grid.batchEditApi.GetCellValue(backRow, 'ProductID')) : "";
        var backProduct = (grid.batchEditApi.GetCellValue(frontRow, 'ProductID') != null) ? (grid.batchEditApi.GetCellValue(frontRow, 'ProductID')) : "";
        if (frontProduct != "" || backProduct != "") {
            IsProduct = "Y";
            break;
        }
        backRow--;
        frontRow++;
    }
    if (IsProduct == "Y") {
        $('#hdfIsDelete').val('D');
        grid.UpdateEdit();
        grid.PerformCallback('CurrencyChangeDisplay~' + PreviousCurrency);
        //grid.PerformCallback('CurrencyChangeDisplay');
    }
}
//...............end.........................
//...............PopulateVAT........................
function PopulateGSTCSTVAT(e) {
    var key = cddl_AmountAre.GetValue();
    if (key == 1 || key == 4) {
        grid.GetEditor('TaxAmount').SetEnabled(true);
        cddlVatGstCst.SetEnabled(false);
        cddlVatGstCst.SetSelectedIndex(-1);
        cbtn_SaveTaxesRecords.SetVisible(true);
        grid.GetEditor('ProductID').Focus();
    }
    else if (key == 2) {
        grid.GetEditor('TaxAmount').SetEnabled(true);
        cddlVatGstCst.SetEnabled(true);
        cddlVatGstCst.PerformCallback('2');
        cddlVatGstCst.Focus();
        cbtn_SaveTaxesRecords.SetVisible(true);
    }
    else if (key == 3) {
        grid.GetEditor('TaxAmount').SetEnabled(false);
        cddlVatGstCst.SetSelectedIndex(-1);
        cddlVatGstCst.SetEnabled(false);
        cbtn_SaveTaxesRecords.SetVisible(false);
    }
}
function Keypressevt() {
    if (event.keyCode == 13) {
        //run code for Ctrl+X -- ie, Save & Exit! 
        SaveWarehouse();
        return false;
    }
}

function SetFocusonDemand(e) {
    var key = cddl_AmountAre.GetValue();
    if (key == '1' || key == '3') {
        if (grid.GetVisibleRowsOnPage() == 1) {
            grid.batchEditApi.StartEdit(-1, 2);
        }
    }
    else if (key == '2') {
        cddlVatGstCst.Focus();
    }
}

//.................End PopulateVAT...............
//................Amount Calculation.........................
function TaxAmountKeyDown(s, e) {
    if (e.htmlEvent.key == "Enter") {
        s.OnButtonClick(0);
    }
}
function taxAmtButnClick1(s, e) {


    var taxtype = cddl_AmountAre.GetValue();
    var Vendortype = cddl_vendortype.GetValue();
    if (taxtype == '3' || Vendortype == 'C') {
        grid.GetEditor('TaxAmount').SetEnabled(false);
    }
    else {
        grid.GetEditor("TaxAmount").SetEnabled(true);
    }

    rowEditCtrl = s;
}
function taxAmtButnClick(s, e) {
    if (e.buttonIndex == 0) {
        if (cddl_AmountAre.GetValue() != null) {
            var ProductID = (grid.GetEditor('ProductID').GetValue() != null) ? grid.GetEditor('ProductID').GetValue() : "";
            if (ProductID.trim() != "") {

                // Running total Calculation Start

                Pre_Quantity = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
                Pre_Amt = (grid.GetEditor('Amount').GetValue() != null) ? grid.GetEditor('Amount').GetValue() : "0";
                Pre_TotalAmt = (grid.GetEditor('TotalAmount').GetValue() != null) ? grid.GetEditor('TotalAmount').GetValue() : "0";

                // Running total Calculation End

                document.getElementById('setCurrentProdCode').value = ProductID.split('||')[0];
                document.getElementById('HdSerialNo').value = grid.GetEditor('SrlNo').GetText();
                ctxtTaxTotAmt.SetValue(0);
                ccmbGstCstVat.SetSelectedIndex(0);
                $('.RecalculateInline').hide();
                caspxTaxpopUp.Show();
                //Set Product Gross Amount and Net Amount 
                var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
                var SpliteDetails = ProductID.split("||@||");
                var strMultiplier = SpliteDetails[7];
                var strFactor = SpliteDetails[8];
                var strRate = (ctxtRate.GetValue() != null && ctxtRate.GetValue() != "0") ? ctxtRate.GetValue() : "1";
                var strStkUOM = SpliteDetails[4];
                var strSalePrice = (grid.GetEditor('PurchasePrice').GetValue() != null) ? grid.GetEditor('PurchasePrice').GetValue() : "";
                if (strRate == 0) {
                    strRate = 1;
                }
                var StockQuantity = strMultiplier * QuantityValue;
                var Amount = parseFloat(Math.round(QuantityValue * strFactor * (strSalePrice / strRate) * 100) / 100).toFixed(2);
                clblTaxProdGrossAmt.SetText(Amount);
                clblProdNetAmt.SetText(grid.GetEditor('Amount').GetValue());
                document.getElementById('HdProdGrossAmt').value = Amount;
                document.getElementById('HdProdNetAmt').value = parseFloat(Math.round(grid.GetEditor('Amount').GetValue() * 100) / 100).toFixed(2);
                //End Here 
                //Set Discount Here
                if (parseFloat(grid.GetEditor('Discount').GetValue()) > 0) {
                    var discount = (Amount * grid.GetEditor('Discount').GetValue() / 100);
                    //var discount = Math.round((Amount * grid.GetEditor('Discount').GetValue() / 100)).toFixed(2);
                    clblTaxDiscount.SetText(discount);
                }
                else {
                    clblTaxDiscount.SetText('0.00');
                }
                //End Here  
                //Checking is gstcstvat will be hidden or not
                if (cddl_AmountAre.GetValue() == "2") {
                    $('.GstCstvatClass').hide();
                    $('.gstGrossAmount').show();
                    clblTaxableGross.SetText("(Taxable)");
                    clblTaxableNet.SetText("(Taxable)");
                    $('.gstNetAmount').hide();
                    //Set Gross Amount with GstValue
                    //Get The rate of Gst
                    var gstRate = parseFloat(cddlVatGstCst.GetValue().split('~')[1]);
                    if (gstRate) {
                        if (gstRate != 0) {
                            var gstDis = (gstRate / 100) + 1;
                            if (cddlVatGstCst.GetValue().split('~')[2] == "G") {
                                $('.gstNetAmount').hide();
                                clblTaxProdGrossAmt.SetText(Math.round(Amount / gstDis).toFixed(2));
                                document.getElementById('HdProdGrossAmt').value = Math.round(Amount / gstDis).toFixed(2);
                                clblGstForGross.SetText(Math.round(Amount - parseFloat(document.getElementById('HdProdGrossAmt').value)).toFixed(2));
                                clblTaxableNet.SetText("");
                            }
                            else {
                                $('.gstGrossAmount').hide();
                                clblProdNetAmt.SetText(Math.round(grid.GetEditor('Amount').GetValue() / gstDis).toFixed(2));
                                document.getElementById('HdProdNetAmt').value = Math.round(grid.GetEditor('Amount').GetValue() / gstDis).toFixed(2);
                                clblGstForNet.SetText(Math.round(grid.GetEditor('Amount').GetValue() - parseFloat(document.getElementById('HdProdNetAmt').value)).toFixed(2));
                                clblTaxableGross.SetText("");
                            }
                        }

                    } else {
                        $('.gstGrossAmount').hide();
                        $('.gstNetAmount').hide();
                        clblTaxableGross.SetText("");
                        clblTaxableNet.SetText("");
                    }
                }
                else if (cddl_AmountAre.GetValue() == "1") {
                    $('.GstCstvatClass').show();
                    $('.gstGrossAmount').hide();
                    $('.gstNetAmount').hide();
                    clblTaxableGross.SetText("");
                    clblTaxableNet.SetText("");
                    //###### Added By : Samrat Roy ##########
                    //Get Customer Shipping StateCode
                    var shippingStCode = '';

                    // Here we are sending Branch StateCode instead of Shipping Statecode after discuss with
                    // Pijush Da and Debjyoti on 14122017
                    //shippingStCode = $("#ucBShfSStateCode").val();

                    //shippingStCode = $("#hdnucSBranchStateCode").val();
                    //shippingStCode = shippingStCode.substring(shippingStCode.lastIndexOf('(')).replace('(State Code:', '').replace(')', '').trim();

                    var shippingStCode = '';
                    shippingStCode = cbsSCmbState.GetText();
                    shippingStCode = shippingStCode.substring(shippingStCode.lastIndexOf('(')).replace('(State Code:', '').replace(')', '').trim();

                    //###### END : Samrat Roy : END ########## 

                    //Debjyoti 09032017
                    if (shippingStCode.trim() != '') {
                        for (var cmbCount = 1; cmbCount < ccmbGstCstVat.GetItemCount() ; cmbCount++) {
                            //Check if gstin is blank then delete all tax
                            if (ccmbGstCstVat.GetItem(cmbCount).value.split('~')[5] != "") {
                                if (ccmbGstCstVat.GetItem(cmbCount).value.split('~')[5] == shippingStCode) {
                                    //if its state is union territories then only UTGST will apply
                                    if (shippingStCode == "4" || shippingStCode == "26" || shippingStCode == "25" || shippingStCode == "35" || shippingStCode == "31" || shippingStCode == "34") {
                                        if (ccmbGstCstVat.GetItem(cmbCount).value.split('~')[4] == 'I' || ccmbGstCstVat.GetItem(cmbCount).value.split('~')[4] == 'C' || ccmbGstCstVat.GetItem(cmbCount).value.split('~')[4] == 'S') {
                                            ccmbGstCstVat.RemoveItem(cmbCount);
                                            cmbCount--;
                                        }
                                    }
                                    else {
                                        if (ccmbGstCstVat.GetItem(cmbCount).value.split('~')[4] == 'I' || ccmbGstCstVat.GetItem(cmbCount).value.split('~')[4] == 'U') {
                                            ccmbGstCstVat.RemoveItem(cmbCount);
                                            cmbCount--;
                                        }
                                    }
                                } else {
                                    if (ccmbGstCstVat.GetItem(cmbCount).value.split('~')[4] == 'S' || ccmbGstCstVat.GetItem(cmbCount).value.split('~')[4] == 'C' || ccmbGstCstVat.GetItem(cmbCount).value.split('~')[4] == 'U') {
                                        ccmbGstCstVat.RemoveItem(cmbCount);
                                        cmbCount--;
                                    }
                                }
                            } else {
                                ccmbGstCstVat.RemoveItem(cmbCount);
                                cmbCount--;
                            }
                        }
                    }

                } else {
                    clblTaxableGross.SetText("");
                    clblTaxableNet.SetText("");

                }
                //End here

                if (globalRowIndex > -1) {
                    cgridTax.PerformCallback(grid.keys[globalRowIndex] + '~' + cddl_AmountAre.GetValue());
                } else {

                    cgridTax.PerformCallback('New~' + cddl_AmountAre.GetValue());
                    //Set default combo
                    cgridTax.cpComboCode = grid.GetEditor('ProductID').GetValue().split('||@||')[9];
                }
                ctxtprodBasicAmt.SetValue(grid.GetEditor('Amount').GetValue());
            } else {
                grid.batchEditApi.StartEdit(globalRowIndex, 13);
            }
        }
    }
}

function SalePriceTextChange(s, e) {
    pageheaderContent.style.display = "block";
    var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
    var ProductID = grid.GetEditor('ProductID').GetValue();
    if (ProductID != null) {
        var SpliteDetails = ProductID.split("||@||");
        var strMultiplier = SpliteDetails[7];//Conversion_Multiplier 
        var strFactor = SpliteDetails[14]; //Packing_Factor 
        var strRate = (ctxtRate.GetValue() != null && ctxtRate.GetValue() != "0") ? ctxtRate.GetValue() : "1";
        var strProductID = SpliteDetails[0];
        var strProductName = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
        var ddlbranch = $("[id*=ddl_Branch]");
        var strBranch = ddlbranch.find("option:selected").text();
        var strStkUOM = SpliteDetails[4];//Stk_UOM_Name
        var strSalePrice = SpliteDetails[6];// purchase Price 
        if (strRate == 0) {
            strRate = 1;
        }
        if (strSalePrice == 0.00000) {
            strSalePrice = 1;
        }
        var StockQuantity = strMultiplier * QuantityValue;
        var Amount = QuantityValue * strFactor * (strSalePrice / strRate);

        $('#lblbranchName').text(strBranch);

        var IsPackingActive = SpliteDetails[13];//IsPackingActive
        var Packing_Factor = SpliteDetails[14];//Packing_Factor
        var Packing_UOM = SpliteDetails[15];//Packing_UOM
        var PackingValue = (Packing_Factor * QuantityValue) + " " + Packing_UOM;
        if (IsPackingActive == "Y" && (parseFloat(Packing_Factor * QuantityValue) > 0)) {
            $('#lblPackingStk').text(PackingValue);
            // divPacking.style.display = "block";
        } else {
            divPacking.style.display = "none";
        }
        var tbAmount = grid.GetEditor("Amount");
        tbAmount.SetValue(Amount);
        var tbTotalAmount = grid.GetEditor("TotalAmount");
        tbTotalAmount.SetValue(Amount);
        DiscountTextChange(s, e);
        //.........AvailableStock.............

    }
    else {
        jAlert('Select a product first.');
        grid.GetEditor('Quantity').SetValue('0');
        grid.GetEditor('ProductID').Focus();
    }
}

// Non Inventory Section By Sam on 24052017

function TDSChecking(s, e) {
    var inventoryItem = $('#ddlInventory').val();
    if (inventoryItem == 'N') {
        var schemeid = cddl_TdsScheme.GetValue()
        if (schemeid != '0') {
            var ProductID = grid.GetEditor('ProductID').GetValue();
            if (ProductID != null) {
            var slno = grid.GetEditor('SrlNo').GetValue();
                if ($('#hdntdschecking').val() == '') {
                    $('#hdntdschecking').val(slno + ',');
                }
                else {
                    var myArray = $('#hdntdschecking').val().split(',');
                    if ($.inArray(slno, myArray) != -1) {

                    }
                    else {
                        $('#hdntdschecking').val($('#hdntdschecking').val() + slno)
                    }


                }
            }
        }
    }
}
function qtyvalidate() {
    var srlno = grid.GetEditor('SrlNo').GetValue();
    var previousqty = grid.GetEditor('Quantity').GetValue();

    return $.ajax({
        type: "POST",
        url: 'PurchaseInvoiceforTransporter.aspx/ValidQuantity',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ srlno: srlno, previousqty: previousqty }),
        cache: false
    });

}
// Non Inventory Section By Sam on 24052017
function QuantityTextChange(s, e) {
    var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
    if (parseFloat(QuantityValue) != parseFloat(ProductGetQuantity)) {
        var type = ($("[id$='rdl_PurchaseInvoice']").find(":checked").val() != null) ? $("[id$='rdl_PurchaseInvoice']").find(":checked").val() : "";
        if (type == 'PO') {
            var TotalQty = (grid.GetEditor('TotalQty').GetText() != null) ? grid.GetEditor('TotalQty').GetText() : "0";
            var BalanceQty = (grid.GetEditor('BalanceQty').GetText() != null) ? grid.GetEditor('BalanceQty').GetText() : "0";
            var CurrQty = 0;

            BalanceQty = parseFloat(BalanceQty);
            TotalQty = parseFloat(TotalQty);
            QuantityValue = parseFloat(QuantityValue);

            if (TotalQty > QuantityValue) {
                CurrQty = BalanceQty + (TotalQty - QuantityValue);
            }
            else {
                CurrQty = BalanceQty - (QuantityValue - TotalQty);
            }

            if (CurrQty < 0) {
                grid.GetEditor("TotalQty").SetValue(TotalQty);
                grid.GetEditor("Quantity").SetValue(TotalQty);
                var OrdeMsg = 'Balance Quantity of selected Product from tagged document is (' + ((QuantityValue - TotalQty) + BalanceQty) + '). <br/>Cannot enter quantity more than balance quantity.';
                grid.batchEditApi.EndEdit();
                jAlert(OrdeMsg, 'Alert Dialog: [Balace Quantity ]', function (r) {
                    grid.batchEditApi.StartEdit(globalRowIndex, 6);
                });
                return false;
            }
            else {
                grid.GetEditor("TotalQty").SetValue(QuantityValue);
                grid.GetEditor("BalanceQty").SetValue(CurrQty);
            }
        }
        else {
            grid.GetEditor("TotalQty").SetValue(QuantityValue);
            grid.GetEditor("BalanceQty").SetValue(QuantityValue);
        }
        
    TDSChecking();
        pageheaderContent.style.display = "block";
        var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
        var ProductID = grid.GetEditor('ProductID').GetValue();
        if (ProductID != null) {
            var SpliteDetails = ProductID.split("||@||");
            var strMultiplier = SpliteDetails[7];//Conversion_Multiplier 
            var strFactor = SpliteDetails[8]; //Packing_Factor 
            var strRate = (ctxtRate.GetValue() != null && ctxtRate.GetValue() != "0") ? ctxtRate.GetValue() : "1";
            var strProductID = SpliteDetails[0];
            var strProductName = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
            var ddlbranch = $("[id*=ddl_Branch]");
            var strBranch = ddlbranch.find("option:selected").text();
            var strStkUOM = SpliteDetails[4];//Stk_UOM_Name 
            var strSalePrice = (grid.GetEditor('PurchasePrice').GetValue() != null) ? grid.GetEditor('PurchasePrice').GetValue() : "0";
           
             if (strFactor == 0) {
                 strFactor = 1;
             }
            if (strRate == 0) {
                strRate = 1;
            }
            if (strSalePrice == 0.00000) {
                strSalePrice = 1;
            }
            var StockQuantity = strMultiplier * QuantityValue;
            var Amount = QuantityValue * strFactor * (strSalePrice / strRate);
            var tbAmount = grid.GetEditor("Amount");
            tbAmount.SetValue(Amount);

            var tbTotalAmount = grid.GetEditor("TotalAmount");
            tbTotalAmount.SetValue(Amount);
            DiscountTextChange(s, e);
            //.........AvailableStock............ 
        }
        else {
            jAlert('Select a product first.');
            grid.GetEditor('Quantity').SetValue('0');
            grid.GetEditor('PurchasePrice').SetValue('0');
            grid.GetEditor('ProductID').Focus();
        }
    }
}
function PurchasePriceTextChange(s, e) {
    var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
    var PriceValue = (grid.GetEditor('PurchasePrice').GetValue() != null) ? grid.GetEditor('PurchasePrice').GetValue() : "0";
    if (ProductPurchaseprice != parseFloat(PriceValue)) {
        var type = ($("[id$='rdl_PurchaseInvoice']").find(":checked").val() != null) ? $("[id$='rdl_PurchaseInvoice']").find(":checked").val() : "";
        if (type == 'PO') {
            var TotalQty = (grid.GetEditor('TotalQty').GetText() != null) ? grid.GetEditor('TotalQty').GetText() : "0";
            var BalanceQty = (grid.GetEditor('BalanceQty').GetText() != null) ? grid.GetEditor('BalanceQty').GetText() : "0";
            var CurrQty = 0;
            BalanceQty = parseFloat(BalanceQty);
            TotalQty = parseFloat(TotalQty);
            QuantityValue = parseFloat(QuantityValue);
            if (TotalQty > QuantityValue) {
                CurrQty = BalanceQty + (TotalQty - QuantityValue);
            }
            else {
                CurrQty = BalanceQty - (QuantityValue - TotalQty);
            }
            if (CurrQty < 0) {
                grid.GetEditor("TotalQty").SetValue(TotalQty);
                grid.GetEditor("Quantity").SetValue(TotalQty);
                var OrdeMsg = 'Balance Quantity of selected Product from tagged document is (' + ((QuantityValue - TotalQty) + BalanceQty) + '). <br/>Cannot enter quantity more than balance quantity.';
                grid.batchEditApi.EndEdit();
                jAlert(OrdeMsg, 'Alert Dialog: [Balace Quantity ]', function (r) {
                    grid.batchEditApi.StartEdit(globalRowIndex, 6);
                });
                return false;
            }
            else {
                grid.GetEditor("TotalQty").SetValue(QuantityValue);
                grid.GetEditor("BalanceQty").SetValue(CurrQty);
            }
        }
        else {
            grid.GetEditor("TotalQty").SetValue(QuantityValue);
            grid.GetEditor("BalanceQty").SetValue(QuantityValue);
        }
        
        TDSChecking();
        pageheaderContent.style.display = "block";
        var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
        var ProductID = grid.GetEditor('ProductID').GetValue();
        if (ProductID != null) {
            var SpliteDetails = ProductID.split("||@||");
            var strMultiplier = SpliteDetails[7];//Conversion_Multiplier 
            var strFactor = SpliteDetails[8]; //Packing_Factor 
            var strRate = (ctxtRate.GetValue() != null && ctxtRate.GetValue() != "0") ? ctxtRate.GetValue() : "1";
            var strProductID = SpliteDetails[0];
            var strProductName = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
            var ddlbranch = $("[id*=ddl_Branch]");
            var strBranch = ddlbranch.find("option:selected").text();
            var strStkUOM = SpliteDetails[4];//Stk_UOM_Name 
            var strSalePrice = (grid.GetEditor('PurchasePrice').GetValue() != null) ? grid.GetEditor('PurchasePrice').GetValue() : "0";
            if (strFactor == 0) {
                strFactor = 1;
            }
            if (strRate == 0) {
                strRate = 1;
            }
            if (strSalePrice == 0.00000) {
                strSalePrice = 1;
            }
            var StockQuantity = strMultiplier * QuantityValue;
            var Amount = QuantityValue * strFactor * (strSalePrice / strRate);
            var tbAmount = grid.GetEditor("Amount");
            tbAmount.SetValue(Amount);
            grid.GetEditor('TaxAmount').SetValue(0);
            var tbTotalAmount = grid.GetEditor("TotalAmount");
            tbTotalAmount.SetValue(Amount);
            DiscountTextChange(s, e);
            //.........AvailableStock............. 
        }
        else {
            jAlert('Select a product first.');
            grid.GetEditor('Quantity').SetValue('0');
            grid.GetEditor('PurchasePrice').SetValue('0');
            grid.GetEditor('ProductID').Focus();
        }
    }
}
function AmtTextChange(s, e) {
    TDSChecking();
    var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
    var PurPrice = (grid.GetEditor('PurchasePrice').GetValue() != null) ? grid.GetEditor('PurchasePrice').GetValue() : "0";
    var Discount = (grid.GetEditor('Discount').GetValue() != null) ? grid.GetEditor('Discount').GetValue() : "0";
    var Discountamt = (grid.GetEditor('Discountamt').GetValue() != null) ? grid.GetEditor('Discountamt').GetValue() : "0";
    var ProductWiseGrsAmt = (grid.GetEditor('Amount').GetValue() != null) ? grid.GetEditor('Amount').GetValue() : "0";
    var ProductID = grid.GetEditor('ProductID').GetValue();
    if (ProductID != null) {
        if (parseFloat(Discount) != parseFloat(ProductDiscount)
            || parseFloat(QuantityValue) != parseFloat(ProductGetQuantity)
            || parseFloat(PurPrice) != parseFloat(ProductPurchaseprice)
            || parseFloat(Discountamt) != parseFloat(ProductDiscountAmt)
             || parseFloat(ProductWiseGrsAmt) != parseFloat(ProductGrsAmt)) {
            var SpliteDetails = ProductID.split("||@||");
            var strFactor = SpliteDetails[8];
            var strRate = (ctxtRate.GetValue() != null && ctxtRate.GetValue() != "0") ? ctxtRate.GetValue() : "1";
            var strSalePrice = (grid.GetEditor('PurchasePrice').GetValue() != null) ? grid.GetEditor('PurchasePrice').GetValue() : "0";
            if (strFactor == 0) {
                strFactor = 1;
            }
            if (strSalePrice == '0') {
                strSalePrice = SpliteDetails[6];
            }
            if (strRate == 0) {
                strRate = 1;
            }
            var Discountamt = (grid.GetEditor('Discountamt').GetValue() != null) ? grid.GetEditor('Discountamt').GetValue() : "0";
            var Amount = QuantityValue * strFactor * (strSalePrice / strRate);
            var Discount = ((parseFloat(Discountamt) * 100) / parseFloat(Amount));
            var amountAfterDiscount = parseFloat(Amount) - ((parseFloat(Discount) * parseFloat(Amount)) / 100);
            var grossamt = grid.GetEditor('Amount').GetValue();
            grid.GetEditor('TaxAmount').SetValue(0);
            var _TotalTaxAmt = (grid.GetEditor('TaxAmount').GetValue() != null) ? grid.GetEditor('TaxAmount').GetValue() : "0";
            var tbTotalAmount = grid.GetEditor("TotalAmount");
            tbTotalAmount.SetValue(parseFloat(grossamt) + parseFloat(_TotalTaxAmt));
            var _TotalAmount = (grid.GetEditor('TotalAmount').GetValue() != null) ? grid.GetEditor('TotalAmount').GetValue() : "0";
            if (parseFloat(_TotalAmount) != parseFloat(ProductGetTotalAmount)) {
                grid.GetEditor('TaxAmount').SetValue(0);
                ctaxUpdatePanel.PerformCallback('DelQtybySl~' + grid.GetEditor("SrlNo").GetValue());
                var incluexclutype = ''
                var taxtype = cddl_AmountAre.GetValue();
                if (taxtype == '1') {
                    incluexclutype = 'E'
                }
                else if (taxtype == '2') {
                    incluexclutype = 'I'
                }
                var checkval = cchk_reversemechenism.GetChecked();
                if (!checkval) {
                    if ($('#hdnADDEditMode').val() != 'Edit') {
                        var schemabranchid = $('#ddl_numberingScheme').val();
                        if (schemabranchid != '0') {
                            var schemabranch = $('#ddl_numberingScheme').val().split('~')[1];
                            // Here we are sending Branch StateID instead of Shipping StateID after discuss with
                            // Pijush Da and Debjyoti on 14122017
                            caluculateAndSetGST(grid.GetEditor("Amount"), grid.GetEditor("TaxAmount"), grid.GetEditor("TotalAmount"), SpliteDetails[18], Amount, grossamt, incluexclutype, $("#hdnucSBranchStateId").val(), schemabranch, 'P')
                            //caluculateAndSetGST(grid.GetEditor("Amount"), grid.GetEditor("TaxAmount"), grid.GetEditor("TotalAmount"), SpliteDetails[18], Amount, grossamt, incluexclutype, $("#bsSCmbStateHF").val(), schemabranch, 'P')

                        }
                    }
                    else if ($('#hdnADDEditMode').val() == 'Edit') {
                        var schemabranchid = $('#ddl_Branch').val();
                        if (schemabranchid != '0') {
                            var schemabranch = schemabranchid;
                            // Here we are sending Branch StateID instead of Shipping StateID after discuss with
                            // Pijush Da and Debjyoti on 14122017
                            caluculateAndSetGST(grid.GetEditor("Amount"), grid.GetEditor("TaxAmount"), grid.GetEditor("TotalAmount"), SpliteDetails[18], Amount, grossamt, incluexclutype, $("#hdnucSBranchStateId").val(), schemabranch, 'P')
                            //caluculateAndSetGST(grid.GetEditor("Amount"), grid.GetEditor("TaxAmount"), grid.GetEditor("TotalAmount"), SpliteDetails[18], Amount, grossamt, incluexclutype, $("#bsSCmbStateHF").val(), schemabranch, 'P')
                        }
                    }
                }
            }
            // Running total Calculation Start
            Cur_Quantity = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
            Cur_Amt = (grid.GetEditor('Amount').GetValue() != null) ? grid.GetEditor('Amount').GetValue() : "0";
            Cur_TotalAmt = (grid.GetEditor('TotalAmount').GetValue() != null) ? grid.GetEditor('TotalAmount').GetValue() : "0";
            CalculateAmount();
            // Running total Calculation End
        }
    }
    else {
        jAlert('Select a product first.');
        grid.GetEditor('Amount').SetValue('0');
        grid.GetEditor('ProductID').Focus();
    }
}

function DiscountAmtTextChange(s, e) {
    TDSChecking();
    var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
    var PurPrice = (grid.GetEditor('PurchasePrice').GetValue() != null) ? grid.GetEditor('PurchasePrice').GetValue() : "0";
    var Discount = (grid.GetEditor('Discount').GetValue() != null) ? grid.GetEditor('Discount').GetValue() : "0";
    var Discountamt = (grid.GetEditor('Discountamt').GetValue() != null) ? grid.GetEditor('Discountamt').GetValue() : "0";
    var ProductID = grid.GetEditor('ProductID').GetValue();
    if (ProductID != null) {
        if (parseFloat(Discount) != parseFloat(ProductDiscount) || parseFloat(QuantityValue) != parseFloat(ProductGetQuantity) || parseFloat(PurPrice) != parseFloat(ProductPurchaseprice) || parseFloat(Discountamt) != parseFloat(ProductDiscountAmt)) {
            var SpliteDetails = ProductID.split("||@||");
            var strFactor = SpliteDetails[8];
            var strRate = (ctxtRate.GetValue() != null && ctxtRate.GetValue() != "0") ? ctxtRate.GetValue() : "1";
            var strSalePrice = (grid.GetEditor('PurchasePrice').GetValue() != null) ? grid.GetEditor('PurchasePrice').GetValue() : "0";
            if (strFactor == 0) {
                strFactor = 1;
            }
            if (strSalePrice == '0') {
                strSalePrice = SpliteDetails[6];
            }
            if (strRate == 0) {
                strRate = 1;
            }
            var Amount = QuantityValue * strFactor * (strSalePrice / strRate);
            var Discount = ((parseFloat(Discountamt) * 100) / parseFloat(Amount));
            var amountAfterDiscount = parseFloat(Amount) - ((parseFloat(Discount) * parseFloat(Amount)) / 100);
            var DiscountamtField = grid.GetEditor("Discount");
            DiscountamtField.SetValue(Discount);
            var tbAmount = grid.GetEditor("Amount");
            tbAmount.SetValue(amountAfterDiscount);

            var IsPackingActive = SpliteDetails[13];
            var Packing_Factor = SpliteDetails[14];
            var Packing_UOM = SpliteDetails[15];
            var PackingValue = (Packing_Factor * QuantityValue) + " " + Packing_UOM;

            if (IsPackingActive == "Y" && (parseFloat(Packing_Factor * QuantityValue) > 0)) {
                $('#lblPackingStk').text(PackingValue);
            } else {
                divPacking.style.display = "none";
            }
            grid.GetEditor('TaxAmount').SetValue(0);
            var tbTotalAmount = grid.GetEditor("TotalAmount");
            var _TotalTaxAmt = (grid.GetEditor('TaxAmount').GetValue() != null) ? grid.GetEditor('TaxAmount').GetValue() : "0";
            var tbTotalAmount = grid.GetEditor("TotalAmount");
            tbTotalAmount.SetValue(parseFloat(amountAfterDiscount) + parseFloat(_TotalTaxAmt));
        }
    }
    else {
        jAlert('Select a product first.');
        grid.GetEditor('Discountamt').SetValue('0');
        grid.GetEditor('ProductID').Focus();
    }
    //Debjyoti  
    var _TotalAmount = (grid.GetEditor('TotalAmount').GetValue() != null) ? grid.GetEditor('TotalAmount').GetValue() : "0";
    if (parseFloat(_TotalAmount) != parseFloat(ProductGetTotalAmount)) {
        grid.GetEditor('TaxAmount').SetValue(0);
        ctaxUpdatePanel.PerformCallback('DelQtybySl~' + grid.GetEditor("SrlNo").GetValue());
        var incluexclutype = ''
        var taxtype = cddl_AmountAre.GetValue();
        if (taxtype == '1') {
            incluexclutype = 'E'
        }
        else if (taxtype == '2') {
            incluexclutype = 'I'
        }
        var checkval = cchk_reversemechenism.GetChecked();
        if (!checkval) {
            if ($('#hdnADDEditMode').val() != 'Edit') {
                var schemabranchid = $('#ddl_numberingScheme').val();
                if (schemabranchid != '0') {
                    var schemabranch = $('#ddl_numberingScheme').val().split('~')[1];
                    // Here we are sending Branch StateId  instead of Shipping StateId after discuss with
                    // Pijush Da and Debjyoti on 14122017
                    caluculateAndSetGST(grid.GetEditor("Amount"), grid.GetEditor("TaxAmount"), grid.GetEditor("TotalAmount"), SpliteDetails[18], Amount, amountAfterDiscount, incluexclutype, $("#hdnucSBranchStateId").val(), schemabranch, 'P')
                    //caluculateAndSetGST(grid.GetEditor("Amount"), grid.GetEditor("TaxAmount"), grid.GetEditor("TotalAmount"), SpliteDetails[18], Amount, amountAfterDiscount, incluexclutype, $("#bsSCmbStateHF").val(), schemabranch, 'P')
                }
            }
            else if ($('#hdnADDEditMode').val() == 'Edit') {
                var schemabranchid = $('#ddl_Branch').val();
                if (schemabranchid != '0') {
                    var schemabranch = schemabranchid;
                    // Here we are sending Branch StateId  instead of Shipping StateId after discuss with
                    // Pijush Da and Debjyoti on 14122017
                    caluculateAndSetGST(grid.GetEditor("Amount"), grid.GetEditor("TaxAmount"), grid.GetEditor("TotalAmount"), SpliteDetails[18], Amount, amountAfterDiscount, incluexclutype, $("#hdnucSBranchStateId").val(), schemabranch, 'P')
                    //caluculateAndSetGST(grid.GetEditor("Amount"), grid.GetEditor("TaxAmount"), grid.GetEditor("TotalAmount"), SpliteDetails[18], Amount, amountAfterDiscount, incluexclutype, $("#bsSCmbStateHF").val(), schemabranch, 'P')
                }
            }
        }
    }
    // Running total Calculation Start
    Cur_Quantity = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
    Cur_Amt = (grid.GetEditor('Amount').GetValue() != null) ? grid.GetEditor('Amount').GetValue() : "0";
    Cur_TotalAmt = (grid.GetEditor('TotalAmount').GetValue() != null) ? grid.GetEditor('TotalAmount').GetValue() : "0";
    CalculateAmount();
    // Running total Calculation End 
}

function DiscountTextChange(s, e) {
    TDSChecking();
    var PurPrice = (grid.GetEditor('PurchasePrice').GetValue() != null) ? grid.GetEditor('PurchasePrice').GetValue() : "0";
    var Discount = (grid.GetEditor('Discount').GetValue() != null) ? grid.GetEditor('Discount').GetValue() : "0";
    var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
    var ProductID = grid.GetEditor('ProductID').GetValue();
    if (ProductID != null) {
        if (parseFloat(Discount) != parseFloat(ProductDiscount) || parseFloat(QuantityValue) != parseFloat(ProductGetQuantity) || parseFloat(PurPrice) != parseFloat(ProductPurchaseprice)) {
            var SpliteDetails = ProductID.split("||@||");
            var strFactor = SpliteDetails[8];
            var strRate = (ctxtRate.GetValue() != null && ctxtRate.GetValue() != "0") ? ctxtRate.GetValue() : "1";
            var strSalePrice = (grid.GetEditor('PurchasePrice').GetValue() != null) ? grid.GetEditor('PurchasePrice').GetValue() : "0";
            if (strFactor == 0) {
                strFactor = 1;
            }
            if (strSalePrice == '0') {
                strSalePrice = SpliteDetails[6];
            }
            if (strRate == 0) {
                strRate = 1;
            }
            var Amount = QuantityValue * strFactor * (strSalePrice / strRate);
            var Discountamt = ((parseFloat(Discount) * parseFloat(Amount)) / 100);
            var amountAfterDiscount = parseFloat(Amount) - ((parseFloat(Discount) * parseFloat(Amount)) / 100);
            var DiscountamtField = grid.GetEditor("Discountamt");
            DiscountamtField.SetValue(Discountamt);
            var tbAmount = grid.GetEditor("Amount");
            tbAmount.SetValue(amountAfterDiscount);
            var IsPackingActive = SpliteDetails[13];
            var Packing_Factor = SpliteDetails[14];
            var Packing_UOM = SpliteDetails[15];
            var PackingValue = (Packing_Factor * QuantityValue) + " " + Packing_UOM;
            if (IsPackingActive == "Y" && (parseFloat(Packing_Factor * QuantityValue) > 0)) {
                $('#lblPackingStk').text(PackingValue);
            } else {
                divPacking.style.display = "none";
            }
            grid.GetEditor('TaxAmount').SetValue(0);
            var _TotalTaxAmt = (grid.GetEditor('TaxAmount').GetValue() != null) ? grid.GetEditor('TaxAmount').GetValue() : "0";
            var tbTotalAmount = grid.GetEditor("TotalAmount");
            tbTotalAmount.SetValue(parseFloat(amountAfterDiscount) + parseFloat(_TotalTaxAmt));
        }
    }
    else {
        jAlert('Select a product first.');
        grid.GetEditor('Discount').SetValue('0');
        grid.GetEditor('ProductID').Focus();
    }
    var _TotalAmount = (grid.GetEditor('TotalAmount').GetValue() != null) ? grid.GetEditor('TotalAmount').GetValue() : "0";
    if (parseFloat(_TotalAmount) != parseFloat(ProductGetTotalAmount)) {
        grid.GetEditor('TaxAmount').SetValue(0);
        ctaxUpdatePanel.PerformCallback('DelQtybySl~' + grid.GetEditor("SrlNo").GetValue());

        var incluexclutype = ''
        var taxtype = cddl_AmountAre.GetValue();
        if (taxtype == '1') {
            incluexclutype = 'E'
        }
        else if (taxtype == '2') {
            incluexclutype = 'I'
        }
        var checkval = cchk_reversemechenism.GetChecked();
        if (!checkval) {
            if ($('#hdnADDEditMode').val() != 'Edit') {
                var schemabranchid = $('#ddl_numberingScheme').val();
                if (schemabranchid != '0') {
                    var schemabranch = $('#ddl_numberingScheme').val().split('~')[1];
                    // Here we are sending Branch StateId  instead of Shipping StateId after discuss with
                    // Pijush Da and Debjyoti on 14122017
                    caluculateAndSetGST(grid.GetEditor("Amount"), grid.GetEditor("TaxAmount"), grid.GetEditor("TotalAmount"), SpliteDetails[18], Amount, amountAfterDiscount, incluexclutype, $("#hdnucSBranchStateId").val(), schemabranch, 'P')
                    //caluculateAndSetGST(grid.GetEditor("Amount"), grid.GetEditor("TaxAmount"), grid.GetEditor("TotalAmount"), SpliteDetails[18], Amount, amountAfterDiscount, incluexclutype, $("#bsSCmbStateHF").val(), schemabranch, 'P')
                }
            }
            else if ($('#hdnADDEditMode').val() == 'Edit') {
                var schemabranchid = $('#ddl_Branch').val();
                if (schemabranchid != '0') {
                    var schemabranch = schemabranchid;
                    // Here we are sending Branch StateId  instead of Shipping StateId after discuss with
                    // Pijush Da and Debjyoti on 14122017
                    caluculateAndSetGST(grid.GetEditor("Amount"), grid.GetEditor("TaxAmount"), grid.GetEditor("TotalAmount"), SpliteDetails[18], Amount, amountAfterDiscount, incluexclutype, $("#hdnucSBranchStateId").val(), schemabranch, 'P')
                    //caluculateAndSetGST(grid.GetEditor("Amount"), grid.GetEditor("TaxAmount"), grid.GetEditor("TotalAmount"), SpliteDetails[18], Amount, amountAfterDiscount, incluexclutype, $("#bsSCmbStateHF").val(), schemabranch, 'P')
                }
            }

        }
    }

    // Running total Calculation Start
    Cur_Quantity = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
    Cur_Amt = (grid.GetEditor('Amount').GetValue() != null) ? grid.GetEditor('Amount').GetValue() : "0";
    Cur_TotalAmt = (grid.GetEditor('TotalAmount').GetValue() != null) ? grid.GetEditor('TotalAmount').GetValue() : "0";
    CalculateAmount();
    // Running total Calculation End

}

//......................Amount Calculation End.......................
/*........................Tax Start...........................*/
var taxAmountGlobalCharges;
var chargejsonTax;
var taxAmountGlobal;
var GlobalCurChargeTaxAmt;
var ChargegstcstvatGlobalName;
var GlobalCurTaxAmt = 0;
var rowEditCtrl;
var globalRowIndex;
var globalTaxRowIndex;
var gstcstvatGlobalName;
var taxJson;
function Save_TaxClick() {
    if (gridTax.GetVisibleRowsOnPage() > 0) {
        gridTax.UpdateEdit();
    }
    else {
        gridTax.PerformCallback('SaveGst');
    }
    cPopup_Taxes.Hide();
}
//Set Running Total for Charges And Tax 
function SetChargesRunningTotal() {
    var runningTot = parseFloat(ctxtProductNetAmount.GetValue());
    for (var i = 0; i < chargejsonTax.length; i++) {
        gridTax.batchEditApi.StartEdit(i, 3);
        if (chargejsonTax[i].applicableOn == "R") {
            gridTax.GetEditor("calCulatedOn").SetValue(runningTot);
            var totLength = gridTax.GetEditor("TaxName").GetText().length;
            var taxNameWithSign = gridTax.GetEditor("Percentage").GetText();
            var sign = gridTax.GetEditor("TaxName").GetText().substring(totLength - 3);
            var ProdAmt = parseFloat(gridTax.GetEditor("calCulatedOn").GetValue());
            var Amount = gridTax.GetEditor("calCulatedOn").GetValue();
            var GlobalTaxAmt = 0;

            var Percentage = gridTax.GetEditor("Percentage").GetText();
            var totLength = gridTax.GetEditor("TaxName").GetText().length;
            var sign = gridTax.GetEditor("TaxName").GetText().substring(totLength - 3);
            Sum = ((parseFloat(Amount) * parseFloat(Percentage)) / 100);

            if (sign == '(+)') {
                GlobalTaxAmt = parseFloat(gridTax.GetEditor("Amount").GetValue());
                gridTax.GetEditor("Amount").SetValue(Sum);
                ctxtQuoteTaxTotalAmt.SetValue(parseFloat(ctxtQuoteTaxTotalAmt.GetValue()) + parseFloat(Sum) - GlobalTaxAmt);
                ctxtTotalAmount.SetValue(parseFloat(Amount) + parseFloat(ctxtQuoteTaxTotalAmt.GetValue()));
                GlobalTaxAmt = 0;
            }
            else {
                GlobalTaxAmt = parseFloat(gridTax.GetEditor("Amount").GetValue());
                gridTax.GetEditor("Amount").SetValue(Sum);
                ctxtQuoteTaxTotalAmt.SetValue(parseFloat(ctxtQuoteTaxTotalAmt.GetValue()) - parseFloat(Sum) + GlobalTaxAmt);
                ctxtTotalAmount.SetValue(parseFloat(Amount) + parseFloat(ctxtQuoteTaxTotalAmt.GetValue()));
                GlobalTaxAmt = 0;
            }
            SetOtherChargeTaxValueOnRespectiveRow(0, Sum, gridTax.GetEditor("TaxName").GetText());
        }
        runningTot = runningTot + parseFloat(gridTax.GetEditor("Amount").GetValue());
        gridTax.batchEditApi.EndEdit();
    }
}
function GetPercentageData() {
    var Amount = ctxtProductAmount.GetValue();
    var GlobalTaxAmt = 0;
    var noofvisiblerows = gridTax.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
    var i, cnt = 1;
    var sumAmount = 0, totalAmount = 0;
    for (i = 0 ; cnt <= noofvisiblerows ; i++) {
        var totLength = gridTax.batchEditApi.GetCellValue(i, 'TaxName').length;
        var sign = gridTax.batchEditApi.GetCellValue(i, 'TaxName').substring(totLength - 3);
        var DisAmount = (gridTax.batchEditApi.GetCellValue(i, 'Amount') != null) ? (gridTax.batchEditApi.GetCellValue(i, 'Amount')) : "0";

        if (sign == '(+)') {
            sumAmount = sumAmount + parseFloat(DisAmount);
        }
        else {
            sumAmount = sumAmount - parseFloat(DisAmount);
        }

        cnt++;
    }
    totalAmount = (parseFloat(Amount)) + (parseFloat(sumAmount));
}
function Save_TaxesClick() {
    grid.batchEditApi.EndEdit();
    var noofvisiblerows = grid.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
    var i, cnt = 1;
    var sumAmount = 0, sumTaxAmount = 0, sumDiscount = 0, sumNetAmount = 0, sumDiscountAmt = 0;
    cnt = 1;
    for (i = -1 ; cnt <= noofvisiblerows ; i--) {
        var Amount = (grid.batchEditApi.GetCellValue(i, 'Amount') != null) ? (grid.batchEditApi.GetCellValue(i, 'Amount')) : "0";
        var TaxAmount = (grid.batchEditApi.GetCellValue(i, 'TaxAmount') != null) ? (grid.batchEditApi.GetCellValue(i, 'TaxAmount')) : "0";
        var Discount = (grid.batchEditApi.GetCellValue(i, 'Discount') != null) ? (grid.batchEditApi.GetCellValue(i, 'Discount')) : "0";
        var NetAmount = (grid.batchEditApi.GetCellValue(i, 'TotalAmount') != null) ? (grid.batchEditApi.GetCellValue(i, 'TotalAmount')) : "0";
        var sumDiscountAmt = ((parseFloat(Discount) * parseFloat(Amount)) / 100);
        sumAmount = sumAmount + parseFloat(Amount);
        sumTaxAmount = sumTaxAmount + parseFloat(TaxAmount);
        sumDiscount = sumDiscount + parseFloat(sumDiscountAmt);
        sumNetAmount = sumNetAmount + parseFloat(NetAmount);
        cnt++;
    }
    if (sumAmount == 0 && sumTaxAmount == 0 && Discount == 0) {
        cnt = 1;
        for (i = 0 ; cnt <= noofvisiblerows ; i++) {
            var Amount = (grid.batchEditApi.GetCellValue(i, 'Amount') != null) ? (grid.batchEditApi.GetCellValue(i, 'Amount')) : "0";
            var TaxAmount = (grid.batchEditApi.GetCellValue(i, 'TaxAmount') != null) ? (grid.batchEditApi.GetCellValue(i, 'TaxAmount')) : "0";
            var Discount = (grid.batchEditApi.GetCellValue(i, 'Discount') != null) ? (grid.batchEditApi.GetCellValue(i, 'Discount')) : "0";
            var NetAmount = (grid.batchEditApi.GetCellValue(i, 'TotalAmount') != null) ? (grid.batchEditApi.GetCellValue(i, 'TotalAmount')) : "0";
            var sumDiscountAmt = ((parseFloat(Discount) * parseFloat(Amount)) / 100);
            sumAmount = sumAmount + parseFloat(Amount);
            sumTaxAmount = sumTaxAmount + parseFloat(TaxAmount);
            sumDiscount = sumDiscount + parseFloat(sumDiscountAmt);
            sumNetAmount = sumNetAmount + parseFloat(NetAmount);
            cnt++;
        }
    }
    //Debjyoti 
    document.getElementById('HdChargeProdAmt').value = sumAmount;
    document.getElementById('HdChargeProdNetAmt').value = sumNetAmount;
    //End Here
    ctxtProductAmount.SetValue(parseFloat(Math.round(sumAmount * 100) / 100).toFixed(2));
    ctxtProductTaxAmount.SetValue(parseFloat(Math.round(sumTaxAmount * 100) / 100).toFixed(2));
    ctxtProductDiscount.SetValue(parseFloat(Math.round(sumDiscount * 100) / 100).toFixed(2));
    ctxtProductNetAmount.SetValue(parseFloat(Math.round(sumNetAmount * 100) / 100).toFixed(2));
    clblChargesTaxableGross.SetText("");
    clblChargesTaxableNet.SetText("");

    //Checking is gstcstvat will be hidden or not
    if (cddl_AmountAre.GetValue() == "2") {

        $('.lblChargesGSTforGross').show();
        $('.lblChargesGSTforNet').show();

        //Set Gross Amount with GstValue
        //Get The rate of Gst
        var gstRate = parseFloat(cddlVatGstCst.GetValue().split('~')[1]);
        if (gstRate) {
            if (gstRate != 0) {
                var gstDis = (gstRate / 100) + 1;
                if (cddlVatGstCst.GetValue().split('~')[2] == "G") {
                    $('.lblChargesGSTforNet').hide();
                    ctxtProductAmount.SetText(Math.round(sumAmount / gstDis).toFixed(2));
                    document.getElementById('HdChargeProdAmt').value = Math.round(sumAmount / gstDis).toFixed(2);
                    clblChargesGSTforGross.SetText(Math.round(sumAmount - parseFloat(document.getElementById('HdChargeProdAmt').value)).toFixed(2));
                    clblChargesTaxableGross.SetText("(Taxable)");
                }
                else {
                    $('.lblChargesGSTforGross').hide();
                    ctxtProductNetAmount.SetText(Math.round(sumNetAmount / gstDis).toFixed(2));
                    document.getElementById('HdChargeProdNetAmt').value = Math.round(sumNetAmount / gstDis).toFixed(2);
                    clblChargesGSTforNet.SetText(Math.round(sumNetAmount - parseFloat(document.getElementById('HdChargeProdNetAmt').value)).toFixed(2));
                    clblChargesTaxableNet.SetText("(Taxable)");
                }
            }
        } else {
            $('.lblChargesGSTforGross').hide();
            $('.lblChargesGSTforNet').hide();
        }
    }
    else if (cddl_AmountAre.GetValue() == "1") {
        $('.lblChargesGSTforGross').hide();
        $('.lblChargesGSTforNet').hide();
    }
    //End here
    //Set Total amount
    ctxtTotalAmount.SetValue(parseFloat(ctxtQuoteTaxTotalAmt.GetValue()) + parseFloat(ctxtProductNetAmount.GetValue()));
    gridTax.PerformCallback('Display');
    //Checking is gstcstvat will be hidden or not
    if (cddl_AmountAre.GetValue() == "2") {
        $('.chargeGstCstvatClass').hide();
    }
    else if (cddl_AmountAre.GetValue() == "1") {
        $('.chargeGstCstvatClass').show();
    }
    //End here
    $('.RecalculateCharge').hide();
    cPopup_Taxes.Show();
    gridTax.StartEditRow(0);
}
function SetFocusonDemand(e) {
    var key = cddl_AmountAre.GetValue();
    if (key == '1' || key == '3') {
        if (grid.GetVisibleRowsOnPage() == 1) {
            grid.batchEditApi.StartEdit(-1, 2);
        }
    }
    else if (key == '2') {
        cddlVatGstCst.Focus();
    }

}
function QuotationTaxAmountTextChange(s, e) {
    var Amount = gridTax.GetEditor("calCulatedOn").GetValue();
    var GlobalTaxAmt = 0;
    var totLength = gridTax.GetEditor("TaxName").GetText().length;
    var sign = gridTax.GetEditor("TaxName").GetText().substring(totLength - 3);
    if (sign == '(+)') {
        GlobalTaxAmt = parseFloat(gridTax.GetEditor("Amount").GetValue());
        ctxtQuoteTaxTotalAmt.SetValue(parseFloat(ctxtQuoteTaxTotalAmt.GetValue()) + GlobalTaxAmt - taxAmountGlobalCharges);
        ctxtTotalAmount.SetText(parseFloat(Amount) + parseFloat(ctxtQuoteTaxTotalAmt.GetValue()));
        GlobalTaxAmt = 0;
        SetOtherChargeTaxValueOnRespectiveRow(0, s.GetValue(), gridTax.GetEditor("TaxName").GetText());
    }
    else {
        GlobalTaxAmt = parseFloat(gridTax.GetEditor("Amount").GetValue());
        ctxtQuoteTaxTotalAmt.SetValue(parseFloat(ctxtQuoteTaxTotalAmt.GetValue()) - GlobalTaxAmt + taxAmountGlobalCharges);
        ctxtTotalAmount.SetValue(parseFloat(Amount) + parseFloat(ctxtQuoteTaxTotalAmt.GetValue()));
        GlobalTaxAmt = 0;
        SetOtherChargeTaxValueOnRespectiveRow(0, s.GetValue(), gridTax.GetEditor("TaxName").GetText());
    }
    RecalCulateTaxTotalAmountCharges();
}
function OnTaxEndCallback(s, e) {
    GetPercentageData();
    $('.gridTaxClass').show();
    if (gridTax.GetVisibleRowsOnPage() == 0) {
        $('.gridTaxClass').hide();
        ccmbGstCstVatcharge.Focus();
    }
    else {
        gridTax.StartEditRow(0);
    }
    //check Json data
    if (gridTax.cpJsonChargeData) {
        if (gridTax.cpJsonChargeData != "") {
            chargejsonTax = JSON.parse(gridTax.cpJsonChargeData);
            gridTax.cpJsonChargeData = null;
        }
    }
    if (gridTax.cpChargesAmt) {
        ctxt_Charges.SetValue(gridTax.cpChargesAmt);
        gridTax.cpChargesAmt = null;
        Pre_Quantity = "0";
        Pre_Amt = "0";
        Pre_TotalAmt = "0";
        Cur_Quantity = "0";
        Cur_Amt = "0";
        Cur_TotalAmt = "0";
        CalculateAmount();
    }
    //Set Total Charges And total Amount
    if (gridTax.cpTotalCharges) {
        if (gridTax.cpTotalCharges != "") {
            ctxtQuoteTaxTotalAmt.SetValue(gridTax.cpTotalCharges);
            ctxtTotalAmount.SetValue(parseFloat(ctxtQuoteTaxTotalAmt.GetValue()) + parseFloat(ctxtProductNetAmount.GetValue()));
            gridTax.cpTotalCharges = null;
        }
    }
    SetChargesRunningTotal();
    ShowTaxPopUp("IN");
}
function QuotationTaxAmountGotFocus(s, e) {
    taxAmountGlobalCharges = parseFloat(s.GetValue());
}
function PercentageTextChange(s, e) {
    var Amount = gridTax.GetEditor("calCulatedOn").GetValue();
    var GlobalTaxAmt = 0;
    var Percentage = s.GetText();
    var totLength = gridTax.GetEditor("TaxName").GetText().length;
    var sign = gridTax.GetEditor("TaxName").GetText().substring(totLength - 3);
    Sum = ((parseFloat(Amount) * parseFloat(Percentage)) / 100);
    if (sign == '(+)') {
        GlobalTaxAmt = parseFloat(gridTax.GetEditor("Amount").GetValue());
        gridTax.GetEditor("Amount").SetValue(Sum);
        ctxtQuoteTaxTotalAmt.SetValue(parseFloat(ctxtQuoteTaxTotalAmt.GetValue()) + parseFloat(Sum) - GlobalTaxAmt);
        ctxtTotalAmount.SetValue(parseFloat(Amount) + parseFloat(ctxtQuoteTaxTotalAmt.GetValue()));
        GlobalTaxAmt = 0;
    }
    else {
        GlobalTaxAmt = parseFloat(gridTax.GetEditor("Amount").GetValue());
        gridTax.GetEditor("Amount").SetValue(Sum);
        ctxtQuoteTaxTotalAmt.SetValue(parseFloat(ctxtQuoteTaxTotalAmt.GetValue()) - parseFloat(Sum) + GlobalTaxAmt);
        ctxtTotalAmount.SetValue(parseFloat(Amount) + parseFloat(ctxtQuoteTaxTotalAmt.GetValue()));
        GlobalTaxAmt = 0;
    }
    SetOtherChargeTaxValueOnRespectiveRow(0, Sum, gridTax.GetEditor("TaxName").GetText());
    SetChargesRunningTotal();
    RecalCulateTaxTotalAmountCharges();
}
function RecalCulateTaxTotalAmountCharges() {
    var totalTaxAmount = 0;
    for (var i = 0; i < chargejsonTax.length; i++) {
        gridTax.batchEditApi.StartEdit(i, 3);
        var totLength = gridTax.GetEditor("TaxName").GetText().length;
        var sign = gridTax.GetEditor("TaxName").GetText().substring(totLength - 3);
        if (sign == '(+)') {
            totalTaxAmount = totalTaxAmount + parseFloat(gridTax.GetEditor("Amount").GetValue());
        } else {
            totalTaxAmount = totalTaxAmount - parseFloat(gridTax.GetEditor("Amount").GetValue());
        }
        gridTax.batchEditApi.EndEdit();
    }
    totalTaxAmount = totalTaxAmount + parseFloat(ctxtGstCstVatCharge.GetValue());
    ctxtQuoteTaxTotalAmt.SetValue(totalTaxAmount);
    ctxtTotalAmount.SetValue(parseFloat(ctxtQuoteTaxTotalAmt.GetValue()) + parseFloat(ctxtProductNetAmount.GetValue()));
}

function ShowTaxPopUp(type) {
    if (type == "IY") {
        $('#ContentErrorMsg').hide();
        $('#content-6').show();
        if (ccmbGstCstVat.GetItemCount() <= 1) {
            $('.InlineTaxClass').hide();
        } else {
            $('.InlineTaxClass').show();
        }
        if (cgridTax.GetVisibleRowsOnPage() < 1) {
            $('.cgridTaxClass').hide();

        } else {
            $('.cgridTaxClass').show();
        }
        if (ccmbGstCstVat.GetItemCount() <= 1 && cgridTax.GetVisibleRowsOnPage() < 1) {
            $('#ContentErrorMsg').show();
            $('#content-6').hide();
        }
    }
    if (type == "IN") {
        $('#ErrorMsgCharges').hide();
        $('#content-5').show();

        if (ccmbGstCstVatcharge.GetItemCount() <= 1) {
            $('.chargesDDownTaxClass').hide();
        } else {
            $('.chargesDDownTaxClass').show();
        }
        if (gridTax.GetVisibleRowsOnPage() < 1) {
            $('.gridTaxClass').hide();

        } else {
            $('.gridTaxClass').show();
        }
        if (ccmbGstCstVatcharge.GetItemCount() <= 1 && gridTax.GetVisibleRowsOnPage() < 1) {
            $('#ErrorMsgCharges').show();
            $('#content-5').hide();
        }
    }
}
function gridFocusedRowChanged(s, e) {
    globalRowIndex = e.visibleIndex;
}
function TaxAmountKeyDown(s, e) {
    if (e.htmlEvent.key == "Enter") {
        s.OnButtonClick(0);
    }
}
function taxAmountGotFocus(s, e) {
    taxAmountGlobal = parseFloat(s.GetValue());
}
function taxAmountLostFocus(s, e) {
    var finalTaxAmt = parseFloat(s.GetValue());
    var totAmt = parseFloat(ctxtTaxTotAmt.GetText());
    var totLength = cgridTax.GetEditor("Taxes_Name").GetText().length;
    var sign = cgridTax.GetEditor("Taxes_Name").GetText().substring(totLength - 3);
    if (sign == '(+)') {
        ctxtTaxTotAmt.SetValue(Math.round(totAmt + finalTaxAmt - taxAmountGlobal));
    } else {
        ctxtTaxTotAmt.SetValue(Math.round(totAmt + (finalTaxAmt * -1) - (taxAmountGlobal * -1)));
    }


    SetOtherTaxValueOnRespectiveRow(0, cgridTax.GetEditor("Amount").GetValue(), cgridTax.GetEditor("Taxes_Name").GetText().replace('(+)', '').replace('(-)', ''));
    //Set Running Total
    SetRunningTotal();

    RecalCulateTaxTotalAmountInline();
}
function ChargecmbGstCstVatChange(s, e) {
    SetOtherChargeTaxValueOnRespectiveRow(0, 0, ChargegstcstvatGlobalName);
    $('.RecalculateCharge').hide();
    var ProdAmt = parseFloat(ctxtProductAmount.GetValue());
    //Set ProductAmount
    if (s.GetValue().split('~')[2] == 'G') {
        ProdAmt = parseFloat(ctxtProductAmount.GetValue());
    }
    else if (s.GetValue().split('~')[2] == 'N') {
        ProdAmt = parseFloat(clblProdNetAmt.GetValue());
    }
    else if (s.GetValue().split('~')[2] == 'O') {
        //Check for Other Dependecy
        $('.RecalculateCharge').show();
        ProdAmt = 0;
        var taxdependentName = s.GetValue().split('~')[3];
        for (var i = 0; i < taxJson.length; i++) {
            gridTax.batchEditApi.StartEdit(i, 3);
            var gridTaxName = gridTax.GetEditor("TaxName").GetText();
            gridTaxName = gridTaxName.substring(0, gridTaxName.length - 3).trim();
            if (gridTaxName == taxdependentName) {
                ProdAmt = gridTax.GetEditor("Amount").GetValue();
            }
        }
    }
    else if (s.GetValue().split('~')[2] == 'R') {
        $('.RecalculateCharge').show();
        ProdAmt = GetChargesTotalRunningAmount();
    }
    GlobalCurChargeTaxAmt = parseFloat(ctxtGstCstVatCharge.GetText());
    var calculatedValue = parseFloat(ProdAmt * ccmbGstCstVatcharge.GetValue().split('~')[1]) / 100;
    ctxtGstCstVatCharge.SetValue(calculatedValue);
    var totAmt = parseFloat(ctxtQuoteTaxTotalAmt.GetText());
    ctxtQuoteTaxTotalAmt.SetValue(totAmt + calculatedValue - GlobalCurChargeTaxAmt);
    //tax others
    SetOtherChargeTaxValueOnRespectiveRow(0, calculatedValue, ctxtGstCstVatCharge.GetText());
    ChargegstcstvatGlobalName = ctxtGstCstVatCharge.GetText();
    //set Total Amount
    ctxtTotalAmount.SetValue(parseFloat(ctxtQuoteTaxTotalAmt.GetValue()) + parseFloat(ctxtProductNetAmount.GetValue()));
}
function GetChargesTotalRunningAmount() {
    var runningTot = parseFloat(ctxtProductNetAmount.GetValue());
    for (var i = 0; i < chargejsonTax.length; i++) {
        gridTax.batchEditApi.StartEdit(i, 3);
        runningTot = runningTot + parseFloat(gridTax.GetEditor("Amount").GetValue());
        gridTax.batchEditApi.EndEdit();
    }
    return runningTot;
}
function chargeCmbtaxClick(s, e) {
    GlobalCurChargeTaxAmt = parseFloat(ctxtGstCstVatCharge.GetText());
    ChargegstcstvatGlobalName = s.GetText();
}
function GetVisibleIndex(s, e) {
    globalRowIndex = e.visibleIndex;
}
function GetTaxVisibleIndex(s, e) {
    globalTaxRowIndex = e.visibleIndex;
}
function cmbtaxCodeindexChange(s, e) {
    if (cgridTax.GetEditor("Taxes_Name").GetText() == "GST/CST/VAT") {
        var taxValue = s.GetValue();
        if (taxValue == null) {
            taxValue = 0;
            GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());
            cgridTax.GetEditor("Amount").SetValue(0);
            ctxtTaxTotAmt.SetValue(parseFloat(ctxtTaxTotAmt.GetValue()) - GlobalCurTaxAmt);
        }
        var isValid = taxValue.indexOf('~');
        if (isValid != -1) {
            var rate = parseFloat(taxValue.split('~')[1]);
            var ProdAmt = parseFloat(ctxtprodBasicAmt.GetValue());
            GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());
            cgridTax.GetEditor("Amount").SetValue(parseFloat(ProdAmt * rate) / 100);
            ctxtTaxTotAmt.SetValue(parseFloat(ctxtTaxTotAmt.GetValue()) + (parseFloat(ProdAmt * rate) / 100) - GlobalCurTaxAmt);
            GlobalCurTaxAmt = 0;
        }
        else {
            s.SetText("");
        }
    } else {
        var ProdAmt = parseFloat(ctxtprodBasicAmt.GetValue());
        if (s.GetValue() == null) {
            s.SetValue(0);
        }
        if (!isNaN(parseFloat(ProdAmt * s.GetText()) / 100)) {
            GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());
            cgridTax.GetEditor("Amount").SetValue(parseFloat(ProdAmt * s.GetText()) / 100);
            ctxtTaxTotAmt.SetValue(parseFloat(ctxtTaxTotAmt.GetValue()) + (parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) - GlobalCurTaxAmt);
            GlobalCurTaxAmt = 0;
        } else {
            s.SetText("");
        }
    }
}

function SetOtherTaxValueOnRespectiveRow(idx, amt, name) {
    for (var i = 0; i < taxJson.length; i++) {
        if (taxJson[i].applicableBy == name) {
            cgridTax.batchEditApi.StartEdit(i, 3);
            cgridTax.GetEditor('calCulatedOn').SetValue(amt);
            var totLength = cgridTax.GetEditor("Taxes_Name").GetText().length;
            var taxNameWithSign = cgridTax.GetEditor("TaxField").GetText();
            var sign = cgridTax.GetEditor("Taxes_Name").GetText().substring(totLength - 3);
            var ProdAmt = parseFloat(cgridTax.GetEditor("calCulatedOn").GetValue());
            var s = cgridTax.GetEditor("TaxField");
            if (sign == '(+)') {
                GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());
                cgridTax.GetEditor("Amount").SetValue(parseFloat(ProdAmt * s.GetText()) / 100);

                ctxtTaxTotAmt.SetValue(parseFloat(ctxtTaxTotAmt.GetValue()) + (parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) - GlobalCurTaxAmt);
                GlobalCurTaxAmt = 0;
            }
            else {
                GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());
                cgridTax.GetEditor("Amount").SetValue((parseFloat(ProdAmt * s.GetText()) / 100) * -1);
                ctxtTaxTotAmt.SetValue(parseFloat(ctxtTaxTotAmt.GetValue()) + ((parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) * -1) - (GlobalCurTaxAmt * -1));
                GlobalCurTaxAmt = 0;
            }
        }
    }
    cgridTax.batchEditApi.EndEdit();
}
function SetOtherChargeTaxValueOnRespectiveRow(idx, amt, name) {
    name = name.substring(0, name.length - 3).trim();
    for (var i = 0; i < chargejsonTax.length; i++) {
        if (chargejsonTax[i].applicableBy == name) {
            gridTax.batchEditApi.StartEdit(i, 3);
            gridTax.GetEditor('calCulatedOn').SetValue(amt);
            var totLength = gridTax.GetEditor("TaxName").GetText().length;
            var taxNameWithSign = gridTax.GetEditor("Percentage").GetText();
            var sign = gridTax.GetEditor("TaxName").GetText().substring(totLength - 3);
            var ProdAmt = parseFloat(gridTax.GetEditor("calCulatedOn").GetValue());
            var s = gridTax.GetEditor("Percentage");
            if (sign == '(+)') {
                GlobalCurTaxAmt = parseFloat(gridTax.GetEditor("Amount").GetValue());
                gridTax.GetEditor("Amount").SetValue(parseFloat(ProdAmt * s.GetText()) / 100);

                ctxtTaxTotAmt.SetValue(parseFloat(ctxtTaxTotAmt.GetValue()) + (parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) - GlobalCurTaxAmt);
                GlobalCurTaxAmt = 0;
            }
            else {

                GlobalCurTaxAmt = parseFloat(gridTax.GetEditor("Amount").GetValue());
                gridTax.GetEditor("Amount").SetValue((parseFloat(ProdAmt * s.GetText()) / 100) * -1);

                ctxtTaxTotAmt.SetValue(parseFloat(ctxtTaxTotAmt.GetValue()) + ((parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) * -1) - (GlobalCurTaxAmt * -1));
                GlobalCurTaxAmt = 0;
            }
        }
    }
    gridTax.batchEditApi.EndEdit();
}
function txtPercentageLostFocus(s, e) {
    var ProdAmt = parseFloat(cgridTax.GetEditor("calCulatedOn").GetValue());
    if (s.GetText().trim() != '') {
        if (!isNaN(parseFloat(ProdAmt * s.GetText()) / 100)) {
            //Checking Add or less
            var totLength = cgridTax.GetEditor("Taxes_Name").GetText().length;
            var taxNameWithSign = cgridTax.GetEditor("TaxField").GetText();
            var sign = cgridTax.GetEditor("Taxes_Name").GetText().substring(totLength - 3);
            if (sign == '(+)') {
                GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());
                cgridTax.GetEditor("Amount").SetValue(parseFloat(ProdAmt * s.GetText()) / 100);
                ctxtTaxTotAmt.SetValue(Math.round(parseFloat(ctxtTaxTotAmt.GetValue()) + (parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) - GlobalCurTaxAmt));
                GlobalCurTaxAmt = 0;
            }
            else {

                GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());
                cgridTax.GetEditor("Amount").SetValue((parseFloat(ProdAmt * s.GetText()) / 100) * -1);
                ctxtTaxTotAmt.SetValue(Math.round(parseFloat(ctxtTaxTotAmt.GetValue()) + ((parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) * -1) - (GlobalCurTaxAmt * -1)));
                GlobalCurTaxAmt = 0;
            }
            SetOtherTaxValueOnRespectiveRow(0, cgridTax.GetEditor("Amount").GetValue(), cgridTax.GetEditor("Taxes_Name").GetText().replace('(+)', '').replace('(-)', ''));
            SetRunningTotal();

        } else {
            s.SetText("");
        }
    }
    RecalCulateTaxTotalAmountInline();
}

function RecalCulateTaxTotalAmountInline() {
    var totalInlineTaxAmount = 0;
    for (var i = 0; i < taxJson.length; i++) {
        cgridTax.batchEditApi.StartEdit(i, 3);
        var totLength = cgridTax.GetEditor("Taxes_Name").GetText().length;
        var sign = cgridTax.GetEditor("Taxes_Name").GetText().substring(totLength - 3);
        if (sign == '(+)') {
            totalInlineTaxAmount = totalInlineTaxAmount + parseFloat(cgridTax.GetEditor("Amount").GetValue());
        } else {
            totalInlineTaxAmount = totalInlineTaxAmount - parseFloat(cgridTax.GetEditor("Amount").GetValue());
        }
        cgridTax.batchEditApi.EndEdit();
    }
    totalInlineTaxAmount = totalInlineTaxAmount + parseFloat(ctxtGstCstVat.GetValue());
    ctxtTaxTotAmt.SetValue(totalInlineTaxAmount);
}
function SetRunningTotal() {
    var runningTot = parseFloat(clblProdNetAmt.GetValue());
    for (var i = 0; i < taxJson.length; i++) {
        cgridTax.batchEditApi.StartEdit(i, 3);
        if (taxJson[i].applicableOn == "R") {
            cgridTax.GetEditor("calCulatedOn").SetValue(runningTot);
            var totLength = cgridTax.GetEditor("Taxes_Name").GetText().length;
            var taxNameWithSign = cgridTax.GetEditor("TaxField").GetText();
            var sign = cgridTax.GetEditor("Taxes_Name").GetText().substring(totLength - 3);
            var ProdAmt = parseFloat(cgridTax.GetEditor("calCulatedOn").GetValue());
            var thisRunningAmt = 0;
            if (sign == '(+)') {
                GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());
                cgridTax.GetEditor("Amount").SetValue(parseFloat(ProdAmt * cgridTax.GetEditor("TaxField").GetValue()) / 100);
                ctxtTaxTotAmt.SetValue(parseFloat(ctxtTaxTotAmt.GetValue()) + (parseFloat(ProdAmt * parseFloat(cgridTax.GetEditor("TaxField").GetValue())) / 100) - GlobalCurTaxAmt);
                GlobalCurTaxAmt = 0;
            }
            else {
                GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());
                cgridTax.GetEditor("Amount").SetValue((parseFloat(ProdAmt * cgridTax.GetEditor("TaxField").GetValue()) / 100) * -1);
                ctxtTaxTotAmt.SetValue(parseFloat(ctxtTaxTotAmt.GetValue()) + ((parseFloat(ProdAmt * parseFloat(cgridTax.GetEditor("TaxField").GetValue())) / 100) * -1) - (GlobalCurTaxAmt * -1));
                GlobalCurTaxAmt = 0;
            }
            SetOtherTaxValueOnRespectiveRow(0, cgridTax.GetEditor("Amount").GetValue(), cgridTax.GetEditor("Taxes_Name").GetText().replace('(+)', '').replace('(-)', ''));
        }
        runningTot = runningTot + parseFloat(cgridTax.GetEditor("Amount").GetValue());
        cgridTax.batchEditApi.EndEdit();
    }
}

function GetTotalRunningAmount() {
    var runningTot = parseFloat(clblProdNetAmt.GetValue());
    for (var i = 0; i < taxJson.length; i++) {
        cgridTax.batchEditApi.StartEdit(i, 3);
        runningTot = runningTot + parseFloat(cgridTax.GetEditor("Amount").GetValue());
        cgridTax.batchEditApi.EndEdit();
    }
    return runningTot;
}
function CmbtaxClick(s, e) {
    GlobalCurTaxAmt = parseFloat(ctxtGstCstVat.GetText());
    gstcstvatGlobalName = s.GetText();
}
function txtTax_TextChanged(s, i, e) {
    cgridTax.batchEditApi.StartEdit(i, 2);
    var ProdAmt = parseFloat(ctxtprodBasicAmt.GetValue());
    cgridTax.GetEditor("Amount").SetValue(parseFloat(ProdAmt * s.GetText()) / 100);
}
function NonInventoryBatchUpdate() {
    var cnt = cgridinventory.GetVisibleItemsOnPage();
    //cgridinventory.PerformCallback('0' + '~' + '0' + '~' + 'SaveNonInventoryProductChg' + '~' + cnt); 
    cgridinventory.AddNewRow();
    cgridinventory.batchEditApi.StartEdit(0, 3);
    cgridinventory.batchEditApi.EndEdit();
    if (cgridinventory.GetVisibleRowsOnPage() > 0) {
        cgridinventory.UpdateEdit();
    }
    //cgridinventory.UpdateEdit();
    return false;
}
function BatchUpdate() {

    $('#ddl_numberingScheme').prop('disabled', true);
    //gridLookup.SetEnabled(false);
    ctxtVendorName.SetEnabled(false);

    if (cgridTax.GetVisibleRowsOnPage() > 0) {
        cgridTax.UpdateEdit();
    }
    else {
        cgridTax.PerformCallback('SaveGST');
    }
    return false;
}
function cgridTax_EndCallBack(s, e) {
    $('.cgridTaxClass').show();
    cgridTax.StartEditRow(0);
    //check Json data
    if (cgridTax.cpJsonData) {
        if (cgridTax.cpJsonData != "") {
            taxJson = JSON.parse(cgridTax.cpJsonData);
            cgridTax.cpJsonData = null;
        }
    }
    //End Here 
    if (cgridTax.cpComboCode) {
        if (cgridTax.cpComboCode != "") {
            if (cddl_AmountAre.GetValue() == "1") {
                var selectedIndex;
                for (var i = 0; i < ccmbGstCstVat.GetItemCount() ; i++) {
                    if (ccmbGstCstVat.GetItem(i).value.split('~')[0] == cgridTax.cpComboCode) {
                        selectedIndex = i;
                    }
                }
                if (selectedIndex && ccmbGstCstVat.GetItem(selectedIndex) != null) {
                    ccmbGstCstVat.SetValue(ccmbGstCstVat.GetItem(selectedIndex).value);
                }
                cgridTax.cpComboCode = null;
            }
        }
    }
    if (cgridTax.cpUpdated.split('~')[0] == 'ok') {
        ctxtTaxTotAmt.SetValue(Math.round(cgridTax.cpUpdated.split('~')[1]));
        var gridValue = parseFloat(cgridTax.cpUpdated.split('~')[1]);
        var ddValue = parseFloat(ctxtGstCstVat.GetValue());
        ctxtTaxTotAmt.SetValue(Math.round(gridValue + ddValue));
        cgridTax.cpUpdated = "";
    }
    else {
        var totAmt = ctxtTaxTotAmt.GetValue();
        cgridTax.CancelEdit();
        caspxTaxpopUp.Hide();
        grid.batchEditApi.StartEdit(globalRowIndex, 13);
        grid.GetEditor("TaxAmount").SetValue(totAmt);
        //  Code Commented by Sam on 19122017 to set Net Amount for Inclusive Tax Section Start
        //grid.GetEditor("TotalAmount").SetValue(DecimalRoundoff(parseFloat(totAmt) + parseFloat(grid.GetEditor("Amount").GetValue()), 2));
        //  Code Commented by Sam on 19122017 to set Net Amount for Inclusive Tax Section End
        if (cddl_AmountAre.GetValue() == '2') {

            //  Code Commented by Sam on 19122017 to set Net Amount for Inclusive Tax Section Start
            var prodqty = (grid.GetEditor("Quantity").GetValue());
            var prodrate = (grid.GetEditor("PurchasePrice").GetValue());
            var prodDiscAmt = (grid.GetEditor("Discountamt").GetValue());
            var prodNetAmt = (parseFloat(prodqty) * parseFloat(prodrate)) - parseFloat(prodDiscAmt)
            grid.GetEditor("TotalAmount").SetValue(parseFloat(prodNetAmt));
            //var prodNetAmt = (grid.GetEditor("TotalAmount").GetValue());
            var prodIncluSiveTax = (grid.GetEditor("TaxAmount").GetValue());
            grid.GetEditor("Amount").SetValue(parseFloat(prodNetAmt) - parseFloat(prodIncluSiveTax));
            //var totalNetAmount = parseFloat(totAmt) + parseFloat(grid.GetEditor("Amount").GetValue());
            //var totalRoundOffAmount = Math.round(totalNetAmount);
            //grid.GetEditor("Amount").SetValue(parseFloat(grid.GetEditor("Amount").GetValue()) + (totalRoundOffAmount - totalNetAmount));
            //  Code Commented by Sam on 19122017 to set Net Amount for Inclusive Tax Section End
        }
        else {
            grid.GetEditor("TotalAmount").SetValue(DecimalRoundoff(parseFloat(totAmt) + parseFloat(grid.GetEditor("Amount").GetValue()), 2));
        }
        // Running total Calculation Start
        Cur_Quantity = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
        Cur_Amt = (grid.GetEditor('Amount').GetValue() != null) ? grid.GetEditor('Amount').GetValue() : "0";
        Cur_TotalAmt = (grid.GetEditor('TotalAmount').GetValue() != null) ? grid.GetEditor('TotalAmount').GetValue() : "0";
        CalculateAmount();
        // Running total Calculation End 
    }
    if (cgridTax.GetVisibleRowsOnPage() == 0) {
        $('.cgridTaxClass').hide();
        ccmbGstCstVat.Focus();
    }
    //Debjyoti Check where any Gst Present or not
    // If Not then hide the hole section 
    SetRunningTotal();
    ShowTaxPopUp("IY");
    RecalCulateTaxTotalAmountInline();
}
function recalculateTax() {
    cmbGstCstVatChange(ccmbGstCstVat);
}
function recalculateTaxCharge() {
    ChargecmbGstCstVatChange(ccmbGstCstVatcharge);
}
/*............................End Tax...........................................*/

function componentEndCallBack(s, e) {
    var loadingmode = $('#hdnADDEditMode').val();
    if (loadingmode != 'Edit') {
        gridquotationLookup.gridView.Refresh();
        cProductsPopup.Hide();
    }
    if (grid.GetVisibleRowsOnPage() == 0) {
        OnAddNewClick();
    }
    // Tagging Component Section Start
    if (cQuotationComponentPanel.cpDetails != null) {
        var details = cQuotationComponentPanel.cpDetails;
        cQuotationComponentPanel.cpDetails = null;
        var SpliteDetails = details.split("~");
        var Reference = SpliteDetails[0];
        var Currency_Id = SpliteDetails[1];
        var SalesmanId = SpliteDetails[2];
        var ExpiryDate = SpliteDetails[3];
        var CurrencyRate = SpliteDetails[4];
        ctxt_Refference.SetValue(Reference);
        ctxtRate.SetValue(CurrencyRate);
        document.getElementById('ddl_Currency').value = Currency_Id;
        document.getElementById('ddl_SalesAgent').value = SalesmanId;
    }
    gridquotationLookup.Focus();
}
function CloseGridQuotationLookup() {
    gridquotationLookup.ConfirmCurrentSelection();
    gridquotationLookup.HideDropDown();
}
function CloseTaggedPBLookup() {
    clookup_TaggedPB.ConfirmCurrentSelection();
    clookup_TaggedPB.HideDropDown();
}
//function CloseTaggedSPBLookup() {
//    clookup_TaggedSPB.ConfirmCurrentSelection();
//    clookup_TaggedSPB.HideDropDown();
//}
function QuotationNumberChanged() {
    var quote_Id = gridquotationLookup.GetValue();
    if (quote_Id != null) {
        cgridproducts.PerformCallback('BindProductsDetails' + '~' + '@' + '~' + quote_Id);
        cProductsPopup.Show();
    }
    else {
        cgridproducts.PerformCallback('BindProductsDetails' + '~' + '$');
        cProductsPopup.Show();
    }
}
function SetDifference1() {
    var diff = CheckDifferenceOfFromDateWithTodate();
}
function CheckDifferenceOfFromDateWithTodate() {
    var startDate = new Date();
    var endDate = new Date();
    var difference = -1;
    startDate = cPLSalesOrderDate.GetDate();
    if (startDate != null) {
        endDate = cExpiryDate.GetDate();
        var startTime = startDate.getTime();
        var endTime = endDate.getTime();
        difference = (startTime - endTime) / 86400000;

    }
    return difference;

}
function SetDifference() {
    var diff = CheckDifference();
}
function CheckDifference() {
    var startDate = new Date();
    var endDate = new Date();
    var difference = -1;
    startDate = cPLSalesOrderDate.GetDate();
    if (startDate != null) {
        endDate = cExpiryDate.GetDate();
        var startTime = startDate.getTime();
        var endTime = endDate.getTime();
        difference = (endTime - startTime) / 86400000;

    }
    return difference;

}

function OnInventoryEndCallback(s, e) {
    if (cgridinventory.cpPopupReq == 'N') {
        OnAddNewClick();
        jAlert('Applicable amount is greater than entered amount.TDS is not required for this amount.')
        return;
    }
    else if (cgridinventory.cpgrid == "Y") {
        cgridinventory.batchEditApi.StartEdit(0, 2);
        //var checkval = cchk_TDSEditable.GetChecked();
        //if (checkval) {
        //    cgridinventory.SetEnabled(true);
        //}
        //else
        //{
        //    cgridinventory.SetEnabled(false);
        //}
        if (cgridinventory.cpbrMonAmtDtl != null) {
            var tdsbranch = cgridinventory.cpbrMonAmtDtl.toString().split('~')[0];
            var tdsmonth = cgridinventory.cpbrMonAmtDtl.toString().split('~')[1];
            var tdsTotalAmt = cgridinventory.cpbrMonAmtDtl.toString().split('~')[2];
            var tdsProbAmt = cgridinventory.cpbrMonAmtDtl.toString().split('~')[3];
            //var TDSEdit = cgridinventory.cpbrMonAmtDtl.toString().split('~')[4];
            //if (TDSEdit == '1')
            //{
            //    cchk_TDSEditable.SetValue(true);
            //}
            //else
            //{
            //    cchk_TDSEditable.SetValue(false);
            //}
            var branchid = $("#ddl_Branch option:selected").text();
            $('#lbltdsBranch').text(branchid);
            if (cgridinventory.cpmonth != null) {
                var monthnm = cgridinventory.cpmonth;
                if (monthnm == '1') {
                    cddl_month.SetValue('January');
                }
                else if (monthnm == '2') {
                    cddl_month.SetValue('February');
                }
                else if (monthnm == '3') {
                    cddl_month.SetValue('March');
                }
                else if (monthnm == '4') {
                    cddl_month.SetValue('April');
                }
                else if (monthnm == '5') {
                    cddl_month.SetValue('May');
                }
                else if (monthnm == '6') {
                    cddl_month.SetValue('June');
                }
                else if (monthnm == '7') {
                    cddl_month.SetValue('July');
                }
                else if (monthnm == '8') {
                    cddl_month.SetValue('August');
                }
                else if (monthnm == '9') {
                    cddl_month.SetValue('September');
                }
                else if (monthnm == '10') {
                    cddl_month.SetValue('October');
                }
                else if (monthnm == '11') {
                    cddl_month.SetValue('November');
                }
                else if (monthnm == '12') {
                    cddl_month.SetValue('December');
                }
            }
            cddl_month.SetValue(tdsmonth);
            if (tdsTotalAmt != null && tdsTotalAmt != '') {
                ctxt_totalnoninventoryproductamt.SetText(Math.round(tdsTotalAmt));
            }
            ctxt_proamt.SetText(tdsProbAmt);
            cgridinventory.cpgrid = null;
        }
        else if (cgridinventory.cpbrMonAmtDtl == null) {
            ctxt_proamt.SetText(parseFloat(cgridinventory.cpproamt));
            ctxt_totalnoninventoryproductamt.SetText(Math.round(parseFloat(cgridinventory.cpprochargeamt)));
            var branchid = $("#ddl_Branch option:selected").text();
            $('#lbltdsBranch').text(branchid);
            if (cgridinventory.cpmonth != null) {
                var monthnm = cgridinventory.cpmonth;
                if (monthnm == '1') {
                    cddl_month.SetValue('January');
                }
                else if (monthnm == '2') {
                    cddl_month.SetValue('February');
                }
                else if (monthnm == '3') {
                    cddl_month.SetValue('March');
                }
                else if (monthnm == '4') {
                    cddl_month.SetValue('April');
                }
                else if (monthnm == '5') {
                    cddl_month.SetValue('May');
                }
                else if (monthnm == '6') {
                    cddl_month.SetValue('June');
                }
                else if (monthnm == '7') {
                    cddl_month.SetValue('July');
                }
                else if (monthnm == '8') {
                    cddl_month.SetValue('August');
                }
                else if (monthnm == '9') {
                    cddl_month.SetValue('September');

                }
                else if (monthnm == '10') {
                    cddl_month.SetValue('October');
                }
                else if (monthnm == '11') {
                    cddl_month.SetValue('November');
                }
                else if (monthnm == '12') {
                    cddl_month.SetValue('December');
                }
            }
        }
        cinventorypopup.Show();
        cgridinventory.cpgrid = null;
    }
    else if (cgridinventory.cppopuphide == "Y") {
        var branchid = $("#ddl_Branch option:selected").text();
        $('#lbltdsBranch').text(branchid);
        cddl_month.SetSelectedIndex(-1);
        ctxt_proamt.SetText('');
        ctxt_totalnoninventoryproductamt.SetText('');
        cinventorypopup.Hide();
        OnAddNewClick();
    }
}

function GridCallBack() {
    grid.PerformCallback('Display');
}
function TDSDetail(s, e) {
    var inventoryItem = $('#ddlInventory').val();
    if (inventoryItem == 'N') {
        document.getElementById('hdnInvWiseSlno').value = grid.GetEditor('SrlNo').GetText();
        var Productdtl = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "";
        var Amt = (grid.GetEditor('Amount').GetText() != null) ? grid.GetEditor('Amount').GetText() : "0";
        var Productdt = Productdtl.split("||@||");
        var ProductID = Productdt[0];
        ctxt_proamt.SetText('Amt');
        cgridinventory.PerformCallback(ProductID + '~' + Amt + '~' + 'ShowBindGrid');
    }
}
function OnCustomButtonClick(s, e) {
    if (e.buttonID == 'CustomDelete') {
        grid.batchEditApi.StartEdit();
        var SrlNo = grid.batchEditApi.GetCellValue(e.visibleIndex, 'SrlNo');
        $('#hdnRefreshType').val('');
        $('#hdnDeleteSrlNo').val(SrlNo);
        var inventoryItem = $('#ddlInventory').val();
        if (inventoryItem == 'Y' || inventoryItem == 'B') {
            if (gridquotationLookup.GetValue() != null) {
                var type = ($("[id$='rdl_PurchaseInvoice']").find(":checked").val() != null) ? $("[id$='rdl_PurchaseInvoice']").find(":checked").val() : "";
                if (type == 'PO') {

                    jAlert('Cannot Delete using this button as the Purchase Order is linked with this Purchase Invoice.<br/>Click on Plus(+) sign to Add or Delete Product from last column.!', 'Alert Dialog', function (r) { });
                }
                else if (type == 'PC') {
                    $('#hdnDeleteSrlNo').val('');
                    jAlert('Cannot Delete using this button as the Purchase Challan is linked with this Purchase Invoice.<br/>Click on Plus(+) sign to Add or Delete Product from last column.!', 'Alert Dialog', function (r) { });
                }
            }
            else {
                var noofvisiblerows = grid.GetVisibleRowsOnPage();
                if (noofvisiblerows != "1" && gridquotationLookup.GetValue() == null) {
                    // Running total Calculation Start 
                    Pre_Quantity = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
                    Pre_Amt = (grid.GetEditor('Amount').GetValue() != null) ? grid.GetEditor('Amount').GetValue() : "0";
                    Pre_TotalAmt = (grid.GetEditor('TotalAmount').GetValue() != null) ? grid.GetEditor('TotalAmount').GetValue() : "0";
                    Cur_Quantity = "0";
                    Cur_Amt = "0";
                    Cur_TotalAmt = "0";
                    CalculateAmount();
                    // Running total Calculation End 
                    grid.DeleteRow(e.visibleIndex);
                    if (inventoryItem == 'N') {
                        $('#hdinvetorttype').val('N');
                    }
                    $('#hdfIsDelete').val('D');
                    grid.UpdateEdit();
                    $('#hdnPageStatus').val('delete');
                }
            }
        }
        else {
            var noofvisiblerows = grid.GetVisibleRowsOnPage();
            if (noofvisiblerows != "1") {
                // Running total Calculation Start 
                Pre_Quantity = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
                Pre_Amt = (grid.GetEditor('Amount').GetValue() != null) ? grid.GetEditor('Amount').GetValue() : "0";
                Pre_TotalAmt = (grid.GetEditor('TotalAmount').GetValue() != null) ? grid.GetEditor('TotalAmount').GetValue() : "0";
                Cur_Quantity = "0";
                Cur_Amt = "0";
                Cur_TotalAmt = "0";
                CalculateAmount();

                // Running total Calculation End 
                grid.DeleteRow(e.visibleIndex);
                if (inventoryItem == 'N') {
                    $('#hdinvetorttype').val('N');
                }
                $('#hdfIsDelete').val('D');
                grid.UpdateEdit();
                $('#hdnPageStatus').val('delete');
            }
        }
    }
    if (e.buttonID == 'CustomAddNewRow') {
        grid.batchEditApi.StartEdit(e.visibleIndex);

        var ProductID = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "";
        var scembranch = '';
        if (ProductID != "") {
            var inventoryItem = $('#ddlInventory').val();
            if (inventoryItem == 'N') {
                //var customerval = gridLookup.GetValue();
                var customerval = GetObjectID('hdnCustomerId').value;
                var vendorid = GetObjectID('hdnCustomerId').value
                if (customerval == '' || customerval == null) {
                    jAlert('Select a Transporter first');
                    return
                }
                else {
                    var Productdtl = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "";
                    if (Productdtl != null && Productdtl != '') {
                        var Productdt = Productdtl.split("||@||");
                        var ProductID = Productdt[0];
                        var schemeid = cddl_TdsScheme.GetValue()
                        if (schemeid != '0') {
                            document.getElementById('hdnInvWiseSlno').value = grid.GetEditor('SrlNo').GetText();
                            var Productdtl = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "";
                            var Amt = (grid.GetEditor('Amount').GetText() != null) ? grid.GetEditor('Amount').GetText() : "0";
                            var Productdt = Productdtl.split("||@||");
                            var ProductID = Productdt[0];
                            ctxt_proamt.SetText('Amt');
                            // Code Added by sam to set focus on TDS gridview on 05022018
                            cgridinventory.batchEditApi.StartEdit();
                            // Code Above Added by sam to set focus on TDS gridview on 05022018
                            cgridinventory.PerformCallback(ProductID + '~' + Amt + '~' + 'ShowBindGrid' + '~' + 'CheckApplicableAmt');
                            var slno = grid.GetEditor('SrlNo').GetValue();
                            if ($('#hdntdschecking').val() != '') {
                                var myArray = $('#hdntdschecking').val().split(',');
                                var index = myArray.indexOf(slno);
                                if (index > -1) {
                                    myArray.splice(index, 1);
                                    $('#hdntdschecking').val(myArray);
                                }

                            }
                            cgridinventory.batchEditApi.EndEdit();
                        }
                        else {
                            OnAddNewClick();
                        }
                    }
                    else {
                        jAlert('Select a Product first.');
                        return;
                    }
                }
            }
            else if (gridquotationLookup.GetValue() == null) {
                var ProductID = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "";
                var SpliteDetails = ProductID.split("||@||");
                var IsComponentProduct = SpliteDetails[16];
                var ComponentProduct = SpliteDetails[17];
                if (IsComponentProduct == "Y") {
                    var messege = "Selected product is defined with components.<br/> Would you like to proceed with components (" + ComponentProduct + ") ?";
                    jConfirm(messege, 'Confirmation Dialog', function (r) {
                        if (r == true) {
                            grid.GetEditor("IsComponentProduct").SetValue("Y");
                            $('#hdfIsComp').val('C');
                            grid.UpdateEdit();
                            grid.PerformCallback('Display~fromComponent');
                        }
                        else {
                            OnAddNewClick();
                        }
                    });
                    document.getElementById('popup_ok').focus();
                }
                    // Component tagging Section End by Sam
                else if (ProductID != "") {
                    OnAddNewClick();
                }
                else {
                    grid.batchEditApi.StartEdit(e.visibleIndex, 2);
                }
            }
            else {
                var type = ($("[id$='rdl_PurchaseInvoice']").find(":checked").val() != null) ? $("[id$='rdl_PurchaseInvoice']").find(":checked").val() : "";
                if (type == 'PO') {
                    cgridproducts.SetEnabled(false);
                }
                else if (type == 'PC') {
                    cgridproducts.SetEnabled(false);
                }
                QuotationNumberChanged();
            }
        }
        else {
            jAlert('Select a product first');
            grid.GetEditor("ProductName").Focus();
            return;
        }
    }

    else if (e.buttonID == "addlDesc") {

        var index = e.visibleIndex;
        grid.batchEditApi.StartEdit(e.visibleIndex, 5);
        cPopup_InlineRemarks.Show();

        $("#txtInlineRemarks").val('');

        var SrlNo = (grid.GetEditor('SrlNo').GetValue() != null) ? grid.GetEditor('SrlNo').GetValue() : "";
        var ProductID = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "";
        if (ProductID != "") {
            // ccallback_InlineRemarks.PerformCallback('BindRemarks'+'~' + '0'+'~'+'0');
            ccallback_InlineRemarks.PerformCallback('DisplayRemarks' + '~' + SrlNo + '~' + '0');

        }
    }

    else if (e.buttonID == 'CustomWarehouse') {
        var inventoryItem = $('#ddlInventory').val();
        if (inventoryItem == 'N') {
            jAlert("NonInventory Item has no warehouse detail.")
            return
        }
        var index = e.visibleIndex;
        grid.batchEditApi.StartEdit(index, 2)
        Warehouseindex = index;
        document.getElementById('hdngridvselectedrowno').value = e.visibleIndex;
        var SrlNo = (grid.GetEditor('SrlNo').GetValue() != null) ? grid.GetEditor('SrlNo').GetValue() : "";
        var ProductID = (grid.GetEditor('ProductID').GetValue() != null) ? grid.GetEditor('ProductID').GetValue() : "";
        var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
        var type = ($("[id$='rdl_PurchaseInvoice']").find(":checked").val() != null) ? $("[id$='rdl_PurchaseInvoice']").find(":checked").val() : "";
        if (type != 'PC' && type != 'PO') {
            return;
        }
        if (QuantityValue == "0.0") {
            jAlert("Quantity should not be zero !.");
        }
        else {
            if (type == 'PC' || type == 'PO') {
                cbtnaddWarehouse.SetVisible(false); // During Tagging Add button should be Enabled false
                cbtnWarehouse.SetVisible(false);
                cButton1.SetVisible(false);
                cbtnrefreshWarehouse.SetVisible(false);
                cCmbWarehouse.SetEnabled(false);
            }
            else {

                cbtnaddWarehouse.SetVisible(true);
                cbtnWarehouse.SetVisible(true);
                cButton1.SetVisible(true);
                cbtnrefreshWarehouse.SetVisible(true);
                cCmbWarehouse.SetEnabled(true);
            }
            $("#spnCmbWarehouse").hide();  // First Hide during stock detail clicking
            $("#spnCmbBatch").hide();      // First Hide during stock detail clicking
            $("#spncheckComboBox").hide(); // First Hide during stock detail clicking
            $("#spntxtQuantity").hide();   // First Hide during stock detail clicking
            //alert(ProductID);
            if (ProductID != "") {
                var SpliteDetails = ProductID.split("||@||");
                var strProductID = SpliteDetails[0];
                var strDescription = SpliteDetails[1];
                var strUOM = SpliteDetails[2];
                var strStkUOM = SpliteDetails[4];
                var strMultiplier = SpliteDetails[7];
                var strProductName = (grid.GetEditor('ProductName').GetText() != null) ? grid.GetEditor('ProductName').GetText() : "";
                var StkQuantityValue = QuantityValue * strMultiplier;
                $('#hdfProductIDPC').val(strProductID);  // assign Productid of the selected row
                $('#hdfProductType').val("");            // assign Producttype of the selected row
                $('#hdfProductSerialID').val(SrlNo);     // assign sl no of the selected row 
                $('#hdnProductQuantity').val(QuantityValue); // assign Product qty of the selected row
                var Ptype = "";
                $('#hdnisserial').val("");        // serial id is black initialized in first time
                $('#hdnisbatch').val("");     // Batch id is black initialized in first time
                $('#hdniswarehouse').val("");    // Warehouse id is black initialized in first time 
                $.ajax({
                    type: "POST",
                    url: 'PurchaseInvoiceforTransporter.aspx/getProductType',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: "{Products_ID:\"" + strProductID + "\"}",
                    success: function (type) {
                        Ptype = type.d;
                        $('#hdfProductType').val(Ptype);
                        if (Ptype == "") {
                            jAlert("No Warehouse or Batch or Serial is actived !.");
                        } else {
                            if (Ptype == "W") {
                                $('#hdnisserial').val("false");
                                $('#hdnisbatch').val("false");
                                $('#hdniswarehouse').val("true");
                            }
                            else if (Ptype == "B") {
                                $('#hdnisserial').val("false");
                                $('#hdnisbatch').val("true");
                                $('#hdniswarehouse').val("false");
                            }
                            else if (Ptype == "S") {
                                $('#hdnisserial').val("true");
                                $('#hdnisbatch').val("false");
                                $('#hdniswarehouse').val("false");
                            }
                            else if (Ptype == "WB") {
                                $('#hdnisserial').val("false");
                                $('#hdnisbatch').val("true");
                                $('#hdniswarehouse').val("true");
                            }
                            else if (Ptype == "WS") {
                                $('#hdnisserial').val("true");
                                $('#hdnisbatch').val("false");
                                $('#hdniswarehouse').val("true");
                            }
                            else if (Ptype == "WBS") {
                                $('#hdnisserial').val("true");
                                $('#hdnisbatch').val("true");
                                $('#hdniswarehouse').val("true");
                            }
                            else if (Ptype == "BS") {
                                $('#hdnisserial').val("true");
                                $('#hdnisbatch').val("true");
                                $('#hdniswarehouse').val("false");
                            }
                            else {
                                $('#hdnisserial').val("false");
                                $('#hdnisbatch').val("false");
                                $('#hdniswarehouse').val("false");
                            }
                            $("#RequiredFieldValidatortxtbatch").css("display", "none");         // validation are hidden in starting for batch detail txt box
                            $("#RequiredFieldValidatortxtserial").css("display", "none");        // validation are hidden in starting for serial detail txt box
                            $("#RequiredFieldValidatorCmbWarehouse").css("display", "none");     // validation are hidden in starting for ware house detail txt box 
                            $("#RequiredFieldValidatortxtbatchqntity").css("display", "none");   // validation are hidden in starting for batch qty txt box
                            $("#RequiredFieldValidatortxtwareqntity").css("display", "none");    // validation are hidden in starting for warehouse qty txt box 
                            $(".blockone").css("display", "none");                // this div is hidden for warehouse detail in starting
                            $(".blocktwo").css("display", "none");                 // this div is hidden for Batch detail in starting
                            $(".blockthree").css("display", "none");             // this div is hidden for Serial detail in starting 
                            ctxtqnty.SetText("0.0");            // for warehouse quantity text box
                            ctxtqnty.SetEnabled(true);
                            ctxtbatchqnty.SetText("0.0");       // for Batch quantity text box not for detail text box
                            ctxtserial.SetText("");             // for serial  detail text box
                            ctxtbatchqnty.SetText("");
                            ctxtbatch.SetEnabled(true);        // for Batch Number text box
                            cCmbWarehouse.SetEnabled(true);
                                        // starting Phase
                            $('#hdnisedited').val("false");           // starting Phase
                            $('#hdnisoldupdate').val("false");        // starting Phase
                            $('#hdnisnewupdate').val("false");        // starting Phase 
                            $('#hdnisolddeleted').val("false");       // starting Phase 
                            $('#hdntotalqntyPC').val(0);              // starting Phase
                            $('#hdnoldrowcount').val(0);              // starting Phase
                            $('#hdndeleteqnity').val(0);              // starting Phase
                            $('#hidencountforserial').val("1");       // starting Phase 
                            $('#hdfstockidPC').val(0);               // starting Phase
                            $('#hdfopeningstockPC').val(0);          // starting Phase
                            $('#oldopeningqntity').val(0);           // starting Phase
                            $('#hdnnewenterqntity').val(0);          // starting Phase 
                            $('#hdnenterdopenqnty').val(0);         // starting Phase
                            $('#hdbranchIDPC').val(0);              // starting Phase 
                            $('#hdnisviewqntityhas').val("false");  // starting Phase 
                            $('#hdndefaultID').val("");             // starting Phase
                            $('#hdnbatchchanged').val("0");        // starting Phase
                            $('#hdnrate').val("0");                // starting Phase
                            $('#hdnvalue').val("0");               // starting Phase
                            $('#hdnstrUOM').val(strUOM);          // starting Phase 
                            var branchid = $("#ddl_Branch option:selected").val();
                            
                            var productid = SpliteDetails[0] ? SpliteDetails[0] : "";
                            var StkQuantityValue = QuantityValue ? QuantityValue : "0.0000";
                            var stockids = 0;
                            var checkstockdecimalornot = StkQuantityValue.toString().split(".")[1]
                            $('#hdnpcslno').val(SrlNo);
                            var ProductName = SpliteDetails[1] ? SpliteDetails[1] : "";
                            var ratevalue = "0";
                            var rate = "0";
                            var branchid = $('#ddl_Branch').val();
                            var BranchNames = $("#ddl_Branch option:selected").text();
                            var strProductID = productid;
                            var strDescription = "";
                            var strUOM = (strUOM != null) ? strUOM : "0";
                            var strProductName = ProductName;
                            document.getElementById('lblbranchName').innerHTML = BranchNames;
                            var availablestock = SpliteDetails[12];
                            $('#hdndefaultID').val("0");
                            $('#hdfstockidPC').val(stockids);
                            var calculateopein = Number(StkQuantityValue) - Number(availablestock);
                            var oldopeing = 0;
                            var oldqnt = Number(oldopeing);
                            $('#hdfopeningstockPC').val(QuantityValue);
                            $('#oldopeningqntity').val(0);
                            $('#hdnnewenterqntity').val(QuantityValue);
                            $('#hdnenterdopenqnty').val(0);
                            $('#hdbranchIDPC').val(branchid);
                            $('#hdnselectedbranch').val(branchid);
                            $('#hdnrate').val(rate);
                            $('#hdnvalue').val(ratevalue);
                            var dtd = (Number(StkQuantityValue)).toFixed(4);
                            $("#lblopeningstock").text(dtd + " " + strUOM);
                            ctxtmkgdate.SetDate = null;
                            txtexpirdate.SetDate = null;
                            ctxtserial.SetValue("");
                            ctxtbatch.SetValue("");
                            ctxtqnty.SetValue("0.0");
                            ctxtbatchqnty.SetValue("0.0");
                            var hv = $('#hdnselectedbranch').val();
                            var iswarehousactive = $('#hdniswarehouse').val();
                            var isactivebatch = $('#hdnisbatch').val();
                            var isactiveserial = $('#hdnisserial').val();
                            cCmbWarehouse.PerformCallback('BindWarehouse');
                            if (iswarehousactive == "true") {    // If Product type Has WH or W Type

                                cCmbWarehouse.SetVisible(true);
                                cCmbWarehouse.SetSelectedIndex(1);
                                cCmbWarehouse.Focus();
                                ctxtqnty.SetVisible(true);
                                $('#hdniswarehouse').val("true");
                                $(".blockone").css("display", "block");

                            } else {
                                cCmbWarehouse.SetVisible(false);
                                ctxtqnty.SetVisible(false);
                                $('#hdniswarehouse').val("false");
                                cCmbWarehouse.SetSelectedIndex(-1);
                                $(".blockone").css("display", "none");
                            }
                            if (isactivebatch == "true") {
                                ctxtbatch.SetVisible(true);
                                ctxtmkgdate.SetVisible(true);
                                ctxtexpirdate.SetVisible(true);
                                $('#hdnisbatch').val("true");
                                $(".blocktwo").css("display", "block");
                            } else {
                                ctxtbatch.SetVisible(false);
                                ctxtmkgdate.SetVisible(false);
                                ctxtexpirdate.SetVisible(false);
                                $('#hdnisbatch').val("false");
                                $(".blocktwo").css("display", "none");
                            }
                            if (isactiveserial == "true") {
                                ctxtserial.SetVisible(true);
                                $('#hdnisserial').val("true");
                                $(".blockthree").css("display", "block");
                            } else {
                                ctxtserial.SetVisible(false);
                                $('#hdnisserial').val("false");
                                $(".blockthree").css("display", "none");
                            }
                            if (iswarehousactive == "false" && isactivebatch == "true") {
                                ctxtbatchqnty.SetVisible(true);
                                $(".blocktwoqntity").css("display", "block");
                            } else {
                                ctxtbatchqnty.SetVisible(false);
                                $(".blocktwoqntity").css("display", "none");
                            }
                            if (iswarehousactive == "false" && isactivebatch == "true") {
                                ctxtbatch.Focus();
                            } else {
                                cCmbWarehouse.Focus();
                            }
                            cGrdWarehousePC.PerformCallback('checkdataexist~' + strProductID + '~' + stockids + '~' + branchid + '~' + strProductName);
                            cPopup_WarehousePC.Show();
                        }
                    }
                });
            }
        }
    }
}

function FinalRemarks() {

    ccallback_InlineRemarks.PerformCallback('RemarksFinal' + '~' + grid.GetEditor('SrlNo').GetValue() + '~' + $('#txtInlineRemarks').val());
    $("#txtInlineRemarks").val('');
}
function callback_InlineRemarks_EndCall(s, e) {

    if (ccallback_InlineRemarks.cpDisplayFocus == "DisplayRemarksFocus") {
        $("#txtInlineRemarks").focus();
    }
    else {
        cPopup_InlineRemarks.Hide();
        grid.batchEditApi.StartEdit(globalRowIndex, 6);
    }
}

function Save_ButtonClick() {
    flag = true;
    LoadingPanel.Show();
    //cbtn_SaveRecords.SetEnabled(false);
    $('#hfControlData').val($('#hfControlSaveData').val());
    var txtPurchaseNo = $("#txtVoucherNo").val();
    var ddl_Vendor = $("#ddl_Vendor").val();


    var ProjectCode = clookup_Project.GetText();
    if ($("#hdnProjectSelectInEntryModule").val() == "1" && $("#hdnProjectMandatory").val() == "1" && ProjectCode == "") {
        flag = false;
        jAlert("Please Select Project.");
        LoadingPanel.Hide();
        return false;
    }

    if (txtPurchaseNo == null || txtPurchaseNo == "") {
        flag = false;
        $("#MandatoryBillNo").show();
        LoadingPanel.Hide();
        return false;
    }
    var Podt = cPLQuoteDate.GetValue();
    if (Podt == null) {
        LoadingPanel.Hide();
        $("#MandatoryDate").show();
        flag = false;
        return false;
    }
    var invtype = $('#ddlInventory').val();
    if (invtype != 'N') {
        if (ctxt_partyInvNo.GetText() == '' || ctxt_partyInvNo.GetText() == null) {
            flag = false;
            $("#MandatorysPartyinvno").show();
            LoadingPanel.Hide();
            return false;
        }
    }



    // Invoice Date validation Start
    var sdate = cdt_partyInvDt.GetValue();
    var edate = cPLQuoteDate.GetValue();
    var startDate = new Date(sdate);
    var endDate = new Date(edate);
    if (invtype == 'N') {
        if (sdate == null || sdate == "") {
        }
        else {
            $('#MandatoryPartyDate').attr('style', 'display:none');

            if (startDate > endDate) {
                LoadingPanel.Hide();
                flag = false;
                $('#MandatoryEgSDate').attr('style', 'display:block');
            }
            else { $('#MandatoryEgSDate').attr('style', 'display:none'); }
        }
    }
    else if (invtype != 'N') {
        if (sdate == null || sdate == "") {
            flag = false;
            $('#MandatoryPartyDate').attr('style', 'display:block');
            LoadingPanel.Hide();
            return false;
        }
        else {
            $('#MandatoryPartyDate').attr('style', 'display:none');
            if (startDate > endDate) {
                LoadingPanel.Hide();
                flag = false;
                $('#MandatoryEgSDate').attr('style', 'display:block');
            }
            else { $('#MandatoryEgSDate').attr('style', 'display:none'); }
        }
    }
    //var customerval = gridLookup.GetValue();
    var customerval = GetObjectID('hdnCustomerId').value
    if (customerval == '' || customerval == null) {
        LoadingPanel.Hide();
        flag = false;
        $('#MandatorysVendor').attr('style', 'display:block');
        return false;
    }
    else {
        $('#MandatorysVendor').attr('style', 'display:none');
    }
    if (Podt == null) {
        $("#MandatoryDate").show();
        LoadingPanel.Hide();
        return false;
    }

    var TDsValue = cddl_TdsScheme.GetValue();
    if (TDsValue == "0" || TDsValue == "") {
        flag = false;
        $("#MandatoryTDS").attr('style', 'display:block');
        LoadingPanel.Hide();
        return false;

    }
    else {
        $('#MandatoryTDS').attr('style', 'display:none');
    }



    var frontRow = 0;
    var backRow = -1;
    var IsProduct = "";
    for (var i = 0; i <= grid.GetVisibleRowsOnPage() ; i++) {
        var frontProduct = (grid.batchEditApi.GetCellValue(backRow, 'ProductID') != null) ? (grid.batchEditApi.GetCellValue(backRow, 'ProductID')) : "";
        var backProduct = (grid.batchEditApi.GetCellValue(frontRow, 'ProductID') != null) ? (grid.batchEditApi.GetCellValue(frontRow, 'ProductID')) : "";

        if (frontProduct != "" || backProduct != "") {
            IsProduct = "Y";
            break;
        }
        backRow--;
        frontRow++;
    }
    if (flag != false) {
        if (IsProduct == "Y") {
            var inventoryItem = $('#ddlInventory').val();
            if (inventoryItem == 'N' && cddl_TdsScheme.GetValue() != '0') {
                if ($('#hdnADDEditMode').val() != 'Edit') {
                    var tdschk = $('#hdntdschecking').val()
                    if (tdschk != '') {
                        LoadingPanel.Hide();
                        jAlert('Please enter TDS detail for Sl no.' + tdschk)
                        return false;
                    }
                }
            }
            if (grid.GetVisibleRowsOnPage() > 0) {
                //var customerval = gridLookup.GetValue();
                var customerval = GetObjectID('hdnCustomerId').value;
                $('#hdfLookupCustomer').val(customerval);
                $('#hdfIsDelete').val('I');
                $('#hdnRefreshType').val('N');
                grid.batchEditApi.EndEdit();
                cacbpCrpUdf.PerformCallback();
            }
            else {
                LoadingPanel.Hide();
                jAlert('Please add atleast single record first');
            }
        }
        else {
            LoadingPanel.Hide();
            jAlert('Cannot Save. You must enter atleast one Product to save this entry.');
        }
    }
}
function SaveExit_ButtonClick() {
    flag = true;
    LoadingPanel.Show();
    //cbtn_SaveRecords.SetEnabled(false);
    $('#hfControlData').val($('#hfControlSaveData').val());
    var txtPurchaseNo = $("#txtVoucherNo").val();
    var ddl_Vendor = $("#ddl_Vendor").val();

    var ProjectCode = clookup_Project.GetText();
    if ($("#hdnProjectSelectInEntryModule").val() == "1" && $("#hdnProjectMandatory").val() == "1" && ProjectCode == "") {
        LoadingPanel.Hide();
        flag = false;
        jAlert("Please Select Project.");
        return false;
    }


    if (txtPurchaseNo == null || txtPurchaseNo == "") {
        LoadingPanel.Hide();
        flag = false;
        $("#MandatoryBillNo").show();
        return false;
    }
    var Podt = cPLQuoteDate.GetValue();
    if (Podt == null) {
        LoadingPanel.Hide();
        $("#MandatoryDate").show();
        flag = false;
        return false;
    }
    var invtype = $('#ddlInventory').val();
    if (invtype != 'N') {
        if (ctxt_partyInvNo.GetText() == '' || ctxt_partyInvNo.GetText() == null) {
            flag = false;
            $("#MandatorysPartyinvno").show();
            LoadingPanel.Hide();
            return false;
        }
    }
    var sdate = cdt_partyInvDt.GetValue();
    var edate = cPLQuoteDate.GetValue();
    var startDate = new Date(sdate);
    var endDate = new Date(edate);
    if (invtype == 'N') {
        if (sdate == null || sdate == "") {
        }
        else {
            $('#MandatoryPartyDate').attr('style', 'display:none');

            if (startDate > endDate) {
                LoadingPanel.Hide();
                flag = false;
                $('#MandatoryEgSDate').attr('style', 'display:block');
            }
            else {
                $('#MandatoryEgSDate').attr('style', 'display:none');
                flag = true;
            }
        }
    }
    else if (invtype != 'N') {
        if (sdate == null || sdate == "") {
            flag = false;
            $('#MandatoryPartyDate').attr('style', 'display:block');
            LoadingPanel.Hide();
            return false;
        }
        else {
            $('#MandatoryPartyDate').attr('style', 'display:none');

            if (startDate > endDate) {
                LoadingPanel.Hide();
                return false;
                flag = false;
                $('#MandatoryEgSDate').attr('style', 'display:block');
            }
            else {
                $('#MandatoryEgSDate').attr('style', 'display:none');
                flag = true;
            }
        }
    }
    //var customerval = gridLookup.GetValue();
    var customerval = GetObjectID('hdnCustomerId').value;
    if (customerval == '' || customerval == null) {
        LoadingPanel.Hide();
        flag = false;
        $('#MandatorysVendor').attr('style', 'display:block');
        return false;
    }
    else {
        $('#MandatorysVendor').attr('style', 'display:none');
    }
    var TDsValue = cddl_TdsScheme.GetValue();
    
    if (TDsValue == "0" || TDsValue == "") {
        flag = false;
        $("#MandatoryTDS").attr('style', 'display:block');
        LoadingPanel.Hide();
        return false;

    }
    else {
        $('#MandatoryTDS').attr('style', 'display:none');
    }


    var frontRow = 0;
    var backRow = -1;
    var IsProduct = "";
    for (var i = 0; i <= grid.GetVisibleRowsOnPage() ; i++) {
        var frontProduct = (grid.batchEditApi.GetCellValue(backRow, 'ProductID') != null) ? (grid.batchEditApi.GetCellValue(backRow, 'ProductID')) : "";
        var backProduct = (grid.batchEditApi.GetCellValue(frontRow, 'ProductID') != null) ? (grid.batchEditApi.GetCellValue(frontRow, 'ProductID')) : "";

        if (frontProduct != "" || backProduct != "") {
            IsProduct = "Y";
            break;
        }
        backRow--;
        frontRow++;
    }

    // alert(flag);

    if (flag != false) {
        if (IsProduct == "Y") {
            var inventoryItem = $('#ddlInventory').val();
            if (inventoryItem == 'N' && cddl_TdsScheme.GetValue() != '0') {
                if ($('#hdnADDEditMode').val() != 'Edit') {
                    var tdschk = $('#hdntdschecking').val()
                    if (tdschk != '') {
                        LoadingPanel.Hide();
                        jAlert('Please enter TDS detail for Sl no.' + tdschk)
                        return false;
                    }
                }
            }
            if (grid.GetVisibleRowsOnPage() > 0) {
                //var customerval = gridLookup.GetValue();
                var customerval = GetObjectID('hdnCustomerId').value;
                $('#hdfLookupCustomer').val(customerval);
                $('#hdnRefreshType').val('E');
                $('#hdfIsDelete').val('I');
                grid.batchEditApi.EndEdit();
                cacbpCrpUdf.PerformCallback();
            }
            else {
                LoadingPanel.Hide();
                jAlert('Please add atleast single record first');
            }
        }
        else {
            LoadingPanel.Hide();
            jAlert('Cannot Save. You must enter atleast one Product to save this entry.');
        }
    }
}

function OnAddNewClick() {
    var inventoryItem = $('#ddlInventory').val();
    if (inventoryItem != 'N') {
        if (gridquotationLookup.GetValue() == null) {
            grid.AddNewRow();

            var noofvisiblerows = grid.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
            var tbQuotation = grid.GetEditor("SrlNo");
            tbQuotation.SetValue(noofvisiblerows);
        }
    }
    else {
        grid.AddNewRow();
        var noofvisiblerows = grid.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
        var tbQuotation = grid.GetEditor("SrlNo");
        tbQuotation.SetValue(noofvisiblerows);
    }
}
function ProductsCombo_SelectedIndexChanged(s, e) {

    var tbDescription = grid.GetEditor("Description");
    var tbUOM = grid.GetEditor("UOM");
    var tbStockUOM = grid.GetEditor("gvColStockUOM");
    var tbPurchasePrice = grid.GetEditor("PurchasePrice");
    var ProductID = (grid.GetEditor('ProductID').GetValue() != null) ? grid.GetEditor('ProductID').GetValue() : "0";
    var SpliteDetails = ProductID.split("||@||");
    var strProductID = SpliteDetails[0];
    var strDescription = SpliteDetails[1];
    var strUOM = SpliteDetails[2];
    var strStockUOM = SpliteDetails[4];
    var strPurchasePrice = SpliteDetails[6];
    var strStockId = SpliteDetails[10];
    tbDescription.SetValue(strDescription);
    tbUOM.SetValue(strUOM);
    tbStockUOM.SetValue(strStockUOM);
    tbPurchasePrice.SetValue(strPurchasePrice);
    if (ProductID != "0") {
        cacpAvailableStock.PerformCallback(strProductID);
    }
}

function ddl_AmountAre_valueChange() {
    var key = $("#ddl_AmountAre").val();
    if (key == 1) {
        cddlVatGstCst.SetEnabled(false);
        cddlVatGstCst.PerformCallback('1');
    }
    else if (key == 2) {
        cddlVatGstCst.SetEnabled(true);
        cddlVatGstCst.PerformCallback('2');
    }
    else if (key == 3) {
        cddlVatGstCst.SetEnabled(false);
        cddlVatGstCst.PerformCallback('3');
    }
}
function CmbBranch_ValueChange() {
    var ddlbranch = $("[id*=ddl_Branch]");
    var strBranch = ddlbranch.find("option:selected").val();
    var type = ($("[id$='rdl_PurchaseInvoice']").find(":checked").val() != null) ? $("[id$='rdl_PurchaseInvoice']").find(":checked").val() : "";
    if (type == 'PO' || type == 'PC') {
        selectValue();
    }
    else {
        ctxtVendorName.SetText('');
        GetObjectID('hdnCustomerId').value = '';
    }
}
function CloseGridLookup() {
    gridLookup.ConfirmCurrentSelection();
    gridLookup.HideDropDown();
    gridLookup.Focus();
}
function SetDifference() {
    var diff = CheckDifference();
    if (diff > 0) {
        clientResult.SetText(diff.toString());
    }
}
function CheckDifference() {
    var startDate = new Date();
    var endDate = new Date();
    var difference = -1;
    startDate = cPLQuoteDate.GetDate();
    if (startDate != null) {
        endDate = cExpiryDate.GetDate();
        var startTime = startDate.getTime();
        var endTime = endDate.getTime();
        difference = (endTime - startTime) / 86400000;

    }
    return difference;

}

//<%--   Warehouse  Script   --%>

    function Keypressevt() {
        if (event.keyCode == 13) {
            //run code for Ctrl+X -- ie, Save & Exit! 
            SaveWarehouse();
            return false;
        }
    }
function getUrlVars() {
    var vars = [], hash;
    var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
    for (var i = 0; i < hashes.length; i++) {
        hash = hashes[i].split('=');
        vars.push(hash[0]);
        vars[hash[0]] = hash[1];
    }
    return vars;
}
$(document).ready(function () {

    //var key = GetObjectID('hdnCustomerId').value;
    //if (key != null) {
    //    if ($("#btn_TermsCondition").is(":visible")) {
    //        callTCspecefiFields_PO(key);
    //    }
    //}

    var val = $("#ddl_numberingScheme").val();
    //var schemabranch = $('#ddl_numberingScheme').val().split('~')[1];
    if (val == '0') {
        document.getElementById('txtVoucherNo').disabled = true;
    }






    if (GetObjectID('hdnCustomerId').value == null || GetObjectID('hdnCustomerId').value == '') {
        page.GetTabByName('Billing/Shipping').SetEnabled(false);
    }
    var isCtrl = false;
    document.onkeydown = function (e) {
        //if (event.keyCode == 78 && event.altKey == true && getUrlVars().req != "V") { //run code for Alt + n -- ie, Save & New  
        if (event.keyCode == 83 && event.altKey == true && getUrlVars().req != "V") { //run code for Alt + s -- ie, Save & New  
            StopDefaultAction(e);
            Save_ButtonClick();
        }
            //else if (event.keyCode == 88 && event.altKey == true && getUrlVars().req != "V") { //run code for Alt+X -- ie, Save & Exit!  
        else if (event.keyCode == 69 && event.altKey == true && getUrlVars().req != "V") { //run code for Alt+e -- ie, Save & Exit!     
            StopDefaultAction(e);
            SaveExit_ButtonClick();
        }
        else if (event.keyCode == 79 && event.altKey == true) { //run code for Alt+O -- ie, Billing/Shipping Samrat!     
            StopDefaultAction(e);
            if (page.GetActiveTabIndex() == 1) {
                fnSaveBillingShipping();
            }
        }
        else if (event.keyCode == 77 && event.altKey == true) { //run code for Alt+m -- ie, TC Sayan!
            $('#TermsConditionseModal').modal({
                show: 'true'
            });
        }
        else if (event.keyCode == 69 && event.altKey == true) { //run code for Alt+e -- ie, TC Sayan!
            if (($("#TermsConditionseModal").data('bs.modal') || {}).isShown) {
                StopDefaultAction(e);
                SaveTermsConditionData();
            }
        }
        else if (event.keyCode == 76 && event.altKey == true) { //run code for Alt+l -- ie, TC Sayan!
            StopDefaultAction(e);
            calcelbuttonclick();
        }
        else if (event.keyCode == 85 && event.altKey == true) {//run code for Alt+U -- ie,
            OpenUdf();
        }
        else if (event.keyCode == 84 && event.altKey == true) {//run code for Alt+T
            Save_TaxesClick();
        }
        else if (event.keyCode == 83 && event.altKey == true) { //run code for Alt+X -- ie, Save & Exit!     
            StopDefaultAction(e);
            if (($("#exampleModal").data('bs.modal') || {}).isShown) {
                SaveVehicleControlData();
            }
        }
        else if (event.keyCode == 67 && event.altKey == true) { //run code for Alt+X -- ie, Save & Exit!     
            StopDefaultAction(e);
            modalShowHide(0);
        }
        else if (event.keyCode == 82 && event.altKey == true) { //run code for Alt+X -- ie, Save & Exit!     
            StopDefaultAction(e);
            $('body').on('shown.bs.modal', '#exampleModal', function () {
                $('input:visible:enabled:first', this).focus();
            })
        }
    }
});
function StopDefaultAction(e) {
    if (e.preventDefault) { e.preventDefault() }
    else { e.stop() };

    e.returnValue = false;
    e.stopPropagation();
}
function DeleteWarehousebatchserial(SrlNo, BatchWarehouseID, viewQuantity, Quantity, WarehouseID, BatchNo) {
    var IsSerial = $('#hdnisserial').val();
    if (IsSerial == "true" && viewQuantity != "1.0000" && viewQuantity != "1.0" && viewQuantity != "") {
        jAlert("Cannot Proceed. You have to delete subsequent data first before delete this data.");
    } else {
        if (BatchWarehouseID == "" || BatchWarehouseID == "0") {
            $('#hdnisolddeleted').val("false");
            if (SrlNo != "") {
                cGrdWarehousePC.PerformCallback('Delete~' + SrlNo + '~' + viewQuantity + '~' + Quantity + '~' + WarehouseID + '~' + BatchNo);
            }
        } else {
            $('#hdnisolddeleted').val("true");
            if (SrlNo != "") {
                cGrdWarehousePC.PerformCallback('Delete~' + SrlNo + '~' + viewQuantity + '~' + Quantity + '~' + WarehouseID + '~' + BatchNo);
            }
        }
    }
}
function UpdateWarehousebatchserial(SrlNo, WarehouseID, BatchNo, SerialNo, isnew, viewQuantity, Quantity) {
    var Isbatch = $('#hdnisbatch').val();
    if (isnew == "old" || isnew == "Updated") {
        $('#hdnisoldupdate').val("true");
        $('#hdncurrentslno').val("");
        cCmbWarehouse.SetValue(WarehouseID);
        if (Quantity != null && Quantity != "" && Isbatch != "true") {
            ctxtqnty.SetText(Quantity);
        } else {
            ctxtqnty.SetText(viewQuantity);
        }
        var IsSerial = $('#hdnisserial').val();
        if (IsSerial == "true") {
            if (viewQuantity == "") {
                ctxtbatch.SetEnabled(false);
                cCmbWarehouse.SetEnabled(false);
                ctxtqnty.SetEnabled(false);
                ctxtserial.Focus();
            } else {
                ctxtbatch.SetEnabled(true);
                cCmbWarehouse.SetEnabled(true);
                ctxtqnty.SetEnabled(true);
                ctxtserial.Focus();
            }
        }
        else {
            ctxtbatch.SetEnabled(true);
            cCmbWarehouse.SetEnabled(true);
            ctxtqnty.SetEnabled(true);
            ctxtbatch.Focus();
        }
        ctxtbatchqnty.SetText(Quantity);
        ctxtbatch.SetText(BatchNo);
        ctxtserial.SetText(SerialNo);
        if (viewQuantity == "") {
            ctxtbatch.SetEnabled(false);
            cCmbWarehouse.SetEnabled(false);
            $('#hdnisviewqntityhas').val("true");
        } else {
            ctxtbatch.SetEnabled(true);
            cCmbWarehouse.SetEnabled(true);
            $('#hdnisviewqntityhas').val("false");
        }
        var hdniswarehouse = $('#hdniswarehouse').val();
        if (hdniswarehouse != "true" && Isbatch == "true") {
            ctxtbatchqnty.SetText(viewQuantity);
            ctxtbatchqnty.Focus();
        } else {
            ctxtqnty.Focus();
        }
        $('#hdncurrentslno').val(SrlNo);
    } else {
        $('#hdnisoldupdate').val("false");
        ctxtqnty.SetText("0.0");
        ctxtqnty.SetEnabled(true);
        ctxtbatchqnty.SetText("0.0");
        ctxtserial.SetText("");
        ctxtbatchqnty.SetText("");
        $('#hdncurrentslno').val("");
        $('#hdnisnewupdate').val("true");
        $('#hdncurrentslno').val("");
        cCmbWarehouse.SetValue(WarehouseID);
        if (Quantity != null && Quantity != "" && Isbatch != "true") {
            ctxtqnty.SetText(Quantity);
        } else {
            ctxtqnty.SetText(viewQuantity);
        }
        var IsSerial = $('#hdnisserial').val();
        if (IsSerial == "true") {
            if (viewQuantity == "") {
                ctxtbatch.SetEnabled(false);
                cCmbWarehouse.SetEnabled(false);
                ctxtqnty.SetEnabled(false);
                $('#hdnisviewqntityhas').val("true");
                ctxtserial.Focus();
            } else {
                ctxtbatch.SetEnabled(true);
                cCmbWarehouse.SetEnabled(true);
                ctxtqnty.SetEnabled(true);
                $('#hdnisviewqntityhas').val("false");
                ctxtserial.Focus();
            }
        } else {
            ctxtbatch.SetEnabled(true);
            cCmbWarehouse.SetEnabled(true);
            ctxtqnty.SetEnabled(true);
            ctxtbatch.Focus();
        }
        ctxtbatchqnty.SetText(Quantity);
        ctxtbatch.SetText(BatchNo);
        ctxtserial.SetText(SerialNo);
        if (viewQuantity == "") {
            ctxtbatch.SetEnabled(false);
            cCmbWarehouse.SetEnabled(false);
        } else {
            ctxtbatch.SetEnabled(true);
            cCmbWarehouse.SetEnabled(true);
        }
        var hdniswarehouse = $('#hdniswarehouse').val();
        if (hdniswarehouse != "true" && Isbatch == "true") {
            ctxtbatchqnty.SetText(viewQuantity);
        } else {
            ctxtqnty.Focus();
        }
        $('#hdncurrentslno').val(SrlNo);
    }
}
function changedqnty(s) {
}
function endcallcmware(s) {
    if (cCmbWarehouse.cpstock != null) {
        var ddd = cCmbWarehouse.cpstock + " " + $('#hdnstrUOM').val();
        document.getElementById('lblAvailableStk').innerHTML = ddd;
        cCmbWarehouse.cpstock = null;
    }
}
function changedqntybatch(s) {
    var qnty = s.GetText();
    var sum = $('#hdntotalqntyPC').val();
    sum = Number(Number(sum) + Number(qnty));
    $('#hdntotalqntyPC').val(sum);
}
function chnagedbtach(s) {
    $('#hdnoldbatchno').val(s.GetText());
    $('#hidencountforserial').val(1);
    var sum = $('#hdnbatchchanged').val();
    sum = Number(Number(sum) + Number(1));
    $('#hdnbatchchanged').val(sum);
    ctxtexpirdate.SetText("");
    ctxtmkgdate.SetText("");
}
function CmbWarehouse_ValueChange(s) {
    var ISupdate = $('#hdnisoldupdate').val();
    var isnewupdate = $('#hdnisnewupdate').val();
    $('#hdnoldwarehousname').val(s.GetText());
    if (ISupdate == "true" || isnewupdate == "true") {
    } else {
        ctxtserial.SetValue("");
        ctxtbatch.SetEnabled(true);
        ctxtexpirdate.SetEnabled(true);
        ctxtmkgdate.SetEnabled(true);
    }
}
function Clraear() {
    ctxtbatch.SetValue("");
    ASPx.CalClearClick('txtmkgdate_DDD_C');
    ASPx.CalClearClick('txtexpirdate_DDD_C');
    $('#hdnisoldupdate').val("false");
    ctxtserial.SetValue("");
    ctxtqnty.SetValue("0.0000");
    ctxtbatchqnty.SetValue("0.0000");
    $('#hdntotalqntyPC').val(0);
    $('#hidencountforserial').val(1);
    $('#hdnbatchchanged').val("0");
    var strProductID = $('#hdfProductIDPC').val();
    var stockids = $('#hdfstockidPC').val();
    var branchid = $('#hdbranchIDPC').val();
    var strProductName = $('#lblProductName').text();
    $('#hdnisnewupdate').val("false");
    ctxtbatch.SetEnabled(true);
    ctxtexpirdate.SetEnabled(true);
    ctxtmkgdate.SetEnabled(true);
    ctxtbatch.SetEnabled(true);
    cCmbWarehouse.SetEnabled(true);
    $('#hdnisviewqntityhas').val("false");
    $('#hdnisolddeleted').val("false");
    ctxtqnty.SetEnabled(true);
    var existingqntity = $('#hdfopeningstockPC').val();
    var totaldeleteqnt = $('#hdndeleteqnity').val();
    var addqntity = Number(existingqntity) + Number(totaldeleteqnt);
    $('#hdndeleteqnity').val(0);
    cGrdWarehousePC.PerformCallback('checkdataexist~' + strProductID + '~' + stockids + '~' + branchid + '~' + strProductName);
}
function SaveWarehouse() {
    if ($('#wbsqtychecking').val() == '1') {
        var qnty = ctxtqnty.GetText();
        var sum = $('#hdntotalqntyPC').val();
        sum = Number(Number(sum) + Number(qnty));
        $('#hdntotalqntyPC').val(sum);
        ctxtqnty.SetEnabled(false);
        $('#wbsqtychecking').val('0');
    }
    var prosrlno = $('#hdfProductSerialID').val();
    var WarehouseID = cCmbWarehouse.GetValue();
    var WarehouseName = cCmbWarehouse.GetText();
    var qnty = ctxtqnty.GetText();
    var IsSerial = $('#hdnisserial').val();
    if (qnty == "0.0000") {
        qnty = ctxtbatchqnty.GetText();
    }
    if (Number(qnty) % 1 != 0 && IsSerial == "true") {
        jAlert("Serial number is activated, Quantity should not contain decimals. ");
        return;
    }
    var BatchName = ctxtbatch.GetText();
    var SerialName = ctxtserial.GetText();
    var Isbatch = $('#hdnisbatch').val();
    var enterdqntity = $('#hdfopeningstockPC').val();
    var hdniswarehouse = $('#hdniswarehouse').val();
    var ISupdate = $('#hdnisoldupdate').val();
    var isnewupdate = $('#hdnisnewupdate').val();
    if (Isbatch == "true" && hdniswarehouse == "false") {
        qnty = ctxtbatchqnty.GetText();
    }
    if (ISupdate == "true") {
        if (hdniswarehouse == "true" && WarehouseID == null) {
            $("#RequiredFieldValidatorCmbWarehouse").css("display", "block");
        }
        else {
            $("#RequiredFieldValidatorCmbWarehouse").css("display", "none");
        }
        if (qnty == "0.0") {
            if (Isbatch != "false" || hdniswarehouse != "false") {
                $("#RequiredFieldValidatortxtbatchqntity").css("display", "block");
                $("#RequiredFieldValidatortxtwareqntity").css("display", "block");
            } else if (Isbatch == "false" && hdniswarehouse == "false" && IsSerial == "true") {
                qnty = "0.00"
                $("#RequiredFieldValidatortxtbatchqntity").css("display", "none");
                $("#RequiredFieldValidatortxtwareqntity").css("display", "none");
            }
        } else {
            qnty = "0.00"
            $("#RequiredFieldValidatortxtbatchqntity").css("display", "none");
            $("#RequiredFieldValidatortxtwareqntity").css("display", "none");
        }
        if (Isbatch == "true" && BatchName == "") {
            $("#RequiredFieldValidatortxtbatch").css("display", "block");
            ctxtbatch.Focus();
        } else {
            $("#RequiredFieldValidatortxtbatch").css("display", "none");
        }
        if (IsSerial == "true" && SerialName == "") {
            $("#RequiredFieldValidatortxtserial").css("display", "block");
            ctxtserial.Focus();
        } else {
            $("#RequiredFieldValidatortxtserial").css("display", "none");
        }
        var slno = $('#hdncurrentslno').val();
        if (slno != "") {
            cGrdWarehousePC.PerformCallback('Updatedata~' + WarehouseID + '~' + WarehouseName + '~' + BatchName + '~' + SerialName + '~' + slno + '~' + qnty + '~' + prosrlno);
            return false;
        }
    } else if (isnewupdate == "true") {
        if (hdniswarehouse == "true" && WarehouseID == null) {
            $("#RequiredFieldValidatorCmbWarehouse").css("display", "block");
        }
        else {
            $("#RequiredFieldValidatorCmbWarehouse").css("display", "none");
        }
        if (qnty == "0.0") {
            if (Isbatch != "false" || hdniswarehouse != "false") {
                $("#RequiredFieldValidatortxtbatchqntity").css("display", "block");
                $("#RequiredFieldValidatortxtwareqntity").css("display", "block");
            } else if (Isbatch == "false" && hdniswarehouse == "false" && IsSerial == "true") {
                qnty = "0.00"
                $("#RequiredFieldValidatortxtbatchqntity").css("display", "none");
                $("#RequiredFieldValidatortxtwareqntity").css("display", "none");
            }
        } else {
            qnty = "0.00"
            $("#RequiredFieldValidatortxtbatchqntity").css("display", "none");
            $("#RequiredFieldValidatortxtwareqntity").css("display", "none");
        }
        if (Isbatch == "true" && BatchName == "") {
            $("#RequiredFieldValidatortxtbatch").css("display", "block");
            ctxtbatch.Focus();
        }
        else {
            $("#RequiredFieldValidatortxtbatch").css("display", "none");
        }
        if (IsSerial == "true" && SerialName == "") {
            $("#RequiredFieldValidatortxtserial").css("display", "block");
            ctxtserial.Focus();
        } else {
            $("#RequiredFieldValidatortxtserial").css("display", "none");
        }
        var slno = $('#hdncurrentslno').val();
        if (slno != "") {
            cGrdWarehousePC.PerformCallback('NewUpdatedata~' + WarehouseID + '~' + WarehouseName + '~' + BatchName + '~' + SerialName + '~' + slno + '~' + qnty + '~' + prosrlno);
            $('#hdnisviewqntityhas').val("false");
            $('#hdnisnewupdate').val("false");
            ctxtbatch.SetEnabled(true);
            cCmbWarehouse.SetEnabled(true);
            ctxtqnty.SetEnabled(true);
            ctxtqnty.SetText("0.0");
            ctxtbatch.SetText("");
            return false;
        }
    }
    else {
        var hdnisediteds = $('#hdnisedited').val();
        if (hdniswarehouse == "true" && WarehouseID == null) {
            $("#RequiredFieldValidatorCmbWarehouse").css("display", "block");

            return;
        } else {
            $("#RequiredFieldValidatorCmbWarehouse").css("display", "none");
        }
        if (qnty == "0.0") {

            if (Isbatch != "false" || hdniswarehouse != "false") {
                $("#RequiredFieldValidatortxtbatchqntity").css("display", "block");
                $("#RequiredFieldValidatortxtwareqntity").css("display", "block");
            } else if (Isbatch == "false" && hdniswarehouse == "false" && IsSerial == "true") {
                qnty = "0.00"
                $("#RequiredFieldValidatortxtbatchqntity").css("display", "none");
                $("#RequiredFieldValidatortxtwareqntity").css("display", "none");
            }
        } else {
            qnty = "0.00"
            $("#RequiredFieldValidatortxtbatchqntity").css("display", "none");
            $("#RequiredFieldValidatortxtwareqntity").css("display", "none");
        }
        if (Isbatch == "true" && BatchName == "") {

            $("#RequiredFieldValidatortxtbatch").css("display", "block");
            ctxtbatch.Focus();
            return;

        } else {
            $("#RequiredFieldValidatortxtbatch").css("display", "none");
        }
        if (IsSerial == "true" && SerialName == "") {
            $("#RequiredFieldValidatortxtserial").css("display", "block");
            ctxtserial.Focus();
            return;
        } else {
            $("#RequiredFieldValidatortxtserial").css("display", "none");
        }
        if (Isbatch == "true" && hdniswarehouse == "false") {
            qnty = ctxtbatchqnty.GetText();
            if (qnty == "0.0000") {
                ctxtbatchqnty.Focus();
            }
        }
        if (qnty == "0.0") {
            if (Isbatch != "false" || hdniswarehouse != "false") {
                $("#RequiredFieldValidatortxtbatchqntity").css("display", "block");
                $("#RequiredFieldValidatortxtwareqntity").css("display", "block");
            } else if (Isbatch == "false" && hdniswarehouse == "false" && IsSerial == "true") {
                qnty = "0.00"
                $("#RequiredFieldValidatortxtbatchqntity").css("display", "none");
                $("#RequiredFieldValidatortxtwareqntity").css("display", "none");
            }
        }
        else if (((hdniswarehouse == "true" && WarehouseID != null) || hdniswarehouse == "false") && ((Isbatch == "true" && BatchName != "") || Isbatch == "false") && ((IsSerial == "true" && SerialName != "") || IsSerial == "false") && qnty != "0.0") {
            $("#RequiredFieldValidatorCmbWarehouse").css("display", "none");
            $("#RequiredFieldValidatortxtbatch").css("display", "none");
            $("#RequiredFieldValidatortxtserial").css("display", "none");
            $("#RequiredFieldValidatortxtwareqntity").removeAttr("style");
            $("#RequiredFieldValidatortxtbatchqntity").removeAttr("style");
            $("#RequiredFieldValidatortxtbatchqntity").css("display", "none");
            $("#RequiredFieldValidatortxtwareqntity").css("display", "none");
            if (Isbatch == "true" && hdniswarehouse == "false") {
                qnty = ctxtbatchqnty.GetText();
                if (qnty = "0.0000") {
                    ctxtbatchqnty.Focus();
                }
            }
            var oldenterqntity = $('#hdnenterdopenqnty').val();
            var enterdqntityss = $('#hdnnewenterqntity').val();
            var deletedquantity = $('#hdndeleteqnity').val();
            if (Number(qnty) > (Number(enterdqntity) + Number(deletedquantity)) && hdnisediteds == "false") {
                qnty = "0.00";
                jAlert("You have entered Quantity greater than Opening Quantity. Cannot Proceed.");
            }
            else {
                cGrdWarehousePC.PerformCallback('Display~' + WarehouseID + '~' + WarehouseName + '~' + BatchName + '~' + SerialName + '~' + qnty + '~' + prosrlno);
                cCmbWarehouse.Focus();
            }
        }
        return false;
    }
}
function SaveWarehouseAll() {
    cGrdWarehousePC.PerformCallback('Saveall~');
}
function cGrdWarehousePCShowError(obj) {
    if (cGrdWarehousePC.cperrorMsg != null && cGrdWarehousePC.cperrorMsg != undefined) {
        jAlert(cGrdWarehousePC.cperrorMsg);
        ctxtserial.Focus();
        return;
    }
    if (cGrdWarehousePC.cpdeletedata != null) {
        var existingqntity = $('#hdfopeningstockPC').val();
        var totaldeleteqnt = $('#hdndeleteqnity').val();
        var addqntity = Number(cGrdWarehousePC.cpdeletedata) + Number(existingqntity);
        var adddeleteqnty = Number(cGrdWarehousePC.cpdeletedata) + Number(totaldeleteqnt);
        $('#hdndeleteqnity').val(adddeleteqnty);
        cGrdWarehousePC.cpdeletedata = null;
    }
    if (cGrdWarehousePC.cpdeletedatasubsequent != null) {
        jAlert(cGrdWarehousePC.cpdeletedatasubsequent);
        cGrdWarehousePC.cpdeletedatasubsequent = null;
    }
    if (cGrdWarehousePC.cpbatchinsertmssg != null) {
        ctxtbatch.SetText("");

        ctxtqnty.SetValue("0.0000");
        ctxtbatchqnty.SetValue("0.0000");
        cGrdWarehousePC.cpbatchinsertmssg = null;
    }
    if (cGrdWarehousePC.cpupdateexistingdata != null) {

        $('#hdnisedited').val("true");
        cGrdWarehousePC.cpupdateexistingdata = null;
    }
    if (cGrdWarehousePC.cpupdatenewdata != null) {

        $('#hdnisedited').val("true");

        cGrdWarehousePC.cpupdateexistingdata = null;
    }
    if (cGrdWarehousePC.cpupdatemssgserialsetdisblebatch != null) {
        ctxtbatch.SetEnabled(false);
        ctxtexpirdate.SetEnabled(false);
        ctxtmkgdate.SetEnabled(false);
        cGrdWarehousePC.cpupdatemssgserialsetdisblebatch = null;
    }
    if (cGrdWarehousePC.cpupdatemssgserialsetenablebatch != null) {
        ctxtbatch.SetEnabled(true);
        ctxtexpirdate.SetEnabled(true);
        ctxtmkgdate.SetEnabled(true);
        $('#hidencountforserial').val(1);
        //New Code Added by Sam
        ctxtqnty.SetEnabled(true)
        $('#wbsqtychecking').val('1')
        //New Code Added by Sam
        $('#hdnbatchchanged').val("0");
        $('#hidencountforserial').val("1");
        ctxtqnty.SetValue("0.0000");
        ctxtbatchqnty.SetValue("0.0000");
        ctxtbatch.SetText("");
        cGrdWarehousePC.cpupdatemssgserialsetenablebatch = null;
    }
    if (cGrdWarehousePC.cpproductname != null) {
        document.getElementById('lblpro').innerHTML = cGrdWarehousePC.cpproductname;
        cGrdWarehousePC.cpproductname = null;
    }
    if (cGrdWarehousePC.cpupdatemssg != null) {
        if (cGrdWarehousePC.cpupdatemssg == "Saved Successfully.") {
            $('#hdntotalqntyPC').val("0");
            $('#hdnbatchchanged').val("0");
            $('#hidencountforserial').val("1");
            ctxtqnty.SetValue("0.0000");
            ctxtbatchqnty.SetValue("0.0000");
            parent.cPopup_WarehousePC.Hide();
            var hdnselectedbranch = $('#hdnselectedbranch').val();
            var selectedrow = $('#hdngridvselectedrowno').val();
            grid.batchEditApi.StartEdit(selectedrow, 8);
        } else {
            jAlert(cGrdWarehousePC.cpupdatemssg);
        }
        cGrdWarehousePC.cpupdatemssg = null;
    }
    if (cGrdWarehousePC.cpupdatemssgserial != null) {
        jAlert(cGrdWarehousePC.cpupdatemssgserial);
        cGrdWarehousePC.cpupdatemssgserial = null;
    }
    if (cGrdWarehousePC.cpinsertmssg != null) {
        $('#hidencountforserial').val(2);
        ctxtserial.SetValue("");
        ctxtserial.Focus();
        cGrdWarehousePC.cpinsertmssg = null;
    }
    if (cGrdWarehousePC.cpinsertmssgserial != null) {
        ctxtserial.SetValue("");
        ctxtserial.Focus();
        cGrdWarehousePC.cpinsertmssgserial = null;
    }
}
function Onddl_VatGstCstEndCallback(s, e) {
    if (s.GetItemCount() == 1) {
        cddlVatGstCst.SetEnabled(false);
    }
}

//<%--   Warehouse Script End    --%>

//<%--Sam Section For extra Modification and tagging Section Start--%>

    $(document).ready(function () {
        $('#ApprovalCross').click(function () {

            window.parent.popup.Hide();
            window.parent.cgridPendingApproval.Refresh()();
        })
    })

function selectValue() {
    var startDate = new Date();
    startDate = cPLQuoteDate.GetValueString();
    //var key = gridLookup.GetValue()
    var key = GetObjectID('hdnCustomerId').value;
    if (key != null && key != '') {
        $('#hdnTaggedVender').val(key);
        
        $('#hdnTaggedVendorName').val(ctxtVendorName.GetText());
    }
    else {
        $("table[id$=rdl_PurchaseInvoice] input:radio:checked").removeAttr("checked");
        jAlert('Select a Transporter first');
        return;
    }
    var invtype = $('#ddlInventory').val();
    var type = ($("[id$='rdl_PurchaseInvoice']").find(":checked").val() != null) ? $("[id$='rdl_PurchaseInvoice']").find(":checked").val() : "";
    var schemaid = $('#ddl_numberingScheme').val();
    if (schemaid != '0') {
        page.tabs[1].SetEnabled(false);
        if (type == "PO") {
            clbl_InvoiceNO.SetText('Purchase Order Date');
            $("#ddlInventory").attr("disabled", true);
            if (invtype == 'Y') {
                grid.GetEditor('ProductName').SetEnabled(false);
            }
        }
        else if (type == "PC") {

            clbl_InvoiceNO.SetText('GRN Date');
            $("#ddlInventory").attr("disabled", true);
            $("#ddl_numberingScheme").attr("disabled", true);
            if (invtype == 'Y' || invtype == 'B' || invtype == 'C') {
                grid.GetEditor('ProductName').SetEnabled(false);
            }
        }
        else {
            $("#ddlInventory").attr("disabled", false);
            $("#ddl_numberingScheme").attr("disabled", false);
            grid.GetEditor('ProductName').SetEnabled(true);
        }
        if (key != null && key != '' && type != "") {
            cQuotationComponentPanel.PerformCallback('DateCheckOnChanged' + '~' + key + '~' + startDate + '~' + 'DateCheck' + '~' + type + '~' + invtype);
        }
        var componentType = gridquotationLookup.GetGridView().GetRowKey(gridquotationLookup.GetGridView().GetFocusedRowIndex());
        if (componentType != null && componentType != '') {
            var noofvisiblerows = grid.GetVisibleRowsOnPage();
            if (noofvisiblerows != '1') {

                grid.PerformCallback('GridBlank');
            }
        }
    }
}

//<%--Sam Section For extra Modification Section End--%>

//<%--Added By : Samrat Roy -- New Billing/Shipping Section--%>

    function SettingTabStatus() {
        if (GetObjectID('hdnCustomerId').value != null && GetObjectID('hdnCustomerId').value != '' && GetObjectID('hdnCustomerId').value != '0') {
            page.GetTabByName('[B]illing/Shipping').SetEnabled(true);
        }
    }

function disp_prompt(name) {
    if (name == "tab0") {
        ctxt_partyInvNo.Focus();
        $("#crossdiv").show();
    }
    if (name == "tab1") {
        var custID = GetObjectID('hdnCustomerId').value;
        if (custID == null && custID == '') {
            jAlert('Please select a customer');
            page.SetActiveTabIndex(0);

            return;
        }
        else {
            page.SetActiveTabIndex(1);
            page.tabs[0].SetEnabled(false);
        }
    }
}
$(document).ready(function () {
    //Toggle fullscreen expandEntryGrid
    $("#expandgrid").click(function (e) {
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
            grid.SetHeight(browserHeight - 150);
            grid.SetWidth(cntWidth);
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
            grid.SetHeight(200);
            var cntWidth = $this.parent('.makeFullscreen').width();
            grid.SetWidth(cntWidth);
        }
    });
});
