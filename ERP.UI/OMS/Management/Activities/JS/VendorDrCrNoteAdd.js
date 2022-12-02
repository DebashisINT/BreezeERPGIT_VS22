
var CanCallback = true;
function AllControlInitilize() {
    //if (CanCallback) {
    //    CanCallback = false;
    //    if ($('#hdnMode').val() == "Entry") {
    //        SetNumberingSchemeDataSource();
    //    }
    //}
}

function SetLostFocusonDemand(e) {

    if ($("#ddlNoteType").val() == "Dr") {
        if ((new Date($("#hdnLockFromDate").val()) <= tDate.GetDate()) && (tDate.GetDate() <= new Date($("#hdnLockToDate").val()))) {
            jAlert("Vendor Debit Note DATA is Freezed between   " + $("#hdnLockFromDateFreeze").val() + " to " + $("#hdnLockToDateFreeze").val() + " for Add.");
        }
    }
    else if ($("#ddlNoteType").val() == "Cr") {
        if ((new Date($("#hdnLockFromDateCon").val()) <= tDate.GetDate()) && (tDate.GetDate() <= new Date($("#hdnLockToDateCon").val()))) {
            jAlert("Vendor Cedit Note DATA is Freezed between   " + $("#hdnLockFromDateConFreeze").val() + " to " + $("#hdnLockToDateConFreeze").val() + " for Add.");
        }
    }
}

function SetNumberingSchemeDataSource() {
    var val = document.getElementById("ddlNoteType").value;
    $.ajax({
        type: "POST",
        url: "VendorDrCrNoteAdd.aspx/GetScheme",
        data: "{sel_type_id:\"" + val + "\"}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (r) {
            //var CmbScheme = $("[id*=CmbScheme]");
            //CmbScheme.empty();
            //CmbScheme.empty().append('<option selected="selected" value="0">Select</option>');
            //$.each(r.d, function () {
            //    CmbScheme.append($("<option></option>").val(this['Value']).html(this['Text']));
            //});
            SetDataSourceOnComboBox(cCmbScheme, r.d);
        }
    });
}
function SetDataSourceOnComboBox(ControlObject, Source) {
    ControlObject.ClearItems();
    for (var count = 0; count < Source.length; count++) {
        ControlObject.AddItem(Source[count].Text, Source[count].Value);
    }
    ControlObject.SetSelectedIndex(0);
}

//Code for UDF Control 
function OpenUdf() {
    if (document.getElementById('IsUdfpresent').value == '0') {
        jAlert("UDF not define.");
    }
    else {
        var keyVal = document.getElementById('Keyval_internalId').value;
        var url = '/OMS/management/Master/frm_BranchUdfPopUp.aspx?Type=VNOTE&&KeyVal_InternalID=' + keyVal;
        popup.SetContentUrl(url);
        popup.Show();
    }
    return true;
}
// End Udf Code

function GetVisibleIndex(s, e) {
    globalRowIndex = e.visibleIndex;

}
function deleteTaxEndCallBack() {

}
//function NoteTypeClick(s, e) {
//    $('#TypeModel').modal('show');

//}
//function NoteTypeKeyDown(s, e) {
//    if (e.htmlEvent.key == "Enter") {
//        $('#TypeModel').modal('show');
//    }
//}
//function drKeyDown(e) {
//    if (e.code == "ArrowUp") {
//        $("input[id=txtcrKey]").focus();
//    }
//    else if (e.code == "Enter" || e.code == "NumpadEnter") {
//        $('#TypeModel').modal('hide');
//        cddlNoteType.SetText("Debit Note");
//        GetObjectID('hdnNoteType').value = "Dr";
//        cCmbScheme.Focus();             

//    }
//}
//function crKeyDown(e) {
//    if (e.code == "ArrowDown") {
//        $("input[id=txtdrKey]").focus();
//    }
//    else if (e.code == "Enter" || e.code == "NumpadEnter") {
//        $('#TypeModel').modal('hide');
//        cddlNoteType.SetText("Credit Note");
//        GetObjectID('hdnNoteType').value = "Cr";
//        cCmbScheme.Focus();             

//    }
//}

//function DrNoteClick(e)
//{
//    $('#TypeModel').modal('hide');
//    cddlNoteType.SetText("Debit Note");
//    GetObjectID('hdnNoteType').value = "Dr";
//    cCmbScheme.Focus();
//}
//function CrNoteClick(e) {
//    $('#TypeModel').modal('hide');
//    cddlNoteType.SetText("Credit Note");
//    GetObjectID('hdnNoteType').value = "Cr";
//    cCmbScheme.Focus();
//}

//function searchElementGetFocus(e) {
//    e.target.parentElement.parentElement.className = "focusrow";

//}

//function searchElementlostFocus(e) {
//    e.target.parentElement.parentElement.className = "";
//}



function ddlNoteType_ValueChange() {
    var val = document.getElementById("ddlNoteType").value;
    // var val = GetObjectID('hdnNoteType').value;
    document.getElementById('txtBillNo').disabled = true;
    document.getElementById('txtBillNo').value = "";
    GetInvoiceDetails();

    if (val == "Dr") {
        document.getElementById('div_InvoiceNo').style.display = 'block';
        document.getElementById('div_InvoiceDate').style.display = 'block';
    }
    else {
        document.getElementById('div_InvoiceNo').style.display = 'none';
        document.getElementById('div_InvoiceDate').style.display = 'none';
    }
    SetNumberingSchemeDataSource();
    //$.ajax({
    //    type: "POST",
    //    url: "VendorDrCrNoteAdd.aspx/GetScheme",
    //    data: "{sel_type_id:\"" + val + "\"}",
    //    contentType: "application/json; charset=utf-8",
    //    dataType: "json",
    //    success: function (r) {
    //        var CmbScheme = $("[id*=CmbScheme]");
    //        CmbScheme.empty().append('<option selected="selected" value="0">Select</option>');
    //        $.each(r.d, function () {
    //            CmbScheme.append($("<option></option>").val(this['Value']).html(this['Text']));
    //        });
    //    }
    //});
    // document.getElementById('ddlNoteType').disabled = true;
}


function IfVendorGstInIsBlank() {
    //State will be load here

}





function CmbScheme_ValueChange() {

    var val = cCmbScheme.GetValue();
    // var val = GetObjectID('hdnSchemaID').value;

    $("#MandatoryBillNo").hide();

    if (val != " ") {

        //var NoSchemeTypedtl = GetObjectID('hdnSchemaID').value;
        var NoSchemeTypedtl = cCmbScheme.GetValue();
        var schemeID = NoSchemeTypedtl.toString().split('~')[0];
        $('#hdnSchemaID').val(schemeID);
        var schemetype = NoSchemeTypedtl.toString().split('~')[1];
        var schemelength = NoSchemeTypedtl.toString().split('~')[2];
        var branchID = (NoSchemeTypedtl.toString().split('~')[3] != null) ? NoSchemeTypedtl.toString().split('~')[3] : "";
        var fromdate = NoSchemeTypedtl.toString().split('~')[4];
        var todate = NoSchemeTypedtl.toString().split('~')[5];
        var dt = new Date();
        tDate.SetDate(dt);
        if (dt < new Date(fromdate)) {
            tDate.SetDate(new Date(fromdate));
        }
        if (dt > new Date(todate)) {
            tDate.SetDate(new Date(todate));
        }

        tDate.SetMinDate(new Date(fromdate));
        tDate.SetMaxDate(new Date(todate));

        if (branchID != "") document.getElementById('ddlBranch').value = branchID;
        GetInvoiceDetails();

        if (schemetype == '0') {
            $('#hdnSchemaType').val('0');
            document.getElementById('txtBillNo').disabled = false;
            document.getElementById('txtBillNo').value = "";
            document.getElementById("txtBillNo").focus();
        }
        else if (schemetype == '1') {
            $('#hdnSchemaType').val('1');
            document.getElementById('txtBillNo').disabled = true;
            document.getElementById('txtBillNo').value = "Auto";
            tDate.Focus();
        }
        else if (schemetype == '2') {
            $('#hdnSchemaType').val('2');
            document.getElementById('txtBillNo').disabled = true;
            document.getElementById('txtBillNo').value = "Datewise";
        }
        else {
            document.getElementById('txtBillNo').disabled = true;
            document.getElementById('txtBillNo').value = "";
        }
    }
    else {
        document.getElementById('txtBillNo').disabled = true;
        document.getElementById('txtBillNo').value = "";
    }

    //clookup_Project.gridView.Refresh();
}
function txtBillNo_TextChanged() {
    var VoucherNo = document.getElementById("txtBillNo").value;
    var type = $('#hdnMode').val();

    if (VoucherNo != "") {
        $("#MandatoryBillNo").hide();
    }

    $.ajax({
        type: "POST",
        url: "VendorDrCrNoteAdd.aspx/CheckUniqueName",
        data: JSON.stringify({ VoucherNo: VoucherNo, Type: type }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var data = msg.d;

            if (data == true) {
                $("#duplicateMandatoryBillNo").show();
                document.getElementById("txtBillNo").value = '';
                document.getElementById("txtBillNo").focus();
            }
            else {
                $("#duplicateMandatoryBillNo").hide();
            }
        }
    });
}

