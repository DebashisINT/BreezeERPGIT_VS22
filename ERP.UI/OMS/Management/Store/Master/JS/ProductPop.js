var prpSet;
var ShowUOMConversion;

function selectChangeUnit()
{
    if($("#Unit").val()=="0")
    {
        $("#AltUnit").prop("disabled", true);
        $("#Altqnty").prop("disabled", true);
        $("#qnty").prop("disabled", true);
        $("#MainUnit").prop("disabled", true);
        $("#MainUnit").val($("#Unit").val());
        document.getElementById("OverrideUOM").disabled = true;
    }
    else
    {
        $("#MainUnit").prop("disabled", true);
        $("#MainUnit").val($("#Unit").val());
        $("#AltUnit").prop("disabled", false);
        $("#Altqnty").prop("disabled", false);
        $("#qnty").prop("disabled", false);
        document.getElementById("OverrideUOM").disabled = false;
    }
    
}


function UniqueCodeProductCheck() {
    debugger;
    var SchemeVal = $("#NumScheme").val();

    var NoSchemeId = SchemeVal.toString().split('~')[0];
    if (SchemeVal == "0") {
        jAlert('Please Select Numbering Scheme');
        //ctxt_SlOrderNo.SetValue('');
        //ctxt_SlOrderNo.Focus();
    }

        //if (NoSchemeId == "0")
    else {
        var CheckUniqueCode = false;
        var uccName = "";
        var Type = "";
       // if (($("#hdnAutoNumStg").val() == "PDAutoNum1") && ($("#hdnTransactionType").val() == "PD")) {
            uccName = $("#DocNum").val();
            Type = "MasterProduct";
        //}


        $.ajax({
            type: "POST",
            url: "sProducts.aspx/CheckUniqueNumberingCode",
            data: JSON.stringify({ uccName: uccName, Type: Type }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                CheckUniqueCode = msg.d;
                if (CheckUniqueCode == true) {
                    //alert('Please enter unique No.');
                    //jAlert('Please enter unique Sales Order No');
                    $("#DocNum").val('');
                    $("#DocNum").focus();
                }

            }

        });
    }
}

function OpenProductMaster() {
    var url = '/OMS/management/store/Master/sProducts.aspx';
    window.location.href = url;
}
function TextChanged_Productname()
{
    var procode = 0;
    var ProductName = $("#PCode").val();
    //chinmoy comment below line 30-07-2019
    //if (procode != "") {
    $.ajax({
        type: "POST",
        url: "ProductServices/ProductPop.asmx/CheckUniqueName",
        //data: "{'ProductName':'" + ProductName + "'}",
        data: JSON.stringify({ ProductName: ProductName, procode: procode }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var data = msg.d;

            if (data == true) {
                //jAlert("Please enter unique name", "Alert", function () { $("#PName").focus(); });
                //ctxtPro_Code.SetText("");

                //jAlert("Please enter Product code");
                jAlert("Please enter Short Name (Unique)");
                $("#PCode").val("");

                //jAlert("Please enter unique product cSetClassode");
               // $("#PCode").val("");	

                $("#PCode").focus();
                return false;
            }
        }

    });
}

$(function () {
    $('#qnty').on('input', function () {
        this.value = this.value
          //.replace(/[^\d.]/g, '')             // numbers and decimals only
          .replace(/(^[\d]{6})[\d]/g, '$1')   // not more than 2 digits at the beginning
          //.replace(/(\..*)\./g, '$1')         // decimal can't exist more than once
          .replace(/(\.[\d]{4})./g, '$1');    // not more than 4 digits after decimal
    });
});

