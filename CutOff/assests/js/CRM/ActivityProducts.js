var StockOfProduct = [];
var warehouserateList=[];

var countActivityProduct = 1;

var EditFlag=0;
var ModuleNameActivityProduct = "";

function ValueSelected(e, indexName) {

    if (e.code == "Enter" || e.code == "NumpadEnter") {
        if (indexName == "ContactIndex") {
            var Id = e.target.parentElement.parentElement.cells[0].innerText;
            var name = e.target.parentElement.parentElement.cells[1].children[0].value;
            if (Id) {
                SetContact(Id, name);
            }
        }
        else if (indexName == "ProductIndex") {
            var Id = e.target.parentElement.parentElement.cells[0].innerText;
            var name = e.target.parentElement.parentElement.cells[1].children[0].value;
            if (Id) {
                SetProduct(Id, name);
            }
        }
        else if (indexName == "ProductIndexCRM") {
            var Id = e.target.parentElement.parentElement.cells[0].innerText;
            var name = e.target.parentElement.parentElement.cells[1].children[0].value;
            if (Id) {
                SetProductCRM(Id, name);
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
            $('#txtContactSearch').focus();
        }
    }

}


$(document).ready(function () {
    $('#ProductModel').on('shown.bs.modal', function () {
        $('#txtProductSearch').focus();
    })
})
function ProductButnClick(s, e) {
    $('#ProductModel').modal('show');
}

function Product_KeyDown(s, e) {
    if (e.htmlEvent.key == "Enter") {
        $('#ProductModel').modal('show');
    }
}


function Productkeydown(e) {
    var OtherDetails = {}

    if ($.trim($("#txtProductSearch").val()) == "" || $.trim($("#txtProductSearch").val()) == null) {
        return false;
    }
    OtherDetails.SearchKey = $("#txtProductSearch").val();

    if (e.code == "Enter" || e.code == "NumpadEnter") {
        var HeaderCaption = [];
        HeaderCaption.push("Product Name");
        HeaderCaption.push("Product Description");
        HeaderCaption.push("HSN/SAC");

        if ($("#txtProductSearch").val() != "") {
            callonServer("../CRMActivity/GetcrmProducts", OtherDetails, "ProductTable", HeaderCaption, "ProductIndex", "SetProduct");
        }
    }
    else if (e.code == "ArrowDown") {
        if ($("input[ProductIndex=0]"))
            $("input[ProductIndex=0]").focus();
    }
}

function SetProduct(Id, Name) {
    debugger;
    var key = Id;
    if (key != null && key != '') {
        $('#ProductModel').modal('hide');
        ctxtProduct.SetText(Name);
        GetObjectID('hdfProductID').value = key;
        ctxtProduct.Focus();
    }
    else {
        ctxtProduct.SetText('');
        GetObjectID('hdfProductID').value = '';
    }
}



function SaveActivityProductDetails(modName) {
    //var Quantity = ctxtQuantity.GetText();
    //var Rate = ctxtRate.GetText();
    //var ProductID = GetObjectID('hdfProductID').value;
    //var ProductName = ctxtProduct.GetText();
    //var Remarks = ctxtRemarks.GetText();

    //if (ProductName == '' && Quantity == 0 && Rate == 0) {
    //    jAlert('Select Product,Quantity & Rate');
    //}
    //else
    //    {
        $("#cActivityProduct").modal('hide')
        SaveSendActivityProduct(modName);
       // }
}

function SaveCRMProductDetails(modName) {
    var Module_id = $("#hdnCrmProductIdentityId").val();
    $("#cActivityProduct").modal('hide')
    SaveCRMProduct(modName, Module_id);

}


function crmProdCancelClick() {
    $("#cActivityProduct").modal('hide');
}

function SaveSendActivityProduct(modName) {

            $.ajax({
                type: "POST",
                url: "../CRMActivity/SaveActivityProductDetails",
                data: "{'list':'" + JSON.stringify(ActivityProduct) + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async :false,
                success: function (msg) {
                },
                error:function(msg){
                }
            });

}

function SaveCRMProduct(modName, Module_id) {
    if (ActivityProduct.length > 0) {
        $.ajax({
            type: "POST",
            url: "../CRMProducts/SaveCRMProductDetails",
            data: "{'list':'" + JSON.stringify(ActivityProduct) + "','Module_Name':'" + modName + "','Module_id':'" + Module_id + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (msg) {
            },
            error: function (msg) {
            }
        });
    }
    else {
        jAlert('Please select atleast one product to proceed.','Alert')
    }

}




function Products(ModuleName) {
    ctxtProduct.SetText('');
    GetObjectID('hdfProductID').value = 0;
    ctxtQuantity.SetValue(0);
    ctxtRate.SetValue(0);
    ctxtRemarks.SetText('');

    EditFlag=null;
    ModuleNameActivityProduct = ModuleName;
    if (ModuleName == 'ACPRD') {

        var FilterSerialMain ="";
        var FilterSerial ="";
    }

    MakeTableFromArrayObject(ActivityProduct);
    $("#cActivityProduct").modal('show')
   
    countActivityProduct = countActivityProduct + 1;
}


//function ShowProducts(ModuleName) {
    
//    GetObjectID('hdfProductID').value = 0;
//    ctxtQuantity.SetValue(0);
//    ctxtRate.SetValue(0);
//    ctxtRemarks.SetText('');

//    EditFlag = null;
//    ModuleNameActivityProduct = ModuleName;
//    if (ModuleName == 'ACPRD') {

//        var FilterSerialMain = "";
//        var FilterSerial = "";
//    }

   
//    $("#cActivityProduct").modal('show')

//    countActivityProduct = countActivityProduct + 1;
//}



function AddActivityProductDetails()
{


    var Quantity=ctxtQuantity.GetText();
    var Rate = ctxtRate.GetText();
    var ProductID = GetObjectID('hdfProductID').value;
    var ProductName = ctxtProduct.GetText();
    var Remarks = ctxtRemarks.GetText();

    if (ProductName == '' && Quantity == 0 && Rate==0)
    {
        jAlert('Select Product,Quantity & Rate');
    }
    else
    {
        if(EditFlag!="" && EditFlag!=null){
            for (var i = 0; i < ActivityProduct.length; i++) {
                if (ActivityProduct[i].guid == EditFlag) {

                    ActivityProduct[i].ProductId = ProductID;
                    ActivityProduct[i].Quantity = Quantity;
                    ActivityProduct[i].Rate = Rate;
                    ActivityProduct[i].Remarks = Remarks;
                    ActivityProduct[i].ProductName = ProductName;

                    MakeTableFromArrayObject(ActivityProduct);
                    return;

                }
            }
            EditFlag=null;
        }
    
        var guid = uuid();

        var ActivityProductboj = {};
        ActivityProductboj.guid = guid;
        ActivityProductboj.Quantity = Quantity;
        ActivityProductboj.Rate = Rate;
        ActivityProductboj.ProductId = ProductID;
        ActivityProductboj.Remarks = Remarks;
        ActivityProductboj.ProductName = ProductName;

        ActivityProduct.push(ActivityProductboj);


        MakeTableFromArrayObject(ActivityProduct);
    }
}

function MakeTableFromArrayObject(arr){
    if (ModuleNameActivityProduct == 'ACPRD')
    {
        var str="";
        for(var i=0;i<arr.length;i++){
            var sl=i+1;
            str+="<tr>"
            str+="<td class='hide'>"+arr[i].guid+"</td>";
            str += "<td class='hide'>" + arr[i].ProductId + "</td>";
            str+="<td>"+sl+"</td>";
            str += "<td>" + arr[i].ProductName + "</td>";
            str += "<td>" + arr[i].Quantity + "</td>";
            str += "<td>" + arr[i].Rate + "</td>";
            str += "<td>" + arr[i].Remarks + "</td>";

            str += "<td><a href='#' class='iconCD link' onclick='EditActivityProd(" + JSON.stringify(arr[i].guid) + ")' ><img src='/assests/images/Edit.png' /></a>";
            str += "<a href='#' class='link' onclick='DeleteActivityProduct(" + JSON.stringify(arr[i].guid) + ")' ><img src='/assests/images/crs.png' /></a></td>";
            str+="</tr>"
        }
        $("#tbodyActivityProduct").html(str);

    }
}


function DeleteActivityProduct(uid) {


    for (var i = 0; i < ActivityProduct.length; i++) {
        if (ActivityProduct[i].guid == uid) {
            ActivityProduct.splice(i, 1);
        }
    }

    //var FilterSerial = $.grep(ActivityProduct, function (e) { return e.WarehouseID == $('#ddlWarehouse').val() && e.ProductId == GetObjectID('hdfProductID').value });

    MakeTableFromArrayObject(ActivityProduct);
}
function EditActivityProd(uid) {
    var EditActivityProdobj = $.grep(ActivityProduct, function (e) { return e.guid == uid });
    EditFlag=uid;

    SetActivityProductOnEdit(EditActivityProdobj);
}


//function ShowCRMProductsEdit(module_name, module_id) {
//    $.ajax({
//        type: "POST",
//        //url: "Url.Action("GetEmployeesTargetByCode", "EmployeesTarget")",
//        url: "../CRMProducts/GetCRMProductsDetails",
//        data: { Module_Name: "Campaign Products", Module_id: module_id },
//    success: function (response) {

//        ActivityProduct = response;
//        MakeTableFromArrayObject(ActivityProduct);

//    },
//    error: function (response) {

//    }
//});
//}

function SetActivityProductOnEdit(EditActivityProdobj) {
    ctxtProduct.SetText(EditActivityProdobj[0].ProductName);
    GetObjectID('hdfProductID').value = EditActivityProdobj[0].ProductId;
    ctxtQuantity.SetValue(EditActivityProdobj[0].Quantity);
    ctxtRate.SetValue(EditActivityProdobj[0].Rate);
    ctxtRemarks.SetText(EditActivityProdobj[0].Remarks);
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

function flexFilter(arr, info) {
    var matchesFilter, matches = [];

    matchesFilter = function (item) {
        var count = 0;
        for (var n = 0; n < info.length; n++) {
            if (info[n]["Values"]==item[info[n]["Field"]]){
                count++;
            }
        }

        return count == info.length;
    }

    for (var i = 0; i < arr.length; i++) {
        if (matchesFilter(arr[i])) {
            matches.push(arr[i]);
        }
    }
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


function Serialkeydown(e){
    if (e.code == "Enter" || e.code == "NumpadEnter") {
        SaveStock();
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


function validationsActivity() {
    var ismandatory = false;

    if (cActivity_Date.date != null) {
        ismandatory = true;
    }
    else {
        ismandatory = false;
        jAlert('Select Activity Date');
        return;
    }

    if (cddlActivity.GetValue() != "0") {
        ismandatory = true;
    }
    else {
        ismandatory = false;
        jAlert('Select Activity');
        return;
    }

    if (cddlActivityType.GetValue() != "0") {
        ismandatory = true;
    }
    else {
        ismandatory = false;
        jAlert('Select Type');
        return;
    }

    if (ctxt_Subject.GetText() != "") {
        ismandatory = true;
    }
    else {
        ismandatory = false;
        jAlert('Select Subject');
        return;
    }

    if (cmemo_Details.GetText() != "") {
        ismandatory = true;
    }
    else {
        ismandatory = false;
        jAlert('Select Details');
        return;
    }

    if ($('#ddlAssignTo').val() != "0") {
        ismandatory = true;
    }
    else {
        ismandatory = false;
        jAlert('Select Assign To');
        return;
    }

    if ($('#ddlDuration').val() != "0") {
        ismandatory = true;
    }
    else {
        ismandatory = false;
        jAlert('Select Duration');
        return;
    }

    if ($('#ddlPriority').val() != "0") {
        ismandatory = true;
    }
    else {
        ismandatory = false;
        jAlert('Select Priority');
        return;
    }

    if (cDue_dt.date != null) {
        ismandatory = true;
    }
    else {
        ismandatory = false;
        jAlert('Select Due Date');
        return;
    }

    return ismandatory;
}



