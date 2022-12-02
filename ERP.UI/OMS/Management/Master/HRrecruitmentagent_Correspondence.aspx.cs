  
using System;
using System.Data;
using System.Web;
using System.Web.UI;
using DevExpress.Web;
////using DevExpress.Web.ASPxClasses;
//using DevExpress.Web;
using System.Configuration;
using BusinessLogicLayer;
using EntityLayer.CommonELS;
using System.Web.Services;
using System.Web.Script.Services;
using System.Linq;
using DataAccessLayer; 

namespace ERP.OMS.Management.Master
{
    public partial class management_master_HRrecruitmentagent_Correspondence : ERP.OMS.ViewState_class.VSPage //System.Web.UI.Page
    {
        //DBEngine oDBEngine = new DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        string emailid = "a";
        String Stat = "N";
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

        //bellow one line code added by debjyoti 
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();

        protected void Page_Init(object sender, EventArgs e)
        {
            Address.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            CountrySelect.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            StateSelect.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            SelectCity.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            SelectArea.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            SelectPin.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            Phone.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            Email.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Master/HRrecruitmentagent.aspx?requesttype=VendorService");
          
            if (!IsPostBack)
            {
                string[,] EmployeeNameID = oDBEngine.GetFieldValue(" tbl_master_contact ", " case when cnt_firstName is null then '' else cnt_firstName end + ' '+case when cnt_middleName is null then '' else cnt_middleName end+ ' '+case when cnt_lastName is null then '' else cnt_lastName end+' ['+cnt_shortName+']' as name ", " cnt_internalId='" + HttpContext.Current.Session["KeyVal_InternalID"] + "'", 1);
                if (EmployeeNameID[0, 0] != "n")
                {
                    lblHeader.Text = EmployeeNameID[0, 0].ToUpper();
                }
            }
        }
        protected void AddressGrid_CellEditorInitialize(object sender, DevExpress.Web.ASPxGridViewEditorEventArgs e)
        {
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
                    int val = 0;
                    try
                    {
                        val = Convert.ToInt32(AddressGrid.GetRowValues(0, "Country"));
                    }
                    catch (Exception ex)
                    {

                    }
                    if (val != 0)
                    {

                        int country = (int)val;
                        ASPxComboBox combo = e.Editor as ASPxComboBox;
                        FillStateCombo(combo, country);

                        combo.Callback += new CallbackEventHandlerBase(cmbState_OnCallback);
                    }
                    else
                    {

                        int country = 1;
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
                    object val = AddressGrid.GetRowValuesByKeyValue(e.KeyValue, "State");
                    if (val == DBNull.Value) return;
                    int state = (int)val;
                    ASPxComboBox combo = e.Editor as ASPxComboBox;
                    FillCityCombo(combo, state);

                    combo.Callback += new CallbackEventHandlerBase(cmbCity_OnCallback);
                }
                else
                {

                    // object val = AddressGrid.GetRowValues(0, "State");

                    int val = 0;
                    try
                    {
                        val = Convert.ToInt32(AddressGrid.GetRowValues(0, "State"));
                    }
                    catch (Exception ex)
                    {

                    }
                    if (val != 0)
                    {
                        //}
                        //if (val == DBNull.Value)
                        //{

                        int state = (int)val;
                        ASPxComboBox combo = e.Editor as ASPxComboBox;
                        FillCityCombo(combo, state);

                        combo.Callback += new CallbackEventHandlerBase(cmbCity_OnCallback);
                    }
                    else
                    {

                        int state = 1;
                        ASPxComboBox combo = e.Editor as ASPxComboBox;
                        FillCityCombo(combo, state);

                        combo.Callback += new CallbackEventHandlerBase(cmbCity_OnCallback);
                    }
                }
            }
            if (e.Column.FieldName == "area")
            {
                if (e.KeyValue != null)
                {
                    //object val = AddressGrid.GetRowValuesByKeyValue(e.KeyValue, "City");
                    int val = 0;
                    try
                    {
                        val = Convert.ToInt32(AddressGrid.GetRowValuesByKeyValue(e.KeyValue, "City"));
                    }
                    catch (Exception ex)
                    {

                    }
                    if (val == 0) return;
                    if (val != 0)
                    {
                        int city = (int)val;
                        ASPxComboBox combo = e.Editor as ASPxComboBox;
                        FillAreaCombo(combo, city);
                        combo.Callback += new CallbackEventHandlerBase(cmbArea_OnCallback);
                    }
                }
                else
                {

                    // object val = AddressGrid.GetRowValues(0, "City");
                    int val = 0;
                    try
                    {
                        val = Convert.ToInt32(AddressGrid.GetRowValues(0, "City"));
                    }
                    catch (Exception ex)
                    {

                    }
                    if (val == 0) return;
                    if (val != 0)
                    {
                        int city = (int)val;
                        ASPxComboBox combo = e.Editor as ASPxComboBox;
                        FillAreaCombo(combo, city);
                        combo.Callback += new CallbackEventHandlerBase(cmbArea_OnCallback);
                    }
                    else
                    {

                        int city = 1;
                        ASPxComboBox combo = e.Editor as ASPxComboBox;
                        FillAreaCombo(combo, city);

                        combo.Callback += new CallbackEventHandlerBase(cmbArea_OnCallback);
                    }
                }
            }

