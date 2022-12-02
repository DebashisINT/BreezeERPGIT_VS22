var globalRowIndex;
function OnEndCallback(s, e) {

    if (grid.cpSuccess == "Success") {
        // cCmbWarehouse.SetValue('');
        // cCmbWarehouse.SetText('');
        //ctxtProdName.SetText('');
        // $("#hdncWiseProductId").val('');
        // $("#hdnClassId").val('');
        // $("#hdnBranndId").val('');
        $("#hdfIsDelete").val('');
        $('#hdfIsDelete').val('');
        LoadingPanel.Hide();
        grid.cpSuccess = null;
        jAlert("Physical stock updated successfully.");
        PerformCallToGridBind();
        //cGrdQuotation.Refresh();
    }
           
    else if(grid.cpSuccess == "RecordNotFound")
    {
        $("#hdfIsDelete").val('');
        $('#hdfIsDelete').val('');
        LoadingPanel.Hide();
        grid.cpSuccess = null;
        jAlert("No records found.");
        PerformCallToGridBind();
        //cGrdQuotation.Refresh();
    }

    else if(grid.cpwidthLoad=="widthLoad")
    {
                 
        grid.cpwidthLoad = null;
        grid.cpwidthLoad="";
    }
    else if (grid.cpAfterWareFocus == "AfterWareFocus")
    {
        grid.batchEditApi.StartEdit(-1, 2);
    }


    //if (grid.GetVisibleRowsOnPage() > 0) {
    //    $("#btn_SaveRecords").show();
    //}
    //else {
    //    $("#btn_SaveRecords").hide();
    //}
}

function OnCommitEndCallback(s,e)
{
    if (cGrdQuotation.cpSavecheck == "Savecheck")
    {
        jAlert("Data Commit Successfully");
        cGrdQuotation.cpSavecheck = null;
        cGrdQuotation.cpSavecheck = "";
                
    }
    //if(cGrdQuotation.GetVisibleRowsOnPage()<1)
    //{
    //    jAlert("No records available.");
    //}

    //if (grid.GetVisibleRowsOnPage() > 0) {
    //    $("#btn_SaveRecords").show();
    //}
    //else {
    //    $("#btn_SaveRecords").hide();
    //}


    //cGrdQuotation.Refresh();
}

function CrossBtnClose()
{
    var val = "0";
    $.ajax({
        type: "POST",
        url: "phycicalStoctVerification.aspx/GetProductCheck",
        data: JSON.stringify(),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (msg) {
            val = msg.d;
        }

    });
    if(val==1)
    {

        jConfirm('Exit Without Save.', 'Confirmation Dialog', function (r) {

            if (r == true) {
                     
                var URL = '/OMS/management/ProjectMainPage.aspx';
                window.location.href = URL;
            }
        });
    }
    else
    {
        var URL = '/OMS/management/ProjectMainPage.aspx';
        window.location.href = URL;
    }



}
        

function OnCustomButtonClick(s, e) {
          
    if (e.buttonID == 'CustomDelete') {
        var SrlNo = grid.batchEditApi.GetCellValue(e.visibleIndex, 'SrlNo');
        grid.batchEditApi.EndEdit();





        $('#hdnDeleteSrlNo').val(SrlNo);
        var noofvisiblerows = grid.GetVisibleRowsOnPage();



        if (noofvisiblerows != "1") {
            grid.DeleteRow(e.visibleIndex);



            $('#hdfIsDelete').val('D');
            grid.UpdateEdit();
            grid.PerformCallback('Display');

            $('#hdnPageStatus').val('delete');
            //grid.batchEditApi.StartEdit(-1, 2);
            //grid.batchEditApi.StartEdit(0, 2);

        }
    }




}

function GetVisibleIndex(s, e) {
    globalRowIndex = e.visibleIndex;
    // StockQuantityGotFocus();
    RowClickProductDetails();

}

function gridFocusedRowChanged(s, e) {
    globalRowIndex = e.visibleIndex;


}



function ClassButnClick(s, e) {
    $('#ClassModel').modal('show');
}
function Class_KeyDown(s, e) {
    if (e.htmlEvent.key == "Enter") {
        $('#ClassModel').modal('show');
    }
}


