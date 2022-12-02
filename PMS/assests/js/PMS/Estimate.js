
    var ProdIndexAddl = 0;
    var ResIndexAddl = 0;
    var globalrowindex2 = 0;
    var globalrowindex = 0;
    var gridtxtbox = '1';
    var slno = 0;
    var firsttime = 0;
    var DetailsID = 0;
    var ProductionID = 0;
    var GEstimateNo = "";
    var Message = "";
    var savemode = "";
    var hasmsg = 0;
    var rowtime = 0;
    var rowtime2 = 0;
    var PrdProdNameIndex = 0, PrdProdDescIndex = 0, PrdProdQtyIndex = 0, PrdProdUOMIndex = 0, PrdWarehouseIndex = 0, PrdPriceIndex = 0, PrdDiscountIndex = 0, PrdAmountIndex = 0;
    var PrdTaxTypeIndex = 0, PrdChargesIndex = 0, PrdNetAmtIndex = 0, PrdBudgPricIndex = 0, PrdAddlDescIndex = 0, PrdRemarksIndex = 0, PrdProdIdIndex = 0, PrdProdWarehouseIDIndex = 0;
    var PrdUpdateEditIndex = 0, PrdTagDetlsIDIndex = 0, PrdTagProdnIDIndex = 0, PrdTaxTypeIDIndex = 0, PrdProdHSNIndex = 0, PrdAddDescIndex = 0, PrdSrlIndex;

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
            //  AddNewRowWithSl();
            //AddNewRowGridResources();
            //gridEstimateProductEntryList.CancelEdit();
            $("#ddlSchema").focus();



        }, 200);


        var columnslres = gridEstimateResourcesList.GetColumnByField("SlNO");
        ResSrlIndex = columnslres.index;

        var columnsl = gridEstimateProductEntryList.GetColumnByField("SlNO");
        PrdSrlIndex = columnsl.index;

        var column1 = gridEstimateProductEntryList.GetColumnByField("ProductName");
        PrdProdNameIndex = column1.index;

        var column2 = gridEstimateProductEntryList.GetColumnByField("ProductDescription");
        PrdProdDescIndex = column2.index;

        var column3 = gridEstimateProductEntryList.GetColumnByField("ProductQty");
        PrdProdQtyIndex = column3.index;

        var column4 = gridEstimateProductEntryList.GetColumnByField("ProductUOM");
        PrdProdUOMIndex = column4.index;

        var column5 = gridEstimateProductEntryList.GetColumnByField("Warehouse");
        PrdWarehouseIndex = column5.index;

        var column6 = gridEstimateProductEntryList.GetColumnByField("Price");
        PrdPriceIndex = column6.index;

        var column7 = gridEstimateProductEntryList.GetColumnByField("Discount");
        PrdDiscountIndex = column7.index;

        var column8 = gridEstimateProductEntryList.GetColumnByField("Amount");
        PrdAmountIndex = column8.index;

        var column9 = gridEstimateProductEntryList.GetColumnByField("TaxType");
        PrdTaxTypeIndex = column9.index;

        var column10 = gridEstimateProductEntryList.GetColumnByField("Charges");
        PrdChargesIndex = column10.index;

        var column11 = gridEstimateProductEntryList.GetColumnByField("NetAmount");
        PrdNetAmtIndex = column11.index;

        var column12 = gridEstimateProductEntryList.GetColumnByField("BudgetedPrice");
        PrdBudgPricIndex = column12.index;

        var column40 = gridEstimateProductEntryList.GetColumnByField("AddlDesc");
        PrdAddlDescIndex = 4;

        var column13 = gridEstimateProductEntryList.GetColumnByField("Remarks");
        PrdRemarksIndex = column13.index;

        var column14 = gridEstimateProductEntryList.GetColumnByField("ProductId");
        PrdProdIdIndex = column14.index;

        var column15 = gridEstimateProductEntryList.GetColumnByField("ProductsWarehouseID");
        PrdProdWarehouseIDIndex = column15.index;

        var column16 = gridEstimateProductEntryList.GetColumnByField("UpdateEdit");
        PrdUpdateEditIndex = column16.index;

        var column17 = gridEstimateProductEntryList.GetColumnByField("Tag_Details_ID");
        PrdTagDetlsIDIndex = column17.index;

        var column18 = gridEstimateProductEntryList.GetColumnByField("Tag_Production_ID");
        PrdTagProdnIDIndex = column18.index;

        var column19 = gridEstimateProductEntryList.GetColumnByField("TaxTypeID");
        PrdTaxTypeIDIndex = column19.index;

        var column20 = gridEstimateProductEntryList.GetColumnByField("ProdHSN");
        PrdProdHSNIndex = column20.index;

        //var AddDescPORD = gridEstimateProductEntryList.GetColumnByField("AddlDesc");
        //PrdAddDescIndex = AddDescPORD.index;

        var column21 = gridEstimateResourcesList.GetColumnByField("ProductName");
        ResProductNameIndex = column21.index;

        var column22 = gridEstimateResourcesList.GetColumnByField("ProductDescription");
        ResProdDescIndex = column22.index;

        var column23 = gridEstimateResourcesList.GetColumnByField("ProductQty");
        ResProdQtyIndex = column23.index;

        var column24 = gridEstimateResourcesList.GetColumnByField("ProductUOM");
        ResProdUOMIndex = column24.index;

        var column25 = gridEstimateResourcesList.GetColumnByField("Warehouse");
        ResWarehouseIndex = column25.index;

        var column26 = gridEstimateResourcesList.GetColumnByField("Price");
        ResPriceIndex = column26.index;

        var column27 = gridEstimateResourcesList.GetColumnByField("Discount");
        ResDiscountIndex = column27.index;

        var column28 = gridEstimateResourcesList.GetColumnByField("Amount");
        ResAmountIndex = column28.index;

        var column29 = gridEstimateResourcesList.GetColumnByField("TaxType");
        ResTaxTypeIndex = column29.index;

        var column30 = gridEstimateResourcesList.GetColumnByField("ResourceCharges");
        ResResourceChargesIndex = column30.index;

        var column31 = gridEstimateResourcesList.GetColumnByField("NetAmount");
        ResNetAmountIndex = column31.index;

        var column32 = gridEstimateResourcesList.GetColumnByField("BudgetedPrice");
        ResBudgetedPriceIndex = column32.index;

        var column33 = gridEstimateResourcesList.GetColumnByField("AddlDesc");
        ResAddlDescIndex = 4;//column33.index;

        var column34 = gridEstimateResourcesList.GetColumnByField("Remarks");
        ResRemarksIndex = column34.index;

        var column35 = gridEstimateResourcesList.GetColumnByField("ProductId");
        ResProductIdIndex = column35.index;

        var column36 = gridEstimateResourcesList.GetColumnByField("ProductsWarehouseID");
        ResProdWarehouseIDIndex = column36.index;

        var column37 = gridEstimateResourcesList.GetColumnByField("UpdateEdit");
        ResUpdateEditIndex = column37.index;

        var column38 = gridEstimateResourcesList.GetColumnByField("TaxTypeID");
        ResTaxTypeIDIndex = column38.index;

        var column39 = gridEstimateResourcesList.GetColumnByField("ProdHSN");
        ResProdHSNIndex = column39.index;

        //  LoadingPanel.Hide();
    });

    function AddNewRowGridResources() {
        //gridEstimateResourcesList.batchEditApi.StartEdit(index, 1);
        gridEstimateResourcesList.batchEditApi.EndEdit();
        gridEstimateResourcesList.AddNewRow();
        index = globalrowindex2;
        //resufflegrid2Serial();

        setTimeout(function () {
            gridEstimateResourcesList.batchEditApi.EndEdit();
            gridEstimateResourcesList.batchEditApi.StartEdit(index, ResSrlIndex);
        }, 200);
    }

    function resufflegrid2Serial() {
        var sl = 1;
        var visiablerow = gridEstimateResourcesList.GetVisibleRowsOnPage();
        if (visiablerow > 0 && rowtime2 == 0) {
            sl = visiablerow;
            rowtime2++;
        }
        for (var i = -1; i > -500; i--) {
            if (gridEstimateResourcesList.GetRow(i)) {
                gridEstimateResourcesList.batchEditApi.StartEdit(i, ResSrlIndex);
                gridEstimateResourcesList.GetEditor('SlNO').SetText(sl);
                gridEstimateResourcesList.batchEditApi.StartEdit(i, ResSrlIndex);
                sl = sl + 1;
            }
        }
    }

    function addNewRowToEditgrid() {
        gridEstimateProductEntryList.batchEditApi.EndEdit();
        gridEstimateProductEntryList.AddNewRow();

        var sl = 1;
        var visiablerow = gridEstimateProductEntryList.GetVisibleRowsOnPage();
        if (visiablerow > 0) {
            sl = visiablerow;
        }
        gridEstimateProductEntryList.batchEditApi.StartEdit(globalrowindex, PrdSrlIndex);
        gridEstimateProductEntryList.GetEditor('SlNO').SetText(sl);


        setTimeout(function () {
            gridEstimateProductEntryList.batchEditApi.EndEdit();
            gridEstimateProductEntryList.batchEditApi.StartEdit(globalrowindex, PrdProdNameIndex);
        }, 200);


    }

    function addNewRowToEditResourcegrid() {
        gridEstimateResourcesList.batchEditApi.EndEdit();
        gridEstimateResourcesList.AddNewRow();

        var sl = 1;
        var visiablerow = gridEstimateResourcesList.GetVisibleRowsOnPage();
        if (visiablerow > 0) {
            sl = visiablerow;
        }
        gridEstimateResourcesList.batchEditApi.StartEdit(globalrowindex2, ResSrlIndex);
        gridEstimateResourcesList.GetEditor('SlNO').SetText(sl);

        setTimeout(function () {
            gridEstimateResourcesList.batchEditApi.EndEdit();
            gridEstimateResourcesList.batchEditApi.StartEdit(globalrowindex2, ResProductNameIndex);
        }, 200);


    }

    function DeleteRowProductGrid(edit) {
        var sl = 1;
        gridEstimateProductEntryList.batchEditApi.EndEdit();
        for (var i = 0; i < 500; i++) {
            if (gridEstimateProductEntryList.GetRow(i) && i != edit) {
                var tr = gridEstimateProductEntryList.GetRow(i);
                if (tr.style.display != "none") {

                    gridEstimateProductEntryList.batchEditApi.StartEdit(i, PrdSrlIndex);
                    gridEstimateProductEntryList.GetEditor('SlNO').SetText(sl);
                    //if (grid.GetEditor('low').GetText() == "") {
                    //    grid.GetEditor('low').SetText(0);
                    //    grid.GetEditor('high').SetText(0);
                    //    grid.GetEditor('value').SetText(0);
                    //}
                    gridEstimateProductEntryList.batchEditApi.StartEdit(i, PrdSrlIndex);
                    sl = sl + 1;
                }
            }
        }

        for (var i = -1; i > -500; i--) {
            if (gridEstimateProductEntryList.GetRow(i) && i != edit) {

                var tr = gridEstimateProductEntryList.GetRow(i);
                if (tr.style.display != "none") {

                    gridEstimateProductEntryList.batchEditApi.StartEdit(i, PrdSrlIndex);
                    gridEstimateProductEntryList.GetEditor('SlNO').SetText(sl);
                    //if (grid.GetEditor('low').GetText() == "") {
                    //    grid.GetEditor('low').SetText(0);
                    //    grid.GetEditor('high').SetText(0);
                    //    grid.GetEditor('value').SetText(0);
                    //}
                    gridEstimateProductEntryList.batchEditApi.StartEdit(i, PrdSrlIndex);
                    sl = sl + 1;
                }
            }
        }
    }

    function DeleteRowResourceGrid(edit) {
        var sl = 1;
        gridEstimateResourcesList.batchEditApi.EndEdit();
        for (var i = 0; i < 500; i++) {
            if (gridEstimateResourcesList.GetRow(i) && i != edit) {
                var tr = gridEstimateResourcesList.GetRow(i);
                if (tr.style.display != "none") {

                    gridEstimateResourcesList.batchEditApi.StartEdit(i, ResSrlIndex);
                    gridEstimateResourcesList.GetEditor('SlNO').SetText(sl);
                    //if (grid.GetEditor('low').GetText() == "") {
                    //    grid.GetEditor('low').SetText(0);
                    //    grid.GetEditor('high').SetText(0);
                    //    grid.GetEditor('value').SetText(0);
                    //}
                    gridEstimateResourcesList.batchEditApi.StartEdit(i, ResSrlIndex);
                    sl = sl + 1;
                }
            }
        }

        for (var i = -1; i > -500; i--) {
            if (gridEstimateResourcesList.GetRow(i) && i != edit) {

                var tr = gridEstimateResourcesList.GetRow(i);
                if (tr.style.display != "none") {

                    gridEstimateResourcesList.batchEditApi.StartEdit(i, ResSrlIndex);
                    gridEstimateResourcesList.GetEditor('SlNO').SetText(sl);
                    //if (grid.GetEditor('low').GetText() == "") {
                    //    grid.GetEditor('low').SetText(0);
                    //    grid.GetEditor('high').SetText(0);
                    //    grid.GetEditor('value').SetText(0);
                    //}
                    gridEstimateResourcesList.batchEditApi.StartEdit(i, ResSrlIndex);
                    sl = sl + 1;
                }
            }
        }
    }

    /*---------------Arindam*----------*/
    function AddNewRowWithSl() {

        gridEstimateProductEntryList.batchEditApi.EndEdit();
        gridEstimateProductEntryList.AddNewRow();
        index = globalrowindex;
        // resuffleSerial();

        setTimeout(function () {
            gridEstimateProductEntryList.batchEditApi.EndEdit();
            gridEstimateProductEntryList.batchEditApi.StartEdit(index, PrdSrlIndex);
        }, 200);
    }


    function resuffleSerial() {

        var sl = 1;
        var visiablerow = gridEstimateProductEntryList.GetVisibleRowsOnPage();
        if (visiablerow > 0 && rowtime == 0) {
            sl = visiablerow;
            rowtime++;
        }

        for (var i = -1; i > -500; i--) {
            if (gridEstimateProductEntryList.GetRow(i)) {
                gridEstimateProductEntryList.batchEditApi.StartEdit(i, PrdSrlIndex);
                gridEstimateProductEntryList.GetEditor('SlNO').SetText(sl);
                //if (grid.GetEditor('low').GetText() == "") {
                //    grid.GetEditor('low').SetText(0);
                //    grid.GetEditor('high').SetText(0);
                //    grid.GetEditor('value').SetText(0);
                //}
                gridEstimateProductEntryList.batchEditApi.StartEdit(i, PrdSrlIndex);
                sl = sl + 1;
            }
        }
    }


    function grid_CustomButtonGridResourcesClick() {
        //if (e.buttonID == "Delete") {
        var noofvisiblerows = gridEstimateResourcesList.GetVisibleRowsOnPage();

        if (noofvisiblerows != 1) {

            //@*$.ajax({
            //    type: "POST",
            //    url: "@Url.Action("ResAdditionalDesc", "Estimate")",
            //    data: { AddlDesc: "", ResAddlDescSl: gridEstimateResourcesList.GetEditor('SlNO').GetText(), Command: "RemarksRemove" },
            //    success: function (response) {
            //        if (response != null) {
            //            $("#txt_ResAddlDesc").val('');
            //            $("#hdnResAddlDescSl").val('');
            //            gridEstimateResourcesList.batchEditApi.StartEdit(ResIndexAddl, ResProdQtyIndex);
            //        }
            //    }
            //});*@


            gridEstimateResourcesList.DeleteRow(globalrowindex2);

            if ($('#hdnDetailsID').val() == 0) {
                resufflegrid2Serial();
            }
            else {
                DeleteRowResourceGrid(globalrowindex2);
            }



            EstimateGridResourceSetTotalAmount();
        }
        //}
        //e.processOnServer = false;

    }

    function grid_CustomButtonClick() {
        // if (e.buttonID == "DeleteProduct") {
        var noofvisiblerows = gridEstimateProductEntryList.GetVisibleRowsOnPage();

        if (noofvisiblerows != 1) {
            gridEstimateProductEntryList.DeleteRow(globalrowindex);
            //gridEstimateProductEntryList.DeleteRow(e.visibleIndex);
            if ($('#hdnDetailsID').val() == 0) {

                resuffleSerial();
            }
            else {
                DeleteRowProductGrid(globalrowindex);
                // DeleteRowProductGrid(e.visibleIndex);
            }

            //     EstimateGridSetTotalAmount();

            //@*$.ajax({
            //    type: "POST",
            //    url: "@Url.Action("ProdAdditionalDesc", "Estimate")",
            //    data: { AddlDesc: "", ProdAddlDescSl: gridEstimateProductEntryList.GetEditor('SlNO').GetText(), Command: "RemarksRemove" },
            //    success: function (response) {
            //        if (response != null) {
            //            //jAlert(response.Message);
            //            $("#txt_AddlDesc").val('');
            //            $("#hdnProdAddlDescSl").val('');
            //            //debugger;
            //            gridEstimateProductEntryList.batchEditApi.StartEdit(ProdIndexAddl, PrdProdQtyIndex);
            //        }
            //    }
            //});*@
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

        //gridEstimateResourcesList.batchEditApi.EndEdit();
        //var slsno = gridEstimateResourcesList.batchEditApi.GetCellValue(e.visibleIndex, 'SlNO');
        //var ProductName = gridEstimateResourcesList.batchEditApi.GetCellValue(e.visibleIndex, 'ProductName');
        //$("#hdnResAddlDescSl").val(slsno);

        if ($("#CustomerId").val() == null || $("#CustomerId").val() == "") {
            jAlert('Please select customer.');
            CustomerTxt.Focus();
            LoadingPanel.Hide();
        }
        else {
            //slno = gridEstimateResourcesList.GetDataRow(globalrowindex2).children[0].innerHTML.trim();
            //GridNonInventoryProductlist("", "nonInventory", slno);
            typemodal = "nonInventory";
            var txt = "<table border='1' class='dynamicPopupTbl' width=\"100%\"><tr class=\"HeaderStyle\"><th class='hide'>id</th><th>Product Code</th><th>Product Name</th><th>UOM</th><th>Inventory</th><th>HSN/SAC</th><th>Class</th><th>Brand</th></tr><table>";
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
            HeaderCaption.push("UOM");
            HeaderCaption.push("Inventory");
            HeaderCaption.push("HSN/SAC");
            HeaderCaption.push("Class");
            HeaderCaption.push("Brand");


            //  callonServerScroll("../Models/PMS_WebServiceList.asmx/GetProductDetailsList", OtherDetails, "GridProductTable", HeaderCaption, "NonIProductIndex", "SetGridNonInventoryProduct");
            callonServer("../Models/PMS_WebServiceList.asmx/GetProductDetailsList", OtherDetails, "GridProductTable", HeaderCaption, "NonIProductIndex", "SetGridNonInventoryProduct");
        }

        //setTimeout(function () {
        //    $('#txtGridProductName').focus();
        //}, 600);
    }

    function OpenProductList(s, e) {


        gridEstimateProductEntryList.batchEditApi.EndEdit();
        var slsno = gridEstimateProductEntryList.batchEditApi.GetCellValue(e.visibleIndex, 'SlNO');
        var ProductName = gridEstimateProductEntryList.batchEditApi.GetCellValue(e.visibleIndex, 'ProductName');
        $("#hdnProdAddlDescSl").val(slsno);

        if ($("#CustomerId").val() == null || $("#CustomerId").val() == "") {
            jAlert('Please select customer.');
            CustomerTxt.Focus();
            LoadingPanel.Hide();
        }
        else {
            if (gridEstimateProductEntryList.GetDataRow(globalrowindex) != null) {
                slno = gridEstimateProductEntryList.GetDataRow(globalrowindex).children[0].innerHTML.trim();
            }
            //debugger;
            GridProductlist("", "A", slno);
            typemodal = "A";

            var txt = "<table  class='dynamicPopupTbl' width=\"100%\"><tr class=\"HeaderStyle\"><th class='hide'>id</th><th>Product Code</th><th>Product Name</th><th>UOM</th><th>Inventory</th><th>HSN/SAC</th><th>Class</th><th>Brand</th></tr><table>";
            $("#txtGridProductName").val("");
            document.getElementById("GridProductTable").innerHTML = txt;
            $('#GridProductlistModel').modal('show');
            $('#txtGridProductName').focus();
            setTimeout(function () {
                $('#txtGridProductName').focus();
            }, 200);
        }
    }

    function OpenSellableProductList(s, e) {

        if ($("#CustomerId").val() == null || $("#CustomerId").val() == "") {
            jAlert('Please select customer.');
            CustomerTxt.Focus();
            LoadingPanel.Hide();
        }
        else {

            GridProductSellablelist("", "SellableProduct", slno);
            typemodal = "SellableProduct";

            var txt = "<table  class='dynamicPopupTbl' width=\"100%\"><tr class=\"HeaderStyle\"><th class='hide'>id</th><th>Product Code</th><th>Product Name</th><th>UOM</th><th>Inventory</th><th>HSN/SAC</th><th>Class</th><th>Brand</th></tr><table>";
            $("#txtGridProductName").val("");
            document.getElementById("GridProductTable").innerHTML = txt;
            $('#GridProductlistModel').modal('show');
            $('#txtGridProductName').focus();
            setTimeout(function () {
                $('#txtGridProductName').focus();
            }, 200);
        }
    }


    function OpenResSellableProductList(s, e) {

        if ($("#CustomerId").val() == null || $("#CustomerId").val() == "") {
            jAlert('Please select customer.');
            CustomerTxt.Focus();
            LoadingPanel.Hide();
        }
        else {

            GridResProductSellablelist("", "ResSellableProduct", slno);
            typemodal = "ResSellableProduct";

            var txt = "<table  class='dynamicPopupTbl' width=\"100%\"><tr class=\"HeaderStyle\"><th class='hide'>id</th><th>Product Code</th><th>Product Name</th><th>UOM</th><th>Inventory</th><th>HSN/SAC</th><th>Class</th><th>Brand</th></tr><table>";
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
        if (gridEstimateProductEntryList.GetDataRow(globalrowindex) != null) {
            slno = gridEstimateProductEntryList.GetDataRow(globalrowindex).children[0].innerHTML.trim();
        }
        $('#GridTaxTypelistModel').modal('show');
        //setTimeout(function () {
        //    $('#ddlTaxTypelist').focus();
        //}, 600);
    }

    function OpenAddlDesc(s, e) {
        //debugger;
        //e.processOnServer = true;
        gridEstimateProductEntryList.batchEditApi.EndEdit();
        if (gridEstimateProductEntryList.GetDataRow(globalrowindex) != null) {
            slno = gridEstimateProductEntryList.GetDataRow(globalrowindex).children[0].innerHTML.trim();
        }
        //gridEstimateProductEntryList.batchEditApi.StartEdit(e.visibleIndex, PrdSrlIndex);
        //var slsno = gridEstimateProductEntryList.GetEditor('SlNO').GetText();

        var slsno = gridEstimateProductEntryList.batchEditApi.GetCellValue(globalrowindex, 'SlNO');

        // var ProductName = gridEstimateProductEntryList.GetEditor('ProductName').GetText();
        var ProductName = gridEstimateProductEntryList.batchEditApi.GetCellValue(globalrowindex, 'ProductName');
        ProdIndexAddl = globalrowindex;
        if (ProductName == "" || ProductName == null) {
            jAlert("Please select product before select Addl. Desc.!");
            setTimeout(function () {
                gridEstimateProductEntryList.batchEditApi.StartEdit(globalrowindex, PrdSrlIndex);
            }, 600);
            return false;
        }
        else {
            if (slsno != null) {
                //@*var txt_AddlDesc = $('#txt_AddlDesc').val();
                //$("#hdnProdAddlDescSl").val(slsno);
                //$.ajax({
                //    type: "POST",
                //    url: "@Url.Action("ProdAdditionalDesc", "Estimate")",
                //    data: { AddlDesc: txt_AddlDesc, ProdAddlDescSl: slsno, Command: "RemarksDisplay" },
                //    success: function (response) {
                //        if (response != null) {
                //            //jAlert(response.Message);
                //            $('#txt_AddlDesc').val(response.Message);
                //        }
                //    }
                //});*@
                var AddlDesc = gridEstimateProductEntryList.batchEditApi.GetCellValue(globalrowindex, 'AddlDesc');
                $('#txt_AddlDesc').val(AddlDesc);
            }

            $('#GridAddlDescModel').modal('show');
            //setTimeout(function () {
            //    $('#txt_AddlDesc').focus();
            //}, 600);
        }
    }


    function OpenResAddlDesc(s, e) {
        // debugger;

        gridEstimateResourcesList.batchEditApi.EndEdit();
        if (gridEstimateResourcesList.GetDataRow(globalrowindex2) != null) {
            slno = gridEstimateResourcesList.GetDataRow(globalrowindex2).children[0].innerHTML.trim();
        }
        //gridEstimateResourcesList.batchEditApi.StartEdit(e.visibleIndex, ResSrlIndex);
        //var slsno = gridEstimateResourcesList.GetEditor('SlNO').GetText(); e.visibleIndex

        var slsno = gridEstimateResourcesList.batchEditApi.GetCellValue(globalrowindex2, 'SlNO');
        // var ProductName = gridEstimateResourcesList.GetEditor('ProductName').GetText();
        var ProductName = gridEstimateResourcesList.batchEditApi.GetCellValue(globalrowindex2, 'ProductName');
        ResIndexAddl = globalrowindex2;
        if (ProductName == "" || ProductName == null) {
            jAlert("Please select product before select Addl. Desc.!");
            setTimeout(function () {
                gridEstimateResourcesList.batchEditApi.StartEdit(globalrowindex2, ResSrlIndex);
            }, 600);
            return false;
        }
        else {
            if (slsno != null) {
                var txt_ResAddlDesc = $('#txt_ResAddlDesc').val();
                $("#hdnResAddlDescSl").val(slsno);
                //@*$.ajax({
                //    type: "POST",
                //    url: "@Url.Action("ResAdditionalDesc", "Estimate")",
                //    data: { AddlDesc: txt_ResAddlDesc, ResAddlDescSl: slsno, Command: "RemarksDisplay" },
                //    success: function (response) {
                //        if (response != null) {
                //            //jAlert(response.Message);
                //            $('#txt_ResAddlDesc').val(response.Message);
                //        }
                //    }
                //});*@
                var AddlDesc = gridEstimateResourcesList.batchEditApi.GetCellValue(globalrowindex2, 'AddlDesc');
                $('#txt_ResAddlDesc').val(AddlDesc);

            }

            $('#GridResAddlDescModel').modal('show');
            setTimeout(function () {
                $('#txt_ResAddlDesc').focus();
            }, 600);
        }
    }

    function OpenResourceTaxTypeList(s, e) {
        if (gridEstimateResourcesList.GetDataRow(globalrowindex2) != null) {
            slno = gridEstimateResourcesList.GetDataRow(globalrowindex2).children[0].innerHTML.trim();
        }
        $('#RescTaxTypelistModel').modal('show');
        //setTimeout(function () {
        //    $('#ddlRescTaxTypelist').focus();
        //}, 600);
    }

    function OpenWarehouseList(s, e) {
        slno = gridEstimateProductEntryList.GetDataRow(globalrowindex).children[0].innerHTML.trim();
        gridEstimateProductEntryList.batchEditApi.StartEdit(globalrowindex, PrdProdNameIndex);
        var ProductName = gridEstimateProductEntryList.GetEditor('ProductName').GetText();
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
        slno = gridEstimateProductEntryList.GetDataRow(globalrowindex).children[0].innerHTML.trim();
        gridEstimateProductEntryList.batchEditApi.StartEdit(globalrowindex, PrdProdNameIndex);
        var ProductName = gridEstimateProductEntryList.GetEditor('ProductName').GetText();
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
                //url: "@Url.Action("getWarehouseRecord", "Estimate")",
                url: "../Estimate/getWarehouseRecord",
                data: { branchid: BankBranchID },
                success: function (response) {
                    $('#ddlWarehouselist').html('');
                    var html = "";
                    html = html + "<option value='select'>Select</option>";
                    for (var i = 0; i < response.length; i++) {
                        html = html + "<option value='" + response[i].WarehouseID + "'>" + response[i].WarehouseName + "</option>";
                    }
                    //$('#ddlWarehouselist').html(html);
                    $('#ddlProductWarehouselist').html(html);
                    //   gridEstimateProductEntryList.batchEditApi.EndEdit();
                    //$('#setWarehousegrid').focus();
                    // $('#GridWarehouselistModel').modal('show');

                    //setTimeout(function () {
                    //    $('#ddlWarehouselist').focus();
                    //}, 600);
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

            gridEstimateProductEntryList.batchEditApi.StartEdit(globalrowindex, PrdWarehouseIndex);
            gridEstimateProductEntryList.GetEditor('Warehouse').SetText(Warehousetxt);

            gridEstimateProductEntryList.batchEditApi.StartEdit(globalrowindex, PrdProdWarehouseIDIndex);
            gridEstimateProductEntryList.GetEditor('ProductsWarehouseID').SetText(Warehouseid);
            $('#GridWarehouselistModel').modal('hide');
            gridEstimateProductEntryList.batchEditApi.StartEdit(globalrowindex, PrdPriceIndex);
        }
        else {
            jAlert("Please select warehouse!");
        }
    }


    function SetTaxTypeInGrid() {

        //var TaxTypeid = $('#ddlTaxTypelist').val();
        var TaxTypeid = $('#ddlTaxTypelistProduct').val();
        //var TaxTypetxt = $('#ddlTaxTypelist option:selected').text();
        if (TaxTypeid != "") {
            //gridEstimateProductEntryList.batchEditApi.StartEdit(globalrowindex, PrdTaxTypeIndex);
            //gridEstimateProductEntryList.GetEditor('TaxType').SetText(TaxTypetxt);

            //gridEstimateProductEntryList.batchEditApi.StartEdit(globalrowindex, PrdTaxTypeIDIndex);
            //gridEstimateProductEntryList.GetEditor('TaxTypeID').SetText(TaxTypeid);
            //$('#GridTaxTypelistModel').modal('hide');



            //var Price = gridEstimateProductEntryList.batchEditApi.GetCellValue(globalrowindex, 'Price');
            //var Qty = gridEstimateProductEntryList.batchEditApi.GetCellValue(globalrowindex, 'ProductQty');
            //var Discount = gridEstimateProductEntryList.batchEditApi.GetCellValue(globalrowindex, 'Discount');

            var Price = cProductPrice.GetText();
            var Qty = cProductQty.GetText();
            var Discount = cProductDiscount.GetText();

            if (Price != "" && Qty != "") {
                var amount = parseFloat((parseFloat(Qty).toFixed(4)) * (parseFloat(Price).toFixed(2))).toFixed(2);
                var amountAfterDiscount = parseFloat(amount) - ((parseFloat(Discount) * parseFloat(amount)) / 100);
                var Tamount = parseFloat(DecimalRoundoff(amountAfterDiscount, 2)).toFixed(2);

                if (TaxTypeid == "3") {
                    //gridEstimateProductEntryList.batchEditApi.StartEdit(globalrowindex, PrdAmountIndex);
                    //gridEstimateProductEntryList.GetEditor('Amount').SetText(Tamount);
                    cProductAmount.SetText(Tamount);

                    //gridEstimateProductEntryList.batchEditApi.StartEdit(globalrowindex, PrdNetAmtIndex);
                    //gridEstimateProductEntryList.GetEditor('NetAmount').SetText(Tamount);
                    cProductNetAmount.SetText(Tamount);

                    //if (gridEstimateProductEntryList.batchEditApi.GetCellValue(globalrowindex, 11) == null) {
                    //gridEstimateProductEntryList.batchEditApi.StartEdit(globalrowindex, PrdChargesIndex);
                    //gridEstimateProductEntryList.GetEditor('Charges').SetText("0.00");
                    cProductCharges.SetText("0.00");
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

                    //var ret = caluculateAndSetGST(gridEstimateProductEntryList.GetEditor("Amount"), gridEstimateProductEntryList.GetEditor("Charges"), gridEstimateProductEntryList.GetEditor("NetAmount"),
                    //        gridEstimateProductEntryList.GetEditor("ProdHSN").GetText(), DecimalRoundoff(amount, 2), DecimalRoundoff(amountAfterDiscount, 2), taxType, "15", $('#ddlBankBranch').val())
                    var ret = caluculateAndSetGST(cProductAmount.GetText(), cProductCharges.GetText(), cProductNetAmount.GetText(),
                 $("#hdnProdHSN").val(), DecimalRoundoff(amount, 2), DecimalRoundoff(amountAfterDiscount, 2), taxType, "15", $('#ddlBankBranch').val())
                    var data = ret.split('~');

                    //var Netamind = gridEstimateProductEntryList.batchEditApi.GetColumnIndex('NetAmount');
                    //gridEstimateProductEntryList.batchEditApi.StartEdit(globalrowindex, Netamind);
                    //gridEstimateProductEntryList.GetEditor('NetAmount').SetText(data[2]);
                    cProductNetAmount.SetText(data[2]);

                    //var Charges = gridEstimateProductEntryList.batchEditApi.GetColumnIndex('Charges');
                    //gridEstimateProductEntryList.batchEditApi.StartEdit(globalrowindex, Charges);
                    //gridEstimateProductEntryList.GetEditor('Charges').SetText(data[1].toString());
                    cProductCharges.SetText(data[1].toString());

                    //var amind = gridEstimateProductEntryList.batchEditApi.GetColumnIndex('Amount');
                    //gridEstimateProductEntryList.batchEditApi.StartEdit(globalrowindex, amind);
                    //gridEstimateProductEntryList.GetEditor('Amount').SetText(data[0]);
                    cProductAmount.SetText(data[0]);

                }
            }
            //gridEstimateProductEntryList.batchEditApi.StartEdit(globalrowindex, PrdChargesIndex);
            cProductCharges.Focus();
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
            //@*$.ajax({
            //    type: "POST",
            //    url: "@Url.Action("ProdAdditionalDesc", "Estimate")",
            //    data: { AddlDesc: txt_AddlDesc, ProdAddlDescSl: hdnProdAddlDescSl, Command: "RemarksAdd" },
            //    success: function (response) {
            //        if (response != null) {
            //            //jAlert(response.Message);
            //            $("#txt_AddlDesc").val('');
            //            $("#hdnProdAddlDescSl").val('');
            //            //debugger;
            //            gridEstimateProductEntryList.batchEditApi.StartEdit(ProdIndexAddl, PrdProdQtyIndex);
            //        }
            //    }
            //});*@

            var AddDesc = gridEstimateProductEntryList.batchEditApi.GetColumnIndex('AddlDesc');
            gridEstimateProductEntryList.batchEditApi.StartEdit(ProdIndexAddl, AddDesc);
            gridEstimateProductEntryList.GetEditor('AddlDesc').SetText(txt_AddlDesc);

            gridEstimateProductEntryList.batchEditApi.StartEdit(ProdIndexAddl, PrdProdQtyIndex);
        }
        else {
            //jAlert("Please Enter Additional Description.");
            //$("#txt_AddlDesc").focus();
            $('#GridAddlDescModel').modal('hide');
            //@*$.ajax({
            //    type: "POST",
            //    url: "@Url.Action("ProdAdditionalDesc", "Estimate")",
            //    data: { AddlDesc: txt_AddlDesc, ProdAddlDescSl: hdnProdAddlDescSl, Command: "RemarksRemove" },
            //    success: function (response) {
            //        if (response != null) {
            //            //jAlert(response.Message);
            //            $("#txt_AddlDesc").val('');
            //            $("#hdnProdAddlDescSl").val('');
            //            //debugger;
            //            gridEstimateProductEntryList.batchEditApi.StartEdit(ProdIndexAddl, PrdProdQtyIndex);
            //        }
            //    }
            //});*@
            var AddDesc = gridEstimateProductEntryList.batchEditApi.GetColumnIndex('AddlDesc');
            gridEstimateProductEntryList.batchEditApi.StartEdit(ProdIndexAddl, AddDesc);
            gridEstimateProductEntryList.GetEditor('AddlDesc').SetText(txt_AddlDesc);

            gridEstimateProductEntryList.batchEditApi.StartEdit(ProdIndexAddl, PrdProdQtyIndex);
        }
    }

    function SetResAddlDescGrid() {
        var txt_AddlDesc = $('#txt_ResAddlDesc').val();
        var hdnResAddlDescSl = $("#hdnResAddlDescSl").val();
        if (txt_AddlDesc != "") {
            $('#GridResAddlDescModel').modal('hide');
            var strOut = txt_AddlDesc.substring(0, 7) + "...";
            //@*$.ajax({
            //    type: "POST",
            //    url: "@Url.Action("ResAdditionalDesc", "Estimate")",
            //    data: { AddlDesc: txt_AddlDesc, ResAddlDescSl: hdnResAddlDescSl, Command: "RemarksAdd" },
            //    success: function (response) {
            //        if (response != null) {
            //            //jAlert(response.Message);
            //            $("#txt_ResAddlDesc").val('');
            //            $("#hdnResAddlDescSl").val('');
            //            //debugger;
            //            gridEstimateResourcesList.batchEditApi.StartEdit(ResIndexAddl, ResProdQtyIndex);
            //            // gridEstimateResourcesList.GetEditor('AddlDesc').SetText(strOut);
            //        }
            //    }
            //});*@
            var AddDescres = gridEstimateResourcesList.batchEditApi.GetColumnIndex('AddlDesc');
            gridEstimateResourcesList.batchEditApi.StartEdit(ResIndexAddl, AddDescres);
            gridEstimateResourcesList.GetEditor('AddlDesc').SetText(txt_AddlDesc);

            gridEstimateResourcesList.batchEditApi.StartEdit(ResIndexAddl, ResProdQtyIndex);
        }
        else {
            //jAlert("Please Enter Additional Description.");
            //$("#txt_ResAddlDesc").focus();
            $('#GridResAddlDescModel').modal('hide');

            //@*$.ajax({
            //    type: "POST",
            //    url: "@Url.Action("ResAdditionalDesc", "Estimate")",
            //    data: { AddlDesc: txt_AddlDesc, ResAddlDescSl: hdnResAddlDescSl, Command: "RemarksRemove" },
            //    success: function (response) {
            //        if (response != null) {
            //            //jAlert(response.Message);
            //            $("#txt_ResAddlDesc").val('');
            //            $("#hdnResAddlDescSl").val('');
            //            //debugger;
            //            gridEstimateResourcesList.batchEditApi.StartEdit(ResIndexAddl, ResProdQtyIndex);
            //            // gridEstimateResourcesList.GetEditor('AddlDesc').SetText('');
            //        }
            //    }
            //});*@

            var AddDescres = gridEstimateResourcesList.batchEditApi.GetColumnIndex('AddlDesc');
            gridEstimateResourcesList.batchEditApi.StartEdit(ResIndexAddl, AddDescres);
            gridEstimateResourcesList.GetEditor('AddlDesc').SetText(txt_AddlDesc);

            gridEstimateResourcesList.batchEditApi.StartEdit(ResIndexAddl, ResProdQtyIndex);
        }
    }

    function SetWarehouseAfterProduct() {
        gridEstimateProductEntryList.batchEditApi.StartEdit(globalrowindex, PrdPriceIndex);
    }

    function SetTaxTypeAfterProduct() {
        //  gridEstimateProductEntryList.batchEditApi.StartEdit(globalrowindex, 11);
    }
    function SetAddlDescClose() {
        $("#txt_AddlDesc").val('');
        $("#hdnProdAddlDescSl").val('');
        gridEstimateProductEntryList.batchEditApi.StartEdit(ProdIndexAddl, PrdProdQtyIndex);
    }

    function SetResAddlDescClose() {
        $("#txt_ResAddlDesc").val('');
        $("#hdnResAddlDescSl").val('');
        gridEstimateResourcesList.batchEditApi.StartEdit(ResIndexAddl, ResProdQtyIndex);
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
        slno = gridEstimateProductEntryList.GetDataRow(globalrowindex).children[0].innerHTML.trim();
        gridEstimateProductEntryList.batchEditApi.StartEdit(globalrowindex, PrdProdNameIndex);
        var ProductName = gridEstimateProductEntryList.GetEditor('ProductName').GetText();
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
        var productid = gridEstimateProductEntryList.batchEditApi.GetCellValue(globalrowindex, 'ProductId');
        var OtherDetails = {}
        var EstimateDate = GetServerDateFormat(EstimateDate_dt.GetValue());
        OtherDetails.SearchKey = SearchKey;
        OtherDetails.ProductID = productid;
        OtherDetails.EstimateDate = EstimateDate;
        OtherDetails.BranchID = $('#ddlBankBranch option:selected').val();
        var HeaderCaption = [];
        HeaderCaption.push("Estimate No.");
        HeaderCaption.push("Estimate Date");
        HeaderCaption.push("Revision No.");
        HeaderCaption.push("Revision Date");

        //callonServerScroll("../Models/PMS_WebServiceList.asmx/GetBOMList", OtherDetails, "GridEstimateTable", HeaderCaption, "EstimateIndex", "SetGridEstimateProduct");
        callonServer("../Models/PMS_WebServiceList.asmx/GetBOMList", OtherDetails, "GridEstimateTable", HeaderCaption, "EstimateIndex", "SetGridEstimateProduct");
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
            HeaderCaption.push("UOM");
            HeaderCaption.push("Inventory");
            HeaderCaption.push("HSN/SAC");
            HeaderCaption.push("Class");
            HeaderCaption.push("Brand");

            //callonServerScroll("../Models/PMS_WebServiceList.asmx/GetProductDetailsList", OtherDetails, "GridProductTable", HeaderCaption, "GridProductIndex", "SetGridProduct");
            callonServer("../Models/PMS_WebServiceList.asmx/GetProductDetailsList", OtherDetails, "GridProductTable", HeaderCaption, "GridProductIndex", "SetGridProduct");
        }



    }

    function GridProductSellablelist(SearchKey, type, txtid) {
        //debugger;

        if (SearchKey != "") {
            gridproductlist = 1;
            var OtherDetails = {}
            OtherDetails.SearchKey = SearchKey;
            OtherDetails.Type = "SellableProduct";
            gridtxtbox = txtid;
            var HeaderCaption = [];
            // HeaderCaption.push("Product ID");
            HeaderCaption.push("Product Code");
            HeaderCaption.push("Product Name");
            HeaderCaption.push("UOM");
            HeaderCaption.push("Inventory");
            HeaderCaption.push("HSN/SAC");
            HeaderCaption.push("Class");
            HeaderCaption.push("Brand");

            //callonServerScroll("../Models/PMS_WebServiceList.asmx/GetProductDetailsList", OtherDetails, "GridProductTable", HeaderCaption, "GridProductIndex", "SetSellableProduct");
            callonServer("../Models/PMS_WebServiceList.asmx/GetProductDetailsList", OtherDetails, "GridProductTable", HeaderCaption, "GridSellableProductIndex", "SetSellableProduct");
        }
    }

    function SetSellableProduct(Id, Name, e) {
        // debugger;
        gridproductlist = 0;
        var ProductID = Id;
        var ProductName = Name;

        if (ProductID != "") {
            var data = ProductID.split('|');
            ProductID = data[0];
            btnSellable.SetText(ProductName);
            $("#hdnSellableProductID").val(ProductID);
            $('#GridProductlistModel').modal('hide');
        }
    }

    function GridResProductSellablelist(SearchKey, type, txtid) {
        //debugger;

        if (SearchKey != "") {
            gridproductlist = 1;
            var OtherDetails = {}
            OtherDetails.SearchKey = SearchKey;
            OtherDetails.Type = "SellableProduct";
            gridtxtbox = txtid;
            var HeaderCaption = [];
            // HeaderCaption.push("Product ID");
            HeaderCaption.push("Product Code");
            HeaderCaption.push("Product Name");
            HeaderCaption.push("UOM");
            HeaderCaption.push("Inventory");
            HeaderCaption.push("HSN/SAC");
            HeaderCaption.push("Class");
            HeaderCaption.push("Brand");

            //callonServerScroll("../Models/PMS_WebServiceList.asmx/GetProductDetailsList", OtherDetails, "GridProductTable", HeaderCaption, "GridProductIndex", "SetSellableProduct");
            callonServer("../Models/PMS_WebServiceList.asmx/GetProductDetailsList", OtherDetails, "GridProductTable", HeaderCaption, "GridSellableProductIndex", "SetResSellableProduct");
        }
    }

    function SetResSellableProduct(Id, Name, e) {
        // debugger;
        gridproductlist = 0;
        var ProductID = Id;
        var ProductName = Name;

        if (ProductID != "") {
            var data = ProductID.split('|');
            ProductID = data[0];
            btnResSellable.SetText(ProductName);
            $("#hdnResSellableProductID").val(ProductID);
            $('#GridProductlistModel').modal('hide');
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
        HeaderCaption.push("UOM");
        HeaderCaption.push("Inventory");
        HeaderCaption.push("HSN/SAC");
        HeaderCaption.push("Class");
        HeaderCaption.push("Brand");

        //callonServerScroll("../Models/PMS_WebServiceList.asmx/GetProductDetailsList", OtherDetails, "ProductTable", HeaderCaption, "ProductIndex", "SetProduct");
        callonServer("../Models/PMS_WebServiceList.asmx/GetProductDetailsList", OtherDetails, "ProductTable", HeaderCaption, "ProductIndex", "SetProduct");
    }

    function SetGridNonInventoryProduct(Id, Name, e) {
        gridnonproductlist = 0;
        var ProductID = Id;
        var ProductName = Name;
        //alert('');
        if (ProductID != "") {

            var slno = $("#hdnResAddlDescSl").val();
            //@*$.ajax({
            //    type: "POST",
            //    url: "@Url.Action("ResAdditionalDesc", "Estimate")",
            //    data: { AddlDesc: ' ', ProdAddlDescSl: slno, Command: "RemarksRemove" },
            //    success: function (response) {
            //        if (response != null) {
            //            //jAlert(response.Message);
            //            //  $("#txt_AddlDesc").val('');
            //            $("#hdnResAddlDescSl").val('');
            //        }
            //    }
            //});*@

            var data = ProductID.split('|');
            ProductID = data[0];

            //var Dis = gridEstimateResourcesList.batchEditApi.GetColumnIndex('Discount');
            //gridEstimateResourcesList.batchEditApi.StartEdit(globalrowindex2, Dis);
            //gridEstimateResourcesList.GetEditor('Discount').SetText("0.00");

            //var amind = gridEstimateResourcesList.batchEditApi.GetColumnIndex('Amount');
            //gridEstimateResourcesList.batchEditApi.StartEdit(globalrowindex2, amind);
            //gridEstimateResourcesList.GetEditor('Amount').SetText("0.00");

            //var ProdHSN = gridEstimateResourcesList.batchEditApi.GetColumnIndex('ProdHSN');
            //gridEstimateResourcesList.batchEditApi.StartEdit(globalrowindex2, ProdHSN);
            //gridEstimateResourcesList.GetEditor('ProdHSN').SetText(data[7]);

            //var qtyindex = gridEstimateResourcesList.batchEditApi.GetColumnIndex('ProductQty');
            //gridEstimateResourcesList.batchEditApi.StartEdit(globalrowindex2, qtyindex);
            //gridEstimateResourcesList.GetEditor('ProductQty').SetText("0.00");

            //gridEstimateResourcesList.batchEditApi.StartEdit(globalrowindex2, ResProductIdIndex);
            //gridEstimateResourcesList.GetEditor('ProductId').SetText(ProductID);

            //gridEstimateResourcesList.batchEditApi.StartEdit(globalrowindex2, ResResourceChargesIndex);
            //gridEstimateResourcesList.GetEditor('ResourceCharges').SetText("0.00");

            //gridEstimateResourcesList.batchEditApi.StartEdit(globalrowindex2, ResProductNameIndex);
            //gridEstimateResourcesList.GetEditor('ProductName').SetText(ProductName);
            ////gridEstimateResourcesList.GetDataRow(globalrowindex).children[1].innerHTML = ProductName;

            ////$('#' + gridtxtbox + '_txtbox').val(ProductName);
            //$("#ddl_AmountAre").prop('disabled', 'disabled');
            //$('#GridProductlistModel').modal('hide');
            //gridEstimateResourcesList.batchEditApi.StartEdit(globalrowindex2, ResProdDescIndex);
            //gridEstimateResourcesList.GetEditor('ProductDescription').SetText(data[6]);
            //gridEstimateResourcesList.batchEditApi.StartEdit(globalrowindex2, ResProdUOMIndex);

            //gridEstimateResourcesList.GetEditor('ProductUOM').SetText(data[1]);
            ////$('#' + gridtxtbox + '_txtDescription').val(data[2]);
            ////$('#' + gridtxtbox + '_txtStockUOM').val(data[1]);
            //gridEstimateResourcesList.batchEditApi.StartEdit(globalrowindex2, ResPriceIndex);
            //gridEstimateResourcesList.GetEditor('Price').SetText(data[3]);
            //gridEstimateResourcesList.batchEditApi.StartEdit(globalrowindex2, ResProdWarehouseIDIndex);
            //gridEstimateResourcesList.GetEditor('ProductsWarehouseID').SetText(data[4]);
            ////$('#' + gridtxtbox + '_txtPrice').val(data[3]);
            ////gridEstimateResourcesList.batchEditApi.StartEdit(globalrowindex2, 6);
            ////gridEstimateResourcesList.GetEditor('Warehouse').SetText(data[5]);
            //gridEstimateResourcesList.batchEditApi.StartEdit(globalrowindex2, ResProdDescIndex);
            ////btnFinishedItem.SetText(ProductName);
            ////document.getElementById('hdnProductID').value = Id;


            $("#txtResProductdescription").val(data[6]);
            btnResProduct.SetText(ProductName);
            $("#hdnResProductID").val(ProductID);
            $("#hdnResHSN").val(data[7]);
            $("#txt_ResProdQty").val("0.00");
            $("#txt_ResProdUOM").val(data[1]);
            $("#txt_ResProdWarehouse").val("");
            $("#txt_ResProdAmount").val("0.00");
            $("#txt_ResProdPrice").val(data[3]);
            $("#txt_ResProdDiscount").val("0.00");
            $("#txt_ResProdCharges").val("0.00");
            $("#txt_ResProdNetAmount").val("0.00");
            $("#txt_ResProdBudgetedPrice").val("0.00");

            $("#ddl_AmountAre").prop('disabled', 'disabled');
            $('#GridProductlistModel').modal('hide');

            $("#txt_ResAddlDescProd").focus();
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
            //@*$.ajax({
            //    type: "POST",
            //    url: "@Url.Action("ProdAdditionalDesc", "Estimate")",
            //    data: { AddlDesc: ' ', ProdAddlDescSl: slno, Command: "RemarksRemove" },
            //    success: function (response) {
            //        if (response != null) {
            //            //jAlert(response.Message);
            //            $("#txt_AddlDesc").val('');
            //            $("#hdnProdAddlDescSl").val('');
            //        }
            //    }
            //});*@
            var data = ProductID.split('|');
            ProductID = data[0];

            //debugger;
            //var Dis = gridEstimateProductEntryList.batchEditApi.GetColumnIndex('Discount');
            //gridEstimateProductEntryList.batchEditApi.StartEdit(globalrowindex, Dis);
            //gridEstimateProductEntryList.GetEditor('Discount').SetText("0.00");

            //var amind = gridEstimateProductEntryList.batchEditApi.GetColumnIndex('Amount');
            //gridEstimateProductEntryList.batchEditApi.StartEdit(globalrowindex, amind);
            //gridEstimateProductEntryList.GetEditor('Amount').SetText("0.00");

            //var Netamind = gridEstimateProductEntryList.batchEditApi.GetColumnIndex('NetAmount');
            //gridEstimateProductEntryList.batchEditApi.StartEdit(globalrowindex, Netamind);
            //gridEstimateProductEntryList.GetEditor('NetAmount').SetText("0.00");

            //var HSN = gridEstimateProductEntryList.batchEditApi.GetColumnIndex('ProdHSN');
            //gridEstimateProductEntryList.batchEditApi.StartEdit(globalrowindex, HSN);
            //gridEstimateProductEntryList.GetEditor('ProdHSN').SetText(data[7]);

            //var qtyindex = gridEstimateProductEntryList.batchEditApi.GetColumnIndex('ProductQty');
            //gridEstimateProductEntryList.batchEditApi.StartEdit(globalrowindex, qtyindex);
            //gridEstimateProductEntryList.GetEditor('ProductQty').SetText("0.00");

            //var qtyindex = gridEstimateProductEntryList.batchEditApi.GetColumnIndex('BudgetedPrice');
            //gridEstimateProductEntryList.batchEditApi.StartEdit(globalrowindex, PrdBudgPricIndex);
            //gridEstimateProductEntryList.GetEditor('BudgetedPrice').SetText("0.00");

            //gridEstimateProductEntryList.batchEditApi.StartEdit(globalrowindex, PrdProdIdIndex);
            //gridEstimateProductEntryList.GetEditor('ProductId').SetText(ProductID);

            //gridEstimateProductEntryList.batchEditApi.StartEdit(globalrowindex, PrdChargesIndex);
            //gridEstimateProductEntryList.GetEditor('Charges').SetText("0.00");

            //gridEstimateProductEntryList.batchEditApi.StartEdit(globalrowindex, PrdProdNameIndex);
            //gridEstimateProductEntryList.GetEditor('ProductName').SetText(ProductName);
            ////gridEstimateProductEntryList.GetDataRow(globalrowindex).children[1].innerHTML = ProductName;

            ////$('#' + gridtxtbox + '_txtbox').val(ProductName);
            //$("#ddl_AmountAre").prop('disabled', 'disabled');
            //$('#GridProductlistModel').modal('hide');
            //gridEstimateProductEntryList.batchEditApi.StartEdit(globalrowindex, PrdProdDescIndex);
            //gridEstimateProductEntryList.GetEditor('ProductDescription').SetText(data[6]);
            //gridEstimateProductEntryList.batchEditApi.StartEdit(globalrowindex, PrdProdUOMIndex);

            //gridEstimateProductEntryList.GetEditor('ProductUOM').SetText(data[1]);
            ////$('#' + gridtxtbox + '_txtDescription').val(data[2]);
            ////$('#' + gridtxtbox + '_txtStockUOM').val(data[1]);
            //gridEstimateProductEntryList.batchEditApi.StartEdit(globalrowindex, PrdPriceIndex);
            //gridEstimateProductEntryList.GetEditor('Price').SetText(data[3]);

            ////gridEstimateProductEntryList.batchEditApi.StartEdit(globalrowindex, 16);
            ////gridEstimateProductEntryList.GetEditor('ProductsWarehouseID').SetText(data[4]);

            ////$('#' + gridtxtbox + '_txtPrice').val(data[3]);

            ////gridEstimateProductEntryList.batchEditApi.StartEdit(globalrowindex, 6);
            ////gridEstimateProductEntryList.GetEditor('Warehouse').SetText(data[5]);

            ////gridEstimateProductEntryList.batchEditApi.StartEdit(globalrowindex, PrdProdQtyIndex);

            //gridEstimateProductEntryList.batchEditApi.StartEdit(globalrowindex, PrdProdDescIndex);


            ////btnFinishedItem.SetText(ProductName);
            ////document.getElementById('hdnProductID').value = Id;


            $("#txtProductdescription").val(data[6]);
            btnProduct.SetText(ProductName);
            cProductQty.SetText("0.00");
            $("#txt_ProdUOM").val(data[1]);
            $("#txt_ProdWarehouse").val("");
            //$("#txt_ProdAmount").val("0.00");
            cProductPrice.SetText(data[3]);
            //$("#txt_ProdPrice").val(data[3]);
            cProductAmount.SetText("0.00");
            //$("#txt_ProdDiscount").val("0.00");
            cProductDiscount.SetText("0.00");
            $("#txt_ProdCharges").val("0.00");
            //cProductAmount.SetText("0.00");
            //$("#txt_ProdNetAmount").val("0.00");
            cProductNetAmount.SetText("0.00");
            //$("#txt_ProdBudgetedPrice").val("0.00");
            cProductBudgetedPrice.SetText("0.00");
            $("#ddl_AmountAre").prop('disabled', 'disabled');
            $('#GridProductlistModel').modal('hide');
            $("#hdnProdHSN").val(data[7]);
            $("#hdnProdProductID").val(ProductID);
            GridWarehouselist();

            $("#txt_AddlDescProd").focus();
        }
    }

    function EstimateGridSetTotalAmount(s, e) {
        //debugger;
        //var TypId = gridEstimateProductEntryList.GetEditor("TaxTypeID").GetText();
        //var Price = gridEstimateProductEntryList.batchEditApi.GetCellValue(globalrowindex, 'Price');
        //var Qty = gridEstimateProductEntryList.batchEditApi.GetCellValue(globalrowindex, 'ProductQty');
        //var Discount = gridEstimateProductEntryList.batchEditApi.GetCellValue(globalrowindex, 'Discount');
        var TypId = $("#ddlTaxTypelistProduct").val();
        var Price = cProductPrice.GetText();
        var Qty = cProductQty.GetText();
        var Discount = cProductDiscount.GetText();
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

            var ret = caluculateAndSetGST(cProductAmount.GetText(), cProductCharges.GetText(), cProductNetAmount.GetText(),
                    $("#hdnProdHSN").val(), DecimalRoundoff(amount, 2), DecimalRoundoff(amountAfterDiscount, 2), taxType, "15", $('#ddlBankBranch').val())
            var data = ret.split('~');


            if (taxType == "O") {
                //var Netamind = gridEstimateProductEntryList.batchEditApi.GetColumnIndex('NetAmount');
                //gridEstimateProductEntryList.batchEditApi.StartEdit(globalrowindex, Netamind);
                //gridEstimateProductEntryList.GetEditor('NetAmount').SetText(Tamount);
                cProductNetAmount.SetText(Tamount);

                //var Charges = gridEstimateProductEntryList.batchEditApi.GetColumnIndex('Charges');
                //gridEstimateProductEntryList.batchEditApi.StartEdit(globalrowindex, Charges);
                //gridEstimateProductEntryList.GetEditor('Charges').SetText("0.00");
                cProductCharges.SetText("0.00");

                //var amind = gridEstimateProductEntryList.batchEditApi.GetColumnIndex('Amount');
                //gridEstimateProductEntryList.batchEditApi.StartEdit(globalrowindex, amind);
                //gridEstimateProductEntryList.GetEditor('Amount').SetText(Tamount);
                cProductAmount.SetText(Tamount);
            }
            else {
                if (TypId == "3" || TypId == "") {
                    //var Netamind = gridEstimateProductEntryList.batchEditApi.GetColumnIndex('NetAmount');
                    //gridEstimateProductEntryList.batchEditApi.StartEdit(globalrowindex, Netamind);
                    //gridEstimateProductEntryList.GetEditor('NetAmount').SetText(Tamount);
                    cProductNetAmount.SetText(Tamount);

                    //var Charges = gridEstimateProductEntryList.batchEditApi.GetColumnIndex('Charges');
                    //gridEstimateProductEntryList.batchEditApi.StartEdit(globalrowindex, Charges);
                    //gridEstimateProductEntryList.GetEditor('Charges').SetText("0.00");
                    cProductCharges.SetText("0.00");

                    //var amind = gridEstimateProductEntryList.batchEditApi.GetColumnIndex('Amount');
                    //gridEstimateProductEntryList.batchEditApi.StartEdit(globalrowindex, amind);
                    //gridEstimateProductEntryList.GetEditor('Amount').SetText(Tamount);
                    cProductAmount.SetText(Tamount);
                }
                else {
                    //var Netamind = gridEstimateProductEntryList.batchEditApi.GetColumnIndex('NetAmount');
                    //gridEstimateProductEntryList.batchEditApi.StartEdit(globalrowindex, Netamind);
                    //gridEstimateProductEntryList.GetEditor('NetAmount').SetText(data[2]);
                    cProductNetAmount.SetText(data[2]);

                    //var Charges = gridEstimateProductEntryList.batchEditApi.GetColumnIndex('Charges');
                    //gridEstimateProductEntryList.batchEditApi.StartEdit(globalrowindex, Charges);
                    //gridEstimateProductEntryList.GetEditor('Charges').SetText(data[1].toString());
                    cProductCharges.SetText(data[1].toString());

                    //var amind = gridEstimateProductEntryList.batchEditApi.GetColumnIndex('Amount');
                    //gridEstimateProductEntryList.batchEditApi.StartEdit(globalrowindex, amind);
                    //gridEstimateProductEntryList.GetEditor('Amount').SetText(data[0]);
                    cProductAmount.SetText(data[0]);
                }
            }
            //var TaxType = gridEstimateProductEntryList.batchEditApi.GetColumnIndex('TaxType');
            //gridEstimateProductEntryList.batchEditApi.StartEdit(globalrowindex, TaxType);

            gridEstimateProductEntryList.batchEditApi.EndEdit();

            //var Dis = gridEstimateProductEntryList.batchEditApi.GetColumnIndex('Discount');
            //gridEstimateProductEntryList.batchEditApi.StartEdit(globalrowindex, Dis);
            //gridEstimateProductEntryList.GetEditor('Discount').SetText("0.00");
        }
        var ToTalAmount = 0;
        for (var i = 500; i > -500; i--) {
            if (gridEstimateProductEntryList.GetRow(i)) {
                var Amountval = gridEstimateProductEntryList.batchEditApi.GetCellValue(i, 'NetAmount');
                if (Amountval != "" && Amountval != null && Amountval != undefined) {
                    var ToTalAmount = parseFloat(parseFloat(ToTalAmount) + parseFloat(Amountval)).toFixed(2);
                    $('#txtGridProductEntryTotalAmount').val(ToTalAmount);
                }
            }
        }






        //var ToTalAmount = $('#txtGridProductEntryTotalAmount').val();
        //var Amountval = gridEstimateProductEntryList.batchEditApi.GetCellValue(globalrowindex, 'Amount');
        //if (ToTalAmount != "" || ToTalAmount != undefined || ToTalAmount != null) {
        //    ToTalAmount = parseFloat(0).toFixed(2);
        //}
        //if (Amountval != "" && Amountval != null && Amountval != undefined) {
        //    var calTotalAmount = parseFloat(parseFloat(ToTalAmount) + parseFloat(Amountval)).toFixed(2);
        //    $('#txtGridProductEntryTotalAmount').val(calTotalAmount);
        //}

    }

    function EstimateGridResourceSetTotalAmount(s, e) {

        var ToTalAmount = 0;
        for (var i = 500; i > -500; i--) {
            if (gridEstimateResourcesList.GetRow(i)) {
                var Amountval = gridEstimateResourcesList.batchEditApi.GetCellValue(i, 'NetAmount');
                if (Amountval != "" && Amountval != null && Amountval != undefined) {
                    var ToTalAmount = parseFloat(parseFloat(ToTalAmount) + parseFloat(Amountval)).toFixed(2);
                    $('#txtGridResourcesTotalAmount').val(ToTalAmount);
                }
            }
        }


        //var ToTalAmount = $('#txtGridResourcesTotalAmount').val();
        //var Amountval = gridEstimateResourcesList.batchEditApi.GetCellValue(globalrowindex2, 'Amount');
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

            gridEstimateProductEntryList.batchEditApi.StartEdit(globalrowindex, PrdTaxTypeIndex);
            //gridEstimateProductEntryList.GetEditor('Charges').SetText(Estimate_No);

            // gridEstimateProductEntryList.batchEditApi.StartEdit(globalrowindex, 10);
            //gridEstimateProductEntryList.GetEditor('RevNo').SetText(REV_No);

            //  gridEstimateProductEntryList.batchEditApi.StartEdit(globalrowindex, 11);
            //gridEstimateProductEntryList.GetEditor('RevDate').SetText(Estimate_Date);

            gridEstimateProductEntryList.batchEditApi.StartEdit(globalrowindex, PrdPriceIndex);
            gridEstimateProductEntryList.GetEditor('Price').SetText(Rate);

            gridEstimateProductEntryList.batchEditApi.StartEdit(globalrowindex, PrdTagDetlsIDIndex);
            gridEstimateProductEntryList.GetEditor('Tag_Details_ID').SetText(Details_ID);

            gridEstimateProductEntryList.batchEditApi.StartEdit(globalrowindex, PrdTagProdnIDIndex);
            gridEstimateProductEntryList.GetEditor('Tag_Production_ID').SetText(Production_ID);

            EstimateGridSetAmount("", "");

            setTimeout(function () {
                gridEstimateProductEntryList.batchEditApi.EndEdit();
                gridEstimateProductEntryList.batchEditApi.StartEdit();
                // EstimateGridSetTotalAmount("", "");
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
        //if (gridEstimateResourcesList.batchEditApi.HasChanges()) {
        //    gridEstimateResourcesList.UpdateEdit();
        //}
    }

    function OnResourcesEndCallbacks(s, e) {
        gridEstimateProductEntryList.AddNewRow();
        SuffleRows();
        gridEstimateProductEntryList.UpdateEdit();
    }
    function OnResourcesEndCallback() {
        //debugger;
        // AddNewRowGridResources();

        $('#EstimateNo').val('');
        var EstimateDate = GetServerDateFormat(EstimateDate_dt.GetValue());
        //var EstimateDate = $('#EstimateDate_dt').val();
        $('#hdnFinishedItem').val('');
        $('#FinishedQty').val(parseFloat(0).toFixed(4));
        $('#FinishedUom').val('');
        //$('#slcEstimatetype').val($("#slcEstimatetype option:first").val());
        $('#RevisionNo').val('');
        $('#ddlBankBranch').val($("#ddlBankBranch option:first").val());
        //$('#ddlWarehouse').val($("#ddlWarehouse option:first").val());
        $('#hdnSchemaId').val('');
        $('#txtActualAdditionalCost').val(parseFloat(0).toFixed(4));
        $('#txtGridProductEntryTotalAmount').val(parseFloat(0).toFixed(2));
        $('#txtGridResourcesTotalAmount').val(parseFloat(0).toFixed(2));


        $('#ddlSchema').val($("#ddlSchema option:first").val());
        $('#EstimateNo').val('');
        //$('#EbtnFinishedItem').hide();
        $('#EEstimateNo').hide();
        $('#EEstimateDate_dt').hide();
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
        ////debugger;
        if (Message == "duplicate" && DetailsID == 0 && ProductionID == 0) {
            savemode = "";
            if (Message == "duplicate") {
                jAlert('This Estimate no already present!');
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
            if (DetailsID > 0 && ProductionID > 0 && GEstimateNo != "") {
                ProductionID = 0;
                DetailsID = 0;
                var msg = $("#hdnApproveReject").val();
                if (msg == "Approve") {
                    jAlert('Estimate Number : ' + GEstimateNo + ' approve successfully.', 'Alert!', function (r) {
                        if (r) {
                            if (savemode == "Exit") {
                                setTimeout(function () {
                                    var url = $('#hdnEstimateListPage').val();
                                    window.location.href = url;
                                }, 500);
                            }
                        }

                    });
                }
                else if (msg == "Reject") {
                    jAlert('Estimate Number : ' + GEstimateNo + ' reject successfully.', 'Alert!', function (r) {
                        if (r) {
                            if (savemode == "Exit") {
                                setTimeout(function () {
                                    var url = $('#hdnEstimateListPage').val();
                                    window.location.href = url;
                                }, 500);
                            }
                        }
                    });
                }
                else {
                    jAlert('Estimate Number : ' + GEstimateNo + ' saved successfully.', 'Alert!', function (r) {
                        if (r) {
                            if (savemode == "Exit") {
                                setTimeout(function () {
                                    var url = $('#hdnEstimateListPage').val();
                                    window.location.href = url;
                                }, 500);
                            }
                        }
                    });
                }
                LoadingPanel.Hide();
                // jAlert('Estimate Number : ' + GEstimateNo + ' Successfully saved.');


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
        ////debugger;
        DetailsID = s.cpDetailsID;
        ProductionID = s.cpProductionID;
        GEstimateNo = s.cpEstimateNo;
        Message = s.cpMessage;
        $('#hdnDetailsID').val(DetailsID);
        if (s.cpBatchUpdate == "1") {

            s.cpBatchUpdate = "0";

            //if (gridEstimateResourcesList.batchEditApi.HasChanges()) {
            //    gridEstimateResourcesList.UpdateEdit();

            setTimeout(function () {
                OnResourcesEndCallback();
            }, 1500);

            //}
        }
        //else {
        // AddNewRowWithSl();
        $('#txtGridProductEntryTotalAmount').val(parseFloat(0).toFixed(2));
        if (Message == "duplicate" && hasmsg == 0) {
            jAlert('This Estimate no already present!');
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
                else if (typemodal == 'SellableProduct') {
                    GridProductSellablelist($("#txtGridProductName").val(), typemodal, null);
                    gridproductlist = 1;
                }
                else if (typemodal == 'ResSellableProduct') {
                    GridResProductSellablelist($("#txtGridProductName").val(), typemodal, null);
                    gridproductlist = 1;
                }
                else {
                    GridProductlist($("#txtGridProductName").val(), typemodal, null);
                    gridproductlist = 1;
                }
            }
            else {
                var txt = "<table  class='dynamicPopupTbl' width=\"100%\"><tr class=\"HeaderStyle\"><th class='hide'>id</th><th>Product Code</th><th>Product Name</th><th>UOM</th><th>Inventory</th><th>HSN/SAC</th><th>Class</th><th>Brand</th></tr><table>";
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
                        var localcolumn = gridEstimateProductEntryList.batchEditApi.GetColumnIndex('Price');
                        gridEstimateProductEntryList.batchEditApi.StartEdit(globalrowindex, localcolumn);
                    }, 500);

                }


                if (Estimatelinkindex == 1) {
                    Estimatelinkindex = 0;
                    setTimeout(function () {
                        var localcolumn = gridEstimateProductEntryList.batchEditApi.GetColumnIndex('Remarks');
                        gridEstimateProductEntryList.batchEditApi.StartEdit(globalrowindex, localcolumn);
                    }, 500);

                }

                setTimeout(function () {
                    var localcolumn = gridEstimateProductEntryList.batchEditApi.GetColumnIndex('Discount');
                    gridEstimateProductEntryList.batchEditApi.StartEdit(globalrowindex, localcolumn);
                }, 500);

                if (gridproductlist == 1) {
                    gridproductlist = 0;
                    setTimeout(function () {
                        var localcolumn = gridEstimateProductEntryList.batchEditApi.GetColumnIndex('ProductQty');
                        gridEstimateProductEntryList.batchEditApi.StartEdit(globalrowindex, localcolumn);
                    }, 500);

                }

                if (gridnonproductlist == 1) {
                    gridnonproductlist = 0;
                    setTimeout(function () {
                        var localcolumn = gridEstimateResourcesList.batchEditApi.GetColumnIndex('ProductQty');
                        gridEstimateResourcesList.batchEditApi.StartEdit(globalrowindex2, localcolumn);
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
            //url: "@Url.Action("getWarehouseRecord", "Estimate")",
            url: "../Estimate/getWarehouseRecord",
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

    

   


    function addNewRowTogridResources() {
        gridEstimateResourcesList.batchEditApi.EndEdit();
        //AddNewRowGridResources();
        index = globalrowindex2;
        setTimeout(function () {
            gridEstimateResourcesList.batchEditApi.EndEdit();
            gridEstimateResourcesList.batchEditApi.StartEdit(index, ResSrlIndex);
        }, 200);

    }

    function addNewRowTogrid() {
        gridEstimateProductEntryList.batchEditApi.EndEdit();
        //gridEstimateProductEntryList.AddNewRow();
        // gridEstimateProductEntryList.AddNewRow();
        //

        //  AddNewRowWithSl();
        index = globalrowindex;


        setTimeout(function () {
            gridEstimateProductEntryList.batchEditApi.EndEdit();
            gridEstimateProductEntryList.batchEditApi.StartEdit(index, PrdSrlIndex);
        }, 200);

    }

    function EstimateGridSetDiscount(s, e) {
        // debugger;
        // gridEstimateProductEntryList.batchEditApi.EndEdit();
        //var Price = s.GetValueString();


        //var TypId = gridEstimateProductEntryList.batchEditApi.GetCellValue(globalrowindex, 'TaxTypeID');
        //var TypId = gridEstimateProductEntryList.GetEditor("TaxTypeID").GetText();
        //var Price = gridEstimateProductEntryList.batchEditApi.GetCellValue(globalrowindex, 'Price');
        //var Qty = gridEstimateProductEntryList.batchEditApi.GetCellValue(globalrowindex, 'ProductQty');
        //var Discount = gridEstimateProductEntryList.batchEditApi.GetCellValue(globalrowindex, 'Discount');
        var TypId = $("#ddlTaxTypelistProduct").val();
        var Price = cProductPrice.GetText();
        var Qty = cProductQty.GetText();
        var Discount = cProductDiscount.GetText();

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

            //var ret = caluculateAndSetGST(gridEstimateProductEntryList.GetEditor("Amount"), gridEstimateProductEntryList.GetEditor("Charges"), gridEstimateProductEntryList.GetEditor("NetAmount"),
            //        gridEstimateProductEntryList.GetEditor("ProdHSN").GetText(), DecimalRoundoff(amount, 2), DecimalRoundoff(amountAfterDiscount, 2), taxType, "15", $('#ddlBankBranch').val())
            var ret = caluculateAndSetGST(cProductAmount.GetText(), cProductCharges.GetText(), cProductNetAmount.GetText(),
                 $("#hdnProdHSN").val(), DecimalRoundoff(amount, 2), DecimalRoundoff(amountAfterDiscount, 2), taxType, "15", $('#ddlBankBranch').val())

            var data = ret.split('~');


            if (taxType == "O") {
                //var Netamind = gridEstimateProductEntryList.batchEditApi.GetColumnIndex('NetAmount');
                //gridEstimateProductEntryList.batchEditApi.StartEdit(globalrowindex, Netamind);
                //gridEstimateProductEntryList.GetEditor('NetAmount').SetText(Tamount);
                cProductNetAmount.SetText(Tamount);

                //var Charges = gridEstimateProductEntryList.batchEditApi.GetColumnIndex('Charges');
                //gridEstimateProductEntryList.batchEditApi.StartEdit(globalrowindex, Charges);
                //gridEstimateProductEntryList.GetEditor('Charges').SetText("0.00");
                cProductCharges.SetText("0.00");

                //var amind = gridEstimateProductEntryList.batchEditApi.GetColumnIndex('Amount');
                //gridEstimateProductEntryList.batchEditApi.StartEdit(globalrowindex, amind);
                //gridEstimateProductEntryList.GetEditor('Amount').SetText(Tamount);
                cProductAmount.SetText(Tamount);
            }
            else {
                if (TypId == "3" || TypId == "") {
                    //var Netamind = gridEstimateProductEntryList.batchEditApi.GetColumnIndex('NetAmount');
                    //gridEstimateProductEntryList.batchEditApi.StartEdit(globalrowindex, Netamind);
                    //gridEstimateProductEntryList.GetEditor('NetAmount').SetText(Tamount);
                    cProductNetAmount.SetText(Tamount);

                    //var Charges = gridEstimateProductEntryList.batchEditApi.GetColumnIndex('Charges');
                    //gridEstimateProductEntryList.batchEditApi.StartEdit(globalrowindex, Charges);
                    //gridEstimateProductEntryList.GetEditor('Charges').SetText("0.00");
                    cProductCharges.SetText("0.00");

                    //var amind = gridEstimateProductEntryList.batchEditApi.GetColumnIndex('Amount');
                    //gridEstimateProductEntryList.batchEditApi.StartEdit(globalrowindex, amind);
                    //gridEstimateProductEntryList.GetEditor('Amount').SetText(Tamount);
                    cProductAmount.SetText(Tamount);
                }
                else {
                    //var Netamind = gridEstimateProductEntryList.batchEditApi.GetColumnIndex('NetAmount');
                    //gridEstimateProductEntryList.batchEditApi.StartEdit(globalrowindex, Netamind);
                    //gridEstimateProductEntryList.GetEditor('NetAmount').SetText(data[2]);
                    cProductNetAmount.SetText(data[2]);

                    //var Charges = gridEstimateProductEntryList.batchEditApi.GetColumnIndex('Charges');
                    //gridEstimateProductEntryList.batchEditApi.StartEdit(globalrowindex, Charges);
                    //gridEstimateProductEntryList.GetEditor('Charges').SetText(data[1].toString());
                    cProductCharges.SetText(data[1].toString());

                    //var amind = gridEstimateProductEntryList.batchEditApi.GetColumnIndex('Amount');
                    //gridEstimateProductEntryList.batchEditApi.StartEdit(globalrowindex, amind);
                    //gridEstimateProductEntryList.GetEditor('Amount').SetText(data[0]);
                    cProductAmount.SetText(data[0]);
                }
            }

            //  gridEstimateProductEntryList.batchEditApi.StartEdit(globalrowindex, 9);
            // gridEstimateProductEntryList.GetEditor('Amount').SetText(Tamount);
        }
        // EstimateGridSetTotalAmount("", "");
        //  gridEstimateProductEntryList.batchEditApi.EndEdit();

        $("#ddlTaxTypelistProduct").focus();
    }


    function EstimateGridSetAmount(s, e) {
        //debugger;
        // gridEstimateProductEntryList.batchEditApi.EndEdit();
        //var Price = s.GetValueString();

        //var TypId = gridEstimateProductEntryList.GetEditor("TaxTypeID").GetText();
        //var Price = gridEstimateProductEntryList.batchEditApi.GetCellValue(globalrowindex, 'Price');
        //var Qty = gridEstimateProductEntryList.batchEditApi.GetCellValue(globalrowindex, 'ProductQty');
        //var Discount = gridEstimateProductEntryList.batchEditApi.GetCellValue(globalrowindex, 'Discount');

        //var TypId = gridEstimateProductEntryList.GetEditor("TaxTypeID").GetText();
        var TypId = $("#ddlTaxTypelistProduct").val();
        var Price = cProductPrice.GetText();
        var Qty = cProductQty.GetText();
        var Discount = cProductDiscount.GetText();

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

            //var ret = caluculateAndSetGST(gridEstimateProductEntryList.GetEditor("Amount"), gridEstimateProductEntryList.GetEditor("Charges"), gridEstimateProductEntryList.GetEditor("NetAmount"),
            //        gridEstimateProductEntryList.GetEditor("ProdHSN").GetText(), DecimalRoundoff(amount, 2), DecimalRoundoff(amountAfterDiscount, 2), taxType, "15", $('#ddlBankBranch').val())
            var ret = caluculateAndSetGST(cProductAmount.GetText(), cProductCharges.GetText(), cProductNetAmount.GetText(),
                 $("#hdnProdHSN").val(), DecimalRoundoff(amount, 2), DecimalRoundoff(amountAfterDiscount, 2), taxType, "15", $('#ddlBankBranch').val())

            var data = ret.split('~');


            if (taxType == "O") {
                //var Netamind = gridEstimateProductEntryList.batchEditApi.GetColumnIndex('NetAmount');
                //gridEstimateProductEntryList.batchEditApi.StartEdit(globalrowindex, Netamind);
                //gridEstimateProductEntryList.GetEditor('NetAmount').SetText(Tamount);
                cProductNetAmount.SetText(Tamount);

                //var Charges = gridEstimateProductEntryList.batchEditApi.GetColumnIndex('Charges');
                //gridEstimateProductEntryList.batchEditApi.StartEdit(globalrowindex, Charges);
                //gridEstimateProductEntryList.GetEditor('Charges').SetText("0.00");
                cProductCharges.SetText("0.00");

                //var amind = gridEstimateProductEntryList.batchEditApi.GetColumnIndex('Amount');
                //gridEstimateProductEntryList.batchEditApi.StartEdit(globalrowindex, amind);
                //gridEstimateProductEntryList.GetEditor('Amount').SetText(Tamount);
                cProductAmount.SetText(Tamount);
            }
            else {
                if (TypId == "3" || TypId == "") {
                    //var Netamind = gridEstimateProductEntryList.batchEditApi.GetColumnIndex('NetAmount');
                    //gridEstimateProductEntryList.batchEditApi.StartEdit(globalrowindex, Netamind);
                    //gridEstimateProductEntryList.GetEditor('NetAmount').SetText(Tamount);
                    cProductNetAmount.SetText(Tamount);

                    //var Charges = gridEstimateProductEntryList.batchEditApi.GetColumnIndex('Charges');
                    //gridEstimateProductEntryList.batchEditApi.StartEdit(globalrowindex, Charges);
                    //gridEstimateProductEntryList.GetEditor('Charges').SetText("0.00");
                    cProductCharges.SetText("0.00");

                    //var amind = gridEstimateProductEntryList.batchEditApi.GetColumnIndex('Amount');
                    //gridEstimateProductEntryList.batchEditApi.StartEdit(globalrowindex, amind);
                    //gridEstimateProductEntryList.GetEditor('Amount').SetText(Tamount);
                    cProductAmount.SetText(Tamount);
                }
                else {
                    //var Netamind = gridEstimateProductEntryList.batchEditApi.GetColumnIndex('NetAmount');
                    //gridEstimateProductEntryList.batchEditApi.StartEdit(globalrowindex, Netamind);
                    //gridEstimateProductEntryList.GetEditor('NetAmount').SetText(data[2]);
                    cProductNetAmount.SetText(data[2]);

                    //var Charges = gridEstimateProductEntryList.batchEditApi.GetColumnIndex('Charges');
                    //gridEstimateProductEntryList.batchEditApi.StartEdit(globalrowindex, Charges);
                    //gridEstimateProductEntryList.GetEditor('Charges').SetText(data[1].toString());
                    cProductCharges.SetText(data[1].toString());

                    //var amind = gridEstimateProductEntryList.batchEditApi.GetColumnIndex('Amount');
                    //gridEstimateProductEntryList.batchEditApi.StartEdit(globalrowindex, amind);
                    //gridEstimateProductEntryList.GetEditor('Amount').SetText(data[0]);
                    cProductAmount.SetText(data[0]);
                }
            }
            //var Discount = gridEstimateProductEntryList.batchEditApi.GetColumnIndex('Discount');
            //gridEstimateProductEntryList.batchEditApi.StartEdit(globalrowindex, Discount);


            //gridEstimateProductEntryList.GetEditor('Discount').SetText("0.00");

            cProductDiscount.SetText("0.00");
            cProductDiscount.Focus();
        }
    }

    function EstimateGridSetAmt(s, e) {

        // var TypId = gridEstimateProductEntryList.GetEditor("TaxTypeID").GetText();
        //var Price = gridEstimateProductEntryList.batchEditApi.GetCellValue(globalrowindex, 'Price');
        //var Qty = gridEstimateProductEntryList.batchEditApi.GetCellValue(globalrowindex, 'ProductQty');
        //var Discount = gridEstimateProductEntryList.batchEditApi.GetCellValue(globalrowindex, 'Discount');

        var TypId = $("#ddlTaxTypelistProduct").val();
        var Price = cProductPrice.GetText();
        var Qty = cProductQty.GetText();
        var Discount = cProductDiscount.GetText();
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

            //var ret = caluculateAndSetGST(gridEstimateProductEntryList.GetEditor("Amount"), gridEstimateProductEntryList.GetEditor("Charges"), gridEstimateProductEntryList.GetEditor("NetAmount"),
            //        gridEstimateProductEntryList.GetEditor("ProdHSN").GetText(), DecimalRoundoff(amount, 2), DecimalRoundoff(amountAfterDiscount, 2), taxType, "15", $('#ddlBankBranch').val())
            var ret = caluculateAndSetGST(cProductAmount.GetText(), cProductCharges.GetText(), cProductNetAmount.GetText(),
                 $("#hdnProdHSN").val(), DecimalRoundoff(amount, 2), DecimalRoundoff(amountAfterDiscount, 2), taxType, "15", $('#ddlBankBranch').val())

            var data = ret.split('~');


            if (taxType == "O") {
                //var Netamind = gridEstimateProductEntryList.batchEditApi.GetColumnIndex('NetAmount');
                //gridEstimateProductEntryList.batchEditApi.StartEdit(globalrowindex, Netamind);
                //gridEstimateProductEntryList.GetEditor('NetAmount').SetText(Tamount);
                cProductNetAmount.SetText(Tamount);

                //var Charges = gridEstimateProductEntryList.batchEditApi.GetColumnIndex('Charges');
                //gridEstimateProductEntryList.batchEditApi.StartEdit(globalrowindex, Charges);
                //gridEstimateProductEntryList.GetEditor('Charges').SetText("0.00");
                cProductCharges.SetText("0.00");

                //var amind = gridEstimateProductEntryList.batchEditApi.GetColumnIndex('Amount');
                //gridEstimateProductEntryList.batchEditApi.StartEdit(globalrowindex, amind);
                //gridEstimateProductEntryList.GetEditor('Amount').SetText(Tamount);
                cProductAmount.SetText(Tamount);
            }
            else {
                if (TypId == "3" || TypId == "") {
                    //var Netamind = gridEstimateProductEntryList.batchEditApi.GetColumnIndex('NetAmount');
                    //gridEstimateProductEntryList.batchEditApi.StartEdit(globalrowindex, Netamind);
                    //gridEstimateProductEntryList.GetEditor('NetAmount').SetText(Tamount);
                    cProductNetAmount.SetText(Tamount);

                    //var Charges = gridEstimateProductEntryList.batchEditApi.GetColumnIndex('Charges');
                    //gridEstimateProductEntryList.batchEditApi.StartEdit(globalrowindex, Charges);
                    //gridEstimateProductEntryList.GetEditor('Charges').SetText("0.00");
                    cProductCharges.SetText("0.00");

                    //var amind = gridEstimateProductEntryList.batchEditApi.GetColumnIndex('Amount');
                    //gridEstimateProductEntryList.batchEditApi.StartEdit(globalrowindex, amind);
                    //gridEstimateProductEntryList.GetEditor('Amount').SetText(Tamount);
                    cProductAmount.SetText(Tamount);
                }
                else {
                    //var Netamind = gridEstimateProductEntryList.batchEditApi.GetColumnIndex('NetAmount');
                    //gridEstimateProductEntryList.batchEditApi.StartEdit(globalrowindex, Netamind);
                    //gridEstimateProductEntryList.GetEditor('NetAmount').SetText(data[2]);
                    cProductNetAmount.SetText(data[2]);

                    //var Charges = gridEstimateProductEntryList.batchEditApi.GetColumnIndex('Charges');
                    //gridEstimateProductEntryList.batchEditApi.StartEdit(globalrowindex, Charges);
                    //gridEstimateProductEntryList.GetEditor('Charges').SetText(data[1].toString());
                    cProductCharges.SetText(data[1].toString());

                    //var amind = gridEstimateProductEntryList.batchEditApi.GetColumnIndex('Amount');
                    //gridEstimateProductEntryList.batchEditApi.StartEdit(globalrowindex, amind);
                    //gridEstimateProductEntryList.GetEditor('Amount').SetText(data[0]);
                    cProductAmount.SetText(data[0]);
                }
            }
            var TaxType = gridEstimateProductEntryList.batchEditApi.GetColumnIndex('TaxType');
            gridEstimateProductEntryList.batchEditApi.StartEdit(globalrowindex, TaxType);

        }
    }

    function EstimateGridSetAmountQty(s, e) {
        //debugger;
        // gridEstimateProductEntryList.batchEditApi.EndEdit();
        //var Price = s.GetValueString();
        //var TypId = gridEstimateProductEntryList.GetEditor("TaxTypeID").GetText();
        //var Price = gridEstimateProductEntryList.batchEditApi.GetCellValue(globalrowindex, 'Price');
        //var Qty = gridEstimateProductEntryList.batchEditApi.GetCellValue(globalrowindex, 'ProductQty');
        //var Discount = gridEstimateProductEntryList.batchEditApi.GetCellValue(globalrowindex, 'Discount');

        var TypId = $("#ddlTaxTypelistProduct").val();
        var Price = cProductPrice.GetText();
        var Qty = cProductQty.GetText();
        var Discount = cProductDiscount.GetText();
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

            //var ret = caluculateAndSetGST(gridEstimateProductEntryList.GetEditor("Amount"), gridEstimateProductEntryList.GetEditor("Charges"), gridEstimateProductEntryList.GetEditor("NetAmount"),
            //        gridEstimateProductEntryList.GetEditor("ProdHSN").GetText(), DecimalRoundoff(amount, 2), DecimalRoundoff(amountAfterDiscount, 2), taxType, "15", $('#ddlBankBranch').val())
            var ret = caluculateAndSetGST(cProductAmount.GetText(), cProductCharges.GetText(), cProductNetAmount.GetText(),
                 $("#hdnProdHSN").val(), DecimalRoundoff(amount, 2), DecimalRoundoff(amountAfterDiscount, 2), taxType, "15", $('#ddlBankBranch').val())

            var data = ret.split('~');


            if (taxType == "O") {
                //var Netamind = gridEstimateProductEntryList.batchEditApi.GetColumnIndex('NetAmount');
                ////gridEstimateProductEntryList.batchEditApi.StartEdit(globalrowindex, Netamind);
                //gridEstimateProductEntryList.GetEditor('NetAmount').SetText(Tamount);

                cProductNetAmount.SetText(Tamount);

                //var Charges = gridEstimateProductEntryList.batchEditApi.GetColumnIndex('Charges');
                //// gridEstimateProductEntryList.batchEditApi.StartEdit(globalrowindex, Charges);
                //gridEstimateProductEntryList.GetEditor('Charges').SetText("0.00");

                cProductCharges.SetText("0.00");

                //var amind = gridEstimateProductEntryList.batchEditApi.GetColumnIndex('Amount');
                ////gridEstimateProductEntryList.batchEditApi.StartEdit(globalrowindex, amind);
                //gridEstimateProductEntryList.GetEditor('Amount').SetText(Tamount);
                cProductAmount.SetText(Tamount);
            }
            else {
                if (TypId == "3" || TypId == "") {
                    //var Netamind = gridEstimateProductEntryList.batchEditApi.GetColumnIndex('NetAmount');
                    //// gridEstimateProductEntryList.batchEditApi.StartEdit(globalrowindex, Netamind);
                    //gridEstimateProductEntryList.GetEditor('NetAmount').SetText(Tamount);
                    cProductNetAmount.SetText(Tamount);

                    //var Charges = gridEstimateProductEntryList.batchEditApi.GetColumnIndex('Charges');
                    //// gridEstimateProductEntryList.batchEditApi.StartEdit(globalrowindex, Charges);
                    //gridEstimateProductEntryList.GetEditor('Charges').SetText("0.00");
                    cProductCharges.SetText("0.00");

                    //var amind = gridEstimateProductEntryList.batchEditApi.GetColumnIndex('Amount');
                    //// gridEstimateProductEntryList.batchEditApi.StartEdit(globalrowindex, amind);
                    //gridEstimateProductEntryList.GetEditor('Amount').SetText(Tamount);
                    cProductAmount.SetText(Tamount);
                }
                else {
                    //var Netamind = gridEstimateProductEntryList.batchEditApi.GetColumnIndex('NetAmount');
                    ////gridEstimateProductEntryList.batchEditApi.StartEdit(globalrowindex, Netamind);
                    //gridEstimateProductEntryList.GetEditor('NetAmount').SetText(data[2]);
                    cProductNetAmount.SetText(data[2]);

                    //var Charges = gridEstimateProductEntryList.batchEditApi.GetColumnIndex('Charges');
                    //// gridEstimateProductEntryList.batchEditApi.StartEdit(globalrowindex, Charges);
                    //gridEstimateProductEntryList.GetEditor('Charges').SetText(data[1].toString());
                    cProductCharges.SetText(data[1].toString());

                    //var amind = gridEstimateProductEntryList.batchEditApi.GetColumnIndex('Amount');
                    //// gridEstimateProductEntryList.batchEditApi.StartEdit(globalrowindex, amind);
                    //gridEstimateProductEntryList.GetEditor('Amount').SetText(data[0]);
                    cProductAmount.SetText(data[0]);
                }
            }
        }
        // EstimateGridSetTotalAmount("", "");

        gridEstimateProductEntryList.batchEditApi.EndEdit();
        var localindex = gridEstimateProductEntryList.batchEditApi.GetColumnIndex('ProductUOM');

        // gridEstimateProductEntryList.batchEditApi.StartEdit(globalrowindex, localindex);
        //else {
        //    gridEstimateProductEntryList.batchEditApi.StartEdit(globalrowindex, 7);
        //}
        //gridEstimateProductEntryList.batchEditApi.EndEdit();
        //gridEstimateProductEntryList.batchEditApi.StartEdit();
        //EstimateGridSetTotalAmount(s, e);

    }

    function EstimateGridSetAmountQty() {
        //var TypId = gridEstimateProductEntryList.GetEditor("TaxTypeID").GetText();
        //var Price = gridEstimateProductEntryList.batchEditApi.GetCellValue(globalrowindex, 'Price');
        //var Qty = gridEstimateProductEntryList.batchEditApi.GetCellValue(globalrowindex, 'ProductQty');
        //var Discount = gridEstimateProductEntryList.batchEditApi.GetCellValue(globalrowindex, 'Discount');

        var TypId = $("#ddlTaxTypelistProduct").val();
        var Price = cProductPrice.GetText();
        var Qty = cProductQty.GetText();
        var Discount = cProductDiscount.GetText();
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

            //var ret = caluculateAndSetGST(gridEstimateProductEntryList.GetEditor("Amount"), gridEstimateProductEntryList.GetEditor("Charges"), gridEstimateProductEntryList.GetEditor("NetAmount"),
            //        gridEstimateProductEntryList.GetEditor("ProdHSN").GetText(), DecimalRoundoff(amount, 2), DecimalRoundoff(amountAfterDiscount, 2), taxType, "15", $('#ddlBankBranch').val())

            var ret = caluculateAndSetGST(cProductAmount.GetText(), cProductCharges.GetText(), cProductNetAmount.GetText(),
                  $("#hdnProdHSN").val(), DecimalRoundoff(amount, 2), DecimalRoundoff(amountAfterDiscount, 2), taxType, "15", $('#ddlBankBranch').val())

            var data = ret.split('~');


            if (taxType == "O") {
                //var Netamind = gridEstimateProductEntryList.batchEditApi.GetColumnIndex('NetAmount');
                ////gridEstimateProductEntryList.batchEditApi.StartEdit(globalrowindex, Netamind);
                //gridEstimateProductEntryList.GetEditor('NetAmount').SetText(Tamount);
                cProductNetAmount.SetText(Tamount);

                //var Charges = gridEstimateProductEntryList.batchEditApi.GetColumnIndex('Charges');
                //// gridEstimateProductEntryList.batchEditApi.StartEdit(globalrowindex, Charges);
                //gridEstimateProductEntryList.GetEditor('Charges').SetText("0.00");
                cProductCharges.SetText("0.00");

                //var amind = gridEstimateProductEntryList.batchEditApi.GetColumnIndex('Amount');
                ////gridEstimateProductEntryList.batchEditApi.StartEdit(globalrowindex, amind);
                //gridEstimateProductEntryList.GetEditor('Amount').SetText(Tamount);
                cProductAmount.SetText(Tamount);
            }
            else {
                if (TypId == "3" || TypId == "") {
                    //var Netamind = gridEstimateProductEntryList.batchEditApi.GetColumnIndex('NetAmount');
                    //// gridEstimateProductEntryList.batchEditApi.StartEdit(globalrowindex, Netamind);
                    //gridEstimateProductEntryList.GetEditor('NetAmount').SetText(Tamount);
                    cProductNetAmount.SetText(Tamount);

                    //var Charges = gridEstimateProductEntryList.batchEditApi.GetColumnIndex('Charges');
                    //// gridEstimateProductEntryList.batchEditApi.StartEdit(globalrowindex, Charges);
                    //gridEstimateProductEntryList.GetEditor('Charges').SetText("0.00");
                    cProductCharges.SetText("0.00");

                    //var amind = gridEstimateProductEntryList.batchEditApi.GetColumnIndex('Amount');
                    //// gridEstimateProductEntryList.batchEditApi.StartEdit(globalrowindex, amind);
                    //gridEstimateProductEntryList.GetEditor('Amount').SetText(Tamount);
                    cProductAmount.SetText(Tamount);
                }
                else {
                    //var Netamind = gridEstimateProductEntryList.batchEditApi.GetColumnIndex('NetAmount');
                    ////gridEstimateProductEntryList.batchEditApi.StartEdit(globalrowindex, Netamind);
                    //gridEstimateProductEntryList.GetEditor('NetAmount').SetText(data[2]);
                    cProductNetAmount.SetText(data[2]);

                    //var Charges = gridEstimateProductEntryList.batchEditApi.GetColumnIndex('Charges');
                    //// gridEstimateProductEntryList.batchEditApi.StartEdit(globalrowindex, Charges);
                    //gridEstimateProductEntryList.GetEditor('Charges').SetText(data[1].toString());
                    cProductCharges.SetText(data[1].toString());

                    //var amind = gridEstimateProductEntryList.batchEditApi.GetColumnIndex('Amount');
                    //// gridEstimateProductEntryList.batchEditApi.StartEdit(globalrowindex, amind);
                    //gridEstimateProductEntryList.GetEditor('Amount').SetText(data[0]);
                    cProductAmount.SetText(data[0]);
                }
            }
        }
        // EstimateGridSetTotalAmount("", "");

        //gridEstimateProductEntryList.batchEditApi.EndEdit();
        //var localindex = gridEstimateProductEntryList.batchEditApi.GetColumnIndex('ProductUOM');
    }

    //Resource

    function EstimateResourceGridSetAmount(s, e) {
        //  gridEstimateResourcesList.batchEditApi.EndEdit();
        //var Price = s.GetValueString();
        //debugger;
        //var TypId = gridEstimateResourcesList.GetEditor("TaxTypeID").GetText();
        //var Price = gridEstimateResourcesList.batchEditApi.GetCellValue(globalrowindex2, 'Price');
        //var Qty = gridEstimateResourcesList.batchEditApi.GetCellValue(globalrowindex2, 'ProductQty');
        //var Discount = gridEstimateResourcesList.batchEditApi.GetCellValue(globalrowindex2, 'Discount');

        var TypId = $("#ddlTaxTypelistResource").val();
        var Price = cResProductPrice.GetText();
        var Qty = cResProductQty.GetText();
        var Discount = cResProductDiscount.GetText();
        if (Price != "" && Qty != "") {
            var amount = parseFloat((parseFloat(Qty).toFixed(4)) * (parseFloat(Price).toFixed(2))).toFixed(2);

            var amountAfterDiscount = parseFloat(amount) - ((parseFloat(Discount) * parseFloat(amount)) / 100);
            var Tamount = parseFloat(DecimalRoundoff(amountAfterDiscount, 2)).toFixed(2);

            //gridEstimateResourcesList.batchEditApi.StartEdit(globalrowindex2, 12);
            //gridEstimateResourcesList.GetEditor('NetAmount').SetText(Tamount);

            //gridEstimateResourcesList.batchEditApi.StartEdit(globalrowindex2, 9);
            //gridEstimateResourcesList.GetEditor('Amount').SetText(Tamount);

            var taxType = "O";
            var amtFor = $("#ddl_AmountAre").val();
            if (amtFor == "2") {
                taxType = "I";
            }
            else if (amtFor == "1") {
                taxType = "E";
            }

            //var ret = caluculateAndSetGST(gridEstimateResourcesList.GetEditor("Amount"), gridEstimateResourcesList.GetEditor("ResourceCharges"), gridEstimateResourcesList.GetEditor("NetAmount"),
            //        gridEstimateResourcesList.GetEditor("ProdHSN").GetText(), DecimalRoundoff(amount, 2), DecimalRoundoff(amountAfterDiscount, 2), taxType, "15", $('#ddlBankBranch').val())
            var ret = caluculateAndSetGST(cResProductAmount.GetText(), cResProductCharges.GetText(), cResProductNetAmount.GetText(),
                                             $("#hdnResHSN").val(), DecimalRoundoff(amount, 2), DecimalRoundoff(amountAfterDiscount, 2), taxType, "15", $('#ddlBankBranch').val())
            var data = ret.split('~');


            if (taxType == "O") {
                //var Netamind = gridEstimateResourcesList.batchEditApi.GetColumnIndex('NetAmount');
                //gridEstimateResourcesList.batchEditApi.StartEdit(globalrowindex2, Netamind);
                //gridEstimateResourcesList.GetEditor('NetAmount').SetText(Tamount);
                cResProductNetAmount.SetText(Tamount);

                //var Charges = gridEstimateResourcesList.batchEditApi.GetColumnIndex('ResourceCharges');
                //gridEstimateResourcesList.batchEditApi.StartEdit(globalrowindex2, Charges);
                //gridEstimateResourcesList.GetEditor('ResourceCharges').SetText("0.00");
                cResProductCharges.SetText("0.00");

                //var amind = gridEstimateResourcesList.batchEditApi.GetColumnIndex('Amount');
                //gridEstimateResourcesList.batchEditApi.StartEdit(globalrowindex2, amind);
                //gridEstimateResourcesList.GetEditor('Amount').SetText(Tamount);
                cResProductAmount.SetText(Tamount);
            }
            else {
                if (TypId == "3" || TypId == "") {
                    //var Netamind = gridEstimateResourcesList.batchEditApi.GetColumnIndex('NetAmount');
                    //gridEstimateResourcesList.batchEditApi.StartEdit(globalrowindex2, Netamind);
                    //gridEstimateResourcesList.GetEditor('NetAmount').SetText(Tamount);
                    cResProductNetAmount.SetText(Tamount);

                    //var Charges = gridEstimateResourcesList.batchEditApi.GetColumnIndex('ResourceCharges');
                    //gridEstimateResourcesList.batchEditApi.StartEdit(globalrowindex2, Charges);
                    //gridEstimateResourcesList.GetEditor('ResourceCharges').SetText("0.00");
                    cResProductCharges.SetText("0.00");

                    //var amind = gridEstimateResourcesList.batchEditApi.GetColumnIndex('Amount');
                    //gridEstimateResourcesList.batchEditApi.StartEdit(globalrowindex2, amind);
                    //gridEstimateResourcesList.GetEditor('Amount').SetText(Tamount);
                    cResProductAmount.SetText(Tamount);
                }
                else {
                    //var Netamind = gridEstimateResourcesList.batchEditApi.GetColumnIndex('NetAmount');
                    //gridEstimateResourcesList.batchEditApi.StartEdit(globalrowindex2, Netamind);
                    //gridEstimateResourcesList.GetEditor('NetAmount').SetText(data[2]);
                    cResProductNetAmount.SetText(Tamount);

                    //var Charges = gridEstimateResourcesList.batchEditApi.GetColumnIndex('ResourceCharges');
                    //gridEstimateResourcesList.batchEditApi.StartEdit(globalrowindex2, Charges);
                    //gridEstimateResourcesList.GetEditor('ResourceCharges').SetText(data[1].toString());
                    cResProductCharges.SetText(data[1].toString());

                    //var amind = gridEstimateResourcesList.batchEditApi.GetColumnIndex('Amount');
                    //gridEstimateResourcesList.batchEditApi.StartEdit(globalrowindex2, amind);
                    //gridEstimateResourcesList.GetEditor('Amount').SetText(data[0]);
                    cResProductAmount.SetText(data[0]);
                }
            }
        }
        //var Discount = gridEstimateResourcesList.batchEditApi.GetColumnIndex('Discount');
        //gridEstimateResourcesList.batchEditApi.StartEdit(globalrowindex2, Discount);
        //EstimateGridResourceSetTotalAmount("", "");
    }

    function EstimateResourceDiscount(s, e) {
        // gridEstimateResourcesList.batchEditApi.EndEdit();
        //var Price = s.GetValueString();
        //debugger;
        //var TypId = gridEstimateResourcesList.GetEditor("TaxTypeID").GetText();
        //var Price = gridEstimateResourcesList.batchEditApi.GetCellValue(globalrowindex2, 'Price');
        //var Qty = gridEstimateResourcesList.batchEditApi.GetCellValue(globalrowindex2, 'ProductQty');
        //var Discount = gridEstimateResourcesList.batchEditApi.GetCellValue(globalrowindex2, 'Discount');
        var TypId = $("#ddlTaxTypelistResource").val();
        var Price = cResProductPrice.GetText();
        var Qty = cResProductQty.GetText();
        var Discount = cResProductDiscount.GetText();
        if (Price != "" && Qty != "") {
            var amount = parseFloat((parseFloat(Qty).toFixed(4)) * (parseFloat(Price).toFixed(2))).toFixed(2);

            var amountAfterDiscount = parseFloat(amount) - ((parseFloat(Discount) * parseFloat(amount)) / 100);
            var Tamount = parseFloat(DecimalRoundoff(amountAfterDiscount, 2)).toFixed(2);

            //gridEstimateResourcesList.batchEditApi.StartEdit(globalrowindex2, 12);
            //gridEstimateResourcesList.GetEditor('NetAmount').SetText(Tamount);

            //gridEstimateResourcesList.batchEditApi.StartEdit(globalrowindex2, 9);
            //gridEstimateResourcesList.GetEditor('Amount').SetText(Tamount);

            var taxType = "O";
            var amtFor = $("#ddl_AmountAre").val();
            if (amtFor == "2") {
                taxType = "I";
            }
            else if (amtFor == "1") {
                taxType = "E";
            }

            //var ret = caluculateAndSetGST(gridEstimateResourcesList.GetEditor("Amount"), gridEstimateResourcesList.GetEditor("ResourceCharges"), gridEstimateResourcesList.GetEditor("NetAmount"),
            //        gridEstimateResourcesList.GetEditor("ProdHSN").GetText(), DecimalRoundoff(amount, 2), DecimalRoundoff(amountAfterDiscount, 2), taxType, "15", $('#ddlBankBranch').val())
            var ret = caluculateAndSetGST(cResProductAmount.GetText(), cResProductCharges.GetText(), cResProductNetAmount.GetText(),
                                        $("#hdnResHSN").val(), DecimalRoundoff(amount, 2), DecimalRoundoff(amountAfterDiscount, 2), taxType, "15", $('#ddlBankBranch').val())
            var data = ret.split('~');


            if (taxType == "O") {
                //var Netamind = gridEstimateResourcesList.batchEditApi.GetColumnIndex('NetAmount');
                //gridEstimateResourcesList.batchEditApi.StartEdit(globalrowindex2, Netamind);
                //gridEstimateResourcesList.GetEditor('NetAmount').SetText(Tamount);
                cResProductNetAmount.SetText(Tamount);

                //var Charges = gridEstimateResourcesList.batchEditApi.GetColumnIndex('ResourceCharges');
                //gridEstimateResourcesList.batchEditApi.StartEdit(globalrowindex2, Charges);
                //gridEstimateResourcesList.GetEditor('ResourceCharges').SetText("0.00");
                cResProductCharges.SetText("0.00");

                //var amind = gridEstimateResourcesList.batchEditApi.GetColumnIndex('Amount');
                //gridEstimateResourcesList.batchEditApi.StartEdit(globalrowindex2, amind);
                //gridEstimateResourcesList.GetEditor('Amount').SetText(Tamount);
                cResProductAmount.SetText(Tamount);
            }
            else {
                if (TypId == "3" || TypId == "") {
                    //var Netamind = gridEstimateResourcesList.batchEditApi.GetColumnIndex('NetAmount');
                    //gridEstimateResourcesList.batchEditApi.StartEdit(globalrowindex2, Netamind);
                    //gridEstimateResourcesList.GetEditor('NetAmount').SetText(Tamount);
                    cResProductNetAmount.SetText(Tamount);

                    //var Charges = gridEstimateResourcesList.batchEditApi.GetColumnIndex('ResourceCharges');
                    //gridEstimateResourcesList.batchEditApi.StartEdit(globalrowindex2, Charges);
                    //gridEstimateResourcesList.GetEditor('ResourceCharges').SetText("0.00");
                    cResProductCharges.SetText("0.00");

                    //var amind = gridEstimateResourcesList.batchEditApi.GetColumnIndex('Amount');
                    //gridEstimateResourcesList.batchEditApi.StartEdit(globalrowindex2, amind);
                    //gridEstimateResourcesList.GetEditor('Amount').SetText(Tamount);
                    cResProductAmount.SetText(Tamount);
                }
                else {
                    //var Netamind = gridEstimateResourcesList.batchEditApi.GetColumnIndex('NetAmount');
                    //gridEstimateResourcesList.batchEditApi.StartEdit(globalrowindex2, Netamind);
                    //gridEstimateResourcesList.GetEditor('NetAmount').SetText(data[2]);
                    cResProductNetAmount.SetText(data[2]);

                    //var Charges = gridEstimateResourcesList.batchEditApi.GetColumnIndex('ResourceCharges');
                    //gridEstimateResourcesList.batchEditApi.StartEdit(globalrowindex2, Charges);
                    //gridEstimateResourcesList.GetEditor('ResourceCharges').SetText(data[1].toString());
                    cResProductCharges.SetText(data[1].toString());

                    //var amind = gridEstimateResourcesList.batchEditApi.GetColumnIndex('Amount');
                    //gridEstimateResourcesList.batchEditApi.StartEdit(globalrowindex2, amind);
                    //gridEstimateResourcesList.GetEditor('Amount').SetText(data[0]);
                    cResProductAmount.SetText(data[0]);
                }
            }
        }
        //gridEstimateResourcesList.batchEditApi.EndEdit();
        // EstimateGridResourceSetTotalAmount("", "");
    }

    function EstimateResourceGridUOMFocus(s, e) {

        if (e.htmlEvent.key == "Tab" && e.htmlEvent.shiftKey) {
            gridEstimateResourcesList.batchEditApi.EndEdit();
            var localindex = gridEstimateResourcesList.batchEditApi.GetColumnIndex('ProductQty');

            gridEstimateResourcesList.batchEditApi.StartEdit(globalrowindex2, localindex);
        }
    }

    function EstimateResourceGridSetQtyKeyDown(s, e) {

        //if (e.htmlEvent.key == "Tab" && e.htmlEvent.shiftKey) {
        //    gridEstimateResourcesList.batchEditApi.EndEdit();
        //    // var localindex = gridEstimateResourcesList.batchEditApi.GetColumnIndex('ProductQty');
        //    gridEstimateResourcesList.batchEditApi.StartEdit(globalrowindex2, ResAddlDescIndex);
        //}
    }

    function ProductQtyKeyDown(s, e) {

        //if (e.htmlEvent.key == "Tab" && e.htmlEvent.shiftKey) {
        //    // gridEstimateProductEntryList.batchEditApi.EndEdit();
        //    // var localindex = gridEstimateResourcesList.batchEditApi.GetColumnIndex('ProductQty');
        //    // gridEstimateProductEntryList.batchEditApi.StartEdit(globalrowindex, PrdAddlDescIndex);
        //}
    }

    function DiscountResourceKeyDown(s, e) {
        //if (e.htmlEvent.key == "Tab" && e.htmlEvent.shiftKey) {
        //    gridEstimateResourcesList.batchEditApi.EndEdit();
        //    var localindex = gridEstimateResourcesList.batchEditApi.GetColumnIndex('Amount');
        //   // gridEstimateResourcesList.batchEditApi.StartEdit(globalrowindex2, localindex);
        //    gridEstimateResourcesList.batchEditApi.EndEdit(e.visibleIndex, ResPriceIndex);
        //}
    }

    function EstimateResourceGridSetAmountQty(s, e) {
        //gridEstimateResourcesList.batchEditApi.EndEdit();
        //debugger;
        //var TypId = gridEstimateResourcesList.GetEditor("TaxTypeID").GetText();
        //var Price = gridEstimateResourcesList.batchEditApi.GetCellValue(globalrowindex2, 'Price');
        //var Qty = gridEstimateResourcesList.batchEditApi.GetCellValue(globalrowindex2, 'ProductQty');
        //var Discount = gridEstimateResourcesList.batchEditApi.GetCellValue(globalrowindex2, 'Discount');

        var TypId = $("#ddlTaxTypelistResource").val();
        var Price = cResProductPrice.GetText();
        var Qty = cResProductQty.GetText();
        var Discount = cResProductDiscount.GetText();

        if (Price != "" && Qty != "") {
            var amount = parseFloat((parseFloat(Qty).toFixed(4)) * (parseFloat(Price).toFixed(2))).toFixed(2);

            var amountAfterDiscount = parseFloat(amount) - ((parseFloat(Discount) * parseFloat(amount)) / 100);
            var Tamount = parseFloat(DecimalRoundoff(amountAfterDiscount, 2)).toFixed(2);

            //gridEstimateResourcesList.batchEditApi.StartEdit(globalrowindex2, 12);
            //gridEstimateResourcesList.GetEditor('NetAmount').SetText(Tamount);
            //gridEstimateResourcesList.batchEditApi.StartEdit(globalrowindex2, 9);
            //gridEstimateResourcesList.GetEditor('Amount').SetText(Tamount);

            var taxType = "O";
            var amtFor = $("#ddl_AmountAre").val();
            if (amtFor == "2") {
                taxType = "I";
            }
            else if (amtFor == "1") {
                taxType = "E";
            }

            //var ret = caluculateAndSetGST(gridEstimateResourcesList.GetEditor("Amount"), gridEstimateResourcesList.GetEditor("ResourceCharges"), gridEstimateResourcesList.GetEditor("NetAmount"),
            //        gridEstimateResourcesList.GetEditor("ProdHSN").GetText(), DecimalRoundoff(amount, 2), DecimalRoundoff(amountAfterDiscount, 2), taxType, "15", $('#ddlBankBranch').val())
            var ret = caluculateAndSetGST(cResProductAmount.GetText(), cResProductCharges.GetText(), cResProductNetAmount.GetText(),
                                            $("#hdnResHSN").val(), DecimalRoundoff(amount, 2), DecimalRoundoff(amountAfterDiscount, 2), taxType, "15", $('#ddlBankBranch').val())
            var data = ret.split('~');


            if (taxType == "O") {
                //var Netamind = gridEstimateResourcesList.batchEditApi.GetColumnIndex('NetAmount');
                //gridEstimateResourcesList.batchEditApi.StartEdit(globalrowindex2, Netamind);
                //gridEstimateResourcesList.GetEditor('NetAmount').SetText(Tamount);
                cResProductNetAmount.SetText(Tamount);

                //var Charges = gridEstimateResourcesList.batchEditApi.GetColumnIndex('ResourceCharges');
                //gridEstimateResourcesList.batchEditApi.StartEdit(globalrowindex2, Charges);
                //gridEstimateResourcesList.GetEditor('ResourceCharges').SetText("0.00");
                cResProductCharges.SetText("0.00");

                //var amind = gridEstimateResourcesList.batchEditApi.GetColumnIndex('Amount');
                //gridEstimateResourcesList.batchEditApi.StartEdit(globalrowindex2, amind);
                //gridEstimateResourcesList.GetEditor('Amount').SetText(Tamount);
                cResProductAmount.SetText(Tamount);
            }
            else {
                if (TypId == "3" || TypId == "") {
                    //var Netamind = gridEstimateResourcesList.batchEditApi.GetColumnIndex('NetAmount');
                    //gridEstimateResourcesList.batchEditApi.StartEdit(globalrowindex2, Netamind);
                    //gridEstimateResourcesList.GetEditor('NetAmount').SetText(Tamount);
                    cResProductNetAmount.SetText(Tamount);

                    //var Charges = gridEstimateResourcesList.batchEditApi.GetColumnIndex('ResourceCharges');
                    //gridEstimateResourcesList.batchEditApi.StartEdit(globalrowindex2, Charges);
                    //gridEstimateResourcesList.GetEditor('ResourceCharges').SetText("0.00");
                    cResProductCharges.SetText("0.00");

                    //var amind = gridEstimateResourcesList.batchEditApi.GetColumnIndex('Amount');
                    //gridEstimateResourcesList.batchEditApi.StartEdit(globalrowindex2, amind);
                    //gridEstimateResourcesList.GetEditor('Amount').SetText(Tamount);
                    cResProductAmount.SetText(Tamount);
                }
                else {
                    //var Netamind = gridEstimateResourcesList.batchEditApi.GetColumnIndex('NetAmount');
                    //gridEstimateResourcesList.batchEditApi.StartEdit(globalrowindex2, Netamind);
                    //gridEstimateResourcesList.GetEditor('NetAmount').SetText(data[2]);
                    cResProductNetAmount.SetText(data[2]);

                    //var Charges = gridEstimateResourcesList.batchEditApi.GetColumnIndex('ResourceCharges');
                    //gridEstimateResourcesList.batchEditApi.StartEdit(globalrowindex2, Charges);
                    //gridEstimateResourcesList.GetEditor('ResourceCharges').SetText(data[1].toString());
                    cResProductCharges.SetText(data[1].toString());

                    //var amind = gridEstimateResourcesList.batchEditApi.GetColumnIndex('Amount');
                    //gridEstimateResourcesList.batchEditApi.StartEdit(globalrowindex2, amind);
                    //gridEstimateResourcesList.GetEditor('Amount').SetText(data[0]);
                    cResProductAmount.SetText(data[0]);
                }
            }
            //gridEstimateResourcesList.batchEditApi.EndEdit();
        }
        //var localindex = gridEstimateResourcesList.batchEditApi.GetColumnIndex('ProductUOM');
        //gridEstimateResourcesList.batchEditApi.StartEdit(globalrowindex2, localindex);
    }


    function SetRescTaxTypeInGrid() {

        //var TaxTypeid = $('#ddlRescTaxTypelist').val();
        var TaxTypeid = $("#ddlTaxTypelistResource").val();
        var TaxTypetxt = $('#ddlRescTaxTypelist option:selected').text();
        if (TaxTypetxt != "") {
            //gridEstimateResourcesList.batchEditApi.StartEdit(globalrowindex2, ResTaxTypeIndex);
            //gridEstimateResourcesList.GetEditor('TaxType').SetText(TaxTypetxt);

            //gridEstimateResourcesList.batchEditApi.StartEdit(globalrowindex2, ResTaxTypeIDIndex);
            //gridEstimateResourcesList.GetEditor('TaxTypeID').SetText(TaxTypeid);
            $('#RescTaxTypelistModel').modal('hide');



            //var Price = gridEstimateResourcesList.batchEditApi.GetCellValue(globalrowindex2, 'Price');
            //var Qty = gridEstimateResourcesList.batchEditApi.GetCellValue(globalrowindex2, 'ProductQty');
            //var Discount = gridEstimateResourcesList.batchEditApi.GetCellValue(globalrowindex2, 'Discount');

            var Price = cResProductPrice.GetText();
            var Qty = cResProductQty.GetText();
            var Discount = cResProductDiscount.GetText();

            if (Price != "" && Qty != "") {
                var amount = parseFloat((parseFloat(Qty).toFixed(4)) * (parseFloat(Price).toFixed(2))).toFixed(2);
                var amountAfterDiscount = parseFloat(amount) - ((parseFloat(Discount) * parseFloat(amount)) / 100);
                var Tamount = parseFloat(DecimalRoundoff(amountAfterDiscount, 2)).toFixed(2);

                if (TaxTypeid == "3" || TaxTypeid == "") {
                    //gridEstimateResourcesList.batchEditApi.StartEdit(globalrowindex2, ResAmountIndex);
                    //gridEstimateResourcesList.GetEditor('Amount').SetText(Tamount);
                    cResProductAmount.SetText(Tamount);

                    //gridEstimateResourcesList.batchEditApi.StartEdit(globalrowindex2, ResNetAmountIndex);
                    //gridEstimateResourcesList.GetEditor('NetAmount').SetText(Tamount);
                    cResProductNetAmount.SetText(Tamount);

                    ////if (gridEstimateProductEntryList.batchEditApi.GetCellValue(globalrowindex, 11) == null) {
                    //gridEstimateResourcesList.batchEditApi.StartEdit(globalrowindex2, ResResourceChargesIndex);
                    //gridEstimateResourcesList.GetEditor('ResourceCharges').SetText("0.00");
                    ////  }
                    cResProductCharges.SetText("0.00");
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

                    //var ret = caluculateAndSetGST(gridEstimateResourcesList.GetEditor("Amount"), gridEstimateResourcesList.GetEditor("ResourceCharges"), gridEstimateResourcesList.GetEditor("NetAmount"),
                    //        gridEstimateResourcesList.GetEditor("ProdHSN").GetText(), DecimalRoundoff(amount, 2), DecimalRoundoff(amountAfterDiscount, 2), taxType, "15", $('#ddlBankBranch').val())
                    var ret = caluculateAndSetGST(cResProductAmount.GetText(), cResProductCharges.GetText(), cResProductNetAmount.GetText(), $("#hdnResHSN").val(),
                                                    DecimalRoundoff(amount, 2), DecimalRoundoff(amountAfterDiscount, 2), taxType, "15", $('#ddlBankBranch').val())
                    var data = ret.split('~');

                    //var Netamind = gridEstimateResourcesList.batchEditApi.GetColumnIndex('NetAmount');
                    //gridEstimateResourcesList.batchEditApi.StartEdit(globalrowindex2, Netamind);
                    //gridEstimateResourcesList.GetEditor('NetAmount').SetText(data[2]);
                    cResProductNetAmount.SetText(data[2]);

                    //var Charges = gridEstimateResourcesList.batchEditApi.GetColumnIndex('ResourceCharges');
                    //gridEstimateResourcesList.batchEditApi.StartEdit(globalrowindex2, Charges);
                    //gridEstimateResourcesList.GetEditor('ResourceCharges').SetText(data[1].toString());
                    cResProductCharges.SetText(data[1].toString());

                    //var amind = gridEstimateResourcesList.batchEditApi.GetColumnIndex('Amount');
                    //gridEstimateResourcesList.batchEditApi.StartEdit(globalrowindex2, amind);
                    //gridEstimateResourcesList.GetEditor('Amount').SetText(data[0]);
                    cResProductAmount.SetText(data[0]);

                }
            }
            //gridEstimateResourcesList.batchEditApi.StartEdit(globalrowindex2, ResTaxTypeIndex);
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
            //var TypId = gridEstimateResourcesList.GetEditor("TaxTypeID").GetText();

            //var Price = gridEstimateResourcesList.batchEditApi.GetCellValue(globalrowindex2, 'Price');
            //var Qty = gridEstimateResourcesList.batchEditApi.GetCellValue(globalrowindex2, 'ProductQty');
            //var Discount = gridEstimateResourcesList.batchEditApi.GetCellValue(globalrowindex2, 'Discount');
            var TypId = $("#ddlTaxTypelistResource").val();
            var Price = cResProductPrice.GetText();
            var Qty = cResProductQty.GetText();
            var Discount = cResProductDiscount.GetText();

            if (Price != "" && Qty != "") {
                var amount = parseFloat((parseFloat(Qty).toFixed(4)) * (parseFloat(Price).toFixed(2))).toFixed(2);
                var amountAfterDiscount = parseFloat(amount) - ((parseFloat(Discount) * parseFloat(amount)) / 100);
                var Tamount = parseFloat(DecimalRoundoff(amountAfterDiscount, 2)).toFixed(2);

                if (TypId == "3" || TypId == "") {
                    //gridEstimateResourcesList.batchEditApi.StartEdit(globalrowindex2, ResAmountIndex);
                    //gridEstimateResourcesList.GetEditor('Amount').SetText(Tamount);
                    cResProductAmount.SetText(Tamount);

                    //gridEstimateResourcesList.batchEditApi.StartEdit(globalrowindex2, ResNetAmountIndex);
                    //gridEstimateResourcesList.GetEditor('NetAmount').SetText(Tamount);
                    cResProductNetAmount.SetText(Tamount);

                    ////if (gridEstimateProductEntryList.batchEditApi.GetCellValue(globalrowindex, 11) == null) {
                    //gridEstimateResourcesList.batchEditApi.StartEdit(globalrowindex2, ResResourceChargesIndex);
                    //gridEstimateResourcesList.GetEditor('ResourceCharges').SetText("0.00");
                    ////  }
                    cResProductCharges.SetText("0.00");
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

                    //var ret = caluculateAndSetGST(gridEstimateResourcesList.GetEditor("Amount"), gridEstimateResourcesList.GetEditor("ResourceCharges"), gridEstimateResourcesList.GetEditor("NetAmount"),
                    //        gridEstimateResourcesList.GetEditor("ProdHSN").GetText(), DecimalRoundoff(amount, 2), DecimalRoundoff(amountAfterDiscount, 2), taxType, "15", $('#ddlBankBranch').val())
                    var ret = caluculateAndSetGST(cResProductAmount.GetText(), cResProductCharges.GetText(), cResProductNetAmount.GetText(),
                                                    $("#hdnResHSN").val(), DecimalRoundoff(amount, 2), DecimalRoundoff(amountAfterDiscount, 2), taxType, "15", $('#ddlBankBranch').val())
                    var data = ret.split('~');

                    //var Netamind = gridEstimateResourcesList.batchEditApi.GetColumnIndex('NetAmount');
                    //gridEstimateResourcesList.batchEditApi.StartEdit(globalrowindex2, Netamind);
                    //gridEstimateResourcesList.GetEditor('NetAmount').SetText(data[2]);
                    cResProductNetAmount.SetText(data[2]);

                    //var amind = gridEstimateResourcesList.batchEditApi.GetColumnIndex('Amount');
                    //gridEstimateResourcesList.batchEditApi.StartEdit(globalrowindex2, amind);
                    //gridEstimateResourcesList.GetEditor('Amount').SetText(data[0]);
                    cResProductAmount.SetText(data[0]);

                    //var Charges = gridEstimateResourcesList.batchEditApi.GetColumnIndex('ResourceCharges');
                    //gridEstimateResourcesList.batchEditApi.StartEdit(globalrowindex2, Charges);
                    //gridEstimateResourcesList.GetEditor('ResourceCharges').SetText(data[1].toString());
                    cResProductCharges.SetText(data[1].toString());

                }
            }
        }
    }

    //function FocusGrid() {
    //    gridEstimateProductEntryList.batchEditApi.StartEdit(-1,0);
    //}

    //function RemarksLostFocus(s, e) {
    //    //gridEstimateProductEntryList.batchEditApi.StartEdit(globalrowindex, 13);
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
            gridEstimateProductEntryList.batchEditApi.EndEdit();
            WarehouseGotFocus();
        }
    }

    function DiscountKeyDown(s, e) {

        console.log(e.htmlEvent.key);
        if (e.htmlEvent.key == "Tab" && e.htmlEvent.shiftKey) {
            gridEstimateProductEntryList.batchEditApi.EndEdit();
            var localindex = gridEstimateProductEntryList.batchEditApi.GetColumnIndex('Amount');
            gridEstimateProductEntryList.batchEditApi.StartEdit(globalrowindex, localindex);
        }
    }

    function WarehouseGotFocus() {

        var localindex = gridEstimateProductEntryList.batchEditApi.GetColumnIndex('Charges');

        gridEstimateProductEntryList.batchEditApi.StartEdit(globalrowindex, localindex);

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
                if (gridEstimateProductEntryList.batchEditApi.GetCellValue(tempindex, 'SlNO') != null && globalrowindex >= 0 && tempindexcount.includes(tempindex) == false) {
                    tempindexcount.push(tempindex);
                    gridEstimateProductEntryList.batchEditApi.EndEdit();
                    setTimeout(function () {
                        var localindex = gridEstimateResourcesList.batchEditApi.GetColumnIndex('ProductName');
                        gridEstimateProductEntryList.batchEditApi.StartEdit(tempindex, localindex);
                    }, 500);

                    hasfoundindex = 1;
                }
                else {
                    var tempindex = -1;
                    if (gridEstimateProductEntryList.batchEditApi.GetCellValue(tempindex, 'SlNO') != null && tempindexcount.includes(tempindex) == false) {
                        tempindexcount.push(tempindex);
                        gridEstimateProductEntryList.batchEditApi.EndEdit();
                        setTimeout(function () {
                            var localindex = gridEstimateResourcesList.batchEditApi.GetColumnIndex('ProductName');
                            gridEstimateProductEntryList.batchEditApi.StartEdit(tempindex, localindex);
                        }, 500);


                    }
                }

            }
            else {


                var tempindex = (globalrowindex - 1);
                if (gridEstimateProductEntryList.batchEditApi.GetCellValue(tempindex, 'SlNO') != null) {
                    gridEstimateProductEntryList.batchEditApi.EndEdit();

                    setTimeout(function () {
                        var localindex = gridEstimateResourcesList.batchEditApi.GetColumnIndex('ProductName');
                        gridEstimateProductEntryList.batchEditApi.StartEdit(tempindex, localindex);
                    }, 500);

                }
                else {
                    var tempindex = (globalrowindex - 1);
                    if (gridEstimateProductEntryList.batchEditApi.GetCellValue(tempindex, 'SlNO') != null) {

                        gridEstimateProductEntryList.batchEditApi.EndEdit();
                        setTimeout(function () {
                            var localindex = gridEstimateResourcesList.batchEditApi.GetColumnIndex('ProductName');
                            gridEstimateProductEntryList.batchEditApi.StartEdit(tempindex, localindex);
                        }, 500);
                    }
                }
            }
        }
    }

    function EstimateKeyDown(s, e) {
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

            //if (gridEstimateProductEntryList.focusedRowIndex != null && gridEstimateProductEntryList.focusedRowIndex != undefined) {
            //    globalrowindex = gridEstimateProductEntryList.focusedRowIndex;
            //}
            s.OnButtonClick(0);
            //OpenProductList(s, e);
        }
            //if (e.htmlEvent.key == "Tab") {
            //    //if (gridEstimateProductEntryList.focusedRowIndex != null && gridEstimateProductEntryList.focusedRowIndex != undefined) {
            //    //    globalrowindex = gridEstimateProductEntryList.focusedRowIndex;
            //    //}
            //    s.OnButtonClick(0);
            //    //OpenProductList(s, e);
            //}
        else if (e.code == "ArrowDown") {
            if ($("input[GridProductIndex=0]"))
                $("input[GridProductIndex=0]").focus();
        }
    }

    function SellableProductKeyDown(s, e) {
        console.log(e.htmlEvent.key);
        if (e.htmlEvent.key == "Enter") {
            s.OnButtonClick(0);
        }
        else if (e.code == "ArrowDown") {
            if ($("input[GridProductIndex=0]"))
                $("input[GridProductIndex=0]").focus();
        }
    }

    function ResSellableProductKeyDown(s, e) {
        console.log(e.htmlEvent.key);
        if (e.htmlEvent.key == "Enter") {
            s.OnButtonClick(0);
        }
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
            var TypId = gridEstimateProductEntryList.GetEditor("TaxTypeID").GetText();

            var Price = gridEstimateProductEntryList.batchEditApi.GetCellValue(globalrowindex, 'Price');
            var Qty = gridEstimateProductEntryList.batchEditApi.GetCellValue(globalrowindex, 'ProductQty');
            var Discount = gridEstimateProductEntryList.batchEditApi.GetCellValue(globalrowindex, 'Discount');
            if (Price != "" && Qty != "") {
                var amount = parseFloat((parseFloat(Qty).toFixed(4)) * (parseFloat(Price).toFixed(2))).toFixed(2);
                var amountAfterDiscount = parseFloat(amount) - ((parseFloat(Discount) * parseFloat(amount)) / 100);
                var Tamount = parseFloat(DecimalRoundoff(amountAfterDiscount, 2)).toFixed(2);

                if (TypId == "3" || TypId == "") {
                    gridEstimateProductEntryList.batchEditApi.StartEdit(globalrowindex, PrdAmountIndex);
                    gridEstimateProductEntryList.GetEditor('Amount').SetText(Tamount);

                    gridEstimateProductEntryList.batchEditApi.StartEdit(globalrowindex, PrdNetAmtIndex);
                    gridEstimateProductEntryList.GetEditor('NetAmount').SetText(Tamount);

                    //if (gridEstimateProductEntryList.batchEditApi.GetCellValue(globalrowindex, 11) == null) {
                    gridEstimateProductEntryList.batchEditApi.StartEdit(globalrowindex, PrdChargesIndex);
                    gridEstimateProductEntryList.GetEditor('Charges').SetText("0.00");
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

                    var ret = caluculateAndSetGST(gridEstimateProductEntryList.GetEditor("Amount"), gridEstimateProductEntryList.GetEditor("Charges"), gridEstimateProductEntryList.GetEditor("NetAmount"),
                            gridEstimateProductEntryList.GetEditor("ProdHSN").GetText(), DecimalRoundoff(amount, 2), DecimalRoundoff(amountAfterDiscount, 2), taxType, "15", $('#ddlBankBranch').val())
                    var data = ret.split('~');

                    var Netamind = gridEstimateProductEntryList.batchEditApi.GetColumnIndex('NetAmount');
                    gridEstimateProductEntryList.batchEditApi.StartEdit(globalrowindex, Netamind);
                    gridEstimateProductEntryList.GetEditor('NetAmount').SetText(data[2]);

                    var amind = gridEstimateProductEntryList.batchEditApi.GetColumnIndex('Amount');
                    gridEstimateProductEntryList.batchEditApi.StartEdit(globalrowindex, amind);
                    gridEstimateProductEntryList.GetEditor('Amount').SetText(data[0]);

                    var Charges = gridEstimateProductEntryList.batchEditApi.GetColumnIndex('Charges');
                    gridEstimateProductEntryList.batchEditApi.StartEdit(globalrowindex, Charges);
                    gridEstimateProductEntryList.GetEditor('Charges').SetText(data[1].toString());
                }
            }
        }
    }

    function SetEstimateFocusGrid() {
        gridEstimateProductEntryList.batchEditApi.StartEdit(globalrowindex, PrdNetAmtIndex);
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
                else if (indexName == "GridSellableProductIndex") {
                    SetSellableProduct(Id, name, null);
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
                else if (indexName == "GridSellableProductIndex") {
                    $('#txtGridProductName').focus();
                }
                else if (indexName == "customerIndex") {
                    $('#txtCustSearch').focus();
                }
                //else
                //    $('#txtCustSearch').focus();
            }
        }

    }

    function SetFocusDesc() {
        gridEstimateProductEntryList.batchEditApi.StartEdit(globalrowindex, PrdProdNameIndex);
        gridEstimateResourcesList.batchEditApi.StartEdit(globalrowindex2, ResProductNameIndex);
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
        var hdnDetailsID = $('#hdnDetailsID').val();
        if (hdnDetailsID > 0 || hdnDetailsID=='') {
            var dt = new Date();
            document.getElementById("EstimateNo").maxLength = schemelength;
            EstimateDate_dt.SetDate(dt);

            if (dt < new Date(fromdate)) {
                EstimateDate_dt.SetDate(new Date(fromdate));
            }

            if (dt > new Date(todate)) {
                EstimateDate_dt.SetDate(new Date(todate));
            }
        }
        EstimateDate_dt.SetMinDate(new Date(fromdate));
        EstimateDate_dt.SetMaxDate(new Date(todate));
        //EstimateDate_dt.focus();

        if (branchID > 0) {
            $('#ddlBankBranch').val(branchID);
        }

        if (schemetype == '0') {
            $('#EstimateNo').removeAttr("disabled");
            $('#EstimateNo').val('');

            $('#EstimateNo').focus();
        }
        else if (schemetype == '1') {
            $('#EstimateNo').attr("disabled", "disabled");
            $('#EstimateNo').val('Auto');

            //$('#EstimateNo').focus();

        }
        else if (schemetype == '2') {
            $('#EstimateNo').attr("disabled", "disabled");
            $('#EstimateNo').val('Datewise');

            //$('#EstimateNo').focus();

        }
        else if (schemetype == 'n') {
            $('#EstimateNo').attr("disabled", "disabled");
            $('#EstimateNo').val('');

            //$('#EstimateNo').focus();
        }
        else {
            $('#EstimateNo').attr("disabled", "disabled");
            $('#EstimateNo').val('');

            //$('#EstimateNo').focus();

        }

    }

    function SuffleRows() {
        for (var i = 0; i < 1000; i++) {
            if (gridEstimateProductEntryList.GetRow(i)) {
                if (gridEstimateProductEntryList.GetRow(i).style.display != "none") {
                    gridEstimateProductEntryList.batchEditApi.StartEdit(i, PrdUpdateEditIndex);
                    gridEstimateProductEntryList.GetEditor("UpdateEdit").SetText(i);
                }
            }
        }

        for (i = -1; i > -1000; i--) {
            if (gridEstimateProductEntryList.GetRow(i)) {
                if (gridEstimateProductEntryList.GetRow(i).style.display != "none") {
                    gridEstimateProductEntryList.batchEditApi.StartEdit(i, PrdUpdateEditIndex);
                    gridEstimateProductEntryList.GetEditor("UpdateEdit").SetText(i);
                }
            }
        }
    }

    function SuffleRowsGrid2() {
        for (var i = 0; i < 1000; i++) {
            if (gridEstimateResourcesList.GetRow(i)) {
                if (gridEstimateResourcesList.GetRow(i).style.display != "none") {
                    gridEstimateResourcesList.batchEditApi.StartEdit(i, ResUpdateEditIndex);
                    gridEstimateResourcesList.GetEditor("UpdateEdit").SetText(i);
                }
            }
        }

        for (i = -1; i > -1000; i--) {
            if (gridEstimateResourcesList.GetRow(i)) {
                if (gridEstimateResourcesList.GetRow(i).style.display != "none") {
                    gridEstimateResourcesList.batchEditApi.StartEdit(i, ResUpdateEditIndex);
                    gridEstimateResourcesList.GetEditor("UpdateEdit").SetText(i);
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
            //gridEstimateResourcesList.batchEditApi.StartEdit(globalrowindex2, ResProductNameIndex);
            setTimeout(function () {
                gridEstimateResourcesList.Refresh();
                gridEstimateResourcesList.Refresh();
            }, 100);

            $(this).hide();
        });

        $('#closeResource').click(function () {
            jConfirm('Are you sure to close? Clicking on "Yes" will clear the data from grid.', 'Alert!', function (r) {
                if (r) {
                    $('#refreshgrid2').hide();
                    $('#showResources').show();
                    for (var i = 500; i > -500; i--) {
                        if (gridEstimateResourcesList.GetRow(i)) {
                            gridEstimateResourcesList.DeleteRow(i);
                        }
                    }
                    $("#txtGridResourcesTotalAmount").val('0.00');
                    //AddNewRowGridResources();
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
            EstimateSave('Exit');
        }
        //if (event.keyCode == 83 && event.altKey == true) { //  && myModal.GetVisible() == true
        //    EstimateSave('New');
        //}
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
        // gridEstimateProductEntryList.batchEditApi.EndEdit();
        //gridEstimateProductEntryList.batchEditApi.StartEdit(globalrowindex, 2);
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
                Proposallist($("#txtProposalName").val());
            }
        }
        else if (e.code == "ArrowDown") {
            if ($("input[ProposalIndex=0]"))
                $("input[ProposalIndex=0]").focus();
        }
    }

   

    function SetProposal(Id, Name, e) {
        finishedproductlist = 0;
        var ProposalID = Id;
        var ProposalName = Name;

        if (ProposalID != "") {

            var data = ProposalID.split('|');
            ProposalID = data[0];
            $("#hdnQuotation_ID").val(ProposalID);

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

        //callonServerScroll("../Models/PMS_WebServiceList.asmx/GetProductDetailsList", OtherDetails, "QuotationTable", HeaderCaption, "QuotationIndex", "SetQuotation");
        callonServer("../Models/PMS_WebServiceList.asmx/GetProductDetailsList", OtherDetails, "QuotationTable", HeaderCaption, "QuotationIndex", "SetQuotation");
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
        slno = gridEstimateResourcesList.GetDataRow(globalrowindex).children[0].innerHTML.trim();
        gridEstimateResourcesList.batchEditApi.StartEdit(globalrowindex, ResProductNameIndex);
        var ProductName = gridEstimateResourcesList.GetEditor('ProductName').GetText();
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


        }
    }

    var projectCode = [];

    function GetProjectSelectedFieldValuesCallback(values) {
        try {
            alert(values);
            projectCode = [];
            for (var i = 0; i < values.length; i++) {
                projectCode.push(values[i]);
            }
        } finally {
            console.log(projectCode);
        }
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


    function gridcustombuttonclick(s, e) {
        // alert(s);
        OpenAddlDesc(s, e);
    }

    function EstimateApprove(mode) {
        //debugger;
        var flag = true;
        var Actions = '';
        if ($("#txt_ApproveRemarks").val() == "") {
            flag = false;
            $("#txt_ApproveRemarks").focus();
        }
        else {
            if (mode == 'Approve') {
                jConfirm('Do you want to approve Estimate?', 'Confirm Dialog', function (r) {
                    if (r == true) {
                    //    @*Actions = "ApproveEstimateData";
                    //    flag = true;

                    //    var detailsid = $("#hdnDetailsID").val();
                    //    var ApproveRemarks = $("#txt_ApproveRemarks").val();

                    //    $.ajax({
                    //        type: "POST",
                    //        url: "@Url.Action("ApproveEstimateDataByID", "Estimate")",
                    //        data: { detailsid: detailsid, Approve_Remarks: ApproveRemarks, Action: Actions },
                    //    success: function (response) {
                    //        if (response != null) {
                    //            jAlert(response.Message);
                    //            if (response.Success) {
                    //                setTimeout(function () {
                    //                    var url = $('#hdnEstimateListPage').val();
                    //                    window.location.href = url;
                    //                }, 500);
                    //            }
                    //        }
                    //    }
                    //});*@
                    $("#hdnApproveReject").val('Approve');// = "Approve";
                    EstimateSave('Exit');
                }
                else {
                        flag = false;
                return false;
            }
        });
    }
    else if (mode == 'Reject') {
        jConfirm('Do you want to reject Estimate?', 'Confirm Dialog', function (r) {
            if (r == true) {
            //    @*Actions = "RejectEstimateData";
            //    flag = true;
            //    var detailsid = $("#hdnDetailsID").val();
            //    var ApproveRemarks = $("#txt_ApproveRemarks").val();

            //    $.ajax({
            //        type: "POST",
            //        url: "@Url.Action("ApproveEstimateDataByID", "Estimate")",
            //        data: { detailsid: detailsid, Approve_Remarks: ApproveRemarks, Action: Actions },
            //    success: function (response) {
            //        if (response != null) {
            //            jAlert(response.Message);
            //            if (response.Success) {
            //                setTimeout(function () {
            //                    var url = $('#hdnEstimateListPage').val();
            //                    window.location.href = url;
            //                }, 500);
            //            }
            //        }
            //    }
            //});*@
            $("#hdnApproveReject").val('Reject');// = "Reject";
            EstimateSave('Exit');
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
        $("#expandgridEstimateProductEntryList").click(function (e) {
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
                gridEstimateProductEntryList.SetHeight(browserHeight - 150);
                gridEstimateProductEntryList.SetWidth(cntWidth);
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
                gridEstimateProductEntryList.SetHeight(300);
                var cntWidth = $this.parent('.makeFullscreen').width();
                gridEstimateProductEntryList.SetWidth(cntWidth);
            }
        });
        $("#expandgridEstimateResourcesList").click(function (e) {
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
                gridEstimateResourcesList.SetHeight(browserHeight - 150);
                gridEstimateResourcesList.SetWidth(cntWidth);
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
                gridEstimateResourcesList.SetHeight(300);
                var cntWidth = $this.parent('.makeFullscreen').width();
                gridEstimateResourcesList.SetWidth(cntWidth);
            }
        });
        $('.navbar-minimalize').click(function () {
            //gridEstimateProductEntryList.Refresh();
            setTimeout(function () {
                var containerWidth = $('#refreshgrid').width();
                gridEstimateProductEntryList.SetWidth(containerWidth);
            }, 100);


        });

        //gridEstimateList.Refresh();
    });


    function Remarkstab() {
        setTimeout(function () {
            gridEstimateProductEntryList.batchEditApi.EndEdit();
            gridEstimateProductEntryList.batchEditApi.StartEdit(-1, PrdSrlIndex);
        }, 5);
    }


    function EditData(values) {
        GridWarehouselist();
        $.ajax({
            type: "POST",
            //url: "@Url.Action("EditData", "Estimate")",
            url: "../Estimate/EditData",
            data: { HiddenID: values },
            success: function (response) {
                if (response != null) {
                    btnProduct.SetValue(response.ProductName),
                     $("#hdnProdProductID").val(response.ProductId),
                     $("#txtProductdescription").val(response.ProductDescription),
                     cProductQty.SetText(response.ProductQty),
                     $("#txt_ProdUOM").val(response.ProductUOM);
                    cProductPrice.SetText(response.Price),
                    cProductAmount.SetText(response.Amount),
                    $("#txt_AddlRemarks").val(response.Remarks),
                    cProductCharges.SetText(response.Charges),
                    cProductDiscount.SetText(response.Discount),
                    cProductNetAmount.SetText(response.NetAmount),
                    cProductBudgetedPrice.SetText(response.BudgetedPrice),
                    $("#ddlTaxTypelistProduct").val(response.TaxTypeID),
                    $("#hdnProdHSN").val(response.ProdHSN),
                    $("#txt_AddlDescProd").val(response.AddlDesc),
                   $("#GuiIDS").val(response.Guids),
                    btnSellable.SetValue(response.Sellable),
                    $("#hdnSellableProductID").val(response.SellableID),
                   $("#ProductDetailsID").val(response.ProductDetailsID),
                   $("#BalQty").val(response.BalQty)

                    if (response.ProductsWarehouseID == 0) {
                        $("#ddlProductWarehouselist").val('select');
                    }
                    else {
                        $("#ddlProductWarehouselist").val(response.ProductsWarehouseID);
                    }

                }
            }
        });
    }

    function DeleteData(values) {
        $.ajax({
            type: "POST",
            //url: "@Url.Action("DeleteData", "Estimate")",
            url: "../Estimate/DeleteData",
            data: { HiddenID: values },
            success: function (response) {
                if (response != null) {
                    gridEstimateProductEntryList.Refresh();
                }
            }
        });
    }

    function ResEditData(values) {
        $.ajax({
            type: "POST",
            //url: "@Url.Action("EditResourcesData", "Estimate")",
            url: "../Estimate/EditResourcesData",
            data: { HiddenID: values },
            success: function (response) {
                if (response != null) {
                    btnResProduct.SetValue(response.ProductName),
                     $("#hdnResProductID").val(response.ProductId),
                     $("#txtResProductdescription").val(response.ProductDescription),
                     cResProductQty.SetText(response.ProductQty),
                     $("#txt_ResProdUOM").val(response.ProductUOM),
                    //$("#ddlProductWarehouselist").val(response.ProductsWarehouseID),
                     cResProductPrice.SetText(response.Price),
                     cResProductAmount.SetText(response.Amount),
                     $("#txt_ResAddlRemarks").val(response.Remarks),
                     cResProductCharges.SetText(response.Charges),
                     cResProductDiscount.SetText(response.Discount),
                     cResProductNetAmount.SetText(response.NetAmount),
                     cResProductBudgetedPrice.SetText(response.BudgetedPrice),
                     $("#ddlTaxTypelistResource").val(response.TaxTypeID),
                     $("#hdnResHSN").val(response.ProdHSN),
                     $("#txt_ResAddlDescProd").val(response.AddlDesc),
                    $("#ResGuiIDS").val(response.Guids),
                       btnResSellable.SetValue(response.Sellable),
                     $("#hdnResSellableProductID").val(response.SellableID),
                    $("#ResBalQty").val(response.BalQty),
                    $("#ResProductDetailsID").val(response.ProductDetailsID)
                }
            }
        });
    }

    function ResDeleteData(values) {
        $.ajax({
            type: "POST",
            //url: "@Url.Action("DeleteResourcesData", "Estimate")",
            url: "../Estimate/DeleteResourcesData",
            data: { HiddenID: values },
            success: function (response) {
                if (response != null) {
                    gridEstimateResourcesList.Refresh();
                }
            }
        });
    }

   
