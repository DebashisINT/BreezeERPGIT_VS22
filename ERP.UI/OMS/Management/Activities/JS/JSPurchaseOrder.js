
function fn_PopOpen()
{
    var url = '/OMS/management/Store/Master/ProductPopup.html?var=4.9';
    cPosView.SetContentUrl(url);
    cPosView.RefreshContentUrl();

    cPosView.Show();
          
}

       
function  fn_productSave()
{
    cPosView.Hide();
}
$(function () {
    $('#UOMModal').on('hide.bs.modal', function () {
        if ($("#hddnMultiUOMSelection").val() == "0") {
            grid.batchEditApi.StartEdit(globalRowIndex, 7);
        }
    });
});

var _ComponentDetails;
function gridProducts_EndCallback(s, e) {
    if (cgridproducts.cpComponentDetails) {
        _ComponentDetails=cgridproducts.cpComponentDetails;
        cgridproducts.cpComponentDetails = null;

        clookup_Project.gridView.Refresh();
        var  _cpProjectID=_ComponentDetails.split('~')[2];
        clookup_Project.gridView.SelectItemsByKey(_cpProjectID);
        //if (_cpProjectID>0) {
        //    //clookup_Project.gridView.SetEnabled=false;
        //    clookup_Project.SetEnabled(false);
        //}
        //else {
        //    clookup_Project.SetEnabled(true);
        //}

        var projID = clookup_Project.GetValue();

        $.ajax({
            type: "POST",
            url: 'PurchaseOrder.aspx/getHierarchyID',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ ProjID: projID }),
            success: function (msg) {
                var data = msg.d;
                $("#ddlHierarchy").val(data);
            }
        });
    }
}
//==========================================================Revision History ============================================================================================
//    1.0   Priti   V2.0.36  17-01-2023   0025582:Error while creating Purchase Order by Tagging Indent.
//========================================== End Revision History =======================================================================================================--%>

function taggingListKeyDown(s, e) {
    if (e.htmlEvent.key == "Enter") {
        s.OnButtonClick(0);
    }
}

function taggingListButnClick(s, e) {
        
    ctaggingGrid.PerformCallback('BindComponentGrid');
    cpopup_taggingGrid.Show();
}

function selectValueForRadioBtn() {
    //Rev  1.0
    var key = GetObjectID('hdnCustomerId').value;
    //Rev  1.0 END
    var checked = $("[id$='rdl_Salesquotation']").find(":checked").val();
    if (checked=="Indent" || checked=="Quotation") {
        ctaggingList.SetEnabled(true);
    }
    else
    {
        ctaggingList.SetEnabled(false);
    }

    if ($('#ddlInventory').val() != 'Y')
    {
        return;
    }
    if (key == null || key == "") {
        jAlert("Customer required !", 'Alert Dialog: [Quoation]', function (r) {
            if (r == true) {
                ctxtVendorName.Focus();
               // gridquotationLookup.SetEnabled(false);
                $('input[type=radio]').prop('checked', false);
            }
        });

        return;

    }

              
}

function closeMultiUOM(s, e) {
    e.cancel = false;   
}

$(function () {
    $(".allownumericwithdecimal").on("keypress keyup blur", function (event) {     
        $(this).val($(this).val().replace(/[^0-9\.]/g, ''));
        if ((event.which != 46 || $(this).val().indexOf('.') != -1) && (event.which < 48 || event.which > 57)) {
            event.preventDefault();
        }
    });
});

function SaveMultiUOM() {        

    var qnty = $("#UOMQuantity").val();


    var UomId = ccmbUOM.GetValue();
    //var UomId = ccmbUOM.SetSelectedIndex(grid.GetEditor('ProductID').GetText().split("||@||")[3] - 1);
    var UomName = ccmbUOM.GetText();
    //var AltQnty = parseFloat($("#AltUOMQuantity").val()).toFixed(4);
    var AltQnty = cAltUOMQuantity.GetValue();
    var AltUomId = ccmbSecondUOM.GetValue();
    var AltUomName = ccmbSecondUOM.GetText();

    // Rev Mantis Issue 24429
    grid.batchEditApi.StartEdit(globalRowIndex);
    // End of Rev Mantis Issue 24429

    var srlNo = (grid.GetEditor('SrlNo').GetValue() != null) ? grid.GetEditor('SrlNo').GetValue() : "";
    var Productdetails = (grid.GetEditor('gvColProduct').GetText() != null) ? grid.GetEditor('gvColProduct').GetText() : "0";
    var ProductID = Productdetails.split("||@||")[0];
    var DetailsId = grid.GetEditor('DetailsId').GetText();
    // Mantis Issue 24429
    var BaseRate = ccmbBaseRate.GetValue();
    var AltRate = ccmbAltRate.GetValue();

    var UpdateRow = 'False';
    if($("#chkUpdateRow").prop("checked")){
        UpdateRow='True';
    }
    if(DetailsId==null || DetailsId=="" || DetailsId==undefined){
        DetailsId=0;
    }
    // End of Mantis Issue 24429
    // Mantis Issue 24429
    //if (srlNo != "" && qnty != "0.0000" && UomId != "" && UomName != "" && AltUomId != "" && ProductID != "" && AltUomId != null && AltUomName != "") {
    if (srlNo != "" && UomId != "" && UomName != "" && AltUomId != "" && ProductID != "" && AltUomId != null && AltUomName != "" && AltQnty!="0.0000") {
        if ((qnty != "0" && UpdateRow == 'True') || (qnty == "0" && UpdateRow == 'False') || (qnty != "0" && UpdateRow == 'False')) {
            //End Rev Mantis Issue 24429
           
            
            // Mantis Issue 24429
            ////cgrid_MultiUOM.PerformCallback('SaveDisplay~' + srlNo + '~' + qnty + '~' + UomName + '~' + AltUomName + '~' + AltQnty + '~' + UomId + '~' + AltUomId + '~' + ProductID + '~' + DetailsId);
            //    cgrid_MultiUOM.PerformCallback('SaveDisplay~' + srlNo + '~' + qnty + '~' + UomName + '~' + AltUomName + '~' + AltQnty + '~' + UomId + '~' + AltUomId + '~' + ProductID + '~'+ DetailsId + '~' + BaseRate + '~'+ AltRate + '~' + UpdateRow);
            //    // End of Mantis Issue 24429
            ////$("#AltUOMQuantity").val(parseFloat(0).toFixed(4));
            //cAltUOMQuantity.SetValue("0.0000");
            //    // Mantis Issue 24429
            //$("#UOMQuantity").val(0);
            //ccmbBaseRate.SetValue(0) 
            //cAltUOMQuantity.SetValue(0)
            //ccmbAltRate.SetValue(0)
            //ccmbSecondUOM.SetValue("")
            if (cbtnMUltiUOM.GetText() == "Update") {
                //alert("Update")
                cgrid_MultiUOM.PerformCallback('UpdateRow~' + srlNo + '~' + qnty + '~' + UomName + '~' + AltUomName + '~' + AltQnty + '~' + UomId + '~' + AltUomId + '~' + ProductID+ '~' + DetailsId  + '~' + BaseRate + '~' + AltRate + '~' + UpdateRow +'~' + hdMultiUOMID);
                //$("#AltUOMQuantity").val(parseFloat(0).toFixed(4));
                cAltUOMQuantity.SetValue("0.0000");
                // Mantis Issue 24428
                $("#UOMQuantity").val(0);
                ccmbBaseRate.SetValue(0);
                cAltUOMQuantity.SetValue(0);
                ccmbAltRate.SetValue(0);
                ccmbSecondUOM.SetValue("");
                cgrid_MultiUOM.cpAllDetails = "";
                cbtnMUltiUOM.SetText("Add");
                // Rev Mantis Issue 24429
                $("#chkUpdateRow").prop('checked', false);
                $("#chkUpdateRow").removeAttr("checked");
                // End of Rev Mantis Issue 24429


            }
            else{
                cgrid_MultiUOM.PerformCallback('SaveDisplay~' + srlNo + '~' + qnty + '~' + UomName + '~' + AltUomName + '~' + AltQnty + '~' + UomId + '~' + AltUomId + '~' + ProductID + '~' + DetailsId + '~' + BaseRate + '~'+ AltRate + '~' + UpdateRow);
                cAltUOMQuantity.SetValue("0.0000");
                $("#UOMQuantity").val(0);
                ccmbBaseRate.SetValue(0) 
                cAltUOMQuantity.SetValue(0)
                ccmbAltRate.SetValue(0)
                ccmbSecondUOM.SetValue("")   
                // Rev Mantis Issue 24429
                $("#chkUpdateRow").prop('checked', false);
                $("#chkUpdateRow").removeAttr("checked");
                // End of Rev Mantis Issue 24429
            }
            // Mantis Issue 24429
        // End of Mantis Issue 24429      
        }
        else {
            return;
        }
        // End of Mantis Issue 24429
    }
    else {
        return;
    }
}

function Delete_MultiUom(keyValue, SrlNo, DetailsId) {


    cgrid_MultiUOM.PerformCallback('MultiUomDelete~' + keyValue + '~' + SrlNo + '~' + DetailsId);

}

function OnMultiUOMEndCallback(s, e) {
    if (cgrid_MultiUOM.cpDuplicateAltUOM == "DuplicateAltUOM") {
        jAlert("Please Enter Different Alt. Quantity.");
        return;
    }
    // Mantis Issue 24429
    if(cgrid_MultiUOM.cpSetBaseQtyRateInGrid != null && cgrid_MultiUOM.cpSetBaseQtyRateInGrid=="1")
    {
        grid.batchEditApi.StartEdit(globalRowIndex, 6);

        var BaseQty = cgrid_MultiUOM.cpBaseQty ;
        var BaseRate = cgrid_MultiUOM.cpBaseRate ;

        // Mantis Issue 24429
        var AltQuantity = cgrid_MultiUOM.cpAltQuantity ;
        var AltUOM = cgrid_MultiUOM.cpAltUOM ;
        // End of Mantis Issue 24429

        grid.GetEditor("gvColQuantity").SetValue(BaseQty);
        grid.GetEditor("gvColStockPurchasePrice").SetValue(BaseRate);
        grid.GetEditor("gvColAmount").SetValue(BaseQty*BaseRate);
        grid.GetEditor("gvColTotalAmountINR").SetValue(BaseQty*BaseRate);
        // Mantis Issue 24429
        grid.GetEditor("PO_AltQuantity").SetValue(AltQuantity);
        grid.GetEditor("PO_AltUOM").SetValue(AltUOM);
        // End of Mantis Issue 24429
        // Mantis Issue 24429
        PurchasePriceTextChange(null,null);
        // End of Mantis Issue 24429
    }
    // End of Mantis Issue 24429
    if (cgrid_MultiUOM.cpOpenFocus == "OpenFocus") {
        ccmbSecondUOM.SetFocus();
    }
    //Mantis Issue 24429
    if (cgrid_MultiUOM.cpAllDetails == "EditData") {
        var Quan = (cgrid_MultiUOM.cpBaseQty).toFixed(4);
        $('#UOMQuantity').val(Quan);
       // $('#UOMQuantity').val(cgrid_MultiUOM.cpBaseQty);
        ccmbBaseRate.SetValue(cgrid_MultiUOM.cpBaseRate)
        ccmbSecondUOM.SetValue(cgrid_MultiUOM.cpAltUom);
        cAltUOMQuantity.SetValue(cgrid_MultiUOM.cpAltQty);
        ccmbAltRate.SetValue(cgrid_MultiUOM.cpAltRate);
        hdMultiUOMID = cgrid_MultiUOM.cpuomid;
        //if (cgrid_MultiUOM.cpUpdatedrow == true) {
        //    $("#chkUpdateRow").attr('checked', true);
        //    $("#chkUpdateRow").attr('checked', 'checked');
       
         
        //}
        //else {
        //    $("#chkUpdateRow").prop("checked", false);
        //}
        if (cgrid_MultiUOM.cpUpdatedrow == true) {
            $("#chkUpdateRow").prop('checked', true);
            $("#chkUpdateRow").attr('checked', 'checked');


        }
        else {
            $("#chkUpdateRow").prop('checked', false);
            $("#chkUpdateRow").removeAttr("checked");
         
        }
    }
    //End of Mantis Issue 24429
}

var Uomlength = 0;
function UomLenthCalculation() {
    grid.batchEditApi.StartEdit(globalRowIndex);
    var SLNo = "";
    var val = 0;
    //Mantis Issue 24429
    //var detailsid=grid.GetEditor('DetailsId').GetValue();
    //if (detailsid != null && detailsid != "") {
    //    SLNo = detailsid;
    //    val = 1;
    //}
    //else {
    //    SLNo = grid.GetEditor('SrlNo').GetValue();
    //}
    SLNo = grid.GetEditor('SrlNo').GetValue();
    //End of Mantis Issue 24429
    $.ajax({
        type: "POST",
        url: "PurchaseOrder.aspx/GetQuantityfromSL",
        data: JSON.stringify({ SLNo: SLNo, val: val }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (msg) {

            Uomlength = msg.d;

        }
    });
}
// Mantis Issue 24429
function Edit_MultiUom(keyValue, SrlNo) {

    cbtnMUltiUOM.SetText("Update");
    cgrid_MultiUOM.PerformCallback('EditData~' + keyValue + '~' + SrlNo);

 

}
// End of Mantis Issue 24429

function FinalMultiUOM() {
    UomLenthCalculation();
    if (Uomlength == 0 || Uomlength < 0) {

        // Mantis Issue 24429
        //jAlert("Please add Alt. Quantity.");
        jAlert("Please add atleast one Alt. Quantity with Update Row as checked.");
        // End of Mantis Issue 24429
        return;
    }
    else {
        cPopup_MultiUOM.Hide();
        // Mantis Issue 24429
        var SLNo = grid.GetEditor('SrlNo').GetValue();
        cgrid_MultiUOM.PerformCallback('SetBaseQtyRateInGrid~'+ SLNo);
        // End of Mantis Issue 24429
        setTimeout(function () {
            grid.batchEditApi.StartEdit(globalRowIndex, 10);
        }, 400)
    }
}
// Mantis Issue 24429
function CalcBaseQty()
{
    //var PackingQtyAlt = Productdetails.split("||@||")[19];
    //var PackingQty = Productdetails.split("||@||")[21];
    //var PackingSaleUOM = Productdetails.split("||@||")[24];  // Alternate UOM selected from Product Master (tbl_master_product_packingDetails.packing_saleUOM)
    
    var Productdetails = (grid.GetEditor('gvColProduct').GetText() != null) ? grid.GetEditor('gvColProduct').GetText() : "0";
    var ProductID = Productdetails.split("||@||")[0];
    var PackingQtyAlt = 0;
    var PackingQty = 0;
    var PackingSaleUOM = 0;

    $.ajax({
        type: "POST",
        url: "PurchaseIndent.aspx/AutoPopulateAltQuantity",
        data: JSON.stringify({ ProductID: ProductID }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
           
            if (msg.d.length != 0) {
                PackingQtyAlt = msg.d[0].packing_quantity;  // Alternate UOM selected from Product Master (tbl_master_product_packingDetails.packing_quantity)
                PackingQty = msg.d[0].sProduct_quantity;  // Alternate UOM selected from Product Master (tbl_master_product_packingDetails.sProduct_quantity)
                PackingSaleUOM = msg.d[0].AltUOMId;  // Alternate UOM selected from Product Master (tbl_master_product_packingDetails.packing_saleUOM)
            }
            else {
                PackingQtyAlt = 0;  // Alternate UOM selected from Product Master (tbl_master_product_packingDetails.packing_quantity)
                PackingQty = 0;  // Alternate UOM selected from Product Master (tbl_master_product_packingDetails.sProduct_quantity)
                PackingSaleUOM = 0;  // Alternate UOM selected from Product Master (tbl_master_product_packingDetails.packing_saleUOM)
            }

            if(PackingQtyAlt==""){
                PackingQtyAlt=0
            }
            if(PackingQty==""){
                PackingQty=0
            }
    
            // if Base UOM of product is not same as the Alternate UOM selected from Product Master, then Calculation of Base Quantity will not happen
            if (ccmbSecondUOM.GetValue() != PackingSaleUOM)
            {
                PackingQtyAlt = 0;
                PackingQty = 0;
            }
      
            var BaseQty = 0
            if(PackingQtyAlt>0){
                var ConvFact = PackingQty/PackingQtyAlt ;
                var altQty = cAltUOMQuantity.GetValue();

                if(ConvFact>0){
                    var BaseQty = (altQty * ConvFact).toFixed(4);
                    $("#UOMQuantity").val(BaseQty);
                }
        
            }
            else {
                $("#UOMQuantity").val("0.0000");
            }
        }
    });

}

function CalcBaseRate()
{
    var altQty = cAltUOMQuantity.GetValue();
    var altRate = ccmbAltRate.GetValue();
    var baseQty = $("#UOMQuantity").val();


    if(baseQty>0){
        var BaseRate = (altQty * altRate)/baseQty ;
        ccmbBaseRate.SetValue(BaseRate);
    }
}
// End of Mantis Issue 24429

function PopulateMultiUomAltQuantity() {
           
    var otherdet = {};
    var Quantity = $("#UOMQuantity").val();
    otherdet.Quantity = Quantity;
    var UomId = ccmbUOM.GetValue();
    otherdet.UomId = UomId;
    var Productdetails = (grid.GetEditor('gvColProduct').GetText() != null) ? grid.GetEditor('gvColProduct').GetText() : "0";
    var ProductID = Productdetails.split("||@||")[0];
    otherdet.ProductID = ProductID;
    var AltUomId = ccmbSecondUOM.GetValue();
    otherdet.AltUomId = AltUomId;

    $.ajax({
        type: "POST",
        url: "PurchaseOrder.aspx/GetPackingQuantity",
        data: JSON.stringify(otherdet),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
                   
            if (msg.d.length != 0) {
                var packingQuantity = msg.d[0].packing_quantity;
                var sProduct_quantity = msg.d[0].sProduct_quantity;
            }
            else {
                var packingQuantity = 0;
                var sProduct_quantity = 0;
            }
            var uomfactor = 0
            if (sProduct_quantity != 0 && packingQuantity != 0) {
                uomfactor = parseFloat(packingQuantity / sProduct_quantity).toFixed(4);
                $('#hddnuomFactor').val(parseFloat(packingQuantity / sProduct_quantity));
            }
            else {
                $('#hddnuomFactor').val(0);
            }

            var uomfac_Qty_to_stock = $('#hddnuomFactor').val();
            var Qty = $("#UOMQuantity").val();
            var calcQuantity = parseFloat(Qty * uomfac_Qty_to_stock).toFixed(4);

            //$("#AltUOMQuantity").val(calcQuantity);

            cAltUOMQuantity.SetValue(calcQuantity);

        }
    });
}


function ParentCustomerOnClose(newCustId, customerName, Unique) {
           

    GetObjectID('hdnCustomerId').value = newCustId;

    AspxDirectAddCustPopup.Hide();
    ctxtShipToPartyShippingAdd.SetText('');
    if (newCustId != "") {
        ctxtVendorName.SetText(customerName);
        SetCustomer(newCustId, customerName);
    }
           
}
       
function AddVendorClick() {
           
    //var isLighterPage = $("#hidIsLigherContactPage").val();
    //// alert(isLighterPage);
    //if (isLighterPage == 1) {
    var url = '/OMS/management/Master/vendorPopup.html?var=1.1.5.7';
    AspxDirectAddCustPopup.SetContentUrl(url);
           
    AspxDirectAddCustPopup.RefreshContentUrl();
    AspxDirectAddCustPopup.Show();
    //}
    //else {
    //    var url = '/OMS/management/Master/HRrecruitmentagent_general.aspx?id=' + 'ADD';
    //    window.location.href = url;
    //    //AspxDirectAddCustPopup.SetContentUrl(url);
           
    //    //AspxDirectAddCustPopup.RefreshContentUrl();
    //    //AspxDirectAddCustPopup.Show();

    //}
            
}

function Tag_ChangeState(value) {
    ctaggingGrid.PerformCallback('SelectAndDeSelectProducts' + '~' + value);
}



var GlobalAllAddress = [];
function VendorButnClick(s, e) {
    var txt = "<table border='1' width=\"100%\" class=\"dynamicPopupTbl\"><tr class=\"HeaderStyle\"><th>Vendor Name</th><th>Unique Id</th></tr><table>";
    document.getElementById("CustomerTable").innerHTML = txt;
    setTimeout(function () { $("#txtCustSearch").focus(); }, 500);
    $('#txtCustSearch').val('');
    $('#CustModel').modal('show');
}
function VendorKeyDown(s, e) {
    if (e.htmlEvent.key == "Enter"|| e.code == "NumpadEnter") {
        s.OnButtonClick(0);
    }
}
function VendorModekkeydown(e) {
    var OtherDetails = {}
    OtherDetails.SearchKey = $("#txtCustSearch").val();
    OtherDetails.BranchID = $('#ddl_Branch').val();
    

    if (e.code == "Enter" || e.code == "NumpadEnter") {
        var HeaderCaption = [];
        HeaderCaption.push("Vendor Name");
        HeaderCaption.push("Unique Id");
        if($("#txtCustSearch").val()!="")
        {
            //callonServer("Services/Master.asmx/GetVendorWithOutBranch", OtherDetails, "CustomerTable", HeaderCaption, "customerIndex", "SetCustomer");
            callonServer("Services/Master.asmx/GetVendorWithBranchPO", OtherDetails, "CustomerTable", HeaderCaption, "customerIndex", "SetCustomer");
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
        ctxtVendorName.SetText(Name);      
        GetObjectID('hdnCustomerId').value = Id;
        $('#MandatorysVendor').attr('style', 'display:none');
        var VendorId=Id;
        GetVendorGSTInFromBillShip(VendorId);
        
        //  GetContactPerson();
        $('#CustModel').modal('hide');
        cContactPerson.Focus();
        $.ajax({
            type: "POST",
            url: "PurchaseOrder.aspx/GetVendorReletedData",
            data: JSON.stringify({ VendorId: VendorId }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                var data = msg.d;                       
                var strDueDate = data.toString().split('~')[0];
                var strcountryId = data.toString().split('~')[1];
                var strOutstanding = data.toString().split('~')[2];
                var strGSTN = data.toString().split('~')[3];

                if (strDueDate != null) {
                    var DeuDate = strDueDate;
                    var myDate = new Date(DeuDate);
                    var invoiceDate = new Date();
                    var datediff = Math.round((myDate - invoiceDate) / (1000 * 60 * 60 * 24));
                    ctxtCreditDays.SetValue(datediff);
                    cdt_PODue.SetDate(myDate);                            
                }
                if (strcountryId != null && strcountryId != "") {
                    var CountryID = strcountryId;
                    if (CountryID != "1") {
                        cddl_AmountAre.SetValue("4");
                        cddl_AmountAre.SetEnabled(false);
                        grid.GetEditor('gvColTaxAmount').SetEnabled(true);
                    }                           
                    else
                    {
                        cddl_AmountAre.SetValue("1");
                        cddl_AmountAre.SetEnabled(true);                        
                    }
                    if (CountryID == "0")
                    {
                                
                        jAlert('You must enter the default Billing/Shipping Address for selected Vendor to proceed further.');
                        ctxtVendorName.SetText("");            
                        GetObjectID('hdnCustomerId').value = "";
                        cddl_AmountAre.SetValue("1");
                        cddl_AmountAre.SetEnabled(true);
                        ctxtVendorName.Focus();
                    }
                    else
                    {
                        GetPurchaseForGstValue();  
                    }
                    SetEntityType(VendorId)
                }
                else
                {
                    cddl_AmountAre.SetValue("1");
                    cddl_AmountAre.SetEnabled(true);
                }

                if (strOutstanding != null) {
                    pageheaderContent.style.display = "block";
                    $("#divOutstanding").attr('style', 'display:block');
                    document.getElementById('lblTotalPayable').innerHTML = strOutstanding;
                }
                else {
                    pageheaderContent.style.display = "none";
                    $("#divOutstanding").attr('style', 'display:none');
                    document.getElementById('lblTotalPayable').innerHTML = '';
                }
                if (strGSTN != null) {
                    $("#divGSTIN").attr('style', 'display:block');
                    document.getElementById('lblGSTIN').innerHTML = strGSTN;
                }
                GetContactPerson();                        
            }
        });
        var VendorId = $('#hdnCustomerId').val();         

        if (VendorId != null && VendorId != "") {         
                
            //cContactPerson.PerformCallback('BindContactPerson~' + VendorId);
            var OtherDetails = {}
            OtherDetails.VendorId = VendorId;
            $.ajax({
                type: "POST",
                url: "PurchaseOrder.aspx/GetContactPerson",
                data: JSON.stringify(OtherDetails),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var returnObject = msg.d;                  
                    if (returnObject) {
                        SetDataSourceOnComboBox(cContactPerson, returnObject);
                    }
                }
            });

            if ($("#btn_TermsCondition").is(":visible")) {
                callTCspecefiFields_PO(VendorId);
            }
        }
    }
}

function SetDataSourceOnComboBox(ControlObject, Source) {
    ControlObject.ClearItems();
    for (var count = 0; count < Source.length; count++) {
        ControlObject.AddItem(Source[count].Name, Source[count].Id);
    }
    ControlObject.SetSelectedIndex(0);
}
function GetContactPerson() {
    var key = GetObjectID('hdnCustomerId').value;
    if (GetObjectID('hdnCustomerId').value != null && GetObjectID('hdnCustomerId').value != '') {
        page.GetTabByName('Billing/Shipping').SetEnabled(true);        
        page.tabs[0].SetEnabled(true);
        page.tabs[1].SetEnabled(true);
        //GetVendorGSTInFromBillShip(key);
    }
}

function IfVendorGstInIsBlank()
{
    if( cddl_AmountAre.GetValue() != "4"){

        cddl_AmountAre.SetValue("3");
        PopulateGSTCSTVAT();
        cddl_AmountAre.SetEnabled(false);
    }
}

function AfterSaveBillingShipiing(validate) {
    GetPurchaseForGstValue();
    if (validate) {
        page.SetActiveTabIndex(0);
        page.tabs[0].SetEnabled(true);
        $("#divcross").show();
    }
    else {
        page.SetActiveTabIndex(1);
        page.tabs[0].SetEnabled(false);
        $("#divcross").hide();
    }
}

function GetPurchaseForGstValue()
{
           
    cPurchaseOrderPosGst.ClearItems();
    if(cPurchaseOrderPosGst.GetItemCount()==0)
    {
        cPurchaseOrderPosGst.AddItem(GetShippingStateName() + '[Shipping]', "S");
        cPurchaseOrderPosGst.AddItem(GetBillingStateName() + '[Billing]', "B");
    }
            
    else  if(cPurchaseOrderPosGst.GetItemCount()>2)
    {
        cPurchaseOrderPosGst.ClearItems();
        //cddl_PosGstSalesOrder.RemoveItem(0);
        //cddl_PosGstSalesOrder.RemoveItem(0);
    }

    if(PosGstId=="" || PosGstId==null)
    {
        cPurchaseOrderPosGst.SetValue("S");
    }
    else
    {
        cPurchaseOrderPosGst.SetValue(PosGstId);
    }
}