function Classkeydown(e) {
    var OtherDetails = {}
    if ($.trim($("#txtClassSearch").val()) == "" || $.trim($("#txtClassSearch").val()) == null) {
        return false;
    }
    OtherDetails.SearchKey = $("#txtClassSearch").val();
    if (e.code == "Enter" || e.code == "NumpadEnter") {
        var HeaderCaption = [];
        HeaderCaption.push("Class Name");

        if ($("#txtClassSearch").val() != "") {
            callonServerM("phycicalStoctVerification.aspx/GetClass", OtherDetails, "ClassTable", HeaderCaption, "dPropertyIndex", "SetSelectedValues", "ClassSource");
        }
    }
    else if (e.code == "ArrowDown") {
        if ($("input[dPropertyIndex=0]"))
            $("input[dPropertyIndex=0]").focus();
    }
}

function SetfocusOnseach(indexName) {
    if (indexName == "dPropertyIndex")
        $('#txtClassSearch').focus();
    else
        $('#txtClassSearch').focus();
}
var beforeWraehouseChangeTxt;
var beforeWraehouseChangeVal
function CmbWarehouse_GotFocus(s, e) {
    beforeWraehouseChangeTxt = cCmbWarehouse.GetText();
    beforeWraehouseChangeVal = cCmbWarehouse.GetValue();

}





function CmbWarehouse_ValueChange(s,e) {


    if (grid.GetVisibleRowsOnPage() > 0) {
        jConfirm('If you change Warehouse and qty entered are not saved with Save option, then it will make Qty entered to ZERO(0) if click on "Yes", if you wish to keep the current Qty for changed warehouse click on "No".', 'Confirmation Dialog', function (r) {

            if (r == true) {
                //alert("hello");
                //setTimeout(function () {
                //    s.SetValue(beforeWraehouseChangeVal);
                //    s.SetText(beforeWraehouseChangeTxt);
                //    //cCmbWarehouse.SetValue(beforeWraehouseChangeVal);
                //    //cCmbWarehouse.SetText(beforeWraehouseChangeTxt);
                //}, 600);
                grid.PerformCallback("QuantityZero");
                             
            }
            //else {
            //    //setTimeout(function () {
            //    //    cCmbWarehouse.SetValue(beforeWraehouseChange);
            //    //}, 200)

            //    grid.PerformCallback("QuantityZero");
            //}

        });
    }
}

$(document).ready(function () {

    // cGrdQuotation.Refresh();

    $('#BrandModel').on('shown.bs.modal', function () {
        $('#txtBrandSearch').focus();
    })
})
var BrandArr = new Array();
$(document).ready(function () {

    //cCmbWarehouse.PerformCallback('BindWarehouse');

    var BrandObj = new Object();
    BrandObj.Name = "BrandSource";
    BrandObj.ArraySource = BrandArr;
    arrMultiPopup.push(BrandObj);
})


function BrandButnClick(s, e) {
    $('#BrandModel').modal('show');
}
function Brand_KeyDown(s, e) {
    if (e.htmlEvent.key == "Enter") {
        $('#BrandModel').modal('show');
    }
}
function Brandkeydown(e) {
    var OtherDetails = {}
    if ($.trim($("#txtBrandSearch").val()) == "" || $.trim($("#txtBrandSearch").val()) == null) {
        return false;
    }
    OtherDetails.SearchKey = $("#txtBrandSearch").val();
    if (e.code == "Enter" || e.code == "NumpadEnter") {
        var HeaderCaption = [];
        HeaderCaption.push("Name");
        if ($("#txtBrandSearch").val() != "") {
            callonServerM("phycicalStoctVerification.aspx/GetBrand", OtherDetails, "BrandTable", HeaderCaption, "dPropertyIndex", "SetSelectedValues", "BrandSource");
        }
    }
    else if (e.code == "ArrowDown") {
        if ($("input[dPropertyIndex=0]"))
            $("input[dPropertyIndex=0]").focus();
    }
}
function SetfocusOnseach(indexName) {
    if (indexName == "dPropertyIndex")
        $('#txtBrandSearch').focus();
    else
        $('#txtBrandSearch').focus();
}


$(document).ready(function () {
    $('#ProdModel').on('shown.bs.modal', function () {
        $('#txtProdSearch').focus();
    });

    $('#ClassModel').on('shown.bs.modal', function () {
        $('#txtClassSearch').focus();
    });
});

var ClassArr = new Array();
$(document).ready(function () {
    var ClassObj = new Object();
    ClassObj.Name = "ClassSource";
    ClassObj.ArraySource = ClassArr;
    arrMultiPopup.push(ClassObj);
});

