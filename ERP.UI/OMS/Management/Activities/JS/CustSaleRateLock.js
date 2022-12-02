
        /***Short cut key handling***/
        document.onkeydown = function (e) {
            if (event.keyCode == 83 && event.altKey == true) { //run code for Ctrl+S -- ie, Save & New  
                StopDefaultAction(e);
                document.getElementById('btnSaveRecords').click();
                return false;
            }
            else if (event.keyCode == 65 && event.altKey == true) {
                StopDefaultAction(e);
                OnAddButtonClick();
            }
            else if (event.keyCode == 67 && event.altKey == true) {
                StopDefaultAction(e);
                cancel()
            }
        }
function StopDefaultAction(e) {
    if (e.preventDefault) { e.preventDefault() }
    else { e.stop() };

    e.returnValue = false;
    e.stopPropagation();
}


    //***Customer***//
    function CustomerButnClick(s, e) {
        $("#CustomerTable").empty();
        var html = "<table border='1' width='100%' class='dynamicPopupTbl'><tr class='HeaderStyle'><th style='display:none'>id</th><th>Customer Name</th><th>Unique Id</th><th>Address</th></tr></table>";
        $("#CustomerTable").html(html);
        setTimeout(function () { $("#txtCustSearch").focus(); }, 500);
        $('#txtCustSearch').val('');
        //shouldCheck = 1;
        //$('#mainActMsg').hide();
        $('#CustModel').modal('show');

    }