$(function () {
    $('#Altqnty').on('input', function () {
        this.value = this.value
          //.replace(/[^\d.]/g, '')             // numbers and decimals only
          .replace(/(^[\d]{6})[\d]/g, '$1')   // not more than 2 digits at the beginning
          //.replace(/(\..*)\./g, '$1')         // decimal can't exist more than once
          .replace(/(\.[\d]{4})./g, '$1');    // not more than 4 digits after decimal
    });
});
function ChangeOfServiceitem()
{

    var txt = "<table border='1' width=\"100%\" class='dynamicPopupTbl'><tr class=\"HeaderStyle\" ><th class=\"hide\">id</th><th style='width:150px;'>Code</th><th>Description</th></tr><table>";
    //var txt = "<table border='1' width=\"100%\"><tr class=\"HeaderStyle\"><th style='width:150px;'>Code</th><th>Description</th></tr><table>";
    $("#txtHSNSearch").val("");
    document.getElementById("HSNTable").innerHTML = txt;
    $('#HSNId').val("0")
    $('#HSNName').val("");

    if (($("#servItm").val() == "1") && ($("#InvProd").val() == "1"))
    {
        $("#InvProd").val("0");
    }
    if ($("#servItm").val() == "1")
    {
        $("#Unit").val("0");
        $("#Unit").prop("disabled", true);
        $("#AltUnit").prop("disabled", true);
        $("#Altqnty").prop("disabled", true);
        $("#qnty").prop("disabled", true);
        $("#MainUnit").prop("disabled", true);
        document.getElementById("OverrideUOM").disabled = true;
        $("#MainUnit").val($("#Unit").val());
        $("#AltUnit").val("0");
        $("#Altqnty").val(0);
        $("#qnty").val(0);
        $("#MainUnit").val("0");
        $("#MainUnit").val($("#Unit").val());
    }
    
}

function ChangeOfInventory()
{
    //var txt = "<table border='1' width=\"100%\" class='dynamicPopupTbl'><tr class=\"HeaderStyle\" ><th class=\"hide\">Id</th><th>Class Name</th></tr><table>";
    //$("#txtClassSearch").val("");
    //document.getElementById("ClassTable").innerHTML = txt;
    //$('#HSNId').val("0")
    //$('#HSNName').val("");

    var txt = "<table border='1' width=\"100%\" class='dynamicPopupTbl'><tr class=\"HeaderStyle\" ><th class=\"hide\">id</th><th style='width:150px;'>Code</th><th>Description</th></tr><table>";
    //var txt = "<table border='1' width=\"100%\"><tr class=\"HeaderStyle\"><th style='width:150px;'>Code</th><th>Description</th></tr><table>";
    $("#txtHSNSearch").val("");
    document.getElementById("HSNTable").innerHTML = txt;
    $('#HSNId').val("0")
    $('#HSNName').val("");


    if($("#InvProd").val()=="1")
    {
        $("#servItm").prop("disabled",true);
    }
    else if ($("#InvProd").val() == "0") {
        $("#servItm").prop("disabled", false);
        //$("#servItm").prop("disabled", true);
    }
    if (($("#servItm").val() == "1") && ($("#InvProd").val() == "1"))
    {
        $("#servItm").val("0");
    }
    if ($("#InvProd").val() == "0") {

        $("#AltUnit").prop("disabled", true);
        $("#Altqnty").prop("disabled", true);
        $("#qnty").prop("disabled", true);
        $("#Unit").prop("disabled", true);
    }
    else if($("#InvProd").val() == "1")
    {
        $("#AltUnit").prop("disabled", false);
        $("#Altqnty").prop("disabled", false);
        $("#qnty").prop("disabled", false);
        $("#Unit").prop("disabled", false);
    }
    if ($("#InvProd").val() == "0") {
        $("#Unit").val("0");
        $("#Unit").prop("disabled", true);
        $("#AltUnit").prop("disabled", true);
        $("#Altqnty").prop("disabled", true);
        $("#qnty").prop("disabled", true);
        $("#MainUnit").prop("disabled", true);
        document.getElementById("OverrideUOM").disabled = true;
        $("#MainUnit").val($("#Unit").val());
        $("#AltUnit").val("0");
        $("#Altqnty").val(0);
        $("#qnty").val(0);
        $("#MainUnit").val("0");
        $("#MainUnit").val($("#Unit").val());
    }
}

