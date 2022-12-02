using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
//using DevExpress.Web.ASPxTabControl;
using DevExpress.Web;
using ClsDropDownlistNameSpace;
using System.Web.Services;
using BusinessLogicLayer;

namespace ERP.OMS.Management.Master
{
    public partial class management_master_ProfBodies_general : ERP.OMS.ViewState_class.VSPage
    {
        //DBEngine oDBEngine = new DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        //Converter oconverter = new Converter();
        clsDropDownList oclsDropDownList = new clsDropDownList();
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]); MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        BusinessLogicLayer.Converter oconverter = new BusinessLogicLayer.Converter();

        protected void Page_Load(object sender, EventArgs e)
        {
            // Code  Added  By Priti on 0712016 to use Convert.ToString instead of ToString()
            if (!IsPostBack)
            {
                txtRegndate.EditFormatString = oconverter.GetDateFormat();
                txtRegndate.EditFormatString = "dd-MM-yyyy";
                Session["ContactType"] = "Professional/Technical Bodies";
                string[,] Data = oDBEngine.GetFieldValue("tbl_master_country", "cou_id, cou_country", null, 2, "cou_country");
                //oDBEngine.AddDataToDropDownList(Data, drpcountry);
                oclsDropDownList.AddDataToDropDownList(Data, drpcountry);

                if (Request.QueryString["id"] != "ADD")
                {

                    if (Request.QueryString["id"] != null)
                    {
                        string ID = Request.QueryString["id"];
                        HttpContext.Current.Session["KeyVal"] = ID;
                        HttpContext.Current.Session["KeyVal_InternalID"] = Convert.ToString(Request.QueryString["id"]);
                    }


                    string[,] ContactData = oDBEngine.GetFieldValue("tbl_master_profTechBodies",
                                                "prof_name, prof_shortname,  prof_descrip, prof_regnNumber, prof_regnDate, prof_regsAname, prof_countryid",
                        //" prof_id='" + HttpContext.Current.Session["KeyVal_InternalID"].ToString() + "'", 7);
                                                " prof_id='" + Convert.ToString(HttpContext.Current.Session["KeyVal_InternalID"]) + "'", 7);

                    //____________ Value Allocation _______________//
                    DisabledTabPage(true);
                    ValueAllocation(ContactData);
                }
                else
                {
                    txtAuthorityName.Text = "";
                    txtDescription.Text = "";
                    txtName.Text = "";
                    txtRegnNumber.Text = "";
                    txtShortName.Text = "";
                    //----Making TABs Disable------//
                    DisabledTabPage(false);

                    //-----End---------------------//
                    HttpContext.Current.Session["KeyVal"] = "0";
                }
            }
        }
        public void ValueAllocation(string[,] ContactData)
        {
            txtName.Text = ContactData[0, 0];
            txtShortName.Text = ContactData[0, 1];
            txtDescription.Text = ContactData[0, 2];
            txtRegnNumber.Text = ContactData[0, 3];
            txtRegndate.Value = Convert.ToDateTime(ContactData[0, 4]);
            txtAuthorityName.Text = ContactData[0, 5];
            if (ContactData[0, 6] != "")
            {
                drpcountry.SelectedValue = ContactData[0, 6];
            }
        }
        public void DisabledTabPage(bool visibleProp)
        {
            TabPage page = ASPxPageControl1.TabPages.FindByName("CorresPondence");
            page.Visible = visibleProp;
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            // Code  Added  By Priti on 0712016 to use Convert.ToString instead of ToString()
            if (Convert.ToString(HttpContext.Current.Session["KeyVal"]) != "0")        //________For Update
            {
                string today = Convert.ToString(oDBEngine.GetDate());
                // string today = oDBEngine.GetDate().ToString();
                String value = "prof_name='" + txtName.Text + "',  prof_shortname='" + txtShortName.Text + "', prof_descrip='" + txtDescription.Text + "', prof_regnNumber='" + txtRegnNumber.Text + "',  prof_regnDate='" + txtRegndate.Value + "',  prof_regsAname='" + txtAuthorityName.Text + "',  prof_countryid=" + drpcountry.SelectedValue + ", lastModifyDate ='" + Convert.ToString(oDBEngine.GetDate()) + "', lastModifyUser=" + HttpContext.Current.Session["userid"]; // + Session["userid"] 
                Int32 rowsEffected = oDBEngine.SetFieldValue("tbl_master_profTechBodies", value, " prof_id='" + HttpContext.Current.Session["KeyVal"] + "'");
            }
            else
            {
                string InternalID = "";
                //String con = ConfigurationSettings.AppSettings["DBConnectionDefault"]; MULTI
                String con = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                using (SqlConnection lcon = new SqlConnection(con))
                {
                    lcon.Open();
                    using (SqlCommand lcmdEmplInsert = new SqlCommand("ProfTechInsert", lcon))
                    {
                        //..................code added by priti on 07122016 to check the short name unique..
                        DataTable dtseg = oDBEngine.GetDataTable("select * from tbl_master_profTechBodies where prof_shortname='" + txtShortName.Text.Trim() + "'");
                        if (dtseg.Rows.Count == 0)
                        {
                            //...........end............
                            lcmdEmplInsert.CommandType = CommandType.StoredProcedure;
                            //___________For Returning InternalID_________//
                            SqlParameter parameter = new SqlParameter("@result", SqlDbType.VarChar, 50);
                            parameter.Direction = ParameterDirection.Output;
                            ///_______________________________________________//
                            lcmdEmplInsert.Parameters.AddWithValue("@prof_name", txtName.Text);
                            lcmdEmplInsert.Parameters.AddWithValue("@prof_shortname", txtShortName.Text);
                            lcmdEmplInsert.Parameters.AddWithValue("@prof_descrip", txtDescription.Text);
                            lcmdEmplInsert.Parameters.AddWithValue("@prof_regnNumber", txtRegnNumber.Text);
                            lcmdEmplInsert.Parameters.Add(parameter);
                            if (txtRegndate.Value != null)
                            {
                                lcmdEmplInsert.Parameters.AddWithValue("@prof_regnDate", txtRegndate.Value);
                            }
                            else
                            {
                                lcmdEmplInsert.Parameters.AddWithValue("@prof_regnDate", "");
                            }

                            lcmdEmplInsert.Parameters.AddWithValue("prof_regsAname", txtAuthorityName.Text);

                            lcmdEmplInsert.Parameters.AddWithValue("@prof_countryid", drpcountry.SelectedItem.Value);
                            lcmdEmplInsert.Parameters.AddWithValue("@CreateUser", Convert.ToString(HttpContext.Current.Session["userid"]));
                            lcmdEmplInsert.ExecuteNonQuery();
                            InternalID = Convert.ToString(parameter.Value);
                           
                            //Purpose : Replace .ToString() with Convert.ToString(..)
                            //Name : Sudip 
                            // Dated : 21-12-2016


                            //..................code added by priti on 07122016 to check the short name unique..    
                        }
                        else
                        {
                            Page.ClientScript.RegisterStartupScript(GetType(), "pagecall", "<script>jAlert('Short Name already exists');</script>");
                            return;
                        }
                        //...........end............

                    }
                }
                Response.Redirect("ProfBodies_general.aspx?id=" + InternalID, false);
            }
        }
        [WebMethod]
        public static bool CheckUniqueName(string ShortName)
        {
            string ShortCode = "0";
            if (HttpContext.Current.Session["KeyVal"] != "") ShortCode = Convert.ToString(HttpContext.Current.Session["KeyVal"]).Trim();

            MShortNameCheckingBL objMShortNameCheckingBL = new MShortNameCheckingBL();
            DataTable dt = new DataTable();
            Boolean status = false;
            BusinessLogicLayer.GenericMethod oGeneric = new BusinessLogicLayer.GenericMethod();
            if (ShortCode != "" && Convert.ToString(ShortName).Trim() != "")
            {
                status = objMShortNameCheckingBL.CheckUnique(ShortName, ShortCode, "MasterTechnicalBodies");
            }
            return status;
        }
    }
}