function ddlBranch_ChangeIndex() {
    GetInvoiceDetails();

    if (oldBranchdata != document.getElementById('ddlBranch').value) {

        if (accountingDataMin != null || accountingDataplus != null) {
            jConfirm('You have changed Branch. All the entries of ledger in this voucher to be reset to blank. \n You have to select and re-enter. Continue?', 'Confirmation Dialog', function (r) {

                if (r == true) {
                    deleteAllRows();
                   
                    grid.AddNewRow();
                    grid.GetEditor('SrlNo').SetValue('1');
                    CountryID.PerformCallback(document.getElementById('ddlBranch').value);
                    if (grid.GetVisibleRowsOnPage() == 1) {
                        grid.batchEditApi.StartEdit(-1, 1);
                    }
                } else {
                    document.getElementById('ddlBranch').value = oldBranchdata;
                }
            });
        }
        else {
            CountryID.PerformCallback(document.getElementById('ddlBranch').value);
        }
    }
}
function GetInvoiceDetails() {
    InvoiceID = "";
    var NoteType = (document.getElementById("ddlNoteType").value != null) ? document.getElementById("ddlNoteType").value : "";
    //var NoteType = GetObjectID('hdnNoteType').value;
    var CustomerID = GetObjectID('hdfLookupCustomer').value;
    if (CustomerID != "") {
        //LoadCustomerAddress(CustomerID, $('#ddlBranch').val(), 'VN');

        SetPurchaseBillingShippingAddress($('#ddlBranch').val());
        GetVendorGSTInFromBillShip(GetObjectID('hdfLookupCustomer').value);
        if ($('#hfBSAlertFlag').val() == "1") {
            jConfirm('Wish to View/Select Billing and Shipping details?', 'Confirmation Dialog', function (r) {
                if (r == true) {
                    page.tabs[0].SetEnabled(true);
                }
            });
        }
        else {
            page.tabs[0].SetEnabled(true);
        }
    }

    clblInvoiceNo.SetText('Ref. Purchase Invoice No.');

}

function NumberingSchemeClick(s, e) {
    $('#SchemeModel').modal('show');
    $('#SchemeTable').empty();
    var html = "<table border='1' width='100%' class='dynamicPopupTbl'><tr class='HeaderStyle'><th class='hide'>id</th><th>Schema Name</th></tr></table>"
    $('#SchemeTable').html(html);
}
function NumberingSchemeKeyDown(s, e) {
    if (e.htmlEvent.key == "Enter" || e.code == "NumpadEnter") {
        $('#SchemeModel').modal('show');
    }
}
function Schemekeydown(e) {
    var OtherDetails = {}
    if ($.trim($("#txtSchemeSearch").val()) == "" || $.trim($("#txtSchemeSearch").val()) == null) {
        return false;
    }
    OtherDetails.SearchKey = $("#txtSchemeSearch").val();
    var NoteType = (document.getElementById("ddlNoteType").value != null) ? document.getElementById("ddlNoteType").value : "";
    //OtherDetails.NoteType = GetObjectID('hdnNoteType').value;
    OtherDetails.NoteType = NoteType;
    if (e.code == "Enter" || e.code == "NumpadEnter") {
        var HeaderCaption = [];
        HeaderCaption.push("Scheme Name");

        callonServer("Services/Master.asmx/GetNumberingSchemaVDCNote", OtherDetails, "SchemeTable", HeaderCaption, "SchemeIndex", "SetScheme");
    }
    else if (e.code == "ArrowDown") {
        if ($("input[SchemeIndex=0]"))
            $("input[SchemeIndex=0]").focus();
    }
    else if (e.code == "Escape") {
        ctxtCustName.Focus();
        $('#SchemeModel').modal('hide');

    }
}
function SetScheme(Id, Name) {
    var SchemaID = Id;
    if (Id != "") {
        $('#SchemeModel').modal('hide');
        cCmbScheme.SetText(Name);
        GetObjectID('hdnSchemaID').value = SchemaID;

        CmbScheme_ValueChange();
    }
}

function GlobalBillingShippingEndCallBack() {
    //var NoteType = GetObjectID('hdnNoteType').value;
    var NoteType = (document.getElementById("ddlNoteType").value != null) ? document.getElementById("ddlNoteType").value : "";
    var CustomerID = GetObjectID('hdfLookupCustomer').value;
    if (NoteType != "" && CustomerID != "") {
        //cddlInvoice.PerformCallback("Cr" + '~' + CustomerID)
    }
}

function VendorButnClick(s, e) {
    $('#CustModel').modal('show');
    $('#CustomerTable').empty();
    var html = "<table border='1' width='100%' class='dynamicPopupTbl'><tr class='HeaderStyle'><th class='hide'>id</th><th>Vendor Name</th><th>Unique Id</th><th>GSTIN</th></tr></table>"
    $('#CustomerTable').html(html);
}
function VendorKeyDown(s, e) {
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
    if (e.code == "Enter" || e.code == "NumpadEnter") {
        var HeaderCaption = [];
        HeaderCaption.push("Vendor Name");
        HeaderCaption.push("Unique ID");
        HeaderCaption.push("GSTIN");
        callonServer("Services/Master.asmx/GetVendorWithOutBranchDrCr", OtherDetails, "CustomerTable", HeaderCaption, "customerIndex", "SetCustomer");
    }
    else if (e.code == "ArrowDown") {
        if ($("input[customerindex=0]"))
            $("input[customerindex=0]").focus();
    }
    else if (e.code == "Escape") {
        ctxtCustName.Focus();
        $('#CustModel').modal('hide');

    }
}

function AfterSaveBillingShipiing() {
    page.SetActiveTabIndex(0);
}


function SetCustomer(Id, Name, e) {
    var VendorID = Id;
    var _GSTIN = e.parentElement.cells[3].innerText;
    if (Id != "") {
        $('#CustModel').modal('hide');
        if (GetObjectID('hdfLookupCustomer').value != "" && GetObjectID('hdfLookupCustomer').value != null && GetObjectID('hdfLookupCustomer').value != 'undefined') {
            cdeleteTax.PerformCallback('DeleteAllTax');
        }

        $("#hdnProjectId").val('');
        ctxtProject.SetText("");
        ctxtCustName.SetText(Name);
        SetPurchaseBillingShippingAddress($('#ddlBranch').val());
        //LoadCustomerAddress(VendorID, $('#ddlBranch').val(), 'VN');
        page.tabs[0].SetEnabled(true);
        page.tabs[1].SetEnabled(true);
        GetObjectID('hdfLookupCustomer').value = VendorID;
        GetVendorGSTInFromBillShip(GetObjectID('hdfLookupCustomer').value);
        if ($('#hfBSAlertFlag').val() == "1") {
            jConfirm('Wish to View/Select Billing and Shipping details?', 'Confirmation Dialog', function (r) {
                if (r == true) {
                    page.SetActiveTabIndex(1);
                }
            });
        }
        //var note = GetObjectID('hdnNoteType').value
        var note = (document.getElementById("ddlNoteType").value != null) ? document.getElementById("ddlNoteType").value : "";
        if (note == "Dr") {
            ctxtPartyInvoice.Focus();
        }
        else if (note == "Cr") {
            ctxtPurchaseInvoiceNo.Focus();
        }
        if (_GSTIN != null && _GSTIN != "") {
            $("#divGSTIN").attr('style', 'display:block');
            document.getElementById('lblGSTIN').innerHTML = "Yes";
            //cddlInvoice.cpGSTN = null;
        }
        else {
            $("#divGSTIN").attr('style', 'display:block');
            document.getElementById('lblGSTIN').innerHTML = "No";
            //cddlInvoice.cpGSTN = null;
        }
    }
    //clookup_Project.gridView.Refresh();
}



$(document).ready(function () {
    $('#MainAccountModel').on('shown.bs.modal', function () {
        $('#txtMainAccountSearch').val("");
        $('#txtMainAccountSearch').focus();
    })
    $('#SubAccountModel').on('shown.bs.modal', function () {
        $('#txtSubAccountSearch').val("");
        $('#txtSubAccountSearch').focus();
    })
    $('#CustModel').on('shown.bs.modal', function () {
        $('#txtCustSearch').val("");
        $('#txtCustSearch').focus();
    })

    $('#TypeModel').on('shown.bs.modal', function () {
        $("input[id=txtcrKey]").focus();
    })
    $('#SchemeModel').on('shown.bs.modal', function () {
        $('#txtSchemeSearch').val("");
        $('#txtSchemeSearch').focus();
    })
    $('#PurchaseInvoiceModel').on('shown.bs.modal', function () {
        $('#txtPurchaseInvoiceSearch').val("");
        $('#txtPurchaseInvoiceSearch').focus();
    })
    $('#ProjectModel').on('shown.bs.modal', function () {
        $('#txtProjectSearch').val("");
        $('#txtProjectSearch').focus();
    })

    if ($('#hdnMode').val() == 'Edit') {
        GetVendorGSTInFromBillShip(GetObjectID('hdfLookupCustomer').value);
    }
});


function PurInvoiceNoButnClick(s, e) {
    $('#PurchaseInvoiceModel').modal('show');
    $('#PurchaseInvoiceTable').empty();
    var html = "<table border='1' width='100%' class='dynamicPopupTbl'><tr class='HeaderStyle'><th class='hide'>id</th><th>Invoice Number</th><th>Party Invoice No.</th><th>Party Invoice Date</th></tr></table>"
    $('#PurchaseInvoiceTable').html(html);
    if ($("#hdnProjectSelectInEntryModule").val() == "1") {
        var ProjectId = $("#hdnProjectId").val();
            //(clookup_Project.GetGridView().GetRowKey(clookup_Project.GetGridView().GetFocusedRowIndex()));
        if (ProjectId != null && ProjectId != "") {
            ShowInvoiceDetailsWithProject();
        }
        else {
            ShowInvoiceDetails();
        }

    }
    else {
        ShowInvoiceDetails();
    }
}
function DeleteProjectCode() {
    $("#hdnProjectId").val("");
    ctxtProject.SetText("");
    $("#ddlHierarchy").val(0);

    ctxtPurchaseInvoiceNo.SetText("");
    
    $("#hdnPurchaseInvoiceID").val("");

    deleteAllRows();
   // deleteAllRows();
    grid.AddNewRow();
    grid.GetEditor('SrlNo').SetValue('1');
}

