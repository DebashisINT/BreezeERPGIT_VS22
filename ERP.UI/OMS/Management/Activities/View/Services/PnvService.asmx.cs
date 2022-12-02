using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;

namespace ERP.OMS.Management.Activities.View.Services
{
    /// <summary>
    /// Summary description for PnvService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [System.Web.Script.Services.ScriptService]
    public class PnvService : System.Web.Services.WebService
    {
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetPnvHeaderDetails(string id)
        {
            PurchaseInvoice return_obj = new PurchaseInvoice();

            List<tax_Details> _tax_Details = new List<tax_Details>();
            List<pnv_Details> _pnv_header_Details = new List<pnv_Details>();
            List<PurchaseInvoiceTax> _PurchaseInvoiceTax = new List<PurchaseInvoiceTax>();
            pnv_header_Details ob = new pnv_header_Details();
            TermsCondition _TermsCondition = new TermsCondition();
            //Rev work start 15.06.2022 Mantise Issue:24952 Transporter Tab is missing in the Purchase Invoice view mode
            Transport _Trnsp = new Transport();
            //Rev work close 15.06.2022 Mantise Issue:24952 Transporter Tab is missing in the Purchase Invoice view mode
            try
            {


                if (HttpContext.Current.Session["userid"] != null)
                {
                    ProcedureExecute proc = new ProcedureExecute("prc_view_pnv_details");
                    proc.AddVarcharPara("@Action", 50, "get_pnv_header_dtls");
                    proc.AddVarcharPara("@id", 50, id);
                    DataSet ds = proc.GetDataSet();
                    ob.InvoiceNumber = ds.Tables[0].Rows[0]["doument_no"].ToString().Trim();
                    ob.BranchDescription = ds.Tables[0].Rows[0]["branch_description"].ToString().Trim();
                    ob.invoice_date = ds.Tables[0].Rows[0]["invoice_date"].ToString().Trim();
                    ob.VendorName = ds.Tables[0].Rows[0]["VendorName"].ToString().Trim();
                    ob.PartyInvoiceNo = ds.Tables[0].Rows[0]["PartyInvoiceNo"].ToString().Trim();
                    ob.Invoice_Reference = ds.Tables[0].Rows[0]["Invoice_Reference"].ToString().Trim();
                    ob.contact_name = ds.Tables[0].Rows[0]["cp_name"].ToString().Trim();
                    ob.Currency_AlphaCode = ds.Tables[0].Rows[0]["Currency_AlphaCode"].ToString().Trim();
                    ob.Currency_Conversion_Rate = ds.Tables[0].Rows[0]["Currency_Conversion_Rate"].ToString().Trim();
                    ob.Invoice_ReverseMechanism = ds.Tables[0].Rows[0]["Invoice_ReverseMechanism"].ToString().Trim();
                    ob.Amounts_are = ds.Tables[0].Rows[0]["Amounts are"].ToString().Trim();
                    ob.EWayBillNumber = ds.Tables[0].Rows[0]["EWayBillNumber"].ToString().Trim();
                    ob.Entry_date = ds.Tables[0].Rows[0]["Entry date"].ToString().Trim();
                    ob.Invoice_Remarks = ds.Tables[0].Rows[0]["Invoice_Remarks"].ToString().Trim();
                    ob.value = ds.Tables[0].Rows[0]["varaible_value"].ToString().Trim();
                    ob.billing_address = ds.Tables[0].Rows[0]["billing_address"].ToString().Trim();
                    ob.shipping_address = ds.Tables[0].Rows[0]["shipping_address"].ToString().Trim();
                    ob.grn_no = ds.Tables[0].Rows[0]["grn_no"].ToString().Trim();
                    ob.invoice_type = ds.Tables[0].Rows[0]["invoice_type"].ToString().Trim();
                    ob.party_invoic_dt = ds.Tables[0].Rows[0]["party_invoic_dt"].ToString().Trim();

                    _pnv_header_Details = (from DataRow dr in ds.Tables[1].Rows
                                           select new pnv_Details()
                                           {
                                               DetailsId = Convert.ToString(dr["DetailsId"]),
                                               InvoceProductDescriptiopn = Convert.ToString(dr["InvoceProductDescriptiopn"]),
                                               DetailsQuantity = Convert.ToString(dr["DetailsQuantity"]),
                                               PurchasePrice = Convert.ToString(dr["PurchasePrice"]),
                                               DetailsAmount = Convert.ToString(dr["DetailsAmount"]),
                                               TaxAmount = Convert.ToString(dr["TaxAmount"]),
                                               AmountBaseCurrency = Convert.ToString(dr["AmountBaseCurrency"]),
                                               Name = Convert.ToString(dr["Name"])


                                           }).ToList();

                    _tax_Details = (from DataRow dr in ds.Tables[2].Rows
                                    select new tax_Details()
                                    {
                                        p_detailid = Convert.ToString(dr["p_detailid"]),
                                        p_percentage = Convert.ToString(dr["p_percentage"]),
                                        p_tax_amnt = Convert.ToString(dr["p_tax_amnt"]),
                                        t_scheme_name = Convert.ToString(dr["t_scheme_name"])
                                    }).ToList();

                    _PurchaseInvoiceTax = (from DataRow dr in ds.Tables[3].Rows
                                           select new PurchaseInvoiceTax()
                                           {
                                               inv_tax_per = Convert.ToString(dr["inv_tax_per"]),
                                               inv_tax_amnt = Convert.ToString(dr["inv_tax_amnt"]),
                                               t_scheme_name = Convert.ToString(dr["t_scheme_name"]),
                                           }).ToList();


                    if (ob.Amounts_are == "Import")
                    {
                        if (ds.Tables[5].Rows.Count > 0)
                        {
                            _TermsCondition.type_of_import = ds.Tables[5].Rows[0]["type_of_import"].ToString().Trim();
                            _TermsCondition.pymnt_trm_rmrk = ds.Tables[5].Rows[0]["pymnt_trm_rmrk"].ToString().Trim();
                            _TermsCondition.inco_dlvry_trm = ds.Tables[5].Rows[0]["inco_dlvry_trm"].ToString().Trim();
                            _TermsCondition.inco_dlvry_trm_rmrks = ds.Tables[5].Rows[0]["inco_dlvry_trm_rmrks"].ToString().Trim();
                            _TermsCondition.shpmnt_scedule = ds.Tables[5].Rows[0]["shpmnt_scedule"].ToString().Trim();
                            _TermsCondition.Port_Description = ds.Tables[5].Rows[0]["Port_Description"].ToString().Trim();
                            _TermsCondition.PortOfDestination = ds.Tables[5].Rows[0]["PortOfDestination"].ToString().Trim();
                            _TermsCondition.BE_Number = ds.Tables[5].Rows[0]["BE_Number"].ToString().Trim();
                            _TermsCondition.bedt = ds.Tables[5].Rows[0]["bedt"].ToString().Trim();
                            _TermsCondition.BE_Value = ds.Tables[5].Rows[0]["BE_Value"].ToString().Trim();
                            _TermsCondition.partial_shipment = ds.Tables[5].Rows[0]["partial_shipment"].ToString().Trim();
                            _TermsCondition.Transshipment = ds.Tables[5].Rows[0]["Transshipment"].ToString().Trim();
                            _TermsCondition.Transshipment = ds.Tables[5].Rows[0]["Transshipment"].ToString().Trim();
                            _TermsCondition.PackingSpec = ds.Tables[5].Rows[0]["PackingSpec"].ToString().Trim();
                            _TermsCondition.val_ordr_dt = ds.Tables[5].Rows[0]["val_ordr_dt"].ToString().Trim();
                            _TermsCondition.ValidityOfOrderRemarks = ds.Tables[5].Rows[0]["ValidityOfOrderRemarks"].ToString().Trim();
                            _TermsCondition.cou_country = ds.Tables[5].Rows[0]["cou_country"].ToString().Trim();
                            _TermsCondition.FreeDetentionPeriod = ds.Tables[5].Rows[0]["FreeDetentionPeriod"].ToString().Trim();
                            _TermsCondition.FreeDetentionPeriodRemark = ds.Tables[5].Rows[0]["FreeDetentionPeriodRemark"].ToString().Trim();
                            _TermsCondition.bnk_bankName = ds.Tables[5].Rows[0]["bnk_bankName"].ToString().Trim();
                            _TermsCondition.Bank_Branch = ds.Tables[5].Rows[0]["Bank_Branch"].ToString().Trim();
                            _TermsCondition.Bank_Address = ds.Tables[5].Rows[0]["Bank_Address"].ToString().Trim();
                            _TermsCondition.Bank_Landmark = ds.Tables[5].Rows[0]["Bank_Landmark"].ToString().Trim();
                            _TermsCondition.Bank_Pin = ds.Tables[5].Rows[0]["Bank_Pin"].ToString().Trim();
                            _TermsCondition.Bank_AcNo = ds.Tables[5].Rows[0]["Bank_AcNo"].ToString().Trim();
                            _TermsCondition.Bank_SwiftCode = ds.Tables[5].Rows[0]["Bank_SwiftCode"].ToString().Trim();
                            _TermsCondition.Bank_RTGSCode = ds.Tables[5].Rows[0]["Bank_RTGSCode"].ToString().Trim();
                            _TermsCondition.Bank_IFSCCode = ds.Tables[5].Rows[0]["Bank_IFSCCode"].ToString().Trim();
                            _TermsCondition.Bank_Remarks = ds.Tables[5].Rows[0]["Bank_Remarks"].ToString().Trim();

                        }                      

                    }
                    else
                    {
                        if (ds.Tables[4].Rows.Count > 0)
                        {
                            _TermsCondition.delvrydt = ds.Tables[4].Rows[0]["delvrydt"].ToString().Trim();
                            _TermsCondition.dlvryrmrk = ds.Tables[4].Rows[0]["dlvryrmrk"].ToString().Trim();
                            _TermsCondition.insrnc_cvrg = ds.Tables[4].Rows[0]["insrnc_cvrg"].ToString().Trim();
                            _TermsCondition.freight_chrgs = ds.Tables[4].Rows[0]["freight_chrgs"].ToString().Trim();
                            _TermsCondition.freight_rmrks = ds.Tables[4].Rows[0]["freight_rmrks"].ToString().Trim();
                            _TermsCondition.PermitValue = ds.Tables[4].Rows[0]["PermitValue"].ToString().Trim();
                            _TermsCondition.Remarks = ds.Tables[4].Rows[0]["Remarks"].ToString().Trim();
                            _TermsCondition.cert_req = ds.Tables[4].Rows[0]["cert_req"].ToString().Trim();
                            _TermsCondition.dlvr_dtls = ds.Tables[4].Rows[0]["dlvr_dtls"].ToString().Trim();
                            _TermsCondition.TransporterName = ds.Tables[4].Rows[0]["TransporterName"].ToString().Trim();
                            _TermsCondition.discnt_rcv = ds.Tables[4].Rows[0]["discnt_rcv"].ToString().Trim();
                            _TermsCondition.dscnt_rcv_dtls = ds.Tables[4].Rows[0]["dscnt_rcv_dtls"].ToString().Trim();
                            _TermsCondition.commission_rcv = ds.Tables[4].Rows[0]["commission_rcv"].ToString().Trim();
                            _TermsCondition.c_rcv_dtls = ds.Tables[4].Rows[0]["c_rcv_dtls"].ToString().Trim();
                            _TermsCondition.CommissionRate = ds.Tables[4].Rows[0]["CommissionRate"].ToString().Trim();
                            _TermsCondition.bnk_bankName = ds.Tables[4].Rows[0]["bnk_bankName"].ToString().Trim();
                            _TermsCondition.Bank_Branch = ds.Tables[4].Rows[0]["Bank_Branch"].ToString().Trim();
                            _TermsCondition.Bank_Address = ds.Tables[4].Rows[0]["Bank_Address"].ToString().Trim();
                            _TermsCondition.Bank_Landmark = ds.Tables[4].Rows[0]["Bank_Landmark"].ToString().Trim();
                            _TermsCondition.Bank_Pin = ds.Tables[4].Rows[0]["Bank_Pin"].ToString().Trim();
                            _TermsCondition.Bank_AcNo = ds.Tables[4].Rows[0]["Bank_AcNo"].ToString().Trim();
                            _TermsCondition.Bank_SwiftCode = ds.Tables[4].Rows[0]["Bank_SwiftCode"].ToString().Trim();
                            _TermsCondition.Bank_RTGSCode = ds.Tables[4].Rows[0]["Bank_RTGSCode"].ToString().Trim();
                            _TermsCondition.Bank_IFSCCode = ds.Tables[4].Rows[0]["Bank_IFSCCode"].ToString().Trim();
                            _TermsCondition.Bank_Remarks = ds.Tables[4].Rows[0]["Bank_Remarks"].ToString().Trim();
                        }
                        //Rev work start 15.06.2022 Mantise Issue:24952 Transporter Tab is missing in the Purchase Invoice view mode
                        if(ds.Tables[5].Rows.Count>0)
                        {
                            _Trnsp.TRANSAPORTERNAME = ds.Tables[5].Rows[0]["TRANSAPORTERNAME"].ToString().Trim();
                            _Trnsp.FINALTRANSPORTER = ds.Tables[5].Rows[0]["FINALTRANSPORTER"].ToString().Trim();
                            _Trnsp.LGL_LEGALSTATUS = ds.Tables[5].Rows[0]["LGL_LEGALSTATUS"].ToString().Trim();
                            _Trnsp.TRP_ISREGISTERED = ds.Tables[5].Rows[0]["TRP_ISREGISTERED"].ToString().Trim();
                            _Trnsp.TRP_GSTIN = ds.Tables[5].Rows[0]["TRP_GSTIN"].ToString().Trim();
                            _Trnsp.TRPVEH_VECHILESNOS = ds.Tables[5].Rows[0]["TRPVEH_VECHILESNOS"].ToString().Trim();
                            _Trnsp.FREIGHTCHARGE = ds.Tables[5].Rows[0]["FREIGHTCHARGE"].ToString().Trim();
                            _Trnsp.LOCATIONPOINT = ds.Tables[5].Rows[0]["LOCATIONPOINT"].ToString().Trim();

                            _Trnsp.LOADINGCHARGE = ds.Tables[5].Rows[0]["LOADINGCHARGE"].ToString().Trim();
                            _Trnsp.UNLOADINGCHARGE = ds.Tables[5].Rows[0]["UNLOADINGCHARGE"].ToString().Trim();
                            _Trnsp.PARKINGCHARGE = ds.Tables[5].Rows[0]["PARKINGCHARGE"].ToString().Trim();
                            _Trnsp.WEIGHT = ds.Tables[5].Rows[0]["WEIGHT"].ToString().Trim();
                            _Trnsp.TOLLTAX = ds.Tables[5].Rows[0]["TOLLTAX"].ToString().Trim();
                            _Trnsp.TRP_SERVICETAXES = ds.Tables[5].Rows[0]["TRP_SERVICETAXES"].ToString().Trim();
                            _Trnsp.TOTALCHARGES = ds.Tables[5].Rows[0]["TOTALCHARGES"].ToString().Trim();

                            _Trnsp.DISTANCE = ds.Tables[5].Rows[0]["DISTANCE"].ToString().Trim();
                            _Trnsp.LRNO = ds.Tables[5].Rows[0]["LRNO"].ToString().Trim();
                            _Trnsp.VEHICLEOUTDATE = ds.Tables[5].Rows[0]["VEHICLEOUTDATE"].ToString().Trim();
                            _Trnsp.VEHICLE_TYPE = ds.Tables[5].Rows[0]["VEHICLE_TYPE"].ToString().Trim();
                            _Trnsp.TRANSPORTER_MODE = ds.Tables[5].Rows[0]["TRANSPORTER_MODE"].ToString().Trim();
                            _Trnsp.TRP_ADDRESS = ds.Tables[5].Rows[0]["TRP_ADDRESS"].ToString().Trim();
                            

                            _Trnsp.COU_COUNTRY = ds.Tables[5].Rows[0]["COU_COUNTRY"].ToString().Trim();
                            _Trnsp.STATE = ds.Tables[5].Rows[0]["STATE"].ToString().Trim();
                            _Trnsp.CITY_NAME = ds.Tables[5].Rows[0]["CITY_NAME"].ToString().Trim();
                            _Trnsp.PIN_CODE = ds.Tables[5].Rows[0]["PIN_CODE"].ToString().Trim();
                            _Trnsp.AREA_NAME = ds.Tables[5].Rows[0]["AREA_NAME"].ToString().Trim();
                            _Trnsp.TRP_PHONE = ds.Tables[5].Rows[0]["TRP_PHONE"].ToString().Trim();
                        }
                        //Rev work close 15.06.2022 Mantise Issue:24952 Transporter Tab is missing in the Purchase Invoice view mode
                    }




                }




                return_obj.header = ob;
                return_obj.PnvDetails = _pnv_header_Details;
                return_obj.TaxDetails = _tax_Details;
                return_obj.PnvTaxDetails = _PurchaseInvoiceTax;
                return_obj.pnvtermscondition = _TermsCondition;
                //Rev work start 15.06.2022 Mantise Issue:24952 Transporter Tab is missing in the Purchase Invoice view mode
                return_obj.pnvTransport = _Trnsp;
                //Rev work close 15.06.2022 Mantise Issue:24952 Transporter Tab is missing in the Purchase Invoice view mode
                return_obj.msg = "ok";
            }
            catch (Exception ex)
            {
                return_obj.msg = ex.Message;
            }
            return return_obj;

        }
    }
    public class pnv_header_Details
    {
        //public int InvoiceId { get; set; }
        public string InvoiceNumber { get; set; }
        public string BranchDescription { get; set; }
        public string invoice_date { get; set; }
        public string VendorName { get; set; }
        public string PartyInvoiceNo { get; set; }
        public string Invoice_Reference { get; set; }
        public string contact_name { get; set; }
        public string Currency_AlphaCode { get; set; }
        public string Currency_Conversion_Rate { get; set; }
        public string Invoice_ReverseMechanism { get; set; }
        public string Amounts_are { get; set; }
        public string Entry_date { get; set; }
        public string Invoice_Remarks { get; set; }
        public string EWayBillNumber { get; set; }
        public string value { get; set; }
        public string billing_address { get; set; }
        public string shipping_address { get; set; }
        public string grn_no { get; set; }
        public string invoice_type { get; set; }
        public string party_invoic_dt { get; set; }


    }
    public class pnv_Details
    {
        public string DetailsId { get; set; }
        public string InvoceProductDescriptiopn { get; set; }
        public string DetailsQuantity { get; set; }
        public string PurchasePrice { get; set; }
        public string DetailsAmount { get; set; }
        public string TaxAmount { get; set; }
        public string AmountBaseCurrency { get; set; }
        public string Name { get; set; }
    }
    public class tax_Details
    {
        public string p_detailid { get; set; }
        public string p_percentage { get; set; }
        public string p_tax_amnt { get; set; }
        public string t_scheme_name { get; set; }
    }
    public class PurchaseInvoiceTax
    {
        public string inv_tax_per { get; set; }
        public string inv_tax_amnt { get; set; }
        public string t_scheme_name { get; set; }

    }
    public class TermsCondition
    {
        public string delvrydt { get; set; }
        public string dlvryrmrk { get; set; }
        public string insrnc_cvrg { get; set; }
        public string freight_chrgs { get; set; }
        public string freight_rmrks { get; set; }
        public string PermitValue { get; set; }
        public string Remarks { get; set; }
        public string cert_req { get; set; }
        public string dlvr_dtls { get; set; }
        public string TransporterName { get; set; }
        public string discnt_rcv { get; set; }
        public string dscnt_rcv_dtls { get; set; }
        public string commission_rcv { get; set; }
        public string c_rcv_dtls { get; set; }
        public string CommissionRate { get; set; }

