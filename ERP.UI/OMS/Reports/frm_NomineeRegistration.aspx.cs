using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
namespace ERP.OMS.Reports
{
    public partial class Reports_frm_NomineeRegistration : System.Web.UI.Page
    {
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        BusinessLogicLayer.Reports oReports = new BusinessLogicLayer.Reports();

        string strErrMsg = "";
        ExcelFile objExcel = new ExcelFile();
        protected void Page_Init(object sender, EventArgs e)
        {
            sqlDsNominees.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            SqlDSCountry.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);

        }
        protected void Page_Load(object sender, EventArgs e)
        {

            if (HttpContext.Current.Session["userid"] == null)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }

            if (!IsPostBack)
            {
                //***
                //Session["userlastsegment"] = "10";//9,10
                //***


                ////DataTable dtCountry = oDBEngine.GetDataTable("Master_Countries", "Country_ShortName,Country_ID", null);
                ////drpCountry.DataSource = dtCountry;
                ////drpCountry.DataTextField = "Country_ShortName";
                ////drpCountry.DataValueField = "Country_ID";
                ////drpCountry.DataBind();
                ////drpCountry.Items.Insert(0, new ListItem("Select", "Select"));
                ////drpGuardianCountry.DataSource = dtCountry;
                ////drpGuardianCountry.DataTextField = "Country_ShortName";
                ////drpGuardianCountry.DataValueField = "Country_ID";
                ////drpGuardianCountry.DataBind();
                ////drpGuardianCountry.Items.Insert(0, new ListItem("Select", "Select"));


                //Page.ClientScript.RegisterStartupScript(GetType(), "minorjs", "<script>ShowHideMinorDetails('N');</script>");
            }
            BindGrid();
            //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");
        }
        protected void grdNominees_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            string strPara = e.Parameters;
            if (strPara != "")
            {
                if (strPara.Split('~')[0] == "D")
                {
                    using (SqlConnection objCon = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"])))
                    {

                        using (SqlCommand objCom = new SqlCommand())
                        {
                            objCom.CommandType = CommandType.Text;
                            objCom.Connection = objCon;

                            if (Convert.ToString(HttpContext.Current.Session["userlastsegment"]) == "9")
                            {
                                objCom.CommandText = "Delete from Master_NSDLNomineeRegister where NSDLNomineeRegister_ID=" + strPara.Split('~')[1];

                            }
                            else if (Convert.ToString(HttpContext.Current.Session["userlastsegment"]) == "10")
                            {
                                objCom.CommandText = "Delete from Master_CDSLNomineeRegister where CDSLNomineeRegister_ID=" + strPara.Split('~')[1];

                            }

                            objCon.Open();
                            int efectedrows = objCom.ExecuteNonQuery();
                            objCon.Close();
                            if (efectedrows > 0)
                                BindGrid();
                            else
                                strErrMsg = "D~no";

                        }

                    }

                }
                else if (strPara == "s")
                {
                    //if (Convert.ToString(HttpContext.Current.Session["userlastsegment"]) == "9")
                    //{
                    //    sqlDsNominees.SelectCommand = "select  NSDLNomineeRegister_ID as NomineeRegisterID, NSDLNomineeRegister_ResgitrationNumber as RegNo,NSDLNomineeRegister_BenID as BenID,convert(varchar, NSDLNomineeRegister_RegistrationDate,101) as RegDate,convert(varchar, NSDLNomineeRegister_RegistrationDate,106) as RegDateShow,NSDLNomineeRegister_Name as Name,NSDLNomineeRegister_Country as country,NSDLNomineeRegister_Address as address,NSDLNomineeRegister_State as state,NSDLNomineeRegister_City as city,NSDLNomineeRegister_PinCode as pincode,NSDLNomineeRegister_IsMinor as minor,NSDLNomineeRegister_DOBMinor as dobminor,NSDLNomineeRegister_GuardianAddress as gaddress,NSDLNomineeRegister_GuardianCountry as gcountry,NSDLNomineeRegister_GuardianState as gstate,NSDLNomineeRegister_GuardianCity as gcity,NSDLNomineeRegister_GuardianPinCode as gpin,NSDLNomineeRegister_NoNominationFlag as nomination,NSDLNomineeRegister_PhoneResidence as resphone,NSDLNomineeRegister_PhoneMobile as mob,NSDLNomineeRegister_Email as email,NSDLNomineeRegister_Remarks as remarks,cast(NsdlClients_BenAccountID as varchar)+' ['+rtrim(ltrim(NsdlClients_BenFirstHolderName))+']' as BenAcc,NsdlClients_BenFirstHolderName as BenAccName from Master_NSDLNomineeRegister nr left outer join master_nsdlclients c on nr.NSDLNomineeRegister_BenID=c.NsdlClients_BenAccountID order by NSDLNomineeRegister_RegistrationDate desc";
                    //    grdNominees.DataBind();

                    //}
                    //else if (Convert.ToString(HttpContext.Current.Session["userlastsegment"]) == "10")
                    //{
                    //    sqlDsNominees.SelectCommand = "select CDSLNomineeRegister_ID as NomineeRegisterID,CDSLNomineeRegister_ResgitrationNumber as RegNo,CDSLNomineeRegister_BenID as BenID,convert(varchar,CDSLNomineeRegister_RegistrationDate,101) as RegDate,convert(varchar,CDSLNomineeRegister_RegistrationDate,106) as RegDateShow,CDSLNomineeRegister_Name as Name,CDSLNomineeRegister_Country as country,CDSLNomineeRegister_Address as address,CDSLNomineeRegister_State as  state,CDSLNomineeRegister_City as city,CDSLNomineeRegister_PinCode as pincode,CDSLNomineeRegister_IsMinor as minor,CDSLNomineeRegister_DOBMinor as dobminor,CDSLNomineeRegister_GuardianAddress as gaddress,CDSLNomineeRegister_GuardianCountry as gcountry,CDSLNomineeRegister_GuardianState as gstate,CDSLNomineeRegister_GuardianCity as  gcity,CDSLNomineeRegister_GuardianPinCode as gpin,CDSLNomineeRegister_NoNominationFlag as nomination,CDSLNomineeRegister_PhoneResidence as resphone,CDSLNomineeRegister_PhoneMobile as mob,CDSLNomineeRegister_Email as email,CDSLNomineeRegister_Remarks as remarks,cast(CdslClients_BenAccountNumber as varchar)+' ['+rtrim(ltrim(CdslClients_FirstHolderName))+']' as BenAcc,CdslClients_FirstHolderName as BenAccName from Master_CDSLNomineeRegister nr left outer join master_cdslclients c on nr.CDSLNomineeRegister_BenID=c.CdslClients_BenAccountNumber order by CDSLNomineeRegister_RegistrationDate desc";
                    //    grdNominees.DataBind();

                    //}

                    grdNominees.Settings.ShowFilterRow = true;


                }
                else if (strPara == "All")
                    grdNominees.FilterExpression = string.Empty;




            }
            ////if (Convert.ToString(HttpContext.Current.Session["userlastsegment"]) == "9")
            ////{
            ////    sqlDsNominees.SelectCommand = "select NSDLNomineeRegister_ID as NomineeRegisterID, NSDLNomineeRegister_ResgitrationNumber as RegNo,NSDLNomineeRegister_BenID as BenID,NSDLNomineeRegister_RegistrationDate as RegDate,NSDLNomineeRegister_Name as Name from Master_NSDLNomineeRegister";
            ////    grdNominees.DataBind();

            ////}
            ////else if (Convert.ToString(HttpContext.Current.Session["userlastsegment"]) == "10")
            ////{
            ////    sqlDsNominees.SelectCommand = "select CDSLNomineeRegister_ID as NomineeRegisterID,CDSLNomineeRegister_ResgitrationNumber as RegNo,CDSLNomineeRegister_BenID as BenID,CDSLNomineeRegister_RegistrationDate as RegDate,CDSLNomineeRegister_Name as Name from Master_CDSLNomineeRegister";
            ////    grdNominees.DataBind();

            ////}


        }
        protected void grdNominees_CustomJSProperties(object sender, DevExpress.Web.ASPxGridViewClientJSPropertiesEventArgs e)
        {
            e.Properties["cpmsg"] = strErrMsg;
        }
        protected void grdNominees_OnHtmlDataCellPrepared(object sender, DevExpress.Web.ASPxGridViewTableDataCellEventArgs e)
        {

        }
        protected void grdNominees_HtmlEditFormCreated(object sender, DevExpress.Web.ASPxGridViewEditFormEventArgs e)
        {
            ASPxComboBox cmbCountry = (ASPxComboBox)grdNominees.FindEditFormTemplateControl("combCountry");
            ASPxComboBox cmbCountryGuardian = (ASPxComboBox)grdNominees.FindEditFormTemplateControl("combGuardianCountry");
            SqlDSCountry.SelectCommand = "select Country_ShortName,Country_ID from Master_Countries";
            // DataTable dtCountry = oDBEngine.GetDataTable("Master_Countries", "Country_ShortName,Country_ID", null);
            //cmbCountry.DataSource = dtCountry;
            cmbCountry.DataBind();
            cmbCountry.Items.Insert(0, new ListEditItem("Select", DBNull.Value));
            cmbCountryGuardian.DataBind();
            cmbCountryGuardian.Items.Insert(0, new ListEditItem("Select", DBNull.Value));
            TextBox txtBenID = (TextBox)grdNominees.FindEditFormTemplateControl("txtBenId");
            if (Convert.ToString(HttpContext.Current.Session["userlastsegment"]) == "9")
                txtBenID.Attributes.Add("onkeyup", "FetchBenIdNSDL(this,'GenericAjaxList',event)");
            else if (Convert.ToString(HttpContext.Current.Session["userlastsegment"]) == "10")
                txtBenID.Attributes.Add("onkeyup", "FetchBenIdCDSL(this,'GenericAjaxList',event)");

            ASPxDateEdit dtRegDate = (ASPxDateEdit)grdNominees.FindEditFormTemplateControl("dtRegistrationDate");
            ASPxComboBox cmbIsMinor = (ASPxComboBox)grdNominees.FindEditFormTemplateControl("combIsMinor");
            ASPxComboBox cmbGState = (ASPxComboBox)grdNominees.FindEditFormTemplateControl("combGuardianState");
            ASPxComboBox cmbGCity = (ASPxComboBox)grdNominees.FindEditFormTemplateControl("combGuardianCity");
            ASPxDateEdit dtDOBMinor = (ASPxDateEdit)grdNominees.FindEditFormTemplateControl("dtDOBMinor");
            ASPxTextBox txtGAddress = (ASPxTextBox)grdNominees.FindEditFormTemplateControl("txtGuardianAddress");
            ASPxTextBox txtGPin = (ASPxTextBox)grdNominees.FindEditFormTemplateControl("txtGuardianPin");

            if (!grdNominees.IsNewRowEditing)
            {
                // HtmlTableRow trAddress = (HtmlTableRow)grdNominees.FindEditFormTemplateControl("trGuardianAddress");
                //trAddress.Visible = false;
                // trAddress.Attributes.Add("style", "display:none");


                Label lblMinor = (Label)grdNominees.FindEditFormTemplateControl("lblMinor");
                cmbIsMinor.Value = lblMinor.Text;
                if (Convert.ToString(cmbIsMinor.Value) == "Y")
                {
                    ////cmbCountryGuardian.Enabled = true;
                    ////cmbGState.Enabled = true;
                    ////cmbGCity.Enabled = true;
                    //////dtDOBMinor.Enabled = true;
                    //////dtDOBMinor.ReadOnly = false;
                    ////txtGAddress.Enabled = true;
                    ////txtGPin.Enabled = true;
                    strErrMsg = "edit~Y";
                }
                else
                {
                    ////cmbCountryGuardian.Enabled = false;
                    ////cmbGState.Enabled = false;
                    ////cmbGCity.Enabled = false;
                    //////dtDOBMinor.Enabled = false;
                    //////dtDOBMinor.ReadOnly = true;
                    ////txtGAddress.Enabled = false;
                    ////txtGPin.Enabled = false;
                    strErrMsg = "edit~N";
                }


                Label lblRegDate = (Label)grdNominees.FindEditFormTemplateControl("lblRegDate");
                dtRegDate.Date = Convert.ToDateTime(lblRegDate.Text);


                Label lblDOBMinor = (Label)grdNominees.FindEditFormTemplateControl("lblDOBMinor");
                if (lblDOBMinor.Text != "")
                    dtDOBMinor.Date = Convert.ToDateTime(lblDOBMinor.Text);


                Label lblGCountry = (Label)grdNominees.FindEditFormTemplateControl("lblGuardianCountry");
                Label lblGState = (Label)grdNominees.FindEditFormTemplateControl("lblGuardianState");
                Label lblGCity = (Label)grdNominees.FindEditFormTemplateControl("lblGuardianCity");

                Label lblCountry = (Label)grdNominees.FindEditFormTemplateControl("lblCountry");
                Label lblState = (Label)grdNominees.FindEditFormTemplateControl("lblState");
                Label lblCity = (Label)grdNominees.FindEditFormTemplateControl("lblCity");

                cmbCountry.Value = lblCountry.Text;

                ASPxComboBox cmbState = (ASPxComboBox)grdNominees.FindEditFormTemplateControl("combState");
                ASPxComboBox cmbCity = (ASPxComboBox)grdNominees.FindEditFormTemplateControl("combCity");

                if (lblCountry.Text != "")
                {
                    cmbState.DataSource = oDBEngine.GetDataTable("Master_States", "State_ShortName,State_ID", "State_CountryID=" + lblCountry.Text);
                    cmbState.DataBind();
                    cmbState.Items.Insert(0, new ListEditItem("Select", DBNull.Value));
                    cmbState.Value = lblState.Text;
                }

                if (lblState.Text != "")
                {
                    cmbCity.DataSource = oDBEngine.GetDataTable("Master_cities", "City_ShortName,City_ID", "City_StateID=" + lblState.Text);
                    cmbCity.DataBind();
                    cmbCity.Items.Insert(0, new ListEditItem("Select", DBNull.Value));
                    cmbCity.Value = lblCity.Text;
                }

                if (lblGCountry.Text != "")
                {
                    cmbCountryGuardian.Value = lblGCountry.Text;

                    cmbGState.DataSource = oDBEngine.GetDataTable("Master_States", "State_ShortName,State_ID", "State_CountryID=" + lblGCountry.Text);
                    cmbGState.DataBind();
                    cmbGState.Items.Insert(0, new ListEditItem("Select", DBNull.Value));
                    cmbGState.Value = lblGState.Text;
                }

                if (lblGState.Text != "")
                {
                    cmbGCity.DataSource = oDBEngine.GetDataTable("Master_cities", "City_ShortName,City_ID", "City_StateID=" + lblGState.Text);
                    cmbGCity.DataBind();
                    cmbGCity.Items.Insert(0, new ListEditItem("Select", DBNull.Value));
                    cmbGCity.Value = lblGCity.Text;
                }

                ASPxComboBox cmbNomination = (ASPxComboBox)grdNominees.FindEditFormTemplateControl("combNomination");
                Label lblNomination = (Label)grdNominees.FindEditFormTemplateControl("lblNomination");
                cmbNomination.Value = lblNomination.Text;
                // Page.ClientScript.RegisterStartupScript(GetType(), "isminorjs", "<script>ShowHideMinorDetails('N');</script>");
                // ScriptManager.RegisterStartupScript(this, GetType(), "isminorjs", "<script>ShowHideMinorDetails('N');</script>",true);
            }
            else
            {
                //dtRegDate.Date = DateTime.Today;
                dtRegDate.Date = oDBEngine.GetDate();
                //cmbIsMinor.Value = "N";
                //dtDOBMinor.Date = DateTime.Today;
                ////cmbCountryGuardian.Enabled = false;
                //////cmbGState.Enabled = false;
                //////cmbGCity.Enabled = false;
                ////dtDOBMinor.ReadOnly=true;
                ////txtGAddress.Enabled = false;
                ////txtGPin.Enabled = false;
                strErrMsg = "new";

            }
        }
        protected void grdNominees_HtmlRowCreated(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
        {

        }
        protected void grdNominees_OnRowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            string aa = Convert.ToString(e.NewValues["Name"]);
            string Country = string.Empty;
            string State = string.Empty;
            string City = string.Empty;
            string GuadianAddress = string.Empty;
            string GuardianCountry = string.Empty;
            string GuardianState = string.Empty;
            string GuardianCity = string.Empty;
            string GuardianPinCode = string.Empty;
            string DOBMinor = string.Empty;
            ASPxComboBox cmbCountry = (ASPxComboBox)grdNominees.FindEditFormTemplateControl("combCountry");
            ASPxComboBox cmbState = (ASPxComboBox)grdNominees.FindEditFormTemplateControl("combState");
            ASPxComboBox cmbCity = (ASPxComboBox)grdNominees.FindEditFormTemplateControl("combCity");

            ASPxComboBox cmbGCountry = (ASPxComboBox)grdNominees.FindEditFormTemplateControl("combGuardianCountry");
            ASPxComboBox cmbGState = (ASPxComboBox)grdNominees.FindEditFormTemplateControl("combGuardianState");
            ASPxComboBox cmbGCity = (ASPxComboBox)grdNominees.FindEditFormTemplateControl("combGuardianCity");

            ASPxDateEdit dtRegDate = (ASPxDateEdit)grdNominees.FindEditFormTemplateControl("dtRegistrationDate");
            ASPxDateEdit dtDOBMinor = (ASPxDateEdit)grdNominees.FindEditFormTemplateControl("dtDOBMinor");

            TextBox txtBenID = (TextBox)grdNominees.FindEditFormTemplateControl("txtBenId");
            ASPxTextBox txtGAddress = (ASPxTextBox)grdNominees.FindEditFormTemplateControl("txtGuardianAddress");

            ASPxComboBox cmbIsMinor = (ASPxComboBox)grdNominees.FindEditFormTemplateControl("combIsMinor");
            ASPxComboBox cmbNomination = (ASPxComboBox)grdNominees.FindEditFormTemplateControl("combNomination");



            if (cmbCountry.SelectedItem != null)
            {
                if (cmbCountry.SelectedItem.Text != "Select")
                    Country = Convert.ToString(cmbCountry.SelectedItem.Value);
                else
                    Country = null;

            }
            else
                Country = null;
            if (cmbState.SelectedItem != null)
            {
                if (cmbState.SelectedItem.Text != "Select")
                    State = Convert.ToString(cmbState.SelectedItem.Value);
                else
                    State = null;
            }
            else
                State = null;

            if (cmbCity.SelectedItem != null)
            {
                if (cmbCity.SelectedItem.Text != "Select")
                    City = Convert.ToString(cmbCity.SelectedItem.Value);
                else
                    City = null;

            }
            else
                City = null;


            if (Convert.ToString(cmbIsMinor.SelectedItem.Value) == "Y")
            {
                GuadianAddress = Convert.ToString(txtGAddress.Text);
                if (cmbGCountry.SelectedItem != null)
                    GuardianCountry = Convert.ToString(cmbGCountry.SelectedItem.Value);
                else
                    GuardianCountry = null;
                if (cmbGState.SelectedItem != null)
                {
                    if (cmbGState.SelectedItem.Text != "Select")
                        GuardianState = Convert.ToString(cmbGState.SelectedItem.Value);
                    else
                        GuardianState = null;

                }
                else
                    GuardianState = null;
                if (cmbGCity.SelectedItem != null)
                {
                    if (cmbGCity.SelectedItem.Text != "Select")
                        GuardianCity = Convert.ToString(cmbGCity.SelectedItem.Value);
                    else
                        GuardianCity = null;
                }
                else
                    GuardianCity = null;
                GuardianPinCode = Convert.ToString(e.NewValues["gpin"]);
                if (dtDOBMinor.Text != "")
                    DOBMinor = Convert.ToString(dtDOBMinor.Text.Split('-')[1] + "-" + dtDOBMinor.Text.Split('-')[0] + "-" + dtDOBMinor.Text.Split('-')[2]);
                else
                    DOBMinor = null;
            }
            else
            {
                GuadianAddress = null;
                GuardianCountry = null;
                GuardianState = null;
                GuardianCity = null;
                GuardianPinCode = null;
                DOBMinor = null;


            }

            oReports.Insert_NomineeRegister(
                Convert.ToString(e.NewValues["RegNo"]),
                   Convert.ToString(dtRegDate.Text.Split('-')[1] + "-" + dtRegDate.Text.Split('-')[0] + "-" + dtRegDate.Text.Split('-')[2]),
                   Convert.ToString(txtBenID.Text.Trim().Split(' ')[0]),
                   Convert.ToString(e.NewValues["Name"]),
                   Convert.ToString(e.NewValues["address"]),
                   Convert.ToString(Country),
                   Convert.ToString(State),
                   Convert.ToString(City),
                   Convert.ToString(e.NewValues["pincode"]),
                   Convert.ToString(cmbIsMinor.SelectedItem.Value),
                   Convert.ToString(GuadianAddress),
                   Convert.ToString(GuardianCountry),
                   Convert.ToString(GuardianState),
                   Convert.ToString(GuardianCity),
                   Convert.ToString(GuardianPinCode),
                   Convert.ToString(DOBMinor),
                   Convert.ToString(e.NewValues["resphone"]),
                    Convert.ToString(e.NewValues["mob"]),
                    Convert.ToString(e.NewValues["email"]),
                    Convert.ToString(e.NewValues["remarks"]),
                    Convert.ToString(cmbNomination.SelectedItem.Value),
                    Convert.ToString(HttpContext.Current.Session["usersegid"]),
                    Convert.ToString(HttpContext.Current.Session["userlastsegment"]),
                    Convert.ToString(HttpContext.Current.Session["userid"]),
                    "insert"
                );

            //   using (SqlConnection objCon = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))
            //  {

            //   using (SqlCommand objCom = new SqlCommand("Insert_NomineeRegister", objCon))
            //   {
            //objCom.CommandType = CommandType.StoredProcedure;

            //objCom.Parameters.AddWithValue("@RegistrationNo", e.NewValues["RegNo"]);
            //objCom.Parameters.AddWithValue("@RegistrationDate", dtRegDate.Text.Split('-')[1] + "-" + dtRegDate.Text.Split('-')[0] + "-" + dtRegDate.Text.Split('-')[2]);
            //objCom.Parameters.AddWithValue("@BenID", txtBenID.Text.Trim().Split(' ')[0]);
            //objCom.Parameters.AddWithValue("@Name", Convert.ToString(e.NewValues["Name"]));
            //objCom.Parameters.AddWithValue("@Address", e.NewValues["address"]);
            //if (cmbCountry.SelectedItem != null)
            //{
            //    if(cmbCountry.SelectedItem.Text!="Select")
            //        objCom.Parameters.AddWithValue("@Country", cmbCountry.SelectedItem.Value);
            //    else
            //        objCom.Parameters.AddWithValue("@Country", System.Data.SqlTypes.SqlInt32.Null);

            //}
            //else
            //    objCom.Parameters.AddWithValue("@Country", System.Data.SqlTypes.SqlInt32.Null);

            //if (cmbState.SelectedItem != null)
            //{
            //    if(cmbState.SelectedItem.Text!="Select")
            //    objCom.Parameters.AddWithValue("@State", cmbState.SelectedItem.Value);
            //    else
            //    objCom.Parameters.AddWithValue("@State", System.Data.SqlTypes.SqlInt32.Null);
            //}
            //else
            //    objCom.Parameters.AddWithValue("@State", System.Data.SqlTypes.SqlInt32.Null);

            //if (cmbCity.SelectedItem != null)
            //{
            //    if(cmbCity.SelectedItem.Text!="Select")
            //        objCom.Parameters.AddWithValue("@City", cmbCity.SelectedItem.Value);
            //    else
            //        objCom.Parameters.AddWithValue("@City", System.Data.SqlTypes.SqlInt32.Null);

            //}
            //else
            //    objCom.Parameters.AddWithValue("@City", System.Data.SqlTypes.SqlInt32.Null);

            //objCom.Parameters.AddWithValue("@PinCode", e.NewValues["pincode"]);
            //objCom.Parameters.AddWithValue("@IsMinor", cmbIsMinor.SelectedItem.Value);
            //if (Convert.ToString(cmbIsMinor.SelectedItem.Value) == "Y")
            //{
            //    objCom.Parameters.AddWithValue("@GuadianAddress", txtGAddress.Text);
            //    if(cmbGCountry.SelectedItem!=null)
            //        objCom.Parameters.AddWithValue("@GuardianCountry", cmbGCountry.SelectedItem.Value);
            //    else
            //        objCom.Parameters.AddWithValue("@GuardianCountry", System.Data.SqlTypes.SqlInt32.Null);
            //    if (cmbGState.SelectedItem != null)
            //    {
            //        if(cmbGState.SelectedItem.Text!="Select")
            //            objCom.Parameters.AddWithValue("@GuardianState", cmbGState.SelectedItem.Value);
            //        else
            //            objCom.Parameters.AddWithValue("@GuardianState", System.Data.SqlTypes.SqlInt32.Null);

            //    }
            //    else
            //        objCom.Parameters.AddWithValue("@GuardianState", System.Data.SqlTypes.SqlInt32.Null);
            //    if (cmbGCity.SelectedItem != null)
            //    {
            //        if(cmbGCity.SelectedItem.Text!="Select")
            //            objCom.Parameters.AddWithValue("@GuardianCity", cmbGCity.SelectedItem.Value);
            //        else
            //            objCom.Parameters.AddWithValue("@GuardianCity", System.Data.SqlTypes.SqlInt32.Null);

            //    }
            //    else
            //        objCom.Parameters.AddWithValue("@GuardianCity", System.Data.SqlTypes.SqlInt32.Null);
            //    objCom.Parameters.AddWithValue("@GuardianPinCode", e.NewValues["gpin"]);
            //    if(dtDOBMinor.Text!="")
            //        objCom.Parameters.AddWithValue("@DOBMinor", dtDOBMinor.Text.Split('-')[1] + "-" + dtDOBMinor.Text.Split('-')[0] + "-" + dtDOBMinor.Text.Split('-')[2]);
            //    else
            //        objCom.Parameters.AddWithValue("@DOBMinor", System.Data.SqlTypes.SqlDateTime.Null);
            //}
            //else
            //{
            //    objCom.Parameters.AddWithValue("@GuadianAddress", System.Data.SqlTypes.SqlString.Null);
            //    objCom.Parameters.AddWithValue("@GuardianCountry", System.Data.SqlTypes.SqlInt32.Null);
            //    objCom.Parameters.AddWithValue("@GuardianState", System.Data.SqlTypes.SqlInt32.Null);
            //    objCom.Parameters.AddWithValue("@GuardianCity", System.Data.SqlTypes.SqlInt32.Null);
            //    objCom.Parameters.AddWithValue("@GuardianPinCode", System.Data.SqlTypes.SqlString.Null);
            //    objCom.Parameters.AddWithValue("@DOBMinor", System.Data.SqlTypes.SqlDateTime.Null);///ok


            //}
            //objCom.Parameters.AddWithValue("@ResidencePhone", e.NewValues["resphone"]);
            //objCom.Parameters.AddWithValue("@Mobile", e.NewValues["mob"]);
            //objCom.Parameters.AddWithValue("@Email", e.NewValues["email"]);
            //objCom.Parameters.AddWithValue("@Remarks", e.NewValues["remarks"]);
            //objCom.Parameters.AddWithValue("@NominationFlag", cmbNomination.SelectedItem.Value);
            //objCom.Parameters.AddWithValue("@DpId", Convert.ToString(HttpContext.Current.Session["usersegid"]));
            //objCom.Parameters.AddWithValue("@Segment", Convert.ToString(HttpContext.Current.Session["userlastsegment"]));//// 10(cdsl) 9(nsdl)
            //objCom.Parameters.AddWithValue("@User", Convert.ToString(HttpContext.Current.Session["userid"]));
            //objCom.Parameters.AddWithValue("@EntryType", "insert");

            //objCon.Open();
            //objCom.ExecuteNonQuery();

            //objCon.Close();


            //  }


            // }


            grdNominees.CancelEdit();
            e.Cancel = true;
        }
        protected void grdNominees_OnRowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            string Country = string.Empty;
            string State = string.Empty;
            string City = string.Empty;
            string GuadianAddress = string.Empty;
            string GuardianCountry = string.Empty;
            string GuardianState = string.Empty;
            string GuardianCity = string.Empty;
            string GuardianPinCode = string.Empty;
            string DOBMinor = string.Empty;
            //string aa = Convert.ToString(e.NewValues["Name"]);

            ASPxComboBox cmbCountry = (ASPxComboBox)grdNominees.FindEditFormTemplateControl("combCountry");
            ASPxComboBox cmbState = (ASPxComboBox)grdNominees.FindEditFormTemplateControl("combState");
            ASPxComboBox cmbCity = (ASPxComboBox)grdNominees.FindEditFormTemplateControl("combCity");

            ASPxComboBox cmbGCountry = (ASPxComboBox)grdNominees.FindEditFormTemplateControl("combGuardianCountry");
            ASPxComboBox cmbGState = (ASPxComboBox)grdNominees.FindEditFormTemplateControl("combGuardianState");
            ASPxComboBox cmbGCity = (ASPxComboBox)grdNominees.FindEditFormTemplateControl("combGuardianCity");

            ASPxDateEdit dtRegDate = (ASPxDateEdit)grdNominees.FindEditFormTemplateControl("dtRegistrationDate");
            ASPxDateEdit dtDOBMinor = (ASPxDateEdit)grdNominees.FindEditFormTemplateControl("dtDOBMinor");

            TextBox txtBenID = (TextBox)grdNominees.FindEditFormTemplateControl("txtBenId");
            ASPxTextBox txtGAddress = (ASPxTextBox)grdNominees.FindEditFormTemplateControl("txtGuardianAddress");

            ASPxComboBox cmbIsMinor = (ASPxComboBox)grdNominees.FindEditFormTemplateControl("combIsMinor");
            ASPxComboBox cmbNomination = (ASPxComboBox)grdNominees.FindEditFormTemplateControl("combNomination");





            if (cmbCountry.SelectedItem != null)
            {
                if (cmbCountry.SelectedItem.Text != "Select")
                    Country = Convert.ToString(cmbCountry.SelectedItem.Value);
                else
                    Country = null;

            }
            else
                Country = null;
            if (cmbState.SelectedItem != null)
            {
                if (cmbState.SelectedItem.Text != "Select")
                    State = Convert.ToString(cmbState.SelectedItem.Value);
                else
                    State = null;
            }
            else
                State = null;

            if (cmbCity.SelectedItem != null)
            {
                if (cmbCity.SelectedItem.Text != "Select")
                    City = Convert.ToString(cmbCity.SelectedItem.Value);
                else
                    City = null;

            }
            else
                City = null;


            if (Convert.ToString(cmbIsMinor.SelectedItem.Value) == "Y")
            {
                GuadianAddress = Convert.ToString(txtGAddress.Text);
                if (cmbGCountry.SelectedItem != null)
                    GuardianCountry = Convert.ToString(cmbGCountry.SelectedItem.Value);
                else
                    GuardianCountry = null;
                if (cmbGState.SelectedItem != null)
                {
                    if (cmbGState.SelectedItem.Text != "Select")
                        GuardianState = Convert.ToString(cmbGState.SelectedItem.Value);
                    else
                        GuardianState = null;

                }
                else
                    GuardianState = null;
                if (cmbGCity.SelectedItem != null)
                {
                    if (cmbGCity.SelectedItem.Text != "Select")
                        GuardianCity = Convert.ToString(cmbGCity.SelectedItem.Value);
                    else
                        GuardianCity = null;
                }
                else
                    GuardianCity = null;
                GuardianPinCode = Convert.ToString(e.NewValues["gpin"]);
                if (dtDOBMinor.Text != "")
                    DOBMinor = Convert.ToString(dtDOBMinor.Text.Split('-')[1] + "-" + dtDOBMinor.Text.Split('-')[0] + "-" + dtDOBMinor.Text.Split('-')[2]);
                else
                    DOBMinor = null;
            }
            else
            {
                GuadianAddress = null;
                GuardianCountry = null;
                GuardianState = null;
                GuardianCity = null;
                GuardianPinCode = null;
                DOBMinor = null;


            }

            oReports.Update_NomineeRegister(
                Convert.ToString(e.NewValues["RegNo"]),
                   Convert.ToString(dtRegDate.Text.Split('-')[1] + "-" + dtRegDate.Text.Split('-')[0] + "-" + dtRegDate.Text.Split('-')[2]),
                   Convert.ToString(txtBenID.Text.Trim().Split(' ')[0]),
                   Convert.ToString(e.NewValues["Name"]),
                   Convert.ToString(e.NewValues["address"]),
                   Convert.ToString(Country),
                   Convert.ToString(State),
                   Convert.ToString(City),
                   Convert.ToString(e.NewValues["pincode"]),
                   Convert.ToString(cmbIsMinor.SelectedItem.Value),
                   Convert.ToString(GuadianAddress),
                   Convert.ToString(GuardianCountry),
                   Convert.ToString(GuardianState),
                   Convert.ToString(GuardianCity),
                   Convert.ToString(GuardianPinCode),
                   Convert.ToString(DOBMinor),
                   Convert.ToString(e.NewValues["resphone"]),
                    Convert.ToString(e.NewValues["mob"]),
                    Convert.ToString(e.NewValues["email"]),
                    Convert.ToString(e.NewValues["remarks"]),
                    Convert.ToString(cmbNomination.SelectedItem.Value),
                    Convert.ToString(HttpContext.Current.Session["usersegid"]),
                    Convert.ToString(HttpContext.Current.Session["userlastsegment"]),
                    Convert.ToString(HttpContext.Current.Session["userid"]),
                    "update",
                    Convert.ToString(e.Keys[0])
                );
            //using (SqlConnection objCon = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))
            //{

            //    using (SqlCommand objCom = new SqlCommand("Insert_NomineeRegister", objCon))
            //    {
            //        objCom.CommandType = CommandType.StoredProcedure;

            //        objCom.Parameters.AddWithValue("@RegistrationNo", e.NewValues["RegNo"]);
            //        objCom.Parameters.AddWithValue("@RegistrationDate", dtRegDate.Text.Split('-')[1] + "-" + dtRegDate.Text.Split('-')[0] + "-" + dtRegDate.Text.Split('-')[2]);
            //        objCom.Parameters.AddWithValue("@BenID", txtBenID.Text.Trim().Split(' ')[0]);
            //        objCom.Parameters.AddWithValue("@Name", Convert.ToString(e.NewValues["Name"]));
            //        objCom.Parameters.AddWithValue("@Address", e.NewValues["address"]);
            //        if (cmbCountry.SelectedItem != null)
            //        {
            //            if(cmbCountry.SelectedItem.Text!="Select")
            //                objCom.Parameters.AddWithValue("@Country", cmbCountry.SelectedItem.Value);
            //            else
            //                objCom.Parameters.AddWithValue("@Country", System.Data.SqlTypes.SqlInt32.Null);
            //        }
            //        else
            //            objCom.Parameters.AddWithValue("@Country", System.Data.SqlTypes.SqlInt32.Null);

            //        if (cmbState.SelectedItem != null)
            //        {
            //            if(cmbState.SelectedItem.Text!="Select")
            //            objCom.Parameters.AddWithValue("@State", cmbState.SelectedItem.Value);
            //            else
            //            objCom.Parameters.AddWithValue("@State", System.Data.SqlTypes.SqlInt32.Null);

            //        }
            //        else
            //            objCom.Parameters.AddWithValue("@State", System.Data.SqlTypes.SqlInt32.Null);

            //        if (cmbCity.SelectedItem != null)
            //        {
            //            if(cmbCity.SelectedItem.Text!="Select")
            //                objCom.Parameters.AddWithValue("@City", cmbCity.SelectedItem.Value);
            //            else
            //                objCom.Parameters.AddWithValue("@City", System.Data.SqlTypes.SqlInt32.Null);

            //        }
            //        else
            //            objCom.Parameters.AddWithValue("@City", System.Data.SqlTypes.SqlInt32.Null);
            //        objCom.Parameters.AddWithValue("@PinCode", e.NewValues["pincode"]);
            //        objCom.Parameters.AddWithValue("@IsMinor", cmbIsMinor.SelectedItem.Value);
            //        if (Convert.ToString(cmbIsMinor.SelectedItem.Value) == "Y")
            //        {
            //            objCom.Parameters.AddWithValue("@GuadianAddress", txtGAddress.Text);
            //            if (cmbGCountry.SelectedItem != null)
            //                objCom.Parameters.AddWithValue("@GuardianCountry", cmbGCountry.SelectedItem.Value);
            //            else
            //                objCom.Parameters.AddWithValue("@GuardianCountry", System.Data.SqlTypes.SqlInt32.Null);
            //            if (cmbGState.SelectedItem != null)
            //            {
            //                if (cmbGState.SelectedItem.Text != "Select")
            //                    objCom.Parameters.AddWithValue("@GuardianState", cmbGState.SelectedItem.Value);
            //                else
            //                    objCom.Parameters.AddWithValue("@GuardianState", System.Data.SqlTypes.SqlInt32.Null);

            //            }
            //            else
            //                objCom.Parameters.AddWithValue("@GuardianState", System.Data.SqlTypes.SqlInt32.Null);
            //            if (cmbGCity.SelectedItem != null)
            //            {
            //                if (cmbGCity.SelectedItem.Text != "Select")
            //                    objCom.Parameters.AddWithValue("@GuardianCity", cmbGCity.SelectedItem.Value);
            //                else
            //                    objCom.Parameters.AddWithValue("@GuardianCity", System.Data.SqlTypes.SqlInt32.Null);

            //            }
            //            else
            //                objCom.Parameters.AddWithValue("@GuardianCity", System.Data.SqlTypes.SqlInt32.Null);
            //            objCom.Parameters.AddWithValue("@GuardianPinCode", e.NewValues["gpin"]);
            //            if (dtDOBMinor.Text != "")
            //                objCom.Parameters.AddWithValue("@DOBMinor", dtDOBMinor.Text.Split('-')[1] + "-" + dtDOBMinor.Text.Split('-')[0] + "-" + dtDOBMinor.Text.Split('-')[2]);
            //            else
            //                objCom.Parameters.AddWithValue("@DOBMinor", System.Data.SqlTypes.SqlDateTime.Null);
            //        }
            //        else
            //        {
            //            objCom.Parameters.AddWithValue("@GuadianAddress", System.Data.SqlTypes.SqlString.Null);
            //            objCom.Parameters.AddWithValue("@GuardianCountry", System.Data.SqlTypes.SqlInt32.Null);
            //            objCom.Parameters.AddWithValue("@GuardianState", System.Data.SqlTypes.SqlInt32.Null);
            //            objCom.Parameters.AddWithValue("@GuardianCity", System.Data.SqlTypes.SqlInt32.Null);
            //            objCom.Parameters.AddWithValue("@GuardianPinCode", System.Data.SqlTypes.SqlString.Null);
            //            objCom.Parameters.AddWithValue("@DOBMinor", System.Data.SqlTypes.SqlDateTime.Null);///ok


            //        }
            //        objCom.Parameters.AddWithValue("@ResidencePhone", e.NewValues["resphone"]);
            //        objCom.Parameters.AddWithValue("@Mobile", e.NewValues["mob"]);
            //        objCom.Parameters.AddWithValue("@Email", e.NewValues["email"]);
            //        objCom.Parameters.AddWithValue("@Remarks", e.NewValues["remarks"]);
            //        objCom.Parameters.AddWithValue("@NominationFlag", cmbNomination.SelectedItem.Value);
            //        objCom.Parameters.AddWithValue("@DpId", Convert.ToString(HttpContext.Current.Session["usersegid"]));
            //        objCom.Parameters.AddWithValue("@Segment", Convert.ToString(HttpContext.Current.Session["userlastsegment"]));//// 10(cdsl) 9(nsdl)
            //        objCom.Parameters.AddWithValue("@User", Convert.ToString(HttpContext.Current.Session["userid"]));
            //        objCom.Parameters.AddWithValue("@EntryType", "update");
            //        objCom.Parameters.AddWithValue("@NomineeId",Convert.ToString(e.Keys[0]));

            //        objCon.Open();
            //        objCom.ExecuteNonQuery();

            //        objCon.Close();


            //    }


            //}


            grdNominees.CancelEdit();
            e.Cancel = true;
        }
        protected void grdNominees_OnRowValidating(object sender, DevExpress.Web.Data.ASPxDataValidationEventArgs e)
        {
            if (Convert.ToString(e.NewValues["RegNo"]) == "")
                e.RowError = "Please Enter Registration No.";
        }
        protected void grdNominees_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {

        }
        protected void BindGrid()
        {

            if (Convert.ToString(HttpContext.Current.Session["userlastsegment"]) == "9")
            {
                sqlDsNominees.SelectCommand = "select top 10 NSDLNomineeRegister_ID as NomineeRegisterID, NSDLNomineeRegister_ResgitrationNumber as RegNo,NSDLNomineeRegister_BenID as BenID,convert(varchar, NSDLNomineeRegister_RegistrationDate,101) as RegDate,convert(varchar, NSDLNomineeRegister_RegistrationDate,106) as RegDateShow,NSDLNomineeRegister_Name as Name,NSDLNomineeRegister_Country as country,NSDLNomineeRegister_Address as address,NSDLNomineeRegister_State as state,NSDLNomineeRegister_City as city,NSDLNomineeRegister_PinCode as pincode,NSDLNomineeRegister_IsMinor as minor,NSDLNomineeRegister_DOBMinor as dobminor,NSDLNomineeRegister_GuardianAddress as gaddress,NSDLNomineeRegister_GuardianCountry as gcountry,NSDLNomineeRegister_GuardianState as gstate,NSDLNomineeRegister_GuardianCity as gcity,NSDLNomineeRegister_GuardianPinCode as gpin,NSDLNomineeRegister_NoNominationFlag as nomination,NSDLNomineeRegister_PhoneResidence as resphone,NSDLNomineeRegister_PhoneMobile as mob,NSDLNomineeRegister_Email as email,NSDLNomineeRegister_Remarks as remarks,cast(NsdlClients_BenAccountID as varchar)+' ['+rtrim(ltrim(NsdlClients_BenFirstHolderName))+']' as BenAcc,NsdlClients_BenFirstHolderName as BenAccName from Master_NSDLNomineeRegister nr left outer join master_nsdlclients c on nr.NSDLNomineeRegister_BenID=c.NsdlClients_BenAccountID order by NSDLNomineeRegister_RegistrationDate desc";
                grdNominees.DataBind();

            }
            else if (Convert.ToString(HttpContext.Current.Session["userlastsegment"]) == "10")
            {
                sqlDsNominees.SelectCommand = "select top 10 CDSLNomineeRegister_ID as NomineeRegisterID,CDSLNomineeRegister_ResgitrationNumber as RegNo,CDSLNomineeRegister_BenID as BenID,convert(varchar,CDSLNomineeRegister_RegistrationDate,101) as RegDate,convert(varchar,CDSLNomineeRegister_RegistrationDate,106) as RegDateShow,CDSLNomineeRegister_Name as Name,CDSLNomineeRegister_Country as country,CDSLNomineeRegister_Address as address,CDSLNomineeRegister_State as  state,CDSLNomineeRegister_City as city,CDSLNomineeRegister_PinCode as pincode,CDSLNomineeRegister_IsMinor as minor,CDSLNomineeRegister_DOBMinor as dobminor,CDSLNomineeRegister_GuardianAddress as gaddress,CDSLNomineeRegister_GuardianCountry as gcountry,CDSLNomineeRegister_GuardianState as gstate,CDSLNomineeRegister_GuardianCity as  gcity,CDSLNomineeRegister_GuardianPinCode as gpin,CDSLNomineeRegister_NoNominationFlag as nomination,CDSLNomineeRegister_PhoneResidence as resphone,CDSLNomineeRegister_PhoneMobile as mob,CDSLNomineeRegister_Email as email,CDSLNomineeRegister_Remarks as remarks,cast(CdslClients_BenAccountNumber as varchar)+' ['+rtrim(ltrim(CdslClients_FirstHolderName))+']' as BenAcc,CdslClients_FirstHolderName as BenAccName from Master_CDSLNomineeRegister nr left outer join master_cdslclients c on nr.CDSLNomineeRegister_BenID=c.CdslClients_BenAccountNumber order by CDSLNomineeRegister_RegistrationDate desc";
                grdNominees.DataBind();

            }
            //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");

        }
        protected void combState_Callback(object source, CallbackEventArgsBase e)
        {
            ASPxComboBox cmbState = (ASPxComboBox)grdNominees.FindEditFormTemplateControl("combState");
            if (e.Parameter != "")
            {

                cmbState.DataSource = oDBEngine.GetDataTable("Master_States", "State_ShortName,State_ID", "State_CountryID=" + e.Parameter);
                cmbState.DataBind();
                cmbState.Items.Insert(0, new ListEditItem("Select", DBNull.Value));
            }
            else
            {
                //ASPxComboBox cmbCity = (ASPxComboBox)grdNominees.FindEditFormTemplateControl("combCity");
                cmbState.Items.Clear();

            }

        }
        protected void combCity_Callback(object source, CallbackEventArgsBase e)
        {
            ASPxComboBox cmbCity = (ASPxComboBox)grdNominees.FindEditFormTemplateControl("combCity");
            if (e.Parameter != "")
            {
                cmbCity.DataSource = oDBEngine.GetDataTable("Master_cities", "City_ShortName,City_ID", "City_StateID=" + e.Parameter);
                cmbCity.DataBind();
                cmbCity.Items.Insert(0, new ListEditItem("Select", DBNull.Value));
            }
            else
                cmbCity.Items.Clear();


        }

        protected void combGuardianState_Callback(object source, CallbackEventArgsBase e)
        {
            ASPxComboBox cmbState = (ASPxComboBox)grdNominees.FindEditFormTemplateControl("combGuardianState");
            if (e.Parameter != "")
            {

                cmbState.DataSource = oDBEngine.GetDataTable("Master_States", "State_ShortName,State_ID", "State_CountryID=" + e.Parameter);
                cmbState.DataBind();
                cmbState.Items.Insert(0, new ListEditItem("Select", DBNull.Value));
            }
            else
                cmbState.Items.Clear();



        }
        protected void combGuardianCity_Callback(object source, CallbackEventArgsBase e)
        {
            ASPxComboBox cmbCity = (ASPxComboBox)grdNominees.FindEditFormTemplateControl("combGuardianCity");
            if (e.Parameter != "")
            {
                cmbCity.DataSource = oDBEngine.GetDataTable("Master_cities", "City_ShortName,City_ID", "City_StateID=" + e.Parameter);
                cmbCity.DataBind();
                cmbCity.Items.Insert(0, new ListEditItem("Select", DBNull.Value));
            }
            else
                cmbCity.Items.Clear();


        }


        protected void drpExport_SelectIndexChanged(object sender, EventArgs e)
        {

            DataTable dtNominees = null;

            if (Convert.ToString(HttpContext.Current.Session["userlastsegment"]) == "9")
            {
                //  dtNominees= oDBEngine.GetDataTable("Master_NSDLNomineeRegister nr left outer join master_nsdlclients c on nr.NSDLNomineeRegister_BenID=c.NsdlClients_BenAccountID", "NSDLNomineeRegister_ResgitrationNumber as RegistrationNo,convert(varchar, NSDLNomineeRegister_RegistrationDate,106) as RegistrationDate,NSDLNomineeRegister_Name as Name,NSDLNomineeRegister_Country as Country,NSDLNomineeRegister_Address as Address,NSDLNomineeRegister_State as State,NSDLNomineeRegister_City as City,NSDLNomineeRegister_PinCode as PinCode,NSDLNomineeRegister_IsMinor as Minor,NSDLNomineeRegister_DOBMinor as DOBofMinor,NSDLNomineeRegister_GuardianAddress as GuardianAddress,NSDLNomineeRegister_GuardianCountry as GuardianCountry,NSDLNomineeRegister_GuardianState as GuardianState,NSDLNomineeRegister_GuardianCity as GuardianCity,NSDLNomineeRegister_GuardianPinCode as GuardianPinCode,NSDLNomineeRegister_NoNominationFlag as NoNominationFlag,NSDLNomineeRegister_PhoneResidence as ResidencePhone,NSDLNomineeRegister_PhoneMobile as Mobile,NSDLNomineeRegister_Email as Email,NSDLNomineeRegister_Remarks as Remarks,cast(NsdlClients_BenAccountID as varchar)+' ['+rtrim(ltrim(NsdlClients_BenFirstHolderName))+']' as BenAccountNo", null, "NSDLNomineeRegister_RegistrationDate desc");
                dtNominees = oDBEngine.GetDataTable("Master_NSDLNomineeRegister nr left outer join master_nsdlclients c on nr.NSDLNomineeRegister_BenID=c.NsdlClients_BenAccountID left outer join  Master_Countries cn on nr.NSDLNomineeRegister_Country=cn.Country_ID left outer join Master_States st on nr.NSDLNomineeRegister_State=st.State_ID left outer join Master_cities ct on nr.NSDLNomineeRegister_City=ct.City_ID left outer join  Master_Countries gcn on nr.NSDLNomineeRegister_GuardianCountry=gcn.Country_ID left outer join Master_States gst on nr.NSDLNomineeRegister_GuardianState=gst.State_ID left outer join Master_cities gct on nr.NSDLNomineeRegister_GuardianCity=gct.City_ID", "NSDLNomineeRegister_ResgitrationNumber as RegistrationNo,convert(varchar, NSDLNomineeRegister_RegistrationDate,106) as RegistrationDate,NSDLNomineeRegister_Name as Name,cn.Country_ShortName as Country,NSDLNomineeRegister_Address as Address,st.State_ShortName as State,ct.City_ShortName as City,NSDLNomineeRegister_PinCode as PinCode,case when NSDLNomineeRegister_IsMinor='Y' then 'Yes' else 'No' end  as Minor,convert(varchar, NSDLNomineeRegister_DOBMinor,106)  as DOBofMinor,NSDLNomineeRegister_GuardianAddress as GuardianAddress,gcn.Country_ShortName as GuardianCountry,gst.State_ShortName as GuardianState,gct.City_ShortName as GuardianCity,NSDLNomineeRegister_GuardianPinCode as GuardianPinCode,case when NSDLNomineeRegister_NoNominationFlag='Y' then 'Yes' else 'No' end  as NoNominationFlag,NSDLNomineeRegister_PhoneResidence as ResidencePhone,NSDLNomineeRegister_PhoneMobile as Mobile,NSDLNomineeRegister_Email as Email,NSDLNomineeRegister_Remarks as Remarks,cast(NsdlClients_BenAccountID as varchar)+' ['+rtrim(ltrim(NsdlClients_BenFirstHolderName))+']' as BenAccountNo", null, "NSDLNomineeRegister_RegistrationDate desc");

            }
            else if (Convert.ToString(HttpContext.Current.Session["userlastsegment"]) == "10")
            {

                //dtNominees= oDBEngine.GetDataTable("Master_CDSLNomineeRegister nr left outer join master_cdslclients c on nr.CDSLNomineeRegister_BenID=c.CdslClients_BenAccountNumber", "CDSLNomineeRegister_ResgitrationNumber as RegistrationNo,convert(varchar,CDSLNomineeRegister_RegistrationDate,106) as RegistrationDate,CDSLNomineeRegister_Name as Name,CDSLNomineeRegister_Country as Country,CDSLNomineeRegister_Address as Address,CDSLNomineeRegister_State as  State,CDSLNomineeRegister_City as City,CDSLNomineeRegister_PinCode as PinCode,CDSLNomineeRegister_IsMinor as Minor,CDSLNomineeRegister_DOBMinor as DOBofMinor,CDSLNomineeRegister_GuardianAddress as GuardianAddress,CDSLNomineeRegister_GuardianCountry as GuardianCountry,CDSLNomineeRegister_GuardianState as GuardianState,CDSLNomineeRegister_GuardianCity as  GuardianCity,CDSLNomineeRegister_GuardianPinCode as GuardianPinCode,CDSLNomineeRegister_NoNominationFlag as NoNominationFlag,CDSLNomineeRegister_PhoneResidence as ResidencePhone,CDSLNomineeRegister_PhoneMobile as Mobile,CDSLNomineeRegister_Email as Email,CDSLNomineeRegister_Remarks as Remarks,cast(CdslClients_BenAccountNumber as varchar)+' ['+rtrim(ltrim(CdslClients_FirstHolderName))+']' as BenAccountNo", null, "CDSLNomineeRegister_RegistrationDate desc");
                dtNominees = oDBEngine.GetDataTable("Master_CDSLNomineeRegister nr left outer join master_cdslclients c on nr.CDSLNomineeRegister_BenID=c.CdslClients_BenAccountNumber left outer join  Master_Countries cn on nr.CDSLNomineeRegister_Country=cn.Country_ID left outer join Master_States st on nr.CDSLNomineeRegister_State=st.State_ID left outer join Master_cities ct on nr.CDSLNomineeRegister_City=ct.City_ID left outer join  Master_Countries gcn on nr.CDSLNomineeRegister_GuardianCountry=gcn.Country_ID left outer join Master_States gst on nr.CDSLNomineeRegister_GuardianState=gst.State_ID left outer join Master_cities gct on nr.CDSLNomineeRegister_GuardianCity=gct.City_ID", "CDSLNomineeRegister_ResgitrationNumber as RegistrationNo,convert(varchar,CDSLNomineeRegister_RegistrationDate,106) as RegistrationDate,CDSLNomineeRegister_Name as Name,cn.Country_ShortName as Country,CDSLNomineeRegister_Address as Address,st.State_ShortName as  State,ct.City_ShortName as City,CDSLNomineeRegister_PinCode as PinCode,case when CDSLNomineeRegister_IsMinor='Y' then 'Yes' else 'No' end as Minor,convert(varchar, CDSLNomineeRegister_DOBMinor,106)  as DOBofMinor,CDSLNomineeRegister_GuardianAddress as GuardianAddress,gcn.Country_ShortName as GuardianCountry,gst.State_ShortName as GuardianState,gct.City_ShortName as  GuardianCity,CDSLNomineeRegister_GuardianPinCode as GuardianPinCode,case when CDSLNomineeRegister_NoNominationFlag='Y' then 'Yes' else 'No' end as NoNominationFlag,CDSLNomineeRegister_PhoneResidence as ResidencePhone,CDSLNomineeRegister_PhoneMobile as Mobile,CDSLNomineeRegister_Email as Email,CDSLNomineeRegister_Remarks as Remarks,cast(CdslClients_BenAccountNumber as varchar)+' ['+rtrim(ltrim(CdslClients_FirstHolderName))+']' as BenAccountNo", null, "CDSLNomineeRegister_RegistrationDate desc");

            }



            DataTable dtReportHeader = new DataTable();
            dtReportHeader.Columns.Add(new DataColumn("Header", typeof(String))); //0
            DataRow HeaderRow = dtReportHeader.NewRow();
            //HeaderRow[0] = CompanyName.Rows[0][0].ToString() + "[" + HttpContext.Current.Session["usersegid"] + "]";
            HeaderRow[0] = "* * * Nominee Registration Details * * *";
            dtReportHeader.Rows.Add(HeaderRow);

            DataTable dtReportFooter = new DataTable();
            dtReportFooter.Columns.Add(new DataColumn("Footer", typeof(String))); //0
            DataRow FooterRow = dtReportFooter.NewRow();
            FooterRow[0] = "* * *  End Of Report * * *   ";
            dtReportFooter.Rows.Add(FooterRow);

            if (drpExport.SelectedItem.Value == "E")
            {
                objExcel.ExportToExcelforExcel(dtNominees, "Nominee Registrations", "Branch/Group Total", dtReportHeader, dtReportFooter);

            }
            //else if (drpExport.SelectedItem.Value == "P")
            //{
            //    objExcel.ExportToPDF(dtNominees, "Nominee Registration", "Branch/Group Total", dtReportHeader, dtReportFooter);
            //}
        }
        protected void grdNominees_ProcessColumnAutoFilter(object sender, ASPxGridViewAutoFilterEventArgs e)
        {
            if (e.Criteria != null)
            {
                string strCriteria = Convert.ToString(e.Criteria);

                string strCriteriaCol = "";
                strCriteriaCol = strCriteria.Split(']')[0];
                strCriteriaCol = strCriteriaCol.Substring(1);
                strCriteria = strCriteria.Split(']')[1];

                if (Convert.ToString(HttpContext.Current.Session["userlastsegment"]) == "9")
                {
                    if (strCriteriaCol == "RegNo")
                        strCriteriaCol = "NSDLNomineeRegister_ResgitrationNumber";
                    else if (strCriteriaCol == "Name")
                        strCriteriaCol = "NSDLNomineeRegister_Name";
                    else if (strCriteriaCol == "BenID")
                        strCriteriaCol = "NSDLNomineeRegister_BenID";
                    else if (strCriteriaCol == "BenAccName")
                        strCriteriaCol = "NsdlClients_BenFirstHolderName";
                    else if (strCriteriaCol == "RegDateShow")
                        strCriteriaCol = "convert(varchar,NSDLNomineeRegister_RegistrationDate,106)";


                    strCriteria = strCriteriaCol + strCriteria;

                    sqlDsNominees.SelectCommand = "select NSDLNomineeRegister_ID as NomineeRegisterID, NSDLNomineeRegister_ResgitrationNumber as RegNo,NSDLNomineeRegister_BenID as BenID,convert(varchar, NSDLNomineeRegister_RegistrationDate,101) as RegDate,convert(varchar, NSDLNomineeRegister_RegistrationDate,106) as RegDateShow,NSDLNomineeRegister_Name as Name,NSDLNomineeRegister_Country as country,NSDLNomineeRegister_Address as address,NSDLNomineeRegister_State as state,NSDLNomineeRegister_City as city,NSDLNomineeRegister_PinCode as pincode,NSDLNomineeRegister_IsMinor as minor,NSDLNomineeRegister_DOBMinor as dobminor,NSDLNomineeRegister_GuardianAddress as gaddress,NSDLNomineeRegister_GuardianCountry as gcountry,NSDLNomineeRegister_GuardianState as gstate,NSDLNomineeRegister_GuardianCity as gcity,NSDLNomineeRegister_GuardianPinCode as gpin,NSDLNomineeRegister_NoNominationFlag as nomination,NSDLNomineeRegister_PhoneResidence as resphone,NSDLNomineeRegister_PhoneMobile as mob,NSDLNomineeRegister_Email as email,NSDLNomineeRegister_Remarks as remarks,cast(NsdlClients_BenAccountID as varchar)+' ['+rtrim(ltrim(NsdlClients_BenFirstHolderName))+']' as BenAcc,NsdlClients_BenFirstHolderName as BenAccName from Master_NSDLNomineeRegister nr left outer join master_nsdlclients c on nr.NSDLNomineeRegister_BenID=c.NsdlClients_BenAccountID where " + strCriteria + " order by NSDLNomineeRegister_RegistrationDate desc";
                    grdNominees.DataBind();

                }
                else if (Convert.ToString(HttpContext.Current.Session["userlastsegment"]) == "10")
                {
                    if (strCriteriaCol == "RegNo")
                        strCriteriaCol = "CDSLNomineeRegister_ResgitrationNumber";
                    else if (strCriteriaCol == "Name")
                        strCriteriaCol = "CDSLNomineeRegister_Name";
                    else if (strCriteriaCol == "BenID")
                        strCriteriaCol = "CDSLNomineeRegister_BenID";
                    else if (strCriteriaCol == "BenAccName")
                        strCriteriaCol = "CdslClients_FirstHolderName";
                    else if (strCriteriaCol == "RegDateShow")
                        strCriteriaCol = "convert(varchar,CDSLNomineeRegister_RegistrationDate,106)";


                    strCriteria = strCriteriaCol + strCriteria;
                    sqlDsNominees.SelectCommand = "select CDSLNomineeRegister_ID as NomineeRegisterID,CDSLNomineeRegister_ResgitrationNumber as RegNo,CDSLNomineeRegister_BenID as BenID,convert(varchar,CDSLNomineeRegister_RegistrationDate,101) as RegDate,convert(varchar,CDSLNomineeRegister_RegistrationDate,106) as RegDateShow,CDSLNomineeRegister_Name as Name,CDSLNomineeRegister_Country as country,CDSLNomineeRegister_Address as address,CDSLNomineeRegister_State as  state,CDSLNomineeRegister_City as city,CDSLNomineeRegister_PinCode as pincode,CDSLNomineeRegister_IsMinor as minor,CDSLNomineeRegister_DOBMinor as dobminor,CDSLNomineeRegister_GuardianAddress as gaddress,CDSLNomineeRegister_GuardianCountry as gcountry,CDSLNomineeRegister_GuardianState as gstate,CDSLNomineeRegister_GuardianCity as  gcity,CDSLNomineeRegister_GuardianPinCode as gpin,CDSLNomineeRegister_NoNominationFlag as nomination,CDSLNomineeRegister_PhoneResidence as resphone,CDSLNomineeRegister_PhoneMobile as mob,CDSLNomineeRegister_Email as email,CDSLNomineeRegister_Remarks as remarks,cast(CdslClients_BenAccountNumber as varchar)+' ['+rtrim(ltrim(CdslClients_FirstHolderName))+']' as BenAcc,CdslClients_FirstHolderName as BenAccName from Master_CDSLNomineeRegister nr left outer join master_cdslclients c on nr.CDSLNomineeRegister_BenID=c.CdslClients_BenAccountNumber where " + strCriteria + " order by CDSLNomineeRegister_RegistrationDate desc";
                    grdNominees.DataBind();

                }

            }
            e.Criteria = null;
        }
    }
}