function ProjectButnClick(s, e) {
    $('#ProjectModel').modal('show');

    $('#ProjectTable').empty();
    var html = "<table border='1' width='100%' class='dynamicPopupTbl'><tr class='HeaderStyle'><th class='hide'>Proj_Id</th><th>Project Code</th><th>Project Name</th><th>Customer</th><th>Hierarchy</th></tr></table>"
    $('#ProjectTable').html(html);
    ShowProjectDetails();
}
function ShowInvoiceDetails() {
    var OtherDetails = {}
    OtherDetails.CustVenID = GetObjectID('hdfLookupCustomer').value;
    OtherDetails.BranchList = $('#ddlBranch').val();
    OtherDetails.SearchKey = $("#txtPurchaseInvoiceSearch").val();

    var HeaderCaption = [];
    HeaderCaption.push("Invoice Number");
    HeaderCaption.push("Party Invoice No.");
    HeaderCaption.push("Party Invoice Date");

    callonServer("Services/Master.asmx/GetInvoiceDetails", OtherDetails, "PurchaseInvoiceTable", HeaderCaption, "PurchaseInvoiceIndex", "SetPurchaseInvoice");

}

function ShowInvoiceDetailsWithProject() {
    var OtherDetails = {}
    var ProjectId = $("#hdnProjectId").val();
        //(clookup_Project.GetGridView().GetRowKey(clookup_Project.GetGridView().GetFocusedRowIndex()));
    OtherDetails.CustVenID = GetObjectID('hdfLookupCustomer').value;
    OtherDetails.BranchList = $('#ddlBranch').val();
    OtherDetails.SearchKey = $("#txtPurchaseInvoiceSearch").val();
    OtherDetails.ProjectId = ProjectId;

    var HeaderCaption = [];
    HeaderCaption.push("Invoice Number");
    HeaderCaption.push("Party Invoice No.");
    HeaderCaption.push("Party Invoice Date");

    callonServer("Services/Master.asmx/GetInvoiceDetailsWithProject", OtherDetails, "PurchaseInvoiceTable", HeaderCaption, "PurchaseInvoiceIndex", "SetPurchaseInvoice");

}

function ShowProjectDetails() {
    var OtherDetails = {}
    //OtherDetails.CustVenID = GetObjectID('hdfLookupCustomer').value;
    OtherDetails.BranchList = $('#ddlBranch').val();
    OtherDetails.SearchKey = $("#txtProjectSearch").val();

    var HeaderCaption = [];
    HeaderCaption.push("Project Code");
    HeaderCaption.push("Project Name");
    HeaderCaption.push("Customer");
    HeaderCaption.push("Hierarchy");

    callonServer("Services/Master.asmx/GetProjectDetails", OtherDetails, "ProjectTable", HeaderCaption, "ProjectIndex", "SetProjectCode");

}
function PurInvoiceNoKeyDown(s, e) {
    if (e.htmlEvent.key == "Enter") {
    $('#PurchaseInvoiceModel').modal('show');
    if ($("#hdnProjectSelectInEntryModule").val() == "1") {
        var ProjectId = $("#hdnProjectId").val();
            //(clookup_Project.GetGridView().GetRowKey(clookup_Project.GetGridView().GetFocusedRowIndex()));
        if (ProjectId != null && ProjectId != "") {
            ShowInvoiceDetailsWithProject();
        }
        else {
            ShowInvoiceDetails();
        }

    }
    else {
        ShowInvoiceDetails();
    }
    }
}
function ProjectKeyDown(s, e) {
    if (e.htmlEvent.key == "Enter") {
    $('#ProjectModel').modal('show');
    ShowProjectDetails();
    }
}
function PurchaseInvoicekeydown(e) {
    var OtherDetails = {}
    //if ($.trim($("#txtPurchaseInvoiceSearch").val()) == "" || $.trim($("#txtPurchaseInvoiceSearch").val()) == null) {
    //    return false;
    //}
    OtherDetails.CustVenID = GetObjectID('hdfLookupCustomer').value;
    OtherDetails.BranchList = $('#ddlBranch').val();
    OtherDetails.SearchKey = $("#txtPurchaseInvoiceSearch").val();
    if (e.code == "Enter" || e.code == "NumpadEnter") {
        var HeaderCaption = [];
        HeaderCaption.push("Invoice Number");
        HeaderCaption.push("Party Invoice No.");
        HeaderCaption.push("Party Invoice Date");

        callonServer("Services/Master.asmx/GetInvoiceDetails", OtherDetails, "PurchaseInvoiceTable", HeaderCaption, "PurchaseInvoiceIndex", "SetPurchaseInvoice");
    }
    else if (e.code == "ArrowDown") {
        if ($("input[PurchaseInvoiceIndex=0]"))
            $("input[PurchaseInvoiceIndex=0]").focus();
    }
    else if (e.code == "Escape") {
        ctxtPurchaseInvoiceNo.Focus();
        $('#PurchaseInvoiceModel').modal('hide');

    }
}


function ProjectCodekeydown(e) {
    var OtherDetails = {}
   
    OtherDetails.BranchList = $('#ddlBranch').val();
    OtherDetails.SearchKey = $("#txtProjectSearch").val();
    if (e.code == "Enter" || e.code == "NumpadEnter") {
        var HeaderCaption = [];
        HeaderCaption.push("Project Code");
        HeaderCaption.push("Project Name");
        HeaderCaption.push("Customer");
        HeaderCaption.push("Hierarchy");

        callonServer("Services/Master.asmx/GetProjectDetails", OtherDetails, "ProjectTable", HeaderCaption, "ProjectIndex", "SetProjectCode");
    }
    else if (e.code == "ArrowDown") {
        if ($("input[ProjectIndex=0]"))
            $("input[ProjectIndex=0]").focus();
    }
    else if (e.code == "Escape") {
        ctxtProject.Focus();
        $('#ProjectModel').modal('hide');

    }
}


function SetPurchaseInvoice(Id, Name) {
    var PurchaseInvoice = Id;
    if (Id != "") {
        $('#PurchaseInvoiceModel').modal('hide');
        ctxtPurchaseInvoiceNo.SetText(Name);
        GetObjectID('hdnPurchaseInvoiceID').value = PurchaseInvoice;


    }
}

function SetProjectCode(Id, Name) {
    var ProjectId = Id;

    if ($("#hdnProjectSelectInEntryModule").val() == "1")
    {
        var gridVal = "";
        if ($("#Keyval_internalId").val() == "Add") {
            grid.batchEditApi.StartEdit(-1);
            gridVal = grid.GetEditor("MainAccount").GetValue();
            grid.batchEditApi.EndEdit();
        }
        else {
            grid.batchEditApi.StartEdit(0);
            gridVal = grid.GetEditor("MainAccount").GetValue();
            grid.batchEditApi.EndEdit(0);
        }

        if( grid.GetVisibleRowsOnPage() > 0 && gridVal != "" && gridVal != null &&  $("#hdnProjectId").val()!=ProjectId)
        {
            jAlert("Project Change will  blank  the grid.");
            ctxtPurchaseInvoiceNo.SetText("");

            $("#hdnPurchaseInvoiceID").val("");

            deleteAllRows();
            //deleteAllRows();
            grid.AddNewRow();
            grid.GetEditor('SrlNo').SetValue('1');
        }

    }


    if (Id != "") {
        $('#ProjectModel').modal('hide');
        ctxtProject.SetText(Name);
        GetObjectID('hdnProjectId').value = ProjectId;
        ProjectValueChange(Name);
        ctxtProject.Focus();
        
    }

}


var debitOldValue;
var isCtrl = false;
document.onkeydown = function (e) {
    if (event.keyCode == 83 && event.altKey == true) { //run code for Ctrl+S -- ie, Save & New  
        StopDefaultAction(e);
        document.getElementById('btnSaveRecords').click();
        return false;
    }
    else if (event.keyCode == 88 && event.altKey == true) { //run code for Ctrl+X -- ie, Save & Exit!   
        StopDefaultAction(e);
        document.getElementById('btn_SaveRecords').click();
        return false;
    }
    else if (event.keyCode == 65 && event.altKey == true) {
        StopDefaultAction(e);
        if (document.getElementById('divAddNew').style.display != 'block') {
            OnAddButtonClick();
        }
    }
    else if (event.keyCode == 79 && event.altKey == true) {
        StopDefaultAction(e);
        if (page.GetActiveTabIndex() == 1) {
            fnSaveBillingShipping();
        }
        return false;
    }
}

