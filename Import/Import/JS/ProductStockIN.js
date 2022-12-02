var StockOfProduct = [];
var warehouserateList=[];

function getMax(array, propName) {
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

function getMin(array, propName) {
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

function SortByLoop(x,y) {
    return ((x.LoopID == y.LoopID) ? 0 : ((x.LoopID > y.LoopID) ? 1 : -1 ));
}

function sortByMultipleKey(keys) {
    return function(a, b) {
        if (keys.length == 0) return 0; // force to equal if keys run out
        key = keys[0]; // take out the first key
        if (a[key] < b[key]) return -1; // will be 1 if DESC
        else if (a[key] > b[key]) return 1; // will be -1 if DESC
        else return sortByMultipleKey(keys.slice(1))(a, b);
    }
}

function flexFilter(arr, info) {
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

function GetDateFormat(today) {
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

function CreateStock() {
    $('#txtBatch').val('');
    ctxtQty.SetValue(0);
    ctxtRate.SetValue(0);
    ctxtMfgDate.SetDate(null);
    ctxtExprieyDate.SetDate(null);
    $('#txtSerial').val('');

    //StockOfProduct=$.grep(StockOfProduct, function (e) { if (e.Status!="D")  return true; });
    var defaultWarehouse = GetObjectID('hdndefaultWarehouse').value;
    if (defaultWarehouse != "") document.getElementById('ddlWarehouse').value = defaultWarehouse;

    var Warehousetype = GetObjectID('hdfWarehousetype').value;
    var IsRateExists = GetObjectID('hdfIsRateExists').value;    

    if (Warehousetype == "W" || Warehousetype == "WC") {
        _div_Warehouse.style.display = 'block';
        _div_Batch.style.display = 'none';
        _div_Manufacture.style.display = 'none';
        _div_Expiry.style.display = 'none';
        _div_Quantity.style.display = 'block';
        _div_Serial.style.display = 'none';
        _div_Upload.style.display = 'none';
        _div_Break.style.display = 'none';
    }
    else if (Warehousetype == "B") {
        _div_Warehouse.style.display = 'none';
        _div_Batch.style.display = 'block';
        _div_Manufacture.style.display = 'block';
        _div_Expiry.style.display = 'block';
        _div_Quantity.style.display = 'block';
        _div_Serial.style.display = 'none';
        _div_Upload.style.display = 'none';
        _div_Break.style.display = 'none';
    }
    else if (Warehousetype == "S") {
        _div_Warehouse.style.display = 'none';
        _div_Batch.style.display = 'none';
        _div_Manufacture.style.display = 'none';
        _div_Expiry.style.display = 'none';
        _div_Quantity.style.display = 'none';
        _div_Serial.style.display = 'block';
        _div_Upload.style.display = 'none';
        _div_Break.style.display = 'none';
    }
    else if (Warehousetype == "WB") {
        _div_Warehouse.style.display = 'block';
        _div_Batch.style.display = 'block';
        _div_Manufacture.style.display = 'block';
        _div_Expiry.style.display = 'block';
        _div_Quantity.style.display = 'block';
        _div_Serial.style.display = 'none';
        _div_Upload.style.display = 'none';
        _div_Break.style.display = 'none';
    }
    else if (Warehousetype == "WS" || Warehousetype == "WSC") {
        _div_Warehouse.style.display = 'block';
        _div_Batch.style.display = 'none';
        _div_Manufacture.style.display = 'none';
        _div_Expiry.style.display = 'none';
        _div_Quantity.style.display = 'none';
        _div_Serial.style.display = 'block';
        _div_Upload.style.display = 'none';
        _div_Break.style.display = 'none';
    }
    else if (Warehousetype == "WBS") {
        _div_Warehouse.style.display = 'block';
        _div_Batch.style.display = 'block';
        _div_Manufacture.style.display = 'block';
        _div_Expiry.style.display = 'block';
        _div_Quantity.style.display = 'none';
        _div_Serial.style.display = 'block';
        _div_Upload.style.display = 'none';
        _div_Break.style.display = 'block';
    }
    else if (Warehousetype == "BS") {
        _div_Warehouse.style.display = 'none';
        _div_Batch.style.display = 'block';
        _div_Manufacture.style.display = 'block';
        _div_Expiry.style.display = 'block';
        _div_Quantity.style.display = 'none';
        _div_Serial.style.display = 'block';
        _div_Upload.style.display = 'none';
        _div_Break.style.display = 'none';
    }
    else {
        _div_Warehouse.style.display = 'none';
        _div_Batch.style.display = 'none';
        _div_Manufacture.style.display = 'none';
        _div_Expiry.style.display = 'none';
        _div_Quantity.style.display = 'none';
        _div_Serial.style.display = 'none';
        _div_Upload.style.display = 'none';
    }

    if(IsRateExists=="Y"){
        _div_Rate.style.display = 'block';

        var List = $.grep(warehouserateList, function (e) { return e.WarehouseID == defaultWarehouse; })

        if (List.length > 0) {
            var Rate = List[0].Rate;
            ctxtRate.SetValue(Rate);
        }
        else {
            ctxtRate.SetValue("0");
        }
    }
    else{
        _div_Rate.style.display = 'none';
    }

    StockDeatils();
    SetFocus("Add");
}

function Serialkeydown(e){
    if (e.code == "Enter" || e.code == "NumpadEnter") {
        SaveStock();
    }
}

function SaveStock() {
    var StockType = GetObjectID('hdfWarehousetype').value;
    var ProductSrlNo = GetObjectID('hdfProductSrlNo').value;
    var ProductID= GetObjectID('hdfProductID').value;
    var UOM =  GetObjectID('hdfUOM').value;
    var ServiceURL =  GetObjectID('hdfServiceURL').value;
    var Branch =  GetObjectID('hdfBranch').value;

    var WarehouseID = $('#ddlWarehouse').val();
    var WarehouseName = $("option:selected", '#ddlWarehouse').text();
    var Batch = $("#txtBatch").val().trim();
    var Qty =ctxtQty.GetValue();   
    var MfgDate = (ctxtMfgDate.GetValue() != null) ? ctxtMfgDate.GetValue() : "";
    var ExprieyDate = (ctxtExprieyDate.GetValue() != null) ? ctxtExprieyDate.GetValue() : "";
    var Serial = $("#txtSerial").val().trim();
    var Rate =ctxtRate.GetValue();  

    MfgDate = GetDateFormat(MfgDate);
    ExprieyDate = GetDateFormat(ExprieyDate);

    if (StockType == "W" ||StockType == "WS" || StockType == "WBS" || StockType == "WB" || StockType == "WC"|| StockType == "WSC"){
        if(WarehouseName==null || WarehouseName==""){
            $("#rfvWarehouse").css("display", "block");
            return false;
        }
        else{
            $("#rfvWarehouse").css("display", "none");
        }
    }
    
    if (StockType == "B" ||StockType == "BS" || StockType == "WBS" || StockType == "WB"){
        if(Batch==""){
            $("#rfvBatch").css("display", "block");
            return false;
        }
        else{
            $("#rfvBatch").css("display", "none");
        }
    }

    if (StockType == "BS" || StockType == "WBS" || StockType == "WS" || StockType == "S" || StockType == "WSC"){
        if(Serial==""){
            $("#rfvSerial").css("display", "block");
            return false;
        }
        else{
            $("#rfvSerial").css("display", "none");
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
            var serialfilteredJson = flexFilter(StockOfProduct, serialCriteria);

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
                            serialfilteredJson = flexFilter(StockOfProduct, serialCriteria);

                            if(serialfilteredJson.length == 0){
                                saveStockData(StockType,ProductSrlNo,ProductID,UOM,WarehouseID,WarehouseName,Batch,Qty,MfgDate,ExprieyDate,Serial,Rate);
                            }
                        }
                        else{
                            jAlert("Duplicate Serial. Cannot Proceed.", "Alert", function () { SetFocus("Save"); });        
                        }
                    }
                });
            }
            else{
                jAlert("Duplicate Serial. Cannot Proceed.", "Alert", function () { SetFocus("Save"); });
            }
        }
    }
    else if (StockType == "W" ||StockType == "WC" || StockType == "WB" || StockType == "B"){
        saveStockData(StockType,ProductSrlNo,ProductID,UOM,WarehouseID,WarehouseName,Batch,Qty,MfgDate,ExprieyDate,Serial,Rate);
    }
}

