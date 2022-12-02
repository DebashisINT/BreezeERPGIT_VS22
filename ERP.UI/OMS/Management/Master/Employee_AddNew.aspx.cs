using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogicLayer;
using ClsDropDownlistNameSpace;
using ERP.OMS.CustomFunctions;
using System.Web.Services;
using System.Collections.Generic;

namespace ERP.OMS.Management.Master
{
    public partial class management_master_Employee_AddNew : ERP.OMS.ViewState_class.VSPage
    {
        // Tier architecture
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]); MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        BusinessLogicLayer.Converter Oconverter = new BusinessLogicLayer.Converter();
        BusinessLogicLayer.GenericMethod oGenericMethod;
        clsDropDownList oclsDropDownList = new clsDropDownList();
        static string CntID = string.Empty;
        string empCompCode = string.Empty;
        CommonBL cbl = new CommonBL();

        SelectListOptions objSelectListOptions = new SelectListOptions();
        DataTable DT = new DataTable();
        /*Rev Work Start 22.04.2022 MantiseID:0024850: Copy feature add in Employee master*/
        Int32 ID;
        public string WLanguage = "";
        public string SpLanguage = "";
        clsDropDownList OclsDropDownList = new clsDropDownList();
        /*Rev Work Close 22.04.2022 MantiseID:0024850: Copy feature add in Employee master*/
        protected void Page_Load(object sender, EventArgs e)
        {
            //cmbDOJ.UseMaskBehavior = true;
            //cmbDOJ.EditFormatString = Oconverter.GetDateFormat();
            //cmbDOJ.EditFormatString = "dd-MM-yyyy";
            //cmbDOJ.DisplayFormatString = "dd-MM-yyyy";

            //JoiningDate.UseMaskBehavior = true;
            //JoiningDate.EditFormatString = Oconverter.GetDateFormat();
            //JoiningDate.EditFormatString = "dd-MM-yyyy";
            //JoiningDate.DisplayFormatString = "dd-MM-yyyy";

            //cmbLeaveEff.UseMaskBehavior = true;
            //cmbLeaveEff.EditFormatString = Oconverter.GetDateFormat();
            //cmbLeaveEff.EditFormatString = "dd-MM-yyyy";
            //cmbLeaveEff.DisplayFormatString = "dd-MM-yyyy";

            if (!IsPostBack)
            {
                /*Rev Work Start 22.04.2022 MantiseID:0024850: Copy feature add in Employee master*/
                if (Request.QueryString["id"] == "ADD")
                {
                    /*Rev Work Close 22.04.2022 MantiseID:0024850: Copy feature add in Employee master*/
                    string IsSTBManagementRequired = cbl.GetSystemSettingsResult("IsSTBManagementRequired");
                    //Mantis Issue 0025113
                    string SMSRequiredInDirectorApproval = cbl.GetSystemSettingsResult("SMSRequiredInDirectorApproval");
                    //End of Mantis Issue 250113
                    if (!String.IsNullOrEmpty(IsSTBManagementRequired))
                    {
                        //Mantis Issue 0025113
                        //if (IsSTBManagementRequired.ToUpper().Trim() == "YES")
                        if (IsSTBManagementRequired.ToUpper().Trim() == "YES" || SMSRequiredInDirectorApproval.ToUpper().Trim() == "YES")
                        //End of Mantis Issue 250113
                        {
                            DivIsDirector.Style.Add("display", "!inline-block");

                        }
                        //Mantis Issue 0025113
                        //else if (IsSTBManagementRequired.ToUpper().Trim() == "NO")
                        else if (IsSTBManagementRequired.ToUpper().Trim() == "NO" && SMSRequiredInDirectorApproval.ToUpper().Trim() == "NO")
                        //End of Mantis Issue 250113
                        {
                            DivIsDirector.Style.Add("display", "none");

                        }
                    }
                    chkUsehierchy.Checked = false;
                    Page.ClientScript.RegisterStartupScript(GetType(), "PageLD", "<script>Pageload();</script>");
                    //txtReportTo.Attributes.Add("onkeyup", "CallList(this,'SearchByEmp',event)");
                    //txtReportTo.Attributes.Add("onfocus", "CallList(this,'SearchByEmp',event)");
                    //txtReportTo.Attributes.Add("onclick", "CallList(this,'SearchByEmp',event)");

                    Session["KeyVal_InternalID"] = "n";



                    DT = new DataTable();

                    DT = objSelectListOptions.SearchByEmp("", Session["KeyVal_InternalID"]);
                    ddlReportTo.DataSource = DT;
                    ddlReportTo.DataValueField = "Id";
                    ddlReportTo.DataTextField = "Name";
                    ddlReportTo.DataBind();




                    ddlReportTo.Items.Insert(0, "");


                    VendorTDSBl tdsdetails = new VendorTDSBl();
                    DataTable tdsMaster = tdsdetails.GetTDSMASTERLIST();
                    if (tdsMaster != null && tdsMaster.Rows.Count > 0)
                    {
                        cmbTDS.TextField = "TYPE_NAME";
                        cmbTDS.ValueField = "ID";
                        cmbTDS.DataSource = tdsMaster;
                        cmbTDS.DataBind();
                    }



                    //JoiningDate.UseMaskBehavior = true;
                    //JoiningDate.EditFormatString = Oconverter.GetDateFormat("Date");
                    //JoiningDate.Value = oDBEngine.GetDate();
                    //JoiningDate.EditFormatString = "dd-MM-yyyy";

                    ShowForm();

                    cmbDOJ.UseMaskBehavior = true;
                    cmbDOJ.EditFormatString = "dd-MM-yyyy";
                    cmbDOJ.DisplayFormatString = "dd-MM-yyyy";
                    cmbDOJ.Date = DateTime.Today;

                    JoiningDate.UseMaskBehavior = true;
                    JoiningDate.EditFormatString = "dd-MM-yyyy";
                    JoiningDate.DisplayFormatString = "dd-MM-yyyy";
                    JoiningDate.Date = DateTime.Today;

                    cmbLeaveEff.UseMaskBehavior = true;
                    cmbLeaveEff.EditFormatString = "dd-MM-yyyy";
                    cmbLeaveEff.DisplayFormatString = "dd-MM-yyyy";
                    cmbLeaveEff.Date = DateTime.Today;
                    /*Rev Work Start 22.04.2022 MantiseID:0024850: Copy feature add in Employee master*/
                }
                else
                {
                    string IsSTBManagementRequired = cbl.GetSystemSettingsResult("IsSTBManagementRequired");
                    //Mantis Issue 0025113
                    string SMSRequiredInDirectorApproval = cbl.GetSystemSettingsResult("SMSRequiredInDirectorApproval");
                    //End of Mantis Issue 250113
                    if (!String.IsNullOrEmpty(IsSTBManagementRequired))
                    {
                        //Mantis Issue 0025113
                        //if (IsSTBManagementRequired.ToUpper().Trim() == "YES")
                        if (IsSTBManagementRequired.ToUpper().Trim() == "YES" || SMSRequiredInDirectorApproval.ToUpper().Trim() == "YES")
                        //End of Mantis Issue 250113
                        {
                            DivIsDirector.Style.Add("display", "!inline-block");

                        }
                        //Mantis Issue 0025113
                        //else if (IsSTBManagementRequired.ToUpper().Trim() == "NO")
                        else if (IsSTBManagementRequired.ToUpper().Trim() == "NO" && SMSRequiredInDirectorApproval.ToUpper().Trim() == "NO")
                        //End of Mantis Issue 250113
                        {
                            DivIsDirector.Style.Add("display", "none");

                        }
                    }
                    chkUsehierchy.Checked = false;
                    Page.ClientScript.RegisterStartupScript(GetType(), "PageLD", "<script>Pageload();</script>");
                    Session["KeyVal_InternalID"] = "n";
                    DT = new DataTable();

                    DT = objSelectListOptions.SearchByEmp("", Session["KeyVal_InternalID"]);
                    ddlReportTo.DataSource = DT;
                    ddlReportTo.DataValueField = "Id";
                    ddlReportTo.DataTextField = "Name";
                    ddlReportTo.DataBind();
                    ddlReportTo.Items.Insert(0, "");


                    VendorTDSBl tdsdetails = new VendorTDSBl();
                    DataTable tdsMaster = tdsdetails.GetTDSMASTERLIST();
                    if (tdsMaster != null && tdsMaster.Rows.Count > 0)
                    {
                        cmbTDS.TextField = "TYPE_NAME";
                        cmbTDS.ValueField = "ID";
                        cmbTDS.DataSource = tdsMaster;
                        cmbTDS.DataBind();
                    }

                    ShowForm();

                    cmbDOJ.UseMaskBehavior = true;
                    cmbDOJ.EditFormatString = "dd-MM-yyyy";
                    cmbDOJ.DisplayFormatString = "dd-MM-yyyy";
                    cmbDOJ.EditFormatString = Oconverter.GetDateFormat();
                    //cmbDOJ.Date = DateTime.Today;

                    JoiningDate.UseMaskBehavior = true;
                    JoiningDate.EditFormatString = "dd-MM-yyyy";
                    JoiningDate.DisplayFormatString = "dd-MM-yyyy";
                    //JoiningDate.Date = DateTime.Today;
                    JoiningDate.EditFormatString = Oconverter.GetDateFormat();

                    cmbLeaveEff.UseMaskBehavior = true;
                    cmbLeaveEff.EditFormatString = "dd-MM-yyyy";
                    cmbLeaveEff.DisplayFormatString = "dd-MM-yyyy";
                    //cmbLeaveEff.Date = DateTime.Today;
                    cmbLeaveEff.EditFormatString = Oconverter.GetDateFormat();
                }
                /*Rev Work Close 22.04.2022 MantiseID:0024850: Copy feature add in Employee master*/
            }

            btnSave.Attributes.Add("Onclick", "Javascript:return ValidateGeneral();");
            btnJoin.Attributes.Add("Onclick", "Javascript:return ValidateDOJ();");
            btnCTC.Attributes.Add("Onclick", "Javascript:return ValidateCTC();");
            btnEmpID.Attributes.Add("Onclick", "Javascript:return ValidateEMPID();");
        }

        [WebMethod]
        public static List<string> GetreportTo(string firstname, string shortname)
        {
            //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]); MULTI
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
            DataTable DT = new DataTable();
            DT.Rows.Clear();
            //Mantis Issue 24689
            //DT = oDBEngine.GetDataTable("tbl_master_employee, tbl_master_contact,tbl_trans_employeeCTC", "ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') +'['+cnt_shortName+']' AS Name, tbl_master_employee.emp_id as Id    ", " tbl_master_employee.emp_contactId = tbl_trans_employeeCTC.emp_cntId and  tbl_trans_employeeCTC.emp_cntId = tbl_master_contact.cnt_internalId   and cnt_contactType='EM'  and (cnt_firstName Like '" + firstname + "%' or cnt_shortName like '" + shortname + "%')");
            DT = oDBEngine.GetDataTable("tbl_master_employee, tbl_master_contact,tbl_trans_employeeCTC", "distinct ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') +'['+cnt_shortName+']' AS Name, tbl_master_employee.emp_id as Id    ", " tbl_master_employee.emp_contactId = tbl_trans_employeeCTC.emp_cntId and  tbl_trans_employeeCTC.emp_cntId = tbl_master_contact.cnt_internalId   and cnt_contactType='EM'  and (cnt_firstName Like '" + firstname + "%' or cnt_shortName like '" + shortname + "%')");
            //End of Mantis Issue 24689
            List<string> obj = new List<string>();
            foreach (DataRow dr in DT.Rows)
            {

                obj.Add(Convert.ToString(dr["Name"]) + "|" + Convert.ToString(dr["Id"]));
            }
            return obj;
            //    Response.Write("No Record Found###No Record Found|");


        }
        protected void btnSave_Click(object sender, EventArgs e) // 1st
        {
            try
            {
                Employee_BL objEmployee = new Employee_BL();
                bool chkAllow = false;

                


                //Tire architecture
                string InternalID = objEmployee.btnSave_Click_BL(Convert.ToString(DBNull.Value), CmbSalutation.SelectedItem.Value, txtFirstNmae.Text,
                txtMiddleName.Text, txtLastName.Text, txtAliasName.Text, "0",
                cmbGender.SelectedItem.Value, Convert.ToString(DBNull.Value), Convert.ToString(DBNull.Value), Convert.ToString(DBNull.Value),
                Convert.ToString(DBNull.Value), Convert.ToString(DBNull.Value), Convert.ToString(DBNull.Value), Convert.ToString(DBNull.Value), Convert.ToString(DBNull.Value),
                chkAllow, Convert.ToString(DBNull.Value), chkUsehierchy.Checked, chkIsDirector.Checked);

                HttpContext.Current.Session["KeyVal_InternalID"] = InternalID;
                string[,] cnt_id = oDBEngine.GetFieldValue(" tbl_master_contact", " cnt_id", " cnt_internalId='" + InternalID + "'", 1);
                if (cnt_id[0, 0].ToString() != "n")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "General", "ForJoin();", true);
                }
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "General", "jAlert('Please Try Again!..');", true);
            }

            ScriptManager.RegisterStartupScript(this, this.GetType(), "General", "ForJoin();", true);
        }
        protected void btnJoin_Click(object sender, EventArgs e) // 2nd
        {
            oGenericMethod = new BusinessLogicLayer.GenericMethod();
            string value = string.Empty;
            //Checking For Expiry Date 
            string ValidationResult = oGenericMethod.IsProductExpired(Convert.ToDateTime(cmbDOJ.Value));
            if (Convert.ToBoolean(ValidationResult.Split('~')[0]))
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Vscript", "jAlert('" + ValidationResult.Split('~')[1] + "');", true);
            else
            {
                value = "emp_din=' ', emp_dateofJoining ='" + cmbDOJ.Value + "', emp_dateofLeaving ='" + DBNull.Value + "',emp_ReasonLeaving  ='" + DBNull.Value + "', emp_NextEmployer ='" + DBNull.Value + "', emp_AddNextEmployer  ='" + DBNull.Value + "',LastModifyDate='" + oDBEngine.GetDate().ToString() + "',LastModifyUser='" + Session["userid"].ToString() + "'";
                Int32 rowsEffected = oDBEngine.SetFieldValue("tbl_master_employee", value, " emp_contactid ='" + HttpContext.Current.Session["KeyVal_InternalID"] + "'");
                //Int32 RowsEffect = oDBEngine.SetFieldValue("tbl_trans_employeeCTC", "emp_effectiveuntil='" + cmbDOL.Value + "',LastModifyDate='" + oDBEngine.GetDate().ToString() + "',LastModifyUser='" + Session["userid"].ToString() + "'", " emp_cntId='" + HttpContext.Current.Session["KeyVal_InternalID"] + "'");
                if (rowsEffected > 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "CTC", "ForCTC();", true);
                    //DateTime dtDob;
                    //dtDob = Convert.ToDateTime(cmbDOJ.Value);
                    //JoiningDate.Date = dtDob;
                }

                if (Session["KeyVal_InternalID"] != "n")
                {
                    DataTable DT_empCTC = oDBEngine.GetDataTable(" tbl_trans_employeeCTC ", " emp_id ", " emp_cntId='" + Convert.ToString(Session["KeyVal_InternalID"]) + "'");
                    if (DT_empCTC.Rows.Count > 0)
                        JoiningDate.Value = oDBEngine.GetDate();
                    else
                    {
                        DataTable dt = oDBEngine.GetDataTable(" tbl_master_employee", "emp_dateofJoining", " (emp_contactId = '" + Convert.ToString(Session["KeyVal_InternalID"]) + "')");
                        if (dt.Rows.Count > 0)
                            JoiningDate.Value = dt.Rows[0][0];

                    }
                }
            }

        }
        protected void btnCTC_Click(object sender, EventArgs e)//3rd
        {
            try
            {
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "GeneralLoading", "startLoading();", true);
                // ------------ Check IF company Schema Exists -------------

                DataTable dtS = new DataTable();
                dtS = oDBEngine.GetDataTable("tbl_master_company", "onrole_schema_id, offrole_schema_id", "cmp_id=" + cmbOrganization.SelectedValue + "");

                if (dtS.Rows[0]["onrole_schema_id"].ToString() == "" || dtS.Rows[0]["offrole_schema_id"].ToString() == "")
                {
                    // ------------ Check IF company Schema Exists -------------
                    string alert = "'Please define Company On & Off Role Schema for " + cmbOrganization.SelectedItem.Text + "'";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "General", "jAlert(" + alert + ");", true);
                    return;
                }
                else if (!checkNMakeEmpCode(Convert.ToInt32(cmbOrganization.SelectedValue)))
                {
                    // ------------ Check for different validation ---------------
                    return;
                }

                //=======================For naming Part / 1st part ========================================
                Employee_BL objEmployee = new Employee_BL();
                bool chkAllow = false;
                //Tire architecture
                string InternalID = objEmployee.btnSave_Click_BL(Convert.ToString(DBNull.Value), CmbSalutation.SelectedItem.Value, txtFirstNmae.Text,
                txtMiddleName.Text, txtLastName.Text, txtAliasName.Text, "0",
                cmbGender.SelectedItem.Value, Convert.ToString(DBNull.Value), Convert.ToString(DBNull.Value), Convert.ToString(DBNull.Value),
                Convert.ToString(DBNull.Value), Convert.ToString(DBNull.Value), Convert.ToString(DBNull.Value), Convert.ToString(DBNull.Value), Convert.ToString(DBNull.Value),
                chkAllow, Convert.ToString(DBNull.Value), chkUsehierchy.Checked,chkIsDirector.Checked, Convert.ToInt64(cmbTDS.Value));

                /// Coded By Samrat Roy -- 18/04/2017 
                /// To Insert Contact Type selection on Employee Type (DME/ISD)
                //if (EmpType.SelectedValue == "21" || EmpType.SelectedValue == "19")
                if (EmpType.SelectedValue == "19")
                {
                    string contactPrefix = string.Empty;
                    switch (EmpType.SelectedValue)
                    {
                        //case "21":
                        //    contactPrefix = "FI";
                        //    break;
                        case "19":
                            contactPrefix = "DV";
                            break;

                    }
                    Employee_AddNew_BL objEmployee_AddNew_BL = new Employee_AddNew_BL();
                    string contactID = Request.Form["ctl00$ContentPlaceHolder1$ContactType"].ToString();
                    if (!string.IsNullOrEmpty(contactID))
                    {
                        objEmployee_AddNew_BL.InsertContactType_BL(InternalID, contactID, contactPrefix);
                    }
                }
                ///////###################################################//////

                HttpContext.Current.Session["KeyVal_InternalID"] = InternalID;
                string[,] cnt_id = oDBEngine.GetFieldValue(" tbl_master_contact", " cnt_id", " cnt_internalId='" + InternalID + "'", 1);
                if (cnt_id[0, 0].ToString() != "n")
                {
                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "General", "ForJoin();", true);

                    //===============================For join Date/ 2nd Part=====================================
                    oGenericMethod = new BusinessLogicLayer.GenericMethod();
                    string value = string.Empty;
                    //Checking For Expiry Date 
                    string ValidationResult = oGenericMethod.IsProductExpired(Convert.ToDateTime(cmbDOJ.Value));
                    if (Convert.ToBoolean(ValidationResult.Split('~')[0]))
                    {
                        //ScriptManager.RegisterStartupScript(this, this.GetType(), "Vscript", "jAlert('" + ValidationResult.Split('~')[1] + "');", true);
                    }
                    else
                    {
                        value = "emp_din=' ', emp_dateofJoining ='" + cmbDOJ.Value + "', emp_dateofLeaving ='" + DBNull.Value + "',emp_ReasonLeaving  ='" + DBNull.Value + "', emp_NextEmployer ='" + DBNull.Value + "', emp_AddNextEmployer  ='" + DBNull.Value + "',LastModifyDate='" + oDBEngine.GetDate().ToString() + "',LastModifyUser='" + Session["userid"].ToString() + "'";
                        Int32 rowsEffected = oDBEngine.SetFieldValue("tbl_master_employee", value, " emp_contactid ='" + HttpContext.Current.Session["KeyVal_InternalID"] + "'");

                        if (Session["KeyVal_InternalID"] != "n")
                        {
                            DataTable DT_empCTC = oDBEngine.GetDataTable(" tbl_trans_employeeCTC ", " emp_id ", " emp_cntId='" + Convert.ToString(Session["KeyVal_InternalID"]) + "'");
                            if (DT_empCTC.Rows.Count > 0)
                                JoiningDate.Value = oDBEngine.GetDate();
                            else
                            {
                                DataTable dt = oDBEngine.GetDataTable(" tbl_master_employee", "emp_dateofJoining", " (emp_contactId = '" + Convert.ToString(Session["KeyVal_InternalID"]) + "')");
                                if (dt.Rows.Count > 0)
                                    JoiningDate.Value = dt.Rows[0][0];
                            }
                        }

                        if (rowsEffected > 0)
                        {
                            Employee_BL objEmployee_BL = new Employee_BL();

                            string emp_cntId = Convert.ToString(HttpContext.Current.Session["KeyVal_InternalID"]);
                            CntID = Convert.ToString(HttpContext.Current.Session["KeyVal_InternalID"]);
                            string joiningDate = string.Empty;
                            string emp_LeaveSchemeAppliedFrom = string.Empty;
                            if (JoiningDate.Value != null)
                            {
                                joiningDate = JoiningDate.Value.ToString();
                            }
                            else
                            {
                                joiningDate = "";
                            }

                            if (cmbLeaveEff.Value != null)
                            {
                                emp_LeaveSchemeAppliedFrom = cmbLeaveEff.Value.ToString();
                            }
                            else
                            {
                                emp_LeaveSchemeAppliedFrom = "";
                            }

                            // Tire Architecture 
                            objEmployee_BL.btnCTC_Click_BL(emp_cntId, joiningDate,
                            cmbOrganization.SelectedItem.Value, cmbJobResponse.SelectedItem.Value, cmbDesg.SelectedItem.Value, EmpType.SelectedItem.Value,
                            cmbDept.SelectedItem.Value, txtReportTo_hidden.Value, txtReportTo_hidden.Value, txtReportTo_hidden.Value,
                            cmbWorkingHr.SelectedItem.Value, cmbLeaveP.SelectedItem.Value, emp_LeaveSchemeAppliedFrom, cmbBranch.SelectedItem.Value, Convert.ToString(DBNull.Value));

                            // Generate and save employee id
                            // added by Aditya 12-Dec-2016
                            oDBEngine.SetFieldValue("tbl_master_contact", "Cnt_UCC ='" + empCompCode.ToString() + "'", " cnt_internalid ='" + emp_cntId + "'");
                            oDBEngine.SetFieldValue("tbl_master_employee", "emp_uniquecode='" + empCompCode.ToString() + "'", "emp_contactID='" + emp_cntId + "'");
                            txtAliasName.Text = empCompCode.ToString();

                            ScriptManager.RegisterStartupScript(this, this.GetType(), "GeneralLoading", "stopLoading();", true);
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "EMP", "ForEMPID();", true);
                        }
                    }
                }
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "General", "jAlert('Please Try Again!..');", true);
            }
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "General", "ForJoin();", true);
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "GeneralLoading", "stopLoading();", true);
        }


        #region checkNMakeEmpCode BackUp
        //protected bool checkNMakeEmpCode(int cmp_Id)
        //{
        //    DataTable dtS = new DataTable();
        //    DataTable dtSchemaOn = new DataTable();
        //    DataTable dtSchemaOff = new DataTable();
        //    DataTable dtCTC = new DataTable();
        //    DataTable dtC = new DataTable();
        //    string prefCompCode = string.Empty, sufxCompCode = string.Empty, ShortName = string.Empty, TempCode = string.Empty,
        //        startNo, paddedStr;
        //    int EmpCode, prefLen, sufxLen, paddCounter;

        //    //string emp_cntId = Convert.ToString(HttpContext.Current.Session["KeyVal_InternalID"]);
        //    //dtCTC = oDBEngine.GetDataTable("tbl_trans_employeeCTC", " *", "emp_cntID='" + emp_cntId + "'");
        //    dtS = oDBEngine.GetDataTable("tbl_master_company", "onrole_schema_id, offrole_schema_id", "cmp_id=" + cmp_Id + "");

        //    if (dtS.Rows[0]["onrole_schema_id"].ToString() != "" && dtS.Rows[0]["offrole_schema_id"].ToString() != "")
        //    {
        //        dtSchemaOn = oDBEngine.GetDataTable("tbl_master_idschema", "prefix, suffix, digit, startno", "id=" + Convert.ToInt64(dtS.Rows[0]["onrole_schema_id"]));
        //        dtSchemaOff = oDBEngine.GetDataTable("tbl_master_idschema", "prefix, suffix, digit, startno", "id=" + Convert.ToInt64(dtS.Rows[0]["offrole_schema_id"]));

        //        if (Convert.ToInt32(EmpType.SelectedValue) > 0)
        //        {
        //            if (Convert.ToInt32(EmpType.SelectedValue) == 1 || Convert.ToInt32(EmpType.SelectedValue) == 2)
        //            {
        //                startNo = dtSchemaOn.Rows[0]["startno"].ToString();
        //                paddCounter = Convert.ToInt32(dtSchemaOn.Rows[0]["digit"]);
        //                paddedStr = startNo.PadLeft(paddCounter, '0');
        //                prefCompCode = dtSchemaOn.Rows[0]["prefix"].ToString();
        //                sufxCompCode = dtSchemaOn.Rows[0]["suffix"].ToString();
        //            }
        //            else
        //            {
        //                startNo = dtSchemaOff.Rows[0]["startno"].ToString();
        //                paddCounter = Convert.ToInt32(dtSchemaOff.Rows[0]["digit"]);
        //                prefCompCode = dtSchemaOff.Rows[0]["prefix"].ToString();
        //                sufxCompCode = dtSchemaOff.Rows[0]["suffix"].ToString();
        //            }
        //            prefLen = Convert.ToInt32(prefCompCode.Length);
        //            sufxLen = Convert.ToInt32(sufxCompCode.Length);

        //            string sql_statement = "SELECT max(tmc.Cnt_UCC) FROM tbl_master_contact tmc WHERE tmc.cnt_internalId IN(SELECT ttectc1.emp_cntId FROM tbl_trans_employeeCTC ttectc1 WHERE ttectc1.emp_Organization = " + cmp_Id + ") AND dbo.RegexMatch('";
        //            if (prefCompCode.Length > 0) sql_statement += "[a-zA-Z0-9-/\\[\\](){}]{" + prefLen + "}";
        //            if (startNo.Length > 0) sql_statement += "[0-9]{" + paddCounter + "}";
        //            if (sufxCompCode.Length > 0) sql_statement += "[a-zA-Z0-9-/\\[\\](){}]{" + sufxLen + "}";
        //            sql_statement += "?$', tmc.cnt_UCC) = 1 AND tmc.cnt_internalid like 'EM%'";
        //            dtC = oDBEngine.GetDataTable(sql_statement);

        //            if (dtC.Rows[0][0].ToString() == "")
        //            {
        //                sql_statement = "SELECT max(tmc.Cnt_UCC) FROM tbl_master_contact tmc WHERE tmc.cnt_internalId IN(SELECT ttectc1.emp_cntId FROM tbl_trans_employeeCTC ttectc1 WHERE ttectc1.emp_Organization = " + cmp_Id + ") AND dbo.RegexMatch('";
        //                if (prefCompCode.Length > 0) sql_statement += "[a-zA-Z0-9-/\\[\\](){}]{" + prefLen + "}";
        //                if (startNo.Length > 0) sql_statement += "[0-9]{" + (paddCounter - 1) + "}";
        //                if (sufxCompCode.Length > 0) sql_statement += "[a-zA-Z0-9-/\\[\\](){}]{" + sufxLen + "}";
        //                sql_statement += "?$', tmc.cnt_UCC) = 1 AND tmc.cnt_internalid like 'EM%'";
        //                dtC = oDBEngine.GetDataTable(sql_statement);
        //            }

        //            if (dtC.Rows.Count > 0 && dtC.Rows[0][0].ToString() != "")
        //            {
        //                string uccCode = dtC.Rows[0][0].ToString();
        //                int UCCLen = uccCode.Length;
        //                int decimalPartLen = UCCLen - (prefCompCode.Length + sufxCompCode.Length);
        //                string uccCodeSubstring = uccCode.Substring(prefCompCode.Length, decimalPartLen);
        //                EmpCode = Convert.ToInt32(uccCodeSubstring) + 1;

        //                if (EmpCode.ToString().Length > paddCounter)
        //                {
        //                    ScriptManager.RegisterStartupScript(this, this.GetType(), "General", "jAlert('Cannot Add More Employees as Schema Exausted for Current Employee Role. <br />Please Update Employee Schema.');", true);
        //                    return false;
        //                }
        //                else
        //                {
        //                    paddedStr = EmpCode.ToString().PadLeft(paddCounter, '0');
        //                    empCompCode = prefCompCode + paddedStr + sufxCompCode;
        //                    return true;
        //                }
        //            }
        //            else
        //            {
        //                paddedStr = startNo.PadLeft(paddCounter, '0');
        //                empCompCode = prefCompCode + paddedStr + sufxCompCode;
        //                return true;
        //            }
        //        }
        //        else
        //        {
        //            ScriptManager.RegisterStartupScript(this, this.GetType(), "JSct", "jAlert('Employee Type Not Found !);", true);
        //            return false;
        //        }
        //    }
        //    else
        //    {
        //        ScriptManager.RegisterStartupScript(this, this.GetType(), "JSct55", "jAlert('Company On / Off Role Schema Not Defined !');", true);
        //        return false;
        //    }
        //}