function StopDefaultAction(e) {
    if (e.preventDefault) { e.preventDefault() }
    else { e.stop() };

    e.returnValue = false;
    e.stopPropagation();
}
function disp_prompt(name) {
    if (name == "tab0") {
    }
    if (name == "tab1") {
    }
}
function OnAddNewClick() {
    grid.AddNewRow();
    var noofvisiblerows = grid.GetVisibleRowsOnPage();
    var i;
    var cnt = 1;
    for (i = -1 ; cnt <= noofvisiblerows ; i--) {
        var tbQuotation = grid.GetEditor("SrlNo");
        tbQuotation.SetValue(cnt);
        cnt++;
    }
}
function ReloadPage() {
    $('#hdnNotelNo').val('');
    window.location.assign("VendorDrCrNoteList.aspx");

}
var InvoiceID;
function ReloadPage() {
    window.location.assign("VendorDrCrNoteList.aspx");
}
function OnKeyDown(s, e) {
    if (e.htmlEvent.keyCode == 40 || e.htmlEvent.keyCode == 38)
        return ASPxClientUtils.PreventEvent(e.htmlEvent);
}
var debitNewValue;
function DebitLostFocus(s, e) {
    debitNewValue = s.GetText();

    var indx = debitNewValue.indexOf(',');
    if (indx != -1) {
        debitNewValue = debitNewValue.replace(/,/g, '');
    }
    if (debitOldValue != debitNewValue) {
        grid.GetEditor('TaxAmount').SetValue("0");
        changeDebitTotalSummary();

    }


    var Amount = grid.GetEditor("WithDrawl").GetText();
    var amountAfterDiscount = grid.GetEditor("WithDrawl").GetText();

    //var SateId = $("#ucBShfSStateCode").val().split("(State Code:")[1];
    //SateId = SateId.split(")")[0];
    if ($("#hfVendorGSTIN").val() != "" && $("#hfVendorGSTIN").val() != null) {

        $("#hfVendorGSTIN").val(GeteShippingStateCode());
    }




    caluculateAndSetGST(grid.GetEditor("WithDrawl"), grid.GetEditor("TaxAmount"), grid.GetEditor("NetAmount"), grid.GetEditor("TAXable").GetText(), Amount, amountAfterDiscount, "E", "", $("#ddlBranch").val(), 'P')







    grid.GetEditor('NetAmount').SetValue(parseFloat(debitNewValue) + parseFloat(grid.GetEditor("TaxAmount").GetText()));
}
function changeDebitTotalSummary() {
    var newDif = debitOldValue - debitNewValue;
    var CurrentSum = c_txt_Debit.GetText();
    var indx = CurrentSum.indexOf(',');
    if (indx != -1) {
        CurrentSum = CurrentSum.replace(/,/g, '');
    }
    cdeleteTax.PerformCallback('DeleteAllTax');
    c_txt_Debit.SetValue(parseFloat(CurrentSum - newDif));
}

function DebitGotFocus(s, e) {
    debitOldValue = s.GetText();
    var indx = debitOldValue.indexOf(',');
    if (indx != -1) {
        debitOldValue = debitOldValue.replace(/,/g, '');
    }
}


function IntializeGlobalVariables(grid) {

    lastCountryID = grid.cplastCountryID;
    currentEditableVisibleIndex = -1;
    setValueFlag = -1;
}
function OnInit(s, e) {
    IntializeGlobalVariables(s);
}
function OnEndCallback(s, e) {
    IntializeGlobalVariables(s);
    // LoadingPanel.Hide();      
    $('#hdnDeleteSrlNo').val('0');


    if ($("#Keyval_internalId").val() != "Add") {
        $('#hdfIsDelete').val('C');
    }
    else {
        $('#hdfIsDelete').val('');
    }
    var value = document.getElementById('hdnRefreshType').value;
    //if (grid.cpSaveSuccessOrFail == "outrange") {
    //    grid.cpSaveSuccessOrFail = null;
    //    jAlert('Can Not Add More Vendor Debit/Credit Note as Scheme Exausted.<br />Update The Scheme and Try Again');
    //}
    //else if (grid.cpSaveSuccessOrFail == "duplicate") {
    //    grid.cpSaveSuccessOrFail = null;
    //    jAlert('Can Not Save as Duplicate Vendor Debit/Credit Note No. Found');
    //}

    if (grid.cpSaveSuccessOrFail == "errorInsert") {
        grid.cpSaveSuccessOrFail = null;
        jAlert('Try again later.');
        cbtnSaveRecords.SetVisible(true);
        cbtn_SaveRecords.SetVisible(true);
    }
    else if (grid.cpSaveSuccessOrFail == "EmptyProject") {
        grid.cpSaveSuccessOrFail = null;
        jAlert('Please select project.');
        cbtnSaveRecords.SetVisible(true);
        cbtn_SaveRecords.SetVisible(true);
    }
    else if (grid.cpSaveSuccessOrFail == "taxREquired") {
        grid.cpSaveSuccessOrFail = null;

       
        jAlert('Selected Ledger is tagged for GST calculation. Click on Charges to calculate GST and Proceed.');
        cbtnSaveRecords.SetVisible(true);
        cbtn_SaveRecords.SetVisible(true);
        OnAddNewClick();
    }

    else if (grid.cpSaveSuccessOrFail == "AddLock") {
        jAlert('DATA is Freezed between ' + grid.cpAddLockStatus+' for Add.');
        OnAddNewClick();
    }

    else if (grid.cpSaveSuccessOrFail == "zeroAmount") {
        grid.cpSaveSuccessOrFail = null;
        OnAddNewClick();
        jAlert('Cannot save with ZERO Amount.');
        cbtnSaveRecords.SetVisible(true);
        cbtn_SaveRecords.SetVisible(true);
    }
    else if (grid.cpSaveSuccessOrFail == "addressrequired") {
        grid.cpSaveSuccessOrFail = null;
        OnAddNewClick();

        jAlert("Please Enter Billing/Shipping and GSTIN Details to Calculate GST.", "Alert !!", function () {
            page.SetActiveTabIndex(1);
            cbsSave_BillingShipping.Focus();
            page.tabs[0].SetEnabled(false);
            $("#divcross").hide();
        });
        cbtnSaveRecords.SetVisible(true);
        cbtn_SaveRecords.SetVisible(true);

    }
    else if (grid.cpSaveSuccessOrFail == "successInsert") {
        grid.cpSaveSuccessOrFail = null;
        var JV_Number = grid.cpVouvherNo;
        var JV_Msg = "Vendor Debit/Credit Note No. " + JV_Number + " generated.";
        var strSchemaType = document.getElementById('hdnSchemaType').value;

        if (value == "E") {
            if (JV_Number != "") {
                jAlert(JV_Msg, 'Alert Dialog: [VendorNote]', function (r) {
                    if (r == true) {
                        window.location.assign("VendorDrCrNoteList.aspx");
                    }
                });
            } else {
                window.location.assign("VendorDrCrNoteList.aspx");
            }
        }
        else if (value == "S") {

            if (JV_Number != "") {
                jAlert(JV_Msg, 'Alert Dialog: [VendorNote]', function (r) {
                    if (r == true) {
                        window.location.assign("VendorDrCrNoteAdd.aspx?key=ADD");
                    }
                });
            }
            else {
                window.location.assign("VendorDrCrNoteAdd.aspx?key=ADD");
            }

        }

    }
    //else if (grid.cpReCalTax != null) {
    //    grid.cpReCalTax = null;
    //    $("#HDParentSlNo").val(grid.GetRowKey(globalRowIndex));
    //    OpenTaxPopUp();
    //}
    //else if (grid.cpReCalTaxLedger != null) {
    //    grid.cpReCalTaxLedger = null;
    //    $("#HDParentSlNo").val(grid.GetRowKey(globalRowIndex));
    //}
    //else {
    //    if ($("#divAddNew").is(':visible')) {
    //        if (caspxTaxpopUp.GetVisible() == false) {
    //            grid.AddNewRow();
    //        }
    //        else if (caspxTaxpopUp.GetVisible() == true) {
    //            $("#Button1").focus();
    //        }
    //    }
    //}

    //if (grid.cpView == "1") {
    //    grid.cpView = null;
    //    viewOnly();
    //}


    //if (ctxtTaxTotAmt.GetValue() == "0.00") {
    //}
    //else {
    //    c_txtTaxableAmount.SetValue(grid.cpTotalTaxableAmount);
    //}

    //c_txtTaxAmount.SetValue(grid.cpTotalTaxAmount);
    //c_txt_Debit.SetValue(grid.cpTotalAmount);

    grid.batchEditApi.EndEdit();

}
function OnBatchEditStartEditing(s, e) {
    currentEditableVisibleIndex = e.visibleIndex;
    globalRowIndex = e.visibleIndex;
}
function OnCustomButtonClick(s, e) {
    if (e.buttonID == 'CustomDelete') {

        var SrlNo = grid.batchEditApi.GetCellValue(e.visibleIndex, 'SrlNo');

        $('#hdnRefreshType').val('');
        $('#hdnDeleteSrlNo').val(SrlNo);
        //CustomDeleteID = "1";
        var noofvisiblerows = grid.GetVisibleRowsOnPage();
        if (noofvisiblerows != "1") {
            grid.batchEditApi.StartEdit(e.visibleIndex, 1);
            grid.DeleteRow(e.visibleIndex);

            $('#hdfIsDelete').val('D');
            grid.UpdateEdit();

            grid.PerformCallback('Display');
            $('#hdnPageStatus').val('delete');


            var Amount = (grid.GetEditor('NetAmount').GetValue() != null) ? grid.GetEditor('NetAmount').GetValue() : "0";
            //var ReceiptValue = (grid.GetEditor('btnRecieve').GetValue() != null) ? parseFloat(grid.GetEditor('btnRecieve').GetValue()) : "0";

            var receiptTotValue = c_txt_Debit.GetValue();
            //var paymentTotValue = ctxtTotalPayment.GetValue();
            var TotalValue = c_txt_Debit.GetValue();
            c_txt_Debit.SetValue(parseFloat(TotalValue) - parseFloat(Amount));
            //ctxtTotalPayment.SetValue(parseFloat(paymentTotValue) - parseFloat(PaymentValue));


            // cbtnSaveRecords.SetVisible(false);
            cbtnSaveRecords.SetVisible(false);
        }
    }
    else if (e.buttonID == 'AddNew') {
        if (grid.GetEditor('gvColMainAccount').GetValue() != null && grid.GetEditor('gvColMainAccount').GetValue() != '' && grid.GetEditor('WithDrawl').GetValue() != "0.00") {
            grid.batchEditApi.StartEdit(e.visibleIndex);
            OnAddNewClick();
        }
    }
}


