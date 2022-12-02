using System;
using System.Data;
using System.Web.UI;
////using DevExpress.Web.ASPxClasses;
//using DevExpress.Web;
using DevExpress.Web;
using System.Web;
using System.Configuration;
using EntityLayer.CommonELS;

namespace ERP.OMS.Management.Master
{
    public partial class management_master_ProfBodies_correspondence : ERP.OMS.ViewState_class.VSPage
    {
        public  EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        protected void Page_Init(object sender, EventArgs e)
        {
            CountrySelect.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            StateSelect.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            SelectCity.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            SelectArea.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            SelectPin.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
        }
        protected void Page_Load(object sender, EventArgs e)
        { 
                rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Master/ProfBodies.aspx");
               
            //------- For Read Only User in SQL Datasource Connection String   Start-----------------

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
                //    Address.ConnectionString = ConfigurationSettings.AppSettings["DBConnectionDefault"];MULTI
                //    Phone.ConnectionString = ConfigurationSettings.AppSettings["DBConnectionDefault"];MULTI
                //    Email.ConnectionString = ConfigurationSettings.AppSettings["DBConnectionDefault"];MULTI
                    Address.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                    Phone.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                    Email.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                }
                //}
            }

            //------- For Read Only User in SQL Datasource Connection String   End-----------------
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
        //            if (val == DBNull.Value) return;
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
        //            if (val == DBNull.Value) return;
        //            if (val != null)
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
            //debjyoti 06-12-2016
            //pin code combobox added
            if (e.Column.FieldName == "PinCode")
            {
                if (e.KeyValue != null)
                {
                    object val = AddressGrid.GetRowValuesByKeyValue(e.KeyValue, "City");
                    if (val == DBNull.Value) return;
                    int city = (int)val;
                    ASPxComboBox combo = e.Editor as ASPxComboBox;
                    FillPinCombo(combo, city);
                    combo.Callback += new CallbackEventHandlerBase(cmbPin_OnCallback);
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
                            FillPinCombo(combo, city);

                        }
                        else
                        {
                            int city = 1;
                            FillPinCombo(combo, city);
                        }
                    }
                    combo.Callback += new CallbackEventHandlerBase(cmbPin_OnCallback);

                }
            }

            //end debjyoti 06-12-2016

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

        //debjyoti 06-12-2016
        protected void FillPinCombo(ASPxComboBox cmb, int city)
        {
            string[,] pin = GetPin(city);
            cmb.Items.Clear();

            for (int i = 0; i < pin.GetLength(0); i++)
            {
                cmb.Items.Add(pin[i, 1], pin[i, 0]);
            }

        }
        string[,] GetPin(int city)
        {
            //Purpose : Replace .ToString() with Convert.ToString(..)
            //Name : Sudip 
            // Dated : 21-12-2016

            SelectPin.SelectParameters[0].DefaultValue = Convert.ToString(city);
            DataView view = (DataView)SelectPin.Select(DataSourceSelectArguments.Empty);
            string[,] DATA = new string[view.Count, 2];
            for (int i = 0; i < view.Count; i++)
            {
                DATA[i, 0] = Convert.ToString(view[i][0]);
                DATA[i, 1] = Convert.ToString(view[i][1]);
            }
            return DATA;
        }
        //end debjyoti 06-12-2016

        string[,] GetState(int country)
        {
            //Purpose : Replace .ToString() with Convert.ToString(..)
            //Name : Sudip 
            // Dated : 21-12-2016

            StateSelect.SelectParameters[0].DefaultValue = Convert.ToString(country);
            DataView view = (DataView)StateSelect.Select(DataSourceSelectArguments.Empty);
            string[,] DATA = new string[view.Count, 2];
            for (int i = 0; i < view.Count; i++)
            {
                DATA[i, 0] = Convert.ToString(view[i][0]);
                DATA[i, 1] = Convert.ToString(view[i][1]);
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
            //Purpose : Replace .ToString() with Convert.ToString(..)
            //Name : Sudip 
            // Dated : 21-12-2016

            SelectCity.SelectParameters[0].DefaultValue = Convert.ToString(state);
            DataView view = (DataView)SelectCity.Select(DataSourceSelectArguments.Empty);
            string[,] DATA = new string[view.Count, 2];
            for (int i = 0; i < view.Count; i++)
            {
                DATA[i, 0] = Convert.ToString(view[i][0]);
                DATA[i, 1] = Convert.ToString(view[i][1]);
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
            //Purpose : Replace .ToString() with Convert.ToString(..)
            //Name : Sudip 
            // Dated : 21-12-2016

            SelectArea.SelectParameters[0].DefaultValue = Convert.ToString(city);
            DataView view = (DataView)SelectArea.Select(DataSourceSelectArguments.Empty);
            string[,] DATA = new string[view.Count, 2];
            for (int i = 0; i < view.Count; i++)
            {
                DATA[i, 0] = Convert.ToString(view[i][0]);
                DATA[i, 1] = Convert.ToString(view[i][1]);
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
        private void cmbArea_OnCallback(object source, CallbackEventArgsBase e)
        {
            FillAreaCombo(source as ASPxComboBox, Convert.ToInt32(e.Parameter));
        }
        //debjyoti 06-12-2016
        private void cmbPin_OnCallback(object source, CallbackEventArgsBase e)
        {
            FillPinCombo(source as ASPxComboBox, Convert.ToInt32(e.Parameter));
        }
        //End debjyoti 06-12-2016



        //Purpose : Add user rights to address grid in correspondence tab
        //Name : Debjyoti 
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
        //Purpose : Add user rights to Phone grid in correspondence tab
        //Name : Debjyoti 
        protected void PhoneGrid_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
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

        protected void AddressGrid_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            AddressGrid.SettingsText.PopupEditFormCaption = "Modify Address";
        }
        protected void PhoneGrid_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            
            PhoneGrid.SettingsText.PopupEditFormCaption = "Modify Phone";
        }
        protected void EmailGrid_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            EmailGrid.SettingsText.PopupEditFormCaption = "Modify Email";
        }

        //Purpose : Add user rights to Email grid in correspondence tab
        //Name : Debjyoti 
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