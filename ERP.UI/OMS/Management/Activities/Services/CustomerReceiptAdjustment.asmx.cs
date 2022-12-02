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
    /// Summary description for CustomerReceiptAdjustment
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class CustomerReceiptAdjustment : System.Web.Services.WebService
    {
        #region Adjustment of Advance with Invoice
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetAdvanceReceiptList(string Mode, string CustomerId, string date, string Branch)
        {
            List<RpList> ReturnList = new List<RpList>();
            CustomerReceiptAdjustmentBl blLayer = new CustomerReceiptAdjustmentBl();
            DataTable ReceiptList = blLayer.GetAdvanceListAdd(date, CustomerId, Branch);
            ReturnList = (from DataRow dr in ReceiptList.Rows
                          select new RpList()
                          {
                              ArId = Convert.ToInt32(dr["ReceiptPayment_uniqueID"]),
                              DocId = Convert.ToInt32(dr["ReceiptPayment_ID"]),
                              docNo = Convert.ToString(dr["ReceiptPayment_VoucherNumber"]),
                              docDt = Convert.ToString(dr["ReceiptPayment_TransactionDate"]),
                              ActAmt = Convert.ToDecimal(dr["Actual_Receipt_Amount"]),
                              avlAmt = Convert.ToDecimal(dr["Curent_Available_Amount"]),
                              Cur = Convert.ToInt32(dr["ReceiptPayment_Currency"]),
                              CurRate = Convert.ToInt32(dr["ReceiptPayment_Rate"]),
                              AdvType = Convert.ToString(dr["AdvType"]),
                              Proj_Id = Convert.ToString(dr["Proj_Id"]),
                              Proj_Code = Convert.ToString(dr["Proj_Code"]),
                              HIERARCHY_NAME = Convert.ToString(dr["HIERARCHY_NAME"])

                          }).ToList();
            return ReturnList;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetDocumentList(string Mode, string ReceiptId, string customerId, string TransDate, string AdjId, string BranchId,
            string AdvType, string ProjectId)
        {
            List<documentList> documentList = new List<CustomerReceiptAdjustment.documentList>();
            CustomerReceiptAdjustmentBl blLayer = new CustomerReceiptAdjustmentBl();
            DataTable DocumentList = blLayer.GetDocumentList(Mode, ReceiptId, customerId, TransDate, AdjId, BranchId, AdvType, ProjectId);
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

        #region Adjustment of Advance With Credit Note

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetOnAccountList(string CustomerId, string TransDate, string BranchId)
        {
            List<OnAccountdocumentList> documentList = new List<CustomerReceiptAdjustment.OnAccountdocumentList>();
            CustomerReceiptAdjustmentBl blLayer = new CustomerReceiptAdjustmentBl();
            DataTable DocumentList = blLayer.GetOnAccountList(CustomerId, TransDate, BranchId);
            documentList = (from DataRow dr in DocumentList.Rows
                            select new OnAccountdocumentList()
                            {
                                id = Convert.ToInt32(dr["DocumentID"]),
                                doctype = Convert.ToString(dr["Type"]),
                                No = Convert.ToString(dr["DocumentNumber"]),
                                docDate = Convert.ToString(dr["DocDate"]),
                                actAmt = Convert.ToDecimal(dr["UnPaidAmount"]),
                                Proj_Id = Convert.ToString(dr["Proj_Id"]),
                                Proj_Code = Convert.ToString(dr["Proj_Code"]),
                                HIERARCHY_NAME = Convert.ToString(dr["HIERARCHY_NAME"]),

                            }).ToList();
            return documentList;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetOnAccountDocumentList(string Mode, string ReceiptId, string customerId, string TransDate, string AdjId, string BranchId, string ProjectId)
        {
            List<documentList> documentList = new List<CustomerReceiptAdjustment.documentList>();
            CustomerReceiptAdjustmentBl blLayer = new CustomerReceiptAdjustmentBl();
            DataTable DocumentList = blLayer.GetOnAccountDocumentList(Mode, ReceiptId, customerId, TransDate, AdjId, BranchId, ProjectId);
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
        public class RpList
        {
            public int ArId { get; set; }
            public int DocId { get; set; }
            public string docNo { get; set; }
            public string docDt { get; set; }
            public decimal ActAmt { get; set; }
            public decimal avlAmt { get; set; }
            public int Cur { get; set; }
            public decimal CurRate { get; set; }
            public string AdvType { get; set; }

            public string Proj_Id { get; set; }
            public string Proj_Code { get; set; }
            public string HIERARCHY_NAME { get; set; }

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
        public class JournaldocumentList
        {
            public string uniqueid { get; set; }
            public string doctype { get; set; }
            public Int32 id { get; set; }
            public string No { get; set; }
            public decimal actAmt { get; set; }
            public decimal unPdAmt { get; set; }
            public string docDate { get; set; }
            public string cur { get; set; }
            //public string Proj_Id { get; set; }
            //public string Proj_Code { get; set; }
            //public string HIERARCHY_NAME { get; set; }
        }
        public class OnAccountdocumentList
        {
            public Int32 id { get; set; }
            public string doctype { get; set; }
            public string No { get; set; }
            public string docDate { get; set; }
            public decimal actAmt { get; set; }
            public decimal payAmt { get; set; }

            public string Proj_Id { get; set; }
            public string Proj_Code { get; set; }
            public string HIERARCHY_NAME { get; set; }

        }

        #endregion

        #region Adjustment of Advance With Debit Note

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetDrNoteDocumentList(string Mode, string ReceiptId, string customerId, string TransDate, string AdjId, string BranchId, string ProjectId)
        {
            List<documentListDrNote> documentListDrNote = new List<CustomerReceiptAdjustment.documentListDrNote>();
            CustomerReceiptAdjustmentBl blLayer = new CustomerReceiptAdjustmentBl();
            DataTable documentltDrNote = blLayer.GetDrNoteDocumentList(Mode, ReceiptId, customerId, TransDate, AdjId, BranchId, ProjectId);
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
        public object GetAdvanceListAddForDrNote(string Mode, string CustomerId, string date, string Branch)
        {
            List<RpListDR> ReturnList = new List<RpListDR>();
            CustomerReceiptAdjustmentBl blLayer = new CustomerReceiptAdjustmentBl();
            DataTable ReceiptList = blLayer.GetAdvanceListAddForDrNote(date, CustomerId, Branch);
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
                              HIERARCHY_NAME = Convert.ToString(dr["HIERARCHY_NAME"]),

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

        #region Adjustment of Cedit Note With Debit Note
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetDrNoteDocumentListForCrNote(string Mode, string ReceiptId, string customerId, string TransDate, string AdjId, string BranchId, string ProjectId)
        {
            List<documentList> documentlist = new List<documentList>();
            CustomerReceiptAdjustmentBl blLayer = new CustomerReceiptAdjustmentBl();
            DataTable DocumentList = blLayer.GetDeNoteDocumentList(Mode, ReceiptId, customerId, TransDate, AdjId, BranchId, ProjectId);
            documentlist = (from DataRow dr in DocumentList.Rows
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

            return documentlist;


        }

        #endregion

        #region Adjustment of journal with sales Invoice
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetJournalVoucharList(string Mode, string CustomerId, string date, string Branch)
        {
            List<RpList> ReturnList = new List<RpList>();
            CustomerReceiptAdjustmentBl blLayer = new CustomerReceiptAdjustmentBl();
            DataTable ReceiptList = blLayer.GetJournalListAdd(date, CustomerId, Branch);
            ReturnList = (from DataRow dr in ReceiptList.Rows
                          select new RpList()
                          {
                              ArId = Convert.ToInt32(dr["ReceiptPayment_uniqueID"]),
                              DocId = Convert.ToInt32(dr["ReceiptPayment_ID"]),
                              docNo = Convert.ToString(dr["ReceiptPayment_VoucherNumber"]),
                              docDt = Convert.ToString(dr["ReceiptPayment_TransactionDate"]),
                              ActAmt = Convert.ToDecimal(dr["Actual_Receipt_Amount"]),
                              avlAmt = Convert.ToDecimal(dr["Curent_Available_Amount"]),
                              Cur = Convert.ToInt32(dr["ReceiptPayment_Currency"]),
                              CurRate = Convert.ToInt32(dr["ReceiptPayment_Rate"]),
                              AdvType = Convert.ToString(dr["AdvType"]),
                              Proj_Id = Convert.ToString(dr["Proj_Id"]),
                              Proj_Code = Convert.ToString(dr["Proj_Code"]),
                              HIERARCHY_NAME = Convert.ToString(dr["HIERARCHY_NAME"]),

                          }).ToList();
            return ReturnList;
        }
        #endregion

        #region Adjustment of journal with Purchase Invoice

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetJournalCreditosVoucharList(string Mode, string CustomerId, string date, string Branch)
        {
            List<RpList> ReturnList = new List<RpList>();
            CustomerReceiptAdjustmentBl blLayer = new CustomerReceiptAdjustmentBl();
            DataTable ReceiptList = blLayer.GetJournalCreditosListAdd(date, CustomerId, Branch);
            ReturnList = (from DataRow dr in ReceiptList.Rows
                          select new RpList()
                          {
                              ArId = Convert.ToInt32(dr["ReceiptPayment_uniqueID"]),
                              DocId = Convert.ToInt32(dr["ReceiptPayment_ID"]),
                              docNo = Convert.ToString(dr["ReceiptPayment_VoucherNumber"]),
                              docDt = Convert.ToString(dr["ReceiptPayment_TransactionDate"]),
                              ActAmt = Convert.ToDecimal(dr["Actual_Receipt_Amount"]),
                              avlAmt = Convert.ToDecimal(dr["Curent_Available_Amount"]),
                              Cur = Convert.ToInt32(dr["ReceiptPayment_Currency"]),
                              CurRate = Convert.ToInt32(dr["ReceiptPayment_Rate"]),
                              AdvType = Convert.ToString(dr["AdvType"]),
                              Proj_Id = Convert.ToString(dr["Proj_Id"]),
                              Proj_Code = Convert.ToString(dr["Proj_Code"]),
                              HIERARCHY_NAME = Convert.ToString(dr["HIERARCHY_NAME"])

                          }).ToList();
            return ReturnList;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetPurchaseInvoiceDocumentList(string Mode, string ReceiptId, string customerId, string TransDate, string AdjId, string BranchId,
            string AdvType, string ProjectId)
        {
            List<documentList> documentList = new List<CustomerReceiptAdjustment.documentList>();
            CustomerReceiptAdjustmentBl blLayer = new CustomerReceiptAdjustmentBl();
            DataTable DocumentList = blLayer.GetPurchaseInvoiceDocumentList(Mode, ReceiptId, customerId, TransDate, AdjId, BranchId, AdvType, ProjectId);
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

        #region Adjustment of journal with Purchase Return

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetJournalCreditosVoucharPBList(string Mode, string CustomerId, string date, string Branch)
        {
            List<RpList> ReturnList = new List<RpList>();
            CustomerReceiptAdjustmentBl blLayer = new CustomerReceiptAdjustmentBl();
            DataTable ReceiptList = blLayer.GetJournalCreditosPRListAdd(date, CustomerId, Branch);
            ReturnList = (from DataRow dr in ReceiptList.Rows
                          select new RpList()
                          {
                              ArId = Convert.ToInt32(dr["ReceiptPayment_uniqueID"]),
                              DocId = Convert.ToInt32(dr["ReceiptPayment_ID"]),
                              docNo = Convert.ToString(dr["ReceiptPayment_VoucherNumber"]),
                              docDt = Convert.ToString(dr["ReceiptPayment_TransactionDate"]),
                              ActAmt = Convert.ToDecimal(dr["Actual_Receipt_Amount"]),
                              avlAmt = Convert.ToDecimal(dr["Curent_Available_Amount"]),
                              Cur = Convert.ToInt32(dr["ReceiptPayment_Currency"]),
                              CurRate = Convert.ToInt32(dr["ReceiptPayment_Rate"]),
                              AdvType = Convert.ToString(dr["AdvType"]),
                                Proj_Id = Convert.ToString(dr["Proj_Id"]),
                              Proj_Code = Convert.ToString(dr["Proj_Code"]),
                              HIERARCHY_NAME = Convert.ToString(dr["HIERARCHY_NAME"])

                          }).ToList();
            return ReturnList;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetPurchaseReturnDocumentList(string Mode, string ReceiptId, string customerId, string TransDate, string AdjId, string BranchId,
            string AdvType, string ProjectId)
        {
            List<documentList> documentList = new List<CustomerReceiptAdjustment.documentList>();
            CustomerReceiptAdjustmentBl blLayer = new CustomerReceiptAdjustmentBl();
            DataTable DocumentList = blLayer.GetPurchaseReturnDocumentList(Mode, ReceiptId, customerId, TransDate, AdjId, BranchId, AdvType, ProjectId);
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

        #region Adjustment of journal with Vendor Payment

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetJournalCreditosVPVoucharList(string Mode, string CustomerId, string date, string Branch)
        {
            List<RpList> ReturnList = new List<RpList>();
            CustomerReceiptAdjustmentBl blLayer = new CustomerReceiptAdjustmentBl();
            DataTable ReceiptList = blLayer.GetJournalCreditosVendorPayListAdd(date, CustomerId, Branch);
            ReturnList = (from DataRow dr in ReceiptList.Rows
                          select new RpList()
                          {
                              ArId = Convert.ToInt32(dr["ReceiptPayment_uniqueID"]),
                              DocId = Convert.ToInt32(dr["ReceiptPayment_ID"]),
                              docNo = Convert.ToString(dr["ReceiptPayment_VoucherNumber"]),
                              docDt = Convert.ToString(dr["ReceiptPayment_TransactionDate"]),
                              ActAmt = Convert.ToDecimal(dr["Actual_Receipt_Amount"]),
                              avlAmt = Convert.ToDecimal(dr["Curent_Available_Amount"]),
                              //Cur = Convert.ToInt32(dr["ReceiptPayment_Currency"]),
                              Cur = Convert.ToInt32(1),
                              CurRate = Convert.ToInt32(dr["ReceiptPayment_Rate"]),
                              AdvType = Convert.ToString(dr["AdvType"]),
                              Proj_Id = Convert.ToString(dr["Proj_Id"]),
                              Proj_Code = Convert.ToString(dr["Proj_Code"]),
                              HIERARCHY_NAME = Convert.ToString(dr["HIERARCHY_NAME"])

                          }).ToList();
            return ReturnList;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetPurchaseInvoiceDocumentVPList(string Mode, string ReceiptId, string customerId, string TransDate, string AdjId, string BranchId,
            string AdvType, string ProjectId)
        {
            List<JournaldocumentList> documentList = new List<CustomerReceiptAdjustment.JournaldocumentList>();
            CustomerReceiptAdjustmentBl blLayer = new CustomerReceiptAdjustmentBl();
            DataTable DocumentList = blLayer.GetVendorPayDocumentList(Mode, ReceiptId, customerId, TransDate, AdjId, BranchId, AdvType, ProjectId);
            documentList = (from DataRow dr in DocumentList.Rows
                            select new JournaldocumentList()
                            {
                                id = Convert.ToInt32(dr["id"]),
                                doctype = Convert.ToString(dr["doctype"]),
                                No = Convert.ToString(dr["No"]),
                                actAmt = Convert.ToDecimal(dr["actAmt"]),
                                unPdAmt = Convert.ToDecimal(dr["unPdAmt"]),
                                uniqueid = Convert.ToString(dr["uniqueid"]),
                                docDate = Convert.ToString(dr["invDate"]),
                                // cur = Convert.ToString(dr["cur"])
                                cur = Convert.ToString(1),
                                //Proj_Id = Convert.ToString(dr["Proj_Id"]),
                                //Proj_Code = Convert.ToString(dr["Proj_Code"]),
                                //HIERARCHY_NAME = Convert.ToString(dr["HIERARCHY_NAME"])

                            }).ToList();
            return documentList;
        }
        #endregion

        #region Adjustment of journal with Vendor Receipt

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetJournalDetorsVRVoucharList(string Mode, string CustomerId, string date, string Branch)
        {
            List<RpList> ReturnList = new List<RpList>();
            CustomerReceiptAdjustmentBl blLayer = new CustomerReceiptAdjustmentBl();
            DataTable ReceiptList = blLayer.GetJournalDetorsVendorRecListAdd(date, CustomerId, Branch);
            ReturnList = (from DataRow dr in ReceiptList.Rows
                          select new RpList()
                          {
                              ArId = Convert.ToInt32(dr["ReceiptPayment_uniqueID"]),
                              DocId = Convert.ToInt32(dr["ReceiptPayment_ID"]),
                              docNo = Convert.ToString(dr["ReceiptPayment_VoucherNumber"]),
                              docDt = Convert.ToString(dr["ReceiptPayment_TransactionDate"]),
                              ActAmt = Convert.ToDecimal(dr["Actual_Receipt_Amount"]),
                              avlAmt = Convert.ToDecimal(dr["Curent_Available_Amount"]),
                              //Cur = Convert.ToInt32(dr["ReceiptPayment_Currency"]),
                              Cur = Convert.ToInt32(1),
                              CurRate = Convert.ToInt32(dr["ReceiptPayment_Rate"]),
                              AdvType = Convert.ToString(dr["AdvType"]),
                               Proj_Id = Convert.ToString(dr["Proj_Id"]),
                                Proj_Code = Convert.ToString(dr["Proj_Code"]),
                                HIERARCHY_NAME = Convert.ToString(dr["HIERARCHY_NAME"])
                          }).ToList();
            return ReturnList;
        }
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetVendorReceiptOnAccountDocumentList(string Mode, string ReceiptId, string customerId, string TransDate, string AdjId, string BranchId,
            string AdvType, string ProjectId)
        {
            List<documentList> documentList = new List<CustomerReceiptAdjustment.documentList>();
            CustomerReceiptAdjustmentBl blLayer = new CustomerReceiptAdjustmentBl();
            DataTable DocumentList = blLayer.GetVendorRecDocumentList(Mode, ReceiptId, customerId, TransDate, AdjId, BranchId, AdvType, ProjectId);
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
                                //cur = Convert.ToString(1)


                            }).ToList();
            return documentList;
        }
        #endregion
    }
}
