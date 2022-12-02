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
using Reports.Model;

namespace Reports.Reports.GridReports
{
    public partial class PartyLedgerPostingReport : System.Web.UI.Page
    {
        DataTable DTIndustry = new DataTable();
        BusinessLogicLayer.Reports objReport = new BusinessLogicLayer.Reports();
        string data = "";
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        decimal TotalDebit = 0, TotalCredit = 0, TotalDBCR = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/Reports/master/LedgerPostingReport.aspx");
            DateTime dtFrom;
            DateTime dtTo;
            if (!IsPostBack)
            {
                DataTable dtBranchSelection = new DataTable();
                string GridHeader = "";
                BusinessLogicLayer.Reports GridHeaderDet = new BusinessLogicLayer.Reports();
                RptHeading.Text = "Consolidated Party Ledger";
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

                //Session["SI_ComponentData"] = null;
                Session["SI_ComponentData_Branch"] = null;
                //Session["dt_PartyLedgerRpt"] = null;

                Session["IsCustFilter"] = null;
                dtFrom = DateTime.Now;
                dtTo = DateTime.Now;
                //   BindDropDownList();
                //ASPxFromDate.Text = dtFrom.ToString("dd-MM-yyyy");
                //ASPxToDate.Text = dtTo.ToString("dd-MM-yyyy");
                ////Date_finyearwise(Convert.ToString(Session["LastFinYear"]));

                dtBranchSelection = oDBEngine.GetDataTable("select Variable_Value from Config_SystemSettings where Variable_Name='ReportsBranchSelection'");
                hdnBranchSelection.Value = dtBranchSelection.Rows[0][0].ToString();

                ASPxFromDate.Value = DateTime.Now;
                ASPxToDate.Value = DateTime.Now;
                dtFrom = Convert.ToDateTime(ASPxFromDate.Date);
                dtTo = Convert.ToDateTime(ASPxToDate.Date);

                string FROMDATE = dtFrom.ToString("yyyy-MM-dd");
                string TODATE = dtTo.ToString("yyyy-MM-dd");

                //string TABLENAME = "Ledger Details";

                string BRANCH_ID = "";
                if (hdnSelectedBranches.Value != "")
                {
                    BRANCH_ID = hdnSelectedBranches.Value;
                }
                Date_finyearwise(Convert.ToString(Session["LastFinYear"]));

                string GROUP_ID = "";     //Suvankar
                if (hdnSelectedGroups.Value != "")
                {
                    GROUP_ID = hdnSelectedGroups.Value;
                }


                //string CASHBANKTYPE = "";
                //string CASHBANKID = "";
                //string UserId = "";
                string CUSTVENDID = "";
                if (hdnSelectedCustomerVendor.Value != "")
                {
                    CUSTVENDID = hdnSelectedCustomerVendor.Value;
                }

                //string LEDGERID = "";

                //if (hdnSelectedLedger.Value != "")
                //{
                //    LEDGERID = hdnSelectedLedger.Value;
                //}
                //string ISCREATEORPREVIEW = "P";


                //GetSalesRegisterdata(FROMDATE, TODATE, BRANCH_ID, CUSTVENDID);
                //Task PopulateStockTrialDataTask = new Task(() => GetSalesRegisterdata(FROMDATE, TODATE, BRANCH_ID, CUSTVENDID));
                //PopulateStockTrialDataTask.RunSynchronously(); 

                //Rev Subhra 20-12-2018   0017670
                Session["BranchNames"] = null;
                //End Rev

                ShowGrid.Columns[7].Visible = false;
            }
            else
            {
                dtFrom = Convert.ToDateTime(ASPxFromDate.Date);
                dtTo = Convert.ToDateTime(ASPxToDate.Date);
            }


            //BindLedgerPosting();

            //String callbackScript = "function CallServer(arg, context){ " + "" + ";}";
            // Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "CallServer", callbackScript, true);

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
                    Session["exportval"] = Filter;
                    // BindDropDownList();
                    bindexport(Filter);
                }
                else if (Convert.ToInt32(Session["exportval"]) != Filter)
                {
                    Session["exportval"] = Filter;
                    // BindDropDownList();
                    bindexport(Filter);
                }

