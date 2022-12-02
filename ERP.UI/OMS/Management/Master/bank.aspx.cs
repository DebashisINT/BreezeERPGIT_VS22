using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using BusinessLogicLayer;
using EntityLayer.CommonELS;

namespace ERP.OMS.Management.Master
{
    public partial class management_master_bank : ERP.OMS.ViewState_class.VSPage
    {
        public string pageAccess = "";
        String bankid = "B";
        // DBEngine oDBEngine = new DBEngine(string.Empty);
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        public EntityLayer.CommonELS.UserRightsForPage  rights = new UserRightsForPage();

        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //'http://localhost:2957/InfluxCRM/management/testProjectMainPage_employee.aspx'
                string sPath = Convert.ToString(HttpContext.Current.Request.Url);
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Master/bank.aspx");
            BankDataSource.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            BankAddress.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            BankPhone.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            BankEmail.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            CountrySelect.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            StateSelect.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            SelectCity.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);

            if (HttpContext.Current.Session["userid"] == null)
            {
                Response.Redirect("~/Login.aspx");
            }
            //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");
            if(!IsPostBack)
            {
                Session["exportval"] = null;
            }
        }
        protected void AddressGrid_BeforePerformDataSelect(object sender, EventArgs e)
        {
            Session["KeyVal_InternalID"] = (sender as ASPxGridView).GetMasterRowKeyValue();
        }

        protected void btnSearch(object sender, EventArgs e)
        {
            BankGrid.Settings.ShowFilterRow = true;
        }
        public void bindexport(int Filter)
        {

            BankGrid.Columns[8].Visible = false;
            //MainAccountGrid.Columns[20].Visible = false;
            // MainAccountGrid.Columns[21].Visible = false;
            string filename = "Banks";
            exporter.FileName = filename;

            exporter.PageHeader.Left = "Banks";
            exporter.PageFooter.Center = "[Page # of Pages #]";
            exporter.PageFooter.Right = "[Date Printed]";

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
        protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
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
        protected void AddressGrid_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {
            e.Editor.ReadOnly = false;
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
            StateSelect.SelectParameters[0].DefaultValue = Convert.ToString(country);
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
            SelectCity.SelectParameters[0].DefaultValue = Convert.ToString(state);
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


        protected void BankGrid_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType == GridViewRowType.Data)
            {
                int commandColumnIndex = -1;
                for (int i = 0; i < BankGrid.Columns.Count; i++)
                    if (BankGrid.Columns[i] is GridViewCommandColumn)
                    {
                        commandColumnIndex = i;
                        break;
                    }
                if (commandColumnIndex == -1)
                    return;
                //____One colum has been hided so index of command column will be leass by 1 
                commandColumnIndex = commandColumnIndex - 2;
                DevExpress.Web.Rendering.GridViewTableCommandCell cell = e.Row.Cells[commandColumnIndex] as DevExpress.Web.Rendering.GridViewTableCommandCell;
                for (int i = 0; i < cell.Controls.Count; i++)
                {
                    DevExpress.Web.Rendering.GridViewCommandItemsCell button = cell.Controls[i] as DevExpress.Web.Rendering.GridViewCommandItemsCell;
                    if (button == null) return;
                    DevExpress.Web.Internal.InternalHyperLink hyperlink = button.Controls[0] as DevExpress.Web.Internal.InternalHyperLink;

                    if (hyperlink.Text == "Delete")
                    {
                        if (Convert.ToString(Session["PageAccess"]) == "DelAdd" || Convert.ToString(Session["PageAccess"]) == "Delete" || Convert.ToString(Session["PageAccess"]) == "All")
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
        protected void BankGrid_HtmlEditFormCreated(object sender, ASPxGridViewEditFormEventArgs e)
        {
            if (!BankGrid.IsNewRowEditing)
            {
                ASPxPageControl RT1 = BankGrid.FindEditFormTemplateControl("pageControl") as ASPxPageControl;
                //var tabPage = RT1.TabPages.FindByName("tabCorrespondence");
                //tabPage.Enabled = false;
                ASPxGridViewTemplateReplacement RT = RT1.FindControl("UpdateButton") as ASPxGridViewTemplateReplacement;
                if (Convert.ToString(Session["PageAccess"]).Trim() == "Add" || Convert.ToString(Session["PageAccess"]).Trim() == "Modify" || Convert.ToString(Session["PageAccess"]).Trim() == "All")
                    RT.Visible = true;
                else
                    RT.Visible = false;
            }
        }
        private bool IsEditable(string id)
        {
            DataTable dtCheck = new DataTable();
            dtCheck = oDBEngine.GetDataTable("Trans_CashBankDetail ", " * ", " CashBankDetail_MainAccountID='" + id + "' or CashBankDetail_subAccountID='" + id + "'");
            if ((dtCheck != null && dtCheck.Rows.Count == 0) || dtCheck == null)
            {
                dtCheck = oDBEngine.GetDataTable("Trans_AccountsLedger ", " * ", " accountsLedger_MainAccountID='" + id + "' or AccountsLedger_SubAccountID='" + id + "'");
            }
            if ((dtCheck != null && dtCheck.Rows.Count == 0) || dtCheck == null)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        public bool IsDeletable(object id)
        {
            DataTable dtCheck = new DataTable();
            dtCheck = oDBEngine.GetDataTable("Trans_CashBankDetail ", " * ", " CashBankDetail_MainAccountID='" + id.ToString() + "' or CashBankDetail_subAccountID='" + id.ToString() + "'");
            if ((dtCheck != null && dtCheck.Rows.Count == 0) || dtCheck == null)
            {
                dtCheck = oDBEngine.GetDataTable("Trans_AccountsLedger ", " * ", " accountsLedger_MainAccountID='" + id.ToString() + "' or AccountsLedger_SubAccountID='" + id.ToString() + "'");
            }
            if ((dtCheck != null && dtCheck.Rows.Count == 0) || dtCheck == null)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        protected void BankGrid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {


            string tranid = Convert.ToString(e.Parameters);
            if (tranid.Length != 0)
            {

                string[] mainid = tranid.Split('~');
                if (Convert.ToString(mainid[0]) == "Delete")
                {


                    DataTable DPID = oDBEngine.GetDataTable("tbl_master_bank ", " * ", " bnk_internalId='" + Convert.ToString(mainid[1]) + "'");
                    string iddp = Convert.ToString(DPID.Rows[0]["BNK_ID"]).Trim();
                    DataTable dtDPS = oDBEngine.GetDataTable("tbl_trans_contactbankdetails ", " * ", " cbd_bankcode ='" + iddp + "'");
                    if (dtDPS.Rows.Count > 0)
                    {
                        bankid = "N";
                        BankGrid.DataBind();
                    }
                    else
                    {
                        oDBEngine.DeleteValue(" tbl_master_Bank ", "bnk_internalId ='" + Convert.ToString(mainid[1]) + "'");
                        oDBEngine.DeleteValue(" tbl_master_address ", "add_cntId ='" + mainid[1].ToString() + "'");
                        oDBEngine.DeleteValue(" tbl_master_phonefax ", " phf_cntId  ='" + mainid[1].ToString() + "'");
                        oDBEngine.DeleteValue(" tbl_master_email ", " eml_cntId ='" + mainid[1].ToString() + "'");
                        oDBEngine.DeleteValue(" tbl_master_bank_branchMap ", " bankId =" + iddp);
                        

                        BankGrid.DataBind();
                    }


                }
                else
                {

                    if (e.Parameters == "s")
                        BankGrid.Settings.ShowFilterRow = true;

                    if (e.Parameters == "All")
                    {
                        BankGrid.FilterExpression = string.Empty;
                    }
                }


            }


        }
        protected void EmailGrid_RowValidating(object sender, DevExpress.Web.Data.ASPxDataValidationEventArgs e)
        {
            ASPxPageControl tab = BankGrid.FindEditFormTemplateControl("pageControl") as ASPxPageControl;
            ASPxPageControl tab1 = tab.FindControl("ASPxPageControl2") as ASPxPageControl;
            ASPxGridView EmailGrid = tab1.FindControl("EmailGrid") as ASPxGridView;
            string ccEmail = "";
            string email = "";
            try
            {
                email = Convert.ToString(e.NewValues["eml_email"]);
            }
            catch
            {
            }
            try
            {
                ccEmail = Convert.ToString(e.NewValues["eml_ccEmail"]);
            }
            catch
            {
            }
            if (!EmailGrid.IsNewRowEditing)
            {
                string emailID = Convert.ToString(e.Keys[0]);
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
            string RowIndex = "";
            //  string ss = e.NewValues["phf_cntId"].ToString();

            // RowIndex = e.Keys["phf_cntId"].ToString();




            ASPxPageControl tab = BankGrid.FindEditFormTemplateControl("pageControl") as ASPxPageControl;
            ASPxPageControl tab1 = tab.FindControl("ASPxPageControl2") as ASPxPageControl;
            ASPxGridView PhoneGrid = tab1.FindControl("PhoneGrid") as ASPxGridView;
            string PhoneType = Convert.ToString(e.NewValues["phf_type"]);
            if (PhoneType == "Mobile")
            {
                string PhoneNumber = Convert.ToString(e.NewValues["phf_phoneNumber"]);
                if (PhoneNumber.Length != 10)
                {
                    e.RowError = "Enter Valid Mobile Number";
                    return;
                }
            }


        }

        protected void BankGrid_RowValidating(object sender, DevExpress.Web.Data.ASPxDataValidationEventArgs e)
        {

            if (e.Keys["bnk_internalId"] != null)
            {
                string RowIndex = Convert.ToString(e.Keys["bnk_internalId"]);
                if (!IsEditable(RowIndex))
                {
                    e.RowError = "This record is not editable as Transaction exists for this bank";
                    return;
                }
                //string rowkey = BankGrid.GetRowValues(BankGrid.EditingRowVisibleIndex, "bnk_internalId").ToString();
            }
            if (e.NewValues["bnk_micrno"] != "" && e.NewValues["bnk_micrno"] != null)
            {
                if (Convert.ToString(e.NewValues["bnk_micrno"]).Trim().Length > 0 && Convert.ToString(e.NewValues["bnk_micrno"]).Trim().Length != 9)
                {
                    e.RowError = "MICR No showld be 9 digits..";
                    return;
                }
            }
            if (e.NewValues["bnk_IFSCCode"] != "" && e.NewValues["bnk_IFSCCode"] != null)
            {
                if (Convert.ToString(e.NewValues["bnk_IFSCCode"]).Trim().Length > 0 && Convert.ToString(e.NewValues["bnk_IFSCCode"]).Trim().Length != 11)
                {
                    e.RowError = "IFSC Code showld be 11 digits..";
                    return;
                }
            }
            if (e.NewValues["bnk_NEFTCode"] != "" && e.NewValues["bnk_NEFTCode"] != null)
            {
                if (Convert.ToString(e.NewValues["bnk_NEFTCode"]).Trim().Length > 0 && Convert.ToString(e.NewValues["bnk_NEFTCode"]).Trim().Length != 11)
                {
                    e.RowError = "NEFT Code showld be 11 digits..";
                    return;
                }
            }
            if (e.NewValues["bnk_RTGSCode"] != "" && e.NewValues["bnk_RTGSCode"] != null)
            {
                if (Convert.ToString(e.NewValues["bnk_RTGSCode"]).Trim().Length > 0 && Convert.ToString(e.NewValues["bnk_RTGSCode"]).Trim().Length != 11)
                {
                    e.RowError = "RTGS Code showld be 11 digits..";
                    return;
                }
            }

        }
        protected void BankGrid_CustomJSProperties(object sender, DevExpress.Web.ASPxGridViewClientJSPropertiesEventArgs e)
        {
            e.Properties["cpHeight"] = bankid;

        }
        protected void BankGrid_RowCommand(object sender, ASPxGridViewRowCommandEventArgs e)
        {

        }
        protected void BankDataSource_Updated(object sender, SqlDataSourceStatusEventArgs e)
        {

        }
        protected void BankDataSource_Updating(object sender, SqlDataSourceCommandEventArgs e)
        {

        }
        protected void BankGrid_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            // string s =Convert.ToString(e.Keys);
        }
        protected void BankGrid_RowUpdated(object sender, DevExpress.Web.Data.ASPxDataUpdatedEventArgs e)
        {

        }
        protected void BankGrid_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
        {

           // e.Visible = EditButtonVisibleCriteria((ASPxGridView)sender, e.VisibleIndex);
            if (!rights.CanDelete)
            {
                if (e.ButtonType == ColumnCommandButtonType.Delete)
                {
                    e.Visible = false;
                }
            }


            if (!rights.CanEdit)
            {
                if (e.ButtonType == ColumnCommandButtonType.Edit)
                {
                    e.Visible = false;
                }
            }

        }
        private bool EditButtonVisibleCriteria(ASPxGridView grid, int visibleIndex)
        {
            string keyValue = (string)grid.GetRowValues(visibleIndex, grid.KeyFieldName);
            if (keyValue == "BKA0000002")
            {
                return IsEditable(keyValue);
            }
            else
            {
                return IsEditable(keyValue);
            }
        }
        protected void BankPhone_Deleted(object sender, SqlDataSourceStatusEventArgs e)
        {

        }

        protected void PhoneFaxGrid_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {
            e.Editor.ReadOnly = false;
        }

        protected void EmailGrid_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {
            e.Editor.ReadOnly = false;
        }

        protected void BankGrid_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            ASPxPageControl RT1 = BankGrid.FindEditFormTemplateControl("pageControl") as ASPxPageControl;
            var tabPage = RT1.TabPages.FindByName("tabCorrespondence");
            tabPage.Enabled = false;
        }

        protected void BankGrid_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            ASPxPageControl RT1 = BankGrid.FindEditFormTemplateControl("pageControl") as ASPxPageControl;
            var tabPage = RT1.TabPages.FindByName("tabCorrespondence");
            tabPage.Enabled = true;
        }

        //Purpose :Checking the edit and delete rights of user in address tab in edit option 
        //Date : 10.11.2016
        //Name : Debjyoti Dhar.
        protected void AddressGrid_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
        {
            if (!rights.CanDelete)
            {
                if (e.ButtonType == ColumnCommandButtonType.Delete)
                {
                    e.Visible = false;
                }
            }


            if (!rights.CanEdit)
            {
                if (e.ButtonType == ColumnCommandButtonType.Edit)
                {
                    e.Visible = false;
                }
            }

        }

        protected void PhoneFaxGrid_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
        {
            if (!rights.CanDelete)
            {
                if (e.ButtonType == ColumnCommandButtonType.Delete)
                {
                    e.Visible = false;
                }
            }


            if (!rights.CanEdit)
            {
                if (e.ButtonType == ColumnCommandButtonType.Edit)
                {
                    e.Visible = false;
                }
            }

        }

        protected void EmailGrid_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
        {
            if (!rights.CanDelete)
            {
                if (e.ButtonType == ColumnCommandButtonType.Delete)
                {
                    e.Visible = false;
                }
            }


            if (!rights.CanEdit)
            {
                if (e.ButtonType == ColumnCommandButtonType.Edit)
                {
                    e.Visible = false;
                }
            }

        }

    }
}