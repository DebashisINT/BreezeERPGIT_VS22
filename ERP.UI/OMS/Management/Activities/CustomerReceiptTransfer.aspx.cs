using BusinessLogicLayer;
using DataAccessLayer;
using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Web.UI.WebControls;

namespace ERP.OMS.Management.Activities
{
    public partial class CustomerReceiptTransfer : ERP.OMS.ViewState_class.VSPage
    {
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        public EntityLayer.CommonELS.UserRightsForPage rights = new EntityLayer.CommonELS.UserRightsForPage();
        CustomerVendorReceiptPaymentBL objCustomerVendorReceiptPaymentBL = new CustomerVendorReceiptPaymentBL();
        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //'http://localhost:2957/InfluxCRM/management/testProjectMainPage_employee.aspx'
                string sPath = HttpContext.Current.Request.Url.ToString();
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {

            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Activities/CustomerReceiptTransfer.aspx");
            if(!IsPostBack)
            {

                dsBranch.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);



                if (!String.IsNullOrEmpty(Convert.ToString(Session["userbranchID"])))
                {
                    string strdefaultBranch = Convert.ToString(Session["userbranchID"]);
                    ddlBranch.SelectedValue = strdefaultBranch;
                    //BindCashBankAccount(strdefaultBranch);
                }
                createBranchJson();
            }
            BindBranch();
            FillGrid();
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Activities/CustomerReceiptTransfer.aspx");
        }
        //public void BindCashBankAccount(string userbranch)
        //{
        //    DataTable dtCustomer = new DataTable();
        //    string CompanyId = Convert.ToString(Session["LastCompany"]);
        //    dtCustomer = objCustomerVendorReceiptPaymentBL.GetCustomerCashBank(userbranch, CompanyId);
        //    if (dtCustomer.Rows.Count > 0)
        //    {
        //        ddlCashBank.TextField = "IntegrateMainAccount";
        //        ddlCashBank.ValueField = "MainAccount_ReferenceID";
        //        ddlCashBank.DataSource = dtCustomer;
        //        ddlCashBank.DataBind();
        //    }
        //    else
        //    {
        //        ddlCashBank.TextField = "IntegrateMainAccount";
        //        ddlCashBank.ValueField = "MainAccount_ReferenceID";
        //        ddlCashBank.DataSource = dtCustomer;
        //        ddlCashBank.DataBind();
        //    }

        //}
        public void BindBranch()
        {
            dsBranch.SelectCommand = "SELECT BRANCH_id AS BANKBRANCH_ID , RTRIM(BRANCH_DESCRIPTION)+' ['+ISNULL(RTRIM(BRANCH_CODE),'')+']' AS BANKBRANCH_NAME  FROM TBL_MASTER_BRANCH where BRANCH_id in(" + Convert.ToString(Session["UserSiblingBranchHierarchy"]).TrimEnd(',') + ")";
            ddlBranch.DataBind();
        }  
        public void FillGrid()
        {
            //DataTable dtdata = GetGridData();
            DataTable dtdata = GetCustomerReceiptTransferListGridData();


            if (dtdata != null && dtdata.Rows.Count > 0)
            {
                Grid_CustomerReceiptTransfer.DataSource = dtdata;
                Grid_CustomerReceiptTransfer.DataBind();
            }
            else
            {
                Grid_CustomerReceiptTransfer.DataSource = null;
                Grid_CustomerReceiptTransfer.DataBind();
            }
        }
        public DataTable GetCustomerReceiptTransferListGridData()
        {

            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_CustomerReceiptPaymentDetails");
            proc.AddVarcharPara("@Action", 500, "GetCustomerReceiptTransferList");
            proc.AddVarcharPara("@FinYear", 500, Convert.ToString(Session["LastFinYear"]));
            proc.AddVarcharPara("@CompanyId", 500, Convert.ToString(Session["LastCompany"]));
            proc.AddVarcharPara("@userbranchlist", 500, Convert.ToString(Session["userbranchHierarchy"]));
            dt = proc.GetTable();
            return dt;
        }
        protected void ddlCashBank_Callback(object sender, CallbackEventArgsBase e)
        {
            string userbranch = e.Parameter.Split('~')[0];
           // BindCashBankAccount(userbranch);

        }
        public DataTable GetCustomerReceiptPaymentEditData(string ReceiptPaymentID)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_CustomerReceiptPaymentDetails");
            proc.AddVarcharPara("@Action", 500, "CustomerReceiptPaymentEditDetails");
            proc.AddIntegerPara("@ReceiptPayment_ID", Convert.ToInt32(ReceiptPaymentID));

