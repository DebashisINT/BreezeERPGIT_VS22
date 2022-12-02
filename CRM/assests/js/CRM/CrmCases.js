function ContractStartCallback(s, e) {

    e.customArgs["Customer_ID"] = $('#CustomerId').val();
    
   
}

function ParentCaseStartCallback(s, e) {

    e.customArgs["Customer_ID"] = $('#CustomerId').val();
    e.customArgs["CASE_ID"] = $('#hdnCase_Id').val();
}



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
    $('#CustomerId').val(id);
    $("#hdnCustomerID").val(id);
    if (key != null && key != '') {
        $('#CustModel').modal('hide');
        CustomerTxt.SetText(Name);
        CustomerTxt.SetFocus();
        var obj = {};
        gridLookupcontact.SetValue(null);
        gridLookupParentCase.SetValue(null);
        obj.Customer_ID = $('#CustomerId').val();
        $.ajax({
            type: "POST",
            url: "../CRMCases/PopulateSummaryContactGrid",
            data: JSON.stringify(obj),
            async: false,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {

            }
        });

        obj.Customer_ID = $('#CustomerId').val();
        $.ajax({
            type: "POST",
            url: "../CRMCases/PopulateSummaryParentCaseGrid",
            data: JSON.stringify(obj),
            async: false,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {

            }
        });



        var objAddr = {};
        objAddr.Customer_ID = $('#CustomerId').val();
        $.ajax({
            type: "POST",
            url: "../CRMCases/SetAddressDetailsBasedInCustomer",
            data: JSON.stringify(objAddr),
            async: false,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {

                if(response !=null)
                {
                    cAddress1.SetText(response.ADDRESS1);
                    cAddress2.SetText(response.ADDRESS2);
                    cAddress3.SetText(response.ADDRESS3);
                    cLandMark.SetText(response.LANDMARK);
                    cpin.SetText(response.PIN_Code);
                    ccity.SetText(response.CITY_Name);
                    cstate.SetText(response.STATE_Name);
                    ccountry.SetText(response.COUNTRY_Name);
                    $("#hdnpinid").val(response.PIN_ID);
                    $("#hdncountryid").val(response.COUNTRY_ID);
                    $("#hdnstateid").val(response.STATE_ID);
                    $("#hdncityid").val(response.CITY_ID);
                   
                }

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


function ValidationServicecontact() {
    var isvalid = false;
    if (CustomerTxt.GetText() == "") {
        isvalid = false;
        jAlert("Account/Customer is Mandatory"); return;
    }
    else if (cCaseTitle.GetText() == "") {
        isvalid = false;
        jAlert("Case Title is Mandatory"); return;
    }
    else {
        isvalid = true;
    }

    return isvalid;
}

function SaveCases() {
    if (ValidationServicecontact()) {
        var obj = {};
        if ($("#hdnCase_Id").val() != "" && $("#hdnCase_Id").val() != "0") {
            obj.CASE_ID = $("#hdnCase_Id").val();
            obj.Action = "Update";

        }
        else {
            obj.CASE_ID = "0";
            obj.Action = "Add";

        }

        obj.TITLE = cCaseTitle.GetText();
        obj.CODE = cCaseID.GetText();
        obj.SUBJECTS = cSubject.GetText();
        obj.CUSTOMER_ID = $("#hdnCustomerID").val();
        obj.ORIGIN_ID = $("#OriginId").val();
        //obj.CONTACT = cContact.GetText();
        var contacts = gridLookupcontact.GetSelectedKeyFieldValues();
        obj.crmcontacts_id = contacts.join();
         var ParentCase = gridLookupParentCase.GetSelectedKeyFieldValues();
         obj.crmParentCase_id = ParentCase.join(); //$("#CaseListId").val(); //ParentCase.join();
        obj.RESPONSED_BY = $("#RESPONSED_BY").val();
        obj.RESOLVED_BY = $("#RESOLVED_BY").val();
        obj.ResponseSent = $("#ResponseSent").val();
        obj.ADDRESS1 = cAddress1.GetText();
        obj.ADDRESS2 = cAddress2.GetText();
        obj.ADDRESS3 = cAddress3.GetText();
        obj.LANDMARK = cLandMark.GetText();
        obj.PIN_ID = $('#hdnpinid').val();
        obj.CITY_ID = $('#hdncityid').val();
        obj.STATE_ID = $('#hdnstateid').val();
        obj.COUNTRY_ID = $('#hdncountryid').val();
        obj.OWNER_ID = $('#OWNER_ID').val();
        obj.ASSIGNED_ID = $('#ASSIGNED_ID').val();
        obj.STATUS_ID = $("#ddlStatusId").val();
        obj.STATUS_DATE = cStatusDate.GetText();
        obj.EST_CLOSE_DATE = cEstCloseDate.GetText();
        obj.EsCalatedTo = $("#EsCalatedTo").val();
        obj.Escalated_DATE = cEscalatedDate.GetText();


        LoadingPanel.Show();
        $.ajax({
            type: "POST",
            url: "../CRMCases/SaveCase",
            data: JSON.stringify(obj),
            async: true,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                //console.log(response);
                LoadingPanel.Hide();
                jAlert(response);
                AddServiceContact();
                gridcrmCases.Refresh();
                gridcrmCases.Refresh();
                $("#addcases").modal('hide');

            },
            error: function (response) {
                jAlert("Please try again later");
                LoadingPanel.Hide();
            }
        });
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
            url: "../CRMCases/AddProduct",
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


function DeleteData(values) {
    $.ajax({
        type: "POST",
        url: "../CRMCases/DeleteData",
        data: { HiddenID: values },
        success: function (response) {
            if (response != null) {
                GetServiceProductEntryList.Refresh();
                GetServiceProductEntryList.Refresh();
            }
        }
    });
}

function AddServiceContact() {

   
    $("#OWNER_ID").val($("#hdnOwnerAssignID").val());
    $("#ASSIGNED_ID").val($("#hdnOwnerAssignID").val());

     $("#hdnCase_Id").val('');
  
    cCaseTitle.SetText("");
    cCaseID.SetText("");
    cSubject.SetText("");
    $("#hdnCustomerID").val('');
    $('#CustomerId').val('');
    $("#OriginId").val();
   CustomerTxt.SetText("");
   gridLookupcontact.SetValue(null);
   gridLookupParentCase.SetValue(null);

     $("#CaseListId").val(""); 
 $("#RESPONSED_BY").val("");
    $("#RESOLVED_BY").val("");
  $("#ResponseSent").val("");
  cAddress1.SetText("");
  cAddress2.SetText("");
  cAddress3.SetText("");
  cLandMark.SetText("");
     $('#hdnpinid').val("");
    $('#hdncityid').val("");
   $('#hdnstateid').val("");
  $('#hdncountryid').val("");
   //$('#OWNER_ID').val("");
   // $('#ASSIGNED_ID').val("");
   $("#ddlStatusId").val("");
   cStatusDate.Clear();
   cEstCloseDate.Clear();
     $("#EsCalatedTo").val("");
     cEscalatedDate.Clear();
     cpin.SetText("");
     ccity.SetText("");
     cstate.SetText("");
     ccountry.SetText("");
     gridcrmCases.Refresh();
     gridcrmCases.Refresh();
 var link = document.getElementById('btnsave');
    link.style.display = 'inline-block';
    var ProductAdd = document.getElementById('ProductAdd');
    ProductAdd.style.display = 'inline-block';
    $('#SummaryTabLink').addClass("active");
    $('#DetailsTabLink').removeClass("active");
    $('#tab_a').addClass("active");
    $('#tab_b').removeClass("active");

    $.ajax({
        type: "POST",
        url: "../CRMCases/AddModeServiceContact",
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


function CancelServiceContact() {
    CloseHeadModal();
}

function CloseHeadModal() {
    $("#hdnCase_Id").val('');

    cCaseTitle.SetText("");
    cCaseID.SetText("");
    cSubject.SetText("");
    $("#hdnCustomerID").val('');
    $('#CustomerId').val('');
    $("#OriginId").val();
    CustomerTxt.SetText("");
    gridLookupcontact.SetValue(null);
    gridLookupParentCase.SetValue(null);
    gridcrmCases.Refresh();
    gridcrmCases.Refresh();
    $("#CaseListId").val("");
    $("#RESPONSED_BY").val("");
    $("#RESOLVED_BY").val("");
    $("#ResponseSent").val("");
    cAddress1.SetText("");
    cAddress2.SetText("");
    cAddress3.SetText("");
    cLandMark.SetText("");
    $('#hdnpinid').val("");
    $('#hdncityid').val("");
    $('#hdnstateid').val("");
    $('#hdncountryid').val("");
    $('#OWNER_ID').val("");
    $('#ASSIGNED_ID').val("");
    $("#ddlStatusId").val("");
    cStatusDate.Clear();
    cEstCloseDate.Clear();
    $("#EsCalatedTo").val("");
    cEscalatedDate.Clear();
    cpin.SetText("");
    ccity.SetText("");
    cstate.SetText("");
    ccountry.SetText("");

    var link = document.getElementById('btnsave');
    link.style.display = 'inline-block';
    var ProductAdd = document.getElementById('ProductAdd');
    ProductAdd.style.display = 'inline-block';
    $('#SummaryTabLink').addClass("active");
    $('#DetailsTabLink').removeClass("active");
    $('#tab_a').addClass("active");
    $('#tab_b').removeClass("active");

    $.ajax({
        type: "POST",
        url: "../CRMCases/AddModeServiceContact",
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
function ViewClick(id) {

    $("#hdnCase_Id").val(id);
    var obj = {};
    obj.CASE_ID = id;
    LoadingPanel.Show();
    $.ajax({
        type: "POST",
        url: "../CRMCases/EditServiceContact",
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
            $("#hdnCase_Id").val(response.CASE_ID)
            cCaseTitle.SetText(response.TITLE);
            cCaseID.SetText(response.CODE);
            cSubject.SetText(response.SUBJECTS);
           
            $("#OriginId").val(response.ORIGIN_ID);

            $("#CaseListId").val(response.CaseListId);
            $("#RESPONSED_BY").val(response.RESPONSED_BY);
            $("#RESOLVED_BY").val(response.RESOLVED_BY);
            $("#ResponseSent").val(response.ResponseSent);
            cAddress1.SetText(response.ADDRESS1);
            cAddress2.SetText(response.ADDRESS2);
            cAddress3.SetText(response.ADDRESS3);
            cLandMark.SetText(response.LANDMARK);

            CustomerTxt.SetText(response.Customer);
            $('#hdnCustomerID').val(response.Customer_ID);
            $('#CustomerId').val(response.Customer_ID);
            cpin.SetText(response.PIN_Code);
            ccity.SetText(response.CITY_Name);
            cstate.SetText(response.STATE_Name);
            ccountry.SetText(response.COUNTRY_Name);

            $('#hdnpinid').val(response.PIN_ID);
            $('#hdncityid').val(response.CITY_ID);
            $('#hdnstateid').val(response.STATE_ID);
            $('#hdncountryid').val(response.COUNTRY_ID);
            $('#OWNER_ID').val(response.OWNER_ID);
            $('#ASSIGNED_ID').val(response.ASSIGNED_ID);
            $("#ddlStatusId").val(response.STATUS_ID);
            if (response.STATUS_DATE != null) {
                var date = new Date(response.STATUS_DATE);
                cStatusDate.SetDate(date);
            }
            if (response.EST_CLOSE_DATE != null) {
                var RenewalStartDate = new Date(response.EST_CLOSE_DATE);
                cEstCloseDate.SetDate(RenewalStartDate);
            }
            if (response.EsCalatedTo != null) {
                var RenewalEndDate = new Date(response.EsCalatedTo);
                cEscalatedDate.SetDate(RenewalEndDate);
            }
            $("#EsCalatedTo").val(response.EsCalatedTo);

            gridLookupcontact.GetGridView().SelectRowsByKey(response.cntids);
          

            GetServiceProductEntryList.Refresh();
            GetServiceProductEntryList.Refresh();
            $('#SummaryTabLink').addClass("active");
            $('#DetailsTabLink').removeClass("active");
            $('#tab_a').addClass("active");
            $('#tab_b').removeClass("active");

            $("#addcases").modal('show');
            setTimeout(function () {
                gridLookupParentCase.GetGridView().SelectRowsByKey(response.ParentCaseids);
            },600);
          
        },
        error: function (response) {
            jAlert("Can not Edit");
            LoadingPanel.Hide();
        }
    });


}

function EditClick(id) {

    $("#hdnCase_Id").val(id);
    var obj = {};
    obj.CASE_ID = id;
    LoadingPanel.Show();
    $.ajax({
        type: "POST",
        url: "../CRMCases/EditServiceContact",
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
            $("#hdnCase_Id").val(response.CASE_ID)
            cCaseTitle.SetText(response.TITLE);
            cCaseID.SetText(response.CODE);
            cSubject.SetText(response.SUBJECTS);
            
             $("#OriginId").val(response.ORIGIN_ID);
          
            $("#CaseListId").val(response.CaseListId);
             $("#RESPONSED_BY").val(response.RESPONSED_BY);
             $("#RESOLVED_BY").val(response.RESOLVED_BY);
             $("#ResponseSent").val(response.ResponseSent);
             cAddress1.SetText(response.ADDRESS1);
             cAddress2.SetText(response.ADDRESS2);
             cAddress3.SetText(response.ADDRESS3);
             cLandMark.SetText(response.LANDMARK);
             CustomerTxt.SetText(response.Customer);
             $('#hdnCustomerID').val(response.Customer_ID);
             $('#CustomerId').val(response.Customer_ID);
             cpin.SetText(response.PIN_Code);
             ccity.SetText(response.CITY_Name);
             cstate.SetText(response.STATE_Name);
             ccountry.SetText(response.COUNTRY_Name);
             $('#hdnpinid').val(response.PIN_ID);
             $('#hdncityid').val(response.CITY_ID);
             $('#hdnstateid').val(response.STATE_ID);
            $('#hdncountryid').val(response.COUNTRY_ID);
             $('#OWNER_ID').val(response.OWNER_ID);
            $('#ASSIGNED_ID').val(response.ASSIGNED_ID);
            $("#ddlStatusId").val(response.STATUS_ID);
            if (response.STATUS_DATE != null) {
                var date = new Date(response.STATUS_DATE);
                cStatusDate.SetDate(date);
            }
            if (response.EST_CLOSE_DATE != null) {
                var RenewalStartDate = new Date(response.EST_CLOSE_DATE);
                cEstCloseDate.SetDate(RenewalStartDate);
            }
            if (response.EsCalatedTo != null) {
                var RenewalEndDate = new Date(response.EsCalatedTo);
                cEscalatedDate.SetDate(RenewalEndDate);
            }
             $("#EsCalatedTo").val(response.EsCalatedTo);
           
            gridLookupcontact.GetGridView().SelectRowsByKey(response.cntids);
            gridLookupParentCase.GetGridView().SelectRowsByKey(response.ParentCaseids);

            GetServiceProductEntryList.Refresh();
            GetServiceProductEntryList.Refresh();
            $('#SummaryTabLink').addClass("active");
            $('#DetailsTabLink').removeClass("active");
            $('#tab_a').addClass("active");
            $('#tab_b').removeClass("active");

            $("#addcases").modal('show');
        },
        error: function (response) {
            jAlert("Can not Edit");
            LoadingPanel.Hide();
        }
    });


}

function DeleteClick(Id) {
    jConfirm('Confirm delete?', 'Confirmation Dialog', function (r) {
        if (r == true) {
            $.ajax({
                type: "POST",
                url: "../CRMCases/DeleteServiceContact",
                data: JSON.stringify({ Id: Id }),
                async: false,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    jAlert("Data Delete Successfully.");
                    gridcrmCases.Refresh();
                    gridcrmCases.Refresh();
                },
                error: function (response) {
                    jAlert("Please try again later.");

                }
            });
        }
    });


}


function ServiceGridSetAmount(s, e) {
    var Price = cProductPrice.GetText();
    var Qty = cProductQty.GetText();
    if (Price != "" && Qty != "") {

        var amt = parseFloat((parseFloat(Qty).toFixed(2)) * (parseFloat(Price).toFixed(2))).toFixed(2);
        cProductAmount.SetText(amt);
    }
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

