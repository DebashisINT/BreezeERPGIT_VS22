/**********************************************
Global Variable
*********************************************/
var GlobalAllAddress = [];
var BillingOrShipping;



function loadAddressbyCustomerIDForBasket(customerId) {

    var OtherDetails = {}
    OtherDetails.CustomerId = customerId;
    $.ajax({
        type: "POST",
        url: "Services/Master.asmx/GetCustomerAddress",
        data: JSON.stringify(OtherDetails),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var returnObject = msg.d;
            GlobalAllAddress = returnObject;
            var BillingObj = $.grep(returnObject, function (e) { return e.Type == "Billing" && e.deflt == 1; })
            var ShippingObj = $.grep(returnObject, function (e) { return e.Type == "Shipping" && e.deflt == 1; })

            //Set Billing
            if (BillingObj.length > 0) {

                ctxtAddress1.SetText(BillingObj[0].add_address1);
                ctxtAddress2.SetText(BillingObj[0].add_address2);
                ctxtAddress3.SetText(BillingObj[0].add_address3);
                $('#hdBillingPin').val(BillingObj[0].pnId);
                ctxtbillingPin.SetText(BillingObj[0].pnCd);
                $('#lblBillingCountry').text(BillingObj[0].conName);
                $('#lblBillingCountryValue').val(BillingObj[0].couId);
                $('#lblBillingState').text(BillingObj[0].stName);
                $('#lblBillingStateText').val(BillingObj[0].stName);
                $('#lblBillingStateValue').val(BillingObj[0].stId);
                $('#lblBillingCity').text(BillingObj[0].ctyName);
                $('#lblBillingCityValue').val(BillingObj[0].ctyId);
                ctxtlandmark.SetText(BillingObj[0].landMk);
                var GSTIN = BillingObj[0].GSTIN;
                var GSTIN1 = GSTIN.substring(0, 2);
                var GSTIN2 = GSTIN.substring(2, 12);
                var GSTIN3 = GSTIN.substring(12, 15);


                ctxtBillingGSTIN1.SetText(GSTIN1);
                ctxtBillingGSTIN2.SetText(GSTIN2);
                ctxtBillingGSTIN3.SetText(GSTIN3);

            } else {
                ctxtAddress1.SetText('');
                ctxtAddress2.SetText('');
                ctxtAddress3.SetText('');
                $('#hdBillingPin').val('');
                ctxtbillingPin.SetText('');
                $('#lblBillingCountry').text('');
                $('#lblBillingCountryValue').val('');
                $('#lblBillingState').text('');
                $('#lblBillingStateText').val('');
                $('#lblBillingStateValue').val('');
                $('#lblBillingCity').text('');
                $('#lblBillingCityValue').val('');
                ctxtlandmark.SetText('');
                ctxtBillingGSTIN1.SetText('');
                ctxtBillingGSTIN2.SetText('');
                ctxtBillingGSTIN3.SetText('');
            }

            //Set Shipping
            if (ShippingObj.length > 0) {
                ctxtsAddress1.SetText(ShippingObj[0].add_address1);
                ctxtsAddress2.SetText(ShippingObj[0].add_address2);
                ctxtsAddress3.SetText(ShippingObj[0].add_address3);
                $('#hdShippingPin').val(ShippingObj[0].pnId);
                ctxtShippingPin.SetText(ShippingObj[0].pnCd);
                $('#lblShippingCountry').text(ShippingObj[0].conName);
                $('#lblShippingCountryValue').val(ShippingObj[0].couId);
                $('#lblShippingState').text(ShippingObj[0].stName);
                $('#lblShippingStateText').val(ShippingObj[0].stName);;
                $('#lblShippingStateValue').val(ShippingObj[0].stId);
                $('#lblShippingCity').text(ShippingObj[0].ctyName);
                $('#lblShippingCityValue').val(ShippingObj[0].ctyId);
                ctxtslandmark.SetText(ShippingObj[0].landMk);
                var GSTIN = ShippingObj[0].GSTIN;
                var GSTIN1 = GSTIN.substring(0, 2);
                var GSTIN2 = GSTIN.substring(2, 12);
                var GSTIN3 = GSTIN.substring(12, 15);


                ctxtShippingGSTIN1.SetText(GSTIN1);
                ctxtShippingGSTIN2.SetText(GSTIN2);
                ctxtShippingGSTIN3.SetText(GSTIN3);

            } else {
                ctxtsAddress1.SetText('');
                ctxtsAddress2.SetText('');
                ctxtsAddress3.SetText('');
                $('#hdShippingPin').val('');
                ctxtShippingPin.SetText('');
                $('#lblShippingCountry').text('');
                $('#lblShippingCountryValue').val('');
                $('#lblShippingState').text('');
                $('#lblShippingStateText').val('');
                $('#lblShippingStateValue').val('');
                $('#lblShippingCity').text('');
                $('#lblShippingCityValue').val('');
                ctxtslandmark.SetText('');
                ctxtShippingGSTIN1.SetText('');
                ctxtShippingGSTIN2.SetText('');
                ctxtShippingGSTIN3.SetText('');
            }


            cbtnSave_citys.DoClick();
        }
    });
}


