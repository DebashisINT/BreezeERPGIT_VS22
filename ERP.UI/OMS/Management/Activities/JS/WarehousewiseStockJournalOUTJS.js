//==========================================================Revision History ============================================================================================
//   1.0   Priti    V2.0.37   15-03-2023    0025689: Alt Qty column & data is not showing while making Warehouse wise Stock out
//   2.0   Priti    V2.0.43   22-02-2024    Mantis: 0027218 Batchwise stock has been issued from any stock out module before receiving date which caused negative stock
//========================================== End Revision History =======================================================================================================--%>


var globalRowIndex;
var globalRowIndexDestination;
var saveNewOrExit = '';
var canCallBack = true;
var RowCount = 0;
var RowCountDest = 0;
var alertShow = false;
var SelectWarehouse = "0";
var SelectWarehouseDest = "0";
var SelectBatch = "0";
var SelectBatchDest = "0";
var SelectSerial = "0";
var SelectSerialDest = "0";
var SelectedWarehouseID = "0";
var SelectedWarehouseIDDest = "0";
var textSeparator = ";";
var selectedChkValue = "";
var selectedChkValueDest = "";
var aarrSWH = [];
var aarrDWH = [];


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
    $('#txRemarks_I').blur(function () {
        if (gridDEstination.GetVisibleRowsOnPage() == 1) {
            gridDEstination.batchEditApi.StartEdit(-1, 2);
        }
    })
    $('#CustModel').on('shown.bs.modal', function () {
        $('#txtCustSearch').focus();
    })
    $('#EntityModel').on('shown.bs.modal', function () {
        $('#txtEntitySearch').focus();
    })
    $('#TechnicianModel').on('shown.bs.modal', function () {
        $('#txtTechnicianSearch').focus();
    })
    //Rev 1.0
    if ($("#hdnShowUOMConversionInEntry").val() != "1") {
        div_AltQuantity.style.display = 'none';
        _div_Uom.style.display = 'none';
    }
    //Rev 1.0 End
});

function AddEntityClick() {

    var url = '/OMS/Management/Master/SrvMastEntity.aspx?id=ADD&status=Y';
    AspxDirectAddCustPopup.SetContentUrl(url);
    AspxDirectAddCustPopup.Show();

}
function ParentCustomerOnClose(InternalID, EntityCode) {

    AspxDirectAddCustPopup.Hide();
    //if (InternalID != "") {
    //    ctxtEntity.SetText(EntityCode);
    //    GetObjectID('hdnEntityId').value = InternalID;
    //}

}
function CustomerButnClick(s, e) {
    if (cCmbScheme.GetValue() == "0~1~0") {
        cCmbScheme.Focus();

    }
    else if ($("#ddlBranch").val() == "0") {
        $("#ddlBranch").focus();

    }
    else {
        $('#CustModel').modal('show');
    }
}
function CustomerKeyDown(s, e) {

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
            $('#CustModel').modal('show');
        }
    }
}
function Customerkeydown(e) {

    var OtherDetails = {}
    OtherDetails.SearchKey = $("#txtCustSearch").val();
    if (e.code == "Enter" || e.code == "NumpadEnter") {
        var HeaderCaption = [];
        HeaderCaption.push("Customer Name");
        HeaderCaption.push("Unique Id");
        HeaderCaption.push("Address");
        if ($("#txtCustSearch").val() != '') {
            callonServer("Services/Master.asmx/GetCustomer", OtherDetails, "CustomerTable", HeaderCaption, "customerIndex", "SetCustomer");
        }
    }
    else if (e.code == "ArrowDown") {
        if ($("input[customerindex=0]"))
            $("input[customerindex=0]").focus();
    }
}
function SetCustomer(Id, Name) {
    if (Id) {
        $('#CustModel').modal('hide');
        ctxtCustName.SetText(Name);
        GetObjectID('hdnCustomerId').value = Id;
    }
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

        if ($("#hdnLinelevelEntityWHSINOUT").val() != "1")
        {
            ctxtEntity.SetText(code);
            GetObjectID('hdnEntityId').value = Id;
        }
        else
        {
            grid.batchEditApi.StartEdit(globalRowIndex);
            grid.GetEditor("EntityID").SetText(Id);
            grid.GetEditor("EntityCode").SetText(code);
            grid.GetEditor("EntityName").SetText(name);
            grid.batchEditApi.EndEdit();
            setTimeout(function () {
                grid.batchEditApi.StartEdit(globalRowIndex, 14);
            }, 200);
        }
       
    }
}


function TechnicianButnClick(s, e) {
    if (cCmbScheme.GetValue() == "0~1~0") {
        cCmbScheme.Focus();
    }
    else if ($("#ddlBranch").val() == "0") {
        $("#ddlBranch").focus();
    }
    else {
        $('#TechnicianModel').modal('show');
    }
}
function TechnicianKeyDown(s, e) {

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
            $('#TechnicianModel').modal('show');
        }
    }
}
function Techniciankeydown(e) {

    var OtherDetails = {}
    OtherDetails.SearchKey = $("#txtTechnicianSearch").val();
    OtherDetails.BranchId = $('#ddlBranch').val();
    if (e.code == "Enter" || e.code == "NumpadEnter") {
        var HeaderCaption = [];
        HeaderCaption.push("Technician Name");
        HeaderCaption.push("Unique Id");

        if ($("#txtTechnicianSearch").val() != '') {
            callonServer("Services/Master.asmx/GetTechnicianByBranch", OtherDetails, "TechnicianTable", HeaderCaption, "TechnicianIndex", "SetTechnician");
        }
    }
    else if (e.code == "ArrowDown") {
        if ($("input[TechnicianIndex=0]"))
            $("input[TechnicianIndex=0]").focus();
    }
}
function SetTechnician(Id, Name) {
    if (Id) {
        $('#TechnicianModel').modal('hide');
        ctxtTechnician.SetText(Name);
        GetObjectID('hdnTechnicianId').value = Id;
    }
}

