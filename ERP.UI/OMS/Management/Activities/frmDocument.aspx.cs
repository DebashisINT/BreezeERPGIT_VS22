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
    public partial class management_Activities_frmDocument : ERP.OMS.ViewState_class.VSPage// System.Web.UI.Page
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
                Session["exportval"] = null;


                if (rights.DocumentCollection)
                    { ASPxPageControl1.TabPages.FindByName("Document Collection").ClientEnabled = true; } else { ASPxPageControl1.TabPages.FindByName("Document Collection").ClientEnabled = false; }

                if (rights.FutureSales)
                    { ASPxPageControl1.TabPages.FindByName("Future Sales").ClientEnabled = true; } else { ASPxPageControl1.TabPages.FindByName("Future Sales").ClientEnabled = false; }

                if (rights.ClosedSales)
                    { ASPxPageControl1.TabPages.FindByName("Closed Sales").ClientEnabled = true; } else { ASPxPageControl1.TabPages.FindByName("Closed Sales").ClientEnabled = false; }

                if (rights.ClarificationRequired)
                    { ASPxPageControl1.TabPages.FindByName("Clarification Required").ClientEnabled = true; } else { ASPxPageControl1.TabPages.FindByName("Clarification Required").ClientEnabled = false; }




                SalesDetailsGrid.SettingsCookies.CookiesID = "BreeezeErpGridCookiesSalesDocumentDetailsGrid";
                this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>addCookiesKeyOnStorage('BreeezeErpGridCookiesSalesDocumentDetailsGrid');</script>");
                if (Request.QueryString["frmdate"] != null && Request.QueryString["todate"] != null)
                {
                    ASPxFromDate.Text = Request.QueryString["frmdate"];
                    ASPxToDate.Text = Request.QueryString["todate"];

                    dtFrom = Convert.ToDateTime(ASPxFromDate.Date);
                    dtTo = Convert.ToDateTime(ASPxToDate.Date);
                }
                else
                {
                    if (Session["Fromdate_D"] != null && Session["ToDate_D"] != null)
                    {

                        ASPxFromDate.Text = Convert.ToDateTime(Session["Fromdate_D"]).ToString("dd-MM-yyyy");
                        ASPxToDate.Text = Convert.ToDateTime(Session["ToDate_D"]).ToString("dd-MM-yyyy");
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
           
           
            //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");
        }
        void BindGrid(DateTime FromDate, DateTime ToDate)
        {

            string cnt_internalId = Convert.ToString(HttpContext.Current.Session["owninternalid"]);//Session usercontactID


            DataTable dt = objAc.GetsalesActivity(cnt_internalId, "1", Convert.ToString(FromDate), Convert.ToString(ToDate));
            Session["GrdActivity"] = dt;

            SalesDetailsGrid.DataSource = dt;
            SalesDetailsGrid.DataBind();
        
        }
        //protected void DocumentGrid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        //{
        //    BindGrid();
        //}


        protected void SalesDetailsGrid_DataBinding(object sender, EventArgs e)
        {

            SalesDetailsGrid.DataSource = (DataTable)Session["GrdActivity"];

        }
        protected void SalesDetailsGrid_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType != GridViewRowType.Data) return;
            HyperLink hpnPh = (HyperLink)SalesDetailsGrid.FindRowCellTemplateControl(e.VisibleIndex, null, "hpnPh");
            HyperLink hpnSv = (HyperLink)SalesDetailsGrid.FindRowCellTemplateControl(e.VisibleIndex, null, "hpnSv");
            //    HyperLink hpnhis = (HyperLink)SalesDetailsGrid.FindRowCellTemplateControl(e.VisibleIndex, null, "hpnhis");
            // HyperLink hpnOtheractv = (HyperLink)SalesDetailsGrid.FindRowCellTemplateControl(e.VisibleIndex, null, "hpnOtheractv");
            HyperLink hpnOtheractvSms = (HyperLink)SalesDetailsGrid.FindRowCellTemplateControl(e.VisibleIndex, null, "hpnOtheractvSms");
            HyperLink hpnOtheractvMeet = (HyperLink)SalesDetailsGrid.FindRowCellTemplateControl(e.VisibleIndex, null, "hpnOtheractvMeet");
            HyperLink hpnOtheractvEmail = (HyperLink)SalesDetailsGrid.FindRowCellTemplateControl(e.VisibleIndex, null, "hpnOtheractvEmail");
            Label lbkactivty = (Label)SalesDetailsGrid.FindRowCellTemplateControl(e.VisibleIndex, null, "lblactivty");

            LinkButton lblProduct = (LinkButton)SalesDetailsGrid.FindRowCellTemplateControl(e.VisibleIndex, null, "lblProduct");
            LinkButton lnkProduct = (LinkButton)SalesDetailsGrid.FindRowCellTemplateControl(e.VisibleIndex, null, "lnkProduct");


            Label lblProductClass = (Label)SalesDetailsGrid.FindRowCellTemplateControl(e.VisibleIndex, null, "lblProductClass");
            LinkButton lnkProductClass = (LinkButton)SalesDetailsGrid.FindRowCellTemplateControl(e.VisibleIndex, null, "lnkProductClass");


            if (Session["export"] == null)
            {
                string type = Convert.ToString(e.GetValue("act_Type"));
                string ProductName = Convert.ToString(e.GetValue("ProductName"));
                string ProductClassName = Convert.ToString(e.GetValue("ProductClasName"));
                string acttype = Convert.ToString(e.GetValue("act_activityTypes"));
                string priorityType = Convert.ToString(e.GetValue("act_priority"));
                string fl = Convert.ToString(e.GetValue("FLAG"));
                if (fl == "N")
                { e.Row.Cells[5].Attributes.Add("style", "color:Red;font-weight:bold"); }

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
                //HyperLink hpnhis = (HyperLink)SalesDetailsGrid.FindRowCellTemplateControl(e.VisibleIndex, null, "hpnhis");
                string salesid = Convert.ToString(e.GetValue("sls_id"));
                string ProductMultipleName = Convert.ToString(e.GetValue("MultipleProduct"));
                string ProductMultipleClassName = Convert.ToString(e.GetValue("MultipleProductClassName"));
                string AssignedById = Convert.ToString(e.GetValue("sls_assignedBy"));

                bool containsPhone = acttype.Contains("1");
                bool containsEmail = acttype.Contains("2");
                bool containsSms = acttype.Contains("3");
                bool containsSalesVisit = acttype.Contains("4");
                bool containsMeeting = acttype.Contains("5");
                bool containsSales = acttype.Contains("6");
                string CusId = Convert.ToString(e.GetValue("cnt_id"));
                if (containsPhone)
                {
                    hpnPh.Visible = true;
                    hpnPh.NavigateUrl = "../CRMPhoneCallWithFrame.aspx?TransSale=" + salesid + "&Assigned=" + AssignedById + "&type=" + type + "&Cid=" + CusId + "&Pid=2";
                }
                else { hpnPh.Visible = false; }
                if (containsSalesVisit)
                {
                    hpnSv.NavigateUrl = "CRMSalesVisitWithIFrame.aspx?TransSale=" + salesid + "&Assigned=" + AssignedById + "&type=" + type + "&Cid=" + CusId + "&Pid=2";
                    hpnSv.Visible = true;
                }
                else { hpnSv.Visible = false; }

                if (containsSms)
                {
                    hpnOtheractvSms.NavigateUrl = "CRMOtherActivities.aspx?TransSale=" + salesid + "&TypId=3" + "&Assigned=" + AssignedById + "&type=" + type + "&Cid=" + CusId + "&Pid=2";
                    hpnOtheractvSms.Visible = true;
                }
                else { hpnOtheractvSms.Visible = false; }
                if (containsEmail)
                {
                    hpnOtheractvEmail.NavigateUrl = "CRMOtherActivities.aspx?TransSale=" + salesid + "&TypId=2" + "&Assigned=" + AssignedById + "&type=" + type + "&Cid=" + CusId + "&Pid=2";
                    hpnOtheractvEmail.Visible = true;
                }
                else { hpnOtheractvEmail.Visible = false; }
                if (containsMeeting)
                {
                    hpnOtheractvMeet.NavigateUrl = "CRMOtherActivities.aspx?TransSale=" + salesid + "&TypId=5" + "&Assigned=" + AssignedById + "&type=" + type + "&Cid=" + CusId + "&Pid=2";
                    hpnOtheractvMeet.Visible = true;
                }
                else { hpnOtheractvMeet.Visible = false; }


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
            exporter.FileName = "SalesActivity_DocumentDetails";
            exporter.PageHeader.Left = "Sales Activity Document Details";
            exporter.PageFooter.Center = "[Page # of Pages #]";
            exporter.PageFooter.Right = "[Date Printed]";

          
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
                Response.Redirect("../Master/DailySalesReport.aspx?Salsmanid=" + salsmanId + "&returnId=2");
            }
            else
            {

                Response.Redirect("../CRMPhoneCallWithFrame.aspx");
            }
        }



        //protected void btMyPendingTask_Click(object sender, EventArgs e)
        //{
        //    if (Session["cntId"] != null)
        //    {
        //        int salsmanId = Convert.ToInt32(Session["cntId"]);
        //        Response.Redirect("../Master/PendingTaskReport.aspx?Salsmanid=" + salsmanId + "&returnId=2");
        //    }
        //    else
        //    {

        //        Response.Redirect("../CRMPhoneCallWithFrame.aspx");
        //    }
        //}
        protected void SalesDetailsGrid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            DateTime dtFrom;
            DateTime dtTo;
            dtFrom = Convert.ToDateTime(ASPxFromDate.Date);
            dtTo = Convert.ToDateTime(ASPxToDate.Date);
            int i2 = 0;
            Session["Fromdate_D"] = dtFrom;
            Session["ToDate_D"] = dtTo;


           // BindGrid(dtFrom, dtTo);

            string[] CallVal = e.Parameters.ToString().Split('~');
            if (CallVal[0].ToString() == "ClosedStatus")
            {
                string Id = Convert.ToString(CallVal[1].ToString());

                string userId = Convert.ToString(HttpContext.Current.Session["userid"]);
                int retValue = objBL.ClosedStatusActivity(userId, Id);
                if (retValue > 0)
                {
                    //  Session["KeyVal"] = "Closed";
                    SalesDetailsGrid.JSProperties["cpDelmsg"] = "Closed Sales";


                    BindGrid(dtFrom, dtTo);

                    // SalesDetailsGrid.DataBinding;
                }

            }

            else  if (CallVal[0].ToString() == "InsertBudgetClass")
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
               
                BindGrid(dtFrom, dtTo);
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


        public void GetProductwiseBudget(string CustomerId, string FinYear, string productclassid, string slsid)
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