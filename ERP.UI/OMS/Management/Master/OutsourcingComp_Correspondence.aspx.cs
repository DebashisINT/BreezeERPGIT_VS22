using System;
using System.Data;
using System.Web;
using System.Web.UI;
////using DevExpress.Web.ASPxClasses;
//using DevExpress.Web;
using DevExpress.Web;
using System.Configuration;
namespace ERP.OMS.Management.Master
{
    public partial class management_master_OutsourcingComp_Correspondence : ERP.OMS.ViewState_class.VSPage
    {
        //DBEngine oDBEngine = new DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        protected void Page_Load(object sender, EventArgs e)
        {

            //------- For Read Only User in SQL Datasource Connection String   Start-----------------
            CountrySelect.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            StateSelect.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            SelectCity.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            SelectArea.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            if (HttpContext.Current.Session["EntryProfileType"] != null)
            {
                if (Convert.ToString(HttpContext.Current.Session["EntryProfileType"]) == "R")
                {
                    //Address.ConnectionString = ConfigurationSettings.AppSettings["DBReadOnlyConnection"];MULTI
                    //Phone.ConnectionString = ConfigurationSettings.AppSettings["DBReadOnlyConnection"];MULTI
                    //Email.ConnectionString = ConfigurationSettings.AppSettings["DBReadOnlyConnection"];MULTI
                    Address.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                    Phone.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                    Email.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);

                }
                else
                {
                    //Address.ConnectionString = ConfigurationSettings.AppSettings["DBConnectionDefault"];MULTI
                    //Phone.ConnectionString = ConfigurationSettings.AppSettings["DBConnectionDefault"];MULTI
                    //Email.ConnectionString = ConfigurationSettings.AppSettings["DBConnectionDefault"];MULTI
                    Address.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                    Phone.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                    Email.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                }
            }

            //------- For Read Only User in SQL Datasource Connection String   End-----------------

            string[,] EmployeeNameID = oDBEngine.GetFieldValue(" tbl_master_contact ", " case when cnt_firstName is null then '' else cnt_firstName end + ' '+case when cnt_middleName is null then '' else cnt_middleName end+ ' '+case when cnt_lastName is null then '' else cnt_lastName end+' ['+cnt_shortName+']' as name ", " cnt_internalId='" + HttpContext.Current.Session["KeyVal_InternalID"] + "'", 1);
            if (EmployeeNameID[0, 0] != "n")
            {
                lblHeader.Text = EmployeeNameID[0, 0].ToUpper();
            }
        }
        //protected void AddressGrid_CellEditorInitialize(object sender, DevExpress.Web.ASPxGridViewEditorEventArgs e)
        //{
        //    if (e.Column.FieldName == "State")
        //    {
        //        if (e.KeyValue != null)
        //        {
        //            object val = AddressGrid.GetRowValuesByKeyValue(e.KeyValue, "Country");
        //            if (val == DBNull.Value) return;
        //            int country = (int)val;
        //            ASPxComboBox combo = e.Editor as ASPxComboBox;
        //            FillStateCombo(combo, country);

        //            combo.Callback += new CallbackEventHandlerBase(cmbState_OnCallback);
        //        }
        //        else
        //        {

        //            object val = AddressGrid.GetRowValues(0, "Country");
        //            if (val != null)
        //            {

        //                int country = (int)val;
        //                ASPxComboBox combo = e.Editor as ASPxComboBox;
        //                FillStateCombo(combo, country);

        //                combo.Callback += new CallbackEventHandlerBase(cmbState_OnCallback);
        //            }
        //            else
        //            {

        //                int country = 1;
        //                ASPxComboBox combo = e.Editor as ASPxComboBox;
        //                FillStateCombo(combo, country);

        //                combo.Callback += new CallbackEventHandlerBase(cmbState_OnCallback);
        //            }
        //        }
        //    }
        //    ///////////////////
        //    if (e.Column.FieldName == "City")
        //    {
        //        if (e.KeyValue != null)
        //        {
        //            object val = AddressGrid.GetRowValuesByKeyValue(e.KeyValue, "State");
        //            if (val == DBNull.Value) return;
        //            int state = (int)val;
        //            ASPxComboBox combo = e.Editor as ASPxComboBox;
        //            FillCityCombo(combo, state);

