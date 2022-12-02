using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using CrystalDecisions.CrystalReports.Engine;
using BusinessLogicLayer;

namespace ERP.OMS.Management.ToolsUtilities
{
    public partial class management_utilities_GenerateOfferLater : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
    {
        static string data = string.Empty;
        public string pageAccess = "";
        //DBEngine oDBEngine = new DBEngine(string.Empty);
        //Converter OConvert = new Converter();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        Utilities oUtilities = new Utilities();
        BusinessLogicLayer.Converter OConvert = new BusinessLogicLayer.Converter();
        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //'http://localhost:2957/InfluxCRM/management/testProjectMainPage_employee.aspx'
                string sPath = HttpContext.Current.Request.Url.ToString();
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            txtJD.EditFormatString = OConvert.GetDateFormat("Date");
            if (!IsPostBack)
            {
                txtJD.Value = Convert.ToDateTime(DateTime.Today);
            }
            btnAdd.Visible = false;
            ClientScript.RegisterStartupScript(typeof(Page), "Today", "<script>PageLoadFirst();</script>");
            //_____For performing operation without refreshing page___//
            String cbReference = Page.ClientScript.GetCallbackEventReference(this, "arg", "ReceiveServerData", "context");
            String callbackScript = "function CallServer(arg, context){ " + cbReference + ";}";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "CallServer", callbackScript, true);
            //___________-end here___//



            if (HttpContext.Current.Session["userid"] == null)
            {
                //Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }


            GridBind();
            ////this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");

            // btnGenerate.Visible = false;
        }

        #region for servercall
        void ICallbackEventHandler.RaiseCallbackEvent(string eventArgument)
        {
            //DBEngine oDBEngine = new DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);
            //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);MULTI
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
            string id = eventArgument.ToString();
            string[] FieldWvalue = id.Split('~');
            string IDs = "";
            data = "";
            if (FieldWvalue[0] == "read")
            {
                data = FieldWvalue[1];

            }

        }
        string ICallbackEventHandler.GetCallbackResult()
        {
            return data;

        }
        #endregion

        protected void GridMessage_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {


            string tranid = e.Parameters.ToString();
            if (tranid.Length != 0)
            {

                string[] mainid = tranid.Split('~');
                if (mainid[0].ToString() == "Delete")
                {
                    oDBEngine.DeleteValue("tbl_trans_RecruitmentDetailTemp", "rde_id ='" + mainid[1].ToString() + "'");
                    //this.Page.ClientScript.RegisterStartupScript(GetType(), "script4", "<script>height();</script>");
                    GridBind();
                }
                else if (mainid[0].ToString() == "Access")
                {
                    DataTable dtStat = new DataTable();
                    dtStat = oDBEngine.GetDataTable("tbl_trans_RecruitmentDetailTemp", " rde_Status ", "rde_id ='" + mainid[1].ToString() + "'");
                    if (dtStat.Rows[0][0].ToString() == "N")
                    {
                        oDBEngine.SetFieldValue("tbl_trans_RecruitmentDetailTemp", "rde_Status='Y'", "rde_id ='" + mainid[1].ToString() + "'");
                        GridBind();

                    }
                    else
                    {
                        oDBEngine.SetFieldValue("tbl_trans_RecruitmentDetailTemp", "rde_Status='N'", "rde_id ='" + mainid[1].ToString() + "'");
                        GridBind();
                    }
                }
                else if (mainid[0].ToString() == "Show")
                {
                    if (mainid[1].ToString() == "s")
                    {
                        GridMessage.Settings.ShowFilterRow = true;
                    }
                    else if (mainid[1].ToString() == "All")
                    {
                        GridMessage.FilterExpression = string.Empty;
                    }
                }


            }
        }



