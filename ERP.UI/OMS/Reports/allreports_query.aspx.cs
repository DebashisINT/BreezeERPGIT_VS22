using System;
using System.Data;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using BusinessLogicLayer;

namespace ERP.OMS.Reports
{
    public partial class Reports_allreports_query : System.Web.UI.Page
    {
        Converter objConverter = new Converter();
        //DBEngine oDBEngine = new DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);MULTI
        DBEngine oDBEngine = new DBEngine();
        ExcelFile objExcel = new ExcelFile();
        DataSet DS = new DataSet();
        DataTable dt1 = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                ScriptManager.RegisterStartupScript(this, this.GetType(), "JSct", "height();", true);
                dt1 = oDBEngine.GetDataTable("trans_compiledquery", "distinct compiledquery_name,compiledquery_path,compiledquery_id", null);
                if (dt1.Rows.Count > 0)
                {

                    ddlBank.DataSource = dt1;
                    ddlBank.DataTextField = "compiledquery_name";
                    ddlBank.DataValueField = "compiledquery_path";
                    //for (int i = 0; i < dt1.Rows.Count; i++)
                    //{
                    //    if (i == 0)
                    //    {
                    //        ddlBank.Items.Add(new ListItem("-Select-", "0"));
                    //        ddlBank.Items.Add(new ListItem(dt1.Rows[i][0].ToString(), dt1.Rows[i][1].ToString()));
                    //    }
                    //    else
                    //    {
                    //        ddlBank.Items.Add(new ListItem(dt1.Rows[i][0].ToString(), dt1.Rows[i][1].ToString()));
                    //    }
                    //}

                }
                //ddlBank.DataSource = dt1;
                //ddlBank.DataTextField = "BankName";
                //ddlBank.DataValueField = "ID";

                ddlBank.DataBind();
                ddlBank.Items.Insert(0, new ListItem("--Select--", "0"));

                //ddlBank.SelectedValue = "0";
            }
        }
        protected void BtnSave_Click(object sender, EventArgs e)
        {
            DataTable dtReportHeader = new DataTable();
            DataTable dtReportFooter = new DataTable();
            dtReportHeader = oDBEngine.GetDataTable("tbl_master_user", "top 1 user_id", null);
            dtReportFooter = oDBEngine.GetDataTable("tbl_master_user", "top 1 user_id", null);
            DataSet ds2 = new DataSet();
            DataTable dt1 = new DataTable();
            string final = null;
            string tmpPdfPath;
            string strquery = null;
            string fname = null;
            fname = ddlBank.SelectedItem.Value.ToString();
            tmpPdfPath = string.Empty;
            tmpPdfPath = HttpContext.Current.Server.MapPath(@"..\CompiledQuery\");
            tmpPdfPath = tmpPdfPath + ddlBank.SelectedValue.ToString().Trim();
            strquery = File.ReadAllText(tmpPdfPath);
            dt1 = oDBEngine.GetDataTable(strquery);
            //StreamReader strreader = new StreamReader(strquery);
            //strreader.Read();
            //strreader.Close();
            objExcel.ExportToExcelforExcel(dt1, fname, "abcd", dtReportHeader, dtReportFooter);




        }
        protected void ddlBank_SelectedIndexChanged1(object sender, EventArgs e)
        {


        }
    }
}