function SetLostFocusonDemand(e) {
    if ((new Date($("#hdnLockFromDate").val()) <= tstartdate.GetDate()) && (tstartdate.GetDate() <= new Date($("#hdnLockToDate").val()))) {
        jAlert("Adding  Invoice(POS) is not allowed as the Data is freezed from   " + $("#hdnLockFromDate").val() + " to " + $("#hdnLockToDate").val());
    }
}

function CustomerButnClick(s, e) {
    $('#CustModel').modal('show');
}
function CustomerKeyDown(s, e) {
    if (e.htmlEvent.key == "Enter" || e.code == "NumpadEnter") {
        $('#CustModel').modal('show');
    }
}

function Customerkeydown(e) {

    var OtherDetails = {}
    OtherDetails.SearchKey = $("#txtCustSearch").val();

    if (e.code == "Enter" || e.code == "NumpadEnter") {
        var HeaderCaption = [];
        HeaderCaption.push("Customer Name");
        HeaderCaption.push("Unique Id");
        HeaderCaption.push("Address");
        if ($("#txtCustSearch").val() != '') {
            callonServer("Services/Master.asmx/GetCustomer", OtherDetails, "CustomerTable", HeaderCaption, "customerIndex", "SetCustomer");
        }
    }
    else if (e.code == "ArrowDown") {
        if ($("input[customerindex=0]"))
            $("input[customerindex=0]").focus();
    }

}

function prodkeydown(e) {
    var OtherDetails = {}
    OtherDetails.SearchKey = $("#txtProdSearch").val();

    if (e.code == "Enter" || e.code == "NumpadEnter") {
        var HeaderCaption = [];
        HeaderCaption.push("Product Name");
        HeaderCaption.push("Product Description");
        HeaderCaption.push("HSN/SAC");
        if ($("#txtProdSearch").val() != '') {
            callonServer("Services/Master.asmx/GetPosProduct", OtherDetails, "ProductTable", HeaderCaption, "ProdIndex", "SetProduct");
        }
    }
    else if (e.code == "ArrowDown") {
        if ($("input[ProdIndex=0]"))
            $("input[ProdIndex=0]").focus();
    }

}

function ValueSelected(e, indexName) {
    if ((indexName == "ProdIndex") || (indexName == "customerIndex")) {
        if (e.code == "Enter" || e.code == "NumpadEnter") {
            var Id = e.target.parentElement.parentElement.cells[0].innerText;
            var name = e.target.parentElement.parentElement.cells[1].children[0].value;
            if (Id) {
                if (indexName == "ProdIndex")
                    SetProduct(Id, name);
                else if (indexName == "customerIndex")
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
                else if (indexName == "customerIndex")
                    $('#txtCustSearch').focus();
            }
        }
    }

    else if (indexName == "MainAccountSIIndex") {
        if (e.code == "Enter" || e.code == "NumpadEnter") {
            var Code = e.target.parentElement.parentElement.cells[0].innerText;
            var name = e.target.parentElement.parentElement.cells[1].children[0].value;

            $("#hdnSIMainAccount").val(Code);
            cSIMainAccount.SetText(name);
            cSIMainAccount_active.SetText(name);
            cMainAccountModelSI.Hide();
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
                $('#txtMainAccountSearch').focus();
            }
        }

    }

    else if (indexName == "HSNIndex") {
        if (e.code == "Enter" || e.code == "NumpadEnter") {
            var Code = e.target.parentElement.parentElement.cells[0].innerText;
            var name = e.target.parentElement.parentElement.cells[1].children[0].value;

            $("#hdnHSN").val(Code);
            cHSNCode.SetText(name);
            cCmbTradingLotUnits.SetFocus();
            cPopHSN.Hide();
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



    else if (indexName == "classIndex") {
        if (e.code == "Enter" || e.code == "NumpadEnter") {
            var Code = e.target.parentElement.parentElement.cells[0].innerText;
            var name = e.target.parentElement.parentElement.cells[1].children[0].value;

            $("#ClassId").val(Code);
            cProClassCode.SetText(name);

            cPopClass.Hide();
            cCmbStatus.SetFocus();

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
                $('#txtClassNameSearch').focus();
            }
        }

    }




    else if (indexName == "MainAccountSRIndex") {
        if (e.code == "Enter" || e.code == "NumpadEnter") {
            var Code = e.target.parentElement.parentElement.cells[0].innerText;
            var name = e.target.parentElement.parentElement.cells[1].children[0].value;

            $("#hdnSRMainAccount").val(Code);
            cSRMainAccount.SetText(name);
            cSRMainAccount_active.SetText(name)
            cMainAccountModelSR.Hide();
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
                $('#txtMainAccountSRSearch').focus();
            }
        }

    }
    else if (indexName == "MainAccountPIIndex") {
        if (e.code == "Enter" || e.code == "NumpadEnter") {
            var Code = e.target.parentElement.parentElement.cells[0].innerText;
            var name = e.target.parentElement.parentElement.cells[1].children[0].value;

            $("#hdnPIMainAccount").val(Code);
            cPIMainAccount.SetText(name);
            cPIMainAccount_active.SetText(name);
            cMainAccountModelPI.Hide();
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
                $('#txtMainAccountPISearch').focus();
            }
        }

    }
    else if (indexName == "MainAccountPRIndex") {
        if (e.code == "Enter" || e.code == "NumpadEnter") {
            var Code = e.target.parentElement.parentElement.cells[0].innerText;
            var name = e.target.parentElement.parentElement.cells[1].children[0].value;

            $("#hdnPRMainAccount").val(Code);
            cPRMainAccount.SetText(name);
            cPRMainAccount_active.SetText(name);
            cMainAccountModelPR.Hide();
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
                $('#txtMainAccountPRSearch').focus();
            }
        }

    }
}

