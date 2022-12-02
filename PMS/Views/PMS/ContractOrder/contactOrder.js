

    var globalrowindex2 = 0;
    var globalrowindex = 0;
    var gridtxtbox = '1';
    var slno = 0;
    var firsttime = 0;
    var DetailsID = 0;
    var ProductionID = 0;
    var GContractNo = "";
    var Message = "";
    var savemode = "";
    var hasmsg = 0;
    var rowtime = 0;
    var rowtime2 = 0;
    function btnProductList_Click(s, e) {
        Productlist('', null);
        setTimeout(function () { $("#txtProductName").focus(); }, 500);
        $('#ProductlistModel').modal('show');


    }

    function btnBOQList_Click(s, e) {
        Productlist('', null);
        setTimeout(function () { $("#txtBOQName").focus(); }, 500);
        $('#BOQlistModel').modal('show');
    }

    function btnEstimateList_Click(s, e) {
        Productlist('', null);
        setTimeout(function () { $("#txtEstimateName").focus(); }, 500);
        $('#EstimatelistModel').modal('show');
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

    $(document).ready(function () {
        setTimeout(function () { AddNewRowWithSl(); AddNewRowGridResources(); }, 200);


    });

    function AddNewRowGridResources() {
        //gridContractResourcesList.batchEditApi.StartEdit(index, 1);
        gridContractResourcesList.batchEditApi.EndEdit();
        gridContractResourcesList.AddNewRow();
        index = globalrowindex2;
        resufflegrid2Serial();

        setTimeout(function () {
            gridContractResourcesList.batchEditApi.EndEdit();
            gridContractResourcesList.batchEditApi.StartEdit(index, 1);
        }, 200);
    }

    function resufflegrid2Serial() {
        var sl = 1;
        var visiablerow = gridContractResourcesList.GetVisibleRowsOnPage();
        if (visiablerow > 0 && rowtime2 == 0) {
            sl = visiablerow;
            rowtime2++;
        }
        for (var i = -1; i > -500; i--) {
            if (gridContractResourcesList.GetRow(i)) {
                gridContractResourcesList.batchEditApi.StartEdit(i, 1);
                gridContractResourcesList.GetEditor('SlNO').SetText(sl);
                gridContractResourcesList.batchEditApi.StartEdit(i, 1);
                sl = sl + 1;
            }
        }
    }

    function addNewRowToEditgrid() {
        gridContractProductEntryList.batchEditApi.EndEdit();
        gridContractProductEntryList.AddNewRow();

        var sl = 1;
        var visiablerow = gridContractProductEntryList.GetVisibleRowsOnPage();
        if (visiablerow > 0) {
            sl = visiablerow;
        }
        gridContractProductEntryList.batchEditApi.StartEdit(globalrowindex, 1);
        gridContractProductEntryList.GetEditor('SlNO').SetText(sl);


        setTimeout(function () {
            gridContractProductEntryList.batchEditApi.EndEdit();
            gridContractProductEntryList.batchEditApi.StartEdit(globalrowindex, 2);
        }, 200);


    }

    function addNewRowToEditResourcegrid() {
        gridContractResourcesList.batchEditApi.EndEdit();
        gridContractResourcesList.AddNewRow();

        var sl = 1;
        var visiablerow = gridContractResourcesList.GetVisibleRowsOnPage();
        if (visiablerow > 0) {
            sl = visiablerow;
        }
        gridContractResourcesList.batchEditApi.StartEdit(globalrowindex2, 1);
        gridContractResourcesList.GetEditor('SlNO').SetText(sl);

        setTimeout(function () {
            gridContractResourcesList.batchEditApi.EndEdit();
            gridContractResourcesList.batchEditApi.StartEdit(globalrowindex2, 2);
        }, 200);


    }

    function DeleteRowProductGrid(edit) {
        var sl = 1;
        gridContractProductEntryList.batchEditApi.EndEdit();
        for (var i = 0; i < 500; i++) {
            if (gridContractProductEntryList.GetRow(i) && i != edit) {
                var tr = gridContractProductEntryList.GetRow(i);
                if (tr.style.display != "none") {

                    gridContractProductEntryList.batchEditApi.StartEdit(i, 1);
                    gridContractProductEntryList.GetEditor('SlNO').SetText(sl);
                    //if (grid.GetEditor('low').GetText() == "") {
                    //    grid.GetEditor('low').SetText(0);
                    //    grid.GetEditor('high').SetText(0);
                    //    grid.GetEditor('value').SetText(0);
                    //}
                    gridContractProductEntryList.batchEditApi.StartEdit(i, 1);
                    sl = sl + 1;
                }
            }
        }

        for (var i = -1; i > -500; i--) {
            if (gridContractProductEntryList.GetRow(i) && i != edit) {

                var tr = gridContractProductEntryList.GetRow(i);
                if (tr.style.display != "none") {

                    gridContractProductEntryList.batchEditApi.StartEdit(i, 1);
                    gridContractProductEntryList.GetEditor('SlNO').SetText(sl);
                    //if (grid.GetEditor('low').GetText() == "") {
                    //    grid.GetEditor('low').SetText(0);
                    //    grid.GetEditor('high').SetText(0);
                    //    grid.GetEditor('value').SetText(0);
                    //}
                    gridContractProductEntryList.batchEditApi.StartEdit(i, 1);
                    sl = sl + 1;
                }
            }
        }
    }

    function DeleteRowResourceGrid(edit) {
        var sl = 1;
        gridContractResourcesList.batchEditApi.EndEdit();
        for (var i = 0; i < 500; i++) {
            if (gridContractResourcesList.GetRow(i) && i != edit) {
                var tr = gridContractResourcesList.GetRow(i);
                if (tr.style.display != "none") {

                    gridContractResourcesList.batchEditApi.StartEdit(i, 1);
                    gridContractResourcesList.GetEditor('SlNO').SetText(sl);
                    //if (grid.GetEditor('low').GetText() == "") {
                    //    grid.GetEditor('low').SetText(0);
                    //    grid.GetEditor('high').SetText(0);
                    //    grid.GetEditor('value').SetText(0);
                    //}
                    gridContractResourcesList.batchEditApi.StartEdit(i, 1);
                    sl = sl + 1;
                }
            }
        }

        for (var i = -1; i > -500; i--) {
            if (gridContractResourcesList.GetRow(i) && i != edit) {

                var tr = gridContractResourcesList.GetRow(i);
                if (tr.style.display != "none") {

                    gridContractResourcesList.batchEditApi.StartEdit(i, 1);
                    gridContractResourcesList.GetEditor('SlNO').SetText(sl);
                    //if (grid.GetEditor('low').GetText() == "") {
                    //    grid.GetEditor('low').SetText(0);
                    //    grid.GetEditor('high').SetText(0);
                    //    grid.GetEditor('value').SetText(0);
                    //}
                    gridContractResourcesList.batchEditApi.StartEdit(i, 1);
                    sl = sl + 1;
                }
            }
        }
    }

    /*---------------Arindam*----------*/
    function AddNewRowWithSl() {

        gridContractProductEntryList.batchEditApi.EndEdit();
        gridContractProductEntryList.AddNewRow();
        index = globalrowindex;
        resuffleSerial();

        setTimeout(function () {
            gridContractProductEntryList.batchEditApi.EndEdit();
            gridContractProductEntryList.batchEditApi.StartEdit(index, 1);
        }, 200);
    }


    function resuffleSerial() {

        var sl = 1;
        var visiablerow = gridContractProductEntryList.GetVisibleRowsOnPage();
        if (visiablerow > 0 && rowtime == 0) {
            sl = visiablerow;
            rowtime++;
        }

        for (var i = -1; i > -500; i--) {
            if (gridContractProductEntryList.GetRow(i)) {
                gridContractProductEntryList.batchEditApi.StartEdit(i, 1);
                gridContractProductEntryList.GetEditor('SlNO').SetText(sl);
                //if (grid.GetEditor('low').GetText() == "") {
                //    grid.GetEditor('low').SetText(0);
                //    grid.GetEditor('high').SetText(0);
                //    grid.GetEditor('value').SetText(0);
                //}
                gridContractProductEntryList.batchEditApi.StartEdit(i, 1);
                sl = sl + 1;
            }
        }
    }


    function grid_CustomButtonGridResourcesClick() {
        //if (e.buttonID == "Delete") {
        var noofvisiblerows = gridContractResourcesList.GetVisibleRowsOnPage();

        if (noofvisiblerows != 1) {
            gridContractResourcesList.DeleteRow(globalrowindex2);

            if ($('#hdnDetailsID').val() == 0) {
                resufflegrid2Serial();
            }
            else {
                DeleteRowResourceGrid(globalrowindex2);
            }

            BOMGridResourceSetTotalAmount();
        }
        //}
        //e.processOnServer = false;

    }

    function grid_CustomButtonClick() {
        // if (e.buttonID == "DeleteProduct") {
        var noofvisiblerows = gridContractProductEntryList.GetVisibleRowsOnPage();

        if (noofvisiblerows != 1) {
            gridContractProductEntryList.DeleteRow(globalrowindex);

            if ($('#hdnDetailsID').val() == 0) {

                resuffleSerial();
            }
            else {
                DeleteRowProductGrid(globalrowindex);
            }

            ContractGridSetTotalAmount();
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
        slno = gridContractResourcesList.GetDataRow(globalrowindex2).children[0].innerHTML.trim();
        GridNonInventoryProductlist("", "nonInventory", slno);
        typemodal = "nonInventory";
        $('#GridProductlistModel').modal('show');
        $('#txtGridProductName').focus();

        setTimeout(function () {
            $('#txtGridProductName').focus();
        }, 600);
    }

    function GridNonInventoryProductlist(SearchKey, type, txtid) {
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


        setTimeout(function () {
            $('#txtGridProductName').focus();
        }, 600);
    }

    function OpenProductList(s, e) {
        if (gridContractProductEntryList.GetDataRow(globalrowindex) != null) {
            slno = gridContractProductEntryList.GetDataRow(globalrowindex).children[0].innerHTML.trim();
        }
        GridProductlist("", "A", slno);
        typemodal = "A";
        $('#GridProductlistModel').modal('show');
        $('#txtGridProductName').focus();
    }

    function OpenWarehouseList(s, e) {
        slno = gridContractProductEntryList.GetDataRow(globalrowindex).children[0].innerHTML.trim();
        gridContractProductEntryList.batchEditApi.StartEdit(globalrowindex, 2);
        var ProductName = gridContractProductEntryList.GetEditor('ProductName').GetText();
        if (ProductName == "") {
            jAlert("Please select product before select warehouse!");
            return false;
        }
        else {
            GridWarehouselist();
            warehousefocus = 1;

        }
    }

    function GridWarehouselist() {
        var BankBranchID = $('#ddlBankBranch option:selected').val();
        if (BankBranchID > 0) {
            $.ajax({
                type: "POST",
                url: "@Url.Action("getWarehouseRecord", "ContractOrder")",
                data: { branchid: BankBranchID },
                success: function (response) {
                    $('#ddlWarehouselist').html('');
                    var html = "";
                    for (var i = 0; i < response.length; i++) {
                        html = html + "<option value='" + response[i].WarehouseID + "'>" + response[i].WarehouseName + "</option>";
                    }
                    $('#ddlWarehouselist').html(html);
                    gridContractProductEntryList.batchEditApi.EndEdit();
                    //$('#setWarehousegrid').focus();
                    $('#GridWarehouselistModel').modal('show');


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
            gridContractProductEntryList.batchEditApi.StartEdit(globalrowindex, 6);
            gridContractProductEntryList.GetEditor('Warehouse').SetText(Warehousetxt);

            gridContractProductEntryList.batchEditApi.StartEdit(globalrowindex, 14);
            gridContractProductEntryList.GetEditor('ProductsWarehouseID').SetText(Warehouseid);
            $('#GridWarehouselistModel').modal('hide');
            gridContractProductEntryList.batchEditApi.StartEdit(globalrowindex, 7);
        }
        else {
            jAlert("Please select warehouse!");
        }
    }

    function SetWarehouseAfterProduct() {
        gridContractProductEntryList.batchEditApi.StartEdit(globalrowindex, 7);
    }

    var globalrowindex = 0;
    function gridclick(s, e) {
        globalrowindex = e.visibleIndex;
    }

    function gridResourceclick(s, e) {
        globalrowindex2 = e.visibleIndex;
    }

    var Contractlinkindex = 0;
    function OpenContractList(s, e) {
        slno = gridContractProductEntryList.GetDataRow(globalrowindex).children[0].innerHTML.trim();
        gridContractProductEntryList.batchEditApi.StartEdit(globalrowindex, 2);
        var ProductName = gridContractProductEntryList.GetEditor('ProductName').GetText();
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

    function GetContractList(SearchKey, slno) {
        var productid = gridContractProductEntryList.batchEditApi.GetCellValue(globalrowindex, 'ProductId');
        var OtherDetails = {}
        var CotractDate = GetServerDateFormat(CotractDate_dt.GetValue());
        OtherDetails.SearchKey = SearchKey;
        OtherDetails.ProductID = productid;
        OtherDetails.CotractDate = CotractDate;
        OtherDetails.BranchID = $('#ddlBankBranch option:selected').val();
        var HeaderCaption = [];
        HeaderCaption.push("BOM No.");
        HeaderCaption.push("BOM Date");
        HeaderCaption.push("Revision No.");
        HeaderCaption.push("Revision Date");

        callonServerScroll("../Models/PMS_WebServiceList.asmx/GetContractList", OtherDetails, "GridBOMTable", HeaderCaption, "BOMIndex", "SetGridBOMProduct");

    }

    function GridProductlist(SearchKey, type, txtid) {
        // debugger;
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

        setTimeout(function () {
            $('#txtGridProductName').focus();
        }, 600);

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

    function BOQListkeydown(e) {
        if (e.code == "Enter" || e.code == "NumpadEnter") {
            if ($("#txtBOQName").val() != '') {
                BOQlist($("#txtBOQName").val(), null);
            }
        }
        else if (e.code == "ArrowDown") {
            if ($("input[BOQIndex=0]"))
                $("input[BOQIndex=0]").focus();
        }
    }

    function BOQlist(SearchKey, type) {
        finishedproductlist = 1;
        var OtherDetails = {}
        OtherDetails.SearchKey = SearchKey;
        OtherDetails.Type = type;

        var HeaderCaption = [];
        // HeaderCaption.push("Product ID");
        HeaderCaption.push("BOQ No.");
        HeaderCaption.push("BOQ Date");

        callonServerScroll("../Models/PMS_WebServiceList.asmx/GetProductDetailsList", OtherDetails, "BOQTable", HeaderCaption, "BOQIndex", "SetBOQ");
    }

    function SetBOQ(Id, Name, e) {
        finishedproductlist = 0;
        var BOQID = Id;
        var BOQName = Name;

        if (BOQID != "") {

            var data = BOQID.split('|');
            BOQID = data[0];
            $('#BOQlistModel').modal('hide');
            btnBOQ.SetText(BOQName);
            $('#hdnBOQID').val(BOQID);
            //document.getElementById('hdnBOQID').value = Id;
            $('#btnBOQ').select();
            $('#btnBOQ').focus();
        }
    }

    function EstimateListkeydown(e) {
        if (e.code == "Enter" || e.code == "NumpadEnter") {
            if ($("#txtEstimateName").val() != '') {
                Estimatelist($("#txtEstimateName").val(), null);
            }
        }
        else if (e.code == "ArrowDown") {
            if ($("input[EstimateIndex=0]"))
                $("input[EstimateIndex=0]").focus();
        }
    }

    function Estimatelist(SearchKey, type) {
        finishedproductlist = 1;
        var OtherDetails = {}
        OtherDetails.SearchKey = SearchKey;
        OtherDetails.Type = type;

        var HeaderCaption = [];
        // HeaderCaption.push("Product ID");
        HeaderCaption.push("Estimate No.");
        HeaderCaption.push("Estimate Date");
      
        callonServerScroll("../Models/PMS_WebServiceList.asmx/GetProductDetailsList", OtherDetails, "EstimateTable", HeaderCaption, "EstimateIndex", "SetEstimate");
    }

    function SetEstimate(Id, Name, e) {
        debugger;
        finishedproductlist = 0;
        var EstimateID = Id;
        var EstimateName = Name;

        if (EstimateID != "") {

            var data = EstimateID.split('|');
            EstimateID = data[0];
            $('#EstimatelistModel').modal('hide');
            btnEstimate.SetText(EstimateName);
            $('#hdnEstimate_ID').val(EstimateID);
           // document.getElementById('hdnFinishedItem').value = Id;
            $('#btnEstimate').select();
            $('#btnEstimate').focus();
        }
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

    function SetGridNonInventoryProduct(Id, Name, e) {
       // debugger;
        gridnonproductlist = 0;
        var ProductID = Id;
        var ProductName = Name;
        //alert('');
        if (ProductID != "") {

            var data = ProductID.split('|');
            ProductID = data[0];

            var amind = gridContractResourcesList.batchEditApi.GetColumnIndex('Amount');
            gridContractResourcesList.batchEditApi.StartEdit(globalrowindex2, amind);
            gridContractResourcesList.GetEditor('Amount').SetText("0.00");

            var qtyindex = gridContractResourcesList.batchEditApi.GetColumnIndex('ProductQty');
            gridContractResourcesList.batchEditApi.StartEdit(globalrowindex2, qtyindex);
            gridContractResourcesList.GetEditor('ProductQty').SetText("0.00");

            gridContractResourcesList.batchEditApi.StartEdit(globalrowindex2, 11);
            gridContractResourcesList.GetEditor('ProductId').SetText(ProductID);

            gridContractResourcesList.batchEditApi.StartEdit(globalrowindex2, 2);
            gridContractResourcesList.GetEditor('ProductName').SetText(ProductName);
            //gridContractResourcesList.GetDataRow(globalrowindex).children[1].innerHTML = ProductName;

            //$('#' + gridtxtbox + '_txtbox').val(ProductName);
            $('#GridProductlistModel').modal('hide');
            gridContractResourcesList.batchEditApi.StartEdit(globalrowindex2, 3);
            gridContractResourcesList.GetEditor('ProductDescription').SetText(data[6]);
            gridContractResourcesList.batchEditApi.StartEdit(globalrowindex2, 5);

            gridContractResourcesList.GetEditor('ProductUOM').SetText(data[1]);
            //$('#' + gridtxtbox + '_txtDescription').val(data[2]);
            //$('#' + gridtxtbox + '_txtStockUOM').val(data[1]);
            gridContractResourcesList.batchEditApi.StartEdit(globalrowindex2, 7);
            gridContractResourcesList.GetEditor('Price').SetText(data[3]);
            gridContractResourcesList.batchEditApi.StartEdit(globalrowindex2, 13);
            gridContractResourcesList.GetEditor('ProductsWarehouseID').SetText(data[4]);
            //$('#' + gridtxtbox + '_txtPrice').val(data[3]);
            //gridContractResourcesList.batchEditApi.StartEdit(globalrowindex2, 6);
            //gridContractResourcesList.GetEditor('Warehouse').SetText(data[5]);
            gridContractResourcesList.batchEditApi.StartEdit(globalrowindex2, 4);
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
            $('#FinishedUom').val(data[1]);
            $('#ProductlistModel').modal('hide');
            btnFinishedItem.SetText(ProductName);
            $('#hdnFinishedItem').val(ProductID);
            document.getElementById('hdnProductID').value = Id;
            $('#FinishedQty').select();
            $('#FinishedQty').focus();
        }
    }

    function SetGridProduct(Id, Name, e) {
       // debugger;
        gridproductlist = 0;
        var ProductID = Id;
        var ProductName = Name;

        if (ProductID != "") {

            var data = ProductID.split('|');
            ProductID = data[0];

            var amind = gridContractProductEntryList.batchEditApi.GetColumnIndex('Amount');
            gridContractProductEntryList.batchEditApi.StartEdit(globalrowindex, amind);
            gridContractProductEntryList.GetEditor('Amount').SetText("0.00");

            var qtyindex = gridContractProductEntryList.batchEditApi.GetColumnIndex('ProductQty');
            gridContractProductEntryList.batchEditApi.StartEdit(globalrowindex, qtyindex);
            gridContractProductEntryList.GetEditor('ProductQty').SetText("0.00");

            gridContractProductEntryList.batchEditApi.StartEdit(globalrowindex, 12);
            gridContractProductEntryList.GetEditor('ProductId').SetText(ProductID);

            gridContractProductEntryList.batchEditApi.StartEdit(globalrowindex, 2);
            gridContractProductEntryList.GetEditor('ProductName').SetText(ProductName);
            //gridContractProductEntryList.GetDataRow(globalrowindex).children[1].innerHTML = ProductName;

            //$('#' + gridtxtbox + '_txtbox').val(ProductName);
            $('#GridProductlistModel').modal('hide');
            gridContractProductEntryList.batchEditApi.StartEdit(globalrowindex, 3);
            gridContractProductEntryList.GetEditor('ProductDescription').SetText(data[2]);
            gridContractProductEntryList.batchEditApi.StartEdit(globalrowindex, 5);

            gridContractProductEntryList.GetEditor('ProductUOM').SetText(data[1]);
            //$('#' + gridtxtbox + '_txtDescription').val(data[2]);
            //$('#' + gridtxtbox + '_txtStockUOM').val(data[1]);
            gridContractProductEntryList.batchEditApi.StartEdit(globalrowindex, 7);
            gridContractProductEntryList.GetEditor('Price').SetText(data[3]);
            gridContractProductEntryList.batchEditApi.StartEdit(globalrowindex, 15);
            gridContractProductEntryList.GetEditor('ProductsWarehouseID').SetText(data[4]);
            //$('#' + gridtxtbox + '_txtPrice').val(data[3]);
            gridContractProductEntryList.batchEditApi.StartEdit(globalrowindex, 6);
            gridContractProductEntryList.GetEditor('Warehouse').SetText(data[5]);
            gridContractProductEntryList.batchEditApi.StartEdit(globalrowindex, 4);
            //btnFinishedItem.SetText(ProductName);
            //document.getElementById('hdnProductID').value = Id;


        }
    }

    function ContractGridSetTotalAmount(s, e) {
        ////debugger;

        var ToTalAmount = 0;
        for (var i = 500; i > -500; i--) {
            if (gridContractProductEntryList.GetRow(i)) {
                var Amountval = gridContractProductEntryList.batchEditApi.GetCellValue(i, 'Amount');
                if (Amountval != "" && Amountval != null && Amountval != undefined) {
                    var ToTalAmount = parseFloat(parseFloat(ToTalAmount) + parseFloat(Amountval)).toFixed(2);
                    $('#txtGridProductEntryTotalAmount').val(ToTalAmount);
                }
            }
        }

        //var ToTalAmount = $('#txtGridProductEntryTotalAmount').val();
        //var Amountval = gridContractProductEntryList.batchEditApi.GetCellValue(globalrowindex, 'Amount');
        //if (ToTalAmount != "" || ToTalAmount != undefined || ToTalAmount != null) {
        //    ToTalAmount = parseFloat(0).toFixed(2);
        //}
        //if (Amountval != "" && Amountval != null && Amountval != undefined) {
        //    var calTotalAmount = parseFloat(parseFloat(ToTalAmount) + parseFloat(Amountval)).toFixed(2);
        //    $('#txtGridProductEntryTotalAmount').val(calTotalAmount);
        //}

    }

    function ContractGridResourceSetTotalAmount(s, e) {

        var ToTalAmount = 0;
        for (var i = 500; i > -500; i--) {
            if (gridContractResourcesList.GetRow(i)) {
                var Amountval = gridContractResourcesList.batchEditApi.GetCellValue(i, 'Amount');
                if (Amountval != "" && Amountval != null && Amountval != undefined) {
                    var ToTalAmount = parseFloat(parseFloat(ToTalAmount) + parseFloat(Amountval)).toFixed(2);
                    $('#txtGridResourcesTotalAmount').val(ToTalAmount);
                }
            }
        }


        //var ToTalAmount = $('#txtGridResourcesTotalAmount').val();
        //var Amountval = gridContractResourcesList.batchEditApi.GetCellValue(globalrowindex2, 'Amount');
        //if (ToTalAmount != "" || ToTalAmount != undefined || ToTalAmount != null) {
        //    ToTalAmount = parseFloat(0).toFixed(2);
        //}
        //if (Amountval != "" && Amountval != null && Amountval != undefined) {
        //    var calTotalAmount = parseFloat(parseFloat(ToTalAmount) + parseFloat(Amountval)).toFixed(2);
        //    $('#txtGridResourcesTotalAmount').val(calTotalAmount);
        //}
    }

    function SetGridBOMProduct(Id, Name, e) {
        //debugger;
        if (Id != "") {
            var data = Id.split('|');
            var Details_ID = data[0];
            var Production_ID = data[1];
            var Contract_No = data[2];
            var REV_No = data[3];
            var Contract_Date = data[4];
            var Rate = data[5];

            gridContractProductEntryList.batchEditApi.StartEdit(globalrowindex, 9);
            gridContractProductEntryList.GetEditor('ContractNo').SetText(Contract_No);

            gridContractProductEntryList.batchEditApi.StartEdit(globalrowindex, 10);
            gridContractProductEntryList.GetEditor('RevNo').SetText(REV_No);

            gridContractProductEntryList.batchEditApi.StartEdit(globalrowindex, 11);
            //  gridContractProductEntryList.GetEditor('RevDate').SetText(Contract_Date);

            gridContractProductEntryList.batchEditApi.StartEdit(globalrowindex, 7);
            gridContractProductEntryList.GetEditor('Price').SetText(Rate);

            gridContractProductEntryList.batchEditApi.StartEdit(globalrowindex, 16);
            gridContractProductEntryList.GetEditor('Tag_Details_ID').SetText(Details_ID);

            gridContractProductEntryList.batchEditApi.StartEdit(globalrowindex, 17);
            gridContractProductEntryList.GetEditor('Tag_Production_ID').SetText(Production_ID);

            ContractGridSetAmount("", "");

            setTimeout(function () {
                gridContractProductEntryList.batchEditApi.EndEdit();
                gridContractProductEntryList.batchEditApi.StartEdit();
                ContractGridSetTotalAmount("", "");
            }, 1000);

        }
        $('#GridBOMlistModel').modal('hide');

    }

    //function OnInit(s, e) {
    //   //debugger;
    //    //var grid = MVCxClientGridView.Cast(s);
    //    //grid.batchEditApi.ValidateRows();
    //}

    function OnGridViewEndCallback(s, e) {
        //debugger;
        if (gridContractResourcesList.batchEditApi.HasChanges()) {
            gridContractResourcesList.UpdateEdit();
        }
    }

    function OnResourcesEndCallback() {
        //debugger;
        AddNewRowGridResources();

        $('#ContractNo').val('');
        var CotractDate = GetServerDateFormat(CotractDate_dt.GetValue());
        //var BOMDate = $('#BOMDate_dt').val();
        $('#RevisionNo').val('');
        $('#ddlBankBranch').val($("#ddlBankBranch option:first").val());
        $('#ddlWarehouse').val($("#ddlWarehouse option:first").val());
        $('#hdnSchemaId').val('');
        $('#txtActualAdditionalCost').val(parseFloat(0).toFixed(4));
        $('#txtGridProductEntryTotalAmount').val(parseFloat(0).toFixed(2));
        $('#txtGridResourcesTotalAmount').val(parseFloat(0).toFixed(2));


        $('#ddlSchema').val($("#ddlSchema option:first").val());
        $('#ContractNo').val('');
        $('#EContractNo').hide();
        $('#ECotractDate_dt').hide();
        $('#ERevisionNo').hide();
        $('#ERevisionDate_dt').hide();
        $('#EddlBankBranch').hide();
        $('#hdnDetailsID').val(0);

        $('#hdnProposal_ID').val('');
        $('#hdnQuotation_ID').val('');
        $('#txt_HeadRemarks').val('');
        $('#btnQuotation').val('');
        $('#btnProposal').val('');

        Scheme_ValueChange();
        ////debugger;
        if (Message == "duplicate" && DetailsID == 0 && ProductionID == 0) {
            savemode = "";
            if (Message == "duplicate") {
                jAlert('This Onctract no already present!');
                return false;
            }
            else {
                jAlert('Please try again later.');
                return false;
            }
            Message = "";

        }
        else {
            if (DetailsID > 0 && ProductionID > 0 && GContractNo != "") {
                ProductionID = 0;
                DetailsID = 0;
                jAlert('Contract Number : ' + GContractNo + ' saved successfully.', 'Alert!', function (r) {
                    if (r) {
                        if (savemode == "Exit") {
                            setTimeout(function () {
                                var url = $('#hdnContractListPage').val();
                                window.location.href = url;
                            }, 500);
                        }
                    }

                });
                // jAlert('BOM Number : ' + GBOMNo + ' Successfully saved.');


            }
            else {
                ProductionID = 0;
                DetailsID = 0;
                savemode = "";
                jAlert('Please try again later.');
                return false;
            }
            Message = "";
        }

    }

    function OnEndCallback(s, e) {
        ////debugger;
        DetailsID = s.cpDetailsID;
        ProductionID = s.cpProductionID;
        GContractNo = s.cpContractNo;
        Message = s.cpMessage;
        $('#hdnDetailsID').val(DetailsID);
        if (s.cpBatchUpdate == "1") {

            s.cpBatchUpdate = "0";

            if (gridContractResourcesList.batchEditApi.HasChanges()) {
                gridContractResourcesList.UpdateEdit();

                setTimeout(function () {
                    OnResourcesEndCallback();
                }, 1500);

            }
        }
        //else {
        AddNewRowWithSl();
        $('#txtGridProductEntryTotalAmount').val(parseFloat(0).toFixed(2));
        if (Message == "duplicate" && hasmsg == 0) {
            jAlert('This contact no already present!');
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
            if ($("#txtGridProductName").val() != '') {
                if (typemodal == 'nonInventory') {
                    GridNonInventoryProductlist($("#txtGridProductName").val(), "nonInventory", globalrowindex2);
                    gridnonproductlist = 1;
                }
                else {
                    GridProductlist($("#txtGridProductName").val(), typemodal, null);
                    gridproductlist = 1;
                }
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

    function GridBomListkeydown() {

    }

    $(function () {
        PopulateWareHouseData();
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
                        var localcolumn = gridContractProductEntryList.batchEditApi.GetColumnIndex('Price');
                        gridContractProductEntryList.batchEditApi.StartEdit(globalrowindex, localcolumn);
                    }, 500);

                }


                if (Contractlinkindex == 1) {
                    Contractlinkindex = 0;
                    setTimeout(function () {
                        var localcolumn = gridContractProductEntryList.batchEditApi.GetColumnIndex('Remarks');
                        gridContractProductEntryList.batchEditApi.StartEdit(globalrowindex, localcolumn);
                    }, 500);

                }

                if (gridproductlist == 1) {
                    gridproductlist = 0;
                    setTimeout(function () {
                        var localcolumn = gridContractProductEntryList.batchEditApi.GetColumnIndex('ProductQty');
                        gridContractProductEntryList.batchEditApi.StartEdit(globalrowindex, localcolumn);
                    }, 500);

                }

                if (gridnonproductlist == 1) {
                    gridnonproductlist = 0;
                    setTimeout(function () {
                        var localcolumn = gridContractResourcesList.batchEditApi.GetColumnIndex('ProductQty');
                        gridContractResourcesList.batchEditApi.StartEdit(globalrowindex2, localcolumn);
                    }, 500);

                }
            }
            //alert(e.keyCode + "|" + e.altKey);
            if (e.keyCode == 88 && e.altKey == true) {
                ContractEntrySave('Exit');
            }

            if (event.keyCode == 83 && event.altKey == true) {
                ContractEntrySave('New');
            }
        });


    });

    function PopulateWareHouseData() {
        var BankBranchID = $('#ddlBankBranch option:selected').val();
        $.ajax({
            type: "POST",
            url: "@Url.Action("getWarehouseRecord", "ContractOrder")",
            data: { branchid: BankBranchID },
            success: function (response) {
                $('#ddlWarehouse').html('');
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
        var type = 100;
        $.ajax({
            type: "POST",
            url: "@Url.Action("getNumberingSchemeRecord", "ContractOrder")",
            data: { type: type },
            success: function (response) {
                var html = "";
                var hdnContrat_SCHEMAID = $('#hdnContrat_SCHEMAID').val();
                for (var i = 0; i < response.length; i++) {
                    if (hdnContrat_SCHEMAID != '') {
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

    function addNewRowTogridResources() {
        gridContractResourcesList.batchEditApi.EndEdit();
        AddNewRowGridResources();
        index = globalrowindex2;
        setTimeout(function () {
            gridContractResourcesList.batchEditApi.EndEdit();
            gridContractResourcesList.batchEditApi.StartEdit(index, 1);
        }, 200);

    }

    function addNewRowTogrid() {
        gridContractProductEntryList.batchEditApi.EndEdit();
        //gridContractProductEntryList.AddNewRow();
        // gridContractProductEntryList.AddNewRow();
        //

        AddNewRowWithSl();
        index = globalrowindex;


        setTimeout(function () {
            gridContractProductEntryList.batchEditApi.EndEdit();
            gridContractProductEntryList.batchEditApi.StartEdit(index, 1);
        }, 200);

    }

    function ContractGridSetAmount(s, e) {
        gridContractProductEntryList.batchEditApi.EndEdit();
        //var Price = s.GetValueString();
        var Price = gridContractProductEntryList.batchEditApi.GetCellValue(globalrowindex, 'Price');
        var Qty = gridContractProductEntryList.batchEditApi.GetCellValue(globalrowindex, 'ProductQty');
        if (Price != "" && Qty != "") {
            var amount = parseFloat((parseFloat(Qty).toFixed(4)) * (parseFloat(Price).toFixed(2))).toFixed(2);

            gridContractProductEntryList.batchEditApi.StartEdit(globalrowindex, 8);
            gridContractProductEntryList.GetEditor('Amount').SetText(amount);

            if (gridContractProductEntryList.batchEditApi.GetCellValue(globalrowindex, 9) == null) {
                gridContractProductEntryList.batchEditApi.StartEdit(globalrowindex, 9);
                gridContractProductEntryList.GetEditor('ContractNo').SetText(" ");
            }

            if (gridContractProductEntryList.batchEditApi.GetCellValue(globalrowindex, 10) == null) {
                gridContractProductEntryList.batchEditApi.StartEdit(globalrowindex, 10);
                gridContractProductEntryList.GetEditor('RevNo').SetText(" ");
            }

            //if (gridContractProductEntryList.batchEditApi.GetCellValue(globalrowindex, 11) == null) {
            //    gridContractProductEntryList.batchEditApi.StartEdit(globalrowindex, 11);
            //  //  gridContractProductEntryList.GetEditor('RevDate').SetText(" ");
            //}
        }
        //else {
        //    gridContractProductEntryList.batchEditApi.StartEdit(globalrowindex, 7);
        //}
        //gridContractProductEntryList.batchEditApi.EndEdit();
        //gridContractProductEntryList.batchEditApi.StartEdit();
      
    }

    function ContractGridSetAmountQty(s, e) {
        debugger;
        gridContractProductEntryList.batchEditApi.EndEdit();
        //var Price = s.GetValueString();
        var Price = gridContractProductEntryList.batchEditApi.GetCellValue(globalrowindex, 'Price');
        var Qty = gridContractProductEntryList.batchEditApi.GetCellValue(globalrowindex, 'ProductQty');
        if (Price != "" && Qty != "") {
            var amount = parseFloat((parseFloat(Qty).toFixed(4)) * (parseFloat(Price).toFixed(2))).toFixed(2);

            gridContractProductEntryList.batchEditApi.StartEdit(globalrowindex, 8);
            gridContractProductEntryList.GetEditor('Amount').SetText(amount);

            if (gridContractProductEntryList.batchEditApi.GetCellValue(globalrowindex, 9) == null) {
                gridContractProductEntryList.batchEditApi.StartEdit(globalrowindex, 9);
                gridContractProductEntryList.GetEditor('ContractNo').SetText(" ");
            }

            if (gridContractProductEntryList.batchEditApi.GetCellValue(globalrowindex, 10) == null) {
                gridContractProductEntryList.batchEditApi.StartEdit(globalrowindex, 10);
                gridContractProductEntryList.GetEditor('RevNo').SetText(" ");
            }

            //if (gridContractProductEntryList.batchEditApi.GetCellValue(globalrowindex, 11) == null) {
            //    gridContractProductEntryList.batchEditApi.StartEdit(globalrowindex, 11);
            //    gridContractProductEntryList.GetEditor('RevDate').SetText(" ");
            //}


        }


        ContractGridSetTotalAmount("", "");

        gridContractProductEntryList.batchEditApi.EndEdit();
        var localindex = gridContractProductEntryList.batchEditApi.GetColumnIndex('ProductUOM');

        gridContractProductEntryList.batchEditApi.StartEdit(globalrowindex, localindex);
        //else {
        //    gridContractProductEntryList.batchEditApi.StartEdit(globalrowindex, 7);
        //}
        //gridContractProductEntryList.batchEditApi.EndEdit();
        //gridContractProductEntryList.batchEditApi.StartEdit();
        //ContractGridSetTotalAmount(s, e);

    }

    function ContractResourceGridSetAmount(s, e) {
        gridContractResourcesList.batchEditApi.EndEdit();
        //var Price = s.GetValueString();
        var Price = gridContractResourcesList.batchEditApi.GetCellValue(globalrowindex2, 'Price');
        var Qty = gridContractResourcesList.batchEditApi.GetCellValue(globalrowindex2, 'ProductQty');
        if (Price != "" && Qty != "") {
            var amount = parseFloat((parseFloat(Qty).toFixed(4)) * (parseFloat(Price).toFixed(2))).toFixed(2);

            gridContractResourcesList.batchEditApi.StartEdit(globalrowindex2, 8);
            gridContractResourcesList.GetEditor('Amount').SetText(amount);
        }

        ContractGridResourceSetTotalAmount("", "");
    }

    function ContractResourceGridUOMFocus(s, e) {

        if (e.htmlEvent.key == "Tab" && e.htmlEvent.shiftKey) {
            gridContractResourcesList.batchEditApi.EndEdit();
            var localindex = gridContractResourcesList.batchEditApi.GetColumnIndex('ProductQty');

            gridContractResourcesList.batchEditApi.StartEdit(globalrowindex2, localindex);
        }
    }

    function ContractResourceGridSetAmountQty(s, e) {
        gridContractResourcesList.batchEditApi.EndEdit();
        //var Price = s.GetValueString();
        var Price = gridContractResourcesList.batchEditApi.GetCellValue(globalrowindex2, 'Price');
        var Qty = gridContractResourcesList.batchEditApi.GetCellValue(globalrowindex2, 'ProductQty');
        if (Price != "" && Qty != "") {
            var amount = parseFloat((parseFloat(Qty).toFixed(4)) * (parseFloat(Price).toFixed(2))).toFixed(2);

            gridContractResourcesList.batchEditApi.StartEdit(globalrowindex2, 8);
            gridContractResourcesList.GetEditor('Amount').SetText(amount);
        }

        ContractGridResourceSetTotalAmount("", "");

        // gridContractProductEntryList.batchEditApi.EndEdit();
        var localindex = gridContractResourcesList.batchEditApi.GetColumnIndex('ProductUOM');

        gridContractResourcesList.batchEditApi.StartEdit(globalrowindex2, localindex);
    }

    //function FocusGrid() {
    //    gridContractProductEntryList.batchEditApi.StartEdit(-1,0);
    //}

    //function RemarksLostFocus(s, e) {
    //    //gridContractProductEntryList.batchEditApi.StartEdit(globalrowindex, 13);
    //    //$('.addEdcircleBtn').focus();
    //    debugger;


    //}

    var warehousefocus = 0;
    function WarehouseKeyDown(s, e) {
        //  console.log(e.htmlEvent.key);
        if (e.htmlEvent.key == "Enter") {

            s.OnButtonClick(0);

            $('#ddlWarehouselist').focus();
        }
    }

    function PriceKeyDown(s, e) {

        //console.log(e.htmlEvent.key);
        if (e.htmlEvent.key == "Tab" && e.htmlEvent.shiftKey) {
            gridContractProductEntryList.batchEditApi.EndEdit();
            WarehouseGotFocus();
        }
    }

    function WarehouseGotFocus() {

        var localindex = gridContractProductEntryList.batchEditApi.GetColumnIndex('Warehouse');

        gridContractProductEntryList.batchEditApi.StartEdit(globalrowindex, localindex);

    }

    function AddRowResourceKeyDown(s, e) {
        //console.log(e.htmlEvent.key);
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
        //console.log(e.htmlEvent.key);
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
                if (gridContractProductEntryList.batchEditApi.GetCellValue(tempindex, 'SlNO') != null && globalrowindex >= 0 && tempindexcount.includes(tempindex) == false) {
                    tempindexcount.push(tempindex);
                    gridContractProductEntryList.batchEditApi.EndEdit();
                    setTimeout(function () {
                        var localindex = gridContractResourcesList.batchEditApi.GetColumnIndex('ProductName');
                        gridContractProductEntryList.batchEditApi.StartEdit(tempindex, localindex);
                    }, 500);

                    hasfoundindex = 1;
                }
                else {
                    var tempindex = -1;
                    if (gridContractProductEntryList.batchEditApi.GetCellValue(tempindex, 'SlNO') != null && tempindexcount.includes(tempindex) == false) {
                        tempindexcount.push(tempindex);
                        gridContractProductEntryList.batchEditApi.EndEdit();
                        setTimeout(function () {
                            var localindex = gridContractResourcesList.batchEditApi.GetColumnIndex('ProductName');
                            gridContractProductEntryList.batchEditApi.StartEdit(tempindex, localindex);
                        }, 500);


                    }
                }

            }
            else {


                var tempindex = (globalrowindex - 1);
                if (gridContractProductEntryList.batchEditApi.GetCellValue(tempindex, 'SlNO') != null) {
                    gridContractProductEntryList.batchEditApi.EndEdit();

                    setTimeout(function () {
                        var localindex = gridContractResourcesList.batchEditApi.GetColumnIndex('ProductName');
                        gridContractProductEntryList.batchEditApi.StartEdit(tempindex, localindex);
                    }, 500);

                }
                else {
                    var tempindex = (globalrowindex - 1);
                    if (gridContractProductEntryList.batchEditApi.GetCellValue(tempindex, 'SlNO') != null) {

                        gridContractProductEntryList.batchEditApi.EndEdit();
                        setTimeout(function () {
                            var localindex = gridContractResourcesList.batchEditApi.GetColumnIndex('ProductName');
                            gridContractProductEntryList.batchEditApi.StartEdit(tempindex, localindex);
                        }, 500);
                    }
                }
            }
        }
    }

    function ContractKeyDown(s, e) {
        //console.log(e.htmlEvent.key);
        if (e.htmlEvent.key == "Enter") {

            s.OnButtonClick(0);
        }
        //if (e.htmlEvent.key == "Tab") {

        //    s.OnButtonClick(0);
        //}
    }

    function ProductKeyDown(s, e) {
        // console.log(e.htmlEvent.key);
        if (e.htmlEvent.key == "Enter") {

            //if (gridContractProductEntryList.focusedRowIndex != null && gridContractProductEntryList.focusedRowIndex != undefined) {
            //    globalrowindex = gridContractProductEntryList.focusedRowIndex;
            //}
            s.OnButtonClick(0);
            //OpenProductList(s, e);
        }
            //if (e.htmlEvent.key == "Tab") {
            //    //if (gridContractProductEntryList.focusedRowIndex != null && gridContractProductEntryList.focusedRowIndex != undefined) {
            //    //    globalrowindex = gridContractProductEntryList.focusedRowIndex;
            //    //}
            //    s.OnButtonClick(0);
            //    //OpenProductList(s, e);
            //}
        else if (e.code == "ArrowDown") {
            if ($("input[GridProductIndex=0]"))
                $("input[GridProductIndex=0]").focus();
        }
    }

    function SetContractFocusGrid() {
        gridContractProductEntryList.batchEditApi.StartEdit(globalrowindex, 12);
    }

    function NonIProductKeyDown(s, e) {
        //console.log(e.htmlEvent.key);
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

    function btnBOQ_KeyDown(s, e) {
        if (e.htmlEvent.key == "Enter") {
            s.OnButtonClick(0);
        }
        else if (e.code == "ArrowDown") {
            if ($("input[ProductIndex=0]"))
                $("input[ProductIndex=0]").focus();
        }
    }

    function btnEstimate_KeyDown(s, e) {
        if (e.htmlEvent.key == "Enter") {
            s.OnButtonClick(0);
        }
        else if (e.code == "ArrowDown") {
            if ($("input[ProductIndex=0]"))
                $("input[ProductIndex=0]").focus();
        }
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

    function ValueSelected(e, indexName) {

        if (e.code == "Enter") {

            var Id = e.target.parentElement.parentElement.cells[0].innerText;
            var name = e.target.parentElement.parentElement.cells[1].children[0].value;
            if (Id) {
                if (indexName == "ProductIndex") {
                    SetProduct(Id, name, null);
                }
                else if (indexName == "BOMIndex") {
                    SetGridBOMProduct(Id, name, null);
                }
                else if (indexName == "GridProductIndex") {
                    SetGridProduct(Id, name, null);
                }
                else if (indexName == "NonIProductIndex") {
                    SetGridNonInventoryProduct(Id, name, null);
                }
                //else if (indexName == "customeraddressIndex") {
                //    SetCustomeraddress(Id, name);
                //}
                //else {
                //    SetCustomer(Id, name);
                //}
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
                else if (indexName == "BOMIndex")
                    $('#txtBOMName').focus();
                else if (indexName == "GridProductIndex")
                    $('#txtGridProductName').focus();
                else if (indexName == "NonIProductIndex")
                    ('#txtGridProductName').focus();
                //else
                //    $('#txtCustSearch').focus();
            }
        }

    }

    function SetFocusDesc() {
        gridContractProductEntryList.batchEditApi.StartEdit(globalrowindex, 3);
        gridContractResourcesList.batchEditApi.StartEdit(globalrowindex2, 3);
    }

    function ContractEntrySave(mode) {
       // debugger;
        savemode = mode;
        hasmsg = 0;
        var ContractNo = $('#ContractNo').val();
        var CotractDate = GetServerDateFormat(CotractDate_dt.GetValue());
        var FinishedItem = $('#hdnFinishedItem').val();
        var FinishedQty = $('#FinishedQty').val();
        var FinishedUom = $('#FinishedUom').val();
        var RevisionNo = $('#RevisionNo').val();
        var RevisionDate = GetServerDateFormat(RevisionDate_dt.GetValue());
        var Unit = $('#ddlBankBranch option:selected').val();
        var WarehouseID = $('#ddlWarehouse option:selected').val();
        var SchemaID = $('#hdnSchemaId').val();
        var ActualAdditionalCost = $('#txtActualAdditionalCost').val();
        if (ActualAdditionalCost == '') {
            ActualAdditionalCost = parseFloat(0).toFixed(2);
            $('#txtActualAdditionalCost').val(ActualAdditionalCost);
        }

        var hdnRevisionNo = $('#hdnRevisionNo').val();
        var hdnDetailsID = $('#hdnDetailsID').val();


        var oneproduct = "";
        var visiablerow = gridContractProductEntryList.GetVisibleRowsOnPage();
        if (visiablerow > 0) {
            for (var i = 100; i > -500; i--) {
                if (gridContractProductEntryList.GetRow(i)) {

                    if (oneproduct == "" || oneproduct == null) {
                        oneproduct = gridContractProductEntryList.batchEditApi.GetCellValue(i, 'ProductName');
                    }
                }
            }
        }
        if (hdnDetailsID == "") {
            RevisionNo = " ";
            RevisionDate = GetServerDateFormat(new Date);
        }

        if (oneproduct != "" && oneproduct != null) {

            if (hdnDetailsID > 0 && hdnRevisionNo == RevisionNo) {
                jAlert("Please enter new revision number to save.");
                return false;
            }
            else {

                if (ContractNo != '' && CotractDate != '' && RevisionNo != '' && RevisionDate != '' && Unit != '' && ActualAdditionalCost != '') {

                    if (hdnDetailsID > 0 && RevisionNo != "") {

                        $.ajax({
                            type: "POST",
                            url: "@Url.Action("ProcessWithRevisionNumber", "ContractOrder")",
                            data: { detailsid: hdnDetailsID, RevisionNo: RevisionNo },
                            success: function (response) {

                                if (response) {

                                    SuffleRows();
                                    SuffleRowsGrid2();
                                    gridContractProductEntryList.UpdateEdit();
                                    gridContractProductEntryList.UpdateEdit();
                                }
                                else {
                                    jAlert("Please enter new revision number to save.");
                                    return false;
                                }

                            }
                        });

                    }
                    else {

                        SuffleRows();
                        SuffleRowsGrid2();
                        gridContractProductEntryList.UpdateEdit();
                        gridContractProductEntryList.UpdateEdit();
                    }
                }
                else {
                    savemode = "";
                    if (ContractNo == '') {
                        $('#EContractNo').show();
                    }
                    else {
                        $('#EContractNo').hide();
                    }
                    if (CotractDate == '') {
                        $('#ECotractDate_dt').show();
                    }
                    else {
                        $('#ECotractDate_dt').hide();
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
                    return false;
                }

            }
        }
        else {
            $("html, body").animate({ scrollTop: 0 }, 600);
            savemode = "";
            if (ContractNo == '') {
                $('#EContractNo').show();
            }
            else {
                $('#EContractNo').hide();
            }
            if (CotractDate == '') {
                $('#ECotractDate_dt').show();
            }
            else {
                $('#ECotractDate_dt').hide();
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

            return false;
        }
    }

    function OnResourcesStartCallback(s, e) {

        var strContractNo = $('#ContractNo').val();
        var CotractDate = GetServerDateFormat(CotractDate_dt.GetValue());
        //var BOMDate = $('#BOMDate_dt').val();
        var BOQID = $('#hdnBOQID').val();
        var Estimate_ID = $('#hdnEstimate_ID').val();
        var Proposal_ID = $('#hdnProposal_ID').val();
        var Quotation_ID = $('#hdnQuotation_ID').val();
        var RevisionNo = $('#RevisionNo').val();
        var RevisionDate = GetServerDateFormat(RevisionDate_dt.GetValue());
        var HeadRemarks = $('#txt_HeadRemarks').val();
        var Unit = $('#ddlBankBranch option:selected').val();
        var WarehouseID = $('#ddlWarehouse option:selected').val();
        var SchemaID = $('#hdnSchemaId').val();
        var ActualAdditionalCost = $('#txtActualAdditionalCost').val();

        var hdnDetailsID = $('#hdnDetailsID').val();
        if (hdnDetailsID > 0) {
            DetailsID = hdnDetailsID;
        }
        if (hdnDetailsID == "") {

            RevisionDate = GetServerDateFormat(new Date);
        }


        if (e != undefined) {
            e.customArgs["strCotractNo"] = strContractNo;
            e.customArgs["CotractDate"] = CotractDate;
            e.customArgs["RevisionNo"] = RevisionNo; //EmployeesCounterTargetList

            e.customArgs["RevisionDate"] = RevisionDate;
            e.customArgs["Unit"] = Unit;
            e.customArgs["WarehouseID"] = WarehouseID;
            e.customArgs["Contract_SCHEMAID"] = SchemaID;
            e.customArgs["ActualAdditionalCost"] = ActualAdditionalCost;

            e.customArgs["ProductionID"] = ProductionID;
            e.customArgs["DetailsID"] = DetailsID;

            e.customArgs["BOQ_ID"] = BOQID;
            e.customArgs["Estimate_ID"] = Estimate_ID;
            e.customArgs["Proposal_ID"] = Proposal_ID;
            e.customArgs["Quotation_ID"] = Quotation_ID;
            e.customArgs["HeadRemarks"] = HeadRemarks;
        }


    }

    function OnStartCallback(s, e) {

        var strContractNo = $('#ContractNo').val();
        var CotractDate = GetServerDateFormat(CotractDate_dt.GetValue());
        //var BOMDate = $('#BOMDate_dt').val();
        var BOQID = $('#hdnBOQID').val();   
        var Estimate_ID = $('#hdnEstimate_ID').val();
        var Proposal_ID = $('#hdnProposal_ID').val();
        var Quotation_ID = $('#hdnQuotation_ID').val();
        var HeadRemarks = $('#txt_HeadRemarks').val();
        var RevisionNo = $('#RevisionNo').val();
        var RevisionDate = GetServerDateFormat(RevisionDate_dt.GetValue());
        //var RevisionDate = $('#RevisionDate_dt').val();
        var Unit = $('#ddlBankBranch option:selected').val();
        var WarehouseID = $('#ddlWarehouse option:selected').val();
        var SchemaID = $('#hdnSchemaId').val();
        var ActualAdditionalCost = $('#txtActualAdditionalCost').val();

        var hdnDetailsID = $('#hdnDetailsID').val();
        if (hdnDetailsID > 0) {
            DetailsID = hdnDetailsID;
        }

        if (hdnDetailsID == "") {

            RevisionDate = GetServerDateFormat(new Date);
        }

        if (e != undefined) {
            e.customArgs["strCotractNo"] = strContractNo;
            e.customArgs["CotractDate"] = CotractDate;
            e.customArgs["RevisionNo"] = RevisionNo; //EmployeesCounterTargetList

            e.customArgs["RevisionDate"] = RevisionDate;
            e.customArgs["Unit"] = Unit;
            e.customArgs["Contract_SCHEMAID"] = SchemaID;
            e.customArgs["ActualAdditionalCost"] = ActualAdditionalCost;

            e.customArgs["ProductionID"] = ProductionID;
            e.customArgs["DetailsID"] = DetailsID;


            e.customArgs["BOQ_ID"] = BOQID;
            e.customArgs["Estimate_ID"] = Estimate_ID;
            e.customArgs["Proposal_ID"] = Proposal_ID;
            e.customArgs["Quotation_ID"] = Quotation_ID;
            e.customArgs["HeadRemarks"] = HeadRemarks;
        }



    }

    function Scheme_ValueChange() {
       // debugger;
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
        document.getElementById("ContractNo").maxLength = schemelength;
        CotractDate_dt.SetDate(dt);

        if (dt < new Date(fromdate)) {
            CotractDate_dt.SetDate(new Date(fromdate));
        }

        if (dt > new Date(todate)) {
            CotractDate_dt.SetDate(new Date(todate));
        }




        CotractDate_dt.SetMinDate(new Date(fromdate));
        CotractDate_dt.SetMaxDate(new Date(todate));

        

        if (branchID > 0) {
            $('#ddlBankBranch').val(branchID);
        }

        if (schemetype == '0') {
            $('#ContractNo').removeAttr("disabled");
            $('#ContractNo').val('');

            $('#ContractNo').focus();
        }
        else if (schemetype == '1') {
            $('#ContractNo').attr("disabled", "disabled");
            $('#ContractNo').val('Auto');

          

        }
        else if (schemetype == '2') {
            $('#ContractNo').attr("disabled", "disabled");
            $('#ContractNo').val('Datewise');

          

        }
        else if (schemetype == 'n') {
            $('#ContractNo').attr("disabled", "disabled");
            $('#ContractNo').val('');

           
        }
        else {
            $('#ContractNo').attr("disabled", "disabled");
            $('#ContractNo').val('');

           

        }

    }

    function SuffleRows() {
        for (var i = 0; i < 1000; i++) {
            if (gridContractProductEntryList.GetRow(i)) {
                if (gridContractProductEntryList.GetRow(i).style.display != "none") {
                    gridContractProductEntryList.batchEditApi.StartEdit(i, 14);
                    gridContractProductEntryList.GetEditor("UpdateEdit").SetText(i);
                }
            }
        }

        for (i = -1; i > -1000; i--) {
            if (gridContractProductEntryList.GetRow(i)) {
                if (gridContractProductEntryList.GetRow(i).style.display != "none") {
                    gridContractProductEntryList.batchEditApi.StartEdit(i, 14);
                    gridContractProductEntryList.GetEditor("UpdateEdit").SetText(i);
                }
            }
        }
    }

    function SuffleRowsGrid2() {
        for (var i = 0; i < 1000; i++) {
            if (gridContractResourcesList.GetRow(i)) {
                if (gridContractResourcesList.GetRow(i).style.display != "none") {
                    gridContractResourcesList.batchEditApi.StartEdit(i, 13);
                    gridContractResourcesList.GetEditor("UpdateEdit").SetText(i);
                }
            }
        }

        for (i = -1; i > -1000; i--) {
            if (gridContractResourcesList.GetRow(i)) {
                if (gridContractResourcesList.GetRow(i).style.display != "none") {
                    gridContractResourcesList.batchEditApi.StartEdit(i, 13);
                    gridContractResourcesList.GetEditor("UpdateEdit").SetText(i);
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

        var hdnDetailsID = $('#hdnDetailsID').val();
        if (hdnDetailsID > 0) {
            $('#ContractNo').attr("disabled", "disabled");
            $('#ddlSchema').attr("disabled", "disabled");
            $('#ddlBankBranch').attr("disabled", "disabled");
            CotractDate_dt.SetEnabled(false);
            //RevisionDate_dt.SetEnabled(false);
            var ContractResourcesTotal = $('#ContractResourcesTotalAm').val();
            if (ContractResourcesTotal != "" && ContractResourcesTotal != undefined) {
                $('#txtGridResourcesTotalAmount').val(parseFloat(ContractResourcesTotal).toFixed(2));
            }

            var ContractEntryProductsTotal = $('#hdnContractResourcesTotalAm').val();
            if (ContractEntryProductsTotal != "" && ContractEntryProductsTotal != undefined) {
                $('#txtGridProductEntryTotalAmount').val(parseFloat(ContractEntryProductsTotal).toFixed(2));
            }
            //$('#ddlSchema').val($('#hdnContract_SCHEMAID').val());
            var hdnContractResourcesTotalAm = $('#hdnContractResourcesTotalAm').val();
            if (hdnContractResourcesTotalAm != "" && hdnContractResourcesTotalAm != undefined) {
                $('#txtGridResourcesTotalAmount').val(parseFloat(hdnContractResourcesTotalAm).toFixed(2));
            }


            $('#btnSaveandNew').hide();

            setTimeout(function () { var noofrow = gridContractResourcesList.GetVisibleRowsOnPage(); if (noofrow > 1) { $('#showResources').click(); } }, 800);

        }
        else {
            //$("#BOMNo").removeAttr("disabled");
            $("#ddlSchema").removeAttr("disabled");
            //$('#ddlBankBranch').removeAttr("disabled");
            CotractDate_dt.SetEnabled(true);
            RevisionDate_dt.SetEnabled(true);
            $('#btnSaveandNew').show();
            RevisionDate_dt.SetDate(null);
            $('#hdnContrat_SCHEMAID').val('');
        }


        $('#GridWarehouselistModel').on('shown.bs.modal', function () {
            $('#ddlWarehouselist').focus();
        })

    });


    function datevalidateTo() {

        if (CotractDate_dt.GetDate()) {
            if (RevisionDate_dt.GetDate() <= CotractDate_dt.GetDate()) {
                if ($('#hdnDetailsID').val() != "") {
                    RevisionDate_dt.SetValue(CotractDate_dt.GetDate());
                    RevisionDate_dt.SetMinDate(CotractDate_dt.GetDate());
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
        //gridContractProductEntryList.Refresh();
       // gridContractProductEntryList.Refresh();
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
            gridContractResourcesList.batchEditApi.StartEdit(globalrowindex2, 2);

            $(this).hide();
        });

        $('#closeResource').click(function () {
            jConfirm('Are you sure to close? Clicking on "Yes" will clear the data from grid.', 'Alert!', function (r) {
                if (r) {
                    $('#refreshgrid2').hide();
                    $('#showResources').show();
                    for (var i = 500; i > -500; i--) {
                        if (gridContractResourcesList.GetRow(i)) {
                            gridContractResourcesList.DeleteRow(i);
                        }
                    }
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
            ContractEntrySave('Exit');
        }
        if (event.keyCode == 83 && event.altKey == true) { //  && myModal.GetVisible() == true
            ContractEntrySave('New');
        }
    }

    $(document).ready(function () {


        var hdnDetailsID = $('#hdnDetailsID').val();
        if (hdnDetailsID == "") {
            $('#FinishedQty').val(parseFloat(0).toFixed(4));
            setTimeout(function () { RevisionDate_dt.SetDate(null); $('#ddlSchema').focus(); }, 900);
        }
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

        $("#ddlWarehouse").focusout(function () {
            gridContractProductEntryList.batchEditApi.EndEdit();
            gridContractProductEntryList.batchEditApi.StartEdit(globalrowindex, 2);
        });
    });

    function SetFocusQty() {
        $('#FinishedQty').select();
        $('#FinishedQty').focus();
    }
