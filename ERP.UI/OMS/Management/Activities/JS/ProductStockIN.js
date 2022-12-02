var StockOfProduct = [];
var warehouserateList=[];

var countSecondUom = 1;

var EditFlag=0;
var ModuleNameSecondUOM="";

var AltQtyModule = "";


function SavePOESecondUOMDetails(){

    
    
    
    for(var i=0;i<SecondUOM.length;i++){
        //var FilterSerialMain = $.grep(aarr, function (e) { return e.slno == SecondUOM[i].WarehouseID && e.productid==SecondUOM[i].ProductId });
        //var FilterSerial = $.grep(SecondUOM, function (e) { return e.WarehouseID == SecondUOM[i].WarehouseID && e.ProductId==SecondUOM[i].ProductId });

        //if(FilterSerialMain[0].packing!=FilterSerial.length){
        //    jAlert('Qunatity mismatched found . Can not proceed','Alert');
        //    return;
        //}
    }

    cSecondUOM.Hide();



}

function SaveSendUOM(modName){


    if(modName=="POE"){
        var UniqueArr=[];
        for (var i = 0; i < SecondUOM.length; i++) {
            var FilterSerial = $.grep(StockOfProduct, function (e) { return e.WarehouseID == SecondUOM[i].WarehouseID });

            if (FilterSerial.length == 0) {
                UniqueArr.push(SecondUOM[i].guid);
            }


        }
                    
        var  FinalSecondUOM = $.grep(SecondUOM, function (e) {
            return !UniqueArr.includes(e.guid);
        });

        if(FinalSecondUOM.length>0){
            $.ajax({
                type: "POST",
                url: "ProductsOpeningEntries.aspx/SaveSecondUOMDetails",
                data: "{'list':'" + JSON.stringify(FinalSecondUOM) + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async :false,
                success: function (msg) {

                },
                error:function(msg){

                }
            });

        }
    }
    else if(modName=="SC" || modName=="POS"){
        var FilterSerial = $.grep(SecondUOM, function (e) { return  e.Checked == 1 });

        if(FilterSerial.length>0){
            $.ajax({
                type: "POST",
                url: "SalesChallanAdd.aspx/SaveSecondUOMDetails",
                data: "{'list':'" + JSON.stringify(FilterSerial) + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async :false,
                success: function (msg) {

                },
                error:function(msg){

                }
            });

        }

    }

    else if(modName=="PC"){
        var FilterSerial = $.grep(SecondUOM, function (e) { return  1 == 1 });

        if(FilterSerial.length>0){
            $.ajax({
                type: "POST",
                url: "PurchaseChallan.aspx/SaveSecondUOMDetails",
                data: "{'list':'" + JSON.stringify(FilterSerial) + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async :false,
                success: function (msg) {

                },
                error:function(msg){

                }
            });

        }

    }
}

function GetSecondUONEditDetails(ProductID,Branch){


   

    $.ajax({
        type: "POST",
        url: "ProductsOpeningEntries.aspx/GetSecondUOMDetails",
        data: "{'ProductID':'" + ProductID + "','branchid':'" + Branch + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (msg) {
            SecondUOM=msg.d;
        }
    });
}

function GetSecondUONEditDetailsSC(ProductID,warehouseid,docid){


    var FilterSerial = $.grep(SecondUOM, function (e) { return e.WarehouseID == warehouseid && e.ProductId==ProductID });

    if(FilterSerial.length==0){
        $.ajax({
            type: "POST",
            url: "SalesChallanAdd.aspx/GetSecondUOMDetails",
            data: "{'ProductID':'" + ProductID + "','warehouseid':'" + warehouseid + "','docid':'"+docid+"'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (msg) {
                if(SecondUOM.length==0){
                    SecondUOM=msg.d;
                }
                else{
                    SecondUOM= SecondUOM.concat(msg.d);
                }
            }
        });
    }
}



