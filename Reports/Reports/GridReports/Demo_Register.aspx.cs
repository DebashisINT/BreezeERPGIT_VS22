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
using DevExpress.Data;
using System.Threading.Tasks;
using DevExpress.XtraPrinting;
using DevExpress.Export;
using System.Drawing;

namespace Reports.Reports.GridReports
{
    public partial class Demo_Register : System.Web.UI.Page
    {
        DataTable DTIndustry = new DataTable();
        BusinessLogicLayer.Reports objReport = new BusinessLogicLayer.Reports();
        string data = "";
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);



        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        protected void Page_Load(object sender, EventArgs e)
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/Reports/master/Customer-Ledger.aspx");
            DateTime dtFrom;
            DateTime dtTo;
            if (!IsPostBack)
            {
                DataTable dtBranchSelection = new DataTable();
                string GridHeader = "";
                BusinessLogicLayer.Reports GridHeaderDet = new BusinessLogicLayer.Reports();
                RptHeading.Text = "Demo Register";
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

                Session["SI_ComponentData"] = null;
                Session["SI_ComponentData_Branch"] = null;
                Session["dt_PartyLedgerRpt"] = null;

                dtFrom = DateTime.Now;
                dtTo = DateTime.Now;
                ASPxFromDate.Text = dtFrom.ToString("dd-MM-yyyy");
                ASPxToDate.Text = dtTo.ToString("dd-MM-yyyy");
                Date_finyearwise(Convert.ToString(Session["LastFinYear"]));
                //dtFrom = Convert.ToDateTime(ASPxFromDate.Date);
                //dtTo = Convert.ToDateTime(ASPxToDate.Date);
                dtFrom = Convert.ToDateTime(DateTime.Now);
                dtTo = Convert.ToDateTime(DateTime.Now);

                //Rev Subhra 18-12-2018   0017670
                Session["BranchNames"] = null;
                //End Rev

                dtBranchSelection = oDBEngine.GetDataTable("select Variable_Value from Config_SystemSettings where Variable_Name='ReportsBranchSelection'");
                hdnBranchSelection.Value = dtBranchSelection.Rows[0][0].ToString();


                string FROMDATE = dtFrom.ToString("yyyy-MM-dd");
                string TODATE = dtTo.ToString("yyyy-MM-dd");

                string BRANCH_ID = "";
                if (hdnSelectedBranches.Value != "")
                {
                    BRANCH_ID = hdnSelectedBranches.Value;
                }

                string LBBranch_ID = "";
                if (HFieldBranches.Value != "")
                {
                    LBBranch_ID = HFieldBranches.Value;
                }

                string CUSTVENDID = "";
                if (hdnSelectedCustomerVendor.Value != "")
                {
                    CUSTVENDID = hdnSelectedCustomerVendor.Value;
                }

            }
            else
            {
                //dtFrom = Convert.ToDateTime(ASPxFromDate.Date);
                //dtTo = Convert.ToDateTime(ASPxToDate.Date);
                dtFrom = Convert.ToDateTime(DateTime.Now);
                dtTo = Convert.ToDateTime(DateTime.Now);
            }

