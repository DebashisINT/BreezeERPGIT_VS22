var ucStockOfProduct = [];
var ucwarehouserateList =[];

function ucgetMax(array, propName) {
    var max = 0;
    var maxItem = null;
    for (var i = 0; i < array.length; i++) {
        var item = array[i];
        if (item[propName] > max) {
            max = item[propName];
            maxItem = item;
        }
    }
    return max;
}

function ucgetMin(array, propName) {
    var min = array[0][propName];
    var minItem = array[0];
    for (var i = 1; i < array.length; i++) {
        var item = array[i];
        if (item[propName] < min) {
            min = item[propName];
            minItem = item;
        }
    }
    return min;
}

function ucSortByLoop(x,y) {
    return ((x.LoopID == y.LoopID) ? 0 : ((x.LoopID > y.LoopID) ? 1 : -1 ));
}

function ucsortByMultipleKey(keys) {
    return function(a, b) {
        if (keys.length == 0) return 0; // force to equal if keys run out
        key = keys[0]; // take out the first key
        if (a[key] < b[key]) return -1; // will be 1 if DESC
        else if (a[key] > b[key]) return 1; // will be -1 if DESC
        else return ucsortByMultipleKey(keys.slice(1))(a, b);
    }
}

function ucflexFilter(arr, info) {
    var matchesFilter, matches = [];

    matchesFilter = function (item) {
        var count = 0;
        for (var n = 0; n < info.length; n++) {
            //if (info[n]["Values"].indexOf(item[info[n]["Field"]]) > -1) {
            if (info[n]["Values"]==item[info[n]["Field"]]){
                count++;
            }
        }

        return count == info.length;
    }

    // Loop through each item in the array
    for (var i = 0; i < arr.length; i++) {
        // Determine if the current item matches the filter criteria
        if (matchesFilter(arr[i])) {
            matches.push(arr[i]);
        }
    }

    // Give us a new array containing the objects matching the filter criteria
    return matches;
}

function ucGetDateFormat(today) {
    if (today != "") {
        var dd = today.getDate();
        var mm = today.getMonth() + 1; //January is 0!

        var yyyy = today.getFullYear();
        if (dd < 10) {
            dd = '0' + dd;
        }
        if (mm < 10) {
            mm = '0' + mm;
        }
        today = dd + '-' + mm + '-' + yyyy;
    }

    return today;
}