function SetProduct(Id, Name) {
    console.log("Id", Id);
    console.log("Name", Name);


    $('#ProductModel').modal('hide');


    var LookUpData = Id;
    var ProductCode = Name;
    if (!ProductCode) {
        LookUpData = null;
    }
    cProductpopUp.Hide();
    grid.batchEditApi.StartEdit(globalRowIndex);
    //Delete hsn
    if (grid.GetEditor("ProductID").GetText() != "") {
        var previousProductId = grid.GetEditor("ProductID").GetText();
        RemoveHSnSacFromList(previousProductId.split("||@||")[19]);
    }


    grid.GetEditor("ProductID").SetText(LookUpData);
    grid.GetEditor("ProductName").SetText(ProductCode);

    pageheaderContent.style.display = "block";
    cddl_AmountAre.SetEnabled(false);

    var tbDescription = grid.GetEditor("Description");
    var tbUOM = grid.GetEditor("UOM");
    var tbSalePrice = grid.GetEditor("SalePrice");

    var ProductID = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
    var SpliteDetails = ProductID.split("||@||");
    var strProductID = SpliteDetails[0];
    var strDescription = SpliteDetails[1];
    var strUOM = SpliteDetails[2];
    var strStkUOM = SpliteDetails[4];
    var strSalePrice = SpliteDetails[6];
    $('#HDSelectedProduct').val(strProductID);

    GetSalesRateSchemePrice($("#hdnCustomerId").val(), strProductID, "0");

    var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
    var IsPackingActive = SpliteDetails[10];
    var Packing_Factor = SpliteDetails[11];
    var Packing_UOM = SpliteDetails[12];
    SetHsnSac(SpliteDetails[19]);
    var strRate = (ctxt_Rate.GetValue() != null && ctxt_Rate.GetValue() != "0") ? ctxt_Rate.GetValue() : "1";
    if (strRate == 0) {
        strSalePrice = strSalePrice;
    }
    else {
        strSalePrice = strSalePrice / strRate;
    }

    tbDescription.SetValue(strDescription);
    tbUOM.SetValue(strUOM);
    tbSalePrice.SetValue(strSalePrice);



    var totalNetAmount = grid.GetEditor("TotalAmount").GetValue();

    var newTotalNetAmount = parseFloat(cbnrlblAmountWithTaxValue.GetValue()) - parseFloat(totalNetAmount);
    cbnrlblAmountWithTaxValue.SetValue(parseFloat(Math.round(Math.abs(newTotalNetAmount) * 100) / 100).toFixed(2));
    SetInvoiceLebelValue();


    grid.GetEditor("Quantity").SetValue("0.00");
    grid.GetEditor("Discount").SetValue("0.00");
    grid.GetEditor("Amount").SetValue("0.00");
    grid.GetEditor("TaxAmount").SetValue("0.00");
    grid.GetEditor("TotalAmount").SetValue("0.00");

    var ddlbranch = $("[id*=ddl_Branch]");
    var strBranch = ddlbranch.find("option:selected").text();

    $('#lblStkQty').text("0.00");
    $('#lblStkUOM').text(strStkUOM);
    $('#lblProduct').text(strDescription);
    $('#lblbranchName').text(strBranch);



    ctaxUpdatePanel.PerformCallback('DelProdbySl~' + grid.GetEditor("SrlNo").GetValue() + '~' + strProductID);
    cbnrOtherChargesvalue.SetText('0.00');
    SetRunningBalance();
    grid.batchEditApi.StartEdit(globalRowIndex, 5);


    if ($("#hdnRateType").val() == "3") {
        if ($("#hdnProductDaynamicRate").val() != "") {
            grid.GetEditor("SalePrice").SetValue($("#hdnProductDaynamicRate").val());
        }
    }

    setTimeout(function () {
        //if ($("#ProductMinPrice").val() != "") {
        //    grid.GetEditor("SalePrice").SetValue($("#ProductMinPrice").val());
        //}
        
        grid.batchEditApi.StartEdit(globalRowIndex, 5);
    }, 100);


}

