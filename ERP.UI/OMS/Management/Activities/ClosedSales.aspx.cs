using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//using DevExpress.Web.ASPxTabControl;
using System.Web.Services;
using System.Collections.Generic;
using BusinessLogicLayer;
using ClsDropDownlistNameSpace;
using ERP.OMS.CustomFunctions;
using System.Text;
using System.Reflection;
using DevExpress.Web;
using EntityLayer.CommonELS;
using BusinessLogicLayer.Budget;
using DataAccessLayer;
namespace ERP.OMS.Management.Activities
{
    public partial class management_Activities_ClosedSales : ERP.OMS.ViewState_class.VSPage// System.Web.UI.Page
    {

        string FinYear = String.Empty;
        Customerbudget model = new Customerbudget();
        SlaesActivitiesBL objAc = new SlaesActivitiesBL();
        public string pageAccess = "";
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        BusinessLogicLayer.Others objBL = new BusinessLogicLayer.Others();
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        DateTime dtFrom;
        DateTime dtTo;
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
               //Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }

            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Activities/crm_sales.aspx");

            if (Session["LastFinYear"] != null)
            {
                FinYear = Convert.ToString(Session["LastFinYear"]).Trim();
            }

            if (!IsPostBack)
            {

                string EditPermitValue = string.Empty;

                string cnt_id = Convert.ToString(Session["cntId"]);

                EditPermitValue = EditPermissionShow(cnt_id);

                if (rights.DocumentCollection)
                    { ASPxPageControl1.TabPages.FindByName("Document Collection").ClientEnabled = true; } else { ASPxPageControl1.TabPages.FindByName("Document Collection").ClientEnabled = false; }

                if (rights.FutureSales)
                    { ASPxPageControl1.TabPages.FindByName("Future Sales").ClientEnabled = true; } else { ASPxPageControl1.TabPages.FindByName("Future Sales").ClientEnabled = false; }

                if (rights.ClosedSales)
                    { ASPxPageControl1.TabPages.FindByName("Closed Sales").ClientEnabled = true; } else { ASPxPageControl1.TabPages.FindByName("Closed Sales").ClientEnabled = false; }

                if (rights.ClarificationRequired)
                    { ASPxPageControl1.TabPages.FindByName("Clarification Required").ClientEnabled = true; } else { ASPxPageControl1.TabPages.FindByName("Clarification Required").ClientEnabled = false; }


                /*if (EditPermitValue == "1")
                {


                    ASPxPageControl1.TabPages.FindByName("Document Collection").ClientEnabled = false;
                }
                else
                {

                    ASPxPageControl1.TabPages.FindByName("Document Collection").ClientEnabled = true;
                }*/
                Lrd.Attributes.Add("onclick", "All_CheckedChanged();");
                Erd.Attributes.Add("onclick", "Specific_CheckedChanged();");
                Session["exportval"] = null;
                SalesDetailsGrid.SettingsCookies.CookiesID = "BreeezeErpGridCookiesSalesClosedDetailsGrid";
                this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>addCookiesKeyOnStorage('BreeezeErpGridCookiesSalesClosedDetailsGrid');</script>");

                if (Request.QueryString["frmdate"] != null && Request.QueryString["todate"] != null)
                {
                    ASPxFromDate.Text = Request.QueryString["frmdate"];
                    ASPxToDate.Text = Request.QueryString["todate"];

                    dtFrom = Convert.ToDateTime(ASPxFromDate.Date);
                    dtTo = Convert.ToDateTime(ASPxToDate.Date);
                }
                else
                {
                    if (Session["Fromdate_Cd"] != null && Session["ToDate_Cd"] != null)
                    {

                        ASPxFromDate.Text = Convert.ToDateTime(Session["Fromdate_Cd"]).ToString("dd-MM-yyyy");
                        ASPxToDate.Text = Convert.ToDateTime(Session["ToDate_Cd"]).ToString("dd-MM-yyyy");

                        dtFrom = Convert.ToDateTime(ASPxFromDate.Date);
                        dtTo = Convert.ToDateTime(ASPxToDate.Date);
                    }



                    else
                    {
                        dtFrom = DateTime.Now;
                        dtTo = DateTime.Now;
                        ASPxFromDate.Text = dtFrom.ToString("dd-MM-yyyy");
                        ASPxToDate.Text = dtTo.ToString("dd-MM-yyyy");
                    }
                }
                BindGrid(dtFrom, dtTo);
            }
           
