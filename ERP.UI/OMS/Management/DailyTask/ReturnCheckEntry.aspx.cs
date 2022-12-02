using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using BusinessLogicLayer;
namespace ERP.OMS.Management.DailyTask
{
    public partial class management_DailyTask_ReturnCheckEntry : ERP.OMS.ViewState_class.VSPage
    {
        BusinessLogicLayer.Converter objConverter = new BusinessLogicLayer.Converter();
       // BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();


        FinancialAccounting oFinancialAccounting = new FinancialAccounting();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                dtDate.EditFormatString = objConverter.GetDateFormat("Date");
                dtDate.Value = Convert.ToDateTime(oDBEngine.GetDate());
                Page.ClientScript.RegisterStartupScript(GetType(), "JS", "<script language='JavaScript'>Page_Load();</script>");
                DataTable DtCurrentSegment = oDBEngine.GetDataTable("(select exch_internalId, isnull((select top 1 exh_shortName from tbl_master_Exchange where exh_cntId=tbl_master_companyExchange.exch_exchId)+'-'+exch_segmentId,exch_membershiptype) as Segment from tbl_master_companyExchange where exch_compid in (select top 1 ls_lastCompany from tbl_trans_Lastsegment where ls_lastSegment=" + Session["userlastsegment"].ToString() + " and ls_userid=" + Session["UserID"].ToString() + ") and exch_compId='" + HttpContext.Current.Session["LastCompany"].ToString() + "') as D", "*", " Segment in(select seg_name from tbl_master_segment where seg_id=" + Session["userlastsegment"].ToString() + ")");
                //DataTable DtCurrentSegment = oDBEngine.GetDataTable("(select exch_internalId, isnull((select exh_shortName from tbl_master_Exchange where exh_cntId=tbl_master_companyExchange.exch_exchId)+'-'+exch_segmentId,exch_membershiptype) as Segment from tbl_master_companyExchange where exch_compid in (select top 1 ls_lastCompany from tbl_trans_Lastsegment where ls_lastSegment=" + Session["userlastsegment"].ToString() + " and ls_userid=" + Session["UserID"].ToString() + ") and exch_compId='" + HttpContext.Current.Session["LastCompany"].ToString() + "') as D", "*", " Segment in(select seg_name from tbl_master_segment where seg_id=" + Session["userlastsegment"].ToString() + ")");
                hdn_CurrentSegment.Value = DtCurrentSegment.Rows[0][0].ToString();
                SetBankNames();
            }
        }

       


        public void SetBankNames()
        {
            //objEngine
            DataTable DT = new DataTable();
            DT = oDBEngine.GetDataTable("tbl_master_Bank", "ltrim(rtrim(bnk_id)) code, ltrim(rtrim(bnk_bankName)) Name", null);
            txtBankName.DataSource = DT;
            txtBankName.DataMember = "Code";
            txtBankName.DataTextField = "Name";
            txtBankName.DataValueField = "Code";
            txtBankName.DataBind();
        }

        protected void btnShow_Click(object sender, EventArgs e)
        {
            if (ddlPurpose.SelectedItem.Value == "I")
            {
                ViewState["Type"] = "Show";
                IssuedByUs();
            }
            else if (ddlPurpose.SelectedItem.Value == "R")
            {
                ViewState["Type"] = "Show";
                ReceivedByUs();
            }
        }
        public void IssuedByUs()
        {
            string Type = ViewState["Type"].ToString();
            DataTable DtSeg = oDBEngine.GetDataTable("(select A.EXCH_INTERNALID AS SEGMENTID ,isnull((TME.EXH_ShortName + '-' + A.EXCH_SEGMENTID),exch_membershipType) AS EXCHANGENAME from (SELECT TMCE.* FROM TBL_MASTER_COMPANYEXCHANGE AS TMCE WHERE  TMCE.EXCH_COMPID='" + Session["LastCompany"].ToString() + "' ) AS A LEFT OUTER JOIN   TBL_MASTER_EXCHANGE AS TME ON TME.EXH_CNTID=A.EXCH_EXCHID) as KK,tbl_master_segment ", " SEGMENTID", " EXCHANGENAME=seg_name and seg_id=" + Session["userlastsegment"].ToString() + "");
            DataSet DS = new DataSet();
            //String conn = ConfigurationSettings.AppSettings["DBConnectionDefault"];
            //using (SqlConnection con = new SqlConnection(conn))
            //{
            //    using (SqlCommand cmd3 = new SqlCommand("ReturnChequeEntry", con))
            //    {
            //        cmd3.CommandType = CommandType.StoredProcedure;
            //        cmd3.Parameters.AddWithValue("@BankID", txtBankName_hidden.Value.Split('~')[0]);
            //        cmd3.Parameters.AddWithValue("@ChequeNumber", txtChequeNumber.Text);
            //        cmd3.Parameters.AddWithValue("@Amount", Convert.ToDecimal(txtAmount.Value));
            //        cmd3.Parameters.AddWithValue("@TranDate", Convert.ToDateTime(dtDate.Value));
            //        cmd3.Parameters.AddWithValue("@CreateUser", Convert.ToInt32(Session["userid"].ToString()));
            //        cmd3.Parameters.AddWithValue("@FinYear", Session["LastFinYear"].ToString());
            //        cmd3.Parameters.AddWithValue("@SegmentID", Convert.ToInt32(DtSeg.Rows[0][0].ToString()));
            //        cmd3.Parameters.AddWithValue("@CompanyID", Session["LastCompany"].ToString());
            //        cmd3.Parameters.AddWithValue("@type", Type);
            //        cmd3.Parameters.AddWithValue("@MNarration", txtNarration.Text);
            //        cmd3.CommandTimeout = 0;
            //        using (SqlDataAdapter Adap = new SqlDataAdapter(cmd3))
            //        {
            //            Adap.Fill(DS);
            //        }
            //    }
            //}
            DS = oFinancialAccounting.ReturnChequeEntry(txtBankName_hidden.Value.Split('~')[0], txtChequeNumber.Text, Convert.ToDecimal(txtAmount.Value),
                Convert.ToDateTime(dtDate.Value).ToString("yyyy-MM-dd"), Convert.ToInt32(Session["userid"].ToString()), Session["LastFinYear"].ToString(),
                Convert.ToInt32(DtSeg.Rows[0][0].ToString()), Session["LastCompany"].ToString(), Type, txtNarration.Text);
            string ReturnValue = DS.Tables[0].Rows[0][0].ToString();
            if (ReturnValue == "4")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "JS1", "alert('This Cheque Has Already Been Entered. Entry DisAllowed !!')", true);
            }
            else if (ReturnValue == "1")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "JS1", "alert('Bank or Cheque or Amount Not Valid !!')", true);
            }
            else if (ReturnValue == "2")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "JS1", "alert('No Cheque in this Bank and Cheque and Amount Combination !!')", true);
            }
            else if (ReturnValue == "3")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "JS1", "alertMessage();", true);
            }
            else
            {
                grdReturnCheque.DataSource = DS.Tables[0];
                grdReturnCheque.DataBind();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "JS1", "Show();", true);
            }
        }
        protected void btnEntry_Click1(object sender, EventArgs e)
        {
            if (ddlPurpose.SelectedItem.Value == "I")
            {
                ViewState["Type"] = "Entry";
                IssuedByUs();
            }
            else if (ddlPurpose.SelectedItem.Value == "R")
            {
                ViewState["Type"] = "Entry";
                ReceivedByUs();
            }
        }
        public void ReceivedByUs()
        {
            string Type = ViewState["Type"].ToString();
            DataTable DtSeg = oDBEngine.GetDataTable("(select A.EXCH_INTERNALID AS SEGMENTID ,isnull((TME.EXH_ShortName + '-' + A.EXCH_SEGMENTID),exch_membershipType) AS EXCHANGENAME from (SELECT top 1 TMCE.* FROM TBL_MASTER_COMPANYEXCHANGE AS TMCE WHERE  TMCE.EXCH_COMPID='" + Session["LastCompany"].ToString() + "' ) AS A LEFT OUTER JOIN   TBL_MASTER_EXCHANGE AS TME ON TME.EXH_CNTID=A.EXCH_EXCHID) as KK,tbl_master_segment ", " SEGMENTID", " EXCHANGENAME=seg_name and seg_id=" + Session["userlastsegment"].ToString() + "");
            //DataTable DtSeg = oDBEngine.GetDataTable("(select A.EXCH_INTERNALID AS SEGMENTID ,isnull((TME.EXH_ShortName + '-' + A.EXCH_SEGMENTID),exch_membershipType) AS EXCHANGENAME from (SELECT TMCE.* FROM TBL_MASTER_COMPANYEXCHANGE AS TMCE WHERE  TMCE.EXCH_COMPID='" + Session["LastCompany"].ToString() + "' ) AS A LEFT OUTER JOIN   TBL_MASTER_EXCHANGE AS TME ON TME.EXH_CNTID=A.EXCH_EXCHID) as KK,tbl_master_segment ", " SEGMENTID", " EXCHANGENAME=seg_name and seg_id=" + Session["userlastsegment"].ToString() + "");
            DataSet DS = new DataSet();
            //String conn = ConfigurationSettings.AppSettings["DBConnectionDefault"];
            //using (SqlConnection con = new SqlConnection(conn))
            //{
            //    using (SqlCommand cmd3 = new SqlCommand("ReturnChequeEntryForReceived", con))
            //    {
            //        cmd3.CommandType = CommandType.StoredProcedure;
            //        cmd3.Parameters.AddWithValue("@BankID", txtBankName_hidden.Value.Split('~')[0]);
            //        cmd3.Parameters.AddWithValue("@ChequeNumber", txtChequeNumber.Text);
            //        cmd3.Parameters.AddWithValue("@Amount", Convert.ToDecimal(txtAmount.Value));
            //        cmd3.Parameters.AddWithValue("@TranDate", Convert.ToDateTime(dtDate.Value));
            //        cmd3.Parameters.AddWithValue("@CreateUser", Convert.ToInt32(Session["userid"].ToString()));
            //        cmd3.Parameters.AddWithValue("@FinYear", Session["LastFinYear"].ToString());
            //        cmd3.Parameters.AddWithValue("@SegmentID", Convert.ToInt32(DtSeg.Rows[0][0].ToString()));
            //        cmd3.Parameters.AddWithValue("@CompanyID", Session["LastCompany"].ToString());
            //        cmd3.Parameters.AddWithValue("@type", Type);
            //        cmd3.Parameters.AddWithValue("@MNarration", txtNarration.Text);
            //        cmd3.CommandTimeout = 0;
            //        using (SqlDataAdapter Adap = new SqlDataAdapter(cmd3))
            //        {
            //            Adap.Fill(DS);
            //        }
            //    }
            //}
            DS = oFinancialAccounting.ReturnChequeEntry(txtBankName_hidden.Value.Split('~')[0], txtChequeNumber.Text, Convert.ToDecimal(txtAmount.Value),
                Convert.ToDateTime(dtDate.Value).ToString("yyyy-MM-dd"), Convert.ToInt32(Session["userid"].ToString()), Session["LastFinYear"].ToString(),
                Convert.ToInt32(DtSeg.Rows[0][0].ToString()), Session["LastCompany"].ToString(), Type, txtNarration.Text);
            string ReturnValue = DS.Tables[0].Rows[0][0].ToString();
            if (ReturnValue == "4")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "JS2", "alert('This Cheque Has Already Been Entered. Entry DisAllowed !!')", true);
            }
            else if (ReturnValue == "1")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "JS2", "alert('Bank or Cheque or Amount Not Valid !!')", true);
            }
            else if (ReturnValue == "2")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "JS2", "alert('No Cheque in this Bank and Cheque and Amount Combination !!')", true);
            }
            else if (ReturnValue == "3")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "JS2", "alertMessage();", true);
            }
            else
            {
                grdReturnCheque.DataSource = DS.Tables[0];
                grdReturnCheque.DataBind();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "JS2", "Show();", true);
            }
        }
    }
}