function BillingPinChange() {
    var billPin = ctxtbillingPin.GetText().trim();
    if (billPin != '') {
        var OtherDetails = {}
        OtherDetails.PinCode = billPin;
        $.ajax({
            type: "POST",
            url: "POSSalesInvoice.aspx/GetAddressbyPin",
            data: JSON.stringify(OtherDetails),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                var returnObject = msg.d;
                if (returnObject) {
                    $('#hdBillingPin').val(returnObject.pinId);
                    $('#lblBillingCountry').text(returnObject.ConName);
                    $('#lblBillingCountryValue').val(returnObject.ConId);
                    $('#lblBillingState').text(returnObject.stName);
                    $('#lblBillingStateText').val(returnObject.stName);
                    $('#lblBillingStateValue').val(returnObject.stId);
                    $('#lblBillingCity').text(returnObject.cityName);
                    $('#lblBillingCityValue').val(returnObject.cityId);

                } else {
                    ctxtbillingPin.Focus();
                    $('#hdBillingPin').val('');
                    $('#lblBillingCountry').text('');
                    $('#lblBillingCountryValue').val('');
                    $('#lblBillingState').text('');
                    $('#lblBillingStateText').val('');
                    $('#lblBillingStateValue').val('');
                    $('#lblBillingCity').text('');
                    $('#lblBillingCityValue').val('');

                }
            }
        });
    }
}


function ShippingPinChange() {
    var billPin = ctxtShippingPin.GetText().trim();
    if (billPin != '') {
        var OtherDetails = {}
        OtherDetails.PinCode = billPin;
        $.ajax({
            type: "POST",
            url: "POSSalesInvoice.aspx/GetAddressbyPin",
            data: JSON.stringify(OtherDetails),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                var returnObject = msg.d;
                if (returnObject) {
                    $('#hdShippingPin').val(returnObject.pinId);
                    $('#lblShippingCountry').text(returnObject.ConName);
                    $('#lblShippingCountryValue').val(returnObject.ConId);
                    $('#lblShippingState').text(returnObject.stName);
                    $('#lblShippingStateText').val(returnObject.stName);
                    $('#lblShippingStateValue').val(returnObject.stId);
                    $('#lblShippingCity').text(returnObject.cityName);
                    $('#lblShippingCityValue').val(returnObject.cityId);

                } else {
                    ctxtShippingPin.Focus();
                    $('#hdShippingPin').val('');
                    $('#lblShippingCountry').text('');
                    $('#lblShippingCountryValue').val('');
                    $('#lblShippingState').text('');
                    $('#lblShippingStateText').val('');
                    $('#lblShippingStateValue').val('');
                    $('#lblShippingCity').text('');
                    $('#lblShippingCityValue').val('');
                }
            }
        });
    }
}