var shouldCheck = 0;
function MainAccountClose(s, e) {
    cMainAccountpopUp.Hide();
    grid.batchEditApi.StartEdit(globalRowIndex, 2);
}
function MainAccountButnClick(s, e) {
    $("#MainAccountTable").empty();
    var html = "<table border='1' width='100%' class='dynamicPopupTbl'><tr class='HeaderStyle'><th>Main Account Name</th><th>Subledger Type</th><th>HSN/SAC</th></tr></table>"
    $("#MainAccountTable").html(html);
    if (e.buttonIndex == 0) {
        setTimeout(function () { $("#txtMainAccountSearch").focus(); }, 500);
        $('#txtMainAccountSearch').val('');
        shouldCheck = 1;
        $('#mainActMsg').hide();
        $('#MainAccountModel').modal('show');

    }

}
function MainAccountKeyDown(s, e) {
    if (e.htmlEvent.key == "Enter") {
        shouldCheck = 0;
        s.OnButtonClick(0);
    }
}

function GetMainAcountComboBox(id, name, IsSub, TaxAble) {
    IsSubAccount = IsSub;
    var TaxApplicable = TaxAble;
    if (grid.GetEditor('CashReportID').GetValue() != "" || grid.GetEditor('CashReportID').GetValue() != null) {
        grid.GetEditor('CashReportID').SetValue("");
    }

    grid.batchEditApi.StartEdit(globalRowIndex, 3);
    grid.GetEditor("MainAccount").SetText(name);

    grid.GetEditor("gvColMainAccount").SetText(id);
    var MainAccountID = (grid.GetEditor('gvColMainAccount').GetValue() != null) ? grid.GetEditor('gvColMainAccount').GetValue() : "0";

    shouldCheck = 0;
    grid.GetEditor("bthSubAccount").SetValue("");
    grid.GetEditor("WithDrawl").SetValue("");
    grid.GetEditor("Narration").SetValue("");
    grid.GetEditor("TaxAmount").SetValue("0.00");
    grid.GetEditor("NetAmount").SetValue("0.00");
    grid.GetEditor("IsSubledger").SetValue(IsSubAccount);
    grid.GetEditor("TAXable").SetValue(TaxApplicable);

    grid.GetEditor("gvColSubAccount").SetValue("");
    $("#IsTaxApplicable").val(TaxApplicable);

}
function MainAccountComboBoxKeyDown(s, e) {
    if (e.htmlEvent.key == "Escape") {
        cMainAccountpopUp.Hide();
        grid.batchEditApi.StartEdit(globalRowIndex, 3);
    }

}

function SubAccountClose(s, e) {
    cSubAccountpopUp.Hide();
    grid.batchEditApi.StartEdit(globalRowIndex, 3);
}
function SubAccountKeyDown(s, e) {
    if (e.htmlEvent.key == "Enter") {
        s.OnButtonClick(0);
    }

}
function SubAccountComboBoxKeyDown(s, e) {
    if (e.htmlEvent.key == "Escape") {
        cSubAccountpopUp.Hide();
        grid.batchEditApi.StartEdit(globalRowIndex, 4);
    }
    if (e.htmlEvent.key == "Enter") {
        GetSubAcountComboBox(e);
    }
}
function GetSubAcountComboBox(id, name) {


    grid.batchEditApi.StartEdit(globalRowIndex, 4);

    grid.GetEditor("bthSubAccount").SetText(name);
    grid.GetEditor("gvColSubAccount").SetText(id);
    //cSubAccountpopUp.Hide();

}
function SubAccountButnClick(s, e) {
    txt = " <table border='1' width=\"100%\"  class=\"dynamicPopupTbl\"><tr class=\"HeaderStyle\"><th>Sub Account Name [Unique Id]</th><th>Sub Account Code</th></tr></table>";
    document.getElementById("SubAccountTable").innerHTML = txt;

    $("#mainActMsgSub").hide();
    if (IsSubAccount != 'None') {
        grid.batchEditApi.StartEdit(e.visibleIndex);
        var strMainAccountID = (grid.GetEditor('MainAccount').GetText() != null) ? grid.GetEditor('MainAccount').GetText() : "0";
        var MainAccountID = (grid.GetEditor('gvColMainAccount').GetValue() != null) ? grid.GetEditor('gvColMainAccount').GetValue() : "0";
        if (e.buttonIndex == 0) {
            if (strMainAccountID.trim() != "") {
                document.getElementById('hdnMainAccountId').value = MainAccountID;
                $('#SubAccountModel').modal('show');

            }
        }
    }
}
function CloseSubModal() {
    $('#SubAccountModel').modal('hide');
    grid.batchEditApi.StartEdit(globalRowIndex, 2);

}