function Classkeydown(e) {
    //debugger;
    var OtherDetails = {}
    OtherDetails.SearchKey = $("#txtClassSearch").val();
    //OtherDetails.contactType = $('#ContactID').text();

    if (e.code == "Enter" || e.code == "NumpadEnter") {
        var HeaderCaption = [];
        HeaderCaption.push("Class Name");
       
        if ($("#txtClassSearch").val() != '') {
            callonServer("ProductServices/ProductPop.asmx/GetClassDetails", OtherDetails, "ClassTable", HeaderCaption, "ClassIndex", "SetClass");
        }
    }
    else if (e.code == "ArrowDown") {
        if ($("input[ClassIndex=0]"))
            $("input[ClassIndex=0]").focus();
    }
    else if (e.code == "Escape") {
        $("ClassModel").hide();
    }
}
function HSNkeydown(e) {
    //debugger;
    var OtherDetails = {}
    OtherDetails.SearchKey = $("#txtHSNSearch").val();
    //OtherDetails.contactType = $('#ContactID').text();

    if (e.code == "Enter" || e.code == "NumpadEnter") {
        var HeaderCaption = [];
        HeaderCaption.push("Code");
        HeaderCaption.push("Description");

        if ($("#txtHSNSearch").val() != '') {
            if ($("#servItm").val() == "0") {
                callonServer("ProductServices/ProductPop.asmx/GetHSNDetails", OtherDetails, "HSNTable", HeaderCaption, "HSNIndex", "SetHSN");
            }
            else if ($("#servItm").val()=="1")
            {
                callonServer("ProductServices/ProductPop.asmx/GetSACDetails", OtherDetails, "HSNTable", HeaderCaption, "HSNIndex", "SetHSN");
            }
        }
    }
    else if (e.code == "ArrowDown") {
        if ($("input[HSNIndex=0]"))
            $("input[HSNIndex=0]").focus();
    }
    else if (e.code == "Escape") {
        //$("HSNModel").hide();
        $('#HSNModel').modal('hide');
    }
}
function ClassButnClick() {
    var txt = "<table border='1' width=\"100%\" class='dynamicPopupTbl'><tr class=\"HeaderStyle\" ><th class=\"hide\">Id</th><th>Class Name</th></tr><table>";
    $("#txtClassSearch").val("");
    document.getElementById("ClassTable").innerHTML = txt;
    //$("ClassModel").Show();
    $('#ClassModel').modal('show');
    $("#txtClassSearch").focus();
    //$('#ClassModel').modal({
    //    backdrop: 'static',
    //    keyboard: false
    //})

}
function HSNButnClick() {
    var txt = "<table border='1' width=\"100%\" class='dynamicPopupTbl'><tr class=\"HeaderStyle\" ><th class=\"hide\">id</th><th style='width:150px;'>Code</th><th>Description</th></tr><table>";
    //var txt = "<table border='1' width=\"100%\"><tr class=\"HeaderStyle\"><th style='width:150px;'>Code</th><th>Description</th></tr><table>";
    $("#txtHSNSearch").val("");
    document.getElementById("HSNTable").innerHTML = txt;
    //$("ClassModel").Show();
    $('#HSNModel').modal('show');
    $("#txtHSNSearch").focus();
    //$('#ClassModel').modal({
    //    backdrop: 'static',
    //    keyboard: false
    //})

}
function Class_KeyDown() {

    //debugger;
    //if (e.htmlEvent.key == "Enter" || e.code == "NumpadEnter") {
    //   // s.OnButtonClick(0);
    //    $("#txtClassSearch").focus();
    //}
}
function HSN_KeyDown() {

    //debugger;
    //if (e.htmlEvent.key == "Enter" || e.code == "NumpadEnter") {
    //   // s.OnButtonClick(0);
    //    $("#txtClassSearch").focus();
    //}
}
function SetClass(id, Name) {
    debugger;
    var key = id;
    $('#ClassId').val(id)
    if (key != null && key != '') {
      
        $('#ClassName').val(Name);
        //$("ClassModel").hide();
        $('#ClassModel').modal('hide');
        
        setTimeout(function () {
            if (datavalProduct == "1") {
                $("#NumScheme").focus();
            }
            else {
                document.getElementById("PCode").focus();
            }
        }, 1000);
    }
}

