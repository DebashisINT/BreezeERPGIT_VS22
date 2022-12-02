using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogicLayer;

namespace ERP.OMS.Management.Others
{
    public partial class management_Others_InterestGeneration : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
    {
        //Converter oconverter = new Converter();
        //DBEngine oDBEngine = new DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        BusinessLogicLayer.Converter oconverter = new BusinessLogicLayer.Converter();
       // BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();


        BusinessLogicLayer.Others oOthers = new BusinessLogicLayer.Others();
        DataSet ds = new DataSet();
        ExcelFile objExcel = new ExcelFile();
        DataTable dtExport = new DataTable();
        string data;
        protected DateTime currentFromDate, currentToDate;
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

            if (HttpContext.Current.Session["userid"] == null)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }

            if (!IsPostBack)
            {
                DateTime Date = oDBEngine.GetDate();
                currentFromDate = Date.AddDays((-1 * Date.Day) + 1);
                currentToDate = Date;
                Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script language='javascript'>Page_Load();</script>");
                date();

            }
            //_____For performing operation without refreshing page___//
            String cbReference = Page.ClientScript.GetCallbackEventReference(this, "arg", "ReceiveServerData", "context");
            String callbackScript = "function CallServer(arg, context){ " + cbReference + ";}";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "CallServer", callbackScript, true);
            //___________-end here___//

        }

        string ICallbackEventHandler.GetCallbackResult()
        {
            return data;
        }
        void ICallbackEventHandler.RaiseCallbackEvent(string eventArgument)
        {
            string id = eventArgument.ToString();
            string[] idlist = id.Split('~');
            string[] cl = idlist[1].Split(',');
            string str = "";
            string str1 = "";
            for (int i = 0; i < cl.Length; i++)
            {
                if (idlist[0].ToString().Trim() == "Clients" || idlist[0].ToString().Trim() == "SubAccount")
                {
                    string[] val = cl[i].Split(';');
                    string[] AcVal = val[0].Split('-');
                    if (str == "")
                    {

                        str = "'" + AcVal[0] + "'";
                        str1 = "'" + AcVal[0] + "'" + ";" + val[1];
                    }
                    else
                    {

                        str += ",'" + AcVal[0] + "'";
                        str1 += "," + "'" + AcVal[0] + "'" + ";" + val[1];
                    }
                }
                else
                {
                    string[] val = cl[i].Split(';');
                    if (str == "")
                    {
                        str = val[0];
                        str1 = val[0] + ";" + val[1];
                    }
                    else
                    {
                        str += "," + val[0];
                        str1 += "," + val[0] + ";" + val[1];
                    }
                }

            }

            if (idlist[0] == "Clients")
            {
                data = "Clients~" + str;
            }
            if (idlist[0] == "SubAccount")
            {
                data = "SubAccount~" + str;
            }
            else if (idlist[0] == "Group")
            {
                data = "Group~" + str;
            }
            else if (idlist[0] == "Branch")
            {
                data = "Branch~" + str;
            }
            else if (idlist[0] == "BranchGroup")
            {
                data = "BranchGroup~" + str;
            }
            else if (idlist[0] == "Segment")
            {
                data = "Segment~" + str;
            }
        }
        void date()
        {
            DtFrom.EditFormatString = oconverter.GetDateFormat("Date");
            DtFrom.Value = Convert.ToDateTime(oDBEngine.GetDate().ToShortDateString());
            DtTo.EditFormatString = oconverter.GetDateFormat("Date");
            DtTo.Value = Convert.ToDateTime(oDBEngine.GetDate().ToShortDateString());

        }

        protected void btnhide_Click(object sender, EventArgs e)
        {
            if (ddlGroup.SelectedItem.Value.ToString() == "1")
            {
                if (HiddenField_Group.Value.ToString().Trim() == "")
                {
                    BindGroup();
                }
            }
        }
        public void BindGroup()
        {
            ddlgrouptype.Items.Clear();
            DataTable DtGroup = oDBEngine.GetDataTable("tbl_master_groupmaster", "distinct gpm_Type", null);
            if (DtGroup.Rows.Count > 0)
            {
                ddlgrouptype.DataSource = DtGroup;
                ddlgrouptype.DataTextField = "gpm_Type";
                ddlgrouptype.DataValueField = "gpm_Type";
                ddlgrouptype.DataBind();
                ddlgrouptype.Items.Insert(0, new ListItem("Select GroupType", "0"));
                DtGroup.Dispose();

            }
            else
            {
                ddlgrouptype.Items.Insert(0, new ListItem("Select GroupType", "0"));
            }
        }

        void procedure()
        {
            //using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))
            //{

            //    SqlCommand cmd = new SqlCommand();
            //    cmd.Connection = con;
            //    cmd.CommandText = "[InterestCalculation]";
            //    cmd.CommandType = CommandType.StoredProcedure;

            //    cmd.Parameters.AddWithValue("@CompanyId", Session["LastCompany"].ToString());

            //    cmd.Parameters.AddWithValue("@SegmentID", HttpContext.Current.Session["UserSegID"]);
            //    if (rdbSegmentAll.Checked)
            //    {
            //        cmd.Parameters.AddWithValue("@Segment", "ALL");
            //    }
            //    else
            //    {
            //        cmd.Parameters.AddWithValue("@Segment", HiddenField_Segment.Value);
            //    }
            //    cmd.Parameters.AddWithValue("@FinYear", HttpContext.Current.Session["LastFinYear"]);
            //    cmd.Parameters.AddWithValue("@FromDate", DtFrom.Value);
            //    cmd.Parameters.AddWithValue("@ToDate", DtTo.Value);
            //    if (ddlAccountType.SelectedItem.Value.ToString().Trim() == "0")
            //    {
            //        cmd.Parameters.AddWithValue("@MainAccount", "SYSTM00001");
            //    }
            //    if (ddlAccountType.SelectedItem.Value.ToString().Trim() == "1")
            //    {
            //        cmd.Parameters.AddWithValue("@MainAccount", "SYSTM00002");
            //    }
            //    if (ddlAccountType.SelectedItem.Value.ToString().Trim() == "3")
            //    {
            //        cmd.Parameters.AddWithValue("@MainAccount", HiddenField_txtMainAccountCode.Text.ToString().Trim());
            //    }

            //    if (rdbClientALL.Checked)
            //    {
            //        cmd.Parameters.AddWithValue("@Client", "ALL");
            //    }
            //    else if (rdPOAClient.Checked)
            //    {
            //        cmd.Parameters.AddWithValue("@Client", "POA");
            //    }
            //    else
            //    {
            //        cmd.Parameters.AddWithValue("@Client", HiddenField_Client.Value);
            //    }
            //    cmd.Parameters.AddWithValue("@CreateUser", HttpContext.Current.Session["userid"]);
            //    cmd.Parameters.AddWithValue("@GenType", DdlRptView.SelectedItem.Value.ToString().Trim());


            //    SqlDataAdapter da = new SqlDataAdapter();
            //    da.SelectCommand = cmd;
            //    cmd.CommandTimeout = 0;
            //    ds.Reset();
            //    ds.Clear();
            //    da.Fill(ds);
            //    da.Dispose();
            //    ViewState["dataset"] = ds;

            //}

            string ParaSegmentAll = string.Empty;
            string ParaAccountType = string.Empty;
            string ParaClientAll = string.Empty;
            if (rdbSegmentAll.Checked)
            {
                ParaSegmentAll = "ALL";
            }
            else
            {
                ParaSegmentAll = HiddenField_Segment.Value.Trim();
            }
            if (ddlAccountType.SelectedItem.Value.ToString().Trim() == "0")
            {
                ParaAccountType = "SYSTM00001";
            }
            if (ddlAccountType.SelectedItem.Value.ToString().Trim() == "1")
            {
                ParaAccountType = "SYSTM00002";
            }
            if (ddlAccountType.SelectedItem.Value.ToString().Trim() == "3")
            {
                ParaAccountType = HiddenField_txtMainAccountCode.Text.ToString().Trim();
            }
            if (rdbClientALL.Checked)
            {
                ParaClientAll = "ALL";
            }
            else if (rdPOAClient.Checked)
            {
                ParaClientAll = "POA";
            }
            else
            {
                ParaClientAll = HiddenField_Client.Value.Trim();
            }
            //ds = oOthers.InterestCalculation(Session["LastCompany"].ToString(), HttpContext.Current.Session["UserSegID"].ToString(),
            //    ParaSegmentAll, Convert.ToDateTime(HttpContext.Current.Session["LastFinYear"].ToString()).ToString("yyyy-MM-dd"), Convert.ToDateTime(DtFrom.Value).ToString("yyyy-MM-dd"),
            //    Convert.ToDateTime(DtTo.Value).ToString("yyyy-MM-dd"), ParaAccountType, ParaClientAll,
            //    Convert.ToInt32(HttpContext.Current.Session["userid"].ToString()), DdlRptView.SelectedItem.Value.ToString().Trim());

            ds = oOthers.InterestCalculation(Session["LastCompany"].ToString(), HttpContext.Current.Session["UserSegID"].ToString(),
               ParaSegmentAll, HttpContext.Current.Session["LastFinYear"].ToString(), DtFrom.Value.ToString(),
               Convert.ToDateTime(DtTo.Value).ToString("yyyy-MM-dd"), ParaAccountType, ParaClientAll,
               Convert.ToInt32(HttpContext.Current.Session["userid"].ToString()), DdlRptView.SelectedItem.Value.ToString().Trim());

            ViewState["dataset"] = ds;
        }
        protected void btnGenerateposition_Click(object sender, EventArgs e)
        {
            procedure();
            ds = (DataSet)ViewState["dataset"];
            if (ds.Tables[0].Rows[0][0].ToString().Contains("Generated") == true)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "AlertRecord2", "AlertRecord('1');", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "AlertRecord1", "AlertRecord('2');", true);

            }
        }

        protected void BtnExcel_Click(object sender, EventArgs e)
        {
            ds = (DataSet)ViewState["dataset"];

            DataTable dtExport = ds.Tables[0].Copy();



            DataTable dtReportHeader = new DataTable();
            DataTable dtReportFooter = new DataTable();

            DataTable CompanyName = oDBEngine.GetDataTable("tbl_master_company", "cmp_Name", " cmp_internalId='" + Session["LastCompany"].ToString() + "'");
            dtReportHeader.Columns.Add(new DataColumn("Header", typeof(String))); //0
            DataRow HeaderRow = dtReportHeader.NewRow();
            HeaderRow[0] = CompanyName.Rows[0][0].ToString();
            dtReportHeader.Rows.Add(HeaderRow);
            DataRow DrRowR1 = dtReportHeader.NewRow();

            string str = null;
            str = "For The Period:" + oconverter.ArrangeDate2(DtFrom.Value.ToString().Trim()) + " - " + oconverter.ArrangeDate2(DtTo.Value.ToString().Trim());

            if (ddlAccountType.SelectedItem.Value.ToString().Trim() == "0" || ddlAccountType.SelectedItem.Value.ToString().Trim() == "1" || ddlAccountType.SelectedItem.Value.ToString().Trim() == "2")
            {
                str = str + " ; Account Type :" + ddlAccountType.SelectedItem.Text.ToString().Trim();
            }
            else
            {
                str = str + " ; Account Name :" + txtMainAccount.Text.ToString().Trim();
            }
            DrRowR1[0] = str;
            dtReportHeader.Rows.Add(DrRowR1);
            DataRow DrRowR2 = dtReportHeader.NewRow();
            dtReportHeader.Rows.Add(DrRowR2);
            DataRow HeaderRow1 = dtReportHeader.NewRow();
            dtReportHeader.Rows.Add(HeaderRow1);
            DataRow HeaderRow2 = dtReportHeader.NewRow();
            dtReportHeader.Rows.Add(HeaderRow2);

            dtReportFooter.Columns.Add(new DataColumn("Footer", typeof(String))); //0
            DataRow FooterRow1 = dtReportFooter.NewRow();
            dtReportFooter.Rows.Add(FooterRow1);
            DataRow FooterRow2 = dtReportFooter.NewRow();
            dtReportFooter.Rows.Add(FooterRow2);
            DataRow FooterRow = dtReportFooter.NewRow();
            FooterRow[0] = "* * *  End Of Report * * *   ";
            dtReportFooter.Rows.Add(FooterRow);

            objExcel.ExportToExcelforExcel(dtExport, "Interest & Penalty Generation Report", "Total", dtReportHeader, dtReportFooter);

        }
        protected void BtnCancel_Click(object sender, EventArgs e)
        {
            date();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "FnCancel", "FnCancel('1');", true);

        }
        protected void BtnGenerate_Click(object sender, EventArgs e)
        {
            DataTable DtReport = new DataTable();
            ds = (DataSet)ViewState["dataset"];
            DtReport = ds.Tables[1].Copy();
            int NoofRowsAffect = 0;
            string TABLEReport = oconverter.ConvertDataTableToXML(DtReport);/////////////////DATATABLE TO CONVERT XML DATA
            //String conn = ConfigurationManager.AppSettings["DBConnectionDefault"];
            //SqlConnection con = new SqlConnection(conn);
            //con.Open();
            //SqlCommand com = new SqlCommand("[InterestCalculation]", con);
            //com.CommandType = CommandType.StoredProcedure;
            //com.Parameters.AddWithValue("@AllData", TABLEReport);
            //com.Parameters.AddWithValue("@CompanyId", Session["LastCompany"].ToString());
            //com.Parameters.AddWithValue("@Segment", Convert.ToInt32(Session["usersegid"].ToString()));
            //com.Parameters.AddWithValue("@FromDate", DtFrom.Value);
            //com.Parameters.AddWithValue("@ToDate", DtTo.Value);
            //com.Parameters.AddWithValue("@Finyear", HttpContext.Current.Session["LastFinYear"]);
            //com.Parameters.AddWithValue("@CreateUser", HttpContext.Current.Session["userid"]);
            //if (ddlAccountType.SelectedItem.Value.ToString().Trim() == "0" || ddlAccountType.SelectedItem.Value.ToString().Trim() == "1" || ddlAccountType.SelectedItem.Value.ToString().Trim() == "2")
            //{
            //    com.Parameters.AddWithValue("@MainAccountSubLedgerType", "Customers");
            //}
            //else
            //{
            //    com.Parameters.AddWithValue("@MainAccountSubLedgerType", HiddenField_txtSubLedgerType.Text.ToString().Trim());
            //}
            //com.CommandTimeout = 0;
            //NoofRowsAffect = com.ExecuteNonQuery();
            //con.Close(); 


            string ParaMainSubLedger = string.Empty;

            if (ddlAccountType.SelectedItem.Value.ToString().Trim() == "0" || ddlAccountType.SelectedItem.Value.ToString().Trim() == "1" || ddlAccountType.SelectedItem.Value.ToString().Trim() == "2")
            {
                ParaMainSubLedger = "Customers";
            }
            else
            {
                ParaMainSubLedger = HiddenField_txtSubLedgerType.Text.ToString().Trim();
            }

            NoofRowsAffect = oOthers.InterestCalculationIns(TABLEReport, Session["LastCompany"].ToString(), Session["usersegid"].ToString(),
                Convert.ToDateTime(DtFrom.Value).ToString("yyyy-MM-dd"), Convert.ToDateTime(DtTo.Value).ToString("yyyy-MM-dd"), HttpContext.Current.Session["LastFinYear"].ToString(),
                Convert.ToInt32(HttpContext.Current.Session["userid"].ToString()), ParaMainSubLedger);

            ScriptManager.RegisterStartupScript(this, this.GetType(), "FnCancel", "FnCancel('2');", true);

        }
        protected void BtnDelete_Click(object sender, EventArgs e)
        {
            DataTable DtReport = new DataTable();
            ds = (DataSet)ViewState["dataset"];
            DtReport = ds.Tables[1].Copy();
            int NoofRowsAffect = 0;
            string TABLEReport = oconverter.ConvertDataTableToXML(DtReport);/////////////////DATATABLE TO CONVERT XML DATA
            //String conn = ConfigurationManager.AppSettings["DBConnectionDefault"];
            //SqlConnection con = new SqlConnection(conn);
            //con.Open();
            //SqlCommand com = new SqlCommand("[InterestCalculation]", con);
            //com.CommandType = CommandType.StoredProcedure;
            //com.Parameters.AddWithValue("@AllData", TABLEReport);
            //com.Parameters.AddWithValue("@CompanyId", Session["LastCompany"].ToString());
            //com.Parameters.AddWithValue("@Segment", Convert.ToInt32(Session["usersegid"].ToString()));
            //com.Parameters.AddWithValue("@FromDate", DtFrom.Value);
            //com.Parameters.AddWithValue("@ToDate", DtTo.Value);
            //com.CommandTimeout = 0;
            //NoofRowsAffect = com.ExecuteNonQuery();
            //con.Close();
            NoofRowsAffect = oOthers.InterestCalculationIns_2(TABLEReport, Session["LastCompany"].ToString(), Session["usersegid"].ToString(),
               Convert.ToDateTime(DtFrom.Value).ToString("yyyy-MM-dd"), Convert.ToDateTime(DtTo.Value).ToString("yyyy-MM-dd"));
            ScriptManager.RegisterStartupScript(this, this.GetType(), "FnCancel", "FnCancel('3');", true);
        }
    }
}