function saveStockData(StockType,ProductSrlNo,ProductID,UOM,WarehouseID,WarehouseName,Batch,Qty,MfgDate,ExprieyDate,Serial,Rate){
    var criteria = [
                    { Field: "Product_SrlNo", Values: ProductSrlNo },
                    { Field: "WarehouseID", Values: WarehouseID },
                    { Field: "Batch", Values: Batch }
    ];
    var filteredJson = flexFilter(StockOfProduct, criteria);

    if (filteredJson.length == 0) {
        // Save Data In Json
        var _SrlNo = parseInt(getMax(StockOfProduct, "SrlNo")) + 1;
        var _LoopID = parseInt(getMax(StockOfProduct, "LoopID")) + 1;
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
        StockOfProduct.push(ProductStock);
    }
    else {
        // Save Data In Json
        var _SrlNo = parseInt(getMax(StockOfProduct, "SrlNo")) + 1;
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
            StockOfProduct.push(ProductStock);

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

    StockDeatils();
    SetFocus("Save");

    //Surojit 19-03-2019 for UOM Conversion
   
    ///SetUOMConversionArray(WarehouseID);

    //Surojit 19-03-2019 for UOM Conversion
}

function FinalSaveStock(){
    var ProductSrlNo = GetObjectID('hdfProductSrlNo').value;
    
    var Criteria = [
      { Field: "Product_SrlNo", Values: ProductSrlNo }
    ];
    var filteredJson = flexFilter(StockOfProduct, Criteria);
    var getQuantity = parseFloat(getMax(filteredJson, "Quantity"));
    var entryQuantity = parseFloat(GetObjectID('hdnProductQuantity').value);

    if (getQuantity == entryQuantity) {
        $.grep(StockOfProduct, function (e) { if (e.Product_SrlNo == ProductSrlNo) e.Status = "I"; });
        $('#ProductStock').modal('hide');
        grid.batchEditApi.StartEdit(Warehouseindex, 8);
    }
    else{
        jAlert("Purchase Quantity must be equal to Warehouse Quantity.");    
    }
}

function SetFocus(Time) {
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

    for (var i = 0; i < StockOfProduct.length; i++) {
        var ProductID = StockOfProduct[i]["Product_SrlNo"];
        var WarehouseID = StockOfProduct[i]["WarehouseID"];
        var BatchID = StockOfProduct[i]["Batch"];

        if (ProductID == Product && WarehouseID == Warehouse && BatchID == Batch) {
            if (Property = "LoopID") {
                PropertyValue = StockOfProduct[i]["LoopID"];
            }
            else if (Property = "Quantity") {
                PropertyValue = StockOfProduct[i]["Quantity"];
            }

            break;
        }
        else {
            if (Property = "LoopID") {
                PropertyValue = StockOfProduct[i]["LoopID"];
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
                var _List= $.grep(StockOfProduct, function (e) { if (e.SalesQuantity != "")return true; });
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
        var _SalesQuantity=StockOfProduct.find(o => o.SrlNo === ID).SalesQuantity;

        var criteria = [
            { Field: "SrlNo", Values: ID }
        ];
        var filteredJson = flexFilter(StockOfProduct, criteria);

        var _DeleteLoopID = parseInt(getMax(filteredJson, "LoopID"));
        var _DeleteQuantity = parseFloat(getMax(filteredJson, "Quantity"));
        var _Quantity=_DeleteQuantity-1;    

        if(_SalesQuantity==""){
            criteria = [
                { Field: "LoopID", Values: _DeleteLoopID }
            ];
            filteredJson = flexFilter(StockOfProduct, criteria);
        
            $.grep(filteredJson, function (e) { e.Quantity = _Quantity; });
            $.grep(filteredJson, function (e) { if (e.SalesQuantity != "") e.SalesQuantity = _Quantity + " " + UOM; });
            removeStockJson(StockOfProduct, function () { return this.SrlNo == ID; });
        }
        else{                
            var _WarehouseName=StockOfProduct.find(o => o.SrlNo === ID).WarehouseName;
            var _Batch=StockOfProduct.find(o => o.SrlNo === ID).Batch;
            var _MfgDate=StockOfProduct.find(o => o.SrlNo === ID).MfgDate;
            var _ExpiryDate=StockOfProduct.find(o => o.SrlNo === ID).ExpiryDate;
            var _SalesQuantity=StockOfProduct.find(o => o.SrlNo === ID).SalesQuantity;
            var _Rate=StockOfProduct.find(o => o.SrlNo === ID).Rate;

            removeStockJson(StockOfProduct, function () { return this.SrlNo == ID; });
        
            criteria = [
                    { Field: "LoopID", Values: _DeleteLoopID }
            ];
            filteredJson = flexFilter(StockOfProduct, criteria);  

            if (filteredJson.length > 0) {     
                $.grep(filteredJson, function (e) { e.Quantity = _Quantity; });

                var _MinSrlID=parseInt(getMin(filteredJson, "SrlNo"));
                $.grep(StockOfProduct, function (e) { if (e.SrlNo == _MinSrlID) e.WarehouseName = _WarehouseName; });
                $.grep(StockOfProduct, function (e) { if (e.SrlNo == _MinSrlID) e.Batch = _Batch; });
                $.grep(StockOfProduct, function (e) { if (e.SrlNo == _MinSrlID) e.MfgDate = _MfgDate; });
                $.grep(StockOfProduct, function (e) { if (e.SrlNo == _MinSrlID) e.ExpiryDate = _ExpiryDate; });
                $.grep(StockOfProduct, function (e) { if (e.SrlNo == _MinSrlID) e.Rate = _Rate; });
                $.grep(StockOfProduct, function (e) { if (e.SrlNo == _MinSrlID) e.SalesQuantity = _Quantity + " " + UOM; });
            }
        }

        StockDeatils();
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

function StockDeatils() {
    var IsBarcodeGenerator=GetObjectID('hdfIsBarcodeGenerator').value;
    var StockType = GetObjectID('hdfWarehousetype').value;
    var ProductSrlNo = GetObjectID('hdfProductSrlNo').value;
    var StockDetails = $.grep(StockOfProduct, function (element, index) { return element.Product_SrlNo == ProductSrlNo });
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
    
    var IsRateExists = GetObjectID('hdfIsRateExists').value;    
    if(IsRateExists=="Y"){
        var index = StockHearder.indexOf("SalesQuantity")+1;
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
                    //if(IsBarcodeGenerator=="N"){
                    //    if(style=="display:none"){
                    //        tabCell.innerHTML = "Used";
                    //    }
                    //    else{
                            var EVENT = "removeRow(" + ID + ")";
                            var anchor=document.createElement('a');
                            anchor.setAttribute('onclick', EVENT);
                            anchor.setAttribute('title', 'Delete');
                            anchor.href='#';

                            var element = document.createElement("img");
                            element.setAttribute("src", "/assests/images/crs.png");
                            anchor.appendChild(element);

                            tabCell.appendChild(anchor);
                       // }
                   // }
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
    var divContainer = document.getElementById("showData");
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