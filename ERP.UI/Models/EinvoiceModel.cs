#region//====================================================Revision History=========================================================================
// 1.0   v2.0.40	Priti	10-10-2023	0026890:Error generating IRN
// 2.0   v2.0.41	Priti	20-11-2023	0027000:EInvoice Changes to be done due to the change in the Flynn Version from Ver 1.0 to Ver 3.0 by Vayana
// 3.0   v2.0.43	Priti	18-03-2024	0027317: An error is showing while generating IRN
#endregion//====================================================End Revision History=====================================================================

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP.Models
{
    public class EinvoiceModel
    {
        public EinvoiceModel(string version)
        {
            this.Version = version;
        }

        
        public string Version { get; set; }
        public TrasporterDetails TranDtls { get; set; }
        public DocumentsDetails DocDtls { get; set; }
        public SellerDetails SellerDtls { get; set; }
        public BuyerDetails BuyerDtls { get; set; }
        public DispatchDetails DispDtls { get; set; }
        public ShipToDetails ShipDtls { get; set; }
        public ValueDetails ValDtls { get; set; }
        public ExportDetails ExpDtls { get; set; }
        public EwayBillDetails EwbDtls { get; set; }

        public PaymentDetails PayDtls { get; set; }
        public ReferenceDetails RefDtls { get; set; }
        public List<AdditionalDocumentDetails> AddlDocDtls { get; set; }
        public List<ProductList> ItemList { get; set; }
    }

    
    public class TrasporterDetails
    {
        public string TaxSch { get; set; }
        public string SupTyp { get; set; }
        public string IgstOnIntra { get; set; }
        public string RegRev { get; set; }
        public string EcmGstin { get; set; }

    }


    public class DocumentsDetails
    {
        public string Typ { get; set; }
        public string No { get; set; }
        public string Dt { get; set; }

    }

    public class SellerDetails
    {
        public string Gstin { get; set; }
        public string LglNm { get; set; }
        public string TrdNm { get; set; }
        public string Addr1 { get; set; }
        public string Addr2 { get; set; }
        public string Loc { get; set; }
        public int Pin { get; set; }
        public string Stcd { get; set; }
        public string Ph { get; set; }
        public string Em { get; set; }


    }

    public class BuyerDetails
    {
        public string Gstin { get; set; }
        public string LglNm { get; set; }
        public string TrdNm { get; set; }
        public string Pos { get; set; }
        public string Addr1 { get; set; }
        public string Addr2 { get; set; }
        public string Loc { get; set; }
        public int Pin { get; set; }
        public string Stcd { get; set; }
        public string Ph { get; set; }
        public string Em { get; set; }


    }

    public class ValueDetails
    {
        public decimal AssVal { get; set; }
        public decimal IgstVal { get; set; }
        public decimal CgstVal { get; set; }
        public decimal SgstVal { get; set; }
        public decimal CesVal { get; set; }
        public decimal StCesVal { get; set; }
        public decimal Discount { get; set; }
        public decimal OthChrg { get; set; }
        public decimal RndOffAmt { get; set; }
        public decimal TotInvVal { get; set; }
        public decimal TotInvValFc { get; set; }

    }


    public class ExportDetails
    {
        public string ShipBNo { get; set; }
        public string ShipBDt { get; set; }
        public string Port { get; set; }
        public string RefClm { get; set; }
        public string ForCur { get; set; }
        public string CntCode { get; set; }
        public decimal ExpDuty { get; set; }

    }


    public class DispatchDetails
    {
        public string Nm { get; set; }
        public string Addr1 { get; set; }
        public string Addr2 { get; set; }
        public string Loc { get; set; }
        public int Pin { get; set; }
        public string Stcd { get; set; }

    }

    public class ShipToDetails
    {
        public string Gstin { get; set; }
        public string LglNm { get; set; }
        public string TrdNm { get; set; }
        public string Addr1 { get; set; }
        public string Addr2 { get; set; }
        public string Loc { get; set; }
        public int Pin { get; set; }
        public string Stcd { get; set; }

    }

    public class EwayBillDetails
    {
        public string TransId { get; set; }
        public string TransName { get; set; }
        public string TransMode { get; set; }
        public Int32 Distance { get; set; }
        public string TransDocNo { get; set; }
        public string TransDocDt { get; set; }
        public string VehNo { get; set; }
        public string VehType { get; set; }

    }

    public class PaymentDetails
    {
        public string Nm { get; set; }
        public string AccDet { get; set; }
        public string Mode { get; set; }
        public string FinInsBr { get; set; }
        public string PayTerm { get; set; }
        public string PayInstr { get; set; }
        public string CrTrn { get; set; }
        public string DirDr { get; set; }
        public int CrDay { get; set; }
        public decimal PaidAmt { get; set; }
        public decimal PaymtDue { get; set; }
    }

    public class ReferenceDetails
    {
        public string InvRm { get; set; }
        public DocumentPerdDetails DocPerdDtls { get; set; }
        public List<PrecDocumentDetails> PrecDocDtls { get; set; }
        public List<ContractDetails> ContrDtls { get; set; }
    }

    public class DocumentPerdDetails
    {
        public string InvStDt { get; set; }
        public string InvEndDt { get; set; }
    }

    public class PrecDocumentDetails
    {
        public string InvNo { get; set; }
        public string InvDt { get; set; }
        public string OthRefNo { get; set; }

    }

    public class ContractDetails
    {
        public string RecAdvRefr { get; set; }
        public string RecAdvDt { get; set; }
        public string TendRefr { get; set; }
        public string ContrRefr { get; set; }
        public string ExtRefr { get; set; }
        public string ProjRefr { get; set; }
        public string PORefr { get; set; }
        public string PORefDt { get; set; }

    }

    public class AdditionalDocumentDetails
    {
        public string Url { get; set; }
        public string Docs { get; set; }
        public string Info { get; set; }

    }

    public class ProductList
    {
        public string SlNo { get; set; }
        public string PrdDesc { get; set; }
        public string IsServc { get; set; }
        public string HsnCd { get; set; }
        public string Barcde { get; set; }
        public decimal Qty { get; set; }
        public decimal FreeQty { get; set; }
        public string Unit { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotAmt { get; set; }
        public decimal Discount { get; set; }
        public decimal PreTaxVal { get; set; }
        public decimal AssAmt { get; set; }
        public decimal GstRt { get; set; }
        public decimal IgstAmt { get; set; }
        public decimal CgstAmt { get; set; }
        public decimal SgstAmt { get; set; }
        public decimal CesRt { get; set; }
        public decimal CesAmt { get; set; }
        public decimal CesNonAdvlAmt { get; set; }
        public decimal StateCesRt { get; set; }
        public decimal StateCesAmt { get; set; }
        public decimal StateCesNonAdvlAmt { get; set; }
        public decimal OthChrg { get; set; }
        public decimal TotItemVal { get; set; }
        public string OrdLineRef { get; set; }
        public string OrgCntry { get; set; }
        public string PrdSlNo { get; set; }
        public string BchDtls { get; set; }
        public List<AttributeDetails> AttribDtls { get; set; }

    }

    public class AttributeDetails
    {
        public string Nm { get; set; }
        public string Val { get; set; }

    }


    public class EinvoiceCancel
    {
        public string Version { get; set; }
        public EinvoiceCancel(string ver)
        {
            this.Version = ver;
        }

        public List<CancelDetails> Canceldtls { get; set; }
    }

    public class CancelDetails
    {
        public string Irn { get; set; }
        public string CnlRsn { get; set; }
        public string CnlRem { get; set; }
    }
    /// <summary>
    ///"handle": "{{ email-set-during-step-1 }}",
    ///"password": "{{ password-set-during-from-step-2 }}",
    ///"handleType": "email"
    /// </summary>
    public class authtokensInput
    {
        public string handle { get; set; }
        public string password { get; set; }
        public string handleType { get; set; }
        public authtokensInput(string email, string pass)
        {
            this.handleType = "email";
            this.handle = email;
            this.password = pass;
        }

    }

    public class authtokensOutput
    {
        public authtokensData data { get; set; }
    }
    public class authtokensData
    {
        public string token { get; set; }
        public string userId { get; set; }
        public Int64 expiry { get; set; }
        public string email { get; set; }
        public string mobile { get; set; }
        public string givenName { get; set; }
        public string state { get; set; }
        public string verificationStatus { get; set; }
        public int? passwordLastModified { get; set; }
        public List<authtokensassociatedOrgs> associatedOrgs { get; set; }

        public string timezone { get; set; }
        public string locale { get; set; }
        public Int64 createdOn { get; set; }
        //public Int64 lastUpdated { get; set; }

        public DateTime? lastUpdated { get; set; }


    }

    public class authtokensassociatedOrgs
    {
        public authtokenorganisation organisation { get; set; }
        public authtokenuserAccessInfo userAccessInfo { get; set; }
    }

    public class authtokenorganisation
    {
        public string id { get; set; }
        public string name { get; set; }
        public string taxIdentifier { get; set; }
        public string taxIdentifierType { get; set; }
        public string country { get; set; }
        public List<authtokenservices> services { get; set; }

    }

    public class authtokenservices
    {
        public string serviceCode { get; set; }
        public string serviceName { get; set; }

    }

    public class authtokenuserAccessInfo
    {
        public string primary { get; set; }
        public string admin { get; set; }
    }

    public class CreateOrgInput
    {
        public string name { get; set; }
        public string taxIdentifier { get; set; }
        public string taxIdentifierType { get; set; }
        public string country { get; set; }
        public string services { get; set; }
        public string timezone { get; set; }
        public string locale { get; set; }

        public CreateOrgInput(string org_name, string org_pan_no)
        {
            this.country = "India";
            this.locale = "en-IN";
            this.name = org_name;
            this.services = "eapi";
            this.taxIdentifier = org_pan_no;
            this.taxIdentifierType = "PAN";
            this.timezone = "Asia/Kolkata";

        }
    }

    public class CreateOrgOutput
    {
        public string id { get; set; }
        public string name { get; set; }
        public string taxIdentifier { get; set; }
        public string taxIdentifierType { get; set; }
        public string country { get; set; }
        public bool enabled { get; set; }
        public List<authtokenservices> services { get; set; }
        public string timezone { get; set; }
        public string locale { get; set; }
        public int createdOn { get; set; }
        public int lastUpdated { get; set; }


    }

    //Rev 2.0
    public class IRNV3
    {
        public string status { get; set; }
        public IRNDetails data { get; set; }

        public string error { get; set; }
        public List<infologV3> info { get; set; }

        public string additionalInfo { get; set; }
        public string alert { get; set; }       

    }

    public class infologV3
    {
        public string InfCd { get; set; }
        public infologV3Desc Desc { get; set; }

    }

    //Rev 2.0 End

    //Rev 3.0
    public class EWAYBILLV3
    {
        public string status { get; set; }
        public IRNDetails data { get; set; }

        public string error { get; set; }
        public List<EWAYBILLinfologV3> info { get; set; }

        public string additionalInfo { get; set; }
        public string alert { get; set; }

    }

    public class EWAYBILLinfologV3
    {
        public string InfCd { get; set; }
        public string Desc { get; set; }

    }
    //Rev 3.0
    public class IRN
    {        
        public string data { get; set; }
        
    }

    public class IRNEnrich
    {
        public TaskModel data { get; set; }
    }
    //Rev 2.0
    public class IRNDetails
    {
        public string AckNo { get; set; }
        public string AckDt { get; set; }
        public string Irn { get; set; }
        public string SignedInvoice { get; set; }
        public string SignedQRCode { get; set; }
        public string Status { get; set; }
        public string EwbNo { get; set; }
        public string EwbDt { get; set; }
        public string EwbValidTill { get; set; }
        public string Remarks { get; set; }
        public string CancelDate { get; set; }
        public string transporterId { get; set; }

    }

    public class IRNERRORV3
    {
        public string status { get; set; }
        public string data { get; set; }

        public errorV3 error { get; set; }
        public List<infologV3> info { get; set; }

        public detailsV3 additionalInfo { get; set; }
        public string alert { get; set; }



    }
    public class infologV3Desc
    {
        public string AckNo { get; set; }
        public string AckDt { get; set; }
        public string Irn { get; set; }

    }
   

    //Rev 2.0 End

    public class CancelIRNOutput
    {
        public string Irn { get; set; }
        public string CancelDate { get; set; }

    }

    public class TaskModel
    {
        [JsonProperty("task-id")]
        public string task_id { get; set; }
    }

    

    public class InfoDtls
    {
        public string InfCd { get; set; }
        public Desc Desc { get; set; }
        public string Remarks { get; set; }

    }
    public class Desc
    {
        public string AckNo { get; set; }
        public string AckDt { get; set; }
        public string Irn { get; set; }
    }

    public class Details
    {
        public string AckNo { get; set; }
        public string AckDt { get; set; }
    }

    #region Enrich

    public class Enrich
    {
        public List<EinvoiceModelEnrich> payload { get; set; }
        public meta meta { get; set; }
    }

    public class meta
    {
        public string generatePdf { get; set; }
        public List<string> emailRecipientList { get; set; }
    }
    public class EinvoiceModelEnrich
    {
        public EinvoiceModelEnrich(string version)
        {
            this.Version = version;
        }
        public string Irn { get; set; }
        public string Version { get; set; }
        public TrasporterDetailsEnrich TranDtls { get; set; }
        public DocumentsDetailsEnrich DocDtls { get; set; }
        public SellerDetailsEnrich SellerDtls { get; set; }
        public BuyerDetailsEnrich BuyerDtls { get; set; }
        public DispatchDetailsEnrich DispDtls { get; set; }
        public ShipToDetailsEnrich ShipDtls { get; set; }
        public ValueDetailsEnrich ValDtls { get; set; }
        public ExportDetailsEnrich ExpDtls { get; set; }
        public PaymentDetailsEnrich PayDtls { get; set; }
        public ReferenceDetailsEnrich RefDtls { get; set; }
        public List<ProductListEnrich> ItemList { get; set; }
    }


    public class TrasporterDetailsEnrich
    {
        public string TaxSch { get; set; }
        public string SupTyp { get; set; }
        public string IgstOnIntra { get; set; }
        public string RegRev { get; set; }
    }


    public class DocumentsDetailsEnrich
    {
        public string Typ { get; set; }
        public string No { get; set; }
        public string Dt { get; set; }

    }

    public class SellerDetailsEnrich
    {
        public string Gstin { get; set; }
        public string LglNm { get; set; }
        public string TrdNm { get; set; }
        public string Addr1 { get; set; }
        public string Loc { get; set; }
        public int Pin { get; set; }
        public string Stcd { get; set; }


    }

    public class BuyerDetailsEnrich
    {
        public string Gstin { get; set; }
        public string LglNm { get; set; }
        public string Pos { get; set; }
        public string Addr1 { get; set; }
        public string Loc { get; set; }
        public int Pin { get; set; }
        public string Stcd { get; set; }

    }

    public class ValueDetailsEnrich
    {
        public decimal AssVal { get; set; }
        public decimal IgstVal { get; set; }
        public decimal CgstVal { get; set; }
        public decimal SgstVal { get; set; }
        public decimal TotInvVal { get; set; }

    }


    public class ExportDetailsEnrich
    {
        public string ShipBNo { get; set; }
        public string ShipBDt { get; set; }
        public string Port { get; set; }
        public string RefClm { get; set; }
        public string ForCur { get; set; }
        public string CntCode { get; set; }
        public decimal ExpDuty { get; set; }

    }


    public class DispatchDetailsEnrich
    {
        public string Nm { get; set; }
        public string Addr1 { get; set; }
        public string Addr2 { get; set; }
        public string Loc { get; set; }
        public int Pin { get; set; }
        public string Stcd { get; set; }

    }

    public class ShipToDetailsEnrich
    {
        public string Gstin { get; set; }
        public string LglNm { get; set; }
        public string TrdNm { get; set; }
        public string Addr1 { get; set; }
        public string Addr2 { get; set; }
        public string Loc { get; set; }
        public int Pin { get; set; }
        public string Stcd { get; set; }

    }



    public class PaymentDetailsEnrich
    {
        public string Nm { get; set; }
        public string Mode { get; set; }
        public string PayTerm { get; set; }
        public string PayInstr { get; set; }
        public string CrTrn { get; set; }
        public string DirDr { get; set; }
        public int CrDay { get; set; }
        public decimal PaidAmt { get; set; }
        public decimal PaymtDue { get; set; }
    }

    public class ReferenceDetailsEnrich
    {
        public string InvRm { get; set; }
        public DocumentPerdDetailsEnrich DocPerdDtls { get; set; }

    }

    public class DocumentPerdDetailsEnrich
    {
        public string InvStDt { get; set; }
        public string InvEndDt { get; set; }
    }



    public class ProductListEnrich
    {
        public string SlNo { get; set; }
        public string PrdDesc { get; set; }
        public string IsServc { get; set; }
        public string HsnCd { get; set; }
        public string Barcde { get; set; }
        public decimal Qty { get; set; }
        public decimal FreeQty { get; set; }
        public string Unit { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotAmt { get; set; }
        public decimal Discount { get; set; }
        public decimal PreTaxVal { get; set; }
        public decimal AssAmt { get; set; }
        public decimal GstRt { get; set; }
        public decimal IgstAmt { get; set; }
        public decimal CgstAmt { get; set; }
        public decimal SgstAmt { get; set; }
        public decimal CesRt { get; set; }
        public decimal CesAmt { get; set; }
        public decimal CesNonAdvlAmt { get; set; }
        public decimal StateCesRt { get; set; }
        public decimal StateCesAmt { get; set; }
        public decimal StateCesNonAdvlAmt { get; set; }
        public decimal OthChrg { get; set; }
        public decimal TotItemVal { get; set; }
        public string OrdLineRef { get; set; }
        public string OrgCntry { get; set; }
        public string PrdSlNo { get; set; }
        public string BchDtls { get; set; }
        public List<AttributeDetailsEnrich> AttribDtls { get; set; }

    }

    public class AttributeDetailsEnrich
    {
        public string Nm { get; set; }
        public string Val { get; set; }

    }

    public class EinvoiceError
    {
        public error error { get; set; }
    }

    public class error
    {
        public string message { get; set; }
        public string type { get; set; }
        public errorDetails args { get; set; }
    }

    public class EinvoiceErrorV3
    {
        public errorV3 error { get; set; }
        
    }
    public class errorV3
    {
        public string message { get; set; }
        public string type { get; set; }
        public irpErrorV3 args { get; set; }
    }
    public class irpErrorV3
    {
        public List<errorlog> details { get; set; }        
        public List<infolog> info { get; set; }
        public additionalDetailslog additionalDetails { get; set; }
        public string alert { get; set; }
        
    }
    public class errorDetails
    {
        [JsonProperty("irp-err")]
        public irpError irp_error { get; set; }
    }

    public class ClientEinvoiceError
    {
        public Clienterror error { get; set; }
    }

    public class Clienterror
    {
        public string message { get; set; }
        public string type3 { get; set; }
        public ClientErrors args { get; set; }
    }


    public class ClientErrors
    {
        public List<string> errors { get; set; }
    }

    public class irpError
    {
        public List<errorlog> details { get; set; }
        public string data { get; set; }        
        public List<infolog> info { get; set; }      
        public additionalDetailslog additionalDetails { get; set; }
        
    }
    public class detailsV3
    {
        public detailsInfoV3 details { get; set; }
    }
        public class detailsInfoV3
    {
       
        public string AckNo { get; set; }
        public string AckDt { get; set; }
        public string Irn { get; set; }
        public string SignedInvoice { get; set; }
        public string SignedQRCode { get; set; }
        public string Status { get; set; }
        public string EwbNo { get; set; }
        public string EwbDt { get; set; }
        public string EwbValidTill { get; set; }
        public string Remarks { get; set; }
    }
    public class additionalDetailslog
    {
          [JsonProperty("additionalDetailslog")]
        public string AckNo { get; set; }
        public string AckDt { get; set; }
        public string Irn { get; set; }
        public string SignedInvoice { get; set; }
        public string SignedQRCode { get; set; }        
        public string Status { get; set; }
        public string EwbNo { get; set; }
        public string EwbDt { get; set; }
        public string EwbValidTill { get; set; }
        public string Remarks { get; set; }
    }

    public class infolog
    {
        public string InfCd { get; set; }

        //REV 1.0
        //public string Desc { get; set; }   
        public InfologDesc Desc { get; set; }
        //REV 1.0
        public InfoDesclog InfoDesc { get; set; }
    }
    //REV 1.0
    public class InfologDesc
    {
        public string AckNo { get; set; }
        public string AckDt { get; set; }
        public string Irn { get; set; }

    }
    //REV 1.0 END
    public class InfoDesclog
    {
        public string AckNo { get; set; }
        public string AckDt { get; set; }
        public string Irn { get; set; }

    }

    public class errorlog
    {
        public string ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
    }





    #endregion


    #region Ewaybill
    public class EwayBillGeneration
    {
        public string Irn { get; set; }
        public Int32 Distance { get; set; }
        public string TransMode { get; set; }
        public string TransId { get; set; }
        public string TransName { get; set; }
        public string TransDocDt { get; set; }
        public string TransDocNo { get; set; }
        public string VehNo { get; set; }
        public string VehType { get; set; }


    }

    public class CancelEwayBill
    {
        public Int64 ewbNo { get; set; }
        public Int32 cancelRsnCode { get; set; }
        public string cancelRmrk { get; set; }

    }

    public class CancelEwayBillOutput
    {
        public string ewayBillNo { get; set; }
        public string cancelDate { get; set; }

    }


    public class UpdateEwayBill
    {
        public Int64 ewbNo { get; set; }
        public string TransMode { get; set; }
        public string transDocNo { get; set; }
        public string transDocDate { get; set; }
        public string fromPlace { get; set; }
        public string fromState { get; set; }
        public string vehicleNo { get; set; }
        public string VehType { get; set; }
        public string reasonCode { get; set; }
        public string reasonRem { get; set; }

    }
    public class UpdateEwayBillTransporter
    {
        public string ewbNo { get; set; }
        public string transporterId { get; set; }

    }


    #endregion


}