function SetHSN(id, Name) {
    debugger;
    var key = id;
    $('#HSNId').val(id)
    if (key != null && key != '') {

        $('#HSNName').val(Name);
        //$("ClassModel").hide();
        $('#HSNModel').modal('hide');
        $("#Unit").focus();
    }
}

function ValueSelected(e, indexName)
{
    if (indexName == "ClassIndex") {
    if (e.code == "Enter" || e.code == "NumpadEnter") {
        var Code = e.target.parentElement.parentElement.cells[0].innerText;
        var name = e.target.parentElement.parentElement.cells[1].children[0].value;
      
        $("#ClassId").val(Code);
        $('#ClassName').val(name);
        $("#ClassModel").hide();
        
        //cPopClass.Hide();
    } else if (e.code == "ArrowDown") {
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
            $('#txtClassSearch').focus();
        }
    }
   
    }
    else if (indexName == "HSNIndex") {
        if (e.code == "Enter" || e.code == "NumpadEnter") {
            var Code = e.target.parentElement.parentElement.cells[0].innerText;
            var name = e.target.parentElement.parentElement.cells[1].children[0].value;

            $("#HSNId").val(Code);
            $('#HSNName').val(name);
            $("#HSNModel").hide();
            $("#Unit").focus();
            //cPopClass.Hide();
        } else if (e.code == "ArrowDown") {
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
                $('#txtHSNSearch').focus();
            }
        }

    }
}

