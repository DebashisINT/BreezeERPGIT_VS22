using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using BusinessLogicLayer;

namespace ERP.OMS.Management.ToolsUtilities
{
    public partial class management_toolsutilities_OfferLetter_CandidateConfirmation : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
    {
        string data = string.Empty;
        public string pageAccess = "";
        //DBEngine oDBEngine = new DBEngine(string.Empty);
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        //Converter OConvert = new Converter();
        //Converter oconverter = new Converter();
        BusinessLogicLayer.Converter OConvert = new BusinessLogicLayer.Converter();
        BusinessLogicLayer.Converter oconverter = new BusinessLogicLayer.Converter();
        Utilities oUtilities = new Utilities();
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


            if (!IsPostBack)
            {
                dtDate.EditFormatString = oconverter.GetDateFormat("Date");
                //dtDate.Value = oDBEngine.GetDate().AddDays((-1 * oDBEngine.GetDate().Day) + 1);
                dtDate.Value = oDBEngine.GetDate().AddDays((-1 * oDBEngine.GetDate().Day) + 1);
                dtToDate.EditFormatString = oconverter.GetDateFormat("Date");
                //dtToDate.Value = oDBEngine.GetDate();
                dtToDate.Value = oDBEngine.GetDate();
                Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script language='javascript'>PageLoadFirst();</script>");
                GetDataSource();
            }
            else
            {
                GridBind();
            }


            //_____For performing operation without refreshing page___//
            String cbReference = Page.ClientScript.GetCallbackEventReference(this, "arg", "ReceiveServerData", "context");
            String callbackScript = "function CallServer(arg, context){ " + cbReference + ";}";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "CallServer", callbackScript, true);
            //___________-end here___//



