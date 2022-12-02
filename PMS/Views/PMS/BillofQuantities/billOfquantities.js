
    var taging = "false";
var ProdTagID = [];
var ProdIndexAddl = 0;
var ResIndexAddl = 0;
var globalrowindex2 = 0;
var globalrowindex = 0;
var gridtxtbox = '1';
var slno = 0;
var firsttime = 0;
var DetailsID = 0;
var ProductionID = 0;
var GBOQNo = "";
var Message = "";
var savemode = "";
var hasmsg = 0;
var rowtime = 0;
var rowtime2 = 0;
var PrdProdNameIndex = 0, PrdProdDescIndex = 0, PrdProdQtyIndex = 0, PrdProdUOMIndex = 0, PrdWarehouseIndex = 0, PrdPriceIndex = 0, PrdDiscountIndex = 0, PrdAmountIndex = 0;
var PrdTaxTypeIndex = 0, PrdChargesIndex = 0, PrdNetAmtIndex = 0, PrdBudgPricIndex = 0, PrdAddlDescIndex = 0, PrdRemarksIndex = 0, PrdProdIdIndex = 0, PrdProdWarehouseIDIndex = 0;
var PrdUpdateEditIndex = 0, PrdTagDetlsIDIndex = 0, PrdTagProdnIDIndex = 0, PrdTaxTypeIDIndex = 0, PrdProdHSNIndex = 0, PrdSrlIndex;

var ResProductNameIndex = 0, ResProdDescIndex = 0, ResProdQtyIndex = 0, ResProdUOMIndex = 0, ResWarehouseIndex = 0, ResPriceIndex = 0, ResDiscountIndex = 0, ResAmountIndex = 0;
var ResTaxTypeIndex = 0, ResResourceChargesIndex = 0, ResNetAmountIndex = 0, ResBudgetedPriceIndex = 0, ResAddlDescIndex = 0, ResRemarksIndex = 0, ResProductIdIndex = 0, ResProdWarehouseIDIndex = 0;
var ResUpdateEditIndex = 0, ResTaxTypeIDIndex = 0, ResProdHSNIndex = 0, ResSrlIndex;
function btnProductList_Click(s, e) {
    Productlist('', null);
    setTimeout(function () { $("#txtProductName").focus(); }, 500);
    $('#ProductlistModel').modal('show');


}

$(document).ready(function () {
    LoadingPanel.Show();

    $('#GridAddlDescModel').on('shown.bs.modal', function () {
        $('#txt_AddlDesc').focus();
    });

    $('#RescTaxTypelistModel').on('shown.bs.modal', function () {
        $('#ddlRescTaxTypelist').focus();
    });

    $('#GridTaxTypelistModel').on('shown.bs.modal', function () {
        $('#ddlTaxTypelist').focus();
    });


    setTimeout(function () {
        AddNewRowWithSl();
        AddNewRowGridResources();
        //gridBOQProductEntryList.CancelEdit();
        $("#ddlSchema").focus();



    }, 200);


    var columnslres = gridBOQResourcesList.GetColumnByField("SlNO");
    ResSrlIndex = columnslres.index;

    var columnsl = gridBOQProductEntryList.GetColumnByField("SlNO");
    PrdSrlIndex = columnsl.index;

    var column1 = gridBOQProductEntryList.GetColumnByField("ProductName");
    PrdProdNameIndex = column1.index;

    var column2 = gridBOQProductEntryList.GetColumnByField("ProductDescription");
    PrdProdDescIndex = column2.index;

    var column3 = gridBOQProductEntryList.GetColumnByField("ProductQty");
    PrdProdQtyIndex = column3.index;

    var column4 = gridBOQProductEntryList.GetColumnByField("ProductUOM");
    PrdProdUOMIndex = column4.index;

    var column5 = gridBOQProductEntryList.GetColumnByField("Warehouse");
    PrdWarehouseIndex = column5.index;

    var column6 = gridBOQProductEntryList.GetColumnByField("Price");
    PrdPriceIndex = column6.index;

    var column7 = gridBOQProductEntryList.GetColumnByField("Discount");
    PrdDiscountIndex = column7.index;

    var column8 = gridBOQProductEntryList.GetColumnByField("Amount");
    PrdAmountIndex = column8.index;

    var column9 = gridBOQProductEntryList.GetColumnByField("TaxType");
    PrdTaxTypeIndex = column9.index;

    var column10 = gridBOQProductEntryList.GetColumnByField("Charges");
    PrdChargesIndex = column10.index;

    var column11 = gridBOQProductEntryList.GetColumnByField("NetAmount");
    PrdNetAmtIndex = column11.index;

    var column12 = gridBOQProductEntryList.GetColumnByField("BudgetedPrice");
    PrdBudgPricIndex = column12.index;

    //var column40 = gridBOQProductEntryList.GetColumnByField("AddlDesc");
    PrdAddlDescIndex = 4;

    var column13 = gridBOQProductEntryList.GetColumnByField("Remarks");
    PrdRemarksIndex = column13.index;

    var column14 = gridBOQProductEntryList.GetColumnByField("ProductId");
    PrdProdIdIndex = column14.index;

    var column15 = gridBOQProductEntryList.GetColumnByField("ProductsWarehouseID");
    PrdProdWarehouseIDIndex = column15.index;

    var column16 = gridBOQProductEntryList.GetColumnByField("UpdateEdit");
    PrdUpdateEditIndex = column16.index;

    var column17 = gridBOQProductEntryList.GetColumnByField("Tag_Details_ID");
    PrdTagDetlsIDIndex = column17.index;

    var column18 = gridBOQProductEntryList.GetColumnByField("Tag_Production_ID");
    PrdTagProdnIDIndex = column18.index;

    var column19 = gridBOQProductEntryList.GetColumnByField("TaxTypeID");
    PrdTaxTypeIDIndex = column19.index;

    var column20 = gridBOQProductEntryList.GetColumnByField("ProdHSN");
    PrdProdHSNIndex = column20.index;

    var column21 = gridBOQResourcesList.GetColumnByField("ProductName");
    ResProductNameIndex = column21.index;

    var column22 = gridBOQResourcesList.GetColumnByField("ProductDescription");
    ResProdDescIndex = column22.index;

    var column23 = gridBOQResourcesList.GetColumnByField("ProductQty");
    ResProdQtyIndex = column23.index;

    var column24 = gridBOQResourcesList.GetColumnByField("ProductUOM");
    ResProdUOMIndex = column24.index;

    var column25 = gridBOQResourcesList.GetColumnByField("Warehouse");
    ResWarehouseIndex = column25.index;

    var column26 = gridBOQResourcesList.GetColumnByField("Price");
    ResPriceIndex = column26.index;

    var column27 = gridBOQResourcesList.GetColumnByField("Discount");
    ResDiscountIndex = column27.index;

    var column28 = gridBOQResourcesList.GetColumnByField("Amount");
    ResAmountIndex = column28.index;

    var column29 = gridBOQResourcesList.GetColumnByField("TaxType");
    ResTaxTypeIndex = column29.index;

    var column30 = gridBOQResourcesList.GetColumnByField("ResourceCharges");
    ResResourceChargesIndex = column30.index;

    var column31 = gridBOQResourcesList.GetColumnByField("NetAmount");
    ResNetAmountIndex = column31.index;

    var column32 = gridBOQResourcesList.GetColumnByField("BudgetedPrice");
    ResBudgetedPriceIndex = column32.index;

    //var column33 = gridBOQResourcesList.GetColumnByField("AddlDesc");
    ResAddlDescIndex = 4;//column33.index;

    var column34 = gridBOQResourcesList.GetColumnByField("Remarks");
    ResRemarksIndex = column34.index;

    var column35 = gridBOQResourcesList.GetColumnByField("ProductId");
    ResProductIdIndex = column35.index;

    var column36 = gridBOQResourcesList.GetColumnByField("ProductsWarehouseID");
    ResProdWarehouseIDIndex = column36.index;

    var column37 = gridBOQResourcesList.GetColumnByField("UpdateEdit");
    ResUpdateEditIndex = column37.index;

    var column38 = gridBOQResourcesList.GetColumnByField("TaxTypeID");
    ResTaxTypeIDIndex = column38.index;

    var column39 = gridBOQResourcesList.GetColumnByField("ProdHSN");
    ResProdHSNIndex = column39.index;

    //  LoadingPanel.Hide();
});

function AddNewRowGridResources() {
    //gridBOQResourcesList.batchEditApi.StartEdit(index, 1);
    gridBOQResourcesList.batchEditApi.EndEdit();
    gridBOQResourcesList.AddNewRow();
    index = globalrowindex2;
    resufflegrid2Serial();

    setTimeout(function () {
        gridBOQResourcesList.batchEditApi.EndEdit();
        gridBOQResourcesList.batchEditApi.StartEdit(index, ResSrlIndex);
    }, 200);
}

function resufflegrid2Serial() {
    var sl = 1;
    var visiablerow = gridBOQResourcesList.GetVisibleRowsOnPage();
    if (visiablerow > 0 && rowtime2 == 0) {
        sl = visiablerow;
        rowtime2++;
    }
    for (var i = -1; i > -500; i--) {
        if (gridBOQResourcesList.GetRow(i)) {
            gridBOQResourcesList.batchEditApi.StartEdit(i, ResSrlIndex);
            gridBOQResourcesList.GetEditor('SlNO').SetText(sl);
            gridBOQResourcesList.batchEditApi.StartEdit(i, ResSrlIndex);
            sl = sl + 1;
        }
    }
}

function addNewRowToEditgrid() {
    gridBOQProductEntryList.batchEditApi.EndEdit();
    gridBOQProductEntryList.AddNewRow();

    var sl = 1;
    var visiablerow = gridBOQProductEntryList.GetVisibleRowsOnPage();
    if (visiablerow > 0) {
        sl = visiablerow;
    }
    gridBOQProductEntryList.batchEditApi.StartEdit(globalrowindex, PrdSrlIndex);
    gridBOQProductEntryList.GetEditor('SlNO').SetText(sl);


    setTimeout(function () {
        gridBOQProductEntryList.batchEditApi.EndEdit();
        gridBOQProductEntryList.batchEditApi.StartEdit(globalrowindex, PrdProdNameIndex);
    }, 200);


}

function addNewRowToEditResourcegrid() {
    gridBOQResourcesList.batchEditApi.EndEdit();
    gridBOQResourcesList.AddNewRow();

    var sl = 1;
    var visiablerow = gridBOQResourcesList.GetVisibleRowsOnPage();
    if (visiablerow > 0) {
        sl = visiablerow;
    }
    gridBOQResourcesList.batchEditApi.StartEdit(globalrowindex2, ResSrlIndex);
    gridBOQResourcesList.GetEditor('SlNO').SetText(sl);

    setTimeout(function () {
        gridBOQResourcesList.batchEditApi.EndEdit();
        gridBOQResourcesList.batchEditApi.StartEdit(globalrowindex2, ResProductNameIndex);
    }, 200);


}

function DeleteRowProductGrid(edit) {
    var sl = 1;
    gridBOQProductEntryList.batchEditApi.EndEdit();
    for (var i = 0; i < 500; i++) {
        if (gridBOQProductEntryList.GetRow(i) && i != edit) {
            var tr = gridBOQProductEntryList.GetRow(i);
            if (tr.style.display != "none") {

                gridBOQProductEntryList.batchEditApi.StartEdit(i, PrdSrlIndex);
                gridBOQProductEntryList.GetEditor('SlNO').SetText(sl);
                //if (grid.GetEditor('low').GetText() == "") {
                //    grid.GetEditor('low').SetText(0);
                //    grid.GetEditor('high').SetText(0);
                //    grid.GetEditor('value').SetText(0);
                //}
                gridBOQProductEntryList.batchEditApi.StartEdit(i, PrdSrlIndex);
                sl = sl + 1;
            }
        }
    }

    for (var i = -1; i > -500; i--) {
        if (gridBOQProductEntryList.GetRow(i) && i != edit) {

            var tr = gridBOQProductEntryList.GetRow(i);
            if (tr.style.display != "none") {

                gridBOQProductEntryList.batchEditApi.StartEdit(i, PrdSrlIndex);
                gridBOQProductEntryList.GetEditor('SlNO').SetText(sl);
                //if (grid.GetEditor('low').GetText() == "") {
                //    grid.GetEditor('low').SetText(0);
                //    grid.GetEditor('high').SetText(0);
                //    grid.GetEditor('value').SetText(0);
                //}
                gridBOQProductEntryList.batchEditApi.StartEdit(i, PrdSrlIndex);
                sl = sl + 1;
            }
        }
    }
}

function DeleteRowResourceGrid(edit) {
    var sl = 1;
    gridBOQResourcesList.batchEditApi.EndEdit();
    for (var i = 0; i < 500; i++) {
        if (gridBOQResourcesList.GetRow(i) && i != edit) {
            var tr = gridBOQResourcesList.GetRow(i);
            if (tr.style.display != "none") {

                gridBOQResourcesList.batchEditApi.StartEdit(i, ResSrlIndex);
                gridBOQResourcesList.GetEditor('SlNO').SetText(sl);
                //if (grid.GetEditor('low').GetText() == "") {
                //    grid.GetEditor('low').SetText(0);
                //    grid.GetEditor('high').SetText(0);
                //    grid.GetEditor('value').SetText(0);
                //}
                gridBOQResourcesList.batchEditApi.StartEdit(i, ResSrlIndex);
                sl = sl + 1;
            }
        }
    }

    for (var i = -1; i > -500; i--) {
        if (gridBOQResourcesList.GetRow(i) && i != edit) {

            var tr = gridBOQResourcesList.GetRow(i);
            if (tr.style.display != "none") {

                gridBOQResourcesList.batchEditApi.StartEdit(i, ResSrlIndex);
                gridBOQResourcesList.GetEditor('SlNO').SetText(sl);
                //if (grid.GetEditor('low').GetText() == "") {
                //    grid.GetEditor('low').SetText(0);
                //    grid.GetEditor('high').SetText(0);
                //    grid.GetEditor('value').SetText(0);
                //}
                gridBOQResourcesList.batchEditApi.StartEdit(i, ResSrlIndex);
                sl = sl + 1;
            }
        }
    }
}

/*---------------Arindam*----------*/
function AddNewRowWithSl() {

    gridBOQProductEntryList.batchEditApi.EndEdit();
    gridBOQProductEntryList.AddNewRow();
    index = globalrowindex;
    resuffleSerial();

    setTimeout(function () {
        gridBOQProductEntryList.batchEditApi.EndEdit();
        gridBOQProductEntryList.batchEditApi.StartEdit(index, PrdSrlIndex);
    }, 200);
}


function resuffleSerial() {
    //  debugger;
    var sl = 1;
    var visiablerow = gridBOQProductEntryList.GetVisibleRowsOnPage();
    if (visiablerow > 0 && rowtime == 0) {
        sl = visiablerow;
        rowtime++;
    }

    for (var i = -1; i > -500; i--) {
        if (gridBOQProductEntryList.GetRow(i)) {
            gridBOQProductEntryList.batchEditApi.StartEdit(i, PrdSrlIndex);
            gridBOQProductEntryList.GetEditor('SlNO').SetText(sl);
            //if (grid.GetEditor('low').GetText() == "") {
            //    grid.GetEditor('low').SetText(0);
            //    grid.GetEditor('high').SetText(0);
            //    grid.GetEditor('value').SetText(0);
            //}
            gridBOQProductEntryList.batchEditApi.StartEdit(i, PrdSrlIndex);
            sl = sl + 1;
        }
    }
}


function grid_CustomButtonGridResourcesClick() {
    //if (e.buttonID == "Delete") {
    var noofvisiblerows = gridBOQResourcesList.GetVisibleRowsOnPage();

    if (noofvisiblerows != 1) {
        gridBOQResourcesList.DeleteRow(globalrowindex2);

        if ($('#hdnDetailsID').val() == 0) {
            resufflegrid2Serial();
        }
        else {
            DeleteRowResourceGrid(globalrowindex2);
        }

        BOQGridResourceSetTotalAmount();
    }
    //}
    //e.processOnServer = false;

}

function grid_CustomButtonClick() {
    // if (e.buttonID == "DeleteProduct") {
    var noofvisiblerows = gridBOQProductEntryList.GetVisibleRowsOnPage();

    if (noofvisiblerows != 1) {
        gridBOQProductEntryList.DeleteRow(globalrowindex);
        //gridBOQProductEntryList.DeleteRow(e.visibleIndex);
        if ($('#hdnDetailsID').val() == 0) {

            resuffleSerial();
        }
        else {
            DeleteRowProductGrid(globalrowindex);
            // DeleteRowProductGrid(e.visibleIndex);
        }

        //     BOQProdGridSetTotalAmount();
    }
    //}
    //e.processOnServer = false;
}


function BatchStartEditing(s, e) {
    globalrowindex = e.visibleIndex;
}

function ResourceBatchStartEditing(s, e) {
    globalrowindex2 = e.visibleIndex;
}

var typemodal = "";
function OpenNonInventoryProductList(s, e) {

    gridBOQResourcesList.batchEditApi.EndEdit();
    var slsno = gridBOQResourcesList.batchEditApi.GetCellValue(e.visibleIndex, 'SlNO');
    var ProductName = gridBOQResourcesList.batchEditApi.GetCellValue(e.visibleIndex, 'ProductName');
    $("#hdnResAddlDescSl").val(slsno);

    if ($("#CustomerId").val() == null || $("#CustomerId").val() == "") {
        jAlert('Please select customer.');
        CustomerTxt.Focus();
        LoadingPanel.Hide();
    }
    else {
        slno = gridBOQResourcesList.GetDataRow(globalrowindex2).children[0].innerHTML.trim();
        GridNonInventoryProductlist("", "nonInventory", slno);
        typemodal = "nonInventory";
        var txt = "<table border='1' class='dynamicPopupTbl' width=\"100%\"><tr class=\"HeaderStyle\"><th class='hide'>id</th><th>Product Code</th><th>Product Name</th><th>Inventory</th><th>HSN/SAC</th><th>Class</th><th>Brand</th></tr><table>";
        $("#txtGridProductName").val("");
        document.getElementById("GridProductTable").innerHTML = txt;
        $('#GridProductlistModel').modal('show');
        $('#txtGridProductName').focus();

        setTimeout(function () {
            $('#txtGridProductName').focus();
        }, 600);
    }
}

function GridNonInventoryProductlist(SearchKey, type, txtid) {
    if (SearchKey != "") {
        gridnonproductlist = 1;
        var OtherDetails = {}
        OtherDetails.SearchKey = SearchKey;
        OtherDetails.Type = type;
        gridtxtbox = txtid;
        var HeaderCaption = [];
        // HeaderCaption.push("Product ID");
        HeaderCaption.push("Product Code");
        HeaderCaption.push("Product Name");
        HeaderCaption.push("Inventory");
        HeaderCaption.push("HSN/SAC");
        HeaderCaption.push("Class");
        HeaderCaption.push("Brand");

        callonServerScroll("../Models/PMS_WebServiceList.asmx/GetProductDetailsList", OtherDetails, "GridProductTable", HeaderCaption, "NonIProductIndex", "SetGridNonInventoryProduct");
    }

    //setTimeout(function () {
    //    $('#txtGridProductName').focus();
    //}, 600);
}

function OpenProductList(s, e) {


    gridBOQProductEntryList.batchEditApi.EndEdit();
    var slsno = gridBOQProductEntryList.batchEditApi.GetCellValue(e.visibleIndex, 'SlNO');
    var ProductName = gridBOQProductEntryList.batchEditApi.GetCellValue(e.visibleIndex, 'ProductName');
    $("#hdnProdAddlDescSl").val(slsno);

    if ($("#CustomerId").val() == null || $("#CustomerId").val() == "") {
        jAlert('Please select customer.');
        CustomerTxt.Focus();
        LoadingPanel.Hide();
    }
    else {
        if (gridBOQProductEntryList.GetDataRow(globalrowindex) != null) {
            slno = gridBOQProductEntryList.GetDataRow(globalrowindex).children[0].innerHTML.trim();
        }
        //debugger;
        GridProductlist("", "A", slno);
        typemodal = "A";

        var txt = "<table  class='dynamicPopupTbl' width=\"100%\"><tr class=\"HeaderStyle\"><th class='hide'>id</th><th>Product Code</th><th>Product Name</th><th>Inventory</th><th>HSN/SAC</th><th>Class</th><th>Brand</th></tr><table>";
        $("#txtGridProductName").val("");
        document.getElementById("GridProductTable").innerHTML = txt;
        $('#GridProductlistModel').modal('show');
        $('#txtGridProductName').focus();
        setTimeout(function () {
            $('#txtGridProductName').focus();
        }, 200);
    }
}

function OpenTaxTypeList(s, e) {
    if (gridBOQProductEntryList.GetDataRow(globalrowindex) != null) {
        slno = gridBOQProductEntryList.GetDataRow(globalrowindex).children[0].innerHTML.trim();
    }
    $('#GridTaxTypelistModel').modal('show');
    //setTimeout(function () {
    //    $('#ddlTaxTypelist').focus();
    //}, 600);
}

function OpenAddlDesc(s, e) {
    //debugger;
    //e.processOnServer = true;
    gridBOQProductEntryList.batchEditApi.EndEdit();
    if (gridBOQProductEntryList.GetDataRow(globalrowindex) != null) {
        slno = gridBOQProductEntryList.GetDataRow(globalrowindex).children[0].innerHTML.trim();
    }
    //gridBOQProductEntryList.batchEditApi.StartEdit(e.visibleIndex, PrdSrlIndex);
    //var slsno = gridBOQProductEntryList.GetEditor('SlNO').GetText();

    var slsno = gridBOQProductEntryList.batchEditApi.GetCellValue(globalrowindex, 'SlNO');

    // var ProductName = gridBOQProductEntryList.GetEditor('ProductName').GetText();
    var ProductName = gridBOQProductEntryList.batchEditApi.GetCellValue(globalrowindex, 'ProductName');
    ProdIndexAddl = globalrowindex;
    if (ProductName == "" || ProductName == null) {
        jAlert("Please select product before select Addl. Desc.!");
        setTimeout(function () {
            gridBOQProductEntryList.batchEditApi.StartEdit(globalrowindex, PrdSrlIndex);
        }, 600);
        return false;
    }
    else {
        if (slsno != null) {
            var txt_AddlDesc = $('#txt_AddlDesc').val();
            $("#hdnProdAddlDescSl").val(slsno);
            $.ajax({
                type: "POST",
                url: "@Url.Action("ProdAdditionalDesc", "BillofQuantities")",
                data: { AddlDesc: txt_AddlDesc, ProdAddlDescSl: slsno, Command: "RemarksDisplay" },
            success: function (response) {
                if (response != null) {
                    //jAlert(response.Message);
                    $('#txt_AddlDesc').val(response.Message);
                }
            }
        });
    }

    $('#GridAddlDescModel').modal('show');
    //setTimeout(function () {
    //    $('#txt_AddlDesc').focus();
    //}, 600);
}
}


