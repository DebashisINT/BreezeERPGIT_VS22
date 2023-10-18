//*********************************************************************************************************
//  Rev 1.0      Sanchita      V2.0.40   17-10-2023      New Fields required in Sales Quotation - RFQ Number, RFQ Date, Project / Site
//                                                       Mantis: 26871
//  Rev 2.0      Sanchita      V2.0.40   18-10-2023  Few Fields required in the Sales Quotation Entry Module for the Purpose of Quotation Print from ERP. Mantis: 26868
// **********************************************************************************************************
var warehousrdet
var Details;
var Txdel;
var OldUnitDetailsValue;
var AdjustType;
var PaymentDel;
var SaleInvoiceTaxDetails;
var headerDet;
// Mantis Issue 25129
var multuomdet;
// End of Mantis Issue 25129
$(document).ready(function () {


    var query = window.location.search.substring(1);
    var UrlArray = query.split("id=");
    if ((UrlArray[1] !=null) && (UrlArray[1] !=""))
    {
        InvoiceDetails(UrlArray[1]);
        $("#hdnInvoiceId").val(UrlArray[1]);
    }
    // Rev 2.0
    ShowHideOtherCondition();
    // End of Rev 2.0

});
var AnotherDetails = {};
var Termsdetails = {};
// Rev 2.0
var OtherConditiondetails = {};

function ShowHideOtherCondition() {
    debugger;
    OtherConditiondetails.InvoiceId = $("#hdnInvoiceId").val();
    var DetailsInvoiceId = $("#hdnInvoiceId").val();
    if (DetailsInvoiceId != null && DetailsInvoiceId != "" && DetailsInvoiceId != undefined) {
        $.ajax({
            type: "POST",
            url: "Services/ViewService.asmx/GetSystemSettings_OtherCondition",
            data: JSON.stringify(OtherConditiondetails),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (msg) {
                var OtherCondition = msg.d;

                if (OtherCondition == "Yes") {
                    btnTermsCondition.style.display = 'none';
                }
                else {
                    btnOtherCondition.style.display = 'none';
                }
            }
        });

    }
}

