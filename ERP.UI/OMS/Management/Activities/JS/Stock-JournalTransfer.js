var options;
var globalRowIndex;

function clookup_Project_LostFocus() {
    //grid.batchEditApi.StartEdit(-1, 2);

    var projID = clookup_Project.GetValue();
    if (projID == null || projID == "") {
        $("#ddlHierarchy").val(0);
    }
}

function ProjectValueChange(s, e) {

    var projID = clookup_Project.GetValue();

    $.ajax({
        type: "POST",
        url: 'Stock-JournalTransfer.aspx/getHierarchyID',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ ProjID: projID }),
        success: function (msg) {
            var data = msg.d;
            $("#ddlHierarchy").val(data);
        }
    });
}

$(function () {

    $("#hdnrmvetobranch").val(0);
    $("#hdnrmvetobranchtext").val(0);
    $("#hdntotWHquantity").val(0);
    $("#hddnwarehouseqty").val(0);
    $("#hdnUOM").val('');
    $("#hddnwarehousetyoe").val('');
    $("#ddl_numberingScheme").focus();

})

$(document).ready(function () {
    //debugger;
    document.onkeydown = function (e) {

        if (event.altKey == true) {
            switch (event.keyCode) {


                case 115:
                    StopDefaultAction(e);
                    if (getUrlVars().req != "V") {

                        SaveNew_Click();
                    }

                    break;

                case 83:
                    StopDefaultAction(e);
                    if (getUrlVars().req != "V") {
                        SaveNew_Click();
                    }
                    break;
                case 88:
                    //debugger;
                    StopDefaultAction(e);
                    if (getUrlVars().req != "V") {
                        SaveExit_Click();
                    }
                    break;
                case 120:
                    StopDefaultAction(e);
                    if (getUrlVars().req != "V") {
                        SaveExit_Click();
                    }
                    break;

                case 100:
                    StopDefaultAction(e);
                    if (getUrlVars().req != "V") {
                        Openwarehousepopup();
                    }
                    break;
                case 68:
                    StopDefaultAction(e);
                    if (getUrlVars().req != "V") {
                        Openwarehousepopup();
                    }
                    break;


                case 118:
                    StopDefaultAction(e);
                    if (getUrlVars().req != "V") {
                        Openwarehousepopup_view();
                    }
                    break;
                case 86:
                    StopDefaultAction(e);
                    if (getUrlVars().req != "V") {
                        Openwarehousepopup_view();
                    }
                    break;


            }
        }
    }
});
function StopDefaultAction(e) {
    if (e.preventDefault) { e.preventDefault() }
    else { e.stop() };

    e.returnValue = false;
    e.stopPropagation();
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

function CmbScheme_ValueChange() {

    var val = $("#ddl_numberingScheme").val();
    var schemetypeValue = val;
    var schemetype = schemetypeValue.toString().split('~')[1];
    var schemelength = schemetypeValue.toString().split('~')[2];

    var branchID = (schemetypeValue.toString().split('~')[3] != null) ? schemetypeValue.toString().split('~')[3] : "";
    if (branchID != "") document.getElementById('ddl_Branch').value = branchID;

    $('#hdnBranchID').val(branchID);
    $('#txtVoucherNo').attr('maxLength', schemelength);
    /// alert($("#hdnrmvetobranch").val() + ' ' + $("#hdnrmvetobranchtext").val());


    //if ($("#hdnrmvetobranch").val() != '0') {
    //    $('#ddl_to_branch').append('<option value=' + $("#hdnrmvetobranch").val() + '>' + $("#hdnrmvetobranchtext").val() + '</option>');
    //}


    //$('#ddl_to_branch option').each(function () {
    //    if ($(this).val() == branchID) {
    //        //  alert($(this).val() + ' ' + $(this).text());
    //        $("#hdnrmvetobranch").val($(this).val());
    //        $("#hdnrmvetobranchtext").val($(this).text());
    //        $(this).remove();
    //    }
    //});


    ///  page.tabs[1].SetEnabled(false);
    var schemetypeValue = val;
    var schemetype = schemetypeValue.toString().split('~')[1];
    var schemelength = schemetypeValue.toString().split('~')[2];
    $('#txtVoucherNo').attr('maxLength', schemelength);
    if (schemetype == '0') {
        document.getElementById('txtVoucherNo').disabled = false;
        document.getElementById('txtVoucherNo').value = "";
        $("#txtVoucherNo").focus();
    }
    else if (schemetype == '1') {
        document.getElementById('txtVoucherNo').disabled = true;
        document.getElementById('txtVoucherNo').value = "Auto";
        $("#MandatoryBillNo").hide();
    }
    else if (schemetype == '2') {
        document.getElementById('txtVoucherNo').disabled = true;
        document.getElementById('txtVoucherNo').value = "Datewise";
    }
    else if (schemetype == 'n') {
        document.getElementById('txtVoucherNo').disabled = true;
        document.getElementById('txtVoucherNo').value = "";
    }
    else {
        document.getElementById('txtVoucherNo').disabled = true;
        document.getElementById('txtVoucherNo').value = "";
    }
    if ($("#hdnProjectSelectInEntryModule").val() == "1") {
        clookup_Project.gridView.Refresh();
    }
}

function ProductButnClick(s, e) {
    if (e.buttonIndex == 0) {

        Pre_Quantity = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
        Pre_Amt = (grid.GetEditor('TotalAmount').GetValue() != null) ? grid.GetEditor('TotalAmount').GetValue() : "0";
        Pre_TotalAmt = (grid.GetEditor('NetAmount').GetValue() != null) ? grid.GetEditor('NetAmount').GetValue() : "0";

        setTimeout(function () { $("#txtProdSearch").focus(); }, 500);

        $('#txtProdSearch').val('');
        $('#ProductModel').modal('show');

    }
}

function ProductKeyDown(s, e) {
    if (e.htmlEvent.key == "Enter") {
        s.OnButtonClick(0);
    }
}

function prodkeydown(e) {
    //Both-->B;Inventory Item-->Y;Capital Goods-->C
    var inventoryType = (document.getElementById("ddlInventory").value != null) ? document.getElementById("ddlInventory").value : "";

    var OtherDetails = {}
    OtherDetails.SearchKey = $("#txtProdSearch").val();
    OtherDetails.InventoryType = inventoryType;

    var Type = grid.GetEditor('Type').GetValue();

    if (Type == 'Stk-Out (Consumable)') {
        $("#hddnwarehousetyoe").val('');
    }


    if (e.code == "Enter" || e.code == "NumpadEnter") {
        var HeaderCaption = [];

        HeaderCaption.push("Product Code");
        HeaderCaption.push("Product Name");
        HeaderCaption.push("Inventory");
        HeaderCaption.push("HSN/SAC");
        HeaderCaption.push("Class");
        HeaderCaption.push("Brand");
        if ($("#txtProdSearch").val() != '') {
            callonServer("Services/Master.asmx/GetStockJournalProduct", OtherDetails, "ProductTable", HeaderCaption, "ProdIndex", "SetProduct");
        }
    }
    else if (e.code == "ArrowDown") {
        if ($("input[ProdIndex=0]"))
            $("input[ProdIndex=0]").focus();
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

function SetProduct(Id, Name) {


    $('#ProductModel').modal('hide');

    var LookUpData = Id;
    var ProductCode = Name;

    if (!ProductCode) {
        LookUpData = null;
    }

    var SpliteDetails = Id.split("||@||");

    var Product_ID = Id;
    var Product_Code = SpliteDetails[1];
    var Product_Name = SpliteDetails[2];
    var HSNCode = SpliteDetails[14];
    var Purchase_UOMID = SpliteDetails[6];
    var Purchase_UOM = SpliteDetails[7];
    var Purchase_Price = SpliteDetails[10];
    var IsPackingActive = SpliteDetails[18];
    var Packing_Factor = SpliteDetails[19];
    var Packing_UOM = SpliteDetails[20];
    var Warehousetype = SpliteDetails[21];
    var IsComponent = SpliteDetails[22];
    var ComponentProduct = SpliteDetails[23];
    var pclass = SpliteDetails[15];
    var pBrand = SpliteDetails[16];

    //ctxtCustName.SetEnabled(false);
    // cddl_AmountAre.SetEnabled(false);
    //document.getElementById("ddl_numberingScheme").disabled = true;
    //document.getElementById("ddlInventory").disabled = true;
    grid.batchEditApi.StartEdit(globalRowIndex);



    grid.batchEditApi.StartEdit(globalRowIndex, 5);


    grid.GetEditor("ProductID").SetText(Product_ID);
    grid.GetEditor("ProductName").SetText(Product_Code);
    grid.GetEditor("ProductDiscription").SetText(Product_Name);
    grid.GetEditor("Quantity").SetText("0");
    grid.GetEditor("PurchaseUOM").SetText(Purchase_UOM);
    grid.GetEditor("PurchasePrice").SetText(Purchase_Price);
    grid.GetEditor("TotalAmount").SetText("0");
    grid.GetEditor("NetAmount").SetText("0");

    grid.GetEditor("ProductClass").SetText(pclass);
    grid.GetEditor("Brand").SetText(pBrand);
    //grid.GetEditor("TotalQty").SetText("0");
    //grid.GetEditor("BalanceQty").SetText("0");
    //grid.GetEditor("IsComponentProduct").SetText("");
    //grid.GetEditor("DocID").SetText("");


    SetFocusAfterProductSelect();

}

function SetFocusAfterProductSelect() {
    setTimeout(function () {
        grid.batchEditApi.StartEdit(globalRowIndex, 3);
        return;
    }, 200);
}

function gridFocusedRowChanged(s, e) {
    globalRowIndex = e.visibleIndex;
}

function PurchasePriceTextFocus(s, e) {


    //var PurchasePrice = (grid.GetEditor('PurchasePrice').GetValue() != null) ? grid.GetEditor('PurchasePrice').GetValue() : "0";
    //var Pre_Quantity = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
    //_GetPurchasePriceValue = PurchasePrice;
    //grid.GetEditor("TotalAmount").SetText(parseFloat(PurchasePrice) * parseFloat(Pre_Quantity));
    //grid.GetEditor("NetAmount").SetText(parseFloat(PurchasePrice) * parseFloat(Pre_Quantity));


}


function AddBatchNew(s, e) {
    var Type = grid.GetEditor('Type').GetValue();
    var keyCode = ASPxClientUtils.GetKeyCode(e.htmlEvent);
    if (keyCode == 13 && Type == 'Stk-Out (Consumable)') {
        setTimeout(function () { grid.batchEditApi.StartEdit(1, 1); }, 500);
    }
}



function SaveNew_Click() {


    //alert($("#hddnwarehouseqty").val() + '- ' + Math.floor($("#hdnquantity").val()));

    $("#hdnquantity").val('');
    $("#hdnUOM").val('');
    $("#hddnwarehousetyoe").val('');
    $("#hddnproductcode").val('');

    if ($("#hdninventorytype").val() != '') {

        flag = true;
        //  LoadingPanel.Show();
        var txtPurchaseNo = $("#txtVoucherNo").val().trim();
        if (txtPurchaseNo == null || txtPurchaseNo == "") {
            //LoadingPanel.Hide();
            $("#MandatoryBillNo").show();
            flag = false;
            return false;
        }
        else {
            $('#MandatoryBillNo').attr('style', 'display:none');
        }

        if (cPLQuoteDate.GetDate() == null) {
            flag = false;
            return false;
        }

        var ProjectCode = clookup_Project.GetText();
        if ($("#hdnProjectSelectInEntryModule").val() == "1" && $("#hdnProjectMandatory").val() == "1" && ProjectCode == "") {
            jAlert("Please Select Project.");
            flag = false;
            return false;
        }


        var RowCount = 0;
        var IsProduct = "";
        for (var i = 0; i <= grid.GetVisibleRowsOnPage() ; i++) {
            var ProductID = (grid.batchEditApi.GetCellValue(RowCount, 'ProductName') != null) ? (grid.batchEditApi.GetCellValue(RowCount, 'ProductName')) : "";

            //  alert(ProductID);
            if (ProductID != "") {
                IsProduct = "Y";
                break;
            }

            RowCount++;

        }

        if (flag != false) {
            if (IsProduct == "Y") {


                flagqty = true;
                flagwarehouse = true;
                flagprod = true;
                flaguom = true;
                flagsameproduct = true;
                var transfrm = ''
                var transfrmtot = ''
                for (var i = 0; i < grid.GetVisibleRowsOnPage() ; i++) {


                    var valueid = grid.batchEditApi.GetCellValue(i, "ProductID").split('||@||')[0];

                    if (valueid == '') {

                        flagprod = false;
                    }

                    var valueqt = grid.batchEditApi.GetCellValue(i, "Quantity");

                    var vwarehousetype = grid.batchEditApi.GetCellValue(i, "ProductID").split('||@||')[21];

                    //  alert(valueuom);


                    if ($("#hdnquantity").val() != '') {

                        if (Math.floor(valueqt) != Math.floor($("#hdnquantity").val())) {
                            flagqty = false;
                        }

                        // alert(Math.floor($("#hddnwarehouseqty").val()) + '-' + Math.floor(valueqt) + '-' + Math.floor($("#hdnquantity").val()));
                        if (Math.floor($("#hddnwarehouseqty").val()) != Math.floor($("#hdnquantity").val())) {
                            flagqty = false;
                        }
                    }


                    if ($("#hddnwarehousetyoe").val() != '') {
                        //alert($("#hddnwarehousetyoe").val() + vwarehousetype);
                        //if (vwarehousetype != ($("#hddnwarehousetyoe").val())) {
                        //    flagwarehouse = false;
                        //}

                        // alert($("#hddnwarehousetyoe").val());
                        if ($("#hddnwarehousetyoe").val() == 'W') {

                            if (vwarehousetype != ($("#hddnwarehousetyoe").val())) {
                                flagwarehouse = false;
                            }
                        }
                    }


                    var valuepr = grid.batchEditApi.GetCellValue(i, "PurchasePrice");
                    var valueuom = grid.batchEditApi.GetCellValue(i, "PurchaseUOM");
                    var valuetype = grid.batchEditApi.GetCellValue(i, "Type");



                    if ($("#hdnUOM").val() != '') {
                        if (valueuom != $("#hdnUOM").val()) {
                            flaguom = false;
                        }
                    }

                    if ($("#hddnproductcode").val() != '') {
                        if (valueid == $("#hddnproductcode").val()) {
                            flagsameproduct = false;
                        }
                    }


                    //    alert($("#hdnUOM").val() + '' + valueuom);

                    if (i == 0) {

                        $("#hdnquantity").val(valueqt);
                        $("#hddnwarehousetyoe").val(vwarehousetype);
                        $("#hdnUOM").val(valueuom);
                        $("#hddnproductcode").val(valueid);

                    }


                    //  alert(valuetype);
                    if (valueid != null && valueqt != null && valuepr != null) {

                        transfrm = valuetype + '|' + valueid + '|' + valueqt + '|' + valueuom + '|' + valuepr + '@'

                    }

                    transfrmtot += transfrm
                }


                var str = transfrmtot.substring(0, transfrmtot.length - 1);

                //  alert(flagqty);

                if (flagprod == true) {
                    if (flagqty == true) {

                        if (flagwarehouse == true) {

                            if (flaguom == true) {

                                if (flagsameproduct == true) {
                                    grid.PerformCallback('InsertJournal~' + str + '~' + 'save' + '~' + $("#hdnquantity").val());
                                }
                                else {

                                    jAlert('Same product should not be allowed to save');
                                }
                            }

                            else {
                                jAlert('Same UOM product should get selected');
                            }
                        }
                        else {
                            jAlert('Same inventory type Product required to transfer without Serial to Serial Product');

                        }
                    }
                    else {
                        jAlert('Quantity should be same for products and warehouse seleted quantity');
                    }

                }
                else {
                    jAlert('Both Stk-Out (Consumable) and Stk-In (Receipt) should be mandatory');

                }


            }

            else {

                jAlert('Select Product.');
            }

        }

        else {
            jAlert('Mandatory fields are required');
        }


    }

    else {

        jAlert('Warehouse need to select.');

    }

}

function SaveExit_Click() {

    $("#hdnquantity").val('');
    $("#hddnwarehousetyoe").val('');
    $("#hdnUOM").val('');
    $("#hddnproductcode").val('');

    if ($("#hdninventorytype").val() != '') {
        flag = true;
        //  LoadingPanel.Show();
        // alert($("#ddlwarehouse").val());
        var txtPurchaseNo = $("#txtVoucherNo").val().trim();
        if (txtPurchaseNo == null || txtPurchaseNo == "") {
            //LoadingPanel.Hide();
            $("#MandatoryBillNo").show();
            flag = false;
            return false;
        }
        else {
            $('#MandatoryBillNo').attr('style', 'display:none');
        }


        if (cPLQuoteDate.GetDate() == null) {
            flag = false;
            return false;
        }
        var ProjectCode = clookup_Project.GetText();
        if ($("#hdnProjectSelectInEntryModule").val() == "1" && $("#hdnProjectMandatory").val() == "1" && ProjectCode == "") {
            jAlert("Please Select Project.");
            flag = false;
            return false;
        }


        var RowCount = 0;
        var IsProduct = "";
        for (var i = 0; i <= grid.GetVisibleRowsOnPage() ; i++) {
            var ProductID = (grid.batchEditApi.GetCellValue(RowCount, 'ProductName') != null) ? (grid.batchEditApi.GetCellValue(RowCount, 'ProductName')) : "";

            if (ProductID != "") {
                IsProduct = "Y";
                break;
            }

            RowCount++;

        }

        if (flag != false) {
            if (IsProduct == "Y") {
                flagqty = true;
                flaguom = true;
                flagprod = true;
                flagwarehouse = true;
                flagsameproduct = true;

                var transfrm = ''
                var transfrmtot = ''
                for (var i = 0; i < grid.GetVisibleRowsOnPage() ; i++) {

                    var valueid = grid.batchEditApi.GetCellValue(i, "ProductID").split('||@||')[0];
                    var valueqt = grid.batchEditApi.GetCellValue(i, "Quantity");
                    var valuepr = grid.batchEditApi.GetCellValue(i, "PurchasePrice");
                    var valueuom = grid.batchEditApi.GetCellValue(i, "PurchaseUOM");
                    var valuetype = grid.batchEditApi.GetCellValue(i, "Type");


                    var vwarehousetype = grid.batchEditApi.GetCellValue(i, "ProductID").split('||@||')[21];

                    if (valueid == '') {

                        flagprod = false;
                    }

                    //  alert(vwarehousetype);

                    if ($("#hdnquantity").val() != '') {

                        if (Math.floor(valueqt) != Math.floor($("#hdnquantity").val())) {
                            flagqty = false;
                        }


                        if (Math.floor($("#hddnwarehouseqty").val()) != Math.floor($("#hdnquantity").val())) {
                            flagqty = false;
                        }

                    }

                    if ($("#hddnwarehousetyoe").val() != '') {

                        if ($("#hddnwarehousetyoe").val() == 'W') {

                            if (vwarehousetype != ($("#hddnwarehousetyoe").val())) {
                                flagwarehouse = false;
                            }


                        }

                    }


                    if ($("#hdnUOM").val() != '') {
                        if (valueuom != $("#hdnUOM").val()) {
                            flaguom = false;
                        }
                    }


                    if ($("#hddnproductcode").val() != '') {
                        if (valueid == $("#hddnproductcode").val()) {
                            flagsameproduct = false;
                        }
                    }

                    // alert($("#hdnUOM").val()+''+valueuom);

                    if (i == 0) {

                        $("#hdnquantity").val(valueqt);
                        $("#hddnwarehousetyoe").val(vwarehousetype);
                        $("#hdnUOM").val(valueuom);
                        $("#hddnproductcode").val(valueid);

                    }



                    //  alert(valuetype);
                    if (valueid != null && valueqt != null && valuepr != null) {

                        transfrm = valuetype + '|' + valueid + '|' + valueqt + '|' + valueuom + '|' + valuepr + '@'

                    }

                    transfrmtot += transfrm
                }
                //  alert(transfrmtot);

                ///Table value paired
                //grid.PerformCallback('InsertJournal~' + ProductID);

                var str = transfrmtot.substring(0, transfrmtot.length - 1);

                // alert(str);

                if (flagprod == true) {
                    if (flagqty == true) {

                        if (flagwarehouse == true) {
                            if (flaguom == true) {

                                if (flagsameproduct == true) {

                                    grid.PerformCallback('InsertJournal~' + str + '~' + 'exit' + '~' + $("#hdnquantity").val());

                                }
                                else {

                                    jAlert('Same product should not be allowed to save');
                                }

                            }
                            else {
                                jAlert('Same UOM product should get selected');
                            }
                        }
                        else {
                            jAlert('Same inventory type Product required');

                        }
                    }
                    else {
                        jAlert('Quantity should be same for products and warehouse seleted quantity');
                    }

                }

                else {
                    jAlert('Both Stk-Out (Consumable) and Stk-In (Receipt) Product should be mandatory');

                }


            }
            else {
                jAlert('Product need to select.');
            }
        }
        else {
            jAlert('Mandatory fields are required');
        }
    }

    else {

        jAlert('Warehouse need to select.');

    }
}

function cSelectPanelEndCall(s, e) {
    if (grid.cpSuccess != null) {

        if (grid.cpSuccess == 'save') {
            jAlert('Stock Journal Saved Successfully', 'Confirmation Dialog', function (r) {
                if (r == true) {
                    window.location.assign("Stock-journalTransfer.aspx?key=ADD");
                }
            });           
        }
        else if (grid.cpSuccess == 'exit') {
            window.location.assign("Stock-journalTransferList.aspx");
        }
        else if (grid.cpSuccess == "EmptyProject") {          
            grid.cpSuccess = null;
            jAlert('Please select project.');
        }
        else {
            jAlert(grid.cpSuccess);
        }
    }


}

function Openwarehousepopup() {



    var product = '';
    for (var i = 0; i < grid.GetVisibleRowsOnPage() ; i++) {

        var valuetype = grid.batchEditApi.GetCellValue(i, "Type");

        //  alert(valuetype);
        if (valuetype == 'Stk-Out (Consumable)') {


            product = grid.batchEditApi.GetCellValue(i, "ProductID").split('||@||')[0];
        }


    }

    cQuotationComponentPanel.PerformCallback('BindComponentGrid~' + product);

}



function Openwarehousepopup_view() {
    $('#stockModelview').modal('show');
}

function Closewarehousepopup_view() {
    $('#stockModelview').modal('hide');
}


function Closewarehousepopup() {

    if ($("#txtqty").val() != '') {
        var valueqt = 0;
        //alert(Math.floor($("#hdntotWHquantity").val()) + '-' + $("#txtqty").val());

        if (Math.floor($("#hdntotWHquantity").val()) < Math.floor($("#txtqty").val())) {

            jAlert('Quantity should be same or less than warehouse quantity.');

        }
        else {


            cQuotationComponentPanel.PerformCallback('Closewarehouse~' + $("#hdninventorytype").val() + '~' + $("#ddlwarehouse").val());
        }
    }
    else {

        $("#txtqty").val(0);
    }

}

function OnGetRowValues(values) {

    var item = values[1];
    alert(item.value);
    $('#stockModel').modal('hide');
    // cQuotationComponentPanel.PerformCallback('Closewarehouse~' + $("#hdninventorytype").val() + '~' + $("#ddlwarehouse").val());
}

function Onpanelendcallback() {
    if (cQuotationComponentPanel.cpSuccess != null) {


        ///alert(cQuotationComponentPanel.cpclose);

        if (cQuotationComponentPanel.cpSuccess == 'No warehouse') {
            $('#stockModel').modal('hide');
            jAlert('There is no stock in the selected Branch/Warehouse');
        }
        else {
            if (cQuotationComponentPanel.cpclose == 'open') {

                $('#stockModel').modal('show');
                $("#hdninventorytype").val(cQuotationComponentPanel.cpSuccess);
            }
            else if (cQuotationComponentPanel.cpclose == 'close') {

                $('#stockModel').modal('hide');
            }

        }
    }
    if (cQuotationComponentPanel.cpquantity != null) {

        $("#hddnwarehouseqty").val(cQuotationComponentPanel.cpquantity);

    }

    //if (cQuotationComponentPanel.cphiddenqty != null) {

    //    $("#hddnwarehouseqty").val(cQuotationComponentPanel.cphiddenqty);

    //}

}

function Warehouseselectedcount() {


    gridwarehouse.GetSelectedFieldValues('SerialID', OnGetSelectedFieldValues);
}

function OnGetSelectedFieldValues(selectedValues) {


    //alert(selectedValues.length);
    $("#txtqtyWHSL").val(selectedValues.length);
}
