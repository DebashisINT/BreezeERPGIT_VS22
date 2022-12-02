using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogicLayer;

namespace ERP.OMS.Reports
{
    public partial class Reports_AuctionCarryForward : System.Web.UI.Page
    {
        static DataSet DS = new DataSet();
        Converter oconverter = new Converter();
        DBEngine oDbEngine = new DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtSourceSett.Text = Session["LastSettNo"].ToString();
                string settNo = Session["LastSettNo"].ToString();
                string SettmentNoSource = settNo.Substring(0, 7);
                string SettType = settNo.Substring(7);
                if (Session["ExchangeSegmentID"].ToString() == "4")
                    txtTargetSett.Text = SettmentNoSource + "D";
                else
                {
                    if (SettType == "W")
                        txtTargetSett.Text = SettmentNoSource + "X";
                    else
                        txtTargetSett.Text = SettmentNoSource + "A";
                }
                Page.ClientScript.RegisterStartupScript(GetType(), "JS", "<script language='JavaScript'>height();</script>");
                Page.ClientScript.RegisterStartupScript(GetType(), "JS12", "<script language='JavaScript'>Page_Load();</script>");
            }
        }
        protected void btnShow_Click(object sender, EventArgs e)
        {
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "4" && txtSourceSett.Text.ToString().Trim().Substring(7, 1) == "Z")
            {
                ClosingRateFind();

            }
            else
            {
                FetchData();
            }
        }
        void ClosingRateFind()
        {
            string settNo = txtSourceSett.Text;
            string SettmentNo = settNo.Substring(0, 7);
            string SettType = settNo.Substring(7);
            DataTable DtSettFetch = oDbEngine.GetDataTable("Master_Settlements", "distinct Settlements_Startdatetime,Settlements_FundsPayout", "Settlements_Number='" + SettmentNo.ToString().Trim() + "' and Settlements_TypeSuffix='" + SettType.ToString().Trim() + "' and  Settlements_ExchangeSegmentId=4");
            if (DtSettFetch.Rows.Count > 0)
            {
                DataTable DtRateFetch = oDbEngine.GetDataTable("Trans_DailyStatistics", "distinct DailyStat_DateTime", "DailyStat_ExchangeSegmentId='4' and DailyStat_DateTime  Between '" + DtSettFetch.Rows[0]["Settlements_Startdatetime"].ToString().Trim() + "' and '" + DtSettFetch.Rows[0]["Settlements_FundsPayout"].ToString().Trim() + "'");
                if (DtRateFetch.Rows.Count > 2)
                {
                    if (DtRateFetch.Rows[0]["DailyStat_DateTime"].ToString().Trim() == DtSettFetch.Rows[0]["Settlements_Startdatetime"].ToString().Trim())
                    {
                        if (DtRateFetch.Rows[DtRateFetch.Rows.Count - 1]["DailyStat_DateTime"].ToString().Trim() == DtSettFetch.Rows[0]["Settlements_FundsPayout"].ToString().Trim())
                        {
                            FetchData();
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "GiveAlert", "GiveAlert('" + oconverter.ArrangeDate2(DtSettFetch.Rows[0]["Settlements_Startdatetime"].ToString().Trim()) + " To " + oconverter.ArrangeDate2(DtSettFetch.Rows[0]["Settlements_FundsPayout"].ToString().Trim()) + "');", true);
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "GiveAlert", "GiveAlert('" + oconverter.ArrangeDate2(DtSettFetch.Rows[0]["Settlements_Startdatetime"].ToString().Trim()) + " To " + oconverter.ArrangeDate2(DtSettFetch.Rows[0]["Settlements_FundsPayout"].ToString().Trim()) + "');", true);
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "GiveAlert", "GiveAlert('" + oconverter.ArrangeDate2(DtSettFetch.Rows[0]["Settlements_Startdatetime"].ToString().Trim()) + " To " + oconverter.ArrangeDate2(DtSettFetch.Rows[0]["Settlements_FundsPayout"].ToString().Trim()) + "');", true);
                }
            }
        }
        void FetchData()
        {
            DS.Dispose();
            DS.Tables.Clear();
            string settNo = txtSourceSett.Text;
            string SettmentNo = settNo.Substring(0, 7);
            string SettType = settNo.Substring(7);
            String conn = ConfigurationSettings.AppSettings["DBConnectionDefault"];
            SqlConnection con = new SqlConnection(conn);
            SqlCommand cmd3 = new SqlCommand("CarryForwardForAuction", con);
            cmd3.CommandType = CommandType.StoredProcedure;
            cmd3.Parameters.AddWithValue("@companyID", Session["LastCompany"].ToString());
            cmd3.Parameters.AddWithValue("@SegmentID", Session["usersegid"].ToString());
            cmd3.Parameters.AddWithValue("@SettNumber", SettmentNo);
            cmd3.Parameters.AddWithValue("@SettType", SettType);
            cmd3.Parameters.AddWithValue("@ExchID", Session["ExchangeSegmentID"].ToString());
            cmd3.Parameters.AddWithValue("@Finyear", HttpContext.Current.Session["LastFinYear"].ToString().Trim());
            cmd3.CommandTimeout = 0;
            SqlDataAdapter Adap = new SqlDataAdapter();
            Adap.SelectCommand = cmd3;
            Adap.Fill(DS);
            grdDematProcessing.DataSource = DS.Tables[0];
            grdDematProcessing.DataBind();
            ScriptManager.RegisterStartupScript(this, GetType(), "JJ12", "ButtonShow();", true);
        }
        protected void grdDematProcessing_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridViewRow row = e.Row;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                try
                {
                    TextBox txtStock = (TextBox)row.FindControl("txtStock");
                    Label lblProID = (Label)row.FindControl("lblProID");
                    string expression = "DematPosition_ProductSeriesID = '" + lblProID.Text + "'";
                    DataRow[] rows = DS.Tables[1].Select(expression);
                    string ExchQty = rows[0][2].ToString();
                    txtStock.Attributes.Add("onBlur", "javascript:DeliverableValue(" + txtStock.ClientID + ",'" + lblProID.Text + "','" + ExchQty + "')");
                }
                catch
                {
                }
            }
            if (e.Row.RowType == DataControlRowType.Header)
            {
                ((CheckBox)e.Row.FindControl("cbSelectAll")).Attributes.Add("onclick", "javascript:SelectAll('" + ((CheckBox)e.Row.FindControl("cbSelectAll")).ClientID + "')");
            }
        }
        protected void btnCheck_Click(object sender, EventArgs e)
        {
            int NoofRowsAffect = 0;
            DataTable DtCarryForward = new DataTable();
            DtCarryForward.Columns.Add(new DataColumn("CustomerID", typeof(String))); //0
            DtCarryForward.Columns.Add(new DataColumn("ISIN", typeof(String)));//1
            DtCarryForward.Columns.Add(new DataColumn("ProductID", typeof(String)));//2
            DtCarryForward.Columns.Add(new DataColumn("Quantity", typeof(Decimal)));//3
            DtCarryForward.Columns.Add(new DataColumn("SettNumberSource", typeof(String)));//4
            DtCarryForward.Columns.Add(new DataColumn("SettTypeSource", typeof(String)));//5
            DtCarryForward.Columns.Add(new DataColumn("SettNumberTarget", typeof(String)));//6
            DtCarryForward.Columns.Add(new DataColumn("SettTypeTarget", typeof(String)));//7
            DtCarryForward.Columns.Add(new DataColumn("CompanyID", typeof(String)));//8
            DtCarryForward.Columns.Add(new DataColumn("SegmentID", typeof(String)));//9
            DtCarryForward.Columns.Add(new DataColumn("FinYear", typeof(String)));//10
            DtCarryForward.Columns.Add(new DataColumn("Position", typeof(String)));//11
            DtCarryForward.Columns.Add(new DataColumn("CType", typeof(String)));//12
            DtCarryForward.Columns.Add(new DataColumn("IncomingOutgoing", typeof(String)));//13
            foreach (GridViewRow row in grdDematProcessing.Rows)
            {
                Label lblCusID = (Label)row.FindControl("lblCusID");
                Label lblISIN = (Label)row.FindControl("lblISIN");
                Label lblProID = (Label)row.FindControl("lblProID");
                TextBox txtStock = (TextBox)row.FindControl("txtStock");
                Label lblDelPos = (Label)row.FindControl("lblDelPos");
                Label lblCType = (Label)row.FindControl("lblCType");
                Label lblInComing = (Label)row.FindControl("lblInComing");
                Label lblOutGoing = (Label)row.FindControl("lblOutGoing");
                string settNo = txtSourceSett.Text;
                string SettmentNoSource = settNo.Substring(0, 7);
                string SettTypeSource = settNo.Substring(7);
                string settNoTarget = txtTargetSett.Text;
                string SettmentNoTarget = settNoTarget.Substring(0, 7);
                string SettTypeTarget = settNoTarget.Substring(7);
                CheckBox ChkDelivery = (CheckBox)row.FindControl("ChkDelivery");
                if (ChkDelivery.Checked == true)
                {
                    DataRow drCarry = DtCarryForward.NewRow();
                    drCarry[0] = lblCusID.Text;
                    drCarry[1] = lblISIN.Text;
                    drCarry[2] = lblProID.Text;
                    if (txtStock.Text != "")
                        drCarry[3] = txtStock.Text;
                    else
                        drCarry[3] = "0";
                    drCarry[4] = SettmentNoSource;
                    drCarry[5] = SettTypeSource;
                    drCarry[6] = SettmentNoTarget;
                    drCarry[7] = SettTypeTarget;
                    drCarry[8] = Session["LastCompany"].ToString();
                    drCarry[9] = Session["usersegid"].ToString();
                    drCarry[10] = Session["LastFinYear"].ToString();
                    drCarry[11] = lblDelPos.Text;
                    drCarry[12] = lblCType.Text;
                    if (lblInComing.Text == "")
                        drCarry[13] = "OutGoing";
                    else
                        drCarry[13] = "Incoming";
                    DtCarryForward.Rows.Add(drCarry);
                }
            }
            string tabledata = oconverter.ConvertDataTableToXML(DtCarryForward);
            String conn = ConfigurationManager.AppSettings["DBConnectionDefault"];
            SqlConnection con = new SqlConnection(conn);
            con.Open();
            SqlCommand com = new SqlCommand("DeliveryProcessingForCarryForwardAuction", con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@AuctionData", tabledata);
            com.Parameters.AddWithValue("@CreateUser", Session["userid"].ToString());
            com.Parameters.AddWithValue("@masterSegment", Session["ExchangeSegmentID"].ToString());
            com.CommandTimeout = 0;
            NoofRowsAffect = com.ExecuteNonQuery();
            con.Close();
            if (NoofRowsAffect > 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "JJ", "alert('Processing Successfully !!');", true);
            }
            btnShow_Click(sender, e);
        }
    }

}