function CustomerKeyDown(s, e) {
    if (e.htmlEvent.key == "Enter" || e.htmlEvent.key == "NumpadEnter") {
        //shouldCheck = 0;
        s.OnButtonClick(0);
    }
}
function Customerkeydown(e) {
    var OtherDetails = {}
    if ($.trim($("#txtCustSearch").val()) == "" || $.trim($("#txtCustSearch").val()) == null) {
        return false;
    }

    OtherDetails.SearchKey = $("#txtCustSearch").val();

    if (e.code == "Enter" || e.code == "NumpadEnter") {
        var HeaderCaption = [];
        HeaderCaption.push("Customer Name");
        HeaderCaption.push("Unique Id");
        HeaderCaption.push("Address");
        if ($("#txtCustSearch").val() != '') {
            callonServer("/OMS/Management/Activities/CustSaleRateLock.aspx/GetCustomer", OtherDetails, "CustomerTable", HeaderCaption, "customerIndex", "SetCustomer");
        }
    }
    else if (e.code == "ArrowDown") {
        if ($("input[customerindex=0]"))
            $("input[customerindex=0]").focus();
    }

}
function SetCustomer(Id, Name) {
    if (Id) {
        $('#CustModel').modal('hide');
        SetCustNametxt(Id, Name)
    }
}
function ValueSelected(e, indexName) {
    if (e.code == "Enter" || e.code == "NumpadEnter") {
        var Id = e.target.parentElement.parentElement.cells[0].innerText;
        var name = e.target.parentElement.parentElement.cells[1].children[0].value;
        if (Id) {
            if (indexName == "customerIndex") {
                $('#CustModel').modal('hide');
                SetCustomer(Id, name);
            }
            else if (indexName == "ProdIndex") {
                $('#ProductModel').modal('hide');
                SetProduct(Id, name);
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
            if (indexName == "customerIndex") {
                $('#txtCustSearch').focus();
            }
            else if (indexName == "ProdIndex") {
                $('#txtProdSearch').focus();
            }
        }
    }
    else if (e.code == "Escape") {
        if (indexName == "customerIndex") {
            $('#CustModel').modal('hide');
            ctxtCustName.Focus();
        }
        else if (indexName == "ProdIndex") {
            $('#ProductModel').modal('hide');
            ctxtProductName.Focus();
        }

    }
}
function SetCustNametxt(id, name) {

    ctxtCustName.SetText(name);
    document.getElementById('hdnCustId').value = id;

    ctxtProductName.Focus();
    $("#MandatorysCustName").hide();


}


    //***Product***//
    function ProductButnClick(s, e) {
        $("#ProductTable").empty();
        var html = "<table border='1' width='100%' class='dynamicPopupTbl'><tr class='HeaderStyle'><th style='display:none'>id</th><th>Product Name</th><th>Product Description</th><th>Min Sale Price</th></tr></table>";
        $("#ProductTable").html(html);
        setTimeout(function () { $("#txtProdSearch").focus(); }, 500);
        $('#txtProdSearch').val('');
        //shouldCheck = 1;
        //$('#mainActMsg').hide();
        $('#ProductModel').modal('show');

    }
function ProductKeyDown(s, e) {
    if (e.htmlEvent.key == "Enter" || e.htmlEvent.key == "NumpadEnter") {
        //shouldCheck = 0;
        s.OnButtonClick(0);
    }
}
function prodkeydown(e) {
    var OtherDetails = {}
    if ($.trim($("#txtProdSearch").val()) == "" || $.trim($("#txtProdSearch").val()) == null) {
        return false;
    }

    OtherDetails.SearchKey = $("#txtProdSearch").val();

    if (e.code == "Enter" || e.code == "NumpadEnter") {
        var HeaderCaption = [];
        HeaderCaption.push("Product Name");
        HeaderCaption.push("Product Description");
        HeaderCaption.push("Min Sale Price");
        if ($("#txtProdSearch").val() != '') {
            callonServer("/OMS/Management/Activities/CustSaleRateLock.aspx/GetProduct", OtherDetails, "ProductTable", HeaderCaption, "ProdIndex", "SetProduct");
        }
    }
    else if (e.code == "ArrowDown") {
        if ($("input[ProdIndex=0]"))
            $("input[ProdIndex=0]").focus();
    }

}
function SetProduct(Id, Name) {
    if (Id) {
        $('#ProductModel').modal('hide');
        SetProdNametxt(Id, Name);
    }
}
function SetProdNametxt(id, name) {
    ctxtMinSalePrice.SetValue("0.00");
    ctxtDiscount.SetValue("0.00");
    ctxtAmount.SetValue("0.00");
    ctxtProductName.SetText(name);
    var varproductid = id.split("||@||")[0];
    var minsaleprice = id.split("||@||")[1];
    document.getElementById('hdnProdId').value = varproductid;
    ctxtDiscount.Focus();
    //ctxtMinSalePrice.SetText = "90";
    ctxtMinSalePrice.SetValue(minsaleprice);
    $("#MandatorysProductName").hide();
}


    /***************Calculation************/
    function AmountCalculate() {
        var minsaleprice = ctxtMinSalePrice.GetValue();
        var percentage = ctxtDiscount.GetValue();
        if (percentage == "0.00") {
            ctxtAmount.SetValue(minsaleprice);
        }
        else {
            var calcPrice = (minsaleprice - (minsaleprice * percentage / 100)).toFixed(2);
            ctxtAmount.SetValue(calcPrice);
        }
    }
function PercentageCalculate() {
    var Amount = ctxtAmount.GetValue();
    var minsaleprice = ctxtMinSalePrice.GetValue();
    if (minsaleprice == "0.00") {
                
    }
    else {
        var calcDis = (((minsaleprice - Amount) * 100) / minsaleprice).toFixed(2);
        if (calcDis == "NaN" || calcDis == "") {
        }
        else {
            ctxtDiscount.SetValue(calcDis);
        }
    }
}



    /****************Save, get, update, Other ************/
    $(document).ready(function () {
        cbtnSaveRecords.Focus()
    });

function SaveButtonClick(flag) {
    $("#MandatorysCustName").hide();
    $("#MandatorysProductName").hide();
    $("#MandatorysDiscount").hide();
    $("#MandatorysFromdt").hide();
    $("#MandatorysTodt").hide();
    $("#MandatorysAmount").hide();

    if (ctxtCustName.GetText() == "") {
        $("#MandatorysCustName").show();
        ctxtCustName.Focus();
        return false;
    }
    if (ctxtProductName.GetText() == "") {
        $("#MandatorysProductName").show();
        ctxtProductName.Focus();
        return false;
    }
    //if (ctxtDiscount.GetValue() == "") {
    //    $("#MandatorysDiscount").show();
    //    ctxtDiscount.Focus();
    //    return false;
    //}
    if (ctxtAmount.GetValue() == "0.00") {
        $("#MandatorysAmount").show();
        ctxtAmount.Focus();
        return false;
    }

    if (cFormDate.GetValue() == "") {
        $("#MandatorysFromdt").show();
        cFormDate.Focus();
        return false;
    }
    if (cToDate.GetValue() == "") {
        $("#MandatorysTodt").show();
        cToDate.Focus();
        return false;
    }
    if (cbtnSaveRecords.GetText() == "Update") {
        flag = "update";
    }
    $.ajax({
        type: "POST",
        url: "/OMS/Management/Activities/CustSaleRateLock.aspx/addSaleRateLock",
        data: JSON.stringify({
            "SaleRateLockID": $.trim($("#HiddenSaleRateLockID").val()), "CustID": $.trim($("#hdnCustId").val()), "ProductID": $.trim($("#hdnProdId").val()), "DiscSalesPrice": ctxtAmount.GetValue(),
            "MinSalePrice": ctxtMinSalePrice.GetValue(), "discount": ctxtDiscount.GetValue(), "fromdt": cFormDate.GetDate(), "todate": cToDate.GetDate(),
            "action": flag
        }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        global: false,
        async: false,
        success: function (msg) {
            if (msg.d) {
                if (flag == "Insert") {
                    if (msg.d == "-12") {
                        alert("From date cannot be greater than to date");
                        $("#MandatorysFromdt").show();
                        return false;
                    }
                    if (msg.d == "-11") {
                        alert("Product already is in sale");
                        return false;
                    }
                    if (msg.d == "-13") {
                        alert("From date and to date is same");
                        return false;
                    }
                    else {
                        alert("Added Successfully");
                        $("#entry").hide();
                        $("#view").show();
                        $("#lblheading").html("Sale Rate Lock");
                        $("#divAddButton").show();
                        $("#divcross").hide();
                        clear();
                        cGridSaleRate.Refresh();
                        return false;
                    }
                }
                if (flag == "update") {
                    if (msg.d == "-12") {
                        alert("From date cannot be greater than to date");
                        $("#MandatorysFromdt").show();
                        return false;
                    }
                    if (msg.d == "-11") {
                        alert("Product already is in sale");
                        return false;
                    }
                    else {
                        $("#entry").hide();
                        $("#view").show();
                        $("#lblheading").html("Sale Rate Lock");
                        $("#divAddButton").show();
                        $("#divcross").hide();
                        cGridSaleRate.Refresh();
                        $("#txtCustName_B0").show();
                        clear();
                        return false;
                    }
                }
            }
        },
        error: function (response) {
            console.log(response);
        }
    });

}
function SaleRateCustomButtonClick(s, e) {
    var id = s.GetRowKey(e.visibleIndex);
    if (e.buttonID == 'CustomBtnEdit') {
        if (id != "") {
            document.getElementById('HiddenSaleRateLockID').value = id;
            $.ajax({
                type: "POST",
                url: "/OMS/Management/Activities/CustSaleRateLock.aspx/GetSaleRateLock",
                data: JSON.stringify({ "SaleRateLockID": id }),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                global: false,
                async: false,
                success: OnSuccess
            });
        }
    }
    if (e.buttonID == 'CustomBtnDelete') {
        if (id != "") {
            if (confirm("Are you sure to delete")) {
                $.ajax({
                    type: "POST",
                    url: "/OMS/Management/Activities/CustSaleRateLock.aspx/DeleteSaleRateLock",
                    data: JSON.stringify({ "SaleRateLockID": id }),
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    global: false,
                    async: false,
                    success: function (msg) {
                        if (msg.d) {
                            if (msg.d == "-998") {
                                alert("Already is in used.Unable to delete");
                                return false;
                            }
                            if (msg.d == "-999") {
                                alert("Deleted Successfuly");
                                cGridSaleRate.Refresh();
                                return false;
                            }
                        }
                    }
                });
            }
        }

    }
}
function OnSuccess(data) {
    for (var i = 0; i < data.d.length; i++) {
        if (data.d[i].IsInUse == "1") {
            alert("Already is in used.Unable to delete");
            return false;
        }
        ctxtCustName.SetText(data.d[i].CustomerName);
        document.getElementById('hdnCustId').value = data.d[i].CustomerID;
        ctxtProductName.SetText(data.d[i].Products_Name);
        document.getElementById('hdnProdId').value = data.d[i].ProductID;
        ctxtMinSalePrice.SetValue(data.d[i].MinSalePrice);
        ctxtDiscount.SetValue(data.d[i].Disc);
        ctxtAmount.SetValue(data.d[i].DiscSalesPrice);
        var frmdt = new Date(data.d[i].ValidFrom);
        cFormDate.SetDate(frmdt);
        var todt = new Date(data.d[i].ValidUpto);
        cToDate.SetDate(todt);
        ctxtCustName.Focus();
        //ctxtCustName.SetButtonVisible(0);
        $("#txtCustName_B0").hide();
                

    }
    $("#entry").show();
    $("#view").hide();
    $("#divAddButton").hide();
    $("#divcross").show();
    $("#lblheading").html("Modify Sale Rate Lock");
    cbtnSaveRecords.SetText("Update");
}

function OnAddButtonClick() {
    $("#divAddButton").hide();
    $("#entry").show();
    $("#view").hide();
    $("#lblheading").html("Add Sale Rate Lock");
    $("#divcross").show();
    ctxtCustName.Focus();
    //ctxtCustName.SetButtonVisible(1);
    $("#txtCustName_B0").show();
    clear();
}
function cancel() {
    clear();
    $("#entry").hide();
    $("#view").show();
    $("#lblheading").html("Sale Rate Lock");
    $("#divAddButton").show();
    cbtnSaveRecords.Focus()
    $("#divcross").hide();
    $("#txtCustName_B0").show();
}
function clear() {
    ctxtCustName.SetText("");
    document.getElementById('hdnCustId').value = "";
    ctxtProductName.SetText("");
    document.getElementById('hdnProdId').value = "";
    ctxtMinSalePrice.SetValue("0.00");
    ctxtDiscount.SetValue("0.00");
    ctxtAmount.SetValue("0.00");
    var frmdt = new Date($.trim($("#Hiddenvalidfrom").val()));
    cFormDate.SetDate(frmdt);
    var todt = new Date($.trim($("#Hiddenvalidupto").val()));
    cToDate.SetDate(todt);
    cbtnSaveRecords.SetText("S&#818;ave");
    $("#MandatorysCustName").hide();
    $("#MandatorysProductName").hide();
    $("#MandatorysDiscount").hide();
    $("#MandatorysFromdt").hide();
    $("#MandatorysTodt").hide();
    $("#MandatorysAmount").hide();

}
$(document).ready(function () {
    console.log('ready');
    $('.navbar-minimalize').click(function () {
        console.log('clicked');
        cGridSaleRate.Refresh();
    });
});

function gridRowclick(s, e) {
    $('#GridSaleRate').find('tr').removeClass('rowActive');
    $('.floatedBtnArea').removeClass('insideGrid');
    //$('.floatedBtnArea a .ico').css({ 'opacity': '0' });
    $(s.GetRow(e.visibleIndex)).find('.floatedBtnArea').addClass('insideGrid');
    $(s.GetRow(e.visibleIndex)).addClass('rowActive');
    setTimeout(function () {
        //alert('delay');
        var lists = $(s.GetRow(e.visibleIndex)).find('.floatedBtnArea a');
        //$(s.GetRow(e.visibleIndex)).find('.floatedBtnArea a .ico').css({'opacity': '1'});
        //$(s.GetRow(e.visibleIndex)).find('.floatedBtnArea a').each(function (e) {
        //    setTimeout(function () {
        //        $(this).fadeIn();
        //    }, 100);
        //});    
        $.each(lists, function (index, value) {
            //console.log(index);
            //console.log(value);
            setTimeout(function () {
                console.log(value);
                $(value).css({ 'opacity': '1' });
            }, 100);
        });
    }, 200);
}