            Session["export"] = null;
           
           
            ////this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");
        }
        void BindGrid(DateTime FromDate, DateTime ToDate)
        {
            string cnt_internalId = Convert.ToString(HttpContext.Current.Session["owninternalid"]);//Session usercontactID


            DataTable dt = objAc.GetsalesActivity(cnt_internalId, "2", Convert.ToString(FromDate), Convert.ToString(ToDate));
            Session["GrdActivity"] = dt;

            SalesDetailsGrid.DataSource = dt;
            SalesDetailsGrid.DataBind();
        
        }
        //protected void ClosedSalesGrid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        //{
        //    BindGrid();
        //}

        protected void SalesDetailsGrid_DataBinding(object sender, EventArgs e)
        {

            SalesDetailsGrid.DataSource = (DataTable)Session["GrdActivity"];

        }
        protected void btMyPendingActivity_Click(object sender, EventArgs e)
        {
            if (Session["cntId"] != null)
            {
                int salsmanId = Convert.ToInt32(Session["cntId"]);




                string frmdate = Convert.ToDateTime(ASPxFromDate.Date).ToString("dd-MM-yyyy");
                string todate = Convert.ToDateTime(ASPxToDate.Date).ToString("dd-MM-yyyy");
                Response.Redirect("TodayTask.aspx?Id=2&frmdate=" + frmdate + "&todate=" + todate);
            }
            else
            {

                Response.Redirect("~/OMS/Management/ProjectMainPage.aspx");
            }
        }
        protected void btMyPendingTask_Click(object sender, EventArgs e)
        {
            if (Session["cntId"] != null)
            {
                int salsmanId = Convert.ToInt32(Session["cntId"]);
                Response.Redirect("../Master/PendingTaskReport.aspx?Salsmanid=" + salsmanId + "&returnId=5");
            }
            else
            {

                Response.Redirect("../CRMPhoneCallWithFrame.aspx");
            }
        }
        public string EditPermissionShow(string cntid)
        {


            string EditPermissionval = "0";
            try
            {
                BusinessLogicLayer.Others objOtherBL = new BusinessLogicLayer.Others();

                DataTable PermissionDt = new DataTable();
                PermissionDt = objOtherBL.GetSalesManDeactivateDocomentActivity(cntid);
                if (PermissionDt != null && PermissionDt.Rows.Count > 0)
                {
                    EditPermissionval = Convert.ToString(PermissionDt.Rows[0]["EditPermission"]);

                }


            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Notify", "alert('Server Internal Error.Please try again');", true);
                //return;
            }


            return EditPermissionval;
        }


        protected void SalesDetailsGrid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            int i2 = 0;

            string[] CallVal = e.Parameters.ToString().Split('~');