var ProdArr = new Array();
$(document).ready(function () {
    var ProdObj = new Object();
    ProdObj.Name = "ProductSource";
    ProdObj.ArraySource = ProdArr;
    arrMultiPopup.push(ProdObj);
})


function ProductButnClick(s, e) {
    $('#ProdModel').modal('show');
}

function Product_KeyDown(s, e) {
    if (e.htmlEvent.key == "Enter") {
        $('#ProdModel').modal('show');
    }
}


function Productkeydown(e) {
    var OtherDetails = {}
    if ($.trim($("#txtProdSearch").val()) == "" || $.trim($("#txtProdSearch").val()) == null) {
        return false;
    }
    OtherDetails.SearchKey = $("#txtProdSearch").val();

    if (e.code == "Enter" || e.code == "NumpadEnter") {
        var HeaderCaption = [];

        HeaderCaption.push("Code");
        HeaderCaption.push("Name");
        HeaderCaption.push("Class");
        HeaderCaption.push("Brand")

        if ($("#txtProdSearch").val() != "") {
            callonServerM("phycicalStoctVerification.aspx/GetProduct", OtherDetails, "ProductTable", HeaderCaption, "dPropertyIndex", "SetSelectedValues", "ProductSource");
        }
    }
    else if (e.code == "ArrowDown") {
        if ($("input[dPropertyIndex=0]"))
            $("input[dPropertyIndex=0]").focus();
    }
}


function GetCheckBoxValue(value) {
    //var value = s.GetChecked();
    if (value == true) {
        $("#hdncWiseProductId").val("AllProducts");
        ctxtProdName.SetEnabled(false);
        ctxtClass.SetEnabled(false);
        ctxtBrandName.SetEnabled(false);
        ctxtProdName.SetText("");
        ctxtClass.SetText("");
        ctxtBrandName.SetText("");
        $("#hdnClassId").val("");
        $("#hdnBranndId").val("");

    } else {
        $("#hdncWiseProductId").val("");
        $("#hdnClassId").val("");
        $("#hdnBranndId").val("");
        ctxtProdName.SetEnabled(true);
        ctxtClass.SetEnabled(true);
        ctxtBrandName.SetEnabled(true);
    }
}



function SetSelectedValues(Id, Name, ArrName) {

    if (ArrName == 'ProductSource') {
        var key = Id;
        var Product_id = 0;
        var loopStock_id = 0;

        var adddat = key.split(',');
        var sproducts_id;
        var stock_id;
        if (key != null && key != '') {

            for (var p = 0; p < adddat.length; p++) {
                var Prodid = adddat[p].split("||@||");
                Product_id = Prodid[0];
                loopStock_id = Prodid[1];
                if (p == 0) {
                    sproducts_id = Product_id
                    stock_id = loopStock_id;
                }
                else {
                    sproducts_id += "," + Product_id
                    stock_id += "," + loopStock_id
                }
            }


            $('#ProdModel').modal('hide');
            ctxtProdName.SetText(Name);
            GetObjectID('hdnCalcommitProductId').value = sproducts_id;
            GetObjectID('hdncWiseProductId').value = sproducts_id;
        }
        else {
            ctxtProdName.SetText('');
            GetObjectID('hdncWiseProductId').value = '';
        }
    }

    else if (ArrName == 'ClassSource') {
        var key = Id;
        if (key != null && key != '') {
            $('#ClassModel').modal('hide');
            ctxtClass.SetText(Name);
            GetObjectID('hdnClassId').value = key;

        }
        else {
            ctxtClass.SetText('');
            GetObjectID('hdnClassId').value = '';
        }
    }
    else if (ArrName == 'BrandSource') {
        var key = Id;
        if (key != null && key != '') {
            $('#BrandModel').modal('hide');
            ctxtBrandName.SetText(Name);
            GetObjectID('hdnBranndId').value = key;
        }
        else {
            ctxtBrandName.SetText('');
            GetObjectID('hdnBranndId').value = '';
        }
    }

}


function SetfocusOnseach(indexName) {
    if (indexName == "dPropertyIndex")
        $('#txtProdSearch').focus();
    else
        $('#txtProdSearch').focus();
}

//$(function () {
//    cProductPanel.PerformCallback('BindProductGrid');
//});