var PosGstId="";
function PopulatePurchasePosGst(e)
{
            
    PosGstId=cPurchaseOrderPosGst.GetValue();
    if(PosGstId=="S")
    {
        cPurchaseOrderPosGst.SetValue("S");  
    }
    else if(PosGstId=="B")
    {
        cPurchaseOrderPosGst.SetValue("B"); 
    }
}

function ValueSelected(e, indexName) {
    if (e.code == "Enter"|| e.code == "NumpadEnter") {
        var Id = e.target.parentElement.parentElement.cells[0].innerText;
        var name = e.target.parentElement.parentElement.cells[1].children[0].value;
        if (Id) {
            if (indexName == "ProdIndex")
                SetProduct(Id, name);
                //Start:Chinmoy 
            else if (indexName == "BillingAreaIndex") {
                SetBillingArea(Id, name);
            }
            else if (indexName == "ShippingAreaIndex") {
                SetShippingArea(Id, name);
            }
            else if(indexName=="customeraddressIndex")
            {
                SetCustomeraddress(Id,name)
            }
                //End
            else
                SetCustomer(Id, name);
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
            if (indexName == "ProdIndex")
                $('#txtProdSearch').focus();
                //Start Chinmoy 
            else if (indexName == "BillingAreaIndex")
                $('#txtbillingArea').focus();
            else if (indexName == "ShippingAreaIndex")
                $('#txtshippingArea').focus();
            else if (indexName == "customeraddressIndex")
                ('#txtshippingShipToParty').focus();
                //End
            else
                $('#txtCustSearch').focus();
        }
    }

}



function prodkeydown(e) {
    var inventoryType = (document.getElementById("ddlInventory").value != null) ? document.getElementById("ddlInventory").value : "";

    var OtherDetails = {}
    OtherDetails.SearchKey = $("#txtProdSearch").val();
    OtherDetails.InventoryType = inventoryType;
    OtherDetails.VendorId = $("#hdnCustomerId").val();
    // console.log(e.code);
    if (e.code == "Enter" || e.code == "NumpadEnter") {
        var HeaderCaption = [];
        HeaderCaption.push("Product Code");
        HeaderCaption.push("Product Name");
        // HeaderCaption.push("Product Description");
        HeaderCaption.push("Inventory");
        HeaderCaption.push("HSN/SAC");
        HeaderCaption.push("Class");
        HeaderCaption.push("Brand");
        // HeaderCaption.push("Installation Reqd.");

        if ($("#txtProdSearch").val() != '') {
            callonServer("Services/Master.asmx/GetPurchaseProductForPONormal", OtherDetails, "ProductTable", HeaderCaption, "ProdIndex", "SetProduct");
        }
    }
    else if (e.code == "ArrowDown") {
        if ($("input[ProdIndex=0]"))
            $("input[ProdIndex=0]").focus();
    }
    else if (e.code == "Escape") {
        //  grid.StartEditRow(globalRowIndex);
        grid.batchEditApi.StartEdit(globalRowIndex, 3);
    }
}


function GlobalBillingShippingEndCallBack() {  
            
    var VendorId = $('#hdnCustomerId').val();         

    if (VendorId != null && VendorId != "") {              
                
        cContactPerson.PerformCallback('BindContactPerson~' + VendorId);
        if ($("#btn_TermsCondition").is(":visible")) {
            callTCspecefiFields_PO(VendorId);
        }
    }
            
}
       
function CreditDays_TextChanged(s, e) {                    
    var CreditDays = ctxtCreditDays.GetValue();
    var today = cPLQuoteDate.GetDate();
    var newdate = cPLQuoteDate.GetDate();
    newdate.setDate(today.getDate() + Math.round(CreditDays));
    cdt_PODue.SetDate(newdate);
}
function BackClick() {
    var keyOpening = document.getElementById('hdnOpening').value;
    if (keyOpening != '') {
        var url = 'PurchaseOrderList.aspx?op=' + 'yes';
    }
    else {
        var url = 'PurchaseOrderList.aspx';
    }
    window.location.href = url;
}
var globalRowIndex;
var rowEditCtrl;
var TaxOfProduct = [];

var _GetQuantityValue = "0";
var _GetPurchasePriceValue = "0";
var _GetDiscountValue = "0";
var _GetAmountValue = "0";

function selectValue() {
    var checked = $('#rdl_IndentRequisition').attr('checked', true);
    if (checked) {
        $(this).attr('checked', false);
    }
    else {
        $(this).attr('checked', true);
    }
    var type = ($("[id$='rdl_IndentRequisition']").find(":checked").val() != null) ? $("[id$='rdl_IndentRequisition']").find(":checked").val() : "";
    if (type != "") {
        if ($('#ddlInventory').val() == 'Y') {
            GetIndentReqNoOnLoad();
        }
    }
}
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
function onBranchItems() {
    //GetIndentReqNoOnLoad();
    grid.batchEditApi.StartEdit(-1, 1);
    var accountingDataMin = grid.GetEditor('ProductName').GetValue();
    grid.batchEditApi.EndEdit();
    grid.batchEditApi.StartEdit(0, 1);
    var accountingDataplus = grid.GetEditor('ProductName').GetValue();
    grid.batchEditApi.EndEdit();
    if (accountingDataMin != null || accountingDataplus != null) {
        jConfirm('Documents tagged are to be automatically De-selected. Confirm?', 'Confirmation Dialog', function (r) {
            if (r == true) {
                cQuotationComponentPanel.PerformCallback('BindNullGrid');

            }
        });
    }
    //Project Look up Refresh
    if ($("#hdnProjectSelectInEntryModule").val() == "1")
        clookup_Project.gridView.Refresh();
}
//............................Product Pazination..............
function ChangeState(value) {
    cgridproducts.PerformCallback('SelectAndDeSelectProducts' + '~' + value);
}
function ctaxUpdatePanelEndCall(s, e) {
    if (ctaxUpdatePanel.cpstock != null) {
        divAvailableStk.style.display = "block";
        var AvlStk = ctaxUpdatePanel.cpstock + " " + document.getElementById('lblStkUOM').innerHTML;
        document.getElementById('lblAvailableStkPro').innerHTML = AvlStk;
        document.getElementById('lblAvailableStk').innerHTML = ctaxUpdatePanel.cpstock;
        ctaxUpdatePanel.cpstock = null;
        //grid.batchEditApi.StartEdit(globalRowIndex, 6);
    }
    return false;
}

function ProductsGotFocusFromID(s, e) {
    pageheaderContent.style.display = "block";
    divAvailableStk.style.display = "block";
    var ProductID = (grid.GetEditor('gvColProduct').GetValue() != null) ? grid.GetEditor('gvColProduct').GetValue() : "0";

    var strProductName = (grid.GetEditor('gvColProduct').GetText() != null) ? grid.GetEditor('gvColProduct').GetText() : "0";
    var QuantityValue = (grid.GetEditor('gvColQuantity').GetValue() != null) ? grid.GetEditor('gvColQuantity').GetValue() : "0";
    var ddlbranch = $("[id*=ddl_Branch]");
    var strBranch = ddlbranch.find("option:selected").text();

    var SpliteDetails = ProductID.split("||@||");
    var strProductID = SpliteDetails[0];
    var strDescription = SpliteDetails[1];
    var strUOM = SpliteDetails[2];
    var strStkUOM = SpliteDetails[4];
    var strSalePrice = SpliteDetails[6];
    var IsPackingActive = SpliteDetails[13];
    var Packing_Factor = SpliteDetails[14];
    var Packing_UOM = SpliteDetails[15];
    var PackingValue = (Packing_Factor * QuantityValue) + " " + Packing_UOM;
    if (IsPackingActive == "Y" && (parseFloat(Packing_Factor * QuantityValue) > 0)) {
        $('#lblPackingStk').text(PackingValue);
    } else {
        divPacking.style.display = "none";
    }

    $('#lblStkQty').text(QuantityValue);
    $('#lblStkUOM').text(strStkUOM);
    $('#lblProduct').text(strProductName);
    $('#lblbranchName').text(strBranch);

    if (ProductID != "0") {
        cacpAvailableStock.PerformCallback(strProductID);
    }
}

function ProductKeyDown(s, e) {
    if (e.htmlEvent.key == "Enter" || e.code == "NumpadEnter") {
        s.OnButtonClick(0);
    }
    //if (e.htmlEvent.key == "Tab") {

    //    s.OnButtonClick(0);
    //}
}
function ProductButnClick(s, e) {
    var VendorID = GetObjectID('hdnCustomerId').value;
    if (VendorID != null && VendorID != "") {
        if (e.buttonIndex == 0) {
            $('#txtProdSearch').val('');
            $('#ProductModel').modal('show');
            setTimeout(function () { $("#txtProdSearch").focus(); }, 500);
        }
    }
    else {
        jAlert("Please Select a Vendor", "Alert", function () { ctxtVendorName.Focus(); });
    }

}
function SetProduct(Id, Name) {
    $('#ProductModel').modal('hide');
    var LookUpData = Id;
    var ProductCode = Name;
    if (!ProductCode) {
        LookUpData = null;
    }
    // cProductpopUp.Hide();
    grid.batchEditApi.StartEdit(globalRowIndex);
    grid.GetEditor("gvColProduct").SetText(LookUpData);


    pageheaderContent.style.display = "block";
    divAvailableStk.style.display = "block";
    cddl_AmountAre.SetEnabled(false);
    ctxtVendorName.SetEnabled(false);
    cPurchaseOrderPosGst.SetEnabled(false);
    if (document.getElementById("ddl_numberingScheme") != null) {
        document.getElementById("ddl_numberingScheme").disabled = true;
    }

    document.getElementById("ddlInventory").disabled = true;
    var tbDescription = grid.GetEditor("gvColDiscription");
    var tbUOM = grid.GetEditor("gvColUOM");
    var tbSalePrice = grid.GetEditor("gvColStockPurchasePrice");
    var ProductID = (grid.GetEditor('gvColProduct').GetText() != null) ? grid.GetEditor('gvColProduct').GetText() : "0";
    var SpliteDetails = ProductID.split("||@||");
    var strProductID = SpliteDetails[0];
    var strDescription = SpliteDetails[1];
    var Product_Name = SpliteDetails[12];
    var strUOM = SpliteDetails[2];
    var strStkUOM = SpliteDetails[4];
    var strSalePrice = SpliteDetails[6];

    var QuantityValue = (grid.GetEditor('gvColQuantity').GetValue() != null) ? grid.GetEditor('gvColQuantity').GetValue() : "0";
    var IsPackingActive = SpliteDetails[10];
    var Packing_Factor = SpliteDetails[11];
    var Packing_UOM = SpliteDetails[12];

    var strRate = (ctxtRate.GetValue() != null && ctxtRate.GetValue() != "0") ? ctxtRate.GetValue() : "1";
    if (strRate == 0) {
        strSalePrice = strSalePrice;
    }
    else {
        strSalePrice = strSalePrice / strRate;
    }

    tbDescription.SetValue(strDescription);
    tbUOM.SetValue(strUOM);
    tbSalePrice.SetValue(strSalePrice);
    grid.GetEditor("ProductName").SetText(Product_Name);
    grid.GetEditor("gvColQuantity").SetValue("0.00");
    grid.GetEditor("gvColDiscount").SetValue("0.00");
    grid.GetEditor("gvColAmount").SetValue("0.00");
    grid.GetEditor("gvColTaxAmount").SetValue("0.00");
    grid.GetEditor("gvColTotalAmountINR").SetValue("0.00");

    var ddlbranch = $("[id*=ddl_Branch]");
    var strBranch = ddlbranch.find("option:selected").text();

    $('#lblStkQty').text("0.00");
    $('#lblStkUOM').text(strStkUOM);
    $('#lblProduct').text(strDescription);
    $('#lblbranchName').text(strBranch);

    if (IsPackingActive == "Y" && (parseFloat(Packing_Factor * QuantityValue) > 0)) {
        $('#lblPackingStk').text(PackingValue);
    } else {
        divPacking.style.display = "none";
    }
    document.getElementById("ddlInventory").disabled = true;
    //ctaxUpdatePanel.PerformCallback('DelProdbySl~' + grid.GetEditor("SrlNo").GetValue() + '~' + strProductID);
    deleteTax('DelProdbySl', grid.GetEditor("SrlNo").GetValue(), strProductID);
    grid.batchEditApi.StartEdit(globalRowIndex, 6);

    var _SrlNo = grid.GetEditor("SrlNo").GetValue();
    if (TaxOfProduct.filter(function (e) { return e.SrlNo == _SrlNo; }).length == 0) {
        var ProductTaxes = { SrlNo: _SrlNo, IsTaxEntry: "N" }
        TaxOfProduct.push(ProductTaxes);

    }
    else {
        $.grep(TaxOfProduct, function (e) { if (e.SrlNo == _SrlNo) e.IsTaxEntry = "N"; });
    }
    SetFocusAfterProductSelect();
}
function SetFocusAfterProductSelect() {
    setTimeout(function () {
        grid.batchEditApi.StartEdit(globalRowIndex, 6);
        return;
    }, 300);
}
function deleteTax(Action, srl, productid) {
    var OtherDetail = {};
    OtherDetail.Action = Action;
    OtherDetail.srl = srl;
    OtherDetail.prodid = productid;            
  
    $.ajax({
        type: "POST",
        url: "PurchaseOrder.aspx/taxUpdatePanel_Callback",
        data: JSON.stringify(OtherDetail),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var Code = msg.d;
            if (Code != null) {
            }
            if (productid != "") {
                setTimeout(function () {
                    grid.batchEditApi.StartEdit(globalRowIndex, 5);
                }, 600)
            }
        }
    });
}