function ShowUOMPOpup(WHType) {

    if ($('#hdnShowUOMConversionInEntry').val() == 0) {
        return;
    }

    var Quantity = '';
    var strProductID = '';
    var splitData = '';
    var BranchId = '';
    $('#hdnWHtype').val(WHType);

    if (WHType == 'SWH') {
        grid.batchEditApi.StartEdit(globalRowIndex);
        Quantity = grid.GetEditor("TransferQuantity").GetValue();
        strProductID = grid.GetEditor("ProductID").GetValue();
        splitData = strProductID.split('||@||');
        BranchId = $('#ddlBranch').val();

        if (aarrSWH.length > 0) {
            aarr = aarrSWH;
        }
        else {
            aarr = [];
        }


    }
    else if (WHType == 'DWH') {
        gridDEstination.batchEditApi.StartEdit(globalRowIndexDestination);
        Quantity = gridDEstination.GetEditor("DestQuantity").GetValue();
        strProductID = gridDEstination.GetEditor("DestProductID").GetValue();
        splitData = strProductID.split('||@||');
        BranchId = $('#ddlBranchTo').val();

        if (aarrDWH.length > 0) {
            aarr = aarrDWH;
        }
        else {
            aarr = [];
        }
    }



    //grid.batchEditApi.EndEdit();
    var actionQry = '';
    var TransferId = '';
    var ConvertionOverideVisible = $('#hdnConvertionOverideVisible').val();
    var ShowUOMConversionInEntry = $('#hdnShowUOMConversionInEntry').val();
    var type = 'add';

    if ($('#hdAddEdit').val() != "Edit") {

        actionQry = 'WSJPackingQtyAdd';
        $.ajax({
            type: "POST",
            url: "Services/Master.asmx/GetMultiUOMDetails",
            data: JSON.stringify({ orderid: splitData[0], action: actionQry, module: 'WarehouseWiseStockJournal', strKey: TransferId }),
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
                var slno = '';
                //var slno = (grid.GetEditor('SrlNo').GetValue() != null) ? grid.GetEditor('SrlNo').GetValue() : "";
                var strProductID = splitData[0];

                var isOverideConvertion = SpliteDetails[4];
                var packing_saleUOM = SpliteDetails[2];
                var sProduct_SaleUom = SpliteDetails[3];
                var sProduct_quantity = SpliteDetails[0];
                var packing_quantity = SpliteDetails[1];

                if (WHType == 'SWH') {
                    slno = (grid.GetEditor('SrlNo').GetValue() != null) ? grid.GetEditor('SrlNo').GetValue() : "";
                    gridprodqty = grid.GetEditor("TransferQuantity").GetValue();
                }
                else if (WHType == 'DWH') {
                    slno = (gridDEstination.GetEditor('SrlNo').GetValue() != null) ? gridDEstination.GetEditor('SrlNo').GetValue() : "";
                    gridprodqty = gridDEstination.GetEditor("DestQuantity").GetValue();
                }


                if (ShowUOMConversionInEntry == "1" && SpliteDetails.length > 5) {
                    ShowUOM(type, "Warehouse Wise Stock Journal", 0, sProduct_quantity, sProduct_SaleUom, packing_quantity, packing_saleUOM, strProductID, slno, isOverideConvertion, gridprodqty, gridPackingQty);
                }
                else {
                    if (WHType == 'DWH') {
                        gridDEstination.batchEditApi.EndEdit();
                        gridDEstination.batchEditApi.StartEdit(globalRowIndexDestination, 8);
                        //setTimeout(function () {
                        //    gridDEstination.batchEditApi.StartEdit(globalRowIndexDestination, 8);
                        //}, 200);
                    }
                    else {
                        grid.batchEditApi.EndEdit();
                        //grid.batchEditApi.StartEdit(globalRowIndexDestination, 8);
                        setTimeout(function () {
                            grid.batchEditApi.StartEdit(globalRowIndexDestination, 8);
                        }, 200);
                    }


                }

            }
        });
    }
    else {

        if (WHType == 'SWH') {
            actionQry = 'WSjPackingQtyEditforSWH';
        }
        else {
            actionQry = 'WSjPackingQtyEditforDWH';
        }
        TransferId = $('#hdAdjustmentId').val();
        $.ajax({
            type: "POST",
            url: "Services/Master.asmx/GetMultiUOMDetails",
            data: JSON.stringify({ orderid: splitData[0], action: actionQry, module: 'WarehouseWiseStockJournal', strKey: TransferId }),
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
                var slno = '';
                //var slno = (grid.GetEditor('SrlNo').GetValue() != null) ? grid.GetEditor('SrlNo').GetValue() : "";
                if (WHType == 'SWH') {
                    slno = (grid.GetEditor('SrlNo').GetValue() != null) ? grid.GetEditor('SrlNo').GetValue() : "";
                }
                else if (WHType == 'DWH') {
                    slno = (gridDEstination.GetEditor('SrlNo').GetValue() != null) ? gridDEstination.GetEditor('SrlNo').GetValue() : "";
                }

                var strProductID = splitData[0];

                var isOverideConvertion = SpliteDetails[4];
                var packing_saleUOM = SpliteDetails[2];
                var sProduct_SaleUom = SpliteDetails[3];
                var sProduct_quantity = SpliteDetails[0];
                var packing_quantity = SpliteDetails[1];


                gridPackingQty = SpliteDetails[6];
                gridprodqty = SpliteDetails[7];

                if (ShowUOMConversionInEntry == "1" && SpliteDetails.length > 5) {
                    ShowUOM(type, "Warehouse Wise Stock Journal", 0, sProduct_quantity, sProduct_SaleUom, packing_quantity, packing_saleUOM, strProductID, slno, isOverideConvertion, gridprodqty, gridPackingQty);
                }
                else {
                    actionQry = 'WSJPackingQtyAdd';
                    $.ajax({
                        type: "POST",
                        url: "Services/Master.asmx/GetMultiUOMDetails",
                        data: JSON.stringify({ orderid: splitData[0], action: actionQry, module: 'WarehouseWiseStockJournal', strKey: TransferId }),
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
                            var slno = '';
                            // var slno = (grid.GetEditor('SrlNo').GetValue() != null) ? grid.GetEditor('SrlNo').GetValue() : "";
                            var strProductID = splitData[0];

                            var isOverideConvertion = SpliteDetails[4];
                            var packing_saleUOM = SpliteDetails[2];
                            var sProduct_SaleUom = SpliteDetails[3];
                            var sProduct_quantity = SpliteDetails[0];
                            var packing_quantity = SpliteDetails[1];

                            if (WHType == 'SWH') {
                                slno = (grid.GetEditor('SrlNo').GetValue() != null) ? grid.GetEditor('SrlNo').GetValue() : "";
                                gridprodqty = grid.GetEditor("TransferQuantity").GetValue();
                            }
                            else if (WHType == 'DWH') {
                                slno = (gridDEstination.GetEditor('SrlNo').GetValue() != null) ? gridDEstination.GetEditor('SrlNo').GetValue() : "";
                                gridprodqty = gridDEstination.GetEditor("DestQuantity").GetValue();
                            }

                            if (ShowUOMConversionInEntry == "1" && SpliteDetails.length > 5) {
                                ShowUOM(type, "Warehouse Wise Stock Journal", 0, sProduct_quantity, sProduct_SaleUom, packing_quantity, packing_saleUOM, strProductID, slno, isOverideConvertion, gridprodqty, gridPackingQty);
                            }
                            else {
                                if (WHType == 'DWH') {
                                    grid.batchEditApi.EndEdit();
                                    gridDEstination.batchEditApi.StartEdit(globalRowIndexDestination, 8);
                                }

                                else {
                                    grid.batchEditApi.EndEdit();
                                    //grid.batchEditApi.StartEdit(globalRowIndexDestination, 8);
                                    setTimeout(function () {
                                        grid.batchEditApi.StartEdit(globalRowIndexDestination, 8);
                                    }, 200);
                                }
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
      grid.batchEditApi.StartEdit(globalRowIndex,9);
    var Quantity = grid.GetEditor("TransferQuantity").GetValue();
    var strProductID = grid.GetEditor("ProductID").GetValue();
    var splitData = strProductID.split('||@||');
    if (strProductID != null) {

        var wHType = 'SWH';
        //ShowUOMPOpup(wHType);

        if ($('#hdnShowUOMConversionInEntry').val() == 0) {
            grid.batchEditApi.EndEdit();
            grid.batchEditApi.StartEdit(globalRowIndex, 6);


            if (parseFloat(grid.GetEditor("TransferQuantity").GetValue()) > parseFloat(grid.GetEditor("AvlStkSourceWH").GetValue())) {

                $.ajax({
                    type: "POST",
                    url: "WarehousewiseStockJournalAddOUT.aspx/GetNegativeStockByProductID",
                    data: JSON.stringify({ ProductId: splitData[0] }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: false,
                    success: function (msg) {
                        var data = msg.d;                        
                        $('#hdnStockNeg').val(data.trim());                    
                        if (data.trim() == 'W') {
                            jConfirm('Product is going negative do you want to proceed?', 'Confirmation Dialog', function (r) {
                                if (r == true) {
                                    if ($("#hdnRateRequiredStockOUT").val() == "1") {
                                        StockValuation();
                                    }
                                    grid.batchEditApi.EndEdit();
                                    setTimeout(function () {
                                        grid.batchEditApi.StartEdit(globalRowIndex, 9);
                                    }, 200);
                                }
                                else {
                                    grid.batchEditApi.StartEdit(globalRowIndex);
                                    grid.GetEditor("TransferQuantity").SetValue("0.0000");
                                    $("#txtQuantity").val("0.0000");
                                    $("#txtPacking").val("0.0000");
                                    cbnrLblTotalQty.SetText("0.0000");
                                    cbnrLblAltTotalQty.SetText("0.0000");
                                    grid.batchEditApi.EndEdit();
                                    setTimeout(function () {
                                        grid.batchEditApi.StartEdit(globalRowIndex, 8);
                                    }, 200);
                                }
                            });
                        }
                        else if (data.trim() == 'B') {
                            jAlert('Product is going negative can not proceed');
                            grid.GetEditor("TransferQuantity").SetValue("0.0000");
                            $("#txtQuantity").val("0.0000");
                            $("#txtPacking").val("0.0000");
                            cbnrLblTotalQty.SetText("0.0000");
                            cbnrLblAltTotalQty.SetText("0.0000");
                            grid.batchEditApi.EndEdit();
                            setTimeout(function () {
                                grid.batchEditApi.StartEdit(globalRowIndex, 8);
                            }, 200);
                        }
                        else {
                            grid.batchEditApi.EndEdit();
                            if ($("#hdnRateRequiredStockOUT").val() == "1") {
                                $.ajax({
                                    type: "POST",
                                    url: "WarehousewiseStockJournalAddOUT.aspx/GetStockValuation",
                                    data: JSON.stringify({ ProductId: splitData[0] }),
                                    contentType: "application/json; charset=utf-8",
                                    dataType: "json",
                                    async: false,
                                    success: function (msg) {
                                        var ObjData = msg.d;
                                        if (ObjData.length > 0) {
                                            $.ajax({
                                                type: "POST",
                                                url: "WarehousewiseStockJournalAddOUT.aspx/GetStockValuationAmount",
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
                                grid.batchEditApi.StartEdit(globalRowIndex, 9);
                            }, 200);
                        }
                    }
                });

            }
            else {
                //Rev Subhra 06-08-2019
                $('#hdnStockNeg').val('NA');
                //End of Rev

                if ($("#hdnRateRequiredStockIN").val() == "1") {
                    StockValuation();                    
                }
                grid.batchEditApi.EndEdit();
                setTimeout(function () {
                    grid.batchEditApi.StartEdit(globalRowIndex, 9);
                }, 200);

            }
        }
        else {
            ShowUOMPOpup(wHType);
        }

    }


}
function QuantityTextChangeGotFocus() {
    //grid.batchEditApi.StartEdit(globalRowIndex,9);
    var Quantity = grid.GetEditor("TransferQuantity").GetValue();
    var strProductID = grid.GetEditor("ProductID").GetValue();
    if (strProductID != null) {
        var wHType = 'SWH';     
            if ($('#hdnShowUOMConversionInEntry').val() == 0) {
                grid.batchEditApi.EndEdit();
                grid.batchEditApi.StartEdit(globalRowIndex, 6);
            }
            else {
                ShowUOMPOpup(wHType);
            }
           
    }
}
function DailyStockQuantity() {
    var strProductID = grid.GetEditor("ProductID").GetValue();
    var splitData = strProductID.split('||@||');
    var BranchId = $('#ddlBranch').val();
    var stockdate = cdtTDate.GetDate();
    $.ajax({
        type: "POST",
        url: "WarehousewiseStockJournalAdd.aspx/GetDailyStock",
        data: JSON.stringify({ ProductId: splitData[0], BranchId: BranchId, stkdate: stockdate }),
        contentType: "application/json; charset=utf-8",
        async: false,
        dataType: "json",
        success: function (msg) {
            var data = msg.d;
            var DailyQuantity = data.split("~")[0].toString();
            var DailAltyQuantity = data.split("~")[1].toString();
            $("#lblDailyStkQty").text(DailyQuantity);
            $("#lblDailyAltStkQty").text(DailAltyQuantity);
            grid.batchEditApi.EndEdit();
            setTimeout(function () {
                grid.batchEditApi.StartEdit(globalRowIndex, 8);
            }, 200);

        }
    });
    return true;
}
function DestQuantityTextChange() {
    var wHType = 'DWH';
    if ($('#hdnShowUOMConversionInEntry').val() == 0) {
        gridDEstination.batchEditApi.EndEdit();
        gridDEstination.batchEditApi.StartEdit(globalRowIndexDestination, 8);
    }
    else {
        ShowUOMPOpup(wHType);
    }
}
function DestQuantityTextChangeGotFocus() {
    var wHType = 'DWH';
    if (DailyDestStockQuantity() == true) {
        if ($('#hdnShowUOMConversionInEntry').val() == 0) {
            gridDEstination.batchEditApi.EndEdit();
            gridDEstination.batchEditApi.StartEdit(globalRowIndexDestination, 8);
        }
        else {
            ShowUOMPOpup(wHType);
        }
    }
}
function DailyDestStockQuantity() {
    var strProductID = gridDEstination.GetEditor("DestProductID").GetValue();
    var splitData = strProductID.split('||@||');
    var BranchId = $('#ddlBranchTo').val();
    var stockdate = cdtTDate.GetDate();
    $.ajax({
        type: "POST",
        url: "WarehousewiseStockJournalAdd.aspx/GetDailyStock",
        data: JSON.stringify({ ProductId: splitData[0], BranchId: BranchId, stkdate: stockdate }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (msg) {
            var data = msg.d;
            var DailyQuantity = data.split("~")[0].toString();
            var DailAltyQuantity = data.split("~")[1].toString();
            $("#lblDailyStkQty").text(DailyQuantity);
            $("#lblDailyAltStkQty").text(DailAltyQuantity);

            gridDEstination.batchEditApi.EndEdit();

        }
    });
    return true;
}
function StockValuation() {
    grid.batchEditApi.StartEdit(globalRowIndex);
    var Quantity = grid.GetEditor("TransferQuantity").GetValue();
    var strProductID = grid.GetEditor("ProductID").GetValue();

    var splitData = strProductID.split('||@||');
    var BranchId = $('#ddlBranch').val();
    var Amount = '';
    var Rate = '';

    $.ajax({
        type: "POST",
        url: "WarehousewiseStockJournalAdd.aspx/GetStockValuation",
        data: JSON.stringify({ ProductId: splitData[0] }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (msg) {


            var ObjData = msg.d;
            if (ObjData.length > 0) {
                $.ajax({
                    type: "POST",
                    url: "WarehousewiseStockJournalAdd.aspx/GetStockValuationAmount",
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

                            var uniqueIndex = globalRowIndexDestination;
                            SetTotalValue(uniqueIndex, 6);
                        }
                    }

                });
            }
        }

    });

}
function StockValuationDestination() {
    gridDEstination.batchEditApi.StartEdit(globalRowIndexDestination);
    var Quantity = gridDEstination.GetEditor("DestQuantity").GetValue();
    var strProductID = gridDEstination.GetEditor("DestProductID").GetValue();

    var splitData = strProductID.split('||@||');
    var BranchId = $('#ddlBranch').val();
    var Amount = '';
    var Rate = '';


    $.ajax({
        type: "POST",
        url: "WarehousewiseStockJournalAdd.aspx/GetStockValuation",
        data: JSON.stringify({ ProductId: splitData[0] }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (msg) {


            var ObjData = msg.d;
            if (ObjData.length > 0) {
                $.ajax({
                    type: "POST",
                    url: "WarehousewiseStockJournalAdd.aspx/GetStockValuationAmount",
                    data: JSON.stringify({ Pro_Id: splitData[0], Qty: Quantity, Valuationsign: ObjData, Fromdate: cdtTDate.date.format('yyyy-MM-dd'), BranchId: BranchId }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: false,
                    success: function (msg1) {
                        var ObjData1 = msg1.d;
                        if (ObjData1.length > 0) {
                            Amount = (ObjData1 * 1);
                            Rate = Amount / Quantity;
                            var tbRate = gridDEstination.GetEditor("DestRate");
                            tbRate.SetValue(Rate);


                            var tbValue = gridDEstination.GetEditor("DestValue");
                            tbValue.SetValue(Amount);

                            var DuniqueIndex = globalRowIndexDestination;
                            SetDestTotalValue(DuniqueIndex, 6);
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

    var WHType = $('#hdnWHtype').val();
    issavePacking = 1;

    var Amount = '';
    var Rate = '';
    var BranchId = '';
    if (WHType == 'SWH') {
        grid.batchEditApi.StartEdit(globalRowIndex);
        if (aarr.length > 0) {
            aarrSWH = aarr;
        }

        grid.GetEditor('TransferQuantity').SetValue(Quantity);
        //Rev Subhra 30-07-2019
        grid.GetEditor('PackingQty').SetValue(packing);
        var uniqueIndex = globalRowIndex;
        SetTotalQuantity(uniqueIndex, 6);
        SetAltTotalQuantity(uniqueIndex, 6);
        SetTotalValue(uniqueIndex, 6);
        grid.batchEditApi.StartEdit(uniqueIndex);
        //End of Rev Subhra 30-07-2019
        BranchId = $('#ddlBranch').val();

        if (parseFloat(grid.GetEditor("TransferQuantity").GetValue()) > parseFloat(grid.GetEditor("AvlStkSourceWH").GetValue())) {

            $.ajax({
                type: "POST",
                url: "WarehousewiseStockJournalAdd.aspx/GetNegativeStockByProductID",
                data: JSON.stringify({ ProductId: productid }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (msg) {
                    var data = msg.d;
                    //Rev Subhra 06-08-2019
                    $('#hdnStockNeg').val(data.trim());
                    //End of Rev
                    if (data.trim() == 'W') {
                        jConfirm('Product is going negative do you want to proceed?', 'Confirmation Dialog', function (r) {
                            if (r == true) {

                                if ($("#hdnRateRequiredStockOUT").val() == "1") {
                                    StockValuation();                                   
                                }
                                grid.batchEditApi.EndEdit();
                                setTimeout(function () {
                                    grid.batchEditApi.StartEdit(globalRowIndex, 9);
                                }, 200);


                            }
                            else {
                                grid.batchEditApi.StartEdit(globalRowIndex);
                                grid.GetEditor("TransferQuantity").SetValue("0.0000");
                                $("#txtQuantity").val("0.0000");
                                $("#txtPacking").val("0.0000");
                                cbnrLblTotalQty.SetText("0.0000");
                                cbnrLblAltTotalQty.SetText("0.0000");

                                grid.batchEditApi.EndEdit();
                                ShowUOMPOpup(WHType);
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
                        cbnrLblTotalQty.SetText("0.0000");
                        cbnrLblAltTotalQty.SetText("0.0000");
                        grid.batchEditApi.EndEdit();
                        setTimeout(function () {
                            grid.batchEditApi.StartEdit(globalRowIndex, 9);
                        }, 200);
                    }
                    else {
                        grid.batchEditApi.EndEdit();
                        if ($("#hdnRateRequiredStockOUT").val() == "1") {                          
                            $.ajax({
                                type: "POST",
                                url: "WarehousewiseStockJournalAdd.aspx/GetStockValuation",
                                data: JSON.stringify({ ProductId: productid }),
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",
                                async: false,
                                success: function (msg) {


                                    var ObjData = msg.d;
                                    if (ObjData.length > 0) {
                                        $.ajax({
                                            type: "POST",
                                            url: "WarehousewiseStockJournalAdd.aspx/GetStockValuationAmount",
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
                        }
                        setTimeout(function () {
                            grid.batchEditApi.StartEdit(globalRowIndex, 9);
                        }, 200);
                    }
                }
            });

        }
        else {
            //Rev Subhra 06-08-2019
            $('#hdnStockNeg').val('NA');
            //End of Rev
            if ($("#hdnRateRequiredStockOUT").val() == "1") {
                StockValuation();               
            }
            grid.batchEditApi.EndEdit();
            setTimeout(function () {
                grid.batchEditApi.StartEdit(globalRowIndex, 9);
            }, 200);
        }
    }
   
}

//Rev Subhra 30-07-2019
function SetTotalValue(vindex, inx) {
    var count = grid.GetVisibleRowsOnPage();
    var totalValue = 0;

    for (var i = 0; i < count + 10; i++) {
        if (grid.GetRow(i)) {
            if (grid.GetRow(i).style.display != "none") {
                grid.batchEditApi.StartEdit(i, 2);
                totalValue = totalValue + DecimalRoundoff(grid.GetEditor("Value").GetValue(), 4);
            }
        }
    }

    for (i = -1; i > -count - 10; i--) {
        if (grid.GetRow(i)) {
            if (grid.GetRow(i).style.display != "none") {
                grid.batchEditApi.StartEdit(i, 2);
                totalValue = totalValue + DecimalRoundoff(grid.GetEditor("Value").GetValue(), 4);


            }
        }
    }
    cbnrLblTotalValue.SetText(DecimalRoundoff(totalValue, 4).toFixed(4));
    //setTimeout(function () {
    //    grid.batchEditApi.EndEdit()
    //    grid.batchEditApi.StartEdit(vindex, inx);
    //}, 200);
}
function SetTotalQuantity(inx, vindex) {
    var count = grid.GetVisibleRowsOnPage();
    var totalQuantity = 0;

    for (var i = 0; i < count + 10; i++) {
        if (grid.GetRow(i)) {
            if (grid.GetRow(i).style.display != "none") {
                grid.batchEditApi.StartEdit(i, 2);
                totalQuantity = totalQuantity + DecimalRoundoff(grid.GetEditor("TransferQuantity").GetValue(), 4);
            }
        }
    }

    for (i = -1; i > -count - 10; i--) {
        if (grid.GetRow(i)) {
            if (grid.GetRow(i).style.display != "none") {
                grid.batchEditApi.StartEdit(i, 2);
                totalQuantity = totalQuantity + DecimalRoundoff(grid.GetEditor("TransferQuantity").GetValue(), 4);
            }
        }
    }
    cbnrLblTotalQty.SetText(DecimalRoundoff(totalQuantity, 4).toFixed(4));
}
function SetAltTotalQuantity(inx, vindex) {

    var count = grid.GetVisibleRowsOnPage();
    var totalAltQuantity = 0;

    for (var i = 0; i < count + 10; i++) {
        if (grid.GetRow(i)) {
            if (grid.GetRow(i).style.display != "none") {
                grid.batchEditApi.StartEdit(i, 2);
                totalAltQuantity = totalAltQuantity + DecimalRoundoff(grid.GetEditor("PackingQty").GetValue(), 4);
            }
        }
    }

    for (i = -1; i > -count - 10; i--) {
        if (grid.GetRow(i)) {
            if (grid.GetRow(i).style.display != "none") {
                grid.batchEditApi.StartEdit(i, 2);
                totalAltQuantity = totalAltQuantity + DecimalRoundoff(grid.GetEditor("PackingQty").GetValue(), 4);
            }
        }
    }
    cbnrLblAltTotalQty.SetText(DecimalRoundoff(totalAltQuantity, 4).toFixed(4));
}
function SetDestTotalValue(vindex, inx) {
    var count = gridDEstination.GetVisibleRowsOnPage();
    var totalValue = 0;
    for (var i = 0; i < count + 10; i++) {
        if (gridDEstination.GetRow(i)) {
            if (gridDEstination.GetRow(i).style.display != "none") {
                gridDEstination.batchEditApi.StartEdit(i, 2);
                totalValue = totalValue + DecimalRoundoff(gridDEstination.GetEditor("DestValue").GetValue(), 4);
            }
        }
    }

    for (i = -1; i > -count - 10; i--) {
        if (gridDEstination.GetRow(i)) {
            if (gridDEstination.GetRow(i).style.display != "none") {
                gridDEstination.batchEditApi.StartEdit(i, 2);
                totalValue = totalValue + DecimalRoundoff(gridDEstination.GetEditor("DestValue").GetValue(), 4);
            }
        }
    }
    cbnrLblDestTotalValue.SetText(DecimalRoundoff(totalValue, 4).toFixed(4));

    setTimeout(function () {
        gridDEstination.batchEditApi.EndEdit()
        gridDEstination.batchEditApi.StartEdit(vindex, inx);
    }, 200);

}
function SetDestTotalQuantity(inx, vindex) {
    var count = gridDEstination.GetVisibleRowsOnPage();
    var totalQuantity = 0;
    for (var i = 0; i < count + 10; i++) {
        if (gridDEstination.GetRow(i)) {
            if (gridDEstination.GetRow(i).style.display != "none") {
                gridDEstination.batchEditApi.StartEdit(i, 2);
                totalQuantity = totalQuantity + DecimalRoundoff(gridDEstination.GetEditor("DestQuantity").GetValue(), 4);
            }
        }
    }

    for (i = -1; i > -count - 10; i--) {
        if (gridDEstination.GetRow(i)) {
            if (gridDEstination.GetRow(i).style.display != "none") {
                gridDEstination.batchEditApi.StartEdit(i, 2);
                totalQuantity = totalQuantity + DecimalRoundoff(gridDEstination.GetEditor("DestQuantity").GetValue(), 4);
            }
        }
    }
    cbnrLblDestTotalQty.SetText(DecimalRoundoff(totalQuantity, 4).toFixed(4));
}
function SetDestAltTotalQuantity(inx, vindex, altqty) {

    var count = gridDEstination.GetVisibleRowsOnPage();
    var totalDestAltQuantity = 0;
    for (var i = 0; i < count + 10; i++) {
        if (gridDEstination.GetRow(i)) {
            if (gridDEstination.GetRow(i).style.display != "none") {
                gridDEstination.batchEditApi.StartEdit(i, 2);
                totalDestAltQuantity = totalDestAltQuantity + DecimalRoundoff(gridDEstination.GetEditor("DestPackingQty").GetValue(), 4);
            }
        }
    }

    for (i = -1; i > -count - 10; i--) {
        if (gridDEstination.GetRow(i)) {
            if (gridDEstination.GetRow(i).style.display != "none") {
                gridDEstination.batchEditApi.StartEdit(i, 2);
                totalDestAltQuantity = totalDestAltQuantity + DecimalRoundoff(gridDEstination.GetEditor("DestPackingQty").GetValue(), 4);
            }
        }
    }

    cbnrLblDestAltTotalQty.SetText(DecimalRoundoff(totalDestAltQuantity, 4).toFixed(4));
}
//End of Rev Subhra 30-07-2019


function GridAddnewRow() {
    grid.AddNewRow();
    RowCount = RowCount + 1;
    grid.GetEditor("SrlNo").SetText(grid.GetVisibleItemsOnPage());
    grid.GetEditor("ActualSL").SetText(RowCount);
}

function GridAddnewRowDistination() {
    gridDEstination.AddNewRow();
    RowCountDest = RowCountDest + 1;
    gridDEstination.GetEditor("SrlNo").SetText(gridDEstination.GetVisibleItemsOnPage());
    gridDEstination.GetEditor("ActualDestSL").SetText(RowCountDest);
}

function AllControlInitilize() {
    if (canCallBack) {

        if ($('#hdAddEdit').val() == "Add") {
            cCmbScheme.Focus();
            GridAddnewRow();
            GridAddnewRowDistination();
            setTimeout(function () { cCmbScheme.Focus(); }, 500);
            cbtnSaveRecords.SetVisible(true)
            cbtn_SaveRecords.SetVisible(true)

        } else {
            RowCount = parseInt($("#HiddenRowCount").val());
            RowCountDest = parseInt($("#DestHiddenRowCount").val());
            SuffleRows();
            SuffleRowsgridDEstination();


            if (grid.GetVisibleItemsOnPage() == 0) {
                GridAddnewRow();
            }
            if (gridDEstination.GetVisibleItemsOnPage() == 0) {
                GridAddnewRowDistination();
            }


            cbtnSaveRecords.SetVisible(false);
            cbtn_SaveRecords.SetVisible(true);
        }

        canCallBack = false;
    }
}


function SuffleRows() {
    for (var i = 0; i < 1000; i++) {
        if (grid.GetRow(i)) {
            if (grid.GetRow(i).style.display != "none") {
                grid.batchEditApi.StartEdit(i, 2);
                //grid.GetEditor("UpdateEdit").SetText(grid.GetEditor("UpdateEdit").GetText() + i);
            }
        }
    }

    for (i = -1; i > -1000; i--) {
        if (grid.GetRow(i)) {
            if (grid.GetRow(i).style.display != "none") {
                grid.batchEditApi.StartEdit(i, 2);
                // grid.GetEditor("UpdateEdit").SetText(grid.GetEditor("UpdateEdit").GetText() + i);
            }
        }
    }
}

function SuffleRowsgridDEstination() {
    for (var i = 0; i < 1000; i++) {
        if (gridDEstination.GetRow(i)) {
            if (gridDEstination.GetRow(i).style.display != "none") {
                gridDEstination.batchEditApi.StartEdit(i, 2);
                //gridDEstination.GetEditor("UpdateEdit").SetText(gridDEstination.GetEditor("UpdateEdit").GetText() + i);
            }
        }
    }

    for (i = -1; i > -1000; i--) {
        if (gridDEstination.GetRow(i)) {
            if (gridDEstination.GetRow(i).style.display != "none") {
                gridDEstination.batchEditApi.StartEdit(i, 2);
                //gridDEstination.GetEditor("UpdateEdit").SetText(gridDEstination.GetEditor("UpdateEdit").GetText() + i);
            }
        }
    }
}
function gridCustomButtonClick(s, e) {
    debugger;
    if (e.buttonID == 'CustomDelete') {
        if (grid.GetVisibleRowsOnPage() > 1) {
            grid.batchEditApi.StartEdit(e.visibleIndex);
            //Rev Subhra 05-08-2019
            //SetUOMDataInArray('CustomDelete', grid.GetEditor('SrlNo').GetValue(), grid.GetEditor('ProductID').GetText());
            //End of Rev 
            grid.DeleteRow(e.visibleIndex);
            SuffuleSerialNumber();
            //Rev Subhra 02-08-2019
            SetTotalQuantity();
            SetAltTotalQuantity();
            SetTotalValue();
            //End of Rev 
        }
    }
    else if (e.buttonID == 'CustomAddNewRow') {
        GridAddnewRow();
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
            var StkQuantityValue = QuantityValue;
            var Ptype = SpliteDetails[3];
            $('#hdfProductType').val(Ptype);
            document.getElementById("lblProductName").innerHTML = strProductName;
            document.getElementById("txt_SalesAmount").innerHTML = QuantityValue;
            document.getElementById("txt_SalesUOM").innerHTML = strUOM;
            document.getElementById("txt_StockAmount").innerHTML = StkQuantityValue;
            document.getElementById("txt_StockUOM").innerHTML = strStkUOM;
            $('#hdfProductID').val(strProductID);
            $('#hdfProductSerialID').val(SrlNo);
            $('#hdfProductSerialID').val(SrlNo);
            $('#hdnProductQuantity').val(QuantityValue);

            //Rev 1.0
            var objectToPass = {}
            var product = $("#hdfProductID").val();
            objectToPass.ProductID = hdfProductID.value;
            $.ajax({
                type: "POST",
                url: "../Activities/Services/Master.asmx/GetUom",
                data: JSON.stringify(objectToPass),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var returnObject = msg.d;
                    var UOMId = returnObject.uom_id;
                    var UOMName = returnObject.UOM_Name;
                    if (returnObject) {
                        SetDataSourceOnComboBoxandSetVal(ccmbPackingUom1, returnObject.uom, UOMId);                       
                        ccmbPackingUom1.SetEnabled(false);                        
                    }
                }
            });
              //Rev 1.0 End
            if (Ptype == "W") {
                div_Warehouse.style.display = 'block';
                div_Batch.style.display = 'none';
                div_Serial.style.display = 'none';
                div_Quantity.style.display = 'block';
                cCmbWarehouse.PerformCallback('BindWarehouse~' + SourceWarehouseID);
                cGrdWarehouse.PerformCallback('Display~' + SrlNo);
                $("#ADelete").css("display", "block");
                SelectedWarehouseID = "0";
                jAlert("No Batch or Serial is activated !");
            }
            else if (Ptype == "B") {
                div_Warehouse.style.display = 'none';
                div_Batch.style.display = 'block';
                div_Serial.style.display = 'none';
                div_Quantity.style.display = 'block';
                cCmbBatch.PerformCallback('BindBatch~' + "0");
                cGrdWarehouse.PerformCallback('Display~' + SrlNo);
                $("#ADelete").css("display", "block");
                SelectedWarehouseID = "0";
                jAlert("No Warehouse is activaed !");
            }
            else if (Ptype == "S") {
                div_Warehouse.style.display = 'none';
                div_Batch.style.display = 'none';
                div_Serial.style.display = 'block';
                div_Quantity.style.display = 'none';
                checkListBox.PerformCallback('BindSerial~' + "0" + '~' + "0");
                cGrdWarehouse.PerformCallback('Display~' + SrlNo);
                $("#ADelete").css("display", "none");
                SelectedWarehouseID = "0";
                jAlert("No Warehouse is activaed !");
            }
            else if (Ptype == "WB") {
                div_Warehouse.style.display = 'block';
                div_Batch.style.display = 'block';
                div_Serial.style.display = 'none';
                div_Quantity.style.display = 'block';
                div_MFGDATE.style.display = 'block';
                cCmbWarehouse.PerformCallback('BindWarehouse~' + SourceWarehouseID);
                cGrdWarehouse.PerformCallback('Display~' + SrlNo);
                $("#ADelete").css("display", "block");
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
                $("#ADelete").css("display", "none");
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
                $("#ADelete").css("display", "none");
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
                $("#ADelete").css("display", "none");
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

}
//Rev 1.0
function ChangePackingByQuantityinjs() {

    if ($("#hdnShowUOMConversionInEntry").val() == "1") {
        var Quantity = ctxtQuantity.GetValue();
        var packing = $('#txtPacking').val();
        if (packing == null || packing == '') {
            $('#txtPacking').val(parseFloat(0).toFixed(4));
            packing = $('#txtPacking').val();
        }
        if (Quantity == null || Quantity == '') {
            $(e).val(parseFloat(0).toFixed(4));
            Quantity = ctxtQuantity.GetValue();
        }
        var packingqty = parseFloat($('#hdnpackingqty').val()).toFixed(4);        
        var uomfac_Qty_to_stock = $('#hdnuomFactor').val();       
        var calcQuantity = parseFloat(Quantity * uomfac_Qty_to_stock).toFixed(4);    
        CtxtPacking.SetText(calcQuantity);
        ChkDataDigitCount(Quantity);
    }
}
function ChkDataDigitCount(e) {
    var data = $(e).val();
    $(e).val(parseFloat(data).toFixed(4));
}
function ChangeQuantityByPacking1() {
    var isOverideConvertion = $('#hdnisOverideConvertion').val();
    if (isOverideConvertion == '1') {
        var packing = CtxtPacking.GetValue();
        var Quantity = ctxtQuantity.GetValue();
        if (packing == null || packing == '') {
            $(e).val(parseFloat(0).toFixed(4));
            packing = CtxtPacking.GetValue();
        }
        if (Quantity == null || Quantity == '') {
            ctxtQuantity.SetValue(parseFloat(0).toFixed(4));
            Quantity = ctxtQuantity.GetValue();
        }
        var packingqty = parseFloat($('#hdnpackingqty').val()).toFixed(4);       
        var uomfac_stock_to_qty = $('#hdnuomFactor').val();        
        var calcQuantity = 0;
        if (parseFloat(uomfac_stock_to_qty) != 0) {
            calcQuantity = parseFloat(packing / uomfac_stock_to_qty).toFixed(4);
        }        
        ctxtQuantity.SetValue(calcQuantity);
    }
    ChkDataDigitCount(Quantity);
}
function QuantityGotFocus(s, e) {
    var ProductID = $('#hdfProductID').val();
    var Branch = $('#ddlBranch').val();
    var WarehouseID = cCmbWarehouse.GetValue();
    var ConvertionOverideVisible = $('#hdnConvertionOverideVisible').val();
    var ShowUOMConversionInEntry = $('#hdnShowUOMConversionInEntry').val();
    var type = 'add';
    var actionQry = 'WarehouseOpeningBalanceProduct';
    var GetserviceURL = "../Activities/Services/Master.asmx/GetMultiUOMDetails";
    $.ajax({
        type: "POST",
        url: GetserviceURL,
        data: JSON.stringify({ orderid: ProductID, action: actionQry, module: 'OpeningBalances', strKey: "" }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var SpliteDetails = msg.d.split("||@||");
            var IsInventory = '';
            if (SpliteDetails[5] == "1") {
                IsInventory = 'Yes';
            }
            var gridprodqty = parseFloat(ctxtQuantity.GetText()).toFixed(4);
            var gridPackingQty = '';
            var slno = WarehouseID;
            var strProductID = ProductID;
            var isOverideConvertion = SpliteDetails[4];
            var packing_saleUOM = SpliteDetails[2];
            var sProduct_SaleUom = SpliteDetails[3];
            var sProduct_quantity = SpliteDetails[0];
            var packing_quantity = SpliteDetails[1];
            var uomfactor = 0
            var prodquantity = sProduct_quantity;
            var packingqty = packing_quantity;
            $('#hdnpackingqty').val(packingqty);
            if (prodquantity != 0 && packingqty != 0) {
                uomfactor = parseFloat(packingqty / prodquantity).toFixed(4);
                $('#hdnuomFactor').val(parseFloat(packingqty / prodquantity));
            }
            else {
                $('#hdnuomFactor').val(0);
            }
            $('#hdnisOverideConvertion').val(isOverideConvertion);
        }
    });

}
function SetDataSourceOnComboBoxandSetVal(ControlObject, Source, id) {
    ControlObject.ClearItems();
    for (var count = 0; count < Source.length; count++) {
        ControlObject.AddItem(Source[count].UOM_Name, Source[count].UOM_Id);
    }
    ControlObject.SetValue(id);
    // ControlObject.SetSelectedIndex(0);
}

//Rev 1.0 End
function SetUOMDataInArray(WHType, srlno, prodid) {
    var productid = prodid.split("||@||")[0];
    if (WHType == 'CustomDelete') {
        var Mainarr = $.grep(aarr, function (element) { return element.productid != productid && element.slno != srlno });
        var Warr = $.grep(aarrSWH, function (element) { return element.productid != productid && element.slno != srlno });
        var i = 0;
        var j = 0;
        $.each(Mainarr, function (index, value) {
            i = i + 1;
            value.slno = i;
        });
        aarr = Mainarr;

        $.each(Warr, function (index, value) {
            j = j + 1;
            value.slno = j;
        });
        aarrSWH = Warr;
    }
    else if (WHType == 'CustomDeleteDest') {
        var MainDarr = $.grep(aarr, function (element) { return element.productid != productid && element.slno != srlno });
        var DWarr = $.grep(aarrDWH, function (element) { return element.productid != productid && element.slno != srlno });
        var i = 0;
        var j = 0;
        $.each(MainDarr, function (index, value) {
            i = i + 1;
            value.slno = i;
        });
        aarr = MainDarr;

        $.each(DWarr, function (index, value) {
            j = j + 1;
            value.slno = j;
        });
        aarrDWH = DWarr;
    }

}
function gridDEstinationCustomButtonClick(s, e) {
    if (e.buttonID == 'CustomDeleteDest') {
        if (gridDEstination.GetVisibleRowsOnPage() > 1) {
            gridDEstination.batchEditApi.StartEdit(e.visibleIndex);
            //Rev Subhra 05-08-2019
            //SetUOMDataInArray('CustomDeleteDest', gridDEstination.GetEditor('SrlNo').GetValue(), gridDEstination.GetEditor('DestProductID').GetText());
            //End of Rev 
            gridDEstination.DeleteRow(e.visibleIndex);
            SuffuleSerialNumberGridDestination();
            //Rev Subhra 02-08-2019
            SetDestTotalQuantity();
            SetDestAltTotalQuantity();
            var uniqueIndex = globalRowIndexDestination;
            SetDestTotalValue(uniqueIndex, 12);
            //End of Rev 

        }
    }
    else if (e.buttonID == 'CustomAddNewRowDest') {
        GridAddnewRowDistination();
    }
    else if (e.buttonID == 'CustomWarehouseDest') {
        var index = e.visibleIndex;
        gridDEstination.batchEditApi.StartEdit(index, 2)
        Warehouseindex = index;
        var SrlNo = (gridDEstination.GetEditor('SrlNo').GetValue() != null) ? gridDEstination.GetEditor('SrlNo').GetValue() : "";
        var ProductID = (gridDEstination.GetEditor('DestProductID').GetText() != null) ? gridDEstination.GetEditor('DestProductID').GetText() : "0";
        var QuantityValue = (gridDEstination.GetEditor('DestQuantity').GetValue() != null) ? gridDEstination.GetEditor('DestQuantity').GetValue() : "0";
        var SourceWarehouseID = (gridDEstination.GetEditor('DestinationWarehouseID').GetValue() != null) ? gridDEstination.GetEditor('DestinationWarehouseID').GetValue() : "0";

        $("#spnCmbWarehouseDest").hide();
        $("#spnCmbBatchDest").hide();
        $("#spncheckComboBoxDest").hide();
        $("#spntxtQuantityDest").hide();

        if (ProductID != "" && parseFloat(QuantityValue) != 0) {
            var SpliteDetails = ProductID.split("||@||");
            var strProductID = SpliteDetails[0];
            var strDescription = SpliteDetails[1];
            var strUOM = SpliteDetails[2];
            var strStkUOM = SpliteDetails[4];
            var strMultiplier = SpliteDetails[7];
            var strProductName = strDescription;
            var StkQuantityValue = QuantityValue;
            var Ptype = SpliteDetails[3];
            $('#hdfProductTypeDest').val(Ptype);
            document.getElementById("lblProductNameDest").innerHTML = strProductName;
            document.getElementById("txt_SalesAmountDest").innerHTML = QuantityValue;
            document.getElementById("txt_SalesUOMdest").innerHTML = strUOM;
            document.getElementById("txt_StockAmountDest").innerHTML = StkQuantityValue;
            document.getElementById("txt_StockUOMDest").innerHTML = strStkUOM;
            $('#hdfProductIDdest').val(strProductID);
            $('#hdfProductSerialIDDest').val(SrlNo);
            $('#hdfProductSerialIDDest').val(SrlNo);
            $('#hdnProductQuantityDest').val(QuantityValue);

            if (Ptype == "W") {
                div_WarehouseDest.style.display = 'block';
                div_BatchDist.style.display = 'none';
                div_SerialDest.style.display = 'none';
                div_QuantityDest.style.display = 'block';
                cCmbWarehouseDest.PerformCallback('BindWarehouse~' + SourceWarehouseID);
                cGrdWarehouseDest.PerformCallback('Display~' + SrlNo);
                $("#ADeleteDest").css("display", "block");
                SelectedWarehouseIDDest = "0";
                jAlert("No Batch or Serial is activated !");
            }
            else if (Ptype == "B") {
                div_WarehouseDest.style.display = 'none';
                div_BatchDist.style.display = 'block';
                div_SerialDest.style.display = 'none';
                div_QuantityDest.style.display = 'block';
                cCmbBatchDest.PerformCallback('BindBatch~' + "0");
                cGrdWarehouseDest.PerformCallback('Display~' + SrlNo);
                $("#ADeleteDest").css("display", "block");
                SelectedWarehouseIDDest = "0";
                jAlert("No Warehouse is activaed !");
            }
            else if (Ptype == "S") {
                div_WarehouseDest.style.display = 'none';
                div_BatchDist.style.display = 'none';
                div_SerialDest.style.display = 'block';
                div_QuantityDest.style.display = 'none';
                checkListBox.PerformCallback('BindSerial~' + "0" + '~' + "0");
                cGrdWarehouseDest.PerformCallback('Display~' + SrlNo);
                $("#ADeleteDest").css("display", "none");
                SelectedWarehouseIDDest = "0";
                jAlert("No Warehouse is activaed !");
            }
            else if (Ptype == "WB") {
                div_WarehouseDest.style.display = 'block';
                div_BatchDist.style.display = 'block';
                div_SerialDest.style.display = 'none';
                div_QuantityDest.style.display = 'block';
                cCmbWarehouseDest.PerformCallback('BindWarehouse~' + SourceWarehouseID);
                cGrdWarehouseDest.PerformCallback('Display~' + SrlNo);
                $("#ADeleteDest").css("display", "block");
                SelectedWarehouseIDDest = "0";
                //Rev Rajdip
                //ctxtStartDate.SetText("");
                //ctxtexpDate.SetText("");
                //cCmbBatchDest.Clear("");
                //End Rev Rajdip
                cPopup_WarehouseDestination.Show();
            }
            else if (Ptype == "WS") {
                div_WarehouseDest.style.display = 'block';
                div_BatchDist.style.display = 'none';
                div_SerialDest.style.display = 'block';
                div_QuantityDest.style.display = 'none';
                cCmbWarehouseDest.PerformCallback('BindWarehouse~' + SourceWarehouseID);
                cGrdWarehouseDest.PerformCallback('Display~' + SrlNo);
                $("#ADeleteDest").css("display", "none");
                SelectedWarehouseIDDest = "0";
                cPopup_WarehouseDestination.Show();
            }
            else if (Ptype == "WBS") {
                div_WarehouseDest.style.display = 'block';
                div_BatchDist.style.display = 'block';
                div_SerialDest.style.display = 'block';
                div_QuantityDest.style.display = 'none';
                cCmbWarehouseDest.PerformCallback('BindWarehouse~' + SourceWarehouseID);
                cGrdWarehouseDest.PerformCallback('Display~' + SrlNo);
                $("#ADeleteDest").css("display", "none");
                SelectedWarehouseIDDest = "0";
                cPopup_WarehouseDestination.Show();
            }
            else if (Ptype == "BS") {
                div_WarehouseDest.style.display = 'none';
                div_BatchDist.style.display = 'block';
                div_SerialDest.style.display = 'block';
                div_QuantityDest.style.display = 'none';
                cCmbBatchDest.PerformCallback('BindBatch~' + "0");
                cGrdWarehouseDest.PerformCallback('Display~' + SrlNo);
                $("#ADeleteDest").css("display", "none");
                SelectedWarehouseIDDest = "0";
                cPopup_WarehouseDestination.Show();
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
//Rev Rajdip
function CallbackPanelEnddestCall(s, e) {
    if (cCallbackPanelDest.cpEdit != null) {
        var strWarehouse = cCallbackPanelDest.cpEdit.split('~')[0];
        var strBatchID = cCallbackPanelDest.cpEdit.split('~')[1];
        var strSrlID = cCallbackPanelDest.cpEdit.split('~')[2];
        var strQuantity = cCallbackPanelDest.cpEdit.split('~')[3];
        var BatchNo = cCallbackPanelDest.cpEdit.split('~')[4];
        var MfgDate = cCallbackPanelDest.cpEdit.split('~')[5];
        var ExpiryDate = cCallbackPanelDest.cpEdit.split('~')[6];
        SelectWarehouse = strWarehouse;
        SelectBatch = strBatchID;
        SelectSerial = strSrlID;

        cCmbBatchDest.SetText(BatchNo);
        cCmbWarehouseDest.SetValue(strWarehouse);
        ctxtQuantityDest.SetText(strQuantity);
        if (ExpiryDate != "") {
            ctxtexpDate.SetDate(new Date(ExpiryDate));
        }
        else {
            ctxtexpDate.Clear();
        }
        if (MfgDate != "") {
            ctxtStartDate.SetDate(new Date(MfgDate));
        }
        else {
            ctxtStartDate.Clear();
        }

        cCmbWarehouse.PerformCallback('BindWarehouse~' + strWarehouse);
        cCmbBatch.PerformCallback('BindBatch~' + strWarehouse);
        checkListBox.PerformCallback('EditSerial~' + strWarehouse + '~' + strBatchID + '~' + strSrlID);

        cCmbWarehouse.SetValue(strWarehouse);
        ctxtQuantity.SetValue(strQuantity);
    }
}
//End Rev Rajdip
function fn_Edit(keyValue) {
    SelectedWarehouseID = keyValue;
    cCallbackPanel.PerformCallback('EditWarehouse~' + keyValue);
}
function fn_EditDist(keyValue) {
    SelectedWarehouseIDDest = keyValue;
    cCallbackPanelDest.PerformCallback('EditWarehouse~' + keyValue);
}

function fn_Deletecity(keyValue) {
    var WarehouseID = (cCmbWarehouse.GetValue() != null) ? cCmbWarehouse.GetValue() : "0";
    var BatchID = (cCmbBatch.GetValue() != null) ? cCmbBatch.GetValue() : "0";

    cGrdWarehouse.PerformCallback('Delete~' + keyValue);
    checkListBox.PerformCallback('BindSerial~' + WarehouseID + '~' + BatchID);
}
function fn_DeletecityDest(keyValue) {
    var WarehouseID = (cCmbWarehouseDest.GetValue() != null) ? cCmbWarehouseDest.GetValue() : "0";
    var BatchID = (cCmbBatchDest.GetValue() != null) ? cCmbBatchDest.GetValue() : "0";

    cGrdWarehouseDest.PerformCallback('Delete~' + keyValue);
    checkListBoxDest.PerformCallback('BindSerial~' + WarehouseID + '~' + BatchID);
}

function CmbWarehouse_ValueChange() {
    var WarehouseID = cCmbWarehouse.GetValue();
    var type = document.getElementById('hdfProductType').value;

    if (type == "WBS" || type == "WB") {
        //Rev 2.0
        var PostingDate = cdtTDate.GetValueString();
        /* cCmbBatch.PerformCallback('BindBatch~' + WarehouseID);*/
        cCmbBatch.PerformCallback('BindBatch~' + WarehouseID + '~' + PostingDate);
        //Rev 2.0 End
    }
    else if (type == "WS") {
        checkListBox.PerformCallback('BindSerial~' + WarehouseID + '~' + "0");
    }
}
function CmbWarehouseDest_ValueChange() {
    var WarehouseID = cCmbWarehouseDest.GetValue();
    var type = document.getElementById('hdfProductTypeDest').value;

    if (type == "WBS" || type == "WB") {
        cCmbBatchDest.PerformCallback('BindBatch~' + WarehouseID);
    }
    else if (type == "WS") {
        checkListBoxDest.PerformCallback('BindSerial~' + WarehouseID + '~' + "0");
    }
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
function CmbWarehouseDestEndCallback(s, e) {
    if (SelectWarehouseDest != "0") {
        cCmbWarehouseDest.SetValue(SelectWarehouseDest);
        SelectWarehouseDest = "0";
    }
    else {
        cCmbWarehouseDest.SetEnabled(true);
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
function CmbBatchDest_ValueChange() {
    var WarehouseID = cCmbWarehouseDest.GetValue();
    var BatchID = cCmbBatchDest.GetValue();
    var type = document.getElementById('hdfProductTypeDest').value;

    if (type == "WBS") {
        checkListBoxDest.PerformCallback('BindSerial~' + WarehouseID + '~' + BatchID);
    }
    else if (type == "BS") {
        checkListBoxDest.PerformCallback('BindSerial~' + "0" + '~' + BatchID);
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
function CmbBatchDestEndCall(s, e) {
    if (SelectBatchDest != "0") {
        cCmbBatchDest.SetValue(SelectBatchDest);
        SelectBatchDest = "0";
    }
    else {
        cCmbBatchDest.SetEnabled(true);
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
function AutoCalculateMandateOnChangeDest(element) {
    $("#spnCmbWarehouseDest").hide();
    $("#spnCmbBatchDest").hide();
    $("#spncheckComboBoxDest").hide();
    $("#spntxtQuantityDest").hide();

    if (document.getElementById("myCheckDest").checked == true) {
        divSingleComboDest.style.display = "block";
        divMultipleCombodest.style.display = "none";

        checkComboBoxDest.Focus();
    }
    else {
        divSingleComboDest.style.display = "none";
        divMultipleCombodest.style.display = "block";

        ctxtserialDest.Focus();
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
function OnListBoxSelectionChangedDest(listBox, args) {
    if (args.index == 0)
        args.isSelected ? listBox.SelectAll() : listBox.UnselectAll();
    UpdateSelectAllItemStateDest();
    UpdateTextDest();
    var selectedItems = checkListBoxDest.GetSelectedItems();
    var val = GetSelectedItemsText(selectedItems);
    var strWarehouse = cCmbWarehouseDest.GetValue();
    var strBatchID = cCmbBatchDest.GetValue();
    var ProducttId = $("#hdfProductIDdest").val();
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
function listBoxEndCallDest(s, e) {
    if (SelectSerialDest != "0") {
        var values = [SelectSerialDest];
        checkListBoxDest.SelectValues(values);
        UpdateSelectAllItemStateDest();
        UpdateTextDest();
        //checkListBox.SetValue(SelectWarehouse);
        SelectSerialDest = "0";
        cCmbBatchDest.SetEnabled(false);
        cCmbWarehouseDest.SetEnabled(false);
    }
}

function UpdateSelectAllItemState() {
    IsAllSelected() ? checkListBox.SelectIndices([0]) : checkListBox.UnselectIndices([0]);
}
function UpdateSelectAllItemStateDest() {
    IsAllSelectedDest() ? checkListBoxDest.SelectIndices([0]) : checkListBoxDest.UnselectIndices([0]);
}

function IsAllSelected() {
    var selectedDataItemCount = checkListBox.GetItemCount() - (checkListBox.GetItem(0).selected ? 0 : 1);
    return checkListBox.GetSelectedItems().length == selectedDataItemCount;
}
function IsAllSelectedDest() {
    var selectedDataItemCount = checkListBoxDest.GetItemCount() - (checkListBoxDest.GetItem(0).selected ? 0 : 1);
    return checkListBoxDest.GetSelectedItems().length == selectedDataItemCount;
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
function UpdateTextDest() {
    var selectedItems = checkListBoxDest.GetSelectedItems();
    selectedChkValueDest = GetSelectedItemsTextDest(selectedItems);

    var itemsCount = GetSelectedItemsCountDest(selectedItems);
    checkComboBoxDest.SetText(itemsCount + " Items");

    var val = GetSelectedItemsTextDest(selectedItems);
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
function SynchronizeListBoxValuesDest(dropDown, args) {
    checkListBoxDest.UnselectAll();

    var texts = selectedChkValueDest.split(textSeparator);

    var values = GetValuesByTextsDest(texts);
    checkListBoxDest.SelectValues(values);
    UpdateSelectAllItemStateDest();
    UpdateTextDest(); // for remove non-existing texts
}
function GetSelectedItemsText(items) {
    var texts = [];
    for (var i = 0; i < items.length; i++)
        if (items[i].index != 0)
            texts.push(items[i].text);
    return texts.join(textSeparator);
}
function GetSelectedItemsTextDest(items) {
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
function GetSelectedItemsCountDest(items) {
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
function GetValuesByTextsDest(texts) {
    var actualValues = [];
    var item;
    for (var i = 0; i < texts.length; i++) {
        item = checkListBoxDest.FindItemByText(texts[i]);
        if (item != null)
            actualValues.push(item.value);
    }
    return actualValues;
}
$(function () {
    $('[data-toggle="popover"]').popover();
})

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
function txtserialDestTextChanged() {
    checkListBoxDest.UnselectAll();
    var SerialNo = (ctxtserialDest.GetValue() != null) ? (ctxtserialDest.GetValue()) : "0";

    if (SerialNo != "0") {
        ctxtserialDest.SetValue("");
        var texts = [SerialNo];
        var values = GetValuesByTextsDest(texts);

        if (values.length > 0) {
            checkListBoxDest.SelectValues(values);
            UpdateSelectAllItemStateDest();
            UpdateTextDest(); // for remove non-existing texts
            SaveWarehouseDest();
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
    var AltQty=0;
    var AltUom = 0;
    var AltUomName = "";
    var IsUomActivate = $("#hdnShowUOMConversionInEntry.ClientID").val();
    if (IsUomActivate == 0) {
        AltQty = 0;
        AltUom = 0;
        AltUomName = "";
    }
    else {
        AltQty = (CtxtPacking.GetText() != null) ? CtxtPacking.GetText() : "0";
        AltUom = (ccmbPackingUom1.GetValue() != null) ? ccmbPackingUom1.GetValue() : "0";
        AltUomName = (ccmbPackingUom1.GetText() != null) ? ccmbPackingUom1.GetText() : "";
    }
    CtxtPacking.SetText("");


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
                //Rev 2.0
                var PostingDate = cdtTDate.GetValueString();
                /* cCmbBatch.PerformCallback('BindBatch~' + WarehouseID);*/
                cCmbBatch.PerformCallback('BindBatch~' + WarehouseID + '~' + PostingDate);
                //Rev 2.0 End
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
            //Rev 2.0
            var PostingDate = cdtTDate.GetValueString();
            /* cCmbBatch.PerformCallback('BindBatch~' + WarehouseID);*/
            cCmbBatch.PerformCallback('BindBatch~' + WarehouseID + '~' + PostingDate);
            //Rev 2.0 End
            checkListBox.PerformCallback('BindSerial~' + "" + '~' + "");
            ctxtQuantity.SetValue("0");
        }
        UpdateText();
        cGrdWarehouse.PerformCallback('SaveDisplay~' + WarehouseID + '~' + WarehouseName + '~' + BatchID + '~' + BatchName + '~' + SerialID + '~' + SerialName + '~' + Qty + '~' + SelectedWarehouseID + '~' + AltQty + '~' + AltUom + '~' + AltUomName);
        SelectedWarehouseID = "0";
    }
}
function SaveWarehouseDest() {
    var WarehouseID = (cCmbWarehouseDest.GetValue() != null) ? cCmbWarehouseDest.GetValue() : "0";
    var WarehouseName = cCmbWarehouseDest.GetText();
    var BatchID = (cCmbBatchDest.GetValue() != null) ? cCmbBatchDest.GetValue() : "0";
    var BatchName = cCmbBatchDest.GetText();
    var SerialID = "";
    var SerialName = "";
    var Qty = ctxtQuantityDest.GetValue();

    var items = checkListBoxDest.GetSelectedItems();
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
    $("#spnCmbWarehouseDest").hide();
    $("#spnCmbBatchDest").hide();
    $("#spncheckComboBoxDest").hide();
    $("#spntxtQuantityDest").hide();

    var Ptype = document.getElementById('hdfProductTypeDest').value;
    if ((Ptype == "W" && WarehouseID == "0") || (Ptype == "WB" && WarehouseID == "0") || (Ptype == "WS" && WarehouseID == "0") || (Ptype == "WBS" && WarehouseID == "0")) {
        $("#spnCmbWarehouseDest").show();
    }
    else if ((Ptype == "B" && BatchID == "0") || (Ptype == "WB" && BatchID == "0") || (Ptype == "WBS" && BatchID == "0") || (Ptype == "BS" && BatchID == "0")) {
        $("#spnCmbBatchDest").show();
    }
    else if ((Ptype == "W" && Qty == "0.0") || (Ptype == "B" && Qty == "0.0") || (Ptype == "WB" && Qty == "0.0")) {
        $("#spntxtQuantityDest").show();
    }
    else if ((Ptype == "S" && SerialID == "") || (Ptype == "WS" && SerialID == "") || (Ptype == "WBS" && SerialID == "") || (Ptype == "BS" && SerialID == "")) {
        $("#spncheckComboBoxDest").show();
    }
    else {
        if (document.getElementById("myCheckDest").checked == true && SelectedWarehouseIDDest == "0") {
            if (Ptype == "W" || Ptype == "WB" || Ptype == "B") {
                cCmbWarehouseDest.PerformCallback('BindWarehouse~' + WarehouseID);
                //cCmbBatchDest.PerformCallback('BindBatch~' + "");
                checkListBoxDest.PerformCallback('BindSerial~' + "" + '~' + "");
                ctxtQuantityDest.SetValue("0");
            }
            else {
                IsPostBack = "N";
                PBWarehouseID = WarehouseID;
                PBBatchID = BatchID;
            }
        }
        else {
            cCmbWarehouseDest.PerformCallback('BindWarehouse~' + WarehouseID);
            //cCmbBatchDest.PerformCallback('BindBatch~' + "");
            checkListBoxDest.PerformCallback('BindSerial~' + "" + '~' + "");
            ctxtQuantityDest.SetValue("0");

        }
        UpdateTextDest();
        cCmbBatchDest.SetText("");
        cGrdWarehouseDest.PerformCallback('SaveDisplay~' + WarehouseID + '~' + WarehouseName + '~' + BatchID + '~' + BatchName + '~' + SerialID + '~' + SerialName + '~' + Qty + '~' + SelectedWarehouseID);
        SelectedWarehouseIDDest = "0";
        ctxtStartDate.Clear();
        ctxtexpDate.Clear();
    }
}
function FinalWarehouse() {
    ccmbDestWarehouse.SetEnabled(false);
    ccmbSourceWarehouse.SetEnabled(false);
    cCmbScheme.SetEnabled(false);
    $("#hdnWHMandatory").val("Yes");
    cGrdWarehouse.PerformCallback('WarehouseFinal');
}
function FinalWarehouseDest() {
    ccmbDestWarehouse.SetEnabled(false);
    ccmbSourceWarehouse.SetEnabled(false);
    cCmbScheme.SetEnabled(false);
    $("#hdnWHMandatory").val("Yes");
    cGrdWarehouseDest.PerformCallback('WarehouseFinal');
}


function closeWarehouse(s, e) {
    e.cancel = false;
    cGrdWarehouse.PerformCallback('WarehouseDelete');
    $('#abpl').popover('hide');//Subhabrata
}
function closeWarehouseDest(s, e) {
    e.cancel = false;
    cGrdWarehouseDest.PerformCallback('WarehouseDelete');
    $('#abplDest').popover('hide');
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
function OnWarehouseEndCallbackDest(s, e) {
    var Ptype = document.getElementById('hdfProductTypeDest').value;

    if (cGrdWarehouseDest.cpIsSave == "Y") {
        cPopup_WarehouseDestination.Hide();
        gridDEstination.batchEditApi.StartEdit(Warehouseindex, 5);
    }
    else if (cGrdWarehouseDest.cpIsSave == "N") {
        jAlert('Quantity must be equal to Warehouse Quantity.');
    }
    else {
        if (document.getElementById("myCheckDest").checked == true) {
            if (IsPostBack == "N") {
                checkListBoxDest.PerformCallback('BindSerial~' + PBWarehouseID + '~' + PBBatchID);

                IsPostBack = "";
                PBWarehouseID = "";
                PBBatchID = "";
            }

            if (Ptype == "W" || Ptype == "WB") {
                cCmbWarehouseDest.Focus();
            }
            else if (Ptype == "B") {
                cCmbBatchDest.Focus();
            }
            else {
                ctxtserialDest.Focus();
            }
        }
        else {
            if (Ptype == "W" || Ptype == "WB" || Ptype == "WS" || Ptype == "WBS") {
                cCmbWarehouseDest.Focus();
            }
            else if (Ptype == "B" || Ptype == "BS") {
                cCmbBatchDest.Focus();
            }
            else if (Ptype == "S") {
                checkComboBoxDest.Focus();
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
function SuffuleSerialNumberGridDestination() {
    var TotRowNumber = gridDEstination.GetVisibleItemsOnPage();
    var SlnoCount = 1;
    for (var i = 0; i < 1000; i++) {
        if (gridDEstination.GetRow(i)) {
            if (gridDEstination.GetRow(i).style.display != "none") {
                gridDEstination.batchEditApi.StartEdit(i, 2);
                gridDEstination.GetEditor("SrlNo").SetText(SlnoCount);
                SlnoCount++;
            }
        }
    }

    for (i = -1; i > -1000; i--) {
        if (gridDEstination.GetRow(i)) {
            if (gridDEstination.GetRow(i).style.display != "none") {
                gridDEstination.batchEditApi.StartEdit(i, 2);
                gridDEstination.GetEditor("SrlNo").SetText(SlnoCount);
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
        // Rev Sanchita
        grid.batchEditApi.StartEdit(0, 2);
        // End of Rev Sanchita
        jAlert(msg);
    }
    else if (grid.cpSaveSuccessOrFail == "duplicateProduct") {
        grid.cpSaveSuccessOrFail = null;
        jAlert('Can not Add Duplicate Product in the Warehouse Wise Stock Journal.');
    }
    else if (grid.cpSaveSuccessOrFail == "AddLock") {
        grid.cpSaveSuccessOrFail = null;
       
        jAlert('DATA is Freezed between ' + grid.cpAddLockStatus + ' for Add.');
        grid.cpAddLockStatus = null;
    }
    else if (grid.cpSaveSuccessOrFail == "EntityMendatory") {
        var SrlNo = grid.cpEntityMendatory;
        grid.cpSaveSuccessOrFail = null;
        var msg = "Please Select Entity for SL No. " + SrlNo;
        jAlert(msg);
       
        grid.cpEntityMendatory = null;
    }

    else {
        gridDEstination.AddNewRow();
        gridDEstination.batchEditApi.StartEdit(0, 2)
        gridDEstination.batchEditApi.StartEdit(-1, 2);
        gridDEstination.UpdateEdit();
    }
    //else if (grid.cpErrorCode == "0") {
    //    jAlert(grid.cpadjustmentNumber, "Alert", function () { afterSave(); alertShow = false; });
    //} else {
    //    jAlert(grid.cpadjustmentNumber, "Alert", function () { grid.batchEditApi.StartEdit(-1, 2); grid.batchEditApi.StartEdit(0, 2); alertShow = false; });
    //}
}
function GridEndCallBackgridDEstination(s, e) {

    alertShow = true;

    if (gridDEstination.cpSaveSuccessOrFail == "checkWarehouse") {
        var SrlNo = gridDEstination.cpProductSrlIDCheck;
        gridDEstination.cpSaveSuccessOrFail = null;
        var msg = " Batch/Serial Details must be entered for SL No. " + SrlNo;
        //OnAddNewClick();
        jAlert(msg);
    }
    else if (gridDEstination.cpSaveSuccessOrFail == "checkWarehouseQty") {
        var SrlNo = gridDEstination.cpProductSrlIDCheck;
        gridDEstination.cpSaveSuccessOrFail = null;
        var msg = "Product Quantity must be equal to Warehouse Quantity for SL No. " + SrlNo;
        jAlert(msg);
    }
    else if (gridDEstination.cpSaveSuccessOrFail == "AddLock") {
        gridDEstination.cpSaveSuccessOrFail = null;

        jAlert('DATA is Freezed between ' + gridDEstination.cpAddLockStatus + ' for Add.');
        gridDEstination.cpAddLockStatus = null;
    }
    else if (gridDEstination.cpSaveSuccessOrFail == "duplicateProduct") {
        gridDEstination.cpSaveSuccessOrFail = null;
        jAlert('Can not Add Duplicate Product in the Warehouse Wise Stock Journal.');
    }
    else if (gridDEstination.cpErrorCode == "0") {
        jAlert(gridDEstination.cpadjustmentNumber, "Alert", function () { afterSave(); alertShow = false; });
    } else {
        jAlert(gridDEstination.cpadjustmentNumber, "Alert", function () { gridDEstination.batchEditApi.StartEdit(-1, 2); gridDEstination.batchEditApi.StartEdit(0, 2); alertShow = false; });
    }
}

function ValidateEntry() {
    var ReturnValue = true;

    if ((cCmbScheme.GetText().trim() == "" || cCmbScheme.GetText().trim() == "-Select-") && $('#hdAddEdit').val() == "Add") {
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

    if ($('#ddlBranch').val() == "" || $('#ddlBranch').val() == "-Select-" || $('#ddlBranch').val() == "0") {
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


    //Rev Subhra 01-08-2019
    //for (var i = 0; i < 1000; i++) {
    //    if (grid.GetRow(i)) {
    //        if (gridDEstination.GetRow(i)) {

    //            if (grid.GetRow(i).style.display != "none") {
    //                if (gridDEstination.GetRow(i).style.display != "none") {
    //                    grid.batchEditApi.StartEdit(i, 2);
    //                    gridDEstination.batchEditApi.StartEdit(i, 2);
    //                    if (grid.GetEditor("ProductID").GetText() == "" && gridDEstination.GetEditor("DestProductID").GetText() == "") {
    //                        //cLoadingPanelCRP.Hide();
    //                        jAlert("Please select at least one valid Product to proceed.");
    //                        return false;
    //                    }
    //                    if (grid.GetEditor("ProductID").GetText() != "")
    //                    {
    //                        if (grid.GetEditor("SourceWarehouseID").GetText() == "") {
    //                            //cLoadingPanelCRP.Hide();
    //                            jAlert("Please select Source Warehouse to proceed.");
    //                            return false;
    //                        }

    //                        if ((grid.GetEditor("TransferQuantity").GetText() == "0.0000" || grid.GetEditor("TransferQuantity").GetText() == "")) {
    //                            //cLoadingPanelCRP.Hide();
    //                            jAlert("Please enter a valid Quantity to proceed.");
    //                            return false;
    //                        }
    //                    }

    //                    if (gridDEstination.GetEditor("DestProductID").GetText() != "") {
    //                        if (gridDEstination.GetEditor("DestinationWarehouseID").GetText() == "") {
    //                            //cLoadingPanelCRP.Hide();
    //                            jAlert("Please select Destination Warehouse to proceed.");
    //                            return false;
    //                        }

    //                        if ((gridDEstination.GetEditor("DestQuantity").GetText() == "0.0000" || gridDEstination.GetEditor("DestQuantity").GetText() == "")) {
    //                            //cLoadingPanelCRP.Hide();
    //                            jAlert("Please enter a valid Quantity to proceed.");
    //                            return false;
    //                        }
    //                    }


    //                }
    //            }
    //        }
    //    }
    //}

    for (var i = 0; i < grid.GetVisibleRowsOnPage() + 10; i++) {
        if (grid.GetRow(i)) {
            if (grid.GetRow(i).style.display != "none") {
                grid.batchEditApi.StartEdit(i, 2);
                if (grid.GetEditor("ProductID").GetText() == "") {
                    jAlert("Please select valid Product from Stock Out Grid to proceed.");
                    return false;
                }
                if (grid.GetEditor("ProductID").GetText() != "") {
                    if (grid.GetEditor("SourceWarehouseID").GetText() == "") {
                        jAlert("Please select valid Warehouse from Stock Out Grid to proceed.");
                        return false;
                    }

                    if ((grid.GetEditor("TransferQuantity").GetText() == "0.0000" || grid.GetEditor("TransferQuantity").GetText() == "")) {
                        jAlert("Please select valid Quantity from Stock Out Grid to proceed.");
                        return false;
                    }
                }
                if ($('#hdnStockNeg').val() == 'B') {
                    jAlert('Product is going negative can not proceed');
                    grid.GetEditor("TransferQuantity").SetValue("0.0000");
                    $("#txtQuantity").val("0.0000");
                    $("#txtPacking").val("0.0000");
                    return false;
                }

            }
        }
    }

    //for (var i = 0; i < gridDEstination.GetVisibleRowsOnPage() + 10; i++) {
    //    if (gridDEstination.GetRow(i)) {
    //        if (gridDEstination.GetRow(i).style.display != "none") {
    //            gridDEstination.batchEditApi.StartEdit(i, 2);
    //            if (gridDEstination.GetEditor("DestProductID").GetText() == "") {
    //                jAlert("Please select valid Product from Stock In Grid to proceed.");
    //                return false;
    //            }
    //            if (gridDEstination.GetEditor("DestProductID").GetText() != "") {
    //                if (gridDEstination.GetEditor("DestinationWarehouseID").GetText() == "") {
    //                    jAlert("Please select valid Warehouse from Stock In Grid to proceed.");
    //                    return false;
    //                }

    //                if ((gridDEstination.GetEditor("DestQuantity").GetText() == "0.0000" || gridDEstination.GetEditor("DestQuantity").GetText() == "")) {
    //                    jAlert("Please select valid Quantity from Stock In Grid to proceed.");
    //                    return false;
    //                }
    //            }
    //            if ($('#hdnDestStockNeg').val() == 'B') {
    //                jAlert('Product is going negative can not proceed');
    //                gridDEstination.GetEditor("DestQuantity").SetValue("0.0000");
    //                $("#txtQuantity").val("0.0000");
    //                $("#txtPacking").val("0.0000");
    //                return false;
    //            }

    //        }

    //    }
    //}

    for (i = -1; i > -(grid.GetVisibleRowsOnPage() + 10) ; i--) {
        if (grid.GetRow(i)) {
            if (grid.GetRow(i).style.display != "none") {
                grid.batchEditApi.StartEdit(i, 2);
                if (grid.GetEditor("ProductID").GetText() == "") {
                    jAlert("Please select valid Product from Stock Out Grid to proceed.");
                    return false;
                }
                if (grid.GetEditor("ProductID").GetText() != "") {
                    if (grid.GetEditor("SourceWarehouseID").GetText() == "") {
                        jAlert("Please select valid Warehouse from Stock Out Grid to proceed.");
                        return false;
                    }

                    if ((grid.GetEditor("TransferQuantity").GetText() == "0.0000" || grid.GetEditor("TransferQuantity").GetText() == "")) {
                        jAlert("Please select valid Quantity from Stock Out Grid to proceed.");
                        return false;
                    }
                }

                if ($('#hdnStockNeg').val() == 'B') {
                    jAlert('Product is going negative can not proceed');
                    grid.GetEditor("TransferQuantity").SetValue("0.0000");
                    $("#txtQuantity").val("0.0000");
                    $("#txtPacking").val("0.0000");
                    return false;
                }

            }
        }
    }

    //for (i = -1; i > -(gridDEstination.GetVisibleRowsOnPage() + 10) ; i--) {
    //    if (gridDEstination.GetRow(i)) {
    //        if (gridDEstination.GetRow(i).style.display != "none") {
    //            gridDEstination.batchEditApi.StartEdit(i, 2);
    //            if (gridDEstination.GetEditor("DestProductID").GetText() == "") {
    //                jAlert("Please select valid Product from Stock In Grid to proceed.");
    //                return false;
    //            }

    //            if (gridDEstination.GetEditor("DestProductID").GetText() != "") {
    //                if (gridDEstination.GetEditor("DestinationWarehouseID").GetText() == "") {
    //                    jAlert("Please select valid Warehouse from Stock In Grid to proceed.");
    //                    return false;
    //                }

    //                if ((gridDEstination.GetEditor("DestQuantity").GetText() == "0.0000" || gridDEstination.GetEditor("DestQuantity").GetText() == "")) {
    //                    jAlert("Please select valid Quantity from Stock In Grid to proceed.");
    //                    return false;
    //                }
    //            }

    //            if ($('#hdnDestStockNeg').val() == 'B') {
    //                jAlert('Product is going negative can not proceed');
    //                gridDEstination.GetEditor("DestQuantity").SetValue("0.0000");
    //                $("#txtQuantity").val("0.0000");
    //                $("#txtPacking").val("0.0000");
    //                return false;
    //            }


    //        }
    //    }
    //}

    //End of Rev Subhra 01-08-2019
    return ReturnValue;
}
//Rev Subhra 29-07-2019
//function RateTextChange() {
//    grid.batchEditApi.StartEdit(globalRowIndex);
//    var TransferQuantity = parseFloat(grid.GetEditor("TransferQuantity").GetValue());
//    var Rate = parseFloat(grid.GetEditor("Rate").GetValue());
//    grid.GetEditor("Value").SetValue(TransferQuantity * Rate);
//    grid.batchEditApi.EndEdit();
//    setTimeout(function () {
//        grid.batchEditApi.StartEdit(globalRowIndex, 16);
//    }, 200);
//}

function RateTextChange(s, e) {
    var QuantityValue = (grid.GetEditor('TransferQuantity').GetValue() != null) ? grid.GetEditor('TransferQuantity').GetValue() : "0";
    var Rate = (grid.GetEditor('Rate').GetValue() != null) ? grid.GetEditor('Rate').GetValue() : "0";
    var amount = (QuantityValue * Rate);
    grid.GetEditor('Value').SetValue(amount);
    var uniqueIndex = globalRowIndex;
    // Mantis Issue 24629
    //SetTotalValue(uniqueIndex, 11);
    SetTotalValue(uniqueIndex, 13);
    // End of Mantis Issue 24629
    //setTimeout(function () {
    //    grid.batchEditApi.StartEdit(globalRowIndex, 16);
    //}, 200);
}

function DestRateTextChange(s, e) {
    var DQuantityValue = (gridDEstination.GetEditor('DestQuantity').GetValue() != null) ? gridDEstination.GetEditor('DestQuantity').GetValue() : "0";
    var DRate = (gridDEstination.GetEditor('DestRate').GetValue() != null) ? gridDEstination.GetEditor('DestRate').GetValue() : "0";
    var Damount = DecimalRoundoff((DecimalRoundoff(DQuantityValue, 2) * DecimalRoundoff(DRate, 2)), 2);
    gridDEstination.GetEditor('DestValue').SetValue(Damount);

    var uniqueIndex = globalRowIndexDestination;
    SetDestTotalValue(uniqueIndex, 12);
}
//End of Rev 

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
    debugger;
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

        // Rev Sanchita
       // spLostFocus(null, null);
        // End of Rev Sanchita

    }

    if (cgrid_MultiUOM.cpAllDetails == "EditData") {
        // $('#UOMQuantity').val(cgrid_MultiUOM.cpBaseQty);
          var Quan = (cgrid_MultiUOM.cpBaseQty).toFixed(4);
        $('#UOMQuantity').val(Quan);
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
   

    //var PackingQtyAlt = Productdetails.split("||@||")[5];
   // var PackingQty = Productdetails.split("||@||")[6];
    //var PackingSaleUOM = Productdetails.split("||@||")[8];  // Alternate UOM selected from Product Master (tbl_master_product_packingDetails.packing_saleUOM
    //debugger;

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
    debugger;
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
    //Rev Bapi
    //if (srlNo != "" && qnty != "0.0000" && UomId != "" && UomName != "" && AltUomId != "" && ProductID != "" && AltUomId != null && AltUomName != ""
    //       && BaseRate != "0.0000" && AltRate != "0.0000") {
    // Rev Sanchita
         //if (srlNo != "" && UomId != "" && UomName != "" && AltUomId != "" && ProductID != "" && AltUomId != null && AltUomName != ""
    //&& AltRate != "0.0000") {
    if (srlNo != "" && UomId != "" && UomName != "" && AltUomId != "" && ProductID != "" && AltUomId != null && AltUomName != "" && AltQnty != "0.0000") {
        if ((qnty != "0" && UpdateRow == 'True') || (qnty == "0" && UpdateRow == 'False') || (qnty != "0" && UpdateRow == 'False')) {
            // End of Rev Sanchita
             // End of Rev Bapi
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
             // Rev Bapi
         }

         else {
             return;
         }
             // End of Rev Bapi
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
               //Rev Sanchita
                //cAltUOMQuantity.SetValue(calcQuantity);
                //End Rev Sanchita
            }

        }
    });
}

function Edit_MultiUom(keyValue, SrlNo) {
    debugger;
    cbtnMUltiUOM.SetText("Update");
    cgrid_MultiUOM.PerformCallback('EditData~' + keyValue + '~' + SrlNo);



}
function Delete_MultiUom(keyValue, SrlNo) {
    debugger;

    cgrid_MultiUOM.PerformCallback('MultiUomDelete~' + keyValue + '~' + SrlNo);

}

// End of Mantis Issue 24428








function SaveButtonClick() {

    var ProjectCode = clookup_Project.GetText();
    if ($("#hdnProjectSelectInEntryModule").val() == "1" && $("#hdnProjectMandatory").val() == "1" && ProjectCode == "") {
        jAlert("Please Select Project.");
        return false;
    }
    var strEntity = ctxtEntity.GetText();
    if ($("#hdnEntityMandatory").val() == "1")
    {
        if (strEntity == "" || strEntity == "-Select-") {
            jAlert("Please Select Entity.");
            return false;
        }
    }
  
    var ddlType = $("#ddlType").val();
    if ($("#hdnAutoReceiptWWSI").val() == "1") {
        if (ddlType == "0" ) {
            jAlert("Please Select Type.");
            return false;
        }
    }

    saveNewOrExit = 'N';

    $('#HiddenSaveButton').val("N");
    if (ValidateEntry()) {
        if (!grid.InCallback()) {
            if (issavePacking == 1) {
                if (aarrSWH.length > 0) {
                    $.ajax({
                        type: "POST",
                        url: "WarehousewiseStockJournalAdd.aspx/SetSessionPacking",
                        data: "{'list':'" + JSON.stringify(aarrSWH) + "'}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        async: false,
                        success: function (msg) {
                            // grid.UpdateEdit();
                        }
                    });

                }
                if (aarrDWH.length > 0) {
                    $.ajax({
                        type: "POST",
                        url: "WarehousewiseStockJournalAdd.aspx/SetSessionPackingDWH",
                        data: "{'list':'" + JSON.stringify(aarrDWH) + "'}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        async: false,
                        success: function (msg) {
                            // grid.UpdateEdit();
                        }
                    });

                }

                grid.UpdateEdit();

            }
            else {
                if (aarrSWH.length > 0) {
                    $.ajax({
                        type: "POST",
                        url: "WarehousewiseStockTransferAdd.aspx/SetSessionPacking",
                        data: "{'list':'" + JSON.stringify(aarrSWH) + "'}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        async: false,
                        success: function (msg) {
                            //grid.UpdateEdit();
                        }
                    });

                }
                if (aarrDWH.length > 0) {
                    $.ajax({
                        type: "POST",
                        url: "WarehousewiseStockJournalAdd.aspx/SetSessionPackingDWH",
                        data: "{'list':'" + JSON.stringify(aarrDWH) + "'}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        async: false,
                        success: function (msg) {
                            // grid.UpdateEdit();
                        }
                    });
                }
                grid.UpdateEdit();
                //else {
                //    grid.UpdateEdit();
                //}
            }
        }
    }
}
function SaveExitButtonClick() {


    var ProjectCode = clookup_Project.GetText();
    if ($("#hdnProjectSelectInEntryModule").val() == "1" && $("#hdnProjectMandatory").val() == "1" && ProjectCode == "") {
        jAlert("Please Select Project.");
        return false;
    }

    var strEntity = ctxtEntity.GetText();
    if ($("#hdnEntityMandatory").val() == "1"){
        if (strEntity == "" || strEntity == "-Select-") {
            jAlert("Please Select Entity.");
            return false;
        }
    }

    var ddlType = $("#ddlType").val();
    if ($("#hdnAutoReceiptWWSI").val() == "1") {
        if (ddlType == "0") {
            jAlert("Please Select Type.");
            return false;
        }
    }

    $('#HiddenSaveButton').val("E");
    saveNewOrExit = 'E';
    if (ValidateEntry()) {
        if (!grid.InCallback()) {
            if (issavePacking == 1) {
                if (aarrSWH.length > 0) {

                    $.ajax({
                        type: "POST",
                        url: "WarehousewiseStockJournalAdd.aspx/SetSessionPacking",
                        data: "{'list':'" + JSON.stringify(aarrSWH) + "'}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (msg) {
                            // grid.UpdateEdit();
                        }
                    });

                }
                if (aarrDWH.length > 0) {
                    $.ajax({
                        type: "POST",
                        url: "WarehousewiseStockJournalAdd.aspx/SetSessionPackingDWH",
                        data: "{'list':'" + JSON.stringify(aarrDWH) + "'}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        async: false,
                        success: function (msg) {
                            // grid.UpdateEdit();
                        }
                    });

                }
                grid.batchEditApi.StartEdit(0, 2)
                grid.batchEditApi.StartEdit(-1, 2);
                grid.UpdateEdit();

            }
            else {
                if (aarr.length > 0) {

                    $.ajax({
                        type: "POST",
                        url: "WarehousewiseStockJournalAdd.aspx/SetSessionPacking",
                        data: "{'list':'" + JSON.stringify(aarr) + "'}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (msg) {
                            // grid.UpdateEdit();
                        }
                    });

                }
                if (aarrDWH.length > 0) {
                    $.ajax({
                        type: "POST",
                        url: "WarehousewiseStockJournalAdd.aspx/SetSessionPackingDWH",
                        data: "{'list':'" + JSON.stringify(aarrDWH) + "'}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        async: false,
                        success: function (msg) {
                            // grid.UpdateEdit();
                        }
                    });

                }
                grid.batchEditApi.StartEdit(0, 2)
                grid.batchEditApi.StartEdit(-1, 2);
                grid.UpdateEdit();
                //else {
                //    grid.UpdateEdit();
                //}
            }
        }
    }
}
function GetVisibleIndex(s, e) {
    globalRowIndex = e.visibleIndex;
}
function GetVisibleIndexDest(s, e) {
    globalRowIndexDestination = e.visibleIndex;
}

function afterSave() {
    var stockJournal = gridDEstination.cpadjustmentId;
    if (saveNewOrExit == 'N') {
        if ($("#hdAddEdit").val() == "Add") {
            if (stockJournal != '') {
                if (document.getElementById('hdnWSTAutoPrint').value == 1) {
                    reportName = "WHStockOUT-Default~D";
                    window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + reportName + '&modulename=WarehouseStockOUT&id=' + stockJournal + '&PrintOption=1', '_blank');
                    //window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + reportName + '&modulename=WarehouseStockOUT&id=' + stockJournal + '&PrintOption=2', '_blank');
                    //window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + reportName + '&modulename=WarehouseStockOUT&id=' + stockJournal + '&PrintOption=3', '_blank');
                }
            }
        }

        window.location.href = 'WarehousewiseStockJournalAddOUT.aspx?Key=Add';
        $('#hdAddEdit').val("Add");
    }
    else {
        window.location.href = 'WarehousewiseStockJournalListOUT.aspx';
        if ($("#hdAddEdit").val() == "Add") {
            if (stockJournal != '') {
                if (document.getElementById('hdnWSTAutoPrint').value == 1) {
                    reportName = "WHStockOUT-Default~D";
                    window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + reportName + '&modulename=WarehouseStockOUT&id=' + stockJournal + '&PrintOption=1', '_blank');
                    // window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + reportName + '&modulename=WarehouseStockOUT&id=' + stockJournal + '&PrintOption=2', '_blank');
                    // window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + reportName + '&modulename=WarehouseStockOUT&id=' + stockJournal + '&PrintOption=3', '_blank');
                }
            }
        }
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
        OtherDetails.BranchID = $('#ddlBranch').val();
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

    clookup_Project.GetText("");
    //if ($("#hdnProjectSelectInEntryModule").val() == "1") {
    //    clookup_Project.gridView.Refresh();
    //}
    $("#ddlBranchTo").focus();
    //var OtherDetails = {}
    //OtherDetails.BranchID = splitData[2];
    //$.ajax({
    //    type: "POST",
    //    url: "WarehousewiseStockJournalAddOUT.aspx/GetTechnician",
    //    data: JSON.stringify(OtherDetails),
    //    contentType: "application/json; charset=utf-8",
    //    dataType: "json",
    //    success: function (msg) {
    //        var returnObject = msg.d;
    //        if (returnObject) {
    //            SetDataSourceOnComboBox(cccmTechnician, returnObject);
    //        }
    //    }
    //});

    //var OtherDetails = {}
    //OtherDetails.BranchID = splitData[2];
    //$.ajax({
    //    type: "POST",
    //    url: "WarehousewiseStockJournalAddOUT.aspx/GetEntity",
    //    data: JSON.stringify(OtherDetails),
    //    contentType: "application/json; charset=utf-8",
    //    dataType: "json",
    //    async: false,
    //    success: function (msg) {
    //        var returnObject = msg.d;
    //        if (returnObject) {
    //            SetDataSourceOnComboBox(cccmEntity, returnObject);
    //        }
    //    }
    //});
   
}
function SetDataSourceOnComboBox(ControlObject, Source) {
    ControlObject.ClearItems();
    for (var count = 0; count < Source.length; count++) {
        ControlObject.AddItem(Source[count].Name, Source[count].Id);
    }
    ControlObject.SetSelectedIndex(0);
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
        HeaderCaption.push("Replaceable");
        if ($("#txtProdSearch").val() != '') {
            callonServer("Services/Master.asmx/GetProductForWHStockInOut", OtherDetails, "ProductTable", HeaderCaption, "ProdIndex", "SetProduct");
        }
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


function DestProductButnClick(s, e) {
    if (e.buttonIndex == 0) {
        setTimeout(function () { $("#txtDestProdSearch").focus(); }, 500);
        $('#txtDestProdSearch').val('');
        $('#DestProductModel').modal('show');
    }
}
function DestProductKeyDown(s, e) {
    console.log(e.htmlEvent.key);
    if (e.htmlEvent.key == "Enter" || e.code == "NumpadEnter") {
        s.OnButtonClick(0);
    }
    //if (e.htmlEvent.key == "Tab") {
    //    s.OnButtonClick(0);
    //}
}
function Destprodkeydown(e) {

    //if ($.trim($("#txtProdSearch").val()) == "" || $.trim($("#txtProdSearch").val()) == null) {

    //    return false;
    //}

    var OtherDetails = {}
    OtherDetails.SearchKey = $("#txtDestProdSearch").val();

    if (e.code == "Enter" || e.code == "NumpadEnter") {
        var HeaderCaption = [];
        HeaderCaption.push("Product Code");
        HeaderCaption.push("Product Description");
        HeaderCaption.push("Inventory");
        HeaderCaption.push("HSN/SAC");
        HeaderCaption.push("Class");
        HeaderCaption.push("Brand");
        HeaderCaption.push("Replaceable");
        if ($("#txtDestProdSearch").val() != '') {
            callonServer("Services/Master.asmx/GetProductForWHStockInOut", OtherDetails, "DestProductTable", HeaderCaption, "DestProdIndex", "SetDestProduct");
        }
    }
    else if (e.code == "ArrowDown") {
        if ($("input[DestProdIndex=0]"))
            $("input[DestProdIndex=0]").focus();
    }
    else if (e.code == "Escape") {
        //grid.batchEditApi.EndEdit();
        gridDEstination.batchEditApi.StartEdit(globalRowIndexDestination, 2);
    }
}




function ValueSelected(e, indexName) {
    if (e.code == "Enter" || e.code == "NumpadEnter") {
        var Id = e.target.parentElement.parentElement.cells[0].innerText;
        var code = e.target.parentElement.parentElement.cells[1].children[0].value;
        var name = e.target.parentElement.parentElement.cells[1].children[0].value;
        //var name = "";
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
            else if (indexName == "DestProdIndex") {
                SetDestProduct(Id, code, name);
            }
            else if (indexName == "customerIndex") {
                SetCustomer(Id, code);
            }
            else if (indexName == "EntityIndex") {
                SetEntity(Id, code, name);
            }
            else if (indexName == "TechnicianIndex") {
                SetTechnician(Id, code);
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
            else if (indexName == "DestProdIndex")
                $('#txtDestProdSearch').focus();
            else if (indexName == "customerIndex")
                $('#txtCustSearch').focus();
            else if (indexName == "EntityIndex")
                $('#txtEntitySearch').focus();
            else if (indexName == "TechnicianIndex")
                $('#txtTechnicianSearch').focus();
        }
    }
}
function SetProduct(Id, code, name) {
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


    grid.GetEditor("SourceWarehouseID").SetText("");
    grid.GetEditor("SourceWarehouse").SetText("");
    grid.GetEditor("AvlStkSourceWH").SetText("0.0000");
    //grid.GetEditor("DestinationWarehouseID").SetText("");
    //grid.GetEditor("DestinationWarehouse").SetText("");
    //grid.GetEditor("AvlStkDestWH").SetText("0.0000");
    grid.GetEditor("TransferQuantity").SetValue("0.0000");
    grid.GetEditor("SaleUOM").SetValue(SaleUOMname);
    grid.GetEditor("Rate").SetValue("0.00");
    grid.GetEditor("Value").SetValue("0.0000");

    grid.batchEditApi.EndEdit();
    setTimeout(function () {
        grid.batchEditApi.StartEdit(globalRowIndex, 5);
    }, 200);



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
function ProductPriceCalculate() {
    //if ((grid.GetEditor('Rate').GetValue() == null || grid.GetEditor('Rate').GetValue() == 0)) {
    var _price = 0;
    var _Qty = grid.GetEditor('TransferQuantity').GetValue();
    var _Amount = grid.GetEditor('Value').GetValue();
    _price = (_Amount / _Qty);
    grid.GetEditor('Rate').SetValue(_price);

    var uniqueIndex = globalRowIndex;
    SetTotalValue(uniqueIndex, 13);
    //}
}
function AmountTextChange(s, e) {
    ProductPriceCalculate();
}
function DestProductPriceCalculate() {
    //if ((gridDEstination.GetEditor('DestRate').GetValue() == null || gridDEstination.GetEditor('DestRate').GetValue() == 0)) {
    var _price = 0;
    var _Qty = gridDEstination.GetEditor('DestQuantity').GetValue();
    var _Amount = gridDEstination.GetEditor('DestValue').GetValue();
    _price = (_Amount / _Qty);
    gridDEstination.GetEditor('DestRate').SetValue(_price);
    //}
}
function DestAmountTextChange(s, e) {
    DestProductPriceCalculate();
    var uniqueIndex = globalRowIndexDestination;
    SetDestTotalValue(uniqueIndex, 13);
}
function SetDestProduct(Id, code, name) {
    var ProductIDS = Id;
    var splitData = ProductIDS.split('||@||');
    $('#DestProductModel').modal('hide');
    var strProductID = splitData[0];
    var LookUpData = Id;
    var ProductDescription = splitData[1];
    var ProductCode = code;
    if (!ProductCode) {
        LookUpData = null;
    }
    var SaleUOMname = splitData[2];

    gridDEstination.batchEditApi.StartEdit(globalRowIndexDestination);
    gridDEstination.GetEditor("DestProductID").SetText(LookUpData);
    gridDEstination.GetEditor("ProductNameDest").SetText(ProductCode);
    gridDEstination.GetEditor("DestDiscription").SetText(ProductDescription);


    //grid.GetEditor("SourceWarehouseID").SetText("");
    //grid.GetEditor("SourceWarehouse").SetText("");
    //grid.GetEditor("AvlStkSourceWH").SetText("0.0000");
    gridDEstination.GetEditor("DestinationWarehouseID").SetText("");
    gridDEstination.GetEditor("DestinationWarehouse").SetText("");
    gridDEstination.GetEditor("AvlStkDestWH").SetText("0.0000");
    gridDEstination.GetEditor("DestQuantity").SetValue("0.0000");
    gridDEstination.GetEditor("DestUOM").SetValue(SaleUOMname);
    gridDEstination.GetEditor("DestRate").SetValue("0.00");
    gridDEstination.GetEditor("DestValue").SetValue("0.0000");

    gridDEstination.batchEditApi.EndEdit();
    setTimeout(function () {
        gridDEstination.batchEditApi.StartEdit(globalRowIndexDestination, 5);
    }, 200);


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

    $('#SourceWarehouseModel').modal('hide');
    grid.batchEditApi.StartEdit(globalRowIndex);
    grid.GetEditor("SourceWarehouseID").SetText(SourceWarehouseID);
    grid.GetEditor("SourceWarehouse").SetText(name);
    grid.GetEditor("TransferQuantity").SetValue("0.0000");
    grid.GetEditor("Rate").SetValue("0.00");
    grid.GetEditor("Value").SetValue("0.0000");
    var strProductID = grid.GetEditor("ProductID").GetValue();
    var splitData = strProductID.split('||@||');
    var BranchId = $('#ddlBranch').val();
    var stockdate = cdtTDate.GetDate();    
    var fromBranch = $('#ddlBranch').val();
    var ToBranch = $('#ddlBranchTo').val()
    
    $.ajax({
        type: "POST",
        url: "WarehousewiseStockJournalAddOUT.aspx/GetStockInHandForWarehouseWiseStockJournal",       
        data: JSON.stringify({ ProductId: splitData[0], WarehouseID: SourceWarehouseID, BranchId: BranchId, fromBranch: fromBranch, ToBranch: ToBranch, Fromdate: cdtTDate.date.format('yyyy-MM-dd'), stkdate: stockdate }),

        //End Rev Rajdip
        //End of Rev 
        contentType: "application/json; charset=utf-8",
        async: false,
        dataType: "json",
        success: function (msg) {
            var data = msg.d;
            var strStockID = data.split("~")[0].toString();
            var strStockUOM = data.split("~")[1].toString();
           
            var Rate = data.split("~")[2].toString();
            //var DailyQuantity = data.split("~")[3].toString();
            //var DailAltyQuantity = data.split("~")[4].toString();
            //$("#lblDailyStkQty").text(DailyQuantity);
            //$("#lblDailyAltStkQty").text(DailAltyQuantity);
            //End of Rev

            grid.GetEditor("AvlStkSourceWH").SetText(strStockID);

            grid.batchEditApi.EndEdit();
            setTimeout(function () {
                grid.batchEditApi.StartEdit(globalRowIndex, 6);
            }, 200);

        }
    });

    var wHType = 'SWH';
    //ShowUOMPOpup(wHType);

    if ($('#hdnShowUOMConversionInEntry').val() == 0) {
        grid.batchEditApi.EndEdit();
        setTimeout(function () {
            grid.batchEditApi.StartEdit(globalRowIndex, 6);
        }, 500);
    }
    else {
        ShowUOMPOpup(wHType);
    }



}

function DestinationWarehouseButnClick(s, e) {

    var strProductID = gridDEstination.GetEditor("DestProductID").GetValue();

    if (strProductID == '' && strProductID == null) {
        jAlert("Please Select a Product", "Alert", function () {
            gridDEstination.batchEditApi.StartEdit(globalRowIndexDestination);
            gridDEstination.batchEditApi.EndEdit();
            setTimeout(function () {
                gridDEstination.batchEditApi.StartEdit(globalRowIndexDestination, 2);
            }, 200);
        });
    }
    else {
        if (e.buttonIndex == 0) {
            setTimeout(function () { $("#txtDestinationWarehouseSearch").focus(); }, 500);
            $('#txtDestinationWarehouseSearch').val('');
            $('#DestinationWarehouseModel').modal('show');
        }
    }
}

function DestinationWarehouseKeyDown(s, e) {

    var strProductID = gridDEstination.GetEditor("DestProductID").GetValue();

    if (strProductID == '' && strProductID == null) {
        jAlert("Please Select a Product", "Alert", function () {
            gridDEstination.batchEditApi.StartEdit(globalRowIndexDestination);
            gridDEstination.batchEditApi.EndEdit();
            setTimeout(function () {
                gridDEstination.batchEditApi.StartEdit(globalRowIndexDestination, 2);
            }, 200);
        });
    }
    else {
        if (e.htmlEvent.key == "Enter" || e.code == "NumpadEnter") {
            s.OnButtonClick(0);
        }
    }
}

function DestinationWarehousekeydown(e) {

    var OtherDetails = {}
    OtherDetails.SearchKey = $("#txtDestinationWarehouseSearch").val();
    OtherDetails.BranchID = $('#ddlBranchTo').val();

    if (e.code == "Enter" || e.code == "NumpadEnter") {
        var HeaderCaption = [];
        HeaderCaption.push("Destination Warehouse");
        if ($("#txtDestinationWarehouseSearch").val() != '') {
            //callonServer("Services/Master.asmx/GetProductForWHStockTransfer", OtherDetails, "ProductTable", HeaderCaption, "ProdIndex", "SetProduct");
            //callonServer("Services/Master.asmx/GetWarehouseByFilterStockTransfer", OtherDetails, "DestinationWarehouseTable", HeaderCaption, "DWHIndex", "SetDestinationWarehouse");
            callonServer("Services/Master.asmx/GetWarehouseByBranchStockTransfer", OtherDetails, "DestinationWarehouseTable", HeaderCaption, "DWHIndex", "SetDestinationWarehouse");


        }
    }
    else if (e.code == "ArrowDown") {
        if ($("input[DWHIndex=0]"))
            $("input[DWHIndex=0]").focus();
    }
    else if (e.code == "Escape") {
        gridDEstination.batchEditApi.StartEdit(globalRowIndexDestination, 5);
    }
}

function SetDestinationWarehouse(Id, name) {

    var DestinationWarehouseID = Id;
    $('#DestinationWarehouseModel').modal('hide');
    gridDEstination.batchEditApi.StartEdit(globalRowIndexDestination);
    gridDEstination.GetEditor("DestinationWarehouseID").SetText(DestinationWarehouseID);
    gridDEstination.GetEditor("DestinationWarehouse").SetText(name);
    gridDEstination.GetEditor("DestQuantity").SetValue("0.0000");
    gridDEstination.GetEditor("DestRate").SetValue("0.00");
    gridDEstination.GetEditor("DestValue").SetValue("0.0000");
    $("#ddlBranchTo").prop("disabled", true);
    var strProductID = gridDEstination.GetEditor("DestProductID").GetValue();
    var splitData = strProductID.split('||@||');
    var BranchId = $('#ddlBranchTo').val();
    var stockdate = cdtTDate.GetDate();   
    var fromBranch = $('#ddlBranch').val();
    var ToBranch = $('#ddlBranchTo').val()
  
    $.ajax({
        type: "POST",      
        url: "WarehousewiseStockJournalAddOUT.aspx/GetStockInHandForWarehouseWiseStockJournal",        
        data: JSON.stringify({ ProductId: splitData[0], WarehouseID: DestinationWarehouseID, BranchId: BranchId, fromBranch: fromBranch, ToBranch: ToBranch, Fromdate: cdtTDate.date.format('yyyy-MM-dd'), stkdate: stockdate }),

        //End Rev Rajdip
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (msg) {
            var data = msg.d;
            var strStockID = data.split("~")[0].toString();
            var strStockUOM = data.split("~")[1].toString();
           
            var Rate = data.split("~")[2].toString();
            //var DailyQuantity = data.split("~")[3].toString();
            //var DailAltyQuantity = data.split("~")[4].toString();
            //$("#lblDailyStkQty").text(DailyQuantity);
            //$("#lblDailyAltStkQty").text(DailAltyQuantity);
           
            gridDEstination.GetEditor("AvlStkDestWH").SetText(strStockID);
            gridDEstination.batchEditApi.EndEdit();
            
        }
    });   

    if ($('#hdnShowUOMConversionInEntry').val() == 0) {
        gridDEstination.batchEditApi.EndEdit();
        // gridDEstination.batchEditApi.StartEdit(globalRowIndexDestination, 8);
        setTimeout(function () {
            gridDEstination.batchEditApi.StartEdit(globalRowIndexDestination, 8);
        }, 200);
    }
    else {
        var wHType = 'DWH';
        ShowUOMPOpup(wHType);
    }
}