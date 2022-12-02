using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI.WebControls;
//////using DevExpress.Web;
using DevExpress.Web;
using BusinessLogicLayer;

namespace ERP.OMS.Management
{
    public partial class management_frm_OpeningBalanceSubAcWC : System.Web.UI.Page
    {
        //public string strDataStatus;
        Management_BL oManagement_BL = new Management_BL();
        public string strSubAcID;
        public string strMainAcID;
        String strOpeningCr;
        String strOpeningDr;
        DataTable dataTable = new DataTable();
        string strSubAccountId;
        string strSubAccountCode;
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
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
                BindDropDown();
               
                // .............................Code Commented and Added by Sam on 05122016 to avoid this section.. .....................................                 

                //string[,] compId = oDBEngine.GetFieldValue("tbl_Trans_Lastsegment", "ls_lastCompany", "ls_UserID='" + Session["UserID"].ToString() + "' and ls_lastSegment='" + HttpContext.Current.Session["userlastsegment"].ToString() + "'", 1);
                //if (compId[0, 0] != "n")
                //{
                //    cmbCompany.Text = Session["LastCompany"].ToString();//compId[0, 0];
                //    SelectSegment.SelectCommand = "select A.EXCH_INTERNALID AS SEGMENTID ,isnull((TME.EXH_ShortName + '-' + A.EXCH_SEGMENTID),exch_membershipType) AS EXCHANGENAME from (SELECT TMCE.* FROM TBL_MASTER_COMPANYEXCHANGE AS TMCE WHERE  TMCE.EXCH_COMPID='" + compId[0, 0].ToString() + "' ) AS A LEFT OUTER JOIN   TBL_MASTER_EXCHANGE AS TME ON TME.EXH_CNTID=A.EXCH_EXCHID";
                //    cmbSegment.DataBind();
                //    string[,] seg1 = oDBEngine.GetFieldValue("tbl_master_segment", "seg_name", " seg_id='" + HttpContext.Current.Session["userlastsegment"].ToString() + "'", 1);
                //    if (seg1[0, 0] != "n")
                //    {
                //        cmbSegment.Text = seg1[0, 0];
                //    }
                //}

                // .............................Code Above Commented and Added by Sam on 05122016 to avoid this section....................................... 

                //CRopening.ClientInstanceName = "Ctxt";
                //DRopening.ClientInstanceName = "Dtxt";

                //DRopening.ClientSideEvents.KeyUp = "function(s,e){aaa(this,event," + DRopening.ClientID + ");}";
                //DRopening.ClientSideEvents.TextChanged = "function(s,e){Ctxt" + ".SetText('000000000.00');}";