function ucCreateStock() {
    $('#txtBatch').val('');
    cuctxtQty.SetValue(0);
    cuctxtRate.SetValue(0);
    cuctxtMfgDate.SetDate(null);
    cuctxtExprieyDate.SetDate(null);
    $('#uctxtSerial').val('');

    //ucStockOfProduct=$.grep(ucStockOfProduct, function (e) { if (e.Status!="D")  return true; });
    var defaultWarehouse = GetObjectID('uchdndefaultWarehouse').value;
    if (defaultWarehouse != "") document.getElementById('ucddlWarehouse').value = defaultWarehouse;

    var Warehousetype = GetObjectID('uchdfWarehousetype').value;
    var IsRateExists = GetObjectID('uchdfIsRateExists').value;    

    if (Warehousetype == "W" || Warehousetype == "WC") {
        uc_div_Warehouse.style.display = 'block';
        uc_div_Batch.style.display = 'none';
        uc_div_Manufacture.style.display = 'none';
        uc_div_Expiry.style.display = 'none';
        uc_div_Quantity.style.display = 'block';
        uc_div_Serial.style.display = 'none';
        uc_div_Upload.style.display = 'none';
        uc_div_Break.style.display = 'none';
    }
    else if (Warehousetype == "B") {
        uc_div_Warehouse.style.display = 'none';
        uc_div_Batch.style.display = 'block';
        uc_div_Manufacture.style.display = 'block';
        uc_div_Expiry.style.display = 'block';
        uc_div_Quantity.style.display = 'block';
        uc_div_Serial.style.display = 'none';
        uc_div_Upload.style.display = 'none';
        uc_div_Break.style.display = 'none';
    }
    else if (Warehousetype == "S") {
        uc_div_Warehouse.style.display = 'none';
        uc_div_Batch.style.display = 'none';
        uc_div_Manufacture.style.display = 'none';
        uc_div_Expiry.style.display = 'none';
        uc_div_Quantity.style.display = 'none';
        uc_div_Serial.style.display = 'block';
        uc_div_Upload.style.display = 'none';
        uc_div_Break.style.display = 'none';
    }
    else if (Warehousetype == "WB") {
        uc_div_Warehouse.style.display = 'block';
        uc_div_Batch.style.display = 'block';
        uc_div_Manufacture.style.display = 'block';
        uc_div_Expiry.style.display = 'block';
        uc_div_Quantity.style.display = 'block';
        uc_div_Serial.style.display = 'none';
        uc_div_Upload.style.display = 'none';
        uc_div_Break.style.display = 'none';
    }
    else if (Warehousetype == "WS" || Warehousetype == "WSC") {
        uc_div_Warehouse.style.display = 'block';
        uc_div_Batch.style.display = 'none';
        uc_div_Manufacture.style.display = 'none';
        uc_div_Expiry.style.display = 'none';
        uc_div_Quantity.style.display = 'none';
        uc_div_Serial.style.display = 'block';
        uc_div_Upload.style.display = 'none';
        uc_div_Break.style.display = 'none';
    }
    else if (Warehousetype == "WBS") {
        uc_div_Warehouse.style.display = 'block';
        uc_div_Batch.style.display = 'block';
        uc_div_Manufacture.style.display = 'block';
        uc_div_Expiry.style.display = 'block';
        uc_div_Quantity.style.display = 'none';
        uc_div_Serial.style.display = 'block';
        uc_div_Upload.style.display = 'none';
        uc_div_Break.style.display = 'block';
    }
    else if (Warehousetype == "BS") {
        uc_div_Warehouse.style.display = 'none';
        uc_div_Batch.style.display = 'block';
        uc_div_Manufacture.style.display = 'block';
        uc_div_Expiry.style.display = 'block';
        uc_div_Quantity.style.display = 'none';
        uc_div_Serial.style.display = 'block';
        uc_div_Upload.style.display = 'none';
        uc_div_Break.style.display = 'none';
    }
    else {
        uc_div_Warehouse.style.display = 'none';
        uc_div_Batch.style.display = 'none';
        uc_div_Manufacture.style.display = 'none';
        uc_div_Expiry.style.display = 'none';
        uc_div_Quantity.style.display = 'none';
        uc_div_Serial.style.display = 'none';
        uc_div_Upload.style.display = 'none';
    }

    if(IsRateExists=="Y"){
        uc_div_Rate.style.display = 'block';

        var List = $.grep(ucwarehouserateList, function (e) { return e.WarehouseID == defaultWarehouse; })

        if (List.length > 0) {
            var Rate = List[0].Rate;
            cuctxtRate.SetValue(Rate);
        }
        else {
            cuctxtRate.SetValue("0");
        }
    }
    else{
        uc_div_Rate.style.display = 'none';
    }

    ucStockDeatils();
    ucSetFocus("Add");
}

function Serialkeydown(e){
    if (e.code == "Enter" || e.code == "NumpadEnter") {
        SaveStock();
    }
}