function OpenResAddlDesc(s, e) {
    // debugger;

    gridBOQResourcesList.batchEditApi.EndEdit();
    if (gridBOQResourcesList.GetDataRow(globalrowindex2) != null) {
        slno = gridBOQResourcesList.GetDataRow(globalrowindex2).children[0].innerHTML.trim();
    }
    //gridBOQResourcesList.batchEditApi.StartEdit(e.visibleIndex, ResSrlIndex);
    //var slsno = gridBOQResourcesList.GetEditor('SlNO').GetText(); e.visibleIndex

    var slsno = gridBOQResourcesList.batchEditApi.GetCellValue(globalrowindex2, 'SlNO');
    // var ProductName = gridBOQResourcesList.GetEditor('ProductName').GetText();
    var ProductName = gridBOQResourcesList.batchEditApi.GetCellValue(globalrowindex2, 'ProductName');
    ResIndexAddl = globalrowindex2;
    if (ProductName == "" || ProductName == null) {
        jAlert("Please select product before select Addl. Desc.!");
        setTimeout(function () {
            gridBOQResourcesList.batchEditApi.StartEdit(globalrowindex2, ResSrlIndex);
        }, 600);
        return false;
    }
    else {
        if (slsno != null) {
            var txt_ResAddlDesc = $('#txt_ResAddlDesc').val();
            $("#hdnResAddlDescSl").val(slsno);
            $.ajax({
                type: "POST",
                url: "@Url.Action("ResAdditionalDesc", "BillofQuantities")",
                data: { AddlDesc: txt_ResAddlDesc, ResAddlDescSl: slsno, Command: "RemarksDisplay" },
            success: function (response) {
                if (response != null) {
                    //jAlert(response.Message);
                    $('#txt_ResAddlDesc').val(response.Message);
                }
            }
        });
    }

    $('#GridResAddlDescModel').modal('show');
    setTimeout(function () {
        $('#txt_ResAddlDesc').focus();
    }, 600);
}
}

function OpenResourceTaxTypeList(s, e) {
    if (gridBOQResourcesList.GetDataRow(globalrowindex2) != null) {
        slno = gridBOQResourcesList.GetDataRow(globalrowindex2).children[0].innerHTML.trim();
    }
    $('#RescTaxTypelistModel').modal('show');
    //setTimeout(function () {
    //    $('#ddlRescTaxTypelist').focus();
    //}, 600);
}

function OpenWarehouseList(s, e) {
    slno = gridBOQProductEntryList.GetDataRow(globalrowindex).children[0].innerHTML.trim();
    gridBOQProductEntryList.batchEditApi.StartEdit(globalrowindex, PrdProdNameIndex);
    var ProductName = gridBOQProductEntryList.GetEditor('ProductName').GetText();
    if (ProductName == "") {
        jAlert("Please select product before select warehouse!");
        return false;
    }
    else {
        GridWarehouselist();
        warehousefocus = 1;

    }
}

function OpenChargesList(s, e) {
    slno = gridBOQProductEntryList.GetDataRow(globalrowindex).children[0].innerHTML.trim();
    gridBOQProductEntryList.batchEditApi.StartEdit(globalrowindex, PrdProdNameIndex);
    var ProductName = gridBOQProductEntryList.GetEditor('ProductName').GetText();
    if (ProductName == "") {
        jAlert("Please select product before select warehouse!");
        return false;
    }
    else {
        //GridWarehouselist();
        // warehousefocus = 1;

    }
}

function GridWarehouselist() {
    var BankBranchID = $('#ddlBankBranch option:selected').val();
    if (BankBranchID > 0) {
        $.ajax({
            type: "POST",
            url: "@Url.Action("getWarehouseRecord", "BillofQuantities")",
            data: { branchid: BankBranchID },
        success: function (response) {
            $('#ddlWarehouselist').html('');
            var html = "";
            for (var i = 0; i < response.length; i++) {
                html = html + "<option value='" + response[i].WarehouseID + "'>" + response[i].WarehouseName + "</option>";
            }
            $('#ddlWarehouselist').html(html);
            gridBOQProductEntryList.batchEditApi.EndEdit();
            //$('#setWarehousegrid').focus();
            $('#GridWarehouselistModel').modal('show');

            setTimeout(function () {
                $('#ddlWarehouselist').focus();
            }, 600);
        }
    });
}
else {
            jAlert('Please select branch!');
$('#GridWarehouselistModel').modal('hide');
return false;
}
}

function SetWarehouseInGrid() {

    var Warehouseid = $('#ddlWarehouselist option:selected').val();
    var Warehousetxt = $('#ddlWarehouselist option:selected').text();
    if (Warehousetxt != "") {
        var werehouses = Warehousetxt.split(':')[2];

        gridBOQProductEntryList.batchEditApi.StartEdit(globalrowindex, PrdWarehouseIndex);
        gridBOQProductEntryList.GetEditor('Warehouse').SetText(Warehousetxt);

        gridBOQProductEntryList.batchEditApi.StartEdit(globalrowindex, PrdProdWarehouseIDIndex);
        gridBOQProductEntryList.GetEditor('ProductsWarehouseID').SetText(Warehouseid);
        $('#GridWarehouselistModel').modal('hide');
        gridBOQProductEntryList.batchEditApi.StartEdit(globalrowindex, PrdPriceIndex);
    }
    else {
        jAlert("Please select warehouse!");
    }
}


function SetTaxTypeInGrid() {

    var TaxTypeid = $('#ddlTaxTypelist').val();
    var TaxTypetxt = $('#ddlTaxTypelist option:selected').text();
    if (TaxTypetxt != "") {
        gridBOQProductEntryList.batchEditApi.StartEdit(globalrowindex, PrdTaxTypeIndex);
        gridBOQProductEntryList.GetEditor('TaxType').SetText(TaxTypetxt);

        gridBOQProductEntryList.batchEditApi.StartEdit(globalrowindex, PrdTaxTypeIDIndex);
        gridBOQProductEntryList.GetEditor('TaxTypeID').SetText(TaxTypeid);
        $('#GridTaxTypelistModel').modal('hide');



        var Price = gridBOQProductEntryList.batchEditApi.GetCellValue(globalrowindex, 'Price');
        var Qty = gridBOQProductEntryList.batchEditApi.GetCellValue(globalrowindex, 'ProductQty');
        var Discount = gridBOQProductEntryList.batchEditApi.GetCellValue(globalrowindex, 'Discount');
        if (Price != "" && Qty != "") {
            var amount = parseFloat((parseFloat(Qty).toFixed(4)) * (parseFloat(Price).toFixed(2))).toFixed(2);
            var amountAfterDiscount = parseFloat(amount) - ((parseFloat(Discount) * parseFloat(amount)) / 100);
            var Tamount = parseFloat(DecimalRoundoff(amountAfterDiscount, 2)).toFixed(2);

            if (TaxTypeid == "3") {
                gridBOQProductEntryList.batchEditApi.StartEdit(globalrowindex, PrdAmountIndex);
                gridBOQProductEntryList.GetEditor('Amount').SetText(Tamount);

                gridBOQProductEntryList.batchEditApi.StartEdit(globalrowindex, PrdNetAmtIndex);
                gridBOQProductEntryList.GetEditor('NetAmount').SetText(Tamount);

                //if (gridBOQProductEntryList.batchEditApi.GetCellValue(globalrowindex, 11) == null) {
                gridBOQProductEntryList.batchEditApi.StartEdit(globalrowindex, PrdChargesIndex);
                gridBOQProductEntryList.GetEditor('Charges').SetText("0.00");
                //  }
            }
            else {
                var taxType = "O";
                var amtFor = $("#ddl_AmountAre").val();
                if (amtFor == "2") {
                    taxType = "I";
                }
                else if (amtFor == "1") {
                    taxType = "E";
                }

                var ret = caluculateAndSetGST(gridBOQProductEntryList.GetEditor("Amount"), gridBOQProductEntryList.GetEditor("Charges"), gridBOQProductEntryList.GetEditor("NetAmount"),
                        gridBOQProductEntryList.GetEditor("ProdHSN").GetText(), DecimalRoundoff(amount, 2), DecimalRoundoff(amountAfterDiscount, 2), taxType, "15", $('#ddlBankBranch').val())
                var data = ret.split('~');

                var Netamind = gridBOQProductEntryList.batchEditApi.GetColumnIndex('NetAmount');
                gridBOQProductEntryList.batchEditApi.StartEdit(globalrowindex, Netamind);
                gridBOQProductEntryList.GetEditor('NetAmount').SetText(data[2]);

                var Charges = gridBOQProductEntryList.batchEditApi.GetColumnIndex('Charges');
                gridBOQProductEntryList.batchEditApi.StartEdit(globalrowindex, Charges);
                gridBOQProductEntryList.GetEditor('Charges').SetText(data[1].toString());

                var amind = gridBOQProductEntryList.batchEditApi.GetColumnIndex('Amount');
                gridBOQProductEntryList.batchEditApi.StartEdit(globalrowindex, amind);
                gridBOQProductEntryList.GetEditor('Amount').SetText(data[0]);

            }
        }
        gridBOQProductEntryList.batchEditApi.StartEdit(globalrowindex, PrdChargesIndex);
    }
    else {
        jAlert("Please select Tax Type!");
    }
}

function SetProdAddlDescGrid() {
    var txt_AddlDesc = $('#txt_AddlDesc').val();
    var hdnProdAddlDescSl = $("#hdnProdAddlDescSl").val();
    if (txt_AddlDesc != "") {
        $('#GridAddlDescModel').modal('hide');
        $.ajax({
            type: "POST",
            url: "@Url.Action("ProdAdditionalDesc", "BillofQuantities")",
            data: { AddlDesc: txt_AddlDesc, ProdAddlDescSl: hdnProdAddlDescSl, Command: "RemarksAdd" },
        success: function (response) {
            if (response != null) {
                //jAlert(response.Message);
                $("#txt_AddlDesc").val('');
                $("#hdnProdAddlDescSl").val('');
                //debugger;
                gridBOQProductEntryList.batchEditApi.StartEdit(ProdIndexAddl, PrdProdQtyIndex);
            }
        }
    });
}
else {
//jAlert("Please Enter Additional Description.");
//$("#txt_AddlDesc").focus();
            $('#GridAddlDescModel').modal('hide');
$.ajax({
    type: "POST",
    url: "@Url.Action("ProdAdditionalDesc", "BillofQuantities")",
    data: { AddlDesc: txt_AddlDesc, ProdAddlDescSl: hdnProdAddlDescSl, Command: "RemarksRemove" },
success: function (response) {
    if (response != null) {
        //jAlert(response.Message);
        $("#txt_AddlDesc").val('');
        $("#hdnProdAddlDescSl").val('');
        //debugger;
        gridBOQProductEntryList.batchEditApi.StartEdit(ProdIndexAddl, PrdProdQtyIndex);
    }
}
});


gridBOQProductEntryList.batchEditApi.StartEdit(ProdIndexAddl, PrdProdQtyIndex);
}
}

function SetResAddlDescGrid() {
    var txt_AddlDesc = $('#txt_ResAddlDesc').val();
    var hdnResAddlDescSl = $("#hdnResAddlDescSl").val();
    if (txt_AddlDesc != "") {
        $('#GridResAddlDescModel').modal('hide');
        var strOut = txt_AddlDesc.substring(0, 7) + "...";
        $.ajax({
            type: "POST",
            url: "@Url.Action("ResAdditionalDesc", "BillofQuantities")",
            data: { AddlDesc: txt_AddlDesc, ResAddlDescSl: hdnResAddlDescSl, Command: "RemarksAdd" },
        success: function (response) {
            if (response != null) {
                //jAlert(response.Message);
                $("#txt_ResAddlDesc").val('');
                $("#hdnResAddlDescSl").val('');
                //debugger;
                gridBOQResourcesList.batchEditApi.StartEdit(ResIndexAddl, ResProdQtyIndex);
                // gridBOQResourcesList.GetEditor('AddlDesc').SetText(strOut);
            }
        }
    });
}
else {
//jAlert("Please Enter Additional Description.");
//$("#txt_ResAddlDesc").focus();
            $('#GridResAddlDescModel').modal('hide');

$.ajax({
    type: "POST",
    url: "@Url.Action("ResAdditionalDesc", "BillofQuantities")",
    data: { AddlDesc: txt_AddlDesc, ResAddlDescSl: hdnResAddlDescSl, Command: "RemarksRemove" },
success: function (response) {
    if (response != null) {
        //jAlert(response.Message);
        $("#txt_ResAddlDesc").val('');
        $("#hdnResAddlDescSl").val('');
        //debugger;
        gridBOQResourcesList.batchEditApi.StartEdit(ResIndexAddl, ResProdQtyIndex);
        // gridBOQResourcesList.GetEditor('AddlDesc').SetText('');
    }
}
});
}
}

function SetWarehouseAfterProduct() {
    gridBOQProductEntryList.batchEditApi.StartEdit(globalrowindex, PrdPriceIndex);
}

function SetTaxTypeAfterProduct() {
    //  gridBOQProductEntryList.batchEditApi.StartEdit(globalrowindex, 11);
}
function SetAddlDescClose() {
    $("#txt_AddlDesc").val('');
    $("#hdnProdAddlDescSl").val('');
    gridBOQProductEntryList.batchEditApi.StartEdit(ProdIndexAddl, PrdProdQtyIndex);
}

function SetResAddlDescClose() {
    $("#txt_ResAddlDesc").val('');
    $("#hdnResAddlDescSl").val('');
    gridBOQResourcesList.batchEditApi.StartEdit(ResIndexAddl, ResProdQtyIndex);
}

var globalrowindex = 0;
function gridclick(s, e) {
    globalrowindex = e.visibleIndex;



}

function gridResourceclick(s, e) {
    globalrowindex2 = e.visibleIndex;
}

var Estimatelinkindex = 0;
function OpenEstimateList(s, e) {
    slno = gridBOQProductEntryList.GetDataRow(globalrowindex).children[0].innerHTML.trim();
    gridBOQProductEntryList.batchEditApi.StartEdit(globalrowindex, PrdProdNameIndex);
    var ProductName = gridBOQProductEntryList.GetEditor('ProductName').GetText();
    if (ProductName == "") {
        jAlert("Please select product before select Estimate!");
        return false;
    }
    else {
        GetEstimateList('', slno);
        Estimatelinkindex = 1;
        setTimeout(function () { $("#txtEstimateName").focus(); }, 500);
        $('#GridEstimatelistModel').modal('show');

    }
}

function GetEstimateList(SearchKey, slno) {
    var productid = gridBOQProductEntryList.batchEditApi.GetCellValue(globalrowindex, 'ProductId');
    var OtherDetails = {}
    var EstimateDate = GetServerDateFormat(BOQDate_dt.GetValue());
    OtherDetails.SearchKey = SearchKey;
    OtherDetails.ProductID = productid;
    OtherDetails.EstimateDate = EstimateDate;
    OtherDetails.BranchID = $('#ddlBankBranch option:selected').val();
    var HeaderCaption = [];
    HeaderCaption.push("Estimate No.");
    HeaderCaption.push("Estimate Date");
    HeaderCaption.push("Revision No.");
    HeaderCaption.push("Revision Date");

    callonServerScroll("../Models/PMS_WebServiceList.asmx/GetBOMList", OtherDetails, "GridEstimateTable", HeaderCaption, "EstimateIndex", "SetGridEstimateProduct");

}

function GridProductlist(SearchKey, type, txtid) {
    //debugger;

    if (SearchKey != "") {
        gridproductlist = 1;
        var OtherDetails = {}
        OtherDetails.SearchKey = SearchKey;
        OtherDetails.Type = type;
        gridtxtbox = txtid;
        var HeaderCaption = [];
        // HeaderCaption.push("Product ID");
        HeaderCaption.push("Product Code");
        HeaderCaption.push("Product Name");
        HeaderCaption.push("Inventory");
        HeaderCaption.push("HSN/SAC");
        HeaderCaption.push("Class");
        HeaderCaption.push("Brand");

        callonServerScroll("../Models/PMS_WebServiceList.asmx/GetProductDetailsList", OtherDetails, "GridProductTable", HeaderCaption, "GridProductIndex", "SetGridProduct");
    }



}

function Productlist(SearchKey, type) {
    finishedproductlist = 1;
    var OtherDetails = {}
    OtherDetails.SearchKey = SearchKey;
    OtherDetails.Type = type;

    var HeaderCaption = [];
    // HeaderCaption.push("Product ID");
    HeaderCaption.push("Product Code");
    HeaderCaption.push("Product Name");
    HeaderCaption.push("Inventory");
    HeaderCaption.push("HSN/SAC");
    HeaderCaption.push("Class");
    HeaderCaption.push("Brand");

    callonServerScroll("../Models/PMS_WebServiceList.asmx/GetProductDetailsList", OtherDetails, "ProductTable", HeaderCaption, "ProductIndex", "SetProduct");
}

function SetGridNonInventoryProduct(Id, Name, e) {
    gridnonproductlist = 0;
    var ProductID = Id;
    var ProductName = Name;
    //alert('');
    if (ProductID != "") {

        var slno = $("#hdnResAddlDescSl").val();
        $.ajax({
            type: "POST",
            url: "@Url.Action("ResAdditionalDesc", "BillofQuantities")",
            data: { AddlDesc: ' ', ProdAddlDescSl: slno, Command: "RemarksRemove" },
        success: function (response) {
            if (response != null) {
                //jAlert(response.Message);
                //  $("#txt_AddlDesc").val('');
                $("#hdnResAddlDescSl").val('');
            }
        }
    });

    var data = ProductID.split('|');
    ProductID = data[0];

    var Dis = gridBOQResourcesList.batchEditApi.GetColumnIndex('Discount');
    gridBOQResourcesList.batchEditApi.StartEdit(globalrowindex2, Dis);
    gridBOQResourcesList.GetEditor('Discount').SetText("0.00");

    var amind = gridBOQResourcesList.batchEditApi.GetColumnIndex('Amount');
    gridBOQResourcesList.batchEditApi.StartEdit(globalrowindex2, amind);
    gridBOQResourcesList.GetEditor('Amount').SetText("0.00");

    var ProdHSN = gridBOQResourcesList.batchEditApi.GetColumnIndex('ProdHSN');
    gridBOQResourcesList.batchEditApi.StartEdit(globalrowindex2, ProdHSN);
    gridBOQResourcesList.GetEditor('ProdHSN').SetText(data[7]);

    var qtyindex = gridBOQResourcesList.batchEditApi.GetColumnIndex('ProductQty');
    gridBOQResourcesList.batchEditApi.StartEdit(globalrowindex2, qtyindex);
    gridBOQResourcesList.GetEditor('ProductQty').SetText("0.00");

    gridBOQResourcesList.batchEditApi.StartEdit(globalrowindex2, ResProductIdIndex);
    gridBOQResourcesList.GetEditor('ProductId').SetText(ProductID);

    gridBOQResourcesList.batchEditApi.StartEdit(globalrowindex2, ResResourceChargesIndex);
    gridBOQResourcesList.GetEditor('ResourceCharges').SetText("0.00");

    gridBOQResourcesList.batchEditApi.StartEdit(globalrowindex2, ResProductNameIndex);
    gridBOQResourcesList.GetEditor('ProductName').SetText(ProductName);
    //gridBOQResourcesList.GetDataRow(globalrowindex).children[1].innerHTML = ProductName;

    //$('#' + gridtxtbox + '_txtbox').val(ProductName);
    $("#ddl_AmountAre").prop('disabled', 'disabled');
    $('#GridProductlistModel').modal('hide');
    gridBOQResourcesList.batchEditApi.StartEdit(globalrowindex2, ResProdDescIndex);
    gridBOQResourcesList.GetEditor('ProductDescription').SetText(data[6]);
    gridBOQResourcesList.batchEditApi.StartEdit(globalrowindex2, ResProdUOMIndex);

    gridBOQResourcesList.GetEditor('ProductUOM').SetText(data[1]);
    //$('#' + gridtxtbox + '_txtDescription').val(data[2]);
    //$('#' + gridtxtbox + '_txtStockUOM').val(data[1]);
    gridBOQResourcesList.batchEditApi.StartEdit(globalrowindex2, ResPriceIndex);
    gridBOQResourcesList.GetEditor('Price').SetText(data[3]);
    gridBOQResourcesList.batchEditApi.StartEdit(globalrowindex2, ResProdWarehouseIDIndex);
    gridBOQResourcesList.GetEditor('ProductsWarehouseID').SetText(data[4]);
    //$('#' + gridtxtbox + '_txtPrice').val(data[3]);
    //gridBOQResourcesList.batchEditApi.StartEdit(globalrowindex2, 6);
    //gridBOQResourcesList.GetEditor('Warehouse').SetText(data[5]);
    gridBOQResourcesList.batchEditApi.StartEdit(globalrowindex2, ResProdDescIndex);
    //btnFinishedItem.SetText(ProductName);
    //document.getElementById('hdnProductID').value = Id;
}
}

function SetProduct(Id, Name, e) {
    finishedproductlist = 0;
    var ProductID = Id;
    var ProductName = Name;

    if (ProductID != "") {

        var data = ProductID.split('|');
        ProductID = data[0];
        //  $('#FinishedUom').val(data[1]);
        $('#ProductlistModel').modal('hide');
        // btnFinishedItem.SetText(ProductName);
        // $('#hdnFinishedItem').val(ProductID);
        document.getElementById('hdnProductID').value = Id;
        // $('#FinishedQty').select();
        //  $('#FinishedQty').focus();
        $("#ddl_AmountAre").prop('disabled', 'disabled');
    }
}

