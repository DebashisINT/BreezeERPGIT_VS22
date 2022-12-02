using OpeningBusinessLogic;
using BusinessLogicLayer.Replacement;
using DevExpress.Web;
using DevExpress.Web.Data;
using EntityLayer.CommonELS;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using OpeningBusinessLogic.Customerconsolidate;
using BusinessLogicLayer;
using OpeningEntry.OpeningEntry.DBML;
using DataAccessLayer;
using System.Globalization;

namespace OpeningEntry.ERP
{
    public partial class ConsolidatedCustomer : System.Web.UI.Page
    {

        DataTable dst = new DataTable();
        string strBranchID = "";
        Consolidatecustomer obj = new Consolidatecustomer();
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

        protected void Page_PreInit(object sender, EventArgs e) // lead add
        {

            if (!IsPostBack)
            {
                string sPath = Convert.ToString(HttpContext.Current.Request.Url);


                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }
        protected void Page_Init(object sender, EventArgs e) // lead add
        {
            dsCustomer.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            SqlSchematype.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
        
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/ERP/ConsolidatedCustomerList.aspx");

            CommonBL ComBL = new CommonBL();
            string ProjectSelectInEntryModule = ComBL.GetSystemSettingsResult("ProjectSelectInEntryModule");
            hdnProject.Value = ProjectSelectInEntryModule;
            if (!String.IsNullOrEmpty(ProjectSelectInEntryModule))
            {
                if (ProjectSelectInEntryModule == "Yes")
                {
                    hdnProjectSelectInEntryModule.Value = "1";
                    lookup_Project.ClientVisible = true;
                    lblProject.Visible = true;
                    OpeningGrid.Columns[3].Visible = true;
                }
                else if (ProjectSelectInEntryModule.ToUpper().Trim() == "NO")
                {
                    hdnProjectSelectInEntryModule.Value = "0";
                    lookup_Project.ClientVisible = false;
                    lblProject.Visible = false;
                    OpeningGrid.Columns[3].Visible = false;
                }
            }
            
            //For Hierarchy Start Tanmoy
            string HierarchySelectInEntryModule = ComBL.GetSystemSettingsResult("Show_Hierarchy");
            if (!String.IsNullOrEmpty(HierarchySelectInEntryModule))
            {
                if (HierarchySelectInEntryModule.ToUpper().Trim() == "YES")
                {
                    ddlHierarchy.Visible = true;
                    lblHierarchy.Visible = true;
                    lookup_Project.Columns[3].Visible = true;
                    OpeningGrid.Columns[4].Visible = true;
                }
                else if (HierarchySelectInEntryModule.ToUpper().Trim() == "NO")
                {
                    ddlHierarchy.Visible = false;
                    lblHierarchy.Visible = false;
                    lookup_Project.Columns[3].Visible = false;
                    OpeningGrid.Columns[4].Visible = false;
                }
            }
            //For Hierarchy End Tanmoy
            string ProjectMandatoryInEntry = ComBL.GetSystemSettingsResult("ProjectMandatoryInEntry");
            if (!String.IsNullOrEmpty(ProjectMandatoryInEntry))
            {
                if (ProjectMandatoryInEntry == "Yes")
                {
                    hddnProjectMandatory.Value = "1";
                }
                else if (ProjectMandatoryInEntry.ToUpper().Trim() == "NO")
                {
                    hddnProjectMandatory.Value = "0";
                }
            }

            if (!IsPostBack)
            {
                Session["exportval2"] = null;

                //Tanmoy Hierarchy
                bindHierarchy();
                ddlHierarchy.Enabled = false;
                //Tanmoy Hierarchy End

                Branchpopulate();
                Agentpopulate();
                strBranchID = Convert.ToString(Session["userbranchID"]);
                DataTable dtsale = obj.GetCustomersalesFinancialyear(Convert.ToString(Session["LastFinYear"]));
                if (dtsale.Rows.Count > 0)
                {
                    dt_date.MaxDate = Convert.ToDateTime(Convert.ToString(dtsale.Rows[0]["FinYear_StartDate"])).AddDays(-1);
                    dt_date2.MaxDate = Convert.ToDateTime(Convert.ToString(dtsale.Rows[0]["FinYear_StartDate"])).AddDays(-1);
                }
                if (Request.QueryString["CustomerId"] != null)
                {
                    OpeningGrid.Visible = true;
                    //divgrid.Visible = true;
                    hdncus.Value = "1";
                    hiddnmodid.Value = Request.QueryString["CustomerId"];
                    ddl_Branch.Enabled = false;
                    lookup_Customer.ReadOnly = true;
                    ddltype.Enabled = false;
                    OpeningGrid.DataSource = GetConsolidatedCustomerGridData(Request.QueryString["CustomerId"], Request.QueryString["branch"]);
                    OpeningGrid.DataBind();


                }
                else
                {
                    ///  divgrid.Visible = false;
                    hiddnmodid.Value = "0";
                    hdncus.Value = "0";
                }
            }
        }


        #region ########  Branch Populate  #######
        protected void Branchpopulate()
        {
            // DataTable dst = new DataTable();
            string userbranchID = Convert.ToString(Session["userbranchID"]);
            dst = obj.GetBranch(Convert.ToInt32(HttpContext.Current.Session["userbranchID"]), Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));

            if (dst.Rows.Count > 0)
            {


                dst.DefaultView.RowFilter = "branch_id <>0";

                ddl_Branch.DataSource = dst.DefaultView;

                //   ddl_Branch.DataSource = dst;
                ddl_Branch.DataTextField = "branch_code";
                ddl_Branch.DataValueField = "branch_id";
                ddl_Branch.DataBind();
                // ddl_Branch.SelectedValue = strBranchID;
                if (Request.QueryString["branch"] != null)
                {
                    ddl_Branch.SelectedValue = Request.QueryString["branch"];

                    Cache["name"] = ddl_Branch.SelectedValue;
                }
                //  ddl_Branch.SelectedValue = strBranchID;
                else if (Session["userbranchID"] != null)
                {
                    ddl_Branch.SelectedValue = userbranchID;
                }
            }
        }