function isNumberKey(evt) {
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if (charCode != 46 && charCode > 31
      && (charCode < 48 || charCode > 57))
        return false;

    return true;
}
function SaveProduct() {
    debugger;
    if (datavalProduct != "1") {
        $("#PCode").closest('div.form-group').removeClass('has-error');
    }

    $("#Product_Name").closest('div.form-group').removeClass('has-error');
    $("#Unit").closest('div.form-group').removeClass('has-error');
    $("#qnty").closest('div.form-group').removeClass('has-error');
    $("#AltUnit").closest('div.form-group').removeClass('has-error');
    $("#Altqnty").closest('div.form-group').removeClass('has-error');
    $("#MainUnit").closest('div.form-group').removeClass('has-error');

    $("#AltName").closest('div.form-group').removeClass('has-error');
    $("#AltUnit").closest('div.form-group').removeClass('has-error');
    if (datavalProduct == "1") {
        $("#NumScheme").closest('div.form-group').removeClass('has-error');
        $("#DocNum").closest('div.form-group').removeClass('has-error');
    }

    var HsnVal = "0";
    var serviceTax = 0;
    var Productname = "";
    var Inventory = $("#InvProd").val();
    if (Inventory == "1")
    {
        var IsInventory = true;
    }
    else
    {
        IsInventory = false;
    }


    var UniqueNum = "";
    var NumberingId = "0";
    var NumberingVal = $("#NumScheme").val()
    if (datavalProduct != "1") {
        Productname = $("#PCode").val();
        NumberingId = "0"
    }
    else {
        UniqueNum = $("#DocNum").val();
        NumberingId = NumberingVal.split("~")[0]
    }
    AddProduct_name = $("#Product_Name").val();

    var ServiceItem = $("#servItm").val();
    if (ServiceItem == "1") {
        var service = true;
    }
    else {
        service = false;
    }

    var Capitalgoods = $("#Capgoods").val();
    if (Capitalgoods == "1") {
        var captGoods = true;
    }
    else {
        captGoods = false;
    }
    var AlternateName = $("#AltName").val();
    var Class = $("#ClassId").val();
    var Unit = $("#Unit").val();
    
    var Qnty = $("#qnty").val();
    if (Qnty == "")
    {
        Qnty = 0;
    }
    var altUnit = $("#AltUnit").val();
    var Altqnty = $("#Altqnty").val();
    if (Altqnty == "") {
        Altqnty = 0;
    }
    if($("#servItm").val() == "0")
    {
         HsnVal = $("#HSNId").val();
    }
    else if($("#servItm").val() == "1")
     {
        var serviceTax = $("#HSNId").val();
    }

    

    if (datavalProduct =="1")
    {
        if( $("#NumScheme").val()=="0")
        {
            //alert("Please Select Numbering Scheme");
            $("#NumScheme").focus();
            $("#NumScheme").closest('div.form-group').addClass('has-error');
            return false;
        }
        if($("#DocNum").val()=="")
        {
            //alert("Please select document number.")
            $("#DocNum").focus();
            $("#DocNum").closest('div.form-group').addClass('has-error');
            return false;
        }

    }
    if (datavalProduct != "1") {
        if (Productname == "") {
            //alert("Enter Valid Prodcut Name.");
            $("#PCode").focus();
            $("#PCode").closest('div.form-group').addClass('has-error');
            return false;
        }
    }

    if (AddProduct_name == "") {
        $("#Product_Name").focus();
        $("#Product_Name").closest('div.form-group').addClass('has-error');
        return false;
    }

    if (prpSet == "Yes") {
        if ($("#AltName").val() == "")
        {
            // alert("Enter Alternate Prodcut Name.");
           
            $("#AltName").focus();
            $("#AltName").closest('div.form-group').addClass('has-error');
            return false;
        }
    }
    if (ShowUOMConversion != "1") {
        if ((ServiceItem == "0") && (Inventory == "1")) {
            if ($("#Unit").val() == "0") {
                $("#Unit").focus();
                $("#Unit").closest('div.form-group').addClass('has-error');
                return false;
            }
            //else if (Qnty == 0) {
            //    $("#qnty").focus();
            //    $("#qnty").closest('div.form-group').addClass('has-error');
            //    return false;
            //}
            //else if (altUnit == "0") {
            //    $("#AltUnit").focus();
            //    $("#AltUnit").closest('div.form-group').addClass('has-error');
            //    return false;
            //}
            //else if (Altqnty == 0) {
            //    $("#Altqnty").focus();
            //    $("#Altqnty").closest('div.form-group').addClass('has-error');
            //    return false;
            //}
            //else if ($("#MainUnit").val() == "0") {
            //    $("#MainUnit").focus();
            //    $("#MainUnit").closest('div.form-group').addClass('has-error');
            //    return false;
            //}
        }
    }
    if (ShowUOMConversion == "1") {
        if (Inventory == "1") {

            if ($("#Unit").val() == "0") {
                console.log('"Show UOM Conversion In Entry" is Activated.You must Select Unit.');
                $("#Unit").focus();
                $("#Unit").closest('div.form-group').addClass('has-error');
                return false;
            }
            if ($("#MainUnit").val() == "0") {
                console.log('"Show UOM Conversion In Entry" is Activated.You must Select Main Unit.');
                $("#MainUnit").focus();
                $("#MainUnit").closest('div.form-group').addClass('has-error');
                return false;
            }
            if (Qnty == 0) {
                console.log('"Show UOM Conversion In Entry" is Activated.You must Select Quantity.');
                $("#qnty").focus();
                $("#qnty").closest('div.form-group').addClass('has-error');
                return false;
            }
            if ($("#AltUnit").val() == "0") {
                console.log('"Show UOM Conversion In Entry" is Activated.You must Select alternate UOM.');
                $("#AltUnit").focus();
                $("#AltUnit").closest('div.form-group').addClass('has-error');
                return false;
            }
            if (Altqnty == 0) {
                console.log('"Show UOM Conversion In Entry" is Activated.You must Select alternate Quantity.');
                $("#Altqnty").focus();
                $("#Altqnty").closest('div.form-group').addClass('has-error');
                return false;
            }
        }
    }

  


    var ProductCreateUser = 0;
    var ProductpopDetails = {};

    if (datavalProduct == "1") {
        ProductpopDetails.ProductCode = UniqueNum;
        ProductpopDetails.ProductName = AddProduct_name;
        ProductpopDetails.ProductDescription = AddProduct_name;
    }
    else {
        ProductpopDetails.ProductCode = Productname;
        ProductpopDetails.ProductName = AddProduct_name;
        ProductpopDetails.ProductDescription = AddProduct_name;
    }

    
    ProductpopDetails.ProductType = "0";
    ProductpopDetails.ProductClassCode = Class;
    ProductpopDetails.ProductGlobalCode = "";
    ProductpopDetails.ProductTradingLot = 1;
    ProductpopDetails.productTradingLotUnit = Unit;
    ProductpopDetails.ProductQuoteCurrency = 1;
    ProductpopDetails.ProductQuoteLot = 1;
    ProductpopDetails.productQuoteLotUnit = 1;
    ProductpopDetails.ProductDeliveryLot = 1;
    ProductpopDetails.ProductDeliveryLotUnit = Unit;
    ProductpopDetails.ProductColor = 0;
    ProductpopDetails.ProductSize = 0;
    ProductpopDetails.ProductCreateUser = ProductCreateUser;
    ProductpopDetails.sizeapplicable = false;
    ProductpopDetails.colorapplicable = false;
    ProductpopDetails.BarCodeSymbology = 0;
    ProductpopDetails.BarCode = "";
    ProductpopDetails.isInventory = IsInventory;
    ProductpopDetails.stkValuation = "F";
    ProductpopDetails.salePrice = 0;
    ProductpopDetails.minSalePrice = 0;
    ProductpopDetails.purPrice = 0;
    ProductpopDetails.MRP = 0;
    ProductpopDetails.stockUOM = Unit;
    ProductpopDetails.minLvl = 0;
    ProductpopDetails.reOrderlvl = 0;
    ProductpopDetails.negativeStock = "W";
    ProductpopDetails.taxCodeSaleScheme = 0;
    ProductpopDetails.taxCodePur = 0;
    ProductpopDetails.taxScheme = 0;
    ProductpopDetails.autoApply = false;
    ProductpopDetails.ImagePath = "";
    ProductpopDetails.ProdComponent = "0";
    ProductpopDetails.ProdStatus = "";
    ProductpopDetails.hsnValue = HsnVal;
    ProductpopDetails.serviceTax = serviceTax;
    ProductpopDetails.quantity = Qnty;
    ProductpopDetails.packing = Altqnty;
    ProductpopDetails.packingUOM = altUnit;
    ProductpopDetails.OverideConvertion = $("#OverrideUOM").is(":checked");
    ProductpopDetails.IsMandatory = false;
    ProductpopDetails.isInstall = false;
    ProductpopDetails.Brand = 0;
    ProductpopDetails.isCapitalGoods = captGoods;
    ProductpopDetails.TdsCode = 0;
    ProductpopDetails.FinYear = "0";
    ProductpopDetails.isOldUnit = false;
    ProductpopDetails.salesInvMainAct = "";
    ProductpopDetails.salesRetMainAct = "";
    ProductpopDetails.purInv = "";
    ProductpopDetails.purRetMainAct = "";
    ProductpopDetails.FurtheranceToBusiness = $("#FurtherBusiness").is(":checked");
    ProductpopDetails.IsServiceItem = service;
    ProductpopDetails.reorder_qty = 0;
    ProductpopDetails.maxLvl = 0;
    ProductpopDetails.lenght = "0";
    ProductpopDetails.width = "0";
    ProductpopDetails.Thickness = "0";
    ProductpopDetails.size = "0";
    ProductpopDetails.SUOM = "1";
    ProductpopDetails.series = "";
    ProductpopDetails.Finish = "";
    ProductpopDetails.LeadTime = "0";
    ProductpopDetails.Coverage = "0";
    ProductpopDetails.covuom = "";
    ProductpopDetails.volume = "0";
    ProductpopDetails.volumeuom = "0";
    ProductpopDetails.weight = "0";
    ProductpopDetails.printname = AlternateName;
    ProductpopDetails.subcat = "";
    ProductpopDetails.NumberingId = NumberingId;

    $.ajax({
        type: "POST",
        url: "ProductServices/ProductPop.asmx/InsertLightweightProductPopUp",
        data: JSON.stringify(ProductpopDetails),
        contentType: "application/json; charset=utf-8",
        datatype: "JSON",
       
        success: function (data) {

            var product = data.d;

            debugger;
            if (product !== "0") {
                jAlert("Save successfully");
                parent.fn_productSave();
            }
            else if (product == "0") {
                jAlert("Either Unique ID Exists OR Unique ID Exhausted.");
            }
           
        },
        error: function (data) {
          
            jAlert("Please try again later");
        }
    });

  
}