function selectAll_Product() {
    gridproductLookup.gridView.SelectRows();
}
function unselectAll_Product() {
    gridproductLookup.gridView.UnselectRows();
}
function CloseGridProductLookup() {
    gridproductLookup.ConfirmCurrentSelection();
    gridproductLookup.HideDropDown();
    gridproductLookup.Focus();
}


function PerformCallToGridBind() {
                 
    if (ctxtProdName.GetText() == "" && ctxtClass.GetText() == "" && ctxtBrandName.GetText() == "" && cChkAllProduct.GetChecked() == false)
    {
        jAlert("Please select atleast one type.");
        return false;
    }

    if (cCmbWarehouse.GetText() == "")
    {
        jAlert("Please select Warehouse.");
        cCmbWarehouse.SetFocus();
        return false;
    }

    var AsOnDate = tstartdate.GetDate();
    if (AsOnDate == null || AsOnDate == "") {
        LoadingPanel.Hide();
        jAlert("Please select Date.");
        tstartdate.SetFocus();
        flag = false;
        return false;
    }


    $("#btn_SaveRecords").show();
    var ProductId = $("#hdncWiseProductId").val();
    var ClassId = $("#hdnClassId").val();
    var BrandId = $("#hdnBranndId").val();
    //tstartdate.SetEnabled(false);
    // cCmbWarehouse.PerformCallback('BindWarehouse');
    if (ProductId != "AllProducts") {
        // grid.Refresh();
        grid.PerformCallback('BindGrid' + '~' + ProductId + '~' + ClassId + '~' + BrandId);
    }
    else {
        // grid.Refresh();
        grid.PerformCallback('BindGrid' + '~' + ProductId + '~' + ClassId + '~' + BrandId);
    }
                  

}

//e.preventDefault();
//}
//// stopping event bubbling up the DOM tree..
//e.stopPropagation();

function CheckDecimal(s, e)

{
                
    // grid.batchEditApi.StartEdit(globalRowIndex, 5);
    var keyCode = e.htmlEvent.keyCode;
    var flagValue = e.htmlEvent.returnValue;
    //var keyCode = (e.which) ? e.which : e.keyCode;
    if ((keyCode >= 48 && keyCode <= 57) || (keyCode == 8) || (keyCode == 45) || (keyCode == 43))
        return true;
                     
    else if (keyCode == 46) {
        var curVal = document.activeElement.value;
        if (curVal != null && curVal.trim().indexOf('.') == -1)
            return true;
                         
        else
            //flagValue=false;
            ASPxClientUtils.PreventEvent(e.htmlEvent);
    }
    else
        // flagValue = false;
        ASPxClientUtils.PreventEvent(e.htmlEvent);
    // e.preventDefault();


}

function RowClickProductDetails(s,e)
{

    var otherdet = {};
    var ProductdetailsID = grid.GetEditor('ProductID').GetValue();
    var splitDet = ProductdetailsID.split("||@||");
    var ProductDisId = splitDet[0];
    var WarhouseId = cCmbWarehouse.GetValue();
    var OnDate = tstartdate.GetDate();
    otherdet.ProductDisId = ProductDisId;

    otherdet.WarhouseId = WarhouseId;
    otherdet.OnDate = OnDate;
    //otherdet.UserId = UserId;
    if (ProductdetailsID != "System.String[]") {

        $.ajax({
            type: "POST",
            url: "phycicalStoctVerification.aspx/GetProductQuantity",
            data: JSON.stringify(otherdet),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (msg) {
                if (msg.d.length > 0) {
                    if (msg.d[0].CountRow == "RowExists") {
                        clblProductQty.SetText("0.0000");
                        clblAltQuantity.SetText("0.0000");
                        jAlert("Quantity entered for this product for physical stock taking date:" + msg.d[0].AsOnDate);
                    }
                    else if (msg.d[0].CountRow == "RowExistsButNotReconcile") {
                        clblProductQty.SetText("0.0000");
                        clblAltQuantity.SetText("0.0000");
                        jAlert("This Product stock already committed but not reconciled on date:" + msg.d[0].AsOnDate);
                    }
                    else if (msg.d[0].CountRow == "RowExistsReconcileButNotStkAdj") {
                        clblProductQty.SetText("0.0000");
                        clblAltQuantity.SetText("0.0000");
                        jAlert("This Product stock has already been reconciled but not adjusted on date:" + msg.d[0].AsOnDate);
                    }
                    else {
                        clblProductQty.SetText((msg.d[0].StockSheet_StockQty).toFixed(4));
                        clblAltQuantity.SetText((msg.d[0].StockSheet_AltQty).toFixed(4));
                        $('#div_Resetbtn').attr('style', 'display:block');
                    }
                }
                else {
                    $('#div_Resetbtn').attr('style', 'display:none');
                    clblProductQty.SetText("0.0000");
                    clblAltQuantity.SetText("0.0000");
                }
            }

        });
    }

    // grid.batchEditApi.EndEdit();

}

