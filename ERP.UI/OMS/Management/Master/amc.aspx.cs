using System;
using System.Data;
using System.Web;
using System.Web.UI;
//////using DevExpress.Web.ASPxClasses;
//////using DevExpress.Web;
//using DevExpress.Web;
////using DevExpress.Web.ASPxTabControl;
using DevExpress.Web;
using BusinessLogicLayer;
using System.Configuration;

namespace ERP.OMS.Management.Master
{
    public partial class management_master_amc : ERP.OMS.ViewState_class.VSPage
    {
        public string pageAccess = "";
        //DBEngine oDBEngine = new DBEngine(string.Empty);
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        protected void Page_PreInit(object sender, EventArgs e)
        {
            //------- For Read Only User in SQL Datasource Connection String   Start-----------------

            if (HttpContext.Current.Session["EntryProfileType"] != null)
            {
                if (Convert.ToString(HttpContext.Current.Session["EntryProfileType"]) == "R")
                {
                    //amc.ConnectionString = ConfigurationSettings.AppSettings["DBReadOnlyConnection"];
                    //amcAddress.ConnectionString = ConfigurationSettings.AppSettings["DBReadOnlyConnection"];
                    //emcPhone.ConnectionString = ConfigurationSettings.AppSettings["DBReadOnlyConnection"];
                    //emcEmail.ConnectionString = ConfigurationSettings.AppSettings["DBReadOnlyConnection"];
                    amc.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                    amcAddress.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                    emcPhone.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                    emcEmail.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);

                }
                else
                {
                    //amc.ConnectionString = ConfigurationSettings.AppSettings["DBConnectionDefault"];
                    //amcAddress.ConnectionString = ConfigurationSettings.AppSettings["DBConnectionDefault"];
                    //emcPhone.ConnectionString = ConfigurationSettings.AppSettings["DBConnectionDefault"];
                    //emcEmail.ConnectionString = ConfigurationSettings.AppSettings["DBConnectionDefault"];
                    amc.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                    amcAddress.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                    emcPhone.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                    emcEmail.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);


                }
            }

            //------- For Read Only User in SQL Datasource Connection String   End-----------------

