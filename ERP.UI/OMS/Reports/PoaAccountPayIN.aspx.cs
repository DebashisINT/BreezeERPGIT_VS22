using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogicLayer;

namespace ERP.OMS.Reports
{
    public partial class Reports_PoaAccountPayIN : System.Web.UI.Page
    {
        DataTable mytable = new DataTable();
        static DataSet DS = new DataSet();
        DBEngine oDBEngine = new DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);
        Converter oconverter = new Converter();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string SettNumber = Session["LastSettNo"].ToString();
                txtSettNo.Text = SettNumber;
                DtTransferDate.Value = Convert.ToDateTime(oDBEngine.GetDate());
                Page.ClientScript.RegisterStartupScript(GetType(), "JS", "<script language='JavaScript'>height();</script>");
                ShowData();
            }
        }
        public void ShowData()
        {
            #region Bind Target A/C
            DataTable DTTarget = oDBEngine.GetDataTable("Master_DpAccounts", "DpAccounts_ID,DpAccounts_ShortName+' ['+rtrim(DpAccounts_ClientID)+']' as Name", " DpAccounts_ExchangeSegmentID=" + Session["usersegid"].ToString() + " and DpAccounts_AccountType in('[POOL]','[PLPAYIN]')");
            ddlTargetAc.DataTextField = "Name";
            ddlTargetAc.DataValueField = "DpAccounts_ID";
            ddlTargetAc.DataSource = DTTarget;
            ddlTargetAc.DataBind();
            #endregion
            #region Bind Source DP
            BindSDP();
            #endregion
            #region POA Gridview Show
            ShowPOAGrid();
            #endregion
        }
        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            int NoofRowsAffect = 0;
            string SettmentNo = txtSettNo.Text.ToString().Substring(0, 7);
            string SettType = txtSettNo.Text.ToString().Substring(7);

            DataTable DtPOAProcessing = new DataTable();
            DtPOAProcessing.Columns.Add(new DataColumn("ClientName", typeof(String))); //0
            DtPOAProcessing.Columns.Add(new DataColumn("Branch", typeof(String)));//1
            DtPOAProcessing.Columns.Add(new DataColumn("SettNumber", typeof(String)));//2
            DtPOAProcessing.Columns.Add(new DataColumn("Scrip", typeof(String)));//3
            DtPOAProcessing.Columns.Add(new DataColumn("ISIN", typeof(String)));//4
            DtPOAProcessing.Columns.Add(new DataColumn("IncomingPending", typeof(String)));//5
            DtPOAProcessing.Columns.Add(new DataColumn("HoldOnPoa", typeof(String)));//6
            DtPOAProcessing.Columns.Add(new DataColumn("Received", typeof(String))); //7
            DtPOAProcessing.Columns.Add(new DataColumn("CustomerID", typeof(String)));//8
            DtPOAProcessing.Columns.Add(new DataColumn("ProductSeriesID", typeof(String)));//9
            DtPOAProcessing.Columns.Add(new DataColumn("dpCode", typeof(String)));//10
            DtPOAProcessing.Columns.Add(new DataColumn("dpClient", typeof(String)));//11
            DtPOAProcessing.Columns.Add(new DataColumn("BranchID", typeof(String)));//12
            DtPOAProcessing.Columns.Add(new DataColumn("dpdID", typeof(String)));//13
            DtPOAProcessing.Columns.Add(new DataColumn("SNumber", typeof(String)));//14
            DtPOAProcessing.Columns.Add(new DataColumn("SType", typeof(String)));//15

            foreach (GridViewRow row in grdPOAClient.Rows)
            {
                Label lblClientID = (Label)row.FindControl("lblClientID");
                Label lblBranch = (Label)row.FindControl("lblBranch");
                Label lblSettNumber = (Label)row.FindControl("lblSettNumber");
                Label lblScrip = (Label)row.FindControl("lblScrip");
                Label lblISIN = (Label)row.FindControl("lblISIN");
                Label lblIncomingPending = (Label)row.FindControl("lblIncomingPending");
                Label lblHoldOnPoa = (Label)row.FindControl("lblHoldOnPoa");
                TextBox txtReceived = (TextBox)row.FindControl("txtReceived");
                Label lblCustomerID = (Label)row.FindControl("lblCustomerID");
                Label lblProductSeriesID = (Label)row.FindControl("lblProductSeriesID");
                Label lbldpCode = (Label)row.FindControl("lbldpCode");
                Label lbldpClient = (Label)row.FindControl("lbldpClient");
                Label lblBranchID = (Label)row.FindControl("lblBranchID");
                Label lbldpdID = (Label)row.FindControl("lbldpdID");
                Label lblSNumber = (Label)row.FindControl("lblSNumber");
                Label lblSType = (Label)row.FindControl("lblSType");
                CheckBox ChkDelivery = (CheckBox)row.FindControl("ChkDelivery");
                if (ChkDelivery.Checked == true)
                {
                    DataRow drPOA = DtPOAProcessing.NewRow();
                    drPOA[0] = lblClientID.Text;
                    drPOA[1] = lblBranch.Text;
                    drPOA[2] = lblSettNumber.Text;
                    drPOA[3] = lblScrip.Text;
                    drPOA[4] = lblISIN.Text;
                    drPOA[5] = lblIncomingPending.Text;
                    drPOA[6] = lblHoldOnPoa.Text;
                    drPOA[7] = txtReceived.Text;
                    drPOA[8] = lblCustomerID.Text;
                    drPOA[9] = lblProductSeriesID.Text;
                    drPOA[10] = lbldpCode.Text;
                    drPOA[11] = lbldpClient.Text;
                    drPOA[12] = lblBranchID.Text;
                    drPOA[13] = lbldpdID.Text;
                    drPOA[14] = lblSNumber.Text;
                    drPOA[15] = lblSType.Text;
                    DtPOAProcessing.Rows.Add(drPOA);
                }
            }
            string tabledata = oconverter.ConvertDataTableToXML(DtPOAProcessing);
            String conn = ConfigurationManager.AppSettings["DBConnectionDefault"];
            SqlConnection con = new SqlConnection(conn);
            con.Open();
            SqlCommand com = new SqlCommand("DeliveryProcessingPOAClient", con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@clientPayoutData", tabledata);
            com.Parameters.AddWithValue("@Finyear", Session["LastFinYear"].ToString());
            com.Parameters.AddWithValue("@compID", Session["LastCompany"].ToString());
            com.Parameters.AddWithValue("@segmentid", Session["usersegid"].ToString());
            com.Parameters.AddWithValue("@settlementNumber", SettmentNo);
            com.Parameters.AddWithValue("@settlementType", SettType);
            com.Parameters.AddWithValue("@TransferDate", Convert.ToDateTime(DtTransferDate.Value));
            com.Parameters.AddWithValue("@CreateUser", Session["userid"].ToString());
            com.Parameters.AddWithValue("@TargetAC", ddlTargetAc.SelectedItem.Value);
            com.Parameters.AddWithValue("@SlipNumber", txtSlipNumber.Text);
            NoofRowsAffect = com.ExecuteNonQuery();
            con.Close();
            if (NoofRowsAffect > 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "JJ", "alert('Processing Successfully !!');", true);
            }
            //ShowData();
        }
        public void ShowPOAGrid()
        {
            try
            {
                //mytable.Clear();
                if (ViewState["CheckDataTable"] == null)
                {
                    //mytable.Dispose();
                    //mytable.Rows.Clear();
                    //mytable.Clear();                
                    DataColumn ClientName = new DataColumn("ClientName");
                    DataColumn Branch = new DataColumn("Branch");
                    DataColumn SettNumber = new DataColumn("SettNumber");
                    DataColumn Scrip = new DataColumn("Scrip");
                    DataColumn ISIN = new DataColumn("ISIN");
                    DataColumn IncomingPending = new DataColumn("IncomingPending");
                    DataColumn HoldOnPoa = new DataColumn("HoldOnPoa");
                    DataColumn Received = new DataColumn("Received");
                    DataColumn CustomerID = new DataColumn("CustomerID");
                    DataColumn ProductSeriesID = new DataColumn("ProductSeriesID");
                    DataColumn dpCode = new DataColumn("dpCode");
                    DataColumn dpClient = new DataColumn("dpClient");
                    DataColumn BranchID = new DataColumn("BranchID");
                    DataColumn dpdID = new DataColumn("dpdID");
                    DataColumn SNumber = new DataColumn("SNumber");
                    DataColumn SType = new DataColumn("SType");
                    mytable.Columns.Add(ClientName);
                    mytable.Columns.Add(Branch);
                    mytable.Columns.Add(SettNumber);
                    mytable.Columns.Add(Scrip);
                    mytable.Columns.Add(ISIN);
                    mytable.Columns.Add(IncomingPending);
                    mytable.Columns.Add(HoldOnPoa);
                    mytable.Columns.Add(Received);
                    mytable.Columns.Add(CustomerID);
                    mytable.Columns.Add(ProductSeriesID);
                    mytable.Columns.Add(dpCode);
                    mytable.Columns.Add(dpClient);
                    mytable.Columns.Add(BranchID);
                    mytable.Columns.Add(dpdID);
                    mytable.Columns.Add(SNumber);
                    mytable.Columns.Add(SType);
                }
                if (ViewState["mytable"] != null)
                    mytable = (DataTable)ViewState["mytable"];
                mytable.Rows.Clear();
                string expression = "dpCode = '" + ddlSourceDp.SelectedItem.Value + "'";
                DataRow[] rows = DS.Tables[0].Select(expression);
                foreach (DataRow row1 in rows)
                {
                    DataRow newrow = mytable.NewRow();
                    newrow["ClientName"] = row1["ClientName"];
                    newrow["Branch"] = row1["Branch"];
                    newrow["SettNumber"] = row1["SettNumber"];
                    newrow["Scrip"] = row1["Scrip"];
                    newrow["ISIN"] = row1["ISIN"];
                    newrow["IncomingPending"] = row1["IncomingPending"];
                    newrow["HoldOnPoa"] = row1["HoldOnPoa"];
                    newrow["Received"] = row1["Received"];
                    newrow["CustomerID"] = row1["CustomerID"];
                    newrow["ProductSeriesID"] = row1["ProductSeriesID"];
                    newrow["dpCode"] = row1["dpCode"];
                    newrow["dpClient"] = row1["dpClient"];
                    newrow["BranchID"] = row1["BranchID"];
                    newrow["dpdID"] = row1["dpdID"];
                    newrow["SNumber"] = row1["SNumber"];
                    newrow["SType"] = row1["SType"];
                    mytable.Rows.Add(newrow);
                }
                grdPOAClient.DataSource = mytable;
                grdPOAClient.DataBind();
                ViewState["mytable"] = mytable;
                ViewState["CheckDataTable"] = "a";
            }
            catch
            {
                DataTable DtDummy = new DataTable();
                grdPOAClient.DataSource = DtDummy;
                grdPOAClient.DataBind();
            }
        }
        protected void ddlSourceDp_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowPOAGrid();
        }
        protected void grdPOAClient_RowCreated(object sender, GridViewRowEventArgs e)
        {
            string rowID = String.Empty;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                rowID = "row" + e.Row.RowIndex;
                e.Row.Attributes.Add("id", "row" + e.Row.RowIndex);
                e.Row.Attributes.Add("onclick", "ChangeRowColor(" + "'" + rowID + "','" + grdPOAClient.Rows.Count + "'" + ")");
            }
        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            BindSDP();
            ShowPOAGrid();
        }
        public void BindSDP()
        {
            DS.Tables.Clear();
            string SettmentNum = txtSettNo.Text.ToString().Substring(0, 7);
            string SettmentType = txtSettNo.Text.ToString().Substring(7);
            String conn = ConfigurationSettings.AppSettings["DBConnectionDefault"];
            SqlConnection con = new SqlConnection(conn);
            SqlCommand cmd3 = new SqlCommand("FetchPOAClient", con);
            cmd3.CommandType = CommandType.StoredProcedure;
            cmd3.Parameters.AddWithValue("@SettNumber", SettmentNum);
            cmd3.Parameters.AddWithValue("@SettType", SettmentType);
            cmd3.Parameters.AddWithValue("@FinYear", Session["LastFinYear"].ToString().Trim());
            cmd3.CommandTimeout = 0;
            SqlDataAdapter Adap = new SqlDataAdapter();
            Adap.SelectCommand = cmd3;
            Adap.Fill(DS);
            ddlSourceDp.DataTextField = "dpd_dpCode";
            ddlSourceDp.DataValueField = "dpd_dpCode";
            ddlSourceDp.DataSource = DS.Tables[1];
            ddlSourceDp.DataBind();
        }
        protected void grdPOAClient_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                ((CheckBox)e.Row.FindControl("cbSelectAll")).Attributes.Add("onclick", "javascript:SelectAll('" + ((CheckBox)e.Row.FindControl("cbSelectAll")).ClientID + "')");
            }
        }
    }
}