        #endregion

        #region ########  Salesman Agent Populate  #######
        protected void Agentpopulate()
        {

            dst = obj.GetAgents("GetSalesmanAgent", Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));

            if (dst.Rows.Count > 0)
            {

                cmbContactPerson.DataSource = dst;
                cmbContactPerson.TextField = "Name";
                cmbContactPerson.ValueField = "cnt_internalId";
                cmbContactPerson.DataBind();


            }
        }

        #endregion


        #region #########   Contact Person Bind(Salesman)   #########

        protected void cmbContactPerson_Callback(object sender, CallbackEventArgsBase e)
        {
            string WhichCall = e.Parameter.Split('~')[0];
            if (WhichCall == "BindContactPerson")
            {
                string InternalId = Convert.ToString(Convert.ToString(e.Parameter.Split('~')[1]));
                PopulateContactPersonOfCustomer(InternalId);



                //DataTable dtTotalDues = objSalesInvoiceBL.GetCustomerTotalDues(InternalId);
                //if (dtTotalDues != null && dtTotalDues.Rows.Count > 0)
                //{
                //    string totalDues = Convert.ToString(dtTotalDues.Rows[0]["NetOutstanding"]);
                //    cmbContactPerson.JSProperties["cpTotalDue"] = totalDues;
                //}
                //else
                //{
                //    cmbContactPerson.JSProperties["cpTotalDue"] = "0.00";
                //}
            }
        }
        protected void PopulateContactPersonOfCustomer(string InternalId)
        {
            string ContactPerson = "";
            DataTable dtContactPerson = new DataTable();

            dtContactPerson = obj.PopulateContactPersonOfCustomer(InternalId);
            if (dtContactPerson != null && dtContactPerson.Rows.Count > 0)
            {
                cmbContactPerson.TextField = "contactperson";
                cmbContactPerson.ValueField = "add_id";
                cmbContactPerson.DataSource = dtContactPerson;
                cmbContactPerson.DataBind();
                foreach (DataRow dr in dtContactPerson.Rows)
                {
                    if (Convert.ToString(dr["Isdefault"]) == "True")
                    {
                        ContactPerson = Convert.ToString(dr["add_id"]);
                        break;
                    }
                }
                cmbContactPerson.SelectedItem = cmbContactPerson.Items.FindByValue(ContactPerson);
            }
        }

        #endregion


        #region  ###### Insert  Update  DATA  through Grid Perform Call Back  ##############
        protected void OpeningGrid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            ASPxGridView grid = lookup_Customer.GridView;
            object cuname = grid.GetRowValues(grid.FocusedRowIndex, new string[] { "Name" });
            List<ConsolidatedCustomerClass> customerconsolidate = new List<ConsolidatedCustomerClass>();
            string returnPara = Convert.ToString(e.Parameters);

            string FinYear = String.Empty;
            string User = String.Empty;
            string Company = String.Empty;


            //if (Session["LastFinYear"] != null)
            //{
            //    FinYear = Convert.ToString(Session["LastFinYear"]).Trim();
            //}
            DataTable dttab;


            string WhichCall = returnPara.Split('~')[0];

            //  string AmountOs = (ddltype.SelectedValue == "SB" || ddltype.SelectedValue == "RET") ? txt_docamt.Text : txt_docamt2.Text;
            string AmountOs = (ddltype.SelectedValue == "SB" || ddltype.SelectedValue == "RET") ? txt_docamt.Text : (ddltype.SelectedValue == "CDB" || ddltype.SelectedValue == "CCR") ? txt_vendor_amt.Text : txt_docamt2.Text;


            string Typeget = ddltype.SelectedValue;
            string CustomerID = Convert.ToString(lookup_Customer.Value);