//..............End Product........................
//.............Available Stock Div Show............................
function ProductsGotFocus(s, e) {
    pageheaderContent.style.display = "block";
    divAvailableStk.style.display = "block";
    var ProductID = (grid.GetEditor('gvColProduct').GetValue() != null) ? grid.GetEditor('gvColProduct').GetValue() : "0";
    var strProductName = (grid.GetEditor('gvColProduct').GetText() != null) ? grid.GetEditor('gvColProduct').GetText() : "0";
    var QuantityValue = (grid.GetEditor('gvColQuantity').GetValue() != null) ? grid.GetEditor('gvColQuantity').GetValue() : "0";
    var ddlbranch = $("[id*=ddl_Branch]");
    var strBranch = ddlbranch.find("option:selected").text();
    var SpliteDetails = ProductID.split("||@||");
    var strProductID = SpliteDetails[0];
    var strDescription = SpliteDetails[1];
    var strUOM = SpliteDetails[2];
    var strStkUOM = SpliteDetails[4];
    var strSalePrice = SpliteDetails[6];
    var IsPackingActive = SpliteDetails[13];
    var Packing_Factor = SpliteDetails[14];
    var Packing_UOM = SpliteDetails[15];
    var PackingValue = (Packing_Factor * QuantityValue) + " " + Packing_UOM;

    if (IsPackingActive == "Y" && (parseFloat(Packing_Factor * QuantityValue) > 0)) {
        $('#lblPackingStk').text(PackingValue);
        //  divPacking.style.display = "block";
    } else {
        divPacking.style.display = "none";
    }

    $('#lblStkQty').text(QuantityValue);
    $('#lblStkUOM').text(strStkUOM);
    $('#lblProduct').text(strProductName);
    $('#lblbranchName').text(strBranch);

    if (ProductID != "0") {
        cacpAvailableStock.PerformCallback(strProductID);
    }
}
function acpAvailableStockEndCall(s, e) {
    if (cacpAvailableStock.cpstock != null) {
        divAvailableStk.style.display = "block";
        //divpopupAvailableStock.style.display = "block";
        var AvlStk = cacpAvailableStock.cpstock + " " + document.getElementById('lblStkUOM').innerHTML;
        document.getElementById('lblAvailableStkPro').innerHTML = AvlStk;
        document.getElementById('lblAvailableStk').innerHTML = cacpAvailableStock.cpstock;
        cCmbWarehouse.cpstock = null;
    }
}
//................Available Stock Div Show....................
//Code for UDF Control 
function OpenUdf(s, e) {
    if (document.getElementById('IsUdfpresent').value == '0') {
        jAlert("UDF not define.");
    }
    else {
        var keyVal = document.getElementById('Keyval_internalId').value;
        var url = '/OMS/management/Master/frm_BranchUdfPopUp.aspx?Type=PO&&KeyVal_InternalID=' + keyVal;
        popup.SetContentUrl(url);
        popup.Show();
    }
    return true;
}
// End Udf Code
//............Check Unique   Purchase Order................
function txtBillNo_TextChanged() {
    var VoucherNo = document.getElementById("txtVoucherNo").value;
    $.ajax({
        type: "POST",
        url: "PurchaseOrder.aspx/CheckUniqueName",
        data: JSON.stringify({ VoucherNo: VoucherNo }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (msg) {
            var data = msg.d;
            if (data == true) {
                $("#MandatoryBillNo").show();
                document.getElementById("txtVoucherNo").value = '';
                document.getElementById("txtVoucherNo").focus();
            }
            else {
                $("#MandatoryBillNo").hide();
            }
        }
    });
}

function ReBindGrid_Currency() {
    var frontRow = 0;
    var backRow = -1;
    var IsProduct = "";
    for (var i = 0; i <= grid.GetVisibleRowsOnPage() ; i++) {
        var frontProduct = (grid.batchEditApi.GetCellValue(backRow, 'gvColProduct') != null) ? (grid.batchEditApi.GetCellValue(backRow, 'gvColProduct')) : "";
        var backProduct = (grid.batchEditApi.GetCellValue(frontRow, 'gvColProduct') != null) ? (grid.batchEditApi.GetCellValue(frontRow, 'gvColProduct')) : "";

        if (frontProduct != "" || backProduct != "") {
            IsProduct = "Y";
            break;
        }

        backRow--;
        frontRow++;
    }

    if (IsProduct == "Y") {
        $('#hdfIsDelete').val('D');
        grid.UpdateEdit();
        //   grid.PerformCallback('CurrencyChangeDisplay');
    }
    cddl_AmountAre.Focus();
}
//...............end.........................
//...............PopulateVAT........................
function PopulateGSTCSTVAT(e) {
    var key = cddl_AmountAre.GetValue();
    if (key == 1) {
        grid.GetEditor('gvColTaxAmount').SetEnabled(true);
        cddlVatGstCst.SetEnabled(false);
        //cddlVatGstCst.PerformCallback('1');
        cddlVatGstCst.SetSelectedIndex(-1);
        cbtn_SaveRecords.SetVisible(true);
        grid.GetEditor('gvColProduct').Focus();
        if (grid.GetVisibleRowsOnPage() == 1) {
            grid.batchEditApi.StartEdit(-1, 2);
        }
    }
    else if (key == 2) {
        grid.GetEditor('gvColTaxAmount').SetEnabled(true);
        cddlVatGstCst.SetEnabled(true);
        cddlVatGstCst.PerformCallback('2');
        cddlVatGstCst.Focus();
        cbtn_SaveRecords.SetVisible(true);
    }
    else if (key == 3) {
        grid.GetEditor('gvColTaxAmount').SetEnabled(false);
        //cddlVatGstCst.PerformCallback('3');
        cddlVatGstCst.SetSelectedIndex(-1);
        cddlVatGstCst.SetEnabled(false);
        // cbtn_SaveRecords.SetVisible(false);
        cbtn_SaveRecordTaxs.SetVisible(false);
        if (grid.GetVisibleRowsOnPage() == 1) {
            grid.batchEditApi.StartEdit(-1, 2);
        }
    }
    //// below code will be executed only in View Mode --- Samrat Roy -- 04-05-2017
    if (getUrlVars().req == "V") {
        cbtn_SaveRecords.SetVisible(false);
        cbtn_SaveRecordExits.SetVisible(false);
    }
}
function Keypressevt() {
    if (event.keyCode == 13) {
        //run code for Ctrl+X -- ie, Save & Exit! 
        SaveWarehouse();
        return false;
    }
}
function SetFocusonDemand(e) {
    ctxtCreditDays.Focus();
}
function SetFocusonGrid(e) {
    if (grid.GetVisibleRowsOnPage() == 1) {
        grid.batchEditApi.StartEdit(-1, 3);
    }
}
function TaxAmountKeyDown(s, e) {
    if (e.htmlEvent.key == "Enter") {
        s.OnButtonClick(0);
    }
}
function taxAmtButnClick1(s, e) {
    rowEditCtrl = s;
}
function taxAmtButnClick(s, e) {
    if (e.buttonIndex == 0) {

        if (cddl_AmountAre.GetValue() != null) {
            var ProductID = (grid.GetEditor('gvColProduct').GetValue() != null) ? grid.GetEditor('gvColProduct').GetValue() : "";
            if (ProductID.trim() != "") {
                document.getElementById('setCurrentProdCode').value = ProductID.split('||')[0];
                document.getElementById('HdSerialNo').value = grid.GetEditor('SrlNo').GetText();
                ctxtTaxTotAmt.SetValue(0);
                ccmbGstCstVat.SetSelectedIndex(0);
                $('.RecalculateInline').hide();
                caspxTaxpopUp.Show();                       
                var QuantityValue = (grid.GetEditor('gvColQuantity').GetValue() != null) ? grid.GetEditor('gvColQuantity').GetValue() : "0";
                var SpliteDetails = ProductID.split("||@||");
                var strMultiplier = SpliteDetails[7];
                var strFactor = SpliteDetails[8];
                var strRate = (ctxtRate.GetValue() != null && ctxtRate.GetValue() != "0") ? ctxtRate.GetValue() : "1";
                var strStkUOM = SpliteDetails[4];
                var strSalePrice = (grid.GetEditor('gvColStockPurchasePrice').GetValue() != null) ? grid.GetEditor('gvColStockPurchasePrice').GetValue() : "";
                document.getElementById('hdnQty').value = grid.GetEditor('gvColQuantity').GetText();
                if (strRate == 0) {
                    strRate = 1;
                }
                var StockQuantity = strMultiplier * QuantityValue;                      
                var Amount = DecimalRoundoff(QuantityValue * strFactor * (strSalePrice / strRate),2);
                clblTaxProdGrossAmt.SetText(DecimalRoundoff(Amount,2));
                var strAmount = DecimalRoundoff(grid.GetEditor('gvColAmount').GetValue(),2);                       
                clblProdNetAmt.SetText(strAmount);
                document.getElementById('HdProdGrossAmt').value = DecimalRoundoff(Amount,2);                       
                document.getElementById('HdProdNetAmt').value = DecimalRoundoff(strAmount,2);                    
                if (parseFloat(grid.GetEditor('gvColDiscount').GetValue()) > 0) {                           
                    var discount = (Amount * grid.GetEditor('gvColDiscount').GetValue() / 100).toFixed(2);
                    clblTaxDiscount.SetText(DecimalRoundoff(discount,2));
                }
                else {
                    clblTaxDiscount.SetText('0.00');
                }                      
                if (cddl_AmountAre.GetValue() == "2") {
                    $('.GstCstvatClass').hide();
                    $('.gstGrossAmount').show();
                    clblTaxableGross.SetText("(Taxable)");
                    clblTaxableNet.SetText("(Taxable)");
                    $('.gstNetAmount').show();                          
                    var gstRate = parseFloat(cddlVatGstCst.GetValue().split('~')[1]);
                    if (gstRate) {
                        if (gstRate != 0) {
                            var gstDis = (gstRate / 100) + 1;
                            if (cddlVatGstCst.GetValue().split('~')[2] == "G") {
                                $('.gstNetAmount').hide();                                      
                                clblTaxProdGrossAmt.SetText(DecimalRoundoff(Amount / gstDis),2);                                      
                                document.getElementById('HdProdGrossAmt').value =(Amount / gstDis).toFixed(2);                                      
                                clblGstForGross.SetText((Amount - parseFloat(document.getElementById('HdProdGrossAmt').value)).toFixed(2));
                                clblTaxableNet.SetText("");
                            }
                            else {
                                $('.gstGrossAmount').hide();                                    
                                clblProdNetAmt.SetText(DecimalRoundoff(grid.GetEditor('gvColAmount').GetValue() / gstDis),2);                                      
                                document.getElementById('HdProdNetAmt').value = (grid.GetEditor('gvColAmount').GetValue() / gstDis).toFixed(2);                                      
                                clblGstForNet.SetText((grid.GetEditor('gvColAmount').GetValue() - parseFloat(document.getElementById('HdProdNetAmt').value)).toFixed(2));
                                clblTaxableGross.SetText("");
                            }
                        }

                    } else {
                        $('.gstGrossAmount').hide();
                        $('.gstNetAmount').hide();
                        clblTaxableGross.SetText("");
                        clblTaxableNet.SetText("");
                    }
                }
                else if (cddl_AmountAre.GetValue() == "1") {
                    $('.GstCstvatClass').show();
                    $('.gstGrossAmount').hide();
                    $('.gstNetAmount').hide();
                    clblTaxableGross.SetText("");
                    clblTaxableNet.SetText("");
                    var shippingStCode = '';
                    if (cPurchaseOrderPosGst.GetValue() == "S") {
                        shippingStCode = GeteShippingStateCode();
                    }
                    else {
                        shippingStCode = GetBillingStateCode();
                    }
                    //shippingStCode = ctxtshippingState.GetText();
                    //shippingStCode = shippingStCode.substring(shippingStCode.lastIndexOf('(')).replace('(State Code:', '').replace(')', '').trim();                        

                         
                    if (shippingStCode.trim() != '') {
                        for (var cmbCount = 1; cmbCount < ccmbGstCstVat.GetItemCount() ; cmbCount++) {                                  
                            if (ccmbGstCstVat.GetItem(cmbCount).value.split('~')[5] != "") {
                                if (ccmbGstCstVat.GetItem(cmbCount).value.split('~')[5] == shippingStCode) {                                       
                                    if (shippingStCode == "4" || shippingStCode == "26" || shippingStCode == "25" || shippingStCode == "35" || shippingStCode == "31" || shippingStCode == "34") {
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
                }
                else {
                    clblTaxableGross.SetText("");
                    clblTaxableNet.SetText("");

                }
                var _SrlNo = document.getElementById('HdSerialNo').value;
                var _IsEntry="";
                if (TaxOfProduct.filter(function (e) { return e.SrlNo == _SrlNo; }).length > 0) {
                    _IsEntry=TaxOfProduct.find(o => o.SrlNo === _SrlNo).IsTaxEntry;
                }
                     
                //if (globalRowIndex > -1) {
                if(_IsEntry=="N"){
                    cgridTax.PerformCallback('New~' + cddl_AmountAre.GetValue());                          
                    cgridTax.cpComboCode = grid.GetEditor('gvColProduct').GetValue().split('||@||')[9];
                            
                } else {
                    cgridTax.PerformCallback(grid.keys[globalRowIndex] + '~' + cddl_AmountAre.GetValue());
                }
                ctxtprodBasicAmt.SetValue(grid.GetEditor('gvColAmount').GetValue());
            } else {
                grid.batchEditApi.StartEdit(globalRowIndex, 13);
            }
        }
    }
}
function QuantityProductsGotFocus(s, e) {

    var ProductID = grid.GetEditor('gvColProduct').GetValue();
    if (ProductID != null) {         
        
        _GetQuantityValue = (grid.GetEditor('gvColQuantity').GetValue() != null) ? grid.GetEditor('gvColQuantity').GetValue() : "0";
        _GetPurchasePriceValue = (grid.GetEditor('gvColStockPurchasePrice').GetValue() != null) ? grid.GetEditor('gvColStockPurchasePrice').GetValue() : "0";
        _GetDiscountValue = (grid.GetEditor('gvColDiscount').GetValue() != null) ? grid.GetEditor('gvColDiscount').GetValue() : "0";
        _GetAmountValue = (grid.GetEditor('gvColAmount').GetValue() != null) ? grid.GetEditor('gvColAmount').GetValue() : "0";
    } 
            
    ////chinmoy added for  for MultiUOM start
    if ($("#hddnMultiUOMSelection").val() == "1") {
        //grid.batchEditApi.StartEdit(globalRowIndex);
       
        if (grid.GetEditor('gvColQuantity').GetValue() != "0.0000") {
            //grid.batchEditApi.StartEdit(globalRowIndex);
            $("#UOMQuantity").val(grid.GetEditor('gvColQuantity').GetValue());
        }
      
    }
        
    ////End
    //Surojit 26-02-2019

    var ProductID = (grid.GetEditor('gvColProduct').GetText() != null) ? grid.GetEditor('gvColProduct').GetText() : "0";
    var strProductName = (grid.GetEditor('gvColProduct').GetText() != null) ? grid.GetEditor('gvColProduct').GetText() : "0";
    var QuantityValue = (grid.GetEditor('gvColQuantity').GetValue() != null) ? grid.GetEditor('gvColQuantity').GetValue() : "0";
    var SpliteDetails = ProductID.split("||@||");
    var strProductID = SpliteDetails[0];
    var strDescription = SpliteDetails[1];
    var strUOM = SpliteDetails[2];
    var strStkUOM = SpliteDetails[4];
    var strSalePrice = SpliteDetails[6];
    var IsPackingActive = SpliteDetails[10];
    var Packing_Factor = SpliteDetails[11];
    var Packing_UOM = SpliteDetails[12];
    var strProductShortCode = SpliteDetails[14];
    var PackingValue = (Packing_Factor * QuantityValue) + " " + Packing_UOM;
    strProductName = strDescription;
    var isOverideConvertion = SpliteDetails[25]; 
    var packing_saleUOM = SpliteDetails[24];
    var sProduct_SaleUom = SpliteDetails[23];
    var sProduct_quantity = SpliteDetails[21];
    var packing_quantity = SpliteDetails[19];
    var slno = (grid.GetEditor('SrlNo').GetText() != null) ? grid.GetEditor('SrlNo').GetText() : "0";
    var Indent_Num = (grid.GetEditor('Indent_Num').GetText() != null) ? grid.GetEditor('Indent_Num').GetText() : "0";
    var ConvertionOverideVisible = $('#hdnConvertionOverideVisible').val();
    var ShowUOMConversionInEntry = $('#hdnShowUOMConversionInEntry').val();
    var type = 'add';
    var gridprodqty = parseFloat(grid.GetEditor('gvColQuantity').GetText()).toFixed(5);
    var gridPackingQty = '';
    var IsInventory = '';
    var actionQry = '';            
    if (SpliteDetails.length == 27) {
        if (SpliteDetails[26] == "1") {
            IsInventory = 'Yes';
        }
    }
    if (SpliteDetails.length > 26) {
        if (SpliteDetails[26] == "1") {
            IsInventory = 'Yes';
            if (Indent_Num != "0" && Indent_Num != "") {
                actionQry = 'PurchaseOrderIndent';                         
                if($("#hddnMultiUOMSelection").val()=="0")
                {
                    $.ajax({
                        type: "POST",
                        url: "Services/Master.asmx/GetMultiUOMDetails",
                        data: JSON.stringify({ orderid: strProductID, action: actionQry, module: 'PurchaseOrder', strKey: Indent_Num }),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (msg) {

                            gridPackingQty = msg.d;
                            type = 'edit';

                            if($("#hddnMultiUOMSelection").val()=="0")
                            {

                                if (ShowUOMConversionInEntry == "1"  && SpliteDetails.length > 1) {
                                    ShowUOM(type, "Purchase", 0, sProduct_quantity, sProduct_SaleUom, packing_quantity, packing_saleUOM, strProductID, slno, isOverideConvertion, gridprodqty, gridPackingQty);
                                }
                            }

                        }
                    });
                }

            }
            else {
                        
                actionQry = 'PurchaseOrderByProductID';
                var orderid = grid.GetRowKey(globalRowIndex);
                if($("#hddnMultiUOMSelection").val()=="0")
                {
                    $.ajax({
                        type: "POST",
                        url: "Services/Master.asmx/GetMultiUOMDetails",
                        data: JSON.stringify({ orderid: orderid, action: actionQry, module: 'PurchaseOrder', strKey: '' }),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (msg) {

                            gridPackingQty = msg.d;
                            //type = 'edit';
                            if($("#hddnMultiUOMSelection").val()=="0")
                            {
                                if (ShowUOMConversionInEntry == "1"  && SpliteDetails.length > 1) {
                                    ShowUOM(type, "PurchaseOrder", 0, sProduct_quantity, sProduct_SaleUom, packing_quantity, packing_saleUOM, strProductID, slno, isOverideConvertion, gridprodqty, gridPackingQty);
                                }
                            }
                        }
                    });
                }
            }

        }
    }
    else {


        if (SpliteDetails.length == 19) {
            actionQry = 'GetPurchaseOrderProduct';
            var orderid = grid.GetRowKey(globalRowIndex);
            if($("#hddnMultiUOMSelection").val()=="0")
            {
                $.ajax({
                    type: "POST",
                    url: "Services/Master.asmx/GetMultiUOMDetails",
                    data: JSON.stringify({ orderid: strProductID, action: actionQry, module: 'PurchaseOrder', strKey: orderid }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                           
                        SpliteDetails = msg.d.split("||@||");
                          
                        if (SpliteDetails[5] == "1") {
                            IsInventory = 'Yes';
                        }

                        isOverideConvertion = SpliteDetails[4];
                        packing_saleUOM = SpliteDetails[2];
                        sProduct_SaleUom = SpliteDetails[3];
                        sProduct_quantity = SpliteDetails[0];
                        packing_quantity = SpliteDetails[1];

                        if(SpliteDetails[6] != ""){
                            gridPackingQty = SpliteDetails[6];
                        }
                        if($("#hddnMultiUOMSelection").val()=="0")
                        {
                            if (ShowUOMConversionInEntry == "1"  && SpliteDetails.length > 1) {
                                ShowUOM(type, "PurchaseOrder", 0, sProduct_quantity, sProduct_SaleUom, packing_quantity, packing_saleUOM, strProductID, slno, isOverideConvertion, gridprodqty, gridPackingQty);
                            }
                        }

                    }
                });
            }
        }
        else{
            if($("#hddnMultiUOMSelection").val()=="0")
            {
                if (ShowUOMConversionInEntry == "1"  && SpliteDetails.length > 1) {
                    ShowUOM(type, "PurchaseOrder", 0, sProduct_quantity, sProduct_SaleUom, packing_quantity, packing_saleUOM, strProductID, slno, isOverideConvertion, gridprodqty, gridPackingQty);
                }
            }
        }
    }

    //Surojit 26-02-2019

}

var issavePacking = 0;

function SetDataToGrid(Quantity, packing, PackingUom, PackingSelectUom, productid, slno) {
    issavePacking = 1;
    grid.batchEditApi.StartEdit(globalRowIndex);
    grid.GetEditor('gvColQuantity').SetValue(Quantity);
             
    setTimeout(function () {
        grid.batchEditApi.StartEdit(globalRowIndex, 7);
    }, 600)
                 

}
function SetFoucs() {

}

function PurchasePriceTextFocus(s, e) {            
    _GetQuantityValue = (grid.GetEditor('gvColQuantity').GetValue() != null) ? grid.GetEditor('gvColQuantity').GetValue() : "0";
    _GetPurchasePriceValue = (grid.GetEditor('gvColStockPurchasePrice').GetValue() != null) ? grid.GetEditor('gvColStockPurchasePrice').GetValue() : "0";
    _GetDiscountValue = (grid.GetEditor('gvColDiscount').GetValue() != null) ? grid.GetEditor('gvColDiscount').GetValue() : "0";
    _GetAmountValue = (grid.GetEditor('gvColAmount').GetValue() != null) ? grid.GetEditor('gvColAmount').GetValue() : "0";
}

function uomGotFocus(s, e) {
    var inx = globalRowIndex;
    SetTotalTaxableAmount(inx, e);
    SetInvoiceLebelValue();
}

function QuantityTextChange(s, e) {          
            
    // Mantis Issue 24429  [ This checking not needed any more since when Multiple UOM is activated, Quantity cannot be enterd from Quantity column of main product grid]      
    //if (($("#hddnMultiUOMSelection").val() == "1")) {              
    //    UomLenthCalculation();          
              
    //    grid.batchEditApi.StartEdit(globalRowIndex);
    //    var SLNo = grid.GetEditor('SrlNo').GetValue();              
    //    if (Uomlength > 0) {
    //        var qnty = $("#UOMQuantity").val();
    //        var QValue = (grid.GetEditor('gvColQuantity').GetValue() != null) ? grid.GetEditor('gvColQuantity').GetValue() : "0.0000";
    //        if (QValue != "0.0000" && QValue != qnty) {
    //            jConfirm('Qunatity Change Will Clear Multiple UOM Details, Confirm?', 'Confirmation Dialog', function (r) {
    //                if (r == true) {
    //                    grid.batchEditApi.StartEdit(globalRowIndex);
    //                    var tbqty = grid.GetEditor('gvColQuantity');                             

    //                    var detailsid = grid.GetEditor('DetailsId').GetValue();
    //                    if (detailsid != null && detailsid != "") {
    //                        cgrid_MultiUOM.PerformCallback('CheckMultiUOmDetailsQuantity~' + SLNo + '~' + detailsid);
    //                    }
    //                    else {
    //                        cgrid_MultiUOM.PerformCallback('CheckMultiUOmDetailsQuantity~' + SLNo + '~' + detailsid);
    //                    }                               
    //                    setTimeout(function () {
    //                        grid.batchEditApi.StartEdit(globalRowIndex, 7);
    //                    }, 600)
    //                }
    //                else {
    //                    grid.batchEditApi.StartEdit(globalRowIndex);
    //                    grid.GetEditor('gvColQuantity').SetValue(qnty);
    //                    setTimeout(function () {
    //                        grid.batchEditApi.StartEdit(globalRowIndex, 7);
    //                    }, 200);
    //                }
    //            });
    //        }
    //        else {
    //            grid.batchEditApi.StartEdit(globalRowIndex);
    //            grid.GetEditor('gvColQuantity').SetValue(qnty);                       
    //            setTimeout(function () {
    //                grid.batchEditApi.StartEdit(globalRowIndex, 7);
    //            }, 600)
                      
    //        }
    //    }                
    //}      
    // End of Mantis Issue 24429
    pageheaderContent.style.display = "block";
    divAvailableStk.style.display = "block";
    var QuantityValue = (grid.GetEditor('gvColQuantity').GetValue() != null) ? grid.GetEditor('gvColQuantity').GetValue() : "0";
    if (parseFloat(QuantityValue) != parseFloat(_GetQuantityValue)) {
        var ProductID = grid.GetEditor('gvColProduct').GetValue();
        if (ProductID != null) {
            var SpliteDetails = ProductID.split("||@||");
            var strMultiplier = SpliteDetails[7];                  
            var strFactor = SpliteDetails[8]; 
            var strRate = (ctxtRate.GetValue() != null && ctxtRate.GetValue() != "0") ? ctxtRate.GetValue() : "1";
            var strProductID = SpliteDetails[0];
            var strProductName = (grid.GetEditor('gvColProduct').GetText() != null) ? grid.GetEditor('gvColProduct').GetText() : "0";
            var ddlbranch = $("[id*=ddl_Branch]");
            var strBranch = ddlbranch.find("option:selected").text();
            var strStkUOM = SpliteDetails[4];
            var strSalePrice = (grid.GetEditor('gvColStockPurchasePrice').GetValue() != null) ? grid.GetEditor('gvColStockPurchasePrice').GetValue() : "0";
            if (strRate == 0) {
                strRate = 1;
            }
            var Amount = (QuantityValue * strFactor * (strSalePrice / strRate)).toFixed(2);
            var tbAmount = grid.GetEditor("gvColAmount");
            tbAmount.SetValue(Amount);
            var tbTotalAmount = grid.GetEditor("gvColTotalAmountINR");
            tbTotalAmount.SetValue(Amount);
            $('#lblbranchName').text(strBranch);
            var IsLinkedProduct = (grid.GetEditor('IsLinkedProduct').GetText() != null) ? grid.GetEditor('IsLinkedProduct').GetText() : "";
            if (IsLinkedProduct != "Y") {
                var tbAmount = grid.GetEditor("gvColAmount");
                tbAmount.SetValue(Amount);
                var tbTotalAmount = grid.GetEditor("gvColTotalAmountINR");
                tbTotalAmount.SetValue(Amount);
                DiscountTextChange(s, e);                       
            }
        }
        else {
            jAlert('Select a product first.');
            grid.GetEditor('gvColQuantity').SetValue('0');
            grid.GetEditor('gvColProduct').Focus();
        }
    }
    if($("#hddnMultiUOMSelection").val()=="1")
    {
        setTimeout(function () {
            grid.batchEditApi.StartEdit(globalRowIndex, 7);
        }, 800)
    }          

}

var globalNetAmount = 0;        
function SetTotalTaxableAmount(inx, vindex) {           
    var count = grid.GetVisibleRowsOnPage();
    var totalAmount = 0;
    var totaltxAmount = 0;
    var totalQuantity = 0;
    for (var i = 0; i < count + 10; i++) {
        if (grid.GetRow(i)) {
            if (grid.GetRow(i).style.display != "none") {
                grid.batchEditApi.StartEdit(i, 2);
                totalAmount = totalAmount + DecimalRoundoff(grid.GetEditor("gvColAmount").GetValue(), 2);
                totalQuantity = totalQuantity + DecimalRoundoff(grid.GetEditor("gvColQuantity").GetValue(), 4);
                if (grid.GetEditor("gvColTaxAmount").GetValue() != null) {
                    totaltxAmount = totaltxAmount + DecimalRoundoff(grid.GetEditor("gvColTaxAmount").GetValue(), 2);
                    grid.GetEditor("gvColTotalAmountINR").SetValue(DecimalRoundoff(grid.GetEditor("gvColTaxAmount").GetValue(), 2) + DecimalRoundoff(grid.GetEditor("gvColAmount").GetValue(), 2))
                }
                else {
                    grid.GetEditor("gvColTotalAmountINR").SetValue(DecimalRoundoff(grid.GetEditor("gvColAmount").GetValue(), 2))
                }                        
            }
        }
    }

    for (i = -1; i > -count - 10; i--) {
        if (grid.GetRow(i)) {
            if (grid.GetRow(i).style.display != "none") {
                grid.batchEditApi.StartEdit(i, 2);
                totalAmount = totalAmount + DecimalRoundoff(grid.GetEditor("gvColAmount").GetValue(), 2);
                totalQuantity = totalQuantity + DecimalRoundoff(grid.GetEditor("gvColQuantity").GetValue(), 4);
                if (grid.GetEditor("gvColTaxAmount").GetValue() != null) {
                    totaltxAmount = totaltxAmount + DecimalRoundoff(grid.GetEditor("gvColTaxAmount").GetValue(), 2);
                    grid.GetEditor("gvColTotalAmountINR").SetValue(DecimalRoundoff(grid.GetEditor("gvColTaxAmount").GetValue(), 2) + DecimalRoundoff(grid.GetEditor("gvColAmount").GetValue(), 2))

                }
                else {
                    grid.GetEditor("gvColTotalAmountINR").SetValue(DecimalRoundoff(grid.GetEditor("gvColAmount").GetValue(), 2))
                }                         
            }
        }
    }
    globalRowIndex = inx;
    cbnrLblTaxableAmtval.SetText(DecimalRoundoff(totalAmount, 2));
    cbnrLblTaxAmtval.SetText(DecimalRoundoff(totaltxAmount, 2));
    cbnrLblTotalQty.SetText(DecimalRoundoff(totalQuantity, 4));
    var totamt = totalAmount + totaltxAmount;
    cbnrlblAmountWithTaxValue.SetText(totamt);
    cbnrLblInvValue.SetText(totamt);
}       
function SetInvoiceLebelValue() {
    var invValue = parseFloat(cbnrlblAmountWithTaxValue.GetValue()) - (parseFloat(cbnrLblLessAdvanceValue.GetValue()) + parseFloat(cbnrLblLessOldMainVal.GetValue())) + parseFloat(cbnrOtherChargesvalue.GetValue());           

    cbnrLblInvValue.SetValue(parseFloat(Math.round(Math.abs(invValue) * 100) / 100).toFixed(2));        

}
function Taxlostfocus(s, e) {          
}
function gvColAmountgotfocus(s, e) {         
    SetTotalTaxableAmount(s, e);
    SetInvoiceLebelValue();
}
function gvColAmountlostfocus(s, e) {         
    SetTotalTaxableAmount(s, e);
    SetInvoiceLebelValue();
}
function TotalAmountgotfocus(s, e) {         
    SetTotalTaxableAmount(s, e);
    SetInvoiceLebelValue();
}  

var globalNetAmount = 0;
function PurchasePriceTextChange(s, e) {
    pageheaderContent.style.display = "block";
    divAvailableStk.style.display = "block";
    var QuantityValue = (grid.GetEditor('gvColQuantity').GetValue() != null) ? grid.GetEditor('gvColQuantity').GetValue() : "0";            
    var ProductID = grid.GetEditor('gvColProduct').GetValue();
    if (ProductID != null) {
        var SpliteDetails = ProductID.split("||@||");
        var strMultiplier = SpliteDetails[7];             
        var strFactor = SpliteDetails[8]; 
        var strRate = (ctxtRate.GetValue() != null && ctxtRate.GetValue() != "0") ? ctxtRate.GetValue() : "1";
        var strProductID = SpliteDetails[0];
        var strProductName = (grid.GetEditor('gvColProduct').GetText() != null) ? grid.GetEditor('gvColProduct').GetText() : "0";
        var ddlbranch = $("[id*=ddl_Branch]");
        var strBranch = ddlbranch.find("option:selected").text();
        var strStkUOM = SpliteDetails[4];
        var strPurPrice = (grid.GetEditor('gvColStockPurchasePrice').GetValue() != null) ? grid.GetEditor('gvColStockPurchasePrice').GetValue() : "0";
        if (parseFloat(_GetPurchasePriceValue) != parseFloat(strPurPrice)) {
            if (strRate == 0) {
                strRate = 1;
            }
            var Amount = (QuantityValue * strFactor * (strPurPrice / strRate)).toFixed(2);
            var tbAmount = grid.GetEditor("gvColAmount");
            tbAmount.SetValue(Amount);
            var tbTotalAmount = grid.GetEditor("gvColTotalAmountINR");
            tbTotalAmount.SetValue(Amount);
            $('#lblbranchName').text(strBranch);
            var IsLinkedProduct = (grid.GetEditor('IsLinkedProduct').GetText() != null) ? grid.GetEditor('IsLinkedProduct').GetText() : "";
            if (IsLinkedProduct != "Y") {
                var tbAmount = grid.GetEditor("gvColAmount");
                tbAmount.SetValue(Amount);
                var tbTotalAmount = grid.GetEditor("gvColTotalAmountINR");
                tbTotalAmount.SetValue(Amount);
                DiscountTextChange(s, e);                       
            }
        }               
        var finalNetAmount = parseFloat(tbTotalAmount);
        var finalAmount = parseFloat(cbnrlblAmountWithTaxValue.GetValue()) + (finalNetAmount - globalNetAmount);
        cbnrlblAmountWithTaxValue.SetText(parseFloat(Math.round(Math.abs(finalAmount) * 100) / 100).toFixed(2));                
        SetTotalTaxableAmount(s, e);
        SetInvoiceLebelValue();               
    }
    else {
        jAlert('Select a product first.');
        grid.GetEditor('gvColQuantity').SetValue('0');
        grid.GetEditor('gvColProduct').Focus();
    }
}
function DiscountTextFocus() {           
    _GetQuantityValue = (grid.GetEditor('gvColQuantity').GetValue() != null) ? grid.GetEditor('gvColQuantity').GetValue() : "0";
    _GetPurchasePriceValue = (grid.GetEditor('gvColStockPurchasePrice').GetValue() != null) ? grid.GetEditor('gvColStockPurchasePrice').GetValue() : "0";
    _GetDiscountValue = (grid.GetEditor('gvColDiscount').GetValue() != null) ? grid.GetEditor('gvColDiscount').GetValue() : "0";
    _GetAmountValue = (grid.GetEditor('gvColAmount').GetValue() != null) ? grid.GetEditor('gvColAmount').GetValue() : "0";
}
function DiscountValueChange(s, e) {
    var ProductID = grid.GetEditor('gvColProduct').GetValue();
    var Discount = (grid.GetEditor('gvColDiscount').GetValue() != null) ? grid.GetEditor('gvColDiscount').GetValue() : "0";
    if (ProductID != null) {
        if (parseFloat(Discount) != parseFloat(_GetDiscountValue)) {
            DiscountTextChange(s, e);
        }
    }
    else {
        jAlert('Select a product first.');
    }
}
function DiscountTextChange(s, e) {

    var Discount = (grid.GetEditor('gvColDiscount').GetValue() != null) ? grid.GetEditor('gvColDiscount').GetValue() : "0";
    var QuantityValue = (grid.GetEditor('gvColQuantity').GetValue() != null) ? grid.GetEditor('gvColQuantity').GetValue() : "0";
    var ProductID = grid.GetEditor('gvColProduct').GetValue();
    if (ProductID != null) {
        var SpliteDetails = ProductID.split("||@||");
        var strFactor = SpliteDetails[8];
        var strRate = (ctxtRate.GetValue() != null && ctxtRate.GetValue() != "0") ? ctxtRate.GetValue() : "1";
        var strSalePrice = (grid.GetEditor('gvColStockPurchasePrice').GetValue() != null) ? grid.GetEditor('gvColStockPurchasePrice').GetValue() : "0";
        if (strSalePrice == '0') {
            strSalePrice = SpliteDetails[6];
        }
        if (strRate == 0) {
            strRate = 1;
        }
        var amountAfterDiscount = "";
        var Amount = QuantityValue * strFactor * (strSalePrice / strRate);
        Amount = Amount.toFixed(2);
        if (Discount != "0" && Discount != "0.00") {
            amountAfterDiscount = parseFloat(Amount) - ((parseFloat(Discount) * parseFloat(Amount)) / 100);
            amountAfterDiscount = amountAfterDiscount.toFixed(2);
            var tbAmount = grid.GetEditor("gvColAmount");
            tbAmount.SetValue(amountAfterDiscount);
            var tbTotalAmount = grid.GetEditor("gvColTotalAmountINR");
            tbTotalAmount.SetValue(amountAfterDiscount);
        }
        else {
            amountAfterDiscount = Amount;
        }
        //chinmoy edited for Discount start
        var ShippingStateCode = $("#bsSCmbStateHF").val();
        //End
        var TaxType = "";
        if (cddl_AmountAre.GetValue() == "1") {
            TaxType = "E";
        }
        else if (cddl_AmountAre.GetValue() == "2") {
            TaxType = "I";
        }

        var _SrlNo = grid.GetEditor("SrlNo").GetValue();
        if (TaxOfProduct.filter(function (e) { return e.SrlNo == _SrlNo; }).length == 0) {
            var ProductTaxes = { SrlNo: _SrlNo, IsTaxEntry: "N" }
            TaxOfProduct.push(ProductTaxes);
        }
        else {
            $.grep(TaxOfProduct, function (e) { if (e.SrlNo == _SrlNo) e.IsTaxEntry = "N"; });
        }

        var CompareStateCode;
        if (cPurchaseOrderPosGst.GetValue()== "S") {
            CompareStateCode = GeteShippingStateID();
        }
        else {
            CompareStateCode = GetBillingStateID();
        }


        caluculateAndSetGST(grid.GetEditor("gvColAmount"), grid.GetEditor("gvColTaxAmount"), grid.GetEditor("gvColTotalAmountINR"), SpliteDetails[18], Amount, amountAfterDiscount, TaxType, CompareStateCode, $('#ddl_Branch').val(), $("#hdnEntityType").val(), cPLQuoteDate.GetDate(), QuantityValue, 'P');
    }
    else {
        jAlert('Select a product first.');
        grid.GetEditor('gvColDiscount').SetValue('0');
        grid.GetEditor('gvColProduct').Focus();
    }
    //ctaxUpdatePanel.PerformCallback('DelQtybySl~' + grid.GetEditor("SrlNo").GetValue());
    deleteTax('DelQtybySl', grid.GetEditor("SrlNo").GetValue(),"");
}

//......................Amount Calculation End.......................
/*........................Tax Start...........................*/
var taxAmountGlobalCharges;
var chargejsonTax;
var taxAmountGlobal;
var GlobalCurChargeTaxAmt;
var ChargegstcstvatGlobalName;
var GlobalCurTaxAmt = 0;
var rowEditCtrl;
var globalRowIndex;
var globalTaxRowIndex;
var gstcstvatGlobalName;
var taxJson;
function Save_TaxClick() {
    if (gridTax.GetVisibleRowsOnPage() > 0) {
        gridTax.UpdateEdit();
    }
    else {
        gridTax.PerformCallback('SaveGst');
    }
    //Rev Rajdip
    cbnrOtherChargesvalue.SetText('0.00');
    SetInvoiceLebelValueofothercharges();
    //End Rev Rajdip
    cPopup_Taxes.Hide();
}
function SetInvoiceLebelValueofothercharges() {
    cbnrOtherChargesvalue.SetValue(ctxtQuoteTaxTotalAmt.GetText());
    if (ctxtTotalAmount.GetValue() == 0.0) {
        cbnrLblInvValue.SetValue(parseFloat(cbnrlblAmountWithTaxValue.GetValue()).toFixed(2));
    }
    else {
        cbnrLblInvValue.SetValue(parseFloat(ctxtTotalAmount.GetValue()).toFixed(2));
    }
}
//Set Running Total for Charges And Tax 
function SetChargesRunningTotal() {
    var runningTot = parseFloat(ctxtProductNetAmount.GetValue());
    for (var i = 0; i < chargejsonTax.length; i++) {
        gridTax.batchEditApi.StartEdit(i, 3);
        if (chargejsonTax[i].applicableOn == "R") {
            gridTax.GetEditor("calCulatedOn").SetValue(runningTot);
            var totLength = gridTax.GetEditor("TaxName").GetText().length;
            var taxNameWithSign = gridTax.GetEditor("Percentage").GetText();
            var sign = gridTax.GetEditor("TaxName").GetText().substring(totLength - 3);
            var ProdAmt = parseFloat(gridTax.GetEditor("calCulatedOn").GetValue());
            var Amount = gridTax.GetEditor("calCulatedOn").GetValue();
            var GlobalTaxAmt = 0;

            var Percentage = gridTax.GetEditor("Percentage").GetText();
            var totLength = gridTax.GetEditor("TaxName").GetText().length;
            var sign = gridTax.GetEditor("TaxName").GetText().substring(totLength - 3);
            Sum = ((parseFloat(Amount) * parseFloat(Percentage)) / 100);

            if (sign == '(+)') {
                GlobalTaxAmt = parseFloat(gridTax.GetEditor("Amount").GetValue());
                gridTax.GetEditor("Amount").SetValue(Sum);
                ctxtQuoteTaxTotalAmt.SetValue(parseFloat(ctxtQuoteTaxTotalAmt.GetValue()) + parseFloat(Sum) - GlobalTaxAmt);
                ctxtTotalAmount.SetValue(parseFloat(Amount) + parseFloat(ctxtQuoteTaxTotalAmt.GetValue()));
                //ctxtTotalAmount.SetText(parseFloat(ctxtTotalAmount.GetValue()) + parseFloat(Sum) - GlobalTaxAmt);
                GlobalTaxAmt = 0;
            }
            else {
                GlobalTaxAmt = parseFloat(gridTax.GetEditor("Amount").GetValue());
                gridTax.GetEditor("Amount").SetValue(Sum);
                ctxtQuoteTaxTotalAmt.SetValue(parseFloat(ctxtQuoteTaxTotalAmt.GetValue()) - parseFloat(Sum) + GlobalTaxAmt);
                ctxtTotalAmount.SetValue(parseFloat(Amount) + parseFloat(ctxtQuoteTaxTotalAmt.GetValue()));
                //ctxtTotalAmount.SetText(parseFloat(ctxtTotalAmount.GetValue()) - parseFloat(Sum) + GlobalTaxAmt);
                GlobalTaxAmt = 0;
            }

            SetOtherChargeTaxValueOnRespectiveRow(0, Sum, gridTax.GetEditor("TaxName").GetText());


        }
        runningTot = runningTot + parseFloat(gridTax.GetEditor("Amount").GetValue());
        gridTax.batchEditApi.EndEdit();
    }
}
function GetPercentageData() {
    var Amount = ctxtProductAmount.GetValue();
    var GlobalTaxAmt = 0;
    var noofvisiblerows = gridTax.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
    var i, cnt = 1;
    var sumAmount = 0, totalAmount = 0;
    for (i = 0 ; cnt <= noofvisiblerows ; i++) {
        var totLength = gridTax.batchEditApi.GetCellValue(i, 'TaxName').length;
        var sign = gridTax.batchEditApi.GetCellValue(i, 'TaxName').substring(totLength - 3);
        var DisAmount = (gridTax.batchEditApi.GetCellValue(i, 'Amount') != null) ? (gridTax.batchEditApi.GetCellValue(i, 'Amount')) : "0";

        if (sign == '(+)') {
            sumAmount = sumAmount + parseFloat(DisAmount);
        }
        else {
            sumAmount = sumAmount - parseFloat(DisAmount);
        }

        cnt++;
    }

    totalAmount = (parseFloat(Amount)) + (parseFloat(sumAmount));
    // ctxtTotalAmount.SetValue(totalAmount);
}
function Save_TaxesClick() {
    grid.batchEditApi.EndEdit();
    var noofvisiblerows = grid.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
    var i, cnt = 1;
    var sumAmount = 0, sumTaxAmount = 0, sumDiscount = 0, sumNetAmount = 0, sumDiscountAmt = 0;

    cnt = 1;
    for (i = -1 ; cnt <= noofvisiblerows ; i--) {
        var Amount = (grid.batchEditApi.GetCellValue(i, 'gvColAmount') != null) ? (grid.batchEditApi.GetCellValue(i, 'gvColAmount')) : "0";
        var TaxAmount = (grid.batchEditApi.GetCellValue(i, 'gvColTaxAmount') != null) ? (grid.batchEditApi.GetCellValue(i, 'gvColTaxAmount')) : "0";
        var Discount = (grid.batchEditApi.GetCellValue(i, 'gvColDiscount') != null) ? (grid.batchEditApi.GetCellValue(i, 'gvColDiscount')) : "0";
        var NetAmount = (grid.batchEditApi.GetCellValue(i, 'gvColTotalAmountINR') != null) ? (grid.batchEditApi.GetCellValue(i, 'gvColTotalAmountINR')) : "0";
        var sumDiscountAmt = ((parseFloat(Discount) * parseFloat(Amount)) / 100);

        sumAmount = sumAmount + parseFloat(Amount);
        sumTaxAmount = sumTaxAmount + parseFloat(TaxAmount);
        sumDiscount = sumDiscount + parseFloat(sumDiscountAmt);
        sumNetAmount = sumNetAmount + parseFloat(NetAmount);

        cnt++;
    }

    if (sumAmount == 0 && sumTaxAmount == 0 && Discount == 0) {
        cnt = 1;
        for (i = 0 ; cnt <= noofvisiblerows ; i++) {
            var Amount = (grid.batchEditApi.GetCellValue(i, 'gvColAmount') != null) ? (grid.batchEditApi.GetCellValue(i, 'gvColAmount')) : "0";
            var TaxAmount = (grid.batchEditApi.GetCellValue(i, 'gvColTaxAmount') != null) ? (grid.batchEditApi.GetCellValue(i, 'gvColTaxAmount')) : "0";
            var Discount = (grid.batchEditApi.GetCellValue(i, 'gvColDiscount') != null) ? (grid.batchEditApi.GetCellValue(i, 'gvColDiscount')) : "0";
            var NetAmount = (grid.batchEditApi.GetCellValue(i, 'gvColTotalAmountINR') != null) ? (grid.batchEditApi.GetCellValue(i, 'gvColTotalAmountINR')) : "0";
            var sumDiscountAmt = ((parseFloat(Discount) * parseFloat(Amount)) / 100);

            sumAmount = sumAmount + parseFloat(Amount);
            sumTaxAmount = sumTaxAmount + parseFloat(TaxAmount);
            sumDiscount = sumDiscount + parseFloat(sumDiscountAmt);
            sumNetAmount = sumNetAmount + parseFloat(NetAmount);

            cnt++;
        }
    }

    //Debjyoti 
    document.getElementById('HdChargeProdAmt').value = sumAmount;
    document.getElementById('HdChargeProdNetAmt').value = sumNetAmount;
    //End Here

    ctxtProductAmount.SetValue(Math.round(sumAmount).toFixed(2));
    ctxtProductTaxAmount.SetValue(Math.round(sumTaxAmount).toFixed(2));
    ctxtProductDiscount.SetValue(Math.round(sumDiscount).toFixed(2));
    ctxtProductNetAmount.SetValue(Math.round(sumNetAmount).toFixed(2));
    clblChargesTaxableGross.SetText("");
    clblChargesTaxableNet.SetText("");

    //Checking is gstcstvat will be hidden or not
    if (cddl_AmountAre.GetValue() == "2") {

        $('.lblChargesGSTforGross').show();
        $('.lblChargesGSTforNet').show();

        //Set Gross Amount with GstValue
        //Get The rate of Gst
        var gstRate = parseFloat(cddlVatGstCst.GetValue().split('~')[1]);
        if (gstRate) {
            if (gstRate != 0) {
                var gstDis = (gstRate / 100) + 1;
                if (cddlVatGstCst.GetValue().split('~')[2] == "G") {
                    $('.lblChargesGSTforNet').hide();
                    ctxtProductAmount.SetText(Math.round(sumAmount / gstDis).toFixed(2));
                    document.getElementById('HdChargeProdAmt').value = Math.round(sumAmount / gstDis).toFixed(2);
                    clblChargesGSTforGross.SetText(Math.round(sumAmount - parseFloat(document.getElementById('HdChargeProdAmt').value)).toFixed(2));
                    clblChargesTaxableGross.SetText("(Taxable)");

                }
                else {
                    $('.lblChargesGSTforGross').hide();
                    ctxtProductNetAmount.SetText(Math.round(sumNetAmount / gstDis).toFixed(2));
                    document.getElementById('HdChargeProdNetAmt').value = Math.round(sumNetAmount / gstDis).toFixed(2);
                    clblChargesGSTforNet.SetText(Math.round(sumNetAmount - parseFloat(document.getElementById('HdChargeProdNetAmt').value)).toFixed(2));
                    clblChargesTaxableNet.SetText("(Taxable)");
                }
            }

        } else {
            $('.lblChargesGSTforGross').hide();
            $('.lblChargesGSTforNet').hide();
        }
    }
    else if (cddl_AmountAre.GetValue() == "1") {
        $('.lblChargesGSTforGross').hide();
        $('.lblChargesGSTforNet').hide();
    }              
    ctxtTotalAmount.SetValue(parseFloat(ctxtQuoteTaxTotalAmt.GetValue()) + parseFloat(ctxtProductNetAmount.GetValue()));

    gridTax.PerformCallback('Display');
    //Checking is gstcstvat will be hidden or not
    if (cddl_AmountAre.GetValue() == "2") {
        $('.chargeGstCstvatClass').hide();
    }
    else if (cddl_AmountAre.GetValue() == "1") {
        $('.chargeGstCstvatClass').show();
    }
    //End here
    $('.RecalculateCharge').hide();
    cPopup_Taxes.Show();
    gridTax.StartEditRow(0);
}

function QuotationTaxAmountTextChange(s, e) {
    //var Amount = ctxtProductAmount.GetValue();
    var Amount = gridTax.GetEditor("calCulatedOn").GetValue();
    var GlobalTaxAmt = 0;
    //var Percentage = (gridTax.GetEditor('Percentage').GetValue() != null) ? gridTax.GetEditor('Percentage').GetValue() : "0";
    //var Percentage = s.GetText();
    var totLength = gridTax.GetEditor("TaxName").GetText().length;
    var sign = gridTax.GetEditor("TaxName").GetText().substring(totLength - 3);
    //Sum = ((parseFloat(Amount) * parseFloat(Percentage)) / 100);

    if (sign == '(+)') {
        GlobalTaxAmt = parseFloat(gridTax.GetEditor("Amount").GetValue());
        //gridTax.GetEditor("Amount").SetValue(Sum);
        ctxtQuoteTaxTotalAmt.SetValue(parseFloat(ctxtQuoteTaxTotalAmt.GetValue()) + GlobalTaxAmt - taxAmountGlobalCharges);
        ctxtTotalAmount.SetText(parseFloat(Amount) + parseFloat(ctxtQuoteTaxTotalAmt.GetValue()));
        //ctxtTotalAmount.SetText(parseFloat(ctxtTotalAmount.GetValue()) + parseFloat(Sum) - GlobalTaxAmt);
        GlobalTaxAmt = 0;
        SetOtherChargeTaxValueOnRespectiveRow(0, s.GetValue(), gridTax.GetEditor("TaxName").GetText());
    }
    else {
        GlobalTaxAmt = parseFloat(gridTax.GetEditor("Amount").GetValue());
        //gridTax.GetEditor("Amount").SetValue(Sum);
        ctxtQuoteTaxTotalAmt.SetValue(parseFloat(ctxtQuoteTaxTotalAmt.GetValue()) - GlobalTaxAmt + taxAmountGlobalCharges);
        ctxtTotalAmount.SetValue(parseFloat(Amount) + parseFloat(ctxtQuoteTaxTotalAmt.GetValue()));
        //ctxtTotalAmount.SetText(parseFloat(ctxtTotalAmount.GetValue()) - parseFloat(Sum) + GlobalTaxAmt);
        GlobalTaxAmt = 0;
        SetOtherChargeTaxValueOnRespectiveRow(0, s.GetValue(), gridTax.GetEditor("TaxName").GetText());
    }

    RecalCulateTaxTotalAmountCharges();
}
function OnTaxEndCallback(s, e) {
    GetPercentageData();
    $('.gridTaxClass').show();
    if (gridTax.GetVisibleRowsOnPage() == 0) {
        $('.gridTaxClass').hide();
        ccmbGstCstVatcharge.Focus();
    }
    else {
        gridTax.StartEditRow(0);
    }
    //check Json data
    if (gridTax.cpJsonChargeData) {
        if (gridTax.cpJsonChargeData != "") {
            chargejsonTax = JSON.parse(gridTax.cpJsonChargeData);
            gridTax.cpJsonChargeData = null;
        }
    }

    //Set Total Charges And total Amount
    if (gridTax.cpTotalCharges) {
        if (gridTax.cpTotalCharges != "") {
            ctxtQuoteTaxTotalAmt.SetValue(gridTax.cpTotalCharges);
            ctxtTotalAmount.SetValue(parseFloat(ctxtQuoteTaxTotalAmt.GetValue()) + parseFloat(ctxtProductNetAmount.GetValue()));
            gridTax.cpTotalCharges = null;
        }
    }


    SetChargesRunningTotal();
    ShowTaxPopUp("IN");
}
function QuotationTaxAmountGotFocus(s, e) {
    taxAmountGlobalCharges = parseFloat(s.GetValue());
}
function PercentageTextChange(s, e) {
    //var Amount = ctxtProductAmount.GetValue();
    var Amount = gridTax.GetEditor("calCulatedOn").GetValue();
    var GlobalTaxAmt = 0;
    //var Percentage = (gridTax.GetEditor('Percentage').GetValue() != null) ? gridTax.GetEditor('Percentage').GetValue() : "0";
    var Percentage = s.GetText();
    var totLength = gridTax.GetEditor("TaxName").GetText().length;
    var sign = gridTax.GetEditor("TaxName").GetText().substring(totLength - 3);
    Sum = ((parseFloat(Amount) * parseFloat(Percentage)) / 100);

    if (sign == '(+)') {
        GlobalTaxAmt = parseFloat(gridTax.GetEditor("Amount").GetValue());
        gridTax.GetEditor("Amount").SetValue(Sum);
        ctxtQuoteTaxTotalAmt.SetValue(parseFloat(ctxtQuoteTaxTotalAmt.GetValue()) + parseFloat(Sum) - GlobalTaxAmt);
        ctxtTotalAmount.SetValue(parseFloat(Amount) + parseFloat(ctxtQuoteTaxTotalAmt.GetValue()));
        //ctxtTotalAmount.SetText(parseFloat(ctxtTotalAmount.GetValue()) + parseFloat(Sum) - GlobalTaxAmt);
        GlobalTaxAmt = 0;
    }
    else {
        GlobalTaxAmt = parseFloat(gridTax.GetEditor("Amount").GetValue());
        gridTax.GetEditor("Amount").SetValue(Sum);
        ctxtQuoteTaxTotalAmt.SetValue(parseFloat(ctxtQuoteTaxTotalAmt.GetValue()) - parseFloat(Sum) + GlobalTaxAmt);
        ctxtTotalAmount.SetValue(parseFloat(Amount) + parseFloat(ctxtQuoteTaxTotalAmt.GetValue()));
        //ctxtTotalAmount.SetText(parseFloat(ctxtTotalAmount.GetValue()) - parseFloat(Sum) + GlobalTaxAmt);
        GlobalTaxAmt = 0;
    }

    SetOtherChargeTaxValueOnRespectiveRow(0, Sum, gridTax.GetEditor("TaxName").GetText());
    SetChargesRunningTotal();

    RecalCulateTaxTotalAmountCharges();
}
function RecalCulateTaxTotalAmountCharges() {
    var totalTaxAmount = 0;
    for (var i = 0; i < chargejsonTax.length; i++) {
        gridTax.batchEditApi.StartEdit(i, 3);
        var totLength = gridTax.GetEditor("TaxName").GetText().length;
        var sign = gridTax.GetEditor("TaxName").GetText().substring(totLength - 3);
        if (sign == '(+)') {
            totalTaxAmount = totalTaxAmount + parseFloat(gridTax.GetEditor("Amount").GetValue());
        } else {
            totalTaxAmount = totalTaxAmount - parseFloat(gridTax.GetEditor("Amount").GetValue());
        }

        gridTax.batchEditApi.EndEdit();
    }

    totalTaxAmount = totalTaxAmount + parseFloat(ctxtGstCstVatCharge.GetValue());

    // ctxtQuoteTaxTotalAmt.SetValue(Math.round(totalTaxAmount));
    ctxtQuoteTaxTotalAmt.SetValue(totalTaxAmount);
    ctxtTotalAmount.SetValue(parseFloat(ctxtQuoteTaxTotalAmt.GetValue()) + parseFloat(ctxtProductNetAmount.GetValue()));
}

function ShowTaxPopUp(type) {
    if (type == "IY") {
        $('#ContentErrorMsg').hide();
        $('#content-6').show();


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
function gridFocusedRowChanged(s, e) {
    globalRowIndex = e.visibleIndex;
}
function TaxAmountKeyDown(s, e) {

    if (e.htmlEvent.key == "Enter") {
        s.OnButtonClick(0);
    }
}


function taxAmountGotFocus(s, e) {
    taxAmountGlobal = parseFloat(s.GetValue());
}

function taxAmountLostFocus(s, e) {
    var finalTaxAmt = parseFloat(s.GetValue());
    var totAmt = parseFloat(ctxtTaxTotAmt.GetText());
    var totLength = cgridTax.GetEditor("Taxes_Name").GetText().length;
    var sign = cgridTax.GetEditor("Taxes_Name").GetText().substring(totLength - 3);
    if (sign == '(+)') {
        ctxtTaxTotAmt.SetValue(Math.round(totAmt + finalTaxAmt - taxAmountGlobal));
    } else {
        ctxtTaxTotAmt.SetValue(Math.round(totAmt + (finalTaxAmt * -1) - (taxAmountGlobal * -1)));
    }


    //SetOtherTaxValueOnRespectiveRow(0, cgridTax.GetEditor("Amount").GetValue(), cgridTax.GetEditor("Taxes_Name").GetText().replace('(+)', '').replace('(-)', ''));
    SetOtherTaxValueOnRespectiveRow(0, cgridTax.GetEditor("Amount").GetValue(), cgridTax.GetEditor("Taxes_Name").GetText().replace('(+)', '').replace('(-)', ''),parseFloat(cgridTax.GetEditor("calCulatedOn").GetValue()),sign);
    
    //Set Running Total
    SetRunningTotal();

    RecalCulateTaxTotalAmountInline();
}

function cmbGstCstVatChange(s, e) {

    SetOtherTaxValueOnRespectiveRow(0, 0, gstcstvatGlobalName);
    $('.RecalculateInline').hide();
    var ProdAmt = parseFloat(ctxtprodBasicAmt.GetValue());
    if (s.GetValue().split('~')[2] == 'G') {
        ProdAmt = parseFloat(clblTaxProdGrossAmt.GetValue());
    }
    else if (s.GetValue().split('~')[2] == 'N') {
        ProdAmt = parseFloat(clblProdNetAmt.GetValue());
    }
    else if (s.GetValue().split('~')[2] == 'O') {
        //Check for Other Dependecy
        $('.RecalculateInline').show();
        ProdAmt = 0;
        var taxdependentName = s.GetValue().split('~')[3];
        for (var i = 0; i < taxJson.length; i++) {
            cgridTax.batchEditApi.StartEdit(i, 3);
            var gridTaxName = cgridTax.GetEditor("Taxes_Name").GetText();
            gridTaxName = gridTaxName.substring(0, gridTaxName.length - 3).trim();
            if (gridTaxName == taxdependentName) {
                ProdAmt = cgridTax.GetEditor("Amount").GetValue();
            }
        }
    }
    else if (s.GetValue().split('~')[2] == 'R') {
        ProdAmt = GetTotalRunningAmount();
        $('.RecalculateInline').show();
    }

    GlobalCurTaxAmt = parseFloat(ctxtGstCstVat.GetText());

    var calculatedValue = parseFloat(ProdAmt * ccmbGstCstVat.GetValue().split('~')[1]) / 100;
    ctxtGstCstVat.SetValue(calculatedValue);

    var totAmt = parseFloat(ctxtTaxTotAmt.GetText());
    ctxtTaxTotAmt.SetValue(Math.round(totAmt + calculatedValue - GlobalCurTaxAmt));

    //tax others
    SetOtherTaxValueOnRespectiveRow(0, calculatedValue, ccmbGstCstVat.GetText());
    gstcstvatGlobalName = ccmbGstCstVat.GetText();
}


//for tax and charges

function ChargecmbGstCstVatChange(s, e) {

    SetOtherChargeTaxValueOnRespectiveRow(0, 0, ChargegstcstvatGlobalName);
    $('.RecalculateCharge').hide();
    var ProdAmt = parseFloat(ctxtProductAmount.GetValue());

    //Set ProductAmount
    if (s.GetValue().split('~')[2] == 'G') {
        ProdAmt = parseFloat(ctxtProductAmount.GetValue());
    }
    else if (s.GetValue().split('~')[2] == 'N') {
        ProdAmt = parseFloat(clblProdNetAmt.GetValue());
    }
    else if (s.GetValue().split('~')[2] == 'O') {
        //Check for Other Dependecy
        $('.RecalculateCharge').show();
        ProdAmt = 0;
        var taxdependentName = s.GetValue().split('~')[3];
        for (var i = 0; i < taxJson.length; i++) {
            gridTax.batchEditApi.StartEdit(i, 3);
            var gridTaxName = gridTax.GetEditor("TaxName").GetText();
            gridTaxName = gridTaxName.substring(0, gridTaxName.length - 3).trim();
            if (gridTaxName == taxdependentName) {
                ProdAmt = gridTax.GetEditor("Amount").GetValue();
            }
        }
    }
    else if (s.GetValue().split('~')[2] == 'R') {
        $('.RecalculateCharge').show();
        ProdAmt = GetChargesTotalRunningAmount();
    }


    GlobalCurChargeTaxAmt = parseFloat(ctxtGstCstVatCharge.GetText());

    var calculatedValue = parseFloat(ProdAmt * ccmbGstCstVatcharge.GetValue().split('~')[1]) / 100;
    ctxtGstCstVatCharge.SetValue(calculatedValue);

    var totAmt = parseFloat(ctxtQuoteTaxTotalAmt.GetText());
    ctxtQuoteTaxTotalAmt.SetValue(totAmt + calculatedValue - GlobalCurChargeTaxAmt);

    //tax others
    SetOtherChargeTaxValueOnRespectiveRow(0, calculatedValue, ctxtGstCstVatCharge.GetText());
    ChargegstcstvatGlobalName = ctxtGstCstVatCharge.GetText();

    //set Total Amount
    ctxtTotalAmount.SetValue(parseFloat(ctxtQuoteTaxTotalAmt.GetValue()) + parseFloat(ctxtProductNetAmount.GetValue()));
}

function GetChargesTotalRunningAmount() {
    var runningTot = parseFloat(ctxtProductNetAmount.GetValue());
    for (var i = 0; i < chargejsonTax.length; i++) {
        gridTax.batchEditApi.StartEdit(i, 3);
        runningTot = runningTot + parseFloat(gridTax.GetEditor("Amount").GetValue());
        gridTax.batchEditApi.EndEdit();
    }

    return runningTot;
}
function chargeCmbtaxClick(s, e) {
    GlobalCurChargeTaxAmt = parseFloat(ctxtGstCstVatCharge.GetText());
    ChargegstcstvatGlobalName = s.GetText();
}


function gridFocusedRowChanged(s, e) {
    globalRowIndex = e.visibleIndex;
}

function GetVisibleIndex(s, e) {
    globalRowIndex = e.visibleIndex;
}
function GetTaxVisibleIndex(s, e) {
    globalTaxRowIndex = e.visibleIndex;
}
function cmbtaxCodeindexChange(s, e) {
    if (cgridTax.GetEditor("Taxes_Name").GetText() == "GST/CST/VAT") {
        var taxValue = s.GetValue();
        if (taxValue == null) {
            taxValue = 0;
            GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());
            cgridTax.GetEditor("Amount").SetValue(0);
            ctxtTaxTotAmt.SetValue(parseFloat(ctxtTaxTotAmt.GetValue()) - GlobalCurTaxAmt);
        }
        var isValid = taxValue.indexOf('~');
        if (isValid != -1) {
            var rate = parseFloat(taxValue.split('~')[1]);
            var ProdAmt = parseFloat(ctxtprodBasicAmt.GetValue());
            GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());
            cgridTax.GetEditor("Amount").SetValue(parseFloat(ProdAmt * rate) / 100);
            ctxtTaxTotAmt.SetValue(parseFloat(ctxtTaxTotAmt.GetValue()) + (parseFloat(ProdAmt * rate) / 100) - GlobalCurTaxAmt);
            GlobalCurTaxAmt = 0;
        }
        else {
            s.SetText("");
        }

    } else {
        var ProdAmt = parseFloat(ctxtprodBasicAmt.GetValue());
        if (s.GetValue() == null) {
            s.SetValue(0);
        }
        if (!isNaN(parseFloat(ProdAmt * s.GetText()) / 100)) {
            GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());
            cgridTax.GetEditor("Amount").SetValue(parseFloat(ProdAmt * s.GetText()) / 100);
            ctxtTaxTotAmt.SetValue(parseFloat(ctxtTaxTotAmt.GetValue()) + (parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) - GlobalCurTaxAmt);
            GlobalCurTaxAmt = 0;
        } else {
            s.SetText("");
        }
    }
}

function SetOtherTaxValueOnRespectiveRow(idx, amt, name,runninTot,signCal) {
    for (var i = 0; i < taxJson.length; i++) {
        if (taxJson[i].applicableBy == name) {
            cgridTax.batchEditApi.StartEdit(i, 3);
            var totCal=0;
            if (signCal == '(+)') {
                totCal= parseFloat(parseFloat(amt)+parseFloat(runninTot));
            }
            else{
                totCal= parseFloat(parseFloat(runninTot)-parseFloat(amt));
            }
            cgridTax.GetEditor('calCulatedOn').SetValue(totCal);

            var totLength = cgridTax.GetEditor("Taxes_Name").GetText().length;
            var taxNameWithSign = cgridTax.GetEditor("TaxField").GetText();
            var sign = cgridTax.GetEditor("Taxes_Name").GetText().substring(totLength - 3);
            var ProdAmt = parseFloat(cgridTax.GetEditor("calCulatedOn").GetValue());
            var s = cgridTax.GetEditor("TaxField");
            if (sign == '(+)') {
                GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());
                cgridTax.GetEditor("Amount").SetValue(parseFloat(ProdAmt * s.GetText()) / 100);

                //ctxtTaxTotAmt.SetValue(parseFloat(ctxtTaxTotAmt.GetValue()) + (parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) - GlobalCurTaxAmt);
                ctxtTaxTotAmt.SetValue(((ctxtTaxTotAmt.GetValue()) + ((ProdAmt * (s.GetText() * 1)) / 100) - (GlobalCurTaxAmt * 1)).toFixed(2));
                GlobalCurTaxAmt = 0;
            }
            else {
                GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());
                cgridTax.GetEditor("Amount").SetValue((parseFloat(ProdAmt * s.GetText()) / 100) * -1);

                //ctxtTaxTotAmt.SetValue(parseFloat(ctxtTaxTotAmt.GetValue()) + ((parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) * -1) - (GlobalCurTaxAmt * -1));
                ctxtTaxTotAmt.SetValue(((ctxtTaxTotAmt.GetValue() * 1) + (((ProdAmt * (s.GetText() * 1)) / 100) * -1) - (GlobalCurTaxAmt * -1)).toFixed(2));
                GlobalCurTaxAmt = 0;
            }
        }
    }
    //return;
    cgridTax.batchEditApi.EndEdit();

}
function SetOtherChargeTaxValueOnRespectiveRow(idx, amt, name) {
    name = name.substring(0, name.length - 3).trim();
    for (var i = 0; i < chargejsonTax.length; i++) {
        if (chargejsonTax[i].applicableBy == name) {
            gridTax.batchEditApi.StartEdit(i, 3);
            gridTax.GetEditor('calCulatedOn').SetValue(amt);
            var totLength = gridTax.GetEditor("TaxName").GetText().length;
            var taxNameWithSign = gridTax.GetEditor("Percentage").GetText();
            var sign = gridTax.GetEditor("TaxName").GetText().substring(totLength - 3);
            var ProdAmt = parseFloat(gridTax.GetEditor("calCulatedOn").GetValue());
            var s = gridTax.GetEditor("Percentage");
            if (sign == '(+)') {
                GlobalCurTaxAmt = parseFloat(gridTax.GetEditor("Amount").GetValue());
                gridTax.GetEditor("Amount").SetValue(parseFloat(ProdAmt * s.GetText()) / 100);
                ctxtTaxTotAmt.SetValue(parseFloat(ctxtTaxTotAmt.GetValue()) + (parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) - GlobalCurTaxAmt);
                GlobalCurTaxAmt = 0;
            }
            else {

                GlobalCurTaxAmt = parseFloat(gridTax.GetEditor("Amount").GetValue());
                gridTax.GetEditor("Amount").SetValue((parseFloat(ProdAmt * s.GetText()) / 100) * -1);
                ctxtTaxTotAmt.SetValue(parseFloat(ctxtTaxTotAmt.GetValue()) + ((parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) * -1) - (GlobalCurTaxAmt * -1));
                GlobalCurTaxAmt = 0;
            }
        }
    }
    //return;
    gridTax.batchEditApi.EndEdit();
}
function txtPercentageLostFocus(s, e) {
    var ProdAmt = parseFloat(cgridTax.GetEditor("calCulatedOn").GetValue());
    if (s.GetText().trim() != '') {
        if (!isNaN(parseFloat(ProdAmt * s.GetText()) / 100)) {
            //Checking Add or less
            var totLength = cgridTax.GetEditor("Taxes_Name").GetText().length;
            var taxNameWithSign = cgridTax.GetEditor("TaxField").GetText();
            var sign = cgridTax.GetEditor("Taxes_Name").GetText().substring(totLength - 3);
            if (sign == '(+)') {
                GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());
                cgridTax.GetEditor("Amount").SetValue(parseFloat(ProdAmt * s.GetText()) / 100);
                //ctxtTaxTotAmt.SetValue(Math.round(parseFloat(ctxtTaxTotAmt.GetValue()) + (parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) - GlobalCurTaxAmt));
                ctxtTaxTotAmt.SetValue(parseFloat(ctxtTaxTotAmt.GetValue()) + (parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) - GlobalCurTaxAmt);
                GlobalCurTaxAmt = 0;
            }
            else {

                GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());
                cgridTax.GetEditor("Amount").SetValue((parseFloat(ProdAmt * s.GetText()) / 100) * -1);
                ctxtTaxTotAmt.SetValue(parseFloat(ctxtTaxTotAmt.GetValue()) + ((parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) * -1) - (GlobalCurTaxAmt * -1));
                //ctxtTaxTotAmt.SetValue(Math.round(parseFloat(ctxtTaxTotAmt.GetValue()) + ((parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) * -1) - (GlobalCurTaxAmt * -1)));
                GlobalCurTaxAmt = 0;
            }

            SetOtherTaxValueOnRespectiveRow(0, cgridTax.GetEditor("Amount").GetValue(), cgridTax.GetEditor("Taxes_Name").GetText().replace('(+)', '').replace('(-)', ''),parseFloat(cgridTax.GetEditor("calCulatedOn").GetValue()),sign);

           // SetOtherTaxValueOnRespectiveRow(0, cgridTax.GetEditor("Amount").GetValue(), cgridTax.GetEditor("Taxes_Name").GetText().replace('(+)', '').replace('(-)', ''));
            //Call for Running Total
            SetRunningTotal();
        } else {
            s.SetText("");
        }
    }
    RecalCulateTaxTotalAmountInline();
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
    //ctxtTaxTotAmt.SetValue(Math.round(totalInlineTaxAmount));
    ctxtTaxTotAmt.SetValue(totalInlineTaxAmount);
}

function SetRunningTotal() {

    var runningTot = parseFloat(clblProdNetAmt.GetValue());
    for (var i = 0; i < taxJson.length; i++) {
        cgridTax.batchEditApi.StartEdit(i, 3);
        var sign = cgridTax.GetEditor("Taxes_Name").GetText().substring(totLength - 3);
        if (taxJson[i].applicableOn == "R") {
            cgridTax.GetEditor("calCulatedOn").SetValue(runningTot);
            var totLength = cgridTax.GetEditor("Taxes_Name").GetText().length;
            var taxNameWithSign = cgridTax.GetEditor("TaxField").GetText();
            
            var ProdAmt = parseFloat(cgridTax.GetEditor("calCulatedOn").GetValue());
            var thisRunningAmt = 0;
            if (sign == '(+)') {
                GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());
                cgridTax.GetEditor("Amount").SetValue(parseFloat(ProdAmt * cgridTax.GetEditor("TaxField").GetValue()) / 100);

                //ctxtTaxTotAmt.SetValue(parseFloat(ctxtTaxTotAmt.GetValue()) + (parseFloat(ProdAmt * parseFloat(cgridTax.GetEditor("TaxField").GetValue())) / 100) - GlobalCurTaxAmt);
                ctxtTaxTotAmt.SetValue(((ctxtTaxTotAmt.GetValue() * 1) + ((ProdAmt * (cgridTax.GetEditor("TaxField").GetValue() * 1)) / 100) - (GlobalCurTaxAmt * 1)).toFixed(2));

                GlobalCurTaxAmt = 0;
            }
            else {

                GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());
                cgridTax.GetEditor("Amount").SetValue((parseFloat(ProdAmt * cgridTax.GetEditor("TaxField").GetValue()) / 100) * -1);

                //ctxtTaxTotAmt.SetValue(parseFloat(ctxtTaxTotAmt.GetValue()) + ((parseFloat(ProdAmt * parseFloat(cgridTax.GetEditor("TaxField").GetValue())) / 100) * -1) - (GlobalCurTaxAmt * -1));
                ctxtTaxTotAmt.SetValue(((ctxtTaxTotAmt.GetValue() * 1) + (((ProdAmt * (cgridTax.GetEditor("TaxField").GetValue() * 1)) / 100) * -1) - (GlobalCurTaxAmt * -1)).toFixed(2));
                GlobalCurTaxAmt = 0;
            }
            SetOtherTaxValueOnRespectiveRow(0, cgridTax.GetEditor("Amount").GetValue(), cgridTax.GetEditor("Taxes_Name").GetText().replace('(+)', '').replace('(-)', ''),ProdAmt,sign);
        }
        if (sign == '(+)') {
            runningTot = runningTot + parseFloat(cgridTax.GetEditor("Amount").GetValue());
        }
        else
        {
            runningTot = runningTot - parseFloat(cgridTax.GetEditor("Amount").GetValue());
        }
        
        cgridTax.batchEditApi.EndEdit();
    }
}

