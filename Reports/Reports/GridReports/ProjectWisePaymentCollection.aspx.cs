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
using System.Threading.Tasks;
using DevExpress.XtraPrinting;
using DevExpress.Export;
using System.Drawing;
using Reports.Model;
using System.Data.SqlClient;
using DataAccessLayer;

namespace Reports.Reports.GridReports
{
    public partial class ProjectWisePaymentCollection : System.Web.UI.Page
    {
        BusinessLogicLayer.Reports objReport = new BusinessLogicLayer.Reports();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        ArrayList amountfields_caption = null;
        ArrayList amountfields_fields = null;
        ArrayList bandedfields = null;

        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string sPath = Convert.ToString(HttpContext.Current.Request.Url);
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            rights = new UserRightsForPage();
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/GridReports/ProjectWisePaymentCollection.aspx");
            DateTime dtFrom;
            DateTime dtTo;

            if (!IsPostBack)
            {
                DataTable dtBranchSelection = new DataTable();
                string GridHeader = "";
                BusinessLogicLayer.Reports GridHeaderDet = new BusinessLogicLayer.Reports();
                RptHeading.Text = "Project Wise Payment/Collection";
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

                Session["exportval"] = null;
                Session["ProjwisePayCollec"] = null;
                Session["ProjwisePayCollecTotal"] = null;
                Session["RECPAY"] = null;
                Session["ProjwisePayCollecHeadersAmountCaption"] = null;
                Session["ProjwisePayCollecHeadersAmountFields"] = null;
                Session["ProjwisePayCollecGridviewsBandedFields"] = null;

                BranchHoOffice();

                dtFrom = DateTime.Now;
                dtTo = DateTime.Now;
                ASPxAsOnDate.Value = DateTime.Now;
                Date_finyearwise(Convert.ToString(Session["LastFinYear"]));
                Session["BranchNames"] = null;
                dtBranchSelection = oDBEngine.GetDataTable("select Variable_Value from Config_SystemSettings where Variable_Name='ReportsBranchSelection'");
                hdnBranchSelection.Value = dtBranchSelection.Rows[0][0].ToString();
            }

            if (ShowGridProjPayCollection.Columns["PROJ_CODE"] == null)
            {
                //==============By default Grid populate in load===================
                DataTable dt = new DataTable();
                dt.Columns.Add("BRANCHDESC");
                dt.Columns.Add("PROJ_CODE");
                dt.Columns.Add("PROJ_NAME");
                dt.Columns.Add("PARTYNAME");

                GridViewDataTextColumn col1 = new GridViewDataTextColumn();
                col1.FieldName = "BRANCHDESC";
                col1.Caption = "Units";
                col1.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                col1.Width = 350;
                ShowGridProjPayCollection.Columns.Add(col1);

                GridViewDataTextColumn col2 = new GridViewDataTextColumn();
                col2.FieldName = "PROJ_CODE";
                col2.Caption = "Project Code";
                col2.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                col2.Width = 350;
                ShowGridProjPayCollection.Columns.Add(col2);

                GridViewDataTextColumn col3 = new GridViewDataTextColumn();
                col3.FieldName = "PROJ_NAME";
                col3.Caption = "Project Name";
                col3.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                col3.Width = 350;
                ShowGridProjPayCollection.Columns.Add(col3);

                GridViewDataTextColumn col4 = new GridViewDataTextColumn();
                col4.FieldName = "PARTYNAME";
                col4.Caption = "Customer";
                col4.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                col4.Width = 350;
                ShowGridProjPayCollection.Columns.Add(col4);
                //=====================================================================

            }
            if (!IsPostBack)
            {
                dtTo = Convert.ToDateTime(ASPxAsOnDate.Date);
                string BRANCH_ID = "";
                if (hdnSelectedBranches.Value != "")
                {
                    BRANCH_ID = hdnSelectedBranches.Value;
                }
            }
        }

