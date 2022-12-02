using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogicLayer;


namespace ERP.OMS.Reports
{
    public partial class Reports_ExportDematTransactioncommCurrency : System.Web.UI.Page
    {
        BusinessLogicLayer.Converter oconverter = new BusinessLogicLayer.Converter();
        BusinessLogicLayer.Reports oReports = new BusinessLogicLayer.Reports();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        DataSet ds = new DataSet();


        bool FileWritten = false;
        string ExportedFileName = "";
        public string ExportedFilePath = "";

        public string[] InputName = new string[1];
        public string[] InputType = new string[1];
        public string[] InputValue = new string[1];

        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.Session["userid"] == null)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            if (!IsPostBack)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script language='javascript'>Page_Load();</script>");

                DtTransactionDate.EditFormatString = oconverter.GetDateFormat("Date");
                DtTransactionDate.Value = Convert.ToDateTime(oDBEngine.GetDate().ToShortDateString());
                DtExecutionDate.EditFormatString = oconverter.GetDateFormat("Date");
                DtExecutionDate.Value = Convert.ToDateTime(oDBEngine.GetDate().ToShortDateString());

                AccountFetch();
            }

        }
        void AccountFetch()
        {
            InputName[0] = "Criteria";
            InputType[0] = "V";

            InputValue[0] = " Where rtrim(DPACCounts_AccountType) in ('[POOL]','[PLPAYIN]','[PLPAYOUT]','[CISA]') and DPACCounts_CompanyID='" + Session["LastCompany"].ToString() + "' and dpaccounts_exchangesegmentid='" + Session["usersegid"].ToString().Trim() + "'";

            DataTable dtDeliverAc = SQLProcedures.SelectProcedureArr("[Fetch_AccountName]", InputName, InputType, InputValue);

            ddlAccount.DataTextField = "ShortName";
            ddlAccount.DataValueField = "ID";
            ddlAccount.DataSource = dtDeliverAc;
            ddlAccount.DataBind();
        }
        protected void BtnScreen_Click(object sender, EventArgs e)
        {
            BindGrid();
        }
        void BindGrid()
        {
            string CDSLTranType = string.Empty;
            if (RdbCDSLTransferType_InterDepository.Checked)
            {
                CDSLTranType = "I";
            }
            else
            {
                CDSLTranType = "O";
            }
            ds = oReports.Report_DematBatchSummaryCommCurrency(
                Convert.ToString(ddlAccount.SelectedItem.Value),
                 Convert.ToString(DtTransactionDate.Value),
                CDSLTranType
                );

            if (ds.Tables[0].Rows.Count > 0)
            {
                gridSummary.DataSource = ds;
                gridSummary.DataBind();
                if (ds.Tables[1].Rows[0][0].ToString() == "")
                {
                    txtNextBatch.Text = "1";
                }
                else
                {
                    txtNextBatch.Text = ds.Tables[1].Rows[0][0].ToString();
                }

                ScriptManager.RegisterStartupScript(this, this.GetType(), "RecordDisplay", "RecordDisplay('2');", true);

            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "RecordDisplay", "RecordDisplay('1');", true);
            }
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            string Account = ddlAccount.SelectedItem.Value;
            string[] RAc = Account.Split('~');

            if (RAc[1].ToString().Trim().Substring(0, 2) == "IN")
            {
                Export_NsdlDematTransactionCommCurrency(RAc[0].ToString().Trim());
            }
            else
            {
                Export_CdslDematTransactionCommCurrency(RAc[0].ToString().Trim());
            }

        }
        void Export_NsdlDematTransactionCommCurrency(string Accid)
        {

            ds = oReports.Export_NsdlDematTransactionCommCurrency(
                Convert.ToString(ddlAccount.SelectedItem.Value),
                Convert.ToString(Session["userid"]),
                Convert.ToString(DtTransactionDate.Value),
                Convert.ToString(DtExecutionDate.Value),
                Convert.ToString(txtparam.Text),
                Convert.ToString(txtNextBatch.Text)
                );

            if (ds.Tables[0].Rows[0][0].ToString().Trim() != "BatchNumber Exists")
            {
                string strDate = oDBEngine.GetDate().ToString("dd_MMM_yyyy_HH_mm_ss");

                ExportedFileName = "Trn_" + Accid + "_" + Session["userid"].ToString() + "_" + strDate;
                ExportedFilePath = Server.MapPath(@"../ExportFiles/DematBatch/") + ExportedFileName;

                if (ds.Tables[ds.Tables.Count - 2].Rows[0]["TotalNo_DetailRecords"].ToString() != "00000")
                    FileWritten = WriteFile(ds, ExportedFilePath, "NSDL");

                else
                {
                    FileWritten = false;
                    ScriptManager.RegisterStartupScript(HttpContext.Current.Handler as Page, HttpContext.Current.GetType(), "aa", "AlertRecord();", true);
                }
            }
            else
            {
                FileWritten = false;
                ScriptManager.RegisterStartupScript(HttpContext.Current.Handler as Page, HttpContext.Current.GetType(), "aa", "BatchExists();", true);
            }

            BindGrid();
            if (FileWritten)
            {
                ViewState["ExportedFilePath"] = ExportedFilePath;
                ScriptManager.RegisterStartupScript(HttpContext.Current.Handler as Page, HttpContext.Current.GetType(), "d", "Download();", true);
            }
            else
                ScriptManager.RegisterStartupScript(HttpContext.Current.Handler as Page, HttpContext.Current.GetType(), "aa", "AlertRecord();", true);
        }
        void Export_CdslDematTransactionCommCurrency(string Accid)
        {

            ds = oReports.Export_CdslDematTransactionCommCurrency(
              Convert.ToString(ddlAccount.SelectedItem.Value),
              Convert.ToString(Session["userid"]),
              Convert.ToString(DtTransactionDate.Value),
              Convert.ToString(DtExecutionDate.Value),
              Convert.ToString(txtparam.Text),
              Convert.ToString(txtNextBatch.Text)
              );

            if (ds.Tables[0].Rows[0][0].ToString().Trim() != "BatchNumber Exists")
            {
                ExportedFileName = ds.Tables[ds.Tables.Count - 1].Rows[0][0].ToString();
                ExportedFilePath = Server.MapPath(@"../ExportFiles/DematBatch/") + ExportedFileName;
                FileWritten = WriteFile(ds, ExportedFilePath, "CDSL");

            }
            else
            {
                FileWritten = false;
                ScriptManager.RegisterStartupScript(HttpContext.Current.Handler as Page, HttpContext.Current.GetType(), "aa", "BatchExists();", true);
            }

            BindGrid();
            if (FileWritten)
            {
                ViewState["ExportedFilePath"] = ExportedFilePath;
                ScriptManager.RegisterStartupScript(HttpContext.Current.Handler as Page, HttpContext.Current.GetType(), "d", "Download();", true);
            }
            else
                ScriptManager.RegisterStartupScript(HttpContext.Current.Handler as Page, HttpContext.Current.GetType(), "aa", "AlertRecord();", true);
        }
        protected bool WriteFile(DataSet ds, string ExportedFilePath, string type)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(ExportedFilePath, false))
                {
                    if (type == "NSDL")
                    {


                        int colCount = ds.Tables[ds.Tables.Count - 2].Columns.Count;

                        foreach (DataRow dr in ds.Tables[ds.Tables.Count - 2].Rows)
                        {
                            for (int j = 0; j < colCount; j++)
                            {

                                if (!Convert.IsDBNull(dr[j]))
                                    sw.Write(dr[j]);


                            }

                            sw.Write(sw.NewLine);
                        }

                        // Detail Tables
                        string strLineDefault = "";
                        strLineDefault = "000000";

                        string strLine;
                        string strLine1 = "";
                        int intLine = 0;

                        for (int i = 0; i < ds.Tables.Count - 2; i++)
                        {

                            colCount = ds.Tables[i].Columns.Count;

                            foreach (DataRow dr in ds.Tables[i].Rows)
                            {

                                if (!Convert.IsDBNull(dr[0]))
                                {
                                    intLine = intLine + 1;
                                    strLine = strLineDefault + Convert.ToString(intLine);
                                    strLine1 = strLine.Substring(strLine.Length - 6, 6);

                                    sw.Write(dr[0]);
                                }
                                if (!Convert.IsDBNull(dr[1]))
                                {
                                    sw.Write(dr[1]);
                                    sw.Write(strLine1);
                                }

                                for (int j = 2; j < colCount; j++)
                                {


                                    if (!Convert.IsDBNull(dr[j]))
                                        sw.Write(dr[j]);


                                }

                                sw.Write(sw.NewLine);
                            }

                        }

                        // Trailer table

                        colCount = ds.Tables[ds.Tables.Count - 1].Columns.Count;

                        foreach (DataRow dr in ds.Tables[ds.Tables.Count - 1].Rows)
                        {
                            for (int j = 0; j < colCount; j++)
                            {

                                if (!Convert.IsDBNull(dr[j]))
                                    sw.Write(dr[j]);


                            }

                            sw.Write(sw.NewLine);
                        }
                        // end bracket

                    }
                    else if (type == "CDSL")
                    {
                        //for (int i = 0; i < ds.Tables.Count-1; i++)
                        for (int i = ds.Tables.Count - 2; i >= 0; i--)
                        {
                            int colCount = ds.Tables[i].Columns.Count;

                            foreach (DataRow dr in ds.Tables[i].Rows)
                            {
                                for (int j = 0; j < colCount; j++)
                                {

                                    if (!Convert.IsDBNull(dr[j]))
                                        sw.Write(dr[j]);


                                }

                                sw.Write('\n');
                            }
                        }
                    }

                }
                ds = oReports.sp_Insert_ExportFiles2(
                     Convert.ToString(Session["userid"]),
                     Convert.ToString(HttpContext.Current.Session["usersegid"]),
                   "Demat Batch",
                   ExportedFileName,
                   Convert.ToString(txtNextBatch.Text),
                  "ExportFiles/DematBatch/" + ExportedFileName
                     );

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }


        }
        protected void btnDownload_Click(object sender, EventArgs e)
        {
            ExportedFilePath = ViewState["ExportedFilePath"].ToString();
            SaveFile(ExportedFilePath);
        }
        protected void SaveFile(string ExportedFilePath)
        {
            string filename = ExportedFilePath;
            FileInfo fileInfo = new FileInfo(filename);
            String fileExt = String.Empty;

            fileExt = fileInfo.Name + ".txt";

            if (fileInfo.Exists)
            {
                Response.Clear();
                Response.AddHeader("Content-Disposition", "attachment;filename=" + fileExt);
                Response.AddHeader("Content-Length", fileInfo.Length.ToString());
                Response.ContentType = "application/unknown";
                Response.Flush();
                Response.WriteFile(fileInfo.FullName);
                //Response.TransmitFile(fileInfo.FullName);
                Response.End();

            }

        }
        protected void gridSummary_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                string batch_print = gridSummary.Rows[index].Cells[1].Text.Trim();
                string slip_print = gridSummary.Rows[index].Cells[2].Text.Trim();

                TextBox txtslip = (TextBox)gridSummary.Rows[index].FindControl("txtslip");

                string type = "";
                string seg = HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim();

                string Account = ddlAccount.SelectedItem.Value;
                string[] RAc = Account.Split('~');

                if (RAc[1].ToString().Trim().Substring(0, 2) == "IN")////NSDl
                {
                    if (gridSummary.Rows[index].Cells[4].Text.Trim() == "Intra-Depository")
                        type = "IntraDP-NSDL";
                    else if (gridSummary.Rows[index].Cells[4].Text.Trim() == "Inter-Depository")
                        type = "InterDP-NSDL";
                    else if (gridSummary.Rows[index].Cells[3].Text.Trim() == "InterSettlement")
                        type = "InterSettlement-NSDL";

                }
                else
                {
                    if (RdbCDSLTransferType_InterDepository.Checked)
                        type = "InterDP-CDSL";
                    else
                        type = "OffMarket-CDSL";
                }

                if (batch_print == "")
                    Page.ClientScript.RegisterStartupScript(GetType(), "batch", "alert('There is no batch generated for these instructions.\nPlease generate batch, then print.');", true);

                //else
                //btnPrint_Click(sender, e);
                //Server.Transfer("frm_Download.aspx?b=" + batch_print + "&s=" + slip_print + "&t=" + type, false);
                Response.Redirect("..//management//frm_Download.aspx?f=download&b=" + batch_print + "&s=" + txtslip.Text.ToString().Trim() + "&t=" + type + "&seg=" + seg + "&partype='Spot'", true);

            }

        }
    }
}