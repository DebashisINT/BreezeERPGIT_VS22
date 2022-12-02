

var Details;
var Txdel;
var OldUnitDetailsValue;
var AdjustType;
var PaymentDel;
var SaleInvoiceTaxDetails;
var headerDet;
$(document).ready(function () {


    var query = window.location.search.substring(1);
    var UrlArray = query.split("id=");
    if ((UrlArray[1] != null) && (UrlArray[1] != "")) {
        InvoiceDetails(UrlArray[1]);
    }


});



debugger;
function InvoiceDetails(InvoiceId) {

    if ((InvoiceId != null) && (InvoiceId != "") && InvoiceId == parseInt(InvoiceId, 10)) {

        $.ajax({
            type: "POST",
            url: "../OpeningServices/OpeningViewService.asmx/GetInvoiceDetails",
            data: JSON.stringify({ InvoiceId: InvoiceId }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                headerDet = msg.d.HeaderDetails;
                invoiceDet = msg.d.ProDetails;
                Txdel = msg.d.TxDetail;
                OldUnitDetailsValue = msg.d.OldDetails;
                AdjustType = msg.d.AdjustDetails;
                PaymentDel = msg.d.Payment;
                SaleInvoiceTaxDetails = msg.d.InvoiceTax;

                var PaymentWOCash = $.grep(PaymentDel, function (e) { return e.PaymentType.trim() != "Cash"; });
                var PaymentWithcash = $.grep(PaymentDel, function (e) { return e.PaymentType.trim() == "Cash"; });




                for (var i = 0; i < invoiceDet.length; i++) {
                    $("#tblPosProductDetails").append("<tr onclick=DetailsRowClick('" + invoiceDet[i].DetailsId + "') style='cursor: pointer;' id='Producttr'><td style='display: none'>" + invoiceDet[i].DetailsId + "</td><td>" + invoiceDet[i].ProductName + "</td><td>" + invoiceDet[i].Quantity + "</td><td>"
                        + invoiceDet[i].UOM + "</td><td>" + invoiceDet[i].Price + "</td><td>" + invoiceDet[i].Amount + "</td><td>" + invoiceDet[i].Charges + "</td><td>" + invoiceDet[i].NetAmount + "</td></tr>");
                }


                if (OldUnitDetailsValue.length == 0) {
                    $('#divOldUnitDetails').css('display', 'none')
                }
                else {
                    for (var i = 0; i < OldUnitDetailsValue.length; i++) {
                        $("#tblOldUnitDetails").append("<tr><td>" + OldUnitDetailsValue[i].ProductName + "</td><td>" + OldUnitDetailsValue[i].Quantity + "</td><td>" + OldUnitDetailsValue[i].UnitValue + "</td></tr>");
                    }
                }

                if (AdjustType.length == 0) {
                    $('#divAdjustmentDetails').css('display', 'none')
                }
                else {
                    for (var i = 0; i < AdjustType.length; i++) {
                        $("#tblAdjustmentDetails").append("<tr><td>" + AdjustType[i].DocumentNumber + "</td><td>" + AdjustType[i].DocumentType + "</td><td>" + AdjustType[i].AdjustmentAmount + "</td></tr>");
                    }
                }

                if (PaymentWOCash.length == 0) {
                    $('#tblPosPaymentDetails').css('display', 'none')
                }
                else {
                    for (var i = 0; i < PaymentWOCash.length; i++) {
                        $("#tblPosPaymentDetails").append("<tr><td>" + PaymentWOCash[i].PaymentType + "</td><td>" + PaymentWOCash[i].InstrumentNo + "</td><td>" + PaymentWOCash[i].cardType + "</td><td>"
                            + PaymentWOCash[i].ApprovalCode + "</td><td>" + PaymentWOCash[i].EnterDate + "</td><td>" + PaymentWOCash[i].DraweeDate + "</td><td>" + PaymentWOCash[i].Remarks + "</td><td>" + PaymentWOCash[i].AccountCode + "</td><td>" + PaymentWOCash[i].Amount + "</td></tr>");
                    }
                }

                if (PaymentWithcash.length == 0) {
                    //document.getElementById('lblCash').style.display = 'none'
                    $('#lblAmount').css('display', 'none')
                }
                else if (PaymentWithcash[0].Amount == 0.00) {
                    //document.getElementById('lblCash').style.display = 'none'
                    $('#lblAmount').css('display', 'none')

                }
                else {
                    //document.getElementById('lblCash').style.display = 'block'
                    for (var i = 0; i < PaymentWithcash.length; i++) {
                        $("#lblAmount").append("<tr><td>" + 'Cash : ' + PaymentWithcash[i].Amount + "</td></tr>");
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


                if (PaymentWOCash.length == 0 && (PaymentWithcash.length == 0 || PaymentWithcash[0].Amount == 0.00)) {
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

                    $('#txtAreaBillAddress').text(headerDet.BillingAddress);
                    $('#txtAreaShipAddress').text(headerDet.ShippingAddress);
                    // document.getElementById("lblFinanceChallanDate").innerHTML = headerDet.FinancerChallanDate; 
                    // document.getElementById("txtAreaBillAddress").innerHTML = headerDet.BillingAddress;
                    // document.getElementById("txtAreaShipAddress").innerHTML = headerDet.ShippingAddress;
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
                else {
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
                    $('#divFinanceDetails').css('display', 'none')
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


    for (var i = 0; i < FilteredtaxDetails.length; i++) {
        htmlScript += "<tr><td style='display: none'>" + FilteredtaxDetails[i].ProductTaxId + "</td><td>" + FilteredtaxDetails[i].TaxSchemeName + "</td><td>" + FilteredtaxDetails[i].TaxPercentage + "</td><td>"
                 + FilteredtaxDetails[i].TaxAmount + "</td></tr>";

    }
    htmlScript += ' </table>';
    document.getElementById('PosTaxTable1').innerHTML = htmlScript;
}