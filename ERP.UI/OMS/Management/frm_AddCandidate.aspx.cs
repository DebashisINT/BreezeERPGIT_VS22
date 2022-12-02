using System;
using System.Configuration;
using System.Web;
using System.Web.UI;
using BusinessLogicLayer;
using ClsDropDownlistNameSpace;

namespace ERP.OMS.Management
{
    public partial class management_frm_AddCandidate : System.Web.UI.Page
    {
        int ID;
        int ID1;
        BusinessLogicLayer.Converter OConvert = new BusinessLogicLayer.Converter();
        public string pageAccess = "";
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);

        clsDropDownList ObjDropDown = new clsDropDownList();

        protected void Page_Load(object sender, EventArgs e)
        {

            if (HttpContext.Current.Session["userid"] == null)
            {
               //Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }

            if (!IsPostBack)
            {
                ID = int.Parse(Request.QueryString["id"].ToString());
                string mode = Request.QueryString["mode"].ToString();
                if (mode == "edit")
                {
                    Session["mode"] = mode;
                    EditCandidate();
                }
                if (mode == "new")
                {
                    Session["mode"] = mode;
                    NewCandidate();
                }

            }
            drpSourceType.Attributes.Add("onchange", "ondropdown();");
            txtReferedBy.Attributes.Add("onkeyup", "call_ajax(this,'referedby',event)");
            ID = int.Parse(Request.QueryString["id"].ToString());
            if (Request.QueryString["id1"] != null)
            {
                ID1 = int.Parse(Request.QueryString["id1"].ToString());
            }
            //____________Calling Calendar on click and onfocus_______________//
            DateTime dt = oDBEngine.GetDate();

            txtDOB.EditFormatString = OConvert.GetDateFormat("Date");
            txtPJD.EditFormatString = OConvert.GetDateFormat("Date");
            txtPJD.Attributes.Add("readonly", "true");
            txtDOB.Attributes.Add("readonly", "true");
        }

        private void NewCandidate()
        {
            //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);MULTI
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

            txtName.Text = "";
            txtLocality.Text = "";
            txtReferedBy.Text = "";
            txtNoofDependent.Text = "0";
            txtPhone.Text = "";
            txtEmailId.Text = "";
            txtCertification.Text = "";
            txtCurrentEmployer.Text = "";
            txtExpYrs.Text = "0";
            txtCurrentCTC.Text = "0";
            txtDesiredCTC.Text = "0";
            txtPreviousCTC.Text = "0";
            txtPreviousEmployer.Text = "";
            txtReasonforChange.Text = "";
            //drpSourceType.SelectedIndex = 1;
            string[,] Data = oDBEngine.GetFieldValue("tbl_master_contactSource ", "cntsrc_id,cntsrc_sourceType", null, 2, "cntsrc_sourceType");
            ObjDropDown.AddDataToDropDownList(Data, drpSourceType, 0);
            Data = oDBEngine.GetFieldValue("tbl_master_maritalstatus ", "mts_id,mts_maritalStatus", null, 2);
            ObjDropDown.AddDataToDropDownList(Data, drpMaritalStatus, 0);
            Data = oDBEngine.GetFieldValue("tbl_master_education ", "edu_id,edu_education", null, 2, "edu_education");
            ObjDropDown.AddDataToDropDownList(Data, drpQualification, 0);
            Data = oDBEngine.GetFieldValue("tbl_master_education ", "edu_id,edu_education", null, 2, "edu_education");
            ObjDropDown.AddDataToDropDownList(Data, drpProfQualification, 0);
            Data = oDBEngine.GetFieldValue("tbl_master_jobResponsibility ", "job_id,job_responsibility", null, 2, "job_responsibility");
            ObjDropDown.AddDataToDropDownList(Data, drpCurrentJobProfile, 0);
            Data = oDBEngine.GetFieldValue("tbl_master_jobResponsibility ", "job_id,job_responsibility", null, 2, "job_responsibility");
            ObjDropDown.AddDataToDropDownList(Data, drpPreviousJobProfile, 0);
            txtPJD.Text = "01-01-0100";

        }
        private void EditCandidate()
        {
            int IdCopy = ID;
            //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);MULTI
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
            if (Request.QueryString["id1"] != null)
            {
                string[,] DT = oDBEngine.GetFieldValue("tbl_trans_ReferalAgentDetail", "rdd_CandidateName as Name, rdd_Location as Locality, isnull(rdd_SourceName,'') as SourceName,rdd_PhoneNo as Phoneno,  rdd_Emailid as Email, rdd_Certification as Certification,rdd_WorkofExp as WorkExperience, rdd_ReasonforRejection as ReasonForChange, rdd_SourceType  as SourceType, isnull(rdd_MaritalStatus,12) as rde_MaritalStatus,rdd_EducationQualification as Qualification, rdd_ProfessinalQualification as ProfQualification, rdd_AggrementDate as ProbableJoinDate,rdd_Dob as Dob, rdd_Sex as Sex ", "rdd_Id=" + IdCopy, 15);
                if (DT[0, 0] != "n")
                {
                    txtName.Text = DT[0, 0];
                    txtLocality.Text = DT[0, 1];
                    if (DT[0, 2] != "")
                    {
                        string[,] SName = oDBEngine.GetFieldValue("tbl_master_contact", "ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '')", " cnt_internalId='" + DT[0, 2] + "'", 1);
                        if (SName[0, 0] != "n")
                        {
                            txtReferedBy.Text = SName[0, 0];
                        }
                    }
                    txtReferedBy_hidden.Text = DT[0, 2];
                    txtPhone.Text = DT[0, 3];
                    txtEmailId.Text = DT[0, 4];
                    txtCertification.Text = DT[0, 5];
                    txtExpYrs.Text = DT[0, 6];
                    txtReasonforChange.Text = DT[0, 7];
                    //drpSourceType.SelectedIndex = 1;
                    string[,] Data = oDBEngine.GetFieldValue("tbl_master_contactSource ", "cntsrc_id,cntsrc_sourceType", null, 2, "cntsrc_sourceType");
                    ObjDropDown.AddDataToDropDownList(Data, drpSourceType, int.Parse(DT[0, 8].ToString()));
                    Data = oDBEngine.GetFieldValue("tbl_master_maritalstatus ", "mts_id,mts_maritalStatus", null, 2, "mts_maritalStatus");
                    ObjDropDown.AddDataToDropDownList(Data, drpMaritalStatus, int.Parse(DT[0, 9].ToString()));
                    Data = oDBEngine.GetFieldValue("tbl_master_education ", "edu_id,edu_education", null, 2, "edu_education");
                    ObjDropDown.AddDataToDropDownList(Data, drpQualification, int.Parse(DT[0, 10].ToString()));
                    Data = oDBEngine.GetFieldValue("tbl_master_education ", "edu_id,edu_education", null, 2, "edu_education");
                    ObjDropDown.AddDataToDropDownList(Data, drpProfQualification, int.Parse(DT[0, 11].ToString()));
                    Data = oDBEngine.GetFieldValue("tbl_master_jobResponsibility ", "job_id,job_responsibility", null, 2, "job_responsibility");
                    ObjDropDown.AddDataToDropDownList(Data, drpCurrentJobProfile);
                    Data = oDBEngine.GetFieldValue("tbl_master_jobResponsibility ", "job_id,job_responsibility", null, 2, "job_responsibility");
                    ObjDropDown.AddDataToDropDownList(Data, drpPreviousJobProfile);
                    //drpPJD.Value = DT[0, 20].ToString();
                    BusinessLogicLayer.Converter oConverter = new BusinessLogicLayer.Converter();
                    txtPJD.Value = Convert.ToDateTime(DT[0, 12].ToString());
                    txtDOB.Value = Convert.ToDateTime(DT[0, 13].ToString());
                    //drpDOB.Value = DT[0, 21].ToString();
                    drpSex.SelectedIndex = int.Parse(DT[0, 14].ToString());

                }
            }
            else
            {
                string[,] DT = oDBEngine.GetFieldValue("tbl_trans_RecruitmentDetail", "rde_Name as Name, rde_ResidenceLocation as Locality, isnull(rde_SourceName,'') as SourceName,rde_NoofDepedent as NoofDependent,rde_PhoneNo as Phoneno,  rde_Email as Email, rde_Certification as Certification, rde_CurrentEmployer as CurrentEmployer, rde_WorkExp as WorkExperience, rde_CurrentCTC as CurrentCTC, rde_CTC as DesiredCtc,rde_PreviousCTC as PreviousCTC,rde_PreviousEmployer as PreviousEmployer, rde_ReasonForChange as ReasonForChange, rde_SourceType  as SourceType, isnull(rde_MaritalStatus,12) as rde_MaritalStatus,rde_EduQualification as Qualification, rde_ProfQualification as ProfQualification, rde_CurrentJobProfile as CurrentJobProfile, rde_PreviousJobProfile as PreviousJobProfile, rde_ProbableJoinDate as ProbableJoinDate,rde_DOB as Dob, rde_CandidateSex as Sex ", "rde_Id=" + IdCopy, 23);
                if (DT[0, 0] != "n")
                {
                    txtName.Text = DT[0, 0];
                    txtLocality.Text = DT[0, 1];
                    if (DT[0, 2] != "")
                    {
                        string[,] SName = oDBEngine.GetFieldValue("tbl_master_contact", "ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '')+' [' + ISNULL(cnt_UCC, '')+']'", " cnt_internalId='" + DT[0, 2] + "'", 1);
                        if (SName[0, 0] != "n")
                        {
                            txtReferedBy.Text = SName[0, 0];
                        }
                    }
                    txtReferedBy_hidden.Text = DT[0, 2];
                    txtNoofDependent.Text = DT[0, 3];
                    txtPhone.Text = DT[0, 4];
                    txtEmailId.Text = DT[0, 5];
                    txtCertification.Text = DT[0, 6];
                    txtCurrentEmployer.Text = DT[0, 7];
                    txtExpYrs.Text = DT[0, 8];
                    txtCurrentCTC.Text = DT[0, 9];
                    txtDesiredCTC.Text = DT[0, 10];
                    txtPreviousCTC.Text = DT[0, 11];
                    txtPreviousEmployer.Text = DT[0, 12];
                    txtReasonforChange.Text = DT[0, 13];
                    //drpSourceType.SelectedIndex = 1;
                    string[,] Data = oDBEngine.GetFieldValue("tbl_master_contactSource ", "cntsrc_id,cntsrc_sourceType", null, 2, "cntsrc_sourceType");
                    ObjDropDown.AddDataToDropDownList(Data, drpSourceType, int.Parse(DT[0, 14].ToString()));
                    Data = oDBEngine.GetFieldValue("tbl_master_maritalstatus ", "mts_id,mts_maritalStatus", null, 2, "mts_maritalStatus");
                    ObjDropDown.AddDataToDropDownList(Data, drpMaritalStatus, int.Parse(DT[0, 15].ToString()));
                    Data = oDBEngine.GetFieldValue("tbl_master_education ", "edu_id,edu_education", null, 2, "edu_education");
                    ObjDropDown.AddDataToDropDownList(Data, drpQualification, int.Parse(DT[0, 16].ToString()));
                    Data = oDBEngine.GetFieldValue("tbl_master_education ", "edu_id,edu_education", null, 2, "edu_education");
                    ObjDropDown.AddDataToDropDownList(Data, drpProfQualification, int.Parse(DT[0, 17].ToString()));
                    Data = oDBEngine.GetFieldValue("tbl_master_jobResponsibility ", "job_id,job_responsibility", null, 2, "job_responsibility");
                    ObjDropDown.AddDataToDropDownList(Data, drpCurrentJobProfile, int.Parse(DT[0, 18].ToString()));
                    Data = oDBEngine.GetFieldValue("tbl_master_jobResponsibility ", "job_id,job_responsibility", null, 2, "job_responsibility");
                    ObjDropDown.AddDataToDropDownList(Data, drpPreviousJobProfile, int.Parse(DT[0, 19].ToString()));
                    //drpPJD.Value = DT[0, 20].ToString();
                    BusinessLogicLayer.Converter oConverter = new BusinessLogicLayer.Converter();
                    txtPJD.Value = Convert.ToDateTime(DT[0, 20].ToString());
                    txtDOB.Value = Convert.ToDateTime(DT[0, 21].ToString());
                    //drpDOB.Value = DT[0, 21].ToString();
                    drpSex.SelectedIndex = int.Parse(DT[0, 22].ToString());

                }
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            string Actid = "";
            if (Session["mode"].ToString() == "new")
            {
                //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);MULTI
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                string IntrnlID = "";
                string Fields = "";
                string values = "";
                if (ID.ToString() != "")
                {
                    values = ID + ",'" + txtName.Text + "'," + drpSourceType.SelectedItem.Value + ",";
                }
                else
                {
                    values = Session["KeyVal"] + ",'" + txtName.Text + "'," + drpSourceType.SelectedItem.Value + ",";
                }
                if (int.Parse(drpSourceType.SelectedItem.Value.ToString()) == 3 || int.Parse(drpSourceType.SelectedItem.Value.ToString()) == 4 || int.Parse(drpSourceType.SelectedItem.Value.ToString()) == 5 || int.Parse(drpSourceType.SelectedItem.Value.ToString()) == 8 || int.Parse(drpSourceType.SelectedItem.Value.ToString()) == 10 || int.Parse(drpSourceType.SelectedItem.Value.ToString()) == 14)
                {
                    //string[,] InternalId = oDBEngine.GetFieldValue("tbl_master_user", "user_contactId", " user_name='" + txtName.Text + "'", 1);
                    // string[] InternalId = txtReferedBy.Text.Split('!');
                    if (txtReferedBy_hidden.Text != " ")
                    {
                        IntrnlID = txtReferedBy_hidden.Text;

                    }
                    else
                    {
                        lblmessage.Text = "Either \"Source Type\" OR The \"Source Name\" is not correct!";
                        return;
                    }
                    values += "'" + IntrnlID + "',";
                    if (Request.QueryString["id1"] != null)
                    {
                        Fields = "rdd_ActivityId,rdd_CandidateName,rdd_SourceType,rdd_SourceName,rdd_Sex,rdd_Dob,rdd_EducationQualification,rdd_ProfessinalQualification,rdd_Certification,rdd_WorkofExp,rdd_ReasonforRejection,rdd_AggrementDate,rdd_Location,rdd_PhoneNo,rdd_Emailid,rdd_MaritalStatus,CreateDate,CreateUser";
                    }
                    else
                    {
                        Fields = "rde_Activityid, rde_Name, rde_SourceType, rde_SourceName, rde_CandidateSex, rde_DOB, rde_EduQualification, rde_ProfQualification,   rde_Certification, rde_WorkExp, rde_CTC, rde_ReasonForChange, rde_ProbableJoinDate, rde_ResidenceLocation, rde_CurrentEmployer,   rde_CurrentJobProfile, rde_CurrentCTC, rde_PreviousEmployer, rde_PreviousJobProfile, rde_PreviousCTC, rde_NoofDepedent, rde_PhoneNo,  rde_Email,rde_MaritalStatus";
                    }
                }
                else
                {
                    if (Request.QueryString["id1"] != null)
                    {
                        Fields = "rdd_ActivityId,rdd_CandidateName,rdd_SourceType,rdd_Sex,rdd_Dob,rdd_EducationQualification,rdd_ProfessinalQualification,rdd_Certification,rdd_WorkofExp,rdd_ReasonforRejection,rdd_AggrementDate,rdd_Location,rdd_PhoneNo,rdd_Emailid,rdd_MaritalStatus,CreateDate,CreateUser";
                    }
                    else
                    {
                        Fields = "rde_Activityid, rde_Name, rde_SourceType, rde_CandidateSex, rde_DOB, rde_EduQualification, rde_ProfQualification,   rde_Certification, rde_WorkExp, rde_CTC, rde_ReasonForChange, rde_ProbableJoinDate, rde_ResidenceLocation, rde_CurrentEmployer,   rde_CurrentJobProfile, rde_CurrentCTC, rde_PreviousEmployer, rde_PreviousJobProfile, rde_PreviousCTC, rde_NoofDepedent, rde_PhoneNo,  rde_Email, rde_MaritalStatus";
                    }
                }
                if (Request.QueryString["id1"] != null)
                {
                    values += drpSex.SelectedItem.Value + ",'" + txtDOB.Value.ToString() + "'," + drpQualification.SelectedItem.Value + "," + drpProfQualification.SelectedItem.Value + ",'" + txtCertification.Text + "','" + txtExpYrs.Text + "','" + txtReasonforChange.Text + "','" + txtPJD.Value.ToString() + "','" + txtLocality.Text + "','" + txtPhone.Text + "','" + txtEmailId.Text + "','" + drpMaritalStatus.SelectedItem.Value + "','" + oDBEngine.GetDate().ToString() + "','" + Session["userid"].ToString() + "'";
                }
                else
                {
                    values += drpSex.SelectedItem.Value + ",'" + txtDOB.Value.ToString() + "'," + drpQualification.SelectedItem.Value + "," + drpProfQualification.SelectedItem.Value + ",'" + txtCertification.Text + "','" + txtExpYrs.Text + "','" + txtDesiredCTC.Text + "','" + txtReasonforChange.Text + "','" + txtPJD.Value.ToString() + "','" + txtLocality.Text + "','" + txtCurrentEmployer.Text + "'," + drpCurrentJobProfile.SelectedItem.Value + ",'" + txtCurrentCTC.Text + "','" + txtPreviousEmployer.Text + "'," + drpPreviousJobProfile.SelectedItem.Value + ",'" + txtPreviousCTC.Text + "','" + txtNoofDependent.Text + "','" + txtPhone.Text + "','" + txtEmailId.Text + "'," + drpMaritalStatus.SelectedItem.Value;
                }
                int NoOfRowEffected = 0;
                if (Request.QueryString["id1"] != null)
                {
                    NoOfRowEffected = oDBEngine.InsurtFieldValue("tbl_trans_ReferalAgentDetail", Fields, values);
                }
                else
                {
                    NoOfRowEffected = oDBEngine.InsurtFieldValue("tbl_trans_RecruitmentDetail", Fields, values);
                }
                if (NoOfRowEffected != 0)
                {
                    if (ID.ToString() != "")
                    {
                        Actid = ID.ToString();
                    }
                    else
                    {
                        Actid = Session["KeyVal"].ToString();
                    }
                    if (Request.QueryString["id1"] != null)
                    {
                        string[,] DT = oDBEngine.GetFieldValue("tbl_trans_ReferalAgentDetail", "rdd_Id", " rdd_ActivityId =" + Actid, 1);
                        if (DT[0, 0] != "n")
                        {
                            oDBEngine.SetFieldValue("tbl_trans_Activies", "act_actualStartDate='" + oDBEngine.GetDate() + "'", " act_id=" + Actid);
                        }
                    }
                    else
                    {
                        string[,] DT = oDBEngine.GetFieldValue("tbl_trans_RecruitmentDetail", "rde_Id", " rde_Activityid =" + Actid, 1);
                        if (DT[0, 0] != "n")
                        {
                            oDBEngine.SetFieldValue("tbl_trans_Activies", "act_actualStartDate='" + oDBEngine.GetDate() + "'", " act_id=" + Actid);
                        }
                    }
                }
            }
            if (Session["mode"].ToString() == "edit")
            {
                //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);MULTI
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                string values = "";
                if (Request.QueryString["id1"] != null)
                {
                    values = "rdd_CandidateName='" + txtName.Text + "',rdd_SourceType=" + drpSourceType.SelectedItem.Value + ",";
                }
                else
                {
                    values = "rde_Name='" + txtName.Text + "',rde_SourceType=" + drpSourceType.SelectedItem.Value + ",";
                }
                if (int.Parse(drpSourceType.SelectedItem.Value.ToString()) == 3 || int.Parse(drpSourceType.SelectedItem.Value.ToString()) == 4 || int.Parse(drpSourceType.SelectedItem.Value.ToString()) == 5 || int.Parse(drpSourceType.SelectedItem.Value.ToString()) == 8 || int.Parse(drpSourceType.SelectedItem.Value.ToString()) == 10 || int.Parse(drpSourceType.SelectedItem.Value.ToString()) == 14)
                {
                    //string[,] InternalId = oDBEngine.GetFieldValue("tbl_master_user", "user_contactId", " user_name='" + txtName.Text + "'", 1);
                    //string[] InternalId = txtReferedBy.Text.Split('!');
                    if (txtReferedBy_hidden.Text != " ")
                    {
                        string IntrnlID = txtReferedBy_hidden.Text;
                        if (Request.QueryString["id1"] != null)
                        {
                            values += "rdd_SourceName='" + IntrnlID + "',";
                        }
                        else
                        {
                            values += "rde_SourceName='" + IntrnlID + "',";
                        }
                    }
                    else
                    {
                        lblmessage.Text = "Either \"Source Type\" OR The \"Source Name\" is not correct!";
                        return;
                    }
                }
                if (Request.QueryString["id1"] != null)
                {
                    values += "rdd_Sex=" + drpSex.SelectedItem.Value + ",rdd_Dob='" + txtDOB.Value.ToString() + "',rdd_EducationQualification=" + drpQualification.SelectedItem.Value + ",rdd_ProfessinalQualification=" + drpProfQualification.SelectedItem.Value + ",rdd_Certification='" + txtCertification.Text + "',";
                    values += "rdd_WorkofExp='" + txtExpYrs.Text + "',rdd_ReasonforRejection='" + txtReasonforChange.Text + "',rdd_AggrementDate='" + txtPJD.Value.ToString() + "',rdd_Location='" + txtLocality.Text + "',rdd_PhoneNo='" + txtPhone.Text + "',rdd_Emailid='" + txtEmailId.Text + "',rdd_MaritalStatus=" + drpMaritalStatus.SelectedItem.Value + ",";
                    values += "LastModifyDate='" + oDBEngine.GetDate().ToString() + "',LastModifyUser='" + Session["userid"].ToString() + "'";
                    int NoRowEffected = oDBEngine.SetFieldValue("tbl_trans_ReferalAgentDetail", values, " rdd_Id=" + ID);
                }
                else
                {
                    values += "rde_CandidateSex=" + drpSex.SelectedItem.Value + ",rde_DOB='" + txtDOB.Value.ToString() + "',rde_EduQualification=" + drpQualification.SelectedItem.Value + ",rde_ProfQualification=" + drpProfQualification.SelectedItem.Value + ",rde_Certification='" + txtCertification.Text + "',";
                    values += "rde_WorkExp='" + txtExpYrs.Text + "',rde_CTC='" + txtDesiredCTC.Text + "',rde_ReasonForChange='" + txtReasonforChange.Text + "',rde_ProbableJoinDate='" + txtPJD.Value.ToString() + "',rde_ResidenceLocation='" + txtLocality.Text + "',rde_CurrentEmployer='" + txtCurrentEmployer.Text + "',rde_CurrentJobProfile=" + drpCurrentJobProfile.SelectedItem.Value + ",rde_CurrentCTC='" + txtCurrentCTC.Text + "',rde_PreviousEmployer='" + txtPreviousEmployer.Text + "',rde_PreviousJobProfile=" + drpPreviousJobProfile.SelectedItem.Value + ",rde_PreviousCTC='" + txtPreviousCTC.Text + "',rde_NoofDepedent='" + txtNoofDependent.Text + "',rde_PhoneNo='" + txtPhone.Text + "',rde_Email='" + txtEmailId.Text + "',rde_MaritalStatus=" + drpMaritalStatus.SelectedItem.Value;

                    int NoRowEffected = oDBEngine.SetFieldValue("tbl_trans_RecruitmentDetail", values, " rde_Id=" + ID);
                }
            }
            //Response.Write("<script language='javaScript'> alert('Candidate Added Successfully!!') </script>");
            if (Request.QueryString["id1"] != null)
            {
                Response.Write("<script language='javaScript'>var hd=window.opener.document.getElementById('ctl00$ContentPlaceHolder3$Hd1');hd.value='" + ID1 + "';window.opener.CandidateRefresh(); window.close(); </script>");
            }
            else
            {
                Response.Redirect("frm_UserRecruitmentEmployee_Detail.aspx?id=" + Session["KeyVal"], false);
            }
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            //__Here Session["KeyVal"] is coming from previous page i.e. "frm_UserRecruitmentEmployee_Detail.aspx", as ID in this page._____//
            if (Request.QueryString["id1"] != null)
            {
                Response.Write("<script language='javaScript'> window.close(); </script>");
            }
            else
            {
                Response.Redirect("frm_UserRecruitmentEmployee_Detail.aspx?id=" + Session["KeyVal"], false);
            }
        }
    }
}
