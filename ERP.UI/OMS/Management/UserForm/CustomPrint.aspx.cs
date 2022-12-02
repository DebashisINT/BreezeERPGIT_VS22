using DataAccessLayer;
using DevExpress.DataAccess.Sql;
using DevExpress.XtraReports.UI;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERP.OMS.Management.UserForm
{
    public partial class CustomPrint : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (IsPostBack)
            {
                XtraReport xtra = new XtraReport();
                DevExpress.DataAccess.Sql.SqlDataSource sql = GenerateSqlDataSource();

                //SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand com = new SqlCommand("prc_CustomReport", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@Action", "LoadLayoutprint");
                com.Parameters.AddWithValue("@ModName", Request.QueryString["ModName"]);
                con.Open();
                byte[] byteArray;
                byteArray = com.ExecuteScalar() as byte[];
                con.Close();
                if (byteArray != null)
                {
                    Stream stream = new MemoryStream(byteArray);
                    xtra.LoadLayout(stream);
                }
                xtra.DataSource = sql;
                ASPxDocumentViewer1.Report = xtra;

            }

        }


        private DevExpress.DataAccess.Sql.SqlDataSource GenerateSqlDataSource()
        {
            DevExpress.DataAccess.Sql.SqlDataSource result = new DevExpress.DataAccess.Sql.SqlDataSource("crmConnectionString");
            ProcedureExecute proc = new ProcedureExecute("prc_CustomReport");
            proc.AddVarcharPara("@Action", 100, "GetSqlQueryPrint");
            proc.AddPara("@id", Request.QueryString["id"]);
            proc.AddPara("@ModName", Request.QueryString["ModName"]);
            DataSet ds = proc.GetDataSet();

            result.Queries.Add(new CustomSqlQuery("Company", Convert.ToString(ds.Tables[0].Rows[0][0])));
           result.Queries.Add(new CustomSqlQuery("Document", Convert.ToString(ds.Tables[1].Rows[0][0])));
           // result.Queries.Add(new CustomSqlQuery("Document","select  id,[Customer Type], [Document Date], [Document No], [Due Amount], [Invoice Amount], [Payment %], [Payment], [vehicle type]  from tbl_Udtform_MyModule   where id=2"));

            result.RebuildResultSchema();
            return result;
        }
    }
}