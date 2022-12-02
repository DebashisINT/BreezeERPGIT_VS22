using BusinessLogicLayer;
using DataAccessLayer;
using DevExpress.Export;
using DevExpress.Web;
using DevExpress.XtraPrinting;
using EntityLayer.CommonELS;
using Reports.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Reports.Reports.GridReports
{
    public partial class STBReceivingRegister : System.Web.UI.Page
    {
        CommonBL ComBL = new CommonBL();
        String userbranch = null;

        DateTime dtFrom;
        DateTime dtTo;
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();

        protected void Page_Load(object sender, EventArgs e)
        {
            string MultiBranchNumberingScheme = ComBL.GetSystemSettingsResult("NumberingSchemeWithMultiBranch");
            rights = new UserRightsForPage();
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/GridReports/SRVMaterialInOutRegisterDetail.aspx");
            if (!IsPostBack)
            {
                Session["chk_MatIORegDettotal"] = 0;
                string GridHeader = "";
                BusinessLogicLayer.Reports GridHeaderDet = new BusinessLogicLayer.Reports();
                RptHeading.Text = "STB Receiving Register";
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

                drdExport.SelectedIndex = 0;
                Session["IsSrvMatIORegDetFilter"] = null;
                dtFrom = DateTime.Now;
                dtTo = DateTime.Now;
                ASPxFromDate.Text = dtFrom.ToString("dd-MM-yyyy");
                ASPxToDate.Text = dtTo.ToString("dd-MM-yyyy");
                ASPxFromDate.Value = DateTime.Now;
                ASPxToDate.Value = DateTime.Now;

                dtFrom = Convert.ToDateTime(ASPxFromDate.Date);
                dtTo = Convert.ToDateTime(ASPxToDate.Date);

                Date_finyearwise(Convert.ToString(Session["LastFinYear"]));

                Session["exportval"] = null;
                Session["IsSrvRegisterFilter"] = null;
                if (!String.IsNullOrEmpty(MultiBranchNumberingScheme))
                {
                    if (MultiBranchNumberingScheme.ToUpper().Trim() == "YES")
                    {
                        userbranch = EmployeeBranchMap();
                    }
                    else
                    {
                        userbranch = Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]);
                    }
                }
                else
                {
                    userbranch = Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]);
                }
                Session["UserBranchMapID"] = userbranch;
            }
            else
            {
                dtFrom = Convert.ToDateTime(ASPxFromDate.Date);
                dtTo = Convert.ToDateTime(ASPxToDate.Date);
            }
        }

        public String EmployeeBranchMap()
        {
            String branches = null;
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_EmployeeBranchMap");
            proc.AddVarcharPara("@USER_ID", 100, Session["userid"].ToString());
            ds = proc.GetTable();
            if (ds != null && ds.Rows.Count > 0)
            {
                branches = ds.Rows[0]["BranchId"].ToString();
            }
            return branches;
        }

        #region Export
        public void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(Convert.ToString(drdExport.SelectedItem.Value));
            if (Filter != 0)
            {
                if (Session["exportval"] == null)
                {
                    //Session["exportval"] = Filter;
                    bindexport(Filter);
                }
                else if (Convert.ToInt32(Session["exportval"]) != Filter)
                {
                    //Session["exportval"] = Filter;
                    bindexport(Filter);
                }
                drdExport.SelectedValue = "0";
            }
        }

        public void bindexport(int Filter)
        {
            string filename = "STBReceivingRegister";
            exporter.FileName = filename;
            string FileHeader = "";

            BusinessLogicLayer.Reports RptHeader = new BusinessLogicLayer.Reports();

            FileHeader = RptHeader.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), true, true, true, true, true, true) + Environment.NewLine + "STB Receiving Register" + Environment.NewLine + "For the period " + Convert.ToDateTime(ASPxFromDate.Date).ToString("dd-MM-yyyy") + " To " + Convert.ToDateTime(ASPxToDate.Date).ToString("dd-MM-yyyy");
            exporter.PageHeader.Left = FileHeader;
            exporter.PageHeader.Font.Size = 10;
            exporter.PageHeader.Font.Name = "Tahoma";

            exporter.Landscape = true;
            exporter.MaxColumnWidth = 100;
            exporter.GridViewID = "ShowGrid";
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
            }
        }

        public string ReplaceFirst(string text, string search, string replace)
        {
            int pos = text.IndexOf(search);
            if (pos < 0)
            {
                return text;
            }
            return text.Substring(0, pos) + Environment.NewLine + replace + Environment.NewLine + text.Substring(pos + search.Length);
        }

        protected void exporter_RenderBrick(object sender, ASPxGridViewExportRenderingEventArgs e)
        {
            e.BrickStyle.BackColor = Color.White;
            e.BrickStyle.ForeColor = Color.Black;
        }

        #endregion

        public void Date_finyearwise(string Finyear)
        {
            CommonBL stkledg = new CommonBL();
            DataTable dtstkledg = new DataTable();

            dtstkledg = stkledg.GetDateFinancila(Finyear);
            if (dtstkledg.Rows.Count > 0)
            {
                ASPxFromDate.MaxDate = Convert.ToDateTime((dtstkledg.Rows[0]["FinYear_EndDate"]));
                ASPxFromDate.MinDate = Convert.ToDateTime((dtstkledg.Rows[0]["FinYear_StartDate"]));

                ASPxToDate.MaxDate = Convert.ToDateTime((dtstkledg.Rows[0]["FinYear_EndDate"]));
                ASPxToDate.MinDate = Convert.ToDateTime((dtstkledg.Rows[0]["FinYear_StartDate"]));

                DateTime MaximumDate = Convert.ToDateTime((dtstkledg.Rows[0]["FinYear_EndDate"]));
                DateTime MinimumDate = Convert.ToDateTime((dtstkledg.Rows[0]["FinYear_StartDate"]));

                DateTime TodayDate = Convert.ToDateTime(DateTime.Now);
                DateTime FinYearEndDate = MaximumDate;

                if (TodayDate > FinYearEndDate)
                {
                    ASPxToDate.Date = FinYearEndDate;
                    ASPxFromDate.Date = MinimumDate;
                }
                else
                {
                    ASPxToDate.Date = TodayDate;
                    ASPxFromDate.Date = MinimumDate;
                }
            }
        }

        #region LinQ
        protected void GenerateEntityServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "SEQ";
            SqlConnection connectionString = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            string Userid = Convert.ToString(HttpContext.Current.Session["userid"]);
            ReportSourceDataContext dc = new ReportSourceDataContext(connectionString);

            if (Convert.ToString(Session["IsSrvMatIORegDetFilter"]) == "Y")
            {
                var q = from d in dc.SRVSTBRECEIVINGREGISTER_REPORTs
                        where Convert.ToString(d.USERID) == Userid && Convert.ToString(d.DETAILSID) != "9999999999"
                        orderby d.SEQ
                        select d;
                e.QueryableSource = q;
            }
            else
            {
                var q = from d in dc.SRVSTBRECEIVINGREGISTER_REPORTs
                        where Convert.ToString(d.SEQ) == "0"
                        orderby d.SEQ
                        select d;
                e.QueryableSource = q;
            }
            ShowGrid.ExpandAll();
        }
        #endregion

        #region EntityCode Populate

        protected void EntityCode_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            if (e.Parameter.Split('~')[0] == "BindEntityCodeGrid")
            {
                DataTable EntityCodeTable = new DataTable();
                string Hoid = e.Parameter.Split('~')[1];

                EntityCodeTable = oDBEngine.GetDataTable("select EntityCodeID as ID,EntityCodeDesc as EntityCode FROM SRV_Master_EntityCode order by EntityCodeDesc");

                if (EntityCodeTable.Rows.Count > 0)
                {
                    Session["EntityCodeData"] = EntityCodeTable;
                    lookup_EntityCode.DataSource = EntityCodeTable;
                    lookup_EntityCode.DataBind();
                }
                else
                {
                    Session["EntityCodeData"] = EntityCodeTable;
                    lookup_EntityCode.DataSource = null;
                    lookup_EntityCode.DataBind();
                }
            }
        }

        protected void lookup_EntityCode_DataBinding(object sender, EventArgs e)
        {
            if (Session["EntityCodeData"] != null)
            {
                lookup_EntityCode.DataSource = (DataTable)Session["EntityCodeData"];
            }
        }

        #endregion

        #region Location Populate

        protected void Location_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            if (e.Parameter.Split('~')[0] == "BindLocationGrid")
            {
                DataTable LocationTable = new DataTable();
                string Hoid = e.Parameter.Split('~')[1];

                if (Session["userbranchHierarchy"] != null)
                {
                    //LocationTable = oDBEngine.GetDataTable("select branch_id as ID,branch_description,branch_code from tbl_master_branch where branch_id in(" + Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]) + ") order by branch_description asc");
                    LocationTable = oDBEngine.GetDataTable("select branch_id as ID,branch_description,branch_code from tbl_master_branch where branch_id in(" + Convert.ToString(HttpContext.Current.Session["UserBranchMapID"]) + ") order by branch_description asc");
                }

                if (LocationTable.Rows.Count > 0)
                {
                    Session["LocationData"] = LocationTable;
                    lookup_Location.DataSource = LocationTable;
                    lookup_Location.DataBind();
                }
                else
                {
                    Session["LocationData"] = LocationTable;
                    lookup_Location.DataSource = null;
                    lookup_Location.DataBind();
                }
            }
        }

        protected void lookup_Location_DataBinding(object sender, EventArgs e)
        {
            if (Session["LocationData"] != null)
            {
                lookup_Location.DataSource = (DataTable)Session["LocationData"];
            }
        }

        #endregion

        protected void CallbackPanel_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            string IsSrvMatIORegDetFilter = Convert.ToString(hfIsSrvMatIORegDetFilter.Value);
            Session["IsSrvMatIORegDetFilter"] = IsSrvMatIORegDetFilter;

            dtFrom = Convert.ToDateTime(ASPxFromDate.Date);
            dtTo = Convert.ToDateTime(ASPxToDate.Date);

            string FROMDATE = dtFrom.ToString("yyyy-MM-dd");
            string TODATE = dtTo.ToString("yyyy-MM-dd");

            string Location_ID = "";

            string LocationID = "";
            List<object> LocationList = lookup_Location.GridView.GetSelectedFieldValues("ID");
            foreach (object lc in LocationList)
            {
                LocationID += "," + lc;
            }
            Location_ID = LocationID.TrimStart(',');

            if (Location_ID == "")
            {
                Location_ID = Convert.ToString(Session["UserBranchMapID"]);
            }

            string EntityCode_ID = "";
            string EntityCodeID = "";
            List<object> EntityCodeList = lookup_EntityCode.GridView.GetSelectedFieldValues("ID");
            foreach (object ec in EntityCodeList)
            {
                EntityCodeID += "," + ec;
            }
            EntityCode_ID = EntityCodeID.TrimStart(',');

            Task PopulateStockTrialDataTask = new Task(() => GetSTBReceivingRegisterDet(FROMDATE, TODATE, Location_ID, EntityCode_ID));
            PopulateStockTrialDataTask.RunSynchronously();
        }

        public void GetSTBReceivingRegisterDet(string FROMDATE, string TODATE, string Location_ID, String EntityCode_ID)
        {
            try
            {
                DataTable ds = new DataTable();
                ProcedureExecute proc = new ProcedureExecute("PRC_SRVSTBRECEIVINGREGISTER_REPORT");
                proc.AddPara("@COMPANYID", Convert.ToString(Session["LastCompany"]));
                proc.AddPara("@FINYEAR", Convert.ToString(Session["LastFinYear"]));
                proc.AddPara("@FROMDATE", FROMDATE);
                proc.AddPara("@TODATE", TODATE);
                proc.AddPara("@EntityCode", EntityCode_ID);
                proc.AddPara("@Location", Location_ID);
                proc.AddPara("@USERID", Convert.ToInt32(Session["userid"]));

                ds = proc.GetTable();

            }
            catch (Exception ex)
            {
            }
        }
    }
}