using BusinessLogicLayer;
using BusinessLogicLayer.ServiceManagement;
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
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Reports.Reports.GridReports
{
    public partial class SRVServiceRegisterReport : System.Web.UI.Page
    {
        CommonBL ComBL = new CommonBL();
        String userbranch = null;
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
       public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        protected void Page_Load(object sender, EventArgs e)
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/GridReports/SRVServiceRegisterReport.aspx");

            string MultiBranchNumberingScheme = ComBL.GetSystemSettingsResult("NumberingSchemeWithMultiBranch");
            if (!IsPostBack)
            {
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
        }

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

        #region Model Populate

        protected void Model_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            if (e.Parameter.Split('~')[0] == "BindModelGrid")
            {
                DataTable ModelTable = new DataTable();
                string Hoid = e.Parameter.Split('~')[1];

                ModelTable = oDBEngine.GetDataTable("select ModelID AS ID,ModelDesc AS Model from Master_Model ORDER BY ModelDesc");

                if (ModelTable.Rows.Count > 0)
                {
                    Session["ModelData"] = ModelTable;
                    lookup_Model.DataSource = ModelTable;
                    lookup_Model.DataBind();
                }
                else
                {
                    Session["ModelData"] = ModelTable;
                    lookup_Model.DataSource = null;
                    lookup_Model.DataBind();
                }
            }
        }

        protected void lookup_Model_DataBinding(object sender, EventArgs e)
        {
            if (Session["ModelData"] != null)
            {
                lookup_Model.DataSource = (DataTable)Session["ModelData"];
            }
        }

        #endregion

        #region ProblemFound Populate

        protected void ProblemFound_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            if (e.Parameter.Split('~')[0] == "BindProblemFoundGrid")
            {
                DataTable ProblemFoundTable = new DataTable();
                string Hoid = e.Parameter.Split('~')[1];

                ProblemFoundTable = oDBEngine.GetDataTable("select convert(nvarchar(10),ProblemID) AS ProblemID,ProblemDesc from master_problem");

                if (ProblemFoundTable.Rows.Count > 0)
                {
                    Session["ProblemFoundData"] = ProblemFoundTable;
                    lookup_ProblemFound.DataSource = ProblemFoundTable;
                    lookup_ProblemFound.DataBind();
                }
                else
                {
                    Session["ProblemFoundData"] = ProblemFoundTable;
                    lookup_ProblemFound.DataSource = null;
                    lookup_ProblemFound.DataBind();
                }
            }
        }

        protected void lookup_ProblemFound_DataBinding(object sender, EventArgs e)
        {
            if (Session["ProblemFoundData"] != null)
            {
                lookup_ProblemFound.DataSource = (DataTable)Session["ProblemFoundData"];
            }
        }

        #endregion

        #region Technician Populate

        protected void Technician_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            if (e.Parameter.Split('~')[0] == "BindTechnicianGrid")
            {
                DataTable TechnicianTable = new DataTable();
                string Hoid = e.Parameter.Split('~')[1];

                TechnicianTable = oDBEngine.GetDataTable("select DISTINCT CNT.cnt_internalId,CNT.cnt_firstName from tbl_master_contact CNT INNER JOIN Srv_master_TechnicianBranch_map MAP ON MAP.Tech_InternalId=CNT.cnt_internalId WHERE MAP.branch_id IN (" + Convert.ToString(HttpContext.Current.Session["UserBranchMapID"]) + ")  AND CNT.cnt_contactType='TM' AND CNT.Is_Active=1");

                if (TechnicianTable.Rows.Count > 0)
                {
                    Session["TechnicianData"] = TechnicianTable;
                    lookup_Technician.DataSource = TechnicianTable;
                    lookup_Technician.DataBind();
                }
                else
                {
                    Session["TechnicianData"] = TechnicianTable;
                    lookup_Technician.DataSource = null;
                    lookup_Technician.DataBind();
                }
            }
        }

        protected void lookup_Technician_DataBinding(object sender, EventArgs e)
        {
            if (Session["TechnicianData"] != null)
            {
                lookup_Technician.DataSource = (DataTable)Session["TechnicianData"];
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

        #region LinQ
        protected void GenerateEntityServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "SEQ";
            SqlConnection connectionString = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            string Userid = Convert.ToString(HttpContext.Current.Session["userid"]);
            ReportSourceDataContext dc = new ReportSourceDataContext(connectionString);

            if (Convert.ToString(Session["IsSrvRegisterFilter"]) == "Y")
            {
                var q = from d in dc.SRV_SERVICEREGISTERREPORTs
                        where Convert.ToString(d.USERID) == Userid
                        orderby d.SEQ
                        select d;
                e.QueryableSource = q;
            }
            else
            {
                var q = from d in dc.SRV_SERVICEREGISTERREPORTs
                        where Convert.ToString(d.SEQ) == "0"
                        orderby d.SEQ
                        select d;
                e.QueryableSource = q;
            }
        }

        #endregion

        protected void ShowGrid_SummaryDisplayText(object sender, ASPxGridViewSummaryDisplayTextEventArgs e)
        {
            e.Text = string.Format("{0}", e.Value);
        }

        [WebMethod]
        public static String ReceiptChalan(ServiceRegisterReportInput Data)
        {
            string Types = Data.Type;
            int k = 1;

            string Report = "";
            k = 1;
            if (Data.Report != null && Data.Report.Count > 0)
            {
                foreach (string item in Data.Report)
                {
                    if (k > 1)
                        Report = Report + "," + item;
                    else
                        Report = item;
                    k++;
                }
            }

            string EntryType = "";
            k = 1;
            if (Data.EntryType != null && Data.EntryType.Count > 0)
            {
                foreach (string item in Data.EntryType)
                {
                    if (k > 1)
                        EntryType = EntryType + "," + item;
                    else
                        EntryType = item;
                    k++;
                }
            }


            if (Data.FromDate != "")
            {

            }



            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_SRVServiceRegisterReport");
            proc.AddVarcharPara("@ACTION", 500, "ReceiptChalan");
            proc.AddPara("@Type", Types);
            proc.AddPara("@Report", Report);
            proc.AddPara("@EntityCode", Data.EntityCode);
            proc.AddPara("@EntryType", EntryType);
            proc.AddPara("@Model", Data.Model);
            proc.AddPara("@ProblemFound", Data.ProblemFound);
            proc.AddPara("@Technician", Data.Technician);
            if (Data.Location == "")
            {
                proc.AddPara("@Location", Convert.ToString(HttpContext.Current.Session["UserBranchMapID"]));
            }
            else
            {
                proc.AddPara("@Location", Data.Location);
            }
            proc.AddPara("@FromDate", Data.FromDate);
            proc.AddPara("@ToDate", Data.ToDate);
            proc.AddPara("@IsBillable", Data.IsBillable);

            proc.AddPara("@ProblemReported", Data.ProblemReported);
            proc.AddPara("@IsProbLemReport", Data.IsProbLemReport);
            proc.AddPara("@IsDelivery", Data.IsDelivery);
            proc.AddPara("@USERID", Convert.ToString(HttpContext.Current.Session["userid"]));

            dt = proc.GetTable();
            HttpContext.Current.Session["IsSrvRegisterFilter"] = "Y";
            if (dt != null && dt.Rows.Count > 0)
            {
                // listStatues = APIHelperMethods.ToModelList<ReceiptChallanReports>(dt);
                HttpContext.Current.Session["IsSrvRegisterFilter"] = "Y";
            }
            return "Sucess";
        }

        [WebMethod]
        public static String DeliveryReport(ServiceRegisterReportInput Data)
        {

            string Types = Data.Type;
            int k = 1;
            string Report = "";
            k = 1;
            if (Data.Report != null && Data.Report.Count > 0)
            {
                foreach (string item in Data.Report)
                {
                    if (k > 1)
                        Report = Report + "," + item;
                    else
                        Report = item;
                    k++;
                }
            }

            string EntryType = "";
            k = 1;
            if (Data.EntryType != null && Data.EntryType.Count > 0)
            {
                foreach (string item in Data.EntryType)
                {
                    if (k > 1)
                        EntryType = EntryType + "," + item;
                    else
                        EntryType = item;
                    k++;
                }
            }

            if (Data.FromDate != "")
            {

            }

            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_SRVServiceRegisterReport");
            proc.AddVarcharPara("@ACTION", 500, "DeliveryReport");
            proc.AddPara("@Type", Types);
            proc.AddPara("@Report", Report);
            proc.AddPara("@EntityCode", Data.EntityCode);
            proc.AddPara("@EntryType", EntryType);
            proc.AddPara("@Model", Data.Model);
            proc.AddPara("@ProblemFound", Data.ProblemFound);
            proc.AddPara("@Technician", Data.Technician);
            //proc.AddPara("@Location", Data.Location);
            if (Data.Location == "")
            {
                proc.AddPara("@Location", Convert.ToString(HttpContext.Current.Session["UserBranchMapID"]));
            }
            else
            {
                proc.AddPara("@Location", Data.Location);
            }
            proc.AddPara("@FromDate", Data.FromDate);
            proc.AddPara("@ToDate", Data.ToDate);
            proc.AddPara("@IsBillable", Data.IsBillable);

            proc.AddPara("@ProblemReported", Data.ProblemReported);
            proc.AddPara("@IsProbLemReport", Data.IsProbLemReport);
            proc.AddPara("@IsDelivery", Data.IsDelivery);
            proc.AddPara("@USERID", Convert.ToString(HttpContext.Current.Session["userid"]));

            dt = proc.GetTable();
            HttpContext.Current.Session["IsSrvRegisterFilter"] = "Y";
            if (dt != null && dt.Rows.Count > 0)
            {
                //   listStatues = APIHelperMethods.ToModelList<DeliveryReports>(dt);
                HttpContext.Current.Session["IsSrvRegisterFilter"] = "Y";
            }
            return "Sucess";
        }

        [WebMethod]
        public static String ReceiptChalanDetails(ServiceRegisterReportInput Data)
        {
            string Types = Data.Type;
            int k = 1;

            string Report = "";
            k = 1;
            if (Data.Report != null && Data.Report.Count > 0)
            {
                foreach (string item in Data.Report)
                {
                    if (k > 1)
                        Report = Report + "," + item;
                    else
                        Report = item;
                    k++;
                }
            }

            string EntryType = "";
            k = 1;
            if (Data.EntryType != null && Data.EntryType.Count > 0)
            {
                foreach (string item in Data.EntryType)
                {
                    if (k > 1)
                        EntryType = EntryType + "," + item;
                    else
                        EntryType = item;
                    k++;
                }
            }

            if (Data.FromDate != "")
            {

            }


            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_SRVServiceRegisterReport");
            proc.AddVarcharPara("@ACTION", 500, "ReceiptChalanDetails");
            proc.AddPara("@Type", Types);
            proc.AddPara("@Report", Report);
            proc.AddPara("@EntityCode", Data.EntityCode);
            proc.AddPara("@EntryType", EntryType);
            proc.AddPara("@Model", Data.Model);
            proc.AddPara("@ProblemFound", Data.ProblemFound);
            proc.AddPara("@Technician", Data.Technician);
            if (Data.Location == "")
            {
                proc.AddPara("@Location", Convert.ToString(HttpContext.Current.Session["UserBranchMapID"]));
            }
            else
            {
                proc.AddPara("@Location", Data.Location);
            }
            proc.AddPara("@FromDate", Data.FromDate);
            proc.AddPara("@ToDate", Data.ToDate);
            proc.AddPara("@IsBillable", Data.IsBillable);

            proc.AddPara("@ProblemReported", Data.ProblemReported);
            proc.AddPara("@IsProbLemReport", Data.IsProbLemReport);
            proc.AddPara("@IsDelivery", Data.IsDelivery);
            proc.AddPara("@USERID", Convert.ToString(HttpContext.Current.Session["userid"]));

            dt = proc.GetTable();
            HttpContext.Current.Session["IsSrvRegisterFilter"] = "Y";
            if (dt != null && dt.Rows.Count > 0)
            {
                //   listStatues = APIHelperMethods.ToModelList<ReceiptChallanReports>(dt);
                HttpContext.Current.Session["IsSrvRegisterFilter"] = "Y";
            }
            return "Sucess";
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
                    else if (GridTyp == "ServiceEntry")
                    {
                        bindexportServiceEntry(Filter);
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
                    else if (GridTyp == "ServiceEntry")
                    {
                        bindexportServiceEntry(Filter);
                    }
                }
            }
        }

        public void bindexportSummary(int Filter)
        {

            string filename = "Service Register Report";
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
            string filename = "Service Register Report";
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

        public void bindexportServiceEntry(int Filter)
        {
            string filename = "Service Register Report";
            exporter2.FileName = filename;
            string FileHeader = "";

            BusinessLogicLayer.Reports RptHeader = new BusinessLogicLayer.Reports();
            FileHeader = ReplaceFirst(FileHeader, "\r\n", Convert.ToString(Session["BranchNames"]));
            exporter2.PageHeader.Left = FileHeader;
            exporter2.PageHeader.Font.Size = 10;
            exporter2.PageHeader.Font.Name = "Tahoma";

            exporter2.GridViewID = "ShowGridEntry";

            exporter2.RenderBrick += exporter_RenderBrick;
            switch (Filter)
            {
                case 1:
                    exporter2.WritePdfToResponse();
                    break;
                case 2:
                    exporter2.WriteXlsxToResponse(new XlsxExportOptionsEx() { ExportType = ExportType.WYSIWYG });
                    break;
                case 3:
                    exporter2.WriteRtfToResponse();
                    break;
                case 4:
                    exporter2.WriteCsvToResponse();
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