function SetGridProduct(Id, Name, e) {
    // debugger;
    gridproductlist = 0;
    var ProductID = Id;
    var ProductName = Name;

    if (ProductID != "") {

        var slno = $("#hdnProdAddlDescSl").val();
        $.ajax({
            type: "POST",
            url: "@Url.Action("ProdAdditionalDesc", "BillofQuantities")",
            data: { AddlDesc: ' ', ProdAddlDescSl: slno, Command: "RemarksRemove" },
        success: function (response) {
            if (response != null) {
                //jAlert(response.Message);
                $("#txt_AddlDesc").val('');
                $("#hdnProdAddlDescSl").val('');
            }
        }
    });
    var data = ProductID.split('|');
    ProductID = data[0];
    //debugger;
    var Dis = gridBOQProductEntryList.batchEditApi.GetColumnIndex('Discount');
    gridBOQProductEntryList.batchEditApi.StartEdit(globalrowindex, Dis);
    gridBOQProductEntryList.GetEditor('Discount').SetText("0.00");

    var amind = gridBOQProductEntryList.batchEditApi.GetColumnIndex('Amount');
    gridBOQProductEntryList.batchEditApi.StartEdit(globalrowindex, amind);
    gridBOQProductEntryList.GetEditor('Amount').SetText("0.00");

    var Netamind = gridBOQProductEntryList.batchEditApi.GetColumnIndex('NetAmount');
    gridBOQProductEntryList.batchEditApi.StartEdit(globalrowindex, Netamind);
    gridBOQProductEntryList.GetEditor('NetAmount').SetText("0.00");

    var HSN = gridBOQProductEntryList.batchEditApi.GetColumnIndex('ProdHSN');
    gridBOQProductEntryList.batchEditApi.StartEdit(globalrowindex, HSN);
    gridBOQProductEntryList.GetEditor('ProdHSN').SetText(data[7]);

    var qtyindex = gridBOQProductEntryList.batchEditApi.GetColumnIndex('ProductQty');
    gridBOQProductEntryList.batchEditApi.StartEdit(globalrowindex, qtyindex);
    gridBOQProductEntryList.GetEditor('ProductQty').SetText("0.00");

    var qtyindex = gridBOQProductEntryList.batchEditApi.GetColumnIndex('BudgetedPrice');
    gridBOQProductEntryList.batchEditApi.StartEdit(globalrowindex, PrdBudgPricIndex);
    gridBOQProductEntryList.GetEditor('BudgetedPrice').SetText("0.00");

    gridBOQProductEntryList.batchEditApi.StartEdit(globalrowindex, PrdProdIdIndex);
    gridBOQProductEntryList.GetEditor('ProductId').SetText(ProductID);

    gridBOQProductEntryList.batchEditApi.StartEdit(globalrowindex, PrdChargesIndex);
    gridBOQProductEntryList.GetEditor('Charges').SetText("0.00");

    gridBOQProductEntryList.batchEditApi.StartEdit(globalrowindex, PrdProdNameIndex);
    gridBOQProductEntryList.GetEditor('ProductName').SetText(ProductName);
    //gridBOQProductEntryList.GetDataRow(globalrowindex).children[1].innerHTML = ProductName;

    //$('#' + gridtxtbox + '_txtbox').val(ProductName);
    $("#ddl_AmountAre").prop('disabled', 'disabled');
    $('#GridProductlistModel').modal('hide');
    gridBOQProductEntryList.batchEditApi.StartEdit(globalrowindex, PrdProdDescIndex);
    gridBOQProductEntryList.GetEditor('ProductDescription').SetText(data[6]);
    gridBOQProductEntryList.batchEditApi.StartEdit(globalrowindex, PrdProdUOMIndex);

    gridBOQProductEntryList.GetEditor('ProductUOM').SetText(data[1]);
    //$('#' + gridtxtbox + '_txtDescription').val(data[2]);
    //$('#' + gridtxtbox + '_txtStockUOM').val(data[1]);
    gridBOQProductEntryList.batchEditApi.StartEdit(globalrowindex, PrdPriceIndex);
    gridBOQProductEntryList.GetEditor('Price').SetText(data[3]);

    //gridBOQProductEntryList.batchEditApi.StartEdit(globalrowindex, 16);
    //gridBOQProductEntryList.GetEditor('ProductsWarehouseID').SetText(data[4]);

    //$('#' + gridtxtbox + '_txtPrice').val(data[3]);

    //gridBOQProductEntryList.batchEditApi.StartEdit(globalrowindex, 6);
    //gridBOQProductEntryList.GetEditor('Warehouse').SetText(data[5]);

    //gridBOQProductEntryList.batchEditApi.StartEdit(globalrowindex, PrdProdQtyIndex);

    gridBOQProductEntryList.batchEditApi.StartEdit(globalrowindex, PrdProdDescIndex);


    //btnFinishedItem.SetText(ProductName);
    //document.getElementById('hdnProductID').value = Id;


}
}

function BOQProdGridSetTotalAmount(s, e) {
    //debugger;
    var TypId = gridBOQProductEntryList.GetEditor("TaxTypeID").GetText();
    var Price = gridBOQProductEntryList.batchEditApi.GetCellValue(globalrowindex, 'Price');
    var Qty = gridBOQProductEntryList.batchEditApi.GetCellValue(globalrowindex, 'ProductQty');
    var Discount = gridBOQProductEntryList.batchEditApi.GetCellValue(globalrowindex, 'Discount');
    if (Price != "" && Qty != "") {

        var amount = parseFloat((parseFloat(Qty).toFixed(4)) * (parseFloat(Price).toFixed(2))).toFixed(2);

        var amountAfterDiscount = parseFloat(amount) - ((parseFloat(Discount) * parseFloat(amount)) / 100);

        var Tamount = parseFloat(DecimalRoundoff(amountAfterDiscount, 2)).toFixed(2);


        var taxType = "O";
        var amtFor = $("#ddl_AmountAre").val();
        if (amtFor == "2") {
            taxType = "I";
        }
        else if (amtFor == "1") {
            taxType = "E";
        }

        var ret = caluculateAndSetGST(gridBOQProductEntryList.GetEditor("Amount"), gridBOQProductEntryList.GetEditor("Charges"), gridBOQProductEntryList.GetEditor("NetAmount"),
                gridBOQProductEntryList.GetEditor("ProdHSN").GetText(), DecimalRoundoff(amount, 2), DecimalRoundoff(amountAfterDiscount, 2), taxType, "15", $('#ddlBankBranch').val())
        var data = ret.split('~');


        if (taxType == "O") {
            var Netamind = gridBOQProductEntryList.batchEditApi.GetColumnIndex('NetAmount');
            gridBOQProductEntryList.batchEditApi.StartEdit(globalrowindex, Netamind);
            gridBOQProductEntryList.GetEditor('NetAmount').SetText(Tamount);

            var Charges = gridBOQProductEntryList.batchEditApi.GetColumnIndex('Charges');
            gridBOQProductEntryList.batchEditApi.StartEdit(globalrowindex, Charges);
            gridBOQProductEntryList.GetEditor('Charges').SetText("0.00");

            var amind = gridBOQProductEntryList.batchEditApi.GetColumnIndex('Amount');
            gridBOQProductEntryList.batchEditApi.StartEdit(globalrowindex, amind);
            gridBOQProductEntryList.GetEditor('Amount').SetText(Tamount);
        }
        else {
            if (TypId == "3" || TypId == "") {
                var Netamind = gridBOQProductEntryList.batchEditApi.GetColumnIndex('NetAmount');
                gridBOQProductEntryList.batchEditApi.StartEdit(globalrowindex, Netamind);
                gridBOQProductEntryList.GetEditor('NetAmount').SetText(Tamount);

                var Charges = gridBOQProductEntryList.batchEditApi.GetColumnIndex('Charges');
                gridBOQProductEntryList.batchEditApi.StartEdit(globalrowindex, Charges);
                gridBOQProductEntryList.GetEditor('Charges').SetText("0.00");

                var amind = gridBOQProductEntryList.batchEditApi.GetColumnIndex('Amount');
                gridBOQProductEntryList.batchEditApi.StartEdit(globalrowindex, amind);
                gridBOQProductEntryList.GetEditor('Amount').SetText(Tamount);
            }
            else {
                var Netamind = gridBOQProductEntryList.batchEditApi.GetColumnIndex('NetAmount');
                gridBOQProductEntryList.batchEditApi.StartEdit(globalrowindex, Netamind);
                gridBOQProductEntryList.GetEditor('NetAmount').SetText(data[2]);

                var Charges = gridBOQProductEntryList.batchEditApi.GetColumnIndex('Charges');
                gridBOQProductEntryList.batchEditApi.StartEdit(globalrowindex, Charges);
                gridBOQProductEntryList.GetEditor('Charges').SetText(data[1].toString());

                var amind = gridBOQProductEntryList.batchEditApi.GetColumnIndex('Amount');
                gridBOQProductEntryList.batchEditApi.StartEdit(globalrowindex, amind);
                gridBOQProductEntryList.GetEditor('Amount').SetText(data[0]);
            }
        }
        //var TaxType = gridBOQProductEntryList.batchEditApi.GetColumnIndex('TaxType');
        //gridBOQProductEntryList.batchEditApi.StartEdit(globalrowindex, TaxType);

        gridBOQProductEntryList.batchEditApi.EndEdit();

        //var Dis = gridBOQProductEntryList.batchEditApi.GetColumnIndex('Discount');
        //gridBOQProductEntryList.batchEditApi.StartEdit(globalrowindex, Dis);
        //gridBOQProductEntryList.GetEditor('Discount').SetText("0.00");
    }
    var ToTalAmount = 0;
    for (var i = 500; i > -500; i--) {
        if (gridBOQProductEntryList.GetRow(i)) {
            var Amountval = gridBOQProductEntryList.batchEditApi.GetCellValue(i, 'NetAmount');
            if (Amountval != "" && Amountval != null && Amountval != undefined) {
                var ToTalAmount = parseFloat(parseFloat(ToTalAmount) + parseFloat(Amountval)).toFixed(2);
                $('#txtGridProductEntryTotalAmount').val(ToTalAmount);
            }
        }
    }
}

function ProdTotalamt() {
    var ToTalAmount = 0;
    for (var i = 500; i > -500; i--) {
        if (gridBOQProductEntryList.GetRow(i)) {
            var Amountval = gridBOQProductEntryList.batchEditApi.GetCellValue(i, 'NetAmount');
            if (Amountval != "" && Amountval != null && Amountval != undefined) {
                var ToTalAmount = parseFloat(parseFloat(ToTalAmount) + parseFloat(Amountval)).toFixed(2);
                $('#txtGridProductEntryTotalAmount').val(ToTalAmount);
            }
        }
    }



    //var ToTalAmount = $('#txtGridProductEntryTotalAmount').val();
    //var Amountval = gridBOQProductEntryList.batchEditApi.GetCellValue(globalrowindex, 'Amount');
    //if (ToTalAmount != "" || ToTalAmount != undefined || ToTalAmount != null) {
    //    ToTalAmount = parseFloat(0).toFixed(2);
    //}
    //if (Amountval != "" && Amountval != null && Amountval != undefined) {
    //    var calTotalAmount = parseFloat(parseFloat(ToTalAmount) + parseFloat(Amountval)).toFixed(2);
    //    $('#txtGridProductEntryTotalAmount').val(calTotalAmount);
    //}

}

function BOQGridResourceSetTotalAmount(s, e) {

    var ToTalAmount = 0;
    for (var i = 500; i > -500; i--) {
        if (gridBOQResourcesList.GetRow(i)) {
            var Amountval = gridBOQResourcesList.batchEditApi.GetCellValue(i, 'NetAmount');
            if (Amountval != "" && Amountval != null && Amountval != undefined) {
                var ToTalAmount = parseFloat(parseFloat(ToTalAmount) + parseFloat(Amountval)).toFixed(2);
                $('#txtGridResourcesTotalAmount').val(ToTalAmount);
            }
        }
    }


    //var ToTalAmount = $('#txtGridResourcesTotalAmount').val();
    //var Amountval = gridBOQResourcesList.batchEditApi.GetCellValue(globalrowindex2, 'Amount');
    //if (ToTalAmount != "" || ToTalAmount != undefined || ToTalAmount != null) {
    //    ToTalAmount = parseFloat(0).toFixed(2);
    //}
    //if (Amountval != "" && Amountval != null && Amountval != undefined) {
    //    var calTotalAmount = parseFloat(parseFloat(ToTalAmount) + parseFloat(Amountval)).toFixed(2);
    //    $('#txtGridResourcesTotalAmount').val(calTotalAmount);
    //}
}

function SetGridEstimateProduct(Id, Name, e) {
    //debugger;
    if (Id != "") {
        var data = Id.split('|');
        var Details_ID = data[0];
        var Production_ID = data[1];
        var Estimate_No = data[2];
        var REV_No = data[3];
        var Estimate_Date = data[4];
        var Rate = data[5];

        gridBOQProductEntryList.batchEditApi.StartEdit(globalrowindex, PrdTaxTypeIndex);
        //gridBOQProductEntryList.GetEditor('Charges').SetText(Estimate_No);

        // gridBOQProductEntryList.batchEditApi.StartEdit(globalrowindex, 10);
        //gridBOQProductEntryList.GetEditor('RevNo').SetText(REV_No);

        //  gridBOQProductEntryList.batchEditApi.StartEdit(globalrowindex, 11);
        //gridBOQProductEntryList.GetEditor('RevDate').SetText(Estimate_Date);

        gridBOQProductEntryList.batchEditApi.StartEdit(globalrowindex, PrdPriceIndex);
        gridBOQProductEntryList.GetEditor('Price').SetText(Rate);

        gridBOQProductEntryList.batchEditApi.StartEdit(globalrowindex, PrdTagDetlsIDIndex);
        gridBOQProductEntryList.GetEditor('Tag_Details_ID').SetText(Details_ID);

        gridBOQProductEntryList.batchEditApi.StartEdit(globalrowindex, PrdTagProdnIDIndex);
        gridBOQProductEntryList.GetEditor('Tag_Production_ID').SetText(Production_ID);

        BOQGridSetAmount("", "");

        setTimeout(function () {
            gridBOQProductEntryList.batchEditApi.EndEdit();
            gridBOQProductEntryList.batchEditApi.StartEdit();
            // BOQProdGridSetTotalAmount("", "");
        }, 1000);

    }
    $('#GridEstimatelistModel').modal('hide');

}

//function OnInit(s, e) {
//   //debugger;
//    //var grid = MVCxClientGridView.Cast(s);
//    //grid.batchEditApi.ValidateRows();
//}

function OnGridViewEndCallback(s, e) {
    //debugger;
    //if (gridBOQResourcesList.batchEditApi.HasChanges()) {
    //    gridBOQResourcesList.UpdateEdit();
    //}
}

function OnResourcesEndCallback() {
    //debugger;
    AddNewRowGridResources();

    $('#BOQNo').val('');
    var BOQDate = GetServerDateFormat(BOQDate_dt.GetValue());
    //var BOQDate = $('#BOQDate_dt').val();
    $('#hdnFinishedItem').val('');
    $('#FinishedQty').val(parseFloat(0).toFixed(4));
    $('#FinishedUom').val('');
    //$('#slcBOQtype').val($("#slcBOQtype option:first").val());
    $('#RevisionNo').val('');
    $('#ddlBankBranch').val($("#ddlBankBranch option:first").val());
    //$('#ddlWarehouse').val($("#ddlWarehouse option:first").val());
    $('#hdnSchemaId').val('');
    $('#txtActualAdditionalCost').val(parseFloat(0).toFixed(4));
    $('#txtGridProductEntryTotalAmount').val(parseFloat(0).toFixed(2));
    $('#txtGridResourcesTotalAmount').val(parseFloat(0).toFixed(2));


    $('#ddlSchema').val($("#ddlSchema option:first").val());
    $('#BOQNo').val('');
    //$('#EbtnFinishedItem').hide();
    $('#EBOQNo').hide();
    $('#EBOQDate_dt').hide();
    // $('#EbtnFinishedItem').hide();
    $('#EFinishedQty').hide();
    $('#EFinishedQty').hide();
    $('#ERevisionNo').hide();
    $('#ERevisionDate_dt').hide();
    $('#EddlBankBranch').hide();
    // btnFinishedItem.Clear();
    $('#FinishedUom').val('');
    $('#hdnDetailsID').val(0);

    $('#hdnProposal_ID').val('');
    $('#hdnQuotation_ID').val('');
    $('#txt_HeadRemarks').val('');
    $('#btnQuotation').val('');
    $('#btnProposal').val('');
    $("#txt_ApproveRemarks").val('');
    Scheme_ValueChange();
    debugger;
    if (Message == "duplicate" && DetailsID == 0 && ProductionID == 0) {
        savemode = "";
        if (Message == "duplicate") {
            jAlert('This Bill of Quantities no already present!');
            return false;
            LoadingPanel.Hide();
        }
        else {
            jAlert('Please try again later.');
            return false;
            LoadingPanel.Hide();
        }
        Message = "";

    }
    else {
        if (DetailsID > 0 && ProductionID > 0 && GBOQNo != "") {
            ProductionID = 0;
            DetailsID = 0;
            var msg = $("#hdnApproveReject").val();
            if (msg == "Approve") {
                jAlert('BOQ Number : ' + GBOQNo + ' approve successfully.', 'Alert!', function (r) {
                    if (r) {
                        if (savemode == "Exit") {
                            setTimeout(function () {
                                var url = $('#hdnBOQListPage').val();
                                window.location.href = url;
                            }, 500);
                        }
                    }

                });
            }
            else if (msg == "Reject") {
                jAlert('BOQ Number : ' + GBOQNo + ' reject successfully.', 'Alert!', function (r) {
                    if (r) {
                        if (savemode == "Exit") {
                            setTimeout(function () {
                                var url = $('#hdnBOQListPage').val();
                                window.location.href = url;
                            }, 500);
                        }
                    }
                });
            }
            else {
                jAlert('BOQ Number : ' + GBOQNo + ' saved successfully.', 'Alert!', function (r) {
                    if (r) {
                        if (savemode == "Exit") {
                            setTimeout(function () {
                                var url = $('#hdnBOQListPage').val();
                                window.location.href = url;
                            }, 500);
                        }
                    }
                });
            }
            LoadingPanel.Hide();
            // jAlert('Estimate Number : ' + GBOQNo + ' Successfully saved.');


        }
        else {
            ProductionID = 0;
            DetailsID = 0;
            savemode = "";
            jAlert('Please try again later.');
            return false;
            LoadingPanel.Hide();
        }
        Message = "";
    }

}

function OnEndCallback(s, e) {
    debugger;
    DetailsID = s.cpDetailsID;
    ProductionID = s.cpProductionID;
    GBOQNo = s.cpBOQNo;
    Message = s.cpMessage;
    $('#hdnDetailsID').val(DetailsID);
    if (s.cpBatchUpdate == "1") {

        s.cpBatchUpdate = "0";

        //if (gridBOQResourcesList.batchEditApi.HasChanges()) {
        //    gridBOQResourcesList.UpdateEdit();

        setTimeout(function () {
            OnResourcesEndCallback();
        }, 1500);

        //}
    }
    //else {
    AddNewRowWithSl();
    $('#txtGridProductEntryTotalAmount').val(parseFloat(0).toFixed(2));
    if (Message == "duplicate" && hasmsg == 0) {
        jAlert('This BOQ no already present!');
        hasmsg = 1;
        return false;
    }
    //else {
    //    jAlert('Please try again later.');
    //    return false;
    //}
    //}

}

function ProductListkeydown(e) {
    if (e.code == "Enter" || e.code == "NumpadEnter") {
        if ($("#txtProductName").val() != '') {
            Productlist($("#txtProductName").val(), null);
        }
    }
    else if (e.code == "ArrowDown") {
        if ($("input[ProductIndex=0]"))
            $("input[ProductIndex=0]").focus();
    }
}

var iindexprod = 0;
var finishedproductlist = 0;
var gridproductlist = 0;
var gridnonproductlist = 0;
function GridProductListkeydown(e) {

    if (e.code == "Enter" || e.code == "NumpadEnter") {
        if ($("#txtGridProductName").val().trim() != '') {
            if (typemodal == 'nonInventory') {
                GridNonInventoryProductlist($("#txtGridProductName").val(), "nonInventory", globalrowindex2);
                gridnonproductlist = 1;
            }
            else {
                GridProductlist($("#txtGridProductName").val(), typemodal, null);
                gridproductlist = 1;
            }
        }
        else {
            var txt = "<table  class='dynamicPopupTbl' width=\"100%\"><tr class=\"HeaderStyle\"><th class='hide'>id</th><th>Product Code</th><th>Product Name</th><th>Inventory</th><th>HSN/SAC</th><th>Class</th><th>Brand</th></tr><table>";
            $("#txtGridProductName").val("");
            document.getElementById("GridProductTable").innerHTML = txt;
        }
    }
    else if (e.code == "ArrowDown") {
        if ($("input[GridProductIndex=" + 0 + "]")) {
            $("input[GridProductIndex=" + 0 + "]").focus();
            iindexprod++;
        }
        if ($("input[NonIProductIndex=" + 0 + "]")) {
            $("input[NonIProductIndex=" + 0 + "]").focus();
            iindexprod++;
        }
    }
}

function GridEstimateListkeydown() {

}

$(function () {
    PopulateWareHouseData();
    PopulateAmountForData();
    PopulateNumberingSchemeData();

    $(document).keyup(function (e) {
        if (e.key === "Escape") {

            if (finishedproductlist == 1) {
                finishedproductlist = 0;
                setTimeout(function () {
                    $('#FinishedQty').select();
                    $('#FinishedQty').focus();
                }, 500);

            }

            if (warehousefocus == 1) {
                warehousefocus = 0;
                $('#GridWarehouselistModel').modal('hide');
                setTimeout(function () {
                    var localcolumn = gridBOQProductEntryList.batchEditApi.GetColumnIndex('Price');
                    gridBOQProductEntryList.batchEditApi.StartEdit(globalrowindex, localcolumn);
                }, 500);

            }


            if (Estimatelinkindex == 1) {
                Estimatelinkindex = 0;
                setTimeout(function () {
                    var localcolumn = gridBOQProductEntryList.batchEditApi.GetColumnIndex('Remarks');
                    gridBOQProductEntryList.batchEditApi.StartEdit(globalrowindex, localcolumn);
                }, 500);

            }

            setTimeout(function () {
                var localcolumn = gridBOQProductEntryList.batchEditApi.GetColumnIndex('Discount');
                gridBOQProductEntryList.batchEditApi.StartEdit(globalrowindex, localcolumn);
            }, 500);

            if (gridproductlist == 1) {
                gridproductlist = 0;
                setTimeout(function () {
                    var localcolumn = gridBOQProductEntryList.batchEditApi.GetColumnIndex('ProductQty');
                    gridBOQProductEntryList.batchEditApi.StartEdit(globalrowindex, localcolumn);
                }, 500);

            }

            if (gridnonproductlist == 1) {
                gridnonproductlist = 0;
                setTimeout(function () {
                    var localcolumn = gridBOQResourcesList.batchEditApi.GetColumnIndex('ProductQty');
                    gridBOQResourcesList.batchEditApi.StartEdit(globalrowindex2, localcolumn);
                }, 500);

            }
        }
        //alert(e.keyCode + "|" + e.altKey);

    });


});




