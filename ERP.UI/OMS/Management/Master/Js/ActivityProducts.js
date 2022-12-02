var StockOfProduct = [];
var warehouserateList=[];

var countActivityProduct = 1;

var EditFlag=0;
var ModuleNameActivityProduct = "";


function SaveActivityProductDetails(modName) {
    var Quantity = ctxtQuantity.GetText();
    var Rate = ctxtRate.GetText();
    var ProductID = GetObjectID('hdfProductID').value;
    var ProductName = ctxtProduct.GetText();
    var Remarks = ctxtRemarks.GetText();

    if (ProductName == '' && Quantity == 0 && Rate == 0) {
        jAlert('Select Product,Quantity & Rate');
    }
    else
        {
        cActivityProduct.Hide();
        SaveSendActivityProduct(modName);
        }
}

function SaveSendActivityProduct(modName) {
    if (modName == 'Activity')
        {
            $.ajax({
                type: "POST",
                url: "Activities.aspx/SaveActivityProductDetails",
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
    else if (modName == 'Lead')
    {
        $.ajax({
            type: "POST",
            url: "frmContactMain.aspx/SaveLeadActivityProductDetails",
            data: "{'list':'" + JSON.stringify(ActivityProduct) + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (msg) {
            },
            error: function (msg) {
            }
        });
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
    cActivityProduct.Show();
   
    countActivityProduct = countActivityProduct + 1;
}



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

