using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
////using DevExpress.Web.ASPxClasses;
//using DevExpress.Web;
using DevExpress.Web;
using BusinessLogicLayer;

namespace ERP.OMS.Management.Master
{
    public partial class management_master_rootComp_Correspondence_old : ERP.OMS.ViewState_class.VSPage
    {
        /* For Tier Structure
        DBEngine oDBEngine = new DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        */

        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        BusinessLogicLayer.Company oRootCompaniesGeneralBL = new BusinessLogicLayer.Company();

        public string pageAccess = "";
        // DBEngine oDBEngine = new DBEngine(string.Empty);

        protected void Page_Load(object sender, EventArgs e)
        {
            Session["ContactType"] = "Company";
            LitCompName.Text = "Company Name :" + "  " + Session["CompanyName"].ToString();
            Session["requesttype"] = "Companies";
            string intid = Session["KeyVal_InternalID"].ToString();
            DataTable dtcmp = oDBEngine.GetDataTable(" tbl_master_address  ", "*", "add_cntId='" + intid + "'");
            Address.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            CountrySelect.ConnectionString=Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            StateSelect.ConnectionString=Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            SelectCity.ConnectionString=Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            SelectArea.ConnectionString=Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            Phone.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            Email.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            if (dtcmp.Rows.Count == 0)
            {
                try
                {
                    /*
                    String con = ConfigurationSettings.AppSettings["DBConnectionDefault"];
                    SqlConnection lcon = new SqlConnection(con);
                    lcon.Open();
                    SqlCommand lcmdEmplInsert = new SqlCommand("AdressDummyInsert", lcon);
                    lcmdEmplInsert.CommandType = CommandType.StoredProcedure;
                    lcmdEmplInsert.Parameters.AddWithValue("@contacttype", "Company");
                    lcmdEmplInsert.Parameters.AddWithValue("@InternalId", intid);
                    lcmdEmplInsert.ExecuteNonQuery();
                    */

                    oRootCompaniesGeneralBL.Insert_DummyAdress("Company", intid);

                }
                catch
                {
                }
            }
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
        protected void AddressGrid_RowValidating(object sender, DevExpress.Web.Data.ASPxDataValidationEventArgs e)
        {
            string Address = e.NewValues["Type"].ToString();


            //string status = e.NewValues["phf_status"].ToString();
            //DataTable dtphone = oDBEngine.GetDataTable("select phf_type,isnull(phf_status,'Y') as phf_status from tbl_master_phonefax where phf_cntid='" + HttpContext.Current.Session["KeyVal_InternalID"].ToString() + "'");
            if (e.IsNewRow)
            {
                DataTable dtadd = oDBEngine.GetDataTable("select add_addressType from tbl_master_address where add_cntid='" + HttpContext.Current.Session["KeyVal_InternalID"].ToString() + "'");

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
                DataTable dtadd = oDBEngine.GetDataTable("select add_addressType from tbl_master_address where add_cntid='" + HttpContext.Current.Session["KeyVal_InternalID"].ToString() + "'");

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
        }

        protected void EmailGrid_RowValidating(object sender, DevExpress.Web.Data.ASPxDataValidationEventArgs e)
        {
            string emailtype = e.NewValues["eml_type"].ToString();

            string email = "";
            if (e.IsNewRow)
            {
                DataTable dtemail = oDBEngine.GetDataTable("select eml_type from tbl_master_email where eml_cntid='" + HttpContext.Current.Session["KeyVal_InternalID"].ToString() + "'");
                for (int m = 0; m < dtemail.Rows.Count; m++)
                {
                    if (dtemail.Rows[m]["eml_type"].ToString() == emailtype.ToString())
                    {
                        e.RowError = "Type is Already Exist";
                        return;
                    }
                }
            }
            string ccEmail = "";
            if (emailtype != "Web Site")
            {
                email = e.NewValues["eml_email"].ToString();
                try
                {
                    ccEmail = e.NewValues["eml_ccEmail"].ToString();
                }
                catch
                {
                }
            }
            else
                email = e.NewValues["eml_website"].ToString();
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
            //else
            //{
            //    e.RowError = "Email Id Already Exists";
            //    return;
            //}
        }
        protected void PhoneGrid_RowValidating(object sender, DevExpress.Web.Data.ASPxDataValidationEventArgs e)
        {
            string PhoneType = e.NewValues["phf_type"].ToString();
            //string status = e.NewValues["phf_status"].ToString();
            //DataTable dtphone = oDBEngine.GetDataTable("select phf_type,isnull(phf_status,'Y') as phf_status from tbl_master_phonefax where phf_cntid='" + HttpContext.Current.Session["KeyVal_InternalID"].ToString() + "'");
            DataTable dtphone = oDBEngine.GetDataTable("select phf_type from tbl_master_phonefax where phf_cntid='" + HttpContext.Current.Session["KeyVal_InternalID"].ToString() + "'");
            if (PhoneType == "Mobile")
            {
                string PhoneNumber = e.NewValues["phf_phoneNumber"].ToString();
                if (PhoneNumber.Length != 10)
                {
                    e.RowError = "Enter Valid Mobile Number";
                    return;
                }
            }
            for (int m = 0; m < dtphone.Rows.Count; m++)
            {
                if (dtphone.Rows[m]["phf_type"].ToString() == PhoneType.ToString())
                {
                    //if (dtphone.Rows[m]["phf_status"].ToString() == status.ToString())
                    //{

                    e.RowError = "Type is Already Exist";
                    return;
                    // }
                }
            }
        }
    }
}