// End of Rev 2.0
function OpenSetTermsCondition()
{
    $('#TermsConditionsModelPopup').modal('show');

    Termsdetails.InvoiceId = $("#hdnInvoiceId").val();
    var DetailsInvoiceId = $("#hdnInvoiceId").val();
    if (DetailsInvoiceId != null && DetailsInvoiceId != "" && DetailsInvoiceId != undefined) {
        $.ajax({
            type: "POST",
            url: "Services/ViewService.asmx/GetInvoiceTermsDetails",
            data: JSON.stringify(Termsdetails),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (msg) {
                var TermsConditions = msg.d;
                if (TermsConditions != null) {
                    $("#lblDEliveryDate").text(TermsConditions.DeliveryScheduleDate);
                    $("#lblDeliveryRemarks").text(TermsConditions.DeliveryRemarks);
                    $("#lblInsuranceCoverage").text(TermsConditions.InsuranceCoverage);
                    $("#lblFreightCharges").text(TermsConditions.FreightCharges);
                    $("#lblFreightRemarks").text(TermsConditions.FreightRemark);
                    $("#lblEpermit").text(TermsConditions.EPermitPermit);
                    $("#lblOtherRemarks").text(TermsConditions.OtherRemarks);
                    $("#lblestCerti").text(TermsConditions.TestCertificateRequired);
                    $("#lblDeliveryDetails").text(TermsConditions.DeliveryDetails);
                    $("#lblPaymentTerms").text(TermsConditions.PaymentTerms);
                    $("#lblBankName").text(TermsConditions.BankName);
                    $("#lblBankBranchName").text(TermsConditions.BankBranchName);
                    $("#lblBankBranchAddress").text(TermsConditions.BankBranchAddress);
                    $("#lblBankBranchLandmark").text(TermsConditions.BankBranchLandmark);
                    $("#lblBankBranchPin").text(TermsConditions.BankBranchPin);
                    $("#lblAccountNumber").text(TermsConditions.AccountNumber);
                    $("#lblSWIFTCode").text(TermsConditions.SWIFTCode);
                    $("#lblRTGS").text(TermsConditions.RTGS);
                    $("#lblIFSCCode").text(TermsConditions.IFSCCode);
                    $("#lblTermsRemarks").text(TermsConditions.Remarks);
                    //Rev work start 09.08.2022 mantise no:0025110: A Text Input field required in the Terms & Condition of the entry Module as "Project"
                    $("#lblProject").text(TermsConditions.Project);
                    //Rev work close 09.08.2022 mantise no:0025110: A Text Input field required in the Terms & Condition of the entry Module as "Project"
                }
                else {
                    $("#lblDEliveryDate").text('');
                    $("#lblDeliveryRemarks").text('');
                    $("#lblInsuranceCoverage").text('');
                    $("#lblFreightCharges").text('');
                    $("#lblFreightRemarks").text('');
                    $("#lblEpermit").text('');
                    $("#lblOtherRemarks").text('');
                    $("#lblestCerti").text('');
                    $("#lblDeliveryDetails").text('');
                    $("#lblPaymentTerms").text('');
                    $("#lblBankName").text('');
                    $("#lblBankBranchName").text('');
                    $("#lblBankBranchAddress").text('');
                    $("#lblBankBranchLandmark").text('');
                    $("#lblBankBranchPin").text('');
                    $("#lblAccountNumber").text('');
                    $("#lblSWIFTCode").text('');
                    $("#lblRTGS").text('');
                    $("#lblIFSCCode").text('');
                    $("#lblTermsRemarks").text('');
                    //Rev work start 09.08.2022 mantise no:0025110: A Text Input field required in the Terms & Condition of the entry Module as "Project"
                    $("#lblProject").text('');
                    //Rev work close 09.08.2022 mantise no:0025110: A Text Input field required in the Terms & Condition of the entry Module as "Project"
                }
            }
        });
    }
    else {
        $("#lblDEliveryDate").text('');
        $("#lblDeliveryRemarks").text('');
        $("#lblInsuranceCoverage").text('');
        $("#lblFreightCharges").text('');
        $("#lblFreightRemarks").text('');
        $("#lblEpermit").text('');
        $("#lblOtherRemarks").text('');
        $("#lblestCerti").text('');
        $("#lblDeliveryDetails").text('');
        $("#lblPaymentTerms").text('');
        $("#lblBankName").text('');
        $("#lblBankBranchName").text('');
        $("#lblBankBranchAddress").text('');
        $("#lblBankBranchLandmark").text('');
        $("#lblBankBranchPin").text('');
        $("#lblAccountNumber").text('');
        $("#lblSWIFTCode").text('');
        $("#lblRTGS").text('');
        $("#lblIFSCCode").text('');
        $("#lblTermsRemarks").text('');
        //Rev work start 09.08.2022 mantise no:0025110: A Text Input field required in the Terms & Condition of the entry Module as "Project"
        $("#lblProject").text('');
        //Rev work close 09.08.2022 mantise no:0025110: A Text Input field required in the Terms & Condition of the entry Module as "Project"
    }
}