            if (WhichCall == "TemporaryData")
            {
                Cache["name"] = ddl_Branch.SelectedValue;
                DataTable dt = new DataTable();
                string docnumber = (ddltype.SelectedValue == "SB" || ddltype.SelectedValue == "RET") ? txt_Docno.Text : txt_Docno2.Text;
                dt = obj.GetDuplicateDoc("DuplicateDoccheck", docnumber, "");
                if (Convert.ToString(dt.Rows[0][0]) == "0" && (ddltype.SelectedValue != "CDB" || ddltype.SelectedValue != "CCR"))
                {
                    if (Session["LastFinYear"] != null)
                    {
                        if (ddltype.SelectedValue == "SB" || ddltype.SelectedValue == "RET")
                        {
                            dttab = obj.GetCustomersalesFinancialyearCode(Convert.ToDateTime(dt_date.Date));
                        }
                        else if (ddltype.SelectedValue == "CDB" || ddltype.SelectedValue == "CCR")
                        {
                            dttab = obj.GetCustomersalesFinancialyearCode(Convert.ToDateTime(dt_vendor.Date));
                        }
                        else
                        {
                            dttab = obj.GetCustomersalesFinancialyearCode(Convert.ToDateTime(dt_date2.Date));
                        }
                        if (dttab.Rows.Count > 0)
                        {
                            FinYear = Convert.ToString(dttab.Rows[0]["FinYear_Code"]);
                        }

                    }
                    if (Session["userid"] != null)
                    {
                        User = Convert.ToString(HttpContext.Current.Session["userid"]).Trim();
                    }

                    if (Session["LastCompany"] != null)
                    {
                        Company = Convert.ToString(Session["LastCompany"]);
                    }


                    //if (Session["GetData"] !=null)
                    //{
                    //    customerconsolidate = (List<ConsolidatedCustomerClass>)Session["GetData"];

                    //}


                    customerconsolidate.Add(new ConsolidatedCustomerClass()
                    {

                        Branch = ddl_Branch.SelectedItem.Text,
                        BranchId = ddl_Branch.SelectedValue,
                        CustomerId = Convert.ToString(lookup_Customer.Value),
                        Customer = cuname.ToString(),
                        Type = ddltype.SelectedItem.Text,
                        TypeId = ddltype.SelectedValue,
                        DocNumber = (ddltype.SelectedValue == "SB" || ddltype.SelectedValue == "RET") ? txt_Docno.Text : txt_Docno2.Text,
                        Date = dt_date.Text,
                        // Date_db = (ddltype.SelectedValue == "SB" || ddltype.SelectedValue == "RET") ? (String.IsNullOrEmpty(dt_date.Text) ? default(DateTime?) : Convert.ToDateTime(dt_date.Date)) : (String.IsNullOrEmpty(dt_date2.Text) ? default(DateTime?) : Convert.ToDateTime(dt_date2.Date)),
                        Date_db = (ddltype.SelectedValue == "SB" || ddltype.SelectedValue == "RET") ? (String.IsNullOrEmpty(dt_date.Text) ? default(DateTime?) : Convert.ToDateTime(dt_date.Date)) : (ddltype.SelectedValue == "CDB" || ddltype.SelectedValue == "CCR") ? (String.IsNullOrEmpty(dt_vendor.Text) ? default(DateTime?) : Convert.ToDateTime(dt_vendor.Date)) : (String.IsNullOrEmpty(dt_date2.Text) ? default(DateTime?) : Convert.ToDateTime(dt_date2.Date)),
                        
                        FullBill = txt_fullbill.Text,
                        DueDate = dtdate_Due.Text,
                        DueDate_db = (String.IsNullOrEmpty(dtdate_Due.Text) ? default(DateTime?) : Convert.ToDateTime(dtdate_Due.Date)),
                        RefDate_db = (String.IsNullOrEmpty(dtdate_Ref.Text) ? default(DateTime?) : Convert.ToDateTime(dtdate_Ref.Date)),
                        //DocAmount = (ddltype.SelectedValue == "SB" || ddltype.SelectedValue == "RET") ? txt_docamt.Text : txt_docamt2.Text,
                        //DSAmount = txt_disamt.Text,
                        DocAmount = txt_disamt.Text,
                        ///    DSAmount = (ddltype.SelectedValue == "SB" || ddltype.SelectedValue == "RET") ? txt_docamt.Text : txt_docamt2.Text,
                        DSAmount = (ddltype.SelectedValue == "SB" || ddltype.SelectedValue == "RET") ? txt_docamt.Text : (ddltype.SelectedValue == "CDB" || ddltype.SelectedValue == "CCR") ? txt_vendor_amt.Text : txt_docamt2.Text,

                        AgentName = cmbContactPerson.Text,
                        AgentId = Convert.ToString(cmbContactPerson.Value),
                        Commpercntag = (ddltype.SelectedValue == "SB" || ddltype.SelectedValue == "RET") ? txt_commprcntg.Text : txt_commprcntg2.Text,
                        CommmAmt = (ddltype.SelectedValue == "SB" || ddltype.SelectedValue == "RET") ? txt_commAmt.Text : txt_commAmt2.Text,
                        Company = Company,
                        FinYear = FinYear,
                        User = User
                    });


                    string CustomerconsolidateXML = Consolidatecustomer.ConvertToXml(customerconsolidate, 0);



                    string RetAmount = Convert.ToString(crtxtRetAmount.Text);
                    string GL = Convert.ToString(hdnROMainAc.Value);
                    DateTime? dtDuedt = null;

                    if (dtDueDate.Text != null && dtDueDate.Text != "")
                    {
                        dtDuedt = dtDueDate.Date;
                    }





                    //Project code add Tanmoy
                    CommonBL cbl = new CommonBL();
                    string ProjectSelectcashbankModule = cbl.GetSystemSettingsResult("ProjectSelectInEntryModule");
                    Int64 ProjId = 0;
                    if (lookup_Project.Text != "")
                    {
                        string projectCode = lookup_Project.Text;
                        DataTable dtSlOrd = GetProjectCode(projectCode);
                        //oDbEngine.GetDataTable("select Proj_Id from Master_ProjectManagement where Proj_Code='" + projectCode + "'");
                        ProjId = Convert.ToInt64(dtSlOrd.Rows[0]["Proj_Id"]);
                    }
                    else if (lookup_Project.Text == "")
                    {
                        ProjId = 0;
                    }
                    else
                    {
                        ProjId = 0;
                    }
                    //Project code add Tanmoy

                    int i2 = obj.InsertReplacementDetails(CustomerconsolidateXML, "InserCustomerConsolidate", AmountOs, Typeget, CustomerID, 0, ProjId,RetAmount,GL,dtDuedt);


                    if (i2 > 0)
                    {

                        OpeningGrid.JSProperties["cpSaveSuccessOrFail"] = "Success";
                    }
                    else
                    {
                        OpeningGrid.JSProperties["cpSaveSuccessOrFail"] = "Mentatory";
                    }
                    // Session["GetData"] = customerconsolidate;
                }
                else
                {

                    OpeningGrid.JSProperties["cpSaveSuccessOrFail"] = "Duplicate";
                }
            }