function PopulateWareHouseData() {
    var BankBranchID = $('#ddlBankBranch option:selected').val();
    $.ajax({
        type: "POST",
        url: "@Url.Action("getWarehouseRecord", "BillofQuantities")",
        data: { branchid: BankBranchID },
    success: function (response) {
        //$('#ddlWarehouse').html('');
        var html = "";
        var hdnProductWarehouseID = $('#hdnProductWarehouseID').val();
        for (var i = 0; i < response.length; i++) {
            if (hdnProductWarehouseID > 0) {
                if (hdnProductWarehouseID == response[i].WarehouseID) {
                    html = html + "<option value='" + response[i].WarehouseID + "' selected>" + response[i].WarehouseName + "</option>";
                }
                else {
                    html = html + "<option value='" + response[i].WarehouseID + "'>" + response[i].WarehouseName + "</option>";
                }
            }
            else {
                html = html + "<option value='" + response[i].WarehouseID + "'>" + response[i].WarehouseName + "</option>";
            }

        }
        $('#ddlWarehouse').html(html);

    }
});
}

function PopulateNumberingSchemeData() {
    var type = "";// $('#slcEstimatetype option:selected').val();
    $.ajax({
        type: "POST",
        url: "@Url.Action("getNumberingSchemeRecord", "BillofQuantities")",
        data: { type: type },
    success: function (response) {
        var html = "";
        var hdnBOQ_SCHEMAID = $('#hdnBOQ_SCHEMAID').val();
        for (var i = 0; i < response.length; i++) {
            if (hdnBOQ_SCHEMAID != '') {
                html = html + "<option value='" + response[i].SchemaID + "' selected>" + response[i].SchemaName + "</option>";
            }
            else {
                html = html + "<option value='" + response[i].SchemaID + "'>" + response[i].SchemaName + "</option>";
            }

        }
        $('#ddlSchema').html(html);


        //$("#ddlSchema > option").each(function () {
        //    var str = this.value;
        //    var n = str.startsWith("1056");
        //    alert(this.value);
        //});
    }
});
}

function PopulateAmountForData() {
    var type = "";// $('#slcEstimatetype option:selected').val();
    $.ajax({
        type: "POST",
        url: "@Url.Action("getAmountForRecord", "BillofQuantities")",
        data: { type: type },
    success: function (response) {
        var html = "";
        var hdnBOQ_SCHEMAID = $('#hdnBOQ_SCHEMAID').val();
        for (var i = 0; i < response.length; i++) {

            html = html + "<option value='" + response[i].taxGrp_Id + "'>" + response[i].taxGrp_Description + "</option>";
        }
        $('#ddl_AmountAre').html(html);
        $('#ddl_AmountAre').val('@Model.TaxID');
    }
});


}


function addNewRowTogridResources() {
    gridBOQResourcesList.batchEditApi.EndEdit();
    AddNewRowGridResources();
    index = globalrowindex2;
    setTimeout(function () {
        gridBOQResourcesList.batchEditApi.EndEdit();
        gridBOQResourcesList.batchEditApi.StartEdit(index, ResSrlIndex);
    }, 200);

}

function addNewRowTogrid() {
    gridBOQProductEntryList.batchEditApi.EndEdit();
    //gridBOQProductEntryList.AddNewRow();
    // gridBOQProductEntryList.AddNewRow();
    //

    AddNewRowWithSl();
    index = globalrowindex;


    setTimeout(function () {
        gridBOQProductEntryList.batchEditApi.EndEdit();
        gridBOQProductEntryList.batchEditApi.StartEdit(index, PrdSrlIndex);
    }, 200);

}

function BOQProdGridSetDiscount(s, e) {
    // debugger;
    // gridBOQProductEntryList.batchEditApi.EndEdit();
    //var Price = s.GetValueString();


    //var TypId = gridBOQProductEntryList.batchEditApi.GetCellValue(globalrowindex, 'TaxTypeID');
    var TypId = gridBOQProductEntryList.GetEditor("TaxTypeID").GetText();
    var Price = gridBOQProductEntryList.batchEditApi.GetCellValue(globalrowindex, 'Price');
    var Qty = gridBOQProductEntryList.batchEditApi.GetCellValue(globalrowindex, 'ProductQty');
    var Discount = gridBOQProductEntryList.batchEditApi.GetCellValue(globalrowindex, 'Discount');
    if (Price != "" && Qty != "") {

        var amount = parseFloat((parseFloat(Qty).toFixed(4)) * (parseFloat(Price).toFixed(2))).toFixed(2);

        var amountAfterDiscount = parseFloat(amount) - ((parseFloat(Discount) * parseFloat(amount)) / 100);

        var Tamount = parseFloat(DecimalRoundoff(amountAfterDiscount, 2)).toFixed(2);


        var taxType = "O";
        var amtFor = $("#ddl_AmountAre").val();
        if (amtFor == "2") {
            taxType = "I";
        }
        else if (amtFor == "1") {
            taxType = "E";
        }

        var ret = caluculateAndSetGST(gridBOQProductEntryList.GetEditor("Amount"), gridBOQProductEntryList.GetEditor("Charges"), gridBOQProductEntryList.GetEditor("NetAmount"),
                gridBOQProductEntryList.GetEditor("ProdHSN").GetText(), DecimalRoundoff(amount, 2), DecimalRoundoff(amountAfterDiscount, 2), taxType, "15", $('#ddlBankBranch').val())
        var data = ret.split('~');


        if (taxType == "O") {
            var Netamind = gridBOQProductEntryList.batchEditApi.GetColumnIndex('NetAmount');
            gridBOQProductEntryList.batchEditApi.StartEdit(globalrowindex, Netamind);
            gridBOQProductEntryList.GetEditor('NetAmount').SetText(Tamount);

            var Charges = gridBOQProductEntryList.batchEditApi.GetColumnIndex('Charges');
            gridBOQProductEntryList.batchEditApi.StartEdit(globalrowindex, Charges);
            gridBOQProductEntryList.GetEditor('Charges').SetText("0.00");

            var amind = gridBOQProductEntryList.batchEditApi.GetColumnIndex('Amount');
            gridBOQProductEntryList.batchEditApi.StartEdit(globalrowindex, amind);
            gridBOQProductEntryList.GetEditor('Amount').SetText(Tamount);
        }
        else {
            if (TypId == "3" || TypId == "") {
                var Netamind = gridBOQProductEntryList.batchEditApi.GetColumnIndex('NetAmount');
                gridBOQProductEntryList.batchEditApi.StartEdit(globalrowindex, Netamind);
                gridBOQProductEntryList.GetEditor('NetAmount').SetText(Tamount);

                var Charges = gridBOQProductEntryList.batchEditApi.GetColumnIndex('Charges');
                gridBOQProductEntryList.batchEditApi.StartEdit(globalrowindex, Charges);
                gridBOQProductEntryList.GetEditor('Charges').SetText("0.00");

                var amind = gridBOQProductEntryList.batchEditApi.GetColumnIndex('Amount');
                gridBOQProductEntryList.batchEditApi.StartEdit(globalrowindex, amind);
                gridBOQProductEntryList.GetEditor('Amount').SetText(Tamount);
            }
            else {
                var Netamind = gridBOQProductEntryList.batchEditApi.GetColumnIndex('NetAmount');
                gridBOQProductEntryList.batchEditApi.StartEdit(globalrowindex, Netamind);
                gridBOQProductEntryList.GetEditor('NetAmount').SetText(data[2]);

                var Charges = gridBOQProductEntryList.batchEditApi.GetColumnIndex('Charges');
                gridBOQProductEntryList.batchEditApi.StartEdit(globalrowindex, Charges);
                gridBOQProductEntryList.GetEditor('Charges').SetText(data[1].toString());

                var amind = gridBOQProductEntryList.batchEditApi.GetColumnIndex('Amount');
                gridBOQProductEntryList.batchEditApi.StartEdit(globalrowindex, amind);
                gridBOQProductEntryList.GetEditor('Amount').SetText(data[0]);
            }
        }

        //  gridBOQProductEntryList.batchEditApi.StartEdit(globalrowindex, 9);
        // gridBOQProductEntryList.GetEditor('Amount').SetText(Tamount);
    }
    // BOQProdGridSetTotalAmount("", "");
    ProdTotalamt();
    gridBOQProductEntryList.batchEditApi.EndEdit();
}


function BOQGridSetAmount(s, e) {
    //debugger;
    // gridBOQProductEntryList.batchEditApi.EndEdit();
    //var Price = s.GetValueString();

    var TypId = gridBOQProductEntryList.GetEditor("TaxTypeID").GetText();
    var Price = gridBOQProductEntryList.batchEditApi.GetCellValue(globalrowindex, 'Price');
    var Qty = gridBOQProductEntryList.batchEditApi.GetCellValue(globalrowindex, 'ProductQty');
    var Discount = gridBOQProductEntryList.batchEditApi.GetCellValue(globalrowindex, 'Discount');
    if (Price != "" && Qty != "") {

        var amount = parseFloat((parseFloat(Qty).toFixed(4)) * (parseFloat(Price).toFixed(2))).toFixed(2);

        var amountAfterDiscount = parseFloat(amount) - ((parseFloat(Discount) * parseFloat(amount)) / 100);

        var Tamount = parseFloat(DecimalRoundoff(amountAfterDiscount, 2)).toFixed(2);


        var taxType = "O";
        var amtFor = $("#ddl_AmountAre").val();
        if (amtFor == "2") {
            taxType = "I";
        }
        else if (amtFor == "1") {
            taxType = "E";
        }

        var ret = caluculateAndSetGST(gridBOQProductEntryList.GetEditor("Amount"), gridBOQProductEntryList.GetEditor("Charges"), gridBOQProductEntryList.GetEditor("NetAmount"),
                gridBOQProductEntryList.GetEditor("ProdHSN").GetText(), DecimalRoundoff(amount, 2), DecimalRoundoff(amountAfterDiscount, 2), taxType, "15", $('#ddlBankBranch').val())
        var data = ret.split('~');


        if (taxType == "O") {
            var Netamind = gridBOQProductEntryList.batchEditApi.GetColumnIndex('NetAmount');
            gridBOQProductEntryList.batchEditApi.StartEdit(globalrowindex, Netamind);
            gridBOQProductEntryList.GetEditor('NetAmount').SetText(Tamount);

            var Charges = gridBOQProductEntryList.batchEditApi.GetColumnIndex('Charges');
            gridBOQProductEntryList.batchEditApi.StartEdit(globalrowindex, Charges);
            gridBOQProductEntryList.GetEditor('Charges').SetText("0.00");

            var amind = gridBOQProductEntryList.batchEditApi.GetColumnIndex('Amount');
            gridBOQProductEntryList.batchEditApi.StartEdit(globalrowindex, amind);
            gridBOQProductEntryList.GetEditor('Amount').SetText(Tamount);
        }
        else {
            if (TypId == "3" || TypId == "") {
                var Netamind = gridBOQProductEntryList.batchEditApi.GetColumnIndex('NetAmount');
                gridBOQProductEntryList.batchEditApi.StartEdit(globalrowindex, Netamind);
                gridBOQProductEntryList.GetEditor('NetAmount').SetText(Tamount);

                var Charges = gridBOQProductEntryList.batchEditApi.GetColumnIndex('Charges');
                gridBOQProductEntryList.batchEditApi.StartEdit(globalrowindex, Charges);
                gridBOQProductEntryList.GetEditor('Charges').SetText("0.00");

                var amind = gridBOQProductEntryList.batchEditApi.GetColumnIndex('Amount');
                gridBOQProductEntryList.batchEditApi.StartEdit(globalrowindex, amind);
                gridBOQProductEntryList.GetEditor('Amount').SetText(Tamount);
            }
            else {
                var Netamind = gridBOQProductEntryList.batchEditApi.GetColumnIndex('NetAmount');
                gridBOQProductEntryList.batchEditApi.StartEdit(globalrowindex, Netamind);
                gridBOQProductEntryList.GetEditor('NetAmount').SetText(data[2]);

                var Charges = gridBOQProductEntryList.batchEditApi.GetColumnIndex('Charges');
                gridBOQProductEntryList.batchEditApi.StartEdit(globalrowindex, Charges);
                gridBOQProductEntryList.GetEditor('Charges').SetText(data[1].toString());

                var amind = gridBOQProductEntryList.batchEditApi.GetColumnIndex('Amount');
                gridBOQProductEntryList.batchEditApi.StartEdit(globalrowindex, amind);
                gridBOQProductEntryList.GetEditor('Amount').SetText(data[0]);
            }
        }
        var Discount = gridBOQProductEntryList.batchEditApi.GetColumnIndex('Discount');
        gridBOQProductEntryList.batchEditApi.StartEdit(globalrowindex, Discount);


        gridBOQProductEntryList.GetEditor('Discount').SetText("0.00");
    }

    ProdTotalamt();
}

function EstimateGridSetAmt(s, e) {

    var TypId = gridBOQProductEntryList.GetEditor("TaxTypeID").GetText();
    var Price = gridBOQProductEntryList.batchEditApi.GetCellValue(globalrowindex, 'Price');
    var Qty = gridBOQProductEntryList.batchEditApi.GetCellValue(globalrowindex, 'ProductQty');
    var Discount = gridBOQProductEntryList.batchEditApi.GetCellValue(globalrowindex, 'Discount');
    if (Price != "" && Qty != "") {

        var amount = parseFloat((parseFloat(Qty).toFixed(4)) * (parseFloat(Price).toFixed(2))).toFixed(2);

        var amountAfterDiscount = parseFloat(amount) - ((parseFloat(Discount) * parseFloat(amount)) / 100);

        var Tamount = parseFloat(DecimalRoundoff(amountAfterDiscount, 2)).toFixed(2);


        var taxType = "O";
        var amtFor = $("#ddl_AmountAre").val();
        if (amtFor == "2") {
            taxType = "I";
        }
        else if (amtFor == "1") {
            taxType = "E";
        }

        var ret = caluculateAndSetGST(gridBOQProductEntryList.GetEditor("Amount"), gridBOQProductEntryList.GetEditor("Charges"), gridBOQProductEntryList.GetEditor("NetAmount"),
                gridBOQProductEntryList.GetEditor("ProdHSN").GetText(), DecimalRoundoff(amount, 2), DecimalRoundoff(amountAfterDiscount, 2), taxType, "15", $('#ddlBankBranch').val())
        var data = ret.split('~');


        if (taxType == "O") {
            var Netamind = gridBOQProductEntryList.batchEditApi.GetColumnIndex('NetAmount');
            gridBOQProductEntryList.batchEditApi.StartEdit(globalrowindex, Netamind);
            gridBOQProductEntryList.GetEditor('NetAmount').SetText(Tamount);

            var Charges = gridBOQProductEntryList.batchEditApi.GetColumnIndex('Charges');
            gridBOQProductEntryList.batchEditApi.StartEdit(globalrowindex, Charges);
            gridBOQProductEntryList.GetEditor('Charges').SetText("0.00");

            var amind = gridBOQProductEntryList.batchEditApi.GetColumnIndex('Amount');
            gridBOQProductEntryList.batchEditApi.StartEdit(globalrowindex, amind);
            gridBOQProductEntryList.GetEditor('Amount').SetText(Tamount);
        }
        else {
            if (TypId == "3" || TypId == "") {
                var Netamind = gridBOQProductEntryList.batchEditApi.GetColumnIndex('NetAmount');
                gridBOQProductEntryList.batchEditApi.StartEdit(globalrowindex, Netamind);
                gridBOQProductEntryList.GetEditor('NetAmount').SetText(Tamount);

                var Charges = gridBOQProductEntryList.batchEditApi.GetColumnIndex('Charges');
                gridBOQProductEntryList.batchEditApi.StartEdit(globalrowindex, Charges);
                gridBOQProductEntryList.GetEditor('Charges').SetText("0.00");

                var amind = gridBOQProductEntryList.batchEditApi.GetColumnIndex('Amount');
                gridBOQProductEntryList.batchEditApi.StartEdit(globalrowindex, amind);
                gridBOQProductEntryList.GetEditor('Amount').SetText(Tamount);
            }
            else {
                var Netamind = gridBOQProductEntryList.batchEditApi.GetColumnIndex('NetAmount');
                gridBOQProductEntryList.batchEditApi.StartEdit(globalrowindex, Netamind);
                gridBOQProductEntryList.GetEditor('NetAmount').SetText(data[2]);

                var Charges = gridBOQProductEntryList.batchEditApi.GetColumnIndex('Charges');
                gridBOQProductEntryList.batchEditApi.StartEdit(globalrowindex, Charges);
                gridBOQProductEntryList.GetEditor('Charges').SetText(data[1].toString());

                var amind = gridBOQProductEntryList.batchEditApi.GetColumnIndex('Amount');
                gridBOQProductEntryList.batchEditApi.StartEdit(globalrowindex, amind);
                gridBOQProductEntryList.GetEditor('Amount').SetText(data[0]);
            }
        }
        var TaxType = gridBOQProductEntryList.batchEditApi.GetColumnIndex('TaxType');
        gridBOQProductEntryList.batchEditApi.StartEdit(globalrowindex, TaxType);

    }
    ProdTotalamt();
}

function BOQGridSetAmountQty(s, e) {
    //debugger;
    // gridBOQProductEntryList.batchEditApi.EndEdit();
    //var Price = s.GetValueString();
    var TypId = gridBOQProductEntryList.GetEditor("TaxTypeID").GetText();
    var Price = gridBOQProductEntryList.batchEditApi.GetCellValue(globalrowindex, 'Price');
    var Qty = gridBOQProductEntryList.batchEditApi.GetCellValue(globalrowindex, 'ProductQty');
    var balQty = (gridBOQProductEntryList.batchEditApi.GetCellValue(globalrowindex, 'BalQty') != null) ? gridBOQProductEntryList.batchEditApi.GetCellValue(globalrowindex, 'BalQty') : "0";

    var Discount = gridBOQProductEntryList.batchEditApi.GetCellValue(globalrowindex, 'Discount');
    var crntQty = 0;

    if (balQty != 0) {
        crntQty = balQty - Qty;
        if (crntQty < 0) {
            // var OrdeMsg = 'Balance Quantity of selected Product from tagged document is (' + crntQty + '). <br/>Cannot enter quantity more than balance quantity.';
            var OrdeMsg = 'Balance Quantity of selected Product from tagged document. <br/>Cannot enter quantity more than balance quantity.';
            jAlert(OrdeMsg, 'Alert Dialog: [Balace Quantity ]', function (r) {
                // grid.batchEditApi.StartEdit(globalRowIndex, 6);
                //  gridBOQProductEntryList.batchEditApi.EndEdit();
                var localindex = gridBOQProductEntryList.batchEditApi.GetColumnIndex('ProductQty');
                gridBOQProductEntryList.GetEditor('ProductQty').SetText(balQty);
            });
            return false;
        }
    }
    if (Price != "" && Qty != "") {

        var amount = parseFloat((parseFloat(Qty).toFixed(4)) * (parseFloat(Price).toFixed(2))).toFixed(2);

        var amountAfterDiscount = parseFloat(amount) - ((parseFloat(Discount) * parseFloat(amount)) / 100);

        var Tamount = parseFloat(DecimalRoundoff(amountAfterDiscount, 2)).toFixed(2);


        var taxType = "O";
        var amtFor = $("#ddl_AmountAre").val();
        if (amtFor == "2") {
            taxType = "I";
        }
        else if (amtFor == "1") {
            taxType = "E";
        }

        var ret = caluculateAndSetGST(gridBOQProductEntryList.GetEditor("Amount"), gridBOQProductEntryList.GetEditor("Charges"), gridBOQProductEntryList.GetEditor("NetAmount"),
                gridBOQProductEntryList.GetEditor("ProdHSN").GetText(), DecimalRoundoff(amount, 2), DecimalRoundoff(amountAfterDiscount, 2), taxType, "15", $('#ddlBankBranch').val())
        var data = ret.split('~');


        if (taxType == "O") {
            var Netamind = gridBOQProductEntryList.batchEditApi.GetColumnIndex('NetAmount');
            //gridBOQProductEntryList.batchEditApi.StartEdit(globalrowindex, Netamind);
            gridBOQProductEntryList.GetEditor('NetAmount').SetText(Tamount);

            var Charges = gridBOQProductEntryList.batchEditApi.GetColumnIndex('Charges');
            // gridBOQProductEntryList.batchEditApi.StartEdit(globalrowindex, Charges);
            gridBOQProductEntryList.GetEditor('Charges').SetText("0.00");

            var amind = gridBOQProductEntryList.batchEditApi.GetColumnIndex('Amount');
            //gridBOQProductEntryList.batchEditApi.StartEdit(globalrowindex, amind);
            gridBOQProductEntryList.GetEditor('Amount').SetText(Tamount);
        }
        else {
            if (TypId == "3" || TypId == "") {
                var Netamind = gridBOQProductEntryList.batchEditApi.GetColumnIndex('NetAmount');
                // gridBOQProductEntryList.batchEditApi.StartEdit(globalrowindex, Netamind);
                gridBOQProductEntryList.GetEditor('NetAmount').SetText(Tamount);

                var Charges = gridBOQProductEntryList.batchEditApi.GetColumnIndex('Charges');
                // gridBOQProductEntryList.batchEditApi.StartEdit(globalrowindex, Charges);
                gridBOQProductEntryList.GetEditor('Charges').SetText("0.00");

                var amind = gridBOQProductEntryList.batchEditApi.GetColumnIndex('Amount');
                // gridBOQProductEntryList.batchEditApi.StartEdit(globalrowindex, amind);
                gridBOQProductEntryList.GetEditor('Amount').SetText(Tamount);
            }
            else {
                var Netamind = gridBOQProductEntryList.batchEditApi.GetColumnIndex('NetAmount');
                //gridBOQProductEntryList.batchEditApi.StartEdit(globalrowindex, Netamind);
                gridBOQProductEntryList.GetEditor('NetAmount').SetText(data[2]);

                var Charges = gridBOQProductEntryList.batchEditApi.GetColumnIndex('Charges');
                // gridBOQProductEntryList.batchEditApi.StartEdit(globalrowindex, Charges);
                gridBOQProductEntryList.GetEditor('Charges').SetText(data[1].toString());

                var amind = gridBOQProductEntryList.batchEditApi.GetColumnIndex('Amount');
                // gridBOQProductEntryList.batchEditApi.StartEdit(globalrowindex, amind);
                gridBOQProductEntryList.GetEditor('Amount').SetText(data[0]);
            }
        }
    }
    // BOQProdGridSetTotalAmount("", "");
    ProdTotalamt();
    gridBOQProductEntryList.batchEditApi.EndEdit();
    var localindex = gridBOQProductEntryList.batchEditApi.GetColumnIndex('ProductUOM');

    // gridBOQProductEntryList.batchEditApi.StartEdit(globalrowindex, localindex);
    //else {
    //    gridBOQProductEntryList.batchEditApi.StartEdit(globalrowindex, 7);
    //}
    //gridBOQProductEntryList.batchEditApi.EndEdit();
    //gridBOQProductEntryList.batchEditApi.StartEdit();
    //BOQProdGridSetTotalAmount(s, e);

}