var IsSubAccount = '';
function MainAccountNewkeydown(e) {
    var OtherDetails = {}
    if ($.trim($("#txtMainAccountSearch").val()) == "" || $.trim($("#txtMainAccountSearch").val()) == null) {
        return false;
    }
    OtherDetails.SearchKey = $("#txtMainAccountSearch").val();
    OtherDetails.branchId = $("#ddlBranch").val();
    if (e.code == "Enter" || e.code == "NumpadEnter") {
        if ($("#txtMainAccountSearch").val() == "")
            return;
        var HeaderCaption = [];
        HeaderCaption.push("Main Account Name");
        HeaderCaption.push("Subledger Type");
        HeaderCaption.push("HSN/SAC");
        callonServer("/OMS/Management/Activities/Services/Master.asmx/GetMainAccountVendorDrCrNote", OtherDetails, "MainAccountTable", HeaderCaption, "MainAccountIndex", "SetMainAccount");
    }
    else if (e.code == "ArrowDown") {
        if ($("input[MainAccountIndex=0]"))
            $("input[MainAccountIndex=0]").focus();
    }
    else if (e.code == "Escape") {
        //  
        $('#MainAccountModel').modal('hide');
        grid.batchEditApi.StartEdit(globalRowIndex, 1);
        var MainAccountID = grid.GetEditor("gvMainAcCode").GetValue();
        if (MainAccountID == 'Customers' || MainAccountID == 'Vendors') {
            if ($("#hdnIsPartyLedger").val() == "") {
                $("#hdnIsPartyLedger").val('1');
            }
            else {
                $("#hdnIsPartyLedger").val(parseFloat($("#hdnIsPartyLedger").val()) + 1);
            }

        }

    }
}
function SubAccountNewkeydown(e) {
    grid.batchEditApi.StartEdit(e.visibleIndex);

    var MainAccountID = (grid.GetEditor('gvColMainAccount').GetValue() != null) ? grid.GetEditor('gvColMainAccount').GetValue() : "0";
    var OtherDetails = {}

    if ($.trim($("#txtSubAccountSearch").val()) == "" || $.trim($("#txtSubAccountSearch").val()) == null) {
        return false;
    }
    OtherDetails.SearchKey = $("#txtSubAccountSearch").val();
    OtherDetails.MainAccountCode = MainAccountID;
    if (e.code == "Enter" || e.code == "NumpadEnter") {
        if ($("#txtSubAccountSearch").val() == "")
            return;
        var HeaderCaption = [];
        HeaderCaption.push("Sub Account Name [Unique Id]");
        HeaderCaption.push("Sub Account Type");

        callonServer("/OMS/Management/Activities/Services/Master.asmx/GetSubAccountJournal", OtherDetails, "SubAccountTable", HeaderCaption, "SubAccountIndex", "SetSubAccount");
    }
    else if (e.code == "ArrowDown") {
        if ($("input[SubAccountIndex=0]"))
            $("input[SubAccountIndex=0]").focus();
    }
    else if (e.code == "Escape") {
        $('#SubAccountModel').modal('hide');
        grid.batchEditApi.StartEdit(globalRowIndex, 2);
    }
}
function SetMainAccount(Id, name, e) {
    $('#MainAccountModel').modal('hide');
    ctxtCustName.SetEnabled(false);
    var IsSub = e.parentElement.cells[2].innerText;
    var TaxAble = e.parentElement.cells[3].innerText;
    GetMainAcountComboBox(Id, name, IsSub, TaxAble);

    setTimeout(function () { grid.batchEditApi.StartEdit(globalRowIndex, 3); }, 500);
    // grid.batchEditApi.StartEdit(globalRowIndex, 2);


}
function SetSubAccount(Id, name) {
    $('#SubAccountModel').modal('hide');
    GetSubAcountComboBox(Id, name);
    setTimeout(function () { grid.batchEditApi.StartEdit(globalRowIndex, 4); }, 500);
    //grid.batchEditApi.StartEdit(globalRowIndex, 3);
}
function ValueSelected(e, indexName) {
    if (e.code == "Enter" || e.code == "NumpadEnter") {
        var Id = e.target.parentElement.parentElement.cells[0].innerText;
        var name = e.target.parentElement.parentElement.cells[1].children[0].value;

        if (Id) {
            if (indexName == "MainAccountIndex") {
                $('#MainAccountModel').modal('hide');
                var IsSub = e.target.parentElement.parentElement.children[2].innerText;
                var TaxAble = e.target.parentElement.parentElement.children[3].innerText;
                GetMainAcountComboBox(Id, name, IsSub, TaxAble);
                setTimeout(function () { grid.batchEditApi.StartEdit(globalRowIndex, 3); }, 500);
                //grid.batchEditApi.StartEdit(globalRowIndex, 2);
            }
            else if (indexName == "SubAccountIndex") {
                $('#SubAccountModel').modal('hide');
                GetSubAcountComboBox(Id, name);
                grid.batchEditApi.StartEdit(globalRowIndex, 4);
            }
            else if (indexName == "customerIndex") {
                SetCustomer(Id, name, e.target.parentElement);
            }
            else if (indexName == "SchemeIndex") {
                SetScheme(Id, name);
            }
            else if (indexName == "PurchaseInvoiceIndex") {
                SetPurchaseInvoice(Id, name);
            }
            else if (indexName == "ProjectIndex") {
                SetProjectCode(Id, name);
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
            if (indexName == "MainAccountIndex")
                $('#txtMainAccountSearch').focus();
            else if (indexName == "SubAccountIndex")
                $('#txtSubAccountSearch').focus();
            else if (indexName == "customerIndex")
                $('#txtCustSearch').focus();
            else if (indexName == "SchemeIndex")
                $('#txtSchemeSearch').focus();

        }
    }
    else if (e.code == "Escape") {
        if (indexName == "MainAccountIndex") {
            $('#MainAccountModel').modal('hide');
            grid.batchEditApi.StartEdit(globalRowIndex, 1);
            var MainAccountID = grid.GetEditor("gvMainAcCode").GetValue();
            if (MainAccountID == 'Customers' || MainAccountID == 'Vendors') {
                if ($("#hdnIsPartyLedger").val() == "") {
                    $("#hdnIsPartyLedger").val('1');
                }
                else {
                    $("#hdnIsPartyLedger").val(parseFloat($("#hdnIsPartyLedger").val()) + 1);
                }

            }

        }
        else if (indexName == "SubAccountIndex") {
            $('#SubAccountModel').modal('hide');
            grid.batchEditApi.StartEdit(globalRowIndex, 2);
        }
    }


}

//------------------------------------------------Tax-------------------------------------
var taxJson;
var ChargegstcstvatGlobalName;
var taxAmountGlobal;
var globalTaxRowIndex;
var gstcstvatGlobalName;
var GlobalCurTaxAmt = 0;
var SrlNo = 0;
function TaxAmountKeyDown(s, e) {
    if (e.htmlEvent.key == "Enter") {
        s.OnButtonClick(0);
    }
}
function taxAmtButnClick1(s, e) {
    rowEditCtrl = s;
}
function taxAmtButnClick(s, e) {
    var TaxAmountOngrid = grid.GetEditor("TaxAmount").GetValue();
    $("#TaxAmountOngrid").val(TaxAmountOngrid);
    $("#VisibleIndexForTax").val(globalRowIndex);
    if (e.buttonIndex == 0) {
        grid.batchEditApi.StartEdit(e.visibleIndex);
        var shippingStCode = '';


        //  shippingStCode = cbsSCmbState.GetText();

        shippingStCode = GeteShippingStateCode();
        document.getElementById('HdSerialNo1').value = grid.GetEditor('SrlNo').GetValue();

        if (shippingStCode != '') {
            showTax();
        }
        else {

            if ($("#IsTaxApplicable").val() != "" && $("#IsTaxApplicable").val() != null) {
                jAlert("Please Enter Billing/Shipping Details to Calculate GST.", "Alert !!", function () {
                    page.SetActiveTabIndex(1);
                    cbsSave_BillingShipping.Focus();
                    page.tabs[0].SetEnabled(false);
                    $("#divcross").hide();
                });
            }

            else {
                showTax();
            }

        }
    }
}
function RecalCulateTaxTotalAmountInline() {
    var totalInlineTaxAmount = 0;
    for (var i = 0; i < taxJson.length; i++) {
        cgridTax.batchEditApi.StartEdit(i, 3);
        var totLength = cgridTax.GetEditor("Taxes_Name").GetText().length;
        var sign = cgridTax.GetEditor("Taxes_Name").GetText().substring(totLength - 3);
        if (sign == '(+)') {
            totalInlineTaxAmount = totalInlineTaxAmount + parseFloat(cgridTax.GetEditor("Amount").GetValue());
        } else {
            totalInlineTaxAmount = totalInlineTaxAmount - parseFloat(cgridTax.GetEditor("Amount").GetValue());
        }

        cgridTax.batchEditApi.EndEdit();
    }
    totalInlineTaxAmount = totalInlineTaxAmount + parseFloat(ctxtGstCstVat.GetValue());
    ctxtTaxTotAmt.SetValue(totalInlineTaxAmount, 2);

}
function taxAmountLostFocus(s, e) {
    var finalTaxAmt = parseFloat(s.GetValue());
    var totAmt = parseFloat(ctxtTaxTotAmt.GetText());
    var totLength = cgridTax.GetEditor("Taxes_Name").GetText().length;
    var sign = cgridTax.GetEditor("Taxes_Name").GetText().substring(totLength - 3);
    if (sign == '(+)') {
        ctxtTaxTotAmt.SetValue(((totAmt + finalTaxAmt - taxAmountGlobal), 2));
    } else {
        ctxtTaxTotAmt.SetValue(((totAmt + (finalTaxAmt * -1) - (taxAmountGlobal * -1)), 2));
    }
    // SetOtherTaxValueOnRespectiveRow(0, cgridTax.GetEditor("Amount").GetValue(), cgridTax.GetEditor("Taxes_Name").GetText().replace('(+)', '').replace('(-)', ''));

    SetRunningTotal();
    RecalCulateTaxTotalAmountInline();
}
function txtPercentageLostFocus(s, e) {

    var ProdAmt = parseFloat(cgridTax.GetEditor("calCulatedOn").GetValue());
    if (s.GetText().trim() != '') {
        if (!isNaN(parseFloat(ProdAmt * s.GetText()) / 100)) {
            var totLength = cgridTax.GetEditor("Taxes_Name").GetText().length;
            var taxNameWithSign = cgridTax.GetEditor("TaxField").GetText();
            var sign = cgridTax.GetEditor("Taxes_Name").GetText().substring(totLength - 3);
            if (sign == '(+)') {
                GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());
                cgridTax.GetEditor("Amount").SetValue(parseFloat(ProdAmt * s.GetText()) / 100);
                ctxtTaxTotAmt.SetValue((parseFloat(ctxtTaxTotAmt.GetValue()) + (parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) - GlobalCurTaxAmt), 2);
                GlobalCurTaxAmt = 0;
            }
            else {

                GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());
                cgridTax.GetEditor("Amount").SetValue((parseFloat(ProdAmt * s.GetText()) / 100) * -1);
                ctxtTaxTotAmt.SetValue((parseFloat(ctxtTaxTotAmt.GetValue()) + ((parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) * -1) - (GlobalCurTaxAmt * -1), 2));
                GlobalCurTaxAmt = 0;
            }
            //  SetOtherTaxValueOnRespectiveRow(0, cgridTax.GetEditor("Amount").GetValue(), cgridTax.GetEditor("Taxes_Name").GetText().replace('(+)', '').replace('(-)', ''));


            SetRunningTotal();
        } else {
            s.SetText("");
        }
    }
    RecalCulateTaxTotalAmountInline();
}
function CmbtaxClick(s, e) {
    GlobalCurTaxAmt = parseFloat(ctxtGstCstVat.GetText());
    gstcstvatGlobalName = s.GetText();
}
function GetTaxVisibleIndex(s, e) {
    globalTaxRowIndex = e.visibleIndex;
}
function taxAmountGotFocus(s, e) {
    taxAmountGlobal = parseFloat(s.GetValue());
}
function chargeCmbtaxClick(s, e) {
    GlobalCurChargeTaxAmt = parseFloat(ctxtGstCstVatCharge.GetText());
    ChargegstcstvatGlobalName = s.GetText();
}
function ShowTaxPopUp(type) {
    if (type == "IY") {
        $('#ContentErrorMsg').hide();
        $('#content-6').show();
        document.getElementById('calculateTotalAmountOK').style.display = 'block';
        if (ccmbGstCstVat.GetItemCount() <= 1) {
            $('.InlineTaxClass').hide();
        } else {
            $('.InlineTaxClass').show();
        }
        if (cgridTax.GetVisibleRowsOnPage() < 1) {
            $('.cgridTaxClass').hide();

        } else {
            $('.cgridTaxClass').show();
        }

        if (ccmbGstCstVat.GetItemCount() <= 1 && cgridTax.GetVisibleRowsOnPage() < 1) {
            $('#ContentErrorMsg').show();
            $('#content-6').hide();
            document.getElementById('calculateTotalAmountOK').style.display = 'none';

        }
    }
    if (type == "IN") {
        $('#ErrorMsgCharges').hide();
        $('#content-5').show();

        if (ccmbGstCstVatcharge.GetItemCount() <= 1) {
            $('.chargesDDownTaxClass').hide();
        } else {
            $('.chargesDDownTaxClass').show();
        }
        if (gridTax.GetVisibleRowsOnPage() < 1) {
            $('.gridTaxClass').hide();

        } else {
            $('.gridTaxClass').show();
        }

        if (ccmbGstCstVatcharge.GetItemCount() <= 1 && gridTax.GetVisibleRowsOnPage() < 1) {
            $('#ErrorMsgCharges').show();
            $('#content-5').hide();
        }
    }
}
function cgridTax_EndCallBack(s, e) {
    $("#TaxAmountOngrid").val("");
    $("#VisibleIndexForTax").val("");

    //cgridTax.batchEditApi.StartEdit(0);

    cgridTax.batchEditApi.StartEdit(0, 4);
    cgridTax.GetEditor('Amount').SetValue(cgridTax.GetEditor('Amount').GetValue());

    $('.cgridTaxClass').show();
    //cgridTax.batchEditApi.StartEdit(0,1);
    //check Json data
    if (cgridTax.cpJsonData) {
        if (cgridTax.cpJsonData != "") {
            taxJson = JSON.parse(cgridTax.cpJsonData);
            cgridTax.cpJsonData = null;
        }
    }
    //End Here
    if (cgridTax.cpComboCode) {
        if (cgridTax.cpComboCode != "") {
            //if (cddl_AmountAre.GetValue() == "1") {
            var selectedIndex;
            for (var i = 0; i < ccmbGstCstVat.GetItemCount() ; i++) {
                if (ccmbGstCstVat.GetItem(i).value.split('~')[0] == cgridTax.cpComboCode) {
                    selectedIndex = i;
                }
            }
            if (selectedIndex && ccmbGstCstVat.GetItem(selectedIndex) != null) {
                ccmbGstCstVat.SetValue(ccmbGstCstVat.GetItem(selectedIndex).value);
            }
            cmbGstCstVatChange(ccmbGstCstVat);
            cgridTax.cpComboCode = null;
            //  }
        }
    }

    if (cgridTax.cpUpdated.split('~')[0] == 'ok') {
        ctxtTaxTotAmt.SetValue(cgridTax.cpUpdated.split('~')[1]);
        var gridValue = parseFloat(cgridTax.cpUpdated.split('~')[1]);
        var ddValue = parseFloat(ctxtGstCstVat.GetValue());
        ctxtTaxTotAmt.SetValue(gridValue + ddValue);
        cgridTax.cpUpdated = "";
        RecalCulateTaxTotalAmountInline();
    }
    else {
        var totAmt = ctxtTaxTotAmt.GetValue();
        caspxTaxpopUp.Hide();
        cgridTax.CancelEdit();
    }
    if (cgridTax.GetVisibleRowsOnPage() == 0) {
        $('.cgridTaxClass').hide();
        ccmbGstCstVat.Focus();
    }
    //Debjyoti Check where any Gst Present or not
    // If Not then hide the hole section
    SetRunningTotal();
    ShowTaxPopUp("IY");
}
function SetRunningTotal() {
    var runningTot = parseFloat(clblProdNetAmt.GetValue());
    for (var i = 0; i < taxJson.length; i++) {
        cgridTax.batchEditApi.StartEdit(i, 3);
        if (taxJson[i].applicableOn == "R") {
            cgridTax.GetEditor("calCulatedOn").SetValue(runningTot);
            var totLength = cgridTax.GetEditor("Taxes_Name").GetText().length;
            var taxNameWithSign = cgridTax.GetEditor("TaxField").GetText();
            var sign = cgridTax.GetEditor("Taxes_Name").GetText().substring(totLength - 3);
            var ProdAmt = parseFloat(cgridTax.GetEditor("calCulatedOn").GetValue());
            var thisRunningAmt = 0;
            if (sign == '(+)') {
                GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());
                cgridTax.GetEditor("Amount").SetValue(parseFloat(ProdAmt * cgridTax.GetEditor("TaxField").GetValue()) / 100);

                ctxtTaxTotAmt.SetValue(Math.round((parseFloat(ctxtTaxTotAmt.GetValue()) + (parseFloat(ProdAmt * parseFloat(cgridTax.GetEditor("TaxField").GetValue())) / 100) - GlobalCurTaxAmt), 2));
                GlobalCurTaxAmt = 0;
            }
            else {

                GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());
                cgridTax.GetEditor("Amount").SetValue((parseFloat(ProdAmt * cgridTax.GetEditor("TaxField").GetValue()) / 100) * -1);

                ctxtTaxTotAmt.SetValue(Math.round((parseFloat(ctxtTaxTotAmt.GetValue()) + ((parseFloat(ProdAmt * parseFloat(cgridTax.GetEditor("TaxField").GetValue())) / 100) * -1) - (GlobalCurTaxAmt * -1)), 2));
                GlobalCurTaxAmt = 0;
            }
            //  SetOtherTaxValueOnRespectiveRow(0, cgridTax.GetEditor("Amount").GetValue(), cgridTax.GetEditor("Taxes_Name").GetText().replace('(+)', '').replace('(-)', ''));
        }
        runningTot = runningTot + parseFloat(cgridTax.GetEditor("Amount").GetValue());
        cgridTax.batchEditApi.EndEdit();
    }
}
function recalculateTax() {
    cmbGstCstVatChange(ccmbGstCstVat);
}
function recalculateTaxCharge() {
    ChargecmbGstCstVatChange(ccmbGstCstVatcharge);
}
function chargeCmbtaxClick(s, e) {
    GlobalCurChargeTaxAmt = parseFloat(ctxtGstCstVatCharge.GetText());
    ChargegstcstvatGlobalName = s.GetText();
}
function showTax() {

    var strMainAccountID = (grid.GetEditor('MainAccount').GetText() != null) ? grid.GetEditor('MainAccount').GetText() : "0";
    var MainAccountID = (grid.GetEditor('gvColMainAccount').GetValue() != null) ? grid.GetEditor('gvColMainAccount').GetValue() : "0";
    var StrAmount = "0";

    var NetAmount = (grid.GetEditor('NetAmount').GetValue() != null) ? parseFloat(grid.GetEditor('NetAmount').GetValue()) : "0";
    var TaxAmount = (grid.GetEditor('TaxAmount').GetValue() != null) ? parseFloat(grid.GetEditor('TaxAmount').GetValue()) : "0";
    var ActualAmount = (grid.GetEditor('WithDrawl').GetValue() != null) ? parseFloat(grid.GetEditor('WithDrawl').GetValue()) : "0";
    //StrAmount = parseFloat(NetAmount) - parseFloat(TaxAmount);
    StrAmount = ActualAmount;
    if (strMainAccountID.trim() != "") {
        globalNetAmount = parseFloat(StrAmount);
        document.getElementById('setCurrentProdCode').value = MainAccountID;
        document.getElementById('HdSerialNo').value = SrlNo + 1;
        var strSrlNo = SrlNo + 1;
        SrlNo = strSrlNo;
        ctxtTaxTotAmt.SetValue(0);
        ccmbGstCstVat.SetSelectedIndex(0);
        $('.RecalculateInline').hide();
        caspxTaxpopUp.Show();
        var Amount = (Math.round(StrAmount * 100) / 100).toFixed(2);
        clblTaxProdGrossAmt.SetText(Amount);
        clblProdNetAmt.SetText(Amount);
        document.getElementById('HdProdGrossAmt').value = Amount;
        document.getElementById('HdProdNetAmt').value = Amount;
        clblTaxDiscount.SetText('0.00');

        $('.GstCstvatClass').show();
        $('.gstGrossAmount').hide();
        $('.gstNetAmount').hide();
        clblTaxableGross.SetText("");
        clblTaxableNet.SetText("");
        var shippingStCode = '';
        // shippingStCode = cbsSCmbState.GetText();

        shippingStCode = GeteShippingStateCode();
        //shippingStCode = shippingStCode.substring(shippingStCode.lastIndexOf('(')).replace('(State Code:', '').replace(')', '').trim();

        if (shippingStCode.trim() != '') {
            for (var cmbCount = 1; cmbCount < ccmbGstCstVat.GetItemCount() ; cmbCount++) {
                if (ccmbGstCstVat.GetItem(cmbCount).value.split('~')[5] != "") {
                    if (ccmbGstCstVat.GetItem(cmbCount).value.split('~')[5] == shippingStCode) {
                        if (shippingStCode == "4" || shippingStCode == "35" || shippingStCode == "26" || shippingStCode == "25" || shippingStCode == "7" || shippingStCode == "31" || shippingStCode == "34") {
                            if (ccmbGstCstVat.GetItem(cmbCount).value.split('~')[4] == 'I' || ccmbGstCstVat.GetItem(cmbCount).value.split('~')[4] == 'C' || ccmbGstCstVat.GetItem(cmbCount).value.split('~')[4] == 'S') {
                                ccmbGstCstVat.RemoveItem(cmbCount);
                                cmbCount--;
                            }
                        }
                        else {
                            if (ccmbGstCstVat.GetItem(cmbCount).value.split('~')[4] == 'I' || ccmbGstCstVat.GetItem(cmbCount).value.split('~')[4] == 'U') {
                                ccmbGstCstVat.RemoveItem(cmbCount);
                                cmbCount--;
                            }
                        }
                    } else {
                        if (ccmbGstCstVat.GetItem(cmbCount).value.split('~')[4] == 'S' || ccmbGstCstVat.GetItem(cmbCount).value.split('~')[4] == 'C' || ccmbGstCstVat.GetItem(cmbCount).value.split('~')[4] == 'U') {
                            ccmbGstCstVat.RemoveItem(cmbCount);
                            cmbCount--;
                        }
                    }
                } else {
                    ccmbGstCstVat.RemoveItem(cmbCount);
                    cmbCount--;
                }
            }
        }

        if (globalRowIndex > -1) {
            cgridTax.PerformCallback('New~' + "1");
        }
        else {
            cgridTax.PerformCallback('New~' + "1");
        }
        ctxtprodBasicAmt.SetValue(Amount);
    }
}
function calculateTotalAmount() {

    var TaxAmount = ctxtTaxTotAmt.GetValue();
    var Amount = grid.GetEditor("WithDrawl").GetValue();
    grid.batchEditApi.StartEdit(globalRowIndex, 7);
    var strNetAmount = parseFloat(TaxAmount) + parseFloat(Amount);
    var cashBankGridTaxAmount = grid.GetEditor("TaxAmount");
    cashBankGridTaxAmount.SetValue(TaxAmount);
    var NetAmountGrid = grid.GetEditor("NetAmount");
    NetAmountGrid.SetValue(strNetAmount.toFixed(2));
    c_txt_Debit.SetValue(strNetAmount.toFixed(2));
    document.getElementById('HdSerialNo').val = grid;
    if (cgridTax.GetVisibleRowsOnPage() > 0) {
        document.getElementById('HdSerialNo1').value = grid.GetEditor('SrlNo').GetText();
        grid.batchEditApi.EndEdit();
        cgridTax.batchEditApi.StartEdit(0, 4);
        cgridTax.GetEditor('Taxes_Name').SetValue(cgridTax.GetEditor('Taxes_Name').GetText() + '  ');
        cgridTax.UpdateEdit();
    }
    else {
        cgridTax.PerformCallback('SaveGst');
    }
    // LoadingPanel.Hide();
    return false;
}
//....................................................End Tax........................................