        public void Date_finyearwise(string Finyear)
        {
            CommonBL cbl = new CommonBL();
            DataTable tcbl = new DataTable();

            tcbl = cbl.GetDateFinancila(Finyear);
            if (tcbl.Rows.Count > 0)
            {
                ASPxAsOnDate.MaxDate = Convert.ToDateTime((tcbl.Rows[0]["FinYear_EndDate"]));
                ASPxAsOnDate.MinDate = Convert.ToDateTime((tcbl.Rows[0]["FinYear_StartDate"]));

                DateTime MaximumDate = Convert.ToDateTime((tcbl.Rows[0]["FinYear_EndDate"]));
                DateTime MinimumDate = Convert.ToDateTime((tcbl.Rows[0]["FinYear_StartDate"]));

                DateTime TodayDate = Convert.ToDateTime(DateTime.Now);
                DateTime FinYearEndDate = MaximumDate;

                if (TodayDate > FinYearEndDate)
                {
                    ASPxAsOnDate.Date = FinYearEndDate;
                }
                else
                {
                    ASPxAsOnDate.Date = TodayDate;
                }
            }

        }

        public void BranchHoOffice()
        {
            CommonBL cbl = new CommonBL();
            DataTable tcbl = new DataTable();
            DataTable dtBranchChild = new DataTable();
            tcbl = cbl.GetBranchheadoffice(Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]), "HO");
            if (tcbl.Rows.Count > 0)
            {
                ddlbranchHO.DataSource = tcbl;
                ddlbranchHO.DataTextField = "Code";
                ddlbranchHO.DataValueField = "branch_id";
                ddlbranchHO.DataBind();
                dtBranchChild = GetChildBranch(Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));
                if (dtBranchChild.Rows.Count > 0)
                {
                    ddlbranchHO.Items.Insert(0, new ListItem("All", "All"));
                }
            }
        }

        public DataTable GetChildBranch(string CHILDBRANCH)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            SqlCommand cmd = new SqlCommand("PRC_FINDCHILDBRANCH_REPORT", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CHILDBRANCH", CHILDBRANCH);
            cmd.CommandTimeout = 0;
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(dt);
            cmd.Dispose();
            con.Dispose();
            return dt;
        }

        protected void Componentbranch_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            string FinYear = Convert.ToString(Session["LastFinYear"]);

            if (e.Parameter.Split('~')[0] == "BindComponentGrid")
            {
                DataTable ComponentTable = new DataTable();
                string Hoid = e.Parameter.Split('~')[1];
                if (Hoid != "All")
                {
                    ComponentTable = GetBranch(Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]), Hoid);
                    if (ComponentTable.Rows.Count > 0)
                    {
                        Session["SI_ComponentData_Branch"] = ComponentTable;
                        lookup_branch.DataSource = ComponentTable;
                        lookup_branch.DataBind();
                    }
                    else
                    {
                        Session["SI_ComponentData_Branch"] = ComponentTable;
                        lookup_branch.DataSource = null;
                        lookup_branch.DataBind();
                    }
                }
                else
                {
                    ComponentTable = oDBEngine.GetDataTable("select * from (select branch_id as ID,branch_description,branch_code from tbl_master_branch a where a.branch_id=1  union all select branch_id as ID,branch_description,branch_code from tbl_master_branch b where b.branch_parentId=1) a order by branch_description");
                    if (ComponentTable.Rows.Count > 0)
                    {
                        Session["SI_ComponentData_Branch"] = ComponentTable;
                        lookup_branch.DataSource = ComponentTable;
                        lookup_branch.DataBind();
                    }
                }
            }
        }

        public DataTable GetBranch(string BRANCH_ID, string Ho)
        {
            DataTable dt = new DataTable();
            //SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);
            SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            SqlCommand cmd = new SqlCommand("GetFinancerBranchfetchhowise", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Branch", BRANCH_ID);
            cmd.Parameters.AddWithValue("@Hoid", Ho);
            cmd.CommandTimeout = 0;
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(dt);
            cmd.Dispose();
            con.Dispose();

            return dt;
        }

        protected void lookup_branch_DataBinding(object sender, EventArgs e)
        {
            if (Session["SI_ComponentData_Branch"] != null)
            {
                lookup_branch.DataSource = (DataTable)Session["SI_ComponentData_Branch"];
            }
        }
        #region Export


        public void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(Convert.ToString(drdExport.SelectedItem.Value));
            if (Filter != 0)
            {
                if (Session["exportval"] == null)
                {
                    Session["exportval"] = Filter;
                    bindexport(Filter);
                }
                else if (Convert.ToInt32(Session["exportval"]) != Filter)
                {
                    Session["exportval"] = Filter;
                    bindexport(Filter);
                }
            }
        }
        public void bindexport(int Filter)
        {

            string filename = "ProjwisePayCollection_Report";
            exporter.FileName = filename;
            string FileHeader = "";

            BusinessLogicLayer.Reports RptHeader = new BusinessLogicLayer.Reports();

            FileHeader = RptHeader.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), true, true, true, true, true, true) + Environment.NewLine + "Project Wise Payment/Collection" + Environment.NewLine + "As On " + Convert.ToDateTime(ASPxAsOnDate.Date).ToString("dd-MM-yyyy");

            exporter.PageHeader.Left = FileHeader;
            exporter.PageHeader.Font.Size = 10;
            exporter.PageHeader.Font.Name = "Tahoma";

            exporter.GridViewID = "ShowGridProjPayCollection";
            exporter.RenderBrick += exporter_RenderBrick;
            switch (Filter)
            {
                case 1:
                    exporter.WriteXlsxToResponse(new XlsxExportOptionsEx() { ExportType = ExportType.WYSIWYG });
                    break;
                case 2:
                    exporter.WritePdfToResponse();
                    break;
                case 3:
                    exporter.WriteCsvToResponse();
                    break;
                case 4:
                    exporter.WriteRtfToResponse();
                    break;
                default:
                    return;
            }

        }

        protected void exporter_RenderBrick(object sender, ASPxGridViewExportRenderingEventArgs e)
        {
            e.BrickStyle.BackColor = Color.White;
            e.BrickStyle.ForeColor = Color.Black;
        }
        #endregion

        #region Project Populate

        protected void Project_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            if (e.Parameter.Split('~')[0] == "BindProjectGrid")
            {
                string Customerid = "";
                DataTable ProjectTable = new DataTable();
                ProjectTable = GetProject(Customerid);

                if (ProjectTable.Rows.Count > 0)
                {
                    Session["ProjectData"] = ProjectTable;
                    lookup_project.DataSource = ProjectTable;
                    lookup_project.DataBind();
                }
                else
                {
                    Session["ProjectData"] = ProjectTable;
                    lookup_project.DataSource = null;
                    lookup_project.DataBind();
                }
            }
            else if (e.Parameter.Split('~')[0] == "BindProjectGridwithCustomer")
            {
                string Customerid = e.Parameter.Split('~')[1];
                DataTable ProjectTable = new DataTable();
                ProjectTable = GetProject(Customerid);

                if (ProjectTable.Rows.Count > 0)
                {
                    Session["ProjectData"] = ProjectTable;
                    lookup_project.DataSource = ProjectTable;
                    lookup_project.DataBind();
                }
                else
                {
                    Session["ProjectData"] = ProjectTable;
                    lookup_project.DataSource = null;
                    lookup_project.DataBind();
                }
            }
        }

        public DataTable GetProject(string Custid)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            SqlCommand cmd = new SqlCommand("PRC_FETCHPROJECTSWITHOTHERPARAMETERS_REPORT", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@OTHERSPARAM", Custid);
            cmd.CommandTimeout = 0;
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(dt);
            cmd.Dispose();
            con.Dispose();

            return dt;
        }

        protected void lookup_project_DataBinding(object sender, EventArgs e)
        {
            if (Session["ProjectData"] != null)
            {
                lookup_project.DataSource = (DataTable)Session["ProjectData"];
            }
        }

        #endregion

        #region Project Wise Payment/Collection grid
        protected void ShowGridProjPayCollection_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            ShowGridProjPayCollection.DataSource = null;
            Session["ProjwisePayCollec"] = null;
            Session["ProjwisePayCollecTotal"] = null;
            Session["RECPAY"] = null;
            Session["ProjwisePayCollecHeadersAmountCaption"] = null;
            Session["ProjwisePayCollecHeadersAmountFields"] = null;
            Session["ProjwisePayCollecGridviewsBandedFields"] = null;

            string returnPara = Convert.ToString(e.Parameters);

            DateTime dtTodate;

            dtTodate = Convert.ToDateTime(ASPxAsOnDate.Date);
            string ASONDATE = dtTodate.ToString("yyyy-MM-dd");

            string BRANCH_ID = "";
            string BranchComponent = "";
            List<object> BranchList = lookup_branch.GridView.GetSelectedFieldValues("ID");
            foreach (object Brnch in BranchList)
            {
                BranchComponent += "," + Brnch;
            }
            BRANCH_ID = BranchComponent.TrimStart(',');

            string PROJECT_ID = "";
            string Projects = "";
            List<object> ProjectList = lookup_project.GridView.GetSelectedFieldValues("ID");
            foreach (object Project in ProjectList)
            {
                Projects += "," + Project;
            }
            PROJECT_ID = Projects.TrimStart(',');

            Task PopulateStockTrialDataTask = new Task(() => GetProjwisePayCollectiondata(ASONDATE, BRANCH_ID,PROJECT_ID));
            PopulateStockTrialDataTask.RunSynchronously();
        }
        public void GetProjwisePayCollectiondata(string ASONDATE, string BRANCH_ID, string PROJECT_ID)
        {
            try
            {
                DataTable dt_FinYear = new DataTable();
                DataSet ds = new DataSet();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("PRC_PROJECTWISEPAYMENTCOLLECTION_REPORT", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@COMPANYID", Convert.ToString(Session["LastCompany"]));
                cmd.Parameters.AddWithValue("@FINYEAR", Convert.ToString(Session["LastFinYear"]));
                cmd.Parameters.AddWithValue("@ASONDATE", ASONDATE);
                cmd.Parameters.AddWithValue("@BRANCHID", BRANCH_ID);
                cmd.Parameters.AddWithValue("@PROJECT_ID", PROJECT_ID);

                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(ds);

                cmd.Dispose();
                con.Dispose();

                DataView dvData = new DataView(ds.Tables[0]);
                dvData.RowFilter = "PROJ_ID <> 9999999999";
                Session["ProjwisePayCollec"] = dvData.ToTable();

                DataView dvData1 = new DataView(ds.Tables[0]);
                dvData1.RowFilter = "PROJ_ID = 9999999999";
                Session["ProjwisePayCollecTotal"] = dvData1.ToTable();

                dt_FinYear = GetFinYear();
                Session["RECPAY"] = dt_FinYear.Rows[0][0];

                ShowGridProjPayCollection.DataSource = dvData.ToTable();
                ShowGridProjPayCollection.DataBind();
            }
            catch (Exception ex)
            {

            }
        }

        public DataTable GetFinYear()
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            SqlCommand cmd = new SqlCommand("PRC_FINDTOTALFINANCIALYEAR_REPORT", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 0;
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(dt);
            cmd.Dispose();
            con.Dispose();

            return dt;
        }

        protected void ShowGridProjPayCollection_DataBinding(object sender, EventArgs e)
        {
            DataTable dt_ProjwisePayCollec = (DataTable)Session["ProjwisePayCollec"];

            if (dt_ProjwisePayCollec.Rows.Count > 0)
            {
                int maxColumnIndex = ShowGridProjPayCollection.Columns.Count - 1;
                for (int i = maxColumnIndex; i >= 0; i--)
                {
                    ShowGridProjPayCollection.Columns.RemoveAt(i);
                }
            }

            if (dt_ProjwisePayCollec.Rows.Count > 0)
            {
                string fieldname = "";
                GridViewDataTextColumn col1 = new GridViewDataTextColumn();
                col1.FieldName = "BRANCHDESC";
                col1.Caption = "Units";
                col1.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                col1.Width = 300;
                ShowGridProjPayCollection.Columns.Add(col1);

                GridViewDataTextColumn col2 = new GridViewDataTextColumn();
                col2.FieldName = "PROJ_CODE";
                col2.Caption = "Project Code";
                col2.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                col2.Width = 200;
                ShowGridProjPayCollection.Columns.Add(col2);

                GridViewDataTextColumn col3 = new GridViewDataTextColumn();
                col3.FieldName = "PROJ_NAME";
                col3.Caption = "Project Name";
                col3.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                col3.Width = 200;
                ShowGridProjPayCollection.Columns.Add(col3);

                GridViewDataTextColumn col4 = new GridViewDataTextColumn();
                col4.FieldName = "PARTYNAME";
                col4.Caption = "Customer";
                col4.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                col4.Width = 250;
                ShowGridProjPayCollection.Columns.Add(col4);

                if (Session["RECPAY"] != null)
                {
                    ArrayList RecYear = null;
                    RecYear = new ArrayList((Session["RECPAY"].ToString()).Split(new char[] { ',' }));

                    GridViewBandColumn bandColumn = new GridViewBandColumn();
                    GridViewDataTextColumn coldyn = new GridViewDataTextColumn();
                    for (int i = 0; i < RecYear.Count; i++)
                    {
                           fieldname = "R_" + RecYear[i].ToString();
                            coldyn = new GridViewDataTextColumn();
                            coldyn.FieldName = fieldname;
                            if (RecYear.Count-1 == i)
                            {
                                coldyn.Caption = "Current Year";
                            }
                            else
                            {
                                coldyn.Caption = RecYear[i].ToString();
                            }
                            coldyn.HeaderStyle.HorizontalAlign = HorizontalAlign.Right;
                            coldyn.CellStyle.HorizontalAlign = HorizontalAlign.Right;
                            coldyn.Width = 110;
                            coldyn.PropertiesTextEdit.DisplayFormatString = "#####,##,##,###0.00;";
                            coldyn.Settings.AllowAutoFilter = DevExpress.Utils.DefaultBoolean.False;
                            bandColumn.Columns.Add(coldyn);
                            
                            if (i == 0)
                            {
                                if (RecYear.Count - 1 == i)
                                {
                                    hdnProjwisePayCollecNoCaption.Value = "Current Year";
                                }
                                else
                                {
                                    hdnProjwisePayCollecNoCaption.Value = RecYear[i].ToString();
                                }
                                hdnProjwisePayCollecNoFields.Value = "R_" + RecYear[i].ToString();
                            }
                            else {
                                if (RecYear.Count - 1 == i)
                                {
                                    hdnProjwisePayCollecNoCaption.Value = hdnProjwisePayCollecNoCaption.Value + "~" + "Current Year";
                                }
                                else
                                {
                                    hdnProjwisePayCollecNoCaption.Value = hdnProjwisePayCollecNoCaption.Value + "~" + RecYear[i].ToString();
                                }
                                hdnProjwisePayCollecNoFields.Value = hdnProjwisePayCollecNoFields.Value + "~" + "R_"+RecYear[i].ToString();
                            }
                    }

                    fieldname = "R_Total";
                    coldyn = new GridViewDataTextColumn();
                    coldyn.FieldName = fieldname;
                    coldyn.Caption = "Total";
                    coldyn.HeaderStyle.HorizontalAlign = HorizontalAlign.Right;
                    coldyn.CellStyle.HorizontalAlign = HorizontalAlign.Right;
                    coldyn.Width = 110;
                    coldyn.HeaderStyle.Font.Bold = true;
                    coldyn.CellStyle.Font.Bold = true;
                    coldyn.PropertiesTextEdit.DisplayFormatString = "#####,##,##,###0.00;";
                    coldyn.Settings.AllowAutoFilter = DevExpress.Utils.DefaultBoolean.False;
                    bandColumn.Columns.Add(coldyn);

                    hdnProjwisePayCollecNoCaption.Value = hdnProjwisePayCollecNoCaption.Value + "~" + "Total";
                    hdnProjwisePayCollecNoFields.Value = hdnProjwisePayCollecNoFields.Value + "~" + "R_Total";

                    bandColumn.Caption = "Collection (As On)";
                    bandColumn.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    ShowGridProjPayCollection.Columns.Add(bandColumn);

                    hdnProjwisePayCollecNoBandedColumn.Value = "Collection (As On)";
                }

                if (Session["RECPAY"] != null)
                {
                    ArrayList PayYear = null;
                    PayYear = new ArrayList((Session["RECPAY"].ToString()).Split(new char[] { ',' }));

                    GridViewBandColumn bandColumn = new GridViewBandColumn();
                    GridViewDataTextColumn coldyn = new GridViewDataTextColumn();
                    for (int j = 0; j < PayYear.Count; j++)
                    {
                        fieldname = "P_" + PayYear[j].ToString();
                        coldyn = new GridViewDataTextColumn();
                        coldyn.FieldName = fieldname;
                        if (PayYear.Count-1 == j)
                        {
                            coldyn.Caption = "Current Year";
                        }
                        else
                        {
                            coldyn.Caption = PayYear[j].ToString();
                        }
                        coldyn.HeaderStyle.HorizontalAlign = HorizontalAlign.Right;
                        coldyn.CellStyle.HorizontalAlign = HorizontalAlign.Right;
                        coldyn.Width = 110;
                        coldyn.PropertiesTextEdit.DisplayFormatString = "#####,##,##,###0.00;";
                        coldyn.Settings.AllowAutoFilter = DevExpress.Utils.DefaultBoolean.False;
                        bandColumn.Columns.Add(coldyn);

                        if (PayYear.Count - 1 == j)
                        {
                            hdnProjwisePayCollecNoCaption.Value = hdnProjwisePayCollecNoCaption.Value + "~" + "Current Year";
                        }
                        else
                        {
                            hdnProjwisePayCollecNoCaption.Value = hdnProjwisePayCollecNoCaption.Value + "~" + PayYear[j].ToString();
                        }
                        hdnProjwisePayCollecNoFields.Value = hdnProjwisePayCollecNoFields.Value + "~" + "P_" + PayYear[j].ToString();
                    }

                    fieldname = "P_Total";
                    coldyn = new GridViewDataTextColumn();
                    coldyn.FieldName = fieldname;
                    coldyn.Caption = "Total";
                    coldyn.HeaderStyle.HorizontalAlign = HorizontalAlign.Right;
                    coldyn.CellStyle.HorizontalAlign = HorizontalAlign.Right;
                    coldyn.Width = 110;
                    coldyn.HeaderStyle.Font.Bold = true;
                    coldyn.CellStyle.Font.Bold = true;
                    coldyn.PropertiesTextEdit.DisplayFormatString = "#####,##,##,###0.00;";
                    coldyn.Settings.AllowAutoFilter = DevExpress.Utils.DefaultBoolean.False;
                    bandColumn.Columns.Add(coldyn);

                    hdnProjwisePayCollecNoCaption.Value = hdnProjwisePayCollecNoCaption.Value + "~" + "Total";
                    hdnProjwisePayCollecNoFields.Value = hdnProjwisePayCollecNoFields.Value + "~" + "P_Total";

                    bandColumn.Caption = "Payment / Expenses (As On)";
                    bandColumn.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    ShowGridProjPayCollection.Columns.Add(bandColumn);

                    hdnProjwisePayCollecNoBandedColumn.Value = hdnProjwisePayCollecNoBandedColumn.Value + "~" + "Payment / Expenses (As On)";
                }

                if (Session["ProjwisePayCollecGridviewsBandedFields"] == null)
                {
                    Session["ProjwisePayCollecGridviewsBandedFields"] = hdnProjwisePayCollecNoBandedColumn.Value;
                    Session["ProjwisePayCollecHeadersAmountCaption"] = hdnProjwisePayCollecNoCaption.Value;
                    Session["ProjwisePayCollecHeadersAmountFields"] = hdnProjwisePayCollecNoFields.Value;
                }

                ShowGridProjPayCollection.DataSource = dt_ProjwisePayCollec;
            }
        }

        protected void ShowGridProjPayCollection_HtmlFooterCellPrepared(object sender, ASPxGridViewTableFooterCellEventArgs e)
        {
            if (IsPostBack)
            {
                DataTable dt_ProjwisePayCollecSummaryTotal = (DataTable)Session["ProjwisePayCollecTotal"];
                if (dt_ProjwisePayCollecSummaryTotal.Rows.Count > 0)
                {

                    if (amountfields_caption == null)
                    {
                        amountfields_caption = new ArrayList((Session["ProjwisePayCollecHeadersAmountCaption"].ToString()).Split(new char[] { '~' }));
                        amountfields_fields = new ArrayList((Session["ProjwisePayCollecHeadersAmountFields"].ToString()).Split(new char[] { '~' }));
                        bandedfields = new ArrayList((Session["ProjwisePayCollecGridviewsBandedFields"].ToString()).Split(new char[] { '~' }));
                    }
                    else
                    {
                        if (e.Column.Caption != "Units" && e.Column.Caption != "Project Code" && e.Column.Caption != "Project Name" && e.Column.Caption != "Customer")
                        {
                            if (e.Column.ParentBand.ToString() == bandedfields[(e.Column.ParentBand.VisibleIndex) - 4].ToString())
                            {
                                if (e.Column.Caption == amountfields_caption[0].ToString())
                                {
                                    if (e.IsTotalFooter)
                                    {
                                        e.Cell.Text = dt_ProjwisePayCollecSummaryTotal.Rows[0][amountfields_fields[0].ToString()].ToString();
                                        if (e.Column.Caption == "Total")
                                        {
                                            e.Cell.Font.Bold = true;
                                        }
                                        amountfields_fields.RemoveAt(0);
                                        amountfields_caption.RemoveAt(0);
                                    }
                                }
                            }
                        }
                    }
                }
                if (e.Column.Caption == "Customer")
                {
                    if (e.IsTotalFooter)
                    {
                        e.Cell.Text = "Net Total:";
                    }
                }
            }

        }
    }
        #endregion
}