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
using ERP.OMS.Management.Master;
using DataAccessLayer;
using System.Threading.Tasks;
using DevExpress.XtraPrinting;
using DevExpress.Export;
using System.Drawing;

namespace ERP.OMS.Reports.Master
{

    public partial class GSTR_SaleRegister : System.Web.UI.Page
    {
        ReportData rpt = new ReportData();
        DataTable DTIndustry = new DataTable();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        BusinessLogicLayer.Reports objReport = new BusinessLogicLayer.Reports();
        string data = "";
      
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
                Session["gridGstR1"] = null;
                Session["gridGstRetrn1"] = null;
                Session["dt_GSTRRpt"] = null;
                Session["SI_ComponentData_Branch"] = null;
                Session["GSTR_DebitNote"] = null;
                dtFrom = DateTime.Now;
                dtTo = DateTime.Now;
                //   BindDropDownList();

                //ASPxFromDate.Text = dtFrom.ToString("dd-MM-yyyy");
                //ASPxToDate.Text = dtTo.ToString("dd-MM-yyyy");
                //Date_finyearwise(Convert.ToString(Session["LastFinYear"]));

                BranchpopulateGSTN();


                ASPxFromDate.Value = DateTime.Now;
                ASPxToDate.Value = DateTime.Now;

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
                string filename = "Sales GSTR Register";
                exporter.FileName = filename;
                string FileHeader = "";

                exporter.FileName = filename;

                BusinessLogicLayer.Reports RptHeader = new BusinessLogicLayer.Reports();

