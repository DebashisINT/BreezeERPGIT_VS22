using DevExpress.Web;
using DevExpress.Web.Mvc;
using EntityLayer.CommonELS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogicLayer;
using System.Collections.Specialized;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;

using DataAccessLayer;
using System.Threading.Tasks;
using DevExpress.XtraPrinting;
using DevExpress.Export;
using System.Drawing;
namespace Reports.Reports.GridReports
{
    public partial class Insurance_Register : System.Web.UI.Page
    {
        ReportData rpt = new ReportData();
        DataTable DTIndustry = new DataTable();
        BusinessLogicLayer.Reports objReport = new BusinessLogicLayer.Reports();
        string data = "";
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();

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
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/Reports/master/GstrReport.aspx");
            DateTime dtFrom;
            DateTime dtTo;
            if (!IsPostBack)
            {
                string GridHeader = "";
                BusinessLogicLayer.Reports GridHeaderDet = new BusinessLogicLayer.Reports();
                RptHeading.Text = "Insurance Register";
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

                Session["gridGstR1"] = null;
                Session["gridGstRetrn1"] = null;
                Session["dt_GSTRRpt"] = null;
                Session["SI_ComponentData_Branch"] = null;
                Session["dt_GSTRRpt_Purchase"] = null;


                dtFrom = DateTime.Now;
                dtTo = DateTime.Now;
                //   BindDropDownList();

                //ASPxFromDate.Text = dtFrom.ToString("dd-MM-yyyy");
                //ASPxToDate.Text = dtTo.ToString("dd-MM-yyyy");
                //Date_finyearwise(Convert.ToString(Session["LastFinYear"]));

                ASPxFromDate.Value = DateTime.Now;
                ASPxToDate.Value = DateTime.Now;


                BranchpopulateGSTN();
             

            }
            else
            {

            }


        }

        public void Date_finyearwise(string Finyear)
        {
            CommonBL bll1 = new CommonBL();
            DataTable stbill = new DataTable();
            stbill = bll1.GetDateFinancila(Finyear);
            if (stbill.Rows.Count > 0)
            {
                ASPxFromDate.Text = Convert.ToDateTime(stbill.Rows[0]["FinYear_StartDate"]).ToString("dd-MM-yyyy");
                ASPxToDate.Text = Convert.ToDateTime(stbill.Rows[0]["FinYear_EndDate"]).ToString("dd-MM-yyyy");
            }

        }

