
function ValueSelected(e, indexName) {
    // debugger;
    if (e.code == "Enter" || e.code == "NumpadEnter") {
        var Id = e.target.parentElement.parentElement.cells[0].innerText;
        var Name = e.target.parentElement.parentElement.cells[1].children[0].value;
        if (Id) {

            if (indexName == "customerIndex") {
                SetCustomer(Id, Name);
            }
            else if (indexName == "GridProductIndex") {
                SetGridProduct(Id, name, null);
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
            else if (indexName == "GridProductIndex")
                $('#txtGridProductName').focus();
        }
    }

}


function CustomerButnClick(s, e) {
    $('#CustModel').modal('show');
}

function Customer_KeyDown(s, e) {
    if (e.htmlEvent.key == "Enter" || e.code == "NumpadEnter")
    {
        $('#CustModel').modal('show');
    }
}
function Customerkeydown(e) {
    var OtherDetails = {}
    if ($.trim($("#txtCustSearch").val()) == "" || $.trim($("#txtCustSearch").val()) == null) {
        return false;
    }
    OtherDetails.SearchKey = $("#txtCustSearch").val();
    // OtherDetails.BranchID = $('#ddl_Branch').val();

    if (e.code == "Enter" || e.code == "NumpadEnter") {
        var HeaderCaption = [];
        HeaderCaption.push("Name");
        HeaderCaption.push("Unique Id");
        HeaderCaption.push("Address");

        if ($("#txtCustSearch").val() != "") {
            callonServer("../CRMActivity/GetCustomer", OtherDetails, "CustomerTable", HeaderCaption, "customerIndex", "SetCustomer");
        }
    }
    else if (e.code == "ArrowDown") {
        if ($("input[customerindex=0]"))
            $("input[customerindex=0]").focus();
    }
}


function SetCustomer(id, Name) {

    var key = id;
    $('#CustomerId').val(id)
    if (key != null && key != '') {
        $('#CustModel').modal('hide');
        CustomerTxt.SetText(Name);
        CustomerTxt.SetFocus();
        var obj = {};
        gridLookupcontact.SetValue(null);
        obj.Customer_ID = $('#CustomerId').val();
        $.ajax({
            type: "POST",
            url: "../ServiceContact/PopulateSummaryContactGrid",
            data: JSON.stringify(obj),
            async: false,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
               
            }
        });
    }
}


function AddLiterature(id, name, code) {


    $('#AttachmentModal').modal('show');

    $("#hdnLead_Id").val(id);
    $('#hdnDocNo').val(id);
    $('#hdndoc_id').val(id);
    $("#docFileName").val(code);
    $("#docNumber").val(name);
    document.getElementById("docFileName").disabled = true;
    document.getElementById("docNumber").disabled = true;

    setTimeout(function () { $('#documentType').focus(); }, 1000);
}

function datevalidateTo() {

    if (cRenewalEndDate_Close.GetText() != cRenewalStartDate_Close.GetText()) {
        if (cRenewalStartDate_Close.GetDate() > cRenewalEndDate_Close.GetDate()) {

            cRenewalStartDate_Close.Clear();
        }
    }
}


function WarrdatevalidateTo() {

    if (cWarrStartdate.GetText() != cWarrEnddate.GetText()) {
        if (cWarrStartdate.GetDate() > cWarrEnddate.GetDate()) {

            cWarrStartdate.Clear();
        }
    }
}

function ValidationServicecontact() {
    var isvalid = false;
    if (CustomerTxt.GetText() == "") {
        isvalid = false;
        jAlert("Account/Customer is Mandatory"); return;
    }
    else if($("#SerViceName").val()=="")
    {
        isvalid = false;
        jAlert("Service Name is Mandatory"); return;
    }
    else {
        isvalid = true;
    }
 
    return isvalid;
}



