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
    public partial class Stbregister : System.Web.UI.Page
    {
        CommonBL ComBL = new CommonBL();
        String userbranch = null;
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();

        protected void Page_Load(object sender, EventArgs e)
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/GridReports/Stbregister.aspx");

            string MultiBranchNumberingScheme = ComBL.GetSystemSettingsResult("NumberingSchemeWithMultiBranch");
            if (!IsPostBack)
            {
                Session["exportval"] = null;
                Session["IsSTBMatIORegDetFilter"] = null;
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

        #region EntityCode Populate

        protected void EntityCode_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            if (e.Parameter.Split('~')[0] == "BindEntityCodeGrid")
            {
                DataTable EntityCodeTable = new DataTable();
                string Hoid = e.Parameter.Split('~')[1];

                EntityCodeTable = oDBEngine.GetDataTable("select cnt_internalId as ID,cnt_UCC as EntityCode from TBL_MASTER_CONTACT WHERE cnt_contactType='EN' order by cnt_UCC");

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

        #region LinQ
        protected void GenerateEntityServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "SEQ";
            SqlConnection connectionString = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            string Userid = Convert.ToString(HttpContext.Current.Session["userid"]);
            ReportSourceDataContext dc = new ReportSourceDataContext(connectionString);

            if (Convert.ToString(Session["IsSTBMatIORegDetFilter"]) == "Y")
            {
                var q = from d in dc.STB_STBRegisterReports
                        where Convert.ToString(d.USERID) == Userid 
                        orderby d.SEQ
                        select d;
                e.QueryableSource = q;
            }
            else
            {
                var q = from d in dc.STB_STBRegisterReports
                        where Convert.ToString(d.SEQ) == "0"
                        orderby d.SEQ
                        select d;
                e.QueryableSource = q;
            }
            ShowGrid.ExpandAll();
        }
        #endregion

        protected void CallbackPanel_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            string IsSTBMatIORegDetFilter = Convert.ToString(hfIsSTBMatIORegDetFilter.Value);
            Session["IsSTBMatIORegDetFilter"] = IsSTBMatIORegDetFilter;

            String dtFrom = Convert.ToString(hdnFromDate.Value);
            String dtTo = Convert.ToString(hdnToDate.Value);

            String Type = Convert.ToString(hdnType.Value);
            String ReportType = Convert.ToString(hdnReportType.Value);
            String IsDetails = Convert.ToString(hdnIsDetails.Value);



            string FROMDATE = dtFrom.Split('-')[2] + '-' + dtFrom.Split('-')[1] + '-' + dtFrom.Split('-')[0];
            string TODATE = dtTo.Split('-')[2] + '-' + dtTo.Split('-')[1] + '-' + dtTo.Split('-')[0];

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

            Task PopulateStockTrialDataTask = new Task(() => GetSTBReceivingRegisterDet(FROMDATE, TODATE, Location_ID, EntityCode_ID, Type, ReportType, IsDetails));
            PopulateStockTrialDataTask.RunSynchronously();
        }

        public void GetSTBReceivingRegisterDet(string FROMDATE, string TODATE, string Location_ID, String EntityCode_ID, String Type, String ReportType, String IsDetails)
        {
            try
            {
                DataTable ds = new DataTable();
                ProcedureExecute proc = new ProcedureExecute("PRC_STBRegister_Report");
                proc.AddPara("@COMPANYID", Convert.ToString(Session["LastCompany"]));
                proc.AddPara("@FINYEAR", Convert.ToString(Session["LastFinYear"]));
                proc.AddPara("@FROM_DATE", FROMDATE);
                proc.AddPara("@TO_DATE", TODATE);
                proc.AddPara("@Type", Type);
                proc.AddPara("@EntryType", ReportType);
                proc.AddPara("@ISDETAILS", IsDetails);
                proc.AddPara("@EntityCode", EntityCode_ID);
                proc.AddPara("@Location", Location_ID);
                proc.AddPara("@ConsInvUpdtDate", (chkConsInvDate.Checked) ? "1" : "0");
                proc.AddPara("@USERID", Convert.ToInt32(Session["userid"]));

                ds = proc.GetTable();

            }
            catch (Exception ex)
            {
            }
        }

        protected void ShowGrid_SummaryDisplayText(object sender, ASPxGridViewSummaryDisplayTextEventArgs e)
        {
            e.Text = string.Format("{0}", e.Value);
        }


        protected void drdExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            String GridTyp = hdnGridTytpe.Value;
            Int32 Filter = int.Parse(Convert.ToString(drdExport.SelectedItem.Value));
            if (Filter != 0)
            {
                if (Session["exportval"] == null)
                {
                    Session["exportval"] = Filter;
                    if (GridTyp == "Details")
                    {
                        bindexportDetails(Filter);
                    }
                    else if (GridTyp == "Summary")
                    {
                        bindexportSummary(Filter);
                    }
                    
                }

                else if (Convert.ToInt32(Session["exportval"]) != Filter)
                {
                    Session["exportval"] = Filter;
                    if (GridTyp == "Details")
                    {
                        bindexportDetails(Filter);
                    }
                    else if (GridTyp == "Summary")
                    {
                        bindexportSummary(Filter);
                    }
                    
                }
            }
        }

        public void bindexportSummary(int Filter)
        {

            string filename = "STB Register Report";
            exporter.FileName = filename;
            string FileHeader = "";

            BusinessLogicLayer.Reports RptHeader = new BusinessLogicLayer.Reports();
            FileHeader = ReplaceFirst(FileHeader, "\r\n", Convert.ToString(Session["BranchNames"]));
            exporter.PageHeader.Left = FileHeader;
            exporter.PageHeader.Font.Size = 10;
            exporter.PageHeader.Font.Name = "Tahoma";

            exporter.GridViewID = "ShowGridHeader";

            exporter.RenderBrick += exporter_RenderBrick;
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

                default:
                    return;
            }
        }

        public void bindexportDetails(int Filter)
        {
            string filename = "STB Register Report";
            exporter1.FileName = filename;
            string FileHeader = "";

            BusinessLogicLayer.Reports RptHeader = new BusinessLogicLayer.Reports();
            FileHeader = ReplaceFirst(FileHeader, "\r\n", Convert.ToString(Session["BranchNames"]));
            exporter1.PageHeader.Left = FileHeader;
            exporter1.PageHeader.Font.Size = 10;
            exporter1.PageHeader.Font.Name = "Tahoma";
            exporter1.GridViewID = "ShowGrid";

            exporter1.RenderBrick += exporter_RenderBrick;
            switch (Filter)
            {
                case 1:
                    exporter1.WritePdfToResponse();
                    break;
                case 2:
                    exporter1.WriteXlsxToResponse(new XlsxExportOptionsEx() { ExportType = ExportType.WYSIWYG });
                    break;
                case 3:
                    exporter1.WriteRtfToResponse();
                    break;
                case 4:
                    exporter1.WriteCsvToResponse();
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

        public string ReplaceFirst(string text, string search, string replace)
        {
            int pos = text.IndexOf(search);
            if (pos < 0)
            {
                return text;
            }
            return text.Substring(0, pos) + Environment.NewLine + replace + Environment.NewLine + text.Substring(pos + search.Length);
        }
    }
}