                //CRopening.ClientSideEvents.KeyUp = "function(s,e){aaa(this,event," + CRopening.ClientID + ");}";
                //CRopening.ClientSideEvents.TextChanged = "function(s,e){Dtxt" + ".SetText('000000000.00');}";
                //cmbCompany.Enabled = false;
                //cmbBranch.Enabled = false;
                // .............................Code Commented and Added by Sam on 05122016 to avoid this section.. .....................................                 
                //cmbSegment.Enabled = false;
                // .............................Code Above Commented and Added by Sam on 05122016 to avoid this section.....
                CheckDataExitOrNot(strSubAcID, strMainAcID);
            }
            else
            {
                //cmbBranch.DataBind();
                //cmbCompany.DataBind();
                //cmbSegment.DataBind();
                //CheckDataExitOrNot(strSubAcID, strMainAcID);

            }

           

        }

        public void BindDropDown()
        {
            DataTable DTCompany = oDBEngine.GetDataTable("TBL_MASTER_COMPANY ", "Top 1 CMP_NAME", "CMP_INTERNALID='" + Convert.ToString(Session["LastCompany"]).Trim() + "'");
            if (DTCompany.Rows.Count > 0)
            {
                //lblCompanyName.Text = DTCompany.Rows[0][0].ToString().Trim();
                lblCompanyName.Text = Convert.ToString(DTCompany.Rows[0][0]).Trim();
            }

            if (Session["userbranchHierarchy"] != null) // Session Null Handling
            {
                //dsBranch.SelectCommand = "SELECT BRANCH_id AS BANKBRANCH_ID , BRANCH_DESCRIPTION AS BANKBRANCH_NAME  FROM TBL_MASTER_BRANCH where BRANCH_id in(" + Session["userbranchHierarchy"].ToString() + ")";
                dsBranch.SelectCommand = "SELECT BRANCH_id AS BANKBRANCH_ID , BRANCH_DESCRIPTION AS BANKBRANCH_NAME  FROM TBL_MASTER_BRANCH where BRANCH_id in(" + Convert.ToString(Session["userbranchHierarchy"]) + ")";
                cmbBranch.DataBind();
            }



            //dsCompany.SelectCommand = "SELECT COMP.CMP_INTERNALID AS COMPANYID , COMP.CMP_NAME AS COMPANYNAME  FROM TBL_MASTER_COMPANY AS COMP";
            //cmbCompany.DataBind();
            //dsBranch.SelectCommand = "SELECT BRANCH_id AS BANKBRANCH_ID , BRANCH_DESCRIPTION AS BANKBRANCH_NAME  FROM TBL_MASTER_BRANCH where BRANCH_id in(select cnt_branchid from tbl_master_contact where cnt_internalid='" + strSubAcID + "')";
            //cmbBranch.DataBind();
            //DataTable DTCompany = oDBEngine.GetDataTable("TBL_MASTER_COMPANY ", "Top 1 CMP_NAME", "CMP_INTERNALID='" + Convert.ToString(Session["LastCompany"]).Trim() + "'");
            //if (DTCompany.Rows.Count > 0)
            //{
            //if (cmbCompany.Items.Count > 0)
            //{
            //    int cmbindex = 0;
            //    foreach(ListEditItem li in cmbCompany.Items)
            //    {
            //        if(Convert.ToString(li.Value)==Convert.ToString(Session["LastCompany"]))
            //        {
            //            break;
            //        }
            //        else
            //        {
            //            cmbindex = cmbindex + 1;
            //        }
            //    }

            //    cmbCompany.SelectedIndex = cmbindex;
            //}
                //lblCompanyName.Text = DTCompany.Rows[0][0].ToString().Trim();
                //lblCompanyName.Text = Convert.ToString(DTCompany.Rows[0][0]).Trim();
            //}
            //lblCompanyName.Text = Convert.ToString(Session["LastCompany"]);

            //string[,] seg = oDBEngine.GetFieldValue("TBL_MASTER_BRANCH", "BRANCH_id", " BRANCH_id in(select cnt_branchid from tbl_master_contact where cnt_internalid='" + strSubAcID + "')", 1);
            //if (seg[0, 0] != "n")
            //{
            //    cmbBranch.Text = seg[0, 0];
            //}
        }

        private void CheckDataExitOrNot(string SubAcID, string MainAcID)
        {
            // .............................Code Commented and Added by Sam on 05122016.to avoid Segment section ..................................... 

            //string SegmentID = null;
            //try
            //{
            //    SegmentID = cmbSegment.SelectedItem.Value.ToString();
            //}
            //catch
            //{
            //    SegmentID = "0";
            //}
            // .............................Code Above Commented and Added by Sam on 05122016...................................... 
            dataTable = oDBEngine.GetDataTable("tbl_master_contact", "cnt_branchid", " cnt_internalId='" + SubAcID + "'");
            if (dataTable.Rows.Count > 0)
            {
                cmbBranch.Value = Convert.ToString(dataTable.Rows[0]["cnt_branchid"]);
                cmbBranch.Enabled = false;
                Session["br"] = "1";
            }
            else
            {
                Session["br"] = null;
            }
            

           
            dataTable = oDBEngine.GetDataTable("MASTER_SUBACCOUNT", "SUBACCOUNT_REFERENCEID,SUBACCOUNT_MAINACREFERENCEID,SUBACCOUNT_CODE,SUBACCOUNT_NAME", "SUBACCOUNT_CODE='" + SubAcID + "'");

            if (dataTable.Rows.Count != 0)
            {
                hdnSubAcCode.Value = dataTable.Rows[0]["SUBACCOUNT_CODE"].ToString();
                // .............................Code Commented and Added by Sam on 05122016.to avoid Segment section ..................................... 

                //dataTable = oDBEngine.GetDataTable("trans_accountsLedger", "accountsLedger_MAINACCOUNTID AS MAINACID,accountsLedger_SUBACCOUNTID AS SUBACID,accountsLedger_COMPANYID SUBCOMPID, accountsLedger_EXCHANGESEGMENTID AS SUBEXCHID,accountsLedger_BRANCHID AS SUBBRANCHID,CAST(accountsLedger_AmountDR AS DECIMAL(18,2)) AS SUBDR,CAST(accountsLedger_AmountCR  AS DECIMAL(18,2))AS SUBCR", "accountsLedger_MAINACCOUNTID in(select MainAccount_AccountCode from master_mainaccount where MainAccount_ReferenceID='" + strMainAcID + "') AND accountsLedger_SUBACCOUNTID='" + hdnSubAcCode.Value + "' and accountsLedger_TransactionType='OpeningBalance' and accountsLedger_ExchangeSegmentID=" + SegmentID + "  and accountsledger_finyear='" + Session["LastFinYear"].ToString() + "' and accountsLedger_COMPANYID='" + Session["LastCompany"].ToString() + "' ");

                dataTable = oDBEngine.GetDataTable("trans_accountsLedger", "accountsLedger_MAINACCOUNTID AS MAINACID,accountsLedger_SUBACCOUNTID AS SUBACID,accountsLedger_COMPANYID SUBCOMPID, accountsLedger_EXCHANGESEGMENTID AS SUBEXCHID,accountsLedger_BRANCHID AS SUBBRANCHID,CAST(accountsLedger_AmountDR AS DECIMAL(18,2)) AS SUBDR,CAST(accountsLedger_AmountCR  AS DECIMAL(18,2))AS SUBCR", "accountsLedger_MAINACCOUNTID in(select MainAccount_AccountCode from master_mainaccount where MainAccount_ReferenceID='" + strMainAcID + "') AND accountsLedger_SUBACCOUNTID='" + hdnSubAcCode.Value + "' and accountsLedger_TransactionType='OpeningBalance'  and accountsledger_finyear='" + Session["LastFinYear"].ToString() + "' and accountsLedger_COMPANYID='" + Session["LastCompany"].ToString() + "' ");
                // .............................Code Above Commented and Added by Sam on 05122016...................................... 
                if (dataTable.Rows.Count != 0)
                {
                    hdnStatus.Value = "2";
                    //if (DBNull.Value != dataTable.Rows[0]["SUBCOMPID"])
                    //{

                    //    cmbCompany.Value = dataTable.Rows[0]["SUBCOMPID"].ToString().Trim();
                    //    ASPxComboBox a = new ASPxComboBox();

                    //}
                    //else
                    //{
                    //    cmbCompany.SelectedIndex = 0;
                    //}
                    // .............................Code Commented and Added by Sam on 05122016.to avoid this section ..................................... 
                    //if (DBNull.Value != dataTable.Rows[0]["SUBEXCHID"])
                    //{

                    //    cmbSegment.Value = dataTable.Rows[0]["SUBEXCHID"].ToString().Trim();
                    //    if (cmbSegment.Value.ToString() == "0")
                    //        cmbSegment.Text = "Accounts";
                    //}
                    //else
                    //{
                    //    cmbSegment.SelectedIndex = 0;
                    //}
                    // .............................Code Above Commented and Added by Sam on 05122016...................................... 
                    if (DBNull.Value != dataTable.Rows[0]["SUBBRANCHID"])
                    {
                        //string branchname = dataTable.Rows[0]["SUBBRANCHID"].ToString();
                        cmbBranch.Value = dataTable.Rows[0]["SUBBRANCHID"].ToString().Trim();
                        if(Session["br"] == null)
                        {
                            cmbBranch.Enabled = true;
                        }
                        

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
                    hdnStatus.Value = "1";
                    // cmbCompany.SelectedIndex = 0;
                    

                    //

                }

            }
            else
            {
                hdnStatus.Value = "0";
                if (Session["br"] != null)
                { 
                    Session["br"] = null;
                }
                else
                {
                    cmbBranch.Enabled = true;
                }

            }


        }

        private void InsertInTransSubAccount(string strSubAccountCode, string strMainAcID)
        {
            // .............................Code Commented and Added by Sam on 05122016.to avoid this section ..................................... 
            //string segment = null;
            //if (cmbSegment.Value.ToString().Trim() == "Accounts")
            //{
            //    segment = "0";
            //}
            //else
            //    segment = cmbSegment.Value.ToString();
            // .............................Code Above Commented and Added by Sam on 05122016...................................... 


            string FinYear = null;
            FinYear = Session["LastFinYear"].ToString();


            //int month = oDBEngine.GetDate().Month;
            //if (month > 3)
            //{
            //    FinYear = oDBEngine.GetDate().Year + "-" + Convert.ToString(Convert.ToInt32(oDBEngine.GetDate().Year) + 1);
            //}
            //else
            //{
            //    FinYear = Convert.ToString(Convert.ToInt32(oDBEngine.GetDate().Year) - 1) + "-" + oDBEngine.GetDate().Year;
            //}
            //SqlConnection lcon = new SqlConnection(ConfigurationSettings.AppSettings["DBConnectionDefault"]);
            //lcon.Open();
            //SqlCommand lcmd = new SqlCommand("InsertTransSubAccount", lcon);
            //lcmd.CommandType = CommandType.StoredProcedure;
            //lcmd.Parameters.Add("@FinancialYear", SqlDbType.VarChar).Value = FinYear;
            //lcmd.Parameters.Add("@MainAccount_ReferenceID", SqlDbType.BigInt).Value = Convert.ToInt64(strMainAcID.ToString());
            //lcmd.Parameters.Add("@SubAccount_ReferenceID", SqlDbType.VarChar).Value = strSubAccountCode.ToString();
            //lcmd.Parameters.Add("@CompanyName", SqlDbType.Char).Value = cmbCompany.Value;
            //lcmd.Parameters.Add("@SegmentName", SqlDbType.Char).Value = segment;
            //lcmd.Parameters.Add("@BranchName", SqlDbType.Char).Value = cmbBranch.Value;
            //strOpeningCr = CRopening.Text.Remove(0, 3);
            //strOpeningDr = DRopening.Text.Remove(0, 3);
            //lcmd.Parameters.Add("@openingCR", SqlDbType.Decimal).Value = Convert.ToDecimal(strOpeningCr.ToString());
            //lcmd.Parameters.Add("@openingDR", SqlDbType.Decimal).Value = Convert.ToDecimal(strOpeningDr.ToString());

            /////Currency Setting
            //lcmd.Parameters.Add("@ActiveCurrency", SqlDbType.Char).Value = Session["ActiveCurrency"].ToString().Split('~')[0];

            //int NoOfRowEffected = lcmd.ExecuteNonQuery();

            strOpeningCr = CRopening.Text.Remove(0, 3);
            strOpeningDr = DRopening.Text.Remove(0, 3);

            // .............................Code Commented and Added by Sam on 05122016.to avoid SEGMANT section ..................................... 
            //int NoOfRowEffected = oManagement_BL.InsertTransSubAccount(FinYear, Convert.ToString(strMainAcID.ToString()), Convert.ToString(strSubAccountCode),
            //    Convert.ToString(cmbCompany.Value), Convert.ToString(segment), Convert.ToString(cmbBranch.Value), Convert.ToDecimal(strOpeningCr.ToString()),
            //    Convert.ToDecimal(strOpeningDr.ToString()), Convert.ToString(Session["ActiveCurrency"].ToString().Split('~')[0]));


            int NoOfRowEffected = oManagement_BL.InsertTransSubAccount(FinYear, Convert.ToString(strMainAcID.ToString()), Convert.ToString(strSubAccountCode),
                Convert.ToString(Session["LastCompany"]).Trim(), "0", Convert.ToString(cmbBranch.Value), Convert.ToDecimal(strOpeningCr.ToString()),
                Convert.ToDecimal(strOpeningDr.ToString()), Convert.ToString(Session["ActiveCurrency"].ToString().Split('~')[0]));

            // .............................Code Above Commented and Added by Sam on 05122016...................................... 
            
            if (NoOfRowEffected != 0)
            {
                //Close Popup.
                string popupScript = "";
                popupScript = "<script language='javascript'>" + "alert('Saved Successfully');window.parent.popup.Hide();</script>";
                ClientScript.RegisterStartupScript(GetType(), "PopupScript", popupScript);
            }
            //lcmd.Dispose();
            //lcon.Close();
            //lcon.Dispose();

        }
        private string CreateSubAccount(string name, string code)
        {
            //SqlConnection lcon = new SqlConnection(ConfigurationSettings.AppSettings["DBConnectionDefault"]);
            //lcon.Open();
            //SqlCommand lcmd = new SqlCommand("CreateSubAccount", lcon);
            //lcmd.CommandType = CommandType.StoredProcedure;
            //lcmd.Parameters.Add("@SubAccount_MainAcReferenceID", SqlDbType.Int).Value = Convert.ToInt32(Request.QueryString["MainAcId"].ToString().Trim());
            //lcmd.Parameters.Add("@SubAccount_Name", SqlDbType.VarChar,100).Value = name.ToString().Trim();
            //lcmd.Parameters.Add("@SubAccount_Code", SqlDbType.VarChar,20).Value = code.ToString().Trim();
            //SqlParameter parameter = new SqlParameter("@id", SqlDbType.VarChar, 20);
            //parameter.Direction = ParameterDirection.Output;
            //lcmd.Parameters.Add(parameter);
            //int NoOfRowEffected = lcmd.ExecuteNonQuery();
            //string SubAccountID = parameter.Value.ToString();
            //lcmd.Dispose();
            //lcon.Close();
            //lcon.Dispose();
            string parameter = string.Empty;
            int NoOfRowEffected = oManagement_BL.CreateSubAccount(Convert.ToInt32(Request.QueryString["MainAcId"].ToString().Trim()), Convert.ToString(name.ToString().Trim()),
                Convert.ToString(code.ToString().Trim()), out parameter);
            string SubAccountID = parameter.ToString();
            return SubAccountID;


        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            string strSubAcId = Convert.ToString(Request.QueryString["id"].ToString());
            string strSubMainId = Convert.ToString(Request.QueryString["MainAcId"].ToString());

            if (hdnStatus.Value.ToString() == "0")
            {
                DataTable dt = new DataTable();
                dt = oDBEngine.GetDataTable("TBL_MASTER_CONTACT", "ISNULL(CNT_FIRSTNAME,'') + ISNULL(CNT_MIDDLENAME, '') + ISNULL(CNT_LASTNAME,'') AS CONTACTNAME", "CNT_INTERNALID='" + strSubAcID + "'");
                if (dt.Rows.Count != 0)
                {
                    strSubAccountCode = CreateSubAccount(dt.Rows[0]["CONTACTNAME"].ToString(), strSubAcId);
                    InsertInTransSubAccount(strSubAcId, strMainAcID);
                }


            }
            else if (hdnStatus.Value.ToString() == "1")
            {
                //DataTable dt = new DataTable();
                //dt = oDBEngine.GetDataTable("TBL_MASTER_CONTACT", "ISNULL(CNT_FIRSTNAME,'') + ISNULL(CNT_MIDDLENAME, '') + ISNULL(CNT_LASTNAME,'') AS CONTACTNAME", "CNT_INTERNALID='" + strSubAcID + "'");
                //strSubAccountCode = CreateSubAccount(dt.Rows[0]["CONTACTNAME"].ToString(), strSubAcID);
                InsertInTransSubAccount(hdnSubAcCode.Value, strMainAcID);

            }
            else
            {

                updateTransSubAccount(hdnSubAcCode.Value, strMainAcID);

            }

        }
        //private void InsertInTransMainAccount(int strSubAcId, int strSubMainId)
        //{


        //    SqlConnection lcon = new SqlConnection(ConfigurationSettings.AppSettings["DBConnectionDefault"]);
        //    lcon.Open();
        //    SqlCommand lcmd = new SqlCommand("InsertTransSubAccount", lcon);
        //    lcmd.CommandType = CommandType.StoredProcedure;
        //    lcmd.Parameters.Add("@FinancialYear", SqlDbType.VarChar).Value = "2009-2010";
        //    lcmd.Parameters.Add("@MainAccount_ReferenceID", SqlDbType.BigInt).Value = Convert.ToInt64(strSubMainId.ToString());
        //    lcmd.Parameters.Add("@SubAccount_ReferenceID", SqlDbType.BigInt).Value = Convert.ToInt64(strSubAcId.ToString());
        //    lcmd.Parameters.Add("@CompanyName", SqlDbType.Char).Value = cmbCompany.Value;
        //    lcmd.Parameters.Add("@SegmentName", SqlDbType.Char).Value = cmbSegment.Value;
        //    lcmd.Parameters.Add("@BranchName", SqlDbType.Char).Value = cmbBranch.Value;
        //    strOpeningCr = CRopening.Text.Remove(0, 3);
        //    strOpeningDr = DRopening.Text.Remove(0, 3);
        //    lcmd.Parameters.Add("@openingCR", SqlDbType.Decimal).Value = Convert.ToDecimal(strOpeningCr.ToString());
        //    lcmd.Parameters.Add("@openingDR", SqlDbType.Decimal).Value = Convert.ToDecimal(strOpeningDr.ToString());

        //    int NoOfRowEffected = lcmd.ExecuteNonQuery();
        //    lcmd.Dispose();
        //    lcon.Close();
        //    lcon.Dispose();

        //}

        private void updateTransSubAccount(string strSubAcId, string strSubMainId)
        {
            // .............................Code Commented and Added by Sam on 05122016.to avoid SEGMANT section ..................................... 
            //string segment = null;
            //if (cmbSegment.Value.ToString().Trim() == "Accounts")
            //{
            //    segment = "0";
            //}
            //else
            //    segment = cmbSegment.Value.ToString();

            // .............................Code Above Commented and Added by Sam on 05122016...................................... 
            string FinYear = null;
            FinYear = Session["LastFinYear"].ToString();
            //int month = oDBEngine.GetDate().Month;
            //if (month > 3)
            //{
            //    FinYear = oDBEngine.GetDate().Year + "-" + Convert.ToString(Convert.ToInt32(oDBEngine.GetDate().Year) + 1);
            //}
            //else
            //{
            //    FinYear = Convert.ToString(Convert.ToInt32(oDBEngine.GetDate().Year) - 1) + "-" + oDBEngine.GetDate().Year;
            //}
            //SqlConnection lcon = new SqlConnection(ConfigurationSettings.AppSettings["DBConnectionDefault"]);
            //lcon.Open();
            //SqlCommand lcmd = new SqlCommand("UpDateAccountSummaryCust", lcon);
            //lcmd.CommandType = CommandType.StoredProcedure;
            //lcmd.Parameters.Add("@Module", SqlDbType.VarChar).Value = "UpDateSubAccountSummary";
            //lcmd.Parameters.Add("@FinancialYear", SqlDbType.VarChar).Value = FinYear;
            //lcmd.Parameters.Add("@AccountRefID", SqlDbType.Int).Value = Convert.ToInt32(strSubMainId.ToString());
            //lcmd.Parameters.Add("@SubAccountRefID", SqlDbType.VarChar).Value = strSubAcId.ToString();
            //lcmd.Parameters.Add("@CompanyName", SqlDbType.Char).Value = cmbCompany.Value;
            //lcmd.Parameters.Add("@SegmentName", SqlDbType.Char).Value = segment;
            //lcmd.Parameters.Add("@BranchName", SqlDbType.Char).Value = cmbBranch.Value;
            //strOpeningCr = CRopening.Text.Remove(0, 3);
            //strOpeningDr = DRopening.Text.Remove(0, 3);
            //lcmd.Parameters.Add("@openingCR", SqlDbType.Decimal).Value = Convert.ToDecimal(strOpeningCr.ToString());
            //lcmd.Parameters.Add("@openingDR", SqlDbType.Decimal).Value = Convert.ToDecimal(strOpeningDr.ToString());

            /////Currency Setting
            //lcmd.Parameters.AddWithValue("@ActiveCurrency", Session["ActiveCurrency"].ToString().Split('~')[0]);
            //lcmd.Parameters.AddWithValue("@TradeCurrency", Session["TradeCurrency"].ToString().Split('~')[0]);

            //int NoOfRowEffected = lcmd.ExecuteNonQuery();
            strOpeningCr = CRopening.Text.Remove(0, 3);
            strOpeningDr = DRopening.Text.Remove(0, 3);

            // .............................Code Commented and Added by Sam on 05122016.to avoid SEGMANT section ..................................... 

            //int NoOfRowEffected = oManagement_BL.UpDateAccountSummaryCust("UpDateSubAccountSummary", FinYear, Convert.ToString(strSubMainId.ToString()),
            //    Convert.ToString(strSubAcId.ToString()), Convert.ToString(cmbCompany.Value), Convert.ToString(segment), Convert.ToString(cmbBranch.Value),
            //    Convert.ToDecimal(strOpeningCr.ToString()), Convert.ToDecimal(strOpeningDr.ToString()), Convert.ToString(Session["ActiveCurrency"].ToString().Split('~')[0]),
            //    Convert.ToString(Session["TradeCurrency"].ToString().Split('~')[0]));

            int NoOfRowEffected = oManagement_BL.UpDateAccountSummaryCust("UpDateSubAccountSummary", FinYear, Convert.ToString(strSubMainId.ToString()),
               Convert.ToString(strSubAcId.ToString()), Convert.ToString(Session["LastCompany"]).Trim(), "0", Convert.ToString(cmbBranch.Value),
               Convert.ToDecimal(strOpeningCr.ToString()), Convert.ToDecimal(strOpeningDr.ToString()), Convert.ToString(Session["ActiveCurrency"].ToString().Split('~')[0]),
               Convert.ToString(Session["TradeCurrency"].ToString().Split('~')[0]));

            // .............................Code Above Commented and Added by Sam on 05122016...................................... 

            if (NoOfRowEffected != 0)
            {
                //Close Popup.
                string popupScript = "";
                popupScript = "<script language='javascript'>" + "alert('Saved successfully');window.parent.popup.Hide();</script>";
                ClientScript.RegisterStartupScript(GetType(), "PopupScript", popupScript);
            }
            //lcmd.Dispose();
            //lcon.Close();
            //lcon.Dispose();


        }
    }
}