            if (WhichCall == "ModifyData")
            {
                Cache["name"] = ddl_Branch.SelectedValue;


                int ModId = Int32.Parse(returnPara.Split('~')[1]);
                DataTable dt = new DataTable();
                string docnumber = (ddltype.SelectedValue == "SB" || ddltype.SelectedValue == "RET") ? txt_Docno.Text : txt_Docno2.Text;
                dt = obj.GetDuplicateDoc("DuplicateDoccheck", docnumber, Convert.ToString(ModId));
                if (Convert.ToString(dt.Rows[0][0]) == "0")
                {
                    customerconsolidate.Add(new ConsolidatedCustomerClass()
                    {
                        ModId = ModId,
                        Branch = ddl_Branch.SelectedItem.Text,
                        BranchId = ddl_Branch.SelectedValue,
                        CustomerId = Convert.ToString(lookup_Customer.Value),
                        Customer = Convert.ToString(cuname),
                        Type = ddltype.SelectedItem.Text,
                        TypeId = ddltype.SelectedValue,
                        DocNumber = (ddltype.SelectedValue == "SB" || ddltype.SelectedValue == "RET") ? txt_Docno.Text : txt_Docno2.Text,
                        Date = dt_date.Text,
                        // Date_db = (ddltype.SelectedValue == "SB" || ddltype.SelectedValue == "RET") ? (String.IsNullOrEmpty(dt_date.Text) ? default(DateTime?) : Convert.ToDateTime(dt_date.Date)) : (String.IsNullOrEmpty(dt_date2.Text) ? default(DateTime?) : Convert.ToDateTime(dt_date2.Date)),

                        Date_db = (ddltype.SelectedValue == "SB" || ddltype.SelectedValue == "RET") ? (String.IsNullOrEmpty(dt_date.Text) ? default(DateTime?) : Convert.ToDateTime(dt_date.Date)) : (ddltype.SelectedValue == "CDB" || ddltype.SelectedValue == "CCR") ? (String.IsNullOrEmpty(dt_vendor.Text) ? default(DateTime?) : Convert.ToDateTime(dt_vendor.Date)) : (String.IsNullOrEmpty(dt_date2.Text) ? default(DateTime?) : Convert.ToDateTime(dt_date2.Date)),


                        FullBill = txt_fullbill.Text,
                        DueDate = dtdate_Due.Text,
                        DueDate_db = (String.IsNullOrEmpty(dtdate_Due.Text) ? default(DateTime?) : Convert.ToDateTime(dtdate_Due.Date)),
                        RefDate_db = (String.IsNullOrEmpty(dtdate_Ref.Text) ? default(DateTime?) : Convert.ToDateTime(dtdate_Ref.Date)),
                        //DocAmount = (ddltype.SelectedValue == "SB" || ddltype.SelectedValue == "RET") ? txt_docamt.Text : txt_docamt2.Text,
                        //DSAmount = txt_disamt.Text,

                        DocAmount = txt_disamt.Text,
                        // DSAmount = (ddltype.SelectedValue == "SB" || ddltype.SelectedValue == "RET") ? txt_docamt.Text : txt_docamt2.Text,
                        DSAmount = (ddltype.SelectedValue == "SB" || ddltype.SelectedValue == "RET") ? txt_docamt.Text : (ddltype.SelectedValue == "CDB" || ddltype.SelectedValue == "CCR") ? txt_vendor_amt.Text : txt_docamt2.Text,
                        AgentName = cmbContactPerson.Text,
                        AgentId = Convert.ToString(cmbContactPerson.Value),
                        Commpercntag = (ddltype.SelectedValue == "SB" || ddltype.SelectedValue == "RET") ? txt_commprcntg.Text : txt_commprcntg2.Text,
                        CommmAmt = (ddltype.SelectedValue == "SB" || ddltype.SelectedValue == "RET") ? txt_commAmt.Text : txt_commAmt2.Text,
                        User = User
                    });


                    string Customerconsolidate22XML = Consolidatecustomer.ConvertToXml(customerconsolidate, 0);

                    //Project code add Tanmoy
                    CommonBL cbl = new CommonBL();
                    string ProjectSelectcashbankModule = cbl.GetSystemSettingsResult("ProjectSelectInEntryModule");
                    Int64 ProjId = 0;
                    if (lookup_Project.Text != "")
                    {
                        string projectCode = lookup_Project.Text;
                        DataTable dtSlOrd = GetProjectCode(projectCode);
                        //oDbEngine.GetDataTable("select Proj_Id from Master_ProjectManagement where Proj_Code='" + projectCode + "'");
                        ProjId = Convert.ToInt64(dtSlOrd.Rows[0]["Proj_Id"]);
                    }
                    else if (lookup_Project.Text == "")
                    {
                        ProjId = 0;
                    }
                    else
                    {
                        ProjId = 0;
                    }
                    //Project code add Tanmoy

                    string RetAmount = Convert.ToString(crtxtRetAmount.Text);
                    string GL = Convert.ToString(hdnROMainAc.Value);
                    DateTime? dtDuedt = null;

                    if (dtDueDate.Text != null && dtDueDate.Text != "")
                    {
                        dtDuedt = dtDueDate.Date;
                    }


                    int i2 = obj.InsertReplacementDetails(Customerconsolidate22XML, "UpdateCustomerConsolidate", AmountOs, Typeget, CustomerID, ModId, ProjId, RetAmount, GL, dtDuedt);


                    if (i2 > 0)
                    {

                        OpeningGrid.JSProperties["cpSaveSuccessOrFail"] = "Success";
                        OpeningGrid.DataSource = GetConsolidatedCustomerGridData(Request.QueryString["CustomerId"], Request.QueryString["branch"]);
                        OpeningGrid.DataBind();
                    }
                    else
                    {
                        OpeningGrid.JSProperties["cpSaveSuccessOrFail"] = "Mentatory";
                    }
                }
                else
                {

                    OpeningGrid.JSProperties["cpSaveSuccessOrFail"] = "Duplicate";
                }
            }