            if (!IsPostBack)
            {
                //'http://localhost:2957/InfluxCRM/management/testProjectMainPage_employee.aspx'
                string sPath = HttpContext.Current.Request.Url.ToString();
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            CountrySelect.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            StateSelect.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            SelectCity.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            if (HttpContext.Current.Session["userid"] == null)
            {
                //Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            ////this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");


        }
        protected void ASPxGridView1_BeforePerformDataSelect(object sender, EventArgs e)
        {
            Session["KeyVal_InternalID"] = (sender as ASPxGridView).GetMasterRowKeyValue();
        }

        protected void btnSearch(object sender, EventArgs e)
        {
            amcGrid.Settings.ShowFilterRow = true;
        }
        protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = Int32.Parse(cmbExport.SelectedItem.Value.ToString());
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
        protected void AddressGrid_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {

            ASPxGridView AddressGrid = (ASPxGridView)sender;
            if (e.Column.FieldName == "State")
            {
                if (e.KeyValue != null)
                {
                    object val = new object();
                    val = AddressGrid.GetRowValuesByKeyValue(e.KeyValue, "Country");
                    if (val == DBNull.Value) return;
                    Int32 country = Convert.ToInt32(val);
                    ASPxComboBox combo = e.Editor as ASPxComboBox;
                    FillStateCombo(combo, country);
                    combo.Callback += new CallbackEventHandlerBase(cmbState_OnCallback);
                }
                else
                {
                    object val = new object();
                    val = AddressGrid.GetRowValues(0, "Country");
                    if (val == null)
                    {
                        Int32 country = Convert.ToInt32(val);
                        ASPxComboBox combo = e.Editor as ASPxComboBox;
                        FillStateCombo(combo, country);
                        combo.Callback += new CallbackEventHandlerBase(cmbState_OnCallback);
                    }
                    else
                    {
                        Int32 country = 1;
                        ASPxComboBox combo = e.Editor as ASPxComboBox;
                        FillStateCombo(combo, country);
                        combo.Callback += new CallbackEventHandlerBase(cmbState_OnCallback);
                    }
                }
            }
            ///////////////////
            if (e.Column.FieldName == "City")
            {
                if (e.KeyValue != null)
                {
                    object val = new object();
                    val = AddressGrid.GetRowValuesByKeyValue(e.KeyValue, "State");
                    if (val == DBNull.Value) return;
                    Int32 state = Convert.ToInt32(val);
                    ASPxComboBox combo = e.Editor as ASPxComboBox;
                    FillCityCombo(combo, state);
                    combo.Callback += new CallbackEventHandlerBase(cmbCity_OnCallback);
                }
                else
                {
                    object val = new object();
                    val = AddressGrid.GetRowValues(0, "State");
                    if (val == null)
                    {
                        Int32 state = Convert.ToInt32(val);
                        ASPxComboBox combo = e.Editor as ASPxComboBox;
                        FillCityCombo(combo, state);
                        combo.Callback += new CallbackEventHandlerBase(cmbCity_OnCallback);
                    }
                    else
                    {
                        Int32 state = 1;
                        ASPxComboBox combo = e.Editor as ASPxComboBox;
                        FillCityCombo(combo, state);
                        combo.Callback += new CallbackEventHandlerBase(cmbCity_OnCallback);
                    }
                }
            }

        }
        protected void FillStateCombo(ASPxComboBox cmb, int country)
        {

            string[,] state = GetState(country);
            cmb.Items.Clear();

            for (int i = 0; i < state.GetLength(0); i++)
            {
                cmb.Items.Add(state[i, 1], state[i, 0]);
            }
        }

        string[,] GetState(int country)
        {
            StateSelect.SelectParameters[0].DefaultValue = country.ToString();
            DataView view = (DataView)StateSelect.Select(DataSourceSelectArguments.Empty);
            string[,] DATA = new string[view.Count, 2];
            for (int i = 0; i < view.Count; i++)
            {
                DATA[i, 0] = view[i][0].ToString();
                DATA[i, 1] = view[i][1].ToString();
            }
            return DATA;
        }
        protected void FillCityCombo(ASPxComboBox cmb, int state)
        {

            string[,] cities = GetCities(state);
            cmb.Items.Clear();

            for (int i = 0; i < cities.GetLength(0); i++)
            {
                cmb.Items.Add(cities[i, 1], cities[i, 0]);
            }
        }

        string[,] GetCities(int state)
        {
            SelectCity.SelectParameters[0].DefaultValue = state.ToString();
            DataView view = (DataView)SelectCity.Select(DataSourceSelectArguments.Empty);
            string[,] DATA = new string[view.Count, 2];
            for (int i = 0; i < view.Count; i++)
            {
                DATA[i, 0] = view[i][0].ToString();
                DATA[i, 1] = view[i][1].ToString();
            }
            return DATA;
        }
        private void cmbState_OnCallback(object source, CallbackEventArgsBase e)
        {
            FillStateCombo(source as ASPxComboBox, Convert.ToInt32(e.Parameter));
        }
        private void cmbCity_OnCallback(object source, CallbackEventArgsBase e)
        {
            FillCityCombo(source as ASPxComboBox, Convert.ToInt32(e.Parameter));
        }
        protected void amcGrid_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType == GridViewRowType.Data)
            {
                int commandColumnIndex = -1;
                for (int i = 0; i < amcGrid.Columns.Count; i++)
                    if (amcGrid.Columns[i] is GridViewCommandColumn)
                    {
                        commandColumnIndex = i;
                        break;
                    }
                if (commandColumnIndex == -1)
                    return;
                //____One colum has been hided so index of command column will be leass by 1 
                commandColumnIndex = commandColumnIndex - 10;
                DevExpress.Web.Rendering.GridViewTableCommandCell cell = e.Row.Cells[commandColumnIndex] as DevExpress.Web.Rendering.GridViewTableCommandCell;
                for (int i = 0; i < cell.Controls.Count; i++)
                {
                    DevExpress.Web.Rendering.GridCommandButtonsCell button = cell.Controls[i] as DevExpress.Web.Rendering.GridCommandButtonsCell;
                    if (button == null) return;
                    DevExpress.Web.Internal.InternalHyperLink hyperlink = button.Controls[0] as DevExpress.Web.Internal.InternalHyperLink;

                    if (hyperlink.Text == "Delete")
                    {
                        if (Session["PageAccess"].ToString() == "DelAdd" || Session["PageAccess"].ToString() == "Delete" || Session["PageAccess"].ToString() == "All")
                        {
                            hyperlink.Enabled = true;
                            continue;
                        }
                        else
                        {
                            hyperlink.Enabled = false;
                            continue;
                        }
                    }


                }

            }

        }
        protected void amcGrid_HtmlEditFormCreated(object sender, ASPxGridViewEditFormEventArgs e)
        {
            if (!amcGrid.IsNewRowEditing)
            {
                ASPxPageControl RT1 = amcGrid.FindEditFormTemplateControl("pageControl") as ASPxPageControl;
                ASPxGridViewTemplateReplacement RT = RT1.FindControl("UpdateButton") as ASPxGridViewTemplateReplacement;
                if (Session["PageAccess"].ToString().Trim() == "Add" || Session["PageAccess"].ToString().Trim() == "Modify" || Session["PageAccess"].ToString().Trim() == "All")
                    RT.Visible = true;
                else
                    RT.Visible = false;
            }
        }
        protected void amcGrid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            if (e.Parameters == "s")
                amcGrid.Settings.ShowFilterRow = true;

            if (e.Parameters == "All")
            {
                amcGrid.FilterExpression = string.Empty;
            }
        }
        protected void EmailGrid_RowValidating(object sender, DevExpress.Web.Data.ASPxDataValidationEventArgs e)
        {
            ASPxPageControl tab = amcGrid.FindEditFormTemplateControl("pageControl") as ASPxPageControl;
            ASPxPageControl tab1 = tab.FindControl("ASPxPageControl2") as ASPxPageControl;
            ASPxGridView EmailAdd = tab1.FindControl("EmailGrid") as ASPxGridView;
            string ccEmail = "";
            string email = "";
            try
            {
                email = e.NewValues["eml_email"].ToString();
            }
            catch
            {
            }
            try
            {
                ccEmail = e.NewValues["eml_ccEmail"].ToString();
            }
            catch
            {
            }
            if (!EmailAdd.IsNewRowEditing)
            {
                string emailID = e.Keys[0].ToString();
                string[,] emailCheck = oDBEngine.GetFieldValue("tbl_master_email", "eml_email", " eml_email='" + email + "' and eml_id<>'" + emailID + "'", 1);
                string email1 = "";
                if (emailCheck[0, 0] != "n")
                {
                    email1 = emailCheck[0, 0];
                }
                string[,] ccEmailCheck = oDBEngine.GetFieldValue("tbl_master_email", "eml_ccEmail", " eml_ccEmail='" + ccEmail + "' and eml_id<>'" + emailID + "'", 1);
                string ccEmail1 = "";
                if (ccEmailCheck[0, 0] != "n")
                {
                    ccEmail1 = ccEmailCheck[0, 0];
                }
                if (email1 == "" && ccEmail1 == "")
                {

                }
                else
                {
                    e.RowError = "Email Id Already Exists";
                    return;
                }
            }
            else
            {
                string[,] emailCheck = oDBEngine.GetFieldValue("tbl_master_email", "eml_email", " eml_email='" + email + "'", 1);
                string email1 = "";
                if (emailCheck[0, 0] != "n")
                {
                    email1 = emailCheck[0, 0];
                }
                string[,] ccEmailCheck = oDBEngine.GetFieldValue("tbl_master_email", "eml_ccEmail", " eml_ccEmail='" + ccEmail + "'", 1);
                string ccEmail1 = "";
                if (ccEmailCheck[0, 0] != "n")
                {
                    ccEmail1 = ccEmailCheck[0, 0];
                }
                if (email1 == "" && ccEmail1 == "")
                {

                }
                else
                {
                    e.RowError = "Email Id Already Exists";
                    return;
                }
            }
        }

        protected void PhoneGrid_RowValidating(object sender, DevExpress.Web.Data.ASPxDataValidationEventArgs e)
        {
            ASPxPageControl tab = amcGrid.FindEditFormTemplateControl("pageControl") as ASPxPageControl;
            ASPxPageControl tab1 = tab.FindControl("ASPxPageControl2") as ASPxPageControl;
            ASPxGridView PhoneGrid = tab1.FindControl("PhoneGrid") as ASPxGridView;
            string PhoneType = e.NewValues["phf_type"].ToString();
            if (PhoneType == "Mobile")
            {
                string PhoneNumber = e.NewValues["phf_phoneNumber"].ToString();
                if (PhoneNumber.Length != 10)
                {
                    e.RowError = "Enter Valid Mobile Number";
                    return;
                }
            }
        }
    }
}