function OpenSetTransporter()
{
    $('#TransporterModelPopup').modal('show');
   
    otherDetails.InvoiceId = $("#hdnInvoiceId").val();
    var DetailsInvoiceId = $("#hdnInvoiceId").val();
    if (DetailsInvoiceId != null && DetailsInvoiceId != "" && DetailsInvoiceId!=undefined)
        {
        $.ajax({
            type: "POST",
            url: "Services/ViewService.asmx/GetInvoiceTYransporterDetails",
            data: JSON.stringify(otherDetails),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async:false,
            success: function (msg) {
                var Transporterdt = msg.d;
                if(Transporterdt!=null)
                {
                    $("#lblTrasnsporterName").text(Transporterdt.TransporterName);
                    $("#lblFinalTransporter").text(Transporterdt.FinalTransporter);
                    $("#lblTransporterType").text(Transporterdt.TransporterType);
                    $("#lblRegistered").text(Transporterdt.Registered);
                    $("#lblVehicleNumber").text(Transporterdt.VehicleNo);
                    $("#lblFright").text(Transporterdt.Freight);
                    $("#lblGTSIN").text(Transporterdt.GSTIN);
                    $("#lblPoint").text(Transporterdt.Point);
                    $("#lblLoading").text(Transporterdt.Loading);
                    $("#lblUnloading").text(Transporterdt.Unloading);
                    $("#lblParking").text(Transporterdt.Parking);
                    $("#lblWeighmentCharges").text(Transporterdt.WeighmentCharges);
                    $("#lblTollTax").text(Transporterdt.TollTax);
                    $("#lblOtherCharges").text(Transporterdt.OtherCharges);
                    $("#lblTotalCharges").text(Transporterdt.TotalCharges);
                    $("#lblDistanceofDelivery").text(Transporterdt.DistanceofDelivery);
                    $("#lblLRNO").text(Transporterdt.LRNO);
                    $("#lblLRDate").text(Transporterdt.LRDate);
                    $("#lblVehicleOutDate").text(Transporterdt.VehicleOutDate);
                    $("#lblVehicleType").text(Transporterdt.VehicleType);
                    $("#lblTransportationMode").text(Transporterdt.TransportationMode);
                    $("#lblAddress").text(Transporterdt.Address);
                    $("#lblCountry").text(Transporterdt.Country);
                    $("#lblState").text(Transporterdt.State);
                    $("#lblCity").text(Transporterdt.City);
                    $("#lblPin").text(Transporterdt.Pin);
                    $("#lblArea").text(Transporterdt.Area);
                    $("#lblPhoneNo").text(Transporterdt.Phone);
                }
                else
                {
                    $("#lblTrasnsporterName").text('');
                    $("#lblFinalTransporter").text('');
                    $("#lblTransporterType").text('');
                    $("#lblRegistered").text('');
                    $("#lblVehicleNumber").text('');
                    $("#lblFright").text('');
                    $("#lblGTSIN").text('');
                    $("#lblPoint").text('');
                    $("#lblLoading").text('');
                    $("#lblUnloading").text('');
                    $("#lblParking").text('');
                    $("#lblWeighmentCharges").text('');
                    $("#lblTollTax").text('');
                    $("#lblOtherCharges").text('');
                    $("#lblTotalCharges").text('');
                    $("#lblDistanceofDelivery").text('');
                    $("#lblLRNO").text('');
                    $("#lblLRDate").text('');
                    $("#lblVehicleOutDate").text('');
                    $("#lblVehicleType").text('');
                    $("#lblTransportationMode").text('');
                    $("#lblAddress").text('');
                    $("#lblCountry").text('');
                    $("#lblState").text('');
                    $("#lblCity").text('');
                    $("#lblPin").text('');
                    $("#lblArea").text('');
                    $("#lblPhoneNo").text('');
                }
            }
        });
    }
    else
    {
        $("#lblTrasnsporterName").text('');
        $("#lblFinalTransporter").text('');
        $("#lblTransporterType").text('');
        $("#lblRegistered").text('');
        $("#lblVehicleNumber").text('');
        $("#lblFright").text('');
        $("#lblGTSIN").text('');
        $("#lblPoint").text('');
        $("#lblLoading").text('');
        $("#lblUnloading").text('');
        $("#lblParking").text('');
        $("#lblWeighmentCharges").text('');
        $("#lblTollTax").text('');
        $("#lblOtherCharges").text('');
        $("#lblTotalCharges").text('');
        $("#lblDistanceofDelivery").text('');
        $("#lblLRNO").text('');
        $("#lblLRDate").text('');
        $("#lblVehicleOutDate").text('');
        $("#lblVehicleType").text('');
        $("#lblTransportationMode").text('');
        $("#lblAddress").text('');
        $("#lblCountry").text('');
        $("#lblState").text('');
        $("#lblCity").text('');
        $("#lblPin").text('');
        $("#lblArea").text('');
        $("#lblPhoneNo").text('');
    }
}