                //drdExport.SelectedValue = "0";
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
            //drdExport.SelectedValue = "0";
        }
        public void bindexport(int Filter)
        {
            string filename = "PartyledgerPosting";
            exporter.FileName = filename;
            string FileHeader = "";

            exporter.FileName = filename;

            BusinessLogicLayer.Reports RptHeader = new BusinessLogicLayer.Reports();

            //Rev Debashis && Implement Party name for export in Excel
            //FileHeader = RptHeader.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), true, true, true, true, true) + Environment.NewLine + "Partyledger Posting Report" + Environment.NewLine + "For the period " + Convert.ToDateTime(ASPxFromDate.Date).ToString("dd-MM-yyyy") + " To " + Convert.ToDateTime(ASPxToDate.Date).ToString("dd-MM-yyyy");
            FileHeader = RptHeader.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), true, true, true,true, true, true) + Environment.NewLine + "Consolidated Partyledger Report " + "A/C- " + Session["CUSTVENDNM"] + Environment.NewLine + "For the period " + Convert.ToDateTime(ASPxFromDate.Date).ToString("dd-MM-yyyy") + " To " + Convert.ToDateTime(ASPxToDate.Date).ToString("dd-MM-yyyy");
            //End of Rev Debashis

            //Rev Subhra 20-12-2018   0017670
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

        //Rev Subhra 20-12-2018   0017670
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

        //protected void BindLedgerPosting()
        //{
        //    try
        //    {
        //        if (Session["dt_PartyLedgerRpt"] != null)
        //        {
        //            ShowGrid.DataSource = (DataTable)Session["dt_PartyLedgerRpt"];
        //            ShowGrid.DataBind();
        //        }
        //    }
        //    catch { }
        //}
        //protected void Grid_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        //{

        //    //Session.Remove("dt_PartyLedgerRpt");


        //    ShowGrid.JSProperties["cpSave"] = null;


        //    string COMPANYID = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
        //    string Finyear = Convert.ToString(HttpContext.Current.Session["LastFinYear"]);

        //    DateTime dtFrom;
        //    DateTime dtTo;
        //    dtFrom = Convert.ToDateTime(ASPxFromDate.Date);
        //    dtTo = Convert.ToDateTime(ASPxToDate.Date);

        //    string FROMDATE = dtFrom.ToString("yyyy-MM-dd");
        //    string TODATE = dtTo.ToString("yyyy-MM-dd");

        //    //string TABLENAME = "Ledger Details";

        //    string BRANCH_ID = "";
        //    //if (hdnSelectedBranches.Value != "")
        //    //{
        //    //    BRANCH_ID = hdnSelectedBranches.Value;
        //    //}

        //    string Group_ID = "";    //Suvankar
        //    if (lookup_Group.GridView.GetSelectedFieldValues("GroupCode").Count() != GetGroupList().Rows.Count)
        //    {
        //        string QuoComponent3 = "";
        //        List<object> QuoList3 = lookup_Group.GridView.GetSelectedFieldValues("GroupCode");
        //        foreach (object Quo3 in QuoList3)
        //        {
        //            QuoComponent3 += "," + Quo3;
        //        }
        //        Group_ID = QuoComponent3.TrimStart(',');
        //    }

        //    string QuoComponent2 = "";
        //    List<object> QuoList2 = lookup_branch.GridView.GetSelectedFieldValues("ID");
        //    foreach (object Quo2 in QuoList2)
        //    {
        //        QuoComponent2 += "," + Quo2;
        //    }
        //    BRANCH_ID = QuoComponent2.TrimStart(',');


        //    //string CASHBANKTYPE = "";
        //    //string CASHBANKID = "";
        //    //string UserId = "";
        //    string CUSTVENDID = "";
        //    string QuoComponent = "";
        //    List<object> QuoList = lookup_custvend.GridView.GetSelectedFieldValues("ID");
        //    foreach (object Quo in QuoList)
        //    {
        //        QuoComponent += "," + Quo;
        //    }
        //    CUSTVENDID = QuoComponent.TrimStart(',');

        //    //if (hdnSelectedCustomerVendor.Value != "")
        //    //{
        //    //    CUSTVENDID = hdnSelectedCustomerVendor.Value;
        //    //}

        //    //string LEDGERID = "";

        //    //if (hdnSelectedLedger.Value != "")
        //    //{
        //    //    LEDGERID = hdnSelectedLedger.Value;
        //    //}
        //    //string ISCREATEORPREVIEW = "P";


        //    ////GetSalesRegisterdata(FROMDATE, TODATE, BRANCH_ID, CUSTVENDID);



        //    Task PopulateStockTrialDataTask = new Task(() => GetSalesRegisterdata(FROMDATE, TODATE, BRANCH_ID, CUSTVENDID, Group_ID));
        //    PopulateStockTrialDataTask.RunSynchronously();
        //}


        public DataTable GetGroupList()
        {
            try
            {
                DataTable dt = oDBEngine.GetDataTable("select GroupCode, GroupDescription from ( select AccountGroup_ReferenceID as GroupCode, AccountGroup_Name + ' ('+AccountGroup_Type + ')' as GroupDescription from Master_AccountGroup where AccountGroup_Type = 'Liability' or AccountGroup_Type = 'Asset' ) AcGrp order by GroupDescription");
                return dt;
            }
            catch
            {
                return null;
            }
        }

        public void GetCustVendLedgerdata(string FROMDATE, string TODATE, string BRANCH_ID, string CUSTVENDID, string GROUP_ID)
        {
            try
            {
                string DriverName = string.Empty;
                string PhoneNo = string.Empty;
                string VehicleNo = string.Empty;

                DataSet ds = new DataSet();
                //SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("prc_PartyLedger_Report", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@COMPANYID", Convert.ToString(Session["LastCompany"]));
                cmd.Parameters.AddWithValue("@FINYEAR", Convert.ToString(Session["LastFinYear"]).Trim());
                cmd.Parameters.AddWithValue("@FROMDATE", FROMDATE);
                cmd.Parameters.AddWithValue("@TODATE", TODATE);
                cmd.Parameters.AddWithValue("@BRANCHID", BRANCH_ID);
                cmd.Parameters.AddWithValue("@P_CODE", CUSTVENDID);
                cmd.Parameters.AddWithValue("@Customer_Type", drp_partytype.SelectedValue);
                cmd.Parameters.AddWithValue("@Group_Code", GROUP_ID);
                cmd.Parameters.AddWithValue("@Partydate", chkparty.Checked == true ? "1" : "0");
                cmd.Parameters.AddWithValue("@USERID", Convert.ToString(Session["userid"]).Trim());
                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(ds);

                cmd.Dispose();
                con.Dispose();

                //Session["dt_PartyLedgerRpt"] = ds.Tables[0];

                //ShowGrid.DataSource = ds.Tables[0];
                //ShowGrid.DataBind();
            }
            catch (Exception ex)
            {
            }
        }



        //[WebMethod]
        //public static List<string> GetBranchesList(String NoteId)
        //{
        //    BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        //    StringBuilder filter = new StringBuilder();
        //    StringBuilder Supervisorfilter = new StringBuilder();
        //    BusinessLogicLayer.Others objbl = new BusinessLogicLayer.Others();
        //    DataTable dtbl = new DataTable();
        //    if (NoteId.Trim() == "")
        //    {
        //        dtbl = oDBEngine.GetDataTable("select branch_id,branch_description from tbl_master_branch where branch_id in(" + Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]) + ")  order by branch_description asc");

        //    }

        //    List<string> obj = new List<string>();

        //    foreach (DataRow dr in dtbl.Rows)
        //    {

        //        obj.Add(Convert.ToString(dr["branch_description"]) + "|" + Convert.ToString(dr["branch_id"]));
        //    }
        //    return obj;
        //}

        public static List<string> GetGroupList(String NoteId)   //Suvankar
        {
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
            StringBuilder filter = new StringBuilder();
            StringBuilder Supervisorfilter = new StringBuilder();
            BusinessLogicLayer.Others objbl = new BusinessLogicLayer.Others();
            DataTable dtbl = new DataTable();
            if (NoteId.Trim() == "")
            {
                dtbl = oDBEngine.GetDataTable("select GroupCode, GroupDescription from ( select AccountGroup_ReferenceID as GroupCode, AccountGroup_Name + ' ('+AccountGroup_Type + ')' as GroupDescription from Master_AccountGroup where AccountGroup_Type = 'Liability' or AccountGroup_Type = 'Asset' ) AcGrp order by GroupDescription");

            }

            List<string> obj = new List<string>();

            foreach (DataRow dr in dtbl.Rows)
            {

                obj.Add(Convert.ToString(dr["GroupDescription"]) + "|" + Convert.ToString(dr["GroupCode"]));
            }
            return obj;
        }

        //[WebMethod]
        //public static List<string> BindLedgerType(String Ids)
        //{
        //    BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);

        //    DataTable dtbl = new DataTable();
        //    if (Ids.Trim() != "")
        //    {
        //        dtbl = oDBEngine.GetDataTable("select A.MainAccount_ReferenceID AS ID,A.MainAccount_Name as 'AccountName' FROM Master_MainAccount A WHERE A.MainAccount_AccountCode IN(SELECT RTRIM(B.AccountsLedger_MainAccountID) FROM Trans_AccountsLedger B WHERE B.AccountsLedger_BranchId in(" + Ids + ")) ORDER BY A.MainAccount_Name ");

        //    }

        //    List<string> obj = new List<string>();

        //    foreach (DataRow dr in dtbl.Rows)
        //    {

        //        obj.Add(Convert.ToString(dr["AccountName"]) + "|" + Convert.ToString(dr["Id"]));
        //    }
        //    return obj;
        //}



        //[WebMethod]
        //public static List<string> BindCustomerVendor(string type)
        //{
        //    BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);

        //    DataTable dtbl = new DataTable();

        //    //dtbl = oDBEngine.GetDataTable("select cnt_id AS ID,ISNULL(cnt_firstName,'')+' '+ISNULL(cnt_middleName,'')+'  '+ISNULL(cnt_lastName,'') as 'Name'  FROM tbl_master_contact WHERE cnt_contactType in('CL','DV') AND cnt_branchid IN("+ Ids +") ORDER BY ISNULL(cnt_firstName,'')+' '+ISNULL(cnt_middleName,'')+' '+ISNULL(cnt_lastName,'') ");
        //    if (type == "0")
        //    {
        //        //Debashis && Implement Contact No.
        //        //dtbl = oDBEngine.GetDataTable("select cnt_internalId AS ID,ISNULL(cnt_firstName,'')+' '+ISNULL(cnt_middleName,'')+'  '+ISNULL(cnt_lastName,'') as 'Name'  FROM tbl_master_contact WHERE cnt_contactType in('CL','DV') ORDER BY ISNULL(cnt_firstName,'')+' '+ISNULL(cnt_middleName,'')+' '+ISNULL(cnt_lastName,'') ");
        //        dtbl = oDBEngine.GetDataTable("select cnt_internalId AS ID,ISNULL(cnt_firstName,'')+' '+ISNULL(cnt_middleName,'')+' '+ISNULL(cnt_lastName,'') as 'Name',CASE WHEN cnt_contactType='CL' THEN cnt_UCC ELSE '' END AS Contact FROM tbl_master_contact WHERE cnt_contactType in('CL','DV') ORDER BY ISNULL(cnt_firstName,'')+' '+ISNULL(cnt_middleName,'')+' '+ISNULL(cnt_lastName,'') ");
        //    }

        //    else if (type == "1")
        //    {
        //        //Debashis && Implement Contact No.
        //        //dtbl = oDBEngine.GetDataTable("select cnt_internalId AS ID,ISNULL(cnt_firstName,'')+' '+ISNULL(cnt_middleName,'')+'  '+ISNULL(cnt_lastName,'') as 'Name'  FROM tbl_master_contact WHERE cnt_contactType in('CL') ORDER BY ISNULL(cnt_firstName,'')+' '+ISNULL(cnt_middleName,'')+' '+ISNULL(cnt_lastName,'') ");
        //        dtbl = oDBEngine.GetDataTable("select cnt_internalId AS ID,ISNULL(cnt_firstName,'')+' '+ISNULL(cnt_middleName,'')+'  '+ISNULL(cnt_lastName,'') as 'Name',cnt_UCC AS Contact FROM tbl_master_contact WHERE cnt_contactType in('CL') ORDER BY ISNULL(cnt_firstName,'')+' '+ISNULL(cnt_middleName,'')+' '+ISNULL(cnt_lastName,'') ");
        //    }

        //    else if (type == "2")
        //    {
        //        //Debashis && Implement Contact No.
        //        //dtbl = oDBEngine.GetDataTable("select cnt_internalId AS ID,ISNULL(cnt_firstName,'')+' '+ISNULL(cnt_middleName,'')+'  '+ISNULL(cnt_lastName,'') as 'Name'  FROM tbl_master_contact WHERE cnt_contactType in('DV') ORDER BY ISNULL(cnt_firstName,'')+' '+ISNULL(cnt_middleName,'')+' '+ISNULL(cnt_lastName,'') ");
        //        dtbl = oDBEngine.GetDataTable("select cnt_internalId AS ID,ISNULL(cnt_firstName,'')+' '+ISNULL(cnt_middleName,'')+'  '+ISNULL(cnt_lastName,'') as 'Name','' AS Contact FROM tbl_master_contact WHERE cnt_contactType in('DV') ORDER BY ISNULL(cnt_firstName,'')+' '+ISNULL(cnt_middleName,'')+' '+ISNULL(cnt_lastName,'') ");
        //    }

        //    List<string> obj = new List<string>();

        //    foreach (DataRow dr in dtbl.Rows)
        //    {

        //        obj.Add(Convert.ToString(dr["Name"]) + "|" + Convert.ToString(dr["Id"]));
        //    }
        //    return obj;
        //}

        public void Date_finyearwise(string Finyear)
        {
            CommonBL bll1 = new CommonBL();
            DataTable stbill = new DataTable();
            DateTime MinDate, MaxDate;

            stbill = bll1.GetDateFinancila(Finyear);
            if (stbill.Rows.Count > 0)
            {

                ASPxFromDate.MaxDate = Convert.ToDateTime((stbill.Rows[0]["FinYear_EndDate"]));
                ASPxFromDate.MinDate = Convert.ToDateTime((stbill.Rows[0]["FinYear_StartDate"]));

                ASPxToDate.MaxDate = Convert.ToDateTime((stbill.Rows[0]["FinYear_EndDate"]));
                ASPxToDate.MinDate = Convert.ToDateTime((stbill.Rows[0]["FinYear_StartDate"]));

                DateTime MaximumDate = Convert.ToDateTime((stbill.Rows[0]["FinYear_EndDate"]));
                DateTime MinimumDate = Convert.ToDateTime((stbill.Rows[0]["FinYear_StartDate"]));

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


        protected void ShowGrid_SummaryDisplayText(object sender, ASPxGridViewSummaryDisplayTextEventArgs e)
        {
            if (e.Item.FieldName == "DEBIT")
            {
                TotalDebit = Convert.ToDecimal(e.Value);
            }
            else if (e.Item.FieldName == "CREDIT")
            {
                TotalCredit = Convert.ToDecimal(e.Value);
            }

            if (e.Item.FieldName == "Closing_Balance")
            {
                TotalDBCR = TotalDebit - TotalCredit;
                if (TotalDBCR > 0)
                {
                    e.Text = Convert.ToString(Math.Abs((TotalDebit - TotalCredit)));// +" Dr";
                }
                else if (TotalDBCR < 0)
                {
                    e.Text = Convert.ToString(Math.Abs((TotalDebit - TotalCredit)));// +" Cr";
                }
                else if (TotalDBCR == 0)
                {
                    e.Text = Convert.ToString(Math.Abs((TotalDebit - TotalCredit)));
                }
            }
            else
            {
                e.Text = string.Format("{0}", e.Value);
            }

            if (e.Item.FieldName == "Closebal_DBCR")
            {
                if ((TotalDebit - TotalCredit) > 0)
                {
                    e.Text = "Dr.";
                }
                else if ((TotalDebit - TotalCredit) < 0)
                {
                    e.Text = "Cr.";
                }
                else if ((TotalDebit - TotalCredit) == 0)
                {
                    e.Text = "";
                }
            }
        }

        //protected void Showgrid_Htmlprepared(object sender, EventArgs e)
        //{
        //    ASPxGridView grid = (ASPxGridView)sender;
        //    if (drp_partytype.SelectedValue == "0")
        //    {
        //        grid.Columns["CustomerVendor"].Caption = "Customer/Vendor";

        //    }

        //    else if (drp_partytype.SelectedValue == "1")
        //    {
        //        grid.Columns["CustomerVendor"].Caption = "Customer";

        //    }

        //    else if (drp_partytype.SelectedValue == "2")
        //    {
        //        grid.Columns["CustomerVendor"].Caption = "Vendor";

        //    }
        //}


        //protected void ComponentGroup_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)  //Suvankar
        //{
        //    string FinYear = Convert.ToString(Session["LastFinYear"]);

        //    if (e.Parameter.Split('~')[0] == "BindComponentGrid")
        //    {
        //        DataTable ComponentTable = new DataTable();
        //        string Hoid = e.Parameter.Split('~')[1];
        //        ComponentTable = oDBEngine.GetDataTable("select GroupCode, GroupDescription from (select AccountGroup_ReferenceID as GroupCode, AccountGroup_Name + ' ('+AccountGroup_Type + ')' as GroupDescription from Master_AccountGroup where AccountGroup_Type = 'Liability' or AccountGroup_Type = 'Asset' ) AcGrp order by GroupDescription");
        //        if (ComponentTable.Rows.Count > 0)
        //        {
        //            Session["SI_ComponentData_Group"] = ComponentTable;
        //            lookup_Group.DataSource = ComponentTable;
        //            lookup_Group.DataBind();
        //        }
        //        else
        //        {
        //            lookup_Group.DataSource = null;
        //            lookup_Group.DataBind();

        //        }
        //    }
        //}
        //protected void lookup_Group_DataBinding(object sender, EventArgs e) //Suvankar
        //{
        //    if (Session["SI_ComponentData_Group"] != null)
        //    {
        //        //    DataTable ComponentTable = oDBEngine.GetDataTable("select branch_id,branch_description,branch_code from tbl_master_branch where branch_parentId='" + Hoid + "' order by branch_description asc");
        //        lookup_Group.DataSource = (DataTable)Session["SI_ComponentData_Group"];
        //    }
        //}

        //protected void GroupEntityServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        //{

        //    e.KeyExpression = "GroupCode";
        //    string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString;
        //    ReportSourceDataContext dc = new ReportSourceDataContext(connectionString);
        //    var q = from d in dc.v_GroupLists

        //            orderby d.GroupDescription ascending
        //            select d;

        //    e.QueryableSource = q;
        //}

        //protected void ComponentCustomerVendor_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        //{
        //    string FinYear = Convert.ToString(Session["LastFinYear"]);

        //    if (e.Parameter.Split('~')[0] == "BindComponentGrid")
        //    {
        //        ///if (e.Parameter.Split('~')[1] != null) Customer = e.Parameter.Split('~')[1];

        //        string groupList = "";
        //        string type = "";
        //        if (e.Parameter.Split('~')[1] != "GrpWs")
        //        {
        //            type = e.Parameter.Split('~')[1];
        //        }
        //        else
        //        {
        //            string ddlVal = drp_partytype.SelectedValue;
        //            if (ddlVal.ToString() == "0")
        //            {
        //                type = "0";
        //            }
        //            else if (ddlVal.ToString() == "1")
        //            {
        //                type = "1";
        //            }
        //            else if (ddlVal.ToString() == "2")
        //            {
        //                type = "2";
        //            }
        //        }


        //        //     string date2 = Convert.ToDateTime(InvoicecreatedDate).ToString("yyyy-MM-dd");

        //        //    DataTable ComponentTable = objreplacement1.GetComponentInvoice(Customer, date2, FinYear, BranchId);
        //        DataTable ComponentTable = new DataTable();
        //        //          ComponentTable = oDBEngine.GetDataTable("select ROW_NUMBER() over(order by sProducts_ID) SrlNo, sProducts_ID AS ID,sProducts_Code as 'Doc Code',sProducts_Description as 'Name' FROM Master_sProducts WHERE sProduct_IsInventory=1 " +
        //        //"AND sProducts_ID IN(SELECT StockBranchWarehouseDetail_ProductId FROM Trans_StockBranchWarehouseDetails) ORDER BY sProducts_Description");


        //        //if (lookup_Group.GridView.GetSelectedFieldValues("GroupCode").Count() != GetGroupList().Rows.Count)
        //        //{
        //        List<object> GroupList = lookup_Group.GridView.GetSelectedFieldValues("GroupCode");
        //        foreach (object GroupCodes in GroupList)
        //        {
        //            groupList += "," + GroupCodes;
        //        }
        //        groupList = groupList.TrimStart(',');
        //        //}



        //        if (type == "0")
        //        {
        //            if (groupList.Trim() != "")
        //            {
        //                //Debashis && Implement Contact No.
        //                //ComponentTable = oDBEngine.GetDataTable("select cnt_internalId AS ID,ISNULL(cnt_firstName,'')+' '+ISNULL(cnt_middleName,'')+'  '+ISNULL(cnt_lastName,'') as 'Name'  FROM tbl_master_contact WHERE cnt_contactType in('CL','DV') and AccountGroupID IN(" + Convert.ToString(groupList) + ") ORDER BY ISNULL(cnt_firstName,'')+' '+ISNULL(cnt_middleName,'')+' '+ISNULL(cnt_lastName,'') ");
        //                ComponentTable = oDBEngine.GetDataTable("select cnt_internalId AS ID,ISNULL(cnt_firstName,'')+' '+ISNULL(cnt_middleName,'')+' '+ISNULL(cnt_lastName,'') as 'Name',CASE WHEN cnt_contactType='CL' THEN cnt_UCC ELSE '' END as Contact FROM tbl_master_contact WHERE cnt_contactType in('CL','DV') and AccountGroupID IN(" + Convert.ToString(groupList) + ") ORDER BY ISNULL(cnt_firstName,'')+' '+ISNULL(cnt_middleName,'')+' '+ISNULL(cnt_lastName,'') ");
        //            }
        //            else
        //            {
        //                //Debashis && Implement Contact No.
        //                //ComponentTable = oDBEngine.GetDataTable("select cnt_internalId AS ID,ISNULL(cnt_firstName,'')+' '+ISNULL(cnt_middleName,'')+'  '+ISNULL(cnt_lastName,'') as 'Name'  FROM tbl_master_contact WHERE cnt_contactType in('CL','DV') ORDER BY ISNULL(cnt_firstName,'')+' '+ISNULL(cnt_middleName,'')+' '+ISNULL(cnt_lastName,'') ");
        //                ComponentTable = oDBEngine.GetDataTable("select cnt_internalId AS ID,ISNULL(cnt_firstName,'')+' '+ISNULL(cnt_middleName,'')+'  '+ISNULL(cnt_lastName,'') as 'Name',CASE WHEN cnt_contactType='CL' THEN cnt_UCC ELSE '' END as Contact FROM tbl_master_contact WHERE cnt_contactType in('CL','DV') ORDER BY ISNULL(cnt_firstName,'')+' '+ISNULL(cnt_middleName,'')+' '+ISNULL(cnt_lastName,'') ");
        //            }
        //        }

        //        else if (type == "1")
        //        {
        //            //Debashis && Implement Contact No.
        //            //ComponentTable = oDBEngine.GetDataTable("select cnt_internalId AS ID,ISNULL(cnt_firstName,'')+' '+ISNULL(cnt_middleName,'')+'  '+ISNULL(cnt_lastName,'') as 'Name'  FROM tbl_master_contact WHERE cnt_contactType in('CL') ORDER BY ISNULL(cnt_firstName,'')+' '+ISNULL(cnt_middleName,'')+' '+ISNULL(cnt_lastName,'') ");
        //            //Rev Debashis && Implement Sales Invoice for Vendor.
        //            //ComponentTable = oDBEngine.GetDataTable("select cnt_internalId AS ID,ISNULL(cnt_firstName,'')+' '+ISNULL(cnt_middleName,'')+'  '+ISNULL(cnt_lastName,'') as 'Name',cnt_UCC as Contact FROM tbl_master_contact WHERE cnt_contactType in('CL') ORDER BY ISNULL(cnt_firstName,'')+' '+ISNULL(cnt_middleName,'')+' '+ISNULL(cnt_lastName,'') ");
        //            ComponentTable = oDBEngine.GetDataTable("select * from (select cnt_internalId AS ID,ISNULL(cnt_firstName,'')+' '+ISNULL(cnt_middleName,'')+' '+ISNULL(cnt_lastName,'') as 'Name',cnt_UCC as Contact FROM tbl_master_contact WHERE cnt_contactType='CL' "+ 
        //            "union ALL " +
        //            "select distinct cnt_internalId AS ID,ISNULL(cnt_firstName,'')+' '+ISNULL(cnt_middleName,'')+' '+ISNULL(cnt_lastName,'') as 'Name',cnt_UCC as Contact FROM tbl_master_contact cn " +
        //            "inner join tbl_trans_salesinvoice si on si.Customer_Id=cn.cnt_internalId and si.InvoiceFor='DV' and cn.cnt_contactType='DV') a " +
        //            "ORDER BY Name ");
        //            //End of Rev Debashis
        //        }

        //        else if (type == "2")
        //        {
        //            if (groupList.Trim() != "")
        //            {
        //                //Debashis && Implement Contact No.
        //                //ComponentTable = oDBEngine.GetDataTable("select cnt_internalId AS ID,ISNULL(cnt_firstName,'')+' '+ISNULL(cnt_middleName,'')+'  '+ISNULL(cnt_lastName,'') as 'Name'  FROM tbl_master_contact WHERE cnt_contactType in('DV') and AccountGroupID IN(" + Convert.ToString(groupList) + ") ORDER BY ISNULL(cnt_firstName,'')+' '+ISNULL(cnt_middleName,'')+' '+ISNULL(cnt_lastName,'') ");
        //                //Rev Debashis && Implement Purchase Invoice for Customer.
        //                //ComponentTable = oDBEngine.GetDataTable("select cnt_internalId AS ID,ISNULL(cnt_firstName,'')+' '+ISNULL(cnt_middleName,'')+'  '+ISNULL(cnt_lastName,'') as 'Name','' AS Contact FROM tbl_master_contact WHERE cnt_contactType in('DV') and AccountGroupID IN(" + Convert.ToString(groupList) + ") ORDER BY ISNULL(cnt_firstName,'')+' '+ISNULL(cnt_middleName,'')+' '+ISNULL(cnt_lastName,'') ");
        //                ComponentTable = oDBEngine.GetDataTable("select * from (select cnt_internalId AS ID,ISNULL(cnt_firstName,'')+' '+ISNULL(cnt_middleName,'')+' '+ISNULL(cnt_lastName,'') as 'Name','' AS Contact FROM tbl_master_contact WHERE cnt_contactType='DV' and AccountGroupID IN(" + Convert.ToString(groupList) + ") "+
        //                "union ALL " +
        //                "select distinct cnt_internalId AS ID,ISNULL(cnt_firstName,'')+' '+ISNULL(cnt_middleName,'')+' '+ISNULL(cnt_lastName,'') as 'Name',cnt_UCC as Contact FROM tbl_master_contact cn " +
        //                "inner join tbl_trans_purchaseinvoice pi on pi.Vendor_Id=cn.cnt_internalId and pi.InvoiceFor='CL' and cn.cnt_contactType='CL' ) a " +
        //                "ORDER BY Name ");
        //                //End of Rev Debashis
        //            }
        //            else
        //            {
        //                //Debashis && Implement Contact No.
        //                //ComponentTable = oDBEngine.GetDataTable("select cnt_internalId AS ID,ISNULL(cnt_firstName,'')+' '+ISNULL(cnt_middleName,'')+'  '+ISNULL(cnt_lastName,'') as 'Name'  FROM tbl_master_contact WHERE cnt_contactType in('DV') ORDER BY ISNULL(cnt_firstName,'')+' '+ISNULL(cnt_middleName,'')+' '+ISNULL(cnt_lastName,'') ");
        //                //Rev Debashis && Implement Purchase Invoice for Customer.
        //                //ComponentTable = oDBEngine.GetDataTable("select cnt_internalId AS ID,ISNULL(cnt_firstName,'')+' '+ISNULL(cnt_middleName,'')+'  '+ISNULL(cnt_lastName,'') as 'Name','' AS Contact FROM tbl_master_contact WHERE cnt_contactType in('DV') ORDER BY ISNULL(cnt_firstName,'')+' '+ISNULL(cnt_middleName,'')+' '+ISNULL(cnt_lastName,'') ");
        //                ComponentTable = oDBEngine.GetDataTable("select * from (select cnt_internalId AS ID,ISNULL(cnt_firstName,'')+' '+ISNULL(cnt_middleName,'')+' '+ISNULL(cnt_lastName,'') as 'Name','' AS Contact FROM tbl_master_contact WHERE cnt_contactType='DV' "+
        //                "union ALL " +
        //                "select distinct cnt_internalId AS ID,ISNULL(cnt_firstName,'')+' '+ISNULL(cnt_middleName,'')+' '+ISNULL(cnt_lastName,'') as 'Name',cnt_UCC as Contact FROM tbl_master_contact cn " +
        //                "inner join tbl_trans_purchaseinvoice pi on pi.Vendor_Id=cn.cnt_internalId and pi.InvoiceFor='CL' and cn.cnt_contactType='CL' ) a " +
        //                "ORDER BY Name ");
        //                //End of Rev Debashis
        //            }

        //        }

        //        if (ComponentTable.Rows.Count > 0)
        //        {
        //            // grid_Products.JSProperties["cptxt_InvoiceDate"] = Convert.ToString(ComponentTable.Rows[0]["Invoice_Date"]);
        //            Session["SI_ComponentData"] = ComponentTable;
        //            lookup_custvend.DataSource = ComponentTable;
        //            lookup_custvend.DataBind();
        //        }
        //        else
        //        {
        //            Session["SI_ComponentData"] = null;
        //            lookup_custvend.DataSource = null;
        //            lookup_custvend.DataBind();
        //        }
        //    }
        //}

        //protected void lookup_vendor_DataBinding(object sender, EventArgs e)
        //{
        //    //   DataTable ComponentTable = new DataTable();

        //    if (Session["SI_ComponentData"] != null)
        //    {
        //        lookup_custvend.DataSource = (DataTable)Session["SI_ComponentData"];
        //    }
        //}

        //protected void VendorCustomerEntityServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        //{
        //    e.KeyExpression = "ID";
        //    string type = "";
        //    string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString;
        //    string IsSelect = Convert.ToString(hdfVendor.Value);
        //    if (IsSelect == "1") //Vendor
        //    {
        //        hdfVendor.Value = "0";
        //        lookup_custvend.GridView.Selection.UnselectAll();
        //        type = "2";
        //    }
        //    else if(IsSelect == "2") //Customer
        //    {
        //        hdfVendor.Value = "0";
        //        lookup_custvend.GridView.Selection.UnselectAll();
        //        lookup_Group.GridView.Selection.UnselectAll();
        //        type = "1";
        //    }
        //    else
        //    {
        //        string ddlVal = drp_partytype.SelectedValue;
        //        if (ddlVal.ToString() == "0")
        //        {
        //            type = "0";
        //        }
        //        else if (ddlVal.ToString() == "1")
        //        {
        //            type = "1";
        //        }
        //        else if (ddlVal.ToString() == "2")
        //        {
        //            type = "2";
        //        }
        //    }

        //    string groupList = "";
        //    List<object> GroupList = lookup_Group.GridView.GetSelectedFieldValues("GroupCode");
        //    foreach (object GroupCodes in GroupList)
        //    {
        //        groupList += "," + GroupCodes;
        //    }
        //    groupList = groupList.TrimStart(',');

        //    ReportSourceDataContext dc = new ReportSourceDataContext(connectionString);
        //    if (type == "0")
        //    {
        //        if (groupList.Trim() != "")
        //        {
        //            List<int> GroupListINT;
        //            GroupListINT = new List<int>(Array.ConvertAll(groupList.Split(','), int.Parse));
        //            var q = from d in dc.v_ReportCustomerVendorLists
        //                    where GroupListINT.Contains(Convert.ToInt32(d.AccountGroupID))
        //                    orderby d.Name ascending
        //                    select d;
        //            e.QueryableSource = q;
        //        }
        //        else
        //        {
        //            var q = from d in dc.v_ReportCustomerVendorLists
        //                    orderby d.Name ascending
        //                    select d;
        //            e.QueryableSource = q;
        //        }
        //    }
        //    else if(type == "1")
        //    {
        //        var q = from d in dc.v_ReportCustomerVendorLists
        //                where d.cnt_contactType == "CL"
        //                orderby d.Name ascending
        //                select d;
        //        e.QueryableSource = q;
        //    }
        //    else if (type == "2")
        //    {
        //        if (groupList.Trim() != "")
        //        {
        //            List<int> GroupListINT;
        //            GroupListINT = new List<int>(Array.ConvertAll(groupList.Split(','), int.Parse));
        //            var q = from d in dc.v_ReportCustomerVendorLists
        //                    where GroupListINT.Contains(Convert.ToInt32(d.AccountGroupID)) &&
        //                            d.cnt_contactType == "DV"
        //                    orderby d.Name ascending
        //                    select d;
        //            e.QueryableSource = q;
        //        }
        //        else
        //        {
        //            var q = from d in dc.v_ReportCustomerVendorLists
        //                    where d.cnt_contactType == "DV"
        //                    orderby d.Name ascending
        //                    select d;
        //            e.QueryableSource = q;
        //        }
        //    }
        //}
        #region Branch Populate

        protected void Componentbranch_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
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
        }

        protected void lookup_branch_DataBinding(object sender, EventArgs e)
        {
            if (Session["SI_ComponentData_Branch"] != null)
            {
                //    DataTable ComponentTable = oDBEngine.GetDataTable("select branch_id,branch_description,branch_code from tbl_master_branch where branch_parentId='" + Hoid + "' order by branch_description asc");
                lookup_branch.DataSource = (DataTable)Session["SI_ComponentData_Branch"];
            }
        }

        //protected void BranchEntityServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        //{
        //    e.KeyExpression = "ID";
        //    string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString;
        //    List<int> branchidlist;
        //    string BranchList = Convert.ToString(Session["userbranchHierarchy"]);
        //    branchidlist = new List<int>(Array.ConvertAll(BranchList.Split(','), int.Parse));

        //    ReportSourceDataContext dc = new ReportSourceDataContext(connectionString);
        //    var q = from d in dc.v_BranchLists
        //            where
        //            branchidlist.Contains(Convert.ToInt32(d.ID))
        //            orderby d.branch_description ascending
        //            select d;

        //    e.QueryableSource = q;
        //}

        #endregion

        //protected void dgvVIEW_CustomSummaryCalculate(object sender, CustomSummaryEventArgs e)
        //{
        //    if (e.Item == ShowGrid.TotalSummary["Closing_Balance"])
        //    {
        //        if (e.SummaryProcess == DevExpress.Data.CustomSummaryProcess.Finalize)
        //        {
        //            Decimal gmv = Convert.ToDecimal(ShowGrid.GetTotalSummaryValue(ShowGrid.TotalSummary["DEBIT"]));
        //            Decimal equity = Convert.ToDecimal(ShowGrid.GetTotalSummaryValue(ShowGrid.TotalSummary["CREDIT"]));

        //            e.TotalValue = gmv - equity;
        //            if (Convert.ToDecimal(e.TotalValue) != 0)
        //            {
        //                if (Convert.ToDecimal(e.TotalValue) < 0)
        //                {

        //                    e.TotalValue = System.Math.Abs(Convert.ToDecimal(e.TotalValue)) + "Cr";
        //                }
        //                else
        //                {


        //                    e.TotalValue = e.TotalValue + "Dr";

        //                }

        //            }

        //            e.TotalValueReady = true;
        //        }
        //    }

        //}

        protected void CallbackPanel_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            string COMPANYID = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
            string Finyear = Convert.ToString(HttpContext.Current.Session["LastFinYear"]);
            string IsCustFilter = Convert.ToString(hfIsCustFilter.Value);
            Session["IsCustFilter"] = IsCustFilter;
            DateTime dtFrom;
            DateTime dtTo;
            dtFrom = Convert.ToDateTime(ASPxFromDate.Date);
            dtTo = Convert.ToDateTime(ASPxToDate.Date);

            string FROMDATE = dtFrom.ToString("yyyy-MM-dd");
            string TODATE = dtTo.ToString("yyyy-MM-dd");

            string BRANCH_ID = "";

            string Group_ID = "";    //Suvankar
            //if (lookup_Group.GridView.GetSelectedFieldValues("GroupCode").Count() != GetGroupList().Rows.Count)
            //{
            //    string QuoComponent3 = "";
            //    List<object> QuoList3 = lookup_Group.GridView.GetSelectedFieldValues("GroupCode");
            //    foreach (object Quo3 in QuoList3)
            //    {
            //        QuoComponent3 += "," + Quo3;
            //    }
            //    Group_ID = QuoComponent3.TrimStart(',');
            //}
            Group_ID = hdnSelectedGroups.Value;

            string QuoComponent2 = "";
            List<object> QuoList2 = lookup_branch.GridView.GetSelectedFieldValues("ID");
            foreach (object Quo2 in QuoList2)
            {
                QuoComponent2 += "," + Quo2;
            }
            BRANCH_ID = QuoComponent2.TrimStart(',');

            //string CASHBANKTYPE = "";
            //string CASHBANKID = "";
            //string UserId = "";
            string CUSTVENDID = "";
            //string QuoComponent = "";
            //List<object> QuoList = lookup_custvend.GridView.GetSelectedFieldValues("ID");
            //foreach (object Quo in QuoList)
            //{
            //    QuoComponent += "," + Quo;
            //}
            //CUSTVENDID = QuoComponent.TrimStart(',');
            CUSTVENDID = hdnSelectedCustomerVendor.Value;
            //Rev Debashis && Implement Party name for export in Excel
            string CUSTVENDNM = "";
            //string CustVendName = "";
            //List<object> CustVendList = lookup_custvend.GridView.GetSelectedFieldValues("Name");
            //foreach (object CustVend in CustVendList)
            //{
            //    CustVendName += "," + CustVend;
            //}
            //CUSTVENDNM = CustVendName.TrimStart(',');
            CUSTVENDNM = txtCustVendName.Text;
            Session["CUSTVENDNM"] = CUSTVENDNM.Trim();
            //End of Rev Debashis

            //Rev Subhra 20-12-2018   0017670

            string BRANCH_NAME = "";
            string BranchNameComponent = "";
            List<object> BranchNameList = lookup_branch.GridView.GetSelectedFieldValues("branch_description");
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
            CallbackPanel.JSProperties["cpBranchNames"] = Convert.ToString(Session["BranchNames"]);

            //End of Rev


            GetCustVendLedgerdata(FROMDATE, TODATE, BRANCH_ID, CUSTVENDID, Group_ID);
        }
        protected void GenerateEntityServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "RID";
            //string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString;
            SqlConnection connectionString = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            string Userid = Convert.ToString(HttpContext.Current.Session["userid"]);
            string type = "";
            string ddlVal = drp_partytype.SelectedValue;
            if (ddlVal.ToString() == "0")
            {
                type = "0";
            }
            else if (ddlVal.ToString() == "1")
            {
                type = "1";
            }
            else if (ddlVal.ToString() == "2")
            {
                type = "2";
            }
            //Rev Debashis 0025193
            else if (ddlVal.ToString() == "3")
            {
                type = "3";
            }
            //End of Rev Debashis 0025193
            ReportSourceDataContext dc = new ReportSourceDataContext(connectionString);

            if (Convert.ToString(Session["IsCustFilter"]) == "Y")
            {
                var q = from d in dc.CONSPARTYLEDGER_REPORTs
                        where Convert.ToString(d.CUSTVENDTYPE) == type && Convert.ToString(d.USERID) == Userid
                        orderby d.RID ascending
                        select d;
                e.QueryableSource = q;
            }
            else
            {
                var q = from d in dc.CONSPARTYLEDGER_REPORTs
                        where Convert.ToString(d.RID) == "0"
                        orderby d.RID ascending
                        select d;
                e.QueryableSource = q;
            }

            string strshownarrationchk = (chkShowHNarratn.Checked) ? "1" : "0";
            if (Convert.ToString(strshownarrationchk) == "0")
            {
                ShowGrid.Columns[7].Visible = false;
            }
            else
            {
                ShowGrid.Columns[7].Visible = true;
            }
        }
    }

}