function SaveServiceContact()
{
    if (ValidationServicecontact()) {
        var obj = {};

        $("#hdnLead_Id").val() == ""

        if ($("#hdnLead_Id").val() == "" || $("#hdnLead_Id").val()=="0") {
            obj.Action = "Add";
            $("#hdnActionType").val('Add');
            obj.ServiceCnt_Id = 0;
        }
        else {
            obj.Action = "Update";
            $("#hdnActionType").val('Update');
            obj.ServiceCnt_Id = $("#hdnLead_Id").val();
        }

        //$("#OwnerID").val($("#hdnOwnerAssignID").val());
        obj.OwnerID = $("#OwnerID").val();
        obj.AssignedID = $("#AssignedID").val();
        obj.ServiceName = $("#SerViceName").val();
        obj.UniqueId = $("#UniqueId").val();
        obj.Customer_ID = $('#CustomerId').val();
        var contacts = gridLookupcontact.GetSelectedKeyFieldValues();
        obj.ServiceStatusId = $("#StatusId").val();
        obj.crmcontacts_id = contacts.join();
        obj.RenewalDate = cRenewalDate_Close.GetText();//GetDate();
        obj.RenewalStartDate = cRenewalStartDate_Close.GetText();//GetDate();
        obj.RenewalEndDate = cRenewalEndDate_Close.GetText();//GetDate();
        obj.Servicedesc = $("#servicedesc").val();
        obj.ServiceAmount = cServiceAmount.GetText();
        obj.ProdServCost = cProdServCost.GetText();
        obj.AdditionalCost = cAdditionalCost.GetText();
        LoadingPanel.Show();
        $.ajax({
            type: "POST",
            url: "/ServiceContact/SaveServiceContact",
            data: JSON.stringify(obj),
            async: false,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                jAlert(response);
                $("#OwnerID").val('');
                $("#AssignedID").val('');
                $("#hdnLead_Id").val('');
                $("#SerViceName").val(''); 
                $("#UniqueId").val('');
                CustomerTxt.SetText('');
                $("#CustomerId").val('');
                cRenewalStartDate_Close.Clear();
                cRenewalEndDate_Close.Clear();
                $("#servicedesc").val('');
                cServiceAmount.SetValue(0.00); 
                cProdServCost.SetValue(0.00);
                cAdditionalCost.SetValue(0.00);
                $("#addFservice").modal('hide');
                GetServiceProductEntryList.Refresh();
                GetServiceProductEntryList.Refresh();
                gvPaging.Refresh();
                gvPaging.Refresh();

                CloseHeadModal();
                //$("#addLead").modal('hide');
                LoadingPanel.Hide();
            },
            error: function (response) {
                jAlert("Please try again later");
                LoadingPanel.Hide();
            }
        });

    }
}

function OpenProductList(s, e) {


    GetServiceProductEntryList.batchEditApi.EndEdit();
   
    //var ProductName = GetServiceProductEntryList.batchEditApi.GetCellValue(e.visibleIndex, 'ProductName');
   

    if ($("#CustomerId").val() == null || $("#CustomerId").val() == "") {
        jAlert('Please select customer.');
        CustomerTxt.Focus();
        LoadingPanel.Hide();
    }
    else {
       
        var Searchvalue = $("#txtGridProductName").val();
        GridProductlist(Searchvalue);
       

        var txt = "<table  class='dynamicPopupTbl' width=\"100%\"><tr class=\"HeaderStyle\"><th class='hide'>id</th><th>Product Name</th><th>Description</th><th>HSN/SAC</th></tr><table>";
        $("#txtGridProductName").val("");
        document.getElementById("GridProductTable").innerHTML = txt;
        $('#GridProductlistModel').modal('show');
        $('#txtGridProductName').focus();
        setTimeout(function () {
            $('#txtGridProductName').focus();
        }, 200);
    }
}

function ProductKeyDown(s, e) {
    console.log(e.htmlEvent.key);
    if (e.htmlEvent.key == "Enter")
    {
        s.OnButtonClick(0);
    }
    else if (e.code == "ArrowDown")
    {
        if ($("input[GridProductIndex=0]"))
            $("input[GridProductIndex=0]").focus();
    }
}

var iindexprod = 0;
function GridProductListkeydown(e) {

    if (e.code == "Enter" || e.code == "NumpadEnter") {
        if ($("#txtGridProductName").val().trim() != '') {
            GridProductlist($("#txtGridProductName").val().trim());
        }
        else {
            var txt = "<table  class='dynamicPopupTbl' width=\"100%\"><tr class=\"HeaderStyle\"><th class='hide'>id</th><th>Product Name</th><th>Description</th><th>HSN/SAC</th></tr><table>";
            $("#txtGridProductName").val("");
            document.getElementById("GridProductTable").innerHTML = txt;
        }
    }
    else if (e.code == "ArrowDown") {
        if ($("input[GridProductIndex=" + 0 + "]")) {
            $("input[GridProductIndex=" + 0 + "]").focus();
            iindexprod++;
        }
        if ($("input[NonIProductIndex=" + 0 + "]")) {
            $("input[NonIProductIndex=" + 0 + "]").focus();
            iindexprod++;
        }
    }
}