var TcsHeader = {};
var TCsHDetails = {};
function OpenSetTCS()
{
    $('#TCSModelPopup').modal('show');
   
    TcsHeader.InvoiceId = $("#hdnInvoiceId").val();
    var DetailsInvoiceId = $("#hdnInvoiceId").val();
    if (DetailsInvoiceId != null && DetailsInvoiceId != "" && DetailsInvoiceId != undefined) {

        $.ajax({
            type: "POST",
            url: "Services/ViewService.asmx/GetHeaderTCSDetails",
            data: JSON.stringify(TcsHeader),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (msg) {

                var TCSHeaderDetails = msg.d;
                if (TCSHeaderDetails != null) {
                    $("#lblTCSSection").text(TCSHeaderDetails.TCSSection);
                    $("#lblTCSApplicableAmount").text(TCSHeaderDetails.TCSApplicableAmount);
                    $("#lblTCSPercentage").text(TCSHeaderDetails.TCSPercentage);
                    $("#lblTCSAmount").text(TCSHeaderDetails.TCSAmount);
                }
                else
                {
                    $("#lblTCSSection").text('');
                    $("#lblTCSApplicableAmount").text('0.00');
                    $("#lblTCSPercentage").text('0.000');
                    $("#lblTCSAmount").text('0.00');
                }

            }
        });
    }
    else
    {
        $("#lblTCSSection").text('');
        $("#lblTCSApplicableAmount").text('0.00');
        $("#lblTCSPercentage").text('0.000');
        $("#lblTCSAmount").text('0.00');
    }

    TCsHDetails.InvoiceId = $("#hdnInvoiceId").val();
    if (DetailsInvoiceId != null && DetailsInvoiceId != "" && DetailsInvoiceId != undefined) {

        $.ajax({
            type: "POST",
            url: "Services/ViewService.asmx/GetTCSDetails",
            data: JSON.stringify(TCsHDetails),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (msg) {

                var TCSHeaderDetails = msg.d.TCSDetails;
                if (TCSHeaderDetails != null) {
              

                    for (var i = 0; i < TCSHeaderDetails.length; i++) {
                        $("#tblTCSDetails").append("<tr><td>" + TCSHeaderDetails[i].SLNO + "</td><td>" + TCSHeaderDetails[i].Invoice_Number + "</td><td>" + TCSHeaderDetails[i].branch_description + "</td><td>" + TCSHeaderDetails[i].Doc_Type + "</td><td>" + TCSHeaderDetails[i].Invoice_Date + "</td><td>" + TCSHeaderDetails[i].TaxableAmount + "</td><td>" + TCSHeaderDetails[i].NetAmount + "</td><td>" + TCSHeaderDetails[i].TaxableRunning + "</td><td>" + TCSHeaderDetails[i].NetRunning + "</td></tr>");
                    }

                }
             

            }
        });
    }
   


}

function StockDetails(InvoiceId, Details_Id)
{
    debugger;
    var otherDetails = {};
    otherDetails.InvoiceId = InvoiceId;
    otherDetails.Details_Id = Details_Id;
    $.ajax({
        type: "POST",
        url: "Services/ViewService.asmx/GetwarehouseDetails",
        data: JSON.stringify(otherDetails),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {

            warehousrdet = msg.d.warehouseDetails;
            $('#PoswarehouseModel').modal('show');
            document.getElementById("Poswarehouse").innerHTML = InvoiceId;
            document.getElementById("Poswarehouse").innerHTML = Details_Id;
            var htmlScript = "<table border=\'1' width=\"100%\" class=\"dynamicPopupTbl\"><tr class=\"HeaderStyle\"><th class=\"text-left\">Warehouse</th><th class=\"text-center\">Quantity</th><th class=\"text-center\">Alt. Quantity</th> </tr>";
            //var FilteredtaxDetails = $.grep(warehousrdet, function (e) { return e.Detailsid.trim() == Details_Id; });


            for (var i = 0; i < warehousrdet.length; i++) {
                htmlScript += "<tr><td class=\"text-left\">" + warehousrdet[i].WarehouseName + "</td><td class=\"text-right\">" + warehousrdet[i].OUT_Quantity + "</td><td class=\"text-right\">" + warehousrdet[i].Alt_StockOut + "</td></tr>";

            }
            htmlScript += ' </table>';
            document.getElementById('Poswarehouse').innerHTML = htmlScript;
        }
    });

}

// Mantis Issue 25129
function MultiUOMDetails(InvoiceId, Details_Id) {
    var otherDetails = {};
    otherDetails.InvoiceId = InvoiceId;
    otherDetails.Details_Id = Details_Id;
    $.ajax({
        type: "POST",
        url: "Services/ViewService.asmx/GetMultipleUOMDetails",
        data: JSON.stringify(otherDetails),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {

            multuomdet = msg.d.multiuomdetails;
            $('#MultipleUOMModel').modal('show');
            document.getElementById("Posmultuom").innerHTML = InvoiceId;
            document.getElementById("Posmultuom").innerHTML = Details_Id;
            var htmlScript = "<table border=\'1' width=\"100%\" class=\"dynamicPopupTbl\"><tr class=\"HeaderStyle\"><th class=\"text-left\">Quantity</th><th class=\"text-center\">UOM</th><th class=\"text-center\">Rate</th> <th class=\"text-center\">Alt. UOM</th><th class=\"text-center\">Alt. Quantity</th> <th class=\"text-center\">Rate</th> <th class=\"text-center\">Update Row</th> </tr>";
            

            for (var i = 0; i < multuomdet.length; i++) {
                htmlScript += "<tr><td class=\"text-right\">" + multuomdet[i].Quantity + "</td><td class=\"text-right\">" + multuomdet[i].UOM + "</td><td class=\"text-right\">" + multuomdet[i].BaseRate + "</td><td class=\"text-right\">" + multuomdet[i].AltUOM + "</td><td class=\"text-right\">" + multuomdet[i].AltQuantity + "</td><td class=\"text-right\">" + multuomdet[i].AltRate + "</td><td class=\"text-right\">" + multuomdet[i].UpdateRow + "</td></tr>";

            }
            htmlScript += ' </table>';
            document.getElementById('Posmultuom').innerHTML = htmlScript;
        }
    });

}

