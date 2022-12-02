using BusinessLogicLayer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERP.OMS.Management.Activities
{



    public partial class ViewPOS : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string InvoiceId = "";
            string DBName = "";

            if (!string.IsNullOrEmpty(Convert.ToString(Request.QueryString["key"])))
                InvoiceId = Convert.ToString(Request.QueryString["key"]);

            if (!string.IsNullOrEmpty(Convert.ToString(Request.QueryString["dbname"])))
                DBName = Convert.ToString(Request.QueryString["dbname"]);




            string baseUrl = Request.Url.Scheme + "://" + Request.Url.Authority +
Request.ApplicationPath.TrimEnd('/') + "/";
            //string url = "Reports/RepxReportDesign/SalesInvoice/DocDesign/PDFFiles/POS-CGST-Original-@invoice.pdf";


            DBEngine oDBEngine = new DBEngine();
            DataTable dtCmb = new DataTable();
            dtCmb = GetDataTableWithoutSession("select dbo.fInvoiceGSTType(" + Convert.ToString(InvoiceId) + ")",DBName);

            string strType = "";
            string HdPosType = "";

            if (dtCmb != null && dtCmb.Rows.Count > 0)
            {
                strType = Convert.ToString(dtCmb.Rows[0][0]);
            }

            dtCmb = GetDataTableWithoutSession("select Pos_EntryType  from tbl_trans_SalesInvoice where Invoice_Id='" + InvoiceId + "'",DBName);

            if (dtCmb != null && dtCmb.Rows.Count > 0)
            {
                HdPosType = Convert.ToString(dtCmb.Rows[0][0]);
            }

            string reportName = "";

            if ("0" != "1")
            {

                if (HdPosType == "Cash")
                {
                    if (strType == "CGST")
                    {
                        reportName = "POS-CGST~D";
                    }
                    else if (strType == "IGST")
                    {
                        reportName = "POS-IGST~D";
                    }

                }
                else if (HdPosType == "Crd")
                {
                    if (strType == "CGST")
                    {
                        reportName = "POS-CGST~D";
                    }
                    else if (strType == "IGST")
                    {
                        reportName = "POS-IGST~D";
                    }


                }
                else if (HdPosType == "Fin")
                {
                    if (strType == "CGST")
                    {
                        reportName = "POS-CGST~D";
                    }
                    else if (strType == "IGST")
                    {
                        reportName = "POS-IGST~D";
                    }


                }
                else if (HdPosType == "IST")
                {
                    if (strType == "CGST")
                    {
                        reportName = "POS-CGST~D";
                    }
                    else if (strType == "IGST")
                    {
                        reportName = "POS-IGST~D";
                    }

                }

            }
            else
            {
                if (HdPosType == "Cash")
                {
                    reportName = "POS-Cash~D";
                }
                else if (HdPosType == "Crd")
                {
                    reportName = "POS-Credit~D";
                }
                else if (HdPosType == "Fin")
                {
                    reportName = "POS-Finance~D";
                }
                else if (HdPosType == "IST")
                {

                    reportName = "InterstateStockTransfer-GST~D";

                }
            }





            string url  = "Reports/RepxReportDesign/SalesInvoice/DocDesign/PDFFiles/" + reportName + "-Original-" + InvoiceId + ".pdf";

            Export.ExportToPDF exportToPDF = new Export.ExportToPDF();
            exportToPDF.ExportToPdfforEmail(reportName, "Invoice_POS", Server.MapPath("~"), "1", Convert.ToString(InvoiceId),DBName);


            //msgBody = msgBody.Replace("@Customer_name", txtCustName.Text.ToString().Trim());

            
            url = baseUrl + url.Replace("~D","");
            Response.Redirect(url);

        }
        private DataTable GetDataTableWithoutSession(string lcSql, string DBname)
        {
            string masterDbanem = DBname;
            string oSql = Convert.ToString(GetConnectionString(masterDbanem));
            SqlConnection oSqlConnection = new SqlConnection(oSql);
            SqlDataAdapter lda = new SqlDataAdapter(lcSql, oSqlConnection);
            DataTable GetTable = new DataTable();
            lda.SelectCommand.CommandTimeout = 0;
            lda.Fill(GetTable);
            oSqlConnection.Close();
            return GetTable;


        }
        public string GetConnectionString(string dbName)
        {
            string Conn = "";
            string DtSource = ConfigurationSettings.AppSettings["sqlDatasource"];
            string UserId = ConfigurationSettings.AppSettings["sqlUserId"];
            string Pwd = ConfigurationSettings.AppSettings["sqlPassword"];
            string IntSq = ConfigurationSettings.AppSettings["sqlAuth"];
            string ispool = ConfigurationSettings.AppSettings["isPool"];
            string poolsize = ConfigurationSettings.AppSettings["PoolSize"];

            SqlConnectionStringBuilder connectionString = new SqlConnectionStringBuilder();
            connectionString.DataSource = DtSource;
            connectionString.InitialCatalog = dbName;
            if (IntSq == "Windows")
            {
                connectionString.IntegratedSecurity = true;
            }
            else
            {
                connectionString.PersistSecurityInfo = true;
                connectionString.IntegratedSecurity = false;
                connectionString.UserID = UserId;
                connectionString.Password = Pwd;

            }
            connectionString.ConnectTimeout = 950;
            connectionString.Pooling = Convert.ToBoolean(ispool);
            connectionString.MaxPoolSize = Convert.ToInt32(poolsize);



            string str = connectionString.ConnectionString;



            return str;
        }
    }
}