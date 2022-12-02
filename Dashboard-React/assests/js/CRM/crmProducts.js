var StockOfProductCRM = [];
var warehouserateListCRM = [];
var crmProductModuleName = "";
var countActivityProductCRM = 1;

var EditFlagCRM = 0;
var ModuleNameActivityProductCRM = "";

//function ValueSelected(e, indexName) {

//    if (e.code == "Enter" || e.code == "NumpadEnter") {
//        if (indexName == "ContactIndex") {
//            var Id = e.target.parentElement.parentElement.cells[0].innerText;
//            var name = e.target.parentElement.parentElement.cells[1].children[0].value;
//            if (Id) {
//                SetContact(Id, name);
//            }
//        }
        
//    }
//    else if (e.code == "ArrowDown") {
//        thisindex = parseFloat(e.target.getAttribute(indexName));
//        thisindex++;
//        if (thisindex < 10)
//            $("input[" + indexName + "=" + thisindex + "]").focus();
//    }
//    else if (e.code == "ArrowUp") {
//        thisindex = parseFloat(e.target.getAttribute(indexName));
//        thisindex--;
//        if (thisindex > -1)
//            $("input[" + indexName + "=" + thisindex + "]").focus();
//        else {
//            $('#txtContactSearch').focus();
//        }
//    }

//}



$(document).ready(function () {
    $('#ProductModelCRM').on('shown.bs.modal', function () {
        $('#txtProductSearchCRM').focus();
    })
})
function ProductButnClickCRM(s, e) {
    $('#ProductModelCRM').modal('show');
}

function Product_KeyDownCRM(s, e) {
    if (e.htmlEvent.key == "Enter") {
        $('#ProductModelCRM').modal('show');
    }
}


function ProductkeydownCRM(e) {
    var OtherDetails = {}

    if ($.trim($("#txtProductSearchCRM").val()) == "" || $.trim($("#txtProductSearchCRM").val()) == null) {
        return false;
    }
    OtherDetails.SearchKey = $("#txtProductSearchCRM").val();

    if (e.code == "Enter" || e.code == "NumpadEnter") {
        var HeaderCaption = [];
        HeaderCaption.push("Product Name");
        HeaderCaption.push("Product Description");
        HeaderCaption.push("HSN/SAC");

        if ($("#txtProductSearchCRM").val() != "") {
            callonServer("../CRMActivity/GetcrmProducts", OtherDetails, "ProductTableCRM", HeaderCaption, "ProductIndexCRM", "SetProductCRM");
        }
    }
    else if (e.code == "ArrowDown") {
        if ($("input[ProductIndexCRM=0]"))
            $("input[ProductIndexCRM=0]").focus();
    }
}

function SetProductCRM(Id, Name) {
    debugger;
    var key = Id;
    if (key != null && key != '') {
        $('#ProductModelCRM').modal('hide');
        ctxtProductCRM.SetText(Name);
        GetObjectID('hdfProductIDCRM').value = key;
        ctxtProductCRM.Focus();
    }
    else {
        ctxtProductCRM.SetText('');
        GetObjectID('hdfProductIDCRM').value = '';
    }
}



function SaveActivityProductDetailsCRM(modName) {
    //var Quantity = ctxtQuantityCRM.GetText();
    //var Rate = ctxtRateCRM.GetText();
    //var ProductID = GetObjectID('hdfProductIDCRM').value;
    //var ProductName = ctxtProductCRM.GetText();
    //var Remarks = ctxtRemarksCRM.GetText();

    //if (ProductName == '' && Quantity == 0 && Rate == 0) {
    //    jAlert('Select Product,Quantity & Rate');
    //}
    //else
    //    {
    $("#cActivityProductCRM").modal('hide')
    SaveSendActivityProductCRM(modName);
    // }
}

function SaveCRMProductDetailsCRM(modName) {
    var Module_id = $("#hdnCrmProductIdentityId").val();
    $("#cActivityProductCRM").modal('hide')
    SaveCRMProductCRM(crmProductModuleName, Module_id);

}


function crmProdCancelClickCRM() {
    $("#cActivityProductCRM").modal('hide');
}



function SaveSendActivityProductCRM(modName) {

    $.ajax({
        type: "POST",
        url: "../CRMActivity/SaveActivityProductDetails",
        data: "{'list':'" + JSON.stringify(ActivityProductCRM) + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (msg) {
        },
        error: function (msg) {
        }
    });

}

function SaveCRMProductCRM(modName, Module_id) {
    if (ActivityProductCRM.length > 0) {
        $.ajax({
            type: "POST",
            url: "../CRMProducts/SaveCRMProductDetails",
            data: "{'list':'" + JSON.stringify(ActivityProductCRM) + "','Module_Name':'" + modName + "','Module_id':'" + Module_id + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (msg) {
                jAlert(msg, 'Alert');
            },
            error: function (msg) {
                jAlert(msg, 'Alert');

            }
        });
    }
    else {
        jAlert('Please select atleast one product to proceed.', 'Alert')
    }

}





function ShowProducts(ModuleName) {

    GetObjectID('hdfProductIDCRM').value = 0;
    ctxtQuantityCRM.SetValue(0);
    ctxtRateCRM.SetValue(0);
    ctxtRemarksCRM.SetText('');

    EditFlagCRM = null;
    ModuleNameActivityProductCRM = ModuleName;
    if (ModuleName == 'ACPRD') {

        var FilterSerialMain = "";
        var FilterSerial = "";
    }


    $("#cActivityProductCRM").modal('show')

    countActivityProductCRM = countActivityProductCRM + 1;
}