        //            combo.Callback += new CallbackEventHandlerBase(cmbCity_OnCallback);
        //        }
        //        else
        //        {

        //            object val = AddressGrid.GetRowValues(0, "State");
        //            if (val == DBNull.Value)
        //            {

        //                int state = (int)val;
        //                ASPxComboBox combo = e.Editor as ASPxComboBox;
        //                FillCityCombo(combo, state);

        //                combo.Callback += new CallbackEventHandlerBase(cmbCity_OnCallback);
        //            }
        //            else
        //            {

        //                int state = 1;
        //                ASPxComboBox combo = e.Editor as ASPxComboBox;
        //                FillCityCombo(combo, state);

        //                combo.Callback += new CallbackEventHandlerBase(cmbCity_OnCallback);
        //            }
        //        }
        //    }
        //    if (e.Column.FieldName == "area")
        //    {
        //        if (e.KeyValue != null)
        //        {
        //            object val = AddressGrid.GetRowValuesByKeyValue(e.KeyValue, "City");
        //            if (val == DBNull.Value) return;
        //            int city = (int)val;
        //            ASPxComboBox combo = e.Editor as ASPxComboBox;
        //            FillAreaCombo(combo, city);
        //            combo.Callback += new CallbackEventHandlerBase(cmbArea_OnCallback);
        //        }
        //        else
        //        {

        //            object val = AddressGrid.GetRowValues(0, "City");
        //            if (val == DBNull.Value) return;
        //            if (val != null)
        //            {
        //                int city = (int)val;
        //                ASPxComboBox combo = e.Editor as ASPxComboBox;
        //                FillAreaCombo(combo, city);
        //                combo.Callback += new CallbackEventHandlerBase(cmbArea_OnCallback);
        //            }
        //            else
        //            {

        //                int city = 1;
        //                ASPxComboBox combo = e.Editor as ASPxComboBox;
        //                FillAreaCombo(combo, city);

        //                combo.Callback += new CallbackEventHandlerBase(cmbArea_OnCallback);
        //            }
        //        }
        //    }
        //}

