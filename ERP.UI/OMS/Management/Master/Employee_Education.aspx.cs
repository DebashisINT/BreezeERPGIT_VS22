using System;
using System.Data;
using System.Web;
using System.Web.UI;
//////using DevExpress.Web.ASPxClasses;
//////using DevExpress.Web;
using DevExpress.Web;
using BusinessLogicLayer;
using System.Configuration;
using EntityLayer.CommonELS;


namespace ERP.OMS.Management.Master
{
    public partial class management_master_Employee_Education : ERP.OMS.ViewState_class.VSPage
    {
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine("");
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();

        protected void Page_Init(object sender, EventArgs e)
        {
            sqleducation.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            sqlDegree.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            SqlCountry.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            StateSelect.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            SelectCity.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
        }
        protected void Page_Load(object sender, EventArgs e)
        {

            if(!IsPostBack)
            {
                #region Check Payroll Active Or Not

                BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();
                DataTable ConfigDT = objEngine.GetDataTable("Config_SystemSettings", " Variable_Value ", " Variable_Name='IsPayrollActive' AND IsActive=1");
                if (ConfigDT != null && ConfigDT.Rows.Count > 0)
                {
                    string IsMandatory = Convert.ToString(ConfigDT.Rows[0]["Variable_Value"]).Trim();
                    if (IsMandatory == "Yes")
                    {
                        ASPxPageControl1.TabPages.FindByName("StautoryDocumentDetails").Visible = true;
                        ASPxPageControl1.TabPages.FindByName("PFESI").Visible = true;
                        ASPxPageControl1.TabPages.FindByName("othrdtls").Visible = false;
                    }
                    else
                    {
                        ASPxPageControl1.TabPages.FindByName("StautoryDocumentDetails").Visible = false;
                        ASPxPageControl1.TabPages.FindByName("PFESI").Visible = false;
                        ASPxPageControl1.TabPages.FindByName("othrdtls").Visible = false;
                    }
                }
                else
                {
                    ASPxPageControl1.TabPages.FindByName("StautoryDocumentDetails").Visible = false;
                    ASPxPageControl1.TabPages.FindByName("PFESI").Visible = false;
                    ASPxPageControl1.TabPages.FindByName("othrdtls").Visible = false;
                }
                #endregion
            }
            //------- For Read Only User in SQL Datasource Connection String   Start-----------------
            
                rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Master/employee.aspx");
               
            if (HttpContext.Current.Session["EntryProfileType"] != null)
            {
                if (Convert.ToString(HttpContext.Current.Session["EntryProfileType"]) == "R")
                {
                    //sqleducation.ConnectionString = ConfigurationSettings.AppSettings["DBReadOnlyConnection"];MULTI
                    sqleducation.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                }
                else
                {
                    //sqleducation.ConnectionString = ConfigurationSettings.AppSettings["DBConnectionDefault"];MULTI
                    sqleducation.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                }
            }

            //------- For Read Only User in SQL Datasource Connection String   End-----------------

            string[,] EmployeeNameID = oDBEngine.GetFieldValue(" tbl_master_contact ", " case when cnt_firstName is null then '' else cnt_firstName end + ' '+case when cnt_middleName is null then '' else cnt_middleName end+ ' '+case when cnt_lastName is null then '' else cnt_lastName end+' ['+cnt_shortName+']' as name ", " cnt_internalId='" + HttpContext.Current.Session["KeyVal_InternalID"] + "'", 1);
            if (EmployeeNameID[0, 0] != "n")
            {
                lblHeader.Text = EmployeeNameID[0, 0].ToUpper();
            }
            DataTable DT_empId = oDBEngine.GetDataTable(" tbl_master_contact ", " cnt_id ", " cnt_internalId='" + HttpContext.Current.Session["KeyVal_InternalID"] + "'");
            if (DT_empId.Rows.Count > 0)
            {
                string id = Convert.ToString(DT_empId.Rows[0]["cnt_id"]);
                hdKeyVal.Value = id;
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
        protected void gridEducation_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            gridEducation.SettingsText.PopupEditFormCaption = "Modify Education";
        }
        protected void gridEducation_CellEditorInitialize1(object sender, ASPxGridViewEditorEventArgs e)
        {
            if (e.Column.FieldName == "edu_courseFrom" || e.Column.FieldName == "edu_courseuntil")
            {
                //ASPxDateEdit txtBox = (ASPxDateEdit)e.Editor;
                //txtBox.ClientSideEvents.Validation = "function(s,e) {{ e.isValid = ValidateEditorValue(); e.errorText = '*';}}";
            }
            
            if (e.Column.FieldName == "edu_state")
            {
                if (e.KeyValue != null)
                {
                    object val = gridEducation.GetRowValuesByKeyValue(e.KeyValue, "edu_country");
                    if (val == DBNull.Value) return;
                    int country = (int)val;
                    ASPxComboBox combo = e.Editor as ASPxComboBox;
                    FillStateCombo(combo, country);

                    combo.Callback += new CallbackEventHandlerBase(cmbState_OnCallback);
                }
                else
                {

                    object val = gridEducation.GetRowValues(0, "edu_country");
                    ASPxComboBox combo = e.Editor as ASPxComboBox;
                    if (!gridEducation.IsNewRowEditing)
                    {
                        if (val != DBNull.Value)
                        {

                            int country = (int)val;

                            FillStateCombo(combo, country);


                        }
                        else
                        {

                            int country = 1;
                            //ASPxComboBox combo = e.Editor as ASPxComboBox;
                            FillStateCombo(combo, country);

                            //combo.Callback += new CallbackEventHandlerBase(cmbState_OnCallback);
                        }
                    }
                    combo.Callback += new CallbackEventHandlerBase(cmbState_OnCallback);
                    ////***************
                }
            }
            ///////////////////
            if (e.Column.FieldName == "edu_city")
            {
                if (e.KeyValue != null)
                {
                    object val = gridEducation.GetRowValuesByKeyValue(e.KeyValue, "edu_state");
                    if (val == DBNull.Value) return;
                    int state = (int)val;
                    ASPxComboBox combo = e.Editor as ASPxComboBox;
                    FillCityCombo(combo, state);

                    combo.Callback += new CallbackEventHandlerBase(cmbCity_OnCallback);
                }
                else
                {

                    object val = gridEducation.GetRowValues(0, "edu_state");
                    ASPxComboBox combo = e.Editor as ASPxComboBox;
                    if (!gridEducation.IsNewRowEditing)
                    {
                        if (val != DBNull.Value)
                        {

                            int state = (int)val;

                            FillCityCombo(combo, state);

                            //combo.Callback += new CallbackEventHandlerBase(cmbCity_OnCallback);
                        }
                        else
                        {

                            int state = 1;
                            //ASPxComboBox combo = e.Editor as ASPxComboBox;
                            FillCityCombo(combo, state);

                            //combo.Callback += new CallbackEventHandlerBase(cmbCity_OnCallback);
                        }
                    }
                    combo.Callback += new CallbackEventHandlerBase(cmbCity_OnCallback);
                }
            }


            //if (e.Column.FieldName == "edu_courseuntil")
            // {

            //     object val = gridEducation.GetRowValuesByKeyValue(e.KeyValue, "edu_courseuntil");
            //     object val1 = gridEducation.GetRowValuesByKeyValue(e.KeyValue, "edu_courseFrom");

            //     DateTime startdate = (DateTime)val1;
            //     DateTime enddate = (DateTime)val;
            //    if(startdate>enddate)
            //    {

            //        Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script language='javascript'>alert('hhh');</script>");
            //    }

            // }
        }
        protected void gridEducation_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            e.NewValues["edu_month_year"] = Convert.ToDateTime(oDBEngine.GetDate().ToString());
        }
        protected void gridEducation_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
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