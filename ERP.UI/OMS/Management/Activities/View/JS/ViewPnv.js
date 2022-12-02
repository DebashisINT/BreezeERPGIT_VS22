//$(document).ready(function () {

//    var full_url = document.URL;
//    var url_array = full_url.split('?id=');
//    var id = url_array[1];
//    GetPnvHeaderDetails(id);
//})

function GetPnvHeaderDetails(id)  // Get Purchase invoice header details
{
    $.ajax({
        type: "POST",
        url: "services/PnvService.asmx/GetPnvHeaderDetails",
        data: JSON.stringify({ 'id': id }),
        async: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            if (response.d) {
                if (response.d.msg == "ok") {


                    var header_details = response.d.header;
                    var pnv_details = response.d.PnvDetails;
                    console.log(header_details.InvoiceNumber);

                    $("#lbldoc_no").text(header_details.InvoiceNumber);
                    $("#lbl_for_unit").text(header_details.BranchDescription);
                    $("#lbl_vendor").text(header_details.VendorName);
                    $("#lbl_party_inv_no").text(header_details.PartyInvoiceNo);
                    $("#lbl_ref").text(header_details.Invoice_Reference);
                    $("#lbl_party_inv_dt").text(header_details.invoice_date);
                    $("#lbl_contact").text(header_details.contact_name);
                    $("#lbl_currency").text(header_details.Currency_AlphaCode);
                    $("#lbl_rate").text(header_details.Currency_Conversion_Rate);
                    $("#lbl_reverse_mchnsm").text(header_details.Invoice_ReverseMechanism);
                    $("#lbl_amnts_are").text(header_details.Amounts_are);
                    $("#lbl_eway_bill_no").text(header_details.EWayBillNumber);
                    $("#lbl_remarks").text(header_details.Invoice_Remarks);
                    $("#lbl_entry_dt").text(header_details.Entry_date);

                    //console.log(response.d.InvoiceNumber);
                    //console.log(response.d.BranchDescription);
                    //console.log(response.d.invoice_date);
                    //console.log(response.d.VendorName);
                    //console.log(response.d.PartyInvoiceNo);
                    //console.log(response.d.Invoice_Reference);
                    //console.log(response.d.contact_name);
                    //console.log(response.d.Currency_AlphaCode);
                    //console.log(response.d.Currency_Conversion_Rate);
                    //console.log(response.d.Invoice_ReverseMechanism);
                    //console.log(response.d.Amounts_are);
                    //console.log(response.d.Entry_date);
                }
            } else {
                alert(response.d.msg);
            }
        },
        error: function (response) {

            // console.log(response);
        }
    });

}


