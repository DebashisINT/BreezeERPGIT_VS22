using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using DevExpress.Web;
using BusinessLogicLayer;

namespace ERP.OMS.Reports
{
    public partial class Reports_frmReport_EmailLogReports : System.Web.UI.Page
    {

        public string pageAccess = "";
        BusinessLogicLayer.Converter OConvert = new BusinessLogicLayer.Converter();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        BusinessLogicLayer.Reports rep = new BusinessLogicLayer.Reports();
        static string WhereStatus = "";
        string Rowid = "";
        string SegmentID = "";
        protected void Page_Init(object sender, EventArgs e)
        {
            LeadGridDataSource.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
        }
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

            SegmentID = HttpContext.Current.Session["userlastsegment"].ToString();
            //string[,] grpsegment = oDBEngine.GetFieldValue("tbl_master_userGroup", "top 1 grp_segmentid", "grp_id in (" + usergroup.ToString() + ")", 1);
            if (SegmentID == "1")
            {
                Show.Visible = true;
                td_check.Visible = true;
            }
            else
            {
                Show.Visible = false;
                td_check.Visible = false;
            }
            txtName.Attributes.Add("onkeyup", "CallAjax(this,'GetNameForEmail',event)");
            txtFromDate.EditFormatString = OConvert.GetDateFormat("Date");
            txtToDate.EditFormatString = OConvert.GetDateFormat("Date");
            if (HttpContext.Current.Session["userid"] == null)
            {
               // Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            Rowid = hdnfrequency1.Value;
            if (!IsPostBack)
            {

                //LeadGrid.VisibleColumns[9].Visible = false;
                LeadGrid.Columns["emails_segment"].Visible = false;
                txtFromDate.Value = Convert.ToDateTime(DateTime.Today);
                txtToDate.Value = Convert.ToDateTime(DateTime.Today);
                ClientScript.RegisterStartupScript(typeof(Page), "Today", "<script>PageLoad();</script>");
                // LeadGrid.VisibleColumns[8].Visible = false;
            }

            showgrid();
         //   //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");


        }
        protected void cbAll_Init(object sender, EventArgs e)
        {

        }
        private void showgrid()
        {


            //CountStatus();
            string types = rbUser.Value.ToString();
            string ContactID = txtName_hidden.Value.ToString();

            ///////////Commented Out Previous Code//////////////////
            //string startdate = txtFromDate.Date.Month.ToString() + "/" + txtFromDate.Date.Day.ToString() + "/" + txtFromDate.Date.Year.ToString() + " 00:01 AM";
            //string Enddate = txtToDate.Date.Month.ToString() + "/" + txtToDate.Date.Day.ToString() + "/" + txtToDate.Date.Year.ToString() + " 11:59 PM";
            //////////////////////////////////////////////////////

            ////////////////////New Code//////////////////////////
            DateTime vStartDate = txtFromDate.Date + new TimeSpan(0, 1, 0);
            DateTime vEndDate = txtToDate.Date + new TimeSpan(23, 59, 0);
            //////////////////////////////////////////////////////

            DataSet dsEmail = new DataSet();
            //String conn = ConfigurationSettings.AppSettings["DBConnectionDefault"];
            //SqlConnection con = new SqlConnection(conn);
            //SqlCommand cmd3 = new SqlCommand("Fetch_TransEmail", con);
            //cmd3.CommandType = CommandType.StoredProcedure;
            //cmd3.Parameters.AddWithValue("@FromDate", startdate);
            //cmd3.Parameters.AddWithValue("@ToDate", Enddate);
            //cmd3.Parameters.AddWithValue("@Segment", HttpContext.Current.Session["userlastsegment"].ToString());
            //cmd3.Parameters.AddWithValue("@Status", WhereStatus);
            //cmd3.Parameters.AddWithValue("@ContactID", ContactID);

            //if (SegmentID == "1")
            //{
            //    if ((SegmentID == "1") && (chk_segment.Checked == true))
            //    {
            //        cmd3.Parameters.AddWithValue("@chk", "chk");
            //        //LeadGrid.VisibleColumns[10].Visible = true;

            //    }
            //    else
            //    {
            //        cmd3.Parameters.AddWithValue("@chk", "");

            //        //LeadGrid.VisibleColumns[10].Visible = false;
            //    }
            //    cmd3.Parameters.AddWithValue("@systmmails", rdbsubscription.SelectedItem.Value);

            //}
            //else
            //{
            //    cmd3.Parameters.AddWithValue("@chk", "");
            //    cmd3.Parameters.AddWithValue("@systmmails", "A");
            //}
            //cmd3.CommandTimeout = 0;
            //SqlDataAdapter Adap = new SqlDataAdapter();
            //Adap.SelectCommand = cmd3;
            //Adap.Fill(dsEmail);
            //cmd3.Dispose();
            //con.Dispose();
            //GC.Collect();

            string chk = "";
            string systmmails = "";
            if (SegmentID == "1")
            {
                if ((SegmentID == "1") && (chk_segment.Checked == true))
                {
                    chk = "chk";
                    //LeadGrid.VisibleColumns[10].Visible = true;

                }
                else
                {
                    chk = "";

                    //LeadGrid.VisibleColumns[10].Visible = false;
                }
                systmmails = rdbsubscription.SelectedItem.Value.ToString();

            }
            else
            {
                chk = "";
                systmmails = "A";
            }
            ///////////Commented Out Previous Code//////////////////
            //dsEmail = rep.Fetch_TransEmail(Convert.ToDateTime(startdate), Convert.ToDateTime(Enddate), HttpContext.Current.Session["userlastsegment"].ToString(), WhereStatus,
            //    ContactID, chk, systmmails);
            //////////////////////////////////////////////////////

            ////////////////////New Code//////////////////////////
            dsEmail = rep.Fetch_TransEmail(vStartDate, vEndDate, HttpContext.Current.Session["userlastsegment"].ToString(), WhereStatus,
                ContactID, chk, systmmails);
            //////////////////////////////////////////////////////

            LeadGrid.DataSource = dsEmail.Tables[0].DefaultView;
            LeadGrid.DataBind();
            if (dsEmail.Tables[0].Rows.Count > 0)
            {
                if ((SegmentID == "1") && (chk_segment.Checked == true))
                {
                    LeadGrid.Columns["emails_segment"].Visible = true;
                }
                else
                {

                    LeadGrid.Columns["emails_segment"].Visible = false;
                }
            }

            lblDelivered.Text = dsEmail.Tables[1].Rows[0]["Delivered"].ToString();
            lblPending.Text = dsEmail.Tables[2].Rows[0]["Pending"].ToString();
            lblBounced.Text = dsEmail.Tables[3].Rows[0]["Bounced"].ToString();
            lblFailed.Text = dsEmail.Tables[4].Rows[0]["Failed"].ToString();
            if (types == "A")
            {
                //tdName.Visible = false;
                //tdName2.Visible = false;
                // ClientScript.RegisterStartupScript(typeof(Page), "Today", "<script>ShowEmployeeFilterForm('A');</script>");
                //ClientScript.RegisterStartupScript(typeof(Page), "Today", "<script>ShowEmployeeFilterForm('A');</script>");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "CallPro1", "ShowEmployeeFilterForm('A');", true);
            }
            else
            {
                //tdName.Visible = true;
                //tdName2.Visible = true;
                //ClientScript.RegisterStartupScript(typeof(Page), "Today", "<script>ShowEmployeeFilterForm('S');</script>");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "CallPro2", "ShowEmployeeFilterForm('S');", true);
            }

         //   ScriptManager.RegisterStartupScript(this, this.GetType(), "CallPro3", "height();", true);

        }
        protected void btnSearch(object sender, EventArgs e)
        {
            LeadGrid.Settings.ShowFilterRow = true;
        }