        protected void AddressGrid_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {
            //if (AddressGrid.IsNewRowEditing) return;

            if (e.Column.FieldName == "State")
            {
                if (e.KeyValue != null)
                {
                    object val = AddressGrid.GetRowValuesByKeyValue(e.KeyValue, "Country");
                    if (val == DBNull.Value) return;
                    int country = (int)val;
                    ASPxComboBox combo = e.Editor as ASPxComboBox;
                    FillStateCombo(combo, country);
                    combo.Callback += new CallbackEventHandlerBase(cmbState_OnCallback);
                }
                else
                {
                    ASPxComboBox combo = e.Editor as ASPxComboBox;
                    if (!AddressGrid.IsNewRowEditing)
                    {
                        object val = AddressGrid.GetRowValues(0, "Country");
                        if (val == DBNull.Value) return;
                        if (val != null)
                        {
                            int country = (int)val;
                            //// ASPxComboBox combo = e.Editor as ASPxComboBox;
                            FillStateCombo(combo, country);
                            ////combo.Callback += new CallbackEventHandlerBase(cmbState_OnCallback);
                        }
                        else
                        {

                            int country = 1;
                            //// ASPxComboBox combo = e.Editor as ASPxComboBox;
                            FillStateCombo(combo, country);
                            ////combo.Callback += new CallbackEventHandlerBase(cmbState_OnCallback);
                        }
                    }
                    combo.Callback += new CallbackEventHandlerBase(cmbState_OnCallback);


                }
            }
            ///////////////////
            if (e.Column.FieldName == "City")
            {
                if (e.KeyValue != null)
                {
                    object val = AddressGrid.GetRowValuesByKeyValue(e.KeyValue, "State");
                    if (val == DBNull.Value) return;
                    int state = (int)val;
                    ASPxComboBox combo = e.Editor as ASPxComboBox;
                    FillCityCombo(combo, state);
                    combo.Callback += new CallbackEventHandlerBase(cmbCity_OnCallback);
                }
                else
                {
                    ASPxComboBox combo = e.Editor as ASPxComboBox;
                    if (!AddressGrid.IsNewRowEditing)
                    {
                        object val = AddressGrid.GetRowValues(0, "State");
                        if (val == DBNull.Value) return;
                        if (val != null)
                        {
                            int state = (int)val;
                            ////ASPxComboBox combo = e.Editor as ASPxComboBox;
                            FillCityCombo(combo, state);
                            ////combo.Callback += new CallbackEventHandlerBase(cmbCity_OnCallback);
                        }
                        else
                        {

                            int state = 1;
                            ////ASPxComboBox combo = e.Editor as ASPxComboBox;
                            FillCityCombo(combo, state);

                            ////combo.Callback += new CallbackEventHandlerBase(cmbCity_OnCallback);
                        }
                    }
                    combo.Callback += new CallbackEventHandlerBase(cmbCity_OnCallback);

                }
            }
            if (e.Column.FieldName == "area")
            {
                if (e.KeyValue != null)
                {
                    object val = AddressGrid.GetRowValuesByKeyValue(e.KeyValue, "City");
                    if (val == DBNull.Value) return;
                    int city = (int)val;
                    ASPxComboBox combo = e.Editor as ASPxComboBox;
                    FillAreaCombo(combo, city);
                    combo.Callback += new CallbackEventHandlerBase(cmbArea_OnCallback);
                }
                else
                {
                    ASPxComboBox combo = e.Editor as ASPxComboBox;
                    if (!AddressGrid.IsNewRowEditing)
                    {
                        object val = AddressGrid.GetRowValues(0, "City");
                        if (val == DBNull.Value) return;
                        if (val != null)
                        {
                            int city = (int)val;
                            ////ASPxComboBox combo = e.Editor as ASPxComboBox;
                            FillAreaCombo(combo, city);
                            ////combo.Callback += new CallbackEventHandlerBase(cmbArea_OnCallback);
                        }
                        else
                        {

                            int city = 1;
                            ////ASPxComboBox combo = e.Editor as ASPxComboBox;
                            FillAreaCombo(combo, city);

                            ////combo.Callback += new CallbackEventHandlerBase(cmbArea_OnCallback);
                        }
                    }
                    combo.Callback += new CallbackEventHandlerBase(cmbArea_OnCallback);

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
        protected void FillAreaCombo(ASPxComboBox cmb, int city)
        {
            string[,] area = GetArea(city);
            cmb.Items.Clear();

            for (int i = 0; i < area.GetLength(0); i++)
            {
                cmb.Items.Add(area[i, 1], area[i, 0]);
            }
        }
        string[,] GetArea(int city)
        {
            SelectArea.SelectParameters[0].DefaultValue = city.ToString();
            DataView view = (DataView)SelectArea.Select(DataSourceSelectArguments.Empty);
            string[,] DATA = new string[view.Count, 2];
            for (int i = 0; i < view.Count; i++)
            {
                DATA[i, 0] = view[i][0].ToString();
                DATA[i, 1] = view[i][1].ToString();
            }
            return DATA;
        }
        private void cmbArea_OnCallback(object source, CallbackEventArgsBase e)
        {
            FillAreaCombo(source as ASPxComboBox, Convert.ToInt32(e.Parameter));
        }
        private void cmbState_OnCallback(object source, CallbackEventArgsBase e)
        {
            FillStateCombo(source as ASPxComboBox, Convert.ToInt32(e.Parameter));
        }
        private void cmbCity_OnCallback(object source, CallbackEventArgsBase e)
        {
            FillCityCombo(source as ASPxComboBox, Convert.ToInt32(e.Parameter));
        }

        protected void EmailGrid_RowValidating(object sender, DevExpress.Web.Data.ASPxDataValidationEventArgs e)
        {
            string ccEmail = "";
            string email = e.NewValues["eml_email"].ToString();
            try
            {
                ccEmail = e.NewValues["eml_ccEmail"].ToString();
            }
            catch
            {
            }
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
}