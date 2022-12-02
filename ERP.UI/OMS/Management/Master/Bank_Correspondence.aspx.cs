using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
//using DevExpress.Web.ASPxClasses;
//using DevExpress.Web.ASPxEditors;
using DevExpress.Web;
//using DevExpress.Web.ASPxTabControl;
using BusinessLogicLayer;
using EntityLayer.CommonELS;
//using System.Collections.Generic;
using System.Linq;
using DevExpress.Utils;
using System.Net.Mime;
using DevExpress.XtraPrinting;
using System.IO;

namespace ERP.OMS.Management.Master
{

    public partial class Bank_Correspondence : ERP.OMS.ViewState_class.VSPage
    {
        private string[] values = new string[] { "CategoryName1", "CategoryName3", "CategoryName5", "CategoryName7" };
        string emailid = "a";
        String Stat = "N";
        //DBEngine oDBEngine = new DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        public string pageAccess = "";
        //Converter Oconverter = new Converter();
        BusinessLogicLayer.Converter Oconverter = new BusinessLogicLayer.Converter();
        // DBEngine oDBEngine = new DBEngine(string.Empty);

        BusinessLogicLayer.Contact OContactGeneralBL = new BusinessLogicLayer.Contact();
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();

        #region Loading Detail
        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //'http://localhost:2957/InfluxCRM/management/testProjectMainPage_employee.aspx'
                // .............................Code Commented and Added by Sam on 08122016 to use Convert.tostring instead of tostring(). ................
                //string sPath = HttpContext.Current.Request.Url.ToString();
                string sPath = Convert.ToString(HttpContext.Current.Request.Url);
                // .............................Code Above Commented and Added by Sam on 08122016 to use Convert.tostring instead of tostring(). ..................................... 
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }
        protected void Page_Init(object sender, EventArgs e)
        {
            Address.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            CountrySelect.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            StateSelect.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            SelectCity.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            SelectArea.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            Phone.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            SelectPin.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            Email.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
        }
        protected void Page_Unload(object sender, EventArgs e)
        {
            //Page.ClientScript.RegisterStartupScript(GetType(), "jscript", "<script language='javascript'>iframesource();</script>");
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Master/bank.aspx");

         

            Session["ContactType"] = "Bank";
           
            
            string cnttype = "";
            if (Session["ContactType"] != null)
            {
                cnttype = Convert.ToString(Session["ContactType"]);
            }
            string intid = "";
            if (Session["KeyVal_InternalID"] != null)
            {
                intid = Convert.ToString(Session["KeyVal_InternalID"]);
            }

           

            if (HttpContext.Current.Session["userid"] == null)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            if (!IsPostBack)
            {
                StDate.UseMaskBehavior = true;
                StDate.EditFormatString = Oconverter.GetDateFormat("Date");
                StDateP.UseMaskBehavior = true;
                StDateP.EditFormatString = Oconverter.GetDateFormat("Date");
                StDateE.UseMaskBehavior = true;
                StDateE.EditFormatString = Oconverter.GetDateFormat("Date");


                if (Session["Name"] != null)
                {
                    lblName.Text = Convert.ToString(Session["Name"]);
                }
               

            }
           
            SalesVisitAddress();

           
        }

        #endregion Loading Detail
    



        #region DropDown Binding Detail


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

            //debjyoti 02-12-2016
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

