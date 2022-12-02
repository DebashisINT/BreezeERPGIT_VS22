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
    if (e.htmlEvent.key == "Enter" || e.code == "NumpadEnter") {
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
            url: "../FieldService/PopulateSummaryContactGrid",
            data: JSON.stringify(obj),
            async: false,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {

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
    if (e.htmlEvent.key == "Enter") {
        s.OnButtonClick(0);
    }
    else if (e.code == "ArrowDown") {
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




       
        btnProduct.SetText(ProductName);
        cProductQty.SetText("0.00");


        cProductPrice.SetText(data[3]);

        cProductAmount.SetText("0.00");

        $('#GridProductlistModel').modal('hide');

        $("#hdnProdProductID").val(ProductID);


    }
}




function ContractStartCallback(s, e) {
  
    e.customArgs["Customer_ID"] = $("#CustomerId").val();
}

function ShowAllClick() {
    gvPaging.Refresh();
    gvPaging.Refresh();

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
            url: "../FieldService/AddProduct",
            // data: "{prod:" + JSON.stringify(data) + "}",
            data: JSON.stringify({ prod: data }),
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

function ValidationServicecontact() {
    var isvalid = false;
    if (CustomerTxt.GetText() == "") {
        isvalid = false;
        jAlert("Account/Customer is Mandatory"); return;
    }
    else if ($("#SerViceName").val() == "") {
        isvalid = false;
        jAlert("Service Name is Mandatory"); return;
    }
    else {
        isvalid = true;
    }

    return isvalid;
}


function ServiceGridSetAmount(s, e) {
    var Price = cProductPrice.GetText();
    var Qty = cProductQty.GetText();
    if (Price != "" && Qty != "") {

        var amt = parseFloat((parseFloat(Qty).toFixed(2)) * (parseFloat(Price).toFixed(2))).toFixed(2);
        cProductAmount.SetText(amt);
    }
}

function CancelServiceContact() {
    CloseHeadModal();
}

function AddServiceContact()
{
    $("#OwnerID").val($("#hdnOwnerAssignID").val());
    $("#AssignedID").val($("#hdnOwnerAssignID").val());
    $("#hdnLead_Id").val('');
    $("#hdnActionType").val('');
    $("#StatusId").val('');
    $("#ServiceTypeId").val('');
    $("#SerViceName").val('');
    $("#UniqueId").val('');
    $("#TechnicianId").val('');
    $("#ProjectId").val('');
    CustomerTxt.SetText('');
    $("#CustomerId").val('');
    cServiceDate.Clear();
    cCallAttendDate.Clear();
    cCloseDate.Clear();
    $("#FaultDetails").val('');
    cBalance.SetValue(0.00);
    $("#Remarks").val('');
    $("#addFservice").modal('hide');
    $("#ddlJobCode").val('0');
    $("#CASE_ID").val('0');
    $("#ddlCasesCode").val('0');
    $("#Casesname").val('');
    gvPaging.Refresh();
    gvPaging.Refresh();

    gridLookupcontact.SetValue(null);
    var link = document.getElementById('btnsave');
    link.style.display = 'inline-block';
    var ProductAdd = document.getElementById('ProductAdd');
    ProductAdd.style.display = 'inline-block';
    //GetServiceProductEntryList.Refresh();
    //GetServiceProductEntryList.Refresh();
    $.ajax({
        type: "POST",
        url: "../FieldService/AddModeServiceContact",
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

    $('#SummaryTabLink').addClass("active");
    $('#tab_a').addClass("active");
    $('#DetailsTabLink').removeClass("active");
    $('#tab_b').removeClass("active");
    ProjectCodeChange();
    CasesCodeChange
}

function CloseHeadModal() {
    $("#OwnerID").val('');
    $("#AssignedID").val('');
    $("#hdnLead_Id").val('');
    $("#hdnActionType").val('');
    $("#StatusId").val('');
    $("#ServiceTypeId").val('');
    $("#SerViceName").val('');
    $("#UniqueId").val('');
    $("#TechnicianId").val('');
    $("#ProjectId").val('');
    CustomerTxt.SetText('');
    $("#CustomerId").val('');
    cServiceDate.Clear();
    cCallAttendDate.Clear();
    cCloseDate.Clear();
    $("#FaultDetails").val('');
    cBalance.SetValue(0.00);
    $("#Remarks").val('');
    $("#addFservice").modal('hide');
    $("#ddlJobCode").val('0');
    $("#CASE_ID").val('0');
    $("#ddlCasesCode").val('0');
    $("#Casesname").val('');
    gvPaging.Refresh();
    gvPaging.Refresh();

    gridLookupcontact.SetValue(null);
    var link = document.getElementById('btnsave');
    link.style.display = 'inline-block';
    var ProductAdd = document.getElementById('ProductAdd');
    ProductAdd.style.display = 'inline-block';
    //GetServiceProductEntryList.Refresh();
    //GetServiceProductEntryList.Refresh();
    $.ajax({
        type: "POST",
        url: "../FieldService/AddModeServiceContact",
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

    $('#SummaryTabLink').addClass("active");
    $('#tab_a').addClass("active");
    $('#DetailsTabLink').removeClass("active");
    $('#tab_b').removeClass("active");

    ProjectCodeChange();
    CasesCodeChange();
}


function SaveServiceContact() {
    if (ValidationServicecontact()) {
        var obj = {};

        $("#hdnLead_Id").val() == ""

        if ($("#hdnLead_Id").val() == "" || $("#hdnLead_Id").val() == "0") {
            obj.Action = "Add";
            $("#hdnActionType").val('Add');
            obj.ServiceCnt_Id = 0;
        }
        else {
            obj.Action = "Update";
            $("#hdnActionType").val('Update');
            obj.ServiceCnt_Id = $("#hdnLead_Id").val();
        }

       
        obj.OwnerID = $("#OwnerID").val();
        obj.AssignedID=$("#AssignedID").val();
        obj.ServiceStatusId = $("#StatusId").val();
        obj.ServiceName = $("#SerViceName").val();
        obj.UniqueId = $("#UniqueId").val();
        obj.Customer_ID = $('#CustomerId').val();
        var contacts = gridLookupcontact.GetSelectedKeyFieldValues();
        
        obj.crmcontacts_id = contacts.join();
        obj.Servicetype=$("#ServiceTypeId").val();
        obj.ServiceDate = cServiceDate.GetText();//GetDate();
        obj.CallAttendDate = cCallAttendDate.GetText();//GetDate();
        obj.CloseDate = cCloseDate.GetText();//GetDate();
        obj.TechnicianId = $("#TechnicianId").val();
        obj.JobId = $("#ProjectId").val();
        obj.JobId = $("#ddlJobCode").val();
        obj.Fault_Description = $("#FaultDetails").val();
        obj.Balance = cBalance.GetValue();
        obj.Remarks = $("#Remarks").val();
        obj.CASE_ID = $("#ddlCasesCode").val();
        obj.CaseName = $("#Casesname").val();
        LoadingPanel.Show();
        $.ajax({
            type: "POST",
            url: "/FieldService/SaveServiceContact",
            data: JSON.stringify(obj),
            async: false,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                jAlert(response);
                $("#OwnerID").val('');
                $("#AssignedID").val('');
                $("#hdnLead_Id").val('');
                $("#hdnActionType").val('');
                $("#StatusId").val('');
                $("#ServiceTypeId").val('');
                $("#SerViceName").val('');
                $("#UniqueId").val('');
                $("#TechnicianId").val('');
                $("#ProjectId").val('');
                $("#ddlJobCode").val('');
                $("#CASE_ID").val('0');
                $("#ddlCasesCode").val('0');
                $("#Casesname").val('');
                CustomerTxt.SetText('');
                $("#CustomerId").val('');
                cServiceDate.Clear();
                cCallAttendDate.Clear();
                cCloseDate.Clear();
                $("#FaultDetails").val('');
                cBalance.SetValue(0.00);
                $("#Remarks").val('');
                $("#addFservice").modal('hide');
                GetServiceProductEntryList.Refresh();
                GetServiceProductEntryList.Refresh();
                gvPaging.Refresh();
                gvPaging.Refresh();
                gridLookupcontact.SetValue(null);
                CloseHeadModal();
               
                LoadingPanel.Hide();
            },
            error: function (response) {
                jAlert("Please try again later");
                LoadingPanel.Hide();
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
function CasesCodeChange() {

    var CASE_ID = $("#ddlCasesCode").val();
    if (CASE_ID == "0") {
        $("#Casesname").val('');
    }

    if (CASE_ID != null && CASE_ID != "" && CASE_ID != "0" && CASE_ID != undefined) {
        $.ajax({
            type: "POST",
            url: "../FieldService/GetCasesName",
            data: JSON.stringify({ CASE_ID: CASE_ID }),
            async: false,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                var d = response;
                if (d != null && d != "") {
                    $("#Casesname").val(d);
                }
            }

        });
    }
}

function ProjectCodeChange()
{
    var ProjectId = $("#ddlJobCode").val();
    if (ProjectId == "0")
    {
        $("#JobName").val('');
    }

    if (ProjectId != null && ProjectId != "" && ProjectId != "0" && ProjectId != undefined) {
        $.ajax({
            type: "POST",
            url: "../FieldService/GetProjectName",
            data: JSON.stringify({ ProjectId: ProjectId }),
            async: false,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                var d = response;
                if(d !=null  && d !="")
                {
                    $("#JobName").val(d);
                }
            }

        });
    }
}

function DeleteData(values) {
    $.ajax({
        type: "POST",
        url: "../FieldService/DeleteData",
        data: { HiddenID: values },
        success: function (response) {
            if (response != null) {
                GetServiceProductEntryList.Refresh();
                GetServiceProductEntryList.Refresh();
            }
        }
    });
}
function DeleteClick(Id) {
    jConfirm('Confirm delete?', 'Confirmation Dialog', function (r) {
        if (r == true) {
            $.ajax({
                type: "POST",
                url: "../FieldService/DeleteServiceContact",
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
        url: "../FieldService/EditServiceContact",
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
            if (response.ServiceDate != null) {
                var date = new Date(response.ServiceDate);
                cServiceDate.SetDate(date);
            }
            if (response.CallAttendDate != null) {
                var RenewalStartDate = new Date(response.CallAttendDate);
                cCallAttendDate.SetDate(RenewalStartDate);
            }
            if (response.CloseDate != null) {
                var RenewalEndDate = new Date(response.CloseDate);
                cCloseDate.SetDate(RenewalEndDate);
            }
            $("#ServiceTypeId").val(response.Servicetype);
            $("#StatusId").val(response.ServiceStatusId);
            $("#SerViceName").val(response.ServiceName);
            $("#UniqueId").val(response.UniqueId);
            CustomerTxt.SetText(response.Customer);
            $("#CustomerId").val(response.Customer_ID);
            $("#TechnicianId").val(response.TechnicianId);
            $("#ProjectId").val(response.JobId);
            $("#ddlJobCode").val(response.JobId);
            cBalance.SetValue(response.Balance);
            $("#FaultDetails").val(response.Fault_Description); 
            $("#Remarks").val(response.Remarks);

            $("#CASE_ID").val(response.CASE_ID);
            $("#ddlCasesCode").val(response.CASE_ID);
            $("#Casesname").val(response.CaseName);

            gridLookupcontact.GetGridView().SelectRowsByKey(response.cntids);


            GetServiceProductEntryList.Refresh();
            GetServiceProductEntryList.Refresh();

            $('#SummaryTabLink').addClass("active");
            $('#tab_a').addClass("active");
            $('#DetailsTabLink').removeClass("active");
            $('#tab_b').removeClass("active");
            $("#addFservice").modal('show');
        },
        error: function (response) {
            jAlert("Can Not View");
            LoadingPanel.Hide();
        }
    });

    ProjectCodeChange();
    CasesCodeChange();
}

function EditClick(id) {

    $("#hdnLead_Id").val(id);
    var obj = {};
    obj.ServiceCnt_Id = id;
    LoadingPanel.Show();
    $.ajax({
        type: "POST",
        url: "../FieldService/EditServiceContact",
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
            if (response.ServiceDate != null) {
                var date = new Date(response.ServiceDate);
                cServiceDate.SetDate(date);
            }
            if (response.CallAttendDate != null) {
                var RenewalStartDate = new Date(response.CallAttendDate);
                cCallAttendDate.SetDate(RenewalStartDate);
            }
            if (response.CloseDate != null) {
                var RenewalEndDate = new Date(response.CloseDate);
                cCloseDate.SetDate(RenewalEndDate);
            }
            $("#ServiceTypeId").val(response.Servicetype);
            $("#StatusId").val(response.ServiceStatusId);
            $("#SerViceName").val(response.ServiceName);
            $("#UniqueId").val(response.UniqueId);
            CustomerTxt.SetText(response.Customer);
            $("#CustomerId").val(response.Customer_ID);
            $("#TechnicianId").val(response.TechnicianId);
            $("#ProjectId").val(response.JobId);
            $("#ddlJobCode").val(response.JobId);
            cBalance.SetValue(response.Balance);
            $("#FaultDetails").val(response.Fault_Description);
            $("#Remarks").val(response.Remarks);

            $("#CASE_ID").val(response.CASE_ID);
            $("#ddlCasesCode").val(response.CASE_ID);
            $("#Casesname").val(response.CaseName);

            gridLookupcontact.GetGridView().SelectRowsByKey(response.cntids);



            GetServiceProductEntryList.Refresh();
            GetServiceProductEntryList.Refresh();

            $('#SummaryTabLink').addClass("active");
            $('#tab_a').addClass("active");
            $('#DetailsTabLink').removeClass("active");
            $('#tab_b').removeClass("active");

            $("#addFservice").modal('show');
        },
        error: function (response) {
            jAlert("Can not Edit");
            LoadingPanel.Hide();
        }
    });

    ProjectCodeChange();
    CasesCodeChange();
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