var err;



var datavalProduct;
$(document).ready(function () {
    debugger;
   
    $("#ClassModel").on('shown.bs.modal', function () {
        $(this).find('#txtClassSearch').focus();
    });

    $("#HSNModel").on('shown.bs.modal', function () {
        $(this).find('#txtHSNSearch').focus();
    });



    $.ajax({
        url: "ProductServices/ProductPop.asmx/getProductNumberSchemeSettings",
        contentType: "application/json; charset=utf-8",
        datatype: "JSON",
        type: "POST",
        async: false,
        success: function (data) {
            debugger;
            datavalProduct = data.d;
            if (datavalProduct == "1") {

                $("#dvProduct").hide();
                $("#dvdoc").show();
                $("#dvNumberingScheme").show();
            }
            else {
                $("#dvProduct").show();
                $("#dvdoc").hide();
                $("#dvNumberingScheme").hide();
            }

        },
        error: function (data) {

            jAlert("Please try again later");
        }
    });

    if (datavalProduct != "1") {
        $("#PCode").focus();
    }
    else {
        $("#NumScheme").focus();
    }

    $.ajax({
        url: "ProductServices/ProductPop.asmx/NumberingSchemeBind",
        contentType: "application/json; charset=utf-8",
        datatype: "JSON",
        type: "POST",
        success: function (data) {

            $("#NumScheme").empty();
            var grpdetl = data.d;

            debugger;

            var opts = "";
            //opts = "<option value='0'>Select</option>";
            for (i in grpdetl)
                //if (opts == "") {
                //    opts = "<option value='0'>Select</option>"
                //}
                opts += "<option value='" + grpdetl[i].Id + "'>" + grpdetl[i].SchemaName + "</option>";

            $("#NumScheme").empty().append(opts);

        },
        error: function (data) {

            jAlert("Please try again later");
        }
    });




    $.ajax({
        url: "ProductServices/ProductPop.asmx/ProductPopUpSettings",
        contentType: "application/json; charset=utf-8",
        datatype: "JSON",
        type: "POST",
        success: function (data) {

            prpSet = data.d;
          
        },
        error: function (data) {
         
        }
    });
    $.ajax({
        url: "ProductServices/ProductPop.asmx/ProductUOMConversionSettings",
        contentType: "application/json; charset=utf-8",
        datatype: "JSON",
        type: "POST",
        success: function (data) {

            ShowUOMConversion = data.d;

        },
        error: function (data) {

        }
    });

    var HsnVal = "0";
    var serviceTax = 0;
    if (datavalProduct != 1) {
        var Productname = $("#PCode").val();
    }
    var Inventory = $("#InvProd").val();
    if (Inventory == "1") {
        var IsInventory = true;
    }
    else {
        IsInventory = false;
    }

    var ServiceItem = $("#servItm").val();
    if (ServiceItem == "1") {
        var service = true;
    }
    else {
        service = false;
    }

    var Capitalgoods = $("#Capgoods").val();
    if (Capitalgoods == "1") {
        var captGoods = true;
    }
    else {
        captGoods = false;
    }
    var AlternateName = $("#AltName").val();
    var Class = $("#ClassId").val();
    var Unit = $("#Unit").val();

    var Qnty = $("#qnty").val();
    if (Qnty == "") {
        Qnty = 0;
    }
    var altUnit = $("#AltUnit").val();
    var Altqnty = $("#Altqnty").val();
    if (Altqnty == "") {
        Altqnty = 0;
    }
    if ($("#servItm").val() == "0") {
        HsnVal = $("#HSNId").val();
    }
    else if ($("#servItm").val() == "1") {
        var serviceTax = $("#HSNId").val();
    }

    if (datavalProduct != 1) {
        $("#PCode").blur(function () {
            if (($("#PCode").val()) != "") {

                $("#PCode").closest('div.form-group').removeClass('has-error');
            }
        });
    }
    $("#AltName").blur(function () {
    if (prpSet == "Yes") {
        if (($("#AltName").val()) != "") {



            $("#AltName").closest('div.form-group').removeClass('has-error');

        }
    }
    });
    $("#AltUnit").blur(function () {
    if (ShowUOMConversion == "1") {
        if (Inventory == "1") {
            if (($("#AltUnit").val()) != "0") {


                $("#AltUnit").closest('div.form-group').removeClass('has-error');

            }
        }
    }
    });


    if (ServiceItem == "0") {

        $("#Unit").blur(function () {
        if (($("#Unit").val()) != "0") {

            $("#Unit").closest('div.form-group').removeClass('has-error');

        }
        });
        $("#qnty").blur(function () {
            if (Qnty != 0 || Qnty!=undefined) {

            $("#qnty").closest('div.form-group').removeClass('has-error');

        }
        });

        if (datavalProduct == "1") {
            $("#NumScheme").blur(function () {
                if ($("#NumScheme") != "0") {

                    $("#NumScheme").closest('div.form-group').removeClass('has-error');

                }
            });
            $("#DocNum").blur(function () {
                if ($("#DocNum").val() != "" || $("#DocNum").val() != undefined) {

                    $("#DocNum").closest('div.form-group').removeClass('has-error');

                }
            });
        }
        $("#AltUnit").blur(function () {
         if (altUnit != "0") {

            $("#AltUnit").closest('div.form-group').removeClass('has-error');

         }
        });
        $("#Altqnty").blur(function () {
            if (Altqnty != 0 || Altqnty!=undefined) {

            $("#Altqnty").closest('div.form-group').removeClass('has-error');

         }
        });
        $("#MainUnit").blur(function () {
         if (($("#MainUnit").val()) != "0") {

            $("#MainUnit").closest('div.form-group').removeClass('has-error');

         }
        });
    }






});

