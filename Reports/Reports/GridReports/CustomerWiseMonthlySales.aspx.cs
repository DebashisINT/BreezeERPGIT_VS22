using DevExpress.Export;
using DevExpress.Web;
using DevExpress.XtraPrinting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Reports.Reports.GridReports
{
    public partial class CustomerWiseMonthlySales : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                string GridHeader = "";
                BusinessLogicLayer.Reports GridHeaderDet = new BusinessLogicLayer.Reports();
                RptHeading.Text = "Customer Wise Monthly Sales Report";
                GridHeader = GridHeaderDet.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), true, false, false, false, false, false);
                CompName.Text = GridHeader;

                GridHeader = GridHeaderDet.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), false, true, false, false, false, false);
                CompAdd.Text = GridHeader;

                GridHeader = GridHeaderDet.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), false, false, true, false, false, false);
                CompOth.Text = GridHeader;

                GridHeader = GridHeaderDet.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), false, false, false, true, false, false);
                CompPh.Text = GridHeader;

                GridHeader = GridHeaderDet.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), false, false, false, false, false, true);
                CompAccPrd.Text = GridHeader;

                Session["Salesman"] = null;
                Session["Industries"] = null;
                Session["Products"] = null;
                //Rev Subhra  0017670   12-12-2018
                //DataTable dt = new DataTable();
                //dt.Columns.Add("Customer Name", typeof(System.String));
                //dt.Columns.Add("Industry", typeof(System.String));
                //dt.Columns.Add("Product", typeof(System.String));
                //dt.Columns.Add("Budget Details", typeof(System.Decimal));
                //dt.Columns.Add("Salesman Name", typeof(System.String));
                //ShowGrid.DataSource = dt;
                //ShowGrid.DataBind();

                //End of Rev
            }
        }

        #region Salesman Populate

        protected void Salesman_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            string FinYear = Convert.ToString(Session["LastFinYear"]);

            if (e.Parameter.Split('~')[0] == "Salesman")
            {
                DataTable ComponentTable = new DataTable();

                //Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]), Hoid
                ComponentTable = GetSalesman();

                if (ComponentTable.Rows.Count > 0)
                {
                    lookup_salesman.DataSource = ComponentTable;
                    lookup_salesman.DataBind();
                }

                else
                {
                    lookup_salesman.DataSource = null;
                    lookup_salesman.DataBind();
                }
            }
        }

        public DataTable GetSalesman()
        {

            DataTable dt = new DataTable();
            //SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);
            SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            SqlCommand cmd = new SqlCommand("Get_CustomerwiseMonthlySale", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Action", "Salesman");
            cmd.CommandTimeout = 0;
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(dt);
            cmd.Dispose();
            con.Dispose();
            return dt;

        }

        protected void lookup_salesman_DataBinding(object sender, EventArgs e)
        {
            DataTable ComponentTable = new DataTable();
            ComponentTable = GetSalesman();

            if (ComponentTable.Rows.Count > 0)
            {
                Session["SI_ComponentData_Branch"] = ComponentTable;

                lookup_salesman.DataSource = ComponentTable;

            }

        }

        #endregion

        #region Industry Populate

        protected void Industry_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            string FinYear = Convert.ToString(Session["LastFinYear"]);

            if (e.Parameter.Split('~')[0] == "Industry")
            {
                DataTable ComponentTable = new DataTable();

                ComponentTable = GetIndustry();

                if (ComponentTable.Rows.Count > 0)
                {
                    lookup_Industry.DataSource = ComponentTable;
                    lookup_Industry.DataBind();
                }

                else
                {
                    lookup_Industry.DataSource = null;
                    lookup_Industry.DataBind();

                }
            }
        }


        public DataTable GetIndustry()
        {
            DataTable dt = new DataTable();
            //SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);
            SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            SqlCommand cmd = new SqlCommand("Get_CustomerwiseMonthlySale", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Action", "Industry");

            cmd.CommandTimeout = 0;
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(dt);
            cmd.Dispose();
            con.Dispose();

            return dt;
        }

        protected void lookup_industry_DataBinding(object sender, EventArgs e)
        {
            DataTable ComponentTable = new DataTable();
            ComponentTable = GetIndustry();

            if (ComponentTable.Rows.Count > 0)
            {
                lookup_Industry.DataSource = ComponentTable;
              

            }

        }

        #endregion

        #region Product Populate

        //protected void Product_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        //{
        //    string FinYear = Convert.ToString(Session["LastFinYear"]);

        //    if (e.Parameter.Split('~')[0] == "Products")
        //    {
        //        DataTable ComponentTable = new DataTable();
        //        string Hoid = e.Parameter.Split('~')[1];

        //        //Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]), Hoid
        //        ComponentTable = GetSalesman();

        //        if (ComponentTable.Rows.Count > 0)
        //        {

        //            lookup_product.DataSource = ComponentTable;
        //            lookup_product.DataBind();

        //        }

        //        else
        //        {

        //            lookup_product.DataSource = null;
        //            lookup_product.DataBind();

        //        }
        //    }
        //}


        //public DataTable GetProduct()
        //{
        //    DataTable dt = new DataTable();
        //    //SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        //    SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
        //    SqlCommand cmd = new SqlCommand("Get_CustomerwiseMonthlySale", con);
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.Parameters.AddWithValue("@Action", "Product");
        //    cmd.CommandTimeout = 0;
        //    SqlDataAdapter da = new SqlDataAdapter();
        //    da.SelectCommand = cmd;
        //    da.Fill(dt);
        //    cmd.Dispose();
        //    con.Dispose();

        //    return dt;
        //}

        //protected void lookup_product_DataBinding(object sender, EventArgs e)
        //{
        //    DataTable ComponentTable = new DataTable();
        //    ComponentTable = GetProduct();

        //    if (ComponentTable.Rows.Count > 0)

        //        lookup_product.DataSource = ComponentTable;

        //}


        #endregion

        #region Grid Populate
        protected void Grid_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {

            DataTable ComponentTable = new DataTable();
            if (e.Parameters.Split('~')[0] == "DelailsGrid")
            {
                string salesman = e.Parameters.Split('~')[1];
                string Industries = e.Parameters.Split('~')[2];
                //string products = e.Parameters.Split('~')[3];
                string products = "";
                string classes="";
                products = hdncWiseProductId.Value;
                classes = hdnClassId.Value;

                Session["Salesman"] = salesman;
                Session["Industries"] = Industries;
                //Session["Products"] = products;
                //Rev Subhra  0017670   12-12-2018
                Session["Classes"] = classes;
                Session["Products"] = products;
                //End of Rev

                //Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]), Hoid
                ComponentTable = GetDetailsMonthly(salesman, Industries, classes,products);

                if (ComponentTable.Rows.Count > 0)
                {
                    ShowGrid.DataSource = ComponentTable;
                    ShowGrid.DataBind();
                }

                else
                {
                    ShowGrid.DataSource = null;
                    ShowGrid.DataBind();
                }
            }

        }


        public DataTable GetDetailsMonthly(string salesman, string Industries,string classes,string products)
        {
            DataTable dt = new DataTable();
            //SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);
            SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            SqlCommand cmd = new SqlCommand("Get_CustomerwiseMonthlySale", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Action", "DetailsBudget");
            cmd.Parameters.AddWithValue("@Finyear", Convert.ToString(Session["LastFinYear"]));
            cmd.Parameters.AddWithValue("@ProductIds", products);
            cmd.Parameters.AddWithValue("@IndustryIds", Industries);
            cmd.Parameters.AddWithValue("@SalesmanIds", salesman);
            cmd.Parameters.AddWithValue("@CLASS", classes);
           

            cmd.CommandTimeout = 0;
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(dt);
            cmd.Dispose();
            con.Dispose();

            return dt;
        }

        protected void ShowGrid_DataBinding(object sender, EventArgs e)
        {

            if (Session["Salesman"] != null && Session["Industries"] != null)
            {
                string products = "";
                string classes = "";
                products = hdncWiseProductId.Value;
                classes = hdnClassId.Value;
                DataTable ComponentTable = new DataTable();
                //Rev Subhra  0017670   12-12-2018
                //ComponentTable = GetDetailsMonthly(Convert.ToString(Session["Salesman"]), Convert.ToString(Session["Industries"]),classes,products);
                ComponentTable = GetDetailsMonthly(Convert.ToString(Session["Salesman"]), Convert.ToString(Session["Industries"]), Convert.ToString(Session["Classes"]), Convert.ToString(Session["Products"]));
                //End of Rev

                if (ComponentTable.Rows.Count > 0)

                    ShowGrid.DataSource = ComponentTable;

               
              
            }
        }
        protected void ShowGrid_DataBound(object sender, EventArgs e)
        {
            if (Session["Salesman"] != null && Session["Industries"] != null)
           {
            //    ShowGrid.Columns["sls_contactlead_id"].Visible = false;
            //    ShowGrid.Columns["act_industryid"].Visible = false;
            //    ShowGrid.Columns["yr"].Visible = false;
            //    ShowGrid.Columns["act_assigned To"].Visible = false;



                ASPxGridView grid = (ASPxGridView)sender;
                foreach (GridViewDataColumn c in grid.Columns)
                {
                    if ((c.FieldName.ToString()).StartsWith("Customer_Id"))
                    {
                        c.Visible = false;
                    }


                    if ((c.FieldName.ToString()).StartsWith("act_assignedTo"))
                    {
                        c.Visible = false;
                    }
           

                    if ((c.FieldName.ToString()).StartsWith("Customer Name"))
                    {
                        c.Width = 150;
                    }
                    if ((c.FieldName.ToString()).StartsWith("Product"))
                    {
                        c.Width = 150;
                    }
                    if ((c.FieldName.ToString()).StartsWith("Industry"))
                    {
                        c.Width = 100;
                    }

                    if ((c.FieldName.ToString()).StartsWith("Budget Details"))
                    {
                        c.Width = 100;
                    }

                    //Rev Subhra  0017670   12-12-2018
                    if ((c.FieldName.ToString()).StartsWith("Salesman Name"))
                    {
                        c.Width = 180;
                    }

                    //End of Rev
                }
            }
        }

        #endregion

        #region  Export Customer Monthly Report
        public void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(Convert.ToString(drdExport.SelectedItem.Value));
            if (Filter != 0)
            {
                bindexport(Filter);
            }

        }
        public void bindexport(int Filter)
        {
            string filename = "Customer Wise Monthly Report";
            exporter.FileName = filename;
            string FileHeader = "";

            exporter.FileName = filename;

            exporter.GridViewID = "ShowGrid";
            switch (Filter)
            {
                case 1:
                    exporter.WritePdfToResponse();

                    break;
                case 2:
                    exporter.WriteXlsxToResponse(new XlsxExportOptionsEx() { ExportType = ExportType.WYSIWYG });
                    break;
                case 3:
                    exporter.WriteRtfToResponse();
                    break;
                case 4:
                    exporter.WriteCsvToResponse();
                    break;
            }

        }

        #endregion

    }









}