function StockQuantityGotFocus(s, e) {
    //grid.batchEditApi.StartEdit(globalRowIndex);

    var otherdet = {};
    var ProductdetailsID = grid.GetEditor('ProductID').GetValue();
    var splitDet = ProductdetailsID.split("||@||");
    var ProductDisId = splitDet[0];
    var WarhouseId = cCmbWarehouse.GetValue();
    var OnDate = tstartdate.GetDate();
    otherdet.ProductDisId = ProductDisId;

    otherdet.WarhouseId = WarhouseId;
    otherdet.OnDate = OnDate;
    //otherdet.UserId = UserId;
    if (ProductdetailsID != "System.String[]") {

        $.ajax({
            type: "POST",
            url: "phycicalStoctVerification.aspx/GetProductQuantity",
            data: JSON.stringify(otherdet),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (msg) {
                if (msg.d.length > 0) {
                    if (msg.d[0].CountRow == "RowExists") {
                        clblProductQty.SetText("0.0000");
                        clblAltQuantity.SetText("0.0000");
                        jAlert("Quantity entered for this product for physical stock taking date:" + msg.d[0].AsOnDate);
                    }
                    else if (msg.d[0].CountRow == "RowExistsButNotReconcile")
                    {
                        clblProductQty.SetText("0.0000");
                        clblAltQuantity.SetText("0.0000");
                        jAlert("This Product stock already committed but not reconciled on date:" + msg.d[0].AsOnDate);
                    }
                    else if (msg.d[0].CountRow == "RowExistsReconcileButNotStkAdj") {
                        clblProductQty.SetText("0.0000");
                        clblAltQuantity.SetText("0.0000");
                        jAlert("This Product stock has already been reconciled but not adjusted on date:" + msg.d[0].AsOnDate);
                    }
                    else {
                        clblProductQty.SetText((msg.d[0].StockSheet_StockQty).toFixed(4));
                        clblAltQuantity.SetText((msg.d[0].StockSheet_AltQty).toFixed(4));
                        $('#div_Resetbtn').attr('style', 'display:block');
                    }
                }
                else {
                    $('#div_Resetbtn').attr('style', 'display:none');
                    clblProductQty.SetText("0.0000");
                    clblAltQuantity.SetText("0.0000");
                }
            }

        });
    }

    // grid.batchEditApi.EndEdit();

}

function PopulateMultiUomStockQuantity(s,e)
{
    grid.batchEditApi.StartEdit(globalRowIndex, 6);

    var otherdet = {};
    var ProductdetailsID = grid.GetEditor('ProductID').GetValue();
    var splitDet = ProductdetailsID.split("||@||");
    var Quantity = (grid.GetEditor('AltUnitQnty').GetValue() != null) ? grid.GetEditor('AltUnitQnty').GetValue() : "0";
    otherdet.Quantity = Quantity;
    var UomId = splitDet[2];
    otherdet.UomId = UomId;

    var ProductID = splitDet[0];
    otherdet.ProductID = ProductID;
    var AltUomId = splitDet[3];
    otherdet.AltUomId = AltUomId;
    if (ProductdetailsID != "System.String[]") {
        $.ajax({
            type: "POST",
            url: "phycicalStoctVerification.aspx/GetPackingQuantity",
            data: JSON.stringify(otherdet),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (msg) {

                if (msg.d.length != 0) {
                    var packingQuantity = msg.d[0].packing_quantity;
                    var sProduct_quantity = msg.d[0].sProduct_quantity;

                    if (msg.d[0].isOverideConvertion==true)
                    {
                        var uomfactor = 0
                        if (sProduct_quantity != 0 && packingQuantity != 0) {
                            uomfactor = parseFloat(packingQuantity / sProduct_quantity).toFixed(4);
                            $('#hdnAltUomFactor').val(parseFloat(packingQuantity / sProduct_quantity));
                        }
                        else {
                            $('#hdnAltUomFactor').val(0);
                        }

                        var uomfac_Qty_to_stock = $('#hdnAltUomFactor').val();
                        var Qty = (grid.GetEditor('AltUnitQnty').GetValue() != null) ? grid.GetEditor('AltUnitQnty').GetValue() : "0";
                        var calcQuantity = parseFloat(Qty / uomfac_Qty_to_stock).toFixed(4);


                        grid.batchEditApi.StartEdit(globalRowIndex);

                        if (grid.GetEditor('AltUnitQnty').GetValue() != null) {
                            var d = grid.GetEditor('AltUnitQnty').GetValue();
                            d = parseFloat(d).toFixed(4);
                            grid.GetEditor('AltUnitQnty').SetValue(d);
                            grid.GetEditor('StockUnitQnty').SetValue(calcQuantity);
                            grid.UpdateEdit();
                        }
                    }
                }
                else {
                    var packingQuantity = 0;
                    var sProduct_quantity = 0;
                }
                            
            }
        });
    }
}


