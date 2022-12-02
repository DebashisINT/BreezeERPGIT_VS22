using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogicLayer;
using DevExpress.Web;

namespace ERP.OMS.Management.ToolsUtilities
{
    public partial class management_ToolsUtilities_userstatus : System.Web.UI.Page
    {
        //DBEngine oDbEngine = new DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        //Converter oDbConverter = new Converter();
        //BusinessLogicLayer.DBEngine oDbEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine oDbEngine = new BusinessLogicLayer.DBEngine();
        BusinessLogicLayer.Converter oDbConverter = new BusinessLogicLayer.Converter();
        Utilities oUtilities = new Utilities();
        DataTable dtActivelist = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ShowActiveList();
                Page.ClientScript.RegisterStartupScript(GetType(), "JS", "<script language='JavaScript'>Page_Load();</script>");
            }
        }
        public void ShowActiveList()
        {
            string Activity = null;
            string FinalActivityName = null;
            DataTable dtActivelist_New = new DataTable();
            dtActivelist_New.Columns.Add(new DataColumn("user_id", typeof(int))); //0
            dtActivelist_New.Columns.Add(new DataColumn("user_loginid", typeof(String))); //1
            dtActivelist_New.Columns.Add(new DataColumn("UserName", typeof(String))); //2 
            //dtActivelist_New.Columns.Add(new DataColumn("user_activity", typeof(String))); //3
            dtActivelist_New.Columns.Add(new DataColumn("Since", typeof(String))); //3
            dtActivelist_New.Columns.Add(new DataColumn("Duration", typeof(String))); //4
            dtActivelist_New.Columns.Add(new DataColumn("user_lastIP", typeof(String))); //5
            dtActivelist = oDbEngine.GetDataTable("tbl_master_user,tbl_master_contact", "user_id,user_loginid,isnull(rtrim(cnt_firstname),'')+' '+isnull(rtrim(cnt_middlename),'')+isnull(rtrim(cnt_lastname),'') as UserName,CONVERT(CHAR(19), cast(tbl_master_user.last_login_date as datetime), 0) as Since,DATEDIFF(MINUTE, cast(tbl_master_user.last_login_date as datetime),getdate()) as Duration,user_lastIP", " cnt_internalId=user_contactid and user_status=1 and user_id not in(" + Session["userid"].ToString() + ")", " cnt_firstname");
            if (dtActivelist.Rows.Count > 0)
            {
                btnSave.Visible = true;
                for (int i = 0; i < dtActivelist.Rows.Count; i++)
                {
                    Activity = null;
                    FinalActivityName = null;
                    string ParntID = null;
                    if (dtActivelist.Rows[i][0] == DBNull.Value)
                        ParntID = "0";
                    else
                        ParntID = dtActivelist.Rows[i][0].ToString();
                    Activity = oDbEngine.getLastActivity(ParntID, "", 0);
                    string[] ActName = Activity.Split(',');
                    for (int k = ActName.Length - 1; k >= 0; k--)
                    {
                        if (FinalActivityName == null || FinalActivityName == "")
                            FinalActivityName = ActName[k];
                        else
                            FinalActivityName += " - >" + ActName[k];
                    }
                    DataRow drActivity = dtActivelist_New.NewRow();
                    drActivity[0] = dtActivelist.Rows[i][0].ToString();
                    drActivity[1] = dtActivelist.Rows[i][1].ToString();
                    drActivity[2] = dtActivelist.Rows[i][2].ToString(); 
                    //drActivity[3] = FinalActivityName;
                    drActivity[3] = dtActivelist.Rows[i][3].ToString();
                    drActivity[4] = dtActivelist.Rows[i][4].ToString();
                    drActivity[5] = dtActivelist.Rows[i][5].ToString();
                    dtActivelist_New.Rows.Add(drActivity);
                }


            }
            else
                btnSave.Visible = false;
            ViewState["dtActivelist"] = dtActivelist_New;
            grdActive2.DataSource = dtActivelist_New;
            grdActive2.DataBind();

        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            //int NoofRowsAffect = 0;
            //DataTable DtInactive = new DataTable();
            //DtInactive.Columns.Add(new DataColumn("LogINID", typeof(String))); //0
            //foreach (GridViewRow row in grdActive2.Rows)
            //{
            //    Label lblLogINID = (Label)row.FindControl("lblLogINID");
            //    CheckBox ChkDelivery = (CheckBox)row.FindControl("ChkDelivery");
            //    if (ChkDelivery.Checked == true)
            //    {
            //        DataRow drInactive = DtInactive.NewRow();
            //        drInactive[0] = lblLogINID.Text;
            //        DtInactive.Rows.Add(drInactive);
            //    }
            //}
            //string tabledata = oDbConverter.ConvertDataTableToXML(DtInactive);
            ////String conn = ConfigurationManager.AppSettings["DBConnectionDefault"];
            ////using (SqlConnection con = new SqlConnection(conn))
            ////{
            ////    con.Open();
            ////    using (SqlCommand com = new SqlCommand("UpdateInactiveFromActive", con))
            ////    {
            ////        com.CommandType = CommandType.StoredProcedure;
            ////        com.Parameters.AddWithValue("@clientPayoutData", tabledata);
            ////        com.Parameters.AddWithValue("@CreateUser", Session["userid"].ToString());
            ////        NoofRowsAffect = com.ExecuteNonQuery();
            ////    }
            ////}

            //NoofRowsAffect = oUtilities.UpdateInactiveFromActive(tabledata, Session["userid"].ToString());
            //if (NoofRowsAffect > 0)
            //{
            //    ScriptManager.RegisterStartupScript(this, GetType(), "JJ", "alert('Reset Successfully !!');", true);
            //}
            //ShowActiveList();
        }

        protected void grdActive2_Sorting(object sender, GridViewSortEventArgs e)
        {
            string sortExpression = e.SortExpression;
            if (GridViewSortDirection == SortDirection.Ascending)
            {
                GridViewSortDirection = SortDirection.Descending;
                SortGridView(sortExpression, " DESC");
            }
            else
            {
                GridViewSortDirection = SortDirection.Ascending;
                SortGridView(sortExpression, " ASC");
            }
        }
        public SortDirection GridViewSortDirection
        {
            get
            {
                if (ViewState["sortDirection"] == null)
                    ViewState["sortDirection"] = SortDirection.Ascending;
                return (SortDirection)ViewState["sortDirection"];
            }
            set { ViewState["sortDirection"] = value; }

        }
        private void SortGridView(string sortExpression, string direction)
        {
            DataTable dtSorting = (DataTable)ViewState["dtActivelist"];
            DataView dv = new DataView(dtSorting);
            dv.Sort = sortExpression + direction;
            grdActive2.DataSource = dv;
            grdActive2.DataBind();
        }
        protected void btnMessage_Click(object sender, EventArgs e)
        {
            string vDBName = ConfigurationManager.AppSettings["DBName"].ToString();
          //string vDBName = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            int NoofRowsAffect = 0;
            DataTable DtMessage = new DataTable();
            DtMessage.Columns.Add(new DataColumn("LogINID", typeof(int))); //0
            DtMessage.Columns.Add(new DataColumn("TextMessage", typeof(String))); //1

            for (int i = 0; i < grdActive2.VisibleRowCount; i++)
            {
                ASPxCheckBox ChkDelivery = (ASPxCheckBox)grdActive2.FindRowCellTemplateControl(i, null, "ChkDeliveryMsg") as ASPxCheckBox;

                if (ChkDelivery.Checked == true)
                {
                    DataRow drMessage = DtMessage.NewRow();
                    drMessage[0] = grdActive2.GetRowValues(i, "user_id").ToString();
                    drMessage[1] = txtMessage.Text;
                    DtMessage.Rows.Add(drMessage);
                }
            }

            //foreach (GridViewRow row in grdActive2.VisibleRow)
            //{
            //    Label lblID = (Label)row.FindControl("lblID");
            //    CheckBox ChkDelivery = (CheckBox)row.FindControl("ChkDeliveryMsg");
            //    if (ChkDelivery.Checked == true)
            //    {
            //        DataRow drMessage = DtMessage.NewRow();
            //        drMessage[0] = lblID.Text;
            //        drMessage[1] = txtMessage.Text;
            //        DtMessage.Rows.Add(drMessage);
            //    }
            //}
            string tabledata = oDbConverter.ConvertDataTableToXML(DtMessage);
            //String conn = ConfigurationManager.AppSettings["DBConnectionDefault"];

            //using (SqlConnection con = new SqlConnection(conn))
            //{
            //    con.Open();
            //    using (SqlCommand com = new SqlCommand("UpdateMessage", con))
            //    {
            //        com.CommandType = CommandType.StoredProcedure;
            //        com.Parameters.AddWithValue("@clientPayoutData", tabledata);
            //        com.Parameters.AddWithValue("@CreateUser", Session["userid"].ToString());
            //        NoofRowsAffect = com.ExecuteNonQuery();
            //    }
            //}
            NoofRowsAffect = oUtilities.UpdateMessage(vDBName, tabledata, Session["userid"].ToString());
            if (NoofRowsAffect > 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "JJ", "alert('Message Send Successfully !!');", true);
            }
            ShowActiveList();
        }
        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            ShowActiveList();
        }

        protected void chkAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < grdActive2.VisibleRowCount; i++)
            {
                ASPxCheckBox ChkDelivery = (ASPxCheckBox)grdActive2.FindRowCellTemplateControl(i, null, "ChkDeliveryMsg") as ASPxCheckBox;
                ChkDelivery.Checked = true;
            }
        }
        protected void UnChkAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < grdActive2.VisibleRowCount; i++)
            {
                ASPxCheckBox ChkDelivery = (ASPxCheckBox)grdActive2.FindRowCellTemplateControl(i, null, "ChkDeliveryMsg") as ASPxCheckBox;
                ChkDelivery.Checked = false;
            }
        }
        protected void grdActive2_RowCreated(object sender, GridViewRowEventArgs e)
        {
            DataTable dtCashBankBook = (DataTable)ViewState["dtActivelist"];
            string rowID = String.Empty;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                rowID = "row" + e.Row.RowIndex;
                e.Row.Attributes.Add("id", "row" + e.Row.RowIndex);
                e.Row.Attributes.Add("onclick", "ChangeRowColor(" + "'" + rowID + "','" + dtCashBankBook.Rows.Count + "'" + ")");
            }
        }
    }
}