#endregion

         protected bool checkNMakeEmpCode(int cmp_Id)
        {
            DataTable dtS = new DataTable();
            DataTable dtSchemaOn = new DataTable();
            DataTable dtSchemaOff = new DataTable();
            DataTable dtCTC = new DataTable();
            DataTable dtC = new DataTable();
            string prefCompCode = string.Empty, sufxCompCode = string.Empty, ShortName = string.Empty, TempCode = string.Empty,
                startNo, paddedStr;
            int EmpCode, prefLen, sufxLen, paddCounter;

            //string emp_cntId = Convert.ToString(HttpContext.Current.Session["KeyVal_InternalID"]);
            //dtCTC = oDBEngine.GetDataTable("tbl_trans_employeeCTC", " *", "emp_cntID='" + emp_cntId + "'");
            dtS = oDBEngine.GetDataTable("tbl_master_company", "onrole_schema_id, offrole_schema_id", "cmp_id=" + cmp_Id + "");

            if (dtS.Rows[0]["onrole_schema_id"].ToString() != "" && dtS.Rows[0]["offrole_schema_id"].ToString() != "")
            {
                dtSchemaOn = oDBEngine.GetDataTable("tbl_master_idschema", "prefix, suffix, digit, startno", "id=" + Convert.ToInt64(dtS.Rows[0]["onrole_schema_id"]));
                dtSchemaOff = oDBEngine.GetDataTable("tbl_master_idschema", "prefix, suffix, digit, startno", "id=" + Convert.ToInt64(dtS.Rows[0]["offrole_schema_id"]));

                if (Convert.ToInt32(EmpType.SelectedValue) > 0)
                {
                    if (Convert.ToInt32(EmpType.SelectedValue) == 1 || Convert.ToInt32(EmpType.SelectedValue) == 2)
                    {
                        startNo = dtSchemaOn.Rows[0]["startno"].ToString();
                        paddCounter = Convert.ToInt32(dtSchemaOn.Rows[0]["digit"]);
                        paddedStr = startNo.PadLeft(paddCounter, '0');
                        prefCompCode = dtSchemaOn.Rows[0]["prefix"].ToString();
                        sufxCompCode = dtSchemaOn.Rows[0]["suffix"].ToString();
                    }
                    else
                    {
                        startNo = dtSchemaOff.Rows[0]["startno"].ToString();
                        paddCounter = Convert.ToInt32(dtSchemaOff.Rows[0]["digit"]);
                        prefCompCode = dtSchemaOff.Rows[0]["prefix"].ToString();
                        sufxCompCode = dtSchemaOff.Rows[0]["suffix"].ToString();
                    }
                    prefLen = Convert.ToInt32(prefCompCode.Length);
                    sufxLen = Convert.ToInt32(sufxCompCode.Length);
                    //Mantis Issue 24689
                    string srhVar = prefCompCode + "%";
                    //End of Mantis Issue 24689
                    string sql_statement = "SELECT max(tmc.Cnt_UCC) FROM tbl_master_contact tmc WHERE tmc.cnt_internalId IN(SELECT ttectc1.emp_cntId FROM tbl_trans_employeeCTC ttectc1 WHERE ttectc1.emp_Organization = " + cmp_Id + ") AND dbo.RegexMatch('";
                    if (prefCompCode.Length > 0) sql_statement += "[" + prefCompCode + "]{" + prefLen + "}"; //"^" + prefCompCode;
                    if (startNo.Length > 0) sql_statement += "[0-9]{" + paddCounter + "}";
                    if (sufxCompCode.Length > 0) sql_statement += "[" + sufxCompCode + "]{" + sufxLen + "}";//"^" + sufxCompCode;
                    //Mantis Issue 24689
                    //sql_statement += "?$', tmc.cnt_UCC) = 1 AND tmc.cnt_internalid like 'EM%'";
                    sql_statement += "?$', tmc.cnt_UCC) = 1 AND tmc.cnt_internalid like 'EM%' AND tmc.Cnt_UCC like '" + srhVar + "'";
                    //End of Mantis Issue 24689
                    dtC = oDBEngine.GetDataTable(sql_statement);

                    if (dtC.Rows[0][0].ToString() == "")
                    {
                        sql_statement = "SELECT max(tmc.Cnt_UCC) FROM tbl_master_contact tmc WHERE tmc.cnt_internalId IN(SELECT ttectc1.emp_cntId FROM tbl_trans_employeeCTC ttectc1 WHERE ttectc1.emp_Organization = " + cmp_Id + ") AND dbo.RegexMatch('";
                        if (prefCompCode.Length > 0) sql_statement += "[" + prefCompCode + "]{" + prefLen + "}"; //"^" + prefCompCode;
                        if (startNo.Length > 0) sql_statement += "[0-9]{" + (paddCounter - 1) + "}";
                        if (sufxCompCode.Length > 0) sql_statement += "[" + sufxCompCode + "]{" + sufxLen + "}";//"^" + sufxCompCode;
                        //Mantis Issue 24689
                        //sql_statement += "?$', tmc.cnt_UCC) = 1 AND tmc.cnt_internalid like 'EM%'";
                        sql_statement += "?$', tmc.cnt_UCC) = 1 AND tmc.cnt_internalid like 'EM%' AND tmc.Cnt_UCC like '" + srhVar + "'";
                        //End of Mantis Issue 24689
                        dtC = oDBEngine.GetDataTable(sql_statement);
                    }

                    if (dtC.Rows.Count > 0 && dtC.Rows[0][0].ToString() != "")
                    {
                        string uccCode = dtC.Rows[0][0].ToString();
                        int UCCLen = uccCode.Length;
                        int decimalPartLen = UCCLen - (prefCompCode.Length + sufxCompCode.Length);
                        string uccCodeSubstring = uccCode.Substring(prefCompCode.Length, decimalPartLen);
                        EmpCode = Convert.ToInt32(uccCodeSubstring) + 1;

                        if (EmpCode.ToString().Length > paddCounter)
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "General", "jAlert('Cannot Add More Employees as Schema Exausted for Current Employee Role. <br />Please Update Employee Schema.');", true);
                            return false;
                        }
                        else
                        {
                            paddedStr = EmpCode.ToString().PadLeft(paddCounter, '0');
                            empCompCode = prefCompCode + paddedStr + sufxCompCode;
                            return true;
                        }
                    }
                    else
                    {
                        paddedStr = startNo.PadLeft(paddCounter, '0');
                        empCompCode = prefCompCode + paddedStr + sufxCompCode;
                        return true;
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "JSct", "jAlert('Employee Type Not Found !);", true);
                    return false;
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "JSct55", "jAlert('Company On / Off Role Schema Not Defined !');", true);
                return false;
            }
        }






        protected void btnEmpID_Click(object sender, EventArgs e)
        {
            if (txtAliasName.Text.ToString() != "")
            {
                /*DataTable dt = oDBEngine.GetDataTable("tbl_master_contact", " cnt_ucc ", " cnt_internalid !='" + Session["KeyVal_InternalID"] + 
                    "' and (cnt_ucc like '" + txtAliasName.Text.ToString().Trim() + "' or cnt_ShortName like  '" + txtAliasName.Text.ToString().Trim() + "')");*/

                DataTable dt = oDBEngine.GetDataTable("SELECT cnt_id FROM tbl_master_contact WHERE cnt_UCC = '" + txtAliasName.Text.ToString().Trim() +
                    "' AND cnt_internalId != '" + Session["KeyVal_InternalID"] + "' AND cnt_internalId IN(SELECT emp_cntId FROM tbl_trans_employeeCTC " +
                    " WHERE emp_Organization IN (SELECT tte.emp_Organization FROM tbl_trans_employeeCTC tte WHERE emp_cntId = '" + Session["KeyVal_InternalID"] + "'))");

                if (dt.Rows.Count > 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert12", "jAlert('Employee Code already Exists!..');", true);
                }
                else
                {
                    oDBEngine.SetFieldValue("tbl_master_employee", "emp_uniquecode='" + txtAliasName.Text.ToString() + "'", "emp_contactID='" + Session["KeyVal_InternalID"] + "'");
                    oDBEngine.SetFieldValue("tbl_master_contact", "cnt_ucc='" + txtAliasName.Text.ToString() + "',cnt_ShortName='" + txtAliasName.Text.ToString() + "'", "cnt_internalid='" + Session["KeyVal_InternalID"] + "'");
                    DataTable dtchk = oDBEngine.GetDataTable("tbl_master_contact", " cnt_id ", "cnt_internalid='" + Session["KeyVal_InternalID"] + "'");
                    ScriptManager.RegisterStartupScript(this, GetType(), "JScript", "window.location ='employee_general.aspx?id=" + Convert.ToInt32(dtchk.Rows[0][0].ToString()) + "';", true);
                }

            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "General", "jAlert('Employee ID can not be blank');", true);
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "CLOSE122", "editwin.close();", true);
            }
        }

        private void ShowForm()
        {
            /*Rev Work Start 22.04.2022 MantiseID:0024850: Copy feature add in Employee master*/
            if (Request.QueryString["id"] == "ADD")
            {
                /*Rev Work Close 22.04.2022*/
                cmbDOJ.EditFormatString = Oconverter.GetDateFormat();
                string[,] Data = oDBEngine.GetFieldValue("tbl_master_salutation", "sal_id, sal_name", null, 2, "sal_name");
                //oDBEngine.AddDataToDropDownList(Data, CmbSalutation);
                oclsDropDownList.AddDataToDropDownList(Data, CmbSalutation);
                CmbSalutation.SelectedValue = "1";
                Data = oDBEngine.GetFieldValue("tbl_master_Company", "cmp_id, cmp_name", null, 2, "cmp_name");
                //oDBEngine.AddDataToDropDownList(Data, cmbOrganization);
                oclsDropDownList.AddDataToDropDownList(Data, cmbOrganization);
                Data = oDBEngine.GetFieldValue("tbl_master_jobresponsibility", "job_id, job_responsibility", null, 2, "job_responsibility");
                //oDBEngine.AddDataToDropDownList(Data, cmbJobResponse);
                oclsDropDownList.AddDataToDropDownList(Data, cmbJobResponse);
                Data = oDBEngine.GetFieldValue("tbl_master_branch", "branch_id, branch_description ", null, 2, "branch_description");
                //oDBEngine.AddDataToDropDownList(Data, cmbBranch);
                oclsDropDownList.AddDataToDropDownList(Data, cmbBranch);
                Data = oDBEngine.GetFieldValue("tbl_master_Designation", "deg_id, deg_designation ", null, 2, "deg_designation");
                //oDBEngine.AddDataToDropDownList(Data, cmbDesg);
                oclsDropDownList.AddDataToDropDownList(Data, cmbDesg);
                Data = oDBEngine.GetFieldValue("tbl_master_employeeType", "emptpy_id, emptpy_type ", null, 2, "emptpy_type");
                //oDBEngine.AddDataToDropDownList(Data, EmpType);
                oclsDropDownList.AddDataToDropDownList(Data, EmpType);
                Data = oDBEngine.GetFieldValue("tbl_master_costCenter", "cost_id, cost_description ", " cost_costCenterType = 'department' ", 2, "cost_description");
                //oDBEngine.AddDataToDropDownList(Data, cmbDept);
                oclsDropDownList.AddDataToDropDownList(Data, cmbDept);
                Data = oDBEngine.GetFieldValue("tbl_EmpWorkingHours", "Id,Name  ", null, 2, "Name");
                //oDBEngine.AddDataToDropDownList(Data, cmbWorkingHr);
                oclsDropDownList.AddDataToDropDownList(Data, cmbWorkingHr);
                Data = oDBEngine.GetFieldValue("tbl_master_LeaveScheme", "ls_id, ls_name  ", null, 2, "ls_name");
                //oDBEngine.AddDataToDropDownList(Data, cmbLeaveP);
                oclsDropDownList.AddDataToDropDownList(Data, cmbLeaveP);


                cmbOrganization.Items.Insert(0, new ListItem("--Select--", "0"));
                cmbJobResponse.Items.Insert(0, new ListItem("--Select--", "0"));
                cmbBranch.Items.Insert(0, new ListItem("--Select--", "0"));
                cmbDesg.Items.Insert(0, new ListItem("--Select--", "0"));
                EmpType.Items.Insert(0, new ListItem("--Select--", "0"));
                cmbDept.Items.Insert(0, new ListItem("--Select--", "0"));
                cmbWorkingHr.Items.Insert(0, new ListItem("--Select--", "0"));
                cmbLeaveP.Items.Insert(0, new ListItem("--Select--", "0"));
                /*Rev Work Start 22.04.2022 MantiseID:0024850: Copy feature add in Employee master*/
            }
            else
            {
               
                if (Request.QueryString["id"] != null && Convert.ToString(Request.QueryString["id"]) != "")
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
                if (InternalId[0, 0].Trim().StartsWith("EM"))
                {
                    // txtAliasName.Enabled = false;
                    LinkButton1.Enabled = false;
                }
                else
                {
                    // txtAliasName.Enabled = true;
                    LinkButton1.Enabled = true;
                }
                HttpContext.Current.Session["KeyVal_InternalID"] = InternalId[0, 0];
                //LanGuage();
                string[,] ContactData;
                string[,] TDSTYpe;
                string[,] IsDirector;               
                string[,] CTCData;
                string[,] EmpData;
                // if emp code is  system generated txtAliasName should be un editable #ag18102016
                if (ID != 0)
                {
                    ContactData = oDBEngine.GetFieldValue("tbl_master_contact",
                                            "cnt_ucc, cnt_salutation,  cnt_firstName, cnt_middleName, cnt_lastName, cnt_shortName, cnt_branchId, cnt_sex, cnt_maritalStatus, cnt_DOB, cnt_anniversaryDate, cnt_legalStatus, cnt_education, cnt_profession, cnt_organization, cnt_jobResponsibility, cnt_designation, cnt_industry, cnt_contactSource, cnt_referedBy, cnt_contactType, cnt_contactStatus,cnt_bloodgroup,WebLogIn,isnull(TDSRATE_TYPE,0) TDSRATE_TYPE",
                                            " cnt_id=" + ID, 24);
                    TDSTYpe = oDBEngine.GetFieldValue("tbl_master_contact", "isnull(TDSRATE_TYPE,0) TDSRATE_TYPE", "cnt_id=" + ID, 1);

                    IsDirector = oDBEngine.GetFieldValue("tbl_master_contact", "Is_Director", "cnt_id=" + ID, 1);

                    CTCData = oDBEngine.GetFieldValue("tbl_trans_employeeCTC",
                                           "emp_Organization, emp_JobResponsibility,  emp_Designation, emp_Department, emp_workinghours, emp_totalLeavePA ,emp_type,emp_reportTo",
                                           " emp_cntId='" + HttpContext.Current.Session["KeyVal_InternalID"] + "'", 8);

                    EmpData = oDBEngine.GetFieldValue("tbl_trans_employeeCTC A LEFT JOIN tbl_master_designation ON A.emp_Designation = tbl_master_designation.deg_id LEFT JOIN tbl_master_costCenter ON A.emp_Department = tbl_master_costCenter.cost_id LEFT JOIN tbl_master_company ON A.emp_Organization = tbl_master_company.cmp_id  LEFT JOIN tbl_master_employee ON A.emp_cntId = tbl_master_employee.emp_contactId ",
                               "A.emp_effectiveDate AS EffectiveDate,convert(varchar(11),A.emp_effectiveDate,113) AS EffectiveDate1,convert(varchar(11),A.emp_effectiveuntil,113) AS EffectiveUntil1, A.emp_effectiveuntil AS EffectiveUntil, A.emp_id AS Id, tbl_master_designation.deg_designation AS Designation, tbl_master_costCenter.cost_description AS Department, tbl_master_company.cmp_Name AS CompanyName, A.emp_id, A.emp_cntId, A.emp_effectiveDate as emp_dateofJoining, A.emp_effectiveuntil, A.emp_Organization, A.emp_JobResponsibility, A.emp_Designation, (select branch_description from tbl_master_branch where branch_id= A.emp_branch) as BranchName,	A.emp_branch,A.emp_LeaveSchemeAppliedFrom,A.emp_type, A.emp_Department, A.emp_reportTo, A.emp_deputy, A.emp_colleague, A.emp_workinghours, A.emp_currentCTC, A.emp_basic, A.emp_HRA, A.emp_CCA, A.emp_spAllowance, A.emp_childrenAllowance, A.emp_totalLeavePA, A.emp_PF, A.emp_medicalAllowance, A.emp_LTA, A.emp_convence, A.emp_mobilePhoneExp,A.EMP_CarAllowance, A.EMP_UniformAllowance,A.EMP_BooksPeriodicals ,A.EMP_SeminarAllowance ,A.EMP_OtherAllowance,A.emp_totalMedicalLeavePA, A.CreateDate, A.CreateUser, A.LastModifyDate, A.LastModifyUser,A.emp_Remarks",
                               " (A.emp_cntId = '" + Convert.ToString(HttpContext.Current.Session["KeyVal_InternalID"]) + "') ", 47);
                }
                else
                {
                    ContactData = oDBEngine.GetFieldValue("tbl_master_contact",
                                            "cnt_ucc, cnt_salutation,  cnt_firstName, cnt_middleName, cnt_lastName, cnt_shortName, cnt_branchId, cnt_sex, cnt_maritalStatus,cnt_DOB, cnt_anniversaryDate, cnt_legalStatus, cnt_education, cnt_profession, cnt_organization, cnt_jobResponsibility, cnt_designation, cnt_industry, cnt_contactSource, cnt_referedBy, cnt_contactType, cnt_contactStatus,cnt_bloodgroup,WebLogIn,isnull(TDSRATE_TYPE,0) TDSRATE_TYPE",
                                            " cnt_id=" + HttpContext.Current.Session["KeyVal"], 24);
                    TDSTYpe = oDBEngine.GetFieldValue("tbl_master_contact", "isnull(TDSRATE_TYPE,0) TDSRATE_TYPE", "cnt_id=" + HttpContext.Current.Session["KeyVal"], 1);
                    IsDirector = oDBEngine.GetFieldValue("tbl_master_contact", "Is_Director", "cnt_id=" + ID, 1);

                    CTCData = oDBEngine.GetFieldValue("tbl_trans_employeeCTC",
                                            "emp_Organization, emp_JobResponsibility,  emp_Designation, emp_Department, emp_workinghours, emp_totalLeavePA,emp_type,emp_reportTo",
                                            " emp_cntId='" + HttpContext.Current.Session["KeyVal_InternalID"] + "'",8);
                    EmpData = oDBEngine.GetFieldValue("tbl_trans_employeeCTC A LEFT JOIN tbl_master_designation ON A.emp_Designation = tbl_master_designation.deg_id LEFT JOIN tbl_master_costCenter ON A.emp_Department = tbl_master_costCenter.cost_id LEFT JOIN tbl_master_company ON A.emp_Organization = tbl_master_company.cmp_id  LEFT JOIN tbl_master_employee ON A.emp_cntId = tbl_master_employee.emp_contactId ",
                               "A.emp_effectiveDate AS EffectiveDate,convert(varchar(11),A.emp_effectiveDate,113) AS EffectiveDate1,convert(varchar(11),A.emp_effectiveuntil,113) AS EffectiveUntil1, A.emp_effectiveuntil AS EffectiveUntil, A.emp_id AS Id, tbl_master_designation.deg_designation AS Designation, tbl_master_costCenter.cost_description AS Department, tbl_master_company.cmp_Name AS CompanyName, A.emp_id, A.emp_cntId, A.emp_effectiveDate as emp_dateofJoining, A.emp_effectiveuntil, A.emp_Organization, A.emp_JobResponsibility, A.emp_Designation, (select branch_description from tbl_master_branch where branch_id= A.emp_branch) as BranchName,	A.emp_branch,A.emp_LeaveSchemeAppliedFrom,A.emp_type, A.emp_Department, A.emp_reportTo, A.emp_deputy, A.emp_colleague, A.emp_workinghours, A.emp_currentCTC, A.emp_basic, A.emp_HRA, A.emp_CCA, A.emp_spAllowance, A.emp_childrenAllowance, A.emp_totalLeavePA, A.emp_PF, A.emp_medicalAllowance, A.emp_LTA, A.emp_convence, A.emp_mobilePhoneExp,A.EMP_CarAllowance, A.EMP_UniformAllowance,A.EMP_BooksPeriodicals ,A.EMP_SeminarAllowance ,A.EMP_OtherAllowance,A.emp_totalMedicalLeavePA, A.CreateDate, A.CreateUser, A.LastModifyDate, A.LastModifyUser,A.emp_Remarks",
                               " (A.emp_cntId = '" + Convert.ToString(HttpContext.Current.Session["KeyVal_InternalID"]) + "') ", 47);
                }

                if (TDSTYpe[0, 0] != "")
                {
                    cmbTDS.Value = Convert.ToString(TDSTYpe[0, 0]);
                }
                if (IsDirector[0, 0] != "n")
                {

                    if (Convert.ToBoolean(IsDirector[0, 0]))
                    {
                        chkIsDirector.Checked = true;
                    }
                    else
                    {
                        chkIsDirector.Checked = false;
                    }
                }
                //____________ Value BINDING and Allocation _______________//

                //txtClentUcc.Text = ContactData[0, 0];
                string[,] Data = oDBEngine.GetFieldValue("tbl_master_salutation", "sal_id, sal_name", null, 2);
                string[,] OrgData = oDBEngine.GetFieldValue("tbl_master_Company", "cmp_id, cmp_Name", null, 2);
                string[,] JobData = oDBEngine.GetFieldValue("tbl_master_jobresponsibility", "job_id, job_responsibility", null, 2, "job_responsibility");
                string[,] DesigData = oDBEngine.GetFieldValue("tbl_master_Designation", "deg_id, deg_designation ", null, 2, "deg_designation");
                string[,] DeptData = oDBEngine.GetFieldValue("tbl_master_costCenter", "cost_id, cost_description ", " cost_costCenterType = 'department' ", 2, "cost_description");
                string[,] WorkHrsData = oDBEngine.GetFieldValue("tbl_EmpWorkingHours", "Id,Name  ", null, 2, "Name");
                string[,] LeaveData = oDBEngine.GetFieldValue("tbl_master_LeaveScheme", "ls_id, ls_name  ", null, 2, "ls_name");
                string[,] EmpTypeData = oDBEngine.GetFieldValue("tbl_master_employeeType", "emptpy_id, emptpy_type ", null, 2, "emptpy_type");
                string[,] ReportToData = oDBEngine.GetFieldValue(" tbl_master_contact", " (ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') + ' [' + cnt_shortName +']') AS cnt_firstName ", " cnt_internalid='" + HttpContext.Current.Session["KeyVal_InternalID"] + "'", 1);
                string[,] DOJ;
                DOJ = oDBEngine.GetFieldValue("Tbl_Master_Employee", "Emp_DateOfJoining", " emp_contactId='" + Convert.ToString(HttpContext.Current.Session["KeyVal_InternalID"]) + "'", 1);

                //if (ContactData[0, 19] != "")
                //{
                //    ddlReferedBy.SelectedIndex = ddlReferedBy.Items.IndexOf(ddlReferedBy.Items.FindByValue(ContactData[0, 19].Trim()));
                //}
                if (ContactData[0, 1] != "")
                {
                    // oDBEngine.AddDataToDropDownList(Data, CmbSalutation, Int32.Parse(ContactData[0, 1]));
                    OclsDropDownList.AddDataToDropDownList(Data, CmbSalutation, Int32.Parse(ContactData[0, 1]));
                }
                else
                {
                    //oDBEngine.AddDataToDropDownList(Data, CmbSalutation, 0);
                    OclsDropDownList.AddDataToDropDownList(Data, CmbSalutation, 0);
                }
                txtFirstNmae.Text = ContactData[0, 2];
                txtMiddleName.Text = ContactData[0, 3];
                txtLastName.Text = ContactData[0, 4];
               // txtAliasName.Text = ContactData[0, 5];
               // cmbBloodgroup.SelectedValue = ContactData[0, 22];
                Data = oDBEngine.GetFieldValue("tbl_master_branch", "branch_id, branch_description ", null, 2);
                if (ContactData[0, 6] != "")
                {
                    //oDBEngine.AddDataToDropDownList(Data, cmbBranch, Int32.Parse(ContactData[0, 6]));
                    OclsDropDownList.AddDataToDropDownList(Data, cmbBranch, Int32.Parse(ContactData[0, 6]));
                }
                else
                {
                    //oDBEngine.AddDataToDropDownList(Data, cmbBranch, 0);
                    OclsDropDownList.AddDataToDropDownList(Data, cmbBranch, 0);
                }
                if (ContactData[0, 7] != "")
                {
                    cmbGender.SelectedValue = ContactData[0, 7];
                }
                else
                {
                    cmbGender.SelectedIndex.Equals(0);
                }
                if (CTCData[0, 0] != "")
                {
                    // oDBEngine.AddDataToDropDownList(Data, CmbSalutation, Int32.Parse(ContactData[0, 1]));
                    oclsDropDownList.AddDataToDropDownList(OrgData, cmbOrganization, Int32.Parse(CTCData[0, 0]));
                }
                else
                {
                    //oDBEngine.AddDataToDropDownList(Data, CmbSalutation, 0);
                    oclsDropDownList.AddDataToDropDownList(OrgData, cmbOrganization, 0);
                }
                if (CTCData[0, 1] != "")
                {
                    oclsDropDownList.AddDataToDropDownList(JobData, cmbJobResponse, Int32.Parse(CTCData[0, 1]));
                }
                else
                {
                    oclsDropDownList.AddDataToDropDownList(JobData, cmbJobResponse, 0);
                }
                if (CTCData[0, 2] != "")
                {
                    oclsDropDownList.AddDataToDropDownList(DesigData, cmbDesg, Int32.Parse(CTCData[0, 2]));
                }
                else
                {
                    oclsDropDownList.AddDataToDropDownList(DesigData, cmbDesg, 0);
                }
                if (CTCData[0, 3] != "")
                {
                    oclsDropDownList.AddDataToDropDownList(DeptData, cmbDept, Int32.Parse(CTCData[0, 3]));
                }
                else
                {
                    oclsDropDownList.AddDataToDropDownList(DeptData, cmbDept, 0);
                }
                if (CTCData[0, 4] != "")
                {
                    oclsDropDownList.AddDataToDropDownList(WorkHrsData, cmbWorkingHr, Int32.Parse(CTCData[0, 4]));
                }
                else
                {
                    oclsDropDownList.AddDataToDropDownList(WorkHrsData, cmbWorkingHr, 0);
                }
                if (CTCData[0, 5] != "")
                {
                    oclsDropDownList.AddDataToDropDownList(LeaveData, cmbLeaveP, Int32.Parse(CTCData[0, 5]));
                }
                else
                {
                    oclsDropDownList.AddDataToDropDownList(LeaveData, cmbLeaveP, 0);
                }
                if (CTCData[0, 6] != "")
                {
                    oclsDropDownList.AddDataToDropDownList(EmpTypeData, EmpType, Int32.Parse(CTCData[0, 6]));
                }
                else
                {
                    oclsDropDownList.AddDataToDropDownList(EmpTypeData, EmpType, 0);
                }
                if (CTCData[0, 7] != "")
                {
                    ddlReportTo.SelectedIndex = ddlReportTo.Items.IndexOf(ddlReportTo.Items.FindByValue(CTCData[0, 7].Trim()));
                }
                
                if (Data[0, 0] != "n")
                {
                   txtReportTo_hidden.Value = CTCData[0, 7].ToString();
                }
                if (EmpData[0, 10] != "" && EmpData[0, 10] != "01/01/1900" && EmpData[0, 10] != "1/1/1900 12:00:00 AM")
                {
                    cmbDOJ.Value = Convert.ToDateTime(EmpData[0, 10]);
                }
                else
                    cmbDOJ.Value = null;
                if (EmpData[0, 17] != "" && EmpData[0, 17] != "01/01/1900" && EmpData[0, 17] != "1/1/1900 12:00:00 AM")
                {
                    cmbLeaveEff.Value = Convert.ToDateTime(EmpData[0, 17]);                  
                }
                else
                    cmbLeaveEff.Value = null;
                string[,] DataRT = oDBEngine.GetFieldValue(" tbl_master_employee, tbl_master_contact", " ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') +'['+cnt_shortName+']' AS Name ", " tbl_master_employee.emp_contactId = tbl_master_contact.cnt_internalId and  tbl_master_employee.emp_id ='" + EmpData[0, 20].ToString() + "'", 1);
                if (DataRT[0, 0] != "n")
                {
                    ddlReportTo.SelectedIndex = ddlReportTo.Items.IndexOf(ddlReportTo.Items.FindByValue(EmpData[0, 20].Trim()));
                    txtReportTo_hidden.Value = EmpData[0, 20].ToString();
                    lstReportTo.Text = EmpData[0, 20].ToString();
                }
                else
                {
                    lstReportTo.Text = EmpData[0, 20].ToString();
                }                
            }
            /*Rev Work Close 22.04.2022 MantiseID:0024850: Copy feature add in Employee master*/
        }       
        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            //DataTable dtV = new DataTable();
            DataTable dtS = new DataTable();
            DataTable dtB = new DataTable();
            DataTable dtC = new DataTable();
            string CompCode = string.Empty;
            int EmpCode;
            String ShortName = string.Empty;
            string TempCode = string.Empty;
            DataTable dtCTC = new DataTable();
            if (Request.QueryString["id"] == "ADD")
            {
                dtCTC = oDBEngine.GetDataTable("tbl_trans_employeeCTC", " *", "emp_cntID='" + CntID + "'");
            }
            else
            {
                dtCTC = oDBEngine.GetDataTable("tbl_trans_employeeCTC", " *", "emp_cntID='" + HttpContext.Current.Session["KeyVal_InternalID"] + "'");

            }
            if (dtCTC.Rows.Count > 0)
            {
                if (dtCTC.Rows[0]["emp_Organization"].ToString().Length != 0 || dtCTC.Rows[0]["emp_branch"].ToString().Length != 0)
                {
                    dtS = oDBEngine.GetDataTable("tbl_master_company", "cmp_OffRoleShortName,cmp_OnRoleShortName", "cmp_id=" + dtCTC.Rows[0]["emp_Organization"] + "");
                    dtB = oDBEngine.GetDataTable("tbl_master_branch", "branch_Code", "branch_id=" + dtCTC.Rows[0]["emp_branch"] + "");
                    if (dtB.Rows.Count > 0)
                    {
                        if (dtS.Rows.Count > 0)
                        {
                            if (dtCTC.Rows[0]["emp_type"].ToString().Length != 0)
                            {
                                if (dtCTC.Rows[0]["emp_type"].ToString() == "1")
                                {
                                    //CompCode = dtS.Rows[0]["cmp_OnRoleShortName"].ToString() + dtB.Rows[0]["branch_Code"].ToString();
                                    CompCode = dtS.Rows[0]["cmp_OnRoleShortName"].ToString();

                                }
                                else
                                {
                                    //CompCode = dtS.Rows[0]["cmp_OffRoleShortName"].ToString() + dtB.Rows[0]["branch_Code"].ToString();
                                    CompCode = dtS.Rows[0]["cmp_OffRoleShortName"].ToString();

                                }

                                if (CompCode.ToString().Length > 0)
                                {
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
                                    txtAliasName.Text = CompCode.ToString();
                                }
                                else
                                {
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "JSct55", "jAlert('Please Add Employee CTC details first.');", true);

                                }

                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "JSct", "jAlert('Employee Type Not Found!);return false;", true);

                            }

                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "JSct43", "jAlert('Company short name not found.');", true);

                        }
                    }
                    else
                    {

                        ScriptManager.RegisterStartupScript(this, this.GetType(), "JSct44", "jAlert('Branch code not found.');", true);

                    }




                }
            }
            else
            {

                ScriptManager.RegisterStartupScript(this, this.GetType(), "JSct3", "jAlert('Please Add Employee CTC details first.');", true);
            }


        }



        [WebMethod]
        public static List<string> BindContactType(string reqID)
        {
            Employee_AddNew_BL objEmployee_AddNew_BL = new Employee_AddNew_BL();
            DataTable ContactDt = objEmployee_AddNew_BL.GetContactTypeonEmployeeType(reqID);

            List<string> obj = new List<string>();
            foreach (DataRow dr in ContactDt.Rows)
            {
                obj.Add(Convert.ToString(dr["Name"]) + "|" + Convert.ToString(dr["ID"]));
            }
            return obj;
            //    Response.Write("No Record Found###No Record Found|");


        }
    }
}