function GetSecondUONEditDetailsPC(ProductID,warehouseid,docid){


    var FilterSerial = $.grep(SecondUOM, function (e) { return e.WarehouseID == warehouseid && e.ProductId==ProductID });

    if(FilterSerial.length==0){
        $.ajax({
            type: "POST",
            url: "PurchaseChallan.aspx/GetSecondUOMDetails",
            data: "{'ProductID':'" + ProductID + "','warehouseid':'" + warehouseid + "','docid':'"+docid+"'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (msg) {
                if(SecondUOM.length==0){
                    SecondUOM=msg.d;
                }
                else{
                    SecondUOM= SecondUOM.concat(msg.d);
                }
            }
        });
    }
}


function AlternateUOMDetails(ModuleName){
    EditFlag=null;
    ModuleNameSecondUOM=ModuleName;
    if(ModuleName=='POE'){

        


        if($('#ddlWarehouse').val()==""){
            jAlert('Please Select warehouse to proceed.','Alert');
            return;
        }

        if($('#ddlWarehouse').val()==null){
            jAlert('Please Select warehouse to proceed.','Alert');
            return;
        }


        //var FilterSerialMain = $.grep(aarr, function (e) { return e.slno == $('#ddlWarehouse').val() && e.productid==SecondUOMProductId });


        //if(FilterSerialMain.length==0)
        //{
        //    jAlert('Please enter Second Quantity details to proceed','Alert');
        //    return;
        //}



    }
    else if(ModuleName=='SC' || ModuleName=='POS'){
        if(cCmbWarehouse.GetValue()==""){
            jAlert('Please Select warehouse to proceed.','Alert');
            return;
        }

        if(cCmbWarehouse.GetValue()==null){
            jAlert('Please Select warehouse to proceed.','Alert');
            return;
        }

        var FilterSerialMain = $.grep(aarr, function (e) { return e.slno == cCmbWarehouse.GetValue() && e.productid==GetObjectID('hdfProductID').value });


        //if(FilterSerialMain.length==0)
        //{
        //    jAlert('Please enter Second Quantity details to proceed','Alert');
        //    return;
        //}

        //if(FilterSerialMain[0].packing){
        //    jAlert('Please enter Second Quantity details to proceed','Alert');
        //    return;
        //}


    }
    if(ModuleName=='POE'){
        var FilterSerial = $.grep(SecondUOM, function (e) { return e.WarehouseID == $('#ddlWarehouse').val() && e.ProductId==GetObjectID('hdfProductID').value });
    }
    else if(ModuleName=='SC' || ModuleName=='POS'){
        GetSecondUONEditDetailsSC(SecondUOMProductId,cCmbWarehouse.GetValue(),$("#hfDocId").val())
        var FilterSerial = $.grep(SecondUOM, function (e) { return e.WarehouseID == cCmbWarehouse.GetValue() && e.ProductId==SecondUOMProductId });

    }
    else if(ModuleName=='PC'){
        GetSecondUONEditDetailsPC(SecondUOMProductId,$("#ddlWarehouse").val(),$("#Keyval_Id").val())
        var FilterSerial = $.grep(SecondUOM, function (e) { return e.WarehouseID == $("#ddlWarehouse").val() && e.ProductId==SecondUOMProductId });
    }


    MakeTableFromArrayObject(FilterSerial);

    cSecondUOM.Show();
    if(countSecondUom == 1){
        $('#dataTbl').DataTable({
            "searching": false,
            "bInfo": false,
            "info": false,
            "ordering": false,
            "paging": false,
            "scrollY": "200px",        
        });
    }
    countSecondUom = countSecondUom + 1;
}

function SizeLostFocus(){
    var Lenght=ctxtLength.GetText();
    var width=ctxtWidth.GetText();
    var totalsize=parseFloat(Lenght) * parseFloat(width)/144;
    ctxtTotal.SetText(totalsize.toFixed(2));
}

