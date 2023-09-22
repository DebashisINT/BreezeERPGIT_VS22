
//==========================================================Revision History ============================================================================================
// 1.0   Priti   V2.0.38   11-04-2023     0025797:Cannot enter duplicate batch in Same warehouse, for the same product with same batch number
// 2.0   Priti   V2.0.39   22-09-2023     0026844: Stock In Happening in different Warehouse even if the Branch selection in different

//========================================== End Revision History =======================================================================================================

var Pre_Quantity = "0";
var Pre_Amt = "0";
var Pre_TotalAmt = "0";
var Cur_Quantity = "0";
var Cur_Amt = "0";
var Cur_TotalAmt = "0";
        $(function () {
            $('#UOMModal').on('hide.bs.modal', function () {
                //setTimeout(function () {
                grid.batchEditApi.StartEdit(globalRowIndex, 7);
                //}, 1000);
            });
        });

  $(document).ready(function () {
      if($("#hdnPageStatus").val() !="ADD")
      {
          cPLQuoteDate.SetEnabled(false);

          var dt = new Date();
          if ($("#hdnBackdateddate").val() != "0" && $("#hdnBackdateddate").val() != "") {
              cPLQuoteDate.SetEnabled(true)
              var Days = $("#hdnBackdateddate").val();
              var today = cPLQuoteDate.GetDate();
              var newdate = cPLQuoteDate.GetDate();
              newdate.setDate(today.getDate() - Math.round(Days));
              if ($("#hdnTagDateForbackdated").val() != "")
              {
                  if (new Date($("#hdnTagDateForbackdated").val()) > newdate) {
                      cPLQuoteDate.SetMinDate(new Date($("#hdnTagDateForbackdated").val()));
                      cPLQuoteDate.SetMaxDate(dt)
                  }
                  else {
                      cPLQuoteDate.SetMinDate(newdate);
                      cPLQuoteDate.SetMaxDate(dt);
                  }
              }
              else
              {
                  cPLQuoteDate.SetMinDate(newdate);
                  cPLQuoteDate.SetMaxDate(dt);
              }
          }
      }


    var mode = $('#hdnADDEditMode').val();
    if (mode == 'Edit') {
        if ($("#hdnCustomerId").val() != "")
        {
            var VendorID = $("#hdnCustomerId").val();
            SetEntityType(VendorID);
        }
    }
});
//<%--Use for set focus on UOM after press ok on UOM--%>

    var ShouldCheck;
    var _ComponentDetails;


    function AltQuantityLostFocus(s,e)
{
        $("#btnAddStock").focus();
}

    function ConversionFromUomToAltQuantity(s,e)
    {
        if($("#hdnShowUOMConversionInEntry").val()=="1")
        {
            grid.batchEditApi.StartEdit(globalRowIndex);

            var otherdet = {};
            var ProductdetailsID = grid.GetEditor('ProductID').GetValue();
            var splitDet = ProductdetailsID.split("||@||");
            
            var Quantity = (ctxtQty.GetValue() != null) ? ctxtQty.GetValue() : "0";
            // otherdet.Quantity = Quantity;
            // var UomId = splitDet[2];
            // otherdet.UomId = UomId;

            var ProductID = splitDet[0];
            otherdet.ProductID = ProductID;
            //var AltUomId = splitDet[3];
            //otherdet.AltUomId = AltUomId;
           
            $.ajax({
                type: "POST",
                url: "PurchaseChallan.aspx/GetWareHousePackingQuantity",
                data: JSON.stringify(otherdet),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (msg) {
                    if (msg.d.length != 0) {
                        var packingQuantity = msg.d[0].packing_quantity;
                        var sProduct_quantity = msg.d[0].sProduct_quantity;
                                 
                        // if (msg.d[0].isOverideConvertion == true) {
                        var uomfactor = 0
                        if (sProduct_quantity != 0 && packingQuantity != 0) {
                            uomfactor = parseFloat(packingQuantity / sProduct_quantity).toFixed(4);
                            $('#hddnuomFactor').val(parseFloat(packingQuantity / sProduct_quantity));
                        }
                        else {
                            $('#hddnuomFactor').val(0);
                        }
                        var uomfac_Qty_to_stock = $('#hddnuomFactor').val();

                        var Qty = (ctxtQty.GetValue() != null) ? ctxtQty.GetValue() : "0";

                        var calcQuantity = parseFloat(Qty * uomfac_Qty_to_stock).toFixed(4);

                        // grid.batchEditApi.StartEdit(globalRowIndex);
                          
                        if(msg.d[0].isOverideConvertion==true)
                        {
                            ctxtAltQty.SetValue(calcQuantity);
                        }
                            


                    }
                    else {
                        var packingQuantity = 0;
                        var sProduct_quantity = 0;
                    }
                            
                }
            });
        }         
}
       



    function disp_prompt(name) {
        if (name == "tab0") {
            page.GetTabByName('[B]illing/Shipping').SetEnabled(true);
            $("#divcross").show();
            cContactPerson.Focus();
        }
        if (name == "tab1") {
            var custID = GetObjectID('hdnCustomerId').value;
            $("#divcross").hide();
            page.GetTabByName('General').SetEnabled(false);
            if (custID == null && custID == '') {
                jAlert('Please select a Vendor');
                page.SetActiveTabIndex(0);
                return;
            }
            else {
                page.SetActiveTabIndex(1);
            }
        }
    }
