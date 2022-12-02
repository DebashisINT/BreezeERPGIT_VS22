using DataAccessLayer;
using DevExpress.DataAccess.Sql;
using DevExpress.XtraReports.UI;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Sql;
using System.Data.SqlClient;
using System.IO;
using DevExpress.DataAccess.ConnectionParameters;

namespace ERP.OMS.Management.UserForm
{
    public partial class CustomDesginer : System.Web.UI.Page
    {

        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                loadReportDesigner();
            }
        }

        public void loadReportDesigner() 
        {
            XtraReport xtra = new XtraReport();
            DevExpress.DataAccess.Sql.SqlDataSource sql = GenerateSqlDataSource();
          

            //SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
            SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            SqlCommand com = new SqlCommand("prc_CustomReport", con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@Action", "LoadLayout");
            com.Parameters.AddWithValue("@id", Request.QueryString["id"]);
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
            ASPxReportDesigner1.OpenReport(xtra);
            
        
        }

        private DevExpress.DataAccess.Sql.SqlDataSource GenerateSqlDataSource()
        {
            //DevExpress.DataAccess.Sql.SqlDataSource result = new DevExpress.DataAccess.Sql.SqlDataSource("crmConnectionString");

            CustomStringConnectionParameters connectionParameters = new CustomStringConnectionParameters(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            DevExpress.DataAccess.Sql.SqlDataSource result = new DevExpress.DataAccess.Sql.SqlDataSource(connectionParameters);
            ProcedureExecute proc = new ProcedureExecute("prc_CustomReport");
            proc.AddVarcharPara("@Action", 100, "GetSqlQuery");
            proc.AddPara("@id", Request.QueryString["id"]);
            DataSet ds = proc.GetDataSet();

            result.Queries.Add(new CustomSqlQuery("Company", Convert.ToString(ds.Tables[0].Rows[0][0])));
            result.Queries.Add(new CustomSqlQuery("Document", Convert.ToString(ds.Tables[1].Rows[0][0])));
           // result.Queries.Add(new CustomSqlQuery("Document", "select  id,[Customer Type], [Document Date], [Document No], [Due Amount], [Invoice Amount], [Payment %], [Payment], [vehicle type]  from tbl_Udtform_MyModule   where id=2"));

            result.RebuildResultSchema();
            return result;
        }

        protected void ASPxReportDesigner1_SaveReportLayout(object sender, DevExpress.XtraReports.Web.SaveReportLayoutEventArgs e)
        {
            byte[] layout = e.ReportLayout;
            ProcedureExecute proc = new ProcedureExecute("prc_CustomReport");
            proc.AddVarcharPara("@Action", 100, "SaveLayout");
            proc.AddPara("@id", Request.QueryString["id"]);
            proc.AddPara("@layout", layout);
            proc.RunActionQuery();
        }


    }
}