            else if (WhichCall == "Delete")
            {
                int ModId = Int32.Parse(returnPara.Split('~')[1]);

                int i2 = obj.IDeleteReplacementDetails(ModId, "DeleteCus");

                if (i2 > 0)
                {
                    OpeningGrid.JSProperties["cpSaveSuccessOrFail"] = "Success";
                }

            }


            else if (WhichCall == "Display")
            {
                OpeningGrid.DataSource = GetConsolidatedCustomerGridData(Request.QueryString["CustomerId"], Request.QueryString["branch"]);
                OpeningGrid.DataBind();
            }


            else if (WhichCall == "ClearData")
            {
                txt_Docno.Text = String.Empty;
                dt_date.Text = String.Empty;
                txt_fullbill.Text = String.Empty;
                dtdate_Due.Text = String.Empty;
                txt_docamt.Text = String.Empty;
                dtdate_Ref.Text = String.Empty;

                cmbContactPerson.Text = String.Empty;

                cmbContactPerson.DataSource = null;
                cmbContactPerson.DataBind();

                txt_commAmt.Text = String.Empty;
                txt_commprcntg.Text = String.Empty;
                txt_commAmt.Text = String.Empty;
            }



        }

        protected void grid_DataBinding(object sender, EventArgs e)
        {
            //if (Session["OpeningDatatable"] != null)
            //{
            //    OpeningGrid.DataSource = (DataTable)Session["OpeningDatatable"];
            //}
            OpeningGrid.DataSource = GetConsolidatedCustomerGridData(Request.QueryString["CustomerId"], Request.QueryString["branch"]);

        }

        //For Project Code Tanmoy
        public DataTable GetProjectCode(string Proj_Code)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_SalesOrder_Details");
            proc.AddVarcharPara("@Action", 500, "GetProjectCode");
            proc.AddVarcharPara("@Proj_Code", 200, Proj_Code);
            dt = proc.GetTable();
            return dt;
        }
        //For Project Code Tanmoy
        #endregion


        #region ########## Bind Data Customer wise   #############
        public DataTable GetConsolidatedCustomerGridData(string CustomerId, string branch)
        {
            try
            {

                DataTable dt = obj.GetCustomesconsolidate("CustomerWisebind", CustomerId, Int32.Parse(branch));
                return dt;
            }
            catch
            {
                return null;
            }

        }
        #endregion


        protected void ComponentQuotation_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            if (e.Parameter.Split('~')[0] == "Fetch")
            {
                string s = e.Parameter.Split('~')[1];
                lookup_Customer.Value = s;

            //}

            //if (e.Parameter.Split('~')[0] == "ProjectCode")
            //{
                //Start for Project id selection Tanmoy

                DataTable dtt = GetProjectEditData(hiddnmodid.Value);
                if (dtt != null)
                {
                    ComponentQuotationPanel.JSProperties["cpProjectID"] = dtt.Rows[0]["Proj_Id"].ToString();
                    //lookup_Project.GridView.Selection.SelectRowByKey(Convert.ToInt64(dtt.Rows[0]["Proj_Id"]));
                    ////Tanmoy  Hierarchy
                    //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                    //DataTable dt2 = oDBEngine.GetDataTable("Select Hierarchy_ID from V_ProjectList where Proj_Id='" + Convert.ToInt64(dtt.Rows[0]["Proj_Id"]) + "'");
                    //if (dt2.Rows.Count > 0)
                    //{
                    //    ddlHierarchy.SelectedValue = Convert.ToString(dt2.Rows[0]["Hierarchy_ID"]);
                    //}
                    //Tanmoy  Hierarchy End
                }
                else
                {
                    ComponentQuotationPanel.JSProperties["cpProjectID"] = 0;
                }
                //End for Project id selection Tanmoy

            }
        }

        protected void lookup_Project_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {

        }

        //Start for Project id selection Tanmoy
        public DataTable GetProjectEditData(String modid)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("Proc_Opening_CustomerConsolidate");
            proc.AddVarcharPara("@Action", 500, "ProjectEditdata");
            proc.AddVarcharPara("@Mod", 500, modid);
            dt = proc.GetTable();
            return dt;
        }
        //End for Project id selection Tanmoy

        public void cmbExport2_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(Convert.ToString(ddldetails.SelectedItem.Value));
            if (Filter != 0)
            {


                if (Session["exportval2"] == null)
                {
                    Session["exportval2"] = Filter;
                    // BindDropDownList();
                    bindexport2(Filter);
                }
                else if (Convert.ToInt32(Session["exportval2"]) != Filter)
                {
                    Session["exportval2"] = Filter;
                    // BindDropDownList();
                    bindexport2(Filter);
                }
            }

        }
        public void bindexport2(int Filter)
        {
            //GrdReplacement.Columns[6].Visible = false;
            string filename = "ConsolidatedCustomerDetails";
            exporter.FileName = filename;
            //    exporter.FileName = "SalesRegiserDetailsReport";

            exporter.PageHeader.Left = "Consolidated Customer Details Report";
            exporter.PageFooter.Center = "[Page # of Pages #]";
            exporter.PageFooter.Right = "[Date Printed]";
            exporter.GridViewID = "OpeningGrid";
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

        protected void EntityServerModeDataSalesChallan_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "Proj_Id";

            string cust_id = "0";
            if (lookup_Customer.Value == null)
            {
                //hdnCust_id.Value = lookup_Customer.Value.ToString();
                lookup_Customer.Value = Convert.ToString(hdnCust_id.Value);
            }
            //string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString;
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);

            OpeningEntryDBMLDataContext dc = new OpeningEntryDBMLDataContext(connectionString);

            string Userid = Convert.ToString(HttpContext.Current.Session["userid"]);
            BusinessLogicLayer.DBEngine BEngine = new BusinessLogicLayer.DBEngine();

            var q = from d in dc.V_ProjectLists
                    where d.ProjectStatus == "Approved" && d.ProjBracnchid == Convert.ToInt64(ddl_Branch.SelectedValue)
                    && d.CustId == Convert.ToString(lookup_Customer.Value)
                    orderby d.Proj_Id descending
                    select d;

            e.QueryableSource = q;

        }

        //Tanmoy Hierarchy
        [WebMethod]
        public static String getHierarchyID(string ProjID)
        {
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
            string Hierarchy_ID = "";
            DataTable dt2 = oDBEngine.GetDataTable("Select Hierarchy_ID from V_ProjectList where Proj_Code='" + ProjID + "'");
            if (dt2.Rows.Count > 0)
            {
                Hierarchy_ID = Convert.ToString(dt2.Rows[0]["Hierarchy_ID"]);
                return Hierarchy_ID;
            }
            else
            {
                Hierarchy_ID = "0";
                return Hierarchy_ID;
            }
        }


        public void bindHierarchy()
        {
            DataTable hierarchydt = oDBEngine.GetDataTable("select 0 as ID ,'Select' as H_Name union select ID,H_Name from V_HIERARCHY");
            if (hierarchydt != null && hierarchydt.Rows.Count > 0)
            {
                ddlHierarchy.DataTextField = "H_Name";
                ddlHierarchy.DataValueField = "ID";
                ddlHierarchy.DataSource = hierarchydt;
                ddlHierarchy.DataBind();
            }
        }


        //End Tanmoy Hierarchy



        [WebMethod]
        public static bool CheckUniqueName(string VoucherNo, string Type)
        {
            MShortNameCheckingBL objMShortNameCheckingBL = new MShortNameCheckingBL();
            DataTable dt = new DataTable();
            Boolean status = false;
            BusinessLogicLayer.GenericMethod oGeneric = new BusinessLogicLayer.GenericMethod();

            if (VoucherNo != "" && Convert.ToString(VoucherNo).Trim() != "")
            {
                status = objMShortNameCheckingBL.CheckUnique(Convert.ToString(VoucherNo).Trim(), Type, "JournalVoucher_Check");
            }
            return status;
        }
        [WebMethod]
        public static string getSchemeType(string sel_scheme_id)
        {
            string strschematype = "", strschemalength = "", strschemavalue = "", strschemaBranch = "", Valid_From = "", Valid_Upto = "";

            // BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);

            BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();

            DataTable DT = objEngine.GetDataTable("tbl_master_Idschema", " schema_type,length,Branch,Valid_From,Valid_Upto ", " Id = " + Convert.ToInt32(sel_scheme_id));


            for (int i = 0; i < DT.Rows.Count; i++)
            {
                strschematype = Convert.ToString(DT.Rows[i]["schema_type"]);
                strschemalength = Convert.ToString(DT.Rows[i]["length"]);
                strschemaBranch = Convert.ToString(DT.Rows[i]["Branch"]);
                Valid_From = Convert.ToDateTime(DT.Rows[i]["Valid_From"]).ToString("MM-dd-yyyy");
                Valid_Upto = Convert.ToDateTime(DT.Rows[i]["Valid_Upto"]).ToString("MM-dd-yyyy");

                strschemavalue = strschematype + "~" + strschemalength + "~" + strschemaBranch;
            }

            DataTable dt = GetSelectedStateOfSupply("GetBranchStateCode", strschemaBranch);
            string BranchStateId = Convert.ToString(dt.Rows[0]["StateCode"]);
            strschemavalue = strschemavalue + "~" + BranchStateId + "~" + Valid_From + "~" + Valid_Upto;
            return Convert.ToString(strschemavalue);
        }

        public static DataTable GetSelectedStateOfSupply(string Action, string BranchId)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_JournalVoucherDetails");
            proc.AddVarcharPara("@Action", 500, Action);
            proc.AddVarcharPara("@BranchID", 500, BranchId);
            dt = proc.GetTable();
            return dt;
        }

        [WebMethod]
        public static object SaveRetentionDetails(string invoice_id, string Ret_Amount, string schema_id, string doc_no, string trans_date)
        {

            ProjectInvoiceBL objProjectInvoiceBL = new ProjectInvoiceBL();
            DataTable dtRetention = null;
            string output = "";

            try
            {
                string bill_no = "";
                checkNMakeJVCode(doc_no, Convert.ToInt32(schema_id), DateTime.ParseExact(trans_date, "dd-MM-yyyy", CultureInfo.InvariantCulture), ref bill_no);

                dtRetention = objProjectInvoiceBL.SaveRetentionDetailsOpening(invoice_id, Ret_Amount, schema_id, bill_no, trans_date);

                if (dtRetention != null && dtRetention.Rows.Count > 0)
                {
                    output = Convert.ToString(dtRetention.Rows[0][0]);
                }
                else
                {
                    output = "error";
                }
            }
            catch
            {
                output = "error";
            }






            return Convert.ToString(output);
        }
        protected static string checkNMakeJVCode(string manual_str, int sel_schema_Id, DateTime postingdate, ref string JVNumStr)
        {
            DataTable dtSchema = new DataTable();
            DataTable dtC = new DataTable();
            DBEngine oDBEngine = new DBEngine();
            string prefCompCode = string.Empty, sufxCompCode = string.Empty, startNo, paddedStr, sqlQuery = string.Empty;
            int EmpCode, prefLen, sufxLen, paddCounter;

            if (sel_schema_Id > 0)
            {

                dtSchema = oDBEngine.GetDataTable("tbl_master_idschema", "prefix, suffix, digit, startno, schema_type", "id=" + sel_schema_Id);
                int scheme_type = Convert.ToInt32(dtSchema.Rows[0]["schema_type"]);

                if (scheme_type != 0)
                {
                    startNo = dtSchema.Rows[0]["startno"].ToString();
                    paddCounter = Convert.ToInt32(dtSchema.Rows[0]["digit"]);
                    paddedStr = startNo.PadLeft(paddCounter, '0');
                    prefCompCode = (dtSchema.Rows[0]["prefix"].ToString() == "TCURDATE/") ? (postingdate.ToString("ddMMyyyy") + "/") : (dtSchema.Rows[0]["prefix"].ToString());
                    sufxCompCode = (dtSchema.Rows[0]["suffix"].ToString() == "/TCURDATE") ? ("/" + postingdate.ToString("ddMMyyyy")) : (dtSchema.Rows[0]["suffix"].ToString());
                    prefLen = Convert.ToInt32(prefCompCode.Length);
                    sufxLen = Convert.ToInt32(sufxCompCode.Length);

                    if ((dtSchema.Rows[0]["prefix"].ToString() == "TCURDATE/") || (dtSchema.Rows[0]["suffix"].ToString() == "/TCURDATE"))
                    {
                        sqlQuery = "SELECT max(tjv.JournalVoucher_BillNumber) FROM Trans_JournalVoucher tjv WHERE dbo.RegexMatch('";
                        if (prefLen > 0)
                            sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
                        else if (scheme_type == 2)
                            sqlQuery += "^";
                        sqlQuery += "[0-9]{" + paddCounter + "}";
                        if (sufxLen > 0)
                            sqlQuery += "[\\" + sufxCompCode + "]{" + sufxLen + "}";
                        //sqlQuery += "?$', LTRIM(RTRIM(tjv.JournalVoucher_BillNumber))) = 1";
                        sqlQuery += "?$', LTRIM(RTRIM(tjv.JournalVoucher_BillNumber))) = 1 and JournalVoucher_BillNumber like '%" + sufxCompCode + "'";
                        if (scheme_type == 2)
                            sqlQuery += " AND CONVERT(DATE, JournalVoucher_CreateDateTime) = CONVERT(DATE, GETDATE())";
                        dtC = oDBEngine.GetDataTable(sqlQuery);

                        if (dtC.Rows[0][0].ToString() == "")
                        {
                            sqlQuery = "SELECT max(tjv.JournalVoucher_BillNumber) FROM Trans_JournalVoucher tjv WHERE dbo.RegexMatch('";
                            if (prefLen > 0)
                                sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
                            else if (scheme_type == 2)
                                sqlQuery += "^";
                            sqlQuery += "[0-9]{" + (paddCounter - 1) + "}";
                            if (sufxLen > 0)
                                sqlQuery += "[\\" + sufxCompCode + "]{" + sufxLen + "}";
                            //sqlQuery += "?$', LTRIM(RTRIM(tjv.JournalVoucher_BillNumber))) = 1";
                            sqlQuery += "?$', LTRIM(RTRIM(tjv.JournalVoucher_BillNumber))) = 1 and JournalVoucher_BillNumber like '%" + sufxCompCode + "'";
                            if (scheme_type == 2)
                                sqlQuery += " AND CONVERT(DATE, JournalVoucher_CreateDateTime) = CONVERT(DATE, GETDATE())";
                            dtC = oDBEngine.GetDataTable(sqlQuery);
                        }

                        if (dtC.Rows.Count > 0 && dtC.Rows[0][0].ToString().Trim() != "")
                        {
                            string uccCode = dtC.Rows[0][0].ToString().Trim();
                            int UCCLen = uccCode.Length;
                            int decimalPartLen = UCCLen - (prefCompCode.Length + sufxCompCode.Length);
                            string uccCodeSubstring = uccCode.Substring(prefCompCode.Length, decimalPartLen);
                            EmpCode = Convert.ToInt32(uccCodeSubstring) + 1;
                            // out of range journal scheme
                            if (EmpCode.ToString().Length > paddCounter)
                            {
                                return "outrange";
                            }
                            else
                            {
                                paddedStr = EmpCode.ToString().PadLeft(paddCounter, '0');
                                JVNumStr = prefCompCode + paddedStr + sufxCompCode;
                                return "ok";
                            }
                        }
                        else
                        {
                            JVNumStr = startNo.PadLeft(paddCounter, '0');
                            JVNumStr = prefCompCode + paddedStr + sufxCompCode;
                            return "ok";
                        }
                    }
                    else
                    {
                        sqlQuery = "SELECT max(tjv.JournalVoucher_BillNumber) FROM Trans_JournalVoucher tjv WHERE dbo.RegexMatch('";
                        if (prefLen > 0)
                            sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
                        else if (scheme_type == 2)
                            sqlQuery += "^";
                        sqlQuery += "[0-9]{" + paddCounter + "}";
                        if (sufxLen > 0)
                            sqlQuery += "[" + sufxCompCode + "]{" + sufxLen + "}";
                        //sqlQuery += "?$', LTRIM(RTRIM(tjv.JournalVoucher_BillNumber))) = 1";
                        sqlQuery += "?$', LTRIM(RTRIM(tjv.JournalVoucher_BillNumber))) = 1 and JournalVoucher_BillNumber like '" + prefCompCode + "%'";
                        if (scheme_type == 2)
                            sqlQuery += " AND CONVERT(DATE, JournalVoucher_CreateDateTime) = CONVERT(DATE, GETDATE())";
                        dtC = oDBEngine.GetDataTable(sqlQuery);

                        if (dtC.Rows[0][0].ToString() == "")
                        {
                            sqlQuery = "SELECT max(tjv.JournalVoucher_BillNumber) FROM Trans_JournalVoucher tjv WHERE dbo.RegexMatch('";
                            if (prefLen > 0)
                                sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
                            else if (scheme_type == 2)
                                sqlQuery += "^";
                            sqlQuery += "[0-9]{" + (paddCounter - 1) + "}";
                            if (sufxLen > 0)
                                sqlQuery += "[" + sufxCompCode + "]{" + sufxLen + "}";
                            //sqlQuery += "?$', LTRIM(RTRIM(tjv.JournalVoucher_BillNumber))) = 1";
                            sqlQuery += "?$', LTRIM(RTRIM(tjv.JournalVoucher_BillNumber))) = 1 and JournalVoucher_BillNumber like '" + prefCompCode + "%'";
                            if (scheme_type == 2)
                                sqlQuery += " AND CONVERT(DATE, JournalVoucher_CreateDateTime) = CONVERT(DATE, GETDATE())";
                            dtC = oDBEngine.GetDataTable(sqlQuery);
                        }

                        if (dtC.Rows.Count > 0 && dtC.Rows[0][0].ToString().Trim() != "")
                        {
                            string uccCode = dtC.Rows[0][0].ToString().Trim();
                            int UCCLen = uccCode.Length;
                            int decimalPartLen = UCCLen - (prefCompCode.Length + sufxCompCode.Length);
                            string uccCodeSubstring = uccCode.Substring(prefCompCode.Length, decimalPartLen);
                            EmpCode = Convert.ToInt32(uccCodeSubstring) + 1;
                            // out of range journal scheme
                            if (EmpCode.ToString().Length > paddCounter)
                            {
                                return "outrange";
                            }
                            else
                            {
                                paddedStr = EmpCode.ToString().PadLeft(paddCounter, '0');
                                JVNumStr = prefCompCode + paddedStr + sufxCompCode;
                                return "ok";
                            }
                        }
                        else
                        {
                            JVNumStr = startNo.PadLeft(paddCounter, '0');
                            JVNumStr = prefCompCode + paddedStr + sufxCompCode;
                            return "ok";
                        }
                    }
                }
                else
                {
                    sqlQuery = "SELECT JournalVoucher_BillNumber FROM Trans_JournalVoucher WHERE JournalVoucher_BillNumber LIKE '" + manual_str.Trim() + "'";
                    dtC = oDBEngine.GetDataTable(sqlQuery);
                    // duplicate manual entry check
                    if (dtC.Rows.Count > 0 && dtC.Rows[0][0].ToString().Trim() != "")
                    {
                        return "duplicate";
                    }

                    JVNumStr = manual_str.Trim();
                    return "ok";
                }
            }
            else
            {
                return "noid";
            }
        }


    }
}