var StockOfProduct = [];
var warehouserateList=[];
var SerialList=[];
var SerialObj = [];




function deleteTax(Action,srl,productid) {
    var OtherDetail = {};
    OtherDetail.Action = Action;
    OtherDetail.srl = srl;
    OtherDetail.prodid = productid;


    $.ajax({
        type: "POST",
        url: "CustomerPendingDelivery.aspx/taxUpdatePanel_Callback",
        data: JSON.stringify(OtherDetail),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var Code = msg.d;

            if (Code != null) {
               
            }
        }
    });
}


function AvailableStockCheck(date,branch,productid) {
  
  


    $.ajax({
        type: "POST",
        url: "CustomerPendingDelivery.aspx/acpAvailableStock_Callback",
        data: JSON.stringify({date:date,branch:branch,productid:productid}),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async:false,
        success: function (msg) {
            var Code = msg.d;

            if (Code != null) {
               
                var AvlStk = Code + " " + $("#lblStkUOM").val();
              
                $("#lblAvailableSStk").val(Code);
                $("#lblAvailableStock").val(Code);
                $("#lblAvailableStockUOM").val($("#lblStkUOM").val());

            }
        }
    });
}




function WarehouseChange(){
    var type = GetObjectID('hdnStockProductType').value;
    var WarehouseID =  $('#ddlWarehouse').val();
    if (WarehouseID != null) {
        if (type == "WBS" || type == "WB") {
            BindStockBatch();
        }
        else if (type == "WS") {
            BindSerial();
        }
    }   
    

    //Rev Rajdip For wirehousewise aviable stock
    var ProductID = grid.GetEditor('ProductID').GetValue();
    var SpliteDetails = ProductID.split("||@||");
    var strProductID = SpliteDetails[0];
    var sl = grid.GetEditor("SrlNo").GetValue();
    var branch = $("#ddl_Branch").val();
    if (WarehouseID != null) {
        getwirehousewiseaviablestock(sl, strProductID, branch, WarehouseID)
    }
    //End Rev Rajdip


}
function BindSerial() {
    var ProductID = $('#hdnStockProductId').val();
    var objectToPass = {}
    objectToPass.BatchID = $('#ddlBatch').val();
    objectToPass.WarehouseID = $('#ddlWarehouse').val();
    objectToPass.Branch = $('#ddl_Branch').val();
    objectToPass.ProductID = $('#hdnStockProductId').val();
    objectToPass.LastFinYear = $('#hdnStockFinYear').val();
    objectToPass.LastCompany = $('#hdnStockLastCompany').val();
    objectToPass.Date = $('#dt_PLSales').val();
    SerialObj=[];

    $.ajax({
        type: "POST",
        url: "Services/Master.asmx/GetStockSerial",
        data: JSON.stringify(objectToPass),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (r) {
            JsonData = r.d;
           
            $.each(r.d, function () {
                item = {}
                item["Value"] = this['Value'];
                item["Text"] = this['Text'];                
                SerialObj.push(item);                
            });

            BindCurrentserialNo();            
        }
    });
}
function BindStockBatch() {

    var objectToPass = {}
    objectToPass.WarehouseID = $('#ddlWarehouse').val();
    objectToPass.Branch = $('#ddl_Branch').val();
    objectToPass.ProductID = $('#hdnStockProductId').val();
    objectToPass.LastFinYear = $('#hdnStockFinYear').val();
    objectToPass.LastCompany = $('#hdnStockLastCompany').val();
    $.ajax({
        type: "POST",
        url: "Services/Master.asmx/GetStockBatch",
        data: JSON.stringify(objectToPass),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (r) {
            var ddlBatch = $("[id*=ddlBatch]");
            ddlBatch.empty(); 
            ddlBatch.append($("<option></option>").val("0").html("Select"));
            $.each(r.d, function () {
                ddlBatch.append($("<option></option>").val(this['Value']).html(this['Text']));

            });
        }
    });
}

function BindCurrentserialNo()
{
    $('#lstSerial').empty();
    var lstSerial = $("[id*=lstSerial]");

    removeStockJson(SerialObj, function () { return this.Value == "0"; });   
    for (var i = 0; i < SerialObj.length; i++) {
        var SerialID = SerialObj[i]["Value"];
        var SerialNo = SerialObj[i]["Text"];
        lstSerial.append($("<option></option>").val(SerialID).html(SerialNo));
    }
    
    $('[id*=lstSerial]').multiselect('rebuild');
}

