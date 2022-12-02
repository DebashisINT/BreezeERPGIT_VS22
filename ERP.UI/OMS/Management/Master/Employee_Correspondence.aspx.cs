using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
////using DevExpress.Web.ASPxClasses;
//////using DevExpress.Web;
using DevExpress.Web;
using DevExpress.Web;
using BusinessLogicLayer;
using System.Text.RegularExpressions;
using EntityLayer.CommonELS;

namespace ERP.OMS.Management.Master
{
    public partial class management_master_Employee_Correspondence : ERP.OMS.ViewState_class.VSPage
    {
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        string emailid = "a";
        String Stat = "N";
        protected void Page_Init(object sender, EventArgs e)
        {
            Address.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            CountrySelect.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            SelectPin.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            SelectCity.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            StateSelect.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            SelectArea.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            Phone.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            Email.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Master/employee.aspx");
            
            if (!IsPostBack)
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
                        //ASPxPageControl1.TabPages.FindByName("RESIGNATION").Visible = true;
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
            string cnttype = "employee";
            string intid = Convert.ToString(HttpContext.Current.Session["KeyVal_InternalID"]);
            //DataTable dtcmp = oDBEngine.GetDataTable(" tbl_master_address  ", "*", "add_cntId='" + intid + "'");
            //if (dtcmp.Rows.Count == 0)
            //{
            //    try
            //    {
            //        String con = ConfigurationSettings.AppSettings["DBConnectionDefault"];
            //        SqlConnection lcon = new SqlConnection(con);
            //        lcon.Open();
            //        SqlCommand lcmdEmplInsert = new SqlCommand("AdressDummyInsert", lcon);
            //        lcmdEmplInsert.CommandType = CommandType.StoredProcedure;
            //        lcmdEmplInsert.Parameters.AddWithValue("@contacttype", cnttype.ToString().Trim());
            //        lcmdEmplInsert.Parameters.AddWithValue("@InternalId", intid);
            //        lcmdEmplInsert.ExecuteNonQuery();
            //    }
            //    catch
            //    {
            //    }
            //}
            

            //bellow line commented by debjyoti (pin code changed)
            //Address.SelectCommand = "select DISTINCT  tbl_master_address.add_id AS Id, tbl_master_address.add_addressType AS Type,tbl_master_address.add_address1 AS Address1,  tbl_master_address.add_address2 AS Address2,tbl_master_address.add_address3 AS Address3,tbl_master_address.add_landMark AS LandMark,tbl_master_address.add_country AS Country,tbl_master_address.add_state AS State,tbl_master_address.add_city AS City,CASE isnull(add_country, '')WHEN '' THEN '' ELSE(SELECT cou_country FROM tbl_master_country WHERE cou_id = add_country) END AS Country1,CASE isnull(add_state, '') WHEN '' THEN '' ELSE(SELECT state FROM tbl_master_state WHERE id = add_state) END AS State1,CASE isnull(add_city, '') WHEN '' THEN '' ELSE(SELECT city_name FROM tbl_master_city WHERE city_id = add_city) END AS City1,CASE add_area WHEN '' THEN '0' Else(select area_name From tbl_master_area Where area_id = add_area) End AS add_area,area = CAST(add_area as int),tbl_master_address.add_pin AS PinCode, tbl_master_address.add_landMark AS LankMark, case when add_status='N' then 'Deactive' else 'Active' end as status from tbl_master_address where add_cntId='" + HttpContext.Current.Session["KeyVal_InternalID"] + "'";
            Address.SelectCommand = "select DISTINCT  tbl_master_address.add_id AS Id, tbl_master_address.add_addressType AS Type,tbl_master_address.add_address1 AS Address1,  tbl_master_address.add_address2 AS Address2,tbl_master_address.add_address3 AS Address3,tbl_master_address.add_landMark AS LandMark,tbl_master_address.add_country AS Country,tbl_master_address.add_state AS State,tbl_master_address.add_city AS City,CASE isnull(add_country, '')WHEN '' THEN '' ELSE(SELECT cou_country FROM tbl_master_country WHERE cou_id = add_country) END AS Country1,CASE isnull(add_state, '') WHEN '' THEN '' ELSE(SELECT state FROM tbl_master_state WHERE id = add_state) END AS State1,CASE isnull(add_city, '') WHEN '' THEN '' ELSE(SELECT city_name FROM tbl_master_city WHERE city_id = add_city) END AS City1,CASE add_area WHEN '' THEN '0' Else(select area_name From tbl_master_area Where area_id = add_area) End AS add_area,area = CAST(add_area as int),CASE isnull(add_pin, '') WHEN '' THEN '' ELSE(SELECT pin_code FROM tbl_master_pinzip WHERE pin_id = add_pin) END AS PinCode,CASE isnull(add_pin, '') WHEN '' THEN '' ELSE(SELECT pin_code FROM tbl_master_pinzip WHERE pin_id = add_pin) END AS PinCode1, tbl_master_address.add_landMark AS LankMark, case when add_status='N' then 'Deactive' else 'Active' end as status from tbl_master_address where add_cntId='" + HttpContext.Current.Session["KeyVal_InternalID"] + "'";


        }
        protected void AddressGrid_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            AddressGrid.SettingsText.PopupEditFormCaption = "Modify Address";
        }