function CmbScheme_ValueChange() {
    var val = $("#ddl_numberingScheme").val();

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

    //if($("#hdnBackdateddate").val() !="0" && $("#hdnBackdateddate").val()!="")
    //{
    //    var Days = $("#hdnBackdateddate").val();
    //    var today = cPLQuoteDate.GetDate();
    //    var newdate = cPLQuoteDate.GetDate();
    //    newdate.setDate(today.getDate() - Math.round(Days));
    //    cPLQuoteDate.SetMinDate(newdate);
    //    cPLQuoteDate.SetMaxDate(dt);

    //}
   


    $("#hdnTCBranchId").val(branchID);
    if (branchID != "") document.getElementById('ddl_Branch').value = branchID;
            
    $('#hdnBranchID').val(branchID);
    if(document.getElementById('btn_TermsCondition'))
        BinducTcBank();

    $('#txtVoucherNo').attr('maxLength', schemelength);

    ctxtCustName.SetText("");
    PosGstId = "";
    cddlPosGstChallan.SetValue(PosGstId);
    GetObjectID('hdnCustomerId').value = "";
    page.tabs[1].SetEnabled(false);

    var schemetypeValue = val;
    var schemetype = schemetypeValue.toString().split('~')[1];
    var schemelength = schemetypeValue.toString().split('~')[2];
    $('#txtVoucherNo').attr('maxLength', schemelength);
    BindWarehouse();

    if (schemetype == '0') {
        document.getElementById('txtVoucherNo').disabled = false;
        document.getElementById('txtVoucherNo').value = "";
        $("#txtVoucherNo").focus();
    }
    else if (schemetype == '1') {
        document.getElementById('txtVoucherNo').disabled = true;
        document.getElementById('txtVoucherNo').value = "Auto";
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

    SetPurchaseBillingShippingAddress( $('#ddl_Branch').val());
    //if ($("#hdnProjectSelectInEntryModule").val() == "1")
    //    clookup_Project.gridView.Refresh();
}

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

function ParentCustomerOnClose(newCustId, customerName, Unique) {
           

    GetObjectID('hdnCustomerId').value = newCustId;

    AspxDirectAddCustPopup.Hide();
    ctxtShipToPartyShippingAdd.SetText('');
    if (newCustId != "") {
        ctxtCustName.SetText(customerName);
        SetCustomer(newCustId, customerName);
    }
           
}

function AddVendorClick() {
    //var isLighterPage = $("#hidIsLigherContactPage").val();
    //// alert(isLighterPage);
    //if (isLighterPage == 1) {
    var url = '/OMS/management/Master/vendorPopup.html?var=1.1.4.6';
    AspxDirectAddCustPopup.SetContentUrl(url);
           
    AspxDirectAddCustPopup.RefreshContentUrl();
    AspxDirectAddCustPopup.Show();
    //}
    //else {
    //    var url = 'HRrecruitmentagent_general.aspx?id=' + 'ADD';
    //    //window.location.href = url;
    //    AspxDirectAddCustPopup.SetContentUrl(url);
           
    //    AspxDirectAddCustPopup.RefreshContentUrl();
    //    AspxDirectAddCustPopup.Show();

    //}
           
          
}



function BindWarehouse(){
    var objectToPass = {}
    objectToPass.Branch = $('#ddl_Branch').val();

    $.ajax({
        type: "POST",
        url: "Services/Master.asmx/GetWarehouse",
        data: JSON.stringify(objectToPass),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (r) {
            var ddlWarehouse = $("[id*=ddlWarehouse]");
            //Rev 2.0
            ddlWarehouse.empty();
             //Rev 2.0 End
            $.each(r.d, function () {
                ddlWarehouse.append($("<option></option>").val(this['Value']).html(this['Text']));
            });
        }
    });
}
//REV 1.0
function BatchNoUniqueCheck() {
    var BatchNo = document.getElementById("txtBatch").value;
    var WarehouseID = $("#ddlWarehouse").val();
    var ProductID=$("#hdfProductID").val();
    $.ajax({
        type: "POST",
        url: "PurchaseChallan.aspx/CheckUniqueBatchNo",
        data: JSON.stringify({ BatchNo: BatchNo, WarehouseID: WarehouseID, ProductID: ProductID }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (msg) {
            var data = msg.d;

            if (data == true) {
                $("#rfvBatch").show();

                document.getElementById("txtBatch").value = '';
                document.getElementById("txtBatch").focus();
            }
            else {
                $("#rfvBatch").hide();
            }
        }
    });
}
//REV 1.0 END
function txtBillNo_TextChanged() {
    var VoucherNo = document.getElementById("txtVoucherNo").value;

    $.ajax({
        type: "POST",
        url: "PurchaseChallan_Add.aspx/CheckUniqueName",
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
//Below function added by chinmoy
function IfVendorGstInIsBlank()
{
    //cddl_AmountAre.SetValue("3");
    cddl_AmountAre.SetValue("1"); //Mantis Id 0022416
    PopulateGSTCSTVAT();
    //cddl_AmountAre.SetEnabled(false);

}

function cmbContactPersonEndCall(s, e) {
    cddl_AmountAre.SetEnabled(true);
    var comboitem = cddl_AmountAre.FindItemByValue('4');
    if (comboitem != undefined && comboitem != null) {
        cddl_AmountAre.RemoveItem(comboitem.index);
    }

    if (cContactPerson.cpGSTN == "No") {
        if (cContactPerson.cpcountry != null && cContactPerson.cpcountry != '') {
            if (cContactPerson.cpcountry != '1') {
                cddl_AmountAre.AddItem("Import", "4");
                cddl_AmountAre.SetValue(4);
                cddl_AmountAre.SetEnabled(false);
            }

                //Added By Chinmoy
            else if($('#hfVendorGSTIN').val()=='')
            {
                IfVendorGstInIsBlank();
            }

            else if($('#hfVendorGSTIN').val()!='') 
            {
                cddl_AmountAre.SetValue("1");
            }
            //end
            //else {
            //    cddl_AmountAre.SetValue(1);
            //}
        }
        //else {
        //    cddl_AmountAre.SetValue(1);
        //}
    }
    //else {
    //    cddl_AmountAre.SetValue(1);
    //}

    cContactPerson.cpGSTN = null;
    cContactPerson.cpcountry = null;
}

function SetFocusonDemand(e) {
    var key = cddl_AmountAre.GetValue();
    //if (key == '1' || key == '3') {
    if (key == '1' || key == '2' || key == '3' || key == '4') {
        //if (grid.GetVisibleRowsOnPage() == 1) {
        //    grid.batchEditApi.StartEdit(0, 3);
        //}
    }
    //else if (key == '2') {
    // cddlVatGstCst.Focus();
    //}
    cddlPosGstChallan.Focus();
}

function PopulateGSTCSTVAT(e) {
    var key = cddl_AmountAre.GetValue();

    if (key == 1) {
        grid.GetEditor('TaxAmount').SetEnabled(true);
        cbtn_SaveRecords.SetVisible(true);

        grid.GetEditor('ProductName').Focus();
        if (grid.GetVisibleRowsOnPage() == 1) {
            grid.batchEditApi.StartEdit(0, 3);
        }

    }
    else if (key == 2) {
        grid.GetEditor('TaxAmount').SetEnabled(true);
        cbtn_SaveRecords.SetVisible(true);

        grid.GetEditor('ProductName').Focus();
        if (grid.GetVisibleRowsOnPage() == 1) {
            grid.batchEditApi.StartEdit(0, 3);
        }
    }
    else if (key == 3) {
        grid.GetEditor('TaxAmount').SetEnabled(false);
        cbtn_SaveRecords.SetVisible(false);

        if (grid.GetVisibleRowsOnPage() == 1) {
            grid.batchEditApi.StartEdit(0, 3);
        }
    }
}

function GlobalBillingShippingEndCallBack() {
    if (cbsComponentPanel.cpGlobalBillingShippingEndCallBack_Edit == "0") {
        cbsComponentPanel.cpGlobalBillingShippingEndCallBack_Edit = "0";
        var VendorID = document.getElementById('hdnCustomerId').value;

        if (VendorID != null) {
           // cContactPerson.PerformCallback('BindContactPerson~' + VendorID);

            var OtherDetails = {}
            OtherDetails.VendorId = VendorID;
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
function GetPCDateFormat(today) {
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
        today = yyyy + '-' + mm + '-' + dd;
    }

    return today;
}

function taggingListKeyDown(s, e) {
    if (e.htmlEvent.key == "Enter") {
        s.OnButtonClick(0);
    }
}

function taggingListButnClick(s, e) {
    var VendorID = GetObjectID('hdnCustomerId').value;

    if (VendorID != null) {
        ctaggingGrid.PerformCallback('BindComponentGrid');
        cpopup_taggingGrid.Show();
    }
}

//Chinmoy added below function
function validateOrderwithAmountAre(){
    //Check Multiple Row amount are selectedor not

    var selectedKeys = ctaggingGrid.GetSelectedKeysOnPage();
    var ammountsAreOrder="";
    if(selectedKeys.length>0){
        for(var loopcount = 0 ; loopcount<ctaggingGrid.GetVisibleRowsOnPage();loopcount++)
        {
            
            var nowselectedKey = ctaggingGrid.GetRowKey(loopcount);

            var found = selectedKeys.find(function(element) {
                return element == nowselectedKey;
            });

            if(found){
                if(ammountsAreOrder !="" && ammountsAreOrder !=ctaggingGrid.GetRow(loopcount).children[5].innerText){
                    jAlert("Unable to procceed. Amount are for the selected order(s) are different");
                    return false;
                }
                else
                    ammountsAreOrder= ctaggingGrid.GetRow(loopcount).children[5].innerText;
            }
            
        }
    
    }

    return true;
}
//End
//Chinmoy edited below function
var SimilarProjectStatus = "0";

function SimilarProjetcheck(quote_Id,Doctype)
{
    $.ajax({
        type: "POST",
        url: "PurchaseChallan.aspx/DocWiseSimilarProjectCheck",
        data: JSON.stringify({ quote_Id: quote_Id, Doctype: Doctype }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (msg) {
            SimilarProjectStatus = msg.d;
            debugger;
            if (SimilarProjectStatus != "1") {
                cPLQADate.SetText("");
                jAlert("Please select document with same project code to proceed.");

                return false;

            }
        }
    });
}


function QuotationNumberChanged() {

    var OrderData = ctaggingGrid.GetSelectedKeysOnPage();


    var quotetag_Id =ctaggingGrid.GetSelectedKeysOnPage();

    if (quotetag_Id.length > 0 && $("#hdnProjectSelectInEntryModule").val() == "1") {
        var Doctype = "Purchase_Order";
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

        SimilarProjetcheck(quote_Id,Doctype);
    }

    if (SimilarProjectStatus != "-1") {
        if(validateOrderwithAmountAre()==false)
        {
            cpopup_taggingGrid.Hide();
            cProductsPopup.Hide();
        }
        else if(OrderData==0)
        {
            cpopup_taggingGrid.Hide();
        }
        else if(OrderData!=0 && validateOrderwithAmountAre()==true)
        {
            cgridproducts.PerformCallback('BindProductsDetails');
            cpopup_taggingGrid.Hide();
            cProductsPopup.Show();
        }

    }
}
//End

function gridProducts_EndCallback(s, e) {
    if (cgridproducts.cpComponentDetails) {
        _ComponentDetails=cgridproducts.cpComponentDetails;
        cgridproducts.cpComponentDetails = null;

        clookup_Project.gridView.Refresh();
        var  _cpProjectID=_ComponentDetails.split('~')[2];
        clookup_Project.gridView.SelectItemsByKey(_cpProjectID);
        if (_cpProjectID>0) {
            //clookup_Project.gridView.SetEnabled=false;
            clookup_Project.SetEnabled(false);
        }
        else {
            clookup_Project.SetEnabled(true);
        }

        var projID = clookup_Project.GetValue();

        $.ajax({
            type: "POST",
            url: 'PurchaseChallan.aspx/getHierarchyID',
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

function ChangeState(value) {
    cgridproducts.PerformCallback('SelectAndDeSelectProducts' + '~' + value);
}

function Tag_ChangeState(value) {
    ctaggingGrid.PerformCallback('SelectAndDeSelectProducts' + '~' + value);
}


//Chinmoy added this function
//Start
var Address = [];
var ReturnDetails;
function  GetPurchaseOrderDocumentAddress(OrderId)
{
    if ((OrderId != null) && (OrderId != "")) {

        $.ajax({
            type: "POST",
            url: "PurchaseChallan.aspx/PurchaseOrderDocumentAddress",
            data: JSON.stringify({OrderId:OrderId}),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                Address = msg.d;
                PurchaseOrerBillingShippingAddress(Address);

            }
        });
    }
}      


function PurchaseOrerBillingShippingAddress(ReturnDetails) {

    var BillingDetails = $.grep(ReturnDetails, function (e) { return e.Type == "Billing" })
    var ShippingDetails = $.grep(ReturnDetails, function (e) { return e.Type == "Shipping" })

    //Billing Address Details
    if (BillingDetails.length > 0) {
        ctxtAddress1.SetText(BillingDetails[0].Address1);
        ctxtAddress2.SetText(BillingDetails[0].Address2);
        ctxtAddress3.SetText(BillingDetails[0].Address3);
        ctxtlandmark.SetText(BillingDetails[0].Landmark);
        ctxtbillingPin.SetText(BillingDetails[0].PinCode);
        $('#hdBillingPin').val(BillingDetails[0].PinId);
        ctxtbillingCountry.SetText(BillingDetails[0].CountryName);
        $('#hdCountryIdBilling').val(BillingDetails[0].CountryId);
        ctxtbillingState.SetText(BillingDetails[0].StateName);
        $('#hdStateCodeBilling').val(BillingDetails[0].StateCode);
        $('#hdStateIdBilling').val(BillingDetails[0].StateId);
        ctxtbillingCity.SetText(BillingDetails[0].CityName);
        $('#hdCityIdBilling').val(BillingDetails[0].CityId);
        ctxtSelectBillingArea.SetText(BillingDetails[0].AreaName);
        $('#hdAreaIdBilling').val(BillingDetails[0].AreaId);
       

        var GSTIN = BillingDetails[0].GSTIN;
        var GSTIN1 = GSTIN.substring(0, 2);
        var GSTIN2 = GSTIN.substring(2, 12);
        var GSTIN3 = GSTIN.substring(12, 15);

        ctxtBillingGSTIN1.SetText(GSTIN1);
        ctxtBillingGSTIN2.SetText(GSTIN2);
        ctxtBillingGSTIN3.SetText(GSTIN3);
       

        //cddlPosGstChallan.SetValue(BillingDetails[0].PosForGst);
        PosGstId = BillingDetails[0].PosForGst;


    }
    else {
        ctxtAddress1.SetText('');
        ctxtAddress2.SetText('');
        ctxtAddress3.SetText('');
        ctxtlandmark.SetText('');
        ctxtbillingPin.SetText('');
        $('#hdBillingPin').val('');
        ctxtbillingCountry.SetText('');
        $('#hdCountryIdBilling').val('');
        ctxtbillingState.SetText('');
        $('#hdStateCodeBilling').val('');
        $('#hdStateIdBilling').val('');
        ctxtbillingCity.SetText('');
        $('#hdCityIdBilling').val('');
        ctxtSelectBillingArea.SetText('');
        $('#hdAreaIdBilling').val('');
     
        ctxtBillingGSTIN1.SetText('');
        ctxtBillingGSTIN2.SetText('');
        ctxtBillingGSTIN3.SetText('');
    
    }

    //Shipping Address Details
    if (ShippingDetails.length > 0) {
        ctxtsAddress1.SetText(ShippingDetails[0].Address1);
        ctxtsAddress2.SetText(ShippingDetails[0].Address2);
        ctxtsAddress3.SetText(ShippingDetails[0].Address3);
        ctxtslandmark.SetText(ShippingDetails[0].Landmark);
        ctxtShippingPin.SetText(ShippingDetails[0].PinCode);
        $('#hdShippingPin').val(ShippingDetails[0].PinId);
        ctxtshippingCountry.SetText(ShippingDetails[0].CountryName);
        $('#hdCountryIdShipping').val(ShippingDetails[0].CountryId);
        ctxtshippingState.SetText(ShippingDetails[0].StateName);
        $('#hdStateCodeShipping').val(ShippingDetails[0].StateCode);
        $('#hdStateIdShipping').val(ShippingDetails[0].StateId);
        ctxtshippingCity.SetText(ShippingDetails[0].CityName);
        $('#hdCityIdShipping').val(ShippingDetails[0].CityId);
        ctxtSelectShippingArea.SetText(ShippingDetails[0].AreaName);
        $('#hdAreaIdShipping').val(ShippingDetails[0].AreaId);
     

        var GSTIN = ShippingDetails[0].GSTIN;
        var GSTIN1 = GSTIN.substring(0, 2);
        var GSTIN2 = GSTIN.substring(2, 12);
        var GSTIN3 = GSTIN.substring(12, 15);


        ctxtShippingGSTIN1.SetText(GSTIN1);
        ctxtShippingGSTIN2.SetText(GSTIN2);
        ctxtShippingGSTIN3.SetText(GSTIN3);
        $('#hdShipToParty').val(ShippingDetails[0].ShipToPartyId);
        ctxtShipToPartyShippingAdd.SetText(ShippingDetails[0].ShipToPartyName);
        // cddlPosGstChallan.SetValue(ShippingDetails[0].PosForGst);
        PosGstId = ShippingDetails[0].PosForGst;
    }
    else {
        ctxtsAddress1.SetText('');
        ctxtsAddress2.SetText('');
        ctxtsAddress3.SetText('');
        ctxtslandmark.SetText('');
        ctxtShippingPin.SetText('');
        $('#hdShippingPin').val('');
        ctxtshippingCountry.SetText('');
        $('#hdCountryIdShipping').val('');
        ctxtshippingState.SetText('');
        $('#hdStateCodeShipping').val('');
        $('#hdStateIdShipping').val('');
        ctxtshippingCity.SetText('');
        $('#hdCityIdShipping').val('');
        ctxtSelectShippingArea.SetText('');
        $('#hdAreaIdShipping').val('');
       
        ctxtShippingGSTIN1.SetText('');
        ctxtShippingGSTIN2.SetText('');
        ctxtShippingGSTIN3.SetText('');
        $('#hdShipToParty').val('');
        ctxtShipToPartyShippingAdd.SetText('');
    
    }
    GetPurchaseForGstValue();

}

//End







$(function () {
    $(".allownumericwithdecimal").on("keypress keyup blur", function (event) {
        //this.value = this.value.replace(/[^0-9\.]/g,'');
        $(this).val($(this).val().replace(/[^0-9\.]/g, ''));
        if ((event.which != 46 || $(this).val().indexOf('.') != -1) && (event.which < 48 || event.which > 57)) {
            event.preventDefault();
        }
    });
});

function PopulateMultiUomAltQuantity() {
           
    var otherdet = {};
    var Quantity = $("#UOMQuantity").val();
    otherdet.Quantity = Quantity;
    var UomId = ccmbUOM.GetValue();
    otherdet.UomId = UomId;
    var Productdetails = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
    var ProductID = Productdetails.split("||@||")[0];
    otherdet.ProductID = ProductID;
    var AltUomId = ccmbSecondUOM.GetValue();
    otherdet.AltUomId = AltUomId;

    $.ajax({
        type: "POST",
        url: "PurchaseChallan.aspx/GetPackingQuantity",
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

function SaveMultiUOM() {
           

    //grid.GetEditor('ProductID').GetText().split("||@||")[3];

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
    var Productdetails = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
    var ProductID = Productdetails.split("||@||")[0];
    var DetailsId = grid.GetEditor('DetailsId').GetValue();
    
    //alert(DetailsId)
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
     if (srlNo != "" && UomId != "" && UomName != "" && AltUomId != "" && ProductID != "" && AltUomId != null && AltUomName != "" && AltQnty != "0.0000")
        {
        // End of Mantis Issue 24429
        //// Mantis Issue 24397
        ////cgrid_MultiUOM.PerformCallback('SaveDisplay~' + srlNo + '~' + qnty + '~' + UomName + '~' + AltUomName + '~' + AltQnty + '~' + UomId + '~' + AltUomId + '~' + ProductID + '~' + DetailsId);
        //cgrid_MultiUOM.PerformCallback('SaveDisplay~' + srlNo + '~' + qnty + '~' + UomName + '~' + AltUomName + '~' + AltQnty + '~' + UomId + '~' + AltUomId + '~' + ProductID + '~' + DetailsId + '~' + BaseRate + '~'+ AltRate + '~' + UpdateRow);
        //// End of Mantis Issue 24429
        ////$("#AltUOMQuantity").val(parseFloat(0).toFixed(4));
        //cAltUOMQuantity.SetValue("0.0000");
        //// Mantis Issue 24429
        //$("#UOMQuantity").val(0);
        //ccmbBaseRate.SetValue(0) 
        //cAltUOMQuantity.SetValue(0)
        //ccmbAltRate.SetValue(0)
        //ccmbSecondUOM.SetValue("")
        if ((qnty != "0" && UpdateRow == 'True') || (qnty == "0" && UpdateRow == 'False') || (qnty != "0" && UpdateRow == 'False')) {
            // End of Rev Mantis Issue 24429
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
        }
            // Rev Mantis Issue 24429
            // End of Mantis Issue 24429
        else{
            return;
        }
        // End of Rev Mantis Issue 24429
              
    }
    else {
        return;
    }
}


        
function Delete_MultiUom(keyValue, SrlNo, DetailsId) {


    cgrid_MultiUOM.PerformCallback('MultiUomDelete~' + keyValue + '~' + SrlNo + '~' + DetailsId);

}
var hdMultiUOMID = "";
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
        //alert(cgrid_MultiUOM.cpBaseRate)
        // Mantis Issue 24429
        var AltQuantity = cgrid_MultiUOM.cpAltQuantity ;
        var AltUOM = cgrid_MultiUOM.cpAltUOM ;
        // End of Mantis Issue 24429
        grid.GetEditor("Quantity").SetValue(BaseQty);
        grid.GetEditor("PurchasePrice").SetValue(BaseRate);
        grid.GetEditor("TotalAmount").SetValue(BaseQty*BaseRate);
        grid.GetEditor("NetAmount").SetValue(BaseQty*BaseRate);
        // Mantis Issue 24429
        grid.GetEditor("Order_AltQuantity").SetValue(AltQuantity);
        grid.GetEditor("Order_AltUOM").SetValue(AltUOM);

        PurchasePriceTextChange(null, null);
        // End of Mantis Issue 24429
    }
    // End of Mantis Issue 24429
    if (cgrid_MultiUOM.cpOpenFocus == "OpenFocus") {
        ccmbSecondUOM.SetFocus();
    }
    //Mantis Issue 24429
    if (cgrid_MultiUOM.cpAllDetails == "EditData") {
        //$('#UOMQuantity').val(cgrid_MultiUOM.cpBaseQty);
        var Quan = (cgrid_MultiUOM.cpBaseQty).toFixed(4);
        $('#UOMQuantity').val(Quan);
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
        grid.batchEditApi.StartEdit(globalRowIndex);
        var SLNo = "";
        var val = 1;
        var detailsid=grid.GetEditor('DetailsId').GetValue();
        //Mantis Issue 24429
        //if (detailsid != null && detailsid != "") {
        //    SLNo = detailsid;
        //    //val = 1;
        //}
        //else {
        //    SLNo = grid.GetEditor('SrlNo').GetValue();
        //    // val = 1;
        //}

        ///End Mantis 24429



        var SLNo = grid.GetEditor('SrlNo').GetValue();
        cgrid_MultiUOM.PerformCallback('SetBaseQtyRateInGrid~'+ SLNo+'~' + detailsid);
        // End of Mantis Issue 24429
        setTimeout(function () {
            grid.batchEditApi.StartEdit(globalRowIndex, 12);
        }, 200)
    }
}
// Mantis Issue 24429
function CalcBaseQty()
{
    //var PackingQtyAlt = Productdetails.split("||@||")[26];
    //var PackingQty = Productdetails.split("||@||")[25];
    //var PackingSaleUOM = Productdetails.split("||@||")[29];  // Alternate UOM selected from Product Master (tbl_master_product_packingDetails.packing_saleUOM)

    var Productdetails = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
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

var Uomlength = 0;
function UomLenthCalculation() {
    grid.batchEditApi.StartEdit(globalRowIndex);
    var SLNo = "";
    var val = "";
    var detailsid=grid.GetEditor('DetailsId').GetValue();
    //Mantis Issue 24429
    //if (detailsid != null && detailsid != "") {
    //    SLNo = detailsid;
    //    val = 1;
    //}
    //else {
    //    SLNo = grid.GetEditor('SrlNo').GetValue();
    //   // val = 1;
    //}
    SLNo = grid.GetEditor('SrlNo').GetValue();
    //End of Mantis Issue 24429
   
    $.ajax({
        type: "POST",
        url: "PurchaseChallan.aspx/GetQuantityfromSL",
        data: JSON.stringify({ SLNo: SLNo, val: val }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (msg) {

            Uomlength = msg.d;

        }
    });
}





    $(document).ready(function(){
        // Change the selector if needed
        if($("#hdnShowUOMConversionInEntry").val()=="1")
        {
            $('#_Altdiv_Quantity').attr('style', 'display:block');
        }
        else
        {
            $('#_Altdiv_Quantity').attr('style', 'display:none');
        }


        if(caspxTaxpopUp.Hide())
        {
            grid.batchEditApi.StartEdit(globalRowIndex, 14);
        }
        if( $('#hdnPageStatus').val() == "EDIT")
        {
            PopulateChallanPosGst();
            cddlPosGstChallan.SetEnabled(false);
            LoadCustomerBillingShippingAddress(GetObjectID('hdnCustomerId').value);
            LoadBranchAddressInEditMode($('#ddl_Branch').val());
        }

        if ($("#hdnShowUOMConversionInEntry").val() == "1") {
            $("#btnSecondUOM").removeClass('hide');
        }
        else {
            $("#btnSecondUOM").addClass('hide');
        }

        var $table = $('table.scroll'),
            $bodyCells = $table.find('tbody tr:first').children(),
            colWidth;

        // Adjust the width of thead cells when window resizes
        $(window).resize(function() {
            // Get the tbody columns width array
            colWidth = $bodyCells.map(function() {
                return $(this).width();
            }).get();
    
            // Set the width of thead columns
            $table.find('thead tr').children().each(function(i, v) {
                $(v).width(colWidth[i]);
            });    
        }).resize(); // Trigger resize handler
    });


    var globalRowIndex;
var rowEditCtrl;
var Stock_EditID = "0";
var TaxOfProduct = [];

var _GetQuantityValue = "0";
var _GetPurchasePriceValue = "0";
var _GetDiscountValue = "0";
var _GetAmountValue = "0";

function GridCallBack() {
    grid.PerformCallback('Display');
}

function gridFocusedRowChanged(s, e) {
    globalRowIndex = e.visibleIndex;
}

function ProductButnClick(s, e) {
    if (e.buttonIndex == 0) {
        var VendorID = GetObjectID('hdnCustomerId').value;
        if (VendorID != null && VendorID != "") {
            Pre_Quantity = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
            Pre_Amt = (grid.GetEditor('TotalAmount').GetValue() != null) ? grid.GetEditor('TotalAmount').GetValue() : "0";
            Pre_TotalAmt = (grid.GetEditor('NetAmount').GetValue() != null) ? grid.GetEditor('NetAmount').GetValue() : "0";

            setTimeout(function () { $("#txtProdSearch").focus(); }, 500);

            $('#txtProdSearch').val('');
            $('#ProductModel').modal('show');
        }
        else {
            jAlert("Please Select a Vendor", "Alert", function () { ctxtCustName.Focus(); });
        }
    }
}

function SetProduct(Id, Name) {
    $('#ProductModel').modal('hide');

    var LookUpData = Id;
    var ProductCode = Name;

    if (!ProductCode) {
        LookUpData = null;
    }

    var SpliteDetails = Id.split("||@||");

    var Product_ID = Id;
    var Product_Code = SpliteDetails[1];
    var Product_Name = SpliteDetails[2];
    var HSNCode = SpliteDetails[14];
    var Purchase_UOMID = SpliteDetails[6];
    var Purchase_UOM = SpliteDetails[7];
    var Purchase_Price = SpliteDetails[10];
    var IsPackingActive = SpliteDetails[18];
    var Packing_Factor = SpliteDetails[19];
    var Packing_UOM = SpliteDetails[20];
    var Warehousetype = SpliteDetails[21];
    var IsComponent = SpliteDetails[22];
    var ComponentProduct = SpliteDetails[23];


    if(HSNCode=="" || HSNCode==null || HSNCode == typeof(undefined))
    {
        jConfirm('Selected product does not mapped with any HSN/SAC. Do you want to proceed?', 'Confirmation Dialog', function (r) {

            if (r == false) {
                return;
            }
            else
            {
                ctxtCustName.SetEnabled(false);
                cPLQuoteDate.SetEnabled(false);
                cddl_AmountAre.SetEnabled(false);
                cddlPosGstChallan.SetEnabled(false);
                document.getElementById("ddl_numberingScheme").disabled = true;
                document.getElementById("ddlInventory").disabled = true;
                grid.batchEditApi.StartEdit(globalRowIndex);

                var strRate = (ctxtRate.GetValue() != null && ctxtRate.GetValue() != "0") ? ctxtRate.GetValue() : "1";
                if (parseFloat(strRate) == 0) {
                    Purchase_Price = Purchase_Price;
                }
                else {
                    Purchase_Price = Purchase_Price / strRate;
                }

                grid.batchEditApi.StartEdit(globalRowIndex, 6);

                if (grid.GetEditor("ProductID").GetValue() != "" && grid.GetEditor("ProductID").GetValue() != null) {
                    var previousProductID = grid.GetEditor("ProductID").GetValue();
                    var _previousProductID = previousProductID.split("||@||")[0];

                    cDeletePanel.PerformCallback('DelProdbySl~' + grid.GetEditor("SrlNo").GetValue());  
                }


                Pre_Quantity = (grid.GetEditor("Quantity").GetText() != null) ? grid.GetEditor("Quantity").GetText() : "0";
                Pre_Amt = (grid.GetEditor("TotalAmount").GetText() != null) ? grid.GetEditor("TotalAmount").GetText() : "0";
                Pre_TotalAmt = (grid.GetEditor("NetAmount").GetText() != null) ? grid.GetEditor("NetAmount").GetText() : "0";

                grid.GetEditor("ProductID").SetText(Product_ID);
                grid.GetEditor("ProductName").SetText(Product_Code);
                grid.GetEditor("ProductDiscription").SetText(Product_Name);
                grid.GetEditor("Quantity").SetText("0");
                grid.GetEditor("PurchaseUOM").SetText(Purchase_UOM);
                grid.GetEditor("PurchasePrice").SetText(Purchase_Price);
                grid.GetEditor("Discount").SetText("0");
                grid.GetEditor("TotalAmount").SetText("0");
                grid.GetEditor("TaxAmount").SetText("0");
                grid.GetEditor("NetAmount").SetText("0");
                grid.GetEditor("TotalQty").SetText("0");
                grid.GetEditor("BalanceQty").SetText("0");
                grid.GetEditor("IsComponentProduct").SetText("");
                grid.GetEditor("DocID").SetText("");


            

                Cur_Quantity = "0";
                Cur_Amt = "0";
                Cur_TotalAmt = "0";
                CalculateAmount();



                var _SrlNo = grid.GetEditor("SrlNo").GetValue();
                if (TaxOfProduct.filter(function (e) { return e.SrlNo == _SrlNo; }).length == 0) {
                    var ProductTaxes = { SrlNo: _SrlNo, IsTaxEntry:"N" }
                    TaxOfProduct.push(ProductTaxes);
                    SetFocusAfterProductSelect();
                }
                else {
                    $.grep(TaxOfProduct, function (e) { if (e.SrlNo == _SrlNo) e.IsTaxEntry = "N"; });
                    SetFocusAfterProductSelect();
                }
            }
        });

    }
    else
    {



        ctxtCustName.SetEnabled(false);
        cPLQuoteDate.SetEnabled(false);
        cddl_AmountAre.SetEnabled(false);
        cddlPosGstChallan.SetEnabled(false);
        document.getElementById("ddl_numberingScheme").disabled = true;
        document.getElementById("ddlInventory").disabled = true;
        grid.batchEditApi.StartEdit(globalRowIndex);

        var strRate = (ctxtRate.GetValue() != null && ctxtRate.GetValue() != "0") ? ctxtRate.GetValue() : "1";
        if (parseFloat(strRate) == 0) {
            Purchase_Price = Purchase_Price;
        }
        else {
            Purchase_Price = Purchase_Price / strRate;
        }

        grid.batchEditApi.StartEdit(globalRowIndex, 6);

        if (grid.GetEditor("ProductID").GetValue() != "" && grid.GetEditor("ProductID").GetValue() != null) {
            var previousProductID = grid.GetEditor("ProductID").GetValue();
            var _previousProductID = previousProductID.split("||@||")[0];

            //cDeletePanel.PerformCallback('DelProdbySl~' + grid.GetEditor("SrlNo").GetValue());  
            deleteTax('DelProdbySl', grid.GetEditor("SrlNo").GetValue());
        }
        Pre_Quantity = (grid.GetEditor("Quantity").GetText() != null) ? grid.GetEditor("Quantity").GetText() : "0";
        Pre_Amt = (grid.GetEditor("TotalAmount").GetText() != null) ? grid.GetEditor("TotalAmount").GetText() : "0";
        Pre_TotalAmt = (grid.GetEditor("NetAmount").GetText() != null) ? grid.GetEditor("NetAmount").GetText() : "0";
        grid.GetEditor("ProductID").SetText(Product_ID);
        grid.GetEditor("ProductName").SetText(Product_Code);
        grid.GetEditor("ProductDiscription").SetText(Product_Name);
        grid.GetEditor("Quantity").SetText("0");
        grid.GetEditor("PurchaseUOM").SetText(Purchase_UOM);
        grid.GetEditor("PurchasePrice").SetText(Purchase_Price);
        grid.GetEditor("Discount").SetText("0");
        grid.GetEditor("TotalAmount").SetText("0");
        grid.GetEditor("TaxAmount").SetText("0");
        grid.GetEditor("NetAmount").SetText("0");
        grid.GetEditor("TotalQty").SetText("0");
        grid.GetEditor("BalanceQty").SetText("0");
        grid.GetEditor("IsComponentProduct").SetText("");
        grid.GetEditor("DocID").SetText("");           

        Cur_Quantity = "0";
        Cur_Amt = "0";
        Cur_TotalAmt = "0";
        CalculateAmount();
        var _SrlNo = grid.GetEditor("SrlNo").GetValue();
        if (TaxOfProduct.filter(function (e) { return e.SrlNo == _SrlNo; }).length == 0) {
            var ProductTaxes = { SrlNo: _SrlNo, IsTaxEntry:"N" }
            TaxOfProduct.push(ProductTaxes);
            SetFocusAfterProductSelect();
        }
        else {
            $.grep(TaxOfProduct, function (e) { if (e.SrlNo == _SrlNo) e.IsTaxEntry = "N"; });
            SetFocusAfterProductSelect();
        }
    }
}
function SetFocusAfterProductSelect(){
    setTimeout(function () {              
        grid.batchEditApi.StartEdit(globalRowIndex, 6);
        return;
    }, 200);
}

function deleteTax(Action, srl) {
    var OtherDetail = {};
    OtherDetail.Action = Action;
    OtherDetail.srl = srl;   
    uniqueId
    $.ajax({
        type: "POST",
        url: "PurchaseChallan.aspx/taxUpdatePanel_Callback",
        data: JSON.stringify(OtherDetail),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var Code = msg.d;
            if (Code != null) {
            }
            if (productid != "") {
                setTimeout(function () {
                    grid.batchEditApi.StartEdit(globalRowIndex, 6);
                }, 600)
            }
        }
    });
}





function ProductKeyDown(s, e) {
    if (e.htmlEvent.key == "Enter" || e.htmlEvent.key == "NumpadEnter") {
        s.OnButtonClick(0);
    }
    //if (e.htmlEvent.key == "Tab") {
    //    s.OnButtonClick(0);
    //}
}

function OnEndCallback(s, e) {
    var refreshType = document.getElementById('hdnRefreshType').value;
    var pageStatus = document.getElementById('hdnPageStatus').value;

    $('#hdnRefreshType').val('');

    if (grid.cpinserterrorwarehouse != null) {
        LoadingPanel.Hide();
        grid.batchEditApi.StartEdit(0, 2);
        jAlert(grid.cpinserterrorwarehouse);
        grid.cpinserterrorwarehouse = null;
    }
    else if (grid.cpSaveSuccessOrFail == "outrange") {
        LoadingPanel.Hide();
        grid.cpSaveSuccessOrFail = null;
        grid.batchEditApi.StartEdit(0, 2);
        jAlert('Can Not Add More Purchase Oder Number as Purchase Order Scheme Exausted.<br />Update The Scheme and Try Again');
    }
    else if (grid.cpSaveSuccessOrFail == "checkPartyInvoice") {
        LoadingPanel.Hide();
        grid.cpSaveSuccessOrFail = null;
        grid.batchEditApi.StartEdit(0, 2);
        jAlert('Party Invoice must be unique for the selected Vendor.');
    }
    else if (grid.cpSaveSuccessOrFail == "nullQuantity") {
        LoadingPanel.Hide();
        grid.cpSaveSuccessOrFail = null;
        grid.batchEditApi.StartEdit(0, 2);
        jAlert('Please fill Quantity');
    }
    else if (grid.cpSaveSuccessOrFail == "duplicateProduct") {
        LoadingPanel.Hide();
        grid.cpSaveSuccessOrFail = null;
        grid.batchEditApi.StartEdit(0, 2);
        jAlert('Can not Duplicate Product in the Challan List.');
    }
    else if (grid.cpSaveSuccessOrFail == "ExceedQuantity") {
        LoadingPanel.Hide();
        grid.cpSaveSuccessOrFail = null;
        grid.batchEditApi.StartEdit(0, 2);
        jAlert('Tagged product quantity exceeded.Update The quantity and Try Again.');
    }
    else if (grid.cpSaveSuccessOrFail == "MINExceedQuantity") {
        LoadingPanel.Hide();
        grid.cpSaveSuccessOrFail = null;
        grid.batchEditApi.StartEdit(0, 2);
        jAlert('Tagged product quantity can not reduce.Update The quantity and Try Again.');
    }
    else if (grid.cpSaveSuccessOrFail == "nullPurchasePrice") {
        LoadingPanel.Hide();
        grid.cpSaveSuccessOrFail = null;
        grid.batchEditApi.StartEdit(0, 2);
        jAlert('Purchase Price is Mandatory. Please enter values.');
    }
    else if (grid.cpSaveSuccessOrFail == "duplicateSerial") {
        LoadingPanel.Hide();
        var Msg = grid.cpduplicateSerialMsg;

        grid.cpSaveSuccessOrFail = null;
        grid.cpduplicateSerialMsg = null;
        grid.batchEditApi.StartEdit(0, 2);

        jAlert(Msg);
    }
   
    else if (grid.cpSaveSuccessOrFail == "checkWarehouse") {
                
        LoadingPanel.Hide();
        grid.batchEditApi.StartEdit(0, 2);
        var SrlNo = grid.cpProductSrlIDCheck;

        grid.cpSaveSuccessOrFail = null;
        grid.cpProductSrlIDCheck = null;
        var msg = "Make sure product quantity are equal with Warehouse quantity for SL No. " + SrlNo;
        jAlert(msg);
                
    }
    // Rev Mantis Issue 24061
    else if (grid.cpSaveSuccessOrFail == "NetAmountExceed") {
        LoadingPanel.Hide();
        grid.batchEditApi.StartEdit(0, 2);
        var SrlNo = grid.cpProductSrlIDCheck;

        grid.cpSaveSuccessOrFail = null;
        grid.cpProductSrlIDCheck = null;
        jAlert('Net Amount of selected Product from tagged document.<br />Cannot enter Net Amount more than Purchase Order Net Amount .');
    }
    // End of Rev Mantis Issue 24061

    else if (grid.cpSaveSuccessOrFail == "checkMultiUOMData") {
        LoadingPanel.Hide();
        grid.batchEditApi.StartEdit(0, 2);
        var SrlNo = grid.cpcheckMultiUOMData;
        var msg = "Please add Alt. Qty for SL No. " + SrlNo;
               
        grid.cpSaveSuccessOrFail = null;
        grid.cpSaveSuccessOrFail = '';
        grid.cpcheckMultiUOMData = null;
        jAlert(msg);
    }

    else if (grid.cpSaveSuccessOrFail == "transporteMandatory") {
        LoadingPanel.Hide();
        grid.cpSaveSuccessOrFail = null;
        grid.batchEditApi.StartEdit(0, 2);
        jAlert("Transporter is set as Mandatory. Please enter values.", "Alert", function () { $("#exampleModal").modal('show'); });
    }
    else if (grid.cpSaveSuccessOrFail == "TCMandatory") {
        LoadingPanel.Hide();
        grid.cpSaveSuccessOrFail = null;
        grid.batchEditApi.StartEdit(0, 2);
        jAlert("Terms and Condition is set as Mandatory. Please enter values.", "Alert", function () { $("#TermsConditionseModal").modal('show'); });
    }
    else if (grid.cpSaveSuccessOrFail == "duplicate") {
        LoadingPanel.Hide();
        grid.cpSaveSuccessOrFail = null;
        grid.batchEditApi.StartEdit(0, 2);
        jAlert('Can Not Save as Duplicate Purchase Order Numbe No. Found');
    }
    else if (grid.cpSaveSuccessOrFail == "errorInsert") {
        LoadingPanel.Hide();
        grid.cpSaveSuccessOrFail = null;
        grid.batchEditApi.StartEdit(0, 2);
        jAlert('Please try after sometime.');
    }
    else if (grid.cpSaveSuccessOrFail == "EmptyProject") {
        LoadingPanel.Hide();
        grid.cpSaveSuccessOrFail = null;
        grid.batchEditApi.StartEdit(0, 2);
        jAlert('Please select project.');
    }
    else if (grid.cpSaveSuccessOrFail == "transactionbeingused") {
        LoadingPanel.Hide();
        grid.cpSaveSuccessOrFail = null;
        grid.batchEditApi.StartEdit(0, 2);
        jAlert('Transaction exist. cannot be processed.');
    }
    else if (grid.cpSaveSuccessOrFail == "Ponotexist") {
        LoadingPanel.Hide();
        grid.cpSaveSuccessOrFail = null;
        grid.batchEditApi.StartEdit(0, 2);
        jAlert('Cannot Save. Selected Purchase   Order(s) in this document do not exist.');
    }
    else if (grid.cpSaveSuccessOrFail == "stockOut") {
        LoadingPanel.Hide();
        grid.cpSaveSuccessOrFail = null;
        grid.batchEditApi.StartEdit(0, 3);
        jAlert('Already stock out for this product.');
    }
    else if (grid.cpSaveSuccessOrFail == "allStockOut") {
        LoadingPanel.Hide();
        grid.cpSaveSuccessOrFail = null;

        jAlert("Already stock out for selected products.", 'Alert Dialog: [PurchaseChallan]', function (r) {
            if (r == true) {
                window.location.reload();
            }
        });
    }
    else if (grid.cpSaveSuccessOrFail == "PurchaseOrderMandatory") {
        LoadingPanel.Hide();
        grid.cpSaveSuccessOrFail = null;
        grid.batchEditApi.StartEdit(0, 2);
        jAlert('Purchase Order is mandatory while save the data.');
    }
    else {
        var PurchaseOrder_Number = grid.cpPurchaseOrderNo;
        var Order_Msg = "Purchase Challan No. " + PurchaseOrder_Number + " saved.";
        if (refreshType == "E") {
            if (grid.cpApproverStatus == "approve") {
                window.parent.popup.Hide();
                window.parent.cgridPendingApproval.PerformCallback();
            }
            else if (grid.cpApproverStatus == "rejected") {
                window.parent.popup.Hide();
                window.parent.cgridPendingApproval.PerformCallback();
            }

            if (PurchaseOrder_Number != "") {


                jAlert(Order_Msg, 'Alert Dialog: [Purchase Challan]', function (r) {
                    if (r == true) {
                        grid.cpSalesOrderNo = null;
                        window.location.assign("PurchaseChallanList.aspx");
                    }
                });

            }
            else {
                window.location.assign("PurchaseChallanList.aspx");
            }
        }
        else if (refreshType == "N") {
            if (grid.cpApproverStatus == "approve") {
                window.parent.popup.Hide();
                window.parent.cgridPendingApproval.PerformCallback();
            }
            else if (grid.cpApproverStatus == "rejected") {
                window.parent.popup.Hide();
                window.parent.cgridPendingApproval.PerformCallback();
            }
            if (PurchaseOrder_Number != "") {
                jAlert(Order_Msg, 'Alert Dialog: [PurchaseOrder]', function (r) {

                    grid.cpSalesOrderNo = null;
                    if (r == true) {
                        window.location.assign("PurchaseChallan.aspx?key=ADD&InvType="+$("#hdnChallanType").val());
                    }
                });
            }
            else {
                window.location.assign("PurchaseChallan.aspx?key=ADD&InvType="+$("#hdnChallanType").val());
            }
        }
        else {
            if (pageStatus == "first") {
                OnAddNewClick();
                grid.batchEditApi.EndEdit();
                $('#hdnPageStatus').val('');
            }
            else if (pageStatus == "update") {
                OnAddNewClick();
                $('#hdnPageStatus').val('');
            }
        }
    }

    if (grid.cpComponent) {
        if (grid.cpComponent == 'true') {
            grid.cpComponent = null;
            OnAddNewClick();
        }
    }
    
    if (grid.cpTaggingStockData) {
        if (grid.cpTaggingStockData!="") {
            var myObj=grid.cpTaggingStockData;            
            var JObject=JSON.parse(myObj);    
            
            if (JObject.length > 0) {
                for (x in JObject) {
                    JObject[x]["SrlNo"]=parseInt(JObject[x]["SrlNo"]);
                    JObject[x]["LoopID"]=parseInt(JObject[x]["LoopID"]);
                }
            }

            StockOfProduct=JObject;
            grid.cpTaggingStockData=null;
        }
    }

    if (grid.cpOrderRunningBalance) {
        var RunningBalance = grid.cpOrderRunningBalance;
        var RunningSpliteDetails = RunningBalance.split("~");
        grid.cpOrderRunningBalance = null;

        var SUM_ChargesAmount = RunningSpliteDetails[0];
        var SUM_Amount = RunningSpliteDetails[1];
        var SUM_TaxAmount = RunningSpliteDetails[3];
        var SUM_TotalAmount = RunningSpliteDetails[4];
        var SUM_ProductQuantity = parseFloat(RunningSpliteDetails[6]).toFixed(2);
        var Tax_Option = RunningSpliteDetails[7];
        var Currency = RunningSpliteDetails[8];
        var Rate = RunningSpliteDetails[9];
        var TaxableAmt = RunningSpliteDetails[10];
        cTaxableAmtval.SetValue(TaxableAmt);
        cTaxAmtval.SetValue(SUM_TaxAmount);
        ctxt_Charges.SetValue(SUM_ChargesAmount);
        cOtherTaxAmtval.SetValue(SUM_ChargesAmount);
        cInvValue.SetValue(SUM_TotalAmount);
        cTotalAmt.SetValue(SUM_TotalAmount);
        cTotalQty.SetValue(SUM_ProductQuantity);

        if(Tax_Option!="") cddl_AmountAre.SetValue(Tax_Option);
        if(Currency!="") document.getElementById('ddl_Currency').value = Currency;
        ctxtRate.SetValue(Rate);
    }

    if (ctaggingList.GetValue() != null && ctaggingList.GetValue()!="") {
        grid.GetEditor('ProductName').SetEnabled(false);

        ctxtCustName.SetEnabled(false);
        cPLQuoteDate.SetEnabled(false);
        cddl_AmountAre.SetEnabled(false);
        document.getElementById("ddl_numberingScheme").disabled = true;
        document.getElementById("ddlInventory").disabled = true;
    }

    cProductsPopup.Hide();
}


function callback_InlineRemarks_EndCall(s, e) {

    if (ccallback_InlineRemarks.cpDisplayFocus == "DisplayRemarksFocus") {
        $("#txtInlineRemarks").focus();
    }
    else  if (ccallback_InlineRemarks.cpRemarksFinalFocus == "RemarksFinalFocus")
    {
        cPopup_InlineRemarks.Hide();
        grid.batchEditApi.StartEdit(globalRowIndex, 6);
    }
    else {
        cPopup_InlineRemarks.Hide();
        grid.batchEditApi.StartEdit(globalRowIndex, 6);
    }
          
}

function FinalRemarks() {


    ccallback_InlineRemarks.PerformCallback('RemarksFinal' + '~' + grid.GetEditor('SrlNo').GetValue() + '~' + $('#txtInlineRemarks').val());
    $("#txtInlineRemarks").val('');
    cPopup_InlineRemarks.Hide();

}
var hdmultiuser = "";
// Mantis Issue 24429
function Edit_MultiUom(keyValue, SrlNo) {

    cbtnMUltiUOM.SetText("Update");
    cgrid_MultiUOM.PerformCallback('EditData~' + keyValue + '~' + SrlNo);

 

}
// End of Mantis Issue 24429

function OnCustomButtonClick(s, e) {
    if (e.buttonID == 'CustomAddNewRow') {
        var ProductID = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "";

        var SpliteDetails = ProductID.split("||@||");
        var IsComponentProduct = SpliteDetails[22];

        if (IsComponentProduct == "Y") {
            var ComponentProduct = SpliteDetails[23];

            var messege = "Selected product is defined with components.<br/> Would you like to proceed with components (" + ComponentProduct + ") ?";
            jConfirm(messege, 'Confirmation Dialog', function (r) {
                if (r == true) {
                    grid.GetEditor("IsComponentProduct").SetValue("Y");
                    $('#hdfIsDelete').val('C');

                    grid.AddNewRow();
                    grid.UpdateEdit();

                    grid.PerformCallback('Display~fromComponent');
                }
                else {
                    OnAddNewClick();
                }
            });
        }
        else {
            if (ProductID != "") {
                OnAddNewClick();
            }
            else {
                grid.batchEditApi.StartEdit(e.visibleIndex, 2);
            }
        }
    }

    else if (e.buttonID == "CustomaddDescRemarks") {

        var index = e.visibleIndex;
        grid.batchEditApi.StartEdit(e.visibleIndex,5);
        cPopup_InlineRemarks.Show();

        $("#txtInlineRemarks").val('');

        var SrlNo = (grid.GetEditor('SrlNo').GetValue() != null) ? grid.GetEditor('SrlNo').GetValue() : "";
        var ProductID = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "";
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

    else if (e.buttonID == 'CustomMultiUOM') {
            
        var index = e.visibleIndex;
        if($("#hddnMultiUOMSelection").val()=="1")
            grid.batchEditApi.StartEdit(e.visibleIndex, 8);
        var Productdetails = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
        var ProductID = Productdetails.split("||@||")[0];
        var UOMName = grid.GetEditor("PurchaseUOM").GetValue();
        var quantity = grid.GetEditor("Quantity").GetValue();
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
                    if($("#hddnMultiUOMSelection").val()=="1")
                        grid.batchEditApi.StartEdit(e.visibleIndex, 8);
                    //grid.batchEditApi.StartEdit(globalRowIndex);
                    var Qnty = grid.GetEditor("Quantity").GetValue();
                    var SrlNo = (grid.GetEditor('SrlNo').GetValue() != null) ? grid.GetEditor('SrlNo').GetValue() : "";
                    var UomId = grid.GetEditor('ProductID').GetText().split("||@||")[4];
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



    else if (e.buttonID == 'CustomDelete') {
        grid.batchEditApi.EndEdit();
        var noofvisiblerows = grid.GetVisibleRowsOnPage();
        $('#hdnRefreshType').val('');

        if (ctaggingList.GetValue() != null) {
            jAlert('Cannot Delete using this button as the Purchase Challan is linked with this Purchase Order.<br /> Click on Plus(+) sign to Add or Delete Product from last column!', 'Alert Dialog: [Delete Challan Products]', function (r) {
            });
        }

        if (noofvisiblerows != "1" && ctaggingList.GetValue() == null) {
            var ProductID = grid.batchEditApi.GetCellValue(e.visibleIndex, 'ProductID');

            if (ProductID != null) {
                Pre_Quantity = (grid.batchEditApi.GetCellValue(e.visibleIndex, 'Quantity') != null) ? grid.batchEditApi.GetCellValue(e.visibleIndex, 'Quantity') : "0";
                Pre_Amt = (grid.batchEditApi.GetCellValue(e.visibleIndex, 'TotalAmount') != null) ? grid.batchEditApi.GetCellValue(e.visibleIndex, 'TotalAmount') : "0";
                Pre_TotalAmt = (grid.batchEditApi.GetCellValue(e.visibleIndex, 'NetAmount') != null) ? grid.batchEditApi.GetCellValue(e.visibleIndex, 'NetAmount') : "0";

                Cur_Quantity = "0";
                Cur_Amt = "0";
                Cur_TotalAmt = "0";
                CalculateAmount();

                grid.DeleteRow(e.visibleIndex);
                $('#hdfIsDelete').val('D');

                grid.AddNewRow();
                grid.UpdateEdit();

                grid.PerformCallback('Display');
            }
        }
    }
    else if (e.buttonID == 'CustomWarehouse') {
        var index = e.visibleIndex;
        // Mantis Issue 24429
        //grid.batchEditApi.StartEdit(index, 9)
        grid.batchEditApi.StartEdit(index, 11)
        // End of Mantis Issue 24429
        Warehouseindex = index;

        var inventoryType = (document.getElementById("ddlInventory").value != null) ? document.getElementById("ddlInventory").value : "";

        if (inventoryType == "C" || inventoryType == "Y" || inventoryType == "B") {
            var SrlNo = (grid.GetEditor('SrlNo').GetValue() != null) ? grid.GetEditor('SrlNo').GetValue() : "";
            var IDs = (grid.GetEditor('ProductID').GetValue() != null) ? grid.GetEditor('ProductID').GetValue() : "";
            var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
            var SpliteDetails = IDs.split("||@||");
            var ProductID = SpliteDetails[0];
           
           
            
            if (parseFloat(QuantityValue) == "0") {
                jAlert("Quantity should not be zero !.");
            }
            else {
                if (ProductID != "") {
                    var Product_Name = SpliteDetails[2];
                    var Purchase_UOM = SpliteDetails[7];
                    var Warehousetype = SpliteDetails[21];
                    var serviceURL = "Services/Master.asmx/CheckDuplicateSerial";

                    $('#hdfProductID').val(ProductID);
                    $('#hdfWarehousetype').val(Warehousetype);
                    $('#hdfProductSrlNo').val(SrlNo);
                    $('#hdnProductQuantity').val(QuantityValue);
                    $('#hdfUOM').val(Purchase_UOM);
                    GetObjectID('hdfServiceURL').value = serviceURL;
                    GetObjectID('hdfBranch').value = $('#ddl_Branch').val();
                    GetObjectID('hdfIsRateExists').value = "N";
                    SecondUOMProductId = ProductID;
                    document.getElementById('lblProductName').innerHTML = Product_Name;
                    document.getElementById('lblEnteredAmount').innerHTML = QuantityValue;
                    document.getElementById('lblEnteredUOM').innerHTML = Purchase_UOM;
                    var AltUomId=SpliteDetails[29];
                    Stock_EditID = "0";
                    //cWarehousePanel.PerformCallback('StockDisplay');

                    if(GetObjectID('IsBarcodeActive').value=="Y"){
                        if (ctaggingList.GetValue()) {
                            if(GetObjectID('hdfWarehousetype').value=="W")
                            {
                                StockHeader.style.display =  'block';
                            }
                            else{
                                StockHeader.style.display = 'none';
                            }
                            
                        }
                        else{
                            StockHeader.style.display =  'block';
                        }
                    }
                    else{
                        StockHeader.style.display =  'block';
                    }

                    CreateStock();
                    ctxtQty.SetValue(QuantityValue);
                    ccmbAltUOM.SetValue(SpliteDetails[29]);
                    cPopupWarehouse.Show();
                }
            }
        }
        else {
            jAlert("You have selected Non-Inventory Item, so You cannot updated Stock.");
        }
    }
}

        
function AutoPopulateMultiUOM() {

    var Productdetails = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
    var ProductID = Productdetails.split("||@||")[0];
    var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";

    $.ajax({
        type: "POST",
        url: "PurchaseChallan.aspx/AutoPopulateAltQuantity",
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
                uomfactor = parseFloat(packingQuantity / sProduct_quantity).toFixed(4);
                $('#hddnuomFactor').val(parseFloat(packingQuantity / sProduct_quantity));
            }
            else {
                $('#hddnuomFactor').val(0);
            }

            var uomfac_Qty_to_stock = $('#hddnuomFactor').val();
            var Qty = QuantityValue;
            var calcQuantity = parseFloat(Qty * uomfac_Qty_to_stock).toFixed(4);
            if ($("#hdnPageStatus").val() == "EDIT") {
                ccmbSecondUOM.SetValue('');
                //$("#AltUOMQuantity").val(calcQuantity);

                //cAltUOMQuantity.SetValue(calcQuantity);
                cAltUOMQuantity.SetValue("0.0000");
            }
            else {
                //ccmbSecondUOM.SetValue(AltUOMId);
                if (AltUOMId == 0) {
                    ccmbSecondUOM.SetValue('');
                }
                else {
                    ccmbSecondUOM.SetValue(AltUOMId);
                }
                //Rev Mantis Issue 24429
                //cAltUOMQuantity.SetValue(calcQuantity);
                //Rev End Mantis Issue 24429
            }

        }
    });
}

function EditQuantityProductsGotFocus(s, e) {
    

    var ProductID = grid.GetEditor('ProductID').GetValue();

    if (ProductID != null) {
       
        Pre_Quantity = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
        Pre_Amt = (grid.GetEditor('TotalAmount').GetValue() != null) ? grid.GetEditor('TotalAmount').GetValue() : "0";
        Pre_TotalAmt = (grid.GetEditor('NetAmount').GetValue() != null) ? grid.GetEditor('NetAmount').GetValue() : "0";
        
        _GetQuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
        _GetPurchasePriceValue = (grid.GetEditor('PurchasePrice').GetValue() != null) ? grid.GetEditor('PurchasePrice').GetValue() : "0";
        _GetDiscountValue = (grid.GetEditor('Discount').GetValue() != null) ? grid.GetEditor('Discount').GetValue() : "0";
        _GetAmountValue = (grid.GetEditor('TotalAmount').GetValue() != null) ? grid.GetEditor('TotalAmount').GetValue() : "0";

       
        var ProductID = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
        var SpliteDetails = ProductID.split("||@||");
        var strProductID = SpliteDetails[0];
        var strProductName = SpliteDetails[1];
        var strDescription = SpliteDetails[1];
        var strUOM = SpliteDetails[2];
        var strStkUOM = SpliteDetails[4];
        var strSalePrice = SpliteDetails[6];
        var IsPackingActive = SpliteDetails[10];
        var Packing_Factor = SpliteDetails[11];
        var Packing_UOM = SpliteDetails[12];

        //debugger;

        //Rev 2.0 Subhra 11-03-2019     
        var isOverideConvertion = SpliteDetails[30];
        var packing_saleUOM = SpliteDetails[29];
        var sProduct_SaleUom = SpliteDetails[28];
        var sProduct_quantity = SpliteDetails[27];
        var packing_quantity = SpliteDetails[26];

        var slno= (grid.GetEditor('SrlNo').GetText() != null) ? grid.GetEditor('SrlNo').GetText() : "0";

        var PurchaseOrderNum= (grid.GetEditor('DocNumber').GetText() != null) ? grid.GetEditor('DocNumber').GetText() : "0";

        var ConvertionOverideVisible = $('#hdnConvertionOverideVisible').val();
        var ShowUOMConversionInEntry = $('#hdnShowUOMConversionInEntry').val();
        var type = 'add';
        var gridprodqty = parseFloat(grid.GetEditor('Quantity').GetText()).toFixed(4);
        var gridPackingQty = '';

        //Rev Surojit 21-06-2019

        if (SpliteDetails.length == 32) {
            if (SpliteDetails[31] == "1") {
                IsInventory = 'Yes';
            }
            else {
                IsInventory = '';
            }
        }
        else {
            IsInventory = '';
        }

        //End of rev 21-06-2019


        if (SpliteDetails.length > 26 ) {
            //IsInventory = 'Yes';
                    
            type = 'edit';

            if (PurchaseOrderNum!="0" && PurchaseOrderNum!="" && $('#hdnPageStatus').val()!="EDIT") {
                if($("#hddnMultiUOMSelection").val()=="0")
                {
                    $.ajax({
                        type: "POST",
                        url: "Services/Master.asmx/GetMultiUOMDetails",
                        data: JSON.stringify({orderid: strProductID,action:'GetPurchaseGRNQtyByOrder',module:'PurchaseGRN',strKey : PurchaseOrderNum}),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (msg) {
                                
                            gridPackingQty = msg.d;
                            

                            if (ShowUOMConversionInEntry == "1" && IsInventory == 'Yes'  && SpliteDetails.length > 1) {
                              //  ShowUOM(type, "Purchase", 0, sProduct_quantity, sProduct_SaleUom, packing_quantity, packing_saleUOM, strProductID, slno, isOverideConvertion, gridprodqty, gridPackingQty);
                            }

                        }
                    });

                }
            }
            else{
                       
                //var orderid = grid.GetRowKey(globalRowIndex);
                var orderid =document.getElementById('Keyval_Id').value;
                if($("#hddnMultiUOMSelection").val()=="0")
                {
                    $.ajax({
                        type: "POST",
                        url: "Services/Master.asmx/GetMultiUOMDetails",
                        data: JSON.stringify({orderid: orderid,action:'GetPurchaseGRNQty',module:'PurchaseGRN',strKey :strProductID}),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (msg) {
                                
                            gridPackingQty = msg.d;

                            if (ShowUOMConversionInEntry == "1" && IsInventory == 'Yes'  && SpliteDetails.length > 1) {
                              //  ShowUOM(type, "Purchase", 0, sProduct_quantity, sProduct_SaleUom, packing_quantity, packing_saleUOM, strProductID, slno, isOverideConvertion, gridprodqty, gridPackingQty);
                            }

                        }
                    });
                }
            }
        }
        else{
            if($("#hddnMultiUOMSelection").val()=="0")
            {
                if (ShowUOMConversionInEntry == "1" && IsInventory == 'Yes'  && SpliteDetails.length > 1) {
                   // ShowUOM(type, "Purchase", 0, sProduct_quantity, sProduct_SaleUom, packing_quantity, packing_saleUOM, strProductID, slno, isOverideConvertion, gridprodqty, gridPackingQty);
                }
            }
        }

        //End of Rev 2.0 Subhra 11-03-2019
    }
    else {
        Pre_Quantity = "0";
        Pre_Amt = "0";
        Pre_TotalAmt = "0";
    }

    //chinmoy added for  for MultiUOM start
    if ($("#hddnMultiUOMSelection").val() == "1") {
        // grid.batchEditApi.StartEdit(globalRowIndex);
        // if ((gridquotationLookup.GetValue() != "") && (gridquotationLookup.GetValue() !=null)) {
        if (grid.GetEditor('Quantity').GetValue() != "0.0000") {
            // grid.batchEditApi.StartEdit(globalRowIndex);
            $("#UOMQuantity").val(grid.GetEditor('Quantity').GetValue());
        }
        // }
    }

    //End



}
function QuantityProductsGotFocus(s, e) {
    

    var ProductID = grid.GetEditor('ProductID').GetValue();

    if (ProductID != null) {
       
        Pre_Quantity = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
        Pre_Amt = (grid.GetEditor('TotalAmount').GetValue() != null) ? grid.GetEditor('TotalAmount').GetValue() : "0";
        Pre_TotalAmt = (grid.GetEditor('NetAmount').GetValue() != null) ? grid.GetEditor('NetAmount').GetValue() : "0";
        
        _GetQuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
        _GetPurchasePriceValue = (grid.GetEditor('PurchasePrice').GetValue() != null) ? grid.GetEditor('PurchasePrice').GetValue() : "0";
        _GetDiscountValue = (grid.GetEditor('Discount').GetValue() != null) ? grid.GetEditor('Discount').GetValue() : "0";
        _GetAmountValue = (grid.GetEditor('TotalAmount').GetValue() != null) ? grid.GetEditor('TotalAmount').GetValue() : "0";

       
        var ProductID = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
        var SpliteDetails = ProductID.split("||@||");
        var strProductID = SpliteDetails[0];
        var strProductName = SpliteDetails[1];
        var strDescription = SpliteDetails[1];
        var strUOM = SpliteDetails[2];
        var strStkUOM = SpliteDetails[4];
        var strSalePrice = SpliteDetails[6];
        var IsPackingActive = SpliteDetails[10];
        var Packing_Factor = SpliteDetails[11];
        var Packing_UOM = SpliteDetails[12];

        //debugger;

        //Rev 2.0 Subhra 11-03-2019     
        var isOverideConvertion = SpliteDetails[30];
        var packing_saleUOM = SpliteDetails[29];
        var sProduct_SaleUom = SpliteDetails[28];
        var sProduct_quantity = SpliteDetails[27];
        var packing_quantity = SpliteDetails[26];

        var slno= (grid.GetEditor('SrlNo').GetText() != null) ? grid.GetEditor('SrlNo').GetText() : "0";

        var PurchaseOrderNum= (grid.GetEditor('DocNumber').GetText() != null) ? grid.GetEditor('DocNumber').GetText() : "0";

        var ConvertionOverideVisible = $('#hdnConvertionOverideVisible').val();
        var ShowUOMConversionInEntry = $('#hdnShowUOMConversionInEntry').val();
        var type = 'add';
        var gridprodqty = parseFloat(grid.GetEditor('Quantity').GetText()).toFixed(4);
        var gridPackingQty = '';

        //Rev Surojit 21-06-2019

        if (SpliteDetails.length == 32) {
            if (SpliteDetails[31] == "1") {
                IsInventory = 'Yes';
            }
            else {
                IsInventory = '';
            }
        }
        else {
            IsInventory = '';
        }

        //End of rev 21-06-2019


        if (SpliteDetails.length > 26 ) {
            //IsInventory = 'Yes';
                    
            type = 'edit';

            if (PurchaseOrderNum!="0" && PurchaseOrderNum!="" && $('#hdnPageStatus').val()!="EDIT") {
                if($("#hddnMultiUOMSelection").val()=="0")
                {
                    $.ajax({
                        type: "POST",
                        url: "Services/Master.asmx/GetMultiUOMDetails",
                        data: JSON.stringify({orderid: strProductID,action:'GetPurchaseGRNQtyByOrder',module:'PurchaseGRN',strKey : PurchaseOrderNum}),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (msg) {
                                
                            gridPackingQty = msg.d;
                            

                            if (ShowUOMConversionInEntry == "1" && IsInventory == 'Yes'  && SpliteDetails.length > 1) {
                                ShowUOM(type, "Purchase", 0, sProduct_quantity, sProduct_SaleUom, packing_quantity, packing_saleUOM, strProductID, slno, isOverideConvertion, gridprodqty, gridPackingQty);
                            }

                        }
                    });

                }
            }
            else{
                       
                //var orderid = grid.GetRowKey(globalRowIndex);
                var orderid =document.getElementById('Keyval_Id').value;
                if($("#hddnMultiUOMSelection").val()=="0")
                {
                    $.ajax({
                        type: "POST",
                        url: "Services/Master.asmx/GetMultiUOMDetails",
                        data: JSON.stringify({orderid: orderid,action:'GetPurchaseGRNQty',module:'PurchaseGRN',strKey :strProductID}),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (msg) {
                                
                            gridPackingQty = msg.d;

                            if (ShowUOMConversionInEntry == "1" && IsInventory == 'Yes'  && SpliteDetails.length > 1) {
                                ShowUOM(type, "Purchase", 0, sProduct_quantity, sProduct_SaleUom, packing_quantity, packing_saleUOM, strProductID, slno, isOverideConvertion, gridprodqty, gridPackingQty);
                            }

                        }
                    });
                }
            }
        }
        else{
            if($("#hddnMultiUOMSelection").val()=="0")
            {
                if (ShowUOMConversionInEntry == "1" && IsInventory == 'Yes'  && SpliteDetails.length > 1) {
                    ShowUOM(type, "Purchase", 0, sProduct_quantity, sProduct_SaleUom, packing_quantity, packing_saleUOM, strProductID, slno, isOverideConvertion, gridprodqty, gridPackingQty);
                }
            }
        }

        //End of Rev 2.0 Subhra 11-03-2019
    }
    else {
        Pre_Quantity = "0";
        Pre_Amt = "0";
        Pre_TotalAmt = "0";
    }

    //chinmoy added for  for MultiUOM start
    if ($("#hddnMultiUOMSelection").val() == "1") {
        // grid.batchEditApi.StartEdit(globalRowIndex);
        // if ((gridquotationLookup.GetValue() != "") && (gridquotationLookup.GetValue() !=null)) {
        if (grid.GetEditor('Quantity').GetValue() != "0.0000") {
            // grid.batchEditApi.StartEdit(globalRowIndex);
            $("#UOMQuantity").val(grid.GetEditor('Quantity').GetValue());
        }
        // }
    }

    //End



}
//Rev Subhra 13-03-2019
var issavePacking = 0;
function SetDataToGrid(Quantity, packing, PackingUom, PackingSelectUom, productid, slno) {
    issavePacking = 1;
    grid.batchEditApi.StartEdit(globalRowIndex);
    grid.GetEditor('Quantity').SetValue(Quantity);
    
    ctxtQty.SetValue(Quantity);
    if($("#hdnShowUOMConversionInEntry").val()=="1")
    {
        ctxtAltQty.SetValue(packing);
        ccmbAltUOM.SetValue(PackingUom);
    }
    QuantityTextChange(globalRowIndex,7);
    
    //SetFoucs();
}
//End of Rev Subhra 13-03-2019
function SetFoucs() {
   
    setTimeout(function () {
        grid.batchEditApi.StartEdit(globalRowIndex, 7);
    }, 1200);
           
}

function UOMLostFocus(s,e)
{
    if($("#hddnMultiUOMSelection").val()=="0")
    {
        setTimeout(function () {
            grid.batchEditApi.StartEdit(globalRowIndex, 9);
        }, 600);
    }
    else
    {
        setTimeout(function () {
            grid.batchEditApi.StartEdit(globalRowIndex, 8);
        }, 600);
    }
}


function QuantityTextChange(s, e) {

    
    //chinmoy added for multiUom start
            
    // Rev Mantis Issue 24429  [ This checking not needed any more since when Multiple UOM is activated, Quantity cannot be enterd from Quantity column of main product grid]        
    //if (($("#hddnMultiUOMSelection").val() == "1")) {

    //    //setTimeout(function () {
    //    UomLenthCalculation();
    //    //  }, 200)
              
    //    grid.batchEditApi.StartEdit(globalRowIndex);
    //    var SLNo = grid.GetEditor('SrlNo').GetValue();
              
    //    if (Uomlength > 0) {
    //        var qnty = $("#UOMQuantity").val();
    //        var QValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0.0000";
    //        if (QValue != "0.0000" && QValue != qnty) {
    //            jConfirm('Qunatity Change Will Clear Multiple UOM Details.?', 'Confirmation Dialog', function (r) {
    //                if (r == true) {
    //                    grid.batchEditApi.StartEdit(globalRowIndex);
    //                    var tbqty = grid.GetEditor('Quantity');
    //                    //tbqty.SetValue(Quantity);

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
    //                    }
    //                else {
    //                    grid.batchEditApi.StartEdit(globalRowIndex);
    //                    grid.GetEditor('Quantity').SetValue(qnty);
    //                    setTimeout(function () {
    //                        grid.batchEditApi.StartEdit(globalRowIndex, 6);
    //                    }, 200);
    //                }


    //            });
    //        }
    //        else {
    //            grid.batchEditApi.StartEdit(globalRowIndex);
    //            grid.GetEditor('Quantity').SetValue(qnty);
                
    //            setTimeout(function () {
    //                grid.batchEditApi.StartEdit(globalRowIndex, 7);
    //            }, 600)
                   
    //                }
    //    }
                
    //}

    // End of Rev Mantis Issue 24429
    //End





    var ProductID = grid.GetEditor('ProductID').GetValue();
    var Quantity = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";

    if (ProductID != null) {
        if (parseFloat(Quantity) != parseFloat(Pre_Quantity)) {
            DiscountTextChange(s, e);
        }
    }
    else {
        jAlert('Select a product first.');
    }

    if($("#hdnShowUOMConversionInEntry").val()=="1")
    {
        setTimeout(function () { grid.batchEditApi.StartEdit(s, e); }, 200)
    }
   
}

function PurchasePriceTextFocus(s, e) {
    var PurchasePrice = (grid.GetEditor('PurchasePrice').GetValue() != null) ? grid.GetEditor('PurchasePrice').GetValue() : "0";
    _GetPurchasePriceValue = PurchasePrice;

    Pre_Quantity = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
    Pre_Amt = (grid.GetEditor('TotalAmount').GetValue() != null) ? grid.GetEditor('TotalAmount').GetValue() : "0";
    Pre_TotalAmt = (grid.GetEditor('NetAmount').GetValue() != null) ? grid.GetEditor('NetAmount').GetValue() : "0";
        
    _GetQuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
    _GetPurchasePriceValue = (grid.GetEditor('PurchasePrice').GetValue() != null) ? grid.GetEditor('PurchasePrice').GetValue() : "0";
    _GetDiscountValue = (grid.GetEditor('Discount').GetValue() != null) ? grid.GetEditor('Discount').GetValue() : "0";
    _GetAmountValue = (grid.GetEditor('TotalAmount').GetValue() != null) ? grid.GetEditor('TotalAmount').GetValue() : "0";
}

function EditPurchasePriceTextChange(s, e) {
    //debugger;
    var ProductID = grid.GetEditor('ProductID').GetValue();
    var PurchasePrice = (grid.GetEditor('PurchasePrice').GetValue() != null) ? grid.GetEditor('PurchasePrice').GetValue() : "0";    
    var editPurchasePrice= (grid.GetEditor('PurchasePriceValue').GetValue() != null) ? grid.GetEditor('PurchasePriceValue').GetValue() : "0";    
    var editPurchaseAmount=  (grid.GetEditor('PurchaseAmountValue').GetValue() != null) ? grid.GetEditor('PurchaseAmountValue').GetValue() : "0"; 
    if (ProductID != null) {
        if(parseFloat(editPurchasePrice) =="0" && parseFloat(editPurchaseAmount) =="0")
            {
            if (parseFloat(PurchasePrice) == "0") {
                jConfirm('Are you sure to make this Amount as Zero(0) as the charges will also become Zero(0)?', 'Confirmation Dialog', function (r) {
                    if (r == true) {
                        //debugger;
                        DiscountTextChange(s, e);
                        grid.batchEditApi.EndEdit();
                        grid.batchEditApi.StartEdit(globalRowIndex, 11);
                        //rev rajdip
                        grid.GetEditor('NetAmount').SetValue(0);
                        grid.GetEditor('TotalAmount').SetValue(0);
                        //end rev rajdip
                    }
                    else {                    
                        if (parseFloat(_GetPurchasePriceValue) != parseFloat(PurchasePrice)) {
                            //debugger;
                            grid.batchEditApi.StartEdit(globalRowIndex, 10);

                            var tbPurchasePrice = grid.GetEditor("PurchasePrice");
                            tbPurchasePrice.SetValue(_GetPurchasePriceValue);
                            DiscountTextChange(s, e);
                        }
                        grid.batchEditApi.StartEdit(globalRowIndex, 10);
                    }
                });
            }
            else {
                if (parseFloat(_GetPurchasePriceValue) != parseFloat(PurchasePrice)) {
                    DiscountTextChange(s, e);
                }
            }
        }
        else if(parseFloat(editPurchasePrice) != "0" && parseFloat(editPurchaseAmount) !="0" && parseFloat(editPurchasePrice) !=parseFloat(PurchasePrice))
        {
            grid.batchEditApi.StartEdit(globalRowIndex, 11);
            grid.GetEditor('PurchasePrice').SetValue(editPurchasePrice);
            DiscountTextChange(s, e);
            jAlert('None Zero Price are not changed.');
        }

    }
    else {
        jAlert('Select a product first.');
    }
}

function EditPurchasePriceTextFocus(s, e) {
    var PurchasePrice = (grid.GetEditor('PurchasePrice').GetValue() != null) ? grid.GetEditor('PurchasePrice').GetValue() : "0";
    _GetPurchasePriceValue = PurchasePrice;
    $("#EditPricevalue").val(PurchasePrice);
    $("#EditAmountvalue").val(grid.GetEditor('TotalAmount').GetValue());
    Pre_Quantity = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
    Pre_Amt = (grid.GetEditor('TotalAmount').GetValue() != null) ? grid.GetEditor('TotalAmount').GetValue() : "0";
    Pre_TotalAmt = (grid.GetEditor('NetAmount').GetValue() != null) ? grid.GetEditor('NetAmount').GetValue() : "0";
        
    _GetQuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
    _GetPurchasePriceValue = (grid.GetEditor('PurchasePrice').GetValue() != null) ? grid.GetEditor('PurchasePrice').GetValue() : "0";
    _GetDiscountValue = (grid.GetEditor('Discount').GetValue() != null) ? grid.GetEditor('Discount').GetValue() : "0";
    _GetAmountValue = (grid.GetEditor('TotalAmount').GetValue() != null) ? grid.GetEditor('TotalAmount').GetValue() : "0";
}

function PurchasePriceTextChange(s, e) {
    //debugger;
    var ProductID = grid.GetEditor('ProductID').GetValue();
    var PurchasePrice = (grid.GetEditor('PurchasePrice').GetValue() != null) ? grid.GetEditor('PurchasePrice').GetValue() : "0";    

    if (ProductID != null) {
        if (parseFloat(PurchasePrice) == "0") {
            jConfirm('Are you sure to make this Amount as Zero(0) as the charges will also become Zero(0)?', 'Confirmation Dialog', function (r) {
                if (r == true) {
                    //debugger;
                    DiscountTextChange(s, e);
                    grid.batchEditApi.EndEdit();
                    grid.batchEditApi.StartEdit(globalRowIndex, 11);
                    //rev rajdip
                    grid.GetEditor('NetAmount').SetValue(0);
                    grid.GetEditor('TotalAmount').SetValue(0);
                    //end rev rajdip
                }
                else {                    
                    if (parseFloat(_GetPurchasePriceValue) != parseFloat(PurchasePrice)) {
                        //debugger;
                        grid.batchEditApi.StartEdit(globalRowIndex, 10);

                        var tbPurchasePrice = grid.GetEditor("PurchasePrice");
                        tbPurchasePrice.SetValue(_GetPurchasePriceValue);
                        DiscountTextChange(s, e);
                    }
                    grid.batchEditApi.StartEdit(globalRowIndex, 10);
                }
            });
        }
        else {
            if (parseFloat(_GetPurchasePriceValue) != parseFloat(PurchasePrice)) {
                DiscountTextChange(s, e);
            }
        }
    }
    else {
        jAlert('Select a product first.');
    }
}

function DiscountTextFocus() {
    Pre_Quantity = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
    Pre_Amt = (grid.GetEditor('TotalAmount').GetValue() != null) ? grid.GetEditor('TotalAmount').GetValue() : "0";
    Pre_TotalAmt = (grid.GetEditor('NetAmount').GetValue() != null) ? grid.GetEditor('NetAmount').GetValue() : "0";
        
    _GetQuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
    _GetPurchasePriceValue = (grid.GetEditor('PurchasePrice').GetValue() != null) ? grid.GetEditor('PurchasePrice').GetValue() : "0";
    _GetDiscountValue = (grid.GetEditor('Discount').GetValue() != null) ? grid.GetEditor('Discount').GetValue() : "0";
    _GetAmountValue = (grid.GetEditor('TotalAmount').GetValue() != null) ? grid.GetEditor('TotalAmount').GetValue() : "0";
}
     
function DiscountValueChange(s, e) {
    var ProductID = grid.GetEditor('ProductID').GetValue();
    var Discount = (grid.GetEditor('Discount').GetValue() != null) ? grid.GetEditor('Discount').GetValue() : "0";

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
    var ProductID = grid.GetEditor('ProductID').GetValue();
    var SpliteDetails = ProductID.split("||@||");
    var Quantity = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
    var PurchasePrice = (grid.GetEditor('PurchasePrice').GetValue() != null) ? grid.GetEditor('PurchasePrice').GetValue() : "0";
    var Discount = (grid.GetEditor('Discount').GetValue() != null) ? grid.GetEditor('Discount').GetValue() : "0";
    var strRate = (ctxtRate.GetValue() != null && ctxtRate.GetValue() != "0") ? ctxtRate.GetValue() : "1";
    var Purchase_UOM_Factor = SpliteDetails[24];    
    var ConversionMultiplier = SpliteDetails[25];
    if (parseFloat(strRate) == 0) strRate = "1";

    if (ProductID != null) {
        var Amount = parseFloat((parseFloat(Quantity)*parseFloat(Purchase_UOM_Factor)*parseFloat(ConversionMultiplier)) * (parseFloat(PurchasePrice) / parseFloat(strRate))).toFixed(2);
        var amountAfterDiscount = parseFloat(parseFloat(Amount) - ((parseFloat(Discount) * parseFloat(Amount)) / 100)).toFixed(2);
        var tbAmount = grid.GetEditor("TotalAmount");
        var tbNetAmount = grid.GetEditor("NetAmount");
        tbAmount.SetValue(amountAfterDiscount);
        tbNetAmount.SetValue(amountAfterDiscount);
        var ShippingStateCode = $("#bsSCmbStateHF").val();
        var HSNCode = SpliteDetails[14];
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
        if (cddlPosGstChallan.GetValue()== "S") {
            CompareStateCode = GeteShippingStateID();
        }
        else {
            CompareStateCode = GetBillingStateID();
        }  
        //caluculateAndSetGST(grid.GetEditor("TotalAmount"), grid.GetEditor("TaxAmount"), grid.GetEditor("NetAmount"), HSNCode, Amount, amountAfterDiscount, TaxType, CompareStateCode, $('#ddl_Branch').val(), 'P');
        //caluculateAndSetGST(grid.GetEditor("TotalAmount"), grid.GetEditor("TaxAmount"), grid.GetEditor("NetAmount"), HSNCode, Amount, amountAfterDiscount, TaxType, CompareStateCode, $('#ddl_Branch').val(), 'PG');
        
        caluculateAndSetGST(grid.GetEditor("TotalAmount"), grid.GetEditor("TaxAmount"), grid.GetEditor("NetAmount"), HSNCode, Amount, amountAfterDiscount, TaxType, CompareStateCode, $('#ddl_Branch').val(),$("#hdnEntityType").val(), cPLQuoteDate.GetDate(), Quantity, 'PG');

        Cur_Quantity = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
        Cur_Amt = (grid.GetEditor('TotalAmount').GetValue() != null) ? grid.GetEditor('TotalAmount').GetValue() : "0";
        Cur_TotalAmt = (grid.GetEditor('NetAmount').GetValue() != null) ? grid.GetEditor('NetAmount').GetValue() : "0";
        CalculateAmount();
    }
    else {
        jAlert('Select a product first.');
    }
}

function AmountTextFocus(s, e) {
    Pre_Quantity = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
    Pre_Amt = (grid.GetEditor('TotalAmount').GetValue() != null) ? grid.GetEditor('TotalAmount').GetValue() : "0";
    Pre_TotalAmt = (grid.GetEditor('NetAmount').GetValue() != null) ? grid.GetEditor('NetAmount').GetValue() : "0";
        
    _GetQuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
    _GetPurchasePriceValue = (grid.GetEditor('PurchasePrice').GetValue() != null) ? grid.GetEditor('PurchasePrice').GetValue() : "0";
    _GetDiscountValue = (grid.GetEditor('Discount').GetValue() != null) ? grid.GetEditor('Discount').GetValue() : "0";
    _GetAmountValue = (grid.GetEditor('TotalAmount').GetValue() != null) ? grid.GetEditor('TotalAmount').GetValue() : "0";
}

function EditAmountTextChange(s, e) {
    ProductPriceCalculate();
    var Amount = (grid.GetEditor('TotalAmount').GetValue() != null) ? grid.GetEditor('TotalAmount').GetValue() : "0";
    var TaxAmount = (grid.GetEditor('TaxAmount').GetValue() != null) ? grid.GetEditor('TaxAmount').GetValue() : "0";
    var ProductID = grid.GetEditor('ProductID').GetValue();
    var SpliteDetails = ProductID.split("||@||");
    var edit_GetPurchasePriceValue= (grid.GetEditor('PurchasePriceValue').GetValue() != null) ? grid.GetEditor('PurchasePriceValue').GetValue() : "0";
    var edit_GetAmountValue = (grid.GetEditor('PurchaseAmountValue').GetValue() != null) ? grid.GetEditor('PurchaseAmountValue').GetValue() : "0";
    if(parseFloat(edit_GetPurchasePriceValue)=="0" && parseFloat(edit_GetAmountValue)=="0")
        {
        if (parseFloat(_GetAmountValue) != parseFloat(Amount)) {
            var Quantity = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
            var Purchase_UOM_Factor = SpliteDetails[24]; 
            var strRate = (ctxtRate.GetValue() != null && ctxtRate.GetValue() != "0") ? ctxtRate.GetValue() : "1";
            var PurchasePrice = (grid.GetEditor('PurchasePrice').GetValue() != null) ? grid.GetEditor('PurchasePrice').GetValue() : "0";
            var ConversionMultiplier = SpliteDetails[25];
            if (parseFloat(strRate) == 0) strRate = "1";
            var Discount = (grid.GetEditor('Discount').GetValue() != null) ? grid.GetEditor('Discount').GetValue() : "0";
            var Amount = parseFloat((parseFloat(Quantity)*parseFloat(Purchase_UOM_Factor)*parseFloat(ConversionMultiplier)) * (parseFloat(PurchasePrice) / parseFloat(strRate))).toFixed(2);
            var amountAfterDiscount = parseFloat(parseFloat(Amount) - ((parseFloat(Discount) * parseFloat(Amount)) / 100)).toFixed(2);

            var tbTotalAmount = grid.GetEditor("NetAmount");
            tbTotalAmount.SetValue(parseFloat(Amount) + parseFloat(TaxAmount));

            var ShippingStateCode = $("#bsSCmbStateHF").val();
            var HSNCode = SpliteDetails[14];
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
            if (cddlPosGstChallan.GetValue()== "S") {
                CompareStateCode = GeteShippingStateID();
            }
            else {
                CompareStateCode = GetBillingStateID();
            }

            //caluculateAndSetGST(grid.GetEditor("TotalAmount"), grid.GetEditor("TaxAmount"), grid.GetEditor("NetAmount"), HSNCode, Amount, Amount, TaxType, CompareStateCode, $('#ddl_Branch').val(), 'P');
            caluculateAndSetGST(grid.GetEditor("TotalAmount"), grid.GetEditor("TaxAmount"), grid.GetEditor("NetAmount"), HSNCode, Amount, amountAfterDiscount, TaxType, CompareStateCode, $('#ddl_Branch').val(),$("#hdnEntityType").val(), cPLQuoteDate.GetDate(), Quantity, 'PG');
            Cur_Quantity = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
            Cur_Amt = (grid.GetEditor('TotalAmount').GetValue() != null) ? grid.GetEditor('TotalAmount').GetValue() : "0";
            Cur_TotalAmt = (grid.GetEditor('NetAmount').GetValue() != null) ? grid.GetEditor('NetAmount').GetValue() : "0";
            CalculateAmount();
        }
    }
    else if(parseFloat(edit_GetPurchasePriceValue) != "0" && parseFloat(edit_GetAmountValue) !="0" && parseFloat(edit_GetAmountValue) !=parseFloat(Amount))
    {
        grid.GetEditor('TotalAmount').SetValue(edit_GetAmountValue);
        var Quantity = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
        var Purchase_UOM_Factor = SpliteDetails[24]; 
        var strRate = (ctxtRate.GetValue() != null && ctxtRate.GetValue() != "0") ? ctxtRate.GetValue() : "1";
        var PurchasePrice = (grid.GetEditor('PurchasePrice').GetValue() != null) ? grid.GetEditor('PurchasePrice').GetValue() : "0";
        var ConversionMultiplier = SpliteDetails[25];
        if (parseFloat(strRate) == 0) strRate = "1";

        var Discount = (grid.GetEditor('Discount').GetValue() != null) ? grid.GetEditor('Discount').GetValue() : "0";

        var Amount = parseFloat((parseFloat(Quantity)*parseFloat(Purchase_UOM_Factor)*parseFloat(ConversionMultiplier)) * (parseFloat(PurchasePrice) / parseFloat(strRate))).toFixed(2);
        var amountAfterDiscount = parseFloat(parseFloat(Amount) - ((parseFloat(Discount) * parseFloat(Amount)) / 100)).toFixed(2);

        var HSNCode = SpliteDetails[14];
        var TaxType = "";

        if (cddl_AmountAre.GetValue() == "1") {
            TaxType = "E";
        }
        else if (cddl_AmountAre.GetValue() == "2") {
            TaxType = "I";
        }
        var CompareStateCode;
        if (cddlPosGstChallan.GetValue()== "S") {
            CompareStateCode = GeteShippingStateID();
        }
        else {
            CompareStateCode = GetBillingStateID();
        }
        caluculateAndSetGST(grid.GetEditor("TotalAmount"), grid.GetEditor("TaxAmount"), grid.GetEditor("NetAmount"), HSNCode, Amount, amountAfterDiscount, TaxType, CompareStateCode, $('#ddl_Branch').val(),$("#hdnEntityType").val(), cPLQuoteDate.GetDate(), Quantity, 'PG');
        Cur_Quantity = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
        Cur_Amt = (grid.GetEditor('TotalAmount').GetValue() != null) ? grid.GetEditor('TotalAmount').GetValue() : "0";
        Cur_TotalAmt = (grid.GetEditor('NetAmount').GetValue() != null) ? grid.GetEditor('NetAmount').GetValue() : "0";
        CalculateAmount();
        
        jAlert('None Zero Amount are not changed.');
    }
}

function EditAmountTextFocus(s, e) {
    Pre_Quantity = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
    Pre_Amt = (grid.GetEditor('TotalAmount').GetValue() != null) ? grid.GetEditor('TotalAmount').GetValue() : "0";
    Pre_TotalAmt = (grid.GetEditor('NetAmount').GetValue() != null) ? grid.GetEditor('NetAmount').GetValue() : "0";
        
    _GetQuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
    _GetPurchasePriceValue = (grid.GetEditor('PurchasePrice').GetValue() != null) ? grid.GetEditor('PurchasePrice').GetValue() : "0";
    _GetDiscountValue = (grid.GetEditor('Discount').GetValue() != null) ? grid.GetEditor('Discount').GetValue() : "0";
    _GetAmountValue = (grid.GetEditor('TotalAmount').GetValue() != null) ? grid.GetEditor('TotalAmount').GetValue() : "0";

    $("#EditPricevalue").val(_GetPurchasePriceValue);
    $("#EditAmountvalue").val(_GetAmountValue);

}
function ProductPriceCalculate() {
    if ((grid.GetEditor('PurchasePrice').GetValue() == null || grid.GetEditor('PurchasePrice').GetValue() == 0) && (grid.GetEditor('Discount').GetValue() == null || grid.GetEditor('Discount').GetValue() == 0)) {
        var _purchaseprice = 0;
        var _Qty = grid.GetEditor('Quantity').GetValue();
        var _Amount = grid.GetEditor('TotalAmount').GetValue();
        _purchaseprice = (_Amount / _Qty);
        grid.GetEditor('PurchasePrice').SetValue(_purchaseprice);
    }
}
function NetAmountLostFocus(s,e)
{
    grid.batchEditApi.StartEdit(globalRowIndex, 15);
}
function AmountTextChange(s, e) {
    ProductPriceCalculate();
    var Amount = (grid.GetEditor('TotalAmount').GetValue() != null) ? grid.GetEditor('TotalAmount').GetValue() : "0";
    var TaxAmount = (grid.GetEditor('TaxAmount').GetValue() != null) ? grid.GetEditor('TaxAmount').GetValue() : "0";
    var ProductID = grid.GetEditor('ProductID').GetValue();
    var SpliteDetails = ProductID.split("||@||");

    if (parseFloat(_GetAmountValue) != parseFloat(Amount)) {
        var tbTotalAmount = grid.GetEditor("NetAmount");
        tbTotalAmount.SetValue(parseFloat(Amount) + parseFloat(TaxAmount));

        var ShippingStateCode = $("#bsSCmbStateHF").val();
        var HSNCode = SpliteDetails[14];
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
        if (cddlPosGstChallan.GetValue()== "S") {
            CompareStateCode = GeteShippingStateID();
        }
        else {
            CompareStateCode = GetBillingStateID();
        }

        //caluculateAndSetGST(grid.GetEditor("TotalAmount"), grid.GetEditor("TaxAmount"), grid.GetEditor("NetAmount"), HSNCode, Amount, Amount, TaxType, CompareStateCode, $('#ddl_Branch').val(), 'P');
        caluculateAndSetGST(grid.GetEditor("TotalAmount"), grid.GetEditor("TaxAmount"), grid.GetEditor("NetAmount"), HSNCode, Amount, amountAfterDiscount, TaxType, CompareStateCode, $('#ddl_Branch').val(),$("#hdnEntityType").val(), cPLQuoteDate.GetDate(), Quantity, 'PG');
        Cur_Quantity = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
        Cur_Amt = (grid.GetEditor('TotalAmount').GetValue() != null) ? grid.GetEditor('TotalAmount').GetValue() : "0";
        Cur_TotalAmt = (grid.GetEditor('NetAmount').GetValue() != null) ? grid.GetEditor('NetAmount').GetValue() : "0";
        CalculateAmount();
    }
}

function OnAddNewClick() {
    if (ctaggingList.GetValue() == null) {
        grid.AddNewRow();
        var noofvisiblerows = grid.GetVisibleRowsOnPage();

        var tbSrl = grid.GetEditor("SrlNo");
        var tbRow = grid.GetEditor("RowNo");

        tbSrl.SetValue(noofvisiblerows);
        tbRow.SetValue(noofvisiblerows);
        // Mantis Issue 24429
        $("#UOMQuantity").val(0);
        Uomlength= 0 ;
        // End of Mantis Issue 24429
    }
    else {
        QuotationNumberChanged();
    }
}


    function TaxAmountFocus(s, e) {
        rowEditCtrl = s;
    }

function TaxAmountKeyDown(s, e) {
    if (e.htmlEvent.key == "Enter" || e.htmlEvent.key== "NumpadEnter") {
        s.OnButtonClick(0);
    }
}

function TaxAmountClick(s, e) {
    //debugger;
    if (e.buttonIndex == 0) {
        if (cddl_AmountAre.GetValue() != null) {
            var IDs = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
            var SpliteDetails = IDs.split("||@||");
            var ProductID = SpliteDetails[0];

            if (ProductID.trim() != "") {
                Pre_Quantity = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
                Pre_Amt = (grid.GetEditor('TotalAmount').GetValue() != null) ? grid.GetEditor('TotalAmount').GetValue() : "0";
                Pre_TotalAmt = (grid.GetEditor('NetAmount').GetValue() != null) ? grid.GetEditor('NetAmount').GetValue() : "0";

                document.getElementById('setCurrentProdCode').value = ProductID;
                document.getElementById('HdSerialNo').value = grid.GetEditor('SrlNo').GetText();

                ctxtTaxTotAmt.SetValue(0);
                ccmbGstCstVat.SetSelectedIndex(0);
                $('.RecalculateInline').hide();
                caspxTaxpopUp.Show();
                //caspxTaxpopUp.SetWidth(window.screen.width - 200);
                //caspxTaxpopUp.popupHorizontalAlign = "WindowCenter";

                //Set Product Gross Amount and Net Amount

                var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
                var strSalePrice = (grid.GetEditor('PurchasePrice').GetValue() != null) ? grid.GetEditor('PurchasePrice').GetValue() : "";
                var Discount = (grid.GetEditor('Discount').GetValue() != null) ? grid.GetEditor('Discount').GetValue() : "0";
                var strRate = (ctxtRate.GetValue() != null && ctxtRate.GetValue() != "0") ? ctxtRate.GetValue() : "1";
                var Purchase_UOM_Factor = SpliteDetails[24];
                var ConversionMultiplier = SpliteDetails[25];
                if (strRate == 0) strRate = 1;

                var StockQuantity = parseFloat(QuantityValue)*parseFloat(Purchase_UOM_Factor);
                var Amount = parseFloat((parseFloat(QuantityValue)*parseFloat(Purchase_UOM_Factor)*parseFloat(ConversionMultiplier)) * (strSalePrice / strRate)).toFixed(2);
                var amountAfterDiscount =(grid.GetEditor('TotalAmount').GetValue() != null) ? grid.GetEditor('TotalAmount').GetValue() : "0";
                //var amountAfterDiscount = (parseFloat(Amount) - ((parseFloat(Discount) * parseFloat(Amount)) / 100)).toFixed(2);
                document.getElementById('hdnQty').value =QuantityValue;
                clblTaxProdGrossAmt.SetText(Amount);
                clblProdNetAmt.SetText(amountAfterDiscount);

                document.getElementById('HdProdGrossAmt').value = Amount;
                document.getElementById('HdProdNetAmt').value = amountAfterDiscount;

                //End Here


                //Set Discount Here

                if (parseFloat(grid.GetEditor('Discount').GetValue()) > 0) {
                    var discount = (parseFloat(Amount * grid.GetEditor('Discount').GetValue() / 100)).toFixed(2);
                    clblTaxDiscount.SetText(discount);
                }
                else {
                    clblTaxDiscount.SetText('0.00');
                }

                //End Here 


                //Checking is gstcstvat will be hidden or not

                if (cddl_AmountAre.GetValue() == "2") {
                    $('.GstCstvatClass').hide();
                    clblTaxableGross.SetText("(Taxable)");
                    clblTaxableNet.SetText("(Taxable)");

                    $('.gstGrossAmount').hide();
                    $('.gstNetAmount').hide();
                    clblTaxableGross.SetText("");
                    clblTaxableNet.SetText("");
                }
                else if (cddl_AmountAre.GetValue() == "1") {
                    $('.GstCstvatClass').show();
                    $('.gstGrossAmount').hide();
                    $('.gstNetAmount').hide();
                    clblTaxableGross.SetText("");
                    clblTaxableNet.SetText("");

                    //Get Customer Shipping StateCode
                    var shippingStCode = '';
                    //shippingStCode = ctxtshippingState.GetText();
                    // shippingStCode = shippingStCode.substring(shippingStCode.lastIndexOf('(')).replace('(State Code:', '').replace(')', '').trim();
                            
                    if (cddlPosGstChallan.GetValue() == "S") {
                        shippingStCode = GeteShippingStateCode();
                    }
                    else {
                        shippingStCode = GetBillingStateCode();
                    }
                }

                //End here
                        
                var _SrlNo = document.getElementById('HdSerialNo').value;
                var _IsEntry="";
                if (TaxOfProduct.filter(function (e) { return e.SrlNo == _SrlNo; }).length > 0) {
                    _IsEntry=TaxOfProduct.find(o => o.SrlNo === _SrlNo).IsTaxEntry;
                }

                if(_IsEntry=="N"){
                    cgridTax.PerformCallback('New~' + cddl_AmountAre.GetValue());
                }
                else{
                    cgridTax.PerformCallback(grid.keys[globalRowIndex] + '~' + cddl_AmountAre.GetValue());
                }

                ctxtprodBasicAmt.SetValue(grid.GetEditor('TotalAmount').GetValue());
            } else {
                grid.batchEditApi.StartEdit(globalRowIndex, 15);
            }
        }
    }
}


    function ctaxUpdatePanelEndCall(s, e) {
        if (ctaxUpdatePanel.cpstock != null) {
            ctaxUpdatePanel.cpstock = null;
            grid.batchEditApi.StartEdit(globalRowIndex, 7);
            return false;
        }
    }
function chargeCmbtaxClick(s, e) {
    GlobalCurChargeTaxAmt = parseFloat(ctxtGstCstVatCharge.GetText());
    ChargegstcstvatGlobalName = s.GetText();
}
var gstcstvatGlobalName;
function CmbtaxClick(s, e) {
    GlobalCurTaxAmt = parseFloat(ctxtGstCstVat.GetText());
    gstcstvatGlobalName = s.GetText();
}
var taxAmountGlobal;
function taxAmountGotFocus(s, e) {
    taxAmountGlobal = parseFloat(s.GetValue());
}
var taxJson;
function cgridTax_EndCallBack(s, e) {
    //cgridTax.batchEditApi.StartEdit(0, 1);
    globalTaxRowIndex=e.visibleIndex;
    $('.cgridTaxClass').show();

    // cgridTax.StartEditRow(0);


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

                cgridTax.cpComboCode = null;
            }
        }
    }
    if(cgridTax.cpUpdated!=null && typeof(cgridTax.cpUpdated)!='undefined')
    {
        if (cgridTax.cpUpdated.split('~')[0] == 'ok') {
            ctxtTaxTotAmt.SetValue(Math.round(cgridTax.cpUpdated.split('~')[1]));
            var gridValue = parseFloat(cgridTax.cpUpdated.split('~')[1]);
            var ddValue = parseFloat(ctxtGstCstVat.GetValue());
            ctxtTaxTotAmt.SetValue(Math.round(gridValue + ddValue));
            cgridTax.cpUpdated = "";
        }
        else {
            var totAmt = ctxtTaxTotAmt.GetValue();
            cgridTax.CancelEdit();
            caspxTaxpopUp.Hide();
            grid.batchEditApi.StartEdit(globalRowIndex, 14);
            grid.GetEditor("TaxAmount").SetValue(totAmt);

            if (cddl_AmountAre.GetValue() == "2") {
                var totalNetAmount = parseFloat(totAmt) + parseFloat(grid.GetEditor("TotalAmount").GetValue());
                var totalRoundOffAmount = Math.round(totalNetAmount);

                grid.GetEditor("NetAmount").SetValue(totalRoundOffAmount);
                grid.GetEditor("TotalAmount").SetValue(DecimalRoundoff(parseFloat(grid.GetEditor("TotalAmount").GetValue()) + (totalRoundOffAmount - totalNetAmount), 2));
            }
            else {
                grid.GetEditor("NetAmount").SetValue(DecimalRoundoff(parseFloat(totAmt) + parseFloat(grid.GetEditor("TotalAmount").GetValue()), 2));
            }

            Cur_Quantity = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
            Cur_Amt = (grid.GetEditor('TotalAmount').GetValue() != null) ? grid.GetEditor('TotalAmount').GetValue() : "0";
            Cur_TotalAmt = (grid.GetEditor('NetAmount').GetValue() != null) ? grid.GetEditor('NetAmount').GetValue() : "0";
            CalculateAmount();
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
    RecalCulateTaxTotalAmountInline();
    setTimeout(function () {  cgridTax.batchEditApi.StartEdit(0,3); }, 600)
           
}
function txtPercentageLostFocus(s, e) {

    //var ProdAmt = parseFloat(ctxtprodBasicAmt.GetValue());
    var ProdAmt = parseFloat(cgridTax.GetEditor("calCulatedOn").GetValue());
    if (s.GetText().trim() != '') {

        if (!isNaN(parseFloat(ProdAmt * s.GetText()) / 100)) {
            //Checking Add or less
            var totLength = cgridTax.GetEditor("Taxes_Name").GetText().length;
            var taxNameWithSign = cgridTax.GetEditor("TaxField").GetText();
            var sign = cgridTax.GetEditor("Taxes_Name").GetText().substring(totLength - 3);
            if (sign == '(+)') {
                GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());
                var _TaxTotAmt=(parseFloat((ProdAmt * s.GetText())/100).toFixed(2));
                //cgridTax.GetEditor("Amount").SetValue(parseFloat(ProdAmt * s.GetText()) / 100);
                cgridTax.GetEditor("Amount").SetValue(_TaxTotAmt);
               // ctxtTaxTotAmt.SetValue(Math.round(parseFloat(ctxtTaxTotAmt.GetValue()) + (parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) - GlobalCurTaxAmt));
                ctxtTaxTotAmt.SetValue(Math.round(parseFloat(ctxtTaxTotAmt.GetValue()) + (_TaxTotAmt) - GlobalCurTaxAmt));
                GlobalCurTaxAmt = 0;
            }
            else {

                GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());
                var _TaxTotAmt=(parseFloat((ProdAmt * s.GetText())/100).toFixed(2))* -1;
                cgridTax.GetEditor("Amount").SetValue(_TaxTotAmt);
                //cgridTax.GetEditor("Amount").SetValue((parseFloat(ProdAmt * s.GetText()) / 100) * -1);
                ctxtTaxTotAmt.SetValue(Math.round(parseFloat(ctxtTaxTotAmt.GetValue()) + (_TaxTotAmt) - (GlobalCurTaxAmt * -1)));
                //ctxtTaxTotAmt.SetValue(Math.round(parseFloat(ctxtTaxTotAmt.GetValue()) + ((parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) * -1) - (GlobalCurTaxAmt * -1)));
                GlobalCurTaxAmt = 0;
            }
            
            //SetOtherTaxValueOnRespectiveRow(0, cgridTax.GetEditor("Amount").GetValue(), cgridTax.GetEditor("Taxes_Name").GetText().replace('(+)', '').replace('(-)', ''));
            SetOtherTaxValueOnRespectiveRow(0, cgridTax.GetEditor("Amount").GetValue(), cgridTax.GetEditor("Taxes_Name").GetText().replace('(+)', '').replace('(-)', ''),parseFloat(cgridTax.GetEditor("calCulatedOn").GetValue()),sign);
   
            //Call for Running Total
            SetRunningTotal();

        } else {
            s.SetText("");
        }
    }

    RecalCulateTaxTotalAmountInline();
    setTimeout(function () {
        cgridTax.batchEditApi.StartEdit(globalTaxRowIndex,4); 
    }, 600);
           
}

function taxAmountLostFocus(s, e) {
    //debugger;
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
var globalTaxRowIndex;
function GetTaxVisibleIndex(s, e) {
    globalTaxRowIndex = e.visibleIndex;
}


//function SetOtherTaxValueOnRespectiveRow(idx, amt, name) {
//    for (var i = 0; i < taxJson.length; i++) {
//        if (taxJson[i].applicableBy == name) {
//            cgridTax.batchEditApi.StartEdit(i, 3);
//            cgridTax.GetEditor('calCulatedOn').SetValue(amt);

//            var totLength = cgridTax.GetEditor("Taxes_Name").GetText().length;
//            var taxNameWithSign = cgridTax.GetEditor("TaxField").GetText();
//            var sign = cgridTax.GetEditor("Taxes_Name").GetText().substring(totLength - 3);
//            var ProdAmt = parseFloat(cgridTax.GetEditor("calCulatedOn").GetValue());
//            var s = cgridTax.GetEditor("TaxField");
//            if (sign == '(+)') {
//                GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());
//                cgridTax.GetEditor("Amount").SetValue(parseFloat(ProdAmt * s.GetText()) / 100);

//                ctxtTaxTotAmt.SetValue(Math.round(parseFloat(ctxtTaxTotAmt.GetValue()) + (parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) - GlobalCurTaxAmt));
//                GlobalCurTaxAmt = 0;
//            }
//            else {

//                GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());
//                cgridTax.GetEditor("Amount").SetValue((parseFloat(ProdAmt * s.GetText()) / 100) * -1);

//                ctxtTaxTotAmt.SetValue(Math.round(parseFloat(ctxtTaxTotAmt.GetValue()) + ((parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) * -1) - (GlobalCurTaxAmt * -1)));
//                GlobalCurTaxAmt = 0;
//            }




//        }
//    }
//    //return;
//    cgridTax.batchEditApi.EndEdit();

//}


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



function SetRunningTotal() {
    //

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
            //SetOtherTaxValueOnRespectiveRow(0, cgridTax.GetEditor("Amount").GetValue(), cgridTax.GetEditor("Taxes_Name").GetText().replace('(+)', '').replace('(-)', ''),ProdAmt,sign);
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


function closeMultiUOM(s, e) {
    e.cancel = false;
    // cPopup_MultiUOM.Hide();
}

function RecalCulateTaxTotalAmountInline() {
    var totalInlineTaxAmount = 0;
    if(taxJson !=undefined)
    {
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

        ctxtTaxTotAmt.SetValue(totalInlineTaxAmount);
    }
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
    setTimeout(function () { grid.batchEditApi.StartEdit(globalRowIndex, 14); }, 800)
            

    return false;
}

function Save_TaxesClick() {
    //debugger;
    grid.batchEditApi.EndEdit();
    var noofvisiblerows = grid.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
    var i, cnt = 1;
    var sumAmount = 0, sumTaxAmount = 0, sumDiscount = 0, sumNetAmount = 0, sumDiscountAmt = 0;

    cnt = 1;
    for (i = -1 ; cnt <= noofvisiblerows ; i--) {
        var Amount = (grid.batchEditApi.GetCellValue(i, 'TotalAmount') != null) ? (grid.batchEditApi.GetCellValue(i, 'TotalAmount')) : "0";
        var TaxAmount = (grid.batchEditApi.GetCellValue(i, 'TaxAmount') != null) ? (grid.batchEditApi.GetCellValue(i, 'TaxAmount')) : "0";
        var Discount = (grid.batchEditApi.GetCellValue(i, 'Discount') != null) ? (grid.batchEditApi.GetCellValue(i, 'Discount')) : "0";
        var NetAmount = (grid.batchEditApi.GetCellValue(i, 'NetAmount') != null) ? (grid.batchEditApi.GetCellValue(i, 'NetAmount')) : "0";
        var sumDiscountAmt = ((parseFloat(Discount) * parseFloat(Amount)) / 100);

        sumAmount = sumAmount + parseFloat(Amount);
        sumTaxAmount = sumTaxAmount + parseFloat(TaxAmount);
        sumDiscount = sumDiscount + parseFloat(sumDiscountAmt);
        sumNetAmount = sumNetAmount + parseFloat(NetAmount);

        cnt++;
    }

    //if (sumAmount == 0 && sumTaxAmount == 0 && Discount == 0) {
    cnt = 1;
    for (i = 0 ; cnt <= noofvisiblerows ; i++) {
        var Amount = (grid.batchEditApi.GetCellValue(i, 'TotalAmount') != null) ? (grid.batchEditApi.GetCellValue(i, 'TotalAmount')) : "0";
        var TaxAmount = (grid.batchEditApi.GetCellValue(i, 'TaxAmount') != null) ? (grid.batchEditApi.GetCellValue(i, 'TaxAmount')) : "0";
        var Discount = (grid.batchEditApi.GetCellValue(i, 'Discount') != null) ? (grid.batchEditApi.GetCellValue(i, 'Discount')) : "0";
        var NetAmount = (grid.batchEditApi.GetCellValue(i, 'NetAmount') != null) ? (grid.batchEditApi.GetCellValue(i, 'NetAmount')) : "0";
        var sumDiscountAmt = ((parseFloat(Discount) * parseFloat(Amount)) / 100);

        sumAmount = sumAmount + parseFloat(Amount);
        sumTaxAmount = sumTaxAmount + parseFloat(TaxAmount);
        sumDiscount = sumDiscount + parseFloat(sumDiscountAmt);
        sumNetAmount = sumNetAmount + parseFloat(NetAmount);

        cnt++;
    }
    //}

    ////Debjyoti 
    //document.getElementById('HdChargeProdAmt').value = sumAmount;
    //document.getElementById('HdChargeProdNetAmt').value = sumNetAmount;
    ////End Here


    document.getElementById('HdChargeProdAmt').value = sumAmount;
    document.getElementById('HdChargeProdNetAmt').value = sumNetAmount;

    ctxtProductAmount.SetValue(parseFloat(sumAmount).toFixed(2));
    ctxtProductTaxAmount.SetValue(parseFloat(sumTaxAmount).toFixed(2));
    ctxtProductDiscount.SetValue(parseFloat(sumDiscount).toFixed(2));
    ctxtProductNetAmount.SetValue(parseFloat(sumNetAmount).toFixed(2));
    clblChargesTaxableGross.SetText("");
    clblChargesTaxableNet.SetText("");

    //Checking is gstcstvat will be hidden or not
    if (cddl_AmountAre.GetValue() == "2") {

        $('.lblChargesGSTforGross').show();
        $('.lblChargesGSTforNet').show();


        $('.lblChargesGSTforGross').hide();
        $('.lblChargesGSTforNet').hide();

    }
    else if (cddl_AmountAre.GetValue() == "1") {
        $('.lblChargesGSTforGross').hide();
        $('.lblChargesGSTforNet').hide();

        //Debjyoti 09032017
        for (var cmbCount = 1; cmbCount < ccmbGstCstVatcharge.GetItemCount() ; cmbCount++) {
            if (ccmbGstCstVatcharge.GetItem(cmbCount).value.split('~')[5] == '19') {
                if (ccmbGstCstVatcharge.GetItem(cmbCount).value.split('~')[4] == 'I') {
                    ccmbGstCstVatcharge.RemoveItem(cmbCount);
                    cmbCount--;
                }
            } else {
                if (ccmbGstCstVatcharge.GetItem(cmbCount).value.split('~')[4] == 'S' || ccmbGstCstVatcharge.GetItem(cmbCount).value.split('~')[4] == 'C') {
                    ccmbGstCstVatcharge.RemoveItem(cmbCount);
                    cmbCount--;
                }
            }
        }

    }
    //End here


    //Set Total amount
    //ctxtTotalAmount.SetValue(parseFloat(ctxtQuoteTaxTotalAmt.GetValue()) + parseFloat(ctxtProductNetAmount.GetValue()));

    ctxtTotalAmount.SetValue(parseFloat($("#txt_TotalAmt").text()));


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

function PercentageTextChange(s, e) {
    //debugger;
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
        ctxtTotalAmount.SetValue(parseFloat(Amount) + parseFloat(ctxtQuoteTaxTotalAmt.GetValue()));  //////////This line should be change
        //ctxtTotalAmount.SetText(parseFloat(ctxtTotalAmount.GetValue()) - parseFloat(Sum) + GlobalTaxAmt);
        GlobalTaxAmt = 0;
    }

    SetOtherChargeTaxValueOnRespectiveRow(0, Sum, gridTax.GetEditor("TaxName").GetText());
    SetChargesRunningTotal();

    RecalCulateTaxTotalAmountCharges();
}

var taxAmountGlobalCharges;
function QuotationTaxAmountGotFocus(s, e) {
    taxAmountGlobalCharges = parseFloat(s.GetValue());
}
var chargejsonTax=[];
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

    if (gridTax.cpChargesAmt) {
        ctxt_Charges.SetValue(gridTax.cpChargesAmt);
        gridTax.cpChargesAmt = null;

        Pre_Quantity = "0";
        Pre_Amt = "0";
        Pre_TotalAmt = "0";
        Cur_Quantity = "0";
        Cur_Amt = "0";
        Cur_TotalAmt = "0";
        CalculateAmount();
    }

    //Set Total Charges And total Amount
    if (gridTax.cpTotalCharges) {
        if (gridTax.cpTotalCharges != "") {
            ctxtQuoteTaxTotalAmt.SetValue(gridTax.cpTotalCharges);

            //ctxtTotalAmount.SetValue(parseFloat(ctxtQuoteTaxTotalAmt.GetValue()) + parseFloat(ctxtProductNetAmount.GetValue()));
            gridTax.cpTotalCharges = null;
        }
    }

    SetChargesRunningTotal();
    ShowTaxPopUp("IN");
}
function QuotationTaxAmountTextChange(s, e) {
    //debugger;
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
        ctxtQuoteTaxTotalAmt.SetValue((DecimalRoundoff(parseFloat(ctxtQuoteTaxTotalAmt.GetValue()) + GlobalTaxAmt - taxAmountGlobalCharges,2)));
        ctxtTotalAmount.SetValue(DecimalRoundoff(parseFloat(ctxtProductNetAmount.GetValue()) + parseFloat(ctxtQuoteTaxTotalAmt.GetValue()),2));
        GlobalTaxAmt = 0;
        SetOtherChargeTaxValueOnRespectiveRow(0, s.GetValue(), gridTax.GetEditor("TaxName").GetText());
    }
    else {
        GlobalTaxAmt = parseFloat(gridTax.GetEditor("Amount").GetValue());
        //gridTax.GetEditor("Amount").SetValue(Sum);
        ctxtQuoteTaxTotalAmt.SetValue(DecimalRoundoff(parseFloat(ctxtQuoteTaxTotalAmt.GetValue()) - GlobalTaxAmt + taxAmountGlobalCharges,2));
        ctxtTotalAmount.SetValue(parseFloat(ctxtProductNetAmount.GetValue()) + parseFloat(ctxtQuoteTaxTotalAmt.GetValue()));
        GlobalTaxAmt = 0;
        SetOtherChargeTaxValueOnRespectiveRow(0, s.GetValue(), gridTax.GetEditor("TaxName").GetText());
    }

    RecalCulateTaxTotalAmountCharges();
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

                ctxtTaxTotAmt.SetValue(Math.round(parseFloat(ctxtTaxTotAmt.GetValue()) + (parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) - GlobalCurTaxAmt));
                GlobalCurTaxAmt = 0;
            }
            else {

                GlobalCurTaxAmt = parseFloat(gridTax.GetEditor("Amount").GetValue());
                gridTax.GetEditor("Amount").SetValue((parseFloat(ProdAmt * s.GetText()) / 100) * -1);

                ctxtTaxTotAmt.SetValue(Math.round(parseFloat(ctxtTaxTotAmt.GetValue()) + ((parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) * -1) - (GlobalCurTaxAmt * -1)));
                GlobalCurTaxAmt = 0;
            }




        }
    }
    //return;
    gridTax.batchEditApi.EndEdit();
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

    ctxtQuoteTaxTotalAmt.SetValue(totalTaxAmount);
    ctxtTotalAmount.SetValue(parseFloat(ctxtQuoteTaxTotalAmt.GetValue()) + parseFloat(ctxtProductNetAmount.GetValue()));
}

function Save_TaxClick() {
    if (gridTax.GetVisibleRowsOnPage() > 0) {
        gridTax.UpdateEdit();
    }
    else {
        gridTax.PerformCallback('SaveGst');
    }
    cPopup_Taxes.Hide();
}





function CalculateAmount() {
    var Quantity = (parseFloat((cTotalQty.GetValue()).toString())).toFixed(2);
    var Amount = (parseFloat((cTaxableAmtval.GetValue()).toString())).toFixed(2);
    var TotalAmount = (parseFloat((cInvValue.GetValue()).toString())).toFixed(2);
    var ChargesAmount = (ctxt_Charges.GetValue() != null) ? (parseFloat(ctxt_Charges.GetValue())).toFixed(2) : "0";

    var Calculate_Quantity = (parseFloat(Quantity) + parseFloat(Cur_Quantity) - parseFloat(Pre_Quantity)).toFixed(2);
    var Calculate_Amount = (parseFloat(Amount) + parseFloat(Cur_Amt) - parseFloat(Pre_Amt)).toFixed(2);
    var Calculate_TotalAmount = (parseFloat(TotalAmount) + parseFloat(Cur_TotalAmt) - parseFloat(Pre_TotalAmt)).toFixed(2);
    var Calculate_TaxAmount = (parseFloat(Calculate_TotalAmount) - parseFloat(Calculate_Amount)).toFixed(2);
    var Calculate_SumAmount = (parseFloat(Calculate_TotalAmount) + parseFloat(ChargesAmount)).toFixed(2);

    cTotalQty.SetValue(Calculate_Quantity);
    cTaxableAmtval.SetValue(Calculate_Amount);
    cTaxAmtval.SetValue(Calculate_TaxAmount);
    cOtherTaxAmtval.SetValue(ChargesAmount);
    cInvValue.SetValue(Calculate_TotalAmount);
    cTotalAmt.SetValue(Calculate_SumAmount);

    Pre_Quantity = (grid.GetEditor("Quantity").GetText() != null) ? grid.GetEditor("Quantity").GetText() : "0";
    Pre_Amt = (grid.GetEditor("TotalAmount").GetText() != null) ? grid.GetEditor("TotalAmount").GetText() : "0";
    Pre_TotalAmt = (grid.GetEditor("NetAmount").GetText() != null) ? grid.GetEditor("NetAmount").GetText() : "0";


}



    function OpenUdf(s, e) {
        if (document.getElementById('IsUdfpresent').value == '0') {
            jAlert("UDF not define.");
        }
        else {
            var keyVal = document.getElementById('Keyval_internalId').value;
            var url = '/OMS/management/Master/frm_BranchUdfPopUp.aspx?Type=SQO&&KeyVal_InternalID=' + keyVal;
            popup.SetContentUrl(url);
            popup.Show();

        }
        return true;
    }


function SetArrForUOM(){
    //Rev Subhra 16-09-2019
    issavePacking = 1;
    //End of Rev Subhra 16-09-2019
    if (aarr.length == 0) {
        for(var i = -500; i < 500;i++)
        {
            if(grid.GetRow(i) != null){
               
                var ProductID = (grid.batchEditApi.GetCellValue(i,'ProductID') != null) ? grid.batchEditApi.GetCellValue(i,'ProductID') : "0";
                if(ProductID!="0") {
                    var actionqty = '';
                    var PurchaseOrderNum = (grid.GetEditor('DocNumber').GetText() != null) ? grid.GetEditor('DocNumber').GetText() : "";
                    if($("#hdnPageStatus").val() == "EDIT"){

                                


                        var SpliteDetails = ProductID.split("||@||");
                        var strProductID = SpliteDetails[0];
                        var orderid = grid.GetRowKey(i);
                        var slnoget = grid.batchEditApi.GetCellValue(i,'SrlNo');
                        var Quantity = grid.batchEditApi.GetCellValue(i,'Quantity');
                        //Rev Subhra 16-09-2019
                        var challanid =document.getElementById('Keyval_Id').value;
                        orderid=challanid;
                        //End of Rev Subhra 16-09-2019
                        if(PurchaseOrderNum!=""){
                            actionqty = 'GetPurchaseGRNQtyByOrder';
                            orderid =strProductID;
                        }
                        else{
                            actionqty = 'GetPurchaseGRNQty';
                            PurchaseOrderNum = strProductID;
                        }

                        $.ajax({
                            type: "POST",
                            url: "Services/Master.asmx/GetMultiUOMDetails",
                            data: JSON.stringify({orderid: orderid,action:actionqty,module:'PurchaseGRN',strKey :PurchaseOrderNum}),
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            async: false,
                            success: function (msg) {
                               
                                gridPackingQty = msg.d;

                                if(msg.d != ""){
                                    var packing = SpliteDetails[26];
                                    var PackingUom = SpliteDetails[29];
                                    var PackingSelectUom = SpliteDetails[28];
                                    var arrobj = {};
                                    if(strProductID !="")
                                    {
                                        arrobj.productid = strProductID;
                                        arrobj.slno = slnoget;
                                        arrobj.Quantity = Quantity;
                                        arrobj.packing = gridPackingQty;
                                        arrobj.PackingUom = PackingUom;
                                        arrobj.PackingSelectUom = PackingSelectUom;

                                        aarr.push(arrobj);
                                    }
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


var canCallBack = true;
function AllControlInitilize() {
    if (canCallBack) {

        if($("#hdnPageStatus").val()=="EDIT" && $("#hdnShowUOMConversionInEntry").val()=="1")
        {
            SetEditModeForAltUOM();
        }


    }
}

function SetEditModeForAltUOM(){
  
    if (aarr.length == 0) {
        for(var i = -500; i < 500;i++)
        {
            if(grid.GetRow(i) != null){
               
                var ProductID = (grid.batchEditApi.GetCellValue(i,'ProductID') != null) ? grid.batchEditApi.GetCellValue(i,'ProductID') : "0";
                if(ProductID!="0") {
                    var actionqty = '';
                    var PurchaseOrderNum = (grid.GetEditor('DocNumber').GetText() != null) ? grid.GetEditor('DocNumber').GetText() : "";
                    if($("#hdnPageStatus").val() == "EDIT"){

                                


                        var SpliteDetails = ProductID.split("||@||");
                        var strProductID = SpliteDetails[0];
                        var orderid = grid.GetRowKey(i);
                        var slnoget = grid.batchEditApi.GetCellValue(i,'SrlNo');
                        var Quantity = grid.batchEditApi.GetCellValue(i,'Quantity');
                        //Rev Subhra 16-09-2019
                        var challanid =document.getElementById('Keyval_Id').value;
                        orderid=challanid;
                        //End of Rev Subhra 16-09-2019
                        if(PurchaseOrderNum!=""){
                            actionqty = 'GetPurchaseGRNQtyByOrder';
                            orderid =strProductID;
                        }
                        else{
                            actionqty = 'GetPurchaseGRNQty';
                            PurchaseOrderNum = strProductID;
                        }

                        $.ajax({
                            type: "POST",
                            url: "Services/Master.asmx/GetMultiUOMDetails",
                            data: JSON.stringify({orderid: orderid,action:actionqty,module:'PurchaseGRN',strKey :PurchaseOrderNum}),
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            async: false,
                            success: function (msg) {
                               
                                gridPackingQty = msg.d;

                                if(msg.d != ""){
                                    var packing = SpliteDetails[26];
                                    var PackingUom = SpliteDetails[29];
                                    var PackingSelectUom = SpliteDetails[28];
                                    var arrobj = {};
                                    if(strProductID !="")
                                    {
                                        arrobj.productid = strProductID;
                                        arrobj.slno = slnoget;
                                        arrobj.Quantity = Quantity;
                                        arrobj.packing = gridPackingQty;
                                        arrobj.PackingUom = PackingUom;
                                        arrobj.PackingSelectUom = PackingSelectUom;

                                        aarr.push(arrobj);
                                    }
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

function SaveNew_Click() {
    flag = true;
    LoadingPanel.Show();

    var ProjectCode = clookup_Project.GetText();
    if ($("#hdnProjectSelectInEntryModule").val() == "1" && $("#hdnProjectMandatory").val()=="1" && ProjectCode == "") {
        flag = false;
        LoadingPanel.Hide();
        jAlert("Please Select Project.");
        return false;
    }

    var txtPurchaseNo = $("#txtVoucherNo").val().trim();
    if (txtPurchaseNo == null || txtPurchaseNo == "") {
        LoadingPanel.Hide();
        $("#MandatoryBillNo").show();
        flag = false;
        return false;
    }
    else {
        $('#MandatoryBillNo').attr('style', 'display:none');
    }


    var customerId = GetObjectID('hdnCustomerId').value;
    if (customerId == '' || customerId == null) {
        LoadingPanel.Hide();
        $('#MandatorysCustomer').attr('style', 'display:block');
        flag = false;
        return false;
    }
    else {
        $('#MandatorysCustomer').attr('style', 'display:none');
    }

    var PartyInvoiceNo = ctxtPartyInvoice.GetValue();
    if (PartyInvoiceNo == '' || PartyInvoiceNo == null) {
        LoadingPanel.Hide();
        $('#MandatorysPartyinvno').attr('style', 'display:block');
        flag = false;
        return false;
    }
    else {
        $('#MandatorysPartyinvno').attr('style', 'display:none');
    }

    var EWayBilMendatory = document.getElementById('hdfEWayBillMendatory').value;// $('#hdfTagMendatory').val();
    var strEWayBillNumber=$("#txtEWayBillNumber").val();
    if(strEWayBillNumber=="")
    {
        if (EWayBilMendatory == 'Yes') {
            LoadingPanel.Hide();
            $("#MandatoryEWayBillNumber").show();
            return false;
        }
         
    }
           


    var RowCount = 0;
    var IsProduct = "";
    for (var i = 0; i <= grid.GetVisibleRowsOnPage() ; i++) {
        var ProductID = (grid.batchEditApi.GetCellValue(RowCount, 'ProductName') != null) ? (grid.batchEditApi.GetCellValue(RowCount, 'ProductName')) : "";

        if (ProductID != "") {
            IsProduct = "Y";
            break;
        }
        RowCount++;
    }

    if (flag != false) {
        //if($("#hddnMultiUOMSelection").val()=="0")
        //    {
        SetArrForUOM(); //Surojit For UOM EDIT

        if (IsProduct == "Y") {
            //Subhra 13-03-2019
            if (issavePacking == 1 && aarr.length > 0) {
                $.ajax({
                    type: "POST",
                    url: "PurchaseChallan.aspx/SetSessionPacking",
                    data: "{'list':'" + JSON.stringify(aarr) + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        $('#hdfIsDelete').val('I');
                        $('#hdnRefreshType').val('N');
                        $('#hfControlData').val($('#hfControlSaveData').val());

                        var JsonProductList = JSON.stringify(TaxOfProduct);
                        GetObjectID('hdnJsonProductTax').value = JsonProductList;

                        var JsonProductStock = JSON.stringify(StockOfProduct);
                        GetObjectID('hdnJsonProductStock').value = JsonProductStock;                    

                        grid.AddNewRow();
                        grid.UpdateEdit();
                    }
                });
            }
            else
            {
                //Subhra 13-03-2019
                $('#hdfIsDelete').val('I');
                $('#hdnRefreshType').val('N');
                $('#hfControlData').val($('#hfControlSaveData').val());

                var JsonProductList = JSON.stringify(TaxOfProduct);
                GetObjectID('hdnJsonProductTax').value = JsonProductList;

                var JsonProductStock = JSON.stringify(StockOfProduct);
                GetObjectID('hdnJsonProductStock').value = JsonProductStock;                    

                grid.AddNewRow();
                grid.UpdateEdit();
            }

                    
        }
        else {
            LoadingPanel.Hide();
            jAlert('Please add atleast single record first');
        }
    }
    //    else
    //    {
    //        LoadingPanel.Hide();
    //    }
    //}
}

function SaveExit_Click() {
    flag = true;
    LoadingPanel.Show();

    var ProjectCode = clookup_Project.GetText();
    if ($("#hdnProjectSelectInEntryModule").val() == "1" && $("#hdnProjectMandatory").val()=="1" && ProjectCode == "") {
        flag = false;
        LoadingPanel.Hide();
        jAlert("Please Select Project.");
        return false;
    }

    var txtPurchaseNo = $("#txtVoucherNo").val().trim();
    if (txtPurchaseNo == null || txtPurchaseNo == "") {
        LoadingPanel.Hide();
        $("#MandatoryBillNo").show();
        flag = false;
        return false;
    }
    else {
        $('#MandatoryBillNo').attr('style', 'display:none');
    }

    var customerId = GetObjectID('hdnCustomerId').value;
    if (customerId == '' || customerId == null) {
        LoadingPanel.Hide();
        $('#MandatorysCustomer').show();
        flag = false;
        return false;
    }
    else {
        $('#MandatorysCustomer').attr('style', 'display:none');
    }

    var PartyInvoiceNo = ctxtPartyInvoice.GetValue();
    if (PartyInvoiceNo == '' || PartyInvoiceNo == null) {
        LoadingPanel.Hide();
        $('#MandatorysPartyinvno').show();
        flag = false;
        return false;
    }
    else {
        $('#MandatorysPartyinvno').attr('style', 'display:none');
    }
    var EWayBilMendatory = document.getElementById('hdfEWayBillMendatory').value;// $('#hdfTagMendatory').val();
    var strEWayBillNumber=$("#txtEWayBillNumber").val();
    if(strEWayBillNumber=="")
    {
        if (EWayBilMendatory == 'Yes') {
            LoadingPanel.Hide();
            $("#MandatoryEWayBillNumber").show();
            return false;
        }
    }

    var RowCount = 0;
    var IsProduct = "";
    for (var i = 0; i <= grid.GetVisibleRowsOnPage() ; i++) {
        var ProductID = (grid.batchEditApi.GetCellValue(RowCount, 'ProductName') != null) ? (grid.batchEditApi.GetCellValue(RowCount, 'ProductName')) : "";

        if (ProductID != "") {
            IsProduct = "Y";
            break;
        }
        RowCount++;
    }

    if (flag != false) {
        //if($("#hddnMultiUOMSelection").val()=="0")
        //    {
        SetArrForUOM(); //Surojit For UOM EDIT
               
        if (IsProduct == "Y") {

            if (issavePacking == 1 && aarr.length > 0) {
                $.ajax({
                    type: "POST",
                    url: "PurchaseChallan.aspx/SetSessionPacking",
                    data: "{'list':'" + JSON.stringify(aarr) + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        $('#hdfIsDelete').val('I');
                        $('#hdnRefreshType').val('E');
                        $('#hfControlData').val($('#hfControlSaveData').val());

                        var JsonProductList = JSON.stringify(TaxOfProduct);
                        GetObjectID('hdnJsonProductTax').value = JsonProductList;
                    
                        var JsonProductStock = JSON.stringify(StockOfProduct);
                        GetObjectID('hdnJsonProductStock').value = JsonProductStock;                    
                        SaveSendUOM('PC');
                        grid.AddNewRow();
                        grid.UpdateEdit();
                    }
                });
            }
            else
            {
                $('#hdfIsDelete').val('I');
                $('#hdnRefreshType').val('E');
                $('#hfControlData').val($('#hfControlSaveData').val());

                var JsonProductList = JSON.stringify(TaxOfProduct);
                GetObjectID('hdnJsonProductTax').value = JsonProductList;
                    
                var JsonProductStock = JSON.stringify(StockOfProduct);
                GetObjectID('hdnJsonProductStock').value = JsonProductStock;                    
                SaveSendUOM('PC');
                grid.AddNewRow();
                grid.UpdateEdit();
            }
        }
        else {
            LoadingPanel.Hide();
            jAlert('Please add atleast single record first');
        }
    }
    //      else
    //      {
    //          LoadingPanel.Hide();

    //      }

    //}
}


    var IsFocus = "0";

function WarehousePanelEndCall(s, e) {
    if (cWarehousePanel.cperrorMsg == "duplicateSerial") {
        cWarehousePanel.cperrorMsg = null;
        jAlert("Duplicate Serial. Cannot Proceed.");
    }
    else if (cWarehousePanel.cpIsSave == "Y") {
        cWarehousePanel.cpIsSave = null;
        cWarehousePanel.cpIsShow = null;
        cPopup_Warehouse.Hide();
        grid.batchEditApi.StartEdit(Warehouseindex, 9);
    }
    else if (cWarehousePanel.cpIsSave == "N") {
        cWarehousePanel.cpIsSave = null;
        jAlert('Purchase Quantity must be equal to Warehouse Quantity.');
    }
    else if (cWarehousePanel.cpduplicateMsg == "_duplicateSerial") {
        var list = cWarehousePanel.cpduplicateSerial;

        cWarehousePanel.cpduplicateMsg = null;
        cWarehousePanel.cpduplicateSerial = null;

        jAlert("Duplicate Serial are : " + list);
    }


    if (cWarehousePanel.cpIsShow == "Y") {
        cWarehousePanel.cpIsShow = null;
        var Warehousetype = document.getElementById("hdfWarehousetype").value;

        if (Warehousetype == "W") {
            div_Warehouse.style.display = 'block';
            div_Batch.style.display = 'none';
            div_Manufacture.style.display = 'none';
            div_Expiry.style.display = 'none';
            div_Quantity.style.display = 'block';
            div_Serial.style.display = 'none';
            div_Upload.style.display = 'none';
            div_Break.style.display = 'none';
            cPopup_Warehouse.Show();
        }
        else if (Warehousetype == "B") {
            div_Warehouse.style.display = 'none';
            div_Batch.style.display = 'block';
            div_Manufacture.style.display = 'block';
            div_Expiry.style.display = 'block';
            div_Quantity.style.display = 'block';
            div_Serial.style.display = 'none';
            div_Upload.style.display = 'none';
            div_Break.style.display = 'none';
            cPopup_Warehouse.Show();
        }
        else if (Warehousetype == "S") {
            div_Warehouse.style.display = 'none';
            div_Batch.style.display = 'none';
            div_Manufacture.style.display = 'none';
            div_Expiry.style.display = 'none';
            div_Quantity.style.display = 'none';
            div_Serial.style.display = 'block';
            div_Upload.style.display = 'block';
            div_Break.style.display = 'none';
            cPopup_Warehouse.Show();
        }
        else if (Warehousetype == "WB") {
            div_Warehouse.style.display = 'block';
            div_Batch.style.display = 'block';
            div_Manufacture.style.display = 'block';
            div_Expiry.style.display = 'block';
            div_Quantity.style.display = 'block';
            div_Serial.style.display = 'none';
            div_Upload.style.display = 'none';
            div_Break.style.display = 'none';
            cPopup_Warehouse.Show();
        }
        else if (Warehousetype == "WS") {
            div_Warehouse.style.display = 'block';
            div_Batch.style.display = 'none';
            div_Manufacture.style.display = 'none';
            div_Expiry.style.display = 'none';
            div_Quantity.style.display = 'none';
            div_Serial.style.display = 'block';
            div_Upload.style.display = 'block';
            div_Break.style.display = 'none';
            cPopup_Warehouse.Show();
        }
        else if (Warehousetype == "WBS") {
            div_Warehouse.style.display = 'block';
            div_Batch.style.display = 'block';
            div_Manufacture.style.display = 'block';
            div_Expiry.style.display = 'block';
            div_Quantity.style.display = 'none';
            div_Serial.style.display = 'block';
            div_Upload.style.display = 'block';
            div_Break.style.display = 'block';
            cPopup_Warehouse.Show();
        }
        else if (Warehousetype == "BS") {
            div_Warehouse.style.display = 'none';
            div_Batch.style.display = 'block';
            div_Manufacture.style.display = 'block';
            div_Expiry.style.display = 'block';
            div_Quantity.style.display = 'none';
            div_Serial.style.display = 'block';
            div_Upload.style.display = 'block';
            div_Break.style.display = 'none';
            cPopup_Warehouse.Show();
        }
        else {
            div_Warehouse.style.display = 'none';
            div_Batch.style.display = 'none';
            div_Manufacture.style.display = 'none';
            div_Expiry.style.display = 'none';
            div_Quantity.style.display = 'none';
            div_Serial.style.display = 'none';
            div_Upload.style.display = 'none';
        }
    }

    if (IsFocus == "1") {
        ctxtserialID.Focus();
        IsFocus = "0";
    }
    //Rev Subhra 15-05-2019
    grid.batchEditApi.StartEdit(globalRowIndex, 9);
    //End of Rev Subhra 15-05-2019
}

function closeWarehouse(s, e) {
    e.cancel = false;
    cWarehousePanel.PerformCallback('WarehouseDelete');
}

function closeStockPopup(s, e) {
    e.cancel = false;
    grid.batchEditApi.StartEdit(globalRowIndex, 10);
}

function FullnFinalSave(){
    cPopupWarehouse.Hide();
    grid.batchEditApi.StartEdit(globalRowIndex, 10);
}

function ClearWarehouse() {
    Stock_EditID = "0";

    ctxtQuantity.SetValue("0");
    ctxtBatchName.SetValue("");
    ctxtStartDate.SetDate(null);
    ctxtEndDate.SetDate(null);
    ctxtserialID.SetValue("");
}

function SubmitWarehouse() {
    var WarehouseID = (cCmbWarehouseID.GetValue() != null) ? cCmbWarehouseID.GetValue() : "0";
    var WarehouseName = (cCmbWarehouseID.GetText() != null) ? cCmbWarehouseID.GetText() : "";
    var BatchName = (ctxtBatchName.GetValue() != null) ? ctxtBatchName.GetValue() : "";
    var MfgDate = (ctxtStartDate.GetValue() != null) ? ctxtStartDate.GetValue() : "";
    var ExpiryDate = (ctxtEndDate.GetValue() != null) ? ctxtEndDate.GetValue() : "";
    var SerialNo = (ctxtserialID.GetValue() != null) ? ctxtserialID.GetValue() : "";
    var Qty = parseFloat(ctxtQuantity.GetValue());
    var altQty=ctxtAltQuantity.GetValue();

    MfgDate = GetDateFormat(MfgDate);
    ExpiryDate = GetDateFormat(ExpiryDate);

    $("#spnCmbWarehouse").hide();
    $("#spntxtBatch").hide();
    $("#spntxtQuantity").hide();
    $("#spntxtserialID").hide();

    var Ptype = document.getElementById('hdfWarehousetype').value;
    if ((Ptype == "W" && WarehouseID == "0") || (Ptype == "WB" && WarehouseID == "0") || (Ptype == "WS" && WarehouseID == "0") || (Ptype == "WBS" && WarehouseID == "0")) {
        $("#spnCmbWarehouse").show();
    }
    else if ((Ptype == "B" && BatchName == "") || (Ptype == "WB" && BatchName == "") || (Ptype == "WBS" && BatchName == "") || (Ptype == "BS" && BatchName == "")) {
        $("#spntxtBatch").show();
    }
    else if ((Ptype == "W" && Qty == "0") || (Ptype == "B" && Qty == "0") || (Ptype == "WB" && Qty == "0")) {
        $("#spntxtQuantity").show();
    }
    else if ((Ptype == "S" && SerialNo == "") || (Ptype == "WS" && SerialNo == "") || (Ptype == "WBS" && SerialNo == "") || (Ptype == "BS" && SerialNo == "")) {
        $("#spntxtserialID").show();
    }
    else {
        if ((Ptype == "S" && Stock_EditID == "0") || (Ptype == "WS" && Stock_EditID == "0") || (Ptype == "WBS" && Stock_EditID == "0") || (Ptype == "BS" && Stock_EditID == "0")) {
            ctxtserialID.SetValue("");
            ctxtserialID.Focus();
            IsFocus = "1";
        }
        else {
            ctxtQuantity.SetValue("0");
            ctxtBatchName.SetValue("");
            ctxtStartDate.SetDate(null);
            ctxtEndDate.SetDate(null);
            ctxtserialID.SetValue("");
        }

        cWarehousePanel.PerformCallback('StockSave~' + WarehouseID + '~' + WarehouseName + '~' + BatchName + '~' + MfgDate + '~' + ExpiryDate + '~' + SerialNo + '~' + Qty + '~' + Stock_EditID+ '~' + altQty);
        Stock_EditID = "0";
    }
}

function FinalWarehouse() {
    cWarehousePanel.PerformCallback('WarehouseFinal');
    //Rev Subhra 15-05-2019
    grid.batchEditApi.StartEdit(globalRowIndex, 9);
    //End of Rev Subhra 15-05-2019
}

function fn_Delete(keyValue) {
    cWarehousePanel.PerformCallback('Delete~' + keyValue);
}

function fn_Edit(keyValue) {
    SelectedWarehouseID = keyValue;

    ctxtQuantity.SetValue("0");
    ctxtBatchName.SetValue("");
    ctxtStartDate.SetDate(null);
    ctxtEndDate.SetDate(null);
    ctxtserialID.SetValue("");

    cWarehousePanel.PerformCallback('EditWarehouse~' + keyValue);
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

function GetPCDateFormat(today) {
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
        today = yyyy + '-' + mm + '-' + dd;
    }

    return today;
}

function GetReverseDateFormat(today) {
    if (today != "") {
        var dd = today.substring(0, 2);
        var mm = today.substring(3, 5);
        var yyyy = today.substring(6, 10);

        today = mm + '-' + dd + '-' + yyyy;
    }

    return today;
}


    document.onkeydown = function (e) {
        if (event.keyCode == 83 && event.altKey == true && getUrlVars().req != "V") { //run code for Alt+S -- ie, Save & New  
            StopDefaultAction(e);
            document.getElementById('btn_SaveRecords').click();
        }
        else if (event.keyCode == 88 && event.altKey == true && getUrlVars().req != "V") { //run code for Alt+X -- ie, Save & Exit!     
            StopDefaultAction(e);
            document.getElementById('btn_SaveRecordsExit').click();
        }
        else if (event.keyCode == 75 && event.altKey == true) { //run code for Alt+K -- ie, Press OK on Billing & Shipping!   
            // StopDefaultAction(e);
            FullnFinalSave();
                
        }
        else if (event.keyCode == 79 && event.altKey == true) { //run code for Alt+K -- ie, Press OK on Billing & Shipping!   
              
            BatchUpdate();
        }
    }

function StopDefaultAction(e) {
    if (e.preventDefault) { e.preventDefault() }
    else { e.stop() };

    e.returnValue = false;
    e.stopPropagation();
}


    function VendorButnClick(s, e) {
        document.getElementById("txtCustSearch").value = "";
        var txt = "<table border='1' width=\"100%\" class=\"dynamicPopupTbl\"><tr class=\"HeaderStyle\"><th>Unique Id</th><th>Vendor Name</th></tr><table>";
        document.getElementById("CustomerTable").innerHTML = txt;

        setTimeout(function () { $("#txtCustSearch").focus(); }, 500);

        $('#txtCustSearch').val('');
        $('#CustModel').modal('show');
    }

function VendorKeyDown(s, e) {
    if (e.htmlEvent.key == "Enter" || e.htmlEvent.key== "NumpadEnter") {
        s.OnButtonClick(0);
    }
}

function Customerkeydown(e) {
    var OtherDetails = {}
    OtherDetails.SearchKey = $("#txtCustSearch").val();
    OtherDetails.BranchID = $('#ddl_Branch').val();

    if (e.code == "Enter" || e.code == "NumpadEnter") {
        var HeaderCaption = [];
        HeaderCaption.push("Unique Id");
        HeaderCaption.push("Vendor Name");

        if ($("#txtCustSearch").val() != '') {
            callonServer("Services/Master.asmx/GetVendorWithBranchPC", OtherDetails, "CustomerTable", HeaderCaption, "customerIndex", "SetCustomer");
        }
    }
    else if (e.code == "ArrowDown") {
        if ($("input[customerindex=0]"))
            $("input[customerindex=0]").focus();
    }
}

function SetCustomer(Id, Name) {
    var VendorID = Id;
    if (VendorID != "") {
        $('#CustModel').modal('hide');
        ctxtCustName.SetText(Name);
        //Chinmoy comment below line
        //LoadCustomerAddress(VendorID, $('#ddl_Branch').val(), 'PC');
        SetEntityType(VendorID);

        GetObjectID('hdnCustomerId').value = VendorID;
        //chinmoy added this line
        cddl_AmountAre.SetEnabled(true);
        PopulateGSTCSTVAT();
        GetVendorGSTInFromBillShip(GetObjectID('hdnCustomerId').value);
        GetPurchaseForGstValue();
        page.tabs[0].SetEnabled(true);
        page.tabs[1].SetEnabled(true);
        if ($('#hfBSAlertFlag').val() == "1") {
            jConfirm('Wish to View/Select Billing and Shipping details?', 'Confirmation Dialog', function (r) {
                if (r == true) {
                    page.SetActiveTabIndex(1);
                }
            });
        }
        cContactPerson.Focus();

        var OtherDetails = {}
        OtherDetails.VendorId = VendorID;
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
    }
}

function SetEntityType(Id) {

    $.ajax({
        type: "POST",
        url: "PurchaseChallan.aspx/GetEntityType",
        data: JSON.stringify({ Id: Id }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (r) {
            $("#hdnEntityType").val(r.d);
        }

    });
} 
//Chinmoy added below function

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
           
    cddlPosGstChallan.ClearItems();
    if(cddlPosGstChallan.GetItemCount()==0)
    {
        cddlPosGstChallan.AddItem(GetShippingStateName() + '[Shipping]', "S");
        cddlPosGstChallan.AddItem(GetBillingStateName() + '[Billing]', "B");
    }
            
    else  if(cddlPosGstChallan.GetItemCount()>2)
    {
        cddlPosGstChallan.ClearItems();
        //cddl_PosGstSalesOrder.RemoveItem(0);
        //cddl_PosGstSalesOrder.RemoveItem(0);
    }

    if(PosGstId=="" || PosGstId==null)
    {
        cddlPosGstChallan.SetValue("S");
    }
    else
    {
        cddlPosGstChallan.SetValue(PosGstId);
    }
}


var PosGstId="";
function PopulateChallanPosGst(e)
{
            
    PosGstId=cddlPosGstChallan.GetValue();
    if(PosGstId=="S")
    {
        cddlPosGstChallan.SetValue("S");  
    }
    else if(PosGstId=="B")
    {
        cddlPosGstChallan.SetValue("B"); 
    }
}

function ValueSelected(e, indexName) {
    if (e.code == "Enter" || e.code== "NumpadEnter") {
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
                //added by chinmoy
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


    //function prodkeydown(e) {
    //    //Both-->B;Inventory Item-->Y;Capital Goods-->C
    //    var inventoryType = (document.getElementById("ddlInventory").value != null) ? document.getElementById("ddlInventory").value : "";

    //    var OtherDetails = {}
    //    OtherDetails.SearchKey = $("#txtProdSearch").val();
    //    OtherDetails.InventoryType = inventoryType;

    //    if (e.code == "Enter" || e.code == "NumpadEnter") {
    //        var HeaderCaption = [];
    //        HeaderCaption.push("Product Code");
    //        HeaderCaption.push("Product Name");
    //        HeaderCaption.push("Inventory");
    //        HeaderCaption.push("HSN/SAC");
    //        HeaderCaption.push("Class");
    //        HeaderCaption.push("Brand");
    //        if ($("#txtProdSearch").val() != '') {
    //            callonServer("Services/Master.asmx/GetPurchaseProduct", OtherDetails, "ProductTable", HeaderCaption, "ProdIndex", "SetProduct");
    //        }
    //    }
    //    else if (e.code == "ArrowDown") {
    //        if ($("input[ProdIndex=0]"))
    //            $("input[ProdIndex=0]").focus();
    //    }
    //}


    function prodkeydown(e) {
        //Both-->B;Inventory Item-->Y;Capital Goods-->C
        var inventoryType = (document.getElementById("ddlInventory").value != null) ? document.getElementById("ddlInventory").value : "";

        var OtherDetails = {}
        OtherDetails.SearchKey = $("#txtProdSearch").val();
        OtherDetails.InventoryType = inventoryType;

        if (e.code == "Enter" || e.code == "NumpadEnter") {
            var HeaderCaption = [];
            HeaderCaption.push("Product Code");
            HeaderCaption.push("Product Name");
            HeaderCaption.push("Inventory");
            HeaderCaption.push("HSN/SAC");
            HeaderCaption.push("GST Rate");
            HeaderCaption.push("Class");
            HeaderCaption.push("Brand");
            if ($("#txtProdSearch").val() != '') {
                callonServer("Services/Master.asmx/GetPurchaseChallanProductForPOpup", OtherDetails, "ProductTable", HeaderCaption, "ProdIndex", "SetProduct");
            }
        }
        else if (e.code == "ArrowDown") {
            if ($("input[ProdIndex=0]"))
                $("input[ProdIndex=0]").focus();
        }
    }


    $(document).ready(function() {

       
        var DocNo=getParameterByName("key");
        var DocType=getParameterByName("type");
        var BranchId=$("#ddl_Branch").val();
        $("#hdnTCBranchId").val(BranchId);
        if(DocNo){
            if(DocNo.toLowerCase()!="add"){
                if(GetObjectID('hdnJsonTempStock').value){
                    var myObj=GetObjectID('hdnJsonTempStock').value;
                    var JObject=JSON.parse(myObj);    
            
                    if (JObject.length > 0) {
                        for (x in JObject) {
                            JObject[x]["SrlNo"]=parseInt(JObject[x]["SrlNo"]);
                            JObject[x]["LoopID"]=parseInt(JObject[x]["LoopID"]);
                        }
                    }

                    StockOfProduct=JObject;
                    GetObjectID('hdnJsonTempStock').value="";

                    if (ctaggingList.GetValue() != null && ctaggingList.GetValue()!="") {
                        grid.GetEditor('ProductName').SetEnabled(false);
                    }
                }        
            } 
        }
    });



    function onBranchItems() {
        if ($("#hdnProjectSelectInEntryModule").val() == "1")
            clookup_Project.gridView.Refresh();

        function ProjectListKeyDown(s, e) {
            if (e.htmlEvent.key == "Enter") {
                s.OnButtonClick(0);
            }
        }

        function ProjectListButnClick(s, e) {
            clookup_Project.ShowDropDown();
        }

    }      

    function Project_gotFocus() {
        if ($("#hdnProjectSelectInEntryModule").val() == "1")
            clookup_Project.gridView.Refresh();
        clookup_Project.ShowDropDown();
    }



    //Hierarchy Start Tanmoy
    function Project_LostFocus() {
        var projID = clookup_Project.GetValue();
        if (projID == null || projID == "") {
            $("#ddlHierarchy").val(0);
        }
    }

function ProjectValueChange(s, e) {
    //debugger;
    var projID = clookup_Project.GetValue();

    $.ajax({
        type: "POST",
        url: 'PurchaseChallan.aspx/getHierarchyID',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ ProjID: projID }),
        success: function (msg) {
            var data = msg.d;
            $("#ddlHierarchy").val(data);
        }
    });
}
//Hierarchy End Tanmoy


function BindOrderProjectdata(OrderId, TagDocType) {
   
    var OtherDetail = {};

    OtherDetail.OrderId = OrderId;
    OtherDetail.TagDocType = 'PO';


    if ((OrderId != null) && (OrderId != "")) {

        $.ajax({
            type: "POST",
            url: "PurchaseChallan.aspx/SetProjectCode",
            data: JSON.stringify(OtherDetail),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                var Code = msg.d;

                clookup_Project.gridView.SelectItemsByKey(Code[0].ProjectId);
                clookup_Project.SetEnabled(false);
            }
        });


        var projID = clookup_Project.GetValue();

        $.ajax({
            type: "POST",
            url: 'PurchaseChallan.aspx/getHierarchyID',
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
//var SimilarProjectStatus = "0";

//function SimilarProjetcheck(quote_Id,Doctype)
//{
//    $.ajax({
//        type: "POST",
//        url: "ProjectPurchaseChallan.aspx/DocWiseSimilarProjectCheck",
//        data: JSON.stringify({ quote_Id: quote_Id, Doctype: Doctype }),
//        contentType: "application/json; charset=utf-8",
//        dataType: "json",
//        async: false,
//        success: function (msg) {
//            SimilarProjectStatus = msg.d;
//            debugger;
//            if (SimilarProjectStatus != "1") {
//                cPLQADate.SetText("");
//                jAlert("Please select document with same project code to proceed.");

//                return false;

//            }
//        }
//    });
//}