//function SearchSerial() {    

//    var returnedData = $.grep(SerialObj, function (element, index) {
//        var allSerialName = document.getElementById("lstSerial");
//        allSerialName.index.checked;
//        return element.Text == $('#txtSerialForStock').val();
//    });
//    if (returnedData.length == 0) {
//        jAlert("Not Exist");
//    }
//    else
//        jAlert("Exist");

//}

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
    // ctxtQty.SetValue(0);
    // ctxtRate.SetValue(0);
    //ctxtMfgDate.SetDate(null);
    //ctxtExprieyDate.SetDate(null);
    $('#txtSerial').val('');
    
    //var defaultWarehouse = GetObjectID('hdndefaultWarehouse').value;
    //if (defaultWarehouse != "") document.getElementById('ddlWarehouse').value = defaultWarehouse;

    var Warehousetype = GetObjectID('hdnStockProductType').value;
    var IsRateExists = GetObjectID('hdfIsRateExists').value;    

    if (Warehousetype == "W" || Warehousetype == "WC") {
        _div_Warehouse.style.display = 'block';
        _div_Batch.style.display = 'none';                  
        _div_Quantity.style.display = 'block';
        _div_Serial.style.display = 'none';
        //_div_text.style.display = 'none';
                    
    }
    else if (Warehousetype == "B") {
        _div_Warehouse.style.display = 'none';
        _div_Batch.style.display = 'block';                 
        _div_Quantity.style.display = 'block';
        _div_Serial.style.display = 'none';
       // _div_text.style.display = 'none';                   
    }
    else if (Warehousetype == "S") {
        _div_Warehouse.style.display = 'none';
        _div_Batch.style.display = 'none';               
        _div_Quantity.style.display = 'none';
        _div_Serial.style.display = 'block';
       // _div_text.style.display = 'block';                   
    }
    else if (Warehousetype == "WB") {
        _div_Warehouse.style.display = 'block';
        _div_Batch.style.display = 'block';                   
        _div_Quantity.style.display = 'block';
        _div_Serial.style.display = 'none';
        //_div_text.style.display = 'none';
                   
    }
    else if (Warehousetype == "WS" || Warehousetype == "WSC") {
        _div_Warehouse.style.display = 'block';
        _div_Batch.style.display = 'none';                 
        _div_Quantity.style.display = 'none';
        _div_Serial.style.display = 'block';
        //_div_text.style.display = 'none';
                  
    }
    else if (Warehousetype == "WBS") {
        _div_Warehouse.style.display = 'block';
        _div_Batch.style.display = 'block';                  
        _div_Quantity.style.display = 'none';
        _div_Serial.style.display = 'block';
       // _div_text.style.display = 'block';
                   
    }
    else if (Warehousetype == "BS") {
        _div_Warehouse.style.display = 'none';
        _div_Batch.style.display = 'block';                  
        _div_Quantity.style.display = 'none';
        _div_Serial.style.display = 'block';
       // _div_text.style.display = 'block';
                   
    }
    else {
        _div_Warehouse.style.display = 'none';
        _div_Batch.style.display = 'none';                  
        _div_Quantity.style.display = 'none';
        _div_Serial.style.display = 'none';
        //_div_text.style.display = 'none';
    }

    //if(IsRateExists=="Y"){
    //    _div_Rate.style.display = 'block';

    //    var List = $.grep(warehouserateList, function (e) { return e.WarehouseID == defaultWarehouse; })

    //    if (List.length > 0) {
    //        var Rate = List[0].Rate;
    //        ctxtRate.SetValue(Rate);
    //    }
    //    else {
    //        ctxtRate.SetValue("0");
    //    }
    //}
    //else{
    //    _div_Rate.style.display = 'none';
    //}

    StockDeatils();
    SetFocus("Add");
}