function SaveButtonClick() {
    var customerval = GetObjectID('hdfLookupCustomer').value;
    // var SchemaID = GetObjectID('hdnSchemaID').value;
    var SchemaID = cCmbScheme.GetValue();
    var IsEdit = $('#hdnMode').val();

  

    if (cbtnSaveRecords.IsVisible() == true) {

        var val = document.getElementById("CmbScheme").value;
        var Branchval = $("#ddlBranch").val();
        $("#MandatoryBillNo").hide();
        $("#MandatorysCustomer").hide();


        var ProjectCode = ctxtProject.GetText();
       
        if (SchemaID == "" && IsEdit == "Entry") {
            jAlert('Select Numbering Scheme');
        }
        else if (document.getElementById('txtBillNo').value == "") {
            $("#MandatoryBillNo").show();
            document.getElementById('txtBillNo').focus();
        }
        else if (Branchval == "0") {
            document.getElementById('ddlBranch').focus();
            jAlert('Enter Branch');
        }
        else if (customerval == "") {
            $("#MandatorysCustomer").show();
        }
        else if ($("#hdnProjectSelectInEntryModule").val() == "1" && $("#hdnProjectMandatory").val() == "1" && ProjectCode == "") {

            jAlert("Please Select Project.");
            //return false;
        }
        else
        {
            grid.batchEditApi.EndEdit();
            var Debit = parseFloat(c_txt_Debit.GetValue());

            var frontRow = 0;
            var backRow = -1;
            var IsJournal = "";
            for (var i = 0; i <= grid.GetVisibleRowsOnPage() ; i++) {
                var frontProduct = (grid.batchEditApi.GetCellValue(backRow, 'MainAccount') != null) ? (grid.batchEditApi.GetCellValue(backRow, 'MainAccount')) : "";
                var backProduct = (grid.batchEditApi.GetCellValue(frontRow, 'MainAccount') != null) ? (grid.batchEditApi.GetCellValue(frontRow, 'MainAccount')) : "";

                if (frontProduct != "" || backProduct != "") {
                    IsJournal = "Y";
                    break;
                }

                backRow--;
                frontRow++;
            }

            if (Debit == 0) {
                jAlert('Total Amount amount must be greater than zero(0)');
            }
            else if (IsJournal == "Y") {
                GetObjectID('hdfLookupCustomer').value = customerval;
                $('#hdnRefreshType').val('S');
                $("#HDstatus").val("S");
                OnAddNewClick();
                cbtnSaveRecords.SetVisible(false);
                cbtn_SaveRecords.SetVisible(false);
                grid.UpdateEdit();
            }
            else {
                jAlert('Cannot Save. You must enter atleast one Ledger to save this entry.');
            }
        }
    }
}
function SaveExitButtonClick() {

    var customerval = GetObjectID('hdfLookupCustomer').value;
    var IsEdit = $('#hdnMode').val();
    //var SchemaID = GetObjectID('hdnSchemaID').value;
    var SchemaID = cCmbScheme.GetValue();

   

    if (cbtn_SaveRecords.IsVisible() == true) {
        var val = document.getElementById("CmbScheme").value;
        var Branchval = $("#ddlBranch").val();
        $("#MandatoryBillNo").hide();
        $("#MandatorysCustomer").hide();

        var ProjectCode = ctxtProject.GetText();
      

        if (SchemaID == "" && IsEdit == "Entry") {
            jAlert('Select Numbering Scheme');
        }
        else if (document.getElementById('txtBillNo').value == "") {
            $("#MandatoryBillNo").show();
            document.getElementById('txtBillNo').focus();
        }
        else if (Branchval == "0") {
            document.getElementById('ddlBranch').focus();
            jAlert('Enter Branch');
        }
        else if (customerval == "") {
            $("#MandatorysCustomer").show();
        }
        else if ($("#hdnProjectSelectInEntryModule").val() == "1" && $("#hdnProjectMandatory").val() == "1" && ProjectCode == "") {

            jAlert("Please Select Project.");
           
        }

        else {
            grid.batchEditApi.EndEdit();
            var Debit = parseFloat(c_txt_Debit.GetValue());

            var frontRow = 0;
            var backRow = -1;
            var IsJournal = "";
            for (var i = 0; i <= grid.GetVisibleRowsOnPage() ; i++) {
                var frontProduct = (grid.batchEditApi.GetCellValue(backRow, 'MainAccount') != null) ? (grid.batchEditApi.GetCellValue(backRow, 'MainAccount')) : "";
                var backProduct = (grid.batchEditApi.GetCellValue(frontRow, 'MainAccount') != null) ? (grid.batchEditApi.GetCellValue(frontRow, 'MainAccount')) : "";

                if (frontProduct != "" || backProduct != "") {
                    IsJournal = "Y";
                    break;
                }

                backRow--;
                frontRow++;
            }

            if (Debit == 0) {
                jAlert('Total Amount  must be greater than zero(0)');
            }
            else
                if (IsJournal == "Y") {
                    GetObjectID('hdfLookupCustomer').value = customerval;
                    $('#hdnRefreshType').val('E');
                    $("#HDstatus").val("S");
                    OnAddNewClick();
                    cbtnSaveRecords.SetVisible(false);
                    cbtn_SaveRecords.SetVisible(false);
                    grid.UpdateEdit();
                }
                else {
                    jAlert('Cannot Save. You must enter atleast one Ledger to save this entry.');
                }
        }
    }
}


