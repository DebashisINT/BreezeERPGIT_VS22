using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using DevExpress.Web;
using BusinessLogicLayer;
using ClsDropDownlistNameSpace;

namespace ERP.OMS.Management.Master
{
    public partial class management_master_OutsourcingComp_general : ERP.OMS.ViewState_class.VSPage
    {
        Int32 ID;
        //Converter objConverter = new Converter();
        BusinessLogicLayer.Converter objConverter = new BusinessLogicLayer.Converter();
        BusinessLogicLayer.OutSourcing_Agent_Company OOutSourcingGeneralBL = new BusinessLogicLayer.OutSourcing_Agent_Company();
        clsDropDownList oclsDropDownList = new clsDropDownList();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //DateOfIncorporation.EditFormatString = objConverter.GetDateFormat();
                //DBEngine oDBEngine = new DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
                //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULti
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                txtReferedBy.Attributes.Add("onkeyup", "CallList(this,'referedby',event)");
                DDLBind();
                Session["ContactType"] = "Outsourcing Agents/Companies";
                if (Request.QueryString["id"] != "ADD")
                {
                    if (Request.QueryString["id"] != null)
                    {
                        ID = Int32.Parse(Request.QueryString["id"]);
                        HttpContext.Current.Session["KeyVal"] = ID;
                    }
                    string[,] InternalId;

                    if (ID != 0)
                    {
                        InternalId = oDBEngine.GetFieldValue("tbl_master_contact", "cnt_internalId", "cnt_id=" + ID, 1);
                    }
                    else
                    {
                        InternalId = oDBEngine.GetFieldValue("tbl_master_contact", "cnt_internalId", "cnt_id=" + HttpContext.Current.Session["KeyVal"], 1);
                    }
                    HttpContext.Current.Session["KeyVal_InternalID"] = InternalId[0, 0];

                    string[,] ContactData;
                    if (ID != 0)
                    {
                        ContactData = oDBEngine.GetFieldValue("tbl_master_contact",
                                                "cnt_ucc, cnt_salutation,  cnt_firstName, cnt_middleName, cnt_lastName, cnt_shortName, cnt_branchId, cnt_sex, cnt_maritalStatus, case cnt_DOB when '1/1/1900 12:00:00 AM' then null else cnt_DOB end as cnt_DOB,case cnt_anniversaryDate when '1/1/1900 12:00:00 AM' then null else cnt_anniversaryDate end as cnt_anniversaryDate, cnt_legalStatus, cnt_education, cnt_profession, cnt_organization, cnt_jobResponsibility, cnt_designation, cnt_industry, cnt_contactSource, cnt_referedBy, cnt_contactType, cnt_contactStatus",
                                                " cnt_id=" + ID, 22);
                    }
                    else
                    {
                        ContactData = oDBEngine.GetFieldValue("tbl_master_contact",
                                                "cnt_ucc, cnt_salutation,  cnt_firstName, cnt_middleName, cnt_lastName, cnt_shortName, cnt_branchId, cnt_sex, cnt_maritalStatus, case cnt_DOB when '1/1/1900 12:00:00 AM' then null else cnt_DOB end as cnt_DOB,case cnt_anniversaryDate when '1/1/1900 12:00:00 AM' then null else cnt_anniversaryDate end as cnt_anniversaryDate, cnt_legalStatus, cnt_education, cnt_profession, cnt_organization, cnt_jobResponsibility, cnt_designation, cnt_industry, cnt_contactSource, cnt_referedBy, cnt_contactType, cnt_contactStatus",
                                                " cnt_id=" + HttpContext.Current.Session["KeyVal"], 22);
                    }

                    //____________ Value Allocation _______________//
                    DisabledTabPage(true);
                    ValueAllocation(ContactData);
                }
                else
                {
                    CmbSalutation.SelectedValue = "24";

                    txtFirstName.Text = "";

                    txtCode.Text = "";

                    cmbBranch.SelectedIndex.Equals(0);

                    DateOfIncorporation.Value = "";

                    cmbLegalStatus.SelectedValue = "3";


                    cmbSource.SelectedIndex.Equals(0);
                    cmbContactStatus.SelectedIndex.Equals(0);
                    //----Making TABs Disable------//
                    DisabledTabPage(false);

                    //-----End---------------------//
                    HttpContext.Current.Session["KeyVal"] = "0";
                    HttpContext.Current.Session["KeyVal_InternalID"] = string.Empty;
                }
                string[,] EmployeeNameID = oDBEngine.GetFieldValue(" tbl_master_contact ", " case when cnt_firstName is null then '' else cnt_firstName end + ' '+case when cnt_middleName is null then '' else cnt_middleName end+ ' '+case when cnt_lastName is null then '' else cnt_lastName end+' ['+cnt_shortName+']' as name ", " cnt_internalId='" + HttpContext.Current.Session["KeyVal_InternalID"] + "'", 1);
                if (EmployeeNameID[0, 0] != "n")
                {
                    lblHeader.Text = EmployeeNameID[0, 0].ToUpper();
                }
            }
        }
        public void DDLBind()
        {
            //DBEngine oDBEngine = new DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
            //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
            string[,] Data = oDBEngine.GetFieldValue("tbl_master_salutation", "sal_id, sal_name", null, 2, "sal_name");
            //oDBEngine.AddDataToDropDownList(Data, CmbSalutation);
            oclsDropDownList.AddDataToDropDownList(Data, CmbSalutation);
            Data = oDBEngine.GetFieldValue("tbl_master_branch", "branch_id, branch_description ", null, 2, "branch_description");
            //oDBEngine.AddDataToDropDownList(Data, cmbBranch);
            oclsDropDownList.AddDataToDropDownList(Data, cmbBranch);
            Data = oDBEngine.GetFieldValue(" tbl_master_ContactSource", "cntsrc_id, cntsrc_sourcetype", null, 2, "cntsrc_sourcetype");
            //oDBEngine.AddDataToDropDownList(Data, cmbSource);
            oclsDropDownList.AddDataToDropDownList(Data, cmbSource);
            Data = oDBEngine.GetFieldValue(" tbl_master_contactstatus", "cntstu_id, cntstu_contactStatus", null, 2, "cntstu_contactStatus");
            //oDBEngine.AddDataToDropDownList(Data, cmbContactStatus);
            oclsDropDownList.AddDataToDropDownList(Data, cmbContactStatus);
            Data = oDBEngine.GetFieldValue("tbl_master_legalstatus", "lgl_id, lgl_legalStatus", null, 2, "lgl_legalStatus");
            //oDBEngine.AddDataToDropDownList(Data, cmbLegalStatus);
            oclsDropDownList.AddDataToDropDownList(Data, cmbLegalStatus);
        }
        public void DisabledTabPage(bool VProp)
        {
            TabPage page = ASPxPageControl1.TabPages.FindByName("CorresPondence");
            page.Visible = VProp;
            page = ASPxPageControl1.TabPages.FindByName("ContactPreson");
            page.Visible = VProp;
            page = ASPxPageControl1.TabPages.FindByName("BankDetails");
            page.Visible = VProp;
            page = ASPxPageControl1.TabPages.FindByName("DPDetails");
            page.Visible = VProp;
            page = ASPxPageControl1.TabPages.FindByName("Documents");
            page.Visible = VProp;
            page = ASPxPageControl1.TabPages.FindByName("GroupMember");
            page.Visible = VProp;
        }
        public void ValueAllocation(string[,] ContactData)
        {
            try
            {

                if (ContactData[0, 1] != "")
                {
                    CmbSalutation.SelectedValue = ContactData[0, 1];
                }
                else
                {
                    CmbSalutation.SelectedIndex.Equals(0);
                }

                txtFirstName.Text = ContactData[0, 2];

                txtCode.Text = ContactData[0, 5];
                if (ContactData[0, 6] != "")
                {
                    cmbBranch.SelectedValue = ContactData[0, 6];
                }
                else
                {
                    cmbBranch.SelectedIndex.Equals(0);
                }

                //DateOfIncorporation.Value = Convert.ToDateTime(objConverter.DateConverter_d_m_y(ContactData[0, 9], "dd/mm/yyyy"));
                if (ContactData[0, 9] != "")
                {
                    DateOfIncorporation.Value = Convert.ToDateTime(ContactData[0, 9]);
                }
                if (ContactData[0, 11] != "")
                {
                    cmbLegalStatus.SelectedValue = ContactData[0, 11];
                }
                else
                {
                    cmbLegalStatus.SelectedIndex.Equals(0);
                }

                if (ContactData[0, 18] != "")
                {
                    cmbSource.SelectedValue = ContactData[0, 18];
                }
                else
                {
                    cmbSource.SelectedIndex.Equals(0);
                }
                txtReferedBy.Text = ContactData[0, 19];
                if (ContactData[0, 20] != "")
                {
                    cmbContactStatus.SelectedValue = ContactData[0, 20];
                }
                else
                {
                    cmbContactStatus.SelectedIndex.Equals(0);
                }
            }
            catch
            {
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            //DBEngine oDBEngine = new DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);
            //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);MULTI
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
            if (HttpContext.Current.Session["KeyVal"].ToString() != "0")        //________For Update
            {
                string today = oDBEngine.GetDate().ToString();
                String value = "cnt_salutation=" + CmbSalutation.SelectedItem.Value + ",  cnt_firstName='" + txtFirstName.Text + "', cnt_shortName='" + txtCode.Text + "', cnt_branchId=" + cmbBranch.SelectedItem.Value + ",  cnt_DOB='" + Convert.ToString(DateOfIncorporation.Value) + "',  cnt_legalStatus=" + cmbLegalStatus.SelectedItem.Value + ",  cnt_contactSource=" + cmbSource.SelectedItem.Value + ", cnt_referedBy='" + txtReferedBy.Text + "', cnt_contactType='OC', cnt_contactStatus=" + cmbContactStatus.SelectedItem.Value + ", lastModifyDate ='" + today + "', lastModifyUser=" + HttpContext.Current.Session["userid"]; // + Session["userid"] 

                //oDBEngine.RunStoredProcedure(EmployeeUpdate, value);
                Int32 rowsEffected = oDBEngine.SetFieldValue("tbl_master_contact", value, " cnt_id=" + HttpContext.Current.Session["KeyVal"]);
            }
            else               //_________For Insurt
            {
                try
                {
                    /* For Tier Structure

                    String con = ConfigurationSettings.AppSettings["DBConnectionDefault"];
                    using (SqlConnection lcon = new SqlConnection(con))
                    {
                        lcon.Open();
                        using (SqlCommand lcmdEmplInsert = new SqlCommand("ContactInsert", lcon))
                        {
                            lcmdEmplInsert.CommandType = CommandType.StoredProcedure;
                            //___________For Returning InternalID_________//
                            SqlParameter parameter = new SqlParameter("@result", SqlDbType.VarChar, 50);
                            parameter.Direction = ParameterDirection.Output;
                            ///_______________________________________________//
                       

                        

                            lcmdEmplInsert.Parameters.AddWithValue("@contacttype", "Outsourcing Agents/Companies");
                            lcmdEmplInsert.Parameters.AddWithValue("@cnt_ucc", "");
                            lcmdEmplInsert.Parameters.AddWithValue("@cnt_salutation", CmbSalutation.SelectedItem.Value);
                            lcmdEmplInsert.Parameters.AddWithValue("@cnt_firstName", txtFirstName.Text);
                            lcmdEmplInsert.Parameters.AddWithValue("@cnt_middleName", "");
                            lcmdEmplInsert.Parameters.AddWithValue("@cnt_lastName", "");
                            lcmdEmplInsert.Parameters.AddWithValue("@cnt_shortName", txtCode.Text);
                            lcmdEmplInsert.Parameters.AddWithValue("@cnt_branchId", cmbBranch.SelectedItem.Value);
                            lcmdEmplInsert.Parameters.AddWithValue("@cnt_sex", "");
                            lcmdEmplInsert.Parameters.AddWithValue("@cnt_maritalStatus", "");
                            lcmdEmplInsert.Parameters.Add(parameter);
                            if (DateOfIncorporation.Value != null)
                            {
                                lcmdEmplInsert.Parameters.AddWithValue("@cnt_DOB", DateOfIncorporation.Value.ToString());
                            }
                            else
                            {
                                lcmdEmplInsert.Parameters.AddWithValue("@cnt_DOB", "");
                            }

                            lcmdEmplInsert.Parameters.AddWithValue("@cnt_anniversaryDate", "");

                        
                       

                            lcmdEmplInsert.Parameters.AddWithValue("@cnt_legalStatus", cmbLegalStatus.SelectedItem.Value);
                            lcmdEmplInsert.Parameters.AddWithValue("@cnt_education", "");
                            lcmdEmplInsert.Parameters.AddWithValue("@cnt_profession", "");
                            lcmdEmplInsert.Parameters.AddWithValue("@cnt_organization", "");
                            lcmdEmplInsert.Parameters.AddWithValue("@cnt_jobResponsibility", "");
                            lcmdEmplInsert.Parameters.AddWithValue("@cnt_designation", "");
                            lcmdEmplInsert.Parameters.AddWithValue("@cnt_industry", "");
                            lcmdEmplInsert.Parameters.AddWithValue("@RPartner", "");
                            lcmdEmplInsert.Parameters.AddWithValue("@cnt_RegistrationDate", "");
                            lcmdEmplInsert.Parameters.AddWithValue("@cnt_rating", "");
                            lcmdEmplInsert.Parameters.AddWithValue("@cnt_reason", "");
                            lcmdEmplInsert.Parameters.AddWithValue("@cnt_contactSource", cmbSource.SelectedItem.Value);
                            lcmdEmplInsert.Parameters.AddWithValue("@cnt_referedBy", txtReferedBy.Text);
                            lcmdEmplInsert.Parameters.AddWithValue("@cnt_contactType", "OC");
                            lcmdEmplInsert.Parameters.AddWithValue("@cnt_contactStatus", cmbContactStatus.SelectedItem.Value);
                            lcmdEmplInsert.Parameters.AddWithValue("@lastModifyUser", HttpContext.Current.Session["userid"]);
                            lcmdEmplInsert.Parameters.AddWithValue("@bloodgroup", "");

                            lcmdEmplInsert.Parameters.AddWithValue("@WebLogIn", "No");
                            lcmdEmplInsert.Parameters.AddWithValue("@PassWord", "");
                            lcmdEmplInsert.ExecuteNonQuery();

                            string InternalID = parameter.Value.ToString();
                            lcon.Close();


                            */




                    DateTime dtDob, dtanniversary, dtReg, dtBusiness;

                    //string dd = Session["requesttype"].ToString();

                    if (DateOfIncorporation.Value != null)
                    {

                        dtDob = Convert.ToDateTime(DateOfIncorporation.Value);
                    }
                    else
                    {
                        dtDob = Convert.ToDateTime("01-01-1900");
                    }


                    dtanniversary = Convert.ToDateTime("01-01-1900");
                    dtReg = Convert.ToDateTime("01-01-1900");





                    string InternalID = OOutSourcingGeneralBL.Insert_ContactGeneral("Outsourcing Agents/Companies", "", CmbSalutation.SelectedItem.Value.ToString(), txtFirstName.Text.Trim(),
               "", "", txtCode.Text, cmbBranch.SelectedItem.Value.ToString(), "", "", dtDob, dtanniversary, cmbLegalStatus.SelectedItem.Value.ToString(),
               "", "", "", "", "", "", "", dtReg, "", "", cmbSource.SelectedItem.Value.ToString(), txtReferedBy.Text.Trim(), "OC",
               cmbContactStatus.SelectedItem.Value.ToString(), HttpContext.Current.Session["userid"].ToString(), "",
               "No", "");



                    string[,] cnt_id = oDBEngine.GetFieldValue(" tbl_master_contact", " cnt_id", " cnt_internalId='" + InternalID + "'", 1);
                    if (cnt_id[0, 0].ToString() != "n")
                    {
                        Response.Redirect("OutsourcingComp_general.aspx?id=" + cnt_id[0, 0].ToString(), false);
                    }

                    //    }
                    //}

                }
                catch
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "Script", "<script>alert('Code Already Exists !')</script>");
                }
            }


        }

    }
}