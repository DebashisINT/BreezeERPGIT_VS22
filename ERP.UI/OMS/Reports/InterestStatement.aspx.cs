using System;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using BusinessLogicLayer;

namespace ERP.OMS.Reports
{
    public partial class Management_PledgeEntry : System.Web.UI.Page
    {
        GenericMethod oGenericMethod;
        GenericExcelExport oGenericExcelExport;
        GenericStoreProcedure objGenericStoreProcedure;

        #region Page Methods
        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Height", "height();", true);
            // string S = Session["FinYearStart"].ToString();
            if (!IsPostBack)
            {
                txtDateFrom.Value = System.DateTime.Now;
                txtDateTo.Value = System.DateTime.Now;
            }
            else
            {
                if (rlstSubAcType.SelectedValue == "0")
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "changestate1", "changeState('0');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "changestate0", "changeState('1');", true);
                }
            }
            //  string s = Request.Form["__EVENTTARGET"];
        }
        protected void Page_PreRender(object sender, EventArgs e)
        {

        }
        #endregion
        #region Methods
        public string GetDateFormat(object dates)
        {
            string format = "dd-MMM-yyyy";
            DateTime now = DateTime.Now;
            DateTime dt = Convert.ToDateTime(dates);
            string s4 = dt.ToString(format);
            return s4;
        }
        public string GetDateTimeFormat(object dates)
        {
            string format = "dd-MMM-yyyy HH:mm";
            DateTime now = DateTime.Now;
            DateTime dt = Convert.ToDateTime(dates);
            string s4 = dt.ToString(format);
            return s4;
        }
        private void RemoveItem()
        {
            int i = lbSubAc.SelectedIndex;
            if (i != -1)
            {
                lbSubAc.Items.RemoveAt(i);
            }
            if (lbSubAc.Items.Count > 0)
            {
                hdnSubAcNo.Value = "1";
            }
            else
            {
                hdnSubAcNo.Value = "0";
            }
            ScriptManager.RegisterStartupScript(this, GetType(), "close", "ClosePopUp();", true);
        }
        private void GeneratePDF()
        {
            ReportDocument reportObj = new ReportDocument();
            try
            {
                double totalInterest = 0;
                objGenericStoreProcedure = new GenericStoreProcedure();
                DataTable Datafetch = new DataTable();
                string[] strSpParam = new string[1];
                strSpParam[0] = "RefID|" + GenericStoreProcedure.ParamDBType.Varchar + "|20|" + Convert.ToString(ViewState["id"]) + "|" + GenericStoreProcedure.ParamType.ExParam;
                Datafetch = objGenericStoreProcedure.Procedure_DataTable(strSpParam, "spInterestDetail");
                //  string strHeaderMessage = "";
                foreach (DataRow dr in Datafetch.Rows)
                {
                    totalInterest += Convert.ToDouble(dr["Interest"]);
                }
                Datafetch.Rows.Add("Total", null, null, null, null, null, null, null, null, null, Convert.ToString(totalInterest)).AcceptChanges();
                string strHeaderMessage = "Interest Details Report of " + GetCompanyName() + " from " + Convert.ToString(txtDateFrom.Value) + " to " + Convert.ToString(txtDateTo.Value);
                Datafetch.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + @"\Reports\InterestReport.xsd");
                string[] connPath = ConfigurationManager.AppSettings["DBConnectionDefault"].Split(';');
                string attachmentName = "IntrestDetailsReport_" + System.DateTime.Now.ToLocalTime().ToString();
                string ReportPath = Server.MapPath("..\\Reports\\InterestDetails.rpt");
                reportObj.Load(ReportPath);
                reportObj.SetDatabaseLogon(connPath[2].Substring(connPath[2].IndexOf("=")).Trim(), connPath[3].Substring(connPath[3].IndexOf("=")).Trim(), connPath[0].Substring(connPath[0].IndexOf("=")).Trim(), connPath[1].Substring(connPath[1].IndexOf("=")).Trim(), false);
                reportObj.SetDataSource(Datafetch);
                reportObj.SetParameterValue("@strHeaderMessage", strHeaderMessage);
                reportObj.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, HttpContext.Current.Response, true, attachmentName);
                //reportObj.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.WordForWindows, HttpContext.Current.Response, true, "contractnote_cumbill");
            }
            finally
            {
                reportObj.Dispose();
                GC.Collect();
            }

        }
        private void BindGrid(int Page)
        {
            ViewState["SummaryData"] = null;
            objGenericStoreProcedure = new GenericStoreProcedure();
            // objGenericStoreProcedure = new GenericStoreProcedure();
            DataTable dtInterestSchemeConfig = new DataTable();
            string[] strSpParam = new string[6];

            strSpParam[0] = "Company|" + GenericStoreProcedure.ParamDBType.Varchar + "|50|" + Convert.ToString(Session["LastCompany"]) + "|" + GenericStoreProcedure.ParamType.ExParam;
            strSpParam[1] = "DateFrom|" + GenericStoreProcedure.ParamDBType.DateTime + "|20|" + txtDateFrom.Value.ToString() + "|" + GenericStoreProcedure.ParamType.ExParam;
            strSpParam[2] = "DateTo|" + GenericStoreProcedure.ParamDBType.DateTime + "|20|" + txtDateTo.Value.ToString() + "|" + GenericStoreProcedure.ParamType.ExParam;
            strSpParam[3] = "MainAccount|" + GenericStoreProcedure.ParamDBType.Varchar + "|20|" + Convert.ToString(txtMainAc_hidden.Value).Split('*')[1] + "|" + GenericStoreProcedure.ParamType.ExParam;
            if (rbtnInterestType.SelectedValue == "1")
            {
                strSpParam[4] = "IntAmount|" + GenericStoreProcedure.ParamDBType.Char + "|1|" + "B" + "|" + GenericStoreProcedure.ParamType.ExParam;
            }
            else
            {
                strSpParam[4] = "IntAmount|" + GenericStoreProcedure.ParamDBType.Char + "|1|" + "A" + "|" + GenericStoreProcedure.ParamType.ExParam;
            }
            if (rlstSubAcType.SelectedValue == "0" && lbSubAc.Items.Count > 0)
            {
                string subAcIds = "'0',";
                foreach (ListItem li in lbSubAc.Items)
                {
                    subAcIds += "'" + li.Value + "'" + ",";
                }
                subAcIds += "'0'";
                strSpParam[5] = "SubAccount|" + GenericStoreProcedure.ParamDBType.Varchar + "|2000|" + subAcIds + "|" + GenericStoreProcedure.ParamType.ExParam;
            }
            else
            {
                strSpParam[5] = "SubAccount|" + GenericStoreProcedure.ParamDBType.Varchar + "|2000|" + string.Empty + "|" + GenericStoreProcedure.ParamType.ExParam;
            }
            dtInterestSchemeConfig = objGenericStoreProcedure.Procedure_DataTable(strSpParam, "spInterestSummary");
            gvInterestSummary.DataSource = dtInterestSchemeConfig;
            gvInterestSummary.PageIndex = Page;
            gvInterestSummary.DataBind();
            if (dtInterestSchemeConfig != null && dtInterestSchemeConfig.Rows.Count > 0)
            {
                ViewState["SummaryData"] = dtInterestSchemeConfig;
                ScriptManager.RegisterStartupScript(this, GetType(), "ShowHide10", "ShowHide('0');", true);
            }
            ScriptManager.RegisterStartupScript(this, GetType(), "close", "ClosePopUp();", true);
        }
        private void LoadInterestDetails(string id, int Page)
        {


            ViewState["id"] = id;
            objGenericStoreProcedure = new GenericStoreProcedure();
            DataTable dtInterestReportDetail = new DataTable();
            string[] strSpParam = new string[1];
            strSpParam[0] = "RefID|" + GenericStoreProcedure.ParamDBType.Varchar + "|20|" + id + "|" + GenericStoreProcedure.ParamType.ExParam;
            dtInterestReportDetail = objGenericStoreProcedure.Procedure_DataTable(strSpParam, "spInterestDetail");
            if (dtInterestReportDetail != null && dtInterestReportDetail.Rows.Count > 0)
            {
                ViewState["ExportableData"] = dtInterestReportDetail;
                gvInterestDetails.DataSource = dtInterestReportDetail;
                gvInterestDetails.PageIndex = Page;
                gvInterestDetails.DataBind();
                ScriptManager.RegisterStartupScript(this, GetType(), "show", "ShowPopUp();", true);
                // ScriptManager.RegisterStartupScript(this, GetType(), "noalert11", "test();", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "close11", "ClosePopUp();", true);
                ScriptManager.RegisterStartupScript(this, GetType(), "noalert", "alert('No Record Found');", true);
            }

        }
        private string GetCompanyName()
        {
            GenericMethod oGenericMethod = new GenericMethod();
            DataTable dtcompany = oGenericMethod.GetDataTable("select ltrim(rtrim(cmp_Name)) as company from tbl_master_company where cmp_internalid ='" + Session["LastCompany"].ToString() + "'");
            return Convert.ToString(dtcompany.Rows[0]["company"]);
        }
        private void ExcelExport()
        {

            double totalInterest = 0;
            // DataSet ds = (DataSet)ViewState["dataset"];
            ExcelFile objExcel = new ExcelFile();
            DataTable dtExport = new DataTable();
            string searchCriteria = null;
            Converter oconverter = new Converter();
            GenericMethod oGenericMethod = new GenericMethod();

            // searchCriteria = "From " + oconverter.ArrangeDate2(dtFrom.Value.ToString()) + "To " + oconverter.ArrangeDate2(dtTo.Value.ToString()) + " Register Of Security   ";
            searchCriteria = "From ";
            // dtExport = ds.Tables[0].Copy();
            if (ViewState["ExportableData"] != null)
            {
                dtExport = (ViewState["ExportableData"] as DataTable);
            }
            foreach (DataRow dr in dtExport.Rows)
            {
                totalInterest += Convert.ToDouble(dr["Interest"]);
            }
            int removePostion = dtExport.Columns.Count - 1;
            dtExport.Columns.RemoveAt(removePostion);

            //  dtExport.Columns.RemoveAt(11);
            dtExport.Columns.RemoveAt(4);
            dtExport.Rows.Add("Total", null, null, null, null, null, null, null, null, null, totalInterest.ToString()).AcceptChanges();
            GenericExcelExport oGenericExcelExport = new GenericExcelExport();
            string strDownloadFileName = "";
            // string exlDateTime = System.DateTime.Now.ToShortDateString();
            string exlDateTime = oGenericMethod.GetDate(113).ToString();
            string exlTime = exlDateTime.Replace(":", "");
            exlTime = exlTime.Replace(" ", "");
            string FileName = "InterestDetails_" + exlTime;
            strDownloadFileName = "~/Documents/";
            oGenericMethod = new GenericMethod();
            DataTable dtcompany = oGenericMethod.GetDataTable("select ltrim(rtrim(cmp_Name)) as company from tbl_master_company where cmp_internalid ='" + Session["LastCompany"].ToString() + "'");
            string[] strHead = new string[3];
            strHead[0] = exlDateTime;
            strHead[1] = searchCriteria;
            strHead[0] = Convert.ToString(txtDateFrom.Value) + " to " + Convert.ToString(txtDateTo.Value);
            strHead[2] = "Interest Details Report of " + dtcompany.Rows[0]["company"];
            string ExcelVersion = "2007";                                                                 //Lots

            string[] ColumnType = { "V", "V", "V", "V", "V", "V", "V", "V", "V", "V", "V" };
            string[] ColumnSize = { "140", "140", "90", "170", "150", "170", "150", "150", "150", "150", "150" };
            string[] ColumnWidthSize = { "30", "25", "30", "15", "15", "15", "15", "15", "10", "15", "25" };
            oGenericExcelExport.ExportToExcel(ColumnType, ColumnSize, ColumnWidthSize, dtExport, Server.MapPath(strDownloadFileName), ExcelVersion, FileName, strHead, null);

        }
        private void SummaryExcelExport()
        {
            double totalInterest = 0;
            // DataSet ds = (DataSet)ViewState["dataset"];
            ExcelFile objExcel = new ExcelFile();
            DataTable dtExport = new DataTable();
            string searchCriteria = null;
            Converter oconverter = new Converter();
            GenericMethod oGenericMethod = new GenericMethod();

            // searchCriteria = "From " + oconverter.ArrangeDate2(dtFrom.Value.ToString()) + "To " + oconverter.ArrangeDate2(dtTo.Value.ToString()) + " Register Of Security   ";
            searchCriteria = "From ";
            // dtExport = ds.Tables[0].Copy();
            if (ViewState["SummaryData"] != null)
            {
                dtExport = (ViewState["SummaryData"] as DataTable);
            }
            foreach (DataRow dr in dtExport.Rows)
            {
                totalInterest += Convert.ToDouble(dr["Interest"]);
            }

            dtExport.Columns.RemoveAt(8);
            dtExport.Columns.RemoveAt(5);
            dtExport.Columns.RemoveAt(0);
            dtExport.Columns.RemoveAt(9);
            dtExport.Rows.Add("Total", null, null, null, totalInterest.ToString(), null, null, null, null).AcceptChanges();
            GenericExcelExport oGenericExcelExport = new GenericExcelExport();
            string strDownloadFileName = "";
            // string exlDateTime = System.DateTime.Now.ToShortDateString();
            string exlDateTime = oGenericMethod.GetDate(113).ToString();
            string exlTime = exlDateTime.Replace(":", "");
            exlTime = exlTime.Replace(" ", "");
            string FileName = Session["LastCompany"].ToString() + "_" + exlTime;
            strDownloadFileName = "~/Documents/";
            oGenericMethod = new GenericMethod();
            DataTable dtcompany = oGenericMethod.GetDataTable("select ltrim(rtrim(cmp_Name)) as company from tbl_master_company where cmp_internalid ='" + Session["LastCompany"].ToString() + "'");
            string[] strHead = new string[3];
            strHead[0] = exlDateTime;
            strHead[1] = searchCriteria;
            strHead[0] = Convert.ToString(txtDateFrom.Value) + " to " + Convert.ToString(txtDateTo.Value);
            strHead[2] = "Interest Details Report of " + dtcompany.Rows[0]["company"];
            string ExcelVersion = "2007";                                                                 //Lots

            string[] ColumnType = { "V", "V", "V", "V", "V", "V", "V", "V", "V" };
            string[] ColumnSize = { "140", "140", "90", "170", "150", "170", "150", "150", "150" };
            string[] ColumnWidthSize = { "40", "40", "60", "25", "15", "15", "15", "15", "10" };
            oGenericExcelExport.ExportToExcel(ColumnType, ColumnSize, ColumnWidthSize, dtExport, Server.MapPath(strDownloadFileName), ExcelVersion, FileName, strHead, null);

        }
        #endregion
        #region Button Events
        protected void btnExport_Click(object sender, EventArgs e)
        {
            SummaryExcelExport();
        }
        protected void btnFilter_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "close", "ClosePopUp();", true);
            ScriptManager.RegisterStartupScript(this, GetType(), "ShowHide10", "ShowHide('1');", true);

        }
        protected void btnRemove_Click(object sender, EventArgs e)
        {
            RemoveItem();
        }
        protected void btnGenerateReport_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "show", "ShowPopUp();", true);
            if (ddlExportType.SelectedValue == "1")
            {
                ExcelExport();
            }
            else if (ddlExportType.SelectedValue == "2")
            {
                GeneratePDF();
            }

        }
        protected void ibtn_Close(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "close", "ClosePopUp();", true);
        }
        protected void btnShow_Click(object sender, EventArgs e)
        {
            BindGrid(0);
        }
        protected void ddlExportType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "show", "ShowPopUp();", true);
            /* if (ddlExportType.SelectedValue == "1")
               {
                   ExcelExport();
               }
               else if (ddlExportType.SelectedValue == "2")
               {
                   GeneratePDF();
               } */
        }
        private void AddSubAc()
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "close", "ClosePopUp();", true);
            if (txtSubAc.Text != string.Empty && txtSubAc_hidden.Value != string.Empty)
            {
                ListItem li = new ListItem();
                li.Text = txtSubAc.Text;
                if (txtSubAc_hidden.Value.Contains("*"))
                {
                    li.Value = txtSubAc_hidden.Value.ToString().Split('*')[1];
                }
                lbSubAc.Items.Add(li);
                txtSubAc.Text = string.Empty;

                hdnSubAcNo.Value = "1";
                //  hdnSelectedSubAcs.Value+= ",'" + txtSubAc_hidden.Value.ToString().Split('*')[1] + "'";
                txtSubAc_hidden.Value = string.Empty;
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "checks", "check();", true);
            }
        }
        #endregion
        #region Control Events
        protected void lbtnAddSubAc_Click(object sender, EventArgs e)
        {
            AddSubAc();
        }
        #endregion
        #region Grid Methods
        protected void gvInterestSummary_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.ToString() == "det")
            {
                ddlExportType.SelectedValue = "0";
                string referenceId = e.CommandArgument.ToString();
                LoadInterestDetails(referenceId, 0);

            }
        }
        protected void gvInterestSummary_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }
        protected void gvInterestSummary_PageIndexChanged(object sender, EventArgs e)
        {

        }
        protected void gvInterestSummary_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //gvInterestSummary.PageIndex = e.NewPageIndex;
            BindGrid(e.NewPageIndex);
        }
        protected void gvInterestSummary_Sorted(object sender, EventArgs e)
        {

        }
        protected void gvInterestSummary_Sorting(object sender, GridViewSortEventArgs e)
        {

        }
        protected void gvInterestDetails_PageIndexChanged(object sender, EventArgs e)
        {

        }
        protected void gvInterestDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "show", "ShowPopUp();", true);

            LoadInterestDetails(Convert.ToString(ViewState["id"]), e.NewPageIndex);

        }
        #endregion

    }
}