function ucSaveStock() {
    var StockType = GetObjectID('uchdfWarehousetype').value;
    var ProductSrlNo = GetObjectID('uchdfProductSrlNo').value;
    var ProductID= GetObjectID('uchdfProductID').value;
    var UOM =  GetObjectID('uchdfUOM').value;
    var ServiceURL =  GetObjectID('uchdfServiceURL').value;
    var Branch =  GetObjectID('uchdfBranch').value;

    var WarehouseID = $('#ucddlWarehouse').val();
    var WarehouseName = $("option:selected", '#ucddlWarehouse').text();
    var Batch = $("#uctxtBatch").val().trim();
    var Qty =cuctxtQty.GetValue();   
    var MfgDate = (cuctxtMfgDate.GetValue() != null) ? cuctxtMfgDate.GetValue() : "";
    var ExprieyDate = (cuctxtExprieyDate.GetValue() != null) ? cuctxtExprieyDate.GetValue() : "";
    var Serial = $("#uctxtSerial").val().trim();
    var Rate =cuctxtRate.GetValue();  

    MfgDate = GetDateFormat(MfgDate);
    ExprieyDate = GetDateFormat(ExprieyDate);

    if (StockType == "W" ||StockType == "WS" || StockType == "WBS" || StockType == "WB" || StockType == "WC"|| StockType == "WSC"){
        if(WarehouseName==null || WarehouseName==""){
            $("#ucrfvWarehouse").css("display", "block");
            return false;
        }
        else{
            $("#ucrfvWarehouse").css("display", "none");
        }
    }
    
    if (StockType == "B" ||StockType == "BS" || StockType == "WBS" || StockType == "WB"){
        if(Batch==""){
            $("#ucrfvBatch").css("display", "block");
            return false;
        }
        else{
            $("#ucrfvBatch").css("display", "none");
        }
    }

    if (StockType == "BS" || StockType == "WBS" || StockType == "WS" || StockType == "S" || StockType == "WSC"){
        if(Serial==""){
            $("#ucrfvSerial").css("display", "block");
            return false;
        }
        else{
            $("#ucrfvSerial").css("display", "none");
        }
    }

    if (StockType == "W" || StockType == "WB" || StockType == "B" || StockType == "WC" ){
        if(parseFloat(Qty)==0){
            $("#rfvQuantity").css("display", "block");
            return false;
        }
        else{
            $("#rfvQuantity").css("display", "none");
        }
    }


    if (StockType == "WS" || StockType == "WBS" || StockType == "BS" || StockType == "S"|| StockType == "WSC") {
        if(Serial!=""){
            var serialCriteria = [{ Field: "SerialNo", Values: Serial }];
            var serialfilteredJson = flexFilter(ucStockOfProduct, serialCriteria);

            if(serialfilteredJson.length == 0){     
                var objectToPass = {}
                objectToPass.SerialNo = Serial;
                objectToPass.ProductID = ProductID;
                objectToPass.BranchID = Branch;

                $.ajax({
                    type: "POST",
                    url: ServiceURL,//"Services/Master.asmx/CheckDuplicateSerial",
                    data: JSON.stringify(objectToPass),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        if(msg.d==0){
                            serialCriteria = [{ Field: "SerialNo", Values: Serial }];
                            serialfilteredJson = flexFilter(ucStockOfProduct, serialCriteria);

                            if(serialfilteredJson.length == 0){
                                ucsaveStockData(StockType,ProductSrlNo,ProductID,UOM,WarehouseID,WarehouseName,Batch,Qty,MfgDate,ExprieyDate,Serial,Rate);
                            }
                        }
                        else{
                            jAlert("Duplicate Serial. Cannot Proceed.", "Alert", function () { ucSetFocus("Save"); });        
                        }
                    }
                });
            }
            else{
                jAlert("Duplicate Serial. Cannot Proceed.", "Alert", function () { ucSetFocus("Save"); });
            }
        }
    }
    else if (StockType == "W" ||StockType == "WC" || StockType == "WB" || StockType == "B"){
        ucsaveStockData(StockType,ProductSrlNo,ProductID,UOM,WarehouseID,WarehouseName,Batch,Qty,MfgDate,ExprieyDate,Serial,Rate);
    }
}

