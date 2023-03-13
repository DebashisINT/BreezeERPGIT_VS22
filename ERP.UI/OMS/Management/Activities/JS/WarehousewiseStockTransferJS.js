//==========================================================Revision History ============================================================================================
//    1.0   Priti   V2.0.36   23-01-2023    0025602: Available Stock & UOM Conversion tab is required in Warehouse wise Stock transfer module
//    2.0   Priti   V2.0.37   13-03-2023    0025602: Stock with multiple batches are not allowing to enter in Warehouse wise Stock Transfer
//========================================== End Revision History =======================================================================================================


var globalRowIndex;
var saveNewOrExit = '';
var canCallBack = true;
var RowCount = 0;
var alertShow = false;
var SelectWarehouse = "0";
var SelectBatch = "0";
var SelectSerial = "0";
var SelectedWarehouseID = "0";
var textSeparator = ";";
var selectedChkValue = "";

function GetVisibleIndex(s, e) {
    globalRowIndex = e.visibleIndex;
}

document.onkeydown = function (e) {
    if (event.keyCode == 83 && event.altKey == true && !alertShow) { //run code for Alt + n -- ie, Save & New  
        if ($('#hdAddEdit').val() == "Add") {
            SaveButtonClick();
        }
    }
    else if (event.keyCode == 88 && event.altKey == true && !alertShow) { //run code for Ctrl+X -- ie, Save & Exit!     

        SaveExitButtonClick();
    }

}
$(document).ready(function () {

    $('#SourceWarehouseModel').on('shown.bs.modal', function () {
        $('#txtSourceWarehouseSearch').focus();
    })
    $('#DestinationWarehouseModel').on('shown.bs.modal', function () {
        $('#txtDestinationWarehouseSearch').focus();
    })
    $('#ProductModel').on('shown.bs.modal', function () {
        $('#txtProdSearch').focus();
    })


    var mode = $('#hdAddEdit').val();
    if (mode == 'Edit') {
        //$("#ddlBranch").prop("disabled", false);
        //$("#ddlBranchTo").prop("disabled", false);
        

    }
    $('#txRemarks_I').blur(function () {
        if (grid.GetVisibleRowsOnPage() == 1) {
            grid.batchEditApi.StartEdit(-1, 2);
        }
    })

    var ShowUOMConversionInEntry = $('#hdnShowUOMConversionInEntry').val();
    if(ShowUOMConversionInEntry != "1")
    {
        liAltQty.style.display = 'none';
    }
    

    var TypeReturn = $('#hdnTypeReturn').val();
    if (mode == 'Return') {
       
        if ($("#hdnProjectSelectInEntryModule").val() == "1") {
            clookup_Project.gridView.Refresh();
        }
        $("#ddlBranch").focus();
    }
});

function ddlBranch_SelectedIndexChanged()
{
    clookup_Project.gridView.Refresh();
   
}
function ddlBranchTo_SelectedIndexChanged()
{
    clookup_ToProject.gridView.Refresh();
}
function ParentCustomerOnClose(InternalID, EntityCode) {  

    AspxDirectAddCustPopup.Hide();    
    //if (InternalID != "") {
    //    ctxtEntity.SetText(EntityCode);
    //    GetObjectID('hdnEntityId').value = InternalID;
    //}
    
}
function AddEntityClick() {

    var url = '/OMS/Management/Master/SrvMastEntity.aspx?id=ADD&status=Y';
    AspxDirectAddCustPopup.SetContentUrl(url);
    AspxDirectAddCustPopup.Show();

}
function AddBatchNew(s, e) {
    var keyCode = ASPxClientUtils.GetKeyCode(e.htmlEvent);
    if ((keyCode === 13)) {
        grid.batchEditApi.StartEdit(globalRowIndex);
        var Product = (grid.GetEditor('ProductName').GetValue() != null) ? grid.GetEditor('ProductName').GetValue() : "";

        var SourceWarehouse = (grid.GetEditor('SourceWarehouse').GetValue() != null) ? grid.GetEditor('SourceWarehouse').GetValue() : "";
        var SourceWarehouseID = (grid.GetEditor('SourceWarehouseID').GetValue() != null) ? grid.GetEditor('SourceWarehouseID').GetValue() : "";

        var DestinationWarehouse = (grid.GetEditor('DestinationWarehouse').GetValue() != null) ? grid.GetEditor('DestinationWarehouse').GetValue() : "";
        var DestinationWarehouseID = (grid.GetEditor('DestinationWarehouseID').GetValue() != null) ? grid.GetEditor('DestinationWarehouseID').GetValue() : "";

        if(Product != "")
        {
            grid.batchEditApi.EndEdit();
            grid.AddNewRow();
            RowCount = RowCount + 1;
            grid.GetEditor("SrlNo").SetText(grid.GetVisibleItemsOnPage());
            
            
            if ($('#hdnWarehouseRepeatStockTransfer').val() == "1") {
                grid.batchEditApi.StartEdit(globalRowIndex);

                grid.GetEditor("SourceWarehouseID").SetText(SourceWarehouseID);
                grid.GetEditor("SourceWarehouse").SetText(SourceWarehouse);
               
                grid.GetEditor("DestinationWarehouseID").SetText(DestinationWarehouseID);
                grid.GetEditor("DestinationWarehouse").SetText(DestinationWarehouse);
            }

            //grid.batchEditApi.EndEdit();
            //grid.batchEditApi.StartEdit(globalRowIndex, 2);
            
        }
        else
        {
            jAlert("Select Product Name.");

        }
        
        //setTimeout(function () { grid.batchEditApi.StartEdit(globalRowIndex, 2); }, 200);
    }
   
}
function fn_PopOpen() {
    var url = '/OMS/management/Store/Master/ProductPopup.html?var=3.99';
    cPosView.SetContentUrl(url);
    cPosView.RefreshContentUrl();

    cPosView.Show();

}

function fn_productSave() {
    cPosView.Hide();
}