//Resource

function BOQResourceGridSetAmount(s, e) {
    //  gridBOQResourcesList.batchEditApi.EndEdit();
    //var Price = s.GetValueString();
    //debugger;
    var TypId = gridBOQResourcesList.GetEditor("TaxTypeID").GetText();
    var Price = gridBOQResourcesList.batchEditApi.GetCellValue(globalrowindex2, 'Price');
    var Qty = gridBOQResourcesList.batchEditApi.GetCellValue(globalrowindex2, 'ProductQty');
    var Discount = gridBOQResourcesList.batchEditApi.GetCellValue(globalrowindex2, 'Discount');
    if (Price != "" && Qty != "") {
        var amount = parseFloat((parseFloat(Qty).toFixed(4)) * (parseFloat(Price).toFixed(2))).toFixed(2);

        var amountAfterDiscount = parseFloat(amount) - ((parseFloat(Discount) * parseFloat(amount)) / 100);
        var Tamount = parseFloat(DecimalRoundoff(amountAfterDiscount, 2)).toFixed(2);

        //gridBOQResourcesList.batchEditApi.StartEdit(globalrowindex2, 12);
        //gridBOQResourcesList.GetEditor('NetAmount').SetText(Tamount);

        //gridBOQResourcesList.batchEditApi.StartEdit(globalrowindex2, 9);
        //gridBOQResourcesList.GetEditor('Amount').SetText(Tamount);

        var taxType = "O";
        var amtFor = $("#ddl_AmountAre").val();
        if (amtFor == "2") {
            taxType = "I";
        }
        else if (amtFor == "1") {
            taxType = "E";
        }

        var ret = caluculateAndSetGST(gridBOQResourcesList.GetEditor("Amount"), gridBOQResourcesList.GetEditor("ResourceCharges"), gridBOQResourcesList.GetEditor("NetAmount"),
                gridBOQResourcesList.GetEditor("ProdHSN").GetText(), DecimalRoundoff(amount, 2), DecimalRoundoff(amountAfterDiscount, 2), taxType, "15", $('#ddlBankBranch').val())
        var data = ret.split('~');


        if (taxType == "O") {
            var Netamind = gridBOQResourcesList.batchEditApi.GetColumnIndex('NetAmount');
            gridBOQResourcesList.batchEditApi.StartEdit(globalrowindex2, Netamind);
            gridBOQResourcesList.GetEditor('NetAmount').SetText(Tamount);

            var Charges = gridBOQResourcesList.batchEditApi.GetColumnIndex('ResourceCharges');
            gridBOQResourcesList.batchEditApi.StartEdit(globalrowindex2, Charges);
            gridBOQResourcesList.GetEditor('ResourceCharges').SetText("0.00");

            var amind = gridBOQResourcesList.batchEditApi.GetColumnIndex('Amount');
            gridBOQResourcesList.batchEditApi.StartEdit(globalrowindex2, amind);
            gridBOQResourcesList.GetEditor('Amount').SetText(Tamount);
        }
        else {
            if (TypId == "3" || TypId == "") {
                var Netamind = gridBOQResourcesList.batchEditApi.GetColumnIndex('NetAmount');
                gridBOQResourcesList.batchEditApi.StartEdit(globalrowindex2, Netamind);
                gridBOQResourcesList.GetEditor('NetAmount').SetText(Tamount);

                var Charges = gridBOQResourcesList.batchEditApi.GetColumnIndex('ResourceCharges');
                gridBOQResourcesList.batchEditApi.StartEdit(globalrowindex2, Charges);
                gridBOQResourcesList.GetEditor('ResourceCharges').SetText("0.00");

                var amind = gridBOQResourcesList.batchEditApi.GetColumnIndex('Amount');
                gridBOQResourcesList.batchEditApi.StartEdit(globalrowindex2, amind);
                gridBOQResourcesList.GetEditor('Amount').SetText(Tamount);
            }
            else {
                var Netamind = gridBOQResourcesList.batchEditApi.GetColumnIndex('NetAmount');
                gridBOQResourcesList.batchEditApi.StartEdit(globalrowindex2, Netamind);
                gridBOQResourcesList.GetEditor('NetAmount').SetText(data[2]);

                var Charges = gridBOQResourcesList.batchEditApi.GetColumnIndex('ResourceCharges');
                gridBOQResourcesList.batchEditApi.StartEdit(globalrowindex2, Charges);
                gridBOQResourcesList.GetEditor('ResourceCharges').SetText(data[1].toString());

                var amind = gridBOQResourcesList.batchEditApi.GetColumnIndex('Amount');
                gridBOQResourcesList.batchEditApi.StartEdit(globalrowindex2, amind);
                gridBOQResourcesList.GetEditor('Amount').SetText(data[0]);
            }
        }
    }
    var Discount = gridBOQResourcesList.batchEditApi.GetColumnIndex('Discount');
    gridBOQResourcesList.batchEditApi.StartEdit(globalrowindex2, Discount);
    //BOQGridResourceSetTotalAmount("", "");
}

function BOQResourceDiscount(s, e) {
    // gridBOQResourcesList.batchEditApi.EndEdit();
    //var Price = s.GetValueString();
    //debugger;
    var TypId = gridBOQResourcesList.GetEditor("TaxTypeID").GetText();
    var Price = gridBOQResourcesList.batchEditApi.GetCellValue(globalrowindex2, 'Price');
    var Qty = gridBOQResourcesList.batchEditApi.GetCellValue(globalrowindex2, 'ProductQty');
    var Discount = gridBOQResourcesList.batchEditApi.GetCellValue(globalrowindex2, 'Discount');
    if (Price != "" && Qty != "") {
        var amount = parseFloat((parseFloat(Qty).toFixed(4)) * (parseFloat(Price).toFixed(2))).toFixed(2);

        var amountAfterDiscount = parseFloat(amount) - ((parseFloat(Discount) * parseFloat(amount)) / 100);
        var Tamount = parseFloat(DecimalRoundoff(amountAfterDiscount, 2)).toFixed(2);

        //gridBOQResourcesList.batchEditApi.StartEdit(globalrowindex2, 12);
        //gridBOQResourcesList.GetEditor('NetAmount').SetText(Tamount);

        //gridBOQResourcesList.batchEditApi.StartEdit(globalrowindex2, 9);
        //gridBOQResourcesList.GetEditor('Amount').SetText(Tamount);

        var taxType = "O";
        var amtFor = $("#ddl_AmountAre").val();
        if (amtFor == "2") {
            taxType = "I";
        }
        else if (amtFor == "1") {
            taxType = "E";
        }

        var ret = caluculateAndSetGST(gridBOQResourcesList.GetEditor("Amount"), gridBOQResourcesList.GetEditor("ResourceCharges"), gridBOQResourcesList.GetEditor("NetAmount"),
                gridBOQResourcesList.GetEditor("ProdHSN").GetText(), DecimalRoundoff(amount, 2), DecimalRoundoff(amountAfterDiscount, 2), taxType, "15", $('#ddlBankBranch').val())
        var data = ret.split('~');


        if (taxType == "O") {
            var Netamind = gridBOQResourcesList.batchEditApi.GetColumnIndex('NetAmount');
            gridBOQResourcesList.batchEditApi.StartEdit(globalrowindex2, Netamind);
            gridBOQResourcesList.GetEditor('NetAmount').SetText(Tamount);

            var Charges = gridBOQResourcesList.batchEditApi.GetColumnIndex('ResourceCharges');
            gridBOQResourcesList.batchEditApi.StartEdit(globalrowindex2, Charges);
            gridBOQResourcesList.GetEditor('ResourceCharges').SetText("0.00");

            var amind = gridBOQResourcesList.batchEditApi.GetColumnIndex('Amount');
            gridBOQResourcesList.batchEditApi.StartEdit(globalrowindex2, amind);
            gridBOQResourcesList.GetEditor('Amount').SetText(Tamount);
        }
        else {
            if (TypId == "3" || TypId == "") {
                var Netamind = gridBOQResourcesList.batchEditApi.GetColumnIndex('NetAmount');
                gridBOQResourcesList.batchEditApi.StartEdit(globalrowindex2, Netamind);
                gridBOQResourcesList.GetEditor('NetAmount').SetText(Tamount);

                var Charges = gridBOQResourcesList.batchEditApi.GetColumnIndex('ResourceCharges');
                gridBOQResourcesList.batchEditApi.StartEdit(globalrowindex2, Charges);
                gridBOQResourcesList.GetEditor('ResourceCharges').SetText("0.00");

                var amind = gridBOQResourcesList.batchEditApi.GetColumnIndex('Amount');
                gridBOQResourcesList.batchEditApi.StartEdit(globalrowindex2, amind);
                gridBOQResourcesList.GetEditor('Amount').SetText(Tamount);
            }
            else {
                var Netamind = gridBOQResourcesList.batchEditApi.GetColumnIndex('NetAmount');
                gridBOQResourcesList.batchEditApi.StartEdit(globalrowindex2, Netamind);
                gridBOQResourcesList.GetEditor('NetAmount').SetText(data[2]);

                var Charges = gridBOQResourcesList.batchEditApi.GetColumnIndex('ResourceCharges');
                gridBOQResourcesList.batchEditApi.StartEdit(globalrowindex2, Charges);
                gridBOQResourcesList.GetEditor('ResourceCharges').SetText(data[1].toString());

                var amind = gridBOQResourcesList.batchEditApi.GetColumnIndex('Amount');
                gridBOQResourcesList.batchEditApi.StartEdit(globalrowindex2, amind);
                gridBOQResourcesList.GetEditor('Amount').SetText(data[0]);
            }
        }
    }
    gridBOQResourcesList.batchEditApi.EndEdit();
    // BOQGridResourceSetTotalAmount("", "");
}

function BOQResourceGridUOMFocus(s, e) {

    if (e.htmlEvent.key == "Tab" && e.htmlEvent.shiftKey) {
        gridBOQResourcesList.batchEditApi.EndEdit();
        var localindex = gridBOQResourcesList.batchEditApi.GetColumnIndex('ProductQty');

        gridBOQResourcesList.batchEditApi.StartEdit(globalrowindex2, localindex);
    }
}

function BOQResourceGridSetQtyKeyDown(s, e) {

    //if (e.htmlEvent.key == "Tab" && e.htmlEvent.shiftKey) {
    //    gridBOQResourcesList.batchEditApi.EndEdit();
    //    // var localindex = gridBOQResourcesList.batchEditApi.GetColumnIndex('ProductQty');

    //    gridBOQResourcesList.batchEditApi.StartEdit(globalrowindex2, ResAddlDescIndex);
    //}
}

function ProductQtyKeyDown(s, e) {

    //if (e.htmlEvent.key == "Tab" && e.htmlEvent.shiftKey) {
    //    // gridBOQProductEntryList.batchEditApi.EndEdit();
    //    // var localindex = gridBOQResourcesList.batchEditApi.GetColumnIndex('ProductQty');

    //    // gridBOQProductEntryList.batchEditApi.StartEdit(globalrowindex, PrdAddlDescIndex);
    //}
}

function DiscountResourceKeyDown(s, e) {
    //if (e.htmlEvent.key == "Tab" && e.htmlEvent.shiftKey) {
    //    gridBOQResourcesList.batchEditApi.EndEdit();
    //    var localindex = gridBOQResourcesList.batchEditApi.GetColumnIndex('Amount');
    //   // gridBOQResourcesList.batchEditApi.StartEdit(globalrowindex2, localindex);
    //    gridBOQResourcesList.batchEditApi.EndEdit(e.visibleIndex, ResPriceIndex);
    //}
}

function BOQResourceGridSetAmountQty(s, e) {
    //gridBOQResourcesList.batchEditApi.EndEdit();
    //debugger;

    var TypId = gridBOQResourcesList.GetEditor("TaxTypeID").GetText();
    var Price = gridBOQResourcesList.batchEditApi.GetCellValue(globalrowindex2, 'Price');
    var Qty = gridBOQResourcesList.batchEditApi.GetCellValue(globalrowindex2, 'ProductQty');
    var Discount = gridBOQResourcesList.batchEditApi.GetCellValue(globalrowindex2, 'Discount');
    if (Price != "" && Qty != "") {
        var amount = parseFloat((parseFloat(Qty).toFixed(4)) * (parseFloat(Price).toFixed(2))).toFixed(2);

        var amountAfterDiscount = parseFloat(amount) - ((parseFloat(Discount) * parseFloat(amount)) / 100);
        var Tamount = parseFloat(DecimalRoundoff(amountAfterDiscount, 2)).toFixed(2);

        //gridBOQResourcesList.batchEditApi.StartEdit(globalrowindex2, 12);
        //gridBOQResourcesList.GetEditor('NetAmount').SetText(Tamount);

        //gridBOQResourcesList.batchEditApi.StartEdit(globalrowindex2, 9);
        //gridBOQResourcesList.GetEditor('Amount').SetText(Tamount);

        var taxType = "O";
        var amtFor = $("#ddl_AmountAre").val();
        if (amtFor == "2") {
            taxType = "I";
        }
        else if (amtFor == "1") {
            taxType = "E";
        }

        var ret = caluculateAndSetGST(gridBOQResourcesList.GetEditor("Amount"), gridBOQResourcesList.GetEditor("ResourceCharges"), gridBOQResourcesList.GetEditor("NetAmount"),
                gridBOQResourcesList.GetEditor("ProdHSN").GetText(), DecimalRoundoff(amount, 2), DecimalRoundoff(amountAfterDiscount, 2), taxType, "15", $('#ddlBankBranch').val())
        var data = ret.split('~');


        if (taxType == "O") {
            var Netamind = gridBOQResourcesList.batchEditApi.GetColumnIndex('NetAmount');
            gridBOQResourcesList.batchEditApi.StartEdit(globalrowindex2, Netamind);
            gridBOQResourcesList.GetEditor('NetAmount').SetText(Tamount);

            var Charges = gridBOQResourcesList.batchEditApi.GetColumnIndex('ResourceCharges');
            gridBOQResourcesList.batchEditApi.StartEdit(globalrowindex2, Charges);
            gridBOQResourcesList.GetEditor('ResourceCharges').SetText("0.00");

            var amind = gridBOQResourcesList.batchEditApi.GetColumnIndex('Amount');
            gridBOQResourcesList.batchEditApi.StartEdit(globalrowindex2, amind);
            gridBOQResourcesList.GetEditor('Amount').SetText(Tamount);
        }
        else {
            if (TypId == "3" || TypId == "") {
                var Netamind = gridBOQResourcesList.batchEditApi.GetColumnIndex('NetAmount');
                gridBOQResourcesList.batchEditApi.StartEdit(globalrowindex2, Netamind);
                gridBOQResourcesList.GetEditor('NetAmount').SetText(Tamount);

                var Charges = gridBOQResourcesList.batchEditApi.GetColumnIndex('ResourceCharges');
                gridBOQResourcesList.batchEditApi.StartEdit(globalrowindex2, Charges);
                gridBOQResourcesList.GetEditor('ResourceCharges').SetText("0.00");

                var amind = gridBOQResourcesList.batchEditApi.GetColumnIndex('Amount');
                gridBOQResourcesList.batchEditApi.StartEdit(globalrowindex2, amind);
                gridBOQResourcesList.GetEditor('Amount').SetText(Tamount);
            }
            else {
                var Netamind = gridBOQResourcesList.batchEditApi.GetColumnIndex('NetAmount');
                gridBOQResourcesList.batchEditApi.StartEdit(globalrowindex2, Netamind);
                gridBOQResourcesList.GetEditor('NetAmount').SetText(data[2]);

                var Charges = gridBOQResourcesList.batchEditApi.GetColumnIndex('ResourceCharges');
                gridBOQResourcesList.batchEditApi.StartEdit(globalrowindex2, Charges);
                gridBOQResourcesList.GetEditor('ResourceCharges').SetText(data[1].toString());

                var amind = gridBOQResourcesList.batchEditApi.GetColumnIndex('Amount');
                gridBOQResourcesList.batchEditApi.StartEdit(globalrowindex2, amind);
                gridBOQResourcesList.GetEditor('Amount').SetText(data[0]);
            }
        }
        gridBOQResourcesList.batchEditApi.EndEdit();
    }




    //var localindex = gridBOQResourcesList.batchEditApi.GetColumnIndex('ProductUOM');

    //gridBOQResourcesList.batchEditApi.StartEdit(globalrowindex2, localindex);
}


function SetRescTaxTypeInGrid() {

    var TaxTypeid = $('#ddlRescTaxTypelist').val();
    var TaxTypetxt = $('#ddlRescTaxTypelist option:selected').text();
    if (TaxTypetxt != "") {
        gridBOQResourcesList.batchEditApi.StartEdit(globalrowindex2, ResTaxTypeIndex);
        gridBOQResourcesList.GetEditor('TaxType').SetText(TaxTypetxt);

        gridBOQResourcesList.batchEditApi.StartEdit(globalrowindex2, ResTaxTypeIDIndex);
        gridBOQResourcesList.GetEditor('TaxTypeID').SetText(TaxTypeid);
        $('#RescTaxTypelistModel').modal('hide');



        var Price = gridBOQResourcesList.batchEditApi.GetCellValue(globalrowindex2, 'Price');
        var Qty = gridBOQResourcesList.batchEditApi.GetCellValue(globalrowindex2, 'ProductQty');
        var Discount = gridBOQResourcesList.batchEditApi.GetCellValue(globalrowindex2, 'Discount');

        if (Price != "" && Qty != "") {
            var amount = parseFloat((parseFloat(Qty).toFixed(4)) * (parseFloat(Price).toFixed(2))).toFixed(2);
            var amountAfterDiscount = parseFloat(amount) - ((parseFloat(Discount) * parseFloat(amount)) / 100);
            var Tamount = parseFloat(DecimalRoundoff(amountAfterDiscount, 2)).toFixed(2);

            if (TaxTypeid == "3" || TaxTypeid == "") {
                gridBOQResourcesList.batchEditApi.StartEdit(globalrowindex2, ResAmountIndex);
                gridBOQResourcesList.GetEditor('Amount').SetText(Tamount);

                gridBOQResourcesList.batchEditApi.StartEdit(globalrowindex2, ResNetAmountIndex);
                gridBOQResourcesList.GetEditor('NetAmount').SetText(Tamount);

                //if (gridBOQProductEntryList.batchEditApi.GetCellValue(globalrowindex, 11) == null) {
                gridBOQResourcesList.batchEditApi.StartEdit(globalrowindex2, ResResourceChargesIndex);
                gridBOQResourcesList.GetEditor('ResourceCharges').SetText("0.00");
                //  }
            }
            else {
                var taxType = "O";
                var amtFor = $("#ddl_AmountAre").val();
                if (amtFor == "2") {
                    taxType = "I";
                }
                else if (amtFor == "1") {
                    taxType = "E";
                }

                var ret = caluculateAndSetGST(gridBOQResourcesList.GetEditor("Amount"), gridBOQResourcesList.GetEditor("ResourceCharges"), gridBOQResourcesList.GetEditor("NetAmount"),
                        gridBOQResourcesList.GetEditor("ProdHSN").GetText(), DecimalRoundoff(amount, 2), DecimalRoundoff(amountAfterDiscount, 2), taxType, "15", $('#ddlBankBranch').val())
                var data = ret.split('~');

                var Netamind = gridBOQResourcesList.batchEditApi.GetColumnIndex('NetAmount');
                gridBOQResourcesList.batchEditApi.StartEdit(globalrowindex2, Netamind);
                gridBOQResourcesList.GetEditor('NetAmount').SetText(data[2]);

                var Charges = gridBOQResourcesList.batchEditApi.GetColumnIndex('ResourceCharges');
                gridBOQResourcesList.batchEditApi.StartEdit(globalrowindex2, Charges);
                gridBOQResourcesList.GetEditor('ResourceCharges').SetText(data[1].toString());

                var amind = gridBOQResourcesList.batchEditApi.GetColumnIndex('Amount');
                gridBOQResourcesList.batchEditApi.StartEdit(globalrowindex2, amind);
                gridBOQResourcesList.GetEditor('Amount').SetText(data[0]);

            }
        }
        gridBOQResourcesList.batchEditApi.StartEdit(globalrowindex2, ResTaxTypeIndex);
    }
    else {
        jAlert("Please select Tax Type!");
    }
}


