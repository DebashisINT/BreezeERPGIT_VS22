using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using BusinessLogicLayer;

namespace ERP.OMS.Management
{
    public partial class management_frm_OpeningBalanceSubAc : System.Web.UI.Page
    {
        //public string strDataStatus;
        public string strSubAcID;
        public string strMainAcID;
        String strOpeningCr;
        String strOpeningDr;
        DataTable dataTable = new DataTable();

        //DBEngine oDBEngine = new DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);MULTI
        DBEngine oDBEngine = new DBEngine();
        protected void Page_Init(object sender, EventArgs e)
        {
            dsBranch.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
        }
        protected void Page_Load(object sender, EventArgs e)
        {

            strSubAcID = Request.QueryString["id"].ToString();
            strMainAcID = Request.QueryString["MainAcId"].ToString();



            if (!IsPostBack)
            {

                ////Company Fetch

                DataTable DTCompany = oDBEngine.GetDataTable("TBL_MASTER_COMPANY ", "Top 1 CMP_NAME", "CMP_INTERNALID='" + Session["LastCompany"].ToString().Trim() + "'");
                if (DTCompany.Rows.Count > 0)
                {
                    lblCompanyName.Text = DTCompany.Rows[0][0].ToString().Trim();
                }

                /////Segment Fetch
                if (Session["userlastsegment"].ToString() == "9")
                {
                    lblSegmentName.Text = "NSDL";
                }
                else if (Session["userlastsegment"].ToString() == "10")
                {
                    lblSegmentName.Text = "CDSL";
                }
                else
                {

                    if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "1")
                        lblSegmentName.Text = "NSE-CM";
                    if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "4")
                        lblSegmentName.Text = "BSE-CM";
                    if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "15")
                        lblSegmentName.Text = "CSE-CM";