function GridProductlist(SearchKey) {
    //debugger;

    if (SearchKey != "") {
        gridproductlist = 1;
        var OtherDetails = {}
        OtherDetails.SearchKey = SearchKey;
      
        var HeaderCaption = [];
        // HeaderCaption.push("Product ID");
        HeaderCaption.push("Product Name");
        HeaderCaption.push("Description");
        HeaderCaption.push("HSN/SAC");
        

        //callonServerScroll("../Models/PMS_WebServiceList.asmx/GetProductDetailsList", OtherDetails, "GridProductTable", HeaderCaption, "GridProductIndex", "SetGridProduct");
        callonServer("../CRMActivity/GetcrmProducts", OtherDetails, "GridProductTable", HeaderCaption, "GridProductIndex", "SetGridProduct");
    }

}
var gridproductlist = 0;
function SetGridProduct(Id, Name, e) {
    // debugger;
    gridproductlist = 0;
    var ProductID = Id;
    var ProductName = Name;

    if (ProductID != "") {

    
        var data = ProductID.split('|');
        ProductID = data[0];

  


        $("#txtProductdescription").val(data[6]);
        btnProduct.SetText(ProductName);
        cProductQty.SetText("0.00");
   
  
        cProductPrice.SetText(data[3]);
   
        cProductAmount.SetText("0.00");
  
        $('#GridProductlistModel').modal('hide');
   
        $("#hdnProdProductID").val(ProductID);
   
    
    }
}


function btnAdd_ProductClick() {

    var ProductQty = cProductQty.GetText();

    if (btnProduct.GetValue() == null) {
        jAlert("Please select Product");
        return;
    }
    else {


        var data = {
            ProductName: btnProduct.GetValue(),
            ProductId: $("#hdnProdProductID").val(),
            ProductQty: cProductQty.GetText(),
            Price: cProductPrice.GetText(),
            Amount: cProductAmount.GetText(),
            Guids: $("#GuiIDS").val(),
            ProductDetailsID: $("#ProductDetailsID").val(),
            WarrentyStartdate: cWarrStartdate.GetText(),//cWarrStartdate.GetDate(),
            WarrentyEnddate: cWarrEnddate.GetText()//GetDate()          

        }

      


        $.ajax({
            type: "POST",
            //url: "@Url.Action("AddProduct", "ServiceContact")",
          url: "../ServiceContact/AddProduct",
           // data: "{prod:" + JSON.stringify(data) + "}",
        data:JSON.stringify({ prod: data }),
            async: false,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
            if (response != null) {
                GetServiceProductEntryList.Refresh();
                GetServiceProductEntryList.Refresh();

                btnProduct.SetValue('');
                $("#hdnProdProductID").val('');
                cProductQty.SetText("0.00");
                cProductPrice.SetText('0.00');
                cProductAmount.SetText('0.00');
                $("#GuiIDS").val('');
                $("#ProductDetailsID").val(0);
                cWarrStartdate.Clear();
                cWarrEnddate.Clear();
            }
        }
    });


}
}

function ServiceGridSetAmount(s, e) {
    var Price = cProductPrice.GetText();
    var Qty = cProductQty.GetText();
    if (Price != "" && Qty != "") {

        var amt = parseFloat((parseFloat(Qty).toFixed(2)) * (parseFloat(Price).toFixed(2))).toFixed(2);
        cProductAmount.SetText(amt);
    }
}

function DeleteData(values) {
    $.ajax({
        type: "POST",
        url: "../ServiceContact/DeleteData",
        data: { HiddenID: values },
    success: function (response) {
        if (response != null) {
            GetServiceProductEntryList.Refresh();
            GetServiceProductEntryList.Refresh();
        }
    }
});
}


function DeleteClick(Id)
{
    jConfirm('Confirm delete?', 'Confirmation Dialog', function (r) {
        if (r == true) {
            $.ajax({
                type: "POST",
                url: "../ServiceContact/DeleteServiceContact",
                data: JSON.stringify({ Id: Id }),
                async: false,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    jAlert("Data Delete Successfully.");
                    gvPaging.Refresh();
                    gvPaging.Refresh();
                },
                error: function (response) {
                    jAlert("Please try again later.");

                }
            });
        }
    });

  
}