            //  BindLedgerPosting();

        }
        protected void Page_PreInit(object sender, EventArgs e) // lead add
        {
            if (!IsPostBack)
            {
                string sPath = Convert.ToString(HttpContext.Current.Request.Url);
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }
        public void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(Convert.ToString(drdExport.SelectedItem.Value));
            if (Filter != 0)
            {
                if (Session["exportval"] == null)
                {
                    //Session["exportval"] = Filter;
                    // BindDropDownList();
                    bindexport(Filter);
                }
                else if (Convert.ToInt32(Session["exportval"]) != Filter)
                {
                    // Session["exportval"] = Filter;
                    // BindDropDownList();
                    bindexport(Filter);
                }

                drdExport.SelectedValue = "0";
            }

        }
        public void BindDropDownList()
        {
            // Declare a Dictionary to hold all the Options with Value and Text.
            Dictionary<string, string> options = new Dictionary<string, string>();
            options.Add("0", "Export to");
            options.Add("1", "PDF");
            options.Add("2", "XLS");
            options.Add("3", "RTF");
            options.Add("4", "CSV");


            // Bind the Dictionary to the DropDownList.
            drdExport.DataSource = options;
            drdExport.DataTextField = "value";
            drdExport.DataValueField = "key";
            drdExport.DataBind();
            drdExport.SelectedValue = "0";
        }
        public void bindexport(int Filter)
        {
            string filename = "DemoRegister";
            exporter.FileName = filename;
            string FileHeader = "";

            exporter.FileName = filename;

            BusinessLogicLayer.Reports RptHeader = new BusinessLogicLayer.Reports();

            FileHeader = RptHeader.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), true, true, true,true, true, true) + Environment.NewLine + "Demo Register Report" + Environment.NewLine + "For the period " + Convert.ToDateTime(ASPxFromDate.Date).ToString("dd-MM-yyyy") + " To " + Convert.ToDateTime(ASPxToDate.Date).ToString("dd-MM-yyyy");
            //Rev Subhra 18-12-2018   0017670
            FileHeader = ReplaceFirst(FileHeader, "\r\n", Convert.ToString(Session["BranchNames"]));
            //End of Rev
            exporter.PageHeader.Left = FileHeader;
            exporter.PageHeader.Font.Size = 10;
            exporter.PageHeader.Font.Name = "Tahoma";


            //exporter.PageFooter.Center = "[Page # of Pages #]";
            //exporter.PageFooter.Right = "[Date Printed]";
            exporter.Landscape = true;
            exporter.MaxColumnWidth = 100;
            exporter.GridViewID = "ShowGrid";

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
            }

        }
        //Rev Subhra 18-12-2018   0017670
        public string ReplaceFirst(string text, string search, string replace)
        {
            int pos = text.IndexOf(search);
            if (pos < 0)
            {
                return text;
            }
            return text.Substring(0, pos) + Environment.NewLine + replace + Environment.NewLine + text.Substring(pos + search.Length);
        }
        //End of Rev
        protected void exporter_RenderBrick(object sender, ASPxGridViewExportRenderingEventArgs e)
        {
            e.BrickStyle.BackColor = Color.White;
            e.BrickStyle.ForeColor = Color.Black;
        }
        protected void BindLedgerPosting()
        {
            try
            {
                if (Session["dt_PartyLedgerRpt"] != null)
                {
                    ShowGrid.DataSource = (DataTable)Session["dt_PartyLedgerRpt"];
                    ShowGrid.DataBind();
                }
            }
            catch { }
        }
        protected void Grid_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {

            Session.Remove("dt_PartyLedgerRpt");


            ShowGrid.JSProperties["cpSave"] = null;


            string COMPANYID = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
            string Finyear = Convert.ToString(HttpContext.Current.Session["LastFinYear"]);
            int ischeckchallandate = 0;
            DateTime dtFrom;
            DateTime dtTo;
            dtFrom = Convert.ToDateTime(ASPxFromDate.Date);
            dtTo = Convert.ToDateTime(ASPxToDate.Date);

            string FROMDATE = dtFrom.ToString("yyyy-MM-dd");
            string TODATE = dtTo.ToString("yyyy-MM-dd");

            //string TABLENAME = "Ledger Details";

            string Class = "";
            string BranchCode = "";

            string QuoComponent2 = "";
            List<object> QuoList2 = lookup_class.GridView.GetSelectedFieldValues("ID");
            foreach (object Quo2 in QuoList2)
            {
                QuoComponent2 += "," + Quo2;
            }
            Class = QuoComponent2.TrimStart(',');


            //string CASHBANKTYPE = "";
            //string CASHBANKID = "";
            //string UserId = "";
            string Category = "";
            string QuoComponent = "";
            List<object> QuoList = lookup_category.GridView.GetSelectedFieldValues("ID");
            foreach (object Quo in QuoList)
            {
                QuoComponent += "," + Quo;
            }
            Category = QuoComponent.TrimStart(',');


            string QuoComponent3 = "";
            List<object> QuoList3 = lookup_LBBranch.GridView.GetSelectedFieldValues("ID");
            foreach (object Quo3 in QuoList3)
            {
                QuoComponent3 += "," + Quo3;
            }
            BranchCode = QuoComponent3.TrimStart(',');

            if (chkischallandate.Checked == true)
            {
                ischeckchallandate = 1;
            }
            else
            {
                ischeckchallandate = 0;
            }
            //Rev Subhra 18-12-2018   0017670

            string BRANCH_NAME = "";
            string BranchNameComponent = "";
            List<object> BranchNameList = lookup_LBBranch.GridView.GetSelectedFieldValues("branch_description");
            foreach (object BranchName in BranchNameList)
            {
                BranchNameComponent += "," + BranchName;
            }
            if (BranchNameList.Count > 1)
            {
                BRANCH_NAME = "Multiple Branch Selected";
                Session["BranchNames"] = BRANCH_NAME;
            }
            else
            {
                BRANCH_NAME = BranchNameComponent.TrimStart(',');
                Session["BranchNames"] = "For Unit : " + BRANCH_NAME + " ";
            }
            ShowGrid.JSProperties["cpBranchNames"] = Convert.ToString(Session["BranchNames"]);

            //End of Rev
            Task PopulateStockTrialDataTask = new Task(() => GetSalesRegisterdata(FROMDATE, TODATE, Class, Category, BranchCode, ischeckchallandate));
            PopulateStockTrialDataTask.RunSynchronously();

        }
        public void GetSalesRegisterdata(string FROMDATE, string TODATE, string classcode, string category, string BranchCode, int ischeckchallandate)
        {
            try
            {

                string DriverName = string.Empty;
                string PhoneNo = string.Empty;
                string VehicleNo = string.Empty;

                DataSet ds = new DataSet();
                //SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("prc_SalesInvoiceDeliveryChallan_Report", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@COMPANYID", Convert.ToString(Session["LastCompany"]));
                cmd.Parameters.AddWithValue("@FINYEAR", Convert.ToString(Session["LastFinYear"]).Trim());
                cmd.Parameters.AddWithValue("@FROMDATE", FROMDATE);
                cmd.Parameters.AddWithValue("@TODATE", TODATE);
                cmd.Parameters.AddWithValue("@CLASS", classcode);
                cmd.Parameters.AddWithValue("@CATEGORY", category);
                cmd.Parameters.AddWithValue("@BRANCHID", BranchCode);
                cmd.Parameters.AddWithValue("@IS_CHALLANDATE", ischeckchallandate);


                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(ds);

                cmd.Dispose();
                con.Dispose();

                Session["dt_PartyLedgerRpt"] = ds.Tables[0];

                ShowGrid.DataSource = ds.Tables[0];
                ShowGrid.DataBind();
            }
            catch (Exception ex)
            {
            }
        }
        [WebMethod]
        public static List<string> GetBranchesList(String NoteId)
        {
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
            StringBuilder filter = new StringBuilder();
            StringBuilder Supervisorfilter = new StringBuilder();
            BusinessLogicLayer.Others objbl = new BusinessLogicLayer.Others();
            DataTable dtbl = new DataTable();
            if (NoteId.Trim() == "")
            {
                dtbl = oDBEngine.GetDataTable("select branch_id,branch_description from tbl_master_branch where branch_id in(" + Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]) + ")  order by branch_description asc");

            }

            List<string> obj = new List<string>();

            foreach (DataRow dr in dtbl.Rows)
            {

                obj.Add(Convert.ToString(dr["branch_description"]) + "|" + Convert.ToString(dr["branch_id"]));
            }
            return obj;
        }
        [WebMethod]
        public static List<string> BindLedgerType(String Ids)
        {
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);

            DataTable dtbl = new DataTable();
            if (Ids.Trim() != "")
            {
                dtbl = oDBEngine.GetDataTable("select A.MainAccount_ReferenceID AS ID,A.MainAccount_Name as 'AccountName' FROM Master_MainAccount A WHERE A.MainAccount_AccountCode IN(SELECT RTRIM(B.AccountsLedger_MainAccountID) FROM Trans_AccountsLedger B WHERE B.AccountsLedger_BranchId in(" + Ids + ")) ORDER BY A.MainAccount_Name ");

            }

            List<string> obj = new List<string>();

            foreach (DataRow dr in dtbl.Rows)
            {

                obj.Add(Convert.ToString(dr["AccountName"]) + "|" + Convert.ToString(dr["Id"]));
            }
            return obj;
        }
        [WebMethod]
        public static List<string> BindCustomerVendor(string type)
        {
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);

            DataTable dtbl = new DataTable();

            //dtbl = oDBEngine.GetDataTable("select cnt_id AS ID,ISNULL(cnt_firstName,'')+' '+ISNULL(cnt_middleName,'')+'  '+ISNULL(cnt_lastName,'') as 'Name'  FROM tbl_master_contact WHERE cnt_contactType in('CL','DV') AND cnt_branchid IN("+ Ids +") ORDER BY ISNULL(cnt_firstName,'')+' '+ISNULL(cnt_middleName,'')+' '+ISNULL(cnt_lastName,'') ");
            if (type == "0")
            {
                dtbl = oDBEngine.GetDataTable("select cnt_internalId AS ID,ISNULL(cnt_firstName,'')+' '+ISNULL(cnt_middleName,'')+'  '+ISNULL(cnt_lastName,'') as 'Name'  FROM tbl_master_contact WHERE cnt_contactType in('CL','DV') ORDER BY ISNULL(cnt_firstName,'')+' '+ISNULL(cnt_middleName,'')+' '+ISNULL(cnt_lastName,'') ");

            }

            else if (type == "1")
            {
                dtbl = oDBEngine.GetDataTable("select cnt_internalId AS ID,ISNULL(cnt_firstName,'')+' '+ISNULL(cnt_middleName,'')+'  '+ISNULL(cnt_lastName,'') as 'Name'  FROM tbl_master_contact WHERE cnt_contactType in('CL') ORDER BY ISNULL(cnt_firstName,'')+' '+ISNULL(cnt_middleName,'')+' '+ISNULL(cnt_lastName,'') ");

            }

            else if (type == "2")
            {
                dtbl = oDBEngine.GetDataTable("select cnt_internalId AS ID,ISNULL(cnt_firstName,'')+' '+ISNULL(cnt_middleName,'')+'  '+ISNULL(cnt_lastName,'') as 'Name'  FROM tbl_master_contact WHERE cnt_contactType in('DV') ORDER BY ISNULL(cnt_firstName,'')+' '+ISNULL(cnt_middleName,'')+' '+ISNULL(cnt_lastName,'') ");

            }

            List<string> obj = new List<string>();

            foreach (DataRow dr in dtbl.Rows)
            {

                obj.Add(Convert.ToString(dr["Name"]) + "|" + Convert.ToString(dr["Id"]));
            }
            return obj;
        }
        public void Date_finyearwise(string Finyear)
        {
            CommonBL bll1 = new CommonBL();
            DataTable stbill = new DataTable();
            stbill = bll1.GetDateFinancila(Finyear);
            if (stbill.Rows.Count > 0)
            {
                //ASPxFromDate.Text = Convert.ToDateTime(stbill.Rows[0]["FinYear_StartDate"]).ToString("dd-MM-yyyy");
                //ASPxToDate.Text = Convert.ToDateTime(stbill.Rows[0]["FinYear_EndDate"]).ToString("dd-MM-yyyy");
                ASPxFromDate.Text = Convert.ToDateTime(DateTime.Now).ToString("dd-MM-yyyy");
                ASPxToDate.Text = Convert.ToDateTime(DateTime.Now).ToString("dd-MM-yyyy");
            }

        }
        protected void ShowGrid_SummaryDisplayText(object sender, ASPxGridViewSummaryDisplayTextEventArgs e)
        {
            e.Text = string.Format("{0}", e.Value);
        }
        protected void Showgrid_Htmlprepared(object sender, EventArgs e)
        {
            if (Session["dt_PartyLedgerRpt"] != null)
            {

                ShowGrid.DataSource = (DataTable)Session["dt_PartyLedgerRpt"];
            }
        }
        protected void ShowGrid_DataBinding(object sender, EventArgs e)
        {
            if (Session["dt_PartyLedgerRpt"] != null)
            {
                ShowGrid.DataSource = (DataTable)Session["dt_PartyLedgerRpt"];
            }
        }

        #region Category Bind
        protected void Componentcategory_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {

            string FinYear = Convert.ToString(Session["LastFinYear"]);

            if (e.Parameter.Split('~')[0] == "BindComponentGrid")
            {

                string type = e.Parameter.Split('~')[1];

                DataTable ComponentTable = new DataTable();


                ComponentTable = oDBEngine.GetDataTable("select  Brand_Id as ID,Brand_Name as Name  from tbl_master_brand");



                if (ComponentTable.Rows.Count > 0)
                {


                    lookup_category.DataSource = ComponentTable;
                    lookup_category.DataBind();

                }
                else
                {

                    lookup_category.DataSource = null;
                    lookup_category.DataBind();

                }
            }
        }
        protected void lookup_category_DataBinding(object sender, EventArgs e)
        {
            DataTable ComponentTable = oDBEngine.GetDataTable("select  Brand_Id as ID,Brand_Name as Name  from tbl_master_brand");

            lookup_category.DataSource = ComponentTable;

        }

        #endregion

        #region Product Class  Bind

        protected void Componentchallan_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            string FinYear = Convert.ToString(Session["LastFinYear"]);

            if (e.Parameter.Split('~')[0] == "BindComponentGrid")
            {
                DataTable ComponentTable = new DataTable();
                string Hoid = e.Parameter.Split('~')[1];
                if (Session["userbranchHierarchy"] != null)
                {
                    ComponentTable = oDBEngine.GetDataTable("select  ProductClass_Code,ProductClass_Name ,ProductClass_ID as ID from Master_ProductClass");
                }
                if (ComponentTable.Rows.Count > 0)
                {

                    lookup_class.DataSource = ComponentTable;
                    lookup_class.DataBind();


                }
                else
                {
                    lookup_class.DataSource = null;
                    lookup_class.DataBind();

                }
            }
        }
        protected void lookup_class_DataBinding(object sender, EventArgs e)
        {

            DataTable ComponentTable = oDBEngine.GetDataTable("select  ProductClass_Code,ProductClass_Name ,ProductClass_ID as ID from Master_ProductClass");

            lookup_class.DataSource = ComponentTable;

        }

        #endregion

        #region Branch  Bind

        protected void Branch_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            string FinYear = Convert.ToString(Session["LastFinYear"]);

            if (e.Parameter.Split('~')[0] == "BindComponentGrid")
            {
                DataTable ComponentTable = new DataTable();
                string Hoid = e.Parameter.Split('~')[1];
                if (Session["userbranchHierarchy"] != null)
                {
                    ComponentTable = oDBEngine.GetDataTable("select branch_id as ID,branch_description,branch_code from tbl_master_branch where branch_id in(" + Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]) + ") order by branch_description asc");
                }
                if (ComponentTable.Rows.Count > 0)
                {

                    Session["SI_ComponentData_Branch"] = ComponentTable;
                    lookup_LBBranch.DataSource = ComponentTable;
                    lookup_LBBranch.DataBind();


                }
                else
                {
                    lookup_LBBranch.DataSource = null;
                    lookup_LBBranch.DataBind();

                }
            }
        }
        protected void lookup_LBBranch_DataBinding(object sender, EventArgs e)
        {
            if (Session["SI_ComponentData_Branch"] != null)
            {
                //    DataTable ComponentTable = oDBEngine.GetDataTable("select branch_id,branch_description,branch_code from tbl_master_branch where branch_parentId='" + Hoid + "' order by branch_description asc");
                lookup_LBBranch.DataSource = (DataTable)Session["SI_ComponentData_Branch"];
            }

        }

        #endregion

    }
}