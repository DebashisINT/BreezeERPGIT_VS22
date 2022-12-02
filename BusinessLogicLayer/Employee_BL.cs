using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAccessLayer;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using BusinessLogicLayer;
using System.Reflection;
using System.Globalization;

namespace BusinessLogicLayer
{

    public class Employee_BL
    {
        string actual = "";


        public int btnCTC_Click_BL(string emp_cntId, string emp_dateofJoining,
       string emp_Organization, string emp_JobResponsibility, string emp_Designation, string emp_type,
       string emp_Department, string emp_reportTo, string emp_deputy, string emp_colleague,
       string emp_workinghours, string emp_totalLeavePA, string emp_LeaveSchemeAppliedFrom, string emp_branch, string emp_Remarks)
        {
            ProcedureExecute proc;
            try
            {
                using (proc = new ProcedureExecute("EmployeeCTCInsert"))
                {
                    proc.AddNVarcharPara("@emp_cntId", 100, emp_cntId);
                    proc.AddNVarcharPara("@emp_dateofJoining", 100, emp_dateofJoining);
                    proc.AddIntegerPara("@emp_Organization", Convert.ToInt32(emp_Organization));
                    proc.AddIntegerPara("@emp_JobResponsibility", Convert.ToInt32(emp_JobResponsibility));
                    proc.AddIntegerPara("@emp_Designation", Convert.ToInt32(emp_Designation));
                    proc.AddIntegerPara("@emp_type", Convert.ToInt32(emp_type));

                    proc.AddIntegerPara("@emp_Department", Convert.ToInt32(emp_Department));
                    proc.AddIntegerPara("@emp_reportTo", Convert.ToInt32(emp_reportTo));
                    proc.AddIntegerPara("@emp_deputy", Convert.ToInt32(emp_deputy));
                    proc.AddIntegerPara("@emp_colleague", Convert.ToInt32(emp_colleague));
                    proc.AddIntegerPara("@emp_workinghours", Convert.ToInt32(emp_workinghours));
                    proc.AddDecimalPara("@emp_currentCTC", 0, 18, 0);
                    proc.AddDecimalPara("@emp_basic", 0, 18, 0);
                    proc.AddDecimalPara("@emp_HRA", 0, 18, 0);
                    proc.AddDecimalPara("@emp_CCA", 0, 18, 0);
                    proc.AddDecimalPara("@emp_spAllowance", 0, 18, 0);
                    proc.AddDecimalPara("@emp_childrenAllowance", 0, 18, 0);
                    proc.AddNVarcharPara("@emp_totalLeavePA", 100, emp_totalLeavePA);

                    proc.AddDecimalPara("@emp_PF", 0, 18, 0);
                    proc.AddDecimalPara("@emp_medicalAllowance", 0, 18, 0);
                    proc.AddDecimalPara("@emp_LTA", 0, 18, 0);
                    proc.AddDecimalPara("@emp_convence", 0, 18, 0);
                    proc.AddDecimalPara("@emp_mobilePhoneExp", 0, 18, 0);
                    //proc.AddNVarcharPara("@emp_totalLeavePA", 100, emp_totalLeavePA);
                    proc.AddNVarcharPara("@emp_totalMedicalLeavePA", 100, "");

                    proc.AddIntegerPara("@userid", Convert.ToInt32(HttpContext.Current.Session["userid"]));
                    proc.AddNVarcharPara("@emp_LeaveSchemeAppliedFrom", 100, emp_LeaveSchemeAppliedFrom);
                    proc.AddIntegerPara("@emp_branch", Convert.ToInt32(emp_branch));
                    proc.AddNVarcharPara("@emp_Remarks", 100, emp_Remarks);

                    proc.AddDecimalPara("@EMP_CarAllowance", 0, 18, 0);
                    proc.AddDecimalPara("@EMP_UniformAllowance", 0, 18, 0);
                    proc.AddDecimalPara("@EMP_BooksPeriodicals", 0, 18, 0);
                    proc.AddDecimalPara("@EMP_SeminarAllowance", 0, 18, 0);
                    proc.AddDecimalPara("@EMP_OtherAllowance", 0, 18, 0);


                    //return
                    int NoOfRowEffected = proc.RunActionQuery();
                    return NoOfRowEffected;
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }

            finally
            {
                proc = null;
            }
        }

        public string btnSave_Click_BL(string cnt_ucc, string cnt_salutation, string cnt_firstName,
       string cnt_middleName, string cnt_lastName, string cnt_shortName, string cnt_branchId,
       string cnt_sex, string cnt_maritalStatus, string DOBDate, string AnniversaryDate,
       string cmbLegalStatus, string cmbEducation, string cmbSource, string referedID, string cmbBloodgroup,
       bool chkAllow, string password, bool Usehierchy, bool chkIsDirector, Int64 TDSType = 0)
        {
            ProcedureExecute proc;

            try
            {
                using (proc = new ProcedureExecute("EmployeeInsert"))
                {
                    proc.AddVarcharPara("@cnt_ucc", 100, cnt_ucc);

                    proc.AddVarcharPara("@cnt_salutation", 100, cnt_salutation);

                    proc.AddVarcharPara("@cnt_firstName", 100, cnt_firstName);

                    proc.AddVarcharPara("@cnt_middleName", 100, cnt_middleName);

                    proc.AddVarcharPara("@cnt_lastName", 100, cnt_lastName);

                    proc.AddVarcharPara("@cnt_shortName", 100, cnt_shortName);

                    proc.AddVarcharPara("@cnt_branchId", 100, cnt_branchId);

                    proc.AddVarcharPara("@cnt_sex", 100, cnt_sex);

                    proc.AddVarcharPara("@cnt_maritalStatus", 100, cnt_maritalStatus);
                    proc.AddBooleanPara("@Usehierchy", Usehierchy);

                    if (DOBDate != null)
                    {
                        proc.AddVarcharPara("@cnt_DOB", 100, DOBDate);
                    }
                    else
                    {
                        proc.AddVarcharPara("@cnt_DOB", 100, "");
                    }
                    if (AnniversaryDate != null)
                    {
                        proc.AddVarcharPara("@cnt_anniversaryDate", 100, AnniversaryDate);
                    }
                    else
                    {
                        proc.AddVarcharPara("@cnt_anniversaryDate", 100, "");
                    }
                    proc.AddVarcharPara("@cnt_legalStatus", 100, cmbLegalStatus);
                    proc.AddVarcharPara("@cnt_education", 100, cmbEducation);
                    proc.AddVarcharPara("@cnt_contactSource", 100, cmbSource);
                    proc.AddVarcharPara("@cnt_referedBy", 100, referedID);
                    proc.AddVarcharPara("@cnt_contactType", 100, "EM");
                    proc.AddBigIntegerPara("@TDSType", TDSType);
                    proc.AddVarcharPara("@lastModifyUser", 100, Convert.ToString(HttpContext.Current.Session["userid"]));
                    proc.AddVarcharPara("@UserContactID", 100, Convert.ToString(HttpContext.Current.Session["usercontactID"]));
                    proc.AddVarcharPara("@bloodgroup", 100, cmbBloodgroup);
                    proc.AddBooleanPara("@IsDirector", chkIsDirector);
                    string webLogin = "No";
                    string Password = string.Empty;
                    if (chkAllow == true)
                    {
                        webLogin = "Yes";
                        Password = password;
                    }
                    proc.AddVarcharPara("@webLogin", 100, webLogin);
                    proc.AddVarcharPara("@Password", 100, Password);
                    proc.AddVarcharPara("@result", 100, "", QueryParameterDirection.Output);
                    int NoOfRowEffected = proc.RunActionQuery();
                    //string RetVal = Convert.ToString(proc.GetParaValue("@RetVal"));
                    string RetVal = Convert.ToString(proc.GetParaValue("@result"));
                    return RetVal;
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }

            finally
            {
                proc = null;
            }
        }

        public DataTable GetAssignedEmployeeDetailByReportingTo(string cnt_internalId)
        {

            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_EmployeeAllDtl");
            proc.AddNVarcharPara("@action", 150, "GetAssignedEmployeeDetailByReportingTo");
            proc.AddNVarcharPara("@cnt_internalId", 50, cnt_internalId);
            dt = proc.GetTable();
            return dt;
        }

        public DataTable GetNewSupervisorList(string cnt_internalId, string activityid, string supervisorId, string assignedToId)
        {

            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_EmployeeAllDtl");
            proc.AddNVarcharPara("@action", 150, "GetNewSupervisorList");
            proc.AddNVarcharPara("@cnt_internalId", 50, cnt_internalId);
            proc.AddNVarcharPara("@activityid", 50, activityid);
            proc.AddNVarcharPara("@supervisorId", 50, supervisorId);
            proc.AddNVarcharPara("@assignedToId", 50, assignedToId);
            dt = proc.GetTable();
            return dt;
        }

        public DataTable GetReassignEmployeeDetailByReportingTo(string cnt_internalId)
        {

            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_EmployeeAllDtl");
            proc.AddNVarcharPara("@action", 150, "GetReassignEmployeeDetailByReportingTo");
            proc.AddNVarcharPara("@cnt_internalId", 50, cnt_internalId);
            dt = proc.GetTable();
            return dt;
        }


        public DataTable GetReassignEmployee(string cnt_internalId)
        {

            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_EmployeeAllDtl");
            proc.AddNVarcharPara("@action", 150, "GetAssignedEmployeeDetailByReportingTo");
            proc.AddNVarcharPara("@cnt_internalId", 50, cnt_internalId);

            dt = proc.GetTable();
            return dt;
        }


        public DataTable GetReassignUser(string cnt_internalId, string UserId)
        {

            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_EmployeeAllDtl");
            proc.AddNVarcharPara("@action", 150, "GetReassignUser");
            proc.AddNVarcharPara("@cnt_internalId", 50, cnt_internalId);
            proc.AddNVarcharPara("@assignedToId", 50, UserId);
            dt = proc.GetTable();
            return dt;
        }

        //aDDED BY:SUBHABRATA
        public DataTable GetEmailAccountConfigDetails(string Create_User, int action)
        {

            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("Proc_GetEmailSetUpDetails");
            proc.AddNVarcharPara("@CreateUser", 150, Create_User);
            proc.AddIntegerPara("@Action", action);
            dt = proc.GetTable();
            return dt;
        }
        //aDDED BY:Kaushik
        public DataTable GetEmailAccountSupervisorSalesmanConfigDetails(string Create_User, int action, string sls_ID)
        {

            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("Proc_GetEmailSetUpDetails");
            proc.AddNVarcharPara("@CreateUser", 150, Create_User);
            proc.AddIntegerPara("@Action", action);

            proc.AddDecimalPara("@sls_ID", 2, 18, Convert.ToDecimal(sls_ID));
            dt = proc.GetTable();
            return dt;
        }

        //aDDED BY:SUBHABRATA
        public DataTable GetAllLevelUsers(string Create_User, int action)
        {

            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("Proc_GetEmailSetUpDetails");
            proc.AddNVarcharPara("@CreateUser", 150, Create_User);
            proc.AddIntegerPara("@Action", action);
            dt = proc.GetTable();
            return dt;
        }


        public DataTable GetEmailInvoice(string Create_User, int action)
        {

            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("Proc_GetEmailSetUpDetails");
            proc.AddNVarcharPara("@InternalID", 150, Create_User);
            proc.AddIntegerPara("@Action", action);
            dt = proc.GetTable();
            return dt;
        }
        public DataTable GetEmailLevelUsersWise(int Create_User, int action, int UserLevel)
        {

            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("Proc_GetEmailSetUpDetails");
            proc.AddIntegerPara("@CreateUser", Create_User);
            proc.AddIntegerPara("@Action", action);
            proc.AddIntegerPara("@User_Level", UserLevel);
            dt = proc.GetTable();
            return dt;
        }

        //Added By:Subhabrata :21-0-2017 Saturday



        public bool AddEmailTemplateDefaultDetails(string Create_User)
        {
            bool IsUpdate = false;
            try
            {

                ProcedureExecute proc = new ProcedureExecute("Prc_EmailTemplate_IsDefault_Chk");
                proc.AddNVarcharPara("@SenderType", 150, Create_User);
                proc.GetScalar();
                IsUpdate = true;
            }
            catch
            {
                IsUpdate = false;
            }
            return IsUpdate;
        }
        public bool AddCustVendHistory(string GSTIn, int CNT_id, DateTime ApplicableFrom, string CreatedBy, string Action)
        {
            bool IsUpdate = false;
            try
            {
                //DateTime temp = DateTime.ParseExact(ApplicableFrom, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                string str = ApplicableFrom.ToString("yyyy-MM-dd");

                ProcedureExecute proc = new ProcedureExecute("Prc_SaveCustVendHistory");
                proc.AddNVarcharPara("@NewGSTIN_ID", 150, GSTIn);
                proc.AddIntegerPara("@Cnt_Id", CNT_id);
                proc.AddPara("@ApplicableFrom", str);
                proc.AddNVarcharPara("@CreateBy", 150, CreatedBy);
                proc.AddNVarcharPara("@Action", 150, Action);
                proc.GetScalar();
                IsUpdate = true;
            }
            catch
            {
                IsUpdate = false;
            }
            return IsUpdate;
        }

        //Added By:Subhabrata :21-0-2017 Saturday
        public DataTable GetEmailTemplateBySenderType(int Create_User)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("Prc_GetEmailTemplateBySenderType");
            proc.AddBigIntegerPara("@SenderType", Create_User);
            dt = proc.GetTable();
            return dt;
        }
        //End

        //kaushik 16_1_2017
        #region GetSupervisorEmployeeDetailByReportingTo
        public DataTable GetSupervisorEmployeeDetailByReportingTo(string cnt_internalId)
        {

            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_EmployeeAllDtl");
            proc.AddNVarcharPara("@action", 150, "GetSupervisorEmployeeDetailByReportingTo");
            proc.AddNVarcharPara("@cnt_internalId", 50, cnt_internalId);
            dt = proc.GetTable();
            return dt;
        }
        #endregion Sales Visit
        //Subhabrata
        public DataTable GetAssignedEmployeeDetailAndSendMail(string cnt_internalId)
        {

            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("ProcGetEmailIDByUID");
            proc.AddNVarcharPara("@Action", 150, "Mailsend");
            proc.AddNVarcharPara("@UID", 50, cnt_internalId);
            dt = proc.GetTable();
            return dt;
        }
        //End
        public DataTable GetAssignedSubordinateEmployeeDetailByReportingTo(string cnt_internalId, string AssignId)
        {

            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_EmployeeAllDtl");
            proc.AddNVarcharPara("@action", 150, "GetAssignedSubordinateEmployeeDetailByReportingTo");
            proc.AddNVarcharPara("@cnt_internalId", 50, cnt_internalId);
            proc.AddNVarcharPara("@AssignId ", 50, AssignId);
            dt = proc.GetTable();
            return dt;
        }

        public DataTable GetEmployeePrefereedProducts(string Uids)
        {

            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("AddNewProduct");
            proc.AddNVarcharPara("@Module", 100, "GetEmployeePrefereedProducts");
            proc.AddNVarcharPara("@Uids", 1000, Uids);
            dt = proc.GetTable();
            return dt;
        }
        public DataTable GetAllProducts()
        {

            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("AddNewProduct");
            proc.AddNVarcharPara("@Module", 100, "GetAllProducts");

            dt = proc.GetTable();
            return dt;
        }


        public string getChildCompany(string companyId, string ListOfCompany)
        {
            DBEngine objDBEngine = new DBEngine();

            DataTable DtSecond = objDBEngine.GetDataTable(" tbl_master_company ", " cmp_id,cmp_internalid ", " cmp_parentid= '" + companyId + "'");
            if (DtSecond.Rows.Count != 0)
            {
                for (int i = 0; i < DtSecond.Rows.Count; i++)
                {
                    ListOfCompany += DtSecond.Rows[i][0].ToString() + ",";
                    actual += DtSecond.Rows[i][0].ToString() + ",";
                    getChildCompany(DtSecond.Rows[i][1].ToString(), ListOfCompany);
                }
            }
            return actual;
        }

        public int UpdateEmployeeBranch(string empInternalID, int Branchid)
        {
            int i;
            int rtrnvalue = 0;
            ProcedureExecute proc = new ProcedureExecute("prc_EmployeeAllDtl");
            proc.AddNVarcharPara("@action", 150, "UpdateEmployeeBranch");
            proc.AddNVarcharPara("@cnt_internalId", 50, empInternalID);
            proc.AddIntegerPara("@branchId", Branchid);
            proc.AddVarcharPara("@ReturnValue", 50, "0", QueryParameterDirection.Output);
            i = proc.RunActionQuery();
            rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
            return rtrnvalue;
        }


        // Code Added By Sam on 23032017 to prevent delete data from tbl_trans_employeectc
        // if number of data in the table =1
        public DataTable NumberOfEmployeeCTC(int empctcid)
        {

            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_EmployeeAllDtl");
            proc.AddNVarcharPara("@action", 100, "NumberOfEmployeeCTC");
            proc.AddIntegerPara("@empctcid", empctcid);
            dt = proc.GetTable();
            return dt;
        }

        /// Coded By Samrat Roy -- 18/04/2017 
        /// To Delete Contact Type selection on Employee Type (DME/ISD)
        public int DeleteContactType(string empInternalID)
        {
            int i;
            int rtrnvalue = 0;
            ProcedureExecute proc = new ProcedureExecute("prc_DeleteContactTypeEmplyeeMapping");
            proc.AddNVarcharPara("@cnt_internalId", 20, empInternalID);
            i = proc.RunActionQuery();
            rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
            return rtrnvalue;
        }




        #region  Email Template Helper

        public DataTable GetEmailConfiguartion()
        {

            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("Proc_Email_Templatehelper");
            proc.AddPara("@Action", "Emailconfig");
            dt = proc.GetTable();
            return dt;
        }



        public DataTable Getemailids(string customerId)
        {

            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("Proc_Email_Templatehelper");
            proc.AddPara("@Action", "GetemailIdofCustomer");
            proc.AddPara("@CustomerID", customerId);

            dt = proc.GetTable();
            return dt;
        }

        public DataTable GetemailidsPO(string customerId)
        {

            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("Proc_Email_Templatehelper");
            proc.AddPara("@Action", "GetemailIdofVendorPO");
            proc.AddPara("@CustomerID", customerId);

            dt = proc.GetTable();
            return dt;
        }
        public DataTable GetemailidsForChallan(string customerId)
        {

            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("Proc_Email_Templatehelper");
            proc.AddPara("@Action", "GetemailIdofCustomerChallan");
            proc.AddPara("@CustomerID", customerId);

            dt = proc.GetTable();
            return dt;
        }

        public DataTable Getemailtemplates(string TypeId)
        {

            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("Proc_Email_Templatehelper");
            proc.AddPara("@Action", "Getemailtemplates");
            proc.AddPara("@TypeID", TypeId);
            dt = proc.GetTable();
            return dt;
        }

        public DataSet GetemailtemplatesPO(string TypeId)
        {

            DataSet dt = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Proc_Email_Templatehelper");
            proc.AddPara("@Action", "GetemailtemplatesPO");
            proc.AddPara("@TypeID", TypeId);
            dt = proc.GetDataSet();
            return dt;
        }
        public DataTable Getemailtagsforpurchase(string Id, string Action)
        {

            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("Proc_Email_Templatehelper");
            proc.AddPara("@Action", Action);
            proc.AddPara("@Id", Id);

            dt = proc.GetTable();
            return dt;
        }


        public static string GetFormattedString<T>(Object myObj, string StringToBeFormatted)
        {
            string FormattedString = "";
            Type temp = typeof(T);
            Type objectType = myObj.GetType();
            T obj = Activator.CreateInstance<T>();

            foreach (PropertyInfo pro in temp.GetProperties())
            {
                object value = myObj.GetType().GetProperty(pro.Name).GetValue(myObj, null);
                string ParamName = "@" + pro.Name + "@";
                string ValueToBeInserted = "";
                if (value != null)
                {
                    ValueToBeInserted = value.ToString();
                }
                StringToBeFormatted = StringToBeFormatted.Replace(ParamName, ValueToBeInserted);
            }

            FormattedString = StringToBeFormatted;

            return FormattedString;
        }


        public DataTable GetApporvalMail(string branchId, string Typecode)
        {

            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("Proc_Mailboxvisibility");
            proc.AddPara("@branchId", branchId);
            proc.AddPara("@TypeCode", Typecode);
            dt = proc.GetTable();
            return dt;
        }

        public DataTable GetSystemsettingmail(string Option)
        {

            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("Proc_SystemsettingforMail");
            proc.AddPara("@Option", Option);

            dt = proc.GetTable();
            return dt;
        }

        public DataTable GetSystemsettingMacAddress(string user, string mac)
        {

            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("Proc_Mac_accessuser");
            proc.AddPara("@UserId", user);
            proc.AddPara("@Mac", mac);
            dt = proc.GetTable();
            return dt;
        }


        #endregion
        /*--------------------Arindam------------------------------------*/


        public DataSet InsertEmployeeDataFromExcel(string EmployeeCode, string userId, string ContactType, string Salutation, string FirstName, string MiddileName, string LastName, string Dob, string Gender, string doj, string Grade, string BloodGroup, string MaritalStatus, string Organization, string JobResposibility, string Branch,
            string Designation, string EmployeeType, string ReportTo, string AddressTypeResidence, string Address1_Res, string Address2_Res, string Address3_Res, string Country_Res, string State_res, string City_District_Res, string Pin_Zip_Res, string Phone_type_res, string Number_Res, string AddressType_off, string Address1_off, string Address2_off, string Address3_off, string Country_off, string State_off, string City_District_Off, string Pin_Zip_Off, string Phone_type_off, string Number_Off, string Email_Type, string Email_Id, string Relationship_1, string Name_1, string RelationShip_2, string Name_2, string Current_Ctc, string Pan, string Aadhar, string Passport, String ValidUpTo, string Epic, string BankName, string Account_No, string AccountType, string Pf_Applicable, string Pf_No, string Uan, string Esi_Applicable,
            string Esi_No)
        {

            DateTime? Dateofbirth = null;
            if (Dob != "")
            {
                Dateofbirth = DateTime.ParseExact(Dob, "dd-MM-yyyy", CultureInfo.InvariantCulture); ;
            }

            DateTime? Dateofjoining = null;
            if (doj != "")
            {
                Dateofjoining = DateTime.ParseExact(doj, "dd-MM-yyyy", CultureInfo.InvariantCulture); ;
            }

            DateTime? ValidUp = null;
            if (ValidUpTo != "")
            {
                ValidUp = DateTime.ParseExact(ValidUpTo, "dd-MM-yyyy", CultureInfo.InvariantCulture); ;
            }

            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_EmployeesImportFromExcel");
            proc.AddVarcharPara("@Action", 100, "InsertEmployeeDataFromExcel");
            proc.AddIntegerPara("@UserId", Convert.ToInt32(userId));
            proc.AddVarcharPara("@ContactType", 10, ContactType);
            proc.AddVarcharPara("@EmployeeCode", 10, EmployeeCode);
            proc.AddVarcharPara("@Salutation", 10, Salutation);
            proc.AddVarcharPara("@FirstName", 200, FirstName);
            proc.AddVarcharPara("@MiddileName", 200, MiddileName);
            proc.AddVarcharPara("@LastName", 200, LastName);
            proc.AddPara("@Dob", Dateofbirth);
            proc.AddVarcharPara("@Gender", 200, Gender);
            proc.AddPara("@doj", Dateofjoining);
            proc.AddVarcharPara("@Grade", 200, Grade);
            proc.AddVarcharPara("@BloodGroup", 200, BloodGroup);
            proc.AddVarcharPara("@MaritalStatus", 200, MaritalStatus);
            proc.AddVarcharPara("@Organization", 200, Organization);
            proc.AddVarcharPara("@JobResposibility", 200, JobResposibility);
            proc.AddVarcharPara("@Branch", 200, Branch);
            proc.AddVarcharPara("@Designation", 200, Designation);
            proc.AddVarcharPara("@EmployeeType", 200, EmployeeType);
            proc.AddVarcharPara("@ReportTo", 200, ReportTo);
            proc.AddVarcharPara("@AddressTypeResidence", 200, AddressTypeResidence);
            proc.AddVarcharPara("@Address1_Res", 200, Address1_Res);

            proc.AddVarcharPara("@Address2_Res", 200, Address2_Res);
            proc.AddVarcharPara("@Address3_Res", 200, Address3_Res);
            proc.AddVarcharPara("@Country_Res", 200, Country_Res);
            proc.AddVarcharPara("@State_res", 200, State_res);
            proc.AddVarcharPara("@City_District_Res", 200, City_District_Res);
            proc.AddVarcharPara("@Pin_Zip_Res", 200, Pin_Zip_Res);
            proc.AddVarcharPara("@Phone_type_res", 200, Phone_type_res);
            proc.AddVarcharPara("@Number_Res", 200, Number_Res);
            proc.AddVarcharPara("@AddressType_off", 200, AddressType_off);

            proc.AddVarcharPara("@Address1_off", 200, Address1_off);

            proc.AddVarcharPara("@Address2_off", 200, Address2_off);
            proc.AddVarcharPara("@Address3_off", 200, Address3_off);
            proc.AddVarcharPara("@Country_off", 200, Country_off);
            proc.AddVarcharPara("@State_off", 200, State_off);



            proc.AddVarcharPara("@City_District_Off", 200, City_District_Off);
            proc.AddVarcharPara("@Pin_Zip_Off", 200, Pin_Zip_Off);
            proc.AddVarcharPara("@Phone_type_off", 200, Phone_type_off);
            proc.AddVarcharPara("@Number_Off", 200, Number_Off);
            proc.AddVarcharPara("@Email_Type", 200, Email_Type);
            proc.AddVarcharPara("@Email_Id", 200, Email_Id);
            proc.AddVarcharPara("@Relationship_1", 200, Relationship_1);
            proc.AddVarcharPara("@Name_1", 200, Name_1);
            proc.AddVarcharPara("@RelationShip_2", 200, RelationShip_2);
            proc.AddVarcharPara("@Name_2", 200, Name_2);
            proc.AddVarcharPara("@Current_Ctc", 200, Current_Ctc);
            proc.AddVarcharPara("@Pan", 200, Pan);
            proc.AddVarcharPara("@Aadhar", 200, Aadhar);
            proc.AddVarcharPara("@Passport", 200, Passport);
            proc.AddPara("@ValidUpTo", ValidUp);
            proc.AddVarcharPara("@Epic", 200, Epic);
            proc.AddVarcharPara("@BankName", 200, BankName);
            proc.AddVarcharPara("@Account_No", 200, Account_No);
            proc.AddVarcharPara("@AccountType", 200, AccountType);
            proc.AddVarcharPara("@Pf_Applicable", 200, Pf_Applicable);
            proc.AddVarcharPara("@Pf_No", 200, Pf_No);
            proc.AddVarcharPara("@Uan", 200, Uan);
            proc.AddVarcharPara("@Esi_Applicable", 200, Esi_Applicable);
            proc.AddVarcharPara("@Esi_No", 200, Esi_No);
            ds = proc.GetDataSet();
            return ds;
        }


        public int InsertEmployeeImportLOg(string empcode, int loopnumber, string empname, string userid, string filename, string description, string status)
        {

            int i;
            //int rtrnvalue = 0;
            ProcedureExecute proc = new ProcedureExecute("prc_insertEmployeeLog");
            proc.AddVarcharPara("@action", 150, "insertlog");
            proc.AddVarcharPara("@EmpCode", 50, empcode);
            proc.AddIntegerPara("@LoopNumber", loopnumber);
            proc.AddVarcharPara("@EmpName", 150, empname);
            proc.AddVarcharPara("@UserId", 150, userid);
            proc.AddVarcharPara("@FileName", 150, filename);
            proc.AddVarcharPara("@decription", 150, description);
            proc.AddVarcharPara("@status", 150, status);
            i = proc.RunActionQuery();

            return i;
        }


        public DataSet GetEmployeeLog(string Filename)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_insertEmployeeLog");
            proc.AddVarcharPara("@action", 150, "getEmployeeLog");
            proc.AddVarcharPara("@FileName", 150, Filename);
            ds = proc.GetDataSet();
            return ds;
        }

        /*--------------------Arindam------------------------------------*/


        public DataTable SubmitEmployeeBranch(String EMPID, String BranchId, String User_id)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_EmployeeBranchMapInsertUpdate");
            proc.AddPara("@EMPID", EMPID);
            proc.AddPara("@BranchId", BranchId);
            proc.AddPara("@User_id", User_id);
            dt = proc.GetTable();
            return dt;
        }

    }
}