            if (HttpContext.Current.Session["userid"] == null)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }


            GridBind();
            //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");

            // btnGenerate.Visible = false;
        }

        #region for servercall
        void ICallbackEventHandler.RaiseCallbackEvent(string eventArgument)
        {
            //DBEngine oDBEngine = new DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);MULTI
            DBEngine oDBEngine = new DBEngine();
            string id = eventArgument.ToString();
            string[] FieldWvalue = id.Split('~');
            string IDs = "";
            if (FieldWvalue[0] == "read")
            {
                data = FieldWvalue[1];
                HDNSelection.Value = FieldWvalue[1];
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
                    this.Page.ClientScript.RegisterStartupScript(GetType(), "script4", "<script>height();</script>");
                    GridBind();
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






        protected void Button1_Click(object sender, EventArgs e)
        {
            GetDataSource();

        }
        protected void GetDataSource()
        {
            string startdate = dtDate.Date.Month.ToString() + "/" + dtDate.Date.Day.ToString() + "/" + dtDate.Date.Year.ToString() + " 00:01 AM";
            string Enddate = dtToDate.Date.Month.ToString() + "/" + dtToDate.Date.Day.ToString() + "/" + dtToDate.Date.Year.ToString() + " 11:59 PM";
            string WhereCond = "";
            DataTable DT = new DataTable();
            if (RadAllRecord.Checked == true)
            {

                WhereCond = "";

            }
            else
            {
                WhereCond = " and  (CAST(createDate AS datetime) >= CONVERT(varchar,'" + startdate + "', 101)) and (CAST(createDate AS datetime) <= CONVERT(varchar,'" + Enddate + "', 101))  ";
            }
            DT = oDBEngine.GetDataTable("tbl_trans_RecruitmentDetailTemp ", " rde_id,(select sal_name from  tbl_master_salutation where sal_id=rde_Salutation) as Salutation,rde_Name ,rde_ResidenceLocation,(select cmp_name from  tbl_master_Company where cmp_id=rde_Company)as company,(select branch_description from tbl_master_branch where branch_id= rde_Branch)as Branch,(select deg_designation from tbl_master_Designation where deg_id=rde_Designation) as Designation,rde_ApprovedCTC ,rde_NoofDepedent,(select user_name from tbl_master_User where user_id=tbl_trans_RecruitmentDetailTemp.CreateUser) as CreateUserName , CONVERT(VARCHAR(20), createDate, 100)  as CreateDate1,case when rde_status='N' then 'Under Process' else 'Generate' end as Status ,(select user_name from tbl_master_User where user_id=tbl_trans_RecruitmentDetailTemp.rde_GenerateUser) as GenerateUserName , CONVERT(VARCHAR(20), rde_GenerateDate, 100)  as GenerateDate,(case when rde_IsConfirmJoin='Y' then 'Joined' else 'Not Join' end) as rde_IsConfirmJoin, CONVERT(VARCHAR(12), rde_ProbableJoinDate, 107) as JoiningDate", " rde_IsEmployee <> 'Y'   and  rde_Status='Y' and rde_IsConfirmJoin='Y' " + WhereCond, " LastModifyDAte  desc");

            if (DT.Rows.Count > 0)
            {
                ViewState["dtBind"] = DT;
                GridBind();
            }
            else
            {
                ViewState["dtBind"] = DT;
                GridBind();
                //  ScriptManager.RegisterStartupScript(this, this.GetType(), "script74", "alert('No Record Found!.');", true);
            }
        }

        public void GridBind()
        {
            if (ViewState["dtBind"] != null)
            {
                DataTable dtNew = (DataTable)ViewState["dtBind"];
                //if (dtNew.Rows.Count != 0)
                //{
                GridMessage.DataSource = dtNew;
                GridMessage.DataBind();
                //}

            }
            this.Page.ClientScript.RegisterStartupScript(GetType(), "heighSCR", "<script>height();</script>");





        }
        protected void GridMessage_PageIndexChanged(object sender, EventArgs e)
        {
            GridBind();
        }

        protected void GridMessage_CustomJSProperties(object sender, DevExpress.Web.ASPxGridViewClientJSPropertiesEventArgs e)
        {
            e.Properties["cpHeight"] = "a";
        }


        protected void GridMessage_HtmlDataCellPrepared(object sender, DevExpress.Web.ASPxGridViewTableDataCellEventArgs e)
        {
            string val = "";
            string aa = e.DataColumn.Caption.ToString();
            if (aa == "Status")
            {
                val = e.CellValue.ToString();
                changeColor(val.Trim(), e.Cell);
            }
            else if (aa == "Join Status")
            {
                val = e.CellValue.ToString();
                changeColor(val.Trim(), e.Cell);
            }
        }

        protected void changeColor(string value, System.Web.UI.WebControls.TableCell tc)
        {

            if (value == "Under Process")
            {
                tc.BackColor = System.Drawing.Color.FromName("#99CCFF");
                tc.Text = "Under Process";
                tc.ToolTip = "Candidate not varified!";
            }
            else if (value == "Generate")
            {
                tc.BackColor = System.Drawing.Color.FromName("#66CC99");
                tc.Text = "Generate";
                tc.ToolTip = "Varify Candidate!";
            }
            else if (value == "Not Join")
            {
                tc.BackColor = System.Drawing.Color.FromName("#99CCFF");
                tc.Text = "Not Join";
                tc.ToolTip = "Candidate Still Not Join!";
            }
            else if (value == "Joined")
            {
                tc.BackColor = System.Drawing.Color.FromName("#66CC99");
                tc.Text = "Joined";
                tc.ToolTip = "Candidate has already joined!";
            }
        }


        protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(cmbExport.SelectedItem.Value.ToString());
            switch (Filter)
            {
                case 1:
                    exporter.WritePdfToResponse();
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
            if (HDNSelection.Value.ToString().Length > 0)
            {
                dtV = oDBEngine.GetDataTable("tbl_trans_RecruitmentDetailTemp ", " * ", " rde_id in (" + HDNSelection.Value + ") and rde_status='Y' and rde_IsConfirmJoin='Y' and rde_IsEmployee='N'");
                if (dtV.Rows.Count > 0)
                {


                    for (int i = 0; i < dtV.Rows.Count; i++)
                    {

                        // dtV = oDBEngine.GetDataTable("tbl_trans_RecruitmentDetailTemp ", " * ", " rde_id in (" + HDNSelection.Value + ") and rde_status='Y'");

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
                                        this.Page.ClientScript.RegisterStartupScript(GetType(), "scripta", "<script>HideFilter();</script>");
                                        this.Page.ClientScript.RegisterStartupScript(GetType(), "script29", "<script>alert('Employee Type Not Found!);return false;</script>");
                                        //   ScriptManager.RegisterStartupScript(this, this.GetType(), "scripts24", "alert('Employee Type Not Found!);return false;", true);

                                    }

                                }
                                else
                                {
                                    this.Page.ClientScript.RegisterStartupScript(GetType(), "scriptb", "<script>HideFilter();</script>");
                                    this.Page.ClientScript.RegisterStartupScript(GetType(), "script24", "<script>alert('Company Short Name Not Found!);</script>");
                                    // ScriptManager.RegisterStartupScript(this, this.GetType(), "scripts84", "alert('Company Short Name Not Found!);return false;", true);

                                }
                            }
                            else
                            {
                                this.Page.ClientScript.RegisterStartupScript(GetType(), "scriptc1", "<script>HideFilter();</script>");
                                this.Page.ClientScript.RegisterStartupScript(GetType(), "script25", "<script>alert('Branch Code Not Found!);</script>");
                                // ScriptManager.RegisterStartupScript(this, this.GetType(), "scripts54", "alert('Branch Code Not Found!);return false;", true);

                            }


                            dtC = oDBEngine.GetDataTable("tbl_master_contact", " max(cast(substring(cnt_shortname,9,5) as  int))   ", "cnt_shortname like '" + CompCode.ToString().Trim() + "%' and cnt_internalid like 'EM%'");
                            if (dtC.Rows.Count > 0)
                            {
                                if (dtC.Rows[0][0].ToString().Length != 0)
                                {
                                    EmpCode = Convert.ToInt32(dtC.Rows[0][0].ToString()) + 1;
                                    if (dtC.Rows[0][0].ToString().Length > 0)
                                    {
                                        if (EmpCode.ToString().Length == 1)
                                        {
                                            TempCode = "0000" + EmpCode.ToString();
                                        }
                                        else if (EmpCode.ToString().Length == 2)
                                        {

                                            TempCode = "000" + EmpCode.ToString();
                                        }
                                        else if (EmpCode.ToString().Length == 3)
                                        {

                                            TempCode = "00" + EmpCode.ToString();
                                        }
                                        else if (EmpCode.ToString().Length == 4)
                                        {
                                            TempCode = "0" + EmpCode.ToString();

                                        }
                                        else
                                        {
                                            TempCode = EmpCode.ToString();
                                        }

                                        CompCode = CompCode.ToString().Trim() + TempCode.ToString().Trim();


                                    }
                                    else
                                    {

                                        CompCode = CompCode.ToString().Trim() + "00001";
                                    }

                                    //int j = dtC.Rows[0][0].ToString().Length;
                                    //int k = j - 7;
                                    //EmpCode = Convert.ToInt32(dtC.Rows[0][0].ToString().Substring(7, k)) + 1;
                                    //if (EmpCode.ToString().Length > 0)
                                    //{
                                    //    if (EmpCode.ToString().Length == 1)
                                    //    {
                                    //        TempCode = "00" + EmpCode.ToString();
                                    //    }
                                    //    else if (EmpCode.ToString().Length == 2)
                                    //    {
                                    //        TempCode = "0" + EmpCode.ToString();
                                    //    }
                                    //    else
                                    //    {
                                    //        TempCode = EmpCode.ToString();

                                    //    }
                                    //    CompCode = CompCode.ToString().Trim() + TempCode.ToString().Trim();




                                    //}
                                }
                                else
                                {
                                    CompCode = CompCode.ToString().Trim() + "00001";

                                }
                            }
                            else
                            {
                                CompCode = CompCode.ToString().Trim() + "00001";

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
                            //lcmdEmplInsert.Parameters.AddWithValue("@lastModifyUser", dtV.Rows[i]["CreateUser"]);
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
                            //lcmdEmplInsert.Parameters.AddWithValue("@rde_ReportTo", dtV.Rows[i]["rde_ReportTo"]);
                            ////lcmdEmplInsert.Parameters.AddWithValue("@rde_CurrentJobProfile", dtV.Rows[i]["rde_CurrentJobProfile"]);
                            //lcmdEmplInsert.Parameters.AddWithValue("@rde_Dept", dtV.Rows[i]["rde_Dept"]);

                            //lcmdEmplInsert.Parameters.AddWithValue("@FatherName", dtV.Rows[i]["rde_FatherName"]);




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
                            if (InternalID.ToString() != "0")
                            {

                                oDBEngine.SetFieldValue("tbl_trans_RecruitmentDetailTemp", "rde_IsEmployee='Y'", " rde_Id=" + dtV.Rows[i]["rde_Id"] + "");
                                GetDataSource();
                                this.Page.ClientScript.RegisterStartupScript(GetType(), "script441", "<script>HideFilter();</script>");
                                this.Page.ClientScript.RegisterStartupScript(GetType(), "script11", "<script>alert('Employee Successfully Added!.');</script>");
                                // ScriptManager.RegisterStartupScript(this, this.GetType(), "scripts35", "alert('Employee Successfully Added!.');", true);

                                HDNSelection.Value = "";
                            }
                            HDNSelection.Value = "";
                        }
                        else
                        {
                            this.Page.ClientScript.RegisterStartupScript(GetType(), "scripte", "<script>HideFilter();</script>");
                            this.Page.ClientScript.RegisterStartupScript(GetType(), "script11", "<script>alert('Emp Code Required!.');</script>");
                            //  ScriptManager.RegisterStartupScript(this, this.GetType(), "scripts33", "alert('Emp Code Required!.');return false;", true);
                        }
                    }

                }
                else
                {
                    this.Page.ClientScript.RegisterStartupScript(GetType(), "scriptf", "<script>HideFilter();</script>");
                    this.Page.ClientScript.RegisterStartupScript(GetType(), "script15", "<script>alert('Permission Required to Add an Employee!.');</script>");
                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "scripts22", "alert('Permission Required to Add an Employee!.');return false;", true);
                }

                //    }
                //}
            }
            else
            {
                this.Page.ClientScript.RegisterStartupScript(GetType(), "scriptg", "<script>HideFilter();</script>");
                this.Page.ClientScript.RegisterStartupScript(GetType(), "script28", "<script>alert('Please Select Employee From List!.');</script>");
                // ScriptManager.RegisterStartupScript(this, this.GetType(), "scripts51", "alert('Please Select Employee From List!.');return false;", true);
            }



        }
    }
}