function PopulateMultiUomAltQuantity(s, e) {


    grid.batchEditApi.StartEdit(globalRowIndex, 6);

    var otherdet = {};
    var ProductdetailsID = grid.GetEditor('ProductID').GetValue();
    var splitDet = ProductdetailsID.split("||@||");
    var Quantity = (grid.GetEditor('StockUnitQnty').GetValue() != null) ? grid.GetEditor('StockUnitQnty').GetValue() : "0";
    otherdet.Quantity = Quantity;
    var UomId = splitDet[2];
    otherdet.UomId = UomId;

    var ProductID = splitDet[0];
    otherdet.ProductID = ProductID;
    var AltUomId = splitDet[3];
    otherdet.AltUomId = AltUomId;
    if (ProductdetailsID != "System.String[]") {
        $.ajax({
            type: "POST",
            url: "phycicalStoctVerification.aspx/GetPackingQuantity",
            data: JSON.stringify(otherdet),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (msg) {
                if (msg.d.length != 0) {
                    var packingQuantity = msg.d[0].packing_quantity;
                    var sProduct_quantity = msg.d[0].sProduct_quantity;
                                 
                    // if (msg.d[0].isOverideConvertion == true) {
                    var uomfactor = 0
                    if (sProduct_quantity != 0 && packingQuantity != 0) {
                        uomfactor = parseFloat(packingQuantity / sProduct_quantity).toFixed(4);
                        $('#hddnuomFactor').val(parseFloat(packingQuantity / sProduct_quantity));
                    }
                    else {
                        $('#hddnuomFactor').val(0);
                    }
                    var uomfac_Qty_to_stock = $('#hddnuomFactor').val();
                    var Qty = (grid.GetEditor('StockUnitQnty').GetValue() != null) ? grid.GetEditor('StockUnitQnty').GetValue() : "0";
                    var calcQuantity = parseFloat(Qty * uomfac_Qty_to_stock).toFixed(4);
                    grid.batchEditApi.StartEdit(globalRowIndex);
                    if (grid.GetEditor('StockUnitQnty').GetValue() != null) {
                        var d = grid.GetEditor('StockUnitQnty').GetValue();
                        d = parseFloat(d).toFixed(4);
                        grid.GetEditor('StockUnitQnty').SetValue(d);
                        if (msg.d[0].isOverideConvertion == true) 
                        {
                            grid.GetEditor('AltUnitQnty').SetValue(calcQuantity);
                        }
                        else
                        {
                            var AltQuantity = grid.GetEditor('AltUnitQnty').GetValue();
                            if (AltQuantity == null || AltQuantity == '') {
                                grid.GetEditor('AltUnitQnty').SetValue(calcQuantity);
                            }
                        }                                       
                                         
                        grid.UpdateEdit();
                    }
                    //}

                }
                else {
                    var packingQuantity = 0;
                    var sProduct_quantity = 0;
                }
                            
            }
        });
    }
}

var GetCommitRowcountVal;
function getCommitRow()
{
    $.ajax({
        type: "POST",
        url: "phycicalStoctVerification.aspx/GetCommitRowcount",
        data: JSON.stringify(),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (msg) {
            GetCommitRowcountVal = msg.d;
                         
        }

    });
}

