using DataAccessLayer;
using EntityLayer.CommonELS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using BusinessLogicLayer;
using System.Data.SqlClient;
using DataAccessLayer;
using System.Threading.Tasks;
using System.Drawing;
using DevExpress.XtraPrinting;
using DevExpress.Export;
using DevExpress.XtraPrintingLinks;
using System.IO;

namespace Reports.Reports.GridReports
{
    public partial class ProjectSummary : System.Web.UI.Page
    {
        BusinessLogicLayer.Reports objReport = new BusinessLogicLayer.Reports();
        CommonBL cbl = new CommonBL();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();

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
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/GridReports/ProjectSummary.aspx");
            DateTime dtFrom;
            DateTime dtTo;
            if (!IsPostBack)
            {
                DataTable dtBranchSelection = new DataTable();
                string GridHeader = "";
                BusinessLogicLayer.Reports GridHeaderDet = new BusinessLogicLayer.Reports();
                RptHeading.Text = "Project Summary";
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

                Session["SI_ComponentData_Branch"] = null;
                BranchHoOffice();
                Session["exportval"] = null;

                dtFrom = DateTime.Now;
                dtTo = DateTime.Now;
                ASPxAsOnDate.Value = DateTime.Now;
                Date_finyearwise(Convert.ToString(Session["LastFinYear"]));
                Session["BranchNames"] = null;
                dtBranchSelection = oDBEngine.GetDataTable("select Variable_Value from Config_SystemSettings where Variable_Name='ReportsBranchSelection'");
                hdnBranchSelection.Value = dtBranchSelection.Rows[0][0].ToString();
            }
            else
            {
                dtFrom = Convert.ToDateTime(ASPxAsOnDate.Date);
            }

            if (!IsPostBack)
            {
                string COMPANYID = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
                string Finyear = Convert.ToString(HttpContext.Current.Session["LastFinYear"]);
                dtFrom = Convert.ToDateTime(ASPxAsOnDate.Date);

                string ASONDATE = dtFrom.ToString("yyyy-MM-dd");
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
            DateTime MinDate, MaxDate;

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

        protected void Component_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            DateTime dtAson;
            dtAson = Convert.ToDateTime(ASPxAsOnDate.Date);
            string ASONDATE = dtAson.ToString("yyyy-MM-dd");

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

            DataSet PROJSUM = new DataSet();
            SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));

            ProcedureExecute proc = new ProcedureExecute("PRC_PROJECTSUMMARY_REPORT");
            proc.AddVarcharPara("@COMPANYID", 20,Convert.ToString(Session["LastCompany"]));
            proc.AddVarcharPara("@FINYEAR", 9,Convert.ToString(Session["LastFinYear"]));
            proc.AddVarcharPara("@ASONDATE", 10, ASONDATE);
            proc.AddVarcharPara("@BRANCHID", -1,BRANCH_ID);
            proc.AddVarcharPara("@PROJECT_ID", -1,PROJECT_ID);

            PROJSUM = proc.GetDataSet();
            Session["CostGrid"] = PROJSUM.Tables[0];
            Session["RevGrid"] = PROJSUM.Tables[1];
            Session["IRSummGrid"] = PROJSUM.Tables[2];
            Session["CustSummGrid"] = PROJSUM.Tables[3];
            Session["VendSummGrid"] = PROJSUM.Tables[4];
            Session["OTHGrid"] = PROJSUM.Tables[5];

