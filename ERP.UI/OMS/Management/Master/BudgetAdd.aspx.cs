using BusinessLogicLayer.Budget;
using DataAccessLayer;
using DevExpress.Web;
using DevExpress.Web.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace ERP.OMS.Management.Master
{

    public partial class BudgetAdd : ERP.OMS.ViewState_class.VSPage
    {

        Customerbudget model = new Customerbudget();
        string FinYear = String.Empty;
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        protected void Page_PreInit(object sender, EventArgs e) // lead add
        {

            if (!IsPostBack)
            {
                string sPath = Convert.ToString(HttpContext.Current.Request.Url);
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }
        
        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (Session["LastFinYear"] != null)
            {
                FinYear = Convert.ToString(Session["LastFinYear"]).Trim();
            }
            if (!IsPostBack)
            {
                ProductDataSource.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
               
                Session["QuotationDetails1"] = null;
                BindproductClass();
                if (Request.QueryString["Cusid"] != null && Request.QueryString["productclassid"] != null)
                {
                    GetProductwiseBudget(Request.QueryString["Cusid"], FinYear, Request.QueryString["productclassid"]);
                    gridproductclass.Value = Request.QueryString["productclassid"];

                }
                else if (Request.QueryString["Cusid"] != null && Request.QueryString["productclassid"] == null)
                {
                    GetProductwiseBudget(Request.QueryString["Cusid"], FinYear,null);
                }

              
                  
                

                hdnchkgridbatch.Value = "New";
                //ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "OnAddNewClick()", true);



                this.Session["CustomerID"] = Request.QueryString["Cusid"];
            }
        }

       
        public void GetProductwiseBudget(string CustomerId, string FinYear,string productclassid)
        {
            DataTable dtdetails =  GetCustomerbudgetData(CustomerId, FinYear,productclassid).Tables[0];
            if (dtdetails.Rows.Count > 0)
            {
                txt_qtyfinyr.Text = dtdetails.Rows[0]["Qty_CurrentFY"].ToString();
                gridproductclass.Value = dtdetails.Rows[0]["ProductClass_Id"].ToString();
            }
            //grid1.DataSource = dtdetails;
            //grid1.DataBind();

            Session["QuotationDetails1"] = dtdetails;

        }
        public void BindproductClass()
        {
            DataTable dtclass = new DataTable();
            dtclass = model.GetProductClassdetailsBudget(Request.QueryString["Cusid"]);
            if (dtclass.Rows.Count > 0)
            {
                divbudget.Attributes.Add("style", "display:block");
                divmsg.Attributes.Add("style", "display:none");
                gridproductclass.DataSource = dtclass;
                gridproductclass.TextField = "ProductClass";
                gridproductclass.ValueField = "Id";
                gridproductclass.DataBind();
            }
            else
            {
                divbudget.Attributes.Add("style", "display:none");
                divmsg.Attributes.Add("style", "display:block");
                gridproductclass.DataSource = null;
                gridproductclass.DataBind();
            }
        }
        public DataSet GetCustomerbudgetData(string CustomerId, string FinYear, string productclassid)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Sp_GetbudgetdataCustomerwise");
            proc.AddPara("@CustomerID", CustomerId);
            proc.AddPara("@Finyear", FinYear);
            proc.AddPara("@ProductClass_ID", productclassid);
            ds = proc.GetDataSet();
            return ds;
        }
        protected void acpCrossBtn_Callback(object sender, CallbackEventArgsBase e)
        {
            int i2 = 0;
            if ( Request.QueryString["Cusid"] != null || Request.QueryString["productclassid"] != null)
            {
                decimal Qty_Permonth = (Convert.ToDecimal(txt_qtyfinyr.Text.Trim())/12);
                if (Request.QueryString["Type"] == "Customer")
                {
                    string slsId = "0";
                    i2 = InsertSalesmanBudget(Request.QueryString["Cusid"], slsId, (gridproductclass.Value).ToString(), txt_qtyfinyr.Text.Trim(), "0", Qty_Permonth.ToString(), HttpContext.Current.Session["userid"].ToString(), HttpContext.Current.Session["userid"].ToString(), FinYear.ToString());
                }
                else
                {
                i2 = InsertSalesmanBudget(Request.QueryString["Cusid"], Request.QueryString["slsid"], Request.QueryString["productclassid"], txt_qtyfinyr.Text.Trim(), "0", Qty_Permonth.ToString(), HttpContext.Current.Session["userid"].ToString(), HttpContext.Current.Session["userid"].ToString(), FinYear.ToString());
                }
            }


            if (i2 > 0)
            {

                if (Request.QueryString["Cusid"] != null && Request.QueryString["productclassid"] != null)
                {
                    GetProductwiseBudget(Request.QueryString["Cusid"], FinYear, Request.QueryString["productclassid"]);

                }
                else if (Request.QueryString["Cusid"] != null && Request.QueryString["productclassid"] == null)
                {
                    GetProductwiseBudget(Request.QueryString["Cusid"], FinYear, null);
                }
                //grid1.JSProperties["cpSaveSuccessOrFail"] = "Success";
                DataTable dt = (DataTable)Session["QuotationDetails1"];
                dt.Rows.Clear();
                //Session["QuotationDetails1"] = null;
            }

        }

      

        public int InsertCustomerBudget(string CustomerID, string slsid, string ProductClass_Id, string Qty_CurrentFY, string Qty_PreviousFY, string Qty_Permonth, string CreatedBy, string Updatedby, string FinYear)
        {
            DataSet dsInst = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Sp_BudgetInsertion");
            proc.AddPara("@CustomerID", CustomerID);
            proc.AddPara("@sls_id", slsid);
            proc.AddPara("@ProductClass_Id", ProductClass_Id);
            proc.AddPara("@Qty_CurrentFY", Qty_CurrentFY);
            proc.AddPara("@Qty_PreviousFY", Qty_PreviousFY);
            proc.AddPara("@Qty_Permonth", Qty_Permonth);
            proc.AddPara("@CreatedBy", CreatedBy);
            proc.AddPara("@Updatedby", Updatedby);
            proc.AddPara("@FiscalYear", FinYear);
            return proc.RunActionQuery();
        }


        public int InsertSalesmanBudget(string CustomerID, string slsid, string ProductClass_Id, string Qty_CurrentFY, string Qty_PreviousFY, string Qty_Permonth, string CreatedBy, string Updatedby, string FinYear)
        {
            DataSet dsInst = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Sp_BudgetInsertion");
            proc.AddPara("@CustomerID", CustomerID);
            proc.AddPara("@sls_id", slsid);
            proc.AddPara("@ProductClass_Id", ProductClass_Id);
            proc.AddPara("@Qty_CurrentFY", Qty_CurrentFY);
            proc.AddPara("@Qty_PreviousFY", Qty_PreviousFY);
            proc.AddPara("@Qty_Permonth", Qty_Permonth);
            proc.AddPara("@CreatedBy", CreatedBy);
            proc.AddPara("@Updatedby", Updatedby);
            proc.AddPara("@FiscalYear", FinYear);
            proc.AddPara("@Action", "AddSalesmanBudget");
            return proc.RunActionQuery();
        }
         /*Abhisek
        public void ProductBind()
        {
            productLookUp.DataSource = model.GetProductDetails(Request.QueryString["Cusid"]);
            productLookUp.DataBind();
        }
        */
         /*Abhisek
      protected void grid1_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
      {

          //  GetProductwiseBudget(Request.QueryString["Cusid"]);

          string strSplitCommand = e.Parameters.Split('~')[0];

          if (strSplitCommand == "BindGridOnQuotation")
          {
              string command = e.Parameters.ToString();
            //  string State = Convert.ToString(e.Parameters.Split('~')[1]);
              
              string ComponentDetailsIDs = string.Empty;


              for (int i = 0; i < grid_Products1.GetSelectedFieldValues("ProductID").Count; i++)
              {
                  ComponentDetailsIDs += "," + Convert.ToString(grid_Products1.GetSelectedFieldValues("ProductID")[i]);
              }


              ComponentDetailsIDs = ComponentDetailsIDs.TrimStart(',');


              //string strInvoiceID = Convert.ToString(Session["SI_InvoiceID"]);
              //if (Session["ReplaceMentNotesDetails"] != null)
              //{
              //    Session["ReplaceMentNotesDetails"] = null;
              //}



              //DataTable dt_QuotationDetails = new DataTable();
              //dt_QuotationDetails.Clear();
              //dt_QuotationDetails = model.GetProductDetailsbypructandproductclass(ComponentDetailsIDs);

              //Session["ReplaceMentNotesDetails"] = dt_QuotationDetails;
              Session["QuotationDetails1"] = model.GetProductDetailsbypructandproductclass(ComponentDetailsIDs, txt_qtyfinyr.Text, Request.QueryString["Cusid"]).Tables[0];
              grid1.DataSource = model.GetProductDetailsbypructandproductclass(ComponentDetailsIDs, txt_qtyfinyr.Text, Request.QueryString["Cusid"]).Tables[0];
              grid1.DataBind();
               
          }


      }
      */
          /*Abhisek
        protected void grid1_DataBinding(object sender, EventArgs e)
        {
            if (Session["QuotationDetails1"] != null)
            {
                DataTable dt = (DataTable)Session["QuotationDetails1"];
                grid1.DataSource = dt;
                //      grid1.DataBind();
            }

          
        }
        */
        //protected void grid1_BatchUpdate(object sender, DevExpress.Web.Data.ASPxDataBatchUpdateEventArgs e)
        //{

        //    DataTable batchbudget = new DataTable();
        //    batchbudget.Clear();
        //    //  string IsDeleteFrom = Convert.ToString(hdfIsDelete.Value);

        //    string validate = "";


        //    batchbudget.Columns.Add("BudgetId", typeof(string));
        //    batchbudget.Columns.Add("CustomerId", typeof(string));
        //    batchbudget.Columns.Add("ProductID", typeof(string));
        //    batchbudget.Columns.Add("Qty_CurrentFY", typeof(string));
        //    batchbudget.Columns.Add("Qty_PreviousFY", typeof(string));
        //    batchbudget.Columns.Add("Qty_Permonth", typeof(string));
        //    batchbudget.Columns.Add("ProductName", typeof(string));
        //    batchbudget.Columns.Add("Description", typeof(string));
        //    batchbudget.Columns.Add("UOM", typeof(string));
        //    batchbudget.Columns.Add("Industry", typeof(string));
        //    batchbudget.Columns.Add("Productclass", typeof(string));



        //    if (Session["QuotationDetails1"] != null)
        //    {
        //        DataTable dt1 = new DataTable();
        //        dt1 = (DataTable)Session["QuotationDetails1"];




        //        for (int i = 0; i < dt1.Rows.Count; i++)
        //        {

        //            string[] ProductDetailsList = Convert.ToString(dt1.Rows[i]["ProductID"]).Split(new string[] { "||@||" }, StringSplitOptions.None);
        //            string ProductID = ProductDetailsList[0];

        //            batchbudget.Rows.Add(
        //                Convert.ToString(dt1.Rows[i]["BudgetId"]),
        //                    Convert.ToString(dt1.Rows[i]["CustomerId"]),
        //                      Convert.ToString(ProductID),
        //                     Convert.ToString(dt1.Rows[i]["Qty_CurrentFY"]),
        //                     Convert.ToString(dt1.Rows[i]["Qty_PreviousFY"]),
        //                      Convert.ToString(dt1.Rows[i]["Qty_Permonth"]),
        //                    Convert.ToString(dt1.Rows[i]["ProductName"]),
        //                    Convert.ToString(dt1.Rows[i]["Description"]),
        //                    Convert.ToString(dt1.Rows[i]["UOM"]),
        //                    Convert.ToString(dt1.Rows[i]["Industry"]),
        //                    Convert.ToString(dt1.Rows[i]["Productclass"])

        //                );
        //        }

        //    }
        //    else
        //    {
        //        //batchbudget.Columns.Add("BudgetId", typeof(string));
        //        //batchbudget.Columns.Add("CustomerId", typeof(string));
        //        //batchbudget.Columns.Add("ProductId", typeof(string));
        //        //batchbudget.Columns.Add("Qty_CurrentFY", typeof(string));
        //        //batchbudget.Columns.Add("Qty_PreviousFY", typeof(string));
        //        //batchbudget.Columns.Add("Qty_Permonth", typeof(string));
        //        //batchbudget.Columns.Add("ProductName", typeof(string));
        //        //batchbudget.Columns.Add("Description", typeof(string));
        //        //batchbudget.Columns.Add("UOM", typeof(string));
        //        //batchbudget.Columns.Add("Industry", typeof(string));
        //        //batchbudget.Columns.Add("Productclass", typeof(string));

        //    }

        //    foreach (var args in e.InsertValues)
        //    {
        //        string ProductDetails = Convert.ToString(args.NewValues["ProductID"]);

        //        if (ProductDetails != "" && ProductDetails != "0")
        //        {
        //            string BudgetId = Convert.ToString(args.NewValues["BudgetId"]);

        //            string[] ProductDetailsList = ProductDetails.Split(new string[] { "||@||" }, StringSplitOptions.None);
        //            string ProductID = ProductDetailsList[0];

        //            string ProductName = Convert.ToString(args.NewValues["ProductName"]);
        //            string Description = Convert.ToString(args.NewValues["Description"]);
        //            string UOM = Convert.ToString(args.NewValues["UOM"]);
        //            string Industry = Convert.ToString(args.NewValues["Industry"]);
        //            string Productclass = Convert.ToString(args.NewValues["Productclass"]);
        //            string Qty_CurrentFY = Convert.ToString(args.NewValues["Qty_CurrentFY"]);
        //            string Qty_PreviousFY = Convert.ToString(args.NewValues["Qty_PreviousFY"]);
        //            string Qty_Permonth = Convert.ToString(args.NewValues["Qty_Permonth"]);


        //            string CustomerId = Request.QueryString["Cusid"];


        //            batchbudget.Rows.Add(BudgetId, CustomerId, ProductID, Qty_CurrentFY, Qty_PreviousFY, Qty_Permonth, ProductName, Description, UOM, Industry, Productclass);


        //        }
        //    }

        //    foreach (var args in e.UpdateValues)
        //    {

        //        string BudgetId = Convert.ToString(args.Keys["BudgetId"]);
        //        string ProductDetails = Convert.ToString(args.NewValues["ProductID"]);

        //        bool isDeleted = false;


        //        foreach (var arg in e.DeleteValues)
        //        {
        //            string DeleteID = Convert.ToString(arg.Keys["BudgetId"]);

        //            if (DeleteID == BudgetId)
        //            {
        //                isDeleted = true;
        //                break;
        //            }
        //        }

        //        if (isDeleted == false)
        //        {
        //            if (ProductDetails != "" && ProductDetails != "0")
        //            {
        //                string[] ProductDetailsList = ProductDetails.Split(new string[] { "||@||" }, StringSplitOptions.None);
        //                string ProductID = Convert.ToString(ProductDetailsList[0]);

        //                string ProductName = Convert.ToString(args.NewValues["ProductName"]);
        //                string Description = Convert.ToString(args.NewValues["Description"]);
        //                string UOM = Convert.ToString(args.NewValues["UOM"]);
        //                string Industry = Convert.ToString(args.NewValues["Industry"]);
        //                string Productclass = Convert.ToString(args.NewValues["Productclass"]);
        //                string Qty_CurrentFY = Convert.ToString(args.NewValues["Qty_CurrentFY"]);
        //                string Qty_PreviousFY = Convert.ToString(args.NewValues["Qty_PreviousFY"]);
        //                string Qty_Permonth = Convert.ToString(args.NewValues["Qty_Permonth"]);
        //                string CustomerId = Request.QueryString["Cusid"];


        //                bool Isexists = false;
        //                foreach (DataRow drr in batchbudget.Rows)
        //                {
        //                    string OldBudgetId = Convert.ToString(drr["BudgetId"]);

        //                    if (OldBudgetId == BudgetId)
        //                    {

        //                        Isexists = true;

        //                        drr["ProductID"] = ProductDetails;
        //                        drr["Description"] = Description;
        //                        drr["UOM"] = UOM;
        //                        drr["UOM"] = UOM;
        //                        drr["Industry"] = Industry;
        //                        drr["Productclass"] = Productclass;
        //                        drr["Qty_CurrentFY"] = Qty_CurrentFY;
        //                        drr["Qty_PreviousFY"] = Qty_PreviousFY;
        //                        drr["Qty_Permonth"] = Qty_Permonth;
        //                        drr["ProductName"] = ProductName;

        //                        break;
        //                    }
        //                }

        //                if (Isexists == false)
        //                {
        //                    batchbudget.Rows.Add(BudgetId, CustomerId, ProductID, Qty_CurrentFY, Qty_PreviousFY, Qty_Permonth, ProductName, Description, UOM, Industry, Productclass);
        //                }
        //            }
        //        }
        //    }





        //    foreach (var args in e.DeleteValues)
        //    {
        //        string BudgetId = Convert.ToString(args.Keys["BudgetId"]);



        //        string SrlNo = "";

        //        for (int i = batchbudget.Rows.Count - 1; i >= 0; i--)
        //        {

        //            DataRow dr = batchbudget.Rows[i];
        //            string delQuotationID = Convert.ToString(dr["BudgetId"]);

        //            DataRow dr1 = batchbudget.Rows[i];
        //            if (dr["BudgetId"] == BudgetId)
        //                dr.Delete();


        //            if (delQuotationID == BudgetId)
        //            {
        //                BudgetId = Convert.ToString(dr["BudgetId"]);
        //                dr.Delete();
        //            }



        //        }
        //        batchbudget.AcceptChanges();

        //    }



        //    // batchbudget.AcceptChanges();
        //    Session["QuotationDetails1"] = batchbudget;

        //    List<BudgetCustomer> budgetlist = new List<BudgetCustomer>();



        //    for (int i = 0; i < batchbudget.Rows.Count; i++)
        //    {



        //        string[] ProductDetailsList = batchbudget.Rows[i]["ProductID"].ToString().Split(new string[] { "||@||" }, StringSplitOptions.None);
        //        string ProductID = Convert.ToString(ProductDetailsList[0]);
        //        decimal Previousyear = 0;
        //        decimal CurrentYear = 0;
        //        decimal Permonth = 0;
        //        if (Convert.ToString(batchbudget.Rows[i]["Qty_PreviousFY"]) != "")
        //        {
        //            Previousyear = Convert.ToDecimal(batchbudget.Rows[i]["Qty_PreviousFY"]);
        //        }
        //        else
        //        {
        //            Previousyear = 0;
        //        }
        //        CurrentYear = (Convert.ToString(batchbudget.Rows[i]["Qty_CurrentFY"]) != "") ? Convert.ToDecimal(batchbudget.Rows[i]["Qty_CurrentFY"]) : 0;
        //        Permonth = (Convert.ToString(batchbudget.Rows[i]["Qty_Permonth"]) != "") ? Convert.ToDecimal(batchbudget.Rows[i]["Qty_Permonth"]) : 0;
        //        budgetlist.Add(new BudgetCustomer()
        //            {

        //                CustomerId = Convert.ToInt64(batchbudget.Rows[i]["CustomerId"]),
        //                ProductId = Convert.ToInt64(ProductID),
        //                Qty_CurrentFY = CurrentYear,
        //                Qty_PreviousFY = Previousyear,
        //                Qty_Permonth = Permonth,
        //                CreatedBy = Convert.ToInt32(HttpContext.Current.Session["userid"]),
        //                FiscalYear = FinYear
        //            });


        //    }

        //    var duplicateExists = budgetlist.GroupBy(n => n.ProductId).Any(g => g.Count() > 1);
        //    if (duplicateExists == false)
        //    {
        //        if (budgetlist != null)
        //        {

        //            string budgetXML = Customerbudget.ConvertToXml(budgetlist, 0);
        //            int i2 = 0;
        //            if (Request.QueryString["slsid"] != null)
        //            {
        //                i2 = model.InsertCustomerBudget(budgetXML, Request.QueryString["Cusid"], Request.QueryString["slsid"]);
        //            }
        //            else
        //            {
        //                i2 = model.InsertCustomerBudget(budgetXML, Request.QueryString["Cusid"]);
        //            }

        //            if (i2 > 0)
        //            {

        //                if (Request.QueryString["Cusid"] != null && Request.QueryString["productclassid"] != null)
        //                {
        //                    GetProductwiseBudget(Request.QueryString["Cusid"], FinYear, Request.QueryString["productclassid"]);

        //                }
        //                else if (Request.QueryString["Cusid"] != null && Request.QueryString["productclassid"] == null)
        //                {
        //                    GetProductwiseBudget(Request.QueryString["Cusid"], FinYear, null);
        //                }
        //                grid1.JSProperties["cpSaveSuccessOrFail"] = "Success";
        //                DataTable dt = (DataTable)Session["QuotationDetails1"];
        //                dt.Rows.Clear();
        //                //Session["QuotationDetails1"] = null;


        //            }
        //            else
        //            {
        //                grid1.JSProperties["cpSaveSuccessOrFail"] = "Error";
        //            }
        //        }
        //    }
        //    else
        //    {

        //        grid1.JSProperties["cpSaveSuccessOrFail"] = "Duplicate";

        //        Session["QuotationDetails1"] = null;

        //        if (Request.QueryString["Cusid"] != null && Request.QueryString["productclassid"] != null)
        //        {
        //            GetProductwiseBudget(Request.QueryString["Cusid"], FinYear, Request.QueryString["productclassid"]);

        //        }
        //        else if (Request.QueryString["Cusid"] != null && Request.QueryString["productclassid"] == null)
        //        {
        //            GetProductwiseBudget(Request.QueryString["Cusid"], FinYear, null);
        //        }
        //    }
        //    //Response.Redirect("frmContactMain.aspx");
        //    //Session["QuotationDetails1"] = null;

        //}
      
        
          /*Abhisek
        protected void Grid1_RowInserting(object sender, ASPxDataInsertingEventArgs e)
        {
            e.Cancel = true;
        }
        protected void Grid1_RowUpdating(object sender, ASPxDataUpdatingEventArgs e)
        {
            e.Cancel = true;
        }
        protected void Grid1_RowDeleting(object sender, ASPxDataDeletingEventArgs e)
        {
            e.Cancel = true;
        }
        */
        /* Abhisek
        protected void cgridProducts_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            string strSplitCommand = e.Parameters.Split('~')[0];
            if (strSplitCommand == "BindProductsDetails")
            {
                string QuoComponent = Convert.ToString(gridproductclass.Value);

                DataTable dtDetails = new DataTable();
                dtDetails = model.GetProductDetailsClasswise(QuoComponent, Request.QueryString["Cusid"]);
                if (dtDetails.Rows.Count > 0)
                {
                    grid_Products1.DataSource = dtDetails;
                    grid_Products1.DataBind();

                }
                //grid_Products.DataSource = dtDetails;
                //grid_Products.DataBind();
            }


            if (strSplitCommand == "SelectAndDeSelectProducts")
            {
                ASPxGridView gv = sender as ASPxGridView;
                string command = e.Parameters.ToString();
                string State = Convert.ToString(e.Parameters.Split('~')[1]);
                if (State == "SelectAll")
                {
                    for (int i = 0; i < gv.VisibleRowCount; i++)
                    {
                        gv.Selection.SelectRow(i);
                    }
                }
                if (State == "UnSelectAll")
                {
                    for (int i = 0; i < gv.VisibleRowCount; i++)
                    {
                        gv.Selection.UnselectRow(i);
                    }
                }
                if (State == "Revart")
                {
                    for (int i = 0; i < gv.VisibleRowCount; i++)
                    {
                        if (gv.Selection.IsRowSelected(i))
                            gv.Selection.UnselectRow(i);
                        else
                            gv.Selection.SelectRow(i);
                    }
                }
            }
        }
        */


       
    }
}