        protected void AddressGrid_HtmlEditFormCreated(object sender, DevExpress.Web.ASPxGridViewEditFormEventArgs e)
        {
            AddressGrid.SettingsText.PopupEditFormCaption = "Add Address";
        }

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
            cmb.Items.Insert(0, new ListEditItem("Select", "0"));
        }

        string[,] GetState(int country)
        {
            StateSelect.SelectParameters[0].DefaultValue =Convert.ToString(country);
            DataView view = (DataView)StateSelect.Select(DataSourceSelectArguments.Empty);
            string[,] DATA = new string[view.Count, 2];
            for (int i = 0; i < view.Count; i++)
            {
                DATA[i, 0] =Convert.ToString( view[i][0]);
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
            cmb.Items.Insert(0, new ListEditItem("Select", "0"));
        }

        string[,] GetCities(int state)
        {


            SelectCity.SelectParameters[0].DefaultValue = Convert.ToString(state);
            DataView view = (DataView)SelectCity.Select(DataSourceSelectArguments.Empty);
            string[,] DATA = new string[view.Count, 2];
            for (int i = 0; i < view.Count; i++)
            {
                DATA[i, 0] = Convert.ToString(view[i][0]);
                DATA[i, 1] =Convert.ToString( view[i][1]);
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
            string email = "";
            string emlentity = "";
            string emltype = "";
            string emlcntid = "";
            string isnewornot=emailisnew.Value;
            emltype = Convert.ToString(e.NewValues["eml_type"]);
            emlcntid = Convert.ToString(HttpContext.Current.Session["KeyVal_InternalID"]);
            emlentity = Convert.ToString(HttpContext.Current.Session["ContactType"]);
            bool ischeckornot = Convert.ToBoolean(e.NewValues["Isdefault"]);
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
                
                string[,] emailCheck = oDBEngine.GetFieldValue("tbl_master_email", "eml_email", " eml_entity='employee' and eml_email='" + email + "' and  eml_cntId<>'" + emlcntid + "' and eml_id<>'" + emailID + "'", 1);
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
                

                string[,] emailCheck = oDBEngine.GetFieldValue("tbl_master_email", "eml_email", " eml_entity='employee' and eml_email='" + email + "' and  eml_cntId<>'" + emlcntid + "'", 1);
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
            // Default value validation

            if (e.IsNewRow && isnewornot == "newinsert")
            {
                if (ischeckornot)
                {
                    //string emailIDisdefault = Convert.ToString(e.Keys[0]);
                    string[,] emailisdefaultCheck = oDBEngine.GetFieldValue("tbl_master_email", "Isdefault", " eml_entity='employee' and  eml_cntId='" + emlcntid + "' ", 1);
                    string isdefault = "";
                    if (emailisdefaultCheck[0, 0] != "n" && emailisdefaultCheck[0, 0] == "True")
                    {
                        isdefault = emailisdefaultCheck[0, 0];
                    }
                    if (string.IsNullOrEmpty(isdefault))
                    {

                    }
                    else
                    {
                        e.RowError = "Default Email Id is already set";
                        return;
                    }
                }
            }
            else {
                if (ischeckornot)
                {
                    string emailIDisdefault = Convert.ToString(e.Keys[0]);
                    string[,] emailisdefaultCheck = oDBEngine.GetFieldValue("tbl_master_email", "Isdefault", " eml_entity='employee' and  eml_cntId='" + emlcntid + "' and eml_type<>'" + emltype + "'", 1);
                    string isdefault = "";
                    if (emailisdefaultCheck[0, 0] != "n" && emailisdefaultCheck[0, 0] == "True")
                    {
                        isdefault = emailisdefaultCheck[0, 0];
                    }
                    if (string.IsNullOrEmpty(isdefault))
                    {

                    }
                    else
                    {
                        e.RowError = "Default Email Id is already set";
                        return;
                    }
                }
            }

           
        }
        protected void PhoneGrid_RowValidating(object sender, DevExpress.Web.Data.ASPxDataValidationEventArgs e)
        {
            string PhoneType = Convert.ToString(e.NewValues["phf_type"]);
            if (PhoneType == "Mobile")
            {
                string PhoneNumber =Convert.ToString( e.NewValues["phf_phoneNumber"]);
                if (PhoneNumber.Length != 10)
                {
                    e.RowError = "Enter Valid Mobile Number";
                    return;
                }
            }
        }

        protected void AddressGrid_RowValidating(object sender, DevExpress.Web.Data.ASPxDataValidationEventArgs e)
        {
            //string country = string.Empty;
            //country = e.NewValues["Country"].ToString();

            //var pinCode = e.NewValues["PinCode"].ToString();
            //if (country.ToLower() == "1")
            //{
            //    if (pinCode.Length != 6 || !Regex.IsMatch(pinCode, "^[0-9]*$"))
            //    {
            //        //Regex.IsMatch(pinCode, @"[0-9]{6}")
            //        e.RowError = "Pincode should be 6 digits";
            //        return;
            //    }
            //    //else if (!Regex.IsMatch(pinCode, @"[0-9]{6}"))
            //    //{ }     
            //}
            //else if (country.ToLower() != "1")
            //{
            //    if (pinCode.Length > 16)
            //    {
            //        e.RowError = "Pincode should be 16 characters";
            //        return;
            //    }
            //}

             string Address = Convert.ToString(e.NewValues["Type"]);
            
            if (e.IsNewRow)
            {
                DataTable dtadd = oDBEngine.GetDataTable("select add_addressType from tbl_master_address where add_cntid='" + Convert.ToString(HttpContext.Current.Session["KeyVal_InternalID"]) + "' and ISNULL(add_status,'Y')='Y'");

                for (int m = 0; m < dtadd.Rows.Count; m++)
                {
                    if (!(Address.Trim().Equals("Billing") || Address.Trim().Equals("Factory/Work/Branch")))
                    {
                        if (Convert.ToString(dtadd.Rows[m]["add_addressType"]) == Address.ToString())
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
            else
            {
                string addressold = Convert.ToString(e.OldValues["Type"]);
                DataTable dtadd = oDBEngine.GetDataTable("select add_addressType from tbl_master_address where add_cntid='" + Convert.ToString(HttpContext.Current.Session["KeyVal_InternalID"]) + "' and ISNULL(add_status,'Y')='Y'");

                if (addressold != Address)
                {
                    for (int m = 0; m < dtadd.Rows.Count; m++)
                    {
                        if (Convert.ToString(dtadd.Rows[m]["add_addressType"]) == Address.ToString())
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

                   

                    //if (countryname[0, 0] == "India")
                    //{
                    //    double Num;
                    //    bool isNum = double.TryParse(pin, out Num);
                    //    int len = pin.Length;
                    //    if (!isNum || len != 6)
                    //    {
                    //        // e.Errors[dataColumn] = "Enter Valid PinCode";
                    //        e.RowError = "Enter Valid PinCode";
                    //        return;
                    //    }
                    //}
                    //if (Convert.ToString(e.NewValues["PinCode"]) == "111")
                    //    e.Errors[dataColumn] = "Value cannot be null.";
                }
            }



        }

        protected void PhoneGrid_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            int id = Convert.ToInt32(Convert.ToString(e.EditingKeyValue));
            Session["id"] = id;
            PhoneGrid.SettingsText.PopupEditFormCaption = "Modify Phone";
        }
   
        protected void EmailGrid_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            EmailGrid.SettingsText.PopupEditFormCaption = "Modify Email";
        }
     
        //Purpose: User rights in Address rab
        //Name : Debjyoti 17-11-2016

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
        //End : debjyoti 17-11-2016
    }
}