function ViewClick(id) {

    $("#hdnLead_Id").val(id);
    var obj = {};
    obj.ServiceCnt_Id = id;
    LoadingPanel.Show();
    $.ajax({
        type: "POST",
        url: "../ServiceContact/EditServiceContact",
        data: JSON.stringify(obj),
        async: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            LoadingPanel.Hide();

            var link = document.getElementById('btnsave');
            link.style.display = 'none';
            var ProductAdd = document.getElementById('ProductAdd');
            ProductAdd.style.display = 'none';

            $("#hdnActionType").val('Update');
            $("#hdnLead_Id").val(response.ServiceCnt_Id);
            $("#OwnerID").val(response.OwnerID);
            $("#AssignedID").val(response.AssignedID);
            if (response.RenewalDate != null) {
                var date = new Date(response.RenewalDate);
                cRenewalDate_Close.SetDate(date);
            }
            if (response.RenewalStartDate != null) {
                var RenewalStartDate = new Date(response.RenewalStartDate);
                cRenewalStartDate_Close.SetDate(RenewalStartDate);
            }
            if (response.RenewalEndDate != null) {
                var RenewalEndDate = new Date(response.RenewalEndDate);
                cRenewalEndDate_Close.SetDate(RenewalEndDate);
            }
            $("#StatusId").val(response.ServiceStatusId);
            $("#SerViceName").val(response.ServiceName);
            $("#UniqueId").val(response.UniqueId);
            CustomerTxt.SetText(response.Customer);
            $("#CustomerId").val(response.Customer_ID);
            $("#servicedesc").val(response.Servicedesc);
            cServiceAmount.SetValue(response.ServiceAmount);
            cProdServCost.SetValue(response.ProdServCost);
            cAdditionalCost.SetValue(response.AdditionalCost);
            gridLookupcontact.GetGridView().SelectRowsByKey(response.cntids);


            GetServiceProductEntryList.Refresh();
            GetServiceProductEntryList.Refresh();
            
            $('#SummaryTabLink').addClass("active");
            $('#DetailsTabLink').removeClass("active");
            $('#tab_a').addClass("active");
            $('#tab_b').removeClass("active");
            $("#addFservice").modal('show');
        },
        error: function (response) {
            jAlert("Can Not View");
            LoadingPanel.Hide();
        }
    });


}

function EditClick(id) {
    
    $("#hdnLead_Id").val(id);
    var obj = {};
    obj.ServiceCnt_Id = id;
    LoadingPanel.Show();
    $.ajax({
        type: "POST",
        url: "../ServiceContact/EditServiceContact",
        data: JSON.stringify(obj),
        async: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            LoadingPanel.Hide();
            
            var link = document.getElementById('btnsave');
            link.style.display = 'inline-block';
            var ProductAdd = document.getElementById('ProductAdd');
            ProductAdd.style.display = 'inline-block';

            $("#hdnActionType").val('Update');
            $("#hdnLead_Id").val(response.ServiceCnt_Id);
            $("#OwnerID").val(response.OwnerID);
            $("#AssignedID").val(response.AssignedID);
            if (response.RenewalDate != null) {
                var date = new Date(response.RenewalDate);
                cRenewalDate_Close.SetDate(date);
            }
            if (response.RenewalStartDate != null) {
                var RenewalStartDate = new Date(response.RenewalStartDate);
                cRenewalStartDate_Close.SetDate(RenewalStartDate);
            }
            if (response.RenewalEndDate != null) {
                var RenewalEndDate = new Date(response.RenewalEndDate);
                cRenewalEndDate_Close.SetDate(RenewalEndDate);
            }
            $("#StatusId").val(response.ServiceStatusId);
            $("#SerViceName").val(response.ServiceName);
            $("#UniqueId").val(response.UniqueId);
            CustomerTxt.SetText(response.Customer);
            $("#CustomerId").val(response.Customer_ID);
            $("#servicedesc").val(response.Servicedesc);
            cServiceAmount.SetValue(response.ServiceAmount);
            cProdServCost.SetValue(response.ProdServCost);
            cAdditionalCost.SetValue(response.AdditionalCost);
            gridLookupcontact.GetGridView().SelectRowsByKey(response.cntids);
           

            GetServiceProductEntryList.Refresh();
            GetServiceProductEntryList.Refresh();
            $('#SummaryTabLink').addClass("active");
            $('#DetailsTabLink').removeClass("active");
            $('#tab_a').addClass("active");
            $('#tab_b').removeClass("active");

            $("#addFservice").modal('show');
        },
        error: function (response) {
            jAlert("Can not Edit");
            LoadingPanel.Hide();
        }
    });


}

