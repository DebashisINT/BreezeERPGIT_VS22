using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls; 
//using System.IO; 
//using DevExpress.Web;
//////using DevExpress.Web.ASPxClasses;
//////using DevExpress.Web;
//using DevExpress.Web;
using BusinessLogicLayer;
using System.Web.Services;
using System.Collections.Generic;

namespace ERP.OMS.Management.Master
{

    public partial class management_master_frm_OpeningBalance : ERP.OMS.ViewState_class.VSPage //System.Web.UI.Page
    {
        //public string strDataStatus;
        public string strID;
        String strOpeningCr;
        String strOpeningDr;
        DataTable dataTable = new DataTable();
        BusinessLogicLayer.MainAccountHead_BL fobbl = new BusinessLogicLayer.MainAccountHead_BL();

        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);multi
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        protected void Page_Load(object sender, EventArgs e)
        {




            if (!IsPostBack)
            {
                ////Company Fetch
                dsBranch.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                DataTable DTCompany = oDBEngine.GetDataTable("TBL_MASTER_COMPANY ", "Top 1 CMP_NAME", "CMP_INTERNALID='" + Convert.ToString(Session["LastCompany"]).Trim() + "'");
                if (DTCompany.Rows.Count > 0)
                {
                    //lblCompanyName.Text = DTCompany.Rows[0][0].ToString().Trim();
                    lblCompanyName.Text = Convert.ToString(DTCompany.Rows[0][0]).Trim();
                }

                Session["id"] = Request.QueryString["id"];
                /////Segment Fetch


                //Account Name
                DataTable dtAccountName = oDBEngine.GetDataTable("Master_MainAccount ", "MainAccount_Name", "MainAccount_ReferenceID=" + Convert.ToString(Request.QueryString["id"]).Trim() );
                if (dtAccountName.Rows.Count > 0)
                {
                    //lblCompanyName.Text = DTCompany.Rows[0][0].ToString().Trim();
                    lblAccountName.Text = " ("+ Convert.ToString(dtAccountName.Rows[0][0]).Trim() +")";
                }


                // .............................Code Commented and Added by Sam on 07122016. to avoid Unnecessary Code..................................... 
                if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "16")
                    lblSegmentName.Text = "Accounts";

                #region Thrash Code
                

                //if (Session["userlastsegment"].ToString() == "9")
                //{
                //    lblSegmentName.Text = "NSDL";
                //}
                //else if (Session["userlastsegment"].ToString() == "10")
                //{
                //    lblSegmentName.Text = "CDSL";
                //}
                //else
                //{

                    //if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "1")
                    //    lblSegmentName.Text = "NSE-CM";
                    //if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "4")
                    //    lblSegmentName.Text = "BSE-CM";
                    //if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "15")
                    //    lblSegmentName.Text = "CSE-CM";
                    //if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "2")
                    //    lblSegmentName.Text = "NSE-FO";
                    //if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "5")
                    //    lblSegmentName.Text = "BSE-FO";
                    //if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "3")
                    //    lblSegmentName.Text = "NSE-CDX";
                    //if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "6")
                    //    lblSegmentName.Text = "BSE-CDX";
                    //if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "7")
                    //    lblSegmentName.Text = "MCX-COMM";
                    //if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "8")
                    //    lblSegmentName.Text = "MCXSX-CDX";
                    //if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "9")
                    //    lblSegmentName.Text = "NCDEX-COMM";
                    //if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "10")
                    //    lblSegmentName.Text = "DGCX-COMM";
                    //if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "11")
                    //    lblSegmentName.Text = "NMCE-COMM";
                    //if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "12")
                    //    lblSegmentName.Text = "ICEX-COMM";
                    //if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "13")
                    //    lblSegmentName.Text = "USE-CDX";
                    //if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "14")
                    //    lblSegmentName.Text = "NSEL-SPOT";
                    //if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "16")
                    //    lblSegmentName.Text = "Accounts";
                    //if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "17")
                    //    lblSegmentName.Text = "ACE-COMM";
                    //if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "18")
                    //    lblSegmentName.Text = "INMX-COMM";
                    //if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "19")
                    //    lblSegmentName.Text = "MCXSX-CM";
                    //if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "20")
                    //    lblSegmentName.Text = "MCXSX-FO";
                    //if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "21")
                    //    lblSegmentName.Text = "BFX-COMM";
                    //if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "22")
                    //    lblSegmentName.Text = "INMX-COMM";
                    //if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "23")
                    //    lblSegmentName.Text = "INFX-COMM";

                //}
                ///////Branch Fetch

                #endregion Thrash Code
                

                if (Session["userbranchHierarchy"] != null) // Session Null Handling
                {
                    //dsBranch.SelectCommand = "SELECT BRANCH_id AS BANKBRANCH_ID , BRANCH_DESCRIPTION AS BANKBRANCH_NAME  FROM TBL_MASTER_BRANCH where BRANCH_id in(" + Session["userbranchHierarchy"].ToString() + ")";
                    dsBranch.SelectCommand = "SELECT BRANCH_id AS BANKBRANCH_ID , BRANCH_DESCRIPTION AS BANKBRANCH_NAME  FROM TBL_MASTER_BRANCH where BRANCH_id in(" + Convert.ToString(Session["userbranchHierarchy"]) + ")";
                    cmbBranch.DataBind();
                }

                //CRopening.ClientInstanceName = "Ctxt";
                //DRopening.ClientInstanceName = "Dtxt";


                //CRopening.ClientSideEvents.KeyUp = "function(s,e){aaa(this,event," + CRopening.ClientID + ");}"; 
                //CRopening.ClientSideEvents.TextChanged = "function(s,e){Dtxt" + ".SetText('000000000.00');}"; 

                //DRopening.ClientSideEvents.KeyUp = "function(s,e){aaa(this,event," + DRopening.ClientID + ");}"; 
                //DRopening.ClientSideEvents.TextChanged = "function(s,e){Ctxt" + ".SetText('000000000.00');}";

                // .............................Code Above Commented and Added by Sam on 07122016...................................... 
               
               

                DataTable dtSegid = new DataTable();
                //if (Convert.ToString(HttpContext.Current.Session["Segmentname"]) == "NSDL")
                //{
                //    dtSegid = oDBEngine.GetDataTable("tbl_master_companyexchange", "Exch_internalid", "exch_tmcode='" + Session["usersegid"].ToString().Trim() + "' and exch_compid='" + Session["LastCompany"].ToString().Trim() + "' and exch_membershiptype='NSDL'");
                //    ViewState["Seg"] = dtSegid.Rows[0][0].ToString().Trim();
                //}
                //else if (Convert.ToString(HttpContext.Current.Session["Segmentname"]) == "CDSL")
                //{
                //    dtSegid = oDBEngine.GetDataTable("tbl_master_companyexchange", "Exch_internalid", "exch_tmcode='" + Session["usersegid"].ToString().Trim() + "' and exch_compid='" + Session["LastCompany"].ToString().Trim() + "'  and exch_membershiptype='CDSL'");
                //    ViewState["Seg"] = dtSegid.Rows[0][0].ToString().Trim();
                //}
                //else
                //{
                if (Session["usersegid"] != null) // Session Null Handling
                {
                    ViewState["Seg"] = Convert.ToString(Session["usersegid"]).Trim();
                    Session["Seg"] = Convert.ToString(Session["usersegid"]).Trim();
                }
                //}

                //CheckDataExitOrNot(Request.QueryString["id"].ToString().Trim());
                CheckDataExitOrNot(Convert.ToString(Request.QueryString["id"]).Trim());
            }

        }

        private void CheckDataExitOrNot(string strKeyvalue)
        {
            // .............................Code Above Commented and Added by Sam on 07122016. to avoid tostring ()..................................... 
            //dataTable = oDBEngine.GetDataTable("trans_accountsLedger", "accountsLedger_MAINACCOUNTID,accountsLedger_COMPANYID as CompId,accountsLedger_EXCHANGESEGMENTID as ExchID,accountsLedger_BRANCHID as BranchID,cast(accountsLedger_AmountDr as decimal(18,2)) as Dr,cast (accountsLedger_AmountCr as decimal(18,2)) as Cr", "accountsLedger_MAINACCOUNTID in(select MainAccount_AccountCode from master_mainaccount where MainAccount_ReferenceID='" + strKeyvalue + "') and accountsLedger_ExchangeSegmentID='" + ViewState["Seg"].ToString().Trim() + "'    and accountsledger_finyear='" + Session["LastFinYear"].ToString().Trim() + "' and accountsLedger_COMPANYID='" + Session["LastCompany"].ToString().Trim() + "'  and accountsLedger_TransactionType='OpeningBalance'");
            dataTable = oDBEngine.GetDataTable("trans_accountsLedger", "accountsLedger_MAINACCOUNTID,accountsLedger_COMPANYID as CompId,accountsLedger_EXCHANGESEGMENTID as ExchID,accountsLedger_BRANCHID as BranchID,cast(accountsLedger_AmountDr as decimal(18,2)) as Dr,cast (accountsLedger_AmountCr as decimal(18,2)) as Cr", "accountsLedger_MAINACCOUNTID in(select MainAccount_AccountCode from master_mainaccount where MainAccount_ReferenceID='" + strKeyvalue + "') and accountsLedger_ExchangeSegmentID='" + Convert.ToString(ViewState["Seg"]).Trim() + "'    and accountsledger_finyear='" + Convert.ToString(Session["LastFinYear"]).Trim() + "' and accountsLedger_COMPANYID='" + Convert.ToString(Session["LastCompany"]).Trim() + "'  and accountsLedger_TransactionType='OpeningBalance'");
            // .............................Code Above Commented and Added by Sam on 07122016. to avoid tostring ()..................................... 

            if (dataTable.Rows.Count != 0)
            {
                hdnStatus.Value = "1";

                if (DBNull.Value != dataTable.Rows[0]["BranchID"])
                {

                    //cmbBranch.Value = dataTable.Rows[0]["BranchID"].ToString().Trim();
                    cmbBranch.Value = Convert.ToString(dataTable.Rows[0]["BranchID"]).Trim();

                }
                else
                {
                    cmbBranch.SelectedIndex = 0;
                }
                if (DBNull.Value != dataTable.Rows[0]["Dr"])
                {
                    //DRopening.Text = dataTable.Rows[0]["Dr"].ToString();
                    DRopening.Text = Convert.ToString(dataTable.Rows[0]["Dr"]);
                }
                else
                {
                    DRopening.Text = "000000.00";
                }
                if (DBNull.Value != dataTable.Rows[0]["Cr"])
                {
                    //CRopening.Text = dataTable.Rows[0]["Cr"].ToString();
                    CRopening.Text = Convert.ToString(dataTable.Rows[0]["Cr"]);
                }
                else
                {
                    CRopening.Text = "00000.00";
                }





            }
            else
            {
                hdnStatus.Value = "0";
                cmbBranch.SelectedIndex = 0;

            }


        }


        protected void Button1_Click(object sender, EventArgs e)
        {
            //int Keyvalue = Convert.ToInt32(Request.QueryString["id"].ToString());
            int Keyvalue = 0;
            if (Convert.ToString(Request.QueryString["id"]) != "")
            {
                Keyvalue = Convert.ToInt32(Request.QueryString["id"]);
            }
            //if (hdnStatus.Value.ToString() == "1")
            if (Convert.ToString(hdnStatus.Value) == "1")
            {
                //Update will be do here.

                updateMainAccount(Keyvalue);
            }
            else
            {
                //Insert will do here.

                InsertInTransMainAccount(Keyvalue);

            }

        }
        private void InsertInTransMainAccount(int Keyvalue)
        {
            // New code for Tire architecture
            strOpeningCr = CRopening.Text.Remove(0, 3);
            strOpeningDr = DRopening.Text.Remove(0, 3);
            // .............................Code Commented and Added by Sam on 07122016 to change tostring()  . .....................................  
            int NoOfRowEffected = fobbl.InsertInTransMainAccount(Convert.ToString(Session["LastFinYear"]).Trim(), Keyvalue,
                Convert.ToString(Session["LastCompany"]).Trim(), Convert.ToString(ViewState["Seg"]).Trim(), Convert.ToString(cmbBranch.Value),
                Convert.ToDecimal(Convert.ToString(strOpeningCr)), Convert.ToDecimal(Convert.ToString(strOpeningDr)), Convert.ToString(Session["ActiveCurrency"]).Split('~')[0]);

            //int NoOfRowEffected = fobbl.InsertInTransMainAccount(Session["LastFinYear"].ToString().Trim(), Keyvalue,
            //    Session["LastCompany"].ToString().Trim(), ViewState["Seg"].ToString().Trim(), cmbBranch.Value.ToString(),
            //    Convert.ToDecimal(strOpeningCr.ToString()), Convert.ToDecimal(strOpeningDr.ToString()), Session["ActiveCurrency"].ToString().Split('~')[0]);
            // .............................Code Above Commented and Added by Sam on 07122016...................................... 
            if (NoOfRowEffected != 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "JScript", "alert('Saved Successfully');window.location ='MainAccountHead.aspx';", true);
                //  ScriptManager.RegisterStartupScript(this, GetType(), "JScript", "alert('Record Updated Successfully');window.parent.popup.Hide();", true);
            }
        }
        private void updateMainAccount(int Keyvalue)
        {
            string segment = null;

            // New code for Tire architecture
            strOpeningCr = CRopening.Text.Remove(0, 3);
            strOpeningDr = DRopening.Text.Remove(0, 3);
            // .............................Code Commented and Added by Sam on 07122016 to change tostring()  . ..................................... 

            int NoOfRowEffected = fobbl.updateMainAccount("UpdateMainAccount", Convert.ToString(Session["LastFinYear"]).Trim(), Keyvalue,
                Convert.ToString(Session["LastCompany"]).Trim(), Convert.ToString(ViewState["Seg"]), Convert.ToString(cmbBranch.Value), 
                Convert.ToDecimal(Convert.ToString(strOpeningCr)),
                Convert.ToDecimal(Convert.ToString(strOpeningDr)), Convert.ToString(Session["ActiveCurrency"]).Split('~')[0], 
                Convert.ToString(Session["TradeCurrency"]).Split('~')[0]);
            //int NoOfRowEffected = fobbl.updateMainAccount("UpdateMainAccount", Session["LastFinYear"].ToString().Trim(), Convert.ToInt64(Keyvalue.ToString()),
            //    Session["LastCompany"].ToString().Trim(), ViewState["Seg"].ToString().Trim(), cmbBranch.Value.ToString(), Convert.ToDecimal(strOpeningCr.ToString()),
            //    Convert.ToDecimal(strOpeningDr.ToString()), Session["ActiveCurrency"].ToString().Split('~')[0], Session["TradeCurrency"].ToString().Split('~')[0]);

            // .............................Code Above Commented and Added by Sam on 07122016...................................... 
            if (NoOfRowEffected != 0)
            {
                // ScriptManager.RegisterStartupScript(this, GetType(), "JScript", "alert('Record Updated Successfully');window.parent.popup.Hide();", true);
                ScriptManager.RegisterStartupScript(this, GetType(), "JScript", "alert('Saved Successfully');window.location ='MainAccountHead.aspx';", true);
            }
        } 
         [WebMethod(EnableSession=true)]
        public static List<string> GetBranchOpeningAmt(string BranchId)
        {
            //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);MULTI
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
            List<string> obj = new List<string>();
            DataTable dt = new DataTable();
            var id = Convert.ToString(HttpContext.Current.Session["id"]);
             
            var segname = Convert.ToString(HttpContext.Current.Session["Seg"]); 

            //dt = oDBEngine.GetDataTable("trans_accountsLedger", "cast(accountsLedger_AmountDR as decimal(18,2)) as Dr,cast (accountsLedger_AmountCR as decimal(18,2)) as Cr", "accountsLedger_MAINACCOUNTID in(select MainAccount_AccountCode from master_mainaccount where MainAccount_ReferenceID='" + Request.QueryString["id"].ToString() + "') and accountsLedger_ExchangeSegmentID='" + ViewState["Seg"].ToString().Trim() + "' and accountsLedger_BranchID='" + cmbBranch.SelectedItem.Value.ToString().Trim() + "' and accountsLedger_TransactionType='OpeningBalance'");
            dt = oDBEngine.GetDataTable("trans_accountsLedger", "cast(accountsLedger_AmountDR as decimal(18,2)) as Dr,cast (accountsLedger_AmountCR as decimal(18,2)) as Cr", "accountsLedger_MAINACCOUNTID in(select MainAccount_AccountCode from master_mainaccount where MainAccount_ReferenceID='" + id + "') and accountsLedger_ExchangeSegmentID='" + segname + "' and accountsLedger_BranchID='" + BranchId + "' and accountsLedger_TransactionType='OpeningBalance'");

            if (dt.Rows.Count > 0)
            { 
                obj.Add(Convert.ToString(dt.Rows[0]["Dr"]) + "|" + Convert.ToString(dt.Rows[0]["Cr"])); 
            } 
            return obj; 
        }

         protected void cmbBranch_SelectedIndexChanged(object sender, EventArgs e)
         {

             //dataTable = oDBEngine.GetDataTable("trans_accountsLedger", "cast(accountsLedger_AmountDR as decimal(18,2)) as Dr,cast (accountsLedger_AmountCR as decimal(18,2)) as Cr", "accountsLedger_MAINACCOUNTID in(select MainAccount_AccountCode from master_mainaccount where MainAccount_ReferenceID='" + Request.QueryString["id"].ToString() + "') and accountsLedger_ExchangeSegmentID='" + ViewState["Seg"].ToString().Trim() + "' and accountsLedger_BranchID='" + cmbBranch.SelectedItem.Value.ToString().Trim() + "' and accountsLedger_TransactionType='OpeningBalance'");

             //if (dataTable.Rows.Count > 0)
             //{
             //    if (DBNull.Value != dataTable.Rows[0]["Dr"])
             //    {
             //        DRopening.Text = dataTable.Rows[0]["Dr"].ToString();
             //    }
             //    else
             //    {
             //        DRopening.Text = "000000.00";
             //    }
             //    if (DBNull.Value != dataTable.Rows[0]["Cr"])
             //    {
             //        CRopening.Text = dataTable.Rows[0]["Cr"].ToString();
             //    }
             //    else
             //    {
             //        CRopening.Text = "00000.00";
             //    }
             //}
             //else
             //{
             //    DRopening.Text = "0.00";
             //    CRopening.Text = "0.00";
             //}
         }
    }
}