        protected void btnGenerate_Click1(object sender, EventArgs e)
        {
            DataSet ds = new DataSet();

            //using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))MULTI
            using (SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"])))
            {
                SqlCommand cmd = new SqlCommand();
                if (data.ToString().Length > 0)
                {
                    cmd.Connection = con;
                    cmd.CommandText = "select  rde_id,(select sal_name from  tbl_master_salutation where sal_id=rde_Salutation) as Salutation,rde_Name ,rde_ResidenceLocation,(select cmp_name from  tbl_master_Company where cmp_id=rde_Company)as company,(select branch_description from tbl_master_branch where branch_id= rde_Branch)as Branch,(select deg_designation from tbl_master_Designation where deg_id=rde_Designation) as Designation,rde_ApprovedCTC  from tbl_trans_RecruitmentDetailTemp where rde_id in (" + data + ") and rde_status='Y'";
                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = cmd;
                    cmd.CommandTimeout = 0;
                    ds.Reset();
                    da.Fill(ds);
                    da.Dispose();
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        oDBEngine.SetFieldValue("tbl_trans_RecruitmentDetailTemp", "rde_GenerateDate='" + oDBEngine.GetDate() + "',rde_GenerateUser='" + HttpContext.Current.Session["userid"] + "'", "rde_Id in (select  rde_id from tbl_trans_RecruitmentDetailTemp WHERE rde_id in (" + data + ") and rde_status='Y')");
                        ReportDocument report = new ReportDocument();
                        // ds.Tables[0].WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\ContactDetails.xsd");
                        //  ds.Tables[0].WriteXmlSchema("C:\\XSDFile\\OfferLetter.xsd");
                        //   ds.Tables[0].WriteXmlSchema("d:\\commonfolderinfluxcrm\\reports\\OfferLetter.xsd");
                        // ds.Tables[0].WriteXmlSchema("\\Reports\\ContactDetails.xsd");
                        //ds.Tables[0].WriteXmlSchema("d:\\commonfolderinfluxcrm\\reports\\OfferLetter.xsd");
                        string tmpPdfPath = string.Empty;
                        tmpPdfPath = HttpContext.Current.Server.MapPath("..\\Reports\\OfferLetter.rpt");
                        report.Load(tmpPdfPath);
                        report.SetDataSource(ds.Tables[0]);
                        report.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, HttpContext.Current.Response, true, "OfferLetter");

                        report.Dispose();
                        GC.Collect();
                        data = "";
                    }
                    else
                    {
                        this.Page.ClientScript.RegisterStartupScript(GetType(), "script1", "<script>alert('You can not Generate offer Letter without permission.');</script>");

                    }
                }
                else
                {
                    this.Page.ClientScript.RegisterStartupScript(GetType(), "script12", "<script>alert('Please Select Candidate!.');</script>");

                }
            }
        }

        public void GridBind()
        {
            DataTable dts = oDBEngine.GetDataTable("tbl_master_user", "user_SuperUser", "user_id=" + HttpContext.Current.Session["userid"] + "");

            if (dts.Rows[0][0].ToString().Trim() == "Y")
            {
                DataTable DT = new DataTable();
                // DT = oDBEngine.GetDataTable("tbl_trans_RecruitmentDetailTemp ", " rde_id,(select sal_name from  tbl_master_salutation where sal_id=rde_Salutation) as Salutation,rde_Name ,rde_ResidenceLocation,(select cmp_name from  tbl_master_Company where cmp_id=rde_Company)as company,(select branch_description from tbl_master_branch where branch_id= rde_Branch)as Branch,(select deg_designation from tbl_master_Designation where deg_id=rde_Designation) as Designation,rde_ApprovedCTC ,(select user_name from tbl_master_User where user_id=tbl_trans_RecruitmentDetailTemp.CreateUser) as CreateUserName , CONVERT(VARCHAR(8), CreateDate, 5)  as CreateDate,case when rde_status='N' then 'Under Process' else 'Generate' end as Status ", " rde_branch in  (" + HttpContext.Current.Session["userbranchHierarchy"] + ")", "rde_Name");
                DT = oDBEngine.GetDataTable("tbl_trans_RecruitmentDetailTemp ", " rde_id,(select sal_name from  tbl_master_salutation where sal_id=rde_Salutation) as Salutation,rde_Name ,rde_ResidenceLocation,(select cmp_name from  tbl_master_Company where cmp_id=rde_Company)as company,(select branch_description from tbl_master_branch where branch_id= rde_Branch)as Branch,(select deg_designation from tbl_master_Designation where deg_id=rde_Designation) as Designation,rde_ApprovedCTC ,rde_NoofDepedent,(select user_name from tbl_master_User where user_id=tbl_trans_RecruitmentDetailTemp.CreateUser) as CreateUserName , CONVERT(VARCHAR(20), createDate, 100)  as CreateDate,case when rde_status='N' then 'Under Process' else 'Generate' end as Status ,(select user_name from tbl_master_User where user_id=tbl_trans_RecruitmentDetailTemp.rde_GenerateUser) as GenerateUserName , CONVERT(VARCHAR(20), rde_GenerateDate, 100)  as GenerateDate,(case when rde_IsConfirmJoin='Y' then 'Joined' else 'Not Joined' end) as rde_IsConfirmJoin, CONVERT(VARCHAR(12), rde_ProbableJoinDate, 107) as JoiningDate", " rde_IsEmployee <> 'Y' ", " CreateDate desc");
                if (DT.Rows.Count != 0)
                {
                    GridMessage.DataSource = DT;
                    GridMessage.DataBind();
                }
                GridMessage.Columns[15].Visible = true;
                btnAdd.Visible = true;
            }
            else
            {
                DataTable DT = new DataTable();
                // DT = oDBEngine.GetDataTable("tbl_trans_RecruitmentDetailTemp ", " rde_id,(select sal_name from  tbl_master_salutation where sal_id=rde_Salutation) as Salutation,rde_Name ,rde_ResidenceLocation,(select cmp_name from  tbl_master_Company where cmp_id=rde_Company)as company,(select branch_description from tbl_master_branch where branch_id= rde_Branch)as Branch,(select deg_designation from tbl_master_Designation where deg_id=rde_Designation) as Designation,rde_ApprovedCTC ,(select user_name from tbl_master_User where user_id=tbl_trans_RecruitmentDetailTemp.CreateUser) as CreateUserName , CONVERT(VARCHAR(8), CreateDate, 5)  as CreateDate,case when rde_status='N' then 'Under Process' else 'Generate' end as Status ", " rde_branch in  (" + HttpContext.Current.Session["userbranchHierarchy"] + ")", "rde_Name");
                DT = oDBEngine.GetDataTable("tbl_trans_RecruitmentDetailTemp ", " rde_id,(select sal_name from  tbl_master_salutation where sal_id=rde_Salutation) as Salutation,rde_Name ,rde_ResidenceLocation,(select cmp_name from  tbl_master_Company where cmp_id=rde_Company)as company,(select branch_description from tbl_master_branch where branch_id= rde_Branch)as Branch,(select deg_designation from tbl_master_Designation where deg_id=rde_Designation) as Designation,rde_ApprovedCTC ,rde_NoofDepedent,(select user_name from tbl_master_User where user_id=tbl_trans_RecruitmentDetailTemp.CreateUser) as CreateUserName , CONVERT(VARCHAR(20), createDate, 100)  as CreateDate,case when rde_status='N' then 'Under Process' else 'Generate' end as Status ,(select user_name from tbl_master_User where user_id=tbl_trans_RecruitmentDetailTemp.rde_GenerateUser) as GenerateUserName , CONVERT(VARCHAR(20), rde_GenerateDate, 100)  as GenerateDate,(case when rde_IsConfirmJoin='Y' then 'Joined' else 'Not Joined' end) as rde_IsConfirmJoin,CONVERT(VARCHAR(12), rde_ProbableJoinDate, 107) as JoiningDate", " CreateUser= '" + HttpContext.Current.Session["userid"] + "' and rde_IsEmployee <> 'Y' ", " CreateDate desc");
                if (DT.Rows.Count != 0)
                {
                    GridMessage.DataSource = DT;
                    GridMessage.DataBind();
                }
                GridMessage.Columns[15].Visible = false;

            }


            //this.Page.ClientScript.RegisterStartupScript(GetType(), "heighSCR", "<script>height();</script>");



            //GridMessage.Columns[5].Visible = false;
        }
        protected void btnAdd_Click(object sender, EventArgs e)
        {

            DataTable dtV = new DataTable();
            DataTable dtS = new DataTable();
            DataTable dtB = new DataTable();
            DataTable dtC = new DataTable();
            string CompCode = string.Empty;
            int EmpCode;
            String ShortName = string.Empty;
            string TempCode = string.Empty;
            if (data.ToString().Length > 0)
            {
                dtV = oDBEngine.GetDataTable("tbl_trans_RecruitmentDetailTemp ", " * ", " rde_id in (" + data + ") and rde_status='Y' and rde_IsConfirmJoin='Y' and rde_IsEmployee='N'");
                if (dtV.Rows.Count > 0)
                {


                    for (int i = 0; i < dtV.Rows.Count; i++)
                    {

                        // dtV = oDBEngine.GetDataTable("tbl_trans_RecruitmentDetailTemp ", " * ", " rde_id in (" + data + ") and rde_status='Y'");

                        //--------------Generate Employee Code------------------------


                        if (dtV.Rows[i]["rde_Company"].ToString().Length != 0 || dtV.Rows[i]["rde_Branch"].ToString().Length != 0)
                        {
                            dtS = oDBEngine.GetDataTable("tbl_master_company", "cmp_OffRoleShortName,cmp_OnRoleShortName", "cmp_id=" + dtV.Rows[i]["rde_Company"] + "");
                            dtB = oDBEngine.GetDataTable("tbl_master_branch", "branch_Code", "branch_id=" + dtV.Rows[i]["rde_Branch"] + "");
                            if (dtB.Rows.Count > 0)
                            {
                                if (dtS.Rows.Count > 0)
                                {
                                    if (dtV.Rows[i]["rde_EmpType"].ToString().Length != 0)
                                    {
                                        if (dtV.Rows[i]["rde_EmpType"].ToString() == "1")
                                        {
                                            CompCode = dtS.Rows[0]["cmp_OnRoleShortName"].ToString() + dtB.Rows[0]["branch_Code"].ToString();

                                        }
                                        else
                                        {
                                            CompCode = dtS.Rows[0]["cmp_OffRoleShortName"].ToString() + dtB.Rows[0]["branch_Code"].ToString();

                                        }

                                    }
                                    else
                                    {

                                        this.Page.ClientScript.RegisterStartupScript(GetType(), "script29", "<script>alert('Employee Type Not Found!);return false;</script>");

                                    }

                                }
                                else
                                {

                                    this.Page.ClientScript.RegisterStartupScript(GetType(), "script24", "<script>alert('Company Short Name Not Found!);</script>");

                                }
                            }
                            else
                            {
                                this.Page.ClientScript.RegisterStartupScript(GetType(), "script25", "<script>alert('Branch Code Not Found!);</script>");

                            }


                            dtC = oDBEngine.GetDataTable("tbl_master_contact", "max(Cnt_UCC) ", "Cnt_UCC like '" + CompCode.ToString().Trim() + "%' and cnt_internalid like 'EM%'");
                            if (dtC.Rows.Count > 0)
                            {
                                if (dtC.Rows[0][0].ToString().Length != 0)
                                {
                                    int j = dtC.Rows[0][0].ToString().Length;
                                    int k = j - 7;
                                    EmpCode = Convert.ToInt32(dtC.Rows[0][0].ToString().Substring(7, k)) + 1;
                                    if (EmpCode.ToString().Length > 0)
                                    {
                                        if (EmpCode.ToString().Length == 1)
                                        {
                                            TempCode = "00" + EmpCode.ToString();

                                        }
                                        else if (EmpCode.ToString().Length == 2)
                                        {
                                            TempCode = "0" + EmpCode.ToString();


                                        }
                                        else
                                        {
                                            TempCode = EmpCode.ToString();

                                        }
                                        CompCode = CompCode.ToString().Trim() + TempCode.ToString().Trim();




                                    }
                                }
                                else
                                {
                                    CompCode = CompCode.ToString().Trim() + "001";

                                }
                            }
                            else
                            {
                                CompCode = CompCode.ToString().Trim() + "001";

                            }

                        }

                        //-------------------------------------------------------------
                        //if (dtV.Rows.Count > 0)
                        //{
                        //    for (int i = 0; i < dtV.Rows.Count; i++)
                        //    {

                        if (CompCode.ToString().Length > 9)
                        {
                            //String con = ConfigurationSettings.AppSettings["DBConnectionDefault"];
                            //SqlConnection lcon = new SqlConnection(con);
                            //lcon.Open();
                            //SqlCommand lcmdEmplInsert = new SqlCommand("sp_CandidateEmployeeInsert", lcon);
                            //lcmdEmplInsert.CommandType = CommandType.StoredProcedure;
                            //lcmdEmplInsert.Parameters.AddWithValue("@cnt_ucc", CompCode);
                            //lcmdEmplInsert.Parameters.AddWithValue("@cnt_salutation", dtV.Rows[i]["rde_Salutation"]);
                            //lcmdEmplInsert.Parameters.AddWithValue("@cnt_firstName", dtV.Rows[i]["rde_Name"]);
                            ////lcmdEmplInsert.Parameters.AddWithValue("@cnt_middleName", dtV.Rows[i]["rde_Salutation"]);
                            ////lcmdEmplInsert.Parameters.AddWithValue("@cnt_lastName", dtV.Rows[i]["rde_Salutation"]);
                            ////lcmdEmplInsert.Parameters.AddWithValue("@cnt_shortName", dtV.Rows[i]["rde_Salutation"]);
                            //lcmdEmplInsert.Parameters.AddWithValue("@cnt_branchId", dtV.Rows[i]["rde_Branch"]);
                            //lcmdEmplInsert.Parameters.AddWithValue("@cnt_sex", dtV.Rows[i]["rde_CandidateSex"]);
                            //lcmdEmplInsert.Parameters.AddWithValue("@cnt_maritalStatus", dtV.Rows[i]["rde_MaritalStatus"]);
                            //if (dtV.Rows[i]["rde_DOB"].ToString() != null)
                            //{
                            //    lcmdEmplInsert.Parameters.AddWithValue("@cnt_DOB", dtV.Rows[i]["rde_DOB"]);
                            //}
                            //else
                            //{
                            //    lcmdEmplInsert.Parameters.AddWithValue("@cnt_DOB", "");
                            //}
                            //SqlParameter parameter = new SqlParameter("@result", SqlDbType.VarChar, 50);
                            //parameter.Direction = ParameterDirection.Output;
                            /////_______________________________________________//

                            //lcmdEmplInsert.Parameters.AddWithValue("@cnt_education", dtV.Rows[i]["rde_EduQualification"]);
                            //lcmdEmplInsert.Parameters.AddWithValue("@cnt_contactSource", dtV.Rows[i]["rde_SourceType"]);
                            //lcmdEmplInsert.Parameters.AddWithValue("@cnt_referedBy", dtV.Rows[i]["rde_SourceName"]);
                            //lcmdEmplInsert.Parameters.AddWithValue("@cnt_contactType", "EM");
                            //lcmdEmplInsert.Parameters.AddWithValue("@lastModifyUser", HttpContext.Current.Session["userid"]);
                            //lcmdEmplInsert.Parameters.AddWithValue("@DateOfJoining", dtV.Rows[i]["rde_ProbableJoinDate"]);
                            //lcmdEmplInsert.Parameters.AddWithValue("@Organization", dtV.Rows[i]["rde_Company"]);
                            //lcmdEmplInsert.Parameters.AddWithValue("@Designation", dtV.Rows[i]["rde_Designation"]);
                            //lcmdEmplInsert.Parameters.AddWithValue("@AprovedCTC", dtV.Rows[i]["rde_ApprovedCTC"]);
                            //lcmdEmplInsert.Parameters.AddWithValue("@emp_typee", dtV.Rows[i]["rde_EmpType"]);

                            //lcmdEmplInsert.Parameters.AddWithValue("@rde_id", dtV.Rows[i]["rde_id"]);
                            //lcmdEmplInsert.Parameters.AddWithValue("@rde_NoofDepedent", dtV.Rows[i]["rde_NoofDepedent"]);
                            //lcmdEmplInsert.Parameters.AddWithValue("@MobileNo", dtV.Rows[i]["rde_PhoneNo"]);
                            //lcmdEmplInsert.Parameters.AddWithValue("@rde_ResidenceLocation", dtV.Rows[i]["rde_ResidenceLocation"]);
                            //lcmdEmplInsert.Parameters.AddWithValue("@rde_Email", dtV.Rows[i]["rde_Email"]);
                            //lcmdEmplInsert.Parameters.Add(parameter);
                            //lcmdEmplInsert.ExecuteNonQuery(); 
                            //string InternalID = parameter.Value.ToString();

                            string cnt_DOB = "";
                            string cnt_contactType = "EM";
                            if (dtV.Rows[i]["rde_DOB"].ToString() != null)
                            {
                                cnt_DOB = dtV.Rows[i]["rde_DOB"].ToString();
                            }
                            else
                            {
                                cnt_DOB = "";
                            }
                            string InternalID = oUtilities.CandidateEmployeeInsert(CompCode, dtV.Rows[i]["rde_Salutation"].ToString(), dtV.Rows[i]["rde_Name"].ToString(),
                                dtV.Rows[i]["rde_Branch"].ToString(), dtV.Rows[i]["rde_CandidateSex"].ToString(), dtV.Rows[i]["rde_MaritalStatus"].ToString(), dtV.Rows[i]["rde_DOB"].ToString(),
                                dtV.Rows[i]["rde_EduQualification"].ToString(), dtV.Rows[i]["rde_SourceType"].ToString(), dtV.Rows[i]["rde_SourceName"].ToString(), cnt_contactType,
                                HttpContext.Current.Session["userid"].ToString(), dtV.Rows[i]["rde_ProbableJoinDate"].ToString(), dtV.Rows[i]["rde_Company"].ToString(), dtV.Rows[i]["rde_Designation"].ToString(),
                                dtV.Rows[i]["rde_ApprovedCTC"].ToString(), dtV.Rows[i]["rde_EmpType"].ToString(), dtV.Rows[i]["rde_id"].ToString(), dtV.Rows[i]["rde_NoofDepedent"].ToString(), dtV.Rows[i]["rde_PhoneNo"].ToString(),
                                dtV.Rows[i]["rde_ResidenceLocation"].ToString(), dtV.Rows[i]["rde_Email"].ToString());

                            if (InternalID.ToString().Length > 0)
                            {
                                oDBEngine.SetFieldValue("tbl_trans_RecruitmentDetailTemp", "rde_IsEmployee='Y'", " rde_Id=" + dtV.Rows[i]["rde_Id"] + "");
                                GridBind();
                                this.Page.ClientScript.RegisterStartupScript(GetType(), "script11", "<script>alert('Employee Successfully Added!.');</script>");

                                data = "";
                            }
                            data = "";
                        }
                        else
                        {
                            this.Page.ClientScript.RegisterStartupScript(GetType(), "script11", "<script>alert('Emp Code Required!.');</script>");

                        }
                    }

                }
                else
                {
                    this.Page.ClientScript.RegisterStartupScript(GetType(), "script15", "<script>alert('Permission Required to Add an Employee!.');</script>");
                }

                //    }
                //}
            }
            else
            {

                this.Page.ClientScript.RegisterStartupScript(GetType(), "script28", "<script>alert('Please Select Employee From List!.');</script>");
            }



        }
        protected void GridMessage_CustomJSProperties(object sender, DevExpress.Web.ASPxGridViewClientJSPropertiesEventArgs e)
        {
            e.Properties["cpHeight"] = "a";
        }
        protected void BtnJoin_Click(object sender, EventArgs e)
        {
            if (data.ToString().Length > 0)
            {
                DataTable dtJ = new DataTable();
                dtJ = oDBEngine.GetDataTable("tbl_trans_RecruitmentDetailTemp ", " * ", " rde_id in (" + data + ") and rde_status='Y'");
                for (int i = 0; i < dtJ.Rows.Count; i++)
                {
                    DataTable dts = oDBEngine.GetDataTable("tbl_master_user", "user_SuperUser", "user_id=" + HttpContext.Current.Session["userid"] + "");

                    if (dts.Rows[0][0].ToString().Trim() == "Y")
                    {
                        oDBEngine.SetFieldValue("tbl_trans_RecruitmentDetailTemp", "rde_ProbableJoinDate='" + txtJD.Value + "',rde_IsConfirmJoin='Y' ", "rde_Id=" + dtJ.Rows[i]["rde_Id"] + "");
                        GridBind();
                        this.Page.ClientScript.RegisterStartupScript(GetType(), "script36", "<script>alert('Candidate Already Joined!.');</script>");
                        data = "";
                    }
                    else
                    {
                        if (dtJ.Rows[i]["rde_IsConfirmJoin"].ToString() != "Y")
                        {

                            oDBEngine.SetFieldValue("tbl_trans_RecruitmentDetailTemp", "rde_ProbableJoinDate='" + txtJD.Value + "',rde_IsConfirmJoin='Y' ", "rde_Id=" + dtJ.Rows[i]["rde_Id"] + "");
                            GridBind();
                            data = "";
                        }
                        else
                        {
                            this.Page.ClientScript.RegisterStartupScript(GetType(), "script34", "<script>alert('Candidate Already Joined!.');</script>");
                        }

                    }


                }
            }
            else
            {
                this.Page.ClientScript.RegisterStartupScript(GetType(), "script33", "<script>alert('Please Select Employee From List!.');</script>");

            }
        }

    }
}