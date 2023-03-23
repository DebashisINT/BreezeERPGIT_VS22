<%--================================================== Revision History =============================================
Rev Number         DATE              VERSION          DEVELOPER           CHANGES
1.0                23-03-2023        2.0.36           Pallab              25733 : Master pages design modification
====================================================== Revision History =============================================--%>

<%@ Page Title="Opening Balances - Product(s)" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="ProductsOpeningEntries.aspx.cs" Inherits="ERP.OMS.Management.Master.ProductsOpeningEntries" %>

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
    <script src="../Activities/JS/ProductOpeningStockIn.js?var=1.27"></script>
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
        .dynamicPopupTbl.scroll > thead > tr{
             padding-right: 17px
         }
        .dynamicPopupTbl > tbody > tr{
            display:flex;
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
         .eqTble > tbody > tr > td {
            padding: 0 7px;
            vertical-align: top;
        }
            .mlableWh{
            padding-top: 22px;
            display:inline-block
        }
        .mlableWh>input +span {
            white-space: nowrap;
        }
   
   
    </style>
    <script>
        $(document).ready(function () {
            var setting = document.getElementById("hdnShowUOMConversionInEntry").value;
            //alert(setting);
            if (setting == 1)
            {
                cAltertxtQty1.SetVisible(true);
                ccmbPackingUom1.SetVisible(true);
                document.getElementById("lblaltqty").style.display = "block";
                document.getElementById("lblaltuom").style.display = "block";
            }
            else
            {
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
            //else if (OpeningGrid.cpfinalMsg == "checkMultiUOMData") {
            //    debugger;
            //    OpeningGrid.cpMessage = null;
            //    LoadingPanel.Hide();
            //    jAlert('Please add Alt. Qty.');
            //}

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
            debugger;
            document.getElementById("hdnaddedit").value = "Add";//Rajdip
            ctxtvalue.SetValue('');
            SecondUOM = [];
            if ($("#hdnmultiwarehouse").val() == "111") {
                ucCheckProductStockdetails(ProductID, ProductName, UOM, VisibleIndex, DefaultWarehouse, Warehousetype, OpeningStock, ccmbbranch.GetValue());
                return;
            }
            if ($("#hddnMultiUOMSelection").val() == "1")
            {
                ctxtQty.SetEnabled(false);
            }
            else {
                ctxtQty.SetEnabled(true);
            }

            //------------------------------Rev Rajdip---------------------------
            var objectToPass = {}
            objectToPass.ProductID = ProductID;
            $.ajax({
                type: "POST",
                url: "../Activities/Services/Master.asmx/GetUom",
                data: JSON.stringify(objectToPass),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    debugger;
                    var returnObject = msg.d;
                    var UOMId = msg.UOM_Id;
                    //REV Bapi

                    hdUOMid.value = msg.d.uomac_id;
                    
                    //End Rev Bapi

                    var UOMName = msg.UOM_Name;
                    if (returnObject) {


                        var setting = document.getElementById("hdnShowUOMConversionInEntry").value;                       
                        if (setting == 1) {
                            SetDataSourceOnComboBox(ccmbPackingUom1, returnObject.uom);
                            ccmbPackingUom1.SetValue(returnObject.uom_id);                            
                            ccmbPackingUom1.SetEnabled(false);                            
                            cAltertxtQty1.SetVisible(true);
                            ccmbPackingUom1.SetVisible(true);
                            document.getElementById("lblaltqty").style.display = "block";
                            document.getElementById("lblaltuom").style.display = "block";
                        }
                        else
                        {
                            document.getElementById("lblaltqty").style.display = "none";
                            document.getElementById("lblaltuom").style.display = "none";
                            cAltertxtQty1.SetVisible(false);
                            ccmbPackingUom1.SetVisible(false);
                        }
                        
                    }
                }
            });
            

         
            function SetDataSourceOnComboBox(ControlObject, Source) {
                ControlObject.ClearItems();
                for (var count = 0; count < Source.length; count++) {
                    ControlObject.AddItem(Source[count].UOM_Name, Source[count].UOM_Id);
                    //ControlObject.AddItem(Source[count].UOM_Id, Source[count].UOM_Name);
                }
                ControlObject.SetSelectedIndex(0);
            }


            //As discussed with pijush da alternate quantity in serial number to be there as it is
            //var objectToPass = {}
            //objectToPass.ProductID = ProductID;
            //$.ajax({
            //    type: "POST",
            //    url: "../Activities/Services/Master.asmx/IsserialActivate",
            //    data: JSON.stringify(objectToPass),
            //    contentType: "application/json; charset=utf-8",
            //    dataType: "json",
            //    success: function (msg) {
            //        var returnObject = msg.d;
            //        var Isserial = msg.Isserial;
            //        if (returnObject) {
            //            if (Isserial == 0) {
            //                ccmbPackingUom1.SetVisible(true);
            //                cAltertxtQty1.SetValue(true);
            //            }
            //            else {
            //                ccmbPackingUom1.SetVisible(false);
            //                cAltertxtQty1.SetVisible(false);
            //            }
            //            //  document.getElementById("cmbPackingUom1").value = ProductID;
            //        }
            //    }
            //});

            //------------------------------End Rev Rajdip-----------------------





            GetObjectID('hdnIsPopUp').value = "Y";

            var Branch = ccmbbranch.GetValue();
            var GetserviceURL = "../Activities/Services/Master.asmx/GetOpeningStockDetails";
            var serviceURL = "../Activities/Services/Master.asmx/CheckDuplicateSerial";
            // GetObjectID('hdnrate').value = Rate;
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
            //  document.getElementById('<%=lblAvailableStock.ClientID %>').innerHTML = OpeningStock;
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
                    var myObj = returnObject.ProductStockDetails;
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

                    if (myObj.length > 0) {
                        var setting = document.getElementById("hdnShowUOMConversionInEntry").value;    
                        if (setting == 1) {
                            for (x in myObj) {
                                var ProductStock = {
                                    Product_SrlNo: myObj[x]["Product_SrlNo"], SrlNo: parseInt(myObj[x]["SrlNo"]), WarehouseID: myObj[x]["WarehouseID"], WarehouseName: myObj[x]["WarehouseName"],
                                    Quantity: myObj[x]["Quantity"], SalesQuantity: myObj[x]["SalesQuantity"], Batch: myObj[x]["Batch"], MfgDate: myObj[x]["MfgDate"], ExpiryDate: myObj[x]["ExpiryDate"],
                                    Rate: myObj[x]["Rate"], Value: myObj[x]["Value"], SerialNo: myObj[x]["SerialNo"], Barcode: myObj[x]["Barcode"], ViewBatch: myObj[x]["Batch"],
                                    ViewMfgDate: myObj[x]["MfgDate"], ViewExpiryDate: myObj[x]["ExpiryDate"], ViewRate: myObj[x]["Rate"],
                                    IsOutStatus: myObj[x]["IsOutStatus"], IsOutStatusMsg: myObj[x]["IsOutStatusMsg"], LoopID: parseInt(myObj[x]["LoopID"]), Status: myObj[x]["Status"], AlterQty: myObj[x]["AlterQty"],
                                    AltUOM: myObj[x]["AltUOM"],
                                }
                                StockOfProduct.push(ProductStock);
                            }
                        }
                        else
                        {
                            for (x in myObj) {
                                var ProductStock = {
                                    Product_SrlNo: myObj[x]["Product_SrlNo"], SrlNo: parseInt(myObj[x]["SrlNo"]), WarehouseID: myObj[x]["WarehouseID"], WarehouseName: myObj[x]["WarehouseName"],
                                    Quantity: myObj[x]["Quantity"], SalesQuantity: myObj[x]["SalesQuantity"], Batch: myObj[x]["Batch"], MfgDate: myObj[x]["MfgDate"], ExpiryDate: myObj[x]["ExpiryDate"],
                                    Rate: myObj[x]["Rate"], Value: myObj[x]["Value"], SerialNo: myObj[x]["SerialNo"], Barcode: myObj[x]["Barcode"], ViewBatch: myObj[x]["Batch"],
                                    ViewMfgDate: myObj[x]["MfgDate"], ViewExpiryDate: myObj[x]["ExpiryDate"], ViewRate: myObj[x]["Rate"],
                                    IsOutStatus: myObj[x]["IsOutStatus"], IsOutStatusMsg: myObj[x]["IsOutStatusMsg"], LoopID: parseInt(myObj[x]["LoopID"]), Status: myObj[x]["Status"], 
                                }
                                StockOfProduct.push(ProductStock);
                            }
                        }

                        StockOfProduct.sort(sortByMultipleKey(['LoopID', 'SrlNo']));
                        StockOfProduct = ReGenerateStock(StockOfProduct);

                        CreateStock();
                        cPopup_Warehouse.Show();
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
                        url: "ProductsOpeningEntries.aspx/SetSessionPacking",
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
            document.getElementById('drdExport').value = "0";

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
         #showData table>thead>tr, #showData table {
             width:100%;
             
         }
         #showData table>tbody>tr>td:last-child, #showData table>thead>tr>th:last-child {
             text-align:center;
         }
        #showData table>tbody>tr>td:last-child a {
            margin-right:5px !important;
        }

        /*Rev 1.0*/

        /*select
        {
            height: 30px !important;
            border-radius: 4px;
            -webkit-appearance: none;
            position: relative;
            z-index: 1;
            background-color: transparent;
            padding-left: 10px !important;
            padding-right: 22px !important;
        }*/

        .dxeButtonEditSys.dxeButtonEdit_PlasticBlue , .dxeTextBox_PlasticBlue
        {
            height: 30px;
            border-radius: 4px;
        }

        .dxeButtonEditButton_PlasticBlue
        {
            background: #094e8c !important;
            border-radius: 4px !important;
            padding: 0 4px !important;
        }

        #ASPxFromDate , #ASPxToDate , #ASPxASondate , #ASPxAsOnDate , #txtDOB , #txtAnniversary , #txtcstVdate , #txtLocalVdate ,
        #txtCINVdate , #txtincorporateDate , #txtErpValidFrom , #txtErpValidUpto , #txtESICValidFrom , #txtESICValidUpto , #FormDate , #toDate
        {
            position: relative;
            z-index: 1;
            background: transparent;
        }

        .dxeDisabled_PlasticBlue
        {
            z-index: 0 !important;
        }

        #ASPxFromDate_B-1 , #ASPxToDate_B-1 , #ASPxASondate_B-1 , #ASPxAsOnDate_B-1 , #txtDOB_B-1 , #txtAnniversary_B-1 , #txtcstVdate_B-1 ,
        #txtLocalVdate_B-1 , #txtCINVdate_B-1 , #txtincorporateDate_B-1 , #txtErpValidFrom_B-1 , #txtErpValidUpto_B-1 , #txtESICValidFrom_B-1 ,
        #txtESICValidUpto_B-1 , #FormDate_B-1 , #toDate_B-1
        {
            background: transparent !important;
            border: none;
            width: 30px;
            padding: 10px !important;
        }

        #ASPxFromDate_B-1 #ASPxFromDate_B-1Img , #ASPxToDate_B-1 #ASPxToDate_B-1Img , #ASPxASondate_B-1 #ASPxASondate_B-1Img , #ASPxAsOnDate_B-1 #ASPxAsOnDate_B-1Img ,
        #txtDOB_B-1 #txtDOB_B-1Img ,
        #txtAnniversary_B-1 #txtAnniversary_B-1Img ,
        #txtcstVdate_B-1 #txtcstVdate_B-1Img ,
        #txtLocalVdate_B-1 #txtLocalVdate_B-1Img , #txtCINVdate_B-1 #txtCINVdate_B-1Img , #txtincorporateDate_B-1 #txtincorporateDate_B-1Img ,
        #txtErpValidFrom_B-1 #txtErpValidFrom_B-1Img , #txtErpValidUpto_B-1 #txtErpValidUpto_B-1Img , #txtESICValidFrom_B-1 #txtESICValidFrom_B-1Img ,
        #txtESICValidUpto_B-1 #txtESICValidUpto_B-1Img , #FormDate_B-1 #FormDate_B-1Img , #toDate_B-1 #toDate_B-1Img
        {
            display: none;
        }

        .dxtcLite_PlasticBlue > .dxtc-stripContainer .dxtc-activeTab, .dxgvFooter_PlasticBlue
        {
            background: #1b5ea4 !important;
        }

        /*select.btn
        {
            padding-right: 10px !important;
        }*/

        .panel-group .panel
        {
            box-shadow: 1px 1px 8px #1111113b;
            border-radius: 8px;
        }

        .dxpLite_PlasticBlue .dxp-current
        {
            background-color: #1b5ea4;
            padding: 3px 5px;
            border-radius: 2px;
        }

        #accordion {
            margin-bottom: 20px;
            margin-top: 10px;
        }

        .dxgvHeader_PlasticBlue {
    background: #1b5ea4 !important;
    color: #fff !important;
}
        #ShowGrid
        {
            margin-top: 10px;
        }

        .pt-25{
                padding-top: 25px !important;
        }

        .dxgvEditFormDisplayRow_PlasticBlue td.dxgv, .dxgvDataRow_PlasticBlue td.dxgv, .dxgvDataRowAlt_PlasticBlue td.dxgv, .dxgvSelectedRow_PlasticBlue td.dxgv, .dxgvFocusedRow_PlasticBlue td.dxgv
        {
            padding: 6px 6px 6px !important;
        }

        #lookupCardBank_DDD_PW-1
        {
                left: -182px !important;
        }
        .plhead a>i
        {
                top: 9px;
        }

        .clsTo
        {
            display: flex;
    align-items: flex-start;
        }

        input[type="radio"], input[type="checkbox"]
        {
            margin-right: 5px;
        }
        .dxeCalendarDay_PlasticBlue
        {
                padding: 6px 6px;
        }

        .modal-dialog
        {
            width: 50%;
        }

        .modal-header
        {
            padding: 8px 4px 8px 10px;
            background: #094e8c !important;
        }

        .TableMain100 #ShowGrid , .TableMain100 #ShowGridList , .TableMain100 #ShowGridRet , .TableMain100 #EmployeeGrid , .TableMain100 #GrdEmployee,
        .TableMain100 #GrdHolidays , #cityGrid
        {
            max-width: 98% !important;
        }

        /*div.dxtcSys > .dxtc-content > div, div.dxtcSys > .dxtc-content > div > div
        {
            width: 95% !important;
        }*/

        .btn-info
        {
                background-color: #1da8d1 !important;
                background-image: none;
        }

        .for-cust-icon {
            position: relative;
            z-index: 1;
        }

        .dxeDisabled_PlasticBlue, .aspNetDisabled
        {
            background: #f3f3f3 !important;
        }

        .dxeButtonDisabled_PlasticBlue
        {
            background: #b5b5b5 !important;
            border-color: #b5b5b5 !important;
        }

        #ddlValTech
        {
            width: 100% !important;
            margin-bottom: 0 !important;
        }

        .dis-flex
        {
            display: flex;
            align-items: baseline;
        }

        input + label
        {
            line-height: 1;
                margin-top: 7px;
        }

        .dxtlHeader_PlasticBlue
        {
            background: #094e8c !important;
        }

        .dxeBase_PlasticBlue .dxichCellSys
        {
            padding-top: 2px !important;
        }

        .pBackDiv
        {
            border-radius: 10px;
            box-shadow: 1px 1px 10px #1111112e;
        }
        .HeaderStyle th
        {
            padding: 5px;
        }

        .dxtcLite_PlasticBlue.dxtc-top > .dxtc-stripContainer
        {
            padding-top: 15px;
        }

        .pt-2
        {
            padding-top: 5px;
        }
        .pt-10
        {
            padding-top: 10px;
        }

        .pt-15
        {
            padding-top: 15px;
        }

        .pb-10
        {
            padding-bottom: 10px;
        }

        .pTop10 {
    padding-top: 20px;
}
        .custom-padd
        {
            padding-top: 4px;
    padding-bottom: 10px;
        }

        input + label
        {
                margin-right: 10px;
        }

        .btn
        {
            margin-bottom: 0;
        }

        .pl-10
        {
            padding-left: 10px;
        }

        .col-md-3>label, .col-md-3>span
        {
            margin-top: 0 !important;
        }

        .devCheck
        {
            margin-top: 5px;
        }

        .mtc-5
        {
            margin-top: 5px;
        }

        .mtc-10
        {
            margin-top: 10px;
        }

        /*select.btn
        {
           position: relative;
           z-index: 0;
        }*/

        select
        {
            margin-bottom: 0;
        }

        .form-control
        {
            background-color: transparent;
        }

        select.btn-radius {
    padding: 4px 8px 6px 11px !important;
}
        .mt-30{
            margin-top: 30px;
        }

        .panel-title h3
        {
            padding-top: 0;
            padding-bottom: 0;
        }

        .btn-radius
        {
            padding: 4px 11px !important;
            border-radius: 4px !important;
        }

        .crossBtn
        {
             right: 30px;
             top: 25px;
        }

        .mb-10
        {
            margin-bottom: 10px;
        }

        .btn-cust
        {
            background-color: #108b47 !important;
            color: #fff;
        }

        .btn-cust:hover
        {
            background-color: #097439 !important;
            color: #fff;
        }

        .gHesder
        {
            background: #1b5ca0 !important;
            color: #ffffff !important;
            padding: 6px 0 6px !important;
        }

        .close
        {
             color: #fff;
             opacity: .5;
             font-weight: 400;
        }

        .mt-24
        {
            margin-top: 24px;
        }

        .col-md-3
        {
            margin-top: 8px;
        }

        #upldBigLogo , #upldSmallLogo
        {
            width: 100%;
        }

        #DivSetAsDefault
        {
            margin-top: 25px;
        }

        .dxeBase_PlasticBlue .dxichTextCellSys label
        {
            color: #fff !important;
        }

        #actv-warh label
        {
            color: #111 !important;
        }

        /*#ShowFilter
        {
            padding-bottom: 3px !important;
        }*/

        /*.dxeDisabled_PlasticBlue, .aspNetDisabled {
            opacity: 0.4 !important;
            color: #ffffff !important;
        }*/
                /*.padTopbutton {
            padding-top: 27px;
        }*/
        /*#lookup_project
        {
            max-width: 100% !important;
        }*/
        /*Rev end 1.0*/
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--Rev 1.0: "outer-div-main" class add --%>
    <div class="outer-div-main">
        <div class="panel-heading">
        <div class="panel-title clearfix" id="td_contact1" runat="server">
            <h3 class="clearfix pull-left">
                <asp:Label ID="lblHeadTitle" runat="server" Text="Opening Balances - Product(s)"></asp:Label>
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
                                <dxe:ASPxSummaryItem FieldName="OpeningStock" SummaryType="Sum" DisplayFormat="{0:n4}" />
                                <dxe:ASPxSummaryItem FieldName="OpeningValue" SummaryType="Sum" DisplayFormat="{0:n4}"/>
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
                                                    <dxe:ASPxTextBox ID="txtQty" runat="server" ClientSideEvents-GotFocus="QuantityGotFocus"  ClientInstanceName="ctxtQty"  HorizontalAlign="Right" Font-Size="12px" Width="100%" Height="15px">
                                                        <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..9999&gt;" IncludeLiterals="DecimalSymbol" /><%--ClientSideEvents-GotFocus="QuantityGotFocus" --%>
                                                        <ValidationSettings Display="None"></ValidationSettings>
                                                         <ClientSideEvents  TextChanged="function(s,e) { ChangePackingByQuantityinjs();}" />
                                                    </dxe:ASPxTextBox>
                                                 
                                                    <span id="rfvQuantity" title="Mandatory" class="tp2 fa fa-exclamation-circle iconRed" style="display: none;"></span>
                                                </div>
                                            </div>
                                               <div class="col-md-3" id="multiuom" runat="server" style="display:none">
                                                <div>
                                                    <label class="col-md-12" style="padding:0px;"><b>Multi UOM</b></label>
                                                    <label class="clearfix"></label>
                                                      
                                                    <a  aria-hidden="true" onclick="MultiUom();"><img src="/assests/images/MultiUomIcon.png" /></a>
                                                 <%--    <input type="button"  value="Multi UOM"    class="fa fa-bars" onclick="MultiUom();" />--%>
                                                </div>
                                            </div>
                                                     <div class="clear">
                                </div> 
                                                 <div class="col-md-3" id="_div_AlterQuantity">
                                                <div>
                                               <label id="lblaltqty">Alt. Qty</label>
                                                </div>
                                                <div class="Left_Content" style="">
                                                    <dxe:ASPxTextBox ID="txtAlterQty1"  runat="server" ClientSideEvents-GotFocus="QuantityGotFocus" ClientInstanceName="cAltertxtQty1"   HorizontalAlign="Right" Font-Size="12px" Width="100%" Height="15px">
                                                        <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..9999&gt;" IncludeLiterals="DecimalSymbol" /><%--ClientSideEvents-GotFocus="QuantityGotFocus"--%>
                                                        <ValidationSettings Display="None"></ValidationSettings>
                                                   <ClientSideEvents  TextChanged="function(s,e) { ChangeQuantityByPacking1();}" />
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
                                            ValueType="System.String" Width="100%"  EnableSynchronization="True"> <%--EnableIncrementalFiltering="False"--%>
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
                                                    <dxe:ASPxTextBox ID="txtRate" runat="server"  ClientInstanceName="ctxtRate" ClientSideEvents-LostFocus="QuantityLostFocus"  HorizontalAlign="Right" Font-Size="12px" Width="100%" Height="15px">
                                                        <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" IncludeLiterals="DecimalSymbol" />
                                                        <ValidationSettings >  

                       
                                                        </ValidationSettings>
                                                    </dxe:ASPxTextBox>

                                                </div>
                                            </div>
                                       
                                            <div class="col-md-3" id="DivValue">
                                                <div>
                                                    Value
                                                </div>
                                                <div class="Left_Content" style=""><%--ClientSideEvents-GotFocus="ValueGotFocus"--%>
                                                    <dxe:ASPxTextBox ID="txtvalue" runat="server" ClientInstanceName="ctxtvalue"  ClientSideEvents-LostFocus="RateGotFocus" HorizontalAlign="Right" Font-Size="12px" Width="100%" Height="15px">
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
                     <%--Multi UOM POPUP--%>
                     <dxe:ASPxPopupControl ID="Popup_MultiUOM" runat="server" ClientInstanceName="cPopup_MultiUOM"
        Width="900px" HeaderText="Multi UOM Details" PopupHorizontalAlign="WindowCenter"
        BackColor="white" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
        ContentStyle-CssClass="pad">
        <ClientSideEvents Closing="function(s, e) {
	closeMultiUOM(s, e);}" />
        <ContentStyle VerticalAlign="Top" CssClass="pad">
        </ContentStyle>
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
                <div class="Top clearfix">



                    <div class="clearfix col-md-12" style="background: #f5f4f3; padding: 8px 0; margin-bottom: 15px; border-radius: 4px; border: 1px solid #ccc;">

                        <table class="eqTble">
                            <tr>
                                <td>
                                    <div>
                                        <div style="margin-bottom: 5px;">
                                            <div>
                                                <label>Base Quantity</label>
                                            </div>
                                            <div>
                                                <%--<input type="text" id="UOMQuantity" style="text-align: right;" maxlength="18"  class="allownumericwithdecimal" />--%>
                                                <input type="text" id="UOMQuantity" style="text-align: right;" maxlength="18"  class="allownumericwithdecimal" onchange="CalcBaseRate()" />
                                                <%--End of Rev Mantis Issue 24428/24429--%>
                                            </div>
                                        </div>
                                    </div>
                                </td>
                                <td>
                                    <div class="Left_Content" style="">
                                        <div>
                                            <label style="text-align: right;">Base UOM</label>
                                        </div>
                                        <div>
                                            <dxe:ASPxComboBox ID="cmbUOM" ClientInstanceName="ccmbUOM" runat="server" SelectedIndex="0" DataSourceID="UomSelect"
                                                ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True" ValueField="UOM_ID" TextField="UOM_Name">
                                            </dxe:ASPxComboBox>
                                        </div>
                                    </div>
                                </td>
                                <%--Rev Sanchita--%>
                                 <td>
                                    <div style="margin-bottom: 5px;">
                                        <div>
                                            <label>Base Rate </label>
                                        </div>
                                        <div>
                                            <dxe:ASPxTextBox ID="cmbBaseRate" runat="server" Width="80px" ClientInstanceName="ccmbBaseRate" DisplayFormatString="0.000" MaskSettings-Mask="&lt;0..99999999&gt;.&lt;00..999&gt;" FocusedStyle-HorizontalAlign="Right" HorizontalAlign="Right" ReadOnly="true" ></dxe:ASPxTextBox>
                                        </div>
                                    </div>
                                </td>
                                <%--End of Rev Sanchita--%>
                                <td>
                                    <span style="font-size: 22px; padding-top: 15px; display: inline-block;">=</span>
                                </td>
                                <td>
                                    <div>
                                        <div>
                                            <label style="text-align: right;">Alt. UOM</label>
                                        </div>
                                        <div class="Left_Content" style="">
                                            <dxe:ASPxComboBox ID="cmbSecondUOM" ClientInstanceName="ccmbSecondUOM" runat="server" SelectedIndex="0" DataSourceID="AltUomSelect"
                                                ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True" ValueField="UOM_ID" TextField="UOM_Name">
                                                <ClientSideEvents TextChanged="function(s,e) { PopulateMultiUomAltQuantity();}" />
                                            </dxe:ASPxComboBox>
                                        </div>
                                    </div>
                                </td>
                                <td>
                                    <div style="margin-bottom: 5px;">
                                        <div>
                                            <label style="white-space:nowrap">Alt. Quantity </label>
                                        </div>
                                        <div>
                                            <%--<input type="text" id="AltUOMQuantity" style="text-align:right;"  maxlength="18" class="allownumericwithdecimal"/> --%>
                                            <dxe:ASPxTextBox Width="80px" ID="AltUOMQuantity" runat="server" ClientInstanceName="cAltUOMQuantity" DisplayFormatString="0.0000" MaskSettings-Mask="&lt;0..999999999&gt;.&lt;00..9999&gt;" FocusedStyle-HorizontalAlign="Right" HorizontalAlign="Right">
                                                <%--Rev Sanchita--%>
                                                <ClientSideEvents TextChanged="function(s,e) { CalcBaseQty();}" />
                                                <%--End of Rev Sanchita--%>
                                            </dxe:ASPxTextBox>
                                        </div>
                                    </div>
                                </td>
                                <%--Rev Sanchita--%>
                                 <td>
                                    <div style="margin-bottom: 5px;">
                                        <div>
                                            <label>Alt Rate </label>
                                        </div>
                                        <div>
                                            <dxe:ASPxTextBox ID="cmbAltRate" Width="80px" runat="server" ClientInstanceName="ccmbAltRate" DisplayFormatString="0.000" MaskSettings-Mask="&lt;0..99999999&gt;.&lt;00..999&gt;" FocusedStyle-HorizontalAlign="Right" HorizontalAlign="Right"  >
                                                <ClientSideEvents TextChanged="function(s,e) { CalcBaseRate();}" />
                                            </dxe:ASPxTextBox>
                                        </div>
                                    </div>
                                </td>
                                <td>
                                    <div style="margin-bottom: 5px;">
                                        <div>
                                            
                                        </div>
                                        <div>
                                         
                                                  <%--Mantis Issue 24428--%>
                                              <%--  <asp:CheckBox ID="chkUpdateRow" Checked="false" runat="server" ></asp:CheckBox>
                                                <span style="margin: 0px 0; display: block">
                                                    <dxe:ASPxLabel ID="ASPxLabel18" runat="server" Text="Update Row">
                                                    </dxe:ASPxLabel>

                                                </span>--%>
                                                 <label class="checkbox-inline mlableWh">
                                                <input type="checkbox" id="chkUpdateRow"  />
                                                <span style="margin: 0px 0; display: block">
                                                    <dxe:ASPxLabel ID="ASPxLabel18" runat="server" Text="Update Row">
                                                    </dxe:ASPxLabel>
                                                </span>
                                            </label>
                                        </div>
                                    </div>

                                    
                                </td>
                                <%--End of Rev Sanchita--%>
                                <td style="padding-top: 14px;">
                                    <dxe:ASPxButton ID="btnMUltiUOM" ClientInstanceName="cbtnMUltiUOM" Width="50px" runat="server" AutoPostBack="False" Text="Add" CssClass="btn btn-primary" UseSubmitBehavior="false">
                                        <ClientSideEvents Click="function(s, e) { SaveMultiUOM();}" />
                                    </dxe:ASPxButton>
                                </td>
                            </tr>
                        </table>

                    </div>
                    <div class="clearfix">
                        <dxe:ASPxGridView ID="grid_MultiUOM" runat="server" KeyFieldName="AltUomId;AltQuantity" AutoGenerateColumns="False"
                            Width="100%" ClientInstanceName="cgrid_MultiUOM" OnCustomCallback="MultiUOM_CustomCallback" OnDataBinding="MultiUOM_DataBinding"
                            SettingsPager-Mode="ShowAllRecords" Settings-VerticalScrollBarMode="auto" Settings-VerticalScrollableHeight="200" SettingsBehavior-AllowSort="false">
                            <Columns>
                                <dxe:GridViewDataTextColumn Caption="Quantity" FieldName="Quantity"
                                    VisibleIndex="0" HeaderStyle-HorizontalAlign="Right">
                                    <PropertiesTextEdit Style-HorizontalAlign="Right"></PropertiesTextEdit>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="UOM" FieldName="UOM"
                                    VisibleIndex="1">
                                </dxe:GridViewDataTextColumn>
                                <%--Rev Sanchita--%>
                                <dxe:GridViewDataTextColumn Caption="Rate" FieldName="BaseRate"
                                    VisibleIndex="2" HeaderStyle-HorizontalAlign="Right">
                                    <PropertiesTextEdit Style-HorizontalAlign="Right"></PropertiesTextEdit>
                                </dxe:GridViewDataTextColumn>
                                <%--End of Rev Sanchita--%>

                                <dxe:GridViewDataTextColumn Caption="Alt. UOM" FieldName="AltUOM"
                                    VisibleIndex="3">
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Alt. Quantity" FieldName="AltQuantity"
                                    VisibleIndex="4" HeaderStyle-HorizontalAlign="Right">
                                </dxe:GridViewDataTextColumn>


                                <dxe:GridViewDataTextColumn Caption="" FieldName="UomId" Width="0px"
                                    VisibleIndex="5">
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="" FieldName="AltUomId" Width="0px"
                                    VisibleIndex="6">
                                </dxe:GridViewDataTextColumn>

                                <%--Rev Sanchita--%>
                                <dxe:GridViewDataTextColumn Caption="Rate" FieldName="AltRate"
                                    VisibleIndex="7" HeaderStyle-HorizontalAlign="Right">
                                    <PropertiesTextEdit Style-HorizontalAlign="Right"></PropertiesTextEdit>
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Update Row" FieldName="UpdateRow"
                                    VisibleIndex="7" HeaderStyle-HorizontalAlign="Right">
                                    <PropertiesTextEdit Style-HorizontalAlign="Right"></PropertiesTextEdit>
                                </dxe:GridViewDataTextColumn>
                                <%--End of Rev Sanchita--%>

                                <dxe:GridViewDataTextColumn VisibleIndex="10" Width="80px" Caption="Action">
                                    <DataItemTemplate>
                                        <a href="javascript:void(0);" onclick="Delete_MultiUom('<%# Container.KeyValue %>','<%#Eval("SrlNo") %>')" title="Delete">
                                            <img src="/assests/images/crs.png" /></a>

                                             <%--Mantis Issue 24428 --%>

                                           <a href="javascript:void(0);" onclick="Edit_MultiUom('<%# Container.KeyValue %>','<%#Eval("MultiUOMSR") %>')" title="Edit">
                                            <img src="/assests/images/Edit.png" /></a>
                                          <%--End of Mantis Issue 24428 --%>
                                    </DataItemTemplate>
                                </dxe:GridViewDataTextColumn>
                            </Columns>
                            <ClientSideEvents EndCallback="OnMultiUOMEndCallback" />
                            <SettingsPager Visible="false"></SettingsPager>
                            <SettingsLoadingPanel Text="Please Wait..." />
                        </dxe:ASPxGridView>
                    </div>
                    <div class="clearfix">
                        <br />
                        <div style="align-content: center">
                            <dxe:ASPxButton ID="ASPxButton7" ClientInstanceName="cbtnfinalUomSave" Width="50px" runat="server" AutoPostBack="False" Text="Save" CssClass="btn btn-primary" UseSubmitBehavior="false">
                                <ClientSideEvents Click="function(s, e) {FinalMultiUOM();}" />
                            </dxe:ASPxButton>
                        </div>
                    </div>
                </div>
            </dxe:PopupControlContentControl>
        </ContentCollection>
        <HeaderStyle BackColor="LightGray" ForeColor="Black" />
    </dxe:ASPxPopupControl>
                       <%--Multi UOM POPUP End--%>

                </div>
            </dxe:PanelContent>
        </PanelCollection>
        <ClientSideEvents EndCallback="CallbackPanelEndCallBack" />
    </dxe:ASPxCallbackPanel>
        <%--  Rev Bapi--%>
        <asp:SqlDataSource ID="UomSelect" runat="server"
        SelectCommand="select UOM_ID,UOM_Name from Master_UOM "></asp:SqlDataSource>

        <asp:SqlDataSource ID="AltUomSelect" runat="server"
        SelectCommand="select UOM_ID,UOM_Name from Master_UOM "></asp:SqlDataSource>
        <div>
        
    <asp:HiddenField ID="hddnuomFactor" runat="server" />
    <asp:HiddenField ID="hddnMultiUOMSelection" runat="server" />
     <%--   rev Bapi End--%>
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
      <%--  rev sanchita--%>
          <asp:HiddenField runat="server" ID="hdmultiuomsessiondata" />
       <%-- end rev sanchita --%>
        <input type="hidden" value="" id="hdnid" />
        <input type="hidden" value="" id="hdnUOMQuantity" />
        <input type="hidden" value="" id="hdnUOMpacking" />
        <input type="hidden" value="" id="hdnUOMPackingUom" />
        <input type="hidden" value="" id="hdnUOMPackingSelectUom" />
        <input type="hidden" value="" id="hdnModeType" />
        <%-- Surojit 19-03-2019 --%>
          <input type="hidden" value="" id="hdUOMid" />
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