function Serialkeydown(e){
    if (e.code == "Enter" || e.code == "NumpadEnter") {

        var serialId="";
        var SerialName = $("#txtSerialForStock").val();  
        var returnedData = $.grep(SerialObj, function (element,index) {
              
            for (i = 0; i < document.getElementById("lstSerial").length; ++i){
                if (document.getElementById("lstSerial").options[i].innerHTML == SerialName){
                    // document.getElementById("lstSerial").options[i].check=true;
                    
                    serialId=i;
                    //$('#lstSerial').text(SerialName);
                }
            }
            
            return element.Text == $('#txtSerialForStock').val();
        });
        if (returnedData.length == 0) {
            jAlert("Not Exist");
        }
        else
        {          
            SaveStockText(returnedData[0].Value,SerialName);
        }      
    }
}

function SaveStockText(serial_Id,SerialName) {
    var StockType = GetObjectID('hdnStockProductType').value;
    var ProductSrlNo = GetObjectID('hdfProductSrlNo').value;
    var ProductID= GetObjectID('hdnStockProductId').value;
    var UOM =  GetObjectID('hdfUOM').value;  
    var Branch =  GetObjectID('ddl_Branch').value;
    var WarehouseID = $('#ddlWarehouse').val();
    var WarehouseName = $("option:selected", '#ddlWarehouse').text();
    var BatchID = $("#ddlBatch").val().trim();
    var BatchName = $("option:selected", '#ddlBatch').text();
    //var Qty =$("#txtQuty").val().trim();      
    var Qty=ctxtQuty.GetValue();
    var SerialId = serial_Id;    
    var Rate =ctxtRate.GetValue();    

    if (StockType == "W" ||StockType == "WS" || StockType == "WBS" || StockType == "WB" || StockType == "WC"|| StockType == "WSC"){
        if(WarehouseName==null || WarehouseName==""){
            $("#DelChWarehouse").css("display", "block");
            return false;
        }
        else{
            $("#DelChWarehouse").css("display", "none");
        }
    }    
    if (StockType == "B" ||StockType == "BS" || StockType == "WBS" || StockType == "WB"){
        if(BatchID==""){
            $("#DelChBatch").css("display", "block");
            return false;
        }
        else{
            $("#DelChBatch").css("display", "none");
        }
    }
    if (StockType == "BS" || StockType == "WBS" || StockType == "WS" || StockType == "S" || StockType == "WSC"){
        if(SerialId==""){
            $("#DelChSerial").css("display", "block");
            return false;
        }
        else{
            $("#DelChSerial").css("display", "none");
        }
    }
    if (StockType == "W" || StockType == "WB" || StockType == "B" || StockType == "WC" ){
        if(parseFloat(Qty)==0){
            $("#spnQuantity").css("display", "block");
            return false;
        }
        else{
            $("#spnQuantity").css("display", "none");
        }
    }
    if (StockType == "WS" || StockType == "WBS" || StockType == "BS" || StockType == "S"|| StockType == "WSC") {
        if(SerialId!=""){
            var serialCriteria = [{ Field: "SerialNo", Values: SerialId }];
            var serialfilteredJson = flexFilter(StockOfProduct, serialCriteria);

            if(serialfilteredJson.length == 0){                 

                saveStockData(StockType,ProductSrlNo,ProductID,UOM,WarehouseID,WarehouseName,BatchID,BatchName,Qty,SerialId,SerialName,Rate);               
            }
            else{
                jAlert(SerialName+ " Duplicate Serial. Cannot Proceed.", "Alert", function () { SetFocus("Save"); });
            }
        }
    }
    else 
        if (StockType == "W" ||StockType == "WC" || StockType == "WB" || StockType == "B"){
            saveStockData(StockType,ProductSrlNo,ProductID,UOM,WarehouseID,WarehouseName,BatchID,BatchName,Qty,SerialId,SerialName,Rate);
        }
    
   
    if (StockType == "WS" ||StockType == "WSC" || StockType == "WBS" || StockType == "BS" || StockType == "S") {        
        //var SerialId = $('#lstSerial').val();
        if(SerialId!="")
        {            
                removeStockJson(SerialObj, function () { return this.Value == SerialId; });            
        }
    }
    
    BindCurrentserialNo();
    StockDeatils();
    SetFocus("Save");
    $('#txtSerialForStock').val("");
}