        //----------------------------------------------Import Data for purchase invoice----------------------------------------------------------------//
        public string type_of_import { get; set; }
        public string pymnt_trm_rmrk { get; set; }
        public string inco_dlvry_trm { get; set; }
        public string inco_dlvry_trm_rmrks { get; set; }
        public string shpmnt_scedule { get; set; }
        public string Port_Description { get; set; }
        public string PortOfDestination { get; set; }
        public string BE_Number { get; set; }
        public string bedt { get; set; }
        public string BE_Value { get; set; }
        public string partial_shipment { get; set; }
        public string Transshipment { get; set; }
        public string PackingSpec { get; set; }
        public string val_ordr_dt { get; set; }
        public string ValidityOfOrderRemarks { get; set; }
        public string cou_country { get; set; }
        public string FreeDetentionPeriod { get; set; }
        public string FreeDetentionPeriodRemark { get; set; }
        //-------------------------------------------------------------------------------------------------------------------------
        public string bnk_bankName { get; set; }
        public string Bank_Branch { get; set; }
        public string Bank_Address { get; set; }
        public string Bank_Landmark { get; set; }
        public string Bank_Pin { get; set; }
        public string Bank_AcNo { get; set; }
        public string Bank_SwiftCode { get; set; }
        public string Bank_RTGSCode { get; set; }
        public string Bank_IFSCCode { get; set; }
        public string Bank_Remarks { get; set; }




    }
    public class PurchaseInvoice
    {
        public string msg { get; set; }
        public pnv_header_Details header { get; set; }
        public List<pnv_Details> PnvDetails { get; set; }
        public List<tax_Details> TaxDetails { get; set; }
        public List<PurchaseInvoiceTax> PnvTaxDetails { get; set; }
        public TermsCondition pnvtermscondition { get; set; }
        //Rev work start 15.06.2022 Mantise Issue:24952 Transporter Tab is missing in the Purchase Invoice view mode
        public Transport pnvTransport { get; set; }
        //Rev work close 15.06.2022 Mantise Issue:24952 Transporter Tab is missing in the Purchase Invoice view mode
    }
    //Rev work start 15.06.2022 Mantise Issue:24952 Transporter Tab is missing in the Purchase Invoice view mode
    public class Transport
    {
        public string TRANSAPORTERNAME { get; set; }	
        public string FINALTRANSPORTER { get; set; }
        public string LGL_LEGALSTATUS { get; set; }
        public string TRP_ISREGISTERED { get; set; }
        public string TRP_GSTIN	 { get; set; }
        public string TRPVEH_VECHILESNOS	 { get; set; }
        public string FREIGHTCHARGE	 { get; set; }
        public string LOCATIONPOINT	 { get; set; }
        public string LOADINGCHARGE	 { get; set; }
        public string UNLOADINGCHARGE	 { get; set; }
        public string PARKINGCHARGE	 { get; set; }
        public string WEIGHT	 { get; set; }
        public string TOLLTAX	 { get; set; }
        public string TRP_SERVICETAXES	 { get; set; }
        public string TOTALCHARGES	 { get; set; }
        public string DISTANCE	 { get; set; }
        public string LRNO	 { get; set; }
        public string VEHICLEOUTDATE	 { get; set; }
        public string VEHICLE_TYPE	 { get; set; }
        public string TRANSPORTER_MODE	 { get; set; }
        public string TRP_ADDRESS	 { get; set; }
        public string COU_COUNTRY	 { get; set; }
        public string STATE	 { get; set; }
        public string CITY_NAME	 { get; set; }
        public string PIN_CODE	 { get; set; }
        public string AREA_NAME	 { get; set; }
        public string TRP_PHONE { get; set; }
        
    }
    //Rev work close 15.06.2022 Mantise Issue:24952 Transporter Tab is missing in the Purchase Invoice view mode
}