function AddSecondUOMDetails(Module_Name)
{

    if(parseFloat(ctxtTotal.GetText())==0){
        jAlert('Please enter proper value in lenght/width to proceed.','Alert');
        return;
    }

    var Lenght=ctxtLength.GetText();
    var width=ctxtWidth.GetText();
    var ActualLength=Lenght + ' * ' +width;
    var ProductID= GetObjectID('hdfProductID').value;
    var UOM =  GetObjectID('hdfUOM').value;
    var Branch =  GetObjectID('hdfBranch').value;

    if(Module_Name!='PC')
        var BranchName =  ccmbbranch.GetText();
    else
        var BranchName =  $("#ddl_Branch").val();

    var WarehouseID = $('#ddlWarehouse').val();
    var WarehouseName = $("option:selected", '#ddlWarehouse').text();
    var total = ctxtTotal.GetText();
    
    if(EditFlag!="" && EditFlag!=null){
        for( var i = 0; i < SecondUOM.length; i++){ 
            if ( SecondUOM[i].guid == EditFlag) {                

                SecondUOM[i].Lenght        =Lenght;
                SecondUOM[i].width         =width;
                SecondUOM[i].ProductId     =GetObjectID('hdfProductID').value;
                SecondUOM[i].ActualLength  =ActualLength;
                SecondUOM[i].UOM           =UOM;
                SecondUOM[i].Branch        =Branch;
                SecondUOM[i].BranchName    =BranchName;
                SecondUOM[i].WarehouseID   =WarehouseID;
                SecondUOM[i].WarehouseName =WarehouseName;
                SecondUOM[i].total         =total;
                SecondUOM[i].Checked         =0;
                MakeTableFromArrayObject(SecondUOM);
                return;

            }
        }
        EditFlag=null;
    }
    
    
    
    var guid = uuid();






    var SencondUOMboj={};
    SencondUOMboj.guid=guid;
    SencondUOMboj.Lenght=Lenght;
    SencondUOMboj.width=width;
    SencondUOMboj.ProductId     =GetObjectID('hdfProductID').value;
    SencondUOMboj.ActualLength=ActualLength;3
    SencondUOMboj.UOM=UOM;
    SencondUOMboj.Branch=Branch;
    SencondUOMboj.BranchName=BranchName;
    SencondUOMboj.WarehouseID=WarehouseID;
    SencondUOMboj.WarehouseName=WarehouseName;
    SencondUOMboj.total=total;
    SencondUOMboj.Checked         =0;
    SecondUOM.push(SencondUOMboj);




    var FilterSerial = $.grep(SecondUOM, function (e) { return e.WarehouseID == $('#ddlWarehouse').val() && e.ProductId==GetObjectID('hdfProductID').value });

    MakeTableFromArrayObject(FilterSerial);

    
    //var str="<tr>"
    //str+="<td class='hide'>"+guid+"</td>";
    //str+="<td class='hide'>"+WarehouseID+"</td>";
    //str+="<td class='hide'>"+ProductID+"</td>";
    //str+="<td>"+BranchName+"</td>";
    //str+="<td>"+WarehouseName+"</td>";
    //str+="<td>"+ActualLength+' ' +UOM +"</td>";
    //str+="<td>"+total+' sq. ' +UOM +"</td>";

    //str+="<td><a href='#' class='padRight15' onclick='EditUOM("+JSON.stringify(guid)+")' ><img src='/assests/images/Edit.png' /></a>";
    //str+="<a href='#' onclick='DeleteUOM("+ JSON.stringify(guid)+")' ><img src='/assests/images/crs.png' /></a></td>";
    //str+="</tr>"

    //var prev=$("#tbodySecondUOM").html();
    //$("#tbodySecondUOM").html('');
    //$("#tbodySecondUOM").html(prev+str);

    

}