$(document).ready(function () {
    //Toggle fullscreen expandEntryGrid

    

    $("#expandgrid").click(function (e) {
        e.preventDefault();
        var $this = $(this);
        if ($this.children('i').hasClass('fa-expand')) {
            $this.removeClass('hovered half').addClass('full');
            $this.attr('title', 'Minimize Grid');
            $this.children('i').removeClass('fa-expand');
            $this.children('i').addClass('fa-arrows-alt');
            var gridId = $(this).attr('data-instance');
            $(this).closest('.makeFullscreen').addClass('panel-fullscreen');
            var cntWidth = $(this).parent('.makeFullscreen').width();
            var browserHeight = document.documentElement.clientHeight;
            var browserWidth = document.documentElement.clientWidth;
            grid.SetHeight(browserHeight - 150);
            grid.SetWidth(cntWidth);
        }
        else if ($this.children('i').hasClass('fa-arrows-alt')) {
            $this.children('i').removeClass('fa-arrows-alt');
            $this.removeClass('full').addClass('hovered half');
            $this.attr('title', 'Maximize Grid');
            $this.children('i').addClass('fa-expand');
            var gridId = $(this).attr('data-instance');
            $(this).closest('.makeFullscreen').removeClass('panel-fullscreen');
            var browserHeight = document.documentElement.clientHeight;
            var browserWidth = document.documentElement.clientWidth;
            grid.SetHeight(300);
            var cntWidth = $this.parent('.makeFullscreen').width();
            grid.SetWidth(cntWidth);
        }
    });
});

//Hierarchy Start Tanmoy
function clookup_Project_LostFocus() {
    // grid.batchEditApi.StartEdit(-1, 2);            
    var projID = clookup_Project.GetValue();
    if (projID == null || projID == "") {
        $("#ddlHierarchy").val(0);
    }
}
function Project_gotFocus() {
    //if ($("#hdnProjectSelectInEntryModule").val() == "1")
        clookup_Project.gridView.Refresh();
    clookup_Project.ShowDropDown();
}
function ProjectValueChange(Name) {
    //debugger;
    var projID = Name;
        //clookup_Project.GetValue();

    $.ajax({
        type: "POST",
        url: 'VendorDrCrNoteAdd.aspx/getHierarchyID',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ ProjID: projID }),
        async:false,
        success: function (msg) {
            var data = msg.d;
            $("#ddlHierarchy").val(data);
        }
    });
}
//Hierarchy End Tanmoy
function deleteAllRows() {
    var frontRow = 0;
    var backRow = -1;
    for (var i = 0; i <= grid.GetVisibleRowsOnPage() + 100 ; i++) {
        grid.DeleteRow(frontRow);
        grid.DeleteRow(backRow);
        backRow--;
        frontRow++;
    }

}