var app = angular.module("myApp", []);
app.controller("myCtrl", function ($scope, $http) {
    var full_url = document.URL;
    var url_array = full_url.split('?id=');
    var id = url_array[1];
    $scope.value = '';
    //alert(id);
    //$http.post("services/PnvService.asmx/GetPnvHeaderDetails?id="+id).then(function (response) {
    //    console.log(response.data);
    //   // console.log($scope.Emps);
    //});

    var obj = {};
    obj.id = id;
    $scope.pnv_header_details = [];
    $scope.tax_details = [];
    $scope.PurchaseInvoiceTax = [];
    $scope.detailsid = '';

    $http({

        method: "POST",
        url: "services/PnvService.asmx/GetPnvHeaderDetails",
        dataType: 'json',
        data: JSON.stringify(obj),
        //params: { date:"a"},

        headers: { "Content-Type": "application/json" }

    }).then(function (response) {
        if (response.data.d) {
            if (response.data.d.msg == "ok") {
                
                $scope.lbl_type = response.data.d.header.invoice_type;
                $scope.lbldoc_no = response.data.d.header.InvoiceNumber;
                $scope.lbl_inv_dt = response.data.d.header.party_invoic_dt;
                $scope.lbl_for_unit = response.data.d.header.BranchDescription;
                $scope.lbl_vendor = response.data.d.header.VendorName;
                $scope.lbl_party_inv_no = response.data.d.header.PartyInvoiceNo;
                $scope.lbl_ref = response.data.d.header.Invoice_Reference;
                $scope.lbl_party_inv_dt = response.data.d.header.invoice_date;
                $scope.lbl_contact = response.data.d.header.contact_name;
                $scope.lbl_currency = response.data.d.header.Currency_AlphaCode;
                $scope.lbl_Rate = response.data.d.header.Currency_Conversion_Rate;
                $scope.lbl_reverse_mchnsm = response.data.d.header.Invoice_ReverseMechanism;
                $scope.lbl_amnts_are = response.data.d.header.Amounts_are;
                $scope.lbl_eway_bill_no = response.data.d.header.EWayBillNumber;
                $scope.lbl_remarks = response.data.d.header.Invoice_Remarks;
                $scope.lbl_entry_dt = response.data.d.header.Entry_date;
                $scope.shipping_addr = response.data.d.header.shipping_address;
                $scope.billing_addr = response.data.d.header.billing_address;
                $scope.value = response.data.d.header.value;
                $scope.lbl_grn = response.data.d.header.grn_no;

                $scope.pnv_header_details = response.data.d.PnvDetails;
                $scope.tax_details = response.data.d.TaxDetails;
                $scope.PurchaseInvoiceTax = response.data.d.PnvTaxDetails;
                if (response.data.d.pnvtermscondition) {
                    if ($scope.lbl_amnts_are == 'Import') {
                        $scope.lbl_imprt_type = response.data.d.pnvtermscondition.type_of_import;
                        $scope.lbl_pymnt_trm_rmrks = response.data.d.pnvtermscondition.pymnt_trm_rmrk;
                        $scope.lbl_inco_term = response.data.d.pnvtermscondition.inco_dlvry_trm;
                        $scope.lbl_inco_trm_rmrks = response.data.d.pnvtermscondition.inco_dlvry_trm_rmrks;
                        $scope.lbl_shpmnt_schedule = response.data.d.pnvtermscondition.shpmnt_scedule;
                        $scope.port_shpmnt = response.data.d.pnvtermscondition.Port_Description;
                        $scope.port_dstntn = response.data.d.pnvtermscondition.PortOfDestination;
                        $scope.lbl_be_nmbr = response.data.d.pnvtermscondition.BE_Number;
                        $scope.lbl_be_dt = response.data.d.pnvtermscondition.bedt;
                        $scope.lbl_be_value = response.data.d.pnvtermscondition.BE_Value;
                        $scope.lbl_partial_shpmnt = response.data.d.pnvtermscondition.partial_shipment;
                        $scope.lbl_trns_shpmnt = response.data.d.pnvtermscondition.Transshipment;
                        $scope.lbl_pkng_spc = response.data.d.pnvtermscondition.PackingSpec;
                        $scope.lbl_val_ordr = response.data.d.pnvtermscondition.val_ordr_dt;
                        $scope.lbl_val_ordr_rmrks = response.data.d.pnvtermscondition.ValidityOfOrderRemarks;
                        $scope.lbl_cntry_origin = response.data.d.pnvtermscondition.cou_country;
                        $scope.lbl_f_dstntn_prd = response.data.d.pnvtermscondition.FreeDetentionPeriod;
                        $scope.lbl_f_dstntn_prd_rmrks = response.data.d.pnvtermscondition.FreeDetentionPeriodRemark;
                        $scope.lbl_bnk_nm = response.data.d.pnvtermscondition.bnk_bankName;
                        $scope.lbl_bnk_brnch_nm = response.data.d.pnvtermscondition.Bank_Branch;
                        $scope.lbl_bnk_brnch_addr = response.data.d.pnvtermscondition.Bank_Address;
                        $scope.lbl_bnk_brnch_lnd_mrk = response.data.d.pnvtermscondition.Bank_Landmark;
                        $scope.lbl_bnk_brnch_pin = response.data.d.pnvtermscondition.Bank_Pin;
                        $scope.lbl_bnk_acnt_nmbr = response.data.d.pnvtermscondition.Bank_AcNo;
                        $scope.lbl_bnk_swft_code = response.data.d.pnvtermscondition.Bank_SwiftCode;
                        $scope.lbl_bnk_rtgs = response.data.d.pnvtermscondition.Bank_RTGSCode;
                        $scope.lbl_bnk_ifsc = response.data.d.pnvtermscondition.Bank_IFSCCode;
                        $scope.lbl_bnk_rmrk = response.data.d.pnvtermscondition.Bank_Remarks;
                    }
                    else {
                        
                        $scope.lbl_delvrydt = response.data.d.pnvtermscondition.delvrydt;
                        $scope.lbl_dlvryrmrk = response.data.d.pnvtermscondition.dlvryrmrk;
                        $scope.lbl_insurance_cvrg = response.data.d.pnvtermscondition.insrnc_cvrg;
                        $scope.lbl_frght_chrgs = response.data.d.pnvtermscondition.freight_chrgs;
                        $scope.lbl_freight_rmrk = response.data.d.pnvtermscondition.freight_rmrks;
                        $scope.lbl_e_payment = response.data.d.pnvtermscondition.PermitValue;
                        $scope.lbl_othr_rmrk = response.data.d.pnvtermscondition.Remarks;
                        $scope.lbl_tst_cert_req = response.data.d.pnvtermscondition.cert_req;
                        $scope.lbl_dlvry_dtls = response.data.d.pnvtermscondition.dlvr_dtls;
                        $scope.lbl_trnsprt_nm = response.data.d.pnvtermscondition.TransporterName;
                        $scope.lbl_dscnt_rcvl = response.data.d.pnvtermscondition.discnt_rcv;
                        $scope.lbl_dscnt_dtls = response.data.d.pnvtermscondition.dscnt_rcv_dtls;
                        $scope.lbl_cmsn_rcvl = response.data.d.pnvtermscondition.commission_rcv;
                        $scope.lbl_cmsn_dtls = response.data.d.pnvtermscondition.c_rcv_dtls;
                        $scope.lbl_cmsn_rate = response.data.d.pnvtermscondition.CommissionRate;
                        $scope.lbl_bnk_nm = response.data.d.pnvtermscondition.bnk_bankName;
                        $scope.lbl_bnk_brnch_nm = response.data.d.pnvtermscondition.Bank_Branch;
                        $scope.lbl_bnk_brnch_addr = response.data.d.pnvtermscondition.Bank_Address;
                        $scope.lbl_bnk_brnch_lnd_mrk = response.data.d.pnvtermscondition.Bank_Landmark;
                        $scope.lbl_bnk_brnch_pin = response.data.d.pnvtermscondition.Bank_Pin;
                        $scope.lbl_bnk_acnt_nmbr = response.data.d.pnvtermscondition.Bank_AcNo;
                        $scope.lbl_bnk_swft_code = response.data.d.pnvtermscondition.Bank_SwiftCode;
                        $scope.lbl_bnk_rtgs = response.data.d.pnvtermscondition.Bank_RTGSCode;
                        $scope.lbl_bnk_ifsc = response.data.d.pnvtermscondition.Bank_IFSCCode;
                        $scope.lbl_bnk_rmrk = response.data.d.pnvtermscondition.Bank_Remarks;
                        //Rev work start 15.06.2022 Mantise Issue:24952 Transporter Tab is missing in the Purchase Invoice view mode
                        
                        $scope.lbl_Transporter = response.data.d.pnvTransport.TRANSAPORTERNAME;
                        $scope.lbl_FinalTransporter = response.data.d.pnvTransport.FINALTRANSPORTER;
                        $scope.lbl_TransporterType = response.data.d.pnvTransport.LGL_LEGALSTATUS;                        
                        $scope.lbl_registercheck = response.data.d.pnvTransport.TRP_ISREGISTERED;
                        $scope.lbl_gstn = response.data.d.pnvTransport.TRP_GSTIN;
                        $scope.lbl_vehicleno = response.data.d.pnvTransport.TRPVEH_VECHILESNOS;
                        $scope.lbl_Freight = response.data.d.pnvTransport.FREIGHTCHARGE;
                        $scope.lbl_Point = response.data.d.pnvTransport.LOCATIONPOINT;

                        $scope.lbl_Loading = response.data.d.pnvTransport.LOADINGCHARGE;
                        $scope.lbl_Unloading = response.data.d.pnvTransport.UNLOADINGCHARGE;
                        $scope.lbl_Parking = response.data.d.pnvTransport.PARKINGCHARGE;
                        $scope.lbl_Weighment = response.data.d.pnvTransport.WEIGHT;
                        $scope.lbl_TollTax = response.data.d.pnvTransport.TOLLTAX;
                        $scope.lbl_ServiceTaxes = response.data.d.pnvTransport.TRP_SERVICETAXES;
                        $scope.lbl_TotalCharges = response.data.d.pnvTransport.TOTALCHARGES;

                        $scope.lbl_DistanceDelvChallan = response.data.d.pnvTransport.DISTANCE;
                        $scope.lbl_Lrno = response.data.d.pnvTransport.LRNO;
                        $scope.lbl_VehicleOutDate = response.data.d.pnvTransport.VEHICLEOUTDATE;
                        $scope.lbl_VehType = response.data.d.pnvTransport.VEHICLE_TYPE;
                        $scope.lbl_TransportMode = response.data.d.pnvTransport.TRANSPORTER_MODE;
                        $scope.lbl_Address = response.data.d.pnvTransport.TRP_ADDRESS;

                        $scope.lbl_Country = response.data.d.pnvTransport.COU_COUNTRY;
                        $scope.lbl_State = response.data.d.pnvTransport.STATE;
                        $scope.lbl_City = response.data.d.pnvTransport.CITY_NAME;
                        $scope.lbl_Pin = response.data.d.pnvTransport.PIN_CODE;
                        $scope.lbl_Area = response.data.d.pnvTransport.AREA_NAME;
                        $scope.lbl_PhoneNo = response.data.d.pnvTransport.TRP_PHONE;
                        //Rev work close 15.06.2022 Mantise Issue:24952 Transporter Tab is missing in the Purchase Invoice view mode
                    }


                }
            }

        }
        else {
            alert(response.data.d.msg);
        }




        console.log(response.data);

    });

    $scope.show = function (id) {
        $scope.detailsid = id;
        $('#TimeUpdateModel').modal('show');
    }

    $scope.trms_show = function () {
        // alert(1)
        $('#TimeUpdateModel1').modal('show');
    }
    //Rev work start 15.06.2022 Mantise Issue:24952 Transporter Tab is missing in the Purchase Invoice view mode
    $scope.transport_show = function () {        
        $('#TranspoterUpdateModel').modal('show');
    }
    //Rev work close 15.06.2022 Mantise Issue:24952 Transporter Tab is missing in the Purchase Invoice view mode
});

