using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Payroll.Controllers.HRPayroll
{
    public class ReportDesignerController : Controller
    {
        //
        // GET: /ReportDesigner/
        public ActionResult Index()
        {
            DevExpress.XtraReports.UI.XtraReport obj = new DevExpress.XtraReports.UI.XtraReport();

            DataSet ds = new DataSet();

            String con = Convert.ToString(Session["ErpConnection"]);
            SqlCommand sqlcmd = new SqlCommand();
            SqlConnection sqlcon = new SqlConnection(con);
            sqlcon.Open();
            sqlcmd = new SqlCommand("PRC_PAYROLL_PAYSLIP", sqlcon);
            sqlcmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
            da.Fill(ds);
            sqlcon.Close();


            obj.DataSource = ds;

            return View(@"/Views/HRPayroll/ReportDesigner/Index.cshtml", obj);
        }
    }
}