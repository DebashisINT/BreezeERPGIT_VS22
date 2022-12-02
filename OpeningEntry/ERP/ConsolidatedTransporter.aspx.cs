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
using OpeningBusinessLogic.Transporterconsolidate;
using OpeningBusinessLogic;
using System.Xml;
using System.Reflection;
using OpeningEntry.OpeningEntry.DBML;
using BusinessLogicLayer;
using DataAccessLayer;


namespace OpeningEntry.ERP
{
    public partial class ConsolidatedTransporter : System.Web.UI.Page
    {
        CommonBL ComBL = new CommonBL();
        DataTable dst = new DataTable();
        string strBranchID = "";
        TransporterConsolidate obj = new TransporterConsolidate();
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]); MULTI
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
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/ERP/ConsolidatedTransportedList.aspx");
          
            string ProjectSelectInEntryModule = ComBL.GetSystemSettingsResult("ProjectSelectInEntryModule");
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
            //For Hierarchy Start 
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
            //For Hierarchy End 
            string ProjectMandatoryInEntry = ComBL.GetSystemSettingsResult("ProjectMandatoryInEntry");
            if (!String.IsNullOrEmpty(ProjectMandatoryInEntry))
            {
                if (ProjectMandatoryInEntry == "Yes")
                {
                    hdnProjectMandatory.Value = "1";
                }
                else if (ProjectMandatoryInEntry.ToUpper().Trim() == "NO")
                {
                    hdnProjectMandatory.Value = "0";
                }
            }
            if (!IsPostBack)
            {
                bindHierarchy();
                ddlHierarchy.Enabled = false;
                Session["exportval2"] = null;
                Branchpopulate();
                strBranchID = Convert.ToString(Session["userbranchID"]);
                DataTable dtsale = obj.GetCustomersalesFinancialyear(Convert.ToString(Session["LastFinYear"]));
                if (dtsale.Rows.Count > 0)
                {
                    dt_date.MaxDate = Convert.ToDateTime(Convert.ToString(dtsale.Rows[0]["FinYear_StartDate"])).AddDays(-1);
                    dt_date2.MaxDate = Convert.ToDateTime(Convert.ToString(dtsale.Rows[0]["FinYear_StartDate"])).AddDays(-1);
                  //  dt_vendor.MaxDate = Convert.ToDateTime(Convert.ToString(dtsale.Rows[0]["FinYear_StartDate"])).AddDays(-1);
                }
                if (Request.QueryString["TransporterId"] != null)
                {
                    OpeningGrid.Visible = true;
                    //divgrid.Visible = true;
                    hdncus.Value = "1";
                    //hiddnmodid.Value = Request.QueryString["TransporterId"];
                    hiddnmodid.Value = "0";
                    ddl_Branch.Enabled = false;
                    lookup_Customer.ReadOnly = true;
                    ddltype.Enabled = false;
                    OpeningGrid.DataSource = GetConsolidatedCustomerGridData(Request.QueryString["TransporterId"], Request.QueryString["branch"]);
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

                if (Request.QueryString["branch"] != null)
                {
                    ddl_Branch.SelectedValue = Request.QueryString["branch"];
                    Cache["name"] = ddl_Branch.SelectedValue;
                }
                else if (Session["userbranchID"] != null)
                {
                    ddl_Branch.SelectedValue = userbranchID;
                }
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

            }
        }
        protected void PopulateContactPersonOfCustomer(string InternalId)
        {
            //string ContactPerson = "";
            //DataTable dtContactPerson = new DataTable();

            //dtContactPerson = obj.PopulateContactPersonOfCustomer(InternalId);
            //if (dtContactPerson != null && dtContactPerson.Rows.Count > 0)
            //{
            //    cmbContactPerson.TextField = "contactperson";
            //    cmbContactPerson.ValueField = "add_id";
            //    cmbContactPerson.DataSource = dtContactPerson;
            //    cmbContactPerson.DataBind();
            //    foreach (DataRow dr in dtContactPerson.Rows)
            //    {
            //        if (Convert.ToString(dr["Isdefault"]) == "True")
            //        {
            //            ContactPerson = Convert.ToString(dr["add_id"]);
            //            break;
            //        }
            //    }
            //    cmbContactPerson.SelectedItem = cmbContactPerson.Items.FindByValue(ContactPerson);
            //}
        }

        #endregion




        #region  ###### Insert  Update  DATA  through Grid Perform Call Back  ##############
        protected void OpeningGrid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            ASPxGridView grid = lookup_Customer.GridView;
            object cuname = grid.GetRowValues(grid.FocusedRowIndex, new string[] { "Name" });
            List<ConsolidatedTransporterClass> customerconsolidate = new List<ConsolidatedTransporterClass>();
            string returnPara = Convert.ToString(e.Parameters);
            string FinYear = String.Empty;
            string User = String.Empty;
            string Company = String.Empty;           
            DataTable dttab;
            string WhichCall = returnPara.Split('~')[0];

          //  string AmountOs = (ddltype.SelectedValue == "PB" || ddltype.SelectedValue == "RET") ? txt_docamt.Text : txt_docamt2.Text;

            string AmountOs = (ddltype.SelectedValue == "PB" || ddltype.SelectedValue == "RET") ? txt_docamt.Text : (ddltype.SelectedValue == "CDB" || ddltype.SelectedValue == "CCR") ? txt_vendor_amt.Text : txt_docamt2.Text;
            string Typeget = ddltype.SelectedValue;
            string CustomerID = Convert.ToString(lookup_Customer.Value);
            if (WhichCall == "TemporaryData")
            {
                Cache["name_vendor"] = ddl_Branch.SelectedValue;
                DataTable dt = new DataTable();
                string docnumber = "";
                docnumber = (ddltype.SelectedValue == "PB" || ddltype.SelectedValue == "RET") ? txt_Docno.Text : txt_Docno2.Text;
                dt = obj.GetDuplicateDoc("DuplicateDoccheck", docnumber, "");
                if (Convert.ToString(dt.Rows[0][0]) == "0" && (ddltype.SelectedValue != "CDB" || ddltype.SelectedValue != "CCR"))
                {
                    if (Session["LastFinYear"] != null)
                    {
                        if (ddltype.SelectedValue == "PB" || ddltype.SelectedValue == "RET")
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


                   
                    customerconsolidate.Add(new ConsolidatedTransporterClass()
                    {

                        Branch = ddl_Branch.SelectedItem.Text,
                        BranchId = ddl_Branch.SelectedValue,
                        CustomerId = Convert.ToString(lookup_Customer.Value),
                        Customer = cuname.ToString(),
                        Type = ddltype.SelectedItem.Text,
                        TypeId = ddltype.SelectedValue,
                        DocNumber = (ddltype.SelectedValue == "PB" || ddltype.SelectedValue == "RET") ? txt_Docno.Text : txt_Docno2.Text,
                        Date = dt_date.Text,
                        //Date_db = (ddltype.SelectedValue == "PB" || ddltype.SelectedValue == "RET") ? (String.IsNullOrEmpty(dt_date.Text) ? default(DateTime?) : Convert.ToDateTime(dt_date.Date)) : (String.IsNullOrEmpty(dt_date2.Text) ? default(DateTime?) : Convert.ToDateTime(dt_date2.Date)),
                        Date_db = (ddltype.SelectedValue == "PB" || ddltype.SelectedValue == "RET") ? (String.IsNullOrEmpty(dt_date.Text) ? default(DateTime?) : Convert.ToDateTime(dt_date.Date)) : (ddltype.SelectedValue == "CDB" || ddltype.SelectedValue == "CCR") ? (String.IsNullOrEmpty(dt_vendor.Text) ? default(DateTime?) : Convert.ToDateTime(dt_vendor.Date)) : (String.IsNullOrEmpty(dt_date2.Text) ? default(DateTime?) : Convert.ToDateTime(dt_date2.Date)),

                        FullBill = txt_fullbill.Text,
                        DueDate = dtdate_Due.Text,
                        DueDate_db = (String.IsNullOrEmpty(dtdate_Due.Text) ? default(DateTime?) : Convert.ToDateTime(dtdate_Due.Date)),
                        RefDate_db = (String.IsNullOrEmpty(dtdate_Ref.Text) ? default(DateTime?) : Convert.ToDateTime(dtdate_Ref.Date)),
                        DocAmount = txt_disamt.Text,
                        DSAmount = (ddltype.SelectedValue == "PB" || ddltype.SelectedValue == "RET") ? txt_docamt.Text : (ddltype.SelectedValue == "CDB" || ddltype.SelectedValue == "CCR") ? txt_vendor_amt.Text : txt_docamt2.Text,
                        AgentName = "",
                        AgentId = "",
                        Commpercntag = (ddltype.SelectedValue == "PB" || ddltype.SelectedValue == "RET") ? txt_commprcntg.Text : txt_commprcntg2.Text,
                        CommmAmt = (ddltype.SelectedValue == "PB" || ddltype.SelectedValue == "RET") ? txt_commAmt.Text : txt_commAmt2.Text,
                        Company = Company,
                        FinYear = FinYear,
                        User = User
                    });

                    CommonBL cbl = new CommonBL();
                    string ProjectSelectcashbankModule = cbl.GetSystemSettingsResult("ProjectSelectInEntryModule");
                    Int64 ProjId = 0;
                    if (lookup_Project.Text != "")
                    {
                        string projectCode = lookup_Project.Text;
                        DataTable dtSlOrd = GetProjectCode(projectCode);                       
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

                    string CustomerconsolidateXML = ConvertToXml(customerconsolidate, 0);

                    int i2 = obj.InsertReplacementDetails(CustomerconsolidateXML, "InserTransporterConsolidate", AmountOs, Typeget, CustomerID, 0, ProjId);


                    if (i2 > 0)
                    {
                        OpeningGrid.JSProperties["cpSaveSuccessOrFail"] = "Success";
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
                // Session["GetData"] = customerconsolidate;
            }


            if (WhichCall == "ModifyData")
            {

                Cache["name_vendor"] = ddl_Branch.SelectedValue;
                DataTable dt = new DataTable();
                int ModId = Int32.Parse(returnPara.Split('~')[1]);
                string docnumber = (ddltype.SelectedValue == "PB" || ddltype.SelectedValue == "RET") ? txt_Docno.Text : txt_Docno2.Text;
                dt = obj.GetDuplicateDoc("DuplicateDoccheck", docnumber, Convert.ToString(ModId));
                if (Convert.ToString(dt.Rows[0][0]) == "0")
                {
                    customerconsolidate.Add(new ConsolidatedTransporterClass()
                    {
                        ModId = ModId,
                        Branch = ddl_Branch.SelectedItem.Text,
                        BranchId = ddl_Branch.SelectedValue,
                        CustomerId = Convert.ToString(lookup_Customer.Value),
                        Customer = Convert.ToString(cuname),
                        Type = ddltype.SelectedItem.Text,
                        TypeId = ddltype.SelectedValue,
                        DocNumber = (ddltype.SelectedValue == "PB" || ddltype.SelectedValue == "RET") ? txt_Docno.Text : txt_Docno2.Text,
                        Date = dt_date.Text,
                      //  Date_db = (ddltype.SelectedValue == "PB" || ddltype.SelectedValue == "RET") ? (String.IsNullOrEmpty(dt_date.Text) ? default(DateTime?) : Convert.ToDateTime(dt_date.Date)) : (String.IsNullOrEmpty(dt_date2.Text) ? default(DateTime?) : Convert.ToDateTime(dt_date2.Date)),
                        Date_db = (ddltype.SelectedValue == "PB" || ddltype.SelectedValue == "RET") ? (String.IsNullOrEmpty(dt_date.Text) ? default(DateTime?) : Convert.ToDateTime(dt_date.Date)) : (ddltype.SelectedValue == "CDB" || ddltype.SelectedValue == "CCR") ? (String.IsNullOrEmpty(dt_vendor.Text) ? default(DateTime?) : Convert.ToDateTime(dt_vendor.Date)) : (String.IsNullOrEmpty(dt_date2.Text) ? default(DateTime?) : Convert.ToDateTime(dt_date2.Date)),

                        FullBill = txt_fullbill.Text,
                        DueDate = dtdate_Due.Text,
                        DueDate_db = (String.IsNullOrEmpty(dtdate_Due.Text) ? default(DateTime?) : Convert.ToDateTime(dtdate_Due.Date)),
                        RefDate_db = (String.IsNullOrEmpty(dtdate_Ref.Text) ? default(DateTime?) : Convert.ToDateTime(dtdate_Ref.Date)),
                        DocAmount = txt_disamt.Text,
                       // DSAmount = (ddltype.SelectedValue == "PB" || ddltype.SelectedValue == "RET") ? txt_docamt.Text : txt_docamt2.Text,

                        DSAmount = (ddltype.SelectedValue == "PB" || ddltype.SelectedValue == "RET") ? txt_docamt.Text : (ddltype.SelectedValue == "CDB" || ddltype.SelectedValue == "CCR") ? txt_vendor_amt.Text : txt_docamt2.Text,
                        AgentName = "",
                        AgentId = "",
                        Commpercntag = (ddltype.SelectedValue == "PB" || ddltype.SelectedValue == "RET") ? txt_commprcntg.Text : txt_commprcntg2.Text,
                        CommmAmt = (ddltype.SelectedValue == "PB" || ddltype.SelectedValue == "RET") ? txt_commAmt.Text : txt_commAmt2.Text,

                    });

                  
                    Int64 ProjId = 0;
                    if (lookup_Project.Text != "")
                    {
                        string projectCode = lookup_Project.Text;
                        DataTable dtSlOrd = GetProjectCode(projectCode);                      
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

                    string Customerconsolidate22XML = ConvertToXml(customerconsolidate, 0);

                    int i2 = obj.InsertReplacementDetails(Customerconsolidate22XML, "UpdateCustomerTransporter", AmountOs, Typeget, CustomerID, ModId, ProjId);


                    if (i2 > 0)
                    {

                        OpeningGrid.JSProperties["cpSaveSuccessOrFail"] = "Success";
                        OpeningGrid.DataSource = GetConsolidatedCustomerGridData(Request.QueryString["TransporterId"], Request.QueryString["branch"]);
                        OpeningGrid.DataBind();
                    }
                    else if (i2 ==-12)
                    {
                        OpeningGrid.JSProperties["cpSaveSuccessOrFail"] = "Check";
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
                OpeningGrid.DataSource = GetConsolidatedCustomerGridData(Request.QueryString["TransporterId"], Request.QueryString["branch"]);
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

            OpeningGrid.DataSource = GetConsolidatedCustomerGridData(Request.QueryString["TransporterId"], Request.QueryString["branch"]);
            //   OpeningGrid.DataBind();
        }

        #endregion



        #region ########## Bind Data Customer wise   #############
        public DataTable GetConsolidatedCustomerGridData(string CustomerId, string branch)
        {
            try
            {

                DataTable dt = obj.GetCustomesconsolidate("TransporterWisebind", CustomerId, Int32.Parse(branch));
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
                DataTable dt = obj.GetVendorpopulateBranch(Int32.Parse(ddl_Branch.SelectedValue));
                if (dt.Rows.Count > 0)
                {
                    lookup_Customer.DataSource = dt;
                    lookup_Customer.DataBind();
                }
                string s = e.Parameter.Split('~')[1];
                lookup_Customer.Value = s;

                DataTable dtt = GetProjectEditData(hiddnmodid.Value);
                if (dtt != null)
                {
                    ComponentQuotationPanel.JSProperties["cpProjectID"] = dtt.Rows[0]["Proj_Id"].ToString();
                    ComponentQuotationPanel.JSProperties["cpHierarchy_ID"] = dtt.Rows[0]["Hierarchy_ID"].ToString();
                }
                else
                {
                    ComponentQuotationPanel.JSProperties["cpProjectID"] = 0;
                }
            }
        }


        //protected void lookup_branch_DataBinding(object sender, EventArgs e)
        //{
        //    if (Session["SI_ComponentData_Branch"] != null)
        //    {
        //        lookup_Customer.DataSource = obj.GetVendorpopulateBranch(Int32.Parse(ddl_Branch.SelectedValue));;
        //    }
        //}


        protected void lookup_vendor_DataBinding(object sender, EventArgs e)
        {

            lookup_Customer.DataSource = obj.GetVendorpopulateBranch(Int32.Parse(ddl_Branch.SelectedValue));

        }

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
            string filename = "TransporterDetails";
            exporter.FileName = filename;
            //    exporter.FileName = "SalesRegiserDetailsReport";

            exporter.PageHeader.Left = "Consolidated Transporter Details Report";
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

        public static string ConvertToXml<T>(List<T> table, int metaIndex = 0)
        {
            XmlDocument ChoiceXML = new XmlDocument();
            ChoiceXML.AppendChild(ChoiceXML.CreateElement("root"));
            Type temp = typeof(T);

            foreach (var item in table)
            {
                XmlElement element = ChoiceXML.CreateElement("data");

                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    element.AppendChild(ChoiceXML.CreateElement(pro.Name)).InnerText = Convert.ToString(item.GetType().GetProperty(pro.Name).GetValue(item, null));
                }
                ChoiceXML.DocumentElement.AppendChild(element);
            }

            return ChoiceXML.InnerXml.ToString();
        }


        protected void EntityServerModeDataSalesChallan_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "Proj_Id";
            string cust_id = "0";
            //if (lookup_Customer.Value == null)
            //{                
            //    lookup_Customer.Value = Convert.ToString(hdnCust_id.Value);
            //}
           
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            OpeningEntryDBMLDataContext dc = new OpeningEntryDBMLDataContext(connectionString);
            string Userid = Convert.ToString(HttpContext.Current.Session["userid"]);
            BusinessLogicLayer.DBEngine BEngine = new BusinessLogicLayer.DBEngine();
            var q = from d in dc.V_ProjectLists
                    where d.ProjectStatus == "Approved" && d.ProjBracnchid == Convert.ToInt64(ddl_Branch.SelectedValue)
                    //&& d.CustId == Convert.ToString(lookup_Customer.Value)
                    orderby d.Proj_Id descending
                    select d;
            e.QueryableSource = q;
        }        
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
        public DataTable GetProjectCode(string Proj_Code)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_SalesOrder_Details");
            proc.AddVarcharPara("@Action", 500, "GetProjectCode");
            proc.AddVarcharPara("@Proj_Code", 200, Proj_Code);
            dt = proc.GetTable();
            return dt;
        }
        public DataTable GetProjectEditData(String modid)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("Proc_Opening_TransporterConsolidate");
            proc.AddVarcharPara("@Action", 500, "ProjectEditdata");
            proc.AddVarcharPara("@Mod", 500, modid);
            dt = proc.GetTable();
            return dt;
        }
    }
}