            //end debjyoti 02-12-2016

        }


        protected void FillStateCombo(ASPxComboBox cmb, int country)
        {

            string[,] state = GetState(country);
            cmb.Items.Clear();

            for (int i = 0; i < state.GetLength(0); i++)
            {
                cmb.Items.Add(state[i, 1], state[i, 0]);
            }
            // cmb.Items.Insert(0, new ListEditItem("Select", "0"));
        }

        //debjyoti 02-12-2016
        protected void FillPinCombo(ASPxComboBox cmb, int city)
        {
            string[,] pin = GetPin(city);
            cmb.Items.Clear();

            for (int i = 0; i < pin.GetLength(0); i++)
            {
                cmb.Items.Add(pin[i, 1], pin[i, 0]);
            }

        }
        //end debjyoti 02-12-2016

        string[,] GetState(int country)
        {
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

        string[,] GetPin(int city)
        {
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

        //end debjyoti 02-12-2016
        protected void FillCityCombo(ASPxComboBox cmb, int state)
        {

            string[,] cities = GetCities(state);
            cmb.Items.Clear();

            for (int i = 0; i < cities.GetLength(0); i++)
            {
                cmb.Items.Add(cities[i, 1], cities[i, 0]);
            }
            // cmb.Items.Insert(0, new ListEditItem("Select", "0"));
        }

        string[,] GetCities(int state)
        {


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
            cmb.Items.Insert(0, new ListEditItem("Select", "0"));
        }
        string[,] GetArea(int city)
        {
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

        //debjyoti 02-12-2016
        private void cmbPin_OnCallback(object source, CallbackEventArgsBase e)
        {
            FillPinCombo(source as ASPxComboBox, Convert.ToInt32(e.Parameter));
        }
        //End debjyoti 02-12-2016

        #endregion DropDown Binding Detail
        public void SalesVisitAddress()
        {
            // .............................Code Commented and Added by Sam on 08122016 to use Convert.tostring instead of tostring(). ................

            if (Session["ContactType"] == null)
            {
                Session["ContactType"] = "Bank";
            }
            try
            {
                if (Request.QueryString["formtype"] != null)
                {
                    string ID = Convert.ToString(Session["InternalId"]);
                    Session["KeyVal_InternalID_New"] = Convert.ToString(ID);
                    //Address.SelectCommand = "select DISTINCT  tbl_master_address.add_id AS Id, tbl_master_address.add_addressType AS Type,tbl_master_address.add_address1 AS Address1,  tbl_master_address.add_address2 AS Address2,tbl_master_address.add_address3 AS Address3,tbl_master_address.add_landMark AS LandMark,tbl_master_address.add_country AS Country,tbl_master_address.add_state AS State,tbl_master_address.add_city AS City,CASE isnull(add_country, '')WHEN '' THEN '' ELSE(SELECT cou_country FROM tbl_master_country WHERE cou_id = add_country) END AS Country1,CASE isnull(add_state, '') WHEN '' THEN '' ELSE(SELECT state FROM tbl_master_state WHERE id = add_state) END AS State1,CASE isnull(add_city, '') WHEN '' THEN '' ELSE(SELECT city_name FROM tbl_master_city WHERE city_id = add_city) END AS City1,CASE add_area WHEN '' THEN '' Else(select area_name From tbl_master_area Where area_id = add_area) End AS add_area, area = CAST(add_area as int), tbl_master_address.add_pin AS PinCode, tbl_master_address.add_landMark AS LankMark, case when add_status='N' then 'Deactive' else 'Active' end as status from tbl_master_address where add_cntId='" + ID + "'";
                    Email.SelectCommand = "select eml_id,eml_cntId,eml_entity,eml_type,eml_email,eml_ccEmail,eml_website,CreateDate,CreateUser,case when eml_Status='N' then 'Deactive' else 'Active' end as status,(case when eml_facility=1 then '1' when eml_facility=2 then '2' else null end) as eml_facility from tbl_master_email where eml_cntId='" + ID + "'";

                    Phone.SelectCommand = "select DISTINCT phf_id,phf_cntId,phf_entity,phf_type,phf_countryCode,phf_areaCode,phf_phoneNumber,phf_extension, ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' + ISNULL(phf_phoneNumber, '') +' '+ ISNULL(phf_extension, '') + ISNULL(phf_faxNumber, '') AS Number,case when phf_Status='N' then 'Deactive' else 'Active' end as status,(case when phf_SMSFacility=1 then '1' when phf_SMSFacility=2 then '2' else null end) as phf_SMSFacility  from tbl_master_phonefax where phf_cntId='" + ID + "'";
                    //Email.SelectCommand = "select eml_id,eml_cntId,eml_entity,eml_type,eml_email,eml_ccEmail,eml_website,CreateDate,CreateUser from tbl_master_email where eml_cntId='" + ID + "'";
                    //Phone.SelectCommand = "select DISTINCT phf_id,phf_cntId,phf_entity,phf_type,phf_countryCode,phf_areaCode,phf_phoneNumber,phf_extension, ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' + ISNULL(phf_phoneNumber, '') +' '+ ISNULL(phf_extension, '') + ISNULL(phf_faxNumber, '') AS Number,case when phf_Status='N' then 'Deactive' else 'Active' end as status  from tbl_master_phonefax where phf_cntId='" + ID + "'";
                  
                    AddressGrid.Columns[15].Visible = true;
                    PhoneGrid.Columns[10].Visible = false;
                    EmailGrid.Columns[7].Visible = false;
                }
                else
                {
                    if (Session["KeyVal_InternalID"] != null)
                    {
                        string ID = Convert.ToString(Session["KeyVal_InternalID"]);
                        Session["KeyVal_InternalID_New"] = Convert.ToString(ID);
                        //Address.SelectCommand = "select DISTINCT  tbl_master_address.add_id AS Id, tbl_master_address.add_addressType AS Type,tbl_master_address.add_address1 AS Address1,  tbl_master_address.add_address2 AS Address2,tbl_master_address.add_address3 AS Address3,tbl_master_address.add_landMark AS LandMark,tbl_master_address.add_country AS Country,tbl_master_address.add_state AS State,tbl_master_address.add_city AS City,CASE isnull(add_country, '')WHEN '' THEN '' ELSE(SELECT cou_country FROM tbl_master_country WHERE cou_id = add_country) END AS Country1,CASE isnull(add_state, '') WHEN '' THEN '' ELSE(SELECT state FROM tbl_master_state WHERE id = add_state) END AS State1,CASE isnull(add_city, '') WHEN '' THEN '' ELSE(SELECT city_name FROM tbl_master_city WHERE city_id = add_city) END AS City1,CASE add_area WHEN '' THEN '' Else(select area_name From tbl_master_area Where area_id = add_area) End AS add_area, area = CAST(add_area as int), tbl_master_address.add_pin AS PinCode, tbl_master_address.add_landMark AS LankMark, case when add_status='N' then 'Deactive' else 'Active' end as status from tbl_master_address where add_cntId='" + ID + "'";
                        Email.SelectCommand = "select eml_id,eml_cntId,eml_entity,eml_type,eml_email,eml_ccEmail,eml_website,CreateDate,CreateUser,case when eml_Status='N' then 'Deactive' else 'Active' end as status,(case when eml_facility=1 then '1' when eml_facility=2 then '2' else null end) as eml_facility from tbl_master_email where eml_cntId='" + ID + "'";
                        //Email.SelectCommand = "select eml_id,eml_cntId,eml_entity,eml_type,eml_email,eml_ccEmail,eml_website,CreateDate,CreateUser,case when eml_Status='N' then 'Deactive' else 'Active' end as status,(case when eml_facility IS null then 'No' when eml_facility=1 then 'yes' else 'No' end) as eml_facility from tbl_master_email where eml_cntId='" + ID + "'";
                        Phone.SelectCommand = "select DISTINCT phf_id,phf_cntId,phf_entity,phf_type,phf_countryCode,phf_areaCode,phf_phoneNumber,phf_extension, ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' + ISNULL(phf_phoneNumber, '') +' '+ ISNULL(phf_extension, '') + ISNULL(phf_faxNumber, '') AS Number,case when phf_Status='N' then 'Deactive' else 'Active' end as status,(case when phf_SMSFacility=1 then '1' when phf_SMSFacility=2 then '2' else null end) as phf_SMSFacility  from tbl_master_phonefax where phf_cntId='" + ID + "'";
                        AddressGrid.Columns[15].Visible = true;
                        PhoneGrid.Columns[10].Visible = false;
                        EmailGrid.Columns[7].Visible = false;
                    }
                    else
                    {
                        if (Request.QueryString["requesttypeP"] != null)
                        {
                            string ID = Convert.ToString(Session["LeadId"]);
                            Session["KeyVal_InternalID_New"] = Convert.ToString(ID);
                            //Address.SelectCommand = "select DISTINCT  tbl_master_address.add_id AS Id, tbl_master_address.add_addressType AS Type,tbl_master_address.add_address1 AS Address1,  tbl_master_address.add_address2 AS Address2,tbl_master_address.add_address3 AS Address3,tbl_master_address.add_landMark AS LandMark,tbl_master_address.add_country AS Country,tbl_master_address.add_state AS State,tbl_master_address.add_city AS City,CASE isnull(add_country, '')WHEN '' THEN '0' ELSE(SELECT cou_country FROM tbl_master_country WHERE cou_id = add_country) END AS Country1,CASE isnull(add_state, '') WHEN '' THEN '' ELSE(SELECT state FROM tbl_master_state WHERE id = add_state) END AS State1,CASE isnull(add_city, '') WHEN '' THEN '' ELSE(SELECT city_name FROM tbl_master_city WHERE city_id = add_city) END AS City1,CASE add_area WHEN '' THEN '' Else(select area_name From tbl_master_area Where area_id = add_area) End AS add_area, area = CAST(add_area as int), tbl_master_address.add_pin AS PinCode, tbl_master_address.add_landMark AS LankMark, case when add_status='N' then 'Deactive' else 'Active' end as status from tbl_master_address where add_cntId='" + ID + "'";
                            Email.SelectCommand = "select eml_id,eml_cntId,eml_entity,eml_type,eml_email,eml_ccEmail,eml_website,CreateDate,CreateUser,case when eml_Status='N' then 'Deactive' else 'Active' end as status,(case when eml_facility=1 then '1' when eml_facility=2 then '2' else null end) as eml_facility from tbl_master_email where eml_cntId='" + ID + "'";

                            Phone.SelectCommand = "select DISTINCT phf_id,phf_cntId,phf_entity,phf_type,phf_countryCode,phf_areaCode,phf_phoneNumber,phf_extension, ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' + ISNULL(phf_phoneNumber, '') +' '+ ISNULL(phf_extension, '') + ISNULL(phf_faxNumber, '') AS Number,case when phf_Status='N' then 'Deactive' else 'Active' end as status,(case when phf_SMSFacility=1 then '1' when phf_SMSFacility=2 then '2' else null end) as phf_SMSFacility  from tbl_master_phonefax where phf_cntId='" + ID + "'";

                            

                            


                            AddressGrid.Columns[15].Visible = true;
                            PhoneGrid.Columns[10].Visible = false;
                            EmailGrid.Columns[7].Visible = false;
                        }
                    }
                }
            }
            catch
            {
            }

            // .............................Code Above Commented and Added by Sam on 08122016 to use Convert.tostring instead of tostring(). ..................................... 
        }


        protected void ASPxPageControl1_ActiveTabChanged(object source, TabControlEventArgs e)
        {

        }





        #region Address Tab Section

        protected void AddressGrid_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            // .............................Code Commented and Added by Sam on 08122016 to use Convert.tostring instead of tostring(). ................
            if (e.Parameters != null)
            {
                // .............................Code Commented and Added by Sanjib on 20122016 due to exception for pincode1 ................
                //Address.SelectCommand = "select DISTINCT  tbl_master_address.add_id AS Id, tbl_master_address.add_addressType AS Type,tbl_master_address.add_address1 AS Address1,  tbl_master_address.add_address2 AS Address2,tbl_master_address.add_address3 AS Address3,tbl_master_address.add_landMark AS LandMark,tbl_master_address.add_country AS Country,tbl_master_address.add_state AS State,tbl_master_address.add_city AS City,CASE isnull(add_country, '')WHEN '' THEN '' ELSE(SELECT cou_country FROM tbl_master_country WHERE cou_id = add_country) END AS Country1,CASE isnull(add_state, '') WHEN '' THEN '' ELSE(SELECT state FROM tbl_master_state WHERE id = add_state) END AS State1,CASE isnull(add_city, '') WHEN '' THEN '' ELSE(SELECT city_name FROM tbl_master_city WHERE city_id = add_city) END AS City1,CASE add_area WHEN '' THEN '' Else(select area_name From tbl_master_area Where area_id = add_area) End AS add_area, area = CAST(add_area as int), tbl_master_address.add_pin AS PinCode, tbl_master_address.add_landMark AS LankMark, case when add_status='N' then 'Deactive' else 'Active' end as status from tbl_master_address where add_cntId='" + Convert.ToString(Session["KeyVal_InternalID_New"]) + "'";
               // Address.SelectCommand = "select DISTINCT  tbl_master_address.add_id AS Id, tbl_master_address.add_addressType AS Type,tbl_master_address.add_address1 AS Address1,  tbl_master_address.add_address2 AS Address2,tbl_master_address.add_address3 AS Address3,tbl_master_address.add_landMark AS LandMark,tbl_master_address.add_country AS Country,tbl_master_address.add_state AS State,tbl_master_address.add_city AS City,tbl_master_address.add_pin as PinCode,CASE add_country WHEN '' THEN '' ELSE(SELECT cou_country FROM tbl_master_country WHERE cou_id = add_country) END AS Country1,CASE add_state WHEN '' THEN '' ELSE(SELECT state FROM tbl_master_state WHERE id = add_state) END AS State1,CASE add_city WHEN '' THEN '' ELSE(SELECT city_name FROM tbl_master_city WHERE city_id = add_city) END AS City1,CASE add_area WHEN '' THEN '' Else(select area_name From tbl_master_area Where area_id = add_area) End AS add_area, area = CAST(add_area as int), CASE add_pin WHEN '' THEN '' ELSE(SELECT pin_code FROM tbl_master_pinzip WHERE pin_id = add_pin) END AS PinCode1, tbl_master_address.add_landMark AS LankMark ,case when add_status='N' then 'Deactive' else 'Active' end as status from tbl_master_address where add_cntId='" + Convert.ToString(Session["KeyVal_InternalID_New"]) + "'";
                Address.SelectCommand = "select DISTINCT  tbl_master_address.add_id AS Id, tbl_master_address.add_addressType AS Type,tbl_master_address.add_address1 AS Address1,  tbl_master_address.add_address2 AS Address2,tbl_master_address.add_address3 AS Address3,tbl_master_address.add_landMark AS LandMark,tbl_master_address.add_country AS Country,tbl_master_address.add_state AS State,tbl_master_address.add_city AS City,CASE isnull(add_country, '')WHEN '' THEN '' ELSE(SELECT cou_country FROM tbl_master_country WHERE cou_id = add_country) END AS Country1,CASE isnull(add_state, '') WHEN '' THEN '' ELSE(SELECT state FROM tbl_master_state WHERE id = add_state) END AS State1,CASE isnull(add_city, '') WHEN '' THEN '' ELSE(SELECT city_name FROM tbl_master_city WHERE city_id = add_city) END AS City1,CASE add_area WHEN '' THEN '' Else(select area_name From tbl_master_area Where area_id = add_area) End AS add_area, area = CAST(add_area as int), CASE add_pin WHEN '' THEN '' ELSE(SELECT pin_code FROM tbl_master_pinzip WHERE pin_id = add_pin) END AS PinCode1,CASE add_pin WHEN '' THEN '' ELSE(SELECT pin_code FROM tbl_master_pinzip WHERE pin_id = add_pin) END AS PinCode, tbl_master_address.add_landMark AS LankMark, case when add_status='N' then 'Deactive' else 'Active' end as status from tbl_master_address where add_cntId='" + Convert.ToString(Session["KeyVal_InternalID_New"]) + "'";
                AddressGrid.DataBind();
            }
        }

        protected void AddressGrid_RowValidating(object sender, DevExpress.Web.Data.ASPxDataValidationEventArgs e)
        {
            // .............................Code Commented and Added by Sam on 08122016 to use Convert.tostring instead of tostring(). ................
            string countryid = "";
            string[,] countryname = null;
            foreach (GridViewColumn column in AddressGrid.Columns)
            {
                GridViewDataColumn dataColumn = column as GridViewDataColumn;
                if (dataColumn == null) continue;
                if (dataColumn.FieldName == "Country")
                {
                    countryid = Convert.ToString(e.NewValues["Country"]);
                    countryname = oDBEngine.GetFieldValue("tbl_master_country", "cou_country", "cou_id=" + countryid, 1);
                }


                if (dataColumn.FieldName == "PinCode")
                {
                    string pin = Convert.ToString(e.NewValues["PinCode"]);

                    
                }
            }
            string Address = Convert.ToString(e.NewValues["Type"]);

            if (e.IsNewRow)
            {
                DataTable dtadd = oDBEngine.GetDataTable("select add_addressType from tbl_master_address where add_cntid='" + Convert.ToString(HttpContext.Current.Session["KeyVal_InternalID"]) + "' and ISNULL(add_status,'Y')='Y'");

                for (int m = 0; m < dtadd.Rows.Count; m++)
                {
                    if (!(Address.Trim().Equals("Billing") || Address.Trim().Equals("Factory/Work/Branch")))
                    {
                        if (Convert.ToString(dtadd.Rows[m]["add_addressType"]) == Convert.ToString(Address))
                        {
                            //if (dtphone.Rows[m]["phf_status"].ToString() == status.ToString())
                            //{if (addtype == "Registered" || addtype == "Residence" || addtype == "Office")
                            if (Address == "Registered")
                            {
                                e.RowError = "[ Registered/Permanent ] Address Type is Already Exist as 'Active'. Select another Address Type to proceed.";
                            }
                            else if (Address == "Residence")
                            {
                                e.RowError = "[ " + Address + " ]" + " Address Type is Already Exist as 'Active'. Select another Address Type to proceed.";
                            }
                            else if (Address == "Office")
                            {
                                e.RowError = "[ " + Address + " ]" + " Address Type is Already Exist as 'Active'. Select another Address Type to proceed.";
                            }

                            // .............................Code Commented and Added by Sam on 08122016 to use populate combobox while validation is false. ................


                            ASPxComboBox combostate = new ASPxComboBox();
                            int countryID = Convert.ToInt32(e.NewValues["Country"]);
                            FillStateCombo(combostate, countryID);

                            ASPxComboBox comcity = new ASPxComboBox();
                            int statid = Convert.ToInt32(e.NewValues["State"]);
                            FillCityCombo(comcity, statid);

                            ASPxComboBox compin = new ASPxComboBox();
                            int cityid = Convert.ToInt32(e.NewValues["City"]);
                            FillPinCombo(compin, cityid);

                            ASPxComboBox comarea = new ASPxComboBox();
                            int pinid = Convert.ToInt32(e.NewValues["PinCode"]);
                            FillAreaCombo(comarea, cityid);

                            // .............................Code Above Commented and Added by Sam on 08122016 to use init the combobox while validation false. ..................................... 

                            return;


                            // }
                        }
                    }
                }
            }
            else
            {
                string addressold = Convert.ToString(e.OldValues["Type"]);
                DataTable dtadd = oDBEngine.GetDataTable("select add_addressType from tbl_master_address where add_cntid='" + Convert.ToString(HttpContext.Current.Session["KeyVal_InternalID"]) + "' and ISNULL(add_status,'Y')='Y'");

                if (addressold != Address)
                {
                    for (int m = 0; m < dtadd.Rows.Count; m++)
                    {
                        if (Convert.ToString(dtadd.Rows[m]["add_addressType"]) == Convert.ToString(Address))
                        {
                            //if (dtphone.Rows[m]["phf_status"].ToString() == status.ToString())
                            //{

                            e.RowError = "[ " + Address + " ]" + "Address Type is Already Exist";
                            return;
                            // }
                        }
                    }
                }

            }

            // .............................Code Above Commented and Added by Sam on 08122016 to use Convert.tostring instead of tostring(). ..................................... 

        }

        protected void ASPxCallbackPanel1_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            // .............................Code Commented and Added by Sam on 08122016 to use Convert.tostring instead of tostring(). ................
            string[] data = e.Parameter.Split('~');
            if (data[0] == "Edit")
            {
                DataTable dtAdd = oDBEngine.GetDataTable("tbl_master_address ", " add_status  ,add_StatusChangeDate,  add_statusChangeReason  ", "add_id ='" + Convert.ToString(data[1]) + "'");
                if (Convert.ToString(dtAdd.Rows[0]["add_status"]) != "")
                {
                    cmbStatus.SelectedValue = Convert.ToString(dtAdd.Rows[0]["add_status"]);
                }
                else
                {
                    cmbStatus.SelectedValue = "N";
                }
                if (Convert.ToString(dtAdd.Rows[0]["add_StatusChangeDate"]) != "")
                {
                    StDate.Value = Convert.ToDateTime(Convert.ToString(dtAdd.Rows[0]["add_StatusChangeDate"]));
                }
                else
                {
                    //StDate.Value = Convert.ToDateTime(oDBEngine.GetDate());
                    StDate.Value = oDBEngine.GetDate();
                }
                txtReason.Text = Convert.ToString(dtAdd.Rows[0]["add_statusChangeReason"]);

            }
            else if (data[0] == "SaveOld")
            {
                if (cmbStatus.SelectedValue == "Y")
                {

                    //........................................... Code Added by Sam on 01112016....................................
                    DataTable dtAdd = oDBEngine.GetDataTable("tbl_master_address ", " add_addressType  ", "add_id ='" + Convert.ToString(data[1]) + "'");
                    string addtype = "";
                    if (dtAdd != null && dtAdd.Rows.Count > 0)
                    {
                        addtype = Convert.ToString(dtAdd.Rows[0]["add_addressType"]);
                    }
                    string intid = "";

                    if (addtype == "Registered" || addtype == "Residence" || addtype == "Office")
                    {
                        if (Session["KeyVal_InternalID"] != null)
                        {
                            intid = Convert.ToString(Session["KeyVal_InternalID"]);
                            DataTable dtallAddtype = oDBEngine.GetDataTable("tbl_master_address ", " add_addressType  ", "add_cntId ='" + intid + "' and add_addressType='" + addtype + "' and (add_status is null or add_status='Y') and add_id !='" + Convert.ToString(data[1]) + "'");
                            if (dtallAddtype != null && dtallAddtype.Rows.Count >= 1)
                            {
                                if (addtype == "Registered")
                                {
                                    Stat = "Y1";
                                }
                                else if (addtype == "Residence")
                                {
                                    Stat = "Y2";
                                }
                                else if (addtype == "Office")
                                {
                                    Stat = "Y3";
                                }
                            }
                            else
                            {
                                int i = oDBEngine.SetFieldValue("tbl_master_address", " add_status='" + cmbStatus.SelectedItem.Value + "'  ,add_StatusChangeDate='" + StDate.Value + "',  add_statusChangeReason='" + txtReason.Text + "'  ", " add_id ='" + Convert.ToString(data[1]) + "'");
                                if (i == 1)
                                {
                                    Stat = "Y";
                                }
                            }

                        }
                        else
                        {
                            if (addtype == "Registered")
                            {
                                Stat = "Y1";
                            }
                            else if (addtype == "Residence")
                            {
                                Stat = "Y2";
                            }
                            else if (addtype == "Office")
                            {
                                Stat = "Y3";
                            }
                            //ScriptManager.RegisterStartupScript(this, this.GetType(), "Notify", "alert('Cannot proceed. Registered/Permanent Address is already exist as ');", true);
                            //return;
                        }

                    }
                    else
                    {
                        int i = oDBEngine.SetFieldValue("tbl_master_address", " add_status='" + cmbStatus.SelectedItem.Value + "'  ,add_StatusChangeDate='" + StDate.Value + "',  add_statusChangeReason='" + txtReason.Text + "'  ", " add_id ='" + Convert.ToString(data[1]) + "'");
                        if (i == 1)
                        {
                            Stat = "Y";
                        }
                    }
                }
                else
                {
                    int i = oDBEngine.SetFieldValue("tbl_master_address", " add_status='" + cmbStatus.SelectedItem.Value + "'  ,add_StatusChangeDate='" + StDate.Value + "',  add_statusChangeReason='" + txtReason.Text + "'  ", " add_id ='" + Convert.ToString(data[1]) + "'");
                    if (i == 1)
                    {
                        Stat = "Y";
                    }
                }





            }

            // .............................Code Above Commented and Added by Sam on 08122016 to use Convert.tostring instead of tostring(). ..................................... 
        }
        protected void ASPxCallbackPanel1_CustomJSProperties(object sender, DevExpress.Web.CustomJSPropertiesEventArgs e)
        {
            e.Properties["cpLast"] = Stat;
        }

        #endregion Address Tab Section end

        #region Phone Tab Section
        protected void PhoneGrid_BeforeGetCallbackResult(object sender, EventArgs e)
        {
            // .............................Code Commented and Added by Sam on 08122016 to use Convert.tostring instead of tostring(). ................

            if (PhoneGrid.IsEditing && !PhoneGrid.IsNewRowEditing)
            {
                if (Session["phtype"] != null)
                {
                    if (Convert.ToString(Session["phtype"]) != "1")
                    {
                        string value = Convert.ToString(PhoneGrid.GetRowValues(PhoneGrid.EditingRowVisibleIndex, "phf_type"));
                        if (value == "Mobile")
                        {
                            //gv.DataColumns["eml_email"].EditFormSettings.Visible = values.Contains(value) ? DefaultBoolean.False : DefaultBoolean.True; 
                            PhoneGrid.DataColumns["phf_countryCode"].EditFormSettings.Visible = DefaultBoolean.False;
                            PhoneGrid.DataColumns["phf_areaCode"].EditFormSettings.Visible = DefaultBoolean.False;
                            PhoneGrid.DataColumns["phf_extension"].EditFormSettings.Visible = DefaultBoolean.False;
                        }
                        else
                        {
                            PhoneGrid.DataColumns["phf_countryCode"].EditFormSettings.Visible = DefaultBoolean.True;
                            PhoneGrid.DataColumns["phf_areaCode"].EditFormSettings.Visible = DefaultBoolean.True;
                            PhoneGrid.DataColumns["phf_extension"].EditFormSettings.Visible = DefaultBoolean.True;
                        }
                    }
                    else
                    {
                        Session["phtype"] = null;
                        string value = Convert.ToString(PhoneGrid.GetRowValues(PhoneGrid.EditingRowVisibleIndex, "phf_type"));
                        if (value == "Mobile")
                        {
                            //gv.DataColumns["eml_email"].EditFormSettings.Visible = values.Contains(value) ? DefaultBoolean.False : DefaultBoolean.True; 
                            PhoneGrid.DataColumns["phf_countryCode"].EditFormSettings.Visible = DefaultBoolean.False;
                            PhoneGrid.DataColumns["phf_areaCode"].EditFormSettings.Visible = DefaultBoolean.False;
                            PhoneGrid.DataColumns["phf_extension"].EditFormSettings.Visible = DefaultBoolean.False;
                        }
                        else
                        {
                            PhoneGrid.DataColumns["phf_countryCode"].EditFormSettings.Visible = DefaultBoolean.True;
                            PhoneGrid.DataColumns["phf_areaCode"].EditFormSettings.Visible = DefaultBoolean.True;
                            PhoneGrid.DataColumns["phf_extension"].EditFormSettings.Visible = DefaultBoolean.True;
                        }
                    }
                }
            }
            // .............................Code Above Commented and Added by Sam on 08122016 to use Convert.tostring instead of tostring(). ..................................... 
        }

        protected void PhoneGrid_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            // .............................Code Commented and Added by Sam on 08122016 to use Convert.tostring instead of tostring(). ................
            string type = "";
            if (e.Parameters != null)
            {
                type = Convert.ToString(e.Parameters);
            }
            if (type == "Mobile")
            {
                //gv.DataColumns["eml_email"].EditFormSettings.Visible = values.Contains(value) ? DefaultBoolean.False : DefaultBoolean.True; 
                PhoneGrid.DataColumns["phf_countryCode"].EditFormSettings.Visible = DefaultBoolean.False;
                PhoneGrid.DataColumns["phf_areaCode"].EditFormSettings.Visible = DefaultBoolean.False;
                PhoneGrid.DataColumns["phf_extension"].EditFormSettings.Visible = DefaultBoolean.False;
            }
            else
            {
                PhoneGrid.DataColumns["phf_countryCode"].EditFormSettings.Visible = DefaultBoolean.True;
                PhoneGrid.DataColumns["phf_areaCode"].EditFormSettings.Visible = DefaultBoolean.True;
                PhoneGrid.DataColumns["phf_extension"].EditFormSettings.Visible = DefaultBoolean.True;
            }

            if (type != "")
            {
                Session["phtype"] = "1";
                //string value = EmailGrid.GetRowValues(EmailGrid.EditingRowVisibleIndex, "eml_type").ToString();

            }
            if (e.Parameters != null)
            {

                Phone.SelectCommand = "select DISTINCT phf_id,phf_cntId,phf_entity,phf_type,phf_countryCode,phf_areaCode,phf_phoneNumber,phf_extension, ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' + ISNULL(phf_phoneNumber, '') +' '+ ISNULL(phf_extension, '') + ISNULL(phf_faxNumber, '') AS Number,case when phf_Status='N' then 'Deactive' else 'Active' end as status ,isnull(phf_SMSFacility,'') as  phf_SMSFacility from tbl_master_phonefax where phf_cntId='" + Convert.ToString(Session["KeyVal_InternalID_New"]) + "'";
                PhoneGrid.DataBind();
            }
            // .............................Code Above Commented and Added by Sam on 08122016 to use Convert.tostring instead of tostring(). ..................................... 
        }

        protected void PhoneGrid_RowValidating(object sender, DevExpress.Web.Data.ASPxDataValidationEventArgs e)
        {
            // .............................Code Commented and Added by Sam on 08122016 to use Convert.tostring instead of tostring(). ................

            string Address = Convert.ToString(e.NewValues["phf_type"]);
            Session["phtype"] = null;
            if (Address == "Mobile")
            {
                //gv.DataColumns["eml_email"].EditFormSettings.Visible = values.Contains(value) ? DefaultBoolean.False : DefaultBoolean.True; 
                PhoneGrid.DataColumns["phf_countryCode"].EditFormSettings.Visible = DefaultBoolean.False;
                PhoneGrid.DataColumns["phf_areaCode"].EditFormSettings.Visible = DefaultBoolean.False;
                PhoneGrid.DataColumns["phf_extension"].EditFormSettings.Visible = DefaultBoolean.False;
            }
            else
            {
                PhoneGrid.DataColumns["phf_countryCode"].EditFormSettings.Visible = DefaultBoolean.True;
                PhoneGrid.DataColumns["phf_areaCode"].EditFormSettings.Visible = DefaultBoolean.True;
                PhoneGrid.DataColumns["phf_extension"].EditFormSettings.Visible = DefaultBoolean.True;
            }
            if (e.IsNewRow)
            {
                DataTable dtphonefax = oDBEngine.GetDataTable("select phf_Type from tbl_master_phonefax where phf_cntid='" + Convert.ToString(HttpContext.Current.Session["KeyVal_InternalID"]) + "' and ISNULL(phf_status,'Y')='Y'");

                for (int m = 0; m < dtphonefax.Rows.Count; m++)
                {
                    if (Convert.ToString(dtphonefax.Rows[m]["phf_type"]) == Convert.ToString(Address))
                    {
                        e.RowError = "[ " + Address + " ]" + "Phone Type is Already Exist";
                        return;
                    }
                }
            }
            else
            {
                string addressold = Convert.ToString(e.OldValues["phf_type"]);
                DataTable dtphonefax = oDBEngine.GetDataTable("select phf_Type from tbl_master_phonefax where phf_cntid='" + Convert.ToString(HttpContext.Current.Session["KeyVal_InternalID"]) + "' and ISNULL(phf_status,'Y')='Y'");

                if (addressold != Address)
                {
                    for (int m = 0; m < dtphonefax.Rows.Count; m++)
                    {
                        if (Convert.ToString(dtphonefax.Rows[m]["phf_type"]) == Convert.ToString(Address))
                        {
                            e.RowError = "[ " + Address + " ]" + "Phone Type is Already Exist";
                            return;

                        }
                    }
                }

            }



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
            // .............................Code Above Commented and Added by Sam on 08122016 to use Convert.tostring instead of tostring(). ..................................... 
        }

        protected void ASPxCallbackPanelP_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            // .............................Code Commented and Added by Sam on 08122016 to use Convert.tostring instead of tostring(). ................
            string[] data = e.Parameter.Split('~');
            if (data[0] == "Edit")
            {
                DataTable dtAdd = oDBEngine.GetDataTable("tbl_master_phonefax ", "  phf_Status  ,phf_StatusChangeDate,  phf_StatusChangeReason ", "phf_id ='" + Convert.ToString(data[1]) + "'");
                if (Convert.ToString(dtAdd.Rows[0]["phf_Status"]) != "")
                {
                    cmbStatusP.SelectedValue = Convert.ToString(dtAdd.Rows[0]["phf_Status"]);
                }
                else
                {
                    cmbStatusP.SelectedValue = "N";
                }
                if (Convert.ToString(dtAdd.Rows[0]["phf_StatusChangeDate"]) != "")
                {
                    StDateP.Value = Convert.ToDateTime(Convert.ToString(dtAdd.Rows[0]["phf_StatusChangeDate"]));
                }
                else
                {
                    //StDateP.Value = Convert.ToDateTime(oDBEngine.GetDate());
                    StDateP.Value = oDBEngine.GetDate();
                }
                txtReasonP.Text = Convert.ToString(dtAdd.Rows[0]["phf_StatusChangeReason"]);

            }
            else if (data[0] == "SaveOld")
            {
                int i = oDBEngine.SetFieldValue("tbl_master_phonefax", " phf_Status='" + cmbStatusP.SelectedItem.Value + "'  ,phf_StatusChangeDate='" + StDateP.Value + "',  phf_StatusChangeReason='" + txtReasonP.Text + "'  ", " phf_id ='" + Convert.ToString(data[1]) + "'");
                if (i == 1)
                {
                    Stat = "Y";
                }
            }

            // .............................Code Above Commented and Added by Sam on 08122016 to use Convert.tostring instead of tostring(). ..................................... 
        }
        protected void ASPxCallbackPanelP_CustomJSProperties(object sender, DevExpress.Web.CustomJSPropertiesEventArgs e)
        {
            e.Properties["cpLast"] = Stat;
        }

        #endregion Phone Tab Section end

        #region Email Tab Section
        protected void EmailGrid_BeforeGetCallbackResult(object sender, EventArgs e)
        {
          

        }
        protected void EmailGrid_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {

            
            // .............................Code Commented and Added by Sam on 08122016 to use Convert.tostring instead of tostring(). ................
            if (e.Parameters != null)
            {
                Email.SelectCommand = "select eml_id,eml_cntId,eml_entity,eml_type,eml_email,eml_ccEmail,eml_website,CreateDate,CreateUser,case when eml_Status='N' then 'Deactive' else 'Active' end as status from tbl_master_email where eml_cntId='" + Convert.ToString(Session["KeyVal_InternalID_New"]) + "'";

                EmailGrid.DataBind();
            }
            // .............................Code Above Commented and Added by Sam on 08122016 to use Convert.tostring instead of tostring(). ..................................... 
        }

        protected void EmailGrid_CustomJSProperties(object sender, DevExpress.Web.ASPxGridViewClientJSPropertiesEventArgs e)
        {
            e.Properties["cpHeight"] = emailid;
        }
        protected void EmailGrid_CancelRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            Session["edit"] = null;
        }

        protected void EmailGrid_RowValidating(object sender, DevExpress.Web.Data.ASPxDataValidationEventArgs e)
        {
            // .............................Code Commented and Added by Sam on 08122016 to use Convert.tostring instead of tostring(). ................
            // ............. Code Commented by Sam on 07112016 due to Flikering Popup....................
            string emailtype = Convert.ToString(e.NewValues["eml_type"]);
           

            // ............. Code Commented by Sam on 07112016 due to Flikering Popup....................

            if (e.IsNewRow)
            {
                DataTable dtemail = oDBEngine.GetDataTable("select eml_type from tbl_master_email where eml_cntid='" + Convert.ToString(HttpContext.Current.Session["KeyVal_InternalID"]) + "'  and ISNULL(eml_status,'Y')='Y'");

                for (int m = 0; m < dtemail.Rows.Count; m++)
                {
                    if (Convert.ToString(dtemail.Rows[m]["eml_type"]) == Convert.ToString(emailtype))
                    {
                        e.RowError = "[ " + emailtype + " ]" + "Email Type is Already Exist";
                        return;
                    }
                }
            }
            else
            {
                string addressold = Convert.ToString(e.OldValues["eml_type"]);
                DataTable dtemail = oDBEngine.GetDataTable("select eml_type from tbl_master_email where eml_cntid='" + Convert.ToString(HttpContext.Current.Session["KeyVal_InternalID"]) + "'  and ISNULL(eml_status,'Y')='Y'");

                if (addressold != emailtype)
                {
                    for (int m = 0; m < dtemail.Rows.Count; m++)
                    {
                        if (Convert.ToString(dtemail.Rows[m]["eml_type"]) == Convert.ToString(emailtype))
                        {
                            e.RowError = "[ " + emailtype + " ]" + "Email Type is Already Exist";
                            
                            return;
                        }
                    }
                }

            }



            string ccEmail = "";
            string email = "";
            string emlentity = "";
            string emltype = "";
            string emlcntid = "";

            emltype = Convert.ToString(e.NewValues["eml_type"]);
            emlcntid = Convert.ToString(HttpContext.Current.Session["KeyVal_InternalID"]);
            emlentity = Convert.ToString(HttpContext.Current.Session["ContactType"]);
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
            // string[,] emailCheck = oDBEngine.GetFieldValue("tbl_master_email", "eml_email", "eml_type='" + emltype + "' and eml_entity='" + emlentity + "' and eml_cntId='" + emlcntid + "' and eml_email='" + email + "'", 1);
            string[,] emailCheck = oDBEngine.GetFieldValue("tbl_master_email", "eml_email", "eml_cntId<>'" + emlcntid + "' and eml_email='" + email + "'", 1);
            string email1 = "";
            if (emailCheck[0, 0] != "n")
            {
                email1 = emailCheck[0, 0];
            }
            //string[,] ccEmailCheck = oDBEngine.GetFieldValue("tbl_master_email", "eml_ccEmail", " eml_ccEmail='" + ccEmail + "'", 1);
            string[,] ccEmailCheck = oDBEngine.GetFieldValue("tbl_master_email", "eml_ccEmail", " eml_cntId<>'" + emlcntid + "' and  eml_ccEmail='" + ccEmail + "'", 1);
            string ccEmail1 = "";
            if (ccEmailCheck[0, 0] != "n")
            {
                ccEmail1 = ccEmailCheck[0, 0];
            }
            if (email1 == "" && ccEmail1 == "")
            {
                emailid = "b";
            }
            else
            {
                emailid = "c";
                e.RowError = "Email Id Already Exists";
                return;
            }
            string emltype1 = Convert.ToString(e.NewValues["eml_type"]);
            if (emltype1 == "Official")
            {

                if (e.NewValues["eml_facility"] == null)
                {
                    e.NewValues["eml_facility"] = "";
                    string emlsmstype = Convert.ToString(e.NewValues["eml_facility"]);
                    if (emlsmstype.Length == 0)
                    {
                        // e.RowError = "Please Select Email Alert type";
                        return;
                    }
                }

            }
            // .............................Code Above Commented and Added by Sam on 08122016 to use Convert.tostring instead of tostring(). ..................................... 
        }

        protected void ASPxCallbackPanelE_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            // .............................Code Commented and Added by Sam on 08122016 to use Convert.tostring instead of tostring(). ................
            string[] data = e.Parameter.Split('~');
            if (data[0] == "Edit")
            {
                DataTable dtAdd = oDBEngine.GetDataTable("tbl_master_email ", "   eml_Status  ,eml_StatusChangeDate,  eml_StatusChangeReason ", "eml_id ='" + Convert.ToString(data[1]) + "'");
                if (Convert.ToString(dtAdd.Rows[0]["eml_Status"]) != "N")
                {
                    //cmbStatusE.SelectedValue = dtAdd.Rows[0]["eml_Status"].ToString();
                    cmbStatusE.SelectedValue = "Y";
                }
                else
                {
                    cmbStatusE.SelectedValue = "N";
                }
                if (Convert.ToString(dtAdd.Rows[0]["eml_StatusChangeDate"]) != "")
                {
                    StDateE.Value = Convert.ToDateTime(Convert.ToString(dtAdd.Rows[0]["eml_StatusChangeDate"]));
                }
                else
                {
                    //StDateE.Value = Convert.ToDateTime(oDBEngine.GetDate());
                    StDateE.Value = oDBEngine.GetDate();
                }
                txtReasonE.Text = Convert.ToString(dtAdd.Rows[0]["eml_StatusChangeReason"]);

            }
            else if (data[0] == "SaveOld")
            {
                int i = oDBEngine.SetFieldValue("tbl_master_email", " eml_Status='" + cmbStatusE.SelectedItem.Value + "'  ,eml_StatusChangeDate='" + StDateE.Value + "',  eml_StatusChangeReason='" + txtReasonE.Text + "'  ", " eml_id ='" + Convert.ToString(data[1]) + "'");
                if (i == 1)
                {
                    Stat = "Y";
                }
            }
            // .............................Code Above Commented and Added by Sam on 08122016 to use Convert.tostring instead of tostring(). ..................................... 

        }
        protected void ASPxCallbackPanelE_CustomJSProperties(object sender, DevExpress.Web.CustomJSPropertiesEventArgs e)
        {
            e.Properties["cpLast"] = Stat;
        }

        #endregion Email Tab Section end


        //Purpose : Add rights to address tab in  corespondence tab 
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
        public void bindexport(int Filter)
        {
            string filename = "BankCorrespondence";
            exporter.FileName = filename;

            exporter.PageHeader.Left = "Bank Correspondence";
            exporter.PageFooter.Center = "[Page # of Pages #]";
            exporter.PageFooter.Right = "[Date Printed]";
           // AddressGrid.Columns[7].Visible = false;
            switch (Filter)
            {
                case 1:
                    exporter.WritePdfToResponse();
                    //bindpdf();
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
           
            //Int32 Filter = int.Parse(Convert.ToString(drdExport.SelectedItem.Value));
            //if (Filter != 0)
            //{
            //    if (Session["exportval"] == null)
            //    {
            //        Session["exportval"] = Filter;
            //        bindexport(Filter);
            //    }
            //    else if (Convert.ToInt32(Session["exportval"]) != Filter)
            //    {
            //        Session["exportval"] = Filter;
            //        bindexport(Filter);
            //    }
            //}
           
            
        }




        public void bindpdf()
        {
            // .............................Code Commented and Added by Sam on 07122016 to use Convert.tostring instead of tostring(). ................
            //using (MemoryStream ms = new MemoryStream())
            //{
            //    PrintableComponentLink pcl = new PrintableComponentLink(new PrintingSystem());
            //    pcl.Component = exporter;
            //    pcl.Margins.Left = pcl.Margins.Right = 50;
            //    pcl.Landscape = true;
            //    pcl.CreateDocument(false);
            //    pcl.PrintingSystem.Document.AutoFitToPagesWidth = 1;
            //    pcl.ExportToPdf(ms);
            //    WriteResponse(this.Response, ms.ToArray(), Convert.ToString(System.Net.Mime.DispositionTypeNames.Inline));
            //}
            // .............................Code Above Commented and Added by Sam on 07122016 to use Convert.tostring instead of tostring(). ..................................... 
        }
        public static void WriteResponse(HttpResponse response, byte[] filearray, string type)
        {
            // .............................Code Commented and Added by Sam on 07122016 to use Convert.tostring instead of tostring(). ................
            response.ClearContent();
            response.Buffer = true;
            response.Cache.SetCacheability(HttpCacheability.Private);
            response.ContentType = "application/pdf";
            ContentDisposition contentDisposition = new ContentDisposition();
            contentDisposition.FileName = "test.pdf";
            contentDisposition.DispositionType = type;
            response.AddHeader("Content-Disposition", Convert.ToString(contentDisposition));
            response.BinaryWrite(filearray);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
            try
            {
                response.End();
            }
            catch (System.Threading.ThreadAbortException)
            {
            }
            // .............................Code Above Commented and Added by Sam on 07122016 to use Convert.tostring instead of tostring(). ..................................... 
        }


    }
}