function CancelServiceContact()
{
    CloseHeadModal();
}

function CloseHeadModal()
{
    $("#SerViceName").val('');
    cRenewalDate_Close.Clear();
    $("#StatusId").val("0");
    $("#UniqueId").val('');
    $("#OwnerID").val('');
    $("#AssignedID").val('');
    $("#UniqueId").val('');
    CustomerTxt.SetText("");
    $("#CustomerId").val('');
    gridLookupcontact.SetValue(null);
    cRenewalStartDate_Close.Clear();
    cRenewalEndDate_Close.Clear();
    $("#servicedesc").val('');
    cServiceAmount.SetValue(0.00);
    cProdServCost.SetValue(0.00);
    cAdditionalCost.SetValue(0.00);
    $("#hdnLead_Id").val("");
    var link = document.getElementById('btnsave');
    link.style.display = 'inline-block';
    var ProductAdd = document.getElementById('ProductAdd');
    ProductAdd.style.display = 'inline-block';
    //GetServiceProductEntryList.Refresh();
    //GetServiceProductEntryList.Refresh();
    $.ajax({
        type: "POST",
        url: "../ServiceContact/AddModeServiceContact",
        data: JSON.stringify(),
        async: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {

        },
        error: function (response) {

        }
    });
    GetServiceProductEntryList.Refresh();
    GetServiceProductEntryList.Refresh();
}

function AddServiceContact() {
    $("#SerViceName").val('');
    cRenewalDate_Close.Clear();
    $("#StatusId").val("0");
    $("#OwnerID").val($("#hdnOwnerAssignID").val());
    $("#AssignedID").val($("#hdnOwnerAssignID").val());
    $("#UniqueId").val('');
    $("#UniqueId").val('');
    CustomerTxt.SetText("");
    $("#CustomerId").val('');
    gridLookupcontact.SetValue(null);
    cRenewalStartDate_Close.Clear();
    cRenewalEndDate_Close.Clear();
    $("#servicedesc").val('');
    cServiceAmount.SetValue(0.00);
    cProdServCost.SetValue(0.00);
    cAdditionalCost.SetValue(0.00);
    $("#hdnLead_Id").val("");
    var link = document.getElementById('btnsave');
    link.style.display = 'inline-block';
    var ProductAdd = document.getElementById('ProductAdd');
    ProductAdd.style.display = 'inline-block';
    $('#SummaryTabLink').addClass("active");
    $('#DetailsTabLink').removeClass("active");
    $('#tab_a').addClass("active");
    $('#tab_b').removeClass("active");
    //GetServiceProductEntryList.Refresh();
    //GetServiceProductEntryList.Refresh();
    $.ajax({
        type: "POST",
        url: "../ServiceContact/AddModeServiceContact",
        data: JSON.stringify(),
        async: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {

        },
        error: function (response) {

        }
    });
    GetServiceProductEntryList.Refresh();
    GetServiceProductEntryList.Refresh();

}


$(function () {
    $('#ddlExport').on('change', function () {
        if ($("#ddlExport option:selected").index() > 0) {
            var selectedValue = $(this).val();
            $('#ddlExport').prop("selectedIndex", 0);

            var url = $('#hdnExportLink').val();
            window.location.href = url.replace("_type_", selectedValue);
        }
    });
});

function ShowAllClick() {
    gvPaging.Refresh();
    gvPaging.Refresh();

}


function ContractStartCallback(s, e) {
    //debugger;
    e.customArgs["Customer_ID"] = $("#CustomerId").val();
    //e.customArgs["ContractNo"] = '@ViewBag.ContractNo';
    //if ('@ViewBag.Unit' != "") {
    //    e.customArgs["Unit"] = '@ViewBag.Unit';
    //}
    //else {
    //    e.customArgs["Unit"] = $("#ddlBankBranch").val();
    //}
    //e.customArgs["ProjectID"] = ProjectGridLookup.GetSelectedKeyFieldValues();
}