        public void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(Convert.ToString(drdExport.SelectedItem.Value));
            if (Filter != 0)
            {


                if (Session["exportval"] == null)
                {
                    //  Session["exportval"] = Filter;
                    // BindDropDownList();
                    bindexport(Filter);
                }
                else if (Convert.ToInt32(Session["exportval"]) != Filter)
                {
                    // Session["exportval"] = Filter;
                    // BindDropDownList();
                    bindexport(Filter);
                }
            }

        }

        public void bindexport(int Filter)
        {

            if (ASPxPageControl1.ActiveTabIndex == 0)
            {
                string filename = "Insurance sales Register";
                exporter.FileName = filename;
                string FileHeader = "";

                exporter.FileName = filename;

                BusinessLogicLayer.Reports RptHeader = new BusinessLogicLayer.Reports();

                FileHeader = RptHeader.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), true, true, true,true, true, true) + Environment.NewLine + "Insurance Sales Register" + Environment.NewLine + "For the period " + Convert.ToDateTime(ASPxFromDate.Date).ToString("dd-MM-yyyy") + " To " + Convert.ToDateTime(ASPxToDate.Date).ToString("dd-MM-yyyy");

                exporter.PageHeader.Left = FileHeader;
                exporter.PageHeader.Font.Size = 10;
                exporter.PageHeader.Font.Name = "Tahoma";

                //exporter.PageFooter.Center = "[Page # of Pages #]";
                //exporter.PageFooter.Right = "[Date Printed]";
                exporter.Landscape = true;
                exporter.MaxColumnWidth = 100;
                exporter.GridViewID = "ShowGrid";
                exporter.RenderBrick += exporter_RenderBrick;
            }

            else
            {

                string filename = "Insurance Purchase  Register";
                exporter.FileName = filename;
                string FileHeader = "";

                exporter.FileName = filename;

                BusinessLogicLayer.Reports RptHeader = new BusinessLogicLayer.Reports();

                FileHeader = RptHeader.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), true, true, true,true, true, true) + Environment.NewLine + "Insurance Purchase Register" + Environment.NewLine + "For the period " + Convert.ToDateTime(ASPxFromDate.Date).ToString("dd-MM-yyyy") + " To " + Convert.ToDateTime(ASPxToDate.Date).ToString("dd-MM-yyyy");

                exporter.PageHeader.Left = FileHeader;
                exporter.PageHeader.Font.Size = 10;
                exporter.PageHeader.Font.Name = "Tahoma";

                exporter.PageFooter.Center = "[Page # of Pages #]";
                exporter.PageFooter.Right = "[Date Printed]";
                exporter.Landscape = true;
                exporter.MaxColumnWidth = 100;
                exporter.GridViewID = "grid_purchase";
                exporter.RenderBrick += exporter_RenderBrick;
            }
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

        protected void exporter_RenderBrick(object sender, ASPxGridViewExportRenderingEventArgs e)
        {
            e.BrickStyle.BackColor = Color.White;
            e.BrickStyle.ForeColor = Color.Black;
        }
   
        #region ########  Branch GRN Populate  #######
        protected void BranchpopulateGSTN()
        {
            // DataTable dst = new DataTable();
            string userbranchID = Convert.ToString(Session["userbranchID"]);

            DataTable ComponentTable = new DataTable();

            DataSet ds = new DataSet();
            //SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);
            SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            SqlCommand cmd = new SqlCommand("GetGSTNfetch", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Company", Convert.ToString(Session["LastCompany"]));
            cmd.Parameters.AddWithValue("@Branchlist", Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));
            cmd.CommandTimeout = 0;
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;

            da.Fill(ComponentTable);

            cmd.Dispose();
            con.Dispose();


            //if (ds.Tables[0].Rows.Count > 0)
            //{
            //    ddlgstn.DataSource = ds.Tables[0];
            //    ddlgstn.DataTextField = "branch_GSTIN";
            //    ddlgstn.DataValueField = "branch_GSTIN";
            //    ddlgstn.DataBind();
            //    ddlgstn.Items.Insert(0, "");
            //}

            if (ComponentTable.Rows.Count > 0)
            {
                // grid_Products.JSProperties["cptxt_InvoiceDate"] = Convert.ToString(ComponentTable.Rows[0]["Invoice_Date"]);

                Session["SI_ComponentData_Branch"] = ComponentTable;

                lookup_branch.DataSource = ComponentTable;
                lookup_branch.DataBind();

            }
            else
            {
                Session["SI_ComponentData_Branch"] = null;
                lookup_branch.DataSource = null;
                lookup_branch.DataBind();

            }

        }

        #endregion

        #region  ############# Insurance Sales Register  Only  #################


        protected void Grid_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {

            Session.Remove("dt_GSTRRpt");

            ShowGrid.JSProperties["cpSave"] = null;
            string returnPara = Convert.ToString(e.Parameters);

            string WhichCall = returnPara.Split('~')[0];
            if (WhichCall == "ListData")
            {
                string WhichCall2 = returnPara.Split('~')[1];
                string COMPANYID = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
                string Finyear = Convert.ToString(HttpContext.Current.Session["LastFinYear"]);


                string BRANCH_ID = "";
                string QuoComponent2 = "";
                List<object> QuoList2 = lookup_branch.GridView.GetSelectedFieldValues("branch_GSTIN");
                foreach (object Quo2 in QuoList2)
                {
                    QuoComponent2 += "," + Quo2;
                }

                if (hdnSelectedBranches.Value != "")
                {
                    BRANCH_ID = hdnSelectedBranches.Value;
                }
             
                BRANCH_ID = QuoComponent2.TrimStart(',');



                ///GetSalesRegisterdata(WhichCall2);

                Task PopulateStockTrialDataTask = new Task(() => SaleInsuranceRegisterdata(WhichCall2, BRANCH_ID));
                PopulateStockTrialDataTask.RunSynchronously(); 

            }
        }



        protected void grid_DataBinding(object sender, EventArgs e)
        {
            if (Session["gridGstR1"] != null)
            {
                ShowGrid.DataSource = (DataTable)Session["gridGstR1"];

            }

        }

        public void SaleInsuranceRegisterdata(string Gstn,string Branch)
        {
            try
            {
                string DriverName = string.Empty;
                string PhoneNo = string.Empty;
                string VehicleNo = string.Empty;
                DataTable dttab = new DataTable();

                DateTime dtFrom;
                DateTime dtTo;

                dtFrom = Convert.ToDateTime(ASPxFromDate.Date);
                dtTo = Convert.ToDateTime(ASPxToDate.Date);

                string FROMDATE = dtFrom.ToString("yyyy-MM-dd");
                string TODATE = dtTo.ToString("yyyy-MM-dd");

                string BRANCH_ID = "";
                string QuoComponent = "";
                List<object> QuoList = lookup_branch.GridView.GetSelectedFieldValues("branch_GSTIN");
                foreach (object Quo in QuoList)
                {
                    QuoComponent += "," + Quo;
                }
                BRANCH_ID = QuoComponent.TrimStart(',');


                //dttab = GetInsuranceSaleRegisterdataDetails(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]).Trim(), "saleinsurance", Gstn, FROMDATE, TODATE, Branch);

                dttab = GetInsuranceSaleRegisterdataDetails(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]).Trim(), "saleinsurance", BRANCH_ID, FROMDATE, TODATE, Branch);


                if (dttab.Rows.Count > 0)
                {
                    Session["gridGstR1"] = dttab;
                    ShowGrid.DataSource = dttab;
                    ShowGrid.DataBind();
                }
                else
                {
                    Session["gridGstR1"] = null;
                    ShowGrid.DataSource = null;
                    ShowGrid.DataBind();

                }
            }
            catch (Exception ex)
            {
            }
        }
        public DataTable GetInsuranceSaleRegisterdataDetails(string Company, string Finyear, string Action, string Gstn, string FROMDATE, string TODATE, string Branch)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("Proc_InuranceRegister_Report");
            proc.AddPara("@COMPANYID", Company);
            proc.AddPara("@FINYEAR", Finyear);
            proc.AddPara("@Acton", Action);
            proc.AddPara("@FROMDATE", FROMDATE);
            proc.AddPara("@TODATE", TODATE);
            proc.AddPara("@GSTIN", Gstn);
            proc.AddPara("@Partydate", chkparty.Checked);
           
        
            ds = proc.GetTable();
            return ds;
        }



        protected void Showgrid_Datarepared(object sender, EventArgs e)
        {
            ASPxGridView grid = (ASPxGridView)sender;
          

        }



        private int totalCount;
        private int totalCountrecp;

        private List<string> Number = new List<string>();


        protected void ASPxGridView1_CustomSummaryCalculate(object sender, DevExpress.Data.CustomSummaryEventArgs e)
        {
            ASPxSummaryItem item = e.Item as ASPxSummaryItem;

            if (e.SummaryProcess == DevExpress.Data.CustomSummaryProcess.Start)
            {
                Number.Clear();

                totalCount = 0;
                totalCountrecp = 0;


            }
            if (e.SummaryProcess == DevExpress.Data.CustomSummaryProcess.Calculate)
            {

                string val = Convert.ToString(e.FieldValue);
                if (!Number.Contains(val))
                {
                    totalCount++;
                    Number.Add(val);
                }


            }
            if (e.SummaryProcess == DevExpress.Data.CustomSummaryProcess.Finalize)
            {

                if (e.Item == ShowGrid.TotalSummary["Number"])
                {
                    e.TotalValue = string.Format("Doc Count={0}", totalCount);
                }

            }



        }

        protected void ShowGrid_SummaryDisplayText(object sender, ASPxGridViewSummaryDisplayTextEventArgs e)
        {

            //    e.Text = string.Format("{0}", Convert.ToDecimal(e.Value));
            if (e.Item == ShowGrid.TotalSummary["TAxAmt"])
            {
                e.Text = string.Format("Total={0}", e.Value);

            }
            if (e.Item == ShowGrid.TotalSummary["Net"])
            {
                e.Text = string.Format("Total={0}", e.Value);

            }


            if (e.Item == ShowGrid.TotalSummary["CGSTRate"])
            {
                e.Text = string.Format("Total={0}", e.Value);

            }

            if (e.Item == ShowGrid.TotalSummary["SGSTRate"])
            {
                e.Text = string.Format("Total={0}", e.Value);

            }

            if (e.Item == ShowGrid.TotalSummary["IGSTRate"])
            {
                e.Text = string.Format("Total={0}", e.Value);

            }

            if (e.Item == ShowGrid.TotalSummary["Qty"])
            {
                e.Text = string.Format("Total={0}", e.Value);

            }


            if (e.Item == ShowGrid.TotalSummary["othertax"])
            {
                e.Text = string.Format("Total={0}", e.Value);

            }

            if (e.Item == ShowGrid.TotalSummary["Totalnet"])
            {
                e.Text = string.Format("Total={0}", e.Value);

            }

            if (e.Item == ShowGrid.TotalSummary["oldunitfet"])
            {
                e.Text = string.Format("Total={0}", e.Value);

            }

        }


        #endregion

        #region  ############# Insurance Purchase Register  Only  #################


        protected void Grid_Purchase_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {

            Session.Remove("dt_GSTRRpt_Purchase");

            ShowGrid.JSProperties["cpSave"] = null;
            string returnPara = Convert.ToString(e.Parameters);

            string WhichCall = returnPara.Split('~')[0];
            if (WhichCall == "ListData")
            {
                string WhichCall2 = returnPara.Split('~')[1];
                string COMPANYID = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
                string Finyear = Convert.ToString(HttpContext.Current.Session["LastFinYear"]);


                string BRANCH_ID = "";
                string QuoComponent2 = "";
                List<object> QuoList2 = lookup_branch.GridView.GetSelectedFieldValues("branch_GSTIN");
                foreach (object Quo2 in QuoList2)
                {
                    QuoComponent2 += "," + Quo2;
                }

                if (hdnSelectedBranches.Value != "")
                {
                    BRANCH_ID = hdnSelectedBranches.Value;
                }

                BRANCH_ID = QuoComponent2.TrimStart(',');



                ///GetSalesRegisterdata(WhichCall2);

                Task PopulateStockTrialDataTask = new Task(() => PurchaseInsuranceRegisterdata(WhichCall2, BRANCH_ID));
                PopulateStockTrialDataTask.RunSynchronously();

            }
        }



        protected void gridpurchase_DataBinding(object sender, EventArgs e)
        {
            if (Session["dt_GSTRRpt_Purchase"] != null)
            {
                grid_purchase.DataSource = (DataTable)Session["dt_GSTRRpt_Purchase"];

            }

        }

        public void PurchaseInsuranceRegisterdata(string Gstn, string Branch)
        {
            try
            {
                string DriverName = string.Empty;
                string PhoneNo = string.Empty;
                string VehicleNo = string.Empty;
                DataTable dttab = new DataTable();

                DateTime dtFrom;
                DateTime dtTo;

                dtFrom = Convert.ToDateTime(ASPxFromDate.Date);
                dtTo = Convert.ToDateTime(ASPxToDate.Date);

                string FROMDATE = dtFrom.ToString("yyyy-MM-dd");
                string TODATE = dtTo.ToString("yyyy-MM-dd");


                string BRANCH_ID = "";
                string QuoComponent = "";
                List<object> QuoList = lookup_branch.GridView.GetSelectedFieldValues("branch_GSTIN");
                foreach (object Quo in QuoList)
                {
                    QuoComponent += "," + Quo;
                }
                BRANCH_ID = QuoComponent.TrimStart(',');

                //dttab = GetInsurancepurchaseRegisterdataDetails(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]).Trim(), "purchaseinsurance", Gstn, FROMDATE, TODATE, Branch);
                dttab = GetInsurancepurchaseRegisterdataDetails(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]).Trim(), "purchaseinsurance", BRANCH_ID, FROMDATE, TODATE, Branch);

                if (dttab.Rows.Count > 0)
                {
                    Session["dt_GSTRRpt_Purchase"] = dttab;
                    grid_purchase.DataSource = dttab;
                    grid_purchase.DataBind();
                }
                else
                {
                    Session["dt_GSTRRpt_Purchase"] = null;
                    grid_purchase.DataSource = null;
                    grid_purchase.DataBind();

                }
            }
            catch (Exception ex)
            {
            }
        }
        public DataTable GetInsurancepurchaseRegisterdataDetails(string Company, string Finyear, string Action, string Gstn, string FROMDATE, string TODATE, string Branch)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("Proc_InuranceRegister_Report");
            proc.AddPara("@COMPANYID", Company);
            proc.AddPara("@FINYEAR", Finyear);
            proc.AddPara("@FROMDATE", FROMDATE);
            proc.AddPara("@Acton", Action);
            proc.AddPara("@TODATE", TODATE);
            proc.AddPara("@GSTIN", Gstn);
            proc.AddPara("@Partydate", chkparty.Checked);

            ds = proc.GetTable();

            return ds;
        }

        protected void ASPxGridpurchase_CustomSummaryCalculate(object sender, DevExpress.Data.CustomSummaryEventArgs e)
        {
            ASPxSummaryItem item = e.Item as ASPxSummaryItem;

            if (e.SummaryProcess == DevExpress.Data.CustomSummaryProcess.Start)
            {
                Number.Clear();

                totalCount = 0;
                totalCountrecp = 0;


            }
            if (e.SummaryProcess == DevExpress.Data.CustomSummaryProcess.Calculate)
            {

                string val = Convert.ToString(e.FieldValue);
                if (!Number.Contains(val))
                {
                    totalCount++;
                    Number.Add(val);
                }


            }
            if (e.SummaryProcess == DevExpress.Data.CustomSummaryProcess.Finalize)
            {

                if (e.Item == grid_purchase.TotalSummary["Number"])
                {
                    e.TotalValue = string.Format("Doc Count={0}", totalCount);
                }

            }



        }

        protected void ShowGridPurchase_SummaryDisplayText(object sender, ASPxGridViewSummaryDisplayTextEventArgs e)
        {

            if (e.Item == grid_purchase.TotalSummary["TAxAmt"])
            {
                e.Text = string.Format("Total={0}", e.Value);

            }
            if (e.Item == grid_purchase.TotalSummary["Net"])
            {
                e.Text = string.Format("Total={0}", e.Value);

            }


            if (e.Item == grid_purchase.TotalSummary["CGSTRate"])
            {
                e.Text = string.Format("Total={0}", e.Value);

            }

            if (e.Item == grid_purchase.TotalSummary["SGSTRate"])
            {
                e.Text = string.Format("Total={0}", e.Value);

            }

            if (e.Item == grid_purchase.TotalSummary["IGSTRate"])
            {
                e.Text = string.Format("Total={0}", e.Value);

            }

            if (e.Item == grid_purchase.TotalSummary["Qty"])
            {
                e.Text = string.Format("Total={0}", e.Value);

            }


            if (e.Item == grid_purchase.TotalSummary["othertax"])
            {
                e.Text = string.Format("Total={0}", e.Value);

            }

            if (e.Item == grid_purchase.TotalSummary["Totalnet"])
            {
                e.Text = string.Format("Total={0}", e.Value);

            }


        }

        #endregion
      
        #region Branch Populate

        protected void Componentbranch_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            string FinYear = Convert.ToString(Session["LastFinYear"]);

            if (e.Parameter.Split('~')[0] == "BindComponentGrid")
            {
                DataTable ComponentTable = new DataTable();
                string Hoid = e.Parameter.Split('~')[1];

                DataSet ds = new DataSet();
                //SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));


                SqlCommand cmd = new SqlCommand("GetGSTNfetch", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Company", Convert.ToString(Session["LastCompany"]));
                cmd.Parameters.AddWithValue("@Branchlist", Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));



                //SqlCommand cmd = new SqlCommand("Getbranchlist_Gsitnwise", con);
                //cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@GstinId", ddlgstn.SelectedValue);


                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(ds);

                cmd.Dispose();
                con.Dispose();

                if (ds.Tables[0].Rows.Count > 0)
                {
                    Session["SI_ComponentData_Branch"] = ds.Tables[0];
                    lookup_branch.DataSource = ds.Tables[0];
                    lookup_branch.DataBind();
                }
                else
                {
                    Session["SI_ComponentData_Branch"] = null;
                    lookup_branch.DataSource = null;
                    lookup_branch.DataBind();
                }
            }
        }

        protected void lookup_branch_DataBinding(object sender, EventArgs e)
        {
            if (Session["SI_ComponentData_Branch"] != null)
            {
                lookup_branch.DataSource = (DataTable)Session["SI_ComponentData_Branch"];
            }
        }

        #endregion
    }
}