            if (CallVal[0].ToString() == "InsertBudgetClass")
            {
                decimal Qty_Permonth = (Convert.ToDecimal(txt_qtyfinyr.Text.Trim()));
                string Remarks = (Convert.ToString(txtRemarks.Text.Trim()));
                i2 = InsertSalesmanBudget(hdncustid.Value, hdnslsid.Value, hdnproductclassid.Value, txt_qtyfinyr.Text.Trim(), "0", Qty_Permonth.ToString(), HttpContext.Current.Session["userid"].ToString(), HttpContext.Current.Session["userid"].ToString(), FinYear.ToString(), Remarks);

                if (i2 > 0)
                {

                    DataTable dt_Grid = (DataTable)Session["GrdActivity"];

                    foreach (DataRow dr in dt_Grid.Rows)
                    {

                        if (Convert.ToString(dr[8]) == hdnslsid.Value) // if id==2
                        {
                            dr["budget"] = Math.Round(Qty_Permonth, 2); //change the name

                            dr["Remarks"] = Remarks;

                        }
                    }


                    dt_Grid.AcceptChanges();
                    Session["GrdActivity"] = dt_Grid;

                    SalesDetailsGrid.DataSource = (DataTable)Session["GrdActivity"];
                    SalesDetailsGrid.DataBind();
                    SalesDetailsGrid.JSProperties["cpSave"] = "1";

                }
            }
            else if (CallVal[0].ToString() == "ShowGrid")
            {
                DateTime dtFrom;
                DateTime dtTo;
                dtFrom = Convert.ToDateTime(ASPxFromDate.Date);
                dtTo = Convert.ToDateTime(ASPxToDate.Date);
                Session["Fromdate_Cd"] = dtFrom;
                Session["ToDate_Cd"] = dtTo;

                BindGrid(dtFrom, dtTo);
            }
        }
      
        protected void SalesDetailsGrid_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType != GridViewRowType.Data) return;
            LinkButton lblProduct = (LinkButton)SalesDetailsGrid.FindRowCellTemplateControl(e.VisibleIndex, null, "lblProduct");
            LinkButton lnkProduct = (LinkButton)SalesDetailsGrid.FindRowCellTemplateControl(e.VisibleIndex, null, "lnkProduct");

            Label lblProductClass = (Label)SalesDetailsGrid.FindRowCellTemplateControl(e.VisibleIndex, null, "lblProductClass");
            LinkButton lnkProductClass = (LinkButton)SalesDetailsGrid.FindRowCellTemplateControl(e.VisibleIndex, null, "lnkProductClass");


            if (Session["export"] == null)
            {
                string priorityType = Convert.ToString(e.GetValue("act_priority"));
                string type = Convert.ToString(e.GetValue("act_Type"));
                string ProductName = Convert.ToString(e.GetValue("ProductName"));
                string ProductMultipleName = Convert.ToString(e.GetValue("MultipleProduct"));
                string ProductMultipleClassName = Convert.ToString(e.GetValue("MultipleProductClassName"));
                string ProductClassName = Convert.ToString(e.GetValue("ProductClasName"));

                string fl = Convert.ToString(e.GetValue("FLAG"));
                if (fl == "N")
                { e.Row.Cells[3].Attributes.Add("style", "color:Red;font-weight:bold"); }



                if (priorityType == "0")
                { e.Row.Cells[6].Attributes.Add("style", "color:rgb(245, 223, 195);font-weight:bold"); }
                else if (priorityType == "1")
                { e.Row.Cells[6].Attributes.Add("style", "color:rgb(102, 193, 155);font-weight:bold"); }
                else if (priorityType == "2")
                { e.Row.Cells[6].Attributes.Add("style", "color:rgb(53, 214, 103);font-weight:bold"); }
                else if (priorityType == "3")
                { e.Row.Cells[6].Attributes.Add("style", "color:rgb(255, 124, 124);font-weight:bold"); }
                else if (priorityType == "4")
                { e.Row.Cells[6].Attributes.Add("style", "color:rgb(249, 71, 7);font-weight:bold"); }

                HyperLink hpnhis = (HyperLink)SalesDetailsGrid.FindRowCellTemplateControl(e.VisibleIndex, null, "hpnhis");
                string salesid = Convert.ToString(e.GetValue("sls_id"));

                if (type == "2" || type == "3")
                {
                    if (ProductMultipleName.IndexOf(",") > 0)
                    {

                        if (lnkProduct != null)
                        { lnkProduct.Visible = true; }
                        if (lblProduct != null)
                        {
                            lblProduct.Visible = false;
                        }
                    }
                    else
                    {
                        if (lnkProduct != null)
                        { lnkProduct.Visible = false; }
                        if (lblProduct != null)
                        {
                            lblProduct.Visible = true;
                            lblProduct.Text = ProductMultipleName;
                        }
                    }

                    if (ProductMultipleClassName.IndexOf(",") > 0)
                    {
                        if (lnkProductClass != null)
                        {
                            lnkProductClass.Visible = true;
                        }
                        if (lblProductClass != null)
                        {
                            lblProductClass.Visible = false;
                        }

                    }
                    else
                    {
                        if (lnkProductClass != null)
                        {
                            lnkProductClass.Visible = false;
                        }
                        if (lblProductClass != null)
                        {
                            lblProductClass.Visible = true;
                            lblProductClass.Text = ProductMultipleClassName;
                        }
                    }

                }
                else
                {
                    if (lnkProduct != null)
                    {
                        lnkProduct.Visible = false;
                    }
                    if (lblProduct != null)
                    {
                        lblProduct.Visible = true;
                        lblProduct.Text = ProductName;
                    }
                    if (lblProductClass != null)
                    {

                        lnkProductClass.Visible = false;
                        lblProductClass.Visible = true;
                        lblProductClass.Text = ProductClassName;
                    }
                }

             //   hpnhis.NavigateUrl = "../Master/ShowHistory_Phonecall.aspx?id1=" + salesid;
            }
        }

        protected void drdSalesActivityDetails_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["export"] = "1";
            Int32 Filter = int.Parse(Convert.ToString(drdSalesActivityDetails.SelectedItem.Value));
            if (Filter != 0)
            {
                if (Session["exportval"] == null)
                {
                    Session["exportval"] = Filter;
                    bindSalesActivityDetailsexport(Filter);
                }
                else if (Convert.ToInt32(Session["exportval"]) != Filter)
                {
                    Session["exportval"] = Filter;
                    bindSalesActivityDetailsexport(Filter);
                }
            }
        }
        public void bindSalesActivityDetailsexport(int Filter)
        {

            exporter.GridViewID = "SalesDetailsGrid";
            exporter.FileName = "SalesActivity_ClosedDetails";

            exporter.PageHeader.Left = "Sales Activity Closed Details";
            exporter.PageFooter.Center = "[Page # of Pages #]";
            exporter.PageFooter.Right = "[Date Printed]";
            //SalesDetailsGrid.Columns[18].Visible = false;
            //SalesDetailsGrid.Columns[17].Visible = false;
            //SalesDetailsGrid.Columns[16].Visible = false;
           // SalesDetailsGrid.Columns[16].Visible = false;
            switch (Filter)
            {
                case 1:
                    exporter.WritePdfToResponse();
                    break;
                case 2:
                    exporter.WriteXlsToResponse();
                    break;
                case 3:
                    exporter.WriteRtfToResponse();
                    break;
                case 4:
                    exporter.WriteCsvToResponse();
                    break;
            }
        }

        protected void AspxProductGrid_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            DataTable DtProduct = new DataTable();
            DtProduct = objBL.GetProductByActivity(Convert.ToString(e.Parameters));


            AspxProductGrid.DataSource = DtProduct;
            AspxProductGrid.DataBind();
        }
        protected void AspxProductclassGrid_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            DataTable DtProduct = new DataTable();
            DtProduct = objBL.GetProductCellsByActivity(Convert.ToString(e.Parameters));


            ASPxGridProductClass.DataSource = DtProduct;
            ASPxGridProductClass.DataBind();
        }
        protected void btMyactivities_Click(object sender, EventArgs e)
        {
            if (Session["cntId"] != null)
            {
                int salsmanId = Convert.ToInt32(Session["cntId"]);
                Response.Redirect("../Master/DailySalesReport.aspx?Salsmanid=" + salsmanId + "&returnId=5");
            }
            else
            {

                Response.Redirect("../CRMPhoneCallWithFrame.aspx");
            }
        }

        public void BindproductClass(string cusid)
        {
            DataTable dtclass = new DataTable();
            dtclass = model.GetProductClassdetailsBudget(cusid);
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


        protected void acpCrossBtn_Callback(object sender, CallbackEventArgsBase e)
        {
           

            string[] CallVal = e.Parameter.ToString().Split('~');

            if (CallVal[0].ToString() == "BudgetClass")
            {
                string cusid = Convert.ToString(CallVal[1].ToString());

                string productclassid = Convert.ToString(CallVal[2].ToString());

                string slsid = Convert.ToString(CallVal[3].ToString());

                //hdncustid.Value = cusid;
                //hdnproductclassid.Value = productclassid;
                //hdnslsid.Value = slsid;
                acpCrossBtn.JSProperties["cpcustid"] = cusid;
                acpCrossBtn.JSProperties["cpproductclassid"] = productclassid;
                acpCrossBtn.JSProperties["cpslsid"] = slsid;
                //  acpCrossBtn
                BindproductClass(cusid);
                if (cusid != null && productclassid != null)
                {
                    GetProductwiseBudget(cusid, FinYear, productclassid, slsid);
                    gridproductclass.Value = productclassid;

                }


            }
           
        }


        public int InsertSalesmanBudget(string CustomerID, string slsid, string ProductClass_Id, string Qty_CurrentFY, string Qty_PreviousFY, string Qty_Permonth, string CreatedBy, string Updatedby, string FinYear, string Remarks)
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
            proc.AddPara("@Remarks", Remarks);
            proc.AddPara("@Action", "AddSalesmanBudget");
            return proc.RunActionQuery();
        }


        public void GetProductwiseBudget(string CustomerId, string FinYear, string productclassid,string slsid)
        {
            DataTable dtdetails = GetCustomerbudgetData(CustomerId, FinYear, productclassid, slsid).Tables[0];
            if (dtdetails.Rows.Count > 0)
            {
                txt_qtyfinyr.Text = dtdetails.Rows[0]["Qty_CurrentFY"].ToString();
                txtRemarks.Text = dtdetails.Rows[0]["Remarks"].ToString();


            }
            else
            {
                txt_qtyfinyr.Text = "0.00";
                txtRemarks.Text = "";

            }


        }


        public DataSet GetCustomerbudgetData(string CustomerId, string FinYear, string productclassid, string slsid)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Sp_GetbudgetdataCustomerwise");
            proc.AddPara("@CustomerID", CustomerId);
            proc.AddPara("@Finyear", FinYear);
            proc.AddPara("@ProductClass_ID", productclassid);
            proc.AddPara("@sls_id", slsid);
            proc.AddPara("@Action", "GetActivityBudgetDetails");
            ds = proc.GetDataSet();
            return ds;
        }
    }
}