function SaveStock() {
    var StockType = GetObjectID('hdnStockProductType').value;

    var ProductSrlNo = GetObjectID('hdfProductSrlNo').value;
   // var ProductSrlNo = GetObjectID('hdnStockProductId').value;
    var ProductID= GetObjectID('hdnStockProductId').value;
    var UOM =  GetObjectID('hdfUOM').value; 
    var Branch =  GetObjectID('ddl_Branch').value;
    var WarehouseID = $('#ddlWarehouse').val();
    var WarehouseName = $("option:selected", '#ddlWarehouse').text();
    var BatchID = $("#ddlBatch").val();
    if(BatchID==null||BatchID=="")
    {
        BatchID="0";
    }
    var BatchName = $("option:selected", '#ddlBatch').text();
   // var Qty =$("#txtQuty").val().trim();     
    var Qty =ctxtQuty.GetValue();
    var SerialId = $('#lstSerial').val();
    var SerialName = "";
    var AltQty=0;
    var AltUOM=0;
    //var allSerialName = document.getElementById("lstSerial");
    //var SerialName = "";
    //for (var i = 0; i < allSerialName.options.length; i++) {
    //    if (allSerialName.options[i].selected) {
    //        SerialName += allSerialName.options[i].innerHTML  + ",";

    //    }
    //}
    //var lastChar = SerialName.slice(-1);
    //if (lastChar == ',') {
    //    SerialName = SerialName.slice(0, -1);
    //}
    
    AltQty = (CtxtPacking.GetText() != null) ? CtxtPacking.GetText() : "0";
    AltUOM = (ccmbPackingUom1.GetValue() != null) ? ccmbPackingUom1.GetValue() : "0";

    var Rate =ctxtRate.GetValue();     

    if (StockType == "W" ||StockType == "WS" || StockType == "WBS" || StockType == "WB" || StockType == "WC"|| StockType == "WSC"){
        if(WarehouseName==null || WarehouseName==""||WarehouseName=="Select"){
            $("#DelChWarehouse").css("display", "block");
            return false;
        }
        else{
            $("#DelChWarehouse").css("display", "none");
        }
    }    
    if (StockType == "B" ||StockType == "BS" || StockType == "WBS" || StockType == "WB"){
        if(BatchID==""||BatchID==null||BatchID=="Select"||BatchID=="0"){
            $("#DelChBatch").css("display", "block");
            return false;
        }
        else{
            $("#DelChBatch").css("display", "none");
        }
    }
    if (StockType == "BS" || StockType == "WBS" || StockType == "WS" || StockType == "S" || StockType == "WSC"){
        if(SerialId==""||SerialId==null){
            $("#DelChSerial").css("display", "block");
            return false;
        }
        else{
            $("#DelChSerial").css("display", "none");
        }
    }
    if (StockType == "W" || StockType == "WB" || StockType == "B" || StockType == "WC" ){
        if(parseFloat(Qty)==0||Qty==""){
            $("#spnQuantity").css("display", "block");
            return false;
        }
        else{
            $("#spnQuantity").css("display", "none");
        }
    }
   

    if (StockType == "WS" || StockType == "WBS" || StockType == "BS" || StockType == "S"|| StockType == "WSC") {
        if(SerialId!=""||SerialId!=null){
            
            var SerialId="";
            var allSerialName = document.getElementById("lstSerial");               
            for (var i = 0; i < allSerialName.options.length; i++) {
                if (allSerialName.options[i].selected) {
                    SerialName = allSerialName.options[i].innerHTML ;
                    SerialId = allSerialName.options[i].value ;

                    var serialCriteria = [{ Field: "SerialId", Values: SerialId }];
                    var serialfilteredJson = flexFilter(StockOfProduct, serialCriteria);

                    if(serialfilteredJson.length == 0){     
                        saveStockData(StockType,ProductSrlNo,ProductID,UOM,WarehouseID,WarehouseName,BatchID,BatchName,Qty,SerialId,SerialName,Rate,AltQty,AltUOM);
                    }
                    else{
                        jAlert(SerialName+ "Duplicate Serial. Cannot Proceed.", "Alert", function () { SetFocus("Save"); });
                    }
                }
            }


            //var objectToPass = {}
            //objectToPass.SerialNo = Serial;
            //objectToPass.ProductID = ProductID;
            //objectToPass.BranchID = Branch;

            //$.ajax({
            //    type: "POST",
            //    url: ServiceURL,//"Services/Master.asmx/CheckDuplicateSerial",
            //    data: JSON.stringify(objectToPass),
            //    contentType: "application/json; charset=utf-8",
            //    dataType: "json",
            //    success: function (msg) {
            //        if(msg.d==0){
            //            serialCriteria = [{ Field: "SerialNo", Values: Serial }];
            //            serialfilteredJson = flexFilter(StockOfProduct, serialCriteria);

            //            if(serialfilteredJson.length == 0){
            //                saveStockData(StockType,ProductSrlNo,ProductID,UOM,WarehouseID,WarehouseName,Batch,Qty,MfgDate,ExprieyDate,Serial,Rate);
            //            }
            //        }
            //        else{
            //            jAlert("Duplicate Serial. Cannot Proceed.", "Alert", function () { SetFocus("Save"); });        
            //        }
            //    }
            //});
        }
            
    }
    
    else 
        if (StockType == "W" ||StockType == "WC" || StockType == "WB" || StockType == "B"){
            saveStockData(StockType,ProductSrlNo,ProductID,UOM,WarehouseID,WarehouseName,BatchID,BatchName,Qty,SerialId,SerialName,Rate,AltQty,AltUOM);
        }


    if (StockType == "WS" ||StockType == "WSC" || StockType == "WBS" || StockType == "BS" || StockType == "S") {        
        var SerialId = $('#lstSerial').val();
        if(SerialId!=""||SerialId!=null){
            for (var j = 0; j < SerialId.length; j++) {
                removeStockJson(SerialObj, function () { return this.Value == SerialId[j]; });  
            }
        }
    }
    
    BindCurrentserialNo();
    StockDeatils();
    SetFocus("Save");
}