function ucsaveStockData(StockType,ProductSrlNo,ProductID,UOM,WarehouseID,WarehouseName,Batch,Qty,MfgDate,ExprieyDate,Serial,Rate){
    var criteria = [
                    { Field: "Product_SrlNo", Values: ProductSrlNo },
                    { Field: "WarehouseID", Values: WarehouseID },
                    { Field: "Batch", Values: Batch }
    ];
    var filteredJson = flexFilter(ucStockOfProduct, criteria);

    if (filteredJson.length == 0) {
        // Save Data In Json
        var _SrlNo = parseInt(getMax(ucStockOfProduct, "SrlNo")) + 1;
        var _LoopID = parseInt(getMax(ucStockOfProduct, "LoopID")) + 1;
        var _Quantity = "1.0000";

        if (StockType == "WS" || StockType == "WBS" || StockType == "BS" || StockType == "S"|| StockType == "WSC") {
            _Quantity = "1.0000";
        }
        else {
            _Quantity = Qty;
        }

        var ProductStock = { Product_SrlNo: ProductSrlNo, SrlNo: _SrlNo, WarehouseID: WarehouseID, WarehouseName: WarehouseName,
            Quantity: _Quantity, SalesQuantity: _Quantity + " " + UOM, Batch: Batch, MfgDate: MfgDate, ExpiryDate: ExprieyDate,Rate:Rate,
            SerialNo: Serial, Barcode: "", ViewBatch: Batch, ViewMfgDate: MfgDate, ViewExpiryDate: ExprieyDate,ViewRate:Rate,
            IsOutStatus: "1", IsOutStatusMsg: "", LoopID: _LoopID, Status: "D"
        }
        ucStockOfProduct.push(ProductStock);
    }
    else {
        // Save Data In Json
        var _SrlNo = parseInt(getMax(ucStockOfProduct, "SrlNo")) + 1;
        var _LoopID = parseInt(getMax(filteredJson, "LoopID"));
        var _Quantity = parseFloat(getMax(filteredJson, "Quantity")) + 1;

        if (StockType == "WS" ||StockType == "WSC" || StockType == "WBS" || StockType == "BS" || StockType == "S") {
            $.grep(filteredJson, function (e) { e.Quantity = _Quantity; });
            $.grep(filteredJson, function (e) { e.Rate = Rate; });

            var ProductStock = { Product_SrlNo: ProductSrlNo, SrlNo: _SrlNo, WarehouseID: WarehouseID, WarehouseName: "",
                Quantity: _Quantity, SalesQuantity: "", Batch: Batch, MfgDate: MfgDate, ExpiryDate: ExprieyDate,Rate:Rate,
                SerialNo: Serial, Barcode: "", ViewBatch: "", ViewMfgDate: "", ViewExpiryDate: "",ViewRate:"",
                IsOutStatus: "1", IsOutStatusMsg: "", LoopID: _LoopID, Status: "D"
            }
            ucStockOfProduct.push(ProductStock);

            $.grep(filteredJson, function (e) { if (e.SalesQuantity != "") e.SalesQuantity = _Quantity + " " + UOM; });
            $.grep(filteredJson, function (e) { if (e.SalesQuantity != "") e.ViewRate = Rate; });
        }
        else  if (StockType == "WC") {
            _Quantity = (parseFloat(getMax(filteredJson, "Quantity")) + parseFloat(Qty));

            if(filteredJson.length==1){
                $.grep(filteredJson, function (e) { e.Rate = Rate; });
                $.grep(filteredJson, function (e) { e.ViewRate = Rate; });
                $.grep(filteredJson, function (e) { e.Quantity = _Quantity });
                $.grep(filteredJson, function (e) { e.SalesQuantity = _Quantity + " " + UOM; ; });
            }
            else{
                $.grep(filteredJson, function (e) { if (e.SalesQuantity != "") e.SalesQuantity = _Quantity + " " + UOM; });
                $.grep(filteredJson, function (e) { if (e.SalesQuantity != "") e.ViewRate = Rate; });

                $.grep(filteredJson, function (e) { e.Rate = Rate; });
                $.grep(filteredJson, function (e) { e.Quantity = _Quantity });
            }
        }
        else {
            _Quantity = (parseFloat(getMax(filteredJson, "Quantity")) + parseFloat(Qty));

            $.grep(filteredJson, function (e) { e.Rate = Rate; });
            $.grep(filteredJson, function (e) { e.ViewRate = Rate; });
            $.grep(filteredJson, function (e) { e.Quantity = _Quantity });
            $.grep(filteredJson, function (e) { e.SalesQuantity = _Quantity + " " + UOM; ; });
        }
    }

    ucStockDeatils();
    ucSetFocus("Save");
}

function FinalSaveStock(){
    var ProductSrlNo = GetObjectID('hdfProductSrlNo').value;
    
    var Criteria = [
      { Field: "Product_SrlNo", Values: ProductSrlNo }
    ];
    var filteredJson = flexFilter(ucStockOfProduct, Criteria);
    var getQuantity = parseFloat(getMax(filteredJson, "Quantity"));
    var entryQuantity = parseFloat(GetObjectID('hdnProductQuantity').value);

    if (getQuantity == entryQuantity) {
        $.grep(ucStockOfProduct, function (e) { if (e.Product_SrlNo == ProductSrlNo) e.Status = "I"; });
        $('#ProductStock').modal('hide');
        grid.batchEditApi.StartEdit(Warehouseindex, 8);
    }
    else{
        jAlert("Purchase Quantity must be equal to Warehouse Quantity.");    
    }
}