function SetCustomer(Id, Name) {
    debugger;
    if (Id) {
        $('#CustModel').modal('hide');
        ctxtCustName.SetText(Name);

        page.GetTabByName('[B]illing/Shipping').SetEnabled(true);
        var type = ($("[id$='rdl_SaleInvoice']").find(":checked").val() != null) ? $("[id$='rdl_SaleInvoice']").find(":checked").val() : "";
        var startDate = new Date();
        startDate = tstartdate.GetValueString();

        if (type != "") {
            cQuotationComponentPanel.PerformCallback('BindComponentGrid' + '~' + Id + '~' + startDate + '~' + '%' + '~' + type);
        }

        var componentType = gridquotationLookup.GetValue();
        if (componentType != null && componentType != '') {
            grid.PerformCallback('GridBlank');
            $('#<%=hdnPageStatus.ClientID %>').val('update');
        }

        GetObjectID('hdnCustomerId').value = Id;
        GetObjectID('hdnAddressDtl').value = '0';

        page.SetActiveTabIndex(1);
        cbtnSave_citys.Focus();
        loadAddressbyCustomerID(Id);
        $('.dxeErrorCellSys').addClass('abc');
        $('.crossBtn').hide();
        page.GetTabByName('General').SetEnabled(false);
        $('#CustModel').modal('hide');
        ShowCustomerBalance();
        //Rev Rajdip For Get & set Credit days

        $.ajax({
            type: "POST",
            url: "PosSalesInvoice.aspx/GetCreditdays",
            data: JSON.stringify({ CustomerId: Id }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            // async:false,
            success: function (msg) {
                OutStandingAmount = msg.d;
                if (OutStandingAmount.CreditDays != 0) {
                    var cday = OutStandingAmount.CreditDays;
                    ctxtCreditDays.SetValue(cday);
                    cdt_SaleInvoiceDue.SetEnabled(false);

                    var CreditDays = ctxtCreditDays.GetValue();
                    var newdate = new Date();
                    var today = new Date();

                    today = tstartdate.GetDate();
                    today.setDate(today.getDate() + Math.round(CreditDays));

                    cdt_SaleInvoiceDue.SetDate(today);
                }
                else {
                    //ctxtCreditDays.SetValue(0);
                    //cdt_SaleInvoiceDue.SetDate(null);
                }
            }
        });
        //End Rev rajdip
    }
}

function ShowCustomerBalance() {
    //Ajax Started
    var CustomerId = GetObjectID('hdnCustomerId').value;
    var BranchId = $('#ddl_Branch').val();
    var AsOnDate = tstartdate.GetDate().format('yyyy-MM-dd');
    $.ajax({
        type: "POST",
        url: "SalesOrderAdd.aspx/GetCustomerOutStandingAmount",
        data: JSON.stringify({ strAsOnDate: AsOnDate, strCustomerId: CustomerId, BranchId: BranchId }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        // async:false,
        success: function (msg) {

            LoadingPanel.Hide();
            OutStandingAmount = msg.d;
            if (OutStandingAmount === "") {
                $('#lblOutstanding').text('0.00');
            }
            else {
                $('#lblOutstanding').text(OutStandingAmount);
            }

        }
    });

    //End
}




function loadAddressbyCustomerID(customerId) {

    var OtherDetails = {}
    OtherDetails.CustomerId = customerId;
    $.ajax({
        type: "POST",
        url: "Services/Master.asmx/GetCustomerAddress",
        data: JSON.stringify(OtherDetails),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var returnObject = msg.d;
            GlobalAllAddress = returnObject;
            var BillingObj = $.grep(returnObject, function (e) { return e.Type == "Billing" && e.deflt == 1; })
            var ShippingObj = $.grep(returnObject, function (e) { return e.Type == "Shipping" && e.deflt == 1; })

            //Set Billing
            if (BillingObj.length > 0) {

                ctxtAddress1.SetText(BillingObj[0].add_address1);
                ctxtAddress2.SetText(BillingObj[0].add_address2);
                ctxtAddress3.SetText(BillingObj[0].add_address3);
                $('#hdBillingPin').val(BillingObj[0].pnId);
                ctxtbillingPin.SetText(BillingObj[0].pnCd);
                $('#lblBillingCountry').text(BillingObj[0].conName);
                $('#lblBillingCountryValue').val(BillingObj[0].couId);
                $('#lblBillingState').text(BillingObj[0].stName);
                $('#lblBillingStateText').val(BillingObj[0].stName);
                $('#lblBillingStateValue').val(BillingObj[0].stId);
                $('#lblBillingCity').text(BillingObj[0].ctyName);
                $('#lblBillingCityValue').val(BillingObj[0].ctyId);
                ctxtlandmark.SetText(BillingObj[0].landMk);
                var GSTIN = BillingObj[0].GSTIN;
                var GSTIN1 = GSTIN.substring(0, 2);
                var GSTIN2 = GSTIN.substring(2, 12);
                var GSTIN3 = GSTIN.substring(12, 15);


                ctxtBillingGSTIN1.SetText(GSTIN1);
                ctxtBillingGSTIN2.SetText(GSTIN2);
                ctxtBillingGSTIN3.SetText(GSTIN3);
            } else {
                ctxtAddress1.SetText('');
                ctxtAddress2.SetText('');
                ctxtAddress3.SetText('');
                $('#hdBillingPin').val('');
                ctxtbillingPin.SetText('');
                $('#lblBillingCountry').text('');
                $('#lblBillingCountryValue').val('');
                $('#lblBillingState').text('');
                $('#lblBillingStateText').val('');
                $('#lblBillingStateValue').val('');
                $('#lblBillingCity').text('');
                $('#lblBillingCityValue').val('');
                ctxtlandmark.SetText('');
                ctxtBillingGSTIN1.SetText('');
                ctxtBillingGSTIN2.SetText('');
                ctxtBillingGSTIN3.SetText('');
            }

            //Set Shipping
            if (ShippingObj.length > 0) {
                ctxtsAddress1.SetText(ShippingObj[0].add_address1);
                ctxtsAddress2.SetText(ShippingObj[0].add_address2);
                ctxtsAddress3.SetText(ShippingObj[0].add_address3);
                $('#hdShippingPin').val(ShippingObj[0].pnId);
                ctxtShippingPin.SetText(ShippingObj[0].pnCd);
                $('#lblShippingCountry').text(ShippingObj[0].conName);
                $('#lblShippingCountryValue').val(ShippingObj[0].couId);
                $('#lblShippingState').text(ShippingObj[0].stName);
                $('#lblShippingStateText').val(ShippingObj[0].stName);;
                $('#lblShippingStateValue').val(ShippingObj[0].stId);
                $('#lblShippingCity').text(ShippingObj[0].ctyName);
                $('#lblShippingCityValue').val(ShippingObj[0].ctyId);
                ctxtslandmark.SetText(ShippingObj[0].landMk);
                var GSTIN = ShippingObj[0].GSTIN;
                var GSTIN1 = GSTIN.substring(0, 2);
                var GSTIN2 = GSTIN.substring(2, 12);
                var GSTIN3 = GSTIN.substring(12, 15);


                ctxtShippingGSTIN1.SetText(GSTIN1);
                ctxtShippingGSTIN2.SetText(GSTIN2);
                ctxtShippingGSTIN3.SetText(GSTIN3);
            } else {
                ctxtsAddress1.SetText('');
                ctxtsAddress2.SetText('');
                ctxtsAddress3.SetText('');
                $('#hdShippingPin').val('');
                ctxtShippingPin.SetText('');
                $('#lblShippingCountry').text('');
                $('#lblShippingCountryValue').val('');
                $('#lblShippingState').text('');
                $('#lblShippingStateText').val('');
                $('#lblShippingStateValue').val('');
                $('#lblShippingCity').text('');
                $('#lblShippingCityValue').val('');
                ctxtslandmark.SetText('');
                ctxtShippingGSTIN1.SetText('');
                ctxtShippingGSTIN2.SetText('');
                ctxtShippingGSTIN3.SetText('');
            }

            GetPosForGstValue();
            if (BillingObj.length == 0)
                txtSelectBillingAdd.Focus();
            else if (ShippingObj.length == 0)
                ctxtSelectShippingAdd.Focus();
            else
                cbtnSave_citys.Focus()
        }
    });
}

function SelectBillingAddClick(s, e) {
    BillingOrShipping = 'B'
    showAllAddress();
}
function SelectBillingAddKeyDown(s, e) {
    if (e.htmlEvent.key == "Enter" || e.code == "NumpadEnter") {
        BillingOrShipping = 'B'
        showAllAddress();
    }
}

function SelectShippingAddClick(s, e) {
    showAllAddress();
    BillingOrShipping = 'S'
}
function SelectShippingAddKeyDown(s, e) {
    if (e.htmlEvent.key == "Enter" || e.code == "NumpadEnter") {
        BillingOrShipping = 'S'
        showAllAddress();
    }
}

function showAllAddress() {
    $('#addressModel').modal('show');
    var htmlScript = "<table class='dynamicPopupTbl' border='1' width='100%'> <tr class='HeaderStyle'> <th class='hide'>id</th> <th>Address</th><th>Address Type</th></tr>";
    for (var le = 0; le < GlobalAllAddress.length; le++) {
        var completeAddress = GlobalAllAddress[le].add_address1 + ',' + GlobalAllAddress[le].add_address2 + ',' + GlobalAllAddress[le].add_address3 + ',' + GlobalAllAddress[le].landMk + ',' + GlobalAllAddress[le].conName + ',' + GlobalAllAddress[le].stName + ',' + GlobalAllAddress[le].ctyName + ',' + GlobalAllAddress[le].pnCd;
        completeAddress = completeAddress.replace(',,', ',');
        completeAddress = completeAddress.replace(',,', ',');
        htmlScript += "<tr> <td><input readonly onclick='AllAddressonClick(" + GlobalAllAddress[le].id + ")' type='text' style='background-color: #3399520a;'AddressIndex=" + le + " onfocus='AllAddressGetFocus(event)'  onblur='AllAddresslostFocus(event)' onkeydown=AllAddressSelected(event," + GlobalAllAddress[le].id + ") width='100%' readonly value='" + completeAddress + "'/></td><td onclick='AllAddressonClick(" + GlobalAllAddress[le].id + ")'> " + GlobalAllAddress[le].Type + "</td></tr>";
    }
    htmlScript += ' </table>';
    document.getElementById('AddressTable').innerHTML = htmlScript;

}


function AllAddressonClick(Id) {
    if (BillingOrShipping == 'B')
        SetBillingAddressFromAllAddress(Id);
    if (BillingOrShipping == 'S')
        SetShippingAddressFromAllAddress(Id);
}
function AllAddressGetFocus(e) {
    e.target.parentElement.parentElement.className = "focusrow";
    e.target.style = "background: #0000ff3d";
}
function AllAddresslostFocus(e) {
    e.target.parentElement.parentElement.className = "";
    e.target.style = "background-color: #3399520a";
}

function SetBillingAddressFromAllAddress(SelectedId) {
    $('#addressModel').modal('hide');
    var billingAdd = $.grep(GlobalAllAddress, function (e) { return e.id == SelectedId; });
    ctxtAddress1.SetText(billingAdd[0].add_address1);
    ctxtAddress2.SetText(billingAdd[0].add_address2);
    ctxtAddress3.SetText(billingAdd[0].add_address3);
    $('#hdBillingPin').val(billingAdd[0].pnId);
    ctxtbillingPin.SetText(billingAdd[0].pnCd);
    $('#lblBillingCountry').text(billingAdd[0].conName);
    $('#lblBillingCountryValue').val(billingAdd[0].couId);
    $('#lblBillingState').text(billingAdd[0].stName);
    $('#lblBillingStateText').val(billingAdd[0].stName);
    $('#lblBillingStateValue').val(billingAdd[0].stId);
    $('#lblBillingCity').text(billingAdd[0].ctyName);
    $('#lblBillingCityValue').val(billingAdd[0].ctyId);
    ctxtlandmark.SetText(billingAdd[0].landMk);
    var GSTIN = billingAdd[0].GSTIN;
    var GSTIN1 = GSTIN.substring(0, 2);
    var GSTIN2 = GSTIN.substring(2, 12);
    var GSTIN3 = GSTIN.substring(12, 15);


    ctxtBillingGSTIN1.SetText(GSTIN1);
    ctxtBillingGSTIN2.SetText(GSTIN2);
    ctxtBillingGSTIN3.SetText(GSTIN3);


    if (ctxtShippingPin.GetText() != '')
        cbtnSave_citys.Focus();
    else
        $('#shiptosame').focus();

}


function SetShippingAddressFromAllAddress(SelectedId) {
    $('#addressModel').modal('hide');
    var billingAdd = $.grep(GlobalAllAddress, function (e) { return e.id == SelectedId; });
    ctxtsAddress1.SetText(billingAdd[0].add_address1);
    ctxtsAddress2.SetText(billingAdd[0].add_address2);
    ctxtsAddress3.SetText(billingAdd[0].add_address3);
    $('#hdShippingPin').val(billingAdd[0].pnId);
    ctxtShippingPin.SetText(billingAdd[0].pnCd);
    $('#lblShippingCountry').text(billingAdd[0].conName);
    $('#lblShippingCountryValue').val(billingAdd[0].couId);
    $('#lblShippingState').text(billingAdd[0].stName);
    $('#lblShippingStateText').val(billingAdd[0].stName);;
    $('#lblShippingStateValue').val(billingAdd[0].stId);
    $('#lblShippingCity').text(billingAdd[0].ctyName);
    $('#lblShippingCityValue').val(billingAdd[0].ctyId);
    ctxtslandmark.SetText(billingAdd[0].landMk);
    var GSTIN = billingAdd[0].GSTIN;
    var GSTIN1 = GSTIN.substring(0, 2);
    var GSTIN2 = GSTIN.substring(2, 12);
    var GSTIN3 = GSTIN.substring(12, 15);


    ctxtShippingGSTIN1.SetText(GSTIN1);
    ctxtShippingGSTIN2.SetText(GSTIN2);
    ctxtShippingGSTIN3.SetText(GSTIN3);
    $('#billtoSame').focus();
    //cbtnSave_citys.Focus();
}


function AllAddressSelected(e, Id) {
    if (e.code == "Enter" || e.code == "NumpadEnter") {
        if (Id) {
            if (BillingOrShipping == 'B')
                SetBillingAddressFromAllAddress(Id);
            if (BillingOrShipping == 'S') {
                SetShippingAddressFromAllAddress(Id);

            }
        }
    }

    else if (e.code == "ArrowDown") {
        thisindex = parseFloat(e.target.getAttribute('AddressIndex'));
        thisindex++;
        if (thisindex < 10)
            $("input[AddressIndex=" + thisindex + "]").focus();
    }
    else if (e.code == "ArrowUp") {
        thisindex = parseFloat(e.target.getAttribute('AddressIndex'));
        thisindex--;
        if (thisindex > -1)
            $("input[AddressIndex=" + thisindex + "]").focus();
    }

}

function BillingCheckChange() {
    billingCopyToshipping();
}

function billingCopyToshipping() {

    ctxtsAddress1.SetText(ctxtAddress1.GetText());
    ctxtsAddress2.SetText(ctxtAddress2.GetText());
    ctxtsAddress3.SetText(ctxtAddress3.GetText());
    $('#hdShippingPin').val($('#hdBillingPin').val());
    ctxtShippingPin.SetText(ctxtbillingPin.GetText());
    $('#lblShippingCountry').text($('#lblBillingCountry').text());
    $('#lblShippingCountryValue').val($('#lblBillingCountryValue').val());
    $('#lblShippingState').text($('#lblBillingState').text());
    //Rev Subhra 09-05-2019 (commmented because below field is not value type,so you can't use val method to get or set data.)
    //$('#lblShippingStateText').val($('#lblBillingStateText').val());
    if ($('#lblShippingStateText').text() == "") {
        $('#lblShippingStateText').val($('#lblBillingStateText').val());
    }
    else {
        $('#lblShippingStateText').val($('#lblBillingStateText').text());
    }


    //End of Rev
    $('#lblShippingStateValue').val($('#lblBillingStateValue').val());
    $('#lblShippingCity').text($('#lblBillingCity').text());
    $('#lblShippingCityValue').val($('#lblBillingCityValue').val());
    ctxtslandmark.SetText(ctxtlandmark.GetText());


    ctxtShippingGSTIN1.SetText(ctxtBillingGSTIN1.GetText());
    ctxtShippingGSTIN2.SetText(ctxtBillingGSTIN2.GetText());
    ctxtShippingGSTIN3.SetText(ctxtBillingGSTIN3.GetText());

    ctaxUpdatePanel.PerformCallback('DeleteAllTax');
    cbtnSave_citys.Focus();
}




function ShippingCheckChange() {
    shippingCopyTobilling();
}

function shippingCopyTobilling() {

    ctxtAddress1.SetText(ctxtsAddress1.GetText());
    ctxtAddress2.SetText(ctxtsAddress2.GetText());
    ctxtAddress3.SetText(ctxtsAddress3.GetText());
    $('#hdBillingPin').val($('#hdShippingPin').val());
    ctxtbillingPin.SetText(ctxtShippingPin.GetText());
    $('#lblBillingCountry').text($('#lblShippingCountry').text());
    $('#lblBillingCountryValue').val($('#lblShippingCountryValue').val());
    $('#lblBillingState').text($('#lblShippingState').text());
    $('#lblBillingStateText').val($('#lblShippingStateText').val());
    $('#lblBillingStateValue').val($('#lblShippingStateValue').val());
    $('#lblBillingCity').text($('#lblShippingCity').text());
    $('#lblBillingCityValue').val($('#lblShippingCityValue').val());
    ctxtlandmark.SetText(ctxtslandmark.GetText());
    ctxtBillingGSTIN1.SetText(ctxtShippingGSTIN1.GetText());
    ctxtBillingGSTIN2.SetText(ctxtShippingGSTIN2.GetText());
    ctxtBillingGSTIN3.SetText(ctxtShippingGSTIN3.GetText());


    cbtnSave_citys.Focus();
}



$(document).ready(function () {
    $('#CustModel').on('shown.bs.modal', function () {
        $('#txtCustSearch').focus();
    })

    $('#CustModel').on('hide.bs.modal', function () {
        ctxtCustName.Focus();
    })

    $('#ProductModel').on('shown.bs.modal', function () {
        $('#txtProdSearch').focus();
    })

    $('#ProductModel').on('hide.bs.modal', function () {
        grid.batchEditApi.StartEdit(globalRowIndex, 3);
    })

    $('#addressModel').on('shown.bs.modal', function () {
        if ($("input[AddressIndex=0]"))
            $("input[AddressIndex=0]").focus();
    })
})



function GetSalesRateSchemePrice(CustomerID, ProductID, SalesPrice) {

    var date = new Date;
    var seconds = date.getSeconds();
    var minutes = date.getMinutes();
    var hour = date.getHours();

    var times = hour + ':' + minutes;

    var sdate = tstartdate.GetValue();
    var startDate = new Date(sdate);
    var OtherDetails = {}
    OtherDetails.CustomerID = CustomerID;
    OtherDetails.ProductID = ProductID;
    OtherDetails.PostingDate = startDate;//+ ' ' + times;
    $.ajax({
        type: "POST",
        url: "Services/Master.asmx/GetSalesRateSchemePrice",
        data: JSON.stringify(OtherDetails),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var returnObject = msg.d;
            console.log(returnObject);
            if (returnObject.length > 0) {
                $("#ProductMinPrice").val(returnObject[0].MinSalePrice);
                $("#ProductMaxPrice").val(returnObject[0].MaxSalePrice);
                $("#hdnRateType").val(returnObject[0].RateType);
                $("#hdnProductDaynamicRate").val(returnObject[0].MRP);
            }
        }
    });
}