function MakeTableFromArrayObject(arr){
    if(ModuleNameSecondUOM=='POE' || ModuleNameSecondUOM=='PC')
    {
        var str="";
        for(var i=0;i<arr.length;i++){
            var sl=i+1;
            str+="<tr>"
            str+="<td class='hide'>"+arr[i].guid+"</td>";
            str+="<td class='hide'>"+arr[i].WarehouseID+"</td>";
            str+="<td class='hide'>"+arr[i].ProductID+"</td>";
            str+="<td>"+sl+"</td>";
            str+="<td>"+arr[i].BranchName+"</td>";
            str+="<td>"+arr[i].WarehouseName+"</td>";
            str+="<td>"+arr[i].ActualLength+' inch' +"</td>";
            str+="<td>"+arr[i].total+' sq. ' +arr[i].UOM +"</td>";

            str+="<td><a href='#' class='padRight15 link' onclick='EditUOM("+JSON.stringify(arr[i].guid)+")' ><img src='/assests/images/Edit.png' /></a>";
            str+="<a href='#' class='link' onclick='DeleteUOM("+ JSON.stringify(arr[i].guid)+")' ><img src='/assests/images/crs.png' /></a></td>";
            str+="</tr>"
        }
        $("#tbodySecondUOM").html(str);

    }
    else if (ModuleNameSecondUOM=='SC' || ModuleNameSecondUOM=='POS'){
        var str="";
        for(var i=0;i<arr.length;i++){
            var sl=i+1;
            str+="<tr>"
            str+="<td class='hide'>"+arr[i].guid+"</td>";
            str+="<td class='hide'>"+arr[i].WarehouseID+"</td>";
            str+="<td class='hide'>"+arr[i].ProductID+"</td>";
            
            if(arr[i].Checked=="1"){
                str+="<td><input type='checkbox' id='chk"+arr[i].guid+"' checked/></td>";
            }
            else{
                str+="<td><input type='checkbox' id='chk"+arr[i].guid+"' /></td>";

            }  
            
            str+="<td>"+sl+"</td>";
            str+="<td>"+arr[i].BranchName+"</td>";
            str+="<td>"+arr[i].WarehouseName+"</td>";
            str+="<td>"+arr[i].ActualLength+' ' +arr[i].UOM +"</td>";
            str+="<td>"+arr[i].total+' sq. ' +arr[i].UOM +"</td>";

            //str+="<td><a href='#' class='padRight15 link' onclick='EditUOM("+JSON.stringify(arr[i].guid)+")' ><img src='/assests/images/Edit.png' /></a>";
            //str+="<a href='#' class='link' onclick='DeleteUOM("+ JSON.stringify(arr[i].guid)+")' ><img src='/assests/images/crs.png' /></a></td>";
            str+="</tr>"
        }
        $("#tbodySecondUOM").html(str);
    }
}

function SaveSecondUOMDetails(){


    if(ModuleNameSecondUOM=='POE'){
        for(var i=0;i<SecondUOM.length;i++){
            //var FilterSerialMain = $.grep(aarr, function (e) { return e.slno == SecondUOM[i].WarehouseID && e.productid==SecondUOM[i].ProductId });
            //var FilterSerial = $.grep(SecondUOM, function (e) { return e.WarehouseID == SecondUOM[i].WarehouseID && e.ProductId==SecondUOM[i].ProductId && e.Checked==1});

            //if(FilterSerialMain[0].packing!=FilterSerial.length){
            //    jAlert('Qunatity mismatched found . Can not proceed','Alert');
            //    return;
            //}
        }
    }
    else if(ModuleNameSecondUOM=='SC' || ModuleNameSecondUOM=='POS' ){
        var FilterSerial = $.grep(SecondUOM, function (e) { return e.WarehouseID == cCmbWarehouse.GetValue() && e.ProductId==SecondUOMProductId });
        for(var i=0;i<FilterSerial.length;i++){
            if($("#chk"+FilterSerial[i].guid).prop('checked')==true){

                for(var j=0;j<SecondUOM.length;j++){
                    if(SecondUOM[j].guid==FilterSerial[i].guid){
                        SecondUOM[j].Checked=1;
                    }
                }
            }
            else{
                for(var j=0;j<SecondUOM.length;j++){
                    if(SecondUOM[j].guid==FilterSerial[i].guid){
                        SecondUOM[j].Checked=0;
                    }
                }
            }
        }
    
    }

        



    


    








    cSecondUOM.Hide();
}