function saveStockData(StockType,ProductSrlNo,ProductID,UOM,WarehouseID,WarehouseName,BatchID,BatchName,Qty,SerialId,SerialName,Rate,AltQty,AltUOM){
    var criteria = [
                    { Field: "Product_SrlNo", Values: ProductSrlNo },
                    { Field: "WarehouseID", Values: WarehouseID },
                    { Field: "BatchID", Values: BatchID }
                   
    ];
    var filteredJson = flexFilter(StockOfProduct, criteria);

    if (filteredJson.length == 0) {
        // Save Data In Json
        var _SrlNo = parseInt(getMax(StockOfProduct, "SrlNo")) + 1;
        var _LoopID = parseInt(getMax(StockOfProduct, "LoopID")) + 1;
        var _Quantity = "1.0000";
        var AltQty=AltQty;
        var AltUOM=AltUOM;
        if (StockType == "WS" || StockType == "WBS" || StockType == "BS" || StockType == "S"|| StockType == "WSC") {
            _Quantity = "1.0000";
        }
        else {
            _Quantity = Qty;
        }

        if(BatchID==""|| BatchID==null)
        {
            BatchID="0";
        }
        if(SerialId==""|| SerialId==null)
        {
            SerialId="0";
        }
        
        var ProductStock = { Product_SrlNo: ProductSrlNo, SrlNo: _SrlNo, WarehouseID: WarehouseID, WarehouseName: WarehouseName,
            Quantity: _Quantity,ViewQty:_Quantity, SalesQuantity: _Quantity + " " + UOM, BatchID:BatchID,BatchName:BatchName, 
            SerialId:SerialId,SerialName:SerialName, LoopID: _LoopID, Status: "D",Msg:"",AltQty:AltQty,AltUOM:AltUOM
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

            if(BatchID==""|| BatchID==null)
            {
                BatchID="0";
            }
            if(SerialId==""|| SerialId==null)
            {
                SerialId="0";
            }
            var ProductStock = { Product_SrlNo: ProductSrlNo, SrlNo: _SrlNo, WarehouseID: WarehouseID, WarehouseName: "",
                Quantity: _Quantity,ViewQty:_Quantity, SalesQuantity: "", BatchID:BatchID,BatchName:BatchName, 
                SerialId:SerialId,SerialName:SerialName, LoopID: _LoopID, Status: "D",Msg:"",AltQty:AltQty,AltUOM:AltUOM
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
    var Warehousetype = GetObjectID('hdnStockProductType').value;

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
            //$("#txtQuty").val(0.0000);   
            ctxtQuty.SetValue(0.0000);
            CtxtPacking.SetValue(0.0000);
            setTimeout(function () { ctxtQuty.Focus(); }, 500);
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
        
        item = {}
        item["Value"] = filteredJson[0].SerialId;
        item["Text"] = filteredJson[0].SerialName;
        SerialObj.push(item);
        BindCurrentserialNo();

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
    //var IsBarcodeGenerator=GetObjectID('hdfIsBarcodeGenerator').value;
    var StockType = GetObjectID('hdnStockProductType').value;
    var ProductSrlNo = GetObjectID('hdfProductSrlNo').value;
    var StockDetails = $.grep(StockOfProduct, function (element, index) { return element.Product_SrlNo == ProductSrlNo });
    var StockHearder = [];

    if (StockType == "W") {
        StockHearder = ["WarehouseName", "SalesQuantity"];
    }
    else if (StockType == "WC") {
        StockHearder = ["WarehouseName","SerialName", "Barcode", "SalesQuantity"];
    }
    else if (StockType == "B") {
        StockHearder = ["ViewBatch", "ViewMfgDate", "ViewExpiryDate", "SalesQuantity"];
    }
    else if (StockType == "S") {
        StockHearder = ["SalesQuantity","SerialName"];
    }
    else if (StockType == "WB") {
        StockHearder = ["WarehouseName", "ViewBatch", "ViewMfgDate", "ViewExpiryDate", "SalesQuantity" ];
    }
    else if (StockType == "WS") {
        StockHearder = ["WarehouseName","SerialName", "SalesQuantity"];
    }
    else if (StockType == "WBS") {
        StockHearder = ["WarehouseName", "BatchName", "SerialName", "SalesQuantity"];
    }
    else if (StockType == "WSC") {
        StockHearder = ["WarehouseName","SerialName", "Barcode", "SalesQuantity"];
    }
    else if (StockType == "BS") {
        StockHearder = ["ViewBatch", "ViewMfgDate", "ViewExpiryDate", "SerialName", "SalesQuantity"];
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
        else if(col[i]=="BatchName") th.innerHTML ="Batch Number";        
        else if(col[i]=="SerialName") th.innerHTML ="Serial Number";
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
                    //    }
                    //}
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


function ChkDataDigitCount(e) {
    var data = $(e).val();
    $(e).val(parseFloat(data).toFixed(4));
}

function ChangePackingByQuantityinjs() {
    if ($("#hdnShowUOMConversionInEntry").val() == "1")
    {
    var Productdetails = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
    var SpliteDetails = Productdetails.split("||@||");
    var otherdet = {};
    var ProductID = Productdetails.split("||@||")[0];
    otherdet.ProductID = ProductID;
    if (Productdetails != "") {
        $.ajax({
            type: "POST",
            url: "CustomerPendingDelivery.aspx/GetPackingQuantityWarehouse",
            data: JSON.stringify(otherdet),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (msg) {

                if (msg.d.length != 0) {
                    var packingQuantity = msg.d[0].packing_quantity;
                    var sProduct_quantity = msg.d[0].sProduct_quantity;
                    var isOverideConvertion = msg.d[0].isOverideConvertion;
                }
                else {
                    var packingQuantity = 0;
                    var sProduct_quantity = 0;
                    var isOverideConvertion = 0;
                }
                var uomfactor = 0
                if (sProduct_quantity != 0 && packingQuantity != 0) {
                    uomfactor = parseFloat(packingQuantity / sProduct_quantity).toFixed(4);
                    $('#hdnuomFactor').val(parseFloat(packingQuantity / sProduct_quantity));
                }
                else {
                    $('#hdnuomFactor').val(0);
                }

                $('#hdnpackingqty').val(packingQuantity);
                $('#hdnisOverideConvertion').val(isOverideConvertion);
                //var uomfac_Qty_to_stock = $('#hddnuomFactor').val();
                //var Qty = $("#UOMQuantity").val();
                //var calcQuantity = parseFloat(Qty * uomfac_Qty_to_stock).toFixed(4);

                ////$("#AltUOMQuantity").val(calcQuantity);

                //cAltUOMQuantity.SetValue(calcQuantity);

            }
        });
    }

    var Quantity = ctxtQuty.GetValue();
    var packing = $('#txtAltQuantity').val();
    if (packing == null || packing == '') {
        $('#txtAltQuantity').val(parseFloat(0).toFixed(4));
        packing = $('#txtAltQuantity').val();
    }

    if (Quantity == null || Quantity == '') {
        $(e).val(parseFloat(0).toFixed(4));
        Quantity = ctxtQuty.GetValue();
    }
    var packingqty = parseFloat($('#hdnpackingqty').val()).toFixed(4);

    //Rev Subhra 05-03-2019
    //var calcQuantity = parseFloat(Quantity * packingqty).toFixed(4);
    var uomfac_Qty_to_stock = $('#hdnuomFactor').val();
    //var uomfac_Qty_to_stock = $('#hdnpackingqty').val();
    var calcQuantity = parseFloat(Quantity * uomfac_Qty_to_stock).toFixed(4);
    //End of Rev Subhra 05-03-2019
    //$('#txtAlterQty1').val(calcQuantity);
    CtxtPacking.SetText(calcQuantity);

    ChkDataDigitCount(Quantity);
}
}
function ChangeQuantityByPacking1() {
    if ($("#hdnShowUOMConversionInEntry").val() == "1")
    {
    var Productdetails = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
    var SpliteDetails = Productdetails.split("||@||");
    var otherdet = {};
    var ProductID = Productdetails.split("||@||")[0];
    otherdet.ProductID = ProductID;
    if (Productdetails != "") {
        $.ajax({
            type: "POST",
            url: "CustomerPendingDelivery.aspx/GetPackingQuantityWarehouse",
            data: JSON.stringify(otherdet),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (msg) {

                if (msg.d.length != 0) {
                    var packingQuantity = msg.d[0].packing_quantity;
                    var sProduct_quantity = msg.d[0].sProduct_quantity;
                    var isOverideConvertion = msg.d[0].isOverideConvertion;
                }
                else {
                    var packingQuantity = 0;
                    var sProduct_quantity = 0;
                    var isOverideConvertion = 0;
                }
                var uomfactor = 0
                if (sProduct_quantity != 0 && packingQuantity != 0) {
                    uomfactor = parseFloat(packingQuantity / sProduct_quantity).toFixed(4);
                    $('#hdnuomFactor').val(parseFloat(packingQuantity / sProduct_quantity));
                }
                else {
                    $('#hdnuomFactor').val(0);
                }

                $('#hdnpackingqty').val(packingQuantity);
                $('#hdnisOverideConvertion').val(isOverideConvertion);
                //var uomfac_Qty_to_stock = $('#hddnuomFactor').val();
                //var Qty = $("#UOMQuantity").val();
                //var calcQuantity = parseFloat(Qty * uomfac_Qty_to_stock).toFixed(4);

                ////$("#AltUOMQuantity").val(calcQuantity);

                //cAltUOMQuantity.SetValue(calcQuantity);

            }
        })

    }

    var isOverideConvertion = $('#hdnisOverideConvertion').val();
    if (isOverideConvertion == "true") {
        isOverideConvertion = '1';
    }
    if (isOverideConvertion == '1') {
        var packing = CtxtPacking.GetValue();
        var Quantity = ctxtQuty.GetValue();
        if (packing == null || packing == '') {
            $(e).val(parseFloat(0).toFixed(4));
            packing = CtxtPacking.GetValue();
        }

        if (Quantity == null || Quantity == '') {
            ctxtQuty.SetValue(parseFloat(0).toFixed(4));

            Quantity = ctxtQuty.GetValue();
        }
        var packingqty = parseFloat($('#hdnpackingqty').val()).toFixed(4);


        //Rev Subhra 06-03-2019
        // var calcQuantity = parseFloat(packing / packingqty).toFixed(4);
        var uomfac_stock_to_qty = $('#hdnuomFactor').val();
        //var uomfac_stock_to_qty = $('#hdnpackingqty').val();
        //Rev Surojit 21-05-2019
        var calcQuantity = 0;
        if (parseFloat(uomfac_stock_to_qty) != 0) {
            calcQuantity = parseFloat(packing / uomfac_stock_to_qty).toFixed(4);
        }
        //End of Rev Surojit 21-05-2019

        //End of Rev Subhra 06-03-2019
        ctxtQuty.SetValue(calcQuantity);
    }
    ChkDataDigitCount(Quantity);
}
}