function GetTotalRunningAmount() {
    var runningTot = parseFloat(clblProdNetAmt.GetValue());
    for (var i = 0; i < taxJson.length; i++) {
        cgridTax.batchEditApi.StartEdit(i, 3);
        runningTot = runningTot + parseFloat(cgridTax.GetEditor("Amount").GetValue());
        cgridTax.batchEditApi.EndEdit();
    }
    return runningTot;
}
function CmbtaxClick(s, e) {
    GlobalCurTaxAmt = parseFloat(ctxtGstCstVat.GetText());
    gstcstvatGlobalName = s.GetText();
}
function txtTax_TextChanged(s, i, e) {
    cgridTax.batchEditApi.StartEdit(i, 2);
    var ProdAmt = parseFloat(ctxtprodBasicAmt.GetValue());
    cgridTax.GetEditor("Amount").SetValue(parseFloat(ProdAmt * s.GetText()) / 100);
}
function BatchUpdate() {
    var _SrlNo = document.getElementById('HdSerialNo').value;
    if (TaxOfProduct.filter(function (e) { return e.SrlNo == _SrlNo; }).length == 0) {
        var ProductTaxes = { SrlNo: _SrlNo, IsTaxEntry: "Y" }
        TaxOfProduct.push(ProductTaxes)
    }
    else {
        $.grep(TaxOfProduct, function (e) { if (e.SrlNo == _SrlNo) e.IsTaxEntry = "Y"; });
    }

    if (cgridTax.GetVisibleRowsOnPage() > 0) {
        cgridTax.UpdateEdit();
    }
    else {
        cgridTax.PerformCallback('SaveGST');
    }
    return false;
}
function cgridTax_EndCallBack(s, e) {
    //cgridTax.batchEditApi.StartEdit(0, 1);
    $('.cgridTaxClass').show();
    cgridTax.StartEditRow(0);
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
            if (cddl_AmountAre.GetValue() == "1") {
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
            }
        }
    }
    if (cgridTax.cpUpdated.split('~')[0] == 'ok') {
        ctxtTaxTotAmt.SetValue(Math.round(cgridTax.cpUpdated.split('~')[1]));
        var gridValue = parseFloat(cgridTax.cpUpdated.split('~')[1]).toFixed(2);
        var ddValue = parseFloat(ctxtGstCstVat.GetValue()).toFixed(2);

        ctxtTaxTotAmt.SetValue(parseFloat(gridValue) + parseFloat(ddValue));
        cgridTax.cpUpdated = "";
    }
    else {
        var totAmt = ctxtTaxTotAmt.GetValue();
        cgridTax.CancelEdit();
        caspxTaxpopUp.Hide();
        grid.batchEditApi.StartEdit(globalRowIndex, 13);
        grid.GetEditor("gvColTaxAmount").SetValue(totAmt);
        if (cddl_AmountAre.GetValue() == "2") {
            var totalNetAmount = parseFloat(totAmt) + parseFloat(grid.GetEditor("gvColAmount").GetValue());
            var totalRoundOffAmount = Math.round(totalNetAmount);

            grid.GetEditor("gvColTotalAmountINR").SetValue(totalRoundOffAmount);
            grid.GetEditor("gvColAmount").SetValue(DecimalRoundoff(parseFloat(grid.GetEditor("gvColAmount").GetValue()) + (totalRoundOffAmount - totalNetAmount), 2));
        }
        else {
            grid.GetEditor("gvColTotalAmountINR").SetValue(DecimalRoundoff(parseFloat(totAmt) + parseFloat(grid.GetEditor("gvColAmount").GetValue()), 2));
        }
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
function recalculateTax() {
    cmbGstCstVatChange(ccmbGstCstVat);
}
function recalculateTaxCharge() {
    ChargecmbGstCstVatChange(ccmbGstCstVatcharge);
}
/*............................End Tax...........................................*/
//Rev Sayantani
function BindOrderProjectdata(OrderId,TagDocType)
{
    // debugger;
    var OtherDetail = {};          
    OtherDetail.OrderId = OrderId;
    var checked = $("[id$='rdl_Salesquotation']").find(":checked").val();
    if (checked=="Indent") {             
        OtherDetail.TagDocType = "POIN";
    }
    else if(checked=="Quotation")
    {
        OtherDetail.TagDocType = "PurchaQuote";
    }        
          
    if ((OrderId != null) && (OrderId != "")) {

        $.ajax({
            type: "POST",
            url: "PurchaseOrder.aspx/SetProjectCode",
            data: JSON.stringify(OtherDetail),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                var  Code = msg.d;
                        
                clookup_Project.gridView.SelectItemsByKey(Code[0].ProjectId);
                clookup_Project.SetEnabled(false);
            }
        });
    }
}
// End of Rev Sayantani