function CalCommit_ButtonClick()
{
    getCommitRow();
                 
                 
    if (GetCommitRowcountVal > 0) {
        cGrdQuotation.Refresh();
        cPopup_CalculateCommit.Show();
        $("#ASPxButton1").show();
    }
    else
    {
        jAlert("No records available.");
        $("#ASPxButton1").hide();
    }
                 
}

function CalCommit_CloseClick() {

    cPopup_CalculateCommit.Hide();

}
              
function DifferentDateProductEntryCheck()
{                 
    var otherdet = {};              
                
    var WarhouseId = cCmbWarehouse.GetValue();
    var OnDate = tstartdate.GetDate();             

    otherdet.WarhouseId = WarhouseId;
    otherdet.OnDate = OnDate;
    $.ajax({
        type: "POST",
        url: "phycicalStoctVerification.aspx/DifferentDateProductEntryCheck",
        data: JSON.stringify(otherdet),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (msg) {
            val = msg.d;
            $("#hdnProductEntryCheck").val(val);
        }

    });
}

function ResetProduct_ButtonClick() {
    var otherdet = {};
    grid.batchEditApi.StartEdit(globalRowIndex);

    var ProductdetailsID = grid.GetEditor('ProductID').GetValue();
    var splitDet = ProductdetailsID.split("||@||");
    var ProductID = splitDet[0];

    otherdet.ProductID = ProductID;
    $.ajax({
        type: "POST",
        url: "phycicalStoctVerification.aspx/DeleteStockSheetData",
        data: JSON.stringify(otherdet),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (msg) {
            val = msg.d;
            if(val>0)
            {
                jAlert("Physical Stock count to its initial state successfully.");
                clblProductQty.SetText("0.0000");
                clblAltQuantity.SetText("0.0000");
            }
        }

    });


}
function Save_ButtonClick() {
    LoadingPanel.Show();
                  
    flag = true;
    grid.batchEditApi.EndEdit();

    var warehouseId = cCmbWarehouse.GetValue();
    if (warehouseId == "" || warehouseId == null) {
        LoadingPanel.Hide();
        jAlert("Plaese select Warehouse.");
        flag = false;
        return false;
    }
    var AsOnDate = tstartdate.GetDate();
    if (AsOnDate == null || AsOnDate == "") {
        LoadingPanel.Hide();
        jAlert("Plaese select As On Date.");
        flag = false;
        return false;
    }
                 
    if (grid.GetVisibleRowsOnPage() == 0 || grid.GetVisibleRowsOnPage() < 0)
    {
        LoadingPanel.Hide();
        flag = false;
        return false;
    }
                
    DifferentDateProductEntryCheck();
    if ($("#hdnProductEntryCheck").val() == -11)
    {
        LoadingPanel.Hide();
        //jAlert("Plaese select Date.");
        flag = false;
        return false;
    }


    //DeSelectAll('ProductSource');
    //DeSelectAll('ClassSource');
    // DeSelectAll('BrandSource');
    clblProductQty.SetText("0.0000");
    clblAltQuantity.SetText("0.0000");
    $('#hdfIsDelete').val('I');

    //var WarhouseId = cCmbWarehouse.GetValue();
    //var OnDate = tstartdate.GetDate();

    //otherdet.WarhouseId = WarhouseId;
    //otherdet.OnDate = OnDate;

    //$.ajax({
    //    type: "POST",
    //    url: "phycicalStoctVerification.aspx/DifferentDateProductEntryCheck",
    //    data: JSON.stringify(otherdet),
    //    contentType: "application/json; charset=utf-8",
    //    dataType: "json",
    //    async: false,
    //    success: function (msg) {
    //        val = msg.d;
    //        $("#hdnProductEntryCheck").val(val);
    //    }

    //});

    grid.AddNewRow();
    grid.UpdateEdit();

}

    $(document).ready(function () {
        //$("#btn_SaveRecords").hide();
        setTimeout(function () {
            if ($('body').hasClass('mini-navbar')) {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 120;
                grid.SetWidth(cntWidth);
            } else {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 240;
                grid.SetWidth(cntWidth);
            }
        }, 1000);
        $('.navbar-minimalize').click(function () {
            if ($('body').hasClass('mini-navbar')) {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 240;
                grid.SetWidth(cntWidth);
            } else {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 120;
                grid.SetWidth(cntWidth);
            }

        });
    });
