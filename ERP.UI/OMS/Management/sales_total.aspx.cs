using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Configuration;

namespace ERP.OMS.Management
{
    public partial class management_sales_total : System.Web.UI.Page
    {
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        public string pageAccess = "";

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
                BindGrid();
            }

        }
        public void BindGrid()
        {
            DataTable dt = new DataTable();
            DataTable dt1 = new DataTable();
            DataColumn col1 = new DataColumn("Total");
            DataColumn col2 = new DataColumn("ConvenExp");
            DataColumn col3 = new DataColumn("TravExp");
            DataColumn col4 = new DataColumn("LodgingExp");
            DataColumn col5 = new DataColumn("FoodingExp");
            DataColumn col6 = new DataColumn("BpExp");
            DataColumn col7 = new DataColumn("OtherExp");

            dt.Columns.Add(col1);
            dt.Columns.Add(col2);
            dt.Columns.Add(col3);
            dt.Columns.Add(col4);
            dt.Columns.Add(col5);
            dt.Columns.Add(col6);
            dt.Columns.Add(col7);
            dt1 = oDBEngine.GetDataTable("tbl_trans_salesExpenditure", "sum(expnd_travExpence) as ConExp,sum(expnd_travTravExpence) as TravExp,sum(expnd_LodgingExpnseAmount) as LodgingExp,sum(expnd_FoodExpnseAmount) as FoodingExp,sum(expnd_BPExpAmount) as BPExp,sum(expnd_otherExpnseAmount) as OtherExp", " expnd_empId='" + Session["SalesVisitID"].ToString() + "'");
            if (dt1.Rows.Count != 0)
            {
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    double conv = 0;
                    double trav = 0;
                    double lodging = 0;
                    double fooding = 0;
                    double bp = 0;
                    double other = 0;
                    DataRow RowNew = dt.NewRow();
                    RowNew["ConvenExp"] = dt1.Rows[i]["ConExp"].ToString();
                    try
                    {
                        conv = Convert.ToDouble(dt1.Rows[i]["ConExp"].ToString());
                    }
                    catch
                    {
                        conv = 0;
                    }
                    RowNew["TravExp"] = dt1.Rows[i]["TravExp"].ToString();
                    try
                    {
                        trav = Convert.ToDouble(dt1.Rows[i]["TravExp"].ToString());
                    }
                    catch
                    {
                        trav = 0;
                    }
                    RowNew["LodgingExp"] = dt1.Rows[i]["LodgingExp"].ToString();
                    try
                    {
                        lodging = Convert.ToDouble(dt1.Rows[i]["LodgingExp"].ToString());
                    }
                    catch
                    {
                        lodging = 0;
                    }
                    RowNew["FoodingExp"] = dt1.Rows[i]["FoodingExp"].ToString();
                    try
                    {
                        fooding = Convert.ToDouble(dt1.Rows[i]["FoodingExp"].ToString());
                    }
                    catch
                    {
                        fooding = 0;
                    }
                    RowNew["BpExp"] = dt1.Rows[i]["BPExp"].ToString();
                    try
                    {
                        bp = Convert.ToDouble(dt1.Rows[i]["BPExp"].ToString());
                    }
                    catch
                    {
                        bp = 0;
                    }
                    RowNew["OtherExp"] = dt1.Rows[i]["OtherExp"].ToString();
                    try
                    {
                        other = Convert.ToDouble(dt1.Rows[i]["OtherExp"].ToString());
                    }
                    catch
                    {
                        other = 0;
                    }
                    double total = conv + trav + lodging + fooding + bp + other;
                    RowNew["Total"] = total;
                    dt.Rows.Add(RowNew);
                }
            }
            gridTotal.DataSource = dt;
            gridTotal.DataBind();
        }
    }
}