function PerformCallToGridBind() {            
    var OrderTaggingData = cgridproducts.GetSelectedKeysOnPage();
    cPurchaseOrderPosGst.SetEnabled(false);
    if(OrderTaggingData==0){ 
        grid.PerformCallback('BindGridOnQuotation' + '~' + '@');
        cProductsPopup.Hide();
    }
    else{
        grid.PerformCallback('BindGridOnQuotation' + '~' + '@');
        // cQuotationComponentPanel.PerformCallback('BindQuotationGridOnSelection');
        $('#hdnPageStatus').val('Quoteupdate');               
        var quote_Id = ctaggingGrid.GetSelectedKeysOnPage();               
        if (quote_Id.length > 0) {
            var ComponentDetails = _ComponentDetails.split("~");
            cgridproducts.cpComponentDetails = null;
            var ComponentNumber = ComponentDetails[0];
            var ComponentDate = ComponentDetails[1];        
            ctaggingList.SetValue(ComponentNumber);
            cPLQADate.SetValue(ComponentDate);
            cPLQuoteDate.SetEnabled(false);
        }
        if (quote_Id.length > 0) {
            BindOrderProjectdata(quote_Id[0],$("#hdnTagDocType").val());
        }
        cProductsPopup.Hide();               
    }
}
function componentEndCallBack(s, e) {            
    if (cQuotationComponentPanel.cpNullGrid != null) {
        deleteAllRows();
        if (grid.GetVisibleRowsOnPage() == 0) {
            OnAddNewClick();
        }
        grid.GetEditor('ProductName').SetEnabled(true);
        cPLQADate.SetText('');
    }
    else {
        gridquotationLookup.gridView.Refresh();
        if (grid.GetVisibleRowsOnPage() == 0) {
            OnAddNewClick();
            grid.GetEditor('ProductName').SetEnabled(true);
            cPLQADate.SetText('');
        }
    }
}
function CloseGridQuotationLookup() {
    gridquotationLookup.ConfirmCurrentSelection();
    gridquotationLookup.HideDropDown();
    gridquotationLookup.Focus();
}
var SimilarProjectStatus = "0";

