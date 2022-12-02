using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DataAccessLayer;

namespace BusinessLogicLayer
{
    public class Utilities
    {
        public string InsertTransEmailforreminder(string Emails_SenderEmailID, string Emails_Subject, string Emails_Content, string Emails_HasAttachement,
                                  string Emails_CreateApplication, string Emails_CreateUser, string Emails_Type,
                                  string Emails_CompanyID, string Emails_Segment, DateTime Emails_createdate)
        {
            ProcedureExecute proc;
            string rtrnvalue = "";
            try
            {


                using (proc = new ProcedureExecute("InsertTransEmailforreminder"))
                {
                    proc.AddVarcharPara("@Emails_SenderEmailID", 150, Emails_SenderEmailID);
                    proc.AddVarcharPara("@Emails_Subject", 150, Emails_Subject);
                    proc.AddVarcharPara("@Emails_Content",-1, Emails_Content);
                    proc.AddCharPara("@Emails_HasAttachement", 1,Convert.ToChar(Emails_HasAttachement));
                    proc.AddIntegerPara("@Emails_CreateApplication", Convert.ToInt32(Emails_CreateApplication));
                    proc.AddIntegerPara("@Emails_CreateUser",Convert.ToInt32(Emails_CreateUser));
                    proc.AddCharPara("@Emails_Type", 1, Convert.ToChar(Emails_Type));
                    proc.AddCharPara("@Emails_CompanyID", 10, Convert.ToChar(Emails_CompanyID));
                    proc.AddCharPara("@Emails_Segment", 10, Convert.ToChar(Emails_Segment));
                    proc.AddDateTimePara("@Emails_createdate", Emails_createdate);
                    proc.AddBigIntegerPara("@result", 20, QueryParameterDirection.Output);

                    int i = proc.RunActionQuery();
                    rtrnvalue = proc.GetParaValue("@result").ToString();
                    return rtrnvalue;


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

        public string InsertTransEmail(string Emails_SenderEmailID, string Emails_Subject, string Emails_Content, string Emails_HasAttachement,
                                 string Emails_CreateApplication, string Emails_CreateUser, string Emails_Type,
                                 string Emails_CompanyID, string Emails_Segment)
        {
            ProcedureExecute proc;
            string rtrnvalue = "";
            try
            {


                using (proc = new ProcedureExecute("InsertTransEmail"))
                {
                    proc.AddVarcharPara("@Emails_SenderEmailID", 150, Emails_SenderEmailID);
                    proc.AddVarcharPara("@Emails_Subject", 150, Emails_Subject);
                    proc.AddVarcharPara("@Emails_Content", -1, Emails_Content);
                    proc.AddCharPara("@Emails_HasAttachement", 1, Convert.ToChar(Emails_HasAttachement));
                    proc.AddIntegerPara("@Emails_CreateApplication", Convert.ToInt32(Emails_CreateApplication));
                    proc.AddIntegerPara("@Emails_CreateUser",Convert.ToInt32(Emails_CreateUser));
                    proc.AddCharPara("@Emails_Type", 1, Convert.ToChar(Emails_Type));
                    proc.AddCharPara("@Emails_CompanyID", 10, Convert.ToChar(Emails_CompanyID));
                    proc.AddCharPara("@Emails_Segment", 10, Convert.ToChar(Emails_Segment));
                    proc.AddBigIntegerPara("@result", 20, QueryParameterDirection.Output);

                    int i = proc.RunActionQuery();
                    rtrnvalue = proc.GetParaValue("@result").ToString();
                    return rtrnvalue;


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


        public DataSet Addressbook(string SearchOnly, string Param, string Param1, string Addtype,
                                string GRPTYPE, string Groupby, string CLIENTS,
                                string Clienttype)
        {
            ProcedureExecute proc;
            DataSet ds = new DataSet();
           
            try
            {


                using (proc = new ProcedureExecute("Addressbook"))
                {
                    proc.AddVarcharPara("@SearchOnly", 100, SearchOnly);
                    proc.AddVarcharPara("@Param", 100, Param);
                    proc.AddVarcharPara("@Param1", 100, Param1);
                    proc.AddVarcharPara("@Addtype", 100, Addtype);
                    proc.AddVarcharPara("@GRPTYPE", 100, GRPTYPE);
                    proc.AddVarcharPara("@Groupby", 100, Groupby);
                    proc.AddVarcharPara("@CLIENTS", 100, CLIENTS);
                    proc.AddVarcharPara("@Clienttype", 100, Clienttype);

                    ds = proc.GetDataSet();
                    return ds;

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


        public int LeaveApplicationUpdate(string la_cntId, string la_appType, string la_Consideration, string la_ReceivedPhysical,
                                DateTime la_appDate, DateTime la_startDateAppl, DateTime la_endDateAppl,
                                string la_appStatus, string la_apprRejBy, DateTime la_apprRejOn, DateTime la_startDateApr, DateTime la_endDateApr,
                                int userId, int la_id, DateTime la_joinDateTime, string la_Remarks)
        {
            ProcedureExecute proc;
            try
            {


                using (proc = new ProcedureExecute("LeaveApplicationUpdate"))
                {
                    proc.AddVarcharPara("@la_cntId", 10, la_cntId);
                    proc.AddVarcharPara("@la_appType", 2, la_appType);
                    proc.AddCharPara("@la_Consideration", 1,Convert.ToChar(la_Consideration));
                    proc.AddCharPara("@la_ReceivedPhysical", 1,Convert.ToChar(la_ReceivedPhysical));
                    proc.AddDateTimePara("@la_appDate", la_appDate);
                    proc.AddDateTimePara("@la_startDateAppl",  la_startDateAppl);
                    proc.AddDateTimePara("@la_endDateAppl",  la_endDateAppl);
                    proc.AddVarcharPara("@la_appStatus", 2, la_appStatus);
                    proc.AddNVarcharPara("@la_apprRejBy", 10, la_apprRejBy);
                    proc.AddDateTimePara("@la_apprRejOn",  la_apprRejOn);
                    proc.AddDateTimePara("@la_startDateApr",  la_startDateApr);
                    proc.AddDateTimePara("@la_endDateApr", la_endDateApr);
                    proc.AddIntegerPara("@userId", userId);
                    proc.AddIntegerPara("@la_id",  la_id);
                    proc.AddDateTimePara("@la_joinDateTime", la_joinDateTime);
                    proc.AddVarcharPara("@la_Remarks", 5000, la_Remarks);
                   
                    int i = proc.RunActionQuery();
                   return i;


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


        public DataSet Fetch_CustomerForEmailSMS(string AccessCode, string Branch, string Group, string Client,
                               string SendType, string GroupType, string Segment)
        {
            ProcedureExecute proc;
            DataSet ds = new DataSet();

            try
            {


                using (proc = new ProcedureExecute("Fetch_CustomerForEmailSMS"))
                {
                    proc.AddVarcharPara("@AccessCode", 1024, AccessCode);
                    proc.AddVarcharPara("@Branch", -1, Branch);
                    proc.AddVarcharPara("@Group", -1, Group);
                    proc.AddVarcharPara("@Client", -1, Client);
                    proc.AddCharPara("@SendType", 1,Convert.ToChar(SendType));
                    proc.AddCharPara("@GroupType",1,Convert.ToChar(GroupType));
                    proc.AddVarcharPara("@Segment", 50, Segment);

                    ds = proc.GetDataSet();
                    return ds;

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

        public DataSet FETCH_NOTIFICATION_REQUESTIDENTITY(string PHONE_EMAIL, string SUBJECT)
        {
            ProcedureExecute proc;
            DataSet ds = new DataSet();

            try
            {


                using (proc = new ProcedureExecute("FETCH_NOTIFICATION_REQUESTIDENTITY"))
                {
                    proc.AddVarcharPara("@PHONE_EMAIL", 50, PHONE_EMAIL);
                    proc.AddVarcharPara("@SUBJECT", 1024, SUBJECT);
                   
                    ds = proc.GetDataSet();
                    return ds;

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

        public string CandidateEmployeeInsert(string CompCode, string rde_Salutation, string rde_Name, string rde_Branch,
                                 string rde_CandidateSex, string rde_MaritalStatus, string rde_DOB,
                                 string rde_EduQualification, string rde_SourceType, string rde_SourceName, string cnt_contactType,
                                 string userid, string rde_ProbableJoinDate, string rde_Company,
                                 string rde_Designation, string rde_ApprovedCTC, string rde_EmpType, string rde_id,
                                 string rde_NoofDepedent, string rde_PhoneNo, string rde_ResidenceLocation, string rde_Email)
        {
            ProcedureExecute proc;
            string rtrnvalue = "";
            try
            {
                using (proc = new ProcedureExecute("sp_CandidateEmployeeInsert"))
                {
                    proc.AddVarcharPara("@cnt_ucc", 50, CompCode);
                    proc.AddIntegerPara("@rde_Salutation",Convert.ToInt32(rde_Salutation));
                    proc.AddVarcharPara("@cnt_firstName", 50, rde_Name);
                    proc.AddIntegerPara("@cnt_branchId",Convert.ToInt32(rde_Branch));
                    proc.AddIntegerPara("@cnt_sex", Convert.ToInt32(rde_CandidateSex));
                    proc.AddIntegerPara("@cnt_maritalStatus",Convert.ToInt32(rde_MaritalStatus));
                    proc.AddDateTimePara("@cnt_DOB",Convert.ToDateTime(rde_DOB));
                    proc.AddIntegerPara("@cnt_education", Convert.ToInt32(rde_EduQualification));
                    proc.AddIntegerPara("@cnt_contactSource", Convert.ToInt32(rde_SourceType));
                    proc.AddVarcharPara("@cnt_referedBy", 50, rde_SourceName);
                    proc.AddVarcharPara("@cnt_contactType", 50, cnt_contactType);
                    proc.AddVarcharPara("@lastModifyUser", 20, userid);
                    proc.AddDateTimePara("@DateOfJoining", Convert.ToDateTime(rde_ProbableJoinDate));
                    proc.AddVarcharPara("@Organization", 50, rde_Company);
                    proc.AddVarcharPara("@Designation", 50, rde_Designation);
                    proc.AddVarcharPara("@AprovedCTC", 50, rde_ApprovedCTC);
                    proc.AddVarcharPara("@emp_typee", 50, rde_EmpType);
                    proc.AddVarcharPara("@rde_id", 50, rde_id);
                    proc.AddVarcharPara("@rde_NoofDepedent", 10, rde_NoofDepedent);
                    proc.AddVarcharPara("@MobileNo", 50, rde_PhoneNo);
                    proc.AddVarcharPara("@rde_ResidenceLocation", 100, rde_ResidenceLocation);
                    proc.AddVarcharPara("@rde_Email", 100, rde_Email);
                    int i = proc.RunActionQuery();
                    rtrnvalue = proc.GetParaValue("@result").ToString();
                    return rtrnvalue;
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

        public void InsertEmailSetup(string cmpid, string EmailAccounts_SegmentID, string EmailAccounts_EmailID, string EmailAccounts_UsedFor,
                                  string EmailAccounts_Password, string EmailAccounts_SMTP, string EmailAccounts_SMTPPort,
                                  string EmailAccounts_POP, string EmailAccounts_POPPort, string EmailAccounts_ReplyToAccount, string EmailAccounts_Disclaimer, string EmailAccounts_InUse,
                                  string EmailAccounts_CreateUser, string EmailAccounts_SSLMode, string EmailAccounts_FromName, string EmailAccounts_ReplyToName)
        {
            ProcedureExecute proc;
            string rtrnvalue = "";
            try
            {
                using (proc = new ProcedureExecute("sp_InsertEmailSetup"))
                {
                    proc.AddNVarcharPara("@EmailAccounts_CompanyID", 10,Convert.ToString(cmpid));
                    proc.AddIntegerPara("@EmailAccounts_SegmentID",Convert.ToInt32(EmailAccounts_SegmentID));
                    proc.AddVarcharPara("@EmailAccounts_EmailID", 100, EmailAccounts_EmailID);
                    proc.AddNVarcharPara("@EmailAccounts_UsedFor", 1, Convert.ToString(EmailAccounts_UsedFor));
                    proc.AddVarcharPara("@EmailAccounts_Password", 20, EmailAccounts_Password);
                    proc.AddVarcharPara("@EmailAccounts_SMTP", 100, EmailAccounts_SMTP);
                    proc.AddNVarcharPara("@EmailAccounts_SMTPPort", 10, Convert.ToString(EmailAccounts_SMTPPort));
                    proc.AddVarcharPara("@EmailAccounts_POP", 100, EmailAccounts_POP);
                    proc.AddNVarcharPara("@EmailAccounts_POPPort", 10, Convert.ToString(EmailAccounts_POPPort));
                    proc.AddVarcharPara("@EmailAccounts_ReplyToAccount", 100, EmailAccounts_ReplyToAccount);
                    proc.AddVarcharPara("@EmailAccounts_Disclaimer", 4000, EmailAccounts_Disclaimer);
                    proc.AddNVarcharPara("@EmailAccounts_InUse", 1, Convert.ToString(EmailAccounts_InUse));
                    proc.AddIntegerPara("@EmailAccounts_CreateUser", Convert.ToInt32(EmailAccounts_CreateUser));
                    proc.AddVarcharPara("@EmailAccounts_SSLMode", 10, EmailAccounts_SSLMode);
                    proc.AddVarcharPara("@EmailAccounts_FromName", 100, EmailAccounts_FromName);
                    proc.AddVarcharPara("@EmailAccounts_ReplyToName", 150, EmailAccounts_ReplyToName);
                    proc.RunActionQuery();

                    //int i = proc.RunActionQuery();
                    //rtrnvalue = proc.GetParaValue("@result").ToString();
                    //return rtrnvalue;
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




        public int RecalculateCashBankJournalToAccountsLedger(string MainCompanyId, string MainSegmentId, string FinYear1)
        {
            ProcedureExecute proc;
           
            try
            {


                using (proc = new ProcedureExecute("RecalculateCashBankJournalToAccountsLedger"))
                {
                    proc.AddVarcharPara("@MainCompanyId", 20, MainCompanyId);
                    proc.AddIntegerPara("@MainSegmentId", Convert.ToInt32(MainSegmentId));
                    proc.AddVarcharPara("@FinYear1", 12, FinYear1);
                    
                    int i = proc.RunActionQuery();

                    return i;


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



        public int xmlDataInsert(string Data, string createuser)
        {
            ProcedureExecute proc;

            try
            {


                using (proc = new ProcedureExecute("xmlDataInsert"))
                {
                    proc.AddNTextPara("@Data", Data);
                    proc.AddVarcharPara("@createuser", 10, createuser);

                    int i = proc.RunActionQuery();

                    return i;


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



        public int xmlDataDelelteInsertUpdate(string databasestr, string doc)
        {
            ProcedureExecute proc;

            try
            {


                using (proc = new ProcedureExecute("sp_Data_Del_Ins_Up_xml"))
                {
                    proc.AddNVarcharPara("@targetDB", 500, databasestr);
                    proc.AddNVarcharPara("@doc", -1, doc);

                    int i = proc.RunActionQuery();

                    return i;


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

        public int UpdateInactiveFromActive(string clientPayoutData, string CreateUser)
        {
            ProcedureExecute proc;

            try
            {


                using (proc = new ProcedureExecute("sp_Data_Del_Ins_Up_xml"))
                {
                    proc.AddNVarcharPara("@clientPayoutData", 500, clientPayoutData);
                    proc.AddNVarcharPara("@CreateUser", -1, CreateUser);

                    int i = proc.RunActionQuery();

                    return i;


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

        public int UpdateMessage(string pDBName, string clientPayoutData, string CreateUser)
        {
            ProcedureExecute proc;
            try
            {
                using (proc = new ProcedureExecute("sp_Data_Del_Ins_Up_xml"))
                {
                    proc.AddNVarcharPara("@targetDB", 50, pDBName);
                    proc.AddNVarcharPara("@doc", 500, clientPayoutData);
                    //proc.AddNVarcharPara("@CreateUser", -1, CreateUser);
                    int i = proc.RunActionQuery();
                    return i;
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
    }
}