function ShowUOMPOpup()
{
    grid.batchEditApi.StartEdit(globalRowIndex);
    var Quantity = grid.GetEditor("TransferQuantity").GetValue();
    var strProductID = grid.GetEditor("ProductID").GetValue();
    var splitData = strProductID.split('||@||');
    var BranchId = $('#ddlBranch').val();
    var Amount = '';
    var Rate = '';

    grid.batchEditApi.EndEdit();
    var actionQry = '';
    var TransferId = '';
    var ConvertionOverideVisible = $('#hdnConvertionOverideVisible').val();
    var ShowUOMConversionInEntry = $('#hdnShowUOMConversionInEntry').val();
    var type = 'add';

    if ($('#hdAddEdit').val() != "Edit") {

        actionQry = 'WSTPackingQtyAdd';
        $.ajax({
            type: "POST",
            url: "Services/Master.asmx/GetMultiUOMDetails",
            data: JSON.stringify({ orderid: splitData[0], action: actionQry, module: 'WarehouseWiseStockTransfer', strKey: TransferId }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (msg) {

                var SpliteDetails = msg.d.split("||@||");
                var IsInventory = '';
                if (SpliteDetails[5] == "1") {
                    IsInventory = 'Yes';
                }
                var gridprodqty = '';
                var gridPackingQty = '';
                var slno = (grid.GetEditor('SrlNo').GetValue() != null) ? grid.GetEditor('SrlNo').GetValue() : "";
                var strProductID = splitData[0];

                var isOverideConvertion = SpliteDetails[4];
                var packing_saleUOM = SpliteDetails[2];
                var sProduct_SaleUom = SpliteDetails[3];
                var sProduct_quantity = SpliteDetails[0];
                var packing_quantity = SpliteDetails[1];

                var gridprodqty = grid.GetEditor("TransferQuantity").GetValue();

                if (ShowUOMConversionInEntry == "1" && SpliteDetails.length > 5) {
                    ShowUOM(type, "Warehouse Wise Stock Transfer", 0, sProduct_quantity, sProduct_SaleUom, packing_quantity, packing_saleUOM, strProductID, slno, isOverideConvertion, gridprodqty, gridPackingQty);
                }

            }
        });
    }
    else {
        actionQry = 'WSTPackingQtyEdit';
        TransferId = $('#hdAdjustmentId').val();
        $.ajax({
            type: "POST",
            url: "Services/Master.asmx/GetMultiUOMDetails",
            data: JSON.stringify({ orderid: splitData[0], action: actionQry, module: 'WarehouseWiseStockTransfer', strKey: TransferId }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (msg) {

                var SpliteDetails = msg.d.split("||@||");
                var IsInventory = '';
                if (SpliteDetails[5] == "1") {
                    IsInventory = 'Yes';
                }
                var gridprodqty = '';
                var gridPackingQty = '';
                var slno = (grid.GetEditor('SrlNo').GetValue() != null) ? grid.GetEditor('SrlNo').GetValue() : "";
                var strProductID = splitData[0];

                var isOverideConvertion = SpliteDetails[4];
                var packing_saleUOM = SpliteDetails[2];
                var sProduct_SaleUom = SpliteDetails[3];
                var sProduct_quantity = SpliteDetails[0];
                var packing_quantity = SpliteDetails[1];


                gridPackingQty = SpliteDetails[6];
                gridprodqty = SpliteDetails[7];

                if (ShowUOMConversionInEntry == "1" && SpliteDetails.length > 5) {
                    ShowUOM(type, "Warehouse Wise Stock Transfer", 0, sProduct_quantity, sProduct_SaleUom, packing_quantity, packing_saleUOM, strProductID, slno, isOverideConvertion, gridprodqty, gridPackingQty);
                }
                else {
                    actionQry = 'WSTPackingQtyAdd';
                    $.ajax({
                        type: "POST",
                        url: "Services/Master.asmx/GetMultiUOMDetails",
                        data: JSON.stringify({ orderid: splitData[0], action: actionQry, module: 'WarehouseWiseStockTransfer', strKey: TransferId }),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        async: false,
                        success: function (msg) {

                            var SpliteDetails = msg.d.split("||@||");
                            var IsInventory = '';
                            if (SpliteDetails[5] == "1") {
                                IsInventory = 'Yes';
                            }
                            var gridprodqty = '';
                            var gridPackingQty = '';
                            var slno = (grid.GetEditor('SrlNo').GetValue() != null) ? grid.GetEditor('SrlNo').GetValue() : "";
                            var strProductID = splitData[0];

                            var isOverideConvertion = SpliteDetails[4];
                            var packing_saleUOM = SpliteDetails[2];
                            var sProduct_SaleUom = SpliteDetails[3];
                            var sProduct_quantity = SpliteDetails[0];
                            var packing_quantity = SpliteDetails[1];

                            var gridprodqty = grid.GetEditor("TransferQuantity").GetValue();

                            if (ShowUOMConversionInEntry == "1" && SpliteDetails.length > 5) {
                                ShowUOM(type, "Warehouse Wise Stock Transfer", 0, sProduct_quantity, sProduct_SaleUom, packing_quantity, packing_saleUOM, strProductID, slno, isOverideConvertion, gridprodqty, gridPackingQty);
                            }

                        }
                    });
                }


            }
        });
    }
}
function QuantityTextChangelOST() {
}

function QuantityTextChange() {
        grid.batchEditApi.StartEdit(globalRowIndex);
        var Quantity = grid.GetEditor("TransferQuantity").GetValue();
        var strProductID = grid.GetEditor("ProductID").GetValue();

        var splitData = strProductID.split('||@||');
        var BranchId = $('#ddlBranch').val();
        var Amount = '';
        var Rate = '';    

        grid.batchEditApi.EndEdit();
        var actionQry = '';
        var TransferId = '';
        var ConvertionOverideVisible = $('#hdnConvertionOverideVisible').val();
        var ShowUOMConversionInEntry = $('#hdnShowUOMConversionInEntry').val();
        var type = 'add';

        if ($('#hdAddEdit').val() != "Edit") {

            actionQry = 'WSTPackingQtyAdd';
            $.ajax({
                type: "POST",
                url: "Services/Master.asmx/GetMultiUOMDetails",
                data: JSON.stringify({ orderid: splitData[0], action: actionQry, module: 'WarehouseWiseStockTransfer', strKey: TransferId }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (msg) {

                    var SpliteDetails = msg.d.split("||@||");
                    var IsInventory = '';
                    if (SpliteDetails[5] == "1") {
                        IsInventory = 'Yes';
                    }
                    var gridprodqty = '';
                    var gridPackingQty = '';
                    var slno = (grid.GetEditor('SrlNo').GetValue() != null) ? grid.GetEditor('SrlNo').GetValue() : "";
                    var strProductID = splitData[0];

                    var isOverideConvertion = SpliteDetails[4];
                    var packing_saleUOM = SpliteDetails[2];
                    var sProduct_SaleUom = SpliteDetails[3];
                    var sProduct_quantity = SpliteDetails[0];
                    var packing_quantity = SpliteDetails[1];

                    var gridprodqty = grid.GetEditor("TransferQuantity").GetValue();

                    if (ShowUOMConversionInEntry == "1" && SpliteDetails.length > 5) {
                        ShowUOM(type, "Warehouse Wise Stock Transfer", 0, sProduct_quantity, sProduct_SaleUom, packing_quantity, packing_saleUOM, strProductID, slno, isOverideConvertion, gridprodqty, gridPackingQty);
                    }
                    else {
                        var uniqueIndex = globalRowIndex;
                        SetTotalTaxableAmount(uniqueIndex, 12);
                    }

                }
            });
        }
        else {
            actionQry = 'WSTPackingQtyEdit';
            TransferId = $('#hdAdjustmentId').val();
            $.ajax({
                type: "POST",
                url: "Services/Master.asmx/GetMultiUOMDetails",
                data: JSON.stringify({ orderid: splitData[0], action: actionQry, module: 'WarehouseWiseStockTransfer', strKey: TransferId }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (msg) {

                    var SpliteDetails = msg.d.split("||@||");
                    var IsInventory = '';
                    if (SpliteDetails[5] == "1") {
                        IsInventory = 'Yes';
                    }
                    var gridprodqty = '';
                    var gridPackingQty = '';
                    var slno = (grid.GetEditor('SrlNo').GetValue() != null) ? grid.GetEditor('SrlNo').GetValue() : "";
                    var strProductID = splitData[0];

                    var isOverideConvertion = SpliteDetails[4];
                    var packing_saleUOM = SpliteDetails[2];
                    var sProduct_SaleUom = SpliteDetails[3];
                    var sProduct_quantity = SpliteDetails[0];
                    var packing_quantity = SpliteDetails[1];


                    gridPackingQty = SpliteDetails[6];
                    gridprodqty = SpliteDetails[7];

                    if (ShowUOMConversionInEntry == "1" && SpliteDetails.length > 5) {
                        ShowUOM(type, "Warehouse Wise Stock Transfer", 0, sProduct_quantity, sProduct_SaleUom, packing_quantity, packing_saleUOM, strProductID, slno, isOverideConvertion, gridprodqty, gridPackingQty);

                    }
                    else
                    {
                        actionQry = 'WSTPackingQtyAdd';
                        $.ajax({
                            type: "POST",
                            url: "Services/Master.asmx/GetMultiUOMDetails",
                            data: JSON.stringify({ orderid: splitData[0], action: actionQry, module: 'WarehouseWiseStockTransfer', strKey: TransferId }),
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            async: false,
                            success: function (msg) {

                                var SpliteDetails = msg.d.split("||@||");
                                var IsInventory = '';
                                if (SpliteDetails[5] == "1") {
                                    IsInventory = 'Yes';
                                }
                                var gridprodqty = '';
                                var gridPackingQty = '';
                                var slno = (grid.GetEditor('SrlNo').GetValue() != null) ? grid.GetEditor('SrlNo').GetValue() : "";
                                var strProductID = splitData[0];

                                var isOverideConvertion = SpliteDetails[4];
                                var packing_saleUOM = SpliteDetails[2];
                                var sProduct_SaleUom = SpliteDetails[3];
                                var sProduct_quantity = SpliteDetails[0];
                                var packing_quantity = SpliteDetails[1];

                                var gridprodqty = grid.GetEditor("TransferQuantity").GetValue();

                                if (ShowUOMConversionInEntry == "1" && SpliteDetails.length > 5) {
                                    ShowUOM(type, "Warehouse Wise Stock Transfer", 0, sProduct_quantity, sProduct_SaleUom, packing_quantity, packing_saleUOM, strProductID, slno, isOverideConvertion, gridprodqty, gridPackingQty);
                                }
                                else {
                                    var uniqueIndex = globalRowIndex;
                                    SetTotalTaxableAmount(uniqueIndex, 12);
                                }

                            }
                        });
                    }

                }
            });
        }

        var ValuationRateWHStockTransfer = $('#hdnValuationRateWHStockTransfer').val();
        if (ValuationRateWHStockTransfer != "0")
        {
            $.ajax({
                type: "POST",
                url: "WarehousewiseStockTransferAdd.aspx/GetStockValuation",
                data: JSON.stringify({ ProductId: splitData[0] }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (msg) {


                    var ObjData = msg.d;
                    if (ObjData.length > 0) {
                        $.ajax({
                            type: "POST",
                            url: "WarehousewiseStockTransferAdd.aspx/GetStockValuationAmount",
                            data: JSON.stringify({ Pro_Id: splitData[0], Qty: Quantity, Valuationsign: ObjData, Fromdate: cdtTDate.date.format('yyyy-MM-dd'), BranchId: BranchId }),
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            async: false,
                            success: function (msg1) {
                                var ObjData1 = msg1.d;
                                if (ObjData1.length > 0) {
                                    Amount = (ObjData1 * 1);
                                    Rate = Amount / Quantity;
                                    var tbRate = grid.GetEditor("Rate");
                                    tbRate.SetValue(Rate);


                                    var tbValue = grid.GetEditor("Value");
                                    tbValue.SetValue(Amount);
                                }
                            }

                        });
                    }
                }

            });
        }

       

        setTimeout(function () {
            grid.batchEditApi.StartEdit(globalRowIndex, 12);
        }, 200); 

}
function QuantityTextChangeGotFocus() {
    grid.batchEditApi.StartEdit(globalRowIndex);
    var Quantity = grid.GetEditor("TransferQuantity").GetValue();
    var strProductID = grid.GetEditor("ProductID").GetValue();

    var splitData = strProductID.split('||@||');
    var BranchId = $('#ddlBranch').val();
    var Amount = '';
    var Rate = '';

   // grid.batchEditApi.EndEdit();
    var actionQry = '';
    var TransferId = '';
    var ConvertionOverideVisible = $('#hdnConvertionOverideVisible').val();
    var ShowUOMConversionInEntry = $('#hdnShowUOMConversionInEntry').val();
    var type = 'add';

    if ($('#hdAddEdit').val() != "Edit") {

        actionQry = 'WSTPackingQtyAdd';
        $.ajax({
            type: "POST",
            url: "Services/Master.asmx/GetMultiUOMDetails",
            data: JSON.stringify({ orderid: splitData[0], action: actionQry, module: 'WarehouseWiseStockTransfer', strKey: TransferId }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
           // async: false,
            success: function (msg) {

                var SpliteDetails = msg.d.split("||@||");
                var IsInventory = '';
                if (SpliteDetails[5] == "1") {
                    IsInventory = 'Yes';
                }
                var gridprodqty = '';
                var gridPackingQty = '';
                var slno = (grid.GetEditor('SrlNo').GetValue() != null) ? grid.GetEditor('SrlNo').GetValue() : "";
                var strProductID = splitData[0];

                var isOverideConvertion = SpliteDetails[4];
                var packing_saleUOM = SpliteDetails[2];
                var sProduct_SaleUom = SpliteDetails[3];
                var sProduct_quantity = SpliteDetails[0];
                var packing_quantity = SpliteDetails[1];

                var gridprodqty = grid.GetEditor("TransferQuantity").GetValue();

                if (ShowUOMConversionInEntry == "1" && SpliteDetails.length > 5) {
                    ShowUOM(type, "Warehouse Wise Stock Transfer", 0, sProduct_quantity, sProduct_SaleUom, packing_quantity, packing_saleUOM, strProductID, slno, isOverideConvertion, gridprodqty, gridPackingQty);
                }

            }
        });
    }
    else {
        actionQry = 'WSTPackingQtyEdit';
        TransferId = $('#hdAdjustmentId').val();
        $.ajax({
            type: "POST",
            url: "Services/Master.asmx/GetMultiUOMDetails",
            data: JSON.stringify({ orderid: splitData[0], action: actionQry, module: 'WarehouseWiseStockTransfer', strKey: TransferId }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            //async: false,
            success: function (msg) {

                var SpliteDetails = msg.d.split("||@||");
                var IsInventory = '';
                if (SpliteDetails[5] == "1") {
                    IsInventory = 'Yes';
                }
                var gridprodqty = '';
                var gridPackingQty = '';
                var slno = (grid.GetEditor('SrlNo').GetValue() != null) ? grid.GetEditor('SrlNo').GetValue() : "";
                var strProductID = splitData[0];

                var isOverideConvertion = SpliteDetails[4];
                var packing_saleUOM = SpliteDetails[2];
                var sProduct_SaleUom = SpliteDetails[3];
                var sProduct_quantity = SpliteDetails[0];
                var packing_quantity = SpliteDetails[1];


                gridPackingQty = SpliteDetails[6];
                gridprodqty = SpliteDetails[7];

                if (ShowUOMConversionInEntry == "1" && SpliteDetails.length > 5) {
                    ShowUOM(type, "Warehouse Wise Stock Transfer", 0, sProduct_quantity, sProduct_SaleUom, packing_quantity, packing_saleUOM, strProductID, slno, isOverideConvertion, gridprodqty, gridPackingQty);

                }
                else {
                    actionQry = 'WSTPackingQtyAdd';
                    $.ajax({
                        type: "POST",
                        url: "Services/Master.asmx/GetMultiUOMDetails",
                        data: JSON.stringify({ orderid: splitData[0], action: actionQry, module: 'WarehouseWiseStockTransfer', strKey: TransferId }),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                       // async: false,
                        success: function (msg) {

                            var SpliteDetails = msg.d.split("||@||");
                            var IsInventory = '';
                            if (SpliteDetails[5] == "1") {
                                IsInventory = 'Yes';
                            }
                            var gridprodqty = '';
                            var gridPackingQty = '';
                            var slno = (grid.GetEditor('SrlNo').GetValue() != null) ? grid.GetEditor('SrlNo').GetValue() : "";
                            var strProductID = splitData[0];

                            var isOverideConvertion = SpliteDetails[4];
                            var packing_saleUOM = SpliteDetails[2];
                            var sProduct_SaleUom = SpliteDetails[3];
                            var sProduct_quantity = SpliteDetails[0];
                            var packing_quantity = SpliteDetails[1];

                            var gridprodqty = grid.GetEditor("TransferQuantity").GetValue();

                            if (ShowUOMConversionInEntry == "1" && SpliteDetails.length > 5) {
                                ShowUOM(type, "Warehouse Wise Stock Transfer", 0, sProduct_quantity, sProduct_SaleUom, packing_quantity, packing_saleUOM, strProductID, slno, isOverideConvertion, gridprodqty, gridPackingQty);
                            }

                        }
                    });
                }

            }
        });
    }

    var ValuationRateWHStockTransfer = $('#hdnValuationRateWHStockTransfer').val();
    if (ValuationRateWHStockTransfer != "0") {
        $.ajax({
            type: "POST",
            url: "WarehousewiseStockTransferAdd.aspx/GetStockValuation",
            data: JSON.stringify({ ProductId: splitData[0] }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            //  async: false,
            success: function (msg) {


                var ObjData = msg.d;
                if (ObjData.length > 0) {
                    $.ajax({
                        type: "POST",
                        url: "WarehousewiseStockTransferAdd.aspx/GetStockValuationAmount",
                        data: JSON.stringify({ Pro_Id: splitData[0], Qty: Quantity, Valuationsign: ObjData, Fromdate: cdtTDate.date.format('yyyy-MM-dd'), BranchId: BranchId }),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        async: false,
                        success: function (msg1) {
                            var ObjData1 = msg1.d;
                            if (ObjData1.length > 0) {
                                Amount = (ObjData1 * 1);
                                Rate = Amount / Quantity;
                                var tbRate = grid.GetEditor("Rate");
                                tbRate.SetValue(Rate);


                                var tbValue = grid.GetEditor("Value");
                                tbValue.SetValue(Amount);
                            }
                        }

                    });
                }
            }

        });
    }

    //setTimeout(function () {
    //    grid.batchEditApi.StartEdit(globalRowIndex, 13);
    //}, 200);

}

function StockValuation()
{
    grid.batchEditApi.StartEdit(globalRowIndex);
    var Quantity = grid.GetEditor("TransferQuantity").GetValue();
    var strProductID = grid.GetEditor("ProductID").GetValue();

    var splitData = strProductID.split('||@||');
    var BranchId = $('#ddlBranch').val();
    var Amount = '';
    var Rate = '';

    //grid.batchEditApi.EndEdit();
    //var actionQry = '';
    //var TransferId = '';
    //var ConvertionOverideVisible = $('#hdnConvertionOverideVisible').val();
    //var ShowUOMConversionInEntry = $('#hdnShowUOMConversionInEntry').val();
    //var type = 'add';

    $.ajax({
        type: "POST",
        url: "WarehousewiseStockTransferAdd.aspx/GetStockValuation",
        data: JSON.stringify({ ProductId: splitData[0] }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (msg) {


            var ObjData = msg.d;
            if (ObjData.length > 0) {
                $.ajax({
                    type: "POST",
                    url: "WarehousewiseStockTransferAdd.aspx/GetStockValuationAmount",
                    data: JSON.stringify({ Pro_Id: splitData[0], Qty: Quantity, Valuationsign: ObjData, Fromdate: cdtTDate.date.format('yyyy-MM-dd'), BranchId: BranchId }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: false,
                    success: function (msg1) {
                        var ObjData1 = msg1.d;
                        if (ObjData1.length > 0) {
                            Amount = (ObjData1 * 1);
                            Rate = Amount / Quantity;
                            var tbRate = grid.GetEditor("Rate");
                            tbRate.SetValue(Rate);


                            var tbValue = grid.GetEditor("Value");
                            tbValue.SetValue(Amount);

                            var uniqueIndex = globalRowIndex;
                            SetTotalTaxableAmount(uniqueIndex, 16);
                           
                        }
                        else
                        {
                            var uniqueIndex = globalRowIndex;
                            SetTotalTaxableAmount(uniqueIndex, 12);
                        }
                    }

                });
            }
        }

    });

   

}


var issavePacking = 0;
var module = '2nd ';
function SetDataToGrid(Quantity, packing, PackingUom, PackingSelectUom, productid, slno) {
    //console.log(packing);
    issavePacking = 1;
    grid.batchEditApi.StartEdit(globalRowIndex);
    grid.GetEditor('TransferQuantity').SetValue(Quantity);
    var BranchId = $('#ddlBranch').val();
    var Amount = '';
    var Rate = '';
    if (parseFloat(grid.GetEditor("TransferQuantity").GetValue()) > parseFloat(grid.GetEditor("AvlStkSourceWH").GetValue())) {

        $.ajax({
            type: "POST",
            url: "WarehousewiseStockTransferAdd.aspx/GetNegativeStockByProductID",
            data: JSON.stringify({ ProductId: productid }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (msg) {
                var data = msg.d;
                //Rev Subhra 06-08-2019
                $('#hdnStockTransfer').val(data.trim());
                //End of Rev
                if (data.trim() == 'W') {
                    jConfirm('Product is going negative do you want to proceed?', 'Confirmation Dialog', function (r) {
                        if (r == true) {

                            //$.ajax({
                            //    type: "POST",
                            //    url: "WarehousewiseStockTransferAdd.aspx/GetStockValuation",
                            //    data: JSON.stringify({ ProductId: productid }),
                            //    contentType: "application/json; charset=utf-8",
                            //    dataType: "json",
                            //    async: false,
                            //    success: function (msg) {


                            //        var ObjData = msg.d;
                            //        if (ObjData.length > 0) {
                            //            $.ajax({
                            //                type: "POST",
                            //                url: "WarehousewiseStockTransferAdd.aspx/GetStockValuationAmount",
                            //                data: JSON.stringify({ Pro_Id: productid, Qty: Quantity, Valuationsign: ObjData, Fromdate: cdtTDate.date.format('yyyy-MM-dd'), BranchId: BranchId }),
                            //                contentType: "application/json; charset=utf-8",
                            //                dataType: "json",
                            //                async: false,
                            //                success: function (msg1) {
                            //                    var ObjData1 = msg1.d;
                            //                    if (ObjData1.length > 0) {
                            //                        Amount = (ObjData1 * 1);
                            //                        Rate = Amount / Quantity;

                            //                        var tbRate = grid.GetEditor("Rate");
                            //                        tbRate.SetValue(Rate);


                            //                        var tbValue = grid.GetEditor("Value");
                            //                        tbValue.SetValue(Amount);

                            //                    }
                            //                }

                            //            });
                            //        }
                            //    }

                            //});
                            var ValuationRateWHStockTransfer = $('#hdnValuationRateWHStockTransfer').val();
                            if (ValuationRateWHStockTransfer != "0") {

                                StockValuation();
                            }
                            else {
                               // grid.batchEditApi.EndEdit();
                                //setTimeout(function () {
                                //    grid.batchEditApi.StartEdit(globalRowIndex, 12);
                                //}, 200);

                                var uniqueIndex = globalRowIndex;
                                SetTotalTaxableAmount(uniqueIndex, 12);
                            }
                            


                        }
                        else {
                            grid.batchEditApi.StartEdit(globalRowIndex);
                            grid.GetEditor("TransferQuantity").SetValue("0.0000");
                            $("#txtQuantity").val("0.0000");
                            $("#txtPacking").val("0.0000");
                            grid.batchEditApi.EndEdit();
                            ShowUOMPOpup();
                            //setTimeout(function () {
                            //    grid.batchEditApi.StartEdit(globalRowIndex, 11);
                            //}, 200);
                        }
                    });
                }
                else if (data.trim() == 'B') {
                    jAlert('Product is going negative can not proceed');
                    grid.GetEditor("TransferQuantity").SetValue("0.0000");
                    $("#txtQuantity").val("0.0000");
                    $("#txtPacking").val("0.0000");
                    grid.batchEditApi.EndEdit();
                    //setTimeout(function () {
                    //    grid.batchEditApi.StartEdit(globalRowIndex, 12);
                    //}, 200);
                    var uniqueIndex = globalRowIndex;
                    SetTotalTaxableAmount(uniqueIndex, 12);
                }
                else {
                    grid.batchEditApi.EndEdit();
                    $.ajax({
                        type: "POST",
                        url: "WarehousewiseStockTransferAdd.aspx/GetStockValuation",
                        data: JSON.stringify({ ProductId: productid }),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        async: false,
                        success: function (msg) {


                            var ObjData = msg.d;
                            if (ObjData.length > 0) {
                                $.ajax({
                                    type: "POST",
                                    url: "WarehousewiseStockTransferAdd.aspx/GetStockValuationAmount",
                                    data: JSON.stringify({ Pro_Id: productid, Qty: Quantity, Valuationsign: ObjData, Fromdate: cdtTDate.date.format('yyyy-MM-dd'), BranchId: BranchId }),
                                    contentType: "application/json; charset=utf-8",
                                    dataType: "json",
                                    async: false,
                                    success: function (msg1) {
                                        var ObjData1 = msg1.d;
                                        if (ObjData1.length > 0) {
                                            Amount = (ObjData1 * 1);
                                            Rate = Amount / Quantity;

                                            var tbRate = grid.GetEditor("Rate");
                                            tbRate.SetValue(Rate);


                                            var tbValue = grid.GetEditor("Value");
                                            tbValue.SetValue(Amount);
                                        }
                                    }

                                });
                            }
                        }

                    });

                    //setTimeout(function () {
                    //    grid.batchEditApi.StartEdit(globalRowIndex, 16);
                    //}, 200);

                    var uniqueIndex = globalRowIndex;
                    SetTotalTaxableAmount(uniqueIndex, 12);
                }

            }
        });

        

    }
    else
    {
        $('#hdnStockTransfer').val('NA');
        var ValuationRateWHStockTransfer = $('#hdnValuationRateWHStockTransfer').val();
        if (ValuationRateWHStockTransfer != "0") {
            StockValuation();
        }
        else
        {
            var uniqueIndex = globalRowIndex;
            SetTotalTaxableAmount(uniqueIndex, 12);
        }
       // grid.batchEditApi.EndEdit();
        //setTimeout(function () {
        //    grid.batchEditApi.StartEdit(globalRowIndex, 12);
        //}, 200);

       

    }


   
    


}
function RateTextChange() {
    grid.batchEditApi.StartEdit(globalRowIndex);
    var TransferQuantity = parseFloat(grid.GetEditor("TransferQuantity").GetValue());
    var Rate = parseFloat(grid.GetEditor("Rate").GetValue());
    grid.GetEditor("Value").SetValue(TransferQuantity * Rate);
    //grid.batchEditApi.EndEdit();
    //setTimeout(function () {
    //    grid.batchEditApi.StartEdit(globalRowIndex, 16);
    //}, 200);

    var uniqueIndex = globalRowIndex;
    SetTotalTaxableAmount(uniqueIndex, 16);
}

function GridAddnewRow() {
    grid.AddNewRow();
    RowCount = RowCount + 1;
    grid.GetEditor("SrlNo").SetText(grid.GetVisibleItemsOnPage());
   // setTimeout(function () { grid.batchEditApi.StartEdit(globalRowIndex, 1); }, 500);
}

function AllControlInitilize() {
    if (canCallBack) {

        if ($('#hdAddEdit').val() == "Add") {
            cCmbScheme.Focus();
            GridAddnewRow();
            setTimeout(function () { cCmbScheme.Focus(); }, 500);
            cbtnSaveRecords.SetVisible(true)
            cbtn_SaveRecords.SetVisible(true)

        } else {
            RowCount = parseInt($("#HiddenRowCount").val());
            //GridAddnewRow();         

            SuffleRows();
            cbtnSaveRecords.SetVisible(false);
            cbtn_SaveRecords.SetVisible(true);
        }

        canCallBack = false;
    }
}
function ProductPriceCalculate() {
    if ((grid.GetEditor('Rate').GetValue() == null || grid.GetEditor('Rate').GetValue() == 0)) {
        var _price = 0;
        var _Qty = grid.GetEditor('TransferQuantity').GetValue();
        var _Amount = grid.GetEditor('Value').GetValue();
        _price = (_Amount / _Qty);
        grid.GetEditor('Rate').SetValue(_price);
    }
}
function AmountTextChange(s, e) {
    ProductPriceCalculate();
}
function SetUOMDataInArray(WHType, srlno, prodid) {
    var productid = prodid.split("||@||")[0];
    if (WHType == 'CustomDelete') {
        var Mainarr = $.grep(aarr, function (element) { return element.productid != productid && element.slno != srlno });
        var i = 0;
        var j = 0;
        $.each(Mainarr, function (index, value) {
            i = i + 1;
            value.slno = i;
        });
        aarr = Mainarr;
    }
  

}
function gridCustomButtonClick(s, e) {
    debugger;
    if (e.buttonID == 'CustomDelete') {
        if (grid.GetVisibleRowsOnPage() > 1) {
            grid.batchEditApi.StartEdit(e.visibleIndex);
            //Rev Subhra 05-08-2019
            SetUOMDataInArray('CustomDelete', grid.GetEditor('SrlNo').GetValue(), grid.GetEditor('ProductID').GetText());
            //End of Rev 
            grid.DeleteRow(e.visibleIndex);
            SuffuleSerialNumber();
            var uniqueIndex = globalRowIndex;
            SetTotalTaxableAmount(uniqueIndex, 16);
        }
    }
    else if (e.buttonID == 'CustomAddNewRow') {
        GridAddnewRow();
        //grid.batchEditApi.StartEdit(globalRowIndex, 2);
        //grid.batchEditApi.StartEdit(globalRowIndex);
        //grid.batchEditApi.EndEdit();
        //setTimeout(function () {
        //    grid.batchEditApi.StartEdit(globalRowIndex, 2);
        //}, 200);
       
       
    }

        // Mantis Issue 24428
    else if (e.buttonID == 'CustomMultiUOM') {

        var index = e.visibleIndex;
        grid.batchEditApi.StartEdit(e.visibleIndex);
        var Productdetails = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
        var ProductID = Productdetails.split("||@||")[0];
        var UOMName = grid.GetEditor("SaleUOM").GetValue();
        var quantity = grid.GetEditor("TransferQuantity").GetValue();
        var DetailsId = 0;           ///grid.GetEditor('DetailsId').GetValue();
        var StockUOM = Productdetails.split("||@||")[5];
        ///rev bapi
        hdProductID.value = ProductID;
        //End Rev Bapi
        if (StockUOM == "") {
            StockUOM = "0";
        }

        //$("#AltUOMQuantity").val(parseFloat(0).toFixed(4));
        cAltUOMQuantity.SetValue("0.0000");
        ///rev bapi
        //  if ((ProductID != "") && (UOMName != "") && (quantity != "0.0000")) {
        if ((ProductID != "") && (UOMName != "")) {
            //End Rev Bapi
            if (StockUOM == "0") {
                jAlert("Main Unit Not Defined.");
            }
            else {
                if ($("#hddnMultiUOMSelection").val() == "1") {
                    ccmbUOM.SetEnabled(false);
                    var index = e.visibleIndex;
                    grid.batchEditApi.StartEdit(e.visibleIndex, 8);
                    //grid.batchEditApi.StartEdit(globalRowIndex);
                    var Qnty = grid.GetEditor("TransferQuantity").GetValue();
                    var SrlNo = (grid.GetEditor('SrlNo').GetValue() != null) ? grid.GetEditor('SrlNo').GetValue() : "";
                    var UomId = grid.GetEditor('ProductID').GetText().split("||@||")[7];
                    ccmbUOM.SetValue(UomId);
                    // Mantis Issue 24428
                    //$("#UOMQuantity").val(Qnty);
                    $("#UOMQuantity").val("0.0000");
                    ccmbBaseRate.SetValue(0)
                    cAltUOMQuantity.SetValue(0)
                    ccmbAltRate.SetValue(0)
                    ccmbSecondUOM.SetValue("")
                    // End of Mantis Issue 24428
                    cPopup_MultiUOM.Show();
                    cgrid_MultiUOM.cpDuplicateAltUOM = "";
                    // if ($("#hdnPageStatus").val() != "update") {
                    AutoPopulateMultiUOM();
                    //}
                    //chinmoy change start
                    cgrid_MultiUOM.PerformCallback('MultiUOMDisPlay~' + SrlNo + '~' + DetailsId);
                    //cgrid_MultiUOM.PerformCallback('MultiUOMDisPlay~' + ProductID);
                }     //End
            }
        }
        else {
            return;
        }
    }

        // End of Mantis Issue 24428


    else if (e.buttonID == 'CustomWarehouse') {
        var index = e.visibleIndex;
        grid.batchEditApi.StartEdit(index, 2)
        //var inventoryType = (document.getElementById("ddlInventory").value != null) ? document.getElementById("ddlInventory").value : "";

        //if (inventoryType == "C" || inventoryType == "Y") {
        Warehouseindex = index;
        var SrlNo = (grid.GetEditor('SrlNo').GetValue() != null) ? grid.GetEditor('SrlNo').GetValue() : "";
        var ProductID = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
        var QuantityValue = (grid.GetEditor('TransferQuantity').GetValue() != null) ? grid.GetEditor('TransferQuantity').GetValue() : "0";
        var SourceWarehouseID = (grid.GetEditor('SourceWarehouseID').GetValue() != null) ? grid.GetEditor('SourceWarehouseID').GetValue() : "0";

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
            //var StkQuantityValue = QuantityValue * strMultiplier;
            var StkQuantityValue = QuantityValue;
            var Ptype = SpliteDetails[3];
            $('#hdfProductType').val(Ptype);

            cCmbWarehouse.SetEnabled(false);


            document.getElementById("lblProductName").innerHTML = strProductName;
            document.getElementById("txt_SalesAmount").innerHTML = QuantityValue;
            document.getElementById("txt_SalesUOM").innerHTML = strUOM;
            document.getElementById("txt_StockAmount").innerHTML = StkQuantityValue;
            document.getElementById("txt_StockUOM").innerHTML = strStkUOM;

            $('#hdfProductID').val(strProductID);
            $('#hdfProductSerialID').val(SrlNo);
            $('#hdfProductSerialID').val(SrlNo);
            $('#hdnProductQuantity').val(QuantityValue);


            //REV 1.0
            if (grid.GetEditor("ProductID").GetValue() != "" && grid.GetEditor("ProductID").GetValue() != null) {
                var SpliteDetails = ProductID.split("||@||");
                var strProductID = SpliteDetails[0];
                var sl = grid.GetEditor("SrlNo").GetValue();
                var branch = $("#ddlBranch").val();
                GETAVAILABLESTOCK(sl, strProductID, branch, SourceWarehouseID);
            }
            var Packing_UOM = SpliteDetails[9];
            var sProduct_quantity = SpliteDetails[6];
            var packing_quantity = SpliteDetails[5];
            var htmlfactor = "";
            htmlfactor = parseFloat(sProduct_quantity).toFixed(4) + " ";
            htmlfactor = htmlfactor + " " + strUOM + " = " + parseFloat(packing_quantity).toFixed(4) + " " + Packing_UOM;
            $('#lbluomfactor1').text(htmlfactor);
            //END REV 1.0
            if (Ptype == "W") {
                div_Warehouse.style.display = 'block';
                div_Batch.style.display = 'none';
                div_Serial.style.display = 'none';
                div_Quantity.style.display = 'block';
                cCmbWarehouse.PerformCallback('BindWarehouse~' + SourceWarehouseID);
                cGrdWarehouse.PerformCallback('Display~' + SrlNo);
                $("#ADelete").css("display", "block");//Subhabrata
                SelectedWarehouseID = "0";
                // cPopup_Warehouse.Show();
                jAlert("No Batch or Serial is activated !");
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
                // cPopup_Warehouse.Show();
                jAlert("No Warehouse is activaed !");
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
                // cPopup_Warehouse.Show();
                jAlert("No Warehouse is activaed !");
            }
            else if (Ptype == "WB") {
                div_Warehouse.style.display = 'block';
                div_Batch.style.display = 'block';
                div_Serial.style.display = 'none';
                div_Quantity.style.display = 'block';
                cCmbWarehouse.PerformCallback('BindWarehouse~' + SourceWarehouseID);
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
                cCmbWarehouse.PerformCallback('BindWarehouse~' + SourceWarehouseID);
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
                cCmbWarehouse.PerformCallback('BindWarehouse~' + SourceWarehouseID);
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
                jAlert("No Warehouse or Batch or Serial is activaed !");
            }
        }
        else if (ProductID != "" && parseFloat(QuantityValue) == 0) {

            jAlert("Please enter Quantity !");
        }
    }
    else {

        jAlert("You have selected Non-Inventory Item, so You cannot updated Stock.");
    }
    // }
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

        cCmbWarehouse.PerformCallback('BindWarehouse~' + strWarehouse);
        cCmbBatch.PerformCallback('BindBatch~' + strWarehouse);
        checkListBox.PerformCallback('EditSerial~' + strWarehouse + '~' + strBatchID + '~' + strSrlID);

        cCmbWarehouse.SetValue(strWarehouse);
        ctxtQuantity.SetValue(strQuantity);
    }
}
function fn_Edit(keyValue) {
    SelectedWarehouseID = keyValue;
    cCallbackPanel.PerformCallback('EditWarehouse~' + keyValue);
}
function fn_Deletecity(keyValue) {
    var WarehouseID = (cCmbWarehouse.GetValue() != null) ? cCmbWarehouse.GetValue() : "0";
    var BatchID = (cCmbBatch.GetValue() != null) ? cCmbBatch.GetValue() : "0";

    cGrdWarehouse.PerformCallback('Delete~' + keyValue);
    checkListBox.PerformCallback('BindSerial~' + WarehouseID + '~' + BatchID);
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
function CmbWarehouseEndCallback(s, e) {
    if (SelectWarehouse != "0") {
        cCmbWarehouse.SetValue(SelectWarehouse);
        SelectWarehouse = "0";
    }
    else {
        //cCmbWarehouse.SetEnabled(true);
        cCmbWarehouse.SetEnabled(false);
    }
}
function CmbBatch_ValueChange() {
    var WarehouseID = cCmbWarehouse.GetValue();
    var BatchID = cCmbBatch.GetValue();
    var type = document.getElementById('hdfProductType').value;
    var strProductID = $("#hdfProductID").val();
    var sl = grid.GetEditor("SrlNo").GetValue();
    var branch = $("#ddlBranch").val();
    if (type == "WBS") {
        checkListBox.PerformCallback('BindSerial~' + WarehouseID + '~' + BatchID);
    }
    else if (type == "BS") {
        checkListBox.PerformCallback('BindSerial~' + "0" + '~' + BatchID);
    }

    if (BatchID != null) {
        GetWirehouseBatchWiseAviableStock(sl, strProductID, branch, WarehouseID, BatchID);
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
    //checkComboBox.SetText(selectedItems.length + " Items");

    var itemsCount = GetSelectedItemsCount(selectedItems);
    checkComboBox.SetText(itemsCount + " Items");

    var val = GetSelectedItemsText(selectedItems);
    $("#abpl").attr('data-content', val);
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
function GetSelectedItemsText(items) {
    var texts = [];
    for (var i = 0; i < items.length; i++)
        if (items[i].index != 0)
            texts.push(items[i].text);
    return texts.join(textSeparator);
}
function GetSelectedItemsCount(items) {
    var texts = [];
    for (var i = 0; i < items.length; i++)
        if (items[i].index != 0)
            texts.push(items[i].text);
    return texts.length;
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
$(function () {
    $('[data-toggle="popover"]').popover();
})

// Mantis Issue 24428 

function OnAddNewClick() {
    grid.AddNewRow();

    var noofvisiblerows = grid.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
    var tbQuotation = grid.GetEditor("SrlNo");
    tbQuotation.SetValue(noofvisiblerows);
    /// Mantis Issue 24428 
    $("#UOMQuantity").val(0);
    Uomlength = 0;
    // End of Mantis Issue 24428 
}

function FinalMultiUOM() {
    debugger;
    UomLenthCalculation();
    if (Uomlength == 0 || Uomlength < 0) {
      
        jAlert("Please add atleast one Alt. Quantity with Update Row as checked.");
        
        return;
    }
    else {
        cPopup_MultiUOM.Hide();
        var SLNo = grid.GetEditor('SrlNo').GetValue();
        cgrid_MultiUOM.PerformCallback('SetBaseQtyRateInGrid~' + SLNo);
        setTimeout(function () {
            grid.batchEditApi.StartEdit(globalRowIndex, 11);
        }, 200)
    }
}
var Uomlength = 0;
function UomLenthCalculation() {
    grid.batchEditApi.StartEdit(globalRowIndex);
    var SLNo = grid.GetEditor('SrlNo').GetValue();

    $.ajax({
        type: "POST",
        url: "WarehousewiseStockTransferAdd.aspx/GetQuantityfromSL",
        data: JSON.stringify({ SLNo: SLNo }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (msg) {

            Uomlength = msg.d;

        }
    });
}
function closeMultiUOM(s, e) {
    e.cancel = false;
    // cPopup_MultiUOM.Hide();
}



function PopulateMultiUomAltQuantity() {
    grid.batchEditApi.StartEdit(globalRowIndex);
    var otherdet = {};
    var Quantity = $("#UOMQuantity").val();
    otherdet.Quantity = Quantity;
    var UomId = ccmbUOM.GetValue();
    otherdet.UomId = UomId;
    var Productdetails = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
    var ProductID = Productdetails.split("||@||")[0];
    otherdet.ProductID = ProductID;
    var AltUomId = ccmbSecondUOM.GetValue();
    otherdet.AltUomId = AltUomId;

    $.ajax({
        type: "POST",
        url: "SalesInquiryAdd.aspx/GetPackingQuantity",
        data: JSON.stringify(otherdet),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (msg) {
            // debugger;
            if (msg.d.length != 0) {
                var packingQuantity = msg.d[0].packing_quantity;
                var sProduct_quantity = msg.d[0].sProduct_quantity;
            }
            else {
                var packingQuantity = 0;
                var sProduct_quantity = 0;
            }
            var uomfactor = 0
            if (sProduct_quantity != 0 && packingQuantity != 0) {
                uomfactor = parseFloat(packingQuantity / sProduct_quantity).toFixed(4);
                $('#hddnuomFactor').val(parseFloat(packingQuantity / sProduct_quantity));
            }
            else {
                $('#hddnuomFactor').val(0);
            }

            var uomfac_Qty_to_stock = $('#hddnuomFactor').val();
            var Qty = $("#UOMQuantity").val();
            var calcQuantity = parseFloat(Qty * uomfac_Qty_to_stock).toFixed(4);

            //$("#AltUOMQuantity").val(calcQuantity);
            cAltUOMQuantity.SetValue(calcQuantity);

        }
    });
}

var hdMultiUOMID = "";
function OnMultiUOMEndCallback(s, e) {
   
    if (cgrid_MultiUOM.cpDuplicateAltUOM == "DuplicateAltUOM") {
        jAlert("Please Enter Different Alt. Quantity.");
        return;
    }
    // Mantis Issue 24428
    if (cgrid_MultiUOM.cpSetBaseQtyRateInGrid != null && cgrid_MultiUOM.cpSetBaseQtyRateInGrid == "1") {
        grid.batchEditApi.StartEdit(globalRowIndex, 6);

        var BaseQty = cgrid_MultiUOM.cpBaseQty;
        var BaseRate = cgrid_MultiUOM.cpBaseRate;

        grid.GetEditor("TransferQuantity").SetValue(BaseQty);
        grid.GetEditor("Rate").SetValue(BaseRate);
        grid.GetEditor("Value").SetValue(BaseQty * BaseRate)


        grid.GetEditor("Order_AltQuantity").SetValue(cgrid_MultiUOM.cpAltQty);
        grid.GetEditor("Order_AltUOM").SetValue(cgrid_MultiUOM.cpAltUom);

    }

    if (cgrid_MultiUOM.cpAllDetails == "EditData") {
        var Quan = (cgrid_MultiUOM.cpBaseQty).toFixed(4);
        $('#UOMQuantity').val(Quan);
       //$('#UOMQuantity').val(cgrid_MultiUOM.cpBaseQty);
        ccmbBaseRate.SetValue(cgrid_MultiUOM.cpBaseRate)
        ccmbSecondUOM.SetValue(cgrid_MultiUOM.cpAltUom);
        cAltUOMQuantity.SetValue(cgrid_MultiUOM.cpAltQty);
        ccmbAltRate.SetValue(cgrid_MultiUOM.cpAltRate);
        hdMultiUOMID = cgrid_MultiUOM.cpuomid;
        if (cgrid_MultiUOM.cpUpdatedrow == true) {
            $("#chkUpdateRow").prop('checked', true);
            $("#chkUpdateRow").attr('checked', 'checked');



        }
        else {
            $("#chkUpdateRow").prop('checked', false);
            $("#chkUpdateRow").removeAttr("checked");
        }

    }

    // End of Mantis Issue 24428
    if (cgrid_MultiUOM.cpOpenFocus == "OpenFocus") {
        ccmbSecondUOM.SetFocus();
    }

}


function CalcBaseQty() {


    //var PackingQtyAlt = Productdetails.split("||@||")[5];  // Alternate UOM selected from Product Master (tbl_master_product_packingDetails.packing_quantity)
    //var PackingQty = Productdetails.split("||@||")[6];  // Alternate UOM selected from Product Master (tbl_master_product_packingDetails.sProduct_quantity)
   // var PackingSaleUOM = Productdetails.split("||@||")[8];  // Alternate UOM selected from Product Master (tbl_master_product_packingDetails.packing_saleUOM)
    

    var Productdetails = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
    var PackingQtyAlt = 0;
    var PackingQty = 0;
    var PackingSaleUOM = 0;


    var ProductID = Productdetails.split("||@||")[0];


    $.ajax({
        type: "POST",
        url: "PurchaseIndent.aspx/AutoPopulateAltQuantity",
        data: JSON.stringify({ ProductID: ProductID }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {

            if (msg.d.length != 0) {
                PackingQtyAlt = msg.d[0].packing_quantity;  // Alternate UOM selected from Product Master (tbl_master_product_packingDetails.packing_quantity)
                PackingQty = msg.d[0].sProduct_quantity;  // Alternate UOM selected from Product Master (tbl_master_product_packingDetails.sProduct_quantity)
                PackingSaleUOM = msg.d[0].AltUOMId;  // Alternate UOM selected from Product Master (tbl_master_product_packingDetails.packing_saleUOM)
            }
            else {
                PackingQtyAlt = 0;  // Alternate UOM selected from Product Master (tbl_master_product_packingDetails.packing_quantity)
                PackingQty = 0;  // Alternate UOM selected from Product Master (tbl_master_product_packingDetails.sProduct_quantity)
                PackingSaleUOM = 0;  // Alternate UOM selected from Product Master (tbl_master_product_packingDetails.packing_saleUOM)
            }

            if (PackingQtyAlt == "") {
                PackingQtyAlt = 0
            }
            if (PackingQty == "") {
                PackingQty = 0
            }

            // if Base UOM of product is not same as the Alternate UOM selected from Product Master, then Calculation of Base Quantity will not happen
            if (ccmbSecondUOM.GetValue() != PackingSaleUOM) {
                PackingQtyAlt = 0;
                PackingQty = 0;
            }

            var BaseQty = 0
            if (PackingQtyAlt > 0) {
                var ConvFact = PackingQty / PackingQtyAlt;
                var altQty = cAltUOMQuantity.GetValue();

                if (ConvFact > 0) {
                    var BaseQty = (altQty * ConvFact).toFixed(4);
                    $("#UOMQuantity").val(BaseQty);
                }
            }
            else {
                $("#UOMQuantity").val("0.0000");
            }
        }
    });

   

}

function CalcBaseRate() {
    var altQty = cAltUOMQuantity.GetValue();
    var altRate = ccmbAltRate.GetValue();
    var baseQty = $("#UOMQuantity").val();


    if (baseQty > 0) {
        var BaseRate = (altQty * altRate) / baseQty;
        ccmbBaseRate.SetValue(BaseRate);
    }
}


function SaveMultiUOM() {
    //debugger;
    var qnty = $("#UOMQuantity").val();
    var UomId = ccmbUOM.GetValue();
    var UomName = ccmbUOM.GetText();
    var AltQnty = cAltUOMQuantity.GetValue();
    var AltUomId = ccmbSecondUOM.GetValue();
    var AltUomName = ccmbSecondUOM.GetText();
    // Rev Sanchita
    grid.batchEditApi.StartEdit(globalRowIndex);
    // End of Rev Sanchita
    var srlNo = (grid.GetEditor('SrlNo').GetValue() != null) ? grid.GetEditor('SrlNo').GetValue() : "";
    var Productdetails = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
    var ProductID = Productdetails.split("||@||")[0];

    if (ProductID == "") {
        ProductID = hdProductID.value;
    }

    // Mantis Issue 24428
    var BaseRate = ccmbBaseRate.GetValue();
    var AltRate = ccmbAltRate.GetValue();

    var UpdateRow = 'False';
    if ($("#chkUpdateRow").prop("checked")) {
        UpdateRow = 'True';
    }
    // Rev Sanchita
    //if (srlNo != "" && qnty != "0.0000" && UomId != "" && UomName != "" && AltUomId != "" && ProductID != "" && AltUomId != null && AltUomName != ""
    //       && BaseRate != "0.0000" && AltRate != "0.0000") {
    if (srlNo != "" && UomId != "" && UomName != "" && AltUomId != "" && ProductID != "" && AltUomId != null && AltUomName != "" && AltQnty!="0.0000") {
        if ((qnty != "0" && UpdateRow == 'True') || (qnty == "0" && UpdateRow == 'False') || (qnty != "0" && UpdateRow == 'False')) {
            // End of Rev Sanchita
        if (cbtnMUltiUOM.GetText() == "Update") {
            cgrid_MultiUOM.PerformCallback('UpdateRow~' + srlNo + '~' + qnty + '~' + UomName + '~' + AltUomName + '~' + AltQnty + '~' + UomId + '~' + AltUomId + '~' + ProductID + '~' + BaseRate + '~' + AltRate + '~' + UpdateRow + '~' + hdMultiUOMID);
            cAltUOMQuantity.SetValue("0.0000");
            $("#UOMQuantity").val(0);
            ccmbBaseRate.SetValue(0);
            cAltUOMQuantity.SetValue(0);
            ccmbAltRate.SetValue(0);
            ccmbSecondUOM.SetValue("");
            cgrid_MultiUOM.cpAllDetails = "";
            cbtnMUltiUOM.SetText("Add");
            // Rev Sanchita
            $("#chkUpdateRow").prop('checked', false);
            $("#chkUpdateRow").removeAttr("checked");
            // End of Rev Sanchita

        }

        else {

        
            cgrid_MultiUOM.PerformCallback('SaveDisplay~' + srlNo + '~' + qnty + '~' + UomName + '~' + AltUomName + '~' + AltQnty + '~' + UomId + '~' + AltUomId + '~' + ProductID + '~' + BaseRate + '~' + AltRate + '~' + UpdateRow);
            cAltUOMQuantity.SetValue("0.0000");
            $("#UOMQuantity").val(0);
            ccmbBaseRate.SetValue(0)
            cAltUOMQuantity.SetValue(0)
            ccmbAltRate.SetValue(0)
            ccmbSecondUOM.SetValue("")
            // Rev Sanchita
            $("#chkUpdateRow").prop('checked', false);
            $("#chkUpdateRow").removeAttr("checked");
            // End of Rev Sanchita
        }
            // Rev Sanchita
        }
        else {
            return;
        }
        // End of Rev Sanchita
    }
    else {
        return;
    }

}

function AutoPopulateMultiUOM() {

    var Productdetails = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
    var ProductID = Productdetails.split("||@||")[0];
    var QuantityValue = (grid.GetEditor('TransferQuantity').GetValue() != null) ? grid.GetEditor('TransferQuantity').GetValue() : "0";

    $.ajax({
        type: "POST",
        url: "WarehousewiseStockTransferAdd.aspx/AutoPopulateAltQuantity",
        data: JSON.stringify({ ProductID: ProductID }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            if (msg.d.length != 0) {
                var packingQuantity = msg.d[0].packing_quantity;
                var sProduct_quantity = msg.d[0].sProduct_quantity;
                var AltUOMId = msg.d[0].AltUOMId;
            }
            else {
                var packingQuantity = 0;
                var sProduct_quantity = 0;
                var AltUOMId = 0;
            }
            var uomfactor = 0
            if (sProduct_quantity != 0 && packingQuantity != 0) {
                uomfactor = parseFloat(packingQuantity / sProduct_quantity).toFixed(4);
                $('#hddnuomFactor').val(parseFloat(packingQuantity / sProduct_quantity));
            }
            else {
                $('#hddnuomFactor').val(0);
            }

            var uomfac_Qty_to_stock = $('#hddnuomFactor').val();
            var Qty = QuantityValue;
            var calcQuantity = parseFloat(Qty * uomfac_Qty_to_stock).toFixed(4);
            if ($("#hdnPageStatus").val() == "update") {
                ccmbSecondUOM.SetValue('');
                //$("#AltUOMQuantity").val(calcQuantity);

                //cAltUOMQuantity.SetValue(calcQuantity);
                cAltUOMQuantity.SetValue("0.0000");
            }
            else {
                if (AltUOMId == 0) {
                    ccmbSecondUOM.SetValue('');
                }
                else {
                    ccmbSecondUOM.SetValue(AltUOMId);
                }
                cAltUOMQuantity.SetValue(calcQuantity);
            }

        }
    });
}




// Mantis Issue 24428
function Edit_MultiUom(keyValue, SrlNo) {

    cbtnMUltiUOM.SetText("Update");
    cgrid_MultiUOM.PerformCallback('EditData~' + keyValue + '~' + SrlNo);



}
function Delete_MultiUom(keyValue, SrlNo) {
    debugger;

    cgrid_MultiUOM.PerformCallback('MultiUomDelete~' + keyValue + '~' + SrlNo);

}

// End of Mantis Issue 24428






function txtserialTextChanged() {
    checkListBox.UnselectAll();
    var SerialNo = (ctxtserial.GetValue() != null) ? (ctxtserial.GetValue()) : "0";

    if (SerialNo != "0") {
        ctxtserial.SetValue("");
        var texts = [SerialNo];
        var values = GetValuesByTexts(texts);

        if (values.length > 0) {
            checkListBox.SelectValues(values);
            UpdateSelectAllItemState();
            UpdateText(); // for remove non-existing texts
            SaveWarehouse();
        }
        else {
            jAlert("This Serial Number does not exists.");
        }
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
            //texts.push(items[i].text);
            //vals.push(items[i].value);
        }
    }

    //WarehouseID, BatchID, SerialID, Qty=0.0
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
               
                cCmbWarehouse.PerformCallback('BindWarehouse~' + WarehouseID);
                 // Rev 2.0
                /* cCmbBatch.PerformCallback('BindBatch~' + "");*/
                cCmbBatch.PerformCallback('BindBatch~' + WarehouseID);
                 // Rev 2.0 End
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
            cCmbWarehouse.PerformCallback('BindWarehouse~' + WarehouseID);
             // Rev 2.0 
            /* cCmbBatch.PerformCallback('BindBatch~' + "");*/
            cCmbBatch.PerformCallback('BindBatch~' + WarehouseID);
             // Rev 2.0 End
            checkListBox.PerformCallback('BindSerial~' + "" + '~' + "");
            ctxtQuantity.SetValue("0");
        }
        UpdateText();
        cGrdWarehouse.PerformCallback('SaveDisplay~' + WarehouseID + '~' + WarehouseName + '~' + BatchID + '~' + BatchName + '~' + SerialID + '~' + SerialName + '~' + Qty + '~' + SelectedWarehouseID);
        SelectedWarehouseID = "0";
    }
}

function FinalWarehouse() {
    ccmbDestWarehouse.SetEnabled(false);
    ccmbSourceWarehouse.SetEnabled(false);
    cCmbScheme.SetEnabled(false);
    $("#hdnWHMandatory").val("Yes");
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
        grid.batchEditApi.StartEdit(Warehouseindex, 5);
    }
    else if (cGrdWarehouse.cpIsSave == "N") {
        jAlert('Quantity must be equal to Warehouse Quantity.');
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

function SuffuleSerialNumber() {
    var TotRowNumber = grid.GetVisibleItemsOnPage();
    var SlnoCount = 1;
    for (var i = 0; i < 1000; i++) {
        if (grid.GetRow(i)) {
            if (grid.GetRow(i).style.display != "none") {
                grid.batchEditApi.StartEdit(i, 2);
                grid.GetEditor("SrlNo").SetText(SlnoCount);
                SlnoCount++;
            }
        }
    }

    for (i = -1; i > -1000; i--) {
        if (grid.GetRow(i)) {
            if (grid.GetRow(i).style.display != "none") {
                grid.batchEditApi.StartEdit(i, 2);
                grid.GetEditor("SrlNo").SetText(SlnoCount);
                SlnoCount++;
            }
        }
    }
}

function GridEndCallBack(s, e) {

    alertShow = true;

    if (grid.cpSaveSuccessOrFail == "checkWarehouse") {
        var SrlNo = grid.cpProductSrlIDCheck;
        grid.cpSaveSuccessOrFail = null;
        var msg = " Batch/Serial Details must be entered for SL No. " + SrlNo;
        //OnAddNewClick();
        jAlert(msg);
    }
    else if (grid.cpSaveSuccessOrFail == "checkWarehouseQty") {
        var SrlNo = grid.cpProductSrlIDCheck;
        grid.cpSaveSuccessOrFail = null;
        var msg = "Product Quantity must be equal to Warehouse Quantity for SL No. " + SrlNo;
        jAlert(msg);
    }
    else if (grid.cpSaveSuccessOrFail == "duplicateProduct") {
        grid.cpSaveSuccessOrFail = null;
        jAlert('Can not Add Duplicate Product in the Warehouse Wise Stock Transfer.');
    }
    else if (grid.cpSaveSuccessOrFail == "AddLock") {
        grid.cpSaveSuccessOrFail = null;
        jAlert('DATA is Freezed between ' + grid.cpAddLockStatus + ' for Add.');
    }
    else if (grid.cpadjustmentId!="0"){
        if (grid.cpErrorCode == "0") {
            jAlert(grid.cpadjustmentNumber, "Alert", function () { afterSave(); alertShow = false; });
        }
        else  {
            jAlert(grid.cpadjustmentNumber, "Alert", function () { grid.batchEditApi.StartEdit(-1, 2); grid.batchEditApi.StartEdit(0, 2); alertShow = false; });
        }
    }

        
}
function ValidateEntry() {
    var ReturnValue = true;

    if (cCmbScheme.GetText().trim() == "" || cCmbScheme.GetText().trim() == "-Select-") {
        $('#MandatoryNumberingScheme').show();
        return false;
    }
    else {
        $('#MandatoryNumberingScheme').hide();
    }

    if (ctxtVoucherNo.GetText().trim() == "") {
        $('#MandatoryAdjNo').show();
        return false;
    } else {
        $('#MandatoryAdjNo').hide();
    }

    if ($('#ddlBranch').val() == "") {
        $('#MandatoryBranch').show();
        return false;
    }
    else {
        $('#MandatoryBranch').hide();
    }
    if ($('#ddlBranchTo').val() == "-Select-" || $('#ddlBranchTo').val() == "" || $('#ddlBranchTo').val() == "0") {
        $('#MandatoryBranchTo').show();
        return false;
    }
    else {
        $('#MandatoryBranchTo').hide();
    }
    //if (ccmbSourceWarehouse.GetText().trim() == "" || ccmbSourceWarehouse.GetText().trim() == "-Select-") {
    //    $('#MandatoryWarehouse').show();
    //    return false;
    //}
    //else {
    //    $('#MandatoryWarehouse').hide();
    //}    

    //if (ccmbDestWarehouse.GetText().trim() == "" || ccmbDestWarehouse.GetText().trim() == "-Select-") {
    //    $('#MandatorycmbDestWarehouse').show();
    //    return false;
    //}
    //else {
    //    $('#MandatorycmbDestWarehouse').hide();
    //}
    //var type = document.getElementById('hdfProductType').value;
    //if (type == "WBS" || type == "WB")
    //{
    //    var WHMandatory = document.getElementById('hdnWHMandatory').value;
    //    if(WHMandatory!='Yes')
    //    {
    //        jAlert("Batch/Serial Details must be entered.");
    //        return false;
    //    }
    //}


    for (var i = 0; i < 1000; i++) {
        if (grid.GetRow(i)) {
            if (grid.GetRow(i).style.display != "none") {
                grid.batchEditApi.StartEdit(i, 2);
                if (grid.GetEditor("ProductID").GetText() == "") {
                    //cLoadingPanelCRP.Hide();
                    jAlert("Please select a valid Product to proceed.");
                    return false;
                }

                if (grid.GetEditor("SourceWarehouseID").GetText() == "") {
                    //cLoadingPanelCRP.Hide();
                    jAlert("Please select Source Warehouse to proceed.");
                    return false;
                }
                if (grid.GetEditor("DestinationWarehouseID").GetText() == "") {
                    //cLoadingPanelCRP.Hide();
                    jAlert("Please select Destination Warehouse to proceed.");
                    return false;
                }

                if ((grid.GetEditor("TransferQuantity").GetText() == "0.0000" || grid.GetEditor("TransferQuantity").GetText() == "")) {
                    //cLoadingPanelCRP.Hide();
                    jAlert("Please enter a valid Quantity to proceed.");
                    return false;
                }
                if ($('#hdnStockTransfer').val() == 'B') {
                    jAlert('Product is going negative can not proceed');
                    grid.GetEditor("TransferQuantity").SetValue("0.0000");
                    $("#txtQuantity").val("0.0000");
                    $("#txtPacking").val("0.0000");
                    return false;
                }


            }
        }
    }

    for (i = -1; i > -1000; i--) {
        if (grid.GetRow(i)) {
            if (grid.GetRow(i).style.display != "none") {
                grid.batchEditApi.StartEdit(i, 2);
                if (grid.GetEditor("ProductID").GetText() == "") {
                    //cLoadingPanelCRP.Hide();
                    jAlert("Please select a valid Product to proceed.");
                    return false;
                }
                if (grid.GetEditor("SourceWarehouseID").GetText() == "") {
                    //cLoadingPanelCRP.Hide();
                    jAlert("Please select Source Warehouse to proceed.");
                    return false;
                }
                if (grid.GetEditor("DestinationWarehouseID").GetText() == "") {
                    //cLoadingPanelCRP.Hide();
                    jAlert("Please select Destination Warehouse to proceed.");
                    return false;
                }
                if ((grid.GetEditor("TransferQuantity").GetText() == "0.0000" || grid.GetEditor("TransferQuantity").GetText() == "")) {
                    // cLoadingPanelCRP.Hide();
                    jAlert("Please enter a valid Quantity to proceed.");
                    return false;
                }
                if ($('#hdnStockTransfer').val() == 'B') {
                    jAlert('Product is going negative can not proceed.');
                    grid.GetEditor("TransferQuantity").SetValue("0.0000");
                    $("#txtQuantity").val("0.0000");
                    $("#txtPacking").val("0.0000");
                    return false;
                }

            }
        }
    }
    return ReturnValue;
}

function SaveButtonClick() {

    var ProjectCode = clookup_Project.GetText();
    if ($("#hdnProjectSelectInEntryModule").val() == "1" && $("#hdnProjectMandatory").val() == "1" && ProjectCode == "") {
        jAlert("Please Select From Project.");
        return false;
    }

    var ProjectCode = clookup_ToProject.GetText();
    if ($("#hdnProjectSelectInEntryModule").val() == "1" && $("#hdnProjectMandatory").val() == "1" && ProjectCode == "") {
        jAlert("Please Select To Project.");
        return false;
    }
    if ($("#hdnMandatoryEntityWarehouseStockTransfer").val() == "1" && $("#hdnEntityId").val() == "" ) {
        jAlert("Please Select Entity.");
        return false;
    }

    saveNewOrExit = 'N';
    $('#HiddenSaveButton').val("N");
    if (ValidateEntry()) {
        if (!grid.InCallback()) {
            if (issavePacking == 1) {
                if (aarr.length > 0) {

                    $.ajax({
                        type: "POST",
                        url: "WarehousewiseStockTransferAdd.aspx/SetSessionPacking",
                        data: "{'list':'" + JSON.stringify(aarr) + "'}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (msg) {
                            grid.UpdateEdit();
                        }
                    });

                }

            }
            else
            {
                if (aarr.length > 0) {

                    $.ajax({
                        type: "POST",
                        url: "WarehousewiseStockTransferAdd.aspx/SetSessionPacking",
                        data: "{'list':'" + JSON.stringify(aarr) + "'}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (msg) {
                            grid.UpdateEdit();
                        }
                    });

                }
                else
                {
                    grid.UpdateEdit();
                }
            }
        }
    }
}
function SaveExitButtonClick() {


    var ProjectCode = clookup_Project.GetText();
    if ($("#hdnProjectSelectInEntryModule").val() == "1" && $("#hdnProjectMandatory").val() == "1" && ProjectCode == "") {
        jAlert("Please Select from Project.");
        return false;
    }

    var ProjectCode = clookup_ToProject.GetText();
    if ($("#hdnProjectSelectInEntryModule").val() == "1" && $("#hdnProjectMandatory").val() == "1" && ProjectCode == "") {
        jAlert("Please Select To Project.");
        return false;
    }

    if ($("#hdnMandatoryEntityWarehouseStockTransfer").val() == "1" && $("#hdnEntityId").val() == "") {
        jAlert("Please Select Entity.");
        return false;
    }

    $('#HiddenSaveButton').val("E");
    saveNewOrExit = 'E';
    if (ValidateEntry()) {
        if (!grid.InCallback()) {





            if (issavePacking == 1) {
                if (aarr.length > 0) {

                    $.ajax({
                        type: "POST",
                        url: "WarehousewiseStockTransferAdd.aspx/SetSessionPacking",
                        data: "{'list':'" + JSON.stringify(aarr) + "'}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (msg) {
                            //grid.UpdateEdit();
                        }
                    });

                }
                //grid.batchEditApi.StartEdit(0, 2)
                //grid.batchEditApi.StartEdit(-1, 2);
               


                var frontRow = 0;
                var backRow = -1;

                for (var i = 0; i <= grid.GetVisibleRowsOnPage() ; i++) {
                    var frontProduct = (grid.batchEditApi.GetCellValue(backRow, 'ProductName') != null) ? (grid.batchEditApi.GetCellValue(backRow, 'ProductName')) : "";
                    var backProduct = (grid.batchEditApi.GetCellValue(frontRow, 'ProductName') != null) ? (grid.batchEditApi.GetCellValue(frontRow, 'ProductName')) : "";
                    if (frontProduct != "" || backProduct != "") {
                        IsType = "Y";
                        grid.batchEditApi.StartEdit(i, 2)
                    }
                    backRow--;
                    frontRow++;
                }

                grid.batchEditApi.EndEdit();
                grid.AddNewItem();                
                grid.UpdateEdit();

            }
            else {
                if (aarr.length > 0) {

                    $.ajax({
                        type: "POST",
                        url: "WarehousewiseStockTransferAdd.aspx/SetSessionPacking",
                        data: "{'list':'" + JSON.stringify(aarr) + "'}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (msg) {
                            //grid.UpdateEdit();
                        }
                    });

                }
                //else {
                //    grid.UpdateEdit();
                //}
                //grid.batchEditApi.StartEdit(0, 2)
                //grid.batchEditApi.StartEdit(-1, 2);

                var frontRow = 0;
                var backRow = -1;

                for (var i = 0; i <= grid.GetVisibleRowsOnPage() ; i++) {
                    var frontProduct = (grid.batchEditApi.GetCellValue(backRow, 'ProductName') != null) ? (grid.batchEditApi.GetCellValue(backRow, 'ProductName')) : "";
                    var backProduct = (grid.batchEditApi.GetCellValue(frontRow, 'ProductName') != null) ? (grid.batchEditApi.GetCellValue(frontRow, 'ProductName')) : "";
                    if (frontProduct != "" || backProduct != "") {
                        IsType = "Y";
                        grid.batchEditApi.StartEdit(i, 2)
                    }
                    backRow--;
                    frontRow++;
                }
                grid.batchEditApi.EndEdit();
                grid.AddNewItem();               
                grid.UpdateEdit();
            }
        }
    }
}

function SuffleRows() {
    for (var i = 0; i < 1000; i++) {
        if (grid.GetRow(i)) {
            if (grid.GetRow(i).style.display != "none") {
                grid.batchEditApi.StartEdit(i, 2);
                grid.GetEditor("UpdateEdit").SetText(grid.GetEditor("UpdateEdit").GetText() + i);
            }
        }
    }

    for (i = -1; i > -1000; i--) {
        if (grid.GetRow(i)) {
            if (grid.GetRow(i).style.display != "none") {
                grid.batchEditApi.StartEdit(i, 2);
                grid.GetEditor("UpdateEdit").SetText(grid.GetEditor("UpdateEdit").GetText() + i);
            }
        }
    }
}



function afterSave() {
    var stockTransfer = grid.cpadjustmentId;
    if (saveNewOrExit == 'N') {
        //Rev Subhra 29-07-2019
        if (stockTransfer != '') {
            //End of Rev
            if (document.getElementById('hdnWSTAutoPrint').value == 1) {
                reportName = "StockTransfer~D";
                window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + reportName + '&modulename=WHRSSTOCKTRANS&id=' + stockTransfer + '&PrintOption=1', '_blank');
                window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + reportName + '&modulename=WHRSSTOCKTRANS&id=' + stockTransfer + '&PrintOption=2', '_blank');
                window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + reportName + '&modulename=WHRSSTOCKTRANS&id=' + stockTransfer + '&PrintOption=3', '_blank');
            }
        }
        window.location.href = 'WarehousewiseStockTransferAdd.aspx?Key=Add';
        $('#hdAddEdit').val("Add");
    }
    else {
        //Rev Subhra 29-07-2019
        if (stockTransfer != '') {
            //End of Rev
            if (document.getElementById('hdnWSTAutoPrint').value == 1) {
                reportName = "StockTransfer~D";
                window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + reportName + '&modulename=WHRSSTOCKTRANS&id=' + stockTransfer + '&PrintOption=1', '_blank');
                window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + reportName + '&modulename=WHRSSTOCKTRANS&id=' + stockTransfer + '&PrintOption=2', '_blank');
                window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + reportName + '&modulename=WHRSSTOCKTRANS&id=' + stockTransfer + '&PrintOption=3', '_blank');
            }
        }
        window.location.href = 'WarehousewiseStockTransferList.aspx';
    }
}

function DestWH_ValueChange(s, e) {
    var Wh = s.GetValue();
    var splitData = Wh.split('~');
    clblDestWHAddress.SetText(splitData[1]);


    if (ccmbSourceWarehouse.GetText().trim() == "-Select-") {
        var OtherDetails = {}
        OtherDetails.BranchID = $('#ddlBranch').val();
        OtherDetails.WarehouseName = ccmbDestWarehouse.GetText().trim();

        $.ajax({
            type: "POST",
            url: "../Activities/Services/Master.asmx/GetWarehouseByFilterStockTransfer",
            data: JSON.stringify(OtherDetails),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                var returnObject = msg.d;
                clblSourceWHAddress.SetText('');
                if (returnObject.ForWareHouse) {
                    SetDataSourceOnComboBox(ccmbSourceWarehouse, returnObject.ForWareHouse);
                }
            }
        });
    }
    var TotRowNumber = grid.GetVisibleItemsOnPage();
    for (var i = 0; i < 1000; i++) {
        if (grid.GetRow(i)) {
            grid.DeleteRow(i);
        }
    }

    for (i = -1; i > -1000; i--) {
        if (grid.GetRow(i)) {
            grid.DeleteRow(i);
        }
    }
    PickedDocument = [];
    //GridAddnewRow();
    grid.AddNewRow();
    grid.GetEditor("SrlNo").SetText(1);

    // DeleteAllRows();
    setTimeout(function () {
        grid.batchEditApi.EndEdit();
        grid.batchEditApi.StartEdit(globalRowIndex, 2);
        return;
    }, 200);
}
function SourceWH_ValueChange(s, e) {
    var Wh = s.GetValue();
    var splitData = Wh.split('~');
    clblSourceWHAddress.SetText(splitData[1]);

    if (ccmbSourceWarehouse.GetText().trim() != "-Select-") {
        var OtherDetails = {}
        OtherDetails.BranchID = $('#ddlBranchTo').val();
        OtherDetails.WarehouseName = ccmbSourceWarehouse.GetText().trim();

        $.ajax({
            type: "POST",
            url: "../Activities/Services/Master.asmx/GetWarehouseByFilterStockTransfer",
            data: JSON.stringify(OtherDetails),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                var returnObject = msg.d;
                clblDestWHAddress.SetText('');
                ccmbDestWarehouse.Focus();
                if (returnObject.ForWareHouse) {
                    SetDataSourceOnComboBoxWH(ccmbDestWarehouse, returnObject.ForWareHouse);
                }
            }
        });
    }

    //DeleteAllRows();
    var TotRowNumber = grid.GetVisibleItemsOnPage();
    for (var i = 0; i < 1000; i++) {
        if (grid.GetRow(i)) {
            grid.DeleteRow(i);
        }
    }

    for (i = -1; i > -1000; i--) {
        if (grid.GetRow(i)) {
            grid.DeleteRow(i);
        }
    }
    PickedDocument = [];
    //GridAddnewRow();
    grid.AddNewRow();
    grid.GetEditor("SrlNo").SetText(1);

    ccmbDestWarehouse.Focus();
}

function CmbScheme_ValueChange(s, e) {
    var numbSchm = s.GetValue();
    var splitData = numbSchm.split('~');
    var startNo = splitData[1];

    var fromdate = splitData[3];
    var todate = splitData[4];

    var dt = new Date();

    cdtTDate.SetDate(dt);

    if (dt < new Date(fromdate)) {
        cdtTDate.SetDate(new Date(fromdate));
    }

    if (dt > new Date(todate)) {
        cdtTDate.SetDate(new Date(todate));
    }
    cdtTDate.SetMinDate(new Date(fromdate));
    cdtTDate.SetMaxDate(new Date(todate));

    if (numbSchm != "") {
        $('#ddlBranch').val(splitData[2]);
        $("#ddlBranch").prop("disabled", true);
        //Rev Subhra 29-07-2019
        $('#ddlBranchTo').val(splitData[2]);
        //End of Rev Subhra
    }
    if (startNo == "1") {
        ctxtVoucherNo.SetText("Auto");
        ctxtVoucherNo.SetEnabled(false);
    } else {
        ctxtVoucherNo.SetText("");
        ctxtVoucherNo.SetEnabled(true);
    }
    if ($("#hdnProjectSelectInEntryModule").val() == "1") {
        clookup_Project.gridView.Refresh();
        clookup_ToProject.gridView.Refresh();
    }
  //  ctxtTransportationMode.Focus();
    $("#ddlBranchTo").focus();

    ctxtEntity.SetText('');
    GetObjectID('hdnEntityId').value = '';

    //setTimeout(function () {
    //    grid.batchEditApi.EndEdit();
    //    grid.batchEditApi.StartEdit(globalRowIndex, 2);
    //    return;
    //}, 200);

    //var OtherDetails = {}
    //OtherDetails.BranchID = $('#ddlBranch').val();

    //$.ajax({
    //    type: "POST",
    //    url: "../Activities/Services/Master.asmx/GetWarehouseByBranchStockTransfer",
    //    data: JSON.stringify(OtherDetails),
    //    contentType: "application/json; charset=utf-8",
    //    dataType: "json",
    //    success: function (msg) {
    //        var returnObject = msg.d;

    //        if (returnObject.ForWareHouse) {
    //            SetDataSourceOnComboBox(ccmbSourceWarehouse, returnObject.ForWareHouse);
    //        }
    //    }
    //});

    //$.ajax({
    //    type: "POST",
    //    url: "../Activities/Services/Master.asmx/GetWarehouseByBranchStockTransfer",
    //    data: JSON.stringify(OtherDetails),
    //    contentType: "application/json; charset=utf-8",
    //    dataType: "json",
    //    success: function (msg) {
    //        var returnObject = msg.d;

    //        if (returnObject.ForWareHouse) {              

    //            SetDataSourceOnComboBoxWH(ccmbDestWarehouse, returnObject.ForWareHouse);
    //        }

    //    }
    //});
    //DeleteAllRows();

    //ccmbSourceWarehouse.Focus();
}

function DeleteAllRows() {
    var TotRowNumber = grid.GetVisibleItemsOnPage();
    for (var i = 0; i < 1000; i++) {
        if (grid.GetRow(i)) {
            grid.DeleteRow(i);
        }
    }

    for (i = -1; i > -1000; i--) {
        if (grid.GetRow(i)) {
            grid.DeleteRow(i);
        }
    }
    PickedDocument = [];
    //GridAddnewRow();
    grid.AddNewRow();
    grid.GetEditor("SrlNo").SetText(1);
}
function SetDataSourceOnComboBoxWH(ControlObject, Source) {
    ControlObject.ClearItems();
    for (var count = 0; count < Source.length; count++) {
        ControlObject.AddItem(Source[count].Name, Source[count].Id);
    }
    ControlObject.SetSelectedIndex(0);
}

function SetDataSourceOnComboBox(ControlObject, Source) {
    ControlObject.ClearItems();
    for (var count = 0; count < Source.length; count++) {
        ControlObject.AddItem(Source[count].Name, Source[count].Id);
    }
    ControlObject.SetSelectedIndex(0);
}


function ProductButnClick(s, e) {
    if (e.buttonIndex == 0) {
        setTimeout(function () { $("#txtProdSearch").focus(); }, 500);
        $('#txtProdSearch').val('');
        $('#ProductModel').modal('show');
    }
}
function ProductKeyDown(s, e) {
    console.log(e.htmlEvent.key);
    if (e.htmlEvent.key == "Enter" || e.code == "NumpadEnter") {
        s.OnButtonClick(0);
    }
    //if (e.htmlEvent.key == "Tab") {
    //    s.OnButtonClick(0);
    //}
}
function prodkeydown(e) {

    //if ($.trim($("#txtProdSearch").val()) == "" || $.trim($("#txtProdSearch").val()) == null) {

    //    return false;
    //}

    var OtherDetails = {}
    OtherDetails.SearchKey = $("#txtProdSearch").val();

    if (e.code == "Enter" || e.code == "NumpadEnter") {
        var HeaderCaption = [];
        HeaderCaption.push("Product Code");
        HeaderCaption.push("Product Description");
        HeaderCaption.push("Inventory");
        HeaderCaption.push("HSN/SAC");
        HeaderCaption.push("Class");
        HeaderCaption.push("Brand");
        if ($("#txtProdSearch").val() != '') {
            callonServer("Services/Master.asmx/GetProductForWHStockTransfer", OtherDetails, "ProductTable", HeaderCaption, "ProdIndex", "SetProduct");
        }

      //var datagrid=  $("#ProductTable").dxDataGrid({
      //      dataSource: new DevExpress.data.CustomStore({
      //          load: function () {
      //              var deferred = $.Deferred();

      //              var xhr = $.ajax({
      //                  method: "POST",
      //                  url: "Services/Master.asmx/GetProductForWHStockTransfer",
      //                  data: JSON.stringify(OtherDetails),
      //                  contentType: "application/json",
      //                  dataType: "JSON",
      //                  //async: false,  
      //                  success: function (result) {
      //                      console.log('before resolve');
      //                      deferred.resolve(result.d);
      //                      console.log('after resolve');
      //                  }
      //              });
      //              return deferred.promise();
      //          }
      //      }),

      //      focusedRowEnabled: true,
      //      focusedRowIndex: 0,
      //      focusedColumnIndex: 0 ,
      //      keyExpr: "Products_ID",
      //      selection: {
      //          mode: "single"
      //      },
      //      filterRow: {
      //          visible: true,
      //          applyFilter: "auto"
      //      },
      //      hoverStateEnabled: true,
      //      showBorders: true,
      //      paging: {
      //          pageSize: 10
      //      },
      //      pager: {
      //          showPageSizeSelector: true,
      //          allowedPageSizes: [10, 25, 50, 100]
      //      },
      //      columnAutoWidth: true,
      //      remoteOperations: false,
      //      searchPanel: {
      //          visible: true,
      //          highlightCaseSensitive: true
      //      },

      //      allowColumnReordering: true,
      //      rowAlternationEnabled: true,
      //      showBorders: true,

      //      //HeaderCaption.push("Product Code");
      //      //HeaderCaption.push("Product Description");
      //      //HeaderCaption.push("Inventory");
      //      //HeaderCaption.push("HSN/SAC");
      //      //HeaderCaption.push("Class");
      //      //HeaderCaption.push("Brand");

      //      columns: [
      //          {
      //              dataField: "Product_Code",
      //              caption: "Product Code",
      //              dataType: "string"
      //          },
      //           {
      //               dataField: "Products_Name",
      //               caption: "Product Description",
      //               dataType: "string"
      //           },
      //            {
      //                dataField: "IsInventory",
      //                caption: "Inventory",
      //                dataType: "string"
      //            },
      //            {
      //                dataField: "HSNSAC",
      //                caption: "HSN/SAC",
      //                dataType: "string"
      //            },
      //             {
      //                 dataField: "ClassCode",
      //                 caption: "Class",
      //                 dataType: "string"
      //             },
      //              {
      //                  dataField: "BrandName",
      //                  caption: "Brand",
      //                  dataType: "string"
      //              }

      //      ],
      //      oninit:function(e){
      //          alert(e);
      //      },
      //      onKeyDown: function (e) {
      //          if (e.event.key == "Enter")
      //          {
      //              var data = e.component.getSelectedRowKeys()[0];
      //              console.log(data);
      //          }
      //         // alert(e);
      //      },
      //      onSelectionChanged: function (selectedItems) {
      //          var data = selectedItems.selectedRowsData[0];
      //          if (data) {
      //              SetProduct(data.Products_ID, data.Product_Code, data.Products_Name);
      //              //$("#UnitNumberingnidal").modal('hide')
      //              //grid.batchEditApi.StartEdit(globalRowIndex, 2);
      //              //grid.GetEditor("UnitID").SetText(data.branch_id);
      //              //grid.GetEditor("Unit").SetText(data.branch_description);
      //              //this.clearSelection();
      //              console.log(data);
      //          }
      //      }
      //  });

      //var c = "";

    }
    else if (e.code == "ArrowDown") {
        if ($("input[ProdIndex=0]"))
            $("input[ProdIndex=0]").focus();
    }
    else if (e.code == "Escape") {
        //grid.batchEditApi.EndEdit();
        grid.batchEditApi.StartEdit(globalRowIndex, 2);
    }
}
function ValueSelected(e, indexName) {
    debugger;
    if (e.code == "Enter" || e.code == "NumpadEnter") {
        var Id = e.target.parentElement.parentElement.cells[0].innerText;
        var code = e.target.parentElement.parentElement.cells[1].children[0].value;
        //var name = e.target.parentElement.parentElement.cells[2].children[0].value;
        var name = "";
        if (Id) {
            if (indexName == "ProdIndex") {
                SetProduct(Id, code, name);
            }
            else if (indexName == "SWHIndex") {
                SetSourceWarehouse(Id, code);
            }
            else if (indexName == "DWHIndex") {
                SetDestinationWarehouse(Id, code);
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
            if (indexName == "ProdIndex")
                $('#txtProdSearch').focus();
            else if (indexName == "SWHIndex")
                $('#txtSourceWarehouseSearch').focus();
            else if (indexName == "DWHIndex")
                $('#txtDestinationWarehouseSearch').focus();
        }
    }
}
function SetProduct(Id, code, name) {
    debugger;
    var ProductIDS = Id;
    var splitData = ProductIDS.split('||@||');
    $('#ProductModel').modal('hide');
    var strProductID = splitData[0];
    var LookUpData = Id;
    var ProductDescription = splitData[1];
    var ProductCode = code;
    if (!ProductCode) {
        LookUpData = null;
    }
    var SaleUOMname = splitData[2];

    grid.batchEditApi.StartEdit(globalRowIndex);
    grid.GetEditor("ProductID").SetText(LookUpData);
    grid.GetEditor("ProductName").SetText(ProductCode);
    grid.GetEditor("Discription").SetText(ProductDescription);


    

    if ($('#hdnWarehouseRepeatStockTransfer').val() == "1")
    {
        //grid.GetEditor("SourceWarehouseID").SetText("");
       // grid.GetEditor("SourceWarehouse").SetText("");
        grid.GetEditor("AvlStkSourceWH").SetText("0.0000");
       // grid.GetEditor("DestinationWarehouseID").SetText("");
       // grid.GetEditor("DestinationWarehouse").SetText("");
        grid.GetEditor("AvlStkDestWH").SetText("0.0000");
        grid.GetEditor("TransferQuantity").SetValue("0.0000");
        grid.GetEditor("SaleUOM").SetValue(SaleUOMname);
        grid.GetEditor("Rate").SetValue("0.00");
        grid.GetEditor("Value").SetValue("0.0000");
    }
    else {
        grid.GetEditor("SourceWarehouseID").SetText("");
        grid.GetEditor("SourceWarehouse").SetText("");
        grid.GetEditor("AvlStkSourceWH").SetText("0.0000");
        grid.GetEditor("DestinationWarehouseID").SetText("");
        grid.GetEditor("DestinationWarehouse").SetText("");
        grid.GetEditor("AvlStkDestWH").SetText("0.0000");
        grid.GetEditor("TransferQuantity").SetValue("0.0000");
        grid.GetEditor("SaleUOM").SetValue(SaleUOMname);
        grid.GetEditor("Rate").SetValue("0.00");
        grid.GetEditor("Value").SetValue("0.0000");

        setTimeout(function () {
            grid.batchEditApi.StartEdit(globalRowIndex, 5);
        }, 200);

        grid.batchEditApi.EndEdit();
    }

    

   
    

    if ($('#hdnWarehouseRepeatStockTransfer').val() == "1") {

        var BranchId = $('#ddlBranch').val();
        var fromBranch = $('#ddlBranch').val();
        var ToBranch = $('#ddlBranchTo').val()
        var SourceWarehouse = (grid.GetEditor('SourceWarehouse').GetValue() != null) ? grid.GetEditor('SourceWarehouse').GetValue() : "";
        var SourceWarehouseID = (grid.GetEditor('SourceWarehouseID').GetValue() != null) ? grid.GetEditor('SourceWarehouseID').GetValue() : "";

        var DestinationWarehouse = (grid.GetEditor('DestinationWarehouse').GetValue() != null) ? grid.GetEditor('DestinationWarehouse').GetValue() : "";
        var DestinationWarehouseID = (grid.GetEditor('DestinationWarehouseID').GetValue() != null) ? grid.GetEditor('DestinationWarehouseID').GetValue() : "";

        if (SourceWarehouseID != "") {


            $.ajax({
                type: "POST",
                url: "WarehousewiseStockTransferAdd.aspx/GetStockInHandForWarehouseWiseStockTransffer",
                data: JSON.stringify({ ProductId: splitData[0], WarehouseID: SourceWarehouseID, BranchId: BranchId, fromBranch: fromBranch, ToBranch: ToBranch, Fromdate: cdtTDate.date.format('yyyy-MM-dd') }),

                contentType: "application/json; charset=utf-8",
                async: false,
                dataType: "json",
                success: function (msg) {
                    var data = msg.d;
                    var strStockID = data.split("~")[0].toString();
                    var strStockUOM = data.split("~")[1].toString();
                    var Rate = data.split("~")[1].toString();
                    grid.GetEditor("AvlStkSourceWH").SetText(strStockID);
                    // grid.batchEditApi.EndEdit();

                    setTimeout(function () {
                        grid.batchEditApi.StartEdit(globalRowIndex, 8);
                    }, 200);

                }
            });

        }
        if (DestinationWarehouseID != "") {

            $.ajax({
                type: "POST",
                url: "WarehousewiseStockTransferAdd.aspx/GetStockInHandForWarehouseWiseStockTransffer",
                data: JSON.stringify({ ProductId: splitData[0], WarehouseID: DestinationWarehouseID, BranchId: BranchId, fromBranch: fromBranch, ToBranch: ToBranch, Fromdate: cdtTDate.date.format('yyyy-MM-dd') }),

                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (msg) {
                    var data = msg.d;
                    var strStockID = data.split("~")[0].toString();
                    var strStockUOM = data.split("~")[1].toString();
                    var Rate = data.split("~")[1].toString();
                    grid.GetEditor("AvlStkDestWH").SetText(strStockID);

                }
            });
        }

    }




    //var SourceWarehouseID = ccmbSourceWarehouse.GetValue().split('~')[0];
    //var BranchId = $('#ddlBranch').val();
    //$.ajax({
    //    type: "POST",
    //    url: "WarehousewiseStockTransferAdd.aspx/GetStockInHand",
    //    data: JSON.stringify({ ProductId: strProductID, WarehouseID: SourceWarehouseID, BranchId: BranchId }),
    //    contentType: "application/json; charset=utf-8",
    //    async:false,
    //    dataType: "json",
    //    success: function (msg) {
    //        var data = msg.d;
    //        var strStockID = data.split("~")[0].toString();
    //        var strStockUOM = data.split("~")[1].toString();
    //        var Rate = data.split("~")[1].toString();

    //        grid.GetEditor("AvlStkSourceWH").SetText(strStockID);

    //    }
    //});

    //var DestWarehouseID = ccmbDestWarehouse.GetValue().split('~')[0];
    //var BranchId = $('#ddlBranch').val();
    //$.ajax({
    //    type: "POST",
    //    url: "WarehousewiseStockTransferAdd.aspx/GetStockInHand",
    //    data: JSON.stringify({ ProductId: strProductID, WarehouseID: DestWarehouseID, BranchId: BranchId }),
    //    contentType: "application/json; charset=utf-8",
    //    dataType: "json",
    //    async:false,
    //    success: function (msg) {
    //        var data = msg.d;
    //        var strStockID = data.split("~")[0].toString();
    //        var strStockUOM = data.split("~")[1].toString();
    //        var Rate = data.split("~")[1].toString();
    //        grid.GetEditor("AvlStkDestWH").SetText(strStockID);
    //        grid.batchEditApi.EndEdit();
    //        setTimeout(function(){
    //         grid.batchEditApi.StartEdit(globalRowIndex, 6);
    //    },200);



    //    }
    //});

   
    //Rev Subhra 30-07-2019
    $("#ddlBranchTo").prop("disabled", true);
    //End of Rev Subhra 30-07-2019


}

function SourceWarehouseButnClick(s, e) {
    var strProductID = grid.GetEditor("ProductID").GetValue();

    if (strProductID == '' && strProductID == null) {
        jAlert("Please Select a Product", "Alert", function () {
            grid.batchEditApi.StartEdit(globalRowIndex);
            grid.batchEditApi.EndEdit();
            setTimeout(function () {
                grid.batchEditApi.StartEdit(globalRowIndex, 2);
            }, 200);
        });
    }
    else {
        if (e.buttonIndex == 0) {
            var txt = "<table border='1' width=\"100%\" class=\"dynamicPopupTbl\"><tr class=\"HeaderStyle\"><th>Source Warehouse</th></tr><table>";
            document.getElementById("SourceWarehouseTable").innerHTML = txt;
            setTimeout(function () { $("#txtSourceWarehouseSearch").focus(); }, 500);
            $('#txtSourceWarehouseSearch').val('');
            $('#SourceWarehouseModel').modal('show');
        }
    }

}

function SourceWarehouseKeyDown(s, e) {
    var strProductID = grid.GetEditor("ProductID").GetValue();

    if (strProductID == '' && strProductID == null) {
        jAlert("Please Select a Product", "Alert", function () {
            grid.batchEditApi.StartEdit(globalRowIndex);
            grid.batchEditApi.EndEdit();
            setTimeout(function () {
                grid.batchEditApi.StartEdit(globalRowIndex, 2);
            }, 200);
        });
    }
    else {

        if (e.htmlEvent.key == "Enter" || e.code == "NumpadEnter") {
            s.OnButtonClick(0);
        }
        //if (e.htmlEvent.key == "Tab") {
        //    s.OnButtonClick(0);
        //}
    }
}

function SourceWarehousekeydown(e) {

    //if ($.trim($("#txtProdSearch").val()) == "" || $.trim($("#txtProdSearch").val()) == null) {

    //    return false;
    //}

    var OtherDetails = {}
    OtherDetails.SearchKey = $("#txtSourceWarehouseSearch").val();
    OtherDetails.BranchID = $('#ddlBranch').val();

    if (e.code == "Enter" || e.code == "NumpadEnter") {
        var HeaderCaption = [];
        HeaderCaption.push("Source Warehouse");

        if ($("#txtSourceWarehouseSearch").val() != '') {
            //callonServer("Services/Master.asmx/GetProductForWHStockTransfer", OtherDetails, "ProductTable", HeaderCaption, "ProdIndex", "SetProduct");
            callonServer("Services/Master.asmx/GetWarehouseByBranchStockTransfer", OtherDetails, "SourceWarehouseTable", HeaderCaption, "SWHIndex", "SetSourceWarehouse");
        }
    }
    else if (e.code == "ArrowDown") {
        if ($("input[SWHIndex=0]"))
            $("input[SWHIndex=0]").focus();
    }
    else if (e.code == "Escape") {
        //grid.batchEditApi.EndEdit();
        grid.batchEditApi.StartEdit(globalRowIndex, 2);
    }
}

function SetSourceWarehouse(Id, name) {
  
    var SourceWarehouseID = Id;
   // var SourceWarehouseID = Id.split('~')[0];

    $('#SourceWarehouseModel').modal('hide');
    grid.batchEditApi.StartEdit(globalRowIndex);
    grid.GetEditor("SourceWarehouseID").SetText(SourceWarehouseID);
    grid.GetEditor("SourceWarehouse").SetText(name);

    if ($("#hdnBranchReqTaggingWST").val() == "0")
    {
        grid.GetEditor("TransferQuantity").SetValue("0.0000");
    }
    
    grid.GetEditor("Rate").SetValue("0.00");
    grid.GetEditor("Value").SetValue("0.00");

    var strProductID = grid.GetEditor("ProductID").GetValue();
    var splitData = strProductID.split('||@||');
    
    var BranchId = $('#ddlBranch').val();   
    var fromBranch = $('#ddlBranch').val();
    var ToBranch = $('#ddlBranchTo').val()
  
    $.ajax({
        type: "POST",         
        url: "WarehousewiseStockTransferAdd.aspx/GetStockInHandForWarehouseWiseStockTransffer",      
        data: JSON.stringify({ ProductId: splitData[0], WarehouseID: SourceWarehouseID, BranchId: BranchId, fromBranch: fromBranch, ToBranch: ToBranch, Fromdate: cdtTDate.date.format('yyyy-MM-dd') }),
       
        contentType: "application/json; charset=utf-8",
        async: false,
        dataType: "json",
        success: function (msg) {
            var data = msg.d;
            var strStockID = data.split("~")[0].toString();
            var strStockUOM = data.split("~")[1].toString();
            var Rate = data.split("~")[1].toString();

            grid.GetEditor("AvlStkSourceWH").SetText(strStockID);

            grid.batchEditApi.EndEdit();
            setTimeout(function () {
                grid.batchEditApi.StartEdit(globalRowIndex, 8);
            }, 200);

        }
    });

    var strDestinationWarehouseID = grid.GetEditor("DestinationWarehouseID").GetValue();
    if (strDestinationWarehouseID != '' && strDestinationWarehouseID != null) {
       
        grid.batchEditApi.StartEdit(globalRowIndex);
        grid.GetEditor("DestinationWarehouseID").SetText('');
        grid.GetEditor("DestinationWarehouse").SetText('');
        grid.GetEditor("AvlStkDestWH").SetText("0.0000");
    }

}

function DestinationWarehouseButnClick(s, e) {
    var SourceWarehouse = grid.GetEditor("SourceWarehouse").GetValue();
    var strProductID = grid.GetEditor("ProductID").GetValue();

    if (strProductID == '' && strProductID == null) {
        jAlert("Please Select a Product", "Alert", function () {
            grid.batchEditApi.StartEdit(globalRowIndex);
            grid.batchEditApi.EndEdit();
            setTimeout(function () {
                grid.batchEditApi.StartEdit(globalRowIndex, 2);
            }, 200);
        });
    }
    else if (SourceWarehouse == '' && SourceWarehouse == null) {
        jAlert("Please Select a Source Warehouse", "Alert", function () {
            grid.batchEditApi.StartEdit(globalRowIndex);
            grid.batchEditApi.EndEdit();
            setTimeout(function () {
                grid.batchEditApi.StartEdit(globalRowIndex, 5);
            }, 200);
        });
    }
    else {
        if (e.buttonIndex == 0) {
            var txt = "<table border='1' width=\"100%\" class=\"dynamicPopupTbl\"><tr class=\"HeaderStyle\"><th>Destination Warehouse</th></tr><table>";
            document.getElementById("DestinationWarehouseTable").innerHTML = txt;
            setTimeout(function () { $("#txtDestinationWarehouseSearch").focus(); }, 500);
            $('#txtDestinationWarehouseSearch').val('');
            $('#DestinationWarehouseModel').modal('show');
        }
    }
}

function DestinationWarehouseKeyDown(s, e) {
    var SourceWarehouse = grid.GetEditor("SourceWarehouse").GetValue();
    var strProductID = grid.GetEditor("ProductID").GetValue();

    if (strProductID == '' && strProductID == null) {
        jAlert("Please Select a Product", "Alert", function () {
            grid.batchEditApi.StartEdit(globalRowIndex);
            grid.batchEditApi.EndEdit();
            setTimeout(function () {
                grid.batchEditApi.StartEdit(globalRowIndex, 2);
            }, 200);
        });
    }
    else if (SourceWarehouse == '' && SourceWarehouse == null) {
        jAlert("Please Select a Source Warehouse", "Alert", function () {
            grid.batchEditApi.StartEdit(globalRowIndex);
            grid.batchEditApi.EndEdit();
            setTimeout(function () {
                grid.batchEditApi.StartEdit(globalRowIndex, 5);
            }, 200);
        });
    }
    else {
        if (e.htmlEvent.key == "Enter" || e.code == "NumpadEnter") {
            s.OnButtonClick(0);
        }
        //if (e.htmlEvent.key == "Tab") {
        //    s.OnButtonClick(0);
        //}
    }
}

function DestinationWarehousekeydown(e) {

    //if ($.trim($("#txtProdSearch").val()) == "" || $.trim($("#txtProdSearch").val()) == null) {

    //    return false;
    //}

    var SourceWarehouse = grid.GetEditor("SourceWarehouse").GetValue();
    var OtherDetails = {}
    OtherDetails.SearchKey = $("#txtDestinationWarehouseSearch").val();
    OtherDetails.BranchID = $('#ddlBranchTo').val();
    OtherDetails.WarehouseName = SourceWarehouse.trim();

    if (e.code == "Enter" || e.code == "NumpadEnter") {
        var HeaderCaption = [];
        HeaderCaption.push("Destination Warehouse");

        if ($("#txtDestinationWarehouseSearch").val() != '' && SourceWarehouse.trim() != '') {
            //callonServer("Services/Master.asmx/GetProductForWHStockTransfer", OtherDetails, "ProductTable", HeaderCaption, "ProdIndex", "SetProduct");
            callonServer("Services/Master.asmx/GetWarehouseByFilterStockTransfer", OtherDetails, "DestinationWarehouseTable", HeaderCaption, "DWHIndex", "SetDestinationWarehouse");
        }
    }
    else if (e.code == "ArrowDown") {
        if ($("input[DWHIndex=0]"))
            $("input[DWHIndex=0]").focus();
    }
    else if (e.code == "Escape") {
        //grid.batchEditApi.EndEdit();
        grid.batchEditApi.StartEdit(globalRowIndex, 7);
    }
}

function SetDestinationWarehouse(Id, name) {
    // var DestinationWarehouseID = Id.split('~')[0];

    var DestinationWarehouseID = Id;
    $('#DestinationWarehouseModel').modal('hide');
    grid.batchEditApi.StartEdit(globalRowIndex);
    grid.GetEditor("DestinationWarehouseID").SetText(DestinationWarehouseID);
    grid.GetEditor("DestinationWarehouse").SetText(name);

    if($("#hdnBranchReqTaggingWST").val() == "0")
    {
        grid.GetEditor("TransferQuantity").SetValue("0.0000");
    }
    grid.GetEditor("Rate").SetValue("0.00");
    grid.GetEditor("Value").SetValue("0.0000");

    var strProductID = grid.GetEditor("ProductID").GetValue();
    var splitData = strProductID.split('||@||');
    // var DestWarehouseID = ccmbDestWarehouse.GetValue().split('~')[0];
    var BranchId = $('#ddlBranch').val();
    //Rev Rajdip
    //var SourceWarehouseID = ccmbSourceWarehouse.GetValue().split('~')[0];
    //Rev Rajdip
    var fromBranch = $('#ddlBranch').val();
    var ToBranch = $('#ddlBranchTo').val()
    //Fromdate: cdtTDate.date.format('yyyy-MM-dd')
    //End Rev Rajdip
    $.ajax({
        type: "POST",
        //Rev Rajdip
        //url: "WarehousewiseStockTransferAdd.aspx/GetStockInHand",
        url: "WarehousewiseStockTransferAdd.aspx/GetStockInHandForWarehouseWiseStockTransffer",
        // data: JSON.stringify({ ProductId: splitData[0], WarehouseID: DestinationWarehouseID, BranchId: BranchId }),
        data: JSON.stringify({ ProductId: splitData[0], WarehouseID: DestinationWarehouseID, BranchId: BranchId, fromBranch: fromBranch, ToBranch: ToBranch, Fromdate: cdtTDate.date.format('yyyy-MM-dd') }),
        //End Rev Rajdip
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (msg) {
            var data = msg.d;
            var strStockID = data.split("~")[0].toString();
            var strStockUOM = data.split("~")[1].toString();
            var Rate = data.split("~")[1].toString();
            grid.GetEditor("AvlStkDestWH").SetText(strStockID);
            //grid.batchEditApi.EndEdit();
            //setTimeout(function () {
            //    grid.batchEditApi.StartEdit(globalRowIndex, 11);
            //}, 200);

        }
    });

    ShowUOMPOpup();

}


function SetTotalTaxableAmount(inx, vindex) {
    var count = grid.GetVisibleRowsOnPage();
    var totalAmount = 0;
    var totalAltQty = 0;
    var totalQuantity = 0;
    for (var i = 0; i < count + 10; i++) {
        if (grid.GetRow(i)) {
            if (grid.GetRow(i).style.display != "none") {
                grid.batchEditApi.StartEdit(i, 2);
                totalAmount = totalAmount + DecimalRoundoff(grid.GetEditor("Value").GetValue(), 2);
                totalQuantity = totalQuantity + DecimalRoundoff(grid.GetEditor("TransferQuantity").GetValue(), 4);
                
            }
        }
    }
    if (aarr.length > 0)
    {
        for (var i = 0; i < aarr.length; i++) {
            totalAltQty = totalAltQty + DecimalRoundoff(aarr[i].packing, 4);
            
        }
        cbnrLblAltQty.SetText(DecimalRoundoff(totalAltQty, 2));
    }
    

    for (i = -1; i > -count - 10; i--) {
        if (grid.GetRow(i)) {
            if (grid.GetRow(i).style.display != "none") {
                grid.batchEditApi.StartEdit(i, 2);
                totalAmount = totalAmount + DecimalRoundoff(grid.GetEditor("Value").GetValue(), 2);
                totalQuantity = totalQuantity + DecimalRoundoff(grid.GetEditor("TransferQuantity").GetValue(), 4);
                
            }
        }
    }    
   
    grid.batchEditApi.EndEdit()
    cbnrLblAmtval.SetText(DecimalRoundoff(totalAmount, 2));
  
    cbnrLblTotalQty.SetText(DecimalRoundoff(totalQuantity, 4));   
    setTimeout(function () { grid.batchEditApi.StartEdit(inx, vindex); }, 200)
}

function EntityButnClick(s, e) {
    if (cCmbScheme.GetValue() == "0~1~0") {
        cCmbScheme.Focus();
    }
    else if ($("#ddlBranch").val() == "0") {
        $("#ddlBranch").focus();
    }
    else {
        $('#EntityModel').modal('show');
    }
}
function EntityKeyDown(s, e) {

    if (e.htmlEvent.key == "Enter" || e.code == "NumpadEnter") {
        if (cCmbScheme.GetValue() == "0~1~0") {
            jAlert("Please Select Numbering Scheme.", "Alert", function () {
                cCmbScheme.Focus();
            });
        }
        else if ($("#ddlBranch").val() == "0") {
            jAlert("Please Select Branch.", "Alert", function () {
                $("#ddlBranch").focus();
            });
        }
        else {
            $('#EntityModel').modal('show');
        }
    }
}
function Entitykeydown(e) {

    var OtherDetails = {}
    OtherDetails.SearchKey = $("#txtEntitySearch").val();
    OtherDetails.BranchId = $('#ddlBranch').val();
    if (e.code == "Enter" || e.code == "NumpadEnter") {
        var HeaderCaption = [];
        HeaderCaption.push("Unique Id");
        HeaderCaption.push("Entity Name");

        if ($("#txtEntitySearch").val() != '') {
            callonServer("Services/Master.asmx/GetEntityByBranch", OtherDetails, "EntityTable", HeaderCaption, "EntityIndex", "SetEntity");
        }
    }
    else if (e.code == "ArrowDown") {
        if ($("input[EntityIndex=0]"))
            $("input[EntityIndex=0]").focus();
    }
}
function SetEntity(Id, code, name) {
    if (Id) {
        name = name.parentElement.children[2].innerHTML;

        $('#EntityModel').modal('hide');

        ctxtEntity.SetText(code);
        GetObjectID('hdnEntityId').value = Id;

    }
}

function taggingListKeyDown(s, e) {
    if (e.htmlEvent.key == "Enter") {
        s.OnButtonClick(0);
    }
}

function taggingListButnClick(s, e) {
    var startDate = cdtTDate.GetValueString();
    ctaggingGrid.PerformCallback('BindComponentGrid' + '~' + startDate);
    cpopup_taggingGrid.Show();
}
function BRNumberChanged() {

    var BRData = ctaggingGrid.GetSelectedKeysOnPage();
   // var BRtag_Id = ctaggingGrid.GetSelectedKeysOnPage();

    //if (OrderData != 0 && validateOrderwithAmountAre() == true) {
    if (BRData != 0) {
        cgridproducts.PerformCallback('BindProductsDetails');
        cpopup_taggingGrid.Hide();
        cProductsPopup.Show();
    }
}
function gridProducts_EndCallback(s, e) {
    if (cgridproducts.cpComponentDetails) {
        _ComponentDetails = cgridproducts.cpComponentDetails;
        cgridproducts.cpComponentDetails = null;

        clookup_Project.gridView.Refresh();
        var _cpProjectID = _ComponentDetails.split('~')[2];
        clookup_Project.gridView.SelectItemsByKey(_cpProjectID);
        if (_cpProjectID > 0) {           
            clookup_Project.SetEnabled(false);
        }
        else {
            clookup_Project.SetEnabled(true);
        }

        clookup_ToProject.gridView.Refresh();
    }
}
function PerformCallToGridBind() {
    var BRTaggingData = cgridproducts.GetSelectedKeysOnPage();  
    if (BRTaggingData == 0) {
        cProductsPopup.Hide();
    }
    else {
        grid.PerformCallback('BindGridOnBR' + '~' + '@');       
        var quote_Id = ctaggingGrid.GetSelectedKeysOnPage();
        if (quote_Id.length > 0) {
            var ComponentDetails = _ComponentDetails.split("~");
            cgridproducts.cpComponentDetails = null;
            var ComponentNumber = ComponentDetails[0];
            var ComponentDate = ComponentDetails[1]; 
            ctaggingList.SetValue(ComponentNumber);
            cdt_BRDate.SetValue(ComponentDate);
        }
        cProductsPopup.Hide();
    }
}

//REV 1.0
var cpstockVal;
function GETAVAILABLESTOCK(sl, strProductID, branch, WarehouseID) {

   
    grid.batchEditApi.StartEdit(globalRowIndex);
   

    $.ajax({
        type: "POST",
        url: "WarehousewiseStockTransferAdd.aspx/getWarehousewisestock",
        data: JSON.stringify({ sl: sl, strProductID: strProductID, branch: branch, WarehouseID: WarehouseID }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (msg) {

            cpstockVal = msg.d;
            divpopupAvailableStock.style.display = "block";            
            document.getElementById('lblAvailableStock').innerHTML = cpstockVal;        
           
            document.getElementById('lblAvailableStockUOM').innerHTML = document.getElementById('txt_StockUOM').innerHTML;
            cpstockVal = null;
            grid.batchEditApi.StartEdit(globalRowIndex, 5);
            return false;


        }
    });
}

function GetWirehouseBatchWiseAviableStock(sl, strProductID, branch, WarehouseID, BatchID) {
    grid.batchEditApi.StartEdit(globalRowIndex);

    $.ajax({
        type: "POST",
        url: "WarehousewiseStockTransferAdd.aspx/getWarehouseBatchwisestock",
        data: JSON.stringify({ sl: sl, strProductID: strProductID, branch: branch, WarehouseID: WarehouseID, BatchID: BatchID }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (msg) {
            cpstockVal = msg.d;
            divpopupAvailableStock.style.display = "block";
            document.getElementById('lblAvailableStock').innerHTML = cpstockVal;

            cpstockVal = null;
            grid.batchEditApi.StartEdit(globalRowIndex, 5);
            return false;
        }
    });
}

//END REV 1.0