function ResourceTaxTypeKeyDown(s, e) {
    // debugger;
    console.log(e.htmlEvent.key);
    if (e.htmlEvent.key == "Enter") {
        s.OnButtonClick(0);
        setTimeout(function () {
            $('#ddlRescTaxTypelist').focus();
        }, 600);
    }
    else if (e.htmlEvent.key == "Tab") {
        var TypId = gridBOQResourcesList.GetEditor("TaxTypeID").GetText();

        var Price = gridBOQResourcesList.batchEditApi.GetCellValue(globalrowindex2, 'Price');
        var Qty = gridBOQResourcesList.batchEditApi.GetCellValue(globalrowindex2, 'ProductQty');
        var Discount = gridBOQResourcesList.batchEditApi.GetCellValue(globalrowindex2, 'Discount');
        if (Price != "" && Qty != "") {
            var amount = parseFloat((parseFloat(Qty).toFixed(4)) * (parseFloat(Price).toFixed(2))).toFixed(2);
            var amountAfterDiscount = parseFloat(amount) - ((parseFloat(Discount) * parseFloat(amount)) / 100);
            var Tamount = parseFloat(DecimalRoundoff(amountAfterDiscount, 2)).toFixed(2);

            if (TypId == "3" || TypId == "") {
                gridBOQResourcesList.batchEditApi.StartEdit(globalrowindex2, ResAmountIndex);
                gridBOQResourcesList.GetEditor('Amount').SetText(Tamount);

                gridBOQResourcesList.batchEditApi.StartEdit(globalrowindex2, ResNetAmountIndex);
                gridBOQResourcesList.GetEditor('NetAmount').SetText(Tamount);

                //if (gridBOQProductEntryList.batchEditApi.GetCellValue(globalrowindex, 11) == null) {
                gridBOQResourcesList.batchEditApi.StartEdit(globalrowindex2, ResResourceChargesIndex);
                gridBOQResourcesList.GetEditor('ResourceCharges').SetText("0.00");
                //  }
            }
            else {
                var taxType = "O";
                var amtFor = $("#ddl_AmountAre").val();
                if (amtFor == "2") {
                    taxType = "I";
                }
                else if (amtFor == "1") {
                    taxType = "E";
                }

                var ret = caluculateAndSetGST(gridBOQResourcesList.GetEditor("Amount"), gridBOQResourcesList.GetEditor("ResourceCharges"), gridBOQResourcesList.GetEditor("NetAmount"),
                        gridBOQResourcesList.GetEditor("ProdHSN").GetText(), DecimalRoundoff(amount, 2), DecimalRoundoff(amountAfterDiscount, 2), taxType, "15", $('#ddlBankBranch').val())
                var data = ret.split('~');

                var Netamind = gridBOQResourcesList.batchEditApi.GetColumnIndex('NetAmount');
                gridBOQResourcesList.batchEditApi.StartEdit(globalrowindex2, Netamind);
                gridBOQResourcesList.GetEditor('NetAmount').SetText(data[2]);

                var amind = gridBOQResourcesList.batchEditApi.GetColumnIndex('Amount');
                gridBOQResourcesList.batchEditApi.StartEdit(globalrowindex2, amind);
                gridBOQResourcesList.GetEditor('Amount').SetText(data[0]);

                var Charges = gridBOQResourcesList.batchEditApi.GetColumnIndex('ResourceCharges');
                gridBOQResourcesList.batchEditApi.StartEdit(globalrowindex2, Charges);
                gridBOQResourcesList.GetEditor('ResourceCharges').SetText(data[1].toString());

            }
        }
    }
}

//function FocusGrid() {
//    gridBOQProductEntryList.batchEditApi.StartEdit(-1,0);
//}

//function RemarksLostFocus(s, e) {
//    //gridBOQProductEntryList.batchEditApi.StartEdit(globalrowindex, 13);
//    //$('.addEdcircleBtn').focus();
//    debugger;


//}

// End Resource

var warehousefocus = 0;
function WarehouseKeyDown(s, e) {
    console.log(e.htmlEvent.key);
    if (e.htmlEvent.key == "Enter") {

        s.OnButtonClick(0);
        setTimeout(function () {
            $('#ddlWarehouselist').focus();
        }, 600);

    }
}

function ChargesKeyDown(s, e) {
    console.log(e.htmlEvent.key);
    if (e.htmlEvent.key == "Enter") {

        s.OnButtonClick(0);
        setTimeout(function () {
            $('#txt_ResAddlDesc').focus();
        }, 600);

        //
    }
}

function PriceKeyDown(s, e) {

    console.log(e.htmlEvent.key);
    if (e.htmlEvent.key == "Tab" && e.htmlEvent.shiftKey) {
        gridBOQProductEntryList.batchEditApi.EndEdit();
        WarehouseGotFocus();
    }
}

function DiscountKeyDown(s, e) {

    console.log(e.htmlEvent.key);
    if (e.htmlEvent.key == "Tab" && e.htmlEvent.shiftKey) {
        gridBOQProductEntryList.batchEditApi.EndEdit();
        var localindex = gridBOQProductEntryList.batchEditApi.GetColumnIndex('Amount');
        gridBOQProductEntryList.batchEditApi.StartEdit(globalrowindex, localindex);
    }
}

function WarehouseGotFocus() {

    var localindex = gridBOQProductEntryList.batchEditApi.GetColumnIndex('Charges');

    gridBOQProductEntryList.batchEditApi.StartEdit(globalrowindex, localindex);

}

function AddRowResourceKeyDown(s, e) {
    console.log(e.htmlEvent.key);
    if (e.htmlEvent.key == "Enter") {
        if ($('#hdnDetailsID').val() == 0) {
            addNewRowTogridResources();
        }
        else {
            addNewRowToEditResourcegrid();
        }
    }
}

var tempindexcount = [];
function AddRowKeyDown(s, e) {
    console.log(e.htmlEvent.key);
    if (e.htmlEvent.key == "Enter") {
        if ($('#hdnDetailsID').val() == 0) {
            addNewRowTogrid();
        }
        else {
            addNewRowToEditgrid();
        }
    }
    if (e.htmlEvent.key == "Tab") {
        var hdnDetailsID = $('#hdnDetailsID').val();
        if (hdnDetailsID > 0) {
            if (globalrowindex == 0) {
                tempindexcount = [];
            }
            var tempindex = (globalrowindex + 1);
            if (gridBOQProductEntryList.batchEditApi.GetCellValue(tempindex, 'SlNO') != null && globalrowindex >= 0 && tempindexcount.includes(tempindex) == false) {
                tempindexcount.push(tempindex);
                gridBOQProductEntryList.batchEditApi.EndEdit();
                setTimeout(function () {
                    var localindex = gridBOQResourcesList.batchEditApi.GetColumnIndex('ProductName');
                    gridBOQProductEntryList.batchEditApi.StartEdit(tempindex, localindex);
                }, 500);

                hasfoundindex = 1;
            }
            else {
                var tempindex = -1;
                if (gridBOQProductEntryList.batchEditApi.GetCellValue(tempindex, 'SlNO') != null && tempindexcount.includes(tempindex) == false) {
                    tempindexcount.push(tempindex);
                    gridBOQProductEntryList.batchEditApi.EndEdit();
                    setTimeout(function () {
                        var localindex = gridBOQResourcesList.batchEditApi.GetColumnIndex('ProductName');
                        gridBOQProductEntryList.batchEditApi.StartEdit(tempindex, localindex);
                    }, 500);


                }
            }

        }
        else {


            var tempindex = (globalrowindex - 1);
            if (gridBOQProductEntryList.batchEditApi.GetCellValue(tempindex, 'SlNO') != null) {
                gridBOQProductEntryList.batchEditApi.EndEdit();

                setTimeout(function () {
                    var localindex = gridBOQResourcesList.batchEditApi.GetColumnIndex('ProductName');
                    gridBOQProductEntryList.batchEditApi.StartEdit(tempindex, localindex);
                }, 500);

            }
            else {
                var tempindex = (globalrowindex - 1);
                if (gridBOQProductEntryList.batchEditApi.GetCellValue(tempindex, 'SlNO') != null) {

                    gridBOQProductEntryList.batchEditApi.EndEdit();
                    setTimeout(function () {
                        var localindex = gridBOQResourcesList.batchEditApi.GetColumnIndex('ProductName');
                        gridBOQProductEntryList.batchEditApi.StartEdit(tempindex, localindex);
                    }, 500);
                }
            }
        }
    }
}

function BOQChargesKeyDown(s, e) {
    console.log(e.htmlEvent.key);
    if (e.htmlEvent.key == "Enter") {

        s.OnButtonClick(0);
    }
    //if (e.htmlEvent.key == "Tab") {

    //    s.OnButtonClick(0);
    //}
}

function ProductKeyDown(s, e) {
    console.log(e.htmlEvent.key);
    if (e.htmlEvent.key == "Enter") {

        //if (gridBOQProductEntryList.focusedRowIndex != null && gridBOQProductEntryList.focusedRowIndex != undefined) {
        //    globalrowindex = gridBOQProductEntryList.focusedRowIndex;
        //}
        s.OnButtonClick(0);
        //OpenProductList(s, e);
    }
        //if (e.htmlEvent.key == "Tab") {
        //    //if (gridBOQProductEntryList.focusedRowIndex != null && gridBOQProductEntryList.focusedRowIndex != undefined) {
        //    //    globalrowindex = gridBOQProductEntryList.focusedRowIndex;
        //    //}
        //    s.OnButtonClick(0);
        //    //OpenProductList(s, e);
        //}
    else if (e.code == "ArrowDown") {
        if ($("input[GridProductIndex=0]"))
            $("input[GridProductIndex=0]").focus();
    }
}

function TaxTypeKeyDown(s, e) {
    console.log(e.htmlEvent.key);
    if (e.htmlEvent.key == "Enter") {
        setTimeout(function () {
            $('#ddlTaxTypelist').focus();
        }, 600);
        s.OnButtonClick(0);
    }
    else if (e.htmlEvent.key == "Tab") {
        var TypId = gridBOQProductEntryList.GetEditor("TaxTypeID").GetText();

        var Price = gridBOQProductEntryList.batchEditApi.GetCellValue(globalrowindex, 'Price');
        var Qty = gridBOQProductEntryList.batchEditApi.GetCellValue(globalrowindex, 'ProductQty');
        var Discount = gridBOQProductEntryList.batchEditApi.GetCellValue(globalrowindex, 'Discount');
        if (Price != "" && Qty != "") {
            var amount = parseFloat((parseFloat(Qty).toFixed(4)) * (parseFloat(Price).toFixed(2))).toFixed(2);
            var amountAfterDiscount = parseFloat(amount) - ((parseFloat(Discount) * parseFloat(amount)) / 100);
            var Tamount = parseFloat(DecimalRoundoff(amountAfterDiscount, 2)).toFixed(2);

            if (TypId == "3" || TypId == "") {
                gridBOQProductEntryList.batchEditApi.StartEdit(globalrowindex, PrdAmountIndex);
                gridBOQProductEntryList.GetEditor('Amount').SetText(Tamount);

                gridBOQProductEntryList.batchEditApi.StartEdit(globalrowindex, PrdNetAmtIndex);
                gridBOQProductEntryList.GetEditor('NetAmount').SetText(Tamount);

                //if (gridBOQProductEntryList.batchEditApi.GetCellValue(globalrowindex, 11) == null) {
                gridBOQProductEntryList.batchEditApi.StartEdit(globalrowindex, PrdChargesIndex);
                gridBOQProductEntryList.GetEditor('Charges').SetText("0.00");
                //  }
            }
            else {
                var taxType = "O";
                var amtFor = $("#ddl_AmountAre").val();
                if (amtFor == "2") {
                    taxType = "I";
                }
                else if (amtFor == "1") {
                    taxType = "E";
                }

                var ret = caluculateAndSetGST(gridBOQProductEntryList.GetEditor("Amount"), gridBOQProductEntryList.GetEditor("Charges"), gridBOQProductEntryList.GetEditor("NetAmount"),
                        gridBOQProductEntryList.GetEditor("ProdHSN").GetText(), DecimalRoundoff(amount, 2), DecimalRoundoff(amountAfterDiscount, 2), taxType, "15", $('#ddlBankBranch').val())
                var data = ret.split('~');

                var Netamind = gridBOQProductEntryList.batchEditApi.GetColumnIndex('NetAmount');
                gridBOQProductEntryList.batchEditApi.StartEdit(globalrowindex, Netamind);
                gridBOQProductEntryList.GetEditor('NetAmount').SetText(data[2]);

                var amind = gridBOQProductEntryList.batchEditApi.GetColumnIndex('Amount');
                gridBOQProductEntryList.batchEditApi.StartEdit(globalrowindex, amind);
                gridBOQProductEntryList.GetEditor('Amount').SetText(data[0]);

                var Charges = gridBOQProductEntryList.batchEditApi.GetColumnIndex('Charges');
                gridBOQProductEntryList.batchEditApi.StartEdit(globalrowindex, Charges);
                gridBOQProductEntryList.GetEditor('Charges').SetText(data[1].toString());
            }
        }
    }
}

function SetEstimateFocusGrid() {
    gridBOQProductEntryList.batchEditApi.StartEdit(globalrowindex, PrdNetAmtIndex);
}

function NonIProductKeyDown(s, e) {
    console.log(e.htmlEvent.key);
    if (e.htmlEvent.key == "Enter") {
        s.OnButtonClick(0);
    }
        //if (e.htmlEvent.key == "Tab") {
        //    s.OnButtonClick(0);
        //}
    else if (e.code == "ArrowDown") {
        if ($("input[NonIProductIndex=0]"))
            $("input[NonIProductIndex=0]").focus();
    }
}

function btnPayStructure_KeyDown(s, e) {

    console.log(e.htmlEvent.key);
    if (e.htmlEvent.key == "Enter") {

        s.OnButtonClick(0);
    }
        //if (e.htmlEvent.key == "Tab") {

        //    s.OnButtonClick(0);
        //}
    else if (e.code == "ArrowDown") {
        if ($("input[ProductIndex=0]"))
            $("input[ProductIndex=0]").focus();
    }
}

function ValueSelected(e, indexName) {

    if (e.code == "Enter") {

        var Id = e.target.parentElement.parentElement.cells[0].innerText;
        var name = e.target.parentElement.parentElement.cells[1].children[0].value;
        if (Id) {
            if (indexName == "ProductIndex") {
                SetProduct(Id, name, null);
            }
            else if (indexName == "EstimateIndex") {
                SetGridEstimateProduct(Id, name, null);
            }
            else if (indexName == "GridProductIndex") {
                SetGridProduct(Id, name, null);
            }
            else if (indexName == "NonIProductIndex") {
                SetGridNonInventoryProduct(Id, name, null);
            }
            if (indexName == "customerIndex") {
                SetCustomer(Id, name);
            }
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
            if (indexName == "ProductIndex")
                $('#txtProductName').focus();
            else if (indexName == "EstimateIndex")
                $('#txtEstimateName').focus();
            else if (indexName == "GridProductIndex")
                $('#txtGridProductName').focus();
            else if (indexName == "NonIProductIndex")
                ('#txtGridProductName').focus();
            else if (indexName == "customerIndex") {
                $('#txtCustSearch').focus();
            }
            //else
            //    $('#txtCustSearch').focus();
        }
    }

}

function SetFocusDesc() {
    gridBOQProductEntryList.batchEditApi.StartEdit(globalrowindex, PrdProdNameIndex);
    gridBOQResourcesList.batchEditApi.StartEdit(globalrowindex2, ResProductNameIndex);
}

function BOQSave(mode) {
    //debugger;
    LoadingPanel.Show();
    savemode = mode;
    hasmsg = 0;
    var BOQNo = $('#BOQNo').val();
    var EstimateDate = GetServerDateFormat(BOQDate_dt.GetValue());
    var RevisionNo = $('#RevisionNo').val();
    var RevisionDate = GetServerDateFormat(RevisionDate_dt.GetValue());
    //var Unit = $('#ddlBankBranch option:selected').val();
    var Unit = $('#ddlBankBranch').val();
    //var WarehouseID = $('#ddlWarehouse option:selected').val();
    var SchemaID = $('#hdnSchemaId').val();
    var hdnApprove = $('#hdnApprove').val();
    var ActualAdditionalCost = $('#txtActualAdditionalCost').val();
    if (ActualAdditionalCost == '') {
        ActualAdditionalCost = parseFloat(0).toFixed(2);
        $('#txtActualAdditionalCost').val(ActualAdditionalCost);
    }
    var EstimateEdit = '@ViewBag.ApproveStusEdit';
    var ProjectMandatoryInEntry = '@ViewBag.ProjectMandatoryInEntry';
    var ProjectSelectInEntryModule = '@ViewBag.ProjectShow';
    var hdnRevisionNo = $('#hdnRevisionNo').val();
    var hdnDetailsID = $('#hdnDetailsID').val();


    var oneproduct = "";
    var visiablerow = gridBOQProductEntryList.GetVisibleRowsOnPage();
    if (visiablerow > 0) {
        for (var i = 100; i > -500; i--) {
            if (gridBOQProductEntryList.GetRow(i)) {

                if (oneproduct == "" || oneproduct == null) {
                    oneproduct = gridBOQProductEntryList.batchEditApi.GetCellValue(i, 'ProductName');
                }
            }
        }
    }
    if (hdnDetailsID == "") {
        RevisionNo = " ";
        RevisionDate = GetServerDateFormat(new Date);
    }

    if (taging == "true") {
        RevisionNo = " ";
        RevisionDate = GetServerDateFormat(new Date);
    }

    var CustomerId = $('#CustomerId').val();
    if (CustomerId == "" || CustomerId == null) {
        jAlert("Please select customer.");
        LoadingPanel.Hide();
        return false;
    }
    if (ProjectMandatoryInEntry == "Yes" && ProjectSelectInEntryModule == "Yes" && ProjectGridLookup.GetSelectedKeyFieldValues() == "") {
        jAlert("Please Select Project.");
        LoadingPanel.Hide();
        return false;
    }

    else {
        if (oneproduct != "" && oneproduct != null) {
            if ($('#hdnApproveRevSettings').val() == "Yes") {
                if (hdnDetailsID > 0 && hdnRevisionNo == RevisionNo && hdnApprove != 'Approve' && EstimateEdit == '1') {
                    jAlert("Please enter new revision number to save.");
                    LoadingPanel.Hide();
                    return false;
                }
                else {
                    if (BOQNo != '' && EstimateDate != '' && RevisionNo != '' && RevisionDate != '' && Unit != '' && ActualAdditionalCost != '') {

                        if (hdnDetailsID > 0 && RevisionNo != "" && hdnApprove != 'Approve' && EstimateEdit == '1') {

                            $.ajax({
                                type: "POST",
                                url: "@Url.Action("ProcessWithRevisionNumber", "BillofQuantities")",
                                data: { detailsid: hdnDetailsID, RevisionNo: RevisionNo },
                            success: function (response) {

                                if (response) {

                                    SuffleRows();
                                    SuffleRowsGrid2();
                                    //  if (gridBOQResourcesList.batchEditApi.HasChanges()) {
                                    gridBOQResourcesList.UpdateEdit();
                                    // }
                                    gridBOQProductEntryList.UpdateEdit();
                                    gridBOQProductEntryList.UpdateEdit();

                                    setTimeout(function () {
                                        // OnResourcesEndCallback();
                                    }, 1500);
                                }
                                else {
                                    jAlert("Please enter new revision number to save.");
                                    LoadingPanel.Hide();
                                    return false;
                                }
                            }
                        });

                    }
                    else {

                        SuffleRows();
                        SuffleRowsGrid2();
                        //if (gridBOQResourcesList.batchEditApi.HasChanges()) {
                        gridBOQResourcesList.UpdateEdit();
                        //}
                        gridBOQProductEntryList.UpdateEdit();
                        gridBOQProductEntryList.UpdateEdit();

                        setTimeout(function () {
                            // OnResourcesEndCallback();
                        }, 1500);
                    }
                }
            else {
                        savemode = "";
                if (BOQNo == '') {
                    $('#EBOQNo').show();
                }
                else {
                    $('#EBOQNo').hide();
                }
                if (EstimateDate == '') {
                    $('#EBOQDate_dt').show();
                }
                else {
                    $('#EBOQDate_dt').hide();
                }
                if (RevisionNo == '') {
                    $('#ERevisionNo').show();
                }
                else {
                    $('#ERevisionNo').hide();
                }
                if (RevisionDate == '') {
                    $('#ERevisionDate_dt').show();
                }
                else {
                    $('#ERevisionDate_dt').hide();
                }
                if (Unit == '') {
                    $('#EddlBankBranch').show();
                }
                else {
                    $('#EddlBankBranch').hide();
                }

                $("html, body").animate({ scrollTop: 0 }, 600);
                LoadingPanel.Hide();
                return false;
            }
        }
    }
else {
                SuffleRows();
    SuffleRowsGrid2();
    gridBOQResourcesList.UpdateEdit();
    gridBOQProductEntryList.UpdateEdit();
    gridBOQProductEntryList.UpdateEdit();
}
}
else {
    $("html, body").animate({ scrollTop: 0 }, 600);
    savemode = "";
    if (BOQNo == '') {
        $('#EBOQNo').show();
    }
    else {
        $('#EBOQNo').hide();
    }
    if (EstimateDate == '') {
        $('#EBOQDate_dt').show();
    }
    else {
        $('#EBOQDate_dt').hide();
    }
    if (RevisionNo == '') {
        $('#ERevisionNo').show();
    }
    else {
        $('#ERevisionNo').hide();
    }
    if (RevisionDate == '') {
        $('#ERevisionDate_dt').show();
    }
    else {
        $('#ERevisionDate_dt').hide();
    }
    if (Unit == '') {
        $('#EddlBankBranch').show();
    }
    else {
        $('#EddlBankBranch').hide();
    }

    jAlert('Cannot Save. You must enter atleast one Product to save this entry.');
    LoadingPanel.Hide();
    return false;

}
}
}