        protected void LeadGrid_CustomCallback1(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            LeadGrid.ClearSort();

            if (e.Parameters == "s")
            {
                LeadGrid.Settings.ShowFilterRow = true;
            }
            else if (e.Parameters == "All")
            {
                WhereStatus = "";
                LeadGrid.FilterExpression = string.Empty;
                showgrid();
            }
            else if (e.Parameters == "Delivered")
            {

                WhereStatus = "D";
                showgrid();
            }
            else if (e.Parameters == "Pending")
            {
                WhereStatus = "P";
                showgrid();
            }
            else if (e.Parameters == "Bounced")
            {
                WhereStatus = "B";
                showgrid();

            }
            else if (e.Parameters == "Failed")
            {
                //WhereStatus = "F";
                WhereStatus = "X,F";
                showgrid();
            }

        }


        protected void btnExport_Click(object sender, EventArgs e)
        {
            exporter.WriteXlsToResponse();
        }

        protected void LeadGrid_CustomJSProperties(object sender, DevExpress.Web.ASPxGridViewClientJSPropertiesEventArgs e)
        {
            e.Properties["cpHeight"] = "a";
        }
        protected void btnGetReport_Click(object sender, EventArgs e)
        {
            WhereStatus = "";
            showgrid();
        }
        protected void btnresend_Click(object sender, EventArgs e)
        {
            if (Rowid == "")
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "JScript18", "<script language='javascript'>Page_Load();</script>");
                Page.ClientScript.RegisterStartupScript(GetType(), "JScript15", "<script language='javascript'>alert('Please Select At least One Item');</script>");
            }
            else
            {
                oDBEngine.SetFieldValue("Trans_EmailRecipients", "EmailRecipients_Status='P',EmailRecipients_AttemptNumber='" + System.DBNull.Value + "',EmailRecipients_SentDateTime='" + System.DBNull.Value + "',EmailRecipients_LastUpdateDateTime='" + System.DBNull.Value + "'", "EmailRecipients_MainID in ( " + Rowid + ")");
                showgrid();
            }
        }
        protected void LeadGrid_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
        {
            int rowindex = e.VisibleIndex;
            //if (Session["LastCompany"].ToString() == null)
            //    e.Enabled = false;
            string date = LeadGrid.GetRowValues(rowindex, "Status").ToString();
            if (date == "Delivered")
            {
                //GridViewCommandColumn ccol = (GridViewCommandColumn)(LeadGrid.Columns[10]);

                e.Enabled = false;


            }



        }
        //protected void LeadGrid_CustomButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
        //{
        //    e.Enabled = false;

        //}
        protected void LeadGrid_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            //string str = Request.QueryString["id"].ToString();
            string Code = e.Values[0].ToString();
            string[,] Count = new string[0, 0];
            Count = oDBEngine.GetFieldValue("Trans_EmailAttachment", "EmailAttachment_MainID", "EmailAttachment_MainID='" + Code + "'", 1);
            if (Count[0, 0] != "n")
            {
                oDBEngine.DeleteValue("Trans_EmailAttachment", "EmailAttachment_MainID='" + Code + "'");
            }
            oDBEngine.DeleteValue("Trans_EmailRecipients", "EmailRecipients_mainid='" + Code + "'");
            oDBEngine.DeleteValue("Trans_Emails", "Emails_id='" + Code + "'");
            e.Cancel = true;
            showgrid();
            //SqlConnection lcon = new SqlConnection(ConfigurationSettings.AppSettings["DBConnectionDefault"]);
            //SqlCommand lcmd = new SqlCommand("DeleteCustomAccount", lcon);
            //lcmd.CommandType = CommandType.StoredProcedure;
            //lcmd.Parameters.Add("@MainAcountCode", SqlDbType.VarChar, 15).Value = str;
            //lcmd.Parameters.Add("@SubAcountCode", SqlDbType.VarChar, 15).Value = Code;
            //lcon.Open();
            //int NoofRowsAffected = lcmd.ExecuteNonQuery();
            //lcon.Close();
            //if (NoofRowsAffected <= 0)
            //{
            //    Error = "b";
            //}
            //string types = rbUser.Value.ToString();
            //string ContactID = txtName_hidden.Value.ToString();
            //string startdate = txtFromDate.Date.Month.ToString() + "/" + txtFromDate.Date.Day.ToString() + "/" + txtFromDate.Date.Year.ToString() + " 00:01 AM";
            //string Enddate = txtToDate.Date.Month.ToString() + "/" + txtToDate.Date.Day.ToString() + "/" + txtToDate.Date.Year.ToString() + " 11:59 PM";

            //DataSet dsEmail = new DataSet();
            //String conn = ConfigurationSettings.AppSettings["DBConnectionDefault"];
            //SqlConnection con = new SqlConnection(conn);
            //SqlCommand cmd3 = new SqlCommand("Fetch_TransEmail", con);
            //cmd3.CommandType = CommandType.StoredProcedure;
            //cmd3.Parameters.AddWithValue("@FromDate", startdate);
            //cmd3.Parameters.AddWithValue("@ToDate", Enddate);
            //cmd3.Parameters.AddWithValue("@Segment", HttpContext.Current.Session["userlastsegment"].ToString());
            //cmd3.Parameters.AddWithValue("@Status", WhereStatus);
            //cmd3.Parameters.AddWithValue("@ContactID", ContactID);
            //if (SegmentID == "1")
            //{
            //    if ((SegmentID == "1") && (chk_segment.Checked == true))
            //    {
            //        cmd3.Parameters.AddWithValue("@chk", "chk");
            //        //LeadGrid.VisibleColumns[10].Visible = true;

            //    }
            //    else
            //    {
            //        cmd3.Parameters.AddWithValue("@chk", "");
            //        //LeadGrid.VisibleColumns[10].Visible = false;
            //    }
            //}
            //else
            //{
            //    cmd3.Parameters.AddWithValue("@chk", "");
            //}
            //cmd3.CommandTimeout = 0;
            //SqlDataAdapter Adap = new SqlDataAdapter();
            //Adap.SelectCommand = cmd3;
            //Adap.Fill(dsEmail);
            //cmd3.Dispose();
            //con.Dispose();
            //GC.Collect();
            //LeadGrid.DataSource = dsEmail.Tables[0].DefaultView;
            //LeadGrid.DataBind();

            e.Cancel = true;
        }
        ////protected void LeadGrid_CustomButtonCallback(object sender, ASPxGridViewCustomButtonCallbackEventArgs e)
        ////{
        ////    if (e.ButtonID != "cbFilter") return;
        ////    ASPxGridView grid = sender as ASPxGridView;
        ////    grid.FilterExpression = "[Title] like '%Sales%'";
        ////    //LeadGrid.c
        ////}



        //protected void LeadGrid_RowCommand(object sender, ASPxGridViewRowCommandEventArgs e)
        //{
        //    //if (e
        //    string code1 = e.KeyValue.ToString();

        //}
    }


}