$(document).ready(function () {
    debugger;
    $("#AltUnit").prop("disabled", true);
    $("#MainUnit").prop("disabled", true);
    $("#Altqnty").prop("disabled", true);
    $("#qnty").prop("disabled", true);
    $("#servItm").prop("disabled", true);
    $("#ClassName").prop("disabled", true);
    $("#HSNName").prop("disabled", true);
    document.getElementById("OverrideUOM").disabled = true;

    $('#NumScheme').change(function () {
        //

        var NoSchemeTypedtl = $("#NumScheme").val();
        var NoSchemeId = NoSchemeTypedtl.toString().split('~')[0];
        var NoSchemeType = NoSchemeTypedtl.toString().split('~')[1];

        var schemeLength = NoSchemeTypedtl.toString().split('~')[2];

        if (NoSchemeType == '1') {
            $("#DocNum").val('Auto');
            $('#DocNum').prop('disabled', true);
            $('#DocNum input').attr('maxlength', schemeLength);


        }
        else if (NoSchemeType == '0') {
            schemeLength = 300;
            $("#DocNum").val('');
            $('#DocNum').prop('disabled', false);
            $('#DocNum input').attr('maxlength', schemeLength);
        }
        else if ($('#NumScheme').val() == "0") {
            $("#DocNum").val('');
            $('#DocNum').prop('disabled', false);
        }

    });



    $.ajax({
        url: "ProductServices/ProductPop.asmx/GetUnit",
        contentType: "application/json; charset=utf-8",
        datatype: "JSON",
        type: "POST",
        success: function (data) {

            $("#Unit").empty();
            $("#AltUnit").empty();
            var grpdetl = data.d;

            debugger;

            var opts = "";
            opts = "<option value='0'>Select</option>";
            for (i in grpdetl)
                //if (opts == "") {
                //    opts = "<option value='0'>Select</option>"
                //}
                opts += "<option value='" + grpdetl[i].UomId + "'>" + grpdetl[i].UomName + "</option>";

            $("#Unit").empty().append(opts);
            $("#MainUnit").empty().append(opts);
            $("#AltUnit").empty().append(opts);
        },
        error: function (data) {
            debugger;
            err = data;
            jAlert(data);
        }
    });
});