                FileHeader = RptHeader.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), true, true, true, true, true) + Environment.NewLine + "Sales GSTR Register" + Environment.NewLine + "For the period " + Convert.ToDateTime(ASPxFromDate.Date).ToString("dd-MM-yyyy") + " To " + Convert.ToDateTime(ASPxToDate.Date).ToString("dd-MM-yyyy");

                exporter.PageHeader.Left = FileHeader;
                exporter.PageHeader.Font.Size = 10;
                exporter.PageHeader.Font.Name = "Tahoma";
                exporter.RenderBrick += exporter_RenderBrick;
                exporter.PageFooter.Center = "[Page # of Pages #]";
                exporter.PageFooter.Right = "[Date Printed]";
                exporter.Landscape = true;
                exporter.MaxColumnWidth = 100;
                exporter.GridViewID = "ShowGrid";
            }
            else if (ASPxPageControl1.ActiveTabIndex == 1)
            {
                string filename = "Sales Return GSTR Register";
                exporter.FileName = filename;
                string FileHeader = "";

                exporter.FileName = filename;

                BusinessLogicLayer.Reports RptHeader = new BusinessLogicLayer.Reports();

                FileHeader = RptHeader.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), true, true, true, true, true) + Environment.NewLine + "Sales Return GSTR Register" + Environment.NewLine + "For the period " + Convert.ToDateTime(ASPxFromDate.Date).ToString("dd-MM-yyyy") + " To " + Convert.ToDateTime(ASPxToDate.Date).ToString("dd-MM-yyyy");

                exporter.PageHeader.Left = FileHeader;
                exporter.PageHeader.Font.Size = 10;
                exporter.PageHeader.Font.Name = "Tahoma";
                exporter.RenderBrick += exporter_RenderBrick;
                exporter.PageFooter.Center = "[Page # of Pages #]";
                exporter.PageFooter.Right = "[Date Printed]";
                exporter.Landscape = true;
                exporter.MaxColumnWidth = 100;
                exporter.GridViewID = "ShowGrid2";
            }
            else if (ASPxPageControl1.ActiveTabIndex == 2)
            {
                string filename = "Customer Debit Note";
                exporter.FileName = filename;
                string FileHeader = "";

                exporter.FileName = filename;

                BusinessLogicLayer.Reports RptHeader = new BusinessLogicLayer.Reports();

                FileHeader = RptHeader.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), true, true, true, true, true) + Environment.NewLine + "Debit Note GSTR Register" + Environment.NewLine + "For the period " + Convert.ToDateTime(ASPxFromDate.Date).ToString("dd-MM-yyyy") + " To " + Convert.ToDateTime(ASPxToDate.Date).ToString("dd-MM-yyyy");

                exporter.PageHeader.Left = FileHeader;
                exporter.PageHeader.Font.Size = 10;
                exporter.PageHeader.Font.Name = "Tahoma";
                exporter.RenderBrick += exporter_RenderBrick;
                exporter.PageFooter.Center = "[Page # of Pages #]";
                exporter.PageFooter.Right = "[Date Printed]";
                exporter.Landscape = true;
                exporter.MaxColumnWidth = 100;
                exporter.GridViewID = "grid_debitNote";
            }
            else if (ASPxPageControl1.ActiveTabIndex == 3)
            {
                string filename = "Customer Credit Note";
                exporter.FileName = filename;
                string FileHeader = "";

                exporter.FileName = filename;

                BusinessLogicLayer.Reports RptHeader = new BusinessLogicLayer.Reports();

                FileHeader = RptHeader.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), true, true, true, true, true) + Environment.NewLine + "Credit Note GSTR Register" + Environment.NewLine + "For the period " + Convert.ToDateTime(ASPxFromDate.Date).ToString("dd-MM-yyyy") + " To " + Convert.ToDateTime(ASPxToDate.Date).ToString("dd-MM-yyyy");

                exporter.PageHeader.Left = FileHeader;
                exporter.PageHeader.Font.Size = 10;
                exporter.PageHeader.Font.Name = "Tahoma";
                exporter.RenderBrick += exporter_RenderBrick;
                exporter.PageFooter.Center = "[Page # of Pages #]";
                exporter.PageFooter.Right = "[Date Printed]";
                exporter.Landscape = true;
                exporter.MaxColumnWidth = 100;
                exporter.GridViewID = "grid_creditNote";
            }


            switch (Filter)
            {
                case 1:
                    exporter.WritePdfToResponse();

                    break;
                case 2:
                    exporter.WriteXlsxToResponse();
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

        #region ########  Branch GRN Populate  #######

        protected void BranchpopulateGSTN()
        {
            // DataTable dst = new DataTable();
            string userbranchID = Convert.ToString(Session["userbranchID"]);



            DataSet ds = new DataSet();
            SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);
            SqlCommand cmd = new SqlCommand("GetGSTNfetch", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Company", Convert.ToString(Session["LastCompany"]));
            cmd.Parameters.AddWithValue("@Branchlist", Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));
            cmd.CommandTimeout = 0;
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(ds);

            cmd.Dispose();
            con.Dispose();


            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlgstn.DataSource = ds.Tables[0];
                ddlgstn.DataTextField = "branch_GSTIN";
                ddlgstn.DataValueField = "branch_GSTIN";
                ddlgstn.DataBind();
                ddlgstn.Items.Insert(0, "");
            }


        }
        protected void Componentbranch_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            string FinYear = Convert.ToString(Session["LastFinYear"]);

            if (e.Parameter.Split('~')[0] == "BindComponentGrid")
            {
                DataTable ComponentTable = new DataTable();
                string Hoid = e.Parameter.Split('~')[1];

                if (Hoid != "0")
                {
                    DataSet ds = new DataSet();
                    SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);
                    SqlCommand cmd = new SqlCommand("Getbranchlist_Gsitnwise", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@GstinId", ddlgstn.SelectedValue);
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
                        lookup_branch.DataSource = null;
                        lookup_branch.DataBind();
                    }

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

        #region  ############# Sales  Only  #################

        private int totalCount;
        private int totalCountrecp;
        private List<string> Number = new List<string>();

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
                List<object> QuoList2 = lookup_branch.GridView.GetSelectedFieldValues("branch_id");
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

                Task PopulateStockTrialDataTask = new Task(() => GetSalesRegisterdata(WhichCall2, BRANCH_ID));
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
        public void GetSalesRegisterdata(string Gstn, string Branch)
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


                dttab = GetBranchGestn(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]).Trim(), "sale", Gstn, FROMDATE, TODATE, Branch);

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
        public DataTable GetBranchGestn(string Company, string Finyear, string Action, string Gstn, string FROMDATE, string TODATE, string Branch)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("GST_Salepurchase");
            proc.AddPara("@COMPANYID", Company);
            proc.AddPara("@FINYEAR", Finyear);
            proc.AddPara("@Acton", Action);
            proc.AddPara("@FROMDATE", FROMDATE);
            proc.AddPara("@TODATE", TODATE);
            proc.AddPara("@GSTIN", Gstn);
            proc.AddPara("@P_BRANCH_ID", Branch);
            proc.AddPara("@ChkwithoutTax", chkwithouttax.Checked);
            proc.AddPara("@INVENTORY", ddlinventory.SelectedValue);
            ds = proc.GetTable();
            return ds;
        }
        protected void Showgrid_Datarepared(object sender, EventArgs e)
        {
            ASPxGridView grid = (ASPxGridView)sender;


        }
        protected void ASPxGridView1_CustomSummaryCalculate(object sender, DevExpress.Data.CustomSummaryEventArgs e)
        {
            //ASPxSummaryItem item = e.Item as ASPxSummaryItem;

            //if (e.SummaryProcess == DevExpress.Data.CustomSummaryProcess.Start)
            //{
            //    Number.Clear();

            //    totalCount = 0;
            //    totalCountrecp = 0;


            //}
            //if (e.SummaryProcess == DevExpress.Data.CustomSummaryProcess.Calculate)
            //{

            //    string val = Convert.ToString(e.FieldValue);
            //    if (!Number.Contains(val))
            //    {
            //        totalCount++;
            //        Number.Add(val);
            //    }


            //}
            //if (e.SummaryProcess == DevExpress.Data.CustomSummaryProcess.Finalize)
            //{

            //    if (e.Item == ShowGrid.TotalSummary["Number"])
            //    {
            //        e.TotalValue = string.Format("Doc Count={0}", totalCount);
            //    }

            //}



        }

        #endregion

        #region  ############# Sales  Return  #################

        private int totalCount2;
        private int totalCountrecp2;
        private List<string> Number2 = new List<string>();

        protected void Grid2_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
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
                List<object> QuoList2 = lookup_branch.GridView.GetSelectedFieldValues("branch_id");
                foreach (object Quo2 in QuoList2)
                {
                    QuoComponent2 += "," + Quo2;
                }

                if (hdnSelectedBranches.Value != "")
                {
                    BRANCH_ID = hdnSelectedBranches.Value;
                }

                BRANCH_ID = QuoComponent2.TrimStart(',');



                ///  GetSalesReturndata(WhichCall2);

                Task PopulateStockTrialDataTask = new Task(() => GetSalesReturndata(WhichCall2, BRANCH_ID));
                PopulateStockTrialDataTask.RunSynchronously();

            }
        }
        protected void grid2_DataBinding(object sender, EventArgs e)
        {
            if (Session["gridGstRetrn1"] != null)
            {
                ShowGrid2.DataSource = (DataTable)Session["gridGstRetrn1"];

            }

        }
        public void GetSalesReturndata(string Gstn, string Branch)
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


                dttab = GetGstinReturn(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]).Trim(), "salereturn", Gstn, FROMDATE, TODATE, Branch);

                if (dttab.Rows.Count > 0)
                {
                    Session["gridGstRetrn1"] = dttab;
                    ShowGrid2.DataSource = dttab;
                    ShowGrid2.DataBind();
                }
                else
                {
                    Session["gridGstRetrn1"] = null;
                    ShowGrid2.DataSource = null;
                    ShowGrid2.DataBind();

                }
            }
            catch (Exception ex)
            {
            }
        }
        public DataTable GetGstinReturn(string Company, string Finyear, string Action, string Gstn, string FROMDATE, string TODATE, string Branch)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("GST_Salepurchase");
            proc.AddPara("@COMPANYID", Company);
            proc.AddPara("@FINYEAR", Finyear);
            proc.AddPara("@Acton", Action);
            proc.AddPara("@FROMDATE", FROMDATE);
            proc.AddPara("@TODATE", TODATE);
            proc.AddPara("@GSTIN", Gstn);
            proc.AddPara("@P_BRANCH_ID", Branch);
            proc.AddPara("@INVENTORY", ddlinventory.SelectedValue);
            ds = proc.GetTable();
            return ds;
        }
        protected void ShowGrid2_SummaryDisplayText(object sender, ASPxGridViewSummaryDisplayTextEventArgs e)
        {

            ///     e.Text = string.Format("{0}", Convert.ToDecimal(e.Value));
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
        protected void ASPxGridView2_CustomSummaryCalculate(object sender, DevExpress.Data.CustomSummaryEventArgs e)
        {
            ASPxSummaryItem item = e.Item as ASPxSummaryItem;

            if (e.SummaryProcess == DevExpress.Data.CustomSummaryProcess.Start)
            {
                Number2.Clear();

                totalCount2 = 0;
                totalCountrecp2 = 0;


            }
            if (e.SummaryProcess == DevExpress.Data.CustomSummaryProcess.Calculate)
            {

                string val = Convert.ToString(e.FieldValue);
                if (!Number2.Contains(val))
                {
                    totalCount2++;
                    Number2.Add(val);
                }


            }
            if (e.SummaryProcess == DevExpress.Data.CustomSummaryProcess.Finalize)
            {

                if (e.Item == ShowGrid2.TotalSummary["Number"])
                {
                    e.TotalValue = string.Format("Doc Count={0}", totalCount2);
                }

            }



        }

        #endregion

        #region  ############# Debit  Note  #################
                
        protected void grid_debitNote_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            string returnPara = Convert.ToString(e.Parameters);
            Session["GSTR_DebitNote"] = null;
            string WhichCall = returnPara.Split('~')[0];
            if (WhichCall == "ListData")
            {
                string WhichCall2 = returnPara.Split('~')[1];
                string COMPANYID = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
                string Finyear = Convert.ToString(HttpContext.Current.Session["LastFinYear"]);


                string BRANCH_ID = "";
                string QuoComponent2 = "";
                List<object> QuoList2 = lookup_branch.GridView.GetSelectedFieldValues("branch_id");
                foreach (object Quo2 in QuoList2)
                {
                    QuoComponent2 += "," + Quo2;
                }

                if (hdnSelectedBranches.Value != "")
                {
                    BRANCH_ID = hdnSelectedBranches.Value;
                }

                BRANCH_ID = QuoComponent2.TrimStart(',');

                Task PopulateStockTrialDataTask = new Task(() => GetDebitNote(WhichCall2, BRANCH_ID));
                PopulateStockTrialDataTask.RunSynchronously();

            }
        }
        protected void grid_debitNote_DataBinding(object sender, EventArgs e)
        {
            if (Session["GSTR_DebitNote"] != null)
            {
                grid_debitNote.DataSource = (DataTable)Session["GSTR_DebitNote"];
            }
        }
        protected void grid_debitNote_SummaryDisplayText(object sender, ASPxGridViewSummaryDisplayTextEventArgs e)
        {
            if (e.Item == grid_debitNote.TotalSummary["TaxableAmount"])
            {
                e.Text = string.Format("Total={0}", e.Value);
            }
            else if (e.Item == grid_debitNote.TotalSummary["CGSTAmount"])
            {
                e.Text = string.Format("Total={0}", e.Value);
            }
            else if (e.Item == grid_debitNote.TotalSummary["SGSTAmount"])
            {
                e.Text = string.Format("Total={0}", e.Value);
            }
            else if (e.Item == grid_debitNote.TotalSummary["IGSTAmount"])
            {
                e.Text = string.Format("Total={0}", e.Value);
            }
            else if (e.Item == grid_debitNote.TotalSummary["UTGSTAmount"])
            {
                e.Text = string.Format("Total={0}", e.Value);
            }
            else if (e.Item == grid_debitNote.TotalSummary["OtherAmount"])
            {
                e.Text = string.Format("Total={0}", e.Value);
            }
            else if (e.Item == grid_debitNote.TotalSummary["NetAmount"])
            {
                e.Text = string.Format("Total={0}", e.Value);
            }
        }
        public void GetDebitNote(string Gstn, string Branch)
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


                dttab = GetGstrDebitNote(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]).Trim(), "debitnote", Gstn, FROMDATE, TODATE, Branch);

                if (dttab.Rows.Count > 0)
                {
                    Session["GSTR_DebitNote"] = dttab;
                    grid_debitNote.DataSource = dttab;
                    grid_debitNote.DataBind();
                }
                else
                {
                    Session["GSTR_DebitNote"] = null;
                    grid_debitNote.DataSource = null;
                    grid_debitNote.DataBind();
                }
            }
            catch (Exception ex)
            {
            }
        }
        public DataTable GetGstrDebitNote(string Company, string Finyear, string Action, string Gstn, string FROMDATE, string TODATE, string Branch)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("GST_Salepurchase");
            proc.AddPara("@COMPANYID", Company);
            proc.AddPara("@FINYEAR", Finyear);
            proc.AddPara("@Acton", Action);
            proc.AddPara("@FROMDATE", FROMDATE);
            proc.AddPara("@TODATE", TODATE);
            proc.AddPara("@GSTIN", Gstn);
            proc.AddPara("@P_BRANCH_ID", Branch);
            proc.AddPara("@INVENTORY", ddlinventory.SelectedValue);
            ds = proc.GetTable();
            return ds;
        }

        #endregion

        #region  ############# Cerdit  Note  #################

        protected void grid_creditNote_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            string returnPara = Convert.ToString(e.Parameters);
            Session["GSTR_DebitNote"] = null;
            string WhichCall = returnPara.Split('~')[0];
            if (WhichCall == "ListData")
            {
                string WhichCall2 = returnPara.Split('~')[1];
                string COMPANYID = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
                string Finyear = Convert.ToString(HttpContext.Current.Session["LastFinYear"]);


                string BRANCH_ID = "";
                string QuoComponent2 = "";
                List<object> QuoList2 = lookup_branch.GridView.GetSelectedFieldValues("branch_id");
                foreach (object Quo2 in QuoList2)
                {
                    QuoComponent2 += "," + Quo2;
                }

                if (hdnSelectedBranches.Value != "")
                {
                    BRANCH_ID = hdnSelectedBranches.Value;
                }

                BRANCH_ID = QuoComponent2.TrimStart(',');

                Task PopulateStockTrialDataTask = new Task(() => GetCreditNote(WhichCall2, BRANCH_ID));
                PopulateStockTrialDataTask.RunSynchronously();

            }
        }
        protected void grid_creditNote_DataBinding(object sender, EventArgs e)
        {
            if (Session["GSTR_DebitNote"] != null)
            {
                grid_creditNote.DataSource = (DataTable)Session["GSTR_DebitNote"];
            }
        }
        protected void grid_creditNote_SummaryDisplayText(object sender, ASPxGridViewSummaryDisplayTextEventArgs e)
        {
            if (e.Item == grid_creditNote.TotalSummary["TaxableAmount"])
            {
                e.Text = string.Format("Total={0}", e.Value);
            }
            else if (e.Item == grid_creditNote.TotalSummary["CGSTAmount"])
            {
                e.Text = string.Format("Total={0}", e.Value);
            }
            else if (e.Item == grid_creditNote.TotalSummary["SGSTAmount"])
            {
                e.Text = string.Format("Total={0}", e.Value);
            }
            else if (e.Item == grid_creditNote.TotalSummary["IGSTAmount"])
            {
                e.Text = string.Format("Total={0}", e.Value);
            }
            else if (e.Item == grid_creditNote.TotalSummary["UTGSTAmount"])
            {
                e.Text = string.Format("Total={0}", e.Value);
            }
            else if (e.Item == grid_creditNote.TotalSummary["OtherAmount"])
            {
                e.Text = string.Format("Total={0}", e.Value);
            }
            else if (e.Item == grid_creditNote.TotalSummary["NetAmount"])
            {
                e.Text = string.Format("Total={0}", e.Value);
            }
        }
        public void GetCreditNote(string Gstn, string Branch)
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


                dttab = GetGstrDebitNote(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]).Trim(), "creditnote", Gstn, FROMDATE, TODATE, Branch);

                if (dttab.Rows.Count > 0)
                {
                    Session["GSTR_DebitNote"] = dttab;
                    grid_creditNote.DataSource = dttab;
                    grid_creditNote.DataBind();
                }
                else
                {
                    Session["GSTR_DebitNote"] = null;
                    grid_creditNote.DataSource = null;
                    grid_creditNote.DataBind();
                }
            }
            catch (Exception ex)
            {
            }
        }

        #endregion
    }
}