            dt = proc.GetTable();
            return dt;
        }
        private bool IsCRTTransactionExist(string CRTid)
        {
            bool IsExist = false;
            if (CRTid != "" && Convert.ToString(CRTid).Trim() != "")
            {
                DataTable dt = new DataTable();
                dt = objCustomerVendorReceiptPaymentBL.CheckCRTTraanaction(CRTid);
                if (dt.Rows.Count > 0 && Convert.ToString(dt.Rows[0]["isexist"]) != "0")
                {
                    IsExist = true;
                }
            }

            return IsExist;
        }
        protected void BranchTransferCallBackPanel_Callback(object sender, CallbackEventArgsBase e)
        {
            string WhichCall = e.Parameter.Split('~')[0];
            if(WhichCall=="Edit")
            {
                string ReceiptPaymentID = e.Parameter.Split('~')[1];
                BranchTransferCallBackPanel.JSProperties["cpEdit"] = ReceiptPaymentID;
                DataTable CRPOrderEditdt = GetCustomerReceiptPaymentEditData(ReceiptPaymentID);
                if (CRPOrderEditdt != null && CRPOrderEditdt.Rows.Count > 0)
                {
                   // hdnEditID.Value = ReceiptPaymentID;
                    string BranchID = Convert.ToString(CRPOrderEditdt.Rows[0]["ReceiptPayment_TransferToBranch"]);   
                    if(BranchID!="")
                    {
                        ddlBranch.SelectedValue = BranchID;
                    }
                    string CashBankID = Convert.ToString(CRPOrderEditdt.Rows[0]["ReceiptPayment_CashBankID"]);     
                    string Narration = Convert.ToString(CRPOrderEditdt.Rows[0]["ReceiptPayment_Narration"]);
                    lblTransferFrom.Text = Convert.ToString(CRPOrderEditdt.Rows[0]["branch_description"]);
                    hdFromBranchID.Value = Convert.ToString(CRPOrderEditdt.Rows[0]["transferFromBranch"]); 

                   // ddlCashBank.Value = CashBankID;
                    txtNarration.Text = Narration.Trim();
                    if (IsCRTTransactionExist(ReceiptPaymentID))
                    {
                        BranchTransferCallBackPanel.JSProperties["cpBtnVisible"] = "false";
                    }
                }
            }
            if (WhichCall == "Save")
            {
                //string strCashBankBranchID = ddlBranch.SelectedValue;
                string strCashBankBranchID = e.Parameter.Split('~')[2];
                //string strCashBankID =Convert.ToString(ddlCashBank.Value);
                string strNarration = txtNarration.Text;

                try
                {
                    DataSet dsInst = new DataSet();


                    //SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);
                    SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));


                    SqlCommand cmd = new SqlCommand("prc_CustomerPaymentReceiptLogInsert", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    string ID = e.Parameter.Split('~')[1];

                    cmd.Parameters.AddWithValue("@EditReceiptPaymentID", Convert.ToInt16(ID));
                    cmd.Parameters.AddWithValue("@ReceiptPaymentBranchID", strCashBankBranchID);
                    //cmd.Parameters.AddWithValue("@ReceiptPaymentCashBankID", strCashBankID);
                    cmd.Parameters.AddWithValue("@Narration", strNarration);

                    cmd.Parameters.AddWithValue("@CompanyID", Convert.ToString(Session["LastCompany"]));
                    cmd.Parameters.AddWithValue("@FinYear", Convert.ToString(Session["LastFinYear"]));
                    cmd.Parameters.AddWithValue("@CreateUser", Convert.ToString(Session["userid"]));

                    cmd.CommandTimeout = 0;
                    SqlDataAdapter Adap = new SqlDataAdapter();
                    Adap.SelectCommand = cmd;
                    Adap.Fill(dsInst);
                    cmd.Dispose();
                    con.Dispose();
                    BranchTransferCallBackPanel.JSProperties["cpTransfer"] = "YES";
                }
                catch (Exception ex)
                {
                    BranchTransferCallBackPanel.JSProperties["cpTransfer"] = "NO";
                }
             }               
               
            
        }

        public void createBranchJson() 
        {
            List<BranchlistwithMainAct> branchList= new List<BranchlistwithMainAct>();
            BranchlistwithMainAct branch;
            DataTable BranchList = oDBEngine.GetDataTable("select branch_id,branch_internalId,branch_code,isnull(branch_MainAccount,'')branch_MainAccount  from tbl_master_branch");
            foreach (DataRow dr in BranchList.Rows)
            { 
               branch= new BranchlistwithMainAct();
                branch.branch_id=Convert.ToInt32(dr["branch_id"]);
                branch.branch_internalId=Convert.ToString(dr["branch_internalId"]);
                branch.branch_code=Convert.ToString(dr["branch_code"]);
                branch.branch_MainAccount=Convert.ToString(dr["branch_MainAccount"]);

                branchList.Add(branch);
            }

            var oSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            hdBranchDetails.Value = oSerializer.Serialize(branchList);
        }

        public class BranchlistwithMainAct
        {
            public int branch_id { get; set; }
            public string branch_internalId { get; set; }
            public string branch_code { get; set; }
            public string branch_MainAccount { get; set; }
        }

    }
}