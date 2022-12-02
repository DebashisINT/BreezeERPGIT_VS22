using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
////using DevExpress.Web.ASPxClasses;
////using DevExpress.Web;
using DevExpress.Web;
////using DevExpress.Web.ASPxTabControl;
using BusinessLogicLayer;
namespace ERP.OMS.Management.Activities
{
    public partial class management_Activities_Contact_Correspondence : ERP.OMS.ViewState_class.VSPage// System.Web.UI.Page
    {

        string emailid = "a";
        String Stat = "N";
        //DBEngine oDBEngine = new DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
       // BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]); MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        
        public string pageAccess = "";
        //Converter Oconverter = new Converter();
        BusinessLogicLayer.Converter Oconverter = new BusinessLogicLayer.Converter();
        // DBEngine oDBEngine = new DBEngine(string.Empty);

        BusinessLogicLayer.Contact OContactGeneralBL = new BusinessLogicLayer.Contact();

        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //'http://localhost:2957/InfluxCRM/management/testProjectMainPage_employee.aspx'
                string sPath = HttpContext.Current.Request.Url.ToString();
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }
        protected void Page_Unload(object sender, EventArgs e)
        {
            Page.ClientScript.RegisterStartupScript(GetType(), "jscript", "<script language='javascript'>iframesource();</script>");
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            string cnttype = Session["ContactType"].ToString();
            string intid = Session["KeyVal_InternalID"].ToString();
            DataTable dtcmp = oDBEngine.GetDataTable(" tbl_master_address  ", "*", "add_cntId='" + intid + "'");
            if (dtcmp.Rows.Count == 0)
            {
                try
                {
                    /* Tier Structure
                    String con = ConfigurationSettings.AppSettings["DBConnectionDefault"];
                    SqlConnection lcon = new SqlConnection(con);
                    lcon.Open();
                    SqlCommand lcmdEmplInsert = new SqlCommand("AdressDummyInsert", lcon);
                    lcmdEmplInsert.CommandType = CommandType.StoredProcedure;
                    lcmdEmplInsert.Parameters.AddWithValue("@contacttype", cnttype);
                    lcmdEmplInsert.Parameters.AddWithValue("@InternalId", intid);
                    lcmdEmplInsert.ExecuteNonQuery();
                    */
                    OContactGeneralBL.Insert_DummyAdress(cnttype, intid);
                }
                catch
                {
                }
            }



            if (HttpContext.Current.Session["userid"] == null)
            {
               //Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            if (!IsPostBack)
            {
                StDate.UseMaskBehavior = true;
                StDate.EditFormatString = Oconverter.GetDateFormat("Date");
                StDateP.UseMaskBehavior = true;
                StDateP.EditFormatString = Oconverter.GetDateFormat("Date");
                StDateE.UseMaskBehavior = true;
                StDateE.EditFormatString = Oconverter.GetDateFormat("Date");


                //Newly Added Connection string from Session
                Address.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                CountrySelect.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                StateSelect.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                SelectCity.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                SelectArea.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                Phone.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                Email.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);




                if (Session["Name"] != null)
                {
                    lblName.Text = Session["Name"].ToString();
                }
                //SalesVisitAddress();
            }
            SalesVisitAddress();

            //DisabledTabPage();

            if (cnttype == "OtherEntity")
            {
                TabPage page = ASPxPageControl1.TabPages.FindByName("CorresPondence");
                page.Visible = true;
                page = ASPxPageControl1.TabPages.FindByName("Bank Details");
                page.Visible = false;
                page = ASPxPageControl1.TabPages.FindByName("DP Details");
                page.Visible = false;
                page = ASPxPageControl1.TabPages.FindByName("Documents");
                page.Visible = true;
                page = ASPxPageControl1.TabPages.FindByName("FamilyMembers");
                page.Visible = false;
                page = ASPxPageControl1.TabPages.FindByName("Registration");
                page.Visible = false;
                page = ASPxPageControl1.TabPages.FindByName("Group Member");
                page.Visible = false;
                page = ASPxPageControl1.TabPages.FindByName("Deposit");
                page.Visible = false;
                page = ASPxPageControl1.TabPages.FindByName("Remarks");
                page.Visible = true;
                page = ASPxPageControl1.TabPages.FindByName("Education");
                page.Visible = false;
                page = ASPxPageControl1.TabPages.FindByName("Trad. Prof.");
                page.Visible = false;
                page = ASPxPageControl1.TabPages.FindByName("Other");
                page.Visible = false;
                page = ASPxPageControl1.TabPages.FindByName("Subscription");
                page.Visible = false;
            }
        }
        //public void DisabledTabPage()
        //{
        //    TabPage page = ASPxPageControl1.TabPages.FindByName("CorresPondence");
        //    page.Visible = true;
        //    page = ASPxPageControl1.TabPages.FindByName("BankDetails");
        //    page.Visible = true;
        //    page = ASPxPageControl1.TabPages.FindByName("DPDetails");
        //    page.Visible = true;
        //    page = ASPxPageControl1.TabPages.FindByName("Documents");
        //    page.Visible = true;
        //    page = ASPxPageControl1.TabPages.FindByName("FamilyMembers");
        //    page.Visible = true;
        //    page = ASPxPageControl1.TabPages.FindByName("Registration");
        //    page.Visible = true;
        //    page = ASPxPageControl1.TabPages.FindByName("GroupMember");
        //    page.Visible = true;
        //    page = ASPxPageControl1.TabPages.FindByName("Deposit");
        //    page.Visible = true;
        //    page = ASPxPageControl1.TabPages.FindByName("Remarks");
        //    page.Visible = true;
        //    page = ASPxPageControl1.TabPages.FindByName("Education");
        //    page.Visible = true;
        //    page = ASPxPageControl1.TabPages.FindByName("Trad. Prof.");
        //    page.Visible = true;
        //    page = ASPxPageControl1.TabPages.FindByName("Other");
        //    page.Visible = true;
        //    page = ASPxPageControl1.TabPages.FindByName("Subscription");
        //    page.Visible = true;
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
            cmb.Items.Insert(0, new ListEditItem("Select", "0"));
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
            cmb.Items.Insert(0, new ListEditItem("Select", "0"));
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
            cmb.Items.Insert(0, new ListEditItem("Select", "0"));
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
        public void SalesVisitAddress()
        {
            if (Session["ContactType"] == null)
            {
                Session["ContactType"] = "Lead";
            }
            try
            {
                if (Request.QueryString["formtype"] != null)
                {
                    string ID = Session["InternalId"].ToString();
                    Session["KeyVal_InternalID_New"] = ID.ToString();
                    Address.SelectCommand = "select DISTINCT  tbl_master_address.add_id AS Id, tbl_master_address.add_addressType AS Type,tbl_master_address.add_address1 AS Address1,  tbl_master_address.add_address2 AS Address2,tbl_master_address.add_address3 AS Address3,tbl_master_address.add_landMark AS LandMark,tbl_master_address.add_country AS Country,tbl_master_address.add_state AS State,tbl_master_address.add_city AS City,CASE isnull(add_country, '')WHEN '' THEN '0' ELSE(SELECT cou_country FROM tbl_master_country WHERE cou_id = add_country) END AS Country1,CASE isnull(add_state, '') WHEN '' THEN '0' ELSE(SELECT state FROM tbl_master_state WHERE id = add_state) END AS State1,CASE isnull(add_city, '') WHEN '' THEN '0' ELSE(SELECT city_name FROM tbl_master_city WHERE city_id = add_city) END AS City1,CASE add_area WHEN '' THEN '0' Else(select area_name From tbl_master_area Where area_id = add_area) End AS area,tbl_master_address.add_pin AS PinCode, tbl_master_address.add_landMark AS LankMark, case when add_status='N' then 'Deactive' else 'Active' end as status from tbl_master_address where add_cntId='" + ID + "'";
                    Email.SelectCommand = "select eml_id,eml_cntId,eml_entity,eml_type,eml_email,eml_ccEmail,eml_website,CreateDate,CreateUser,case when eml_Status='N' then 'Deactive' else 'Active' end as status,(case when eml_facility=1 then '1' when eml_facility=2 then '2' else null end) as eml_facility from tbl_master_email where eml_cntId='" + ID + "'";

                    Phone.SelectCommand = "select DISTINCT phf_id,phf_cntId,phf_entity,phf_type,phf_countryCode,phf_areaCode,phf_phoneNumber,phf_extension, ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' + ISNULL(phf_phoneNumber, '') +' '+ ISNULL(phf_extension, '') + ISNULL(phf_faxNumber, '') AS Number,case when phf_Status='N' then 'Deactive' else 'Active' end as status,(case when phf_SMSFacility=1 then '1' when phf_SMSFacility=2 then '2' else null end) as phf_SMSFacility  from tbl_master_phonefax where phf_cntId='" + ID + "'";
                    //Email.SelectCommand = "select eml_id,eml_cntId,eml_entity,eml_type,eml_email,eml_ccEmail,eml_website,CreateDate,CreateUser from tbl_master_email where eml_cntId='" + ID + "'";
                    //Phone.SelectCommand = "select DISTINCT phf_id,phf_cntId,phf_entity,phf_type,phf_countryCode,phf_areaCode,phf_phoneNumber,phf_extension, ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' + ISNULL(phf_phoneNumber, '') +' '+ ISNULL(phf_extension, '') + ISNULL(phf_faxNumber, '') AS Number,case when phf_Status='N' then 'Deactive' else 'Active' end as status  from tbl_master_phonefax where phf_cntId='" + ID + "'";
                    TabPage page = ASPxPageControl1.TabPages.FindByName("Documents");
                    page.Visible = false;
                    page = ASPxPageControl1.TabPages.FindByName("General");
                    page.Visible = false;
                    page = ASPxPageControl1.TabPages.FindByName("Bank Details");
                    page.Visible = false;
                    page = ASPxPageControl1.TabPages.FindByName("DP Details");
                    page.Visible = false;
                    page = ASPxPageControl1.TabPages.FindByName("Documents");
                    page.Visible = false;
                    page = ASPxPageControl1.TabPages.FindByName("Family Members");
                    page.Visible = false;
                    page = ASPxPageControl1.TabPages.FindByName("Registration");
                    page.Visible = false;
                    page = ASPxPageControl1.TabPages.FindByName("Group Member");
                    page.Visible = false;
                    page = ASPxPageControl1.TabPages.FindByName("Deposit");
                    page.Visible = false;
                    page = ASPxPageControl1.TabPages.FindByName("Remarks");
                    page.Visible = false;
                    page = ASPxPageControl1.TabPages.FindByName("Education");
                    page.Visible = false;
                    page = ASPxPageControl1.TabPages.FindByName("Trad. Prof.");
                    page.Visible = false;
                    page = ASPxPageControl1.TabPages.FindByName("Other");
                    page.Visible = false;
                    AddressGrid.Columns[15].Visible = true;
                    PhoneGrid.Columns[10].Visible = false;
                    EmailGrid.Columns[7].Visible = false;
                }
                else
                {
                    if (Session["KeyVal_InternalID"] != null)
                    {
                        string ID = Session["KeyVal_InternalID"].ToString();
                        Session["KeyVal_InternalID_New"] = ID.ToString();
                        Address.SelectCommand = "select DISTINCT  tbl_master_address.add_id AS Id, tbl_master_address.add_addressType AS Type,tbl_master_address.add_address1 AS Address1,  tbl_master_address.add_address2 AS Address2,tbl_master_address.add_address3 AS Address3,tbl_master_address.add_landMark AS LandMark,tbl_master_address.add_country AS Country,tbl_master_address.add_state AS State,tbl_master_address.add_city AS City,CASE isnull(add_country, '')WHEN '' THEN '0' ELSE(SELECT cou_country FROM tbl_master_country WHERE cou_id = add_country) END AS Country1,CASE isnull(add_state, '') WHEN '' THEN '0' ELSE(SELECT state FROM tbl_master_state WHERE id = add_state) END AS State1,CASE isnull(add_city, '') WHEN '' THEN '0' ELSE(SELECT city_name FROM tbl_master_city WHERE city_id = add_city) END AS City1,CASE add_area WHEN '' THEN '0' Else(select area_name From tbl_master_area Where area_id = add_area) End AS area,tbl_master_address.add_pin AS PinCode, tbl_master_address.add_landMark AS LankMark, case when add_status='N' then 'Deactive' else 'Active' end as status from tbl_master_address where add_cntId='" + ID + "'";
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
                            string ID = Session["LeadId"].ToString();
                            Session["KeyVal_InternalID_New"] = ID.ToString();
                            Address.SelectCommand = "select DISTINCT  tbl_master_address.add_id AS Id, tbl_master_address.add_addressType AS Type,tbl_master_address.add_address1 AS Address1,  tbl_master_address.add_address2 AS Address2,tbl_master_address.add_address3 AS Address3,tbl_master_address.add_landMark AS LandMark,tbl_master_address.add_country AS Country,tbl_master_address.add_state AS State,tbl_master_address.add_city AS City,CASE isnull(add_country, '')WHEN '' THEN '0' ELSE(SELECT cou_country FROM tbl_master_country WHERE cou_id = add_country) END AS Country1,CASE isnull(add_state, '') WHEN '' THEN '0' ELSE(SELECT state FROM tbl_master_state WHERE id = add_state) END AS State1,CASE isnull(add_city, '') WHEN '' THEN '0' ELSE(SELECT city_name FROM tbl_master_city WHERE city_id = add_city) END AS City1,CASE add_area WHEN '' THEN '0' Else(select area_name From tbl_master_area Where area_id = add_area) End AS area,tbl_master_address.add_pin AS PinCode, tbl_master_address.add_landMark AS LankMark, case when add_status='N' then 'Deactive' else 'Active' end as status from tbl_master_address where add_cntId='" + ID + "'";
                            Email.SelectCommand = "select eml_id,eml_cntId,eml_entity,eml_type,eml_email,eml_ccEmail,eml_website,CreateDate,CreateUser,case when eml_Status='N' then 'Deactive' else 'Active' end as status,(case when eml_facility=1 then '1' when eml_facility=2 then '2' else null end) as eml_facility from tbl_master_email where eml_cntId='" + ID + "'";

                            Phone.SelectCommand = "select DISTINCT phf_id,phf_cntId,phf_entity,phf_type,phf_countryCode,phf_areaCode,phf_phoneNumber,phf_extension, ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' + ISNULL(phf_phoneNumber, '') +' '+ ISNULL(phf_extension, '') + ISNULL(phf_faxNumber, '') AS Number,case when phf_Status='N' then 'Deactive' else 'Active' end as status,(case when phf_SMSFacility=1 then '1' when phf_SMSFacility=2 then '2' else null end) as phf_SMSFacility  from tbl_master_phonefax where phf_cntId='" + ID + "'";

                            //Email.SelectCommand = "select eml_id,eml_cntId,eml_entity,eml_type,eml_email,eml_ccEmail,eml_website,CreateDate,CreateUser,case when eml_Status='N' then 'Deactive' else 'Active' end as status from tbl_master_email where eml_cntId='" + ID + "'";
                            //Phone.SelectCommand = "select DISTINCT phf_id,phf_cntId,phf_entity,phf_type,phf_countryCode,phf_areaCode,phf_phoneNumber,phf_extension, ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' + ISNULL(phf_phoneNumber, '') +' '+ ISNULL(phf_extension, '') + ISNULL(phf_faxNumber, '') AS Number,case when phf_Status='N' then 'Deactive' else 'Active' end as status  from tbl_master_phonefax where phf_cntId='" + ID + "'";
                            //TabPage page = ASPxPageControl1.TabPages.FindByName("Documents");
                            //page.Visible = false;
                            //page = ASPxPageControl1.TabPages.FindByName("General");
                            //page.Visible = false;
                            //page = ASPxPageControl1.TabPages.FindByName("Bank Details");
                            //page.Visible = false;
                            //page = ASPxPageControl1.TabPages.FindByName("DP Details");
                            //page.Visible = false;
                            //page = ASPxPageControl1.TabPages.FindByName("Documents");
                            //page.Visible = false;
                            //page = ASPxPageControl1.TabPages.FindByName("Family Members");
                            //page.Visible = false;
                            //page = ASPxPageControl1.TabPages.FindByName("Registration");
                            //page.Visible = false;
                            //page = ASPxPageControl1.TabPages.FindByName("Group Member");
                            //page.Visible = false;
                            //page = ASPxPageControl1.TabPages.FindByName("Deposit");
                            //page.Visible = false;
                            //page = ASPxPageControl1.TabPages.FindByName("Remarks");
                            //page.Visible = false;
                            //page = ASPxPageControl1.TabPages.FindByName("Education");
                            //page.Visible = false;
                            //page = ASPxPageControl1.TabPages.FindByName("Trad. Prof.");
                            //page.Visible = false;
                            //page = ASPxPageControl1.TabPages.FindByName("Other");
                            //page.Visible = false;

                            TabPage page = ASPxPageControl1.TabPages.FindByName("Documents");
                            page.Visible = true;
                            page = ASPxPageControl1.TabPages.FindByName("General");
                            page.Visible = true;
                            page = ASPxPageControl1.TabPages.FindByName("Bank Details");
                            page.Visible = true;
                            page = ASPxPageControl1.TabPages.FindByName("DP Details");
                            page.Visible = true;
                            page = ASPxPageControl1.TabPages.FindByName("Documents");
                            page.Visible = true;
                            page = ASPxPageControl1.TabPages.FindByName("Family Members");
                            page.Visible = true;
                            page = ASPxPageControl1.TabPages.FindByName("Registration");
                            page.Visible = true;
                            page = ASPxPageControl1.TabPages.FindByName("Group Member");
                            page.Visible = true;
                            page = ASPxPageControl1.TabPages.FindByName("Deposit");
                            page.Visible = true;
                            page = ASPxPageControl1.TabPages.FindByName("Remarks");
                            page.Visible = true;
                            page = ASPxPageControl1.TabPages.FindByName("Education");
                            page.Visible = true;
                            page = ASPxPageControl1.TabPages.FindByName("Trad. Prof.");
                            page.Visible = true;
                            page = ASPxPageControl1.TabPages.FindByName("Other");
                            page.Visible = true;


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
        }
        protected void EmailGrid_RowValidating(object sender, DevExpress.Web.Data.ASPxDataValidationEventArgs e)
        {


            string emailtype = e.NewValues["eml_type"].ToString();
            if (e.IsNewRow)
            {
                DataTable dtemail = oDBEngine.GetDataTable("select eml_type from tbl_master_email where eml_cntid='" + HttpContext.Current.Session["KeyVal_InternalID"].ToString() + "'  and ISNULL(eml_status,'Y')='Y'");

                for (int m = 0; m < dtemail.Rows.Count; m++)
                {
                    if (dtemail.Rows[m]["eml_type"].ToString() == emailtype.ToString())
                    {
                        e.RowError = "[ " + emailtype + " ]" + "Email Type is Already Exist";
                        return;
                    }
                }
            }
            else
            {
                string addressold = e.OldValues["eml_type"].ToString();
                DataTable dtemail = oDBEngine.GetDataTable("select eml_type from tbl_master_email where eml_cntid='" + HttpContext.Current.Session["KeyVal_InternalID"].ToString() + "'  and ISNULL(eml_status,'Y')='Y'");

                if (addressold != emailtype)
                {
                    for (int m = 0; m < dtemail.Rows.Count; m++)
                    {
                        if (dtemail.Rows[m]["eml_type"].ToString() == emailtype.ToString())
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

            emltype = e.NewValues["eml_type"].ToString();
            emlcntid = HttpContext.Current.Session["KeyVal_InternalID"].ToString();
            emlentity = HttpContext.Current.Session["ContactType"].ToString();
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
            string emltype1 = e.NewValues["eml_type"].ToString();
            if (emltype1 == "Official")
            {

                if (e.NewValues["eml_facility"] == null)
                {
                    e.NewValues["eml_facility"] = "";
                    string emlsmstype = e.NewValues["eml_facility"].ToString();
                    if (emlsmstype.Length == 0)
                    {
                        e.RowError = "Please Select Email Alert type";
                        return;
                    }
                }

            }
        }
        protected void PhoneGrid_RowValidating(object sender, DevExpress.Web.Data.ASPxDataValidationEventArgs e)
        {
            string Address = e.NewValues["phf_type"].ToString();
            if (e.IsNewRow)
            {
                DataTable dtphonefax = oDBEngine.GetDataTable("select phf_Type from tbl_master_phonefax where phf_cntid='" + HttpContext.Current.Session["KeyVal_InternalID"].ToString() + "' and ISNULL(phf_status,'Y')='Y'");

                for (int m = 0; m < dtphonefax.Rows.Count; m++)
                {
                    if (dtphonefax.Rows[m]["phf_type"].ToString() == Address.ToString())
                    {
                        e.RowError = "[ " + Address + " ]" + "Phone Type is Already Exist";
                        return;
                    }
                }
            }
            else
            {
                string addressold = e.OldValues["phf_type"].ToString();
                DataTable dtphonefax = oDBEngine.GetDataTable("select phf_Type from tbl_master_phonefax where phf_cntid='" + HttpContext.Current.Session["KeyVal_InternalID"].ToString() + "' and ISNULL(phf_status,'Y')='Y'");

                if (addressold != Address)
                {
                    for (int m = 0; m < dtphonefax.Rows.Count; m++)
                    {
                        if (dtphonefax.Rows[m]["phf_type"].ToString() == Address.ToString())
                        {
                            e.RowError = "[ " + Address + " ]" + "Phone Type is Already Exist";
                            return;

                        }
                    }
                }

            }



            string PhoneType = e.NewValues["phf_type"].ToString();
            if (PhoneType == "Mobile")
            {
                string PhoneNumber = e.NewValues["phf_phoneNumber"].ToString();
                if (e.NewValues["phf_SMSFacility"] == null)
                {
                    e.NewValues["phf_SMSFacility"] = "";
                    string smstype = e.NewValues["phf_SMSFacility"].ToString();
                    if (smstype.Length == 0)
                    {
                        e.RowError = "Please Select Sms Alert type";
                        return;
                    }
                }
                if (PhoneNumber.Length != 10)
                {
                    e.RowError = "Enter Valid Mobile Number";
                    return;
                }
            }
        }
        protected void ASPxPageControl1_ActiveTabChanged(object source, TabControlEventArgs e)
        {

        }
        protected void EmailGrid_CustomJSProperties(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            //e.Properties["cpHeight"] = emailid;
        }

        protected void ASPxCallbackPanel1_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {

            string[] data = e.Parameter.Split('~');
            if (data[0] == "Edit")
            {
                DataTable dtAdd = oDBEngine.GetDataTable("tbl_master_address ", " add_status  ,add_StatusChangeDate,  add_statusChangeReason  ", "add_id ='" + data[1].ToString() + "'");
                if (dtAdd.Rows[0]["add_status"].ToString() != "")
                {
                    cmbStatus.SelectedValue = dtAdd.Rows[0]["add_status"].ToString();
                }
                else
                {
                    cmbStatus.SelectedValue = "N";
                }
                if (dtAdd.Rows[0]["add_StatusChangeDate"].ToString() != "")
                {
                    StDate.Value = Convert.ToDateTime(dtAdd.Rows[0]["add_StatusChangeDate"].ToString());
                }
                else
                {
                    //StDate.Value = Convert.ToDateTime(oDBEngine.GetDate());
                    StDate.Value = oDBEngine.GetDate();
                }
                txtReason.Text = dtAdd.Rows[0]["add_statusChangeReason"].ToString();

            }
            else if (data[0] == "SaveOld")
            {
                int i = oDBEngine.SetFieldValue("tbl_master_address", " add_status='" + cmbStatus.SelectedItem.Value + "'  ,add_StatusChangeDate='" + StDate.Value + "',  add_statusChangeReason='" + txtReason.Text + "'  ", " add_id ='" + data[1].ToString() + "'");
                if (i == 1)
                {
                    Stat = "Y";
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
                Address.SelectCommand = "select DISTINCT  tbl_master_address.add_id AS Id, tbl_master_address.add_addressType AS Type,tbl_master_address.add_address1 AS Address1,  tbl_master_address.add_address2 AS Address2,tbl_master_address.add_address3 AS Address3,tbl_master_address.add_landMark AS LandMark,tbl_master_address.add_country AS Country,tbl_master_address.add_state AS State,tbl_master_address.add_city AS City,CASE isnull(add_country, '')WHEN '' THEN '0' ELSE(SELECT cou_country FROM tbl_master_country WHERE cou_id = add_country) END AS Country1,CASE isnull(add_state, '') WHEN '' THEN '0' ELSE(SELECT state FROM tbl_master_state WHERE id = add_state) END AS State1,CASE isnull(add_city, '') WHEN '' THEN '0' ELSE(SELECT city_name FROM tbl_master_city WHERE city_id = add_city) END AS City1,CASE add_area WHEN '' THEN '0' Else(select area_name From tbl_master_area Where area_id = add_area) End AS area,tbl_master_address.add_pin AS PinCode, tbl_master_address.add_landMark AS LankMark, case when add_status='N' then 'Deactive' else 'Active' end as status from tbl_master_address where add_cntId='" + Session["KeyVal_InternalID_New"].ToString() + "'";
                AddressGrid.DataBind();
            }
        }



        protected void ASPxCallbackPanelP_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {

            string[] data = e.Parameter.Split('~');
            if (data[0] == "Edit")
            {
                DataTable dtAdd = oDBEngine.GetDataTable("tbl_master_phonefax ", "  phf_Status  ,phf_StatusChangeDate,  phf_StatusChangeReason ", "phf_id ='" + data[1].ToString() + "'");
                if (dtAdd.Rows[0]["phf_Status"].ToString() != "")
                {
                    cmbStatusP.SelectedValue = dtAdd.Rows[0]["phf_Status"].ToString();
                }
                else
                {
                    cmbStatusP.SelectedValue = "N";
                }
                if (dtAdd.Rows[0]["phf_StatusChangeDate"].ToString() != "")
                {
                    StDateP.Value = Convert.ToDateTime(dtAdd.Rows[0]["phf_StatusChangeDate"].ToString());
                }
                else
                {
                    //StDateP.Value = Convert.ToDateTime(oDBEngine.GetDate());
                    StDateP.Value = oDBEngine.GetDate();
                }
                txtReasonP.Text = dtAdd.Rows[0]["phf_StatusChangeReason"].ToString();

            }
            else if (data[0] == "SaveOld")
            {
                int i = oDBEngine.SetFieldValue("tbl_master_phonefax", " phf_Status='" + cmbStatusP.SelectedItem.Value + "'  ,phf_StatusChangeDate='" + StDateP.Value + "',  phf_StatusChangeReason='" + txtReasonP.Text + "'  ", " phf_id ='" + data[1].ToString() + "'");
                if (i == 1)
                {
                    Stat = "Y";
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

                Phone.SelectCommand = "select DISTINCT phf_id,phf_cntId,phf_entity,phf_type,phf_countryCode,phf_areaCode,phf_phoneNumber,phf_extension, ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' + ISNULL(phf_phoneNumber, '') +' '+ ISNULL(phf_extension, '') + ISNULL(phf_faxNumber, '') AS Number,case when phf_Status='N' then 'Deactive' else 'Active' end as status ,isnull(phf_SMSFacility,'') as  phf_SMSFacility from tbl_master_phonefax where phf_cntId='" + Session["KeyVal_InternalID_New"].ToString() + "'";
                PhoneGrid.DataBind();
            }
        }



        protected void ASPxCallbackPanelE_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {

            string[] data = e.Parameter.Split('~');
            if (data[0] == "Edit")
            {
                DataTable dtAdd = oDBEngine.GetDataTable("tbl_master_email ", "   eml_Status  ,eml_StatusChangeDate,  eml_StatusChangeReason ", "eml_id ='" + data[1].ToString() + "'");
                if (dtAdd.Rows[0]["eml_Status"].ToString() != "")
                {
                    cmbStatusE.SelectedValue = dtAdd.Rows[0]["eml_Status"].ToString();
                }
                else
                {
                    cmbStatusE.SelectedValue = "N";
                }
                if (dtAdd.Rows[0]["eml_StatusChangeDate"].ToString() != "")
                {
                    StDateE.Value = Convert.ToDateTime(dtAdd.Rows[0]["eml_StatusChangeDate"].ToString());
                }
                else
                {
                    //StDateE.Value = Convert.ToDateTime(oDBEngine.GetDate());
                    StDateE.Value = oDBEngine.GetDate();
                }
                txtReasonE.Text = dtAdd.Rows[0]["eml_StatusChangeReason"].ToString();

            }
            else if (data[0] == "SaveOld")
            {
                int i = oDBEngine.SetFieldValue("tbl_master_email", " eml_Status='" + cmbStatusE.SelectedItem.Value + "'  ,eml_StatusChangeDate='" + StDateE.Value + "',  eml_StatusChangeReason='" + txtReasonE.Text + "'  ", " eml_id ='" + data[1].ToString() + "'");
                if (i == 1)
                {
                    Stat = "Y";
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
                Email.SelectCommand = "select eml_id,eml_cntId,eml_entity,eml_type,eml_email,eml_ccEmail,eml_website,CreateDate,CreateUser,case when eml_Status='N' then 'Deactive' else 'Active' end as status from tbl_master_email where eml_cntId='" + Session["KeyVal_InternalID_New"].ToString() + "'";

                EmailGrid.DataBind();
            }
        }

        protected void AddressGrid_RowValidating(object sender, DevExpress.Web.Data.ASPxDataValidationEventArgs e)
        {
            string Address = e.NewValues["Type"].ToString();
            if (e.IsNewRow)
            {
                DataTable dtadd = oDBEngine.GetDataTable("select add_addressType from tbl_master_address where add_cntid='" + HttpContext.Current.Session["KeyVal_InternalID"].ToString() + "' and ISNULL(add_status,'Y')='Y'");

                for (int m = 0; m < dtadd.Rows.Count; m++)
                {
                    if (dtadd.Rows[m]["add_addressType"].ToString() == Address.ToString())
                    {
                        //if (dtphone.Rows[m]["phf_status"].ToString() == status.ToString())
                        //{

                        e.RowError = "[ " + Address + " ]" + "Address Type is Already Exist";
                        return;

                        // }
                    }
                }
            }
            else
            {
                string addressold = e.OldValues["Type"].ToString();
                DataTable dtadd = oDBEngine.GetDataTable("select add_addressType from tbl_master_address where add_cntid='" + HttpContext.Current.Session["KeyVal_InternalID"].ToString() + "' and ISNULL(add_status,'Y')='Y'");

                if (addressold != Address)
                {
                    for (int m = 0; m < dtadd.Rows.Count; m++)
                    {
                        if (dtadd.Rows[m]["add_addressType"].ToString() == Address.ToString())
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
                    if (countryname[0, 0] == "India")
                    {
                        double Num;
                        bool isNum = double.TryParse(pin, out Num);
                        int len = pin.Length;
                        if (!isNum || len != 6)
                        {
                            // e.Errors[dataColumn] = "Enter Valid PinCode";
                            e.RowError = "Enter Valid PinCode";
                            return;
                        }
                    }
                    //if (Convert.ToString(e.NewValues["PinCode"]) == "111")
                    //    e.Errors[dataColumn] = "Value cannot be null.";
                }
            }


        }

        protected void ASPxPageControl2_ActiveTabChanged(object source, TabControlEventArgs e)
        {

        }
    }
}