function AddActivityProductDetailsCRM() {


    var Quantity = ctxtQuantityCRM.GetText();
    var Rate = ctxtRateCRM.GetText();
    var ProductID = GetObjectID('hdfProductIDCRM').value;
    var ProductName = ctxtProductCRM.GetText();
    var Remarks = ctxtRemarksCRM.GetText();

    if (ProductName == '' && Quantity == 0 && Rate == 0) {
        jAlert('Select Product,Quantity & Rate');
    }
    else {
        if (EditFlagCRM != "" && EditFlagCRM != null) {
            for (var i = 0; i < ActivityProductCRM.length; i++) {
                if (ActivityProductCRM[i].guid == EditFlagCRM) {

                    ActivityProductCRM[i].ProductId = ProductID;
                    ActivityProductCRM[i].Quantity = Quantity;
                    ActivityProductCRM[i].Rate = Rate;
                    ActivityProductCRM[i].Remarks = Remarks;
                    ActivityProductCRM[i].ProductName = ProductName;

                    MakeTableFromArrayObjectCRM(ActivityProductCRM);
                    return;

                }
            }
            EditFlagCRM = null;
        }

        var guid = uuidCRM();

        var ActivityProductboj = {};
        ActivityProductboj.guid = guid;
        ActivityProductboj.Quantity = Quantity;
        ActivityProductboj.Rate = Rate;
        ActivityProductboj.ProductId = ProductID;
        ActivityProductboj.Remarks = Remarks;
        ActivityProductboj.ProductName = ProductName;

        ActivityProductCRM.push(ActivityProductboj);


        MakeTableFromArrayObjectCRM(ActivityProductCRM);
    }
}

function MakeTableFromArrayObjectCRM(arr) {
    if (ModuleNameActivityProductCRM == 'ACPRD') {
        var str = "";
        for (var i = 0; i < arr.length; i++) {
            var sl = i + 1;
            str += "<tr>"
            str += "<td class='hide'>" + arr[i].guid + "</td>";
            str += "<td class='hide'>" + arr[i].ProductId + "</td>";
            str += "<td>" + sl + "</td>";
            str += "<td>" + arr[i].ProductName + "</td>";
            str += "<td>" + arr[i].Quantity + "</td>";
            str += "<td>" + arr[i].Rate + "</td>";
            str += "<td>" + arr[i].Remarks + "</td>";

            str += "<td><a href='#' class='iconCD link' onclick='EditActivityProdCRM(" + JSON.stringify(arr[i].guid) + ")' ><img src='/assests/images/Edit.png' /></a>";
            str += "<a href='#' class='link' onclick='DeleteActivityProductCRM(" + JSON.stringify(arr[i].guid) + ")' ><img src='/assests/images/crs.png' /></a></td>";
            str += "</tr>"
        }
        $("#tbodyActivityProductCRM").html(str);

    }
}


function DeleteActivityProductCRM(uid) {


    for (var i = 0; i < ActivityProductCRM.length; i++) {
        if (ActivityProductCRM[i].guid == uid) {
            ActivityProductCRM.splice(i, 1);
        }
    }

    //var FilterSerial = $.grep(ActivityProductCRM, function (e) { return e.WarehouseID == $('#ddlWarehouse').val() && e.ProductId == GetObjectID('hdfProductIDCRM').value });

    MakeTableFromArrayObjectCRM(ActivityProductCRM);
}
function EditActivityProdCRM(uid) {
    var EditActivityProdobj = $.grep(ActivityProductCRM, function (e) { return e.guid == uid });
    EditFlagCRM = uid;

    SetActivityProductOnEditCRM(EditActivityProdobj);
}


function ShowCRMProductsEditCRM(module_name, module_id) {
    crmProductModuleName = module_name;
    $.ajax({
        type: "POST",
        //url: "Url.Action("GetEmployeesTargetByCode", "EmployeesTarget")",
        url: "../CRMProducts/GetCRMProductsDetails",
        data: { Module_Name: "Campaign Products", Module_id: module_id },
        success: function (response) {

            ActivityProductCRM = response;
            MakeTableFromArrayObjectCRM(ActivityProductCRM);

        },
        error: function (response) {

        }
    });
}

function SetActivityProductOnEditCRM(EditActivityProdobj) {
    ctxtProductCRM.SetText(EditActivityProdobj[0].ProductName);
    GetObjectID('hdfProductIDCRM').value = EditActivityProdobj[0].ProductId;
    ctxtQuantityCRM.SetValue(EditActivityProdobj[0].Quantity);
    ctxtRateCRM.SetValue(EditActivityProdobj[0].Rate);
    ctxtRemarksCRM.SetText(EditActivityProdobj[0].Remarks);
}

function uuidCRM() {
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

function flexFilterCRM(arr, info) {
    var matchesFilter, matches = [];

    matchesFilter = function (item) {
        var count = 0;
        for (var n = 0; n < info.length; n++) {
            if (info[n]["Values"] == item[info[n]["Field"]]) {
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

function GetDateFormatCRM(today) {
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