function OnResourcesStartCallback(s, e) {
    //debugger;
    var strBOQNo = $('#BOQNo').val();
    var BOQDate = GetServerDateFormat(BOQDate_dt.GetValue());
    //var EstimateDate = $('#BOQDate_dt').val();
    //var EstimateType = $('#slcEstimatetype option:selected').val();
    var RevisionNo = $('#RevisionNo').val();
    var RevisionDate = GetServerDateFormat(RevisionDate_dt.GetValue());
    if (RevisionDate == "") {
        RevisionDate = GetServerDateFormat(new Date);
    }
    //var RevisionDate = $('#RevisionDate_dt').val();
    var Unit = $('#ddlBankBranch option:selected').val();
    //var WarehouseID = $('#ddlWarehouse option:selected').val();
    var SchemaID = $('#hdnSchemaId').val();
    var ActualAdditionalCost = $('#txtActualAdditionalCost').val();

    var Proposal_ID = $('#hdnProposal_ID').val();
    var Quotation_ID = $('#hdnQuotation_ID').val();
    var HeadRemarks = $('#txt_HeadRemarks').val();

    var hdnDetailsID = $('#hdnDetailsID').val();
    if (hdnDetailsID > 0) {
        DetailsID = hdnDetailsID;
    }
    if (hdnDetailsID == "") {

        RevisionDate = GetServerDateFormat(new Date);
    }
    var taxId = $('#ddl_AmountAre').val();
    var CustomerId = $('#CustomerId').val();
    var ApproveRemarks = $("#txt_ApproveRemarks").val();

    var apprvRejt = "0";
    var approveedval = '@ViewBag.ApproveStusEdit';
    var EditAction = "";
    if ($('#hdnApprove').val() != "") {
        EditAction = $('#hdnApprove').val();
        apprvRejt = approveedval;
    }
    else {
        if (approveedval == "1" || approveedval == "") {
            EditAction = "";
            apprvRejt = "0";
        }
        else {
            EditAction = "Approve";
            apprvRejt = approveedval;
        }
    }



    if ($("#hdnApproveReject").val() == "Approve") {
        apprvRejt = "1";
    }
    else if ($("#hdnApproveReject").val() == "Reject") {
        apprvRejt = "2";
    }

    if (e != undefined) {
        e.customArgs["strBOQNo"] = strBOQNo;
        e.customArgs["BOQDate"] = BOQDate;
        //e.customArgs["FinishedItem"] = FinishedItem;
        //e.customArgs["FinishedQty"] = FinishedQty;

        //e.customArgs["FinishedUom"] = FinishedUom;
        //e.customArgs["EstimateType"] = EstimateType;
        e.customArgs["RevisionNo"] = RevisionNo; //EmployeesCounterTargetList

        e.customArgs["RevisionDate"] = RevisionDate;
        e.customArgs["Unit"] = Unit;
        // e.customArgs["WarehouseID"] = WarehouseID;
        e.customArgs["BOQ_SCHEMAID"] = SchemaID;
        e.customArgs["ActualAdditionalCost"] = ActualAdditionalCost;

        e.customArgs["ProductionID"] = ProductionID;
        e.customArgs["DetailsID"] = DetailsID;

        e.customArgs["Proposal_ID"] = ProposalGridLookup.GetSelectedKeyFieldValues();
        e.customArgs["Quotation_ID"] = Quotation_ID;
        e.customArgs["HeadRemarks"] = HeadRemarks;

        e.customArgs["ContractNo"] = ContractID;//ContractGridLookup.GetSelectedKeyFieldValues();
        e.customArgs["ProjectID"] = ProjectGridLookup.GetSelectedKeyFieldValues();
        e.customArgs["TaxID"] = projectCode;
        e.customArgs["ContractNo"] = ContractID;
        e.customArgs["ApproveRemarks"] = ApproveRemarks;
        e.customArgs["ApprvRejct"] = apprvRejt;
        e.customArgs["Approve"] = EditAction;
        e.customArgs["TagingID"] = ProdTagID;
        e.customArgs["IsTagging"] = taging;
        e.customArgs["EstimateID"] = EstimateGridLookup.GetSelectedKeyFieldValues();
        e.customArgs["IsTaggingModule"] = TaggingModule;
        e.customArgs["TaggingModuleSave"] = $("#hdnTagModule").val();
        e.customArgs["ApprovRevSettings"] = $("#hdnApproveRevSettings").val();
    }
}

function OnStartCallback(s, e) {
    //debugger;
    var strBOQNo = $('#BOQNo').val();
    var BOQDate = GetServerDateFormat(BOQDate_dt.GetValue());
    //var EstimateType = $('#slcEstimatetype option:selected').val();
    var RevisionNo = $('#RevisionNo').val();
    var RevisionDate = GetServerDateFormat(RevisionDate_dt.GetValue());
    if (RevisionDate == "") {
        RevisionDate = GetServerDateFormat(new Date);
    }
    //var RevisionDate = $('#RevisionDate_dt').val();
    var Unit = $('#ddlBankBranch option:selected').val();
    //var WarehouseID = $('#ddlWarehouse option:selected').val();
    var SchemaID = $('#hdnSchemaId').val();
    var ActualAdditionalCost = $('#txtActualAdditionalCost').val();

    var hdnDetailsID = $('#hdnDetailsID').val();

    var Proposal_ID = $('#hdnProposal_ID').val();
    var Quotation_ID = $('#hdnQuotation_ID').val();
    var HeadRemarks = $('#txt_HeadRemarks').val();

    var CustomerId = $('#CustomerId').val();
    var taxId = $('#ddl_AmountAre').val();

    var ApproveRemarks = $("#txt_ApproveRemarks").val();

    if (hdnDetailsID > 0) {
        DetailsID = hdnDetailsID;
    }

    if (hdnDetailsID == "") {

        RevisionDate = GetServerDateFormat(new Date);
    }
    var apprvRejt = "0";
    var approveedval = '@ViewBag.ApproveStusEdit';
    var EditAction = "";
    if ($('#hdnApprove').val() != "") {
        EditAction = $('#hdnApprove').val();
        apprvRejt = approveedval;
    }
    else {
        if (approveedval == "1" || approveedval == "") {
            EditAction = "";
            apprvRejt = "0";
        }
        else {
            EditAction = "Approve";
            apprvRejt = approveedval;
        }
    }

    if ($("#hdnApproveReject").val() == "Approve") {
        apprvRejt = "1";
    }
    else if ($("#hdnApproveReject").val() == "Reject") {
        apprvRejt = "2";
    }

    if (e != undefined) {
        e.customArgs["strBOQNo"] = strBOQNo;
        e.customArgs["BOQDate"] = BOQDate;
        //e.customArgs["FinishedItem"] = FinishedItem;
        //e.customArgs["FinishedQty"] = FinishedQty;

        //e.customArgs["FinishedUom"] = FinishedUom;
        // e.customArgs["EstimateType"] = EstimateType;
        e.customArgs["RevisionNo"] = RevisionNo; //EmployeesCounterTargetList

        e.customArgs["RevisionDate"] = RevisionDate;
        e.customArgs["Unit"] = Unit;
        // e.customArgs["WarehouseID"] = WarehouseID;
        e.customArgs["BOQ_SCHEMAID"] = SchemaID;
        e.customArgs["ActualAdditionalCost"] = ActualAdditionalCost;

        e.customArgs["ProductionID"] = ProductionID;
        e.customArgs["DetailsID"] = DetailsID;

        e.customArgs["Proposal_ID"] = ProposalGridLookup.GetSelectedKeyFieldValues();
        e.customArgs["Quotation_ID"] = Quotation_ID;
        e.customArgs["HeadRemarks"] = HeadRemarks;
        e.customArgs["Customer_ID"] = CustomerId;

        e.customArgs["ContractNo"] = ContractID;//ContractGridLookup.GetSelectedKeyFieldValues();
        e.customArgs["ProjectID"] = ProjectGridLookup.GetSelectedKeyFieldValues();
        e.customArgs["TaxID"] = taxId;
        e.customArgs["ApproveRemarks"] = ApproveRemarks;
        e.customArgs["ApprvRejct"] = apprvRejt;
        e.customArgs["Approve"] = EditAction;
        e.customArgs["TagingID"] = ProdTagID;
        e.customArgs["IsTagging"] = taging;
        e.customArgs["EstimateID"] = EstimateGridLookup.GetSelectedKeyFieldValues();
        e.customArgs["IsTaggingModule"] = TaggingModule;
        e.customArgs["TaggingModuleSave"] = $("#hdnTagModule").val();
        e.customArgs["ApprovRevSettings"] = $("#hdnApproveRevSettings").val();
    }
}

function Scheme_ValueChange() {
    var val = $('#ddlSchema option:selected').val();
    var schemetypeValue = val;
    var schemetype = schemetypeValue.toString().split('~')[1];
    var schemelength = schemetypeValue.toString().split('~')[2];
    var branchID = (schemetypeValue.toString().split('~')[3] != null) ? schemetypeValue.toString().split('~')[3] : "";
    var SchemaID = schemetypeValue.toString().split('~')[0];
    $('#hdnSchemaId').val(SchemaID);
    var fromdate = (schemetypeValue.toString().split('~')[5] != null) ? schemetypeValue.toString().split('~')[5] : "";
    var todate = (schemetypeValue.toString().split('~')[6] != null) ? schemetypeValue.toString().split('~')[6] : "";

    var dt = new Date();
    document.getElementById("BOQNo").maxLength = schemelength;
    BOQDate_dt.SetDate(dt);

    if (dt < new Date(fromdate)) {
        BOQDate_dt.SetDate(new Date(fromdate));
    }

    if (dt > new Date(todate)) {
        BOQDate_dt.SetDate(new Date(todate));
    }

    BOQDate_dt.SetMinDate(new Date(fromdate));
    BOQDate_dt.SetMaxDate(new Date(todate));

    //BOQDate_dt.focus();

    if (branchID > 0) {
        $('#ddlBankBranch').val(branchID);
    }

    if (schemetype == '0') {
        $('#BOQNo').removeAttr("disabled");
        $('#BOQNo').val('');

        $('#BOQNo').focus();
    }
    else if (schemetype == '1') {
        $('#BOQNo').attr("disabled", "disabled");
        $('#BOQNo').val('Auto');

        //$('#BOQNo').focus();

    }
    else if (schemetype == '2') {
        $('#BOQNo').attr("disabled", "disabled");
        $('#BOQNo').val('Datewise');

        //$('#BOQNo').focus();

    }
    else if (schemetype == 'n') {
        $('#BOQNo').attr("disabled", "disabled");
        $('#BOQNo').val('');

        //$('#BOQNo').focus();
    }
    else {
        $('#BOQNo').attr("disabled", "disabled");
        $('#BOQNo').val('');

        //$('#BOQNo').focus();

    }

}

function SuffleRows() {
    for (var i = 0; i < 1000; i++) {
        if (gridBOQProductEntryList.GetRow(i)) {
            if (gridBOQProductEntryList.GetRow(i).style.display != "none") {
                gridBOQProductEntryList.batchEditApi.StartEdit(i, PrdUpdateEditIndex);
                gridBOQProductEntryList.GetEditor("UpdateEdit").SetText(i);
            }
        }
    }

    for (i = -1; i > -1000; i--) {
        if (gridBOQProductEntryList.GetRow(i)) {
            if (gridBOQProductEntryList.GetRow(i).style.display != "none") {
                gridBOQProductEntryList.batchEditApi.StartEdit(i, PrdUpdateEditIndex);
                gridBOQProductEntryList.GetEditor("UpdateEdit").SetText(i);
            }
        }
    }
}

function SuffleRowsGrid2() {
    for (var i = 0; i < 1000; i++) {
        if (gridBOQResourcesList.GetRow(i)) {
            if (gridBOQResourcesList.GetRow(i).style.display != "none") {
                gridBOQResourcesList.batchEditApi.StartEdit(i, ResUpdateEditIndex);
                gridBOQResourcesList.GetEditor("UpdateEdit").SetText(i);
            }
        }
    }

    for (i = -1; i > -1000; i--) {
        if (gridBOQResourcesList.GetRow(i)) {
            if (gridBOQResourcesList.GetRow(i).style.display != "none") {
                gridBOQResourcesList.batchEditApi.StartEdit(i, ResUpdateEditIndex);
                gridBOQResourcesList.GetEditor("UpdateEdit").SetText(i);
            }
        }
    }
}

function parseDate(str) {
    var mdy = str.split('-');
    return new Date(mdy[2], mdy[1] - 1, mdy[0]);
}

function GetServerDateFormat(today) {
    if (today != "" && today != null) {
        var dd = today.getDate();
        var mm = today.getMonth() + 1;
        var yyyy = today.getFullYear();

        if (dd < 10) {
            dd = '0' + dd;
        }
        if (mm < 10) {
            mm = '0' + mm;
        }
        today = yyyy + '-' + mm + '-' + dd;
    }
    else {
        today = "";
    }

    return today;
}

function ChkDataDigitCount(e) {

    var data = $(e).val();
    $(e).val(parseFloat(data).toFixed(4));
    if (data == '' || data == null) {
        $(e).val(parseFloat(0).toFixed(4));
    }
}

function ChkMoneyDigitCount(e) {
    var data = $(e).val();
    $(e).val(parseFloat(data).toFixed(2));
}

$(document).ready(function () {
    // LoadingPanel.Show();
    //debugger;

    $("body").bind("keydown", function (event) {
        if ('@ViewBag.View' != "View") {
            if (hdnApprove != "Approve") {
                if (event.keyCode == 88 && event.altKey == true) {
                    BOQSave('Exit');
                }

                if (event.keyCode == 83 && event.altKey == true) {
                    BOQSave('New');
                }

            }
        }
        if (event.keyCode == 82 && event.altKey == true) {
            $('#showResources').click();
        }
    });

    var hdnDetailsID = $('#hdnDetailsID').val();
    if (hdnDetailsID > 0) {
        //$('#slcEstimatetype option:selected').val(hdnEstimateTYPE);
        $('#BOQNo').attr("disabled", "disabled");
        $('#ddlSchema').attr("disabled", "disabled");
        //$('#slcEstimatetype').attr("disabled", "disabled");

        //$('#BOQDate_dt').attr("disabled", "disabled");
        //btnFinishedItem.SetButtonVisible(0, '');
        //$('#FinishedQty').attr("disabled", "disabled");
        $('#ddlBankBranch').attr("disabled", "disabled");
        //$('#ddlWarehouse').attr("disabled", "disabled");
        BOQDate_dt.SetEnabled(false);
        //RevisionDate_dt.SetEnabled(false);
        CustomerTxt.SetEnabled(false);
        //var EstimateResourcesTotal = $('#EstimateResourcesTotalAm').val();
        //if (EstimateResourcesTotal != "" && EstimateResourcesTotal != undefined) {
        //    $('#txtGridResourcesTotalAmount').val(parseFloat(EstimateResourcesTotal).toFixed(2));
        //}

        BOQDate_dt.SetDate(new Date('@ViewBag.BOQDate'));
        RevisionDate_dt.SetDate(new Date('@ViewBag.RevDate'));

        var BOQProductsTotal = $('#hdnBOQEntryProductsTotalAm').val();
        if (BOQProductsTotal != "" && BOQProductsTotal != undefined) {
            $('#txtGridProductEntryTotalAmount').val(parseFloat(BOQProductsTotal).toFixed(2));
        }
        //$('#ddlSchema').val($('#hdnBOQ_SCHEMAID').val());
        var hdnBOQResourcesTotalAm = $('#hdnBOQResourcesTotalAm').val();
        if (hdnBOQResourcesTotalAm != "" && hdnBOQResourcesTotalAm != undefined) {
            $('#txtGridResourcesTotalAmount').val(parseFloat(hdnBOQResourcesTotalAm).toFixed(2));
        }

        var cntNo = '@ViewBag.ContractNo';

        ContractID = cntNo.split(',');

        $("#ddl_AmountAre").prop('disabled', 'disabled');

        $('#btnSaveandNew').hide();

        // debugger;
        //ProjectGridLookup.gridView.Refresh()
        //ProjectGridLookup.gridView.Refresh()
        //var projCode = $('#hdnProjectCode').val();
        //ProjectGridLookup.SetValue(projCode);
        //ProjectGridLookup.SetText(projCode);
        // $('#ddl_AmountAre').val('@Model.TaxID');
        $('#RevisionNo').removeAttr("disabled");


        setTimeout(function () { var noofrow = gridBOQResourcesList.GetVisibleRowsOnPage(); if (noofrow > 1) { $('#showResources').click(); } }, 800);

    }
    else {
        //$("#BOQNo").removeAttr("disabled");
        $("#ddlSchema").removeAttr("disabled");
        //$("#slcEstimatetype").removeAttr("disabled");
        $('#FinishedQty').removeAttr("disabled");
        $('#RevisionNo').removeAttr("disabled");
        //$('#ddlBankBranch').removeAttr("disabled");
        //$('#ddlWarehouse').removeAttr("disabled");
        BOQDate_dt.SetEnabled(true);
        RevisionDate_dt.SetEnabled(true);
        $('#btnSaveandNew').show();
        RevisionDate_dt.SetDate(null);
        $('#hdnBOQ_SCHEMAID').val('');

        $("#ddl_AmountAre").removeAttr("disabled");
        gridBOQProductEntryList.batchEditApi.EndEdit();
        gridBOQProductEntryList.batchEditApi.StartEdit(globalrowindex, PrdProdNameIndex);
        gridBOQProductEntryList.CancelEdit();
        setTimeout(function () {
            $("#ddlSchema").focus();
        }, 600);

    }

    if ('@ViewBag.Hierarchy' == "1") {
        $('#divHierarchy').removeClass('hidden');
    }
    else {
        $('#divHierarchy').addClass('hidden');
    }

    var hdnApprove = $('#hdnAprove').val();

    if (hdnApprove == "Approve") {
        $('#btnSaveandNew').hide();
        $('#btnSaveandExit').hide();
        $('#btnApprove').show();
        $('#btnReject').show();
        $('#divApproveButton').removeClass('hidden');
        $('#divApproveRemarks').removeClass('hidden');
        $('#RevisionNo').attr("disabled", "disabled");
        RevisionDate_dt.SetEnabled(false);
        var rejct = '@ViewBag.ApproveStus';
        if (rejct == '1') {
            $('#btnApprove').hide();
        }
        else if (rejct == '2') {
            $('#btnReject').hide();
        }
        setTimeout(function () {
            $('#txt_ApproveRemarks').focus();
        }, 1000);
    }
    else {
        $('#btnSaveandNew').show();
        $('#btnSaveandExit').show();
        $('#btnApprove').hide();
        $('#btnReject').hide();
        $('#divApproveRemarks').addClass('hidden');
        // $('#RevisionNo').removeClass("hidden");
        // RevisionDate_dt.SetEnabled(false);
        $("#lblApproveName").addClass('hidden');
        $('#divApproveButton').addClass('hidden');
    }

    if ('@ViewBag.ApproveStusEdit' != "1" && '@ViewBag.ApproveStusEdit' != "") {

        $('#RevisionNo').attr("disabled", "disabled");
        RevisionDate_dt.SetEnabled(false);
        $('#btnSaveandNew').hide();
    }

    if ('@ViewBag.ProjectShow' == "Yes") {
        $("#divProj").removeClass("hidden");
    }
    else {
        $("#divProj").addClass("hidden");
    }

    if ('@ViewBag.View' == "View") {
        $('#btnSaveandNew').hide();
        $('#btnSaveandExit').hide();
        $('#btnApprove').hide();
        $('#btnReject').hide();
        $('#divApproveRemarks').removeClass('hidden');
        $("#lblApproveName").removeClass('hidden');
        $('#divApproveButton').removeClass('hidden');

        setTimeout(function () {
            var noofrow = gridBOQResourcesList.GetVisibleRowsOnPage();
            if (noofrow > 1) {
                $('#showResources').removeClass('hidden');
            }
            else {
                $('#showResources').addClass('hidden');
            }
        }, 800);
    }

    if ('@ViewBag.Estimateid' != null) {
        // EstimateGridLookup
    }

    $('#GridWarehouselistModel').on('shown.bs.modal', function () {
        //$('#ddlWarehouselist').focus();
    })
    // LoadingPanel.Hide();




    if ($('#hdnApproveRevSettings').val() == "Yes") {
        $("#divRevDate").removeClass('hide');
        $("#divRevNo").removeClass('hide');
    }
    else {
        $("#divRevDate").addClass('hide');
        $("#divRevNo").addClass('hide');
    }


});


function datevalidateTo() {

    if (BOQDate_dt.GetDate()) {
        if (RevisionDate_dt.GetDate() <= BOQDate_dt.GetDate()) {
            if ($('#hdnDetailsID').val() != "") {
                RevisionDate_dt.SetValue(BOQDate_dt.GetDate());
                RevisionDate_dt.SetMinDate(BOQDate_dt.GetDate());
            }
        }
    }
}


function validateFloatKeyPress(el, evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode;
    var number = el.value.split('.');
    if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
        return false;
    }
    //just one dot (thanks ddlab)
    if (number.length > 1 && charCode == 46) {
        return false;
    }
    //get the carat position
    var caratPos = getSelectionStart(el);
    var dotPos = el.value.indexOf(".");
    if (caratPos > dotPos && dotPos > -1 && (number[1].length > 1)) {
        return false;
    }
    return true;
}


function getSelectionStart(o) {
    if (o.createTextRange) {
        var r = document.selection.createRange().duplicate()
        r.moveEnd('character', o.value.length)
        if (r.text == '') return o.value.length
        return o.value.lastIndexOf(r.text)
    } else return o.selectionStart
}

    $(function () {
        $('#refreshgrid2').hide();
        $(".decimalCheck").on("keypress keyup blur", function (event) {
            //this.value = this.value.replace(/[^0-9\.]/g,'');
            $(this).val($(this).val().replace(/[^0-9\.]/g, ''));
            if ((event.which != 46 || $(this).val().indexOf('.') != -1) && (event.which < 48 || event.which > 57)) {
                event.preventDefault();
            }
        });

        $('#showResources').click(function () {
            $('#refreshgrid2').show();
            gridBOQResourcesList.batchEditApi.StartEdit(globalrowindex2, ResProductNameIndex);

            $(this).hide();
        });

        $('#closeResource').click(function () {
            jConfirm('Are you sure to close? Clicking on "Yes" will clear the data from grid.', 'Alert!', function (r) {
                if (r) {
                    $('#refreshgrid2').hide();
                    $('#showResources').show();
                    for (var i = 500; i > -500; i--) {
                        if (gridBOQResourcesList.GetRow(i)) {
                            gridBOQResourcesList.DeleteRow(i);
                        }
                    }
                    $("#txtGridResourcesTotalAmount").val('0.00');
                    AddNewRowGridResources();
                }
            });
        });

        var hdnDetailsID = $('#hdnDetailsID').val();
        if (hdnDetailsID == "") {
            $('#redREV').hide();
            $('#redREVDate').hide();
            $('#RevisionNo').attr("disabled", "disabled");
            RevisionDate_dt.SetEnabled(false);
        }
    });


document.onkeydown = function (e) {

    if (event.keyCode == 88 && event.altKey == true) { //  && myModal.GetVisible() == true
        BOQSave('Exit');
    }
    if (event.keyCode == 83 && event.altKey == true) { //  && myModal.GetVisible() == true
        BOQSave('New');
    }
}