function DeleteUOM(uid){


    for( var i = 0; i < SecondUOM.length; i++){ 
        if ( SecondUOM[i].guid == uid) {
            SecondUOM.splice(i, 1); 
        }
    }

    var FilterSerial = $.grep(SecondUOM, function (e) { return e.WarehouseID == $('#ddlWarehouse').val() && e.ProductId==GetObjectID('hdfProductID').value });

    MakeTableFromArrayObject(FilterSerial);
}
function EditUOM(uid){

    var EditUOMobj = $.grep(SecondUOM, function (e) { return e.guid == uid });
    EditFlag=uid;


}

function uuid() {
    function randomDigit() {
        if (crypto && crypto.getRandomValues) {
            var rands = new Uint8Array(1);
            crypto.getRandomValues(rands);
            return (rands[0] % 16).toString(16);
        } else {
            return ((Math.random() * 16) | 0).toString(16);
        }
    }
    var crypto = window.crypto || window.msCrypto;
    return 'xxxxxxxx-xxxx-4xxx-8xxx-xxxxxxxxxxxx'.replace(/x/g, randomDigit);
}


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

function SaveStockPC() {
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
    var AltQty=ctxtAltQty.GetValue();
    var AltUOM=ccmbAltUOM.GetValue();
    ctxtAltQty.SetValue(0);


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
                                saveStockDataPC(StockType,ProductSrlNo,ProductID,UOM,WarehouseID,WarehouseName,Batch,Qty,MfgDate,ExprieyDate,Serial,Rate,AltQty,AltUOM);
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
        saveStockDataPC(StockType,ProductSrlNo,ProductID,UOM,WarehouseID,WarehouseName,Batch,Qty,MfgDate,ExprieyDate,Serial,Rate,AltQty,AltUOM);
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
   
    SetUOMConversionArray(WarehouseID);

    //Surojit 19-03-2019 for UOM Conversion
}

//Common file so method declare here


function SetUOMConversionArray(WarehouseID) {
    if(AltQtyModule == "Opening Balances"){
        var Quantity = $('#hdnUOMQuantity').val();
        var packing = $('#hdnUOMpacking').val();
        var PackingUom = $('#hdnUOMPackingUom').val();
        var PackingSelectUom = $('#hdnUOMPackingSelectUom').val();
        var slnoget = WarehouseID;

        if (StockOfProduct.length > 0) {
            var extobj = {};
            var PackingUom = $('#hdnUOMPackingUom').val();
            var PackingSelectUom = $('#hdnUOMPackingSelectUom').val();

            var productidget = $('#hdfProductID').val();


            for (i = 0; i < aarrUOM.length; i++) {
                extobj = aarr[i];
                console.log(extobj);
                if (extobj.slno == slnoget && extobj.productid == productidget) {
                    //aarr.pop(extobj);
                    aarrUOM.splice(i, 1);
                }
                extobj = {};
            }


            var arrobj = {};
            arrobj.productid = productidget;
            arrobj.slno = slnoget;
            arrobj.Quantity = Quantity;
            arrobj.packing = packing;
            arrobj.PackingUom = PackingUom;
            arrobj.PackingSelectUom = PackingSelectUom;

            aarrUOM.push(arrobj);
        }
    }
}
//Common file so method declare here

