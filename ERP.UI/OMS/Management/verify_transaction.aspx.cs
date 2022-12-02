using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using BusinessLogicLayer;

namespace ERP.OMS.Management
{

    public partial class management_verify_transaction : System.Web.UI.Page
    {
        //Reports_CMNetPosition DeliveryProcessing.aspx//sp_export_Annexure3
        // SqlConnection con = new SqlConnection(ConfigurationSettings.AppSettings["DBConnectionDefault"]);
        BusinessLogicLayer.Converter objConverter = new BusinessLogicLayer.Converter();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        ExcelFile objExcel = new ExcelFile();
        static DataSet ds = new DataSet();
        int pageindex = 0;
        int pagecount = 0;
        int pageSize = 0;
        public string dp = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            dp = Request.QueryString["dp"].ToString();
            if (!ClientScript.IsStartupScriptRegistered("Today"))
                ClientScript.RegisterStartupScript(typeof(Page), "Today", "<script>PageLoad();</script>");
            ScriptManager.RegisterStartupScript(this, this.GetType(), "hide2", "height();", true);
            //txtbranch.Attributes.Add("onkeyup", "CallAjax(this,'BranchName',event)");
            //TradechangeCLD.aspx
            txtuser.Attributes.Add("onkeyup", "CallAjax(this,'SearchByUserID',event)");
            if (!IsPostBack)
            {
                //Search by userName & InternalID
                //BranchName SearchByBranchName

            }
        }
        public void bindverify()
        {
            if (dtexec.Value == null)
            {
                ScriptManager.RegisterStartupScript(HttpContext.Current.Handler as Page, HttpContext.Current.GetType(), "di", "alert('Transaction date should not be blank');", true);

            }
            else
            {
                ds.Reset();
                string date = dtexec.Text.Split('-')[2] + "-" + dtexec.Text.Split('-')[1] + "-" + dtexec.Text.Split('-')[0];



                string selectionmode = "";

                if (rduser.SelectedItem.Text == "All")
                {
                    selectionmode = "All";
                }
                else
                {
                    selectionmode = hideuser.Value;
                }
                //}

                //using (SqlConnection con = new SqlConnection(ConfigurationSettings.AppSettings["DBConnectionDefault"]))MULTI
                using (SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"])))
                {
                    using (SqlCommand com = new SqlCommand("sp_fetch_verify_transaction", con))
                    {
                        com.CommandType = CommandType.StoredProcedure;
                        SqlParameter param = com.Parameters.AddWithValue("@date", date);
                        com.Parameters.AddWithValue("@transactionmode", "User");
                        com.Parameters.AddWithValue("@selectionmode", selectionmode);
                        com.Parameters.AddWithValue("@dp", dp);
                        com.Parameters.AddWithValue("@branchid", HttpContext.Current.Session["userbranchHierarchy"].ToString());
                        using (SqlDataAdapter ad = new SqlDataAdapter(com))
                        {
                            com.CommandTimeout = 0;
                            ad.Fill(ds);
                        }
                    }
                }
                ViewState["bind"] = ds;
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "hide2", "height();", true);
                    btnsave.Visible = true;
                    Panel1.Visible = true;
                    ddlExport.Visible = true;
                }
                else
                {
                    btnsave.Visible = false;
                    Panel1.Visible = false;
                    ddlExport.Visible = false;
                    ScriptManager.RegisterStartupScript(HttpContext.Current.Handler as Page, HttpContext.Current.GetType(), "di", "alert('No result found');", true);

                }
                pageSize = 5;
                pagecount = ds.Tables[0].Rows.Count / pageSize + 1;
                TotalPages.Value = pagecount.ToString();
                offlineGrid.PageIndex = pageindex;
                CurrentPage.Value = pageindex.ToString();
                if (pageindex <= 0)
                {
                    pageindex = 0;


                    ScriptManager.RegisterStartupScript(this, this.GetType(), "hide3", "DisableBranch('P');", true);
                }
                if (pageindex >= int.Parse(TotalPages.Value.ToString()))
                {
                    pageindex = int.Parse(TotalPages.Value.ToString()) - 1;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "hide3", "DisableBranch('N');", true);
                }
                if (pageindex >= (int.Parse(TotalPages.Value.ToString()) - 1))
                {
                    pageindex = int.Parse(TotalPages.Value.ToString()) - 1;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "hide3", "DisableBranch('N');", true);
                }
                DataView dv = new DataView(ds.Tables[0]);
                //dv.Sort = sortExpressionbranch + directionbranch;
                offlineGrid.DataSource = dv;
                offlineGrid.DataBind();

            }




        }
        protected void btnshow_Click(object sender, EventArgs e)
        {
            bindverify();
        }
        protected void btnsave_Click(object sender, EventArgs e)
        {
            try
            {
                //ddlExport.SelectedItem.Value = "Ex";
                RememberOldValues();
                string s = "";
                ArrayList categoryIDList = new ArrayList();
                categoryIDList = (ArrayList)Session["CHECKED_ITEMS"];
                if (categoryIDList == null)
                {
                    s = "null";
                }
                else
                {
                    for (int i = 0; i < categoryIDList.Count; i++)
                    {
                        //s = s + ",'" + ((Label)(offlineGrid.Rows[i].Cells[0].FindControl("lblid"))).Text.Trim() + "'";   
                        s = s + ",'" + categoryIDList[i].ToString() + "'";


                    }
                }

                //using (SqlConnection con = new SqlConnection(ConfigurationSettings.AppSettings["DBConnectionDefault"]))MULTI
                using (SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"])))
                {
                    using (SqlCommand com = new SqlCommand("sp_insert_verification_log", con))
                    {
                        com.CommandType = CommandType.StoredProcedure;
                        com.Parameters.Add("@id", SqlDbType.VarChar, 5000).Value = s;
                        com.Parameters.Add("@userid", SqlDbType.VarChar, 500).Value = Session["userid"].ToString();
                        com.Parameters.Add("@dp", SqlDbType.VarChar, 5000).Value = dp;
                        com.ExecuteNonQuery();
                        ScriptManager.RegisterStartupScript(HttpContext.Current.Handler as Page, HttpContext.Current.GetType(), "di", "alert('Updated successfully');", true);
                        Session.Remove("CHECKED_ITEMS");
                    }
                }
            }
            catch
            {
                ScriptManager.RegisterStartupScript(HttpContext.Current.Handler as Page, HttpContext.Current.GetType(), "di", "alert('Can not update');", true);
                Session.Remove("CHECKED_ITEMS");
            }

            bindverify();
        }
        protected void btncan_Click(object sender, EventArgs e)
        {

        }

        protected void offlineGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "reject" && dp == "CDSL")
            {

                DataTable dt = oDBEngine.GetDataTable("Trans_CdslOffline", "CdslOffline_Rejected", "CdslOffline_ID='" + e.CommandArgument + "'");

                ScriptManager.RegisterStartupScript(this, GetType(), "cbclick", "popup('" + e.CommandArgument + "','" + dt.Rows[0][0].ToString() + "')", true);
            }
            else if (e.CommandName == "reject" && dp == "NSDL")
            {

                DataTable dt = oDBEngine.GetDataTable("Trans_nsdlOffline", "nsdlOffline_Rejected", "NsdlOffline_ID='" + e.CommandArgument + "'");

                ScriptManager.RegisterStartupScript(this, GetType(), "cbclick", "popup('" + e.CommandArgument + "','" + dt.Rows[0][0].ToString() + "')", true);
            }

        }
        protected void NavigationLink_Click1(Object sender, CommandEventArgs e)
        {
            RememberOldValues();
            switch (e.CommandName)
            {
                case "First":
                    pageindex = 0;
                    break;
                case "Next":
                    pageindex = int.Parse(CurrentPage.Value) + 1;
                    break;
                case "Prev":
                    if (int.Parse(CurrentPage.Value) == 0)
                        pageindex = 0;
                    else
                        pageindex = int.Parse(CurrentPage.Value) - 1;
                    break;
                case "Last":
                    pageindex = int.Parse(TotalPages.Value);
                    break;
                default:
                    pageindex = int.Parse(e.CommandName.ToString());
                    break;
            }

            bindverify();
            RePopulateValues();

        }


        private void SortGridView_Branch(string sortExpressionbranch, string directionbranch)
        {


            DataView dv1 = new DataView(ds.Tables[0]);
            //dv1.Sort = sortExpressionbranch + directionbranch;
            offlineGrid.DataSource = dv1;
            offlineGrid.DataBind();
        }
        public string sortExpressionbranch
        {
            get
            {
                if (this.ViewState["sortExpressionbranch"] == null)
                    return "SlipNumber";
                //return null;

                else
                    return Convert.ToString(this.ViewState["sortExpressionbranch"]);

            }
            set
            {
                this.ViewState["sortExpressionbranch"] = value;

            }
        }

        public string directionbranch
        {
            get
            {
                if (this.ViewState["directionbranch"] == null)
                    return " ASC";

                else
                    return Convert.ToString(this.ViewState["directionbranch"]);

            }
            set
            {
                this.ViewState["directionbranch"] = value;

            }

        }



        protected void offlineGrid_Sorting(object sender, GridViewSortEventArgs e)
        {
            sortExpressionbranch = e.SortExpression;
            if (GridViewSortDirection == SortDirection.Ascending)
            {
                directionbranch = " DESC";
                GridViewSortDirection = SortDirection.Descending;

                SortGridView_Branch(sortExpressionbranch, directionbranch);
            }
            else
            {
                directionbranch = " ASC";
                GridViewSortDirection = SortDirection.Ascending;

                SortGridView_Branch(sortExpressionbranch, directionbranch);
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

        private void RememberOldValues()
        {
            ArrayList categoryIDList = new ArrayList();
            Int64 index = -1;
            foreach (GridViewRow row in offlineGrid.Rows)
            {
                index = (Int64)offlineGrid.DataKeys[row.RowIndex].Value;
                bool result = ((CheckBox)row.FindControl("CheckBox3")).Checked;

                // Check in the Session
                if (Session["CHECKED_ITEMS"] != null)
                    categoryIDList = (ArrayList)Session["CHECKED_ITEMS"];
                if (result)
                {
                    if (!categoryIDList.Contains(index))
                    {
                        categoryIDList.Add(index);

                    }
                }
                else
                    categoryIDList.Remove(index);
            }
            if (categoryIDList != null && categoryIDList.Count > 0)
                Session["CHECKED_ITEMS"] = categoryIDList;
        }

        private void RePopulateValues()
        {
            ArrayList categoryIDList = (ArrayList)Session["CHECKED_ITEMS"];
            if (categoryIDList != null && categoryIDList.Count > 0)
            {
                foreach (GridViewRow row in offlineGrid.Rows)
                {
                    Int64 index = (Int64)offlineGrid.DataKeys[row.RowIndex].Value;
                    if (categoryIDList.Contains(index))
                    {
                        CheckBox myCheckBox = (CheckBox)row.FindControl("CheckBox3");
                        myCheckBox.Checked = true;
                    }
                }
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            //if (hideid.Value == "")
            //{
            RememberOldValues();
            bindverify();
            RePopulateValues();
            hideid.Value = "";
            //}
            //else
            //{
            //    DataTable d = new DataTable();
            //    if (dp == "CDSL")
            //    {
            //        d = oDBEngine.GetDataTable("Trans_cdslOffline", "CdslOffline_VerifyUser,CdslOffline_RejectUser", " CdslOffline_ID=" + hideid.Value.ToString().Split(',')[1] + "");
            //        hideid.Value = d.Rows[0][0].ToString();
            //    }
            //    else
            //    {
            //        d = oDBEngine.GetDataTable("Trans_nsdlOffline", "NsdlOffline_VerifiedBy,NsdlOffline_RejectUser", " NsdlOffline_ID=" + hideid.Value.ToString().Split(',')[1] + "");
            //        hideid.Value = d.Rows[0][0].ToString();
            //    }           
            //}

        }
        protected void offlineGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (DataBinder.Eval(e.Row.DataItem, "verifyuser") == DBNull.Value)
                {
                }
                else
                {
                    int verified = (int)DataBinder.Eval(e.Row.DataItem, "verifyuser");
                    e.Row.ForeColor = System.Drawing.Color.Crimson;
                    e.Row.Font.Italic = true;
                    e.Row.BackColor = System.Drawing.Color.Lavender;
                    CheckBox c = (CheckBox)e.Row.Cells[6].FindControl("CheckBox3");
                    c.Checked = true;
                }
                if (DataBinder.Eval(e.Row.DataItem, "Rejected") == DBNull.Value)
                {
                    //string rowID = "row" + e.Row.RowIndex;
                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "JSWE1", "aaa('" + rowID + "');", true);

                    //e.Row.ForeColor = System.Drawing.Color.White;
                    //e.Row.Font.Italic = true;
                    //e.Row.BackColor = System.Drawing.Color.White;
                    //Button b = (Button)e.Row.Cells[6].FindControl("btnreject");
                    //b.BackColor = System.Drawing.Color.SteelBlue;
                }
                else
                {
                    string rejected = (string)DataBinder.Eval(e.Row.DataItem, "Rejected");
                    if (rejected != null)
                    {
                        e.Row.ForeColor = System.Drawing.Color.Crimson;
                        e.Row.Font.Italic = true;
                        e.Row.BackColor = System.Drawing.Color.Bisque;
                        Button b = (Button)e.Row.Cells[6].FindControl("btnreject");
                        b.BackColor = System.Drawing.Color.DarkRed;
                        b.ForeColor = System.Drawing.Color.White;
                        CheckBox c = (CheckBox)e.Row.Cells[6].FindControl("CheckBox3");
                        c.Checked = false;
                        c.Enabled = false;
                    }
                    else
                    {

                    }
                }
            }

        }
        protected void Button2_Click(object sender, EventArgs e)
        {
            SqlConnection con = null;
            SqlCommand com = null;
            try
            {
                //con = new SqlConnection(ConfigurationSettings.AppSettings["DBConnectionDefault"]);MULTI
                con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));


                if (con.State != ConnectionState.Open)
                    con.Open();

                //SqlCommand com = new SqlCommand("update Trans_CdslOffline set CdslOffline_Rejected=1,CdslOffline_RejectUser='" + Session["userid"].ToString() + "',CdslOffline_RejectDateTime=getdate(),CdslOffline_RejectReason='" + txtreject.Text + "',CdslOffline_VerifyUser ='" + Session["userid"].ToString() + "',CdslOffline_VerifiyDateTime =getdate() where CdslOffline_ID='" + Request.QueryString["id"].ToString() + "'", con);
                if (dp == "CDSL")
                {
                    com = new SqlCommand("update Trans_CdslOffline set CdslOffline_Rejected=null,CdslOffline_RejectUser= null ,CdslOffline_RejectDateTime=null,CdslOffline_RejectReason = null where CdslOffline_ID='" + reject.Value + "'", con);
                }
                else
                {
                    com = new SqlCommand("update Trans_nsdlOffline set NsdlOffline_Rejected=null,NsdlOffline_RejectUser= null ,NsdlOffline_RejectDateTime=null,NsdlOffline_RejectReason= null where nsdlOffline_ID='" + reject.Value + "'", con);
                }
                com.CommandType = CommandType.Text;
                com.ExecuteNonQuery();
                ScriptManager.RegisterStartupScript(HttpContext.Current.Handler as Page, HttpContext.Current.GetType(), "di", "alert('Updated successfully');", true);
                //ScriptManager.RegisterStartupScript(this, GetType(), "cbclick1", "changecolor('" + reject.Value + "')", true);

                //bindverify();
            }
            catch
            {
                ScriptManager.RegisterStartupScript(HttpContext.Current.Handler as Page, HttpContext.Current.GetType(), "di", "alert('Can not update');", true);

            }
            finally
            {
                if (com != null)
                    com.Dispose();
                if (con != null)
                    con.Dispose();
            }
        }
        protected void offlineGrid_RowCreated(object sender, GridViewRowEventArgs e)
        {
            //string rowID = String.Empty;
            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
            //    rowID = "row" + e.Row.RowIndex;
            //    e.Row.Attributes.Add("id", "row" + e.Row.RowIndex);
            //    e.Row.Attributes.Add("onclick", "ChangeRowColor(" + "'" + rowID + "','" + ds.Tables[0].Rows.Count + "'" + ")");
            //}

        }

        protected void ddlExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //transDs.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\management\\verifytransaction.xsd");
                //DataSet ds = (DataSet)ViewState["bind"];
                //ds.WriteXmlSchema("c:\\verifytransaction.xsd");
                ReportDocument verifytransaction = new ReportDocument();
                //string[] connPath = ConfigurationSettings.AppSettings["DBConnectionDefault"].Split(';');MULTI
                string[] connPath = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]).Split(';');
                string path = Server.MapPath("..\\management\\verifytransaction.rpt");
                verifytransaction.Load(path);
                verifytransaction.SetDatabaseLogon(connPath[2].Substring(connPath[2].IndexOf("=")).Trim(), connPath[3].Substring(connPath[3].IndexOf("=")).Trim(), connPath[0].Substring(connPath[0].IndexOf("=")).Trim(), connPath[1].Substring(connPath[1].IndexOf("=")).Trim(), false);
                verifytransaction.SetDataSource(ds.Tables[0]);

                //cdslTransctionReportDocu.SetDataSource(holdingDs);
                //cdslTransctionReportDocu.Subreports[0].SetDataSource(transDs.Tables[1]);
                verifytransaction.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, Response, true, "Verify Transaction");
                ds.Clear();
                ds.Dispose();

                /*
                DataTable CompanyName = oDBEngine.GetDataTable("tbl_master_company", "cmp_Name", " cmp_internalId='" + Session["LastCompany"].ToString() + "'");
                DataTable dtReportHeader = new DataTable();
                dtReportHeader.Columns.Add(new DataColumn("Header", typeof(String))); //0
                DataRow HeaderRow = dtReportHeader.NewRow();
                HeaderRow[0] = CompanyName.Rows[0][0].ToString();
                dtReportHeader.Rows.Add(HeaderRow);
                //DataRow DrRowR1 = dtReportHeader.NewRow();
                //DrRowR1[0] = "Net Position [" + ds1.Tables[7].Rows[0]["segmentname"].ToString() + "]" + ' ' + oconverter.ArrangeDate2(dtFor.Value.ToString());
                //dtReportHeader.Rows.Add(DrRowR1);
                DataRow DrRowR2 = dtReportHeader.NewRow();
                dtReportHeader.Rows.Add(DrRowR2);
                DataRow HeaderRow1 = dtReportHeader.NewRow();
                dtReportHeader.Rows.Add(HeaderRow1);
                DataRow HeaderRow2 = dtReportHeader.NewRow();
                dtReportHeader.Rows.Add(HeaderRow2);

                DataTable dtReportFooter = new DataTable();
                dtReportFooter.Columns.Add(new DataColumn("Footer", typeof(String))); //0
                DataRow FooterRow1 = dtReportFooter.NewRow();
                dtReportFooter.Rows.Add(FooterRow1);
                DataRow FooterRow2 = dtReportFooter.NewRow();
                dtReportFooter.Rows.Add(FooterRow2);
                DataRow FooterRow = dtReportFooter.NewRow();
                FooterRow[0] = "* * *  End Of Report * * *   ";
                dtReportFooter.Rows.Add(FooterRow);
                DataSet s = (DataSet)ViewState["bind"];
                //s=(DataSet)s.Tables[0].Columns.Remove("id");
                if (ddlExport.SelectedItem.Value == "E")
                {
                    objExcel.ExportToExcelforExcel(s.Tables[0], "NET POSITION", "Total", dtReportHeader, dtReportFooter);
                }
                else if (ddlExport.SelectedItem.Value == "P")
                {
                    objExcel.ExportToPDF(s.Tables[0], "NET POSITION", "Total", dtReportHeader, dtReportFooter);
                }
               */
            }
            catch (Exception ex)
            {
            }
        }
        protected void offlineGrid_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

            string id = offlineGrid.DataKeys[e.RowIndex].Value.ToString();
            try
            {
                //using (SqlConnection con = new SqlConnection(ConfigurationSettings.AppSettings["DBConnectionDefault"]))MULTI
                using (SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"])))
                {
                    using (SqlCommand com = new SqlCommand("sp_delete_verification_log", con))
                    {
                        com.CommandType = CommandType.StoredProcedure;
                        SqlParameter param = com.Parameters.AddWithValue("@id", id);
                        com.Parameters.AddWithValue("@dp", dp);
                        //com.Parameters.AddWithValue("@dpid", Session["BenAccountNumber"].ToString());
                        //com.Parameters.AddWithValue("@benaccountnumber", Session["usersegid"].ToString());

                        com.ExecuteNonQuery();

                        bindverify();
                        ScriptManager.RegisterStartupScript(HttpContext.Current.Handler as Page, HttpContext.Current.GetType(), "di", "alert('Deleted successfully');", true);
                    }
                }
            }
            catch
            {

            }

        }

    }
}