// End of Mantis Issue 25129


function InvoiceDetails(InvoiceId)
{
    debugger;
    if ((InvoiceId != null) && (InvoiceId != "") && InvoiceId == parseInt(InvoiceId, 10))
    {
      
        $.ajax({
            type: "POST",
            url: "Services/ViewService.asmx/GetInvoiceDetails",
            data: JSON.stringify({ InvoiceId: InvoiceId }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                headerDet= msg.d.HeaderDetails;
                invoiceDet = msg.d.ProDetails;
                Txdel = msg.d.TxDetail;
                OldUnitDetailsValue = msg.d.OldDetails;
                AdjustType = msg.d.AdjustDetails;
                PaymentDel = msg.d.Payment;
                SaleInvoiceTaxDetails = msg.d.InvoiceTax;

                var PaymentWOCash = $.grep(PaymentDel, function (e) { return e.PaymentType.trim() != "Cash"; });
                var PaymentWithcash = $.grep(PaymentDel, function (e) { return e.PaymentType.trim() == "Cash"; });

                //debugger;
                console.log(headerDet);

                for (var i = 0; i < invoiceDet.length; i++) {
                    $("#tblPosProductDetails").append("<tr onclick=DetailsRowClick('" + invoiceDet[i].DetailsId + "') style='cursor: pointer;' id='Producttr'><td style='display: none'>" + invoiceDet[i].DetailsId + "</td><td>" + invoiceDet[i].SL + "</td><td>" + invoiceDet[i].ProductName + "</td><td class=\"text-right\">" + invoiceDet[i].Quantity + "</td><td>" + invoiceDet[i].UOM + "</td><td class=\"text-right\">" + invoiceDet[i].QTY2 + "</td><td>"
                        + invoiceDet[i].UOM2 + "</td><td style='text-align: center;'><button class='btn btn-default btn-xs' title='View Multi UOM' onclick='event.stopPropagation();return false,MultiUOMDetails(headerDet.Id," + invoiceDet[i].DetailsId + ")' ><img src='/assests/images/MultiUomIcon.png' /></button></td><td class=\"text-right\">" + invoiceDet[i].Price + "</td><td class=\"text-right\">" + invoiceDet[i].Amount + "</td><td class=\"text-right\">" + invoiceDet[i].Charges + "</td><td class=\"text-right\">" + invoiceDet[i].NetAmount + "</td><td style='text-align: center;'><button class='btn btn-default btn-xs' title='View Stock' onclick='event.stopPropagation();return false,StockDetails(headerDet.Id," + invoiceDet[i].DetailsId + ")' ><img src='/assests/images/warehouse.png' /></button></td></tr>");
                } 


                if (OldUnitDetailsValue.length == 0) {
                    $('#divOldUnitDetails').css('display', 'none')
                }
                else {
                    for (var i = 0; i < OldUnitDetailsValue.length; i++) {
                        $("#tblOldUnitDetails").append("<tr><td>" + OldUnitDetailsValue[i].ProductName + "</td><td class=\"text-right\">" + OldUnitDetailsValue[i].Quantity + "</td><td>" + OldUnitDetailsValue[i].UnitValue + "</td></tr>");
                    }
                }

                if (AdjustType.length == 0) {
                    $('#divAdjustmentDetails').css('display', 'none')
                }
                else {
                    for (var i = 0; i < AdjustType.length; i++) {
                        $("#tblAdjustmentDetails").append("<tr><td>" + AdjustType[i].DocumentNumber + "</td><td>" + AdjustType[i].DocumentType + "</td><td class=\"text-right\">" + AdjustType[i].AdjustmentAmount + "</td></tr>");
                    }
                }

                if (PaymentWOCash.length == 0) {
                    $('#tblPosPaymentDetails').css('display', 'none')
                }
                else {
                    for (var i = 0; i < PaymentWOCash.length; i++) {
                        $("#tblPosPaymentDetails").append("<tr><td>" + PaymentWOCash[i].PaymentType + "</td><td>" + PaymentWOCash[i].InstrumentNo + "</td><td>" + PaymentWOCash[i].cardType + "</td><td>"
                            + PaymentWOCash[i].ApprovalCode + "</td><td>" + PaymentWOCash[i].EnterDate + "</td><td>" + PaymentWOCash[i].DraweeDate + "</td><td>" + PaymentWOCash[i].Remarks + "</td><td>" + PaymentWOCash[i].AccountCode + "</td><td class=\"text-right\">" + PaymentWOCash[i].Amount + "</td></tr>");
                    }
                }

                if (PaymentWithcash.length == 0) {
                    //document.getElementById('lblCash').style.display = 'none'
                    $('#lblAmount').css('display', 'none')
                }
                else if (PaymentWithcash[0].Amount == 0.00)
                {
                    //document.getElementById('lblCash').style.display = 'none'
                    $('#lblAmount').css('display', 'none')
                   
                }
                else
                {
                    //document.getElementById('lblCash').style.display = 'block'
                    for (var i = 0; i < PaymentWithcash.length; i++) {
                        $("#lblAmount").append("<tr><td>" +'Cash : '+ PaymentWithcash[i].Amount + "</td></tr>");
                    }
                }


                if (SaleInvoiceTaxDetails.length == 0) {
                    $('#divSaleInvoiceTax').css('display', 'none')
                }
                else {
                    for (var i = 0; i < SaleInvoiceTaxDetails.length; i++) {
                        $("#tblSaleInvoiceTax").append("<tr><td>" + SaleInvoiceTaxDetails[i].TaxSchemeName + "</td><td>" + SaleInvoiceTaxDetails[i].TaxPercentage + "</td><td>" + SaleInvoiceTaxDetails[i].TaxAmount + "</td></tr>");
                    }
                }


                if (PaymentWOCash.length == 0 && (PaymentWithcash.length == 0 || PaymentWithcash[0].Amount == 0.00))
                {
                    $('#divPaymentDetails').css('display', 'none')
                }


                if (headerDet) {

                    if (!headerDet.isFromPos)
                        if (parent.cPosView)
                        parent.cPosView.SetHeaderText('Sales Invoice')

                    document.getElementById("lblInvoiceNumber").innerHTML = headerDet.InvoiceNumber;
                    document.getElementById("lblInvoiceDate").innerHTML = headerDet.InvoiceDate;
                    document.getElementById("lblDeliveryDate").innerHTML = headerDet.DelivaryDate;
                    document.getElementById("lblCustomerName").innerHTML = headerDet.CustomerName;
                    document.getElementById("lblSalesMan").innerHTML = headerDet.SalesmanName;
                    document.getElementById("lblReference").innerHTML = headerDet.Reference;
                    document.getElementById("lblChallanNo").innerHTML = headerDet.challan_no;
                    document.getElementById("lblChallanDate").innerHTML = headerDet.ChallanDate;
                    document.getElementById("lblRemarks").innerHTML = headerDet.Remarks;
                    document.getElementById("lblEntryType").innerHTML = headerDet.EntryType;

                    document.getElementById("lblTaxableAmount").innerHTML = headerDet.tot_taxable_amount;
                    document.getElementById("lblTaxAmount").innerHTML = headerDet.tot_taxamount;
                    document.getElementById("lblNewTotalAmount").innerHTML = headerDet.tot_amount; 
                    document.getElementById("lblVehicleNo").innerHTML = headerDet.Vehicle_No;

                    document.getElementById("lblmainqty").innerHTML = headerDet.total_main_qty;
                    document.getElementById("lblaltqty").innerHTML = headerDet.total_alt_qty;

                    // Rev 1.0
                    if (headerDet.ShowRFQ == "Yes") {
                        document.getElementById("lblRFQNumber").innerHTML = headerDet.RFQNumber;
                        document.getElementById("lblRFQDate").innerHTML = headerDet.RFQDate;
                    }
                    else {
                        $('#tdFRQNo').css('display', 'none')
                        $('#tdFRQDt').css('display', 'none')
                    }

                    if (headerDet.ShowProjectSite == "Yes") {
                        document.getElementById("lblProjectSite").innerHTML = headerDet.ProjectSite;
                    }
                    else {
                        $('#tdProjSitedet').css('display', 'none')
                    }
                    // End of Rev 1.0

                    $('#txtAreaBillAddress').text(headerDet.BillingAddress);
                    $('#txtAreaShipAddress').text(headerDet.ShippingAddress);
                   // document.getElementById("lblDisAmount").innerHTML = headerDet.InvoiceDiscount;
                   // document.getElementById("lblFinanceChallanDate").innerHTML = headerDet.FinancerChallanDate; 
                   // document.getElementById("txtAreaBillAddress").innerHTML = headerDet.BillingAddress;
                    // document.getElementById("txtAreaShipAddress").innerHTML = headerDet.ShippingAddress;

                    if (headerDet.BillingfromAddress != "")
                    {
                        $('#txtBillfromAddress').text(headerDet.BillingfromAddress);
                        $('#txtShipfromAddress').text(headerDet.ShippingFromAddress);
                        
                    }
                    else
                    {
                        $('#divBillFromDispatch').css('display', 'none')
                    }
                    
                }
                
               

                       if (headerDet.EntryType == "Fin") {
                           document.getElementById('lblFin').style.display = 'block'
                           document.getElementById('lblexec').style.display = 'block'
                           document.getElementById('lblEMI').style.display = 'block'
                           document.getElementById('lblSC').style.display = 'block'
                           document.getElementById('lblSF').style.display = 'block'
                           document.getElementById('lblDB').style.display = 'block'
                           document.getElementById('lblDBPer').style.display = 'block'
                           document.getElementById('lblDownpay').style.display = 'block'
                           document.getElementById('lblProc').style.display = 'block'
                           document.getElementById('lblCharges').style.display = 'block'
                           document.getElementById('lblDP').style.display = 'block'
                           document.getElementById('lblDue').style.display = 'block'
                           document.getElementById('lblFinCh').style.display = 'block'
                           document.getElementById('lblFinDate').style.display = 'block'
                           document.getElementById("lblFinancerName").innerHTML = headerDet.FinancerName;
                           document.getElementById("lblExecutiveName").innerHTML = headerDet.ExcutiveName;
                           document.getElementById("lblEmiDetails").innerHTML = headerDet.EmiDetails;
                           document.getElementById("lblScheme").innerHTML = headerDet.Scheme;
                           document.getElementById("lblSfCode").innerHTML = headerDet.SFCode;
                           document.getElementById("lblDbd").innerHTML = headerDet.DBD;
                           document.getElementById("lblDbdPercent").innerHTML = headerDet.DBDPercent;
                           $('#lblDownpayment').text(headerDet.Downpayment);
                           //document.getElementById("lblDownpayment").innerHTML = headerDet.Downpayment;
                           document.getElementById("lblProcFee").innerHTML = headerDet.Fee;
                           document.getElementById("lblEmiOtherCharges").innerHTML = headerDet.EMIOtherCharges;
                           document.getElementById("lblTotalDpAmt").innerHTML = headerDet.TotalDPAmount;
                           document.getElementById("lblFinancerDue").innerHTML = headerDet.FinancerDue;
                           document.getElementById("lblFinanceChallanNo").innerHTML = headerDet.FinancerChallanNo;
                           $('#lblFinanceChallanDate').text(headerDet.FinancerChallanDate);
                       }
                       else
                       {
                           document.getElementById('lblFinancerName').style.display = 'none'
                           document.getElementById('lblExecutiveName').style.display = 'none'
                           document.getElementById('lblEmiDetails').style.display = 'none'
                           document.getElementById('lblScheme').style.display = 'none'
                           document.getElementById('lblSfCode').style.display = 'none'
                           document.getElementById('lblDbd').style.display = 'none'
                           document.getElementById('lblDbdPercent').style.display = 'none'
                           document.getElementById('lblDownpayment').style.display = 'none'
                           document.getElementById('lblProcFee').style.display = 'none'
                           document.getElementById('lblEmiOtherCharges').style.display = 'none'
                           document.getElementById('lblTotalDpAmt').style.display = 'none'
                           document.getElementById('lblFinancerDue').style.display = 'none'
                           document.getElementById('lblFinanceChallanNo').style.display = 'none'
                           document.getElementById('lblFinanceChallanDate').style.display = 'none'
                           $('#divFinanceDetails').css('display','none')
                       }



            }
        });
    }
}