$(document).ready(function () {


    //  LoadingPanel.Show();
    $('#slideResource').click(function () {
        //$('#slideDiv').slideUp();
        if (!$('#slideDiv').hasClass('out')) {

            $('#slideDiv').addClass('out');
            $('#slideDiv').slideUp();
            $('#slideResource .fa-chevron-up').hide();
            $('#slideResource .fa-chevron-down').show();
        } else {

            $('#slideDiv').removeClass('out')
            $('#slideDiv').addClass('in');
            $('#slideDiv').slideDown();
            $('#slideResource .fa-chevron-up').show();
            $('#slideResource .fa-chevron-down').hide();
        }
    })

    // $("#ddlWarehouse").focusout(function () {
    // gridBOQProductEntryList.batchEditApi.EndEdit();
    //gridBOQProductEntryList.batchEditApi.StartEdit(globalrowindex, 2);
    //});
    var hdnDetailsID = $('#hdnDetailsID').val();
    if (hdnDetailsID == "") {
        //$('#FinishedQty').val(parseFloat(0).toFixed(4));
        $('#ddlSchema').focus();
    }
    // LoadingPanel.Hide();
});

function SetFocusQty() {
    $('#FinishedQty').select();
    $('#FinishedQty').focus();
}



function btnProposalList_Click(s, e) {
    Productlist('', null);
    setTimeout(function () { $("#txtProposalName").focus(); }, 500);
    $('#ProposallistModel').modal('show');
}

function btnQuotationList_Click(s, e) {
    Productlist('', null);
    setTimeout(function () { $("#txtQuotationName").focus(); }, 500);
    $('#QuotationlistModel').modal('show');
}

function btnProposal_KeyDown(s, e) {
    if (e.htmlEvent.key == "Enter") {
        s.OnButtonClick(0);
    }
    else if (e.code == "ArrowDown") {
        if ($("input[ProductIndex=0]"))
            $("input[ProductIndex=0]").focus();
    }
}

function btnQuotation_KeyDown(s, e) {
    if (e.htmlEvent.key == "Enter") {
        s.OnButtonClick(0);
    }
    else if (e.code == "ArrowDown") {
        if ($("input[ProductIndex=0]"))
            $("input[ProductIndex=0]").focus();
    }
}

function SetFocusQty() {
    $('#FinishedQty').select();
    $('#FinishedQty').focus();
}

function ProposalListkeydown(e) {
    if (e.code == "Enter" || e.code == "NumpadEnter") {
        if ($("#txtProposalName").val() != '') {
            Proposallist($("#txtProposalName").val(), null);
        }
    }
    else if (e.code == "ArrowDown") {
        if ($("input[ProposalIndex=0]"))
            $("input[ProposalIndex=0]").focus();
    }
}

function Proposallist(SearchKey, type) {
    finishedproductlist = 1;
    var OtherDetails = {}
    OtherDetails.SearchKey = SearchKey;
    OtherDetails.Type = type;

    var HeaderCaption = [];
    // HeaderCaption.push("Product ID");
    HeaderCaption.push("Proposal No.");
    HeaderCaption.push("Proposal Date");

    callonServerScroll("../Models/PMS_WebServiceList.asmx/GetProductDetailsList", OtherDetails, "ProposalTable", HeaderCaption, "ProposalIndex", "SetProposal");
}

function SetProposal(Id, Name, e) {
    finishedproductlist = 0;
    var ProposalID = Id;
    var ProposalName = Name;

    if (ProposalID != "") {

        var data = ProposalID.split('|');
        ProposalID = data[0];

        $('#ProposallistModel').modal('hide');
        btnProposal.SetText(ProposalName);
        $('#hdnProposal_ID').val(ProposalID);
        //  document.getElementById('hdnProposal_ID').value = Id;
        $('#btnProposal').select();
        $('#btnProposal').focus();
    }
}

function QuotationListkeydown(e) {
    //   debugger;
    if (e.code == "Enter" || e.code == "NumpadEnter") {
        if ($("#txtQuotationName").val() != '') {
            Quotationlist($("#txtQuotationName").val(), null);
        }
    }
    else if (e.code == "ArrowDown") {
        if ($("input[QuotationIndex=0]"))
            $("input[QuotationIndex=0]").focus();
    }
}

function Quotationlist(SearchKey, type) {
    //  debugger;
    finishedproductlist = 1;
    var OtherDetails = {}
    OtherDetails.SearchKey = SearchKey;
    OtherDetails.Type = type;

    var HeaderCaption = [];
    // HeaderCaption.push("Product ID");
    HeaderCaption.push("Quotation No.");
    HeaderCaption.push("Quotation Date");

    callonServerScroll("../Models/PMS_WebServiceList.asmx/GetProductDetailsList", OtherDetails, "QuotationTable", HeaderCaption, "QuotationIndex", "SetQuotation");
}

function SetQuotation(Id, Name, e) {
    // debugger;
    finishedproductlist = 0;
    var QuotationID = Id;
    var QuotationName = Name;

    if (QuotationID != "") {

        var data = QuotationID.split('|');
        QuotationID = data[0];

        $('#QuotationlistModel').modal('hide');
        btnQuotation.SetText(QuotationName);
        $('#hdnQuotation_ID').val(QuotationID);
        // document.getElementById('hdnQuotation_ID').value = Id;
        $('#btnQuotation').select();
        $('#btnQuotation').focus();
    }
}

var Contractlinkindex = 0;
function OpenEstimateList(s, e) {
    slno = gridBOQResourcesList.GetDataRow(globalrowindex).children[0].innerHTML.trim();
    gridBOQResourcesList.batchEditApi.StartEdit(globalrowindex, ResProductNameIndex);
    var ProductName = gridBOQResourcesList.GetEditor('ProductName').GetText();
    if (ProductName == "") {
        jAlert("Please select product before select Product!");
        return false;
    }
    else {
        GetContractList('', slno);
        Contractlinkindex = 1;
        setTimeout(function () { $("#txtBOMName").focus(); }, 500);
        $('#GridBOMlistModel').modal('show');

    }
}

function EstimateKeyDown(s, e) {
    //console.log(e.htmlEvent.key);
    if (e.htmlEvent.key == "Enter") {
        s.OnButtonClick(0);
    }
}

function CustomerButnClick(s, e) {
    //debugger;
    if ($('#ddlSchema').val() != "") {   //$('#ddlSchema').val()!=null ||
        $('#CustModel').modal('show');
    }
    else {
        jAlert("Please select numbering scheme.");
        $("#ddlSchema").focus();
        $("#ddlSchema").focus();
    }
}

function Customer_KeyDown(s, e) {
    if (e.htmlEvent.key == "Enter" || e.code == "NumpadEnter") {
        if ($('#ddlSchema').val() != null || $('#ddlSchema').val() != "") {
            $('#CustModel').modal('show');
            $("#txtCustSearch").focus();
        }
        else {
            jAlert("Please select numbering scheme.");
            $('#ddlSchema').focus();
        }
    }
}

function Customerkeydown(e) {
    var OtherDetails = {}
    if ($.trim($("#txtCustSearch").val()) == "" || $.trim($("#txtCustSearch").val()) == null) {
        return false;
    }
    OtherDetails.SearchKey = $("#txtCustSearch").val();
    // OtherDetails.BranchID = $('#ddl_Branch').val();

    if (e.code == "Enter" || e.code == "NumpadEnter") {
        var HeaderCaption = [];
        HeaderCaption.push("Name");
        HeaderCaption.push("Unique Id");
        HeaderCaption.push("Address");

        if ($("#txtCustSearch").val() != "") {
            callonServer("../Models/CustomAddress.asmx/GetCustomer", OtherDetails, "CustomerTable", HeaderCaption, "customerIndex", "SetCustomer");
        }
    }
    else if (e.code == "ArrowDown") {
        if ($("input[customerindex=0]"))
            $("input[customerindex=0]").focus();
    }
}


function SetCustomer(id, Name) {

    var key = id;
    $('#CustomerId').val(id)
    if (key != null && key != '') {
        $('#CustModel').modal('hide');
        // ClstContactBy.SetText(Name);
        //$("#CustomerTxt").val(Name);
        CustomerTxt.SetText(Name);
        CustomerTxt.SetFocus();
        // e.customArgs["Customer_ID"] = id;
        //  gridBOQProductEntryList.Refresh();
        // gridBOQResourcesList.Refresh();



        for (var i = 500; i > -500; i--) {
            if (gridBOQProductEntryList.GetRow(i)) {
                gridBOQProductEntryList.DeleteRow(i);
            }
        }

        for (var i = 500; i > -500; i--) {
            if (gridBOQResourcesList.GetRow(i)) {
                gridBOQResourcesList.DeleteRow(i);
            }
        }

        EstimateGridLookup.GetGridView().Refresh();
        addNewRowToEditgrid();
        AddNewRowGridResources();
        // $('#hdnDetailsID').val('');
    }
}

    var projectCode = [];
function ProjectSelectionChanged(s, e) {
    //debugger;
    //  ProjectGridLookup.gridView.GetSelectedFieldValues("Proj_Id", GetProjectSelectedFieldValuesCallback);

    var projId = ProjectGridLookup.GetValue();
    $.ajax({
        type: "POST",
        url: "@Url.Action("getHierarchyID", "BillofQuantities")",
        data: { ProjID: projId },
    success: function (response) {
        if (response != null) {
            //jAlert(response.Message);
            $('#ddlHierarchy').val(response.Message);
        }
    }
});
}

function GetProjectSelectedFieldValuesCallback(values) {
    try {
        projectCode = [];
        for (var i = 0; i < values.length; i++) {
            projectCode.push(values[i]);
        }
    } finally {
        console.log(projectCode);
    }
}

function ProjectStartCallback(s, e) {
    //debugger;
    e.customArgs["Customer_ID"] = $("#CustomerId").val();
    e.customArgs["Project_ID"] = '@ViewBag.ProjectID';
    e.customArgs["TagProject_ID"] = taggedProjectId;
    // e.customArgs["Proj_Code"] = ProjectGridLookup.GetSelectedKeyFieldValues();
    if ('@ViewBag.Unit' != "") {
        e.customArgs["Unit"] = '@ViewBag.Unit';
    }
    else {
        e.customArgs["Unit"] = $("#ddlBankBranch").val();
    }
}
function ProjectLookupValChange() {
    ProjectGridLookup.GetGridView().Refresh();
    //  ProjectGridLookup.GetGridView().Refresh();
}

var ContractID = [];
function ContractSelectionChanged(s, e) {
    ContractGridLookup.gridView.GetSelectedFieldValues("Details_ID", GetContractSelectedFieldValuesCallback);
}

function GetContractSelectedFieldValuesCallback(values) {
    try {
        ContractID = [];
        for (var i = 0; i < values.length; i++) {
            ContractID.push(values[i]);
        }
    } finally {
        console.log(ContractID);
    }
}

var TaggingModule = "";

function OnBOQTaggingProductCallback(s, e) {
    debugger;
    var BOQDate = GetServerDateFormat(BOQDate_dt.GetValue());
    var Unit = $('#ddlBankBranch option:selected').val();
    e.customArgs["Unit"] = Unit;
    e.customArgs["Customer_ID"] = $("#CustomerId").val();
    @*e.customArgs["ContractNo"] = '@ViewBag.ContractNo';*@
    if (TaggingModule == "Estimate") {
        e.customArgs["EstimateCode"] = EstimateGridLookup.GetSelectedKeyFieldValues();
    }
else if (TaggingModule == "Proposal") {
    e.customArgs["EstimateCode"] = ProposalGridLookup.GetSelectedKeyFieldValues();
}
    // '@ViewBag.Estimatecode' = EstimateGridLookup.GetSelectedKeyFieldValues();
    e.customArgs["ProposalId"] = ProposalGridLookup.GetSelectedKeyFieldValues();
    e.customArgs["BOQDate"] = BOQDate;
    e.customArgs["Module"] = TaggingModule;
}

var Estimatecode = "";

function EstimateStartCallback(s, e) {
    // $("#hdnLead_Id").val(id);
    // debugger;
    var BOQDate = GetServerDateFormat(BOQDate_dt.GetValue());
    var Unit = $('#ddlBankBranch option:selected').val();
    $('#hdnDetailsID').val('');
    e.customArgs["Unit"] = Unit;
    e.customArgs["Customer_ID"] = $("#CustomerId").val();
    e.customArgs["ContractNo"] = '@ViewBag.ContractNo';
    if ('@ViewBag.ProjectID' != null) {
        e.customArgs["EstimateCode"] = '@ViewBag.ProjectID';
    }
    else {
        e.customArgs["EstimateCode"] = EstimateGridLookup.GetSelectedKeyFieldValues();
    }
    e.customArgs["BOQDate"] = BOQDate;

    e.customArgs["ApprovRevSettings"] = $('#hdnApproveRevSettings').val();

    // TagProductGrid.Refresh();
}

function ProposalStartCallback(s, e) {
    // debugger;
    var BOQDate = GetServerDateFormat(BOQDate_dt.GetValue());
    var Unit = $('#ddlBankBranch option:selected').val();
    $('#hdnDetailsID').val('');
    e.customArgs["Unit"] = Unit;
    e.customArgs["Customer_ID"] = $("#CustomerId").val();
    e.customArgs["ContractNo"] = '@ViewBag.ContractNo';
    @*if ('@ViewBag.ProjectID' != null) {
        e.customArgs["Proposal"] = '@ViewBag.ProjectID';
    }
else {*@
e.customArgs["ProposalId"] = ProposalGridLookup.GetSelectedKeyFieldValues();
    //}
    e.customArgs["BOQDate"] = BOQDate;

}

function EstimateSelectionChanged(s, e) {
    ProdTagID = [];
    gridBOQProductEntryList.Refresh();
    gridBOQResourcesList.Refresh();
    Estimatecode = EstimateGridLookup.GetSelectedKeyFieldValues();
    if (Estimatecode != "") {
        $.ajax({
            type: "POST",
            //url: "Url.Action("GetEmployeesTargetByCode", "EmployeesTarget")",
            url: "@Url.Action("DoActivity", "BillofQuantities")",
            data: { Module_Name: "Estimate", Module_id: 0, Estimate_code: Estimatecode },
        success: function (response) {
            PMScrPopUp.SetContentHtml(response);
            MVCxClientUtils.FinalizeCallback();
            PMScrPopUp.SetHeaderText('Select Products');
            PMScrPopUp.Show();
            TaggingModule = "Estimate";
            $("#hdnTagModule").val('Estimate');
        }
    });
}
else {
            ProposalGridLookup.SetEnabled(true);
}
}


function ProposalSelectionChanged(s, e) {
    ProdTagID = [];
    gridBOQProductEntryList.Refresh();
    gridBOQResourcesList.Refresh();
    Proposalcode = ProposalGridLookup.GetSelectedKeyFieldValues();
    if (Proposalcode != "") {
        $.ajax({
            type: "POST",
            //url: "Url.Action("GetEmployeesTargetByCode", "EmployeesTarget")",
            url: "@Url.Action("DoActivity", "BillofQuantities")",
            data: { Module_Name: "Proposal", Module_id: 0, Estimate_code: Proposalcode },
        success: function (response) {
            PMScrPopUp.SetContentHtml(response);
            MVCxClientUtils.FinalizeCallback();
            PMScrPopUp.SetHeaderText('Select Products');
            PMScrPopUp.Show();
            TaggingModule = "Proposal";
            $("#hdnTagModule").val('Proposal');
        }
    });
}
else {
            EstimateGridLookup.SetEnabled(true);
}
}





var balQty = 0;

function GetBalQty(id) {

    $.ajax({
        type: "POST",
        url: "@Url.Action("EstimateBalQty", "BillofQuantities")",
        data: { detailsid: id },
    success: function (response) {
        if (response != null) {
            //jAlert(response.Message);
            if (response.Success) {
                balQty = response.Message;
            }
        }
    }
});
}


var taggedProjectId = "";

function SetTagProduct() {
    taging = "true";
    rowtime = 0;
    rowtime2 = 0;
    PMScrPopUp.Hide();
    PMScrPopUp.Hide();
    TagProductGrid.Refresh();
    gridBOQProductEntryList.Refresh();
    gridBOQResourcesList.Refresh();
    // AddNewRowWithSl();

    $.ajax({
        type: "POST",
        url: "@Url.Action("GetTagDetails", "BillofQuantities")",
        data: { detailsid: EstimateGridLookup.GetSelectedKeyFieldValues(), Tagid: ProdTagID },
    success: function (response) {
        if (response != null) {
            //jAlert(response.Message);
            if (response.Success) {
                var data = response.Message.split('~');
                $('#ddl_AmountAre').val(data[0]);
                //setTimeout(function () {
                //    $('#txtGridProductEntryTotalAmount').val(data[1].trim().toString());
                //}, 600);
                $('#txtGridResourcesTotalAmount').val(data[2]);

                //ProjectGridLookup.gridView.SelectItemsByKey(data[3]);

                taggedProjectId = data[3];
                $("#ddl_AmountAre").prop('disabled', 'disabled');
            }
        }
    }
});

setTimeout(function () { var noofrow = gridBOQResourcesList.GetVisibleRowsOnPage(); if (noofrow > 1) { $('#showResources').click(); } }, 800);
resuffleSerial();
resufflegrid2Serial();
ProjectGridLookup.GetGridView().Refresh();
ProjectGridLookup.GetGridView().Refresh();
ProdTotalamt();
if (TaggingModule == "Proposal") {
    EstimateGridLookup.SetEnabled(false);
}
else if (TaggingModule == "Estimate") {
    ProposalGridLookup.SetEnabled(false);
}
}

function ContractStartCallback(s, e) {
    //debugger;
    e.customArgs["Customer_ID"] = $("#CustomerId").val();
    e.customArgs["ContractNo"] = '@ViewBag.ContractNo';
    if ('@ViewBag.Unit' != "") {
        e.customArgs["Unit"] = '@ViewBag.Unit';
    }
    else {
        e.customArgs["Unit"] = $("#ddlBankBranch").val();
    }
    e.customArgs["ProjectID"] = ProjectGridLookup.GetSelectedKeyFieldValues();

    e.customArgs["ApprovRevSettings"] = $('#hdnApproveRevSettings').val();
}
function ContractLookupValChange() {
    ContractGridLookup.GetGridView().Refresh();
}

function EstimateLookupValChange() {
    EstimateGridLookup.GetGridView().Refresh();
}

function ProposalLookupValChange() {
    ProposalGridLookup.GetGridView().Refresh();
}

function gridProductBeginCallback(s, e) {
    e.customArgs["TagingID"] = ProdTagID;
}

function OnTagSelectionChanged(s, e) {
    //TagProductGrid.gridView.GetSelectedFieldValues("Details_ID", GetTagProdSelectedFieldValuesCallback);
    s.GetSelectedFieldValues("Details_ID", GetTagProdSelectedFieldValuesCallback);
}
function GetTagProdSelectedFieldValuesCallback(values) {
    try {
        ProdTagID = [];
        for (var i = 0; i < values.length; i++) {
            ProdTagID.push(values[i]);
        }
    } finally {
        console.log(ProdTagID);
    }
}


    function gridcustombuttonclick(s, e) {
        // alert(s);
        OpenAddlDesc(s, e);
    }

function BOQApprove(mode) {
    //debugger;
    var flag = true;
    var Actions = '';
    if ($("#txt_ApproveRemarks").val() == "") {
        flag = false;
        $("#txt_ApproveRemarks").focus();
    }
    else {
        if (mode == 'Approve') {
            jConfirm('Do you want to approve BOQ?', 'Confirm Dialog', function (r) {
                if (r == true) {
                    @*Actions = "ApproveEstimateData";
                    flag = true;

                    var detailsid = $("#hdnDetailsID").val();
                    var ApproveRemarks = $("#txt_ApproveRemarks").val();

                    $.ajax({
                        type: "POST",
                        url: "@Url.Action("ApproveEstimateDataByID", "Estimate")",
                        data: { detailsid: detailsid, Approve_Remarks: ApproveRemarks, Action: Actions },
                    success: function (response) {
                        if (response != null) {
                            jAlert(response.Message);
                            if (response.Success) {
                                setTimeout(function () {
                                    var url = $('#hdnEstimateListPage').val();
                                    window.location.href = url;
                                }, 500);
                            }
                        }
                    }
                });*@
                    $("#hdnApproveReject").val('Approve');// = "Approve";
                BOQSave('Exit');
            }
            else {
                    flag = false;
            return false;
        }
    });
}
else if (mode == 'Reject') {
    jConfirm('Do you want to reject BOQ?', 'Confirm Dialog', function (r) {
        if (r == true) {
            @*Actions = "RejectEstimateData";
            flag = true;
            var detailsid = $("#hdnDetailsID").val();
            var ApproveRemarks = $("#txt_ApproveRemarks").val();

            $.ajax({
                type: "POST",
                url: "@Url.Action("ApproveEstimateDataByID", "Estimate")",
                data: { detailsid: detailsid, Approve_Remarks: ApproveRemarks, Action: Actions },
            success: function (response) {
                if (response != null) {
                    jAlert(response.Message);
                    if (response.Success) {
                        setTimeout(function () {
                            var url = $('#hdnEstimateListPage').val();
                            window.location.href = url;
                        }, 500);
                    }
                }
            }
        });*@
                    $("#hdnApproveReject").val('Reject');// = "Reject";
        BOQSave('Exit');
    }
    else {
            flag = false;
    return false;
}
});
}
}
return flag;
}
$(document).ready(function () {
    //Toggle fullscreen expandEntryGrid
    $("#expandgridBOQProductEntryList").click(function (e) {
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
            gridBOQProductEntryList.SetHeight(browserHeight - 150);
            gridBOQProductEntryList.SetWidth(cntWidth);
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
            gridBOQProductEntryList.SetHeight(300);
            var cntWidth = $this.parent('.makeFullscreen').width();
            gridBOQProductEntryList.SetWidth(cntWidth);
        }
    });
    $("#expandgridBOQResourcesList").click(function (e) {
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
            gridBOQResourcesList.SetHeight(browserHeight - 150);
            gridBOQResourcesList.SetWidth(cntWidth);
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
            gridBOQResourcesList.SetHeight(300);
            var cntWidth = $this.parent('.makeFullscreen').width();
            gridBOQResourcesList.SetWidth(cntWidth);
        }
    });
});

function Remarkstab() {
    setTimeout(function () {
        gridBOQProductEntryList.batchEditApi.EndEdit();
        gridBOQProductEntryList.batchEditApi.StartEdit(-1, PrdSrlIndex);
    }, 5);
}