function ucSetFocus(Time) {
    var Warehousetype = GetObjectID('hdfWarehousetype').value;

    if (Time == "Add") {
        if (Warehousetype == "W" || Warehousetype == "WB" || Warehousetype == "WBS" || Warehousetype == "WS"|| Warehousetype == "WSC"|| Warehousetype == "WC") {
            setTimeout(function () { $("#ddlWarehouse").focus(); }, 500);
        }
        else if (Warehousetype == "BS" || Warehousetype == "B") {
            setTimeout(function () { $("#txtBatch").focus(); }, 500);
        }
        else if (Warehousetype == "S") {
            setTimeout(function () { $("#txtSerial").focus(); }, 500);
        }
    }
    else if (Time == "Save") {
        if (Warehousetype == "W" || Warehousetype == "B" || Warehousetype == "WB"|| Warehousetype == "WC") {
            ctxtQty.SetValue(0);
            setTimeout(function () { ctxtQty.Focus(); }, 500);
        }
        else if (Warehousetype == "WS" || Warehousetype == "WBS" || Warehousetype == "BS" || Warehousetype == "S" || Warehousetype == "WSC") {
            $('#txtSerial').val('');
            setTimeout(function () { $("#txtSerial").focus(); }, 500);
        }
    }
}

function getPropertyValue(Product, Warehouse, Batch, Property) {
    var PropertyValue = "0";

    for (var i = 0; i < ucStockOfProduct.length; i++) {
        var ProductID = ucStockOfProduct[i]["Product_SrlNo"];
        var WarehouseID = ucStockOfProduct[i]["WarehouseID"];
        var BatchID = ucStockOfProduct[i]["Batch"];

        if (ProductID == Product && WarehouseID == Warehouse && BatchID == Batch) {
            if (Property = "LoopID") {
                PropertyValue = ucStockOfProduct[i]["LoopID"];
            }
            else if (Property = "Quantity") {
                PropertyValue = ucStockOfProduct[i]["Quantity"];
            }

            break;
        }
        else {
            if (Property = "LoopID") {
                PropertyValue = ucStockOfProduct[i]["LoopID"];
            }
            else if (Property = "Quantity") {
                PropertyValue = "1";
            }
        }

        return PropertyValue;
    } 
}

function removeRow(ID) {
    var IsProceed="Y";

    if(GetObjectID('IsStockBlock')){
        var Warehousetype = GetObjectID('hdfWarehousetype').value;
        var IsStockBlock=GetObjectID('IsStockBlock').value;
        var AvailableQty=GetObjectID('AvailableQty').value;
        var CurrentQty=GetObjectID('CurrentQty').value;

        if(IsStockBlock=="Y"){
            if ( Warehousetype == "WSC"|| Warehousetype == "WC") {
                var _List= $.grep(ucStockOfProduct, function (e) { if (e.SalesQuantity != "")return true; });
                var EditQty=0;

                for (var i = 0; i < _List.length; i++) {
                    EditQty=parseFloat(EditQty)+parseFloat(_List[i]["Quantity"]);
                }

                var remainQty=parseFloat(AvailableQty)+parseFloat(EditQty)-parseFloat(CurrentQty)-parseFloat("1");

                if(parseFloat(remainQty)<0){
                    IsProceed="N";
                }
            }
        }
    }   
    
    if(IsProceed=="Y"){
        var UOM =  GetObjectID('hdfUOM').value;
        var _SalesQuantity=ucStockOfProduct.find(o => o.SrlNo === ID).SalesQuantity;

        var criteria = [
            { Field: "SrlNo", Values: ID }
        ];
        var filteredJson = flexFilter(ucStockOfProduct, criteria);

        var _DeleteLoopID = parseInt(getMax(filteredJson, "LoopID"));
        var _DeleteQuantity = parseFloat(getMax(filteredJson, "Quantity"));
        var _Quantity=_DeleteQuantity-1;    

        if(_SalesQuantity==""){
            criteria = [
                { Field: "LoopID", Values: _DeleteLoopID }
            ];
            filteredJson = flexFilter(ucStockOfProduct, criteria);
        
            $.grep(filteredJson, function (e) { e.Quantity = _Quantity; });
            $.grep(filteredJson, function (e) { if (e.SalesQuantity != "") e.SalesQuantity = _Quantity + " " + UOM; });
            removeStockJson(ucStockOfProduct, function () { return this.SrlNo == ID; });
        }
        else{                
            var _WarehouseName=ucStockOfProduct.find(o => o.SrlNo === ID).WarehouseName;
            var _Batch=ucStockOfProduct.find(o => o.SrlNo === ID).Batch;
            var _MfgDate=ucStockOfProduct.find(o => o.SrlNo === ID).MfgDate;
            var _ExpiryDate=ucStockOfProduct.find(o => o.SrlNo === ID).ExpiryDate;
            var _SalesQuantity=ucStockOfProduct.find(o => o.SrlNo === ID).SalesQuantity;
            var _Rate=ucStockOfProduct.find(o => o.SrlNo === ID).Rate;

            removeStockJson(ucStockOfProduct, function () { return this.SrlNo == ID; });
        
            criteria = [
                    { Field: "LoopID", Values: _DeleteLoopID }
            ];
            filteredJson = flexFilter(ucStockOfProduct, criteria);  

            if (filteredJson.length > 0) {     
                $.grep(filteredJson, function (e) { e.Quantity = _Quantity; });

                var _MinSrlID=parseInt(getMin(filteredJson, "SrlNo"));
                $.grep(ucStockOfProduct, function (e) { if (e.SrlNo == _MinSrlID) e.WarehouseName = _WarehouseName; });
                $.grep(ucStockOfProduct, function (e) { if (e.SrlNo == _MinSrlID) e.Batch = _Batch; });
                $.grep(ucStockOfProduct, function (e) { if (e.SrlNo == _MinSrlID) e.MfgDate = _MfgDate; });
                $.grep(ucStockOfProduct, function (e) { if (e.SrlNo == _MinSrlID) e.ExpiryDate = _ExpiryDate; });
                $.grep(ucStockOfProduct, function (e) { if (e.SrlNo == _MinSrlID) e.Rate = _Rate; });
                $.grep(ucStockOfProduct, function (e) { if (e.SrlNo == _MinSrlID) e.SalesQuantity = _Quantity + " " + UOM; });
            }
        }

        ucStockDeatils();
    }
    else{
        jAlert("Stock going negative. Cannot Proceed.");
    }
}