function DetailsRowClick(id) {
    
    $('#PosTaxModel').modal('show');
    document.getElementById("PosTaxTable1").innerHTML = id;

    var htmlScript = "<table border=\'1' width=\"100%\" class=\"dynamicPopupTbl\"><tr class=\"HeaderStyle\"><th class=\"hide\">Product TaxId</th><th>Tax Scheme Name</th><th>Tax Percentage</th><th>Tax Amount</th> </tr>";

    var FilteredtaxDetails = $.grep(Txdel, function (e) { return e.ProductTaxId.trim() == id; });


    for (var i = 0; i < FilteredtaxDetails.length; i++)
 {
        htmlScript += "<tr><td style='display: none'>" + FilteredtaxDetails[i].ProductTaxId + "</td><td>" + FilteredtaxDetails[i].TaxSchemeName + "</td><td>" + FilteredtaxDetails[i].TaxPercentage + "</td><td>"
                 + FilteredtaxDetails[i].TaxAmount + "</td></tr>";

 }
 htmlScript += ' </table>';
 document.getElementById('PosTaxTable1').innerHTML = htmlScript;
}

// Rev 2.0
function OpenSetOtherCondition() {
    $('#OtherConditionseModal').modal('show');

    OtherConditiondetails.InvoiceId = $("#hdnInvoiceId").val();
    var DetailsInvoiceId = $("#hdnInvoiceId").val();
    if (DetailsInvoiceId != null && DetailsInvoiceId != "" && DetailsInvoiceId != undefined) {
        $.ajax({
            type: "POST",
            url: "Services/ViewService.asmx/GetInvoiceOtherDetails",
            data: JSON.stringify(OtherConditiondetails),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (msg) {
                var OtherConditions = msg.d;
                if (OtherConditions != null) {
                    $("#lblPriceBasis").text(OtherConditions.strPriceBasis);
                    $("#lblLoadingCharges").text(OtherConditions.strLoadingCharges);
                    $("#lblDetentionCharges").text(OtherConditions.strDetentionCharges);
                    $("#lblDeliveryPeriod").text(OtherConditions.strDeliveryPeriod);
                    $("#lblInspection").text(OtherConditions.strInspection);
                    $("#lblPaymentTermsOther").text(OtherConditions.strPaymentTermsOther);
                    $("#lblOfferValidUpto").text(OtherConditions.OfferValidUpto);
                    $("#lblQuantityTol").text(OtherConditions.strQuantityTol);
                    $("#lblDimensionalTol").text(OtherConditions.strDimensionalTol);
                    $("#lblThicknessTol").text(OtherConditions.strThicknessTol);
                    $("#lblWarranty").text(OtherConditions.strWarranty);
                    $("#lblDeviation").text(OtherConditions.strDeviation);
                    $("#lblLDClause").text(OtherConditions.strLDClause);
                    $("#lblInterestClause").text(OtherConditions.strInterestClause);
                    $("#lblPriceEscalationClause").text(OtherConditions.strPriceEscalationClause);
                    $("#lblInternalCoating").text(OtherConditions.strInternalCoating);
                    $("#lblExternalCoating").text(OtherConditions.strExternalCoating);
                    $("#lblSpecialNote").text(OtherConditions.strSpecialNote);
                   
                }
                else {
                    $("#lblPriceBasis").text('');
                    $("#lblLoadingCharges").text('');
                    $("#lblDetentionCharges").text('');
                    $("#lblDeliveryPeriod").text('');
                    $("#lblInspection").text('');
                    $("#lblPaymentTermsOther").text('');
                    $("#lblOfferValidUpto").text('');
                    $("#lblQuantityTol").text('');
                    $("#lblDimensionalTol").text('');
                    $("#lblThicknessTol").text('');
                    $("#lblWarranty").text('');
                    $("#lblDeviation").text('');
                    $("#lblLDClause").text('');
                    $("#lblInterestClause").text('');
                    $("#lblPriceEscalationClause").text('');
                    $("#lblInternalCoating").text('');
                    $("#lblExternalCoating").text('');
                    $("#lblSpecialNote").text('');
                }
            }
        });
    }
    else {
        $("#lblPriceBasis").text('');
        $("#lblLoadingCharges").text('');
        $("#lblDetentionCharges").text('');
        $("#lblDeliveryPeriod").text('');
        $("#lblInspection").text('');
        $("#lblPaymentTermsOther").text('');
        $("#lblOfferValidUpto").text('');
        $("#lblQuantityTol").text('');
        $("#lblDimensionalTol").text('');
        $("#lblThicknessTol").text('');
        $("#lblWarranty").text('');
        $("#lblDeviation").text('');
        $("#lblLDClause").text('');
        $("#lblInterestClause").text('');
        $("#lblPriceEscalationClause").text('');
        $("#lblInternalCoating").text('');
        $("#lblExternalCoating").text('');
        $("#lblSpecialNote").text('');
    }
}
// End of Rev 2.0