function saveStockDataPC(StockType,ProductSrlNo,ProductID,UOM,WarehouseID,WarehouseName,Batch,Qty,MfgDate,ExprieyDate,Serial,Rate,AltQty,AltUOM){
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
            IsOutStatus: "1", IsOutStatusMsg: "", LoopID: _LoopID, Status: "D",AltQty:AltQty,AltUOM:AltUOM
        }
        StockOfProduct.push(ProductStock);
    }
    else {
        // Save Data In Json
        var _SrlNo = parseInt(getMax(StockOfProduct, "SrlNo")) + 1;
        var _LoopID = parseInt(getMax(filteredJson, "LoopID"));
        var _Quantity = parseFloat(getMax(filteredJson, "Quantity")) + 1;

        var _AltQty = parseFloat(getMax(filteredJson, "AltQty")) + 1;

        if (StockType == "WS" ||StockType == "WSC" || StockType == "WBS" || StockType == "BS" || StockType == "S") {
            $.grep(filteredJson, function (e) { e.Quantity = _Quantity; });
            $.grep(filteredJson, function (e) { e.Rate = Rate; });

            var ProductStock = { Product_SrlNo: ProductSrlNo, SrlNo: _SrlNo, WarehouseID: WarehouseID, WarehouseName: "",
                Quantity: _Quantity, SalesQuantity: "", Batch: Batch, MfgDate: MfgDate, ExpiryDate: ExprieyDate,Rate:Rate,
                SerialNo: Serial, Barcode: "", ViewBatch: "", ViewMfgDate: "", ViewExpiryDate: "",ViewRate:"",
                IsOutStatus: "1", IsOutStatusMsg: "", LoopID: _LoopID, Status: "D",AltQty:_AltQty,AltUOM:AltUOM
            }
            StockOfProduct.push(ProductStock);

            $.grep(filteredJson, function (e) { if (e.SalesQuantity != "") e.SalesQuantity = _Quantity + " " + UOM; });
            $.grep(filteredJson, function (e) { if (e.SalesQuantity != "") e.ViewRate = Rate; });
        }
        else  if (StockType == "WC") {
            _Quantity = (parseFloat(getMax(filteredJson, "Quantity")) + parseFloat(Qty));
            _AltQty = (parseFloat(getMax(filteredJson, "AltQty")) + parseFloat(Qty));

            if(filteredJson.length==1){
                $.grep(filteredJson, function (e) { e.Rate = Rate; });
                $.grep(filteredJson, function (e) { e.ViewRate = Rate; });
                $.grep(filteredJson, function (e) { e.Quantity = _Quantity });
                $.grep(filteredJson, function (e) { e.AltQty = _AltQty });
                $.grep(filteredJson, function (e) { e.SalesQuantity = _Quantity + " " + UOM; ; });
            }
            else{
                $.grep(filteredJson, function (e) { if (e.SalesQuantity != "") e.SalesQuantity = _Quantity + " " + UOM; });
                $.grep(filteredJson, function (e) { if (e.SalesQuantity != "") e.ViewRate = Rate; });

                $.grep(filteredJson, function (e) { e.Rate = Rate; });
                $.grep(filteredJson, function (e) { e.Quantity = _Quantity });
                $.grep(filteredJson, function (e) { e.AltQty = _AltQty });

            }
        }
        else {
            _Quantity = (parseFloat(getMax(filteredJson, "Quantity")) + parseFloat(Qty));
            _AltQty = (parseFloat(getMax(filteredJson, "AltQty")) + parseFloat(AltQty));


            $.grep(filteredJson, function (e) { e.Rate = Rate; });
            $.grep(filteredJson, function (e) { e.ViewRate = Rate; });
            $.grep(filteredJson, function (e) { e.Quantity = _Quantity });
            $.grep(filteredJson, function (e) { e.AltQty = _AltQty });
            $.grep(filteredJson, function (e) { e.SalesQuantity = _Quantity + " " + UOM; ; });
        }
    }

    StockDeatils();
    SetFocus("Save");

    //Surojit 19-03-2019 for UOM Conversion
   
    SetUOMConversionArray(WarehouseID);

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
            setTimeout(function () 
            { 
                $("#ddlWarehouse").focus(); // Surojit  25-07-2019   // 0020188 
                //ctxtQty.Focus();  // Surojit  25-07-2019   // 0020188
            }, 500);
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
        var whId="";
        var criteria = [
            { Field: "SrlNo", Values: ID }
        ];
        var filteredJson = flexFilter(StockOfProduct, criteria);
        if(filteredJson.length>0)
            whId=filteredJson[0].WarehouseID;
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
        
       
        //if(ModuleName=='POE'){        
        //    whId=$('#ddlWarehouse').val();

        //}
        //else if(ModuleName=='SC' || ModuleName=='POS'){
        //    whId=cCmbWarehouse.GetValue();
        //}
        if(whId!=""){
            if(SecondUOM.length>0){
                var FilterSerial = $.grep(SecondUOM, function (e) { return e.WarehouseID != whId });
                SecondUOM=FilterSerial;
            }
        }
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
        else if(col[i]=="ViewBatch") th.innerHTML ="Batch/Lot Number";
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
