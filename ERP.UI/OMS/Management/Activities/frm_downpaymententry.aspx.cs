using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogicLayer;
using System.Data;
using System.Configuration;
using EntityLayer.CommonELS;
using System.Drawing;
using DataAccessLayer;
using ERP.Models;

namespace ERP.OMS.Management.Activities
{

    public partial class frm_downpaymententry : ERP.OMS.ViewState_class.VSPage
    {

       // BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);

        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        BusinessLogicLayer.Converter objConverter = new BusinessLogicLayer.Converter();
        SlaesActivitiesBL objSlaesActivitiesBL = new SlaesActivitiesBL();
        DownPaymentEntry downPaymentEntry = new DownPaymentEntry();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.Session["userid"] != null)
            {
                rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/Management/Activities/frm_downpaymententry.aspx");
                if (!IsPostBack)
                {


                    String finyear = "";
                    finyear = Convert.ToString(Session["LastFinYear"]).Trim();
                    SlaesActivitiesBL objSlaesActivitiesBL = new SlaesActivitiesBL();
                    DataTable dtFinYear = objSlaesActivitiesBL.GetFinacialYearBasedQouteDate(finyear);
                    Session["FinYearStartDate"] = Convert.ToString(dtFinYear.Rows[0]["finYearStartDate"]);
                    Session["FinYearEndDate"] = Convert.ToString(dtFinYear.Rows[0]["finYearEndDate"]);

                    FormDate.MaxDate = Convert.ToDateTime(Session["FinYearEndDate"]);
                    FormDate.MinDate = Convert.ToDateTime(Session["FinYearStartDate"]);
                    toDate.MaxDate = Convert.ToDateTime(Session["FinYearEndDate"]);
                    toDate.MinDate = Convert.ToDateTime(Session["FinYearStartDate"]);

                    CustomerDataSource.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                    dsProduct.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);


                    string userbranchHierachy = Convert.ToString(Session["userbranchHierarchy"]);
                    string FinYear = Convert.ToString(Session["LastFinYear"]);
                    PopulateDropDown(userbranchHierachy, FinYear);
                }
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
        }
        protected void Page_PreInit(object sender, EventArgs e) // lead add
        {
            if (!IsPostBack)
            {
                string sPath = Convert.ToString(HttpContext.Current.Request.Url);
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }

        #region Grid Event

        //protected void downpaygrid_DataBinding(object sender, EventArgs e)
        //{
        //    DataTable sourceTable = (DataTable)Session["DownPaymentSalesInvoiceList"];
        //    if (sourceTable != null)
        //        downpaygrid.DataSource = sourceTable;
        //}
        protected void downpaygrid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            string clientParam = e.Parameters;

            if (clientParam.Split('~')[0] == "FilterGridByDate")
            {
                string fromdate = e.Parameters.Split('~')[1];
                string toDate = e.Parameters.Split('~')[2];
                string branch = e.Parameters.Split('~')[3];

                DataTable dtdata = new DataTable();
                dtdata = downPaymentEntry.GetInvoiceListGridDataByDate(Convert.ToString(Session["userbranchHierarchy"]), Convert.ToString(HttpContext.Current.Session["LastCompany"]), fromdate, toDate, branch);
                if (dtdata != null)
                {
                    //Session["DownPaymentSalesInvoiceList"] = dtdata;
                    downpaygrid.DataBind();
                }
            }
            else if (clientParam.Split('~')[0] == "SaveDownPayment")
            {
                if (SaveDownPayment())
                {
                    string Action = "Add";
                    if (Convert.ToString(hdfID.Value) != "" && Convert.ToString(hdfID.Value) != "0") Action = "Edit";

                    if (Action == "Add")
                    {
                        downpaygrid.JSProperties["cpMsg"] = "Saved Successfully.";
                    }
                    else if (Action == "Edit")
                    {
                        downpaygrid.JSProperties["cpMsg"] = "Updated Successfully.";
                    }
                }
                else
                {
                    downpaygrid.JSProperties["cpMsg"] = "Please Try Again.";
                }
            }
            else if (clientParam.Split('~')[0] == "Delete")
            {
                string ID = Convert.ToString(clientParam.Split('~')[1]);
                if (DeleteDownPayment(ID))
                {
                    downpaygrid.JSProperties["cpMsg"] = "Deleted Successfully.";
                }
                else
                {
                    downpaygrid.JSProperties["cpMsg"] = "Please Try Again.";
                }
            }
        }
        protected void downpaygrid_SummaryDisplayText(object sender, ASPxGridViewSummaryDisplayTextEventArgs e)
        {
            e.Text = string.Format("{0}", e.Value);
        }
        protected bool DeleteDownPayment(string ID)
        {
            bool returnVal = true;
            try
            {
                downPaymentEntry.DeleteOpening(ID);
            }
            catch (Exception e)
            {
                returnVal = false;
            }
            return returnVal;
        }
        protected bool SaveDownPayment()
        {
            bool returnVal = true;
            try
            {
                string DownPaymentNumber = txtdownPayNo.Text;
                string EntryDate = dtEntryDate.Date.ToString("yyyy-MM-dd");
                string DpDate = dtDpDate.Date.ToString("yyyy-MM-dd");
                string financer = Convert.ToString(cmbFinancer.Value);
                string Branch = Convert.ToString(cmbBranch.Value);
                string BillNo = txtBillNo.Text;
                decimal BillAmount = Convert.ToDecimal(txtBillAmount.Text);
                string BillDate = dtBilldate.Date.ToString("yyyy-MM-dd");
                string ChallanNo = txtChallanNo.Text;
                string Customer = Convert.ToString(CustomerComboBox.Value);
                decimal FinanceAmount = Convert.ToDecimal(txtFinanceAmount.Text);
                string SFCode = txtSfCode.Text;
                string ModeofPayment = txtModeofPayment.Text;
                decimal DownPay1 = Convert.ToDecimal(txtDownPay1.Text);
                string AdjmntNo = txtAdjmntNo.Text;
                String AdjmntDt = txtAdjmntDt.Text;
                decimal DownPay2 = Convert.ToDecimal(txtDownPay2.Text);
                string FinalMr = txtfinalMr.Text;
                decimal FinalPayment = Convert.ToDecimal(txtfinalPayment.Text);
                decimal DbdParcentage = Convert.ToDecimal(txtDbdPercentage.Text);
                decimal DbdAmount = Convert.ToDecimal(txtDbdAmount.Text);
                decimal MbdParcentage = Convert.ToDecimal(txtMbdPercentage.Text);
                decimal MbdAmount = Convert.ToDecimal(txtMbdAmount.Text);
                decimal ProcessingFee = Convert.ToDecimal(txtProcessingFee.Text);
                decimal TotalPay = Convert.ToDecimal(txtTotalPay.Text);
                decimal Balance = Convert.ToDecimal(txtbalance.Text);
                string DpStatus = Convert.ToString(cmbStatus.Value);
                string Narration = txtNaration.Text;

                string DivestmentNo1 = txtDivestmentNo1.Text;
                string DivestmentDt1 = txtDivestmentDT1.Text;
                decimal DivestmentAmt1 = Convert.ToDecimal(txtDivestmentAmt1.Text);

                string DivestmentNo2 = txtDivestmentNo2.Text;
                string DivestmentDt2 = txtDivestmentDT2.Text;
                decimal DivestmentAmt2 = Convert.ToDecimal(txtDivestmentAmt2.Text);

                string DivestmentNo3 = txtDivestmentNo3.Text;
                string DivestmentDt3 = txtDivestmentDT3.Text;
                decimal DivestmentAmt3 = Convert.ToDecimal(txtDivestmentAmt3.Text);
                string product = txtproduct.Text;
                string InvoiceID = Convert.ToString(hdfInvoiceID.Value);

                string Action = "AddOP";
                string DownPaymentID = "";

                if (Convert.ToString(hdfID.Value) != "" && Convert.ToString(hdfID.Value) != "0")
                {
                    Action = "EditOP";
                    DownPaymentID = Convert.ToString(hdfID.Value);
                }

                downPaymentEntry.AddOpening(DownPaymentNumber, EntryDate, DpDate, financer, Branch, BillNo, BillAmount, BillDate, ChallanNo, Customer, FinanceAmount,
                    SFCode, ModeofPayment, DownPay1, AdjmntNo, AdjmntDt, DownPay2, FinalMr, FinalPayment, DbdParcentage, DbdAmount, MbdParcentage, MbdAmount,
                    ProcessingFee, TotalPay, Balance, DpStatus, Narration, DivestmentNo1, DivestmentDt1, DivestmentAmt1,
                    DivestmentNo2, DivestmentDt2, DivestmentAmt2, DivestmentNo3, DivestmentDt3, DivestmentAmt3, product, DownPaymentID, InvoiceID, Action);
            }
            catch (Exception e)
            {
                returnVal = false;
            }
            return returnVal;
        }

        #endregion

        #region Other Event

        protected void CallbackPanel_Callback(object sender, CallbackEventArgsBase e)
        {
            string performpara = e.Parameter;

            if (performpara.Split('~')[0] == "Edit")
            {
                string Action = "";
                int RowIndex = Convert.ToInt32(performpara.Split('~')[1]);
                string ReconciliatonID = downpaygrid.GetRowValues(RowIndex, "ID").ToString();
                string InvoiceID = downpaygrid.GetRowValues(RowIndex, "Invoice_Id").ToString();

                if (ReconciliatonID != "0") Action = "Edit";
                else Action = "InvoiceEdit";

                if (Action == "Edit")
                {
                    string SrlNo = ReconciliatonID;

                    DataTable dtdata = downPaymentEntry.GetDownPaymentDetails(SrlNo);
                    if (dtdata != null && dtdata.Rows.Count > 0)
                    {
                        string DownPaymentNumber = Convert.ToString(dtdata.Rows[0]["DownPaymentNumber"]);
                        string EntryDate = Convert.ToString(dtdata.Rows[0]["EntryDate"]);
                        string DpDate = Convert.ToString(dtdata.Rows[0]["DownPaymentDate"]);
                        string financer = Convert.ToString(dtdata.Rows[0]["Financer"]);
                        string Branch = Convert.ToString(dtdata.Rows[0]["Branch"]);
                        string BillNo = Convert.ToString(dtdata.Rows[0]["BillNumber"]);
                        string BillAmount = Convert.ToString(dtdata.Rows[0]["BillAmount"]);
                        string BillDate = Convert.ToString(dtdata.Rows[0]["BillDate"]);
                        string ChallanNo = Convert.ToString(dtdata.Rows[0]["ChallanNumber"]);
                        string Customer = Convert.ToString(dtdata.Rows[0]["Customer"]);
                        string FinanceAmount = Convert.ToString(dtdata.Rows[0]["FinanceAmount"]);
                        string SFCode = Convert.ToString(dtdata.Rows[0]["SFCode"]);
                        string ModeofPayment = Convert.ToString(dtdata.Rows[0]["ModeOfPayment"]);
                        string DownPay1 = Convert.ToString(dtdata.Rows[0]["DownPay1"]);
                        string AdjmntNo = Convert.ToString(dtdata.Rows[0]["AdjustmentNo"]);
                        string AdjmntDt = Convert.ToString(dtdata.Rows[0]["AdjustmentDate"]);
                        string DownPay2 = Convert.ToString(dtdata.Rows[0]["DownPay2"]);
                        string FinalMr = Convert.ToString(dtdata.Rows[0]["FinalMr"]);
                        string FinalPayment = Convert.ToString(dtdata.Rows[0]["FinalPayment"]);
                        string DbdParcentage = Convert.ToString(dtdata.Rows[0]["DbdParcentage"]);
                        string DbdAmount = Convert.ToString(dtdata.Rows[0]["DbdAmount"]);
                        string MbdParcentage = Convert.ToString(dtdata.Rows[0]["MbdParcentage"]);
                        string MbdAmount = Convert.ToString(dtdata.Rows[0]["MbdAmount"]);
                        string ProcessingFee = Convert.ToString(dtdata.Rows[0]["ProcessFee"]);
                        string TotalPay = Convert.ToString(dtdata.Rows[0]["TotalPay"]);
                        string Balance = Convert.ToString(dtdata.Rows[0]["Balance"]);
                        string DpStatus = Convert.ToString(dtdata.Rows[0]["DpStatus"]);
                        string Narration = Convert.ToString(dtdata.Rows[0]["Narration"]);
                        string DivestmentNo1 = Convert.ToString(dtdata.Rows[0]["DivestmentNo1"]);
                        string DivestmentDt1 = Convert.ToString(dtdata.Rows[0]["DivestmentDt1"]);
                        string DivestmentAmt1 = Convert.ToString(dtdata.Rows[0]["DivestmentAmt1"]);
                        string DivestmentNo2 = Convert.ToString(dtdata.Rows[0]["DivestmentNo2"]);
                        string DivestmentDt2 = Convert.ToString(dtdata.Rows[0]["DivestmentDt2"]);
                        string DivestmentAmt2 = Convert.ToString(dtdata.Rows[0]["DivestmentAmt2"]);
                        string DivestmentNo3 = Convert.ToString(dtdata.Rows[0]["DivestmentNo3"]);
                        string DivestmentDt3 = Convert.ToString(dtdata.Rows[0]["DivestmentDt3"]);
                        string DivestmentAmt3 = Convert.ToString(dtdata.Rows[0]["DivestmentAmt3"]);
                        string product = Convert.ToString(dtdata.Rows[0]["Product"]);
                        string InvoiceId = Convert.ToString(dtdata.Rows[0]["InvoiceId"]);

                        hdfID.Value = SrlNo;
                        hdfInvoiceID.Value = InvoiceId;
                        bidfinancerByBranch(Branch);

                        dtEntryDate.Enabled = true;
                        dtDpDate.Enabled = true;

                        txtdownPayNo.Text = DownPaymentNumber;
                        dtEntryDate.Date = Convert.ToDateTime(EntryDate);
                        dtDpDate.Date = Convert.ToDateTime(DpDate);
                        cmbFinancer.Value = financer;
                        cmbBranch.Value = Branch;
                        txtBillNo.Text = BillNo;
                        txtBillAmount.Text = BillAmount;
                        dtBilldate.Date = Convert.ToDateTime(BillDate);
                        txtChallanNo.Text = ChallanNo;
                        SetCustomerDDbyValue(Customer);
                        txtFinanceAmount.Text = FinanceAmount;
                        txtSfCode.Text = SFCode;
                        txtModeofPayment.Text = ModeofPayment;
                        txtDownPay1.Text = DownPay1;
                        txtAdjmntNo.Text = AdjmntNo;
                        txtAdjmntDt.Text = AdjmntDt;
                        txtDownPay2.Text = DownPay2;
                        txtfinalMr.Text = FinalMr;
                        txtfinalPayment.Text = FinalPayment;
                        txtDbdPercentage.Text = DbdParcentage;
                        txtDbdAmount.Text = DbdAmount;
                        txtMbdPercentage.Text = MbdParcentage;
                        txtMbdAmount.Text = MbdAmount;
                        txtProcessingFee.Text = ProcessingFee;
                        txtTotalPay.Text = TotalPay;
                        txtbalance.Text = Balance;
                        cmbStatus.Value = DpStatus;
                        txtNaration.Text = Narration;
                        txtDivestmentNo1.Text = DivestmentNo1;
                        txtDivestmentDT1.Text = DivestmentDt1;
                        txtDivestmentAmt1.Text = DivestmentAmt1;
                        txtDivestmentNo2.Text = DivestmentNo2;
                        txtDivestmentDT2.Text = DivestmentDt2;
                        txtDivestmentAmt2.Text = DivestmentAmt2;
                        txtDivestmentNo3.Text = DivestmentNo3;
                        txtDivestmentDT3.Text = DivestmentDt3;
                        txtDivestmentAmt3.Text = DivestmentAmt3;
                        txtproduct.Text = product;

                        dtEntryDate.ClientEnabled = false;
                        dtDpDate.ClientEnabled = false;

                        if (InvoiceId != "0")
                        {
                            //txtproduct.Enabled = false;
                            //txtdownPayNo.Enabled = false;
                            //dtEntryDate.ClientEnabled = false;
                            //dtDpDate.ClientEnabled = false;
                            //cmbFinancer.ClientEnabled = false;
                            //cmbBranch.ClientEnabled = false;
                            //txtBillNo.Enabled = false;
                            //txtBillAmount.ClientEnabled = false;
                            //dtBilldate.ClientEnabled = false;
                            //txtChallanNo.Enabled = false;
                            //CustomerComboBox.ClientEnabled = false;
                            //txtFinanceAmount.ClientEnabled = false;
                            //txtSfCode.Enabled = false;
                            //txtDownPay1.ClientEnabled = false;
                            //txtModeofPayment.Enabled = false;
                            //txtfinalMr.Enabled = false;
                            //txtfinalPayment.ClientEnabled = false;
                            //txtDbdPercentage.ClientEnabled = false;
                            //txtDbdAmount.ClientEnabled = false;
                            //txtMbdPercentage.ClientEnabled = false;
                            //txtMbdAmount.ClientEnabled = false;
                            //txtProcessingFee.ClientEnabled = false;
                            //txtTotalPay.ClientEnabled = false;
                            //txtbalance.ClientEnabled = false;
                            //cmbStatus.ClientEnabled = false;
                            //txtNaration.Enabled = false;
                            //txtDivestmentNo1.Enabled = false;
                            //txtDivestmentDT1.Enabled = false;
                            //txtDivestmentAmt1.ClientEnabled = false;
                            //txtDivestmentNo2.Enabled = false;
                            //txtDivestmentDT2.Enabled = false;
                            //txtDivestmentAmt2.ClientEnabled = false;
                            //txtDivestmentNo3.Enabled = false;
                            //txtDivestmentDT3.Enabled = false;
                            //txtDivestmentAmt3.ClientEnabled = false;

                            //txtAdjmntNo.Enabled = true;
                            //txtAdjmntDt.Enabled = true;
                            //txtDownPay2.ClientEnabled = true;

                            txtproduct.Enabled = false;
                            txtdownPayNo.Enabled = false;
                            dtEntryDate.ClientEnabled = false;
                            dtDpDate.ClientEnabled = false;
                            cmbFinancer.ClientEnabled = false;
                            cmbBranch.ClientEnabled = false;
                            txtBillNo.Enabled = false;
                            txtBillAmount.ClientEnabled = false;
                            dtBilldate.ClientEnabled = false;
                            txtChallanNo.Enabled = false;
                            CustomerComboBox.ClientEnabled = false;
                            txtFinanceAmount.ClientEnabled = false;
                            txtSfCode.Enabled = false;
                            txtDownPay1.ClientEnabled = false;
                        }
                    }
                }
                else if (Action == "InvoiceEdit")
                {
                    string SrlNo = InvoiceID;

                    DataTable dtdata = downPaymentEntry.GetFinInvoiceDetails(SrlNo);
                    if (dtdata != null && dtdata.Rows.Count > 0)
                    {
                        string DownPaymentNumber = Convert.ToString(dtdata.Rows[0]["DownPaymentNumber"]);
                        string EntryDate = Convert.ToString(dtdata.Rows[0]["EntryDate"]);
                        string DpDate = Convert.ToString(dtdata.Rows[0]["DownPaymentDate"]);
                        string financer = Convert.ToString(dtdata.Rows[0]["Financer"]);
                        string Branch = Convert.ToString(dtdata.Rows[0]["Branch"]);
                        string BillNo = Convert.ToString(dtdata.Rows[0]["BillNumber"]);
                        string BillAmount = Convert.ToString(dtdata.Rows[0]["BillAmount"]);
                        string BillDate = Convert.ToString(dtdata.Rows[0]["BillDate"]);
                        string ChallanNo = Convert.ToString(dtdata.Rows[0]["ChallanNumber"]);
                        string Customer = Convert.ToString(dtdata.Rows[0]["Customer"]);
                        string FinanceAmount = Convert.ToString(dtdata.Rows[0]["FinanceAmount"]);
                        string SFCode = Convert.ToString(dtdata.Rows[0]["SFCode"]);
                        string ModeofPayment = Convert.ToString(dtdata.Rows[0]["ModeOfPayment"]);
                        string DownPay1 = Convert.ToString(dtdata.Rows[0]["DownPay1"]);
                        string AdjmntNo = Convert.ToString(dtdata.Rows[0]["AdjustmentNo"]);
                        string AdjmntDt = Convert.ToString(dtdata.Rows[0]["AdjustmentDate"]);
                        string DownPay2 = Convert.ToString(dtdata.Rows[0]["DownPay2"]);
                        string FinalMr = Convert.ToString(dtdata.Rows[0]["FinalMr"]);
                        string FinalPayment = Convert.ToString(dtdata.Rows[0]["FinalPayment"]);
                        string DbdParcentage = Convert.ToString(dtdata.Rows[0]["DbdParcentage"]);
                        string DbdAmount = Convert.ToString(dtdata.Rows[0]["DbdAmount"]);
                        string MbdParcentage = Convert.ToString(dtdata.Rows[0]["MbdParcentage"]);
                        string MbdAmount = Convert.ToString(dtdata.Rows[0]["MbdAmount"]);
                        string ProcessingFee = Convert.ToString(dtdata.Rows[0]["ProcessFee"]);
                        string TotalPay = Convert.ToString(dtdata.Rows[0]["TotalPay"]);
                        string Balance = Convert.ToString(dtdata.Rows[0]["Balance"]);
                        string DpStatus = Convert.ToString(dtdata.Rows[0]["DpStatus"]);
                        string Narration = Convert.ToString(dtdata.Rows[0]["Narration"]);
                        string DivestmentNo1 = Convert.ToString(dtdata.Rows[0]["DivestmentNo1"]);
                        string DivestmentDt1 = Convert.ToString(dtdata.Rows[0]["DivestmentDt1"]);
                        string DivestmentAmt1 = Convert.ToString(dtdata.Rows[0]["DivestmentAmt1"]);
                        string DivestmentNo2 = Convert.ToString(dtdata.Rows[0]["DivestmentNo2"]);
                        string DivestmentDt2 = Convert.ToString(dtdata.Rows[0]["DivestmentDt2"]);
                        string DivestmentAmt2 = Convert.ToString(dtdata.Rows[0]["DivestmentAmt2"]);
                        string DivestmentNo3 = Convert.ToString(dtdata.Rows[0]["DivestmentNo3"]);
                        string DivestmentDt3 = Convert.ToString(dtdata.Rows[0]["DivestmentDt3"]);
                        string DivestmentAmt3 = Convert.ToString(dtdata.Rows[0]["DivestmentAmt3"]);
                        string product = Convert.ToString(dtdata.Rows[0]["Product"]);
                        string Id = Convert.ToString(dtdata.Rows[0]["Id"]);
                        string InvoiceId = Convert.ToString(dtdata.Rows[0]["InvoiceId"]);

                        if (Id != "" && Id != "0") hdfID.Value = Id;
                        else hdfID.Value = "";

                        hdfInvoiceID.Value = InvoiceId;
                        bidfinancerByBranch(Branch);

                        dtEntryDate.Enabled = true;
                        dtDpDate.Enabled = true;

                        txtdownPayNo.Text = DownPaymentNumber;
                        dtEntryDate.Date = Convert.ToDateTime(EntryDate);
                        dtDpDate.Date = Convert.ToDateTime(DpDate);
                        cmbFinancer.Value = financer;
                        cmbBranch.Value = Branch;
                        txtBillNo.Text = BillNo;
                        txtBillAmount.Text = BillAmount;
                        dtBilldate.Date = Convert.ToDateTime(BillDate);
                        txtChallanNo.Text = ChallanNo;
                        SetCustomerDDbyValue(Customer);
                        txtFinanceAmount.Text = FinanceAmount;
                        txtSfCode.Text = SFCode;
                        txtModeofPayment.Text = ModeofPayment;
                        txtDownPay1.Text = DownPay1;
                        txtAdjmntNo.Text = AdjmntNo;
                        txtAdjmntDt.Text = AdjmntDt;
                        txtDownPay2.Text = DownPay2;
                        txtfinalMr.Text = FinalMr;
                        txtfinalPayment.Text = FinalPayment;
                        txtDbdPercentage.Text = DbdParcentage;
                        txtDbdAmount.Text = DbdAmount;
                        txtMbdPercentage.Text = MbdParcentage;
                        txtMbdAmount.Text = MbdAmount;
                        txtProcessingFee.Text = ProcessingFee;
                        txtTotalPay.Text = TotalPay;
                        txtbalance.Text = Balance;
                        cmbStatus.Value = DpStatus;
                        txtNaration.Text = Narration;
                        txtDivestmentNo1.Text = DivestmentNo1;
                        txtDivestmentDT1.Text = DivestmentDt1;
                        txtDivestmentAmt1.Text = DivestmentAmt1;
                        txtDivestmentNo2.Text = DivestmentNo2;
                        txtDivestmentDT2.Text = DivestmentDt2;
                        txtDivestmentAmt2.Text = DivestmentAmt2;
                        txtDivestmentNo3.Text = DivestmentNo3;
                        txtDivestmentDT3.Text = DivestmentDt3;
                        txtDivestmentAmt3.Text = DivestmentAmt3;
                        txtproduct.Text = product;

                        if (Id == "")
                        {
                            decimal _TotalPay = Convert.ToDecimal(txtDownPay1.Text) + Convert.ToDecimal(txtDownPay2.Text) +
                                                Convert.ToDecimal(txtDivestmentAmt1.Text) + Convert.ToDecimal(txtDivestmentAmt2.Text) + Convert.ToDecimal(txtDivestmentAmt3.Text) +
                                                Convert.ToDecimal(txtDbdAmount.Text) + Convert.ToDecimal(txtMbdAmount.Text) + Convert.ToDecimal(txtProcessingFee.Text);

                            decimal _Balance = Convert.ToDecimal(txtBillAmount.Text) - Convert.ToDecimal(_TotalPay);
                            decimal _BillAmount = Convert.ToDecimal(txtBillAmount.Text);

                            txtTotalPay.Text = Convert.ToString(_TotalPay);
                            txtbalance.Text = Convert.ToString(_Balance);

                            if (_BillAmount == _TotalPay)
                            {
                                cmbStatus.Value = "C";
                            }
                            else if (_BillAmount > _TotalPay)
                            {
                                if ((_BillAmount - _TotalPay) <= 999)
                                {
                                    cmbStatus.Value = "S";
                                }
                                else if ((_BillAmount - _TotalPay) > 999)
                                {
                                    cmbStatus.Value = "O";
                                }
                            }
                            else if (_BillAmount < _TotalPay)
                            {
                                cmbStatus.Value = "E";
                            }

                            txtproduct.Enabled = false;
                            txtdownPayNo.Enabled = false;
                            dtEntryDate.ClientEnabled = false;
                            dtDpDate.ClientEnabled = false;
                            cmbFinancer.ClientEnabled = false;
                            cmbBranch.ClientEnabled = false;
                            txtBillNo.Enabled = false;
                            txtBillAmount.ClientEnabled = false;
                            dtBilldate.ClientEnabled = false;
                            txtChallanNo.Enabled = false;
                            CustomerComboBox.ClientEnabled = false;
                            txtFinanceAmount.ClientEnabled = false;
                            txtSfCode.Enabled = false;
                            txtDownPay1.ClientEnabled = false;
                        }
                        else
                        {
                            txtproduct.Enabled = false;
                            txtdownPayNo.Enabled = false;
                            dtEntryDate.ClientEnabled = false;
                            dtDpDate.ClientEnabled = false;
                            cmbFinancer.ClientEnabled = false;
                            cmbBranch.ClientEnabled = false;
                            txtBillNo.Enabled = false;
                            txtBillAmount.ClientEnabled = false;
                            dtBilldate.ClientEnabled = false;
                            txtChallanNo.Enabled = false;
                            CustomerComboBox.ClientEnabled = false;
                            txtFinanceAmount.ClientEnabled = false;
                            txtSfCode.Enabled = false;
                            txtDownPay1.ClientEnabled = false;
                            txtModeofPayment.Enabled = false;
                            txtfinalMr.Enabled = false;
                            txtfinalPayment.ClientEnabled = false;
                            txtDbdPercentage.ClientEnabled = false;
                            txtDbdAmount.ClientEnabled = false;
                            txtMbdPercentage.ClientEnabled = false;
                            txtMbdAmount.ClientEnabled = false;
                            txtProcessingFee.ClientEnabled = false;
                            txtTotalPay.ClientEnabled = false;
                            txtbalance.ClientEnabled = false;
                            cmbStatus.ClientEnabled = false;
                            txtNaration.Enabled = false;
                            txtDivestmentNo1.Enabled = false;
                            txtDivestmentDT1.Enabled = false;
                            txtDivestmentAmt1.ClientEnabled = false;
                            txtDivestmentNo2.Enabled = false;
                            txtDivestmentDT2.Enabled = false;
                            txtDivestmentAmt2.ClientEnabled = false;
                            txtDivestmentNo3.Enabled = false;
                            txtDivestmentDT3.Enabled = false;
                            txtDivestmentAmt3.ClientEnabled = false;

                            txtAdjmntNo.Enabled = true;
                            txtAdjmntDt.Enabled = true;
                            txtDownPay2.ClientEnabled = true;
                        }
                    }
                }
            }
        }
        public void PopulateDropDown(string branchHierchy, string FinYear)
        {
            DataSet dst = downPaymentEntry.GetAllDropDownDetail(branchHierchy, FinYear);

            FormDate.Date = DateTime.Now;
            toDate.Date = DateTime.Now;
            dtBilldate.Date = DateTime.Now;

            if (dst.Tables[0] != null && dst.Tables[0].Rows.Count > 0)
            {
                cmbBranchfilter.DataSource = dst.Tables[0];
                cmbBranchfilter.ValueField = "branch_id";
                cmbBranchfilter.TextField = "branch_description";
                cmbBranchfilter.DataBind();

                cmbBranchfilter.Value = Convert.ToString(Session["userbranchID"]);
            }

            //if (dst.Tables[1] != null && dst.Tables[1].Rows.Count > 0)
            //{
            //    cmbFinancer.DataSource = dst.Tables[1];
            //    cmbFinancer.ValueField = "ID";
            //    cmbFinancer.TextField = "FinancerName";
            //    cmbFinancer.DataBind();
            //    cmbFinancer.SelectedIndex = 0;
            //}

            string finyear = "";
            DateTime MinDate, MaxDate;

            if (Session["LastFinYear"] != null)
            {
                finyear = Convert.ToString(Session["LastFinYear"]).Trim();
                DataTable dtFinYear = dst.Tables[2];
                if (dtFinYear != null && dtFinYear.Rows.Count > 0)
                {
                    MinDate = Convert.ToDateTime(dtFinYear.Rows[0]["finYearStartDate"]);
                    MaxDate = Convert.ToDateTime(dtFinYear.Rows[0]["finYearEndDate"]);

                    dtEntryDate.Value = null;
                    dtEntryDate.MinDate = MinDate;
                    dtEntryDate.MaxDate = MaxDate;
                    hdfDate.Value = Convert.ToString(MinDate);

                    if (DateTime.Now > MaxDate)
                    {
                        dtEntryDate.Value = MaxDate;
                    }
                    else
                    {
                        dtEntryDate.Value = DateTime.Now;
                    }
                }
            }

            if (dst.Tables[3] != null && dst.Tables[3].Rows.Count > 0)
            {
                cmbBranch.DataSource = dst.Tables[3];
                cmbBranch.ValueField = "branch_id";
                cmbBranch.TextField = "branch_description";
                cmbBranch.DataBind();
            }
        }
        protected void cmbFinancer_Callback(object sender, CallbackEventArgsBase e)
        {
            string branchID = Convert.ToString(cmbBranch.Value);
            if (branchID != "")
            {
                bidfinancerByBranch(branchID);
            }
            else
            {
                bidfinancerByBranch("0");
            }
        }
        public void bidfinancerByBranch(string branchId)
        {
            ProcedureExecute proc = new ProcedureExecute("prc_PosCRMSalesInvoice_Details");
            proc.AddVarcharPara("@Action", 500, "GetFinancerBranch");
            proc.AddVarcharPara("@BranchID", 100, branchId);
            DataTable FinancerDt = proc.GetTable();

            if (FinancerDt != null && FinancerDt.Rows.Count > 0)
            {
                cmbFinancer.TextField = "cnt_firstName";
                cmbFinancer.ValueField = "cnt_internalId";
                cmbFinancer.DataSource = FinancerDt;
                cmbFinancer.DataBind();
            }
        }

        #endregion

        #region Export Event

        protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(Convert.ToString(drdExport.SelectedItem.Value));
            if (Filter != 0)
            {
                bindexport(Filter);
            }
        }
        public void bindexport(int Filter)
        {
            downpaygrid.Columns[12].Visible = false;
            exporter.FileName = "Downpayment Entry";
            exporter.GridViewID = "downpaygrid";

            exporter.PageHeader.Left = "Downpayment Entry";
            exporter.PageFooter.Center = "[Page # of Pages #]";
            exporter.PageFooter.Right = "[Date Printed]";

            switch (Filter)
            {
                case 1:
                    exporter.WritePdfToResponse();
                    break;
                case 2:
                    exporter.WriteXlsToResponse();
                    break;
                case 3:
                    exporter.WriteRtfToResponse();
                    break;
                case 4:
                    exporter.WriteCsvToResponse();
                    break;
            }
        }

        #endregion

        #region predictive Customer
        protected void ASPxComboBox_OnItemsRequestedByFilterCondition_SQL(object source, ListEditItemsRequestedByFilterConditionEventArgs e)
        {
            if (e.Filter != "")
            {
                ASPxComboBox comboBox = (ASPxComboBox)source;
                CustomerDataSource.SelectCommand =
                       @"select cnt_internalid,uniquename,Name,Billing from (SELECT cnt_internalid,uniquename,Name,Billing, row_number()over(order by t.[Name]) as [rn]  from v_pos_customerDetails  as t where (([uniquename] + ' ' + [Name] ) LIKE @filter)) as st where st.[rn] between @startIndex and @endIndex";

                CustomerDataSource.SelectParameters.Clear();
                CustomerDataSource.SelectParameters.Add("filter", TypeCode.String, string.Format("%{0}%", e.Filter));
                CustomerDataSource.SelectParameters.Add("startIndex", TypeCode.Int64, (e.BeginIndex + 1).ToString());
                CustomerDataSource.SelectParameters.Add("endIndex", TypeCode.Int64, (e.EndIndex + 1).ToString());
                comboBox.DataSource = CustomerDataSource;
                comboBox.DataBind();
            }
        }
        protected void ASPxComboBox_OnItemRequestedByValue_SQL(object source, ListEditItemRequestedByValueEventArgs e)
        {
            long value = 0;
            if (e.Value == null || !Int64.TryParse(e.Value.ToString(), out value))
                return;
            ASPxComboBox comboBox = (ASPxComboBox)source;
            CustomerDataSource.SelectCommand = @"SELECT cnt_internalid,uniquename,Name,Billing FROM v_pos_customerDetails WHERE (cnt_internalid = @ID) ";

            CustomerDataSource.SelectParameters.Clear();
            CustomerDataSource.SelectParameters.Add("ID", TypeCode.String, e.Value.ToString());
            comboBox.DataSource = CustomerDataSource;
            comboBox.DataBind();
        }
        protected void SetCustomerDDbyValue(string customerId)
        {
            CustomerDataSource.SelectCommand = @"SELECT cnt_internalid,uniquename,Name,Billing FROM v_pos_customerDetails WHERE (cnt_internalid = @ID) ";

            CustomerDataSource.SelectParameters.Clear();
            CustomerDataSource.SelectParameters.Add("ID", TypeCode.String, customerId);
            CustomerComboBox.DataSource = CustomerDataSource;
            CustomerComboBox.DataBind();
            CustomerComboBox.Value = customerId;
        }

        #endregion

        protected void EntityServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "SrlNo";

            //string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString;
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);

            string IsFilter = Convert.ToString(hfIsFilter.Value);
            string strFromDate = Convert.ToString(hfFromDate.Value);
            string strToDate = Convert.ToString(hfToDate.Value);
            string strBranchID = (Convert.ToString(hfBranchID.Value) == "") ? "0" : Convert.ToString(hfBranchID.Value);

            List<int> branchidlist;

            if (IsFilter == "Y")
            {
                if (strBranchID == "0")
                {
                    string BranchList = Convert.ToString(Session["userbranchHierarchy"]);
                    branchidlist = new List<int>(Array.ConvertAll(BranchList.Split(','), int.Parse));

                    ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
                    var q = from d in dc.v_FinanceReconciliationLists
                            where d.CheckDate >= Convert.ToDateTime(strFromDate) && d.CheckDate <= Convert.ToDateTime(strToDate)
                            && branchidlist.Contains(Convert.ToInt32(d.BranchID))
                            orderby d.CheckDate descending
                            select d;
                    e.QueryableSource = q;
                }
                else
                {
                    branchidlist = new List<int>(Array.ConvertAll(strBranchID.Split(','), int.Parse));

                    ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
                    var q = from d in dc.v_FinanceReconciliationLists
                            where
                            d.CheckDate >= Convert.ToDateTime(strFromDate) && d.CheckDate <= Convert.ToDateTime(strToDate) &&
                            branchidlist.Contains(Convert.ToInt32(d.BranchID))
                            orderby d.CheckDate descending
                            select d;
                    e.QueryableSource = q;
                }
            }
            else
            {
                ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
                var q = from d in dc.v_FinanceReconciliationLists
                        where d.BranchID == '0'
                        orderby d.CheckDate descending
                        select d;
                e.QueryableSource = q;
            }
        }
    }
}