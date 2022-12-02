using BusinessLogicLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;

namespace ERP.OMS.Management.Activities.Services
{
    /// <summary>
    /// Summary description for VendorPaymentAdjustment
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class VendorPaymentAdjustment : System.Web.Services.WebService
    {

        #region Adjustment of Advance with Invoice
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetAdvancePaymentList(string Mode, string VendorID, string date, string Branch)
        {
            List<RpListVP> ReturnList = new List<RpListVP>();
            VendorPaymentAdjustmentBL blLayer = new VendorPaymentAdjustmentBL();
            DataTable ReceiptList = blLayer.GetAdvanceListAdd(date, VendorID, Branch);
            ReturnList = (from DataRow dr in ReceiptList.Rows
                          select new RpListVP()
                          {
                             
                              DocId = Convert.ToInt32(dr["ReceiptPayment_ID"]),
                              docNo = Convert.ToString(dr["ReceiptPayment_VoucherNumber"]),
                              ArId = Convert.ToInt32(dr["ReceiptPayment_uniqueID"]),
                              docDt = Convert.ToString(dr["ReceiptPayment_TransactionDate"]),
                              ActAmt = Convert.ToDecimal(dr["Actual_Payment_Amount"]),
                              avlAmt = Convert.ToDecimal(dr["Curent_Available_Amount"]),
                              AdvType = Convert.ToString(dr["AdvType"]),
                              Proj_Id = Convert.ToString(dr["Proj_Id"]),
                              Proj_Code = Convert.ToString(dr["Proj_Code"]),
                              HIERARCHY_NAME = Convert.ToString(dr["HIERARCHY_NAME"])

                          }).ToList();
            return ReturnList;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetDocumentList(string Mode, string ReceiptId, string VendorId, string TransDate, string AdjId, string BranchId,
            string AdvType, string ProjectId)
        {
            List<InvoicedocumentList> documentList = new List<VendorPaymentAdjustment.InvoicedocumentList>();
            VendorPaymentAdjustmentBL blLayer = new VendorPaymentAdjustmentBL();
            DataTable DocumentList = blLayer.GetDocumentList(Mode, ReceiptId, VendorId, TransDate, AdjId, BranchId, AdvType, ProjectId);
            documentList = (from DataRow dr in DocumentList.Rows
                            select new InvoicedocumentList()
                            {
                                id = Convert.ToInt32(dr["id"]),
                                doctype = Convert.ToString(dr["doctype"]),
                                No = Convert.ToString(dr["No"]),
                                actAmt = Convert.ToDecimal(dr["actAmt"]),
                                unPdAmt = Convert.ToDecimal(dr["unPdAmt"]),
                                uniqueid = Convert.ToString(dr["uniqueid"]),
                                docDate = Convert.ToString(dr["invDate"]),
                                cur = Convert.ToString(dr["cur"]),
                                PartyInvoiceNo = Convert.ToString(dr["PartyInvoiceNo"])


                            }).ToList();
            return documentList;
        }
        #endregion

        #region Adjustment of Advance With Credit Note

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetDebitNoteListAdd(string VendorId, string TransDate, string BranchId)
        {
            List<DebitNotedocumentList> documentList = new List<VendorPaymentAdjustment.DebitNotedocumentList>();
            VendorPaymentAdjustmentBL blLayer = new VendorPaymentAdjustmentBL();
            DataTable DocumentList = blLayer.GetDebitNoteList(VendorId, TransDate, BranchId);
            documentList = (from DataRow dr in DocumentList.Rows
                            select new DebitNotedocumentList()
                            {
                                id = Convert.ToInt32(dr["DocumentID"]),
                                doctype = Convert.ToString(dr["Type"]),
                                No = Convert.ToString(dr["DocumentNumber"]),
                                docDate = Convert.ToString(dr["DocDate"]),
                                actAmt = Convert.ToDecimal(dr["UnPaidAmount"]),


                            }).ToList();
            return documentList;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetPBDocumentList(string Mode, string ReceiptId, string VendorId, string TransDate, string AdjId, string BranchId)
        {
            List<documentList> documentList = new List<VendorPaymentAdjustment.documentList>();
            VendorPaymentAdjustmentBL blLayer = new VendorPaymentAdjustmentBL();
            DataTable DocumentList = blLayer.GetInvoiceDocumentList(Mode, ReceiptId, VendorId, TransDate, AdjId, BranchId);
            documentList = (from DataRow dr in DocumentList.Rows
                            select new documentList()
                            {
                                id = Convert.ToInt32(dr["id"]),
                                doctype = Convert.ToString(dr["doctype"]),
                                No = Convert.ToString(dr["No"]),
                                actAmt = Convert.ToDecimal(dr["actAmt"]),
                                unPdAmt = Convert.ToDecimal(dr["unPdAmt"]),
                                uniqueid = Convert.ToString(dr["uniqueid"]),
                                docDate = Convert.ToString(dr["invDate"]),
                                cur = Convert.ToString(dr["cur"])


                            }).ToList();
            return documentList;
        }

        public class RpListForInvoice
        {

            public int DocId { get; set; }
            public string docNo { get; set; }
            //public int ArId { get; set; }
            public string docDt { get; set; }
            public decimal ActAmt { get; set; }
            public decimal avlAmt { get; set; }
            public string AdvType { get; set; }

        }

        public class RpListVP
        {

            public int DocId { get; set; }
            public string docNo { get; set; }
            public int ArId { get; set; }
            public string docDt { get; set; }
            public decimal ActAmt { get; set; }
            public decimal avlAmt { get; set; }
            public string AdvType { get; set; }

            public string Proj_Id { get; set; }
            public string Proj_Code { get; set; }
            public string HIERARCHY_NAME { get; set; }


        }

        public class RpList
        {
          
            public int DocId { get; set; }
            public string docNo { get; set; }
           public int ArId { get; set; }
            public string docDt { get; set; }
            public decimal ActAmt { get; set; }
            public decimal avlAmt { get; set; }
            public string AdvType { get; set; }

           


        }
        public class documentList
        {
            public string uniqueid { get; set; }
            public string doctype { get; set; }
            public Int32 id { get; set; }
            public string No { get; set; }
            public decimal actAmt { get; set; }
            public decimal unPdAmt { get; set; }
            public string docDate { get; set; }
            public string cur { get; set; }
        }

        public class InvoicedocumentList
        {
            public string uniqueid { get; set; }
            public string doctype { get; set; }
            public Int32 id { get; set; }
            public string No { get; set; }
            public decimal actAmt { get; set; }
            public decimal unPdAmt { get; set; }
            public string docDate { get; set; }
            public string cur { get; set; }
            public string PartyInvoiceNo { get; set; }

        }

        public class DebitNotedocumentList
        {
            public Int32 id { get; set; }
            public string doctype { get; set; }
            public string No { get; set; }
            public string docDate { get; set; }
            public decimal actAmt { get; set; }
            public decimal payAmt { get; set; }

        }

        #endregion

        #region Adjustment of Advance With Debit Note

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetCrNoteDocumentList(string Mode, string ReceiptId, string VendorId, string TransDate, string AdjId, string BranchId, string ProjectId)
        {
            List<documentListDrNote> documentListDrNote = new List<VendorPaymentAdjustment.documentListDrNote>();
            VendorPaymentAdjustmentBL blLayer = new VendorPaymentAdjustmentBL();
            DataTable documentltDrNote = blLayer.GetCrNoteCocumentList(Mode, ReceiptId, VendorId, TransDate, AdjId, BranchId, ProjectId);
            documentListDrNote = (from DataRow dr in documentltDrNote.Rows
                                  select new documentListDrNote()
                                  {
                                      id = Convert.ToInt32(dr["id"]),
                                      doctype = Convert.ToString(dr["doctype"]),
                                      No = Convert.ToString(dr["No"]),
                                      actAmt = Convert.ToDecimal(dr["actAmt"]),
                                      unPdAmt = Convert.ToDecimal(dr["unPdAmt"]),
                                      uniqueid = Convert.ToString(dr["uniqueid"]),
                                      docDate = Convert.ToString(dr["invDate"]),
                                      cur = Convert.ToString(dr["cur"])


                                  }).ToList();
            return documentListDrNote;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetAdvanceListAddForCrNote(string Mode, string VendorId, string date, string Branch)
        {
            List<RpListDR> ReturnList = new List<RpListDR>();
            VendorPaymentAdjustmentBL blLayer = new VendorPaymentAdjustmentBL();
            DataTable ReceiptList = blLayer.GetAdvanceListAddForCrNote(date, VendorId, Branch);
            ReturnList = (from DataRow dr in ReceiptList.Rows
                          select new RpListDR()
                          {
                              ArId = Convert.ToInt32(dr["ReceiptPayment_ID"]),
                              doctype = Convert.ToString(dr["AdvanceType"]),
                              docNo = Convert.ToString(dr["ReceiptPayment_VoucherNumber"]),
                              docDt = Convert.ToString(dr["ReceiptPayment_TransactionDate"]),
                              ActAmt = Convert.ToDecimal(dr["Actual_Receipt_Amount"]),
                              avlAmt = Convert.ToDecimal(dr["Curent_Available_Amount"]),
                              Cur = Convert.ToInt32(dr["ReceiptPayment_Currency"]),
                              CurRate = Convert.ToInt32(dr["ReceiptPayment_Rate"]),
                               Proj_Id = Convert.ToString(dr["Proj_Id"]),
                              Proj_Code = Convert.ToString(dr["Proj_Code"]),
                              HIERARCHY_NAME = Convert.ToString(dr["HIERARCHY_NAME"])

                          }).ToList();
            return ReturnList;
        }
        public class documentListDrNote
        {
            public string uniqueid { get; set; }
            public string doctype { get; set; }
            public Int32 id { get; set; }
            public string No { get; set; }
            public decimal actAmt { get; set; }
            public decimal unPdAmt { get; set; }
            public string docDate { get; set; }
            public string cur { get; set; }
        }
        public class RpListDR
        {
            public int ArId { get; set; }
            public string doctype { get; set; }
            public string docNo { get; set; }
            public string docDt { get; set; }
            public decimal ActAmt { get; set; }
            public decimal avlAmt { get; set; }
            public int Cur { get; set; }
            public decimal CurRate { get; set; }
            public string Proj_Id { get; set; }
            public string Proj_Code { get; set; }
            public string HIERARCHY_NAME { get; set; }
        }
        #endregion



        #region Adjustment of Rate difference 
         [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetDocumentListRatediff(string Mode, string VendorID, string date, string Branch)
        {
            List<RDiffList> ReturnList = new List<RDiffList>();
            VendorPaymentAdjustmentBL blLayer = new VendorPaymentAdjustmentBL();
            DataTable ReceiptList = blLayer.GetDocumentListAdd(date, VendorID, Branch);
            ReturnList = (from DataRow dr in ReceiptList.Rows
                          select new RDiffList()
                          {
                              ArId = Convert.ToInt32(dr["UniqueID"]),
                              DocId = Convert.ToInt32(dr["DocumentID"]),
                              docNo = Convert.ToString(dr["DocumentNumber"]),
                              docDt = Convert.ToString(dr["DocDate"]),
                              ActAmt = Convert.ToDecimal(dr["Actual_Amount"]),
                              avlAmt = Convert.ToDecimal(dr["Curent_Available_Amount"]),
                              AdvType = Convert.ToString(dr["Type"]),
                              Cur = Convert.ToInt32(dr["Currency_Id"]),
                              CurRate = Convert.ToDecimal(dr["Currency_Conversion_Rate"])

                          }).ToList();
            return ReturnList;
        }
         [WebMethod(EnableSession = true)]
         [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
         public object GetInvoiceDocumentListRateDiff(string Mode, string ReceiptId, string VendorId, string TransDate, string AdjId, string BranchId,
             string AdvType)
         {
             List<documentList> documentList = new List<VendorPaymentAdjustment.documentList>();
             VendorPaymentAdjustmentBL blLayer = new VendorPaymentAdjustmentBL();
             DataTable DocumentList = blLayer.GetInvoiceDocumentRateDiffList(Mode, ReceiptId, VendorId, TransDate, AdjId, BranchId, AdvType);
             documentList = (from DataRow dr in DocumentList.Rows
                             select new documentList()
                             {
                                 id = Convert.ToInt32(dr["id"]),
                                 doctype = Convert.ToString(dr["doctype"]),
                                 No = Convert.ToString(dr["No"]),
                                 actAmt = Convert.ToDecimal(dr["actAmt"]),
                                 unPdAmt = Convert.ToDecimal(dr["unPdAmt"]),
                                 uniqueid = Convert.ToString(dr["uniqueid"]),
                                 docDate = Convert.ToString(dr["invDate"]),
                                 cur = Convert.ToString(dr["cur"])


                             }).ToList();
             return documentList;
         }

         public class RDiffList
         {
             public int DocId { get; set; }
             public string docNo { get; set; }
             public int ArId { get; set; }
             public string docDt { get; set; }
             public decimal ActAmt { get; set; }
             public decimal avlAmt { get; set; }
             public string AdvType { get; set; }
             public int Cur { get; set; }
             public decimal CurRate { get; set; }

         }
        
        #endregion

         #region Adjustment of Debit Note with Invoice
         [WebMethod(EnableSession = true)]
         [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
         public object GetDebitNoteListPurchaseInvoice(string Mode, string VendorID, string date, string Branch)
         {
             List<RpListVP> ReturnList = new List<RpListVP>();
             VendorPaymentAdjustmentBL blLayer = new VendorPaymentAdjustmentBL();
             DataTable ReceiptList = blLayer.GetDebitNoteListAdd(date, VendorID, Branch);
             ReturnList = (from DataRow dr in ReceiptList.Rows
                           select new RpListVP()
                           {
                              
                               DocId = Convert.ToInt32(dr["DocumentID"]),
                               docNo = Convert.ToString(dr["DocumentNumber"]),
                               ArId = Convert.ToInt32(dr["DocumentID"]),
                               docDt = Convert.ToString(dr["DocDate"]),
                               ActAmt = Convert.ToDecimal(dr["actualAmount"]),
                               avlAmt = Convert.ToDecimal(dr["UnPaidAmount"]),
                               AdvType = Convert.ToString(dr["Type"]),
                               Proj_Id = Convert.ToString(dr["Proj_Id"]),
                               Proj_Code = Convert.ToString(dr["Proj_Code"]),
                               HIERARCHY_NAME = Convert.ToString(dr["HIERARCHY_NAME"])


                           }).ToList();
             return ReturnList;
         }

         [WebMethod(EnableSession = true)]
         [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
         public object GetPurchaseDocumentList(string Mode, string ReceiptId, string VendorId, string TransDate, string AdjId, string BranchId,
             string AdvType, string ProjectId)
         {
             List<documentList> documentList = new List<VendorPaymentAdjustment.documentList>();
             VendorPaymentAdjustmentBL blLayer = new VendorPaymentAdjustmentBL();
             DataTable DocumentList = blLayer.GetDocumentList(Mode, ReceiptId, VendorId, TransDate, AdjId, BranchId, AdvType, ProjectId);
             documentList = (from DataRow dr in DocumentList.Rows
                             select new documentList()
                             {
                                 id = Convert.ToInt32(dr["id"]),
                                 doctype = Convert.ToString(dr["doctype"]),
                                 No = Convert.ToString(dr["No"]),
                                 actAmt = Convert.ToDecimal(dr["actAmt"]),
                                 unPdAmt = Convert.ToDecimal(dr["unPdAmt"]),
                                 uniqueid = Convert.ToString(dr["uniqueid"]),
                                 docDate = Convert.ToString(dr["invDate"]),
                                 cur = Convert.ToString(dr["cur"])


                             }).ToList();
             return documentList;
         }
         #endregion
    }
}