function SimilarProjetcheck(quote_Id, Doctype) {
    $.ajax({
        type: "POST",
        url: "PurchaseOrder.aspx/DocWiseSimilarProjectCheck",
        data: JSON.stringify({ quote_Id: quote_Id, Doctype: Doctype }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (msg) {
            SimilarProjectStatus = msg.d;
            if (SimilarProjectStatus != "1") {
                cPLQADate.SetText("");
                jAlert("Please select document with same project code to proceed.");

                return false;

            }
        }
    });
}
//function QuotationNumberChanged() {

//    document.getElementById('hdfTagMendatory').value = 'No';
//    $("#MandatorysIndentReq").hide();            
//    var OrderData = ctaggingGrid.GetSelectedKeysOnPage();
//    if(OrderData==0){
//        cgridproducts.PerformCallback('BindProductsDetails');
//        cpopup_taggingGrid.Hide();
//        cProductsPopup.Show();
//    }
//    else{
//        cgridproducts.PerformCallback('BindProductsDetails');
//        cpopup_taggingGrid.Hide();
//        cProductsPopup.Show();
//    }               
//}
function QuotationNumberChanged() {

    document.getElementById('hdfTagMendatory').value = 'No';
    $("#MandatorysIndentReq").hide();            
    var OrderData = ctaggingGrid.GetSelectedKeysOnPage();


    var quotetag_Id = ctaggingGrid.GetSelectedKeysOnPage();

    if (quotetag_Id.length > 0 && $("#hdnProjectSelectInEntryModule").val() == "1") {
        var Doctype = $("#rdl_Salesquotation").find(":checked").val();
        var quote_Id = "";
        // otherDets.quote_Id = quote_Id;
        for (var i = 0; i < quotetag_Id.length; i++) {
            if (quote_Id == "") {
                quote_Id = quotetag_Id[i];
            }
            else {
                quote_Id += ',' + quotetag_Id[i];
            }
        }

        SimilarProjetcheck(quote_Id, Doctype);
    }


    if (SimilarProjectStatus != "-1") {
        if (OrderData == 0) {
            cgridproducts.PerformCallback('BindProductsDetails');
            cpopup_taggingGrid.Hide();
            cProductsPopup.Show();
        }
        else {
            cgridproducts.PerformCallback('BindProductsDetails');
            cpopup_taggingGrid.Hide();
            cProductsPopup.Show();
        }
    }
}
function SetDifference1() {
    var diff = CheckDifferenceOfFromDateWithTodate();
}
function CheckDifferenceOfFromDateWithTodate() {
    var startDate = new Date();
    var endDate = new Date();
    var difference = -1;
    startDate = cPLSalesOrderDate.GetDate();
    if (startDate != null) {
        endDate = cExpiryDate.GetDate();
        var startTime = startDate.getTime();
        var endTime = endDate.getTime();
        difference = (startTime - endTime) / 86400000;

    }
    return difference;
}

function BtnVisible() {
    document.getElementById('btnSaveExit').style.display = 'none'
    document.getElementById('btn_SaveRecords').style.display = 'none'
    document.getElementById('tagged').style.display = 'block'
}

       

function GridCallBack() {             
    grid.PerformCallback('Display');
}


function AutoPopulateMultiUOM() {

    var Productdetails = (grid.GetEditor('gvColProduct').GetText() != null) ? grid.GetEditor('gvColProduct').GetText() : "0";
    var ProductID = Productdetails.split("||@||")[0];
    var QuantityValue = (grid.GetEditor('gvColQuantity').GetValue() != null) ? grid.GetEditor('gvColQuantity').GetValue() : "0";

    $.ajax({
        type: "POST",
        url: "PurchaseOrder.aspx/AutoPopulateAltQuantity",
        data: JSON.stringify({ ProductID: ProductID }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
                   
            if (msg.d.length != 0) {
                var packingQuantity = msg.d[0].packing_quantity;
                var sProduct_quantity = msg.d[0].sProduct_quantity;
                var AltUOMId = msg.d[0].AltUOMId;
            }
            else {
                var packingQuantity = 0;
                var sProduct_quantity = 0;
                var AltUOMId = 0;
            }
            var uomfactor = 0
            if (sProduct_quantity != 0 && packingQuantity != 0) {
                uomfactor = parseFloat(packingQuantity / sProduct_quantity);
                $('#hddnuomFactor').val(parseFloat(packingQuantity / sProduct_quantity));
            }
            else {
                $('#hddnuomFactor').val(0);
            }

            var uomfac_Qty_to_stock = $('#hddnuomFactor').val();
            var Qty = QuantityValue;
            var calcQuantity = parseFloat(Qty * uomfac_Qty_to_stock);
            if ($("#hdnPageStatusForMultiUOM").val() == "Quoteupdate") {               
                ccmbSecondUOM.SetValue('');              
                cAltUOMQuantity.SetValue("0.0000");
            }
            else {               
                if (AltUOMId == 0) {
                    ccmbSecondUOM.SetValue('');
                }
                else {
                    ccmbSecondUOM.SetValue(AltUOMId);
                }
                //Rev Mantis Issue 24429
                // cAltUOMQuantity.SetValue(calcQuantity);
                //End Rev Mantis Issue 24429
            }

        }
    });
}


function FinalRemarks() {


    ccallback_InlineRemarks.PerformCallback('RemarksFinal' + '~' + grid.GetEditor('SrlNo').GetValue() + '~' + $('#txtInlineRemarks').val());
    $("#txtInlineRemarks").val('');
    cPopup_InlineRemarks.Hide();

}

function callback_InlineRemarks_EndCall(s, e) {

    if (ccallback_InlineRemarks.cpDisplayFocus == "DisplayRemarksFocus") {
        $("#txtInlineRemarks").focus();
    }
    else {
        cPopup_InlineRemarks.Hide();
        grid.batchEditApi.StartEdit(globalRowIndex, 7);
    }
}

function OnCustomButtonClick(s, e) {

    if (e.buttonID == 'CustomDelete') {
        grid.batchEditApi.EndEdit();      
        
        var totalNetAmount = grid.batchEditApi.GetCellValue(e.visibleIndex, 'gvColTotalAmountINR');
        var oldAmountWithTaxValue= parseFloat(cbnrlblAmountWithTaxValue.GetValue());
        var AfterdeleteoldAmountWithTaxValue=oldAmountWithTaxValue-parseFloat(totalNetAmount);
        cbnrlblAmountWithTaxValue.SetText(parseFloat(AfterdeleteoldAmountWithTaxValue).toFixed(2));
        //cbnrLblInvValue.SetText(parseFloat(AfterdeleteoldAmountWithTaxValue).toFixed(2));

        var RowQuantity = grid.batchEditApi.GetCellValue(e.visibleIndex, 'gvColQuantity');
        var totalquantity = parseFloat(cbnrLblTotalQty.GetValue());
        var updatedquantity=(parseFloat(totalquantity)-parseFloat(RowQuantity));
        //cbnrLblTotalQty.SetText(DecimalRoundoff(updatedquantity, 4));

        var rowTaxAmount = grid.batchEditApi.GetCellValue(e.visibleIndex,'gvColTaxAmount');
        var totaltaxamt=parseFloat(cbnrLblTaxAmtval.GetValue());
        var updatedtaxamt=parseFloat(totaltaxamt)-parseFloat(rowTaxAmount);
        //cbnrLblTaxAmtval.SetText(DecimalRoundoff(updatedtaxamt, 4));

        var rowAmount = grid.batchEditApi.GetCellValue(e.visibleIndex,'gvColAmount');
        var TotalAmt=parseFloat(cbnrLblTaxableAmtval.GetValue());
        var updatedAmt=parseFloat(TotalAmt)-parseFloat(rowAmount);
        //cbnrLblTaxableAmtval.SetText(DecimalRoundoff(updatedAmt, 4));

        if (ctaggingList.GetValue() != null) {         
            jAlert('Cannot Delete using this button as the Purchase Indent is linked with this Purchase Order.<br/>Click on Plus(+) sign to Add or Delete Product from last column.!', 'Alert Dialog', function (r) {

            });
        }
        var noofvisiblerows = grid.GetVisibleRowsOnPage();       
        if (noofvisiblerows != "1" && ctaggingList.GetValue() == null) {
            grid.DeleteRow(e.visibleIndex);

            cbnrLblInvValue.SetText(parseFloat(AfterdeleteoldAmountWithTaxValue).toFixed(2));
            cbnrLblTotalQty.SetText(DecimalRoundoff(updatedquantity, 4));
            cbnrLblTaxAmtval.SetText(DecimalRoundoff(updatedtaxamt, 4));
            cbnrLblTaxableAmtval.SetText(DecimalRoundoff(updatedAmt, 4));

            $('#hdfIsDelete').val('D');
            grid.UpdateEdit();
            grid.PerformCallback('Display');
            $('#hdnPageStatus').val('delete');           
        }
    }


    else if (e.buttonID == "addDescRemarks") {

        var index = e.visibleIndex;
        grid.batchEditApi.StartEdit(e.visibleIndex,5);
        cPopup_InlineRemarks.Show();

        $("#txtInlineRemarks").val('');

        var SrlNo = (grid.GetEditor('SrlNo').GetValue() != null) ? grid.GetEditor('SrlNo').GetValue() : "";
        var ProductID = (grid.GetEditor('gvColProduct').GetText() != null) ? grid.GetEditor('gvColProduct').GetText() : "";
        if (ProductID != "") {
            // ccallback_InlineRemarks.PerformCallback('BindRemarks'+'~' + '0'+'~'+'0');
            ccallback_InlineRemarks.PerformCallback('DisplayRemarks' + '~' + SrlNo + '~' + '0');

        }
        else {
            $("#txtInlineRemarks").val('');
        }
        //$("#txtInlineRemarks").focus();
        document.getElementById("txtInlineRemarks").focus();
    }

     else if (e.buttonID == 'CustomAddNewRow') {

        if (ctaggingList.GetValue() == null) {
            grid.batchEditApi.StartEdit(e.visibleIndex, 2);
            var Product = (grid.GetEditor('gvColProduct').GetValue() != null) ? grid.GetEditor('gvColProduct').GetValue() : "";
            var SpliteDetails = Product.split("||@||");
            var IsComponentProduct = SpliteDetails[16];
            var ComponentProduct = SpliteDetails[17];
            if (IsComponentProduct == "Y") {
                var messege = "Selected product is defined with components.<br/> Would you like to proceed with components (" + ComponentProduct + ") ?";
                jConfirm(messege, 'Confirmation Dialog', function (r) {
                    if (r == true) {
                        grid.batchEditApi.StartEdit(globalRowIndex);
                        var IsComponentProduct = grid.GetEditor("IsComponentProduct");
                        IsComponentProduct.SetValue("Y");
                        $('#hdfIsDelete').val('C');
                        grid.UpdateEdit();
                        grid.PerformCallback('Display~fromComponent');
                        
                    }
                    else {
                        OnAddNewClick();                       
                    }
                });
            }
            else if (Product != "") {
                OnAddNewClick();
                setTimeout(function () {
                    grid.batchEditApi.StartEdit(globalRowIndex, 3);
                }, 500);
                return false;
            }
            else {                
                setTimeout(function () {
                    grid.batchEditApi.StartEdit(globalRowIndex, 3);
                }, 50);
                return false;
               
            }
        }
        else {
            QuotationNumberChanged();
        }      
    }


    else if (e.buttonID == 'CustomMultiUOM') {
            
        var index = e.visibleIndex;
        grid.batchEditApi.StartEdit(e.visibleIndex);
        var Productdetails = (grid.GetEditor('gvColProduct').GetText() != null) ? grid.GetEditor('gvColProduct').GetText() : "0";
        var ProductID = Productdetails.split("||@||")[0];
        var UOMName = grid.GetEditor("gvColUOM").GetValue();
        var quantity = grid.GetEditor("gvColQuantity").GetValue();
        var DetailsId = grid.GetEditor('DetailsId').GetText();
        var StockUOM = Productdetails.split("||@||")[5];
        if (StockUOM == "") {
            StockUOM="0";
        }
        //$("#AltUOMQuantity").val(parseFloat(0).toFixed(4));
        cAltUOMQuantity.SetValue("0.0000");
        //Mantis Issue 24429
        //if ((ProductID != "") && (UOMName != "") && (quantity != "0.0000")) {
        if ((ProductID != "") && (UOMName != "")) {
        //End of Mantis Issue 24429

            if (StockUOM == "0") {
                jAlert("Main Unit Not Defined.");
            }

            else
            {
                if ($("#hddnMultiUOMSelection").val() == "1") {
                    ccmbUOM.SetEnabled(false);
                    var index = e.visibleIndex;
                    grid.batchEditApi.StartEdit(e.visibleIndex);
                    //grid.batchEditApi.StartEdit(globalRowIndex);
                    var Qnty = grid.GetEditor("gvColQuantity").GetValue();
                    var SrlNo = (grid.GetEditor('SrlNo').GetValue() != null) ? grid.GetEditor('SrlNo').GetValue() : "";
                    var UomId = grid.GetEditor('gvColProduct').GetText().split("||@||")[3];
                    ccmbUOM.SetValue(UomId);
                    // Mantis Issue 24429
                    //$("#UOMQuantity").val(Qnty);
                    $("#UOMQuantity").val("0.0000");
                    ccmbBaseRate.SetValue(0) 
                    cAltUOMQuantity.SetValue(0)
                    ccmbAltRate.SetValue(0)
                    ccmbSecondUOM.SetValue("")
                    // End of Mantis Issue 24429
                    cPopup_MultiUOM.Show();
                    cgrid_MultiUOM.cpDuplicateAltUOM = "";
                    AutoPopulateMultiUOM();
                    //chinmoy change start
                    cgrid_MultiUOM.PerformCallback('MultiUOMDisPlay~' + SrlNo + '~' + DetailsId);
                    //cgrid_MultiUOM.PerformCallback('MultiUOMDisPlay~' + ProductID);
                }     //End
            }
        }
        else {
            return;
        }
    }

    else if (e.buttonID == 'CustomWarehouse') {
        var index = e.visibleIndex;
        grid.batchEditApi.StartEdit(index, 2)
        Warehouseindex = index;

        var SrlNo = (grid.GetEditor('SrlNo').GetValue() != null) ? grid.GetEditor('SrlNo').GetValue() : "";
        var ProductID = (grid.GetEditor('gvColProduct').GetValue() != null) ? grid.GetEditor('gvColProduct').GetValue() : "";
        var QuantityValue = (grid.GetEditor('gvColQuantity').GetValue() != null) ? grid.GetEditor('gvColQuantity').GetValue() : "0";
        if (QuantityValue == "0.0") {
            jAlert("Quantity should not be zero !.");
        } else {

            $("#spnCmbWarehouse").hide();
            $("#spnCmbBatch").hide();
            $("#spncheckComboBox").hide();
            $("#spntxtQuantity").hide();

            if (ProductID != "") {
                var SpliteDetails = ProductID.split("||@||");
                var strProductID = SpliteDetails[0];
                var strDescription = SpliteDetails[1];
                var strUOM = SpliteDetails[2];
                var strStkUOM = SpliteDetails[4];
                var strMultiplier = SpliteDetails[7];
                var strProductName = (grid.GetEditor('gvColProduct').GetText() != null) ? grid.GetEditor('gvColProduct').GetText() : "";
                var StkQuantityValue = QuantityValue * strMultiplier;

                $('#hdfProductIDPC').val(strProductID);
                $('#hdfProductType').val("");
                $('#hdfProductSerialID').val(SrlNo);
                $('#hdfProductSerialID').val(SrlNo);
                $('#hdnProductQuantity').val(QuantityValue);
                var Ptype = "";

                $('#hdnisserial').val("");
                $('#hdnisbatch').val("");
                $('#hdniswarehouse').val("");
                document.getElementById('lblAvailableStkunit').innerHTML = strUOM;
                document.getElementById('lblopeningstockUnit').innerHTML = strUOM;
                $.ajax({
                    type: "POST",
                    url: 'PurchaseOrder.aspx/getProductType',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: "{Products_ID:\"" + strProductID + "\"}",
                    success: function (type) {
                        Ptype = type.d;
                        $('#hdfProductType').val(Ptype);
                        ctxtqnty.SetText("0.0");
                        ctxtqnty.SetEnabled(true);
                        if (Ptype == "") {
                            jAlert("No Warehouse or Batch or Serial is actived !.");
                        } else {
                            if (Ptype == "W") {
                                $('#hdnisserial').val("false");
                                $('#hdnisbatch').val("false");
                                $('#hdniswarehouse').val("true");                               
                            }
                            else if (Ptype == "B") {
                                $('#hdnisserial').val("false");
                                $('#hdnisbatch').val("false");
                                $('#hdniswarehouse').val("false");
                            }
                            else if (Ptype == "S") {
                                $('#hdnisserial').val("false");
                                $('#hdnisbatch').val("false");
                                $('#hdniswarehouse').val("false");
                                ctxtqnty.SetText(QuantityValue);
                                ctxtqnty.SetEnabled(false);
                            }
                            else if (Ptype == "WB") {
                                $('#hdnisserial').val("false");
                                $('#hdnisbatch').val("true");
                                $('#hdniswarehouse').val("true");                               
                            }
                            else if (Ptype == "WS") {
                                $('#hdnisserial').val("true");
                                $('#hdnisbatch').val("false");
                                $('#hdniswarehouse').val("true");
                                ctxtqnty.SetText(QuantityValue);
                                ctxtqnty.SetEnabled(false);                                
                            }
                            else if (Ptype == "WBS") {
                                $('#hdnisserial').val("true");
                                $('#hdnisbatch').val("true");
                                $('#hdniswarehouse').val("true");
                                ctxtqnty.SetText(QuantityValue);
                                ctxtqnty.SetEnabled(false);
                             
                            }
                            else if (Ptype == "BS") {
                                $('#hdnisserial').val("true");
                                $('#hdnisbatch').val("true");
                                $('#hdniswarehouse').val("false");
                                ctxtqnty.SetText(QuantityValue);
                                ctxtqnty.SetEnabled(false);
                            }
                            else {
                                $('#hdnisserial').val("false");
                                $('#hdnisbatch').val("false");
                                $('#hdniswarehouse').val("false");
                            }

                            $("#RequiredFieldValidatortxtbatch").css("display", "none");
                            $("#RequiredFieldValidatortxtserial").css("display", "none");
                            $("#RequiredFieldValidatorCmbWarehouse").css("display", "none");
                            $("#RequiredFieldValidatortxtbatchqntity").css("display", "none");
                            $("#RequiredFieldValidatortxtwareqntity").css("display", "none");
                            $(".blockone").css("display", "none");
                            $(".blocktwo").css("display", "none");
                            $(".blockthree").css("display", "none");

                            //ctxtqnty.SetText("0.0");
                            //ctxtqnty.SetEnabled(true);

                            ctxtbatchqnty.SetText("0.0");
                            ctxtserial.SetText("");
                            ctxtbatchqnty.SetText("");

                            ctxtbatch.SetEnabled(true);
                            cCmbWarehouse.SetEnabled(true);

                            $('#hdnoutstock').val("0");
                            $('#hdnisedited').val("false");
                            $('#hdnisoldupdate').val("false");
                            $('#hdnisnewupdate').val("false");
                            $('#hdnisolddeleted').val("false");
                            $('#hdntotalqntyPC').val(0);
                            $('#hdnoldrowcount').val(0);
                            $('#hdndeleteqnity').val(0);
                            $('#hidencountforserial').val("1");
                            $('#hdfstockidPC').val(0);
                            $('#hdfopeningstockPC').val(0);
                            $('#oldopeningqntity').val(0);
                            $('#hdnnewenterqntity').val(0);
                            $('#hdnenterdopenqnty').val(0);
                            $('#hdbranchIDPC').val(0);
                            $('#hdnisviewqntityhas').val("false");
                            $('#hdndefaultID').val("");
                            $('#hdnbatchchanged').val("0");
                            $('#hdnrate').val("0");
                            $('#hdnvalue').val("0");
                            $('#hdnstrUOM').val(strUOM);

                            var branchid = $("#ddl_Branch option:selected").val();
                            $('#hdnisreduing').val("false");
                            var productid = strProductID ? strProductID : "";
                            var StkQuantityValue = QuantityValue ? QuantityValue : "0.0000";
                            var stockids = SpliteDetails[10];
                            var checkstockdecimalornot = StkQuantityValue.toString().split(".")[1]
                            $('#hdnpcslno').val(SrlNo);                           
                            var ProductName = SpliteDetails[1];
                            var ratevalue = "0";
                            var rate = "0";

                            var branchid = $('#ddl_Branch').val();
                            var BranchNames = $("#ddl_Branch option:selected").text();
                            var strProductID = productid;
                            var strDescription = "";
                            var strUOM = (strUOM != null) ? strUOM : "0";
                            var strProductName = ProductName;

                            document.getElementById('lblbranchName').innerHTML = BranchNames;
                            var availablestock = SpliteDetails[12];
                            $('#hdndefaultID').val("0");

                            $('#hdfstockidPC').val(stockids);
                            var calculateopein = Number(StkQuantityValue) - Number(availablestock);
                            var oldopeing = 0;
                            var oldqnt = Number(oldopeing);

                            $('#hdfopeningstockPC').val(QuantityValue);
                            $('#oldopeningqntity').val(0);
                            $('#hdnnewenterqntity').val(QuantityValue);
                            $('#hdnenterdopenqnty').val(0);
                            $('#hdbranchIDPC').val(branchid);
                            $('#hdnselectedbranch').val(branchid);

                            $('#hdnrate').val(rate);
                            $('#hdnvalue').val(ratevalue);

                            var dtd = (Number(StkQuantityValue)).toFixed(4);


                            $("#lblopeningstock").text(dtd);
                            ctxtmkgdate.SetDate = null;
                            txtexpirdate.SetDate = null;
                            ctxtserial.SetValue("");
                            ctxtbatch.SetValue("");
                            //ctxtqnty.SetValue("0.0");
                            ctxtbatchqnty.SetValue("0.0");

                            var hv = $('#hdnselectedbranch').val();
                            var iswarehousactive = $('#hdniswarehouse').val();
                            var isactivebatch = $('#hdnisbatch').val();
                            var isactiveserial = $('#hdnisserial').val();

                            cCmbWarehouse.PerformCallback('BindWarehouse');

                            if (iswarehousactive == "true") {
                                cCmbWarehouse.SetVisible(true);
                                cCmbWarehouse.SetSelectedIndex(1);
                                cCmbWarehouse.Focus();
                                ctxtqnty.SetVisible(true);
                                $('#hdniswarehouse').val("true");
                                $(".blockone").css("display", "block");

                            } else {
                                cCmbWarehouse.SetVisible(false);
                                ctxtqnty.SetVisible(false);
                                $('#hdniswarehouse').val("false");
                                cCmbWarehouse.SetSelectedIndex(-1);
                                $(".blockone").css("display", "none");
                            }
                            if (isactivebatch == "true") {
                                ctxtbatch.SetVisible(true);
                                ctxtmkgdate.SetVisible(true);
                                ctxtexpirdate.SetVisible(true);
                                $('#hdnisbatch').val("true");
                                $(".blocktwo").css("display", "block");

                            } else {
                                ctxtbatch.SetVisible(false);
                                ctxtmkgdate.SetVisible(false);
                                ctxtexpirdate.SetVisible(false);
                                $('#hdnisbatch').val("false");
                                $(".blocktwo").css("display", "none");
                            }
                            if (isactiveserial == "true") {
                                ctxtserial.SetVisible(true);
                                $('#hdnisserial').val("true");
                                $(".blockthree").css("display", "block");
                            } else {
                                ctxtserial.SetVisible(false);
                                $('#hdnisserial').val("false");
                                $(".blockthree").css("display", "none");
                            }
                            if (iswarehousactive == "false" && isactivebatch == "true") {
                                ctxtbatchqnty.SetVisible(true);
                                $(".blocktwoqntity").css("display", "block");
                            } else {
                                ctxtbatchqnty.SetVisible(false);
                                $(".blocktwoqntity").css("display", "none");
                            }
                            if (iswarehousactive == "false" && isactivebatch == "true") {
                                ctxtbatch.Focus();
                            } else {
                                cCmbWarehouse.Focus();
                            }
                            cbtnWarehouse.SetVisible(true);
                            cGrdWarehousePC.PerformCallback('checkdataexist~' + strProductID + '~' + stockids + '~' + branchid + '~' + strProductName);
                            cPopup_WarehousePC.Show();
                        }
                    }
                });
            }
        }
    }
}


function SetArrForUOM(){
    if (aarr.length == 0) {
        for(var i = -500; i < 500;i++)
        {
            if(grid.GetRow(i) != null){
       
                var ProductID = (grid.batchEditApi.GetCellValue(i,'gvColProduct') != null) ? grid.batchEditApi.GetCellValue(i,'gvColProduct') : "0";
                if(ProductID!="0"){
                    var Indent_Num= (grid.GetEditor('Indent_Num').GetText() != null) ? grid.GetEditor('Indent_Num').GetText() : "";
                    var actionQry = '';
                    //if($("#hdAddOrEdit").val() == "Edit"){

                    if (Indent_Num != "0" && Indent_Num != "") {
                        actionQry = 'PurchaseOrderIndent';
                    }
                    else{
                        actionQry = 'PurchaseOrderByProductID';
                    }


                    var SpliteDetails = ProductID.split("||@||");
                    var strProductID = SpliteDetails[0];
                    
                    var orderid = grid.GetRowKey(i);
                    if(orderid!="" && orderid!=null)
                    {
                        orderid = (orderid.split("Q~"));
                    }
                    else
                    {
                        orderid=0;
                    }
                    var slnoget = grid.batchEditApi.GetCellValue(i,'SrlNo');
                    var Quantity = grid.batchEditApi.GetCellValue(i,'gvColQuantity');
                    if($("#hddnMultiUOMSelection").val()=="0")
                    {
                        $.ajax({
                            type: "POST",
                            url: "Services/Master.asmx/GetMultiUOMDetails",
                            data: JSON.stringify({orderid: orderid,action:actionQry,module:'PurchaseOrder',strKey :Indent_Num}),
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            async: false,
                            success: function (msg) {
                       
                                gridPackingQty = msg.d;

                                if(msg.d != ""){
                                    var packing = SpliteDetails[19];
                                    var PackingUom = SpliteDetails[23];
                                    var PackingSelectUom = SpliteDetails[24];
                                    var arrobj = {};
                                    arrobj.productid = strProductID;
                                    arrobj.slno = slnoget;
                                    arrobj.Quantity = Quantity;
                                    arrobj.packing = gridPackingQty;
                                    arrobj.PackingUom = PackingUom;
                                    arrobj.PackingSelectUom = PackingSelectUom;

                                    aarr.push(arrobj);
                                    //alert();
                                }
                            }
                        });
                    }
                }
            }
        }

    }
}

function Save_ButtonClick() {
    LoadingPanel.Show();
    flag = true;
    //Mantis Issue 25152
    if ($("#hdnSettings").val() == "Yes" && $('#chkDirectorApprovalRequired').is(':checked') == true) {
        if (cdddlApprovalEmployee.GetValue() == "0") {
            jAlert("Please Select Employee.");
            cLoadingPanelCRP.Hide();
            return false;
        }
        else {
            var val = cdddlApprovalEmployee.GetValue();
            $("#hdnEmployee").val(val);
        }
    }
    //End of Mantis Issue 25152
    var ProjectCode = clookup_Project.GetText();
    if ($("#hdnProjectSelectInEntryModule").val() == "1" && $("#hdnProjectMandatory").val()=="1" && ProjectCode == "") {
        LoadingPanel.Hide();
        flag = false;
        jAlert("Please Select Project.");
        return false;
    }

    var revdate=ctxtRevisionDate.GetText();	
    if($("#hdnApproveStatus").val()==1 && $("#hdnEditPageStatus").val() == "Quoteupdate" && $("#hdnApprovalsetting").val()=="1")	
    {	
        if(ctxtRevisionNo.GetText()=="")	
        {	
            flag = false;	
            LoadingPanel.Hide();	
            jAlert("Please Enter Revision Details.");	
            ctxtRevisionNo.SetFocus();	
            return false;	
        }	
    }	
    if($("#hdnApproveStatus").val()==1 && $("#hdnEditPageStatus").val() == "Quoteupdate" && $("#hdnApprovalsetting").val()=="1")	
    {	
        if(revdate=="01-01-0100"||revdate==null||revdate=="")	
        {	
            flag = false;	
            LoadingPanel.Hide();	
            jAlert("Please Enter Revision Details.");	
            ctxtRevisionDate.SetFocus();	
            return false;	
        }	
    }	
    if($("#hdnApproveStatus").val()==1 && $("#hdnEditPageStatus").val() == "Quoteupdate" && $("#hdnApprovalsetting").val()=="1")	
    {	
        var detRev={};	
        detRev.RevNo=ctxtRevisionNo.GetText();	
        detRev.Order=$("#hdnEditOrderId").val();	
           	
        $.ajax({	
            type: "POST",	
            url: "PurchaseOrder.aspx/Duplicaterevnumbercheck",	
            data: JSON.stringify(detRev),	
            contentType: "application/json; charset=utf-8",	
            dataType: "json",	
            async:false,	
            success: function (msg) {	
                var duplicateRevCheck=msg.d;	
                if (duplicateRevCheck==1)	
                {	
                    flag = false;	
                    LoadingPanel.Hide();	
                    jAlert("Please Enter a valid Revision No");	
                    //alert("Please Enter a valid Revision No");	
                    //  LoadingPanel.Hide();	
                    //$("#txtRevisionNo").val("");	
                    ctxtRevisionNo.SetFocus();	
                }	
            }	
        });	
    }


    var txtPurchaseNo = $("#txtVoucherNo").val();
    var ddl_Vendor = $("#ddl_Vendor").val();
    var Podt = cPLQuoteDate.GetValue();

    if (txtPurchaseNo == null || txtPurchaseNo == "") {
        //flag = false;
        LoadingPanel.Hide();
        $("#MandatoryBillNo").show();
        return false;
    }
    if (Podt == null) {
        LoadingPanel.Hide();
        $("#MandatoryDate").show();
        return false;
    }
    var customerId = GetObjectID('hdnCustomerId').value
    if (customerId == '' || customerId == null) {
        LoadingPanel.Hide();
        $('#MandatorysVendor').attr('style', 'display:block');
        return false;
    }
    else {
        $('#MandatorysVendor').attr('style', 'display:none');
    }
    var TagMendatory = document.getElementById('hdfTagMendatory').value;// $('#hdfTagMendatory').val();
    if ($('#ddlInventory').val() == 'Y') {
        if (TagMendatory == 'Yes') {
            LoadingPanel.Hide();
            $("#MandatorysIndentReq").show();

            return false;
        }
    }
    var PoDuedt = cdt_PODue.GetValue();
    if (PoDuedt == null) {
        LoadingPanel.Hide();
        $("#MandatoryDueDate").show();
        return false;
    }
    var IsType = "";
    var frontRow = 0;
    var backRow = -1;

    for (var i = 0; i <= grid.GetVisibleRowsOnPage() ; i++) {
        var frontProduct = (grid.batchEditApi.GetCellValue(backRow, 'ProductName') != null) ? (grid.batchEditApi.GetCellValue(backRow, 'ProductName')) : "";
        var backProduct = (grid.batchEditApi.GetCellValue(frontRow, 'ProductName') != null) ? (grid.batchEditApi.GetCellValue(frontRow, 'ProductName')) : "";


        if (frontProduct != "" || backProduct != "") {
            IsType = "Y";
            break;
        }
        backRow--;
        frontRow++;
    }

    if (flag != false) {
        SetArrForUOM(); //Surojit For UOM EDIT

        if (grid.GetVisibleRowsOnPage() > 0) {
            if (IsType == "Y") {
                var IsInventory=$("#ddlInventory").val();
                console.log("1"+IsInventory);
                if(IsInventory!='N')
                {            
                    if (issavePacking == 1) {
                        if (aarr.length > 0) {
                            $.ajax({
                                type: "POST",
                                url: "PurchaseOrder.aspx/SetSessionPacking",
                                data: "{'list':'" + JSON.stringify(aarr) + "'}",
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",
                                success: function (msg) {
                                    //var customerval = (gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) != null) ? gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) : "";
                                    var VendorId = $('#hdnCustomerId').val();
                                    $('#hdfLookupCustomer').val(VendorId);
                                    $('#hdfIsDelete').val('I');
                                    $('#hdnRefreshType').val('N');
                                    grid.batchEditApi.EndEdit();
                                    $('#hfControlData').val($('#hfControlSaveData').val());
                                    OnAddNewClick();

                                    var JsonProductList = JSON.stringify(TaxOfProduct);
                                    GetObjectID('hdnJsonProductTax').value = JsonProductList;
                                    grid.UpdateEdit();
                                }
                            });
                        }
                        else {

                            //var customerval = (gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) != null) ? gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) : "";
                            var VendorId = $('#hdnCustomerId').val();
                            $('#hdfLookupCustomer').val(VendorId);
                            $('#hdfIsDelete').val('I');
                            $('#hdnRefreshType').val('N');
                            grid.batchEditApi.EndEdit();
                            $('#hfControlData').val($('#hfControlSaveData').val());
                            OnAddNewClick();

                            var JsonProductList = JSON.stringify(TaxOfProduct);
                            GetObjectID('hdnJsonProductTax').value = JsonProductList;
                            grid.UpdateEdit();
                        }
                    }
                    else {

                        if (aarr.length > 0) {
                            $.ajax({
                                type: "POST",
                                url: "PurchaseOrder.aspx/SetSessionPacking",
                                data: "{'list':'" + JSON.stringify(aarr) + "'}",
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",
                                success: function (msg) {
                                    //var customerval = (gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) != null) ? gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) : "";
                                    var VendorId = $('#hdnCustomerId').val();
                                    $('#hdfLookupCustomer').val(VendorId);
                                    $('#hdfIsDelete').val('I');
                                    $('#hdnRefreshType').val('N');
                                    grid.batchEditApi.EndEdit();
                                    $('#hfControlData').val($('#hfControlSaveData').val());
                                    OnAddNewClick();

                                    var JsonProductList = JSON.stringify(TaxOfProduct);
                                    GetObjectID('hdnJsonProductTax').value = JsonProductList;
                                    grid.UpdateEdit();
                                }
                            });
                        }
                        else{

                            //var customerval = (gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) != null) ? gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) : "";
                            var VendorId = $('#hdnCustomerId').val();
                            $('#hdfLookupCustomer').val(VendorId);
                            $('#hdfIsDelete').val('I');
                            $('#hdnRefreshType').val('N');
                            grid.batchEditApi.EndEdit();
                            $('#hfControlData').val($('#hfControlSaveData').val());
                            OnAddNewClick();

                            var JsonProductList = JSON.stringify(TaxOfProduct);
                            GetObjectID('hdnJsonProductTax').value = JsonProductList;
                            grid.UpdateEdit();
                        }
                    }
                }
                else
                {
                    var VendorId = $('#hdnCustomerId').val();
                    $('#hdfLookupCustomer').val(VendorId);
                    $('#hdnRefreshType').val('E');
                    $('#hdfIsDelete').val('I');
                    $('#hfControlData').val($('#hfControlSaveData').val());
                    grid.batchEditApi.EndEdit();
                    OnAddNewClick();
                    var JsonProductList = JSON.stringify(TaxOfProduct);
                    GetObjectID('hdnJsonProductTax').value = JsonProductList;
                    grid.UpdateEdit();
                }
            
            }
            else {
                LoadingPanel.Hide();
                jAlert('Cannot Save. You must enter atleast one Product to save this entry.');
            }
        }
        else {
            LoadingPanel.Hide();
            jAlert('Cannot Save. You must enter atleast one Product to save this entry.');
        }
    }            
}
function SaveExit_ButtonClick() {
    LoadingPanel.Show();
    flag = true;
    //Mantis Issue 25152
    if ($("#hdnSettings").val() == "Yes" && $('#chkDirectorApprovalRequired').is(':checked') == true) {
        if (cdddlApprovalEmployee.GetValue() == "0") {
            jAlert("Please Select Employee.");
            cLoadingPanelCRP.Hide();
            return false;
        }
        else {
            var val = cdddlApprovalEmployee.GetValue();
            $("#hdnEmployee").val(val);
        }
    }
    //End of Mantis Issue 25152
    var ProjectCode = clookup_Project.GetText();
    if ($("#hdnProjectSelectInEntryModule").val() == "1" && $("#hdnProjectMandatory").val()=="1" && ProjectCode == "") {
        flag = false;
        LoadingPanel.Hide();
        jAlert("Please Select Project.");
        return false;
    }

    var revdate=ctxtRevisionDate.GetText();	
    if($("#hdnApproveStatus").val()==1 && $("#hdnEditPageStatus").val() == "Quoteupdate" && $("#hdnApprovalsetting").val()=="1")	
    {	
        if(ctxtRevisionNo.GetText()=="")	
        {	
            flag = false;	
            LoadingPanel.Hide();	
            jAlert("Please Enter Revision Details.");	
            ctxtRevisionNo.SetFocus();	
            return false;	
        }	
    }	
    if($("#hdnApproveStatus").val()==1 && $("#hdnEditPageStatus").val() == "Quoteupdate" && $("#hdnApprovalsetting").val()=="1")	
    {	
        if(revdate=="01-01-0100"||revdate==null||revdate=="")	
        {	
            flag = false;	
            LoadingPanel.Hide();	
            jAlert("Please Enter Revision Details.");	
            ctxtRevisionDate.SetFocus();	
            return false;	
        }	
    }	
    if($("#hdnApproveStatus").val()==1 && $("#hdnEditPageStatus").val() == "Quoteupdate" && $("#hdnApprovalsetting").val()=="1")	
    {	
        var detRev={};	
        detRev.RevNo=ctxtRevisionNo.GetText();	
        detRev.Order=$("#hdnEditOrderId").val();	
           	
        $.ajax({	
            type: "POST",	
            url: "PurchaseOrder.aspx/Duplicaterevnumbercheck",	
            data: JSON.stringify(detRev),	
            contentType: "application/json; charset=utf-8",	
            dataType: "json",	
            async:false,	
            success: function (msg) {	
                var duplicateRevCheck=msg.d;	
                if (duplicateRevCheck==1)	
                {	
                    flag = false;	
                    LoadingPanel.Hide();	
                    jAlert("Please Enter a valid Revision No");	                             	
                    ctxtRevisionNo.SetFocus();	
                }	
            }	
        });	
    }

    var txtPurchaseNo = $("#txtVoucherNo").val();
    var ddl_Vendor = $("#ddl_Vendor").val();          
    if (txtPurchaseNo == null || txtPurchaseNo == "") {
        flag = false;
        LoadingPanel.Hide();
        $("#MandatoryBillNo").show();
        return false;
    }
    var Podt = cPLQuoteDate.GetValue();
    if (Podt == null) {
        LoadingPanel.Hide();
        $("#MandatoryDate").show();
        return false;
    }
    var customerId = GetObjectID('hdnCustomerId').value
    if (customerId == '' || customerId == null) {
        LoadingPanel.Hide();
        $('#MandatorysVendor').attr('style', 'display:block');
        return false;
    }
    else {
        $('#MandatorysVendor').attr('style', 'display:none');
    }
    var TagMendatory = document.getElementById('hdfTagMendatory').value;
    if ($('#ddlInventory').val() == 'Y') {
        if (TagMendatory == 'Yes') {
            LoadingPanel.Hide();
            $("#MandatorysIndentReq").show();
            return false;
        }
    }
    var PoDuedt = cdt_PODue.GetValue();
    if (PoDuedt == null) {
        LoadingPanel.Hide();
        $("#MandatoryDueDate").show();
        return false;
    }
    var IsType = "";
    var frontRow = 0;
    var backRow = -1;

    for (var i = 0; i <= grid.GetVisibleRowsOnPage() ; i++) {
        var frontProduct = (grid.batchEditApi.GetCellValue(backRow, 'ProductName') != null) ? (grid.batchEditApi.GetCellValue(backRow, 'ProductName')) : "";
        var backProduct = (grid.batchEditApi.GetCellValue(frontRow, 'ProductName') != null) ? (grid.batchEditApi.GetCellValue(frontRow, 'ProductName')) : "";
        if (frontProduct != "" || backProduct != "") {
            IsType = "Y";
            break;
        }
        backRow--;
        frontRow++;
    }
    if (flag != false) {
        SetArrForUOM(); //Surojit For UOM EDIT

        if (grid.GetVisibleRowsOnPage() > 0) {
            if (IsType == "Y") {
                var IsInventory=$("#ddlInventory").val();
                console.log("2"+IsInventory);
                if(IsInventory!='N')
                {   
                    if (issavePacking == 1) {
                        if (aarr.length > 0) {
                            $.ajax({
                                type: "POST",
                                url: "PurchaseOrder.aspx/SetSessionPacking",
                                data: "{'list':'" + JSON.stringify(aarr) + "'}",
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",
                                success: function (msg) {
                                    // var customerval = (gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) != null) ? gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) : "";
                                    var VendorId = $('#hdnCustomerId').val();
                                    $('#hdfLookupCustomer').val(VendorId);
                                    $('#hdnRefreshType').val('E');
                                    $('#hdfIsDelete').val('I');
                                    $('#hfControlData').val($('#hfControlSaveData').val());
                                    grid.batchEditApi.EndEdit();
                                    OnAddNewClick();
                                    var JsonProductList = JSON.stringify(TaxOfProduct);
                                    GetObjectID('hdnJsonProductTax').value = JsonProductList;
                                    grid.UpdateEdit();
                                }
                            });
                        }
                        else {

                            // var customerval = (gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) != null) ? gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) : "";
                            var VendorId = $('#hdnCustomerId').val();
                            $('#hdfLookupCustomer').val(VendorId);
                            $('#hdnRefreshType').val('E');
                            $('#hdfIsDelete').val('I');
                            $('#hfControlData').val($('#hfControlSaveData').val());
                            grid.batchEditApi.EndEdit();
                            OnAddNewClick();
                            var JsonProductList = JSON.stringify(TaxOfProduct);
                            GetObjectID('hdnJsonProductTax').value = JsonProductList;
                            grid.UpdateEdit();
                        }

                    }
                    else {


                        if (aarr.length > 0) {
                            $.ajax({
                                type: "POST",
                                url: "PurchaseOrder.aspx/SetSessionPacking",
                                data: "{'list':'" + JSON.stringify(aarr) + "'}",
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",
                                success: function (msg) {
                                    // var customerval = (gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) != null) ? gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) : "";
                                    var VendorId = $('#hdnCustomerId').val();
                                    $('#hdfLookupCustomer').val(VendorId);
                                    $('#hdnRefreshType').val('E');
                                    $('#hdfIsDelete').val('I');
                                    $('#hfControlData').val($('#hfControlSaveData').val());
                                    grid.batchEditApi.EndEdit();
                                    OnAddNewClick();
                                    var JsonProductList = JSON.stringify(TaxOfProduct);
                                    GetObjectID('hdnJsonProductTax').value = JsonProductList;
                                    grid.UpdateEdit();
                                }
                            });
                        }
                        else{

                            // var customerval = (gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) != null) ? gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) : "";
                            var VendorId = $('#hdnCustomerId').val();
                            $('#hdfLookupCustomer').val(VendorId);
                            $('#hdnRefreshType').val('E');
                            $('#hdfIsDelete').val('I');
                            $('#hfControlData').val($('#hfControlSaveData').val());
                            grid.batchEditApi.EndEdit();
                            OnAddNewClick();
                            var JsonProductList = JSON.stringify(TaxOfProduct);
                            GetObjectID('hdnJsonProductTax').value = JsonProductList;
                            grid.UpdateEdit();
                        }
                    }
                }
                else
                {
                    var VendorId = $('#hdnCustomerId').val();
                    $('#hdfLookupCustomer').val(VendorId);
                    $('#hdnRefreshType').val('E');
                    $('#hdfIsDelete').val('I');
                    $('#hfControlData').val($('#hfControlSaveData').val());
                    grid.batchEditApi.EndEdit();
                    OnAddNewClick();
                    var JsonProductList = JSON.stringify(TaxOfProduct);
                    GetObjectID('hdnJsonProductTax').value = JsonProductList;
                    grid.UpdateEdit();
                }
            }
            else {
                LoadingPanel.Hide();
                jAlert('Cannot Save. You must enter atleast one Product to save this entry.');
            }

        }
        else {
            LoadingPanel.Hide();
            jAlert('Cannot Save. You must enter atleast one Product to save this entry.');
        }
    }
}

         
function Reject_ButtonClick()	
{	
    if($("#hdnProjectApproval").val()=="ProjApprove")	
    {	
        if($("#txtAppRejRemarks").val()=="")	
        {	
            jAlert("Please Enter Reject Remarks.")	
            $("#txtAppRejRemarks").focus();	
            return false;	
        }	
    }	
    var otherdet={};	
    otherdet.ApproveRemarks=$("#txtAppRejRemarks").val();	
    otherdet.ApproveRejStatus=2;	
    otherdet.OrderId= $("#hdnEditOrderId").val();	
    $.ajax({	
        type: "POST",	
        url: "PurchaseOrder.aspx/SetApproveReject",	
        data: JSON.stringify(otherdet),	
        contentType: "application/json; charset=utf-8",	
        dataType: "json",	
        success: function (msg) {	
            var value=msg.d;	
            if (value=="1")	
            {	
                jAlert("Order Rejected.");	
                window.location.href="PurchaseOrderList.aspx";	
            }	
        }	
    });	
}	
function Approve_ButtonClick()	
{	
    LoadingPanel.Show();	
    flag = true;	
    if($("#hdnProjectApproval").val()=="ProjApprove")	
    {	
        if($("#txtAppRejRemarks").val()=="")	
        {	
            flag = false;	
            LoadingPanel.Hide();	
            jAlert("Please Enter Approval Remarks.")	
            $("#txtAppRejRemarks").focus();	
            return false;	
        }	
    }	
    $("#hdnApproveStatus").val(1);	
    var txtPurchaseNo = $("#txtVoucherNo").val();	
    var ddl_Vendor = $("#ddl_Vendor").val();          	
    if (txtPurchaseNo == null || txtPurchaseNo == "") {	
        flag = false;	
        LoadingPanel.Hide();	
        $("#MandatoryBillNo").show();	
        return false;	
    }	
    var Podt = cPLQuoteDate.GetValue();	
    if (Podt == null) {	
        LoadingPanel.Hide();	
        $("#MandatoryDate").show();	
        return false;	
    }	
    var customerId = GetObjectID('hdnCustomerId').value	
    if (customerId == '' || customerId == null) {	
        LoadingPanel.Hide();	
        $('#MandatorysVendor').attr('style', 'display:block');	
        return false;	
    }	
    else {	
        $('#MandatorysVendor').attr('style', 'display:none');	
    }	
    var TagMendatory = document.getElementById('hdfTagMendatory').value;// $('#hdfTagMendatory').val();	
    if ($('#ddlInventory').val() == 'Y') {	
        if (TagMendatory == 'Yes') {	
            LoadingPanel.Hide();	
            $("#MandatorysIndentReq").show();	
            return false;	
        }	
    }	
    var PoDuedt = cdt_PODue.GetValue();	
    if (PoDuedt == null) {	
        LoadingPanel.Hide();	
        $("#MandatoryDueDate").show();	
        return false;	
    }	
    var IsType = "";	
    var frontRow = 0;	
    var backRow = -1;	
    for (var i = 0; i <= grid.GetVisibleRowsOnPage() ; i++) {	
        var frontProduct = (grid.batchEditApi.GetCellValue(backRow, 'ProductName') != null) ? (grid.batchEditApi.GetCellValue(backRow, 'ProductName')) : "";	
        var backProduct = (grid.batchEditApi.GetCellValue(frontRow, 'ProductName') != null) ? (grid.batchEditApi.GetCellValue(frontRow, 'ProductName')) : "";	
        if (frontProduct != "" || backProduct != "") {	
            IsType = "Y";	
            break;	
        }	
        backRow--;	
        frontRow++;	
    }	
    if (flag != false) {	
        SetArrForUOM(); //Surojit For UOM EDIT	
        if (grid.GetVisibleRowsOnPage() > 0) {	
            if (IsType == "Y") {	
                if (issavePacking == 1) {	
                    if (aarr.length > 0) {	
                        $.ajax({	
                            type: "POST",	
                            url: "PurchaseOrder.aspx/SetSessionPacking",	
                            data: "{'list':'" + JSON.stringify(aarr) + "'}",	
                            contentType: "application/json; charset=utf-8",	
                            dataType: "json",	
                            success: function (msg) {	
                                // var customerval = (gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) != null) ? gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) : "";	
                                var VendorId = $('#hdnCustomerId').val();	
                                $('#hdfLookupCustomer').val(VendorId);	
                                $('#hdnRefreshType').val('E');	
                                $('#hdfIsDelete').val('I');	
                                $('#hfControlData').val($('#hfControlSaveData').val());	
                                grid.batchEditApi.EndEdit();	
                                OnAddNewClick();	
                                var JsonProductList = JSON.stringify(TaxOfProduct);	
                                GetObjectID('hdnJsonProductTax').value = JsonProductList;	
                                grid.UpdateEdit();	
                            }	
                        });	
                    }	
                    else {	
                        // var customerval = (gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) != null) ? gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) : "";	
                        var VendorId = $('#hdnCustomerId').val();	
                        $('#hdfLookupCustomer').val(VendorId);	
                        $('#hdnRefreshType').val('E');	
                        $('#hdfIsDelete').val('I');	
                        $('#hfControlData').val($('#hfControlSaveData').val());	
                        grid.batchEditApi.EndEdit();	
                        OnAddNewClick();	
                        var JsonProductList = JSON.stringify(TaxOfProduct);	
                        GetObjectID('hdnJsonProductTax').value = JsonProductList;	
                        grid.UpdateEdit();	
                    }	
                }	
                else {	
                    if (aarr.length > 0) {	
                        $.ajax({	
                            type: "POST",	
                            url: "PurchaseOrder.aspx/SetSessionPacking",	
                            data: "{'list':'" + JSON.stringify(aarr) + "'}",	
                            contentType: "application/json; charset=utf-8",	
                            dataType: "json",	
                            success: function (msg) {	
                                // var customerval = (gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) != null) ? gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) : "";	
                                var VendorId = $('#hdnCustomerId').val();	
                                $('#hdfLookupCustomer').val(VendorId);	
                                $('#hdnRefreshType').val('E');	
                                $('#hdfIsDelete').val('I');	
                                $('#hfControlData').val($('#hfControlSaveData').val());	
                                grid.batchEditApi.EndEdit();	
                                OnAddNewClick();	
                                var JsonProductList = JSON.stringify(TaxOfProduct);	
                                GetObjectID('hdnJsonProductTax').value = JsonProductList;	
                                grid.UpdateEdit();	
                            }	
                        });	
                    }	
                    else{	
                        // var customerval = (gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) != null) ? gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) : "";	
                        var VendorId = $('#hdnCustomerId').val();	
                        $('#hdfLookupCustomer').val(VendorId);	
                        $('#hdnRefreshType').val('E');	
                        $('#hdfIsDelete').val('I');	
                        $('#hfControlData').val($('#hfControlSaveData').val());	
                        grid.batchEditApi.EndEdit();	
                        OnAddNewClick();	
                        var JsonProductList = JSON.stringify(TaxOfProduct);	
                        GetObjectID('hdnJsonProductTax').value = JsonProductList;	
                        grid.UpdateEdit();	
                    }	
                }	
            }	
            else {	
                LoadingPanel.Hide();	
                jAlert('Cannot Save. You must enter atleast one Product to save this entry.');	
            }	
        }	
        else {	
            LoadingPanel.Hide();	
            jAlert('Cannot Save. You must enter atleast one Product to save this entry.');	
        }	
    }	
}	
       
function OnAddNewClick() {            
    grid.AddNewRow();
    var noofvisiblerows = grid.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
    var i;
    var cnt = 1;
    for (i = -1 ; cnt <= noofvisiblerows ; i--) {
        var tbQuotation = grid.GetEditor("SrlNo");
        tbQuotation.SetValue(cnt);
        cnt++;
    }
    // Mantis Issue 24429
    $("#UOMQuantity").val(0);
    Uomlength= 0 ;
    // End of Mantis Issue 24429
}
function ProductsCombo_SelectedIndexChanged(s, e) {

    var tbDescription = grid.GetEditor("gvColDiscription");
    var tbUOM = grid.GetEditor("gvColUOM");
    var tbStockUOM = grid.GetEditor("gvColStockUOM");
    var tbPurchasePrice = grid.GetEditor("gvColStockPurchasePrice");
    var ProductID = (grid.GetEditor('gvColProduct').GetValue() != null) ? grid.GetEditor('gvColProduct').GetValue() : "0";            
    var SpliteDetails = ProductID.split("||@||");
    var strProductID = SpliteDetails[0];
    var strDescription = SpliteDetails[1];
    var strUOM = SpliteDetails[2];
    var strStockUOM = SpliteDetails[4];
    var strPurchasePrice = SpliteDetails[6];
    var strStockId = SpliteDetails[10];
    tbDescription.SetValue(strDescription);
    tbUOM.SetValue(strUOM);
    tbStockUOM.SetValue(strStockUOM);
    tbPurchasePrice.SetValue(strPurchasePrice);
    if (ProductID != "0") {
        cacpAvailableStock.PerformCallback(strProductID);
    }
}
          
function ddl_AmountAre_valueChange() {
    var key = $("#ddl_AmountAre").val();
    if (key == 1) {                 
        cddlVatGstCst.SetEnabled(false);
        cddlVatGstCst.PerformCallback('1');
    }
    else if (key == 2) {                
        cddlVatGstCst.SetEnabled(true);
        cddlVatGstCst.PerformCallback('2');
    }
    else if (key == 3) {                
        cddlVatGstCst.SetEnabled(false);
        cddlVatGstCst.PerformCallback('3');
    }
}

function GetIndentREquiNo(e) {

    var PODate = new Date();
    PODate = cPLQuoteDate.GetValueString();
    cQuotationComponentPanel.PerformCallback('BindIndentGrid' + '~' + PODate);

    grid.batchEditApi.StartEdit(-1, 1);
    var accountingDataMin = grid.GetEditor('ProductName').GetValue();
    grid.batchEditApi.EndEdit();

    grid.batchEditApi.StartEdit(0, 1);
    var accountingDataplus = grid.GetEditor('ProductName').GetValue();

    grid.batchEditApi.EndEdit();

    if (accountingDataMin != null || accountingDataplus != null) {
        jConfirm('Documents tagged are to be automatically De-selected. Confirm?', 'Confirmation Dialog', function (r) {

            if (r == true) {
                ctaxUpdatePanel.PerformCallback('DeleteAllTax');
                grid.PerformCallback('GridBlank');
            }
        });
        //onBranchItems();
    }
}

function GetIndentReqNoOnLoad() {

    var PODate = new Date();
    PODate = cPLQuoteDate.GetValueString();
    cQuotationComponentPanel.PerformCallback('BindIndentGrid' + '~' + PODate);

}
function GetContactPersonPhone(e) {
    var key = cContactPerson.GetValue();
    cacpContactPersonPhone.PerformCallback('ContactPersonPhone~' + key);
}     
        
function ShowIndntRequisition() {

}
function cmbContactPersonEndCall(s, e) {             
}
function acpContactPersonPhoneEndCall(s, e) {
    if (cacpContactPersonPhone.cpPhone != null && cacpContactPersonPhone.cpPhone != undefined) {
        pageheaderContent.style.display = "block";
        $("#divContactPhone").attr('style', 'display:block');
        document.getElementById('lblContactPhone').innerHTML = cacpContactPersonPhone.cpPhone;
        cacpContactPersonPhone.cpPhone = null;
    }
}
$(document).ready(function () {
    var mode = $('#hdnADDEditMode').val();
    if (mode == 'Edit') {
        if ($("#hdnCustomerId").val() != "")
        {
            var VendorID = $("#hdnCustomerId").val();
            SetEntityType(VendorID);
        }
    }
    //#### added by Sayan Dutta for TC Purchase Order Specefic Fields #######################
    var key = GetObjectID('hdnCustomerId').value;
    if (key != null && key != "") {
        if ($("#btn_TermsCondition").is(":visible")) {
            callTCspecefiFields_PO(key);
        }
    }
    var checked = $("[id$='rdl_Salesquotation']").find(":checked").val();
    if (checked=="Indent" || checked=="Quotation") {
        ctaggingList.SetEnabled(true);
    }
    else
    {
        ctaggingList.SetEnabled(false);
    }

    if ($('#hdnPageStatus').val() == "Quoteupdate")
    {
        cPurchaseOrderPosGst.SetEnabled(false);
        PopulatePurchasePosGst();

        if($("#hdnApproveStatus").val()==1 && $("#hdnApprovalsetting").val()=="1")	
        {	
            document.getElementById("dvRevisionDate").style.display="block";	
            document.getElementById("dvRevision").style.display="block";	
        }
    }

    if(($("#hdnEditPageStatus").val()=="Quoteupdate")&&($("#hdnApproveStatus").val()==1)&&($("#hdnProjectApproval").val()=="ProjApprove") && $("#hdnApprovalsetting").val()=="1")	
    {	
        document.getElementById("dvRevisionDate").style.display="block";	
        document.getElementById("dvRevision").style.display="block";	
        document.getElementById("dvAppRejRemarks").style.display="block";	
        document.getElementById("dvReject").style.display="none";	
        document.getElementById("dvApprove").style.display="none";	
    }	
    if(($("#hdnEditPageStatus").val()=="Quoteupdate")&&($("#hdnApproveStatus").val()==2)&&($("#hdnProjectApproval").val()=="ProjApprove") && $("#hdnApprovalsetting").val()=="1")	
    {	
        document.getElementById("dvAppRejRemarks").style.display="block";	
        document.getElementById("dvReject").style.display="none";	
        document.getElementById("dvApprove").style.display="inline-block";	
    }	
    if(($("#hdnEditPageStatus").val()=="Quoteupdate")&&($("#hdnApproveStatus").val()==0)&&($("#hdnProjectApproval").val()=="ProjApprove") && $("#hdnApprovalsetting").val()=="1")	
    {	
        document.getElementById("dvAppRejRemarks").style.display="block";	
        document.getElementById("dvReject").style.display="inline-block";	
        document.getElementById("dvApprove").style.display="inline-block";	
    }

    LoadCustomerBillingShippingAddress(GetObjectID('hdnCustomerId').value);
    LoadBranchAddressInEditMode($('#ddl_Branch').val());

    //#### End : added by Sayan Dutta for TC Purchase Order Specefic Fields : End #############
    $('#ProductModel').on('shown.bs.modal', function () {
        $('#txtProdSearch').focus();
    })             
    var schemaid = $('#ddl_numberingScheme').val();
    if (schemaid != null) {
        if (schemaid == '0') {
            document.getElementById('txtVoucherNo').disabled = true;
        }
    }             
    $('#ApprovalCross').click(function () {
        window.parent.popup.Hide();
        window.parent.cgridPendingApproval.Refresh();
    })         


    if($('#Keyval_internalId').val()=="Add" && $('#ddl_numberingScheme').val() != "0"){
        CmbScheme_ValueChange();
    }


});
function CmbScheme_ValueChange() {
    var val = $("#ddl_numberingScheme").val();
    //Mantis Issue 24920
    //ctxtVendorName.SetText("");
    //GetObjectID('hdnCustomerId').value = "";
    if($("#hdnIsCopy").val()=="COPY"){

    }
    else{
        ctxtVendorName.SetText("");
        GetObjectID('hdnCustomerId').value = "";
    }
    //End of Mantis Issue 24920
    page.tabs[1].SetEnabled(false);
    var schemetypeValue = val;
    var schemetype = schemetypeValue.toString().split('~')[1];
    var schemelength = schemetypeValue.toString().split('~')[2];
    var branchID = (schemetypeValue.toString().split('~')[3] != null) ? schemetypeValue.toString().split('~')[3] : "";
    //Rev Debashis
    var fromdate = (schemetypeValue.toString().split('~')[4] != null) ? schemetypeValue.toString().split('~')[4] : "";
    var todate = (schemetypeValue.toString().split('~')[5] != null) ? schemetypeValue.toString().split('~')[5] : "";
    var dt = new Date();
    cPLQuoteDate.SetDate(dt);
    if (dt < new Date(fromdate)) {
        cPLQuoteDate.SetDate(new Date(fromdate));
    }
    if (dt > new Date(todate)) {
        cPLQuoteDate.SetDate(new Date(todate));
    }
    cPLQuoteDate.SetMinDate(new Date(fromdate));
    cPLQuoteDate.SetMaxDate(new Date(todate));
    //End of Rev Debashis
    $("#hdnTCBranchId").val(branchID);
    if (branchID != "") document.getElementById('ddl_Branch').value = branchID;
    document.getElementById('ddl_Branch').disabled = true;
    $('#txtVoucherNo').attr('maxLength', schemelength);

    if (schemetype == '0') {
        document.getElementById('txtVoucherNo').disabled = false;
        document.getElementById('txtVoucherNo').value = "";
        $("#txtVoucherNo").focus();
    }
    else if (schemetype == '1') {
        document.getElementById('txtVoucherNo').disabled = true;
        document.getElementById('txtVoucherNo').value = "Auto";
        cPLQuoteDate.Focus();
        $("#MandatoryBillNo").hide();

        if($("#HdnBackDatedEntryPurchaseGRN").val()=="1")
        {
            cPLQuoteDate.SetEnabled(true);
        }
        else
        {
            if($("#hdnBackdateddate").val() !="0" && $("#hdnBackdateddate").val()!="")
            {
                var Days = $("#hdnBackdateddate").val();
                var today = cPLQuoteDate.GetDate();
                var newdate = cPLQuoteDate.GetDate();
                newdate.setDate(today.getDate() - Math.round(Days));
                cPLQuoteDate.SetMinDate(newdate);
                cPLQuoteDate.SetMaxDate(dt);
                cPLQuoteDate.SetEnabled(true);
            }
            else
            {
                cPLQuoteDate.SetEnabled(false);
            }
        }

      



    }
    else if (schemetype == '2') {
        document.getElementById('txtVoucherNo').disabled = true;
        document.getElementById('txtVoucherNo').value = "Datewise";
    }
    else if (schemetype == 'n') {
        document.getElementById('txtVoucherNo').disabled = true;
        document.getElementById('txtVoucherNo').value = "";
    }
    else {
        document.getElementById('txtVoucherNo').disabled = true;
        document.getElementById('txtVoucherNo').value = "";
    }
    //Chinmoy added this line
    //Mantis Issue 24920
    //PosGstId = "";
    //cPurchaseOrderPosGst.SetValue(PosGstId);
    if($("#hdnIsCopy").val()=="COPY"){

    }
    else{
        PosGstId = "";
        cPurchaseOrderPosGst.SetValue(PosGstId);
    }
    //End of Mantis Issue 24920
    SetPurchaseBillingShippingAddress( $('#ddl_Branch').val());
            

    //if ($("#hdnProjectSelectInEntryModule").val() == "1")
    //    clookup_Project.gridView.Refresh();
}
function IndentRequisitionNo_ValueChange() {
    var val = $("#ddl_IndentRequisitionNo").val();
    if (val != 0) {
        $.ajax({
            type: "POST",
            url: 'PurchaseOrder.aspx/getIndentRequisitionDate',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{IndentRequisitionNo:\"" + val + "\"}",
            success: function (type) {
                var Transdt = new Date(type.d);
                cIndentRequisDate.SetDate(Transdt);
            }
        });
    }
    else {
        cIndentRequisDate.SetVal("");
    }
}       
function SetDifference() {
    var diff = CheckDifference();
    if (diff > 0) {
        clientResult.SetText(diff.toString());
    }
}
function CheckDifference() {
    var startDate = new Date();
    var endDate = new Date();
    var difference = -1;
    startDate = cPLQuoteDate.GetDate();
    if (startDate != null) {
        endDate = cExpiryDate.GetDate();
        var startTime = startDate.getTime();
        var endTime = endDate.getTime();
        difference = (endTime - startTime) / 86400000;
    }
    return difference;
}        

function Keypressevt() {
    if (event.keyCode == 13) {
        //run code for Ctrl+X -- ie, Save & Exit! 
        SaveWarehouse();
        return false;
    }
}
function getUrlVars() {
    var vars = [], hash;
    var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
    for (var i = 0; i < hashes.length; i++) {
        hash = hashes[i].split('=');
        vars.push(hash[0]);
        vars[hash[0]] = hash[1];
    }
    return vars;
}
$(document).ready(function () {



    var urlKeys = getUrlVars();
    if (urlKeys.key != 'ADD') {

        cPLQuoteDate.SetEnabled(false);
        if(cddl_AmountAre.GetValue()=="3")
        {                    
            grid.GetEditor('gvColTaxAmount').SetEnabled(false);
        }
    }
    var isCtrl = false;
    document.onkeydown = function (e) {
        if (event.keyCode == 83 && event.altKey == true) {
            if (($("#exampleModal").data('bs.modal') || {}).isShown) {

                SaveVehicleControlData();
            }
        }
        if (event.keyCode == 67 && event.altKey == true && getUrlVars().req != "V") {

            modalShowHide(0);
        }
        if (event.keyCode == 82 && event.altKey == true && getUrlVars().req != "V") {
            modalShowHide(1);
            $('body').on('shown.bs.modal', '#exampleModal', function () {
                $('input:visible:enabled:first', this).focus();
            })
        }
        if (event.keyCode == 83 && event.altKey == true && getUrlVars().req != "V" && $("#hddnDocumentIdTagged").val()!="1") {

            Save_ButtonClick();
        }
        else if (event.keyCode == 88 && event.altKey == true && getUrlVars().req != "V" && $("#hddnDocumentIdTagged").val()!="1") {

            SaveExit_ButtonClick();
        }
        else if (event.keyCode == 85 && event.altKey == true) {

            OpenUdf();
        }
        else if (event.keyCode == 84 && event.altKey == true) {

            Save_TaxesClick();
        }
        else if (event.keyCode == 79 && event.altKey == true) { //run code for Ctrl+O -- ie, Billing/Shipping Samrat!     
            StopDefaultAction(e);
            if (page.GetActiveTabIndex() == 1) {
                fnSaveBillingShipping();
            }
        }
        else if (event.keyCode == 77 && event.altKey == true) {
            $('#TermsConditionseModal').modal({
                show: 'true'
            });
        }
        else if (event.keyCode == 69 && event.altKey == true) {
            if (($("#TermsConditionseModal").data('bs.modal') || {}).isShown) {
                StopDefaultAction(e);
                SaveTermsConditionData();
            }
        }
        else if (event.keyCode == 76 && event.altKey == true) {
            StopDefaultAction(e);
            calcelbuttonclick();
        }
    }
});
function StopDefaultAction(e) {
    if (e.preventDefault) { e.preventDefault() }
    else { e.stop() };

    e.returnValue = false;
    e.stopPropagation();
}

function DeleteWarehousebatchserial(SrlNo, BatchWarehouseID, viewQuantity, Quantity, WarehouseID, BatchNo) {
    var IsSerial = $('#hdnisserial').val();             
    if (BatchWarehouseID == "" || BatchWarehouseID == "0") {
        $('#hdnisolddeleted').val("false");
        if (SrlNo != "") {
            cGrdWarehousePC.PerformCallback('Delete~' + SrlNo + '~' + viewQuantity + '~' + Quantity + '~' + WarehouseID + '~' + BatchNo);
        }
    } else {
        $('#hdnisolddeleted').val("true");
        if (SrlNo != "") {
            cGrdWarehousePC.PerformCallback('Delete~' + SrlNo + '~' + viewQuantity + '~' + Quantity + '~' + WarehouseID + '~' + BatchNo);
        }
    }             
}

function Setenterfocuse(s) {             
}

function UpdateWarehousebatchserial(SrlNo, WarehouseID, BatchNo, SerialNo, isnew, viewQuantity, Quantity) {
    var Isbatch = $('#hdnisbatch').val();
    if (isnew == "old" || isnew == "Updated") {
        $('#hdnisoldupdate').val("true");
        $('#hdncurrentslno').val("");
        cCmbWarehouse.SetValue(WarehouseID);
        if (Quantity != null && Quantity != "" && Isbatch != "true") {
            ctxtqnty.SetText(Quantity);
        } else {
            ctxtqnty.SetText(viewQuantity);
        }
        var IsSerial = $('#hdnisserial').val();
        if (IsSerial == "true") {
            if (viewQuantity == "") {
                ctxtbatch.SetEnabled(false);
                cCmbWarehouse.SetEnabled(false);
                ctxtqnty.SetEnabled(false);
                ctxtserial.Focus();
            } else {
                ctxtbatch.SetEnabled(true);
                cCmbWarehouse.SetEnabled(true);
                ctxtqnty.SetEnabled(false);
                ctxtserial.Focus();
                ctxtqnty.SetText(viewQuantity);
            }
        }
        else {
            ctxtbatch.SetEnabled(true);
            cCmbWarehouse.SetEnabled(true);
            ctxtqnty.SetEnabled(true);
            ctxtbatch.Focus();
        }                 
        ctxtbatchqnty.SetText(Quantity);                
        ctxtbatch.SetText(BatchNo);
        ctxtserial.SetText(SerialNo);
        if (viewQuantity == "") {
            ctxtbatch.SetEnabled(false);
            cCmbWarehouse.SetEnabled(false);
            $('#hdnisviewqntityhas').val("true");
        } else {
            ctxtbatch.SetEnabled(true);
            cCmbWarehouse.SetEnabled(true);
            $('#hdnisviewqntityhas').val("false");
        }
        var hdniswarehouse = $('#hdniswarehouse').val();
        if (hdniswarehouse != "true" && Isbatch == "true") {
            ctxtbatchqnty.SetText(viewQuantity);
            ctxtbatchqnty.Focus();

        } else {
            ctxtqnty.Focus();
        }
        $('#hdncurrentslno').val(SrlNo);

    } else {

        $('#hdnisoldupdate').val("false");
        ctxtqnty.SetText("0.0");
        ctxtqnty.SetEnabled(true);
        ctxtbatchqnty.SetText("0.0");
        ctxtserial.SetText("");
        ctxtbatchqnty.SetText("");
        $('#hdncurrentslno').val("");

        $('#hdnisnewupdate').val("true");
        $('#hdncurrentslno').val("");
        cCmbWarehouse.SetValue(WarehouseID);
        if (Quantity != null && Quantity != "" && Isbatch != "true") {
            ctxtqnty.SetText(Quantity);
        } else {
            ctxtqnty.SetText(viewQuantity);
        }
        var IsSerial = $('#hdnisserial').val();
        if (IsSerial == "true") {

            if (viewQuantity == "") {
                ctxtbatch.SetEnabled(false);
                cCmbWarehouse.SetEnabled(false);
                ctxtqnty.SetEnabled(false);
                $('#hdnisviewqntityhas').val("true");
                ctxtserial.Focus();
            } else {
                ctxtbatch.SetEnabled(true);
                cCmbWarehouse.SetEnabled(true);
                ctxtqnty.SetEnabled(false);
                $('#hdnisviewqntityhas').val("false");
                ctxtserial.Focus();
                ctxtqnty.SetText(viewQuantity);
            }

        } else {
            ctxtbatch.SetEnabled(true);
            cCmbWarehouse.SetEnabled(true);
            ctxtqnty.SetEnabled(true);
            ctxtbatch.Focus();
        }
        // ctxtqnty.SetEnabled(false);

        ctxtbatchqnty.SetText(Quantity);
        //ctxtbatchqnty.SetEnabled(false);
        ctxtbatch.SetText(BatchNo);
        ctxtserial.SetText(SerialNo);

        if (viewQuantity == "") {
            ctxtbatch.SetEnabled(false);
            cCmbWarehouse.SetEnabled(false);
        } else {
            ctxtbatch.SetEnabled(true);
            cCmbWarehouse.SetEnabled(true);
        }
        var hdniswarehouse = $('#hdniswarehouse').val();
        if (hdniswarehouse != "true" && Isbatch == "true") {
            ctxtbatchqnty.SetText(viewQuantity);
        } else {
            ctxtqnty.Focus();
        }
        $('#hdncurrentslno').val(SrlNo);                 
    }
}

function changedqnty(s) {
    var qnty = s.GetText();
    var sum = $('#hdntotalqntyPC').val();
    sum = Number(Number(sum) + Number(qnty));            
    $('#hdntotalqntyPC').val(sum);              
}
function endcallcmware(s) {
    if (cCmbWarehouse.cpstock != null) {
        var ddd = cCmbWarehouse.cpstock + " " + $('#hdnstrUOM').val();
        document.getElementById('lblAvailableStk').innerHTML = ddd;
        cCmbWarehouse.cpstock = null;
    }
}
function changedqntybatch(s) {
    var qnty = s.GetText();
    var sum = $('#hdntotalqntyPC').val();
    sum = Number(Number(sum) + Number(qnty));
    $('#hdntotalqntyPC').val(sum);
}
function chnagedbtach(s) {
    $('#hdnoldbatchno').val(s.GetText());
    $('#hidencountforserial').val(1);
    var sum = $('#hdnbatchchanged').val();
    sum = Number(Number(sum) + Number(1));
    $('#hdnbatchchanged').val(sum);            
    ctxtexpirdate.SetText("");
    ctxtmkgdate.SetText("");
}

function CmbWarehouse_ValueChange(s) {
    var ISupdate = $('#hdnisoldupdate').val();
    var isnewupdate = $('#hdnisnewupdate').val();
    $('#hdnoldwarehousname').val(s.GetText());
    if (ISupdate == "true" || isnewupdate == "true") {
    } else {              

        ctxtserial.SetValue("");                 
        ctxtbatch.SetEnabled(true);
        ctxtexpirdate.SetEnabled(true);
        ctxtmkgdate.SetEnabled(true);                 
    }
}

function Clraear() {
    ctxtbatch.SetValue("");
    ASPx.CalClearClick('txtmkgdate_DDD_C');
    ASPx.CalClearClick('txtexpirdate_DDD_C');
    $('#hdnisoldupdate').val("false");          
    ctxtserial.SetValue("");
    ctxtqnty.SetValue("0.0000");
    ctxtbatchqnty.SetValue("0.0000");
    $('#hdntotalqntyPC').val(0);
    $('#hidencountforserial').val(1);
    $('#hdnbatchchanged').val("0");
    var strProductID = $('#hdfProductIDPC').val();
    var stockids = $('#hdfstockidPC').val();
    var branchid = $('#hdbranchIDPC').val();
    var strProductName = $('#lblProductName').text();
    $('#hdnisnewupdate').val("false");
    ctxtbatch.SetEnabled(true);
    ctxtexpirdate.SetEnabled(true);
    ctxtmkgdate.SetEnabled(true);
    ctxtbatch.SetEnabled(true);
    cCmbWarehouse.SetEnabled(true);
    $('#hdnisviewqntityhas').val("false");
    $('#hdnisolddeleted').val("false");
    ctxtqnty.SetEnabled(true);
    var existingqntity = $('#hdfopeningstockPC').val();
    var totaldeleteqnt = $('#hdndeleteqnity').val();
    var addqntity = Number(existingqntity) + Number(totaldeleteqnt);
    $('#hdndeleteqnity').val(0);   

    cGrdWarehousePC.PerformCallback('checkdataexist~' + strProductID + '~' + stockids + '~' + branchid + '~' + strProductName);
}

function DataPopulatedWarehouseGrid()
{
    var WarehouseID = cCmbWarehouse.GetValue();
    var WarehouseName = cCmbWarehouse.GetText();
    var qnty = ctxtqnty.GetText();
    var IsSerial = $('#hdnisserial').val();     
    if (qnty == "0.0000") {
        qnty = ctxtbatchqnty.GetText();
    }
    if (Number(qnty) % 1 != 0 && IsSerial == "true") {
        jAlert("Serial number is activated, Quantity should not contain decimals. ");
        return;
    }           
    var BatchName = ctxtbatch.GetText();
    var SerialName = ctxtserial.GetText();
    var Isbatch = $('#hdnisbatch').val();
    var enterdqntity = $('#hdfopeningstockPC').val();
    var hdniswarehouse = $('#hdniswarehouse').val();
    var ISupdate = $('#hdnisoldupdate').val();
    var isnewupdate = $('#hdnisnewupdate').val();          
    if (Isbatch == "true" && hdniswarehouse == "false") {
        qnty = ctxtbatchqnty.GetText();
    }
    if (ISupdate == "true") {
        if (hdniswarehouse == "true" && WarehouseID == null) {
            $("#RequiredFieldValidatorCmbWarehouse").css("display", "block");
        }
        else {
            $("#RequiredFieldValidatorCmbWarehouse").css("display", "none");
        }
        if (qnty == "0.0") {
            if (Isbatch != "false" || hdniswarehouse != "false") {
                $("#RequiredFieldValidatortxtbatchqntity").css("display", "block");
                $("#RequiredFieldValidatortxtwareqntity").css("display", "block");                      
            } else if (Isbatch == "false" && hdniswarehouse == "false" && IsSerial == "true") {
                qnty = "0.00"
                $("#RequiredFieldValidatortxtbatchqntity").css("display", "none");
                $("#RequiredFieldValidatortxtwareqntity").css("display", "none");
            }
        } else {
            qnty = "0.00"
            $("#RequiredFieldValidatortxtbatchqntity").css("display", "none");
            $("#RequiredFieldValidatortxtwareqntity").css("display", "none");
        }
        if (Isbatch == "true" && BatchName == "") {
            $("#RequiredFieldValidatortxtbatch").css("display", "block");
            ctxtbatch.Focus();
        } else {
            $("#RequiredFieldValidatortxtbatch").css("display", "none");
        }
        if (IsSerial == "true" && SerialName == "") {
            $("#RequiredFieldValidatortxtserial").css("display", "block");
            ctxtserial.Focus();
        } else {
            $("#RequiredFieldValidatortxtserial").css("display", "none");
        }
        var slno = $('#hdncurrentslno').val();
        if (slno != "") {
            cGrdWarehousePC.PerformCallback('Updatedata~' + WarehouseID + '~' + WarehouseName + '~' + BatchName + '~' + SerialName + '~' + slno + '~' + qnty);              
            return false;
        }
    } else if (isnewupdate == "true") {
        if (hdniswarehouse == "true" && WarehouseID == null) {
            $("#RequiredFieldValidatorCmbWarehouse").css("display", "block");
        }
        else {
            $("#RequiredFieldValidatorCmbWarehouse").css("display", "none");
        }
        if (qnty == "0.0") {
            if (Isbatch != "false" || hdniswarehouse != "false") {
                $("#RequiredFieldValidatortxtbatchqntity").css("display", "block");
                $("#RequiredFieldValidatortxtwareqntity").css("display", "block");                       
            } else if (Isbatch == "false" && hdniswarehouse == "false" && IsSerial == "true") {
                qnty = "0.00"
                $("#RequiredFieldValidatortxtbatchqntity").css("display", "none");
                $("#RequiredFieldValidatortxtwareqntity").css("display", "none");
            }
        } else {
            qnty = "0.00"
            $("#RequiredFieldValidatortxtbatchqntity").css("display", "none");
            $("#RequiredFieldValidatortxtwareqntity").css("display", "none");
        }
        if (Isbatch == "true" && BatchName == "") {
            $("#RequiredFieldValidatortxtbatch").css("display", "block");
            ctxtbatch.Focus();
        }
        else {
            $("#RequiredFieldValidatortxtbatch").css("display", "none");
        }
        if (IsSerial == "true" && SerialName == "") {
            $("#RequiredFieldValidatortxtserial").css("display", "block");
            ctxtserial.Focus();
        } else {
            $("#RequiredFieldValidatortxtserial").css("display", "none");
        }
        var slno = $('#hdncurrentslno').val();
        if (slno != "") {
            cGrdWarehousePC.PerformCallback('NewUpdatedata~' + WarehouseID + '~' + WarehouseName + '~' + BatchName + '~' + SerialName + '~' + slno + '~' + qnty);
            $('#hdnisviewqntityhas').val("false");
            $('#hdnisnewupdate').val("false");
            ctxtbatch.SetEnabled(true);
            cCmbWarehouse.SetEnabled(true);
            ctxtqnty.SetEnabled(true);
            ctxtqnty.SetText("0.0");
            ctxtbatch.SetText("");
            return false;
        }
    }
    else {

        var hdnisediteds = $('#hdnisedited').val();
        if (hdniswarehouse == "true" && WarehouseID == null) {
            $("#RequiredFieldValidatorCmbWarehouse").css("display", "block");
            return;
        } else {
            $("#RequiredFieldValidatorCmbWarehouse").css("display", "none");
        }
        if (qnty == "0.0") {
            if (Isbatch != "false" || hdniswarehouse != "false") {
                $("#RequiredFieldValidatortxtbatchqntity").css("display", "block");
                $("#RequiredFieldValidatortxtwareqntity").css("display", "block");                        
            } else if (Isbatch == "false" && hdniswarehouse == "false" && IsSerial == "true") {
                qnty = "0.00"
                $("#RequiredFieldValidatortxtbatchqntity").css("display", "none");
                $("#RequiredFieldValidatortxtwareqntity").css("display", "none");
            }
        } else {
            qnty = "0.00"
            $("#RequiredFieldValidatortxtbatchqntity").css("display", "none");
            $("#RequiredFieldValidatortxtwareqntity").css("display", "none");
        }
        if (Isbatch == "true" && BatchName == "") {
            $("#RequiredFieldValidatortxtbatch").css("display", "block");
            ctxtbatch.Focus();
            return;
        } else {
            $("#RequiredFieldValidatortxtbatch").css("display", "none");
        }
        if (IsSerial == "true" && SerialName == "") {
            $("#RequiredFieldValidatortxtserial").css("display", "block");
            ctxtserial.Focus();
            return;
        } else {
            $("#RequiredFieldValidatortxtserial").css("display", "none");
        }
        if (Isbatch == "true" && hdniswarehouse == "false") {
            qnty = ctxtbatchqnty.GetText();
            if (qnty == "0.0000") {                         
                ctxtbatchqnty.Focus();
            }
        }
        if (qnty == "0.0") {
            if (Isbatch != "false" || hdniswarehouse != "false") {
                $("#RequiredFieldValidatortxtbatchqntity").css("display", "block");
                $("#RequiredFieldValidatortxtwareqntity").css("display", "block");                         
            } else if (Isbatch == "false" && hdniswarehouse == "false" && IsSerial == "true") {
                qnty = "0.00"
                $("#RequiredFieldValidatortxtbatchqntity").css("display", "none");
                $("#RequiredFieldValidatortxtwareqntity").css("display", "none");
            }
        }
        else if (((hdniswarehouse == "true" && WarehouseID != null) || hdniswarehouse == "false") && ((Isbatch == "true" && BatchName != "") || Isbatch == "false") && ((IsSerial == "true" && SerialName != "") || IsSerial == "false") && qnty != "0.0") {
            $("#RequiredFieldValidatorCmbWarehouse").css("display", "none");
            $("#RequiredFieldValidatortxtbatch").css("display", "none");
            $("#RequiredFieldValidatortxtserial").css("display", "none");
            $("#RequiredFieldValidatortxtwareqntity").removeAttr("style");
            $("#RequiredFieldValidatortxtbatchqntity").removeAttr("style");
            $("#RequiredFieldValidatortxtbatchqntity").css("display", "none");
            $("#RequiredFieldValidatortxtwareqntity").css("display", "none");
            if (Isbatch == "true" && hdniswarehouse == "false") {
                qnty = ctxtbatchqnty.GetText();
                if (qnty = "0.0000") {
                    ctxtbatchqnty.Focus();
                }
            }
            var oldenterqntity = $('#hdnenterdopenqnty').val();
            var enterdqntityss = $('#hdnnewenterqntity').val();
            var deletedquantity = $('#hdndeleteqnity').val();                    
            if (Number(qnty) > (Number(enterdqntity) + Number(deletedquantity)) && hdnisediteds == "false") {
                qnty = "0.00";
                jAlert("You have entered Quantity greater than Opening Quantity. Cannot Proceed.");
            }
            else {
                cGrdWarehousePC.PerformCallback('Display~' + WarehouseID + '~' + WarehouseName + '~' + BatchName + '~' + SerialName + '~' + qnty);                         
                cCmbWarehouse.Focus();
            }
        }                 
        return false;
    }
}
function SaveWarehouse() {            
    var IsSerial = $('#hdnisserial').val(); 
    if (IsSerial == "true") 
    {
        var SerialNo = ctxtserial.GetText();
        var ProductID = $('#hdfProductIDPC').val(); 
        var Branch = $('#ddl_Branch').val();
        var objectToPass = {}
        objectToPass.SerialNo = SerialNo;
        objectToPass.ProductID = ProductID;
        objectToPass.BranchID = Branch;
        $.ajax({
            type: "POST",
            url: "Services/Master.asmx/CheckDuplicateSerial",
            data: JSON.stringify(objectToPass),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                if(msg.d==0){
                    DataPopulatedWarehouseGrid();
                }
                else{
                    ctxtserial.SetText("");
                    jAlert("Duplicate Serial No. entered. Cannot proceed.", "Alert", function () { ctxtserial.Focus(); });        
                }
            }
        });
    }
    else
    {
        DataPopulatedWarehouseGrid();
    }
}     

function SaveWarehouseAll() {         
    cGrdWarehousePC.PerformCallback('Saveall~');
}
function cGrdWarehousePCShowError(obj) {
    if (cGrdWarehousePC.cpdeletedata != null) {
        var existingqntity = $('#hdfopeningstockPC').val();
        var totaldeleteqnt = $('#hdndeleteqnity').val();
        var addqntity = Number(cGrdWarehousePC.cpdeletedata) + Number(existingqntity);
        var adddeleteqnty = Number(cGrdWarehousePC.cpdeletedata) + Number(totaldeleteqnt);
        $('#hdndeleteqnity').val(adddeleteqnty);                  
        cGrdWarehousePC.cpdeletedata = null;
    }

    if (cGrdWarehousePC.cpdeletedatasubsequent != null) {
        jAlert(cGrdWarehousePC.cpdeletedatasubsequent);
        cGrdWarehousePC.cpdeletedatasubsequent = null;
    }
    if (cGrdWarehousePC.cpbatchinsertmssg != null) {
        ctxtbatch.SetText("");
        ctxtqnty.SetValue("0.0000");
        ctxtbatchqnty.SetValue("0.0000");
        cGrdWarehousePC.cpbatchinsertmssg = null;
    }
    if (cGrdWarehousePC.cpupdateexistingdata != null) {
        $('#hdnisedited').val("true");
        cGrdWarehousePC.cpupdateexistingdata = null;
    }
    if (cGrdWarehousePC.cpupdatenewdata != null) {
        $('#hdnisedited').val("true");
        cGrdWarehousePC.cpupdateexistingdata = null;
    }
    if (cGrdWarehousePC.cpupdatemssgserialsetdisblebatch != null) {
        ctxtbatch.SetEnabled(false);
        ctxtexpirdate.SetEnabled(false);
        ctxtmkgdate.SetEnabled(false);
        cGrdWarehousePC.cpupdatemssgserialsetdisblebatch = null;
    }
    if (cGrdWarehousePC.cpupdatemssgserialsetenablebatch != null) {
        ctxtbatch.SetEnabled(true);
        ctxtexpirdate.SetEnabled(true);
        ctxtmkgdate.SetEnabled(true);
        $('#hidencountforserial').val(1);
        $('#hdnbatchchanged').val("0");
        $('#hidencountforserial').val("1");
        ctxtqnty.SetValue("0.0000");
        ctxtbatchqnty.SetValue("0.0000");
        ctxtbatch.SetText("");
        cGrdWarehousePC.cpupdatemssgserialsetenablebatch = null;
    }
    if (cGrdWarehousePC.cpproductname != null) {
        document.getElementById('lblpro').innerHTML = cGrdWarehousePC.cpproductname;
        cGrdWarehousePC.cpproductname = null;
    }             

    if (cGrdWarehousePC.cpupdatemssg != null) {
        if (cGrdWarehousePC.cpupdatemssg == "Saved Successfully.") {
            $('#hdntotalqntyPC').val("0");
            $('#hdnbatchchanged').val("0");
            $('#hidencountforserial').val("1");
            ctxtqnty.SetValue("0.0000");
            ctxtbatchqnty.SetValue("0.0000");
            parent.cPopup_WarehousePC.Hide();
            var hdnselectedbranch = $('#hdnselectedbranch').val();
            grid.batchEditApi.StartEdit(globalRowIndex, 9);                  
        } else {
            jAlert(cGrdWarehousePC.cpupdatemssg);
        }
        cGrdWarehousePC.cpupdatemssg = null;
    }
    if (cGrdWarehousePC.cpupdatemssgserial != null) {
        jAlert(cGrdWarehousePC.cpupdatemssgserial);
        cGrdWarehousePC.cpupdatemssgserial = null;
    }

    if (cGrdWarehousePC.cpinsertmssg != null) {
        $('#hidencountforserial').val(2);
        ctxtserial.SetValue("");
        ctxtserial.Focus();
        cGrdWarehousePC.cpinsertmssg = null;
    }
    if (cGrdWarehousePC.cpinsertmssgserial != null) {

        ctxtserial.SetValue("");
        ctxtserial.Focus();
        cGrdWarehousePC.cpinsertmssgserial = null;
    }


}
function Onddl_VatGstCstEndCallback(s, e) {
    if (s.GetItemCount() == 1) {
        cddlVatGstCst.SetEnabled(false);
    }
}
function ddlInventory_OnChange() {

    if ($('#ddlInventory').val() == 'Y') {
        ctaggingList.SetEnabled(true);
        //gridquotationLookup.SetEnabled(true);
        //document.getElementById('indentRequisition').style.display = 'block'
    }
    else {
        ctaggingList.SetEnabled(false);
        //gridquotationLookup.SetEnabled(false);
        //document.getElementById('indentRequisition').style.display = 'none'
    }
    //cproductLookUp.GetGridView().Refresh();
    var txt = "<table border='1' width=\"100%\" class=\"dynamicPopupTbl\"><tr class=\"HeaderStyle\"><th>Unique Id</th><th>Vendor Name</th></tr><table>";
    document.getElementById("CustomerTable").innerHTML = txt;
        
    //var _txt = "<table border='1' width=\"100%\" class=\"dynamicPopupTbl\"><tr class=\"HeaderStyle\"><th>Product Name</th><th>Inventory</th><th>HSN/SAC</th><th>Class</th><th>Brand</th><th>Installation Reqd.</th></tr><table>";
    var _txt = "<table border='1' width=\"100%\" class=\"dynamicPopupTbl\"><tr class=\"HeaderStyle\"><th>Product Code</th><th>Product Name</th><th>Inventory</th><th>HSN/SAC</th><th>Class</th><th>Brand</th></tr><table>";
    document.getElementById("ProductTable").innerHTML = _txt
}
         
$(document).ready(function () {
    if (GetObjectID('hdnCustomerId').value == null || GetObjectID('hdnCustomerId').value == '') {
        page.GetTabByName('Billing/Shipping').SetEnabled(false);
    }          
})
function SettingTabStatus() {
    if (GetObjectID('hdnCustomerId').value != null && GetObjectID('hdnCustomerId').value != '' && GetObjectID('hdnCustomerId').value != '0') {
        page.GetTabByName('[B]illing/Shipping').SetEnabled(true);
    }
}
function disp_prompt(name) {
    if (name == "tab0") {
        ctxtVendorName.Focus();
        //page.GetTabByName('[B]illing/Shipping').SetEnabled(true);
        page.GetTabByName('Billing/Shipping').SetEnabled(true);
        $("#divcross").show();                      
    }
    if (name == "tab1") {
        $("#divcross").hide();
        page.GetTabByName('General').SetEnabled(false);
        var custID = GetObjectID('hdnCustomerId').value;
        if (custID == null && custID == '') {
            jAlert('Please select a customer');
            page.SetActiveTabIndex(0);
            return;
        }
        else {
                     
        }
    }
}

$(document).ready(function () {           
    $(".makeFullscreen-icon").click(function (e) {
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

function Project_LostFocus() {           
    var projID = clookup_Project.GetValue();
    if (projID == null || projID == "") {
        $("#ddlHierarchy").val(0);
    }
}
function Project_gotFocus()
{            
    if ($("#hdnProjectSelectInEntryModule").val() == "1")
        clookup_Project.gridView.Refresh();
    clookup_Project.ShowDropDown();
}
function ProjectValueChange(s, e) {           
    var projID = clookup_Project.GetValue();
    $.ajax({
        type: "POST",
        url: 'PurchaseOrder.aspx/getHierarchyID',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ ProjID: projID }),
        success: function (msg) {
            var data = msg.d;
            $("#ddlHierarchy").val(data);
        }
    });
}  



function SetEntityType(Id) {

    $.ajax({
        type: "POST",
        url: "SalesQuotation.aspx/GetEntityType",
        data: JSON.stringify({ Id: Id }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (r) {
            $("#hdnEntityType").val(r.d);
        }

    });
}

//Mantis Issue 25152
function chkDirectorApprovalRequired_change() {
    var dropdownli = ""
    if ($('#chkDirectorApprovalRequired').is(':checked') == true) {
        $("#divEmployee, #divEmployeeIn").removeClass('hide');
    }
    else {
        $("#divEmployee, #divEmployeeIn").addClass('hide');
    }
}
//$(document).ready(function () {
            
//})
function BindModalEmployee() {
    var det = {};
    det.DBName = $('#hdDbName').val();
    var $select = $('#ddl_DirEmployee');
    $select.empty();
    //$select.append("<option value='0'>--Select--</option>");
    $.ajax({
        type: "POST",
        url: 'PurchaseOrder.aspx/AddModalEmployee',
        contentType: "application/json; charset=utf-8",
        async: false,
        dataType: "json",
        data: JSON.stringify(det),
        success: function (data) {
            var arr = data.d;
            console.log(arr)

            var htm ='';
            for (var i = 0; i < arr.length; i++) {
                htm += '<option value="' + arr[i].cnt_internalId + '">' + arr[i].DirectorName + '</option>'
            }
            $('#ddl_DirEmployee').html(htm)
            //$('<option>', {
            //    value: data.d.cnt_internalId
            //}).append(data.d.DirectorName).appendTo($select);
            //alert(data.d[0].cnt_internalId)
            //$.each(data.d, function (i, data) {
            //    alert(data.d[i].cnt_internalId)
            //    $('<option>', {
            //        value: data.d[i].cnt_internalId
            //    }).append(data.d[i].DirectorName).appendTo($select);
            //});

        },
        error: function (mydata) { alert("error"); alert(mydata); },

    });
}
//End od Mantis Issue 25152