            //debjyoti 05-12-2016
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

            //end debjyoti 05-12-2016
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
            //Purpose : Replace .ToString() with Convert.ToString(..)
            //Name : Sudip 
            // Dated : 22-12-2016

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
            // Dated : 22-12-2016

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
        //debjyoti 05-12-2016
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
            // Dated : 22-12-2016

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
            // Dated : 22-12-2016

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
        private void cmbState_OnCallback(object source, CallbackEventArgsBase e)
        {
            FillStateCombo(source as ASPxComboBox, Convert.ToInt32(e.Parameter));
        }
        private void cmbCity_OnCallback(object source, CallbackEventArgsBase e)
        {
            FillCityCombo(source as ASPxComboBox, Convert.ToInt32(e.Parameter));
        }

        //debjyoti 05-12-2016
        private void cmbPin_OnCallback(object source, CallbackEventArgsBase e)
        {
            FillPinCombo(source as ASPxComboBox, Convert.ToInt32(e.Parameter));
        }
        //End debjyoti 05-12-2016
        protected void ASPxPageControl2_ActiveTabChanged(object source, TabControlEventArgs e)
        {

        }
        protected void AddressGrid_RowValidating(object sender, DevExpress.Web.Data.ASPxDataValidationEventArgs e)
        {
            //Purpose : Replace .ToString() with Convert.ToString(..)
            //Name : Sudip 
            // Dated : 22-12-2016

            string Address = string.Empty;
            Int32 Isdefault = 0;
            string flag = string.Empty;
            try
            {
                Isdefault = Convert.ToInt32(e.NewValues["Isdefault"]);
                if (Isdefault == 1)
                {
                    flag = "True";
                }
                else
                {
                    flag = "False";
                }
                Address = Convert.ToString(e.NewValues["Type"]);
            }
            catch (Exception ex)
            {
                Address = "''";
            }
            if (e.IsNewRow)
            {
                DataTable dtadd = oDBEngine.GetDataTable("select add_addressType,ISNULL(Isdefault,0) Isdefault from tbl_master_address where add_cntid='" + Convert.ToString(HttpContext.Current.Session["KeyVal_InternalID"]) + "' and ISNULL(add_status,'Y')='Y'");

                for (int m = 0; m < dtadd.Rows.Count; m++)
                {
                    //Rev Rajdip
                    //if (Convert.ToString(dtadd.Rows[m]["add_addressType"]) == Convert.ToString(Address))
                    //{
                    //    //if (dtphone.Rows[m]["phf_status"].ToString() == status.ToString())
                    //    //{

                    //    e.RowError = "[ " + Address + " ]" + "Address Type is Already Exist";
                    //    return;

                    //    // }
                    //}
                    if ((Convert.ToString(dtadd.Rows[m]["add_addressType"]) == Convert.ToString(Address)) && (Convert.ToString(dtadd.Rows[m]["Isdefault"]) == Convert.ToString(flag)) && (flag == "True") && (Convert.ToString(dtadd.Rows[m]["Isdefault"]) == "True"))
                    {
                        //if (dtphone.Rows[m]["phf_status"].ToString() == status.ToString())
                        //{

                        e.RowError = "[ " + Address + " ]" + "Address Type is Already Exist With default";
                        return;

                        // }
                    }
                    //End Rev Rajdip
                }
            }
            else
            {
                string addressold = Convert.ToString(e.OldValues["Type"]);
                DataTable dtadd = oDBEngine.GetDataTable("select add_addressType,ISNULL(Isdefault,0) Isdefault from tbl_master_address where add_cntid='" + Convert.ToString(HttpContext.Current.Session["KeyVal_InternalID"]) + "' and ISNULL(add_status,'Y')='Y'");

                if (addressold != Address)
                {
                    for (int m = 0; m < dtadd.Rows.Count; m++)
                    {
                        //Rev Rajdip
                        //if (Convert.ToString(dtadd.Rows[m]["add_addressType"]) == Convert.ToString(Address))
                        //{
                        //    //if (dtphone.Rows[m]["phf_status"].ToString() == status.ToString())
                        //    //{

                        //    e.RowError = "[ " + Address + " ]" + "Address Type is Already Exist";
                        //    return;
                        //    // }
                        //}

                        if ((Convert.ToString(dtadd.Rows[m]["add_addressType"]) == Convert.ToString(Address)) && (Convert.ToString(dtadd.Rows[m]["Isdefault"]) == Convert.ToString(flag)) && (flag == "True") && (Convert.ToString(dtadd.Rows[m]["Isdefault"]) == "True"))
                        {
                            //if (dtphone.Rows[m]["phf_status"].ToString() == status.ToString())
                            //{

                            e.RowError = "[ " + Address + " ]" + "Address Type is Already Exist With default";
                            return;
                            // }
                        }

                        //End Rev Rajdip
                    }
                }

            }
            string countryid = string.Empty;
            string[,] countryname = null;
            foreach (GridViewColumn column in AddressGrid.Columns)
            {
                GridViewDataColumn dataColumn = column as GridViewDataColumn;
                if (dataColumn == null) continue;
                if (dataColumn.FieldName == "Country")
                {
                    countryid = Convert.ToString(e.NewValues["Country"]);
                    if (countryid == "")
                    {
                        countryid = "''";
                    }
                    countryname = oDBEngine.GetFieldValue("tbl_master_country", "cou_country", "cou_id=" + countryid, 1);
                }


                //commented by debjyoti 
                //Reason: Pin code lenth checking does not need any more, coz pin conde now select from drop down
                //if (dataColumn.FieldName == "PinCode")
                //{
                //    string pin = Convert.ToString(e.NewValues["PinCode"]);
                //    if (countryname[0, 0] == "India")
                //    {
                //        double Num;
                //        bool isNum = double.TryParse(pin, out Num);
                //        int len = pin.Length;
                //        if (!isNum || len != 6)
                //        {
                //            // e.Errors[dataColumn] = "Enter Valid PinCode";
                //            e.RowError = "Enter Valid PinCode";
                //            return;
                //        }
                //    }

                //}
            }

        }
        protected void PhoneGrid_RowValidating(object sender, DevExpress.Web.Data.ASPxDataValidationEventArgs e)
        {
            //Purpose : Replace .ToString() with Convert.ToString(..)
            //Name : Sudip 
            // Dated : 22-12-2016

            string Address = Convert.ToString(e.NewValues["phf_type"]);
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
                //if (e.NewValues["phf_SMSFacility"] == null)
                //{
                //    e.NewValues["phf_SMSFacility"] = "";
                //    string smstype = Convert.ToString(e.NewValues["phf_SMSFacility"]);
                //    if (smstype.Length == 0)
                //    {
                //        e.RowError = "Please Select Sms Alert type";
                //        return;
                //    }
                //}
                if (PhoneNumber.Length != 10)
                {
                    e.RowError = "Enter Valid Mobile Number";
                    return;
                }
            }
        }
        protected void EmailGrid_RowValidating1(object sender, DevExpress.Web.Data.ASPxDataValidationEventArgs e)
        {
            //Purpose : Replace .ToString() with Convert.ToString(..)
            //Name : Sudip 
            // Dated : 22-12-2016

            #region
            //string ccEmail = "";
            //string email = e.NewValues["eml_email"].ToString();
            //try
            //{
            //    ccEmail = e.NewValues["eml_ccEmail"].ToString();
            //}
            //catch
            //{
            //}
            //string[,] emailCheck = oDBEngine.GetFieldValue("tbl_master_email", "eml_email", " eml_email='" + email + "'", 1);
            //string email1 = "";
            //if (emailCheck[0, 0] != "n")
            //{
            //    email1 = emailCheck[0, 0];
            //}
            //string[,] ccEmailCheck = oDBEngine.GetFieldValue("tbl_master_email", "eml_ccEmail", " eml_ccEmail='" + ccEmail + "'", 1);
            //string ccEmail1 = "";
            //if (ccEmailCheck[0, 0] != "n")
            //{
            //    ccEmail1 = ccEmailCheck[0, 0];
            //}
            //if (email1 == "" && ccEmail1 == "")
            //{

            //}
            //else
            //{
            //    e.RowError = "Email Id Already Exists";
            //    return;
            //}
            #endregion

            string emailtype = Convert.ToString(e.NewValues["eml_type"]);
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
            string[,] ccEmailCheck = oDBEngine.GetFieldValue("tbl_master_email", "eml_ccEmail", " eml_ccEmail='" + ccEmail + "'", 1);
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
                //e.RowError = "Email Id Already Exists";
                return;
            }
            string emltype1 = Convert.ToString(e.NewValues["eml_type"]);
            if (emltype1 == "Official")
            {

                //if (e.NewValues["eml_facility"] == null)
                //{
                //    e.NewValues["eml_facility"] = "";
                //    string emlsmstype = Convert.ToString(e.NewValues["eml_facility"]);
                //    if (emlsmstype.Length == 0)
                //    {
                //        e.RowError = "Please Select Email Alert type";
                //        return;
                //    }
                //}

            }

        }
        //Purpose: User rights in Address tab in correspondence tab
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
            if (rights.CanAdd)
            {
                if (e.ButtonType == ColumnCommandButtonType.New)
                {
                    e.Visible = false;
                }
            }
            //if (!rights.CanEdit)
            //{
            //    if (e.ButtonType == ColumnCommandButtonType.Edit)
            //    {
            //        e.Visible = false;
            //    }
            //}

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

            //if (!rights.CanEdit)
            //{
            //    if (e.ButtonType == ColumnCommandButtonType.Edit)
            //    {
            //        e.Visible = false;
            //    }
            //}

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

            //if (!rights.CanEdit)
            //{
            //    if (e.ButtonType == ColumnCommandButtonType.Edit)
            //    {
            //        e.Visible = false;
            //    }
            //}

        }
        //End : debjyoti 17-11-2016

        // Purpose : Add Action Func() in Email, Address & Phone Grid
        // Name : Sudip 
        // Dated : 22-12-2016

        protected void ASPxCallbackPanel1_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
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
                    StDate.Value = oDBEngine.GetDate();
                }
                txtReason.Text = Convert.ToString(dtAdd.Rows[0]["add_statusChangeReason"]);

            }
            else if (data[0] == "SaveOld")
            {
                if (cmbStatus.SelectedValue == "Y")
                {
                    DataTable dtAdd = oDBEngine.GetDataTable("tbl_master_address ", " add_addressType  ", "add_id ='" + Convert.ToString(data[1]) + "'");
                    string addtype = "";
                    if (dtAdd != null && dtAdd.Rows.Count > 0)
                    {
                        addtype = Convert.ToString(dtAdd.Rows[0]["add_addressType"]);
                    }
                    string intid = "";

                    if (addtype == "Correspondence" || addtype == "Residence" || addtype == "Office")
                    {
                        if (Session["KeyVal_InternalID"] != null)
                        {
                            intid = Convert.ToString(Session["KeyVal_InternalID"]);
                            DataTable dtallAddtype = oDBEngine.GetDataTable("tbl_master_address ", " add_addressType  ", "add_cntId ='" + intid + "' and add_addressType='" + addtype + "' and (add_status is null or add_status='Y') and add_id !='" + Convert.ToString(data[1]) + "'");
                            if (dtallAddtype != null && dtallAddtype.Rows.Count >= 1)
                            {
                                if (addtype == "Correspondence")
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
                            if (addtype == "Correspondence")
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
        }
        protected void ASPxCallbackPanel1_CustomJSProperties(object sender, DevExpress.Web.CustomJSPropertiesEventArgs e)
        {
            e.Properties["cpLast"] = Stat;
        }
        protected void AddressGrid_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            if (e.Parameters != null)
            {
                AddressGrid.DataBind();
            }
        }
        protected void ASPxCallbackPanelP_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
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
                    StDateP.Value = oDBEngine.GetDate();
                }
                txtReasonP.Text = Convert.ToString(dtAdd.Rows[0]["phf_StatusChangeReason"]);

            }
            else if (data[0] == "SaveOld")
            {
                if (cmbStatusP.SelectedValue == "Y")
                {
                    DataTable dtAdd = oDBEngine.GetDataTable("tbl_master_phonefax ", " phf_type  ", "phf_id ='" + Convert.ToString(data[1]) + "'");
                    string addtype = "";
                    if (dtAdd != null && dtAdd.Rows.Count > 0)
                    {
                        addtype = Convert.ToString(dtAdd.Rows[0]["phf_type"]);
                    }
                    string intid = "";


                    if (addtype == "Residence" || addtype == "Office" || addtype == "Correspondenc" || addtype == "Mobile" || addtype == "Fax")
                    {
                        if (Session["KeyVal_InternalID"] != null)
                        {
                            intid = Convert.ToString(Session["KeyVal_InternalID"]);
                            DataTable dtallAddtype = oDBEngine.GetDataTable("tbl_master_phonefax ", " phf_type  ", "phf_cntId ='" + intid + "' and phf_type='" + addtype + "' and (phf_Status is null or phf_Status='Y') and phf_id !='" + Convert.ToString(data[1]) + "'");
                            if (dtallAddtype != null && dtallAddtype.Rows.Count >= 1)
                            {
                                if (addtype == "Residence")
                                {
                                    Stat = "Y1";
                                }
                                else if (addtype == "Office")
                                {
                                    Stat = "Y2";
                                }
                                else if (addtype == "Correspondenc")
                                {
                                    Stat = "Y3";
                                }
                                else if (addtype == "Mobile")
                                {
                                    Stat = "Y4";
                                }
                                else if (addtype == "Fax")
                                {
                                    Stat = "Y5";
                                }
                            }
                            else
                            {
                                int i = oDBEngine.SetFieldValue("tbl_master_phonefax", " phf_Status='" + cmbStatusP.SelectedItem.Value + "'  ,phf_StatusChangeDate='" + StDateP.Value + "',  phf_StatusChangeReason='" + txtReasonP.Text + "'  ", " phf_id ='" + Convert.ToString(data[1]) + "'");
                                if (i == 1)
                                {
                                    Stat = "Y";
                                }
                            }
                        }
                        else
                        {
                            if (addtype == "Residence")
                            {
                                Stat = "Y1";
                            }
                            else if (addtype == "Office")
                            {
                                Stat = "Y2";
                            }
                            else if (addtype == "Correspondenc")
                            {
                                Stat = "Y3";
                            }
                            else if (addtype == "Mobile")
                            {
                                Stat = "Y4";
                            }
                            else if (addtype == "Fax")
                            {
                                Stat = "Y5";
                            }
                        }
                    }
                    else
                    {
                        int i = oDBEngine.SetFieldValue("tbl_master_phonefax", " phf_Status='" + cmbStatusP.SelectedItem.Value + "'  ,phf_StatusChangeDate='" + StDateP.Value + "',  phf_StatusChangeReason='" + txtReasonP.Text + "'  ", " phf_id ='" + Convert.ToString(data[1]) + "'");
                        if (i == 1)
                        {
                            Stat = "Y";
                        }
                    }
                }
                else
                {
                    int i = oDBEngine.SetFieldValue("tbl_master_phonefax", " phf_Status='" + cmbStatusP.SelectedItem.Value + "'  ,phf_StatusChangeDate='" + StDateP.Value + "',  phf_StatusChangeReason='" + txtReasonP.Text + "'  ", " phf_id ='" + Convert.ToString(data[1]) + "'");
                    if (i == 1)
                    {
                        Stat = "Y";
                    }
                }
            }
        }
        protected void ASPxCallbackPanelP_CustomJSProperties(object sender, DevExpress.Web.CustomJSPropertiesEventArgs e)
        {
            e.Properties["cpLast"] = Stat;
        }
        protected void PhoneGrid_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            if (e.Parameters != null)
            {
                PhoneGrid.DataBind();
            }
        }
        protected void ASPxCallbackPanelE_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            string[] data = e.Parameter.Split('~');
            if (data[0] == "Edit")
            {
                DataTable dtAdd = oDBEngine.GetDataTable("tbl_master_email ", "   eml_Status  ,eml_StatusChangeDate,  eml_StatusChangeReason ", "eml_id ='" + Convert.ToString(data[1]) + "'");
                if (Convert.ToString(dtAdd.Rows[0]["eml_Status"]) != "N")
                {
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
                    StDateE.Value = oDBEngine.GetDate();
                }
                txtReasonE.Text = Convert.ToString(dtAdd.Rows[0]["eml_StatusChangeReason"]);
            }
            else if (data[0] == "SaveOld")
            {
                if (cmbStatusE.SelectedValue == "Y")
                {
                    DataTable dtAdd = oDBEngine.GetDataTable("tbl_master_email ", " eml_type  ", "eml_id ='" + Convert.ToString(data[1]) + "'");
                    string addtype = "";
                    if (dtAdd != null && dtAdd.Rows.Count > 0)
                    {
                        addtype = Convert.ToString(dtAdd.Rows[0]["eml_type"]);
                    }
                    string intid = "";


                    if (addtype == "Personal" || addtype == "Official" || addtype == "Correspondenc")
                    {
                        if (Session["KeyVal_InternalID"] != null)
                        {
                            intid = Convert.ToString(Session["KeyVal_InternalID"]);
                            DataTable dtallAddtype = oDBEngine.GetDataTable("tbl_master_email ", " eml_type  ", "eml_cntId ='" + intid + "' and eml_type='" + addtype + "' and (eml_Status is null or eml_Status='Y') and eml_id !='" + Convert.ToString(data[1]) + "'");
                            if (dtallAddtype != null && dtallAddtype.Rows.Count >= 1)
                            {
                                if (addtype == "Personal")
                                {
                                    Stat = "Y1";
                                }
                                else if (addtype == "Official")
                                {
                                    Stat = "Y2";
                                }
                                else if (addtype == "Web Site")
                                {
                                    Stat = "Y3";
                                }
                            }
                            else
                            {
                                int i = oDBEngine.SetFieldValue("tbl_master_email", " eml_Status='" + cmbStatusE.SelectedItem.Value + "'  ,eml_StatusChangeDate='" + StDateE.Value + "',  eml_StatusChangeReason='" + txtReasonE.Text + "'  ", " eml_id ='" + Convert.ToString(data[1]) + "'");
                                if (i == 1)
                                {
                                    Stat = "Y";
                                }
                            }
                        }
                        else
                        {
                            if (addtype == "Personal")
                            {
                                Stat = "Y1";
                            }
                            else if (addtype == "Official")
                            {
                                Stat = "Y2";
                            }
                            else if (addtype == "Web Site")
                            {
                                Stat = "Y3";
                            }
                        }
                    }
                    else
                    {
                        int i = oDBEngine.SetFieldValue("tbl_master_email", " eml_Status='" + cmbStatusE.SelectedItem.Value + "'  ,eml_StatusChangeDate='" + StDateE.Value + "',  eml_StatusChangeReason='" + txtReasonE.Text + "'  ", " eml_id ='" + Convert.ToString(data[1]) + "'");
                        if (i == 1)
                        {
                            Stat = "Y";
                        }
                    }
                }
                else
                {
                    int i = oDBEngine.SetFieldValue("tbl_master_email", " eml_Status='" + cmbStatusE.SelectedItem.Value + "'  ,eml_StatusChangeDate='" + StDateE.Value + "',  eml_StatusChangeReason='" + txtReasonE.Text + "'  ", " eml_id ='" + Convert.ToString(data[1]) + "'");
                    if (i == 1)
                    {
                        Stat = "Y";
                    }
                }
            }

        }
        protected void ASPxCallbackPanelE_CustomJSProperties(object sender, DevExpress.Web.CustomJSPropertiesEventArgs e)
        {
            e.Properties["cpLast"] = Stat;
        }
        protected void EmailGrid_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            if (e.Parameters != null)
            {
                EmailGrid.DataBind();
            }
        }

        #region Web Method 
        //-------------------------------------------------Arindam----------------------------------------------------------//
         [WebMethod]
         [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static object GetAllStateByCountry(string country_id)
        {
            string output = string.Empty;
            //int NoOfRowEffected = 0;
            //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
          System.Collections.Generic.List<ReceiveJsonData> ob = new System.Collections.Generic.List<ReceiveJsonData>();
            DataTable state = new DataTable();
            try
            {
            state = oDBEngine.GetDataTable(@"SELECT s.id as ID,s.state as State from tbl_master_state s where s.countryId='"+country_id+"'order BY s.id");
                 
                ob = (from DataRow dr in state.Rows
                      select new ReceiveJsonData()
                                   {
                                       id = Convert.ToInt32(dr["ID"]),
                                       name = Convert.ToString(dr["State"]).Trim()



                                   }).ToList();
            
            }
            catch (Exception ex)
            {
                output = ex.Message.ToString();
            }


            return ob;


        }
        [WebMethod]
         public static object GetAllCityBYState(string state_id)
         {
             string output = string.Empty;
             //int NoOfRowEffected = 0;
             //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
             BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
             System.Collections.Generic.List<ReceiveJsonData> ob = new System.Collections.Generic.List<ReceiveJsonData>();
             DataTable state = new DataTable();
             try
             {
                 state = oDBEngine.GetDataTable(@"SELECT c.city_id AS CityId, c.city_name AS City FROM tbl_master_city c where c.state_id='" + state_id + "'  order BY c.city_name");

                 ob = (from DataRow dr in state.Rows
                       select new ReceiveJsonData()
                       {
                           id = Convert.ToInt32(dr["CityId"]),
                           name = Convert.ToString(dr["City"]).Trim()



                       }).ToList();
                 

             }
             catch (Exception ex)
             {
                 output = ex.Message.ToString();
             }


             return ob;


         }
        [WebMethod]
         public static object GetAllPinAreaByCity(string city_id)
         {
             string output = string.Empty;
             //int NoOfRowEffected = 0;
             //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
             BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
            // System.Collections.Generic.List<ReceiveJsonData> ob = new System.Collections.Generic.List<ReceiveJsonData>();
             listofjsondata obj = new listofjsondata();
             DataSet AreaPin = new DataSet();
             try
             {
                 ProcedureExecute proc = new ProcedureExecute("prc_getareapindetails");
                 proc.AddIntegerPara("@city_id", Convert.ToInt32(city_id));
                 proc.AddVarcharPara("@is_success", 500, "", QueryParameterDirection.Output);
                 AreaPin = proc.GetDataSet();
                 output = Convert.ToString(proc.GetParaValue("@is_success"));

                 obj._receivedata_area = (from DataRow dr in AreaPin.Tables[0].Rows
                       select new ReceiveJsonData()
                       {
                           id = Convert.ToInt32(dr["area_id"]),
                           name = Convert.ToString(dr["area_name"]).Trim()



                       }).ToList();
                 obj._receivedatapin = (from DataRow dr in AreaPin.Tables[1].Rows
                                          select new ReceiveJsonData()
                                          {
                                              id = Convert.ToInt32(dr["pin_id"]),
                                              name = Convert.ToString(dr["pin_code"]).Trim()



                                          }).ToList();


             }
             catch (Exception ex)
             {
                 output = ex.Message.ToString();
             }


             return obj;


         }


        
        //-------------------------------------------------Arindam----------------------------------------------------------//

        public class ReceiveJsonData
        {
            public int id { get; set; }
            public string name { get; set; }
            
        }
        public class listofjsondata
        {
            public System.Collections.Generic.List<ReceiveJsonData> _receivedatapin { get; set; }
            public System.Collections.Generic.List<ReceiveJsonData> _receivedata_area { get; set; }
          
        }

        //public class ReceivePinData
        //{
        //    public int pin_id { get; set; }
        //    public string pin_code { get; set; }
        //}

        #endregion

        //End : Sudip 22-12-2016
    }
}