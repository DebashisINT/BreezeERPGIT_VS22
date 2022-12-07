using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP.Models
{
    public class EinvoiceModelWebtel
    {
        public EinvoiceModelWebtel(string version)
        {
            this.Version = version;
        }

        public string CDKey { get; set; }
        public string EInvUserName { get; set; }
        public string EInvPassword { get; set; }
        public string EFUserName { get; set; }
        public string EFPassword { get; set; }
        public string GSTIN { get; set; }
        public string GetQRImg { get; set; }
        public string GetSignedInvoice { get; set; }
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

    public class webtelIRNDetails
    {
        public string ErrorMessage { get; set; }
        public string ErrorCode { get; set; }
        public string Status { get; set; }
        public string GSTIN { get; set; }
        public string DocNo { get; set; }
        public string DocType { get; set; }
        public string DocDate { get; set; }
        public string Irn { get; set; }
        public string AckDate { get; set; }
        public string AckNo { get; set; }
        public string EwbNo { get; set; }
        public string EwbDt { get; set; }
        public string EwbValidTill { get; set; }
        public string SignedInvoice { get; set; }
        public string SignedQRCode { get; set; }
        public string IrnStatus { get; set; }
        public InfoDtls InfoDtls { get; set; }
        public string Remarks { get; set; }


    }

    public class WebTelCancelDetails
    {
        public string Irn { get; set; }
        public string GSTIN { get; set; }
        public string CnlRsn { get; set; }
        public string CnlRem { get; set; }
        public string CDKey { get; set; }
        public string EFUserName { get; set; }
        public string EFPassword { get; set; }
        public string EInvUserName { get; set; }
        public string EInvPassword { get; set; }       
    }

    public class Push_Data_List
    {
        public List<WebTelCancelDetails> Data { get; set; }
    }


    public class CancelList
    {
        public Push_Data_List Push_Data_List { get; set; }
    }

    public class EwayBillGenerationWebTel
    {
        public List<Push_Data_ListEwayBillGeneration> Push_Data_List { get; set; }
    }
    public class Push_Data_ListEwayBillGeneration
    {
        public string Irn { get; set; }
        public string TransMode { get; set; }
        public string Transid { get; set; }
        public string Transname { get; set; }
        public Int32 Distance { get; set; }
        public string Transdocno { get; set; }
        public string TransdocDt { get; set; }
        public string VehNo { get; set; }
        public string VehType { get; set; }

        public string ShipFrom_Nm { get; set; }
        public string ShipFrom_Addr1 { get; set; }
        public string ShipFrom_Addr2 { get; set; }
        public string ShipFrom_Loc { get; set; }
        public Int32 ShipFrom_Pin { get; set; }
        public string ShipFrom_Stcd { get; set; }

        public string ShipTo_Addr1 { get; set; }
        public string ShipTo_Addr2 { get; set; }
        public string ShipTo_Loc { get; set; }
        public Int32 ShipTo_Pin { get; set; }
        public string ShipTo_Stcd { get; set; }

        public string CDKey { get; set; }
       
        public string EWbUserName { get; set; }
        public string EWbPassword { get; set; }
        public string EFUserName { get; set; }
        public string EFPassword { get; set; }

        public string GSTIN { get; set; }
    }
}