                    if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "3")
                        lblSegmentName.Text = "NSE-CDX";
                    if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "6")
                        lblSegmentName.Text = "BSE-CDX";
                    if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "7")
                        lblSegmentName.Text = "MCX-COMM";
                    if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "8")
                        lblSegmentName.Text = "MCXSX-CDX";
                    if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "9")
                        lblSegmentName.Text = "NCDEX-COMM";
                    if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "10")
                        lblSegmentName.Text = "DGCX-COMM";
                    if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "11")
                        lblSegmentName.Text = "NMCE-COMM";
                    if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "12")
                        lblSegmentName.Text = "ICEX-COMM";
                    if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "13")
                        lblSegmentName.Text = "USE-CDX";
                    if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "14")
                        lblSegmentName.Text = "NSEL-SPOT";
                    if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "16")
                        lblSegmentName.Text = "Accounts";
                    if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "17")
                        lblSegmentName.Text = "ACE-COMM";
                    if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "18")
                        lblSegmentName.Text = "INMX-COMM";
                    if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "19")
                        lblSegmentName.Text = "MCXSX-CM";
                    if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "20")
                        lblSegmentName.Text = "MCXSX-FO";
                    if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "21")
                        lblSegmentName.Text = "BFX-COMM";
                    if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "22")
                        lblSegmentName.Text = "INSX-COMM";
                    if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "23")
                        lblSegmentName.Text = "INFX-COMM";

                }
                ///////Branch Fetch
                dsBranch.SelectCommand = "SELECT BRANCH_id AS BANKBRANCH_ID , BRANCH_DESCRIPTION AS BANKBRANCH_NAME  FROM TBL_MASTER_BRANCH where BRANCH_id in(" + Session["userbranchHierarchy"].ToString() + ")";
                cmbBranch.DataBind();
                cmbBranch.SelectedIndex = 0;


                CRopening.ClientInstanceName = "Ctxt";
                DRopening.ClientInstanceName = "Dtxt";

                DRopening.ClientSideEvents.KeyUp = "function(s,e){aaa(this,event," + DRopening.ClientID + ");}";
                DRopening.ClientSideEvents.TextChanged = "function(s,e){Ctxt" + ".SetText('000000000.00');}";

                CRopening.ClientSideEvents.KeyUp = "function(s,e){aaa(this,event," + CRopening.ClientID + ");}";
                CRopening.ClientSideEvents.TextChanged = "function(s,e){Dtxt" + ".SetText('000000000.00');}";

                DataTable dtSegid = new DataTable();
                // .............................Code Commented and Added by Sam on 05122016. To ignore null exception because Session["Segmentname"] is null..................................... 
                if (Session["Segmentname"] != null)
                {
                // .............................Code Above Commented and Added by Sam on 05122016...................................... 
                    if (HttpContext.Current.Session["Segmentname"].ToString().Trim() == "NSDL")
                    {
                        dtSegid = oDBEngine.GetDataTable("tbl_master_companyexchange", "Exch_internalid", "exch_tmcode='" + Session["usersegid"].ToString().Trim() + "' and exch_compid='" + Session["LastCompany"].ToString().Trim() + "' and exch_membershiptype='NSDL'");
                        ViewState["Seg"] = dtSegid.Rows[0][0].ToString().Trim();
                    }
                    else if (HttpContext.Current.Session["Segmentname"].ToString().Trim() == "CDSL")
                    {
                        dtSegid = oDBEngine.GetDataTable("tbl_master_companyexchange", "Exch_internalid", "exch_tmcode='" + Session["usersegid"].ToString().Trim() + "' and exch_compid='" + Session["LastCompany"].ToString().Trim() + "'  and exch_membershiptype='CDSL'");
                        ViewState["Seg"] = dtSegid.Rows[0][0].ToString().Trim();
                    }
                    else
                    {
                        ViewState["Seg"] = Session["usersegid"].ToString().Trim();
                    }
                // .............................Code Commented and Added by Sam on 05122016. To ignore null exception because Session["Segmentname"] is null 
                    //and put the debbuging in the following line ViewState["Seg"] = Session["usersegid"].ToString().Trim();..................................... 
                }
                else
                {
                    ViewState["Seg"] = Session["usersegid"].ToString().Trim();
                } 
                // .............................Code Above Commented and Added by Sam on 05122016...................................... 

                CheckDataExitOrNot(strSubAcID, strMainAcID);
            }


        }

        private void CheckDataExitOrNot(string SubAcID, string MainAcID)
        {

            dataTable = oDBEngine.GetDataTable("trans_accountsLedger", "accountsLedger_MAINACCOUNTID AS MAINACID,accountsLedger_SUBACCOUNTID AS SUBACID,accountsLedger_COMPANYID SUBCOMPID, accountsLedger_EXCHANGESEGMENTID AS SUBEXCHID,accountsLedger_BRANCHID AS SUBBRANCHID,CAST(accountsLedger_AmountDR AS DECIMAL(18,2)) AS SUBDR,CAST(accountsLedger_AmountCR  AS DECIMAL(18,2))AS SUBCR", "accountsLedger_MAINACCOUNTID in(select MainAccount_AccountCode from master_mainaccount where MainAccount_ReferenceID='" + MainAcID + "') AND accountsLedger_SUBACCOUNTID='" + SubAcID + "' and accountsLedger_ExchangeSegmentID='" + ViewState["Seg"].ToString().Trim() + "' and accountsLedger_TransactionType='OpeningBalance' and  accountsledger_Finyear='" + Session["LastFinYear"].ToString().Trim() + "'");


            if (dataTable.Rows.Count != 0)
            {
                hdnStatus.Value = "1";

                if (DBNull.Value != dataTable.Rows[0]["SUBBRANCHID"])
                {

                    cmbBranch.Value = dataTable.Rows[0]["SUBBRANCHID"].ToString().Trim();

                }
                else
                {
                    cmbBranch.SelectedIndex = 0;
                }
                if (DBNull.Value != dataTable.Rows[0]["SUBDR"])
                {
                    DRopening.Text = dataTable.Rows[0]["SUBDR"].ToString();
                }
                else
                {
                    DRopening.Text = "000000.00";
                }
                if (DBNull.Value != dataTable.Rows[0]["SUBCR"])
                {
                    CRopening.Text = dataTable.Rows[0]["SUBCR"].ToString();
                }
                else
                {
                    CRopening.Text = "00000.00";
                }





            }
            else
            {
                hdnStatus.Value = "0";

            }


        }


        protected void Button1_Click(object sender, EventArgs e)
        {
            string strSubAcId = Request.QueryString["id"].ToString();
            int strSubMainId = Convert.ToInt32(Request.QueryString["MainAcId"].ToString());
            if (hdnStatus.Value.ToString() == "1")
            {
                //Update will be do here.

                updateTransSubAccount(strSubAcId, strSubMainId);
            }
            else
            {
                //Insert will do here.

                InsertInTransMainAccount(strSubAcId, strSubMainId);

            }

        }
        private void InsertInTransMainAccount(string strSubAcId, int strSubMainId)
        {

            //SqlConnection lcon = new SqlConnection(ConfigurationSettings.AppSettings["DBConnectionDefault"]);MULTI
            SqlConnection lcon = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            lcon.Open();
            SqlCommand lcmd = new SqlCommand("InsertTransSubAccount", lcon);
            lcmd.CommandType = CommandType.StoredProcedure;
            lcmd.Parameters.Add("@FinancialYear", SqlDbType.VarChar).Value = Session["LastFinYear"].ToString().Trim();
            lcmd.Parameters.Add("@MainAccount_ReferenceID", SqlDbType.BigInt).Value = Convert.ToInt64(strSubMainId.ToString());
            lcmd.Parameters.Add("@SubAccount_ReferenceID", SqlDbType.VarChar).Value = strSubAcId.ToString();
            lcmd.Parameters.Add("@CompanyName", SqlDbType.Char).Value = Session["LastCompany"].ToString().Trim();
            lcmd.Parameters.Add("@SegmentName", SqlDbType.Char).Value = ViewState["Seg"].ToString().Trim();
            lcmd.Parameters.Add("@BranchName", SqlDbType.Char).Value = cmbBranch.Value;
            strOpeningCr = CRopening.Text.Remove(0, 3);
            strOpeningDr = DRopening.Text.Remove(0, 3);
            lcmd.Parameters.Add("@openingCR", SqlDbType.Decimal).Value = Convert.ToDecimal(strOpeningCr.ToString());
            lcmd.Parameters.Add("@openingDR", SqlDbType.Decimal).Value = Convert.ToDecimal(strOpeningDr.ToString());
            ///Currency Setting
            lcmd.Parameters.Add("@ActiveCurrency", SqlDbType.Char).Value = Session["ActiveCurrency"].ToString().Split('~')[0];

            int NoOfRowEffected = lcmd.ExecuteNonQuery();
            if (NoOfRowEffected != 0)
            {
                //Close Popup.
                ScriptManager.RegisterStartupScript(this, GetType(), "JScript", "alert('Record Updated Successfully');window.parent.popup.Hide();", true);
            }

            lcmd.Dispose();
            lcon.Close();
            lcon.Dispose();

        }

        private void updateTransSubAccount(string strSubAcId, int strSubMainId)
        {

            //SqlConnection lcon = new SqlConnection(ConfigurationSettings.AppSettings["DBConnectionDefault"]);MULTI
            SqlConnection lcon = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            lcon.Open();
            SqlCommand lcmd = new SqlCommand("UpDateAccountSummary1", lcon);
            lcmd.CommandType = CommandType.StoredProcedure;
            lcmd.Parameters.Add("@Module", SqlDbType.VarChar).Value = "UpDateSubAccountSummary";
            lcmd.Parameters.Add("@FinancialYear", SqlDbType.VarChar).Value = Session["LastFinYear"].ToString().Trim();
            lcmd.Parameters.Add("@AccountRefID", SqlDbType.Int).Value = Convert.ToInt32(strSubMainId.ToString());
            lcmd.Parameters.Add("@SubAccountRefID", SqlDbType.VarChar).Value = strSubAcId.ToString();
            lcmd.Parameters.Add("@CompanyName", SqlDbType.Char).Value = Session["LastCompany"].ToString().Trim();
            lcmd.Parameters.Add("@SegmentName", SqlDbType.Char).Value = ViewState["Seg"].ToString().Trim();
            lcmd.Parameters.Add("@BranchName", SqlDbType.Char).Value = cmbBranch.Value;
            strOpeningCr = CRopening.Text.Remove(0, 3);
            strOpeningDr = DRopening.Text.Remove(0, 3);
            lcmd.Parameters.Add("@openingCR", SqlDbType.Decimal).Value = Convert.ToDecimal(strOpeningCr.ToString());
            lcmd.Parameters.Add("@openingDR", SqlDbType.Decimal).Value = Convert.ToDecimal(strOpeningDr.ToString());
            ///Currency Setting
            lcmd.Parameters.AddWithValue("@ActiveCurrency", Session["ActiveCurrency"].ToString().Split('~')[0]);


            int NoOfRowEffected = lcmd.ExecuteNonQuery();
            if (NoOfRowEffected != 0)
            {
                //Close Popup.
                ScriptManager.RegisterStartupScript(this, GetType(), "JScript", "alert('Record Updated Successfully');window.parent.popup.Hide();", true);
            }
            lcmd.Dispose();
            lcon.Close();
            lcon.Dispose();


        }
        protected void cmbBranch_SelectedIndexChanged(object sender, EventArgs e)
        {
            string branchId = cmbBranch.SelectedItem.Value.ToString();
            dataTable = oDBEngine.GetDataTable("trans_accountsLedger", "cast(accountsLedger_AmountDR as decimal(18,2)) as Dr,cast (accountsLedger_AmountCR as decimal(18,2)) as Cr", " accountsLedger_MAINACCOUNTID in(select MainAccount_AccountCode from master_mainaccount where MainAccount_ReferenceID='" + Request.QueryString["MainAcId"].ToString() + "') AND accountsLedger_SUBACCOUNTID='" + Request.QueryString["id"].ToString() + "' and accountsLedger_ExchangeSegmentID='" + ViewState["Seg"].ToString().Trim() + "' and accountsLedger_BranchID='" + branchId + "' and accountsLedger_TransactionType='OpeningBalance'");
            if (dataTable.Rows.Count > 0)
            {
                if (DBNull.Value != dataTable.Rows[0]["Dr"])
                {
                    DRopening.Text = dataTable.Rows[0]["Dr"].ToString();
                }
                else
                {
                    DRopening.Text = "000000.00";
                }
                if (DBNull.Value != dataTable.Rows[0]["Cr"])
                {
                    CRopening.Text = dataTable.Rows[0]["Cr"].ToString();
                }
                else
                {
                    CRopening.Text = "00000.00";
                }
            }
            else
            {
                DRopening.Text = "0.00";
                CRopening.Text = "0.00";
            }
        }
    }

}