function removeStockJson(arr, func) {
    for (var i = 0; i < arr.length; i++) {
        if (func.call(arr[i])) {
            arr.splice(i, 1);
            return arr;
        }
    }
}

function ucStockDeatils() {
    var IsBarcodeGenerator=GetObjectID('uchdfIsBarcodeGenerator').value;
    var StockType = GetObjectID('uchdfWarehousetype').value;
    var ProductSrlNo = GetObjectID('uchdfProductSrlNo').value;
    var StockDetails = $.grep(ucStockOfProduct, function (element, index) { return element.Product_SrlNo == ProductSrlNo });
    var StockHearder = [];

    if (StockType == "W") {
        StockHearder = ["WarehouseName", "SalesQuantity"];
    }
    else if (StockType == "WC") {
        StockHearder = ["WarehouseName","SerialNo", "Barcode", "SalesQuantity"];
    }
    else if (StockType == "B") {
        StockHearder = ["ViewBatch", "ViewMfgDate", "ViewExpiryDate", "SalesQuantity"];
    }
    else if (StockType == "S") {
        StockHearder = ["SalesQuantity","SerialNo"];
    }
    else if (StockType == "WB") {
        StockHearder = ["WarehouseName", "ViewBatch", "ViewMfgDate", "ViewExpiryDate", "SalesQuantity" ];
    }
    else if (StockType == "WS") {
        StockHearder = ["WarehouseName","SerialNo", "SalesQuantity"];
    }
    else if (StockType == "WBS") {
        StockHearder = ["WarehouseName", "ViewBatch", "ViewMfgDate", "ViewExpiryDate","SerialNo", "SalesQuantity"];
    }
    else if (StockType == "WSC") {
        StockHearder = ["WarehouseName","SerialNo", "Barcode", "SalesQuantity"];
    }
    else if (StockType == "BS") {
        StockHearder = ["ViewBatch", "ViewMfgDate", "ViewExpiryDate", "SerialNo", "SalesQuantity"];
    }
    
    var IsRateExists = GetObjectID('uchdfIsRateExists').value;    
    if(IsRateExists=="Y"){
        var index = StockHearder.indexOf("ucSalesQuantity")+1;
        StockHearder.splice( index, 0, "Rate");
    }

    StockDetails.sort(sortByMultipleKey(['LoopID', 'SrlNo']));
    var td_width=parseFloat(100/StockHearder.length).toFixed(2)+"%";
    
    // EXTRACT VALUE FOR HTML HEADER. 
    var col = [];

    if (StockDetails.length > 0) {
        var temp_col = [];

        for (var i = 0; i < StockDetails.length; i++) {
            for (var key in StockDetails[i]) {
                if (temp_col.indexOf(key) === -1) {
                    if (StockHearder.indexOf(key) > -1) {
                        temp_col.push(key);
                    }
                }
            }
        }

        for (var i = 0; i < StockHearder.length; i++) {
            var key=StockHearder[i];
            if(temp_col.indexOf(key) > -1) {
                col.push(key);
            }
        }
    }
    else {
        var arrayLength = StockHearder.length;
        for (var i = 0; i < arrayLength; i++) {
            col.push(StockHearder[i]);
        }
    }
    col.push("Action");

    // CREATE DYNAMIC TABLE.
    var table = document.createElement("table");
    table.setAttribute("class", "dynamicPopupTbl back scroll");

    // CREATE HTML TABLE HEADER ROW USING THE EXTRACTED HEADERS ABOVE.
    var tr = table.insertRow(-1);                   // TABLE ROW.
    
    var header = table.createTHead();
    var row = header.insertRow(0);
    for (var i = 0; i < col.length; i++) {
        //var cell = row.insertCell(parseInt(i));
        //cell.innerHTML = col[i];

        var th = document.createElement("th");      // TABLE HEADER.
        th.width=td_width;
        
        if(col[i]=="SalesQuantity") th.style.textAlign="right";
        else if(col[i]=="Rate") th.style.textAlign="right";

        if(col[i]=="WarehouseName") th.innerHTML ="Warehouse"; 
        else if(col[i]=="SalesQuantity") th.innerHTML ="Quantity";
        else if(col[i]=="ViewBatch") th.innerHTML ="Batch Number";
        else if(col[i]=="ViewMfgDate") th.innerHTML ="Mfg Date";
        else if(col[i]=="ViewExpiryDate") th.innerHTML ="Expiry Date";
        else if(col[i]=="SerialNo") th.innerHTML ="Serial Number";
        else th.innerHTML = col[i];

        row.appendChild(th);
    }

    // ADD JSON DATA TO THE TABLE AS ROWS.
    if (StockDetails.length > 0) {
        for (var i = 0; i < StockDetails.length; i++) {
            tr = table.insertRow(-1);

            var ID = StockDetails[i]["SrlNo"];
            var style=StockDetails[i]["IsOutStatus"];

            for (var j = 0; j < col.length; j++) {
                var tabCell = tr.insertCell(-1);
                tabCell.width=td_width;

                if (col[j] == "Action") {                    
                    if(IsBarcodeGenerator=="N"){
                        if(style=="display:none"){
                            tabCell.innerHTML = "Used";
                        }
                        else{
                            var EVENT = "removeRow(" + ID + ")";
                            var anchor=document.createElement('a');
                            anchor.setAttribute('onclick', EVENT);
                            anchor.setAttribute('title', 'Delete');
                            anchor.href='#';

                            var element = document.createElement("img");
                            element.setAttribute("src", "/assests/images/crs.png");
                            anchor.appendChild(element);

                            tabCell.appendChild(anchor);
                        }
                    }
                }
                else {
                    if (col[j] == "SalesQuantity") tabCell.style.textAlign="right";
                    else if (col[j] == "Rate") tabCell.style.textAlign="right";

                    tabCell.innerHTML = StockDetails[i][col[j]];
                }
            }
        }
    }

    // FINALLY ADD THE NEWLY CREATED TABLE WITH JSON DATA TO A CONTAINER.
    var divContainer = document.getElementById("ucshowData");
    divContainer.innerHTML = "";
    divContainer.appendChild(table);
}

function getParameterByName(name, url) {
    if (!url) url = window.location.href;
    name = name.replace(/[\[\]]/g, "\\$&");
    var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
        results = regex.exec(url);
    if (!results) return null;
    if (!results[2]) return '';
    return decodeURIComponent(results[2].replace(/\+/g, " "));
}