            CostGrid.DataBind();
            RevGrid.DataBind();
            IRSummGrid.DataBind();
            CustSummGrid.DataBind();
            VendSummGrid.DataBind();
            OTHGrid.DataBind();
        }

        protected void cCostGrid_DataBinding(object sender, EventArgs e)
        {
            DataTable CostGridDT = (DataTable)Session["CostGrid"];
            if (CostGridDT != null)
            {
                CostGrid.DataSource = CostGridDT;
            }
        }

        protected void cRevGrid_DataBinding(object sender, EventArgs e)
        {
            DataTable RevGridDT = (DataTable)Session["RevGrid"];
            if (RevGridDT != null)
            {
                RevGrid.DataSource = RevGridDT;
            }
        }

        protected void cIRSummGrid_DataBinding(object sender, EventArgs e)
        {
            DataTable IRSummGridDT = (DataTable)Session["IRSummGrid"];
            if (IRSummGridDT != null)
            {
                IRSummGrid.DataSource = IRSummGridDT;
            }
        }

        protected void cCustSummGrid_DataBinding(object sender, EventArgs e)
        {
            DataTable CustSummGridDT = (DataTable)Session["CustSummGrid"];
            if (CustSummGridDT != null)
            {
                CustSummGrid.DataSource = CustSummGridDT;
            }
        }

        protected void cVendSummGrid_DataBinding(object sender, EventArgs e)
        {
            DataTable VendSummGridDT = (DataTable)Session["VendSummGrid"];
            if (VendSummGridDT != null)
            {
                VendSummGrid.DataSource = VendSummGridDT;
            }
        }

        protected void cOTHGrid_DataBinding(object sender, EventArgs e)
        {
            DataTable OTHGridDT = (DataTable)Session["OTHGrid"];
            if (OTHGridDT != null)
            {
                OTHGrid.DataSource = OTHGridDT;
            }
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            string FileHeader = "";
            BusinessLogicLayer.Reports RptHeader = new BusinessLogicLayer.Reports();
            FileHeader = RptHeader.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), true, true, true, true, true, true) + Environment.NewLine + "Project Summary" + Environment.NewLine + "As On " + Convert.ToDateTime(ASPxAsOnDate.Date).ToString("dd-MM-yyyy");

            PrintingSystemBase ps = new PrintingSystemBase();

            PrintableComponentLinkBase link1 = new PrintableComponentLinkBase(ps);
            CostGridExporter.PageHeader.Left = FileHeader;
            link1.Component = CostGridExporter;

            PrintableComponentLinkBase link2 = new PrintableComponentLinkBase(ps);
            RevGridExporter.PageHeader.Left = FileHeader;
            link2.Component = RevGridExporter;

            PrintableComponentLinkBase link3 = new PrintableComponentLinkBase(ps);
            IRSummGridExporter.PageHeader.Left = FileHeader;
            link3.Component = IRSummGridExporter;

            PrintableComponentLinkBase link4 = new PrintableComponentLinkBase(ps);
            CustSummGridExporter.PageHeader.Left = FileHeader;
            link4.Component = CustSummGridExporter;

            PrintableComponentLinkBase link5 = new PrintableComponentLinkBase(ps);
            VendSummGridExporter.PageHeader.Left = FileHeader;
            link5.Component = VendSummGridExporter;

            PrintableComponentLinkBase link6 = new PrintableComponentLinkBase(ps);
            OTHGridExporter.PageHeader.Left = FileHeader;
            link6.Component = OTHGridExporter;

            CompositeLinkBase compositeLink = new CompositeLinkBase(ps);
            compositeLink.Links.AddRange(new object[] { link1, link2, link3, link4, link5, link6 });

            compositeLink.CreatePageForEachLink();

            using (MemoryStream stream = new MemoryStream())
            {
                XlsxExportOptions options = new XlsxExportOptions();
                options.TextExportMode = TextExportMode.Text;
                options.ExportMode = XlsxExportMode.SingleFilePageByPage;
                compositeLink.PrintingSystemBase.XlsxDocumentCreated += PrintingSystemBase_XlsxDocumentCreated;

                compositeLink.PrintingSystemBase.ExportToXlsx(stream, options);

                Response.Clear();
                Response.Buffer = false;
                Response.AppendHeader("Content-Type", "application/xlsx");
                Response.AppendHeader("Content-Transfer-Encoding", "binary");
                Response.AppendHeader("Content-Disposition", "attachment; filename=Project_Summary" + ".xlsx");
                stream.Position = 0;
                Response.BinaryWrite(stream.ToArray());
                Response.End();
            }
            ps.Dispose();
        }

        void PrintingSystemBase_XlsxDocumentCreated(object sender, XlsxDocumentCreatedEventArgs e)
        {
            e.SheetNames[0] = "Cost";
            e.SheetNames[1] = "Revenue";
            e.SheetNames[2] = "Summary - Initial & Revised";
            e.SheetNames[3] = "Summary - Customer";
            e.SheetNames[4] = "Summary - Vendor";
            e.SheetNames[5] = "Others, If Any";
        }
    }
}