﻿<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="ProjectProductsOpeningEntries.aspx.cs" Inherits="ERP.OMS.Management.Master.ProjectProductsOpeningEntries" 
    EnableEventValidation="false" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>
<%@ Register Src="~/OMS/Management/Activities/UserControls/StockMultiLevelWareHouse.ascx" TagPrefix="ucWH" TagName="MultiWarehouceuc" %>
<%@ Register Src="~/OMS/Management/Activities/UserControls/UOMConversion.ascx" TagPrefix="uc3" TagName="UOMConversionControl" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="http://cdn.datatables.net/1.10.19/css/jquery.dataTables.min.css" />
    <script src="http://cdn.datatables.net/1.10.19/js/jquery.dataTables.min.js"></script>
    <script type='text/javascript'>
        var SecondUOM = [];
        var SecondUOMProductId = "";
    </script>
    <%--<script src="../Activities/JS/ProductStockIN.js?v1.00.00.08"></script>--%>
    <script src="../Activities/JS/ProjectProductOpeningStockIn.js?var=1.5"></script>
    <link href="../Activities/CSS/SearchPopup.css" rel="stylesheet" />
    <%--<script src="../Activities/JS/StockINMultiWarehouse.js"></script>--%>

    <style>
        #dataTbl .dataTables_empty {
            display: none;
        }

        .padRight15 {
            padding-right: 15px;
        }

        .padTop23 {
            padding-top: 23px;
        }

        .dynamicPopupTbl > thead > tr > th, .dynamicPopupTbl > tbody > tr > td {
            padding: 5px 8px !important;
        }

        .dynamicPopupTbl.scroll > thead > tr {
            padding-right: 17px;
        }

        .dynamicPopupTbl > tbody > tr {
            display: flex;
        }

            .dynamicPopupTbl > tbody > tr > td {
                cursor: pointer;
            }

        .dynamicPopupTbl.back > thead > tr > th {
            background: #4e64a6;
            color: #fff;
        }

        .dynamicPopupTbl.back > tbody > tr > td {
            background: #fff;
        }

        .dynamicPopupTbl > tbody > tr > td input {
            border: none !important;
            cursor: pointer;
            background: transparent !important;
        }

        .focusrow {
            background-color: #0000ff3d;
        }

        .HeaderStyle {
            background-color: #180771d9;
            color: #f5f5f5;
        }

        .gridStatic {
            margin-top: 25px;
        }

            .gridStatic .dynamicPopupTbl {
                width: 100%;
            }

        .bodBot {
            border-bottom: 1px solid #ccc;
            padding-bottom: 10px;
        }

        table.scroll {
            /* width: 100%; */ /* Optional */
            /* border-collapse: collapse; */
            border-spacing: 0;
            width: 100%;
        }

            table.scroll tbody,
            table.scroll thead {
                display: block;
            }

        thead tr th {
            height: 30px;
            line-height: 30px;
            /* text-align: left; */
        }

        table.scroll tbody {
            height: 250px;
            overflow-y: auto;
            overflow-x: hidden;
        }

        .tp2 {
            right: -4px;
            top: 17px;
            position: absolute;
        }
    </style>
    <script>
        $(document).ready(function () {
            var setting = document.getElementById("hdnShowUOMConversionInEntry").value;
            //alert(setting);
            if (setting == 1) {
                cAltertxtQty1.SetVisible(true);
                ccmbPackingUom1.SetVisible(true);
                document.getElementById("lblaltqty").style.display = "block";
                document.getElementById("lblaltuom").style.display = "block";
            }
            else {
                document.getElementById("lblaltqty").style.display = "none";
                document.getElementById("lblaltuom").style.display = "none";
                cAltertxtQty1.SetVisible(false);
                ccmbPackingUom1.SetVisible(false);
            }

        });
    </script>
    <script>
        var currentEditableVisibleIndex;

        $(function () {
            if ($("#hdnShowUOMConversionInEntry").val() == "1") {
                $("#btnSecondUOM").removeClass('hide');
            }
            else {
                $("#btnSecondUOM").addClass('hide');
            }

            AltQtyModule = "Opening Balances";
        });


        function OnKeyDown(s, e) {
            if (e.htmlEvent.keyCode == 40 || e.htmlEvent.keyCode == 38)
                return ASPxClientUtils.PreventEvent(e.htmlEvent);
        }

        function OnBatchStartEdit(s, e) {
            currentEditableVisibleIndex = e.visibleIndex;
        }

        function OnEndCallback(s, e) {
            if (OpeningGrid.cpfinalMsg == "checkWarehouse") {
                OpeningGrid.Refresh();
                OpeningGrid.cpfinalMsg = null;
                jAlert("You must enter Quantity and Opening Quantity <br/>as same value to proceed further.");
            }
            else if (OpeningGrid.cpfinalMsg == "errorrInsert") {
                OpeningGrid.cpfinalMsg = null;
                jAlert("Try after sometime.");
            }
            else if (OpeningGrid.cpfinalMsg == "SuccesInsert") {
                GetObjectID('hdnJsonProductStock').value = "";
                OpeningGrid.cpfinalMsg = null;
                cPopup_Warehouse.Hide();
                jAlert("Document has been saved successfully.");
            }
            else if (OpeningGrid.cpfinalMsg == "nullStock") {
                GetObjectID('hdnJsonProductStock').value = "";
                OpeningGrid.cpfinalMsg = null;
                cPopup_Warehouse.Hide();
            }
            else if (OpeningGrid.cpfinalMsg == "negativestock") {
                GetObjectID('hdnJsonProductStock').value = "";
                OpeningGrid.cpfinalMsg = null;
                cPopup_Warehouse.Hide();
                jAlert("Current stock can't be less than available stock.");
            }
            if (OpeningGrid.cpTotalSum != null) {
                var TotalSum = OpeningGrid.cpTotalSum;
                OpeningGrid.cpTotalSum = null;
                document.getElementById('<%=lblTotalSum.ClientID %>').innerHTML = TotalSum;
            }

            if (OpeningGrid.cpMessage == "Generated") {
                OpeningGrid.cpMessage = null;
                LoadingPanel.Hide();
                jAlert('Barcode has been generated successfully.');
            }
            else if (OpeningGrid.cpMessage == "NullStock") {
                OpeningGrid.cpMessage = null;
                LoadingPanel.Hide();
                jAlert('Cannot proceed as no stock available.');
            }
            else if (OpeningGrid.cpMessage == "NullBarcode") {
                OpeningGrid.cpMessage = null;
                LoadingPanel.Hide();
                jAlert('No quantity is pending for Barcode Generation.');
            }
            else if (OpeningGrid.cpMessage == "BarcodeInactive") {
                OpeningGrid.cpMessage = null;
                LoadingPanel.Hide();
                jAlert('You must activate Barcode from <br/><u>Masters - Product and Services - Inventory Configuration</u><br/>to proceed further.');
            }
            else if (OpeningGrid.cpMessage == "Error") {
                OpeningGrid.cpMessage = null;
                LoadingPanel.Hide();
                jAlert('Please try again later');
            }
            else if (OpeningGrid.cpMessage == "BarcodeNotPresent") {
                OpeningGrid.cpMessage = null;
                LoadingPanel.Hide();
                jAlert('You must activate Barcode from <br/><u>Masters - Product and Services - Inventory Configuration</u><br/>to proceed further.');
            }
            else if (OpeningGrid.cpMessage == "StockNotPresent") {
                OpeningGrid.cpMessage = null;
                LoadingPanel.Hide();
                jAlert('Cannot proceed as no stock available.');
            }
            else if (OpeningGrid.cpMessage == "BarcodeStockPresent") {
                OpeningGrid.cpMessage = null;
                LoadingPanel.Hide();

                var visibleIndex = OpeningGrid.GetFocusedRowIndex();
                var key = OpeningGrid.GetRowKey(visibleIndex);
                var Branch = ccmbbranch.GetValue();
                var doctype = 'OP';
                var module = 'BRCODE';
                var reportName = 'Barcode~D';
                window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + reportName + "&modulename=BRCODE&id=" + key + "&doctype=" + doctype + "&Branch=" + Branch, '_blank')
            }


        }
    </script>
    <script>
        function ddlWarehouse_ValueChange() {
            var WarehouseID = $('#ddlWarehouse').val();

            var List = $.grep(warehouserateList, function (e) { return e.WarehouseID == defaultWarehouse; })
            if (List.length > 0) {
                var Rate = List[0].Rate;
                ctxtRate.SetValue(Rate);
            }
            else {
                ctxtRate.SetValue("0");
            }
        }

        function CheckProductStockdetails(ProductID, ProductName, UOM, VisibleIndex, DefaultWarehouse, Warehousetype, OpeningStock, Openingvalue) {
            document.getElementById("hdnaddedit").value = "Add";
            ctxtvalue.SetValue('');
            SecondUOM = [];
            if ($("#hdnmultiwarehouse").val() == "111") {
                ucCheckProductStockdetails(ProductID, ProductName, UOM, VisibleIndex, DefaultWarehouse, Warehousetype, OpeningStock, ccmbbranch.GetValue());
                return;
            }

            if ($("#hdnShowUOMConversionInEntry").val() == "1") {
                var objectToPass = {}
                objectToPass.ProductID = ProductID;
                $.ajax({
                    type: "POST",
                    url: "../Activities/Services/Master.asmx/GetUom",
                    data: JSON.stringify(objectToPass),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        var returnObject = msg.d;
                        var UOMId = msg.UOM_Id;
                        var UOMName = msg.UOM_Name;
                        if (returnObject) {
                            SetDataSourceOnComboBox(ccmbPackingUom1, returnObject.uom);
                            ccmbPackingUom1.SetValue(returnObject.uom_id);
                            ccmbPackingUom1.SetEnabled(false);
                        }
                    }
                });

            }

            function SetDataSourceOnComboBox(ControlObject, Source) {
                ControlObject.ClearItems();
                for (var count = 0; count < Source.length; count++) {
                    ControlObject.AddItem(Source[count].UOM_Name, Source[count].UOM_Id);                   
                }
                ControlObject.SetSelectedIndex(0);
            }            
            GetObjectID('hdnIsPopUp').value = "Y";
            var Branch = ccmbbranch.GetValue();
            var GetserviceURL = "../Activities/Services/Master.asmx/GetProjectOpeningStockDetails";
            var serviceURL = "../Activities/Services/Master.asmx/CheckDuplicateSerial";        
            GetObjectID('hdfProductSrlNo').value = ProductID;
            GetObjectID('hdfProductID').value = ProductID;
            GetObjectID('hdfWarehousetype').value = Warehousetype;
            GetObjectID('hdndefaultWarehouse').value = DefaultWarehouse;
            GetObjectID('hdfUOM').value = UOM;
            GetObjectID('hdfServiceURL').value = serviceURL;
            GetObjectID('hdfBranch').value = Branch;
            GetObjectID('hdfIsRateExists').value = "Y";
            GetObjectID('hdnvalue').value = Openingvalue;         
            OpeningStock = parseFloat(OpeningStock).toFixed(4);

            document.getElementById('<%=lblProductName.ClientID %>').innerHTML = ProductName.replace("squot", "'").replace("coma", ",").replace("slash", "/");
            document.getElementById('<%=txt_EnteredSalesAmount.ClientID %>').innerHTML = "0.0000";
            document.getElementById('<%=txt_EnteredSalesUOM.ClientID %>').innerHTML = UOM;          
            document.getElementById('<%=lblAvailableStockUOM.ClientID %>').innerHTML = UOM;

            var objectToPass = {}
            objectToPass.ProductID = ProductID;
            objectToPass.BranchID = Branch;

            $.ajax({
                type: "POST",
                url: GetserviceURL,
                data: JSON.stringify(objectToPass),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var returnObject = msg.d;
                    var myObj = returnObject.ProjectProductStockDetails;
                    var RateList = returnObject.WarehouseRate;
                    var BlockList = returnObject.StockBlock;

                    StockOfProduct = [];
                    warehouserateList = [];

                    if (BlockList.length > 0) {
                        for (x in BlockList) {
                            GetObjectID('IsStockBlock').value = BlockList[x]["IsStockBlock"];
                            GetObjectID('AvailableQty').value = parseFloat(BlockList[x]["AvailableQty"]).toFixed(4);
                            GetObjectID('CurrentQty').value = parseFloat(OpeningStock).toFixed(4);
                        }
                    }

                    if (RateList.length > 0) {
                        for (x in RateList) {
                            var Rates = { WarehouseID: RateList[x]["WarehouseID"], Rate: parseFloat(RateList[x]["Rate"]).toFixed(2) }
                            warehouserateList.push(Rates);
                        }
                    }
                    if (myObj != null)
                    {
                        if (myObj.length > 0) {
                            for (x in myObj) {
                                var ProductStock = {
                                    Product_SrlNo: myObj[x]["Product_SrlNo"], SrlNo: parseInt(myObj[x]["SrlNo"]), WarehouseID: myObj[x]["WarehouseID"], WarehouseName: myObj[x]["WarehouseName"],
                                    Quantity: myObj[x]["Quantity"], SalesQuantity: myObj[x]["SalesQuantity"], Batch: myObj[x]["Batch"], MfgDate: myObj[x]["MfgDate"], ExpiryDate: myObj[x]["ExpiryDate"],
                                    Rate: myObj[x]["Rate"], Value: myObj[x]["Value"], SerialNo: myObj[x]["SerialNo"], Barcode: myObj[x]["Barcode"], ViewBatch: myObj[x]["Batch"],
                                    ViewMfgDate: myObj[x]["MfgDate"], ViewExpiryDate: myObj[x]["ExpiryDate"], ViewRate: myObj[x]["Rate"],
                                    IsOutStatus: myObj[x]["IsOutStatus"], IsOutStatusMsg: myObj[x]["IsOutStatusMsg"], LoopID: parseInt(myObj[x]["LoopID"]), Status: myObj[x]["Status"], AlterQty: myObj[x]["AlterQty"],
                                    AltUOM: myObj[x]["AltUOM"], ProjectID: myObj[x]["ProjectID"], ProjectCode: myObj[x]["ProjectCode"], HierarchyID: myObj[x]["HierarchyID"], Hierarchy: myObj[x]["Hierarchy"]
                                }
                                StockOfProduct.push(ProductStock);
                            }

                            StockOfProduct.sort(sortByMultipleKey(['LoopID', 'SrlNo']));
                            StockOfProduct = ReGenerateStock(StockOfProduct);

                            CreateStock();
                            cPopup_Warehouse.Show();
                            clookup_Project.gridView.Refresh();
                        }
                        else
                        {
                            CreateStock();
                            cPopup_Warehouse.Show();
                        }
                    }
                   
                    else {
                        CreateStock();
                        cPopup_Warehouse.Show();
                    }
                }
            });


            GetSecondUONEditDetails(ProductID, Branch);



        }

        function closeWarehouse(s, e) {
            GetObjectID('hdnIsPopUp').value = "";
            e.cancel = false;
        }

        function ReGenerateStock(myObj) {
            var Previous_LoopID = "";
            for (x in myObj) {
                var Current_LoopID = myObj[x]["LoopID"];

                if (Current_LoopID == Previous_LoopID) {
                    myObj[x]["WarehouseName"] = "";
                    myObj[x]["SalesQuantity"] = "";
                    myObj[x]["ViewBatch"] = "";
                    myObj[x]["ViewMfgDate"] = "";
                    myObj[x]["ViewExpiryDate"] = "";
                }

                Previous_LoopID = myObj[x]["LoopID"];
            }

            return myObj;
        }

        function FullnFinalSave() {

            if (issavePacking == 1) {

                var UniqueArr = [];
                SaveSendUOM('POE');
                if (aarrUOM.length > 0) {
                    $.ajax({
                        type: "POST",
                        url: "ProjectProductsOpeningEntries.aspx/SetSessionPacking",
                        data: "{'list':'" + JSON.stringify(aarrUOM) + "'}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (msg) {
                            aarrUOM = [];
                            aarr = [];
                            GetObjectID('hdnIsPopUp').value = "";
                            var JsonProductStock = JSON.stringify(StockOfProduct);
                            GetObjectID('hdnJsonProductStock').value = JsonProductStock;

                            OpeningGrid.PerformCallback('FinalSubmit');
                        }
                    });
                }
                else {
                    SaveSendUOM('POE');
                    GetObjectID('hdnIsPopUp').value = "";
                    var JsonProductStock = JSON.stringify(StockOfProduct);
                    GetObjectID('hdnJsonProductStock').value = JsonProductStock;

                    OpeningGrid.PerformCallback('FinalSubmit');
                }
            }
            else {
                SaveSendUOM('POE');
                GetObjectID('hdnIsPopUp').value = "";
                var JsonProductStock = JSON.stringify(StockOfProduct);
                GetObjectID('hdnJsonProductStock').value = JsonProductStock;
                OpeningGrid.PerformCallback('FinalSubmit');
            }
            document.getElementById("hdnaddedit").value = "Add";//---Rajdip---
        }
    </script>
    <script>
        function chnagedcombo(s) {
           // document.getElementById('drdExport').value = "0";

            if (GetObjectID('hdfIsBarcodeActive').value == "Y") {
                if (ccmbbranch.GetValue() == "0" || ccmbbranch.GetValue() == "00") {
                    if (cbtnGenerate != null) cbtnGenerate.SetVisible(false);
                    if (cbtnPrint != null) cbtnPrint.SetVisible(false);
                }
                else {
                    if (cbtnGenerate != null) cbtnGenerate.SetVisible(true);
                    if (cbtnPrint != null) cbtnPrint.SetVisible(true);
                }
            }

            //OpeningGrid.PerformCallback("ReBindGrid");
            cCallbackPanel.PerformCallback("GridBindByBranch");
            clookup_Project.gridView.Refresh();
        }

        function GenerateBarcode() {
            LoadingPanel.Show();
            LoadingPanel.SetText("Please wait...");

            var visibleIndex = OpeningGrid.GetFocusedRowIndex();
            var key = OpeningGrid.GetRowKey(visibleIndex);

            if (visibleIndex != -1) {
                OpeningGrid.PerformCallback("GenerateBarcode~" + key);
            }
            else {
                LoadingPanel.Hide();
                jAlert('No data available.');
            }
        }
        function PrintBarcode() {
            //LoadingPanel.Show();
            //LoadingPanel.SetText("Please wait...");

            var visibleIndex = OpeningGrid.GetFocusedRowIndex();
            var key = OpeningGrid.GetRowKey(visibleIndex);
            //var Branch = ccmbbranch.GetValue();
            var doctype = 'OP';
            //var module = 'BRCODE';
            //var reportName = 'Barcode~N';

            OpeningGrid.PerformCallback("PrintBarcode~" + key + "~" + doctype);
            //window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=Barcode~N&modulename=BRCODE&id=" + key + "&doctype=" + doctype + "&Branch=" + Branch, '_blank')

        }
    </script>
    <script>
        document.onkeydown = function (e) {
            if (event.keyCode == 88 && event.altKey == true && GetObjectID('hdnIsPopUp').value == "Y") { //run code for ALT+X -- ie, Save & Exit!     
                StopDefaultAction(e);
                FullnFinalSave();
            }
        }

        function StopDefaultAction(e) {
            if (e.preventDefault) { e.preventDefault() }
            else { e.stop() };

            e.returnValue = false;
            e.stopPropagation();
        }

        function CallbackPanelEndCallBack(s, e) {
            if (GetObjectID('hdfIsBarcodeActive').value == "Y") {
                if (ccmbbranch.GetValue() == "0" || ccmbbranch.GetValue() == "00") {
                    if (cbtnGenerate != null) cbtnGenerate.SetVisible(false);
                    if (cbtnPrint != null) cbtnPrint.SetVisible(false);
                }
                else {
                    if (cbtnGenerate != null) cbtnGenerate.SetVisible(true);
                    if (cbtnPrint != null) cbtnPrint.SetVisible(true);
                }
            }
        }

        //Rajdip---------------
        function Setquantity() {
            var StockType = GetObjectID('hdfWarehousetype').value;
            var qty = document.getElementById("txtQuantity").value;
            if (StockType == "WS") {
            }
            else {
                var rate = ctxtRate.GetValue();
                var quantity = ctxtQty.GetValue();
                var Value = rate * qty * (1.00);
                ctxtvalue.SetValue(Value);
            }
        }
        function ValueGotFocus() {
            var StockType = GetObjectID('hdfWarehousetype').value;
            var qty = document.getElementById("txtQuantity").value;
            if (StockType == "WS") {
            }
            else {
                var rate = ctxtRate.GetValue();
                var quantity = ctxtQty.GetValue();
                var Value = rate * qty;
                ctxtvalue.SetValue(Value);
            }
        }
        function QuantityLostFocus() {
            var StockType = GetObjectID('hdfWarehousetype').value;
            var qty = document.getElementById("txtQuantity").value;
            if (StockType == "WS" || StockType == "WBS") {
                var rate = ctxtRate.GetValue();
                var qty = 1;
                var Value = rate * qty;
                ctxtvalue.SetValue(Value);

            }
            else {
                var rate = ctxtRate.GetValue();
                var quantity = ctxtQty.GetValue();
                var Value = rate * quantity;
                ctxtvalue.SetValue(Value);
            }
        }
        function RateGotFocus() {
            var qty = document.getElementById("txtQuantity").value;
            var StockType = GetObjectID('hdfWarehousetype').value;
            if (StockType == "WS" || StockType == "WBS") {
                qty = 1;
                var value = ctxtvalue.GetValue();
                var rate = value / qty;
                ctxtRate.SetValue(rate);
            }
            else {
                var quantity = ctxtQty.GetValue();
                var value = ctxtvalue.GetValue();
                if (qty == "0.00") {
                    qty = quantity;
                }
                else {
                    qty = quantity;
                    var rate = value / qty;
                    ctxtRate.SetValue(rate);
                }
            }
        }
        //---------------------
        //Surojit 19-03-2019
        function QuantityGotFocus(s, e) {

            //Rajdip--------------------------------
            //var value = ctxtvalue.GetValue();
            //var rate = ctxtRate.GetValue();
            //var quantity = value / rate;
            //ctxtQty.SetValue(quantity);
            //--------------------------------------
            var ProductID = $('#hdfProductID').val();
            var Branch = ccmbbranch.GetValue();
            var WarehouseID = $('#ddlWarehouse').val();

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
                    //Rev Subhra 0019966 08-05-2019
                    //var gridprodqty = '';
                    var gridprodqty = parseFloat(ctxtQty.GetText()).toFixed(4);
                    //End of Rev
                    var gridPackingQty = '';
                    var slno = WarehouseID;
                    var strProductID = ProductID;

                    var isOverideConvertion = SpliteDetails[4];
                    var packing_saleUOM = SpliteDetails[2];
                    var sProduct_SaleUom = SpliteDetails[3];
                    var sProduct_quantity = SpliteDetails[0];
                    var packing_quantity = SpliteDetails[1];

                    //if (ShowUOMConversionInEntry == "1" && SpliteDetails.length > 5) {
                    //    ShowUOM(type, "Opening Balances", 0, sProduct_quantity, sProduct_SaleUom, packing_quantity, packing_saleUOM, strProductID, slno, isOverideConvertion, gridprodqty, gridPackingQty);
                    //}
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

        var issavePacking = 0;

        var aarrUOM = [];

        function SetDataToGrid(Quantity, packing, PackingUom, PackingSelectUom, productid, slno) {
            issavePacking = 1;
            //grid.batchEditApi.StartEdit(globalRowIndex);
            //grid.GetEditor('Quantity').SetValue(Quantity);
            //SetFoucs();
            ctxtQty.SetValue(Quantity);


            $('#hdnUOMQuantity').val(Quantity);
            $('#hdnUOMpacking').val(packing);
            $('#hdnUOMPackingUom').val(PackingUom);
            $('#hdnUOMPackingSelectUom').val(PackingSelectUom);
            QuantityLostFocus();
            RateGotFocus();
        }

        //function SetUOMConversionArray(WarehouseID) {

        //    var Quantity = $('#hdnUOMQuantity').val();
        //    var packing = $('#hdnUOMpacking').val();
        //    var PackingUom = $('#hdnUOMPackingUom').val();
        //    var PackingSelectUom = $('#hdnUOMPackingSelectUom').val();
        //    var slnoget = WarehouseID;

        //    if (StockOfProduct.length > 0) {
        //        var extobj = {};
        //        var PackingUom = $('#hdnUOMPackingUom').val();
        //        var PackingSelectUom = $('#hdnUOMPackingSelectUom').val();

        //        var productidget = $('#hdfProductID').val();


        //        for (i = 0; i < aarrUOM.length; i++) {
        //            extobj = aarr[i];
        //            console.log(extobj);
        //            if (extobj.slno == slnoget && extobj.productid == productidget) {
        //                //aarr.pop(extobj);
        //                aarrUOM.splice(i, 1);
        //            }
        //            extobj = {};
        //        }


        //        var arrobj = {};
        //        arrobj.productid = productidget;
        //        arrobj.slno = slnoget;
        //        arrobj.Quantity = Quantity;
        //        arrobj.packing = packing;
        //        arrobj.PackingUom = PackingUom;
        //        arrobj.PackingSelectUom = PackingSelectUom;

        //        aarrUOM.push(arrobj);
        //    }
        //}

        //Surojit 19-03-2019
    </script>
    <style>
        #OpeningGrid_DXStatus span > a {
            display: none;
        }

        #OpeningGrid_DXStatus {
            display: none;
        }

        #OpeningGrid_DXMainTable > tbody > tr > td:first-child {
            display: none !important;
        }

        #OpeningGrid_DXMainTable > tbody > tr > td:nth-child(2) {
            display: none !important;
        }

        #OpeningGrid_DXMainTable > tbody > tr > td:nth-child(3) {
            display: none !important;
        }

        .pullleftClass {
            position: absolute;
            right: -3px;
            top: 18px;
        }

        #showData table > thead > tr, #showData table {
            width: 100%;
        }

            #showData table > tbody > tr > td:last-child, #showData table > thead > tr > th:last-child {
                text-align: center;
            }

                #showData table > tbody > tr > td:last-child a {
                    margin-right: 5px !important;
                }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title clearfix" id="td_contact1" runat="server">
            <h3 class="clearfix pull-left">
                <asp:Label ID="lblHeadTitle" runat="server" Text="Project Opening Balances - Product(s)"></asp:Label>
            </h3>
            <div id="pageheaderContent" class="hide">
                <div class="Top clearfix">
                    <ul>
                        <li>
                            <div class="lblHolder">
                                <table>
                                    <tr>
                                        <td>Total Values</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <b>
                                                <asp:Label ID="lblTotalSum" runat="server" Text=""></asp:Label></b>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </li>
                    </ul>
                </div>
            </div>
        </div>
    </div>
    <dxe:ASPxCallbackPanel runat="server" ID="CallbackPanel" ClientInstanceName="cCallbackPanel" OnCallback="CallbackPanel_Callback">
        <PanelCollection>
            <dxe:PanelContent runat="server">
                <div class="form_main">
                    <div class="SearchArea">
                        <div class="FilterSide">
                            <div style="width: 60px; float: left; padding-top: 5px">Branch: </div>
                            <div class="col-sm-4">
                                <dxe:ASPxComboBox ID="cmbbranch" runat="server" ClientInstanceName="ccmbbranch" TextField="branch_description" ValueField="branch_id">
                                    <ClientSideEvents SelectedIndexChanged="function(s,e) { chnagedcombo(s);}" />
                                </dxe:ASPxComboBox>
                            </div>
                        </div>

                        <asp:HiddenField ID="hdnmultiwarehouse" runat="server" />

                        <div class="FilterSide">
                            <div class="pull-right">
                                <dxe:ASPxButton ID="btnGenerate" ClientInstanceName="cbtnGenerate" runat="server" AutoPostBack="false" Text="Generate Barcode" CssClass="btn btn-success" UseSubmitBehavior="false">
                                    <ClientSideEvents Click="function(s, e) {GenerateBarcode();}" />
                                </dxe:ASPxButton>
                                <dxe:ASPxButton ID="btnPrint" ClientInstanceName="cbtnPrint" runat="server" AutoPostBack="false" Text="Print Barcode" CssClass="btn btn-warning" UseSubmitBehavior="false">
                                    <ClientSideEvents Click="function(s, e) {PrintBarcode();}" />
                                </dxe:ASPxButton>
                                <% if (rights.CanExport)
                                   { %>
                                <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-sm btn-primary" OnChange="if(!AvailableExportOption()){return false;}" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true">
                                    <asp:ListItem Value="0">Export to</asp:ListItem>
                                    <asp:ListItem Value="1">PDF</asp:ListItem>
                                    <asp:ListItem Value="2">XLS</asp:ListItem>
                                    <asp:ListItem Value="3">RTF</asp:ListItem>
                                    <asp:ListItem Value="4">CSV</asp:ListItem>
                                </asp:DropDownList>
                                <% } %>
                            </div>
                        </div>
                        <div class="clear">
                            <br />
                        </div>
                    </div>
                    <div>
                        <dxe:ASPxGridView ID="OpeningGrid" ClientInstanceName="OpeningGrid" runat="server" KeyFieldName="ProductID" AutoGenerateColumns="False"
                            Width="100%" EnableRowsCache="true" SettingsBehavior-AllowFocusedRow="true"
                            OnRowInserting="Grid_RowInserting" OnRowUpdating="Grid_RowUpdating" OnRowDeleting="Grid_RowDeleting" OnDataBinding="OpeningGrid_DataBinding"
                            OnCustomCallback="OpeningGrid_CustomCallback" SettingsDataSecurity-AllowEdit="false" SettingsDataSecurity-AllowInsert="false" SettingsDataSecurity-AllowDelete="false">
                            <SettingsSearchPanel Visible="True" Delay="5000" />
                            <Columns>
                                <dxe:GridViewDataTextColumn FieldName="AvailableStock" Caption="AvailableStock" VisibleIndex="0">
                                    <Settings AllowAutoFilterTextInputTimer="False" />
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="InventoryType" Caption="InventoryType" VisibleIndex="1">
                                    <Settings AllowAutoFilterTextInputTimer="False" />
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="StockID" Caption="StockID" VisibleIndex="2">
                                    <Settings AllowAutoFilterTextInputTimer="False" />
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataColumn Caption="Product Code" FieldName="ProductCode" VisibleIndex="6" Width="25%" Settings-AutoFilterCondition="Contains" CellStyle-Wrap="True">
                                    <Settings AllowAutoFilterTextInputTimer="False" />
                                    <EditFormSettings Visible="False" />
                                </dxe:GridViewDataColumn>
                                <dxe:GridViewDataColumn Caption="Product Name" FieldName="ViewProductName" VisibleIndex="7" Width="30%" Settings-AutoFilterCondition="Contains" CellStyle-Wrap="True">
                                    <Settings AllowAutoFilterTextInputTimer="False" />
                                    <EditFormSettings Visible="False" />
                                </dxe:GridViewDataColumn>
                                <dxe:GridViewDataTextColumn Caption="Opening Quantity" FieldName="OpeningStock" VisibleIndex="8" Width="10%" Settings-ShowFilterRowMenu="true"
                                    Settings-AllowHeaderFilter="true" Settings-AllowAutoFilter="true">
                                    <Settings AllowAutoFilterTextInputTimer="False" />
                                    <EditFormSettings Visible="False" />
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="Stock UOM" FieldName="UOM" ReadOnly="true" VisibleIndex="9" Width="10%">
                                    <EditFormSettings Visible="False" />
                                    <Settings AllowAutoFilterTextInputTimer="False" />
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="Stock Details" FieldName="Stock_ID" VisibleIndex="10" CellStyle-VerticalAlign="Middle"
                                    CellStyle-HorizontalAlign="Center" Settings-ShowFilterRowMenu="False" Settings-AllowHeaderFilter="False" Settings-AllowAutoFilter="False" Width="5%">
                                    <EditFormSettings Visible="False" />
                                    <DataItemTemplate>
                                        <a href="javascript:void(0);" title="Stock Details" onclick="CheckProductStockdetails('<%#Eval("ProductID")%>','<%#Eval("ProductName")%>','<%#Eval("UOM")%>','<%#GetvisibleIndex(Container)%>','<%#Eval("DefaultWarehouse")%>','<%#Eval("InventoryType")%>','<%#Eval("OpeningStock")%>','<%#Eval("OpeningValue")%>','<%#Eval("OpeningValue")%>')" class="pad" style='<%#Eval("IsInventoryEnable")%>'>
                                            <img src="../../../assests/images/warehouse.png" />
                                        </a>
                                    </DataItemTemplate>
                                    <Settings AllowAutoFilterTextInputTimer="False" />
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="Value" FieldName="OpeningValue" VisibleIndex="11" Width="10%" Settings-ShowFilterRowMenu="true"
                                    Settings-AllowHeaderFilter="true" Settings-AllowAutoFilter="true">
                                    <EditFormSettings Visible="False" />
                                    <Settings AllowAutoFilterTextInputTimer="False" />
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="Barcode Gen" FieldName="IsAllBarcodeGenerate" ReadOnly="true" VisibleIndex="12" Width="5%">
                                    <EditFormSettings Visible="False" />
                                    <Settings AllowAutoFilterTextInputTimer="False" />
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="Barcode Print" FieldName="IsAllPrint" ReadOnly="true" VisibleIndex="13" Width="5%">
                                    <EditFormSettings Visible="False" />
                                    <Settings AllowAutoFilterTextInputTimer="False" />
                                </dxe:GridViewDataTextColumn>
                            </Columns>
                            <SettingsContextMenu Enabled="true"></SettingsContextMenu>
                            <ClientSideEvents BatchEditStartEditing="OnBatchStartEdit" EndCallback="OnEndCallback" />
                            <SettingsDataSecurity AllowEdit="false" />
                            <SettingsEditing Mode="Batch">
                                <BatchEditSettings ShowConfirmOnLosingChanges="false" EditMode="row" />
                            </SettingsEditing>
                            <Settings ShowFilterRow="true" ShowFilterRowMenu="true" ShowFooter="true" ShowGroupFooter="VisibleIfExpanded" />
                            <SettingsBehavior ColumnResizeMode="Disabled" />
                            <SettingsPager NumericButtonCount="10" PageSize="15" ShowSeparators="True" Mode="ShowPager">
                                <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
                                <FirstPageButton Visible="True">
                                </FirstPageButton>
                                <LastPageButton Visible="True">
                                </LastPageButton>
                            </SettingsPager>
                            <TotalSummary>
                                <dxe:ASPxSummaryItem FieldName="OpeningStock" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="OpeningValue" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="ProductCode" SummaryType="Count" DisplayFormat="Product Count : #######" />
                            </TotalSummary>
                        </dxe:ASPxGridView>
                    </div>
                </div>
                <div>
                    <%--Warehouse Details Start--%>
                    <dxe:ASPxPopupControl ID="Popup_Warehouse" runat="server" ClientInstanceName="cPopup_Warehouse"
                        Width="1000px" HeaderText="Warehouse Details" PopupHorizontalAlign="WindowCenter"
                        BackColor="white" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
                        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
                        ContentStyle-CssClass="pad">
                        <ClientSideEvents Closing="function(s, e) {
	                    closeWarehouse(s, e);}" />
                        <ContentStyle VerticalAlign="Top" CssClass="pad">
                        </ContentStyle>
                        <ContentCollection>
                            <dxe:PopupControlContentControl runat="server">
                                <uc3:UOMConversionControl runat="server" ID="UOMConversionControl" />

                                <div id="content-5" class="pull-right wrapHolder content horizontal-images" style="width: 100%; margin-right: 0px;">
                                    <ul>
                                        <li>
                                            <div class="lblHolder" style="display: none;">
                                                <table>
                                                    <tbody>
                                                        <tr>
                                                            <td>Entered Quantity </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="txt_EnteredSalesAmount" runat="server" Font-Bold="true"></asp:Label>
                                                                <asp:Label ID="txt_EnteredSalesUOM" runat="server" Font-Bold="true"></asp:Label>

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
                                                            <td>Opening Stock</td>
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
                                    </ul>
                                </div>
                                <div class="clear">
                                </div>
                                <div class="clearfix  modal-body" style="padding: 8px 0 8px 0; margin-bottom: 15px; margin-top: 15px; border-radius: 4px; border: 1px solid #ccc;">
                                    <div class="col-md-12">
                                        <div class="clearfix  row">
                                            <div class="col-md-3" id="_div_Warehouse">
                                                <div>
                                                    Warehouse
                                                </div>
                                                <div class="Left_Content" style="">
                                                    <asp:DropDownList ID="ddlWarehouse" runat="server" Width="100%" DataTextField="WarehouseName" DataValueField="WarehouseID" onchange="ddlWarehouse_ValueChange()">
                                                    </asp:DropDownList>
                                                    <span id="rfvWarehouse" title="Mandatory" class="tp2 fa fa-exclamation-circle iconRed" style="display: none;"></span>
                                                </div>
                                            </div>
                                            <div class="col-md-3" id="_div_Quantity">
                                                <div>
                                                    Quantity
                                                </div>
                                                <div class="Left_Content" style="">
                                                    <dxe:ASPxTextBox ID="txtQty" runat="server" ClientSideEvents-GotFocus="QuantityGotFocus" ClientInstanceName="ctxtQty" HorizontalAlign="Right" Font-Size="12px" Width="100%" Height="15px">
                                                        <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..9999&gt;" IncludeLiterals="DecimalSymbol" />
                                                        <%--ClientSideEvents-GotFocus="QuantityGotFocus" --%>
                                                        <ValidationSettings Display="None"></ValidationSettings>
                                                        <ClientSideEvents TextChanged="function(s,e) { ChangePackingByQuantityinjs();}" />
                                                    </dxe:ASPxTextBox>

                                                    <span id="rfvQuantity" title="Mandatory" class="tp2 fa fa-exclamation-circle iconRed" style="display: none;"></span>
                                                </div>
                                            </div>
                                            <div class="col-md-3" id="_div_AlterQuantity">
                                                <div>
                                                    <label id="lblaltqty">Alt. Qty</label>
                                                </div>
                                                <div class="Left_Content" style="">
                                                    <dxe:ASPxTextBox ID="txtAlterQty1" runat="server" ClientSideEvents-GotFocus="QuantityGotFocus" ClientInstanceName="cAltertxtQty1" HorizontalAlign="Right" Font-Size="12px" Width="100%" Height="15px">
                                                        <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..9999&gt;" IncludeLiterals="DecimalSymbol" />
                                                        <%--ClientSideEvents-GotFocus="QuantityGotFocus"--%>
                                                        <ValidationSettings Display="None"></ValidationSettings>
                                                        <ClientSideEvents TextChanged="function(s,e) { ChangeQuantityByPacking1();}" />
                                                    </dxe:ASPxTextBox>
                                                    <%--        <script>
                                                        function ChangeQuantityByPacking1()
                                                        {
                                                            alert("new");
                                                        }
                                                    </script>--%>
                                                    <span id="rfvAlterQuantity" title="Mandatory" class="tp2 fa fa-exclamation-circle iconRed" style="display: none;"></span>

                                                </div>
                                            </div>

                                            <div class="col-md-3" id="_div_Uom">
                                                <div>
                                                    <label id="lblaltuom">Alt. UOM</label>
                                                </div>
                                                <div class="Left_Content" style="">
                                                    <dxe:ASPxComboBox ID="cmbPackingUom1" ClientInstanceName="ccmbPackingUom1" runat="server" SelectedIndex="0"
                                                        ValueType="System.String" Width="100%" EnableSynchronization="True">
                                                        <%--EnableIncrementalFiltering="False"--%>
                                                    </dxe:ASPxComboBox>

                                                    <%-- <span id="rfvUom" title="Mandatory" class="tp2 fa fa-exclamation-circle iconRed" style="display: none;"></span>--%>
                                                </div>
                                            </div>
                                            <div class="col-md-3" id="_div_Batch">
                                                <div>
                                                    Batch/Lot
                                                </div>
                                                <div class="Left_Content" style="">
                                                    <input type="text" id="txtBatch" placeholder="Batch" />
                                                    <span id="rfvBatch" title="Mandatory" class="tp2 fa fa-exclamation-circle iconRed" style="display: none;"></span>
                                                </div>
                                            </div>
                                            <div class="clear">
                                            </div>
                                            <div class="col-md-3" id="_div_Manufacture">
                                                <div>
                                                    Mfg Date
                                                </div>
                                                <div class="Left_Content" style="">
                                                    <%-- <input type="text" id="txtMfgDate" placeholder="Mfg Date" />--%>
                                                    <dxe:ASPxDateEdit ID="txtMfgDate" runat="server" Width="100%" EditFormat="custom" UseMaskBehavior="True"
                                                        ClientInstanceName="ctxtMfgDate" AllowNull="true" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy">
                                                        <ButtonStyle Width="13px">
                                                        </ButtonStyle>
                                                    </dxe:ASPxDateEdit>
                                                </div>
                                            </div>
                                            <div class="col-md-3" id="_div_Expiry">
                                                <div>
                                                    Expiry Date
                                                </div>
                                                <div class="Left_Content" style="">
                                                    <%-- <input type="text" id="txtExprieyDate" placeholder="Expiry Date" />--%>
                                                    <dxe:ASPxDateEdit ID="txtExprieyDate" runat="server" Width="100%" EditFormat="custom" UseMaskBehavior="True"
                                                        ClientInstanceName="ctxtExprieyDate" AllowNull="true" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy">
                                                        <ButtonStyle Width="13px">
                                                        </ButtonStyle>
                                                    </dxe:ASPxDateEdit>
                                                </div>
                                            </div>
                                            <div class="clear"></div>
                                            <div class="clear" id="_div_Break"></div>
                                            <div class="col-md-3" id="_div_Rate">
                                                <div>
                                                    Rate
                                                </div>
                                                <div class="Left_Content" style="">
                                                    <dxe:ASPxTextBox ID="txtRate" runat="server" ClientInstanceName="ctxtRate" ClientSideEvents-LostFocus="QuantityLostFocus" HorizontalAlign="Right" Font-Size="12px" Width="100%" Height="15px">
                                                        <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" IncludeLiterals="DecimalSymbol" />
                                                        <ValidationSettings>
                                                        </ValidationSettings>
                                                    </dxe:ASPxTextBox>

                                                </div>
                                            </div>

                                            <div class="col-md-3" id="DivValue">
                                                <div>
                                                    Value
                                                </div>
                                                <div class="Left_Content" style="">
                                                    <%--ClientSideEvents-GotFocus="ValueGotFocus"--%>
                                                    <dxe:ASPxTextBox ID="txtvalue" runat="server" ClientInstanceName="ctxtvalue" ClientSideEvents-LostFocus="RateGotFocus" HorizontalAlign="Right" Font-Size="12px" Width="100%" Height="15px">
                                                        <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" IncludeLiterals="DecimalSymbol" />
                                                        <ValidationSettings Display="None"></ValidationSettings>
                                                    </dxe:ASPxTextBox>
                                                    <span id="rfvValue" title="Mandatory" class="tp2 fa fa-exclamation-circle iconRed" style="display: none;"></span>
                                                </div>
                                            </div>
                                            <div class="col-md-3" id="_div_Serial">
                                                <div>
                                                    Serial No
                                                </div>
                                                <div class="Left_Content" style="">
                                                    <input type="text" id="txtSerial" placeholder="Serial No" onkeyup="Serialkeydown(event)" onchange="QuantityLostFocus()" />
                                                    <span id="rfvSerial" title="Mandatory" class="tp2 fa fa-exclamation-circle iconRed" style="display: none;"></span>
                                                </div>
                                            </div>
                                            <div class="col-md-3" id="_div_Upload">
                                                <div class="col-md-3">
                                                    <div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <div>
                                                    Project
                                                </div>
                                                <div class="Left_Content" style="">
                                                    <dxe:ASPxGridLookup ID="lookup_Project" runat="server" ClientInstanceName="clookup_Project" DataSourceID="EntityServerModeDataJournal"
                                                        KeyFieldName="Proj_Id" Width="100%" TextFormatString="{1}" AutoGenerateColumns="False">
                                                        <Columns>
                                                          <%--   <dxe:GridViewDataColumn FieldName="Proj_Id" Visible="true" VisibleIndex="0" Caption="Project Code" Settings-AutoFilterCondition="Contains" Width="0">
                                                            </dxe:GridViewDataColumn>--%>
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
                                                        <ClientSideEvents GotFocus="function(s,e){clookup_Project.ShowDropDown();}" LostFocus="clookup_Project_LostFocus" ValueChanged="ProjectValueChange" />
                                                        <ClearButton DisplayMode="Always">
                                                        </ClearButton>
                                                    </dxe:ASPxGridLookup>
                                                    <dx:LinqServerModeDataSource ID="EntityServerModeDataJournal" runat="server" OnSelecting="EntityServerModeDataJournal_Selecting"
                                                        ContextTypeName="ERPDataClassesDataContext" TableName="V_ProjectList" />
                                                    <span id="MandatoryProject" class="pullleftClass fa fa-exclamation-circle iconRed " style="color: red; display: none; padding-left: 9px;" title="Mandatory"></span>
                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <div class="Left_Content" style="">
                                                    Hierarchy
                                                </div>
                                                <div>
                                                    <asp:DropDownList ID="ddlHierarchy" runat="server" Width="100%" Enabled="false">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <div>
                                                </div>
                                                <div class="Left_Content" style="padding-top: 7px">
                                                    <input type="button" onclick="SaveStock()" value="Add" class="btn btn-primary" />
                                                    <input id="btnSecondUOM" type="button" onclick="AlternateUOMDetails('POE')" value="Alt Unit Details" class="btn btn-success" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div id="showData" class="gridStatic">
                                </div>
                                <div class="clearfix  row">
                                    <div class="col-md-3">
                                        <div>
                                        </div>
                                        <div class="Left_Content" style="padding-top: 14px">
                                            <input type="button" onclick="FullnFinalSave()" value="Save & Ex&#818;it" class="btn btn-primary" />
                                        </div>
                                    </div>
                                </div>
                            </dxe:PopupControlContentControl>
                        </ContentCollection>
                        <HeaderStyle BackColor="LightGray" ForeColor="Black" />
                    </dxe:ASPxPopupControl>
                    <%--Warehouse Details End--%>
                </div>
            </dxe:PanelContent>
        </PanelCollection>
        <ClientSideEvents EndCallback="CallbackPanelEndCallBack" />
    </dxe:ASPxCallbackPanel>
    <div>
        <asp:HiddenField runat="server" ID="hdnvalue" />
        <asp:HiddenField runat="server" ID="hdnuomFactor" />
        <asp:HiddenField runat="server" ID="hdnisOverideConvertion" />
        <asp:HiddenField runat="server" ID="hdnpackingqty" />
        <asp:HiddenField runat="server" ID="hdnrate" />
        <asp:HiddenField runat="server" ID="hdfWarehousetype" />
        <asp:HiddenField runat="server" ID="hdfProductSrlNo" />
        <asp:HiddenField runat="server" ID="hdfProductID" />
        <asp:HiddenField runat="server" ID="hdndefaultWarehouse" />
        <asp:HiddenField runat="server" ID="hdfUOM" />
        <asp:HiddenField runat="server" ID="hdfServiceURL" />
        <asp:HiddenField runat="server" ID="hdfBranch" />
        <asp:HiddenField runat="server" ID="hdfIsRateExists" />
        <asp:HiddenField runat="server" ID="hdnJsonProductStock" />
        <asp:HiddenField runat="server" ID="hdnIsPopUp" />
        <asp:HiddenField runat="server" ID="IsStockBlock" />
        <asp:HiddenField runat="server" ID="AvailableQty" />
        <asp:HiddenField runat="server" ID="CurrentQty" />
        <asp:HiddenField runat="server" ID="hdfIsBarcodeActive" />
        <asp:HiddenField runat="server" ID="hdfIsBarcodeGenerator" />
        <%----- Rajdip--- --%>
        <input type="hidden" value="" id="hdnaddedit" runat="server" />
        <input type="hidden" value="" id="hdnaddeditwhensave" runat="server" />
        <input type="hidden" value="" id="hddnqty" runat="server" />
        <%-- ---Rajdip--- --%>
        <%-- Surojit 19-03-2019 --%>
        <asp:HiddenField runat="server" ID="hdnConvertionOverideVisible" />
        <asp:HiddenField runat="server" ID="hdnShowUOMConversionInEntry" />
        <input type="hidden" value="" id="hdnid" />
        <input type="hidden" value="" id="hdnUOMQuantity" />
        <input type="hidden" value="" id="hdnUOMpacking" />
        <input type="hidden" value="" id="hdnUOMPackingUom" />
        <input type="hidden" value="" id="hdnUOMPackingSelectUom" />
        <input type="hidden" value="" id="hdnModeType" />
        <%-- Surojit 19-03-2019 --%>

        <input type="hidden" value="" id="hdnProjectID" />
        <input type="hidden" value="" id="hdnHierarchyID" />
        <input type="hidden" value="" id="hdnProject" />
        <input type="hidden" value="" id="hdnHierarchy" />
    </div>
    <div>
        <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="false" PaperKind="A2" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
        </dxe:ASPxGridViewExporter>
    </div>
    <div style="display: none">
        <dxe:ASPxGridView ID="openingGridExport" runat="server" KeyFieldName="ProductID" AutoGenerateColumns="True"
            Width="100%" EnableRowsCache="true" SettingsBehavior-AllowFocusedRow="true"
            OnDataBinding="openingGridExport_DataBinding">
            <Columns>
                <dxe:GridViewDataTextColumn FieldName="Product_Code" Caption="Product Code" VisibleIndex="0">
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn FieldName="Product_Name" Caption="Product Name" VisibleIndex="1">
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn FieldName="Opening_Quontity" Caption="Opening Quantity" VisibleIndex="2">
                    <PropertiesTextEdit DisplayFormatString="0.0000"></PropertiesTextEdit>
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn FieldName="Stock_UOM" Caption="Stock UOM" VisibleIndex="3">
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn FieldName="Opening_Value" Caption="Opening Value" VisibleIndex="4">
                    <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn FieldName="Barcode_Gen" Caption="Barcode Gen" VisibleIndex="5">
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn FieldName="Barcode_Print" Caption="Barcode Print" VisibleIndex="6">
                </dxe:GridViewDataTextColumn>
            </Columns>
        </dxe:ASPxGridView>
    </div>
    <dxe:ASPxLoadingPanel ID="LoadingPanel" runat="server" ClientInstanceName="LoadingPanel" Modal="True" ContainerElementID="OpeningGrid">
    </dxe:ASPxLoadingPanel>

    <ucWH:MultiWarehouceuc runat="Server" ID="MultiWarehouceuc"></ucWH:MultiWarehouceuc>


    <dxe:ASPxPopupControl ID="SecondUOMpopup" runat="server" ClientInstanceName="cSecondUOM" ShowCloseButton="false"
        Width="850px" HeaderText="Second UOM Details" PopupHorizontalAlign="WindowCenter"
        BackColor="white" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
        ContentStyle-CssClass="pad">
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
                <div class="clearfix boxStyle">
                    <div class="col-md-3">
                        <label>Length (inch)</label>
                        <dxe:ASPxTextBox runat="server" ID="txtLength" ClientInstanceName="ctxtLength">
                            <ClientSideEvents LostFocus="SizeLostFocus" />

                            <MaskSettings Mask="&lt;0..99999999999&gt;.&lt;00..99&gt;" />

                        </dxe:ASPxTextBox>
                    </div>
                    <div class="col-md-3">
                        <label>Width (inch)</label>
                        <dxe:ASPxTextBox runat="server" ID="txtWidth" ClientInstanceName="ctxtWidth">
                            <ClientSideEvents LostFocus="SizeLostFocus" />
                            <MaskSettings Mask="&lt;0..99999999999&gt;.&lt;00..99&gt;" />

                        </dxe:ASPxTextBox>
                    </div>
                    <div class="col-md-3">
                        <label>Total (Sq. Feet)</label>
                        <dxe:ASPxTextBox runat="server" ID="txtTotal" ClientEnabled="false" ClientInstanceName="ctxtTotal">
                            <MaskSettings Mask="&lt;0..99999999999&gt;.&lt;00..99&gt;" />
                        </dxe:ASPxTextBox>
                    </div>
                    <div class="col-md-3 padTop23 pdLeft0">
                        <label></label>
                        <button type="button" onclick="AddSecondUOMDetails();" class="btn btn-primary">Add</button>
                    </div>
                </div>
                <div class="clearfix"></div>
                <div class="col-md-12">
                    <table id="dataTbl" class="display nowrap" style="width: 100%">
                        <thead>
                            <tr>
                                <th class="hide">GUID</th>
                                <th class="hide">WarehouseID</th>
                                <th class="hide">ProductId</th>
                                <th>SL</th>
                                <th>Branch</th>
                                <th>Warehouse</th>
                                <th>Size</th>
                                <th>Total</th>
                                <th>Action</th>
                            </tr>
                        </thead>
                        <tbody id="tbodySecondUOM">
                        </tbody>
                    </table>
                </div>
                <div class="col-md-12 text-right pdTop15">
                    <button class="btn btn-success" type="button" onclick="SavePOESecondUOMDetails();">OK</button>
                    <button class="btn btn-danger hide" type="button" onclick="return cSecondUOM.Hide();">Cancel</button>
                </div>
            </dxe:PopupControlContentControl>
        </ContentCollection>

    </dxe:ASPxPopupControl>
    <style>
        .boxStyle {
            padding: 5px;
            background: #f7f7f7;
            margin: 0 15px 8px 15px;
            border: 1px solid #ccc;
        }

        .link {
            cursor: pointer;
        }

        .pdLeft0 {
            padding-left: 0 !important;
        }

        #dataTbl_wrapper .dataTables_scrollHeadInner, #dataTbl_wrapper .dataTables_scrollHeadInner table {
            width: 100% !important;
        }

            #dataTbl_wrapper .dataTables_scrollHeadInner table > thead > tr > th {
                background: #337ab7;
                color: #fff;
                padding: 2px 15px;
            }

        #tbodySecondUOM > td {
            padding: 2px 25px;
        }

        #dataTbl_wrapper .dataTables_scrollHeadInner table > thead > tr > th:not(:last-child) {
            border-right: #333;
        }
    </style>
    <script>
        $(function () {
            //var table = 

        });
    </script>
</asp:Content>
